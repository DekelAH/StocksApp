namespace ServiceContracts.View_Models
{
    public class CompanyProfile
    {
        #region Properties

        public string? Country { get; set; }
        public string? Currency { get; set; }
        public string? Exchange { get; set; }
        public string? FinnhubIndustry { get; set; }
        public DateTime? IPO { get; set; }
        public string? Logo { get; set; }
        public int MarketCapitalization { get; set; }
        public string? Name { get; set; }
        public string? Phone { get; set; }
        public double ShareOutstanding { get; set; }
        public string? Ticker { get; set; }
        public string? WebUrl { get; set; }

        #endregion
    }
}
