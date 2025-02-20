using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Intranet_NEW.Models.RH
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
    }
}
