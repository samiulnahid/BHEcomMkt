using BHEcom.Common.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BHEcom.Common.Helper
{
    public class FtpUploader
    {
        private readonly FtpSettings _ftpSettings;

        public FtpUploader(IOptions<FtpSettings> ftpSettings)
        {
            _ftpSettings = ftpSettings.Value;
        }

        public async Task<string> UploadFileAsync(IFormFile file, string folderName)
        {
            try
            {
                string uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(file.FileName);
                string remoteFilePath = $"{_ftpSettings.Server}/{folderName}/{uniqueFileName}";

                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(remoteFilePath);
                request.Method = WebRequestMethods.Ftp.UploadFile;
                request.Credentials = new NetworkCredential(_ftpSettings.Username, _ftpSettings.Password);
                request.UseBinary = true;

                using (Stream requestStream = request.GetRequestStream())
                {
                    await file.CopyToAsync(requestStream);
                }

                FtpWebResponse response = (FtpWebResponse)await request.GetResponseAsync();
                if (response.StatusCode != FtpStatusCode.ClosingData)
                {
                    throw new Exception($"FTP upload failed: {response.StatusDescription}");
                }
                response.Close();

                // Convert the FTP URL to HTTP URL
                string httpUrl = remoteFilePath.Replace("ftp://", "http://");

                return httpUrl;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error uploading file: {ex.Message}");
            }
        }

        public void DeleteFile(string imageUrl)
        {
            try
            {
                // Convert the HTTP URL back to an FTP URL
                string ftpUrl = imageUrl.Replace("http://", "ftp://");

                // Create an FTP WebRequest to delete the file
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(ftpUrl);
                request.Method = WebRequestMethods.Ftp.DeleteFile;
                request.Credentials = new NetworkCredential(_ftpSettings.Username, _ftpSettings.Password);

                using (FtpWebResponse response = (FtpWebResponse)request.GetResponse())
                {
                    if (response.StatusCode != FtpStatusCode.FileActionOK)
                    {
                        throw new Exception($"FTP delete failed: {response.StatusDescription}");
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error deleting file: {ex.Message}");
            }
        }

    }
}
