using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using Intranet_NEW.Controllers.DAL;

namespace Intranet.BLL.QUE
{
    public class Questionario
    {
        public DataSet ListaQuestionario(DateTime DT_INICIO, DateTime DT_FIM)
        {
            try
            {
                int DataInicio = int.Parse(DT_INICIO.ToString("yyyyMMdd"));
                int DataFim = int.Parse(DT_FIM.ToString("yyyyMMdd"));
            
                SqlCommand sqlcommand = new SqlCommand();
                sqlcommand.CommandType = CommandType.Text;
                sqlcommand.Parameters.AddWithValue("@DT_INIC", DataInicio);
                sqlcommand.Parameters.AddWithValue("@DT_FIM", DataFim);
                sqlcommand.CommandText = "SELECT A.NR_QUESTAO, NM_QUESTAO, DT_ATIVACAO, CONCAT(HORA_INICIAL,' AS ', HORA_FINAL) AS HR_APLICACAO \n";
                sqlcommand.CommandText += ", CASE WHEN ATIVO = 0  THEN 'CANCELADO' \n";
                sqlcommand.CommandText += " WHEN DT_ATIVACAO < CONVERT(CHAR(8),GETDATE(),112) THEN 'FINALIZADO' \n";
                sqlcommand.CommandText += " WHEN DT_ATIVACAO > CONVERT(CHAR(8), GETDATE(), 112) THEN 'AGENDADO' \n";
                sqlcommand.CommandText += " WHEN DT_ATIVACAO = CONVERT(CHAR(8), GETDATE(), 112) AND CONVERT(VARCHAR(5),GETDATE(),108) BETWEEN HORA_INICIAL AND HORA_FINAL THEN 'DISPONIVEL' \n";
                sqlcommand.CommandText += " WHEN DT_ATIVACAO = CONVERT(CHAR(8), GETDATE(), 112) AND CONVERT(VARCHAR(5),GETDATE(),108) < HORA_INICIAL THEN 'AGENDADO' \n";
                sqlcommand.CommandText += " WHEN DT_ATIVACAO = CONVERT(CHAR(8), GETDATE(), 112) AND CONVERT(VARCHAR(5),GETDATE(),108) >= HORA_FINAL  THEN 'FINALIZADO' ELSE 'N.D.' END AS NM_STATUS \n";
                sqlcommand.CommandText += ", B.QTDE_RESP, B.QTDE_CERTA, B.RESULTADO, LEFT(NM_QUESTAO,100) AS RES_QUESTAO  \n";
                sqlcommand.CommandText += " FROM TBL_WEB_QUESTIONARIO A \n";
                sqlcommand.CommandText += " LEFT JOIN (SELECT NR_QUESTAO, COUNT(*) AS QTDE_RESP, SUM(NR_STATUS) AS QTDE_CERTA, (CAST(SUM(NR_STATUS) AS DECIMAL(15,2))/COUNT(*)) * 100 AS RESULTADO FROM TBL_WEB_QUESTIONARIO_RESPOSTA GROUP BY NR_QUESTAO) AS B ON A.NR_QUESTAO=B.NR_QUESTAO \n";
                sqlcommand.CommandText += " WHERE A.DT_ATIVACAO BETWEEN @DT_INIC AND @DT_FIM AND ATIVO = 1";
                sqlcommand.CommandText += " ORDER BY DT_ATIVACAO, A.HORA_INICIAL";

                DAL_TREINAMENTO AcessaDadosTreinamento = new Intranet.DAL.DAL_TREINAMENTO();
                return AcessaDadosTreinamento.ConsultaSQL(sqlcommand);
            }
            catch (Exception ex)
            {
                throw new Exception("BLL.QUE.Questionario_001: " + ex.Message, ex);
            }
        }

