using API.DTO;
using CORE.DTO;
using CORE.GenericUOW;

namespace API.Controllers.RcHrYearPlaning
{
    public interface IRcHrYearPlaningRepository: IGenericRepository<RC_HR_YEAR_PLANING, RcHrYearPlaningDTO>
    {
       Task<GenericPhaseTwoListResponse<RcHrYearPlaningDTO>> SinglePhaseQueryList(GenericQueryListDTO<RcHrYearPlaningDTO> request);
        Task<FormatedResponse> GetAllPlaning(long? id);


    }
}

