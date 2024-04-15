using ProfileDAL.ViewModels;
using Common.Interfaces;
using Common.Paging;
using Common.Extensions;
using API.Entities;
using CORE.DTO;

namespace ProfileDAL.Repositories
{
    public interface IProvinceRepository : IRepository<HU_PROVINCE>
    {
        Task<PagedResult<ProvinceDTO>> GetAll(ProvinceDTO param);

		Task<GenericPhaseTwoListResponse<ProvinceDTO>> SinglePhaseQueryList(GenericQueryListDTO<ProvinceDTO> request);
		Task<ResultWithError> GetById(long id);
        Task<ResultWithError> CreateAsync(ProvinceInputDTO param);
        Task<ResultWithError> UpdateAsync(ProvinceInputDTO param);
        Task<ResultWithError> ChangeStatusAsync(List<long> ids);
        Task<ResultWithError> GetListProvince();
        Task<PagedResult<DistrictDTO>> DistrictGetAll(DistrictDTO param);
        Task<ResultWithError> DistrictById(long id);
        Task<ResultWithError> DistrictCreateAsync(DistrictInputDTO param);
        Task<ResultWithError> DistrictUpdateAsync(DistrictInputDTO param);
        Task<ResultWithError> DistrictChangeStatusAsync(List<long> ids);
        Task<ResultWithError> GetListDistrict(int ProvinceId);
        Task<PagedResult<WardDTO>> WardGetAll(WardDTO param);
        Task<ResultWithError> WardCreateAsync(WardInputDTO param);
        Task<ResultWithError> WardUpdateAsync(WardInputDTO param);
        Task<ResultWithError> WardChangeStatusAsync(List<long> ids);
        Task<ResultWithError> GetListWard(int DistrictId);
    }
}
