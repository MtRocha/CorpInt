using Intranet_NEW.Controllers.DAL;
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
    public class TAB
    {
        public DataSet GeraRelatorioTabulacao(int DT_REFERENCIA, string CANAL)
        {
            try
            {
                SqlCommand sqlcommand = new SqlCommand();
                sqlcommand.CommandType = CommandType.StoredProcedure;
                sqlcommand.CommandText = "SP_RET_RELATORIO_TABULACAO_CE_NAO_CE";
                sqlcommand.Parameters.AddWithValue("@DT_REFERENCIA", DT_REFERENCIA);
                sqlcommand.Parameters.AddWithValue("@TP_CANAL", CANAL);

                DAL_MIS AcessaDadosMisN = new Intranet.DAL.DAL_MIS();

                DataSet dsTabulacao = AcessaDadosMisN.ConsultaSQL(sqlcommand);
                if (dsTabulacao.Tables.Count > 0)
                {
                    DataTable dtHorario = dsTabulacao.Tables[0].DefaultView.ToTable(true, "DS_HORA_ACIONAMENTO");
                    DataTable dtTabulac = dsTabulacao.Tables[0].DefaultView.ToTable(true, "GR_CE", "CD_ACAO", "NM_ACAO");

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

                    DataTable dtTabulacao = dsTabulacao.Tables[0];
                    var dtCeN = dtTabulacao.AsEnumerable().Where(f => f.Field<byte>("GR_CE") == 0);
                    var dtCeS = dtTabulacao.AsEnumerable().Where(f => f.Field<byte>("GR_CE") == 1);

                    TimeSpan tm;
                    DataView dv;
                    foreach (DataRow dr1 in dtTabulac.Rows)
                    {
                        int GR_CE = int.Parse(dr1[0].ToString());
                        string CD_ACAO = (string)dr1[1];
                        string NM_ACAO = dr1[2].ToString();

                        if (GR_CE == 1)
                        {
                            if (CD_ACAO == "9999")
                            {
                                dv = dtTab01.DefaultView;
                                dv.Sort = "TOTAL_A desc";
                                dtTab01 = dv.ToTable();
                            }

                            DataRow NovaLinha = dtTab01.NewRow();
                            NovaLinha[0] = string.Format("{0:0000}", CD_ACAO);
                            NovaLinha[1] = NM_ACAO;
                            NovaLinha[2] = dtCeS.Where(f => f.Field<string>("CD_ACAO") == CD_ACAO).Sum(s => s.Field<Int32>("QT_ACIONAMENTO"));

                            tm = TimeSpan.FromSeconds((dtCeS.Where(f => f.Field<string>("CD_ACAO") == CD_ACAO).Sum(s => s.Field<Int64>("NR_TEMPO_FALANDO"))));
                            NovaLinha[3] = string.Format("{0:00}:{1:00}:{2:00}", tm.TotalHours, tm.Minutes, tm.Seconds);

                            foreach (DataRow dr2 in dtHorario.Rows)
                            {
                                var RESULT = dtCeS.Where(f => f.Field<string>("CD_ACAO") == CD_ACAO && f.Field<string>("DS_HORA_ACIONAMENTO") == dr2[0].ToString()).Select(s => new { QT_ACIONAMENTO = s.Field<Int32>("QT_ACIONAMENTO"), NR_TEMPO_FALANDO = s.Field<Int64>("NR_TEMPO_FALANDO") }).ToList();
                                NovaLinha[dr2[0].ToString() + "_A"] = RESULT.Count == 0 ? 0 : RESULT[0].QT_ACIONAMENTO;
                                tm = RESULT.Count == 0 ? TimeSpan.FromSeconds(0) : TimeSpan.FromSeconds((double)RESULT[0].NR_TEMPO_FALANDO);
                                NovaLinha[dr2[0].ToString() + "_B"] = string.Format("{0:00}:{1:00}:{2:00}", tm.TotalHours, tm.Minutes, tm.Seconds);
                            }
                            dtTab01.Rows.Add(NovaLinha);
                        }
                        else
                        {
                            if (CD_ACAO == "9999")
                            {
                                dv = dtTab02.DefaultView;
                                dv.Sort = "TOTAL_A desc";
                                dtTab02 = dv.ToTable();
                            }

                            DataRow NovaLinha = dtTab02.NewRow();
                            NovaLinha[0] = string.Format("{0:0000}", CD_ACAO);
                            NovaLinha[1] = NM_ACAO;
                            NovaLinha[2] = dtCeN.Where(f => f.Field<string>("CD_ACAO") == CD_ACAO).Sum(s => s.Field<Int32>("QT_ACIONAMENTO"));
                            tm = TimeSpan.FromSeconds((dtCeN.Where(f => f.Field<string>("CD_ACAO") == CD_ACAO).Sum(s => s.Field<Int64>("NR_TEMPO_FALANDO"))));
                            NovaLinha[3] = string.Format("{0:00}:{1:00}:{2:00}", tm.TotalHours, tm.Minutes, tm.Seconds);

                            foreach (DataRow dr2 in dtHorario.Rows)
                            {
                                var RESULT = dtCeN.Where(f => f.Field<string>("CD_ACAO") == CD_ACAO && f.Field<string>("DS_HORA_ACIONAMENTO") == dr2[0].ToString()).Select(s => new { QT_ACIONAMENTO = s.Field<Int32>("QT_ACIONAMENTO"), NR_TEMPO_FALANDO = s.Field<Int64>("NR_TEMPO_FALANDO") }).ToList();
                                NovaLinha[dr2[0].ToString() + "_A"] = RESULT.Count == 0 ? 0 : RESULT[0].QT_ACIONAMENTO;
                                tm = RESULT.Count == 0 ? TimeSpan.FromSeconds(0) : TimeSpan.FromSeconds((double)RESULT[0].NR_TEMPO_FALANDO);
                                NovaLinha[dr2[0].ToString() + "_B"] = string.Format("{0:00}:{1:00}:{2:00}", tm.TotalHours, tm.Minutes, tm.Seconds);
                            }
                            dtTab02.Rows.Add(NovaLinha);
                        }
                    }
                    DataSet dsTabulacao01 = new DataSet();
                    dsTabulacao01.Tables.Add(dtTab01);
                    dsTabulacao01.Tables.Add(dtTab02);
                    return dsTabulacao01;
                }
                else
                    return new DataSet();
            }
            catch (Exception ex)
            {
                throw new Exception("RET.Tabulacao_001: " + ex.Message, ex);
            }
        }

        public DataSet GeraRelatorioTabulacaoOperadorRes(string DT_INI, string DT_FIM, string NR_COORDENADOR, string NR_SUPERVISOR)
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

                DAL_MIS AcessaDadosMisN = new Intranet.DAL.DAL_MIS();

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
                    dtNovaTabulacao.Columns.Add("DS_HORA_ACIONAMENTO", typeof(string));

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

        public DataSet GeraRelatorioTabulacaoOperadorDet(string DT_INI, string DT_FIM, string NR_COORDENADOR, string NR_SUPERVISOR)
        {
            try
            {
                SqlCommand sqlcommand = new SqlCommand();
                sqlcommand.CommandType = CommandType.StoredProcedure;
                sqlcommand.CommandText = "SP_WEB_ACOMPANHAMENTO_TABULACAO_OPERADOR_HORA";

                sqlcommand.Parameters.AddWithValue("@DT_INI", DT_INI);
                sqlcommand.Parameters.AddWithValue("@DT_FIM", DT_FIM);
                sqlcommand.Parameters.AddWithValue("@NR_COORDENADOR", NR_COORDENADOR);
                sqlcommand.Parameters.AddWithValue("@NR_SUPERVISOR", NR_SUPERVISOR);

                DAL_MIS AcessaDadosMisN = new Intranet.DAL.DAL_MIS();

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
                    dtNovaTabulacao.Columns.Add("DS_HORA_ACIONAMENTO", typeof(string));

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
                        dtCol = dtTabulacao.DefaultView.ToTable(true, "NM_COORDENADOR", "DS_HORA_ACIONAMENTO");
                        dtCol.Columns.Add("NM_LOGIN_OLOS", typeof(string));
                        ColunaChave = "NM_COORDENADOR";
                    }
                    else if (NR_SUPERVISOR == "0")
                    {
                        dtCol = dtTabulacao.DefaultView.ToTable(true, "NM_SUPERVISOR", "DS_HORA_ACIONAMENTO");
                        dtCol.Columns.Add("NM_LOGIN_OLOS", typeof(string));
                        ColunaChave = "NM_SUPERVISOR";
                    }
                    else
                    {
                        dtCol = dtTabulacao.DefaultView.ToTable(true, "NM_COLABORADOR", "NM_LOGIN_OLOS", "DS_HORA_ACIONAMENTO");
                        ColunaChave = "NM_COLABORADOR";
                    }

                    var lstColaboradorTabulacao = dtTabulacao.AsEnumerable()
                           .GroupBy(g => new { NM_COLABORADOR = g.Field<string>(ColunaChave), DS_HORA_ACIONAMENTO = g.Field<string>("DS_HORA_ACIONAMENTO"), CD_ACAO = g.Field<Int32>("CD_ACAO"), GR_CE = g.Field<byte>("GR_CE") })
                           .Select(sel => new { NM_COLABORADOR = sel.Key.NM_COLABORADOR, DS_HORA_ACIONAMENTO = sel.Key.DS_HORA_ACIONAMENTO, GR_CE = sel.Key.GR_CE, CD_ACAO = sel.Key.CD_ACAO, QT_ACIONAMENTO = sel.Sum(sum => sum.Field<Int32>("QT_ACIONAMENTO")), NR_TEMPO_FALANDO = sel.Sum(sum => sum.Field<Int64>("NR_TEMPO_FALANDO")) }).ToList();

                    foreach (DataRow drCol in dtCol.Rows)
                    {
                        var find = lstColaboradorTabulacao.Where(f => f.NM_COLABORADOR == drCol[ColunaChave].ToString() && f.DS_HORA_ACIONAMENTO == drCol["DS_HORA_ACIONAMENTO"].ToString()).ToList();
                        DataRow NovaLinha = dtNovaTabulacao.NewRow();

                        NovaLinha["NM_COLABORADOR"] = drCol[ColunaChave].ToString();
                        NovaLinha["NM_LOGIN_OLOS"] = drCol["NM_LOGIN_OLOS"].ToString();
                        NovaLinha["DS_HORA_ACIONAMENTO"] = drCol["DS_HORA_ACIONAMENTO"].ToString();

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
                    dv.Sort = "NM_COLABORADOR ASC, DS_HORA_ACIONAMENTO ASC";
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

        public DataSet GeraRelatorioTempoTabulacao(string DT, string NR_COORDENADOR, string NR_SUPERVISOR, string NM_TURNO)
        {
            try
            {
                SqlCommand sqlcommand = new SqlCommand();
                sqlcommand.CommandType = CommandType.StoredProcedure;
                sqlcommand.CommandText = "SP_RET_RELATORIO_TEMPO_TABULACAO";

                sqlcommand.Parameters.AddWithValue("@DT_REFERENCIA", DT);
                sqlcommand.Parameters.AddWithValue("@NR_COORDENADOR", NR_COORDENADOR);
                sqlcommand.Parameters.AddWithValue("@NR_SUPERVISOR", NR_SUPERVISOR);
                sqlcommand.Parameters.AddWithValue("@NM_TURNO", NM_TURNO);

                DAL_MIS AcessaDadosMisN = new Intranet.DAL.DAL_MIS();

                DataSet dsTabulacao = AcessaDadosMisN.ConsultaSQL(sqlcommand);

                return dsTabulacao;
            }

            catch (Exception ex)
            {
                throw new Exception("RET.Tabulacao_003: " + ex.Message, ex);
            }
        }

        public DataSet GeraRelatorioCritica(string DT, string HRINI, string HRFIM, string TIPO_OC)
        {
            try
            {
                SqlCommand sqlcommand = new SqlCommand();
                sqlcommand.CommandType = CommandType.StoredProcedure;
                sqlcommand.CommandText = "SP_RET_CRITICA_WEB";

                sqlcommand.Parameters.AddWithValue("@DT_INIC", DT.ToString());
                sqlcommand.Parameters.AddWithValue("@HR_INIC", HRINI.ToString());
                sqlcommand.Parameters.AddWithValue("@HR_FIM", HRFIM.ToString());
                sqlcommand.Parameters.AddWithValue("@TIPO_OC", TIPO_OC.ToString());
                DAL_MIS AcessaDadosMisN = new Intranet.DAL.DAL_MIS();

                DataSet dsTabulacao = AcessaDadosMisN.ConsultaSQL(sqlcommand);

                return dsTabulacao;
            }

            catch (Exception ex)
            {
                throw new Exception("RET.Tabulacao_003: " + ex.Message, ex);
            }
        }


        public DataTable ListaTabulacao()
        {
            try
            {
                SqlCommand sqlcommand = new SqlCommand();
                sqlcommand.CommandType = CommandType.Text;
                sqlcommand.CommandText = "SELECT \n"
                                        + "    DISTINCT CD_ACAO,NM_ACAO  \n"
                                        + "FROM TBL_"+DateTime.Now.ToString("yyyyMM")+"_RET_ACIONAMENTO_HORA_TABULACAO  \n"
                                        + "ORDER BY NM_ACAO \n";

                DAL_PROC AcessaDadosProc = new Intranet.DAL.DAL_PROC();
                return AcessaDadosProc.ConsultaSQL(sqlcommand).Tables[0];
            }
            catch (Exception ex)
            {
                throw new Exception("RET.Tabulacao.ADO_004: " + ex.Message, ex);
            }
        }

        public DataTable ListaDetalhe(string CD_ACAO)
        {
            try
            {

                SqlCommand sqlcommand = new SqlCommand();
                sqlcommand.CommandType = CommandType.Text;
                if (CD_ACAO != "0")
                {
                    sqlcommand.CommandText = "SELECT \n"
                                       + "    DISTINCT ID_DETALHE,NM_VALOR  \n"
                                       + "FROM TBL_RET_ACIONAMENTO_ACAO_DETALHE \n"
                                       + "WHERE ID_ACAO = " + CD_ACAO + "\n"
                                       + "ORDER BY NM_VALOR \n";

                }
                else
                {
                    sqlcommand.CommandText = "SELECT \n"
                                      + "    DISTINCT ID_DETALHE,NM_VALOR  \n"
                                      + "FROM TBL_RET_ACIONAMENTO_ACAO_DETALHE \n"
                                      + "ORDER BY NM_VALOR \n";

                }


                DAL_PROC AcessaDadosProc = new Intranet.DAL.DAL_PROC();
                return AcessaDadosProc.ConsultaSQL(sqlcommand).Tables[0];
            }
            catch (Exception ex)
            {
                throw new Exception("RET.Tabulacao.ADO_004: " + ex.Message, ex);
            }
        }


        public DataSet GeraRelatorioTabulacaoProduto(int DT, int CDACAO)
        {
            try
            {
                SqlCommand sqlcommand = new SqlCommand();
                sqlcommand.CommandType = CommandType.StoredProcedure;
                sqlcommand.CommandText = "SP_WEB_TABULACAO_PRODUTO";

                sqlcommand.Parameters.AddWithValue("@DATA", DT);
                sqlcommand.Parameters.AddWithValue("@CD_ACAO", CDACAO);

                DAL_PROC AcessaDadosProc = new Intranet.DAL.DAL_PROC();

                DataSet dsTabulacao = AcessaDadosProc.ConsultaSQL(sqlcommand);

                return dsTabulacao;
            }

            catch (Exception ex)
            {
                throw new Exception("RET.Tabulacao_003: " + ex.Message, ex);
            }
        }

        public DataSet GeraPesquisaTabulacao(string DT_INIC, string DT_FIM, int NR_COORDENADOR, int NR_SUPERVISOR, string ID_DETALHE, int TABULACAO, int TEMPO_INIC, int TEMPO_FIM)
        {
            try
            {
                SqlCommand sqlcommand = new SqlCommand();
                sqlcommand.CommandType = CommandType.StoredProcedure;
                sqlcommand.CommandText = "SP_RET_PESQUISA_TABULACAO";

                sqlcommand.Parameters.AddWithValue("@DT_INIC", DT_INIC);
                sqlcommand.Parameters.AddWithValue("@DT_FIM", DT_FIM);
                sqlcommand.Parameters.AddWithValue("@NR_SUPERVISOR", NR_SUPERVISOR);
                sqlcommand.Parameters.AddWithValue("@NR_COORDENADOR", NR_COORDENADOR);
                sqlcommand.Parameters.AddWithValue("@NR_TABULACAO", TABULACAO);
                sqlcommand.Parameters.AddWithValue("@TEMPO_INIC", TEMPO_INIC);
                sqlcommand.Parameters.AddWithValue("@TEMPO_FIM", TEMPO_FIM);
                sqlcommand.Parameters.AddWithValue("@NM_DETALHE", ID_DETALHE);

                DAL_MIS AcessaDadosMisN = new Intranet.DAL.DAL_MIS();

                DataSet dsTabulacao = AcessaDadosMisN.ConsultaSQL(sqlcommand);

                return dsTabulacao;
            }

            catch (Exception ex)
            {
                throw new Exception("RET.Tabulacao_004: " + ex.Message, ex);
            }
        }

        public object CD_OPERACAO { get; set; }

        public DataSet GeraPesquisaTabulacaoDuplicada(DateTime DT_INIC, DateTime DT_FIM)
        {
            try
            {
                SqlCommand sqlcommand = new SqlCommand();
                sqlcommand.CommandType = CommandType.StoredProcedure;
                sqlcommand.CommandText = "SP_RET_TABULACAO_DUPLICADA";

                sqlcommand.Parameters.AddWithValue("@DT_INIC", DT_INIC);
                sqlcommand.Parameters.AddWithValue("@DT_FIM", DT_FIM);

                DAL_MIS AcessaDadosMisN = new Intranet.DAL.DAL_MIS();
                DataSet dsTabulacao = AcessaDadosMisN.ConsultaSQL(sqlcommand);

                return dsTabulacao;
            }

            catch (Exception ex)
            {
                throw new Exception("RET.Tabulacao_005: " + ex.Message, ex);
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