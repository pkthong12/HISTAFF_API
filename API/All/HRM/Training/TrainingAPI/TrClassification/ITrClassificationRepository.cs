using API.DTO;
using CORE.DTO;
using CORE.GenericUOW;

namespace API.Controllers.TrClassification
{
    public interface ITrClassificationRepository: IGenericRepository<TR_CLASSIFICATION, TrClassificationDTO>
    {
       Task<GenericPhaseTwoListResponse<TrClassificationDTO>> SinglePhaseQueryList(GenericQueryListDTO<TrClassificationDTO> request);
    }
}

