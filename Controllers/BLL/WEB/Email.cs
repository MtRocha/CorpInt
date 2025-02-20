using Intranet_NEW.Controllers.DAL;
using System;
using System.Data;
using System.Data.SqlClient;


namespace Intranet.BLL.WEB
{
    public class Email : System.Web.UI.Page
    {

        DAL_Acesso AcessaAtestado = new Intranet.DAL.DAL_Acesso();
        DAL_ROTINAS AcessaBancoRotinas = new DAL.DAL_ROTINAS();

        public Intranet_NEW.Models.WEB.Email.Conta DadosConta(string NM_CONTA, string NM_EMAIL, int NR_COLABORADOR)
        {
            try
            {
                SqlCommand sqlcommand = new SqlCommand();
                sqlcommand.CommandType = CommandType.Text;
                sqlcommand.Parameters.AddWithValue("@NM_CONTA", NM_CONTA);
                sqlcommand.Parameters.AddWithValue("@NM_EMAIL", NM_EMAIL);
                sqlcommand.CommandText = "SELECT TOP 15 \n"
                                        + "   A.ID_CONTA        AS [ID_CONTA] \n"
                                        + "	, A.DT_INCLUSAO		AS [DT_INCLUSAO] \n"
                                        + "	, A.NM_CONTA		AS [NM_CONTA] \n"
                                        + "	, A.NM_EMAIL		AS [NM_EMAIL] \n"
                                        + "	, A.NM_SENHA		AS [NM_SENHA] \n"
                                        + "	, a.NM_STATUS	    AS [NM_STATUS] \n"
                                        + "FROM TBL_EMAIL_CONTAS A \n"
                                        + "WHERE ((@NM_CONTA <> '' AND A.NM_CONTA = @NM_CONTA) OR (@NM_EMAIL <> '' AND A.NM_EMAIL = @NM_EMAIL))  \n";

                
                DataSet ds = AcessaBancoRotinas.ConsultaSQL(sqlcommand);

                if ((ds.Tables.Count > 0) && (ds.Tables[0].Rows.Count > 0))
                {
                    DataRow dr = ds.Tables[0].Rows[0];

                    Intranet_NEW.Models.WEB.Email.Conta dtoConta = new Intranet_NEW.Models.WEB.Email.Conta();

                    dtoConta.ID_CONTA = int.Parse(dr["ID_CONTA"].ToString());
                    dtoConta.DT_INCLUSAO = dr["DT_INCLUSAO"] != DBNull.Value ? dr["DT_INCLUSAO"].ToString() : "";
                    dtoConta.NM_CONTA = dr["NM_CONTA"] != DBNull.Value ? dr["NM_CONTA"].ToString() : "";
                    dtoConta.NM_EMAIL = dr["NM_EMAIL"] != DBNull.Value ? dr["NM_EMAIL"].ToString() : "";
                    dtoConta.NM_SENHA = dr["NM_SENHA"] != DBNull.Value ? dr["NM_SENHA"].ToString() : "";
                    dtoConta.NM_STATUS = dr["NM_STATUS"] != DBNull.Value ? dr["NM_STATUS"].ToString() : "";

                    return dtoConta;
                }
                return new Intranet_NEW.Models.WEB.Email.Conta();
            }
            catch (Exception ex)
            {
                throw new Exception("BLL.WEB.Email_Conta_001: " + ex.Message, ex);
            }
        }

