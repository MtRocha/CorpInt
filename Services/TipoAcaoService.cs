
using Intranet_NEW.DAL;
using Intranet_NEW.Models.WEB;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Intranet_NEW.Services
{
    public class TipoAcaoService
    {
        private readonly DAL_MIS _daoMis;
        public TipoAcaoService()
        {

            _daoMis = new DAL_MIS();
        }
        public TipoAcaoModel MontaTipoAcao(DataRow row)
        {
            TipoAcaoModel tipoAcao = new TipoAcaoModel();
            tipoAcao.Id = row["ID"].ToString();
            tipoAcao.Name = row["NM_TIPO_ACAO"].ToString();
            return tipoAcao;
        }
        public List<TipoAcaoModel> ListaTipoAcao()
        {
            List<TipoAcaoModel> tipoAcoes = new List<TipoAcaoModel>();
            SqlCommand command = new SqlCommand("SELECT * FROM TBL_WEB_PUBLICACAO_TIPO_ACAO");
            DataSet ds = _daoMis.ConsultaSQL(command);
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                TipoAcaoModel tipoAcao = MontaTipoAcao(row);
                tipoAcoes.Add(tipoAcao);
            }
            return tipoAcoes;
        }
    }
}
