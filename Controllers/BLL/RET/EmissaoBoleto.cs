using Intranet_NEW.Controllers.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Intranet.BLL.RET
{
    public class EmissaoBoleto
    {
        public DataSet CIWEB()
        {
            try
            {
                SqlCommand sqlcommand = new SqlCommand();
                sqlcommand.CommandType = CommandType.StoredProcedure;
                sqlcommand.CommandText = "SP_WEB_BOLETO_LISTA_EMISSAO_CIWEB";
                
                DAL_MIS AcessaDadosMis = new Intranet.DAL.DAL_MIS();

                DataSet ds = AcessaDadosMis.ConsultaSQL(sqlcommand);
                return ds;
            }
            catch (Exception ex)
            {
                throw new Exception("RET.EmissaoBoleto_001: " + ex.Message, ex);
            }
        }

        public DataSet GeraBoletoListaEmissao(int DT_INI, int DT_FIM, string PRODUTO, Int64 CPF, Int64 PAUSA, int ENVIO, string CANAL)
        {
            try
            {
                SqlCommand sqlcommand = new SqlCommand();
                sqlcommand.CommandType = CommandType.StoredProcedure;
                sqlcommand.CommandText = "SP_WEB_BOLETO_LISTA_EMISSAO";
                sqlcommand.Parameters.AddWithValue("@DT_INI", DT_INI);
                sqlcommand.Parameters.AddWithValue("@DT_FIM", DT_FIM);
                sqlcommand.Parameters.AddWithValue("@PRODUTO", PRODUTO); 
                sqlcommand.Parameters.AddWithValue("@CPF", CPF);
                sqlcommand.Parameters.AddWithValue("@PAUSA", PAUSA);
                sqlcommand.Parameters.AddWithValue("@ENVIO", ENVIO);
                sqlcommand.Parameters.AddWithValue("@CANAL", CANAL);

                DAL_MIS AcessaDadosMis = new Intranet.DAL.DAL_MIS();

                DataSet ds = AcessaDadosMis.ConsultaSQL(sqlcommand);
                return ds;
            }
            catch (Exception ex)
            {
                throw new Exception("RET.EmissaoBoleto_001: " + ex.Message, ex);
            }
        }

        public DataSet GeraBoletoListaEmitidosSim(int DT_INI, int DT_FIM)
        {
            try
            {
                SqlCommand sqlcommand = new SqlCommand();
                sqlcommand.CommandType = CommandType.StoredProcedure;
                sqlcommand.CommandText = "SP_WEB_BOLETO_LISTA_EMITIDOS_SIM";
                sqlcommand.Parameters.AddWithValue("@DT_INI", DT_INI);
                sqlcommand.Parameters.AddWithValue("@DT_FIM", DT_FIM);

                DAL_MIS AcessaDadosMis = new Intranet.DAL.DAL_MIS();

                DataSet ds = AcessaDadosMis.ConsultaSQL(sqlcommand);
                return ds;
            }
            catch (Exception ex)
            {
                throw new Exception("RET.EmissaoBoleto_002: " + ex.Message, ex);
            }
        }

        public DataSet GeraBoletoListaEmitidosNao(int DT_INI, int DT_FIM, int TP_ERRO)
        {
            try
            {
                SqlCommand sqlcommand = new SqlCommand();
                sqlcommand.CommandType = CommandType.StoredProcedure;
                sqlcommand.CommandText = "SP_WEB_BOLETO_LISTA_EMITIDOS_NAO";
                sqlcommand.Parameters.AddWithValue("@DT_INI", DT_INI);
                sqlcommand.Parameters.AddWithValue("@DT_FIM", DT_FIM);
                sqlcommand.Parameters.AddWithValue("@TP_ERRO", TP_ERRO);

                DAL_MIS AcessaDadosMis = new Intranet.DAL.DAL_MIS();

                DataSet ds = AcessaDadosMis.ConsultaSQL(sqlcommand);
                return ds;
            }
            catch (Exception ex)
            {
                throw new Exception("RET.EmissaoBoleto_002: " + ex.Message, ex);
            }
        }

        public DataSet GeraBoletoListaResumo(int DT_INI, int DT_FIM)
        {
            try
            {
                SqlCommand sqlcommand = new SqlCommand();
                sqlcommand.CommandType = CommandType.StoredProcedure;
                sqlcommand.CommandText = "SP_WEB_BOLETO_LISTA_RESUMO";
                sqlcommand.Parameters.AddWithValue("@DT_INI", DT_INI);
                sqlcommand.Parameters.AddWithValue("@DT_FIM", DT_FIM);

                DAL_MIS AcessaDadosMis = new Intranet.DAL.DAL_MIS();

                DataSet ds = AcessaDadosMis.ConsultaSQL(sqlcommand);
                return ds;
            }
            catch (Exception ex)
            {
                throw new Exception("RET.EmissaoBoleto_003: " + ex.Message, ex);
            }
        }

        public DataSet GeraBoletoListaProdutividade(int DT_INI, int DT_FIM, string PRODUTO)
        {
            try
            {
                SqlCommand sqlcommand = new SqlCommand();
                sqlcommand.CommandType = CommandType.StoredProcedure;
                sqlcommand.CommandText = "SP_WEB_BOLETO_LISTA_PRODUTIVIDADE";
                sqlcommand.Parameters.AddWithValue("@DT_INI", DT_INI);
                sqlcommand.Parameters.AddWithValue("@DT_FIM", DT_FIM);
                sqlcommand.Parameters.AddWithValue("@PRODUTO", PRODUTO);

                DAL_MIS AcessaDadosMis = new Intranet.DAL.DAL_MIS();

                DataSet ds = AcessaDadosMis.ConsultaSQL(sqlcommand);
                return ds;
            }
            catch (Exception ex)
            {
                throw new Exception("RET.EmissaoBoleto_003: " + ex.Message, ex);
            }
        }

        public DataSet GeraStatusRobo()
        {
            try
            {
                SqlCommand sqlcommand = new SqlCommand();
                sqlcommand.CommandType = CommandType.StoredProcedure;
                sqlcommand.CommandText = "SP_WEB_ROBO_ACOMPANHAMENTO";

                DAL_MIS AcessaDadosMis = new Intranet.DAL.DAL_MIS();

                DataSet ds = AcessaDadosMis.ConsultaSQL(sqlcommand);
                return ds;
            }
            catch (Exception ex)
            {
                throw new Exception("RET.EmissaoBoleto_006: " + ex.Message, ex);
            }
        }

        public Intranet_NEW.Models.WEB.EmissaoBoleto DetalheBoleto(int NR_BOLETO, int NR_COLABORADOR)
        {
            try
            {
                SqlCommand sqlcommand = new SqlCommand();
                sqlcommand.CommandType = CommandType.Text;
                sqlcommand.CommandText = " SELECT A.*, B.NM_COLABORADOR, C.NM_PRODUTO, C.TP_PRODUTO \n"
                                        + "FROM TBL_WEB_EMISSAO_BOLETO A WITH(NOLOCK) \n"
                                        + " LEFT JOIN TBL_WEB_COLABORADOR_DADOS B WITH(NOLOCK) ON A.NR_USUARIO_EMISSAO = B.NR_COLABORADOR \n"
                                        + " LEFT JOIN TBL_REM_PRODUTO_CAIXA     C WITH(NOLOCK) ON (A.CD_OPERACAO       = C.NR_PRODUTO) \n"
                                        + " WHERE NR_BOLETO = @NR_BOLETO ";

                if (NR_COLABORADOR != 0)
                {
                    sqlcommand.CommandText += "\n\n UPDATE TBL_WEB_EMISSAO_BOLETO SET NR_USUARIO_EMISSAO = @NR_COLABORADOR, DT_EMISSAO = GETDATE() WHERE  NR_USUARIO_EMISSAO IS NULL AND NR_BOLETO = @NR_BOLETO \n";
                    sqlcommand.Parameters.AddWithValue("@NR_COLABORADOR", NR_COLABORADOR);
                }

                sqlcommand.Parameters.AddWithValue("@NR_BOLETO", NR_BOLETO);

                DAL_MIS AcessaDadosMis = new Intranet.DAL.DAL_MIS();

                DataSet ds = AcessaDadosMis.ConsultaSQL(sqlcommand);
                Intranet_NEW.Models.WEB.EmissaoBoleto dto = new DTO.WEB.EmissaoBoleto();
                if ((ds.Tables.Count > 0) && (ds.Tables[0].Rows.Count > 0))
                {
                    DataRow dr = ds.Tables[0].Rows[0];
                    dto.NR_BOLETO = dr["NR_BOLETO"].ToString();

                    dto.DT_ACAO = dr["DT_ACAO"] != DBNull.Value ? DateTime.Parse(string.Format("{0:####/##/##}", decimal.Parse(dr["DT_ACAO"].ToString()))).ToString("dd/MM/yyyy") : "";

                    dto.NM_CLIENTE = dr["NM_CLIENTE"].ToString();
                    dto.NR_CPF = dr["NR_CPF"] != DBNull.Value ? string.Format(@"{0:000\.000\.000\-00}", ((Int64)dr["NR_CPF"])) : "";

                    dto.NR_CONTRATO = ds.Tables[0].Rows[0]["NR_CONTRATO"].ToString();

                    dto.DT_VENCIMENTO = dr["DT_VENCIMENTO"] != DBNull.Value ? DateTime.Parse(string.Format("{0:####/##/##}", decimal.Parse(dr["DT_VENCIMENTO"].ToString()))).ToString("dd/MM/yyyy") : "";
                    dto.DT_PAGAMENTO = dr["DT_PAGAMENTO"] != DBNull.Value ? DateTime.Parse(string.Format("{0:####/##/##}", decimal.Parse(dr["DT_PAGAMENTO"].ToString()))).ToString("dd/MM/yyyy") : "";

                    dto.NM_EMAIL = ds.Tables[0].Rows[0]["NM_EMAIL"].ToString();

                    dto.NM_OBSERVACAO_OPERADOR = ds.Tables[0].Rows[0]["NM_OBSERVACAO_OPERADOR"].ToString();

                    dto.NR_USUARIO_EMISSAO = dr["NR_USUARIO_EMISSAO"].ToString();

                    dto.NR_TELEFONE = dr["NR_DDD"].ToString() + " " + dr["NR_TELEFONE"].ToString();
                    dto.TP_ENVIO = dr["TP_ENVIO"].ToString();
                    dto.NM_OBSERVACAO = dr["NM_OBSERVACAO"].ToString();
                    dto.TP_ERRO = dr["TP_ERRO"].ToString() != "" ? dr["TP_ERRO"].ToString() : "1";
                    dto.DT_EMISSAO = dr["DT_EMISSAO"] != DBNull.Value ? string.Format("Boleto emitido em: {0}", ((DateTime)dr["DT_EMISSAO"]).ToString("dd/MM/yyyy HH:mm:ss")) : "";
                    dto.NM_COLABORADOR = dr["NM_COLABORADOR"] != DBNull.Value ? string.Format("<br>Responsável pela emissão: <b>{0}</b>", dr["NM_COLABORADOR"].ToString().ToUpper()) : "";

                    if (dr["TP_EMISSAO"].ToString() == "0")
                    {
                        dto.DT_EMISSAO = dr["DT_EMISSAO"] != DBNull.Value ? string.Format("Solicitação atendida em: {0}", ((DateTime)dr["DT_EMISSAO"]).ToString("dd/MM/yyyy HH:mm:ss")) : "";
                        dto.TP_EMAIL_ERRO = dr["TP_EMAIL_ERRO"].ToString() == "1" ? "<br>E-mail de notificação de não emissão de boleto enviado com sucesso." : "<br>E-mail de notificação de não emissão de boleto não enviado para o cliente.";
                    }

                    dto.NM_PRODUTO = dr["NM_PRODUTO"].ToString();
                    dto.TP_PRODUTO = dr["TP_PRODUTO"].ToString();
                    dto.TP_PAUSA = dr["TP_PAUSA"].ToString() == "1" ? "SIM" : "NAO";


                    /* TIPO DE ENVIO */
                    switch (dr["ID_ACAO"].ToString())
                    {
                        case "55": dto.TP_ENVIO = "N"; break;
                        case "69": dto.TP_ENVIO = "R"; break;
                        case "99": dto.TP_ENVIO = "P"; break;
                        case "88": dto.TP_ENVIO = "I1"; break;
                        case "77": dto.TP_ENVIO = "I2"; break;
                        case "11": dto.TP_ENVIO = "153"; break;
                        case "50": dto.TP_ENVIO = "PENDENTE";break;
                    }

                    
                    //dto.TP_ENVIO = dr["TP_ENVIO"].ToString() == "1" ? "N" : "R";

                    string TP_BOLETO = "";
                    dto.TP_BOLETO = dr["TP_BOLETO"].ToString() == "0" ? "TODAS" : dr["TP_BOLETO"].ToString();
                    dto.TP_BOLETO = dr["TP_BOLETO"].ToString() == "5" ? "WPP PENDENTE" : dr["TP_BOLETO"].ToString();
                    if (dr["TP_BOLETO"].ToString() == "5") { TP_BOLETO = "WPP PENDENTE"; };
                    if (dr["TP_BOLETO"].ToString() == "1") { TP_BOLETO = "UNICA"; } ;
                    if (dr["TP_BOLETO"].ToString() == "0") { TP_BOLETO = "TODAS"; } ;
                    if (dr["TP_BOLETO"].ToString() != "0" && dr["TP_BOLETO"].ToString() != "1") { TP_BOLETO = dr["TP_BOLETO"].ToString(); };

                    dto.TP_BOLETO = TP_BOLETO.ToString();

                }

                return dto;
            }
            catch (Exception ex)
            {
                throw new Exception("RET.EmissaoBoleto_004: " + ex.Message, ex);
            }
        }

        public int GravaBoleto(Intranet_NEW.Models.WEB.EmissaoBoleto dto)
        {
            try
            {
                SqlCommand sqlcommand = new SqlCommand();
                sqlcommand.CommandType = CommandType.Text;
                sqlcommand.CommandText = "UPDATE TBL_WEB_EMISSAO_BOLETO SET "
                                        + " NM_OBSERVACAO		  = @NM_OBSERVACAO "
                                        + " , NR_USUARIO_EMISSAO  = @NR_USUARIO "
                                        + " , DT_EMISSAO		  = GETDATE() "
                                        + " , TP_EMISSAO		  = @TP_EMISSAO "
                                        + " , TP_ERRO		      = @TP_ERRO "
                                        + " , TP_EMAIL_ERRO		  = @TP_EMAIL_ERRO "
                                        + " , TP_PENDENTE_EMISSAO = '0' "
                                        + " WHERE NR_BOLETO = @NR_BOLETO";

                sqlcommand.Parameters.AddWithValue("@NR_BOLETO", dto.NR_BOLETO);
                sqlcommand.Parameters.AddWithValue("@NM_OBSERVACAO", dto.NM_OBSERVACAO);
                sqlcommand.Parameters.AddWithValue("@NR_USUARIO", dto.NR_USUARIO_EMISSAO);
                sqlcommand.Parameters.AddWithValue("@TP_EMISSAO", dto.TP_EMISSAO);
                sqlcommand.Parameters.AddWithValue("@TP_ERRO", dto.TP_ERRO);
                sqlcommand.Parameters.AddWithValue("@TP_EMAIL_ERRO", "0");

                DAL_MIS AcessaDadosMis = new Intranet.DAL.DAL_MIS();

                return AcessaDadosMis.ExecutaComandoSQL(sqlcommand);
            }
            catch (Exception ex)
            {
                throw new Exception("RET.EmissaoBoleto_005: " + ex.Message, ex);
            }
        }

        public int GravaBoletoPendente(Intranet_NEW.Models.WEB.EmissaoBoleto dto)
        {
            try
            {
                SqlCommand sqlcommand = new SqlCommand();
                sqlcommand.CommandType = CommandType.Text;
                sqlcommand.CommandText = "UPDATE TBL_WEB_EMISSAO_BOLETO SET "
                                        + "   NM_OBSERVACAO		  = @NM_OBSERVACAO "
                                        + " , NR_USUARIO_EMISSAO  = NULL "
                                        + " , TP_PENDENTE_EMISSAO = '1' "
                                        + " , DT_EMISSAO  = NULL \n"
                                        + " WHERE NR_BOLETO = @NR_BOLETO";

                sqlcommand.Parameters.AddWithValue("@NR_BOLETO", dto.NR_BOLETO);
                sqlcommand.Parameters.AddWithValue("@NM_OBSERVACAO", dto.NM_OBSERVACAO);

                DAL_MIS AcessaDadosMis = new Intranet.DAL.DAL_MIS();

                return AcessaDadosMis.ExecutaComandoSQL(sqlcommand);
            }
            catch (Exception ex)
            {
                throw new Exception("RET.EmissaoBoleto_005: " + ex.Message, ex);
            }
        }

        public int DesmarcaBoleto(string NR_BOLETO)
        {
            try
            {
                SqlCommand sqlcommand = new SqlCommand();
                sqlcommand.CommandType = CommandType.Text;
                sqlcommand.CommandText = "UPDATE TBL_WEB_EMISSAO_BOLETO SET \n"
                                        + "  NR_USUARIO_EMISSAO  = NULL \n"
                                        + ", DT_EMISSAO          = NULL \n"
                                        + " WHERE NR_BOLETO = @NR_BOLETO";

                sqlcommand.Parameters.AddWithValue("@NR_BOLETO", NR_BOLETO);

                DAL_MIS AcessaDadosMis = new Intranet.DAL.DAL_MIS();

                return AcessaDadosMis.ExecutaComandoSQL(sqlcommand);
            }
            catch (Exception ex)
            {
                throw new Exception("RET.EmissaoBoleto_005: " + ex.Message, ex);
            }
        }

        public DataTable ListaMotivoNaoEmissao()
        {
            try
            {
                SqlCommand sqlcommand = new SqlCommand();
                sqlcommand.CommandType = CommandType.Text;
                sqlcommand.CommandText = "SELECT CD_MOTIVO, DS_MOTIVO FROM TBL_WEB_EMISSAO_BOLETO_TP_FALHA WITH(NOLOCK) WHERE TP_STATUS = 1";

                DAL_MIS AcessaDadosMis = new Intranet.DAL.DAL_MIS();

                return AcessaDadosMis.ConsultaSQL(sqlcommand).Tables[0];
            }
            catch (Exception ex)
            {
                throw new Exception("RET.EmissaoBoleto_003: " + ex.Message, ex);
            }
        }

        public DataSet ListaProdutos(int DT_INI, int DT_FIM)
        {
            try
            {
                SqlCommand sqlcommand = new SqlCommand();
                sqlcommand.CommandType = CommandType.Text;
                sqlcommand.CommandText = "SELECT DISTINCT (CASE WHEN TP_PRODUTO = 'H' THEN 'HABITACIONAL' \n"
                                        + "                     WHEN B.NM_PRODUTO IS NULL THEN 'DESCONHECIDO' \n"
                                        + "				 ELSE B.NM_PRODUTO END) AS NM_PRODUTO  \n"
                                        + " FROM	TBL_WEB_EMISSAO_BOLETO  A WITH(NOLOCK) \n"
                                        + " LEFT JOIN  TBL_REM_PRODUTO_CAIXA  B WITH(NOLOCK) ON (A.CD_OPERACAO = B.NR_PRODUTO)  \n"
                                        + "WHERE 1=1  \n"
                                        + "  AND CONVERT(VARCHAR(8),DT_IMPORTACAO,112) BETWEEN @DT_INI AND @DT_FIM \n"
                                        + "  AND NR_USUARIO_EMISSAO IS NULL  \n";

                sqlcommand.Parameters.AddWithValue("@DT_INI", DT_INI);
                sqlcommand.Parameters.AddWithValue("@DT_FIM", DT_FIM);

                DAL_MIS AcessaDadosMisN = new Intranet.DAL.DAL_MIS();
                return AcessaDadosMisN.ConsultaSQL(sqlcommand);
            }
            catch (Exception ex)
            {
                throw new Exception("RET.EmissaoBoleto_004: " + ex.Message, ex);
            }
        }

        public DataSet ListaCanal(int DT_INI, int DT_FIM)
        {
            try
            {
                SqlCommand sqlcommand = new SqlCommand();
                sqlcommand.CommandType = CommandType.Text;
                sqlcommand.CommandText = "SELECT DISTINCT TP_ENVIO \n"
                                        + " FROM	TBL_WEB_EMISSAO_BOLETO  A WITH(NOLOCK) \n"
                                        + " WHERE 1=1  \n"
                                        + "  AND CONVERT(VARCHAR(8),DT_IMPORTACAO,112) BETWEEN @DT_INI AND @DT_FIM \n"
                                        + "  AND NR_USUARIO_EMISSAO IS NULL  \n";

                sqlcommand.Parameters.AddWithValue("@DT_INI", DT_INI);
                sqlcommand.Parameters.AddWithValue("@DT_FIM", DT_FIM);

                DAL_MIS AcessaDadosMisN = new Intranet.DAL.DAL_MIS();
                return AcessaDadosMisN.ConsultaSQL(sqlcommand);
            }
            catch (Exception ex)
            {
                throw new Exception("RET.EmissaoBoleto_004: " + ex.Message, ex);
            }
        }

        public DataSet GeraPesquisa(Int64 NR_CPF)
        {
            try
            {
                SqlCommand sqlcommand = new SqlCommand();
                sqlcommand.CommandType = CommandType.StoredProcedure;
                sqlcommand.CommandText = "SP_WEB_BOLETO_PESQUISA";
                sqlcommand.Parameters.AddWithValue("@NR_CPF", NR_CPF);

                DAL_MIS AcessaDadosMis = new Intranet.DAL.DAL_MIS();

                DataSet ds = AcessaDadosMis.ConsultaSQL(sqlcommand);
                return ds;
            }
            catch (Exception ex)
            {
                throw new Exception("RET.EmissaoBoleto_005: " + ex.Message, ex);
            }
        }
    }
}