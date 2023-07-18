using Entities.DbContextModels;
using Entities.Models;
using RepositoryContracts;

namespace Repositories
{
    public class StocksRepository : IStocksRepository
    {
        #region Fields

        private readonly StockMarketDbContext _stockMarketDbContext;

        #endregion

        #region Ctors

        public StocksRepository(StockMarketDbContext stockMarketDbContext)
        {
            _stockMarketDbContext = stockMarketDbContext;
        }

        #endregion

        #region Methods

        public Task<BuyOrder> CreateBuyOrder(BuyOrder buyOrder)
        {
            _stockMarketDbContext.Sp_InsertBuyOrder(buyOrder);
            return Task.FromResult(buyOrder);
        }

        public Task<SellOrder> CreateSellOrder(SellOrder sellOrder)
        {
            _stockMarketDbContext.Sp_InsertSellOrder(sellOrder);
            return Task.FromResult(sellOrder);
        }

        public Task<List<BuyOrder>> GetBuyOrders()
        {
            List<BuyOrder> buyOrders = _stockMarketDbContext.Sp_GetAllBuyOrders();
            return Task.FromResult(buyOrders);
        }

        public Task<List<SellOrder>> GetSellOrders()
        {
            List<SellOrder> sellOrders = _stockMarketDbContext.Sp_GetAllSellOrders();
            return Task.FromResult(sellOrders);
        }

        #endregion
    }
}
