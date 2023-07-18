namespace StocksApp.ServiceContracts
{
    public interface IFinnhubService
    {
        #region Methods

        /// <summary>
        /// Getting the stock data from finnhub website and returns it in a form of dictionary.
        /// </summary>
        /// <param name="stockSymbol">String stock symbol</param>
        /// <returns>Data in form of dictionary</returns>
        Task<Dictionary<string, object>?> GetStockPriceQuote(string stockSymbol);

        /// <summary>
        /// Getting the company profile data from finnhub website and returns it in a form of dictionary
        /// </summary>
        /// <param name="stockSymbol">String stock symbol</param>
        /// <returns>Data in form of dictionary</returns>
        Task<Dictionary<string, object>?> GetCompanyProfile(string stockSymbol);

        /// <summary>
        /// Getting the stocks data from finnhub website and returns it in a form of dictionary
        /// </summary>
        /// <returns>Data in form of dictionary</returns>
        Task<List<Dictionary<string, object>>?> GetStocks();

        /// <summary>
        /// Getting the stock details by stock symbol
        /// </summary>
        /// <param name="stockSymbolToSearch">String stock symbol to search</param>
        /// <returns>Data in form of dictionary</returns>
        Task<Dictionary<string, object>?> SearchStocks(string stockSymbolToSearch);

        #endregion
    }
}

