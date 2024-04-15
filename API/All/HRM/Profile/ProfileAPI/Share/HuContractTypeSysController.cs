using Microsoft.AspNetCore.Mvc;
using ProfileDAL.Repositories;
using ProfileDAL.ViewModels;

namespace ProfileAPI.Share
{
    [HiStaffAuthorize]
    [ApiExplorerSettings(GroupName = "002-PROFILE-CONTRACT-TYPE-SYS")]
    [ApiController]

    [Route("api/[controller]/[action]")]
    public class HuContractTypeSysController : BaseController1
    {
        public HuContractTypeSysController(IProfileUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }
        [HttpGet]
        public async Task<ActionResult> GetAll(ContractTypeSysViewDTO param)
        {
            var r = await _unitOfWork.ContractTypeSysRepository.GetAll(param);
            return  Ok(r);
        }
        [HttpGet]
        public async Task<ActionResult> Get(int Id)
        {
            var r = await _unitOfWork.ContractTypeSysRepository.GetById(Id);
            return ResponseResult(r);
        }
        [HttpPost]
        public async Task<ActionResult> Add([FromBody]ContractTypeSysInputDTO param)
        {
            if (param == null)
            {
                return ResponseResult("PARAM_NOT_BLANK");
            }
            if(string.IsNullOrWhiteSpace(param.Code))
            {
                return ResponseResult("CODE_NOT_BLANK");
            }
            if (string.IsNullOrWhiteSpace(param.Name))
            {
                return ResponseResult("NAME_NOT_BLANK");
            }
            
            var r = await _unitOfWork.ContractTypeSysRepository.CreateAsync(param);
            return ResponseResult(r);
        }
        [HttpPost]
        public async Task<ActionResult> Update([FromBody]ContractTypeSysInputDTO param)
        {
            if (param == null)
            {
                return ResponseResult("PARAM_NOT_BLANK");
            }
            if (param.Id == null)
            {
                return ResponseResult("ID_NOT_BLANK");
            }         
            if (string.IsNullOrWhiteSpace(param.Code))
            {
                return ResponseResult("CODE_NOT_BLANK");
            }
            if (string.IsNullOrWhiteSpace(param.Name))
            {
                return ResponseResult("NAME_NOT_BLANK");
            }

            var r = await _unitOfWork.ContractTypeSysRepository.UpdateAsync(param);
            return ResponseResult(r);
        }
        [HttpPost]
        public async Task<ActionResult> ChangeStatus([FromBody]List<long> ids)
        {
            var r = await _unitOfWork.ContractTypeSysRepository.ChangeStatusAsync(ids);
            return ResponseResult(r);
        }
        [HttpGet]
        public async Task<ActionResult> GetList()
        {
            var r = await _unitOfWork.ContractTypeSysRepository.GetList();
            return ResponseResult(r);
        }

    }
}
