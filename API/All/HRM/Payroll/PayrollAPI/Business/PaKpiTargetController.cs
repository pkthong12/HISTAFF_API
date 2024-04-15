using Microsoft.AspNetCore.Mvc;
using PayrollDAL.ViewModels;
using PayrollDAL.Repositories;

namespace PayrollAPI.List
{
    [HiStaffAuthorize]
    [ApiExplorerSettings(GroupName = "003-PAYROLL-KPI-TARGET")]
    [ApiController]

    [Route("api/[controller]/[action]")]
    public class PaKpiTargetController : BaseController
    {
        public PaKpiTargetController(IPayrollUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }

        [HttpGet]
        public async Task<ActionResult> GetAll(KpiTargetDTO values)
        {
            var r = await _unitOfWork.KpiTargetRepository.GetAll(values);
            return Ok(r);
        }

        [HttpGet]
        public async Task<ActionResult> Get(int Id)
        {
            var r = await _unitOfWork.KpiTargetRepository.GetById(Id);
            return ResponseResult(r);
        }
        [HttpGet]
        public async Task<ActionResult> GetList()
        {
            var r = await _unitOfWork.KpiTargetRepository.GetList();
            return ResponseResult(r);
        }
        //[HttpGet]
        //public async Task<ActionResult> GetList(int KpiGroupId, int? typeId)
        //{
        //    var r = await _unitOfWork.KpiTargetRepository.GetList(KpiGroupId, typeId);
        //    return ResponseResult(r);
        //}
        [HttpPost]
        public async Task<ActionResult> Add([FromBody] KpiTargetInputDTO param)
        {

            var r = await _unitOfWork.KpiTargetRepository.CreateAsync(param);
            return ResponseResult(r);
        }

        [HttpPost]
        public async Task<ActionResult> Update([FromBody] KpiTargetInputDTO param)
        {

            var r = await _unitOfWork.KpiTargetRepository.UpdateAsync(param);
            return ResponseResult(r);
        }

        [HttpPost]
        public async Task<ActionResult> ChangeStatus([FromBody] List<long> ids)
        {
            var r = await _unitOfWork.KpiTargetRepository.ChangeStatusAsync(ids);
            return ResponseResult(r);
        }


        [HttpPost]
        public async Task<ActionResult> QuickUpdate([FromBody] KpiTargetQickDTO param)
        {

            var r = await _unitOfWork.KpiTargetRepository.QuickUpdate(param);
            return ResponseResult(r);
        }
    }
}
