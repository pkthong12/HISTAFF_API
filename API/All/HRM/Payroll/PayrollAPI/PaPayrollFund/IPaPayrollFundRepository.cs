using API.DTO;
using CORE.DTO;
using CORE.GenericUOW;

namespace API.Controllers.PaPayrollFund
{
    public interface IPaPayrollFundRepository : IGenericRepository<PA_PAYROLL_FUND, PaPayrollFundDTO>
    {
        Task<GenericPhaseTwoListResponse<PaPayrollFundDTO>> SinglePhaseQueryList(GenericQueryListDTO<PaPayrollFundDTO> request);
        Task<FormatedResponse> GetCompany();
        Task<FormatedResponse> GetListFundSource(long id);
        Task<FormatedResponse> GetListFund(long id);
        Task<FormatedResponse> GetMonth(long year);
    }
}

