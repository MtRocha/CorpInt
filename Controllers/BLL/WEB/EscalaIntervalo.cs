using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;
using Intranet_NEW.Controllers.DAL;

namespace Intranet.BLL.WEB
{
    public class EscalaIntervalo
    {
        public DataSet CarregaEquipe()
        {
            try
            {
                SqlCommand sqlcommand = new SqlCommand();
                sqlcommand.CommandType = CommandType.StoredProcedure;

                sqlcommand.CommandText = "SP_WEB_ESCALA_INTERVALO";

                DAL_MIS AcessaDadosMis = new DAL.DAL_MIS();
                DataSet ds = AcessaDadosMis.ConsultaSQL(sqlcommand);

                DataSet dsFinal = new DataSet();
                dsFinal.Tables.Add(CriaDataTableEscalaManha(ds.Tables[0].AsEnumerable().Where(f => f.Field<string>("TP_INTERVALO") == "1").OrderBy(o => o.Field<string>("DS_HORARIO")).ToList()));
                dsFinal.Tables.Add(CriaDataTableEscalaManha(ds.Tables[0].AsEnumerable().Where(f => f.Field<string>("TP_INTERVALO") == "2").OrderBy(o => o.Field<string>("DS_HORARIO")).ToList()));
                dsFinal.Tables.Add(CriaDataTableEscalaManha(ds.Tables[0].AsEnumerable().Where(f => f.Field<string>("TP_INTERVALO") == "3").OrderBy(o => o.Field<string>("DS_HORARIO")).ToList()));

                dsFinal.Tables.Add(CriaDataTableEscalaTarde(ds.Tables[1].AsEnumerable().Where(f => f.Field<string>("TP_INTERVALO") == "1").OrderBy(o => o.Field<string>("DS_HORARIO")).ToList()));
                dsFinal.Tables.Add(CriaDataTableEscalaTarde(ds.Tables[1].AsEnumerable().Where(f => f.Field<string>("TP_INTERVALO") == "2").OrderBy(o => o.Field<string>("DS_HORARIO")).ToList()));
                dsFinal.Tables.Add(CriaDataTableEscalaTarde(ds.Tables[1].AsEnumerable().Where(f => f.Field<string>("TP_INTERVALO") == "3").OrderBy(o => o.Field<string>("DS_HORARIO")).ToList()));

                return (dsFinal);
            }
            catch (Exception ex)
            {
                throw new Exception("BLL.WEB.EscalaIntervalo_001: " + ex.Message, ex);
            }
        }

