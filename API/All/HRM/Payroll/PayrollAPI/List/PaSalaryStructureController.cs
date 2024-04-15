using Microsoft.AspNetCore.Mvc;
using PayrollDAL.ViewModels;
using PayrollDAL.Repositories;
using Common.Extensions;

namespace PayrollAPI.List
{
    [HiStaffAuthorize]
    [ApiExplorerSettings(GroupName = "003-PAYROLL-SALARY-STRUCTURE")]
    [ApiController]

    [Route("api/[controller]/[action]")]
    public class PaSalaryStructureController : BaseController
    {
        public PaSalaryStructureController(IPayrollUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }

        [HttpGet]
        public async Task<ActionResult> GetAll(SalaryStructureDTO values)
        {
            var r = await _unitOfWork.SalaryStructureRepository.GetAll(values);
            return Ok(r);
        }

        [HttpGet]
        public async Task<ActionResult> Get(int Id)
        {
            var r = await _unitOfWork.SalaryStructureRepository.GetById(Id);
            return ResponseResult(r);
        }
        [HttpGet]
        public async Task<ActionResult> GetList(int SalaryTypeId)
        {
            var r = await _unitOfWork.SalaryStructureRepository.GetList(SalaryTypeId);
            return ResponseResult(r);
        }
        [HttpGet]
        public async Task<ActionResult> GetListImport(int SalaryTypeId)
        {
            var r = await _unitOfWork.SalaryStructureRepository.GetListImport(SalaryTypeId);
            return ResponseResult(r);
        }
        [HttpPost]
        public async Task<ActionResult> Add([FromBody] SalaryStructureInputDTO param)
        {

            var r = await _unitOfWork.SalaryStructureRepository.CreateAsync(param);
            return ResponseResult(r);
        }

        [HttpPost]
        public async Task<ActionResult> Update([FromBody] SalaryStructureInputDTO param)
        {

            var r = await _unitOfWork.SalaryStructureRepository.UpdateAsync(param);
            return ResponseResult(r);
        }
        [HttpGet]
        public async Task<ActionResult> GetSalaryElement(long salaryTypeId)
        {
            var r = await _unitOfWork.SalaryStructureRepository.GetElement(salaryTypeId);
            return ResponseResult(r);
        }

        [HttpPost]
        public async Task<ActionResult> QuickUpdate([FromBody] SalaryStructureInputDTO param)
        {

            var r = await _unitOfWork.SalaryStructureRepository.QuickUpdate(param);
            return ResponseResult(r);
        }

        [HttpPost]
        public async Task<ActionResult> MoveTableIndex([FromBody] List<TempSortInputDTO> param)
        {
            if (!ModelState.IsValid)
            {
                return ResponseValidation();
            }
            var r = await _unitOfWork.FormulaRepository.MoveTableIndex(param, OtherConfig.SORT_FML_STRUCT);
            return Ok(r);
        }

        [HttpPost]
        public async Task<ActionResult> Delete([FromBody] int id)
        {
            if (!ModelState.IsValid)
            {
                return ResponseValidation();
            }
            var r = await _unitOfWork.SalaryStructureRepository.Delete(id);
            return Ok(r);
        }
    }
}
