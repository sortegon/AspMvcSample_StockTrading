using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace S_Buck_HW_4.Models
{
    public class StockSymbol
    {
        public string Symbol { get; set; }
        public string Name { get; set; }
        public string Date { get; set; }
        public bool IsEnabled { get; set; }
        public string Type { get; set; }
        public string IexId { get; set; }
    }

}
