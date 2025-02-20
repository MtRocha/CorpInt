using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using Intranet_NEW.Controllers.DAL;

namespace Intranet.BLL.WEB
{
    public class Midia
    {
        public DataSet ListaMidia(DateTime DT_INICIO, DateTime DT_FIM, int TP_ENVIO, int CD_EMPRESA)
        {
            try
            {
                SqlCommand sqlcommand = new SqlCommand();
                sqlcommand.CommandType = CommandType.StoredProcedure;
                sqlcommand.Parameters.AddWithValue("@DT_INI", int.Parse(DT_INICIO.ToString("yyyyMMdd")));
                sqlcommand.Parameters.AddWithValue("@DT_FIM", int.Parse(DT_FIM.ToString("yyyyMMdd")));
                sqlcommand.Parameters.AddWithValue("@TP_ENVIO", TP_ENVIO);
                sqlcommand.Parameters.AddWithValue("@CD_EMPRESA", CD_EMPRESA);
                sqlcommand.CommandText = "SP_WEB_MIDIA";

                DAL_PROC AcessaDadosProc = new DAL.DAL_PROC();
                return AcessaDadosProc.ConsultaSQL(sqlcommand);
            }
            catch (Exception ex)
            {
                throw new Exception("BLL.WEB.Midia_001: " + ex.Message, ex);
            }
        }

        public DataTable ListaEmpresa()
        {
            try
            {
                SqlCommand sqlcommand = new SqlCommand();
                sqlcommand.CommandType = CommandType.Text;
                sqlcommand.CommandText = "SELECT * FROM DB_PROC..TBL_MIDIA_EMPRESAS ORDER BY 2 \n";

                DAL_PROC AcessaDadosProc = new Intranet.DAL.DAL_PROC();
                return AcessaDadosProc.ConsultaSQL(sqlcommand).Tables[0];
            }
            catch (Exception ex)
            {
                throw new Exception("RET.Midia.ADO_002: " + ex.Message, ex);
            }
        }

        public DataSet ListaVolume()
        {
            try
            {
                SqlCommand sqlcommand = new SqlCommand();
                sqlcommand.CommandType = CommandType.Text;
                sqlcommand.CommandText = "SELECT TIPO_ENVIO, COUNT(DISTINCT NR_CPF) AS QTDE_CLIENTE, COUNT(*) AS QTDE_EMAIL \n"
                    + " FROM DB_PROC..TBL_CONTROLE_MIDIA \n"
                    + " WHERE 1=1 \n"
                    + " AND LEFT(DATA_ENVIO_MIDIA,6) = LEFT(CONVERT(CHAR(8), GETDATE(), 112), 6) \n"
                    + " GROUP BY TIPO_ENVIO ORDER BY 1"
                    + " "
                    + "SELECT TIPO_ENVIO, COUNT(DISTINCT NR_CPF) AS QTDE_CLIENTE, COUNT(*) AS QTDE_EMAIL \n"
                    + " FROM DB_PROC..TBL_CONTROLE_MIDIA \n"
                    + " WHERE 1=1 \n"
                    + " AND DATA_ENVIO_MIDIA = (SELECT MAX(DATA_ENVIO_MIDIA) FROM TBL_CONTROLE_MIDIA WHERE TIPO_ENVIO = 3)"
                    + " GROUP BY TIPO_ENVIO ORDER BY 1"
                    + " "
                    + "SELECT TIPO_ENVIO, MAX(UPPER(DATA_ENVIO_MIDIA)) FROM TBL_CONTROLE_MIDIA GROUP BY TIPO_ENVIO";

                DAL_PROC AcessaDadosProc = new DAL.DAL_PROC();
                return AcessaDadosProc.ConsultaSQL(sqlcommand);
            }
            catch (Exception ex)
            {
                throw new Exception("RET.Midia_003: " + ex.Message, ex);
            }
        }
    }
}