        public int GravarQuestao(Intranet_NEW.Models.WEB.Questionario objQuest, DataTable dtResposta, int NR_USUARIO)
        {
            try
            {
                /* Incluir Questao */
                SqlCommand sqlcommand = new SqlCommand();
                sqlcommand.CommandType = CommandType.Text;

                sqlcommand.Parameters.AddWithValue("@NM_QUESTAO", objQuest.NM_QUESTAO);
                sqlcommand.Parameters.AddWithValue("@NR_ALTERNATIVA_CORRETA", objQuest.NR_ALTERNATIVA_CORRETA);
                sqlcommand.Parameters.AddWithValue("@DT_ATIVACAO", objQuest.DT_ATIVACAO);
                sqlcommand.Parameters.AddWithValue("@HORA_INICIAL", objQuest.HORA_INICIAL);
                sqlcommand.Parameters.AddWithValue("@HORA_FINAL", objQuest.HORA_FINAL);

                sqlcommand.Parameters.AddWithValue("@NR_COLABORADOR", NR_USUARIO);

                sqlcommand.CommandText = "INSERT INTO TBL_WEB_QUESTIONARIO(NM_QUESTAO, NR_ALTERNATIVA_CORRETA, DT_ATIVACAO, HORA_INICIAL, HORA_FINAL, NR_COLABORADOR, DT_INCLUSAO, ATIVO ) \n";
                sqlcommand.CommandText += " VALUES (@NM_QUESTAO,@NR_ALTERNATIVA_CORRETA, @DT_ATIVACAO, @HORA_INICIAL, @HORA_FINAL, @NR_COLABORADOR, GETDATE(), 1) \n";
                sqlcommand.CommandText += " \n \n";
                sqlcommand.CommandText += "SELECT MAX(NR_QUESTAO) AS NR_QUESTAO FROM TBL_WEB_QUESTIONARIO";

                DAL_TREINAMENTO AcessaDadosTrein = new DAL.DAL_TREINAMENTO();
                DataSet ds = AcessaDadosTrein.ConsultaSQL(sqlcommand);


                foreach (DataRow dr in dtResposta.Rows)
                {
                    dr["NR_QUESTAO"] = ds.Tables[0].Rows[0][0].ToString();
                    dr["NR_ALTERNATIVA"] = dr["NR_ALTERNATIVA"].ToString().Replace("ABC", ds.Tables[0].Rows[0][0].ToString());
                }

                /* Incluir Reposta */
                AcessaDadosTrein.ExecutaBulkCopySQL(dtResposta, "TBL_WEB_QUESTIONARIO_ALTERNATIVA");

                return int.Parse(ds.Tables[0].Rows[0][0].ToString());

            }
            catch (Exception ex)
            {
                throw new Exception("BLL.QUE.Questionario_002: " + ex.Message, ex);
            }
        }

