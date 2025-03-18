using Intranet_NEW.Models.WEB;
using Intranet_NEW.Services;
using Intranet_NEW.Services.Validadores;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Intranet_NEW.Controllers
{
    public class PublicacaoController : Controller
    {
        private readonly PublicacaoService _publicacaoService;
        private readonly PublicacaoValidator _publicacaoValidator;
        private readonly CarteiraService _carteiraService;

        public PublicacaoController()
        {
            _publicacaoService = new PublicacaoService();
            _publicacaoValidator = new PublicacaoValidator();
            _carteiraService = new CarteiraService();
        }

        [Authorize]
        public IActionResult Index()
        {
            List<SelectListItem> carteiras = new List<SelectListItem>();
            List<CarteiraModel> listaCarteiras = _carteiraService.ListaCarteiras();
            carteiras.Add(new SelectListItem( "Todas as Carteiras","0"));
            foreach (CarteiraModel carteira in listaCarteiras)
            {
                carteiras.Add(new SelectListItem { Value = carteira.Id, Text = carteira.Name });
            }

            ViewBag.Carteiras = carteiras;
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

        [Authorize]
        [HttpPost]
        public IActionResult Publicar([FromBody] PublicacaoModel model)
        {
            var modelstate = _publicacaoValidator.Validate(model);

            if (modelstate.IsValid)
            {

                _publicacaoService.InserirPublicacao(model);
                return RedirectToAction("Index", "Publicacao");

            }
            else
            {
                foreach (var erro in modelstate.Errors)
                {
                    ModelState.AddModelError(string.Empty, erro.ErrorMessage);
                }
                return View("Index");
            }
        }

    }
}
