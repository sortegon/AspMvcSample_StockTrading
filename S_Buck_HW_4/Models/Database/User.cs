using System.Collections.Generic;
using System.ComponentModel;

namespace S_Buck_HW_4.Models.Database
{
    public class User
    {
        public int UserID { get; set; }
        [DisplayName("First Name")]
        public string FirstName { get; set; }
        [DisplayName("Last Name")]
        public string LastName { get; set; }

        public virtual ICollection<UserStock> Stocks { get; set; }
        public virtual ICollection<UserStockFavorite> Favorites { get; set; }
    }
}
