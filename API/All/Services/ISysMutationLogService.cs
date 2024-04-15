using API.Main;
using CORE.DTO;

namespace API.All.Services
{
    public interface ISysMutationLogService
    {
        Task<FormatedResponse> Write(SysMutationLogBeforeAfterRequest request, string sid);
    }
}
