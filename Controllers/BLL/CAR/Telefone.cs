using Intranet_NEW.Controllers.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Intranet.BLL.CAR
{
    public class Telefone
    {
        public DataSet PesquisaTelefone(Int64 CPF, Int64 FONE)
        {
            try
            {
                SqlCommand sqlcommand = new SqlCommand();
                sqlcommand.CommandType = CommandType.StoredProcedure;
                sqlcommand.CommandText = "SP_REM_TEL_PESQUISA";
                sqlcommand.Parameters.AddWithValue("@NR_CPF", CPF);
                sqlcommand.Parameters.AddWithValue("@FONE", FONE);

                DAL_PROC AcessaDadosProc = new Intranet.DAL.DAL_PROC();
                DataSet dsFonePesquisa = AcessaDadosProc.ConsultaSQL(sqlcommand);

                return dsFonePesquisa;
            }
            catch (Exception ex)
            {
                throw new Exception("REM.Telefone001: " + ex.Message, ex);
            }
        }

        public DataSet TelefoneDesabilitado(int DATA_INI, int DATA_FIM, Int64 CPF, Int64 FONE, string TP_MOTIVO)
        {
            try
            {
                SqlCommand sqlcommand = new SqlCommand();
                sqlcommand.CommandType = CommandType.StoredProcedure;
                sqlcommand.CommandText = "SP_REM_TEL_DESABILITADOS";
                sqlcommand.Parameters.AddWithValue("@DT_INIC", DATA_INI);
                sqlcommand.Parameters.AddWithValue("@DT_FIM", DATA_FIM);
                sqlcommand.Parameters.AddWithValue("@TP_INIBICAO", TP_MOTIVO);
                sqlcommand.Parameters.AddWithValue("@NR_CPF", CPF);
                sqlcommand.Parameters.AddWithValue("@FONE", FONE);

                DAL_PROC AcessaDadosProc = new Intranet.DAL.DAL_PROC();
                DataSet dsFonePesquisa = AcessaDadosProc.ConsultaSQL(sqlcommand);

                return dsFonePesquisa;
            }
            catch (Exception ex)
            {
                throw new Exception("REM.Telefone002: " + ex.Message, ex);
            }
        }
    }
}
