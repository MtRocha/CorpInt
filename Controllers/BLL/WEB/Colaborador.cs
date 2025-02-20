using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;
using Intranet_NEW.Models.WEB;
using Intranet_NEW.Controllers.DAL;

namespace Intranet.BLL.WEB
{
    public class Colaborador : ColaboradorAcesso
    {
        #region Dados cadastrais do colaborador

        public DataSet ListaColaborador(string NR_EMPRESA, string NR_FILIAL, string NM_COLABORADOR, string TP_STATUS, string TP_FUNCAO, string TP_NOVO, string NR_COORDENADOR, string NR_SUPERVISOR)
        {
            try
            {
                SqlCommand sqlcommand = new SqlCommand();
                sqlcommand.CommandType = CommandType.StoredProcedure;
                sqlcommand.Parameters.AddWithValue("@NM_COLABORADOR", NM_COLABORADOR);
                sqlcommand.Parameters.AddWithValue("@TP_STATUS", TP_STATUS);
                sqlcommand.Parameters.AddWithValue("@NR_EMPRESA", NR_EMPRESA);
                sqlcommand.Parameters.AddWithValue("@NR_FILIAL", NR_FILIAL);
                sqlcommand.Parameters.AddWithValue("@TP_FUNCAO", TP_FUNCAO);
                sqlcommand.Parameters.AddWithValue("@TP_NOVO", TP_NOVO);

                sqlcommand.Parameters.AddWithValue("@NR_COORDENADOR", NR_COORDENADOR);
                sqlcommand.Parameters.AddWithValue("@NR_SUPERVISOR", NR_SUPERVISOR);

                sqlcommand.CommandText = "SP_WEB_LISTA_COLABORADOR";

                DAL_MIS AcessaDadosMis = new DAL.DAL_MIS();
                return AcessaDadosMis.ConsultaSQL(sqlcommand);
            }
            catch (Exception ex)
            {
                throw new Exception("BLL.WEB.Operador_001: " + ex.Message, ex);
            }
        }

