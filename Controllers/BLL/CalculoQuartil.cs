using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

namespace Intranet.BLL
{
    public class CalculoQuartil
    {
        public DataTable Quartil(DataTable dt, string ColunaValor, string ColunaQuartil, string Ordem)
        {
            if (dt.Rows.Count > 3)
            {
                DataTable dtValor = dt.DefaultView.ToTable(false, ColunaValor);
                DataView dv = dtValor.DefaultView;
                dv.Sort = string.Format("{0} ASC", ColunaValor);
                dtValor = dv.ToTable();

                // verifica se na coluna existe numero invalido
                try { dtValor.AsEnumerable().Sum(s => s.Field<Int32>(dtValor.Columns[0].ColumnName)); }
                catch
                { return dt; }

                int[] I1 = RetornaIndice(0.25, dtValor.Rows.Count);
                int[] I2 = RetornaIndice(0.50, dtValor.Rows.Count);
                int[] I3 = RetornaIndice(0.75, dtValor.Rows.Count);

                double Q1 = RetornaMedia((Int32)dtValor.Rows[I1[0]][0], (Int32)dtValor.Rows[I1[1]][0]); // 4
                double Q2 = RetornaMedia((Int32)dtValor.Rows[I2[0]][0], (Int32)dtValor.Rows[I2[1]][0]); // 3
                double Q3 = RetornaMedia((Int32)dtValor.Rows[I3[0]][0], (Int32)dtValor.Rows[I3[1]][0]); // 2

                if (Ordem == "ASC")
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        double Valor = (Int32)dr[ColunaValor];

                        if (Valor > Q3) dr[ColunaQuartil] = "1";
                        else if (Valor > Q2 && Valor <= Q3) dr[ColunaQuartil] = "2";
                        else if (Valor > Q1 && Valor <= Q2) dr[ColunaQuartil] = "3";
                        else if (Valor <= Q1) dr[ColunaQuartil] = "4";
                    }
                }
                else
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        double Valor = (Int32)dr[ColunaValor];

                        if (Valor > Q3) dr[ColunaQuartil] = "4";
                        else if (Valor > Q2 && Valor <= Q3) dr[ColunaQuartil] = "3";
                        else if (Valor > Q1 && Valor <= Q2) dr[ColunaQuartil] = "2";
                        else if (Valor <= Q1) dr[ColunaQuartil] = "1";
                    }
                }
            }
            return dt;
        }

        private int[] RetornaIndice(double Quartil, int Quantidade)
        {
            int[] Indice = new int[2];
            double Q = Quartil * (Quantidade + 1);

            Indice[0] = (Convert.ToInt32(Math.Floor(Q)) - 1);
            Indice[1] = (Convert.ToInt32(Math.Floor(Q)) - 1) + 1;

            return Indice;
        }

        private double RetornaMedia(double x1, double x2)
        {
            return ((x1 + x2) / 2);
        }
    }
}