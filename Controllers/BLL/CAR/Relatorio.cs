using Intranet_NEW.Controllers.DAL;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Intranet.BLL.CAR
{
    public class Relatorio
    {
        public DataSet GeraRelatorio(string EMAIL, string CPF, string CONTRATO, string TP_CONT, 
            string PRODUTO, string FAIXA,string ANO, string MES, string DIA, int VL1, int VL2, int DAT1, int DAT2)
        {
            try
            {
                SqlCommand sqlcommand = new SqlCommand();
                sqlcommand.CommandType = CommandType.StoredProcedure;
                sqlcommand.CommandText = "SP_WEB_REM_ANALITICO";
                sqlcommand.Parameters.AddWithValue("@EMAIL", EMAIL);
                sqlcommand.Parameters.AddWithValue("@CPF", CPF);
                sqlcommand.Parameters.AddWithValue("@CONTRATO", CONTRATO);
                sqlcommand.Parameters.AddWithValue("@TP_CONT", TP_CONT);
                sqlcommand.Parameters.AddWithValue("@PRODUTO", PRODUTO);
                sqlcommand.Parameters.AddWithValue("@FAIXA",FAIXA);
                sqlcommand.Parameters.AddWithValue("@ANO", ANO);
                sqlcommand.Parameters.AddWithValue("@MES",MES);
                sqlcommand.Parameters.AddWithValue("@DIA", DIA);
                sqlcommand.Parameters.AddWithValue("@VL1", VL1);
                sqlcommand.Parameters.AddWithValue("@VL2", VL2);
                sqlcommand.Parameters.AddWithValue("@DAT1", DAT1);
                sqlcommand.Parameters.AddWithValue("@DAT2", DAT2);

                DAL_MIS Acessa = new Intranet.DAL.DAL_MIS();

                DataSet dsRelatorio = Acessa.ConsultaSQL(sqlcommand);

                DataTable dt = dsRelatorio.Tables[0].Rows.Cast<System.Data.DataRow>().Take(100).CopyToDataTable();

                DataSet dsRel = new DataSet();
                dsRel.Tables.Add(dt);
                
                return dsRel;
            }
            catch (Exception ex)
            {
                throw new Exception("REM.cmdGrafico: " + ex.Message, ex);
            }
        }

        public DataSet Excel(string EMAIL, string CPF, string CONTRATO, string TP_CONT,
            string PRODUTO, string FAIXA, string ANO, string MES, string DIA, int VL1, int VL2, int DAT1, int DAT2)
        {
            try
            {
                SqlCommand sqlcommand = new SqlCommand();
                sqlcommand.CommandType = CommandType.StoredProcedure;
                sqlcommand.CommandText = "SP_WEB_REM_ANALITICO";
                sqlcommand.Parameters.AddWithValue("@EMAIL", EMAIL);
                sqlcommand.Parameters.AddWithValue("@CPF", CPF);
                sqlcommand.Parameters.AddWithValue("@CONTRATO", CONTRATO);
                sqlcommand.Parameters.AddWithValue("@TP_CONT", TP_CONT);
                sqlcommand.Parameters.AddWithValue("@PRODUTO", PRODUTO);
                sqlcommand.Parameters.AddWithValue("@FAIXA", FAIXA);
                sqlcommand.Parameters.AddWithValue("@ANO", ANO);
                sqlcommand.Parameters.AddWithValue("@MES", MES);
                sqlcommand.Parameters.AddWithValue("@DIA", DIA);
                sqlcommand.Parameters.AddWithValue("@VL1", VL1);
                sqlcommand.Parameters.AddWithValue("@VL2", VL2);
                sqlcommand.Parameters.AddWithValue("@DAT1", DAT1);
                sqlcommand.Parameters.AddWithValue("@DAT2", DAT2);

                DAL_MIS Acessa = new Intranet.DAL.DAL_MIS();

                DataSet dsRelatorio = Acessa.ConsultaSQL(sqlcommand);

                return dsRelatorio;
            }
            catch (Exception ex)
            {
                throw new Exception("REM.cmdGrafico: " + ex.Message, ex);
            } 
        }
    }
}