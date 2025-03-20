using Intranet_NEW.Models.WEB;
using Intranet_NEW.Services.Validadores;
using Intranet_NEW.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using FluentValidation;

namespace Intranet_NEW.Controllers
{
    public class LoginController : Controller
    {

        private readonly UsuarioService usuarioService;
        private readonly UsuarioValidator usuarioValidator;

        public LoginController()
        {
            usuarioService = new UsuarioService();
            usuarioValidator = new UsuarioValidator();
        }



        #region Acesso 


        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login","Login");
        }

        [AllowAnonymous]
        public IActionResult AcessoNegado()
        {
            ViewBag.Timeout = true;
            return View("Login");
        }

        [AllowAnonymous]
        public async Task<IActionResult> RealizarLogin(Colaborador model)
        {
            var senhaEstacorreta = usuarioValidator.Validate(model, options => options.IncludeRuleSets("VerificarSenha"));

            if (senhaEstacorreta.IsValid)
            {
                model = usuarioService.GetColaborador(model.NR_CPF.Replace("-", "").Replace(".", ""));

                await AutenticarUsuario(model);

                return RedirectToAction("Feed", "Home"); 
            }
            else
            {
                ViewBag.ErroLog = true;
                return View("Login");
            }

        }

        private async Task AutenticarUsuario(Colaborador model)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, model.NM_COLABORADOR),
                new Claim(ClaimTypes.NameIdentifier, model.NR_COLABORADOR),
                new Claim(ClaimTypes.Actor, model.NM_FUNCAO_RH),
                new Claim(ClaimTypes.GroupSid, model.NR_ATIVIDADE_RH),
                new Claim(ClaimTypes.Role, model.TP_PRIORIDADE_ACESSO.ToString())
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var authProperties = new AuthenticationProperties { IsPersistent = false };

            // Realiza a autenticação, ou seja, cria o cookie de autenticação
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);


        }


        [AllowAnonymous]
        public async Task<IActionResult> ResetarSenha([FromBody] Colaborador model)
        {
            var modelstate = usuarioValidator.Validate(model, options => options.IncludeRuleSets("AlterarSenha"));

            if (!modelstate.IsValid)
            {
                return BadRequest(new { erro = modelstate.Errors.Select(e => e.ErrorMessage) });
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
