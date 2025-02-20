using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;

namespace Intranet.BLL.WEB
{
    public class ColaboradorRh : ColaboradorAcesso
    {

   
        Intranet.DAL.DAL_MIS_N AcessaDadosMis = new Intranet.DAL.DAL_MIS_N();

        #region Métodos para validação e alteração de senha

        public Intranet_NEW.Models.WEB.Colaborador VerificaLoginUsuario(string NR_CPF, string NM_SENHA)
        {
            NR_CPF = NR_CPF.Replace(".", "").Replace("-", "");

            Intranet_NEW.Models.WEB.Colaborador objColaborador = new Intranet.DTO.Colaborador();
            SqlCommand sqlcommand = new SqlCommand();
            sqlcommand.CommandType = CommandType.Text;
            sqlcommand.CommandText = "SELECT NR_FILIAL, NR_MATRICULA, NM_COLABORADOR, TP_ALTERA_SENHA, DT_NASCIMENTO FROM TBL_WEB_COLABORADOR_DADOS WHERE NR_CPF = @NR_CPF AND NM_SENHA IN(@NM_SENHA1, @NM_SENHA2)";

            try
            {
                sqlcommand.Parameters.Add(new SqlParameter("@NR_CPF", NR_CPF));
                sqlcommand.Parameters.Add(new SqlParameter("@NM_SENHA1", Criptsha1(NM_SENHA)));
                sqlcommand.Parameters.Add(new SqlParameter("@NM_SENHA2", NM_SENHA));

                DataSet ds = AcessaDadosMis.ConsultaSQL(sqlcommand);

                if ((ds.Tables.Count > 0) && (ds.Tables[0].Rows.Count > 0))
                {
                    DataRow dr = ds.Tables[0].Rows[0];
                    objColaborador.NR_FILIAL = dr["NR_FILIAL"].ToString();
                    objColaborador.NR_MATRICULA = dr["NR_MATRICULA"].ToString();

                    objColaborador.NM_COLABORADOR = dr["NM_COLABORADOR"].ToString();
                    objColaborador.TP_ALTERA_SENHA = dr["TP_ALTERA_SENHA"].ToString();
                    objColaborador.DT_NASCIMENTO = ((DateTime)dr["DT_NASCIMENTO"]).ToString("dd/MM/yyyy");
                    objColaborador.NR_CPF = NR_CPF;
                }
                return objColaborador;
            }
            catch (Exception ex)
            {
                throw new Exception("RH.AcessoSistema_001: " + ex.Message, ex);
            }
        }

        public int AlteraSenhaUsuario(string NR_MATRICULA, string NM_SENHA_OLD, string NM_SENHA_NEW)
        {
            SqlCommand sqlcommand = new SqlCommand();
            sqlcommand.CommandType = CommandType.Text;
            sqlcommand.CommandText = "UPDATE TBL_WEB_COLABORADOR_DADOS SET NM_SENHA = @NM_SENHA_NEW, TP_ALTERA_SENHA = 0 WHERE NR_MATRICULA = @NR_MATRICULA AND NM_SENHA IN(@NM_SENHA_OLD1, @NM_SENHA_OLD2)";

            try
            {
                sqlcommand.Parameters.Add(new SqlParameter("@NR_MATRICULA", NR_MATRICULA));
                sqlcommand.Parameters.AddWithValue("@NM_SENHA_OLD1", NM_SENHA_OLD);
                sqlcommand.Parameters.AddWithValue("@NM_SENHA_OLD2", Criptsha1(NM_SENHA_OLD));
                sqlcommand.Parameters.AddWithValue("@NM_SENHA_NEW", Criptsha1(NM_SENHA_NEW));
                return AcessaDadosMis.ExecutaComandoSQL(sqlcommand);
            }
            catch (Exception ex)
            {
                throw new Exception("RH.AcessoSistema_002: " + ex.Message, ex);
            }
        }

