using Microsoft.AspNetCore.Mvc;
using ProfileDAL.Repositories;
using ProfileDAL.ViewModels;
using CORE.Enum;
using CORE.GenericUOW;
using CORE.StaticConstant;
using CORE.DTO;
using API.All.DbContexts;
using API.Entities;
using ProfileDAL.Repositories;
using Azure.Core;
using API.Main;
using Common.Extensions;
using System;
using Org.BouncyCastle.Asn1.Ocsp;
using Org.BouncyCastle.Ocsp;
using API;
using Microsoft.Extensions.Options;

namespace ProfileAPI.List
{
    [HiStaffAuthorize]
    [ApiExplorerSettings(GroupName = "002-PROFILE-SALARY-SCALE")]
    [ApiController]

    [Route("api/[controller]/[action]")]
    public class HuSalaryScaleController : BaseController1
    {
        private IHttpContextAccessor _accessor;
        private readonly ProfileDbContext _profileDbContext;
        private readonly GenericUnitOfWork _uow;
        private IGenericRepository<HU_SALARY_SCALE, SalaryScaleEditDTO> _genericRepository;


        // khai báo thêm
        private readonly AppSettings _appSettings;

        public HuSalaryScaleController(
            IProfileUnitOfWork unitOfWork,
            IHttpContextAccessor accessor,
            ProfileDbContext coreDbContext,
            ProfileDbContext profileDbContext,
            IOptions<AppSettings> options
        ) : base(unitOfWork)
        {
            _accessor = accessor;
            _profileDbContext = profileDbContext;
            _uow = new GenericUnitOfWork(coreDbContext);
            _genericRepository = _uow.GenericRepository<HU_SALARY_SCALE, SalaryScaleEditDTO>();
            _appSettings = options.Value;
        }
        [HttpPost]
        public async Task<IActionResult> QueryList(GenericQueryListDTO<SalaryScaleViewDTO> request)
        {
            try
            {
                /* HERE WE CAN EITHER CHOOSE ONE OF THE 2 APPROACHS: TwoPhaseQueryList or SinglePhaseQueryList */
                /* WE WILL KNOW WHAT APPROACH IS FASTER IN THE FUTURE */

                //var response = await _unitOfWork.SalaryScaleRepository.TwoPhaseQueryList(request);

                var response = await _unitOfWork.SalaryScaleRepository.SinglePhaseQueryList(request);
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
            var queryCode = (from x in _profileDbContext.SalaryScales where x.CODE.Length == 6 select x.CODE).ToList();
            var existingCode = (from p in queryCode where Decimal.TryParse(p.Substring(3), out num) orderby p descending select p).ToList();
            string newcode = StringCodeGenerator.CreateNewCode("TBL", 3, existingCode);
            return Ok(new FormatedResponse() { InnerBody = new { Code = newcode } });
        }
        [HttpGet]
        public async Task<ActionResult> GetAll(SalaryScaleDTO param)
        {
            var r = await _unitOfWork.SalaryScaleRepository.GetAll(param);
            return Ok(r);
        }
        [HttpGet]
        public async Task<ActionResult> Get(int Id)
        {
            var r = await _unitOfWork.SalaryScaleRepository.GetById(Id);
            return ResponseResult(r);
        }
        [HttpGet]
        public async Task<ActionResult> GetById(long id)
        {
            var r = await _unitOfWork.SalaryScaleRepository.GetById(id);
            return Ok(new FormatedResponse()
            {
                InnerBody = r.Data
            });
        }
        [HttpPost]
        public async Task<ActionResult> Add([FromBody] SalaryScaleInputDTO param)
        {
            if (param == null)
            {
                return ResponseResult("PARAM_NOT_BLANK");
            }

            if (string.IsNullOrWhiteSpace(param.Code))
            {
                return ResponseResult("CODE_NOT_BLANK");
            }
            if (string.IsNullOrWhiteSpace(param.Name))
            {
                return ResponseResult("NAME_NOT_BLANK");
            }

            var r = await _unitOfWork.SalaryScaleRepository.CreateAsync(param);
            return ResponseResult(r);
        }

        [HttpPost]
        public async Task<ActionResult> ChangeStatus([FromBody] List<long> ids)
        {
            var r = await _unitOfWork.SalaryScaleRepository.ChangeStatusAsync(ids);
            return ResponseResult(r);
        }
        [HttpGet]
        public async Task<ActionResult> GetList()
        {
            try
            {
                var r = await _unitOfWork.SalaryScaleRepository.GetList();
                return Ok(new FormatedResponse() { InnerBody = r.Data });
            }
            catch (Exception ex)
            {
                return Ok(new FormatedResponse() { ErrorType = EnumErrorType.UNCATCHABLE, StatusCode = EnumStatusCode.StatusCode500, MessageCode = ex.Message });
            }
        }
        [HttpPost]
        public async Task<IActionResult> Create(SalaryScaleInputDTO request)
        {
            try
            {
                var r1 = _profileDbContext.SalaryScales.Where(x => x.CODE == request.Code).Count();
                if (r1 > 0)
                {
                    return Ok(new FormatedResponse() { ErrorType = EnumErrorType.UNCATCHABLE, MessageCode = CommonMessageCode.CREATE_OBJECT_HAS_SAME_CODE });
                }
                if (request.EffectiveDate > request.ExpirationDate)
                {
                    return Ok(new FormatedResponse() { MessageCode = CommonMessageCode.EFFECTIVEDATE_NOT_BIGGER_THAN_EXPIRATIONDATE, ErrorType = EnumErrorType.UNCATCHABLE, StatusCode = EnumStatusCode.StatusCode500 });
                }
                if (_profileDbContext.SalaryScales.Where(x => x.NAME == request.Name).Any())
                {
                    return Ok(new FormatedResponse() { MessageCode = CommonMessageCode.NOT_THE_SAME_NAME, ErrorType = EnumErrorType.UNCATCHABLE, StatusCode = EnumStatusCode.StatusCode500 });
                }


                HU_SALARY_SCALE objData = new()
                {
                    CODE = request.Code,
                    NAME = request.Name,
                    NOTE = request.Note,
                    IS_ACTIVE = true,
                    CREATED_DATE = DateTime.Now,
                    IS_TABLE_SCORE = request.IsTableScore,
                    EXPIRATION_DATE = request.ExpirationDate,
                    EFFECTIVE_DATE = request.EffectiveDate,

                };
                _profileDbContext.SalaryScales.Add(objData);
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
        public async Task<IActionResult> DeleteIds(IdsRequest model)
        {
            try
            {
                if (model.Ids != null)
                {

                    _uow.CreateTransaction();
                    foreach (var id in model.Ids)
                    {
                        var item = await _profileDbContext.SalaryScales.Where(x => x.ID == id).AsNoTracking().SingleOrDefaultAsync();
                        if (item != null && item.IS_ACTIVE == true)
                        {
                            _uow.Rollback();
                            return Ok(new FormatedResponse() { ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400, MessageCode = CommonMessageCode.DO_NOT_DELETE_DATA_IS_ACTIVE });
                        }
                    }
                    foreach (var item in model.Ids)
                    {
                        var r1 = await _profileDbContext.SalaryRanks.Where(x => x.SALARY_SCALE_ID == item).ToListAsync();
                        foreach (var item1 in r1)
                        {
                            if (item1.IS_ACTIVE == true)
                            {
                                _uow.Rollback();
                                return Ok(new FormatedResponse() { ErrorType = EnumErrorType.CATCHABLE, MessageCode = CommonMessageCode.DELETE_REQUEST_NULL_ID });
                            }
                            else
                            {
                                item1.SALARY_SCALE_ID = null;
                            }

                        }
                        _uow.Save();
                    }

                    var response = await _genericRepository.DeleteIds(_uow, model.Ids);
                    if (response.InnerBody != null)
                    {
                        _uow.Commit();
                        return Ok(response);
                    }
                    else
                    {
                        _uow.Rollback();
                        return Ok(new FormatedResponse() { ErrorType = EnumErrorType.CATCHABLE, MessageCode = CommonMessageCode.DELETE_REQUEST_NULL_ID });
                    }
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
        public async Task<ActionResult> Update([FromBody] SalaryScaleInputDTO param)
        {
            if (param == null)
            {
                return ResponseResult("PARAM_NOT_BLANK");
            }
            if (param.Id == null)
            {
                return ResponseResult("ID_NOT_BLANK");
            }
            if (string.IsNullOrWhiteSpace(param.Code))
            {
                return ResponseResult("CODE_NOT_BLANK");
            }
            if (param.EffectiveDate == null)
            {
                return ResponseResult("EFFECTIVE_NOT_BLANK");
            }
            if (param.EffectiveDate > param.ExpirationDate)
            {
                return Ok(new FormatedResponse() { MessageCode = CommonMessageCode.EFFECTIVEDATE_NOT_BIGGER_THAN_EXPIRATIONDATE, ErrorType = EnumErrorType.UNCATCHABLE, StatusCode = EnumStatusCode.StatusCode500 });
            }

            var r = await _unitOfWork.SalaryScaleRepository.UpdateAsync(param);
            return Ok(new FormatedResponse()
            {
                InnerBody = r.Data
            });
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
            // trong bảng HU_SALARY_SCALE
            // var entity = _profileDbContext.Set<HU_SALARY_SCALE>().AsNoTracking().AsQueryable();
            var allRecord = (from item in _profileDbContext.SalaryScales
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