        public DataSet ListaQuestaoOperador(DateTime DT_INICIO, DateTime DT_FIM, int NR_COLABORADOR)
        {
            try
            {
                int DataInicio = int.Parse(DT_INICIO.ToString("yyyyMMdd"));
                int DataFim = int.Parse(DT_FIM.ToString("yyyyMMdd"));

                SqlCommand sqlcommand = new SqlCommand();
                sqlcommand.CommandType = CommandType.Text;
                sqlcommand.Parameters.AddWithValue("@DT_INIC", DataInicio);
                sqlcommand.Parameters.AddWithValue("@DT_FIM", DataFim);
                sqlcommand.Parameters.AddWithValue("@NR_COLABORADOR", NR_COLABORADOR);
                sqlcommand.CommandText = "SELECT TOP 1 ROW_NUMBER() OVER(ORDER BY A.NR_QUESTAO ASC) AS QUESTAO, A.NR_QUESTAO, A.NM_QUESTAO, A.NR_ALTERNATIVA_CORRETA \n";
                sqlcommand.CommandText += " FROM TBL_WEB_QUESTIONARIO A LEFT JOIN TBL_WEB_QUESTIONARIO_RESPOSTA B ON A.NR_QUESTAO = B.NR_QUESTAO AND B.NR_COLABORADOR = @NR_COLABORADOR \n";
                sqlcommand.CommandText += " WHERE DT_ATIVACAO BETWEEN @DT_INIC AND @DT_FIM \n";
                sqlcommand.CommandText += " AND CONVERT(VARCHAR(5),GETDATE(),108) BETWEEN HORA_INICIAL AND HORA_FINAL \n";
                sqlcommand.CommandText += " AND B.NR_QUESTAO IS NULL \n";
                sqlcommand.CommandText += " AND A.ATIVO = 1 \n";
                sqlcommand.CommandText += "\n \n";
                sqlcommand.CommandText += "SELECT COUNT(*) AS QTDE \n";
                sqlcommand.CommandText += " FROM TBL_WEB_QUESTIONARIO A LEFT JOIN TBL_WEB_QUESTIONARIO_RESPOSTA B ON A.NR_QUESTAO = B.NR_QUESTAO AND B.NR_COLABORADOR = @NR_COLABORADOR \n";
                sqlcommand.CommandText += " WHERE DT_ATIVACAO BETWEEN @DT_INIC AND @DT_FIM \n";
                sqlcommand.CommandText += " AND CONVERT(VARCHAR(5),GETDATE(),103) BETWEEN HORA_INICIAL AND HORA_FINAL \n";
                sqlcommand.CommandText += " AND B.NR_QUESTAO IS NULL \n";
                sqlcommand.CommandText += " AND A.ATIVO = 1 \n";

                DAL_TREINAMENTO AcessaDadosTreinamento = new Intranet.DAL.DAL_TREINAMENTO();
                return AcessaDadosTreinamento.ConsultaSQL(sqlcommand);
            }
            catch (Exception ex)
            {
                throw new Exception("BLL.QUE.Questionario_003: " + ex.Message, ex);
            }
        }

        public DataTable ListaAlternativasOperador(int NR_QUESTAO)
        {
            try
            {
                SqlCommand sqlcommand = new SqlCommand();
                sqlcommand.CommandType = CommandType.Text;
                sqlcommand.Parameters.AddWithValue("@NR_QUESTAO", NR_QUESTAO);
                sqlcommand.CommandText = "SELECT NR_QUESTAO, CASE WHEN NR_ALTERNATIVA=1001 THEN 'A)' WHEN NR_ALTERNATIVA=1002 THEN 'B)' WHEN NR_ALTERNATIVA=1003 THEN 'C)' WHEN NR_ALTERNATIVA=1004 THEN 'D)' ELSE NULL END as NR_ALTERNATIVA, NM_ALTERNATIVA \n";
                sqlcommand.CommandText += " FROM TBL_WEB_QUESTIONARIO_ALTERNATIVA \n";
                sqlcommand.CommandText += " WHERE NR_QUESTAO = @NR_QUESTAO";

                DAL_TREINAMENTO AcessaDadosTreinamento = new Intranet.DAL.DAL_TREINAMENTO();
                return AcessaDadosTreinamento.ConsultaSQL(sqlcommand).Tables[0];
            }
            catch (Exception ex)
            {
                throw new Exception("BLL.QUE.Questionario_004: " + ex.Message, ex);
            }
        }

