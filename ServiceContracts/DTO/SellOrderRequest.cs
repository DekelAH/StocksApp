using Entities.CustomValidators;
using Entities.Models;
using System.ComponentModel.DataAnnotations;

namespace ServiceContracts.DTO
{
    public class SellOrderRequest
    {
        #region Properties

        [Required(ErrorMessage = "StockSymbol can't be blank.")]
        public string? StockSymbol { get; set; }

        [Required(ErrorMessage = "StockName can't be blank.")]
        public string? StockName { get; set; }

        [MinimumDateCustomValidator(2000, ErrorMessage = "Minimum date should be no higher than Jan 1, {0}")]
        public DateTime? DateAndTimeOfOrder { get; set; }

        [Range(1, 100000, ErrorMessage = "Quantity should be between 1 and 100,000")]
        public uint Quantity { get; set; }

        [Range(1, 10000, ErrorMessage = "Price should be between 1 and 10,000")]
        public double Price { get; set; }

        #endregion

        #region Methods

        public SellOrder ToSellOrder()
        {
            return new SellOrder()
            {
                StockSymbol = StockSymbol,
                StockName = StockName,
                DateAndTimeOfOrder = DateAndTimeOfOrder,
                Quantity = Quantity,
                Price = Price,
            };
        }

        #endregion
    }
}
