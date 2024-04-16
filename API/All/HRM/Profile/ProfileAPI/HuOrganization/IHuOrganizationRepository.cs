using API.All.HRM.Profile.ProfileAPI.HuOrganization;
using API.DTO;
using Common.Interfaces;
using CORE.DTO;
using CORE.GenericUOW;

namespace API.Controllers.HuOrganization
{
    public interface IHuOrganizationRepository : IRepository<HU_ORGANIZATION>//IGenericRepository<HU_ORGANIZATION, HuOrganizationDTO>
    {
        Task<GenericPhaseTwoListResponse<HuOrganizationDTO>> SinglePhaseQueryList(GenericQueryListDTO<HuOrganizationDTO> request);
        Task<List<HuOrganizationTreeBlockDTO>> BuildOrgTree(string sid);
        Task<List<HuOrganizationTreeBlockPositionDTO>> BuildOrgPositionTree(string sid);
        Task<bool> Dissolution(HU_ORGANIZATION obj);
        void ScanDissolveOrg();
    }
}

