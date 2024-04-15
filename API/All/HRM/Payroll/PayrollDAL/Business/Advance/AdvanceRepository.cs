using Common.Paging;
using PayrollDAL.ViewModels;
using Common.Repositories;
using Common.Extensions;
using System.Data;
using Common.EPPlus;
using API.All.DbContexts;
using API.Entities;

namespace PayrollDAL.Repositories
{
    public class AdvanceRepository : RepositoryBase<PA_ADVANCE>, IAdvanceRepository
    {
        private PayrollDbContext _appContext => (PayrollDbContext)_context;
        public AdvanceRepository(PayrollDbContext context) : base(context)
        {

        }
        /// <summary>
        /// CMS Get All Data for System
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<PagedResult<AdvanceDTO>> GetAll(AdvanceDTO param)
        {
            var queryable = from p in _appContext.Advances
                            join e in _appContext.Employees on p.EMPLOYEE_ID equals e.ID
                            join po in _appContext.Positions on e.POSITION_ID equals po.ID
                            join org in _appContext.Organizations on e.ORG_ID equals org.ID
                            join pe in _appContext.SalaryPeriods on p.PERIOD_ID equals pe.ID
                            orderby p.ADVANCE_DATE
                            select new AdvanceDTO
                            {

                                Id = p.ID,
                                EmployeeId = p.EMPLOYEE_ID,
                                EmployeeCode = e.CODE,
                                EmployeeName = e.Profile.FULL_NAME,
                                OrgId = e.ORG_ID,
                                OrgName = org.NAME,
                                Year = p.YEAR,
                                Money = p.MONEY,
                                PeriodId = p.PERIOD_ID,
                                Period = pe.NAME,
                                PositionName = po.NAME,
                                Note = p.NOTE,
                                AdvanceDate = p.ADVANCE_DATE,
                                SignerName = p.SIGNER_NAME,
                                SignerPosition = p.SIGNER_POSITION,
                                SignDate = p.SIGN_DATE,
                                StatusId = p.STATUS_ID,


                            };

            var orgIds = await QueryData.ExecuteList(Procedures.PKG_COMMON_LIST_ORG,
                    new
                    {
                        P_IS_ADMIN = _appContext.IsAdmin == true ? 1 : 0,
                        P_ORG_ID = param.OrgId,
                        P_CURENT_USER_ID = _appContext.CurrentUserId,
                        P_CUR = QueryData.OUT_CURSOR
                    }, false);


            List<long?> ids = orgIds.Select(c => (long?)((dynamic)c).ID).ToList();
            if (param.OrgId != null)
            {
                ids.Add(param.OrgId);
            }
            queryable = queryable.Where(p => ids.Contains(p.OrgId));
            if (param.PeriodId!= null)
            {
                queryable = queryable.Where(p => p.PeriodId == param.PeriodId);
            }
            if (!string.IsNullOrWhiteSpace(param.EmployeeCode))
            {
                queryable = queryable.Where(p => p.EmployeeCode.ToUpper().Contains(param.EmployeeCode.ToUpper()));
            }
            if (!string.IsNullOrWhiteSpace(param.EmployeeName))
            {
                queryable = queryable.Where(p => p.EmployeeName.ToUpper().Contains(param.EmployeeName.ToUpper()));
            }

            if (!string.IsNullOrWhiteSpace(param.PositionName))
            {
                queryable = queryable.Where(p => p.PositionName.ToUpper().Contains(param.PositionName.ToUpper()));
            }
            if (param.AdvanceDate != null)
            {
                queryable = queryable.Where(p => p.AdvanceDate == param.AdvanceDate);
            }


            return await PagingList(queryable, param);
        }
        /// <summary>
        /// CMS Get Detail for System
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>        
        public async Task<ResultWithError> GetById(long id)
        {
            try
            {
                var r = await (from p in _appContext.Advances
                               join e in _appContext.Employees on p.EMPLOYEE_ID equals e.ID
                               join o in _appContext.Organizations on e.ORG_ID equals o.ID
                               join o2 in _appContext.Organizations on o.PARENT_ID equals o2.ID into tmp1
                               from o3 in tmp1.DefaultIfEmpty()
                               join t in _appContext.Positions on e.POSITION_ID equals t.ID
                               where p.ID == id
                               select new
                               {
                                   Id = p.ID,
                                   EmployeeId = p.EMPLOYEE_ID,
                                   EmployeeName = e.Profile.FULL_NAME,
                                   EmployeeCode = e.CODE,
                                   PositionName = t.NAME,
                                   Year = p.YEAR,
                                   PeriodId = p.PERIOD_ID,
                                   OrgId = e.ORG_ID,
                                   OrgName = o.NAME,
                                   OrgParentName = o3.NAME,
                                   AdvanceDate = p.ADVANCE_DATE,
                                   Money = p.MONEY,
                                   StatusId = p.STATUS_ID,
                                   SignId = p.SIGN_ID,
                                   SignerName = p.SIGNER_NAME,
                                   SignerPosition = p.SIGNER_POSITION,
                                   SignDate = p.SIGN_DATE,
                                   Note = p.NOTE
                               }).FirstOrDefaultAsync();
                return new ResultWithError(r);
            }
            catch (Exception ex)
            {
                return new ResultWithError(ex.Message);
            }
        }
        /// <summary>
        /// Create Data for System
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<ResultWithError> CreateAsync(AdvanceInputDTO param)
        {
            try
            {
                // lấy kỳ lương
                //var pe = _appContext.SalaryPeriods.Where(p => p.ID == param.periodId).FirstOrDefault();
                //if (pe != null)
                //{
                //    if (pe.DATE_START > param.AdvanceDate || pe.DATE_END < param.AdvanceDate)
                //    {
                //        return new ResultWithError("PERIOD_LOCK");
                //    }    

                //}
                var data = Map(param, new PA_ADVANCE());
                var result = await _appContext.Advances.AddAsync(data);
                await _appContext.SaveChangesAsync();

                return new ResultWithError(data);
            }
            catch (Exception ex)
            {
                return new ResultWithError(ex.Message);
            }
        }

