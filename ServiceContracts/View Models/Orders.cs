
using ServiceContracts.DTO;

namespace ServiceContracts.View_Models
{
    public class Orders
    {
        #region Properties

        public List<BuyOrderResponse>? BuyOrders { get; set; }
        public List<SellOrderResponse>? SellOrders { get; set; }

        #endregion
    }
}
