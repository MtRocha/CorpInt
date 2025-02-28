using System;
using System.Data.SqlClient;

public class Conexao
{
    

}

public class ModeloGrid
{
    public Int32 NR_REGISTRO  { get; set; }
    public Int32 NR_COLABORADOR { get; set; }
    public String NM_ESCOLARIDADE { get; set; }
    public string NM_DESCRICAO { get; set; }
    public string NM_INSTITUICAO { get; set; }
    public string NM_CURSO { get; set; }
    public DateTime DT_INICIO { get; set; }
    public DateTime DT_CONCLUSAO { get; set; }
}