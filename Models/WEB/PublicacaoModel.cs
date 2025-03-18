namespace Intranet_NEW.Models.WEB
{
    public class PublicacaoModel
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public string Conteudo { get; set; }
        public int Curtidas { get; set; }
        public int Descurtidas { get; set; }
        public CarteiraModel Carteira { get; set; }
    }
}
