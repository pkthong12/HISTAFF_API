using AttendanceDAL.ViewModels;
using Common.Interfaces;
using Common.Paging;
using Common.Extensions;
using API.Entities;

namespace AttendanceDAL.Repositories
{
    public interface IDayOffYearRepository : IRepository<AT_DAYOFFYEAR>
    {
        Task<PagedResult<DayOffYearDTO>> GetAll(DayOffYearDTO param);
        Task<ResultWithError> GetById();
        Task<ResultWithError> CreateAsync(DayOffYearConfigDTO param);
        Task<ResultWithError> UpdateAsync(DayOffYearConfigDTO param);
        Task<ResultWithError> CreateDayOffYear(long empId, int yearId);
        //Task<ResultWithError> ChangeStatusAsync(List<long> ids);
        //Task<ResultWithError> GetList(int? Id);

        Task<ResultWithError> PortalEntitlementCur();
    }
}
