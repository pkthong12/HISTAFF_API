using Microsoft.AspNetCore.Mvc;
using CoreDAL.ViewModels;
using CoreDAL.Repositories;

namespace CoreAPI
{
    [ApiExplorerSettings(GroupName = "005-SYSTEM-GROUP-FUNCTION")]
    [ApiController]
    [HiStaffAuthorize]
    [Route("api/[controller]/[action]")]
    public class SysGroupUserController : BaseController
    {
        //private readonly IConfiguration _config;
        public SysGroupUserController(IUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }

        [HttpGet]
        public async Task<ActionResult> GetAll(SysGroupUserDTO values)
        {
            var r = await _unitOfWork.SysGroupUsers.GetAll(values);
            return Ok(r);
        }

        [HttpGet]
        public async Task<ActionResult> Get(int Id)
        {
            var r = await _unitOfWork.SysGroupUsers.GetById(Id);
            return ResponseResult(r);
        }

        [HttpPost]
        public async Task<ActionResult> Add([FromBody]SysGroupUserInputDTO param)
        {
            //VALID REQUIRE
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
            var r = await _unitOfWork.SysGroupUsers.CreateAsync(param);
            return ResponseResult(r);

        }
        [HttpPost]
        public async Task<ActionResult> Update([FromBody]SysGroupUserInputDTO param)
        {
            //VALID REQUIRE
            if (param == null)
            {
                return BadRequest();
            }
            if (param.Id == 0)
            {
                return ResponseResult("ID_NOT_BLANK");
            }
            if (param.Code == null)
            {
                return ResponseResult("CODE_NOT_BLANK");
            }
            if (param.Name == null)
            {
                return ResponseResult("NAME_NOT_BLANK");
            }
            var r = await _unitOfWork.SysGroupUsers.UpdateAsync(param);
            return ResponseResult(r);
        }
        [HttpPost]
        public async Task<ActionResult> ChangeStatus([FromBody]List<long> ids)
        {
            if (ids.Count == 0)
            {
                return ResponseResult("ID_NOT_BLANK");
            }
            var r = await _unitOfWork.SysGroupUsers.ChangeStatusAsync(ids);
            return ResponseResult(r);
        }
        [HttpGet]
        public async Task<ActionResult> GetList()
        {
            var r = await _unitOfWork.SysGroupUsers.GetList();
            return ResponseResult(r);
        }
    }
}