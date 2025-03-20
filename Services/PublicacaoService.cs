using Intranet_NEW.DAL;
using Intranet_NEW.Models.WEB;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Tweetinvi.Core.Models;

namespace Intranet_NEW.Services
{
    public class PublicacaoService
    {
        private readonly DAL_MIS _daoMis;
        public PublicacaoService() {
        
            _daoMis = new DAL_MIS();

        }

        public PublicacaoModel MontaPublicacao(DataRow row,int idUsuario)
        {
            if (row == null)
                throw new ArgumentNullException(nameof(row), "O DataRow fornecido é nulo.");

            int reacao = VerificaReacao(idUsuario, row.Field<int>("ID"));

            return new PublicacaoModel
            {
                Id = row.Field<int>("ID"),
                Titulo = row.Field<string>("TITULO") ?? string.Empty,
                Conteudo = row.Field<string>("CONTEUDO") ?? string.Empty,
                Curtidas = row["CURTIDAS"] == DBNull.Value ? 0 : row.Field<int>("CURTIDAS"),
                Descurtidas = row["DESCURTIDAS"] == DBNull.Value ? 0 : row.Field<int>("DESCURTIDAS"),
                Carteira = row.Field<string>("CARTEIRA") ?? string.Empty,
                Autor = row.Field<string>("NM_COLABORADOR") ?? string.Empty,
                IdAutor = row["NR_COLABORADOR_AUTOR"] == DBNull.Value ? 0 : row.Field<int>("NR_COLABORADOR_AUTOR"),
                Arquivada = row["TP_EXCLUIDA"] == DBNull.Value ? 0 : row.Field<int>("TP_EXCLUIDA"),
                DataPublicacao = row.Field<DateTime>("DT_PUBLICACAO"),
                FoiReagido = reacao == 0 ? false : true,
                TipoReacao = reacao
            };
        }

        public PublicacaoModel BuscaPublicacao(int id)
        {
            PublicacaoModel publicacoes = new();
            SqlCommand command = new SqlCommand("SELECT A.*,NM_COLABORADOR FROM TBL_WEB_PUBLICACAO A \r\nJOIN TBL_WEB_COLABORADOR_DADOS B ON A.NR_COLABORADOR_AUTOR = B.NR_COLABORADOR\r\nWHERE TP_EXCLUIDA = 0 AND ID = @ID ORDER BY DT_PUBLICACAO DESC ");
            command.Parameters.Add(new SqlParameter("@ID",id));
            DataSet ds = _daoMis.ConsultaSQL(command);

            if (ds.Tables[0].Rows.Count > 0)
            {
                return MontaPublicacao(ds.Tables[0].Rows[0],0);
            }
            return null;
        }

        public void AtualizaCurtida(int id,int novoValor) 
        {
            SqlCommand command = new($"UPDATE TBL_WEB_PUBLICACAO SET CURTIDAS = @NOVO_VALOR WHERE ID = @ID ");
            command.Parameters.Add(new SqlParameter("@ID", id));
            command.Parameters.Add(new SqlParameter("@NOVO_VALOR", novoValor));
            _daoMis.ExecutaComandoSQL (command);
        }

        public void AtualizaDescurtida(int id, int novoValor)
        {
            SqlCommand command =  new($"UPDATE TBL_WEB_PUBLICACAO SET DESCURTIDAS = @NOVO_VALOR WHERE ID = @ID ");
            command.Parameters.Add(new SqlParameter("@ID", id));
            command.Parameters.Add(new SqlParameter("@NOVO_VALOR", novoValor));
            _daoMis.ExecutaComandoSQL(command);
        }

