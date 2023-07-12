using Entities.CustomValidators;
using System.ComponentModel.DataAnnotations;

namespace Entities.Models
{
    public class SellOrder
    {
        #region Properties

        [Key]
        public Guid SellOrderID { get; set; }

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
