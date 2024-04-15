using AttendanceDAL.ViewModels;
using Common.Interfaces;
using Common.Paging;
using Common.Extensions;
using API.Entities;

namespace AttendanceDAL.Repositories
{
    public interface IOverTimeRepository : IRepository<AT_OVERTIME>
    {
        
        Task<PagedResult<OverTimeDTO>> GetAll(OverTimeDTO param);
        Task<ResultWithError> CreateAsync(OverTimeCreateDTO param);
        Task<ResultWithError> UpdateAsync(OverTimeInputDTO param);
        Task<ResultWithError> ChangeStatusAsync(List<long> ids, int StatusId);
        Task<ResultWithError> Delete(List<long> ids);
        Task<ResultWithError> UpdateConfig(OverTimeConfigDTO param);
        Task<ResultWithError> GetConfig();
        
    }
}
