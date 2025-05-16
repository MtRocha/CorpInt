using Intranet_NEW.Models.WEB;
using Intranet_NEW.DAL;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Intranet_NEW.Services
{
    public class CanalService
    {
        private readonly DAL_INTRANET _dao;
        private readonly MensagemService _mensagemService;

        public CanalService()
        {
            _mensagemService = new MensagemService();
            _dao = new DAL_INTRANET();
        }

        public void InserirCanal(CanalModel canal)
        {
            SqlCommand cmd = new SqlCommand("SP_INSERIR_CANAL");
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@NM_GRUPO", canal.Nome);
            cmd.Parameters.AddWithValue("@TP_PRIORIDADE_ACESSO", canal.TipoAcesso);
            cmd.Parameters.AddWithValue("@COD_FUNCAO", canal.TipoFuncao);

            _dao.ExecutaComandoSQL(cmd);
        }

        public void AtualizarCanal(CanalModel canal)
        {
            SqlCommand cmd = new SqlCommand("SP_ATUALIZAR_CANAL");
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@ID", canal.IdCanal);
            cmd.Parameters.AddWithValue("@NM_GRUPO", canal.Nome);
            cmd.Parameters.AddWithValue("@TP_PRIORIDADE_ACESSO", canal.TipoAcesso);
            cmd.Parameters.AddWithValue("@COD_FUNCAO", canal.TipoFuncao);

            _dao.ExecutaComandoSQL(cmd);
        }

        public void ExcluirCanal(int id)
        {
            SqlCommand cmd = new SqlCommand("SP_EXCLUIR_CANAL");
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ID", id);

            _dao.ExecutaComandoSQL(cmd);
        }

        public List<CanalModel> ListarPorFuncao(int codFuncao,int tipoAcesso,int id_usuario,string carteira)
        {
            SqlCommand cmd = new SqlCommand("SP_LISTAR_CANAIS_FUNCAO");
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@TP_ACESSO", tipoAcesso);
            cmd.Parameters.AddWithValue("@COD_FUNCAO", codFuncao);

            List<CanalModel> canais = new();
            DataSet ds = _dao.ConsultaSQL(cmd);
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                canais.Add(MontaCanais(row,id_usuario,carteira));
            }
            return canais; 

        }

        private CanalModel MontaCanais(DataRow row,int idUsuario,string carteira)
        {
            var model = new CanalModel();
            MensagemModel ultimaMensagem = _mensagemService.BuscaUltimaMensagem(row["NM_GRUPO"].ToString(),0,carteira);

            model.IdCanal = Convert.ToInt32(row["ID"]);
            model.Nome = row["NM_GRUPO"].ToString();
            model.NomeExibicao = row["NM_CANAL"].ToString();
            model.TipoAcesso = Convert.ToInt32(row["TP_PRIORIDADE_ACESSO"]);
            model.TipoFuncao = row["COD_FUNCAO"] == DBNull.Value ? 0 : Convert.ToInt32(row["COD_FUNCAO"]);
            model.UltimaMensagem = ultimaMensagem;
            model.PeriodoMensagem = ultimaMensagem?.DataEnvio.ToString("dd/MM/yyyy HH:mm");
            model.QtdMensagens = _mensagemService.ObterQuantidadeMensagensNaoLidas(idUsuario, row["NM_GRUPO"].ToString(), carteira);

            return model;
        }

    }
}

