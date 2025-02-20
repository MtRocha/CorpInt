using Intranet_NEW.Controllers.DAL;
using Intranet_NEW.Models.WEB;
using System;
using System.Data;
using System.Data.SqlClient;

namespace Intranet.BLL.WEB
{
    public class HelpDeskPeriferico
    {
        public DataSet ListaControleAtivo()
        {
            try
            {
                SqlCommand sqlcommand = new SqlCommand();
                sqlcommand.CommandType = CommandType.Text;
                sqlcommand.CommandText = "SELECT \n"
                                        + "	  C.NR_DESCRICAO \n"
                                        + "	, C.DS_DESCRICAO \n"
                                        + "	, SUM(CASE A.TP_NOTA \n"
                                        + "			WHEN '0' THEN B.QT_PROD \n"
                                        + "			WHEN '1' THEN B.QT_PROD \n"
                                        + "			WHEN '2' THEN B.QT_PROD * -1 \n"
                                        + "			WHEN '3' THEN B.QT_PROD * -1 \n"
                                        + "	  END) AS [QT_PROD] \n"
                                        + "FROM TBL_WEB_HELPDESK_NOTA A \n"
                                        + "     INNER JOIN TBL_WEB_HELPDESK_NOTA_ITEM		B ON A.NR_NOTA = B.NR_NOTA \n"
                                        + "     INNER JOIN TBL_WEB_HELPDESK_TIPO_DESCRICAO C ON C.NR_DESCRICAO = B.NR_EQUIP \n"
                                        + "GROUP BY \n"
                                        + "	  C.NR_DESCRICAO \n"
                                        + "	, C.DS_DESCRICAO \n";


                DAL_MIS AcessaDadosMis = new DAL.DAL_MIS();
                return AcessaDadosMis.ConsultaSQL(sqlcommand);
            }
            catch (Exception ex)
            {
                throw new Exception("BLL.WEB.TI_001: " + ex.Message, ex);
            }
        }

        #region Controle de Periferico

        public int GravaPeriferico(ControlePeriferico dto, string Nota)
        {
            SqlCommand sqlcommand = new SqlCommand();
            sqlcommand.CommandType = CommandType.Text;

            sqlcommand.Parameters.AddWithValue("@NR_NOTA", int.Parse(dto.NR_NOTA));
            sqlcommand.Parameters.AddWithValue("@DT_NOTA", dto.DT_NOTA);
            sqlcommand.Parameters.AddWithValue("@TP_NOTA", int.Parse(dto.TP_NOTA));
            sqlcommand.Parameters.AddWithValue("@NR_RESPONSAVEL", int.Parse(dto.NR_RESPONSAVEL));

            string InsertItem = "INSERT INTO TBL_WEB_HELPDESK_NOTA_ITEM(NR_NOTA, NR_EQUIP, QT_PROD) VALUES (@NR_NOTA, {0}, {1}) \n";
            string InsertItems = "DELETE FROM TBL_WEB_HELPDESK_NOTA_ITEM WHERE NR_NOTA = @NR_NOTA\n";
            foreach (DataRow dr in dto.DT_ITEMS.Rows)
                InsertItems += string.Format(InsertItem, dr["NR_EQUIP"].ToString(), dr["QT_PROD"].ToString());

            sqlcommand.CommandText = InsertItems + "\n\n";

            return Novo(sqlcommand);
        }
        private int Novo(SqlCommand sqlcommand)
        {
            sqlcommand.CommandText += "INSERT INTO TBL_WEB_HELPDESK_NOTA \n"
                                    + "        (NR_NOTA, DT_NOTA, TP_NOTA, NR_RESPONSAVEL) \n"
                                    + "VALUES  (@NR_NOTA, @DT_NOTA, @TP_NOTA, @NR_RESPONSAVEL) \n";

            DAL_MIS AcessaDadosMis = new DAL.DAL_MIS();
            return AcessaDadosMis.ExecutaComandoSQL(sqlcommand);
        }

        #endregion

        #region Controle de Notas Periferico

        public DataSet ListaControleNotaPeriferico(string NR_EQUIP)
        {
            try
            {
                SqlCommand sqlcommand = new SqlCommand();
                sqlcommand.CommandType = CommandType.Text;
                sqlcommand.Parameters.AddWithValue("@NR_EQUIP", NR_EQUIP);

                sqlcommand.CommandText = "SELECT \n"
                                        + "      A.NR_NOTA \n"
                                        + "	, CAST(UPPER(DT_NOTA) AS DATETIME) AS[DT_NOTA] \n"
                                        + "	, A.TP_NOTA \n"
                                        + "	, CASE A.TP_NOTA \n"
                                        + "			WHEN '0' THEN B.QT_PROD \n"
                                        + "			WHEN '1' THEN B.QT_PROD \n"
                                        + "			WHEN '2' THEN B.QT_PROD * -1 \n"
                                        + "			WHEN '3' THEN B.QT_PROD * -1 \n"
                                        + "	 END AS [QT_PROD] \n"
                                        + "   , C.NM_COLABORADOR \n"
                                        + "FROM TBL_WEB_HELPDESK_NOTA A \n"
                                        + "    INNER JOIN TBL_WEB_HELPDESK_NOTA_ITEM B ON A.NR_NOTA        = B.NR_NOTA \n"
                                        + "    INNER JOIN TBL_WEB_COLABORADOR_DADOS  C ON C.NR_COLABORADOR = A.NR_RESPONSAVEL \n"
                                        + "WHERE B.NR_EQUIP = @NR_EQUIP \n"
                                        + "ORDER BY \n"
                                        + "    DT_NOTA \n";

                DAL_MIS AcessaDadosMis = new DAL.DAL_MIS();
                return AcessaDadosMis.ConsultaSQL(sqlcommand);
            }
            catch (Exception ex)
            {
                throw new Exception("BLL.WEB.TI_001: " + ex.Message, ex);
            }
        }

        #endregion
    }
}