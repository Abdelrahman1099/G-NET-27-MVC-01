using GymManagement.BLL.Services.Attachment;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.BLL.Services.Classes
{
    public class AttachmentService : IAttachmentService
    {
        public AttachmentService(IWebHostEnvironment env) 
        {
            _env = env;
        }

        private readonly long _maxFileSize = 5 * 1024 * 1024;
        private readonly string[] _allowedExtension = [".jpg", ".jpeg", ".png"];
        private readonly IWebHostEnvironment _env;

        public async Task<string?> UploadAsync(Stream fileStream, string folderName, string fileName, CancellationToken ct = default)
        {
            if (fileStream is null || !fileStream.CanRead) return null;
            if (fileStream.Length == 0) return null;
            if (fileStream.Length > _maxFileSize) return null;

            var extension = Path.GetExtension(fileName);
            if (string.IsNullOrWhiteSpace(extension) || !_allowedExtension.Contains(extension.ToLower()))
                return null;

            var uploadsFolder = Path.Combine(_env.ContentRootPath, folderName);
            Directory.CreateDirectory(uploadsFolder);

            var storedFileName = $"{Guid.NewGuid()}{fileName}";
            
            var filePath = Path.Combine(uploadsFolder, storedFileName);

            try
            {

                using var fs = new FileStream(filePath, FileMode.Create, FileAccess.Write);
                await fileStream.CopyToAsync(fs);
                return storedFileName;
            }

            catch (Exception ex) 
            {
                return null;
            }

        }

        public bool Delete(string folderName, string fileName)
        {
            try
            {
                var filePath = Path.Combine(_env.ContentRootPath, folderName, fileName);
                if (!File.Exists(filePath)) return false;
                    File.Delete(filePath);
                return true;
            }
            catch (Exception ex) 
            {
                return false;   
            }
        }

        public (Stream stream, string contentType)? GetFile(string folderName, string fileName)
        {
            if (string.IsNullOrWhiteSpace(folderName) || string.IsNullOrWhiteSpace(fileName)) return null;
            var filePath = Path.Combine(_env.ContentRootPath, folderName, fileName);
            if (!File.Exists(filePath)) return null;

            var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            var extension = Path.GetExtension(filePath).ToLower();
            var contentType = extension switch
            {
                ".png" => "img/png",
                "jpg" or "jpeg" => "img/jpeg",
                _ => "application/octet-stream"
            };

            return (stream, contentType);   
        }
    }
}
