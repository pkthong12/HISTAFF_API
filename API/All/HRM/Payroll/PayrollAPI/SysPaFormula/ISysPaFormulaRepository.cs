using API.DTO;
using CORE.DTO;
using CORE.GenericUOW;

namespace API.Controllers.SysPaFormula
{
    public interface ISysPaFormulaRepository: IGenericRepository<SYS_PA_FORMULA, SysPaFormulaDTO>
    {
       Task<GenericPhaseTwoListResponse<SysPaFormulaDTO>> SinglePhaseQueryList(GenericQueryListDTO<SysPaFormulaDTO> request);

        Task<FormatedResponse> CheckFormula(SysPaFormulaDTO param);
    }
}