        public int AlteraSenhaUsuario(string NM_COLABORADOR, string NR_CPF, DateTime DT_NASCIMENTO, string NM_SENHA_NEW)
        {
            NR_CPF = NR_CPF.Replace(".", "").Replace("-", "");

            SqlCommand sqlcommand = new SqlCommand();
            sqlcommand.CommandType = CommandType.Text;
            sqlcommand.CommandText = " IF (SELECT COUNT(*) FROM TBL_WEB_COLABORADOR_DADOS WHERE NM_COLABORADOR = @NM_COLABORADOR AND NR_CPF = @NR_CPF AND DT_NASCIMENTO = @DT_NASCIMENTO AND DT_DEMISSAO IS NULL) = 1 \n"
                                    + " BEGIN \n"
                                    + "     UPDATE TBL_WEB_COLABORADOR_DADOS SET NM_SENHA = @NM_SENHA_NEW \n"
                                    + "     WHERE NM_COLABORADOR = @NM_COLABORADOR AND NR_CPF = @NR_CPF AND DT_NASCIMENTO = @DT_NASCIMENTO AND DT_DEMISSAO IS NULL \n"
                                    + " END \n";
            try
            {
                sqlcommand.Parameters.AddWithValue("@NM_COLABORADOR", NM_COLABORADOR);
                sqlcommand.Parameters.AddWithValue("@NR_CPF", NR_CPF);
                sqlcommand.Parameters.AddWithValue("@DT_NASCIMENTO", DT_NASCIMENTO.ToString("yyyy-MM-dd"));
                sqlcommand.Parameters.AddWithValue("@NM_SENHA_NEW", Criptsha1(NM_SENHA_NEW));
                return AcessaDadosMis.ExecutaComandoSQL(sqlcommand);
            }
            catch (Exception ex)
            {
                throw new Exception("RH.AcessoSistema_003: " + ex.Message, ex);
            }
        }

        #endregion

        public Intranet_NEW.Models.WEB.Colaborador DadosColaborador(string NR_FILIAL, string NR_MATRICULA)
        {
            try
            {
                SqlCommand sqlcommand = new SqlCommand();
                sqlcommand.CommandType = CommandType.Text;
                sqlcommand.Parameters.AddWithValue("@NR_FILIAL", NR_FILIAL);
                sqlcommand.Parameters.AddWithValue("@NR_MATRICULA", NR_MATRICULA);
                sqlcommand.CommandText = "SELECT * \n"
                                        + "FROM TBL_WEB_RH_COLABORADOR_DADOS \n"
                                        + "WHERE NR_FILIAL = @NR_FILIAL AND NR_MATRICULA = @NR_MATRICULA \n";

                DAL.DAL_MIS_N AcessaDadosMis = new DAL.DAL_MIS_N();
                DataSet ds = AcessaDadosMis.ConsultaSQL(sqlcommand);

                if ((ds.Tables.Count > 0) && (ds.Tables[0].Rows.Count > 0))
                {
                    DataRow dr = ds.Tables[0].Rows[0];
                    Intranet_NEW.Models.WEB.Colaborador dto = new DTO.Colaborador();

                    dto.NR_FILIAL = dr["NR_FILIAL"] != DBNull.Value ? dr["NR_FILIAL"].ToString() : "";
                    dto.NR_MATRICULA = dr["NR_MATRICULA"] != DBNull.Value ? dr["NR_MATRICULA"].ToString() : "";

                    dto.NM_ATIVIDADE_RH = dr["NM_ATIVIDADE"] != DBNull.Value ? dr["NM_ATIVIDADE"].ToString() : "";
                    dto.NM_FUNCAO_RH = dr["NM_FUNCAO"] != DBNull.Value ? dr["NM_FUNCAO"].ToString() : "";

                    dto.NM_COLABORADOR = dr["NM_COLABORADOR"] != DBNull.Value ? dr["NM_COLABORADOR"].ToString() : "";
                    dto.NR_CPF = dr["NR_CPF"] != DBNull.Value ? string.Format(@"{0:000\.000\.000\-00}", ((decimal)dr["NR_CPF"])) : "";

                    dto.DT_NASCIMENTO = dr["DT_NASCIMENTO"] != DBNull.Value ? ((DateTime)dr["DT_NASCIMENTO"]).ToString("dd/MM/yyyy") : "";
                    dto.DT_ADMISSAO = dr["DT_ADMISSAO"] != DBNull.Value ? ((DateTime)dr["DT_ADMISSAO"]).ToString("dd/MM/yyyy") : "";
                    dto.DT_DEMISSAO = dr["DT_DEMISSAO"] != DBNull.Value ? ((DateTime)dr["DT_DEMISSAO"]).ToString("dd/MM/yyyy") : "";

                    dto.NM_JORNADA_TRAB_RH = dr["NM_JORNADA_TRAB"] != DBNull.Value ? dr["NM_JORNADA_TRAB"].ToString() : "";
                    
                    return dto;
                }
                return new DTO.Colaborador();
            }
            catch (Exception ex)
            {
                throw new Exception("BLL.RH.Operador_001: " + ex.Message, ex);
            }
        }
    }
}
