using Intranet_NEW.Controllers.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;

namespace Intranet.BLL.RH
{
    public class SolicitacaoJustificativa
    {
        public int GravaSolicitacaoJustificativa(Intranet_NEW.Models.RH.SolicitacaoJustificativa dto)
        {
            try
            {
                SqlCommand sqlCommand = new SqlCommand();
                sqlCommand.CommandType = CommandType.Text;
                sqlCommand.CommandText = "INSERT INTO TBL_WEB_RH_PONTO_JUSTIFICATIVA \n"
                                        + "( DT_SOLICITACAO, NR_SOLICITANTE, DT_MESREF, DT_MARCACAO, NM_JUSTIFICATIVA, TP_MOTIVO, NM_RECEBIDO_POR \n"
                                        + ", NR_SUP_RESPONSAVEL, TP_SUP_STATUS, DT_SUP_ANALISE, DS_SUP_OBSERVACAO \n"
                                        + ", NR_GER_RESPONSAVEL, TP_GER_STATUS, DT_GER_ANALISE, DS_GER_OBSERVACAO \n"
                                        + ", TP_RH_STATUS \n"
                                        + ") VALUES \n"
                                        + "( GETDATE(), @NR_SOLICITANTE, @DT_MESREF, @DT_MARCACAO, @NM_JUSTIFICATIVA, @TP_MOTIVO, @NM_RECEBIDO_POR  \n"
                                        + ", @NR_SUP_RESPONSAVEL, @TP_SUP_STATUS, @DT_SUP_ANALISE, @DS_SUP_OBSERVACAO \n"
                                        + ", @NR_GER_RESPONSAVEL, @TP_GER_STATUS, @DT_GER_ANALISE, @DS_GER_OBSERVACAO \n"
                                        + ", @TP_RH_STATUS \n"
                                        + ") \n";

                sqlCommand.Parameters.AddWithValue("@NR_SOLICITACAO", 0);
                sqlCommand.Parameters.AddWithValue("@NR_SOLICITANTE", dto.NR_SOLICITANTE);

                sqlCommand.Parameters.AddWithValue("@DT_MESREF", dto.DT_MESREF);
                sqlCommand.Parameters.AddWithValue("@DT_MARCACAO", DateTime.Parse(dto.DT_MARCACAO));
                sqlCommand.Parameters.AddWithValue("@NM_JUSTIFICATIVA", dto.NM_JUSTIFICATIVA);
                sqlCommand.Parameters.AddWithValue("@TP_MOTIVO", dto.TP_MOTIVO);
                sqlCommand.Parameters.AddWithValue("@NM_RECEBIDO_POR", dto.NM_RECEBIDO_POR != "" ? (object)dto.NM_RECEBIDO_POR : DBNull.Value);

                sqlCommand.Parameters.AddWithValue("@TP_RH_STATUS", 0);

                if (dto.TP_MOTIVO == "3")
                {
                    sqlCommand.Parameters.AddWithValue("@NR_SUP_RESPONSAVEL", dto.NR_SUP_RESPONSAVEL != "" ? (object)dto.NR_SUP_RESPONSAVEL : DBNull.Value);
                    sqlCommand.Parameters.AddWithValue("@TP_SUP_STATUS", 1);
                    sqlCommand.Parameters.AddWithValue("@DT_SUP_ANALISE", DateTime.Now);
                    sqlCommand.Parameters.AddWithValue("@DS_SUP_OBSERVACAO", "APROVAÇÃO AUTOMATICA PELO SISTEMA. SEM ANALISE DO SUPERVISOR");

                    sqlCommand.Parameters.AddWithValue("@NR_GER_RESPONSAVEL", dto.NR_GER_RESPONSAVEL != "" ? (object)dto.NR_GER_RESPONSAVEL : DBNull.Value);
                    sqlCommand.Parameters.AddWithValue("@TP_GER_STATUS", 1);
                    sqlCommand.Parameters.AddWithValue("@DT_GER_ANALISE", DateTime.Now);
                    sqlCommand.Parameters.AddWithValue("@DS_GER_OBSERVACAO", "APROVAÇÃO AUTOMATICA PELO SISTEMA. SEM ANALISE DO GESTOR");
                }
                else
                {
                    if (dto.NR_SUP_RESPONSAVEL == "")
                    {
                        sqlCommand.Parameters.AddWithValue("@NR_SUP_RESPONSAVEL", dto.NR_SUP_RESPONSAVEL != "" ? (object)dto.NR_SUP_RESPONSAVEL : DBNull.Value);
                        sqlCommand.Parameters.AddWithValue("@TP_SUP_STATUS", 1);
                        sqlCommand.Parameters.AddWithValue("@DT_SUP_ANALISE", DateTime.Now);
                        sqlCommand.Parameters.AddWithValue("@DS_SUP_OBSERVACAO", "APROVAÇÃO AUTOMATICA PELO SISTEMA. SEM ANALISE DO SUPERVISOR");
                    }
                    else
                    {
                        sqlCommand.Parameters.AddWithValue("@NR_SUP_RESPONSAVEL", dto.NR_SUP_RESPONSAVEL != "" ? (object)dto.NR_SUP_RESPONSAVEL : DBNull.Value);
                        sqlCommand.Parameters.AddWithValue("@TP_SUP_STATUS", 0);
                        sqlCommand.Parameters.AddWithValue("@DT_SUP_ANALISE", DBNull.Value);
                        sqlCommand.Parameters.AddWithValue("@DS_SUP_OBSERVACAO", DBNull.Value);
                    }

                    sqlCommand.Parameters.AddWithValue("@NR_GER_RESPONSAVEL", dto.NR_GER_RESPONSAVEL != "" ? (object)dto.NR_GER_RESPONSAVEL : DBNull.Value);
                    sqlCommand.Parameters.AddWithValue("@TP_GER_STATUS", 0);
                    sqlCommand.Parameters.AddWithValue("@DT_GER_ANALISE", DBNull.Value);
                    sqlCommand.Parameters.AddWithValue("@DS_GER_OBSERVACAO", DBNull.Value);
                }

                DAL_MIS AcessaDadosMis = new Intranet.DAL.DAL_MIS();
                return AcessaDadosMis.ExecutaComandoSQL(sqlCommand);
            }
            catch (Exception ex)
            {
                throw new Exception("RH.SolicitacaoJustificativa_001: " + ex.Message, ex);
            }
        }

