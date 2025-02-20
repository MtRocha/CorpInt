using Intranet_NEW.Controllers.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;

namespace Intranet.BLL.WEB
{
    public class MonitoramentoPontuacao
    {
        public Intranet_NEW.Models.WEB.MonitoramentoPontuacao ListaPontuacao()
        {
            try
            {
                SqlCommand sqlcommand = new SqlCommand();
                sqlcommand.CommandType = CommandType.Text;
                sqlcommand.CommandText = "SELECT A.*, B.NM_COLABORADOR FROM TBL_WEB_MONITORAMENTO_PONTUACAO A \n"
                                        + "    INNER JOIN TBL_WEB_COLABORADOR_DADOS B ON A.NR_USUARIO_SISTEMA = B.NR_COLABORADOR";

                DAL_MIS AcessaDadosMis = new DAL.DAL_MIS();
                DataSet ds = AcessaDadosMis.ConsultaSQL(sqlcommand);

                Intranet_NEW.Models.WEB.MonitoramentoPontuacao dto = new DTO.WEB.MonitoramentoPontuacao();

                if ((ds.Tables.Count > 0) && (ds.Tables[0].Rows.Count > 0))
                {
                    dto.NR_PONTO_CE = ds.Tables[0].Rows[0]["NR_PONTO_CE"].ToString();
                    dto.NR_PONTO_CPC = ds.Tables[0].Rows[0]["NR_PONTO_CPC"].ToString();
                    dto.NR_PONTO_PP = ds.Tables[0].Rows[0]["NR_PONTO_PP"].ToString();
                    dto.NR_PONTO_T_FALANDO = ds.Tables[0].Rows[0]["NR_PONTO_T_FALANDO"].ToString();
                    dto.NR_INDICE_DESEMPATE = ds.Tables[0].Rows[0]["NR_INDICE_DESEMPATE"].ToString();

                    dto.NM_COLABORADOR = ds.Tables[0].Rows[0]["NM_COLABORADOR"].ToString();
                    dto.DT_INCLUSAO = ((DateTime)ds.Tables[0].Rows[0]["DT_INCLUSAO"]).ToString("dd/MM/yyyy HH:mm:ss");
                }
                return dto;

            }
            catch (Exception ex)
            {
                throw new Exception("BLL.WEB.Monitoramento.Pontuacao_001: " + ex.Message, ex);
            }
        }

        public int GravaPontuacao(Intranet_NEW.Models.WEB.MonitoramentoPontuacao dto)
        {
            try
            {
                SqlCommand sqlcommand = new SqlCommand();
                sqlcommand.CommandType = CommandType.Text;
                sqlcommand.CommandText = "DELETE FROM TBL_WEB_MONITORAMENTO_PONTUACAO \n"
                                        + "INSERT INTO TBL_WEB_MONITORAMENTO_PONTUACAO VALUES(@NR_PONTO_CE, @NR_PONTO_CPC, @NR_PONTO_PP, @NR_PONTO_T_FALANDO, @NR_USUARIO_SISTEMA, GETDATE())";

                int num = 0;
                sqlcommand.Parameters.AddWithValue("@NR_PONTO_CE", int.TryParse(dto.NR_PONTO_CE, out num) ? int.Parse(dto.NR_PONTO_CE) : 1);
                sqlcommand.Parameters.AddWithValue("@NR_PONTO_CPC", int.TryParse(dto.NR_PONTO_CPC, out num) ? int.Parse(dto.NR_PONTO_CPC) : 1);
                sqlcommand.Parameters.AddWithValue("@NR_PONTO_PP", int.TryParse(dto.NR_PONTO_PP, out num) ? int.Parse(dto.NR_PONTO_PP) : 1);
                sqlcommand.Parameters.AddWithValue("@NR_PONTO_T_FALANDO", int.TryParse(dto.NR_PONTO_T_FALANDO, out num) ? int.Parse(dto.NR_PONTO_T_FALANDO) : 1);
                sqlcommand.Parameters.AddWithValue("@NR_INDICE_DESEMPATE", int.TryParse(dto.NR_INDICE_DESEMPATE, out num) ? int.Parse(dto.NR_INDICE_DESEMPATE) : 1);

                sqlcommand.Parameters.AddWithValue("@NR_USUARIO_SISTEMA", dto.NR_USUARIO_SISTEMA);

                DAL_MIS AcessaDadosMis = new DAL.DAL_MIS();
                return AcessaDadosMis.ExecutaComandoSQL(sqlcommand);
            }
            catch (Exception ex)
            {
                throw new Exception("BLL.WEB.Monitoramento.Pontuacao_002: " + ex.Message, ex);
            }
        }
    }
}
