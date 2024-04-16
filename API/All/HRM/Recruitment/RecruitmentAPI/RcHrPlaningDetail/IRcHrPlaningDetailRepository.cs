using API.DTO;
using CORE.DTO;
using CORE.GenericUOW;

namespace API.Controllers.RcHrPlaningDetail
{
    public interface IRcHrPlaningDetailRepository : IGenericRepository<RC_HR_PLANING_DETAIL, RcHrPlaningDetailDTO>
    {
        Task<GenericPhaseTwoListResponse<RcHrPlaningDetailDTO>> SinglePhaseQueryList(GenericQueryListDTO<RcHrPlaningDetailDTO> request);
        Task<FormatedResponse> GetAllPositionByOrgs(RcHrPlaningDetailServiceDTO model);
    }
}

