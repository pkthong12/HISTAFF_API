using API.DTO;
using CORE.DTO;
using CORE.GenericUOW;

namespace API.Controllers.PaPayrollsheetSumBackdate
{
    public interface IPaPayrollsheetSumBackdateRepository: IGenericRepository<PA_PAYROLLSHEET_SUM_BACKDATE, PaPayrollsheetSumBackdateDTO>
    {
       Task<GenericPhaseTwoListResponse<PaPayrollsheetSumBackdateDTO>> SinglePhaseQueryList(GenericQueryListDTO<PaPayrollsheetSumBackdateDTO> request);

        Task<FormatedResponse> GetDynamicName(PaPayrollsheetSumBackdateDTO param);

        Task<FormatedResponse> GetList(PaPayrollsheetSumBackdateDTO param);

        Task<FormatedResponse> ChangeStatusParoxBackdate(PaPayrollsheetSumBackdateDTO dto);

        Task<FormatedResponse> HandleRequest(PaPayrollsheetSumBackdateDTO param);

        Task<FormatedResponse> CheckRequest(long id);

        Task<FormatedResponse> GetNextPeriod(long periodId);

        Task<FormatedResponse> GetListSalaryInYear(PaPayrollsheetSumBackdateDTO param);
        Task<FormatedResponse> GetObjSalPayrollBackdateGroup();
    }
}

