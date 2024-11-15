using BHEcom.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHEcom.Services.Interfaces
{
    public interface IAttributeService
    {
        Task<IEnumerable<Attributes>> GetAllAttributesAsync();
        Task<Attributes> GetAttributeByIdAsync(Guid id);
        Task AddAttributeAsync(Attributes attribute);
        Task UpdateAttributeAsync(Attributes attribute);
        Task DeleteAttributeAsync(Guid id);
    }
}
