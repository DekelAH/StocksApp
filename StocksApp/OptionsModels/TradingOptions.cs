namespace StocksApp.OptionsModels
{
    public class TradingOptions
    {
        #region Properties

        public string? DefaultStockSymbol { get; set; }
        public string? DefaultOrderQuantity { get; set; }
        public string? Top25PopularStocks { get; set; }

        #endregion
    }
}
