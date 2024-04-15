using Common.Paging;
using AttendanceDAL.ViewModels;
using Common.Repositories;
using Common.Extensions;
using Common.EPPlus;
using API.All.DbContexts;
using API.Entities;

namespace AttendanceDAL.Repositories
{
    public class EntitlementEditRepository : RepositoryBase<AT_ENTITLEMENT_EDIT>, IEntitlementEditRepository
    {
        private AttendanceDbContext _appContext => (AttendanceDbContext)_context;
        public EntitlementEditRepository(AttendanceDbContext context) : base(context)
        {

        }

        /// <summary>
        /// CMS Get All Data
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<PagedResult<EntitlementEditDTO>> GetAll(EntitlementEditDTO param)
        {
            try
            {
                var queryable = from p in _appContext.EntitlementEdits
                                join e in _appContext.Employees on p.EMPLOYEE_ID equals e.ID
                                join o in _appContext.Organizations on e.ORG_ID equals o.ID
                                join po in _appContext.Positions on e.POSITION_ID equals po.ID
                                
                                orderby p.ID descending
                                select new EntitlementEditDTO
                                {
                                    Id = p.ID,
                                    EmployeeId = p.EMPLOYEE_ID,
                                    EmployeeName = e.Profile.FULL_NAME,
                                    EmployeeCode = e.CODE,
                                    OrgId = e.ORG_ID,
                                    OrgName = o.NAME,
                                    PositionName = po.NAME,
                                    Year = p.YEAR,
                                    Month = p.MONTH,
                                    NumberChange = p.NUMBER_CHANGE,
                                    Note = p.NOTE
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

                if (!string.IsNullOrWhiteSpace(param.OrgName))
                {
                    queryable = queryable.Where(p => p.OrgName.ToUpper().Contains(param.OrgName.ToUpper()));
                }
                if (!string.IsNullOrWhiteSpace(param.PositionName))
                {
                    queryable = queryable.Where(p => p.PositionName.ToUpper().Contains(param.PositionName.ToUpper()));
                }
                if (param.Year != 0)
                {
                    queryable = queryable.Where(p => p.Year == param.Year);
                }
                if (param.Month != 0)
                {
                    queryable = queryable.Where(p => p.Month == param.Month);
                }
                return await PagingList(queryable, param);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<ResultWithError> CreateAsync(EntitlementEditInputDTO param)
        {
            try
            {
                var data = Map(param, new AT_ENTITLEMENT_EDIT());
                await _appContext.EntitlementEdits.AddAsync(data);
                await _appContext.SaveChangesAsync();
                var r = await QueryData.ExecuteObject("PKG_ENTITLEMENT.EDIT_ENTITLEMENT",
                    new
                    {
                        P_EMP_ID = param.EmployeeId,
                        P_YEAR = param.Year,
                        P_MONTH = param.Month,
                        P_NUMBER = param.NumberChange
                    }, true);
                return new ResultWithError(param);
            }
            catch (Exception ex)
            {
                return new ResultWithError(ex.Message);
            }
        }

        public async Task<ResultWithError> UpdateAsync(EntitlementEditInputDTO param)
        {
            try
            {
                var r = _appContext.EntitlementEdits.Where(c => c.ID == param.Id).FirstOrDefault();
                if (r == null)
                {
                    return new ResultWithError(Message.RECORD_NOT_FOUND);
                }

                var data = Map(param, r);
                var result = _appContext.EntitlementEdits.Update(r);
                await _appContext.SaveChangesAsync();
                await QueryData.ExecuteObject("PKG_ENTITLEMENT.EDIT_ENTITLEMENT",
                    new
                    {
                        P_EMP_ID = param.EmployeeId,
                        P_YEAR = param.Year,
                        P_MONTH = param.Month,
                        P_NUMBER = param.NumberChange
                    }, true);
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
                var r = await _appContext.EntitlementEdits.Where(x => ids.Contains(x.ID) ).ToListAsync();
                if (r.Count > 0)
                {
                    _appContext.EntitlementEdits.RemoveRange(r);
                    await _appContext.SaveChangesAsync();
                }

                return new ResultWithError(200);
            }
            catch (Exception ex)
            {
                return new ResultWithError(ex.Message);
            }
        }

        /// <summary>
        /// Portal GetBY Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ResultWithError> GetBy(long id)
        {
            try
            {
                var r = await (from p in _appContext.EntitlementEdits
                               join e in _appContext.Employees on p.EMPLOYEE_ID equals e.ID
                               join o in _appContext.Organizations on e.ORG_ID equals o.ID
                               join o1 in _appContext.Organizations on o.PARENT_ID equals o1.ID into tmp1
                               from o2 in tmp1.DefaultIfEmpty()
                               join po in _appContext.Positions on e.POSITION_ID equals po.ID
                               where p.ID == id
                               select new
                               {
                                   Id = p.ID,
                                   EmployeeId = p.EMPLOYEE_ID,
                                   EmployeeName = e.Profile.FULL_NAME,
                                   EmployeeCode = e.CODE,
                                   OrgName = o.NAME,
                                   OrgParentName = o2.NAME,
                                   PositionName = po.NAME,
                                   Year = p.YEAR,
                                   Month = p.MONTH,
                                   Note = p.NOTE,
                                   NumberChange = p.NUMBER_CHANGE
                               }).FirstOrDefaultAsync();
                return new ResultWithError(r);
            }
            catch (Exception ex)
            {
                return new ResultWithError(ex.Message);
            }
        }

        public async Task<ResultWithError> ExportTemplate(ParaOrg param)
        {
            try
            {
                var ds = QueryData.ExecuteStoreToTable(Procedures.PKG_PROFILE_LIST_EMPLOYEE,
                new
                {
                    P_IS_ADMIN = _appContext.IsAdmin == true ? 1 : 0,
                    P_ORG_ID = param.OrgId,
                    P_CUR = QueryData.OUT_CURSOR,
                }, true);

                if (ds.Tables[0].Rows.Count <= 0)
                {
                    return new ResultWithError("DATA_EMPTY");
                }
                ds.Tables[0].TableName = "data";
                var pathTemp = _appContext._config["urlTempEntitlement"];
                var memoryStream = Template.FillReportOne(pathTemp, ds);
                return new ResultWithError(memoryStream);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// API IMPORT 
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<ResultWithError> ImportTemplate(EntitlementEditParam param)
        {
            try
            {
                param.Data.RemoveRange(0, 2);
                if (param.Data.Count == 0)
                {
                    return new ResultWithError(404);
                }
                var lst = new List<AT_ENTITLEMENT_EDIT>();
                var guid = Guid.NewGuid().ToString();
                var error = false;
                var lstError = new List<EntitlementEditImp>();

                foreach (var item in param.Data)
                {
                    var dtl = new AT_ENTITLEMENT_EDIT();
                    if (string.IsNullOrWhiteSpace(item.Code))
                    {
                        error = true;
                        item.Code = "!Không được để trống";
                    }

                    if (string.IsNullOrWhiteSpace(item.Year))
                    {
                        error = true;
                        item.Year = "!Không được để trống";
                    }
                    else
                    {
                        try
                        {
                            dtl.YEAR = int.Parse(item.Year);
                        }
                        catch
                        {
                            error = true;
                            item.Year = "!Sai định dạng kiểu số";
                        }
                    }

                    if (string.IsNullOrWhiteSpace(item.Month))
                    {
                        error = true;
                        item.Month = "!Không được để trống";
                    }
                    else
                    {
                        try
                        {
                            dtl.MONTH = int.Parse(item.Month);
                        }
                        catch
                        {
                            error = true;
                            item.Month = "!Sai định dạng kiểu số";
                        }
                    }

                    if (string.IsNullOrWhiteSpace(item.NumberChange))
                    {
                        error = true;
                        item.NumberChange = "!Không được để trống";
                    }
                    else
                    {
                        try
                        {
                            dtl.NUMBER_CHANGE = decimal.Parse(item.NumberChange);
                        }
                        catch
                        {
                            error = true;
                            item.NumberChange = "!Sai định dạng kiểu số";
                        }
                    }
                    if (error)
                    {
                        error = false;
                        lstError.Add(item);
                    }
                    else
                    {
                        dtl.CODE_REF = guid;
                        dtl.CODE = item.Code;
                        dtl.NOTE = item.Note;
                        lst.Add(dtl);
                    }
                }

                if (lstError.Count > 0)
                {
                    var pathTemp = _appContext._config["urlTempEntitlement"];
                    var memoryStream = Template.FillTemp(pathTemp, lstError);
                    return new ResultWithError(memoryStream);
                }
                else
                {
                    if (lst.Count > 0)
                    {
                        await _appContext.EntitlementEdits.AddRangeAsync(lst);
                        await _appContext.SaveChangesAsync();
                        await QueryData.Execute("PKG_ENTITLEMENT.IMPORT_ENTITLEMENT",
                           new
                           {
                               P_CODE_REF = guid
                           }, true);
                    }
                }
                return new ResultWithError(200);
            }
            catch (Exception ex)
            {
                return new ResultWithError(204);
            }
        }

        /// <summary>
        /// PORTAL GET INFOR 
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<ResultWithError> PortalEntilmentGet()
        {
            try
            {
                var r = await QueryData.ExecuteStore<PortalEntiParam>("PKG_ENTITLEMENT.PORTAL_GETBY",
                new
                {
                    P_EMP_ID = _appContext.EmpId,
                    P_CUR = QueryData.OUT_CURSOR,
                }, true);
                return new ResultWithError(r);
            }
            catch (Exception ex)
            {
                return new ResultWithError(ex.Message);
            }
        }
    }
}
