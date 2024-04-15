using API.DTO;
using CORE.DTO;
using CORE.GenericUOW;

namespace API.All.HRM.Payroll.PayrollAPI.PaSalImportBackdate
{
    public interface IPaSalImportBackdateRepository : IGenericRepository<PA_SAL_IMPORT_BACKDATE, PaSalImportBackdateDTO>
    {
        Task<GenericPhaseTwoListResponse<PaSalImportBackdateDTO>> SinglePhaseQueryList(GenericQueryListDTO<PaSalImportBackdateDTO> request);

        Task<FormatedResponse> GetCurrentPeriodSalary();

        Task<FormatedResponse> GetEmployeeInfo(PaSalImportBackdateDTO param);

        Task<FormatedResponse> GetShiftDefault(PaSalImportBackdateDTO param);

        Task<FormatedResponse> GetListSalaryInYear(AtSalaryPeriodDTO param);

    }
}