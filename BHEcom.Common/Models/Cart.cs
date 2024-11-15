using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHEcom.Common.Models
{
    public class Cart
    {
        [Key]
        public Guid CartID { get; set; }  
        public Guid UserID { get; set; } 
        public DateTime CreatedDate { get; set; }  

        
        public Cart()
        {
            
        }
    }
}
