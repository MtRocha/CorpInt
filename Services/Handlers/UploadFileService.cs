using Microsoft.Extensions.Configuration;

namespace Intranet_NEW.Services.Handlers
{
    public interface IUploadFileService
    {
        Task<string> UploadFile(IFormFile file);
    }

    public class UploadFileService : IUploadFileService
    {
        private readonly string _uploadPath;

        public UploadFileService(IConfiguration configuration)
        {
            _uploadPath = configuration["UploadSettings:BasePath"] ?? "C:\\uploads";

            // Garante que o diretório exista
            if (!Directory.Exists(_uploadPath))
                Directory.CreateDirectory(_uploadPath);
        }

        public async Task<string> UploadFile(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return null;

            var fileName = Path.GetFileName(file.FileName); // Segurança
            string path = Path.Combine(_uploadPath, fileName);

            using (var stream = new FileStream(path, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return fileName;
        }
    }
}
