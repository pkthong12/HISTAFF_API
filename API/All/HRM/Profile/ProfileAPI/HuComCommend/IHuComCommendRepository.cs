using API.DTO;
using CORE.DTO;
using CORE.GenericUOW;

namespace API.Controllers.HuCommend
{
    public interface IHuComCommendRepository: IGenericRepository<HU_COM_COMMEND, HuComCommendDTO>
    {
       Task<GenericPhaseTwoListResponse<HuComCommendDTO>> SinglePhaseQueryList(GenericQueryListDTO<HuComCommendDTO> request);
       Task<FormatedResponse> ApproveCommend(List<long> Ids);
    }
}

