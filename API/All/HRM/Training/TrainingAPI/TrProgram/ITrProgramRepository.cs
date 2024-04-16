using API.DTO;
using CORE.DTO;
using CORE.GenericUOW;

namespace API.Controllers.TrProgram
{
    public interface ITrProgramRepository: IGenericRepository<TR_PROGRAM, TrProgramDTO>
    {
       Task<GenericPhaseTwoListResponse<TrProgramDTO>> SinglePhaseQueryList(GenericQueryListDTO<TrProgramDTO> request);

        Task<FormatedResponse> GetListProgram();
    }
}

