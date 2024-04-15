using API.DTO;
using CORE.DTO;
using CORE.GenericUOW;

namespace API.Controllers.HuEmployeeCvEdit
{
    public interface IApproveHuEmployeeCvEditRepository : IGenericRepository<HU_EMPLOYEE_CV_EDIT, HuEmployeeCvEditDTO>
    {
        Task<GenericPhaseTwoListResponse<HuEmployeeCvEditDTO>> SinglePhaseQueryList(GenericQueryListDTO<HuEmployeeCvEditDTO> request);
        Task<GenericPhaseTwoListResponse<HuEmployeeCvEditDTO>> QueryListCvEdit(GenericQueryListDTO<HuEmployeeCvEditDTO> request);
        Task<FormatedResponse> ApproveCvEdit(GenericUnapprovePortalDTO model);
        Task<GenericPhaseTwoListResponse<HuEmployeeCvEditDTO>> QueryListContactEdit(GenericQueryListDTO<HuEmployeeCvEditDTO> request);
        Task<FormatedResponse> ApproveContactEdit(GenericUnapprovePortalDTO model);
        Task<GenericPhaseTwoListResponse<HuEmployeeCvEditDTO>> QueryListAdditionalInfoEdit(GenericQueryListDTO<HuEmployeeCvEditDTO> request);
        Task<FormatedResponse> ApproveAdditionalEdit(GenericUnapprovePortalDTO model);
        Task<GenericPhaseTwoListResponse<HuEmployeeCvEditDTO>> QueryListBankInfoEdit(GenericQueryListDTO<HuEmployeeCvEditDTO> request);
        Task<FormatedResponse> ApproveBankInfoEdit(GenericUnapprovePortalDTO model, string sid);
        Task<GenericPhaseTwoListResponse<HuEmployeeCvEditDTO>> QueryListEducationEdit(GenericQueryListDTO<HuEmployeeCvEditDTO> request);
        Task<FormatedResponse> ApproveEducationEdit(GenericUnapprovePortalDTO model);

    }
}

