using ProfileDAL.ViewModels;
using Common.Interfaces;
using Common.Paging;
using Common.Extensions;
using API.Entities;

namespace ProfileDAL.Repositories
{
    public interface ISettingMapRepository : IRepository<SYS_SETTING_MAP>
    {
        Task<PagedResult<SettingMapDTO>> GetAll(SettingMapDTO param);
        Task<ResultWithError> GetById(long id);
        Task<ResultWithError> CreateAsync(SettingMapInputDTO param);
        Task<ResultWithError> UpdateAsync(SettingMapInputDTO param);
        Task<ResultWithError> ChangeStatusAsync(List<long> ids);
        Task<ResultWithError> GetList();
        ResultWithError GetIP();
        ResultWithError GetBSSID();
    }
}
