using API.DTO;
using CORE.DTO;
using CORE.GenericUOW;

namespace API.Controllers.AtOtherList
{
    public interface IAtOtherListRepository: IGenericRepository<AT_OTHER_LIST, AtOtherListDTO>
    {
       Task<GenericPhaseTwoListResponse<AtOtherListDTO>> SinglePhaseQueryList(GenericQueryListDTO<AtOtherListDTO> request);
    }
}

