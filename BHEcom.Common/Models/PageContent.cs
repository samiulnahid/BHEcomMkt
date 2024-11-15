using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHEcom.Common.Models
{
    public class PageContent
    {
        [Key]
        public Guid ContentID { get; set; }
        public Guid PageID { get; set; }
        public string ContentType { get; set; }
        public string Content { get; set; }
        public string ImageUrl { get; set; }
        public string VideoUrl { get; set; }
        public string LinkUrl { get; set; }
        public string LinkText { get; set; }
        public int SortOrder { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime CreateDate { get; set; }
        public Guid LastModifiedBy { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public bool IsActive { get; set; }
    }
}
