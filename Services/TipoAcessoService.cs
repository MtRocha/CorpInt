using Intranet_NEW.DAL;
using Intranet_NEW.Models.WEB;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Intranet_NEW.Services
{
    public class TipoAcessoService
    {

        private readonly DAL_INTRANET _dal_intranet;

        public TipoAcessoService()
        {
            _dal_intranet = new DAL_INTRANET();
        }

        public TipoAcessoModel MontaTipoAcesso(DataRow row)
        {
            TipoAcessoModel tipoAcesso = new TipoAcessoModel
            {
                Id = Convert.ToInt32(row["COD_FUNCAO"]),
                Nome = row["NM_HIERARQUIA"].ToString()
            };
            return tipoAcesso;
        }

        public List<TipoAcessoModel> ListaTipoAcesso()
        {
            SqlCommand cmd = new();
            cmd.CommandType = System.Data.CommandType.Text;
            cmd.CommandText = " SP_LISTAR_FUNCOES_HIERARQUIA";
            DataSet ds = _dal_intranet.ConsultaSQL(cmd);
            List<TipoAcessoModel> listaTipoAcesso = new();
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                listaTipoAcesso.Add(MontaTipoAcesso(row));
            }
            return listaTipoAcesso;
        }
        public void InserirFuncaoHierarquia(TipoAcessoModel model)
        {
            SqlCommand cmd = new();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_INSERIR_FUNCAO_HIERARQUIA";
            cmd.Parameters.AddWithValue("@COD_FUNCAO", model.Id);
            cmd.Parameters.AddWithValue("@NM_HIERARQUIA", model.Nome);

            _dal_intranet.ExecutaComandoSQL(cmd);
        }

        public void AlterarFuncaoHierarquia(TipoAcessoModel model)
        {
            SqlCommand cmd = new();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_ALTERAR_FUNCAO_HIERARQUIA";
            cmd.Parameters.AddWithValue("@COD_FUNCAO", model.Id);
            cmd.Parameters.AddWithValue("@NM_HIERARQUIA", model.Nome);

            _dal_intranet.ExecutaComandoSQL(cmd);
        }

        public void ExcluirFuncaoHierarquia(TipoAcessoModel model)
        {
            SqlCommand cmd = new();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_EXCLUIR_FUNCAO_HIERARQUIA";
            cmd.Parameters.AddWithValue("@COD_FUNCAO", model.Id);
            _dal_intranet.ExecutaComandoSQL(cmd);
        }

        public TipoAcessoModel BuscarTipoAcesso(int id)
        {

            SqlCommand cmd = new();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_OBTER_FUNCAO_HIERARQUIA";
            cmd.Parameters.AddWithValue("@COD_FUNCAO", id);
            DataSet ds = _dal_intranet.ConsultaSQL(cmd);
            TipoAcessoModel tipoAcesso = new TipoAcessoModel();
            if (ds.Tables[0].Rows.Count > 0)
            {
                tipoAcesso = MontaTipoAcesso(ds.Tables[0].Rows[0]);
            }
            return tipoAcesso;
        }




    }
}
