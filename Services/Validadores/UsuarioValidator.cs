using FluentValidation;
using FluentValidation.Results;
using Intranet_NEW.DAL;
using Intranet_NEW.Models.WEB;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Text;
using Tweetinvi.Security;
namespace Intranet_NEW.Services.Validadores
{
    public class UsuarioValidator : AbstractValidator<Colaborador>
    {
        public UsuarioValidator() 
        {

            RuleSet("VerificarSenha", () =>
            {
                RuleFor(x => x).Must(VerificarSenha);
            });


            RuleSet("AlterarSenha", () =>
            {
                RuleFor(x => x).Must(VerificarUsuario).WithMessage("Não encontramos usuario para o CPF e Data de Nascimento Informados");
            });

        }

        private bool VerificarSenha(Colaborador model)
        {
                DAL_MIS dao = new();
                SqlCommand cmd = new();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = "SELECT * FROM TBL_WEB_COLABORADOR_DADOS WHERE (NR_CPF = @NM_LOGIN) AND ( NM_SENHA = @NM_SENHA1 OR NM_SENHA = @NM_SENHA) AND NM_SENHA IS NOT NULL AND TP_STATUS = 1";
                cmd.Parameters.Add(new SqlParameter("@NM_LOGIN", model.NR_CPF.Replace(".", "").Replace("-", "")));
                cmd.Parameters.Add(new SqlParameter("@NM_SENHA", Criptsha1(model.NM_SENHA)));
                cmd.Parameters.Add(new SqlParameter("@NM_SENHA1", model.NM_SENHA));
                DataSet ds = dao.ConsultaSQL(cmd);
                if (ds.Tables[0].Rows.Count > 0)
                    return true;
                else
                    return false;
        }
        public bool VerificarUsuario(Colaborador model)
        {
                DAL_MIS dao = new();
                SqlCommand cmd = new();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = "SELECT * FROM TBL_WEB_COLABORADOR_DADOS WHERE (NR_CPF = @NM_LOGIN) AND TP_STATUS = 1 AND CAST(DT_NASCIMENTO AS DATE) = @DT_NASCIMENTO ";
                cmd.Parameters.Add(new SqlParameter("@NM_LOGIN", model.NR_CPF.Replace(".", "").Replace("-", "")));
                cmd.Parameters.Add(new SqlParameter("@DT_NASCIMENTO", model.DT_NASCIMENTO));
                DataSet ds = dao.ConsultaSQL(cmd);
                if (ds.Tables[0].Rows.Count > 0)
                    return true;
                else
                    return false;
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

    }
}
