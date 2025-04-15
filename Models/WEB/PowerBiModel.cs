using Tweetinvi.Core.Extensions;

namespace Intranet_NEW.Models.WEB
{
    public class PowerBiModel
    {
        public string Id { get; set; }
        public string Titulo { get; set; }
        public DateTime DtCriacao { get; set; }
        public int idAutor { get; set; }
        public string NomeAutor { get; set; }
        public string Link { get; set; }
        public int TipoAcesso { get; set; }
        public string CaminhoImagem { get; set; }
        public IFormFile Imagem { get; set; }
        public int IntervaloAtualizacao { get; set; }
        public string Descricao { get; set; }
    }
}