        public List<PublicacaoModel> ListaPublicacoes(int idUsuario)
        {
            List<PublicacaoModel> publicacoes = new List<PublicacaoModel>();
            SqlCommand command = new SqlCommand("SELECT A.*,NM_COLABORADOR FROM TBL_WEB_PUBLICACAO A \r\nJOIN TBL_WEB_COLABORADOR_DADOS B ON A.NR_COLABORADOR_AUTOR = B.NR_COLABORADOR\r\nWHERE TP_EXCLUIDA = 0 ORDER BY DT_PUBLICACAO DESC ");
            DataSet ds = _daoMis.ConsultaSQL(command);
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                PublicacaoModel publicacao = MontaPublicacao(row,idUsuario);
                publicacoes.Add(publicacao);
            }
            return publicacoes;
        }

        public List<PublicacaoModel> ListaPublicacoesParaFeed(string carteira,int idUsuario)
        {
            List<PublicacaoModel> publicacoes = new List<PublicacaoModel>();
            SqlCommand command = new SqlCommand("SELECT A.*,NM_COLABORADOR FROM TBL_WEB_PUBLICACAO A \r\n      JOIN TBL_WEB_COLABORADOR_DADOS B ON A.NR_COLABORADOR_AUTOR = B.NR_COLABORADOR\r\n      WHERE TP_EXCLUIDA = '0' \r\n      AND (CARTEIRA = '0' OR (CARTEIRA IN (SELECT NM_CARTEIRA FROM TBL_WEB_RH_COMBO_ATIVIDADE_ATIVA WHERE NR_ATIVIDADE = @CARTEIRA)))\r\n      ORDER BY DT_PUBLICACAO DESC   ");
            command.Parameters.Add(new SqlParameter("@CARTEIRA", carteira));
            DataSet ds = _daoMis.ConsultaSQL(command);
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                PublicacaoModel publicacao = MontaPublicacao(row,idUsuario);
                publicacoes.Add(publicacao);
            }
            return publicacoes;
        }

        public int VerificaReacao(int idUsuario,int idPub)
        {

            SqlCommand command = new("SELECT ID_REACAO FROM TBL_WEB_PUBLICACAO_REACAO WHERE ID_PUBLICACAO = @ID_PUB AND NR_USUARIO = @NR_USUARIO ");
            command.Parameters.Add(new SqlParameter("@ID_PUB", idPub));
            command.Parameters.Add(new SqlParameter("@NR_USUARIO", idUsuario));

            DataSet ds = _daoMis.ConsultaSQL(command);

            if (ds.Tables[0].Rows.Count == 0)
                return 0;
            else
                return Convert.ToInt32(ds.Tables[0].Rows[0]["ID_REACAO"]);

        }

        public void InserirPublicacao(PublicacaoModel publicacao)
        {
            if (publicacao == null)
                throw new ArgumentNullException(nameof(publicacao), "A publicação fornecida é nula.");
            SqlCommand command = new SqlCommand("INSERT INTO TBL_WEB_PUBLICACAO (CARTEIRA,TITULO, CONTEUDO,DT_PUBLICACAO,TP_EXCLUIDA,NR_COLABORADOR_AUTOR) VALUES (@CARTEIRA,@TITULO, @CONTEUDO,@DT_PUBLICACAO,@TP_EXCLUIDA,@NR_COLABORADOR_AUTOR)");
            command.Parameters.AddWithValue("@TITULO", publicacao.Titulo);
            command.Parameters.AddWithValue("@CONTEUDO", publicacao.Conteudo);
            command.Parameters.AddWithValue("@CARTEIRA", publicacao.Carteira.Trim());
            command.Parameters.AddWithValue("@DT_PUBLICACAO", publicacao.DataPublicacao);
            command.Parameters.AddWithValue("@TP_EXCLUIDA",0);
            command.Parameters.AddWithValue("@NR_COLABORADOR_AUTOR",publicacao.IdAutor);
            _daoMis.ExecutaComandoSQL(command);
        }

        public void ExcluirPublicacao(int id)
        {
            SqlCommand command = new SqlCommand("UPDATE TBL_WEB_PUBLICACAO SET TP_EXCLUIDA = 1 WHERE ID = @ID");
            command.Parameters.Add(new SqlParameter("@ID", id));
            _daoMis.ExecutaComandoSQL(command);
        }


    }
}
