namespace StocksApp.ServiceContracts
{
    public interface IFinnhubSearcherService
    {
        #region Methods

        /// <summary>
        /// Getting the stock details by stock symbol
        /// </summary>
        /// <param name="stockSymbolToSearch">String stock symbol to search</param>
        /// <returns>Data in form of dictionary</returns>
        Task<Dictionary<string, object>?> SearchStocks(string stockSymbolToSearch);

        #endregion
    }
}

