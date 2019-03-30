using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace S_Buck_HW_4.Models.API
{
    public class StockQuote
    {
        public string Symbol { get; set; }
        public string CompanyName { get; set; }
        public string PrimaryExchange { get; set; }
        public string Sector { get; set; }
        public string CalculationPrice { get; set; }
        public decimal? Open { get; set; }
        public long? OpenTime { get; set; }
        public decimal? Close { get; set; }
        public long? CloseTime { get; set; }
        public decimal? High { get; set; }
        public decimal? Low { get; set; }
        [DataType(DataType.Currency)]
        public decimal? LatestPrice { get; set; }
        public string LatestSource { get; set; }
        public string LatestTime { get; set; }
        public long? LatestUpdate { get; set; }
        public int? LatestVolume { get; set; }
        public decimal? IexRealtimePrice { get; set; }
        public int? IexRealtimeSize { get; set; }
        public long? IexLastUpdated { get; set; }
        public decimal? DelayedPrice { get; set; }
        public long? DelayedPriceTime { get; set; }
        public decimal? ExtendedPrice { get; set; }
        public decimal? ExtendedChange { get; set; }
        public decimal? ExtendedChangePercent { get; set; }
        public long? ExtendedPriceTime { get; set; }
        public decimal? PreviousClose { get; set; }
        public decimal? Change { get; set; }
        public decimal? ChangePercent { get; set; }
        public decimal? IexMarketPercent { get; set; }
        public int? IexVolume { get; set; }
        public int? AvgTotalVolume { get; set; }
        public decimal? IexBidPrice { get; set; }
        public int? IexBidSize { get; set; }
        public decimal? IexAskPrice { get; set; }
        public int? IexAskSize { get; set; }
        public long? MarketCap { get; set; }
        public decimal? PeRatio { get; set; }
        public decimal? Week52High { get; set; }
        public decimal? Week52Low { get; set; }
        public decimal? YtdChange { get; set; }
    }
}
