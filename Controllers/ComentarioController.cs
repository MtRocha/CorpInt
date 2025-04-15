using Intranet_NEW.Models.WEB;
using Intranet_NEW.Services;
using Intranet_NEW.Services.Handlers;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Intranet_NEW.Controllers
{
    public class ComentarioController : Controller
    {
        private readonly ComentarioService _comentarioService;
        IViewRenderService _viewRenderService;
        public ComentarioController(IViewRenderService viewRenderService)
        {
            _comentarioService = new ComentarioService();
            _viewRenderService = viewRenderService;
        }

        public async Task<IActionResult> ListaFeed(int pubId,int quantidade, int pagina,DateTime data)
        {
            ViewBag.PermitirExclusao = User.FindFirst(ClaimTypes.Role).Value == "0" ? false : true;
            List<ComentarioModel> publicacoes = _comentarioService.ListaComentarios(pubId,pagina,quantidade,data);
            List<string> comentariosCarregados = new List<string>();
            foreach (ComentarioModel item in publicacoes)
            {
                comentariosCarregados.Add(await _viewRenderService.RenderToStringAsync(this, "_CompPublicacao", item));
            }

            return Json(comentariosCarregados);
        }


    }
}
