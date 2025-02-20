using Intranet_NEW.Controllers.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;

namespace Intranet.BLL.RET
{
    public class HoraHora
    {

        public DataSet OLOS()
        {
            try
            {
                SqlCommand sqlcommand = new SqlCommand();
                sqlcommand.CommandType = CommandType.Text;
                sqlcommand.CommandText = " SELECT MAX(LastLogout) as DATAHORA FROM ExportDataV3..AGENTCD WITH(NOLOCK)";

                DAL_OLOS AcessaDadosOlos = new Intranet.DAL.DAL_OLOS();

                DataSet ds = AcessaDadosOlos.ConsultaSQL(sqlcommand);

                return ds;
            }
            catch (Exception ex)
            {
                throw new Exception("RET.CmdTabulacao_001: " + ex.Message, ex);
            }
        }

        public DataSet WEDOO()
        {
            DateTime a = DateTime.Now;
            string ANO = Convert.ToString(a.Year);
            string MES = Convert.ToString(a.Month);
            if (Convert.ToInt32(MES) < 10)
            {
                MES = "0" + MES;
            }
            try
            {
                SqlCommand sqlcommand = new SqlCommand();
                sqlcommand.CommandType = CommandType.Text;
                sqlcommand.CommandText = " SELECT MAX(DT_ACAO) AS DATAHORA \n"
                    + " FROM DB_PROC..TBL_" + ANO + MES + "_RET_ACIONAMENTO_HORA_TABULACAO WITH(NOLOCK)";

                DAL_PROC AcessaDadosProc = new Intranet.DAL.DAL_PROC();

                DataSet ds = AcessaDadosProc.ConsultaSQL(sqlcommand);

                return ds;
            }
            catch (Exception ex)
            {
                throw new Exception("RET.CmdTabulacao_001: " + ex.Message, ex);
            }
        }

        public DataSet GeraRelatorioHoraHoraTabulacao(DateTime DT_REFERENCIA, String NR_CARTEIRA)
        {
            try
            {
                SqlCommand sqlcommand = new SqlCommand();
                sqlcommand.CommandType = CommandType.StoredProcedure;
                sqlcommand.CommandText = "SP_RET_RELATORIO_HORA_HORA_TABULACAO";
                sqlcommand.Parameters.AddWithValue("@DT_REFERENCIA", DT_REFERENCIA.ToString("yyyyMMdd"));
                sqlcommand.Parameters.AddWithValue("@NR_CARTEIRA", NR_CARTEIRA);

                DAL_MIS AcessaDadosMis = new Intranet.DAL.DAL_MIS();

                DataSet ds = AcessaDadosMis.ConsultaSQL(sqlcommand);
                return ds;
            }
            catch (Exception ex)
            {
                throw new Exception("RET.CmdTabulacao_001: " + ex.Message, ex);
            }
        }

        public DataSet GeraRelatorioHoraHoraLigacao(DateTime DT_REFERENCIA, String NR_CARTEIRA)
        {
            //try
            //{
                SqlCommand sqlcommand = new SqlCommand();
                sqlcommand.CommandType = CommandType.StoredProcedure;
                sqlcommand.CommandText = "SP_RET_RELATORIO_HORA_HORA_LIGACAO_NEW";
                sqlcommand.Parameters.AddWithValue("@DT_REFERENCIA", DT_REFERENCIA.ToString("yyyyMMdd"));
                sqlcommand.Parameters.AddWithValue("@NR_CARTEIRA", NR_CARTEIRA);

            DAL_MIS AcessaDadosMis = new Intranet.DAL.DAL_MIS();

                DataSet ds = AcessaDadosMis.ConsultaSQL(sqlcommand);
                return ds;
            }
            //catch {
                
            //}
            //{
                //throw new Exception("RET.CmdLigacao_001: " + ex.Message, ex);
            //}
        //}