        public DataSet ListaSolicitacao(string NR_RESPONSAVEL, string DT_MESREF, string TP_SUP_STATUS, string TP_GER_STATUS, string TP_RH_STATUS, string NR_SUPERVISOR, string NR_GESTOR, string NR_RH, string TP_MOTIVO, string NM_COLABORADOR)
        {
            try
            {
                SqlCommand sqlCommand = new SqlCommand();
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.CommandText = "SP_WEB_RH_LISTA_JUSTIFICATIVA";

                sqlCommand.Parameters.AddWithValue("@NR_RESPONSAVEL", NR_RESPONSAVEL);
                sqlCommand.Parameters.AddWithValue("@DT_MESREF", DT_MESREF);
                sqlCommand.Parameters.AddWithValue("@TP_SUP_STATUS", TP_SUP_STATUS);
                sqlCommand.Parameters.AddWithValue("@TP_GER_STATUS", TP_GER_STATUS);
                sqlCommand.Parameters.AddWithValue("@TP_RH_STATUS", TP_RH_STATUS);
                sqlCommand.Parameters.AddWithValue("@TP_MOTIVO", TP_MOTIVO);

                sqlCommand.Parameters.AddWithValue("@NR_SUPERVISOR", NR_SUPERVISOR);
                sqlCommand.Parameters.AddWithValue("@NR_GESTOR", NR_GESTOR);
                sqlCommand.Parameters.AddWithValue("@NR_RH", NR_RH);

                sqlCommand.Parameters.AddWithValue("@NM_COLABORADOR", NM_COLABORADOR != "" ? string.Format("%{0}%", NM_COLABORADOR) : "");
                
                DAL_MIS AcessaDadosMis = new Intranet.DAL.DAL_MIS();
                return AcessaDadosMis.ConsultaSQL(sqlCommand);
            }
            catch (Exception ex)
            {
                throw new Exception("RH.SolicitacaoJustificativa_002: " + ex.Message, ex);
            }
        }

