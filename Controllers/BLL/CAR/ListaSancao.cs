using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Intranet.DTO.WEB;
using Intranet_NEW.Controllers.DAL;

namespace Intranet.BLL.CAR
{
    public class ListaSancao
    {

        public string NM_COLABORADOR { get; set; }
        public string DT_APLICACAO { get; set; }


        public DataSet PesquisarListSan(Int64 dt_inic, Int64 dt_fim)
        {
            try
            {
                SqlCommand sqlcommand = new SqlCommand();
                sqlcommand.CommandType = CommandType.Text;

                sqlcommand.Parameters.AddWithValue("@DT_INIC", dt_inic);
                sqlcommand.Parameters.AddWithValue("@DT_FIM", dt_fim);
                sqlcommand.CommandText = "Select C.NM_COLABORADOR AS NM_Colaborador, A.DT_APLICACAO AS DT_APLICACAO, B.DS_MOTIVO AS DS_Motivo,\n"
           + "CASE A.TP_SANCAO\n"
           + "       WHEN '1' THEN 'EXITO'\n"
           + "      WHEN '2' THEN 'CAIXA'\n"
           + "END AS Emissor, \n"
           + "CASE A.NR_ADVERTENCIA\n"
           + "                WHEN '1' THEN '1º Advertência'\n"
           + "                    when '2' THEN '2º Advertência'\n"
           + "                    when '3' THEN '1º Suspensão'\n"
           + "                    when '4' THEN '2º Suspensão'\n"
           + "                    when '5' THEN '3º Suspensão'\n"
           + "                    when '6' THEN 'Feedback'\n"
           + "                           ELSE 'NULL'\n"
           + "                           END AS NR_Advertido \n"
           + " from DB_MIS..TBL_WEB_RH_ACAO_DISCIP A\n"
           + "INNER JOIN DB_MIS..TBL_WEB_RH_ACAO_DISCIP_MOT B ON A.NR_MOTIVO = B.NR_MOTIVO\n"
           + "INNER JOIN DB_MIS..TBL_WEB_COLABORADOR_DADOS C ON A.NR_COLABORADOR = C.NR_COLABORADOR\n"
           + "WHERE   \n"
           + "A.TP_EFETIVADO = 1\n"
           //+" AND C.NM_COLABORADOR LIKE '%@NM_COLABORADOR%'\n"
           + "AND CONVERT(CHAR(8),A.DT_APLICACAO,112) BETWEEN @DT_INIC AND @DT_FIM";
                

                DAL_MIS AcessaDadosMis = new DAL.DAL_MIS();
                return AcessaDadosMis.ConsultaSQL(sqlcommand);
            }
            catch (Exception ex)
            {
                throw new Exception("REM.BLACK_LIST001: " + ex.Message, ex);
            }
        }

        
    }
}



