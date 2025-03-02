using BHEcom.Common.Models;
using BHEcom.Data.Repositories;
using BHEcom.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace BHEcom.Services.Implementations
{
   
    public class ImageService : IImageService
    {
        private readonly IImageRepository _imageRepository;

        public ImageService(IImageRepository imageRepository)
        {
            _imageRepository = imageRepository;
        }

        public async Task<List<Images>> GetImagesByXIdAsync(Guid xId, string xType)
        {
            return await _imageRepository.GetImagesByXIdAsync(xId, xType);
        }
        public async Task<bool> DeleteImagesByXIdAsync(Guid xId, string xType)
        {
            return await _imageRepository.DeleteImagesByXIdAsync(xId, xType);
        }
        public async Task<(bool isDeleted, string? oldImageUrl)> DeleteImagesByImageIdAsync(Guid id)
        {
            return await _imageRepository.DeleteImagesByImageIdAsync(id);
        }
       
        public async Task<bool> AddImagelistAsync(List<Images> images)
        {
            return await _imageRepository.AddImagelistAsync(images);
        }

        public async Task<bool> AddImageAsync(Images image)
        {
            return await _imageRepository.AddImageAsync(image);
        }

    }
}
