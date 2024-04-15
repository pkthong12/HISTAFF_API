using API.DTO;
using CORE.DTO;
using CORE.GenericUOW;

namespace API.Controllers.SeProcess
{
    public interface ISeProcessRepository : IGenericRepository<SE_PROCESS, SeProcessDTO>
    {
        Task<GenericPhaseTwoListResponse<SeProcessDTO>> SinglePhaseQueryList(GenericQueryListDTO<SeProcessDTO> request);
        Task<FormatedResponse> CreateNewCode();
        Task<FormatedResponse> GetProcessType();
        Task<FormatedResponse> GetProcessTypeById(long id);
    }
}

