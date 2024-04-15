using Microsoft.AspNetCore.Mvc;
using ProfileDAL.Repositories;
using ProfileDAL.ViewModels;

namespace ProfileAPI.Share
{
    [HiStaffAuthorize]
    [ApiExplorerSettings(GroupName = "002-PROFILE-SHARE-SALARY-TYPE-SYS")]
    [ApiController]

    [Route("api/[controller]/[action]")]
    public class HuSalaryTypeSysController : BaseController1
    {
        public HuSalaryTypeSysController(IProfileUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }
        [HttpGet]
        public async Task<ActionResult> GetAll(SalaryTypeSysDTO param)
        {
            var r = await _unitOfWork.SalarySysRepository.GetAll(param);
            return Ok(r);
        }
        [HttpGet]
        public async Task<ActionResult> Get(int Id)
        {
            var r = await _unitOfWork.SalarySysRepository.GetById(Id);
            return ResponseResult(r);
        }
        [HttpPost]
        public async Task<ActionResult> Add([FromBody]SalaryTypeSysInputDTO param)
        {
            if (!ModelState.IsValid)
            {
                return ResponseValidation();
            }
            var r = await _unitOfWork.SalarySysRepository.CreateAsync(param);
            return ResponseResult(r);
        }
        [HttpPost]
        public async Task<ActionResult> Update([FromBody]SalaryTypeSysInputDTO param)
        {
            if (param == null)
            {
                return ResponseResult("PARAM_NOT_BLANK");
            }
            if (param.Id == null)
            {
                return ResponseResult("ID_NOT_BLANK");
            }
            if (string.IsNullOrWhiteSpace(param.Code))
            {
                return ResponseResult("CODE_NOT_BLANK");
            }
            if (string.IsNullOrWhiteSpace(param.Name))
            {
                return ResponseResult("NAME_NOT_BLANK");
            }

            var r = await _unitOfWork.SalarySysRepository.UpdateAsync(param);
            return ResponseResult(r);
        }
        [HttpPost]
        public async Task<ActionResult> ChangeStatus([FromBody]List<long> ids)
        {
            var r = await _unitOfWork.SalarySysRepository.ChangeStatusAsync(ids);
            return ResponseResult(r);
        }
        [HttpGet]
        public async Task<ActionResult> GetList(long areaId)
        {
            var r = await _unitOfWork.SalarySysRepository.GetList(areaId);
            return ResponseResult(r);
        }

    }
}
