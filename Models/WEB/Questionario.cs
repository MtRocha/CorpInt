using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Intranet_NEW.Models.WEB
{
    public class Questionario
    {
        public int NR_QUESTAO { get; set; }
        public string NM_QUESTAO { get; set; }
        public int NR_ALTERNATIVA_CORRETA { get; set; }
        public int DT_ATIVACAO { get; set; }
        public string HORA_INICIAL { get; set; }
        public string HORA_FINAL { get; set; }
        public string DT_INCLUSAO { get; set; }

    }

    public class Resposta
    {
        public int NR_QUESTAO { get; set; }
        public int NR_ALTERNATIVA { get; set; }
        public string NM_ALTERNATIVA { get; set; }
    }

}


