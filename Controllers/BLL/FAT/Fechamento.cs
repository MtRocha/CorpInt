using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;
using Intranet_NEW.Controllers.DAL;

namespace Intranet.BLL.FAT
{
    public class Fechamento
    {
        DAL_MIS AcessaDadosMis = new Intranet.DAL.DAL_MIS();
        DAL_PROC AcessaDadosProc = new Intranet.DAL.DAL_PROC();

        public DataSet GeraEspelhoFaturamentoMensal(DateTime dtReferencia, int EMP)
        {
            try
            {
                SqlCommand sqlcommand = new SqlCommand();
                sqlcommand.CommandType = CommandType.StoredProcedure;

                if (EMP == 2)
                {
                    sqlcommand.CommandText = "SP_FAT_ESPELHO_FATURAMENTO_MENSAL";
                    sqlcommand.Parameters.AddWithValue("@DT_REFERENCIA", dtReferencia.ToString("yyyyMMdd"));
                    sqlcommand.Parameters.AddWithValue("@EMP", EMP);
                }
                else
                {
                    sqlcommand.CommandText = "SP_FAT_ESPELHO_FATURAMENTO_MENSAL_ANT";
                    sqlcommand.Parameters.AddWithValue("@DT_REFERENCIA", dtReferencia.ToString("yyyyMMdd"));
                    sqlcommand.Parameters.AddWithValue("@EMP", EMP);
                }

                DataSet dsFechamento = AcessaDadosMis.ConsultaSQL(sqlcommand);
                               
                if ((dsFechamento.Tables.Count > 0) && (dsFechamento.Tables[0].Rows.Count > 0))
                {
                    DataTable dtMediaMulta = new DataTable("Table4");
                    dtMediaMulta.Columns.Add("NM_PROCESSO", typeof(string));
                    dtMediaMulta.Columns.Add("TX_MEDIA", typeof(decimal));
                    dtMediaMulta.Columns.Add("TX_MULTA", typeof(decimal));

                    decimal TotalMulta = 0;
                    for (int col = 0; col < dsFechamento.Tables[1].Columns.Count; col++)
                    {
                        DataRow drNovaLinha = dtMediaMulta.NewRow();
                        drNovaLinha[0] = dsFechamento.Tables[1].Columns[col].ColumnName == "TX_CE" ? "1 - CE" :
                                         dsFechamento.Tables[1].Columns[col].ColumnName == "TX_INDISPONIVEL" ? "2 - Indisp." :
                                         dsFechamento.Tables[1].Columns[col].ColumnName == "TX_ABANDONO" ? "3 - Aband." :
                                         dsFechamento.Tables[1].Columns[col].ColumnName == "TX_TME_NS" ? "4 - TME/NS" :
                                         dsFechamento.Tables[1].Columns[col].ColumnName == "TX_TEMPO_FALANDO" ? "5 - Tempo Fal." :
                                         dsFechamento.Tables[1].Columns[col].ColumnName == "TX_TEMPO_PRODUTIVO" ? "6 - Tempo Prod." :
                                         dsFechamento.Tables[1].Columns[col].ColumnName == "TX_PP" ? "7 - PP" :
                                         dsFechamento.Tables[1].Columns[col].ColumnName == "TX_CPC" ? "8 - CPC" :
                                         dsFechamento.Tables[1].Columns[col].ColumnName == "QTDE_CPC" ? "CPC" :
                                         dsFechamento.Tables[1].Columns[col].ColumnName == "QTDE_PP" ? "PP" :
                                         dsFechamento.Tables[1].Columns[col].ColumnName == "TX_PP_CLIENTE" ? "TX_PP(CLIENTE)" :
                                         dsFechamento.Tables[1].Columns[col].ColumnName == "QT_PA_LOGADA" ? "PA Logada" :
                                         dsFechamento.Tables[1].Columns[col].ColumnName == "VL_CUSTO_LIGACAO" ? "Custo Ligação" : "";

                        decimal Valor = dsFechamento.Tables[1].Rows[0][col].GetType() == typeof(decimal) ? (decimal)dsFechamento.Tables[1].Rows[0][col] : (decimal)((int)dsFechamento.Tables[1].Rows[0][col]);
                        drNovaLinha[1] = Valor;

                        if (dsFechamento.Tables[2].Rows[0][col].ToString() != "")
                        {
                            decimal Multa = dsFechamento.Tables[2].Rows[0][col].GetType() == typeof(decimal) ? (decimal)dsFechamento.Tables[2].Rows[0][col] : (decimal)((int)dsFechamento.Tables[2].Rows[0][col]);
                            drNovaLinha[2] = Multa;
                            TotalMulta += Multa;
                        }

                        if (drNovaLinha[0].ToString() != "")
                            dtMediaMulta.Rows.Add(drNovaLinha);
                    }
                    dtMediaMulta.Rows.Add("Total de Multa", null, TotalMulta);


                    dsFechamento.Tables.Remove(dsFechamento.Tables[2]);
                    dsFechamento.Tables.Remove(dsFechamento.Tables[1]);

                    dsFechamento.Tables.Add(dtMediaMulta);
                }
                return dsFechamento;
            }
            catch (Exception ex)
            {
                throw new Exception("RET.CmdFechamento_001: " + ex.Message, ex);
            }
        }


        public DataSet GeraEspelhoProjFaturamentoMensal(int MetaPA, int ProjSab)
        {
            try
            {
                SqlCommand sqlcommand = new SqlCommand();
                sqlcommand.CommandType = CommandType.StoredProcedure;
                sqlcommand.CommandText = "SP_FAT_ESPELHO_FATURAMENTO_PROJECAO";
                sqlcommand.Parameters.AddWithValue("@PROJ_PA", MetaPA);
                sqlcommand.Parameters.AddWithValue("@PROJ_SAB", ProjSab);
                DataSet dsFechamento = AcessaDadosProc.ConsultaSQL(sqlcommand);

                return dsFechamento;
            }
            catch (Exception ex)
            {
                throw new Exception("RET.CmdFechamento_003: " + ex.Message, ex);
            }
        }

        public void AtualizaCustoLigacao(DateTime DT_ACIONAMENTO, decimal VL_CUSTO_LIGACAO)
        {
            try
            {
                SqlCommand sqlcommand = new SqlCommand();
                sqlcommand.CommandType = CommandType.Text;
                sqlcommand.CommandText = "UPDATE TBL_FAT_RELATORIO_ESPELHO_MENSAL SET VL_CUSTO_LIGACAO = @VL_CUSTO_LIGACAO WHERE DT_ACIONAMENTO = @DT_ACIONAMENTO";

                sqlcommand.Parameters.AddWithValue("@DT_ACIONAMENTO", DT_ACIONAMENTO.ToString("yyyyMMdd"));
                sqlcommand.Parameters.AddWithValue("@VL_CUSTO_LIGACAO", VL_CUSTO_LIGACAO);

                DAL_MIS AcessaDadosMis = new Intranet.DAL.DAL_MIS();
                AcessaDadosMis.ExecutaComandoSQL(sqlcommand);
            }
            catch (Exception ex)
            {
                throw new Exception("RET.CmdFechamento_002: " + ex.Message, ex);
            }
        }
    }
}