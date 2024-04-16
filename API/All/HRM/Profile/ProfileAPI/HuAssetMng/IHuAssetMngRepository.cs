using API.DTO;
using CORE.DTO;
using CORE.GenericUOW;

namespace API.Controllers.HuAssetMng
{
    public interface IHuAssetMngRepository: IGenericRepository<HU_ASSET_MNG, HuAssetMngDTO>
    {
       Task<GenericPhaseTwoListResponse<HuAssetMngDTO>> SinglePhaseQueryList(GenericQueryListDTO<HuAssetMngDTO> request);
        Task<FormatedResponse> GetAsset();
        Task<FormatedResponse> GetStatusAsset();
    }
}