        public Intranet_NEW.Models.WEB.Colaborador DadosColaborador(decimal NR_COLABORADOR, string NR_CPF)
        {
            try
            {
                SqlCommand sqlcommand = new SqlCommand();
                sqlcommand.CommandType = CommandType.Text;
                sqlcommand.Parameters.AddWithValue("@NR_COLABORADOR", NR_COLABORADOR);
                sqlcommand.Parameters.AddWithValue("@NR_CPF", NR_CPF);
                sqlcommand.CommandText = "SELECT \n"
                                        + "   A.* \n"
                                        + "	, B.NM_FUNCAO		AS [NM_FUNCAO_RH] \n"
                                        + "	, C.NM_ATIVIDADE	AS [NM_ATIVIDADE_RH] \n"
                                        + "	, D.NM_JORNADA_TRAB AS [NM_JORNADA_TRAB_RH] \n"

                                        + "	, W.NM_COLABORADOR	AS [NM_GESTOR] \n"
                                        + "	, W.NR_COLABORADOR	AS [NR_GESTOR] \n"

                                        + "	, Y.NM_COLABORADOR	AS [NM_COORDENADOR] \n"
                                        + "	, Y.NR_COLABORADOR	AS [NR_COORDENADOR] \n"

                                        + "	, Z.NM_COLABORADOR	AS [NM_SUPERVISOR] \n"
                                        + "	, Z.NR_COLABORADOR	AS [NR_SUPERVISOR] \n"

                                        + "FROM TBL_WEB_COLABORADOR_DADOS A \n"
                                        + "	LEFT JOIN TBL_WEB_RH_COMBO_FUNCAO       B ON A.NR_FUNCAO_RH       = B.NR_FUNCAO \n"
                                        + "	LEFT JOIN TBL_WEB_RH_COMBO_ATIVIDADE    C ON A.NR_ATIVIDADE_RH    = C.NR_ATIVIDADE \n"
                                        + "	LEFT JOIN TBL_WEB_RH_COMBO_JORNADA_TRAB D ON A.NR_JORNADA_TRAB_RH = D.NR_JORNADA_TRAB \n"
                                        + "	LEFT JOIN TBL_WEB_COLABORADOR_DADOS W ON A.NR_GESTOR      = W.NR_COLABORADOR \n"
                                        + "	LEFT JOIN TBL_WEB_COLABORADOR_DADOS Y ON A.NR_COORDENADOR = Y.NR_COLABORADOR \n"
                                        + "	LEFT JOIN TBL_WEB_COLABORADOR_DADOS Z ON A.NR_SUPERVISOR  = Z.NR_COLABORADOR \n"
                                        + "WHERE ((@NR_COLABORADOR <> 0 AND A.NR_COLABORADOR = @NR_COLABORADOR) OR (@NR_CPF <> '' AND A.NR_CPF = @NR_CPF))  \n";

                DAL_MIS AcessaDadosMis = new DAL.DAL_MIS();
                DataSet ds = AcessaDadosMis.ConsultaSQL(sqlcommand);

                if ((ds.Tables.Count > 0) && (ds.Tables[0].Rows.Count > 0))
                {
                    DataRow dr = ds.Tables[0].Rows[0];
                    Intranet_NEW.Models.WEB.Colaborador dto = new DTO.Colaborador();

                    dto.NR_COLABORADOR = dr["NR_COLABORADOR"] != DBNull.Value ? dr["NR_COLABORADOR"].ToString() : "";
                    dto.NR_EMPRESA = dr["NR_EMPRESA"] != DBNull.Value ? dr["NR_EMPRESA"].ToString() : "";
                    dto.NR_MATRICULA = dr["NR_MATRICULA"] != DBNull.Value ? dr["NR_MATRICULA"].ToString() : "";

                    dto.NR_CATRACA_GTZ = dr["NR_CATRACA_GTZ"] != DBNull.Value ? dr["NR_CATRACA_GTZ"].ToString() : "";
                    dto.NR_CATRACA_MTZ = dr["NR_CATRACA_MTZ"] != DBNull.Value ? dr["NR_CATRACA_MTZ"].ToString() : "";

                    dto.DT_ADMISSAO = dr["DT_ADMISSAO"] != DBNull.Value ? ((DateTime)dr["DT_ADMISSAO"]).ToString("dd/MM/yyyy") : "";
                    dto.DT_DEMISSAO = dr["DT_DEMISSAO"] != DBNull.Value ? ((DateTime)dr["DT_DEMISSAO"]).ToString("dd/MM/yyyy") : "";

                    dto.DT_NASCIMENTO = dr["DT_NASCIMENTO"] != DBNull.Value ? ((DateTime)dr["DT_NASCIMENTO"]).ToString("dd/MM/yyyy") : "";
                    dto.NM_COLABORADOR = dr["NM_COLABORADOR"] != DBNull.Value ? dr["NM_COLABORADOR"].ToString() : "";
                    dto.NR_CPF = dr["NR_CPF"].ToString();
                    dto.TP_SEXO = dr["TP_SEXO"] != DBNull.Value ? dr["TP_SEXO"].ToString() : "";

                    dto.TP_GESTOR = dr["TP_GESTOR"] != DBNull.Value ? dr["TP_GESTOR"].ToString() : "0";

                    dto.NR_DDD01 = dr["NR_DDD01"] != DBNull.Value ? dr["NR_DDD01"].ToString() : "";
                    dto.NR_TELEFONE01 = dr["NR_TELEFONE01"] != DBNull.Value ? dr["NR_TELEFONE01"].ToString() : "";

                    dto.NR_DDD02 = dr["NR_DDD02"] != DBNull.Value ? dr["NR_DDD02"].ToString() : "";
                    dto.NR_TELEFONE02 = dr["NR_TELEFONE02"] != DBNull.Value ? dr["NR_TELEFONE02"].ToString() : "";

                    dto.NR_RAMAL = dr["NR_RAMAL"] != DBNull.Value ? dr["NR_RAMAL"].ToString() : "";
                    dto.NM_EMAIL = dr["NM_EMAIL"] != DBNull.Value ? dr["NM_EMAIL"].ToString() : "";
                    dto.TP_FUNCAO = dr["TP_FUNCAO"] != DBNull.Value ? dr["TP_FUNCAO"].ToString() : "";

                    dto.TP_ACESSO_SISTEMA = dr["TP_ACESSO_SISTEMA"] != DBNull.Value ? dr["TP_ACESSO_SISTEMA"].ToString() : "";
                    dto.NM_SENHA = dr["NM_SENHA"] != DBNull.Value ? dr["NM_SENHA"].ToString() : "";
                    dto.NM_EQUIPE = dr["NM_EQUIPE"] != DBNull.Value ? dr["NM_EQUIPE"].ToString() : "";

                    dto.NR_GESTOR = dr["NR_GESTOR"] != DBNull.Value ? dr["NR_GESTOR"].ToString() : "";
                    dto.NR_FILIAL = dr["NR_FILIAL"] != DBNull.Value ? dr["NR_FILIAL"].ToString() : "";
                    dto.NM_FUNCAO_RH = dr["NM_FUNCAO_RH"] != DBNull.Value ? dr["NM_FUNCAO_RH"].ToString() : "";
                    dto.NM_ATIVIDADE_RH = dr["NM_ATIVIDADE_RH"] != DBNull.Value ? dr["NM_ATIVIDADE_RH"].ToString() : "";
                    dto.NM_JORNADA_TRAB_RH = dr["NM_JORNADA_TRAB_RH"] != DBNull.Value ? dr["NM_JORNADA_TRAB_RH"].ToString() : "";

                    dto.NR_SUPERVISOR = dr["NR_SUPERVISOR"] != DBNull.Value ? dr["NR_SUPERVISOR"].ToString() : "";
                    dto.NR_COORDENADOR = dr["NR_COORDENADOR"] != DBNull.Value ? dr["NR_COORDENADOR"].ToString() : "";

                    dto.NR_OLOS = dr["NR_OLOS"] != DBNull.Value ? dr["NR_OLOS"].ToString() : "";
                    dto.NM_LOGIN_OLOS = dr["NM_LOGIN_OLOS"] != DBNull.Value ? dr["NM_LOGIN_OLOS"].ToString() : "";

                    dto.NR_OLOS_MTZ = dr["NR_OLOS_MTZ"] != DBNull.Value ? dr["NR_OLOS_MTZ"].ToString() : "";
                    dto.NM_LOGIN_OLOS_MTZ = dr["NM_LOGIN_OLOS_MTZ"] != DBNull.Value ? dr["NM_LOGIN_OLOS_MTZ"].ToString() : "";

                    dto.TP_PAVIMENTO = dr["TP_PAVIMENTO"] != DBNull.Value ? dr["TP_PAVIMENTO"].ToString() : "";
                    dto.TP_TURNO = dr["TP_TURNO"] != DBNull.Value ? dr["TP_TURNO"].ToString() : "";

                    dto.TP_STATUS = dr["TP_STATUS"] != DBNull.Value ? dr["TP_STATUS"].ToString() : "";
                    dto.DT_STATUS01 = dr["DT_STATUS01"] != DBNull.Value ? ((DateTime)dr["DT_STATUS01"]).ToString("dd/MM/yyyy") : "";
                    dto.DT_STATUS02 = dr["DT_STATUS02"] != DBNull.Value ? ((DateTime)dr["DT_STATUS02"]).ToString("dd/MM/yyyy") : "";

                    dto.NM_OBSERVACAO = dr["NM_OBSERVACAO"] != DBNull.Value ? dr["NM_OBSERVACAO"].ToString() : "";

                    dto.NM_GESTOR = dr["NM_GESTOR"] != DBNull.Value ? dr["NM_GESTOR"].ToString() : "";
                    dto.NM_SUPERVISOR = dr["NM_SUPERVISOR"] != DBNull.Value ? dr["NM_SUPERVISOR"].ToString() : "";
                    dto.NM_COORDENADOR = dr["NM_COORDENADOR"] != DBNull.Value ? dr["NM_COORDENADOR"].ToString() : "";

                    return dto;
                }
                return new DTO.Colaborador();
            }
            catch (Exception ex)
            {
                throw new Exception("BLL.WEB.Operador_002: " + ex.Message, ex);
            }
        }

