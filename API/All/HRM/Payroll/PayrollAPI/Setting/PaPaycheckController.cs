using Microsoft.AspNetCore.Mvc;
using PayrollDAL.ViewModels;
using PayrollDAL.Repositories;

namespace PayrollAPI.List
{
    [HiStaffAuthorize]
    [ApiExplorerSettings(GroupName = "003-PAYROLL-PAY-CHECK")]
    [ApiController]

    [Route("api/[controller]/[action]")]
    public class PaPaycheckController : BaseController
    {
        public PaPaycheckController(IPayrollUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }

        [HttpGet]
        public async Task<ActionResult> GetAll(PaycheckDTO values)
        {
            var r = await _unitOfWork.PaycheckRepository.GetAll(values);
            return Ok(r);
        }

        [HttpGet]
        public async Task<ActionResult> Get(int Id)
        {
            var r = await _unitOfWork.PaycheckRepository.GetById(Id);
            return ResponseResult(r);
        }
        
      
        [HttpPost]
        public async Task<ActionResult> Add([FromBody] PaycheckInputListDTO param)
        {

            var r = await _unitOfWork.PaycheckRepository.CreateAsync(param);
            return ResponseResult(r);
        }

        [HttpPost]
        public async Task<ActionResult> Update([FromBody] PaycheckInputDTO param)
        {

            var r = await _unitOfWork.PaycheckRepository.UpdateAsync(param);
            return ResponseResult(r);
        }
        

        [HttpPost]
        public async Task<ActionResult> QuickUpdate([FromBody] PaycheckInputDTO param)
        {

            var r = await _unitOfWork.PaycheckRepository.QuickUpdate(param);
            return ResponseResult(r);
        }

        [HttpPost]
        public async Task<ActionResult> Remove([FromBody] List<long> ids)
        {

            var r = await _unitOfWork.PaycheckRepository.RemoveAsync(ids);
            return ResponseResult(r);
        }
    }
}
