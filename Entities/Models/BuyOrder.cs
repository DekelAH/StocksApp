using System.ComponentModel.DataAnnotations;

namespace Entities.Models
{
    public class BuyOrder
    {
        #region Properties

        [Key]
        public Guid BuyOrderID { get; set; }

        [StringLength(50)]
        public string? StockSymbol { get; set; }

        [StringLength(50)]
        public string? StockName { get; set; }

        public DateTime? DateAndTimeOfOrder { get; set; }

        public uint Quantity { get; set; }

        public double Price { get; set; }

        #endregion
    }
}
