using API.Entities;
using AttendanceDAL.ViewModels;
using Common.Interfaces;
using Common.Paging;
using Common.Extensions;
using CORE.DTO;

namespace AttendanceDAL.Repositories
{
    public interface ISalaryPeriodRepository : IRepository<AT_SALARY_PERIOD>
    {
        Task<GenericPhaseTwoListResponse<SalaryPeriodDTO>> TwoPhaseQueryList(GenericQueryListDTO<SalaryPeriodDTO> request);
        Task<PagedResult<SalaryPeriodDTO>> GetAll(SalaryPeriodDTO param);
        Task<ResultWithError> GetById(long id);
        Task<ResultWithError> CreateAsync(SalaryPeriodInputDTO param);
        Task<ResultWithError> UpdateAsync(SalaryPeriodInputDTO param);
        Task<ResultWithError> ChangeStatusAsync(List<long> ids);
        Task<ResultWithError> GetList(int? Id);
        Task<ResultWithError> GetYear();
        Task<ResultWithError> PortalGetYear();
        Task<ResultWithError> PortalByYear(int year);
    }
}
