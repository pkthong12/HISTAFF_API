using Microsoft.AspNetCore.Mvc;
using ProfileDAL.Repositories;
using ProfileDAL.ViewModels;

namespace ProfileAPI.Share
{
    [HiStaffAuthorize]
    [ApiExplorerSettings(GroupName = "002-PROFILE-SHARE-SHIFT-SYS")]
    [ApiController]

    [Route("api/[controller]/[action]")]
    public class HuShiftSysController : BaseController1
    {
        public HuShiftSysController(IProfileUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }
        [HttpGet]
        public async Task<ActionResult> GetAll(ShiftSysDTO param)
        {
            try
            {
                var r = await _unitOfWork.ShiftSysRepository.GetAll(param);
                return Ok(r);
            }
            catch (Exception ex)
            {

                throw;
            }
            
        }
        [HttpGet]
        public async Task<ActionResult> Get(int Id)
        {
            var r = await _unitOfWork.ShiftSysRepository.GetById(Id);
            return ResponseResult(r);
        }
        [HttpGet]
        public async Task<ActionResult> GetList()
        {
            var r = await _unitOfWork.ShiftSysRepository.GetList();
            return ResponseResult(r);
        }
        [HttpGet]
        public async Task<ActionResult> GetShiftCycle(int Id)
        {
            var r = await _unitOfWork.ShiftSysRepository.GetShiftCycle(Id);
            return ResponseResult(r);
        }
        [HttpPost]
        public async Task<ActionResult> Add([FromBody] ShiftSysInputDTO param)
        {
            if (!ModelState.IsValid)
            {
                return ResponseValidation();
            }
            var r = await _unitOfWork.ShiftSysRepository.CreateAsync(param);
            return ResponseResult(r);
        }
        [HttpPost]
        public async Task<ActionResult> Update([FromBody] ShiftSysInputDTO param)
        {
            if (!ModelState.IsValid)
            {
                return ResponseValidation();
            }
            var r = await _unitOfWork.ShiftSysRepository.UpdateAsync(param);
            return ResponseResult(r);

        }
        [HttpPost]
        public async Task<ActionResult> UpdateShiftCycle([FromBody] ShiftCycleSysInput param)
        {
            if (!ModelState.IsValid)
            {
                return ResponseValidation();
            }
            var r = await _unitOfWork.ShiftSysRepository.UpdateShiftCycle(param);
            return ResponseResult(r);

        }
    }
}
