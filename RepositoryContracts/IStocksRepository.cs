using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryContracts
{
    public interface IStocksRepository
    {
        #region Methods

        /// <summary>
        /// Inserts a new buy order into the database table called 'BuyOrders'.
        /// </summary>
        /// <param name="buyOrder">BuyOrder to add</param>
        /// <returns>Returns the same BuyOrder details, along with newly generated id and trade amount</returns>
        Task<BuyOrder> CreateBuyOrder(BuyOrder buyOrder);

        /// <summary>
        /// Inserts a new sell order into the database table called 'SellOrders'.
        /// </summary>
        /// <param name="sellOrder">SellOrder to add</param>
        /// <returns>Returns the same SellOrder details, along with newly generated id and trade amount</returns>
        Task<SellOrder> CreateSellOrder(SellOrder sellOrder);

        /// <summary>
        /// Returns the existing list of buy orders retrieved from database table called 'BuyOrders'.
        /// </summary>
        /// <returns>A list of object of type BuyOrder</returns>
        Task<List<BuyOrder>> GetBuyOrders();

        /// <summary>
        /// Returns the existing list of sell orders retrieved from database table called 'SellOrders'.
        /// </summary>
        /// <returns>A list of object of type SellOrder</returns>
        Task<List<SellOrder>> GetSellOrders();

        #endregion
    }
}
