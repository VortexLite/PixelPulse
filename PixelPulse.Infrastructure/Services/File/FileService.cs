using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PixelPulse.Application.Interfaces.Service;

namespace PixelPulse.Infrastructure.Services.File;

public class FileService : IFileService
{
    private readonly IWebHostEnvironment _env;
    private ILogger<FileService> _logger;
    private readonly string? _webUiRootPath;

    public FileService(IWebHostEnvironment env, ILogger<FileService> logger, IConfiguration configuration)
    {
        _env = env ?? throw new ArgumentNullException(nameof(env));
        _logger = logger;
        _webUiRootPath = configuration["WebUIRootPath"]; 
    }
    
    public async Task<string> SaveFileAsync(IFormFile file, string subFolder)
    {
        if (file == null || file.Length == 0)
            throw new ArgumentException("File is empty");
        
        var directoryPath = Path.Combine(_webUiRootPath, subFolder);
        if (!Directory.Exists(directoryPath))
            Directory.CreateDirectory(directoryPath);

        var fileName = $"{Guid.NewGuid()}_{file.FileName}";
        var filePath = Path.Combine(directoryPath, fileName);
        
        await using var stream = new FileStream(filePath, FileMode.Create);
        await file.CopyToAsync(stream);
        
        return $"/{subFolder}/{fileName}";
    }
}