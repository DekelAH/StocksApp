using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using ServiceContracts.Contracts;
using ServiceContracts.View_Models;
using StocksApp.OptionsModels;
using StocksApp.ServiceContracts;

namespace StocksApp.Controllers
{
    [Route("[controller]")]
    public class StocksController : Controller
    {
        #region Fields

        private readonly IOptions<TradingOptions> _options;
        private readonly IFinnhubService _finnhubService;
        private readonly IStocksService _stocksService;

        #endregion

        #region Ctors

        public StocksController(IOptions<TradingOptions> options, IFinnhubService finnhubService, IStocksService stocksService)
        {
            _finnhubService = finnhubService;
            _stocksService = stocksService;
            _options = options;
        }

        #endregion

        #region Methods

        [Route("[action]/{stockSymbol?}")]
        [Route("/")]
        [HttpGet]
        public async Task<IActionResult> Explore(string? stockSymbol, bool showAll = false)            
        {          
            List<Dictionary<string, object>>? allStocks = await _finnhubService.GetStocks();
            List<Stock> filteredStocks = new List<Stock>();
            if (allStocks != null)
            {
                if (_options.Value.Top25PopularStocks != null && !showAll)
                {
                    string[] top25PopularStocksArr = _options.Value.Top25PopularStocks.Split(",");
                    if (top25PopularStocksArr != null)
                    {
                        allStocks = allStocks.Where(stock => top25PopularStocksArr.Contains(stock["symbol"].ToString())).ToList();
                    }
                }

                filteredStocks = allStocks.Select(stock => new Stock() 
                { 
                    StockName = stock["description"].ToString(), 
                    StockSymbol = stock["symbol"].ToString() 
                }).ToList();
            }

            ViewBag.stockSymbol = stockSymbol;
            return View(filteredStocks);
        }

        #endregion
    }
}
