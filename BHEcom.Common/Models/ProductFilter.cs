using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHEcom.Common.Models
{
    public class ProductFilter
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public bool? LowToHigh { get; set; } = null;
        public bool? HighToLow { get; set; } = null ;
        public bool? LatestToOld { get; set; } = null;  
        public decimal? MinPrice { get; set; } = null;
        public decimal? MaxPrice { get; set; } = null;
        public Guid? BrandId { get; set; } = null;    
        public Guid? CategoryId { get; set; } = null;

    }
}
