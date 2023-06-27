namespace Entities.View_Models
{
    public class StockTrade
    {
        #region Properties

        public string? StockSymbol { get; set; }
        public double CurrentPrice { get; set; }
        public double Change { get; set; }
        public double PercentChange { get; set; }
        public double HighPrice { get; set; }
        public double LowPrice { get; set; }
        public double OpenPrice { get; set; }
        public double PreviousClosePrice { get; set; }
        public uint Quantity { get; set; }
        public CompanyProfile? CompanyProfile { get; set; }

        #endregion
    }
}
