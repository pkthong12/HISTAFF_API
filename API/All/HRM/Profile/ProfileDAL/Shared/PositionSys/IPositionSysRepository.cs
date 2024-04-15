using ProfileDAL.ViewModels;
using Common.Interfaces;
using Common.Paging;
using Common.Extensions;
using API.Entities;

namespace ProfileDAL.Repositories
{
    public interface IPositionSysRepository : IRepository<HU_POSITION_GROUP>
    {
        Task<PagedResult<PositionSysViewDTO>> GetAll(PositionSysViewDTO param);
        Task<ResultWithError> GetById(long id);
        Task<ResultWithError> CreateAsync(PositionSysInputDTO param);
        Task<ResultWithError> UpdateAsync(PositionSysInputDTO param);
        Task<ResultWithError> ChangeStatusAsync(List<long> ids);
        Task<ResultWithError> GetList(int groupId);
        Task<ResultWithError> Delete(long id);
    }
}
