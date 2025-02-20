using Intranet_NEW.Controllers.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;

namespace Intranet.BLL.WEB
{
    public class MsgOperador
    {
        public DataSet ListaMensagensSupervisor(string NR_COLABORADOR_REM, string NR_COLABORADOR_DES)
        {
            try
            {
                DateTime DT_FIM = DateTime.Now;
                DateTime DT_INI = DT_FIM.AddDays(-30);

                SqlCommand sqlcommand = new SqlCommand();
                sqlcommand.CommandType = CommandType.StoredProcedure;
                sqlcommand.Parameters.AddWithValue("@NR_COLABORADOR_REM", NR_COLABORADOR_REM);
                sqlcommand.Parameters.AddWithValue("@NR_COLABORADOR_DES", NR_COLABORADOR_DES);
                sqlcommand.Parameters.AddWithValue("@DT_INI", DT_INI.ToString("yyyyMMdd"));
                sqlcommand.Parameters.AddWithValue("@DT_FIM", DT_FIM.ToString("yyyyMMdd"));

                sqlcommand.CommandText = "SP_WEB_MSG_LISTA_SUPERVISOR";

                DAL_MIS AcessaDadosMis = new DAL.DAL_MIS();
                return AcessaDadosMis.ConsultaSQL(sqlcommand);
            }
            catch (Exception ex)
            {
                throw new Exception("BLL.WEB.MsgOperador_001: " + ex.Message, ex);
            }
        }

        public DataSet ListaMensagensQualidade(string NR_COLABORADOR_REM, string NR_COLABORADOR_DES,string TP_MENSAGEM)
        {
            try
            {
                DateTime DT_FIM = DateTime.Now;
                DateTime DT_INI = DT_FIM.AddDays(-7);

                SqlCommand sqlcommand = new SqlCommand();
                sqlcommand.CommandType = CommandType.StoredProcedure;
                sqlcommand.Parameters.AddWithValue("@NR_COLABORADOR_REM", NR_COLABORADOR_REM);
                sqlcommand.Parameters.AddWithValue("@NR_COLABORADOR_DES", NR_COLABORADOR_DES);
                sqlcommand.Parameters.AddWithValue("@TP_MENSAGEM", TP_MENSAGEM);
                sqlcommand.Parameters.AddWithValue("@DT_INI", DT_INI.ToString("yyyyMMdd"));
                sqlcommand.Parameters.AddWithValue("@DT_FIM", DT_FIM.ToString("yyyyMMdd"));

                sqlcommand.CommandText = "SP_WEB_MSG_LISTA_QUALIDADE";

                DAL_MIS AcessaDadosMis = new DAL.DAL_MIS();
                return AcessaDadosMis.ConsultaSQL(sqlcommand);
            }
            catch (Exception ex)
            {
                throw new Exception("BLL.WEB.MsgOperador_001: " + ex.Message, ex);
            }
        }

        public DataSet ListaMensagensOperador(string NR_COLABORADOR_DES)
        {
            try
            {
                DateTime DT_MSG = DateTime.Now;

                SqlCommand sqlcommand = new SqlCommand();
                sqlcommand.CommandType = CommandType.StoredProcedure;
                sqlcommand.Parameters.AddWithValue("@NR_COLABORADOR_DES", NR_COLABORADOR_DES);
                sqlcommand.Parameters.AddWithValue("@DT_INI", DT_MSG.ToString("yyyyMMdd"));
                sqlcommand.Parameters.AddWithValue("@DT_FIM", DT_MSG.ToString("yyyyMMdd"));

                sqlcommand.CommandText = "SP_WEB_MSG_LISTA_OPERADOR";

                DAL_MIS AcessaDadosMis = new DAL.DAL_MIS();
                return AcessaDadosMis.ConsultaSQL(sqlcommand);
            }
            catch (Exception ex)
            {
                throw new Exception("BLL.WEB.MsgOperador_002: " + ex.Message, ex);
            }
        }

