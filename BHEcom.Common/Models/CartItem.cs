using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHEcom.Common.Models
{
    [Table("CartItems")]
    public class CartItem
    {
        [Key]
        public Guid CartItemID { get; set; } 
        public Guid CartID { get; set; }    
        public Guid ProductID { get; set; }
        public int Quantity { get; set; } = 0;
    }
}
