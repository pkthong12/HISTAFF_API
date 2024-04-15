using API.DTO;
using CORE.DTO;
using CORE.GenericUOW;

namespace API.All.HRM.Profile.ProfileAPI.Portal.PortalEducation
{
    public interface IPortalStaffProfileRepository : IGenericRepository<HU_EMPLOYEE_CV, HuEmployeeCvDTO>
    {
        Task<FormatedResponse> GetEducationByPortal(long employeeId, long? time);
        Task<FormatedResponse> GetEducationByPortalCorrect(long employeeId);
        Task<FormatedResponse> GetProfileInfoByPortal(long employeeId);
        Task<FormatedResponse> GetCurriculumByPortal(long employeeId);
        Task<FormatedResponse> UpdateCurriculumByPortal(HuEmployeeCvEditDTO request);
        Task<FormatedResponse> GetHuEmployeeCvEditCvApproving(long employeeId);
        Task<FormatedResponse> GetHuEmployeeCvSave(long employeeId);
        Task<FormatedResponse> GetHuEmployeeCvEditCvSaveById(long id);
        Task<FormatedResponse> GetHuEmployeeCvEditById(long id);
        Task<FormatedResponse> GetHuEmployeeCvUnapprove(long employeeId);
        Task<FormatedResponse> GetHuEmployeeCvUnapproveById(long id);
        Task<FormatedResponse> GetHuEmployeeCvEditCorrectById(long id);
        Task<FormatedResponse> GetHuEmployeeCvEditAdditionalInfoSaveEdit(long id);
        Task<FormatedResponse> GetBankInfoByEmployeeId(long employeeId);
        Task<FormatedResponse> GetHuEmployeeCvEditBankInfoApproving(long employeeId);
        Task<FormatedResponse> GetContactByEmployeeId(long employeeId);
        Task<FormatedResponse> GetHuEmployeeCvEditContactApproving(long employeeId);
        Task<FormatedResponse> GetHuEmployeeCvEditContactCorrect(long id);
        Task<FormatedResponse> GetHuEmployeeCvEditContactUnapprove(long employeeId);
        Task<FormatedResponse> GetHuEmployeeCvEditContactUnapproveById(long id);
        Task<FormatedResponse> GetHuEmployeeCvAdditionalInfoByEmployeeId(long employeeId, long? time);
        Task<FormatedResponse> GetHuEmployeeCvAdditionalInfoApproving(long employeeId, long? time);
        Task<FormatedResponse> GetHuEmployeeCvAdditionalInfoUnapprove(long employeeId, long? time);
        Task<FormatedResponse> GetHuEmployeeCvAdditionalInfoUnapproveById(long id);
        Task<FormatedResponse> GetInsuarenceInfoByEmployeeId(long employeeId);
        Task<FormatedResponse> GetHuEmployeeCvEditInsuarenceApproving(long employeeId);
        Task<FormatedResponse> GetHuEmployeeCvEditInsurenceSave(long employeeId);
        Task<FormatedResponse> GetInsurenceSaveById(long id);
        Task<FormatedResponse> GetHuEmployeeCvEditBankInfoSave(long employeeId);
        Task<FormatedResponse> GetHuEmployeeCvEditBankInfoById(long id);
        Task<FormatedResponse> GetHuEmployeeCvEditBankInfoUnapprove(long employeeId);
        Task<FormatedResponse> GetHuEmployeeCvEditBankInfoUnapproveById(long id);
        Task<FormatedResponse> GetHuEmployeeCvEditContactSave(long employeeId);
        Task<FormatedResponse> GetHuEmployeeCvEditContactSaveById(long id);
        Task<FormatedResponse> GetHuEmployeeCvEditAdditionalInfoSave(long employeeId, long? time);
        Task<FormatedResponse> GetHuEmployeeCvEditAddtionalInfoById(long id);


        Task<FormatedResponse> GetProfileInfoByPortal2(long employeeId);
    }
}
