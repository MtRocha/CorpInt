using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Intranet_NEW.Models.WEB
{
    public class Monitoria
    {
        public string ID_OPERADOR { get; set; }

        public string ID_ACIONAMENTO { get; set; }

        public string CPF_CLIENTE { get; set; }

        public DateTime DATA_ACIONAMENTO { get; set; }

        public string DURACAO_ATENDIMENTO { get; set; }

        public string NR_CONTRATO { get; set; }

        public string CD_ACAO { get; set; }

        public string RESULTADO { get; set; }

        public string OBSERVACOES { get; set; }

        public string SUPERVISOR { get; set; }

        public string CALLID { get; set; }

        public string TABULACAO { get; set; }

        public string SUBTABULACAO { get; set; }

        public string NOME_OPERADOR { get; set; }

        public string CD_PRODUTO { get; set; }

        public string NOME_PRODUTO { get; set; }

        public string NOME_MAILING { get; set; }
    }
}
