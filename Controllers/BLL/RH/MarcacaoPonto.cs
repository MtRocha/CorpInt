using Intranet_NEW.Controllers.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;

namespace Intranet.BLL.RH
{
    public class MarcacaoPonto
    {

        public DataSet GravaMarcacaoPonto(string NR_CPF, string TP_MARCACAO, string TP_ENTRADA)
        {
            SqlCommand sqlCommand = new SqlCommand();
            sqlCommand.CommandType = CommandType.Text;
            sqlCommand.CommandText = " INSERT INTO TBL_WEB_RH_MARCACAO_PONTO_INTERNO \n"
                                    + " SELECT NR_CPF, GETDATE(), @TP_MARCACAO, @TP_ENTRADA FROM TBL_WEB_COLABORADOR_DADOS WHERE NR_CPF = @NR_CPF AND TP_STATUS IN (1,5) \n"
                                    + " SELECT NM_COLABORADOR	 FROM TBL_WEB_COLABORADOR_DADOS WHERE NR_CPF = @NR_CPF AND TP_STATUS IN (1,5)";

            try
            {
                sqlCommand.Parameters.AddWithValue("@NR_CPF", NR_CPF);
                sqlCommand.Parameters.AddWithValue("@TP_MARCACAO", TP_MARCACAO);
                sqlCommand.Parameters.AddWithValue("@TP_ENTRADA", TP_ENTRADA);
             
                DAL_MIS AcessaDadosMis = new Intranet.DAL.DAL_MIS();
                return AcessaDadosMis.ConsultaSQL(sqlCommand);
            }
            catch (Exception ex)
            {
                throw new Exception("RH.MarcacaoPonto_001: " + ex.Message, ex);
            }
        }
    }
}