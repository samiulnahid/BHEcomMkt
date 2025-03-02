using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHEcom.Common.Models
{
    public class WishlistManager
    {
        public Guid WishlistID { get; set; }
        public Guid UserID { get; set; }
        public Guid WishlistItemID { get; set; }
        public Guid ProductID { get; set; }
        public String? ProductName { get; set; }
        public String? ShortDescription { get; set; }
        public String? StoreName { get; set; }
        public Guid? StoreID { get; set; }
        public decimal? Discount { get; set; }
        public decimal? Price { get; set; }
        public decimal? TotalPrice { get; set; }
        public int Quantity { get; set; } = 0;
        public int? Stock { get; set; }
        public String? Image { get; set; }
        public DateTime? WishlistCreatedDate { get; set; }
        public DateTime? AddedDate { get; set; }
    }

    public class WishlistManagerList
    {
        public Guid userId { get; set; }
        public List<WishlistManager> WishlistManager { get; set; }
       
    }
}
