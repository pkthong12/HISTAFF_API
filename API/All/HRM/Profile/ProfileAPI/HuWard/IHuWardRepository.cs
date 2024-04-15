using API.DTO;
using CORE.DTO;
using CORE.GenericUOW;

namespace API.Controllers.HuWard
{
    public interface IHuWardRepository: IGenericRepository<HU_WARD, HuWardDTO>
    {
        Task<GenericPhaseTwoListResponse<HuWardDTO>> SinglePhaseQueryList(GenericQueryListDTO<HuWardDTO> request);
		Task<FormatedResponse> CreateNewCode();

        Task<FormatedResponse> CheckActive(List<long> ids);

    }
}