        public int GravarRepostaOperador(int NR_QUESTAO, int NR_ALTERNATIVA_CORRETA, int NR_ALTERNATIVA_RESP, int NR_COLABORADOR, int NR_SUPERVISOR, int NR_COORDENADOR)
        {
            try
            {
                /* Incluir Questao */
                SqlCommand sqlcommand = new SqlCommand();
                sqlcommand.CommandType = CommandType.Text;

                sqlcommand.Parameters.AddWithValue("@NR_QUESTAO", NR_QUESTAO);
                sqlcommand.Parameters.AddWithValue("@NR_ALTERNATIVA_CORRETA", NR_ALTERNATIVA_CORRETA);
                sqlcommand.Parameters.AddWithValue("@NR_ALTERNATIVA_RESP", NR_ALTERNATIVA_RESP);
                sqlcommand.Parameters.AddWithValue("@NR_COLABORADOR", NR_COLABORADOR);
                sqlcommand.Parameters.AddWithValue("@NR_SUPERVISOR", NR_SUPERVISOR);
                sqlcommand.Parameters.AddWithValue("@NR_COORDENADOR", NR_COORDENADOR);

                sqlcommand.CommandText = "INSERT INTO TBL_WEB_QUESTIONARIO_RESPOSTA(NR_QUESTAO, NR_ALTERNATIVA_CORRETA, NR_ALTERNATIVA_RESP, NR_STATUS, NR_COLABORADOR, NR_SUPERVISOR, NR_COORDENADOR, DT_RESPOSTA) \n";
                sqlcommand.CommandText += " VALUES (@NR_QUESTAO,@NR_ALTERNATIVA_CORRETA,@NR_ALTERNATIVA_RESP, CASE WHEN @NR_ALTERNATIVA_CORRETA = @NR_ALTERNATIVA_RESP THEN 1 ELSE 0 END, @NR_COLABORADOR, @NR_SUPERVISOR, @NR_COORDENADOR, GETDATE()) \n";
                sqlcommand.CommandText += " \n \n";

                DAL_TREINAMENTO AcessaDadosTrein = new DAL.DAL_TREINAMENTO();
                DataSet ds = AcessaDadosTrein.ConsultaSQL(sqlcommand);

                return 1;

            }
            catch (Exception ex)
            {
                throw new Exception("BLL.QUE.Questionario_003: " + ex.Message, ex);
            }
        }

        public DataSet GeraRelatorioResposta(string DT_INI, string DT_FIM, string NR_COORDENADOR, string NR_SUPERVISOR)
        {
            try
            {
                SqlCommand sqlcommand = new SqlCommand();
                sqlcommand.CommandType = CommandType.StoredProcedure;
                sqlcommand.CommandText = "SP_WEB_RESULTADO_QUESTIONARIO";
                sqlcommand.Parameters.AddWithValue("@DT_INI", DT_INI);
                sqlcommand.Parameters.AddWithValue("@DT_FIM", DT_FIM);

                sqlcommand.Parameters.AddWithValue("@NR_COORDENADOR", NR_COORDENADOR);
                sqlcommand.Parameters.AddWithValue("@NR_SUPERVISOR", NR_SUPERVISOR);

                DAL_TREINAMENTO AcessaDadosTrein = new Intranet.DAL.DAL_TREINAMENTO();
                DataSet dsOperador = AcessaDadosTrein.ConsultaSQL(sqlcommand);
                return dsOperador;
            }
            catch (Exception ex)
            {
                throw new Exception("BLL.QUE.Questionario_004: " + ex.Message, ex);
            }
        }

        public DataSet GeraRelatorioAnalitico(string DT_INI, string DT_FIM)
        {
            try
            {
                SqlCommand sqlcommand = new SqlCommand();
                sqlcommand.CommandType = CommandType.StoredProcedure;
                sqlcommand.CommandText = "SP_WEB_RESULTADO_QUESTIONARIO_ANALITICO";
                sqlcommand.Parameters.AddWithValue("@DT_INICIO", DT_INI);
                sqlcommand.Parameters.AddWithValue("@DT_FIM", DT_FIM);

                DAL_TREINAMENTO AcessaDadosTrein = new Intranet.DAL.DAL_TREINAMENTO();
                DataSet dsOperador = AcessaDadosTrein.ConsultaSQL(sqlcommand);
                return dsOperador;
            }
            catch (Exception ex)
            {
                throw new Exception("BLL.QUE.Questionario_009: " + ex.Message, ex);
            }
        }