        public int GravaConta(Intranet_NEW.Models.WEB.Email.Conta dto)
        {
            try
            {
                SqlCommand sqlcommand = new SqlCommand();
                sqlcommand.CommandType = CommandType.Text;

                DateTime dt = DateTime.MinValue;
                int num = 0;

                sqlcommand.Parameters.AddWithValue("@ID_CONTA", int.TryParse(dto.ID_CONTA.ToString(), out num) ? (object)int.Parse(dto.ID_CONTA.ToString()) : 0);
                sqlcommand.Parameters.AddWithValue("@DT_INCLUSAO", DateTime.TryParse(dto.DT_INCLUSAO, out dt) ? (object)DateTime.Parse(dto.DT_INCLUSAO) : DBNull.Value);
                sqlcommand.Parameters.AddWithValue("@NR_USUARIO_INC", int.TryParse(dto.NR_USUARIO_INC.ToString(), out num) ? (object)int.Parse(dto.NR_USUARIO_INC.ToString()) : 0);
                sqlcommand.Parameters.AddWithValue("@NM_CONTA", dto.NM_CONTA);

                sqlcommand.Parameters.AddWithValue("@NM_EMAIL", dto.NM_EMAIL);
                sqlcommand.Parameters.AddWithValue("@NM_SENHA", dto.NM_SENHA);
                sqlcommand.Parameters.AddWithValue("@NM_STATUS", dto.NM_STATUS);

                //ALTERACAO
                sqlcommand.Parameters.AddWithValue("@DT_ALTERACAO", DateTime.TryParse(dto.DT_ALTERACAO, out dt) ? (object)DateTime.Parse(dto.DT_ALTERACAO) : DBNull.Value);
                sqlcommand.Parameters.AddWithValue("@NR_USUARIO_ALT", int.TryParse(dto.NR_USUARIO_ALT.ToString(), out num) ? (object)int.Parse(dto.NR_USUARIO_ALT.ToString()) : 0);

                if (dto.ID_CONTA < 1)
                    return NovaConta(sqlcommand);
                else
                    return AtualizarConta(sqlcommand);
            }
            catch (Exception ex)
            {
                throw new Exception("BLL.WEB.Email_Conta_002: " + ex.Message, ex);
            }
        }

        private int NovaConta(SqlCommand sqlcommand)
        {
            try
            {
                DataSet ds = new DataSet();
                sqlcommand.CommandType = CommandType.Text;
                sqlcommand.CommandText = "";
                sqlcommand.CommandText = "SELECT COUNT(*) FROM TBL_EMAIL_CONTAS WHERE NM_CONTA = @NM_CONTA OR NM_EMAIL = @NM_EMAIL";
                ds = AcessaBancoRotinas.ConsultaSQL(sqlcommand);

                if (int.Parse(ds.Tables[0].Rows[0][0].ToString()) == 0)
                {
                    sqlcommand.CommandText = "";
                    sqlcommand.CommandText = "INSERT INTO TBL_EMAIL_CONTAS \n"
                                                    + "        (DT_INCLUSAO, NM_CONTA, NM_EMAIL, NM_SENHA, NR_USUARIO_INC, NM_STATUS) \n"
                                                    + " VALUES (GETDATE(), @NM_CONTA, @NM_EMAIL, @NM_SENHA, @NR_USUARIO_INC, @NM_STATUS)  \n";

                    return AcessaBancoRotinas.ExecutaComandoSQL(sqlcommand);
                }
                else
                    return -1;
            }
            catch (Exception ex)
            {
                throw new Exception("BLL.WEB.Email_Conta_003: " + ex.Message, ex);
            }
        }

        private int AtualizarConta(SqlCommand sqlcommand)
        {
            try
            {
                sqlcommand.CommandText = "UPDATE TBL_EMAIL_CONTAS SET \n"
                                        + "  NM_CONTA = @NM_CONTA, NM_EMAIL = @NM_EMAIL, NM_SENHA = @NM_SENHA, NM_STATUS = @NM_STATUS, DT_ALTERACAO = GETDATE(), NR_USUARIO_ALT = @NR_USUARIO_ALT \n"
                                        + "WHERE ID_CONTA = @ID_CONTA \n";
               
                return AcessaBancoRotinas.ExecutaComandoSQL(sqlcommand);
            }
            catch (Exception ex)
            {
                throw new Exception("BLL.WEB.Email_Conta_004: " + ex.Message, ex);
            }
        }

