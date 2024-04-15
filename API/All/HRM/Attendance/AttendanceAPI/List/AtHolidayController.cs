using Microsoft.AspNetCore.Mvc;
using AttendanceDAL.ViewModels;
using API.All.DbContexts;
using API.Entities;
using CORE.DTO;
using CORE.Enum;
using CORE.GenericUOW;
using CORE.StaticConstant;
using ProfileDAL.ViewModels;
using System.Text.RegularExpressions;
using API.Main;
using API;
using Microsoft.Extensions.Options;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Logical;
using Hangfire.Storage;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Ocsp;

namespace AttendanceAPI.List
{
    [HiStaffAuthorize]
    [ApiExplorerSettings(GroupName = "002-SYSTEM-LIST-HOLIDAY")]
    [ApiController]

    [Route("api/[controller]/[action]")]
    public class AtHolidayController : ControllerBase
    {
        private readonly GenericUnitOfWork _uow;
        private readonly AttendanceDbContext _attendanceDbContext;
        private IGenericRepository<AT_HOLIDAY, HolidayDTO> _genericRepository;
        private readonly GenericReducer<AT_HOLIDAY, HolidayDTO> genericReducer;
        private AppSettings _appSettings;
        public AtHolidayController(AttendanceDbContext attendanceDbContext, IOptions<AppSettings> options)
        {
            _attendanceDbContext = attendanceDbContext;
            _uow = new GenericUnitOfWork(_attendanceDbContext);
            _genericRepository = _uow.GenericRepository<AT_HOLIDAY, HolidayDTO>();
            _appSettings = options.Value;
            genericReducer = new();
        }

