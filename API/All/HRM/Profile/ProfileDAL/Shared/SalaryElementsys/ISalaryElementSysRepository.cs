using ProfileDAL.ViewModels;
using Common.Interfaces;
using Common.Paging;
using Common.Extensions;
using API.Entities;

namespace ProfileDAL.Repositories
{
    public interface ISalaryElementSysRepository : IRepository<SYS_PA_ELEMENT>
    {
        Task<PagedResult<SalaryElementSysDTO>> GetAll(SalaryElementSysDTO param);
        Task<ResultWithError> GetById(long id);
        Task<ResultWithError> CreateAsync(SalaryElementSysInputDTO param);
        Task<ResultWithError> UpdateAsync(SalaryElementSysInputDTO param);
        Task<ResultWithError> GetSalaryElement(GroupElementSysDTO param);
        Task<ResultWithError> GetListGroup();
        Task<ResultWithError> GetList(int groupid);
        Task<ResultWithError> GetListCal(int SalaryTypeId);
        Task<ResultWithError> ChangeStatusAsync(List<long> ids);
    }
}
