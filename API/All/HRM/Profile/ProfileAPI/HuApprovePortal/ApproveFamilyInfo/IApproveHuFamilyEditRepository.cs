using API.DTO;
using CORE.DTO;
using CORE.GenericUOW;

namespace API.Controllers.HuFamilyEdit
{
    public interface IApproveHuFamilyEditRepository: IGenericRepository<HU_FAMILY_EDIT, HuFamilyEditDTO>
    {
       Task<GenericPhaseTwoListResponse<HuFamilyEditDTO>> SinglePhaseQueryList(GenericQueryListDTO<HuFamilyEditDTO> request);
       Task<FormatedResponse> GetAllHuFamilyEdit();
       Task<FormatedResponse> ApproveHuFamilyEdit(GenericUnapprovePortalDTO request);
    }
}

