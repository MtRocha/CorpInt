namespace Intranet_NEW.Models.WEB
{
    public class ComentarioModel
    {

        public int Id { get; set; }
        public int IdPub { get; set; }
        public int IdUsuario { get; set; }
        public string UsuarioNome { get; set; }
        public string Conteudo { get; set; }
        public DateTime dtComentario { get; set; }



    }
}
