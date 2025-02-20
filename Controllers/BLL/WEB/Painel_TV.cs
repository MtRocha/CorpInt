using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;
using Intranet_NEW.Controllers.DAL;

namespace Intranet.BLL.WEB
{
    public class Painel_TV
    {
        DAL_MIS AcessaBancoMIS = new DAL.DAL_MIS();
        DAL_OLOS AcessaBancoOlos = new DAL.DAL_OLOS();

        public DataSet ResultadoCPC()
        {
            try
            {
                SqlCommand sqlcommand = new SqlCommand();
                sqlcommand.CommandType = CommandType.Text;
                sqlcommand.CommandText = "; with cte_dados as \n"
                                    + "(select row_number() over(order by DT_ACAO) as NUMERADOR, DT_ACAO, SUM(QTDE_CPC)AS QTDE_CPC from DB_PROC..TB_WEB_HISTORICO_CPC WHERE MES_REF = CONVERT(char(6), GETDATE(), 112) GROUP BY DT_ACAO) \n"

                                    + "select CAST(UPPER(a.dt_acao) AS DATETIME) AS DT_ACAO, a.QTDE_CPC , sum(iif(a.numerador >= b.numerador, b.qtde_cpc, 0)) as acumulado \n"
                                    + "from cte_dados a, \n"
                                    + " cte_dados b \n"
                                    + " group by CAST(UPPER(a.dt_acao) AS DATETIME), a.QTDE_CPC ORDER BY 1 \n"
                                    + " Select Sum(qtde_cpc) as total from DB_PROC..TB_WEB_HISTORICO_CPC WHERE MES_REF = CONVERT(char(6), GETDATE(), 112)";
                DAL_MIS AcessaDadosMis = new DAL.DAL_MIS();
                return AcessaDadosMis.ConsultaSQL(sqlcommand);
            }
            catch (Exception ex)
            {
                throw new Exception("BLL.WEB.CPC_001: " + ex.Message, ex);
            }
        }

        public DataSet ResultadoCarteira()
        {
            try
            {
                DateTime dtAtual = DateTime.Now;
                Intranet.BLL.CAR.Mapeamento Carteira = new BLL.CAR.Mapeamento();
               
                SqlCommand sqlcommand = new SqlCommand();
                sqlcommand.CommandType = CommandType.StoredProcedure;
                sqlcommand.CommandText = "SP_REV_GERA_RESUMO_STATUS_NEW";
                sqlcommand.Parameters.AddWithValue("@DT_REFERENCIA", dtAtual);
                sqlcommand.Parameters.AddWithValue("@TP_ARQUIVO", 99);
                sqlcommand.Parameters.AddWithValue("@TP_PRODUTO", "T");
                sqlcommand.Parameters.AddWithValue("@TP_CAMPANHA", "GERAL");
                sqlcommand.Parameters.AddWithValue("@CD_ACAO", 0);

                DAL_MIS AcessaDadosCaixa = new Intranet.DAL.DAL_MIS();

                DataSet dsCarteira = AcessaDadosCaixa.ConsultaSQL(sqlcommand);

                return dsCarteira;

            }
            catch (Exception ex)
            {
                throw new Exception("BLL.WEB.ACIONADO: " + ex.Message, ex);
            }
        }


        public DataSet ResultadoANSLigacao()
        {
            try
            {
                DateTime dtAtual = DateTime.Now;
                Intranet.BLL.CAR.Mapeamento Carteira = new BLL.CAR.Mapeamento();

                SqlCommand sqlcommand = new SqlCommand();
                sqlcommand.CommandType = CommandType.StoredProcedure;
                sqlcommand.CommandText = "SP_RET_LIGACAO_GRAFICO";
                sqlcommand.Parameters.AddWithValue("@DT_REFERENCIA", dtAtual);

                DAL_MIS AcessaDadosCaixa = new Intranet.DAL.DAL_MIS();

                DataSet dsCarteira = AcessaDadosCaixa.ConsultaSQL(sqlcommand);

                return dsCarteira;

            }
            catch (Exception ex)
            {
                throw new Exception("BLL.WEB.ANSLIGACAO: " + ex.Message, ex);
            }
        }

