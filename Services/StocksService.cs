using Entities.DbContextModels;
using Entities.Models;
using Microsoft.Extensions.Logging;
using RepositoryContracts;
using Serilog;
using ServiceContracts.Contracts;
using ServiceContracts.DTO;
using Services.Helpers;
using System.Collections.Generic;

namespace Services
{
    public class StocksService : IStocksService
    {
        #region Fields

        private readonly IStocksRepository _stocksRepository;
        private readonly ILogger<StocksService> _logger;
        private readonly IDiagnosticContext _diagnosticContext;

        #endregion

        #region Ctors

        public StocksService(IStocksRepository stocksRepository, ILogger<StocksService> logger, IDiagnosticContext diagnosticContext)
        {
            _stocksRepository = stocksRepository;
            _logger = logger;
            _diagnosticContext = diagnosticContext;
        }

        #endregion

        #region Methods

        public async Task<BuyOrderResponse> CreateBuyOrder(BuyOrderRequest? buyOrderRequest)
        {
            if (buyOrderRequest == null)
            {
                throw new ArgumentNullException(nameof(buyOrderRequest));
            }
            ValidationHelper.ModelValidation(buyOrderRequest);

            BuyOrder buyOrder = buyOrderRequest.ToBuyOrder();
            buyOrder.BuyOrderID = Guid.NewGuid();

            BuyOrder buyOrderFromRepository = await _stocksRepository.CreateBuyOrder(buyOrder);

            return buyOrder.ToBuyOrderResponse();
        }

        public async Task<SellOrderResponse> CreateSellOrder(SellOrderRequest? sellOrderRequest)
        {
            if (sellOrderRequest == null)
            {
                throw new ArgumentNullException(nameof(sellOrderRequest));
            }
            ValidationHelper.ModelValidation(sellOrderRequest);

            SellOrder sellOrder = sellOrderRequest.ToSellOrder();
            sellOrder.SellOrderID = Guid.NewGuid();

            SellOrder sellOrderFromRepository = await _stocksRepository.CreateSellOrder(sellOrder);

            return sellOrder.ToSellOrderResponse();
        }

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
