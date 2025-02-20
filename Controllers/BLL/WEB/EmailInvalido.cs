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
    public class EmailInvalido
    {
        #region Dados cadastrais do email

        public DataSet ListaEmailInvalido(String NM_EMAIL, string TP_IMPORTADO, DateTime DT_INI, DateTime DT_FIM)
        {
            try
            {
                SqlCommand sqlcommand = new SqlCommand();
                sqlcommand.CommandType = CommandType.StoredProcedure;
                sqlcommand.Parameters.AddWithValue("@NM_EMAIL", NM_EMAIL);
                sqlcommand.Parameters.AddWithValue("@TP_IMPORTADO", TP_IMPORTADO);
                sqlcommand.Parameters.AddWithValue("@DT_INI", DT_INI);
                sqlcommand.Parameters.AddWithValue("@DT_FIM", DT_FIM.AddDays(1));

                sqlcommand.CommandText = "SP_WEB_LISTA_EMAILINVALIDO";

                DAL_MIS AcessaDadosMis = new DAL.DAL_MIS();
                return AcessaDadosMis.ConsultaSQL(sqlcommand);
            }
            catch (Exception ex)
            {
                throw new Exception("BLL.WEB.EmailInvalido_001: " + ex.Message, ex);
            }
        }

        public int GravaEmailInvalido(EmissaoBoleto_EmailInvalido dto)
        {
            try
            {
                SqlCommand sqlcommand = new SqlCommand();
                sqlcommand.CommandType = CommandType.Text;

                DateTime dt = DateTime.MinValue;
                int num = 0;

                sqlcommand.Parameters.AddWithValue("@NR_REGISTRO", int.TryParse(dto.NR_REGISTRO.ToString(), out num) ? (object)int.Parse(dto.NR_REGISTRO.ToString()) : DBNull.Value);
                sqlcommand.Parameters.AddWithValue("@NM_EMAIL", dto.NM_EMAIL);
                sqlcommand.Parameters.AddWithValue("NR_USUARIO_EMISSAO", dto.NR_USUARIO_EMISSAO);

                return Novo(sqlcommand);
            }

            catch (Exception ex)
            {
                throw new Exception("BLL.WEB.EmailInvalido_003: " + ex.Message, ex);
            }
        }

        private int Novo(SqlCommand sqlcommand)
        {
            try
            {
                sqlcommand.CommandText = "INSERT INTO TBL_WEB_EMISSAO_BOLETO_EMAIL_INVALIDO \n"
                                        + "        (DT_REGISTRO, NM_EMAIL, CD_ENVIO_CRM, NR_USUARIO_EMISSAO) \n"
                                        + " VALUES (GETDATE(), @NM_EMAIL, 0, @NR_USUARIO_EMISSAO)  \n";

                DAL_MIS AcessaDadosMis = new DAL.DAL_MIS();
                return AcessaDadosMis.ExecutaComandoSQL(sqlcommand);
            }
            catch (Exception ex)
            {
                throw new Exception("BLL.WEB.EmailInvalido_004: " + ex.Message, ex);
            }
        }

        #endregion
    }
}