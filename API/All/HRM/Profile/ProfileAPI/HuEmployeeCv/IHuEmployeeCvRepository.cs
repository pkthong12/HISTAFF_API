using API.DTO;
using CORE.DTO;
using CORE.GenericUOW;
using System.Data;

namespace API.Controllers.HuEmployeeCv
{
    public interface IHuEmployeeCvRepository : IGenericRepository<HU_EMPLOYEE_CV, HuEmployeeCvDTO>
    {
        Task<GenericPhaseTwoListResponse<HuEmployeeCvDTO>> SinglePhaseQueryList(GenericQueryListDTO<HuEmployeeCvDTO> request);
        Task<FormatedResponse> GetBankInfo(long employeeCvId);
        Task<FormatedResponse> GetBank(long id);
        Task<FormatedResponse> GetBasic(long employeeId);
        Task<FormatedResponse> GetCv(long employeeCvId);
        Task<FormatedResponse> GetGeneralInfo(long id);
        Task<FormatedResponse> CheckSameName(string name);
        Task<FormatedResponse> CheckSameIdNo(string idNo);
        Task<FormatedResponse> CheckSameTaxCode(string taxCode);
        Task<FormatedResponse> CheckSameItimeid(string itimeId);
        Task<FormatedResponse> GetPolitical(long id);
        Task<FormatedResponse> UpdatePolitical(StaffProfileUpdateDTO request, string sid);
        Task<FormatedResponse> UpdatePoliticalOrganizationId(StaffProfileUpdateDTO request, string sid);
        Task<FormatedResponse> UpdateBank(StaffProfileUpdateDTO request, string sid);
        Task<FormatedResponse> GetPoliticalOrganization(long employeeCvId);
        Task<FormatedResponse> GetPoliticalOrganizationId(long id);
        Task<FormatedResponse> UpdateGeneralInfo(StaffProfileUpdateDTO request, string sid);
        Task<FormatedResponse> GetEmployeeStatusList();
        Task<FormatedResponse> InsertStaffProfile(StaffProfileEditDTO request, string sid);
        Task<FormatedResponse> CheckPositionMasterInterim(long? id);
        Task<FormatedResponse> UpdateAvatar(StaffProfileEditDTO request, string sid);
        //Task<FormatedResponse> GetCode(string code);
        Task<FormatedResponse> GetAdditonalInfo(long employeeCvId);
        Task<FormatedResponse> GetPoliticalBackground(long employeeCvId);

        Task<FormatedResponse> GetPapers(long employeeCvId);

        Task<FormatedResponse> UpdatePapers(DynamicDTO model, string sid);
        Task<FormatedResponse> GetCurruculum(long id);
        Task<FormatedResponse> UpdateCurruculum(StaffProfileUpdateDTO request, string sid);
        Task<FormatedResponse> UpdateAdditonal(StaffProfileUpdateDTO request, string sid);
        Task<FormatedResponse> GetAdditonal(long id);
        Task<FormatedResponse> GetEducation(long employeeCvId);
        Task<FormatedResponse> GetEducationId(long id);
        Task<FormatedResponse> UpdateEducationId(StaffProfileUpdateDTO request, string sid);

        Task<FormatedResponse> GetPresenter(long employeeCvId);
        Task<FormatedResponse> GetPresenterId(long id);
        Task<FormatedResponse> GetSituation(long employeeCvId);
        Task<FormatedResponse> GetSituationId(long id);
        Task<FormatedResponse> UpdatePresenterId(StaffProfileUpdateDTO request, string sid);
        Task<FormatedResponse> GetContact(long employeeCvId);
        Task<FormatedResponse> GetContactId(long id);
        Task<FormatedResponse> UpdateContactId(StaffProfileUpdateDTO request, string sid);
        Task<FormatedResponse> UpdateSituationId(StaffProfileUpdateDTO request, string sid);
        Task<FormatedResponse> GetAll();

        Task<FormatedResponse> GetAllIgnoreCurrentUser(string sid);
        Task<FormatedResponse> GetGenderDashboard(HuEmployeeCvInputDTO? model);
        Task<FormatedResponse> GetEmpMonthDashboard(HuEmployeeCvInputDTO? model);
        Task<FormatedResponse> GetEmpSeniorityDashboard(HuEmployeeCvInputDTO? model);
        Task<FormatedResponse> GetGeneralInfomationDashboard(HuEmployeeCvInputDTO? model);
        Task<FormatedResponse> GetNativeInfomationDashboard(HuEmployeeCvInputDTO? model);
        Task<FormatedResponse> GetIsMemberInfomationDashboard(HuEmployeeCvInputDTO? model);
        Task<FormatedResponse> GetJobInfomationDashboard(HuEmployeeCvInputDTO? model);
        Task<FormatedResponse> GetPositionInfomationDashboard(HuEmployeeCvInputDTO? model);
        Task<FormatedResponse> GetLevelInfomationDashboard(HuEmployeeCvInputDTO? model);
        Task<FormatedResponse> GetWorkingAgeInfomationDashboard(HuEmployeeCvInputDTO? model);
        Task<FormatedResponse> GetNewEmpMonthDashboard(HuEmployeeCvInputDTO? model);
        Task<FormatedResponse> GetNameOrgDashboard(HuEmployeeCvInputDTO? model);
        Task<FormatedResponse> GetIdOrgDissolve(HuEmployeeCvInputDTO? model);
    }
}

