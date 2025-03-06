using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Mail;
using System.Net;

namespace Intranet_NEW.DAL
{
    public class MAIL
    {
        private static MAIL instancia;

        private static string servidor = "200.219.212.5";
        private static int porta = 587;
        private static NetworkCredential Credencial = new NetworkCredential("jmorais@exitobr.com.br", "abc123");

        private static int ContTentativa = 0;

        public static MAIL Instancia
        {
            get
            {
                if (instancia == null)
                    instancia = new MAIL();
                return instancia;
            }
        }

        #region Métodos de envio utilizando servidor

        /// <summary>
        /// Metodo responsavel pelo envio de emails
        /// </summary>
        /// <param name="objEmail"> Dados do e-mail</param>
        /// <param name="rotina">Rotina que disparou o e-mail</param>
        /// <returns> 1 = enviado / 0 = não enviado</returns>
        public int Enviar(MailMessage objEmail, string rotina)
        {
            SmtpClient objSmtp = new SmtpClient(servidor, porta);

            //objSmtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            objSmtp.UseDefaultCredentials = false;
            objSmtp.Credentials = Credencial;

            //Define o remetente do e-mail.
            objEmail.From = new MailAddress("Processamento Diario <jmorais@exitobr.com.br>");

            objEmail.IsBodyHtml = true;
            objEmail.SubjectEncoding = Encoding.GetEncoding("ISO-8859-1");
            objEmail.BodyEncoding = Encoding.GetEncoding("ISO-8859-1");
            objEmail.Priority = MailPriority.Normal;

            try
            {
                objSmtp.Send(objEmail);
                objEmail.Dispose();
                return 1;
            }
            catch (Exception ex)
            {
                if (ContTentativa < 2)
                {
                    ContTentativa++;
                    Enviar(objEmail, rotina);
                }
                else
                {
                    objEmail.Dispose();
                    throw new Exception("Envio Email:" + ex.Message, ex);
                }
            }
            return 0;
        }

        /// <summary>
        /// Metodo responsavel pelo envio de emails de erro
        /// </summary>
        /// <param name="objEmail"> Dados do e-mail</param>
        /// <param name="rotina">Rotina que disparou o e-mail</param>
        /// <returns> 1 = enviado / 0 = não enviado</returns>
        public int Erro(MailMessage objEmail, string rotina)
        {
            SmtpClient objSmtp = new SmtpClient(servidor, porta);

            //objSmtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            objSmtp.UseDefaultCredentials = false;
            objSmtp.Credentials = Credencial;

            //Define o remetente do e-mail.
            objEmail.From = new MailAddress("Processamento Diario <jmorais@exitobr.com.br>");

            objEmail.IsBodyHtml = true;
            objEmail.SubjectEncoding = Encoding.GetEncoding("ISO-8859-1");
            objEmail.BodyEncoding = Encoding.GetEncoding("ISO-8859-1");
            objEmail.Priority = MailPriority.High;

            try
            {
                objSmtp.Send(objEmail);
                objEmail.Dispose();
                return 1;
            }
            catch (Exception ex)
            {
                objEmail.Dispose();
                throw new Exception("Envio Email:" + ex.Message, ex);
            }
        }

        #endregion Métodos de envio utilizando servidor

        public static int EnviarEmailCentralNegociosCaixa(string assunto, string destinatario, string mensagem)
        {
            try
            {
                MailMessage mailMessage = new MailMessage();
                //Endereço que irá aparecer no e-mail do usuário
                mailMessage.From = new MailAddress("centraldenegocioscaixa@exitobr.com.br", "Central de Negocios da CAIXA");

                //destinatarios do e-mail, para incluir mais de um basta separar por ponto e virgula
                mailMessage.To.Add(destinatario);
                mailMessage.Subject = assunto;
                mailMessage.IsBodyHtml = true;

                //conteudo do corpo do e-mail
                mailMessage.Body = mensagem;
                mailMessage.Priority = MailPriority.High;

                //smtp do e-mail que irá enviar
                SmtpClient smtpClient = new SmtpClient("200.219.212.5");
                smtpClient.EnableSsl = false;

                //Email Entrada --- 200.219.212.4

                //credenciais da conta que utilizará para enviar o e-mail
                smtpClient.Credentials = new NetworkCredential("centraldenegocioscaixa@exitobr.com.br", "caixa123");
                smtpClient.Send(mailMessage);
                return 1;
            }
            catch
            {
                return 0;
            }
        }

        public static int SendMail(string assunto, string destinatario, string mensagem)
        {
            try
            {
                MailMessage mailMessage = new MailMessage();
                //Endereço que irá aparecer no e-mail do usuário
                mailMessage.From = new MailAddress("jmorais@exitobr.com.br", "MIS");

                //destinatarios do e-mail, para incluir mais de um basta separar por ponto e virgula
                mailMessage.To.Add(destinatario);
                mailMessage.Subject = assunto;
                mailMessage.IsBodyHtml = false;

                //conteudo do corpo do e-mail
                mailMessage.Body = mensagem;
                mailMessage.Priority = MailPriority.High;

                //smtp do e-mail que irá enviar
                SmtpClient smtpClient = new SmtpClient("200.219.212.5");
                smtpClient.EnableSsl = false;

                //Email Entrada --- 200.219.212.4

                //credenciais da conta que utilizará para enviar o e-mail
                smtpClient.Credentials = new NetworkCredential("jmorais@exitobr.com.br", "abc123");
                smtpClient.Send(mailMessage);
                return 1;
            }
            catch
            {
                return 0;
            }
        }
    }
}