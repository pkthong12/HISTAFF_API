using Common.Paging;
using ProfileDAL.ViewModels;
using Common.Repositories;
using Common.Extensions;
using API.All.DbContexts;
using API.Entities;

namespace ProfileDAL.Repositories
{
    public class ContractTypeSysRepository : RepositoryBase<SYS_CONTRACT_TYPE>, IContractTypeSysRepository
    {
        private ProfileDbContext _appContext => (ProfileDbContext)_context;
        public ContractTypeSysRepository(ProfileDbContext context) : base(context)
        {

        }
        /// <summary>
        /// CMS Get All Data
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<PagedResult<ContractTypeSysViewDTO>> GetAll(ContractTypeSysViewDTO param)
        {
            var queryable = from p in _appContext.ContractTypeSyses
                            select new ContractTypeSysViewDTO
                            {
                                Id = p.ID,
                                Code = p.CODE,
                                Name = p.NAME,
                                Period = p.PERIOD,
                                DayNotice = p.DAY_NOTICE,
                                Note = p.NOTE,
                                IsActive = p.IS_ACTIVE,
                                CreateBy = p.CREATED_BY,
                                UpdatedBy = p.UPDATED_BY,
                                CreateDate = p.CREATED_DATE,
                                UpdatedDate = p.UPDATED_DATE,
                                IsLeave = p.IS_LEAVE
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
                var r = await (from p in _appContext.ContractTypeSyses
                               select new
                               {
                                   Id = p.ID,
                                   code = p.CODE,
                                   name = p.NAME,
                                   Period = p.PERIOD,
                                   DayNotice = p.DAY_NOTICE,
                                   Note = p.NOTE,
                                   IsActive = p.IS_ACTIVE,
                                   IsLeave = p.IS_LEAVE
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
        public async Task<ResultWithError> CreateAsync(ContractTypeSysInputDTO param)
        {

            try
            {
                var r = _appContext.ContractTypeSyses.Where(x => x.CODE == param.Code).Count();
                if (r > 0)
                {
                    return new ResultWithError(Consts.CODE_EXISTS);
                }
                var data = Map(param, new SYS_CONTRACT_TYPE());
                data.IS_ACTIVE = true;
                var result = await _appContext.ContractTypeSyses.AddAsync(data);
                await _appContext.SaveChangesAsync();
                //return data
                var output = new ContractTypeSysOutputDTO();
                var values = Map(data, output);
                return new ResultWithError(output);
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
        public async Task<ResultWithError> UpdateAsync(ContractTypeSysInputDTO param)
        {
            try
            {
                // check code
                var c = _appContext.ContractTypeSyses.Where(x => x.CODE.ToLower() == param.Code.ToLower() && x.ID != param.Id).Count();
                if (c > 0)
                {
                    return new ResultWithError(Consts.CODE_EXISTS);
                }

                var r = _appContext.ContractTypeSyses.Where(x => x.ID == param.Id).FirstOrDefault();
                if (r == null)
                {
                    return new ResultWithError(404);
                }

                var data = Map(param, r);
                var result = _appContext.ContractTypeSyses.Update(data);
                await _appContext.SaveChangesAsync();
                return new ResultWithError(200);
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
                    var r = _appContext.ContractTypeSyses.Where(x => x.ID == item).FirstOrDefault();
                    if (r == null)
                    {
                        return new ResultWithError(404);
                    }
                    r.IS_ACTIVE = !r.IS_ACTIVE;
                    var result = _appContext.ContractTypeSyses.Update(r);
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
                var queryable = await (from p in _appContext.ContractTypeSyses
                                       where p.IS_ACTIVE == true
                                       orderby p.CODE
                                       select new
                                       {
                                           Id = p.ID,
                                           Name = p.NAME,
                                           Code = p.CODE,
                                           Month = p.PERIOD
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
