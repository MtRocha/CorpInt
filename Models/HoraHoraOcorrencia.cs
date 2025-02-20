using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Intranet_NEW.Models
{
    public class HoraHoraOcorrencia
    {
        public HoraHoraOcorrencia() { }

        public int NR_USUARIO { get; set; }
        public int ID_OCORRENCIA { get; set; }
        public DateTime DT_OCORRENCIA { get; set; }
        public string HR_OCORRENCIA { get; set; }
        public string TP_DETALHE { get; set; }
        public string TP_OCORRENCIA { get; set; }
        public double NR_QUANTIDADE_TIA { get; set; }
        public double NR_QUANTIDADE_TIA_MONITORADAS { get; set; }
        public string NM_ORGAO_QOA { get; set; }
        public string NM_SISTEMA_CX { get; set; }
        public string TP_SISTEMA_DAO { get; set; }
        public string TP_MOTIVO_EGS { get; set; }
        public string MOTIVO_EAR { get; set; }
        public string NR_USER_INCLUSAO { get; set; }
        public string NM_USER_INCLUSAO { get; set; }
        public DateTime DT_INCLUSAO { get; set; }
        public string NM_USER_ARQUIVAMENTO { get; set; }
        public DateTime? DT_ARQUIVAMENTO { get; set; }
        public string NR_DURACAO_DAO { get; set; }
        public string NR_DURACAO_SIST { get; set; }
        public string TP_ARQUIVADO { get; set; }

    }
}