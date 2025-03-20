using FluentValidation;
using Intranet_NEW.Models.WEB;
using Intranet_NEW.Services;
using Intranet_NEW.Services.Validadores;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Options;
using System.Security.Claims;

namespace Intranet_NEW.Controllers
{
    public class PublicacaoController : Controller
    {
        private readonly PublicacaoService _publicacaoService;
        private readonly PublicacaoValidator _publicacaoValidator;
        private readonly CarteiraService _carteiraService;
        private readonly ReacaoService _reacaoService;

        public PublicacaoController()
        {
            _publicacaoService = new PublicacaoService();
            _publicacaoValidator = new PublicacaoValidator();
            _carteiraService = new CarteiraService();
            _reacaoService = new ReacaoService();
        }

        private async void CarregarItems()
        {
            List<SelectListItem> carteiras = new List<SelectListItem>();
            List<CarteiraModel> listaCarteiras = _carteiraService.ListaCarteiras();
            List<PublicacaoModel> publicacoes = _publicacaoService.ListaPublicacoes(Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier).Value));
            carteiras.Add(new SelectListItem("Todas as Carteiras", "0"));
            foreach (CarteiraModel carteira in listaCarteiras)
            {
                carteiras.Add(new SelectListItem { Value = carteira.Name, Text = carteira.Name });
            }
            ViewBag.Carteiras = carteiras;
            ViewBag.Publicacoes = publicacoes;
            ViewBag.PermitirExclusao = true;

        }

        [Authorize]
        public IActionResult Index()
        {
            PublicacaoModel model = new();
            CarregarItems();
            return View("Index",model);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Curtir(int Id)
        {
            PublicacaoModel model = _publicacaoService.BuscaPublicacao(Id);
            model.Curtidas += 1;
            if (model == null)
                return BadRequest();

            ReacaoModel reacao = new ();
            reacao.IdReacao = 1;
            reacao.dataReacao = DateTime.Now;
            reacao.Usuario = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            _reacaoService.Reagir(reacao,model);

            return Json(new { novaQuantidade = model.Curtidas});
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Descurtir(int Id)
        {
            PublicacaoModel model = _publicacaoService.BuscaPublicacao(Id);
            model.Descurtidas += 1;
            if (model == null)
                return BadRequest();

            ReacaoModel reacao = new();
            reacao.IdReacao = 2;
            reacao.dataReacao = DateTime.Now;
            reacao.Usuario = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier).Value);

             _reacaoService.Reagir(reacao, model);

            return Json(new { novaQuantidade = model.Descurtidas });
        }


        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest(new { message = "Nenhum arquivo selecionado." });

            var rand = new Random();

            var fileName = Path.GetFileName(file.FileName.Replace("mceclip","imgupload" + rand.Next(0,999999)));
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads", fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                file.CopyTo(stream);
            }

            var fileUrl = $"/uploads/{fileName}";
            return Json(new { location = fileUrl });
        }

        [Authorize]
        public IActionResult Publicar(PublicacaoModel model)
        {
            ModelState.Clear();
            var modelstate = _publicacaoValidator.Validate(model, options => { options.IncludeRuleSets("ValidarInsercao"); });

            if (modelstate.IsValid)
            {
                model.DataPublicacao = DateTime.Now;
                model.IdAutor = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier).Value);
                _publicacaoService.InserirPublicacao(model);
                return RedirectToAction("Index", "Publicacao");

            }
            else
            {
                foreach (var erro in modelstate.Errors)
                {
                    ModelState.AddModelError(string.Empty, erro.ErrorMessage);
                }
                ViewBag.ErroPub = true;
                CarregarItems();
                return View("Index",model);
            }
        }

        [Authorize]
        public IActionResult Excluir(int pubId)
        {
            _publicacaoService.ExcluirPublicacao(pubId);
            return RedirectToAction("Index");
        }

    }
}
