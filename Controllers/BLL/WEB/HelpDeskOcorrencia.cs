using Intranet_NEW.Controllers.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;

namespace Intranet.BLL.WEB
{
    public class HelpDeskOcorrencia
    {
        #region Dados das ocorrencias abertas para ti

        public DataSet ListaOcorrencia(DateTime DT_INI, DateTime DT_FIM)
        {
            try
            {
                SqlCommand sqlcommand = new SqlCommand();
                sqlcommand.CommandType = CommandType.StoredProcedure;
                sqlcommand.CommandText = "SP_WEB_TI_LISTA_OCORRENCIA";

                sqlcommand.Parameters.AddWithValue("@DT_INI", DT_INI);
                sqlcommand.Parameters.AddWithValue("@DT_FIM", DT_FIM);

                DAL_MIS AcessaDadosMis = new DAL.DAL_MIS();
                return AcessaDadosMis.ConsultaSQL(sqlcommand);
            }
            catch (Exception ex)
            {
                throw new Exception("BLL.WEB.HelpDeskOcorrencia_001: " + ex.Message, ex);
            }
        }
        public DataSet ListaChamadoAberto()
        {
            SqlCommand sqlcommand = new SqlCommand();
            sqlcommand.CommandType = CommandType.Text;
            sqlcommand.CommandText = "SELECT  \n"
                                    + "	  A.NR_OCORRENCIA \n"
                                    + "	, A.DT_OCORRENCIA \n"
                                    + "	, A.HR_OCORRENCIA \n"
                                    + "	, B.NM_COLABORADOR AS [NM_SUPERVISOR] \n"
                                    + "	, C.NM_COLABORADOR AS [NM_OPERADOR] \n"
                                    + "	, A.NR_PA \n"
                                    + "	, A.NR_IP	 \n"
                                    + " , A.TP_PAVIMENTO \n"
                                    + " , CASE A.TP_STATUS \n"
                                    + "     WHEN 0 THEN 'ABERTO' \n"
                                    + "     WHEN 1 THEN 'EM ATENDIMENTO' \n"
                                    + "   END AS [TP_STATUS] \n"
                                    + " , D.NM_COLABORADOR  AS [NM_TECNICO]"
                                    + "FROM TBL_WEB_HELPDESK_OCORRENCIA        A \n"
                                    + "	LEFT JOIN TBL_WEB_COLABORADOR_DADOS B ON A.NR_SUPERVISOR = B.NR_COLABORADOR \n"
                                    + "	LEFT JOIN TBL_WEB_COLABORADOR_DADOS C ON A.NR_OPERADOR   = C.NR_COLABORADOR \n"
                                    + "	LEFT JOIN TBL_WEB_COLABORADOR_DADOS D ON A.NR_TECNICO    = D.NR_COLABORADOR \n"
                                    + "WHERE A.TP_STATUS != 2 \n"
                                    + "ORDER BY 1 \n";

            DAL_MIS AcessaDadosMis = new DAL.DAL_MIS();
            return AcessaDadosMis.ConsultaSQL(sqlcommand);
        }
        public Intranet_NEW.Models.WEB.HelpDeskOcorrencia DadosOcorrencia(int NR_OCORRENCIA)
        {
            try
            {
                SqlCommand sqlcommand = new SqlCommand();
                sqlcommand.CommandType = CommandType.Text;
                sqlcommand.Parameters.AddWithValue("@NR_OCORRENCIA", NR_OCORRENCIA);
                sqlcommand.CommandText = "SELECT * FROM TBL_WEB_HELPDESK_OCORRENCIA WHERE NR_OCORRENCIA = @NR_OCORRENCIA \n";

                DAL_MIS AcessaDadosMis = new DAL.DAL_MIS();
                DataSet ds = AcessaDadosMis.ConsultaSQL(sqlcommand);

                Intranet_NEW.Models.WEB.HelpDeskOcorrencia dto = new DTO.HelpDeskOcorrencia();
                if ((ds.Tables.Count > 0) && (ds.Tables[0].Rows.Count > 0))
                {
                    DataRow dr = ds.Tables[0].Rows[0];

                    dto.NR_OCORRENCIA = dr["NR_OCORRENCIA"] != DBNull.Value ? dr["NR_OCORRENCIA"].ToString() : "";
                    dto.DT_OCORRENCIA = dr["DT_OCORRENCIA"] != DBNull.Value ? ((DateTime)dr["DT_OCORRENCIA"]).ToString("dd/MM/yyyy") : "";
                    dto.HR_OCORRENCIA = dr["HR_OCORRENCIA"] != DBNull.Value ? dr["HR_OCORRENCIA"].ToString() : "";
                    dto.NR_COORDENADOR = dr["NR_COORDENADOR"] != DBNull.Value ? dr["NR_COORDENADOR"].ToString() : "";
                    dto.NR_SUPERVISOR = dr["NR_SUPERVISOR"] != DBNull.Value ? dr["NR_SUPERVISOR"].ToString() : "";
                    dto.NR_OPERADOR = dr["NR_OPERADOR"] != DBNull.Value ? dr["NR_OPERADOR"].ToString() : "";
                    dto.TP_PAVIMENTO = dr["TP_PAVIMENTO"] != DBNull.Value ? dr["TP_PAVIMENTO"].ToString() : "";
                    dto.NR_PA = dr["NR_PA"] != DBNull.Value ? dr["NR_PA"].ToString() : "";
                    dto.NR_IP = dr["NR_IP"] != DBNull.Value ? dr["NR_IP"].ToString() : "";
                    dto.NR_TECNICO = dr["NR_TECNICO"] != DBNull.Value ? dr["NR_TECNICO"].ToString() : "";

                    dto.TP_TIPO = dr["TP_TIPO"] != DBNull.Value ? dr["TP_TIPO"].ToString() : "";
                    dto.TP_DESCRICAO = dr["TP_DESCRICAO"] != DBNull.Value ? dr["TP_DESCRICAO"].ToString() : "";
                    dto.TP_DEFEITO_HEADSET = dr["TP_DEFEITO_HEADSET"] != DBNull.Value ? dr["TP_DEFEITO_HEADSET"].ToString() : "";
                    dto.NR_HEADSET = dr["NR_HEADSET"] != DBNull.Value ? dr["NR_HEADSET"].ToString() : "";

                    dto.TP_RESOLUCAO = dr["TP_RESOLUCAO"] != DBNull.Value ? dr["TP_RESOLUCAO"].ToString() : "";

                    dto.NM_OBSERVACAO = dr["NM_OBSERVACAO"] != DBNull.Value ? dr["NM_OBSERVACAO"].ToString() : "";
                    dto.TP_STATUS = dr["TP_STATUS"] != DBNull.Value ? dr["TP_STATUS"].ToString() : "";

                    dto.DS_PROBLEMA = dr["DS_PROBLEMA"] != DBNull.Value ? dr["DS_PROBLEMA"].ToString() : "";

                    return dto;
                }
                return dto;
            }
            catch (Exception ex)
            {
                throw new Exception("BLL.WEB.Operador_002: " + ex.Message);
            }
        }
        public int VerificaChamadoAberto(int NR_OPERADOR)
        {
            try
            {
                SqlCommand sqlcommand = new SqlCommand();
                sqlcommand.CommandType = CommandType.Text;
                sqlcommand.CommandText = "SELECT * FROM TBL_WEB_HELPDESK_OCORRENCIA WHERE NR_OPERADOR = @NR_OPERADOR AND TP_STATUS = 0";
                sqlcommand.Parameters.AddWithValue("@NR_OPERADOR", NR_OPERADOR);

                DAL_MIS AcessaDadosMis = new DAL.DAL_MIS();
                DataSet ds = AcessaDadosMis.ConsultaSQL(sqlcommand);

                if ((ds.Tables.Count > 0) && (ds.Tables[0].Rows.Count > 0))
                    return 1;
                else
                    return 0;
            }
            catch (Exception ex)
            {
                throw new Exception("BLL.WEB.HelpDeskOcorrencia_003: " + ex.Message, ex);
            }
        }
        public int GravaOcorrencia(Intranet_NEW.Models.WEB.HelpDeskOcorrencia dto, string Origem)
        {
            try
            {
                int NR_OPERADOR = int.Parse(dto.NR_OPERADOR);
                if (VerificaChamadoAberto(NR_OPERADOR) > 0 && Origem == "Operador")
                    return -1;

                SqlCommand sqlcommand = new SqlCommand();
                sqlcommand.CommandType = CommandType.Text;

                DateTime dt = DateTime.MinValue;
                int num = 0;

                sqlcommand.Parameters.AddWithValue("@NR_OCORRENCIA", int.TryParse(dto.NR_OCORRENCIA, out num) ? (object)int.Parse(dto.NR_OCORRENCIA) : DBNull.Value);
                sqlcommand.Parameters.AddWithValue("@DT_OCORRENCIA", DateTime.TryParse(dto.DT_OCORRENCIA, out dt) ? (object)DateTime.Parse(dto.DT_OCORRENCIA) : DBNull.Value);
                sqlcommand.Parameters.AddWithValue("@HR_OCORRENCIA", dto.HR_OCORRENCIA);

                sqlcommand.Parameters.AddWithValue("@NR_COORDENADOR", dto.NR_COORDENADOR == "" ? 0 : int.Parse(dto.NR_COORDENADOR));
                sqlcommand.Parameters.AddWithValue("@NR_SUPERVISOR", dto.NR_SUPERVISOR == "" ? 0 : int.Parse(dto.NR_SUPERVISOR));
                sqlcommand.Parameters.AddWithValue("@NR_OPERADOR", NR_OPERADOR);
                sqlcommand.Parameters.AddWithValue("@TP_PAVIMENTO", dto.TP_PAVIMENTO);
                sqlcommand.Parameters.AddWithValue("@NR_PA", int.TryParse(dto.NR_PA, out num) ? (object)int.Parse(dto.NR_PA) : 0);
                sqlcommand.Parameters.AddWithValue("@NR_IP", dto.NR_IP);

                sqlcommand.Parameters.AddWithValue("@NR_TECNICO", int.Parse(dto.NR_TECNICO));

                sqlcommand.Parameters.AddWithValue("@TP_TIPO", dto.TP_TIPO);
                sqlcommand.Parameters.AddWithValue("@TP_DESCRICAO", dto.TP_DESCRICAO);
                sqlcommand.Parameters.AddWithValue("@TP_DEFEITO_HEADSET", dto.TP_DEFEITO_HEADSET);
                sqlcommand.Parameters.AddWithValue("@NR_HEADSET", dto.NR_HEADSET);

                sqlcommand.Parameters.AddWithValue("@TP_RESOLUCAO", int.Parse(dto.TP_RESOLUCAO));
                sqlcommand.Parameters.AddWithValue("@TP_LOGOUT", dto.TP_LOGOUT == null ? 0 : int.Parse(dto.TP_LOGOUT));

                sqlcommand.Parameters.AddWithValue("@TP_STATUS", dto.TP_STATUS);
                sqlcommand.Parameters.AddWithValue("@NM_OBSERVACAO", dto.NM_OBSERVACAO);
                sqlcommand.Parameters.AddWithValue("@DS_PROBLEMA", dto.DS_PROBLEMA);

                sqlcommand.Parameters.AddWithValue("@NR_USUARIO_SISTEMA", dto.NR_USUARIO_SISTEMA);


                /* Email Abertura de Chamado - GLPI */
                Intranet_NEW.Models.WEB.Colaborador dtoperador = new DTO.Colaborador();
                BLL.WEB.Colaborador cmd = new BLL.WEB.Colaborador();
                dtoperador = cmd.DadosColaborador(int.Parse(dto.NR_OPERADOR), "");

                //StringBuilder bldTable = new StringBuilder();
                //bldTable.AppendLine("<table style=\"border: solid 1px black;\">");
                //bldTable.AppendLine("<tr style=\"border: solid 1px black;\">");
                //bldTable.Append(string.Format("<td style=\"border: solid 1px black;\">{0}</td>", "OPERADOR: "));
                //bldTable.Append(string.Format("<td style=\"border: solid 1px black;\">{0}</td>", dtoperador.NM_COLABORADOR.ToString()));
                //bldTable.AppendLine("</tr>");
                //bldTable.AppendLine("<tr style=\"border: solid 1px black;\">");
                //bldTable.Append(string.Format("<td style=\"border: solid 1px black;\">{0}</td>", "SUPERVISOR: "));
                //bldTable.Append(string.Format("<td style=\"border: solid 1px black;\">{0}</td>", dtoperador.NM_SUPERVISOR.ToString()));
                //bldTable.AppendLine("</tr>");
                //bldTable.AppendLine("<tr style=\"border: solid 1px black;\">");
                //bldTable.Append(string.Format("<td style=\"border: solid 1px black;\">{0}</td>", "COORDENADOR: "));
                //bldTable.Append(string.Format("<td style=\"border: solid 1px black;\">{0}</td>", dtoperador.NM_COORDENADOR.ToString()));
                //bldTable.AppendLine("</tr>");
                //bldTable.AppendLine("<tr style=\"border: solid 1px black;\">");
                //bldTable.Append(string.Format("<td style=\"border: solid 1px black;\">{0}</td>", "PAVIMENTO: "));
                //bldTable.Append(string.Format("<td style=\"border: solid 1px black;\">{0}</td>", dtoperador.TP_PAVIMENTO.ToString()));
                //bldTable.AppendLine("</tr>");
                //bldTable.AppendLine("<tr style=\"border: solid 1px black;\">");
                //bldTable.Append(string.Format("<td style=\"border: solid 1px black;\">{0}</td>", "TURNO: "));
                //bldTable.Append(string.Format("<td style=\"border: solid 1px black;\">{0}</td>", dtoperador.TP_TURNO.ToString()));
                //bldTable.AppendLine("</tr>");
                //bldTable.AppendLine("<tr style=\"border: solid 1px black;\">");
                //bldTable.Append(string.Format("<td style=\"border: solid 1px black;\">{0}</td>", "FUNCAO: "));
                //bldTable.Append(string.Format("<td style=\"border: solid 1px black;\">{0}</td>", dtoperador.NM_FUNCAO_RH.ToString()));
                //bldTable.AppendLine("</tr>");
                //bldTable.AppendLine("<tr style=\"border: solid 1px black;\">");
                //bldTable.Append(string.Format("<td style=\"border: solid 1px black;\">{0}</td>", "IP: "));
                //bldTable.Append(string.Format("<td style=\"border: solid 1px black;\">{0}</td>", dto.NR_IP.ToString()));
                //bldTable.AppendLine("</tr>");
                //bldTable.AppendLine("<tr style=\"border: solid 1px black;\">");
                //bldTable.Append(string.Format("<td style=\"border: solid 1px black;\">{0}</td>", "DESCRICAO: "));
                //bldTable.Append(string.Format("<td style=\"border: solid 1px black;\">{0}</td>", dto.DS_PROBLEMA.ToString()));
                //bldTable.AppendLine("</tr>");

                //bldTable.AppendLine("</table>");

                //InformeArquivoRemessaTarde(bldTable.ToString());

                if (dto.NR_OCORRENCIA == "")
                    return Novo(sqlcommand);
                else
                    return Atualizar(sqlcommand);
                //return 1;
            }
            catch (Exception ex)
            {
                throw new Exception("BLL.WEB.HelpDeskOcorrencia_003: " + ex.Message, ex);
            }
        }
        private int Novo(SqlCommand sqlcommand)
        {
            try
            {
                sqlcommand.CommandText = "INSERT INTO TBL_WEB_HELPDESK_OCORRENCIA \n"
                                        + "        (DT_OCORRENCIA, HR_OCORRENCIA, NR_COORDENADOR, NR_SUPERVISOR,  NR_OPERADOR, TP_PAVIMENTO, NR_PA, NR_IP, NR_TECNICO, TP_TIPO, TP_DESCRICAO, TP_DEFEITO_HEADSET, NR_HEADSET, TP_RESOLUCAO, TP_LOGOUT, NM_OBSERVACAO, DS_PROBLEMA, TP_STATUS, NR_USUARIO_INCLUSAO, DT_ULTIMA_ALTERACAO, NR_USUARIO_ALTERACAO) \n"
                                        + "VALUES  (@DT_OCORRENCIA, @HR_OCORRENCIA, @NR_COORDENADOR, @NR_SUPERVISOR, @NR_OPERADOR, @TP_PAVIMENTO, @NR_PA, @NR_IP, @NR_TECNICO, @TP_TIPO, @TP_DESCRICAO, @TP_DEFEITO_HEADSET, @NR_HEADSET, @TP_RESOLUCAO, @TP_LOGOUT, @NM_OBSERVACAO, @DS_PROBLEMA, @TP_STATUS, @NR_USUARIO_SISTEMA, GETDATE(), @NR_USUARIO_SISTEMA) \n";

                DAL_MIS AcessaDadosMis = new DAL.DAL_MIS();
                return AcessaDadosMis.ExecutaComandoSQL(sqlcommand);
            }
            catch (Exception ex)
            {
                throw new Exception("BLL.WEB.HelpDeskOcorrencia_004: " + ex.Message, ex);
            }
        }
        private int Atualizar(SqlCommand sqlcommand)
        {
            try
            {
                sqlcommand.CommandText = "UPDATE TBL_WEB_HELPDESK_OCORRENCIA SET \n"
                                        + "  DT_OCORRENCIA = @DT_OCORRENCIA,HR_OCORRENCIA = @HR_OCORRENCIA, NR_COORDENADOR = @NR_COORDENADOR, NR_SUPERVISOR = @NR_SUPERVISOR,NR_OPERADOR = @NR_OPERADOR,TP_PAVIMENTO = @TP_PAVIMENTO,NR_PA = @NR_PA, NR_IP = @NR_IP   \n"
                                        + ", NR_TECNICO = @NR_TECNICO, TP_TIPO = @TP_TIPO, TP_DESCRICAO = @TP_DESCRICAO, TP_LOGOUT = @TP_LOGOUT, TP_DEFEITO_HEADSET = @TP_DEFEITO_HEADSET, NR_HEADSET = @NR_HEADSET,TP_RESOLUCAO = @TP_RESOLUCAO \n"
                                        + ", NM_OBSERVACAO = @NM_OBSERVACAO, DS_PROBLEMA = @DS_PROBLEMA, TP_STATUS = @TP_STATUS, DT_ULTIMA_ALTERACAO = GETDATE(), NR_USUARIO_ALTERACAO = @NR_USUARIO_SISTEMA \n"
                                        + "WHERE NR_OCORRENCIA = @NR_OCORRENCIA \n";

                DAL_MIS AcessaDadosMis = new DAL.DAL_MIS();
                return AcessaDadosMis.ExecutaComandoSQL(sqlcommand);
            }
            catch (Exception ex)
            {
                throw new Exception("BLL.WEB.HelpDeskOcorrencia_005: " + ex.Message, ex);
            }
        }
        public int AtualizaTecnico(string NR_OCORRENCIA, string NR_TECNICO)
        {
            try
            {
                SqlCommand sqlcommand = new SqlCommand();
                sqlcommand.CommandType = CommandType.Text;

                DateTime dt = DateTime.MinValue;

                sqlcommand.Parameters.AddWithValue("@NR_OCORRENCIA", NR_OCORRENCIA);
                sqlcommand.Parameters.AddWithValue("@NR_TECNICO", NR_TECNICO);

                sqlcommand.CommandText = "UPDATE TBL_WEB_HELPDESK_OCORRENCIA SET \n"
                                        + "  NR_TECNICO = @NR_TECNICO \n"
                                        + "WHERE NR_OCORRENCIA = @NR_OCORRENCIA \n";

                DAL_MIS AcessaDadosMis = new DAL.DAL_MIS();
                return AcessaDadosMis.ExecutaComandoSQL(sqlcommand);
            }
            catch (Exception ex)
            {
                throw new Exception("BLL.WEB.HelpDeskOcorrencia_005: " + ex.Message, ex);
            }
        }
        public DataTable CarregaDescricaoTipo(string TP_TIPO)
        {
            try
            {
                SqlCommand sqlcommand = new SqlCommand();
                sqlcommand.CommandType = CommandType.Text;
                sqlcommand.CommandText = "SELECT NR_DESCRICAO, DS_DESCRICAO FROM TBL_WEB_HELPDESK_TIPO_DESCRICAO WHERE TP_TIPO = @TP_TIPO ORDER BY 1";
                sqlcommand.Parameters.AddWithValue("@TP_TIPO", TP_TIPO);

                DAL_MIS AcessaDadosMis = new DAL.DAL_MIS();
                return AcessaDadosMis.ConsultaSQL(sqlcommand).Tables[0];
            }
            catch (Exception ex)
            {
                throw new Exception("BLL.WEB.HelpDeskOcorrencia_005: " + ex.Message, ex);
            }
        }
        public DataSet ListaOcorrenciaAnalitico(DateTime DT_INI, DateTime DT_FIM, int TP_RESOLUCAO, string NM_COLABORADOR)
        {
            try
            {
                SqlCommand sqlcommand = new SqlCommand();
                sqlcommand.CommandType = CommandType.StoredProcedure;
                sqlcommand.CommandText = "SP_WEB_TI_LISTA_OCORRENCIA_ANALITICO";

                sqlcommand.Parameters.AddWithValue("@DT_INI", DT_INI);
                sqlcommand.Parameters.AddWithValue("@DT_FIM", DT_FIM);
                sqlcommand.Parameters.AddWithValue("@TP_RESOLUCAO", TP_RESOLUCAO);
                sqlcommand.Parameters.AddWithValue("@NM_COLABORADOR", NM_COLABORADOR);

                DAL_MIS AcessaDadosMis = new DAL.DAL_MIS();
                return AcessaDadosMis.ConsultaSQL(sqlcommand);
            }
            catch (Exception ex)
            {
                throw new Exception("BLL.WEB.HelpDeskOcorrencia_001: " + ex.Message, ex);
            }
        }
        public void InformeArquivoRemessaTarde(string strResultado)
        {
            //Cria objeto com dados do e-mail.
            //Antigo SMTP MANDIC string servidor = "200.219.212.5";
            string servidor = "177.70.110.120";
            int porta = 587;

            System.Net.Mail.SmtpClient objSmtp = new System.Net.Mail.SmtpClient(servidor, porta);
            System.Net.NetworkCredential Credencial = new System.Net.NetworkCredential("operador@exitobr.com.br", "exitobrasil");
            System.Net.Mail.MailMessage objEmail = new System.Net.Mail.MailMessage();
            objSmtp.UseDefaultCredentials = false;
            objSmtp.Credentials = Credencial;
            objEmail.From = new System.Net.Mail.MailAddress("[CHAMADO] Operador <operador@exitobr.com.br>");

            //Define os destinatários do e-mail.
            objEmail.To.Add("Chamado <chamados@exitobr.com.br>");
            //objEmail.To.Add("Chamado <jmorais@exitobr.com.br>");

            //Define a prioridade do e-mail.
            objEmail.Priority = System.Net.Mail.MailPriority.High;

            //Define o formato do e-mail HTML (caso não queira HTML alocar valor false)
            objEmail.IsBodyHtml = true;

            //Define título do e-mail.
            objEmail.Subject = "[Span] ABERTURA DE CHAMADO - OPERAÇÃO";

            objEmail.Body = "ABERTURA DE CHAMADO <br><br>"
                          + strResultado
                          + "<br><br>Data do processamento: " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");

            objEmail.SubjectEncoding = System.Text.Encoding.GetEncoding("ISO-8859-1");
            objEmail.BodyEncoding = System.Text.Encoding.GetEncoding("ISO-8859-1");
            objEmail.Priority = System.Net.Mail.MailPriority.Normal;

            try
            {
                objSmtp.Send(objEmail);
                objEmail.Dispose();
            }
            catch (Exception ex)
            {
                objEmail.Dispose();
                throw new Exception("Envio Email:" + ex.Message, ex);
            }
        }

