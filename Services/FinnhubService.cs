using Exceptions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RepositoryContracts;
using Serilog;
using SerilogTimings;
using Services;
using StocksApp.ServiceContracts;
using System.Text.Json;

namespace StocksApp.Services
{
    public class FinnhubService : IFinnhubService
    {
        #region Fields

        private readonly IFinnhubRepository _finnhubRepository;
        private readonly ILogger<StocksService> _logger;
        private readonly IDiagnosticContext _diagnosticContext;

        #endregion

        #region Ctors

        public FinnhubService(IFinnhubRepository finnhubRepository, ILogger<StocksService> logger,
            IDiagnosticContext diagnosticContext)
        {
            _finnhubRepository = finnhubRepository;
            _logger = logger;
            _diagnosticContext = diagnosticContext;
        }

        #endregion

        #region Methods

        public async Task<Dictionary<string, object>?> GetCompanyProfile(string stockSymbol)
        {
            try
            {
                _logger.LogInformation("GetCompanyProfile method in Finnhub Service");
                Dictionary<string, object>? responseDictionary = await _finnhubRepository.GetCompanyProfile(stockSymbol);
                _diagnosticContext.Set("CompanyProfile", responseDictionary);
                return responseDictionary;
            }
            catch (Exception ex)
            {
                FinnhubExceptionHandler finnhubExceptionHandler =
                                    new FinnhubExceptionHandler("Unable to connect to Finnhub service", ex);
                throw finnhubExceptionHandler;
            }
        }

        public async Task<Dictionary<string, object>?> GetStockPriceQuote(string stockSymbol)
        {
            try
            {
                _logger.LogInformation("GetStockPriceQuote method in Finnhub Service");
                Dictionary<string, object>? responseDictionary = await _finnhubRepository.GetStockPriceQuote(stockSymbol);
                _diagnosticContext.Set("StockPriceQuote", responseDictionary);
                return responseDictionary;
            }
            catch (Exception ex)
            {
                FinnhubExceptionHandler finnhubExceptionHandler =
                    new FinnhubExceptionHandler("Unable to connect to Finnhub service", ex);
                throw finnhubExceptionHandler;
            }
            
        }

        public async Task<List<Dictionary<string, object>>?> GetStocks()
        {
            try
            {
                List<Dictionary<string, object>>? responseDictionary;
                using (Operation.Time("Time for Getting Stocks from Finnhub"))
                {
                    _logger.LogInformation("GetStocks method in Finnhub Service");
                    responseDictionary = await _finnhubRepository.GetStocks();
                    _diagnosticContext.Set("Stocks", responseDictionary);
                }
                return responseDictionary;
            }
            catch (Exception ex)
            {
                FinnhubExceptionHandler finnhubExceptionHandler =
                    new FinnhubExceptionHandler("Unable to connect to Finnhub service", ex);
                throw finnhubExceptionHandler;
            }
        }

        public async Task<Dictionary<string, object>?> SearchStocks(string stockSymbolToSearch)
        {
            try
            {
                Dictionary<string, object>? responseDictionary = await _finnhubRepository.SearchStocks(stockSymbolToSearch);
                return responseDictionary;
            }
            catch (Exception ex)
            {
                FinnhubExceptionHandler finnhubExceptionHandler = 
                    new FinnhubExceptionHandler("Unable to connect to Finnhub service", ex);
                throw finnhubExceptionHandler;
            }
            
        }

        #endregion
    }
}

