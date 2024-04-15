using OfficeOpenXml;
using System.Data;

namespace Common.EPPlus
{
    public class Template
    {
        public static MemoryStream FillReport(string templatefilename, DataSet data)
        {
            return FillReport(templatefilename, data, new string[] { "%", "%" });
        }

        public static MemoryStream FillTemplate(string templatefilename, DataSet data)
        {
            return FillTemp(templatefilename, data, new string[] { "%", "%" });
        }
        /// <summary>
        /// Convert Stream file excel to Dataset read all Table
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static DataSet ConvertToDataSet(MemoryStream stream)
        {
            var ds = new DataSet();
            using (ExcelPackage excelPackage = new ExcelPackage(stream))
            {
                ////loop all Workbook
                //foreach (var Workbook in excelPackage.Workbook.Names)
                //{
                //    var worksheet = Workbook.Worksheet;
                //    foreach (var table in worksheet.Tables)
                //    {
                //        int startRow = table.Address.Start.Column;
                //        int startCol = table.Address.Start.Row;
                //        int endRow = table.Address.End.Column;
                //        int endCol = table.Address.End.Row;
                //        DataTable tbl = new DataTable();
                //        foreach (var firstRowCell in worksheet.Cells[startRow, startCol, startRow, endCol])
                //        {
                //            var colName = firstRowCell.Text;
                //            tbl.Columns.Add(colName);
                //        }
                //        for (int rowNum = startRow + 1; rowNum <= endCol; rowNum++)
                //        {
                //            var wsRow = worksheet.Cells[rowNum, startCol, rowNum, endRow];
                //            DataRow row = tbl.Rows.Add();
                //            foreach (var cell in wsRow)
                //            {

                //                row[cell.Start.Column - startRow] = cell.Value;
                //            }

                //        }
                //        tbl.TableName = table.Name;
                //        ds.Tables.Add(tbl);
                //    }

                //}
                //loop all worksheets
                foreach (ExcelWorksheet worksheet in excelPackage.Workbook.Worksheets)
                {
                    foreach (var table in worksheet.Tables)
                    {
                        int startRow = table.Address.Start.Column;
                        int startCol = table.Address.Start.Row;
                        int endRow = table.Address.End.Column;
                        int endCol = table.Address.End.Row;
                        DataTable tbl = new DataTable();
                        foreach (var firstRowCell in worksheet.Cells[startCol, startRow, startCol, endRow])
                        {
                            var colName = firstRowCell.Text;
                            tbl.Columns.Add(colName);
                        }
                        for (int rowNum = startCol + 1; rowNum <= endCol; rowNum++)
                        {
                            var wsRow = worksheet.Cells[rowNum, startRow, rowNum, endRow];
                            DataRow row = tbl.Rows.Add();
                            foreach (var cell in wsRow)
                            {
                                try
                                {
                                    row[cell.Start.Column - startRow] = cell.Text;
                                }
                                catch (Exception ex)
                                {

                                    throw ex;
                                }

                            }

                        }
                        tbl.TableName = table.Name;
                        ds.Tables.Add(tbl);
                    }

                }
            }

            return ds;
        }

        public static MemoryStream FillReportOne(string templatefilename, DataSet data)
        {
            return FillReportOne(templatefilename, data, new string[] { "%", "%" });
        }

