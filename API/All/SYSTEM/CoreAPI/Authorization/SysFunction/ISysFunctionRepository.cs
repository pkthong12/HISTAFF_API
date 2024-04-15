using API.All.SYSTEM.CoreAPI.Authorization.SysFunction;
using API.DTO;
using CORE.DTO;
using CORE.GenericUOW;

namespace API.Controllers.SysFunction
{
    public interface ISysFunctionRepository : IGenericRepository<SYS_FUNCTION, SysFunctionDTO>
    {
        Task<GenericPhaseTwoListResponse<SysFunctionDTO>> SinglePhaseQueryList(GenericQueryListDTO<SysFunctionDTO> request);
        Task<GenericPhaseTwoListResponse<SysFunctionDTO>> FunctionPermissionList(GenericQueryListDTO<SysFunctionDTO> request);
        Task<FormatedResponse> CreateFunctionThenUpdateFunctionIdForMenu(CreateUpdateFunctionThenAsignFunctionIdToMenuDTO request, string sid);
        Task<FormatedResponse> UpdateFunctionThenUpdateFunctionIdForMenu(CreateUpdateFunctionThenAsignFunctionIdToMenuDTO request, string sid);
        Task<FormatedResponse> ReadAllWithAllActions();
    }
}

