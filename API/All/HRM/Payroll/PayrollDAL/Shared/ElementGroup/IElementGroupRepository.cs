using PayrollDAL.ViewModels;
using Common.Interfaces;
using Common.Paging;
using Common.Extensions;
using API.Entities;

namespace PayrollDAL.Repositories
{
    public interface IElementGroupRepository : IRepository<PA_ELEMENT_GROUP>
    {
        Task<PagedResult<ElementGroupDTO>> GetAll(ElementGroupDTO param);
        Task<ResultWithError> GetById(long id);
        Task<ResultWithError> CreateAsync(ElementGroupInputDTO param);
        Task<ResultWithError> UpdateAsync(ElementGroupInputDTO param);
        Task<ResultWithError> ChangeStatusAsync(List<long> ids);
        Task<ResultWithError> GetList();
    }
}
