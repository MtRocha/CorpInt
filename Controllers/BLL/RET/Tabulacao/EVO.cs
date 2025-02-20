using Intranet_NEW.Controllers.DAL;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Intranet.BLL.RET.Tabulacao
{
    public class EVO : Filtro
    {
        public DataSet GeraRelatorioEvolucaoMensal(string DT_INI, string DT_FIM, string NR_COORDENADOR, string NR_SUPERVISOR, string NR_OPERADOR)
        {
            try
            {
                SqlCommand sqlcommand = new SqlCommand();
                sqlcommand.CommandType = CommandType.StoredProcedure;
                sqlcommand.CommandText = "SP_RET_RELATORIO_EVOLUCAO_OPERADOR";

                sqlcommand.Parameters.AddWithValue("@DT_INI", DT_INI);
                sqlcommand.Parameters.AddWithValue("@DT_FIM", DT_FIM);
                sqlcommand.Parameters.AddWithValue("@NR_COORDENADOR", NR_COORDENADOR);
                sqlcommand.Parameters.AddWithValue("@NR_SUPERVISOR", NR_SUPERVISOR);
                sqlcommand.Parameters.AddWithValue("@NR_OPERADOR", NR_OPERADOR);

                DAL_MIS AcessaDadosMisN = new Intranet.DAL.DAL_MIS();
                DataSet dsTabulacao = AcessaDadosMisN.ConsultaSQL(sqlcommand);

                if (dsTabulacao.Tables.Count > 0)
                {
                    Decimal PARIND01 = 85;
                    Decimal PARIND02 = 45;
                    Decimal PARIND03 = 60;
                    Decimal PARIND04 = 70;
                    Decimal PARIND05 = 95;

                    var DT_COMPLETO = dsTabulacao.Tables[0].AsEnumerable();
                    DataTable DT_MES = dsTabulacao.Tables[0].DefaultView.ToTable(true, "DT_ACIONAMENTO");
                    DataTable DT_USR = dsTabulacao.Tables[0].DefaultView.ToTable(true, "DESCRICAO", "NM_LOGIN_OLOS");

                    DataTable DT_EVO = new DataTable();
                    DT_EVO.Columns.Add("NM_COLABORADOR", typeof(string));
                    DT_EVO.Columns.Add("NM_LOGIN_OLOS", typeof(string));
                    DT_EVO.Columns.Add("NM_INDICADOR", typeof(string));

                    foreach (DataRow dr in DT_MES.Rows)
                    {
                        DT_EVO.Columns.Add(dr[0].ToString() + "_A", typeof(string));
                        DT_EVO.Columns.Add(dr[0].ToString() + "_B", typeof(string));
                        DT_EVO.Columns.Add(dr[0].ToString() + "_C", typeof(string));
                    }

                    foreach (DataRow DR_COL in DT_USR.Rows)
                    {
                        DataRow DR_NM_COLABORADOR01 = DT_EVO.NewRow();
                        DataRow DR_NM_COLABORADOR02 = DT_EVO.NewRow();
                        DataRow DR_NM_COLABORADOR03 = DT_EVO.NewRow();
                        DataRow DR_NM_COLABORADOR04 = DT_EVO.NewRow();
                        DataRow DR_NM_COLABORADOR05 = DT_EVO.NewRow();

                        DR_NM_COLABORADOR01[0] = DR_COL[0];
                        DR_NM_COLABORADOR01[1] = DR_COL[1];

                        DR_NM_COLABORADOR01[2] = "CE";
                        DR_NM_COLABORADOR02[2] = "CPC";
                        DR_NM_COLABORADOR03[2] = "PP";
                        DR_NM_COLABORADOR04[2] = "T. Falando";
                        DR_NM_COLABORADOR05[2] = "T. Produtivo";

                        Decimal OLDIND01 = 0;
                        Decimal OLDIND02 = 0;
                        Decimal OLDIND03 = 0;
                        Decimal OLDIND04 = 0;
                        Decimal OLDIND05 = 0;
                        string[] RETORNO = new string[] { "", "" };

                        for (int lm = 0; lm < DT_MES.Rows.Count; lm++)
                        {
                            DataRow DR_MES = DT_MES.Rows[lm];

                            var RESULT = DT_COMPLETO.Where(f => f.Field<string>("DT_ACIONAMENTO") == DR_MES[0].ToString() && f.Field<string>("DESCRICAO") == DR_COL[0].ToString()).Select(s => new { IND01 = s.Field<Decimal>("TX_CE"), IND02 = s.Field<Decimal>("TX_CPC"), IND03 = s.Field<Decimal>("TX_PP"), IND04 = s.Field<Decimal>("TX_TEMPO_FALANDO"), IND05 = s.Field<Decimal>("TX_TEMPO_PRODUTIVO") }).ToList();
                            if (RESULT.Count > 0)
                            {
                                DR_NM_COLABORADOR01[DR_MES[0].ToString() + "_A"] = string.Format("{0:N2}", RESULT[0].IND01);
                                DR_NM_COLABORADOR02[DR_MES[0].ToString() + "_A"] = string.Format("{0:N2}", RESULT[0].IND02);
                                DR_NM_COLABORADOR03[DR_MES[0].ToString() + "_A"] = string.Format("{0:N2}", RESULT[0].IND03);
                                DR_NM_COLABORADOR04[DR_MES[0].ToString() + "_A"] = string.Format("{0:N2}", RESULT[0].IND04);
                                DR_NM_COLABORADOR05[DR_MES[0].ToString() + "_A"] = string.Format("{0:N2}", RESULT[0].IND05);

                                RETORNO = VeriificaIndicadorImg(PARIND01, OLDIND01, RESULT[0].IND01);
                                DR_NM_COLABORADOR01[DR_MES[0].ToString() + "_B"] = RETORNO[0];
                                DR_NM_COLABORADOR01[DR_MES[0].ToString() + "_C"] = RETORNO[1];

                                RETORNO = VeriificaIndicadorImg(PARIND02, OLDIND02, RESULT[0].IND02);
                                DR_NM_COLABORADOR02[DR_MES[0].ToString() + "_B"] = RETORNO[0];
                                DR_NM_COLABORADOR02[DR_MES[0].ToString() + "_C"] = RETORNO[1];

                                RETORNO = VeriificaIndicadorImg(PARIND03, OLDIND03, RESULT[0].IND03);
                                DR_NM_COLABORADOR03[DR_MES[0].ToString() + "_B"] = RETORNO[0];
                                DR_NM_COLABORADOR03[DR_MES[0].ToString() + "_C"] = RETORNO[1];

                                RETORNO = VeriificaIndicadorImg(PARIND04, OLDIND04, RESULT[0].IND04);
                                DR_NM_COLABORADOR04[DR_MES[0].ToString() + "_B"] = RETORNO[0];
                                DR_NM_COLABORADOR04[DR_MES[0].ToString() + "_C"] = RETORNO[1];

                                RETORNO = VeriificaIndicadorImg(PARIND05, OLDIND05, RESULT[0].IND05);
                                DR_NM_COLABORADOR05[DR_MES[0].ToString() + "_B"] = RETORNO[0];
                                DR_NM_COLABORADOR05[DR_MES[0].ToString() + "_C"] = RETORNO[1];
                                
                                OLDIND01 = RESULT[0].IND01;
                                OLDIND02 = RESULT[0].IND02;
                                OLDIND03 = RESULT[0].IND03;
                                OLDIND04 = RESULT[0].IND04;
                                OLDIND05 = RESULT[0].IND05;
                            }
                            else
                            {
                                OLDIND01 = 0;
                                OLDIND02 = 0;
                                OLDIND03 = 0;
                                OLDIND04 = 0;
                                OLDIND05 = 0;
                            }
                        }

                        DT_EVO.Rows.Add(DR_NM_COLABORADOR01);
                        DT_EVO.Rows.Add(DR_NM_COLABORADOR02);
                        DT_EVO.Rows.Add(DR_NM_COLABORADOR03);
                        DT_EVO.Rows.Add(DR_NM_COLABORADOR04);
                        DT_EVO.Rows.Add(DR_NM_COLABORADOR05);
                    }

                    DataSet dsFinal = new DataSet();
                    dsFinal.Tables.Add(DT_MES);
                    dsFinal.Tables.Add(DT_EVO);
                    return dsFinal;
                }
                return new DataSet();
            }
            catch (Exception ex)
            {
                throw new Exception("RET.Tabulacao.ADO_001: " + ex.Message, ex);
            }
        }

        private string[] VeriificaIndicadorImg(Decimal IND_PAR, Decimal IND_OLD, Decimal IND_NEW)
        {
            string[] retorno = new string[] { "", "" };

            if (IND_OLD > 0)
            {
                if (IND_NEW > IND_PAR)
                {
                    if (IND_NEW > IND_OLD)
                    {
                        retorno[0] = "/Image/EVO001.png";
                        retorno[1] = "Indicador em uma crescente acima da meta";
                    }
                    else
                    {
                        retorno[0] = "/Image/EVO005.png";
                        retorno[1] = "Indicador em uma decrescente acima da meta";
                    }
                }
                else
                {
                    if (IND_NEW > IND_OLD)
                    {
                        retorno[0] = "/Image/EVO006.png";
                        retorno[1] = "Indicador em uma crescente abaixo da meta";
                    }
                    else
                    {
                        retorno[0] = "/Image/EVO004.png";
                        retorno[1] = "Indicador em uma decrescente abaixo da meta";
                    }
                }
            }
            else
            {
                if (IND_NEW > IND_PAR)
                {
                    retorno[0] = "/Image/EVO002.png";
                    retorno[1] = "Início da apuração indicador acima da meta";
                }
                else
                {
                    retorno[0] = "/Image/EVO003.png";
                    retorno[1] = "Início da apuração indicador abaixo da meta";
                }
            }
            return retorno;
        }
    }

    public class EvolucaoGriViewTemplate : ITemplate
    {
        private DataControlRowType templateType;
        private string columnName;
        private string TipoRelatorio;

        public EvolucaoGriViewTemplate(DataControlRowType type, string colname, string Tipo)
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
                        if (columnName.Contains("_B"))
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
                default: break;
            }
        }
        private void NovaColunaLbl_DataBinding(Object sender, EventArgs e)
        {
            Label l = (Label)sender;
            GridViewRow row = (GridViewRow)l.NamingContainer;

            l.Text = string.Format("{0:N0}", DataBinder.Eval(row.DataItem, columnName).ToString());
        }

        private void NovaColunaImg_DataBinding(Object sender, EventArgs e)
        {
            Image img = (Image)sender;
            GridViewRow row = (GridViewRow)img.NamingContainer;

            img.ID = columnName;
            img.ImageUrl = DataBinder.Eval(row.DataItem, columnName).ToString();
            img.ToolTip = DataBinder.Eval(row.DataItem, columnName.Replace("_B", "_C")).ToString();
        }
    }
}