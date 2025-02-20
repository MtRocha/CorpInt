using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace Intranet.DAL
{
    public class DAL_PROC
    {
        string strConexao;
        SqlConnection SQLConexao;
        static DAL_PROC instancia;

        public DAL_PROC()
        {
            /* BASE DESENVOLVIMENTO */
            strConexao = "Server=172.20.1.248; Database=DB_PROC; User Id=SisIntranet; Password=_P@ssw0rdEX!T0; Max Pool Size=600";
            SQLConexao = new SqlConnection(strConexao);
        }

        public static DAL_PROC Instancia
        {
            get
            {
                if (instancia == null)
                    instancia = new DAL_PROC();
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

        public int ExecutaComandoSQL(SqlCommand sqlCommand)
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
                throw new Exception("DAL_PROC_001: " + ex.Message, ex);
            }
        }

        public DataSet ConsultaSQL(SqlCommand sqlCommand)
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
                throw new Exception("DAL_PROC_002: " + ex.Message, ex);
            }
        }

        public int ExecutaBulkCopySQL(DataTable TabelaCarga, string TabelaDestino)
        {
            ConectaDataBase();
            try
            {
                SqlBulkCopy sqlCopy = new SqlBulkCopy(SQLConexao);
                sqlCopy.BulkCopyTimeout = 0;
                sqlCopy.DestinationTableName = TabelaDestino;
                sqlCopy.WriteToServer(TabelaCarga);

                DesconectaDataBase();
                return (1);
            }
            catch (Exception ex)
            {
                DesconectaDataBase();
                throw new Exception("DAL_PROC_003: " + ex.Message, ex);
            }
        }
        public int ExecutaBulkCopySQL(DataRow[] TabelaCarga, string TabelaDestino)
        {
            ConectaDataBase();
            try
            {
                SqlBulkCopy sqlCopy = new SqlBulkCopy(SQLConexao);
                sqlCopy.BulkCopyTimeout = 0;
                sqlCopy.DestinationTableName = TabelaDestino;
                sqlCopy.WriteToServer(TabelaCarga);

                DesconectaDataBase();
                return (1);
            }
            catch (Exception ex)
            {
                DesconectaDataBase();
                throw new Exception("DAL_PROC_004: " + ex.Message, ex);
            }
        }
    }
}
