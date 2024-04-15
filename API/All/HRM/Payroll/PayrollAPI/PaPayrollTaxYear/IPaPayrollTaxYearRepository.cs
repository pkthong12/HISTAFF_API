using API.DTO;
using CORE.DTO;
using CORE.GenericUOW;

namespace API.Controllers.PaPayrollTaxYear
{
    public interface IPaPayrollTaxYearRepository: IGenericRepository<PA_PAYROLL_TAX_YEAR, PaPayrollTaxYearDTO>
    {
       Task<GenericPhaseTwoListResponse<PaPayrollTaxYearDTO>> SinglePhaseQueryList(GenericQueryListDTO<PaPayrollTaxYearDTO> request);

        Task<FormatedResponse> GetDynamicName(PaPayrollTaxYearDTO param);

        Task<FormatedResponse> GetList(PaPayrollTaxYearDTO param);

        Task<FormatedResponse> HandleRequest(PaPayrollTaxYearDTO param);

        Task<FormatedResponse> CheckRequest(long id);

        Task<FormatedResponse> GetObjSalTaxGroup();
        Task<FormatedResponse> ChangeStatusParoxTaxYear(PaPayrollTaxYearDTO dto);
    }
}

