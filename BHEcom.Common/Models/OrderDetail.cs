using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHEcom.Common.Models
{
    public class OrderDetail
    {
        [Key]
        public Guid OrderDetailID { get; set; }
        public Guid OrderID { get; set; }
        public Guid ProductID { get; set; }
        public int Quantity { get; set; }

        [Range(0, 9999999999999999.99)]
        public decimal Price { get; set; }
    }
}
