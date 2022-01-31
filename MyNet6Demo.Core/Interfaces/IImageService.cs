using Microsoft.AspNetCore.Http;

namespace MyNet6Demo.Core.Interfaces
{
    public interface IImageService
    {
        Task<string> UploadImageAsync(IFormFile image, CancellationToken cancellationToken);

        Task<Byte[]> DownloadImageAsync(string uniqueFileName, CancellationToken cancellationToken);
    }
}