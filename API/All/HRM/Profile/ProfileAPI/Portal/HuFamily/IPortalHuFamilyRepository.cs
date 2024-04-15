using API.DTO;
using CORE.DTO;
using CORE.GenericUOW;

namespace API.Controllers.HuFamily
{
    public interface IPortalHuFamilyRepository: IGenericRepository<HU_FAMILY, HuFamilyDTO>
    {
       Task<GenericPhaseTwoListResponse<HuFamilyDTO>> SinglePhaseQueryList(GenericQueryListDTO<HuFamilyDTO> request);
       Task<FormatedResponse> GetAllFamilyByEmployee(long employeeId);
       //Task<FormatedResponse> ApproveHuFamilyEdit(List<long> ids);
    }
}

