namespace Intranet_NEW.Services.Handlers
{
    public interface IUploadFileService
    {
        Task<string> UploadFile(IFormFile file);
    }

    public class UploadFileService : IUploadFileService
    {
        public async Task<string> UploadFile(IFormFile file)
        {
            string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", file.FileName);
            using (var stream = new FileStream(path, FileMode.Create))
            {
                file.CopyTo(stream);
                return file.FileName.ToString();
            }
        }

    }
}
