using Intranet_NEW.Models.WEB;
using Intranet_NEW.Services;
using Intranet_NEW.Services.Handlers;
using Intranet_NEW.Services.Validadores;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using System.Security.Claims;

namespace Intranet_NEW.Controllers
{
    public class ComentarioController : Controller
    {
        private readonly ComentarioService _comentarioService;
        private readonly ComentarioValidator _comentarioValidator;
        private readonly IHubContext<ReactionHubService> _reactionHubContext;
        IViewRenderService _viewRenderService;
        public ComentarioController(IViewRenderService viewRenderService
                                    , IHubContext<ReactionHubService> reactionHub)
        {
            _comentarioService = new ComentarioService();
            _comentarioValidator = new ComentarioValidator();
            _viewRenderService = viewRenderService;
            _reactionHubContext = reactionHub;

        }
        [Authorize]
        public async Task<IActionResult> ListaComentarios(int pubId,int quantidade, int pagina,DateTime data)
        {
            ViewBag.PermitirExclusao = User.FindFirst(ClaimTypes.Role).Value == "0" ? false : true;
            List<ComentarioModel> publicacoes = _comentarioService.ListaComentarios(pubId,pagina,quantidade,data);
            List<string> comentariosCarregados = new List<string>();
            foreach (ComentarioModel item in publicacoes)
            {
                comentariosCarregados.Add(await _viewRenderService.RenderToStringAsync(this, "_CompComentario", item));
            }

            return Json(comentariosCarregados);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Comentar(int id,string conteudo)
        {
            ModelState.Clear();
            ComentarioModel model = new();
            model.IdPub = id;
            model.dtComentario = DateTime.Now;
            model.Conteudo = conteudo;
            model.IdUsuario = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            model.UsuarioNome = User.FindFirst(ClaimTypes.Name).Value;
            var modelstate = _comentarioValidator.Validate(model);
            if (modelstate.IsValid)
            {
                _comentarioService.InsertComentario(model);
                int quantidadeComentarios = _comentarioService.ContarComentarios(model.IdPub);
                string comentarioComponente = await _viewRenderService.RenderToStringAsync(this, "_CompComentario", model);
                await _reactionHubContext.Clients.All.SendAsync("NovoComentario", model.IdPub, quantidadeComentarios,comentarioComponente);
                return Ok(new { comentarios = quantidadeComentarios , componente = comentarioComponente});

            }
            else
            {
                var erros = modelstate.Errors
                         .GroupBy(e => e.PropertyName)
                         .ToDictionary(
                             g => g.Key,
                             g => g.Select(e => e.ErrorMessage).ToList()
                         );

                return BadRequest(new
                {
                    erros = erros
                });
            } 

        }

    }
}