        /// <summary>
        /// CMS Edit Data for System
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<ResultWithError> UpdateAsync(AdvanceInputDTO param)
        {
            try
            {

                var r = _appContext.Advances.Where(x => x.ID == param.Id).FirstOrDefault();
                if (r == null)
                {
                    return new ResultWithError(404);
                }
                if (r.STATUS_ID == 2)
                {
                    return new ResultWithError(Message.RECORD_IS_APPROVED);
                }
                r.STATUS_ID = 1;
                var data = Map(param, r);
                var result = _appContext.Advances.Update(data);
                await _appContext.SaveChangesAsync();
                return new ResultWithError(200);
            }
            catch (Exception ex)
            {
                return new ResultWithError(ex);
            }
        }


        public async Task<ResultWithError> Delete(List<long> ids)
        {
            try
            {
                foreach (var item in ids)
                {
                    var r = _appContext.Advances.Where(x => x.ID == item).FirstOrDefault();
                    if (r == null)
                    {
                        return new ResultWithError(404);
                    }
                    var result = _appContext.Remove(r);
                }
                await _appContext.SaveChangesAsync();
                return new ResultWithError(200);
            }
            catch (Exception ex)
            {

                return new ResultWithError(ex.Message);
            }
        }
        public async Task<ResultWithError> TemplateImport(AdvanceTmpParam param)
        {
            try
            {
                // xử lý fill dữ liệu vào master data
                var ds = QueryData.ExecuteStoreToTable(Procedures.PKG_IMPORT_ADVANCE_DATA_IMPORT,
                new
                {
                    P_ORG_ID = param.OrgId,
                    P_PERIOD_ID  = param.PeriodId,
                    P_CUR = QueryData.OUT_CURSOR
                }, true);

                if (ds.Tables[0].Rows.Count <= 0)
                {
                    return new ResultWithError("DATA_EMPTY");
                }
                ds.Tables[0].TableName = "Data";
                var pathTemp = _appContext._config["urlTempAdvance"];
                var memoryStream = Template.FillTemplate(pathTemp, ds);
                return new ResultWithError(memoryStream);
            }
            catch (Exception ex)
            {
                return new ResultWithError(ex.Message);
            }
        }

