using ServiceContracts.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceContracts.Contracts
{
    public interface IStocksService
    {
        /// <summary>
        /// Inserts a new buy order into the database table called 'BuyOrders'.
        /// </summary>
        /// <param name="buyOrderRequest">BuyOrder to add</param>
        /// <returns>Returns the same BuyOrder details, along with newly generated id and trade amount</returns>
        Task<BuyOrderResponse> CreateBuyOrder(BuyOrderRequest? buyOrderRequest);

        /// <summary>
        /// Inserts a new sell order into the database table called 'SellOrders'.
        /// </summary>
        /// <param name="sellOrderRequest">SellOrder to add</param>
        /// <returns>Returns the same SellOrder details, along with newly generated id and trade amount</returns>
        Task<SellOrderResponse> CreateSellOrder(SellOrderRequest? sellOrderRequest);

        /// <summary>
        /// Returns the existing list of buy orders retrieved from database table called 'BuyOrders'.
        /// </summary>
        /// <returns>A list of object of type BuyOrderResponse</returns>
        Task<List<BuyOrderResponse>> GetBuyOrders();

        /// <summary>
        /// Returns the existing list of sell orders retrieved from database table called 'SellOrders'.
        /// </summary>
        /// <returns>A list of object of type SellOrderResponse</returns>
        Task<List<SellOrderResponse>> GetSellOrders();
    }
}
