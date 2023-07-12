using Entities.Models;

namespace ServiceContracts.DTO
{
    public class SellOrderResponse : IOrderResponse
    {
        #region Properties

        public Guid SellOrderID { get; set; }
        public string? StockSymbol { get; set; }
        public string? StockName { get; set; }
        public DateTime? DateAndTimeOfOrder { get; set; }
        public uint Quantity { get; set; }
        public double Price { get; set; }
        public double? TradeAmount { get; set; }
        public OrderType TypeOfOrder => OrderType.SellOrder;

        #endregion

        #region Methods

        /// <summary>
        /// Compares the current object data with the parameter object
        /// </summary>
        /// <param name="obj">The BuyOrderResponse object to compare</param>
        /// <returns>True or false, indicating whether all BuyOrder details are
        /// matched with the specified parameter object</returns>
        public override bool Equals(object? obj)
        {
            if (obj == null)
            {
                return false;
            }
            if (obj.GetType() != typeof(SellOrderResponse))
            {
                return false;
            }

            SellOrderResponse sellOrderResponse = (SellOrderResponse)obj;

            return this.SellOrderID == sellOrderResponse.SellOrderID &&
                   this.StockSymbol == sellOrderResponse.StockSymbol &&
                   this.StockName == sellOrderResponse.StockName &&
                   this.DateAndTimeOfOrder == sellOrderResponse.DateAndTimeOfOrder &&
                   this.Quantity == sellOrderResponse.Quantity &&
                   this.Price == sellOrderResponse.Price &&
                   this.TradeAmount == sellOrderResponse.TradeAmount;
        }

        public override string ToString()
        {
            return $"\nSellOrderID: {SellOrderID}\n" +
                   $"StockSymbol: {StockSymbol}\n" +
                   $"StockName: {StockName}\n" +
                   $"DateAndTimeOfOrder: {DateAndTimeOfOrder}\n" +
                   $"Quantity: {Quantity}\n" +
                   $"Price: {Price}\n" +
                   $"TradeAmount: {TradeAmount}\n";
        }

        public override int GetHashCode()
        {
            throw new NotImplementedException();
        }

        #endregion
    }

    public static class SellOrderExtensions
    {
        /// <summary>
        /// An Extension method to convert an object of BuyOrder class into SellOrderResponse class
        /// </summary>
        /// <param name="sellOrder">The sellOrder object to convert</param>
        /// <returns>Returns the converted SellOrderResponse object</returns>
        public static SellOrderResponse ToSellOrderResponse(this SellOrder sellOrder)
        {
            return new SellOrderResponse()
            {
                SellOrderID = sellOrder.SellOrderID,
                StockSymbol = sellOrder.StockSymbol,
                StockName = sellOrder.StockName,
                Price = sellOrder.Price,
                Quantity = sellOrder.Quantity,
                DateAndTimeOfOrder = sellOrder.DateAndTimeOfOrder,
                TradeAmount = sellOrder.Price * sellOrder.Quantity
            };
        }
    }
}
