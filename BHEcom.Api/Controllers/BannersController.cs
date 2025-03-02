using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using BHEcom.Common.Models;
using BHEcom.Services;
using BHEcom.Services.Interfaces;
using BHEcom.Data.Repositories;
using BHEcom.Common.Helper;
using BHEcom.Services.Implementations;

namespace BHEcom.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BannersController : ControllerBase
    {
        private readonly IBannerService _bannerService;
        private readonly ILogger<BannersController> _logger;
        private readonly FtpUploader _ftpUploader;

        public BannersController(IBannerService bannerService, ILogger<BannersController> logger, FtpUploader ftpUploader)
        {
            _bannerService = bannerService;
            _logger = logger;
            _ftpUploader = ftpUploader;
        }

        [HttpGet("GetAll")]
        public async Task<ActionResult<IEnumerable<Banner>>> GetAll()
        {
            try
            {
                var banners = await _bannerService.GetAllBannersAsync();
                return Ok(new { data = banners, Success = true });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving banners.");
                return StatusCode(500, new { Message = ex.Message, Success = false });
            }
        }

        [HttpPost("Create")]
        [Consumes("multipart/form-data")]
        public async Task<ActionResult> Create([FromForm] Banner banner, [FromForm] IFormFile imageFile)
        {
            try
            {
                if (imageFile != null && imageFile.Length > 0)
                {
                    string folderName = "ecom/banners";
                    string imageUrl = await _ftpUploader.UploadFileAsync(imageFile, folderName);

                    banner.Image = imageUrl;
                }
                banner.CreatedDate = DateTime.Now;
                banner.IsActive = true;
                Guid bannerID = await _bannerService.AddBannerAsync(banner);

                return Ok(new { id = bannerID, Success = true });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating a banner.");
                return StatusCode(500, new { Message = ex.Message, Success = false });
            }
        }

        [HttpGet("GetById/{id}")]
        public async Task<ActionResult<Banner>> GetById(Guid id)
        {
            try
            {
                var banner = await _bannerService.GetBannerByIdAsync(id);
                if (banner == null)
                {
                    return Ok(new {data = banner, Message = "Banner not found", Success = true });
                }
                return Ok(new { data = banner, Success = true });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving a banner.");
                return StatusCode(500, new { Message = ex.Message, Success = false });
            }
        }

        [HttpPut("Update/{id}")]
        [Consumes("multipart/form-data")]
        public async Task<ActionResult> Update(Guid id, [FromForm] Banner banner, [FromForm] IFormFile imageFile)
        {
            try
            {
                if (id != banner.BannerID)
                {
                    return BadRequest(new { Message = "Banner ID mismatch", Success = false });
                }

                if (imageFile != null && imageFile.Length > 0)
                {
                    string folderName = "ecom/banners";
                    string imageUrl = await _ftpUploader.UploadFileAsync(imageFile, folderName);
                    banner.Image = imageUrl;
                }
                banner.ModifiedDate = DateTime.Now;
                var (isUpdated, oldImageUrl) = await _bannerService.UpdateBannerAsync(banner);

                if (!isUpdated)
                {
                    return Ok(new { Message = "Update failed", Success = false });
                }

                if (!string.IsNullOrEmpty(oldImageUrl))
                {
                    _ftpUploader.DeleteFile(oldImageUrl);
                }

                return Ok(new { Message = "Successfully updated", Success = true });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating a banner.");
                return StatusCode(500, new { Message = ex.Message, Success = false });
            }
        }

        [HttpDelete("Delete/{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            try
            {
                await _bannerService.DeleteBannerAsync(id);
                return Ok(new { Message = "Successfully deleted", Success = true });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting a banner.");
                return StatusCode(500, new { Message = ex.Message, Success = false });
            }
        }
    }

}
