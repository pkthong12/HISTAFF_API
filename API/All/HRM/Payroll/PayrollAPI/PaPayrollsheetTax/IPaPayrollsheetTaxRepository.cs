using API.DTO;
using CORE.DTO;
using CORE.GenericUOW;

namespace API.Controllers.PaPayrollsheetTax
{
    public interface IPaPayrollsheetTaxRepository: IGenericRepository<PA_PAYROLLSHEET_TAX, PaPayrollsheetTaxDTO>
    {
       Task<GenericPhaseTwoListResponse<PaPayrollsheetTaxDTO>> SinglePhaseQueryList(GenericQueryListDTO<PaPayrollsheetTaxDTO> request);
        Task<FormatedResponse> GetDynamicName(PaPayrollsheetTaxDTO param);

        Task<FormatedResponse> GetList(PaPayrollsheetTaxDTO param);

        Task<FormatedResponse> ChangeStatusParoxTaxMonth(PaPayrollsheetTaxDTO dto);

        Task<FormatedResponse> HandleRequest(PaPayrollsheetTaxDTO param);

        Task<FormatedResponse> CheckRequest(long id);

        Task<FormatedResponse> GetTaxDate(PaPayrollsheetTaxDTO param);

        Task<FormatedResponse> GetMonth(PaPayrollsheetTaxDTO param);
        Task<FormatedResponse> GetPeriodId(long year, long month);

        Task<FormatedResponse> GetObjSal();

    }
}

