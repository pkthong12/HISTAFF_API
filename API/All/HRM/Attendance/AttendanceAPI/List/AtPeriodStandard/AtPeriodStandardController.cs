using API.All.DbContexts;
using API.DTO;
using API.Main;
using AttendanceDAL.ViewModels;
using CORE.DTO;
using CORE.Enum;
using CORE.GenericUOW;
using CORE.StaticConstant;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ProfileDAL.ViewModels;
using System.Text.RegularExpressions;

namespace API.Controllers.AtPeriodStandard
{
    [ApiExplorerSettings(GroupName = "013-ATTENDANCE-AT_PERIOD_STANDARD")]
    [ApiController]
    [HiStaffAuthorize]
    [Route("api/[controller]/[action]")]
    public class AtPeriodStandardController : ControllerBase
    {
        private readonly GenericUnitOfWork _uow;
        private IGenericRepository<AT_PERIOD_STANDARD, AtPeriodStandardDTO> _genericRepository;
        private readonly GenericReducer<AT_PERIOD_STANDARD, AtPeriodStandardDTO> genericReducer;
        private AppSettings _appSettings;
        public AtPeriodStandardController(IOptions<AppSettings> options, AttendanceDbContext attendanceDbContext)
        {
            _uow = new GenericUnitOfWork(attendanceDbContext);
            _genericRepository = _uow.GenericRepository<AT_PERIOD_STANDARD, AtPeriodStandardDTO>();
            genericReducer = new();
            _appSettings = options.Value;
        }

