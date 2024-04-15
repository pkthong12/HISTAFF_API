using Microsoft.AspNetCore.Mvc;
using Common.Extensions;
using CoreDAL.ViewModels;
using CoreDAL.Repositories;

namespace CoreAPI.Authorization
{
    [ApiExplorerSettings(GroupName = "006-SYSTEM-PERMISSION")]
    [ApiController]
    [HiStaffAuthorize]
    [Route("api/[controller]/[action]")]
    public class SysPermissionController : BaseController
    {
        public SysPermissionController(IUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }

        [HttpGet]
        public async Task<ActionResult> GetAll(SysPermissionDTO values)
        {
            var r = await _unitOfWork.SysPermissions.GetAll(values);
            return Ok(r);
        }

        [HttpGet]
        public async Task<ActionResult> Get(int Id)
        {
            var r = await _unitOfWork.SysPermissions.Get(Id);
            if (r != null)
            {
                return Ok(r);
            }
            else
            {
                return BadRequest(new ResultWithError(400));
            }

        }
        [HttpPost]
        public async Task<ActionResult> Add([FromBody]SysPermissionInputDTO param)
        {
            //VALID REQUIRE
            if (param.Code == null)
            {
                return ResponseResult("CODE_NOT_BLANK");
            }
            if (param.Name == null)
            {
                return ResponseResult("NAME_NOT_BLANK");
            }
            var r = await _unitOfWork.SysPermissions.CreateAsync(param);
            return ResponseResult(r);
        }

        [HttpPost]
        public async Task<ActionResult> Update([FromBody]SysPermissionInputDTO param)
        {
            //VALID REQUIRE
            if (param.Code == null)
            {
                return ResponseResult("CODE_NOT_BLANK");
            }
            if (param.Name == null)
            {
                return ResponseResult("NAME_NOT_BLANK");
            }
            var r = await _unitOfWork.SysPermissions.UpdateAsync(param);
            return ResponseResult(r);
        }
        [HttpPost]
        public async Task<ActionResult> ChangeStatus([FromBody]List<long> ids)
        {
            if (ids.Count == 0)
            {
                return ResponseResult("ID_NOT_BLANK");
            }
            var r = await _unitOfWork.SysPermissions.ChangeStatusAsync(ids);
            return ResponseResult(r);
        }

        [HttpGet]
        public async Task<ActionResult> GetListPermission()
        {
            var r = await _unitOfWork.SysPermissions.GetListPermission();
            return ResponseResult(r);
        }
    }
}
