
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;
using Intranet_NEW.Controllers.DAL;

namespace Intranet.BLL.DMP
{
    public class Relatorio
    {
        DAL_MIS AcessaDados_MIS = new Intranet.DAL.DAL_MIS();

        public DataTable PorHora(int dtIni, int dtFim, string tpTel, string tpRel)
        {
            try
            {
                DataTable dt = new DataTable("ReportDump");
                dt.Columns.Add("HR_COL01");
                dt.Columns.Add("EMP01_COL01");
                dt.Columns.Add("EMP01_T_COL01");
                dt.Columns.Add("EMP01_COL02");
                dt.Columns.Add("EMP01_T_COL02");
                dt.Columns.Add("EMP01_COL03");
                dt.Columns.Add("EMP01_T_COL03");
                dt.Columns.Add("EMP02_COL01");
                dt.Columns.Add("EMP02_T_COL01");
                dt.Columns.Add("EMP02_COL02");
                dt.Columns.Add("EMP02_T_COL02");
                dt.Columns.Add("EMP02_COL03");
                dt.Columns.Add("EMP02_T_COL03");
                dt.Columns.Add("EMP03_COL01");
                dt.Columns.Add("EMP03_T_COL01");
                dt.Columns.Add("EMP03_COL02");
                dt.Columns.Add("EMP03_T_COL02");
                dt.Columns.Add("EMP03_COL03");
                dt.Columns.Add("EMP03_T_COL03");
                dt.Columns.Add("EMP04_COL01");
                dt.Columns.Add("EMP04_T_COL01");
                dt.Columns.Add("EMP04_COL02");
                dt.Columns.Add("EMP04_T_COL02");
                dt.Columns.Add("EMP04_COL03");
                dt.Columns.Add("EMP04_T_COL03");
                dt.Columns.Add("EMP05_COL01");
                dt.Columns.Add("EMP05_T_COL01");
                dt.Columns.Add("EMP05_COL02");
                dt.Columns.Add("EMP05_T_COL02");
                dt.Columns.Add("EMP05_COL03");
                dt.Columns.Add("EMP05_T_COL03");
                dt.Columns.Add("EMP06_COL01");
                dt.Columns.Add("EMP06_T_COL01");
                dt.Columns.Add("EMP06_COL02");
                dt.Columns.Add("EMP06_T_COL02");
                dt.Columns.Add("EMP06_COL03");
                dt.Columns.Add("EMP06_T_COL03");
                dt.Columns.Add("EMP07_COL01");
                dt.Columns.Add("EMP07_T_COL01");
                dt.Columns.Add("EMP07_COL02");
                dt.Columns.Add("EMP07_T_COL02");
                dt.Columns.Add("EMP07_COL03");
                dt.Columns.Add("EMP07_T_COL03");
                dt.Columns.Add("TOT_COL01");
                dt.Columns.Add("TOT_T_COL01");
                dt.Columns.Add("TOT_COL02");
                dt.Columns.Add("TOT_T_COL02");
                dt.Columns.Add("TOT_COL03");
                dt.Columns.Add("TOT_T_COL03");

                SqlCommand sqlcommand = new SqlCommand();
                sqlcommand.CommandType = CommandType.StoredProcedure;
                sqlcommand.CommandText = "SP_DMP_RELATORIO_HORA";

                sqlcommand.Parameters.AddWithValue("@DT_INI", dtIni);
                sqlcommand.Parameters.AddWithValue("@DT_FIM", dtFim);
                sqlcommand.Parameters.AddWithValue("@TP_TEL", tpTel);

                DataSet ds = AcessaDados_MIS.ConsultaSQL(sqlcommand);

                if ((ds.Tables.Count > 0) && (ds.Tables[0].Rows.Count > 0))
                {
                    DataTable dtHorario = ds.Tables[0].AsDataView().ToTable(true, "HR_LIGACAO");
                    string[] ArrayRota = { "PRIMACOM02", "PRIMACOM-B", "OKTOR", "SG_CLIESP", "VonexSIP1" };

                    foreach (DataRow drHorario in dtHorario.Rows)
                    {
                        DataRow NovaLinha = dt.NewRow();
                        string Horario = drHorario[0].ToString();
                        Int64 TotalLinhaSucesso_S = 0;
                        Int64 TotalLinhaSucesso_N = 0;

                        NovaLinha[0] = Horario.PadLeft(2, '0');
                        for (int i = 0; i < ArrayRota.Length; i++)
                        {
                            DataRow[] drFind_Falha_S = ds.Tables[0].Select(string.Format("DS_ROTA = '{0}' AND HR_LIGACAO = '{1}' AND ST_FALHA = 'S'", ArrayRota[i], Horario)); // Falha = sim
                            DataRow[] drFind_Falha_N = ds.Tables[0].Select(string.Format("DS_ROTA = '{0}' AND HR_LIGACAO = '{1}' AND ST_FALHA = 'N'", ArrayRota[i], Horario)); // falha = nao

                            Int64 Sucesso_S = drFind_Falha_N.Length > 0 ? Int64.Parse(drFind_Falha_N[0]["QT_LIGACAO"].ToString()) : 0;
                            Int64 Sucesso_N = drFind_Falha_S.Length > 0 ? Int64.Parse(drFind_Falha_S[0]["QT_LIGACAO"].ToString()) : 0;

                            TotalLinhaSucesso_S += Sucesso_S;
                            TotalLinhaSucesso_N += Sucesso_N;

                            if ((Sucesso_S + Sucesso_N) > 0)
                            {
                                if (tpRel == "N")
                                {
                                    NovaLinha[1 + (((i + 1) - 1) * 6)] = string.Format("{0:N0}", Sucesso_S);
                                    NovaLinha[2 + (((i + 1) - 1) * 6)] = string.Format("{0:N2}%", (double)((double)(Sucesso_S * 100) / (Sucesso_S + Sucesso_N)));
                                    NovaLinha[3 + (((i + 1) - 1) * 6)] = string.Format("{0:N0}", Sucesso_N);
                                    NovaLinha[4 + (((i + 1) - 1) * 6 )] = string.Format("{0:N2}%", (double)((double)(Sucesso_N * 100) / (Sucesso_S + Sucesso_N)));
                                    NovaLinha[5 + (((i + 1) - 1) * 6)] = string.Format("{0:N0}", (Sucesso_S + Sucesso_N));
                                    NovaLinha[6 + (((i + 1) - 1) * 6)] = string.Format("{0:N2}%", 100);
                                }
                                else
                                {
                                    NovaLinha[1 + (((i + 1) - 1) * 6)] = string.Format("{0:N2}%", (double)((double)(Sucesso_S * 100) / (Sucesso_S + Sucesso_N)));
                                    NovaLinha[2 + (((i + 1) - 1) * 6)] = string.Format("{0:N0}", Sucesso_S);
                                    NovaLinha[3 + (((i + 1) - 1) * 6)] = string.Format("{0:N2}%", (double)((double)(Sucesso_N * 100) / (Sucesso_S + Sucesso_N)));
                                    NovaLinha[4 + (((i + 1) - 1) * 6)] = string.Format("{0:N0}", Sucesso_N);
                                    NovaLinha[5 + (((i + 1) - 1) * 6)] = string.Format("{0:N0}", (Sucesso_S + Sucesso_N));
                                    NovaLinha[6 + (((i + 1) - 1) * 6)] = string.Format("{0:N2}%", 100);
                                }
                            }
                        }
                        if (tpRel == "N")
                        {
                            NovaLinha[43] = string.Format("{0:N0}", TotalLinhaSucesso_S);
                            NovaLinha[44] = string.Format("{0:N2}%", (double)((double)(TotalLinhaSucesso_S * 100) / (TotalLinhaSucesso_S + TotalLinhaSucesso_N)));
                            NovaLinha[45] = string.Format("{0:N0}", TotalLinhaSucesso_N);
                            NovaLinha[46] = string.Format("{0:N2}%", (double)((double)(TotalLinhaSucesso_N * 100) / (TotalLinhaSucesso_S + TotalLinhaSucesso_N)));
                            NovaLinha[47] = string.Format("{0:N0}", (TotalLinhaSucesso_S + TotalLinhaSucesso_N));
                            NovaLinha[48] = string.Format("{0:N2}%", 100);
                        }
                        else
                        {
                            NovaLinha[43] = string.Format("{0:N2}%", (double)((double)(TotalLinhaSucesso_S * 100) / (TotalLinhaSucesso_S + TotalLinhaSucesso_N)));
                            NovaLinha[44] = string.Format("{0:N0}", TotalLinhaSucesso_S);
                            NovaLinha[45] = string.Format("{0:N2}%", (double)((double)(TotalLinhaSucesso_N * 100) / (TotalLinhaSucesso_S + TotalLinhaSucesso_N)));
                            NovaLinha[46] = string.Format("{0:N0}", TotalLinhaSucesso_N);
                            NovaLinha[47] = string.Format("{0:N0}", (TotalLinhaSucesso_S + TotalLinhaSucesso_N));
                            NovaLinha[48] = string.Format("{0:N2}%", 100);
                        }

                        dt.Rows.Add(NovaLinha);
                    }
                }
                dt.DefaultView.Sort = "HR_COL01";
                return dt;
            }
            catch (Exception ex)
            {
                throw new Exception("DMP.CmdRelatorio_001: " + ex.Message, ex);
            }

        }