        public DataSet CarregaQuestionario(int NR_QUESTAO)
        {
            try
            {
                SqlCommand sqlcommand = new SqlCommand();
                sqlcommand.CommandType = CommandType.Text;
                sqlcommand.Parameters.AddWithValue("@NR_QUESTAO", NR_QUESTAO);
                sqlcommand.CommandText = " SELECT *  \n";
                sqlcommand.CommandText += " FROM TBL_WEB_QUESTIONARIO  \n";
                sqlcommand.CommandText += " WHERE NR_QUESTAO = @NR_QUESTAO \n";
                sqlcommand.CommandText += "\n \n";
                sqlcommand.CommandText += "\n \n";
                sqlcommand.CommandText += " SELECT *  \n";
                sqlcommand.CommandText += " FROM TBL_WEB_QUESTIONARIO_ALTERNATIVA  \n";
                sqlcommand.CommandText += " WHERE NR_QUESTAO = @NR_QUESTAO ORDER BY NR_ALTERNATIVA \n";
                sqlcommand.CommandText += "\n ";
                DAL_TREINAMENTO AcessaDadosTreinamento = new Intranet.DAL.DAL_TREINAMENTO();
                return AcessaDadosTreinamento.ConsultaSQL(sqlcommand);
            }
            catch (Exception ex)
            {
                throw new Exception("BLL.QUE.Questionario_005: " + ex.Message, ex);
            }
        }

        public int VerificaConstaResposta(int NR_QUESTAO)
        {
            try
            {
                SqlCommand sqlcommand = new SqlCommand();
                sqlcommand.CommandType = CommandType.Text;
                sqlcommand.Parameters.AddWithValue("@NR_QUESTAO", NR_QUESTAO);
                sqlcommand.CommandText = " SELECT COUNT(*) AS QTDE  \n";
                sqlcommand.CommandText += " FROM TBL_WEB_QUESTIONARIO_RESPOSTA  \n";
                sqlcommand.CommandText += " WHERE NR_QUESTAO = @NR_QUESTAO \n";
                sqlcommand.CommandText += "\n";
                DAL_TREINAMENTO AcessaDadosTreinamento = new Intranet.DAL.DAL_TREINAMENTO();

                DataSet ds = new DataSet();
                ds = AcessaDadosTreinamento.ConsultaSQL(sqlcommand);

                return int.Parse(ds.Tables[0].Rows[0][0].ToString());
            }
            catch (Exception ex)
            {
                throw new Exception("BLL.QUE.Questionario_005: " + ex.Message, ex);
            }
        }

