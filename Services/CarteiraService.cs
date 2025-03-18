using Intranet_NEW.DAL;
using Intranet_NEW.Models.WEB;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Intranet_NEW.Services
{
    public class CarteiraService
    {
        private readonly DAL_MIS _daoMis;
        public CarteiraService() {

            _daoMis = new DAL_MIS();
        }

        public CarteiraModel MontaCarteira(DataRow row)
        {

            CarteiraModel carteira = new CarteiraModel();
            carteira.Id = row["NR_ATIVIDADE"].ToString();
            carteira.Name = row["NM_CARTEIRA"].ToString();
            return carteira;

        }

        public List<CarteiraModel> ListaCarteiras()
        {
            List<CarteiraModel> carteiras = new List<CarteiraModel>();
            SqlCommand command = new SqlCommand("SELECT DISTINCT MAX(NR_ATIVIDADE) AS NR_ATIVIDADE,NM_CARTEIRA FROM TBL_WEB_RH_COMBO_ATIVIDADE_ATIVA GROUP BY NM_CARTEIRA");
            DataSet ds =  _daoMis.ConsultaSQL(command);

            foreach (DataRow row in ds.Tables[0].Rows)
            {
                CarteiraModel carteira = MontaCarteira(row);
                carteiras.Add(carteira);
            }

            return carteiras;
        }

    }
}
