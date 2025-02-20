using System;
using System.IO;
using System.Net;
using System.Diagnostics;
using System.Data.SqlClient;
using System.Data;
using System.Net.Mail;
using Google.Cloud.Storage.V1;
using Intranet_NEW.Controllers.DAL;
namespace Intranet.BLL.MON
{
    public class HVoice
    {
        private static string user = ".\\Administrador";
        private static string password = "75D4mCj6EW7ObJhp08UXTYsN";
        private NetworkCredential credenciais = new NetworkCredential(user, password);
        private string bucketName = "sv-roveri-audio-incoming";

        public void ImportarGravacoes(string callIds,string solicitante,DateTime inicio)
        {
            try
            {
                DAL_PROC_ROVERI dao = new DAL_PROC_ROVERI();
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = $"SELECT * FROM [TB_IVENTARIO_GRAVACOES] WHERE 1 = 0 {callIds} ";

                DataSet ds = dao.ConsultaSQL(cmd);
                DataTable dt = ds.Tables[0];
                DataTable dtNaoEncontrado = new DataTable();
                string destino = @"C:\Processo\TesteGrav";

                foreach (DataRow dr in dt.Rows)
                {
                    string origem = dr["NOME_COMPLETO"].ToString().Trim();  

                    destino = Path.Combine(destino, dr["NOME_ARQUIVO"].ToString().Trim());

                    MapearUnidadeDeRede(origem); 

                    if (File.Exists(origem))
                    {
                        File.Copy(origem, destino, overwrite: true);
                       
                    }
                    else
                    {
                        dtNaoEncontrado.Rows.Add(dr);
                    }
                }

                DateTime final = DateTime.Now;

                if (dtNaoEncontrado.Rows.Count > 0)
                {
                    EnvioEmailConclusao(dtNaoEncontrado,2,solicitante, inicio, final, "");
                }
                else
                {
                    EnvioEmailConclusao(dtNaoEncontrado, 1, solicitante, inicio, final, "");
                }

                

            }
            catch (Exception ex)
            {
                DataTable dtNaoEncontrado = new DataTable();
                DateTime final = DateTime.Now;
                EnvioEmailConclusao(dtNaoEncontrado, 1, solicitante, inicio, final, ex.Message.ToString());
            }
        }


