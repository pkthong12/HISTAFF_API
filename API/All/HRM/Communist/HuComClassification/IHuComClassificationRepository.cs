using API.DTO;
using CORE.DTO;
using CORE.GenericUOW;

namespace API.Controllers.HuComClassification
{
    public interface IHuComClassificationRepository: IGenericRepository<HU_COM_CLASSIFICATION, HuComClassificationDTO>
    {
       Task<GenericPhaseTwoListResponse<HuComClassificationDTO>> SinglePhaseQueryList(GenericQueryListDTO<HuComClassificationDTO> request);
    }
}