        public Intranet_NEW.Models.RH.SolicitacaoJustificativa DetalheSolicitacao(string NR_SOLICITACAO)
        {
            try
            {
                SqlCommand sqlCommand = new SqlCommand();
                sqlCommand.CommandType = CommandType.Text;
                sqlCommand.CommandText = "SELECT \n"
                                        + "	  A.* \n"
                                        + "	, B.NM_COLABORADOR AS [NM_SOLICITANTE] \n"

                                        + "	, C.NR_COLABORADOR AS [NR_SUP_RESPONSAVEL] \n"
                                        + "	, C.NM_COLABORADOR AS [NM_SUP_RESPONSAVEL] \n"

                                        + "	, D.NR_COLABORADOR AS [NR_GER_RESPONSAVEL] \n"
                                        + "	, D.NM_COLABORADOR AS [NM_GER_RESPONSAVEL] \n"

                                        + "	, E.NR_COLABORADOR AS [NR_RH_RESPONSAVEL] \n"
                                        + "	, E.NM_COLABORADOR AS [NM_RH_RESPONSAVEL] \n"

                                        + "FROM TBL_WEB_RH_PONTO_JUSTIFICATIVA     A \n"
                                        + "	LEFT JOIN TBL_WEB_COLABORADOR_DADOS B ON B.NR_COLABORADOR = A.NR_SOLICITANTE \n"
                                        + "	LEFT JOIN TBL_WEB_COLABORADOR_DADOS C ON C.NR_COLABORADOR = A.NR_SUP_RESPONSAVEL \n"
                                        + "	LEFT JOIN TBL_WEB_COLABORADOR_DADOS D ON D.NR_COLABORADOR = A.NR_GER_RESPONSAVEL \n"
                                        + "	LEFT JOIN TBL_WEB_COLABORADOR_DADOS E ON D.NR_COLABORADOR = A.NR_RH_RESPONSAVEL \n"
                                        + "WHERE A.NR_SOLICITACAO = @NR_SOLICITACAO \n";

                sqlCommand.Parameters.AddWithValue("@NR_SOLICITACAO", NR_SOLICITACAO);

                DAL_MIS AcessaDadosMis = new Intranet.DAL.DAL_MIS();
                DataSet ds = AcessaDadosMis.ConsultaSQL(sqlCommand);

                Intranet_NEW.Models.RH.SolicitacaoJustificativa dto = new DTO.RH.SolicitacaoJustificativa();
                if ((ds.Tables.Count > 0) && (ds.Tables[0].Rows.Count > 0))
                {
                    DataRow dr = ds.Tables[0].Rows[0];

                    dto.NR_SOLICITACAO = dr["NR_SOLICITACAO"].ToString();
                    dto.DT_SOLICITACAO = ((DateTime)dr["DT_SOLICITACAO"]).ToString("dd/MM/yyyy");
                    dto.NR_SOLICITANTE = dr["NR_SOLICITANTE"].ToString();
                    dto.NM_SOLICITANTE = dr["NM_SOLICITANTE"].ToString();
                    dto.DT_MESREF = dr["DT_MESREF"].ToString();

                    dto.NM_RECEBIDO_POR = dr["NM_RECEBIDO_POR"].ToString();

                    dto.NR_SUP_RESPONSAVEL = dr["NR_SUP_RESPONSAVEL"].ToString();
                    dto.NM_SUP_RESPONSAVEL = dr["NM_SUP_RESPONSAVEL"].ToString();
                    dto.TP_SUP_STATUS = dr["TP_SUP_STATUS"].ToString();
                    dto.DT_SUP_ANALISE = dr["DT_SUP_ANALISE"] != DBNull.Value ? ((DateTime)dr["DT_SUP_ANALISE"]).ToString("dd/MM/yyyy") : "";
                    dto.DS_SUP_OBSERVACAO = dr["DS_SUP_OBSERVACAO"].ToString();

                    dto.NR_GER_RESPONSAVEL = dr["NR_GER_RESPONSAVEL"].ToString();
                    dto.NM_GER_RESPONSAVEL = dr["NM_GER_RESPONSAVEL"].ToString();
                    dto.TP_GER_STATUS = dr["TP_GER_STATUS"].ToString();
                    dto.DT_GER_ANALISE = dr["DT_GER_ANALISE"] != DBNull.Value ? ((DateTime)dr["DT_GER_ANALISE"]).ToString("dd/MM/yyyy") : "";
                    dto.DS_GER_OBSERVACAO = dr["DS_GER_OBSERVACAO"].ToString();

                    dto.NR_RH_RESPONSAVEL = dr["NR_RH_RESPONSAVEL"].ToString();
                    dto.NM_RH_RESPONSAVEL = dr["NM_RH_RESPONSAVEL"].ToString();
                    dto.TP_RH_STATUS = dr["TP_RH_STATUS"].ToString();
                    dto.DT_RH_ANALISE = dr["DT_RH_ANALISE"] != DBNull.Value ? ((DateTime)dr["DT_RH_ANALISE"]).ToString("dd/MM/yyyy") : "";
                    dto.DS_RH_OBSERVACAO = dr["DS_RH_OBSERVACAO"].ToString();

                    dto.DT_MARCACAO = dr["DT_MARCACAO"] != DBNull.Value ? ((DateTime)dr["DT_MARCACAO"]).ToString("dd/MM/yyyy") : "";
                    dto.NM_JUSTIFICATIVA = dr["NM_JUSTIFICATIVA"].ToString();
                    dto.TP_MOTIVO = dr["TP_MOTIVO"].ToString();
                }
                return dto;
            }
            catch (Exception ex)
            {
                throw new Exception("RH.SolicitacaoJustificativa_003: " + ex.Message, ex);
            }
        }

