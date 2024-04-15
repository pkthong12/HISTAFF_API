using Microsoft.AspNetCore.Mvc;
using ProfileDAL.Repositories;
using ProfileDAL.ViewModels;

namespace ProfileAPI.Share
{
    [HiStaffAuthorize]
    [ApiExplorerSettings(GroupName = "002-PROFILE-SHARE-SALARY-STRUCTURE")]
    [ApiController]

    [Route("api/[controller]/[action]")]
    public class SalaryStructureSysController : BaseController1
    {
        public SalaryStructureSysController(IProfileUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }

        [HttpGet]
        public async Task<ActionResult> GetAll(SalaryStructureSysDTO values)
        {
            var r = await _unitOfWork.SalaryStructureSysRepository.GetAll(values);
            return Ok(r);
        }

        [HttpGet]
        public async Task<ActionResult> Get(int Id)
        {
            var r = await _unitOfWork.SalaryStructureSysRepository.GetById(Id);
            return ResponseResult(r);
        }
        [HttpGet]
        public async Task<ActionResult> GetList(int SalaryTypeId)
        {
            var r = await _unitOfWork.SalaryStructureSysRepository.GetList(SalaryTypeId);
            return ResponseResult(r);
        }
        [HttpPost]
        public async Task<ActionResult> Add([FromBody] SalaryStructureSysInputDTO param)
        {

            var r = await _unitOfWork.SalaryStructureSysRepository.CreateAsync(param);
            return ResponseResult(r);
        }

        [HttpPost]
        public async Task<ActionResult> Update([FromBody] SalaryStructureSysInputDTO param)
        {

            var r = await _unitOfWork.SalaryStructureSysRepository.UpdateAsync(param);
            return ResponseResult(r);
        }
        [HttpGet]
        public async Task<ActionResult> GetSalaryElement(long salaryTypeId)
        {
            var r = await _unitOfWork.SalaryStructureSysRepository.GetElement(salaryTypeId);
            return ResponseResult(r);
        }
    }
}
