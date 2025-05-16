using Intranet_NEW.DAL;
using Intranet_NEW.Models.WEB;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Intranet_NEW.Services
{
    public class MensagemService
    {
        private readonly DAL_INTRANET _dao;

        public MensagemService()
        {
            _dao = new DAL_INTRANET();
        }

        public void InserirMensagem(MensagemModel mensagem)
        {
            SqlCommand command = new SqlCommand("SP_INSERIR_MENSAGEM");
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@REMETENTE", mensagem.IdRemetente);
            command.Parameters.AddWithValue("@DESTINATARIO", mensagem.Destinatario.HasValue ? mensagem.Destinatario.Value : DBNull.Value);
            command.Parameters.AddWithValue("@GRUPO", string.IsNullOrEmpty(mensagem.GrupoDestino) ? DBNull.Value : mensagem.GrupoDestino);
            command.Parameters.AddWithValue("@MENSAGEM", mensagem.Mensagem);
            command.Parameters.AddWithValue("@DATA", mensagem.DataEnvio);
            command.Parameters.AddWithValue("@NM_CARTEIRA ", mensagem.Carteira);

            _dao.ExecutaComandoSQL(command);
        }

        public List<MensagemModel> ListarMensagensPorUsuario(int idUsuario, int pagina, int quantidade)
        {
            SqlCommand command = new SqlCommand("SP_LISTAR_MENSAGENS_USUARIO");
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@ID_USUARIO", idUsuario);
            command.Parameters.AddWithValue("@QUANTIDADE", quantidade);
            command.Parameters.AddWithValue("@PAGINA", pagina);

            List<MensagemModel> mensagens = new List<MensagemModel>();
            DataSet ds = _dao.ConsultaSQL(command);

            foreach (DataRow row in ds.Tables[0].Rows)
            {
                mensagens.Add(MontaMensagens(row));
            }

            return mensagens;
        }

        public List<MensagemModel> ListarMensagensPorGrupo(string nomeGrupo,int gestor, int quantidade,int pagina,string carteira)
        {
            SqlCommand command = new SqlCommand("SP_LISTAR_MENSAGENS_GRUPO");
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@GRUPO", nomeGrupo);
            command.Parameters.AddWithValue("@QUANTIDADE", quantidade);
            command.Parameters.AddWithValue("@PAGINA", pagina);
            command.Parameters.AddWithValue("@COD_GESTOR", gestor);
            command.Parameters.AddWithValue("@CARTEIRA", carteira);

            List<MensagemModel> mensagens = new List<MensagemModel>();
            DataSet ds = _dao.ConsultaSQL(command);

            foreach (DataRow row in ds.Tables[0].Rows)
            {
                mensagens.Add(MontaMensagens(row));
            }

            return mensagens;
        }

        public MensagemModel BuscaUltimaMensagem(string nomeGrupo, int gestor,string carteira)
        {
            SqlCommand command = new SqlCommand("SP_BUSCA_MENSAGEM_ATUAL_GRUPO");
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@GRUPO", nomeGrupo);
            command.Parameters.AddWithValue("@COD_GESTOR", gestor);
            command.Parameters.AddWithValue("@CARTEIRA", carteira);

            DataSet ds = _dao.ConsultaSQL(command);

            if (ds.Tables[0].Rows.Count == 0)
            {
                return null;
            }

            return MontaMensagens(ds.Tables[0].Rows[0]);
        }

        public int ObterQuantidadeMensagensNaoLidas(int idUsuario, string grupoDestino,string carteira)
        {
            SqlCommand command = new SqlCommand("SP_VERIFICAR_MENSAGENS_NAO_LIDAS");
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@ID_USUARIO", idUsuario);
            command.Parameters.AddWithValue("@GRUPO_DESTINO", grupoDestino);
            command.Parameters.AddWithValue("@CARTEIRA", carteira);

            DataSet ds = _dao.ConsultaSQL(command);
            return Convert.ToInt32(ds.Tables[0].Rows[0]["QTDE"]);
        }


        public void MarcarComoLida(int idMensagem, DateTime dataVisualizado)
        {
            SqlCommand command = new SqlCommand("SP_ATUALIZAR_STATUS_LEITURA");
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@ID_MENSAGEM", idMensagem);
            command.Parameters.AddWithValue("@DATA_LEITURA", dataVisualizado);

            _dao.ExecutaComandoSQL(command);
        }

        public void ExcluirMensagem(int idMensagem)
        {
            SqlCommand command = new SqlCommand("SP_EXCLUIR_MENSAGEM");
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@ID_MENSAGEM", idMensagem);

            _dao.ExecutaComandoSQL(command);
        }

        public int ContarMensagens(string canal)
        {
            SqlCommand command = new SqlCommand("SELECT COUNT(*) AS QTDE FROM TBL_WEB_MENSAGENS WHERE NM_GRUPO_DESTINO = @CANAL AND TP_EXCLUIDO = 0 AND TP_LIDO = 0");
            command.CommandType = CommandType.Text;
            command.Parameters.AddWithValue("@CANAL", canal);
            DataSet ds = _dao.ConsultaSQL(command);
            if (ds.Tables[0].Rows.Count == 0)
            {
                return 0;
            }
            return Convert.ToInt32(ds.Tables[0].Rows[0]["QTDE"]);


        }
        private MensagemModel MontaMensagens(DataRow mensagem)
        {
            MensagemModel model = new();
            model.Id = Convert.ToInt32(mensagem["ID"]);
            model.IdRemetente = Convert.ToInt32(mensagem["NR_USUARIO_REMETENTE"]);
            model.Remetente = mensagem["NM_COLABORADOR"].ToString();
            model.Destinatario = mensagem["NR_USUARIO_DESTINATARIO"] != DBNull.Value ? Convert.ToInt32(mensagem["NR_USUARIO_DESTINATARIO"]) : null;
            model.GrupoDestino = mensagem["NM_GRUPO_DESTINO"] != DBNull.Value ? mensagem["NM_GRUPO_DESTINO"].ToString() : null;
            model.Mensagem = mensagem["DS_MENSAGEM"].ToString();
            model.DataEnvio = Convert.ToDateTime(mensagem["DT_ENVIO"]);
            model.DataVisualizado = mensagem["DT_VISUALIZADO"] != DBNull.Value ? (DateTime?)mensagem["DT_VISUALIZADO"] : null;
            model.Lida = mensagem["TP_LIDO"] != DBNull.Value ? Convert.ToInt32(mensagem["TP_LIDO"]) : 0;
            model.Excluida = mensagem["TP_EXCLUIDO"] != DBNull.Value ? Convert.ToInt32(mensagem["TP_EXCLUIDO"]) : 0;
            return model;
        }

        public void MarcarComoLida(int idUsuario, string grupoDestino, DateTime? dataReferencia = null)
        {
            using (SqlCommand command = new SqlCommand("SP_ATUALIZAR_STATUS_LEITURA"))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@ID_USUARIO", idUsuario);
                command.Parameters.AddWithValue("@GRUPO_DESTINO", grupoDestino);

                if (dataReferencia.HasValue)
                    command.Parameters.AddWithValue("@DATA_REFERENCIA", dataReferencia.Value);
                else
                    command.Parameters.AddWithValue("@DATA_REFERENCIA", DBNull.Value);

                _dao.ExecutaComandoSQL(command);
            }
        }

    }
}
