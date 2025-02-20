using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using Intranet_NEW.Controllers.DAL;

namespace Intranet.BLL.MAI
{
    public class DiscagemMailing
    {
        public DataSet GeraRelatorioMensal(DateTime DT_REFERENCIA)
        {
            try
            {
                int DATA = int.Parse(DT_REFERENCIA.ToString("yyyyMMdd"));

                SqlCommand sqlcommand = new SqlCommand();
                sqlcommand.CommandType = CommandType.StoredProcedure;
                sqlcommand.CommandText = "SP_MAI_RELATORIO";
                sqlcommand.Parameters.AddWithValue("@DT_ARQUIVO", DATA);
                sqlcommand.Parameters.AddWithValue("@TP_RELATORIO", 0);

                DAL_MIS AcessaDadosCaixa = new Intranet.DAL.DAL_MIS();

                DataSet dsMensal = new DataSet();
                dsMensal = AcessaDadosCaixa.ConsultaSQL(sqlcommand);

                return dsMensal;
            }
            catch (Exception ex)
            {
                throw new Exception("MAI.DiscagemMailing_001: " + ex.Message, ex);
            }
        }

        public DataSet GeraRelatorioDiario(DateTime DT_REFERENCIA)
        {
            try
            {
                int DATA = int.Parse(DT_REFERENCIA.ToString("yyyyMMdd"));

                SqlCommand sqlcommand = new SqlCommand();
                sqlcommand.CommandType = CommandType.StoredProcedure;
                sqlcommand.CommandText = "SP_MAI_RELATORIO";
                sqlcommand.Parameters.AddWithValue("@DT_ARQUIVO", DATA);
                sqlcommand.Parameters.AddWithValue("@TP_RELATORIO", 1);

                DAL_MIS AcessaDadosCaixa = new Intranet.DAL.DAL_MIS();

                DataSet dsMensal = new DataSet();
                dsMensal = AcessaDadosCaixa.ConsultaSQL(sqlcommand);

                return dsMensal;
            }
            catch (Exception ex)
            {
                throw new Exception("MAI.DiscagemMailing_002: " + ex.Message, ex);
            }
        }
    }

}
