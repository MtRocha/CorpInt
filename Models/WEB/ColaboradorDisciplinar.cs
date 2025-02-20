namespace Intranet_NEW.Models.WEB
{
    public class ColaboradorDisciplinar
    {
        public string TP_SANCAO { get; set; }
        public string NR_DIA { get; set; }
        public string TP_FALTA { get; set; }
        public string NR_ACAO { get; set; }
        public string DT_APLICACAO { get; set; }
        public string NR_COLABORADOR { get; set; }
        public string NR_RESPONSAVEL { get; set; }
        public string NR_SUPERVISOR { get; set; }
        public string NR_ADVERTENCIA { get; set; }
        public string DT_INFRACAO { get; set; }
        public string NR_MOTIVO { get; set; }
        public byte[] DS_ARQUIVO { get; set; }
        public string TP_EFETIVADO { get; set; }
    }
}