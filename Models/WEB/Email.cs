using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Intranet_NEW.Models.WEB
{
    public class Email
    {
        public class Conta
        {
            public long ID_CONTA { get; set; }
            public string DT_INCLUSAO { get; set; }
            public string NM_CONTA { get; set; }
            public string NM_EMAIL { get; set; }
            public string NM_SENHA { get; set; }
            public string NM_STATUS { get; set; }
            public long NR_USUARIO_INC { get; set; }
            public long NR_USUARIO_ALT { get; set; }
            public string DT_ALTERACAO { get; set; }
        }

        public class Acao
        {
            public long ID_ACAO { get; set; }
            public string DT_INCLUSAO { get; set; }
            public string NM_ACAO { get; set; }
            public long ID_CONTA { get; set; }
            public long ID_LISTA { get; set; }
            public long ID_TEMPLATE { get; set; }
            public string NM_EMAIL_COPIA { get; set; }
            public string DT_DISPARO { get; set; }
            public string HR_DISPARO { get; set; }
            public string NM_DESCRICAO { get; set; }
            public string NM_ASSUNTO { get; set; }
            public string STATUS { get; set; }
            public long NR_USUARIO { get; set; }
        }
    }
}
