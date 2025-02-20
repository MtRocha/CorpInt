using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI;
using Intranet_NEW.Controllers.DAL;
using Intranet_NEW.Models.WEB;

namespace Intranet.BLL.WEB
{
    public class PendenciasTI
    {

        public DataSet ListaPendencias()
        {
            try
            {
                SqlCommand sqlcommand = new SqlCommand();
                sqlcommand.CommandType = CommandType.StoredProcedure;
                sqlcommand.CommandText = "SP_WEB_TI_LISTA_PENDENCIAS";

                DAL_MIS AcessaDadosMis = new DAL.DAL_MIS();
                return AcessaDadosMis.ConsultaSQL(sqlcommand);
            }
            catch (Exception ex)
            {
                throw new Exception("BLL.WEB.PendenciasTI_001: " + ex.Message, ex);
            }
        }

        public Pendencia DadosPendencia(int ID_PENDENCIA)
        {
            try
            {
                SqlCommand sqlcommand = new SqlCommand();
                sqlcommand.CommandType = CommandType.Text;
                sqlcommand.Parameters.AddWithValue("@ID_PENDENCIA", ID_PENDENCIA);
                sqlcommand.CommandText = "SELECT * FROM TBL_PENDENCIAS WHERE ID_PENDENCIA = @ID_PENDENCIA \n";

                DAL_MIS AcessaDadosMis = new DAL.DAL_MIS();
                DataSet ds = AcessaDadosMis.ConsultaSQL(sqlcommand);

                Pendencia dto = new DTO.WEB.Pendencia();
                if ((ds.Tables.Count > 0) && (ds.Tables[0].Rows.Count > 0))
                {
                    DataRow dr = ds.Tables[0].Rows[0];

                    dto.ID_PENDENCIA = dr["ID_PENDENCIA"] != DBNull.Value ? dr["ID_PENDENCIA"].ToString() : "";
                    dto.NR_TECNICO = dr["NR_TECNICO"] != DBNull.Value ? dr["NR_TECNICO"].ToString() : "";
                    dto.TAREFA = dr["TAREFA"] != DBNull.Value ? dr["TAREFA"].ToString() : "";
                    dto.DESCRICAO = dr["DESCRICAO"] != DBNull.Value ? dr["DESCRICAO"].ToString() : "";
                    dto.OBSERVACAO = dr["OBSERVACAO"] != DBNull.Value ? dr["OBSERVACAO"].ToString() : "";
                    dto.LOCAL = dr["LOCAL"] != DBNull.Value ? dr["LOCAL"].ToString() : "";
                    dto.STATUS = dr["STATUS"] != DBNull.Value ? dr["STATUS"].ToString() : "";
                    dto.NM_SOLICITANTE = dr["NM_SOLICITANTE"] != DBNull.Value ? dr["NM_SOLICITANTE"].ToString() : "";
                    dto.USUARIO_ALTERACAO = dr["USUARIO_ALTERACAO"] != DBNull.Value ? dr["USUARIO_ALTERACAO"].ToString() : "";

                    return dto;
                }
                return dto;
            }
            catch (Exception ex)
            {
                throw new Exception("BLL.WEB.PendenciasTI_002: " + ex.Message);
            }
        }

        public int GravaPendencia(Pendencia dto)
        {
            SqlCommand sqlcommand = new SqlCommand();
            sqlcommand.CommandType = CommandType.Text;

            sqlcommand.Parameters.AddWithValue("@TAREFA", dto.TAREFA);
            sqlcommand.Parameters.AddWithValue("@DESCRICAO", dto.DESCRICAO);
            sqlcommand.Parameters.AddWithValue("@NR_TECNICO", int.Parse(dto.NR_TECNICO));
            sqlcommand.Parameters.AddWithValue("@OBSERVACAO", dto.OBSERVACAO);
            sqlcommand.Parameters.AddWithValue("@LOCAL", dto.LOCAL);
            sqlcommand.Parameters.AddWithValue("@STATUS", dto.STATUS);
            sqlcommand.Parameters.AddWithValue("@DT_ALTERACAO", DateTime.Now);
            sqlcommand.Parameters.AddWithValue("@NM_SOLICITANTE", dto.NM_SOLICITANTE);
            sqlcommand.Parameters.AddWithValue("@USUARIO_ALTERACAO", dto.USUARIO_ALTERACAO);

            if (dto.ID_PENDENCIA == "")
                return Novo(sqlcommand, dto);
            else
                return Atualizar(sqlcommand, dto.ID_PENDENCIA);
        }

