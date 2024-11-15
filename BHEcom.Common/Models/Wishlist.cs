using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHEcom.Common.Models
{
    public class Wishlist
    {
        [Key]
        public Guid WishlistID { get; set; }
        public Guid UserID { get; set; }
        public Guid ProductID { get; set; }
        public DateTime AddedDate { get; set; }
    }
}
