using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace S_Buck_HW_4.Models
{
    public class UserStock
    {
        [Key]
        public int UserID { get; set; }
        [Key]
        public string Symbol { get; set; }

        public int Shares { get; set; }
        
        /// <summary>
        /// The amount paid for the stocks a user holds
        /// </summary>
        public decimal Basis { get; set; }
    }
}
