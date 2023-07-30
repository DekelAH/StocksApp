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
        private readonly IFinnhubGetterService _finnhubGetterService;
        private readonly IStocksGetterService _stocksService;
        private readonly IConfiguration _configuration;

        #endregion

        #region Ctors

        public SelectedStockViewComponent(IOptions<TradingOptions> tradingOptions, IFinnhubGetterService finnhubGetterService,
            IStocksGetterService stocksService, IConfiguration configuration)
        {
            _tradingOptions = tradingOptions.Value;
            _finnhubGetterService = finnhubGetterService;
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
                companyProfileDictionary = await _finnhubGetterService.GetCompanyProfile(stockSymbol);
                var stockPriceDic = await _finnhubGetterService.GetStockPriceQuote(stockSymbol);

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
