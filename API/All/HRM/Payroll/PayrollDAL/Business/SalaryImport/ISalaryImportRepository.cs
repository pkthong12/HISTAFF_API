using PayrollDAL.ViewModels;
using Common.Interfaces;
using Common.Paging;
using Common.Extensions;
using System.Dynamic;
using API.Entities;

namespace PayrollDAL.Repositories
{
    public interface ISalaryImportRepository : IRepository<PA_SAL_IMPORT>
    {
       
        Task<ResultWithError> ExportTemplate(SalImpSearchParam param);
        Task<ResultWithError> ImportTemplate(SalImpImportParam param);
        Task<PagedResult<ExpandoObject>> GetAll(SalImportDTO param);
        Task<ResultWithError> Delete(SalImportDelParam param);
        
    }
}
