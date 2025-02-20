using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Intranet_NEW.Models.WEB
{
    public class EmissaoBoleto
    {
        public string NR_BOLETO { get; set; }

        public string DT_IMPORTACAO { get; set; }

        public string DT_ACAO { get; set; }

        public string ID_CLIENTE { get; set; }

        public string ID_CLIENTE_ACAO { get; set; }

        public string NM_CLIENTE { get; set; }

        public string NR_CPF { get; set; }

        public string NR_CONTRATO { get; set; }

        public string DT_VENCIMENTO { get; set; }

        public string DT_PAGAMENTO { get; set; }

        public string NM_EMAIL { get; set; }

        public string NR_DDD { get; set; }

        public string NR_TELEFONE { get; set; }

        public string TP_ENVIO { get; set; }

        public string NM_OBSERVACAO { get; set; }

        public string NR_USUARIO_EMISSAO { get; set; }

        public string DT_EMISSAO { get; set; }
        public string TP_EMISSAO { get; set; }

        public string NM_COLABORADOR { get; set; }

        public string TP_ERRO { get; set; }

        public string NM_PRODUTO { get; set; }

        public string TP_PRODUTO { get; set; }

        public string TP_EMAIL_ERRO { get; set; }

        public string NM_OBSERVACAO_OPERADOR { get; set; }

        public string TP_PAUSA { get; set; }

        public string TP_BOLETO { get; set; }

    }

    public class EmissaoBoleto_EmailInvalido
    {
        public int NR_REGISTRO { get; set; }
        public DateTime DT_REGISTRO { get; set; }
        public string NM_EMAIL { get; set; }
        public int NR_BOLETO { get; set; }
        public int CD_ENVIO_CRM { get; set; }
        public DateTime DT_ENVIO_CRM { get; set; }
        public int NR_USUARIO_EMISSAO { get; set; }

    }
}
