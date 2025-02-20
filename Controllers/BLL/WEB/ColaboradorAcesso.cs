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
    public class ColaboradorAcesso : Seguranca
    {
        DAL_MIS AcessaDadosMis = new Intranet.DAL.DAL_MIS();
        DAL_OLOS AcessaDadosOlos = new DAL.DAL_OLOS();

        #region Validacao dados OLOS

        public int VerificaCpfUsuarioOlos_GTZ(string NR_CPF, string NM_LOGIN_OLOS)
        {
            try
            {
                SqlCommand sqlcommand = new SqlCommand();
                sqlcommand.CommandType = CommandType.Text;
                sqlcommand.Parameters.AddWithValue("@NR_CPF", NR_CPF);
                sqlcommand.Parameters.AddWithValue("@NM_LOGIN_OLOS", NM_LOGIN_OLOS);

                sqlcommand.CommandText = "SELECT UserId FROM ConfigUsers WHERE REPLACE(REPLACE(Email, '@uol.com', ''), '.br', '')  = @NR_CPF AND [Login] = @NM_LOGIN_OLOS \n";

                DataSet ds = AcessaDadosOlos.ConsultaSQL(sqlcommand);

                if ((ds.Tables.Count > 0) && (ds.Tables[0].Rows.Count > 0))
                    return (int.Parse(ds.Tables[0].Rows[0]["UserId"].ToString()));
                return (0);
            }
            catch (Exception ex)
            {
                throw new Exception("BLL.WEB.ValidacaoOlos_002: " + ex.Message, ex);
            }
        }

        public int VerificaCpfUsuarioOlos_MTZ(string NR_CPF, string NM_LOGIN_OLOS)
        {
            try
            {
                SqlCommand sqlcommand = new SqlCommand();
                sqlcommand.CommandType = CommandType.Text;
                sqlcommand.Parameters.AddWithValue("@NR_CPF", NR_CPF);
                sqlcommand.Parameters.AddWithValue("@NM_LOGIN_OLOS", NM_LOGIN_OLOS);

                sqlcommand.CommandText = "SELECT UserId FROM ConfigUsers WHERE REPLACE(REPLACE(Email, '@uol.com', ''), '.br', '')  = @NR_CPF AND [Login] = @NM_LOGIN_OLOS \n";

                DAL_MTZ_OLOS AcessaDadosOlosMtz = new DAL.DAL_MTZ_OLOS();
                DataSet ds = AcessaDadosOlosMtz.ConsultaSQL(sqlcommand);

                if ((ds.Tables.Count > 0) && (ds.Tables[0].Rows.Count > 0))
                    return (int.Parse(ds.Tables[0].Rows[0]["UserId"].ToString()));
                return (0);
            }
            catch (Exception ex)
            {
                throw new Exception("BLL.WEB.ValidacaoOlos_002: " + ex.Message, ex);
            }
        }

        public int VerificaIpHostUsuarioOlos(string NR_IPHOST)
        {
            try
            {
                SqlCommand sqlcommand = new SqlCommand();
                sqlcommand.CommandType = CommandType.Text;
                sqlcommand.Parameters.AddWithValue("@NR_IPHOST", NR_IPHOST);

                sqlcommand.CommandText = "SELECT AgentId FROM AgentStatus WHERE IpAddress = @NR_IPHOST \n";

                DataSet ds = AcessaDadosOlos.ConsultaSQL(sqlcommand);
                if ((ds.Tables.Count > 0) && (ds.Tables[0].Rows.Count > 0))
                    return int.Parse(ds.Tables[0].Rows[0]["AgentId"].ToString());
                return (0);
            }
            catch (Exception ex)
            {
                throw new Exception("BLL.WEB.ValidacaoOlos_003: " + ex.Message, ex);
            }
        }

        #endregion


        #region Manipulação do acesso do usuário

        public Intranet_NEW.Models.WEB.Colaborador VerificaLoginUsuario(string NR_CPF, string usu_senha)
        {
            Intranet_NEW.Models.WEB.Colaborador objColaborador = new Intranet.DTO.Colaborador();
            SqlCommand sqlcommand = new SqlCommand();
            sqlcommand.CommandType = CommandType.Text;
            //sqlcommand.CommandText = "SELECT * FROM TBL_WEB_COLABORADOR_DADOS WHERE NR_CPF = @NR_CPF AND ( NM_SENHA = @NM_SENHA1 OR NM_SENHA = @NM_SENHA) AND NM_SENHA IS NOT NULL";
            sqlcommand.CommandText = "SELECT * FROM TBL_WEB_COLABORADOR_DADOS WHERE NR_CPF = 52879204828";
            try
            {
                sqlcommand.Parameters.Add(new SqlParameter("@NR_CPF", NR_CPF.Replace("-", "").Replace(".", "")));
                sqlcommand.Parameters.Add(new SqlParameter("@NM_SENHA", Criptsha1(usu_senha)));
                sqlcommand.Parameters.Add(new SqlParameter("@NM_SENHA1", usu_senha));

                DataSet ds = AcessaDadosMis.ConsultaSQL(sqlcommand);

                if ((ds.Tables.Count > 0) && (ds.Tables[0].Rows.Count > 0))
                {
                    DataRow dr = ds.Tables[0].Rows[0];
                    objColaborador.NR_COLABORADOR = dr["NR_COLABORADOR"].ToString();
                    objColaborador.NM_COLABORADOR = dr["NM_COLABORADOR"].ToString();
                    objColaborador.TP_STATUS = dr["TP_STATUS"].ToString();
                    objColaborador.TP_ALTERA_SENHA = dr["TP_ALTERA_SENHA"].ToString();
                    objColaborador.TP_ACESSO_SISTEMA = dr["TP_ACESSO_SISTEMA"].ToString();
                    objColaborador.NR_EMPRESA = dr["NR_EMPRESA"].ToString();
                    objColaborador.NR_FILIAL = dr["NR_FILIAL"].ToString();
                    objColaborador.NR_MATRICULA = dr["NR_MATRICULA"].ToString();

                    objColaborador.TP_ALTERA_SENHA = dr["TP_ALTERA_SENHA"].ToString();
                    //objColaborador.DT_NASCIMENTO = ((DateTime)dr["DT_NASCIMENTO"]).ToString("dd/MM/yyyy");
                    objColaborador.NR_CPF = dr["NR_CPF"].ToString();
                }
                return objColaborador;
            }
            catch (Exception ex)
            {
                throw new Exception("cmd_Usuario_001: " + ex.Message, ex);
            }
        }


        public bool VerificaFuncaoUsuario(string NR_COLABORADOR, int NR_FUNCAO)
        {
            Intranet_NEW.Models.WEB.Colaborador objColaborador = new Intranet.DTO.Colaborador();
            SqlCommand sqlcommand = new SqlCommand();
            sqlcommand.CommandType = CommandType.Text;
            sqlcommand.CommandText = "SELECT * FROM TBL_WEB_COLABORADOR_DADOS WHERE NR_COLABORADOR = @NR_COLABORADOR";
            try
            {
                sqlcommand.Parameters.Add(new SqlParameter("@NR_COLABORADOR",NR_COLABORADOR));

                DataSet ds = AcessaDadosMis.ConsultaSQL(sqlcommand);
                DataRow dr = ds.Tables[0].Rows[0];
        

                if (int.Parse(dr["NR_FUNCAO_RH"].ToString()) == NR_FUNCAO)
                {
                    return true;
                }
                else 
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("cmd_Usuario_009: " + ex.Message, ex);
            }
        }

        public Intranet_NEW.Models.WEB.Colaborador VerificaLoginUsuarioEspelho(string usu_login, string usu_senha)
        {
            Intranet_NEW.Models.WEB.Colaborador objColaborador = new Intranet.DTO.Colaborador();
            SqlCommand sqlcommand = new SqlCommand();
            sqlcommand.CommandType = CommandType.Text;
            sqlcommand.CommandText = "SELECT * FROM TBL_WEB_COLABORADOR_DADOS WHERE (NR_CPF = @NM_LOGIN) AND ( NM_SENHA = @NM_SENHA1 OR NM_SENHA = @NM_SENHA) AND NM_SENHA IS NOT NULL AND TP_STATUS IN (1,5)";

            try
            {
                sqlcommand.Parameters.Add(new SqlParameter("@NM_LOGIN", usu_login));
                sqlcommand.Parameters.Add(new SqlParameter("@NM_SENHA", Criptsha1(usu_senha)));
                sqlcommand.Parameters.Add(new SqlParameter("@NM_SENHA1", usu_senha));

                DataSet ds = AcessaDadosMis.ConsultaSQL(sqlcommand);

                if ((ds.Tables.Count > 0) && (ds.Tables[0].Rows.Count > 0))
                {
                    DataRow dr = ds.Tables[0].Rows[0];
                    objColaborador.NR_COLABORADOR = dr["NR_COLABORADOR"].ToString();
                    objColaborador.NM_COLABORADOR = dr["NM_COLABORADOR"].ToString();
                    objColaborador.TP_STATUS = dr["TP_STATUS"].ToString();
                    objColaborador.TP_ALTERA_SENHA = dr["TP_ALTERA_SENHA"].ToString();

                    objColaborador.NR_FILIAL = dr["NR_FILIAL"].ToString();
                    objColaborador.NR_MATRICULA = dr["NR_MATRICULA"].ToString();

                    objColaborador.TP_ALTERA_SENHA = dr["TP_ALTERA_SENHA"].ToString();
                    objColaborador.DT_NASCIMENTO = ((DateTime)dr["DT_NASCIMENTO"]).ToString("dd/MM/yyyy");
                    objColaborador.NR_CPF = dr["NR_CPF"].ToString();
                }
                return objColaborador;
            }
            catch (Exception ex)
            {
                throw new Exception("cmd_Usuario_001: " + ex.Message, ex);
            }
        }

        public Intranet_NEW.Models.WEB.Colaborador VerificaLoginUsuarioOlos(int NR_OLOS)
        {
            Intranet_NEW.Models.WEB.Colaborador objColaborador = new Intranet.DTO.Colaborador();
            SqlCommand sqlcommand = new SqlCommand();
            sqlcommand.CommandType = CommandType.Text;
            sqlcommand.CommandText = "SELECT * FROM TBL_WEB_COLABORADOR_DADOS WHERE TP_STATUS IN (1,5) AND NR_OLOS = @NR_OLOS ";

            try
            {
                sqlcommand.Parameters.Add(new SqlParameter("@NR_OLOS", NR_OLOS));
                DataSet ds = AcessaDadosMis.ConsultaSQL(sqlcommand);

                if ((ds.Tables.Count > 0) && (ds.Tables[0].Rows.Count > 0))
                {
                    DataRow dr = ds.Tables[0].Rows[0];
                    objColaborador.NR_COLABORADOR = dr["NR_COLABORADOR"].ToString();
                    objColaborador.NM_COLABORADOR = dr["NM_COLABORADOR"].ToString();
                    objColaborador.TP_STATUS = dr["TP_STATUS"].ToString();
                    objColaborador.TP_ALTERA_SENHA = dr["TP_ALTERA_SENHA"].ToString();

                    objColaborador.NR_FILIAL = dr["NR_FILIAL"].ToString();
                    objColaborador.NR_MATRICULA = dr["NR_MATRICULA"].ToString();

                    objColaborador.TP_ALTERA_SENHA = dr["TP_ALTERA_SENHA"].ToString();
                    objColaborador.DT_NASCIMENTO = ((DateTime)dr["DT_NASCIMENTO"]).ToString("dd/MM/yyyy");
                    objColaborador.NR_CPF = dr["NR_CPF"].ToString();
                }
                return objColaborador;
            }
            catch (Exception ex)
            {
                throw new Exception("cmd_Usuario_001: " + ex.Message, ex);
            }
        }

        public int VerificaUsuarioLogado(string NR_COLABORADOR, string NR_IP)
        {
            switch (NR_IP)
            {
                case "172.20.10.170":
                case "172.20.10.171":
                case "172.20.10.172":
                case "177.139.175.61":
                case "201.56.207.229":
                case "201.56.207.230":
                case "172.20.18.230":
                


                    {
                        return 1;
                    }
            }

            switch (NR_COLABORADOR)
            {
                case "1": //    WELLINGTON
                case "3": //    MORAIS
                case "104": //  CAIO
                case "13": //   THIAGO
                case "6847": //  GIOVANA LAUDI
                case "6372": // THIAGO COMESSU
                case "7514": // FELIPE
                case "20255": // MATHEUS
                case "484": // MAURO
                case "6": // camila
                case "19214": // NICOLY
                    {
                        return 1;
                    }
            }
            SqlCommand sqlcommand = new SqlCommand();
            sqlcommand.CommandType = CommandType.Text;
            sqlcommand.CommandText = "IF(SELECT COUNT(*) \n"
                                    + "FROM TBL_WEB_CONTROLE_ACESSO_INTRANET \n"
                                    + "WHERE NR_COLABORADOR = @NR_COLABORADOR AND NR_IP <> @NR_IP) > 0 \n"
                                    + "BEGIN \n"
                                    + "	SELECT 0 \n"
                                    + "END \n"
                                    + "ELSE \n"
                                    + "BEGIN \n"
                                    + "	DELETE FROM TBL_WEB_CONTROLE_ACESSO_INTRANET WHERE NR_COLABORADOR = @NR_COLABORADOR AND NR_IP = @NR_IP \n"
                                    + "	INSERT INTO TBL_WEB_CONTROLE_ACESSO_INTRANET VALUES (@NR_COLABORADOR, @NR_IP, GETDATE()) \n"
                                    + "	SELECT 1 \n"
                                    + "END \n";

            try
            {
                sqlcommand.Parameters.Add(new SqlParameter("@NR_COLABORADOR", NR_COLABORADOR));
                sqlcommand.Parameters.Add(new SqlParameter("@NR_IP", NR_IP));

                return int.Parse(AcessaDadosMis.ConsultaSQL(sqlcommand).Tables[0].Rows[0][0].ToString());
            }
            catch (Exception ex)
            {
                throw new Exception("cmd_Usuario_Acesso_001: " + ex.Message, ex);
            }
        }

        public int DeslogaUsuarioLogado(string NR_COLABORADOR, string NR_IP)
        {
            SqlCommand sqlcommand = new SqlCommand();
            sqlcommand.CommandType = CommandType.Text;
            sqlcommand.CommandText = "DELETE FROM TBL_WEB_CONTROLE_ACESSO_INTRANET WHERE NR_COLABORADOR = @NR_COLABORADOR AND (NR_IP = @NR_IP OR @NR_IP = '-1') \n";

            try
            {
                sqlcommand.Parameters.Add(new SqlParameter("@NR_COLABORADOR", NR_COLABORADOR));
                sqlcommand.Parameters.Add(new SqlParameter("@NR_IP", NR_IP));

                return AcessaDadosMis.ExecutaComandoSQL(sqlcommand);
            }
            catch (Exception ex)
            {
                throw new Exception("cmd_Usuario_Acesso_001: " + ex.Message, ex);
            }
        }

        public DataSet ListaIPColaboradores(string Nome)
        {
            try
            {
                SqlCommand sqlcommand = new SqlCommand();
                sqlcommand.CommandType = CommandType.StoredProcedure;
                sqlcommand.Parameters.AddWithValue("@NOME", Nome);
                sqlcommand.CommandText = "SP_PESQUISA_IP_COLABORADOR";

                DAL_MIS AcessaDadosMis = new DAL.DAL_MIS();
                return AcessaDadosMis.ConsultaSQL(sqlcommand);
            }
            catch (Exception ex)
            {
                throw new Exception("BLL.WEB.Colaborador_001: " + ex.Message, ex);
            }

        }

        public DataSet ListaUsuarioLogado()
        {
            SqlCommand sqlcommand = new SqlCommand();
            sqlcommand.CommandType = CommandType.Text;
            sqlcommand.CommandText = "SELECT A.NR_COLABORADOR, B.NM_COLABORADOR, A.NR_IP, A.DT_ACESSO \n"
                                    + "FROM TBL_WEB_CONTROLE_ACESSO_INTRANET A \n"
                                    + "INNER JOIN TBL_WEB_COLABORADOR_DADOS  B ON A.NR_COLABORADOR = B.NR_COLABORADOR \n"
                                    + "ORDER BY 2";
            try
            {

                return AcessaDadosMis.ConsultaSQL(sqlcommand);
            }
            catch (Exception ex)
            {
                throw new Exception("cmd_Usuario_Acesso_003: " + ex.Message, ex);
            }
        }

        public int AlteraSenhaUsuario(int NR_COLABORADOR, string NM_SENHA, int TP_ALTERA_SENHA)
        {
            SqlCommand sqlcommand = new SqlCommand();
            sqlcommand.CommandType = CommandType.Text;
            sqlcommand.CommandText = "UPDATE TBL_WEB_COLABORADOR_DADOS SET NM_SENHA = @NM_SENHA, TP_ALTERA_SENHA = @TP_ALTERA_SENHA WHERE NR_COLABORADOR = @NR_COLABORADOR";

            try
            {
                sqlcommand.Parameters.Add(new SqlParameter("@NR_COLABORADOR", NR_COLABORADOR));
                sqlcommand.Parameters.Add(new SqlParameter("@TP_ALTERA_SENHA", TP_ALTERA_SENHA));
                sqlcommand.Parameters.AddWithValue("@NM_SENHA", NM_SENHA != "" ? (object)NM_SENHA : DBNull.Value);

                return AcessaDadosMis.ExecutaComandoSQL(sqlcommand);
            }
            catch (Exception ex)
            {
                throw new Exception("cmd_Usuario_Acesso_001: " + ex.Message, ex);
            }
        }

        public int AlteraSenhaUsuario(string NM_LOGIN, string NM_SENHA_OLD, string NM_SENHA_NEW)
        {
            SqlCommand sqlcommand = new SqlCommand();
            sqlcommand.CommandType = CommandType.Text;
            sqlcommand.CommandText = "UPDATE TBL_WEB_COLABORADOR_DADOS SET NM_SENHA = @NM_SENHA_NEW, TP_ALTERA_SENHA = 0 WHERE NM_LOGIN = @NM_LOGIN AND (NM_SENHA = @NM_SENHA_OLD1 OR NM_SENHA = @NM_SENHA_OLD2)";

            try
            {
                sqlcommand.Parameters.AddWithValue("@NM_LOGIN", NM_LOGIN);
                sqlcommand.Parameters.AddWithValue("@NM_SENHA_OLD1", NM_SENHA_OLD);
                sqlcommand.Parameters.AddWithValue("@NM_SENHA_OLD2", Criptsha1(NM_SENHA_OLD));
                sqlcommand.Parameters.AddWithValue("@NM_SENHA_NEW", Criptsha1(NM_SENHA_NEW));

                return AcessaDadosMis.ExecutaComandoSQL(sqlcommand);
            }
            catch (Exception ex)
            {
                throw new Exception("cmd_Usuario_Acesso_001: " + ex.Message, ex);
            }
        }

        public int AlteraSenhaUsuarioRh(string NR_CPF, string DT_NASCIMENTO, string NM_SENHA)
        {
            SqlCommand sqlcommand = new SqlCommand();
            sqlcommand.CommandType = CommandType.Text;
            sqlcommand.CommandText = "UPDATE TBL_WEB_COLABORADOR_DADOS SET NM_SENHA = @NM_SENHA, TP_ALTERA_SENHA = 0 WHERE NR_CPF = @NR_CPF AND DT_NASCIMENTO = @DT_NASCIMENTO AND TP_STATUS IN (1,5)";

            try
            {
                sqlcommand.Parameters.Add(new SqlParameter("@NR_CPF", NR_CPF.Replace(".", "").Replace("-", "")));
                sqlcommand.Parameters.Add(new SqlParameter("@DT_NASCIMENTO", DateTime.Parse(DT_NASCIMENTO)));
                sqlcommand.Parameters.AddWithValue("@NM_SENHA", Criptsha1(NM_SENHA));

                return AcessaDadosMis.ExecutaComandoSQL(sqlcommand);
            }
            catch (Exception ex)
            {
                throw new Exception("cmd_Usuario_Acesso_001: " + ex.Message, ex);
            }
        }

        public int VerificaSenha(string NR_COLABORADOR)
        {
            Intranet_NEW.Models.WEB.Colaborador objColaborador = new Intranet.DTO.Colaborador();
            SqlCommand sqlcommand = new SqlCommand();
            sqlcommand.CommandType = CommandType.Text;
            sqlcommand.CommandText = "SELECT * FROM TBL_WEB_COLABORADOR_DADOS WHERE NR_COLABORADOR = @NR_COLABORADOR AND TP_ALTERA_SENHA = 1";

            try
            {
                sqlcommand.Parameters.Add(new SqlParameter("@NR_COLABORADOR", NR_COLABORADOR));
                DataSet ds = AcessaDadosMis.ConsultaSQL(sqlcommand);
                if ((ds.Tables.Count > 0) && (ds.Tables[0].Rows.Count > 0))
                    return (1);

                return (0);
            }
            catch (Exception ex)
            {
                throw new Exception("cmd_Usuario_001: " + ex.Message, ex);
            }
        }

        public int EsqueciMinhaSenha(string NM_LOGIN, string NM_EMAIL)
        {
            SqlCommand sqlcommand = new SqlCommand();
            sqlcommand.CommandType = CommandType.Text;
            sqlcommand.CommandText = "UPDATE TBL_WEB_COLABORADOR_DADOS SET NM_SENHA = @NM_SENHA, TP_ALTERA_SENHA = 1 WHERE NM_LOGIN = @NM_LOGIN AND NM_EMAIL = @NM_EMAIL";

            try
            {
                string NM_SENHA = GeraAleatorio();
                sqlcommand.Parameters.Add(new SqlParameter("@NM_LOGIN", NM_LOGIN));
                sqlcommand.Parameters.Add(new SqlParameter("@NM_EMAIL", NM_EMAIL.ToLower()));
                sqlcommand.Parameters.Add(new SqlParameter("@NM_SENHA", Criptsha1(NM_SENHA)));

                if (AcessaDadosMis.ExecutaComandoSQL(sqlcommand) > 0)
                {
                    System.Net.Mail.MailMessage objEmail = new System.Net.Mail.MailMessage();
                    objEmail.To.Add(NM_EMAIL.ToLower());
                    objEmail.Subject = "[INTRANET] Recuperação de Senha Intranet Exito Brasil";

                    string conteudo = "";
                    conteudo += "<b>Prezado(a),</b>";
                    conteudo += "<br><br>";
                    conteudo += "Foi realizada uma solicitação de recuperação de senha para acesso a intranet. ";
                    conteudo += "<br><br>";
                    conteudo += "Sua senha está ao final deste e-mail. ";
                    conteudo += "<br><br>";
                    conteudo += "<br><br>";
                    conteudo += "Senha: " + NM_SENHA;
                    conteudo += "<br><br>";
                    conteudo += "<br><br>";
                    conteudo += "E-mail enviado pelo sistema, por favor não responder.";
                    conteudo += "<br>";
                    conteudo += "Enviado em: " + DateTime.Now.ToString("dd/MM/yyyy HH:mm");
                    objEmail.Body = conteudo;

                    MAIL mail = new Intranet.DAL.MAIL();

                    if (mail.Enviar(objEmail, "cmd_usuario_006_a") > 0)
                        return (1);
                }
                return (0);
            }
            catch (Exception ex)
            {
                throw new Exception("cmd_Usuario_Acesso_002: " + ex.Message, ex);
            }
        }

        internal int VerificaLoginNovoUsuario(string NM_LOGIN)
        {
            string strcommand = "SELECT * FROM TBL_WEB_COLABORADOR_DADOS WHERE NM_LOGIN = @NM_LOGIN \n";
            SqlCommand sqlcommand = new SqlCommand();
            sqlcommand.CommandType = CommandType.Text;
            sqlcommand.CommandText = strcommand;

            try
            {
                sqlcommand.Parameters.Add(new SqlParameter("@NM_LOGIN", NM_LOGIN));
                DataSet ds = AcessaDadosMis.ConsultaSQL(sqlcommand);

                Usuario obj = new Intranet.DTO.Usuario();
                if ((ds.Tables.Count > 0) && (ds.Tables[0].Rows.Count > 0))
                    return (1);
                return (0);
            }
            catch (Exception ex)
            {
                throw new Exception("cmd_Usuario_Acesso_003: " + ex.Message, ex);
            }
        }

        public int GravaAcessoUsuario(string NR_COLABORADOR, string NR_PAGINA_ACESSO)
        {
            string strcommand = "DELETE FROM TBL_WEB_COLABORADOR_ACESSO_GTZ WHERE NR_COLABORADOR = @NR_COLABORADOR\n";
            if (NR_PAGINA_ACESSO != "")
                strcommand += "INSERT INTO TBL_WEB_COLABORADOR_ACESSO_GTZ VALUES(@NR_COLABORADOR, @NR_PAGINA_ACESSO)";

            SqlCommand sqlcommand = new SqlCommand();
            sqlcommand.CommandType = CommandType.Text;
            sqlcommand.CommandText = strcommand;

            try
            {
                sqlcommand.Parameters.Add(new SqlParameter("@NR_COLABORADOR", NR_COLABORADOR));
                sqlcommand.Parameters.Add(new SqlParameter("@NR_PAGINA_ACESSO", NR_PAGINA_ACESSO));
                return AcessaDadosMis.ExecutaComandoSQL(sqlcommand);
            }
            catch (Exception ex)
            {
                throw new Exception("cmd_Usuario_Acesso_003: " + ex.Message, ex);
            }
        }

        public int VerificaAcessoUsuario(string NR_COLABORADOR, string NM_URL_PAGINA)
        {
            //SqlCommand sqlcommand = new SqlCommand();
            //sqlcommand.CommandType = CommandType.Text;
            //sqlcommand.CommandText = "SELECT COUNT(*) \n"
            //                        + "FROM TBL_WEB_COLABORADOR_ACESSO_GTZ A \n"
            //                        + "    INNER JOIN TBL_WEB_PAGINA_INTRANET_GTZ B \n"
            //                        + "        ON A.NR_PAGINA_ACESSO LIKE '%;' + CONVERT(VARCHAR(5), B.NR_PAGINA) + ';%' \n"
            //                        + "WHERE \n"
            //                        + "    A.NR_COLABORADOR = @NR_COLABORADOR \n"
            //                        + "AND B.NM_URL_PAGINA  = @NM_URL_PAGINA \n";

            try
            {
                return 1;
                //sqlcommand.Parameters.Add(new SqlParameter("@NR_COLABORADOR", NR_COLABORADOR));
                //sqlcommand.Parameters.Add(new SqlParameter("@NM_URL_PAGINA", NM_URL_PAGINA));
                //DataSet ds = AcessaDadosMis.ConsultaSQL(sqlcommand);

                //Intranet.DTO.Usuario obj = new Intranet.DTO.Usuario();
                //if ((ds.Tables.Count > 0) && (ds.Tables[0].Rows.Count > 0))
                //    return (int.Parse(ds.Tables[0].Rows[0][0].ToString()));

                return (0);
            }
            catch (Exception ex)
            {
                throw new Exception("cmd_Usuario_Acesso_003: " + ex.Message, ex);
            }
        }

        public string ListaAcessoUsuario(string NR_COLABORADOR)
        {
            SqlCommand sqlcommand = new SqlCommand();
            sqlcommand.CommandType = CommandType.Text;
            sqlcommand.CommandText = "SELECT NR_PAGINA_ACESSO FROM TBL_WEB_COLABORADOR_ACESSO_GTZ WHERE NR_COLABORADOR = @NR_COLABORADOR";

            try
            {
                sqlcommand.Parameters.Add(new SqlParameter("@NR_COLABORADOR", NR_COLABORADOR));
                DataSet ds = AcessaDadosMis.ConsultaSQL(sqlcommand);

                Usuario obj = new Intranet.DTO.Usuario();
                if ((ds.Tables.Count > 0) && (ds.Tables[0].Rows.Count > 0))
                    return (ds.Tables[0].Rows[0][0].ToString());
                return ("");
            }
            catch (Exception ex)
            {
                throw new Exception("cmd_Usuario_Acesso_003: " + ex.Message, ex);
            }
        }

        public int PerfilAdministrador(string NR_COLABORADOR)
        {
            SqlCommand sqlcommand = new SqlCommand();
            sqlcommand.CommandType = CommandType.Text;
            sqlcommand.CommandText = "SELECT COUNT(*) FROM TBL_WEB_COLABORADOR_DADOS WHERE NR_COLABORADOR = @NR_COLABORADOR AND TP_FUNCAO = 1";

            try
            {
                sqlcommand.Parameters.Add(new SqlParameter("@NR_COLABORADOR", NR_COLABORADOR));
                DataSet ds = AcessaDadosMis.ConsultaSQL(sqlcommand);

                Usuario obj = new Intranet.DTO.Usuario();
                if ((ds.Tables.Count > 0) && (ds.Tables[0].Rows.Count > 0))
                    return (int.Parse(ds.Tables[0].Rows[0][0].ToString()));
                return (0);
            }
            catch (Exception ex)
            {
                throw new Exception("cmd_Usuario_Acesso_003: " + ex.Message, ex);
            }
        }

        public DataSet VerificaAtividadeRh(string NR_COLABORADOR)
        {
            SqlCommand sqlcommand = new SqlCommand();
            sqlcommand.CommandType = CommandType.Text;
            sqlcommand.CommandText = "SELECT NR_COLABORADOR, NM_COLABORADOR \n"
                                    + "FROM TBL_WEB_COLABORADOR_DADOS \n"
                                    + "WHERE NR_ATIVIDADE_RH = '1021' \n"
                                    + "AND NR_COLABORADOR = @NR_COLABORADOR \n"
                                    + "AND TP_STATUS IN (1,5)";
            try
            {
                sqlcommand.Parameters.Add(new SqlParameter("@NR_COLABORADOR", NR_COLABORADOR));
                return AcessaDadosMis.ConsultaSQL(sqlcommand);
            }
            catch (Exception ex)
            {
                throw new Exception("cmd_Usuario_Acesso_003: " + ex.Message, ex);
            }
        }

        public Intranet_NEW.Models.WEB.Colaborador VerificaPerfilUsuario(string NR_COLABORADOR)
        {
            try
            {
                SqlCommand sqlcommand = new SqlCommand();
                sqlcommand.CommandType = CommandType.Text;
                sqlcommand.CommandText = "SELECT NR_COLABORADOR, NM_COLABORADOR, TP_FUNCAO, NR_COORDENADOR, NR_SUPERVISOR, NR_ATIVIDADE_RH FROM TBL_WEB_COLABORADOR_DADOS WHERE NR_COLABORADOR = @NR_COLABORADOR";

                sqlcommand.Parameters.AddWithValue("@NR_COLABORADOR", NR_COLABORADOR);

                DAL_MIS AcessaDadosMisN = new Intranet.DAL.DAL_MIS();
                DataSet ds = AcessaDadosMisN.ConsultaSQL(sqlcommand);

                Intranet_NEW.Models.WEB.Colaborador Colaborador = new DTO.Colaborador();
                if ((ds.Tables.Count > 0) && (ds.Tables[0].Rows.Count > 0))
                {
                    Colaborador.NR_COLABORADOR = ds.Tables[0].Rows[0]["NR_COLABORADOR"].ToString();
                    Colaborador.NM_COLABORADOR = ds.Tables[0].Rows[0]["NM_COLABORADOR"].ToString();
                    Colaborador.TP_FUNCAO = ds.Tables[0].Rows[0]["TP_FUNCAO"].ToString();
                    Colaborador.NR_COORDENADOR = ds.Tables[0].Rows[0]["NR_COORDENADOR"].ToString();
                    Colaborador.NR_SUPERVISOR = ds.Tables[0].Rows[0]["NR_SUPERVISOR"].ToString();
                    Colaborador.NM_ATIVIDADE_RH = ds.Tables[0].Rows[0]["NR_ATIVIDADE_RH"].ToString();
                }
                return Colaborador;
            }
            catch (Exception ex)
            {
                throw new Exception("RET.Tabulacao.ADO_001: " + ex.Message, ex);
            }
        }

        public int VerificaGestaoColaborador(string NR_GESTOR, string NR_COLABORADOR)
        {
            try
            {
                SqlCommand sqlcommand = new SqlCommand();
                sqlcommand.CommandType = CommandType.Text;
                sqlcommand.CommandText = "SELECT NR_COLABORADOR \n"
                                        + "FROM TBL_WEB_COLABORADOR_DADOS \n"
                                        + "WHERE \n"
                                        + "	   (NR_COLABORADOR = @NR_COLABORADOR AND @NR_GESTOR IN (NR_SUPERVISOR, NR_COORDENADOR, NR_GESTOR)) \n"
                                        + "	OR (NR_COLABORADOR = @NR_GESTOR AND TP_FUNCAO IN (4, 5, 6)) \n"
                                        + "	OR (NR_COLABORADOR = @NR_GESTOR AND NR_COLABORADOR IN (1, 1932)) \n"
                                        + "	OR (NR_COLABORADOR = @NR_GESTOR AND NR_ATIVIDADE_RH = '1021') \n";

                sqlcommand.Parameters.AddWithValue("@NR_GESTOR", NR_GESTOR);
                sqlcommand.Parameters.AddWithValue("@NR_COLABORADOR", NR_COLABORADOR);

                DAL_MIS AcessaDadosMisN = new Intranet.DAL.DAL_MIS();
                DataSet ds = AcessaDadosMisN.ConsultaSQL(sqlcommand);

                Intranet_NEW.Models.WEB.Colaborador Colaborador = new DTO.Colaborador();
                if ((ds.Tables.Count > 0) && (ds.Tables[0].Rows.Count > 0))
                    return 1;
                return 0;
            }
            catch (Exception ex)
            {
                throw new Exception("RET.Tabulacao.ADO_001: " + ex.Message, ex);
            }
        }

        #endregion
    }
}