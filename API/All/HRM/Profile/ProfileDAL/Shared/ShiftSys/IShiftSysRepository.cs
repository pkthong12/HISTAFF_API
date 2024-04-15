using Common.Interfaces;
using Common.Paging;
using Common.Extensions;
using ProfileDAL.ViewModels;
using API.Entities;

namespace ProfileDAL.Repositories
{
    public interface IShiftSysRepository : IRepository<SYS_SHIFT>
    {
        Task<PagedResult<ShiftSysDTO>> GetAll(ShiftSysDTO param);
        Task<ResultWithError> GetById(long id);
        Task<ResultWithError> CreateAsync(ShiftSysInputDTO param);
        Task<ResultWithError> UpdateAsync(ShiftSysInputDTO param);
        Task<ResultWithError> ChangeStatusAsync(List<long> ids);
        Task<ResultWithError> GetList();
        Task<ResultWithError> GetShiftCycle(int id);
        Task<ResultWithError> UpdateShiftCycle(ShiftCycleSysInput param);
    }
}
