using API.DTO;
using CORE.DTO;
using CORE.GenericUOW;

namespace API.Controllers.AtSwipeData
{
    public interface IAtSwipeDataRepository: IGenericRepository<AT_SWIPE_DATA, AtSwipeDataDTO>
    {
       Task<GenericPhaseTwoListResponse<AtSwipeDataDTO>> SinglePhaseQueryList(GenericQueryListDTO<AtSwipeDataDTO> request);
    }
}

