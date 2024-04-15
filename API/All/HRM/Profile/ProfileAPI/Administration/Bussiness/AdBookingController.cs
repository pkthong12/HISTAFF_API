using Microsoft.AspNetCore.Mvc;
using ProfileDAL.Repositories;
using ProfileDAL.ViewModels;
using Common.Extensions;

namespace ProfileAPI.List
{
    [ApiExplorerSettings(GroupName = "002-PROFILE-BOOKING")]
    [ApiController]
    [HiStaffAuthorize]
    [Route("api/[controller]/[action]")]
    public class AdBookingController : BaseController1
    {
        public AdBookingController(IProfileUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }
        [HttpGet]
        public async Task<ActionResult> GetAll(BookingDTO param)
        {
            var r = await _unitOfWork.BookingRepository.GetAll(param);
            return Ok(r);
        }
        [HttpGet]
        public async Task<ActionResult> Get(int Id)
        {
            var r = await _unitOfWork.BookingRepository.GetById(Id);
            return ResponseResult(r);
        }
        [HttpPost]
        public async Task<ActionResult> PortalReg([FromBody]BookingInputDTO param)
        {
            if (!ModelState.IsValid)
            {
                return ResponseValidation();
            }
            var r = await _unitOfWork.BookingRepository.PortalReg(param);
            return ResponseResult(r);
        }
        [HttpPost]
        public async Task<ActionResult> PortalEditReg([FromBody]BookingInputDTO param)
        {
            if (!ModelState.IsValid)
            {
                return ResponseValidation();
            }
            var r = await _unitOfWork.BookingRepository.PortalEditReg(param);
            return ResponseResult(r);
        }
        [HttpPost]
        public async Task<ActionResult> PortalDelete([FromBody]int id)
        {
            var r = await _unitOfWork.BookingRepository.PortalDelete(id);
            return ResponseResult(r);
        }

        [HttpPost]
        public async Task<ActionResult> Approve([FromBody]ApproveDTO param)
        {
            var r = await _unitOfWork.BookingRepository.ChangeStatusAsync(param.Id, OtherConfig.STATUS_APPROVE,param.ApproveNote);
            return ResponseResult(r);
        }
        [HttpPost]
        public async Task<ActionResult> Reject([FromBody]ApproveDTO param)
        {
            var r = await _unitOfWork.BookingRepository.ChangeStatusAsync(param.Id, OtherConfig.STATUS_DECLINE, param.ApproveNote);
            return ResponseResult(r);
        }

        [HttpGet]
        public async Task<ActionResult> PortalMyList()
        {
            var r = await _unitOfWork.BookingRepository.PortalMyList();
            return Ok(r);
        }

        [HttpPost]
        public async Task<ActionResult> PortalListByRoom([FromBody]BookingDTO param)
        {
            var r = await _unitOfWork.BookingRepository.PortalListByRoom(param);
            return ResponseResult(r);
        }
        [HttpGet]
        public async Task<ActionResult> PortalGetBy(long id)
        {
            var r = await _unitOfWork.BookingRepository.PortalGetBy(id);
            return Ok(r);
        }
    }
}
