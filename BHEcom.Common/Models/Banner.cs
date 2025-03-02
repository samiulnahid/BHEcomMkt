using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHEcom.Common.Models
{
    [Table("Banners")]
    public class Banner
    {
        [Key]
        public Guid BannerID { get; set; }
        public string Title { get; set; }
        public string? SubTitle { get; set; }
        public string? Image { get; set; }
        public string? ButtonText { get; set; }
        public string? ButtonLink { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }

}
