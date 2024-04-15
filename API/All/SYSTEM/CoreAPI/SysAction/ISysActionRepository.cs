using API.DTO;
using CORE.DTO;
using CORE.GenericUOW;

namespace API.Controllers.SysAction
{
    public interface ISysActionRepository : IGenericRepository<SYS_ACTION, SysActionDTO>
    {
        Task<GenericPhaseTwoListResponse<SysActionDTO>> SinglePhaseQueryList(GenericQueryListDTO<SysActionDTO> request);
        Task<FormatedResponse> GetAll();
    }
}

