using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHEcom.Common.Models
{
    public class ResponseModel
    {
        public string Code { get; set; }
        public string Message { get; set; }
        public override string ToString()
        {
            // Customize the string representation as needed
            return $"Code: {Code}, Message: {Message}";
        }
    }
}
