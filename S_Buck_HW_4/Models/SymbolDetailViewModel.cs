using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using S_Buck_HW_4.Models.API;

namespace S_Buck_HW_4.Models
{
    public class SymbolDetailViewModel
    {
        public StockQuote Quote { get; set; }
        public StockDetails Details { get; set; }
    }
}
