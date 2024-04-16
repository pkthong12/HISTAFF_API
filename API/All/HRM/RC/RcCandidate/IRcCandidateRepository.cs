using API.DTO;
using CORE.DTO;
using CORE.GenericUOW;

namespace API.Controllers.RcCandidate
{
    public interface IRcCandidateRepository: IGenericRepository<RC_CANDIDATE, RcCandidateDTO>
    {
       Task<GenericPhaseTwoListResponse<RcCandidateDTO>> SinglePhaseQueryList(GenericQueryListDTO<RcCandidateDTO> request);
       Task<FormatedResponse> InsertProfileRecruitment(GenericUnitOfWork _uow, CandidateEditDTO request, string sid);

    }
}

