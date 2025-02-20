using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace Intranet_NEW.Controllers.DAL
{
    public class DAL_Caixa
    {
        string strConexao;
        SqlConnection SQLConexao;
        static DAL_Caixa instancia;

        public DAL_Caixa()
        {
            /* BASE DESENVOLVIMENTO */
            strConexao = "Server=172.20.1.248; Database=CAIXA; User Id=sa; Password=t!gt3@br@sil; Max Pool Size=600";
            SQLConexao = new SqlConnection(strConexao);
        }

        public static DAL_Caixa Instancia
        {
            get
            {
                if (instancia == null)
                    instancia = new DAL_Caixa();
                return instancia;
            }
        }

        private void ConectaDataBase()
        {
            SQLConexao.Open();
        }

        private void DesconectaDataBase()
        {
            SQLConexao.Close();
        }

        public int ExecutaComandoSQL(SqlCommand sqlCommand, string Rotina)
        {
            int LinhasAfetadas = 0;

            ConectaDataBase();
            SqlTransaction sqltran = SQLConexao.BeginTransaction();

            try
            {
                sqlCommand.Transaction = sqltran;
                sqlCommand.CommandTimeout = 0;
                sqlCommand.Connection = SQLConexao;
                LinhasAfetadas = sqlCommand.ExecuteNonQuery();

                sqltran.Commit();
                DesconectaDataBase();
                return LinhasAfetadas;
            }
            catch (Exception ex)
            {
                sqltran.Rollback();
                DesconectaDataBase();
                //System.Web.HttpContext.Current.Session.Add("erro", ex);
                //System.Web.HttpContext.Current.Response.Redirect("erro.aspx?erro=" + Rotina, true);
                throw new Exception("Erro no procedimento: ExecutaComandoSQL", ex);
            }
        }

        public DataSet ConsultaSQL(SqlCommand sqlCommand, string Rotina)
        {
            DataSet ds = new DataSet();
            ConectaDataBase();
            SqlTransaction sqltran = SQLConexao.BeginTransaction();

            try
            {
                sqlCommand.Connection = SQLConexao;
                sqlCommand.CommandTimeout = 0;
                sqlCommand.Transaction = sqltran;

                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = sqlCommand;
                da.Fill(ds);

                sqltran.Commit();
                DesconectaDataBase();
                return ds;
            }
            catch (Exception ex)
            {
                sqltran.Rollback();
                DesconectaDataBase();
                //System.Web.HttpContext.Current.Session.Add("erro", ex);
                //System.Web.HttpContext.Current.Response.Redirect("erro.aspx?erro=" + Rotina, true);
                throw new Exception("Erro no procedimento: ConsultaSQL", ex);
            }
        }


        public int ExecutaBulkCopySQL(DataTable TabelaCarga, string TabelaDestino, string Rotina)
        {
            ConectaDataBase();
            try
            {
                SqlBulkCopy sqlCopy = new SqlBulkCopy(SQLConexao);
                sqlCopy.BulkCopyTimeout = 0;
                sqlCopy.DestinationTableName = TabelaDestino;
                sqlCopy.WriteToServer(TabelaCarga);

                DesconectaDataBase();
                return 1;
            }
            catch (Exception ex)
            {
                DesconectaDataBase();
                //CamadaDados.MAIL mail = new CamadaDados.MAIL();
                // mail.InformaErro(ex, Rotina);
                throw new Exception("Erro no procedimento: ExecutaBulkCopySQL_1", ex);
            }
        }

        public int ExecutaBulkCopySQL(DataRow[] TabelaCarga, string TabelaDestino, string Rotina)
        {
            ConectaDataBase();
            try
            {
                SqlBulkCopy sqlCopy = new SqlBulkCopy(SQLConexao);
                sqlCopy.BulkCopyTimeout = 0;
                sqlCopy.DestinationTableName = TabelaDestino;
                sqlCopy.WriteToServer(TabelaCarga);

                DesconectaDataBase();
                return 1;
            }
            catch (Exception ex)
            {
                DesconectaDataBase();
                //CamadaDados.MAIL mail = new CamadaDados.MAIL();
                //mail.InformaErro(ex, Rotina);
                throw new Exception("Erro no procedimento: ExecutaBulkCopySQL_2", ex);
            }
        }
    }
}
