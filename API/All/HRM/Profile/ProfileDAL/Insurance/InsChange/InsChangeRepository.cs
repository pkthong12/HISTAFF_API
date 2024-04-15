using Common.Paging;
using ProfileDAL.ViewModels;
using Common.Repositories;
using Common.Extensions;
using Common.EPPlus;
using API.All.DbContexts;
using API.Entities;

namespace ProfileDAL.Repositories
{
    public class InsChangeRepository : RepositoryBase<INS_CHANGE>, IInsChangeRepository
    {
        private ProfileDbContext _appContext => (ProfileDbContext)_context;
        public InsChangeRepository(ProfileDbContext context) : base(context)
        {

        }
        /// <summary>
        /// CMS Get All Data
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<PagedResult<InsChangeDTO>> GetAll(InsChangeDTO param)
        {
            var queryable = from p in _appContext.InsChanges
                            from e in _appContext.Employees.Where(c => c.ID == p.EMPLOYEE_ID)
                            from o in _appContext.Organizations.Where(c => c.ID == e.ORG_ID)
                            from a in _appContext.InsuranceTypes.Where(c => c.ID == p.CHANGE_TYPE_ID).DefaultIfEmpty()
                            
                            orderby p.CHANGE_MONTH descending, e.CODE
                            select new InsChangeDTO
                            {
                                Id = p.ID,
                                EmployeeName = e.Profile.FULL_NAME,
                                EmployeeCode = e.CODE,
                                OrgId = e.ORG_ID,
                                OrgName = o.NAME,
                                ChangeTypeName = a.NAME,
                                ChangeMonth = p.CHANGE_MONTH,
                                SalaryOld = (double?)p.SALARY_OLD,
                                SalaryNew = (double?)p.SALARY_NEW,
                                WorkStatusId = e.WORK_STATUS_ID,
                                TerEffectDate = e.TER_EFFECT_DATE,
                                IsBhxh = p.IS_BHXH,
                                IsBhyt = p.IS_BHYT,
                                IsBhtn = p.IS_BHTN,
                                IsBnn = p.IS_BNN
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

            if (!string.IsNullOrWhiteSpace(param.EmployeeName))
            {
                queryable = queryable.Where(p => p.EmployeeName.ToUpper().Contains(param.EmployeeName.ToUpper()));
            }
            if (!string.IsNullOrWhiteSpace(param.EmployeeCode))
            {
                queryable = queryable.Where(p => p.EmployeeCode.ToUpper().Contains(param.EmployeeCode.ToUpper()));
            }
            if (param.ChangeMonth != null)
            {
                queryable = queryable.Where(p => p.ChangeMonth == param.ChangeMonth);
            }
            if (param.SalaryOld != null)
            {
                queryable = queryable.Where(p => p.SalaryOld == param.SalaryOld);
            }
            if (param.SalaryNew != null)
            {
                queryable = queryable.Where(p => p.SalaryNew == param.SalaryNew);
            }

            if (param.WorkStatusId == null || param.WorkStatusId == 0)
            {
                queryable = queryable.Where(p => p.WorkStatusId != OtherConfig.EMP_STATUS_TERMINATE || (p.WorkStatusId == OtherConfig.EMP_STATUS_TERMINATE && p.TerEffectDate > DateTime.Now));
            }
            return await PagingList(queryable, param);

        }
        /// <summary>
        /// CMS Get Detail
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>        
        public async Task<ResultWithError> GetById(long id)
        {
            try
            {
                var r = await (
                               from p in _appContext.InsChanges
                               join e in _appContext.Employees on p.EMPLOYEE_ID equals e.ID
                               join o in _appContext.Organizations on e.ORG_ID equals o.ID
                               join po in _appContext.Positions on e.POSITION_ID equals po.ID
                               //join a in _appContext.InsuranceTypes on p.CHANGE_TYPE_ID equals a.ID
                               where p.ID == id
                               select new
                               {
                                   Id = p.ID,
                                   EmployeeId = p.EMPLOYEE_ID,
                                   EmployeeName = e.Profile.FULL_NAME,
                                   EmployeeCode = e.CODE,
                                   PositionName = po.NAME,
                                   OrgName = o.NAME,
                                   ChangeTypeId = p.CHANGE_TYPE_ID,
                                   //ChangeTypeName = a.NAME,
                                   ChangeMonth = p.CHANGE_MONTH,
                                   SalaryOld = p.SALARY_OLD,
                                   SalaryNew = p.SALARY_NEW,
                                   Note = p.NOTE,
                                   IsBhxh = p.IS_BHXH,
                                   IsBhyt = p.IS_BHYT,
                                   IsBhtn = p.IS_BHTN,
                                   IsBnn = p.IS_BNN
                               }).FirstOrDefaultAsync();

                return new ResultWithError(r);
            }
            catch (Exception ex)
            {
                return new ResultWithError(ex.Message);
            }
        }
        /// <summary>
        /// Create Data
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<ResultWithError> CreateAsync(InsChangeInputDTO param)
        {
            try
            {
                var emp = await (_appContext.Employees.Where(c => c.ID == param.EmployeeId ).CountAsync());
                if (emp == 0)
                {
                    return new ResultWithError(Message.EMP_NOT_EXIST);
                }

                var data = Map(param, new INS_CHANGE());
                await _appContext.InsChanges.AddAsync(data);
                await _appContext.SaveChangesAsync();
                return new ResultWithError(data);
            }
            catch (Exception ex)
            {
                return new ResultWithError(ex.Message);
            }
        }

        /// <summary>
        /// CMS Edit Data
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<ResultWithError> UpdateAsync(InsChangeInputDTO param)
        {
            try
            {
                var r = await _appContext.InsChanges.Where(c => c.ID == param.Id ).FirstOrDefaultAsync();
                if (r is null)
                {
                    return new ResultWithError(Message.RECORD_NOT_FOUND);
                }

                var emp = await (_appContext.Employees.Where(c => c.ID == param.EmployeeId ).CountAsync());
                if (emp == 0)
                {
                    return new ResultWithError(Message.EMP_NOT_EXIST);
                }
                param.EmployeeId = null;
                var data = Map(param, r);

                _appContext.InsChanges.Update(data);
                await _appContext.SaveChangesAsync();
                return new ResultWithError(data);
            }
            catch (Exception ex)
            {
                return new ResultWithError(ex);
            }
        }
        /// <summary>
        /// CMS Change Status Data
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<ResultWithError> RemoveAsync(List<long> ids)
        {
            try
            {
                await _appContext.Database.BeginTransactionAsync();
                foreach (var item in ids)
                {
                    var r = await _appContext.InsChanges.Where(x => x.ID == item ).FirstOrDefaultAsync();
                    if (r == null)
                    {
                        return new ResultWithError(404);
                    }
                    _appContext.InsChanges.Remove(r);
                }
                
                await _appContext.SaveChangesAsync();
                _appContext.Database.CommitTransaction();
                return new ResultWithError(200);
            }
            catch (Exception ex)
            {
                _appContext.Database.RollbackTransaction();
                return new ResultWithError(ex.Message);
            }
        }
        public async Task<ResultWithError> TemplateImport(long orgId)
        {
            try
            {
                // xử lý fill dữ liệu vào master data
                var ds = QueryData.ExecuteStoreToTable(Procedures.PKG_IMPORT_INSURANCE_DATA_IMPORT,
                new
                {
                    P_ORG_ID = orgId,
                    P_CUR_STATUS = QueryData.OUT_CURSOR,
                    P_CUR_INSURANCE = QueryData.OUT_CURSOR,
                    P_CUR = QueryData.OUT_CURSOR
                }, true);

                if (ds.Tables[0].Rows.Count <= 0)
                {
                    return new ResultWithError("DATA_EMPTY");
                }
                ds.Tables[0].TableName = "Status";
                ds.Tables[1].TableName = "Insurance";
                ds.Tables[2].TableName = "Data";
                var pathTemp = _appContext._config["urlTempInsurance"];
                var memoryStream = Template.FillTemplate(pathTemp, ds);
                return new ResultWithError(memoryStream);
            }
            catch (Exception ex)
            {
                return new ResultWithError(ex.Message);
            }
        }
        /// <summary>
        /// Import Data
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<ResultWithError> ImportTemplate(ImportInsParam param)
        {
            try
            {
                param.Data.RemoveRange(0, 2);
                if (param.Data.Count == 0)
                {
                    return new ResultWithError(404);
                }
                var lst = new List<TMP_INS_CHANGE>();
                var guid = Guid.NewGuid().ToString();
                var error = false;
                var lstError = new List<ImportInsDtlParam>();
                foreach (var item in param.Data)
                {
                    var dtl = new TMP_INS_CHANGE();
                    if (string.IsNullOrWhiteSpace(item.TypeName))
                    {
                        error = true;
                        item.TypeName = "!Không được để trống";
                    }
                    if (string.IsNullOrWhiteSpace(item.ChangeMonth))
                    {
                        error = true;
                        item.ChangeMonth = "!Không được để trống";
                    }
                    try
                    {
                        dtl.CHANGE_MONTH = DateTime.ParseExact("01/" + item.ChangeMonth.Trim(), "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    }
                    catch
                    {
                        error = true;
                        item.ChangeMonth = "!Không đúng định dạng dd/MM/yyyy";
                    }
                    if (string.IsNullOrWhiteSpace(item.SalaryOld))
                    {
                        error = true;
                        item.SalaryOld = "!Không được để trống";
                    }
                    else
                    {
                        try
                        {
                            dtl.SALARY_OLD = decimal.Parse(item.SalaryOld);
                        }
                        catch
                        {
                            error = true;
                            item.SalaryOld = "!Sai định dạng kiểu số";
                        }
                    }
                    if (string.IsNullOrWhiteSpace(item.SalaryNew))
                    {
                        error = true;
                        item.SalaryNew = "!Không được để trống";
                    }
                    else
                    {
                        try
                        {
                            dtl.SALARY_NEW = decimal.Parse(item.SalaryNew);
                        }
                        catch
                        {
                            error = true;
                            item.SalaryNew = "!Sai định dạng kiểu số";
                        }
                    }
                    if (string.IsNullOrWhiteSpace(item.Bhxh))
                    {
                        error = true;
                        item.Bhxh = "!Không được để trống";
                    }
                    else
                    {
                        try
                        {
                            var bhxh = item.Bhxh.Split("-");
                            dtl.IS_BHXH = int.Parse(bhxh[0]);
                        }
                        catch
                        {

                            error = true;
                            item.Bhxh = "!Không đúng định dạng";
                        }
                    }
                    if (string.IsNullOrWhiteSpace(item.Bhyt))
                    {
                        error = true;
                        item.Bhyt = "!Không được để trống";
                    }
                    else
                    {
                        try
                        {
                            var bhxh = item.Bhyt.Split("-");
                            dtl.IS_BHYT = int.Parse(bhxh[0]);
                        }
                        catch
                        {

                            error = true;
                            item.Bhyt = "!Không đúng định dạng";
                        }
                    }
                    if (string.IsNullOrWhiteSpace(item.Bhtn))
                    {
                        error = true;
                        item.Bhtn = "!Không được để trống";
                    }
                    else
                    {
                        try
                        {
                            var bhxh = item.Bhtn.Split("-");
                            dtl.IS_BHTN = int.Parse(bhxh[0]);
                        }
                        catch
                        {

                            error = true;
                            item.Bhtn = "!Không đúng định dạng";
                        }
                    }
                    if (string.IsNullOrWhiteSpace(item.Bnn))
                    {
                        error = true;
                        item.Bnn = "!Không được để trống";
                    }
                    else
                    {
                        try
                        {
                            var bhxh = item.Bhtn.Split("-");
                            dtl.IS_BNN = int.Parse(bhxh[0]);
                        }
                        catch
                        {

                            error = true;
                            item.Bnn = "!Không đúng định dạng";
                        }
                    }

                    if (error)
                    {
                        error = false;
                        lstError.Add(item);
                    }
                    else
                    {
                        dtl.REF_CODE = guid;
                        dtl.CODE = item.Code.Trim();
                        dtl.TYPE_NAME = item.TypeName.Trim();
                        lst.Add(dtl);
                    }
                }

                if (lstError.Count > 0)
                {
                    var pathTemp = _appContext._config["urlTempInsurance"];
                    var memoryStream = Template.FillTemp(pathTemp, lstError);
                    return new ResultWithError(memoryStream);
                }
                else
                {
                    if (lst.Count > 0)
                    {
                        await _appContext.InsChangeTmps.AddRangeAsync(lst);
                        await _appContext.SaveChangesAsync();
                        // xử lý fill dữ liệu vào master data
                        var ds = QueryData.ExecuteStoreToTable(Procedures.PKG_IMPORT_INSURANCE_IMPORT,
                        new
                        {
                            P_REF_CODE = guid,
                            P_CUR = QueryData.OUT_CURSOR
                        }, true);

                        if (ds.Tables.Count > 0)
                        {
                            ds.Tables[0].TableName = "Data";
                            var pathTemp = _appContext._config["urlTempInsurance"];
                            var memoryStream = Template.FillTemplate(pathTemp, ds);
                            return new ResultWithError(memoryStream);
                        }
                    }
                }
                return new ResultWithError(200);
            }
            catch
            {
                return new ResultWithError(204);
            }
        }
        /// <summary>
        /// PortalGetAll
        /// </summary>
        /// <returns></returns>        
        public async Task<ResultWithError> PortalGetAll()
        {
            try
            {
                var r = await (from p in _appContext.InsChanges
                               from a in _appContext.InsuranceTypes.Where(c => c.ID == p.CHANGE_TYPE_ID).DefaultIfEmpty()
                               where p.EMPLOYEE_ID == _appContext.EmpId
                               orderby p.CHANGE_MONTH descending
                               select new
                               {
                                   Id = p.ID,
                                   ChangeTypeName = a.NAME,
                                   ChangeMonth = p.CHANGE_MONTH,
                                   SalaryOld = p.SALARY_OLD,
                                   SalaryNew = p.SALARY_NEW
                               }).ToListAsync();
                return new ResultWithError(r);
            }
            catch (Exception ex)
            {
                return new ResultWithError(ex.Message);
            }
        }

    }
}
