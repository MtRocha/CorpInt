using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Net;
using Intranet_NEW.Controllers.DAL;


namespace Intranet.BLL.RET
{
    public class HoraHora_Script
    {
        DAL_OLOS AcessaDadosOlos = new Intranet.DAL.DAL_OLOS();

        public int ScriptHoraHora_Falando()
        {
            try
            {

                // Cria SP_REM_GERA_TABELA
                SqlCommand sqlcommand = new SqlCommand();
                StreamReader arqLeitura = new StreamReader(@"C:\Processo\01-ScriptSql\SP_OLS_GERA_RELATORIO_HORA_HORA_LIGACAO_V01.sql", Encoding.GetEncoding("ISO-8859-1"));
                sqlcommand = new SqlCommand();
                sqlcommand.CommandType = CommandType.Text;
                sqlcommand.CommandText = arqLeitura.ReadToEnd();
                AcessaDadosOlos.ExecutaComandoSQL(sqlcommand);
                arqLeitura.Close();
                arqLeitura.Dispose();

                return 1;
            }
            catch (Exception ex)
            {
                throw new Exception("RET.HoraHoraScript_001: " + ex.Message, ex);
            }

        }

        public int ScriptHoraHora_PA()
        {
            try
            {
                // Cria SP_REM_GERA_TABELA
                SqlCommand sqlcommand = new SqlCommand();
                StreamReader arqLeitura = new StreamReader(@"C:\Processo\01-ScriptSql\SP_OLS_GERA_RELATORIO_HORA_HORA_LIGACAO_V02.sql", Encoding.GetEncoding("ISO-8859-1"));
                sqlcommand = new SqlCommand();
                sqlcommand.CommandType = CommandType.Text;
                sqlcommand.CommandText = arqLeitura.ReadToEnd();
                AcessaDadosOlos.ExecutaComandoSQL(sqlcommand);
                arqLeitura.Close();
                arqLeitura.Dispose();

                return 1;
            }
            catch (Exception ex)
            {
                throw new Exception("RET.HoraHoraScript_001: " + ex.Message, ex);
            }

        }

        public int ScriptRetorno_Falando()
        {
            try
            {
                // Cria SP_REM_GERA_TABELA
                SqlCommand sqlcommand = new SqlCommand();
                StreamReader arqLeitura = new StreamReader(@"C:\Processo\01-ScriptSql\SP_RETORNO_V01.sql", Encoding.GetEncoding("ISO-8859-1"));
                sqlcommand = new SqlCommand();
                sqlcommand.CommandType = CommandType.Text;
                sqlcommand.CommandText = arqLeitura.ReadToEnd();
                AcessaDadosOlos.ExecutaComandoSQL(sqlcommand);
                arqLeitura.Close();
                arqLeitura.Dispose();

                return 1;
            }
            catch (Exception ex)
            {
                throw new Exception("RET.HoraHoraScript_001: " + ex.Message, ex);
            }

        }

        public int ScriptRetornoData_Falando()
        {
            try
            {
                // Cria SP_REM_GERA_TABELA
                SqlCommand sqlcommand = new SqlCommand();
                StreamReader arqLeitura = new StreamReader(@"C:\Processo\01-ScriptSql\SP_RETORNO_POR_DATA_V01.sql", Encoding.GetEncoding("ISO-8859-1"));
                sqlcommand = new SqlCommand();
                sqlcommand.CommandType = CommandType.Text;
                sqlcommand.CommandText = arqLeitura.ReadToEnd();
                AcessaDadosOlos.ExecutaComandoSQL(sqlcommand);
                arqLeitura.Close();
                arqLeitura.Dispose();

                return 1;
            }
            catch (Exception ex)
            {
                throw new Exception("RET.HoraHoraScript_001: " + ex.Message, ex);
            }

        }

