using API.DTO;
using CORE.DTO;
using CORE.GenericUOW;

namespace API.Controllers.HuNation
{
    public interface IHuNationRepository: IGenericRepository<HU_NATION, HuNationDTO>
    {
        Task<GenericPhaseTwoListResponse<HuNationDTO>> SinglePhaseQueryList(GenericQueryListDTO<HuNationDTO> request);

		Task<FormatedResponse> CreateNewCode();

        Task<FormatedResponse> CheckActive(List<long> ids);

    }
}

