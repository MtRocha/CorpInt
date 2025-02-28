using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Intranet_NEW.Models.WEB
{
    public class HelpDeskOcorrencia
    {
        public string NR_OCORRENCIA { get; set; }
        public string DT_OCORRENCIA { get; set; }
        public string HR_OCORRENCIA { get; set; }
    
        public string NR_COORDENADOR { get; set; }
        public string NR_SUPERVISOR { get; set; }
        public string NR_OPERADOR { get; set; }
    
        public string TP_PAVIMENTO { get; set; }
        public string NR_PA { get; set; }
        public string NR_IP { get; set; }
        public string NR_TECNICO { get; set; }
        public string DS_PROBLEMA { get; set; }
   
        public string TP_TIPO { get; set; }
        public string TP_DESCRICAO { get; set; }
        public string TP_DEFEITO_HEADSET { get; set; }
        public string NR_HEADSET { get; set; }

        public string TP_RESOLUCAO { get; set; }
        public string NM_OBSERVACAO { get; set; }
        
        public string TP_STATUS { get; set; }
        public string NR_USUARIO_SISTEMA { get; set; }

        public string TP_LOGOUT { get; set; }
    }
}