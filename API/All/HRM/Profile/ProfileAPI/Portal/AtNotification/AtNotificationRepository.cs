using CORE.GenericUOW;
using CORE.DTO;
using API.DTO;
using CORE.Enum;
using CORE.StaticConstant;
using API.All.DbContexts;
using CORE.AutoMapper;
using System;
using Common.Extensions;

namespace API.Controllers.AtNotification
{
    public class AtNotificationRepository : IAtNotificationRepository
    {
        private readonly GenericUnitOfWork _uow;
        private readonly FullDbContext _dbContext;
        private IGenericRepository<AT_NOTIFICATION, AtNotificationDTO> _genericRepository;
        private readonly GenericReducer<AT_NOTIFICATION, AtNotificationDTO> _genericReducer;

        public AtNotificationRepository(FullDbContext context, GenericUnitOfWork uow)
        {
            _dbContext = context;
            _uow = uow;
            _genericRepository = _uow.GenericRepository<AT_NOTIFICATION, AtNotificationDTO>();
            _genericReducer = new();
        }

        public async Task<GenericPhaseTwoListResponse<AtNotificationDTO>> SinglePhaseQueryList(GenericQueryListDTO<AtNotificationDTO> request)
        {
            var joined = from p in _dbContext.AtNotifications.AsNoTracking()
                         // JOIN OTHER ENTITIES BASED ON THE BUSINESS
                         select new AtNotificationDTO
                         {
                             Id = p.ID
                         };

            var singlePhaseResult = await _genericReducer.SinglePhaseReduce(joined, request);
            return singlePhaseResult;
        }

        public async Task<FormatedResponse> ReadAll()
        {
            await Task.Run(() => null);
            throw new NotImplementedException();
        }
        public async Task<FormatedResponse> GetNotify(long employeeId)
        {
            try
            {
                var list = await (from o in _dbContext.AtNotifications
                                  from c in _dbContext.HuEmployees.Where(x => x.ID == o.EMP_CREATE_ID).DefaultIfEmpty()
                                  from cv in _dbContext.HuEmployeeCvs.Where(x => x.ID == c.PROFILE_ID).DefaultIfEmpty()
                                  where o.EMP_NOTIFY_ID!.Contains(employeeId.ToString()) && ((DateTime.Now.DayOfYear - o.CREATED_DATE!.Value.DayOfYear) <= OtherConfig.NUM_OFF_NOTIFICATION)
                                  orderby o.CREATED_DATE descending
                                  select new
                                  {
                                      Id = o.ID,
                                      CreatedDate = o.CREATED_DATE,
                                      UpdatedDate = o.UPDATED_DATE,
                                      Name = c.CODE + " - " + cv.FULL_NAME,
                                      Action = o.ACTION,
                                      Type = o.TYPE,
                                      RefId = o.REF_ID,
                                      StatusNotify = o.STATUS_NOTIFY,
                                      Title = o.TITLE,
                                      ModelChange = o.MODEL_CHANGE,
                                      IsRead = o.IS_READ
                                  }).ToListAsync();
                return new FormatedResponse() { InnerBody = list, MessageCode = "", StatusCode = EnumStatusCode.StatusCode200 };
            } 
            catch(Exception ex)
            {
                return new FormatedResponse() { InnerBody = null, StatusCode = EnumStatusCode.StatusCode500 };
            }

        }
        public async Task<FormatedResponse> GetCountNotifyUnRead(long? employeeId)
        {
            try
            {
                var list = await _dbContext.AtNotifications.Where(x => x.EMP_NOTIFY_ID!.Contains(employeeId!.Value.ToString()) && (x.IS_READ == false || x.IS_READ == null) && ((DateTime.Now.DayOfYear - x.CREATED_DATE!.Value.DayOfYear) <= OtherConfig.NUM_OFF_NOTIFICATION)).AsNoTracking().ToListAsync();
                if (list != null)
                {
                    return new FormatedResponse() { InnerBody = list.Count, StatusCode = EnumStatusCode.StatusCode200 };
                }

                return new FormatedResponse() { InnerBody = null, StatusCode = EnumStatusCode.StatusCode400 };
            }
            catch(Exception ex)
            {
                return new FormatedResponse() { InnerBody = null, StatusCode = EnumStatusCode.StatusCode500, MessageCode = ex.Message };
            }

        }

        public async Task<FormatedResponse> ReadAllByKey(string key, long value)
        {
            var response = await _genericRepository.ReadAllByKey(key, value);
            return response;
        }

        public async Task<FormatedResponse> ReadAllByKey(string key, string value)
        {
            var response = await _genericRepository.ReadAllByKey(key, value);
            return response;
        }

        public async Task<FormatedResponse> GetById(long id)
        {
            var res = await _genericRepository.GetById(id);
            if (res.InnerBody != null)
            {
                var response = res.InnerBody;
                var list = new List<AT_NOTIFICATION>
                    {
                        (AT_NOTIFICATION)response
                    };
                var joined = (from l in list
                              // JOIN OTHER ENTITIES BASED ON THE BUSINESS
                              select new AtNotificationDTO
                              {
                                  Id = l.ID
                              }).FirstOrDefault();

                return new FormatedResponse() { InnerBody = joined };
            }
            else
            {
                return new FormatedResponse() { MessageCode = CommonMessageCode.ENTITY_NOT_FOUND, ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400 };
            }
        }

