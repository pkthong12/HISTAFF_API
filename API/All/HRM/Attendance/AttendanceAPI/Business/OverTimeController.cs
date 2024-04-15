using Microsoft.AspNetCore.Mvc;
using AttendanceDAL.Repositories;
using AttendanceDAL.ViewModels;
using Common.Extensions;

namespace AttendanceAPI.List
{
    [HiStaffAuthorize]
    [ApiExplorerSettings(GroupName = "3-ATTENDANCE-OVER-TIME")]
    [ApiController]

    [Route("api/hr/OverTime/[action]")]
    public class OverTimeController : BaseController2
    {
        public OverTimeController(IAttendanceUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }
       
        //[HttpGet]
        //public async Task<ActionResult> GetAll(OverTimeDTO param)
        //{
        //    var r = await _unitOfWork.OverTimeRepository.GetAll(param);
        //    return Ok(r);
        //}
        [HttpPost]
        public async Task<ActionResult> Create([FromBody] OverTimeCreateDTO param)
        {
            if (!ModelState.IsValid)
            {
                return ResponseValidation();
            }
            var r = await _unitOfWork.OverTimeRepository.CreateAsync(param);
            return ResponseResult(r);
        }
        [HttpPost]
        public async Task<ActionResult> Update([FromBody] OverTimeInputDTO param)
        {
            if (!ModelState.IsValid)
            {
                return ResponseValidation();
            }
            var r = await _unitOfWork.OverTimeRepository.UpdateAsync(param);
            return ResponseResult(r);
        }
        [HttpPost]
        public async Task<ActionResult> Approved([FromBody] List<long> param)
        {
            if (!ModelState.IsValid)
            {
                return ResponseValidation();
            }
            var r = await _unitOfWork.OverTimeRepository.ChangeStatusAsync(param, OtherConfig.STATUS_APPROVE);
            return ResponseResult(r);
        }
        [HttpPost]
        public async Task<ActionResult> Denied([FromBody] List<long> param)
        {
            if (!ModelState.IsValid)
            {
                return ResponseValidation();
            }
            var r = await _unitOfWork.OverTimeRepository.ChangeStatusAsync(param, OtherConfig.STATUS_DECLINE);
            return ResponseResult(r);
        }
        [HttpPost]
        public async Task<ActionResult> Delete([FromBody] List<long> param)
        {
            if (!ModelState.IsValid)
            {
                return ResponseValidation();
            }
            var r = await _unitOfWork.OverTimeRepository.Delete(param);
            return ResponseResult(r);
        }


        [HttpGet]
        public async Task<ActionResult> GetConfig()
        {
            var r = await _unitOfWork.OverTimeRepository.GetConfig();
            return Ok(r);
        }
        [HttpPost]
        public async Task<ActionResult> UpdateConfig([FromBody] OverTimeConfigDTO param)
        {
            if (!ModelState.IsValid)
            {
                return ResponseValidation();
            }
            var r = await _unitOfWork.OverTimeRepository.UpdateConfig(param);
            return ResponseResult(r);
        }
       

        [HttpPost]
        public async Task<ActionResult> PortalReg([FromBody] RegisterOffInputDTO param)
        {
            if (!ModelState.IsValid)
            {
                return ResponseValidation();
            }
            var r = await _unitOfWork.RegisterOffRepository.PortalReg(param, Consts.REGISTER_OT);
            return ResponseResult(r);
        }

        [HttpPost]
        public async Task<ActionResult> PortalApprove([FromBody] PortalApproveDTO param)
        {
            if (!ModelState.IsValid)
            {
                return ResponseValidation();
            }
            var r = await _unitOfWork.RegisterOffRepository.PortalApprove(param, Consts.REGISTER_OT, OtherConfig.STATUS_APPROVE);
            return ResponseResult(r);
        }

        [HttpPost]
        public async Task<ActionResult> PortalReject([FromBody] PortalApproveDTO param)
        {
            if (!ModelState.IsValid)
            {
                return ResponseValidation();
            }
            var r = await _unitOfWork.RegisterOffRepository.PortalApprove(param, Consts.REGISTER_OT, OtherConfig.STATUS_DECLINE);
            return ResponseResult(r);
        }

        [HttpGet]
        public async Task<ActionResult> PortalWaitList()
        {
            var r = await _unitOfWork.RegisterOffRepository.PortalWaitList(Consts.REGISTER_OT);
            return Ok(r);
        }

    }
}
