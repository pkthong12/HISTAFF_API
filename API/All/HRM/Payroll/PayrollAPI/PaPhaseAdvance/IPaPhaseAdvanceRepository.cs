using API.DTO;
using CORE.DTO;
using CORE.GenericUOW;

namespace API.Controllers.PaPhaseAdvance
{
    public interface IPaPhaseAdvanceRepository: IGenericRepository<PA_PHASE_ADVANCE, PaPhaseAdvanceDTO>
    {
       Task<GenericPhaseTwoListResponse<PaPhaseAdvanceDTO>> SinglePhaseQueryList(GenericQueryListDTO<PaPhaseAdvanceDTO> request);

        Task<FormatedResponse> GetYearPeriod();
        Task<FormatedResponse> GetOrgId();
        Task<FormatedResponse> GetMonthPeriodAt(int year);
        Task<FormatedResponse> GetOtherListSal();
        Task<FormatedResponse> GetAtSymbol();
        Task<FormatedResponse> GetYearByPeriod(long id);
    }
}

