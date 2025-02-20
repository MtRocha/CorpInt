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
    public class ColaboradorAbs
    {
        public DataSet GeraAbsDiario(string NR_COORDENADOR, string NR_SUPERVISOR, string NR_EMPRESA, string NR_FILIAl)
        {
            try
            {
                SqlCommand sqlcommand = new SqlCommand();
                sqlcommand.CommandType = CommandType.StoredProcedure;
                sqlcommand.CommandText = "SP_WEB_ABS_COLABORADOR";

                sqlcommand.Parameters.AddWithValue("@NR_COORDENADOR", NR_COORDENADOR);
                sqlcommand.Parameters.AddWithValue("@NR_SUPERVISOR", NR_SUPERVISOR);          

                sqlcommand.Parameters.AddWithValue("@NR_EMPRESA", NR_EMPRESA);
                sqlcommand.Parameters.AddWithValue("@NR_FILIAL", NR_FILIAl);

                DAL_MIS AcessaDadosMisN = new DAL.DAL_MIS();
                return AcessaDadosMisN.ConsultaSQL(sqlcommand);
            }
            catch (Exception ex)
            {
                throw new Exception("WEB.ColaboradorAbs_001: " + ex.Message, ex);
            }
        }
    }
}