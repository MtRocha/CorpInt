using Intranet_NEW.Controllers.DAL;
using Intranet_NEW.Models.WEB;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Intranet.BLL.RET.Tabulacao
{
    public class Filtro
    {
        public Colaborador VerificaPerfilUsuario(string NR_COLABORADOR)
        {
            try
            {
                SqlCommand sqlcommand = new SqlCommand();
                sqlcommand.CommandType = CommandType.Text;
                sqlcommand.CommandText = "SELECT NR_COLABORADOR, NM_COLABORADOR, TP_FUNCAO FROM TBL_WEB_COLABORADOR_DADOS WITH(NOLOCK) WHERE NR_COLABORADOR = @NR_COLABORADOR";

                sqlcommand.Parameters.AddWithValue("@NR_COLABORADOR", NR_COLABORADOR);

                DAL_MIS AcessaDadosMisN = new Intranet.DAL.DAL_MIS();
                DataSet ds = AcessaDadosMisN.ConsultaSQL(sqlcommand);

                Colaborador Colaborador = new DTO.Colaborador();
                if ((ds.Tables.Count > 0) && (ds.Tables[0].Rows.Count > 0))
                {
                    Colaborador.NR_COLABORADOR = ds.Tables[0].Rows[0]["NR_COLABORADOR"].ToString();
                    Colaborador.NM_COLABORADOR = ds.Tables[0].Rows[0]["NM_COLABORADOR"].ToString();
                    Colaborador.TP_FUNCAO = ds.Tables[0].Rows[0]["TP_FUNCAO"].ToString();
                }
                return Colaborador;
            }
            catch (Exception ex)
            {
                throw new Exception("RET.Tabulacao.ADO_001: " + ex.Message, ex);
            }
        }

        public DataTable ListaCoordenador(string DT_INI, string DT_FIM)
        {
            try
            {
                SqlCommand sqlcommand = new SqlCommand();
                sqlcommand.CommandType = CommandType.Text;
                sqlcommand.CommandText = "SELECT \n"
                                        + "    DISTINCT B.NR_COLABORADOR, B.NM_COLABORADOR, B.TP_TURNO  \n"
                                        + "FROM TBL_RET_RELATORIO_HORA_HORA_OPERADOR A WITH(NOLOCK)  \n"
                                        + "    INNER JOIN  TBL_WEB_COLABORADOR_DADOS B WITH(NOLOCK)  \n"
                                        + "        ON  B.TP_FUNCAO =  4  \n"
                                        + "        AND A.NR_COORDENADOR = B.NR_COLABORADOR  \n"
                                        + "WHERE A.DT_ACIONAMENTO BETWEEN @DT_INI AND @DT_FIM  \n"
                                        + "ORDER BY B.NM_COLABORADOR \n";

                sqlcommand.Parameters.AddWithValue("@DT_INI", DT_INI);
                sqlcommand.Parameters.AddWithValue("@DT_FIM", DT_FIM);

                DAL_MIS AcessaDadosMisN = new Intranet.DAL.DAL_MIS();
                return AcessaDadosMisN.ConsultaSQL(sqlcommand).Tables[0];
            }
            catch (Exception ex)
            {
                throw new Exception("RET.Tabulacao.ADO_001: " + ex.Message, ex);
            }
        }

        public DataTable ListaCoordenador(string DT_INI, string DT_FIM, string NR_SUPERVISOR)
        {
            try
            {
                SqlCommand sqlcommand = new SqlCommand();
                sqlcommand.CommandType = CommandType.Text;
                sqlcommand.CommandText = "SELECT \n"
                                        + "    DISTINCT B.NR_COLABORADOR, B.NM_COLABORADOR  \n"
                                        + "FROM TBL_RET_RELATORIO_HORA_HORA_OPERADOR A WITH(NOLOCK)  \n"
                                        + "    INNER JOIN  TBL_WEB_COLABORADOR_DADOS B WITH(NOLOCK)  \n"
                                        + "        ON  B.TP_FUNCAO =  4  \n"
                                        + "        AND A.NR_COORDENADOR = B.NR_COLABORADOR  \n"
                                        + "WHERE A.DT_ACIONAMENTO BETWEEN @DT_INI AND @DT_FIM  \n"
                                         + "AND  A.NR_SUPERVISOR = @NR_SUPERVISOR \n"
                                       + "ORDER BY B.NM_COLABORADOR \n";

                sqlcommand.Parameters.AddWithValue("@DT_INI", DT_INI);
                sqlcommand.Parameters.AddWithValue("@DT_FIM", DT_FIM);
                sqlcommand.Parameters.AddWithValue("@NR_SUPERVISOR", NR_SUPERVISOR);

                DAL_MIS AcessaDadosMisN = new Intranet.DAL.DAL_MIS();
                return AcessaDadosMisN.ConsultaSQL(sqlcommand).Tables[0];
            }
            catch (Exception ex)
            {
                throw new Exception("RET.Tabulacao.ADO_001: " + ex.Message, ex);
            }
        }

        public DataTable ListaSupervisor(string DT_INI, string DT_FIM, string NR_COORDENADOR)
        {
            try
            {
                SqlCommand sqlcommand = new SqlCommand();
                sqlcommand.CommandType = CommandType.Text;
                sqlcommand.CommandText = "SELECT \n"
                                        + "    DISTINCT B.NR_COLABORADOR, B.NM_COLABORADOR  \n"
                                        + "FROM TBL_RET_RELATORIO_HORA_HORA_OPERADOR A WITH(NOLOCK)  \n"
                                        + "    INNER JOIN  TBL_WEB_COLABORADOR_DADOS B WITH(NOLOCK) \n"
                                        + "        ON  B.TP_FUNCAO = 11  \n"
                                        + "        AND A.NR_SUPERVISOR = B.NR_COLABORADOR  \n"
                                        + "WHERE A.DT_ACIONAMENTO BETWEEN @DT_INI AND @DT_FIM  \n"
                                        + "AND ((@NR_COORDENADOR = '') OR (@NR_COORDENADOR <> '' AND A.NR_COORDENADOR = @NR_COORDENADOR)) \n"
                                        + "ORDER BY B.NM_COLABORADOR \n";

                sqlcommand.Parameters.AddWithValue("@DT_INI", DT_INI);
                sqlcommand.Parameters.AddWithValue("@DT_FIM", DT_FIM);
                sqlcommand.Parameters.AddWithValue("@NR_COORDENADOR", NR_COORDENADOR);

                DAL_MIS AcessaDadosMisN = new Intranet.DAL.DAL_MIS();
                return AcessaDadosMisN.ConsultaSQL(sqlcommand).Tables[0];
            }
            catch (Exception ex)
            {
                throw new Exception("RET.Tabulacao.ADO_001: " + ex.Message, ex);
            }
        }

        public DataTable ListaOperador(string DT_INI, string DT_FIM, string NR_COORDENADOR, string NR_SUPERVISOR)
        {
            try
            {
                SqlCommand sqlcommand = new SqlCommand();
                sqlcommand.CommandType = CommandType.Text;
                sqlcommand.CommandText = "SELECT \n"
                                        + "    DISTINCT B.NR_COLABORADOR, B.NM_COLABORADOR  \n"
                                        + "FROM TBL_RET_RELATORIO_HORA_HORA_OPERADOR A WITH(NOLOCK)  \n"
                                        + "    INNER JOIN  TBL_WEB_COLABORADOR_DADOS B WITH(NOLOCK)  \n"
                                        + "        ON  B.TP_FUNCAO = 10  \n"
                                        + "        AND A.NR_OPERADOR = B.NR_COLABORADOR  \n"
                                        + "WHERE A.DT_ACIONAMENTO BETWEEN @DT_INI AND @DT_FIM  \n"
                                        + "AND ((@NR_COORDENADOR = '') OR (@NR_COORDENADOR <> '' AND A.NR_COORDENADOR = @NR_COORDENADOR)) \n"
                                        + "AND ((@NR_SUPERVISOR = '') OR (@NR_SUPERVISOR <> '' AND A.NR_SUPERVISOR = @NR_SUPERVISOR)) " 
                                        + "AND B.DT_DEMISSAO = NULL \n"
                                        + "ORDER BY B.NM_COLABORADOR \n";

                sqlcommand.Parameters.AddWithValue("@DT_INI", DT_INI);
                sqlcommand.Parameters.AddWithValue("@DT_FIM", DT_FIM);
                sqlcommand.Parameters.AddWithValue("@NR_COORDENADOR", NR_COORDENADOR);
                sqlcommand.Parameters.AddWithValue("@NR_SUPERVISOR", NR_SUPERVISOR);

                DAL_MIS AcessaDadosMisN = new Intranet.DAL.DAL_MIS();
                return AcessaDadosMisN.ConsultaSQL(sqlcommand).Tables[0];
            }
            catch (Exception ex)
            {
                throw new Exception("RET.Tabulacao.ADO_001: " + ex.Message, ex);
            }
        }
    }
}
