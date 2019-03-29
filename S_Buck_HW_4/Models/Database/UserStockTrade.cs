using System;

namespace S_Buck_HW_4.Models.Database
{
    public class UserStockTrade
    {
        public int UserID { get; set; }
        public virtual User User { get; set; }

        public string Symbol { get; set; }
        public virtual StockCompany StockCompany { get; set; }

        public DateTime TradeDateTime { get; set; }

        public int Shares { get; set; }
        public decimal Price { get; set; }

        public virtual UserStock UserStock { get; set; }
    }
}