        public DataSet GeraRelatorioHoraHoraTabulacaoCamp(DateTime DT_REFERENCIA, string TP_CARTEIRA)
        {
            try
            {
                SqlCommand sqlcommand = new SqlCommand();
                sqlcommand.CommandType = CommandType.StoredProcedure;
                sqlcommand.CommandText = "SP_RET_GERA_RELATORIO_HORA_HORA_CAMPANHA";
                sqlcommand.Parameters.AddWithValue("@DT_REFERENCIA", DT_REFERENCIA.ToString("yyyy-MM-dd HH:mm:ss"));
                sqlcommand.Parameters.AddWithValue("@CAMPANHA", TP_CARTEIRA);

                DAL_PROC AcessaDadosProc = new Intranet.DAL.DAL_PROC();

                DataSet ds = AcessaDadosProc.ConsultaSQL(sqlcommand);
                return ds;
            }
            catch (Exception ex)
            {
                throw new Exception("RET.CmdTabulacao_001: " + ex.Message, ex);
            }
        }


        public DataSet Relatorio_Incorporacao(int DT_INIC, int DT_FIM)
        {
            try
            {
                SqlCommand sqlcommand = new SqlCommand();
                sqlcommand.CommandType = CommandType.StoredProcedure;
                sqlcommand.CommandText = "SP_WEB_INCORPORACAO";
                sqlcommand.Parameters.AddWithValue("@DT_INIC", DT_INIC);
                sqlcommand.Parameters.AddWithValue("@DT_FIM", DT_FIM);
                 DAL_PROC AcessaDadosProc = new Intranet.DAL.DAL_PROC();

                DataSet ds = AcessaDadosProc.ConsultaSQL(sqlcommand);
                return ds;
            }
            catch (Exception ex)
            {
                throw new Exception("RET.RelatorioIncorporacao_001: " + ex.Message, ex);
            }
        }

        public DataSet Relatorio_Incorporacao_Analitico(int DT_INIC, int DT_FIM)
        {
            try
            {
                SqlCommand sqlcommand = new SqlCommand();
                sqlcommand.CommandType = CommandType.StoredProcedure;
                sqlcommand.CommandText = "SP_WEB_INCORPORACAO_ANALITICO";
                sqlcommand.Parameters.AddWithValue("@DT_INIC", DT_INIC);
                sqlcommand.Parameters.AddWithValue("@DT_FIM", DT_FIM);
                DAL_PROC AcessaDadosProc = new Intranet.DAL.DAL_PROC();

                DataSet ds = AcessaDadosProc.ConsultaSQL(sqlcommand);
                return ds;
            }
            catch (Exception ex)
            {
                throw new Exception("RET.RelatorioIncorporacao_001: " + ex.Message, ex);
            }
        }

        public DataSet Relatorio_Resumo(int Data)
        {
            try
            {
                SqlCommand sqlcommand = new SqlCommand();
                sqlcommand.CommandType = CommandType.StoredProcedure;
                sqlcommand.CommandText = "SP_PAINEL_HOME_RESUMO";
                sqlcommand.Parameters.AddWithValue("@DT_REFERENCIA", Data);
                DAL_PROC AcessaDadosProc = new Intranet.DAL.DAL_PROC();

                DataSet ds = AcessaDadosProc.ConsultaSQL(sqlcommand);
                return ds;
            }
            catch (Exception ex)
            {
                throw new Exception("RET.Relatorioresumo_001: " + ex.Message, ex);
            }
        }

        public DataTable Operadores_Online()
        {
            try
            {
                SqlCommand sqlcommand = new SqlCommand();
                sqlcommand.CommandType = CommandType.StoredProcedure;
                sqlcommand.CommandText = "SP_PAINEL_HOMEOFFICE_LOGADO";
                DAL_MIS AcessaDadosProc = new Intranet.DAL.DAL_MIS();

                DataTable dt = AcessaDadosProc.ConsultaSQL(sqlcommand).Tables[0];
                return dt;
            }
            catch (Exception ex)
            {
                throw new Exception("RET.Relatorioresumo_002: " + ex.Message, ex);
            }
        }

