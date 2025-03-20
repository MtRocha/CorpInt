using System.Diagnostics;
using System.Security.Claims;
using FluentValidation;
using Intranet_NEW.Models;
using Intranet_NEW.Models.WEB;
using Intranet_NEW.Services;
using Intranet_NEW.Services.Validadores;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace Intranet_NEW.Controllers
{
    public class HomeController : Controller
    {
        private readonly PublicacaoService _publicacaoService;

        public HomeController()
        {

            _publicacaoService = new PublicacaoService();
        
        }

        [Authorize]
        public IActionResult Feed()
        {
            List<PublicacaoModel> publicacoes = _publicacaoService.ListaPublicacoesParaFeed(User.FindFirst(ClaimTypes.GroupSid).Value, Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier).Value));
            ViewBag.Publicacoes = publicacoes;
            ViewBag.PermitirExclusao = false;
            return View();
        }

        

    }


}
