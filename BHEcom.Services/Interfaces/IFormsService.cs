using BHEcom.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHEcom.Services.Interfaces
{
    public interface IFormsService
    {
        Task<IEnumerable<Forms>> GetAllFormssAsync();
        Task<Forms> GetFormsByIdAsync(Guid id);
        Task AddFormsAsync(Forms forms);
        Task UpdateFormsAsync(Forms forms);
        Task DeleteFormsAsync(Guid id);
    }
}