        public int AtualizaQuestionario(int NR_USUARIO, int NR_QUESTAO, string NM_QUESTAO, int NR_ALTERNATIVA_CERTA, int DATA_APLIC, string HR_INIC, string HR_FIM, string RESPOSTA01, string RESPOSTA02, string RESPOSTA03, string RESPOSTA04)
        {
            try
            {
                SqlCommand sqlcommand = new SqlCommand();
                sqlcommand.CommandType = CommandType.Text;
                sqlcommand.Parameters.AddWithValue("@NR_QUESTAO", NR_QUESTAO);
                sqlcommand.Parameters.AddWithValue("@NM_QUESTAO", NM_QUESTAO);
                sqlcommand.Parameters.AddWithValue("@NR_ALTERNATIVA_CERTA", NR_ALTERNATIVA_CERTA);
                sqlcommand.Parameters.AddWithValue("@DATA_APLIC", DATA_APLIC);
                sqlcommand.Parameters.AddWithValue("@HR_INIC", HR_INIC);
                sqlcommand.Parameters.AddWithValue("@HR_FIM", HR_FIM);
                sqlcommand.Parameters.AddWithValue("@NM_RESPOSTA01", RESPOSTA01);
                sqlcommand.Parameters.AddWithValue("@NM_RESPOSTA02", RESPOSTA02);
                sqlcommand.Parameters.AddWithValue("@NM_RESPOSTA03", RESPOSTA03);
                sqlcommand.Parameters.AddWithValue("@NM_RESPOSTA04", RESPOSTA04);

                sqlcommand.Parameters.AddWithValue("@NR_USUARIO", NR_USUARIO);

                sqlcommand.CommandText = " UPDATE TBL_WEB_QUESTIONARIO \n";
                sqlcommand.CommandText += " SET NM_QUESTAO = @NM_QUESTAO, NR_ALTERNATIVA_CORRETA = @NR_ALTERNATIVA_CERTA, DT_ATIVACAO = @DATA_APLIC, \n";
                sqlcommand.CommandText += " HORA_INICIAL = @HR_INIC, HORA_FINAL = @HR_FIM, NR_COLABORADOR_ALT=@NR_USUARIO, DT_ALTERACAO=GETDATE()  \n";
                sqlcommand.CommandText += " WHERE NR_QUESTAO = @NR_QUESTAO \n";
                sqlcommand.CommandText += " \n \n";
                sqlcommand.CommandText += " UPDATE TBL_WEB_QUESTIONARIO_ALTERNATIVA \n";
                sqlcommand.CommandText += " SET NM_ALTERNATIVA = @NM_RESPOSTA01 \n";
                sqlcommand.CommandText += " WHERE NR_QUESTAO = @NR_QUESTAO AND NR_ALTERNATIVA = 1001 \n";
                sqlcommand.CommandText += " \n \n";
                sqlcommand.CommandText += " UPDATE TBL_WEB_QUESTIONARIO_ALTERNATIVA \n";
                sqlcommand.CommandText += " SET NM_ALTERNATIVA = @NM_RESPOSTA02 \n";
                sqlcommand.CommandText += " WHERE NR_QUESTAO = @NR_QUESTAO AND NR_ALTERNATIVA = 1002 \n";
                sqlcommand.CommandText += " \n \n";
                sqlcommand.CommandText += " UPDATE TBL_WEB_QUESTIONARIO_ALTERNATIVA \n";
                sqlcommand.CommandText += " SET NM_ALTERNATIVA = @NM_RESPOSTA03 \n";
                sqlcommand.CommandText += " WHERE NR_QUESTAO = @NR_QUESTAO AND NR_ALTERNATIVA = 1003 \n";
                sqlcommand.CommandText += " \n \n";
                sqlcommand.CommandText += " UPDATE TBL_WEB_QUESTIONARIO_ALTERNATIVA \n";
                sqlcommand.CommandText += " SET NM_ALTERNATIVA = @NM_RESPOSTA04 \n";
                sqlcommand.CommandText += " WHERE NR_QUESTAO = @NR_QUESTAO AND NR_ALTERNATIVA = 1004 \n";
                sqlcommand.CommandText += " \n \n";
                DAL_TREINAMENTO AcessaDadosTreinamento = new Intranet.DAL.DAL_TREINAMENTO();

                AcessaDadosTreinamento.ExecutaComandoSQL(sqlcommand);

                return 1;
            }
            catch (Exception ex)
            {
                throw new Exception("BLL.QUE.Questionario_006: " + ex.Message, ex);
            }
        }

        public int DesativarQuestao(int NR_USUARIO, int NR_QUESTAO)
        {
            try
            {
                SqlCommand sqlcommand = new SqlCommand();
                sqlcommand.CommandType = CommandType.Text;
                sqlcommand.Parameters.AddWithValue("@NR_QUESTAO", NR_QUESTAO);
                sqlcommand.Parameters.AddWithValue("@NR_USUARIO", NR_USUARIO);

                sqlcommand.CommandText = " UPDATE TBL_WEB_QUESTIONARIO \n";
                sqlcommand.CommandText += " SET ATIVO = 0, NR_COLABORADOR_ALT=@NR_USUARIO, DT_ALTERACAO=GETDATE()  \n";
                sqlcommand.CommandText += " WHERE NR_QUESTAO = @NR_QUESTAO \n";
                sqlcommand.CommandText += " \n \n";
                DAL_TREINAMENTO AcessaDadosTreinamento = new Intranet.DAL.DAL_TREINAMENTO();

                AcessaDadosTreinamento.ExecutaComandoSQL(sqlcommand);

                return 1;
            }
            catch (Exception ex)
            {
                throw new Exception("BLL.QUE.Questionario_007: " + ex.Message, ex);
            }
        }

    }
}
