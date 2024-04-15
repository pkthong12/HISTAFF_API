using Microsoft.AspNetCore.Mvc;
using ProfileDAL.ViewModels;
using API.All.DbContexts;
using API.Entities;
using CORE.DTO;
using CORE.Enum;
using CORE.GenericUOW;
using CORE.StaticConstant;
using API.Main;
using API;
using Microsoft.Extensions.Options;

namespace ProfileAPI.List
{
    [HiStaffAuthorize]
    [ApiExplorerSettings(GroupName = "002-PROFILE-SALARY-RANK")]
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class HuSalaryRankController : ControllerBase
    {
        private readonly GenericUnitOfWork _uow;
        private readonly ProfileDbContext _profileDbContext;
        private IGenericRepository<HU_SALARY_RANK, SalaryRankDTO> _genericRepository;
        private readonly GenericReducer<HU_SALARY_RANK, SalaryRankDTO> genericReducer;


        // khai báo thêm
        private readonly AppSettings _appSettings;

        public HuSalaryRankController(
            ProfileDbContext profileDbContext,
            IOptions<AppSettings> options
        )
        {
            _profileDbContext = profileDbContext;
            _uow = new GenericUnitOfWork(_profileDbContext);
            _genericRepository = _uow.GenericRepository<HU_SALARY_RANK, SalaryRankDTO>();
            genericReducer = new();
            _appSettings = options.Value;
        }

        [HttpPost]
        public async Task<IActionResult> QueryList(GenericQueryListStringIdDTO<SalaryRankDTO> request)
        {
            try
            {
                var entity = _profileDbContext.Set<HU_SALARY_RANK>().AsNoTracking().AsQueryable();
                var joined = from p in entity
                             from r in _profileDbContext.SalaryScales.Where(x => x.ID == p.SALARY_SCALE_ID).DefaultIfEmpty()
                             select new SalaryRankDTO
                             {
                                 Id = p.ID,
                                 Code = p.CODE,
                                 Name = p.NAME,
                                 SalaryScaleId = p.SALARY_SCALE_ID,
                                 SalaryScaleName = r.NAME,
                                 Note = p.NOTE,
                                 IsActive = p.IS_ACTIVE,
                                 UpdatedBy = p.UPDATED_BY,
                                 UpdatedDate = p.UPDATED_DATE,
                                 EffectiveDate = p.EFFECTIVE_DATE,
                                 ExpirationDate = p.EXPIRATION_DATE,
                                 Status = (p.IS_ACTIVE.HasValue && p.IS_ACTIVE.Value) ? "Áp dụng" : "Ngừng áp dụng"
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

        [HttpGet]
        public async Task<IActionResult> GetCode()
        {
            decimal num;
            var queryCode = (from x in _profileDbContext.SalaryRanks where x.CODE.Length == 5 select x.CODE).ToList();
            var existingCode = (from p in queryCode where Decimal.TryParse(p.Substring(2), out num) orderby p descending select p).ToList();
            string newcode = StringCodeGenerator.CreateNewCode("NL", 3, existingCode);
            return Ok(new FormatedResponse() { InnerBody = new { Code = newcode } });
        }

        [HttpGet]
        public async Task<IActionResult> GetById(long id)
        {
            try
            {
                var objRes = await (from p in _profileDbContext.SalaryRanks
                                    where p.ID == id
                                    select new
                                    {
                                        Id = p.ID,
                                        Code = p.CODE,
                                        Name = p.NAME,
                                        SalaryScaleId = p.SALARY_SCALE_ID,
                                        Note = p.NOTE,
                                        EffectiveDate = p.EFFECTIVE_DATE,
                                        ExpirationDate = p.EXPIRATION_DATE,
                                    }).FirstOrDefaultAsync();
                return Ok(new FormatedResponse() { InnerBody = objRes });
            }
            catch (Exception ex)
            {
                return Ok(new FormatedResponse() { MessageCode = ex.Message, ErrorType = EnumErrorType.UNCATCHABLE, StatusCode = EnumStatusCode.StatusCode500 });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create(SalaryRankInputDTO request)
        {
            try
            {
                if (request.EffectiveDate > request.ExpirationDate)
                {
                    return Ok(new FormatedResponse() { MessageCode = CommonMessageCode.EFFECTIVEDATE_NOT_BIGGER_THAN_EXPIRATIONDATE, ErrorType = EnumErrorType.UNCATCHABLE, StatusCode = EnumStatusCode.StatusCode500 });
                }
                var checkListName = _profileDbContext.SalaryRanks.Where(x => x.SALARY_SCALE_ID == request.SalaryScaleId).ToList();
                if (checkListName.Count > 0)
                {
                    foreach (var item in checkListName)
                    {
                        if (item.NAME == request.Name)
                        {
                            return Ok(new FormatedResponse() { MessageCode = CommonMessageCode.NOT_THE_SAME_NAME_IN_SAME_SCALE_NAME, ErrorType = EnumErrorType.UNCATCHABLE, StatusCode = EnumStatusCode.StatusCode500 });
                        }
                    }
                }
                var check = _profileDbContext.SalaryRanks.Where(x => x.CODE.ToUpper().Equals(request.Code.ToUpper())).Any();
                if (check) return Ok(new FormatedResponse() { MessageCode = CommonMessageCode.SALARY_RANK_CODE_EXISTED, ErrorType = EnumErrorType.UNCATCHABLE, StatusCode = EnumStatusCode.StatusCode500 });
                HU_SALARY_RANK objData = new()
                {
                    CODE = request.Code,
                    NAME = request.Name,
                    NOTE = request.Note,
                    SALARY_SCALE_ID = request.SalaryScaleId,
                    IS_ACTIVE = true,
                    CREATED_DATE = DateTime.Now,
                    EFFECTIVE_DATE = request.EffectiveDate,
                    EXPIRATION_DATE = request.ExpirationDate,
                };
                _profileDbContext.SalaryRanks.Add(objData);
                await _profileDbContext.SaveChangesAsync();
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
        public async Task<IActionResult> Update(SalaryRankInputDTO request)
        {
            try
            {
                if (request.EffectiveDate > request.ExpirationDate)
                {
                    return Ok(new FormatedResponse() { MessageCode = CommonMessageCode.EFFECTIVEDATE_NOT_BIGGER_THAN_EXPIRATIONDATE, ErrorType = EnumErrorType.UNCATCHABLE, StatusCode = EnumStatusCode.StatusCode500 });
                }
                if (request.Id == null) return Ok(new FormatedResponse() { MessageCode = CommonMessageCode.ENTITY_NOT_FOUND, ErrorType = EnumErrorType.UNCATCHABLE, StatusCode = EnumStatusCode.StatusCode404 });
                var objData = _profileDbContext.SalaryRanks.Where(x => x.ID == request.Id.Value).FirstOrDefault();
                if (objData == null) return Ok(new FormatedResponse() { MessageCode = CommonMessageCode.ENTITY_NOT_FOUND, ErrorType = EnumErrorType.UNCATCHABLE, StatusCode = EnumStatusCode.StatusCode404 });

                //var check = _profileDbContext.SalaryRanks.Where(x => x.CODE.ToUpper().Equals(request.Code.ToUpper()) && x.ID != request.Id.Value).Any();
                //if (check) return Ok(new FormatedResponse() { MessageCode = "SALARY_RANK_CODE_EXISTED", ErrorType = EnumErrorType.UNCATCHABLE, StatusCode = EnumStatusCode.StatusCode500 });

                objData.CODE = request.Code;
                objData.NAME = request.Name;
                objData.NOTE = request.Note;
                objData.SALARY_SCALE_ID = request.SalaryScaleId;

                await _profileDbContext.SaveChangesAsync();
                return Ok(new FormatedResponse()
                {
                    InnerBody = request,
                    MessageCode = CommonMessageCode.UPDATE_SUCCESS,
                    StatusCode = EnumStatusCode.StatusCode200,
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
                    foreach (var id in req.Ids)
                    {
                        var item = await _profileDbContext.SalaryRanks.Where(x => x.ID == id).AsNoTracking().SingleOrDefaultAsync();
                        if (item != null && item.IS_ACTIVE == true)
                        {
                            return Ok(new FormatedResponse() { ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400, MessageCode = CommonMessageCode.DO_NOT_DELETE_DATA_IS_ACTIVE });
                        }
                    }
                    var response = await _genericRepository.DeleteIds(_uow, req.Ids);
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

        [HttpGet]
        public async Task<IActionResult> GetScales()
        {
            try
            {
                var query = await (from p in _profileDbContext.SalaryScales.Where(x => x.IS_ACTIVE.HasValue && x.IS_ACTIVE.Value)
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

        [HttpGet]
        public async Task<IActionResult> GetRankByScaleId(int scaleId)
        {
            try
            {
                var query = await _profileDbContext.SalaryRanks.Where(x => x.SALARY_SCALE_ID == scaleId && x.IS_ACTIVE == true).AsNoTracking().ToListAsync();
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

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var entity = _profileDbContext.Set<HU_SALARY_RANK>().AsNoTracking().AsQueryable();
                return Ok(new FormatedResponse()
                {
                    InnerBody = entity
                });
            }
            catch (Exception ex)
            {
                return Ok(new FormatedResponse() { ErrorType = EnumErrorType.UNCATCHABLE, StatusCode = EnumStatusCode.StatusCode500, MessageCode = ex.Message });
            }
        }


        // làm chức năng
        // chuyển trạng thái
        [HttpPost]
        public async Task<IActionResult> ToggleActiveIds(GenericToggleIsActiveDTO model)
        {
            // khai báo biến sid
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();


            // lấy tất cả bản ghi
            // trong bảng HU_SALARY_RANK
            var allRecord = (from item in _profileDbContext.SalaryRanks
                             select item).ToList();


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