        #endregion

        #region  Acompanhamento dos processo diarios

        public DataSet Acompanhamento()
        {
            try
            {
                SqlCommand sqlcommand = new SqlCommand();
                sqlcommand.CommandType = CommandType.Text;
                sqlcommand.CommandText = "SELECT NR_PROCESSO, NM_PROCESSO, CONCAT(HR_INI_EXEC, ' as ', HR_FIM_EXEC) AS FAIXA, NR_INT_EXEC AS INTERVALO, DT_INI_ULT_EXEC, DT_FIM_ULT_EXEC, ISNULL(NR_TMP_ULT_EXEC, 0) AS [NR_TMP_ULT_EXEC], DT_PRO_EXEC, TP_STS_EXEC \r\n" +
                                          " FROM TBL_ROT_PROC_DIARIO \r\n" +
                                          "WHERE 1=1\r\n " +
                                          "AND (NM_PROCESSO NOT LIKE 'ROBO%' OR NR_PROCESSO = 16)\r\n " +
                                          "AND (DT_FIM_ULT_EXEC >= '2024-11-12' OR TP_STS_EXEC IN ('1', 'E', '0')) \r\n" +
                                           "ORDER BY NM_PROCESSO";

                DAL_PROC AcessaDadosProc = new DAL.DAL_PROC();
                return AcessaDadosProc.ConsultaSQL(sqlcommand);
            }
            catch (Exception ex)
            {
                throw new Exception("BLL.WEB.HelpDeskOcorrencia_011: " + ex.Message, ex);
            }
        }

