using System.ComponentModel;

namespace S_Buck_HW_4.Models.API
{
    public class StockDetails
    {
        [DisplayName("Company Name")]
        public string CompanyName { get; set; }
        public long Marketcap { get; set; }
        public float Beta { get; set; }
        public float Week52high { get; set; }
        public float Week52low { get; set; }
        public float Week52change { get; set; }
        public int ShortInterest { get; set; }
        public string ShortDate { get; set; }
        public float DividendRate { get; set; }
        public float DividendYield { get; set; }
        public string ExDividendDate { get; set; }
        public float LatestEPS { get; set; }
        public string LatestEPSDate { get; set; }
        public long SharesOutstanding { get; set; }
        public long Float { get; set; }
        public float ReturnOnEquity { get; set; }
        public float ConsensusEPS { get; set; }
        public int NumberOfEstimates { get; set; }
        public string Symbol { get; set; }
        public long EBITDA { get; set; }
        public long Revenue { get; set; }
        public long GrossProfit { get; set; }
        public long Cash { get; set; }
        public long Debt { get; set; }
        public float TtmEPS { get; set; }
        public float RevenuePerShare { get; set; }
        public float RevenuePerEmployee { get; set; }
        public float PeRatioHigh { get; set; }
        public float PeRatioLow { get; set; }
        public object EPSSurpriseDollar { get; set; }
        public float EPSSurprisePercent { get; set; }
        public float ReturnOnAssets { get; set; }
        public object ReturnOnCapital { get; set; }
        public float ProfitMargin { get; set; }
        public float PriceToSales { get; set; }
        public float? PriceToBook { get; set; }
        public float Day200MovingAvg { get; set; }
        public float Day50MovingAvg { get; set; }
        public float InstitutionPercent { get; set; }
        public object InsiderPercent { get; set; }
        public float? ShortRatio { get; set; }
        public float Year5ChangePercent { get; set; }
        public float Year2ChangePercent { get; set; }
        public float Year1ChangePercent { get; set; }
        public float YtdChangePercent { get; set; }
        public float Month6ChangePercent { get; set; }
        public float Month3ChangePercent { get; set; }
        public float Month1ChangePercent { get; set; }
        public float Day5ChangePercent { get; set; }
    }
}
