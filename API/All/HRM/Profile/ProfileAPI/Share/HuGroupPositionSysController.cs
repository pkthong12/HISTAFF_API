using Microsoft.AspNetCore.Mvc;
using ProfileDAL.Repositories;
using ProfileDAL.ViewModels;

namespace ProfileAPI.Share
{
    [HiStaffAuthorize]
    [ApiExplorerSettings(GroupName = "002-PROFILE-SHARE-GROUP-POSITION-SYS")]
    [ApiController]

    [Route("api/[controller]/[action]")]
    public class HuGroupPositionSysController : BaseController1
    {
        public HuGroupPositionSysController(IProfileUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        [HttpGet]
        public async Task<ActionResult> GetAll(GroupPositionSysDTO param)
        {
            var r = await _unitOfWork.GroupPositionSysRepository.GetAll(param);
            return Ok(r);
        }
        [HttpGet]
        public async Task<ActionResult> Get(int Id)
        {
            var r = await _unitOfWork.GroupPositionSysRepository.GetById(Id);
            return ResponseResult(r);
        }
        [HttpPost]
        public async Task<ActionResult> Add([FromBody]GroupPositionSysInputDTO param)
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


            var r = await _unitOfWork.GroupPositionSysRepository.CreateAsync(param);
            return ResponseResult(r);
        }

        [HttpPost]
        public async Task<ActionResult> Update([FromBody]GroupPositionSysInputDTO param)
        {

            if (param.Id == null)
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

            var r = await _unitOfWork.GroupPositionSysRepository.UpdateAsync(param);
            return ResponseResult(r);
        }
        [HttpPost]
        public async Task<ActionResult> ChangeStatus([FromBody]List<long> ids)
        {
            var r = await _unitOfWork.GroupPositionSysRepository.ChangeStatusAsync(ids);
            return ResponseResult(r);
        }

        [HttpGet]
        public async Task<ActionResult> GetList()
        {
            try
            {
                var r = await _unitOfWork.GroupPositionSysRepository.GetList();
                return ResponseResult(r);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpGet]
        public async Task<ActionResult> GetListByArea(int areaId)
        {
            try
            {
                var r = await _unitOfWork.GroupPositionSysRepository.GetListByArea(areaId);
                return ResponseResult(r);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        [HttpPost]
        public async Task<ActionResult> Delete([FromBody]int id)
        {
            var r = await _unitOfWork.GroupPositionSysRepository.Delete(id);
            return ResponseResult(r);
        }
    }
}
