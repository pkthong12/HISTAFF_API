using Microsoft.AspNetCore.Mvc;
using ProfileDAL.Repositories;
using ProfileDAL.ViewModels;

namespace ProfileAPI.List
{
    [HiStaffAuthorize]
    [ApiExplorerSettings(GroupName = "002-PROFILE-ROOM")]
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class AdRoomController : BaseController1
    {
        public AdRoomController(IProfileUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }
        [HttpGet]
        public async Task<ActionResult> GetAll(RoomDTO param)
        {
            var r = await _unitOfWork.RoomRepository.GetAll(param);
            return Ok(r);
        }
        [HttpGet]
        public async Task<ActionResult> Get(int Id)
        {
            var r = await _unitOfWork.RoomRepository.GetById(Id);
            return ResponseResult(r);
        }
        [HttpPost]
        public async Task<ActionResult> Add([FromBody]RoomInputDTO param)
        {
            if (!ModelState.IsValid)
            {
                return ResponseValidation();
            }
            var r = await _unitOfWork.RoomRepository.CreateAsync(param);
            return ResponseResult(r);
        }
        [HttpPost]
        public async Task<ActionResult> Update([FromBody]RoomInputDTO param)
        {
            if (!ModelState.IsValid)
            {
                return ResponseValidation();
            }
            var r = await _unitOfWork.RoomRepository.UpdateAsync(param);
            return ResponseResult(r);
        }
        [HttpPost]
        public async Task<ActionResult> Remove([FromBody]int id)
        {
            var r = await _unitOfWork.RoomRepository.Remove(id);
            return ResponseResult(r);
        }

        [HttpPost]
        public async Task<ActionResult> ChangeStatus([FromBody]List<long> ids)
        {
            var r = await _unitOfWork.RoomRepository.ChangeStatusAsync(ids);
            return ResponseResult(r);
        }

        [HttpGet]
        public async Task<ActionResult> GetList()
        {
            var r = await _unitOfWork.RoomRepository.GetList();
            return Ok(r);
        }
    }
}
