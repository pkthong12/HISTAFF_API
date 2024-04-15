using API.DTO;
using CORE.DTO;
using CORE.GenericUOW;

namespace API.All.SYSTEM.CoreAPI.SysConfigurationCommon
{
    public interface ISysConfigurationCommonRepository : IGenericRepository<SYS_CONFIGURATION_COMMON, SysConfigurationCommonDTO>
    {
        Task<GenericPhaseTwoListResponse<SysConfigurationCommonDTO>> SinglePhaseQueryList(GenericQueryListDTO<SysConfigurationCommonDTO> request);
    }
}