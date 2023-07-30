using ServiceContracts.DTO;

namespace ServiceContracts.Contracts
{
    public interface IStocksGetterService
    {
        /// <summary>
        /// Returns the existing list of buy orders retrieved from database table called 'BuyOrders'.
        /// </summary>
        /// <returns>A list of object of type BuyOrderResponse</returns>
        Task<List<BuyOrderResponse>> GetAllBuyOrders();

        /// <summary>
        /// Returns the existing list of sell orders retrieved from database table called 'SellOrders'.
        /// </summary>
        /// <returns>A list of object of type SellOrderResponse</returns>
        Task<List<SellOrderResponse>> GetAllSellOrders();
    }
}
