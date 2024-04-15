using API.All.DbContexts;
using API.Controllers.SysOtherList;
using API.DTO;
using API.Entities;
using API.Main;
using AttendanceDAL.ViewModels;
using Common.Extensions;
using CORE.DTO;
using CORE.Enum;
using CORE.GenericUOW;
using CORE.StaticConstant;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using ProfileDAL.ViewModels;
using System;
using System.Collections;
using System.Text.RegularExpressions;

namespace API.All.SYSTEM.CoreAPI.OrtherList
{
    [ApiExplorerSettings(GroupName = "147-SYSTEM-SYS_OTHER_LIST")]
    [ApiController]
    [HiStaffAuthorize]
    [Route("api/[controller]/[action]")]
    public class SysOrtherListController : ControllerBase
    {

        private readonly GenericUnitOfWork _uow;
        private readonly ISysOtherListRepository _SysOtherListRepository;
        private readonly AppSettings _appSettings;
        private IGenericRepository<SYS_OTHER_LIST, SysOtherListDTO> _genericRepository;

        public SysOrtherListController(
            FullDbContext dbContext,
            IOptions<AppSettings> options)
        {
            _uow = new GenericUnitOfWork(dbContext);
            _SysOtherListRepository = new SysOtherListRepository(dbContext, _uow);
            _appSettings = options.Value;
            _genericRepository = _uow.GenericRepository<SYS_OTHER_LIST, SysOtherListDTO>();
        }

        [HttpGet]
        public async Task<IActionResult> ReadAll()
        {
            var response = await _SysOtherListRepository.ReadAll();
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> ReadAllByNumberKey(ReadAllByKeyLongRequest request)
        {
            var response = await _SysOtherListRepository.ReadAllByKey(request.Key, request.Value);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> ReadAllByStringKey(ReadAllByKeyStringRequest request)
        {
            var response = await _SysOtherListRepository.ReadAllByKey(request.Key, request.Value);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> QueryList(GenericQueryListDTO<SysOtherListDTO> request)
        {

            var response = await _SysOtherListRepository.SinglePhaseQueryList(request);
            return Ok(new FormatedResponse() { InnerBody = response });
        }

        [HttpPost]
        public async Task<IActionResult> Create(SysOtherListDTO model)
        {
            try
            {
                var sid = Request.Sid(_appSettings);
                if (sid == null) return Unauthorized();
                model.IsActive = true;
                var response = await _SysOtherListRepository.Create(_uow, model, sid);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return Ok(new FormatedResponse() { MessageCode = ex.Message, ErrorType = EnumErrorType.UNCATCHABLE, StatusCode = EnumStatusCode.StatusCode500 });
            }
        }
        [HttpGet]
        public async Task<IActionResult> GetScales()
        {
            var response = await (from t in _uow.Context.Set<SYS_OTHER_LIST_TYPE>().AsNoTracking().DefaultIfEmpty()
                                  select new
                                  {
                                      Id = t.ID,
                                      Name = t.NAME,
                                  }).ToListAsync();
            return Ok(new FormatedResponse() { InnerBody = response });
        }
        [HttpGet]
        public async Task<IActionResult> GetById(long id)
        {
            try
            {
                var objRes = await _SysOtherListRepository.GetById(id);
                return Ok(new FormatedResponse() { InnerBody = objRes.InnerBody });
            }
            catch (Exception ex)
            {
                return Ok(new FormatedResponse() { MessageCode = ex.Message, ErrorType = EnumErrorType.UNCATCHABLE, StatusCode = EnumStatusCode.StatusCode500 });
            }
        }
        [HttpGet]
        public async Task<IActionResult> GetAllGroupOtherListType()

        {
            try
            {
                var query = await _SysOtherListRepository.GetAllGroupOtherListType();
                return Ok(query);
            }
            catch (Exception ex)
            {

                return Ok(new FormatedResponse() { ErrorType = EnumErrorType.UNCATCHABLE, StatusCode = EnumStatusCode.StatusCode500, MessageCode = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Update(SysOtherListDTO model)
        {
            try
            {
                var sid = Request.Sid(_appSettings);
                if (sid == null) return Unauthorized();
                var response = await _SysOtherListRepository.Update(_uow, model, sid);
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
                var response = await _SysOtherListRepository.DeleteIds(_uow, model.Ids);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return Ok(new FormatedResponse() { ErrorType = EnumErrorType.UNCATCHABLE, StatusCode = EnumStatusCode.StatusCode500, MessageCode = ex.Message });
            }
        }
        /// <summary>
        /// Backend Get Data is Active by Type
        /// </summary>
        /// <param name="param"></param>

        [HttpGet]
        public async Task<IActionResult> GetOtherListByType(string typeCode)
        {
            var sysOtherList = _uow.Context.Set<SYS_OTHER_LIST>();
            var sysOtherListType = _uow.Context.Set<SYS_OTHER_LIST_TYPE>();
            var r = await (from p in sysOtherList.AsNoTracking()
                           from o in sysOtherListType.AsNoTracking().Where(x => x.ID == p.TYPE_ID)
                           where p.IS_ACTIVE == true && (o.CODE == typeCode || p.CODE.Contains(typeCode))
                           select new
                           {
                               Id = p.ID,
                               Name = p.NAME,
                               Code = p.CODE,
                               IndexId= "Index"+p.ID
                           }).ToListAsync();
            return Ok(new FormatedResponse() { InnerBody = r });
        }
        [HttpGet]
        public async Task<IActionResult> GetAllCommendObjByKey(long id)
        {
            var response = await _SysOtherListRepository.GetAllCommendObjByKey(id);
            return Ok(response);
        }
        [HttpGet]
        public async Task<IActionResult> GetAllStatusByKey(long id)
        {
            var response = await _SysOtherListRepository.GetAllStatusByKey(id);
            return Ok(response);
        }
        [HttpGet]
        public async Task<IActionResult> GetAllSourceByKey(long id)
        {
            var response = await _SysOtherListRepository.GetAllStatusByKey(id);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllGender(long id)
        {
            var response = await _SysOtherListRepository.GetAllGender(id);
            return Ok(response);
        }
        [HttpGet]
        public async Task<IActionResult> GetAllWelfareByKey(long id)
        {
            var response = await _SysOtherListRepository.GetAllWelfareByKey(id);
            return Ok(response);
        }
        [HttpGet]
        public async Task<IActionResult> GetCode()
        {
            decimal num;
            var queryCode =  (from x in _uow.Context.Set<SYS_OTHER_LIST>() where( x.CODE.Length == 5 )select x.CODE).ToList();
            var existingCode =  (from p in queryCode where Decimal.TryParse(p, out num) orderby p descending select p).ToList();
            string newcode = StringCodeGenerator.CreateNewCodeNonePrefix(5, existingCode);
            return Ok(new FormatedResponse() { InnerBody = new { Code = newcode } });
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
        [HttpGet]
        public async Task<IActionResult> GetStatusCommend()
        {
            var response = await _SysOtherListRepository.GetStatusCommend();
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetStatusApproveHuFamilyEdit()
        {
            var response = await _SysOtherListRepository.GetStatusApproveHuFamilyEdit();
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetEvaluateType()
        {
            var response = await _SysOtherListRepository.GetEvaluateType();
            return Ok(response);
        }


        // get id status approve by code
        [HttpGet]
        public async Task<IActionResult> GetIdStatusByCode(string code)
        {
            var response = await _SysOtherListRepository.GetIdStatusByCode(code);
            return Ok(response);
        }
    }
}
