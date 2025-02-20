using Intranet_NEW.Controllers.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;

namespace Intranet.BLL.WEB
{
    public class Equipe
    {
        public DataSet CarregaEquipe(string NR_USUARIO)
        {
            try
            {
                SqlCommand sqlcommand = new SqlCommand();
                sqlcommand.CommandType = CommandType.StoredProcedure;
                sqlcommand.Parameters.AddWithValue("@NR_USUARIO", NR_USUARIO);

                sqlcommand.CommandText = "SP_WEB_EQUIPE_LOAD";

                DAL_MIS AcessaDadosMis = new DAL.DAL_MIS();
                return AcessaDadosMis.ConsultaSQL(sqlcommand);
            }
            catch (Exception ex)
            {
                throw new Exception("BLL.WEB.Equipe_001: " + ex.Message, ex);
            }
        }

        public DataSet ListaComparacaoEquipe()
        {
            try
            {
                SqlCommand sqlcommand = new SqlCommand();
                sqlcommand.CommandType = CommandType.StoredProcedure;

                sqlcommand.CommandText = "SP_WEB_EQUIPE_COMPARACAO";

                DAL_MIS AcessaDadosMis = new DAL.DAL_MIS();
                return AcessaDadosMis.ConsultaSQL(sqlcommand);
            }
            catch (Exception ex)
            {
                throw new Exception("BLL.WEB.Equipe_002: " + ex.Message, ex);
            }
        }

        public DataSet ListaOperadorOrigemEquipe(string NR_SUPERVISOR)
        {
            try
            {
                SqlCommand sqlcommand = new SqlCommand();
                sqlcommand.CommandType = CommandType.StoredProcedure;
                sqlcommand.Parameters.AddWithValue("@NR_SUPERVISOR", NR_SUPERVISOR);

                sqlcommand.CommandText = "SP_WEB_EQUIPE_LISTA_OPERDOR";

                DAL_MIS AcessaDadosMis = new DAL.DAL_MIS();
                return AcessaDadosMis.ConsultaSQL(sqlcommand);
            }
            catch (Exception ex)
            {
                throw new Exception("BLL.WEB.Equipe_003: " + ex.Message, ex);
            }
        }

        public int MovimentaOperador(string NR_SUPERVISOR_DESTINO, List<string> ListaOperador)
        {
            try
            {
                string temp = "";
                foreach (string t in ListaOperador)
                    temp += ", " + t;

                SqlCommand sqlcommand = new SqlCommand();
                sqlcommand.CommandType = CommandType.Text;
                sqlcommand.Parameters.AddWithValue("@NR_SUPERVISOR_DESTINO", NR_SUPERVISOR_DESTINO);
                sqlcommand.CommandText = "UPDATE TMP_WEB_EQUIPE_OPERADOR SET \n"
                                        + "      NR_COORDENADOR = B.NR_COORDENADOR \n"
                                        + "    , NR_SUPERVISOR  = B.NR_COLABORADOR \n"
                                        + "FROM \n"
                                        + "    TMP_WEB_EQUIPE_OPERADOR A \n"
                                        + "        INNER JOIN TBL_WEB_COLABORADOR_DADOS B ON B.NR_COLABORADOR = @NR_SUPERVISOR_DESTINO \n"
                                        + "WHERE \n"
                                        + "    A.NR_COLABORADOR IN (" + temp.Substring(2) + ") \n";

                DAL_MIS AcessaDadosMis = new DAL.DAL_MIS();
                return AcessaDadosMis.ExecutaComandoSQL(sqlcommand);
            }
            catch (Exception ex)
            {
                throw new Exception("BLL.WEB.Equipe_004: " + ex.Message, ex);
            }
        }
        
