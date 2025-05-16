using Intranet_NEW.Models;
using Intranet_NEW.Models.WEB;
using Intranet_NEW.Services;
using Intranet_NEW.Services.Handlers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;
using System.Text.RegularExpressions;
using Tweetinvi.Core.Models;

namespace Intranet_NEW.Controllers
{
    public class MensagensController : Controller
    {
        private readonly MensagemService _mensagemService;
        private readonly CanalService _canalService;
        private readonly IHubContext<ChatHubService> _hubContext;
        private readonly IViewRenderService _viewRenderService;
        private readonly CarteiraService _carteiraService;

        public MensagensController( IHubContext<ChatHubService> hubContext, IViewRenderService viewRenderService)
        {
            _mensagemService = new MensagemService();
            _canalService = new CanalService();
            _hubContext = hubContext;
            _viewRenderService = viewRenderService;
            _carteiraService = new CarteiraService();
        }
        private async void CarregarItems()
        {
            List<SelectListItem> carteiras = new List<SelectListItem>();
            List<CarteiraModel> listaCarteiras = _carteiraService.ListaCarteiras();
            foreach (CarteiraModel carteira in listaCarteiras)
            {
                carteiras.Add(new SelectListItem { Value = carteira.Id, Text = carteira.Name });
            }
            ViewBag.Carteiras = carteiras;
            ViewBag.UsuarioQualidade = 0;
            bool UsuarioQualidade = PerfilModel.Qualidade.Contains(Convert.ToInt32(User.FindFirst(ClaimTypes.PrimaryGroupSid).Value));
            if (UsuarioQualidade)
            {
                ViewBag.UsuarioQualidade = 1;
            }
        }
        [Authorize]
        [HttpGet]
        public IActionResult Chat()
        {
            int tipoAcesso = Convert.ToInt32(User.FindFirst(ClaimTypes.Role).Value);
            int idUsuario = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            int tipoFuncao = PerfilModel.ObterChavePorValor(Convert.ToInt32(User.FindFirst(ClaimTypes.PrimaryGroupSid).Value));
            string carteira= User.FindFirst(ClaimTypes.GroupSid).Value;
            List<CanalModel> models = _canalService.ListarPorFuncao(tipoFuncao,tipoAcesso,idUsuario,carteira);
            CarregarItems();
            return View(models);
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> EnviarMensagem(string grupo, string conteudo, string carteira)
        {
            MensagemModel model = new();

            model.GrupoDestino = grupo;
            model.Destinatario = null;
            model.DataEnvio = DateTime.Now;
            model.Mensagem = conteudo;
            model.IdRemetente = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            model.Remetente = User.FindFirst(ClaimTypes.Name)?.Value;

            // Se canal for Qualidade e carteira veio do filtro, usa ela. Senão, usa a do claim.
            if (grupo == "Qualidade" && !string.IsNullOrEmpty(carteira))
                model.Carteira = carteira;
            else
                model.Carteira = User.FindFirst("CentroCusto")?.Value.Trim();

            _mensagemService.InserirMensagem(model);

            if (!string.IsNullOrEmpty(model.GrupoDestino))
            {
                ReceberMensagem(model);
            }

            return Ok();
        }


        [Authorize]
        public async void ReceberMensagem(MensagemModel model)
        {
            string componente = await _viewRenderService.RenderToStringAsync(this, "_CompMensagem", model);
            await _hubContext.Clients.Group(model.GrupoDestino)
                .SendAsync("Receber", model.GrupoDestino, componente, model);
        }


        [Authorize]
        public async Task<IActionResult> ListarMensagens(string grupo,int quantidade,int pagina,string carteira)
        {
            if (string.IsNullOrEmpty(grupo))
                return BadRequest("Grupo não especificado");

            int gestor = 0;

            var supervisor =  User.FindFirst("Supervisor");
            var coordenador = User.FindFirst("Coordenador");
            if (string.IsNullOrEmpty(carteira))
            {
               carteira = User.FindFirst(ClaimTypes.GroupSid).Value;
            }

            if (grupo == "Operadores")
                gestor =  string.IsNullOrEmpty(supervisor.Value) ? 0 : Convert.ToInt32(supervisor.Value);
            if (grupo == "Coordenador")
                gestor = string.IsNullOrEmpty(coordenador.Value) ? 0 : Convert.ToInt32(coordenador.Value);

            List<MensagemModel> mensagens = _mensagemService.ListarMensagensPorGrupo(grupo,gestor, quantidade, pagina,carteira);

            List<string> mensagensRenderizadas = new List<string>();
            foreach (var item in mensagens)
            {
                mensagensRenderizadas.Add(await _viewRenderService.RenderToStringAsync(this, "_CompMensagem", item));
            }

            int idUsuario =  Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier).Value); 
            Task.Run(() => MarcarMensagensComoLidas(idUsuario,grupo));

            return Json(mensagensRenderizadas);
        }
        [Authorize]
        public async Task MarcarMensagensComoLidas(int idUsuario,string grupoDestino)
        {
            try
            {

                _mensagemService.MarcarComoLida(idUsuario, grupoDestino);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
        }


    }
}
