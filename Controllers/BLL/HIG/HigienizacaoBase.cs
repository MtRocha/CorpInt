using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Intranet_NEW.Controllers.DAL;

namespace Intranet.BLL.HIG
{
    public class HigienizacaoBase
    {
        public string GeraArquivoParaHigienizar(string TP_IMPORTACAO)
        {
            try
            {
                SqlCommand sqlcommand = new SqlCommand();
                sqlcommand.CommandType = CommandType.StoredProcedure;
                sqlcommand.CommandText = "SP_HIG_LISTA_CPF_CONTATO_INVALIDO";
                sqlcommand.Parameters.AddWithValue("@TP_IMPORTACAO", TP_IMPORTACAO);

                DAL_PROC AcessaDadosProc = new Intranet.DAL.DAL_PROC();

                DataSet ds = AcessaDadosProc.ConsultaSQL(sqlcommand);

                if ((ds.Tables.Count > 0) && (ds.Tables[0].Rows.Count > 0))
                {
                    string FileName = string.Format(@"C:\Dados\Planejamento\Demandas\99.Higienizacao\HG_EV_{0}.txt", DateTime.Now.ToString("yyyyMMdd"));
                    StreamWriter Escrita = new StreamWriter(FileName);
                    foreach (DataRow dr in ds.Tables[0].Rows)
                        Escrita.WriteLine(dr[0].ToString());

                    Escrita.Close();
                    Escrita.Dispose();

                    return (FileName);
                }
                return "";
            }
            catch (Exception ex)
            {
                throw new Exception("HIG.HigienizacaoBase_001: " + ex.Message, ex);
            }
        }
    }
}