        public int ConfirmaMovimentacao(string NR_USUARIO)
        {
            try
            {
                SqlCommand sqlcommand = new SqlCommand();
                sqlcommand.CommandType = CommandType.Text;
                sqlcommand.Parameters.AddWithValue("@NR_USUARIO", NR_USUARIO);

                sqlcommand.CommandText = "UPDATE TMP_WEB_EQUIPE_OPERADOR SET TP_FINALIZADO = 1 \n\n"
                                        + "SELECT \n"
                                        + "      B.NM_COLABORADOR \n"
                                        + "    , (SELECT TOP 1 NM_COLABORADOR FROM TBL_WEB_COLABORADOR_DADOS WHERE NR_COLABORADOR = B.NR_SUPERVISOR) AS [SUPERVISOR_ANTERIOR] \n"
                                        + "    , (SELECT TOP 1 NM_COLABORADOR FROM TBL_WEB_COLABORADOR_DADOS WHERE NR_COLABORADOR = A.NR_SUPERVISOR) AS [SUPERVISOR_ATUAL] \n"
                                        + "INTO #TMP \n"
                                        + "FROM TMP_WEB_EQUIPE_OPERADOR A \n"
                                        + "    INNER JOIN TBL_WEB_COLABORADOR_DADOS B ON A.NR_COLABORADOR = B.NR_COLABORADOR \n\n"

                                        + "SELECT * FROM #TMP WHERE SUPERVISOR_ANTERIOR <> SUPERVISOR_ATUAL \n"
                                        + "SELECT NM_COLABORADOR, NM_EMAIL FROM TBL_WEB_COLABORADOR_DADOS WHERE NR_COLABORADOR = @NR_USUARIO \n"
                                        + "DROP TABLE #TMP \n";

                DAL_MIS AcessaDadosMis = new DAL.DAL_MIS();
                DataSet ds = AcessaDadosMis.ConsultaSQL(sqlcommand);

                StringBuilder strTable = new StringBuilder();

                if ((ds.Tables.Count > 0) && (ds.Tables[0].Rows.Count > 0))
                {
                    strTable.AppendLine("<table style=\"border-collapse: collapse;\">");
                    strTable.AppendLine("<tr style\"border:1px solid #808080;\">");
                    strTable.AppendLine("<td style=\"border:1px solid #808080; text-align:center; font-weight:bold;\">" + ds.Tables[0].Columns[0].ColumnName + "</td>");
                    strTable.AppendLine("<td style=\"border:1px solid #808080; text-align:center; font-weight:bold;\">" + ds.Tables[0].Columns[1].ColumnName + "</td>");
                    strTable.AppendLine("<td style=\"border:1px solid #808080; text-align:center; font-weight:bold;\">" + ds.Tables[0].Columns[2].ColumnName + "</td>");
                    strTable.AppendLine("</tr>");

                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        strTable.AppendLine("<tr>");
                        for (int c = 0; c < ds.Tables[0].Columns.Count; c++)
                            strTable.AppendLine("<td style=\"border:1px solid #808080; padding-left:15px; padding-right:15px;\">" + dr[c].ToString() + "</td>");
                        strTable.AppendLine("</tr>");
                    }
                    strTable.AppendLine("</table>");
                }

                System.Net.Mail.MailMessage objEmail = new System.Net.Mail.MailMessage();

                objEmail.To.Add("Camila Boldo<camila.boldo@exitobr.com.br>");
                objEmail.To.Add("Floriza Costa<fcosta@exitobr.com.br>");
                objEmail.To.Add("Fabio Fonseca Dos Santos<fabio.fonseca@exitobr.com.br>");

                objEmail.To.Add("Edna Pires<edna@exitobr.com.br>");
                objEmail.To.Add("Josielho Morais<jmorais@exitobr.com.br>");
                objEmail.Bcc.Add("Wellington<wcarvalho@exitobr.com.br>");

                if (ds.Tables[1].Rows[0][1].ToString() != "")
                    objEmail.To.Add(string.Format("{0}<{1}>", ds.Tables[1].Rows[0][0].ToString(), ds.Tables[1].Rows[0][1].ToString()));


                objEmail.Subject = "[INTRANET] Alteração no ABS online";

                string conteudo = "";
                conteudo += "<b>Prezados(as),</b>";
                conteudo += "<br><br>";
                conteudo += "Foi realizado as seguintes alterações no ABS online:";
                conteudo += "<br>";
                conteudo += "As alterações realizadas no ABS online serão aplicadas após o expediente de hoje.";
                conteudo += "<br>";
                conteudo += "Alterado por: <b>" + ds.Tables[1].Rows[0][0].ToString() + "</b>";
                conteudo += "<br><br>";

                conteudo += strTable.ToString();

                conteudo += "<br><br>";
                conteudo += "E-mail enviado pelo sistema, por favor não responder.";
                conteudo += "<br>";
                conteudo += "Enviado em: " + DateTime.Now.ToString("dd/MM/yyyy HH:mm");
                objEmail.Body = conteudo;

                MAIL mail = new Intranet.DAL.MAIL();

                if (mail.Enviar(objEmail, "BLL.WEB.Equipe_005_A") == 0)
                    return (1);

                return (0);
            }
            catch (Exception ex)
            {
                throw new Exception("BLL.WEB.Equipe_005: " + ex.Message, ex);
            }
        }
        
        public void LimpaTabelaEquipe()
        {
            try
            {
                SqlCommand sqlcommand = new SqlCommand();
                sqlcommand.CommandType = CommandType.Text;

                sqlcommand.CommandText = "IF (OBJECT_ID('TMP_WEB_EQUIPE_OPERADOR') IS NOT NULL) \n"
                                        + "    DROP TABLE TMP_WEB_EQUIPE_OPERADOR \n";

                DAL_MIS AcessaDadosMis = new DAL.DAL_MIS();
                AcessaDadosMis.ExecutaComandoSQL(sqlcommand);
            }
            catch (Exception ex)
            {
                throw new Exception("BLL.WEB.Equipe_006: " + ex.Message, ex);
            }
        }
    }
}