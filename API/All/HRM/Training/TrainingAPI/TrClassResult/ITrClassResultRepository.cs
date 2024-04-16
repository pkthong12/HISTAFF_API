using API.DTO;
using CORE.DTO;
using CORE.GenericUOW;

namespace API.Controllers.TrClassResult
{
    public interface ITrClassResultRepository: IGenericRepository<TR_CLASS_RESULT, TrClassResultDTO>
    {
       Task<GenericPhaseTwoListResponse<TrClassResultDTO>> SinglePhaseQueryList(GenericQueryListDTO<TrClassResultDTO> request);
    }
}

