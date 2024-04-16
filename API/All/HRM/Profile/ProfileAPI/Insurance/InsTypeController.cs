using API.All.DbContexts;
using API.DTO;
using API.Main;
using CORE.DTO;
using CORE.Enum;
using CORE.GenericUOW;
using CORE.StaticConstant;
using DocumentFormat.OpenXml.InkML;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace API.All.HRM.Profile.ProfileAPI.Insurance
{
    [HiStaffAuthorize]
    [ApiExplorerSettings(GroupName = "002-PROFILE-INS-TYPE")]
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class InsTypeController : ControllerBase
    {
        private readonly GenericUnitOfWork _uow;
        private readonly ProfileDbContext _profileDbContext;
        private readonly FullDbContext _dbContext;
        private IGenericRepository<INS_TYPE, InsTypeDTO> _genericRepository;
        private readonly GenericReducer<INS_TYPE, InsTypeDTO> genericReducer;
        private readonly AppSettings _appSettings;

        public InsTypeController(ProfileDbContext profileDbContext, FullDbContext context, IOptions<AppSettings> options)
        {
            _uow = new GenericUnitOfWork(profileDbContext);
            _profileDbContext = profileDbContext;
            _dbContext = context;
            _genericRepository = _uow.GenericRepository<INS_TYPE, InsTypeDTO>();
            _appSettings = options.Value;
            genericReducer = new();
        }

        [HttpPost]
        public async Task<IActionResult> QueryList(GenericQueryListDTO<InsTypeDTO> request)
        {
            try
            {
                var joined = from p in _profileDbContext.InsuranceTypes.AsNoTracking()
                                 // JOIN OTHER ENTITIES BASED ON THE BUSINESS
                             from o in _profileDbContext.OtherLists.AsNoTracking().Where(x => x.ID == p.TYPE_ID).DefaultIfEmpty()
                             select new InsTypeDTO
                             {
                                 Id = p.ID,
                                 Code = p.CODE,
                                 Name = p.NAME,
                                 TypeId = p.TYPE_ID,
                                 TypeName = o.NAME,
                                 Note = p.NOTE,
                                 Status = p.IS_ACTIVE == true ? "Áp dụng" : "Ngừng áp dụng"
                             };

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
        [HttpPost]
        public async Task<IActionResult> Create(InsTypeDTO model)
        {
            try
            {
                model.IsActive = true;
                var response = await _genericRepository.Create(_uow, model, Request.Sid(_appSettings) ?? "");
                return Ok(response);
            }
            catch (Exception ex)
            {
                return Ok(new FormatedResponse() { MessageCode = ex.Message, ErrorType = EnumErrorType.UNCATCHABLE, StatusCode = EnumStatusCode.StatusCode500 });
            }
        }

        //[HttpGet]
        //public async Task<IActionResult> GetById(long id)
        //{
        //    try
        //    {
        //        var joined = (from p in _profileDbContext.InsuranceTypes.AsNoTracking().Where(x => x.ID == id)
        //                      from o in _profileDbContext.OtherLists.AsNoTracking().Where(x => x.ID == p.TYPE_ID).DefaultIfEmpty()
        //                      select new InsTypeDTO
        //                      {
        //                          Id = p.ID,
        //                          Code = p.CODE,
        //                          TypeName = o.NAME,
        //                          Note = p.NOTE,

        //                      }).FirstOrDefaultAsync();
        //        return Ok(new FormatedResponse() { InnerBody = joined });
        //    }
        //    catch (Exception ex)
        //    {
        //        return Ok(new FormatedResponse() { MessageCode = ex.Message, ErrorType = EnumErrorType.UNCATCHABLE, StatusCode = EnumStatusCode.StatusCode500 });
        //    }
        //}

        [HttpGet]
        public async Task<IActionResult> GetInsTypeById(long id)
        {
            try
            {
                var joined = (from p in _profileDbContext.InsuranceTypes.AsNoTracking().Where(x => x.ID == id)
                              from o in _profileDbContext.OtherLists.AsNoTracking().Where(x => x.ID == p.TYPE_ID).DefaultIfEmpty()
                              select new InsTypeDTO
                              {
                                  Id = p.ID,
                                  Code = p.CODE,
                                  Name = p.NAME,
                                  TypeId = p.TYPE_ID,
                                  TypeName = o.NAME,
                                  Note = p.NOTE,

                              }).FirstOrDefault();

                return Ok(new FormatedResponse() { InnerBody = joined });
            }
            catch (Exception ex)
            {
                return Ok(new FormatedResponse() { MessageCode = ex.Message, ErrorType = EnumErrorType.UNCATCHABLE, StatusCode = EnumStatusCode.StatusCode500 });
            }
        }
        [HttpGet]
        public async Task<IActionResult> GetListInsType()

        {
            try
            {
                var query = await (from p in _profileDbContext.OtherLists.AsNoTracking().Where(x => x.TYPE_ID == 83)
                                   select new
                                   {
                                       Id = p.ID,
                                       Name = p.NAME
                                   }).ToListAsync();
                return Ok(new FormatedResponse()
                {
                    InnerBody = query
                });
            }
            catch (Exception ex)
            {

                return Ok(new FormatedResponse() { ErrorType = EnumErrorType.UNCATCHABLE, StatusCode = EnumStatusCode.StatusCode500, MessageCode = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Update(InsTypeDTO model)
        {
            try
            {
                //SYS_MENU entity = new();
                model.UpdatedDate = DateTime.Now;
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
                //check ban ghi phat sinh tai cac man #
                foreach (var item in model.Ids)
                {
                    var check1 = _dbContext.InsArisings.Where(x => x.INS_TYPE_ID == item).AsNoTracking().Count();
                    var check2 = _dbContext.InsChanges.Where(x => x.UNIT_INSURANCE_TYPE_ID == item).AsNoTracking().Count();
                    if (check1 > 0)
                    {
                        return Ok(new FormatedResponse() { ErrorType = EnumErrorType.CATCHABLE, MessageCode = CommonMessageCode.DO_NOT_DELETE_DATA_RECORD_ARISING, StatusCode = EnumStatusCode.StatusCode400 });
                    }else if (check2 > 0)
                    {
                        return Ok(new FormatedResponse() { ErrorType = EnumErrorType.CATCHABLE, MessageCode = CommonMessageCode.DO_NOT_DELETE_DATA_RECORD_CHANGE, StatusCode = EnumStatusCode.StatusCode400 });
                    }
                }

                if (model.Ids != null)
                {
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
        public async Task<IActionResult> ToggleActiveIds(GenericToggleIsActiveDTO model)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _genericRepository.ToggleActiveIds(_uow, model.Ids, model.ValueToBind, sid);
            return Ok(response);
        }
        //public virtual async Task<FormatedResponse> ToggleActiveIds(GenericUnitOfWork _uow, List<long> ids, bool valueToBind, string sid)
        //{
        //    var response = await _genericRepository.ToggleActiveIds(_uow, ids, valueToBind, sid);
        //    return response;
        //}
    }
}
    
