using Entities.Models;
using Microsoft.Extensions.Logging;
using RepositoryContracts;
using Serilog;
using ServiceContracts.Contracts;
using ServiceContracts.DTO;

namespace Services
{
    public class StocksGetterService : IStocksGetterService
    {
        #region Fields

        private readonly IStocksRepository _stocksRepository;
        private readonly ILogger<StocksGetterService> _logger;
        private readonly IDiagnosticContext _diagnosticContext;

        #endregion

        #region Ctors

        public StocksGetterService(IStocksRepository stocksRepository, ILogger<StocksGetterService> logger, IDiagnosticContext diagnosticContext)
        {
            _stocksRepository = stocksRepository;
            _logger = logger;
            _diagnosticContext = diagnosticContext;
        }

        #endregion

        #region Methods

        public async Task<List<BuyOrderResponse>> GetAllBuyOrders()
        {
            List<BuyOrder> buyOrders = await _stocksRepository.GetBuyOrders();
            return buyOrders.Select(buyOrder => buyOrder.ToBuyOrderResponse()).ToList();
        }

        public async Task<List<SellOrderResponse>> GetAllSellOrders()
        {
            List<SellOrder> sellOrders = await _stocksRepository.GetSellOrders();
            return sellOrders.Select(sellOrder => sellOrder.ToSellOrderResponse()).ToList();
        }

        #endregion
    }
}
