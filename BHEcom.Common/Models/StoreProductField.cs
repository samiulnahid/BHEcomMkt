using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHEcom.Common.Models
{
    [Table("StoreProductFields")]
    public class StoreProductField
    {
        [Key]
        public Guid ProductFieldID { get; set; }
        public Guid StoreCategoryID { get; set; }
        public Guid CategoryID { get; set; }
        public string EntityName { get; set; }
        public bool IsActive { get; set; } = false;
    }
}
