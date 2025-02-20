using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;
using System.ComponentModel;
using Intranet_NEW.Controllers.DAL;

namespace Intranet.BLL.RH
{
    public class Monitoria
    {
        DAL_PROC_ROVERI AcessaDadosProc = new Intranet.DAL.DAL_PROC_ROVERI();

        public void GravaMonitoria(List<Intranet_NEW.Models.WEB.Monitoria> registros)
        {
            SqlCommand command = new SqlCommand();

            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "SP_INSERE_MONITORIA";

            try
            {
                foreach (var registro in registros)
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    command.Parameters.Clear();

                    command.Parameters.AddWithValue("@ID_OPERADOR", registro.ID_OPERADOR);
                    command.Parameters.AddWithValue("@ID_ACIONAMENTO", Convert.ToInt64(registro.ID_ACIONAMENTO));  
                    command.Parameters.AddWithValue("@CPF_CLIENTE", registro.CPF_CLIENTE);
                    command.Parameters.Add("@DATA_ACIONAMENTO", SqlDbType.DateTime).Value = registro.DATA_ACIONAMENTO;
                    command.Parameters.AddWithValue("@DURACAO_ATENDIMENTO", registro.DURACAO_ATENDIMENTO);
                    command.Parameters.AddWithValue("@NR_CONTRATO", registro.NR_CONTRATO);
                    command.Parameters.AddWithValue("@CD_ACAO", registro.CD_ACAO);
                    command.Parameters.AddWithValue("@RESULTADO", registro.RESULTADO);
                    command.Parameters.Add("@OBSERVACOES", SqlDbType.NVarChar).Value = registro.OBSERVACOES;
                    command.Parameters.AddWithValue("@SUPERVISOR", registro.SUPERVISOR);
                    command.Parameters.AddWithValue("@CALLID", registro.CALLID);
                    command.Parameters.AddWithValue("@TABULACAO", registro.TABULACAO);
                    command.Parameters.AddWithValue("@SUBTABULACAO", registro.SUBTABULACAO);
                    command.Parameters.AddWithValue("@NOME_OPERADOR", registro.NOME_OPERADOR);
                    command.Parameters.AddWithValue("@NOME_MAILING", registro.NOME_MAILING);
                    command.Parameters.AddWithValue("@NOME_PRODUTO", registro.NOME_PRODUTO);
                    command.Parameters.AddWithValue("@CD_PRODUTO", registro.CD_PRODUTO);



                    AcessaDadosProc.ExecutaComandoSQL(command);
                }
            }
            catch(Exception ex) {

                throw new Exception(ex.Message.ToString());

            }

        }

        public DataTable GeraAmostraMonitoriaOperador(string operadores,string dtRefIni,string dtRefFIm,string canal)
        {
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "SP_AMOSTRA_MONITORIA";
                cmd.Parameters.Add(new SqlParameter("@LISTA_OP", operadores));
                cmd.Parameters.Add(new SqlParameter("@TP_EXECUCAO",2));
                cmd.Parameters.Add(new SqlParameter("@CE", ""));
                cmd.Parameters.Add(new SqlParameter("@CPC", ""));
                cmd.Parameters.Add(new SqlParameter("@PP", ""));
                cmd.Parameters.Add(new SqlParameter("@IMPROD", ""));
                cmd.Parameters.Add(new SqlParameter("@INI_CE", ""));
                cmd.Parameters.Add(new SqlParameter("@INI_CPC", ""));
                cmd.Parameters.Add(new SqlParameter("@INI_PP", ""));
                cmd.Parameters.Add(new SqlParameter("@INI_IMPROD", ""));
                cmd.Parameters.Add(new SqlParameter("@FIM_CE", ""));
                cmd.Parameters.Add(new SqlParameter("@FIM_CPC", ""));
                cmd.Parameters.Add(new SqlParameter("@FIM_PP", ""));
                cmd.Parameters.Add(new SqlParameter("@FIM_IMPROD", ""));
                cmd.Parameters.Add(new SqlParameter("@DT_INI", dtRefIni));
                cmd.Parameters.Add(new SqlParameter("@DT_FIM", dtRefFIm));
                cmd.Parameters.Add(new SqlParameter("@TP_CANAL", canal));
                DataSet ds = AcessaDadosProc.ConsultaSQL(cmd);


                return ds.Tables[0];
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message.ToString()); ;
            }

            

        }

        public DataTable GeraAmostraMonitoriaTabulacao(string ce, string cpc, string pp, string improd, string iniCe, string fimCe, string iniCpc, string fimCpc, string iniPp, string fimPp, string iniImprod, string fimImprod,string inicioPeriodo, string fimPeriodo,string canal)
        {
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "SP_AMOSTRA_MONITORIA";
                cmd.Parameters.Add(new SqlParameter("@TP_EXECUCAO", 1));
                cmd.Parameters.Add(new SqlParameter("@CE" ,  ce));
                cmd.Parameters.Add(new SqlParameter("@CPC",  cpc));
                cmd.Parameters.Add(new SqlParameter("@PP" ,  pp));
                cmd.Parameters.Add(new SqlParameter("@IMPROD" ,  improd));
                cmd.Parameters.Add(new SqlParameter("@INI_CE" ,  iniCe));
                cmd.Parameters.Add(new SqlParameter("@INI_CPC",  iniCpc));
                cmd.Parameters.Add(new SqlParameter("@INI_PP",  iniPp));
                cmd.Parameters.Add(new SqlParameter("@INI_IMPROD",  iniImprod));
                cmd.Parameters.Add(new SqlParameter("@FIM_CE", fimCe));
                cmd.Parameters.Add(new SqlParameter("@FIM_CPC", fimCpc));
                cmd.Parameters.Add(new SqlParameter("@FIM_PP", fimPp));
                cmd.Parameters.Add(new SqlParameter("@FIM_IMPROD", fimImprod));
                cmd.Parameters.Add(new SqlParameter("@DT_INI",inicioPeriodo));
                cmd.Parameters.Add(new SqlParameter("@DT_FIM",fimPeriodo));
                cmd.Parameters.Add(new SqlParameter("@TP_CANAL",canal));
                cmd.Parameters.Add(new SqlParameter("@LISTA_OP", ""));

                DataSet ds = AcessaDadosProc.ConsultaSQL(cmd);

                return ds.Tables[0];
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message.ToString()); ;
            }



        }

    }
}
