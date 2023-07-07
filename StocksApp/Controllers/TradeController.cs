using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using ServiceContracts.Contracts;
using ServiceContracts.DTO;
using ServiceContracts.View_Models;
using StocksApp.OptionsModels;
using StocksApp.ServiceContracts;

namespace StocksApp.Controllers
{
    [Route("trade")]
    public class TradeController : Controller
    {
        #region Fields

        private readonly IFinnhubService _finnHubService;
        private readonly IStocksService _stocksService;
        private readonly IOptions<TradingOptions> _tradingOptions;
        private readonly string? _token;

        #endregion

        #region Ctor

        public TradeController(IFinnhubService finnHubService, IStocksService stocksService, 
            IOptions<TradingOptions> tradingOptions, IConfiguration configuration)
        {
            _finnHubService = finnHubService;
            _stocksService = stocksService;
            _tradingOptions = tradingOptions;
            _token = configuration["FinnhubToken"];
        }

        #endregion

        #region Action Methods

        [Route("[action]")]
        [Route("/")]
        [HttpGet]
        public async Task<IActionResult> Index(string? stockSymbolSearch)
        {
            ViewBag.Token = _token;
            if (_tradingOptions.Value.DefaultStockSymbol == null)
            {
                _tradingOptions.Value.DefaultStockSymbol = "MSFT";
            }
            if (_tradingOptions.Value.DefaultOrderQuantity == null)
            {
                _tradingOptions.Value.DefaultOrderQuantity = "100";
            }

            ViewBag.CurrentStockSymbolSearch = stockSymbolSearch;
            Dictionary<string, object>? stockDetailsDic = await _finnHubService.GetStockPriceQuote(_tradingOptions.Value.DefaultStockSymbol);
            Dictionary<string, object>? companyProfileDic = await _finnHubService.GetCompanyProfile(_tradingOptions.Value.DefaultStockSymbol);

            if (stockDetailsDic != null && companyProfileDic != null)
            {
                StockTrade stock = new StockTrade()
                {
                    StockSymbol = _tradingOptions.Value.DefaultStockSymbol,
                    StockName = companyProfileDic["name"].ToString(),
                    Price = Convert.ToDouble(stockDetailsDic["c"].ToString()),
                    Quantity = uint.Parse(_tradingOptions.Value.DefaultOrderQuantity)
                };

                return View(stock);
            }

            return View();
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<IActionResult> BuyOrder(BuyOrderRequest buyOrderRequest)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();

                StockTrade stock = new StockTrade()
                {
                    StockSymbol = buyOrderRequest.StockSymbol,
                    StockName = buyOrderRequest.StockName,
                    Price = buyOrderRequest.Price,
                    Quantity = buyOrderRequest.Quantity
                };
                return View("Index", stock);
            }

            buyOrderRequest.DateAndTimeOfOrder = DateTime.Now;
            BuyOrderResponse buyOrderResponse = await _stocksService.CreateBuyOrder(buyOrderRequest);

            return RedirectToAction("Orders");
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<IActionResult> SellOrder(SellOrderRequest sellOrderRequest)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();

                StockTrade stock = new StockTrade()
                {
                    StockSymbol = sellOrderRequest.StockSymbol,
                    StockName = sellOrderRequest.StockName,
                    Price = sellOrderRequest.Price,
                    Quantity = sellOrderRequest.Quantity
                };
                return View("Index", stock);
            }

            sellOrderRequest.DateAndTimeOfOrder = DateTime.Now;
            SellOrderResponse sellOrderResponse = await _stocksService.CreateSellOrder(sellOrderRequest);

            return RedirectToAction("Orders");
        }

        [Route("[action]")]
        [HttpGet]
        public async Task<IActionResult> Orders()
        {
            Orders orders = new()
            {
                BuyOrders = await _stocksService.GetAllBuyOrders(),
                SellOrders = await _stocksService.GetAllSellOrders(),
            };

            return View(orders);
        }

        #endregion
    }
}
