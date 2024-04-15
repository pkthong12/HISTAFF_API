using Microsoft.AspNetCore.Mvc;
using AttendanceDAL.Repositories;
using AttendanceDAL.ViewModels;
using Common.Extensions;

namespace AttendanceAPI.List
{
    [HiStaffAuthorize]
    [ApiExplorerSettings(GroupName = "3-ATTENDANCE-TIME-LATE-EARLY")]
    [ApiController]

    [Route("api/hr/TimeLateEarly/[action]")]
    public class TimeLateEarlyController : BaseController2
    {
        public TimeLateEarlyController(IAttendanceUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }
        //[HttpGet]
        //public async Task<ActionResult> GetAll(RegisterOffDTO param)
        //{
        //    var r = await _unitOfWork.TimeLateEarlyRepository.GetAll(param);
        //    return Ok(r);
        //}
        [HttpGet]
        public async Task<ActionResult> Get(int Id)
        {
            var r = await _unitOfWork.TimeLateEarlyRepository.GetById(Id);
            return ResponseResult(r);
        }
        [HttpPost]
        public async Task<ActionResult> Add([FromBody] RegisterOffInputDTO param)
        {
            if (!ModelState.IsValid)
            {
                return ResponseValidation();
            }
            var r = await _unitOfWork.RegisterOffRepository.CreateAsync(param, Consts.REGISTER_LATE);
            return ResponseResult(r);
        }
        [HttpPost]
        public async Task<ActionResult> Update([FromBody] TimeLateEarlyInputDTO param)
        {
            if (!ModelState.IsValid)
            {
                return ResponseValidation();
            }
            var r = await _unitOfWork.TimeLateEarlyRepository.UpdateAsync(param);
            return ResponseResult(r);
        }



        [HttpPost]
        public async Task<ActionResult> PortalReg([FromBody] RegisterOffInputDTO param)
        {
            if (!ModelState.IsValid)
            {
                return ResponseValidation();
            }
            var r = await _unitOfWork.RegisterOffRepository.PortalReg(param,Consts.REGISTER_LATE);
            return ResponseResult(r);
        }



        [HttpPost]
        public async Task<ActionResult> PortalApprove([FromBody] PortalApproveDTO param)
        {
            if (!ModelState.IsValid)
            {
                return ResponseValidation();
            }
            var r = await _unitOfWork.RegisterOffRepository.PortalApprove(param, Consts.REGISTER_LATE, OtherConfig.STATUS_APPROVE);
            return ResponseResult(r);
        }

        [HttpPost]
        public async Task<ActionResult> PortalReject([FromBody] PortalApproveDTO param)
        {
            if (!ModelState.IsValid)
            {
                return ResponseValidation();
            }
            var r = await _unitOfWork.RegisterOffRepository.PortalApprove(param, Consts.REGISTER_LATE, OtherConfig.STATUS_DECLINE);
            return ResponseResult(r);
        }

        [HttpGet]
        public async Task<ActionResult> PortalWaitList()
        {
            var r = await _unitOfWork.RegisterOffRepository.PortalWaitList(Consts.REGISTER_LATE);
            return Ok(r);
        }
    }
}
