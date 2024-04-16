using API.DTO;
using CORE.DTO;
using CORE.GenericUOW;

namespace API.Controllers.TrProgramResult
{
    public interface ITrProgramResultRepository: IGenericRepository<TR_PROGRAM_RESULT, TrProgramResultDTO>
    {
       Task<GenericPhaseTwoListResponse<TrProgramResultDTO>> SinglePhaseQueryList(GenericQueryListDTO<TrProgramResultDTO> request);
    }
}

