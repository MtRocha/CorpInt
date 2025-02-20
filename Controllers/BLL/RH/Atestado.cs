using Intranet_NEW.Controllers.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;

namespace Intranet.BLL.RH
{
    public class Atestado
    {
        public DataSet ListaAtestado(DateTime DT_INI, DateTime DT_FIM, string NM_COLABORADOR)
        {
            try
            {
                SqlCommand sqlcommand = new SqlCommand();
                sqlcommand.CommandType = CommandType.Text;
                sqlcommand.Parameters.AddWithValue("@DT_INI", DT_INI);
                sqlcommand.Parameters.AddWithValue("@DT_FIM", DT_FIM);
                sqlcommand.Parameters.AddWithValue("@NM_COLABORADOR", NM_COLABORADOR != "" ? "%" + NM_COLABORADOR + "%" : "");

                sqlcommand.CommandText = "SELECT \n"
                                + "	  A.NR_ATESTADO \n"
                                + "	, A.DT_JUSTIFICADA \n"
                                + "	, B.NM_COLABORADOR \n"
                                + "	, A.TP_ATESTADO \n"
                                + "	, A.TP_ABONO \n"
                                + "	, CASE A.TP_ATESTADO \n"
                                        + "			WHEN '0'  THEN 'Alistamento' \n"
                                        + "			WHEN '1'  THEN 'Auto Escola' \n"
                                        + "			WHEN '2'  THEN 'Boletim de Ocorrência' \n"
                                        + "			WHEN '3'  THEN 'Escola'   \n"
                                        + "			WHEN '4'  THEN 'Justiça'  \n"
                                        + "			WHEN '5'  THEN 'Atestado Médico DIA'    \n"
                                        + "			WHEN '6'  THEN 'Atestado Médico Horas'  \n"
                                        + "			WHEN '7'  THEN 'Óbito'  \n"
                                        + "			WHEN '8'  THEN 'Odontologico'  \n"
                                        + "			WHEN '9'  THEN 'Prova'   \n"
                                        + "			WHEN '10' THEN 'Reunião' \n"
                                        + "			WHEN '11' THEN 'Doação de Sangue' \n"
                                        + "	 END AS [A.TP_ATESTADO] \n"
                                + "	, CASE WHEN TP_CHECKLIST LIKE '%1%' THEN '/Image/bullet_gre.png' END AS [TP_01] \n"
                                + "	, CASE WHEN TP_CHECKLIST LIKE '%2%' THEN '/Image/bullet_gre.png' END AS [TP_02] \n"
                                + "	, CASE WHEN TP_CHECKLIST LIKE '%3%' THEN '/Image/bullet_gre.png' END AS [TP_03] \n"
                                + "FROM TBL_WEB_RH_ATESTADO_MEDICO A \n"
                                + "	INNER JOIN TBL_WEB_COLABORADOR_DADOS B \n"
                                + "		ON A.NR_COLABORADOR = B.NR_COLABORADOR \n"
                                + "WHERE DT_JUSTIFICADA BETWEEN @DT_INI AND @DT_FIM \n"
                                + "AND ((@NM_COLABORADOR = '') OR (@NM_COLABORADOR <> '' AND B.NM_COLABORADOR LIKE @NM_COLABORADOR)) \n"
                                + "ORDER BY \n"
                                + "	A.DT_JUSTIFICADA \n";

                DAL_MIS AcessaDadosMis = new DAL.DAL_MIS();
                return AcessaDadosMis.ConsultaSQL(sqlcommand);
            }
            catch (Exception ex)
            {
                throw new Exception("BLL.RH.Atestado_001: " + ex.Message, ex);
            }
        }

        public Intranet_NEW.Models.RH.Atestado DadosAtestado(string NR_ATESTADO)
        {
            try
            {
                SqlCommand sqlcommand = new SqlCommand();
                sqlcommand.CommandType = CommandType.Text;
                sqlcommand.Parameters.AddWithValue("@NR_ATESTADO", NR_ATESTADO);
                sqlcommand.CommandText = "SELECT A.* \n"
                                        + ", (SELECT NM_COLABORADOR FROM TBL_WEB_COLABORADOR_DADOS WHERE NR_COLABORADOR = A.NR_COLABORADOR) AS [NM_COLABORADOR] \n"
                                        + ", (SELECT NM_COLABORADOR FROM TBL_WEB_COLABORADOR_DADOS WHERE NR_COLABORADOR = A.NR_RECEBIDO_POR) AS [NM_RECEBIDO_POR] \n"
                                        + ", (SELECT NM_COLABORADOR FROM TBL_WEB_COLABORADOR_DADOS WHERE NR_COLABORADOR = A.NR_RESPONSAVEL_RH) AS [NM_RESPONSAVEL_RH] \n"
                                        + "FROM TBL_WEB_RH_ATESTADO_MEDICO A \n"
                                        + "WHERE A.NR_ATESTADO = @NR_ATESTADO \n";

                DAL_MIS AcessaDadosMis = new DAL.DAL_MIS();
                DataSet ds = AcessaDadosMis.ConsultaSQL(sqlcommand);

                if ((ds.Tables.Count > 0) && (ds.Tables[0].Rows.Count > 0))
                {
                    DataRow dr = ds.Tables[0].Rows[0];
                    Intranet_NEW.Models.RH.Atestado dto = new DTO.RH.Atestado();

                    dto.NR_ATESTADO = dr["NR_ATESTADO"].ToString();

                    dto.NR_COLABORADOR = dr["NR_COLABORADOR"].ToString();
                    dto.NM_COLABORADOR = dr["NM_COLABORADOR"].ToString();

                    dto.DT_JUSTIFICADA = dr["DT_JUSTIFICADA"] != DBNull.Value ? ((DateTime)dr["DT_JUSTIFICADA"]).ToString("dd/MM/yyyy") : "";
                    dto.DT_ENTREGA = dr["DT_ENTREGA"] != DBNull.Value ? ((DateTime)dr["DT_ENTREGA"]).ToString("dd/MM/yyyy") : "";
                    dto.NM_OBSERVACAO = dr["NM_OBSERVACAO"].ToString();
                    dto.TP_CHECKLIST = dr["TP_CHECKLIST"].ToString();

                    dto.NR_RECEBIDO_POR = dr["NR_RECEBIDO_POR"].ToString();
                    dto.NM_RECEBIDO_POR = dr["NM_RECEBIDO_POR"].ToString();

                    dto.NR_RESPONSAVEL_RH = dr["NR_RESPONSAVEL_RH"].ToString();
                    dto.NM_RESPONSAVEL_RH = dr["NM_RESPONSAVEL_RH"].ToString();

                    return dto;
                }
                return new DTO.RH.Atestado();
            }
            catch (Exception ex)
            {
                throw new Exception("BLL.RH.Atestado_002: " + ex.Message, ex);
            }
        }

