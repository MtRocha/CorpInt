using System;
using System.Text;
using System.Security.Cryptography;

namespace Intranet.BLL.WEB
{
    public class Seguranca
    {
        private string Chave = "41D4C75D2D56416CD53A374D6CFB117E11522D02";

        /// <summary>
        /// Responsavel por criptografar uma string
        /// </summary>
        /// <param name="Variavel">Valor</param>
        /// <returns>Retorna valor criptografado</returns>
        public string Criptmd5(string Variavel)
        {
            byte[] keyArray;
            byte[] toEncryptArray = UTF8Encoding.UTF8.GetBytes(Variavel);

            System.Configuration.AppSettingsReader settingsReader = new System.Configuration.AppSettingsReader();
            string key = Chave;
            MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
            keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
            hashmd5.Clear();

            TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
            tdes.Key = keyArray;
            tdes.Mode = CipherMode.ECB;

            tdes.Padding = PaddingMode.PKCS7;

            ICryptoTransform cTransform = tdes.CreateEncryptor();
            byte[] resultArray =
              cTransform.TransformFinalBlock(toEncryptArray, 0,
              toEncryptArray.Length);
            tdes.Clear();
            string code = Convert.ToBase64String(resultArray, 0, resultArray.Length);

            return code;
        }

        /// <summary>
        /// Responsavel por descriptografar uma string
        /// </summary>
        /// <param name="Variavel">Valor criptografado</param>
        /// <returns>Retorna valor descriptografado</returns>
        public string Descriptmd5(string Variavel)
        {
            byte[] keyArray;

            byte[] toEncryptArray = Convert.FromBase64String(Variavel);

            System.Configuration.AppSettingsReader settingsReader = new System.Configuration.AppSettingsReader();
            string key = Chave;

            MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
            keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));

            hashmd5.Clear();

            TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
            tdes.Key = keyArray;

            tdes.Mode = CipherMode.ECB;
            tdes.Padding = PaddingMode.PKCS7;

            ICryptoTransform cTransform = tdes.CreateDecryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(
                                 toEncryptArray, 0, toEncryptArray.Length);
            tdes.Clear();

            return UTF8Encoding.UTF8.GetString(resultArray);
        }

        /// <summary>
        /// Responsavel por criptografar a senha informada pelo usuario
        /// </summary>
        /// <param name="entrada">Valor enviado</param>
        /// <returns>Retorna senha criptografada</returns>
        public string Criptsha1(string entrada)
        {
            SHA1CryptoServiceProvider sha1 = new SHA1CryptoServiceProvider();
            byte[] to_be_hash = ASCIIEncoding.Default.GetBytes(entrada);
            byte[] hash = sha1.ComputeHash(to_be_hash);
            string delimitedHexHash = BitConverter.ToString(hash);
            string hexHash = delimitedHexHash.Replace("-", "");

            return hexHash;
        }
        
        public string GeraAleatorio()
        {
            string guid = Guid.NewGuid().ToString().Replace("-", "");

            Random clsRan = new Random();
            Int32 tamanhoSenha = clsRan.Next(6, 8);

            string senha = "";
            for (Int32 i = 0; i <= tamanhoSenha; i++)
                senha += guid.Substring(clsRan.Next(1, guid.Length), 1);

            return senha;
        }
    }
}
