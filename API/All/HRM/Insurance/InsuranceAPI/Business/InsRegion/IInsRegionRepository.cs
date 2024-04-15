using API.DTO;
using CORE.DTO;
using CORE.GenericUOW;

namespace API.Controllers.InsRegion
{
    public interface IInsRegionRepository : IGenericRepository<INS_REGION, InsRegionDTO>
    {
        Task<GenericPhaseTwoListResponse<InsRegionDTO>> SinglePhaseQueryList(GenericQueryListDTO<InsRegionDTO> request);

        Task<FormatedResponse> GetSalaryBasicByRegion(string code);
        Task<FormatedResponse> GetSysOrtherList();
        Task<FormatedResponse> GetRegionByDateNow();
    }
}

