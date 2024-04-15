using API.DTO;
using CORE.DTO;
using CORE.GenericUOW;

namespace API.Controllers.HuQuestion
{
    public interface IHuQuestionRepository: IGenericRepository<HU_QUESTION, HuQuestionDTO>
    {
       Task<GenericPhaseTwoListResponse<HuQuestionDTO>> SinglePhaseQueryList(GenericQueryListDTO<HuQuestionDTO> request);
    }
}

