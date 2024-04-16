using API.DTO;
using CORE.DTO;
using CORE.GenericUOW;

namespace API.Controllers.HuAsset
{
    public interface IHuAssetRepository: IGenericRepository<HU_ASSET, HuAssetDTO>
    {
       Task<GenericPhaseTwoListResponse<HuAssetDTO>> SinglePhaseQueryList(GenericQueryListDTO<HuAssetDTO> request);
        Task<FormatedResponse> GetGroupAsset();
    }
}

