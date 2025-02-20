using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;
using Intranet_NEW.Controllers.DAL;


namespace Intranet.BLL.CAR
{
    public class Remessa
    {
        public DataSet ArquivoProcessado(DateTime dtProcessamento)
        {
            SqlCommand sqlcommand = new SqlCommand();
            sqlcommand.CommandType = CommandType.Text;
            sqlcommand.CommandText = "SELECT [DT_INI_IMPORTACAO],[DT_ARQUIVO],[NM_ARQUIVO],[QT_CLIENTE],[QT_CONTRATO],[QT_TELEFONE],[QT_EMAIL],[QT_REGISTRO],[QT_CLIENTE_NOVO],[QT_CLIENTE_BAIXADO],[QT_CLIENTE_SEM_FONE] \n"
                                   + "FROM DB_PROC_ROVERI..LOG_REM_IMPORTACAO WHERE LEFT(DT_ARQUIVO,6) = @DT_ARQUIVO \n"
                                   + "\n\n"
                                   + "UNION ALL"
                                   + "\n\n"
                                   + "SELECT NULL, '99999999', NULL, SUM(QT_CLIENTE), SUM([QT_CONTRATO]), SUM([QT_TELEFONE]), SUM([QT_EMAIL]), SUM([QT_REGISTRO]), SUM([QT_CLIENTE_NOVO]), SUM([QT_CLIENTE_BAIXADO]), SUM([QT_CLIENTE_SEM_FONE]) \n"
                                   + " FROM DB_PROC_ROVERI..LOG_REM_IMPORTACAO \n"
                                   + " WHERE LEFT(DT_ARQUIVO,6) = @DT_ARQUIVO \n"
                                   + " ORDER BY DT_ARQUIVO \n\n";

            sqlcommand.Parameters.AddWithValue("DT_ARQUIVO", dtProcessamento.ToString("yyyyMM"));
            
            DAL_PROC AcessaDadosProc = new DAL.DAL_PROC();
            return AcessaDadosProc.ConsultaSQL(sqlcommand);
        }
    }
}