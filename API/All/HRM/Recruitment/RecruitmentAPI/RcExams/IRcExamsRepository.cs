using API.DTO;
using CORE.DTO;
using CORE.GenericUOW;

namespace API.All.HRM.Recruitment.RecruitmentAPI.RcExams
{
    public interface IRcExamsRepository : IGenericRepository<RC_EXAMS, RcExamsDTO>
    {
        Task<GenericPhaseTwoListResponse<RcExamsDTO>> SinglePhaseQueryList(GenericQueryListDTO<RcExamsDTO> request);
        Task<FormatedResponse> GetPositionIsEmptyOwner(long orgId);
    }
}