        private void MapearUnidadeDeRede(string caminhoOrigem)
        {
            try
            {
                string comando = $"/C net use {caminhoOrigem} /user:{credenciais.UserName} {credenciais.Password}";
                ProcessStartInfo pro = new ProcessStartInfo("cmd", comando);
                pro.WindowStyle = ProcessWindowStyle.Hidden;
                Process.Start(pro);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
        }
        
        
        //// Layouts de Envio ////////////////////////////////
        //   0 Layout de Erro                              //
        //   1 Layout de Sucesso                           //
        //   2 Layout de Sucesso com Gravações Ausentes    //
        /////////////////////////////////////////////////////
        public void EnvioEmailConclusao(DataTable dt,int tipoEmail, string solicitante, DateTime inicio,DateTime final,string exception)
        {
            try
            {

                MAIL mail = new MAIL();
                System.Net.Mail.MailMessage mailMessage = new System.Net.Mail.MailMessage();
                //destinatarios do e-mail, para incluir mais de um basta separar por ponto e virgula
                mailMessage.To.Add("matheus.rocha@roveriadvogados.com.br");
                //mailMessage.To.Add("thaina.santos@roveriadvogados.com.br");
                //mailMessage.To.Add("renato.calixto@gruporoveri.com.br");
                mailMessage.IsBodyHtml = true;

                if (tipoEmail == 1)
                {
                    mailMessage.Subject = "Exportação de Gravações - Sucesso ";

                    #region BODY HTML

                    string body = $@"
                                    <!DOCTYPE html>
                                    <html lang='pt-BR'>
                                    <head>
                                        <meta charset='UTF-8'>
                                        <meta name='viewport' content='width=device-width, initial-scale=1.0'>
                                        <title>Assistente de Exportação de Gravações</title>
                                        <style>
                                            body {{
                                                font-family: Arial, sans-serif;
                                                background-color: #f4f4f4;
                                                color: #333;
                                                margin: 0;
                                                padding: 0;
                                            }}
                                            .container {{
                                                max-width: 600px;
                                                margin: 20px auto;
                                                background-color: #fff;
                                                padding: 20px;
                                                border-radius: 8px;
                                                box-shadow: 0 2px 10px rgba(0, 0, 0, 0.1);
                                            }}
                                            h1 {{
                                                color: #4CAF50;
                                                text-align: center;
                                            }}
                                            p {{
                                                font-size: 16px;
                                                line-height: 1.6;
                                            }}
                                            .success {{
                                                color: #4CAF50;
                                                font-weight: bold;
                                            }}
                                            .warning {{
                                                color: #FF9800;
                                                font-weight: bold;
                                            }}
                                            .footer {{
                                                text-align: center;
                                                font-size: 14px;
                                                color: #888;
                                                margin-top: 20px;
                                            }}
                                            .table {{
                                                width: 100%;
                                                border-collapse: collapse;
                                                margin-top: 20px;
                                            }}
                                            .table th, .table td {{
                                                border: 1px solid #ddd;
                                                padding: 8px;
                                                text-align: left;
                                            }}
                                            .table th {{
                                                background-color: #f2f2f2;
                                            }}
                                        </style>
                                    </head>
                                    <body>
                                        <div class='container'>
                                            <h1>Exportação Concluída</h1>
                                            <p>
                                                Prezados, <br><br>
                                                A exportação solicitada foi concluída com <span class='success'>sucesso</span>.
                                            </p>

                                            <h3>Detalhes da Exportação:</h3>
                                            <table class='table'>
                                                <tr>
                                                    <th>Solicitante</th>
                                                    <td>{solicitante}</td>
                                                </tr>
                                                <tr>
                                                    <th>Data e Hora de Início</th>
                                                    <td>{inicio.ToString("dd/MM/yyyy hh:mm:ss")}</td>
                                                </tr>
                                                <tr>
                                                    <th>Data e Hora de Conclusão</th>
                                                    <td>{final.ToString("dd/MM/yyyy hh:mm:ss")}</td>
                                                </tr>
                                            </table>

                                            <div class='footer'>
                                                <p>Atenciosamente, <br>MIS Caixa</p>
                                            </div>
                                        </div>
                                    </body>
                                    </html>";

                    #endregion
                    mailMessage.Body = body;

                    mail.Enviar(mailMessage, "RET.CmdAcionamentoHoraHora");

                }
                if (tipoEmail == 2)
                {
                    string caminho = @"C:\Processo\20-AnexoEmail\ENV_MONITORIA\\NãoEncontrados" + DateTime.Now.ToString("yyyyMMdd") + ".csv";
                    ExportacaoExcel.DataTableToCSV(dt, ';', caminho);
                    mailMessage.Attachments.Add(new Attachment(caminho));
                    mailMessage.Subject = "Exportação de Gravações - Sucesso (Gravações não Encontradas)";

                    #region BODY HTML

                    string body = $@"
                                    <!DOCTYPE html>
                                    <html lang='pt-BR'>
                                    <head>
                                        <meta charset='UTF-8'>
                                        <meta name='viewport' content='width=device-width, initial-scale=1.0'>
                                        <title>Assistente de Exportação de Gravações</title>
                                        <style>
                                            body {{
                                                font-family: Arial, sans-serif;
                                                background-color: #f4f4f4;
                                                color: #333;
                                                margin: 0;
                                                padding: 0;
                                            }}
                                            .container {{
                                                max-width: 600px;
                                                margin: 20px auto;
                                                background-color: #fff;
                                                padding: 20px;
                                                border-radius: 8px;
                                                box-shadow: 0 2px 10px rgba(0, 0, 0, 0.1);
                                            }}
                                            h1 {{
                                                color: #4CAF50;
                                                text-align: center;
                                            }}
                                            p {{
                                                font-size: 16px;
                                                line-height: 1.6;
                                            }}
                                            .success {{
                                                color: #4CAF50;
                                                font-weight: bold;
                                            }}
                                            .warning {{
                                                color: #FF9800;
                                                font-weight: bold;
                                            }}
                                            .footer {{
                                                text-align: center;
                                                font-size: 14px;
                                                color: #888;
                                                margin-top: 20px;
                                            }}
                                            .table {{
                                                width: 100%;
                                                border-collapse: collapse;
                                                margin-top: 20px;
                                            }}
                                            .table th, .table td {{
                                                border: 1px solid #ddd;
                                                padding: 8px;
                                                text-align: left;
                                            }}
                                            .table th {{
                                                background-color: #f2f2f2;
                                            }}
                                        </style>
                                    </head>
                                    <body>
                                        <div class='container'>
                                            <h1>Exportação Concluída</h1>
                                            <p>
                                                Prezados, <br><br>
                                                A exportação solicitada foi concluída com <span class='success'>sucesso</span>. No entanto, algumas gravações não foram encontradas durante o processo. 
                                            </p>
                                            <p>
                                                <span class='warning'>Aviso:</span> Algumas gravações não foram localizadas durante a exportação. O relatório detalhado com os arquivos não encontrados está disponível no arquivo CSV em anexo.
                                            </p>

                                            <h3>Detalhes da Exportação:</h3>
                                            <table class='table'>
                                                <tr>
                                                    <th>Solicitante</th>
                                                    <td>{solicitante}</td>
                                                </tr>
                                                <tr>
                                                    <th>Data e Hora de Início</th>
                                                    <td>{inicio.ToString("dd/MM/yyyy hh:mm:ss")}</td>
                                                </tr>
                                                <tr>
                                                    <th>Data e Hora de Conclusão</th>
                                                    <td>{final.ToString("dd/MM/yyyy hh:mm:ss")}</td>
                                                </tr>
                                            </table>

                                            <div class='footer'>
                                                <p>Atenciosamente, <br>MIS Caixa</p>
                                            </div>
                                        </div>
                                    </body>
                                    </html>";

                    #endregion

                    mailMessage.Body = body;

                    mail.Enviar(mailMessage, "RET.CmdAcionamentoHoraHora");
                    File.Delete(caminho);


                }
                if (tipoEmail == 0)
                {
                    mailMessage.Subject = "Exportação de Gravações - Erro ";

                    #region BODY HTML

                    string body = $@"
                                <!DOCTYPE html>
                                <html lang='pt-BR'>
                                <head>
                                    <meta charset='UTF-8'>
                                    <meta name='viewport' content='width=device-width, initial-scale=1.0'>
                                    <title>Assistente de Exportação de Gravações - Erro</title>
                                    <style>
                                        body {{
                                            font-family: Arial, sans-serif;
                                            background-color: #f4f4f4;
                                            color: #333;
                                            margin: 0;
                                            padding: 0;
                                        }}
                                        .container {{
                                            max-width: 600px;
                                            margin: 20px auto;
                                            background-color: #fff;
                                            padding: 20px;
                                            border-radius: 8px;
                                            box-shadow: 0 2px 10px rgba(0, 0, 0, 0.1);
                                        }}
                                        h1 {{
                                            color: #FF6347;
                                            text-align: center;
                                        }}
                                        p {{
                                            font-size: 16px;
                                            line-height: 1.6;
                                        }}
                                        .error {{
                                            color: #FF6347;
                                            font-weight: bold;
                                        }}
                                        .footer {{
                                            text-align: center;
                                            font-size: 14px;
                                            color: #888;
                                            margin-top: 20px;
                                        }}
                                    </style>
                                </head>
                                <body>
                                    <div class='container'>
                                        <h1>Erro na Exportação</h1>
                                        <p>
                                            Prezados, <br><br>
                                            Ocorreu um erro durante o processo de exportação. A exportação não foi concluída.
                                        </p>

                                        <h3>Detalhes do Erro:</h3>
                                        <p class='error'>
                                            {exception}
                                        </p>

                                        <div class='footer'>
                                            <p>Atenciosamente, <br>Equipe de Suporte</p>
                                        </div>
                                    </div>
                                </body>
                                </html>";

                    #endregion

                    mailMessage.Body = body;

                    mail.Enviar(mailMessage, "RET.CmdAcionamentoHoraHora");


                }
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
