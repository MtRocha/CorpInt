using Intranet_NEW.Controllers.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Intranet.BLL.WEB
{
    public class ColaboradorAddRelatorio
    {
        #region Relatorio Ações Aplicadas

        public DataSet PorColaborador(string DT_INI, string DT_FIM, string NR_RESPONSAVEL, string NR_SUPERVISOR, string NM_COLABORADOR)
        {
            try
            {
                NM_COLABORADOR = NM_COLABORADOR == "" ? "" : string.Format("%{0}%", NM_COLABORADOR);
                SqlCommand sqlcommand = new SqlCommand();
                sqlcommand.CommandType = CommandType.StoredProcedure;
                sqlcommand.CommandText = "SP_WEB_RH_ACOES_APLICADAS_COLABORADOR";

                sqlcommand.Parameters.AddWithValue("@DT_INI", DT_INI);
                sqlcommand.Parameters.AddWithValue("@DT_FIM", DT_FIM);
                sqlcommand.Parameters.AddWithValue("@NR_RESPONSAVEL", NR_RESPONSAVEL);
                sqlcommand.Parameters.AddWithValue("@NR_SUPERVISOR", NR_SUPERVISOR);
                sqlcommand.Parameters.AddWithValue("@NM_COLABORADOR", NM_COLABORADOR);

                DAL_MIS AcessaDadosMis = new DAL.DAL_MIS();
                DataSet ds = AcessaDadosMis.ConsultaSQL(sqlcommand);

                ds.Tables.Add(ListaResponsavel(ds.Tables[1], "RESPONSAVEL"));
                ds.Tables.Add(ListaResponsavel(ds.Tables[1], "SUPERVISOR"));
                ds.Tables.Remove(ds.Tables[1]);

                return ds;
            }
            catch (Exception ex)
            {
                throw new Exception("BLL.WEB.ColaboradorAddRelatorio_001: " + ex.Message, ex);
            }
        }

        public DataSet PorMotivo(string DT_INI, string DT_FIM, string NR_RESPONSAVEL, string NR_SUPERVISOR, string NR_MOTIVO)
        {
            try
            {
                SqlCommand sqlcommand = new SqlCommand();
                sqlcommand.CommandType = CommandType.StoredProcedure;
                sqlcommand.CommandText = "SP_WEB_RH_ACOES_APLICADAS_MOTIVO";

                sqlcommand.Parameters.AddWithValue("@DT_INI", DT_INI);
                sqlcommand.Parameters.AddWithValue("@DT_FIM", DT_FIM);
                sqlcommand.Parameters.AddWithValue("@NR_RESPONSAVEL", NR_RESPONSAVEL);
                sqlcommand.Parameters.AddWithValue("@NR_SUPERVISOR", NR_SUPERVISOR);
                sqlcommand.Parameters.AddWithValue("@NR_MOTIVO", NR_MOTIVO);

                DAL_MIS AcessaDadosMis = new DAL.DAL_MIS();
                DataSet ds = AcessaDadosMis.ConsultaSQL(sqlcommand);

                ds.Tables.Add(ListaResponsavel(ds.Tables[2], "RESPONSAVEL"));
                ds.Tables.Add(ListaResponsavel(ds.Tables[2], "SUPERVISOR"));
                ds.Tables.Remove(ds.Tables[2]);

                return ds;
            }
            catch (Exception ex)
            {
                throw new Exception("BLL.WEB.ColaboradorAddRelatorio_001: " + ex.Message, ex);
            }
        }

        internal DataTable ListaResponsavel(DataTable dtOrigem, string t)
        {
            DataTable dt = new DataTable("TBL_" + t);
            dt.Columns.Add("NR_COLABORADOR", typeof(Int32));
            dt.Columns.Add("NM_COLABORADOR", typeof(string));

            var result = dtOrigem.AsEnumerable().Where(f => f.Field<Int32>("NR_" + t) != -1).Select(s => new { NR_COLABORADOR = s.Field<Int32>("NR_" + t), NM_COLABORADOR = s.Field<string>("NM_" + t) }).Distinct().ToArray();

            foreach (var r in result)
                dt.Rows.Add(r.NR_COLABORADOR, r.NM_COLABORADOR);

            return dt;
        }

        #endregion
    }
}