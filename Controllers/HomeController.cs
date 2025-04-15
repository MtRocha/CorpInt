using System.Diagnostics;
using System.Security.Claims;
using FluentValidation;
using Intranet_NEW.Models;
using Intranet_NEW.Models.WEB;
using Intranet_NEW.Services;
using Intranet_NEW.Services.Handlers;
using Intranet_NEW.Services.Validadores;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;

namespace Intranet_NEW.Controllers
{
    public class HomeController : Controller
    {
        private readonly PublicacaoService _publicacaoService;
        private readonly TipoAcaoService _tipoAcaoService;
        private readonly IViewRenderService _viewRenderService;
        public HomeController(IViewRenderService viewRenderService)
        {
            _tipoAcaoService = new TipoAcaoService();
            _publicacaoService = new PublicacaoService();
            _viewRenderService = viewRenderService;
        }

        private async void CarregarItems()
        {
            List<SelectListItem> acoes = new List<SelectListItem>();
            List<TipoAcaoModel> listaAcoes = _tipoAcaoService.ListaTipoAcao();
            acoes.Add(new SelectListItem("Todas","0"));

            foreach (TipoAcaoModel item in listaAcoes)
            {
                acoes.Add(new SelectListItem { Value = item.Id, Text = item.Name });
            }
            ViewBag.TipoAcao = acoes;
            ViewBag.PermitirExclusao = true;

        }



        [Authorize]
        public IActionResult Feed()
        {
            CarregarItems();
            ViewBag.PermitirExclusao = false;
            return View();
        }
    }


}
