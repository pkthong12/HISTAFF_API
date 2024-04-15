using Microsoft.AspNetCore.Mvc;
using CoreDAL.ViewModels;
using CoreDAL.Repositories;

namespace CoreAPI.Authorization
{
    [ApiExplorerSettings(GroupName = "007-SYSTEM-USER-PERMISSION")]
    [ApiController]
    [HiStaffAuthorize]
    [Route("api/[controller]/[action]")]
    public class SysUserPermissionController : BaseController
    {
        public SysUserPermissionController(IUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }

        [HttpGet]
        public async Task<ActionResult> GetAll(SysUserPermissionDTO param)
        {
            var r = await _unitOfWork.SysUserPermissions.GetAll(param);
            return Ok(r);
        }

        [HttpGet]
        public async Task<ActionResult> GetBy(SysUserPermissionDTO param)
        {
            var r = await _unitOfWork.SysUserPermissions.GetBy(param);
            return ResponseResult(r);
        }
        [HttpPost]
        public async Task<ActionResult> Update([FromBody]List<UserPermissionInputDTO> param)
        {
            if (param == null)
            {
                return BadRequest();
            }

            var r = await _unitOfWork.SysUserPermissions.UpdateAsync(param);
            return ResponseResult(r);
        }
        [HttpGet]
        public async Task<ActionResult> GridFuntion(GridFunctionInput param)
        {
            if (param == null)
            {
                return BadRequest();
            }
            var r = await _unitOfWork.SysUserPermissions.GridPermission(param);
            return Ok(r);
        }

    }
}
