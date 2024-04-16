using API.DTO;
using CORE.DTO;
using CORE.GenericUOW;

namespace API.All.HRM.Training.TrainingAPI.TrProgramCommit
{
    public interface ITrProgramCommitRepository : IGenericRepository<TR_PROGRAM_COMMIT, TrProgramCommitDTO>
    {
        Task<GenericPhaseTwoListResponse<TrProgramCommitDTO>> SinglePhaseQueryList(GenericQueryListDTO<TrProgramCommitDTO> request);
    }
}