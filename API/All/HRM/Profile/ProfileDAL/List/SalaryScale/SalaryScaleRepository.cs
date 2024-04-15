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
namespace ProfileDAL.Repositories
{
    public class SalaryScaleRepository : RepositoryBase<HU_SALARY_SCALE>, ISalaryScaleRepository
    {
        private ProfileDbContext _appContext => (ProfileDbContext)_context;
        private readonly GenericReducer<HU_SALARY_SCALE, SalaryScaleViewDTO> genericReducer;

        public SalaryScaleRepository(ProfileDbContext context) : base(context)
        {
            genericReducer = new();
        }
        /// <summary>
        /// CMS Get All Data
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        /// 
        public async Task<GenericPhaseTwoListResponse<SalaryScaleViewDTO>> TwoPhaseQueryList(GenericQueryListDTO<SalaryScaleViewDTO> request)
        {
            var raw = _appContext.SalaryScales.AsQueryable();
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

            var joined = from p in _appContext.SalaryScales.Where(p => ids.Contains(p.ID.ToString()))
                         select new SalaryScaleViewDTO
                         {
                             Id = p.ID,
                             Code = p.CODE,
                             Name = p.NAME,
                             Note = p.NOTE,
                             CreateDate = p.CREATED_DATE,
                             IsActive = p.IS_ACTIVE,
                             IsTableScore = p.IS_TABLE_SCORE,
                             ExpirationDate = p.EXPIRATION_DATE,
                             EffectiveDate = p.EFFECTIVE_DATE
                         };
            var phase2 = await genericReducer.SecondPhaseReduce(joined, request);
            return phase2;
        }

        public async Task<GenericPhaseTwoListResponse<SalaryScaleViewDTO>> SinglePhaseQueryList(GenericQueryListDTO<SalaryScaleViewDTO> request)
        {
            var joined = from p in _appContext.SalaryScales.AsNoTracking()
                         select new SalaryScaleViewDTO
                         {
                             Id = p.ID,
                             Code = p.CODE,
                             Name = p.NAME,
                             Note = p.NOTE,
                             CreateDate = p.CREATED_DATE,
                             IsTableScore = p.IS_TABLE_SCORE,
                             ExpirationDate = p.EXPIRATION_DATE,
                             EffectiveDate = p.EFFECTIVE_DATE,
                             IsActive = p.IS_ACTIVE,
                             Status = p.IS_ACTIVE == true ? "Áp dụng" : "Ngừng áp dụng"
                         };

            var singlePhaseResult = await genericReducer.SinglePhaseReduce(joined, request);
            return singlePhaseResult;
        }

        public async Task<PagedResult<SalaryScaleDTO>> GetAll(SalaryScaleDTO param)
        {
            var queryable = from p in _appContext.SalaryScales

                            select new SalaryScaleDTO
                            {
                                Id = p.ID,
                                Code = p.CODE,
                                Name = p.NAME,
                                Orders = p.ORDERS,
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
                var r = await (from p in _appContext.SalaryScales
                               where p.ID == id
                               select new
                               {
                                   Id = p.ID,
                                   Code = p.CODE,
                                   Name = p.NAME,
                                   Orders = p.ORDERS,
                                   IsActive = p.IS_ACTIVE,
                                   Note = p.NOTE,
                                   IsTableScore = p.IS_TABLE_SCORE,
                                   ExpirationDate = p.EXPIRATION_DATE,
                                   EffectiveDate = p.EFFECTIVE_DATE,
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
        public async Task<ResultWithError> CreateAsync(SalaryScaleInputDTO param)
        {
            try
            {
                // check code
                var c = _appContext.SalaryScales.Where(x => x.CODE.ToLower() == param.Code.ToLower()).Count();
                if (c > 0)
                {
                    return new ResultWithError(Consts.CODE_EXISTS);
                }
                var data = Map(param, new HU_SALARY_SCALE());
                data.IS_ACTIVE = true;
                var result = await _appContext.SalaryScales.AddAsync(data);
                await _appContext.SaveChangesAsync();
                return new ResultWithError(data);
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
        public async Task<ResultWithError> UpdateAsync(SalaryScaleInputDTO param)
        {
            try
            {
                // check code
                var c = _appContext.SalaryScales.Where(x => x.CODE.ToLower() == param.Code.ToLower() && x.ID != param.Id).Count();
                if (c > 0)
                {
                    return new ResultWithError(Consts.CODE_EXISTS);
                }

                var r = _appContext.SalaryScales.Where(x => x.ID == param.Id).FirstOrDefault();
                if (r == null)
                {
                    return new ResultWithError(404);
                }

                var data = Map(param, r);
                var result = _appContext.SalaryScales.Update(data);
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
        public async Task<ResultWithError> ChangeStatusAsync(List<long> ids)
        {
            try
            {
                foreach (var item in ids)
                {
                    var r = _appContext.SalaryScales.Where(x => x.ID == item).FirstOrDefault();
                    if (r == null)
                    {
                        return new ResultWithError(404);
                    }
                    r.IS_ACTIVE = !r.IS_ACTIVE;
                    var result = _appContext.SalaryScales.Update(r);
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
                var queryable = await (from p in _appContext.SalaryScales
                                       where p.IS_ACTIVE == true
                                       orderby p.ORDERS
                                       select new
                                       {
                                           Id = p.ID,
                                           Name = p.NAME,
                                           IsTableScore = p.IS_TABLE_SCORE
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