        public int AtualizaJustificativa(Intranet_NEW.Models.RH.SolicitacaoJustificativa dto)
        {
            try
            {
                SqlCommand sqlCommand = new SqlCommand();
                sqlCommand.CommandType = CommandType.Text;
                sqlCommand.CommandText = "UPDATE TBL_WEB_RH_PONTO_JUSTIFICATIVA SET \n"
                                        + "	 --  DT_SOLICITACAO = @DT_SOLICITACAO \n"
                                        + "	 --, NR_SOLICITANTE = @NR_SOLICITANTE \n"
                                        + "	 --, DT_MESREF = @DT_MESREF \n"
                                        + "	 --, DT_MARCACAO = @DT_MARCACAO \n"
                                        + "	 --, NM_JUSTIFICATIVA = @NM_JUSTIFICATIVA \n"

                                        + "	   NR_SUP_RESPONSAVEL = @NR_SUP_RESPONSAVEL \n"
                                        + "	 , TP_SUP_STATUS = @TP_SUP_STATUS \n"
                                        + "	 , DT_SUP_ANALISE = @DT_SUP_ANALISE \n"
                                        + "	 , DS_SUP_OBSERVACAO = @DS_SUP_OBSERVACAO \n"

                                        + "	 , NR_GER_RESPONSAVEL = @NR_GER_RESPONSAVEL \n"
                                        + "	 , TP_GER_STATUS = @TP_GER_STATUS \n"
                                        + "	 , DT_GER_ANALISE = @DT_GER_ANALISE \n"
                                        + "	 , DS_GER_OBSERVACAO = @DS_GER_OBSERVACAO \n"

                                        + "	 , NR_RH_RESPONSAVEL = @NR_RH_RESPONSAVEL \n"
                                        + "	 , TP_RH_STATUS = @TP_RH_STATUS \n"
                                        + "	 , DT_RH_ANALISE = @DT_RH_ANALISE \n"
                                        + "	 , DS_RH_OBSERVACAO = @DS_RH_OBSERVACAO \n"

                                        + "WHERE \n"
                                        + "	   NR_SOLICITACAO = @NR_SOLICITACAO \n";

                sqlCommand.Parameters.AddWithValue("@NR_SOLICITACAO", dto.NR_SOLICITACAO != "" ? (object)dto.NR_SOLICITACAO : DBNull.Value);

                //sqlCommand.Parameters.AddWithValue("@DT_SOLICITACAO", dto.DT_SOLICITACAO != "" ? (object)dto.DT_SOLICITACAO : DBNull.Value);
                //sqlCommand.Parameters.AddWithValue("@NR_SOLICITANTE", dto.NR_SOLICITANTE != "" ? (object)dto.NR_SOLICITANTE : DBNull.Value);
                //sqlCommand.Parameters.AddWithValue("@DT_MESREF", dto.DT_MESREF != "" ? (object)dto.DT_MESREF : DBNull.Value);
                //sqlCommand.Parameters.AddWithValue("@DT_MARCACAO", dto.DT_MARCACAO != "" ? (object)dto.DT_MARCACAO : DBNull.Value);
                //sqlCommand.Parameters.AddWithValue("@NM_JUSTIFICATIVA", dto.NM_JUSTIFICATIVA != "" ? (object)dto.NM_JUSTIFICATIVA : DBNull.Value);

                sqlCommand.Parameters.AddWithValue("@NR_SUP_RESPONSAVEL", dto.NR_SUP_RESPONSAVEL != "" ? (object)dto.NR_SUP_RESPONSAVEL : DBNull.Value);
                sqlCommand.Parameters.AddWithValue("@TP_SUP_STATUS", dto.TP_SUP_STATUS != "" ? (object)dto.TP_SUP_STATUS : 0);
                sqlCommand.Parameters.AddWithValue("@DT_SUP_ANALISE", dto.DT_SUP_ANALISE != "" ? (object)DateTime.Parse(dto.DT_SUP_ANALISE) : DBNull.Value);
                sqlCommand.Parameters.AddWithValue("@DS_SUP_OBSERVACAO", dto.DS_SUP_OBSERVACAO != "" ? (object)dto.DS_SUP_OBSERVACAO : DBNull.Value);

                sqlCommand.Parameters.AddWithValue("@NR_GER_RESPONSAVEL", dto.NR_GER_RESPONSAVEL != "" ? (object)dto.NR_GER_RESPONSAVEL : DBNull.Value);
                sqlCommand.Parameters.AddWithValue("@TP_GER_STATUS", dto.TP_GER_STATUS != "" ? (object)dto.TP_GER_STATUS : 0);
                sqlCommand.Parameters.AddWithValue("@DT_GER_ANALISE", dto.DT_GER_ANALISE != "" ? (object)DateTime.Parse(dto.DT_GER_ANALISE) : DBNull.Value);
                sqlCommand.Parameters.AddWithValue("@DS_GER_OBSERVACAO", dto.DS_GER_OBSERVACAO != "" ? (object)dto.DS_GER_OBSERVACAO : DBNull.Value);

                sqlCommand.Parameters.AddWithValue("@NR_RH_RESPONSAVEL", dto.NR_RH_RESPONSAVEL != "" ? (object)dto.NR_RH_RESPONSAVEL : DBNull.Value);
                sqlCommand.Parameters.AddWithValue("@TP_RH_STATUS", dto.TP_RH_STATUS != "" ? (object)dto.TP_RH_STATUS : 0);
                sqlCommand.Parameters.AddWithValue("@DT_RH_ANALISE", dto.DT_RH_ANALISE != "" ? (object)DateTime.Parse(dto.DT_RH_ANALISE) : DBNull.Value);
                sqlCommand.Parameters.AddWithValue("@DS_RH_OBSERVACAO", dto.DS_RH_OBSERVACAO != "" ? (object)dto.DS_RH_OBSERVACAO : DBNull.Value);

                DAL_MIS AcessaDadosMis = new Intranet.DAL.DAL_MIS();
                return AcessaDadosMis.ExecutaComandoSQL(sqlCommand);
            }
            catch (Exception ex)
            {
                throw new Exception("RH.SolicitacaoJustificativa_004: " + ex.Message, ex);
            }
        }
    }
}