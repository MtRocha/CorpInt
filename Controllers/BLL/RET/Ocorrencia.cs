using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Google.Apis.Storage.v1;
using Intranet_NEW.Controllers.DAL;

namespace Intranet.BLL.RET
{
    public class Ocorrencia
    {
        DAL_MIS AcessaDadosMis = new Intranet.DAL.DAL_MIS();
        DAL_PROC_ROVERI AcessaDadosProc = new Intranet.DAL.DAL_PROC_ROVERI();

        public List<Intranet_NEW.Models.HoraHoraOcorrencia> ListaOcorrencia(string tp_ocorrencia, string dtIni , string dtFim)
        {
            SqlCommand sqlcommand = new SqlCommand();
            List<Intranet_NEW.Models.HoraHoraOcorrencia> ocorrencias = new List<Intranet_NEW.Models.HoraHoraOcorrencia>();
            sqlcommand.CommandType = CommandType.StoredProcedure;
            sqlcommand.CommandText = "SP_LISTA_OCORRENCIA";
            if (string.IsNullOrEmpty(tp_ocorrencia) || tp_ocorrencia == "Todos")
            {
                tp_ocorrencia = "ALL";
            }

            sqlcommand.Parameters.Add(new SqlParameter("@TP_OCORRENCIA", tp_ocorrencia));
            sqlcommand.Parameters.Add(new SqlParameter("@DT_INI", dtIni));
            sqlcommand.Parameters.Add(new SqlParameter("@DT_FIM", dtFim));

            try
            {

                DataSet ds = AcessaDadosProc.ConsultaSQL(sqlcommand);

                if (ds.Tables[0].Rows.Count > 0)
                {

                    foreach (DataRow item in ds.Tables[0].Rows)
                    {
                        Intranet_NEW.Models.HoraHoraOcorrencia model = new Intranet.DTO.HoraHoraOcorrencia();

                        model.ID_OCORRENCIA                    = (int)      item["ID_OCORRENCIA"];
                        model.TP_OCORRENCIA                    = (string)   item["TP_OCORRENCIA"];
                        model.NM_ORGAO_QOA                     = (string)   item["NM_ORGAO_QOA"];
                        model.TP_SISTEMA_DAO                   = (string)   item["TP_SISTEMA_DAO"];
                        model.DT_OCORRENCIA                    = (DateTime) item["DT_OCORRENCIA"];
                        model.DT_INCLUSAO                      = (DateTime) item["DT_INCLUSAO"];
                        model.NR_USER_INCLUSAO                 = (string)   item["NR_USER_INCLUSAO"];
                        model.NM_USER_INCLUSAO                 = (string)   item["NM_USER_INCLUSAO"];
                        model.NR_QUANTIDADE_TIA                = Convert.ToDouble(item["NR_QUANTIDADE_TIA"]);
                        model.MOTIVO_EAR                       = (string)   item["MOTIVO_EAR"];
                        model.TP_ARQUIVADO                     = (string)   item["TP_ARQUIVADO"];
                        model.NR_QUANTIDADE_TIA_MONITORADAS    = item["NR_QUANTIDADE_TIA_MONITORADAS"] == DBNull.Value ? 0 : Convert.ToDouble(item["NR_QUANTIDADE_TIA_MONITORADAS"]);
                        model.TP_MOTIVO_EGS                    = item["TP_MOTIVO_EGS"]                 == DBNull.Value ? "": (string)item["TP_MOTIVO_EGS"];
                        model.DT_ARQUIVAMENTO                  = item["DT_ARQUIVAMENTO"]               == DBNull.Value ? DateTime.MinValue : (DateTime) item["DT_ARQUIVAMENTO"];
                        model.NM_USER_ARQUIVAMENTO             = item["NM_USER_ARQUIVAMENTO"]          == DBNull.Value ? ""   : (string) item["NM_USER_ARQUIVAMENTO"];
                        model.NR_DURACAO_DAO                   = item["NR_DURACAO_DAO"]                == DBNull.Value ? ""   : (string) item["NR_DURACAO_DAO"];
                        model.NR_DURACAO_SIST                  = item["NR_DURACAO_SIST"]               == DBNull.Value ? ""   : (string) item["NR_DURACAO_SIST"];
                        model.NM_SISTEMA_CX                    = item["NM_SISTEMA_CX"]                 == DBNull.Value ? ""   : (string) item["NM_SISTEMA_CX"];
                        PegarDetalhe(model);           
                        ocorrencias.Add(model);
                    }
                    return ocorrencias;
                }
                else
                {
                    return ocorrencias;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("" + ex.Message, ex);
            }
        }


        public void PegarDetalhe(Intranet_NEW.Models.HoraHoraOcorrencia obj)
        {
            switch (obj.TP_OCORRENCIA)
            {
                case "EAR":
                    obj.TP_DETALHE = $"Motivo do EAR : {obj.MOTIVO_EAR}";
                    break;
                case "EGS":
                    obj.TP_DETALHE  = $"Motivo do EGS :{obj.TP_MOTIVO_EGS}";
                    break;
                case "QOA":
                    obj.TP_DETALHE = $"Orgão do QOA  :{obj.NM_ORGAO_QOA}";
                    break;
                case "DAO":
                    obj.TP_DETALHE = $"Sistema do DAO  :{obj.TP_SISTEMA_DAO} Duração da Indisponibilidade : {obj.NR_DURACAO_DAO}";
                    break;
                case "TIA":
                    decimal percentual = (Decimal) (obj.NR_QUANTIDADE_TIA / obj.NR_QUANTIDADE_TIA_MONITORADAS) * 100;
                    obj.TP_DETALHE = $"Quantidade de Inconformes: {obj.NR_QUANTIDADE_TIA}" +
                                     $"       Quantidade de Monitorias Realizadas: {obj.NR_QUANTIDADE_TIA_MONITORADAS}   ";
                    break;
                case "SIST":
                    obj.TP_DETALHE = $"Sistema CAIXA  :{obj.NM_SISTEMA_CX}";
                    break;
            }
        }

        public void ArquivaOcorrencia(Intranet_NEW.Models.HoraHoraOcorrencia obj)
        {
            SqlCommand sqlcommand = new SqlCommand();
            sqlcommand.CommandType = CommandType.StoredProcedure;
            sqlcommand.CommandText = "SP_ARQUIVA_OCORRENCIA";
            sqlcommand.Parameters.AddWithValue("@ID_OCORRENCIA", obj.ID_OCORRENCIA);
            sqlcommand.Parameters.AddWithValue("@DT_ARQUIVAMENTO", obj.DT_ARQUIVAMENTO);
            sqlcommand.Parameters.AddWithValue("@TP_ARQUIVADO", obj.TP_ARQUIVADO);
            sqlcommand.Parameters.AddWithValue("@NM_USER_ARQUIVAMENTO", obj.NM_USER_ARQUIVAMENTO);
        }


        public void GravaOcorrencia(Intranet_NEW.Models.HoraHoraOcorrencia obj)
        {
            SqlCommand sqlcommand = new SqlCommand();
            sqlcommand.CommandType = CommandType.StoredProcedure;

            sqlcommand.CommandText = "SP_INCLUSAO_OCORRENCIA";

            try
            {
                sqlcommand.Parameters.AddWithValue("@TP_OCORRENCIA ", obj.TP_OCORRENCIA);
                sqlcommand.Parameters.AddWithValue("@NR_USER_INCLUSAO", obj.NR_USER_INCLUSAO);
                sqlcommand.Parameters.AddWithValue("@DT_INCLUSAO", obj.DT_INCLUSAO);
                sqlcommand.Parameters.AddWithValue("@DT_OCORRENCIA", obj.DT_OCORRENCIA);

                if (obj.TP_OCORRENCIA == "TIA")
                {
                    sqlcommand.Parameters.AddWithValue("@NR_QUANTIDADE_TIA_MONITORADAS", obj.NR_QUANTIDADE_TIA_MONITORADAS);
                    sqlcommand.Parameters.AddWithValue("@NR_QUANTIDADE_TIA", obj.NR_QUANTIDADE_TIA);
                }
                else
                {
                    sqlcommand.Parameters.AddWithValue("@NR_QUANTIDADE_TIA_MONITORADAS", obj.NR_QUANTIDADE_TIA_MONITORADAS);
                    sqlcommand.Parameters.AddWithValue("@NR_QUANTIDADE_TIA", obj.NR_QUANTIDADE_TIA);
                }
                if(obj.TP_OCORRENCIA == "QOA")
                    sqlcommand.Parameters.AddWithValue("@NM_ORGAO_QOA", obj.NM_ORGAO_QOA);
                else
                    sqlcommand.Parameters.AddWithValue("@NM_ORGAO_QOA", "");

                if (obj.TP_OCORRENCIA == "DAO")
                    sqlcommand.Parameters.AddWithValue("@TP_PLATAFORMA_EGS", obj.TP_MOTIVO_EGS);
                else
                    sqlcommand.Parameters.AddWithValue("@TP_PLATAFORMA_EGS", "");

                if (obj.TP_OCORRENCIA == "EAR")
                    sqlcommand.Parameters.AddWithValue("@MOTIVO_EAR", obj.MOTIVO_EAR);
                else
                    sqlcommand.Parameters.AddWithValue("@MOTIVO_EAR", "");

                if (obj.TP_OCORRENCIA == "DAO")
                {
                    sqlcommand.Parameters.AddWithValue("@TP_SISTEMA_DAO", obj.TP_SISTEMA_DAO);
                    sqlcommand.Parameters.AddWithValue("@NR_DURACAO_DAO",obj.NR_DURACAO_DAO);
                }
                else
                {
                    sqlcommand.Parameters.AddWithValue("@NR_DURACAO_DAO", DBNull.Value);
                    sqlcommand.Parameters.AddWithValue("@TP_SISTEMA_DAO", "");
                }

                if (obj.TP_OCORRENCIA == "SIST")
                {
                    sqlcommand.Parameters.AddWithValue("@NM_SISTEMA_CX", obj.NM_SISTEMA_CX);
                    sqlcommand.Parameters.AddWithValue("@NR_DURACAO_SIST", obj.NR_DURACAO_SIST);
                }
                else
                {
                    sqlcommand.Parameters.AddWithValue("@NM_SISTEMA_CX", DBNull.Value);
                    sqlcommand.Parameters.AddWithValue("@NR_DURACAO_SIST", "");
                }

                AcessaDadosProc.ExecutaComandoSQL(sqlcommand);
            }
            catch (Exception ex)
            {
                throw new Exception("" + ex.Message, ex);
            }
        }
    }
}
