using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHEcom.Common.Models
{
    public class Category
    {
        [Key]
        public Guid CategoryID { get; set; }
        public string CategoryName { get; set; }
        public string? Description { get; set; }
        public Guid? ParentCategoryID { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public bool IsActive { get; set; }
        public string? Image { get; set; }
    }
}
