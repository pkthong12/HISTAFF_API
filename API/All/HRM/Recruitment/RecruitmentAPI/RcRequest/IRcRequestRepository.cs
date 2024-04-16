using API.DTO;
using CORE.DTO;
using CORE.GenericUOW;

namespace API.All.HRM.Recruitment.RecruitmentAPI.RcRequest
{
    public interface IRcRequestRepository : IGenericRepository<RC_REQUEST, RcRequestDTO>
    {
        Task<GenericPhaseTwoListResponse<RcRequestDTO>> SinglePhaseQueryList(GenericQueryListDTO<RcRequestDTO> request);
        Task<FormatedResponse> GetDropDownRecruitmentForm(string code);
        Task<FormatedResponse> GetWorkingAddressAccordingToCompany(long orgId);
        Task<FormatedResponse> GetDropDownRecruitmentReason(string code);
        Task<FormatedResponse> ReadWorkAddress(long id);
    }
}