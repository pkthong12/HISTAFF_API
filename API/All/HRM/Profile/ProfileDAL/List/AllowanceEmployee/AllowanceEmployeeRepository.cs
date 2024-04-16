using Common.Paging;
using ProfileDAL.ViewModels;
using Common.Repositories;
using Common.Extensions;
using API.All.DbContexts;
using API.Entities;
using CORE.DTO;
using CORE.GenericUOW;
using CORE.Enum;
using CORE.StaticConstant;
using Microsoft.AspNetCore.Http;
using API.DTO;

namespace ProfileDAL.Repositories
{
    public class AllowanceEmpRepository : RepositoryBase<HU_ALLOWANCE_EMP>, IAllowanceEmpRepository
    {
        private ProfileDbContext _appContext => (ProfileDbContext)_context;
        private readonly GenericReducer<HU_ALLOWANCE_EMP, HuAllowanceEmpDTO> genericReducer;
        public AllowanceEmpRepository(ProfileDbContext context) : base(context)
        {
            genericReducer = new();
        }
        /// <summary>
        /// CMS Get All Data
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<PagedResult<AllowanceEmpDTO>> GetAll(AllowanceEmpDTO param)
        {
            var queryable = from p in _appContext.AllowanceEmps
                            from g in _appContext.Allowances.Where(c => c.ID == p.ALLOWANCE_ID)
                            from e in _appContext.Employees.Where(x => x.ID == p.EMPLOYEE_ID)
                            from o in _appContext.Organizations.Where(x => x.ID == e.ORG_ID)
                            from t in _appContext.Positions.Where(x => x.ID == e.POSITION_ID)
                            
                            select new AllowanceEmpDTO
                            {
                                Id = p.ID,
                                EmployeeId = p.EMPLOYEE_ID,
                                EmployeeCode = e.CODE,
                                EmployeeName = e.Profile.FULL_NAME,
                                OrgId = e.ORG_ID,
                                OrgName = o.NAME,
                                PosName = t.NAME,
                                AllowanceId = p.ALLOWANCE_ID,
                                AllowanceName = g.NAME,
                                Monney = p.MONNEY,
                                DateStart = p.DATE_START,
                                DateEnd = p.DATE_END,
                                Note = p.NOTE,
                                IsActive = p.IS_ACTIVE,
                                CreateBy = p.CREATED_BY,
                                UpdatedBy = p.UPDATED_BY,
                                CreateDate = p.CREATED_DATE,
                                UpdatedDate = p.UPDATED_DATE,
                                Coefficient=p.COEFFICIENT
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
            if (!string.IsNullOrWhiteSpace(param.EmployeeCode))
            {
                queryable = queryable.Where(p => p.EmployeeCode.ToUpper().Contains(param.EmployeeCode.ToUpper()));
            }
            if (!string.IsNullOrWhiteSpace(param.EmployeeName))
            {
                queryable = queryable.Where(p => p.EmployeeName.ToUpper().Contains(param.EmployeeName.ToUpper()));
            }
            if (!string.IsNullOrWhiteSpace(param.OrgName))
            {
                queryable = queryable.Where(p => p.OrgName.ToUpper().Contains(param.OrgName.ToUpper()));
            }
            if (!string.IsNullOrWhiteSpace(param.PosName))
            {
                queryable = queryable.Where(p => p.PosName.ToUpper().Contains(param.PosName.ToUpper()));
            }
            if (param.IsActive != null)
            {
                queryable = queryable.Where(p => p.IsActive == param.IsActive);
            }
            if (param.Monney != null)
            {
                queryable = queryable.Where(p => p.Monney == param.Monney);
            }
            if (param.DateStart != null)
            {
                queryable = queryable.Where(p => p.DateStart == param.DateStart);
            }
            if (param.DateEnd != null)
            {
                queryable = queryable.Where(p => p.DateEnd == param.DateEnd);
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
                var r = await (from p in _appContext.AllowanceEmps
                               from a in _appContext.Employees.Where(c => c.ID == p.EMPLOYEE_ID)
                               join t in _appContext.Positions on a.POSITION_ID equals t.ID
                                join o in _appContext.Organizations on a.ORG_ID equals o.ID
                               where p.ID == id 
                               select new
                               {
                                   Id = p.ID,
                                   EmployeeId = p.EMPLOYEE_ID,
                                   EmployeeCode=a.CODE,
                                   EmployeeName =  a.Profile.FULL_NAME,
                                   PosName = t.NAME,
                                   OrgName = o.NAME,
                                   AllowanceId = p.ALLOWANCE_ID,
                                   Monney = p.MONNEY,
                                   Coefficient = p.COEFFICIENT,
                                   DateStart = p.DATE_START,
                                   DateEnd = p.DATE_END,
                                   Note = p.NOTE,
                                   IsActive = p.IS_ACTIVE
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
        public async Task<FormatedResponse> CreateAsync(AllowanceEmpInputDTO param)
        {
            try
            {
                foreach (var item in param.EmployeeIds!)
                {
                    var r = _appContext.AllowanceEmps.Where(x => x.EMPLOYEE_ID == item  && x.ALLOWANCE_ID == param.AllowanceId).FirstOrDefault();
                    if (r != null && (r.DATE_END is null || r.DATE_END >= DateTime.Now))
                    {
                        return new FormatedResponse{  MessageCode ="ALLOWANCE_STILL_EFFECT",
                                                    InnerBody = param,
                                                    StatusCode = EnumStatusCode.StatusCode400 };
                    }

                    var data = Map(param, new HU_ALLOWANCE_EMP());
                    data.EMPLOYEE_ID = item;
                    data.IS_ACTIVE = true;
                    var result = await _appContext.AllowanceEmps.AddAsync(data);
                }
                await _appContext.SaveChangesAsync();
                var newRecord = await _appContext.AllowanceEmps.OrderByDescending(x => x.ID).FirstOrDefaultAsync();
                return new FormatedResponse{
                    MessageCode = CommonMessageCode.CREATE_SUCCESS,
                    InnerBody = newRecord,
                    StatusCode = EnumStatusCode.StatusCode200
                };
            }
            catch (Exception ex)
            {
                 return new FormatedResponse
                {
                    MessageCode =ex.Message,
                    InnerBody = param,
                    StatusCode = EnumStatusCode.StatusCode400
                };
            }
        }
        /// <summary>
        /// CMS Edit Data
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<FormatedResponse> UpdateAsync(AllowanceEmpInputDTO param)
        {
            try
            {
                var r = _appContext.AllowanceEmps.Where(x => x.ID == param.Id ).FirstOrDefault();
                if (r == null)
                {
                     return new FormatedResponse{  MessageCode =Consts.ID_NOT_FOUND,
                                                    InnerBody = param,
                                                    StatusCode = EnumStatusCode.StatusCode404 };
                }
                var data = Map(param, r);
                var result = _appContext.AllowanceEmps.Update(data);
                await _appContext.SaveChangesAsync();
                return new FormatedResponse{
                    MessageCode = CommonMessageCode.UPDATE_SUCCESS,
                    InnerBody = param,
                    StatusCode = EnumStatusCode.StatusCode200
                };
            }
            catch (Exception ex)
            {

                return new FormatedResponse
                {
                    MessageCode =ex.Message,
                    InnerBody = param,
                    StatusCode = EnumStatusCode.StatusCode400
                };
            }
        }
        /// <summary>
        /// CMS Change Status Data
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<ResultWithError> ChangeStatusAsync(List<long> ids)
        {
            try
            {
                foreach (var item in ids)
                {
                    var r = _appContext.AllowanceEmps.Where(x => x.ID == item ).FirstOrDefault();
                    if (r == null)
                    {
                        return new ResultWithError(404);
                    }
                    r.IS_ACTIVE = !r.IS_ACTIVE;
                    var result = _appContext.AllowanceEmps.Update(r);
                }
                await _appContext.SaveChangesAsync();
                return new ResultWithError(200);
            }
            catch (Exception ex)
            {

                return new ResultWithError(ex.Message);
            }
        }

        /// <summary>
        /// CMS Change Status Data
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<ResultWithError> RemoteAsync(List<long> ids)
        {
            try
            {
                foreach (var item in ids)
                {
                    var r = _appContext.AllowanceEmps.Where(x => x.ID == item ).FirstOrDefault();
                    if (r == null)
                    {
                        return new ResultWithError(404);
                    }

                    var result = _appContext.AllowanceEmps.Remove(r);
                }
                await _appContext.SaveChangesAsync();
                return new ResultWithError(200);
            }
            catch (Exception ex)
            {
                return new ResultWithError(ex.Message);
            }
        }
        public async Task<GenericPhaseTwoListResponse<HuAllowanceEmpDTO>> SinglePhaseQueryList(GenericQueryListDTO<HuAllowanceEmpDTO> request)
        {
            var queryable = from p in _appContext.AllowanceEmps
                            from g in _appContext.Allowances.Where(c => c.ID == p.ALLOWANCE_ID).DefaultIfEmpty()
                            from e in _appContext.Employees.Where(x => x.ID == p.EMPLOYEE_ID).DefaultIfEmpty()
                            from o in _appContext.Organizations.Where(x => x.ID == e.ORG_ID).DefaultIfEmpty()
                            from t in _appContext.Positions.Where(x => x.ID == e.POSITION_ID).DefaultIfEmpty()
                            from j in _appContext.HUJobs.Where(x => x.ID == t.JOB_ID).DefaultIfEmpty()

                            select new HuAllowanceEmpDTO
                            {
                                Id = p.ID,
                                EmployeeId = p.EMPLOYEE_ID,
                                EmployeeCode = e.CODE,
                                EmployeeName = e.Profile.FULL_NAME,
                                OrgId = e.ORG_ID,
                                OrgName = o.NAME,
                                PosName = t.NAME,
                                AllowanceId = p.ALLOWANCE_ID,
                                AllowanceName = g.NAME,
                                Monney = p.MONNEY,
                                DateStart = p.DATE_START,
                                DateEnd = p.DATE_END,
                                Note = p.NOTE,
                                IsActive = p.IS_ACTIVE,
                                CreatedBy = p.CREATED_BY,
                                UpdatedBy = p.UPDATED_BY,
                                CreatedDate = p.CREATED_DATE,
                                UpdatedDate = p.UPDATED_DATE,
                                Coefficient=p.COEFFICIENT,
                                WorkStatusId=e.WORK_STATUS_ID,
                                JobOrderNum = (int)(j.ORDERNUM ?? 99)
                            };

            var singlePhaseResult = await genericReducer.SinglePhaseReduce(queryable, request);
            return singlePhaseResult;
        }
        public async Task<ResultWithError> GetList()
        {
            try
            {
                var queryable = await (from p in _appContext.Allowances
                                       where p.IS_ACTIVE == true && (p.IS_INSURANCE ==false || !p.IS_INSURANCE.HasValue)
                                       orderby p.CODE
                                       select new
                                       {
                                           Id = p.ID,
                                           Name = p.NAME,
                                           Code = p.CODE,
                                           IsSal = p.IS_SAL
                                       }).ToListAsync();
                return new ResultWithError(queryable);
            }
            catch (Exception ex)
            {
                return new ResultWithError(ex.Message);
            }
        }

    }
}
