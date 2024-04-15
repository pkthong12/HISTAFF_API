using API.All.DbContexts;
using Aspose.Cells;
using ClosedXML.Excel;
using Common.DataAccess;
using Common.Extensions;
using Common.Interfaces;
using CORE.DTO;
using CORE.Enum;
using CORE.StaticConstant;
using DocumentFormat.OpenXml.Office2010.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Logical;
using System;
using System.Data;
using System.Globalization;
using System.IO;
using System.IO.Packaging;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using ValidationType = Aspose.Cells.ValidationType;

namespace API.All.SYSTEM.CoreAPI.Excel
{
    public class ExcelRepository : IExcelRespository
    {

        private readonly FullDbContext _dbContext;

        public ExcelRepository(FullDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        #region HUNGNX
        public async Task<byte[]> DeleteExcelSheetByName(string sheetName, byte[] excelFileBytes)
        {
            using (var memoryStream = new MemoryStream(excelFileBytes))
            {
                using (var workbook = new XLWorkbook(memoryStream))
                {
                    // Get the worksheet by its name
                    var sheetToDelete = workbook.Worksheet(sheetName);

                    if (sheetToDelete != null)
                    {
                        // Delete the sheet
                        workbook.Worksheets.Delete(sheetName);
                    }

                    // Save the changes asynchronously
                    using (var modifiedMemoryStream = new MemoryStream())
                    {
                        await Task.Run(() => workbook.SaveAs(modifiedMemoryStream));
                        return modifiedMemoryStream.ToArray();
                    }
                }
            }
        }

        public Aspose.Cells.Workbook ConvertBase64ToWorkbook(string base64String)
        {
            byte[] bytes = Convert.FromBase64String(base64String);

            using (MemoryStream stream = new MemoryStream(bytes))
            {
                Aspose.Cells.LoadOptions loadOptions = new Aspose.Cells.LoadOptions(LoadFormat.Xlsx);
                Aspose.Cells.Workbook workbook = new Aspose.Cells.Workbook(stream, loadOptions);
                return workbook;
            }
        }

        public async Task<byte[]> ExportTempImportBasic(string filePath, DataSet dtDataValue)
        {
            WorkbookDesigner designer;
            Aspose.Cells.Workbook workbook;
            try
            {
                designer = new WorkbookDesigner();
                workbook = new Aspose.Cells.Workbook(filePath);
                designer.Workbook = workbook;


                designer.SetDataSource(dtDataValue);
                designer.Process();

                // Save the modified workbook to a byte array
                MemoryStream stream = new MemoryStream();
                workbook.Save(stream, SaveFormat.Xlsx);
                byte[] fileBytes = stream.ToArray();

                return await DeleteExcelSheetByName("Evaluation Warning", fileBytes);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while exporting the template.", ex);
            }
        }
        #endregion

        #region import lương, thuế tháng
        public async Task<byte[]> ExportTempImportSalary(string filePath, DataSet dtDataValue, DataTable dtColname)
        {
            WorkbookDesigner designer;
            Aspose.Cells.Workbook workbook;
            try
            {
                designer = new WorkbookDesigner();
                workbook = new Aspose.Cells.Workbook(filePath);
                designer.Workbook = workbook;

                Cells cell = designer.Workbook.Worksheets[0].Cells;
                ValidationCollection validations = designer.Workbook.Worksheets[0].Validations;

                int i = 6;
                foreach (DataRow dr in dtColname.Rows)
                {
                    cell[1, i].PutValue(dr["COLNAME"]);
                    cell[2, i].PutValue(dr["COLVAL"]);
                    cell[3, i].PutValue(dr["COLDATA"]);
                    CellArea cellArea = new CellArea();
                    Validation validation = validations[validations.Add()];
                    string pattern = "[^A-Z]";
                    string col = cell[3, i - 1].Name;
                    string colName = Regex.Replace(col, pattern, "");
                    cellArea.StartRow = 3;
                    cellArea.EndRow = dtColname.Rows.Count;
                    cellArea.StartColumn = i;
                    cellArea.EndColumn = i;
                    validation.Type = ValidationType.AnyValue;
                    validation.Formula1 = "";
                    validation.AddArea(cellArea);
                    cell.SetColumnWidth(i, 25);
                    i++;
                    cell.InsertColumn(i);
                }
                // Remove the last two extra columns
                cell.DeleteColumn(i + 1);
                cell.DeleteColumn(i);

                designer.SetDataSource(dtDataValue);
                designer.Process();
                MemoryStream stream = new MemoryStream();
                workbook.Save(stream, SaveFormat.Xlsx);
                byte[] fileBytes = stream.ToArray();

                return await DeleteExcelSheetByName("Evaluation Warning", fileBytes);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while exporting the template.", ex);
            }
        }


        public async Task<FormatedResponse> ImportTempSalary(long? salObj, long? periodId, long? otherId, string base64, List<string> lstColVal, long? recordSuccess, long? year, string typeImport)
        {
            try
            {
                DataSet dsDataPre = new DataSet();
                base64 = base64.Replace("data:application/octet-stream;base64,", "");
                Aspose.Cells.Workbook workbook = ConvertBase64ToWorkbook(base64);
                Aspose.Cells.Worksheet worksheet;

                string sqlInsert = lstColVal.Aggregate((cur, next) => cur + "," + next);
                string sqlInsert_Temp;

                worksheet = workbook.Worksheets[0];
                dsDataPre.Tables.Add(worksheet.Cells.ExportDataTableAsString(2, 0, worksheet.Cells.MaxRow - 1, worksheet.Cells.MaxColumn + 1, true));
                DataTable dtData = dsDataPre.Tables[0];

                if (lstColVal.Count <= 0)
                {
                    return new FormatedResponse() { MessageCode = CommonMessageCode.MUST_BE_HAVE_ONE_RECORD, ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400 };
                }
                else
                {
                    foreach (DataRow dr in dtData.Rows)
                    {
                        if (dr["ID"] is DBNull || dr["ID"].ToString() == "")
                        {
                            continue;
                        }

                        sqlInsert_Temp = "," + sqlInsert + ",";
                        string sqlInsertVal = "";

                        foreach (string parm in lstColVal)
                        {
                            if (dr[parm].ToString() != "")
                            {
                                string sParam = dr[parm].ToString().Replace(",", "");
                                sqlInsertVal += sParam + ",";
                            }
                            else
                            {

                                //sqlInsert_Temp = sqlInsert_Temp.Replace("," + parm + ",", ",");
                                sqlInsertVal += "0" + ",";
                            }
                        }

                        if (sqlInsertVal != "")
                        {
                            sqlInsertVal = sqlInsertVal.Remove(sqlInsertVal.Length - 1, 1);
                        }

                        if (sqlInsert_Temp != "")
                        {
                            sqlInsert_Temp = sqlInsert_Temp.Remove(0, 1);
                            sqlInsert_Temp = sqlInsert_Temp.Remove(sqlInsert_Temp.Length - 1, 1);
                        }
                        var QueryData = new SqlQueryDataTemplate(_dbContext);

                        string[] mangChuoi1 = sqlInsert_Temp.Split(',');
                        string[] mangChuoi2 = sqlInsertVal.Split(',');
                        string chuoiMoi = "";
                        if (mangChuoi1.Length == mangChuoi2.Length)
                        {


                            for (int i = 0; i < mangChuoi1.Length; i++)
                            {
                                chuoiMoi += mangChuoi1[i] + "= " + mangChuoi2[i];

                                if (i < mangChuoi1.Length - 1)
                                {
                                    chuoiMoi += ", ";
                                }
                            }
                        }
                        if (typeImport == "BACK_DATE")
                        {
                            var r = await QueryData.ExecuteNonQuery("PKG_IMPORT_SALARY_BACKDATE",
                                    new
                                    {
                                        P_OBJ_SALARY_ID = salObj,
                                        P_PERIOD_ID = periodId,
                                        P_PERIOD_ADD_ID = otherId,
                                        P_YEAR = year,
                                        P_EMPLOYEE_ID = long.Parse(dr["ID"].ToString()),
                                        P_ORG_ID = long.Parse(dr["ORG_ID"].ToString()),
                                        P_NOTE = dr["NOTE"].ToString(),
                                        P_CREATED_BY = _dbContext.UserName,
                                        P_CREATED_LOG = _dbContext.CurrentUserId,
                                        P_LISTCOL = sqlInsert_Temp,
                                        P_LISTVAL = sqlInsertVal,
                                        P_UPDATE = chuoiMoi
                                    }, false);
                        }
                        else if (typeImport == "ADD")
                        {
                            var r = await QueryData.ExecuteNonQuery("PKG_IMPORT_SALARY_ADD",
                                    new
                                    {
                                        P_OBJ_SALARY_ID = salObj,
                                        P_PERIOD_ID = periodId,
                                        P_YEAR = year,
                                        P_EMPLOYEE_ID = long.Parse(dr["ID"].ToString()),
                                        P_ORG_ID = long.Parse(dr["ORG_ID"].ToString()),
                                        P_NOTE = dr["NOTE"].ToString(),
                                        P_PHASE_ID = otherId,
                                        P_CREATED_BY = _dbContext.UserName,
                                        P_CREATED_LOG = _dbContext.CurrentUserId,
                                        P_LISTCOL = sqlInsert_Temp,
                                        P_LISTVAL = sqlInsertVal,
                                        P_UPDATE = chuoiMoi
                                    }, false);
                        }
                        else if (typeImport == "TAX_MONTH")
                        {
                            var r = await QueryData.ExecuteNonQuery("PKG_IMPORT_TAX_MONTH",
                                    new
                                    {
                                        P_OBJ_SALARY_ID = salObj,
                                        P_PERIOD_ID = periodId,
                                        P_YEAR = year,
                                        P_EMPLOYEE_ID = long.Parse(dr["ID"].ToString()),
                                        P_ORG_ID = long.Parse(dr["ORG_ID"].ToString()),
                                        P_NOTE = dr["NOTE"].ToString(),
                                        P_DATE_CAL_ID = otherId,
                                        P_CREATED_BY = _dbContext.UserName,
                                        P_CREATED_LOG = _dbContext.CurrentUserId,
                                        P_LISTCOL = sqlInsert_Temp,
                                        P_LISTVAL = sqlInsertVal,
                                        P_UPDATE = chuoiMoi
                                    }, false);
                        }
                        else if (typeImport == "TAX_YEAR")
                        {
                            var r = await QueryData.ExecuteNonQuery("PKG_IMPORT_TAX_YEAR",
                                   new
                                   {
                                       P_OBJ_SALARY_ID = salObj,
                                       P_YEAR = year,
                                       P_EMPLOYEE_ID = long.Parse(dr["ID"].ToString()),
                                       P_ORG_ID = long.Parse(dr["ORG_ID"].ToString()),
                                       P_NOTE = dr["NOTE"].ToString(),
                                       P_CREATED_BY = _dbContext.UserName,
                                       P_CREATED_LOG = _dbContext.CurrentUserId,
                                       P_LISTCOL = sqlInsert_Temp,
                                       P_LISTVAL = sqlInsertVal,
                                       P_UPDATE = chuoiMoi
                                   }, false);
                        }
                        else
                        {
                            var r = await QueryData.ExecuteNonQuery("PKG_IMPORT_SALARY",
                                    new
                                    {
                                        P_OBJ_SALARY_ID = salObj,
                                        P_PERIOD_ID = periodId,
                                        P_YEAR = year,
                                        P_EMPLOYEE_ID = long.Parse(dr["ID"].ToString()),
                                        P_ORG_ID = long.Parse(dr["ORG_ID"].ToString()),
                                        P_NOTE = dr["NOTE"].ToString(),
                                        P_CREATED_BY = _dbContext.UserName,
                                        P_CREATED_LOG = _dbContext.CurrentUserId,
                                        P_LISTCOL = sqlInsert_Temp,
                                        P_LISTVAL = sqlInsertVal,
                                        P_UPDATE = chuoiMoi
                                    }, false);
                        }
                    }
                    return new() { InnerBody = null, MessageCode = CommonMessageCode.IMPORT_SALARY_SUCCESS };
                }

            }
            catch (Exception ex)
            {
                return new FormatedResponse() { MessageCode = ex.Message, ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400 };
            }
        }

        #endregion

        #region import dữ liệu công
        public async Task<byte[]> ExportTempImportTimeSheet(string filePath, DataSet dtDataValue, long? periodId)
        {
            var QueryData = new SqlQueryDataTemplate(_dbContext);
            var periodObj = await _dbContext.AtSalaryPeriods.FirstOrDefaultAsync(x => x.ID == periodId);
            if (periodObj != null)
            {
                DataTable dtvariable = new DataTable();
                dtvariable.Columns.Add("D1", typeof(DateTime));
                dtvariable.Columns.Add("D2", typeof(DateTime));
                dtvariable.Columns.Add("D3", typeof(DateTime));
                dtvariable.Columns.Add("D4", typeof(DateTime));
                dtvariable.Columns.Add("D5", typeof(DateTime));
                dtvariable.Columns.Add("D6", typeof(DateTime));
                dtvariable.Columns.Add("D7", typeof(DateTime));
                dtvariable.Columns.Add("D8", typeof(DateTime));
                dtvariable.Columns.Add("D9", typeof(DateTime));
                dtvariable.Columns.Add("D10", typeof(DateTime));
                dtvariable.Columns.Add("D11", typeof(DateTime));
                dtvariable.Columns.Add("D12", typeof(DateTime));
                dtvariable.Columns.Add("D13", typeof(DateTime));
                dtvariable.Columns.Add("D14", typeof(DateTime));
                dtvariable.Columns.Add("D15", typeof(DateTime));
                dtvariable.Columns.Add("D16", typeof(DateTime));
                dtvariable.Columns.Add("D17", typeof(DateTime));
                dtvariable.Columns.Add("D18", typeof(DateTime));
                dtvariable.Columns.Add("D19", typeof(DateTime));
                dtvariable.Columns.Add("D20", typeof(DateTime));
                dtvariable.Columns.Add("D21", typeof(DateTime));
                dtvariable.Columns.Add("D22", typeof(DateTime));
                dtvariable.Columns.Add("D23", typeof(DateTime));
                dtvariable.Columns.Add("D24", typeof(DateTime));
                dtvariable.Columns.Add("D25", typeof(DateTime));
                dtvariable.Columns.Add("D26", typeof(DateTime));
                dtvariable.Columns.Add("D27", typeof(DateTime));
                dtvariable.Columns.Add("D28", typeof(DateTime));
                dtvariable.Columns.Add("D29", typeof(DateTime));
                dtvariable.Columns.Add("D30", typeof(DateTime));
                dtvariable.Columns.Add("D31", typeof(DateTime));
                DateTime dDay = periodObj.START_DATE.Date;
                DataRow row = dtvariable.NewRow();
                int DaySum = 0;
                while (dDay <= periodObj.END_DATE.Date)
                {
                    row["D" + dDay.Day] = dDay;
                    dDay = dDay.AddDays(1);
                    DaySum = DaySum + 1;
                }
                dtvariable.Rows.Add(row);
                dtvariable.TableName = "Variable";

                return await DeleteExcelSheetByName("Evaluation Warning", GetByteTimeSheet(filePath, dtDataValue, dtvariable, DaySum));
            }
            else
            {
                return null;
            }
        }

        public byte[] GetByteTimeSheet(string filePath, DataSet dtData, DataTable dtVariable, int DaySum)
        {
            WorkbookDesigner designer;
            Aspose.Cells.Workbook workbook;
            designer = new WorkbookDesigner();
            workbook = new Aspose.Cells.Workbook(filePath);
            designer.Workbook = workbook;
            designer.SetDataSource(dtData);
            if (dtVariable != null)
            {
                int intCols = dtVariable.Columns.Count;
                for (int i = 0; i < intCols; i++)
                {
                    designer.SetDataSource(dtVariable.Columns[i].ColumnName.ToString(), dtVariable.Rows[0].ItemArray[i].ToString());
                }
            }
            designer.Process();
            designer.Workbook.CalculateFormula();
            Aspose.Cells.Workbook workbook1 = designer.Workbook;
            Aspose.Cells.Worksheet worksheet = workbook1.Worksheets[0];
            Style style = workbook1.CreateStyle();
            style.Font.Name = "Times New Roman";
            style.HorizontalAlignment = TextAlignmentType.Center;
            style.VerticalAlignment = TextAlignmentType.Center;
            style.Font.IsBold = true;
            style.ForegroundColor = System.Drawing.Color.Red;
            style.Pattern = BackgroundType.Solid;
            style.Borders[BorderType.TopBorder].LineStyle = CellBorderType.Thin;
            style.Borders[BorderType.TopBorder].Color = System.Drawing.Color.Black;
            style.Borders[BorderType.BottomBorder].LineStyle = CellBorderType.Thin;
            style.Borders[BorderType.BottomBorder].Color = System.Drawing.Color.Black;
            style.Borders[BorderType.LeftBorder].LineStyle = CellBorderType.Thin;
            style.Borders[BorderType.LeftBorder].Color = System.Drawing.Color.Black;
            style.Borders[BorderType.RightBorder].LineStyle = CellBorderType.Thin;
            style.Borders[BorderType.RightBorder].Color = System.Drawing.Color.Black;
            for (int index = 7; index <= 37; index++)
            {
                if (worksheet.Cells.GetCell(2, index).Value.ToString() != "")
                {
                    if (DateTime.Parse((string)worksheet.Cells.GetCell(2, index).Value).DayOfWeek == DayOfWeek.Saturday || DateTime.Parse((string)worksheet.Cells.GetCell(2, index).Value).DayOfWeek == DayOfWeek.Sunday)
                    {
                        worksheet.Cells.GetCell(2, index).SetStyle(style);
                    }
                    worksheet.Cells.GetCell(2, index).PutValue(DateTime.Parse((string)worksheet.Cells.GetCell(2, index).Value).ToString("dd/MM"));
                }
            }
            if (DaySum == 28)
            {
                workbook1.Worksheets[0].Cells.HideColumn(37);
                workbook1.Worksheets[0].Cells.HideColumn(36);
                workbook1.Worksheets[0].Cells.HideColumn(35);
            }
            else if (DaySum == 29)
            {
                workbook1.Worksheets[0].Cells.HideColumn(37);
                workbook1.Worksheets[0].Cells.HideColumn(36);
            }
            else if (DaySum == 30)
            {
                workbook1.Worksheets[0].Cells.HideColumn(37);
            }
            workbook1.CalculateFormula();
            MemoryStream stream = new MemoryStream();
            workbook1.Save(stream, SaveFormat.Xlsx);
            byte[] fileBytes = stream.ToArray();
            return fileBytes;
        }


        public async Task<FormatedResponse> ImportTimeSheetDaily(string base64, long? PeriodID)
        {
            try
            {

                DataSet dsDataPre = new DataSet();
                base64 = base64.Replace("data:application/octet-stream;base64,", "");
                Aspose.Cells.Workbook workbook = ConvertBase64ToWorkbook(base64);
                Aspose.Cells.Worksheet worksheet;

                worksheet = workbook.Worksheets[0];
                dsDataPre.Tables.Add(worksheet.Cells.ExportDataTableAsString(3, 0, worksheet.Cells.MaxRow + 1, worksheet.Cells.MaxColumn + 1, true));

                DataTable dtData = dsDataPre.Tables[0];

                var QueryData = new SqlQueryDataTemplate(_dbContext);
                var periodObj = _dbContext.AtSalaryPeriods.FirstOrDefault(x => x.ID == PeriodID);
                var dtManual = await _dbContext.AtTimeTypes.Where(x => x.IS_ACTIVE == true).ToListAsync();
                foreach (DataRow row in dtData.Rows)
                {
                    if (row["EMPLOYEE_ID"].ToString() != "")
                    {
                        var objParam = new
                        {
                            P_EMPLOYEEID = row["EMPLOYEE_ID"].ToString(),
                            P_ORG_ID = row["ORG_ID"].ToString(),
                            P_PERIODId = PeriodID,
                            P_USERNAME = _dbContext.UserName,
                            P_D1 = (from p in dtManual where p.CODE.ToUpper() == row["D1"].ToString().ToUpper() select p.ID).FirstOrDefault(),
                            P_D2 = (from p in dtManual where p.CODE.ToUpper() == row["D2"].ToString().ToUpper() select p.ID).FirstOrDefault(),
                            P_D3 = (from p in dtManual where p.CODE.ToUpper() == row["D3"].ToString().ToUpper() select p.ID).FirstOrDefault(),
                            P_D4 = (from p in dtManual where p.CODE.ToUpper() == row["D4"].ToString().ToUpper() select p.ID).FirstOrDefault(),
                            P_D5 = (from p in dtManual where p.CODE.ToUpper() == row["D5"].ToString().ToUpper() select p.ID).FirstOrDefault(),
                            P_D6 = (from p in dtManual where p.CODE.ToUpper() == row["D6"].ToString().ToUpper() select p.ID).FirstOrDefault(),
                            P_D7 = (from p in dtManual where p.CODE.ToUpper() == row["D7"].ToString().ToUpper() select p.ID).FirstOrDefault(),
                            P_D8 = (from p in dtManual where p.CODE.ToUpper() == row["D8"].ToString().ToUpper() select p.ID).FirstOrDefault(),
                            P_D9 = (from p in dtManual where p.CODE.ToUpper() == row["D9"].ToString().ToUpper() select p.ID).FirstOrDefault(),
                            P_D10 = (from p in dtManual where p.CODE.ToUpper() == row["D10"].ToString().ToUpper() select p.ID).FirstOrDefault(),
                            P_D11 = (from p in dtManual where p.CODE.ToUpper() == row["D11"].ToString().ToUpper() select p.ID).FirstOrDefault(),
                            P_D12 = (from p in dtManual where p.CODE.ToUpper() == row["D12"].ToString().ToUpper() select p.ID).FirstOrDefault(),
                            P_D13 = (from p in dtManual where p.CODE.ToUpper() == row["D13"].ToString().ToUpper() select p.ID).FirstOrDefault(),
                            P_D14 = (from p in dtManual where p.CODE.ToUpper() == row["D14"].ToString().ToUpper() select p.ID).FirstOrDefault(),
                            P_D15 = (from p in dtManual where p.CODE.ToUpper() == row["D15"].ToString().ToUpper() select p.ID).FirstOrDefault(),
                            P_D16 = (from p in dtManual where p.CODE.ToUpper() == row["D16"].ToString().ToUpper() select p.ID).FirstOrDefault(),
                            P_D17 = (from p in dtManual where p.CODE.ToUpper() == row["D17"].ToString().ToUpper() select p.ID).FirstOrDefault(),
                            P_D18 = (from p in dtManual where p.CODE.ToUpper() == row["D18"].ToString().ToUpper() select p.ID).FirstOrDefault(),
                            P_D19 = (from p in dtManual where p.CODE.ToUpper() == row["D19"].ToString().ToUpper() select p.ID).FirstOrDefault(),
                            P_D20 = (from p in dtManual where p.CODE.ToUpper() == row["D20"].ToString().ToUpper() select p.ID).FirstOrDefault(),
                            P_D21 = (from p in dtManual where p.CODE.ToUpper() == row["D21"].ToString().ToUpper() select p.ID).FirstOrDefault(),
                            P_D22 = (from p in dtManual where p.CODE.ToUpper() == row["D22"].ToString().ToUpper() select p.ID).FirstOrDefault(),
                            P_D23 = (from p in dtManual where p.CODE.ToUpper() == row["D23"].ToString().ToUpper() select p.ID).FirstOrDefault(),
                            P_D24 = (from p in dtManual where p.CODE.ToUpper() == row["D24"].ToString().ToUpper() select p.ID).FirstOrDefault(),
                            P_D25 = (from p in dtManual where p.CODE.ToUpper() == row["D25"].ToString().ToUpper() select p.ID).FirstOrDefault(),
                            P_D26 = (from p in dtManual where p.CODE.ToUpper() == row["D26"].ToString().ToUpper() select p.ID).FirstOrDefault(),
                            P_D27 = (from p in dtManual where p.CODE.ToUpper() == row["D27"].ToString().ToUpper() select p.ID).FirstOrDefault(),
                            P_D28 = (from p in dtManual where p.CODE.ToUpper() == row["D28"].ToString().ToUpper() select p.ID).FirstOrDefault(),
                            P_D29 = (from p in dtManual where p.CODE.ToUpper() == row["D29"].ToString().ToUpper() select p.ID).FirstOrDefault(),
                            P_D30 = (from p in dtManual where p.CODE.ToUpper() == row["D30"].ToString().ToUpper() select p.ID).FirstOrDefault(),
                            P_D31 = (from p in dtManual where p.CODE.ToUpper() == row["D31"].ToString().ToUpper() select p.ID).FirstOrDefault()
                        };
                        await QueryData.ExecuteNonQuery("PKG_INSERT_WORKSIGN_DATE", objParam, false);
                    }

                }
                //PKG 2
                var objParam1 = new
                {
                    P_STARTDATE = periodObj.START_DATE,
                    P_ENDDATE = periodObj.END_DATE,
                    P_USERNAME = _dbContext.UserName
                };

                await QueryData.ExecuteNonQuery("PKG_UPDATE_LEAVESHEET_DAILY", objParam1, false);

                return new() { MessageCode = CommonMessageCode.IMPORT_TIMESHEET_DAILY_SUCCESS, ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode200 };
            }
            catch (Exception ex)
            {
                return new() { MessageCode = ex.Message, ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400 };

            }

        }

        public async Task<byte[]> CheckValidImportTimeSheet(string errorFilePath, string base64, long? PeriodID)
        {
            try
            {
                DataSet dsDataPre = new DataSet();
                base64 = base64.Replace("data:application/octet-stream;base64,", "");
                Aspose.Cells.Workbook workbook = ConvertBase64ToWorkbook(base64);
                Aspose.Cells.Worksheet worksheet;
                worksheet = workbook.Worksheets[0];
                dsDataPre.Tables.Add(worksheet.Cells.ExportDataTableAsString(3, 0, worksheet.Cells.MaxRow + 1, worksheet.Cells.MaxColumn + 1, true));
                DataTable dtData = dsDataPre.Tables[0];


                for (int rowIndex = dtData.Rows.Count - 1; rowIndex >= 0; rowIndex--)
                {
                    DataRow row = dtData.Rows[rowIndex];
                    bool isRowEmpty = true;
                    foreach (var item in row.ItemArray)
                    {
                        if (!string.IsNullOrEmpty(item?.ToString()))
                        {
                            isRowEmpty = false;
                            break;
                        }
                    }
                    if (isRowEmpty)
                    {
                        dtData.Rows.RemoveAt(rowIndex);
                    }
                }

                DataTable dtError = new DataTable("ERROR");
                var dtDatas = await _dbContext.AtSalaryPeriods.FirstOrDefaultAsync(x => x.ID == PeriodID);
                DataRow rowError;
                bool isError = false;
                string sError = string.Empty;
                DataTable dtDataImportEmployee;
                DateTime startTime = DateTime.UtcNow;
                int irow = 6;

                dtDataImportEmployee = dtData.Clone();
                dtError = dtData.Clone();
                dtError.Columns.Add("STT");
                var dtManual = await _dbContext.AtTimeTypes.Where(x => x.IS_ACTIVE == true).ToListAsync();
                foreach (DataRow row in dtData.Rows)
                {
                    isError = false;
                    rowError = dtError.NewRow();
                    sError = "Chưa nhập mã nhân viên";
                    if (row["EMPLOYEE_CODE"].ToString() == "")
                    {
                        rowError["EMPLOYEE_CODE"] = sError;
                        isError = true;
                    }
                    for (int index = 1; index <= (dtDatas.END_DATE - dtDatas.START_DATE).TotalDays + 1; index++)
                    {
                        if (row["D" + index].ToString() != "")
                        {
                            var r = row["D" + index].ToString();
                            var exists = (from p in dtManual where p.CODE.ToUpper() == r.ToUpper() select p).FirstOrDefault();
                            if (exists == null)
                            {
                                rowError["D" + index] = row["D" + index].ToString() + " không tồn tại";
                                isError = true;
                            }
                            else
                            {
                                row["D" + index] = exists.ID;
                            }
                        }
                    }
                    if (isError)
                    {
                        rowError["STT"] = irow;
                        if (rowError["EMPLOYEE_CODE"].ToString() == "")
                        {
                            rowError["EMPLOYEE_CODE"] = row["EMPLOYEE_CODE"].ToString();
                        }
                        rowError["VN_FULLNAME"] = row["VN_FULLNAME"].ToString();
                        rowError["ORG_NAME"] = row["ORG_NAME"].ToString();
                        rowError["TITLE_NAME"] = row["TITLE_NAME"].ToString();
                        dtError.Rows.Add(rowError);
                    }
                    irow += 1;
                }
                if (dtError.Rows.Count > 0)
                {
                    dtError.TableName = "DATA";
                    DataSet dsData = new DataSet();
                    dsData.Tables.Add(dtError);
                    WorkbookDesigner designer;
                    Aspose.Cells.Workbook workbook1;
                    designer = new WorkbookDesigner();
                    workbook1 = new Aspose.Cells.Workbook(errorFilePath);
                    designer.Workbook = workbook1;
                    designer.SetDataSource(dsData);
                    designer.Process();
                    MemoryStream stream = new MemoryStream();
                    workbook1.Save(stream, SaveFormat.Xlsx);
                    byte[] fileBytes = stream.ToArray();
                    return await DeleteExcelSheetByName("Evaluation Warning", fileBytes);
                }
            }
            catch (Exception ex)
            {
                return null;
            }
            return new byte[0];
        }


        #endregion

        #region import xếp ca

        public async Task<byte[]> ExportTempImportShiftSort(string filePath, DataSet dtDataValue, long? periodId)
        {
            var QueryData = new SqlQueryDataTemplate(_dbContext);
            var periodObj = _dbContext.AtSalaryPeriods.FirstOrDefault(x => x.ID == periodId);
            if (periodObj != null)
            {
                DataTable dtvariable = new DataTable();
                dtvariable.Columns.Add("D1", typeof(DateTime));
                dtvariable.Columns.Add("D2", typeof(DateTime));
                dtvariable.Columns.Add("D3", typeof(DateTime));
                dtvariable.Columns.Add("D4", typeof(DateTime));
                dtvariable.Columns.Add("D5", typeof(DateTime));
                dtvariable.Columns.Add("D6", typeof(DateTime));
                dtvariable.Columns.Add("D7", typeof(DateTime));
                dtvariable.Columns.Add("D8", typeof(DateTime));
                dtvariable.Columns.Add("D9", typeof(DateTime));
                dtvariable.Columns.Add("D10", typeof(DateTime));
                dtvariable.Columns.Add("D11", typeof(DateTime));
                dtvariable.Columns.Add("D12", typeof(DateTime));
                dtvariable.Columns.Add("D13", typeof(DateTime));
                dtvariable.Columns.Add("D14", typeof(DateTime));
                dtvariable.Columns.Add("D15", typeof(DateTime));
                dtvariable.Columns.Add("D16", typeof(DateTime));
                dtvariable.Columns.Add("D17", typeof(DateTime));
                dtvariable.Columns.Add("D18", typeof(DateTime));
                dtvariable.Columns.Add("D19", typeof(DateTime));
                dtvariable.Columns.Add("D20", typeof(DateTime));
                dtvariable.Columns.Add("D21", typeof(DateTime));
                dtvariable.Columns.Add("D22", typeof(DateTime));
                dtvariable.Columns.Add("D23", typeof(DateTime));
                dtvariable.Columns.Add("D24", typeof(DateTime));
                dtvariable.Columns.Add("D25", typeof(DateTime));
                dtvariable.Columns.Add("D26", typeof(DateTime));
                dtvariable.Columns.Add("D27", typeof(DateTime));
                dtvariable.Columns.Add("D28", typeof(DateTime));
                dtvariable.Columns.Add("D29", typeof(DateTime));
                dtvariable.Columns.Add("D30", typeof(DateTime));
                dtvariable.Columns.Add("D31", typeof(DateTime));
                DateTime dDay = periodObj.START_DATE.Date;
                DataRow row = dtvariable.NewRow();
                int DaySum = 0;
                while (dDay <= periodObj.END_DATE.Date)
                {
                    row["D" + dDay.Day] = dDay;
                    dDay = dDay.AddDays(1);
                    DaySum = DaySum + 1;
                }
                dtvariable.Rows.Add(row);
                dtvariable.TableName = "Variable";
                dtDataValue.Tables.Add(dtvariable);

                return await DeleteExcelSheetByName("Evaluation Warning", GetByteShiftSort(filePath, dtDataValue, dtvariable, DaySum));
            }
            else
            {
                return null;
            }
        }

        public async Task<FormatedResponse> ImportShiftSort(string base64, long? PeriodID)
        {
            try
            {

                DataSet dsDataPre = new DataSet();
                base64 = base64.Replace("data:application/octet-stream;base64,", "");
                Aspose.Cells.Workbook workbook = ConvertBase64ToWorkbook(base64);
                Aspose.Cells.Worksheet worksheet;

                worksheet = workbook.Worksheets[0];
                dsDataPre.Tables.Add(worksheet.Cells.ExportDataTableAsString(4, 0, worksheet.Cells.MaxRow + 1, worksheet.Cells.MaxColumn + 1, true));

                DataTable dtData = dsDataPre.Tables[0];

                var QueryData = new SqlQueryDataTemplate(_dbContext);
                var periodObj = _dbContext.AtSalaryPeriods.FirstOrDefault(x => x.ID == PeriodID);
                var dtManual = await _dbContext.AtShifts.Where(x => x.IS_ACTIVE == true).ToListAsync();
                foreach (DataRow row in dtData.Rows)
                {
                    if (row["EMPLOYEE_ID"].ToString() != "")
                    {
                        var objParam = new
                        {
                            P_EMPLOYEEID = row["EMPLOYEE_ID"].ToString(),
                            P_ORG_ID = row["ORG_ID"].ToString(),
                            P_PERIODId = PeriodID,
                            P_USERNAME = _dbContext.UserName,
                            P_D1 = (from p in dtManual where p.CODE.ToUpper() == row["D1"].ToString().ToUpper() select p.ID).FirstOrDefault(),
                            P_D2 = (from p in dtManual where p.CODE.ToUpper() == row["D2"].ToString().ToUpper() select p.ID).FirstOrDefault(),
                            P_D3 = (from p in dtManual where p.CODE.ToUpper() == row["D3"].ToString().ToUpper() select p.ID).FirstOrDefault(),
                            P_D4 = (from p in dtManual where p.CODE.ToUpper() == row["D4"].ToString().ToUpper() select p.ID).FirstOrDefault(),
                            P_D5 = (from p in dtManual where p.CODE.ToUpper() == row["D5"].ToString().ToUpper() select p.ID).FirstOrDefault(),
                            P_D6 = (from p in dtManual where p.CODE.ToUpper() == row["D6"].ToString().ToUpper() select p.ID).FirstOrDefault(),
                            P_D7 = (from p in dtManual where p.CODE.ToUpper() == row["D7"].ToString().ToUpper() select p.ID).FirstOrDefault(),
                            P_D8 = (from p in dtManual where p.CODE.ToUpper() == row["D8"].ToString().ToUpper() select p.ID).FirstOrDefault(),
                            P_D9 = (from p in dtManual where p.CODE.ToUpper() == row["D9"].ToString().ToUpper() select p.ID).FirstOrDefault(),
                            P_D10 = (from p in dtManual where p.CODE.ToUpper() == row["D10"].ToString().ToUpper() select p.ID).FirstOrDefault(),
                            P_D11 = (from p in dtManual where p.CODE.ToUpper() == row["D11"].ToString().ToUpper() select p.ID).FirstOrDefault(),
                            P_D12 = (from p in dtManual where p.CODE.ToUpper() == row["D12"].ToString().ToUpper() select p.ID).FirstOrDefault(),
                            P_D13 = (from p in dtManual where p.CODE.ToUpper() == row["D13"].ToString().ToUpper() select p.ID).FirstOrDefault(),
                            P_D14 = (from p in dtManual where p.CODE.ToUpper() == row["D14"].ToString().ToUpper() select p.ID).FirstOrDefault(),
                            P_D15 = (from p in dtManual where p.CODE.ToUpper() == row["D15"].ToString().ToUpper() select p.ID).FirstOrDefault(),
                            P_D16 = (from p in dtManual where p.CODE.ToUpper() == row["D16"].ToString().ToUpper() select p.ID).FirstOrDefault(),
                            P_D17 = (from p in dtManual where p.CODE.ToUpper() == row["D17"].ToString().ToUpper() select p.ID).FirstOrDefault(),
                            P_D18 = (from p in dtManual where p.CODE.ToUpper() == row["D18"].ToString().ToUpper() select p.ID).FirstOrDefault(),
                            P_D19 = (from p in dtManual where p.CODE.ToUpper() == row["D19"].ToString().ToUpper() select p.ID).FirstOrDefault(),
                            P_D20 = (from p in dtManual where p.CODE.ToUpper() == row["D20"].ToString().ToUpper() select p.ID).FirstOrDefault(),
                            P_D21 = (from p in dtManual where p.CODE.ToUpper() == row["D21"].ToString().ToUpper() select p.ID).FirstOrDefault(),
                            P_D22 = (from p in dtManual where p.CODE.ToUpper() == row["D22"].ToString().ToUpper() select p.ID).FirstOrDefault(),
                            P_D23 = (from p in dtManual where p.CODE.ToUpper() == row["D23"].ToString().ToUpper() select p.ID).FirstOrDefault(),
                            P_D24 = (from p in dtManual where p.CODE.ToUpper() == row["D24"].ToString().ToUpper() select p.ID).FirstOrDefault(),
                            P_D25 = (from p in dtManual where p.CODE.ToUpper() == row["D25"].ToString().ToUpper() select p.ID).FirstOrDefault(),
                            P_D26 = (from p in dtManual where p.CODE.ToUpper() == row["D26"].ToString().ToUpper() select p.ID).FirstOrDefault(),
                            P_D27 = (from p in dtManual where p.CODE.ToUpper() == row["D27"].ToString().ToUpper() select p.ID).FirstOrDefault(),
                            P_D28 = (from p in dtManual where p.CODE.ToUpper() == row["D28"].ToString().ToUpper() select p.ID).FirstOrDefault(),
                            P_D29 = (from p in dtManual where p.CODE.ToUpper() == row["D29"].ToString().ToUpper() select p.ID).FirstOrDefault(),
                            P_D30 = (from p in dtManual where p.CODE.ToUpper() == row["D30"].ToString().ToUpper() select p.ID).FirstOrDefault(),
                            P_D31 = (from p in dtManual where p.CODE.ToUpper() == row["D31"].ToString().ToUpper() select p.ID).FirstOrDefault()
                        };
                        await QueryData.ExecuteNonQuery("PKG_INSERT_WORKSIGN_DATE", objParam, false);
                    }

                }
                //PKG 2
                var objParam1 = new
                {
                    P_STARTDATE = periodObj.START_DATE,
                    P_ENDDATE = periodObj.END_DATE,
                    P_USERNAME = _dbContext.UserName
                };

                await QueryData.ExecuteNonQuery("PKG_IMPORT_WORKSIGN_DATE", objParam1, false);

                return new() { MessageCode = CommonMessageCode.IMPORT_SHIFT_SUCCESS, ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode200 };
            }
            catch (Exception ex)
            {
                return new() { MessageCode = ex.Message, ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400 };

            }

        }

        public byte[] GetByteShiftSort(string filePath, DataSet dtData, DataTable dtVariable, int DaySum)
        {
            WorkbookDesigner designer;
            Aspose.Cells.Workbook workbook;
            designer = new WorkbookDesigner();
            workbook = new Aspose.Cells.Workbook(filePath);
            designer.Workbook = workbook;
            designer.SetDataSource(dtData);
            if (dtVariable != null)
            {
                int intCols = dtVariable.Columns.Count;
                for (int i = 0; i < intCols; i++)
                {
                    designer.SetDataSource(dtVariable.Columns[i].ColumnName.ToString(), dtVariable.Rows[0].ItemArray[i].ToString());
                }
            }
            designer.Process();
            designer.Workbook.CalculateFormula();
            if (DaySum == 28)
            {
                designer.Workbook.Worksheets[0].Cells.HideColumn(37);
                designer.Workbook.Worksheets[0].Cells.HideColumn(36);
                designer.Workbook.Worksheets[0].Cells.HideColumn(35);
            }
            else if (DaySum == 29)
            {
                designer.Workbook.Worksheets[0].Cells.HideColumn(37);
                designer.Workbook.Worksheets[0].Cells.HideColumn(36);
            }
            else if (DaySum == 30)
            {
                designer.Workbook.Worksheets[0].Cells.HideColumn(37);
            }

            MemoryStream stream = new MemoryStream();
            designer.Workbook.Save(stream, SaveFormat.Xlsx);
            byte[] fileBytes = stream.ToArray();
            return fileBytes;
        }

        public async Task<byte[]> CheckValidImportShiftSort(string errorFilePath, string base64, long? PeriodID)
        {
            try
            {
                DataSet dsDataPre = new DataSet();
                base64 = base64.Replace("data:application/octet-stream;base64,", "");
                Aspose.Cells.Workbook workbook = ConvertBase64ToWorkbook(base64);
                Aspose.Cells.Worksheet worksheet;
                worksheet = workbook.Worksheets[0];
                dsDataPre.Tables.Add(worksheet.Cells.ExportDataTableAsString(4, 0, worksheet.Cells.MaxRow + 1, worksheet.Cells.MaxColumn + 1, true));
                DataTable dtData = dsDataPre.Tables[0];


                for (int rowIndex = dtData.Rows.Count - 1; rowIndex >= 0; rowIndex--)
                {
                    DataRow row = dtData.Rows[rowIndex];
                    bool isRowEmpty = true;
                    foreach (var item in row.ItemArray)
                    {
                        if (!string.IsNullOrEmpty(item?.ToString()))
                        {
                            isRowEmpty = false;
                            break;
                        }
                    }
                    if (isRowEmpty)
                    {
                        dtData.Rows.RemoveAt(rowIndex);
                    }
                }
                DataTable dtError = new DataTable("ERROR");
                var dtDatas = await _dbContext.AtSalaryPeriods.FirstOrDefaultAsync(x => x.ID == PeriodID);
                DataRow rowError;
                bool isError = false;
                string sError = string.Empty;
                DataTable dtDataImportEmployee;
                DateTime startTime = DateTime.UtcNow;
                int irow = 6;

                dtDataImportEmployee = dtData.Clone();
                dtError = dtData.Clone();
                dtError.Columns.Add("STT");
                var dtManual = await _dbContext.AtShifts.Where(x => x.IS_ACTIVE == true).ToListAsync();
                foreach (DataRow row in dtData.Rows)
                {
                    isError = false;
                    rowError = dtError.NewRow();
                    sError = "Chưa nhập mã nhân viên";
                    if (row["EMPLOYEE_CODE"].ToString() == "")
                    {
                        rowError["EMPLOYEE_CODE"] = sError;
                        isError = true;
                    }
                    for (int index = 1; index <= (dtDatas.END_DATE - dtDatas.START_DATE).TotalDays + 1; index++)
                    {
                        if (row["D" + index].ToString() != "")
                        {
                            var r = row["D" + index].ToString();
                            var exists = (from p in dtManual where p.CODE.ToUpper() == r.ToUpper() select p).FirstOrDefault();
                            if (exists == null)
                            {
                                rowError["D" + index] = row["D" + index].ToString() + " không tồn tại";
                                isError = true;
                            }
                            else
                            {
                                row["D" + index] = exists.ID;
                            }
                        }
                    }
                    if (isError)
                    {
                        rowError["STT"] = irow;
                        if (rowError["EMPLOYEE_CODE"].ToString() == "")
                        {
                            rowError["EMPLOYEE_CODE"] = row["EMPLOYEE_CODE"].ToString();
                        }
                        rowError["VN_FULLNAME"] = row["VN_FULLNAME"].ToString();
                        rowError["ORG_NAME"] = row["ORG_NAME"].ToString();
                        rowError["TITLE_NAME"] = row["TITLE_NAME"].ToString();
                        dtError.Rows.Add(rowError);
                    }
                    irow += 1;
                }
                if (dtError.Rows.Count > 0)
                {
                    dtError.TableName = "DATA";
                    DataSet dsData = new DataSet();
                    dsData.Tables.Add(dtError);
                    WorkbookDesigner designer;
                    Aspose.Cells.Workbook workbook1;
                    designer = new WorkbookDesigner();
                    workbook1 = new Aspose.Cells.Workbook(errorFilePath);
                    designer.Workbook = workbook1;
                    designer.SetDataSource(dsData);
                    designer.Process();
                    MemoryStream stream = new MemoryStream();
                    workbook1.Save(stream, SaveFormat.Xlsx);
                    byte[] fileBytes = stream.ToArray();
                    return await DeleteExcelSheetByName("Evaluation Warning", fileBytes);
                }
            }
            catch (Exception ex)
            {
                return new byte[0];
            }
            return null;
        }

        #endregion

        #region import điều chỉnh thâm niên , phép
        public async Task<byte[]> CheckValidImportDeclareSeniority(string errorFilePath, string base64)
        {
            try
            {
                DataSet dsDataPre = new DataSet();
                base64 = base64.Replace("data:application/octet-stream;base64,", "");
                Aspose.Cells.Workbook workbook = ConvertBase64ToWorkbook(base64);
                Aspose.Cells.Worksheet worksheet;
                worksheet = workbook.Worksheets[0];
                dsDataPre.Tables.Add(worksheet.Cells.ExportDataTableAsString(3, 0, worksheet.Cells.MaxRow + 1, worksheet.Cells.MaxColumn + 1, true));
                DataTable dtData = dsDataPre.Tables[0];

                for (int rowIndex = dtData.Rows.Count - 1; rowIndex >= 0; rowIndex--)
                {
                    DataRow row = dtData.Rows[rowIndex];
                    bool isRowEmpty = true;
                    foreach (var item in row.ItemArray)
                    {
                        if (!string.IsNullOrEmpty(item?.ToString()))
                        {
                            isRowEmpty = false;
                            break;
                        }
                    }
                    if (isRowEmpty)
                    {
                        dtData.Rows.RemoveAt(rowIndex);
                    }
                }
                DataTable dtError = new DataTable("ERROR");

                DataRow rowError;
                bool isError = false;
                string sError = string.Empty;
                DataTable dtDataImportEmployee;
                DateTime startTime = DateTime.UtcNow;
                int irow = 6;

                dtDataImportEmployee = dtData.Clone();
                dtError = dtData.Clone();
                dtError.Columns.Add("STT");
                var dtManual = await _dbContext.AtTimeTypes.Where(x => x.IS_ACTIVE == true).ToListAsync();
                foreach (DataRow row in dtData.Rows)
                {
                    isError = false;
                    rowError = dtError.NewRow();
                    sError = "Chưa nhập mã nhân viên";
                    if (row["EMPLOYEE_CODE"].ToString() == "")
                    {
                        rowError["EMPLOYEE_CODE"] = sError;
                        isError = true;
                    }

                    sError = "Nhập sai định dạng năm";
                    if (row["YEAR_DECLARE"].ToString() != "")
                    {
                        try
                        {
                            decimal.Parse(row["YEAR_DECLARE"].ToString());
                        }
                        catch
                        {
                            rowError["YEAR_DECLARE"] = sError;
                            isError = true;
                        }
                    }

                    sError = "Nhập sai định dạng tháng";
                    if (row["MONTH_ADJUST"].ToString() != "")
                    {
                        try
                        {
                            decimal.Parse(row["MONTH_ADJUST"].ToString());
                        }
                        catch
                        {
                            rowError["MONTH_ADJUST"] = sError;
                            isError = true;
                        }
                    }

                    sError = "Nhập sai định dạng số";
                    if (row["MONTH_ADJUST_NUMBER"].ToString() != "")
                    {
                        try
                        {
                            decimal.Parse(row["MONTH_ADJUST_NUMBER"].ToString());
                        }
                        catch
                        {
                            rowError["MONTH_ADJUST_NUMBER"] = sError;
                            isError = true;
                        }
                    }

                    sError = "Nhập sai định dạng tháng";
                    if (row["MONTH_DAY_OFF"].ToString() != "")
                    {
                        try
                        {
                            decimal.Parse(row["MONTH_DAY_OFF"].ToString());
                        }
                        catch
                        {
                            rowError["MONTH_DAY_OFF"] = sError;
                            isError = true;
                        }
                    }

                    sError = "Nhập sai định dạng số";
                    if (row["NUMBER_DAY_OFF"].ToString() != "")
                    {
                        try
                        {
                            decimal.Parse(row["NUMBER_DAY_OFF"].ToString());
                        }
                        catch
                        {
                            rowError["NUMBER_DAY_OFF"] = sError;
                            isError = true;
                        }
                    }

                    sError = "Nhập sai định dạng số";
                    if (row["TOTAL"].ToString() != "")
                    {
                        try
                        {
                            decimal.Parse(row["TOTAL"].ToString());
                        }
                        catch
                        {
                            rowError["TOTAL"] = sError;
                            isError = true;
                        }
                    }

                    if (isError)
                    {
                        rowError["STT"] = irow;
                        if (rowError["EMPLOYEE_CODE"].ToString() == "")
                        {
                            rowError["EMPLOYEE_CODE"] = row["EMPLOYEE_CODE"].ToString();
                        }
                        rowError["EMPLOYEE_NAME"] = row["EMPLOYEE_NAME"].ToString();
                        rowError["ORG_NAME"] = row["ORG_NAME"].ToString();
                        rowError["POSITION_NAME"] = row["POSITION_NAME"].ToString();
                        dtError.Rows.Add(rowError);
                    }
                    irow += 1;
                }
                if (dtError.Rows.Count > 0)
                {
                    dtError.TableName = "DATA";
                    DataSet dsData = new DataSet();
                    dsData.Tables.Add(dtError);
                    WorkbookDesigner designer;
                    Aspose.Cells.Workbook workbook1;
                    designer = new WorkbookDesigner();
                    workbook1 = new Aspose.Cells.Workbook(errorFilePath);
                    designer.Workbook = workbook1;
                    designer.SetDataSource(dsData);
                    designer.Process();
                    MemoryStream stream = new MemoryStream();
                    workbook1.Save(stream, SaveFormat.Xlsx);
                    byte[] fileBytes = stream.ToArray();
                    return await DeleteExcelSheetByName("Evaluation Warning", fileBytes);
                }
            }
            catch (Exception ex)
            {
                return new byte[0];
            }
            return null;
        }
        public async Task<FormatedResponse> ImportDeclareSeniority(string base64)
        {
            try
            {

                DataSet dsDataPre = new DataSet();
                base64 = base64.Replace("data:application/octet-stream;base64,", "");
                Aspose.Cells.Workbook workbook = ConvertBase64ToWorkbook(base64);
                Aspose.Cells.Worksheet worksheet;

                worksheet = workbook.Worksheets[0];
                dsDataPre.Tables.Add(worksheet.Cells.ExportDataTableAsString(3, 0, worksheet.Cells.MaxRow + 1, worksheet.Cells.MaxColumn + 1, true));

                DataTable dtData = dsDataPre.Tables[0];
                foreach (DataRow row in dtData.Rows)
                {
                    AT_DECLARE_SENIORITY objParam = new AT_DECLARE_SENIORITY();
                    if (row["EMPLOYEE_ID"].ToString() != "")
                    {
                        objParam.EMPLOYEE_ID = long.Parse(row["EMPLOYEE_ID"].ToString());
                    }
                    if (row["YEAR_DECLARE"].ToString() != "")
                    {
                        objParam.YEAR_DECLARE = long.Parse(row["YEAR_DECLARE"].ToString());
                    }
                    if (row["MONTH_ADJUST"].ToString() != "")
                    {
                        objParam.MONTH_ADJUST = long.Parse(row["MONTH_ADJUST"].ToString());
                    }
                    if (row["MONTH_ADJUST_NUMBER"].ToString() != "")
                    {
                        objParam.MONTH_ADJUST_NUMBER = long.Parse(row["MONTH_ADJUST_NUMBER"].ToString());
                    }

                    objParam.REASON_ADJUST = row["REASON_ADJUST"].ToString();

                    if (row["MONTH_DAY_OFF"].ToString() != "")
                    {
                        objParam.MONTH_DAY_OFF = long.Parse(row["MONTH_DAY_OFF"].ToString());
                    }
                    if (row["NUMBER_DAY_OFF"].ToString() != "")
                    {
                        objParam.NUMBER_DAY_OFF = long.Parse(row["NUMBER_DAY_OFF"].ToString());
                    }

                    objParam.REASON_ADJUST_DAY_OFF = row["REASON_ADJUST_DAY_OFF"].ToString();

                    if (row["TOTAL"].ToString() != "")
                    {
                        objParam.TOTAL = long.Parse(row["TOTAL"].ToString());
                    }
                    objParam.CREATED_DATE = DateTime.Now;
                    objParam.CREATED_BY = _dbContext.UserName;

                    if (objParam.EMPLOYEE_ID != 0)
                    {
                        await _dbContext.AtDeclareSenioritys.AddAsync(objParam);

                    }
                }
                _dbContext.SaveChanges();
                return new() { MessageCode = CommonMessageCode.IMPORT_DECLARE_SENIORITY_SUCCESS, ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode200 };
            }
            catch (Exception ex)
            {
                return new() { MessageCode = ex.Message, ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400 };

            }

        }
        #endregion

        #region import đăng ký nghỉ
        public async Task<FormatedResponse> ImportRegisterLeave(string base64)
        {
            try
            {
                string format = "dd/MM/yyyy";
                DataSet dsDataPre = new DataSet();
                base64 = base64.Replace("data:application/octet-stream;base64,", "");
                Aspose.Cells.Workbook workbook = ConvertBase64ToWorkbook(base64);
                Aspose.Cells.Worksheet worksheet;

                worksheet = workbook.Worksheets[0];
                dsDataPre.Tables.Add(worksheet.Cells.ExportDataTableAsString(6, 0, worksheet.Cells.MaxRow + 1, worksheet.Cells.MaxColumn + 1, true));


                DataTable dtData = dsDataPre.Tables[0];

                for (int rowIndex = dtData.Rows.Count - 1; rowIndex >= 0; rowIndex--)
                {
                    DataRow row = dtData.Rows[rowIndex];
                    bool isRowEmpty = true;
                    foreach (var item in row.ItemArray)
                    {
                        if (!string.IsNullOrEmpty(item?.ToString()))
                        {
                            isRowEmpty = false;
                            break;
                        }
                    }
                    if (isRowEmpty)
                    {
                        dtData.Rows.RemoveAt(rowIndex);
                    }
                }

                foreach (DataRow row in dtData.Rows)
                {
                    var data = new AT_REGISTER_LEAVE();
                    data.TYPE_OFF = long.Parse(row["LEAVE_TYPE"].ToString());
                    data.TYPE_ID = long.Parse(row["MANUAL_ID"].ToString());
                    data.DATE_END = DateTime.ParseExact(row["LEAVE_TO"].ToString().Trim(), format, CultureInfo.InvariantCulture);
                    data.DATE_START = DateTime.ParseExact(row["LEAVE_FROM"].ToString().Trim(), format, CultureInfo.InvariantCulture);
                    data.NOTE = row["NOTE"].ToString();
                    data.REASON = row["REASON"].ToString();
                    data.EMPLOYEE_ID = long.Parse(row["EMPLOYEE_ID"].ToString());
                    data.CREATED_BY = _dbContext.UserName;
                    data.CREATED_DATE = DateTime.Now;
                    data.UPDATED_BY = _dbContext.UserName;
                    data.UPDATED_DATE = DateTime.Now;

                    await _dbContext.AtRegisterLeaves.AddAsync(data);
                    await _dbContext.SaveChangesAsync();


                    var obj = _dbContext.AtRegisterLeaves.Where(x => x.EMPLOYEE_ID == data.EMPLOYEE_ID && x.DATE_START == data.DATE_START && x.DATE_END == data.DATE_END && x.TYPE_OFF == data.TYPE_OFF && x.TYPE_ID == data.TYPE_ID).FirstOrDefault();

                    if (obj != null)
                    {

                        try
                        {
                            for (int i = 0; i <= (obj.DATE_END - obj.DATE_START).Value.Days; i++)
                            {
                                var dataDetail = new AT_REGISTER_LEAVE_DETAIL();
                                dataDetail.REGISTER_ID = obj.ID;
                                dataDetail.LEAVE_DATE = obj.DATE_START.Value.AddDays(i);
                                dataDetail.NUMBER_DAY = 1;
                                dataDetail.MANUAL_ID = obj.TYPE_ID;
                                dataDetail.TYPE_OFF = obj.TYPE_OFF;
                                await _dbContext.AtRegisterLeaveDetails.AddAsync(dataDetail);
                            }
                        }
                        catch
                        {
                            _dbContext.AtRegisterLeaves.Remove(obj);
                        }

                    }
                }



                _dbContext.SaveChanges();
                return new() { MessageCode = CommonMessageCode.IMPORT_REGISTER_LEAVE_SUCCESS, ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode200 };
            }
            catch (Exception ex)
            {
                return new() { MessageCode = ex.Message, ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400 };

            }

        }
        public async Task<byte[]> CheckValidImportRegisterLeave(string errorFilePath, string base64)
        {
            try
            {
                string format = "dd/MM/yyyy";
                var QueryData = new SqlQueryDataTemplate(_dbContext);
                DataSet dsDataPre = new DataSet();
                base64 = base64.Replace("data:application/octet-stream;base64,", "");
                Aspose.Cells.Workbook workbook = ConvertBase64ToWorkbook(base64);
                Aspose.Cells.Worksheet worksheet;
                worksheet = workbook.Worksheets[0];
                dsDataPre.Tables.Add(worksheet.Cells.ExportDataTableAsString(6, 0, worksheet.Cells.MaxRow + 1, worksheet.Cells.MaxColumn + 1, true));
                DataTable dtData = dsDataPre.Tables[0];
                for (int rowIndex = dtData.Rows.Count - 1; rowIndex >= 0; rowIndex--)
                {
                    DataRow row = dtData.Rows[rowIndex];
                    bool isRowEmpty = true;
                    foreach (var item in row.ItemArray)
                    {
                        if (!string.IsNullOrEmpty(item?.ToString()))
                        {
                            isRowEmpty = false;
                            break;
                        }
                    }
                    if (isRowEmpty)
                    {
                        dtData.Rows.RemoveAt(rowIndex);
                    }
                }
                DataTable dtError = new DataTable("ERROR");
                DataTable dtDataImportEmployee = new DataTable();
                DateTime startTime = DateTime.UtcNow;
                DataRow rowError;
                bool isError = false;
                string sError = string.Empty;
                DataTable dtEmpID;
                bool is_Validate = false;
                dtData.TableName = "DATA";
                dtDataImportEmployee = dtData.Clone();
                dtError = dtData.Clone();
                dtError.Columns.Add("STT", typeof(int));
                DataTable dtDataUserPHEPNAM = new DataTable();
                DataTable dtDataUserPHEPBU = new DataTable();
                DataTable totalDayRes = new DataTable();
                decimal totalDayWanApp = 0;
                int irow = 8;
                int irowEm = 8;
                bool isValid = true;

                foreach (DataRow row in dtData.Rows)
                {
                    rowError = dtError.NewRow();
                    sError = "Chưa nhập mã nhân viên";
                    if (row["EMPLOYEE_CODE"].ToString() == "")
                    {
                        rowError["EMPLOYEE_CODE"] = sError;
                        isError = true;
                    }
                    sError = "Nghỉ từ ngày không được để trống";
                    if (row["LEAVE_FROM"].ToString().Trim() == "")
                    {
                        rowError["LEAVE_FROM"] = sError;
                        isError = true;
                    }
                    sError = "Nghỉ đến ngày không được để trống";
                    if (row["LEAVE_TO"].ToString().Trim() == "")
                    {
                        rowError["LEAVE_TO"] = sError;
                        isError = true;
                    }
                    sError = "Loại nghỉ không được để trống";
                    if (row["MANUAL_ID"].ToString() == "#N/A" || row["MANUAL_ID"].ToString() == "")
                    {
                        rowError["MANUAL_NAME"] = sError;
                        isError = true;
                    }
                    sError = "Mã kiểu nghỉ không được để trống";
                    if (row["LEAVE_TYPE"].ToString() == "#N/A" || row["LEAVE_TYPE"].ToString() == "")
                    {
                        rowError["LEAVE_TYPE_NAME"] = sError;
                        isError = true;
                    }

                    sError = "Ngày sai định dạng";
                    if (row["LEAVE_FROM"].ToString().Trim() != "")
                    {
                        try
                        {
                            DateTime.ParseExact(row["LEAVE_FROM"].ToString().Trim(), format, CultureInfo.InvariantCulture);
                        }
                        catch
                        {
                            rowError["LEAVE_FROM"] = sError;
                            isError = true;
                            is_Validate = true;
                        }

                    }
                    if (row["LEAVE_TO"].ToString().Trim() != "")
                    {
                        try
                        {
                            DateTime.ParseExact(row["LEAVE_TO"].ToString().Trim(), format, CultureInfo.InvariantCulture);
                        }
                        catch
                        {
                            rowError["LEAVE_TO"] = sError;
                            isError = true;
                            is_Validate = true;
                        }
                    }
                    if (rowError["LEAVE_FROM"].ToString() == "" &&
                        rowError["LEAVE_TO"].ToString() == "" &&
                        row["LEAVE_FROM"].ToString().Trim() != "" &&
                        row["LEAVE_TO"].ToString().Trim() != "")
                    {
                        if (!is_Validate)
                        {
                            DateTime startdate = DateTime.ParseExact(row["LEAVE_FROM"].ToString().Trim(), format, CultureInfo.InvariantCulture);
                            DateTime enddate = DateTime.ParseExact(row["LEAVE_TO"].ToString().Trim(), format, CultureInfo.InvariantCulture);
                            if (startdate > enddate)
                            {
                                rowError["LEAVE_FROM"] = "Nghỉ từ ngày phải nhỏ hơn nghỉ tới ngày";
                                isError = true;
                            }
                        }

                    }
                    if (row["LEAVE_FROM"].ToString().Trim() != "" && row["LEAVE_TO"].ToString().Trim() != "" && row["EMPLOYEE_ID"].ToString() != "")
                    {
                        if (!is_Validate)
                        {
                            var checkList = _dbContext.AtRegisterLeaves.Where(x => x.EMPLOYEE_ID == long.Parse(row["EMPLOYEE_ID"].ToString())).ToList();
                            foreach (var itemCheck in checkList)
                            {
                                if (DateTime.ParseExact(row["LEAVE_FROM"].ToString().Trim(), format, CultureInfo.InvariantCulture) <= DateTime.ParseExact(row["LEAVE_TO"].ToString().Trim(), format, CultureInfo.InvariantCulture) && DateTime.ParseExact(row["LEAVE_TO"].ToString().Trim(), format, CultureInfo.InvariantCulture).Date < itemCheck.DATE_START.Value.Date)
                                {
                                    isValid = true;
                                }
                                else if (DateTime.ParseExact(row["LEAVE_FROM"].ToString().Trim(), format, CultureInfo.InvariantCulture) <= DateTime.ParseExact(row["LEAVE_TO"].ToString().Trim(), format, CultureInfo.InvariantCulture) && DateTime.ParseExact(row["LEAVE_FROM"].ToString().Trim(), format, CultureInfo.InvariantCulture).Date > itemCheck.DATE_END.Value.Date)
                                {
                                    isValid = true;
                                }
                                else
                                {
                                    isValid = false;
                                    break;
                                }
                            }
                            if (isValid == false)
                            {
                                rowError["OTHER_ERROR"] = "Đã tồn tại đăng ký nghỉ trong khoảng thời gian này";
                                isError = true;
                                isValid = true;
                            }

                            var ValidateCheck = await QueryData.ExecuteList("CHECK_VALIDATE_REGISTER_LEAVE",
                             new
                             {
                                 P_EMPLOYEEID = long.Parse(row["EMPLOYEE_ID"].ToString()),
                                 P_LEAVEFROM = row["LEAVE_FROM"].ToString().Trim(),
                                 P_LEAVETO = row["LEAVE_TO"].ToString().Trim(),
                                 P_MANUALID = long.Parse(row["MANUAL_ID"].ToString()),
                                 P_LEAVEID = 0,
                                 P_LEAVE_TYPE = long.Parse(row["LEAVE_TYPE"].ToString()),
                                 P_DAYNUM = 0,
                             }, false);
                            if (ValidateCheck != null)
                            {
                                var ListValidates = ValidateCheck.Select(c => (string?)((dynamic)c).MESSAGECODE).FirstOrDefault();
                                if (ListValidates != null && ListValidates != "")
                                {
                                    rowError["OTHER_ERROR2"] = ListValidates;
                                    isError = true;
                                }

                            }
                        }
                    }
                    if (isError)
                    {
                        rowError["STT"] = irow;
                        if (rowError["EMPLOYEE_CODE"].ToString() == "")
                        {
                            rowError["EMPLOYEE_CODE"] = row["EMPLOYEE_CODE"].ToString();
                        }
                        rowError["VN_FULLNAME"] = row["VN_FULLNAME"].ToString();
                        rowError["ORG_NAME"] = row["ORG_NAME"].ToString();
                        rowError["TITLE_NAME"] = row["TITLE_NAME"].ToString();
                        dtError.Rows.Add(rowError);
                    }
                    else
                    {
                        dtDataImportEmployee.ImportRow(row);
                    }
                    irow = irow + 1;
                    isError = false;
                    is_Validate = false;
                }
                if (dtError.Rows.Count > 0)
                {
                    dtError.TableName = "DATA";
                    DataSet dsData = new DataSet();
                    dsData.Tables.Add(dtError);
                    WorkbookDesigner designer;
                    Aspose.Cells.Workbook workbook1;
                    designer = new WorkbookDesigner();
                    workbook1 = new Aspose.Cells.Workbook(errorFilePath);
                    designer.Workbook = workbook1;
                    designer.SetDataSource(dsData);
                    designer.Process();
                    MemoryStream stream = new MemoryStream();
                    workbook1.Save(stream, SaveFormat.Xlsx);
                    byte[] fileBytes = stream.ToArray();
                    return await DeleteExcelSheetByName("Evaluation Warning", fileBytes);
                }
            }
            catch (Exception ex)
            {
                return new byte[0];
            }
            return null;
        }
        #endregion
         
        #region import đăng ký OT
        public async Task<byte[]> CheckValidImportRegisterOT(string errorFilePath, string base64)
        {
            try
            {
                string format = "dd/MM/yyyy";
                var QueryData = new SqlQueryDataTemplate(_dbContext);
                DataSet dsDataPre = new DataSet();
                base64 = base64.Replace("data:application/octet-stream;base64,", "");
                Aspose.Cells.Workbook workbook = ConvertBase64ToWorkbook(base64);
                Aspose.Cells.Worksheet worksheet;
                worksheet = workbook.Worksheets[0];
                dsDataPre.Tables.Add(worksheet.Cells.ExportDataTableAsString(4, 0, worksheet.Cells.MaxRow + 1, worksheet.Cells.MaxColumn + 1, true));
                DataTable dtData = dsDataPre.Tables[0];

                for (int rowIndex = dtData.Rows.Count - 1; rowIndex >= 0; rowIndex--)
                {
                    DataRow row = dtData.Rows[rowIndex];
                    bool isRowEmpty = true;
                    foreach (var item in row.ItemArray)
                    {
                        if (!string.IsNullOrEmpty(item?.ToString()))
                        {
                            isRowEmpty = false;
                            break;
                        }
                    }
                    if (isRowEmpty)
                    {
                        dtData.Rows.RemoveAt(rowIndex);
                    }
                }

                DataTable dtError = new DataTable("ERROR");
                DataTable dtDataImportEmployee = new DataTable();
                DateTime startTime = DateTime.UtcNow;
                DataRow rowError;
                bool isValidDate = false;
                bool isError = false;
                string pattern = @"^([01][0-9]|2[0-3]):[0-5][0-9]$";
                string sError = string.Empty;
                dtData.TableName = "DATA";
                dtDataImportEmployee = dtData.Clone();
                dtError = dtData.Clone();
                dtError.Columns.Add("STT", typeof(int));
                dtError.Columns.Add("ERROR", typeof(string));
                dtError.Columns.Add("ERROR1", typeof(string));
                dtError.Columns.Add("ERROR2", typeof(string));
                dtError.Columns.Add("ERROR3", typeof(string));
                dtError.Columns.Add("ERROR4", typeof(string));
                dtError.Columns.Add("ERROR5", typeof(string));
                dtError.Columns.Add("ERROR6", typeof(string));
                dtError.Columns.Add("ERROR7", typeof(string));
                dtError.Columns.Add("ERROR8", typeof(string));
                DataTable dtDataUserPHEPNAM = new DataTable();
                DataTable dtDataUserPHEPBU = new DataTable();
                DataTable totalDayRes = new DataTable();
                int irow = 6;

                foreach (DataRow row in dtData.Rows)
                {
                    rowError = dtError.NewRow();

                    HashSet<DataRow> duplicateRows = new HashSet<DataRow>();

                    if (!duplicateRows.Contains(row))
                    {
                        duplicateRows.Add(row);
                    }
                    else
                    {
                        rowError["ERROR6"] = "Hàng dữ liệu bị trùng lặp";
                        isError = true;
                    }


                    sError = "Chưa nhập mã nhân viên";
                    if (row["EMPLOYEE_CODE"].ToString() == "")
                    {
                        rowError["EMPLOYEE_CODE"] = sError;
                        isError = true;
                    }
                    sError = "Từ ngày không được để trống";
                    if (row["FROM_DATE"].ToString().Trim() == "")
                    {
                        rowError["FROM_DATE"] = sError;
                        isError = true;
                    }
                    else
                    {
                        try
                        {
                            DateTime.ParseExact(row["FROM_DATE"].ToString().Trim(), format, CultureInfo.InvariantCulture);
                        }
                        catch
                        {
                            rowError["FROM_DATE"] = "Ngày sai định dạng";
                            isError = true;
                            isValidDate = true;
                        }
                    }
                    sError = "Đến ngày không được để trống";
                    if (row["TO_DATE"].ToString().Trim() == "")
                    {
                        rowError["TO_DATE"] = sError;
                        isError = true;
                    }
                    else
                    {
                        try
                        {
                            DateTime.ParseExact(row["TO_DATE"].ToString().Trim(), format, CultureInfo.InvariantCulture);
                        }
                        catch
                        {
                            rowError["TO_DATE"] = "Ngày sai định dạng";
                            isError = true;
                            isValidDate = true;
                        }
                    }

                    if (row["FROM_DATE"].ToString().Trim() != "" && row["TO_DATE"].ToString().Trim() != "")
                    {
                        try
                        {
                            if (DateTime.ParseExact(row["TO_DATE"].ToString().Trim(), format, CultureInfo.InvariantCulture) < DateTime.ParseExact(row["FROM_DATE"].ToString().Trim(), format, CultureInfo.InvariantCulture))
                            {
                                rowError["TO_DATE"] = "Từ ngày phải nhỏ hơn đến ngày";
                                isError = true;
                            }
                        }
                        catch
                        {
                            rowError["TO_DATE"] = "Ngày sai định dạng";
                            rowError["FROM_DATE"] = "Ngày sai định dạng";
                            isError = true;
                            isValidDate = true;
                        }
                    }


                    sError = "Từ giờ không được để trống";
                    if (row["FROM_HOUR"].ToString() == "")
                    {
                        rowError["FROM_HOUR"] = sError;
                        isError = true;
                    }
                    else
                    {
                        if (!Regex.IsMatch(row["FROM_HOUR"].ToString().Trim(), pattern))
                        {
                            rowError["FROM_HOUR"] = "Giờ sai định dạng";
                            isError = true;
                            isValidDate = true;
                        }
                    }
                    sError = "Đến giờ không được để trống";
                    if (row["TO_HOUR"].ToString() == "")
                    {
                        rowError["TO_HOUR"] = sError;
                        isError = true;
                        isValidDate = true;
                    }
                    else
                    {
                        if (!Regex.IsMatch(row["TO_HOUR"].ToString().Trim(), pattern))
                        {
                            rowError["TO_HOUR"] = "Giờ sai định dạng";
                            isError = true;
                            isValidDate = true;
                        }
                    }
                    if (rowError["FROM_DATE"].ToString() == "" &&
                        rowError["TO_DATE"].ToString() == "" &&
                        row["FROM_DATE"].ToString().Trim() != "" &&
                        row["TO_DATE"].ToString().Trim() != "")
                    {
                        if (!isValidDate)
                        {
                            DateTime startdate = DateTime.ParseExact(row["FROM_DATE"].ToString().Trim(), format, CultureInfo.InvariantCulture);
                            DateTime enddate = DateTime.ParseExact(row["TO_DATE"].ToString().Trim(), format, CultureInfo.InvariantCulture);
                            if (startdate > enddate)
                            {
                                rowError["FROM_DATE"] = "Nghỉ từ ngày phải nhỏ hơn nghỉ tới ngày";
                                isError = true;
                            }
                        }

                    }
                    if (row["FROM_DATE"].ToString().Trim() != "" && row["TO_DATE"].ToString().Trim() != "" && row["EMPLOYEE_ID"].ToString() != "" && row["FROM_HOUR"].ToString() != "" && row["TO_HOUR"].ToString() != "")
                    {

                        var a = _dbContext.AtWorksigns.AsNoTracking().Where(x => x.EMPLOYEE_ID == long.Parse(row["EMPLOYEE_ID"].ToString())).ToList();
                        if (a.Count == 0)
                        {
                            rowError["ERROR1"] = "Nhân viên chưa được xếp ca";
                            isError = true;
                        }
                        else
                        {
                            if (!isValidDate)
                            {
                                DateTime dateStart1 = DateTime.ParseExact(row["FROM_DATE"].ToString().Trim(), format, CultureInfo.InvariantCulture);
                                DateTime dateEnd1 = DateTime.ParseExact(row["TO_DATE"].ToString().Trim(), format, CultureInfo.InvariantCulture);
                                var dateT = (from t in _dbContext.AtWorksigns.Where(x => x.EMPLOYEE_ID == long.Parse(row["EMPLOYEE_ID"].ToString())) orderby t.WORKINGDAY descending select t.WORKINGDAY).ToList().First();//lay den ngay
                                var dateF = (from w in _dbContext.AtWorksigns.Where(x => x.EMPLOYEE_ID == long.Parse(row["EMPLOYEE_ID"].ToString())) orderby w.WORKINGDAY descending select w.WORKINGDAY).ToList().Last();//lay tu ngay
                                if (DateTime.ParseExact(row["FROM_DATE"].ToString().Trim(), format, CultureInfo.InvariantCulture) < dateF)
                                {
                                    rowError["FROM_DATE"] = "Từ ngày phải lớn hơn hoặc bằng ngày xếp ca làm việc";
                                    isError = true;
                                }
                                if (DateTime.ParseExact(row["TO_DATE"].ToString().Trim(), format, CultureInfo.InvariantCulture) > dateT)
                                {
                                    rowError["TO_DATE"] = "Đến ngày phải nhỏ hơn hoặc bằng ngày xếp ca làm việc";
                                    isError = true;
                                }
                                long? checkperiod = 0;
                                checkperiod = a.First().PERIOD_ID;
                                var o = (from t in _dbContext.HuEmployees.Where(x => x.ID == long.Parse(row["EMPLOYEE_ID"].ToString())) orderby t.ID descending select t.ORG_ID).ToList().FirstOrDefault();
                                var period = await _dbContext.AtOrgPeriods.Where(x => x.PERIOD_ID == checkperiod && x.STATUSCOLEX == 1 && x.ORG_ID == o).AnyAsync();
                                if (period)
                                {
                                    rowError["ERROR2"] = "Kỳ công đã ngừng áp dụng";
                                    isError = true;
                                }


                                long? shiftID = 0;
                                a.ForEach(p =>
                                {
                                    if (p.WORKINGDAY!.Value.ToShortDateString() == DateTime.ParseExact(row["FROM_DATE"].ToString().Trim(), format, CultureInfo.InvariantCulture).ToShortDateString())
                                    {
                                        shiftID = p.SHIFT_ID;
                                    }
                                });

                                var shift = _dbContext.AtShifts.AsNoTracking().Where(p => p.ID == shiftID).Any();// Check ton tai ca
                                TimeSpan timeStart = new TimeSpan(int.Parse(row["FROM_HOUR"].ToString().Trim().Substring(0, 2)), int.Parse(row["FROM_HOUR"].ToString().Trim().Substring(3, 2)), 0);
                                TimeSpan timeStop = new TimeSpan(int.Parse(row["TO_HOUR"].ToString().Trim().Substring(0, 2)), int.Parse(row["TO_HOUR"].ToString().Trim().Substring(3, 2)), 0);
                                if (!shift)
                                {
                                    rowError["ERROR3"] = "Ca làm việc không tồn tại";
                                    isError = true;
                                }
                                else
                                {
                                    var shiftTimeStop = (from s in _dbContext.AtShifts.AsNoTracking().Where(p => p.ID == shiftID)
                                                         select s.HOURS_STOP).First();// lay dc gio ket thuc ca lm vc

                                    var shiftE = _dbContext.AtShifts.AsNoTracking().Where(p => p.ID == shiftID).FirstOrDefault();
                                    if (shiftE.CODE != "T7" && shiftE.CODE != "CN" && shiftE.CODE != "OFF")
                                    {
                                        if (shiftTimeStop != null)
                                        {
                                            if (timeStart < timeStop)
                                            {
                                                rowError["FROM_HOUR"] = "Giờ bắt đầu làm thêm phải lơn hơn giờ kết thúc ca làm việc";
                                                isError = true;
                                            }
                                        }
                                    }
                                }







                                //check dang ky nhieu ngay
                                TimeSpan range = dateEnd1 - dateStart1;

                                List<DateTime> days = new List<DateTime>();

                                for (int i = 0; i <= range.Days; i++)
                                {
                                    days.Add((dateStart1).AddDays(i));
                                }
                                foreach (var datetime in days)
                                {
                                    var dateEndOneDay = new DateTime(datetime.Year, datetime.Month, datetime.Day, int.Parse(row["TO_HOUR"].ToString().Trim().Substring(0, 2)), int.Parse(row["TO_HOUR"].ToString().Trim().Substring(3, 2)), 0);
                                    if (timeStart > timeStop)
                                    {
                                        dateEndOneDay.AddDays(1);
                                    }

                                    long? shiftIdOneDay = 0;
                                    a.ForEach(p =>
                                    {
                                        if (p.WORKINGDAY!.Value.ToShortDateString() == dateEndOneDay.ToShortDateString())
                                        {
                                            shiftIdOneDay = p.SHIFT_ID;
                                        }
                                    });

                                    var shiftTimeStopOneDay = (from s in _dbContext.AtShifts.AsNoTracking().Where(p => p.ID == shiftIdOneDay)
                                                               select s).First();
                                    TimeSpan timeStartOneDay = new TimeSpan(shiftTimeStopOneDay.HOURS_START.Hour, shiftTimeStopOneDay.HOURS_START.Minute, shiftTimeStopOneDay.HOURS_START.Second);
                                    TimeSpan timeStopOneDay = new TimeSpan(shiftTimeStopOneDay.HOURS_STOP.Hour, shiftTimeStopOneDay.HOURS_STOP.Minute, shiftTimeStopOneDay.HOURS_STOP.Second);
                                    //check gio ket thuc trung ca moi
                                    var timeStopCheck = new TimeSpan(dateEndOneDay.Hour, dateEndOneDay.Minute, dateEndOneDay.Second);
                                    if (shiftTimeStopOneDay.CODE != "T7" && shiftTimeStopOneDay.CODE != "CN" && shiftTimeStopOneDay.CODE != "OFF")
                                    {
                                        if (timeStartOneDay <= timeStopCheck && timeStopCheck <= timeStopOneDay)
                                        {

                                            rowError["FROM_HOUR"] = "Giờ bắt đầu làm thêm phải lơn hơn giờ kết thúc ca làm việc";
                                            isError = true;
                                        }
                                    }

                                }
                            }
                        }
                        //check tong gio lam them cua nv trong thang
                        if (!isValidDate)
                        {
                            DateTime dateStart1 = DateTime.ParseExact(row["FROM_DATE"].ToString().Trim(), format, CultureInfo.InvariantCulture);
                            DateTime dateEnd1 = DateTime.ParseExact(row["TO_DATE"].ToString().Trim(), format, CultureInfo.InvariantCulture);
                            var h1 = _dbContext.AtOvertimes.AsNoTracking().Where(x => x.EMPLOYEE_ID == long.Parse(row["EMPLOYEE_ID"].ToString())).ToList();
                            int ms = DateTime.ParseExact(row["FROM_DATE"].ToString().Trim(), format, CultureInfo.InvariantCulture).Month;
                            int me = DateTime.ParseExact(row["TO_DATE"].ToString().Trim(), format, CultureInfo.InvariantCulture).Month;
                            int ys = DateTime.ParseExact(row["FROM_DATE"].ToString().Trim(), format, CultureInfo.InvariantCulture).Year;
                            int ye = DateTime.ParseExact(row["TO_DATE"].ToString().Trim(), format, CultureInfo.InvariantCulture).Year;
                            float toltalH = 0;
                            float toltalHY = 0;
                            foreach (var hour in h1)//so gio lam them da dang ky
                            {
                                //lay ban ghi cung thang, nam
                                int m1 = hour.START_DATE!.Value.Month;
                                int y1 = hour.START_DATE!.Value.Year;
                                int m2 = hour.END_DATE!.Value.Month;
                                int y2 = hour.END_DATE!.Value.Year;
                                if (ms == m1 && me == m2 && ys == y1 && ye == y2)
                                {
                                    //so gio lam them 1 ca/day
                                    DateTime timeS = hour.TIME_START.Value;
                                    DateTime timeE = hour.TIME_END.Value;
                                    TimeSpan numTimeS = new TimeSpan(timeS.Hour, timeS.Minute, timeS.Second);
                                    TimeSpan numTimeE = new TimeSpan(timeE.Hour, timeE.Minute, timeE.Second);
                                    TimeSpan numHour = (numTimeE - numTimeS);
                                    float toltalHour = (float)numHour.TotalHours;

                                    //so gio lam them thang
                                    DateTime dateE = hour.END_DATE!.Value.Date;
                                    DateTime dateS = hour.START_DATE!.Value.Date;
                                    TimeSpan day = (dateE - dateS);
                                    float numDays = (float)day.TotalDays + 1;
                                    //if (numDays == 0) numDays = 1;
                                    toltalH += (float)(toltalHour * numDays);
                                }
                                if (ys == y1 && ye == y2)
                                {
                                    //so gio lam them 1 ca/day
                                    DateTime timeS = hour.TIME_START.Value;
                                    DateTime timeE = hour.TIME_END.Value;
                                    TimeSpan numTimeS = new TimeSpan(timeS.Hour, timeS.Minute, timeS.Second);
                                    TimeSpan numTimeE = new TimeSpan(timeE.Hour, timeE.Minute, timeE.Second);
                                    TimeSpan numHour = (numTimeE - numTimeS);
                                    float toltalHourY = (float)numHour.TotalHours;

                                    //so gio lam them thang
                                    DateTime dateE = hour.END_DATE!.Value.Date;
                                    DateTime dateS = hour.START_DATE!.Value.Date;
                                    TimeSpan day = (dateE - dateS);
                                    float numDays = (float)day.TotalDays + 1;
                                    //if (numDays == 0) numDays = 1;
                                    toltalHY += (float)(toltalHourY * numDays);
                                }
                            }
                            //so gio lam them dky
                            TimeSpan snumH = new TimeSpan(int.Parse(row["FROM_HOUR"].ToString().Trim().Substring(0, 2)), int.Parse(row["FROM_HOUR"].ToString().Trim().Substring(3, 2)), 0);
                            TimeSpan enumH = new TimeSpan(int.Parse(row["TO_HOUR"].ToString().Trim().Substring(0, 2)), int.Parse(row["TO_HOUR"].ToString().Trim().Substring(3, 2)), 0);
                            int sc = dateStart1.DayOfYear;
                            TimeSpan numH = (enumH - snumH);
                            int ec = dateEnd1.DayOfYear;
                            float numDayDky = (ec - sc) + 1; float numHourDky = (float)numH.TotalHours;
                            toltalH += (numHourDky * numDayDky);
                            toltalHY += (numHourDky * numDayDky);
                            var hourM = (from p in _dbContext.AtOtherLists.OrderByDescending(x => x.ID).Where(x => x.IS_ACTIVE == true) select p.MAX_WORKING_MONTH).First();//tong gio lam them thang
                            var hourY = (from p in _dbContext.AtOtherLists.OrderByDescending(x => x.ID).Where(x => x.IS_ACTIVE == true) select p.MAX_WORKING_YEAR).First();//tong gio lam them nam
                            if (toltalH > hourM)
                            {
                                rowError["ERROR4"] = " Nhân viên quá số giờ làm thêm quy định trong tháng";
                                isError = true;
                            }
                            if (toltalHY > hourY)
                            {
                                rowError["ERROR5"] = " Nhân viên quá số giờ làm thêm quy định trong năm";
                                isError = true;
                            }
                        }

                        if (!isValidDate)
                        {
                            DataRow[] duplicateRowsArray = duplicateRows.ToArray();

                            DateTime checkStart = DateTime.ParseExact(duplicateRowsArray[duplicateRowsArray.Length - 1]["FROM_DATE"].ToString().Trim(), format, CultureInfo.InvariantCulture);
                            DateTime checkEnd = DateTime.ParseExact(duplicateRowsArray[duplicateRowsArray.Length - 1]["TO_DATE"].ToString().Trim(), format, CultureInfo.InvariantCulture);

                            for (int i = 0; i < duplicateRowsArray.Length - 1; i++)
                            {
                                DateTime dateStart = DateTime.ParseExact(duplicateRowsArray[i]["FROM_DATE"].ToString().Trim(), format, CultureInfo.InvariantCulture);
                                DateTime dateEnd = DateTime.ParseExact(duplicateRowsArray[i]["TO_DATE"].ToString().Trim(), format, CultureInfo.InvariantCulture);

                                if ((checkStart >= dateStart && checkStart <= dateEnd) || (checkEnd <= dateEnd && checkEnd >= dateStart))
                                {
                                    rowError["ERROR8"] = "Trùng khoảng thời gian đăng ký";
                                    isError = true;
                                }

                            }

                        }

                        if (!isValidDate)
                        {
                            bool isValid = true;

                            var checkList = _dbContext.AtOvertimes.Where(x => x.EMPLOYEE_ID == long.Parse(row["EMPLOYEE_ID"].ToString())).ToList();
                            foreach (var itemCheck in checkList)
                            {
                                if (DateTime.ParseExact(row["FROM_DATE"].ToString().Trim(), format, CultureInfo.InvariantCulture) <= DateTime.ParseExact(row["TO_DATE"].ToString().Trim(), format, CultureInfo.InvariantCulture) && DateTime.ParseExact(row["TO_DATE"].ToString().Trim(), format, CultureInfo.InvariantCulture).Date < itemCheck.START_DATE.Value.Date)
                                {
                                    isValid = true;
                                }
                                else if (DateTime.ParseExact(row["FROM_DATE"].ToString().Trim(), format, CultureInfo.InvariantCulture) <= DateTime.ParseExact(row["TO_DATE"].ToString().Trim(), format, CultureInfo.InvariantCulture) && DateTime.ParseExact(row["FROM_DATE"].ToString().Trim(), format, CultureInfo.InvariantCulture).Date > itemCheck.END_DATE.Value.Date)
                                {
                                    isValid = true;
                                }
                                else
                                {
                                    isValid = false;
                                    break;
                                }
                            }
                            if (isValid == false)
                            {
                                rowError["ERROR7"] = "Đã tồn tại đăng ký nghỉ trong khoảng thời gian này";
                                isError = true;
                                isValid = true;
                            }
                        }

                    }



                    if (isError)
                    {
                        rowError["STT"] = irow;
                        if (rowError["EMPLOYEE_CODE"].ToString() == "")
                        {
                            rowError["EMPLOYEE_CODE"] = row["EMPLOYEE_CODE"].ToString();
                        }
                        rowError["VN_FULLNAME"] = row["VN_FULLNAME"].ToString();
                        rowError["ORG_NAME"] = row["ORG_NAME"].ToString();
                        rowError["TITLE_NAME"] = row["TITLE_NAME"].ToString();

                        dtError.Rows.Add(rowError);
                    }
                    else
                    {
                        dtDataImportEmployee.ImportRow(row);
                    }
                    irow = irow + 1;
                    isError = false;
                    isValidDate = false;
                }
                if (dtError.Rows.Count > 0)
                {
                    dtError.TableName = "DATA";
                    DataSet dsData = new DataSet();
                    dsData.Tables.Add(dtError);
                    WorkbookDesigner designer;
                    Aspose.Cells.Workbook workbook1;
                    designer = new WorkbookDesigner();
                    workbook1 = new Aspose.Cells.Workbook(errorFilePath);
                    designer.Workbook = workbook1;
                    designer.SetDataSource(dsData);
                    designer.Process();
                    MemoryStream stream = new MemoryStream();
                    workbook1.Save(stream, SaveFormat.Xlsx);
                    byte[] fileBytes = stream.ToArray();
                    return await DeleteExcelSheetByName("Evaluation Warning", fileBytes);
                }
            }
            catch
            {
                return new byte[0];
            }
            return null;
        }
        public async Task<FormatedResponse> ImportRegisterOT(string base64)
        {
            try
            {
                string format = "dd/MM/yyyy";
                string formatHM = "dd/MM/yyyy HH:mm";
                DataSet dsDataPre = new DataSet();
                base64 = base64.Replace("data:application/octet-stream;base64,", "");
                Aspose.Cells.Workbook workbook = ConvertBase64ToWorkbook(base64);
                Aspose.Cells.Worksheet worksheet;

                worksheet = workbook.Worksheets[0];
                dsDataPre.Tables.Add(worksheet.Cells.ExportDataTableAsString(4, 0, worksheet.Cells.MaxRow + 1, worksheet.Cells.MaxColumn + 1, true));


                DataTable dtData = dsDataPre.Tables[0];

                for (int rowIndex = dtData.Rows.Count - 1; rowIndex >= 0; rowIndex--)
                {
                    DataRow row = dtData.Rows[rowIndex];
                    bool isRowEmpty = true;
                    foreach (var item in row.ItemArray)
                    {
                        if (!string.IsNullOrEmpty(item?.ToString()))
                        {
                            isRowEmpty = false;
                            break;
                        }
                    }
                    if (isRowEmpty)
                    {
                        dtData.Rows.RemoveAt(rowIndex);
                    }
                }
                foreach (DataRow row in dtData.Rows)
                {
                    var data = new AT_OVERTIME();
                    data.TIME_START = DateTime.ParseExact(row["FROM_DATE"].ToString().Trim().Trim() + " " + row["FROM_HOUR"].ToString().Trim(), formatHM, CultureInfo.InvariantCulture);
                    data.TIME_END = DateTime.ParseExact(row["FROM_DATE"].ToString().Trim().Trim() + " " + row["TO_HOUR"].ToString().Trim(), formatHM, CultureInfo.InvariantCulture);
                    data.NOTE = row["NOTE"].ToString();
                    data.START_DATE = DateTime.ParseExact(row["FROM_DATE"].ToString().Trim(), format, CultureInfo.InvariantCulture);
                    data.END_DATE = DateTime.ParseExact(row["TO_DATE"].ToString().Trim(), format, CultureInfo.InvariantCulture);
                    data.REASON = row["REASON"].ToString();
                    data.EMPLOYEE_ID = long.Parse(row["EMPLOYEE_ID"].ToString());
                    data.CREATED_BY = _dbContext.UserName;
                    data.CREATED_DATE = DateTime.Now;
                    data.UPDATED_BY = _dbContext.UserName;
                    data.UPDATED_DATE = DateTime.Now;
                    await _dbContext.AtOvertimes.AddAsync(data);
                }
                _dbContext.SaveChanges();
                return new() { MessageCode = CommonMessageCode.IMPORT_REGISTER_LEAVE_SUCCESS, ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode200 };
            }
            catch (Exception ex)
            {
                return new() { MessageCode = ex.Message, ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400 };

            }

        }
        #endregion

    }
}
