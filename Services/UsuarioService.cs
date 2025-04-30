using Intranet_NEW.DAL;
using Intranet_NEW.Models.WEB;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Text;
using System.Security.Cryptography;
using Intranet_NEW.Services.Validadores;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;

namespace Intranet_NEW.Services
{
    public class UsuarioService
    {
        private readonly DAL_MIS dao_MIS;
        public UsuarioService()
        {
            dao_MIS = new();
        }

        #region Selects

        public Colaborador GetColaborador(string cpf)
        {
            Colaborador model = new();
            SqlCommand cmd = new();

            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "SELECT    A.NR_COLABORADOR "+
                              ",A.NR_EMPRESA      "+
                              ",A.NR_MATRICULA    "+
                              ",A.DT_ADMISSAO     "+
                              ",A.DT_NASCIMENTO   "+
                              ",A.NM_COLABORADOR  "+
                              ",A.NR_CPF          "+
                              ",A.TP_SEXO         "+
                              ",A.NR_RAMAL        "+
                              ",A.NM_EMAIL        "+
                              ",A.NR_GESTOR       "+
                              ",A.NR_FILIAL       "+
                              ",B.NM_FUNCAO       "+
                              ",A.NR_FUNCAO_RH    "+
                              ",A.NR_ATIVIDADE_RH "+
                              ",A.NM_SENHA        "+
                              ",A.NR_COORDENADOR  "+
                              ",A.NR_SUPERVISOR   "+
                              ",A.NM_EQUIPE       "+
                              ",A.NR_OLOS         "+
                              ",A.NM_LOGIN_OLOS   "+
                              ",A.TP_TURNO        "+
                              ",C.NM_COLABORADOR AS NM_SUPERVISOR   "+
                              ",D.NM_COLABORADOR AS NM_COORDENADOR  "+
                              ",A.TP_PRIORIDADE_ACESSO  "+
                              "FROM TBL_WEB_COLABORADOR_DADOS A                                           "+
                              "LEFT JOIN TBL_WEB_RH_COMBO_FUNCAO B ON A.NR_FUNCAO_RH = B.NR_FUNCAO             "+
                              "LEFT JOIN TBL_WEB_COLABORADOR_DADOS C ON A.NR_SUPERVISOR = C.NR_COLABORADOR     "+
                              "LEFT JOIN TBL_WEB_COLABORADOR_DADOS D ON A.NR_COORDENADOR = D.NR_COLABORADOR    "+
                              "WHERE A.NR_CPF = @NR_CPF";
            cmd.Parameters.Add(new SqlParameter("@NR_CPF", cpf));

            DataSet ds = dao_MIS.ConsultaSQL(cmd);

            return MontaColaborador(ds.Tables[0].Rows[0]);
        }

        public static Colaborador MontaColaborador(DataRow row)
        {
            return new Colaborador
            {
                NR_COLABORADOR = row["NR_COLABORADOR"].ToString(),    
                NR_EMPRESA = row["NR_EMPRESA"].ToString(),
                NR_MATRICULA = row["NR_MATRICULA"].ToString(),
                DT_ADMISSAO = row["DT_ADMISSAO"].ToString(),
                DT_NASCIMENTO = row["DT_NASCIMENTO"].ToString(),
                NM_COLABORADOR = row["NM_COLABORADOR"].ToString(),
                NR_CPF = row["NR_CPF"].ToString(),
                TP_SEXO = row["TP_SEXO"].ToString(),
                NR_RAMAL = row["NR_RAMAL"].ToString(),
                NM_EMAIL = row["NM_EMAIL"].ToString(),
                NR_GESTOR = row["NR_GESTOR"].ToString(),
                NR_FILIAL = row["NR_FILIAL"].ToString(),
                NM_FUNCAO_RH = row["NM_FUNCAO"].ToString(),
                NR_FUNCAO_RH = row["NR_FUNCAO_RH"].ToString(),
                NR_ATIVIDADE_RH = row["NR_ATIVIDADE_RH"].ToString(),
                NM_SENHA = row["NM_SENHA"].ToString(),
                NR_COORDENADOR = row["NR_COORDENADOR"].ToString(),
                NR_SUPERVISOR = row["NR_SUPERVISOR"].ToString(),
                NM_EQUIPE = row["NM_EQUIPE"].ToString(),
                NR_OLOS = row["NR_OLOS"].ToString(),
                NM_LOGIN_OLOS = row["NM_LOGIN_OLOS"].ToString(),
                TP_TURNO = row["TP_TURNO"].ToString(),
                NM_COORDENADOR = row["NM_COORDENADOR"].ToString(),
                NM_SUPERVISOR = row["NM_SUPERVISOR"].ToString(),
                TP_PRIORIDADE_ACESSO = Convert.ToInt32(row["TP_PRIORIDADE_ACESSO"])
            };
        }

        #endregion

        #region Updates 
        public int AlteraSenhaUsuario(Colaborador model)
        {
            
            SqlCommand sqlcommand = new SqlCommand();
            sqlcommand.CommandType = CommandType.Text;
            sqlcommand.CommandText = "UPDATE TBL_WEB_COLABORADOR_DADOS SET NM_SENHA = @NM_SENHA, TP_ALTERA_SENHA = 0 WHERE NR_CPF = @NR_CPF AND DT_NASCIMENTO = @DT_NASCIMENTO AND TP_STATUS = 1";

            try
            {
                sqlcommand.Parameters.Add(new SqlParameter("@NR_CPF", model.NR_CPF.Replace(".", "").Replace("-", "")));
                sqlcommand.Parameters.Add(new SqlParameter("@DT_NASCIMENTO", DateTime.Parse(model.DT_NASCIMENTO)));
                sqlcommand.Parameters.AddWithValue("@NM_SENHA", Criptsha1(model.NM_SENHA));

                return dao_MIS.ExecutaComandoSQL(sqlcommand);
            }
            catch (Exception ex)
            {
                throw new Exception("cmd_Usuario_Acesso_001: " + ex.Message, ex);
            }


        }
        public string Criptsha1(string entrada)
        {
            SHA1CryptoServiceProvider sha1 = new();
            byte[] to_be_hash = ASCIIEncoding.Default.GetBytes(entrada);
            byte[] hash = sha1.ComputeHash(to_be_hash);
            string delimitedHexHash = BitConverter.ToString(hash);
            string hexHash = delimitedHexHash.Replace("-", "");

            return hexHash;
        }
        #endregion


    }
}
