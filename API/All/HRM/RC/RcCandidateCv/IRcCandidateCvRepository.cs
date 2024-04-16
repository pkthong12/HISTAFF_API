using API.DTO;
using CORE.DTO;
using CORE.GenericUOW;

namespace API.Controllers.RcCandidateCv
{
    public interface IRcCandidateCvRepository: IGenericRepository<RC_CANDIDATE_CV, RcCandidateCvDTO>
    {
       Task<GenericPhaseTwoListResponse<RcCandidateCvDTO>> SinglePhaseQueryList(GenericQueryListDTO<RcCandidateCvDTO> request);
       Task<FormatedResponse> GetListPos();
        Task<FormatedResponse> GetCv(long employeeCvId);
        Task<FormatedResponse> GetCvById(long id);
        Task<FormatedResponse> UpdateCv(CandidateEditDTO request);
        Task<FormatedResponse> GetLevelInfo(long employeeCvId);
        Task<FormatedResponse> GetLevelInfoById(long id);
        Task<FormatedResponse> UpdateLevelInfo(CandidateEditDTO request);
        Task<FormatedResponse> GetWish(long employeeCvId);
        Task<FormatedResponse> GetWishById(long id);
        Task<FormatedResponse> UpdateWish(CandidateEditDTO request);
        Task<FormatedResponse> GetInfoOther(long employeeCvId);
        Task<FormatedResponse> GetInfoOtherById(long id);
        Task<FormatedResponse> UpdateInfoOther(CandidateEditDTO request);
    }
}

