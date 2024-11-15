using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHEcom.Common.Models
{
    public class StoreCategory
    {
        public Guid StoreCategoryID { get; set; }
        public Guid StoreID { get; set; }
        public Guid CategoryID { get; set; }
        public bool IsActive { get; set; } 

        [NotMapped]
        public string? CategoryName { get; set; }
    }
}