        public DataSet ResultadoANSTabulacao()
        {
            try
            {
                DateTime dtAtual = DateTime.Now;
                Intranet.BLL.CAR.Mapeamento Carteira = new BLL.CAR.Mapeamento();

                SqlCommand sqlcommand = new SqlCommand();
                sqlcommand.CommandType = CommandType.StoredProcedure;
                sqlcommand.CommandText = "SP_RET_TABULACAO_GRAFICO";
                sqlcommand.Parameters.AddWithValue("@DT_REFERENCIA", dtAtual);

                DAL_MIS AcessaDadosCaixa = new Intranet.DAL.DAL_MIS();

                DataSet dsCarteira = AcessaDadosCaixa.ConsultaSQL(sqlcommand);

                return dsCarteira;

            }
            catch (Exception ex)
            {
                throw new Exception("BLL.WEB.ANSTABULACAO: " + ex.Message, ex);
            }
        }

        public DataSet ResultadoLogados()
        {
            try
            {

                SqlCommand sqlcommand = new SqlCommand();
                sqlcommand.CommandType = CommandType.Text;
                sqlcommand.CommandText = "select SUM(IIF(agentStatusId in (2), 1, 0)) as FALANDO \n"
                        + ", SUM(IIF(agentStatusId in (4), 1, 0)) as PAUSA \n"
                        + ", SUM(IIF(agentStatusId in (20), 1, 0)) as TABULANDO \n"
                        + ", SUM(IIF(agentStatusId not in (2, 4, 20), 1, 0)) as LIVRE \n"
                        + ",   COUNT(*) AS LOGADO \n"
                        + "from AgentStatus NOLOCK";
                DAL_MIS AcessaDadosMis = new DAL.DAL_MIS();
                return AcessaBancoOlos.ConsultaSQL(sqlcommand);

            }
            catch (Exception ex)
            {
                throw new Exception("BLL.WEB.LOGADOS: " + ex.Message, ex);
            }
        }

        public string converteDataTabelParaJSON(DataTable dtb)
        {
            try
            {
                //define um array de strings
                string[] jsonArray = new string[dtb.Columns.Count];
                string headString = string.Empty;
                //percorre as colunas
                for (int i = 0; i < dtb.Columns.Count; i++)
                {
                    jsonArray[i] = dtb.Columns[i].Caption; // Array para todas as colunas
                    headString += "'" + jsonArray[i] + "' : '" + jsonArray[i] + i.ToString() + "%" + "',";
                }
                headString = headString.Substring(0, headString.Length - 1);
                //define um stringbuilder
                StringBuilder sb = new StringBuilder();
                sb.Append("[");
                if (dtb.Rows.Count > 0)
                {
                    for (int i = 0; i < dtb.Rows.Count; i++)
                    {
                        string tempString = headString;
                        sb.Append("{");
                        // pega cada valor do  datatable
                        for (int j = 0; j < dtb.Columns.Count; j++)
                        {
                            tempString = tempString.Replace(dtb.Columns[j] + j.ToString() + "%", dtb.Rows[i][j].ToString());
                        }
                        sb.Append(tempString + "},");
                    }
                }
                else
                {
                    string tempString = headString;
                    sb.Append("{");
                    for (int j = 0; j < dtb.Columns.Count; j++)
                    {
                        tempString = tempString.Replace(dtb.Columns[j] + j.ToString() + "%", "-");
                    }
                    sb.Append(tempString + "},");
                }
                sb = new StringBuilder(sb.ToString().Substring(0, sb.ToString().Length - 1));
                sb.Append("]");
                return sb.ToString(); // saida json formatada
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet PainelHome()
        {

            try
            {
                DateTime dtAtual = DateTime.Now;
                string turno = "M";

                if ((dtAtual.Hour == 14 && dtAtual.Minute >= 41) || dtAtual.Hour > 14)
                    turno = "T";

                SqlCommand sqlcommand = new SqlCommand();
                sqlcommand.CommandType = CommandType.StoredProcedure;
                sqlcommand.CommandText = "SP_PAINEL_HOMEOFFICE";
                sqlcommand.Parameters.AddWithValue("@TP_TURNO", turno);

                DAL_MIS AcessaDadosCaixa = new Intranet.DAL.DAL_MIS();

                DataSet dsHome = AcessaDadosCaixa.ConsultaSQL(sqlcommand);

                return dsHome;

            }
            catch (Exception ex)
            {
                throw new Exception("BLL.WEB.HOME: " + ex.Message, ex);
            }

        }
    }


}