        public async Task<FormatedResponse> GetHistoryApprove(long employeeId)
        {
            try
            {

                var typeDictionary = new Dictionary<long, string>()
                { 
                    {1,"Nghỉ" },
                    {2,"Làm thêm" },
                    {3,"Giải trình công" },
                    {4,"Hồ sơ nhân viên" },
                    {5,"Thông tin người thân" },
                    {6,"Bằng cấp chứng chỉ" },
                    {7,"Quá trình công tác" },
                    {8,"Quá trình công tác trước đây" },
                    {9,"Thông tin liên hệ" },
                    {10,"Thông tin phụ" },
                    {11,"Trình độ học vấn" },
                    {12,"Thông tin tài khoản" },
                };
                var response = await (from o in _dbContext.AtNotifications
                                      where o.EMP_NOTIFY_ID!.Contains(employeeId.ToString()) || o.EMP_CREATE_ID == employeeId
                                      orderby o.CREATED_DATE descending
                                      select new
                                      {
                                          Id=o.ID,
                                          TypeName = typeDictionary[o.TYPE!.Value],
                                          //TypeName = o.TYPE == 1 ? "Nghỉ" : (o.TYPE == 2 ? "Làm thêm" : (o.TYPE == 3 ? "Giải trình công" : o.TYPE == 4 ? "Hồ sơ nhân viên"
                                          //: (o.TYPE == 5 ? "Thông tin người thân" : (o.TYPE == 6 ? "Bằng cấp chứng chỉ" : (o.TYPE == 7 ? "Quá trình công tác" : (o.TYPE == 8 ? "Quá trình công tác trước đây"
                                          //: (o.TYPE == 9 ? "Thông tin liên hệ" : (o.TYPE == 10 ? "Thông tin phụ" : (o.TYPE == 11 ? "Trình độ học vấn" : "Thông tin tài khoản"))))))))),
                                          Date = o.CREATED_DATE,
                                          StatusName = o.STATUS_NOTIFY == 0 ? "Chờ phê duyệt" : (o.STATUS_NOTIFY == 1 ? "Được phê duyệt" : "Bị từ chối")
                                      }).Take(10).ToListAsync();
                return new FormatedResponse() { InnerBody = response };
            }
            catch (Exception ex)
            {

                return new FormatedResponse() { ErrorType = EnumErrorType.UNCATCHABLE, StatusCode = EnumStatusCode.StatusCode500, MessageCode = ex.Message };
            }


        }

        public async Task<FormatedResponse> GetById(string id)
        {
            await Task.Run(() => null);
            throw new NotImplementedException();
        }

        public async Task<FormatedResponse> Create(GenericUnitOfWork _uow, AtNotificationDTO dto, string sid)
        {
            var response = await _genericRepository.Create(_uow, dto, sid);
            return response;
        }

        public async Task<FormatedResponse> CreateRange(GenericUnitOfWork _uow, List<AtNotificationDTO> dtos, string sid)
        {
            var add = new List<AtNotificationDTO>();
            add.AddRange(dtos);
            var response = await _genericRepository.CreateRange(_uow, add, sid);
            return response;
        }

        public async Task<FormatedResponse> Update(GenericUnitOfWork _uow, AtNotificationDTO dto, string sid, bool patchMode = true)
        {
            var response = await _genericRepository.Update(_uow, dto, sid, patchMode);
            return response;
        }

        public async Task<FormatedResponse> UpdateRange(GenericUnitOfWork _uow, List<AtNotificationDTO> dtos, string sid, bool patchMode = true)
        {
            var response = await _genericRepository.UpdateRange(_uow, dtos, sid, patchMode);
            return response;
        }

        public async Task<FormatedResponse> Delete(GenericUnitOfWork _uow, long id)
        {
            var response = await _genericRepository.Delete(_uow, id);
            return response;
        }

        public async Task<FormatedResponse> Delete(GenericUnitOfWork _uow, string id)
        {
            await Task.Run(() => null);
            throw new NotImplementedException();
        }

        public async Task<FormatedResponse> DeleteIds(GenericUnitOfWork _uow, List<long> ids)
        {
            var response = await _genericRepository.DeleteIds(_uow, ids);
            return response;
        }

        public async Task<FormatedResponse> DeleteIds(GenericUnitOfWork _uow, List<string> ids)
        {
            await Task.Run(() => null);
            throw new NotImplementedException();
        }

        public virtual async Task<FormatedResponse> ToggleActiveIds(GenericUnitOfWork _uow, List<long> ids, bool valueToBind, string sid) 
        {
            var response = await _genericRepository.ToggleActiveIds(_uow, ids, valueToBind, sid);
            return response;
        }
    }
}

