using FluentValidation;
using Intranet_NEW.Models.WEB;
using Intranet_NEW.Services;
using Intranet_NEW.Services.Handlers;
using Intranet_NEW.Services.Validadores;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Options;
using System.Security.Claims;

namespace Intranet_NEW.Controllers
{
    public class PublicacaoController : Controller
    {
        private readonly PublicacaoService _publicacaoService;
        private readonly PublicacaoValidator _publicacaoValidator;
        private readonly CarteiraService _carteiraService;
        private readonly ReacaoService _reacaoService;
        private readonly TipoAcaoService _tipoAcaoService;
        private readonly IViewRenderService _viewRenderService;
        private readonly IUploadFileService _uploadFileService;
        private readonly IHubContext<ReactionHubService> _reactionHubContext;
        private readonly IHubContext<ObjectHubService> _objectHubContext;

        public PublicacaoController(IViewRenderService viewRenderService
                                  , IUploadFileService uploadFileService
                                  , IHubContext<ReactionHubService> reactionHub
                                  , IHubContext<ObjectHubService> objectHub)
        {
            _publicacaoService = new PublicacaoService();
            _publicacaoValidator = new PublicacaoValidator();
            _carteiraService = new CarteiraService();
            _reacaoService = new ReacaoService();
            _tipoAcaoService = new TipoAcaoService();
            _viewRenderService = viewRenderService;
            _uploadFileService = uploadFileService;
            _reactionHubContext = reactionHub;
            _objectHubContext = objectHub;
        }

        private async void CarregarItems()
        {
            List<SelectListItem> carteiras = new List<SelectListItem>();
            List<CarteiraModel> listaCarteiras = _carteiraService.ListaCarteiras();
            List<PublicacaoModel> publicacoes = _publicacaoService.ListaPublicacoes(Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier).Value));
            List<TipoAcaoModel> listaAcoes = _tipoAcaoService.ListaTipoAcao();
            List<SelectListItem> acoes = new();
            List<SelectListItem> acoesFiltro = new();
            acoesFiltro.Add(new SelectListItem("Todas", "0"));
            carteiras.Add(new SelectListItem("Todas as Carteiras", "0"));
            foreach (CarteiraModel carteira in listaCarteiras)
            {
                carteiras.Add(new SelectListItem { Value = carteira.Name, Text = carteira.Name });
            }
            foreach (TipoAcaoModel item in listaAcoes)
            {
                acoes.Add(new SelectListItem { Value = item.Id, Text = item.Name });
                acoesFiltro.Add(new SelectListItem { Value = item.Id, Text = item.Name });
            }
            ViewBag.TipoAcao = acoes;
            ViewBag.TipoAcaoFiltro = acoesFiltro;
            ViewBag.Carteiras = carteiras;
            ViewBag.Publicacoes = publicacoes;
            ViewBag.PermitirExclusao = true;

        }

        [Authorize]
        public IActionResult Publicacao()
        {
            PublicacaoModel model = new();
            CarregarItems();
            return View("Publicacao",model);
        }

        #region Metodos de Assincronos

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Curtir(int Id)
        {
            PublicacaoModel model = _publicacaoService.BuscaPublicacao(Id);
            model.Curtidas += 1;
            if (model == null)
                return BadRequest();

            ReacaoModel reacao = new ();
            reacao.IdReacao = 1;
            reacao.dataReacao = DateTime.Now;
            reacao.Usuario = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            _reacaoService.Reagir(reacao,model);

            await _reactionHubContext.Clients.All.SendAsync("AtualizarCurtida", model.Id, model.Curtidas);

            return Json(new { novaQuantidade = model.Curtidas});
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Descurtir(int Id)
        {
            PublicacaoModel model = _publicacaoService.BuscaPublicacao(Id);
            model.Descurtidas += 1;
            if (model == null)
                return BadRequest();

            ReacaoModel reacao = new();
            reacao.IdReacao = 2;
            reacao.dataReacao = DateTime.Now;
            reacao.Usuario = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier).Value);

             _reacaoService.Reagir(reacao, model);

            await _reactionHubContext.Clients.All.SendAsync("AtualizarDescurtida", model.Id, model.Descurtidas);

            return Json(new { novaQuantidade = model.Descurtidas });
        }


        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest(new { message = "Nenhum arquivo selecionado." });

            var rand = new Random();

            file.FileName.Replace(file.FileName, file.FileName.Replace("mceclip","imgupload" + rand.Next(0,999999)));

            var fileName = await _uploadFileService.UploadFile(file);

            var fileUrl = $"/uploads/{fileName}";
            return Json(new { location = fileUrl });
        }

        [Authorize]
        public async Task<IActionResult> ListaFeed(int quantidade, int pagina, int tipo,DateTime data,string conteudo,string aba)
        {
            ViewBag.PermitirExclusao = aba != "Publicacao" ? false: true;
            List<PublicacaoModel> publicacoes = _publicacaoService.ListaPublicacoesParaFeed(User.FindFirst(ClaimTypes.GroupSid).Value, Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier).Value), pagina,quantidade,data,tipo,conteudo);
            List<string> publicacoesRenderizadas = new List<string>();
            foreach (PublicacaoModel item in publicacoes)
            {
                publicacoesRenderizadas.Add(await _viewRenderService.RenderToStringAsync(this, "_CompPublicacao", item));
            }

            return Json(publicacoesRenderizadas);
        }



        #endregion

        #region CRUD

        [Authorize]
        public IActionResult Publicar(PublicacaoModel model)
        {
            ModelState.Clear();
            var modelstate = _publicacaoValidator.Validate(model, options => { options.IncludeRuleSets("ValidarInsercao"); });

            if (modelstate.IsValid)
            {
                model.DataPublicacao = DateTime.Now;
                model.IdAutor = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier).Value);
                _publicacaoService.InserirPublicacao(model);
                return RedirectToAction("Publicacao", "Publicacao");

            }
            else
            {
                foreach (var erro in modelstate.Errors)
                {
                    ModelState.AddModelError(string.Empty, erro.ErrorMessage);
                }
                CarregarItems();
                ViewBag.ErroPub = true;
                return View("Publicacao",model);
            }
        }

        [Authorize]
        public IActionResult Excluir(int pubId)
        {
            _publicacaoService.ExcluirPublicacao(pubId);
            return RedirectToAction("Publicacao", "Publicacao");
        }
        #endregion
    }
}