        protected DataTable CriaDataTableEscalaManha(List<DataRow> obj)
        {
            try
            {
                var DS_HORARIO = obj.AsEnumerable().Select(s => new { DS_HORARIO = s.Field<string>("DS_HORARIO") }).Distinct().ToList();

                DataTable dtManha = new DataTable();
                dtManha.Columns.Add("COL01", typeof(string));
                dtManha.Columns.Add("COL02", typeof(string));
                dtManha.Columns.Add("COL03", typeof(string));
                dtManha.Columns.Add("COL04", typeof(string));
                dtManha.Columns.Add("COL05", typeof(string));
                dtManha.Columns.Add("COL06", typeof(string));
                dtManha.Columns.Add("COL07", typeof(string));
                dtManha.Columns.Add("COL08", typeof(string));

                if (DS_HORARIO.Count() == 5)
                {
                    foreach (var str in DS_HORARIO)
                    {
                        DataRow NovaLinha = dtManha.NewRow();
                        NovaLinha[0] = str.DS_HORARIO;

                        for (Int16 Coluna = 1; Coluna < 7; Coluna++)
                        {
                            if (Coluna == 6)
                            {
                                NovaLinha[6] = str.DS_HORARIO;

                                var NM_SUPERVISOR = obj.AsEnumerable().Where(f => f.Field<string>("DS_HORARIO") == str.DS_HORARIO && f.Field<Int16>("NR_DIA_SEMANA") == Coluna).ToList();
                                if (NM_SUPERVISOR.Count() > 0)
                                {
                                    string nome01 = NM_SUPERVISOR[0]["NM_SUPERVISOR01"].ToString() != "" ? NM_SUPERVISOR[0]["NM_SUPERVISOR01"].ToString().Substring(0, NM_SUPERVISOR[0]["NM_SUPERVISOR01"].ToString().IndexOf(' ')) : "";
                                    string nome02 = NM_SUPERVISOR[0]["NM_SUPERVISOR02"].ToString() != "" ? NM_SUPERVISOR[0]["NM_SUPERVISOR02"].ToString().Substring(0, NM_SUPERVISOR[0]["NM_SUPERVISOR02"].ToString().IndexOf(' ')) : "";
                                    string nome03 = NM_SUPERVISOR[0]["NM_SUPERVISOR03"].ToString() != "" ? NM_SUPERVISOR[0]["NM_SUPERVISOR03"].ToString().Substring(0, NM_SUPERVISOR[0]["NM_SUPERVISOR03"].ToString().IndexOf(' ')) : "";
                                    string nome04 = NM_SUPERVISOR[0]["NM_SUPERVISOR04"].ToString() != "" ? NM_SUPERVISOR[0]["NM_SUPERVISOR04"].ToString().Substring(0, NM_SUPERVISOR[0]["NM_SUPERVISOR04"].ToString().IndexOf(' ')) : "";
                                    string supervisores = "";
                                    if (!string.IsNullOrEmpty(nome01))
                                    {
                                        supervisores += nome01;
                                        if (!string.IsNullOrEmpty(nome02))
                                        {
                                            supervisores += "/" + nome02;
                                            if (!string.IsNullOrEmpty(nome03))
                                            {
                                                supervisores += "/" + nome03;
                                                if (!string.IsNullOrEmpty(nome04))
                                                {
                                                    supervisores += "/" + nome04;
                                                }
                                            }
                                        }
                                    }
                                    NovaLinha[7] = supervisores;
                                }
                            }
                            else
                            {
                                var NM_SUPERVISOR = obj.AsEnumerable().Where(f => f.Field<string>("DS_HORARIO") == str.DS_HORARIO && f.Field<Int16>("NR_DIA_SEMANA") == Coluna).ToList();
                                if (NM_SUPERVISOR.Count() > 0)
                                {
                                    string nome01 = NM_SUPERVISOR[0]["NM_SUPERVISOR01"].ToString() != "" ? NM_SUPERVISOR[0]["NM_SUPERVISOR01"].ToString().Substring(0, NM_SUPERVISOR[0]["NM_SUPERVISOR01"].ToString().IndexOf(' ')) : "";
                                    string nome02 = NM_SUPERVISOR[0]["NM_SUPERVISOR02"].ToString() != "" ? NM_SUPERVISOR[0]["NM_SUPERVISOR02"].ToString().Substring(0, NM_SUPERVISOR[0]["NM_SUPERVISOR02"].ToString().IndexOf(' ')) : "";
                                    string nome03 = NM_SUPERVISOR[0]["NM_SUPERVISOR03"].ToString() != "" ? NM_SUPERVISOR[0]["NM_SUPERVISOR03"].ToString().Substring(0, NM_SUPERVISOR[0]["NM_SUPERVISOR03"].ToString().IndexOf(' ')) : "";
                                    string nome04 = NM_SUPERVISOR[0]["NM_SUPERVISOR04"].ToString() != "" ? NM_SUPERVISOR[0]["NM_SUPERVISOR04"].ToString().Substring(0, NM_SUPERVISOR[0]["NM_SUPERVISOR04"].ToString().IndexOf(' ')) : "";

                                    string supervisores = "";
                                    if (!string.IsNullOrEmpty(nome01))
                                    {
                                        supervisores += nome01;
                                        if (!string.IsNullOrEmpty(nome02))
                                        {
                                            supervisores += "/" + nome02;
                                            if (!string.IsNullOrEmpty(nome03))
                                            {
                                                supervisores += "/" + nome03;
                                                if (!string.IsNullOrEmpty(nome04))
                                                {
                                                    supervisores += "/" + nome04;
                                                }
                                            }
                                        }
                                    }
                                    NovaLinha[Coluna] = supervisores;
                                }
                            }
                        }
                        dtManha.Rows.Add(NovaLinha);
                    }
                }
                return dtManha;

            }
            catch (Exception ex)
            {
                throw new Exception("BLL.WEB.EscalaIntervalo_002: " + ex.Message, ex);
            }
        }

