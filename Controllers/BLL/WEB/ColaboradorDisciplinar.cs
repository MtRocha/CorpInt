using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Intranet.BLL.WEB
{
    public class ColaboradorDisciplinar
    {
        public DataSet ListaMotivoAcao()
        {
            try
            {
                SqlCommand sqlcommand = new SqlCommand();
                sqlcommand.CommandType = CommandType.Text;
                sqlcommand.CommandText = "SELECT NR_MOTIVO, DS_MOTIVO FROM TBL_WEB_RH_ACAO_DISCIP_MOT ORDER BY DS_MOTIVO";

                DAL.DAL_MIS_N AcessaDadosMis = new DAL.DAL_MIS_N();
                return AcessaDadosMis.ConsultaSQL(sqlcommand);
            }
            catch (Exception ex)
            {
                throw new Exception("BLL.WEB.ColaboradorDisciplinar_001: " + ex.Message, ex);
            }
        }

        public int GravaAcao(Intranet_NEW.Models.WEB.ColaboradorDisciplinar dto)
        {
            try
            {
                SqlCommand sqlcommand = new SqlCommand();
                sqlcommand.CommandType = CommandType.Text;

                DateTime dt = DateTime.MinValue;
                int num = 0;

                sqlcommand.Parameters.AddWithValue("@TP_SANCAO", int.TryParse(dto.TP_SANCAO, out num) ? (object)int.Parse(dto.TP_SANCAO) : DBNull.Value);
                sqlcommand.Parameters.AddWithValue("@NR_DIA", int.TryParse(dto.NR_DIA, out num) ? (object)int.Parse(dto.NR_DIA) : DBNull.Value);
                sqlcommand.Parameters.AddWithValue("@TP_FALTA", int.TryParse(dto.TP_FALTA, out num) ? (object)int.Parse(dto.TP_FALTA) : DBNull.Value);

                sqlcommand.Parameters.AddWithValue("@NR_ACAO", int.TryParse(dto.NR_ACAO, out num) ? (object)int.Parse(dto.NR_ACAO) : DBNull.Value);
                sqlcommand.Parameters.AddWithValue("@NR_COLABORADOR", int.TryParse(dto.NR_COLABORADOR, out num) ? (object)int.Parse(dto.NR_COLABORADOR) : DBNull.Value);
                sqlcommand.Parameters.AddWithValue("@NR_RESPONSAVEL", int.TryParse(dto.NR_RESPONSAVEL, out num) ? (object)int.Parse(dto.NR_RESPONSAVEL) : DBNull.Value);
                sqlcommand.Parameters.AddWithValue("@NR_SUPERVISOR", int.TryParse(dto.NR_SUPERVISOR, out num) ? (object)int.Parse(dto.NR_SUPERVISOR) : DBNull.Value);
                sqlcommand.Parameters.AddWithValue("@NR_ADVERTENCIA", int.TryParse(dto.NR_ADVERTENCIA, out num) ? (object)int.Parse(dto.NR_ADVERTENCIA) : DBNull.Value);
                sqlcommand.Parameters.AddWithValue("@NR_MOTIVO", int.TryParse(dto.NR_MOTIVO, out num) ? (object)int.Parse(dto.NR_MOTIVO) : DBNull.Value);
                sqlcommand.Parameters.AddWithValue("@DT_INFRACAO", DateTime.TryParse(dto.DT_INFRACAO, out dt) ? (object)DateTime.Parse(dto.DT_INFRACAO) : DBNull.Value);

                sqlcommand.CommandText = "INSERT INTO TBL_WEB_RH_ACAO_DISCIP(TP_SANCAO, DT_APLICACAO, NR_COLABORADOR, NR_RESPONSAVEL, NR_SUPERVISOR, NR_ADVERTENCIA, DT_INFRACAO, NR_MOTIVO, NR_DIA, TP_FALTA, TP_EFETIVADO, TP_EXCLUSAO) \n"
                         + "VALUES (@TP_SANCAO, GETDATE(), @NR_COLABORADOR, @NR_RESPONSAVEL, @NR_SUPERVISOR, @NR_ADVERTENCIA, @DT_INFRACAO, @NR_MOTIVO, @NR_DIA, @TP_FALTA, 0, 0) \n"
                         + "SELECT SCOPE_IDENTITY() AS [NR_ACAO]";
                DAL.DAL_MIS_N AcessaDadosMis = new DAL.DAL_MIS_N();
                DataSet ds = AcessaDadosMis.ConsultaSQL(sqlcommand);
                return int.Parse(ds.Tables[0].Rows[0][0].ToString());
            }
            catch (Exception ex)
            {
                throw new Exception("BLL.WEB.ColaboradorDisciplinar_002: " + ex.Message, ex);
            }
        }

        public DataTable PrintAcao(string NR_ACAO)
        {
            try
            {
                SqlCommand sqlcommand = new SqlCommand();
                sqlcommand.CommandType = CommandType.Text;
                sqlcommand.Parameters.AddWithValue("@NR_ACAO", NR_ACAO);
                sqlcommand.CommandText = "SELECT \n"
                                        + "	  A.DT_INFRACAO \n"
                                        + "	, A.NR_ADVERTENCIA \n"
                                        + "	, E.NR_MOTIVO \n"
                                        + "	, E.DS_MOTIVO \n"

                                        + "	, A.NR_DIA \n"
                                        + "	, A.TP_FALTA \n"

                                        + "	, B.NM_COLABORADOR AS [NM_COLABORADOR] \n"
                                        + "	, C.NM_COLABORADOR AS [NM_SUPERVISOR] \n"
                                        + "	, D.NM_COLABORADOR AS [NM_RESPONSAVEL] \n"
                                        + "FROM TBL_WEB_RH_ACAO_DISCIP A \n"
                                        + "	LEFT JOIN TBL_WEB_COLABORADOR_DADOS B ON B.NR_COLABORADOR = A.NR_COLABORADOR \n"
                                        + "	LEFT JOIN TBL_WEB_COLABORADOR_DADOS C ON C.NR_COLABORADOR = A.NR_SUPERVISOR \n"
                                        + "	LEFT JOIN TBL_WEB_COLABORADOR_DADOS D ON D.NR_COLABORADOR = A.NR_RESPONSAVEL \n"
                                        + "	LEFT JOIN TBL_WEB_RH_ACAO_DISCIP_MOT E ON E.NR_MOTIVO = A.NR_MOTIVO \n"
                                        + "WHERE NR_ACAO = @NR_ACAO \n";

                DAL.DAL_MIS_N AcessaDadosMis = new DAL.DAL_MIS_N();
                DataTable dt = AcessaDadosMis.ConsultaSQL(sqlcommand).Tables[0];

                if ((dt.Rows[0]["NR_MOTIVO"].ToString() == "5") && (int.Parse(dt.Rows[0]["NR_DIA"].ToString()) > 1))
                {
                    string NR_DIA = NumeroParaExtenso(Decimal.Parse(dt.Rows[0]["NR_DIA"].ToString())).ToUpper();
                    string tmp = string.Format("{0} FALTAS INJUSTIFICADAS {1}", NR_DIA, dt.Rows[0]["TP_FALTA"].ToString() == "1" ? "CONSECUTIVAS" : "ALTERNADAS");
                    dt.Rows[0]["DS_MOTIVO"] = tmp;
                }


                return dt;
            }
            catch (Exception ex)
            {
                throw new Exception("BLL.WEB.ColaboradorDisciplinar_003: " + ex.Message, ex);
            }
        }

        public DataTable ListaAcaoColaborador(string NR_COLABORADOR)
        {
            try
            {
                SqlCommand sqlcommand = new SqlCommand();
                sqlcommand.CommandType = CommandType.Text;
                sqlcommand.Parameters.AddWithValue("@NR_COLABORADOR", NR_COLABORADOR);
                sqlcommand.CommandText = "SELECT \n"
                                        + "   A.NR_ACAO \n"
                                        + "	, A.DT_APLICACAO \n"
                                        + "	, A.NR_ADVERTENCIA \n"
                                        + "	, A.DT_INFRACAO	 \n"
                                        + "	, B.NM_COLABORADOR	AS [NM_RESPONSAVEL] \n"
                                        + "	, C.DS_MOTIVO \n"
                                        + " , IIF(A.DS_ARQUIVO IS NULL, 0 , 1) AS [NR_UPLOAD] \n"
                                        + "FROM TBL_WEB_RH_ACAO_DISCIP A \n"
                                        + "	LEFT JOIN TBL_WEB_COLABORADOR_DADOS  B ON A.NR_RESPONSAVEL = B.NR_COLABORADOR \n"
                                        + "	LEFT JOIN TBL_WEB_RH_ACAO_DISCIP_MOT C ON A.NR_MOTIVO      = C.NR_MOTIVO \n"
                                        + "WHERE \n"
                                        + "	A.NR_COLABORADOR = @NR_COLABORADOR \n"
                                        + "	AND A.TP_EXCLUSAO = 0 \n"
                                        + "ORDER BY  \n"
                                        + "	4 \n";

                DAL.DAL_MIS_N AcessaDadosMis = new DAL.DAL_MIS_N();
                return AcessaDadosMis.ConsultaSQL(sqlcommand).Tables[0];
            }
            catch (Exception ex)
            {
                throw new Exception("BLL.WEB.ColaboradorDisciplinar_004: " + ex.Message, ex);
            }
        }

        public int VerificaAcaoTomada(string NR_COLABORADOR, string NR_ADVERTENCIA)
        {
            try
            {
                SqlCommand sqlcommand = new SqlCommand();
                sqlcommand.CommandType = CommandType.Text;
                sqlcommand.Parameters.AddWithValue("@NR_COLABORADOR", NR_COLABORADOR);
                sqlcommand.Parameters.AddWithValue("@NR_ADVERTENCIA", NR_ADVERTENCIA);
                sqlcommand.CommandText = "SELECT NR_ACAO \n"
                                        + "FROM TBL_WEB_RH_ACAO_DISCIP \n"
                                        + "WHERE NR_COLABORADOR = @NR_COLABORADOR \n"
                                        + "AND TP_EXCLUSAO = 0 AND @NR_ADVERTENCIA = NR_ADVERTENCIA AND NR_ADVERTENCIA NOT IN (5, 6) \n";

                DAL.DAL_MIS_N AcessaDadosMis = new DAL.DAL_MIS_N();
                DataSet ds = AcessaDadosMis.ConsultaSQL(sqlcommand);
                if ((ds.Tables.Count > 0) && (ds.Tables[0].Rows.Count > 0))
                    return (1);
                return (0);
            }
            catch (Exception ex)
            {
                throw new Exception("BLL.WEB.ColaboradorDisciplinar_007: " + ex.Message, ex);
            }
        }

        public Intranet_NEW.Models.WEB.ColaboradorDisciplinar ViewAcao(string NR_ACAO)
        {
            try
            {
                SqlCommand sqlcommand = new SqlCommand();
                sqlcommand.CommandType = CommandType.Text;
                sqlcommand.Parameters.AddWithValue("@NR_ACAO", NR_ACAO);
                sqlcommand.CommandText = "SELECT \n"
                                        + "	  A.NR_ACAO \n"
                                        + "	, B.NM_COLABORADOR AS [NM_COLABORADOR] \n"
                                        + "	, A.DS_ARQUIVO \n"
                                        + "FROM TBL_WEB_RH_ACAO_DISCIP A \n"
                                        + "	LEFT JOIN TBL_WEB_COLABORADOR_DADOS B ON B.NR_COLABORADOR = A.NR_COLABORADOR \n"
                                        + "WHERE NR_ACAO = @NR_ACAO \n";

                DAL.DAL_MIS_N AcessaDadosMis = new DAL.DAL_MIS_N();
                Intranet_NEW.Models.WEB.ColaboradorDisciplinar dtoDis = new DTO.ColaboradorDisciplinar();
                DataSet ds = AcessaDadosMis.ConsultaSQL(sqlcommand);
                if ((ds.Tables.Count > 0) && (ds.Tables[0].Rows.Count > 0))
                {
                    dtoDis.NR_ACAO = string.Format("{0:000000} ", (Int32)ds.Tables[0].Rows[0]["NR_ACAO"]);
                    dtoDis.NR_COLABORADOR = ds.Tables[0].Rows[0]["NM_COLABORADOR"].ToString();
                    dtoDis.DS_ARQUIVO = (byte[])ds.Tables[0].Rows[0]["DS_ARQUIVO"];
                }
                return dtoDis;
            }
            catch (Exception ex)
            {
                throw new Exception("BLL.WEB.ColaboradorDisciplinar_005: " + ex.Message, ex);
            }
        }

        public int UploadArquivo(int NR_ACAO, string NR_USUARIO, byte[] DS_ARQUIVO)
        {
            try
            {
                SqlCommand sqlcommand = new SqlCommand();
                sqlcommand.CommandType = CommandType.Text;

                DateTime dt = DateTime.MinValue;

                sqlcommand.Parameters.AddWithValue("@NR_ACAO", NR_ACAO);
                sqlcommand.Parameters.AddWithValue("@DS_ARQUIVO", DS_ARQUIVO);
                sqlcommand.Parameters.AddWithValue("@NR_USUARIO", NR_USUARIO);

                sqlcommand.CommandText = "UPDATE TBL_WEB_RH_ACAO_DISCIP SET \n"
                                        + "  DS_ARQUIVO = @DS_ARQUIVO \n"
                                        + ", TP_EFETIVADO   = 1 \n"
                                        + ", NR_USER_EFETI   = @NR_USUARIO \n"
                                        + ", DT_USER_EFETI   = GETDATE() \n"
                                        + "WHERE NR_ACAO = @NR_ACAO \n";

                DAL.DAL_MIS_N AcessaDadosMis = new DAL.DAL_MIS_N();
                return AcessaDadosMis.ExecutaComandoSQL(sqlcommand);
            }
            catch (Exception ex)
            {
                throw new Exception("BLL.WEB.ColaboradorDisciplinar_004: " + ex.Message, ex);
            }
        }


        public DataTable ListaAcaoImpressao()
        {
            try
            {
                SqlCommand sqlcommand = new SqlCommand();
                sqlcommand.CommandType = CommandType.Text;
                sqlcommand.CommandText = "SELECT \n"
                                        + "	  A.NR_ACAO \n"
                                        + "	, A.NR_ADVERTENCIA \n"
                                        + "	, A.DT_INFRACAO	 \n"
                                        + "	, B.NM_COLABORADOR	AS [NM_RESPONSAVEL] \n"
                                        + "	, D.NM_COLABORADOR	AS [NM_COLABORADOR] \n"
                                        + "	, C.DS_MOTIVO \n"
                                        + "FROM TBL_WEB_RH_ACAO_DISCIP A \n"
                                        + "	LEFT JOIN TBL_WEB_COLABORADOR_DADOS  D ON A.NR_COLABORADOR = D.NR_COLABORADOR \n"
                                        + "	LEFT JOIN TBL_WEB_COLABORADOR_DADOS  B ON A.NR_RESPONSAVEL = B.NR_COLABORADOR \n"
                                        + "	LEFT JOIN TBL_WEB_RH_ACAO_DISCIP_MOT C ON A.NR_MOTIVO      = C.NR_MOTIVO \n"
                                        + "WHERE \n"
                                        + "	A.TP_EFETIVADO = 0 \n"
                                        + " AND A.TP_EXCLUSAO = 0"
                                        + "ORDER BY 5 \n";

                DAL.DAL_MIS_N AcessaDadosMis = new DAL.DAL_MIS_N();
                return AcessaDadosMis.ConsultaSQL(sqlcommand).Tables[0];
            }
            catch (Exception ex)
            {
                throw new Exception("BLL.WEB.ColaboradorDisciplinar_006: " + ex.Message, ex);
            }
        }

        public int RemoveAdvertencia(int NR_ACAO, string NR_USUARIO)
        {
            try
            {
                SqlCommand sqlcommand = new SqlCommand();
                sqlcommand.CommandType = CommandType.Text;

                sqlcommand.Parameters.AddWithValue("@NR_ACAO", NR_ACAO);
                sqlcommand.Parameters.AddWithValue("@NR_USUARIO", NR_USUARIO);
                sqlcommand.CommandText = "UPDATE TBL_WEB_RH_ACAO_DISCIP SET \n"
                                        + "  TP_EXCLUSAO   = 1 \n"
                                        + ", NR_USER_EXCLU   = @NR_USUARIO \n"
                                        + ", DT_USER_EXCLU   = GETDATE() \n"
                                        + "WHERE NR_ACAO = @NR_ACAO \n";

                DAL.DAL_MIS_N AcessaDadosMis = new DAL.DAL_MIS_N();
                return AcessaDadosMis.ExecutaComandoSQL(sqlcommand);
            }
            catch (Exception ex)
            {
                throw new Exception("BLL.WEB.ColaboradorDisciplinar_004: " + ex.Message, ex);
            }
        }


        #region Escreve numero por extenso

        private static string[] strUnidades = { "", "um", "dois", "três", "quatro", "cinco", "seis", "sete", "oito", "nove", "dez", "onze", "doze", "treze", "quatorze", "quinze", "dezesseis", "dezessete", "dezoito", "dezenove" };
        private static string[] strDezenas = { "", "dez", "vinte", "trinta", "quarenta", "cinqüenta", "sessenta", "setenta", "oitenta", "noventa" };
        private static string[] strCentenas = { "", "cem", "duzentos", "trezentos", "quatrocentos", "quinhentos", "seiscentos", "setecentos", "oitocentos", "novecentos" };
        private static decimal decMin = 0.01M;
        private static decimal decMax = 999999999999999.99M;
        private static string strMoeda = "";
        private static string strMoedas = "";
        private static string strCentesimo = "";
        private static string strCentesimos = "";
        private static string ConversaoRecursiva(Int64 intNumero)
        {

            Int64 intResto = 0;

            if ((intNumero >= 1) && (intNumero <= 19))

                return strUnidades[intNumero];

            else if ((intNumero == 20) || (intNumero == 30) || (intNumero == 40) ||

            (intNumero == 50) || (intNumero == 60) || (intNumero == 70) ||

            (intNumero == 80) || (intNumero == 90))

                return strDezenas[Math.DivRem(intNumero, 10, out intResto)] + " ";

            else if ((intNumero >= 21) && (intNumero <= 29) ||

            (intNumero >= 31) && (intNumero <= 39) ||

            (intNumero >= 41) && (intNumero <= 49) ||

            (intNumero >= 51) && (intNumero <= 59) ||

            (intNumero >= 61) && (intNumero <= 69) ||

            (intNumero >= 71) && (intNumero <= 79) ||

            (intNumero >= 81) && (intNumero <= 89) ||

            (intNumero >= 91) && (intNumero <= 99))

                return strDezenas[Math.DivRem(intNumero, 10, out intResto)] + " e " + ConversaoRecursiva(intNumero % 10);

            else if ((intNumero == 100) || (intNumero == 200) || (intNumero == 300) ||

            (intNumero == 400) || (intNumero == 500) || (intNumero == 600) ||

            (intNumero == 700) || (intNumero == 800) || (intNumero == 900))

                return strCentenas[Math.DivRem(intNumero, 100, out intResto)] + " ";

            else if ((intNumero >= 101) && (intNumero <= 199))

                return " cento e " + ConversaoRecursiva(intNumero % 100);

            else if ((intNumero >= 201) && (intNumero <= 299) ||

            (intNumero >= 301) && (intNumero <= 399) ||

            (intNumero >= 401) && (intNumero <= 499) ||

            (intNumero >= 501) && (intNumero <= 599) ||

            (intNumero >= 601) && (intNumero <= 699) ||

            (intNumero >= 701) && (intNumero <= 799) ||

            (intNumero >= 801) && (intNumero <= 899) ||

            (intNumero >= 901) && (intNumero <= 999))

                return strCentenas[Math.DivRem(intNumero, 100, out intResto)] + " e " + ConversaoRecursiva(intNumero % 100);

            else if ((intNumero >= 1000) && (intNumero <= 999999))

                return ConversaoRecursiva(Math.DivRem(intNumero, 1000, out intResto)) + " mil " + ConversaoRecursiva(intNumero % 1000);

            else if ((intNumero >= 1000000) && (intNumero <= 1999999))

                return ConversaoRecursiva(Math.DivRem(intNumero, 1000000, out intResto)) + " milhão " + ConversaoRecursiva(intNumero % 1000000);

            else if ((intNumero >= 2000000) && (intNumero <= 999999999))

                return ConversaoRecursiva(Math.DivRem(intNumero, 1000000, out intResto)) + " milhões " + ConversaoRecursiva(intNumero % 1000000);

            else if ((intNumero >= 1000000000) && (intNumero <= 1999999999))

                return ConversaoRecursiva(Math.DivRem(intNumero, 1000000000, out intResto)) + " bilhão " + ConversaoRecursiva(intNumero % 1000000000);

            else if ((intNumero >= 2000000000) && (intNumero <= 999999999999))

                return ConversaoRecursiva(Math.DivRem(intNumero, 1000000000, out intResto)) + " bilhões " + ConversaoRecursiva(intNumero % 1000000000);

            else if ((intNumero >= 1000000000000) && (intNumero <= 1999999999999))

                return ConversaoRecursiva(Math.DivRem(intNumero, 1000000000000, out intResto)) + " trilhão " + ConversaoRecursiva(intNumero % 1000000000000);

            else if ((intNumero >= 2000000000000) && (intNumero <= 999999999999999))

                return ConversaoRecursiva(Math.DivRem(intNumero, 1000000000000, out intResto)) + " trilhões " + ConversaoRecursiva(intNumero % 1000000000000);

            else

                return "";

        }
        private static string LimpaEspacos(string strTexto)
        {

            string strRetorno = "";

            bool booUltIs32 = false;

            foreach (char chrChar in strTexto)
            {

                if ((int)chrChar != 32)
                {

                    strRetorno += chrChar;

                    booUltIs32 = false;

                }

                else if (!booUltIs32)
                {

                    strRetorno += chrChar;

                    booUltIs32 = true;

                }

            }

            return strRetorno.Trim();

        }

        public string NumeroParaExtenso(decimal decNumero)
        {
            string strRetorno = "";
            if ((decNumero >= decMin) && (decNumero <= decMax))
            {
                Int64 intInteiro = Convert.ToInt64(Math.Truncate(decNumero));
                Int64 intCentavos = Convert.ToInt64(Math.Truncate((decNumero - Math.Truncate(decNumero)) * 100));
                strRetorno += ConversaoRecursiva(intInteiro) + (string)(intInteiro <= 1 ? strMoeda : strMoedas);
                if (intCentavos > 0)
                    strRetorno += " e " + ConversaoRecursiva(intCentavos) + (string)(intCentavos == 1 ? strCentesimo : strCentesimos);
            }
            else
                return ("Zero");
            return LimpaEspacos(strRetorno);
        }

        #endregion

    }
}