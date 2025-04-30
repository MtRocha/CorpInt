using Intranet_NEW.Models.WEB;
using Intranet_NEW.Services;
using Intranet_NEW.Services.Handlers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace Intranet_NEW.Controllers
{
    public class MensagensController : Controller
    {
        private readonly MensagemService _mensagemService;
        private readonly IHubContext<ChatHubService> _hubContext;
        private readonly IViewRenderService _viewRenderService;
        public MensagensController(MensagemService mensagemService, IHubContext<ChatHubService> hubContext, IViewRenderService viewRenderService)
        {
            _mensagemService = mensagemService;
            _hubContext = hubContext;
            _viewRenderService = viewRenderService;
        }

        [HttpGet]
        public IActionResult Chat()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> EnviarMensagem([FromBody] MensagemModel mensagem)
        {
            if (!ModelState.IsValid) return BadRequest("Mensagem inválida");

            mensagem.DataEnvio = DateTime.Now;
            _mensagemService.InserirMensagem(mensagem);

            if (!string.IsNullOrEmpty(mensagem.GrupoDestino))
            {
                await _hubContext.Clients.Group(mensagem.GrupoDestino)
                    .SendAsync("Receber", mensagem.Remetente, mensagem.Mensagem);
            }

            return Ok();
        }

        [Authorize]
        public async Task<IActionResult> ListarMensagens(string grupo, int quantidade, int pagina)
        {
            if (string.IsNullOrEmpty(grupo))
                return BadRequest("Grupo não especificado");

            // Busca mensagens diretamente da procedure com paginação aplicada
            List<MensagemModel> mensagens = _mensagemService.ListarMensagensPorGrupo(grupo, quantidade, pagina);

            List<string> mensagensRenderizadas = new List<string>();
            foreach (var item in mensagens)
            {
                mensagensRenderizadas.Add(await _viewRenderService.RenderToStringAsync(this, "_CompMensagem", item));
            }

            return Json(mensagensRenderizadas);
        }

    }
}
