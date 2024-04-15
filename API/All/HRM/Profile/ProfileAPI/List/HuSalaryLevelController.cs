using API;
using API.All.DbContexts;
using API.Entities;
using API.Main;
using Azure.Core;
using CORE.DTO;
using CORE.Enum;
using CORE.GenericUOW;
using CORE.StaticConstant;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using ProfileDAL.ViewModels;

namespace ProfileAPI.List
{
    [HiStaffAuthorize]
    [ApiExplorerSettings(GroupName = "002-PROFILE-SALARY-LEVEL")]
    [ApiController]

    [Route("api/[controller]/[action]")]
    public class HuSalaryLevelController : ControllerBase
    {
        private readonly GenericUnitOfWork _uow;
        private readonly ProfileDbContext _profileDbContext;
        private IGenericRepository<HU_SALARY_LEVEL, SalaryLevelDTO> _genericRepository;
        private readonly GenericReducer<HU_SALARY_LEVEL, SalaryLevelDTO> genericReducer;


        // khai báo thêm
        private readonly AppSettings _appSettings;


        public HuSalaryLevelController(
            ProfileDbContext profileDbContext,
            IOptions<AppSettings> options
        )
        {
            _profileDbContext = profileDbContext;
            _uow = new GenericUnitOfWork(_profileDbContext);
            _genericRepository = _uow.GenericRepository<HU_SALARY_LEVEL, SalaryLevelDTO>();
            genericReducer = new();
            _appSettings = options.Value;
        }

