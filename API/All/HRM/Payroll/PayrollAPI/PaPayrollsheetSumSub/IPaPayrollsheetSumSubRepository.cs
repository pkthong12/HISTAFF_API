using API.DTO;
using CORE.DTO;
using CORE.GenericUOW;

namespace API.Controllers.PaPayrollsheetSumSub
{
    public interface IPaPayrollsheetSumSubRepository: IGenericRepository<PA_PAYROLLSHEET_SUM_SUB, PaPayrollsheetSumSubDTO>
    {
       Task<GenericPhaseTwoListResponse<PaPayrollsheetSumSubDTO>> SinglePhaseQueryList(GenericQueryListDTO<PaPayrollsheetSumSubDTO> request);

        Task<FormatedResponse> GetDynamicName(PaPayrollsheetSumSubDTO param);

        Task<FormatedResponse> GetList(PaPayrollsheetSumSubDTO param);

        Task<FormatedResponse> ChangeStatusParoxSub(PaPayrollsheetSumSubDTO dto);

        Task<FormatedResponse> HandleRequest(PaPayrollsheetSumSubDTO param);

        Task<FormatedResponse> CheckRequest(long id);

        Task<FormatedResponse> GetPhaseAdvance(PaPayrollsheetSumSubDTO param);
        Task<FormatedResponse> GetPhaseAdvanceById(long id);

        Task<FormatedResponse> GetObjSalPayrollSubGroup();
    }
}

