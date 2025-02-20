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
    public class Atestado
    {
        DAL_Acesso AcessaAtestado = new Intranet.DAL.DAL_Acesso();
        DAL_MIS AcessaBancoMIS = new DAL.DAL_MIS();

        public DataSet AtualizaGV(Nullable<DateTime> DT1, Nullable<DateTime> DT2, int COORD, int SUPER, int OPE, int TIPO)
        {
            try
            {
                SqlCommand sqlcommand = new SqlCommand();
                sqlcommand.CommandType = CommandType.StoredProcedure;
                sqlcommand.CommandText = "SP_WEB_RELATORIO_ATESTADO";

                sqlcommand.Parameters.AddWithValue("@DT1", DT1);
                sqlcommand.Parameters.AddWithValue("@DT2", DT2);
                sqlcommand.Parameters.AddWithValue("@NR_COORD", COORD);
                sqlcommand.Parameters.AddWithValue("@NR_SUPER", SUPER);
                sqlcommand.Parameters.AddWithValue("@NR_OPE", OPE);
                sqlcommand.Parameters.AddWithValue("@TIPO", TIPO);

                //sqlcommand.Parameters.AddWithValue("@DT1", DT1.ToString("yyyyMMdd"));

                DataSet dsAtestado = AcessaBancoMIS.ConsultaSQL(sqlcommand);

                return dsAtestado;
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