        public DataTable ListaConta()
        {
            try
            {
                SqlCommand sqlcommand = new SqlCommand();
                sqlcommand.CommandType = CommandType.Text;
                //sqlcommand.Parameters.AddWithValue("@NM_CONTA", NM_CONTA);
                sqlcommand.CommandText = "SELECT \n"
                                        + "   A.ID_CONTA        AS [ID_CONTA] \n"
                                        + "	, A.DT_INCLUSAO		AS [DT_INCLUSAO] \n"
                                        + "	, A.NM_CONTA		AS [NM_CONTA] \n"
                                        + "	, A.NM_EMAIL		AS [NM_EMAIL] \n"
                                        + "	, A.NM_SENHA		AS [NM_SENHA] \n"
                                        + "	, A.NM_STATUS	    AS [NM_STATUS] \n"
                                        + "	, A.DT_ALTERACAO	AS [DT_ALTERACAO] \n"
                                        + "FROM TBL_EMAIL_CONTAS A \n";
                return AcessaBancoRotinas.ConsultaSQL(sqlcommand).Tables[0];
            }
            catch (Exception ex)
            {
                throw new Exception("BLL.WEB.Email_Conta_005: " + ex.Message, ex);
            }
        }

        public DataTable ListaEmail(int relatorio)
        {
            try
            {

                SqlCommand sqlcommand = new SqlCommand();
                sqlcommand.CommandType = CommandType.Text;
                if (relatorio == 0)
                {
                    sqlcommand.CommandText = "SELECT \n"
                                            + "   A.ID_LISTA            AS [ID_LISTA] \n"
                                            + "	, A.DT_INCLUSAO		    AS [DT_INCLUSAO] \n"
                                            + "	, A.NM_LISTA		    AS [NM_LISTA] \n"
                                            + "	, A.QTDE_EMAIL		    AS [QTDE_EMAIL] \n"
                                            + "	, A.QTDE_CLIENTE		AS [QTDE_CLIENTE] \n"
                                            + "FROM TBL_EMAIL_LISTA_CONTATOS A ORDER BY DT_INCLUSAO DESC\n";
                }
                else
                {
                    sqlcommand.CommandText = "SELECT DISTINCT ID_LISTA \n"
                        + "INTO #LISTA \n"
                        + "FROM TBL_EMAIL_LISTA_CONTATOS_ANALITICO WHERE DT_ENVIO IS NOT NULL \n"
                        + " \n"
                        + "CREATE INDEX IDX001 ON #LISTA(ID_LISTA)"
                        + " \n"
                        + " SELECT \n"
                        + "   A.ID_LISTA            AS [ID_LISTA] \n"
                        + "	, A.DT_INCLUSAO		    AS [DT_INCLUSAO] \n"
                        + "	, A.NM_LISTA		    AS [NM_LISTA] \n"
                        + "	, A.QTDE_EMAIL		    AS [QTDE_EMAIL] \n"
                        + "	, A.QTDE_CLIENTE		AS [QTDE_CLIENTE] \n"
                        + "FROM TBL_EMAIL_LISTA_CONTATOS A \n"
                        + "LEFT JOIN #LISTA B ON A.ID_LISTA = B.ID_LISTA \n"
                        + "WHERE B.ID_LISTA IS NULL \n"
                        + "ORDER BY DT_INCLUSAO DESC \n";

                }
                return AcessaBancoRotinas.ConsultaSQL(sqlcommand).Tables[0];
            }
            catch (Exception ex)
            {
                throw new Exception("BLL.WEB.Email_Lista_001: " + ex.Message, ex);
            }
        }

