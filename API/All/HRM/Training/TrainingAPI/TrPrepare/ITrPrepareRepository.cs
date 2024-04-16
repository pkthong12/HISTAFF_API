using API.DTO;
using CORE.DTO;
using CORE.GenericUOW;

namespace API.Controllers.TrPrepare
{
    public interface ITrPrepareRepository: IGenericRepository<TR_PREPARE, TrPrepareDTO>
    {
       Task<GenericPhaseTwoListResponse<TrPrepareDTO>> SinglePhaseQueryList(GenericQueryListDTO<TrPrepareDTO> request);
    }
}

