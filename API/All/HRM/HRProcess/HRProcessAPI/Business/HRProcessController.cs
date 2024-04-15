/*
using Microsoft.AspNetCore.Mvc;
using HRProcessDAL.Repositories;
using HRProcessDAL.ViewModels;

namespace HRProcessAPI.List
{
    [HiStaffAuthorize]
    [Produces("application/json")]
    [Route("api/hr/hrprocess/[action]")]
    public class HRProcessController : BaseController3
    {
        public HRProcessController(IHRProcessBusiness unitOfWork) : base(unitOfWork)
        {

        }
        [HttpGet]
        public async Task<ActionResult> GetListHrProcessType(SeHrProcessTypeDTO param)
        {
            if (!ModelState.IsValid)
            {
                return ResponseValidation();
            }
            var r = await _unitOfWork.HRProcessRepository.GetAll(param);
            return Ok(r);
        }
    }
}
*/