        public DataTable IncluirListaEmail(string NOME_LISTA, string USUARIO)
        {
            try
            {

                DataSet ds = new DataSet();
                SqlCommand sqlcommand = new SqlCommand();
                sqlcommand.CommandType = CommandType.Text;
                sqlcommand.CommandText = "";
                sqlcommand.Parameters.AddWithValue("@NM_LISTA", NOME_LISTA);
                sqlcommand.CommandText = "SELECT COUNT(*) FROM TBL_EMAIL_LISTA_CONTATOS WHERE NM_LISTA = @NM_LISTA";
                ds = AcessaBancoRotinas.ConsultaSQL(sqlcommand);

                if (int.Parse(ds.Tables[0].Rows[0][0].ToString()) == 0)
                {

                    sqlcommand.CommandType = CommandType.Text;
                    sqlcommand.CommandText = "";
                    sqlcommand.Parameters.AddWithValue("@USUARIO", USUARIO);

                    DateTime dt = DateTime.MinValue;
                    sqlcommand.CommandText = "";
                    sqlcommand.CommandText = "INSERT INTO TBL_EMAIL_LISTA_CONTATOS \n"
                                                        + "        (DT_INCLUSAO, NM_LISTA, QTDE_EMAIL, QTDE_CLIENTE, NR_USUARIO_INC) \n"
                                                        + " VALUES (GETDATE(), @NM_LISTA, 0, 0, @USUARIO)  \n"
                                                        + " \n"
                                                        + " SELECT MAX(ID_LISTA) FROM TBL_EMAIL_LISTA_CONTATOS";
 
                    DataTable dtresult = new DataTable();
                    dtresult = AcessaBancoRotinas.ConsultaSQL(sqlcommand).Tables[0];

                    return dtresult;
                }
                else
                    return ds.Tables[0];

            }
            catch (Exception ex)
            {
                throw new Exception("BLL.WEB.Email_Conta_003: " + ex.Message, ex);
            }
        }

        public int IncluirListaEmailAnalitico(DataTable ListaEmail, string ID)
        {
            try
            {

                DataTable ListaImportar = new DataTable();
                ListaImportar.Columns.Add("ID_LISTA");
                ListaImportar.Columns.Add("DT_IMPORTACAO");
                ListaImportar.Columns.Add("NR_CPF");
                ListaImportar.Columns.Add("NM_EMAIL");
                ListaImportar.Columns.Add("DT_ENVIO");
                ListaImportar.Columns.Add("TP_STATUS");
                ListaImportar.Columns.Add("NM_NOME");

                for (int i = 0; i < ListaEmail.Rows.Count; i++)
                {
                    DataRow dr = ListaImportar.NewRow();
                    dr[0] = int.Parse(ID.ToString());
                    dr[1] = Int64.Parse(DateTime.Now.ToString("yyyyMMddHHmmss"));
                    dr[2] = Int64.Parse(ListaEmail.Rows[i][0].ToString());
                    dr[3] = ListaEmail.Rows[i][1].ToString();
                    dr[4] = DBNull.Value;
                    dr[5] = 0;
                    dr[6] = ListaEmail.Rows[i][2].ToString(); ;
                    ListaImportar.Rows.Add(dr);
                }

                /* Importar Base */
                AcessaBancoRotinas.ExecutaBulkCopySQL(ListaImportar, "TBL_EMAIL_LISTA_CONTATOS_ANALITICO_" + DateTime.Now.ToString("yyyyMM"));
               

                /* Atualiza Valores de Importacao */
                SqlCommand sqlcommand = new SqlCommand();
                sqlcommand.CommandType = CommandType.Text;
                sqlcommand.Parameters.AddWithValue("@ID_LISTA", ID);
                sqlcommand.Parameters.AddWithValue("@QTDE_CLIENTE", ListaEmail.Rows.Count);
                sqlcommand.Parameters.AddWithValue("@QTDE_EMAIL", ListaEmail.Rows.Count);

                DateTime dt = DateTime.MinValue;
                sqlcommand.CommandText = "";
                sqlcommand.CommandText = "UPDATE TBL_EMAIL_LISTA_CONTATOS \n"
                                                    + " SET QTDE_CLIENTE = @QTDE_CLIENTE, QTDE_EMAIL = @QTDE_EMAIL \n"
                                                    + " WHERE ID_LISTA = @ID_LISTA";

                return AcessaBancoRotinas.ExecutaComandoSQL(sqlcommand);

            }
            catch (Exception ex)
            {
                throw new Exception("BLL.WEB.Email_Conta_004: " + ex.Message, ex);
            }
        }

