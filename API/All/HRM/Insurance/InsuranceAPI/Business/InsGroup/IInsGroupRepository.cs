using API.DTO;
using CORE.DTO;
using CORE.GenericUOW;

namespace API.Controllers.InsGroup
{
    public interface IInsGroupRepository : IGenericRepository<INS_GROUP, InsGroupDTO>
    {
        Task<GenericPhaseTwoListResponse<InsGroupDTO>> SinglePhaseQueryList(GenericQueryListDTO<InsGroupDTO> request);
        Task<FormatedResponse> CreateNewCode();
    }
}

