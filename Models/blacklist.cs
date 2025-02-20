using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Intranet.DTO
{
    public class dtoblacklist
    {
        public String NM_EMAIL { get; set; }


        public Int64 NR_CPF { get; set; }
        public Int64 NR_FONE { get; set; }
        public Boolean CKWEDOO { get; set; }
        public Boolean CKATTO { get; set; }
        public DateTime DT_ENCERRAMENTO { get; set; }
    }
}