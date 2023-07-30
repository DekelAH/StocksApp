using ServiceContracts.DTO;

namespace ServiceContracts.Contracts
{
    public interface IStocksCreaterService
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
    }
}