        public DataTable PorData(int dtIni, int dtFim, string tpTel, string tpRel)
        {
            try
            {
                DataTable dt = new DataTable("ReportDump");
                dt.Columns.Add("DT_COL01", Type.GetType("System.DateTime"));
                dt.Columns.Add("EMP01_COL01");
                dt.Columns.Add("EMP01_T_COL01");
                dt.Columns.Add("EMP01_COL02");
                dt.Columns.Add("EMP01_T_COL02");
                dt.Columns.Add("EMP01_COL03");
                dt.Columns.Add("EMP01_T_COL03");
                dt.Columns.Add("EMP02_COL01");
                dt.Columns.Add("EMP02_T_COL01");
                dt.Columns.Add("EMP02_COL02");
                dt.Columns.Add("EMP02_T_COL02");
                dt.Columns.Add("EMP02_COL03");
                dt.Columns.Add("EMP02_T_COL03");
                dt.Columns.Add("EMP03_COL01");
                dt.Columns.Add("EMP03_T_COL01");
                dt.Columns.Add("EMP03_COL02");
                dt.Columns.Add("EMP03_T_COL02");
                dt.Columns.Add("EMP03_COL03");
                dt.Columns.Add("EMP03_T_COL03");
                dt.Columns.Add("EMP04_COL01");
                dt.Columns.Add("EMP04_T_COL01");
                dt.Columns.Add("EMP04_COL02");
                dt.Columns.Add("EMP04_T_COL02");
                dt.Columns.Add("EMP04_COL03");
                dt.Columns.Add("EMP04_T_COL03");
                dt.Columns.Add("EMP05_COL01");
                dt.Columns.Add("EMP05_T_COL01");
                dt.Columns.Add("EMP05_COL02");
                dt.Columns.Add("EMP05_T_COL02");
                dt.Columns.Add("EMP05_COL03");
                dt.Columns.Add("EMP05_T_COL03");
                dt.Columns.Add("EMP06_COL01");
                dt.Columns.Add("EMP06_T_COL01");
                dt.Columns.Add("EMP06_COL02");
                dt.Columns.Add("EMP06_T_COL02");
                dt.Columns.Add("EMP06_COL03");
                dt.Columns.Add("EMP06_T_COL03");
                dt.Columns.Add("EMP07_COL01");
                dt.Columns.Add("EMP07_T_COL01");
                dt.Columns.Add("EMP07_COL02");
                dt.Columns.Add("EMP07_T_COL02");
                dt.Columns.Add("EMP07_COL03");
                dt.Columns.Add("EMP07_T_COL03");
                dt.Columns.Add("TOT_COL01");
                dt.Columns.Add("TOT_T_COL01");
                dt.Columns.Add("TOT_COL02");
                dt.Columns.Add("TOT_T_COL02");
                dt.Columns.Add("TOT_COL03");
                dt.Columns.Add("TOT_T_COL03");

                DataTable dtFinal = dt.Clone();
                dtFinal.Columns[0].DataType = Type.GetType("System.String");

                SqlCommand sqlcommand = new SqlCommand();
                sqlcommand.CommandType = CommandType.StoredProcedure;
                sqlcommand.CommandText = "SP_DMP_RELATORIO_DATA";

                sqlcommand.Parameters.AddWithValue("@DT_INI", dtIni);
                sqlcommand.Parameters.AddWithValue("@DT_FIM", dtFim);
                sqlcommand.Parameters.AddWithValue("@TP_TEL", tpTel);

                DataSet ds = AcessaDados_MIS.ConsultaSQL(sqlcommand);

                if ((ds.Tables.Count > 0) && (ds.Tables[0].Rows.Count > 0))
                {
                    DataTable dtHorario = ds.Tables[0].AsDataView().ToTable(true, "DT_LIGACAO");

                    DataView dv = dtHorario.DefaultView;
                    dv.Sort = "DT_LIGACAO ASC";
                    dtHorario = dv.ToTable();

                    string[] ArrayRota = {"PRIMACOM02", "PRIMACOM-B", "OKTOR", "SG_CLIESP", "VonexSIP1" };

                    foreach (DataRow drHorario in dtHorario.Rows)
                    {
                        DataRow NovaLinha = dt.NewRow();
                        string Data = drHorario[0].ToString();
                        Int64 TotalLinhaSucesso_S = 0;
                        Int64 TotalLinhaSucesso_N = 0;

                        if (Data != "Total")
                            NovaLinha[0] = DateTime.Parse(DateTime.Parse(string.Format("{0:####/##/##}", Int64.Parse(Data))).ToString("dd/MM/yyyy"));
                        else
                            NovaLinha[0] = DateTime.MinValue;

                        for (int i = 0; i < ArrayRota.Length; i++)
                        {
                            DataRow[] drFind_Falha_S = ds.Tables[0].Select(string.Format("DS_ROTA = '{0}' AND DT_LIGACAO = '{1}' AND ST_FALHA = 'S'", ArrayRota[i], Data)); // FALHA = SIM
                            DataRow[] drFind_Falha_N = ds.Tables[0].Select(string.Format("DS_ROTA = '{0}' AND DT_LIGACAO = '{1}' AND ST_FALHA = 'N'", ArrayRota[i], Data)); // FALHA = NAO

                            Int64 Sucesso_S = drFind_Falha_N.Length > 0 ? Int64.Parse(drFind_Falha_N[0]["QT_LIGACAO"].ToString()) : 0;
                            Int64 Sucesso_N = drFind_Falha_S.Length > 0 ? Int64.Parse(drFind_Falha_S[0]["QT_LIGACAO"].ToString()) : 0;

                            TotalLinhaSucesso_S += Sucesso_S;
                            TotalLinhaSucesso_N += Sucesso_N;

                            if ((Sucesso_S + Sucesso_N) > 0)
                            {
                                if (tpRel == "N")
                                {
                                    NovaLinha[1 + (((i + 1) - 1) * 6)] = string.Format("{0:N0}", Sucesso_S);
                                    NovaLinha[2 + (((i + 1) - 1) * 6)] = string.Format("{0:N2}%", (double)((double)(Sucesso_S * 100) / (Sucesso_S + Sucesso_N)));
                                    NovaLinha[3 + (((i + 1) - 1) * 6)] = string.Format("{0:N0}", Sucesso_N);
                                    NovaLinha[4 + (((i + 1) - 1) * 6)] = string.Format("{0:N2}%", (double)((double)(Sucesso_N * 100) / (Sucesso_S + Sucesso_N)));
                                    NovaLinha[5 + (((i + 1) - 1) * 6)] = string.Format("{0:N0}", (Sucesso_S + Sucesso_N));
                                    NovaLinha[6 + (((i + 1) - 1) * 6)] = string.Format("{0:N2}%", 100);
                                }
                                else
                                {
                                    NovaLinha[1 + (((i + 1) - 1) * 6)] = string.Format("{0:N2}%", (double)((double)(Sucesso_S * 100) / (Sucesso_S + Sucesso_N)));
                                    NovaLinha[2 + (((i + 1) - 1) * 6)] = string.Format("{0:N0}", Sucesso_S);
                                    NovaLinha[3 + (((i + 1) - 1) * 6)] = string.Format("{0:N2}%", (double)((double)(Sucesso_N * 100) / (Sucesso_S + Sucesso_N)));
                                    NovaLinha[4 + (((i + 1) - 1) * 6)] = string.Format("{0:N0}", Sucesso_N);
                                    NovaLinha[5 + (((i + 1) - 1) * 6)] = string.Format("{0:N0}", (Sucesso_S + Sucesso_N));
                                }
                            }
                        }
                        if (tpRel == "N")
                        {
                            NovaLinha[43] = string.Format("{0:N0}", TotalLinhaSucesso_S);
                            NovaLinha[44] = string.Format("{0:N2}%", (double)((double)(TotalLinhaSucesso_S * 100) / (TotalLinhaSucesso_S + TotalLinhaSucesso_N)));
                            NovaLinha[45] = string.Format("{0:N0}", TotalLinhaSucesso_N);
                            NovaLinha[46] = string.Format("{0:N2}%", (double)((double)(TotalLinhaSucesso_N * 100) / (TotalLinhaSucesso_S + TotalLinhaSucesso_N)));
                            NovaLinha[47] = string.Format("{0:N0}", (TotalLinhaSucesso_S + TotalLinhaSucesso_N));
                            NovaLinha[48] = string.Format("{0:N2}%", 100);
                        }
                        else
                        {
                            NovaLinha[43] = string.Format("{0:N2}%", (double)((double)(TotalLinhaSucesso_S * 100) / (TotalLinhaSucesso_S + TotalLinhaSucesso_N)));
                            NovaLinha[44] = string.Format("{0:N0}", TotalLinhaSucesso_S);
                            NovaLinha[45] = string.Format("{0:N2}%", (double)((double)(TotalLinhaSucesso_N * 100) / (TotalLinhaSucesso_S + TotalLinhaSucesso_N)));
                            NovaLinha[46] = string.Format("{0:N0}", TotalLinhaSucesso_N);
                            NovaLinha[47] = string.Format("{0:N0}", (TotalLinhaSucesso_S + TotalLinhaSucesso_N));
                            NovaLinha[48] = string.Format("{0:N2}%", 100);
                        }

                        //31,32,33,34,35,36

                        if ((DateTime)NovaLinha[0] == DateTime.MinValue)
                        {
                            dt.DefaultView.Sort = "DT_COL01";
                            DataRow drNovaLinha;
                            foreach (DataRow dr in dt.Rows)
                            {
                                drNovaLinha = dtFinal.NewRow();
                                drNovaLinha[0] = ((DateTime)dr[0]).ToString("dd/MM/yyyy");
                                for (int i = 1; i < dr.Table.Columns.Count; i++)
                                    drNovaLinha[i] = dr[i];
                                dtFinal.Rows.Add(drNovaLinha);
                            }

                            drNovaLinha = dtFinal.NewRow();
                            drNovaLinha[0] = "Total";
                            for (int i = 1; i < NovaLinha.Table.Columns.Count; i++)
                                drNovaLinha[i] = NovaLinha[i];
                            dtFinal.Rows.Add(drNovaLinha);
                        }
                        else
                            dt.Rows.Add(NovaLinha);
                    }
                }
                return dtFinal;
            }
            catch (Exception ex)
            {
                throw new Exception("DMP.CmdRelatorio_002: " + ex.Message, ex);
            }
        }

