using Intranet_NEW.Models.WEB;
using Intranet_NEW.Services.Validadores;
using Intranet_NEW.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using FluentValidation;
using System.Text;
using System.Text.Json;

namespace Intranet_NEW.Controllers
{
    public class LoginController : Controller
    {

        private readonly UsuarioService _usuarioService;
        private readonly UsuarioValidator _usuarioValidator;

        public LoginController()
        {
            _usuarioService = new UsuarioService();
            _usuarioValidator = new UsuarioValidator();

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
            var senhaEstacorreta = _usuarioValidator.Validate(model, options => options.IncludeRuleSets("VerificarSenha"));

            if (senhaEstacorreta.IsValid)
            {
                model = _usuarioService.GetColaborador(model.NR_CPF.Replace("-", "").Replace(".", ""));

                await AutenticarUsuario(model);

                if (model.TP_PRIORIDADE_ACESSO != 10)
                {
                    return RedirectToAction( "Index", "Administrativo"); 
                }
                return RedirectToAction("Feed", "Home"); 
            }
            else
            {
                ViewBag.ErroLog = true;
                return View("Login");
            }

        }

        [Authorize]
        public async Task<IActionResult> LoginCopilot()
        {
            string userCpf = User.FindFirst("CPF").Value;
            string userCopilotSenha = _usuarioService.ObterSenhaCopilot(userCpf);

            HttpClient client = new();

            var data = new Dictionary<string, string> {
                { "CPF_COLABORADOR" , userCpf },
                {  "senha" , userCopilotSenha}
            };

            var json = JsonSerializer.Serialize(data);
            
            var content = new StringContent(json,Encoding.UTF8,"application/json");

            var response = await client.PostAsync("http://172.20.1.126:5008/login", content);

            if (!response.IsSuccessStatusCode)
            {
                return Json(new { sucesso = false, mensagem = "Erro de login" });
            }

            var jsonResonse = await response.Content.ReadAsStringAsync();

            return Content(json,"application/json") ;
        }

        private async Task AutenticarUsuario(Colaborador model)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, model.NM_COLABORADOR),
                new Claim(ClaimTypes.NameIdentifier, model.NR_COLABORADOR),
                new Claim(ClaimTypes.Actor, model.NM_FUNCAO_RH),
                new Claim(ClaimTypes.PrimaryGroupSid,model.NR_FUNCAO_RH),
                new Claim(ClaimTypes.GroupSid, model.NR_ATIVIDADE_RH),
                new Claim("CentroCusto", model.NM_ATIVIDADE_RH),
                new Claim(ClaimTypes.Role, model.TP_PRIORIDADE_ACESSO.ToString()),
                new Claim("Supervisor",model.NR_SUPERVISOR.ToString()),
                new Claim("Coodernador",model.NR_SUPERVISOR.ToString()),
                new Claim("CPF",model.NR_CPF.ToString())
            };
            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var authProperties = new AuthenticationProperties { IsPersistent = false };

            // Realiza a autenticação, ou seja, cria o cookie de autenticação
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);


        }


        [AllowAnonymous]
        public async Task<IActionResult> ResetarSenha([FromBody] Colaborador model)
        {
            var modelstate = _usuarioValidator.Validate(model, options => options.IncludeRuleSets("AlterarSenha"));

            if (!modelstate.IsValid)
            {
                return BadRequest(new { erro = modelstate.Errors.Select(e => e.ErrorMessage) });
            }
            else
            {
                _usuarioService.AlteraSenhaUsuario(model);
                return Ok(new { sucesso = "Senha Alterada" });
            }
        }

        #endregion
    }
}
