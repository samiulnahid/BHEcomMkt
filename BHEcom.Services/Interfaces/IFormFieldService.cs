using BHEcom.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHEcom.Services.Interfaces
{
    public interface IFormFieldService
    {
        Task<IEnumerable<FormField>> GetAllFormFieldsAsync();
        Task<FormField> GetFormFieldByIdAsync(Guid id);
        Task AddFormFieldAsync(FormField formField);
        Task UpdateFormFieldAsync(FormField formField);
        Task DeleteFormFieldAsync(Guid id);
    }
}
