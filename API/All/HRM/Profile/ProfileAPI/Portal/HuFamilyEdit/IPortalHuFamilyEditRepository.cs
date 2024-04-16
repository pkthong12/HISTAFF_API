using API.DTO;
using CORE.DTO;
using CORE.GenericUOW;

namespace API.Controllers.HuFamilyEdit
{
    public interface IPortalHuFamilyEditRepository: IGenericRepository<HU_FAMILY_EDIT, HuFamilyEditDTO>
    {
       Task<GenericPhaseTwoListResponse<HuFamilyEditDTO>> SinglePhaseQueryList(GenericQueryListDTO<HuFamilyEditDTO> request);
       Task<FormatedResponse> GetHuFamilyEditNotApproved(long employeeId);
       Task<FormatedResponse> GetHuFamilyEditSave(long employeeId);
       Task<FormatedResponse> GetHuFamilyEditSaveById(long id);
       Task<FormatedResponse> GetHuFamilyEditByIdCorrect(long id);
       Task<FormatedResponse> GetHuFamilyEditRefuse(long employeeId);
    }
}

