using ProfileDAL.ViewModels;
using Common.Interfaces;
using Common.Paging;
using Common.Extensions;
using API.Entities;

namespace ProfileDAL.Repositories
{
    public interface IBookingRepository : IRepository<AD_BOOKING>
    {
        Task<PagedResult<BookingDTO>> GetAll(BookingDTO param);
        Task<ResultWithError> GetById(long id);
        Task<ResultWithError> PortalReg(BookingInputDTO param);
        Task<ResultWithError> PortalEditReg(BookingInputDTO param);
        Task<ResultWithError> PortalDelete(long id);
        Task<ResultWithError> ChangeStatusAsync(long id, long statusId, string note);
        Task<ResultWithError> PortalMyList();
        Task<ResultWithError> PortalListByRoom(BookingDTO param);
        Task<ResultWithError> PortalGetBy(long id);
    }
}
