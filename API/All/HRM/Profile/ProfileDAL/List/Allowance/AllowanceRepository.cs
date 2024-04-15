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

namespace ProfileDAL.Repositories
{
    public class AllowanceRepository : RepositoryBase<HU_ALLOWANCE>, IAllowanceRepository
    {
        private ProfileDbContext _appContext => (ProfileDbContext)_context;
        private readonly GenericReducer<HU_ALLOWANCE, AllowanceViewDTO> genericReducer;
        public AllowanceRepository(ProfileDbContext context) : base(context)
        {
            genericReducer = new();
        }
        /// <summary>
        /// CMS Get All Data
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<PagedResult<AllowanceViewDTO>> GetAll(AllowanceViewDTO param)
        {
            var queryable = from p in _appContext.Allowances
                            from g in _appContext.OtherLists.Where(c => c.ID == p.TYPE_ID).DefaultIfEmpty()

                            select new AllowanceViewDTO
                            {
                                Id = p.ID,
                                Code = p.CODE,
                                Name = p.NAME,
                                TypeId = p.TYPE_ID,
                                TypeName = g.NAME,
                                IsActive = p.IS_ACTIVE,
                                Note = p.NOTE,
                                CreateBy = p.CREATED_BY,
                                UpdatedBy = p.UPDATED_BY,
                                CreateDate = p.CREATED_DATE,
                                UpdatedDate = p.UPDATED_DATE
                            };


            if (!string.IsNullOrWhiteSpace(param.Name))
            {
                queryable = queryable.Where(p => p.Name.ToUpper().Contains(param.Name.ToUpper()));
            }
            if (!string.IsNullOrWhiteSpace(param.Code))
            {
                queryable = queryable.Where(p => p.Code.ToUpper().Contains(param.Code.ToUpper()));
            }
            if (!string.IsNullOrWhiteSpace(param.Note))
            {
                queryable = queryable.Where(p => p.Note.ToUpper().Contains(param.Note.ToUpper()));
            }
            if (param.IsActive != null)
            {
                queryable = queryable.Where(p => p.IsActive == param.IsActive);
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
                var r = await (from p in _appContext.Allowances
                               from g in _appContext.OtherLists.Where(c => c.ID == p.TYPE_ID).DefaultIfEmpty()
                               where p.ID == id
                               select new
                               {
                                   Id = p.ID,
                                   Code = p.CODE,
                                   Name = p.NAME,
                                   TypeId = p.TYPE_ID,
                                   TypeName = g.NAME,
                                   IsInsurance = p.IS_INSURANCE,
                                   IsCoefficient = p.IS_COEFFICIENT,
                                   IsSal = p.IS_SAL,
                                   IsActive = p.IS_ACTIVE,
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
        /// Create Data
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<FormatedResponse> CreateAsync(AllowanceInputDTO param)
        {
            try
            {
                var r0 = _appContext.Allowances.Where(x => x.NAME.ToUpper().Trim() == param.Name.ToUpper().Trim()).Count();
                if (r0 > 0)
                {
                    return new FormatedResponse
                    {
                        MessageCode = "ALLOWANCE_NAME_EXIST",
                        InnerBody = param,
                        ErrorType = EnumErrorType.CATCHABLE,
                        StatusCode = EnumStatusCode.StatusCode400
                    };
                }
                var r1 = _appContext.Allowances.Where(x => x.CODE == param.Code).Count();
                if (r1 > 0)
                {
                    return new FormatedResponse
                    {
                        MessageCode = Consts.CODE_EXISTS,
                        InnerBody = param,
                        ErrorType = EnumErrorType.CATCHABLE,
                        StatusCode = EnumStatusCode.StatusCode400
                    };
                }
                var r = _appContext.OtherLists.Where(x => x.ID == param.TypeId).Count();
                if (r == 0)
                {
                    return new FormatedResponse
                    {
                        MessageCode = Message.TYPE_NOT_EXIST,
                        InnerBody = param,
                        ErrorType = EnumErrorType.CATCHABLE,
                        StatusCode = EnumStatusCode.StatusCode400
                    };
                }
                var data = Map(param, new HU_ALLOWANCE());
                data.IS_ACTIVE = true;
                data.COL_NAME = data.CODE;
                var result = await _appContext.Allowances.AddAsync(data);


                // add col and element to payroll sheet sum
                //dynamic res = await QueryData.ExecuteObject(Procedures.PKG_PAYROLL_ADD_ALLOWANCE,
                //    new
                //    {

                //        P_CODE = data.CODE,
                //        P_CUR = QueryData.OUT_CURSOR,
                //    }, false);
                //if(res.STATUS == 400)
                //{
                //    return new ResultWithError(res.MESSAGE);
                //}

                await _appContext.SaveChangesAsync();


                // lấy ra bản ghi mới nhất trong db
                var all_record = _appContext.Allowances.OrderByDescending(x => x.ID).ToList();
                var new_record = all_record[0];



                param.Id = new_record.ID;


                return new FormatedResponse
                {
                    MessageCode = CommonMessageCode.CREATE_SUCCESS,
                    InnerBody = param,
                    StatusCode = EnumStatusCode.StatusCode200
                };
            }
            catch (Exception ex)
            {
                return new FormatedResponse
                {
                    MessageCode = ex.Message,
                    InnerBody = param,
                    ErrorType = EnumErrorType.CATCHABLE,
                    StatusCode = EnumStatusCode.StatusCode400
                };
            }
        }
        /// <summary>
        /// CMS Edit Data
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<FormatedResponse> UpdateAsync(AllowanceInputDTO param)
        {
            try
            {
                var r1 = _appContext.Allowances.Where(x => x.NAME.ToUpper().Trim() == param.Name.ToUpper().Trim() && x.ID != param.Id).Count();
                if (r1 > 0)
                {
                    return new FormatedResponse
                    {
                        MessageCode = "ALLOWANCE_NAME_EXIST",
                        InnerBody = param,
                        StatusCode = EnumStatusCode.StatusCode400
                    };
                }
                var type = _appContext.OtherLists.Where(x => x.ID == param.TypeId).Count();
                if (type == 0)
                {
                    return new FormatedResponse
                    {
                        MessageCode = Message.TYPE_NOT_EXIST,
                        InnerBody = param,
                        StatusCode = EnumStatusCode.StatusCode400
                    };
                }
                // check code
                var c = _appContext.Allowances.Where(x => x.CODE.ToLower() == param.Code.ToLower() && x.ID != param.Id).Count();
                if (c > 0)
                {
                    return new FormatedResponse
                    {
                        MessageCode = Consts.CODE_EXISTS,
                        InnerBody = param,
                        StatusCode = EnumStatusCode.StatusCode400
                    };
                }

                var r = _appContext.Allowances.Where(x => x.ID == param.Id).FirstOrDefault();
                if (r == null)
                {
                    return new FormatedResponse
                    {
                        MessageCode = Consts.ID_NOT_FOUND,
                        InnerBody = param,
                        StatusCode = EnumStatusCode.StatusCode400
                    };
                }
                // check code cua phu cap, phan tu luong dã dung lap cong thuc chua

                var data = Map(param, r);
                var result = _appContext.Allowances.Update(data);
                await _appContext.SaveChangesAsync();
                return new FormatedResponse
                {
                    MessageCode = CommonMessageCode.UPDATE_SUCCESS,
                    InnerBody = param,
                    StatusCode = EnumStatusCode.StatusCode200
                };
            }
            catch (Exception ex)
            {

                return new FormatedResponse
                {
                    MessageCode = ex.Message,
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
                    var r = _appContext.Allowances.Where(x => x.ID == item).FirstOrDefault();
                    if (r == null)
                    {
                        return new ResultWithError(404);
                    }
                    r.IS_ACTIVE = !r.IS_ACTIVE;
                    var result = _appContext.Allowances.Update(r);
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
        /// Get List Group is Activce
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<ResultWithError> GetList()
        {
            try
            {
                var queryable = await (from p in _appContext.Allowances
                                       where p.IS_ACTIVE == true
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
        /// <summary>
        /// CMS Get Detail
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>       
        public async Task<ResultWithError> CheckAllowIsUsed(string param)
        {
            try
            {
                // check log time sheet
                string r = await QueryData.ExecuteStoreString(Procedures.PKG_DEMO_CHECK_ALLOWANCE_USED, new
                {

                    P_CODE = param,
                    p_cur = QueryData.OUT_CURSOR
                });
                return new ResultWithError(r);
            }
            catch (Exception ex)
            {
                return new ResultWithError(ex.Message);
            }
        }
        public async Task<ResultWithError> RemoteAsync(List<long> ids)
        {
            try
            {
                foreach (var item in ids)
                {
                    var r = _appContext.Allowances.Where(x => x.ID == item).FirstOrDefault();
                    if (r == null)
                    {
                        return new ResultWithError(404);
                    }

                    var result = _appContext.Allowances.Remove(r);
                }
                await _appContext.SaveChangesAsync();
                return new ResultWithError(200);
            }
            catch (Exception ex)
            {
                return new ResultWithError(ex.Message);
            }
        }
        public async Task<GenericPhaseTwoListResponse<AllowanceViewDTO>> SinglePhaseQueryList(GenericQueryListDTO<AllowanceViewDTO> request)
        {
            var queryable = (from p in _appContext.Allowances
                             from g in _appContext.OtherLists.Where(c => c.ID == p.TYPE_ID).DefaultIfEmpty()
                                 // where p.IS_ACTIVE == true
                             orderby p.CODE
                             select new AllowanceViewDTO
                             {
                                 Id = p.ID,
                                 Code = p.CODE,
                                 Name = p.NAME,
                                 TypeId = p.TYPE_ID,
                                 TypeName = g.NAME,
                                 IsActive = p.IS_ACTIVE,
                                 IsActiveStr = p.IS_ACTIVE == true ? "Áp dụng" : "Ngừng áp dụng",
                                 Note = p.NOTE,
                                 IsInsurance = p.IS_INSURANCE,
                                 IsCoefficient = p.IS_COEFFICIENT,
                                 IsSal = p.IS_SAL,
                                 CreateBy = p.CREATED_BY,
                                 UpdatedBy = p.UPDATED_BY,
                                 CreateDate = p.CREATED_DATE,
                                 UpdatedDate = p.UPDATED_DATE

                             });
            var singlePhaseResult = await genericReducer.SinglePhaseReduce(queryable, request);
            return singlePhaseResult;
        }
    }
}
