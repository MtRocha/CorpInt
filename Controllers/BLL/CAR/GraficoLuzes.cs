using Intranet_NEW.Controllers.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Intranet.BLL.CAR
{
    public class GraficoLuzes
    {
        public DataSet GeraRelatorioGraficoLuzes(string DT_REFERENCIA)
        {
            try
            {
                SqlCommand sqlcommand = new SqlCommand();
                sqlcommand.CommandType = CommandType.StoredProcedure;
                sqlcommand.CommandText = "SP_WEB_REM_GRAFICO";
                sqlcommand.Parameters.AddWithValue("@MESREF", DT_REFERENCIA.ToString());

                DAL_MIS AcessaDadosCaixa = new Intranet.DAL.DAL_MIS();

                DataSet dsFechamento = AcessaDadosCaixa.ConsultaSQL(sqlcommand);

                return dsFechamento;
            }
            catch (Exception ex)
            {
                throw new Exception("REM.cmdGrafico: " + ex.Message, ex);
            }
        }

    }
}