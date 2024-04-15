using AttendanceDAL.ViewModels;
using Common.Repositories;
using Common.Extensions;
using API.All.DbContexts;
using API.Entities;

namespace AttendanceDAL.Repositories
{
    public class ConfigRepository : RepositoryBase<AT_CONFIG>, IConfigRepository
    {
        private AttendanceDbContext _appContext => (AttendanceDbContext)_context;
        public ConfigRepository(AttendanceDbContext context) : base(context)
        {

        }
        /// <summary>
        /// CMS Get All Data
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<ResultWithError> GetConfig()
        {
            var r = await (from p in _appContext.AttendanceConfigs
                           
                           select new ConfigDTO
                           {
                               Id = p.ID,
                               DateClear = p.DATE_CLEAR,
                               AdvanceNumber = p.ADVANCE_NUMBER
                           }).FirstOrDefaultAsync();
            return new ResultWithError(r);
        }

        /// <summary>
        /// CMS Edit Data
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<ResultWithError> UpdateAsync(ConfigDTO param)
        {
            try
            {
                if (param.Id != null)
                {
                    var r = await _appContext.AttendanceConfigs.Where(x => x.ID == param.Id ).FirstOrDefaultAsync();
                    if (r == null)
                    {
                        return new ResultWithError(404);
                    }
                    r.ADVANCE_NUMBER = param.AdvanceNumber;
                    r.DATE_CLEAR = param.DateClear;
                    var result = _appContext.AttendanceConfigs.Update(r);
                }
                else
                {
                    var data = new AT_CONFIG();
                    data.ADVANCE_NUMBER = param.AdvanceNumber;
                    data.DATE_CLEAR = param.DateClear;
                    var result = _appContext.AttendanceConfigs.AddAsync(data);
                }
                await _appContext.SaveChangesAsync();
                return new ResultWithError(200);
            }
            catch (Exception ex)
            {
                return new ResultWithError(ex.Message);
            }
        }

    }
}
