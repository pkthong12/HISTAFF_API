using API.DTO;
using CORE.DTO;
using CORE.GenericUOW;

namespace API.Controllers.HuAnswer
{
    public interface IHuAnswerRepository: IGenericRepository<HU_ANSWER, HuAnswerDTO>
    {
       Task<GenericPhaseTwoListResponse<HuAnswerDTO>> SinglePhaseQueryList(GenericQueryListDTO<HuAnswerDTO> request);
    }
}

