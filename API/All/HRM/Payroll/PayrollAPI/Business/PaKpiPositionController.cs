using Microsoft.AspNetCore.Mvc;
using PayrollDAL.ViewModels;
using PayrollDAL.Repositories;

namespace PayrollAPI.List
{
    [HiStaffAuthorize]
    [ApiExplorerSettings(GroupName = "003-PAYROLL-KPI-POSITION")]
    [ApiController]

    [Route("api/[controller]/[action]")]
    public class PaKpiPositionController : BaseController
    {
        public PaKpiPositionController(IPayrollUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }

        [HttpGet]
        public async Task<ActionResult> GetAll(KpiPositionDTO values)
        {
            var r = await _unitOfWork.KpiPositionRepository.GetAll(values);
            return Ok(r);
        }

        [HttpGet]
        public async Task<ActionResult> Get(int Id)
        {
            var r = await _unitOfWork.KpiPositionRepository.GetById(Id);
            return ResponseResult(r);
        }


        [HttpPost]
        public async Task<ActionResult> Add([FromBody] KpiPositionInputDTO param)
        {

            var r = await _unitOfWork.KpiPositionRepository.CreateAsync(param);
            return ResponseResult(r);
        }

        [HttpPost]
        public async Task<ActionResult> Removes([FromBody] List<long> ids)
        {
            var r = await _unitOfWork.KpiPositionRepository.Removes(ids);
            return ResponseResult(r);
        }
    }
}
