using Intranet_NEW.Controllers.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Intranet.BLL.RET.Tabulacao
{
    public class ADO : Filtro
    {
        public DataSet GeraRelatorioHoraHoraPorHora(string DT_INI, string DT_FIM,string HR_INI,string HR_FIM,string NR_COORDENADOR, string NR_SUPERVISOR, string NR_OPERADOR, string TP_RELATORIO)
        {
            try
            {
                SqlCommand sqlcommand = new SqlCommand();
                sqlcommand.CommandType = CommandType.StoredProcedure;
                sqlcommand.CommandText = "SP_RET_RELATORIO_HORA_HORA_OPERADOR_POR_HORA";
                sqlcommand.Parameters.AddWithValue("@DT_INI", DT_INI);
                sqlcommand.Parameters.AddWithValue("@DT_FIM", DT_FIM);
                sqlcommand.Parameters.AddWithValue("@HR_INI", HR_INI);
                sqlcommand.Parameters.AddWithValue("@HR_FIM", HR_FIM);
                sqlcommand.Parameters.AddWithValue("@NR_COORDENADOR", NR_COORDENADOR);
                sqlcommand.Parameters.AddWithValue("@NR_SUPERVISOR", NR_SUPERVISOR);
                sqlcommand.Parameters.AddWithValue("@NR_OPERADOR", NR_OPERADOR);
                sqlcommand.Parameters.AddWithValue("@TP_RELATORIO", TP_RELATORIO);

                DAL_MIS AcessaDadosMisN = new Intranet.DAL.DAL_MIS();
                DataSet dsOperador = AcessaDadosMisN.ConsultaSQL(sqlcommand);
                return dsOperador;
            }
            catch (Exception ex)
            {
                throw new Exception("RET.Tabulacao.ADO_001: " + ex.Message, ex);
            }
        }

        public DataSet GeraRelatorioHoraHora(string DT_INI, string DT_FIM,string NR_COORDENADOR, string NR_SUPERVISOR, string NR_OPERADOR, string TP_RELATORIO)
        {
            try
            {
                SqlCommand sqlcommand = new SqlCommand();
                sqlcommand.CommandType = CommandType.StoredProcedure;
                sqlcommand.CommandText = "SP_RET_RELATORIO_HORA_HORA_OPERADOR";
                sqlcommand.Parameters.AddWithValue("@DT_INI", DT_INI);
                sqlcommand.Parameters.AddWithValue("@DT_FIM", DT_FIM);
                sqlcommand.Parameters.AddWithValue("@NR_COORDENADOR", NR_COORDENADOR);
                sqlcommand.Parameters.AddWithValue("@NR_SUPERVISOR", NR_SUPERVISOR);
                sqlcommand.Parameters.AddWithValue("@NR_OPERADOR", NR_OPERADOR);
                sqlcommand.Parameters.AddWithValue("@TP_RELATORIO", TP_RELATORIO);

                DAL_MIS AcessaDadosMisN = new Intranet.DAL.DAL_MIS();
                DataSet dsOperador = AcessaDadosMisN.ConsultaSQL(sqlcommand);
                return dsOperador;
            }
            catch (Exception ex)
            {
                throw new Exception("RET.Tabulacao.ADO_001: " + ex.Message, ex);
            }
        }

        public DataSet GeraRelatorioHoraHoraOperador(string DT_INI, string DT_FIM, string NR_OPERADOR)
        {
            try
            {
                SqlCommand sqlcommand = new SqlCommand();
                sqlcommand.CommandType = CommandType.StoredProcedure;
                sqlcommand.CommandText = "SP_RET_RELATORIO_HORA_HORA_OPERADOR_RESUMO";
                sqlcommand.Parameters.AddWithValue("@DT_INI", DT_INI);
                sqlcommand.Parameters.AddWithValue("@DT_FIM", DT_FIM);
                sqlcommand.Parameters.AddWithValue("@NR_OPERADOR", NR_OPERADOR);

                DAL_MIS AcessaDadosMisN = new Intranet.DAL.DAL_MIS();
                DataSet dsOperador = AcessaDadosMisN.ConsultaSQL(sqlcommand);
                return dsOperador;
            }
            catch (Exception ex)
            {
                throw new Exception("RET.Tabulacao.ADO_001: " + ex.Message, ex);
            }
        }

        public DataSet GeraRelatorioHoraHoraScore(string DT_INI, string DT_FIM, int NR_COORDENADOR)
        {
            try
            {
                SqlCommand sqlcommand = new SqlCommand();
                sqlcommand.CommandType = CommandType.StoredProcedure;
                sqlcommand.CommandText = "SP_RET_RELATORIO_HORA_HORA_SCORE";
                sqlcommand.Parameters.AddWithValue("@DT_INI", DT_INI);
                sqlcommand.Parameters.AddWithValue("@DT_FIM", DT_FIM);

                sqlcommand.Parameters.AddWithValue("@NR_COORDENADOR", NR_COORDENADOR);
     
                DAL_MIS AcessaDadosMisN = new Intranet.DAL.DAL_MIS();
                DataSet dsOperador = AcessaDadosMisN.ConsultaSQL(sqlcommand);
                return dsOperador;
            }
            catch (Exception ex)
            {
                throw new Exception("RET.Tabulacao.ADO_001: " + ex.Message, ex);
            }
        }




        public DataSet GeraRelatorioHoraHoraQuartil(string DT_INI, string DT_FIM, string NR_COORDENADOR, string NR_SUPERVISOR, int ORDERBY, string ORDER)
        {
            try
            {
                SqlCommand sqlcommand = new SqlCommand();
                sqlcommand.CommandType = CommandType.StoredProcedure;
                sqlcommand.CommandText = "SP_RET_RELATORIO_HORA_HORA_QUARTIL_OPERADOR";
                sqlcommand.Parameters.AddWithValue("@DT_INI", DT_INI);
                sqlcommand.Parameters.AddWithValue("@DT_FIM", DT_FIM);

                sqlcommand.Parameters.AddWithValue("@NR_COORDENADOR", NR_COORDENADOR);
                sqlcommand.Parameters.AddWithValue("@NR_SUPERVISOR", NR_SUPERVISOR);
                sqlcommand.Parameters.AddWithValue("@ORDERBY", ORDERBY);
                sqlcommand.Parameters.AddWithValue("@ORDER", ORDER);

                DAL_MIS AcessaDadosMisN = new Intranet.DAL.DAL_MIS();
                DataSet dsOperador = AcessaDadosMisN.ConsultaSQL(sqlcommand);
                return dsOperador;
            }
            catch (Exception ex)
            {
                throw new Exception("RET.Tabulacao.ADO_001: " + ex.Message, ex);
            }
        }

        public DataSet GeraPausaOperador(int NR_OPERADOR)
        {
            try
            {
                SqlCommand sqlcommand = new SqlCommand();
                sqlcommand.CommandType = CommandType.StoredProcedure;
                sqlcommand.CommandText = "SP_RET_RELATORIO_PAUSA_OPERADOR";
                 sqlcommand.Parameters.AddWithValue("@NR_OPERADOR", NR_OPERADOR);

                DAL_MIS AcessaDadosMisN = new Intranet.DAL.DAL_MIS();
                DataSet dsOperador = AcessaDadosMisN.ConsultaSQL(sqlcommand);
                return dsOperador;
            }
            catch (Exception ex)
            {
                throw new Exception("RET.OperadorPAUSA.ADO_001: " + ex.Message, ex);
            }
        }
    }
}