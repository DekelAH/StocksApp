using Entities.Models;
using ServiceContracts.Contracts;
using ServiceContracts.DTO;
using Services.Helpers;

namespace Services
{
    public class StocksService : IStocksService
    {
        #region Fields

        private readonly List<BuyOrder> _buyOrders;
        private readonly List<SellOrder> _sellOrders;

        #endregion

        #region Ctors

        public StocksService()
        {
            _buyOrders = new List<BuyOrder>();
            _sellOrders = new List<SellOrder>();
        }

        #endregion

        #region Methods

        public Task<BuyOrderResponse> CreateBuyOrder(BuyOrderRequest? buyOrderRequest)
        {
            if (buyOrderRequest == null)
            {
                throw new ArgumentNullException(nameof(buyOrderRequest));
            }
            ValidationHelper.ModelValidation(buyOrderRequest);

            BuyOrder buyOrder = buyOrderRequest.ToBuyOrder();
            buyOrder.BuyOrderID = Guid.NewGuid();
            _buyOrders.Add(buyOrder);

            return Task.FromResult(buyOrder.ToBuyOrderResponse());
        }

        public Task<SellOrderResponse> CreateSellOrder(SellOrderRequest? sellOrderRequest)
        {
            if (sellOrderRequest == null)
            {
                throw new ArgumentNullException(nameof(sellOrderRequest));
            }
            ValidationHelper.ModelValidation(sellOrderRequest);

            SellOrder sellOrder = sellOrderRequest.ToSellOrder();
            sellOrder.SellOrderID = Guid.NewGuid();
            _sellOrders.Add(sellOrder);

            return Task.FromResult(sellOrder.ToSellOrderResponse());
        }

        public Task<List<BuyOrderResponse>> GetAllBuyOrders()
        {
            return Task.FromResult(_buyOrders.Select(buyOrder => buyOrder.ToBuyOrderResponse()).ToList());
        }

        public Task<List<SellOrderResponse>> GetAllSellOrders()
        {
            return Task.FromResult(_sellOrders.Select(sellOrder => sellOrder.ToSellOrderResponse()).ToList());
        }

        #endregion
    }
}