        public DataSet AcompanhamentoCob()
        {
            try
            {
                SqlCommand sqlcommand = new SqlCommand();
                sqlcommand.CommandType = CommandType.Text;
                sqlcommand.CommandText = "SELECT NR_PROCESSO, NM_PROCESSO, CONCAT(HR_INI_EXEC, ' as ', HR_FIM_EXEC) AS FAIXA, NR_INT_EXEC AS INTERVALO, DT_INI_ULT_EXEC, DT_FIM_ULT_EXEC, ISNULL(NR_TMP_ULT_EXEC, 0) AS [NR_TMP_ULT_EXEC], DT_PRO_EXEC, TP_STS_EXEC, DS_ARGUMENTO AS ARQUIVO FROM TBL_ROT_PROC_DIARIO WHERE NM_PROCESSO NOT LIKE 'ROBO%' ORDER BY TP_STS_EXEC, NM_PROCESSO";

                DAL_PROC AcessaDadosProc = new DAL.DAL_PROC();
                return AcessaDadosProc.ConsultaSQL(sqlcommand);
            }
            catch (Exception ex)
            {
                throw new Exception("BLL.WEB.HelpDeskOcorrencia_011: " + ex.Message, ex);
            }
        }
        
        public DataSet LogProcessamento()
        {
            try
            {
                SqlCommand sqlcommand = new SqlCommand();
                sqlcommand.CommandType = CommandType.Text;
                sqlcommand.CommandText = "SELECT DT_INICIO AS DT_PROCESSAMENTO, NM_ROTINA AS NM_STATUS FROM LOG_MAI_DISCADOR_ACERTO ORDER BY 1 DESC";
                                         
                DAL_PROC AcessaDadosProc = new DAL.DAL_PROC();
                return AcessaDadosProc.ConsultaSQL(sqlcommand);
            }
            catch (Exception ex)
            {
                throw new Exception("BLL.WEB.HelpDeskOcorrencia_016: " + ex.Message, ex);
            }
        }

