using Intranet_NEW.Controllers.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace Intranet.BLL.RET
{
    public class HoraHoraOcorrencia
    {
        DAL_MIS AcessaDadosMis = new Intranet.DAL.DAL_MIS();

        public DataSet ListaOcorrencia(DateTime DT_OCORRENCIA, string HR_OCORRENCIA)
        {
            SqlCommand sqlcommand = new SqlCommand();
            sqlcommand.CommandType = CommandType.Text;

            sqlcommand.CommandText += "SELECT b.NM_COLABORADOR, a.DS_OCORRENCIA, a.DT_INCLUSAO \n";
            sqlcommand.CommandText += "FROM TBL_RET_RELATORIO_HORA_HORA_OCORRENCIA a \n";
            sqlcommand.CommandText += "    INNER JOIN TBL_WEB_COLABORADOR_DADOS b ON a.NR_USUARIO = b.NR_COLABORADOR \n";
            sqlcommand.CommandText += "WHERE a.DT_OCORRENCIA = @DT_OCORRENCIA AND a.HR_OCORRENCIA = @HR_OCORRENCIA \n";

            try
            {
                sqlcommand.Parameters.AddWithValue("@DT_OCORRENCIA", DT_OCORRENCIA.ToString("yyyyMMdd"));
                sqlcommand.Parameters.AddWithValue("@HR_OCORRENCIA", HR_OCORRENCIA);

                DataSet ds = AcessaDadosMis.ConsultaSQL(sqlcommand);

                foreach (DataRow dr in ds.Tables[0].Rows) dr[1] = dr[1].ToString().Replace("\n", "<br>");
                return ds;
            }
            catch (Exception ex)
            {
                throw new Exception("" + ex.Message, ex);
            }
        }

        public DataTable ListaTotalOcorrencia(DateTime DT_OCORRENCIA)
        {
            SqlCommand sqlcommand = new SqlCommand();
            sqlcommand.CommandType = CommandType.Text;

            sqlcommand.CommandText += "SELECT HR_OCORRENCIA, COUNT(*) AS [QT_OCORRENCIA] \n";
            sqlcommand.CommandText += "FROM TBL_RET_RELATORIO_HORA_HORA_OCORRENCIA \n";
            sqlcommand.CommandText += "WHERE DT_OCORRENCIA = @DT_OCORRENCIA \n";
            sqlcommand.CommandText += "GROUP BY HR_OCORRENCIA \n";

            try
            {
                sqlcommand.Parameters.AddWithValue("@DT_OCORRENCIA", DT_OCORRENCIA.ToString("yyyyMMdd"));
                return AcessaDadosMis.ConsultaSQL(sqlcommand).Tables[0];
            }
            catch (Exception ex)
            {
                throw new Exception("" + ex.Message, ex);
            }
        }

        public DataSet GravaOcorrencia(Intranet_NEW.Models.HoraHoraOcorrencia obj)
        {
            SqlCommand sqlcommand = new SqlCommand();
            sqlcommand.CommandType = CommandType.Text;

            sqlcommand.CommandText += "INSERT INTO TBL_RET_RELATORIO_HORA_HORA_OCORRENCIA(NR_USUARIO, DT_INCLUSAO, DT_OCORRENCIA, HR_OCORRENCIA, DS_OCORRENCIA) \n";
            sqlcommand.CommandText += "VALUES(@NR_USUARIO, GETDATE(), @DT_OCORRENCIA, @HR_OCORRENCIA, @DS_OCORRENCIA) \n";

            try
            {
                sqlcommand.Parameters.AddWithValue("@NR_USUARIO", obj.NR_USUARIO);
                sqlcommand.Parameters.AddWithValue("@DT_OCORRENCIA", obj.DT_OCORRENCIA.ToString("yyyyMMdd"));
                sqlcommand.Parameters.AddWithValue("@HR_OCORRENCIA", obj.HR_OCORRENCIA);

                AcessaDadosMis.ExecutaComandoSQL(sqlcommand);
                return ListaOcorrencia(obj.DT_OCORRENCIA, obj.HR_OCORRENCIA);
            }
            catch (Exception ex)
            {
                throw new Exception("" + ex.Message, ex);
            }
        }
    }
}