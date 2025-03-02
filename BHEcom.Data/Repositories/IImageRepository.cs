using System.Collections.Generic;
using System.Threading.Tasks;
using BHEcom.Common.Models;

namespace BHEcom.Data.Repositories
{
    public interface IImageRepository
    {
        Task<bool> DeleteImagesByXIdAsync(Guid xId, string xType);
        Task<(bool isDeleted, string? oldImageUrl)> DeleteImagesByImageIdAsync(Guid id);
        Task<List<Images>> GetImagesByXIdAsync(Guid xId, string xType);
        Task<bool> AddImagelistAsync(List<Images> images);
        Task<bool> AddImageAsync(Images image);
    }
}