        [HttpPost]
        public async Task<IActionResult> QueryList(GenericQueryListStringIdDTO<AtPeriodStandardDTO> request)
        {
            try
            {
                var status = "";
                if (request.Search != null)
                {
                    request.Search.ForEach(x =>
                    {
                        if (x.Field == "status")
                        {
                            status = x.SearchFor.ToUpper();
                            x.SearchFor = "";
                        }
                    });
                }
                var entity = _uow.Context.Set<AT_PERIOD_STANDARD>().AsNoTracking().AsQueryable();
                var periods = _uow.Context.Set<AT_SALARY_PERIOD>().AsNoTracking().AsQueryable();
                var otherlists = _uow.Context.Set<SYS_OTHER_LIST>().AsNoTracking().AsQueryable();
                var joined = from p in entity.Where(p=>(!status.IsNullOrEmpty() && "NGỪNG".Contains(status.ToUpper().Trim()))?  p.IS_ACTIVE !=true : true)
                             from a in periods.Where(x => x.ID == p.PERIOD_ID).DefaultIfEmpty()
                             from c in otherlists.Where(x => x.ID == p.OBJECT_ID).DefaultIfEmpty()
                             select new AtPeriodStandardDTO
                             {
                                 Id = p.ID,
                                 Note = p.NOTE,
                                 PeriodId = p.PERIOD_ID,
                                 PeriodName = a.NAME,
                                 ObjectId = p.OBJECT_ID,
                                 ObjectName = c.NAME,
                                 PeriodStandard = p.PERIOD_STANDARD,
                                 PeriodStandardNight = p.PERIOD_STANDARD_NIGHT,
                                 Year = p.YEAR,
                                 IsActive = p.IS_ACTIVE,
                                 UpdatedBy = p.UPDATED_BY,
                                 UpdatedDate = p.UPDATED_DATE,
                                 CreatedDate = p.CREATED_DATE,
                                 Status = (p.IS_ACTIVE.HasValue && p.IS_ACTIVE.Value) ? "Áp dụng" : "Ngừng áp dụng"
                             };
                request.Sort = new List<SortItem>();
                request.Sort.Add(new SortItem() { Field = "CreatedDate", SortDirection = EnumSortDirection.DESC });
                
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
                        ErrorType = response.ErrorType,
                        MessageCode = response.MessageCode ?? CommonMessageCode.NOT_ALL_CASES_CATCHED,
                        StatusCode = EnumStatusCode.StatusCode200,
                        InnerBody = response
                    }) ;
                }
            }
            catch (Exception ex)
            {
                return Ok(new FormatedResponse() { ErrorType = EnumErrorType.UNCATCHABLE, StatusCode = EnumStatusCode.StatusCode500, MessageCode = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetPeriod(long year)
        {

            try
            {
                var entity = _uow.Context.Set<AT_SALARY_PERIOD>().AsNoTracking().AsQueryable();
                var joined = from p in entity
                             where p.YEAR == year
                             orderby p.START_DATE
                             select new SalaryPeriodDTO
                             {
                                 Id = p.ID,
                                 Name = p.NAME,
                                 Month = p.MONTH
                             };
                return Ok(new FormatedResponse() { InnerBody = joined });
            }
            catch (Exception ex)
            {
                return Ok(new FormatedResponse() { ErrorType = EnumErrorType.UNCATCHABLE, StatusCode = EnumStatusCode.StatusCode500, MessageCode = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetPeriodById(long id)
        {
            try
            {
                var entity = _uow.Context.Set<AT_SALARY_PERIOD>().AsNoTracking().AsQueryable();
                var joined = await (from p in entity
                                    where p.ID == id
                                    select new AtSalaryPeriodDTO
                                    {
                                        Id = p.ID,
                                        Name = p.NAME
                                    }).FirstOrDefaultAsync();
                return Ok(new FormatedResponse() { InnerBody = joined });
            }
            catch (Exception ex)
            {
                return Ok(new FormatedResponse() { MessageCode = ex.Message, ErrorType = EnumErrorType.UNCATCHABLE, StatusCode = EnumStatusCode.StatusCode500 });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetById(long id)
        {
            try
            {
                var entity = _uow.Context.Set<AT_PERIOD_STANDARD>().AsNoTracking().AsQueryable();
                var periods = _uow.Context.Set<AT_SALARY_PERIOD>().AsNoTracking().AsQueryable();
                var otherlists = _uow.Context.Set<SYS_OTHER_LIST>().AsNoTracking().AsQueryable();
                var joined = await (from p in entity
                                    from a in periods.Where(x => x.ID == p.PERIOD_ID).DefaultIfEmpty()
                                    from c in otherlists.Where(x => x.ID == p.OBJECT_ID).DefaultIfEmpty()
                                    where p.ID == id
                                    select new AtPeriodStandardDTO
                                    {
                                        Id = p.ID,
                                        Note = p.NOTE,
                                        PeriodId = p.PERIOD_ID,
                                        PeriodName = a.NAME,
                                        ObjectId = p.OBJECT_ID,
                                        ObjectName = c.NAME,
                                        PeriodStandard = p.PERIOD_STANDARD,
                                        PeriodStandardNight = p.PERIOD_STANDARD_NIGHT,
                                        Year = p.YEAR,
                                        IsActive = p.IS_ACTIVE,
                                        UpdatedBy = p.UPDATED_BY,
                                        UpdatedDate = p.UPDATED_DATE,
                                        CreatedDate = p.CREATED_DATE,
                                        Status = (p.IS_ACTIVE.HasValue && p.IS_ACTIVE.Value) ? "Áp dụng" : "Ngừng áp dụng"
                                    }).FirstOrDefaultAsync();
                return Ok(new FormatedResponse() { InnerBody = joined });
            }
            catch (Exception ex)
            {
                return Ok(new FormatedResponse() { MessageCode = ex.Message, ErrorType = EnumErrorType.UNCATCHABLE, StatusCode = EnumStatusCode.StatusCode500 });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create(AtPeriodStandardDTO model)
        {
            try
            {
                var entity = _uow.Context.Set<AT_PERIOD_STANDARD>().AsNoTracking().AsQueryable();

                // check for duplicates
                bool checkForDuplicates = entity
                                          .Any(x =>
                                              x.YEAR == model.Year
                                              && x.PERIOD_ID == model.PeriodId
                                              && x.OBJECT_ID == model.ObjectId
                                              );

                if (checkForDuplicates)
                {
                    return Ok(new FormatedResponse()
                    {
                        ErrorType = EnumErrorType.CATCHABLE,
                        MessageCode = "Không được trùng năm, kỳ công, đối tượng công",
                        StatusCode = EnumStatusCode.StatusCode400
                    });
                }


                model.IsActive = true;
                var sid = Request.Sid(_appSettings);
                var response = await _genericRepository.Create(_uow, model, sid ?? "");
                response.MessageCode = CommonMessageCode.CREATE_SUCCESS;
                return Ok(response);
            }
            catch (Exception ex)
            {
                return Ok(new FormatedResponse() { MessageCode = ex.Message, ErrorType = EnumErrorType.UNCATCHABLE, StatusCode = EnumStatusCode.StatusCode500 });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Update(AtPeriodStandardDTO model)
        {
            try
            {
                var entity = _uow.Context.Set<AT_PERIOD_STANDARD>().AsNoTracking().AsQueryable();

                // check for duplicates
                bool checkForDuplicates = entity
                                          .Any(x =>
                                              x.YEAR == model.Year
                                              && x.PERIOD_ID == model.PeriodId
                                              && x.OBJECT_ID == model.ObjectId
                                              && x.ID != model.Id
                                              );

                if (checkForDuplicates)
                {
                    return Ok(new FormatedResponse()
                    {
                        ErrorType = EnumErrorType.CATCHABLE,
                        MessageCode = "Không được trùng năm, kỳ công, đối tượng công",
                        StatusCode = EnumStatusCode.StatusCode400
                    });
                }


                var response = await _genericRepository.Update(_uow, model, Request.Sid(_appSettings));
                return Ok(response);
            }
            catch (Exception ex)
            {
                return Ok(new FormatedResponse() { MessageCode = ex.Message, ErrorType = EnumErrorType.UNCATCHABLE, StatusCode = EnumStatusCode.StatusCode500 });
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteIds(IdsRequest model)
        {
            try
            {
                if (model.Ids != null)
                {
                    var entity = _uow.Context.Set<AT_PERIOD_STANDARD>().AsNoTracking().AsQueryable();
                    var checkActive = await entity.AsNoTracking().Where(p => model.Ids.Contains(p.ID) && p.IS_ACTIVE == true).CountAsync() > 0 ? true : false;

                    if (checkActive)
                    {
                        return Ok(new FormatedResponse() { ErrorType = EnumErrorType.CATCHABLE, MessageCode = CommonMessageCode.DO_NOT_DELETE_DATA_IS_ACTIVE, StatusCode = EnumStatusCode.StatusCode400 });
                    }
                    var response = await _genericRepository.DeleteIds(_uow, model.Ids);
                    return Ok(response);
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
        public async Task<IActionResult> Delete(AtPeriodStandardDTO model)
        {
            try
            {
                if (model.Id != null)
                {
                    var response = await _genericRepository.Delete(_uow, (long)model.Id);
                    return Ok(response);
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

