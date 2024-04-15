using API.DTO;
using CORE.DTO;
using CORE.GenericUOW;

namespace API.Controllers.HuCommend
{
    public interface IHuCommendRepository: IGenericRepository<HU_COMMEND, HuCommendDTO>
    {
       Task<GenericPhaseTwoListResponse<HuCommendDTO>> SinglePhaseQueryList(GenericQueryListDTO<HuCommendDTO> request);
       Task<FormatedResponse> ApproveCommend(List<long> Ids);
    }
}

