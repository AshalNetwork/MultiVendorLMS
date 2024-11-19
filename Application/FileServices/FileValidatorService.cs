using Microsoft.AspNetCore.Http;

namespace  Services.FileServices
{
    public class FileValidatorService:IFileValidatorService
    {
        public void Validate(IFormFile file)
        {
            List<string> extensions = new List<string> { ".png", ".jpg", ".jpeg",".xlsx" };
            if (file is null ||
                file.Length <= 0 ||
                !extensions.Contains(Path.GetExtension(file.FileName).ToLower()))
                    throw new InvalidDataException("An error happened");

        }
    }
}
