
using Microsoft.AspNetCore.Http;
using System;

namespace Services.FileServices
{
    public class FileConverterService(IFileValidatorService fileValidate) : IFileConverterService

    {
        public async Task<string> Convert(IFormFile file)
        {
            fileValidate.Validate(file);
            var fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
            var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Files");
            var filePath = Path.Combine(folderPath, fileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
            return $"/Files/{fileName}";
        }
    }
}
