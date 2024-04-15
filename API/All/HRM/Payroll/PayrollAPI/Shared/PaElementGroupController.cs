using Microsoft.AspNetCore.Mvc;
using PayrollDAL.ViewModels;
using PayrollDAL.Repositories;

namespace PayrollAPI.List
{
    [HiStaffAuthorize]
    [ApiExplorerSettings(GroupName = "003-PAYROLL-ELEMENT-GROUP")]
    [ApiController]

    [Route("api/[controller]/[action]")]
    public class PaElementGroupController : BaseController
    {
        public PaElementGroupController(IPayrollUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }

        [HttpGet]
        public async Task<ActionResult> GetAll(ElementGroupDTO values)
        {
            var r = await _unitOfWork.ElementGroupRepository.GetAll(values);
            return Ok(r);
        }

        [HttpGet]
        public async Task<ActionResult> Get(int Id)
        {
            var r = await _unitOfWork.ElementGroupRepository.GetById(Id);
            return ResponseResult(r);
        }
        [HttpPost]
        public async Task<ActionResult> Add([FromBody]ElementGroupInputDTO param)
        {

            var r = await _unitOfWork.ElementGroupRepository.CreateAsync(param);
            return ResponseResult(r);
        }

        [HttpPost]
        // old update
        // new udateElement
        public async Task<ActionResult> UpdateElement([FromBody]ElementGroupInputDTO param)
        {

            var r = await _unitOfWork.ElementGroupRepository.UpdateAsync(param);
            return ResponseResult(r);
        }

        [HttpPost]
        public async Task<ActionResult> Update([FromBody]List<long> ids)
        {

            var r = await _unitOfWork.ElementGroupRepository.ChangeStatusAsync(ids);
            return ResponseResult(r);
        }

        [HttpGet]
        public async Task<ActionResult> GetList()
        {
            var r = await _unitOfWork.ElementGroupRepository.GetList();
            return ResponseResult(r);
        }
    }
}
