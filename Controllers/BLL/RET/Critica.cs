using Intranet_NEW.Controllers.DAL;
using System;
using System.Data;
using System.Data.SqlClient;

namespace Intranet.BLL.RET
{
    public class Critica
    {
        public DataSet ListaRelatorio(int DT_INI, int DT_FIM, string TP_STATUS, Int64 NR_CPF, string NM_CRITICA)
        {
            try
            {
                SqlCommand sqlcommand = new SqlCommand();
                sqlcommand.CommandType = CommandType.Text;
                sqlcommand.Parameters.AddWithValue("@DT_INI", DT_INI);
                sqlcommand.Parameters.AddWithValue("@DT_FIM", DT_FIM);
                sqlcommand.Parameters.AddWithValue("@TP_STATUS", TP_STATUS);
                sqlcommand.Parameters.AddWithValue("@NR_CPF", NR_CPF);
                sqlcommand.Parameters.AddWithValue("@CRITICA", NM_CRITICA);

                sqlcommand.CommandText = "SELECT ID_OCORRENCIA AS ID, A.NR_CPF, CONCAT(LEFT(A.DT_ACAO,4),'-', SUBSTRING(A.DT_ACAO,5,2),'-',RIGHT(A.DT_ACAO,2)) AS DATA \n"
                                        + ", CONCAT(LEFT(A.HR_ACAO,2),':', SUBSTRING(A.HR_ACAO,3,2),':',RIGHT(A.HR_ACAO,2)) AS HR_ACIONAMENTO \n"
                                        + ", LEFT(B.NM_ACAO,50) AS NM_ACAO, A.DT_PROMESSA, NM_INCORPORACAO, CD_ACAO04, OBSERVACAO, CRITICA, IIF(A.TP_STATUS = 0,'PENDENTE', 'OK') AS STATUS  \n"
                                        + " FROM	TBL_CRITICA_INCORPORACAO A WITH(NOLOCK) \n"
                                        + " LEFT JOIN  DB_PROC..TBL_RET_ACIONAMENTO_ACAO B WITH(NOLOCK) ON A.CD_ACAO = B.CD_ACAO AND B.TP_ACIONAMENTO = 1  \n"
                                        + "WHERE 1=1  \n"
                                        + "  AND CONVERT(VARCHAR(8),A.DT_ACAO,112) BETWEEN @DT_INI AND @DT_FIM \n"
                                        + "  AND ((@NR_CPF = 0 AND NR_CPF = NR_CPF) OR (NR_CPF = @NR_CPF)) \n"
                                        + "  AND ((@TP_STATUS = 9 AND TP_STATUS = TP_STATUS) OR (TP_STATUS = @TP_STATUS)) \n"
                                        + "  AND ((@CRITICA = 'TODOS' AND CRITICA = CRITICA) OR (CRITICA = @CRITICA)) \n"
                                        + "ORDER BY 3,4";

                DAL_MIS AcessaDadosMisN = new Intranet.DAL.DAL_MIS();
                return AcessaDadosMisN.ConsultaSQL(sqlcommand);
            }
            catch (Exception ex)
            {
                throw new Exception("RET.EmissaoBoleto_004: " + ex.Message, ex);
            }
        }

        public int CriticaGravaOcorrencia(int Id, int tp_status, string NM_oBSERVACAO, Int64 NR_USUARIO)
        {
            try
            {
                SqlCommand sqlcommand = new SqlCommand();
                sqlcommand.CommandType = CommandType.Text;
                sqlcommand.Parameters.AddWithValue("@ID", Id);
                sqlcommand.Parameters.AddWithValue("@TP_STATUS", tp_status);
                sqlcommand.Parameters.AddWithValue("@NM_OBSERVACAO", NM_oBSERVACAO);
                sqlcommand.Parameters.AddWithValue("@NR_USUARIO", NR_USUARIO);

                sqlcommand.CommandText = "INSERT INTO TBL_CRITICA_INCORPORACAO_OCORRENCIA \n "
                                       + " VALUES(@ID, GETDATE(), @NM_OBSERVACAO, @TP_STATUS, @NR_USUARIO)";

                DAL_MIS AcessaDadosMisN = new Intranet.DAL.DAL_MIS();
                return AcessaDadosMisN.ExecutaComandoSQL(sqlcommand);
            }
            catch (Exception ex)
            {
                return 0;
                throw new Exception("RET.EmissaoBoleto_004: " + ex.Message, ex);
            }
        }

        public int CriticaAtualizaOcorrencia(int Id, int tp_status, string NM_OBSERVACAO, Int64 NR_USUARIO)
        {
            try
            {
                SqlCommand sqlcommand = new SqlCommand();
                sqlcommand.CommandType = CommandType.Text;
                sqlcommand.Parameters.AddWithValue("@ID", Id);
                sqlcommand.Parameters.AddWithValue("@TP_STATUS", tp_status);
                sqlcommand.Parameters.AddWithValue("@NR_USUARIO", NR_USUARIO);

                sqlcommand.CommandText = " UPDATE TBL_CRITICA_INCORPORACAO \n "
                                       + " SET TP_STATUS = @TP_STATUS \n "
                                       + " , DT_ANALISE = GETDATE() \n "
                                       + " , NR_COLABORADOR = @NR_USUARIO \n "
                                       + " WHERE ID_OCORRENCIA = @ID";

                DAL_MIS AcessaDadosMisN = new Intranet.DAL.DAL_MIS();
                return AcessaDadosMisN.ExecutaComandoSQL(sqlcommand);
            }
            catch (Exception ex)
            {
                return 0;
                throw new Exception("RET.EmissaoBoleto_004: " + ex.Message, ex);
            }
        }

        public DataSet ListaOcorrencia(int ID)
        {
            SqlCommand sqlcommand = new SqlCommand();
            sqlcommand.CommandType = CommandType.Text;

            sqlcommand.CommandText += "SELECT b.NM_COLABORADOR, a.NM_DESCRICAO, a.DT_OCORRENCIA \n";
            sqlcommand.CommandText += "FROM TBL_CRITICA_INCORPORACAO_OCORRENCIA a \n";
            sqlcommand.CommandText += "    INNER JOIN TBL_WEB_COLABORADOR_DADOS b ON a.NR_USUARIO = b.NR_COLABORADOR \n";
            sqlcommand.CommandText += "WHERE ID_OCORRENCIA = @ID \n";

            try
            {
                DAL_MIS AcessaDadosMisN = new Intranet.DAL.DAL_MIS();
                sqlcommand.Parameters.AddWithValue("@ID", ID);
                DataSet ds = AcessaDadosMisN.ConsultaSQL(sqlcommand);

                foreach (DataRow dr in ds.Tables[0].Rows) dr[1] = dr[1].ToString().Replace("\n", "<br>");
                return ds;
            }
            catch (Exception ex)
            {
                throw new Exception("" + ex.Message, ex);
            }
        }

    }
}
