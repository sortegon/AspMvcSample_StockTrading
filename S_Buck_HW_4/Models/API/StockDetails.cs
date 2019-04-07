using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace S_Buck_HW_4.Models.API
{
    public class StockDetails
    {
        [DisplayName("Company Name")]
        public string CompanyName { get; set; }

        [DisplayName("Market Cap."), DataType(DataType.Currency), DisplayFormat(DataFormatString = "{0:C}", NullDisplayText = "----")]
        public long? MarketCap { get; set; }

        public decimal? Beta { get; set; }

        [DisplayName("52-Week High"), DataType(DataType.Currency), DisplayFormat(DataFormatString = "{0:C}", NullDisplayText = "----")]
        public decimal? Week52high { get; set; }

        [DisplayName("52-Week Low"), DataType(DataType.Currency), DisplayFormat(DataFormatString = "{0:C}", NullDisplayText = "----")]
        public decimal? Week52low { get; set; }

        [DisplayName("52-Week Change"), DataType(DataType.Currency), DisplayFormat(DataFormatString = "{0:C}", NullDisplayText = "----")]
        public decimal? Week52change { get; set; }

        public int? ShortInterest { get; set; }

        public string ShortDate { get; set; }

        [DisplayName("Dividend Rate"), DataType(DataType.Currency), DisplayFormat(DataFormatString = "{0:C}", NullDisplayText = "----")]
        public decimal? DividendRate { get; set; }

        [DisplayName("Dividend Yield"), DisplayFormat(DataFormatString =@"{0:0.000\%}", NullDisplayText = "----")]
        public decimal? DividendYield { get; set; }

        [DisplayName("Ex-Dividend Date"), DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:d}", NullDisplayText = "----")]
        public DateTime? ExDividendDate { get; set; }

        public decimal? LatestEPS { get; set; }

        public string LatestEPSDate { get; set; }

        public long? SharesOutstanding { get; set; }
        
        public long? Float { get; set; }

        public decimal? ReturnOnEquity { get; set; }

        public decimal? ConsensusEPS { get; set; }

        public int? NumberOfEstimates { get; set; }

        public string Symbol { get; set; }
        
        public long? EBITDA { get; set; }

        [DataType(DataType.Currency)]
        public long? Revenue { get; set; }
        
        public long? GrossProfit { get; set; }
        
        public long? Cash { get; set; }
        
        public long? Debt { get; set; }

        public decimal? TtmEPS { get; set; }
        
        public decimal? RevenuePerShare { get; set; }

        public decimal? RevenuePerEmployee { get; set; }

        [DisplayName("P/E Ratio High"), DataType(DataType.Currency), DisplayFormat(DataFormatString = "{0:C}", NullDisplayText = "----")]
        public decimal? PeRatioHigh { get; set; }

        [DisplayName("P/E Ratio Low"), DataType(DataType.Currency), DisplayFormat(DataFormatString = "{0:C}", NullDisplayText = "----")]
        public decimal? PeRatioLow { get; set; }

        public object EPSSurpriseDollar { get; set; }

        public decimal? EPSSurprisePercent { get; set; }

        public decimal? ReturnOnAssets { get; set; }

        public object ReturnOnCapital { get; set; }

        public decimal? ProfitMargin { get; set; }

        public decimal? PriceToSales { get; set; }

        public decimal? PriceToBook { get; set; }

        [DisplayName("200-Day Moving Avg."), DataType(DataType.Currency), DisplayFormat(DataFormatString = "{0:C}", NullDisplayText = "----")]
        public decimal? Day200MovingAvg { get; set; }

        [DisplayName("50-Day Moving Avg."), DataType(DataType.Currency), DisplayFormat(DataFormatString = "{0:C}", NullDisplayText = "----")]
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
