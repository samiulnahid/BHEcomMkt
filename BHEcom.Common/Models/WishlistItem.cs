using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHEcom.Common.Models
{
    [Table("WishlistItems")]
    public class WishlistItem
    {
        [Key]
        public Guid WishlistItemID { get; set; }
        public Guid WishlistID { get; set; }
        public Guid ProductID { get; set; }
        public int Quantity { get; set; } = 1;
        public DateTime? AddedDate { get; set; }
    }
}
