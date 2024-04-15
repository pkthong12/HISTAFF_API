using PayrollDAL.ViewModels;
using Common.Interfaces;
using Common.Paging;
using Common.Extensions;
using API.Entities;

namespace PayrollDAL.Repositories
{
    public interface ISalaryElementRepository : IRepository<PA_ELEMENT>
    {
        Task<PagedResult<SalaryElementDTO>> GetAll(SalaryElementDTO param);
        Task<ResultWithError> GetById(long id);
        Task<ResultWithError> AllowanceToElement(ReferParam param, int type);
        Task<ResultWithError> CreateAsync(SalaryElementInputDTO param);
        Task<ResultWithError> UpdateAsync(SalaryElementInputDTO param);
        Task<ResultWithError> GetSalaryElement(long groupId);
        Task<ResultWithError> GetListGroup();
        Task<ResultWithError> GetSalaryElementSys();
        Task<ResultWithError> GetList(int groupid);
        Task<ResultWithError> GetListCal(int SalaryTypeId);
        Task<ResultWithError> ChangeStatusAsync(List<long> ids);
        Task<ResultWithError> GetListAll();
    }
}
