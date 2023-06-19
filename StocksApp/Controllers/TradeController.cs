using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using StocksApp.Models;
using StocksApp.OptionsModels;
using StocksApp.ServiceContracts;

namespace StocksApp.Controllers
{
    public class TradeController : Controller
    {
        #region Fields

        private readonly IFinnhubService _finnHubService;
        private readonly IOptions<TradingOptions> _tradingOptions;
        private readonly string _token;

        #endregion

        #region Ctor

        public TradeController(IFinnhubService finnHubService, IOptions<TradingOptions> tradingOptions, IConfiguration configuration)
        {
            _finnHubService = finnHubService;
            _tradingOptions = tradingOptions;
            _token = configuration["FinnhubToken"];
        }

        #endregion

        #region Action Methods

        [HttpGet]
        [Route("/")]
        public async Task<IActionResult> Index()
        {
            ViewBag.Token = _token;
            if (_tradingOptions.Value.DefaultStockSymbol == null)
            {
                _tradingOptions.Value.DefaultStockSymbol = "MSFT";
            }

            Dictionary<string, object>? stockDetailsDic = await _finnHubService.GetStockPriceQuote(_tradingOptions.Value.DefaultStockSymbol);
            Dictionary<string, object>? companyProfileDic = await _finnHubService.GetCompanyProfile(_tradingOptions.Value.DefaultStockSymbol);

            if (stockDetailsDic != null && companyProfileDic != null)
            {
                StockTrade stock = new StockTrade()
                {
                    StockSymbol = _tradingOptions.Value.DefaultStockSymbol,
                    CurrentPrice = Convert.ToDouble(stockDetailsDic["c"].ToString()),
                    Change = Convert.ToDouble(stockDetailsDic["d"].ToString()),
                    PercentChange = Convert.ToDouble(stockDetailsDic["dp"].ToString()),
                    HighPrice = Convert.ToDouble(stockDetailsDic["h"].ToString()),
                    LowPrice = Convert.ToDouble(stockDetailsDic["l"].ToString()),
                    OpenPrice = Convert.ToDouble(stockDetailsDic["o"].ToString()),
                    PreviousClosePrice = Convert.ToDouble(stockDetailsDic["pc"].ToString()),
                    CompanyProfile = new CompanyProfile()
                    {
                        Name = companyProfileDic["name"].ToString(),
                    }
                };

                return View(stock);
            }

            return View();
        }

        #endregion
    }
}