        public DataTable IncluirTemplate(string NOME_TEMPLATE, string NM_HTML, string USUARIO)
        {
            try
            {
                SqlCommand sqlcommand = new SqlCommand();
                sqlcommand.CommandType = CommandType.Text;
                sqlcommand.Parameters.AddWithValue("@NM_TEMPLATE", NOME_TEMPLATE);
                sqlcommand.Parameters.AddWithValue("@NM_HTML", NM_HTML);
                sqlcommand.Parameters.AddWithValue("@USUARIO", USUARIO);

                DateTime dt = DateTime.MinValue;
                sqlcommand.CommandText = "";
                sqlcommand.CommandText = "INSERT INTO TBL_EMAIL_TEMPLATE \n"
                                                    + "        (DT_INCLUSAO, NM_TEMPLATE, NM_HTML, NR_USUARIO_INC) \n"
                                                    + " VALUES (GETDATE(), @NM_TEMPLATE, @NM_HTML, @USUARIO)  \n"
                                                    + " \n"
                                                    + " SELECT * FROM TBL_EMAIL_TEMPLATE WHERE NM_TEMPLATE = @NM_TEMPLATE";

                DataTable dtresult = new DataTable();
                dtresult = AcessaBancoRotinas.ConsultaSQL(sqlcommand).Tables[0];

                return dtresult;

            }
            catch (Exception ex)
            {
                throw new Exception("BLL.WEB.Email_Conta_005: " + ex.Message, ex);
            }
        }

        public DataTable ListaTemplate()
        {
            try
            {
                SqlCommand sqlcommand = new SqlCommand();
                sqlcommand.CommandType = CommandType.Text;
                sqlcommand.CommandText = "SELECT \n"
                                       + "   A.ID_TEMPLATE        AS [ID_TEMPLATE] \n"
                                       + "	, A.DT_INCLUSAO		AS [DT_INCLUSAO] \n"
                                       + "	, A.NM_TEMPLATE		AS [NM_TEMPLATE] \n"
                                       + "	, A.NM_HTML		AS [NM_HTML] \n"
                                       + "FROM TBL_EMAIL_TEMPLATE A ORDER BY DT_INCLUSAO DESC\n";

                return AcessaBancoRotinas.ConsultaSQL(sqlcommand).Tables[0];
            }
            catch (Exception ex)
            {
                throw new Exception("BLL.WEB.Email_Conta_006: " + ex.Message, ex);
            }
        }

