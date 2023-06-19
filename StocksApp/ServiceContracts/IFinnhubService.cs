namespace StocksApp.ServiceContracts
{
    public interface IFinnhubService
    {
        #region Methods

        Task<Dictionary<string, object>?> GetStockPriceQuote(string stockSymbol);
        Task<Dictionary<string, object>?> GetCompanyProfile(string stockSymbol);

        #endregion
    }
}
