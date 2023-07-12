using Entities.Models;

namespace ServiceContracts.DTO
{
    public class BuyOrderResponse : IOrderResponse
    {
        #region Properties

        public Guid BuyOrderID { get; set; }
        public string? StockSymbol { get; set; }
        public string? StockName { get; set; }
        public DateTime? DateAndTimeOfOrder { get; set; }
        public uint Quantity { get; set; }
        public double Price { get; set; }
        public double? TradeAmount { get; set; }
        public OrderType TypeOfOrder => OrderType.BuyOrder;

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
            if (obj.GetType() != typeof(BuyOrderResponse))
            {
                return false;
            }

            BuyOrderResponse buyOrderResponse = (BuyOrderResponse)obj;

            return this.BuyOrderID == buyOrderResponse.BuyOrderID &&
                   this.StockSymbol == buyOrderResponse.StockSymbol &&
                   this.StockName == buyOrderResponse.StockName &&
                   this.DateAndTimeOfOrder == buyOrderResponse.DateAndTimeOfOrder &&
                   this.Quantity == buyOrderResponse.Quantity &&
                   this.Price == buyOrderResponse.Price &&
                   this.TradeAmount == buyOrderResponse.TradeAmount;
        }

        public override string ToString()
        {
            return $"\nBuyOrderID: {BuyOrderID}\n" +
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

    public static class BuyOrderExtensions
    {
        /// <summary>
        /// An Extension method to convert an object of BuyOrder class into BuyOrderResponse class
        /// </summary>
        /// <param name="buyOrder">The buyOrder object to convert</param>
        /// <returns>Returns the converted BuyOrderResponse object</returns>
        public static BuyOrderResponse ToBuyOrderResponse(this BuyOrder buyOrder)
        {
            return new BuyOrderResponse()
            {
                BuyOrderID = buyOrder.BuyOrderID,
                StockSymbol = buyOrder.StockSymbol,
                StockName = buyOrder.StockName,
                Price = buyOrder.Price,
                Quantity = buyOrder.Quantity,
                DateAndTimeOfOrder = buyOrder.DateAndTimeOfOrder,
                TradeAmount = buyOrder.Price * buyOrder.Quantity
            };
        }
    }
}
