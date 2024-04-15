using Microsoft.AspNetCore.Mvc;
using PayrollDAL.ViewModels;
using PayrollDAL.Repositories;
using Common.Extensions;

namespace PayrollAPI.List
{
    [HiStaffAuthorize]
    [ApiExplorerSettings(GroupName = "000-PAYROLL-SALARY-ELEMENT")]
    [ApiController]

    [Route("api/[controller]/[action]")]
    public class PaSalaryElementController : BaseController
    {
        public PaSalaryElementController(IPayrollUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }

        [HttpGet]
        public async Task<ActionResult> GetAll(SalaryElementDTO values)
        {
            var r = await _unitOfWork.SalaryElementRepository.GetAll(values);
            return Ok(r);
        }

        [HttpGet]
        public async Task<ActionResult> Get(int Id)
        {
            var r = await _unitOfWork.SalaryElementRepository.GetById(Id);
            return ResponseResult(r);
        }
        [HttpGet]
        public async Task<ActionResult> GetListGroup()
        {
            var r = await _unitOfWork.SalaryElementRepository.GetListGroup();
            return ResponseResult(r);
        }
        [HttpGet]
        public async Task<ActionResult> GetList(int groupid)
        {
            var r = await _unitOfWork.SalaryElementRepository.GetList(groupid);
            return ResponseResult(r);
        }
        [HttpPost]
        public async Task<ActionResult> Add([FromBody] SalaryElementInputDTO param)
        {

            var r = await _unitOfWork.SalaryElementRepository.CreateAsync(param);
            return ResponseResult(r);
        }

        [HttpPost]
        public async Task<ActionResult> Update([FromBody] SalaryElementInputDTO param)
        {

            var r = await _unitOfWork.SalaryElementRepository.UpdateAsync(param);
            return ResponseResult(r);
        }
        [HttpGet]
        public async Task<ActionResult> GetSalaryElement(long groupId)
        {
            var r = await _unitOfWork.SalaryElementRepository.GetSalaryElement(groupId);
            return ResponseResult(r);
        }
        [HttpGet]
        public async Task<ActionResult> GetSalaryElementSys()
        {
            var r = await _unitOfWork.SalaryElementRepository.GetSalaryElementSys();
            return ResponseResult(r);
        }
        [HttpGet]
        public async Task<ActionResult> GetListCal(int SalaryTypeId)
        {
            var r = await _unitOfWork.SalaryElementRepository.GetListCal(SalaryTypeId);
            return ResponseResult(r);
        }
        [HttpPost]
        public async Task<ActionResult> ChangeStatus([FromBody] List<long> ids)
        {
            var r = await _unitOfWork.SalaryElementRepository.ChangeStatusAsync(ids);
            return ResponseResult(r);
        }

        [HttpPost]
        public async Task<ActionResult> ShiftToElement([FromBody] ReferParam param)
        {
            var r = await _unitOfWork.SalaryElementRepository.AllowanceToElement(param, 2);
            return ResponseResult(r);
        }
        [HttpGet]
        public async Task<ActionResult> GetListAll()
        {
            var r = await _unitOfWork.SalaryElementRepository.GetListAll();
            return ResponseResult(r);
        }
        
    }
}
