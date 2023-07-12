using Entities.DbContextModels;
using Entities.Models;
using ServiceContracts.Contracts;
using ServiceContracts.DTO;
using Services.Helpers;

namespace Services
{
    public class StocksService : IStocksService
    {
        #region Fields

        private readonly StockMarketDbContext _db;

        #endregion

        #region Ctors

        public StocksService(StockMarketDbContext stockMarketDbContext)
        {
            _db = stockMarketDbContext;
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

            _db.Sp_InsertBuyOrder(buyOrder);
            //await _db.AddAsync(buyOrder);
            //await _db.SaveChangesAsync();

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

            _db.Sp_InsertSellOrder(sellOrder);
            //await _db.AddAsync(sellOrder);
            //await _db.SaveChangesAsync();

            return Task.FromResult(sellOrder.ToSellOrderResponse());
        }

        public Task<List<BuyOrderResponse>> GetAllBuyOrders()
        {
            return Task.FromResult(_db.Sp_GetAllBuyOrders().Select(buyOrder => buyOrder.ToBuyOrderResponse()).ToList());
        }

        public Task<List<SellOrderResponse>> GetAllSellOrders()
        {
            return Task.FromResult(_db.Sp_GetAllSellOrders().Select(sellOrder => sellOrder.ToSellOrderResponse()).ToList());
        }

        #endregion
    }
}
