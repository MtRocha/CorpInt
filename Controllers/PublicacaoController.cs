using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Intranet_NEW.Controllers
{
    public class PublicacaoController : Controller
    {

        public IActionResult Index()
        {
            return View("Index","Publicacao");
        }

        
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest(new { message = "Nenhum arquivo selecionado." });

            var fileName = Path.GetFileName(file.FileName);
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads", fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                file.CopyTo(stream);
            }

            var fileUrl = $"/uploads/{fileName}";
            return Json(new { location = fileUrl });
        }

    }
}
