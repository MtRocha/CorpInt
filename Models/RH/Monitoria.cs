using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Intranet_NEW.Models.RH
{
    public class Monitoria
    {

        public Monitoria() { }

        public DateTime DT_INCLUSAO { get; set; }
        public string EMPRESA { get; set; }
        public string NM_COLABORADOR { get; set; }
        public int CPF_COLABORADOR { get; set; }
        public string MATRICULA_ATENDENTE { get; set; }
        public DateTime DT_ACIONAMENTO { get; set; }
        public DateTime DATA_MONITORIA { get; set; }
        public int CPF_CLIENTE { get; set; }
        public int TEL_ACIONADO { get; set; }
        public string COD_ACAO { get; set; }
        public string COD_RESULTADO_ACAO { get; set; }
        public int DURACAO_ATENDIMENTO { get; set; }
        public string OBS_MONITORIA { get; set; }
        public string TP_ACIONAMENTO { get; set; }
        public int CALLID { get; set; }


    }
}
