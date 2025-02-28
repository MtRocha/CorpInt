using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Intranet_NEW.Models.WEB
{
    public class Gradu
    {
            public int NR_COLABORADOR { get; set; }
            public string NM_ESCOLARIDADE { get; set; }
            public string NM_DESCRICAO { get; set; }
            public string NM_INSTITUICAO { get; set; }
            public string NM_CURSO { get; set; }
            public DateTime DT_INICIO { get; set; }
            public DateTime DT_CONCLUSAO { get; set; }
    }
}
