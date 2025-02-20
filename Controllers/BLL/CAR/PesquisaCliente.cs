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
    public class PesquisaCliente
    {
        public DataSet PorCPF(Int64 CPF, String MESREF)
        {

            try
            {
                SqlCommand sqlcommand = new SqlCommand();
                sqlcommand.CommandType = CommandType.StoredProcedure;
                sqlcommand.CommandText = "SP_WEB_REM_PESQUISA_CPF";
                sqlcommand.Parameters.AddWithValue("@NR_CPF", CPF);
                sqlcommand.Parameters.AddWithValue("@MESREF", MESREF);


                DAL_MIS AcessaDadosMis = new Intranet.DAL.DAL_MIS();
                DataSet dsFonePesquisa = AcessaDadosMis.ConsultaSQL(sqlcommand);

                return dsFonePesquisa;
            }
            catch (Exception ex)
            {
                throw new Exception("CAR.PesquisaCliente001: " + ex.Message, ex);
            }
        }
    }
}