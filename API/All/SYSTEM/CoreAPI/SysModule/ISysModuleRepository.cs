using API.DTO;
using CORE.DTO;
using CORE.GenericUOW;

namespace API.Controllers.SysModule
{
    public interface ISysModuleRepository: IGenericRepository<SYS_MODULE, SysModuleDTO>
    {
       Task<GenericPhaseTwoListResponse<SysModuleDTO>> SinglePhaseQueryList(GenericQueryListDTO<SysModuleDTO> request);
    }
}

