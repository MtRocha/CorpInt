namespace Intranet_NEW.Models.WEB
{
    public class MensagemModel
    {
        public int Id { get; set; }
        public int Remetente { get; set; }
        public int? Destinatario { get; set; }
        public string? GrupoDestino { get; set; }
        public string Mensagem { get; set; } = string.Empty;
        public DateTime DataEnvio { get; set; }
        public DateTime? DataVisualizado { get; internal set; }
        public int Lida { get; internal set; }
        public int Excluida { get; internal set; }
    }

}
