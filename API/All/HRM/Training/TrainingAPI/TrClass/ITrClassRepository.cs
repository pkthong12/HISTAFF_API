using API.DTO;
using CORE.DTO;
using CORE.GenericUOW;

namespace API.Controllers.TrClass
{
    public interface ITrClassRepository: IGenericRepository<TR_CLASS, TrClassDTO>
    {
       Task<GenericPhaseTwoListResponse<TrClassDTO>> SinglePhaseQueryList(GenericQueryListDTO<TrClassDTO> request);
    }
}

