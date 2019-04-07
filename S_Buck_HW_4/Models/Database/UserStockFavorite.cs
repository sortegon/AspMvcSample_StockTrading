using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace S_Buck_HW_4.Models.Database
{
    public class UserStockFavorite
    {
        public int UserID { get; set; }
        public virtual User User { get; set; }

        public string Symbol { get; set; }
        public virtual StockCompany StockCompany { get; set; }
    }
}
