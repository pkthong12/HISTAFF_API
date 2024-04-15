using Common.Paging;
using ProfileDAL.ViewModels;
using Common.Repositories;
using Common.Extensions;
using API.All.DbContexts;
using API.Entities;

namespace ProfileDAL.Repositories
{
    public class BookingRepository : RepositoryBase<AD_BOOKING>, IBookingRepository
    {
        private ProfileDbContext _appContext => (ProfileDbContext)_context;
        public BookingRepository(ProfileDbContext context) : base(context)
        {

        }
        /// <summary>
        /// Get list Approved and Waiting
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<PagedResult<BookingDTO>> GetAll(BookingDTO param)
        {
            var queryable = from p in _appContext.Bookings
                            join r in _appContext.Rooms on p.ROOM_ID equals r.ID
                            join e in _appContext.Employees on p.EMP_ID equals e.ID
                            join o in _appContext.Organizations on e.ORG_ID equals o.ID
                            from a in _appContext.Employees.Where(x => x.ID == p.APPROVED_ID).DefaultIfEmpty()
                            join f in _appContext.OtherListFixs.Where(c => c.TYPE == SystemConfig.STATUS_APPROVE) on p.STATUS_ID equals f.ID
                            
                            orderby p.STATUS_ID, p.HOUR_FROM
                            select new BookingDTO
                            {
                                Id = p.ID,
                                RoomName = r.NAME,
                                EmpCode = e.CODE,
                                EmpName = e.Profile.FULL_NAME,
                                OrgName = o.NAME,
                                BookingDay = p.BOOKING_DAY,
                                HourFrom = p.HOUR_FROM,
                                HourTo = p.HOUR_TO,
                                Note = p.NOTE,
                                StatusName = f.NAME,
                                StatusId = p.STATUS_ID,
                                CreateDate = p.CREATED_DATE,
                                ApproveDate = p.APPROVED_DATE,
                                ApproveName = a.Profile.FULL_NAME,
                                ApproveNote = p.APPROVED_NOTE
                            };

            if (param.EmpCode != null)
            {
                queryable = queryable.Where(p => p.EmpCode.ToUpper().Contains(param.EmpCode.ToUpper()));
            }

            if (param.EmpName != null)
            {
                queryable = queryable.Where(p => p.EmpName.ToUpper().Contains(param.EmpName.ToUpper()));
            }

            if (param.StatusId != null)
            {
                queryable = queryable.Where(p => p.StatusId == param.StatusId);
            }
            return await PagingList(queryable, param);
        }

