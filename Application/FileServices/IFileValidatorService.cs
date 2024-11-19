using Microsoft.AspNetCore.Http;

namespace  Services.FileServices
{
    public interface IFileValidatorService
    {
        public void Validate(IFormFile file);
    }
}