        public DataSet Relatorio_Resumo_Carteira(int Data)
        {
            try
            {
                SqlCommand sqlcommand = new SqlCommand();
                sqlcommand.CommandType = CommandType.StoredProcedure;
                sqlcommand.CommandText = "SP_PAINEL_HOME_RESUMO_CARTEIRA";
                sqlcommand.Parameters.AddWithValue("@DT_REFERENCIA", Data);
                DAL_PROC AcessaDadosProc = new Intranet.DAL.DAL_PROC();

                DataSet ds = AcessaDadosProc.ConsultaSQL(sqlcommand);
                return ds;
            }
            catch (Exception ex)
            {
                throw new Exception("RET.Relatorioresumo_003: " + ex.Message, ex);
            }
        }

        public DataTable TempoLoginLogout(int Data)
        {
            try
            {
                SqlCommand sqlcommand = new SqlCommand();
                sqlcommand.CommandType = CommandType.StoredProcedure;
                sqlcommand.CommandText = "SP_HORA_HORA_TEMPO_LOGIN";
                sqlcommand.Parameters.AddWithValue("@DT_REFERENCIA", Data);
                DAL_OLOS AcessaDadosOlos = new Intranet.DAL.DAL_OLOS();

                DataSet ds = AcessaDadosOlos.ConsultaSQL(sqlcommand);

                return ds.Tables[0];
            }
            catch (Exception ex)
            {
                throw new Exception("RET.Relatorioresumo_005: " + ex.Message, ex);
            }
        }

        public DataTable TempoLoginLogoutProjetado()
        {
            try
            {
                SqlCommand sqlcommand = new SqlCommand();
                sqlcommand.CommandType = CommandType.StoredProcedure;
                sqlcommand.CommandText = "SP_PROJECAO_PA_DIA";
                DAL_OLOS AcessaDadosOlos = new Intranet.DAL.DAL_OLOS();

                DataSet ds = AcessaDadosOlos.ConsultaSQL(sqlcommand);

                return ds.Tables[0];
            }
            catch (Exception ex)
            {
                throw new Exception("RET.Relatorioresumo_010: " + ex.Message, ex);
            }
        }


        public DataTable OperadoresLogado(int Data)
        {

            string ANO = Data.ToString().Substring(0, 4);
            string MES = Data.ToString().Substring(4, 2);

            if (MES.Length < 2)
            {
                MES = "0" + MES;
            }


            string Mes_Atual = DateTime.Now.Month.ToString();
            if (Mes_Atual.Length < 2)
            {
                Mes_Atual = "0" + Mes_Atual;
            }

            try
            {
                string DataBase = "DB_PROC";

                if (MES != Mes_Atual) DataBase = "DB_CAIXA_" + ANO + MES;

                SqlCommand sqlcommand = new SqlCommand();
                sqlcommand.CommandType = CommandType.Text;
                sqlcommand.CommandText = " SELECT COUNT(DISTINCT NM_LOGIN) AS QTDE \n"
                    + " FROM " + DataBase + "..TBL_" + ANO + MES + "_RET_ACIONAMENTO_HORA_TABULACAO WITH(NOLOCK) \n"
                    + " WHERE CONVERT(VARCHAR(8),DT_ACAO,112) = " + Data + " AND NM_LOGIN NOT LIKE '%BACKOFFICE%' AND NM_LOGIN NOT IN ('WHATSBOT', 'MIDIA')";

                DAL_PROC AcessaDadosProc = new Intranet.DAL.DAL_PROC();

                DataTable ds = AcessaDadosProc.ConsultaSQL(sqlcommand).Tables[0];

                return ds;
            }
            catch (Exception ex)
            {
                throw new Exception("RET.CmdTabulacao_001: " + ex.Message, ex);
            }
        }
    }
}