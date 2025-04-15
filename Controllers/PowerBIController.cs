using Intranet_NEW.Models.WEB;
using Intranet_NEW.Services.Validadores;
using Intranet_NEW.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using FluentValidation;
using Microsoft.AspNetCore.Mvc.Rendering;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Intranet_NEW.Services.Handlers;

namespace Intranet_NEW.Controllers
{
    public class PowerBIController : Controller
    {
        private readonly PowerBIService _powerBIService;
        private readonly TipoAcessoService _tipoAcessoService;
        private readonly PowerBiValidator _powerBIValidator;
        private readonly IViewRenderService _viewRenderService;
        private readonly IUploadFileService _uploadFileService;

        public PowerBIController(IViewRenderService viewRenderService,IUploadFileService uploadFileService)
        {
            _powerBIService = new PowerBIService();
            _viewRenderService = viewRenderService;
            _tipoAcessoService = new TipoAcessoService();
            _uploadFileService = uploadFileService;
            _powerBIValidator = new PowerBiValidator();
        }

        [Authorize]
        public IActionResult PowerBI()
        {
            PowerBiModel model = new();
           CarregarItems();
            return View(model);
        }

        private void CarregarItems()
        {
            List<SelectListItem> listaAcessos = new List<SelectListItem>();
            List<TipoAcessoModel> acessos = _tipoAcessoService.ListaTipoAcesso();
            foreach (TipoAcessoModel model in acessos)
            {
                listaAcessos.Add(new SelectListItem { Value = model.Id.ToString(), Text = model.Nome });
            }

            ViewBag.Acessos = listaAcessos;
            ViewBag.Edicao = false;
        }

        [Authorize]
        public async Task<IActionResult> CriarDashboard(PowerBiModel model)
        {
            ModelState.Clear();
            var modelstate = _powerBIValidator.Validate(model, options => { options.IncludeRuleSets("ValidarInsercao"); });

            if (modelstate.IsValid)
            {
                string caminho = model.Imagem == null ? "PowerBI_Template.png" : await _uploadFileService.UploadFile(model.Imagem);
                model.DtCriacao = DateTime.Now;
                model.idAutor = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier).Value);
                model.CaminhoImagem = $"/uploads/{caminho}";
                model.DtCriacao = DateTime.Now;
                model.idAutor = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier).Value);
                _powerBIService.IncluiDashBoard(model);
                return RedirectToAction("PowerBI");

            }
            else
            {
                foreach (var erro in modelstate.Errors)
                {
                    ModelState.AddModelError(string.Empty, erro.ErrorMessage);
                }
                ViewBag.Erro = true;
                CarregarItems();
                return View("PowerBI", model);
            }
        }

        [Authorize]
        public async Task<IActionResult> ListarDashBoards()
        {
            List<PowerBiModel> dashboards = _powerBIService.ListaDashboards(Convert.ToInt32(User.FindFirst(ClaimTypes.Role).Value), Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier).Value));
            List<string> dashBoardsRenderizados = new List<string>();
            foreach (PowerBiModel item in dashboards)
            {
                dashBoardsRenderizados.Add(await _viewRenderService.RenderToStringAsync(this, "_CompDashBoard", item));
            }

            return Json(dashBoardsRenderizados);

        }

        [Authorize]
        public IActionResult Excluir(int id)
        {
            _powerBIService.ExcluiDashBoard(id,Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier).Value));
            return RedirectToAction("PowerBi");

        }

        [Authorize]
        public IActionResult PrepararParaEdicao(int id)
        {

            PowerBiModel model = _powerBIService.BuscaDashBoard(id);
            CarregarItems();
            ViewBag.Edicao = true;
            return Json(new { model });

        }

        [Authorize]
        public async Task<IActionResult> EditarDashBoard(PowerBiModel model)
        {
            ModelState.Clear();
            var modelstate = _powerBIValidator.Validate(model, options => { options.IncludeRuleSets("ValidarInsercao"); });

            if (modelstate.IsValid)
            {
           
                string caminho = model.Imagem == null ? "PowerBI_Template.png" : await _uploadFileService.UploadFile(model.Imagem);
                model.DtCriacao = DateTime.Now;
                model.idAutor = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier).Value);
                model.CaminhoImagem = $"/uploads/{caminho}";
                model.DtCriacao = DateTime.Now;
                model.idAutor = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier).Value);
                _powerBIService.AlteraDashBoard(model,Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier).Value));
                return RedirectToAction("PowerBI");

            }
            else
            {
                foreach (var erro in modelstate.Errors)
                {
                    ModelState.AddModelError(string.Empty, erro.ErrorMessage);
                }
                CarregarItems();
                ViewBag.ErroEdit = true;
                ViewBag.Edicao = true;
                return View("PowerBI", model);
            }

        }

        [Authorize]
        public IActionResult VisualizarBI(int id)
        {
        
            PowerBiModel model = _powerBIService.BuscaDashBoard(id);

            if (Convert.ToInt32(User.FindFirst(ClaimTypes.Role).Value) <= model.TipoAcesso)
                return View("PowerBiVisualizar", model);
            else
                return RedirectToAction("PowerBI");
        
        }

    }

}
