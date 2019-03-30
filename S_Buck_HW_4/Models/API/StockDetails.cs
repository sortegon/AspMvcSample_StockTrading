using System.ComponentModel;

namespace S_Buck_HW_4.Models.API
{
    public class StockDetails
    {
        [DisplayName("Company Name")]
        public string CompanyName { get; set; }
        public long? MarketCap { get; set; }
        public decimal? Beta { get; set; }
        public decimal? Week52high { get; set; }
        public decimal? Week52low { get; set; }
        public decimal? Week52change { get; set; }
        public int? ShortInterest { get; set; }
        public string ShortDate { get; set; }
        public decimal? DividendRate { get; set; }
        public decimal? DividendYield { get; set; }
        public string ExDividendDate { get; set; }
        public decimal? LatestEPS { get; set; }
        public string LatestEPSDate { get; set; }
        public long? SharesOutstanding { get; set; }
        public long? Float { get; set; }
        public decimal? ReturnOnEquity { get; set; }
        public decimal? ConsensusEPS { get; set; }
        public int? NumberOfEstimates { get; set; }
        public string Symbol { get; set; }
        public long? EBITDA { get; set; }
        public long? Revenue { get; set; }
        public long? GrossProfit { get; set; }
        public long? Cash { get; set; }
        public long? Debt { get; set; }
        public decimal? TtmEPS { get; set; }
        public decimal? RevenuePerShare { get; set; }
        public decimal? RevenuePerEmployee { get; set; }
        public decimal? PeRatioHigh { get; set; }
        public decimal? PeRatioLow { get; set; }
        public object EPSSurpriseDollar { get; set; }
        public decimal? EPSSurprisePercent { get; set; }
        public decimal? ReturnOnAssets { get; set; }
        public object ReturnOnCapital { get; set; }
        public decimal? ProfitMargin { get; set; }
        public decimal? PriceToSales { get; set; }
        public decimal? PriceToBook { get; set; }
        public decimal? Day200MovingAvg { get; set; }
        public decimal? Day50MovingAvg { get; set; }
        public decimal? InstitutionPercent { get; set; }
        public object InsiderPercent { get; set; }
        public decimal? ShortRatio { get; set; }
        public decimal? Year5ChangePercent { get; set; }
        public decimal? Year2ChangePercent { get; set; }
        public decimal? Year1ChangePercent { get; set; }
        public decimal? YtdChangePercent { get; set; }
        public decimal? Month6ChangePercent { get; set; }
        public decimal? Month3ChangePercent { get; set; }
        public decimal? Month1ChangePercent { get; set; }
        public decimal? Day5ChangePercent { get; set; }
    }
}