        public async Task<ResultWithError> ImportTemplate(AdvanceTmpParam param)
        {
            try
            {

                param.Data.RemoveRange(0, 2);
                if (param.Data.Count == 0)
                {
                    return new ResultWithError(404);
                }

                var data = new List<PA_ADVANCE_TMP>();
                var guid = Guid.NewGuid().ToString();
                var lstError = new List<AdvanceTmpDtlParam>();
                var count = param.Data.Count();
                string[] codes = new string[count];
                string[] refCode = new string[count];
                decimal[] moneys = new decimal[count];
                DateTime[] advanceDates = new DateTime[count];
                string[] sigernames = new string[count];
                string[] sigerposs = new string[count];
                DateTime[] signDates = new DateTime[count];
                string[] notes = new string[count];
                string[] statusnames = new string[count];

                int i = 0;
                var error = false;
                foreach (var item in param.Data)
                {
                    var dtl = new PA_ADVANCE_TMP();
                    if (string.IsNullOrWhiteSpace(item.EmpCode))
                    {
                        error = true;
                        item.EmpCode = "!Không được để trống";
                    }
                    else
                    {
                        codes[i] = item.EmpCode;
                    }

                    if (!string.IsNullOrWhiteSpace(item.AdvanceDate))
                    {
                        try
                        {
                            advanceDates[i] = DateTime.ParseExact(item.AdvanceDate.Trim(), "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                        }
                        catch(Exception ex)
                        {
                            error = true;
                            item.AdvanceDate = "!Không được để trống";
                        }
                    }
                    else
                    {
                        error = true;
                        item.AdvanceDate = "!Không đúng định dạng dd/MM/yyyy";
                    }

                    if (!string.IsNullOrWhiteSpace(item.Money))
                    {
                        try
                        {
                            moneys[i] = decimal.Parse(item.Money);
                        }
                        catch
                        {

                            error = true;
                            item.Money = "!Sai định dạng kiểu số";
                        }
                    }
                    if (!string.IsNullOrWhiteSpace(item.SignDate))
                    {
                        try
                        {
                            signDates[i] = DateTime.ParseExact(item.SignDate.Trim(), "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                        }
                        catch
                        {
                            error = true;
                            item.SignDate = "!Không đúng định dạng dd/MM/yyyy";
                        }
                    }
                    if (string.IsNullOrWhiteSpace(item.StatusName))
                    {
                        error = true;
                        item.StatusName = "!Không được để trống";
                    }
                    else
                    {
                        statusnames[i] = item.StatusName;
                    }
                    if (error)
                    {
                        error = false;
                        lstError.Add(item);
                    }
                    else
                    {
                        refCode[i] = guid;
                        sigernames[i] = item.SignerName;
                        sigerposs[i] = item.SignerPosition;
                        notes[i] = item.Note;
                    }
                    i += 1;
                }


                if (lstError.Count > 0)
                {
                    var pathTemp = _appContext._config["urlTempAdvance"];
                    var memoryStream = Template.FillTemp(pathTemp, lstError);
                    return new ResultWithError(memoryStream);
                }
                else
                {
                    // xử lý fill dữ liệu vào master data
                    var ds = QueryData.ExecuteStoreToTable(Procedures.PKG_IMPORT_ADVANCE_IMPORT,
                    new
                    {
                        P_REF_CODE = guid,
                        P_YEAR = param.Year,
                        P_PERIOD_ID = param.PeriodId,
                        P_CUR = QueryData.OUT_CURSOR
                    }, true);

                    if (ds.Tables.Count > 0)
                    {
                        ds.Tables[0].TableName = "Data";
                        var pathTemp = _appContext._config["urlTempAdvance"];
                        var memoryStream = Template.FillTemplate(pathTemp, ds);
                        return new ResultWithError(memoryStream);
                    }
                    return new ResultWithError(200);
                }
            }
            catch (Exception ex)
            {
                return new ResultWithError(ex.Message);
            }
        }
    }
}
