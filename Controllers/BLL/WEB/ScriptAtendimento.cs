using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.OleDb;
using Intranet_NEW.Controllers.DAL;

namespace Intranet.BLL.WEB
{
    public class ScriptAtendimento
    {
        public DataTable ScriptAtendimentoTitulo()
        {
            try
            {
                SqlCommand sqlcommand = new SqlCommand();
                sqlcommand.CommandType = CommandType.Text;
                sqlcommand.CommandText = "SELECT DS_SCRIPT, CONCAT(NR_SCRIPT, '-', NR_SLIDE) AS [TP_SCRIPT] FROM TBL_WEB_SCRIPT_ATENDIMENTO_TIT ORDER BY NR_SCRIPT";

                DAL_MIS AcessaDadosMis = new DAL.DAL_MIS();
                return AcessaDadosMis.ConsultaSQL(sqlcommand).Tables[0];
            }
            catch (Exception ex)
            {
                throw new Exception("BLL.WEB.ScriptAtendimento_001: " + ex.Message, ex);
            }
        }

        public DataTable ScriptAtendimentoSubTitulo(string NR_SCRIPT)
        {
            try
            {
                SqlCommand sqlcommand = new SqlCommand();
                sqlcommand.CommandType = CommandType.Text;
                sqlcommand.Parameters.AddWithValue("@NR_SCRIPT", NR_SCRIPT);
                sqlcommand.CommandText = "SELECT DS_SUBTIT, CONCAT(NR_SUBTIT, '-', NR_SLIDE) AS [TP_SUBTIT] FROM TBL_WEB_SCRIPT_ATENDIMENTO_SUB WHERE NR_SCRIPT = @NR_SCRIPT ORDER BY NR_SUBTIT";

                DAL_MIS AcessaDadosMis = new DAL.DAL_MIS();
                return AcessaDadosMis.ConsultaSQL(sqlcommand).Tables[0];
            }
            catch (Exception ex)
            {
                throw new Exception("BLL.WEB.ScriptAtendimento_002: " + ex.Message, ex);
            }
        }

        public DataTable PesquisaScriptAtendimento(string DS_CHAVE)
        {
            try
            {
                SqlCommand sqlcommand = new SqlCommand();
                sqlcommand.CommandType = CommandType.Text;
                sqlcommand.Parameters.AddWithValue("@DS_CHAVE", string.Format("%{0}%", DS_CHAVE));
                sqlcommand.CommandText = "SELECT NR_SCRIPT, DS_SCRIPT FROM TBL_WEB_SCRIPT_ATENDIMENTO WHERE DS_CHAVE LIKE @DS_CHAVE";

                DAL_MIS AcessaDadosMis = new DAL.DAL_MIS();
                return AcessaDadosMis.ConsultaSQL(sqlcommand).Tables[0];
            }
            catch (Exception ex)
            {
                throw new Exception("BLL.WEB.ScriptAtendimento_001: " + ex.Message, ex);
            }
        }

        public string SelecionaScriptAtendimento(string NR_SCRIPT)
        {
            try
            {
                string retorno = "";
                SqlCommand sqlcommand = new SqlCommand();
                sqlcommand.CommandType = CommandType.Text;
                sqlcommand.Parameters.AddWithValue("@NR_SCRIPT", NR_SCRIPT);
                sqlcommand.CommandText = "SELECT NR_SLIDE FROM TBL_WEB_SCRIPT_ATENDIMENTO WHERE NR_SCRIPT = @NR_SCRIPT";

                DAL_MIS AcessaDadosMis = new DAL.DAL_MIS();
                DataSet ds = AcessaDadosMis.ConsultaSQL(sqlcommand);
                if ((ds.Tables.Count > 0) && (ds.Tables[0].Rows.Count > 0))
                    retorno = ds.Tables[0].Rows[0]["NR_SLIDE"].ToString();
                return retorno;
            }
            catch (Exception ex)
            {
                throw new Exception("BLL.WEB.ScriptAtendimento_002: " + ex.Message, ex);
            }
        }
        public DataTable ListaScriptAtendimento()
        {
            try
            {
                SqlCommand sqlcommand = new SqlCommand();
                sqlcommand.CommandType = CommandType.Text;
                sqlcommand.CommandText = "SELECT \n"
                                        + "	  NR_SCRIPT, 0 AS [NR_SUB], DS_SCRIPT, NR_SLIDE \n"
                                        + "FROM TBL_WEB_SCRIPT_ATENDIMENTO_TIT \n"
                                        + "UNION ALL \n"
                                        + "SELECT * FROM TBL_WEB_SCRIPT_ATENDIMENTO_SUB \n"
                                        + "ORDER BY \n"
                                        + "1,2 \n";

                DAL_MIS AcessaDadosMis = new DAL.DAL_MIS();
                DataSet ds = AcessaDadosMis.ConsultaSQL(sqlcommand);

                DataTable dt = new DataTable();
                dt.Columns.Add("NR_SCRIPT");
                dt.Columns.Add("DS_SCRIPT");
                dt.Columns.Add("NR_SLIDE");

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    dt.Rows.Add(
                                    (dr[0].ToString() + "." + dr[1].ToString())
                                    , dr[2].ToString()
                                    , dr[3].ToString()
                               );
                }

                return dt;
            }
            catch (Exception ex)
            {
                throw new Exception("BLL.WEB.ScriptAtendimento_002: " + ex.Message, ex);
            }
        }
        public int AtualizaScript()
        {
            try
            {
                SqlCommand sqlcommand = new SqlCommand();
                sqlcommand.CommandType = CommandType.Text;

                sqlcommand.CommandText = "UPDATE TBL_WEB_SCRIPT_ATENDIMENTO_TIT SET \n"
                                        + "  DS_SCRIPT = @DS_SCRIPT \n"
                                        + "  NR_SLIDE  = @NR_SLIDE  \n"
                                        + "WHERE NR_SCRIPT = @NR_SCRIPT \n";

                DAL_MIS AcessaDadosMis = new DAL.DAL_MIS();
                return AcessaDadosMis.ExecutaComandoSQL(sqlcommand);
            }
            catch (Exception ex)
            {
                throw new Exception("BLL.WEB.ScriptAtendimento_002: " + ex.Message, ex);
            }
        }

    }
}