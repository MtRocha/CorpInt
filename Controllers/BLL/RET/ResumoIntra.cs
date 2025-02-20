using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using Intranet_NEW.Controllers.DAL;

namespace Intranet.BLL.RET
{
    public class ResumoIntra
    {
        public DataSet Resumo_Intraday(String ANO, String MES)
        {
            var mesRef = ANO + MES;
            try
            {
                SqlCommand sqlcommand = new SqlCommand();
                sqlcommand.CommandType = CommandType.StoredProcedure;
                sqlcommand.CommandText = "SP_WEB_TI_RESUMO_INTRADAY";
                sqlcommand.Parameters.AddWithValue("@ANO", ANO);
                sqlcommand.Parameters.AddWithValue("@MES", MES);
                sqlcommand.Parameters.AddWithValue("@MESREF", mesRef);
                DAL_MIS AcessaDadosMis = new Intranet.DAL.DAL_MIS();

                DataSet ds = AcessaDadosMis.ConsultaSQL(sqlcommand);
                return ds;
            }
            catch (Exception ex)
            {
               if (ex.Message.Contains("deadlocked")) return Resumo_Intraday(ANO, MES);
                else throw new Exception("RET.ResumoIntraday_001: " + ex.Message, ex);
            }
        }
    }
}
