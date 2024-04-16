using API.DTO;
using CORE.DTO;
using CORE.GenericUOW;

namespace API.All.HRM.Asset.AsProject
{
    public interface IAsProjectRepository : IGenericRepository<AS_PROJECT, AsProjectDTO>
    {
        Task<GenericPhaseTwoListResponse<AsProjectDTO>> SinglePhaseQueryList(GenericQueryListDTO<AsProjectDTO> request);
    }
}