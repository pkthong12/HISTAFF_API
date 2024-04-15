using AttendanceDAL.ViewModels;
using Common.Interfaces;
using Common.Paging;
using Common.Extensions;
using API.Entities;

namespace AttendanceDAL.Repositories
{
    public interface ITimeTypeRepository : IRepository<AT_TIME_TYPE>
    {
        Task<PagedResult<TimeTypeDTO>> GetAll(TimeTypeDTO param);
        Task<ResultWithError> GetById(long id);
        Task<ResultWithError> CreateAsync(TimeTypeInputDTO param);
        Task<ResultWithError> UpdateAsync(TimeTypeInputDTO param);
        Task<ResultWithError> ChangeStatusAsync(List<long> ids);
        Task<ResultWithError> GetList();
        Task<ResultWithError> GetListOff();
        Task<ResultWithError> PortalGetListOff();
    }
}
