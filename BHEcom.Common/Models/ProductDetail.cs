using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHEcom.Common.Models
{
    public class ProductDetail
    {
        [Key]
        public Guid DetailID { get; set; }
        public Guid ProductID { get; set; }
        public string DetailName { get; set; }
        public string DetailValue { get; set; }
    }
}
