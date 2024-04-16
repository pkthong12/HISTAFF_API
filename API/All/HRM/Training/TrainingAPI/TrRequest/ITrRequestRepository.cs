using API.DTO;
using CORE.DTO;
using CORE.GenericUOW;

namespace API.Controllers.TrRequest
{
    public interface ITrRequestRepository: IGenericRepository<TR_REQUEST, TrRequestDTO>
    {
       Task<GenericPhaseTwoListResponse<TrRequestDTO>> SinglePhaseQueryList(GenericQueryListDTO<TrRequestDTO> request);
    }
}

