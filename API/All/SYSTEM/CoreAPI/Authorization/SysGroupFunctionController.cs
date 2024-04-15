using Microsoft.AspNetCore.Mvc;
using CoreDAL.Repositories;
using CoreDAL.ViewModels;

namespace CoreAPI.Authorization
{
    [ApiExplorerSettings(GroupName = "003-SYSTEM-GROUP-FUNCTION")]
    [ApiController]
    [HiStaffAuthorize]
    [Route("api/[controller]/[action]")]
    public class SysGroupFunctionController : BaseController
    {
        //private readonly IConfiguration _config;
        public SysGroupFunctionController(IUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }

        [HttpGet]
        public async Task<ActionResult> GetAll(SysGroupFunctionDTO param)
        {
            var r = await _unitOfWork.SysGroupFunctions.GetAll(param);
            return Ok(r);
        }

        [HttpGet]
        public async Task<ActionResult> Get(int Id)
        {
            var r = await _unitOfWork.SysGroupFunctions.GetById(Id);
            return ResponseResult(r);
        }
        [HttpPost]
        public async Task<ActionResult> Add([FromBody]SysGroupFunctionInputDTO param)
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
            if (param.ApplicationId == 0)
            {
                return ResponseResult("APPLICATION_NOT_BLANK");
            }

            var r = await _unitOfWork.SysGroupFunctions.CreateAsync(param);
            return ResponseResult(r);
        }

        [HttpPost]
        public async Task<ActionResult> Update([FromBody]SysGroupFunctionInputDTO param)
        {
            if (param == null)
            {
                return BadRequest();
            }
            if (param.Id == 0)
            {
                return ResponseResult("ID_NOT_BLANK");
            }
            if (param.ApplicationId == 0)
            {
                return ResponseResult("APPLICATION_NOT_BLANK");
            }
            if (param.Code == null)
            {
                return ResponseResult("CODE_NOT_BLANK");
            }
            if (param.Name == null)
            {
                return ResponseResult("NAME_NOT_BLANK");
            }
            if (param.ApplicationId == 0)
            {
                return ResponseResult("APPLICATION_NOT_BLANK");
            }
            var r = await _unitOfWork.SysGroupFunctions.UpdateAsync(param);
            return ResponseResult(r);
        }
        [HttpPost]
        public async Task<ActionResult> ChangeStatus([FromBody]List<long> ids)
        {
            var r = await _unitOfWork.SysGroupFunctions.ChangeStatusAsync(ids);
            return ResponseResult(r);
        }
    }
}
