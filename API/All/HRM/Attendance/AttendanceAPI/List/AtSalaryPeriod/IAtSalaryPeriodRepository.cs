using API.DTO;
using CORE.DTO;
using CORE.GenericUOW;

namespace API.Controllers.AtSalaryPeriod
{
    public interface IAtSalaryPeriodRepository : IGenericRepository<AT_SALARY_PERIOD, AtSalaryPeriodDTO>
    {
        Task<GenericPhaseTwoListResponse<AtSalaryPeriodDTO>> SinglePhaseQueryList(GenericQueryListDTO<AtSalaryPeriodDTO> request);

        Task<FormatedResponse> AddNewWithAtOrgPeriods(AtSalaryPeriodDTO reqquest, string sid);
        Task<FormatedResponse> UpdateWithAtOrgPeriods(GenericUnitOfWork _uow, AtSalaryPeriodDTO reqquest,string sid, bool patchMode = true);

        Task<FormatedResponse> GetListSalaryInYear(AtSalaryPeriodDTO param);

        Task<FormatedResponse> GetListSalaryPeriod(AtSalaryPeriodDTO param);
    }
}

