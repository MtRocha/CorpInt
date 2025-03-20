using Intranet_NEW.DAL;
using Intranet_NEW.Models.WEB;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Intranet_NEW.Services
{
    public class ReacaoService
    {
        private readonly DAL_MIS _daoMis;
        private readonly PublicacaoService _publicacaoService;
        public ReacaoService() {
        
            _daoMis = new DAL_MIS();
            _publicacaoService = new PublicacaoService();
        }

        public void Reagir(ReacaoModel reacao,PublicacaoModel pub)
        {
            int tipoReacao = reacao.IdReacao == 2 ? reacao.IdReacao : 1;
            SqlCommand cmdReacao = new SqlCommand("INSERT INTO TBL_WEB_PUBLICACAO_REACAO (ID_REACAO,NR_USUARIO,DT_REACAO,ID_PUBLICACAO) VALUES (@ID_REACAO,@NR_COLABORADOR,@DT_REACAO,@ID_PUBLICACAO)");
            cmdReacao.Parameters.Add(new SqlParameter("@ID_REACAO", reacao.IdReacao));
            cmdReacao.Parameters.Add(new SqlParameter("@ID_PUBLICACAO", pub.Id));
            cmdReacao.Parameters.Add(new SqlParameter("@NR_COLABORADOR", reacao.Usuario));
            cmdReacao.Parameters.Add(new SqlParameter("@DT_REACAO", reacao.dataReacao));
            _daoMis.ExecutaComandoSQL(cmdReacao);

            if (tipoReacao == 1)
                _publicacaoService.AtualizaCurtida(pub.Id, pub.Curtidas);
            else
                _publicacaoService.AtualizaDescurtida(pub.Id, pub.Descurtidas);

        }

        public ConfigReacaoModel MontaReacao(DataRow row)
        {
            return new ConfigReacaoModel
            {
                Id = row.Field<int>("ID"),
                Nome = row.Field<string>("Nome")
            };

        }

        public List<ConfigReacaoModel> ListaReacoes()
        {
            SqlCommand command = new("SELECT * FROM ConfigReacao WHERE TP_ATIVO = 1");
            List<ConfigReacaoModel> listaReacoes = new List<ConfigReacaoModel>();
            DataSet ds = _daoMis.ConsultaSQL(command);
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                listaReacoes.Add(MontaReacao(row));
            }
            return listaReacoes;
        }

    }
}