        public DataSet ListaMensagensOperadorMonitoria(string NR_COLABORADOR_DES)
        {
            try
            {
                DateTime DT_MSG = DateTime.Now;

                SqlCommand sqlcommand = new SqlCommand();
                sqlcommand.CommandType = CommandType.StoredProcedure;
                sqlcommand.Parameters.AddWithValue("@NR_COLABORADOR_DES", NR_COLABORADOR_DES);
                sqlcommand.Parameters.AddWithValue("@NR_COLABORADOR_REM",0);
                sqlcommand.Parameters.AddWithValue("@DT_INI", DT_MSG.ToString("yyyyMMdd"));
                sqlcommand.Parameters.AddWithValue("@DT_FIM", DT_MSG.ToString("yyyyMMdd"));

                sqlcommand.CommandText = "SP_WEB_MSG_LISTA_OPERADOR_MONITORIA";

                DAL_MIS AcessaDadosMis = new DAL.DAL_MIS();
                return AcessaDadosMis.ConsultaSQL(sqlcommand);
            }
            catch (Exception ex)
            {
                throw new Exception("BLL.WEB.MsgOperador_002: " + ex.Message, ex);
            }
        }

        public DataSet ResultadoPesquisaOperador(Int64 NR_CPF)
        {
            try
            {
                DateTime DT_MSG = DateTime.Now;

                SqlCommand sqlcommand = new SqlCommand();
                sqlcommand.CommandType = CommandType.StoredProcedure;
                sqlcommand.Parameters.AddWithValue("@NR_CPF", NR_CPF);
                sqlcommand.CommandText = "SP_RESULTADO_PESQUISA_OPERADOR";

                DAL_PROC AcessaDadosMis = new DAL.DAL_PROC();
                return AcessaDadosMis.ConsultaSQL(sqlcommand);
            }
            catch (Exception ex)
            {
                throw new Exception("BLL.WEB.MsgOperador_002: " + ex.Message, ex);
            }
        }


        public DataSet ListaMensagensCoordenador(string DT_INI, string DT_FIM, string NR_COLABORADOR_REM, string NR_COLABORADOR_DES)
        {
            try
            {
                SqlCommand sqlcommand = new SqlCommand();
                sqlcommand.CommandType = CommandType.StoredProcedure;
                sqlcommand.Parameters.AddWithValue("@NR_COLABORADOR_REM", NR_COLABORADOR_REM);
                sqlcommand.Parameters.AddWithValue("@NR_COLABORADOR_DES", NR_COLABORADOR_DES);
                sqlcommand.Parameters.AddWithValue("@DT_INI", DT_INI);
                sqlcommand.Parameters.AddWithValue("@DT_FIM", DT_FIM);
                sqlcommand.Parameters.AddWithValue("@TP_GERENCIAL", 0);

                sqlcommand.CommandText = "SP_WEB_MSG_LISTA_GERENCIAL";

                DAL_MIS AcessaDadosMis = new DAL.DAL_MIS();
                return AcessaDadosMis.ConsultaSQL(sqlcommand);
            }
            catch (Exception ex)
            {
                throw new Exception("BLL.WEB.MsgOperador_003: " + ex.Message, ex);
            }
        }


        public DataSet ListaMensagensGerencia(string DT_INI, string DT_FIM)
        {
            try
            {
                SqlCommand sqlcommand = new SqlCommand();
                sqlcommand.CommandType = CommandType.StoredProcedure;
                sqlcommand.Parameters.AddWithValue("@NR_COLABORADOR_REM", 0);
                sqlcommand.Parameters.AddWithValue("@NR_COLABORADOR_DES", 0);
                sqlcommand.Parameters.AddWithValue("@DT_INI", DT_INI);
                sqlcommand.Parameters.AddWithValue("@DT_FIM", DT_FIM);
                sqlcommand.Parameters.AddWithValue("@TP_GERENCIAL", 1);

                sqlcommand.CommandText = "SP_WEB_MSG_LISTA_GERENCIAL";

                DAL_MIS AcessaDadosMis = new DAL.DAL_MIS();
                return AcessaDadosMis.ConsultaSQL(sqlcommand);
            }
            catch (Exception ex)
            {
                throw new Exception("BLL.WEB.MsgOperador_003: " + ex.Message, ex);
            }
        }

