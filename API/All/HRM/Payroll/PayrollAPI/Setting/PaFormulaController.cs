using Microsoft.AspNetCore.Mvc;
using PayrollDAL.ViewModels;
using PayrollDAL.Repositories;
using Common.Extensions;

namespace PayrollAPI.List
{
    [HiStaffAuthorize]
    [ApiExplorerSettings(GroupName = "003-PAYROLL-FORMULA")]
    [ApiController]

    [Route("api/[controller]/[action]")]
    public class PaFormulaController : BaseController
    {
        public PaFormulaController(IPayrollUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }


        [HttpGet]
        public async Task<ActionResult> GetElementCal(FormulaDTO values)
        {
            var r = await _unitOfWork.FormulaRepository.GetElementCal(values);
            return Ok(r);
        }

        [HttpPost]
        public async Task<ActionResult> Update([FromBody] FormulaInputDTO param)
        {

            var r = await _unitOfWork.FormulaRepository.Update(param);
            return ResponseResult(r);
        }
        [HttpGet]
        public async Task<ActionResult> ListPayrollSum(PayrollSumDTO param)
        {

            var r = await _unitOfWork.FormulaRepository.ListPayrollSum(param);
            return Ok(r.Data);
        }
        [HttpGet]
        public async Task<ActionResult> MBPayrollSum(PayrollInputMobile param)
        {

            var r = await _unitOfWork.FormulaRepository.MBPayrollSum(param);
            return Ok(r);
        }
        [HttpPost]
        public async Task<ActionResult> PayrollCal([FromBody] PayrollInputDTO param)
        {
            if (!ModelState.IsValid)
            {
                return ResponseValidation();
            }
            var r = await _unitOfWork.FormulaRepository.PayrollCal(param);
            return Ok(r);
        }
        [HttpPost]
        public async Task<ActionResult> CheckTimesheetLock([FromBody] PayrollInputDTO param)
        {
            if (!ModelState.IsValid)
            {
                return ResponseValidation();
            }
            var r = await _unitOfWork.FormulaRepository.CheckTimesheetLock(param);
            return Ok(r);
        }
        [HttpPost]
        public async Task<ActionResult> MoveTableIndex([FromBody] List<TempSortInputDTO> param)
        {
            if (!ModelState.IsValid)
            {
                return ResponseValidation();
            }
            var r = await _unitOfWork.FormulaRepository.MoveTableIndex(param, OtherConfig.SORT_FML_PAYROLL);
            return Ok(r);
        }
    }
}
