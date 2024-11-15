using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHEcom.Common.Models
{
    public class FormField
    {
        [Key]
        public Guid FieldID { get; set; }
        public Guid FormID { get; set; }
        public string FieldName { get; set; }
        public string FieldType { get; set; }
        public string FieldLabel { get; set; }
        public bool IsRequired { get; set; }
        public int SortOrder { get; set; }
        public string TagType { get; set; }
        public string Placeholder { get; set; }
        public string CssforLabel { get; set; }
        public string CssforField { get; set; }
    }
}
