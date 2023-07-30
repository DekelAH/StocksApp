using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Rotativa.AspNetCore;
using ServiceContracts.Contracts;
using ServiceContracts.DTO;
using ServiceContracts.View_Models;
using StocksApp.Filters.ActionFilters;
using StocksApp.OptionsModels;
using StocksApp.ServiceContracts;

namespace StocksApp.Controllers
{
    [Route("trade")]
    public class TradeController : Controller
    {
        #region Fields

        private readonly IFinnhubSearcherService _finnHubSearcherService;
        private readonly IFinnhubGetterService _finnHubGetterService;

        private readonly IStocksGetterService _stocksGetterService;
        private readonly IStocksCreaterService _stocksCreaterService;

        private readonly IOptions<TradingOptions> _tradingOptions;
        private readonly ILogger<StocksController> _logger;

        private readonly string? _token;

        #endregion

        #region Ctor

        public TradeController(
            IFinnhubGetterService finnhubGetterService, 
            IFinnhubSearcherService finnhubSearcherService, 
            IStocksGetterService stocksGetterService, 
            IStocksCreaterService stocksCreaterService, 
            IOptions<TradingOptions> tradingOptions, 
            IConfiguration configuration, 
            ILogger<StocksController> logger)
        {
            _finnHubGetterService = finnhubGetterService;
            _finnHubSearcherService = finnhubSearcherService;
            _stocksGetterService = stocksGetterService;
            _stocksCreaterService = stocksCreaterService;
            _tradingOptions = tradingOptions;
            _token = configuration["FinnhubToken"];
            _logger = logger;
        }

        #endregion

        #region Action Methods

        [Route("[action]/{stockSymbol}")]
        [Route("~/[action]/{stockSymbol}")]
        [HttpGet]
        public async Task<IActionResult> Index(string stockSymbol)
        {
            _logger.LogInformation("Index action method in Trade Controller");
            _logger.LogDebug($"stockSymbol: {stockSymbol}");
            if (string.IsNullOrEmpty(stockSymbol))
            {
                stockSymbol = "MSFT";
            }
            if (_tradingOptions.Value.DefaultOrderQuantity == null)
            {
                _tradingOptions.Value.DefaultOrderQuantity = "100";
            }

            ViewBag.Token = _token;
            ViewBag.CurrentStockSymbolSearch = stockSymbol;
            Dictionary<string, object>? stockDetailsDic = await _finnHubGetterService.GetStockPriceQuote(stockSymbol);
            Dictionary<string, object>? companyProfileDic = await _finnHubGetterService.GetCompanyProfile(stockSymbol);

            if (stockDetailsDic != null && companyProfileDic != null)
            {
                StockTrade stock = new StockTrade()
                {
                    StockSymbol = companyProfileDic["ticker"].ToString(),
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
        [TypeFilter(typeof(CreateOrderActionFilter))]
        public async Task<IActionResult> BuyOrder(BuyOrderRequest orderRequest)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();

                StockTrade stock = new StockTrade()
                {
                    StockSymbol = orderRequest.StockSymbol,
                    StockName = orderRequest.StockName,
                    Price = orderRequest.Price,
                    Quantity = orderRequest.Quantity
                };
                return View("Index", stock);
            }

            orderRequest.DateAndTimeOfOrder = DateTime.Now;
            BuyOrderResponse buyOrderResponse = await _stocksCreaterService.CreateBuyOrder(orderRequest);

            return RedirectToAction("Orders");
        }

        [Route("[action]")]
        [HttpPost]
        [TypeFilter(typeof(CreateOrderActionFilter))]
        public async Task<IActionResult> SellOrder(SellOrderRequest orderRequest)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();

                StockTrade stock = new StockTrade()
                {
                    StockSymbol = orderRequest.StockSymbol,
                    StockName = orderRequest.StockName,
                    Price = orderRequest.Price,
                    Quantity = orderRequest.Quantity
                };
                return View("Index", stock);
            }

            orderRequest.DateAndTimeOfOrder = DateTime.Now;
            SellOrderResponse sellOrderResponse = await _stocksCreaterService.CreateSellOrder(orderRequest);

            return RedirectToAction("Orders");
        }

        [Route("[action]")]
        [HttpGet]
        public async Task<IActionResult> Orders()
        {
            Orders orders = new()
            {
                BuyOrders = await _stocksGetterService.GetAllBuyOrders(),
                SellOrders = await _stocksGetterService.GetAllSellOrders(),
            };

            return View(orders);
        }

        [Route("[action]")]
        public async Task<IActionResult> OrdersPDF()
        {
            List<IOrderResponse> orders = new();
            orders.AddRange(await _stocksGetterService.GetAllBuyOrders());
            orders.AddRange(await _stocksGetterService.GetAllSellOrders());
            orders = orders.OrderByDescending(order => order.DateAndTimeOfOrder).ToList();

            return new ViewAsPdf("OrdersPDF", orders, ViewData)
            {
                PageMargins = new Rotativa.AspNetCore.Options.Margins() {Top = 20, Bottom = 20, Left = 20, Right = 20 },
                PageOrientation = Rotativa.AspNetCore.Options.Orientation.Landscape
            };
        }

        #endregion
    }
}