        public int GravaAcao(Intranet_NEW.Models.WEB.Email.Acao dto)
        {
            try
            {
                SqlCommand sqlcommand = new SqlCommand();
                sqlcommand.CommandType = CommandType.Text;

                DateTime dt = DateTime.MinValue;
                int num = 0;

                sqlcommand.Parameters.AddWithValue("@ID_ACAO", int.TryParse(dto.ID_ACAO.ToString(), out num) ? (object)int.Parse(dto.ID_ACAO.ToString()) : 0);
                sqlcommand.Parameters.AddWithValue("@DT_INCLUSAO", DateTime.TryParse(dto.DT_INCLUSAO, out dt) ? (object)DateTime.Parse(dto.DT_INCLUSAO) : DBNull.Value);
                sqlcommand.Parameters.AddWithValue("@NR_USUARIO", int.TryParse(dto.NR_USUARIO.ToString(), out num) ? (object)int.Parse(dto.NR_USUARIO.ToString()) : 0);
                sqlcommand.Parameters.AddWithValue("@NM_ACAO", dto.NM_ACAO);

                sqlcommand.Parameters.AddWithValue("@ID_CONTA", dto.ID_CONTA);
                sqlcommand.Parameters.AddWithValue("@ID_LISTA", dto.ID_LISTA);
                sqlcommand.Parameters.AddWithValue("@ID_TEMPLATE", dto.ID_TEMPLATE);
                sqlcommand.Parameters.AddWithValue("@NM_EMAIL_COPIA", dto.NM_EMAIL_COPIA);
                sqlcommand.Parameters.AddWithValue("@NM_DESCRICAO", dto.NM_DESCRICAO);
                sqlcommand.Parameters.AddWithValue("@NM_STATUS", dto.STATUS);
                sqlcommand.Parameters.AddWithValue("@NM_ASSUNTO", dto.NM_ASSUNTO);

                sqlcommand.Parameters.AddWithValue("@DT_DISPARO", DateTime.Parse(dto.DT_DISPARO.ToString()));
                sqlcommand.Parameters.AddWithValue("@HR_DISPARO", dto.HR_DISPARO);


                if (dto.ID_ACAO < 1)
                    return IncluirAcao(sqlcommand);
                else
                    return AtualizarAcao(sqlcommand);
            }
            catch (Exception ex)
            {
                throw new Exception("BLL.WEB.Email_Acao_007: " + ex.Message, ex);
            }
        }

        private int IncluirAcao(SqlCommand sqlcommand)
        {
            try
            {
                DataSet ds = new DataSet();
                sqlcommand.CommandType = CommandType.Text;
                sqlcommand.CommandText = "";
                sqlcommand.CommandText = "SELECT COUNT(*) FROM TBL_EMAIL_ACAO WHERE NM_ACAO = @NM_ACAO";
                ds = AcessaBancoRotinas.ConsultaSQL(sqlcommand);

                if (int.Parse(ds.Tables[0].Rows[0][0].ToString()) == 0)
                {
                    sqlcommand.CommandText = "";
                    sqlcommand.CommandText = "INSERT INTO TBL_EMAIL_ACAO \n"
                                                       + "        (DT_INCLUSAO, NM_ACAO, ID_CONTA, ID_LISTA, ID_TEMPLATE, Nm_EMAIL_COPIA, DT_DISPARO, HR_DISPARO, NM_DESCRICAO, NR_USUARIO_INC, NM_STATUS, NM_ASSUNTO) \n"
                                                       + " VALUES (GETDATE(), @NM_ACAO, @ID_CONTA, @ID_LISTA, @ID_TEMPLATE, @Nm_EMAIL_COPIA, @DT_DISPARO, @HR_DISPARO, @NM_DESCRICAO, @NR_USUARIO, @NM_STATUS, @NM_ASSUNTO)";

                    return AcessaBancoRotinas.ExecutaComandoSQL(sqlcommand);
                }
                else
                    return -1;
            }
            catch (Exception ex)
            {
                throw new Exception("BLL.WEB.Email_Acao_008: " + ex.Message, ex);
            }
        }

        private int AtualizarAcao(SqlCommand sqlcommand)
        {
            try
            {
                sqlcommand.CommandText = "";
                sqlcommand.CommandText = "UPDATE TBL_EMAIL_ACAO \n"
                                                   + " SET NM_EMAIL_COPIA = @NM_EMAIL_COPIA, DT_DISPARO = @DT_DISPARO, NM_ASSUNTO = @NM_ASSUNTO, \n"
                                                   + " HR_DISPARO = @HR_DISPARO, NM_DESCRICAO = @NM_DESCRICAO, NM_STATUS = @NM_STATUS \n"
                                                   + " WHERE ID_ACAO = @ID_ACAO";
                return AcessaBancoRotinas.ExecutaComandoSQL(sqlcommand);
            }
            catch (Exception ex)
            {
                throw new Exception("BLL.WEB.Email_Acao_009: " + ex.Message, ex);
            }
        }

