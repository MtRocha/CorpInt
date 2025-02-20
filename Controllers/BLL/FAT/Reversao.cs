using Intranet_NEW.Controllers.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace Intranet.BLL.FAT
{
    public class Reversao
    {
        public DataSet GeraRelatorioReversaoMes(DateTime DT_REFERENCIA)
        {
            try
            {
                SqlCommand sqlcommand = new SqlCommand();
                sqlcommand.CommandType = CommandType.StoredProcedure;
                sqlcommand.CommandText = "SP_REV_GERA_RESUMO_REVERSAO_MES";
                sqlcommand.Parameters.AddWithValue("@DT_REFERENCIA", DT_REFERENCIA.ToString("yyyy-MM-dd HH:mm:ss"));

                DAL_MIS AcessaDadosCaixa = new Intranet.DAL.DAL_MIS();

                DataSet dsFechamento = AcessaDadosCaixa.ConsultaSQL(sqlcommand);

                DataSet dsFinal = new DataSet("ReportReversao");
                if ((dsFechamento.Tables.Count > 0) && (dsFechamento.Tables[0].Rows.Count > 0))
                {
                    DataTable dtCarteira = new DataTable("Carteira");
                    dtCarteira.Columns.Add("COL01");
                    dtCarteira.Columns.Add("COL02");
                    dtCarteira.Columns.Add("COL03");
                    dtCarteira.Columns.Add("COL04");

                    DataTable dtReversao = new DataTable("Reversao");
                    dtReversao.Columns.Add("COL01");
                    dtReversao.Columns.Add("COL02");
                    dtReversao.Columns.Add("COL03");
                    dtReversao.Columns.Add("COL04");


                    if (DT_REFERENCIA >= DateTime.Parse("2017-06-01 00:00:00.000")) //BASE >= 201706
                    {
                       
                        /* VALORES DE CARTEIRA */
                        dtCarteira.Rows.Add(
                                          "CARTEIRA NORMAL"
                                        , string.Format("R$ {0:N2}", (decimal)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("TP_CONTRATO") == "C" && a.Field<string>("TP_CARTEIRA") == "NORMAL").Sum(x => x.Field<decimal>("VL_SALDO_DIARIO")))
                                        , string.Format("R$ {0:N2}", (decimal)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("TP_CONTRATO") == "H" && a.Field<string>("TP_CARTEIRA") == "NORMAL").Sum(x => x.Field<decimal>("VL_SALDO_DIARIO")))
                                        , string.Format("R$ {0:N2}", (decimal)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("TP_CARTEIRA") == "NORMAL").Sum(x => x.Field<decimal>("VL_SALDO_DIARIO")))
                            );

                        dtCarteira.Rows.Add(
                                         "CAMPANHA FAIXA 01"
                                       , string.Format("R$ {0:N2}", (decimal)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("TP_CONTRATO") == "C" && a.Field<string>("TP_CARTEIRA") == "CAMPANHA_C_<360").Sum(x => x.Field<decimal>("VL_SALDO_DIARIO")))
                                       , string.Format("R$ {0:N2}", (decimal)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("TP_CONTRATO") == "H" && a.Field<string>("TP_CARTEIRA") == "CAMPANHA_H_<540").Sum(x => x.Field<decimal>("VL_SALDO_DIARIO")))
                                       , string.Format("R$ {0:N2}", (decimal)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("TP_CARTEIRA") == "CAMPANHA_C_<360" || a.Field<string>("TP_CARTEIRA") == "CAMPANHA_H_<540").Sum(x => x.Field<decimal>("VL_SALDO_DIARIO")))
                           );

                        dtCarteira.Rows.Add(
                                         "CAMPANHA FAIXA 02"
                                       , string.Format("R$ {0:N2}", (decimal)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("TP_CONTRATO") == "C" && a.Field<string>("TP_CARTEIRA") == "CAMPANHA_C_>360").Sum(x => x.Field<decimal>("VL_SALDO_DIARIO")))
                                       , string.Format("R$ {0:N2}", (decimal)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("TP_CONTRATO") == "H" && a.Field<string>("TP_CARTEIRA") == "CAMPANHA_H_>540").Sum(x => x.Field<decimal>("VL_SALDO_DIARIO")))
                                       , string.Format("R$ {0:N2}", (decimal)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("TP_CARTEIRA") == "CAMPANHA_C_>360" || a.Field<string>("TP_CARTEIRA") == "CAMPANHA_H_>540").Sum(x => x.Field<decimal>("VL_SALDO_DIARIO")))
                           );

                        dtCarteira.Rows.Add(
                                       "TOTAL "
                                     , string.Format("R$ {0:N2}", (decimal)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("TP_CONTRATO") == "C").Sum(x => x.Field<decimal>("VL_SALDO_DIARIO")))
                                     , string.Format("R$ {0:N2}", (decimal)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("TP_CONTRATO") == "H").Sum(x => x.Field<decimal>("VL_SALDO_DIARIO")))
                                     , string.Format("R$ {0:N2}", (decimal)dsFechamento.Tables[0].AsEnumerable().Sum(x => x.Field<decimal>("VL_SALDO_DIARIO")))
                         );



                        /* VALORES DE REVERSAO */
                        dtReversao.Rows.Add(
                                          "BAIXA NORMAL"
                                        , string.Format("R$ {0:N2}", (decimal)dsFechamento.Tables[0].AsEnumerable().Where(a => (a.Field<string>("TP_CONTRATO") == "C" && a.Field<string>("TP_CARTEIRA") == "NORMAL")).Sum(x => x.Field<decimal>("VL_RECUPERACAO")))
                                        , string.Format("R$ {0:N2}", (decimal)dsFechamento.Tables[0].AsEnumerable().Where(a => (a.Field<string>("TP_CONTRATO") == "H" && a.Field<string>("TP_CARTEIRA") == "NORMAL")).Sum(x => x.Field<decimal>("VL_RECUPERACAO")))
                                        , string.Format("R$ {0:N2}", (decimal)dsFechamento.Tables[0].AsEnumerable().Where(a => (a.Field<string>("TP_CARTEIRA") == "NORMAL")).Sum(x => x.Field<decimal>("VL_RECUPERACAO")))
                            );

                        dtReversao.Rows.Add(
                                          "BAIXA CAMPANHA FAIXA 01"
                                        , string.Format("R$ {0:N2}", (decimal)dsFechamento.Tables[0].AsEnumerable().Where(a => (a.Field<string>("TP_CONTRATO") == "C" && a.Field<string>("TP_CARTEIRA") == "CAMPANHA_C_<360")).Sum(x => x.Field<decimal>("VL_RECUPERACAO")))
                                        , string.Format("R$ {0:N2}", (decimal)dsFechamento.Tables[0].AsEnumerable().Where(a => (a.Field<string>("TP_CONTRATO") == "H" && a.Field<string>("TP_CARTEIRA") == "CAMPANHA_H_<540")).Sum(x => x.Field<decimal>("VL_RECUPERACAO")))
                                        , string.Format("R$ {0:N2}", (decimal)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("TP_CARTEIRA") == "CAMPANHA_C_<360" || a.Field<string>("TP_CARTEIRA") == "CAMPANHA_H_<540").Sum(x => x.Field<decimal>("VL_RECUPERACAO")))
                            );

                        dtReversao.Rows.Add(
                                          "BAIXA CAMPANHA FAIXA 02"
                                        , string.Format("R$ {0:N2}", (decimal)dsFechamento.Tables[0].AsEnumerable().Where(a => (a.Field<string>("TP_CONTRATO") == "C" && a.Field<string>("TP_CARTEIRA") == "CAMPANHA_C_>360")).Sum(x => x.Field<decimal>("VL_RECUPERACAO")))
                                        , string.Format("R$ {0:N2}", (decimal)dsFechamento.Tables[0].AsEnumerable().Where(a => (a.Field<string>("TP_CONTRATO") == "H" && a.Field<string>("TP_CARTEIRA") == "CAMPANHA_H_>540")).Sum(x => x.Field<decimal>("VL_RECUPERACAO")))
                                        , string.Format("R$ {0:N2}", (decimal)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("TP_CARTEIRA") == "CAMPANHA_C_>360" || a.Field<string>("TP_CARTEIRA") == "CAMPANHA_H_>540").Sum(x => x.Field<decimal>("VL_RECUPERACAO")))
                            );

                        dtReversao.Rows.Add(
                                         "SUBTOTAL"
                                       , string.Format("R$ {0:N2}", (decimal)dsFechamento.Tables[0].AsEnumerable().Where(a => (a.Field<string>("TP_CONTRATO") == "C")).Sum(x => x.Field<decimal>("VL_RECUPERACAO")))
                                       , string.Format("R$ {0:N2}", (decimal)dsFechamento.Tables[0].AsEnumerable().Where(a => (a.Field<string>("TP_CONTRATO") == "H")).Sum(x => x.Field<decimal>("VL_RECUPERACAO")))
                                       , string.Format("R$ {0:N2}", (decimal)dsFechamento.Tables[0].AsEnumerable().Sum(x => x.Field<decimal>("VL_RECUPERACAO")))
                            );

                        dtReversao.Rows.Add("&nbsp;", "&nbsp;", "&nbsp;", "&nbsp;");

                        dtReversao.Rows.Add(
                                      "INDICE REVERSÃO - NORMAL"
                                      , string.Format("{0:N2} %", decimal.Parse(dtCarteira.Rows[0][1].ToString().Replace("R$ ", "")) != 0 ? (decimal.Parse(dtReversao.Rows[0][1].ToString().Replace("R$ ", "")) * 100) / decimal.Parse(dtCarteira.Rows[0][1].ToString().Replace("R$ ", "")) : (decimal)0.0)
                                      , string.Format("{0:N2} %", decimal.Parse(dtCarteira.Rows[0][2].ToString().Replace("R$ ", "")) != 0 ? (decimal.Parse(dtReversao.Rows[0][2].ToString().Replace("R$ ", "")) * 100) / decimal.Parse(dtCarteira.Rows[0][2].ToString().Replace("R$ ", "")) : (decimal)0.0)
                                      , string.Format("{0:N2} %", decimal.Parse(dtCarteira.Rows[0][3].ToString().Replace("R$ ", "")) != 0 ? (decimal.Parse(dtReversao.Rows[0][3].ToString().Replace("R$ ", "")) * 100) / decimal.Parse(dtCarteira.Rows[0][3].ToString().Replace("R$ ", "")) : (decimal)0.0)
                            );

                        dtReversao.Rows.Add(
                                     "INDICE REVERSÃO - CAMPANHA FAIXA 01"
                                     , string.Format("{0:N2} %", decimal.Parse(dtCarteira.Rows[1][1].ToString().Replace("R$ ", "")) != 0 ? (decimal.Parse(dtReversao.Rows[1][1].ToString().Replace("R$ ", "")) * 100) / decimal.Parse(dtCarteira.Rows[1][1].ToString().Replace("R$ ", "")) : (decimal)0.0)
                                     , string.Format("{0:N2} %", decimal.Parse(dtCarteira.Rows[1][2].ToString().Replace("R$ ", "")) != 0 ? (decimal.Parse(dtReversao.Rows[1][2].ToString().Replace("R$ ", "")) * 100) / decimal.Parse(dtCarteira.Rows[1][2].ToString().Replace("R$ ", "")) : (decimal)0.0)
                                     , string.Format("{0:N2} %", decimal.Parse(dtCarteira.Rows[1][3].ToString().Replace("R$ ", "")) != 0 ? (decimal.Parse(dtReversao.Rows[1][3].ToString().Replace("R$ ", "")) * 100) / decimal.Parse(dtCarteira.Rows[1][3].ToString().Replace("R$ ", "")) : (decimal)0.0)
                           );

                        dtReversao.Rows.Add(
                                     "INDICE REVERSÃO -  CAMPANHA FAIXA 02"
                                     , string.Format("{0:N2} %", decimal.Parse(dtCarteira.Rows[2][1].ToString().Replace("R$ ", "")) != 0 ? (decimal.Parse(dtReversao.Rows[2][1].ToString().Replace("R$ ", "")) * 100) / decimal.Parse(dtCarteira.Rows[2][1].ToString().Replace("R$ ", "")) : (decimal)0.0)
                                     , string.Format("{0:N2} %", decimal.Parse(dtCarteira.Rows[2][2].ToString().Replace("R$ ", "")) != 0 ? (decimal.Parse(dtReversao.Rows[2][2].ToString().Replace("R$ ", "")) * 100) / decimal.Parse(dtCarteira.Rows[2][2].ToString().Replace("R$ ", "")) : (decimal)0.0)
                                     , string.Format("{0:N2} %", decimal.Parse(dtCarteira.Rows[2][3].ToString().Replace("R$ ", "")) != 0 ? (decimal.Parse(dtReversao.Rows[2][3].ToString().Replace("R$ ", "")) * 100) / decimal.Parse(dtCarteira.Rows[2][3].ToString().Replace("R$ ", "")) : (decimal)0.0)
                           );

                        dtReversao.Rows.Add(
                                     "INDICE REVERSÃO - TOTAL"
                                     , string.Format("{0:N2} %", decimal.Parse(dtCarteira.Rows[3][1].ToString().Replace("R$ ", "")) != 0 ? (decimal.Parse(dtReversao.Rows[3][1].ToString().Replace("R$ ", "")) * 100) / decimal.Parse(dtCarteira.Rows[3][1].ToString().Replace("R$ ", "")) : (decimal)0.0)
                                     , string.Format("{0:N2} %", decimal.Parse(dtCarteira.Rows[3][2].ToString().Replace("R$ ", "")) != 0 ? (decimal.Parse(dtReversao.Rows[3][2].ToString().Replace("R$ ", "")) * 100) / decimal.Parse(dtCarteira.Rows[3][2].ToString().Replace("R$ ", "")) : (decimal)0.0)
                                     , string.Format("{0:N2} %", decimal.Parse(dtCarteira.Rows[3][3].ToString().Replace("R$ ", "")) != 0 ? (decimal.Parse(dtReversao.Rows[3][3].ToString().Replace("R$ ", "")) * 100) / decimal.Parse(dtCarteira.Rows[3][3].ToString().Replace("R$ ", "")) : (decimal)0.0)
                           );
                    }
                    else //BASE ANTERIOR A 201706
                    {
                        /* VALORES DE CARTEIRA */
                        dtCarteira.Rows.Add(
                                          "CARTEIRA"
                                        , string.Format("R$ {0:N2}", (decimal)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("TP_CONTRATO") == "C").Sum(x => x.Field<decimal>("VL_SALDO_DIARIO")))
                                        , string.Format("R$ {0:N2}", (decimal)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("TP_CONTRATO") == "H").Sum(x => x.Field<decimal>("VL_SALDO_DIARIO")))
                                        , string.Format("R$ {0:N2}", (decimal)dsFechamento.Tables[0].AsEnumerable().Sum(x => x.Field<decimal>("VL_SALDO_DIARIO")))
                            );
                        
                        /* VALORES DE REVERSAO */
                        dtReversao.Rows.Add(
                                          "BAIXA"
                                        , string.Format("R$ {0:N2}", (decimal)dsFechamento.Tables[0].AsEnumerable().Where(a => (a.Field<string>("TP_CONTRATO") == "C")).Sum(x => x.Field<decimal>("VL_RECUPERACAO")))
                                        , string.Format("R$ {0:N2}", (decimal)dsFechamento.Tables[0].AsEnumerable().Where(a => (a.Field<string>("TP_CONTRATO") == "H")).Sum(x => x.Field<decimal>("VL_RECUPERACAO")))
                                        , string.Format("R$ {0:N2}", (decimal)dsFechamento.Tables[0].AsEnumerable().Sum(x => x.Field<decimal>("VL_RECUPERACAO")))
                            );

                        dtReversao.Rows.Add("&nbsp;", "&nbsp;", "&nbsp;", "&nbsp;");

                        dtReversao.Rows.Add(
                                      "INDICE REVERSÃO - NORMAL"
                                      , string.Format("{0:N2} %", decimal.Parse(dtCarteira.Rows[0][1].ToString().Replace("R$ ", "")) != 0 ? (decimal.Parse(dtReversao.Rows[0][1].ToString().Replace("R$ ", "")) * 100) / decimal.Parse(dtCarteira.Rows[0][1].ToString().Replace("R$ ", "")) : (decimal)0.0)
                                      , string.Format("{0:N2} %", decimal.Parse(dtCarteira.Rows[0][2].ToString().Replace("R$ ", "")) != 0 ? (decimal.Parse(dtReversao.Rows[0][2].ToString().Replace("R$ ", "")) * 100) / decimal.Parse(dtCarteira.Rows[0][2].ToString().Replace("R$ ", "")) : (decimal)0.0)
                                      , string.Format("{0:N2} %", decimal.Parse(dtCarteira.Rows[0][3].ToString().Replace("R$ ", "")) != 0 ? (decimal.Parse(dtReversao.Rows[0][3].ToString().Replace("R$ ", "")) * 100) / decimal.Parse(dtCarteira.Rows[0][3].ToString().Replace("R$ ", "")) : (decimal)0.0)
                            );

                    }

                    dsFinal.Tables.Add(dtCarteira);
                    dsFinal.Tables.Add(dtReversao);
                    dsFinal.Tables.Add(dsFechamento.Tables[1].Copy());
                }
                return dsFinal;
            }
            catch (Exception ex)
            {
                throw new Exception("FAT.CmdReversao_001: " + ex.Message, ex);
            }
        }

        public DataSet GeraRelatorioReversaoData(DateTime DT_REFERENCIA)
        {
            try
            {
                SqlCommand sqlcommand = new SqlCommand();
                sqlcommand.CommandType = CommandType.StoredProcedure;
                sqlcommand.CommandText = "SP_REV_GERA_RESUMO_REVERSAO_DATA";
                sqlcommand.Parameters.AddWithValue("@DT_REFERENCIA", DT_REFERENCIA.ToString("yyyy-MM-dd HH:mm:ss"));

                DAL_MIS AcessaDadosCaixa = new Intranet.DAL.DAL_MIS();

                DataSet dsReversao = AcessaDadosCaixa.ConsultaSQL(sqlcommand);
                DataSet dsFinal = new DataSet("dsFinal");

                DataTable dtPorData = new DataTable("PorData");
                dtPorData.Columns.Add("COL00");
                dtPorData.Columns.Add("COL01");

                dtPorData.Columns.Add("COL02");
                dtPorData.Columns.Add("COL03");
                dtPorData.Columns.Add("COL04");

                dtPorData.Columns.Add("COL05");
                dtPorData.Columns.Add("COL06");
                dtPorData.Columns.Add("COL07");

                dtPorData.Columns.Add("COL08");
                dtPorData.Columns.Add("COL09");
                dtPorData.Columns.Add("COL10");

                if ((dsReversao.Tables.Count > 0) && (dsReversao.Tables[0].Rows.Count > 0))
                {
                    DateTime ultimoDia = new DateTime(DT_REFERENCIA.Year, DT_REFERENCIA.Month, DateTime.DaysInMonth(DT_REFERENCIA.Year, DT_REFERENCIA.Month));
                    DT_REFERENCIA = DT_REFERENCIA.AddDays(-1);

                    while (DT_REFERENCIA <= ultimoDia)
                    {
                        //if (DT_REFERENCIA.DayOfWeek != DayOfWeek.Sunday)
                        //{
                        var ValorC = dsReversao.Tables[0].AsEnumerable().Where(a => a.Field<Decimal>("DT_DISPONIBULIZACAO") == Decimal.Parse(DT_REFERENCIA.ToString("yyyyMMdd")) && a.Field<string>("TP_CONTRATO") == "C").Select(s => new { VL_SALDO_DIARIO = s.Field<decimal>("VL_SALDO_DIARIO"), VL_RECUPERACAO = s.Field<decimal>("VL_RECUPERACAO") });
                        decimal CA1 = ValorC.Count() > 0 ? ValorC.ToList()[0].VL_SALDO_DIARIO : 0;
                        decimal CA2 = ValorC.Count() > 0 ? ValorC.ToList()[0].VL_RECUPERACAO : 0;
                        decimal CA3 = CA1 != 0 ? (CA2 / CA1) * 100 : 0;

                        var ValorH = dsReversao.Tables[0].AsEnumerable().Where(a => a.Field<Decimal>("DT_DISPONIBULIZACAO") == Decimal.Parse(DT_REFERENCIA.ToString("yyyyMMdd")) && a.Field<string>("TP_CONTRATO") == "H").Select(s => new { VL_SALDO_DIARIO = s.Field<decimal>("VL_SALDO_DIARIO"), VL_RECUPERACAO = s.Field<decimal>("VL_RECUPERACAO") });
                        decimal HA1 = ValorH.Count() > 0 ? ValorH.ToList()[0].VL_SALDO_DIARIO : 0;
                        decimal HA2 = ValorH.Count() > 0 ? ValorH.ToList()[0].VL_RECUPERACAO : 0;
                        decimal HA3 = HA1 != 0 ? (HA2 / HA1) * 100 : 0;

                        decimal TA1 = CA1 + HA1;
                        decimal TA2 = CA2 + HA2;
                        decimal TA3 = TA1 != 0 ? (TA2 / TA1) * 100 : 0;

                        if (ValorC.Count() == 0 && ValorH.Count() == 0)
                        {
                            dtPorData.Rows.Add(
                                  (DT_REFERENCIA.ToString("dddd", new System.Globalization.CultureInfo("pt-BR"))).Substring(0, 3).ToLower()
                                , DT_REFERENCIA.ToString("dd/MM/yyyy")
                                , "&nbsp;"
                                , "&nbsp;"
                                , "&nbsp;"
                                , "&nbsp;"
                                , "&nbsp;"
                                , "&nbsp;"
                                , "&nbsp;"
                                , "&nbsp;"
                                , "&nbsp;"
                                );
                        }
                        else
                        {
                            dtPorData.Rows.Add(
                                  (DT_REFERENCIA.ToString("dddd", new System.Globalization.CultureInfo("pt-BR"))).Substring(0, 3).ToLower()
                                , DT_REFERENCIA.ToString("dd/MM/yyyy")
                                , string.Format("R$ {0:N2}", CA1)
                                , string.Format("R$ {0:N2}", CA2)
                                , string.Format("{0:N2} %", CA3)

                                , string.Format("R$ {0:N2}", HA1)
                                , string.Format("R$ {0:N2}", HA2)
                                , string.Format("{0:N2} %", HA3)

                                , string.Format("R$ {0:N2}", TA1)
                                , string.Format("R$ {0:N2}", TA2)
                                , string.Format("{0:N2} %", TA3)

                                );
                            //  }
                        }
                        DT_REFERENCIA = DT_REFERENCIA.AddDays(1);
                    }
                    var aValorC = dsReversao.Tables[0].AsEnumerable().GroupBy(a => a.Field<string>("TP_CONTRATO")).Select(sel => new { Tipo = sel.Key, VL_SALDO_DIARIO = sel.Sum(sum => sum.Field<decimal>("VL_SALDO_DIARIO")), VL_RECUPERACAO = sel.Sum(sum => sum.Field<decimal>("VL_RECUPERACAO")) });

                    decimal aCA1 = aValorC.Where(s => s.Tipo == "C").ToList()[0].VL_SALDO_DIARIO;
                    decimal aCA2 = aValorC.Where(s => s.Tipo == "C").ToList()[0].VL_RECUPERACAO;
                    decimal aCA3 = aCA1 != 0 ? (aCA2 / aCA1) * 100 : 0;

                    decimal aHA1 = aValorC.Where(s => s.Tipo == "H").ToList()[0].VL_SALDO_DIARIO;
                    decimal aHA2 = aValorC.Where(s => s.Tipo == "H").ToList()[0].VL_RECUPERACAO;
                    decimal aHA3 = aHA1 != 0 ? (aHA2 / aHA1) * 100 : 0;

                    decimal aTA1 = aCA1 + aHA1;
                    decimal aTA2 = aCA2 + aHA2;
                    decimal aTA3 = aTA1 != 0 ? (aTA2 / aTA1) * 100 : 0;

                    dtPorData.Rows.Add(
                                      ""
                                    , "TOTAL"
                                    , string.Format("R$ {0:N2}", aCA1)
                                    , string.Format("R$ {0:N2}", aCA2)
                                    , string.Format("{0:N2} %", aCA3)

                                    , string.Format("R$ {0:N2}", aHA1)
                                    , string.Format("R$ {0:N2}", aHA2)
                                    , string.Format("{0:N2} %", aHA3)

                                    , string.Format("R$ {0:N2}", aTA1)
                                    , string.Format("R$ {0:N2}", aTA2)
                                    , string.Format("{0:N2} %", aTA3)
                                    );

                    dsFinal.Tables.Add(dtPorData);
                    dsFinal.Tables.Add(dsReversao.Tables[1].Copy());
                }

                return dsFinal;
            }
            catch (Exception ex)
            {
                throw new Exception("FAT.CmdReversao_001: " + ex.Message, ex);
            }
        }

        public DataSet GeraRelatorioReversaoBaixa(DateTime DT_REFERENCIA)
        {
            try
            {
                SqlCommand sqlcommand = new SqlCommand();
                sqlcommand.CommandType = CommandType.StoredProcedure;
                sqlcommand.CommandText = "SP_REV_GERA_RESUMO_REVERSAO_BAIXA";
                sqlcommand.Parameters.AddWithValue("@DT_REFERENCIA", DT_REFERENCIA.ToString("yyyy-MM-dd HH:mm:ss"));

                DAL_MIS AcessaDadosCaixa = new Intranet.DAL.DAL_MIS();

                DataSet dsReversao = AcessaDadosCaixa.ConsultaSQL(sqlcommand);
                DataSet dsFinal = new DataSet("dsFinal");

                DataTable dtPorBaixa = new DataTable("PorBaixa");
                dtPorBaixa.Columns.Add("COL00");
                dtPorBaixa.Columns.Add("COL01");
                dtPorBaixa.Columns.Add("COL02");
                dtPorBaixa.Columns.Add("COL03");
                dtPorBaixa.Columns.Add("COL04");
                dtPorBaixa.Columns.Add("COL05");
                dtPorBaixa.Columns.Add("COL06");
                dtPorBaixa.Columns.Add("COL07");
                dtPorBaixa.Columns.Add("COL08");
                dtPorBaixa.Columns.Add("COL09");
                dtPorBaixa.Columns.Add("COL10");
                dtPorBaixa.Columns.Add("COL11");
                dtPorBaixa.Columns.Add("COL12");
                dtPorBaixa.Columns.Add("COL13");
                dtPorBaixa.Columns.Add("COL14");
                dtPorBaixa.Columns.Add("COL15");
                dtPorBaixa.Columns.Add("COL16");
                dtPorBaixa.Columns.Add("COL17");
                dtPorBaixa.Columns.Add("COL18");
                dtPorBaixa.Columns.Add("COL19");

                if ((dsReversao.Tables.Count > 0) && (dsReversao.Tables[0].Rows.Count > 0))
                {
                    dsReversao.Tables[0].Columns["VL_EVOLUCAO"].DefaultValue = 0;

                    var TBL = dsReversao.Tables[0].AsEnumerable();
                    DateTime ultimoDia = new DateTime(DT_REFERENCIA.Year, DT_REFERENCIA.Month, DateTime.DaysInMonth(DT_REFERENCIA.Year, DT_REFERENCIA.Month));

                    while (DT_REFERENCIA < ultimoDia)
                    {
                        if (DT_REFERENCIA.DayOfWeek != DayOfWeek.Sunday)
                        {
                            var
                            _var = TBL.Where(a => a.Field<Decimal>("DT_BAIXA") == Decimal.Parse(DT_REFERENCIA.ToString("yyyyMMdd")) && a.Field<Int16>("CD_MOTIVO_BAIXA") == 1 && a.Field<string>("TP_CONTRATO") == "C").Select(b => new { VL_EVOLUCAO = b.Field<decimal>("VL_EVOLUCAO") }); decimal COL02 = _var.Count() > 0 ? _var.ToList()[0].VL_EVOLUCAO : 0;
                            _var = TBL.Where(a => a.Field<Decimal>("DT_BAIXA") == Decimal.Parse(DT_REFERENCIA.ToString("yyyyMMdd")) && a.Field<Int16>("CD_MOTIVO_BAIXA") == 1 && a.Field<string>("TP_CONTRATO") == "H").Select(b => new { VL_EVOLUCAO = b.Field<decimal>("VL_EVOLUCAO") }); decimal COL03 = _var.Count() > 0 ? _var.ToList()[0].VL_EVOLUCAO : 0;
                            _var = TBL.Where(a => a.Field<Decimal>("DT_BAIXA") == Decimal.Parse(DT_REFERENCIA.ToString("yyyyMMdd")) && a.Field<Int16>("CD_MOTIVO_BAIXA") == 1).Select(b => new { VL_EVOLUCAO = b.Field<decimal>("VL_EVOLUCAO") }); decimal COL04 = _var.Count() > 0 ? _var.ToList().Sum(c => c.VL_EVOLUCAO) : 0;

                            _var = TBL.Where(a => a.Field<Decimal>("DT_BAIXA") == Decimal.Parse(DT_REFERENCIA.ToString("yyyyMMdd")) && a.Field<Int16>("CD_MOTIVO_BAIXA") == 2 && a.Field<string>("TP_CONTRATO") == "C").Select(b => new { VL_EVOLUCAO = b.Field<decimal>("VL_EVOLUCAO") }); decimal COL05 = _var.Count() > 0 ? _var.ToList()[0].VL_EVOLUCAO : 0;
                            _var = TBL.Where(a => a.Field<Decimal>("DT_BAIXA") == Decimal.Parse(DT_REFERENCIA.ToString("yyyyMMdd")) && a.Field<Int16>("CD_MOTIVO_BAIXA") == 2 && a.Field<string>("TP_CONTRATO") == "H").Select(b => new { VL_EVOLUCAO = b.Field<decimal>("VL_EVOLUCAO") }); decimal COL06 = _var.Count() > 0 ? _var.ToList()[0].VL_EVOLUCAO : 0;
                            _var = TBL.Where(a => a.Field<Decimal>("DT_BAIXA") == Decimal.Parse(DT_REFERENCIA.ToString("yyyyMMdd")) && a.Field<Int16>("CD_MOTIVO_BAIXA") == 2).Select(b => new { VL_EVOLUCAO = b.Field<decimal>("VL_EVOLUCAO") }); decimal COL07 = _var.Count() > 0 ? _var.ToList().Sum(c => c.VL_EVOLUCAO) : 0;

                            _var = TBL.Where(a => a.Field<Decimal>("DT_BAIXA") == Decimal.Parse(DT_REFERENCIA.ToString("yyyyMMdd")) && a.Field<Int16>("CD_MOTIVO_BAIXA") == 3 && a.Field<string>("TP_CONTRATO") == "C").Select(b => new { VL_EVOLUCAO = b.Field<decimal>("VL_EVOLUCAO") }); decimal COL08 = _var.Count() > 0 ? _var.ToList()[0].VL_EVOLUCAO : 0;
                            _var = TBL.Where(a => a.Field<Decimal>("DT_BAIXA") == Decimal.Parse(DT_REFERENCIA.ToString("yyyyMMdd")) && a.Field<Int16>("CD_MOTIVO_BAIXA") == 3 && a.Field<string>("TP_CONTRATO") == "H").Select(b => new { VL_EVOLUCAO = b.Field<decimal>("VL_EVOLUCAO") }); decimal COL09 = _var.Count() > 0 ? _var.ToList()[0].VL_EVOLUCAO : 0;
                            _var = TBL.Where(a => a.Field<Decimal>("DT_BAIXA") == Decimal.Parse(DT_REFERENCIA.ToString("yyyyMMdd")) && a.Field<Int16>("CD_MOTIVO_BAIXA") == 3).Select(b => new { VL_EVOLUCAO = b.Field<decimal>("VL_EVOLUCAO") }); decimal COL10 = _var.Count() > 0 ? _var.ToList().Sum(c => c.VL_EVOLUCAO) : 0;

                            _var = TBL.Where(a => a.Field<Decimal>("DT_BAIXA") == Decimal.Parse(DT_REFERENCIA.ToString("yyyyMMdd")) && a.Field<Int16>("CD_MOTIVO_BAIXA") == 4 && a.Field<string>("TP_CONTRATO") == "C").Select(b => new { VL_EVOLUCAO = b.Field<decimal>("VL_EVOLUCAO") }); decimal COL11 = _var.Count() > 0 ? _var.ToList()[0].VL_EVOLUCAO : 0;
                            _var = TBL.Where(a => a.Field<Decimal>("DT_BAIXA") == Decimal.Parse(DT_REFERENCIA.ToString("yyyyMMdd")) && a.Field<Int16>("CD_MOTIVO_BAIXA") == 4 && a.Field<string>("TP_CONTRATO") == "H").Select(b => new { VL_EVOLUCAO = b.Field<decimal>("VL_EVOLUCAO") }); decimal COL12 = _var.Count() > 0 ? _var.ToList()[0].VL_EVOLUCAO : 0;
                            _var = TBL.Where(a => a.Field<Decimal>("DT_BAIXA") == Decimal.Parse(DT_REFERENCIA.ToString("yyyyMMdd")) && a.Field<Int16>("CD_MOTIVO_BAIXA") == 4).Select(b => new { VL_EVOLUCAO = b.Field<decimal>("VL_EVOLUCAO") }); decimal COL13 = _var.Count() > 0 ? _var.ToList().Sum(c => c.VL_EVOLUCAO) : 0;

                            _var = TBL.Where(a => a.Field<Decimal>("DT_BAIXA") == Decimal.Parse(DT_REFERENCIA.ToString("yyyyMMdd")) && a.Field<Int16>("CD_MOTIVO_BAIXA") == 5 && a.Field<string>("TP_CONTRATO") == "C").Select(b => new { VL_EVOLUCAO = b.Field<decimal>("VL_EVOLUCAO") }); decimal COL14 = _var.Count() > 0 ? _var.ToList()[0].VL_EVOLUCAO : 0;
                            _var = TBL.Where(a => a.Field<Decimal>("DT_BAIXA") == Decimal.Parse(DT_REFERENCIA.ToString("yyyyMMdd")) && a.Field<Int16>("CD_MOTIVO_BAIXA") == 5 && a.Field<string>("TP_CONTRATO") == "H").Select(b => new { VL_EVOLUCAO = b.Field<decimal>("VL_EVOLUCAO") }); decimal COL15 = _var.Count() > 0 ? _var.ToList()[0].VL_EVOLUCAO : 0;
                            _var = TBL.Where(a => a.Field<Decimal>("DT_BAIXA") == Decimal.Parse(DT_REFERENCIA.ToString("yyyyMMdd")) && a.Field<Int16>("CD_MOTIVO_BAIXA") == 5).Select(b => new { VL_EVOLUCAO = b.Field<decimal>("VL_EVOLUCAO") }); decimal COL16 = _var.Count() > 0 ? _var.ToList().Sum(c => c.VL_EVOLUCAO) : 0;

                            _var = TBL.Where(a => a.Field<Decimal>("DT_BAIXA") == Decimal.Parse(DT_REFERENCIA.ToString("yyyyMMdd")) && a.Field<string>("TP_CONTRATO") == "C").Select(b => new { VL_EVOLUCAO = b.Field<decimal>("VL_EVOLUCAO") }); decimal COL17 = _var.Count() > 0 ? _var.ToList()[0].VL_EVOLUCAO : 0;
                            _var = TBL.Where(a => a.Field<Decimal>("DT_BAIXA") == Decimal.Parse(DT_REFERENCIA.ToString("yyyyMMdd")) && a.Field<string>("TP_CONTRATO") == "H").Select(b => new { VL_EVOLUCAO = b.Field<decimal>("VL_EVOLUCAO") }); decimal COL18 = _var.Count() > 0 ? _var.ToList()[0].VL_EVOLUCAO : 0;
                            _var = TBL.Where(a => a.Field<Decimal>("DT_BAIXA") == Decimal.Parse(DT_REFERENCIA.ToString("yyyyMMdd"))).Select(b => new { VL_EVOLUCAO = b.Field<decimal>("VL_EVOLUCAO") }); decimal COL19 = _var.Count() > 0 ? _var.ToList().Sum(c => c.VL_EVOLUCAO) : 0;

                            dtPorBaixa.Rows.Add(
                                  (DT_REFERENCIA.ToString("dddd", new System.Globalization.CultureInfo("pt-BR"))).Substring(0, 3).ToLower()
                                , DT_REFERENCIA.ToString("dd/MM/yyyy")
                                , COL02 != 0 ? string.Format("R$ {0:N2}", COL02) : "&nbsp;"
                                , COL03 != 0 ? string.Format("R$ {0:N2}", COL03) : "&nbsp;"
                                , COL04 != 0 ? string.Format("R$ {0:N2}", COL04) : "&nbsp;"
                                , COL05 != 0 ? string.Format("R$ {0:N2}", COL05) : "&nbsp;"
                                , COL06 != 0 ? string.Format("R$ {0:N2}", COL06) : "&nbsp;"
                                , COL07 != 0 ? string.Format("R$ {0:N2}", COL07) : "&nbsp;"
                                , COL08 != 0 ? string.Format("R$ {0:N2}", COL08) : "&nbsp;"
                                , COL09 != 0 ? string.Format("R$ {0:N2}", COL09) : "&nbsp;"
                                , COL10 != 0 ? string.Format("R$ {0:N2}", COL10) : "&nbsp;"
                                , COL11 != 0 ? string.Format("R$ {0:N2}", COL11) : "&nbsp;"
                                , COL12 != 0 ? string.Format("R$ {0:N2}", COL12) : "&nbsp;"
                                , COL13 != 0 ? string.Format("R$ {0:N2}", COL13) : "&nbsp;"
                                , COL14 != 0 ? string.Format("R$ {0:N2}", COL14) : "&nbsp;"
                                , COL15 != 0 ? string.Format("R$ {0:N2}", COL15) : "&nbsp;"
                                , COL16 != 0 ? string.Format("R$ {0:N2}", COL16) : "&nbsp;"
                                , COL17 != 0 ? string.Format("R$ {0:N2}", COL17) : "&nbsp;"
                                , COL18 != 0 ? string.Format("R$ {0:N2}", COL18) : "&nbsp;"
                                , COL19 != 0 ? string.Format("R$ {0:N2}", COL19) : "&nbsp;"
                                );
                        }
                        DT_REFERENCIA = DT_REFERENCIA.AddDays(1);
                    }

                    var
                    _var1 = TBL.Where(a => a.Field<Decimal>("DT_BAIXA") == 99999999 && a.Field<Int16>("CD_MOTIVO_BAIXA") == 2 && a.Field<string>("TP_CONTRATO") == "C").Select(b => new { VL_EVOLUCAO = b.Field<decimal>("VL_EVOLUCAO") }); decimal _COL05 = _var1.Count() > 0 ? _var1.ToList().Sum(c => c.VL_EVOLUCAO) : 0;
                    _var1 = TBL.Where(a => a.Field<Decimal>("DT_BAIXA") == 99999999 && a.Field<Int16>("CD_MOTIVO_BAIXA") == 2 && a.Field<string>("TP_CONTRATO") == "H").Select(b => new { VL_EVOLUCAO = b.Field<decimal>("VL_EVOLUCAO") }); decimal _COL06 = _var1.Count() > 0 ? _var1.ToList().Sum(c => c.VL_EVOLUCAO) : 0;
                    _var1 = TBL.Where(a => a.Field<Decimal>("DT_BAIXA") == 99999999 && a.Field<Int16>("CD_MOTIVO_BAIXA") == 2).Select(b => new { VL_EVOLUCAO = b.Field<decimal>("VL_EVOLUCAO") }); decimal _COL07 = _var1.Count() > 0 ? _var1.ToList().Sum(c => c.VL_EVOLUCAO) : 0;
                    if (_COL07 != 0)
                        dtPorBaixa.Rows.Add(
                              "&nbsp;"
                            , "&nbsp;"
                            , "&nbsp;"
                            , "&nbsp;"
                            , "&nbsp;"
                            , _COL05 != 0 ? string.Format("<span style=\"color: red;\">*</span> R$ {0:N2}", _COL05) : "&nbsp;"
                            , _COL06 != 0 ? string.Format("<span style=\"color: red;\">*</span> R$ {0:N2}", _COL06) : "&nbsp;"
                            , _COL07 != 0 ? string.Format("<span style=\"color: red;\">*</span> R$ {0:N2}", _COL07) : "&nbsp;"
                            , "&nbsp;"
                            , "&nbsp;"
                            , "&nbsp;"
                            , "&nbsp;"
                            , "&nbsp;"
                            , "&nbsp;"
                            , "&nbsp;"
                            , "&nbsp;"
                            , "&nbsp;"
                            , "&nbsp;"
                            , "&nbsp;"
                            , "&nbsp;"
                            );

                    _var1 = TBL.Where(a => a.Field<Int16>("CD_MOTIVO_BAIXA") == 1 && a.Field<string>("TP_CONTRATO") == "C").Select(b => new { VL_EVOLUCAO = b.Field<decimal>("VL_EVOLUCAO") }); decimal _COL02 = _var1.Count() > 0 ? _var1.ToList().Sum(c => c.VL_EVOLUCAO) : 0;
                    _var1 = TBL.Where(a => a.Field<Int16>("CD_MOTIVO_BAIXA") == 1 && a.Field<string>("TP_CONTRATO") == "H").Select(b => new { VL_EVOLUCAO = b.Field<decimal>("VL_EVOLUCAO") }); decimal _COL03 = _var1.Count() > 0 ? _var1.ToList().Sum(c => c.VL_EVOLUCAO) : 0;
                    _var1 = TBL.Where(a => a.Field<Int16>("CD_MOTIVO_BAIXA") == 1).Select(b => new { VL_EVOLUCAO = b.Field<decimal>("VL_EVOLUCAO") }); decimal _COL04 = _var1.Count() > 0 ? _var1.ToList().Sum(c => c.VL_EVOLUCAO) : 0;

                    _var1 = TBL.Where(a => a.Field<Int16>("CD_MOTIVO_BAIXA") == 2 && a.Field<string>("TP_CONTRATO") == "C").Select(b => new { VL_EVOLUCAO = b.Field<decimal>("VL_EVOLUCAO") }); _COL05 = _var1.Count() > 0 ? _var1.ToList().Sum(c => c.VL_EVOLUCAO) : 0;
                    _var1 = TBL.Where(a => a.Field<Int16>("CD_MOTIVO_BAIXA") == 2 && a.Field<string>("TP_CONTRATO") == "H").Select(b => new { VL_EVOLUCAO = b.Field<decimal>("VL_EVOLUCAO") }); _COL06 = _var1.Count() > 0 ? _var1.ToList().Sum(c => c.VL_EVOLUCAO) : 0;
                    _var1 = TBL.Where(a => a.Field<Int16>("CD_MOTIVO_BAIXA") == 2).Select(b => new { VL_EVOLUCAO = b.Field<decimal>("VL_EVOLUCAO") }); _COL07 = _var1.Count() > 0 ? _var1.ToList().Sum(c => c.VL_EVOLUCAO) : 0;

                    _var1 = TBL.Where(a => a.Field<Int16>("CD_MOTIVO_BAIXA") == 3 && a.Field<string>("TP_CONTRATO") == "C").Select(b => new { VL_EVOLUCAO = b.Field<decimal>("VL_EVOLUCAO") }); decimal _COL08 = _var1.Count() > 0 ? _var1.ToList().Sum(c => c.VL_EVOLUCAO) : 0;
                    _var1 = TBL.Where(a => a.Field<Int16>("CD_MOTIVO_BAIXA") == 3 && a.Field<string>("TP_CONTRATO") == "H").Select(b => new { VL_EVOLUCAO = b.Field<decimal>("VL_EVOLUCAO") }); decimal _COL09 = _var1.Count() > 0 ? _var1.ToList().Sum(c => c.VL_EVOLUCAO) : 0;
                    _var1 = TBL.Where(a => a.Field<Int16>("CD_MOTIVO_BAIXA") == 3).Select(b => new { VL_EVOLUCAO = b.Field<decimal>("VL_EVOLUCAO") }); decimal _COL10 = _var1.Count() > 0 ? _var1.ToList().Sum(c => c.VL_EVOLUCAO) : 0;

                    _var1 = TBL.Where(a => a.Field<Int16>("CD_MOTIVO_BAIXA") == 4 && a.Field<string>("TP_CONTRATO") == "C").Select(b => new { VL_EVOLUCAO = b.Field<decimal>("VL_EVOLUCAO") }); decimal _COL11 = _var1.Count() > 0 ? _var1.ToList().Sum(c => c.VL_EVOLUCAO) : 0;
                    _var1 = TBL.Where(a => a.Field<Int16>("CD_MOTIVO_BAIXA") == 4 && a.Field<string>("TP_CONTRATO") == "H").Select(b => new { VL_EVOLUCAO = b.Field<decimal>("VL_EVOLUCAO") }); decimal _COL12 = _var1.Count() > 0 ? _var1.ToList().Sum(c => c.VL_EVOLUCAO) : 0;
                    _var1 = TBL.Where(a => a.Field<Int16>("CD_MOTIVO_BAIXA") == 4).Select(b => new { VL_EVOLUCAO = b.Field<decimal>("VL_EVOLUCAO") }); decimal _COL13 = _var1.Count() > 0 ? _var1.ToList().Sum(c => c.VL_EVOLUCAO) : 0;

                    _var1 = TBL.Where(a => a.Field<Int16>("CD_MOTIVO_BAIXA") == 5 && a.Field<string>("TP_CONTRATO") == "C").Select(b => new { VL_EVOLUCAO = b.Field<decimal>("VL_EVOLUCAO") }); decimal _COL14 = _var1.Count() > 0 ? _var1.ToList().Sum(c => c.VL_EVOLUCAO) : 0;
                    _var1 = TBL.Where(a => a.Field<Int16>("CD_MOTIVO_BAIXA") == 5 && a.Field<string>("TP_CONTRATO") == "H").Select(b => new { VL_EVOLUCAO = b.Field<decimal>("VL_EVOLUCAO") }); decimal _COL15 = _var1.Count() > 0 ? _var1.ToList().Sum(c => c.VL_EVOLUCAO) : 0;
                    _var1 = TBL.Where(a => a.Field<Int16>("CD_MOTIVO_BAIXA") == 5).Select(b => new { VL_EVOLUCAO = b.Field<decimal>("VL_EVOLUCAO") }); decimal _COL16 = _var1.Count() > 0 ? _var1.ToList().Sum(c => c.VL_EVOLUCAO) : 0;

                    _var1 = TBL.Where(a => a.Field<string>("TP_CONTRATO") == "C").Select(b => new { VL_EVOLUCAO = b.Field<decimal>("VL_EVOLUCAO") }); decimal _COL17 = _var1.Count() > 0 ? _var1.ToList().Sum(c => c.VL_EVOLUCAO) : 0;
                    _var1 = TBL.Where(a => a.Field<string>("TP_CONTRATO") == "H").Select(b => new { VL_EVOLUCAO = b.Field<decimal>("VL_EVOLUCAO") }); decimal _COL18 = _var1.Count() > 0 ? _var1.ToList().Sum(c => c.VL_EVOLUCAO) : 0;
                    _var1 = TBL.Select(b => new { VL_EVOLUCAO = b.Field<decimal>("VL_EVOLUCAO") }); decimal _COL19 = _var1.Count() > 0 ? _var1.ToList().Sum(c => c.VL_EVOLUCAO) : 0;

                    dtPorBaixa.Rows.Add(
                          ""
                        , "TOTAL"
                        , _COL02 != 0 ? string.Format("R$ {0:N2}", _COL02) : "&nbsp;"
                        , _COL03 != 0 ? string.Format("R$ {0:N2}", _COL03) : "&nbsp;"
                        , _COL04 != 0 ? string.Format("R$ {0:N2}", _COL04) : "&nbsp;"
                        , _COL05 != 0 ? string.Format("R$ {0:N2}", _COL05) : "&nbsp;"
                        , _COL06 != 0 ? string.Format("R$ {0:N2}", _COL06) : "&nbsp;"
                        , _COL07 != 0 ? string.Format("R$ {0:N2}", _COL07) : "&nbsp;"
                        , _COL08 != 0 ? string.Format("R$ {0:N2}", _COL08) : "&nbsp;"
                        , _COL09 != 0 ? string.Format("R$ {0:N2}", _COL09) : "&nbsp;"
                        , _COL10 != 0 ? string.Format("R$ {0:N2}", _COL10) : "&nbsp;"
                        , _COL11 != 0 ? string.Format("R$ {0:N2}", _COL11) : "&nbsp;"
                        , _COL12 != 0 ? string.Format("R$ {0:N2}", _COL12) : "&nbsp;"
                        , _COL13 != 0 ? string.Format("R$ {0:N2}", _COL13) : "&nbsp;"
                        , _COL14 != 0 ? string.Format("R$ {0:N2}", _COL14) : "&nbsp;"
                        , _COL15 != 0 ? string.Format("R$ {0:N2}", _COL15) : "&nbsp;"
                        , _COL16 != 0 ? string.Format("R$ {0:N2}", _COL16) : "&nbsp;"
                        , _COL17 != 0 ? string.Format("R$ {0:N2}", _COL17) : "&nbsp;"
                        , _COL18 != 0 ? string.Format("R$ {0:N2}", _COL18) : "&nbsp;"
                        , _COL19 != 0 ? string.Format("R$ {0:N2}", _COL19) : "&nbsp;"
                        );

                    dsFinal.Tables.Add(dtPorBaixa);
                    dsFinal.Tables.Add(dsReversao.Tables[1].Copy());
                }
                return dsFinal;
            }
            catch (Exception ex)
            {
                throw new Exception("FAT.CmdReversao_001: " + ex.Message, ex);
            }
        }

        public object[] GeraRelatorioReversaoProduto(DateTime DT_REFERENCIA, string TP_CONTRATO)
        {
            try
            {
                SqlCommand sqlcommand = new SqlCommand();
                sqlcommand.CommandType = CommandType.StoredProcedure;
                sqlcommand.CommandText = "SP_REV_GERA_RESUMO_REVERSAO_PRODUTO";
                sqlcommand.Parameters.AddWithValue("@DT_REFERENCIA", DT_REFERENCIA.ToString("yyyy-MM-dd HH:mm:ss"));
                sqlcommand.Parameters.AddWithValue("@TP_CONTRATO", TP_CONTRATO);

                DAL_MIS AcessaDadosCaixa = new Intranet.DAL.DAL_MIS();

                DataSet dsReversaoProduto = AcessaDadosCaixa.ConsultaSQL(sqlcommand);

                object[] retorno = new object[2];

                if ((dsReversaoProduto.Tables.Count > 0) && (dsReversaoProduto.Tables[0].Rows.Count > 0))
                {
                    // Gera Html Titulo da tabela
                    DataView dv = (dsReversaoProduto.Tables[0].DefaultView.ToTable(true, "DT_DISPONIBILIZACAO")).DefaultView;
                    dv.Sort = "DT_DISPONIBILIZACAO asc";
                    DataTable DataDistinct = dv.ToTable();

                    System.Web.UI.HtmlControls.HtmlGenericControl HtmlTableFinal = GeraTabelaHtml(DataDistinct);

                    DataTable ProdutoDistinct = dsReversaoProduto.Tables[0].DefaultView.ToTable(true, "NR_PRODUTO", "TP_CONTRATO", "NM_PRODUTO");

                    DataTable dtReversaoProduto = new DataTable("dtReversaoProduto");
                    dtReversaoProduto.Columns.Add("NR_PRODUTO");
                    dtReversaoProduto.Columns.Add("TP_CONTRATO");
                    dtReversaoProduto.Columns.Add("NM_PRODUTO");

                    dtReversaoProduto.Columns.Add("NR_VL_01", typeof(decimal));
                    dtReversaoProduto.Columns.Add("NR_VL_02", typeof(decimal));
                    dtReversaoProduto.Columns.Add("NR_IN_03", typeof(decimal));

                    for (int i = 0; i < DataDistinct.Rows.Count; i++)
                    {
                        dtReversaoProduto.Columns.Add(string.Format("NR_VL{0}_01", (i + 1)), typeof(decimal));
                        dtReversaoProduto.Columns.Add(string.Format("NR_VL{0}_02", (i + 1)), typeof(decimal));
                        dtReversaoProduto.Columns.Add(string.Format("NR_IN{0}_03", (i + 1)), typeof(decimal));
                    }

                    decimal TOT01 = 0;
                    decimal TOT02 = 0;
                    decimal TOT03 = 0;
                    
                    foreach (DataRow drProduto in ProdutoDistinct.Rows)
                    {
                        Int32 NR_PRODUTO = Int32.Parse(drProduto["NR_PRODUTO"].ToString());

                        DataRow NovaLinha = dtReversaoProduto.NewRow();
                        NovaLinha["NR_PRODUTO"] = string.Format("{0:00000}", NR_PRODUTO);
                        NovaLinha["TP_CONTRATO"] = drProduto["TP_CONTRATO"];
                        NovaLinha["NM_PRODUTO"] = drProduto["NM_PRODUTO"];

                        var TOTAL = dsReversaoProduto.Tables[0].AsEnumerable().Where(a => a.Field<Int32>("NR_PRODUTO") == NR_PRODUTO).Select(s => new { VL_SALDO_DIARIO = s.Field<decimal>("VL_SALDO_DIARIO"), VL_RECUPERACAO = s.Field<decimal>("VL_RECUPERACAO") });
                        TOT01 = TOTAL.Count() > 0 ? TOTAL.Sum(s => s.VL_SALDO_DIARIO) : 0;
                        TOT02 = TOTAL.Count() > 0 ? TOTAL.Sum(s => s.VL_RECUPERACAO) : 0;
                        TOT03 = TOT01 != 0 ? (TOT02 / TOT01) * 100 : 0;

                        NovaLinha["NR_VL_01"] = TOT01;
                        NovaLinha["NR_VL_02"] = TOT02;
                        NovaLinha["NR_IN_03"] = TOT03;

                        for (int r = 0; r < DataDistinct.Rows.Count; r++)
                        {
                            decimal DataRef = decimal.Parse(DataDistinct.Rows[r]["DT_DISPONIBILIZACAO"].ToString());
                            var ValorC = dsReversaoProduto.Tables[0].AsEnumerable().Where(a => a.Field<decimal>("DT_DISPONIBILIZACAO") == DataRef && a.Field<Int32>("NR_PRODUTO") == NR_PRODUTO).Select(s => new { VL_SALDO_DIARIO = s.Field<decimal>("VL_SALDO_DIARIO"), VL_RECUPERACAO = s.Field<decimal>("VL_RECUPERACAO") });
                            TOT01 = ValorC.Count() > 0 ? ValorC.ToList()[0].VL_SALDO_DIARIO : 0;
                            TOT02 = ValorC.Count() > 0 ? ValorC.ToList()[0].VL_RECUPERACAO : 0;
                            TOT03 = TOT01 != 0 ? (TOT02 / TOT01) * 100 : 0;

                            NovaLinha[0 + (((r + 3) - 1) * 3)] = TOT01;
                            NovaLinha[1 + (((r + 3) - 1) * 3)] = TOT02;
                            NovaLinha[2 + (((r + 3) - 1) * 3)] = TOT03;
                        }
                        dtReversaoProduto.Rows.Add(NovaLinha);
                    }

                    dv = dtReversaoProduto.DefaultView;
                    dv.Sort = "NR_VL_01 desc";
                    dtReversaoProduto = dv.ToTable();

                    DataRow LinhaTotal = dtReversaoProduto.NewRow();
                    LinhaTotal["NR_PRODUTO"] = "";
                    LinhaTotal["TP_CONTRATO"] = "";
                    LinhaTotal["NM_PRODUTO"] = "TOTAL";

                    int Col = 3;
                    while (Col < dtReversaoProduto.Columns.Count)
                    {
                        TOT01 = dtReversaoProduto.AsEnumerable().Sum(s => s.Field<decimal>(dtReversaoProduto.Columns[Col].ColumnName));
                        TOT02 = dtReversaoProduto.AsEnumerable().Sum(s => s.Field<decimal>(dtReversaoProduto.Columns[Col + 1].ColumnName));
                        TOT03 = TOT01 != 0 ? (TOT02 / TOT01) * 100 : 0;

                        LinhaTotal[Col] = TOT01;
                        LinhaTotal[Col + 1] = TOT02;
                        LinhaTotal[Col + 2] = TOT03;

                        Col += 3;
                    }

                    dtReversaoProduto.Rows.Add(LinhaTotal);

                    foreach (DataRow dr in dtReversaoProduto.Rows)
                        HtmlTableFinal.Controls.Add(GeraLihaTabelaHtml(dr));

                    retorno[0] = HtmlTableFinal;
                    retorno[1] = ((DateTime)dsReversaoProduto.Tables[1].Rows[0][0]).ToString("dd/MM/yyyy  HH:mm");
                }
                return retorno;
            }
            catch (Exception ex)
            {
                throw new Exception("FAT.CmdReversao_001: " + ex.Message, ex);
            }
        }

        private System.Web.UI.HtmlControls.HtmlGenericControl GeraTabelaHtml(DataTable dtData)
        {
            try
            {
                System.Web.UI.HtmlControls.HtmlGenericControl HtmlTable = new System.Web.UI.HtmlControls.HtmlGenericControl("table");
                HtmlTable.Attributes["runat"] = "server";
                HtmlTable.Attributes["id"] = "Table";
                HtmlTable.Attributes["class"] = "GridView-Table";

                #region Monta primeira linha da tabela

                System.Web.UI.HtmlControls.HtmlGenericControl HtmlRow = new System.Web.UI.HtmlControls.HtmlGenericControl("tr");
                HtmlRow.Attributes["class"] = "GridView-Header";

                System.Web.UI.HtmlControls.HtmlGenericControl HtmlCol = new System.Web.UI.HtmlControls.HtmlGenericControl("th");
                HtmlCol.Attributes["colspan"] = "3";
                HtmlRow.Controls.Add(HtmlCol);

                HtmlCol = new System.Web.UI.HtmlControls.HtmlGenericControl("th");
                HtmlCol.Attributes["colspan"] = "3";
                HtmlCol.InnerHtml = "TOTAL";
                HtmlRow.Controls.Add(HtmlCol);

                foreach (DataRow dr in dtData.Rows)
                {
                    HtmlCol = new System.Web.UI.HtmlControls.HtmlGenericControl("th");
                    HtmlCol.Attributes["colspan"] = "3";
                    HtmlCol.InnerHtml = DateTime.Parse(string.Format("{0:####/##/##}", Int64.Parse(dr[0].ToString()))).ToString("dd/MM/yyyy");
                    HtmlRow.Controls.Add(HtmlCol);
                }
                HtmlTable.Controls.Add(HtmlRow);

                #endregion

                #region Monta segunda linha da tabela

                HtmlRow = new System.Web.UI.HtmlControls.HtmlGenericControl("tr");

                HtmlCol = new System.Web.UI.HtmlControls.HtmlGenericControl("th");
                HtmlCol.Attributes["class"] = "GridView-SubHeader";
                HtmlCol.InnerHtml = "CÓDIGO";
                HtmlRow.Controls.Add(HtmlCol);

                HtmlCol = new System.Web.UI.HtmlControls.HtmlGenericControl("th");
                HtmlCol.Attributes["class"] = "GridView-SubHeader";
                HtmlCol.InnerHtml = "TIPO";
                HtmlRow.Controls.Add(HtmlCol);

                HtmlCol = new System.Web.UI.HtmlControls.HtmlGenericControl("th");
                HtmlCol.Attributes["class"] = "GridView-SubHeader";
                HtmlCol.InnerHtml = "PRODUTO";
                HtmlRow.Controls.Add(HtmlCol);

                for (int i = 0; i < dtData.Rows.Count + 1; i++)
                {
                    HtmlCol = new System.Web.UI.HtmlControls.HtmlGenericControl("th");
                    HtmlCol.Attributes["class"] = "GridView-SubHeader";
                    HtmlCol.InnerHtml = "CARTEIRA";
                    HtmlRow.Controls.Add(HtmlCol);

                    HtmlCol = new System.Web.UI.HtmlControls.HtmlGenericControl("th");
                    HtmlCol.Attributes["class"] = "GridView-SubHeader";
                    HtmlCol.InnerHtml = "REVERSÃO";
                    HtmlRow.Controls.Add(HtmlCol);

                    HtmlCol = new System.Web.UI.HtmlControls.HtmlGenericControl("th");
                    HtmlCol.Attributes["class"] = "GridView-SubHeader";
                    HtmlCol.InnerHtml = "INDICE";
                    HtmlRow.Controls.Add(HtmlCol);
                }

                HtmlTable.Controls.Add(HtmlRow);

                #endregion

                //StringBuilder generatedHtml = new StringBuilder();
                //System.Web.UI.HtmlTextWriter htw = new System.Web.UI.HtmlTextWriter(new System.IO.StringWriter(generatedHtml));
                //HtmlTable.RenderControl(htw);
                //string output = generatedHtml.ToString();

                return HtmlTable;
            }
            catch (Exception ex)
            {
                throw new Exception("FAT.CmdReversao_001: " + ex.Message, ex);
            }
        }

        private System.Web.UI.HtmlControls.HtmlGenericControl GeraLihaTabelaHtml(DataRow drProduto)
        {
            System.Web.UI.HtmlControls.HtmlGenericControl HtmlTableRow = new System.Web.UI.HtmlControls.HtmlGenericControl("tr");
            HtmlTableRow.Attributes["class"] = "GridView-Rows";

            System.Web.UI.HtmlControls.HtmlGenericControl HtmlTableCol = new System.Web.UI.HtmlControls.HtmlGenericControl("td");
            HtmlTableCol.Attributes["class"] = "GridView-Cells Column-Title Centralizado";
            HtmlTableCol.Attributes["style"] = "background-color: silver;";
            HtmlTableCol.InnerHtml = drProduto["NR_PRODUTO"].ToString();
            HtmlTableRow.Controls.Add(HtmlTableCol);

            HtmlTableCol = new System.Web.UI.HtmlControls.HtmlGenericControl("td");
            HtmlTableCol.Attributes["class"] = "GridView-Cells Column-Title Centralizado";
            HtmlTableCol.Attributes["style"] = "background-color: silver;";
            HtmlTableCol.InnerHtml = drProduto["TP_CONTRATO"].ToString();
            HtmlTableRow.Controls.Add(HtmlTableCol);

            HtmlTableCol = new System.Web.UI.HtmlControls.HtmlGenericControl("td");
            HtmlTableCol.Attributes["class"] = "GridView-Cells Column-Title Esquerda";
            HtmlTableCol.Attributes["style"] = "background-color: silver;";
            HtmlTableCol.InnerHtml = drProduto["NM_PRODUTO"].ToString();
            HtmlTableRow.Controls.Add(HtmlTableCol);

            int cont = 1;
            bool ColImpar = false;

            for (int c = 3; c < drProduto.Table.Columns.Count; c++)
            {
                System.Web.UI.HtmlControls.HtmlGenericControl HtmlCol = new System.Web.UI.HtmlControls.HtmlGenericControl("td");
                decimal valor = (decimal)drProduto[c];

                HtmlCol.Attributes["class"] = ColImpar == true ? "GridView-Cells Direita Impar" : "GridView-Cells Direita";

                if (cont == 3)
                {
                    HtmlCol.InnerHtml = valor != 0 ? string.Format("{0:N2} %", valor) : "&nbsp;";

                    HtmlCol.Attributes["style"] = "font-weight: bold;";
                    ColImpar = ColImpar == false ? true : false;
                    cont = 0;
                }
                else
                {
                    HtmlCol.InnerHtml = valor != 0 ? string.Format("R$ {0:N2}", valor) : "&nbsp;";
                }

                HtmlTableRow.Controls.Add(HtmlCol);
                cont++;
            }

            return (HtmlTableRow);
        }
    }
}