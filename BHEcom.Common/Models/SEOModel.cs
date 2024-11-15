using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHEcom.Common.Models
{
    public class SEO
    {
        [Key]
        public Guid SEOId { get; set; }
        public string MetaTitle { get; set; }
        public string MetaDescription { get; set; }
        public string MetaKeywords { get; set; }
        public string CanonicalUrl { get; set; }
        public string RobotsContent { get; set; }
        public string OpenGraphTitle { get; set; }
        public string OpenGraphDescription { get; set; }
        public string OpenGraphImage { get; set; }
        public string TwitterCard { get; set; }
        public string TwitterTitle { get; set; }
        public string TwitterDescription { get; set; }
        public string TwitterImage { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime CreateDate { get; set; }
        public Guid LastModifiedBy { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public bool IsActive { get; set; }
    }
}
