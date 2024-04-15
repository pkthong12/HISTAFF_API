using ProfileDAL.ViewModels;
using Common.Interfaces;
using Common.Extensions;
using API.Entities;

namespace ProfileDAL.Repositories
{
    public interface IFormListSysRepository : IRepository<SYS_FORM_LIST>
    {
        Task<ResultWithError> GetById(long id);
        Task<ResultWithError> GetList();
        Task<ResultWithError> UpdateAsync(FormListSysDTO param);
    }
}