        /// <summary>
        /// Fillter Datatable
        /// </summary>
        /// <param name="templatefilename"></param>
        /// <param name="data"></param>
        /// <param name="deliminator"></param>
        /// <returns></returns>
        public static MemoryStream FillReportOne(string templatefilename, DataSet data, string[] deliminator)
        {
            try
            {

                var file = new MemoryStream();

                using (FileStream temp = new FileStream(templatefilename, FileMode.Open))
                {

                    using (var xls = new ExcelPackage(file, temp))
                    {
                        // process workbook regions
                        foreach (var n in xls.Workbook.Names)
                        {
                            FillWorksheetData(data, n.Worksheet, n, deliminator);
                        }

                        // process worksheet regions
                        foreach (var ws in xls.Workbook.Worksheets)
                        {
                            var rage = ws.Names.ToString();
                            foreach (var n in ws.Names)
                            {
                                var rage1 = n.Name;
                                var rage2 = n.NameComment;
                                FillWorksheetData(data, ws, n, deliminator);
                            }
                        }

                        // process single cells
                        foreach (var ws in xls.Workbook.Worksheets)
                        {
                            int index = 0;
                            foreach (var c in ws.Cells)
                            {

                                //var s = "" + c.Value;
                                //if (s.StartsWith(deliminator[0]) == false && s.EndsWith(deliminator[1]) == false)
                                //    continue;
                                //s = s.Replace(deliminator[0], "").Replace(deliminator[1], "");
                                //var ss = s.Split('.');
                                try
                                {
                                    index += 1;
                                    ws.Cells[3, 1].LoadFromDataTable(data.Tables[0], false);
                                    //ws.DeleteRow(2, 1);
                                    break;
                                }
                                catch { }
                            }
                        }

                        xls.Save();
                    }
                }
                file.Position = 0;
                return file;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public static MemoryStream FillReport(string templatefilename, DataSet data, string[] deliminator)
        {
            try
            {

                var file = new MemoryStream();

                using (FileStream temp = new FileStream(templatefilename, FileMode.Open))
                {

                    using (var xls = new ExcelPackage(file, temp))
                    {
                        // process workbook regions
                        foreach (var n in xls.Workbook.Names)
                        {
                            FillWorksheetData(data, n.Worksheet, n, deliminator);
                        }

                        // process worksheet regions
                        foreach (var ws in xls.Workbook.Worksheets)
                        {
                            var rage = ws.Names.ToString();
                            foreach (var n in ws.Names)
                            {
                                var rage1 = n.Name;
                                var rage2 = n.NameComment;
                                FillWorksheetData(data, ws, n, deliminator);
                            }
                        }

                        // process single cells
                        foreach (var ws in xls.Workbook.Worksheets)
                        {
                            foreach (var c in ws.Cells)
                            {
                                var s = "" + c.Value;
                                if (s.StartsWith(deliminator[0]) == false && s.EndsWith(deliminator[1]) == false)
                                    continue;
                                s = s.Replace(deliminator[0], "").Replace(deliminator[1], "");
                                var ss = s.Split('.');
                                try
                                {
                                    c.Value = data.Tables[ss[0]].Rows[0][ss[1]];
                                }
                                catch { }
                            }
                        }

                        xls.Save();
                    }
                }
                file.Position = 0;
                return file;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        // FEATURE : handle multidimensional regions??
        private static void FillWorksheetData(DataSet data, ExcelWorksheet ws, ExcelNamedRange n, string[] deliminator)
        {
            // region exists in data
            if (data.Tables.Contains(n.Name) == false)
                return;

            var dt = data.Tables[n.Name];

            int row = n.Start.Row;

            var cn = new string[n.Columns];
            var st = new int[n.Columns];
            for (int i = 0; i < n.Columns; i++)
            {
                var x = (n.Value as object[,])[0, i];
                if (x != null)
                {
                    cn[i] = x.ToString().Replace(deliminator[0], "").Replace(deliminator[1], "");
                }
                else
                {
                    cn[i] = "";
                }
                if (cn[i].Contains("."))
                    cn[i] = cn[i].Split('.')[1];

                st[i] = ws.Cells[row, n.Start.Column + i].StyleID;
            }

            foreach (DataRow r in dt.Rows)
            {
                for (int col = 0; col < n.Columns; col++)
                {
                    if (dt.Columns.Contains(cn[col]))
                        ws.Cells[row, n.Start.Column + col].Value = r[cn[col]]; // set cell data

                    ws.Cells[row, n.Start.Column + col].StyleID = st[col]; // set cell style
                }
                row++;
            }

            // extend table formatting range to all rows
            foreach (var t in ws.Tables)
            {
                var a = t.Address;
                if (n.Start.Row.Between(a.Start.Row, a.End.Row) &&
                    n.Start.Column.Between(a.Start.Column, a.End.Column))
                    t.ExtendRows(dt.Rows.Count - 1);

            }
        }

        public static MemoryStream FillColumn(string templatefilename, DataSet data)
        {
            return FillColumnReport(templatefilename, data, new string[] { "%", "%" });
        }

        public static MemoryStream FillColumnReport(string templatefilename, DataSet data, string[] deliminator)
        {
            try
            {

                var file = new MemoryStream();
                using (FileStream temp = new FileStream(templatefilename, FileMode.Open))
                {
                    using (var xls = new ExcelPackage(file, temp))
                    {

                        DataTable dt = data.Tables["head"];
                        //Set Header
                        foreach (var ws in xls.Workbook.Worksheets)
                        {
                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                ws.Cells[1, 7 + i].Value = dt.Rows[i][0].ToString();
                                ws.Cells[2, 7 + i].Value = dt.Rows[i][1].ToString();
                            }
                            ws.Cells[3, 1].LoadFromDataTable(data.Tables["detail"], false);
                            ws.Row(1).Hidden = true;
                            ws.Column(2).Hidden = true;
                            ws.OutLineApplyStyle = true;
                        }
                        xls.Save();
                    }
                }
                file.Position = 0;
                return file;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static MemoryStream FillTemp(string templatefilename, DataSet data, string[] deliminator)
        {
            try
            {

                var file = new MemoryStream();

                using (FileStream temp = new FileStream(templatefilename, FileMode.Open))
                {

                    using (var xls = new ExcelPackage(file, temp))
                    {
                        // process workbook regions
                        foreach (var n in xls.Workbook.Names)
                        {
                            FillWorksheetData(data, n.Worksheet, n, deliminator);
                        }

                        //// process worksheet regions
                        //foreach (var ws in xls.Workbook.Worksheets)
                        //{
                        //    var rage = ws.Names.ToString();
                        //    foreach (var n in ws.Names)
                        //    {
                        //        var rage1 = n.Name;
                        //        var rage2 = n.NameComment;
                        //        FillWorksheetData(data, ws, n, deliminator);
                        //    }
                        //}

                        // process single cells
                        foreach (var ws in xls.Workbook.Worksheets)
                        {
                            if (ws.Name == "Data")
                            {
                                ws.Cells[4, 1].LoadFromDataTable(data.Tables[ws.Name], false);
                                //foreach (var c in ws.Cells)
                                //{
                                //    var s = "" + c.Value;
                                //    if (s.StartsWith(deliminator[0]) == false && s.EndsWith(deliminator[1]) == false)
                                //        continue;
                                //    s = s.Replace(deliminator[0], "").Replace(deliminator[1], "");
                                //    var ss = s.Split('.');
                                //    try
                                //    {
                                //        c.Value = data.Tables[ss[0]].Rows[0][ss[1]];
                                //    }
                                //    catch { }
                                //}
                            }

                        }

                        xls.Save();
                    }
                }
                file.Position = 0;
                return file;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public static MemoryStream FillTemp<T>(string templatefilename, IEnumerable<T> list)
        {
            try
            {
                var file = new MemoryStream();
                using (FileStream temp = new FileStream(templatefilename, FileMode.Open))
                {

                    using (var xls = new ExcelPackage(file, temp))
                    {
                        // process single cells
                        foreach (var ws in xls.Workbook.Worksheets)
                        {
                            if (ws.Name == "Data")
                            {
                                ws.Cells[4, 1].LoadFromCollection(list);
                            }
                        }
                        xls.Save();
                    }
                }
                file.Position = 0;
                return file;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="templatefilename"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static MemoryStream FillTablePayroll(string templatefilename, DataSet data)
        {
            return DynamicPayroll(templatefilename, data, new string[] { "%", "%" });
        }
        /// <summary>
        /// Fillter Datatable
        /// </summary>
        /// <param name="templatefilename"></param>
        /// <param name="data"></param>
        /// <param name="deliminator"></param>
        /// <returns></returns>
        public static MemoryStream DynamicPayroll(string templatefilename, DataSet data, string[] deliminator)
        {
            try
            {

                var file = new MemoryStream();

                using (FileStream temp = new FileStream(templatefilename, FileMode.Open))
                {

                    using (var xls = new ExcelPackage(file, temp))
                    {

                        // process single cells
                        foreach (var ws in xls.Workbook.Worksheets)
                        {
                            ws.Cells[1, 1].Value = data.Tables["Title"].Rows[0]["PNAME"].ToString();
                            ws.Cells[2, 1].Value = data.Tables["Title"].Rows[0]["ONAME"].ToString();
                            if (data.Tables["head"].Rows.Count > 0)
                            {
                                if (data.Tables["head"].Rows.Count > 2)
                                {
                                    ws.InsertColumn(10, data.Tables["head"].Rows.Count-2, 9);
                                   
                                }
                                
                                for (int i = 0; i < data.Tables["head"].Rows.Count ; i++)
                                {
                                    ws.Column(9 + i).Width = 15;
                                    ws.Cells[4, 9 + i].Value = data.Tables["head"].Rows[i][0].ToString();
                                    ws.Cells[4, 9 + i, 5, 9 + i].Merge = true;
                                }
                                //  ws.Cells["A1:C1"].Merge = true;
                            }
                            ws.InsertRow(7, data.Tables["Data"].Rows.Count - 1, 6);
                            ws.Cells[6, 1].LoadFromDataTable(data.Tables["Data"], false);
                        }
                        xls.Save();
                    }
                }
                file.Position = 0;
                return file;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public static MemoryStream FillColumnBinhQuan(string templatefilename, DataSet data)
        {
            return FillColumnReportBinhQuan(templatefilename, data, new string[] { "%", "%" });
        }
        public static MemoryStream FillColumnReportBinhQuan(string templatefilename, DataSet data, string[] deliminator)
        {
            try
            {
                var file = new MemoryStream();
                using (FileStream temp = new FileStream(templatefilename, FileMode.Open))
                {
                    using (var xls = new ExcelPackage(file, temp))
                    {

                        //DataTable dt = data.Tables["head"];
                        //Set Header
                        foreach (var ws in xls.Workbook.Worksheets)
                        {
                            //for (int i = 0; i < dt.Rows.Count; i++)
                            //{
                            //    ws.Cells[1, 7 + i].Value = dt.Rows[i][0].ToString();
                            //    ws.Cells[2, 7 + i].Value = dt.Rows[i][1].ToString();
                            //}
                            DataTable dt = data.Tables["detail"];
                            ws.Cells[9, 1].LoadFromDataTable(data.Tables["detail"], false);
                            ws.Row(1).Hidden = true;
                           // ws.Column(2).Hidden = true;
                            ws.OutLineApplyStyle = true;
                        }
                        xls.Save();
                    }
                }
                file.Position = 0;
                return file;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }

    public static class int_between
    {
        public static bool Between(this int v, int a, int b)
        {
            return v >= a && v <= b;
        }
    }
}
