using Microsoft.AspNetCore.Mvc;
using ProfileDAL.Repositories;
using ProfileDAL.ViewModels;

namespace ProfileAPI.Share
{
    [HiStaffAuthorize]
    [ApiExplorerSettings(GroupName = "002-PROFILE-SHARE-SALARY-ELEMENT-SYS")]
    [ApiController]

    [Route("api/[controller]/[action]")]
    public class HuSalaryElementSysController : BaseController1
    {
        public HuSalaryElementSysController(IProfileUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }

        [HttpGet]
        public async Task<ActionResult> GetAll(SalaryElementSysDTO values)
        {
            var r = await _unitOfWork.SalaryElementSysRepository.GetAll(values);
            return Ok(r);
        }

        [HttpGet]
        public async Task<ActionResult> Get(int Id)
        {
            var r = await _unitOfWork.SalaryElementSysRepository.GetById(Id);
            return ResponseResult(r);
        }
        [HttpGet]
        public async Task<ActionResult> GetListGroup()
        {
            var r = await _unitOfWork.SalaryElementSysRepository.GetListGroup();
            return ResponseResult(r);
        }
        [HttpGet]
        public async Task<ActionResult> GetList(int groupid)
        {
            var r = await _unitOfWork.SalaryElementSysRepository.GetList(groupid);
            return ResponseResult(r);
        }
        [HttpPost]
        public async Task<ActionResult> Add([FromBody] SalaryElementSysInputDTO param)
        {

            var r = await _unitOfWork.SalaryElementSysRepository.CreateAsync(param);
            return ResponseResult(r);
        }

        [HttpPost]
        public async Task<ActionResult> Update([FromBody] SalaryElementSysInputDTO param)
        {

            var r = await _unitOfWork.SalaryElementSysRepository.UpdateAsync(param);
            return ResponseResult(r);
        }
        [HttpGet]
        public async Task<ActionResult> GetSalaryElement(GroupElementSysDTO param)
        {
            var r = await _unitOfWork.SalaryElementSysRepository.GetSalaryElement(param);
            return ResponseResult(r);
        }
        [HttpGet]
        public async Task<ActionResult> GetListCal(int SalaryTypeId)
        {
            var r = await _unitOfWork.SalaryElementSysRepository.GetListCal(SalaryTypeId);
            return ResponseResult(r);
        }
        [HttpPost]
        public async Task<ActionResult> ChangeStatus([FromBody] List<long> ids)
        {
            var r = await _unitOfWork.SalaryElementSysRepository.ChangeStatusAsync(ids);
            return ResponseResult(r);
        }
    }
}
