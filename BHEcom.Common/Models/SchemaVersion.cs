using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHEcom.Common.Models
{
    public class SchemaVersion
    {
        [Key]
        public string Feature { get; set; }
        public string CompatibleSchemaVersion { get; set; }
        public bool IsCurrentVersion { get; set; }
    }
}