        [HttpPost]
        public async Task<IActionResult> QueryList(GenericQueryListStringIdDTO<HolidayDTO> request)
        {
            try
            {
                var entity = _attendanceDbContext.Set<AT_HOLIDAY>().AsNoTracking().AsQueryable();
                var joined = from p in entity
                             group p by p.CODE into g
                             select new HolidayDTO
                             {
                                 Id = g.First().ID,
                                 Code = g.First().CODE,
                                 Name = g.First().NAME,
                                 Note = g.First().NOTE,
                                 IsActive = g.First().IS_ACTIVE,
                                 StartDayoff = g.First().START_DAYOFF,
                                 EndDayoff = g.First().END_DAYOFF,
                                 Status = g.First().IS_ACTIVE ? "Áp dụng" : "Ngừng áp dụng",
                                 CreateDate = g.First().CREATED_DATE
                             };
                request.Sort = new List<SortItem>();
                request.Sort.Add(new SortItem() { Field = "CreateDate", SortDirection = EnumSortDirection.DESC });
                var response = await genericReducer.SinglePhaseReduce(joined, request);
                if (response.ErrorType != EnumErrorType.NONE)
                {
                    return Ok(new FormatedResponse()
                    {
                        ErrorType = response.ErrorType,
                        MessageCode = response.MessageCode ?? CommonMessageCode.NOT_ALL_CASES_CATCHED,
                        StatusCode = EnumStatusCode.StatusCode400,
                    });
                }
                else
                {
                    return Ok(new FormatedResponse()
                    {
                        InnerBody = response
                    });
                }
            }
            catch (Exception ex)
            {
                return Ok(new FormatedResponse() { ErrorType = EnumErrorType.UNCATCHABLE, StatusCode = EnumStatusCode.StatusCode500, MessageCode = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetById(long id)
        {
            try
            {
                var objRes = await (from p in _attendanceDbContext.Holidays
                                    where p.ID == id
                                    select new
                                    {
                                        Id = p.ID,
                                        Code = p.CODE,
                                        Name = p.NAME,
                                        Note = p.NOTE,
                                        IsActive = p.IS_ACTIVE,
                                        StartDayoff = p.START_DAYOFF,
                                        EndDayoff = p.END_DAYOFF
                                    }).FirstOrDefaultAsync();
                return Ok(new FormatedResponse() { InnerBody = objRes });
            }
            catch (Exception ex)
            {
                return Ok(new FormatedResponse() { MessageCode = ex.Message, ErrorType = EnumErrorType.UNCATCHABLE, StatusCode = EnumStatusCode.StatusCode500 });
            }
        }

        [HttpGet]
        public async Task<IActionResult> CreateCodeAuto()
        {
            try
            {
                decimal num;
                string str;
                var rs = "NNL001";

                var entity = _uow.Context.Set<AT_HOLIDAY>().AsNoTracking().AsQueryable();
                var joined = (from p in entity
                              where (p.CODE != null || p.CODE != "") && p.CODE.Length == 6
                              select new HolidayDTO
                              {
                                  Id = p.ID,
                                  Name = p.NAME,
                                  Code = p.CODE
                              }).ToList();
                var maxCode = (from p in joined where Regex.IsMatch(p.Code, @"(NNL)(\d{3}$)") orderby p.Code.Substring(3, 3) descending select p.Code.Substring(3, 3)).FirstOrDefault();
                if (maxCode != null)
                {
                    rs = "NNL" + (int.Parse(maxCode) + 1).ToString("000");
                }
                return Ok(new FormatedResponse() { InnerBody = new { Code = rs } });
            }
            catch (Exception ex)
            {
                return Ok(new FormatedResponse() { MessageCode = ex.Message, ErrorType = EnumErrorType.UNCATCHABLE, StatusCode = EnumStatusCode.StatusCode500 });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create(HolidayDTO model)
        {
            try
            {
                var entity = _attendanceDbContext.Set<AT_HOLIDAY>().AsNoTracking().AsQueryable();
                var validate = (from p in entity where p.NAME == model.Name && p.ID != model.Id select p).Any();
                var objData = new AT_HOLIDAY();


                // cmt dòng code kiểm tra trùng tên lại
                // BA yêu cầu

                //if (validate)
                //{
                //    return Ok(new FormatedResponse() { ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400, MessageCode = "UI_FORM_CONTROL_ERROR_AT_HOLIDAY_DATA_EXIST" });
                //}


                var sid = Request.Sid(_appSettings);
                for (DateTime counter = (DateTime)model.StartDayoff; counter <= (DateTime)model.EndDayoff; counter = counter.AddDays(1))
                {
                    objData = new AT_HOLIDAY()
                    {
                        START_DAYOFF = (DateTime)model.StartDayoff,
                        END_DAYOFF = (DateTime)model.EndDayoff,
                        CODE = model.Code,
                        NAME = model.Name,
                        IS_ACTIVE = true,
                        NOTE = model.Note,
                        WORKINGDAY = counter,
                        CREATED_BY = sid,
                        CREATED_DATE = DateTime.Now,
                        UPDATED_BY = sid,
                        UPDATED_DATE = DateTime.Now
                    };
                    _attendanceDbContext.Holidays.AddRange(objData);
                }
                await _attendanceDbContext.SaveChangesAsync();
                return Ok(new FormatedResponse()
                {
                    MessageCode = CommonMessageCode.CREATE_SUCCESS,
                    InnerBody = objData,
                    StatusCode = EnumStatusCode.StatusCode200
                });
            }
            catch (Exception ex)
            {
                return Ok(new FormatedResponse() { MessageCode = ex.Message, ErrorType = EnumErrorType.UNCATCHABLE, StatusCode = EnumStatusCode.StatusCode500 });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Update(HolidayDTO model)
        {
            try
            {
                // bỏ cái này đi
                // trạng thái = "ngừng áp dụng"
                // vẫn cho sửa bản ghi bình thường

                // kiểm tra trạng thái
                // của bản ghi
                //if (model.IsActive == false || model.IsActive == null)
                //{
                //    return Ok(new FormatedResponse()
                //    {
                //        StatusCode = EnumStatusCode.StatusCode400,
                //        MessageCode = "Bản ghi đã ngừng áp dụng, không sửa được"
                //    });
                //}


                // tạo đối tượng rỗng
                // nó chỉ có cái khung là AT_HOLIDAY thôi
                var objData = new AT_HOLIDAY();
                

                var entity = _attendanceDbContext.Set<AT_HOLIDAY>().AsNoTracking().AsQueryable();
                
                var validate = (from p in entity
                                where p.NAME == model.Name && p.ID != model.Id &&
                                    model.Code != p.CODE
                                select p).Any();
                
                
                // cmt dòng code kiểm tra trùng tên lại
                // BA yêu cầu

                //if (validate)
                //{
                //    return Ok(new FormatedResponse() {
                //        ErrorType = EnumErrorType.CATCHABLE,
                //        StatusCode = EnumStatusCode.StatusCode400,
                //        MessageCode = "UI_FORM_CONTROL_ERROR_AT_HOLIDAY_DATA_EXIST"
                //    });
                //}
                
                var sid = Request.Sid(_appSettings);

                var deleteEntity = (from p in entity
                                    where model.Code == p.CODE
                                    select p).ToList();
                
                _attendanceDbContext.Holidays.RemoveRange(deleteEntity);
                
                for (DateTime counter = (DateTime)model.StartDayoff;
                    counter <= (DateTime)model.EndDayoff;
                    counter = counter.AddDays(1)
                )
                {
                    objData = new AT_HOLIDAY()
                    {
                        START_DAYOFF = (DateTime)model.StartDayoff,
                        END_DAYOFF = (DateTime)model.EndDayoff,
                        CODE = model.Code,
                        NAME = model.Name,
                        IS_ACTIVE = (bool)model.IsActive,
                        NOTE = model.Note,
                        WORKINGDAY = counter,
                        CREATED_BY = sid,
                        CREATED_DATE = DateTime.Now,
                        UPDATED_BY = sid,
                        UPDATED_DATE = DateTime.Now
                    };

                    _attendanceDbContext.Holidays.AddRange(objData);
                }

                await _attendanceDbContext.SaveChangesAsync();
                
                return Ok(new FormatedResponse()
                {
                    MessageCode = CommonMessageCode.UPDATE_SUCCESS,
                    InnerBody = objData,
                    StatusCode = EnumStatusCode.StatusCode200
                });
            }
            catch (Exception ex)
            {
                return Ok(new FormatedResponse() { MessageCode = ex.Message, ErrorType = EnumErrorType.UNCATCHABLE, StatusCode = EnumStatusCode.StatusCode500 });
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteIds(IdsRequest req)
        {
            try
            {
                if (req.Ids != null)
                {
                    var entity = _attendanceDbContext.Set<AT_HOLIDAY>().AsNoTracking().AsQueryable();


                    // lấy ra list_code
                    // từ list các id được truyền vào
                    var list_code = (from p in entity
                                     where req.Ids.Contains(p.ID)
                                     select p.CODE).ToList();


                    // bây giờ phải gán lại cho Ids
                    req.Ids = (from p in entity
                               where list_code.Contains(p.CODE)
                               select p.ID).ToList();


                    var lstCode = (from p in entity where req.Ids.Contains(p.ID) select p.CODE).ToList();
                    var deleteEntity = (from p in entity where lstCode.Contains(p.CODE) select p).ToList();
                    var workingDay = (from p in _attendanceDbContext.TimeTimeSheetDailys.AsNoTracking().DefaultIfEmpty()
                                      from t in _attendanceDbContext.TimeTypes.AsNoTracking().Where(t => t.ID == p.MANUAL_ID).DefaultIfEmpty()
                                      where t.CODE == "L"
                                      select p.WORKINGDAY).ToList();
                    #region 
                    //foreach (var dlt in deleteEntity)
                    //{
                    //    bool check = true;
                    //    foreach (var wd in workingDay)
                    //    {
                    //        if (dlt.START_DAYOFF <= wd && wd <= dlt.END_DAYOFF)
                    //        {
                    //            check = false;
                    //        }
                    //    }
                    //    if (check && dlt.IS_ACTIVE)
                    //    {
                    //        _attendanceDbContext.Holidays.Remove(dlt);
                    //    }
                    //}
                    #endregion

                    bool check = true;
                    string record ="";
                    foreach (var dlt in deleteEntity)
                    {
                        if (dlt.IS_ACTIVE)
                        {
                            check = false;
                            record = dlt.CODE;
                            break;
                        }
                    }

                    if (check)
                    {
                        _attendanceDbContext.Holidays.RemoveRange(deleteEntity);
                        await _attendanceDbContext.SaveChangesAsync();
                        return Ok(new FormatedResponse()
                        {
                            MessageCode = "Xóa bản ghi thành công",
                            StatusCode = EnumStatusCode.StatusCode200
                        });
                    }
                    return Ok(new FormatedResponse() {
                        ErrorType = EnumErrorType.CATCHABLE,
                        MessageCode = CommonMessageCode.DO_NOT_DELETE_DATA_IS_ACTIVE,
                        StatusCode = EnumStatusCode.StatusCode400
                    });
                }
                else
                {
                    return Ok(new FormatedResponse() { ErrorType = EnumErrorType.CATCHABLE, MessageCode = CommonMessageCode.DELETE_REQUEST_NULL_ID });
                }
            }
            catch (Exception ex)
            {
                return Ok(new FormatedResponse() { ErrorType = EnumErrorType.UNCATCHABLE, StatusCode = EnumStatusCode.StatusCode500, MessageCode = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> ToggleActiveIds(GenericToggleIsActiveDTO model)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();


            // lấy tất cả bản ghi
            // trong bảng AT_HOLIDAY
            var entity = _attendanceDbContext.Set<AT_HOLIDAY>().AsNoTracking().AsQueryable();


            // lấy ra list_code
            // từ list các id được truyền vào
            var list_code = (from p in entity
                             where model.Ids.Contains(p.ID)
                             select p.CODE).ToList();


            // bây giờ phải gán lại cho Ids
            model.Ids = (from p in entity
                       where list_code.Contains(p.CODE)
                       select p.ID).ToList();


            var response = await _genericRepository.ToggleActiveIds(_uow, model.Ids, model.ValueToBind, sid);
            return Ok(new FormatedResponse()
            {
                MessageCode = response.MessageCode,
                InnerBody = response.InnerBody,
                StatusCode = EnumStatusCode.StatusCode200
            });
        }

    }
}
