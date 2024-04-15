using ProfileDAL.ViewModels;
using Common.Interfaces;
using Common.Paging;
using Common.Extensions;
using API.Entities;

namespace ProfileDAL.Repositories
{
    public interface IGroupPositionSysRepository : IRepository<SYS_POSITION_GROUP>
    {
        Task<PagedResult<GroupPositionSysDTO>> GetAll(GroupPositionSysDTO param);
        Task<ResultWithError> GetById(long id);
        Task<ResultWithError> CreateAsync(GroupPositionSysInputDTO param);
        Task<ResultWithError> UpdateAsync(GroupPositionSysInputDTO param);
        Task<ResultWithError> ChangeStatusAsync(List<long> ids);
        Task<ResultWithError> GetList();
        Task<ResultWithError> GetListByArea(int id);
        Task<ResultWithError> Delete(long id);
    }
}