        public int GravaMensagensOperador(Intranet_NEW.Models.WEB.MsgOperador dto)
        {
            try
            {
                DateTime DT_FIM = DateTime.Now;
                DateTime DT_INI = DT_FIM.AddDays(-30);

                SqlCommand sqlcommand = new SqlCommand();
                sqlcommand.CommandType = CommandType.StoredProcedure;
                sqlcommand.Parameters.AddWithValue("@NR_COLABORADOR_REM", dto.NR_COLABORADOR_REM);
                sqlcommand.Parameters.AddWithValue("@NR_COLABORADOR_DES", dto.NR_COLABORADOR_DES);
                sqlcommand.Parameters.AddWithValue("@NM_MENSAGEM", dto.NM_MENSAGEM);

                sqlcommand.CommandText = "SP_WEB_MSG_GRAVA_OPERADOR";

                DAL_MIS AcessaDadosMis = new DAL.DAL_MIS();
                return AcessaDadosMis.ExecutaComandoSQL(sqlcommand);
            }
            catch (Exception ex)
            {
                throw new Exception("BLL.WEB.MsgOperador_004: " + ex.Message, ex);
            }
        }

        public int MarcaComoLidaMensagensOperador(string NR_MENSAGEM)
        {
            try
            {
                DateTime DT_FIM = DateTime.Now;
                DateTime DT_INI = DT_FIM.AddDays(-30);

                SqlCommand sqlcommand = new SqlCommand();
                sqlcommand.CommandType = CommandType.Text;
                sqlcommand.Parameters.AddWithValue("@NR_MENSAGEM", NR_MENSAGEM);

                sqlcommand.CommandText = "UPDATE TBL_WEB_MSG_OPERADOR SET \n"
                                        + "	TP_DESTINATARIO_LEU = 1 \n"
                                        + "WHERE \n"
                                        + "	NR_MENSAGEM = @NR_MENSAGEM";

                DAL_MIS AcessaDadosMis = new DAL.DAL_MIS();
                return AcessaDadosMis.ExecutaComandoSQL(sqlcommand);
            }
            catch (Exception ex)
            {
                throw new Exception("BLL.WEB.MsgOperador_005: " + ex.Message, ex);
            }
        }

        public int GravaMensagensOperadorTodos(Intranet_NEW.Models.WEB.MsgOperador dto)
        {
            try
            {
                DateTime DT_FIM = DateTime.Now;
                DateTime DT_INI = DT_FIM.AddDays(-30);

                SqlCommand sqlcommand = new SqlCommand();
                sqlcommand.CommandType = CommandType.StoredProcedure;
                sqlcommand.Parameters.AddWithValue("@NR_COLABORADOR_REM", dto.NR_COLABORADOR_REM);
                sqlcommand.Parameters.AddWithValue("@NR_COLABORADOR_DES", dto.NR_COLABORADOR_DES);
                sqlcommand.Parameters.AddWithValue("@NM_MENSAGEM", dto.NM_MENSAGEM);

                sqlcommand.CommandText = "SP_WEB_MSG_GRAVA_OPERADOR";

                DAL_MIS AcessaDadosMis = new DAL.DAL_MIS();
                return AcessaDadosMis.ExecutaComandoSQL(sqlcommand);
            }
            catch (Exception ex)
            {
                throw new Exception("BLL.WEB.MsgOperador_004: " + ex.Message, ex);
            }
        }
    }
}