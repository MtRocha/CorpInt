using Microsoft.AspNetCore.SignalR;

namespace Intranet_NEW.Services.Handlers
{
    public class ReactionHubService : Hub
    {
        #region Publicações 
        public async Task AtualizarCurtida(int publicacaoId, int novaQuantidade)
        {
            await Clients.All.SendAsync("AtualizarCurtida", publicacaoId, novaQuantidade);
        }

        // Descurtida
        public async Task AtualizarDescurtida(int publicacaoId, int novaQuantidade)
        {
            await Clients.All.SendAsync("AtualizarDescurtida", publicacaoId, novaQuantidade);
        }

        public async Task NovoComentario(int publicacaoId, int quantidadeComentarios,string comentarioComponente)
        {
            await Clients.All.SendAsync("NovoComentario", publicacaoId, quantidadeComentarios, comentarioComponente);
        }

        #endregion
    }
}
