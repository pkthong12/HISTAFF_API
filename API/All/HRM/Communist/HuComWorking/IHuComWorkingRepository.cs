using API.DTO;
using CORE.DTO;
using CORE.GenericUOW;

namespace API.Controllers.HuComWorking
{
    public interface IHuComWorkingRepository: IGenericRepository<HU_COM_WORKING, HuComWorkingDTO>
    {
       Task<GenericPhaseTwoListResponse<HuComWorkingDTO>> SinglePhaseQueryList(GenericQueryListDTO<HuComWorkingDTO> request);
    }
}

