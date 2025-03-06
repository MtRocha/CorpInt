using System.Diagnostics;
using FluentValidation;
using Intranet_NEW.Models;
using Intranet_NEW.Models.WEB;
using Intranet_NEW.Services;
using Intranet_NEW.Services.Validadores;
using Microsoft.AspNetCore.Mvc;

namespace Intranet_NEW.Controllers
{
    public class HomeController : Controller
    {
        private readonly UsuarioService usuarioService;
        private readonly UsuarioValidator usuarioValidator;

        public HomeController()
        {
            usuarioService = new UsuarioService();
            usuarioValidator = new UsuarioValidator();
        }

        #region Acesso 

        public IActionResult Login()
        {
            return View();
        }

        public IActionResult RealizarLogin(Colaborador model)
        {
            var senhaEstacorreta = usuarioValidator.Validate(model,options => options.IncludeRuleSets("VerificarSenha"));

            if (senhaEstacorreta.IsValid)
            {
                model = usuarioService.GetColaborador(model.NR_CPF.Replace("-","").Replace(".",""));
                return View("Feed",model);
            }
            else
            {
                ViewBag.ErroLog = true;
                return View("Login");
            }

        }

        public async Task<IActionResult> ResetarSenha([FromBody] Colaborador model)
        {
            var modelstate = usuarioValidator.Validate(model,options => options.IncludeRuleSets("AlterarSenha"));

            if (!modelstate.IsValid)
            {
                return BadRequest(new { erro = modelstate.Errors.Select(e => e.ErrorMessage)});
            }
            else
            {
                usuarioService.AlteraSenhaUsuario(model);
                return Ok(new { sucesso = "Senha Alterada" });
            }
        }

        #endregion


    }


}
