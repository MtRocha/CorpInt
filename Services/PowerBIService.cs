using Intranet_NEW.DAL;
using Intranet_NEW.Models.WEB;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Intranet_NEW.Services
{
    public class PowerBIService
    {
        private readonly DAL_INTRANET _dalIntranet;

        public PowerBIService()
        {
            _dalIntranet = new DAL_INTRANET();
        }

        public PowerBiModel MontaDashBoard(DataRow row)
        {

            PowerBiModel powerBi = new PowerBiModel
            {
                Id = row["ID_DASH"].ToString(),
                Titulo = row["TITULO"].ToString(),
                DtCriacao = Convert.ToDateTime(row["DT_CRIACAO"]),
                Link = row["DS_LINK"].ToString(),
                idAutor = Convert.ToInt32(row["NR_USUARIO_AUTOR"].ToString()),
                NomeAutor = row["NM_COLABORADOR"].ToString(),
                TipoAcesso = Convert.ToInt32(row["TP_ACESSO"].ToString()),
                CaminhoImagem = row["DS_IMAGEM"].ToString(),
                Descricao = row["DS_DESCRICAO"].ToString(),
                IntervaloAtualizacao = Convert.ToInt32(row["NR_INT_ATUALIZACAO"])
            };
            return powerBi;


        }

        public List<PowerBiModel> ListaDashboards(int tipoAcesso,int idUsuario)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "SP_LISTA_POWERBI";
            cmd.Parameters.Add(new SqlParameter("@TP_ACESSO", tipoAcesso));
            cmd.Parameters.Add(new SqlParameter("@NR_USUARIO_AUTOR", idUsuario));
            DataSet ds = _dalIntranet.ConsultaSQL(cmd);

            List<PowerBiModel> lista = new List<PowerBiModel>();

            if (ds.Tables.Count > 0)
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {

                    lista.Add(MontaDashBoard(row));
                }
            }
            return lista;
        }

        public PowerBiModel BuscaDashBoard(int id)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "SP_BUSCA_POWERBI";
            cmd.Parameters.Add(new SqlParameter("@ID_DASH", id));
            DataSet ds = _dalIntranet.ConsultaSQL(cmd);
            if (ds.Tables.Count > 0)
            {
                return MontaDashBoard(ds.Tables[0].Rows[0]);
            }
            return null;
        }

        public void IncluiDashBoard(PowerBiModel powerBi)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "SP_INCLUI_POWERBI";

            cmd.Parameters.AddWithValue("@TITULO", powerBi.Titulo);
            cmd.Parameters.AddWithValue("@DT_CRIACAO", powerBi.DtCriacao);
            cmd.Parameters.AddWithValue("@DS_LINK", powerBi.Link);
            cmd.Parameters.AddWithValue("@NR_USUARIO_AUTOR", powerBi.idAutor);
            cmd.Parameters.AddWithValue("@TP_ACESSO", powerBi.TipoAcesso);
            cmd.Parameters.AddWithValue("@DS_DESCRICAO", powerBi.Descricao);
            cmd.Parameters.AddWithValue("@DS_IMAGEM", powerBi.CaminhoImagem);
            cmd.Parameters.AddWithValue("@NR_INT_ATUALIZACAO", powerBi.IntervaloAtualizacao);
            _dalIntranet.ExecutaComandoSQL(cmd);

        }

        public void AlteraDashBoard(PowerBiModel powerBi,int idUsuario)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "SP_ALTERAR_CONTROLE_BI";
            cmd.Parameters.AddWithValue("@ID_DASH", powerBi.Id);
            cmd.Parameters.AddWithValue("@TITULO", powerBi.Titulo);
            cmd.Parameters.AddWithValue("@DT_CRIACAO", powerBi.DtCriacao);
            cmd.Parameters.AddWithValue("@DS_LINK", powerBi.Link);
            cmd.Parameters.AddWithValue("@TP_ACESSO", powerBi.TipoAcesso);
            cmd.Parameters.AddWithValue("@DS_DESCRICAO", powerBi.Descricao);
            cmd.Parameters.AddWithValue("@NR_INT_ATUALIZACAO", powerBi.IntervaloAtualizacao);
            _dalIntranet.ExecutaComandoSQL(cmd);
            IncluiLogBI(Convert.ToInt32(powerBi.Id), "ALTERAÇÃO", idUsuario);
        }

        public void ExcluiDashBoard(int id,int idUsuario)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "SP_EXCLUIR_CONTROLE_BI";
            cmd.Parameters.AddWithValue("@ID_DASH", id);
            _dalIntranet.ExecutaComandoSQL(cmd);
            IncluiLogBI(id, "EXCLUSÃO", idUsuario);


        }

        private void IncluiLogBI(int id, string acao,int idUsuario)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "SP_INSERIR_LOG_BI";
            cmd.Parameters.AddWithValue("@ID_DASH", id);
            cmd.Parameters.AddWithValue("@TP_ALTERACAO", acao);
            cmd.Parameters.AddWithValue("@NR_USUARIO_ALTERACAO", idUsuario);

            _dalIntranet.ExecutaComandoSQL(cmd);
        }



    }
}
