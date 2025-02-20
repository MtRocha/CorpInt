using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;
using Intranet_NEW.Controllers.DAL;

namespace Intranet.BLL.FAT
{
    public class Projecao
    {
        DAL_MIS AcessaDadosMis = new Intranet.DAL.DAL_MIS();

        public DataTable GeraEspelhoFaturamentoMensal(DateTime dtReferencia)
        {
            try
            {
                SqlCommand sqlcommand = new SqlCommand();
                sqlcommand.CommandType = CommandType.StoredProcedure;
                sqlcommand.CommandText = "SP_FAT_PROJECAO_FATURAMENTO_MENSAL";
                sqlcommand.Parameters.AddWithValue("@DT_REFERENCIA", dtReferencia.ToString("yyyy-MM-dd HH:mm:ss"));

                DataSet dsFechamento = AcessaDadosMis.ConsultaSQL(sqlcommand);

                DateTime DT_INI = DateTime.MinValue;
                DateTime DT_FIM = new DateTime(dtReferencia.Year, dtReferencia.Month, DateTime.DaysInMonth(dtReferencia.Year, dtReferencia.Month));

                DataTable dtFaturamento;

                decimal NR_CE_AT = 0;
                decimal NR_CPC_AT = 0;
                decimal NR_PP_AT = 0;

                if (dsFechamento.Tables.Count > 0)
                {
                    dtFaturamento = dsFechamento.Tables[0];
                    if (dsFechamento.Tables[0].Rows.Count > 0)
                    {
                        DT_INI = DateTime.Parse(string.Format("{0:####-##-##}", dtFaturamento.AsEnumerable().Max(a => a.Field<decimal>("DT_ACIONAMENTO"))));
                        if (DT_INI == DT_FIM)
                            return dtFaturamento;
                    }
                    else
                        DT_INI = new DateTime(dtReferencia.Year, dtReferencia.Month, 1);
                }

                decimal NR_META_CE_DIARI0 = 12000;
                decimal NR_META_TEMPO_FALANDO_DIARIO = 3600 * 12;
                decimal NR_META_TEMPO_PRODUTI_DIARIO = 3400 * 12;

                int DIA_U = 0;
                int DIA_F = 0;

                while (DT_INI != DateTime.MinValue && DT_INI <= DT_FIM)
                {
                    switch (DT_INI.DayOfWeek)
                    {
                        case DayOfWeek.Monday:
                        case DayOfWeek.Tuesday:
                        case DayOfWeek.Wednesday:
                        case DayOfWeek.Thursday:
                        case DayOfWeek.Friday:
                            {
                                DIA_U++;
                                break;
                            }
                        case DayOfWeek.Saturday:
                            {
                                DIA_F++;
                                break;
                            }
                    }
                    DT_INI = DT_INI.AddDays(1);
                }





                decimal NR_CE = 0;
                decimal NR_CPC = 0; // NR_CE  * 45%
                decimal NR_PP = 0; //  NR_CPC * 60%

                Int64 NR_TEMPO_FALANDO = 0;




                return new DataTable();
            }
            catch (Exception ex)
            {
                throw new Exception("RET.CmdFechamento_001: " + ex.Message, ex);
            }
        }
    }
}