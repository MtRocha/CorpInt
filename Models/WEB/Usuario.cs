using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Intranet_NEW.Models.WEB
{
    public class Usuario
    {
        public int NR_USUARIO { get; set; }
        public string NM_USUARIO { get; set; }
        public string NM_EMAIL { get; set; }
        public decimal NR_RAMAL { get; set; }
        public string NM_LOGIN { get; set; }
        public string NM_SENHA { get; set; }
        public int TP_ATIVO { get; set; }
        public int NR_USUARIO_INCLUSAO { get; set; }
        public System.DateTime DT_ULTIMA_ALTERACAO { get; set; }
        public int NR_USUARIO_ALTERACAO { get; set; }
        public List<string[]> TP_ACESSO_USUARIO { get; set; }
    }
}