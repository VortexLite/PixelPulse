using Microsoft.AspNetCore.Http;

namespace PixelPulse.Application.Interfaces.Service;

public interface IFileService
{
    Task<string> SaveFileAsync(IFormFile file, string subFolder);
}