        public int GravaAtestado(Intranet_NEW.Models.RH.Atestado dto)
        {
            try
            {
                SqlCommand sqlcommand = new SqlCommand();
                sqlcommand.CommandType = CommandType.Text;

                DateTime dt = DateTime.MinValue;
                int i = 0;
                sqlcommand.Parameters.AddWithValue("@NR_ATESTADO", dto.NR_ATESTADO);
                sqlcommand.Parameters.AddWithValue("@TP_ATESTADO", dto.TP_ATESTADO);
                sqlcommand.Parameters.AddWithValue("@TP_ABONO", dto.TP_ABONO);
                sqlcommand.Parameters.AddWithValue("@NR_COLABORADOR", dto.NR_COLABORADOR);
                sqlcommand.Parameters.AddWithValue("@DT_JUSTIFICADA", DateTime.TryParse(dto.DT_JUSTIFICADA, out dt) ? (object)DateTime.Parse(dto.DT_JUSTIFICADA) : DBNull.Value);
                sqlcommand.Parameters.AddWithValue("@DT_ENTREGA", DateTime.TryParse(dto.DT_ENTREGA, out dt) ? (object)DateTime.Parse(dto.DT_ENTREGA) : DBNull.Value);
                sqlcommand.Parameters.AddWithValue("@NM_OBSERVACAO", dto.NM_OBSERVACAO.ToUpper());
                sqlcommand.Parameters.AddWithValue("@TP_CHECKLIST", dto.TP_CHECKLIST);

                sqlcommand.Parameters.AddWithValue("@NR_RECEBIDO_POR", int.TryParse(dto.NR_RECEBIDO_POR, out i) ? (object)int.Parse(dto.NR_RECEBIDO_POR) : DBNull.Value);
                sqlcommand.Parameters.AddWithValue("@NR_RESPONSAVEL_RH", int.TryParse(dto.NR_RESPONSAVEL_RH, out i) ? (object)int.Parse(dto.NR_RESPONSAVEL_RH) : DBNull.Value);

                if (dto.NR_ATESTADO == "")
                    return Novo(sqlcommand);
                else
                    return Atualizar(sqlcommand);
            }
            catch (Exception ex)
            {
                throw new Exception("BLL.RH.Atestado_003: " + ex.Message, ex);
            }
        }

        private int Novo(SqlCommand sqlcommand)
        {
            try
            {
                sqlcommand.CommandText = "INSERT INTO TBL_WEB_RH_ATESTADO_MEDICO \n"
                                        + "        (NR_COLABORADOR, DT_JUSTIFICADA, DT_ENTREGA, NM_OBSERVACAO, TP_CHECKLIST, NR_RECEBIDO_POR, NR_RESPONSAVEL_RH, TP_ATESTADO, TP_ABONO) \n"
                                        + " VALUES (@NR_COLABORADOR, @DT_JUSTIFICADA, @DT_ENTREGA, @NM_OBSERVACAO, @TP_CHECKLIST, @NR_RECEBIDO_POR, @NR_RESPONSAVEL_RH, @TP_ATESTADO, @TP_ABONO)  \n";

                DAL_MIS AcessaDadosMis = new DAL.DAL_MIS();
                return AcessaDadosMis.ExecutaComandoSQL(sqlcommand);
            }
            catch (Exception ex)
            {
                throw new Exception("BLL.RH.Atestado_004: " + ex.Message, ex);
            }
        }

        private int Atualizar(SqlCommand sqlcommand)
        {
            try
            {
                sqlcommand.CommandText = "UPDATE TBL_WEB_RH_ATESTADO_MEDICO SET \n"
                                        + "NR_COLABORADOR = @NR_COLABORADOR, DT_JUSTIFICADA = @DT_JUSTIFICADA, DT_ENTREGA     = @DT_ENTREGA, NM_OBSERVACAO  = @NM_OBSERVACAO, TP_CHECKLIST   = @TP_CHECKLIST, NR_RECEBIDO_POR = @NR_RECEBIDO_POR, NR_RESPONSAVEL_RH  = @NR_RESPONSAVEL_RH, TP_ATESTADO = @TP_ATESTADO, TP_ABONO = @TP_ABONO \n"
                                        + "WHERE NR_ATESTADO = @NR_ATESTADO \n";

                DAL_MIS AcessaDadosMis = new DAL.DAL_MIS();
                return AcessaDadosMis.ExecutaComandoSQL(sqlcommand);
            }
            catch (Exception ex)
            {
                throw new Exception("BLL.RH.Atestado_005: " + ex.Message, ex);
            }
        }
    }
}
