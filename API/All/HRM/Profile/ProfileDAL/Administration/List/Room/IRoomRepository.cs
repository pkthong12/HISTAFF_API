using ProfileDAL.ViewModels;
using Common.Interfaces;
using Common.Paging;
using Common.Extensions;
using API.Entities;

namespace ProfileDAL.Repositories
{
    public interface IRoomRepository : IRepository<AD_ROOM>
    {
        Task<PagedResult<RoomDTO>> GetAll(RoomDTO param);
        Task<ResultWithError> GetById(long id);
        Task<ResultWithError> CreateAsync(RoomInputDTO param);
        Task<ResultWithError> UpdateAsync(RoomInputDTO param);
        Task<ResultWithError> ChangeStatusAsync(List<long> ids);
        Task<ResultWithError> GetList();
        Task<ResultWithError> Remove(int id);
    }
}