        public int GravaColaborador(Intranet_NEW.Models.WEB.Colaborador dto)
        {
            try
            {
                SqlCommand sqlcommand = new SqlCommand();
                sqlcommand.CommandType = CommandType.Text;

                DateTime dt = DateTime.MinValue;
                int num = 0;

                sqlcommand.Parameters.AddWithValue("@NR_COLABORADOR", int.TryParse(dto.NR_COLABORADOR, out num) ? (object)int.Parse(dto.NR_COLABORADOR) : DBNull.Value);
                sqlcommand.Parameters.AddWithValue("@NR_EMPRESA", dto.NR_EMPRESA);
                sqlcommand.Parameters.AddWithValue("@NR_MATRICULA", dto.NR_MATRICULA != "" ? (object)dto.NR_MATRICULA : DBNull.Value);

                sqlcommand.Parameters.AddWithValue("@NR_CATRACA_GTZ", int.TryParse(dto.NR_CATRACA_GTZ, out num) ? (object)int.Parse(dto.NR_CATRACA_GTZ) : DBNull.Value);
                sqlcommand.Parameters.AddWithValue("@NR_CATRACA_MTZ", int.TryParse(dto.NR_CATRACA_MTZ, out num) ? (object)int.Parse(dto.NR_CATRACA_MTZ) : DBNull.Value);

                sqlcommand.Parameters.AddWithValue("@DT_ADMISSAO", DateTime.TryParse(dto.DT_ADMISSAO, out dt) ? (object)DateTime.Parse(dto.DT_ADMISSAO) : DBNull.Value);
                sqlcommand.Parameters.AddWithValue("@DT_DEMISSAO", DateTime.TryParse(dto.DT_DEMISSAO, out dt) ? (object)DateTime.Parse(dto.DT_DEMISSAO) : DBNull.Value);

                sqlcommand.Parameters.AddWithValue("@DT_NASCIMENTO", DateTime.TryParse(dto.DT_NASCIMENTO, out dt) ? (object)DateTime.Parse(dto.DT_NASCIMENTO) : DBNull.Value);
                sqlcommand.Parameters.AddWithValue("@NM_COLABORADOR", dto.NM_COLABORADOR.ToUpper());
                sqlcommand.Parameters.AddWithValue("@NR_CPF", dto.NR_CPF);
                sqlcommand.Parameters.AddWithValue("@TP_SEXO", dto.TP_SEXO);
                sqlcommand.Parameters.AddWithValue("@TP_GESTOR", dto.TP_GESTOR);

                if (dto.NR_TELEFONE01 != "")
                {
                    dto.NR_TELEFONE01 = dto.NR_TELEFONE01.Replace("(", "").Replace(")", "").Replace("-", "");
                    sqlcommand.Parameters.AddWithValue("@NR_DDD01", int.Parse(dto.NR_TELEFONE01.Split(' ')[0]));
                    sqlcommand.Parameters.AddWithValue("@NR_TELEFONE01", int.Parse(dto.NR_TELEFONE01.Split(' ')[1]));
                }
                else
                {
                    sqlcommand.Parameters.AddWithValue("@NR_DDD01", DBNull.Value);
                    sqlcommand.Parameters.AddWithValue("@NR_TELEFONE01", DBNull.Value);
                }

                if (dto.NR_TELEFONE02 != "")
                {
                    dto.NR_TELEFONE02 = dto.NR_TELEFONE02.Replace("(", "").Replace(")", "").Replace("-", "");
                    sqlcommand.Parameters.AddWithValue("@NR_DDD02", int.Parse(dto.NR_TELEFONE02.Split(' ')[0]));
                    sqlcommand.Parameters.AddWithValue("@NR_TELEFONE02", int.Parse(dto.NR_TELEFONE02.Split(' ')[1]));
                }
                else
                {
                    sqlcommand.Parameters.AddWithValue("@NR_DDD02", DBNull.Value);
                    sqlcommand.Parameters.AddWithValue("@NR_TELEFONE02", DBNull.Value);
                }

                sqlcommand.Parameters.AddWithValue("@NR_GESTOR", int.TryParse(dto.NR_GESTOR, out num) ? (object)int.Parse(dto.NR_GESTOR) : DBNull.Value);
                sqlcommand.Parameters.AddWithValue("@NR_FILIAL", dto.NR_FILIAL);

                sqlcommand.Parameters.AddWithValue("@TP_ACESSO_SISTEMA", ""); // dto.TP_ACESSO_SISTEMA);

                sqlcommand.Parameters.AddWithValue("@NR_RAMAL", int.TryParse(dto.NR_RAMAL, out num) ? (object)int.Parse(dto.NR_RAMAL) : DBNull.Value);
                sqlcommand.Parameters.AddWithValue("@NM_EMAIL", dto.NM_EMAIL.ToLower());
                sqlcommand.Parameters.AddWithValue("@TP_FUNCAO", int.TryParse(dto.TP_FUNCAO, out num) ? (object)int.Parse(dto.TP_FUNCAO) : DBNull.Value);

                sqlcommand.Parameters.AddWithValue("@NR_SUPERVISOR", int.TryParse(dto.NR_SUPERVISOR, out num) ? (object)int.Parse(dto.NR_SUPERVISOR) : DBNull.Value);
                sqlcommand.Parameters.AddWithValue("@NR_COORDENADOR", int.TryParse(dto.NR_COORDENADOR, out num) ? (object)int.Parse(dto.NR_COORDENADOR) : DBNull.Value);
                sqlcommand.Parameters.AddWithValue("@NM_EQUIPE", dto.NM_EQUIPE != "" ? (object)dto.NM_EQUIPE : DBNull.Value);

                if (dto.NM_LOGIN_OLOS != "")
                {
                    dto.NR_OLOS = VerificaCpfUsuarioOlos_GTZ(dto.NR_CPF, dto.NM_LOGIN_OLOS).ToString();
                    if (dto.NR_OLOS == "0" && (dto.TP_FUNCAO == "4" || dto.TP_FUNCAO == "10" || dto.TP_FUNCAO == "11"))
                        return -2;
                }

                if (dto.NM_LOGIN_OLOS_MTZ != "")
                {
                    dto.NR_OLOS_MTZ = VerificaCpfUsuarioOlos_MTZ(dto.NR_CPF, dto.NM_LOGIN_OLOS_MTZ).ToString();
                    if (dto.NR_OLOS_MTZ == "0" && (dto.TP_FUNCAO == "4" || dto.TP_FUNCAO == "10" || dto.TP_FUNCAO == "11"))
                        return -2;
                }

                sqlcommand.Parameters.AddWithValue("@NR_OLOS", int.TryParse(dto.NR_OLOS, out num) ? (object)int.Parse(dto.NR_OLOS) : DBNull.Value);
                sqlcommand.Parameters.AddWithValue("@NM_LOGIN_OLOS", dto.NM_LOGIN_OLOS.ToUpper());

                sqlcommand.Parameters.AddWithValue("@NR_OLOS_MTZ", int.TryParse(dto.NR_OLOS_MTZ, out num) ? (object)int.Parse(dto.NR_OLOS_MTZ) : DBNull.Value);
                sqlcommand.Parameters.AddWithValue("@NM_LOGIN_OLOS_MTZ", dto.NM_LOGIN_OLOS_MTZ.ToUpper());

                sqlcommand.Parameters.AddWithValue("@TP_PAVIMENTO", dto.TP_PAVIMENTO);
                sqlcommand.Parameters.AddWithValue("@TP_TURNO", dto.TP_TURNO);
                sqlcommand.Parameters.AddWithValue("@TP_STATUS", int.TryParse(dto.TP_STATUS, out num) ? (object)int.Parse(dto.TP_STATUS) : DBNull.Value);
                sqlcommand.Parameters.AddWithValue("@DT_STATUS01", DateTime.TryParse(dto.DT_STATUS01, out dt) ? (object)DateTime.Parse(dto.DT_STATUS01) : DBNull.Value);
                sqlcommand.Parameters.AddWithValue("@DT_STATUS02", DateTime.TryParse(dto.DT_STATUS02, out dt) ? (object)DateTime.Parse(dto.DT_STATUS02) : DBNull.Value);

                sqlcommand.Parameters.AddWithValue("@NM_OBSERVACAO", dto.NM_OBSERVACAO.ToUpper());

                sqlcommand.Parameters.AddWithValue("@NR_USUARIO_SISTEMA", dto.NR_USUARIO_SISTEMA);
                sqlcommand.Parameters.AddWithValue("@NR_USUARIO_ALTERACAO", dto.NR_USUARIO_SISTEMA);

                if (dto.NR_COLABORADOR == "")
                {
                    return Novo(sqlcommand);
                }
                else
                {
                    if (dto.TP_FUNCAO == "11")
                        AtualizarCoordenadorEquipe(dto.NR_COORDENADOR, dto.NR_COLABORADOR);
                    return Atualizar(sqlcommand);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("BLL.WEB.Operador_003: " + ex.Message, ex);
            }
        }

        private int Novo(SqlCommand sqlcommand)
        {
            try
            {
                sqlcommand.CommandText = "INSERT INTO TBL_WEB_COLABORADOR_DADOS \n"
                                        + "        (NR_EMPRESA, NR_MATRICULA, NR_CATRACA_GTZ, NR_CATRACA_MTZ, DT_ADMISSAO, DT_DEMISSAO, DT_NASCIMENTO, NM_COLABORADOR, NR_CPF, TP_SEXO, TP_GESTOR, NR_DDD01, NR_TELEFONE01, NR_DDD02, NR_TELEFONE02, NR_RAMAL, NM_EMAIL, TP_FUNCAO, NR_GESTOR, NR_FILIAL,  NR_COORDENADOR, NR_SUPERVISOR, NM_EQUIPE, NR_OLOS, NM_LOGIN_OLOS, NR_OLOS_MTZ, NM_LOGIN_OLOS_MTZ, TP_PAVIMENTO, TP_TURNO, TP_STATUS, NM_OBSERVACAO, NR_USUARIO_INCLUSAO, DT_ULTIMA_ALTERACAO, NR_USUARIO_ALTERACAO, DT_STATUS01 , DT_STATUS02) \n"
                                        + " VALUES (@NR_EMPRESA, @NR_MATRICULA, @NR_CATRACA_GTZ, @NR_CATRACA_MTZ, @DT_ADMISSAO, @DT_DEMISSAO, @DT_NASCIMENTO, @NM_COLABORADOR, @NR_CPF, @TP_SEXO, @TP_GESTOR, @NR_DDD01, @NR_TELEFONE01, @NR_DDD02, @NR_TELEFONE02, @NR_RAMAL, @NM_EMAIL, @TP_FUNCAO, @NR_GESTOR, @NR_FILIAL,  @NR_COORDENADOR, @NR_SUPERVISOR, @NM_EQUIPE, @NR_OLOS, @NM_LOGIN_OLOS, @NR_OLOS_MTZ, @NM_LOGIN_OLOS_MTZ, @TP_PAVIMENTO, @TP_TURNO, @TP_STATUS, @NM_OBSERVACAO, @NR_USUARIO_SISTEMA, GETDATE(), @NR_USUARIO_SISTEMA, @DT_STATUS01, @DT_STATUS02)  \n";

                DAL_MIS AcessaDadosMis = new DAL.DAL_MIS();
                return AcessaDadosMis.ExecutaComandoSQL(sqlcommand);
            }
            catch (Exception ex)
            {
                throw new Exception("BLL.WEB.Operador_004: " + ex.Message, ex);
            }
        }

        private int Atualizar(SqlCommand sqlcommand)
        {
            try
            {
                sqlcommand.CommandText = "UPDATE TBL_WEB_COLABORADOR_DADOS SET \n"
                                        + "  NR_EMPRESA = @NR_EMPRESA, NR_MATRICULA = @NR_MATRICULA, NR_CATRACA_GTZ = @NR_CATRACA_GTZ, NR_CATRACA_MTZ = @NR_CATRACA_MTZ, DT_ADMISSAO = @DT_ADMISSAO, DT_DEMISSAO = @DT_DEMISSAO \n"
                                        + ", DT_NASCIMENTO = @DT_NASCIMENTO, NM_COLABORADOR = @NM_COLABORADOR, NR_CPF = @NR_CPF, TP_SEXO = @TP_SEXO \n"
                                        + ", TP_GESTOR = @TP_GESTOR, NR_DDD01 = @NR_DDD01, NR_TELEFONE01 = @NR_TELEFONE01, NR_DDD02 = @NR_DDD02, NR_TELEFONE02 = @NR_TELEFONE02, NR_RAMAL = @NR_RAMAL, NM_EMAIL = @NM_EMAIL, TP_FUNCAO = @TP_FUNCAO, NR_GESTOR = @NR_GESTOR, NR_FILIAL = @NR_FILIAL \n"
                                        + ", NR_COORDENADOR = @NR_COORDENADOR, NR_SUPERVISOR = @NR_SUPERVISOR, NM_EQUIPE = @NM_EQUIPE \n"
                                        + ", NR_OLOS = @NR_OLOS, NM_LOGIN_OLOS = @NM_LOGIN_OLOS, NR_OLOS_MTZ = @NR_OLOS_MTZ, NM_LOGIN_OLOS_MTZ = @NM_LOGIN_OLOS_MTZ \n"
                                        + ", TP_PAVIMENTO = @TP_PAVIMENTO, TP_TURNO = @TP_TURNO, TP_STATUS = @TP_STATUS, DT_STATUS01 = @DT_STATUS01, DT_STATUS02 = @DT_STATUS02 \n"
                                        + ", NM_OBSERVACAO = @NM_OBSERVACAO, DT_ULTIMA_ALTERACAO = GETDATE(), NR_USUARIO_ALTERACAO = @NR_USUARIO_SISTEMA \n"
                                        + "WHERE NR_COLABORADOR = @NR_COLABORADOR \n";

                DAL_MIS AcessaDadosMis = new DAL.DAL_MIS();
                return AcessaDadosMis.ExecutaComandoSQL(sqlcommand);
            }
            catch (Exception ex)
            {
                throw new Exception("BLL.WEB.Operador_005: " + ex.Message, ex);
            }
        }

        private void AtualizarCoordenadorEquipe(string NR_COORDENADOR, string NR_SUPERVISOR)
        {
            try
            {
                int num = 0;
                if (int.TryParse(NR_SUPERVISOR, out num) && int.TryParse(NR_COORDENADOR, out num))
                {
                    SqlCommand sqlcommand = new SqlCommand();
                    sqlcommand.CommandType = CommandType.Text;

                    sqlcommand.CommandText = "UPDATE TBL_WEB_COLABORADOR_DADOS SET NR_COORDENADOR = @NR_COORDENADOR WHERE NR_COORDENADOR <> @NR_COORDENADOR AND NR_SUPERVISOR = @NR_SUPERVISOR AND TP_FUNCAO = 10 \n";

                    sqlcommand.Parameters.AddWithValue("@NR_SUPERVISOR", (object)int.Parse(NR_SUPERVISOR));
                    sqlcommand.Parameters.AddWithValue("@NR_COORDENADOR", (object)int.Parse(NR_COORDENADOR));

                    DAL_MIS AcessaDadosMis = new DAL.DAL_MIS();
                    AcessaDadosMis.ExecutaComandoSQL(sqlcommand);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("BLL.WEB.Operador_005: " + ex.Message, ex);
            }
        }

        public ColaboradorOp DadosGestaoOperador(string NR_COLABORADOR)
        {
            try
            {
                SqlCommand sqlcommand = new SqlCommand();
                sqlcommand.CommandType = CommandType.Text;
                sqlcommand.Parameters.AddWithValue("@NR_COLABORADOR", NR_COLABORADOR);
                sqlcommand.CommandText = "SELECT \n"
                                        + "	  A.NR_COLABORADOR AS [NR_OPERADOR] \n"
                                        + "	, A.NM_COLABORADOR AS [NM_OPERADOR] \n" 
                                        + " , A.NR_OLOS        AS [NR_OLOS] "
                                        + "	, C.NR_COLABORADOR AS [NR_SUPERVIS] \n"
                                        + "	, C.NM_COLABORADOR AS [NM_SUPERVIS] \n"
                                        + "	, ISNULL(B.NR_COLABORADOR, D.NR_COLABORADOR) AS [NR_COORDENA] \n"
                                        + "	, ISNULL(B.NM_COLABORADOR, D.NM_COLABORADOR) AS [NM_COORDENA] \n"
                                        + "	, C.NM_EQUIPE	   AS [NM_EQUIPE] \n"
                                        + "FROM TBL_WEB_COLABORADOR_DADOS A \n"
                                        + "		LEFT JOIN TBL_WEB_COLABORADOR_DADOS B ON B.NR_COLABORADOR = A.NR_COORDENADOR \n"
                                        + "		LEFT JOIN TBL_WEB_COLABORADOR_DADOS C ON C.NR_COLABORADOR = A.NR_SUPERVISOR \n"
                                        + "		LEFT JOIN TBL_WEB_COLABORADOR_DADOS D ON D.NR_COLABORADOR = A.NR_GESTOR \n"
                                        + "WHERE \n"
                                        + "	A.NR_COLABORADOR = @NR_COLABORADOR \n";

                DAL_MIS AcessaDadosMis = new DAL.DAL_MIS();
                DataSet ds = AcessaDadosMis.ConsultaSQL(sqlcommand);

                ColaboradorOp dto = new DTO.ColaboradorOp();
                if ((ds.Tables.Count > 0) && (ds.Tables[0].Rows.Count > 0))
                {
                    DataRow dr = ds.Tables[0].Rows[0];

                    dto.NR_OPERADOR = dr["NR_OPERADOR"].ToString();
                    dto.NM_OPERADOR = dr["NM_OPERADOR"].ToString();
                    dto.NR_SUPERVIS = dr["NR_SUPERVIS"].ToString();
                    dto.NM_SUPERVIS = dr["NM_SUPERVIS"].ToString();
                    dto.NR_COORDENA = dr["NR_COORDENA"].ToString();
                    dto.NM_COORDENA = dr["NM_COORDENA"].ToString();
                    dto.NM_EQUIPE = dr["NM_EQUIPE"].ToString();
                    dto.NR_OLOS = Convert.ToInt32(dr["NR_OLOS"].ToString());
                }
                return dto;
            }
            catch (Exception ex)
            {
                throw new Exception("BLL.WEB.Operador_002: " + ex.Message, ex);
            }
        }

        public DataSet PesquisaColaborador(string NM_COLABORADOR)
        {
            try
            {
                SqlCommand sqlcommand = new SqlCommand();
                sqlcommand.CommandType = CommandType.StoredProcedure;
                sqlcommand.Parameters.AddWithValue("@NM_COLABORADOR", NM_COLABORADOR);
                sqlcommand.Parameters.AddWithValue("@TP_STATUS", "0");
                sqlcommand.Parameters.AddWithValue("@NR_EMPRESA", "0000");
                sqlcommand.Parameters.AddWithValue("@NR_FILIAL", "0");
                sqlcommand.Parameters.AddWithValue("@TP_FUNCAO", 253);
                sqlcommand.Parameters.AddWithValue("@TP_NOVO", 1);
                sqlcommand.Parameters.AddWithValue("@NR_COORDENADOR", 0);
                sqlcommand.Parameters.AddWithValue("@NR_SUPERVISOR", 0);

                sqlcommand.CommandText = "SP_WEB_LISTA_COLABORADOR";

                DAL_MIS AcessaDadosMis = new DAL.DAL_MIS();
                return AcessaDadosMis.ConsultaSQL(sqlcommand);
            }
            catch (Exception ex)
            {
                throw new Exception("BLL.WEB.Operador_001: " + ex.Message, ex);
            }
        }

        private int VerificaLoginIntranet(string NR_COLABORADOR, string NM_LOGIN)
        {
            try
            {
                SqlCommand sqlcommand = new SqlCommand();
                sqlcommand.CommandType = CommandType.Text;

                sqlcommand.CommandText = "SELECT NR_COLABORADOR FROM TBL_WEB_COLABORADOR_DADOS WHERE NM_LOGIN = @NM_LOGIN AND NR_COLABORADOR <> @NR_COLABORADOR \n";

                sqlcommand.Parameters.AddWithValue("@NR_COLABORADOR", NR_COLABORADOR);
                sqlcommand.Parameters.AddWithValue("@NM_LOGIN", NM_LOGIN);

                DAL_MIS AcessaDadosMis = new DAL.DAL_MIS();
                DataSet ds = AcessaDadosMis.ConsultaSQL(sqlcommand);
                if ((ds.Tables.Count > 0) && (ds.Tables[0].Rows.Count > 0))
                    return (1);
                return (0);
            }
            catch (Exception ex)
            {
                throw new Exception("BLL.WEB.Operador_005: " + ex.Message, ex);
            }
        }

        public DataSet OperadorPesquisaContrato(Int64 CPF, Int64 CONTRATO)
        {
            try
            {
                SqlCommand sqlcommand = new SqlCommand();
                sqlcommand.CommandType = CommandType.StoredProcedure;
                sqlcommand.Parameters.AddWithValue("@CPF", CPF);
                sqlcommand.Parameters.AddWithValue("@CONTRATO", CONTRATO);
                sqlcommand.CommandText = "SP_WEB_ACAO_QUITAFACIL";

                DAL_PROC AcessaDadosProc = new DAL.DAL_PROC();
                return AcessaDadosProc.ConsultaSQL(sqlcommand);
            }
            catch (Exception ex)
            {
                throw new Exception("BLL.WEB.Operador_Pesquisa: " + ex.Message, ex);
            }
        }

        #endregion Dados cadastrais do colaborador

        #region DropDownList Coordenador, Supervisor, Operador e Lista de Emails Gerencia

        public DataSet ListaColaboradorDropDownList(string NR_EMPRESA, string NR_FILIAL, int TP_FUNCAO, int TP_STATUS, string NR_SUPERVISOR, string NR_COORDENADOR)
        {
            try
            {
                SqlCommand sqlcommand = new SqlCommand();
                sqlcommand.CommandType = CommandType.Text;

                sqlcommand.Parameters.AddWithValue("@NR_EMPRESA", NR_EMPRESA);
                sqlcommand.Parameters.AddWithValue("@NR_FILIAL", NR_FILIAL);

                sqlcommand.Parameters.AddWithValue("@TP_FUNCAO", TP_FUNCAO);
                sqlcommand.Parameters.AddWithValue("@TP_STATUS", TP_STATUS);
                sqlcommand.Parameters.AddWithValue("@NR_SUPERVISOR", NR_SUPERVISOR);
                sqlcommand.Parameters.AddWithValue("@NR_COORDENADOR", NR_COORDENADOR);

                sqlcommand.CommandText = "SELECT \n"
                                        + "      a.NR_COLABORADOR \n"
                                        + "    , a.NM_COLABORADOR \n"
                                        + "FROM TBL_WEB_COLABORADOR_DADOS a \n"
                                        + "WHERE \n"
                                        + "    TP_FUNCAO = @TP_FUNCAO AND TP_STATUS = @TP_STATUS \n"
                                        + "AND TP_GESTOR = 0 \n"
                                        + "AND ((@NR_EMPRESA = 0) OR (@NR_EMPRESA <> 0 AND NR_EMPRESA = @NR_EMPRESA)) \n"
                                        + "AND ((@NR_FILIAL = 0) OR (@NR_FILIAL <> 0 AND NR_FILIAL = @NR_FILIAL)) \n"
                                        + "AND ((@NR_COORDENADOR = 0) OR (@NR_COORDENADOR <> 0 AND NR_COORDENADOR = @NR_COORDENADOR)) \n"
                                        + "AND ((@NR_SUPERVISOR = 0)  OR (@NR_SUPERVISOR <> 0 AND NR_SUPERVISOR = @NR_SUPERVISOR)) \n"

                                        + "ORDER BY a.NM_COLABORADOR \n";
                DAL_MIS AcessaDadosMis = new DAL.DAL_MIS();
                return AcessaDadosMis.ConsultaSQL(sqlcommand);
            }
            catch (Exception ex)
            {
                throw new Exception("BLL.WEB.Operador_001: " + ex.Message, ex);
            }
        }

        public DataSet BuscaMensagemColaborador(string NM_COLABORADOR)
        {
            try
            {
                SqlCommand sqlcommand = new SqlCommand();
                sqlcommand.CommandType = CommandType.StoredProcedure;
                sqlcommand.Parameters.Add("@NM_COLABORADOR", NM_COLABORADOR);
                sqlcommand.CommandText = "SP_LISTA_OPERADORES_CAIXA_ATIVOS";
                DAL_MIS AcessaDadosMis = new DAL.DAL_MIS();
                return AcessaDadosMis.ConsultaSQL(sqlcommand);
            }
            catch (Exception ex)
            {
                throw new Exception("BLL.WEB.Operador_010: " + ex.Message, ex);
            }
        }


        public DataSet ListaTodosColaboradores(string NR_EMPRESA, string NR_FILIAL, int TP_FUNCAO, int TP_STATUS)
        {
            try
            {
                SqlCommand sqlcommand = new SqlCommand();
                sqlcommand.CommandType = CommandType.StoredProcedure;
                sqlcommand.Parameters.Add("@NM_COLABORADOR",DBNull.Value);
                sqlcommand.CommandText = "SP_LISTA_OPERADORES_CAIXA_ATIVOS";
                DAL_MIS AcessaDadosMis = new DAL.DAL_MIS();
                return AcessaDadosMis.ConsultaSQL(sqlcommand);
            }
            catch (Exception ex)
            {
                throw new Exception("BLL.WEB.Operador_009: " + ex.Message, ex);
            }
        }

        public List<string> ListaEmailGerencia()
        {
            try
            {
                SqlCommand sqlcommand = new SqlCommand();
                sqlcommand.CommandType = CommandType.Text;

                sqlcommand.CommandText = "SELECT NM_EMAIL FROM TBL_WEB_COLABORADOR_DADOS WHERE TP_FUNCAO IN (6) AND NM_EMAIL != ''";

                DAL_MIS AcessaDadosMis = new DAL.DAL_MIS();
                DataSet ds = AcessaDadosMis.ConsultaSQL(sqlcommand);

                List<string> ListaEmail = new List<string>();

                if ((ds.Tables.Count > 0) && (ds.Tables[0].Rows.Count > 0))
                    foreach (DataRow dr in ds.Tables[0].Rows)
                        ListaEmail.Add(dr["NM_EMAIL"].ToString().ToLower());

                return ListaEmail;
            }
            catch (Exception ex)
            {
                throw new Exception("BLL.WEB.Operador_999: " + ex.Message, ex);
            }
        }

        public DataSet ListaResponsavelGestao()
        {
            try
            {
                SqlCommand sqlcommand = new SqlCommand();
                sqlcommand.CommandType = CommandType.Text;
                sqlcommand.CommandText = "SELECT \n"
                                        + "      NR_COLABORADOR \n"
                                        + "    , NM_COLABORADOR \n"
                                        + "FROM TBL_WEB_COLABORADOR_DADOS \n"
                                        + "WHERE  TP_STATUS IN (1,5) AND TP_GESTOR = '1' \n"
                                        + "ORDER BY \n"
                                        + "    NM_COLABORADOR \n";
                DAL_MIS AcessaDadosMis = new DAL.DAL_MIS();
                return AcessaDadosMis.ConsultaSQL(sqlcommand);
            }
            catch (Exception ex)
            {
                throw new Exception("BLL.WEB.Operador_999: " + ex.Message, ex);
            }
        }

        public DataSet CalcularLoginsOperador(int NR_OLOS)
        {
            try
            {
                SqlCommand sqlcommand = new SqlCommand();
                sqlcommand.CommandType = CommandType.StoredProcedure;
                sqlcommand.Parameters.Add(new SqlParameter("@AGENTID", NR_OLOS));
                sqlcommand.CommandText = "SP_LISTA_LOGINS_OPERADORES";
                DAL_MIS AcessaDadosMis = new DAL.DAL_MIS();
                return AcessaDadosMis.ConsultaSQL(sqlcommand);
            }
            catch (Exception ex)
            {
                throw new Exception("BLL.WEB.Operador_999: " + ex.Message, ex);
            }
        }


        #endregion DropDownList Coordenador, Supervisor, Operador e Lista de Emails Gerencia
    }
}