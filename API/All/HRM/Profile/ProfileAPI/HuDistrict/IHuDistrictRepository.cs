using API.DTO;
using CORE.DTO;
using CORE.GenericUOW;

namespace API.Controllers.HuDistrict
{
    public interface IHuDistrictRepository: IGenericRepository<HU_DISTRICT, HuDistrictDTO>
    {
        Task<GenericPhaseTwoListResponse<HuDistrictDTO>> SinglePhaseQueryList(GenericQueryListDTO<HuDistrictDTO> request);

		Task<FormatedResponse> CreateNewCode();
        Task<FormatedResponse> CheckActive(List<long> ids);

    }
}

