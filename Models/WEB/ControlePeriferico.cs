using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Intranet_NEW.Models.WEB
{
    public class ControlePeriferico
    {
        public string NR_NOTA { get; set; }
        public string DT_NOTA { get; set; }
        public string TP_NOTA { get; set; }
        public string NR_RESPONSAVEL { get; set; }

        public DataTable DT_ITEMS { get; set; }
    }
}