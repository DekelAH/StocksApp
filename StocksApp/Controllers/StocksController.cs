using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using ServiceContracts.Contracts;
using ServiceContracts.View_Models;
using StocksApp.OptionsModels;
using StocksApp.ServiceContracts;

namespace StocksApp.Controllers
{
    [Route("stocks")]
    public class StocksController : Controller
    {
        #region Fields

        private readonly TradingOptions _options;
        private readonly IFinnhubService _finnhubService;
        private readonly IStocksService _stocksService;

        #endregion

        #region Ctors

        public StocksController(IOptions<TradingOptions> options, IFinnhubService finnhubService, IStocksService stocksService)
        {
            _options = options.Value;
            _finnhubService = finnhubService;
            _stocksService = stocksService;
        }

        #endregion

        #region Methods

        [Route("[action]/{stockSymbol?}")]
        [HttpGet]
        public async Task<IActionResult> Explore(string? stockSymbol)       
        
        {          
            List<Dictionary<string, object>>? allStocks = await _finnhubService.GetStocks();
            List<Stock> filteredStocks = new List<Stock>();
            if (allStocks != null)
            {
                if (_options.Top25PopularStocks != null)
                {
                    string[] top25PopularStocksArr = _options.Top25PopularStocks.Split(",");
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
