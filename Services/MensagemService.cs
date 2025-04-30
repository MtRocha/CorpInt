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

            command.Parameters.AddWithValue("@REMETENTE", mensagem.Remetente);
            command.Parameters.AddWithValue("@DESTINATARIO", mensagem.Destinatario.HasValue ? mensagem.Destinatario.Value : DBNull.Value);
            command.Parameters.AddWithValue("@GRUPO", string.IsNullOrEmpty(mensagem.GrupoDestino) ? DBNull.Value : mensagem.GrupoDestino);
            command.Parameters.AddWithValue("@MENSAGEM", mensagem.Mensagem);
            command.Parameters.AddWithValue("@DATA", mensagem.DataEnvio);

            _dao.ExecutaComandoSQL(command);
        }

        public List<MensagemModel> ListarMensagensPorUsuario(int idUsuario, int pagina, int quantidade)
        {
            SqlCommand command = new SqlCommand("SP_LISTAR_MENSAGENS_USUARIO");
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@ID_USUARIO", idUsuario);
            command.Parameters.AddWithValue("@QUANTIDADE", quantidade);
            command.Parameters.AddWithValue("@PAGINA", pagina);
            
            DataSet ds = _dao.ConsultaSQL(command);
            return MontaMensagens(ds);
        }

        public List<MensagemModel> ListarMensagensPorGrupo(string nomeGrupo,int pagina,int quantidade)
        {
            SqlCommand command = new SqlCommand("SP_LISTAR_MENSAGENS_GRUPO");
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@GRUPO", nomeGrupo);
            command.Parameters.AddWithValue("@QUANTIDADE", quantidade);
            command.Parameters.AddWithValue("@PAGINA", pagina);

            DataSet ds = _dao.ConsultaSQL(command);
            return MontaMensagens(ds);
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

        private List<MensagemModel> MontaMensagens(DataSet ds)
        {
            List<MensagemModel> mensagens = new List<MensagemModel>();

            foreach (DataRow row in ds.Tables[0].Rows)
            {
                mensagens.Add(new MensagemModel
                {
                    Id = Convert.ToInt32(row["ID"]),
                    Remetente = Convert.ToInt32(row["NR_USUARIO_REMETENTE"]),
                    Destinatario = row["NR_USUARIO_DESTINATARIO"] != DBNull.Value ? Convert.ToInt32(row["NR_USUARIO_DESTINATARIO"]) : null,
                    GrupoDestino = row["NM_GRUPO_DESTINO"] != DBNull.Value ? row["NM_GRUPO_DESTINO"].ToString() : null,
                    Mensagem = row["DS_MENSAGEM"].ToString(),
                    DataEnvio = Convert.ToDateTime(row["DT_ENVIO"]),
                    DataVisualizado = row["DT_VISUALIZADO"] != DBNull.Value ? (DateTime?)row["DT_VISUALIZADO"] : null,
                    Lida = row["TP_LIDO"] != DBNull.Value ? Convert.ToInt32(row["TP_LIDO"]) : 0,
                    Excluida = row["TP_EXCLUIDO"] != DBNull.Value ? Convert.ToInt32(row["TP_EXCLUIDO"]) : 0
                });
            }

            return mensagens;
        }
    }
}
