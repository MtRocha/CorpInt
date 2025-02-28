using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Intranet_NEW.Models.RH
{
    public class SolicitacaoJustificativa
    {
        public string NR_SOLICITACAO { get; set; }
        public string DT_SOLICITACAO { get; set; }
        public string NR_SOLICITANTE { get; set; }
        public string NM_SOLICITANTE { get; set; }
        public string DT_MESREF { get; set; }

        public string NM_RECEBIDO_POR { get; set; }
        public string DT_MARCACAO { get; set; }
        public string NM_JUSTIFICATIVA { get; set; }
        public string TP_MOTIVO { get; set; }


        public string NR_SUP_RESPONSAVEL { get; set; }
        public string NM_SUP_RESPONSAVEL { get; set; }
        public string TP_SUP_STATUS { get; set; }
        public string DT_SUP_ANALISE { get; set; }
        public string DS_SUP_OBSERVACAO { get; set; }


        public string NR_GER_RESPONSAVEL { get; set; }
        public string NM_GER_RESPONSAVEL { get; set; }
        public string TP_GER_STATUS { get; set; }
        public string DT_GER_ANALISE { get; set; }
        public string DS_GER_OBSERVACAO { get; set; }


        public string NR_RH_RESPONSAVEL { get; set; }
        public string NM_RH_RESPONSAVEL { get; set; }
        public string TP_RH_STATUS { get; set; }
        public string DT_RH_ANALISE { get; set; }
        public string DS_RH_OBSERVACAO { get; set; }
    }
}