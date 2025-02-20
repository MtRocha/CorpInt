using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Intranet_NEW.Models.WEB
{
    public class Pendencia
    {
        public string ID_PENDENCIA { get; set; }

        public string NR_TECNICO { get; set; }

        public string TAREFA { get; set; }

        public string DESCRICAO { get; set; }

        public string OBSERVACAO { get; set; }

        public string LOCAL { get; set; }

        public string STATUS { get; set; }

        public string NM_SOLICITANTE { get; set; }

        public string USUARIO_ALTERACAO { get; set; }

        public DateTime DT_ALTERACAO { get; set; }

    }
}
