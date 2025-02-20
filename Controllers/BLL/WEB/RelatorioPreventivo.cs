using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;
using Intranet_NEW.Controllers.DAL;

namespace Intranet.BLL.WEB
{
    public class RelatorioPreventivo
    {

        DAL_PROC AcessaBancoProc = new DAL.DAL_PROC();

        public DataSet CarregaListagem(string MesRef)
        {
            try
            {
                SqlCommand sqlcommand = new SqlCommand();
                sqlcommand.CommandType = CommandType.StoredProcedure;
                sqlcommand.CommandText = "SP_WEB_REPETICOES_LIGACAO";
                sqlcommand.Parameters.AddWithValue("@MESREF", MesRef);
                DataSet dsRelatorio = AcessaBancoProc.ConsultaSQL(sqlcommand);

                return dsRelatorio;
            }
            catch (Exception ex)
            {
                throw new Exception("RET.CmdFechamento_001: " + ex.Message, ex);
            }
        }

    }
}
