using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Intranet.DTO.RH
{
    public class Atestado
    {
        public string NR_ATESTADO { get; set; }
        public string NR_COLABORADOR { get; set; }
        public string NM_COLABORADOR { get; set; }
        public string DT_JUSTIFICADA { get; set; }
        public string DT_ENTREGA { get; set; }
        public string NM_OBSERVACAO { get; set; }
        public string TP_CHECKLIST { get; set; }
        public string NR_RECEBIDO_POR { get; set; }
        public string NM_RECEBIDO_POR { get; set; }
        public string NR_RESPONSAVEL_RH { get; set; }
        public string NM_RESPONSAVEL_RH { get; set; }
        public string TP_ATESTADO { get; set; }
        public string TP_ABONO { get; set; }
        public string DS_CID { get; set; }
        public string NM_HOSPITAL { get; set; }
        public string NM_MEDICO { get; set; }
        public string CD_CRM { get; set; }
    }
}
