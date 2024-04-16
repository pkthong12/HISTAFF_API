using API.DTO;
using CORE.DTO;
using CORE.GenericUOW;

namespace API.Controllers.InsListProgram
{
    public interface IInsListProgramRepository : IGenericRepository<INS_LIST_PROGRAM, InsListProgramDTO>
    {
        Task<GenericPhaseTwoListResponse<InsListProgramDTO>> SinglePhaseQueryList(GenericQueryListDTO<InsListProgramDTO> request);
    }
}