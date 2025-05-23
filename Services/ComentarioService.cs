﻿using Intranet_NEW.DAL;
using Intranet_NEW.Models.WEB;
using Intranet_NEW.Services.Handlers;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Intranet_NEW.Services
{
    public class ComentarioService
    {
        private readonly DAL_INTRANET _dalIntranet;
        public ComentarioService()
        {
            _dalIntranet = new DAL_INTRANET();
        }

        public void InsertComentario(ComentarioModel model)
        {
            SqlCommand cmd = new();
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "SP_INSERE_COMENTARIO";
            cmd.Parameters.Add(new SqlParameter("@NR_USUARIO", model.IdUsuario));
            cmd.Parameters.Add(new SqlParameter("@DT_COMENTARIO", model.dtComentario));
            cmd.Parameters.Add(new SqlParameter("@DS_COMENTARIO", model.Conteudo));
            cmd.Parameters.Add(new SqlParameter("@ID_PUB", model.IdPub));


            _dalIntranet.ExecutaComandoSQL(cmd);

        }

        public List<ComentarioModel> ListaComentarios(int idPublicacao, int pagina, int quantidade, DateTime data)
        {
            SqlCommand cmd = new();
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "SP_WEB_LISTA_COMENTARIO";
            cmd.Parameters.Add(new SqlParameter("@ID_PUBLICACAO", idPublicacao));
            cmd.Parameters.Add(new SqlParameter("@PAGINA", pagina));
            cmd.Parameters.Add(new SqlParameter("@QUANTIDADE", quantidade));
            cmd.Parameters.Add(new SqlParameter("@DATA", data == DateTime.MinValue ? DBNull.Value : data));

            DataSet ds = _dalIntranet.ConsultaSQL(cmd);
            List<ComentarioModel> comentarios = new();

            foreach (DataRow row in ds.Tables[0].Rows)
            {
                comentarios.Add(MontaComentario(row));
            }

            return comentarios;
        }

        public ComentarioModel MontaComentario(DataRow row)
        {
            ComentarioModel comentario = new ComentarioModel();
            comentario.Id = Convert.ToInt32(row["ID_COMENTARIO"]);
            comentario.IdPub = Convert.ToInt32(row["ID_PUB"]);
            comentario.IdUsuario = Convert.ToInt32(row["NR_USUARIO"]);
            comentario.UsuarioNome = row["NM_COLABORADOR"].ToString();
            comentario.Conteudo = row["COMENTARIO_CONTEUDO"].ToString();
            comentario.dtComentario = Convert.ToDateTime(row["DT_COMENTARIO"]);
            return comentario;
        }

        public int ContarComentarios(int idPublicacao)
        {
            SqlCommand cmd = new();
            cmd.CommandText = "SELECT COUNT(*) AS QT_COMENTARIO FROM TBL_WEB_PUBLICACAO_COMENTARIO WHERE TP_EXCLUIDO IS NULL AND ID_PUB = @ID_PUB";
            cmd.Parameters.Add(new SqlParameter("@ID_PUB", idPublicacao));
            DataSet ds = _dalIntranet.ConsultaSQL(cmd);
            return Convert.ToInt32(ds.Tables[0].Rows[0]["QT_COMENTARIO"]);

        }
    }
}
