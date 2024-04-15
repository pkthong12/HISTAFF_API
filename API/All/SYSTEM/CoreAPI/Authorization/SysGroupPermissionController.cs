using Microsoft.AspNetCore.Mvc;
using CoreDAL.ViewModels;
using CoreDAL.Repositories;

namespace CoreAPI.Authorization
{
    [ApiExplorerSettings(GroupName = "004-SYSTEM-GROUP-FUNCTION")]
    [ApiController]
    [HiStaffAuthorize]
    [Route("api/[controller]/[action]")]
    public class SysGroupPermissionController : BaseController
    {
        public SysGroupPermissionController(IUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }

        [HttpGet]
        public async Task<ActionResult> GetAll(SysGroupPermissionDTO param)
        {
            var r = await _unitOfWork.SysGroupPermissions.GetAll(param);
            return Ok(r);
        }

        [HttpGet]
        public async Task<ActionResult> GetBy(SysGroupPermissionDTO param)
        {
            var r = await _unitOfWork.SysGroupPermissions.GetBy(param);
            return ResponseResult(r);
        }
        [HttpPost]
        public async Task<ActionResult> Update([FromBody]List<GroupPermissionInputDTO> param)
        {
            if (param == null)
            {
                return BadRequest();
            }
            var r = await _unitOfWork.SysGroupPermissions.UpdateAsync(param);
            return ResponseResult(r);
        }
        [HttpGet]
        public async Task<ActionResult> GridFuntion(GridFunctionInput param)
        {
            if (param == null)
            {
                return BadRequest();
            }
            var r = await _unitOfWork.SysGroupPermissions.GridPermission(param);
            return Ok(r);
        }
    }
}
