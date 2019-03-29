using System.Collections.Generic;

namespace S_Buck_HW_4.Models.Database
{
    public class UserStock
    {
        public int UserID { get; set; }
        public virtual User User { get; set; }

        public string Symbol { get; set; }
        public virtual StockCompany StockCompany { get; set; }

        public int Shares { get; set; }
        
        /// <summary>
        /// The amount paid for the stocks a user holds
        /// </summary>
        public decimal Basis { get; set; }

        public virtual ICollection<UserStockTrade> UserStockTrades { get; set; }
    }
}
