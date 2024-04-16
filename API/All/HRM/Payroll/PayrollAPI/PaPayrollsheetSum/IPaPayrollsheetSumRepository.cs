using API.DTO;
using CORE.DTO;
using CORE.GenericUOW;

namespace API.Controllers.PaPayrollsheetSum
{
    public interface IPaPayrollsheetSumRepository : IGenericRepository<PA_PAYROLLSHEET_SUM, PaPayrollsheetSumDTO>
    {
        Task<GenericPhaseTwoListResponse<PaPayrollsheetSumDTO>> SinglePhaseQueryList(GenericQueryListDTO<PaPayrollsheetSumDTO> request);

        Task<FormatedResponse> GetDynamicName(PaPayrollsheetSumDTO param);

        Task<FormatedResponse> GetList(PaPayrollsheetSumDTO param);

        Task<FormatedResponse> ChangeStatusParox(PaPayrollsheetSumDTO dto);

        Task<FormatedResponse> HandleRequest(PaPayrollsheetSumDTO param);

        Task<FormatedResponse> CheckRequest(long id);

        Task<FormatedResponse> GetPayrollByEmployee(long id, long salaryPeriodId);

    }
}
