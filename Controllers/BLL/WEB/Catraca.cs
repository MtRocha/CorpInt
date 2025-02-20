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
    public class Catraca
    {
        DAL_Acesso AcessaCatraca = new Intranet.DAL.DAL_Acesso();
        DAL_MIS AcessaBancoMIS = new DAL.DAL_MIS();

        public DataSet AtualizaGV(string CPF, DateTime DT1, DateTime DT2, int COORD, int SUPER, int CAT)
        {
            try
            {
                SqlCommand sqlcommand = new SqlCommand();
                sqlcommand.CommandType = CommandType.StoredProcedure;
                sqlcommand.CommandText = "SP_WEB_ACOMPANHAMENTO_CATRACA";
                sqlcommand.Parameters.AddWithValue("@CPF", CPF);
                sqlcommand.Parameters.AddWithValue("@DT1", DT1.ToString("yyyyMMdd"));
                sqlcommand.Parameters.AddWithValue("@DT2", DT2.ToString("yyyyMMdd"));
                sqlcommand.Parameters.AddWithValue("@COORD", COORD);
                sqlcommand.Parameters.AddWithValue("@SUPER", SUPER);
                sqlcommand.Parameters.AddWithValue("@CAT", CAT);

                DataSet dsCatraca = AcessaCatraca.ConsultaSQL(sqlcommand);

                return dsCatraca;
            }
            catch (Exception ex)
            {
                throw new Exception("RET.CmdFechamento_001: " + ex.Message, ex);
            }
        }

        public DataSet CarregaList(int SC, string FILIAL, int NR_COORD)
        {
            try
            {
                SqlCommand sqlcommand = new SqlCommand();
                sqlcommand.CommandType = CommandType.StoredProcedure;
                sqlcommand.CommandText = "SP_WEB_LISTA_SUPERIORES";
                sqlcommand.Parameters.AddWithValue("@SC", SC);
                sqlcommand.Parameters.AddWithValue("@FILIAL", FILIAL);
                sqlcommand.Parameters.AddWithValue("@NR_COORD", NR_COORD);

                DataSet dsSuperior = AcessaBancoMIS.ConsultaSQL(sqlcommand);

                return dsSuperior;
            }
            catch (Exception ex)
            {
                throw new Exception("RET.CmdFechamento_001: " + ex.Message, ex);
            }
        }

        public DataSet ListaOpe(int NR_SUPERVISOR)
        {
            try
            {
                SqlCommand sqlcommand = new SqlCommand();
                sqlcommand.CommandType = CommandType.Text;

                sqlcommand.Parameters.AddWithValue("@NR_SUPERVISOR", NR_SUPERVISOR);

                sqlcommand.CommandText = "SELECT \n"
                                        + "      a.NR_COLABORADOR \n"
                                        + "    , a.NM_COLABORADOR \n"
                                        + "FROM TBL_WEB_COLABORADOR_DADOS a \n"
                                        + "WHERE \n"
                                        + "(((@NR_SUPERVISOR = 0)  OR (@NR_SUPERVISOR <> 0 AND NR_SUPERVISOR = @NR_SUPERVISOR))) \n"
                                        + "ORDER BY \n"
                                        + "    a.NM_COLABORADOR \n";
                DAL_MIS AcessaDadosMis = new DAL.DAL_MIS();
                return AcessaDadosMis.ConsultaSQL(sqlcommand);
            }
            catch (Exception ex)
            {
                throw new Exception("BLL.WEB.Operador_001: " + ex.Message, ex);
            }
        }
    }
}
     