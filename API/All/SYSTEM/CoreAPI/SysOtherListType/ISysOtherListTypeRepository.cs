using API.DTO;
using CORE.DTO;
using CORE.GenericUOW;

namespace API.Controllers.SysOtherListType
{
    public interface ISysOtherListTypeRepository: IGenericRepository<SYS_OTHER_LIST_TYPE, SysOtherListTypeDTO>
    {
       Task<GenericPhaseTwoListResponse<SysOtherListTypeDTO>> SinglePhaseQueryList(GenericQueryListDTO<SysOtherListTypeDTO> request);
    }
}

