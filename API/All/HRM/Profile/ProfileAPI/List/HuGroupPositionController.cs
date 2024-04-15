using API;
using API.All.DbContexts;
using API.DTO;
using API.Entities;
using API.Main;
using AttendanceDAL.ViewModels;
using Azure;
using CORE.DTO;
using CORE.Enum;
using CORE.GenericUOW;
using CORE.StaticConstant;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;
using ProfileDAL.Repositories;
using ProfileDAL.ViewModels;
using System.Text.RegularExpressions;

namespace ProfileAPI.List
{
    [HiStaffAuthorize]
    [ApiExplorerSettings(GroupName = "002-PROFILE-GROUP-POSITION")]
    [ApiController]

    [Route("api/[controller]/[action]")]
    public class HuGroupPositionController : BaseController1
    {
        private IHttpContextAccessor _accessor;
        private readonly GenericUnitOfWork _uow;
        private IGenericRepository<HU_POSITION_GROUP, GroupPositionEditDTO> _genericRepository;
        private AppSettings _appSettings;
        public HuGroupPositionController(IProfileUnitOfWork unitOfWork, IHttpContextAccessor accessor, ProfileDbContext coreDbContext, IOptions<AppSettings> options) : base(unitOfWork)
        {
            _accessor = accessor;
            _uow = new GenericUnitOfWork(coreDbContext);
            _genericRepository = _uow.GenericRepository<HU_POSITION_GROUP, GroupPositionEditDTO>();
            _appSettings = options.Value;
        }
        [HttpPost]
        public async Task<IActionResult> QueryList(GenericQueryListDTO<GroupPositionViewDTO> request)
        {
            try
            {
                /* HERE WE CAN EITHER CHOOSE ONE OF THE 2 APPROACHS: TwoPhaseQueryList or SinglePhaseQueryList */
                /* WE WILL KNOW WHAT APPROACH IS FASTER IN THE FUTURE */

                var response = await _unitOfWork.GroupPositionRepository.TwoPhaseQueryList(request);

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
        public async Task<IActionResult> CreateCodeAuto()
        {
            try
            {
                decimal num;
                string str;
                var rs = "001";

                var entity = _uow.Context.Set<HU_POSITION_GROUP>().AsNoTracking().AsQueryable();
                var joined = (from p in entity
                              where p.CODE.Length == 3
                              
                             select p.CODE).ToList();
                var maxCode = (from p in joined where Decimal.TryParse(p, out num) orderby p descending select p).FirstOrDefault();
                if (maxCode != null)
                {
                    rs = (int.Parse(maxCode) + 1).ToString("000");
                }
                return Ok(new FormatedResponse() { InnerBody = new { Code = rs } });
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
                var response = await _genericRepository.GetById(id);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return Ok(new FormatedResponse() { MessageCode = ex.Message, ErrorType = EnumErrorType.UNCATCHABLE, StatusCode = EnumStatusCode.StatusCode500 });
            }
        }

        [HttpGet]
        public async Task<ActionResult> GetAll(GroupPositionDTO param)
        {
            var r = await _unitOfWork.GroupPositionRepository.GetAll(param);
            return Ok(new FormatedResponse()
            {
                InnerBody = r
            });
        }
        [HttpGet]
        public async Task<ActionResult> Get(int Id)
        {
            var r = await _unitOfWork.GroupPositionRepository.GetById(Id);
            return Ok(new FormatedResponse()
            {
                InnerBody = r
            });
        }
        [HttpPost]
        public async Task<ActionResult> Add([FromBody]GroupPositionInputDTO param)
        {
            if (param == null)
            {
                return BadRequest();
            }
            if (param.Code == null)
            {
                return ResponseResult("CODE_NOT_BLANK");
            }

            if (param.Name == null)
            {
                return ResponseResult("NAME_NOT_BLANK");
            }


            var r = await _unitOfWork.GroupPositionRepository.CreateAsync(param);
            return ResponseResult(r);
        }


        [HttpPost]
        public async Task<IActionResult> Update(GroupPositionEditDTO model)
        {
            try
            {
                var val = _uow.Context.Set<HU_POSITION_GROUP>().AsNoTracking().AsQueryable().Where(f => f.CODE == model.Code && f.ID != model.Id).Any();
                if (val)
                {
                    return Ok(new FormatedResponse() { ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode519, MessageCode = "UI_FORM_CONTROL_ERROR_GROUP_POSITION_CODE_DUPLICATED" });
                }
                var val2 = _uow.Context.Set<HU_POSITION_GROUP>().AsNoTracking().AsQueryable().Where(f => f.NAME == model.Name && f.ID != model.Id).Any();
                if (val2)
                {
                    return Ok(new FormatedResponse() { ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode519, MessageCode = "UI_FORM_CONTROL_ERROR_GROUP_POSITION_NAME_DUPLICATED" });
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
        public async Task<ActionResult> ChangeStatus([FromBody]List<int> ids)
        {
            var r = await _unitOfWork.GroupPositionRepository.ChangeStatusAsync(ids);
            return ResponseResult(r);
        }

        [HttpGet]
        public async Task<ActionResult> GetList()
        {
            try
            {
                var r = await _unitOfWork.GroupPositionRepository.GetList();
                return Ok(new FormatedResponse()
                {
                    InnerBody = r.Data
                });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //----------------------------------------------------------------------------------------------
        [HttpPost]
        public async Task<IActionResult> Create(GroupPositionEditDTO model)
        {
            try
            {
                var val1 = _uow.Context.Set<HU_POSITION_GROUP>().AsNoTracking().AsQueryable().Where(f => f.CODE == model.Code).Any();
                if(val1)
                {
                    return Ok(new FormatedResponse() { ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode519, MessageCode = "UI_FORM_CONTROL_ERROR_GROUP_POSITION_CODE_DUPLICATED" });
                }
                var val2 = _uow.Context.Set<HU_POSITION_GROUP>().AsNoTracking().AsQueryable().Where(f => f.NAME == model.Name).Any();
                if (val2)
                {
                    return Ok(new FormatedResponse() { ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode519, MessageCode = "UI_FORM_CONTROL_ERROR_GROUP_POSITION_NAME_DUPLICATED" });
                }
                var response = await _genericRepository.Create(_uow, model, Request.Sid(_appSettings) ?? "");
                response.MessageCode = CommonMessageCode.CREATE_SUCCESS;
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
    }
}
