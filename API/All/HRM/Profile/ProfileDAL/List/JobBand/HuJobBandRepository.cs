using Common.Paging;
using ProfileDAL.ViewModels;
using Common.Repositories;
using Common.Extensions;
using API.All.DbContexts;
using API.Entities;

namespace ProfileDAL.Repositories
{
    public class HuJobBandRepository : RepositoryBase<HU_JOB_BAND>, IHuJobBandRepository
    {
        private ProfileDbContext _appContext => (ProfileDbContext)_context;
        private readonly ILogger _logger;

        public HuJobBandRepository(ProfileDbContext context) : base(context)
        {

        }

        public async Task<PagedResult<HUJobBandDTO>> GetJobBands(HUJobBandDTO param)
        {
            try
            {
                var queryable = from p in _appContext.HUJobBands
                                from o in _appContext.OtOtherLists.Where(x => x.ID == p.TITLE_GROUP_ID).DefaultIfEmpty()

                                select new HUJobBandDTO
                                {
                                    Id = p.ID,
                                    NameVN = p.NAME_VN,
                                    NameEN = p.NAME_EN,
                                    LevelFrom = p.LEVEL_FROM,
                                    LevelTo = p.LEVEL_TO,
                                    Status = p.STATUS,
                                    TitleGroupId = p.TITLE_GROUP_ID,
                                    TitleGroupName = o.NAME
                                };
                if (param.NameVN != null)
                {
                    queryable = queryable.Where(p => p.NameVN.ToUpper().Contains(param.NameVN.ToUpper()));
                }

                if (param.NameEN != null)
                {
                    queryable = queryable.Where(p => p.NameEN.ToUpper().Contains(param.NameEN.ToUpper()));
                }

                if (param.LevelFrom != null)
                {
                    queryable = queryable.Where(p => p.LevelFrom.ToUpper().Contains(param.LevelFrom.ToUpper()));
                }

                if (param.LevelTo != null)
                {
                    queryable = queryable.Where(p => p.LevelTo.ToUpper().Contains(param.LevelTo.ToUpper()));
                }

                if (param.StatusName != null)
                {
                    queryable = queryable.Where(p => p.StatusName.ToUpper().Contains(param.StatusName.ToUpper()));
                }


                return await PagingList(queryable, param);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.ToString(), param);
                return null;
            }
        }

        public async Task<ResultWithError> GetJobBand(int id)
        {
            try
            {
                var query = await (from p in _appContext.HUJobBands
                                   where p.ID == id
                                select new HUJobBandInputDTO
                                {
                                    Id = p.ID,
                                    NameVn = p.NAME_VN,
                                    NameEn = p.NAME_EN,
                                    LevelFrom = p.LEVEL_FROM,
                                    LevelTo = p.LEVEL_TO,
                                    Status = p.STATUS,
                                    TitleGroupId = p.TITLE_GROUP_ID,
                                }).FirstOrDefaultAsync();


                return new ResultWithError(query);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return new ResultWithError(ex.Message);
            }
        }

        public async Task<ResultWithError> UpdateAsync(HUJobBandInputDTO param)
        {
            try
            {
                if(param.Id != 0)
                {
                    var obj = (from p in _appContext.HUJobBands where p.ID == param.Id select p).FirstOrDefault();
                    if(obj == null)
                    {
                        return new ResultWithError("OBJ_NULL");
                    }

                    obj.NAME_VN = param.NameVn;
                    obj.NAME_EN = param.NameEn;
                    obj.LEVEL_FROM = param.LevelFrom;
                    obj.MODIFIED_DATE = DateTime.Now;
                    obj.CREATED_DATE = DateTime.Now;
                    var result = _appContext.HUJobBands.Update(obj);
                    await _appContext.SaveChangesAsync();
                }
                else
                {
                    var data = Map(param, new HU_JOB_BAND());
                    data.STATUS = true;
                    data.MODIFIED_DATE = DateTime.Now;
                    var result = await _appContext.HUJobBands.AddAsync(data);
                    await _appContext.SaveChangesAsync();
                    return new ResultWithError(data);
                }
                return new ResultWithError(200);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return new ResultWithError(ex.Message);
            }
        }

        public async Task<ResultWithError> ChangeStatusAsync(List<long> ids, bool status)
        {
            try
            {
                var query = from p in _appContext.HUJobBands where ids.Contains(p.ID) select p;
                foreach(var item in query)
                {
                    item.STATUS = status;
                    item.MODIFIED_DATE = DateTime.Now;
                }
                await _appContext.SaveChangesAsync();
                return new ResultWithError(200);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return new ResultWithError(ex.Message);
            }
        }

        public async Task<ResultWithError> ChangeStatusAsync(List<long> ids)
        {
            try
            {
                var query = from p in _appContext.HUJobBands where ids.Contains(p.ID) select p;
                foreach (var item in query)
                {
                    item.STATUS = item.STATUS;
                    item.MODIFIED_DATE = DateTime.Now;
                }
                await _appContext.SaveChangesAsync();
                return new ResultWithError(200);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return new ResultWithError(ex.Message);
            }
        }

        public async Task<ResultWithError> DeleteAsync(List<long> ids)
        {
            try
            {
                var query = from p in _appContext.HUJobBands where ids.Contains(p.ID) select p;
                foreach (var item in query)
                {
                    _appContext.HUJobBands.Remove(item);
                }
                await _appContext.SaveChangesAsync();
                return new ResultWithError(200);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return new ResultWithError(ex.Message);
            }
        }

        public async Task<bool> ValidateJobBand(HUJobBandInputDTO param)
        {
            try
            {
                var c = _appContext.HUJobBands.Where(x => x.NAME_VN.ToLower().Equals(param.NameVn.ToLower()) && x.ID != param.Id);
                return  await c.AnyAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return false;
            }
        }

        public async Task<ResultWithError> GetCboJobBand()
        {
            try
            {
                var data = await (from p in _appContext.HUJobBands
                                  select new{Id = p.ID,
                                             name=p.LEVEL_FROM}).ToListAsync();
                return new ResultWithError(data);
            }
            catch (Exception ex)
            {
                return new ResultWithError(ex.Message);
            }
        }

    }
}