        protected DataTable CriaDataTableEscalaTarde(List<DataRow> obj)
        {
            try
            {
                var DS_HORARIO = obj.AsEnumerable().Select(s => new { DS_HORARIO = s.Field<string>("DS_HORARIO") }).Distinct().ToList();

                DataTable dtTarde = new DataTable();
                dtTarde.Columns.Add("COL01", typeof(string));
                dtTarde.Columns.Add("COL02", typeof(string));
                dtTarde.Columns.Add("COL03", typeof(string));
                dtTarde.Columns.Add("COL04", typeof(string));
                dtTarde.Columns.Add("COL05", typeof(string));
                dtTarde.Columns.Add("COL06", typeof(string));

                if (DS_HORARIO.Count() == 5 || DS_HORARIO.Count() == 4)
                {
                    foreach (var str in DS_HORARIO)
                    {
                        DataRow NovaLinha = dtTarde.NewRow();
                        NovaLinha[0] = str.DS_HORARIO;

                        for (Int16 Coluna = 1; Coluna < 7; Coluna++)
                        {
                            var NM_SUPERVISOR = obj.AsEnumerable().Where(f => f.Field<string>("DS_HORARIO") == str.DS_HORARIO && f.Field<Int16>("NR_DIA_SEMANA") == Coluna).ToList();
                            if (NM_SUPERVISOR.Count() > 0)
                            {
                                string nome01 = NM_SUPERVISOR[0]["NM_SUPERVISOR01"].ToString() != "" ? NM_SUPERVISOR[0]["NM_SUPERVISOR01"].ToString().Substring(0, NM_SUPERVISOR[0]["NM_SUPERVISOR01"].ToString().IndexOf(' ')) : "";
                                string nome02 = NM_SUPERVISOR[0]["NM_SUPERVISOR02"].ToString() != "" ? NM_SUPERVISOR[0]["NM_SUPERVISOR02"].ToString().Substring(0, NM_SUPERVISOR[0]["NM_SUPERVISOR02"].ToString().IndexOf(' ')) : "";
                                string nome03 = NM_SUPERVISOR[0]["NM_SUPERVISOR03"].ToString() != "" ? NM_SUPERVISOR[0]["NM_SUPERVISOR03"].ToString().Substring(0, NM_SUPERVISOR[0]["NM_SUPERVISOR03"].ToString().IndexOf(' ')) : "";
                                string nome04 = NM_SUPERVISOR[0]["NM_SUPERVISOR04"].ToString() != "" ? NM_SUPERVISOR[0]["NM_SUPERVISOR04"].ToString().Substring(0, NM_SUPERVISOR[0]["NM_SUPERVISOR04"].ToString().IndexOf(' ')) : "";

                                string supervisores = "";
                                if (!string.IsNullOrEmpty(nome01))
                                {
                                    supervisores += nome01;
                                    if (!string.IsNullOrEmpty(nome02))
                                    {
                                        supervisores += "/" + nome02;
                                        if (!string.IsNullOrEmpty(nome03))
                                        {
                                            supervisores += "/" + nome03;
                                            if (!string.IsNullOrEmpty(nome04))
                                            {
                                                supervisores += "/" + nome04;
                                            }
                                        }
                                    }
                                }
                                NovaLinha[Coluna] = supervisores;
                            }
                        
                        }
                        dtTarde.Rows.Add(NovaLinha);
                    }
                }
                return dtTarde;
            }
            catch (Exception ex)
            {
                throw new Exception("BLL.WEB.EscalaIntervalo_003: " + ex.Message, ex);
            }
        }
    }
}