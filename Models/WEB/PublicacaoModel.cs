namespace Intranet_NEW.Models.WEB
{
    public class PublicacaoModel
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public string Conteudo { get; set; }
        public int Curtidas { get; set; }
        public int Descurtidas { get; set; }
        public string Carteira { get; set; }
        public int Tipo { get; set; }
        public string Autor { get; set; }
        public int IdAutor { get; set; }
        public int Arquivada { get; set; }
        public DateTime DataPublicacao { get; set; }
        public bool FoiReagido { get; set; }
        public int TipoReacao { get; set; }
        public int QuantidadeComentario { get; set; }



    }
}