        public DataSet PorStatus(int dtIni, int dtFim, string tpTel, string tpRota, string tpStatus)
        {
            try
            {
                SqlCommand sqlcommand = new SqlCommand();
                sqlcommand.CommandType = CommandType.StoredProcedure;
                sqlcommand.CommandText = "SP_DMP_RELATORIO_STATUS";

                sqlcommand.Parameters.AddWithValue("@DT_INI", dtIni);
                sqlcommand.Parameters.AddWithValue("@DT_FIM", dtFim);
                sqlcommand.Parameters.AddWithValue("@TP_TEL", tpTel);
                sqlcommand.Parameters.AddWithValue("@DS_ROTA", tpRota);
                sqlcommand.Parameters.AddWithValue("@DS_STATUS", tpStatus);

                DataSet ds = AcessaDados_MIS.ConsultaSQL(sqlcommand);

                DataSet dsFinal = new DataSet();
                DataTable dtFinal = new DataTable("TableFinal");
                if ((ds.Tables.Count > 0) && (ds.Tables[0].Rows.Count > 0))
                {
                    DataTable dtHorario = ds.Tables[0].AsDataView().ToTable(true, "DT_LIGACAO"); dtHorario.TableName = "dtHorario";
                    DataTable dtStatus = ds.Tables[0].AsDataView().ToTable(true, "DS_STATUS"); dtStatus.TableName = "dtStatus";

                    DataView dv = dtHorario.DefaultView;
                    dv.Sort = "DT_LIGACAO ASC";
                    dtHorario = dv.ToTable();

                    dtFinal.Columns.Add("Descrição", typeof(string));
                    foreach (DataRow drHorario in dtHorario.Rows)
                        dtFinal.Columns.Add(DateTime.Parse(string.Format("{0:####/##/##}", (decimal)drHorario["DT_LIGACAO"])).ToString("dd/MM/yyyy"), typeof(decimal));

                    foreach (DataRow drStatus in dtStatus.Rows)
                    {
                        DataRow NovaLinha = dtFinal.NewRow();
                        NovaLinha[0] = drStatus[0].ToString();
                        foreach (DataRow drHorario in dtHorario.Rows)
                        {
                            DataRow[] drFind_Status = ds.Tables[0].Select(string.Format("DS_STATUS = '{0}' AND DT_LIGACAO = '{1}'", drStatus["DS_STATUS"].ToString(), (decimal)drHorario["DT_LIGACAO"]));
                            NovaLinha[DateTime.Parse(string.Format("{0:####/##/##}", (decimal)drHorario["DT_LIGACAO"])).ToString("dd/MM/yyyy")] = drFind_Status.Length > 0 ? Int64.Parse(drFind_Status[0]["QT_LIGACAO"].ToString()) : 0;
                        }
                        dtFinal.Rows.Add(NovaLinha);
                    }

                    DataRow LinhaTotal = dtFinal.NewRow();
                    LinhaTotal[0] = "Total";
                    for (int i = 1; i < dtFinal.Columns.Count; i++)
                        LinhaTotal[i] = decimal.Parse(dtFinal.AsEnumerable().Sum(x => x.Field<decimal>(dtFinal.Columns[i].ColumnName)).ToString());

                    dtFinal.Rows.Add(LinhaTotal);

                    dsFinal.Tables.Add(dtFinal);
                    dsFinal.Tables.Add(ds.Tables[1].Copy());
                    dsFinal.Tables.Add(ds.Tables[2].Copy());
                }
                return dsFinal;
            }
            catch (Exception ex)
            {
                throw new Exception("DMP.CmdRelatorio_003: " + ex.Message, ex);
            }
        }
    }
}