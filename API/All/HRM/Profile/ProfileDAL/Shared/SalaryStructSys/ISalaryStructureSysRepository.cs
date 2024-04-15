using ProfileDAL.ViewModels;
using Common.Interfaces;
using Common.Paging;
using Common.Extensions;
using API.Entities;

namespace ProfileDAL.Repositories
{
    public interface ISalaryStructureSysRepository : IRepository<SYS_SALARY_STRUCTURE>
    {
        Task<PagedResult<SalaryStructureSysDTO>> GetAll(SalaryStructureSysDTO param);
        Task<ResultWithError> GetById(long id);
        Task<ResultWithError> CreateAsync(SalaryStructureSysInputDTO param);
        Task<ResultWithError> UpdateAsync(SalaryStructureSysInputDTO param);
        Task<ResultWithError> GetElement(long salaryTypeId);
        /// <summary>
        /// Get list element salary visible 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ResultWithError> GetList(int id);
    }
}
