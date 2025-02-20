using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Intranet.BLL.RET.Tabulacao
{
    public class Tabulacao
    {
        public DataSet GeraRelatorioTabulacao(int DT_REFERENCIA)
        {
            try
            {
                SqlCommand sqlcommand = new SqlCommand();
                sqlcommand.CommandType = CommandType.StoredProcedure;
                sqlcommand.CommandText = "SP_RET_RELATORIO_TABULACAO_CE_NAO_CE";
                sqlcommand.Parameters.AddWithValue("@DT_REFERENCIA", DT_REFERENCIA);

                Intranet.DAL.DAL_MIS_N AcessaDadosMisN = new Intranet.DAL.DAL_MIS_N();

                DataTable dtTabulacao = AcessaDadosMisN.ConsultaSQL(sqlcommand).Tables[0];

                DataTable dtHorario = dtTabulacao.DefaultView.ToTable(true, "DS_HORA_ACIONAMENTO");
                DataTable dtTabulac = dtTabulacao.DefaultView.ToTable(true, "GR_CE", "CD_ACAO", "NM_ACAO");

                DataTable dtTab01 = new DataTable();
                dtTab01.Columns.Add("CD_ACAO", typeof(string));
                dtTab01.Columns.Add("DS_ACAO", typeof(string));
                dtTab01.Columns.Add("TOTAL_A", typeof(Int32));
                dtTab01.Columns.Add("TOTAL_B", typeof(string));

                DataTable dtTab02 = new DataTable();
                dtTab02.Columns.Add("CD_ACAO", typeof(string));
                dtTab02.Columns.Add("DS_ACAO", typeof(string));
                dtTab02.Columns.Add("TOTAL_A", typeof(Int32));
                dtTab02.Columns.Add("TOTAL_B", typeof(string));

                foreach (DataRow dr in dtHorario.Rows)
                {
                    dtTab01.Columns.Add(dr[0].ToString() + "_A", typeof(Int32));
                    dtTab01.Columns.Add(dr[0].ToString() + "_B", typeof(string));

                    dtTab02.Columns.Add(dr[0].ToString() + "_A", typeof(Int32));
                    dtTab02.Columns.Add(dr[0].ToString() + "_B", typeof(string));
                }

                var dtCeN = dtTabulacao.AsEnumerable().Where(f => f.Field<byte>("GR_CE") == 0);
                var dtCeS = dtTabulacao.AsEnumerable().Where(f => f.Field<byte>("GR_CE") == 1);

                TimeSpan tm;
                foreach (DataRow dr1 in dtTabulac.Rows)
                {
                    int GR_CE = int.Parse(dr1[0].ToString());
                    int CD_ACAO = (int)dr1[1];
                    string NM_ACAO = dr1[2].ToString();


                    if (GR_CE == 1)
                    {
                        DataRow NovaLinha = dtTab01.NewRow();
                        NovaLinha[0] = string.Format("{0:0000}", CD_ACAO);
                        NovaLinha[1] = NM_ACAO;
                        NovaLinha[2] = dtCeS.Where(f => f.Field<Int32>("CD_ACAO") == CD_ACAO).Sum(s => s.Field<Int32>("QT_ACIONAMENTO"));

                        tm = TimeSpan.FromSeconds((dtCeS.Where(f => f.Field<Int32>("CD_ACAO") == CD_ACAO).Sum(s => s.Field<Int64>("NR_TEMPO_FALANDO"))));
                        NovaLinha[3] = string.Format("{0:00}:{1:00}:{2:00}", tm.TotalHours, tm.Minutes, tm.Seconds);

                        foreach (DataRow dr2 in dtHorario.Rows)
                        {
                            var RESULT = dtCeS.Where(f => f.Field<Int32>("CD_ACAO") == CD_ACAO && f.Field<string>("DS_HORA_ACIONAMENTO") == dr2[0].ToString()).Select(s => new { QT_ACIONAMENTO = s.Field<Int32>("QT_ACIONAMENTO"), NR_TEMPO_FALANDO = s.Field<Int64>("NR_TEMPO_FALANDO") }).ToList();
                            NovaLinha[dr2[0].ToString() + "_A"] = RESULT.Count == 0 ? 0 : RESULT[0].QT_ACIONAMENTO;
                            tm = RESULT.Count == 0 ? TimeSpan.FromSeconds(0) : TimeSpan.FromSeconds((double)RESULT[0].NR_TEMPO_FALANDO);
                            NovaLinha[dr2[0].ToString() + "_B"] = string.Format("{0:00}:{1:00}:{2:00}", tm.TotalHours, tm.Minutes, tm.Seconds);
                        }
                        dtTab01.Rows.Add(NovaLinha);
                    }
                    else
                    {
                        DataRow NovaLinha = dtTab02.NewRow();
                        NovaLinha[0] = string.Format("{0:0000}", CD_ACAO);
                        NovaLinha[1] = NM_ACAO;
                        NovaLinha[2] = dtCeN.Where(f => f.Field<Int32>("CD_ACAO") == CD_ACAO).Sum(s => s.Field<Int32>("QT_ACIONAMENTO"));
                        tm = TimeSpan.FromSeconds((dtCeN.Where(f => f.Field<Int32>("CD_ACAO") == CD_ACAO).Sum(s => s.Field<Int64>("NR_TEMPO_FALANDO"))));
                        NovaLinha[3] = string.Format("{0:00}:{1:00}:{2:00}", tm.TotalHours, tm.Minutes, tm.Seconds);

                        foreach (DataRow dr2 in dtHorario.Rows)
                        {
                            var RESULT = dtCeN.Where(f => f.Field<Int32>("CD_ACAO") == CD_ACAO && f.Field<string>("DS_HORA_ACIONAMENTO") == dr2[0].ToString()).Select(s => new { QT_ACIONAMENTO = s.Field<Int32>("QT_ACIONAMENTO"), NR_TEMPO_FALANDO = s.Field<Int64>("NR_TEMPO_FALANDO") }).ToList();
                            NovaLinha[dr2[0].ToString() + "_A"] = RESULT.Count == 0 ? 0 : RESULT[0].QT_ACIONAMENTO;
                            tm = RESULT.Count == 0 ? TimeSpan.FromSeconds(0) : TimeSpan.FromSeconds((double)RESULT[0].NR_TEMPO_FALANDO);
                            NovaLinha[dr2[0].ToString() + "_B"] = string.Format("{0:00}:{1:00}:{2:00}", tm.TotalHours, tm.Minutes, tm.Seconds);
                        }
                        dtTab02.Rows.Add(NovaLinha);
                    }
                }

                DataSet dsTabulacao = new DataSet();
                dsTabulacao.Tables.Add(dtTab01);
                dsTabulacao.Tables.Add(dtTab02);

                return dsTabulacao;
            }
            catch (Exception ex)
            {
                throw new Exception("RET.Tabulacao_001: " + ex.Message, ex);
            }
        }

        public DataSet GeraRelatorioTabulacaoOperador(string DT_INI, string DT_FIM, string NR_COORDENADOR, string NR_SUPERVISOR)
        {
            try
            {
                SqlCommand sqlcommand = new SqlCommand();
                sqlcommand.CommandType = CommandType.StoredProcedure;
                sqlcommand.CommandText = "SP_WEB_ACOMPANHAMENTO_TABULACAO_OPERADOR";

                sqlcommand.Parameters.AddWithValue("@DT_INI", DT_INI);
                sqlcommand.Parameters.AddWithValue("@DT_FIM", DT_FIM);
                sqlcommand.Parameters.AddWithValue("@NR_COORDENADOR", NR_COORDENADOR);
                sqlcommand.Parameters.AddWithValue("@NR_SUPERVISOR", NR_SUPERVISOR);

                Intranet.DAL.DAL_MIS_N AcessaDadosMisN = new Intranet.DAL.DAL_MIS_N();

                DataSet dsTabulacao = AcessaDadosMisN.ConsultaSQL(sqlcommand);
                DataSet dsFinal = new DataSet();
                if ((dsTabulacao.Tables.Count > 0) && (dsTabulacao.Tables[0].Rows.Count > 0) && (dsTabulacao.Tables[0].DefaultView.ToTable(true, "GR_CE").Rows.Count > 1))
                {
                    DataTable dtTabulacao = dsTabulacao.Tables[0];
                    DataTable dtTabulac = dtTabulacao.DefaultView.ToTable(true, "GR_CE", "CD_ACAO", "NM_ACAO");
                    DataView dv = dtTabulac.DefaultView;
                    dv.Sort = "GR_CE, CD_ACAO ASC";
                    dtTabulac = dv.ToTable();

                    DataTable dtNovaTabulacao = new DataTable();
                    dtNovaTabulacao.Columns.Add("NM_COLABORADOR", typeof(string));
                    dtNovaTabulacao.Columns.Add("NM_LOGIN_OLOS", typeof(string));

                    bool bN_CE = false;
                    byte iN_CE = 0;
                    foreach (DataRow dr in dtTabulac.Rows)
                    {
                        if (((byte)dr["GR_CE"] != iN_CE) && (bN_CE == false))
                        {
                            dtNovaTabulacao.Columns.Add("TOTAL_N_A", typeof(Int32));
                            dtNovaTabulacao.Columns.Add("TOTAL_N_B", typeof(string));
                            dtNovaTabulacao.Columns.Add("TOTAL_N_Q", typeof(string));
                            bN_CE = true;
                        }

                        int CD_ACAO = (Int32)dr["CD_ACAO"];
                        dtNovaTabulacao.Columns.Add(string.Format("{0:0000}", CD_ACAO) + "_A", typeof(Int32));
                        dtNovaTabulacao.Columns.Add(string.Format("{0:0000}", CD_ACAO) + "_B", typeof(string));
                    }

                    dtNovaTabulacao.Columns.Add("TOTAL_S_A", typeof(Int32));
                    dtNovaTabulacao.Columns.Add("TOTAL_S_B", typeof(string));
                    dtNovaTabulacao.Columns.Add("TOTAL_S_Q", typeof(string));

                    DataTable dtCol;
                    string ColunaChave = "";

                    if (NR_COORDENADOR == "0")
                    {
                        dtCol = dtTabulacao.DefaultView.ToTable(true, "NM_COORDENADOR");
                        dtCol.Columns.Add("NM_LOGIN_OLOS", typeof(string));
                        ColunaChave = "NM_COORDENADOR";
                    }
                    else if (NR_SUPERVISOR == "0")
                    {
                        dtCol = dtTabulacao.DefaultView.ToTable(true, "NM_SUPERVISOR");
                        dtCol.Columns.Add("NM_LOGIN_OLOS", typeof(string));
                        ColunaChave = "NM_SUPERVISOR";
                    }
                    else
                    {
                        dtCol = dtTabulacao.DefaultView.ToTable(true, "NM_COLABORADOR", "NM_LOGIN_OLOS");
                        ColunaChave = "NM_COLABORADOR";
                    }

                    var lstColaboradorTabulacao = dtTabulacao.AsEnumerable()
                           .GroupBy(g => new { NM_COLABORADOR = g.Field<string>(ColunaChave), CD_ACAO = g.Field<Int32>("CD_ACAO"), GR_CE = g.Field<byte>("GR_CE") })
                           .Select(sel => new { NM_COLABORADOR = sel.Key.NM_COLABORADOR, GR_CE = sel.Key.GR_CE, CD_ACAO = sel.Key.CD_ACAO, QT_ACIONAMENTO = sel.Sum(sum => sum.Field<Int32>("QT_ACIONAMENTO")), NR_TEMPO_FALANDO = sel.Sum(sum => sum.Field<Int64>("NR_TEMPO_FALANDO")) }).ToList();

                    foreach (DataRow drCol in dtCol.Rows)
                    {
                        var find = lstColaboradorTabulacao.Where(f => f.NM_COLABORADOR == drCol[ColunaChave].ToString()).ToList();
                        DataRow NovaLinha = dtNovaTabulacao.NewRow();

                        NovaLinha["NM_COLABORADOR"] = drCol[ColunaChave].ToString();
                        NovaLinha["NM_LOGIN_OLOS"] = drCol["NM_LOGIN_OLOS"].ToString();

                        foreach (var v in find)
                        {
                            NovaLinha[string.Format("{0:0000}", v.CD_ACAO) + "_A"] = v.QT_ACIONAMENTO;
                            TimeSpan tm = TimeSpan.FromSeconds((double)v.NR_TEMPO_FALANDO);
                            NovaLinha[string.Format("{0:0000}", v.CD_ACAO) + "_B"] = string.Format("{0:00}:{1:00}:{2:00}", tm.TotalHours, tm.Minutes, tm.Seconds);
                        }

                        var Total = find
                            .GroupBy(g => new { GR_CE = g.GR_CE })
                            .Select(sel => new { GR_CE = sel.Key.GR_CE, QT_ACIONAMENTO = sel.Sum(sum => sum.QT_ACIONAMENTO), NR_TEMPO_FALANDO = sel.Sum(sum => sum.NR_TEMPO_FALANDO) }).ToList();

                        foreach (var t in Total)
                        {
                            switch (t.GR_CE)
                            {
                                case 0: { NovaLinha["TOTAL_N_A"] = t.QT_ACIONAMENTO; TimeSpan tm = TimeSpan.FromSeconds((double)t.NR_TEMPO_FALANDO); NovaLinha["TOTAL_N_B"] = string.Format("{0:00}:{1:00}:{2:00}", tm.TotalHours, tm.Minutes, tm.Seconds); break; }
                                case 1: { NovaLinha["TOTAL_S_A"] = t.QT_ACIONAMENTO; TimeSpan tm = TimeSpan.FromSeconds((double)t.NR_TEMPO_FALANDO); NovaLinha["TOTAL_S_B"] = string.Format("{0:00}:{1:00}:{2:00}", tm.TotalHours, tm.Minutes, tm.Seconds); break; }
                            }
                        }
                        dtNovaTabulacao.Rows.Add(NovaLinha);
                    }

                    BLL.CalculoQuartil calculo = new BLL.CalculoQuartil();
                    dtNovaTabulacao = calculo.Quartil(dtNovaTabulacao, "TOTAL_N_A", "TOTAL_N_Q", "DESC");
                    dtNovaTabulacao = calculo.Quartil(dtNovaTabulacao, "TOTAL_S_A", "TOTAL_S_Q", "ASC");
                    
                    dv = dtNovaTabulacao.DefaultView;
                    dv.Sort = "NM_COLABORADOR ASC";
                    dtNovaTabulacao = dv.ToTable();

                    dsFinal.Tables.Add(dtTabulac.Copy());
                    dsFinal.Tables.Add(dtNovaTabulacao);
                }
                return dsFinal;
            }
            catch (Exception ex)
            {
                throw new Exception("RET.Tabulacao_002: " + ex.Message, ex);
            }
        }
    }

    public class TabulacaoGriViewTemplate : ITemplate
    {
        private DataControlRowType templateType;
        private string columnName;
        private string TipoRelatorio;

        public TabulacaoGriViewTemplate(DataControlRowType type, string colname, string Tipo)
        {
            templateType = type;
            columnName = colname;
            TipoRelatorio = Tipo;
        }

        public void InstantiateIn(System.Web.UI.Control container)
        {
            // Create the content for the different row types.
            switch (templateType)
            {
                case DataControlRowType.DataRow:
                    {
                        if (columnName.Contains("_Q"))
                        {
                            Image img = new Image();
                            img.DataBinding += new EventHandler(this.NovaColunaImg_DataBinding);
                            container.Controls.Add(img);
                        }
                        else
                        {
                            Label NovaColuna = new Label();
                            NovaColuna.DataBinding += new EventHandler(this.NovaColunaLbl_DataBinding);
                            container.Controls.Add(NovaColuna);
                        }
                        break;
                    }
                case DataControlRowType.Header:
                    {
                        Label NovaColuna = new Label();

                        NovaColuna.Text = columnName.Replace("_A", "");
                        NovaColuna.ToolTip = TipoRelatorio;
                        container.Controls.Add(NovaColuna);
                        break;
                    }
                default: break;
            }
        }

        private void NovaColunaLbl_DataBinding(Object sender, EventArgs e)
        {
            Label l = (Label)sender;
            GridViewRow row = (GridViewRow)l.NamingContainer;

            if (TipoRelatorio == "1")
            {
                l.Text = string.Format("{0:N0}", DataBinder.Eval(row.DataItem, columnName).ToString());
                l.ToolTip = DataBinder.Eval(row.DataItem, columnName.Replace("_A", "_B")).ToString();
            }
            else
            {
                l.Text = string.Format("{0:N0}", DataBinder.Eval(row.DataItem, columnName).ToString());
                l.ToolTip = DataBinder.Eval(row.DataItem, columnName.Replace("_B", "_A")).ToString();
            }
        }
        private void NovaColunaImg_DataBinding(Object sender, EventArgs e)
        {
            Image img = (Image)sender;
            GridViewRow row = (GridViewRow)img.NamingContainer;

            img.ID = columnName;
            img.ToolTip = string.Format("{0:N0}", DataBinder.Eval(row.DataItem, columnName).ToString());
        }
    }
}