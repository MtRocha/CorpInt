namespace Intranet_NEW.Models.WEB
{
    public class CanalModel
    {
        public int IdCanal { get; set; }
        public string Nome { get; set; }
        public int TipoAcesso { get; set; }
        public int TipoFuncao { get; set; }
        public MensagemModel UltimaMensagem { get; set; }
        public string PeriodoMensagem { get; set; }
        public int QtdMensagens { get; set; }
        public string? NomeExibicao { get; internal set; }
    }
}
