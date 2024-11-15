using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHEcom.Common.Models
{
    public class ProductManager
    {
        public string? Description { get; set; }
        public string? ShortDescription { get; set; }

        public IEnumerable<ProductDetail>? ProductDetailList { get; set; }


    }

}
