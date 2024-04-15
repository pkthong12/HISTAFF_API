using Common.Paging;
using ProfileDAL.ViewModels;
using Common.Repositories;
using Common.Extensions;
using API.All.DbContexts;
using API.Entities;

namespace ProfileDAL.Repositories
{
    public class InsuranceTypeRepository : RepositoryBase<INS_TYPE>, IInsuranceTypeRepository
    {
        private ProfileDbContext _appContext => (ProfileDbContext)_context;
        public InsuranceTypeRepository(ProfileDbContext context) : base(context)
        {

        }
        /// <summary>
        /// CMS Get All Data
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<PagedResult<InsuranceTypeDTO>> GetAll(InsuranceTypeDTO param)
        {
            var queryable = from p in _appContext.InsuranceTypes
                            from o in _appContext.OtherListFixs.Where(x=>x.ID==p.TYPE_ID)
                            select new InsuranceTypeDTO
                            {
                                Id = p.ID,
                                TypeId = p.TYPE_ID,
                                TypeName = o.NAME,
                                Name = p.NAME,
                                Note = p.NOTE,
                                IsActive = p.IS_ACTIVE,
                                CreateBy = p.CREATED_BY,
                                UpdatedBy = p.UPDATED_BY,
                                CreateDate = p.CREATED_DATE,
                                UpdatedDate = p.UPDATED_DATE
                            };

            if (!string.IsNullOrWhiteSpace(param.TypeName))
            {
                queryable = queryable.Where(p => p.TypeName.ToUpper().Contains(param.TypeName.ToUpper()));
            }
            if (!string.IsNullOrWhiteSpace(param.Name))
            {
                queryable = queryable.Where(p => p.Name.ToUpper().Contains(param.Name.ToUpper()));
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
                var r = await (from p in _appContext.InsuranceTypes
                               where p.ID == id
                               select new
                               {
                                   Id = p.ID,
                                   TypeId = p.TYPE_ID,
                                   Name = p.NAME,
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
        public async Task<ResultWithError> CreateAsync(InsuranceTypeInputDTO param)
        {
            try
            {
                var data = Map(param, new INS_TYPE());
                data.IS_ACTIVE = true;
                var result = await _appContext.InsuranceTypes.AddAsync(data);
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
        public async Task<ResultWithError> UpdateAsync(InsuranceTypeInputDTO param)
        {
            try
            {                
                var r = _appContext.InsuranceTypes.Where(x => x.ID == param.Id).FirstOrDefault();
                if (r == null)
                {
                    return new ResultWithError(404);
                }

                var data = Map(param, r);
                var result = _appContext.InsuranceTypes.Update(data);
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
                    var r = _appContext.InsuranceTypes.Where(x => x.ID == item).FirstOrDefault();
                    if (r == null)
                    {
                        return new ResultWithError(404);
                    }
                    r.IS_ACTIVE = !r.IS_ACTIVE;
                    var result = _appContext.InsuranceTypes.Update(r);
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
                var queryable = await (from p in _appContext.InsuranceTypes
                                       where p.IS_ACTIVE == true
                                       orderby p.NAME
                                       select new
                                       {
                                           Id = p.ID,
                                           Name = p.NAME,
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
