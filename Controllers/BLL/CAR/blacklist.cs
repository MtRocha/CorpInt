using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Intranet.DTO.WEB;
using Intranet_NEW.Controllers.DAL;

namespace Intranet.BLL.CAR
{
    public class blacklist
    {

        public string NM_EMAIL { get; set; }
        public string NR_CPF { get; set; }
        public string NR_FONE { get; set; }

        public DataSet PesquisarListWedoo(Int64 CPF, Int64 FONE, string EMAIL)
        {
            try
            {
                SqlCommand sqlcommand = new SqlCommand();
                sqlcommand.CommandType = CommandType.StoredProcedure;
                sqlcommand.CommandText = "SP_REM_BLACK_LIST";
                sqlcommand.Parameters.AddWithValue("@NR_CPF", CPF);
                sqlcommand.Parameters.AddWithValue("@FONE", FONE);
                sqlcommand.Parameters.AddWithValue("@EMAIL", EMAIL);

                DAL_PROC AcessaDadosProc = new Intranet.DAL.DAL_PROC();
                DataSet dsBlackList = AcessaDadosProc.ConsultaSQL(sqlcommand);

                return dsBlackList;
            }
            catch (Exception ex)
            {
                throw new Exception("REM.BLACK_LIST001: " + ex.Message, ex);
            }
        }

        public DataSet PesquisarListAtto(Int64 FONE)
        {
            try
            {
                SqlCommand sqlcommand = new SqlCommand();
                sqlcommand.CommandType = CommandType.Text;
                sqlcommand.CommandText = "SELECT * FROM BL_BloqueioTelefones WHERE TEL LIKE '%" + FONE + "%'";

                DAL_Atto AcessaDadosAtto = new Intranet.DAL.DAL_Atto();
                DataSet dsBlackList = AcessaDadosAtto.ConsultaSQL(sqlcommand);

                return dsBlackList;
            }
            catch (Exception ex)
            {
                throw new Exception("REM.BLACK_LISTATTO001: " + ex.Message, ex);
            }
        }

        public int InclusaoWedoo(Int64 NR_FONE, String NM_EMAIL, Int64 NR_CPF, int NR_COLABORADOR, DateTime DT_EXPIRACAO)
        {
            try
            {
                SqlCommand sqlcommand = new SqlCommand();
                sqlcommand.CommandType = CommandType.Text;

                sqlcommand.Parameters.AddWithValue("@NR_FONE", NR_FONE);
                sqlcommand.Parameters.AddWithValue("@NM_EMAIL", NM_EMAIL);
                sqlcommand.Parameters.AddWithValue("@NR_CPF", NR_CPF);
                sqlcommand.Parameters.AddWithValue("@NR_COLABORADOR", NR_COLABORADOR);
                sqlcommand.Parameters.AddWithValue("@DT_EXPIRACAO", DT_EXPIRACAO);

                sqlcommand.CommandText = "INSERT INTO TBL_WEB_BLACKLIST \n"
                                        + "        (DT_INCLUSAO, NR_FONE, NM_EMAIL, NR_CPF, NR_COLABORADOR_INC, STATUS, DT_EXPIRACAO) \n"
                                        + " VALUES (GETDATE(), IIF(@NR_FONE = 0, NULL, @NR_FONE), @NM_EMAIL,  IIF(@NR_CPF = 0, NULL, @NR_CPF), @NR_COLABORADOR, 'A', @DT_EXPIRACAO)  \n";

                DAL_MIS AcessaDadosMis = new DAL.DAL_MIS();
                return AcessaDadosMis.ExecutaComandoSQL(sqlcommand);
            }
            catch (Exception ex)
            {
                throw new Exception("REM.BLACK_LIST001: " + ex.Message, ex);

            }

        }

        public int InclusaoAtto(Int64 NR_FONE)
        {
            try
            {
                SqlCommand sqlcommand = new SqlCommand();
                sqlcommand.CommandType = CommandType.Text;

                sqlcommand.Parameters.AddWithValue("@NR_FONE", NR_FONE);
                sqlcommand.CommandText = "INSERT INTO BL_BloqueioTelefones \n"
                                        + "        (TEL) \n"
                                        + " VALUES (IIF(@NR_FONE = 0, NULL, @NR_FONE)) \n";

                DAL_Atto AcessaDadosAtto = new DAL.DAL_Atto();
                return AcessaDadosAtto.ExecutaComandoSQL(sqlcommand);
            }
            catch (Exception ex)
            {
                throw new Exception("REM.BLACK_LISTATTO001: " + ex.Message, ex);

            }

        }


    }
}