        public DataTable ListaAcao()
        {
            try
            {
                SqlCommand sqlcommand = new SqlCommand();
                sqlcommand.CommandType = CommandType.Text;
                //sqlcommand.Parameters.AddWithValue("@NM_CONTA", NM_CONTA);
                sqlcommand.CommandText = "SELECT \n"
                                        + " A.ID_ACAO, A.DT_INCLUSAO, A.NM_ACAO, D.NM_CONTA, B.NM_LISTA, C.NM_TEMPLATE, CONCAT(CONVERT(CHAR(10),A.DT_DISPARO,103), ' ', A.HR_DISPARO) AS DT_AGENDAMENTO, A.NM_STATUS, A.NM_ASSUNTO, NM_EMAIL_COPIA, NM_DESCRICAO \n"
                                        + "	FROM TBL_EMAIL_ACAO A \n"
                                        + "	LEFT JOIN TBL_EMAIL_LISTA_CONTATOS B ON A.ID_LISTA = B.ID_LISTA \n"
                                        + "	LEFT JOIN TBL_EMAIL_TEMPLATE       C ON A.ID_TEMPLATE = C.ID_TEMPLATE \n"
                                        + "	LEFT JOIN TBL_EMAIL_CONTAS          D ON A.ID_CONTA = D.ID_CONTA \n"
                                        + " ORDER BY A.DT_INCLUSAO DESC";
                return AcessaBancoRotinas.ConsultaSQL(sqlcommand).Tables[0];
            }
            catch (Exception ex)
            {
                throw new Exception("BLL.WEB.Email_Acao_009: " + ex.Message, ex);
            }
        }

        public DataTable ListaPainel(DateTime DataInicio, DateTime DataFim)
        {
            try
            {
                SqlCommand sqlcommand = new SqlCommand();
                sqlcommand.CommandType = CommandType.Text;
                sqlcommand.Parameters.AddWithValue("@DT_INICIO", DataInicio);
                sqlcommand.Parameters.AddWithValue("@DT_FIM", DataFim);
                sqlcommand.CommandText = "SELECT \n"
                                        + " A.ID_ACAO, A.NM_ACAO, A.NM_STATUS, CONVERT(VARCHAR(20),CONCAT(CONVERT(CHAR(10), A.DT_DISPARO, 120), ' ', A.HR_DISPARO),112) AS DT_DISPARO, MIN(B.DT_ENVIO) AS DT_INICIO, MAX(B.DT_ENVIO) AS DT_ULTIMO, COUNT(*) AS QTDE_EMAIL, SUM(IIF(B.TP_STATUS = 1,1,0)) AS QTDE_ENVIADO, SUM(IIF(B.TP_STATUS = -1,1,0)) AS QTDE_ERRO \n"
                                        + "	FROM TBL_EMAIL_ACAO A \n"
                                        + " INNER JOIN TBL_EMAIL_LISTA_CONTATOS_ANALITICO_" + DateTime.Now.ToString("yyyyMM") + " B ON A.ID_LISTA = B.ID_LISTA \n"
                                        + " WHERE 1=1 AND A.DT_DISPARO BETWEEN @DT_INICIO AND @DT_FIM \n"
                                        + "	 GROUP BY A.ID_ACAO, A.NM_ACAO, A.NM_STATUS, A.DT_DISPARO, A.HR_DISPARO \n"
                                        + "	 ORDER BY A.DT_DISPARO, A.HR_DISPARO DESC \n";
                DataTable dt = new DataTable();
                dt = AcessaBancoRotinas.ConsultaSQL(sqlcommand).Tables[0];

                return dt;
            }
            catch (Exception ex)
            {
                throw new Exception("BLL.WEB.Email_Acao_010: " + ex.Message, ex);
            }
        }
    }
}
