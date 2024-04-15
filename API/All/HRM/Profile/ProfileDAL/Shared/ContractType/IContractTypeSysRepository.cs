using ProfileDAL.ViewModels;
using Common.Interfaces;
using Common.Paging;
using Common.Extensions;
using API.Entities;

namespace ProfileDAL.Repositories
{
    public interface IContractTypeSysRepository : IRepository<SYS_CONTRACT_TYPE>
    {
        Task<PagedResult<ContractTypeSysViewDTO>> GetAll(ContractTypeSysViewDTO param);
        Task<ResultWithError> GetById(long id);
        Task<ResultWithError> CreateAsync(ContractTypeSysInputDTO param);
        Task<ResultWithError> UpdateAsync(ContractTypeSysInputDTO param);
        Task<ResultWithError> ChangeStatusAsync(List<long> ids);
        Task<ResultWithError> GetList();
    }
}
