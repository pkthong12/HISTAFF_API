using Microsoft.AspNetCore.Mvc;
using PayrollDAL.ViewModels;
using PayrollDAL.Repositories;

namespace PayrollAPI.List
{
    [HiStaffAuthorize]
    [ApiExplorerSettings(GroupName = "003-PAYROLL-PAYROLL")]
    [ApiController]

    [Route("api/[controller]/[action]")]
    public class PaPayrollController : BaseController
    {
        public PaPayrollController(IPayrollUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }

        [HttpGet]
        public async Task<ActionResult> PortalGetBy(int periodId)
        {

            var r = await _unitOfWork.FormulaRepository.PortalGetBy(periodId);
            return Ok(r);
        }
        [HttpGet]
        public async Task<ActionResult> CheckPayrollLock(LockInputDTO param)
        {
            if (!ModelState.IsValid)
            {
                return ResponseValidation();
            }
            var r = await _unitOfWork.FormulaRepository.IsLockPayroll(param);
            return Ok(r);
        }
        [HttpGet]
        public async Task<ActionResult> LockPayroll(LockInputDTO param)
        {
            if (!ModelState.IsValid)
            {
                return ResponseValidation();
            }
            var r = await _unitOfWork.FormulaRepository.LockPayroll(param);
            return Ok(r);
        }
    }
}
