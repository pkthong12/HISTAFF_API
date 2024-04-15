using Common.Paging;
using PayrollDAL.ViewModels;
using Common.Repositories;
using Common.Extensions;
using System.Data;
using Common.EPPlus;
using System.Dynamic;
using API.All.DbContexts;
using API.Entities;

namespace PayrollDAL.Repositories
{
    public class SalaryImportRepository : RepositoryBase<PA_SAL_IMPORT>, ISalaryImportRepository
    {
        private PayrollDbContext _appContext => (PayrollDbContext)_context;
        public SalaryImportRepository(PayrollDbContext context) : base(context)
        {

        }

        public async Task<ResultWithError> ExportTemplate(SalImpSearchParam param)
        {
            var ds = QueryData.ExecuteStoreToTable(Procedures.PKG_IMPORT_SAL_IMPORT_LIST,
                new
                {
                    P_IS_ADMIN = _appContext.IsAdmin == true ? 1 : 0,
                    P_ORG_ID = param.OrgId,
                    P_TYPE_SAL = param.SalTypeId,
                    P_PERIOD_ID = param.PeriodId,
                    P_CUR_HEAD = QueryData.OUT_CURSOR,
                    P_CUR_DETAIL = QueryData.OUT_CURSOR
                }, true);

            if (ds.Tables[0].Rows.Count <= 0)
            {
                return new ResultWithError("DATA_EMPTY");
            }
            ds.Tables[0].TableName = "head";
            ds.Tables[1].TableName = "detail";
            var pathTemp = _appContext._config["urlTempSalImp"];
            var memoryStream = Template.FillColumn(pathTemp, ds);
            return new ResultWithError(memoryStream);
        }

        /// <summary>
        /// Import Data
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<ResultWithError> ImportTemplate(SalImpImportParam param)
        {
            try
            {
                var json = JsonConvert.SerializeObject(param.Data);
                DataTable dt = (DataTable)JsonConvert.DeserializeObject(json, (typeof(DataTable)));
                dt.Rows[0].Delete();
                if (dt.Rows.Count == 0)
                {
                    return new ResultWithError(404);
                }
                var lst = new List<PA_SAL_IMPORT>();
                string empIdTmp = "";
                for (int i = 6; i < dt.Columns.Count; i++)
                {
                    string[] sId = dt.Columns[i].ColumnName.ToString().Trim().Split("-");
                    int input = int.Parse(sId[1]);
                    long elementId = long.Parse(sId[0]);
                    for (int j = 0; j < dt.Rows.Count; j++)
                    {
                        var data = new PA_SAL_IMPORT();
                        if (dt.Rows[j][i].ToString().Length > 0)
                        {  // Kiêm tra mã nhân viên
                            long empId = long.Parse(dt.Rows[j][1].ToString().Trim());
                            data.EMPLOYEE_ID = empId;
                            empIdTmp += "," + empId.ToString();
                            //data.ELEMENT_ID = elementId;
                            //data.PERIOD_ID = param.PeriodId;
                            //data.TYPE_SAL = param.SalTypeId;
                            //// Kiêm tra số tiền
                            //if (input == 0)
                            //{
                            //    data.MONEY = decimal.Parse(dt.Rows[j][i].ToString());
                            //}
                            //else
                            //{
                            //    data.MONEY1 = dt.Rows[j][i].ToString().Trim();
                            //}

                            lst.Add(data);

                        }
                    }
                }
                if (lst.Count > 0)
                {
                    var ds = QueryData.ExecuteNonQuery("PKG_IMPORT.SAL_IMPORT_DELETE",
                            new
                            {
                                P_TYPE_SAL = param.SalTypeId,
                                P_PERIOD_ID = param.PeriodId,
                                P_EMP_IDS = empIdTmp.Substring(1)
                            }, true);

                    await _appContext.SalaryImports.AddRangeAsync(lst);
                    await _appContext.SaveChangesAsync();
                }
                else
                {
                    return new ResultWithError(400);
                }
                return new ResultWithError(200);
            }
            catch (Exception ex)
            {
                return new ResultWithError(ex.Message);
            }
        }
        public async Task<PagedResult<ExpandoObject>> GetAll(SalImportDTO param)
        {
            try
            {
                if (param.PageNo == 0)
                {
                    param.PageNo = 1;
                }
                if (param.PageSize == 0)
                {
                    param.PageSize = 20;
                }
                return await QueryData.ExecutePaging("PKG_IMPORT.LIST_SAL_IMPORTED",
                    new
                    {
                        p_is_admin = _appContext.IsAdmin == true ? 1 : 0,
                        p_org_id = param.OrgId,
                        p_period_id = param.PeriodId,
                        P_SALARY_TYPE_ID = param.SalaryTypeId,
                        P_CODE = param.EmpCode,
                        P_NAME = param.FullName,
                        P_ORG_NAME = param.OName,
                        P_POS_NAME = param.PName,
                        p_page_no = param.PageNo,
                        p_page_size = param.PageSize,
                        P_CUR = QueryData.OUT_CURSOR,
                        P_CUR_PAGE = QueryData.OUT_CURSOR
                    }, true);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<ResultWithError> Delete(SalImportDelParam param)
        {
            try
            {
                string sEmp = "";
                foreach (var item in param.Ids)
                {
                    sEmp = sEmp + "," + item;
                }
                sEmp = sEmp.Substring(1);
                var ds = QueryData.ExecuteNonQuery("PKG_PAYROLL.REMOVE_IMPORT",
                           new
                           {
                               P_PERIOD_ID = param.PeriodId,
                               P_TYPE_ID = param.SalaryTypeId,
                               P_EMPS = sEmp
                           }, true);

                return new ResultWithError(200);
            }
            catch (Exception ex)
            {
                return new ResultWithError(ex.Message);
            }
        }
    }
}
