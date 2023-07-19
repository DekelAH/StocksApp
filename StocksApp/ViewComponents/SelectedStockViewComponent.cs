using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using ServiceContracts.Contracts;
using StocksApp.OptionsModels;
using StocksApp.ServiceContracts;

namespace StocksApp.ViewComponents
{
    public class SelectedStockViewComponent : ViewComponent
    {
        #region Fields

        private readonly TradingOptions _tradingOptions;
        private readonly IFinnhubService _finnhubService;
        private readonly IStocksService _stocksService;
        private readonly IConfiguration _configuration;

        #endregion

        #region Ctors

        public SelectedStockViewComponent(IOptions<TradingOptions> tradingOptions, IFinnhubService finnhubService,
            IStocksService stocksService, IConfiguration configuration)
        {
            _tradingOptions = tradingOptions.Value;
            _finnhubService = finnhubService;
            _stocksService = stocksService;
            _configuration = configuration;
        }

        #endregion

        #region Methods

        public async Task<IViewComponentResult> InvokeAsync(string? stockSymbol)
        {
            Dictionary<string, object>? companyProfileDictionary = null;

            if (stockSymbol != null)
            {
                companyProfileDictionary = await _finnhubService.GetCompanyProfile(stockSymbol);
                var stockPriceDic = await _finnhubService.GetStockPriceQuote(stockSymbol);

                if (companyProfileDictionary != null && stockPriceDic != null)
                {
                    companyProfileDictionary.Add("price", stockPriceDic["c"]);
                }
            }
            if (companyProfileDictionary != null && companyProfileDictionary.ContainsKey("logo"))
            {
                return View(companyProfileDictionary);
            }
            else
            {
                return View("");
            }
        }

        #endregion
    }
}