        public DataSet ReiniciaProcessamento(int NR_PROCESSO)
        {
            try
            {
                SqlCommand sqlcommand = new SqlCommand();
                sqlcommand.CommandType = CommandType.Text;
                sqlcommand.CommandText = "UPDATE TBL_ROT_PROC_DIARIO SET \n"
                                        + "      DT_INI_ULT_EXEC = NULL \n"
                                        + "    , DT_FIM_ULT_EXEC = NULL \n"
                                        + "    , NR_TMP_ULT_EXEC = NULL \n"
                                        + "    , DT_PRO_EXEC = NULL \n"
                                        + "    , TP_STS_EXEC = '0' \n"
                                        + "WHERE \n"
                                        + "      NR_PROCESSO = @NR_PROCESSO \n";

                sqlcommand.Parameters.AddWithValue("@NR_PROCESSO", NR_PROCESSO);
                DAL_PROC AcessaDadosProc = new DAL.DAL_PROC();
                return AcessaDadosProc.ConsultaSQL(sqlcommand);
            }
            catch (Exception ex)
            {
                throw new Exception("BLL.WEB.HelpDeskOcorrencia_011: " + ex.Message, ex);
            }
        }

        public DataSet LogProcessamentoDiario()
        {
            try
            {
                SqlCommand sqlcommand = new SqlCommand();
                sqlcommand.CommandType = CommandType.Text;
                sqlcommand.CommandText = "SELECT * FROM LOG_EXEC_PROCESSO_DIARIO";

                DAL_MIS AcessaDadosMis = new DAL.DAL_MIS();
                return AcessaDadosMis.ConsultaSQL(sqlcommand);
            }
            catch (Exception ex)
            {
                throw new Exception("BLL.WEB.HelpDeskOcorrencia_017: " + ex.Message, ex);
            }
        }

