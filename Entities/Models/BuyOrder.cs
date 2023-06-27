using Entities.CustomValidators;
using System.ComponentModel.DataAnnotations;

namespace Entities.Models
{
    public class BuyOrder
    {
        #region Properties

        [Required(ErrorMessage = "BuyOrderID can't be blank.")]
        public Guid BuyOrderID { get; set; }

        [Required(ErrorMessage = "StockSymbol can't be blank.")]
        public string? StockSymbol { get; set; }

        [Required(ErrorMessage = "StockName can't be blank.")]
        public string? StockName { get; set; }

        [MinimumDateCustomValidator(2000, ErrorMessage = "Minimum date should be no higher than Jan 1, {0}")]
        public DateTime? DateAndTimeOfOrder { get; set; }

        [Range(1, 100000, ErrorMessage = "{0} should be between {1} and {2}")]
        public uint Quantity { get; set; }

        [Range(1, 10000, ErrorMessage = "{0} should be between {1} and {2}")]
        public double Price { get; set; }

        #endregion
    }
}