        public int ScriptRetorno_PA()
        {
            try
            {
                // Cria SP_REM_GERA_TABELA
                SqlCommand sqlcommand = new SqlCommand();
                StreamReader arqLeitura = new StreamReader(@"C:\Processo\01-ScriptSql\SP_RETORNO_V02.sql", Encoding.GetEncoding("ISO-8859-1"));
                sqlcommand = new SqlCommand();
                sqlcommand.CommandType = CommandType.Text;
                sqlcommand.CommandText = arqLeitura.ReadToEnd();
                AcessaDadosOlos.ExecutaComandoSQL(sqlcommand);
                arqLeitura.Close();
                arqLeitura.Dispose();

                return 1;
            }
            catch (Exception ex)
            {
                throw new Exception("RET.HoraHoraScript_001: " + ex.Message, ex);
            }

        }

        public int ScriptRetornoData_PA()
        {
            try
            {
                // Cria SP_REM_GERA_TABELA
                SqlCommand sqlcommand = new SqlCommand();
                StreamReader arqLeitura = new StreamReader(@"C:\Processo\01-ScriptSql\SP_RETORNO_POR_DATA_V02.sql", Encoding.GetEncoding("ISO-8859-1"));
                sqlcommand = new SqlCommand();
                sqlcommand.CommandType = CommandType.Text;
                sqlcommand.CommandText = arqLeitura.ReadToEnd();
                AcessaDadosOlos.ExecutaComandoSQL(sqlcommand);
                arqLeitura.Close();
                arqLeitura.Dispose();

                return 1;
            }
            catch (Exception ex)
            {
                throw new Exception("RET.HoraHoraScript_001: " + ex.Message, ex);
            }

        }

        public int ExcluirScriptAnterior()
        {
            try
            {
                SqlCommand sqlcommand = new SqlCommand();
                sqlcommand.CommandType = CommandType.Text;
                sqlcommand.CommandText = "DROP PROCEDURE  SP_OLS_GERA_RELATORIO_HORA_HORA_LIGACAO";
                int retorno = AcessaDadosOlos.ExecutaComandoSQL(sqlcommand);

                //sqlcommand.CommandText = "DROP PROCEDURE  PROC_GET_ARQUIVO_RETORNO";
                //retorno = AcessaDadosOlos.ExecutaComandoSQL(sqlcommand);

                //sqlcommand.CommandText = "DROP PROCEDURE  PROC_GET_ARQUIVO_RETORNO_PER_DATE";
                //retorno = AcessaDadosOlos.ExecutaComandoSQL(sqlcommand);

                return retorno;

            }
            catch (Exception ex)
            {
                return 0;
                throw new Exception("RET.EmissaoBoleto_004: " + ex.Message, ex);
            }
        }

        public void AlterarVisao_TXT(string Visao)
        {
            string Arquivo = @"C:\Processo\01-ScriptSql\Intranet_Visao_Atual.txt";
            string Valor = "";
            using (StreamReader file = new StreamReader(Arquivo))
            {
                Valor = file.ReadToEnd();
            }

            using (StreamWriter file = new StreamWriter(Arquivo, false))
            {
                file.WriteLine(Visao);
            }

        }

        public string CarregaTela()
        {
            try
            {
                string Arquivo = @"C:\Processo\01-ScriptSql\Intranet_Visao_Atual.txt";
                string Valor = "";
                using (StreamReader file = new StreamReader(Arquivo))
                {
                    Valor = file.ReadToEnd();
                }

                return "Visão Atual = " + Valor;
            }
            catch (Exception ex)
            {
                return "";
                throw new Exception("RET.vISAO003: " + ex.Message, ex);
            }
        }


        public string valorMetaCPC()
        {
            try
            {
                string Arquivo = @"C:\Processo\01-ScriptSql\MetaCPC.txt";
                string Valor = "";
                using (StreamReader file = new StreamReader(Arquivo))
                {
                    Valor = file.ReadToEnd();
                }

                return Valor;
            }
            catch (Exception ex)
            {
                return "";
                throw new Exception("RET.vISAO003: " + ex.Message, ex);
            }
        }


    }
}
