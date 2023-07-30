using Exceptions;
using Microsoft.Extensions.Logging;
using RepositoryContracts;
using Serilog;
using SerilogTimings;
using StocksApp.ServiceContracts;

namespace StocksApp.Services
{
    public class FinnhubGetterService : IFinnhubGetterService
    {
        #region Fields

        private readonly IFinnhubRepository _finnhubRepository;
        private readonly ILogger<FinnhubGetterService> _logger;
        private readonly IDiagnosticContext _diagnosticContext;

        #endregion

        #region Ctors

        public FinnhubGetterService(IFinnhubRepository finnhubRepository, ILogger<FinnhubGetterService> logger,
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

        #endregion
    }
}

