using Intranet_NEW.DAL;
using Intranet_NEW.Models.WEB;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Intranet_NEW.Services
{
    public class PublicacaoService
    {
        private readonly DAL_MIS _daoMis;
        public PublicacaoService() {
        
            _daoMis = new DAL_MIS();

        }

        public PublicacaoModel MontaPublicacao(DataRow row)
        {
            if (row == null)
                throw new ArgumentNullException(nameof(row), "O DataRow fornecido é nulo.");

            return new PublicacaoModel
            {
                Id = row.Field<int>("ID"),
                Titulo = row.Field<string>("TITULO") ?? string.Empty,
                Conteudo = row.Field<string>("CONTEUDO") ?? string.Empty,
                Curtidas = row.Field<int>("CURTIDAS"),
                Descurtidas = row.Field<int>("DESCURTIDAS"),
                Carteira = new CarteiraModel
                 {
                    Name = row.Field<string>("CARTEIRA") ?? string.Empty,
                    Id = row.Field<string>("NM_CARTEIRA") ?? string.Empty
                 }
            };
        }

        public List<PublicacaoModel> ListaPublicacoes()
        {
            List<PublicacaoModel> publicacoes = new List<PublicacaoModel>();
            SqlCommand command = new SqlCommand("SELECT * FROM TBL_WEB_PUBLICACAO");
            DataSet ds = _daoMis.ConsultaSQL(command);
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                PublicacaoModel publicacao = MontaPublicacao(row);
                publicacoes.Add(publicacao);
            }
            return publicacoes;
        }

        public void InserirPublicacao(PublicacaoModel publicacao)
        {
            if (publicacao == null)
                throw new ArgumentNullException(nameof(publicacao), "A publicação fornecida é nula.");
            SqlCommand command = new SqlCommand("INSERT INTO TBL_WEB_PUBLICACAO (TITULO, CONTEUDO, CURTIDAS, DESCURTIDAS, ID_CARTEIRA) VALUES (@TITULO, @CONTEUDO, @CURTIDAS, @DESCURTIDAS, @ID_CARTEIRA)");
            command.Parameters.AddWithValue("@TITULO", publicacao.Titulo);
            command.Parameters.AddWithValue("@CONTEUDO", publicacao.Conteudo);
            command.Parameters.AddWithValue("@CURTIDAS", publicacao.Curtidas);
            command.Parameters.AddWithValue("@DESCURTIDAS", publicacao.Descurtidas);
            command.Parameters.AddWithValue("@ID_CARTEIRA", publicacao.Carteira.Id);
            _daoMis.ExecutaComandoSQL(command);
        }
    }
}