        [HttpPost]
        public async Task<IActionResult> QueryList(GenericQueryListStringIdDTO<SalaryLevelDTO> request)
        {
            try
            {

                var entity = _profileDbContext.Set<HU_SALARY_LEVEL>().AsNoTracking().AsQueryable();
                var insRegion = _profileDbContext.Set<INS_REGION>().AsNoTracking().AsQueryable();
                var salaryScale = _profileDbContext.Set<HU_SALARY_SCALE>().AsNoTracking().AsQueryable();
                var salaryRank = _profileDbContext.Set<HU_SALARY_RANK>().AsNoTracking().AsQueryable();
                var otherList = _profileDbContext.Set<SYS_OTHER_LIST>().AsNoTracking().AsQueryable();
                var joined = (from p in entity
                              from r in salaryScale.Where(x => x.ID == p.SALARY_SCALE_ID).DefaultIfEmpty()
                              from f in salaryRank.Where(x => x.ID == p.SALARY_RANK_ID).DefaultIfEmpty()
                              from t in insRegion.Where(x => x.ID == p.REGION_ID).DefaultIfEmpty()
                              from o in otherList.Where(x => x.ID == t.AREA_ID).DefaultIfEmpty()
                              select new SalaryLevelDTO
                              {
                                  Id = p.ID,
                                  Code = p.CODE,
                                  Name = p.NAME,
                                  SalaryScaleId = p.SALARY_SCALE_ID,
                                  SalaryRankId = p.SALARY_RANK_ID,
                                  SalaryRankName = f.NAME,
                                  PerformBonus = p.PERFORM_BONUS,
                                  OtherBonus = p.OTHER_BONUS,
                                  SalaryScaleName = r.NAME,
                                  Note = p.NOTE,
                                  IsActive = p.IS_ACTIVE,
                                  TotalSalary = p.COEFFICIENT * t.MONEY, //Tổng lương bằng hệ số nhân lương cơ bản
                                  UpdatedBy = p.UPDATED_BY,
                                  ExpirationDate = p.EXPIRATION_DATE,
                                  EffectiveDate = p.EFFECTIVE_DATE,
                                  Monney = t.MONEY,
                                  UpdatedDate = p.UPDATED_DATE,
                                  HoldingMonth = p.HOLDING_MONTH,
                                  HoldingTime = p.HOLDING_TIME,
                                  Coefficient = p.COEFFICIENT,
                                  RegionName = o.NAME,
                                  Status = (p.IS_ACTIVE.HasValue && p.IS_ACTIVE.Value == true) ? "Áp dụng" : "Ngừng áp dụng"
                              });

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
            var queryCode = (from x in _profileDbContext.SalaryLevels where x.CODE.Length == 5 select x.CODE).ToList();
            var existingCode = (from p in queryCode where Decimal.TryParse(p.Substring(2), out num) orderby p descending select p).ToList();
            string newcode = StringCodeGenerator.CreateNewCode("BL", 3, existingCode);
            return Ok(new FormatedResponse() { InnerBody = new { Code = newcode } });
        }
        [HttpPost]
        public async Task<IActionResult> Create(SalaryLevelInputDTO request)
        {
            try
            {
                if (request.EffectiveDate > request.ExpirationDate)
                {
                    return Ok(new FormatedResponse() { MessageCode = CommonMessageCode.EFFECTIVEDATE_NOT_BIGGER_THAN_EXPIRATIONDATE, ErrorType = EnumErrorType.UNCATCHABLE, StatusCode = EnumStatusCode.StatusCode500 });
                }

                var check = _profileDbContext.SalaryLevels.Where(x => x.CODE.ToUpper().Equals(request.Code.ToUpper())).Any();
                var listSameRankAndScale = _profileDbContext.SalaryLevels.Where(x => x.SALARY_RANK_ID == request.SalaryRankId && x.SALARY_SCALE_ID == request.SalaryScaleId).ToList();
                if (listSameRankAndScale.Count > 0)
                {
                    foreach (var item in listSameRankAndScale)
                    {
                        if (item.NAME == request.Name)
                        {
                            return Ok(new FormatedResponse() { MessageCode = "CANNOT_HAVE_THE_SAME_NAME_WHEN_HAVE_SAME_SALARY_LEVEL_AND_SALARY_SCALE", ErrorType = EnumErrorType.UNCATCHABLE, StatusCode = EnumStatusCode.StatusCode500 });
                        }
                    }
                }

                if (check) return Ok(new FormatedResponse() { MessageCode = "SALARY_LEVEL_CODE_EXISTED", ErrorType = EnumErrorType.UNCATCHABLE, StatusCode = EnumStatusCode.StatusCode500 });
                HU_SALARY_LEVEL objData = new()
                {
                    CODE = request.Code,
                    NAME = request.Name,
                    NOTE = request.Note,
                    IS_ACTIVE = true,
                    ORDERS = request.Orders,
                    CREATED_DATE = DateTime.Now,
                    SALARY_SCALE_ID = request.SalaryScaleId,
                    SALARY_RANK_ID = request.SalaryRankId,
                    OTHER_BONUS = request.OtherBonus,
                    PERFORM_BONUS = request.PerformBonus,
                    COEFFICIENT = request.Coefficient,
                    HOLDING_MONTH = request.HoldingMonth,
                    EFFECTIVE_DATE = request.EffectiveDate,
                    EXPIRATION_DATE = request.ExpirationDate,
                    HOLDING_TIME = request.HoldingMonth.HasValue ? DateTime.Now.AddMonths(request.HoldingMonth.Value) : null,
                    REGION_ID = request.RegionId,
                };
                _profileDbContext.SalaryLevels.Add(objData);
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

        [HttpGet]
        public async Task<IActionResult> GetById(long id)
        {
            try
            {
                var objRes = await (from p in _profileDbContext.SalaryLevels
                                    where p.ID == id
                                    select new
                                    {
                                        Id = p.ID,
                                        Code = p.CODE,
                                        Name = p.NAME,
                                        Note = p.NOTE,
                                        Order = p.ORDERS,
                                        Coefficient = p.COEFFICIENT,
                                        SalaryRankId = p.SALARY_RANK_ID,
                                        SalaryScaleId = p.SALARY_SCALE_ID,
                                        RegionId = p.REGION_ID,
                                        OtherBonus = p.OTHER_BONUS,
                                        HoldingMonth = p.HOLDING_MONTH,
                                        ExpirationDate = p.EXPIRATION_DATE,
                                        EffectiveDate = p.EFFECTIVE_DATE,
                                        PerformBonus = p.PERFORM_BONUS
                                    }).FirstOrDefaultAsync();
                return Ok(new FormatedResponse() { InnerBody = objRes });
            }
            catch (Exception ex)
            {
                return Ok(new FormatedResponse() { MessageCode = ex.Message, ErrorType = EnumErrorType.UNCATCHABLE, StatusCode = EnumStatusCode.StatusCode500 });
            }
        }


        [HttpPost]
        public async Task<IActionResult> Update(SalaryLevelInputDTO request)
        {
            try
            {
                if (request.EffectiveDate > request.ExpirationDate)
                {
                    return Ok(new FormatedResponse() { MessageCode = CommonMessageCode.EFFECTIVEDATE_NOT_BIGGER_THAN_EXPIRATIONDATE, ErrorType = EnumErrorType.UNCATCHABLE, StatusCode = EnumStatusCode.StatusCode500 });
                }
                if (request.Id == null) return Ok(new FormatedResponse() { MessageCode = CommonMessageCode.ENTITY_NOT_FOUND, ErrorType = EnumErrorType.UNCATCHABLE, StatusCode = EnumStatusCode.StatusCode404 });
                var objData = _profileDbContext.SalaryLevels.Where(x => x.ID == request.Id.Value).FirstOrDefault();
                if (objData == null) return Ok(new FormatedResponse() { MessageCode = CommonMessageCode.ENTITY_NOT_FOUND, ErrorType = EnumErrorType.UNCATCHABLE, StatusCode = EnumStatusCode.StatusCode404 });

                //var check = _profileDbContext.SalaryLevels.Where(x => x.CODE.ToUpper().Equals(request.Code.ToUpper()) && x.ID != request.Id.Value).Any();
                //if (check) return Ok(new FormatedResponse() { MessageCode = "SALARY_LEVEL_CODE_EXISTED", ErrorType = EnumErrorType.UNCATCHABLE, StatusCode = EnumStatusCode.StatusCode500 });

                objData.CODE = request.Code;
                objData.NAME = request.Name;
                objData.NOTE = request.Note;
                objData.SALARY_SCALE_ID = request.SalaryScaleId;
                objData.SALARY_RANK_ID = request.SalaryRankId;
                objData.HOLDING_MONTH = request.HoldingMonth;
                objData.COEFFICIENT = request.Coefficient;

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
                        var item = await _profileDbContext.SalaryLevels.AsNoTracking().Where(x => x.ID == id).FirstOrDefaultAsync();
                        var getWorking = _profileDbContext.Workings.AsNoTracking().Where( x => x.SALARY_LEVEL_ID == id).FirstOrDefault();
                        if (item != null && item.IS_ACTIVE == true)
                        {
                            return Ok(new FormatedResponse() { ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400, MessageCode = CommonMessageCode.DO_NOT_DELETE_DATA_IS_ACTIVE });
                        }
                        else if(getWorking != null)
                        {
                            return Ok(new FormatedResponse() { StatusCode = EnumStatusCode.StatusCode400, ErrorType = EnumErrorType.CATCHABLE, MessageCode = CommonMessageCode.CAN_NOT_DELETE_RECORDS_HAVE_USE });
                        }
                    }
                    var response = await _genericRepository.DeleteIds(_uow, req.Ids);
                    return Ok(response);
                }
                else
                {
                    return Ok(new FormatedResponse() { ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400, MessageCode = CommonMessageCode.DELETE_REQUEST_NULL_ID });
                }
            }
            catch (Exception ex)
            {
                return Ok(new FormatedResponse() { ErrorType = EnumErrorType.UNCATCHABLE, StatusCode = EnumStatusCode.StatusCode500, MessageCode = ex.Message });
            }
        }
        [HttpGet]
        public async Task<IActionResult> GetLevelByRankId(long rankId)
        {
            try
            {
                var query = await _profileDbContext.SalaryLevels.AsNoTracking().Where(x => x.SALARY_RANK_ID == rankId && x.IS_ACTIVE == true).ToListAsync();
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


        // làm chức năng
        // chuyển trạng thái
        [HttpPost]
        public async Task<IActionResult> ToggleActiveIds(GenericToggleIsActiveDTO model)
        {
            // khai báo biến sid
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();


            // lấy tất cả bản ghi
            // trong bảng HU_SALARY_LEVEL
            var allRecord = (from item in _profileDbContext.SalaryLevels
                             select item).ToList();


            var response = await _genericRepository.ToggleActiveIds(_uow, model.Ids, model.ValueToBind, sid);
            return Ok(new FormatedResponse()
            {
                MessageCode = response.MessageCode,
                InnerBody = response.InnerBody,
                StatusCode = EnumStatusCode.StatusCode200
            });
        }



        //[HttpGet]
        //public async Task<ActionResult> GetAll(SalaryLevelDTO param)
        //{
        //    var r = await _profileDbContext.SalaryLevelRepository.GetAll(param);
        //    return Ok(r);
        //}
        //[HttpGet]
        //public async Task<ActionResult> Get(int Id)
        //{
        //    var r = await _profileDbContext.SalaryLevelRepository.GetById(Id);
        //    return Ok(r);
        //}
        //[HttpPost]
        //public async Task<ActionResult> Add([FromBody] SalaryLevelInputDTO param)
        //{
        //    if (param == null)
        //    {
        //        return ResponseResult("PARAM_NOT_BLANK");
        //    }

        //    if (string.IsNullOrWhiteSpace(param.Code))
        //    {
        //        return ResponseResult("CODE_NOT_BLANK");
        //    }
        //    if (string.IsNullOrWhiteSpace(param.Name))
        //    {
        //        return ResponseResult("NAME_NOT_BLANK");
        //    }
        //    var r = await _unitOfWork.SalaryLevelRepository.CreateAsync(param);
        //    return ResponseResult(r);
        //}
        //[HttpPost]
        //public async Task<ActionResult> Update([FromBody] SalaryLevelInputDTO param)
        //{
        //    if (param == null)
        //    {
        //        return ResponseResult("PARAM_NOT_BLANK");
        //    }
        //    if (param.Id == null)
        //    {
        //        return ResponseResult("ID_NOT_BLANK");
        //    }
        //    if (string.IsNullOrWhiteSpace(param.Code))
        //    {
        //        return ResponseResult("CODE_NOT_BLANK");
        //    }
        //    if (string.IsNullOrWhiteSpace(param.Name))
        //    {
        //        return ResponseResult("NAME_NOT_BLANK");
        //    }

        //    var r = await _unitOfWork.SalaryLevelRepository.UpdateAsync(param);
        //    return ResponseResult(r);
        //}
        //[HttpPost]
        //public async Task<ActionResult> ChangeStatus([FromBody] List<long> ids)
        //{
        //    var r = await _unitOfWork.SalaryLevelRepository.ChangeStatusAsync(ids);
        //    return ResponseResult(rstatus);
        //}

        //[HttpGet]
        //public async Task<ActionResult> GetList(int rankId)
        //{
        //    var r = await _unitOfWork.SalaryLevelRepository.GetList(rankId);
        //    return ResponseResult(r);
        //}
    }
}