        #endregion


        #region  Acompanhamento dos processo diarios Robôs

        public DataSet Acompanhamento2()
        {
            try
            {
                SqlCommand sqlcommand = new SqlCommand();
                sqlcommand.CommandType = CommandType.Text;
                sqlcommand.CommandText = "SELECT NR_PROCESSO, NM_PROCESSO, CONCAT(HR_INI_EXEC, ' as ', HR_FIM_EXEC) AS FAIXA, NR_INT_EXEC AS INTERVALO, DT_INI_ULT_EXEC, DT_FIM_ULT_EXEC, ISNULL(NR_TMP_ULT_EXEC, 0) AS [NR_TMP_ULT_EXEC], DT_PRO_EXEC, TP_STS_EXEC FROM TBL_ROT_PROC_DIARIO WHERE NM_PROCESSO like 'ROBO%' ORDER BY NM_PROCESSO";

                DAL_PROC AcessaDadosProc = new DAL.DAL_PROC();
                return AcessaDadosProc.ConsultaSQL(sqlcommand);
            }
            catch (Exception ex)
            {
                throw new Exception("BLL.WEB.HelpDeskOcorrencia_012: " + ex.Message, ex);
            }
        }

        public DataSet ReiniciaProcessamento2(int NR_PROCESSO)
        {
            try
            {
                SqlCommand sqlcommand = new SqlCommand();
                sqlcommand.CommandType = CommandType.Text;
                sqlcommand.CommandText = "UPDATE TBL_ROT_PROC_DIARIO SET \n"
                                        + "      DT_INI_ULT_EXEC = NULL \n"
                                        + "    , DT_FIM_ULT_EXEC = NULL \n"
                                        + "    , NR_TMP_ULT_EXEC = NULL \n"
                                        + "    , DT_PRO_EXEC = NULL \n"
                                        + "    , TP_STS_EXEC = 0 \n"
                                        + "WHERE \n"
                                        + "      NR_PROCESSO = @NR_PROCESSO \n";

                sqlcommand.Parameters.AddWithValue("@NR_PROCESSO", NR_PROCESSO);
                DAL_PROC AcessaDadosProc = new DAL.DAL_PROC();
                return AcessaDadosProc.ConsultaSQL(sqlcommand);
            }
            catch (Exception ex)
            {
                throw new Exception("BLL.WEB.HelpDeskOcorrencia_012: " + ex.Message, ex);
            }
        }
        #endregion
    }
}