using API.DTO;
using CORE.DTO;
using CORE.GenericUOW;

namespace API.Controllers.InsSpecifiedObjects
{
    public interface IInsSpecifiedObjectsRepository: IGenericRepository<INS_SPECIFIED_OBJECTS, InsSpecifiedObjectsDTO>
    {
       Task<GenericPhaseTwoListResponse<InsSpecifiedObjectsDTO>> SinglePhaseQueryList(GenericQueryListDTO<InsSpecifiedObjectsDTO> request);
    }
}

