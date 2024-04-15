using Microsoft.AspNetCore.Mvc;
using PayrollDAL.ViewModels;
using PayrollDAL.Repositories;
using Common.Extensions;

namespace PayrollAPI.List
{
    [HiStaffAuthorize]
    [ApiExplorerSettings(GroupName = "003-PAYROLL-KPI-FORMULA")]
    [ApiController]

    [Route("api/[controller]/[action]")]
    public class PaKpiFormulaController : BaseController
    {
        public PaKpiFormulaController(IPayrollUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }

        [HttpGet]
        public async Task<ActionResult> GetAll(KpiFormulaDTO values)
        {
            var r = await _unitOfWork.KpiFormulaRepository.GetAll(values);
            return Ok(r);
        }

        [HttpGet]
        public async Task<ActionResult> Get(int Id)
        {
            var r = await _unitOfWork.KpiFormulaRepository.GetById(Id);
            return ResponseResult(r);
        }

        [HttpGet]
        public async Task<ActionResult> GetList()
        {
            var r = await _unitOfWork.KpiFormulaRepository.GetList();
            return ResponseResult(r);
        }
        [HttpGet]
        public async Task<ActionResult> GetListFomula()
        {
            var r = await _unitOfWork.KpiTargetRepository.GetListFomula();
            return ResponseResult(r);
        }
       

        [HttpPost]
        public async Task<ActionResult> Update([FromBody] KpiFormulaCreateDTO param)
        {

            var r = await _unitOfWork.KpiFormulaRepository.UpdateAsync(param);
            return ResponseResult(r);
        }

        [HttpPost]
        public async Task<ActionResult> ChangeStatus([FromBody] List<long> ids)
        {
            var r = await _unitOfWork.KpiFormulaRepository.ChangeStatusAsync(ids);
            return ResponseResult(r);
        }
        [HttpPost]
        public async Task<ActionResult> MoveTableIndex([FromBody] List<TempSortInputDTO> param)
        {
            if (!ModelState.IsValid)
            {
                return ResponseValidation();
            }
            var r = await _unitOfWork.FormulaRepository.MoveTableIndex(param, OtherConfig.SORT_FML_KPI);
            return Ok(r);
        }
    }
}
