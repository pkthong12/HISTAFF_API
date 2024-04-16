using API.All.DbContexts;
using API.DTO;
using API.Main;
using CORE.DTO;
using CORE.Enum;
using CORE.GenericUOW;
using CORE.StaticConstant;
using DocumentFormat.OpenXml.Drawing.Charts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Globalization;

namespace API.Controllers.AtSetupTimeEmp
{
    [ApiExplorerSettings(GroupName = "017-ATTENDANCE-AT_SETUP_TIME_EMP")]
    [ApiController]
    [HiStaffAuthorize]
    [Route("api/[controller]/[action]")]
    public class AtSetupTimeEmpController : ControllerBase
    {
        private readonly GenericUnitOfWork _uow;
        private readonly IAtSetupTimeEmpRepository _AtSetupTimeEmpRepository;
        private readonly AppSettings _appSettings;
        private readonly FullDbContext _fullDbContext;

        public AtSetupTimeEmpController(
            FullDbContext dbContext,
            IOptions<AppSettings> options)
        {
            _uow = new GenericUnitOfWork(dbContext);
            _fullDbContext = dbContext;
            _AtSetupTimeEmpRepository = new AtSetupTimeEmpRepository(dbContext, _uow);
            _appSettings = options.Value;
        }

        [HttpGet]
        public async Task<IActionResult> ReadAll()
        {
            var response = await _AtSetupTimeEmpRepository.ReadAll();
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> ReadAllByNumberKey(ReadAllByKeyLongRequest request)
        {
            var response = await _AtSetupTimeEmpRepository.ReadAllByKey(request.Key, request.Value);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> ReadAllByStringKey(ReadAllByKeyStringRequest request)
        {
            var response = await _AtSetupTimeEmpRepository.ReadAllByKey(request.Key, request.Value);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> QueryList(GenericQueryListDTO<AtSetupTimeEmpDTO> request)
        {
            var response = await _AtSetupTimeEmpRepository.SinglePhaseQueryList(request);
            return Ok(new FormatedResponse() { InnerBody = response });
        }

        [HttpGet]
        public async Task<IActionResult> GetById(long id)
        {
            var response = await _AtSetupTimeEmpRepository.GetById(id);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetByStringId(string id)
        {
            var response = await _AtSetupTimeEmpRepository.GetById(id);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Create(AtSetupTimeEmpDTO model)
        {
            try
            {
                var sid = Request.Sid(_appSettings);
                if (sid == null) return Unauthorized();

                var data = _fullDbContext.AtSetupTimeEmps.Where(x => x.EMPLOYEE_ID == model.EmployeeId && x.IS_ACTIVE == true).ToList();
                if (data != null)
                {
                    foreach (var item in data)
                    {
                        DateTime sDate = item.START_DATE_HL!.Value;//ngay hieu luc cua ban ghi nv da co
                        //DateTime eDate = item.END_DATE_HL!.Value;//ngay het hieu luc cua ban ghi nv da co
                        DateTime sDateNew = model.StartDateHl!.Value;//ngay hieu luc ban ghi nv moi

                        if (item.END_DATE_HL != null && model.EndDateHl != null) //(model.EndDateHl != null)
                        {
                            DateTime eDate = item.END_DATE_HL!.Value;//ngay het hieu luc cua ban ghi nv da co
                            DateTime eDateNew = model.EndDateHl!.Value;//ngay het hieu luc ban ghi nv moi
                            if ((sDateNew >= sDate && sDateNew <= eDate) || (eDateNew >= sDate && eDateNew <= eDate) || (sDateNew <= sDate && eDateNew >= eDate)
                             || (sDate >= sDateNew && sDate <= sDateNew) || (sDate >= sDateNew && sDate <= eDateNew) || (eDate >= sDateNew && eDate <= eDateNew))
                            {
                                return Ok(new FormatedResponse() { ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400, MessageCode = "THE_EMPLOYEE_HAS_RECORDS_FOR_THIS_TIME_PERIOD" });
                            }
                        }
                        else if(item.END_DATE_HL != null)
                        {
                            DateTime eDate = item.END_DATE_HL!.Value;//ngay het hieu luc cua ban ghi nv da co
                            if ((sDate >= sDateNew && sDate <= sDateNew) || sDateNew >= sDate && sDateNew <= eDate)
                            {
                                return Ok(new FormatedResponse() { ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400, MessageCode = "THE_EMPLOYEE_HAS_RECORDS_FOR_THIS_TIME_PERIOD" });
                            }

                            //update ngay het hieu cho ban ghi cu (null)
                            var updateDate = await _fullDbContext.AtSetupTimeEmps.Where(p => p.EMPLOYEE_ID == model.EmployeeId).OrderByDescending(p => p.ID).FirstAsync();
                            //var updateDate = await _fullDbContext.AtSetupTimeEmps.AsNoTracking().Where(x => x.ID == item.ID).FirstOrDefaultAsync();
                            updateDate!.END_DATE_HL = sDateNew.AddDays(-1);
                            _fullDbContext.Entry(updateDate).State = EntityState.Modified;
                            await _fullDbContext.SaveChangesAsync();
                            _uow.Commit();
                        }

                    }
                }
                //var checkExists = _fullDbContext.AtSetupTimeEmps.Where(x => x.EMPLOYEE_ID == model.EmployeeId && x.IS_ACTIVE == true).Count();
                //if (checkExists > 0)
                //{
                //    return Ok(new FormatedResponse() { ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode500, MessageCode = "EXIST_RECORD_APPLICABLE_STATE" });
                //}
                var response = await _AtSetupTimeEmpRepository.Create(_uow, model, sid);
                return Ok(response);
            }
            catch (Exception ex)
            {
                _uow.Rollback();
                return Ok(new FormatedResponse() { MessageCode = ex.Message, ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400 });

            }
        }
        [HttpPost]
        public async Task<IActionResult> CreateRange(List<AtSetupTimeEmpDTO> models)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _AtSetupTimeEmpRepository.CreateRange(_uow, models, sid);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Update(AtSetupTimeEmpDTO model)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _AtSetupTimeEmpRepository.Update(_uow, model, sid);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateRange(List<AtSetupTimeEmpDTO> models)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _AtSetupTimeEmpRepository.UpdateRange(_uow, models, sid);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(AtSetupTimeEmpDTO model)
        {
            if (model.Id != null)
            {
                var response = await _AtSetupTimeEmpRepository.Delete(_uow, (long)model.Id);
                return Ok(response);
            }
            else
            {
                return Ok(new FormatedResponse() { ErrorType = EnumErrorType.CATCHABLE, MessageCode = CommonMessageCode.DELETE_REQUEST_NULL_ID });
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteIds(IdsRequest model)
        {
            var response = await _AtSetupTimeEmpRepository.DeleteIds(_uow, model.Ids);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteStringIds(StringIdsRequest model)
        {
            var response = await _AtSetupTimeEmpRepository.DeleteIds(_uow, model.Ids);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> ToggleActiveIds(GenericToggleIsActiveDTO model)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _AtSetupTimeEmpRepository.ToggleActiveIds(_uow, model.Ids, model.ValueToBind, sid);
            return Ok(response);
        }
    }
}