        private int Atualizar(SqlCommand sqlcommand, string id_pendencia)
        {
            try
            {
                sqlcommand.CommandText = "UPDATE TBL_PENDENCIAS SET \n"
                                        + "  NR_TECNICO = @NR_TECNICO,TAREFA = @TAREFA, DESCRICAO = @DESCRICAO, OBSERVACAO = @OBSERVACAO,\n"
                                        + "LOCAL = @LOCAL,STATUS = @STATUS,DT_ALTERACAO = @DT_ALTERACAO, USUARIO_ALTERACAO = @USUARIO_ALTERACAO \n"
                                        + "WHERE ID_PENDENCIA = " + id_pendencia;

                DAL_MIS AcessaDadosMis = new DAL.DAL_MIS();
                if (AcessaDadosMis.ExecutaComandoSQL(sqlcommand) == 1)
                {
                    EnviarEmail(DadosPendencia(int.Parse(id_pendencia)));
                    return 1;
                }
                else return 0;

            }
            catch (Exception ex)
            {
                throw new Exception("BLL.WEB.PendenciasTI_003: " + ex.Message, ex);
            }
        }

        private int Novo(SqlCommand sqlcommand, Pendencia dto)
        {
            try
            {
                sqlcommand.CommandText = "INSERT INTO TBL_PENDENCIAS \n"
                                        + "        (NR_TECNICO, TAREFA, DESCRICAO, OBSERVACAO, LOCAL, STATUS, DT_ALTERACAO, NM_SOLICITANTE, USUARIO_ALTERACAO) \n"
                                        + "VALUES  (@NR_TECNICO, @TAREFA, @DESCRICAO, @OBSERVACAO, @LOCAL, @STATUS, @DT_ALTERACAO, @NM_SOLICITANTE, @USUARIO_ALTERACAO)";

                DAL_MIS AcessaDadosMis = new DAL.DAL_MIS();
                if (AcessaDadosMis.ExecutaComandoSQL(sqlcommand) == 1)
                {
                    EnviarEmail(dto, true);
                    return 1;
                }
                else return 0;
            }
            catch (Exception ex)
            {
                throw new Exception("BLL.WEB.PendenciasTI_004: " + ex.Message, ex);
            }
        }

        public int EnviarEmail(Pendencia pendencia, bool novo = false)
        {
            string[] nm_tecnico = NomeEmailTecnico(pendencia.NR_TECNICO);
            string emailSolicitante = EmailSolicitante(pendencia.NM_SOLICITANTE);
            string status = StatusPendencia(pendencia.STATUS);
            try
            {
                System.Net.Mail.MailMessage objEmail = new System.Net.Mail.MailMessage();
                if (status == "Finalizado")
                {
                    if (emailSolicitante == "") return -1;
                    objEmail.To.Add(emailSolicitante);
                }
                else if (status == "Aberto") objEmail.To.Add(nm_tecnico[1].ToLower());
                else return -1;
                objEmail.Subject = "[INTRANET] Atualização na lista de Pendências ";
                objEmail.Body = GeraCorpoEmail(pendencia, nm_tecnico, status, objEmail, novo);

                MAIL mail = new Intranet.DAL.MAIL();

                if (mail.Enviar(objEmail, "pendências") > 0)
                    return (1);
                return (0);
            }
            catch (Exception ex)
            {
                throw new Exception("EMAIL_PENDENCIAS_001: " + ex.Message, ex);
            }
        }

