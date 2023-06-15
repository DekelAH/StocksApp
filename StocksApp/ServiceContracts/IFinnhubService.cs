namespace StocksApp.ServiceContracts
{
    public interface IFinnhubService
    {
        #region Methods

        Task<Dictionary<string, object>?> GetStockPriceQuote(string stockSymbol);

        #endregion
    }
}
