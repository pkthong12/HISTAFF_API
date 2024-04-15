using ProfileDAL.ViewModels;
using Common.Interfaces;
using Common.Paging;
using Common.Extensions;
using API.Entities;
using CORE.DTO;

namespace ProfileDAL.Repositories
{
    public interface ISalaryScaleRepository : IRepository<HU_SALARY_SCALE>
    {
        Task<GenericPhaseTwoListResponse<SalaryScaleViewDTO>> TwoPhaseQueryList(GenericQueryListDTO<SalaryScaleViewDTO> request);
        Task<GenericPhaseTwoListResponse<SalaryScaleViewDTO>> SinglePhaseQueryList(GenericQueryListDTO<SalaryScaleViewDTO> request);
        Task<PagedResult<SalaryScaleDTO>> GetAll(SalaryScaleDTO param);
        Task<ResultWithError> GetById(long id);
        Task<ResultWithError> CreateAsync(SalaryScaleInputDTO param);
        Task<ResultWithError> UpdateAsync(SalaryScaleInputDTO param);
        Task<ResultWithError> ChangeStatusAsync(List<long> ids);
        Task<ResultWithError> GetList();
    }
}
