using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace S_Buck_HW_4.Models.Database
{
    public class StockCompany
    {
        [Key]
        public string Symbol { get; set; }
        [DisplayName("Company Name")]
        public string CompanyName { get; set; }
    }
}
