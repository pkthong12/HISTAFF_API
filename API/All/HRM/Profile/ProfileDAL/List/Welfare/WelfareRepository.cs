using Common.Paging;
using ProfileDAL.ViewModels;
using Common.Repositories;
using Common.Extensions;
using API.All.DbContexts;
using API.Entities;
using CORE.DTO;
using CORE.Enum;
using CORE.GenericUOW;
using CORE.StaticConstant;
using API.DTO;

namespace ProfileDAL.Repositories
{
    public class WelfareRepository : RepositoryBase<HU_WELFARE>, IWelfareRepository
    {
        private ProfileDbContext _appContext => (ProfileDbContext)_context;
        private readonly GenericReducer<HU_WELFARE, WelfareDTO> genericReducer;
        public WelfareRepository(ProfileDbContext context) : base(context)
        {
            genericReducer = new();
        }
        public async Task<GenericPhaseTwoListResponse<WelfareDTO>> TwoPhaseQueryList(GenericQueryListDTO<WelfareDTO> request)
        {
            var raw = _appContext.Welfares.AsQueryable();
            var phase1 = await genericReducer.FirstPhaseReduce(raw, request);

            if (phase1.ErrorType != EnumErrorType.NONE)
            {
                return new()
                {
                    ErrorType = phase1.ErrorType,
                    MessageCode = phase1.MessageCode,
                    ErrorPhase = 1
                };
            }

            if (phase1.Queryable == null)
            {
                return new()
                {
                    ErrorType = EnumErrorType.CATCHABLE,
                    MessageCode = CommonMessageCode.ENTITIES_NOT_FOUND,
                    ErrorPhase = 1
                };
            }

            var phase1IdsResult = phase1.Queryable.ToList().Aggregate("", (prev, curr) => prev + curr.ID.ToString() + ";");
            var ids = phase1IdsResult.Split(';');

            var joined = from p in _appContext.Welfares.Where(p => ids.Contains(p.ID.ToString()))
                         from g in _appContext.OtherLists.Where(f => f.ID == p.GENDER_ID).DefaultIfEmpty()
                         select new WelfareDTO
                         {
                             Id = p.ID,
                             Code = p.CODE,
                             Name = p.NAME,
                             Monney = p.MONNEY,
                             Seniority = p.SENIORITY,
                             DateStart = p.DATE_START,
                             DateEnd = p.DATE_END,
                             Active = p.IS_ACTIVE == true ? "Áp dụng" : "Ngừng áp dụng",
                             Note = p.NOTE,
                             CreateBy = p.CREATED_BY,
                             UpdatedBy = p.UPDATED_BY,
                             CreateDate = p.CREATED_DATE,
                             UpdatedDate = p.UPDATED_DATE,
                             //ContractTypeName = string.Join(";", (from t in _appContext.WelfareContracts
                             //                                     join d in _appContext.ContractTypes on t.CONTRACT_TYPE_ID equals d.ID
                             //                                     where t.WELFARE_ID == p.ID
                             //                                     select d.NAME).ToList()),
                             seniorityAbove = p.SENIORITY_ABOVE,
                             isAutoActive = p.IS_AUTO_ACTIVE,
                             isCalTax = p.IS_CAL_TAX,
                             paymentDate = p.PAYMENT_DATE,
                             percentage = p.PERCENTAGE,
                             genderId = g.NAME,
                             ageFrom = p.AGE_FROM,
                             ageTo = p.AGE_TO,
                             workLeaveNopayFrom = p.WORK_LEAVE_NOPAY_FROM,
                             workLeaveNopayTo = p.WORK_LEAVE_NOPAY_TO,
                             monthsPendFrom = p.MONTHS_PEND_FROM,
                             monthsPendTo = p.MONTHS_PEND_TO,
                             monthsWorkInYear = p.MONTHS_WORK_IN_YEAR,
                         };
            var phase2 = await genericReducer.SecondPhaseReduce(joined, request);
            return phase2;
        }
        /// <summary>
        /// CMS Get All Data
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<PagedResult<WelfareDTO>> GetAll(WelfareDTO param)
        {
            var queryable = from p in _appContext.Welfares
                            orderby p.CREATED_DATE descending
                            select new WelfareDTO
                            {
                                Id = p.ID,
                                Code = p.CODE,
                                Name = p.NAME,
                                Monney = p.MONNEY,
                                Seniority = p.SENIORITY,
                                DateStart = p.DATE_START,
                                DateEnd = p.DATE_END,
                                //ContractTypeName = String.Join(";", (from t in _appContext.WelfareContracts
                                //                                     join d in _appContext.ContractTypes on t.CONTRACT_TYPE_ID equals d.ID
                                //                                     where t.WELFARE_ID == p.ID
                                //                                     select d.NAME).ToList()),
                                //ContractTypeIds = String.Join(";", (from t in _appContext.WelfareContracts
                                //                                    where t.WELFARE_ID == p.ID
                                //                                    select t.CONTRACT_TYPE_ID).ToList()),
                                IsActive = p.IS_ACTIVE,
                                Note = p.NOTE,
                                CreateBy = p.CREATED_BY,
                                UpdatedBy = p.UPDATED_BY,
                                CreateDate = p.CREATED_DATE,
                                UpdatedDate = p.UPDATED_DATE,
                            };


            if (!string.IsNullOrWhiteSpace(param.Name))
            {
                queryable = queryable.Where(p => p.Name.ToUpper().Contains(param.Name.ToUpper()));
            }
            if (!string.IsNullOrWhiteSpace(param.Code))
            {
                queryable = queryable.Where(p => p.Code.ToUpper().Contains(param.Code.ToUpper()));
            }
            if (!string.IsNullOrWhiteSpace(param.ContractTypeName))
            {
                queryable = queryable.Where(p => p.ContractTypeName.ToUpper().Contains(param.ContractTypeName.ToUpper()));
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


                var r = await (from p in _appContext.Welfares
                               where p.ID == id
                               select new WelfareInputDTO
                               {
                                   Id = p.ID,
                                   Name = p.NAME,
                                   Code = p.CODE,
                                   Monney = p.MONNEY,
                                   Seniority = p.SENIORITY,
                                   DateStart = p.DATE_START,
                                   DateEnd = p.DATE_END,
                                   Note = p.NOTE,
                                   //ContractTypes = (from t in _appContext.WelfareContracts
                                   //            where t.WELFARE_ID == p.ID
                                   //            select t.CONTRACT_TYPE_ID).ToList(),
                                   seniorityAbove = p.SENIORITY_ABOVE,
                                   isAutoActive = p.IS_AUTO_ACTIVE,
                                   isCalTax = p.IS_CAL_TAX,
                                   paymentDate = p.PAYMENT_DATE,
                                   percentage = p.PERCENTAGE,
                                   genderId = p.GENDER_ID,
                                   ageFrom = p.AGE_FROM,
                                   ageTo = p.AGE_TO,
                                   workLeaveNopayFrom = p.WORK_LEAVE_NOPAY_FROM,
                                   workLeaveNopayTo = p.WORK_LEAVE_NOPAY_TO,
                                   monthsPendFrom = p.MONTHS_PEND_FROM,
                                   monthsPendTo = p.MONTHS_PEND_TO,
                                   monthsWorkInYear = p.MONTHS_WORK_IN_YEAR,
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
        public async Task<ResultWithError> CreateAsync(WelfareInputDTO param)
        {
            try
            {
                var r1 = _appContext.Welfares.Where(x => x.CODE == param.Code).Count();
                if (r1 > 0)
                {
                    return new ResultWithError(Consts.CODE_EXISTS);
                }
                var data = Map(param, new HU_WELFARE());
                data.IS_ACTIVE = true;
                var result = await _appContext.Welfares.AddAsync(data);

                await _appContext.SaveChangesAsync();
                param.Id = data.ID;
                //if (param.ContractTypes != null && param.ContractTypes.Count > 0)
                //{
                //    foreach (var item in param.ContractTypes)
                //    {
                //        var con = new HU_WELFARE_CONTRACT();
                //        con.CONTRACT_TYPE_ID = item;
                //        con.WELFARE_ID = data.ID;
                //        await _appContext.WelfareContracts.AddAsync(con);

                //    }
                //}
                //await _appContext.SaveChangesAsync();
                return new ResultWithError(param);
            }
            catch (Exception ex)
            {
                return new ResultWithError(ex);
            }
        }
        /// <summary>
        /// CMS Edit Data
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<ResultWithError> UpdateAsync(WelfareInputDTO param)
        {
            try
            {
                // check code
                var c = _appContext.Welfares.Where(x => x.CODE.ToLower() == param.Code.ToLower() && x.DATE_START == param.DateStart && x.ID != param.Id).Count();
                if (c > 0)
                {
                    return new ResultWithError(Consts.CODE_EXISTS);
                }

                var r = _appContext.Welfares.Where(x => x.ID == param.Id).FirstOrDefault();
                if (r == null)
                {
                    return new ResultWithError(404);
                }

                var data = Map(param, r);
                var result = _appContext.Welfares.Update(data);

                //var d = _appContext.WelfareContracts.Where(x => x.WELFARE_ID == param.Id).ToList();
                //if (d != null)
                //{
                //    _appContext.WelfareContracts.RemoveRange(d);
                //}
                //if (param.ContractTypes != null && param.ContractTypes.Count > 0)
                //{
                //    foreach (var item in param.ContractTypes)
                //    {
                //        var con = new HU_WELFARE_CONTRACT();
                //        con.CONTRACT_TYPE_ID = item;
                //        con.WELFARE_ID = data.ID;
                //        await _appContext.WelfareContracts.AddAsync(con);
                //    }
                //}
                await _appContext.SaveChangesAsync();
                return new ResultWithError(param);
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
        public async Task<ResultWithError> ChangeStatusAsync(WelfareInputDTO request)
        {
            try
            {
                foreach (var item in request.Ids)
                {
                    var r = _appContext.Welfares.Where(x => x.ID == item).FirstOrDefault();
                    if (r == null)
                    {
                        return new ResultWithError(404);
                    }
                    r.IS_ACTIVE = request.ValueToBind;
                    var result = _appContext.Welfares.Update(r);
                }
                await _appContext.SaveChangesAsync();
                return new ResultWithError(request);
            }
            catch (Exception ex)
            {
                return new ResultWithError(ex.Message);
            }
        }

        public async Task<ResultWithError> GetList()
        {
            var queryable = await (from p in _appContext.Welfares
                                   where p.IS_ACTIVE == true && p.IS_AUTO_ACTIVE == false
                                   orderby p.CODE
                                   select new
                                   {
                                       Id = p.ID,
                                       Name = p.NAME,
                                       Code = p.CODE
                                   }).ToListAsync();
            return new ResultWithError(queryable);
        }

        public async Task<ResultWithError> GetListAuto()
        {
            var queryable = await (from p in _appContext.Welfares
                                   where p.IS_ACTIVE == true && p.IS_AUTO_ACTIVE == true
                                   orderby p.CODE
                                   select new
                                   {
                                       Id = p.ID,
                                       Name = p.NAME,
                                       Code = p.CODE
                                   }).ToListAsync();
            return new ResultWithError(queryable);
        }
        public async Task<FormatedResponse> GetListInPeriod(HuWelfareDTO param)
        {
            var queryable = await (from p in _appContext.Welfares
                                   where p.IS_ACTIVE == true && (p.DATE_START!.Value.Date <= param.DateStart!.Value.Date && p.DATE_END!.Value.Date >= param.DateStart!.Value.Date) && p.IS_AUTO_ACTIVE != true
                                   orderby p.CODE
                                   select new
                                   {
                                       Id = p.ID,
                                       Name = p.NAME,
                                       Code = p.CODE,
                                       Date1 = p.DATE_START!.Value.Date,
                                       Date2 = p.DATE_END!.Value.Date,
                                   }).ToListAsync();
            return new() { InnerBody = queryable };
        }
    }
}
