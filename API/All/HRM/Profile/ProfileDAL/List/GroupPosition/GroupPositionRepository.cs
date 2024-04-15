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
    public class GroupPositionRepository : RepositoryBase<HU_POSITION_GROUP>, IGroupPositionRepository
    {
        private ProfileDbContext _appContext => (ProfileDbContext)_context;
        private readonly GenericReducer<HU_POSITION_GROUP, GroupPositionViewDTO> genericReducer;
        public GroupPositionRepository(ProfileDbContext context) : base(context)
        {
            genericReducer = new();
        }
        public async Task<GenericPhaseTwoListResponse<GroupPositionViewDTO>> TwoPhaseQueryList(GenericQueryListDTO<GroupPositionViewDTO> request)
        {
            var raw = _appContext.GroupPositions.AsQueryable();
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

            var joined = from p in _appContext.GroupPositions.Where(p => ids.Contains(p.ID.ToString()))
                         orderby p.CREATED_DATE descending
                         select new GroupPositionViewDTO
                         {
                             Id = p.ID,
                             Code = p.CODE,
                             Name = p.NAME,                                                   
                             Note = p.NOTE,
                             StatusName = p.IS_ACTIVE == true ? "Áp dụng" : "Ngưng áp dụng",
                             IsActive = p.IS_ACTIVE
                            
                         };
            var phase2 = await genericReducer.SecondPhaseReduce(joined, request);
            return phase2;
        }
        /// <summary>
        /// CMS Get All Data
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<PagedResult<GroupPositionDTO>> GetAll(GroupPositionDTO param)
        {
            var queryable = from p in _appContext.GroupPositions
                            
                            select new GroupPositionDTO
                            {
                                Id = p.ID,
                                Name = p.NAME,
                                Code = p.CODE,
                                IsActive = p.IS_ACTIVE,
                                Note = p.NOTE,
                                CreateBy = p.CREATED_BY,
                                CreateDate = p.CREATED_DATE,
                                UpdateBy = p.UPDATED_BY,
                                UpdateDate = p.UPDATED_DATE
                            };

            if (param.Name != null)
            {
                queryable = queryable.Where(p => p.Name.ToUpper().Contains(param.Name.ToUpper()));
            }

            if (param.Code != null)
            {
                queryable = queryable.Where(p => p.Code.ToUpper().Contains(param.Code.ToUpper()));
            }

            if (param.Note != null)
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
        public async Task<ResultWithError> GetById(int id)
        {
            var r = await (from p in _appContext.GroupPositions
                           where p.ID == id 
                           select new GroupPositionDTO
                           {
                               Id = p.ID,
                               Name = p.NAME,
                               Code = p.CODE,
                               Note = p.NOTE,
                               IsActive = p.IS_ACTIVE
                           }).FirstOrDefaultAsync();
            return new ResultWithError(r);
        }
        /// <summary>
        /// Create Data
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<ResultWithError> CreateAsync(GroupPositionInputDTO param)
        {
            var r = _appContext.GroupPositions.Where(x => x.CODE == param.Code ).Count();
            if (r > 0)
            {
                return new ResultWithError(Consts.CODE_EXISTS);
            }
            var data = Map(param, new HU_POSITION_GROUP());
            data.IS_ACTIVE = true;
            var result = await _appContext.GroupPositions.AddAsync(data);
            try
            {
                await _appContext.SaveChangesAsync();
                param.Id = data.ID;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return new ResultWithError(param);
        }
        /// <summary>
        /// CMS Edit Data
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<ResultWithError> UpdateAsync(GroupPositionInputDTO param)
        {
            // check code
            var c = _appContext.GroupPositions.Where(x => x.CODE.ToLower() == param.Code.ToLower()  && x.ID != param.Id).Count();
            if (c > 0)
            {
                return new ResultWithError(Consts.CODE_EXISTS);
            }

            var r = _appContext.GroupPositions.Where(x => x.ID == param.Id ).FirstOrDefault();
            if (r == null)
            {
                return new ResultWithError(404);
            }

            var data = Map(param, r);
            var result = _appContext.GroupPositions.Update(data);
            await _appContext.SaveChangesAsync();
            return new ResultWithError(param);
        }
        /// <summary>
        /// CMS Change Status Data
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<ResultWithError> ChangeStatusAsync(List<int> ids)
        {
            foreach (var item in ids)
            {
                var r = _appContext.GroupPositions.Where(x => x.ID == item ).FirstOrDefault();
                if (r == null)
                {
                    return new ResultWithError(404);
                }
                r.IS_ACTIVE = !r.IS_ACTIVE;
                var result = _appContext.GroupPositions.Update(r);
            }
            await _appContext.SaveChangesAsync();
            return new ResultWithError(200);
        }
        /// <summary>
        /// Get List Group is Activce
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<ResultWithError> GetList()
        {
            var queryable = await (from p in _appContext.GroupPositions
                                   where p.IS_ACTIVE == true 
                                   orderby p.CODE
                                   select new
                                   {
                                       Id = p.ID,
                                       Name = p.NAME,
                                       Code = p.CODE
                                   }).ToListAsync();
            return new ResultWithError(queryable);
        }

        public async Task<ResultWithError> Delete(int id)
        {
            try
            {
                var r = await _appContext.Positions.Where(x => x.GROUP_ID == id).CountAsync();
                if (r > 0)
                {
                    return new ResultWithError(Message.DATA_IS_USED);
                }
                var item = await _appContext.GroupPositions.Where(x => x.ID == id ).FirstOrDefaultAsync();
                _appContext.GroupPositions.Remove(item);
                await _appContext.SaveChangesAsync();
                return new ResultWithError(item);
            }
            catch (Exception ex)
            {
                return new ResultWithError(ex.Message);
            }
        }
    }
}
