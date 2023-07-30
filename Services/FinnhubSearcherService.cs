using Exceptions;
using Microsoft.Extensions.Logging;
using RepositoryContracts;
using Serilog;
using StocksApp.ServiceContracts;

namespace StocksApp.Services
{
    public class FinnhubSearcherService : IFinnhubSearcherService
    {
        #region Fields

        private readonly IFinnhubRepository _finnhubRepository;
        private readonly ILogger<FinnhubSearcherService> _logger;
        private readonly IDiagnosticContext _diagnosticContext;

        #endregion

        #region Ctors

        public FinnhubSearcherService(IFinnhubRepository finnhubRepository, ILogger<FinnhubSearcherService> logger,
            IDiagnosticContext diagnosticContext)
        {
            _finnhubRepository = finnhubRepository;
            _logger = logger;
            _diagnosticContext = diagnosticContext;
        }

        #endregion

        #region Methods

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

