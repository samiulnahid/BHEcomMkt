using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHEcom.Common.Models
{
    [Table("Cart")]
    public class Cart
    {
        [Key]
        public Guid CartID { get; set; }  
        public Guid UserID { get; set; } 
        public DateTime CreatedDate { get; set; }  

       
    }
}
