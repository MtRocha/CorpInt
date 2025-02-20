using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;
using Intranet_NEW.Controllers.DAL;

namespace Intranet.BLL.CAR
{
    public class Mapeamento
    {
        public DataSet GeraRelatorioFaixa(int DT_REFERENCIA)
        {
            try
            {
                SqlCommand sqlcommand = new SqlCommand();
                sqlcommand.CommandType = CommandType.StoredProcedure;
                sqlcommand.CommandText = "SP_WEB_REM_CARGA_RESUMO";
                sqlcommand.Parameters.AddWithValue("@DT_ARQUIVO", DT_REFERENCIA);

                DAL_MIS AcessaDadosCaixa = new Intranet.DAL.DAL_MIS();

                DataSet dsFechamento = AcessaDadosCaixa.ConsultaSQL(sqlcommand);

                DataSet dsFinal = new DataSet("ReportFaixadeAtraso");
                if ((dsFechamento.Tables.Count > 0) && (dsFechamento.Tables[0].Rows.Count > 0))
                {
                    DataTable dtComercial = new DataTable("Comercial");
                    dtComercial.Columns.Add("COL01");
                    dtComercial.Columns.Add("COL02");
                    dtComercial.Columns.Add("COL03");
                    dtComercial.Columns.Add("COL04");
                    dtComercial.Columns.Add("COL05");
                    dtComercial.Columns.Add("COL06");
                    dtComercial.Columns.Add("COL07");
                    dtComercial.Columns.Add("COL08");
                    dtComercial.Columns.Add("COL09");
                    dtComercial.Columns.Add("COL10");

                    DataTable dtHabitacional = new DataTable("Habitacional");
                    dtHabitacional.Columns.Add("COL01");
                    dtHabitacional.Columns.Add("COL02");
                    dtHabitacional.Columns.Add("COL03");
                    dtHabitacional.Columns.Add("COL04");
                    dtHabitacional.Columns.Add("COL05");
                    dtHabitacional.Columns.Add("COL06");
                    dtHabitacional.Columns.Add("COL07");
                    dtHabitacional.Columns.Add("COL08");
                    dtHabitacional.Columns.Add("COL09");
                    dtHabitacional.Columns.Add("COL10");

                    DataTable dtTotal = new DataTable("Total");
                    dtTotal.Columns.Add("COL01");
                    dtTotal.Columns.Add("COL02");
                    dtTotal.Columns.Add("COL03");
                    dtTotal.Columns.Add("COL04");
                    dtTotal.Columns.Add("COL05");
                    dtTotal.Columns.Add("COL06");
                    dtTotal.Columns.Add("COL07");
                    dtTotal.Columns.Add("COL08");
                    dtTotal.Columns.Add("COL09");
                    dtTotal.Columns.Add("COL10");


                    dtComercial.Rows.Add(
                                      "FAIXA 01A10"
                                    , string.Format("{0:N0}", (Int32)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("TP_CONTRATO") == "C" && a.Field<string>("NM_FAIXA_ATRASO") == "01 a 10" && a.Field<Boolean>("NR_STATUS") == false).Sum(x => x.Field<Int32>("QT_CLIENTE")))
                                    , string.Format("R$ {0:N2}", (decimal)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("TP_CONTRATO") == "C" && a.Field<string>("NM_FAIXA_ATRASO") == "01 a 10" && a.Field<Boolean>("NR_STATUS") == false).Sum(x => x.Field<decimal>("VL_SALDO")))
                                    , string.Format("R$ {0:N2}", (decimal)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("TP_CONTRATO") == "C" && a.Field<string>("NM_FAIXA_ATRASO") == "01 a 10" && a.Field<Boolean>("NR_STATUS") == false).Sum(x => x.Field<decimal>("VL_SALDO") / x.Field<Int32>("QT_CLIENTE")))
                                    , string.Format("{0:N0}", (Int32)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("TP_CONTRATO") == "C" && a.Field<string>("NM_FAIXA_ATRASO") == "01 a 10" && a.Field<Boolean>("NR_STATUS") == true).Sum(x => x.Field<Int32>("QT_CLIENTE")))
                                    , string.Format("R$ {0:N2}", (decimal)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("TP_CONTRATO") == "C" && a.Field<string>("NM_FAIXA_ATRASO") == "01 a 10" && a.Field<Boolean>("NR_STATUS") == true).Sum(x => x.Field<decimal>("VL_SALDO")))
                                    , string.Format("R$ {0:N2}", (decimal)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("TP_CONTRATO") == "C" && a.Field<string>("NM_FAIXA_ATRASO") == "01 a 10" && a.Field<Boolean>("NR_STATUS") == true).Sum(x => x.Field<decimal>("VL_SALDO") / x.Field<Int32>("QT_CLIENTE")))
                                    , string.Format("{0:N0}", (Int32)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("TP_CONTRATO") == "C" && a.Field<string>("NM_FAIXA_ATRASO") == "01 a 10").Sum(x => x.Field<Int32>("QT_CLIENTE")))
                                    , string.Format("R$ {0:N2}", (decimal)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("TP_CONTRATO") == "C" && a.Field<string>("NM_FAIXA_ATRASO") == "01 a 10").Sum(x => x.Field<decimal>("VL_SALDO")))
                                    , string.Format("R$ {0:N2}", (decimal)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("TP_CONTRATO") == "C" && a.Field<string>("NM_FAIXA_ATRASO") == "01 a 10").Sum(x => x.Field<decimal>("VL_SALDO")) / (Int32)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("TP_CONTRATO") == "C" && a.Field<string>("NM_FAIXA_ATRASO") == "01 a 10").Sum(x => x.Field<Int32>("QT_CLIENTE")))
                                    );


                    dtComercial.Rows.Add(
                                      "FAIXA 11A20"
                                    , string.Format("{0:N0}", (Int32)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("TP_CONTRATO") == "C" && a.Field<string>("NM_FAIXA_ATRASO") == "11 a 20" && a.Field<Boolean>("NR_STATUS") == false).Sum(x => x.Field<Int32>("QT_CLIENTE")))
                                    , string.Format("R$ {0:N2}", (decimal)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("TP_CONTRATO") == "C" && a.Field<string>("NM_FAIXA_ATRASO") == "11 a 20" && a.Field<Boolean>("NR_STATUS") == false).Sum(x => x.Field<decimal>("VL_SALDO")))
                                    , string.Format("R$ {0:N2}", (decimal)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("TP_CONTRATO") == "C" && a.Field<string>("NM_FAIXA_ATRASO") == "11 a 20" && a.Field<Boolean>("NR_STATUS") == false).Sum(x => x.Field<decimal>("VL_SALDO") / x.Field<Int32>("QT_CLIENTE")))
                                    , string.Format("{0:N0}", (Int32)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("TP_CONTRATO") == "C" && a.Field<string>("NM_FAIXA_ATRASO") == "11 a 20" && a.Field<Boolean>("NR_STATUS") == true).Sum(x => x.Field<Int32>("QT_CLIENTE")))
                                    , string.Format("R$ {0:N2}", (decimal)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("TP_CONTRATO") == "C" && a.Field<string>("NM_FAIXA_ATRASO") == "11 a 20" && a.Field<Boolean>("NR_STATUS") == true).Sum(x => x.Field<decimal>("VL_SALDO")))
                                    , string.Format("R$ {0:N2}", (decimal)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("TP_CONTRATO") == "C" && a.Field<string>("NM_FAIXA_ATRASO") == "11 a 20" && a.Field<Boolean>("NR_STATUS") == true).Sum(x => x.Field<decimal>("VL_SALDO") / x.Field<Int32>("QT_CLIENTE")))
                                    , string.Format("{0:N0}", (Int32)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("TP_CONTRATO") == "C" && a.Field<string>("NM_FAIXA_ATRASO") == "11 a 20").Sum(x => x.Field<Int32>("QT_CLIENTE")))
                                    , string.Format("R$ {0:N2}", (decimal)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("TP_CONTRATO") == "C" && a.Field<string>("NM_FAIXA_ATRASO") == "11 a 20").Sum(x => x.Field<decimal>("VL_SALDO")))
                                    , string.Format("R$ {0:N2}", (decimal)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("TP_CONTRATO") == "C" && a.Field<string>("NM_FAIXA_ATRASO") == "11 a 20").Sum(x => x.Field<decimal>("VL_SALDO")) / (Int32)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("TP_CONTRATO") == "C" && a.Field<string>("NM_FAIXA_ATRASO") == "11 a 20").Sum(x => x.Field<Int32>("QT_CLIENTE")))
                                    );

                    dtComercial.Rows.Add(
                                      "FAIXA 21A30"
                                    , string.Format("{0:N0}", (Int32)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("TP_CONTRATO") == "C" && a.Field<string>("NM_FAIXA_ATRASO") == "21 a 30" && a.Field<Boolean>("NR_STATUS") == false).Sum(x => x.Field<Int32>("QT_CLIENTE")))
                                    , string.Format("R$ {0:N2}", (decimal)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("TP_CONTRATO") == "C" && a.Field<string>("NM_FAIXA_ATRASO") == "21 a 30" && a.Field<Boolean>("NR_STATUS") == false).Sum(x => x.Field<decimal>("VL_SALDO")))
                                    , string.Format("R$ {0:N2}", (decimal)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("TP_CONTRATO") == "C" && a.Field<string>("NM_FAIXA_ATRASO") == "21 a 30" && a.Field<Boolean>("NR_STATUS") == false).Sum(x => x.Field<decimal>("VL_SALDO") / x.Field<Int32>("QT_CLIENTE")))
                                    , string.Format("{0:N0}", (Int32)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("TP_CONTRATO") == "C" && a.Field<string>("NM_FAIXA_ATRASO") == "21 a 30" && a.Field<Boolean>("NR_STATUS") == true).Sum(x => x.Field<Int32>("QT_CLIENTE")))
                                    , string.Format("R$ {0:N2}", (decimal)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("TP_CONTRATO") == "C" && a.Field<string>("NM_FAIXA_ATRASO") == "21 a 30" && a.Field<Boolean>("NR_STATUS") == true).Sum(x => x.Field<decimal>("VL_SALDO")))
                                    , string.Format("R$ {0:N2}", (decimal)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("TP_CONTRATO") == "C" && a.Field<string>("NM_FAIXA_ATRASO") == "21 a 30" && a.Field<Boolean>("NR_STATUS") == true).Sum(x => x.Field<decimal>("VL_SALDO") / x.Field<Int32>("QT_CLIENTE")))
                                    , string.Format("{0:N0}", (Int32)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("TP_CONTRATO") == "C" && a.Field<string>("NM_FAIXA_ATRASO") == "21 a 30").Sum(x => x.Field<Int32>("QT_CLIENTE")))
                                    , string.Format("R$ {0:N2}", (decimal)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("TP_CONTRATO") == "C" && a.Field<string>("NM_FAIXA_ATRASO") == "21 a 30").Sum(x => x.Field<decimal>("VL_SALDO")))
                                    , string.Format("R$ {0:N2}", (decimal)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("TP_CONTRATO") == "C" && a.Field<string>("NM_FAIXA_ATRASO") == "21 a 30").Sum(x => x.Field<decimal>("VL_SALDO")) / (Int32)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("TP_CONTRATO") == "C" && a.Field<string>("NM_FAIXA_ATRASO") == "21 a 30").Sum(x => x.Field<Int32>("QT_CLIENTE")))
                                    );

                    dtComercial.Rows.Add(
                                     "FAIXA 31A60"
                                    , string.Format("{0:N0}", (Int32)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("TP_CONTRATO") == "C" && a.Field<string>("NM_FAIXA_ATRASO") == "31 a 60" && a.Field<Boolean>("NR_STATUS") == false).Sum(x => x.Field<Int32>("QT_CLIENTE")))
                                    , string.Format("R$ {0:N2}", (decimal)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("TP_CONTRATO") == "C" && a.Field<string>("NM_FAIXA_ATRASO") == "31 a 60" && a.Field<Boolean>("NR_STATUS") == false).Sum(x => x.Field<decimal>("VL_SALDO")))
                                    , string.Format("R$ {0:N2}", (decimal)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("TP_CONTRATO") == "C" && a.Field<string>("NM_FAIXA_ATRASO") == "31 a 60" && a.Field<Boolean>("NR_STATUS") == false).Sum(x => x.Field<decimal>("VL_SALDO") / x.Field<Int32>("QT_CLIENTE")))
                                    , string.Format("{0:N0}", (Int32)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("TP_CONTRATO") == "C" && a.Field<string>("NM_FAIXA_ATRASO") == "31 a 60" && a.Field<Boolean>("NR_STATUS") == true).Sum(x => x.Field<Int32>("QT_CLIENTE")))
                                    , string.Format("R$ {0:N2}", (decimal)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("TP_CONTRATO") == "C" && a.Field<string>("NM_FAIXA_ATRASO") == "31 a 60" && a.Field<Boolean>("NR_STATUS") == true).Sum(x => x.Field<decimal>("VL_SALDO")))
                                    , string.Format("R$ {0:N2}", (decimal)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("TP_CONTRATO") == "C" && a.Field<string>("NM_FAIXA_ATRASO") == "31 a 60" && a.Field<Boolean>("NR_STATUS") == true).Sum(x => x.Field<decimal>("VL_SALDO") / x.Field<Int32>("QT_CLIENTE")))
                                    , string.Format("{0:N0}", (Int32)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("TP_CONTRATO") == "C" && a.Field<string>("NM_FAIXA_ATRASO") == "31 a 60").Sum(x => x.Field<Int32>("QT_CLIENTE")))
                                    , string.Format("R$ {0:N2}", (decimal)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("TP_CONTRATO") == "C" && a.Field<string>("NM_FAIXA_ATRASO") == "31 a 60").Sum(x => x.Field<decimal>("VL_SALDO")))
                                    , string.Format("R$ {0:N2}", (decimal)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("TP_CONTRATO") == "C" && a.Field<string>("NM_FAIXA_ATRASO") == "31 a 60").Sum(x => x.Field<decimal>("VL_SALDO")) / (Int32)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("TP_CONTRATO") == "C" && a.Field<string>("NM_FAIXA_ATRASO") == "31 a 60").Sum(x => x.Field<Int32>("QT_CLIENTE")))
                                    );

                    dtComercial.Rows.Add(
                                     "FAIXA MAIOR 60"
                                    , string.Format("{0:N0}", (Int32)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("TP_CONTRATO") == "C" && a.Field<string>("NM_FAIXA_ATRASO") == "Maior_60" && a.Field<Boolean>("NR_STATUS") == false).Sum(x => x.Field<Int32>("QT_CLIENTE")))
                                    , string.Format("R$ {0:N2}", (decimal)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("TP_CONTRATO") == "C" && a.Field<string>("NM_FAIXA_ATRASO") == "Maior_60" && a.Field<Boolean>("NR_STATUS") == false).Sum(x => x.Field<decimal>("VL_SALDO")))
                                    , string.Format("R$ {0:N2}", (decimal)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("TP_CONTRATO") == "C" && a.Field<string>("NM_FAIXA_ATRASO") == "Maior_60" && a.Field<Boolean>("NR_STATUS") == false).Sum(x => x.Field<decimal>("VL_SALDO") / x.Field<Int32>("QT_CLIENTE")))
                                    , string.Format("{0:N0}", (Int32)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("TP_CONTRATO") == "C" && a.Field<string>("NM_FAIXA_ATRASO") == "Maior_60" && a.Field<Boolean>("NR_STATUS") == true).Sum(x => x.Field<Int32>("QT_CLIENTE")))
                                    , string.Format("R$ {0:N2}", (decimal)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("TP_CONTRATO") == "C" && a.Field<string>("NM_FAIXA_ATRASO") == "Maior_60" && a.Field<Boolean>("NR_STATUS") == true).Sum(x => x.Field<decimal>("VL_SALDO")))
                                    , string.Format("R$ {0:N2}", (decimal)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("TP_CONTRATO") == "C" && a.Field<string>("NM_FAIXA_ATRASO") == "Maior_60" && a.Field<Boolean>("NR_STATUS") == true).Sum(x => x.Field<decimal>("VL_SALDO") / x.Field<Int32>("QT_CLIENTE")))
                                    , string.Format("{0:N0}", (Int32)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("TP_CONTRATO") == "C" && a.Field<string>("NM_FAIXA_ATRASO") == "Maior_60").Sum(x => x.Field<Int32>("QT_CLIENTE")))
                                    , string.Format("R$ {0:N2}", (decimal)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("TP_CONTRATO") == "C" && a.Field<string>("NM_FAIXA_ATRASO") == "Maior_60").Sum(x => x.Field<decimal>("VL_SALDO")))
                                    , string.Format("R$ {0:N2}", (decimal)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("TP_CONTRATO") == "C" && a.Field<string>("NM_FAIXA_ATRASO") == "Maior_60").Sum(x => x.Field<decimal>("VL_SALDO")) / (Int32)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("TP_CONTRATO") == "C" && a.Field<string>("NM_FAIXA_ATRASO") == "Maior_60").Sum(x => x.Field<Int32>("QT_CLIENTE")))
                                    );



                    dtComercial.Rows.Add(
                                     "SUBTOTAL"
                                    , string.Format("{0:N0}", (Int32)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("TP_CONTRATO") == "C" && a.Field<Boolean>("NR_STATUS") == false).Sum(x => x.Field<Int32>("QT_CLIENTE")))
                                    , string.Format("R$ {0:N2}", (decimal)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("TP_CONTRATO") == "C" && a.Field<Boolean>("NR_STATUS") == false).Sum(x => x.Field<decimal>("VL_SALDO")))
                                    , string.Format("R$ {0:N2}", (decimal)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("TP_CONTRATO") == "C" && a.Field<Boolean>("NR_STATUS") == false).Sum(x => x.Field<decimal>("VL_SALDO")) / (Int32)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("TP_CONTRATO") == "C" && a.Field<Boolean>("NR_STATUS") == false).Sum(x => x.Field<Int32>("QT_CLIENTE")) > 0 ? (Int32)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("TP_CONTRATO") == "C" && a.Field<Boolean>("NR_STATUS") == false).Sum(x => x.Field<Int32>("QT_CLIENTE")) : 1)
                                    , string.Format("{0:N0}", (Int32)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("TP_CONTRATO") == "C" && a.Field<Boolean>("NR_STATUS") == true).Sum(x => x.Field<Int32>("QT_CLIENTE")))
                                    , string.Format("R$ {0:N2}", (decimal)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("TP_CONTRATO") == "C" && a.Field<Boolean>("NR_STATUS") == true).Sum(x => x.Field<decimal>("VL_SALDO")))
                                    , string.Format("R$ {0:N2}", (decimal)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("TP_CONTRATO") == "C" && a.Field<Boolean>("NR_STATUS") == true).Sum(x => x.Field<decimal>("VL_SALDO")) / (Int32)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("TP_CONTRATO") == "C" && a.Field<Boolean>("NR_STATUS") == true).Sum(x => x.Field<Int32>("QT_CLIENTE")) > 0 ? (Int32)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("TP_CONTRATO") == "C" && a.Field<Boolean>("NR_STATUS") == true).Sum(x => x.Field<Int32>("QT_CLIENTE")) : 1)
                                    , string.Format("{0:N0}", (Int32)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("TP_CONTRATO") == "C").Sum(x => x.Field<Int32>("QT_CLIENTE")))
                                    , string.Format("R$ {0:N2}", (decimal)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("TP_CONTRATO") == "C").Sum(x => x.Field<decimal>("VL_SALDO")))
                                    , string.Format("R$ {0:N2}", (decimal)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("TP_CONTRATO") == "C").Sum(x => x.Field<decimal>("VL_SALDO")) / (Int32)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("TP_CONTRATO") == "C").Sum(x => x.Field<Int32>("QT_CLIENTE")) > 0 ? (Int32)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("TP_CONTRATO") == "C").Sum(x => x.Field<Int32>("QT_CLIENTE")) : 1)
                         );


                    dtHabitacional.Rows.Add(
                                      "FAIXA 01A10"
                                    , string.Format("{0:N0}", (Int32)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("TP_CONTRATO") == "H" && a.Field<string>("NM_FAIXA_ATRASO") == "01 a 10" && a.Field<Boolean>("NR_STATUS") == false).Sum(x => x.Field<Int32>("QT_CLIENTE")))
                                    , string.Format("R$ {0:N2}", (decimal)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("TP_CONTRATO") == "H" && a.Field<string>("NM_FAIXA_ATRASO") == "01 a 10" && a.Field<Boolean>("NR_STATUS") == false).Sum(x => x.Field<decimal>("VL_SALDO")))
                                    , string.Format("R$ {0:N2}", (decimal)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("TP_CONTRATO") == "H" && a.Field<string>("NM_FAIXA_ATRASO") == "01 a 10" && a.Field<Boolean>("NR_STATUS") == false).Sum(x => x.Field<decimal>("VL_SALDO") / x.Field<Int32>("QT_CLIENTE")))
                                    , string.Format("{0:N0}", (Int32)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("TP_CONTRATO") == "H" && a.Field<string>("NM_FAIXA_ATRASO") == "01 a 10" && a.Field<Boolean>("NR_STATUS") == true).Sum(x => x.Field<Int32>("QT_CLIENTE")))
                                    , string.Format("R$ {0:N2}", (decimal)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("TP_CONTRATO") == "H" && a.Field<string>("NM_FAIXA_ATRASO") == "01 a 10" && a.Field<Boolean>("NR_STATUS") == true).Sum(x => x.Field<decimal>("VL_SALDO")))
                                    , string.Format("R$ {0:N2}", (decimal)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("TP_CONTRATO") == "H" && a.Field<string>("NM_FAIXA_ATRASO") == "01 a 10" && a.Field<Boolean>("NR_STATUS") == true).Sum(x => x.Field<decimal>("VL_SALDO") / x.Field<Int32>("QT_CLIENTE")))
                                    , string.Format("{0:N0}", (Int32)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("TP_CONTRATO") == "H" && a.Field<string>("NM_FAIXA_ATRASO") == "01 a 10").Sum(x => x.Field<Int32>("QT_CLIENTE")))
                                    , string.Format("R$ {0:N2}", (decimal)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("TP_CONTRATO") == "H" && a.Field<string>("NM_FAIXA_ATRASO") == "01 a 10").Sum(x => x.Field<decimal>("VL_SALDO")))
                                    , string.Format("R$ {0:N2}", (decimal)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("TP_CONTRATO") == "H" && a.Field<string>("NM_FAIXA_ATRASO") == "01 a 10").Sum(x => x.Field<decimal>("VL_SALDO")) / (Int32)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("TP_CONTRATO") == "H" && a.Field<string>("NM_FAIXA_ATRASO") == "01 a 10").Sum(x => x.Field<Int32>("QT_CLIENTE")))
                                   );

                    dtHabitacional.Rows.Add(
                                      "FAIXA 11A20"
                                    , string.Format("{0:N0}", (Int32)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("TP_CONTRATO") == "H" && a.Field<string>("NM_FAIXA_ATRASO") == "11 a 20" && a.Field<Boolean>("NR_STATUS") == false).Sum(x => x.Field<Int32>("QT_CLIENTE")))
                                    , string.Format("R$ {0:N2}", (decimal)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("TP_CONTRATO") == "H" && a.Field<string>("NM_FAIXA_ATRASO") == "11 a 20" && a.Field<Boolean>("NR_STATUS") == false).Sum(x => x.Field<decimal>("VL_SALDO")))
                                    , string.Format("R$ {0:N2}", (decimal)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("TP_CONTRATO") == "H" && a.Field<string>("NM_FAIXA_ATRASO") == "11 a 20" && a.Field<Boolean>("NR_STATUS") == false).Sum(x => x.Field<decimal>("VL_SALDO") / x.Field<Int32>("QT_CLIENTE")))
                                    , string.Format("{0:N0}", (Int32)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("TP_CONTRATO") == "H" && a.Field<string>("NM_FAIXA_ATRASO") == "11 a 20" && a.Field<Boolean>("NR_STATUS") == true).Sum(x => x.Field<Int32>("QT_CLIENTE")))
                                    , string.Format("R$ {0:N2}", (decimal)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("TP_CONTRATO") == "H" && a.Field<string>("NM_FAIXA_ATRASO") == "11 a 20" && a.Field<Boolean>("NR_STATUS") == true).Sum(x => x.Field<decimal>("VL_SALDO")))
                                    , string.Format("R$ {0:N2}", (decimal)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("TP_CONTRATO") == "H" && a.Field<string>("NM_FAIXA_ATRASO") == "11 a 20" && a.Field<Boolean>("NR_STATUS") == true).Sum(x => x.Field<decimal>("VL_SALDO") / x.Field<Int32>("QT_CLIENTE")))
                                    , string.Format("{0:N0}", (Int32)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("TP_CONTRATO") == "H" && a.Field<string>("NM_FAIXA_ATRASO") == "11 a 20").Sum(x => x.Field<Int32>("QT_CLIENTE")))
                                    , string.Format("R$ {0:N2}", (decimal)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("TP_CONTRATO") == "H" && a.Field<string>("NM_FAIXA_ATRASO") == "11 a 20").Sum(x => x.Field<decimal>("VL_SALDO")))
                                    , string.Format("R$ {0:N2}", (decimal)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("TP_CONTRATO") == "H" && a.Field<string>("NM_FAIXA_ATRASO") == "11 a 20").Sum(x => x.Field<decimal>("VL_SALDO")) / (Int32)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("TP_CONTRATO") == "H" && a.Field<string>("NM_FAIXA_ATRASO") == "11 a 20").Sum(x => x.Field<Int32>("QT_CLIENTE")))
                                    );

                    dtHabitacional.Rows.Add(
                                      "FAIXA 21A30"
                                    , string.Format("{0:N0}", (Int32)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("TP_CONTRATO") == "H" && a.Field<string>("NM_FAIXA_ATRASO") == "21 a 30" && a.Field<Boolean>("NR_STATUS") == false).Sum(x => x.Field<Int32>("QT_CLIENTE")))
                                    , string.Format("R$ {0:N2}", (decimal)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("TP_CONTRATO") == "H" && a.Field<string>("NM_FAIXA_ATRASO") == "21 a 30" && a.Field<Boolean>("NR_STATUS") == false).Sum(x => x.Field<decimal>("VL_SALDO")))
                                    , string.Format("R$ {0:N2}", (decimal)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("TP_CONTRATO") == "H" && a.Field<string>("NM_FAIXA_ATRASO") == "21 a 30" && a.Field<Boolean>("NR_STATUS") == false).Sum(x => x.Field<decimal>("VL_SALDO") / x.Field<Int32>("QT_CLIENTE")))
                                    , string.Format("{0:N0}", (Int32)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("TP_CONTRATO") == "H" && a.Field<string>("NM_FAIXA_ATRASO") == "21 a 30" && a.Field<Boolean>("NR_STATUS") == true).Sum(x => x.Field<Int32>("QT_CLIENTE")))
                                    , string.Format("R$ {0:N2}", (decimal)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("TP_CONTRATO") == "H" && a.Field<string>("NM_FAIXA_ATRASO") == "21 a 30" && a.Field<Boolean>("NR_STATUS") == true).Sum(x => x.Field<decimal>("VL_SALDO")))
                                    , string.Format("R$ {0:N2}", (decimal)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("TP_CONTRATO") == "H" && a.Field<string>("NM_FAIXA_ATRASO") == "21 a 30" && a.Field<Boolean>("NR_STATUS") == true).Sum(x => x.Field<decimal>("VL_SALDO") / x.Field<Int32>("QT_CLIENTE")))
                                    , string.Format("{0:N0}", (Int32)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("TP_CONTRATO") == "H" && a.Field<string>("NM_FAIXA_ATRASO") == "21 a 30").Sum(x => x.Field<Int32>("QT_CLIENTE")))
                                    , string.Format("R$ {0:N2}", (decimal)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("TP_CONTRATO") == "H" && a.Field<string>("NM_FAIXA_ATRASO") == "21 a 30").Sum(x => x.Field<decimal>("VL_SALDO")))
                                    , string.Format("R$ {0:N2}", (decimal)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("TP_CONTRATO") == "H" && a.Field<string>("NM_FAIXA_ATRASO") == "21 a 30").Sum(x => x.Field<decimal>("VL_SALDO")) / (Int32)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("TP_CONTRATO") == "H" && a.Field<string>("NM_FAIXA_ATRASO") == "21 a 30").Sum(x => x.Field<Int32>("QT_CLIENTE")))
                                    );

                    dtHabitacional.Rows.Add(
                                     "FAIXA 31A60"
                                    , string.Format("{0:N0}", (Int32)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("TP_CONTRATO") == "H" && a.Field<string>("NM_FAIXA_ATRASO") == "31 a 60" && a.Field<Boolean>("NR_STATUS") == false).Sum(x => x.Field<Int32>("QT_CLIENTE")))
                                    , string.Format("R$ {0:N2}", (decimal)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("TP_CONTRATO") == "H" && a.Field<string>("NM_FAIXA_ATRASO") == "31 a 60" && a.Field<Boolean>("NR_STATUS") == false).Sum(x => x.Field<decimal>("VL_SALDO")))
                                    , string.Format("R$ {0:N2}", (decimal)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("TP_CONTRATO") == "H" && a.Field<string>("NM_FAIXA_ATRASO") == "31 a 60" && a.Field<Boolean>("NR_STATUS") == false).Sum(x => x.Field<decimal>("VL_SALDO") / x.Field<Int32>("QT_CLIENTE")))
                                    , string.Format("{0:N0}", (Int32)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("TP_CONTRATO") == "H" && a.Field<string>("NM_FAIXA_ATRASO") == "31 a 60" && a.Field<Boolean>("NR_STATUS") == true).Sum(x => x.Field<Int32>("QT_CLIENTE")))
                                    , string.Format("R$ {0:N2}", (decimal)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("TP_CONTRATO") == "H" && a.Field<string>("NM_FAIXA_ATRASO") == "31 a 60" && a.Field<Boolean>("NR_STATUS") == true).Sum(x => x.Field<decimal>("VL_SALDO")))
                                    , string.Format("R$ {0:N2}", (decimal)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("TP_CONTRATO") == "H" && a.Field<string>("NM_FAIXA_ATRASO") == "31 a 60" && a.Field<Boolean>("NR_STATUS") == true).Sum(x => x.Field<decimal>("VL_SALDO") / x.Field<Int32>("QT_CLIENTE")))
                                    , string.Format("{0:N0}", (Int32)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("TP_CONTRATO") == "H" && a.Field<string>("NM_FAIXA_ATRASO") == "31 a 60").Sum(x => x.Field<Int32>("QT_CLIENTE")))
                                    , string.Format("R$ {0:N2}", (decimal)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("TP_CONTRATO") == "H" && a.Field<string>("NM_FAIXA_ATRASO") == "31 a 60").Sum(x => x.Field<decimal>("VL_SALDO")))
                                    , string.Format("R$ {0:N2}", (decimal)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("TP_CONTRATO") == "H" && a.Field<string>("NM_FAIXA_ATRASO") == "31 a 60").Sum(x => x.Field<decimal>("VL_SALDO")) / (Int32)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("TP_CONTRATO") == "H" && a.Field<string>("NM_FAIXA_ATRASO") == "31 a 60").Sum(x => x.Field<Int32>("QT_CLIENTE")))
                                    );

                    dtHabitacional.Rows.Add(
                                     "FAIXA MAIOR 60"
                                    , string.Format("{0:N0}", (Int32)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("TP_CONTRATO") == "H" && a.Field<string>("NM_FAIXA_ATRASO") == "Maior_60" && a.Field<Boolean>("NR_STATUS") == false).Sum(x => x.Field<Int32>("QT_CLIENTE")))
                                    , string.Format("R$ {0:N2}", (decimal)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("TP_CONTRATO") == "H" && a.Field<string>("NM_FAIXA_ATRASO") == "Maior_60" && a.Field<Boolean>("NR_STATUS") == false).Sum(x => x.Field<decimal>("VL_SALDO")))
                                    , string.Format("R$ {0:N2}", (decimal)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("TP_CONTRATO") == "H" && a.Field<string>("NM_FAIXA_ATRASO") == "Maior_60" && a.Field<Boolean>("NR_STATUS") == false).Sum(x => x.Field<decimal>("VL_SALDO") / x.Field<Int32>("QT_CLIENTE")))
                                    , string.Format("{0:N0}", (Int32)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("TP_CONTRATO") == "H" && a.Field<string>("NM_FAIXA_ATRASO") == "Maior_60" && a.Field<Boolean>("NR_STATUS") == true).Sum(x => x.Field<Int32>("QT_CLIENTE")))
                                    , string.Format("R$ {0:N2}", (decimal)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("TP_CONTRATO") == "H" && a.Field<string>("NM_FAIXA_ATRASO") == "Maior_60" && a.Field<Boolean>("NR_STATUS") == true).Sum(x => x.Field<decimal>("VL_SALDO")))
                                    , string.Format("R$ {0:N2}", (decimal)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("TP_CONTRATO") == "H" && a.Field<string>("NM_FAIXA_ATRASO") == "Maior_60" && a.Field<Boolean>("NR_STATUS") == true).Sum(x => x.Field<decimal>("VL_SALDO") / x.Field<Int32>("QT_CLIENTE")))
                                    , string.Format("{0:N0}", (Int32)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("TP_CONTRATO") == "H" && a.Field<string>("NM_FAIXA_ATRASO") == "Maior_60").Sum(x => x.Field<Int32>("QT_CLIENTE")))
                                    , string.Format("R$ {0:N2}", (decimal)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("TP_CONTRATO") == "H" && a.Field<string>("NM_FAIXA_ATRASO") == "Maior_60").Sum(x => x.Field<decimal>("VL_SALDO")))
                                    , string.Format("R$ {0:N2}", (decimal)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("TP_CONTRATO") == "H" && a.Field<string>("NM_FAIXA_ATRASO") == "Maior_60").Sum(x => x.Field<decimal>("VL_SALDO")) / (Int32)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("TP_CONTRATO") == "H" && a.Field<string>("NM_FAIXA_ATRASO") == "Maior_60").Sum(x => x.Field<Int32>("QT_CLIENTE")))
                                    );


                    dtHabitacional.Rows.Add(
                                     "SUBTOTAL"
                                    , string.Format("{0:N0}", (Int32)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("TP_CONTRATO") == "H" && a.Field<Boolean>("NR_STATUS") == false).Sum(x => x.Field<Int32>("QT_CLIENTE")))
                                    , string.Format("R$ {0:N2}", (decimal)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("TP_CONTRATO") == "H" && a.Field<Boolean>("NR_STATUS") == false).Sum(x => x.Field<decimal>("VL_SALDO")))
                                    , string.Format("R$ {0:N2}", (decimal)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("TP_CONTRATO") == "H" && a.Field<Boolean>("NR_STATUS") == false).Sum(x => x.Field<decimal>("VL_SALDO")) / (Int32)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("TP_CONTRATO") == "H" && a.Field<Boolean>("NR_STATUS") == false).Sum(x => x.Field<Int32>("QT_CLIENTE")))
                                    , string.Format("{0:N0}", (Int32)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("TP_CONTRATO") == "H" && a.Field<Boolean>("NR_STATUS") == true).Sum(x => x.Field<Int32>("QT_CLIENTE")))
                                    , string.Format("R$ {0:N2}", (decimal)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("TP_CONTRATO") == "H" && a.Field<Boolean>("NR_STATUS") == true).Sum(x => x.Field<decimal>("VL_SALDO")))
                                    , string.Format("R$ {0:N2}", (decimal)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("TP_CONTRATO") == "H" && a.Field<Boolean>("NR_STATUS") == true).Sum(x => x.Field<decimal>("VL_SALDO")) / (Int32)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("TP_CONTRATO") == "H" && a.Field<Boolean>("NR_STATUS") == true).Sum(x => x.Field<Int32>("QT_CLIENTE")))
                                    , string.Format("{0:N0}", (Int32)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("TP_CONTRATO") == "H").Sum(x => x.Field<Int32>("QT_CLIENTE")))
                                    , string.Format("R$ {0:N2}", (decimal)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("TP_CONTRATO") == "H").Sum(x => x.Field<decimal>("VL_SALDO")))
                                    , string.Format("R$ {0:N2}", (decimal)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("TP_CONTRATO") == "H").Sum(x => x.Field<decimal>("VL_SALDO")) / (Int32)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("TP_CONTRATO") == "H").Sum(x => x.Field<Int32>("QT_CLIENTE")))
                         );


                    dtTotal.Rows.Add(
                                       "FAIXA 01A10"
                                     , string.Format("{0:N0}", (Int32)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("NM_FAIXA_ATRASO") == "01 a 10" && a.Field<Boolean>("NR_STATUS") == false).Sum(x => x.Field<Int32>("QT_CLIENTE")))
                                     , string.Format("R$ {0:N2}", (decimal)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("NM_FAIXA_ATRASO") == "01 a 10" && a.Field<Boolean>("NR_STATUS") == false).Sum(x => x.Field<decimal>("VL_SALDO")))
                                     , string.Format("R$ {0:N2}", (decimal)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("NM_FAIXA_ATRASO") == "01 a 10" && a.Field<Boolean>("NR_STATUS") == false).Sum(x => x.Field<decimal>("VL_SALDO") / x.Field<Int32>("QT_CLIENTE")))
                                     , string.Format("{0:N0}", (Int32)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("NM_FAIXA_ATRASO") == "01 a 10" && a.Field<Boolean>("NR_STATUS") == true).Sum(x => x.Field<Int32>("QT_CLIENTE")))
                                     , string.Format("R$ {0:N2}", (decimal)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("NM_FAIXA_ATRASO") == "01 a 10" && a.Field<Boolean>("NR_STATUS") == true).Sum(x => x.Field<decimal>("VL_SALDO")))
                                     , string.Format("R$ {0:N2}", (decimal)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("NM_FAIXA_ATRASO") == "01 a 10" && a.Field<Boolean>("NR_STATUS") == true).Sum(x => x.Field<decimal>("VL_SALDO") / x.Field<Int32>("QT_CLIENTE")))
                                     , string.Format("{0:N0}", (Int32)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("NM_FAIXA_ATRASO") == "01 a 10").Sum(x => x.Field<Int32>("QT_CLIENTE")))
                                     , string.Format("R$ {0:N2}", (decimal)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("NM_FAIXA_ATRASO") == "01 a 10").Sum(x => x.Field<decimal>("VL_SALDO")))
                                     , string.Format("R$ {0:N2}", (decimal)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("NM_FAIXA_ATRASO") == "01 a 10").Sum(x => x.Field<decimal>("VL_SALDO")) / (Int32)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("NM_FAIXA_ATRASO") == "01 a 10").Sum(x => x.Field<Int32>("QT_CLIENTE")))
                                    );

                    dtTotal.Rows.Add(
                                      "FAIXA 11A20"
                                    , string.Format("{0:N0}", (Int32)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("NM_FAIXA_ATRASO") == "11 a 20" && a.Field<Boolean>("NR_STATUS") == false).Sum(x => x.Field<Int32>("QT_CLIENTE")))
                                    , string.Format("R$ {0:N2}", (decimal)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("NM_FAIXA_ATRASO") == "11 a 20" && a.Field<Boolean>("NR_STATUS") == false).Sum(x => x.Field<decimal>("VL_SALDO")))
                                    , string.Format("R$ {0:N2}", (decimal)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("NM_FAIXA_ATRASO") == "11 a 20" && a.Field<Boolean>("NR_STATUS") == false).Sum(x => x.Field<decimal>("VL_SALDO") / x.Field<Int32>("QT_CLIENTE")))
                                    , string.Format("{0:N0}", (Int32)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("NM_FAIXA_ATRASO") == "11 a 20" && a.Field<Boolean>("NR_STATUS") == true).Sum(x => x.Field<Int32>("QT_CLIENTE")))
                                    , string.Format("R$ {0:N2}", (decimal)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("NM_FAIXA_ATRASO") == "11 a 20" && a.Field<Boolean>("NR_STATUS") == true).Sum(x => x.Field<decimal>("VL_SALDO")))
                                    , string.Format("R$ {0:N2}", (decimal)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("NM_FAIXA_ATRASO") == "11 a 20" && a.Field<Boolean>("NR_STATUS") == true).Sum(x => x.Field<decimal>("VL_SALDO") / x.Field<Int32>("QT_CLIENTE")))
                                    , string.Format("{0:N0}", (Int32)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("NM_FAIXA_ATRASO") == "11 a 20").Sum(x => x.Field<Int32>("QT_CLIENTE")))
                                    , string.Format("R$ {0:N2}", (decimal)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("NM_FAIXA_ATRASO") == "11 a 20").Sum(x => x.Field<decimal>("VL_SALDO")))
                                    , string.Format("R$ {0:N2}", (decimal)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("NM_FAIXA_ATRASO") == "11 a 20").Sum(x => x.Field<decimal>("VL_SALDO")) / (Int32)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("NM_FAIXA_ATRASO") == "11 a 20").Sum(x => x.Field<Int32>("QT_CLIENTE")))
                                    );

                    dtTotal.Rows.Add(
                                      "FAIXA 21A30"
                                    , string.Format("{0:N0}", (Int32)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("NM_FAIXA_ATRASO") == "21 a 30" && a.Field<Boolean>("NR_STATUS") == false).Sum(x => x.Field<Int32>("QT_CLIENTE")))
                                    , string.Format("R$ {0:N2}", (decimal)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("NM_FAIXA_ATRASO") == "21 a 30" && a.Field<Boolean>("NR_STATUS") == false).Sum(x => x.Field<decimal>("VL_SALDO")))
                                    , string.Format("R$ {0:N2}", (decimal)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("NM_FAIXA_ATRASO") == "21 a 30" && a.Field<Boolean>("NR_STATUS") == false).Sum(x => x.Field<decimal>("VL_SALDO") / x.Field<Int32>("QT_CLIENTE")))
                                    , string.Format("{0:N0}", (Int32)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("NM_FAIXA_ATRASO") == "21 a 30" && a.Field<Boolean>("NR_STATUS") == true).Sum(x => x.Field<Int32>("QT_CLIENTE")))
                                    , string.Format("R$ {0:N2}", (decimal)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("NM_FAIXA_ATRASO") == "21 a 30" && a.Field<Boolean>("NR_STATUS") == true).Sum(x => x.Field<decimal>("VL_SALDO")))
                                    , string.Format("R$ {0:N2}", (decimal)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("NM_FAIXA_ATRASO") == "21 a 30" && a.Field<Boolean>("NR_STATUS") == true).Sum(x => x.Field<decimal>("VL_SALDO") / x.Field<Int32>("QT_CLIENTE")))
                                    , string.Format("{0:N0}", (Int32)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("NM_FAIXA_ATRASO") == "21 a 30").Sum(x => x.Field<Int32>("QT_CLIENTE")))
                                    , string.Format("R$ {0:N2}", (decimal)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("NM_FAIXA_ATRASO") == "21 a 30").Sum(x => x.Field<decimal>("VL_SALDO")))
                                    , string.Format("R$ {0:N2}", (decimal)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("NM_FAIXA_ATRASO") == "21 a 30").Sum(x => x.Field<decimal>("VL_SALDO")) / (Int32)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("NM_FAIXA_ATRASO") == "21 a 30").Sum(x => x.Field<Int32>("QT_CLIENTE")))
                                    );

                    dtTotal.Rows.Add(
                                     "FAIXA 31A60"
                                    , string.Format("{0:N0}", (Int32)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("NM_FAIXA_ATRASO") == "31 a 60" && a.Field<Boolean>("NR_STATUS") == false).Sum(x => x.Field<Int32>("QT_CLIENTE")))
                                    , string.Format("R$ {0:N2}", (decimal)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("NM_FAIXA_ATRASO") == "31 a 60" && a.Field<Boolean>("NR_STATUS") == false).Sum(x => x.Field<decimal>("VL_SALDO")))
                                    , string.Format("R$ {0:N2}", (decimal)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("NM_FAIXA_ATRASO") == "31 a 60" && a.Field<Boolean>("NR_STATUS") == false).Sum(x => x.Field<decimal>("VL_SALDO") / x.Field<Int32>("QT_CLIENTE")))
                                    , string.Format("{0:N0}", (Int32)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("NM_FAIXA_ATRASO") == "31 a 60" && a.Field<Boolean>("NR_STATUS") == true).Sum(x => x.Field<Int32>("QT_CLIENTE")))
                                    , string.Format("R$ {0:N2}", (decimal)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("NM_FAIXA_ATRASO") == "31 a 60" && a.Field<Boolean>("NR_STATUS") == true).Sum(x => x.Field<decimal>("VL_SALDO")))
                                    , string.Format("R$ {0:N2}", (decimal)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("NM_FAIXA_ATRASO") == "31 a 60" && a.Field<Boolean>("NR_STATUS") == true).Sum(x => x.Field<decimal>("VL_SALDO") / x.Field<Int32>("QT_CLIENTE")))
                                    , string.Format("{0:N0}", (Int32)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("NM_FAIXA_ATRASO") == "31 a 60").Sum(x => x.Field<Int32>("QT_CLIENTE")))
                                    , string.Format("R$ {0:N2}", (decimal)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("NM_FAIXA_ATRASO") == "31 a 60").Sum(x => x.Field<decimal>("VL_SALDO")))
                                    , string.Format("R$ {0:N2}", (decimal)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("NM_FAIXA_ATRASO") == "31 a 60").Sum(x => x.Field<decimal>("VL_SALDO")) / (Int32)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("NM_FAIXA_ATRASO") == "31 a 60").Sum(x => x.Field<Int32>("QT_CLIENTE")))
                   );

                    dtTotal.Rows.Add(
                                     "FAIXA MAIOR 60"
                                    , string.Format("{0:N0}", (Int32)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("NM_FAIXA_ATRASO") == "Maior_60" && a.Field<Boolean>("NR_STATUS") == false).Sum(x => x.Field<Int32>("QT_CLIENTE")))
                                    , string.Format("R$ {0:N2}", (decimal)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("NM_FAIXA_ATRASO") == "Maior_60" && a.Field<Boolean>("NR_STATUS") == false).Sum(x => x.Field<decimal>("VL_SALDO")))
                                    , string.Format("R$ {0:N2}", (decimal)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("NM_FAIXA_ATRASO") == "Maior_60" && a.Field<Boolean>("NR_STATUS") == false).Sum(x => x.Field<decimal>("VL_SALDO") / x.Field<Int32>("QT_CLIENTE")))
                                    , string.Format("{0:N0}", (Int32)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("NM_FAIXA_ATRASO") == "Maior_60" && a.Field<Boolean>("NR_STATUS") == true).Sum(x => x.Field<Int32>("QT_CLIENTE")))
                                    , string.Format("R$ {0:N2}", (decimal)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("NM_FAIXA_ATRASO") == "Maior_60" && a.Field<Boolean>("NR_STATUS") == true).Sum(x => x.Field<decimal>("VL_SALDO")))
                                    , string.Format("R$ {0:N2}", (decimal)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("NM_FAIXA_ATRASO") == "Maior_60" && a.Field<Boolean>("NR_STATUS") == true).Sum(x => x.Field<decimal>("VL_SALDO") / x.Field<Int32>("QT_CLIENTE")))
                                    , string.Format("{0:N0}", (Int32)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("NM_FAIXA_ATRASO") == "Maior_60").Sum(x => x.Field<Int32>("QT_CLIENTE")))
                                    , string.Format("R$ {0:N2}", (decimal)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("NM_FAIXA_ATRASO") == "Maior_60").Sum(x => x.Field<decimal>("VL_SALDO")))
                                    , string.Format("R$ {0:N2}", (decimal)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("NM_FAIXA_ATRASO") == "Maior_60").Sum(x => x.Field<decimal>("VL_SALDO") / x.Field<Int32>("QT_CLIENTE")))
                                    );


                    dtTotal.Rows.Add(
                                     "SUBTOTAL"
                                    , string.Format("{0:N0}", (Int32)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<Boolean>("NR_STATUS") == false).Sum(x => x.Field<Int32>("QT_CLIENTE")))
                                    , string.Format("R$ {0:N2}", (decimal)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<Boolean>("NR_STATUS") == false).Sum(x => x.Field<decimal>("VL_SALDO")))
                                    , string.Format("R$ {0:N2}", (decimal)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<Boolean>("NR_STATUS") == false).Sum(x => x.Field<decimal>("VL_SALDO") / x.Field<Int32>("QT_CLIENTE")))
                                    , string.Format("{0:N0}", (Int32)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<Boolean>("NR_STATUS") == true).Sum(x => x.Field<Int32>("QT_CLIENTE")))
                                    , string.Format("R$ {0:N2}", (decimal)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<Boolean>("NR_STATUS") == true).Sum(x => x.Field<decimal>("VL_SALDO")))
                                    , string.Format("R$ {0:N2}", (decimal)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<Boolean>("NR_STATUS") == true).Sum(x => x.Field<decimal>("VL_SALDO") / x.Field<Int32>("QT_CLIENTE")))
                                    , string.Format("{0:N0}", (Int32)dsFechamento.Tables[0].AsEnumerable().Sum(x => x.Field<Int32>("QT_CLIENTE")))
                                    , string.Format("R$ {0:N2}", (decimal)dsFechamento.Tables[0].AsEnumerable().Sum(x => x.Field<decimal>("VL_SALDO")))
                                    , string.Format("R$ {0:N2}", (decimal)dsFechamento.Tables[0].AsEnumerable().Sum(x => x.Field<decimal>("VL_SALDO")) / (Int32)dsFechamento.Tables[0].AsEnumerable().Sum(x => x.Field<Int32>("QT_CLIENTE")))
                                     );

                    dsFinal.Tables.Add(dtComercial);
                    dsFinal.Tables.Add(dtHabitacional);
                    dsFinal.Tables.Add(dtTotal);

                }
                return dsFinal;
            }
            catch (Exception ex)
            {
                throw new Exception("FAT.CmdReversao_001: " + ex.Message, ex);
            }
        }

        public DataSet GeraRelatorioProduto(int DT_REFERENCIA)
        {
            try
            {
                SqlCommand sqlcommand = new SqlCommand();
                sqlcommand.CommandType = CommandType.StoredProcedure;
                sqlcommand.CommandText = "SP_WEB_REM_CARGA_RESUMO";
                sqlcommand.Parameters.AddWithValue("@DT_ARQUIVO", DT_REFERENCIA);

                DAL_MIS AcessaDadosCaixa = new Intranet.DAL.DAL_MIS();

                DataSet dsFechamento = AcessaDadosCaixa.ConsultaSQL(sqlcommand);

                DataSet dsFinal = new DataSet("ReportProduto");
                if ((dsFechamento.Tables.Count > 0) && (dsFechamento.Tables[1].Rows.Count > 0))
                {
                    DataTable dtProduto = new DataTable("Produto");
                    dtProduto.Columns.Add("COL01");
                    dtProduto.Columns.Add("COL02");
                    dtProduto.Columns.Add("COL03");
                    dtProduto.Columns.Add("COL04");
                    dtProduto.Columns.Add("COL05");
                    dtProduto.Columns.Add("COL06");
                    dtProduto.Columns.Add("COL07");
                    dtProduto.Columns.Add("COL08");
                    dtProduto.Columns.Add("COL09");
                    dtProduto.Columns.Add("COL10");


                    var listaprodutos = dsFechamento.Tables[1].AsEnumerable()
                           .GroupBy(g => new { NM_PRODUTO = g.Field<string>("NM_PRODUTO") })
                           .Select(sel => new { NM_PRODUTO = sel.Key.NM_PRODUTO, QT_ACIONAMENTO = sel.Sum(sum => sum.Field<Int32>("QT_CLIENTE")) }).ToList();


                    for (int i = 0; i < listaprodutos.Count; i++)
                    {

                        dtProduto.Rows.Add(
                                    listaprodutos[i].NM_PRODUTO.ToString()
                                  , string.Format("{0:N0}", (Int32)dsFechamento.Tables[1].AsEnumerable().Where(a => a.Field<string>("NM_PRODUTO") == listaprodutos[i].NM_PRODUTO.ToString() && a.Field<Boolean>("NR_STATUS") == false).Sum(x => x.Field<Int32>("QT_CLIENTE")))
                                  , string.Format("R$ {0:N2}", (decimal)dsFechamento.Tables[1].AsEnumerable().Where(a => a.Field<string>("NM_PRODUTO") == listaprodutos[i].NM_PRODUTO.ToString() && a.Field<Boolean>("NR_STATUS") == false).Sum(x => x.Field<decimal>("VL_SALDO")))
                                  , string.Format("R$ {0:N2}", (decimal)dsFechamento.Tables[1].AsEnumerable().Where(a => a.Field<string>("NM_PRODUTO") == listaprodutos[i].NM_PRODUTO.ToString() && a.Field<Boolean>("NR_STATUS") == false).Sum(x => x.Field<decimal>("TICKET")))
                                  , string.Format("{0:N0}", (Int32)dsFechamento.Tables[1].AsEnumerable().Where(a => a.Field<string>("NM_PRODUTO") == listaprodutos[i].NM_PRODUTO.ToString() && a.Field<Boolean>("NR_STATUS") == true).Sum(x => x.Field<Int32>("QT_CLIENTE")))
                                  , string.Format("R$ {0:N2}", (decimal)dsFechamento.Tables[1].AsEnumerable().Where(a => a.Field<string>("NM_PRODUTO") == listaprodutos[i].NM_PRODUTO.ToString() && a.Field<Boolean>("NR_STATUS") == true).Sum(x => x.Field<decimal>("VL_SALDO")))
                                  , string.Format("R$ {0:N2}", (decimal)dsFechamento.Tables[1].AsEnumerable().Where(a => a.Field<string>("NM_PRODUTO") == listaprodutos[i].NM_PRODUTO.ToString() && a.Field<Boolean>("NR_STATUS") == true).Sum(x => x.Field<decimal>("TICKET")))
                                  , string.Format("{0:N0}", (Int32)dsFechamento.Tables[1].AsEnumerable().Where(a => a.Field<string>("NM_PRODUTO") == listaprodutos[i].NM_PRODUTO.ToString()).Sum(x => x.Field<Int32>("QT_CLIENTE")))
                                  , string.Format("R$ {0:N2}", (decimal)dsFechamento.Tables[1].AsEnumerable().Where(a => a.Field<string>("NM_PRODUTO") == listaprodutos[i].NM_PRODUTO.ToString()).Sum(x => x.Field<decimal>("VL_SALDO")))
                                  , string.Format("R$ {0:N2}", (decimal)dsFechamento.Tables[1].AsEnumerable().Where(a => a.Field<string>("NM_PRODUTO") == listaprodutos[i].NM_PRODUTO.ToString()).Sum(x => x.Field<decimal>("TICKET")))
                                 );

                    }

                    dtProduto.Rows.Add(
                                "TOTAL"
                              , string.Format("{0:N0}", (Int32)dsFechamento.Tables[1].AsEnumerable().Where(a => a.Field<Boolean>("NR_STATUS") == false).Sum(x => x.Field<Int32>("QT_CLIENTE")))
                              , string.Format("R$ {0:N2}", (decimal)dsFechamento.Tables[1].AsEnumerable().Where(a => a.Field<Boolean>("NR_STATUS") == false).Sum(x => x.Field<decimal>("VL_SALDO")))
                              , string.Format("R$ {0:N2}", (decimal)dsFechamento.Tables[1].AsEnumerable().Where(a => a.Field<Boolean>("NR_STATUS") == false).Sum(x => x.Field<decimal>("TICKET")))
                              , string.Format("{0:N0}", (Int32)dsFechamento.Tables[1].AsEnumerable().Where(a => a.Field<Boolean>("NR_STATUS") == true).Sum(x => x.Field<Int32>("QT_CLIENTE")))
                              , string.Format("R$ {0:N2}", (decimal)dsFechamento.Tables[1].AsEnumerable().Where(a => a.Field<Boolean>("NR_STATUS") == true).Sum(x => x.Field<decimal>("VL_SALDO")))
                              , string.Format("R$ {0:N2}", (decimal)dsFechamento.Tables[1].AsEnumerable().Where(a => a.Field<Boolean>("NR_STATUS") == true).Sum(x => x.Field<decimal>("TICKET")))
                              , string.Format("{0:N0}", (Int32)dsFechamento.Tables[1].AsEnumerable().Sum(x => x.Field<Int32>("QT_CLIENTE")))
                              , string.Format("R$ {0:N2}", (decimal)dsFechamento.Tables[1].AsEnumerable().Sum(x => x.Field<decimal>("VL_SALDO")))
                              , string.Format("R$ {0:N2}", (decimal)dsFechamento.Tables[1].AsEnumerable().Sum(x => x.Field<decimal>("TICKET")))
                             );


                    dsFinal.Tables.Add(dtProduto);
                }
                return dsFinal;
            }
            catch (Exception ex)
            {
                throw new Exception("FAT.CmdReversao_001: " + ex.Message, ex);
            }
        }

        public DataSet GeraRelatorioUF(int DT_REFERENCIA)
        {
            try
            {
                SqlCommand sqlcommand = new SqlCommand();
                sqlcommand.CommandType = CommandType.StoredProcedure;
                sqlcommand.CommandText = "SP_WEB_REM_CARGA_RESUMO";
                sqlcommand.Parameters.AddWithValue("@DT_ARQUIVO", DT_REFERENCIA);

                DAL_MIS AcessaDadosCaixa = new Intranet.DAL.DAL_MIS();

                DataSet dsFechamento = AcessaDadosCaixa.ConsultaSQL(sqlcommand);

                DataSet dsFinal = new DataSet("ReportUF");
                if ((dsFechamento.Tables.Count > 0) && (dsFechamento.Tables[2].Rows.Count > 0))
                {
                    DataTable dtUF = new DataTable("UF");
                    dtUF.Columns.Add("COL01");
                    dtUF.Columns.Add("COL02");
                    dtUF.Columns.Add("COL03");
                    dtUF.Columns.Add("COL04");
                    dtUF.Columns.Add("COL05");
                    dtUF.Columns.Add("COL06");
                    dtUF.Columns.Add("COL07");
                    dtUF.Columns.Add("COL08");
                    dtUF.Columns.Add("COL09");
                    dtUF.Columns.Add("COL10");


                    var listaUF = dsFechamento.Tables[2].AsEnumerable()
                           .GroupBy(g => new { TP_UF = g.Field<string>("TP_UF") })
                           .Select(sel => new { TP_UF = sel.Key.TP_UF, QT_ACIONAMENTO = sel.Sum(sum => sum.Field<Int32>("QT_CLIENTE")) }).ToList();


                    for (int i = 0; i < listaUF.Count; i++)
                    {

                        dtUF.Rows.Add(
                                    listaUF[i].TP_UF.ToString()
                                  , string.Format("{0:N0}", (Int32)dsFechamento.Tables[2].AsEnumerable().Where(a => a.Field<string>("TP_UF") == listaUF[i].TP_UF.ToString() && a.Field<Boolean>("NR_STATUS") == false).Sum(x => x.Field<Int32>("QT_CLIENTE")))
                                  , string.Format("R$ {0:N2}", (decimal)dsFechamento.Tables[2].AsEnumerable().Where(a => a.Field<string>("TP_UF") == listaUF[i].TP_UF.ToString() && a.Field<Boolean>("NR_STATUS") == false).Sum(x => x.Field<decimal>("VL_SALDO")))
                                  , string.Format("R$ {0:N2}", (decimal)dsFechamento.Tables[2].AsEnumerable().Where(a => a.Field<string>("TP_UF") == listaUF[i].TP_UF.ToString() && a.Field<Boolean>("NR_STATUS") == false).Sum(x => x.Field<decimal>("VL_SALDO") / x.Field<Int32>("QT_CLIENTE")))
                                  , string.Format("{0:N0}", (Int32)dsFechamento.Tables[2].AsEnumerable().Where(a => a.Field<string>("TP_UF") == listaUF[i].TP_UF.ToString() && a.Field<Boolean>("NR_STATUS") == true).Sum(x => x.Field<Int32>("QT_CLIENTE")))
                                  , string.Format("R$ {0:N2}", (decimal)dsFechamento.Tables[2].AsEnumerable().Where(a => a.Field<string>("TP_UF") == listaUF[i].TP_UF.ToString() && a.Field<Boolean>("NR_STATUS") == true).Sum(x => x.Field<decimal>("VL_SALDO")))
                                  , string.Format("R$ {0:N2}", (decimal)dsFechamento.Tables[2].AsEnumerable().Where(a => a.Field<string>("TP_UF") == listaUF[i].TP_UF.ToString() && a.Field<Boolean>("NR_STATUS") == true).Sum(x => x.Field<decimal>("VL_SALDO") / x.Field<Int32>("QT_CLIENTE")))
                                  , string.Format("{0:N0}", (Int32)dsFechamento.Tables[2].AsEnumerable().Where(a => a.Field<string>("TP_UF") == listaUF[i].TP_UF.ToString()).Sum(x => x.Field<Int32>("QT_CLIENTE")))
                                  , string.Format("R$ {0:N2}", (decimal)dsFechamento.Tables[2].AsEnumerable().Where(a => a.Field<string>("TP_UF") == listaUF[i].TP_UF.ToString()).Sum(x => x.Field<decimal>("VL_SALDO")))
                                  , string.Format("R$ {0:N2}", (decimal)dsFechamento.Tables[2].AsEnumerable().Where(a => a.Field<string>("TP_UF") == listaUF[i].TP_UF.ToString()).Sum(x => x.Field<decimal>("VL_SALDO") / x.Field<Int32>("QT_CLIENTE")))
                                 );

                    }

                    dtUF.Rows.Add(
                                "TOTAL"
                              , string.Format("{0:N0}", (Int32)dsFechamento.Tables[2].AsEnumerable().Where(a => a.Field<Boolean>("NR_STATUS") == false).Sum(x => x.Field<Int32>("QT_CLIENTE")))
                              , string.Format("R$ {0:N2}", (decimal)dsFechamento.Tables[2].AsEnumerable().Where(a => a.Field<Boolean>("NR_STATUS") == false).Sum(x => x.Field<decimal>("VL_SALDO")))
                              , string.Format("R$ {0:N2}", (decimal)dsFechamento.Tables[2].AsEnumerable().Where(a => a.Field<Boolean>("NR_STATUS") == false).Sum(x => x.Field<decimal>("VL_SALDO") / x.Field<Int32>("QT_CLIENTE")))
                              , string.Format("{0:N0}", (Int32)dsFechamento.Tables[2].AsEnumerable().Where(a => a.Field<Boolean>("NR_STATUS") == true).Sum(x => x.Field<Int32>("QT_CLIENTE")))
                              , string.Format("R$ {0:N2}", (decimal)dsFechamento.Tables[2].AsEnumerable().Where(a => a.Field<Boolean>("NR_STATUS") == true).Sum(x => x.Field<decimal>("VL_SALDO")))
                              , string.Format("R$ {0:N2}", (decimal)dsFechamento.Tables[2].AsEnumerable().Where(a => a.Field<Boolean>("NR_STATUS") == true).Sum(x => x.Field<decimal>("VL_SALDO") / x.Field<Int32>("QT_CLIENTE")))
                              , string.Format("{0:N0}", (Int32)dsFechamento.Tables[2].AsEnumerable().Sum(x => x.Field<Int32>("QT_CLIENTE")))
                              , string.Format("R$ {0:N2}", (decimal)dsFechamento.Tables[2].AsEnumerable().Sum(x => x.Field<decimal>("VL_SALDO")))
                              , string.Format("R$ {0:N2}", (decimal)dsFechamento.Tables[2].AsEnumerable().Sum(x => x.Field<decimal>("VL_SALDO") / x.Field<Int32>("QT_CLIENTE")))
                             );


                    dsFinal.Tables.Add(dtUF);
                }
                return dsFinal;
            }
            catch (Exception ex)
            {
                throw new Exception("FAT.CmdReversao_001: " + ex.Message, ex);
            }
        }

        public DataSet GeraRelatorioFaixa_New(int DT_REFERENCIA, int TP_CARTEIRA)
        {
            try
            {
                SqlCommand sqlcommand = new SqlCommand();
                sqlcommand.CommandType = CommandType.StoredProcedure;
                sqlcommand.CommandText = "SP_REM_CARGA_HISTORICO_NEW";
                sqlcommand.Parameters.AddWithValue("@DT_ARQUIVO", DT_REFERENCIA);
                sqlcommand.Parameters.AddWithValue("@TP_CARTEIRA", TP_CARTEIRA);

                DAL_PROC AcessaDadosCaixa = new Intranet.DAL.DAL_PROC();

                DataSet dsFechamento = AcessaDadosCaixa.ConsultaSQL(sqlcommand);

                return dsFechamento;
            }
            catch (Exception ex)
            {
                throw new Exception("MAP.Mapeamento_004: " + ex.Message, ex);
            }
        }


        public DataSet GeraRelatorioCarteira(DateTime DT_REFERENCIA, int TP_ARQUIVO, string TP_PRODUTO, string TP_CAMPANHA, int TP_STATUS, int CD_ACAO)
        {
            try
            {
                SqlCommand sqlcommand = new SqlCommand();
                sqlcommand.CommandType = CommandType.StoredProcedure;
                sqlcommand.CommandText = "SP_REV_GERA_RESUMO_STATUS_NEW";
                sqlcommand.Parameters.AddWithValue("@DT_REFERENCIA", DT_REFERENCIA.ToString("yyyy-MM-dd HH:mm:ss"));
                sqlcommand.Parameters.AddWithValue("@TP_ARQUIVO", TP_ARQUIVO);
                sqlcommand.Parameters.AddWithValue("@TP_PRODUTO", TP_PRODUTO.ToString());
                sqlcommand.Parameters.AddWithValue("@TP_CAMPANHA", TP_CAMPANHA.ToString());
                sqlcommand.Parameters.AddWithValue("@CD_ACAO", CD_ACAO);

                DAL_MIS AcessaDadosCaixa = new Intranet.DAL.DAL_MIS();

                DataSet dsFechamento = AcessaDadosCaixa.ConsultaSQL(sqlcommand);

                DataSet dsFinal = new DataSet("ReportCarteira");
                if ((dsFechamento.Tables.Count > 0) && (dsFechamento.Tables[0].Rows.Count > 0))
                {
                    DataTable dtCarteira = new DataTable("Carteira");
                    dtCarteira.Columns.Add("COL01");
                    dtCarteira.Columns.Add("COL02");
                    dtCarteira.Columns.Add("COL03");
                    dtCarteira.Columns.Add("COL04");
                    dtCarteira.Columns.Add("COL05");
                    dtCarteira.Columns.Add("COL06");

                    if (TP_STATUS == 1)
                    {
                        decimal _qt_total_ativo = (decimal)dsFechamento.Tables[0].AsEnumerable().Sum(x => x.Field<Int32>("QTDE_CLIENTE_AT"));
                        decimal _vl_total_ativo = (decimal)dsFechamento.Tables[0].AsEnumerable().Sum(x => x.Field<decimal>("SALDO_DIVIDA_AT"));

                        /* VALORES DE CARTEIRA */
                        dtCarteira.Rows.Add(
                                        "Contatos Efetivos"
                                      , string.Format("{0:N0}", (Int32)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<Int32>("GRUPO") == 66 || a.Field<Int32>("GRUPO") == 55).Sum(x => x.Field<Int32>("QTDE_CLIENTE_AT")))
                                      , string.Format("{0:N2} %",  (Int32)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<Int32>("GRUPO") == 66 || a.Field<Int32>("GRUPO") == 55).Sum(x => x.Field<Int32>("QTDE_CLIENTE_AT"))/ _qt_total_ativo * 100)
                                      , string.Format("R$ {0:N2}", (decimal)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<Int32>("GRUPO") == 66 || a.Field<Int32>("GRUPO") == 55).Sum(x => x.Field<decimal>("SALDO_AT")))
                                      , string.Format("R$ {0:N2}", (decimal)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<Int32>("GRUPO") == 66 || a.Field<Int32>("GRUPO") == 55).Sum(x => x.Field<decimal>("SALDO_DIVIDA_AT")))
                                      , string.Format("{0:N2} %", (decimal)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<Int32>("GRUPO") == 66 || a.Field<Int32>("GRUPO") == 55).Sum(x => x.Field<decimal>("SALDO_DIVIDA_AT"))/_vl_total_ativo * 100)
                                        );
                        dtCarteira.Rows.Add(
                                        "*Contato com a Pessoa Certa (CPC)"
                                      , string.Format("{0:N0}", (Int32)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<Int32>("GRUPO") == 55).Sum(x => x.Field<Int32>("QTDE_CLIENTE_AT")))
                                      , string.Format("{0:N2} %", (Int32)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<Int32>("GRUPO") == 55).Sum(x => x.Field<Int32>("QTDE_CLIENTE_AT")) / _qt_total_ativo * 100)
                                      , string.Format("R$ {0:N2}", (decimal)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<Int32>("GRUPO") == 55).Sum(x => x.Field<decimal>("SALDO_AT")))
                                      , string.Format("R$ {0:N2}", (decimal)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<Int32>("GRUPO") == 55).Sum(x => x.Field<decimal>("SALDO_DIVIDA_AT")))
                                      , string.Format("{0:N2} %", (decimal)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<Int32>("GRUPO") == 55).Sum(x => x.Field<decimal>("SALDO_DIVIDA_AT")) / _vl_total_ativo * 100)
                                        );
                        dtCarteira.Rows.Add(
                                          "**Promessa de Pagamento"
                                        , string.Format("{0:N0}", (Int32)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("STATUS") == "01-PP").Sum(x => x.Field<Int32>("QTDE_CLIENTE_AT")))
                                        , string.Format("{0:N2} %", (Int32)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("STATUS") == "01-PP").Sum(x => x.Field<Int32>("QTDE_CLIENTE_AT")) / _qt_total_ativo * 100)
                                        , string.Format("R$ {0:N2}", (decimal)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("STATUS") == "01-PP").Sum(x => x.Field<decimal>("SALDO_AT")))
                                        , string.Format("R$ {0:N2}", (decimal)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("STATUS") == "01-PP").Sum(x => x.Field<decimal>("SALDO_DIVIDA_AT")))
                                        , string.Format("{0:N2} %", (decimal)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("STATUS") == "01-PP").Sum(x => x.Field<decimal>("SALDO_DIVIDA_AT")) / _vl_total_ativo * 100)
                                          );

                        dtCarteira.Rows.Add(
                                          "**Outros Resultado CPC"
                                        , string.Format("{0:N0}", (Int32)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("STATUS") == "02-CPC").Sum(x => x.Field<Int32>("QTDE_CLIENTE_AT")))
                                        , string.Format("{0:N2} %", (Int32)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("STATUS") == "02-CPC").Sum(x => x.Field<Int32>("QTDE_CLIENTE_AT")) / _qt_total_ativo * 100)
                                        , string.Format("R$ {0:N2}", (decimal)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("STATUS") == "02-CPC").Sum(x => x.Field<decimal>("SALDO_AT")))
                                        , string.Format("R$ {0:N2}", (decimal)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("STATUS") == "02-CPC").Sum(x => x.Field<decimal>("SALDO_DIVIDA_AT")))
                                        , string.Format("{0:N2} %", (decimal)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("STATUS") == "02-CPC").Sum(x => x.Field<decimal>("SALDO_DIVIDA_AT")) / _vl_total_ativo * 100)
                                          );

                        dtCarteira.Rows.Add(
                                          "*Recado Terceiro"
                                        , string.Format("{0:N0}", (Int32)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("STATUS") == "03-RECADO TERCEIRO").Sum(x => x.Field<Int32>("QTDE_CLIENTE_AT")))
                                        , string.Format("{0:N2} %", (Int32)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("STATUS") == "03-RECADO TERCEIRO").Sum(x => x.Field<Int32>("QTDE_CLIENTE_AT")) / _qt_total_ativo * 100)
                                        , string.Format("R$ {0:N2}", (decimal)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("STATUS") == "03-RECADO TERCEIRO").Sum(x => x.Field<decimal>("SALDO_AT")))
                                        , string.Format("R$ {0:N2}", (decimal)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("STATUS") == "03-RECADO TERCEIRO").Sum(x => x.Field<decimal>("SALDO_DIVIDA_AT")))
                                        , string.Format("{0:N2} %", (decimal)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("STATUS") == "03-RECADO TERCEIRO").Sum(x => x.Field<decimal>("SALDO_DIVIDA_AT")) / _vl_total_ativo * 100)
                                          );

                        dtCarteira.Rows.Add(
                                          "Problema Cadastral"
                                        , string.Format("{0:N0}", (Int32)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("STATUS") == "04-PROBLEMA CADASTRAL").Sum(x => x.Field<Int32>("QTDE_CLIENTE_AT")))
                                        , string.Format("{0:N2} %", (Int32)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("STATUS") == "04-PROBLEMA CADASTRAL").Sum(x => x.Field<Int32>("QTDE_CLIENTE_AT")) / _qt_total_ativo * 100)
                                        , string.Format("R$ {0:N2}", (decimal)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("STATUS") == "04-PROBLEMA CADASTRAL").Sum(x => x.Field<decimal>("SALDO_AT")))
                                        , string.Format("R$ {0:N2}", (decimal)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("STATUS") == "04-PROBLEMA CADASTRAL").Sum(x => x.Field<decimal>("SALDO_DIVIDA_AT")))
                                        , string.Format("{0:N2} %", (decimal)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("STATUS") == "04-PROBLEMA CADASTRAL").Sum(x => x.Field<decimal>("SALDO_DIVIDA_AT")) / _vl_total_ativo * 100)
                                          );
                        dtCarteira.Rows.Add(
                                          "Indicio Problema Cadastral"
                                        , string.Format("{0:N0}", (Int32)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("STATUS") == "05-INDICIO PROBLEMA CADASTRAL").Sum(x => x.Field<Int32>("QTDE_CLIENTE_AT")))
                                        , string.Format("{0:N2} %", (Int32)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("STATUS") == "05-INDICIO PROBLEMA CADASTRAL").Sum(x => x.Field<Int32>("QTDE_CLIENTE_AT")) / _qt_total_ativo * 100)
                                        , string.Format("R$ {0:N2}", (decimal)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("STATUS") == "05-INDICIO PROBLEMA CADASTRAL").Sum(x => x.Field<decimal>("SALDO_AT")))
                                        , string.Format("R$ {0:N2}", (decimal)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("STATUS") == "05-INDICIO PROBLEMA CADASTRAL").Sum(x => x.Field<decimal>("SALDO_DIVIDA_AT")))
                                        , string.Format("{0:N2} %", (decimal)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("STATUS") == "05-INDICIO PROBLEMA CADASTRAL").Sum(x => x.Field<decimal>("SALDO_DIVIDA_AT")) / _vl_total_ativo * 100)
                                          );
                        dtCarteira.Rows.Add(
                                          "Midia"
                                        , string.Format("{0:N0}", (Int32)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<Int32>("GRUPO") == 88).Sum(x => x.Field<Int32>("QTDE_CLIENTE_AT")))
                                        , string.Format("{0:N2} %", (Int32)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<Int32>("GRUPO") == 88).Sum(x => x.Field<Int32>("QTDE_CLIENTE_AT")) / _qt_total_ativo * 100)
                                        , string.Format("R$ {0:N2}", (decimal)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<Int32>("GRUPO") == 88).Sum(x => x.Field<decimal>("SALDO_AT")))
                                        , string.Format("R$ {0:N2}", (decimal)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<Int32>("GRUPO") == 88).Sum(x => x.Field<decimal>("SALDO_DIVIDA_AT")))
                                        , string.Format("{0:N2} %", (decimal)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<Int32>("GRUPO") == 88).Sum(x => x.Field<decimal>("SALDO_DIVIDA_AT")) / _vl_total_ativo * 100)
                                          );
                        dtCarteira.Rows.Add(
                                          "**SMS"
                                        , string.Format("{0:N0}", (Int32)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("STATUS") == "06-SMS").Sum(x => x.Field<Int32>("QTDE_CLIENTE_AT")))
                                        , string.Format("{0:N2} %", (Int32)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("STATUS") == "06-SMS").Sum(x => x.Field<Int32>("QTDE_CLIENTE_AT")) / _qt_total_ativo * 100)
                                        , string.Format("R$ {0:N2}", (decimal)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("STATUS") == "06-SMS").Sum(x => x.Field<decimal>("SALDO_AT")))
                                        , string.Format("R$ {0:N2}", (decimal)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("STATUS") == "06-SMS").Sum(x => x.Field<decimal>("SALDO_DIVIDA_AT")))
                                        , string.Format("{0:N2} %", (decimal)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("STATUS") == "06-SMS").Sum(x => x.Field<decimal>("SALDO_DIVIDA_AT")) / _vl_total_ativo * 100)
                                          );
                        dtCarteira.Rows.Add(
                                         "**Email / Carta"
                                       , string.Format("{0:N0}", (Int32)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("STATUS") == "07-EMAIL / CARTA").Sum(x => x.Field<Int32>("QTDE_CLIENTE_AT")))
                                       , string.Format("{0:N2} %", (Int32)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("STATUS") == "07-EMAIL / CARTA").Sum(x => x.Field<Int32>("QTDE_CLIENTE_AT")) / _qt_total_ativo * 100)
                                       , string.Format("R$ {0:N2}", (decimal)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("STATUS") == "07-EMAIL / CARTA").Sum(x => x.Field<decimal>("SALDO_AT")))
                                       , string.Format("R$ {0:N2}", (decimal)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("STATUS") == "07-EMAIL / CARTA").Sum(x => x.Field<decimal>("SALDO_DIVIDA_AT")))
                                       , string.Format("{0:N2} %", (decimal)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("STATUS") == "07-EMAIL / CARTA").Sum(x => x.Field<decimal>("SALDO_DIVIDA_AT")) / _vl_total_ativo * 100)
                                         );
                        dtCarteira.Rows.Add(
                                         "**Mensagem de Voz"
                                       , string.Format("{0:N0}", (Int32)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("STATUS") == "08-MENSAGEM DE VOZ").Sum(x => x.Field<Int32>("QTDE_CLIENTE_AT")))
                                       , string.Format("{0:N2} %", (Int32)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("STATUS") == "08-MENSAGEM DE VOZ").Sum(x => x.Field<Int32>("QTDE_CLIENTE_AT")) / _qt_total_ativo * 100)
                                       , string.Format("R$ {0:N2}", (decimal)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("STATUS") == "08-MENSAGEM DE VOZ").Sum(x => x.Field<decimal>("SALDO_AT")))
                                       , string.Format("R$ {0:N2}", (decimal)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("STATUS") == "08-MENSAGEM DE VOZ").Sum(x => x.Field<decimal>("SALDO_DIVIDA_AT")))
                                       , string.Format("{0:N2} %", (decimal)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("STATUS") == "08-MENSAGEM DE VOZ").Sum(x => x.Field<decimal>("SALDO_DIVIDA_AT")) / _vl_total_ativo * 100)
                                         );
                        dtCarteira.Rows.Add(
                                         "Não Acionados"
                                       , string.Format("{0:N0}", (Int32)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("STATUS") == "99-NAO ACIONADO").Sum(x => x.Field<Int32>("QTDE_CLIENTE_AT")))
                                       , string.Format("{0:N2} %", (Int32)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("STATUS") == "99-NAO ACIONADO").Sum(x => x.Field<Int32>("QTDE_CLIENTE_AT")) / _qt_total_ativo * 100)
                                       , string.Format("R$ {0:N2}", (decimal)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("STATUS") == "99-NAO ACIONADO").Sum(x => x.Field<decimal>("SALDO_AT")))
                                       , string.Format("R$ {0:N2}", (decimal)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("STATUS") == "99-NAO ACIONADO").Sum(x => x.Field<decimal>("SALDO_DIVIDA_AT")))
                                       , string.Format("{0:N2} %", (decimal)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("STATUS") == "99-NAO ACIONADO").Sum(x => x.Field<decimal>("SALDO_DIVIDA_AT")) / _vl_total_ativo * 100)
                                         );
                        dtCarteira.Rows.Add(
                                         "TOTAL"
                                       , string.Format("{0:N0}", (Int32)dsFechamento.Tables[0].AsEnumerable().Sum(x => x.Field<Int32>("QTDE_CLIENTE_AT")))
                                       , string.Format("{0:N2} %", _qt_total_ativo / _qt_total_ativo * 100)
                                       , string.Format("R$ {0:N2}", (decimal)dsFechamento.Tables[0].AsEnumerable().Sum(x => x.Field<decimal>("SALDO_AT")))
                                       , string.Format("R$ {0:N2}", (decimal)dsFechamento.Tables[0].AsEnumerable().Sum(x => x.Field<decimal>("SALDO_DIVIDA_AT")))
                                       , string.Format("{0:N2} %", _vl_total_ativo / _vl_total_ativo * 100)
                                         );
                    }

                    if (TP_STATUS == 2)
                    {

                        decimal _qt_total_bx = (decimal)dsFechamento.Tables[0].AsEnumerable().Sum(x => x.Field<Int32>("QTDE_CLIENTE_BX"));
                        decimal _vl_total_bx = (decimal)dsFechamento.Tables[0].AsEnumerable().Sum(x => x.Field<decimal>("SALDO_DIVIDA_BX"));

                        /* VALORES DE CARTEIRA */
                        dtCarteira.Rows.Add(
                                        "Contatos Efetivos"
                                      , string.Format("{0:N0}", (Int32)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<Int32>("GRUPO") == 66 || a.Field<Int32>("GRUPO") == 55).Sum(x => x.Field<Int32>("QTDE_CLIENTE_BX")))
                                      , string.Format("{0:N2} %", (Int32)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<Int32>("GRUPO") == 66 || a.Field<Int32>("GRUPO") == 55).Sum(x => x.Field<Int32>("QTDE_CLIENTE_BX"))/ _qt_total_bx*100)
                                      , string.Format("R$ {0:N2}", (decimal)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<Int32>("GRUPO") == 66 || a.Field<Int32>("GRUPO") == 55).Sum(x => x.Field<decimal>("SALDO_BX")))
                                      , string.Format("R$ {0:N2}", (decimal)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<Int32>("GRUPO") == 66 || a.Field<Int32>("GRUPO") == 55).Sum(x => x.Field<decimal>("SALDO_DIVIDA_BX")))
                                      , string.Format("{0:N2} %", (decimal)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<Int32>("GRUPO") == 66 || a.Field<Int32>("GRUPO") == 55).Sum(x => x.Field<decimal>("SALDO_DIVIDA_BX"))/_vl_total_bx*100)
                                        );
                        dtCarteira.Rows.Add(
                                        "*Contato com a Pessoa Certa (CPC)"
                                      , string.Format("{0:N0}", (Int32)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<Int32>("GRUPO") == 55).Sum(x => x.Field<Int32>("QTDE_CLIENTE_BX")))
                                      , string.Format("{0:N2} %", (Int32)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<Int32>("GRUPO") == 55).Sum(x => x.Field<Int32>("QTDE_CLIENTE_BX"))/ _qt_total_bx*100)
                                      , string.Format("R$ {0:N2}", (decimal)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<Int32>("GRUPO") == 55).Sum(x => x.Field<decimal>("SALDO_BX")))
                                      , string.Format("R$ {0:N2}", (decimal)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<Int32>("GRUPO") == 55).Sum(x => x.Field<decimal>("SALDO_DIVIDA_BX")))
                                      , string.Format("{0:N2} %", (decimal)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<Int32>("GRUPO") == 55).Sum(x => x.Field<decimal>("SALDO_DIVIDA_BX"))/ _vl_total_bx*100)
                                        );
                        dtCarteira.Rows.Add(
                                          "**Promessa de Pagamento"
                                        , string.Format("{0:N0}", (Int32)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("STATUS") == "01-PP").Sum(x => x.Field<Int32>("QTDE_CLIENTE_BX")))
                                        , string.Format("{0:N2} %", (Int32)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("STATUS") == "01-PP").Sum(x => x.Field<Int32>("QTDE_CLIENTE_BX")) / _qt_total_bx * 100)
                                        , string.Format("R$ {0:N2}", (decimal)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("STATUS") == "01-PP").Sum(x => x.Field<decimal>("SALDO_BX")))
                                        , string.Format("R$ {0:N2}", (decimal)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("STATUS") == "01-PP").Sum(x => x.Field<decimal>("SALDO_DIVIDA_BX")))
                                        , string.Format("{0:N2} %", (decimal)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("STATUS") == "01-PP").Sum(x => x.Field<decimal>("SALDO_DIVIDA_BX")) / _vl_total_bx * 100)
                                          );

                        dtCarteira.Rows.Add(
                                          "**Outros Resultado CPC"
                                        , string.Format("{0:N0}", (Int32)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("STATUS") == "02-CPC").Sum(x => x.Field<Int32>("QTDE_CLIENTE_BX")))
                                        , string.Format("{0:N2} %", (Int32)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("STATUS") == "02-CPC").Sum(x => x.Field<Int32>("QTDE_CLIENTE_BX")) / _qt_total_bx * 100)
                                        , string.Format("R$ {0:N2}", (decimal)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("STATUS") == "02-CPC").Sum(x => x.Field<decimal>("SALDO_BX")))
                                        , string.Format("R$ {0:N2}", (decimal)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("STATUS") == "02-CPC").Sum(x => x.Field<decimal>("SALDO_DIVIDA_BX")))
                                        , string.Format("{0:N2} %", (decimal)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("STATUS") == "02-CPC").Sum(x => x.Field<decimal>("SALDO_DIVIDA_BX")) / _vl_total_bx * 100)
                                          );

                        dtCarteira.Rows.Add(
                                          "*Recado Terceiro"
                                        , string.Format("{0:N0}", (Int32)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("STATUS") == "03-RECADO TERCEIRO").Sum(x => x.Field<Int32>("QTDE_CLIENTE_BX")))
                                        , string.Format("{0:N2} %", (Int32)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("STATUS") == "03-RECADO TERCEIRO").Sum(x => x.Field<Int32>("QTDE_CLIENTE_BX")) / _qt_total_bx * 100)
                                        , string.Format("R$ {0:N2}", (decimal)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("STATUS") == "03-RECADO TERCEIRO").Sum(x => x.Field<decimal>("SALDO_BX")))
                                        , string.Format("R$ {0:N2}", (decimal)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("STATUS") == "03-RECADO TERCEIRO").Sum(x => x.Field<decimal>("SALDO_DIVIDA_BX")))
                                        , string.Format("{0:N2} %", (decimal)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("STATUS") == "03-RECADO TERCEIRO").Sum(x => x.Field<decimal>("SALDO_DIVIDA_BX")) / _vl_total_bx * 100)
                                          );

                        dtCarteira.Rows.Add(
                                          "Problema Cadastral"
                                        , string.Format("{0:N0}", (Int32)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("STATUS") == "04-PROBLEMA CADASTRAL").Sum(x => x.Field<Int32>("QTDE_CLIENTE_BX")))
                                        , string.Format("{0:N2} %", (Int32)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("STATUS") == "04-PROBLEMA CADASTRAL").Sum(x => x.Field<Int32>("QTDE_CLIENTE_BX")) / _qt_total_bx * 100)
                                        , string.Format("R$ {0:N2}", (decimal)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("STATUS") == "04-PROBLEMA CADASTRAL").Sum(x => x.Field<decimal>("SALDO_BX")))
                                        , string.Format("R$ {0:N2}", (decimal)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("STATUS") == "04-PROBLEMA CADASTRAL").Sum(x => x.Field<decimal>("SALDO_DIVIDA_BX")))
                                        , string.Format("{0:N2} %", (decimal)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("STATUS") == "04-PROBLEMA CADASTRAL").Sum(x => x.Field<decimal>("SALDO_DIVIDA_BX")) / _vl_total_bx * 100)
                                          );
                        dtCarteira.Rows.Add(
                                          "Indicio Problema Cadastral"
                                        , string.Format("{0:N0}", (Int32)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("STATUS") == "05-INDICIO PROBLEMA CADASTRAL").Sum(x => x.Field<Int32>("QTDE_CLIENTE_BX")))
                                        , string.Format("{0:N2} %", (Int32)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("STATUS") == "05-INDICIO PROBLEMA CADASTRAL").Sum(x => x.Field<Int32>("QTDE_CLIENTE_BX")) / _qt_total_bx * 100)
                                        , string.Format("R$ {0:N2}", (decimal)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("STATUS") == "05-INDICIO PROBLEMA CADASTRAL").Sum(x => x.Field<decimal>("SALDO_BX")))
                                        , string.Format("R$ {0:N2}", (decimal)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("STATUS") == "05-INDICIO PROBLEMA CADASTRAL").Sum(x => x.Field<decimal>("SALDO_DIVIDA_BX")))
                                        , string.Format("{0:N2} %", (decimal)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("STATUS") == "05-INDICIO PROBLEMA CADASTRAL").Sum(x => x.Field<decimal>("SALDO_DIVIDA_BX")) / _vl_total_bx * 100)
                                          );
                        dtCarteira.Rows.Add(
                                          "Midia"
                                        , string.Format("{0:N0}", (Int32)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<Int32>("GRUPO") == 88).Sum(x => x.Field<Int32>("QTDE_CLIENTE_BX")))
                                        , string.Format("{0:N2} %", (Int32)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<Int32>("GRUPO") == 88).Sum(x => x.Field<Int32>("QTDE_CLIENTE_BX")) / _qt_total_bx * 100)
                                        , string.Format("R$ {0:N2}", (decimal)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<Int32>("GRUPO") == 88).Sum(x => x.Field<decimal>("SALDO_BX")))
                                        , string.Format("R$ {0:N2}", (decimal)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<Int32>("GRUPO") == 88).Sum(x => x.Field<decimal>("SALDO_DIVIDA_BX")))
                                        , string.Format("{0:N2} %", (decimal)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<Int32>("GRUPO") == 88).Sum(x => x.Field<decimal>("SALDO_DIVIDA_BX")) / _vl_total_bx * 100)
                                          );
                        dtCarteira.Rows.Add(
                                          "**SMS"
                                        , string.Format("{0:N0}", (Int32)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("STATUS") == "06-SMS").Sum(x => x.Field<Int32>("QTDE_CLIENTE_BX")))
                                        , string.Format("{0:N2} %", (Int32)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("STATUS") == "06-SMS").Sum(x => x.Field<Int32>("QTDE_CLIENTE_BX")) / _qt_total_bx * 100)
                                        , string.Format("R$ {0:N2}", (decimal)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("STATUS") == "06-SMS").Sum(x => x.Field<decimal>("SALDO_BX")))
                                        , string.Format("R$ {0:N2}", (decimal)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("STATUS") == "06-SMS").Sum(x => x.Field<decimal>("SALDO_DIVIDA_BX")))
                                        , string.Format("{0:N2} %", (decimal)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("STATUS") == "06-SMS").Sum(x => x.Field<decimal>("SALDO_DIVIDA_BX")) / _vl_total_bx * 100)
                                          );
                        dtCarteira.Rows.Add(
                                         "**Email / Carta"
                                       , string.Format("{0:N0}", (Int32)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("STATUS") == "07-EMAIL / CARTA").Sum(x => x.Field<Int32>("QTDE_CLIENTE_BX")))
                                       , string.Format("{0:N2} %", (Int32)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("STATUS") == "07-EMAIL / CARTA").Sum(x => x.Field<Int32>("QTDE_CLIENTE_BX")) / _qt_total_bx * 100)
                                       , string.Format("R$ {0:N2}", (decimal)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("STATUS") == "07-EMAIL / CARTA").Sum(x => x.Field<decimal>("SALDO_BX")))
                                       , string.Format("R$ {0:N2}", (decimal)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("STATUS") == "07-EMAIL / CARTA").Sum(x => x.Field<decimal>("SALDO_DIVIDA_BX")))
                                       , string.Format("{0:N2} %", (decimal)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("STATUS") == "07-EMAIL / CARTA").Sum(x => x.Field<decimal>("SALDO_DIVIDA_BX")) / _vl_total_bx * 100)
                                         );
                        dtCarteira.Rows.Add(
                                         "**Mensagem de Voz"
                                       , string.Format("{0:N0}", (Int32)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("STATUS") == "08-MENSAGEM DE VOZ").Sum(x => x.Field<Int32>("QTDE_CLIENTE_BX")))
                                       , string.Format("{0:N2} %", (Int32)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("STATUS") == "08-MENSAGEM DE VOZ").Sum(x => x.Field<Int32>("QTDE_CLIENTE_BX")) / _qt_total_bx * 100)
                                       , string.Format("R$ {0:N2}", (decimal)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("STATUS") == "08-MENSAGEM DE VOZ").Sum(x => x.Field<decimal>("SALDO_BX")))
                                       , string.Format("R$ {0:N2}", (decimal)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("STATUS") == "08-MENSAGEM DE VOZ").Sum(x => x.Field<decimal>("SALDO_DIVIDA_BX")))
                                       , string.Format("{0:N2} %", (decimal)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("STATUS") == "08-MENSAGEM DE VOZ").Sum(x => x.Field<decimal>("SALDO_DIVIDA_BX")) / _vl_total_bx * 100)
                                         );
                        dtCarteira.Rows.Add(
                                         "Não Acionados"
                                       , string.Format("{0:N0}", (Int32)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("STATUS") == "99-NAO ACIONADO").Sum(x => x.Field<Int32>("QTDE_CLIENTE_BX")))
                                       , string.Format("{0:N2} %", (Int32)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("STATUS") == "99-NAO ACIONADO").Sum(x => x.Field<Int32>("QTDE_CLIENTE_BX")) / _qt_total_bx * 100)
                                       , string.Format("R$ {0:N2}", (decimal)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("STATUS") == "99-NAO ACIONADO").Sum(x => x.Field<decimal>("SALDO_BX")))
                                       , string.Format("R$ {0:N2}", (decimal)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("STATUS") == "99-NAO ACIONADO").Sum(x => x.Field<decimal>("SALDO_DIVIDA_BX")))
                                       , string.Format("{0:N2} %", (decimal)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("STATUS") == "99-NAO ACIONADO").Sum(x => x.Field<decimal>("SALDO_DIVIDA_BX")) / _vl_total_bx * 100)
                                         );
                        dtCarteira.Rows.Add(
                                         "TOTAL"
                                       , string.Format("{0:N0}", (Int32)dsFechamento.Tables[0].AsEnumerable().Sum(x => x.Field<Int32>("QTDE_CLIENTE_BX")))
                                       , string.Format("{0:N2} %", (Int32)dsFechamento.Tables[0].AsEnumerable().Sum(x => x.Field<Int32>("QTDE_CLIENTE_BX")) / _qt_total_bx * 100)
                                       , string.Format("R$ {0:N2}", (decimal)dsFechamento.Tables[0].AsEnumerable().Sum(x => x.Field<decimal>("SALDO_BX")))
                                       , string.Format("R$ {0:N2}", (decimal)dsFechamento.Tables[0].AsEnumerable().Sum(x => x.Field<decimal>("SALDO_DIVIDA_BX")))
                                       , string.Format("{0:N2} %", (decimal)dsFechamento.Tables[0].AsEnumerable().Sum(x => x.Field<decimal>("SALDO_DIVIDA_BX")) / _vl_total_bx * 100)
                                         );
                    }

                    if (TP_STATUS == 0)
                    {
                        decimal _qt_total = (decimal)dsFechamento.Tables[0].AsEnumerable().Sum(x => x.Field<Int32>("QTDE_CLIENTE_TT"));
                        decimal _vl_total = (decimal)dsFechamento.Tables[0].AsEnumerable().Sum(x => x.Field<decimal>("SALDO_DIVIDA_TT"));

                        /* VALORES DE CARTEIRA */
                        dtCarteira.Rows.Add(
                                        "Contatos Efetivos"
                                      , string.Format("{0:N0}", (Int32)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<Int32>("GRUPO") == 66 || a.Field<Int32>("GRUPO") == 55).Sum(x => x.Field<Int32>("QTDE_CLIENTE_TT")))
                                      , string.Format("{0:N2} %", (Int32)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<Int32>("GRUPO") == 66 || a.Field<Int32>("GRUPO") == 55).Sum(x => x.Field<Int32>("QTDE_CLIENTE_TT")) / _qt_total * 100 )
                                      , string.Format("R$ {0:N2}", (decimal)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<Int32>("GRUPO") == 66 || a.Field<Int32>("GRUPO") == 55).Sum(x => x.Field<decimal>("SALDO_TT")))
                                      , string.Format("R$ {0:N2}", (decimal)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<Int32>("GRUPO") == 66 || a.Field<Int32>("GRUPO") == 55).Sum(x => x.Field<decimal>("SALDO_DIVIDA_TT")))
                                      , string.Format("{0:N2} %", (decimal)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<Int32>("GRUPO") == 66 || a.Field<Int32>("GRUPO") == 55).Sum(x => x.Field<decimal>("SALDO_DIVIDA_TT"))/ _vl_total * 100)                                       );
                        dtCarteira.Rows.Add(
                                        "*Contato com a Pessoa Certa (CPC)"
                                      , string.Format("{0:N0}", (Int32)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<Int32>("GRUPO") == 55).Sum(x => x.Field<Int32>("QTDE_CLIENTE_TT")))
                                      , string.Format("{0:N2} %", (Int32)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<Int32>("GRUPO") == 55).Sum(x => x.Field<Int32>("QTDE_CLIENTE_TT")) / _qt_total * 100)
                                      , string.Format("R$ {0:N2}", (decimal)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<Int32>("GRUPO") == 55).Sum(x => x.Field<decimal>("SALDO_TT")))
                                      , string.Format("R$ {0:N2}", (decimal)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<Int32>("GRUPO") == 55).Sum(x => x.Field<decimal>("SALDO_DIVIDA_TT")))
                                      , string.Format("{0:N2} %", (decimal)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<Int32>("GRUPO") == 55).Sum(x => x.Field<decimal>("SALDO_DIVIDA_TT")) / _vl_total * 100)
                                        );
                        dtCarteira.Rows.Add(
                                          "**Promessa de Pagamento"
                                        , string.Format("{0:N0}", (Int32)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("STATUS") == "01-PP").Sum(x => x.Field<Int32>("QTDE_CLIENTE_TT")))
                                        , string.Format("{0:N2} %", (Int32)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("STATUS") == "01-PP").Sum(x => x.Field<Int32>("QTDE_CLIENTE_TT")) / _qt_total * 100)
                                        , string.Format("R$ {0:N2}", (decimal)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("STATUS") == "01-PP").Sum(x => x.Field<decimal>("SALDO_TT")))
                                        , string.Format("R$ {0:N2}", (decimal)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("STATUS") == "01-PP").Sum(x => x.Field<decimal>("SALDO_DIVIDA_TT")))
                                        , string.Format("{0:N2} %", (decimal)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("STATUS") == "01-PP").Sum(x => x.Field<decimal>("SALDO_DIVIDA_TT")) / _vl_total * 100)
                                          );

                        dtCarteira.Rows.Add(
                                          "**Outros Resultado CPC"
                                        , string.Format("{0:N0}", (Int32)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("STATUS") == "02-CPC").Sum(x => x.Field<Int32>("QTDE_CLIENTE_TT")))
                                        , string.Format("{0:N2} %", (Int32)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("STATUS") == "02-CPC").Sum(x => x.Field<Int32>("QTDE_CLIENTE_TT")) / _qt_total * 100)
                                        , string.Format("R$ {0:N2}", (decimal)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("STATUS") == "02-CPC").Sum(x => x.Field<decimal>("SALDO_TT")))
                                        , string.Format("R$ {0:N2}", (decimal)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("STATUS") == "02-CPC").Sum(x => x.Field<decimal>("SALDO_DIVIDA_TT")))
                                        , string.Format("{0:N2} %", (decimal)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("STATUS") == "02-CPC").Sum(x => x.Field<decimal>("SALDO_DIVIDA_TT")) / _vl_total * 100)
                                          );

                        dtCarteira.Rows.Add(
                                          "*Recado Terceiro"
                                        , string.Format("{0:N0}", (Int32)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("STATUS") == "03-RECADO TERCEIRO").Sum(x => x.Field<Int32>("QTDE_CLIENTE_TT")))
                                        , string.Format("{0:N2} %", (Int32)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("STATUS") == "03-RECADO TERCEIRO").Sum(x => x.Field<Int32>("QTDE_CLIENTE_TT")) / _qt_total * 100)
                                        , string.Format("R$ {0:N2}", (decimal)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("STATUS") == "03-RECADO TERCEIRO").Sum(x => x.Field<decimal>("SALDO_TT")))
                                        , string.Format("R$ {0:N2}", (decimal)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("STATUS") == "03-RECADO TERCEIRO").Sum(x => x.Field<decimal>("SALDO_DIVIDA_TT")))
                                        , string.Format("{0:N2} %", (decimal)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("STATUS") == "03-RECADO TERCEIRO").Sum(x => x.Field<decimal>("SALDO_DIVIDA_TT")) / _vl_total * 100)
                                          );

                        dtCarteira.Rows.Add(
                                          "Problema Cadastral"
                                        , string.Format("{0:N0}", (Int32)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("STATUS") == "04-PROBLEMA CADASTRAL").Sum(x => x.Field<Int32>("QTDE_CLIENTE_TT")))
                                        , string.Format("{0:N2} %", (Int32)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("STATUS") == "04-PROBLEMA CADASTRAL").Sum(x => x.Field<Int32>("QTDE_CLIENTE_TT")) / _qt_total * 100)
                                        , string.Format("R$ {0:N2}", (decimal)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("STATUS") == "04-PROBLEMA CADASTRAL").Sum(x => x.Field<decimal>("SALDO_TT")))
                                        , string.Format("R$ {0:N2}", (decimal)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("STATUS") == "04-PROBLEMA CADASTRAL").Sum(x => x.Field<decimal>("SALDO_DIVIDA_TT")))
                                        , string.Format("{0:N2} %", (decimal)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("STATUS") == "04-PROBLEMA CADASTRAL").Sum(x => x.Field<decimal>("SALDO_DIVIDA_TT")) / _vl_total * 100)
                                          );
                        dtCarteira.Rows.Add(
                                          "Indicio Problema Cadastral"
                                        , string.Format("{0:N0}", (Int32)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("STATUS") == "05-INDICIO PROBLEMA CADASTRAL").Sum(x => x.Field<Int32>("QTDE_CLIENTE_TT")))
                                        , string.Format("{0:N2} %", (Int32)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("STATUS") == "05-INDICIO PROBLEMA CADASTRAL").Sum(x => x.Field<Int32>("QTDE_CLIENTE_TT")) / _qt_total * 100)
                                        , string.Format("R$ {0:N2}", (decimal)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("STATUS") == "05-INDICIO PROBLEMA CADASTRAL").Sum(x => x.Field<decimal>("SALDO_TT")))
                                        , string.Format("R$ {0:N2}", (decimal)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("STATUS") == "05-INDICIO PROBLEMA CADASTRAL").Sum(x => x.Field<decimal>("SALDO_DIVIDA_TT")))
                                        , string.Format("{0:N2} %", (decimal)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("STATUS") == "05-INDICIO PROBLEMA CADASTRAL").Sum(x => x.Field<decimal>("SALDO_DIVIDA_TT")) / _vl_total * 100)
                                          );
                        dtCarteira.Rows.Add(
                                          "Midia"
                                        , string.Format("{0:N0}", (Int32)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<Int32>("GRUPO") == 88).Sum(x => x.Field<Int32>("QTDE_CLIENTE_TT")))
                                        , string.Format("{0:N2} %", (Int32)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<Int32>("GRUPO") == 88).Sum(x => x.Field<Int32>("QTDE_CLIENTE_TT")) / _qt_total * 100)
                                        , string.Format("R$ {0:N2}", (decimal)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<Int32>("GRUPO") == 88).Sum(x => x.Field<decimal>("SALDO_TT")))
                                        , string.Format("R$ {0:N2}", (decimal)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<Int32>("GRUPO") == 88).Sum(x => x.Field<decimal>("SALDO_DIVIDA_TT")))
                                        , string.Format("{0:N2} %", (decimal)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<Int32>("GRUPO") == 88).Sum(x => x.Field<decimal>("SALDO_DIVIDA_TT")) / _vl_total * 100)
                                          );
                        dtCarteira.Rows.Add(
                                          "**SMS"
                                        , string.Format("{0:N0}", (Int32)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("STATUS") == "06-SMS").Sum(x => x.Field<Int32>("QTDE_CLIENTE_TT")))
                                        , string.Format("{0:N2} %", (Int32)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("STATUS") == "06-SMS").Sum(x => x.Field<Int32>("QTDE_CLIENTE_TT")) / _qt_total * 100)
                                        , string.Format("R$ {0:N2}", (decimal)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("STATUS") == "06-SMS").Sum(x => x.Field<decimal>("SALDO_TT")))
                                        , string.Format("R$ {0:N2}", (decimal)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("STATUS") == "06-SMS").Sum(x => x.Field<decimal>("SALDO_DIVIDA_TT")))
                                        , string.Format("{0:N2} %", (decimal)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("STATUS") == "06-SMS").Sum(x => x.Field<decimal>("SALDO_DIVIDA_TT")) / _vl_total * 100)
                                          );
                        dtCarteira.Rows.Add(
                                         "**Email / Carta"
                                       , string.Format("{0:N0}", (Int32)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("STATUS") == "07-EMAIL / CARTA").Sum(x => x.Field<Int32>("QTDE_CLIENTE_TT")))
                                       , string.Format("{0:N2} %", (Int32)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("STATUS") == "07-EMAIL / CARTA").Sum(x => x.Field<Int32>("QTDE_CLIENTE_TT")) / _qt_total * 100)
                                       , string.Format("R$ {0:N2}", (decimal)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("STATUS") == "07-EMAIL / CARTA").Sum(x => x.Field<decimal>("SALDO_TT")))
                                       , string.Format("R$ {0:N2}", (decimal)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("STATUS") == "07-EMAIL / CARTA").Sum(x => x.Field<decimal>("SALDO_DIVIDA_TT")))
                                       , string.Format("{0:N2} %", (decimal)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("STATUS") == "07-EMAIL / CARTA").Sum(x => x.Field<decimal>("SALDO_DIVIDA_TT")) / _vl_total * 100)
                                         );
                        dtCarteira.Rows.Add(
                                         "**Mensagem de Voz"
                                       , string.Format("{0:N0}", (Int32)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("STATUS") == "08-MENSAGEM DE VOZ").Sum(x => x.Field<Int32>("QTDE_CLIENTE_TT")))
                                       , string.Format("{0:N2} %", (Int32)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("STATUS") == "08-MENSAGEM DE VOZ").Sum(x => x.Field<Int32>("QTDE_CLIENTE_TT")) / _qt_total * 100)
                                       , string.Format("R$ {0:N2}", (decimal)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("STATUS") == "08-MENSAGEM DE VOZ").Sum(x => x.Field<decimal>("SALDO_TT")))
                                       , string.Format("R$ {0:N2}", (decimal)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("STATUS") == "08-MENSAGEM DE VOZ").Sum(x => x.Field<decimal>("SALDO_DIVIDA_TT")))
                                       , string.Format("{0:N2} %", (decimal)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("STATUS") == "08-MENSAGEM DE VOZ").Sum(x => x.Field<decimal>("SALDO_DIVIDA_TT")) / _vl_total * 100)
                                         );
                        dtCarteira.Rows.Add(
                                         "Não Acionados"
                                       , string.Format("{0:N0}", (Int32)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("STATUS") == "99-NAO ACIONADO").Sum(x => x.Field<Int32>("QTDE_CLIENTE_TT")))
                                       , string.Format("{0:N2} %", (Int32)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("STATUS") == "99-NAO ACIONADO").Sum(x => x.Field<Int32>("QTDE_CLIENTE_TT")) / _qt_total * 100)
                                       , string.Format("R$ {0:N2}", (decimal)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("STATUS") == "99-NAO ACIONADO").Sum(x => x.Field<decimal>("SALDO_TT")))
                                       , string.Format("R$ {0:N2}", (decimal)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("STATUS") == "99-NAO ACIONADO").Sum(x => x.Field<decimal>("SALDO_DIVIDA_TT")))
                                       , string.Format("{0:N2} %", (decimal)dsFechamento.Tables[0].AsEnumerable().Where(a => a.Field<string>("STATUS") == "99-NAO ACIONADO").Sum(x => x.Field<decimal>("SALDO_DIVIDA_TT")) / _vl_total * 100)
                                         );
                        dtCarteira.Rows.Add(
                                         "TOTAL"
                                       , string.Format("{0:N0}", _qt_total)
                                       , string.Format("{0:N2} %", (Int32)dsFechamento.Tables[0].AsEnumerable().Sum(x => x.Field<Int32>("QTDE_CLIENTE_TT")) / _qt_total * 100)
                                       , string.Format("R$ {0:N2}", (decimal)dsFechamento.Tables[0].AsEnumerable().Sum(x => x.Field<decimal>("SALDO_TT")))
                                       , string.Format("R$ {0:N2}", _vl_total)
                                       , string.Format("{0:N2} %", _vl_total / _vl_total * 100)
                                         );
                    }


                    dsFinal.Tables.Add(dtCarteira);
                }
                return dsFinal;
            }
            catch (Exception ex)
            {
                throw new Exception("FAT.CmdReversao_001: " + ex.Message, ex);
            }
        }
    }
}