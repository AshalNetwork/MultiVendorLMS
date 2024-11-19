using Microsoft.AspNetCore.Http;

namespace  Services.FileServices
{
    public interface IFileConverterService
    {
        Task<string> Convert(IFormFile file);
    }
}
