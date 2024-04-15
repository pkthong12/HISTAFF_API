using ProfileDAL.ViewModels;
using Common.Interfaces;
using Common.Paging;
using Common.Extensions;
using API.Entities;

namespace ProfileDAL.Repositories
{
    public interface IInsuranceTypeRepository : IRepository<INS_TYPE>
    {
        Task<PagedResult<InsuranceTypeDTO>> GetAll(InsuranceTypeDTO param);
        Task<ResultWithError> GetById(long id);
        Task<ResultWithError> CreateAsync(InsuranceTypeInputDTO param);
        Task<ResultWithError> UpdateAsync(InsuranceTypeInputDTO param);
        Task<ResultWithError> ChangeStatusAsync(List<long> ids);
        Task<ResultWithError> GetList();
    }
}
