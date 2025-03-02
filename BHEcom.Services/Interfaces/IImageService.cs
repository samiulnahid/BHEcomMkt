using BHEcom.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHEcom.Services.Interfaces
{
    public interface IImageService
    {
        Task<bool> DeleteImagesByXIdAsync(Guid xId, string xType);
        Task<(bool isDeleted, string? oldImageUrl)> DeleteImagesByImageIdAsync(Guid id);
        Task<List<Images>> GetImagesByXIdAsync(Guid xId, string xType);
        Task<bool> AddImagelistAsync(List<Images> images);
        Task<bool> AddImageAsync(Images image);
    }
   
}
