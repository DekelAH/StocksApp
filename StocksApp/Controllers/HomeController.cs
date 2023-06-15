using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using StocksApp.Models;
using StocksApp.OptionsModels;
using StocksApp.ServiceContracts;

namespace StocksApp.Controllers
{
    public class HomeController : Controller
    {
        #region Fields

        private readonly IFinnhubService _finnHubService;
        private readonly IOptions<TradingOptions> _tradingOptions;

        #endregion

        #region Ctor

        public HomeController(IFinnhubService finnHubService, IOptions<TradingOptions> tradingOptions)
        {
            _finnHubService = finnHubService;
            _tradingOptions = tradingOptions;
        }

        #endregion

        #region Action Methods

        [Route("/")]
        public async Task<IActionResult> Index()
        {
            if (_tradingOptions.Value.DefaultStockSymbol == null)
            {
                _tradingOptions.Value.DefaultStockSymbol = "MSFT";
            }

            Dictionary<string, object>? responseDic = await _finnHubService.GetStockPriceQuote(_tradingOptions.Value.DefaultStockSymbol);
            Stock stock = new Stock()
            {
                StockSymbol = _tradingOptions.Value.DefaultStockSymbol,
                CurrentPrice = Convert.ToDouble(responseDic["c"].ToString()),
                Change = Convert.ToDouble(responseDic["d"].ToString()),
                PercentChange = Convert.ToDouble(responseDic["dp"].ToString()),
                HighPrice = Convert.ToDouble(responseDic["h"].ToString()),
                LowPrice = Convert.ToDouble(responseDic["l"].ToString()),
                OpenPrice = Convert.ToDouble(responseDic["o"].ToString()),
                PreviousClosePrice = Convert.ToDouble(responseDic["pc"].ToString())
            };

            return View(stock);
        }

        #endregion
    }
}
