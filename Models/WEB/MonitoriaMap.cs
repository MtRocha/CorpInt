using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsvHelper;

namespace Intranet_NEW.Models.WEB
{
    public class MonitoriasMap : ClassMap<Monitoria>
    {
        public MonitoriasMap()
        {
            Map(m => m.ID_OPERADOR).Name("ID do Operador");
            Map(m => m.ID_ACIONAMENTO).Name("ID do Acionamento");
            Map(m => m.CPF_CLIENTE).Name("CPF/CNPJ do Cliente");
            Map(m => m.DATA_ACIONAMENTO).Name("Data Hora do Acionamento").TypeConverterOption.Format("dd/MM/yyyy HH:mm:ss");
            Map(m => m.DURACAO_ATENDIMENTO).Name("Duração do Acionamento");
            Map(m => m.NR_CONTRATO).Name("Número do Contrato");
            Map(m => m.CD_ACAO).Name("Códigos de Ação e Resultado Informado");
            Map(m => m.RESULTADO).Name("Resultado Apurado - Conforme e Inconforme");
            Map(m => m.OBSERVACOES).Name("Observações");
            Map(m => m.SUPERVISOR).Name("Supervisor (Controle Interno)");
            Map(m => m.CALLID).Name("Call ID (Controle Interno)");
            Map(m => m.TABULACAO).Name("Tabulação (Controle Interno)");
            Map(m => m.SUBTABULACAO).Name("Subtabulação (Controle Interno)");
            Map(m => m.NOME_OPERADOR).Name("Nome do Operador (Controle Interno)");
            Map(m => m.CD_PRODUTO).Name("Código do Produto (Controle Interno)");
            Map(m => m.NOME_PRODUTO).Name("Nome do Produto (Controle Interno)");
            Map(m => m.NOME_MAILING).Name("Nome do Mailing (Controle Interno)");

        }

    }

}
