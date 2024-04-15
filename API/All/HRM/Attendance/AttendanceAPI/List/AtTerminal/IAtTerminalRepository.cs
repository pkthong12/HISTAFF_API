using API.DTO;
using CORE.DTO;
using CORE.GenericUOW;

namespace API.Controllers.AtTerminal
{
    public interface IAtTerminalRepository: IGenericRepository<AT_TERMINAL, AtTerminalDTO>
    {
       Task<GenericPhaseTwoListResponse<AtTerminalDTO>> SinglePhaseQueryList(GenericQueryListDTO<AtTerminalDTO> request);

        Task<FormatedResponse> CreateNewCode();
    }
}

