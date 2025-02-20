using Intranet_NEW.Controllers.DAL;
using System;
using System.Data;
using System.Data.SqlClient;

namespace Intranet.BLL.WEB
{
    public class Ponto
    {
        DAL_TOTVS Acessa = new Intranet.DAL.DAL_TOTVS();
        DAL_MIS AcessaBancoMIS = new DAL.DAL_MIS();

        public DataSet AtualizaGV_FOLHA(string CPF, DateTime DT1, DateTime DT2, int COORD, int SUPER)
        {
            try
            {
                SqlCommand sqlcommand = new SqlCommand();
                sqlcommand.CommandType = CommandType.StoredProcedure;
                sqlcommand.CommandText = "SP_WEB_ACOMPANHAMENTO_PONTO";
                sqlcommand.Parameters.AddWithValue("@CPF", CPF);
                sqlcommand.Parameters.AddWithValue("@DT1", DT1.ToString("yyyyMMdd"));
                sqlcommand.Parameters.AddWithValue("@DT2", DT2.ToString("yyyyMMdd"));
                sqlcommand.Parameters.AddWithValue("@NR_COORD", COORD);
                sqlcommand.Parameters.AddWithValue("@NR_SUPER", SUPER);

                DataSet dsPonto = Acessa.ConsultaSQL(sqlcommand);

                return dsPonto;
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

    }
}
     