        private string GeraCorpoEmail(Pendencia pendencia, string[] nm_tecnico, string status, System.Net.Mail.MailMessage objEmail, bool novo)
        {
            string conteudo = "";

            if (status == "Finalizado")
            {
                conteudo += "<b>Prezado(a) " + pendencia.NM_SOLICITANTE + ",</b>";
                conteudo += "<br><br>";
                conteudo += "Uma tarefa solicitada por você foi finalizada!";
                conteudo += "<br><br><b>Dados da Pendência:</b>";
                conteudo += "<br>Tarefa: " + pendencia.TAREFA;
                conteudo += "<br>Descrição: " + pendencia.DESCRICAO;
                conteudo += "<br>Status: " + status;
                conteudo += "<br>Usuário de alteração: " + pendencia.USUARIO_ALTERACAO;
                conteudo += "<br><br>";
                conteudo += "E-mail enviado pelo sistema, por favor não responder.";
                conteudo += "<br>";
                conteudo += "Enviado em: " + DateTime.Now.ToString("dd/MM/yyyy HH:mm");
            }
            else if (novo)
            {
                conteudo += "<b>Prezado(a) " + nm_tecnico[0] + ",</b>";
                conteudo += "<br><br>";
                conteudo += "Uma tarefa foi adicionada à sua lista de pendências, favor verificar!";
                conteudo += "<br><br><b>Dados da Pendência:</b>";
                conteudo += "<br>Tarefa: " + pendencia.TAREFA;
                conteudo += "<br>Descrição: " + pendencia.DESCRICAO;
                conteudo += "<br>Status: " + status;
                conteudo += "<br>Solicitante: " + pendencia.NM_SOLICITANTE;
                conteudo += "<br><br>";
                conteudo += "E-mail enviado pelo sistema, por favor não responder.";
                conteudo += "<br>";
                conteudo += "Enviado em: " + DateTime.Now.ToString("dd/MM/yyyy HH:mm");
            }
            else
            {
                conteudo += "<b>Prezado(a) " + nm_tecnico[0] + ",</b>";
                conteudo += "<br><br>";
                conteudo += "Sua lista de pendências foi atualizada, favor verificar!";
                conteudo += "<br><br><b>Dados da Pendência:</b>";
                conteudo += "<br>Tarefa: " + pendencia.TAREFA;
                conteudo += "<br>Descrição: " + pendencia.DESCRICAO;
                conteudo += "<br>Status: " + status;
                conteudo += "<br>Usuário de alteração: " + pendencia.USUARIO_ALTERACAO;
                conteudo += "<br><br>";
                conteudo += "E-mail enviado pelo sistema, por favor não responder.";
                conteudo += "<br>";
                conteudo += "Enviado em: " + DateTime.Now.ToString("dd/MM/yyyy HH:mm");
            }
            return conteudo;
        }

        private string EmailSolicitante(string NM_SOLICITANTE)
        {
            if (NM_SOLICITANTE == null || string.IsNullOrEmpty(NM_SOLICITANTE)) return "";
            else if (NM_SOLICITANTE.Contains("JOSIELHO")) return "jmorais@exitobr.com.br";
            else if (NM_SOLICITANTE.Contains("LUCAS")) return "lpinaco@exitobr.com.br";
            else if (NM_SOLICITANTE.Contains("CAIO")) return "cguize@exitobr.com.br";
            else if (NM_SOLICITANTE.Contains("NICOLLAS")) return "nicollas.oliveira@exitobr.com.br";
            else if (NM_SOLICITANTE.Contains("LEANDRO")) return "suporte@exitobr.com.br";
            else if (NM_SOLICITANTE.Contains("THIAGO")) return "thiago@exitobr.com.br";
            else return "";
        }

        private string StatusPendencia(string STATUS)
        {
            if (STATUS == "0") return "Aberto";
            else if (STATUS == "1") return "Em Atendimento";
            else return "Finalizado";
        }

        private string[] NomeEmailTecnico(string NR_TECNICO)
        {
            switch (NR_TECNICO)
            {
                case "13":
                    return new string[] { "THIAGO MORAIS DA SILVA", "thiago@exitobr.com.br" };
                case "104":
                    return new string[] { "CAIO VINICIUS GUIZE", "cguize@exitobr.com.br" };
                case "392":
                    return new string[] { "LEANDRO BARROS DOS SANTOS", "suporte@exitobr.com.br" };
                case "1962":
                    return new string[] { "NICOLLAS OLIVEIRA", "nicollas.oliveira@exitobr.com.br" };
                case "6548":
                    return new string[] { "LUCAS OLIVEIRA PINACO", "lpinaco@exitobr.com.br" };
                case "3":
                    return new string[] { "JOSIELHO DELFINO DE MORAIS", "jmorais@exitobr.com.br" };
                default:
                    return new string[] { "", "" };
            }
        }
    }
}
