using Intranet_NEW.Controllers.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Intranet.BLL.RH
{
    public class FolhaPonto
    {
        public DataTable GeraFolhaPonto(string NR_COLABORADOR, string NR_CPF, string DT_MESREF, bool Impressao)
        {
            try
            {
                SqlCommand sqlcommand = new SqlCommand();
                sqlcommand.CommandType = CommandType.Text;
                sqlcommand.CommandText = "SELECT  \n"
                                        + "	  NR_CPF \n"
                                        + "	, DT_MESREF \n"
                                        + "	, DT_DIA_MARC \n"
                                        + "	, NM_DIA_MARC \n"
                                        + "	, HR_ENTRADA \n"
                                        + "	, HR_SAIDA_ALMOCO \n"
                                        + "	, HR_RETOR_ALMOCO \n"
                                        + "	, HR_SAIDA_NR17_1 \n"
                                        + "	, HR_RETOR_NR17_1 \n"
                                        + "	, HR_SAIDA_NR17_2 \n"
                                        + "	, HR_RETOR_NR17_2 \n"
                                        + "	, HR_SAIDA \n"
                                        + "	, TP_MARC \n"
                                        + "--	, HR_TOTAL \n"
                                        + "--	, HR_DIARIA \n"
                                        + "--	, TP_SALDO \n"
                                        + "--	, HR_SALDO \n"
                                        + "--	, NM_OBSERVACAO \n"
                                        + "FROM TBL_WEB_RH_ESPELHO_PONTO_COLABORADOR \n"
                                        + "WHERE NR_CPF = @NR_CPF AND DT_MESREF = @DT_MESREF \n"
                                        + "ORDER BY DT_DIA_MARC \n\n"

                                        + "SELECT DISTINCT DT_MARCACAO \n"
                                        + "FROM TBL_WEB_RH_PONTO_JUSTIFICATIVA \n"
                                        + "WHERE  \n"
                                        + "	NR_SOLICITANTE = @NR_COLABORADOR \n"
                                        + "AND DT_MESREF   = @DT_MESREF \n";

                sqlcommand.Parameters.AddWithValue("@NR_CPF", NR_CPF);
                sqlcommand.Parameters.AddWithValue("@NR_COLABORADOR", NR_COLABORADOR);
                sqlcommand.Parameters.AddWithValue("@DT_MESREF", DT_MESREF);

                DAL_MIS AcessaDadosMis = new Intranet.DAL.DAL_MIS();

                DataSet ds = AcessaDadosMis.ConsultaSQL(sqlcommand);

                if ((ds.Tables.Count > 0) && (ds.Tables[0].Rows.Count > 0))
                {
                    return GeraTabelaHtml(ds.Tables[0], ds.Tables[1], Impressao);
                }
                return new DataTable();
            }
            catch (Exception ex)
            {
                throw new Exception("FAT.CmdReversao_001: " + ex.Message, ex);
            }
        }

        private DataTable GeraTabelaHtml(DataTable dtEspelho, DataTable dtSolicitacao, bool Impressao)
        {
            try
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("COL01", typeof(string));
                dt.Columns.Add("COL02", typeof(string));
                dt.Columns.Add("COL03", typeof(string));
                dt.Columns.Add("COL04", typeof(string));
                dt.Columns.Add("COL05", typeof(string));
                dt.Columns.Add("COL06", typeof(string));
                dt.Columns.Add("COL07", typeof(string));
                dt.Columns.Add("COL08", typeof(string));
                dt.Columns.Add("COL09", typeof(string));
                dt.Columns.Add("COL10", typeof(string));
                dt.Columns.Add("COL11", typeof(string));

                DateTime DT_VAR = ((DateTime)dtEspelho.Rows[0]["DT_DIA_MARC"]);
                int DT_DIA;
                // MONTA OS DIAS ANTERIORES DA SEMANA DO PRIMEIRO DIA DE APONTAMENTO
                if (Impressao == false)
                {
                    DT_DIA = (int)DT_VAR.DayOfWeek;
                    if (DT_DIA != 0)
                    {
                        DT_DIA = DT_DIA - 1;
                        while (DT_DIA != 0)
                        {
                            dt.Rows.Add(GeraLihaTabelaVazio(dt.NewRow(), DT_VAR.AddDays(-DT_DIA)));
                            DT_DIA--;
                        }
                    }
                }

                foreach (DataRow dr in dtEspelho.Rows)
                {
                    DT_VAR = ((DateTime)dr["DT_DIA_MARC"]);
                    dt.Rows.Add(GeraLihaTabela(dt.NewRow(), dr, dtSolicitacao));
                    if (Impressao == false)
                    {
                        if (((DateTime)dr["DT_DIA_MARC"]).DayOfWeek == DayOfWeek.Sunday)
                            dt.Rows.Add(GeraLihaTabelaSoma(dt.NewRow()));
                    }
                }

                if (Impressao == false)
                {
                    // MONTA OS DIAS POSTERIORES DA SEMANA DO ULTIMO DIA DE APONTAMENTO
                    DT_DIA = (int)DT_VAR.DayOfWeek;
                    while ((int)DT_VAR.DayOfWeek != 0)
                    {
                        DT_VAR = DT_VAR.AddDays(1);
                        dt.Rows.Add(GeraLihaTabelaVazio(dt.NewRow(), DT_VAR));
                    }
                }
                return dt;
            }
            catch (Exception ex)
            {
                throw new Exception("FAT.CmdReversao_001: " + ex.Message, ex);
            }
        }

        private DataRow GeraLihaTabela(DataRow NovaLinha, DataRow drPonto, DataTable dtSolicitacao)
        {
            DateTime DT_MARC = (DateTime)drPonto["DT_DIA_MARC"];

            NovaLinha["COL01"] = DT_MARC.ToString("dd/MM/yyyy");
            NovaLinha["COL02"] = drPonto["NM_DIA_MARC"].ToString();
            NovaLinha["COL11"] = drPonto["TP_MARC"].ToString();

            if ((DateTime)drPonto["DT_DIA_MARC"] > DateTime.Now.AddDays(-1))
            {
                NovaLinha["COL11"] = "DF";
                return (NovaLinha);
            }

            string retorno = VerificaFeriadoNacional((DateTime)drPonto["DT_DIA_MARC"]);
            if (retorno == "FR")
            {
                NovaLinha["COL11"] = "FR";
            }

            double dd = 0;
            for (int c = 4; c < drPonto.Table.Columns.Count - 1; c++)
            {

                string Valor = "-";

                if (NovaLinha["COL11"].ToString() == "I")
                    Valor = " ";

                if (double.TryParse(drPonto[c].ToString(), out dd))
                {
                    double tmp = (Int64)drPonto[c];
                    TimeSpan ts = TimeSpan.FromSeconds(tmp);
                    Valor = ts.ToString().Substring(0, 5);
                }
                else if (drPonto[c].ToString() != "")
                    Valor = drPonto[c].ToString();

                NovaLinha[c - 2] = Valor;
            }

            if (dtSolicitacao.Select(string.Format("DT_MARCACAO = '{0}'", DT_MARC)).Length > 0)
                NovaLinha["COL11"] = "JP";

            return (NovaLinha);
        }

        private DataRow GeraLihaTabelaVazio(DataRow NovaLinha, DateTime DiaVazio)
        {
            NovaLinha["COL01"] = DiaVazio.ToString("dd/MM/yyyy");
            NovaLinha["COL02"] = DiaVazio.ToString("dddd", new System.Globalization.CultureInfo("pt-BR")).Substring(0, 3).ToUpper();

            NovaLinha["COL11"] = "VZ";

            for (int c = 2; c < 10; c++)
                NovaLinha[c] = "-";

            return (NovaLinha);
        }

        private DataRow GeraLihaTabelaSoma(DataRow NovaLinha)
        {
            NovaLinha["COL01"] = "SEMANA";
            NovaLinha["COL02"] = "-";
            NovaLinha["COL11"] = "SM";

            for (int c = 2; c < 10; c++)
                NovaLinha[c] = "-";

            return (NovaLinha);
        }

        protected string VerificaFeriadoNacional(DateTime Data)
        {
            string feriado = "";

            List<DateTime> ListaFeriadoNacional = new List<DateTime>();

            ListaFeriadoNacional.Add(DateTime.Parse("01/01/1900")); // ano novo
            ListaFeriadoNacional.Add(DateTime.Parse("21/04/1900")); // tiradentes
            ListaFeriadoNacional.Add(DateTime.Parse("01/05/1900")); // dia do trabalho
            ListaFeriadoNacional.Add(DateTime.Parse("09/07/1900")); // independencia do brasil,
            ListaFeriadoNacional.Add(DateTime.Parse("07/09/1900")); // independencia do brasil,
            ListaFeriadoNacional.Add(DateTime.Parse("12/10/1900")); // nossa senhora aparecida
            ListaFeriadoNacional.Add(DateTime.Parse("02/11/1900")); // finados
            ListaFeriadoNacional.Add(DateTime.Parse("15/11/1900")); // proclamacao da repubilca
            ListaFeriadoNacional.Add(DateTime.Parse("20/11/1900")); // conciencia ,
            ListaFeriadoNacional.Add(DateTime.Parse("25/12/1900")); // natal

            ListaFeriadoNacional.Add(DateTime.Parse("26/12/2015")); // SABADO LIBERADO PELO CLIENTE
            ListaFeriadoNacional.Add(DateTime.Parse("02/01/2016")); // SABADO LIBERADO PELO CLIENTE

            for (int i = 0; i < ListaFeriadoNacional.Count && feriado == ""; i++)
                if (((Data.Day == ListaFeriadoNacional[i].Day) && (Data.Month == ListaFeriadoNacional[i].Month) && (ListaFeriadoNacional[i].Year == 1900))
                    || (ListaFeriadoNacional[i] == Data))
                {
                    feriado = "FR";
                    Data = Data.AddDays(1);
                }
            return feriado;
        }
    }

    /*
    public class FolhaPonto2
    {
        public System.Web.UI.HtmlControls.HtmlGenericControl GeraFolhaPonto(string NR_CPF, string DT_MESREF)
        {
            try
            {
                SqlCommand sqlcommand = new SqlCommand();
                sqlcommand.CommandType = CommandType.Text;
                sqlcommand.CommandText = "SELECT  \n"
                                        + "	  NR_CPF \n"
                                        + "	, DT_MESREF \n"
                                        + "	, DT_DIA_MARC \n"
                                        + "	, NM_DIA_MARC \n"
                                        + "	, HR_ENTRADA \n"
                                        + "	, HR_SAIDA_ALMOCO \n"
                                        + "	, HR_RETOR_ALMOCO \n"
                                        + "	, HR_SAIDA_NR17_1 \n"
                                        + "	, HR_RETOR_NR17_1 \n"
                                        + "	, HR_SAIDA_NR17_2 \n"
                                        + "	, HR_RETOR_NR17_2 \n"
                                        + "	, HR_SAIDA \n"
                                        + "	, HR_TOTAL \n"
                                        + "	, HR_DIARIA \n"
                                        + "--	, TP_SALDO \n"
                                        + "	, HR_SALDO \n"
                                        + "	, NM_OBSERVACAO \n"
                                        + "	, TP_MARC \n"
                                        + "FROM TBL_WEB_RH_ESPELHO_PONTO_COLABORADOR \n"
                                        + "WHERE NR_CPF = @NR_CPF AND DT_MESREF = @DT_MESREF";

                sqlcommand.Parameters.AddWithValue("@NR_CPF", NR_CPF);
                sqlcommand.Parameters.AddWithValue("@DT_MESREF", DT_MESREF);

                Intranet.DAL.DAL_MIS_N AcessaDadosMis = new Intranet.DAL.DAL_MIS_N();

                DataSet ds = AcessaDadosMis.ConsultaSQL(sqlcommand);

                if ((ds.Tables.Count > 0) && (ds.Tables[0].Rows.Count > 0))
                {
                    System.Web.UI.HtmlControls.HtmlGenericControl HtmlTableFinal = GeraTabelaHtml(ds.Tables[0]);
                    return HtmlTableFinal;
                }
                return new System.Web.UI.HtmlControls.HtmlGenericControl("div");
            }
            catch (Exception ex)
            {
                throw new Exception("FAT.CmdReversao_001: " + ex.Message, ex);
            }
        }

        private System.Web.UI.HtmlControls.HtmlGenericControl GeraTabelaHtml(DataTable dtEspelho)
        {
            try
            {
                System.Web.UI.HtmlControls.HtmlGenericControl HtmlTable = new System.Web.UI.HtmlControls.HtmlGenericControl("table");
                HtmlTable.Attributes["runat"] = "server";
                HtmlTable.Attributes["id"] = "Table";
                HtmlTable.Attributes["class"] = "GridView-Table";

                System.Web.UI.HtmlControls.HtmlGenericControl HtmlCol;

                #region Monta primeira linha da tabela

                System.Web.UI.HtmlControls.HtmlGenericControl HtmlRow = new System.Web.UI.HtmlControls.HtmlGenericControl("tr");
                HtmlRow.Attributes["class"] = "GridView-Header";

                HtmlCol = new System.Web.UI.HtmlControls.HtmlGenericControl("th");
                HtmlCol.Attributes["colspan"] = "4";
                HtmlCol.InnerHtml = "&nbsp;";
                HtmlRow.Controls.Add(HtmlCol);

                HtmlCol = new System.Web.UI.HtmlControls.HtmlGenericControl("th");
                HtmlCol.Attributes["colspan"] = "2";
                HtmlCol.InnerHtml = "INT. LANCHE / ALMOÇO";
                HtmlRow.Controls.Add(HtmlCol);

                HtmlCol = new System.Web.UI.HtmlControls.HtmlGenericControl("th");
                HtmlCol.Attributes["colspan"] = "2";
                HtmlCol.InnerHtml = "NR17-I / TREINAMENTO";
                HtmlRow.Controls.Add(HtmlCol);

                HtmlCol = new System.Web.UI.HtmlControls.HtmlGenericControl("th");
                HtmlCol.Attributes["colspan"] = "2";
                HtmlCol.InnerHtml = "NR17-II";
                HtmlRow.Controls.Add(HtmlCol);

                HtmlCol = new System.Web.UI.HtmlControls.HtmlGenericControl("th");
                //HtmlCol.Attributes["colspan"] = "5";
                HtmlCol.InnerHtml = "&nbsp;";
                HtmlRow.Controls.Add(HtmlCol);

                HtmlTable.Controls.Add(HtmlRow);

                #endregion

                #region Monta segunda linha da tabela

                HtmlRow = new System.Web.UI.HtmlControls.HtmlGenericControl("tr");
                HtmlRow.Attributes["class"] = "GridView-SubHeader";

                HtmlCol = new System.Web.UI.HtmlControls.HtmlGenericControl("th");
                HtmlCol.Attributes["colspan"] = "3";
                HtmlCol.InnerHtml = "DATA";
                HtmlRow.Controls.Add(HtmlCol);

                HtmlCol = new System.Web.UI.HtmlControls.HtmlGenericControl("th");
                HtmlCol.InnerHtml = "ENTRADA";
                HtmlRow.Controls.Add(HtmlCol);

                HtmlCol = new System.Web.UI.HtmlControls.HtmlGenericControl("th");
                HtmlCol.InnerHtml = "SAÍDA";
                HtmlRow.Controls.Add(HtmlCol);

                HtmlCol = new System.Web.UI.HtmlControls.HtmlGenericControl("th");
                HtmlCol.InnerHtml = "RETORNO";
                HtmlRow.Controls.Add(HtmlCol);

                HtmlCol = new System.Web.UI.HtmlControls.HtmlGenericControl("th");
                HtmlCol.InnerHtml = "SAÍDA";
                HtmlRow.Controls.Add(HtmlCol);

                HtmlCol = new System.Web.UI.HtmlControls.HtmlGenericControl("th");
                HtmlCol.InnerHtml = "RETORNO";
                HtmlRow.Controls.Add(HtmlCol);

                HtmlCol = new System.Web.UI.HtmlControls.HtmlGenericControl("th");
                HtmlCol.InnerHtml = "SAÍDA";
                HtmlRow.Controls.Add(HtmlCol);

                HtmlCol = new System.Web.UI.HtmlControls.HtmlGenericControl("th");
                HtmlCol.InnerHtml = "RETORNO";
                HtmlRow.Controls.Add(HtmlCol);

                HtmlCol = new System.Web.UI.HtmlControls.HtmlGenericControl("th");
                HtmlCol.InnerHtml = "SAÍDA";
                HtmlRow.Controls.Add(HtmlCol);

                //HtmlCol = new System.Web.UI.HtmlControls.HtmlGenericControl("th");
                //HtmlCol.InnerHtml = "TOTAL";
                //HtmlRow.Controls.Add(HtmlCol);

                //HtmlCol = new System.Web.UI.HtmlControls.HtmlGenericControl("th");
                //HtmlCol.InnerHtml = "CONTRATADO";
                //HtmlRow.Controls.Add(HtmlCol);

                //HtmlCol = new System.Web.UI.HtmlControls.HtmlGenericControl("th");
                //HtmlCol.InnerHtml = "SALDO";
                //HtmlRow.Controls.Add(HtmlCol);

                //HtmlCol = new System.Web.UI.HtmlControls.HtmlGenericControl("th");
                //HtmlCol.InnerHtml = "OBSERVAÇÃO";
                //HtmlRow.Controls.Add(HtmlCol);

                HtmlTable.Controls.Add(HtmlRow);

                #endregion

                // MONTA OS DIAS ANTERIORES DA SEMANA DO PRIMEIRO DIA DE APONTAMENTO
                DateTime DT_VAR = ((DateTime)dtEspelho.Rows[0]["DT_DIA_MARC"]);
                int DT_DIA = (int)DT_VAR.DayOfWeek;
                if (DT_DIA != 0)
                {
                    DT_DIA = DT_DIA - 1;
                    while (DT_DIA != 0)
                    {
                        HtmlTable.Controls.Add(GeraLihaTabelaHtmlVazio(DT_VAR.AddDays(-DT_DIA)));
                        DT_DIA--;
                    }
                }


                DataRow drVazio;
                foreach (DataRow dr in dtEspelho.Rows)
                {
                    DT_VAR = ((DateTime)dr["DT_DIA_MARC"]);
                    HtmlTable.Controls.Add(GeraLihaTabelaHtml(dr));

                    if (((DateTime)dr["DT_DIA_MARC"]).DayOfWeek == DayOfWeek.Sunday)
                    {
                        drVazio = dtEspelho.NewRow();
                        HtmlTable.Controls.Add(GeraLihaTabelaHtmlSoma(drVazio));
                    }
                }


                // MONTA OS DIAS POSTERIORES DA SEMANA DO ULTIMO DIA DE APONTAMENTO
                DT_DIA = (int)DT_VAR.DayOfWeek;
                while ((int)DT_VAR.DayOfWeek != 0)
                {
                    DT_VAR = DT_VAR.AddDays(1);
                    HtmlTable.Controls.Add(GeraLihaTabelaHtmlVazio(DT_VAR));
                }

                //drVazio = dtEspelho.NewRow();
                //HtmlTable.Controls.Add(GeraLihaTabelaHtmlSoma(drVazio));


                return HtmlTable;
            }
            catch (Exception ex)
            {
                throw new Exception("FAT.CmdReversao_001: " + ex.Message, ex);
            }
        }

        private System.Web.UI.HtmlControls.HtmlGenericControl GeraLihaTabelaHtml(DataRow drPonto)
        {
            System.Web.UI.HtmlControls.HtmlGenericControl HtmlTableRow = new System.Web.UI.HtmlControls.HtmlGenericControl("tr");
            HtmlTableRow.Attributes["class"] = "GridView-Rows";

            System.Web.UI.HtmlControls.HtmlGenericControl HtmlCol = new System.Web.UI.HtmlControls.HtmlGenericControl("td");
            HtmlCol.Attributes["class"] = "GridView-Cells Centralizado";

            System.Web.UI.WebControls.CheckBox chk = new System.Web.UI.WebControls.CheckBox();
            chk.ID = "chk_" + ((DateTime)drPonto["DT_DIA_MARC"]).ToString("yyyyMMdd");
            HtmlCol.Controls.Add(chk);

            HtmlTableRow.Controls.Add(HtmlCol);

            HtmlCol = new System.Web.UI.HtmlControls.HtmlGenericControl("td");
            HtmlCol.Attributes["class"] = "GridView-Cells Centralizado";
            HtmlCol.InnerHtml = ((DateTime)drPonto["DT_DIA_MARC"]).ToString("dd/MM/yyyy");
            HtmlTableRow.Controls.Add(HtmlCol);

            HtmlCol = new System.Web.UI.HtmlControls.HtmlGenericControl("td");
            HtmlCol.Attributes["class"] = "GridView-Cells Centralizado";
            HtmlCol.InnerHtml = drPonto["NM_DIA_MARC"].ToString();
            HtmlTableRow.Controls.Add(HtmlCol);

            if ((drPonto["TP_MARC"].ToString() != "C") && ((DateTime)drPonto["DT_DIA_MARC"] < DateTime.Now.AddDays(-1)))
                HtmlTableRow.Attributes["style"] = "color: red;";

            double dd = 0;
            for (int c = 4; c < drPonto.Table.Columns.Count - 5; c++)
            {
                HtmlCol = new System.Web.UI.HtmlControls.HtmlGenericControl("td");
                HtmlCol.Attributes["class"] = "GridView-Cells Centralizado";

                string Valor = "-";
                if (double.TryParse(drPonto[c].ToString(), out dd))
                {
                    double tmp = (Int64)drPonto[c];
                    TimeSpan ts = TimeSpan.FromSeconds(tmp);
                    Valor = ts.ToString().Substring(0, 5);
                }
                else if (drPonto[c].ToString() != "")
                    Valor = drPonto[c].ToString();

                HtmlCol.InnerHtml = Valor;
                HtmlTableRow.Controls.Add(HtmlCol);
            }

            return (HtmlTableRow);
        }

        private System.Web.UI.HtmlControls.HtmlGenericControl GeraLihaTabelaHtmlSoma(DataRow drSoma)
        {
            System.Web.UI.HtmlControls.HtmlGenericControl HtmlTableRow = new System.Web.UI.HtmlControls.HtmlGenericControl("tr");
            HtmlTableRow.Attributes["class"] = "GridView-Rows Total";

            System.Web.UI.HtmlControls.HtmlGenericControl HtmlCol = new System.Web.UI.HtmlControls.HtmlGenericControl("td");
            HtmlCol.Attributes["class"] = "GridView-Cells Centralizado";
            HtmlTableRow.Controls.Add(HtmlCol);

            HtmlCol = new System.Web.UI.HtmlControls.HtmlGenericControl("td");
            HtmlCol.Attributes["class"] = "GridView-Cells Centralizado";
            HtmlCol.InnerHtml = "SEMANA";
            HtmlTableRow.Controls.Add(HtmlCol);

            HtmlCol = new System.Web.UI.HtmlControls.HtmlGenericControl("td");
            HtmlCol.Attributes["class"] = "GridView-Cells Centralizado";
            HtmlCol.InnerHtml = "-";
            HtmlTableRow.Controls.Add(HtmlCol);

            for (int c = 4; c < drSoma.Table.Columns.Count - 5; c++)
            {
                HtmlCol = new System.Web.UI.HtmlControls.HtmlGenericControl("td");
                HtmlCol.Attributes["class"] = "GridView-Cells Centralizado";
                HtmlCol.InnerHtml = drSoma[c].ToString();
                HtmlTableRow.Controls.Add(HtmlCol);
            }

            return (HtmlTableRow);
        }

        private System.Web.UI.HtmlControls.HtmlGenericControl GeraLihaTabelaHtmlVazio(DateTime DiaVazio)
        {
            System.Web.UI.HtmlControls.HtmlGenericControl HtmlTableRow = new System.Web.UI.HtmlControls.HtmlGenericControl("tr");
            HtmlTableRow.Attributes["class"] = "GridView-Rows";


            System.Web.UI.HtmlControls.HtmlGenericControl HtmlCol = new System.Web.UI.HtmlControls.HtmlGenericControl("td");
            HtmlCol.Attributes["class"] = "GridView-Cells Centralizado";
            HtmlTableRow.Controls.Add(HtmlCol);

            HtmlCol = new System.Web.UI.HtmlControls.HtmlGenericControl("td");
            HtmlCol.Attributes["class"] = "GridView-Cells Centralizado";
            HtmlCol.InnerHtml = DiaVazio.ToString("dd/MM/yyyy");
            HtmlTableRow.Controls.Add(HtmlCol);

            HtmlCol = new System.Web.UI.HtmlControls.HtmlGenericControl("td");
            HtmlCol.Attributes["class"] = "GridView-Cells Centralizado";
            HtmlCol.InnerHtml = DiaVazio.ToString("dddd", new System.Globalization.CultureInfo("pt-BR")).Substring(0, 3).ToUpper();
            HtmlTableRow.Controls.Add(HtmlCol);

            for (int c = 0; c < 8; c++)
            {
                HtmlCol = new System.Web.UI.HtmlControls.HtmlGenericControl("td");
                HtmlCol.Attributes["class"] = "GridView-Cells Centralizado";
                HtmlCol.InnerHtml = "-";
                HtmlTableRow.Controls.Add(HtmlCol);
            }
            return (HtmlTableRow);
        }
    }

    */
}
