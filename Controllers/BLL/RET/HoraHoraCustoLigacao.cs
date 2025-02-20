using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlTypes;
using Intranet_NEW.Controllers.DAL;


namespace Intranet.BLL.RET
{
    public class HoraHoraCustoLigacao
    {

        DAL_MIS AcessaDadosMis = new DAL.DAL_MIS();

        public void AtualizaCustoLigacao(DateTime DT_ACIONAMENTO, int HR_ACIONAMENTO, decimal NR_CUSTO_LIGACAO, int NR_USUARIO)
        {
            try
            {
                int retorno = int.Parse(ListaLançamentoCusto(DT_ACIONAMENTO, HR_ACIONAMENTO).ToString());

                if (retorno > 0)
                {
                    SqlCommand sqlcommand = new SqlCommand();
                    sqlcommand.CommandType = CommandType.Text;
                    sqlcommand.CommandText = "UPDATE TBL_RET_RELATORIO_CUSTO \n";
                    sqlcommand.CommandText += " SET NR_CUSTO_LIGACAO = @NR_CUSTO_LIGACAO, DT_ATUALIZACAO = GETDATE(), NR_USUARIO_ATU = @NR_USUARIO, NR_CARTEIRA = 0 \n";
                    sqlcommand.CommandText += " WHERE (DT_ACIONAMENTO = @DT_ACIONAMENTO AND NR_HORA_ACIONAMENTO = @HR_ACIONAMENTO)";

                    sqlcommand.Parameters.AddWithValue("@DT_ACIONAMENTO", DT_ACIONAMENTO.ToString("yyyyMMdd"));
                    sqlcommand.Parameters.AddWithValue("@HR_ACIONAMENTO", HR_ACIONAMENTO);
                    sqlcommand.Parameters.AddWithValue("@NR_CUSTO_LIGACAO", NR_CUSTO_LIGACAO);
                    sqlcommand.Parameters.AddWithValue("@NR_USUARIO", NR_USUARIO);

                    DAL_MIS AcessaDadosMis = new Intranet.DAL.DAL_MIS();
                    AcessaDadosMis.ExecutaComandoSQL(sqlcommand);
                }

                else
                {
                    SqlCommand sqlcommand = new SqlCommand();
                    sqlcommand.CommandType = CommandType.Text;
                    sqlcommand.CommandText = "INSERT INTO TBL_RET_RELATORIO_CUSTO (DT_ACIONAMENTO, NR_HORA_ACIONAMENTO,NR_CUSTO_LIGACAO,DT_INCLUSAO, NR_USUARIO_INC, NR_CARTEIRA)  \n";
                    sqlcommand.CommandText += " VALUES (@DT_ACIONAMENTO, @HR_ACIONAMENTO, @NR_CUSTO_LIGACAO, GETDATE(), @NR_USUARIO, 0)";

                    sqlcommand.Parameters.AddWithValue("@DT_ACIONAMENTO", DT_ACIONAMENTO.ToString("yyyyMMdd"));
                    sqlcommand.Parameters.AddWithValue("@HR_ACIONAMENTO", HR_ACIONAMENTO);
                    sqlcommand.Parameters.AddWithValue("@NR_CUSTO_LIGACAO", NR_CUSTO_LIGACAO);
                    sqlcommand.Parameters.AddWithValue("@NR_USUARIO", NR_USUARIO);


                    DAL_MIS AcessaDadosMis = new Intranet.DAL.DAL_MIS();
                    AcessaDadosMis.ExecutaComandoSQL(sqlcommand);

                }
            }
            catch (Exception ex)
            {
                throw new Exception("RET.CmdLigacao_001: " + ex.Message, ex);
            }
        }

        public int ListaLançamentoCusto(DateTime DT_ACIONAMENTO, int HR_ACIONAMENTO)
        {
            try
            {
                SqlCommand sqlcommand = new SqlCommand();
                sqlcommand.CommandType = CommandType.Text;

                sqlcommand.CommandText += "SELECT COUNT(*) AS [QT_REGISTRO] \n";
                sqlcommand.CommandText += "FROM TBL_RET_RELATORIO_CUSTO \n";
                sqlcommand.CommandText += "WHERE DT_ACIONAMENTO = @DT_OCORRENCIA AND NR_HORA_ACIONAMENTO = @HR_ACIONAMENTO \n";

                sqlcommand.Parameters.AddWithValue("@DT_OCORRENCIA", DT_ACIONAMENTO.ToString("yyyyMMdd"));
                sqlcommand.Parameters.AddWithValue("@HR_ACIONAMENTO", HR_ACIONAMENTO);

                return int.Parse(AcessaDadosMis.ConsultaSQL(sqlcommand).Tables[0].Rows[0][0].ToString());

            }

            catch (Exception ex)
            {
                throw new Exception("RET.CmdLigacao_002: " + ex.Message, ex);
            }
        }
    }
}