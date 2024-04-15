using AttendanceDAL.ViewModels;
using Common.Interfaces;
using Common.Paging;
using Common.Extensions;
using API.Entities;

namespace AttendanceDAL.Repositories
{
    public interface ITimeLateEarlyRepository : IRepository<AT_TIME_LATE_EARLY>
    {
        Task<PagedResult<RegisterOffDTO>> GetAll(RegisterOffDTO param);
        Task<ResultWithError> GetById(long id);
        Task<ResultWithError> CreateAsync(TimeLateEarlyInputDTO param);
        Task<ResultWithError> UpdateAsync(TimeLateEarlyInputDTO param);
       
      

    }
}
