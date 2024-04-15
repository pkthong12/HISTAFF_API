using Microsoft.AspNetCore.Mvc;
using ProfileDAL.Repositories;
using ProfileDAL.ViewModels;
using PayrollDAL.Repositories;
using Common.Extensions;
using CORE.DTO;
using CORE.Enum;
using CORE.StaticConstant;
using API.All.SYSTEM.CoreAPI.OrtherList;
using System.Text;
using API.Main;
using API.All.DbContexts;
using CORE.GenericUOW;

namespace ProfileAPI.List
{
    [HiStaffAuthorize]
    [ApiExplorerSettings(GroupName = "002-PROFILE-ALLOWANCE")]
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class HuAllowanceController : BaseController1
    {
        private readonly ProfileDbContext _profileDbContext;
        private readonly GenericUnitOfWork _uow;
        private readonly FullDbContext _dbContext;
        private IGenericRepository<HU_ALLOWANCE, AllowanceEditDTO> _genericRepository;
        public HuAllowanceController(IProfileUnitOfWork unitOfWork, FullDbContext dbcontext, ProfileDbContext context ) : base(unitOfWork)
        {
            _profileDbContext = context;
            _dbContext = dbcontext;
            _uow = new GenericUnitOfWork(context);
            _genericRepository = _uow.GenericRepository<HU_ALLOWANCE, AllowanceEditDTO>();
        }
        [HttpPost]
        public async Task<IActionResult> QueryList(GenericQueryListDTO<AllowanceViewDTO> request)
        {
            try
            {

                var response = await _unitOfWork.AllowanceRepository.SinglePhaseQueryList(request);

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
        public async Task<ActionResult> GetAll(AllowanceViewDTO param)
        {
            var r = await _unitOfWork.AllowanceRepository.GetAll(param);
            return Ok(r);
        }
        [HttpGet]
        public async Task<IActionResult> GetById(long id)
        {
            var r = await _unitOfWork.AllowanceRepository.GetById(id);
            return Ok( new FormatedResponse() { InnerBody = r.Data});
        }
        [HttpGet]
        public async Task<IActionResult> GetCode()
        {
            decimal num;
            var queryCode = (from x in _profileDbContext.Allowances where x.CODE.Length == 5 select x.CODE).ToList();
            var existingCode = (from p in queryCode where Decimal.TryParse(p.Substring(2), out num) orderby p descending select p).ToList();
            string newcode = StringCodeGenerator.CreateNewCode("PC", 3, existingCode);
            return Ok(new FormatedResponse() { InnerBody = new { Code = newcode } });
        }
        [HttpPost]
        public async Task<ActionResult> Create([FromBody] AllowanceInputDTO param)
        {
            if (!ModelState.IsValid)
            {
                return ResponseValidation();
            }
            var r = new ReferParam();
            r.Name = param.Name;
            r.Code = param.Code;
            //var x = await _payrollBusiness.SalaryElementRepository.AllowanceToElement(r, 1);
            //if (x.StatusCode == "400")
            //{
            //    return ResponseResult(x);
            //}
            var y = await _unitOfWork.AllowanceRepository.CreateAsync(param);

            return Ok(y);
        }
        [HttpPost]
        public async Task<ActionResult> Update([FromBody] AllowanceInputDTO param)
        {

            if (!ModelState.IsValid)
            {
                return ResponseValidation();
            }
            var r = await _unitOfWork.AllowanceRepository.UpdateAsync(param);
            return Ok(r);
        }

        [HttpPost]
        public async Task<ActionResult> ChangeStatus([FromBody] List<long> ids)
        {
            var r = await _unitOfWork.AllowanceRepository.ChangeStatusAsync(ids);
            return ResponseResult(r);
        }
         [HttpPost]
        public async Task<ActionResult> ChangeStatusApprove(AllowanceInputDTO request)
        {
            foreach (var item in request.ids!)
            {
                var r = _profileDbContext.Allowances.Where(x => x.ID == item).FirstOrDefault();
                if (r != null)
                {
                    r.IS_ACTIVE = request.ValueToBind;
                    var result = _profileDbContext.Allowances.Update(r);
                }
            }
            await _profileDbContext.SaveChangesAsync();
            return Ok(new FormatedResponse() { MessageCode = request.ValueToBind == true ? CommonMessageCode.TOGGLE_IS_ACTIVE_SUCCESS : (request.ValueToBind == false ? CommonMessageCode.TOGGLE_IS_INACTIVE_SUCCESS : ""), StatusCode = EnumStatusCode.StatusCode200 });

        }
        [HttpGet]
        public async Task<ActionResult> GetList()
        {
            var r = await _unitOfWork.AllowanceRepository.GetList();
            return Ok(new FormatedResponse() { InnerBody = r.Data });
        }
        [HttpGet]
        public async Task<ActionResult> CheckAllowIsUsed(string code)
        {
            var r = await _unitOfWork.AllowanceRepository.CheckAllowIsUsed(code);
            return ResponseResult(r);
        }
        [HttpPost]
        public async Task<ActionResult> DeleteIds(IdsRequest model)
        {
            try
            {
                //check ban ghi phat sinh tai cac man #
                foreach (var item in model.Ids)
                {
                    var check1 = _dbContext.HuWorkingAllows.Where(x => x.ALLOWANCE_ID == item).AsNoTracking().Count();
                    var check2 = _dbContext.HuAllowanceEmps.Where(x => x.ALLOWANCE_ID == item).AsNoTracking().Count();
                    if (check1 > 0)
                    {
                        return Ok(new FormatedResponse() { ErrorType = EnumErrorType.CATCHABLE, MessageCode = "DO_NOT_DELETE_DATA_RECORD_WORKING", StatusCode = EnumStatusCode.StatusCode400 });
                    }
                    else if (check2 > 0)
                    {
                        return Ok(new FormatedResponse() { ErrorType = EnumErrorType.CATCHABLE, MessageCode = "DO_NOT_DELETE_DATA_RECORD_ALLOWANCE_EMP", StatusCode = EnumStatusCode.StatusCode400 });
                    }
                }
                if (model.Ids != null)
                {

                    _uow.CreateTransaction();
                    foreach (var id in model.Ids)
                    {
                        var item = await _profileDbContext.Allowances.Where(x => x.ID == id).AsNoTracking().SingleOrDefaultAsync();
                        if (item != null && item.IS_ACTIVE == true)
                        {
                            _uow.Rollback();
                            return Ok(new FormatedResponse() { ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400, MessageCode = CommonMessageCode.DO_NOT_DELETE_DATA_IS_ACTIVE });
                        }
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
    }
}
