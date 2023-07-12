using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceContracts.DTO
{
    public interface IOrderResponse
    {
        public string? StockSymbol { get; set; }
        public string? StockName { get; set; }
        OrderType TypeOfOrder { get; }
        public DateTime? DateAndTimeOfOrder { get; set; }
        public uint Quantity { get; set; }
        public double Price { get; set; }
        public double? TradeAmount { get; set; }
    }

    public enum OrderType
    {
        BuyOrder, SellOrder
    }
}