        /// <summary>
        /// CMS Get Detail by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>        
        public async Task<ResultWithError> GetById(long id)
        {
            var queryable = await (from p in _appContext.Bookings
                                   join r in _appContext.Rooms on p.ROOM_ID equals r.ID
                                   join e in _appContext.Employees on p.EMP_ID equals e.ID
                                   
                                   from a in _appContext.Employees.Where(x => x.ID == p.APPROVED_ID).DefaultIfEmpty()
                                   join f in _appContext.OtherListFixs.Where(c => c.TYPE == SystemConfig.STATUS_APPROVE) on p.STATUS_ID equals f.ID
                                   
                                   select new BookingDTO
                                   {
                                       Id = p.ID,
                                       RoomId = p.ROOM_ID,
                                       RoomName = r.NAME,
                                       EmpId = p.EMP_ID,
                                       EmpCode = e.Profile.FULL_NAME,
                                       EmpName = e.CODE,
                                       BookingDay = p.BOOKING_DAY,
                                       HourFrom = p.HOUR_FROM,
                                       HourTo = p.HOUR_TO,
                                       Note = p.NOTE,
                                       StatusName = f.NAME,
                                       StatusId = p.STATUS_ID,
                                       CreateDate = p.CREATED_DATE,
                                       ApproveDate = p.APPROVED_DATE,
                                       ApproveName = a.Profile.FULL_NAME,
                                       ApproveNote = p.APPROVED_NOTE
                                   }).FirstOrDefaultAsync();
            return new ResultWithError(queryable);
        }
        /// <summary>
        /// Register
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<ResultWithError> PortalReg(BookingInputDTO param)
        {
            var r = _appContext.Bookings.Where(x=>x.STATUS_ID == OtherConfig.STATUS_APPROVE && ((param.HourFrom >= x.HOUR_FROM && param.HourFrom <= x.HOUR_TO) || (param.HourTo >= x.HOUR_FROM && param.HourTo <= x.HOUR_TO))).Count();
            if (r > 0)
            {
                return new ResultWithError("TIME_EXISTS");
            }

            var a = _appContext.Bookings.Where(x => x.EMP_ID == _appContext.EmpId && x.ROOM_ID == param.RoomId && ((param.HourFrom >= x.HOUR_FROM && param.HourFrom <= x.HOUR_TO) || (param.HourTo >= x.HOUR_FROM && param.HourTo <= x.HOUR_TO))).Count();
            if (a > 0)
            {
                return new ResultWithError("EMP_TIME_IS_REGISTED");
            }

            var data = Map(param, new AD_BOOKING());
            data.EMP_ID = _appContext.EmpId;
            data.STATUS_ID = OtherConfig.STATUS_WAITING;
            var result = await _appContext.Bookings.AddAsync(data);
            try
            {
                await _appContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return new ResultWithError(200);
        }
        /// <summary>
        /// Portal Edit Data
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<ResultWithError> PortalEditReg(BookingInputDTO param)
        {
            var r = _appContext.Bookings.Where(x => x.ID == param.Id ).FirstOrDefault();
            if (r == null)
            {
                return new ResultWithError(404);
            }

            var c = _appContext.Bookings.Where(x=>x.STATUS_ID == OtherConfig.STATUS_APPROVE && ((param.HourFrom >= x.HOUR_FROM && param.HourFrom <= x.HOUR_TO) || (param.HourTo >= x.HOUR_FROM && param.HourTo <= x.HOUR_TO))).Count();
            if (c > 0)
            {
                return new ResultWithError("TIME_EXISTS");
            }

            var a = _appContext.Bookings.Where(x => x.EMP_ID == _appContext.EmpId && x.ID != param.Id && ((param.HourFrom >= x.HOUR_FROM && param.HourFrom <= x.HOUR_TO) || (param.HourTo >= x.HOUR_FROM && param.HourTo <= x.HOUR_TO))).Count();
            if (a > 0)
            {
                return new ResultWithError("EMP_TIME_IS_REGISTED");
            }

            var data = Map(param, r);
            var result = _appContext.Bookings.Update(data);
            await _appContext.SaveChangesAsync();
            return new ResultWithError(200);
        }
        /// <summary>
        /// Portal Delete
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ResultWithError> PortalDelete(long id)
        {
            try
            {
                var item = await _appContext.Bookings.Where(x => x.ID == id ).FirstOrDefaultAsync();
                if (item.STATUS_ID == OtherConfig.STATUS_APPROVE)
                {
                    return new ResultWithError(Message.RECORD_IS_APPROVED);
                }
                _appContext.Bookings.Remove(item);
                await _appContext.SaveChangesAsync();
                return new ResultWithError(item);
            }
            catch (Exception ex)
            {
                return new ResultWithError(ex.Message);
            }
        }
        /// <summary>
        /// Portal Change Status Data
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<ResultWithError> ChangeStatusAsync(long id, long statusId, string note)
        {
            var r = _appContext.Bookings.Where(x => x.ID == id ).FirstOrDefault();
            if (r == null)
            {
                return new ResultWithError(404);
            }
            if (r.STATUS_ID == OtherConfig.STATUS_APPROVE || r.STATUS_ID == OtherConfig.STATUS_DECLINE)
            {
                return new ResultWithError(Message.RECORD_IS_APPROVED);
            }

            var c = _appContext.Bookings.Where(x=>x.STATUS_ID == OtherConfig.STATUS_APPROVE && ((r.HOUR_FROM >= x.HOUR_FROM && r.HOUR_FROM <= x.HOUR_TO) || (r.HOUR_TO >= x.HOUR_FROM && r.HOUR_TO <= x.HOUR_TO))).Count();
            if (c > 0)
            {
                return new ResultWithError("TIME_EXISTS");
            }
            var room = _appContext.Rooms.Where(x => x.ID == r.ROOM_ID).FirstOrDefault();
            r.STATUS_ID = statusId;
            r.APPROVED_ID = _appContext.EmpId; // xem lại vì webapp chưa đẩy emp
            r.APPROVED_DATE = DateTime.Now;
            r.APPROVED_NOTE = note;
            var result = _appContext.Bookings.Update(r);
            await _appContext.SaveChangesAsync();

            // tạo notify
            // lấy fmc token
            var lstEmpId = new List<long>() { r.EMP_ID };
            var lstToken = await QueryData.ExecuteList("connectionString",
                "PKG_NOTIFI.GET_FMC_TOKEN", new
                {
                    P_EMP_ID = string.Join(",", lstEmpId),
                    P_CUR = QueryData.OUT_CURSOR
                });
            var IsRead = new List<bool>(lstToken.Count);
            List<string> lstDevice = lstToken.Select(f => (string)((dynamic)f).FCM_TOKEN).ToList();
            if (lstDevice.Count > 0)
            {
                for (int i = 0; i < lstToken.Count; i++)
                {
                    IsRead.Add(false);
                }
                // 
                var no = new AT_NOTIFICATION();
                no.TYPE = Consts.REGISTER_ROOM;

                no.NOTIFI_ID = r.ID;
                no.EMP_CREATE_ID = _appContext.EmpId;
                no.FMC_TOKEN = string.Join(";", lstDevice);
                //no.IS_READ = string.Join(";", IsRead);
                //no.EMP_NOTIFY_ID = string.Join(";", lstEmpId);
                no.CREATED_DATE = DateTime.Now;

                // sau khi lưu gửi thông báo
                var Title = "";
                var Body = "";
                if (statusId == OtherConfig.STATUS_APPROVE)
                {
                    no.ACTION = Consts.ACTION_APPROVE;
                    Title = "Đăng ký " + room.NAME + " đã được duyệt";
                    Body = "Đăng ký " + room.NAME + " đã được duyệt " + DateTime.Now.ToString("MM/dd/yyyy");
                }
                else
                {
                    no.ACTION = Consts.ACTION_DENIED;
                    Title = "Đăng ký " + room.NAME + " bị từ chối";
                    Body = "Đăng ký " + room.NAME + " bị từ chối " + DateTime.Now.ToString("MM/dd/yyyy");
                }

                await _appContext.Notifications.AddAsync(no);
                await SendNotification(new NotificationModel
                {
                    Devices = lstDevice,
                    Title = Title,
                    Body = Body

                });
             }

                return new ResultWithError(200);
        }
        /// <summary>
        /// Portal Get My List
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<ResultWithError> PortalMyList()
        {
            var queryable = await (from p in _appContext.Bookings
                            join r in _appContext.Rooms on p.ROOM_ID equals r.ID
                            from e in _appContext.Employees.Where(x=>x.ID == p.APPROVED_ID).DefaultIfEmpty()
                            
                            orderby p.STATUS_ID, p.BOOKING_DAY descending, p.HOUR_FROM descending
                            select new 
                            {
                                Id = p.ID,
                                RoomName = r.NAME,
                                BookingDay = p.BOOKING_DAY,
                                HourFrom = p.HOUR_FROM,
                                HourTo = p.HOUR_TO,
                                Note = p.NOTE,
                                StatusId = p.STATUS_ID,
                                ApproveName = e.Profile.FULL_NAME,
                                ApproveDate = p.APPROVED_DATE,
                                ApproveNote = p.APPROVED_NOTE
                            }).ToListAsync();
            return new ResultWithError(queryable);
        }
        /// <summary>
        /// PortalListByRoom
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<ResultWithError> PortalListByRoom(BookingDTO param )
        {
            var queryable = await (from p in _appContext.Bookings
                                   join e in _appContext.Employees on p.EMP_ID equals e.ID
                                   where p.ROOM_ID == param.RoomId && p.BOOKING_DAY == param.BookingDay
                                   orderby p.HOUR_FROM descending
                                   select new
                                   {
                                       Id = p.ID,
                                       BookingDay = p.BOOKING_DAY,
                                       Avatar = e.Profile.AVATAR,
                                       FullName = e.Profile.FULL_NAME,
                                       HourFrom = p.HOUR_FROM,
                                       HourTo = p.HOUR_TO,
                                       Note = p.NOTE
                                   }).ToListAsync();
            return new ResultWithError(queryable);
        }

        public async Task<ResultWithError> PortalGetBy(long id)
        {
            var queryable = await (from p in _appContext.Bookings
                                   join r in _appContext.Rooms on p.ROOM_ID equals r.ID
                                   from a in _appContext.Employees.Where(x=>x.ID == p.APPROVED_ID).DefaultIfEmpty()
                                   where p.ID == id
                                   select new
                                   {
                                       Id = p.ID,
                                       RoomId = p.ROOM_ID,
                                       RoomName = r.NAME,
                                       BookingDay = p.BOOKING_DAY,
                                       HourFrom = p.HOUR_FROM,
                                       HourTo = p.HOUR_TO,
                                       Note = p.NOTE,
                                       StatusId = p.STATUS_ID,
                                       CreateDate = p.CREATED_DATE,
                                       ApproveDate = p.APPROVED_DATE,
                                       ApproveName = a.Profile.FULL_NAME,
                                       ApproveNote = p.APPROVED_NOTE
                                   }).ToListAsync();
            return new ResultWithError(queryable);
        }

    }
}
