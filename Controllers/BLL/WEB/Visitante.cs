using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using Intranet_NEW.Controllers.DAL;

namespace Intranet.BLL.WEB
{
    public class Visitante
    {
        //private object DT_SAIDA;
        public DataSet ListaVisitante(DateTime DT_INICIO, DateTime DT_FIM)
        {
            try
            {
                SqlCommand sqlcommand = new SqlCommand();
                sqlcommand.CommandType = CommandType.StoredProcedure;
                sqlcommand.Parameters.AddWithValue("@DT_INI", DT_INICIO);
                sqlcommand.Parameters.AddWithValue("@DT_FIM", DT_FIM);
                sqlcommand.CommandText = "SP_WEB_LISTA_VISITANTE";

                DAL_MIS AcessaDadosMis = new DAL.DAL_MIS();
                return AcessaDadosMis.ConsultaSQL(sqlcommand);
            }
            catch (Exception ex)
            {
                throw new Exception("BLL.WEB.Visitante_001: " + ex.Message, ex);
            }
        }
        public int GravarVisitante(string NM_VISITANTE, DateTime DT_ENTRADA, string NR_CPF, int TP_DOC)
        {
            try
            {
                SqlCommand sqlcommand = new SqlCommand();
                sqlcommand.CommandType = CommandType.Text;
                sqlcommand.Parameters.AddWithValue("@NM_VISITANTE", NM_VISITANTE);
                sqlcommand.Parameters.AddWithValue("@DT_DIA", DT_ENTRADA);
                sqlcommand.Parameters.AddWithValue("@NR_CPF", NR_CPF);
                sqlcommand.Parameters.AddWithValue("@TP_DOC", TP_DOC);
                
                sqlcommand.CommandText = "INSERT INTO TBL_WEB_CONTROLE_VISITANTE \n"
                                        + "        (NM_VISITANTE, DT_ENTRADA, DT_SAIDA, NR_CPF, NM_RESPONSAVEL, NM_EMPRESA, TP_DOCUMENTO) \n"
                                        + " VALUES (@NM_VISITANTE, @DT_DIA, GETDATE(), @NR_CPF, 'RH - ENTREVISTA', 'ENTREVISTA', @TP_DOC)  \n";

                DAL_MIS AcessaDadosMis = new DAL.DAL_MIS();
                return AcessaDadosMis.ExecutaComandoSQL(sqlcommand);
            }
            catch (Exception ex)
            {
                throw new Exception("BLL.WEB.Visitante_002: " + ex.Message, ex);
            }
        }
    }
}
