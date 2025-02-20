using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Intranet_NEW.Models.WEB
{
    public class Visitante
    {
        public string NM_VISITANTE { get; set; }

        public string DT_DIA { get; set; }

        public string NR_CPF { get; set; }

        public string NM_MOTIVO { get; set; }

        public int TP_DOCUMENTO { get; set; }
    }
}
