using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHEcom.Common.Models
{
    public class Page
    {
        [Key]
        public Guid PageID { get; set; }

        [Required]
        [StringLength(255)]
        public string Category { get; set; }

        [Required]
        [StringLength(255)]
        public string Title { get; set; }

        [Required]
        public Guid SEOId { get; set; }

        [Required]
        public Guid CreatedBy { get; set; }

        [Required]
        public DateTime CreateDate { get; set; }

        [Required]
        public Guid LastModifiedBy { get; set; }

        [Required]
        public DateTime LastModifiedDate { get; set; }

        [Required]
        public bool IsActive { get; set; }
    }
}
