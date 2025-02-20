using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;
using Intranet_NEW.Controllers.DAL;


namespace Intranet.BLL.WEB
{
    public class PausaEstendida
    {
        #region Dados Pausa Estendida

        public DataSet ListaPausaEstendida(DateTime DT_INI, DateTime DT_FIM)
        {
            try
            {
                SqlCommand sqlcommand = new SqlCommand();
                sqlcommand.CommandType = CommandType.StoredProcedure;
                sqlcommand.Parameters.AddWithValue("@DATA_INIC", int.Parse(DT_INI.ToString("yyyyMMdd")));
                sqlcommand.Parameters.AddWithValue("@DATA_FIM", int.Parse(DT_FIM.ToString("yyyyMMdd")));

                sqlcommand.CommandText = "SP_WEB_PAUSA_ESTENDIDA";

                DAL_MIS AcessaDadosMis = new DAL.DAL_MIS();
                return AcessaDadosMis.ConsultaSQL(sqlcommand);
            }
            catch (Exception ex)
            {
                throw new Exception("BLL.WEB.PausaEstendida_001: " + ex.Message, ex);
            }
        }

        #endregion
    }
}