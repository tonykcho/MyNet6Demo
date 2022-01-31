using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using MyNet6Demo.Core.Interfaces;

namespace MyNet6Demo.Core.Services
{
    public class ImageService : IImageService
    {
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ImageService(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<string> UploadImageAsync(IFormFile image, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            ArgumentNullException.ThrowIfNull(image);

            string uploadsFolder = Path.Combine(_webHostEnvironment.ContentRootPath, "images");

            string uniqueFileName = $"{Guid.NewGuid().ToString()}_{image.FileName}";

            string filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using(var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await image.CopyToAsync(fileStream, cancellationToken);
            }

            return uniqueFileName;
        }

        public async Task<Byte[]> DownloadImageAsync(string uniqueFileName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            ArgumentNullException.ThrowIfNull(uniqueFileName);

            string uploadsFolder = Path.Combine(_webHostEnvironment.ContentRootPath, "images");

            string filePath = Path.Combine(uploadsFolder, uniqueFileName);

            Byte[] image = await File.ReadAllBytesAsync(filePath, cancellationToken);

            return image;
        }
    }
}