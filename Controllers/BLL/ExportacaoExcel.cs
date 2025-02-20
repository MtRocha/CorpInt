using System;
using System.Collections.Generic;
using Excel = Microsoft.Office.Interop.Excel;
using System.Data;
using System.Text;
using System.IO;

namespace Intranet.BLL
{
    public static class ExportacaoExcel
    {
        public static void ExportToExcel(this DataTable Tbl, string ExcelFilePath = null)
        {
            try
            {
                if (Tbl == null || Tbl.Columns.Count == 0)
                    throw new Exception("ExportToExcel_001: Null or empty input table!\n");

                // load excel, and create a new workbook
                Excel.Application excelApp = new Excel.Application();
                excelApp.Workbooks.Add();

                // single worksheet
                Excel._Worksheet workSheet = excelApp.ActiveSheet;

                // column headings
                for (int i = 0; i < Tbl.Columns.Count; i++)
                {
                    workSheet.Cells[1, (i + 1)] = Tbl.Columns[i].ColumnName;
                }

                // rows
                for (int i = 0; i < Tbl.Rows.Count; i++)
                {
                    // to do: format datetime values before printing
                    for (int j = 0; j < Tbl.Columns.Count; j++)
                    {
                        workSheet.Cells[(i + 2), (j + 1)] = Tbl.Rows[i][j].ToString();
                    }
                }

                // check fielpath
                if (ExcelFilePath != null && ExcelFilePath != "")
                {
                    try
                    {
                        workSheet.SaveAs(ExcelFilePath);
                        excelApp.Quit();
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("ExportToExcel_002: Excel file could not be saved! Check filepath.\n"
                            + ex.Message);
                    }
                }
                else    // no filepath is given
                {
                    excelApp.Visible = true;
                }

                excelApp.Quit();
            }
            catch (Exception ex)
            {
                throw new Exception("ExportacaoExcel_001: " + ex.Message, ex);
            }
        }

        public static void ExportDataSetToExcel(DataSet ds, string NomeArquivo)
        {
            try
            {
                Excel.Application excelApp = new Excel.Application();

                Excel.Workbook excelWorkBook = excelApp.Workbooks.Add();

                foreach (System.Data.DataTable table in ds.Tables)
                {
                    Excel.Worksheet excelWorkSheet = excelWorkBook.Sheets.Add();
                    excelWorkSheet.Name = table.TableName;

                    for (int i = 1; i < table.Columns.Count + 1; i++)
                    {
                        excelWorkSheet.Cells[1, i] = table.Columns[i - 1].ColumnName;
                    }

                    for (int j = 0; j < table.Rows.Count; j++)
                    {
                        for (int k = 0; k < table.Columns.Count; k++)
                        {
                            excelWorkSheet.Cells[j + 2, k + 1] = table.Rows[j].ItemArray[k].ToString();
                        }
                    }

                    //Alterar para Negrito a Primeira Linha
                    excelWorkSheet.get_Range("A1", "Z1").Font.Bold = true;

                    //Auto dimensionar as colunas
                    Excel.Range celulas;
                    celulas = excelWorkSheet.get_Range("A1", "Z1");
                    celulas.EntireColumn.AutoFit();
                }
                Excel.Worksheet defalultSheet = null;

                foreach (Excel.Worksheet sheet in excelWorkBook.Sheets)
                {
                    if (sheet.Name == "Planilha1")
                    {
                        defalultSheet = sheet;
                        break;
                    }
                }



                defalultSheet.Delete();

                if (File.Exists(NomeArquivo)) 
                { 
                    File.Delete(NomeArquivo);
                }

                excelWorkBook.SaveAs(NomeArquivo);
                excelWorkBook.Close();
                excelApp.Quit();
            }
            catch (Exception ex)
            {
                throw new Exception("ExportacaoExcel_002: " + ex.Message, ex);
            }
        }

        public static void DataTableToCSV(this DataTable datatable, char seperator, string nm_arquivo)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < datatable.Columns.Count; i++)
            {
                sb.Append(datatable.Columns[i]);
                if (i < datatable.Columns.Count - 1)
                    sb.Append(seperator);
            }
            sb.AppendLine();
            foreach (DataRow dr in datatable.Rows)
            {
                for (int i = 0; i < datatable.Columns.Count; i++)
                {
                    sb.Append(dr[i].ToString());

                    if (i < datatable.Columns.Count - 1)
                        sb.Append(seperator);
                }
                sb.AppendLine();
            }

            File.WriteAllText(nm_arquivo, sb.ToString());
        }

        public static DataSet ToDataSet<T>(this IList<T> list)
        {
            Type elementType = typeof(T);
            DataSet ds = new DataSet();
            DataTable t = new DataTable();
            ds.Tables.Add(t);

            //add a column to table for each public property on T
            foreach (var propInfo in elementType.GetProperties())
            {
                Type ColType = Nullable.GetUnderlyingType(propInfo.PropertyType) ?? propInfo.PropertyType;

                t.Columns.Add(propInfo.Name, ColType);
            }

            //go through each property on T and add each value to the table
            foreach (T item in list)
            {
                DataRow row = t.NewRow();

                foreach (var propInfo in elementType.GetProperties())
                {
                    row[propInfo.Name] = propInfo.GetValue(item, null) ?? DBNull.Value;
                }

                t.Rows.Add(row);
            }

            return ds;
        }
    }
}