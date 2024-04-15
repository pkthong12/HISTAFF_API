/* 
 * DEPRECATED - ĐÃ ĐỔI NHÀ SANG THƯ MỤC NGOÀI

using CORE.DTO;
using CORE.Enum;
using Microsoft.AspNetCore.Mvc;
using ProfileDAL.Repositories;
using ProfileDAL.ViewModels;

namespace ProfileAPI.List
{
    [HiStaffAuthorize]
    [ApiExplorerSettings(GroupName = "002-PROFILE-ORGANIZATION")]
    [ApiController]

    [Route("api/[controller]/[action]")]
    public class HuOrganizationController : BaseController1
    {
        public HuOrganizationController(IProfileUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }
        [HttpGet]
        public async Task<ActionResult> GetAll(OrganizationInputDTO param)
        {
            try
            {
                var r = await _unitOfWork.OrganizationRepository.GetAll(param);
                return Ok(new FormatedResponse() { InnerBody = r.Data });
            } catch (Exception ex)
            {
                return Ok(new FormatedResponse() { ErrorType = EnumErrorType.UNCATCHABLE, StatusCode = EnumStatusCode.StatusCode500, MessageCode = ex.Message });
            }
            
        }
        [HttpGet]
        public async Task<ActionResult> GetTreeView()
        {
            var r = await _unitOfWork.OrganizationRepository.GetTreeView();
            return ResponseResult(r);
        }
        [HttpGet]
        public async Task<ActionResult> GetById(int Id)
        {
            try
            {
                var r = await _unitOfWork.OrganizationRepository.GetById(Id);
                return Ok(new FormatedResponse() { InnerBody = r.Data });
            } catch (Exception ex)
            {
                return Ok(new FormatedResponse() { ErrorType = EnumErrorType.UNCATCHABLE, StatusCode = EnumStatusCode.StatusCode500, MessageCode = ex.Message });
            }
        }
        [HttpPost]
        public async Task<ActionResult> Add([FromBody] OrganizationInputDTO param)
        {
            if (!ModelState.IsValid)
            {
                return ResponseValidation();
            }
            if (param == null)
            {
                return ResponseResult("PARAM_NOT_BLANK");
            }
            if (string.IsNullOrWhiteSpace(param.Code))
            {
                return ResponseResult("CODE_NOT_BLANK");
            }
            if (string.IsNullOrWhiteSpace(param.Name))
            {
                return ResponseResult("NAME_NOT_BLANK");
            }

            var r = await _unitOfWork.OrganizationRepository.CreateAsync(param);
            return ResponseResult(r);
        }
        [HttpPost]
        public async Task<ActionResult> Update([FromBody] OrganizationInputDTO param)
        {
            if (param == null)
            {
                return ResponseResult("PARAM_NOT_BLANK");
            }
          

            var r = await _unitOfWork.OrganizationRepository.UpdateAsync(param);
            return ResponseResult(r);
        }

        [HttpPost]
        public async Task<ActionResult> Sort([FromBody] OrganizationInputDTO param)
        {
            if (param.Id == null)
            {
                return ResponseResult("PARAM_ID_NOT_BLANK");
            }
            if (param.ParentId == null)
            {
                return ResponseResult("PARAM_PARENT_NOT_BLANK");
            }
            var r = await _unitOfWork.OrganizationRepository.SortAsync(param);
            return ResponseResult(r);
        }
        
        [HttpGet]
        public async Task<ActionResult> GetList()
        {
            var r = await _unitOfWork.OrganizationRepository.GetList();
            return ResponseResult(r);
        }
        [HttpGet]
        public async Task<ActionResult> Delete(long id)
        {
            if (id == 0)
            {
                return ResponseResult("PARAM_NOT_BLANK");
            }
            var r = await _unitOfWork.OrganizationRepository.Delete(id);
            return ResponseResult(r);
        }
        [HttpGet]
        public async Task<ActionResult> GetOrgPermission()
        {
            var r = await _unitOfWork.UserOrganiRepository.GetOrgPermission();
            return Ok(r);
        }
        [HttpGet]
        public async Task<ActionResult> GetAllOrgChartPosition(OrgChartRptInputDTO param)
        {
            var r = await _unitOfWork.OrganizationRepository.GetAllOrgChartPosition(param);
            return ResponseResult(r);
        }
        [HttpGet]
        public async Task<ActionResult> GetJobPosTree(JobPositionTreeInputDTO param)
        {
            var r = await _unitOfWork.OrganizationRepository.GetJobPosTree(param);
            return ResponseResult(r);
        }
        [HttpPost]
        public async Task<ActionResult> UpdateCreateRptJobPosHis(JobPositionTreeInputDTO param)
        {
            var r = _unitOfWork.OrganizationRepository.UpdateCreateRptJobPosHisAsync(param);
            return ResponseResult(r);
        }
        [HttpGet]
        public async Task<ActionResult> GetJobChildTree(JobChildTreeInputDTO param)
        {
            var r = await _unitOfWork.OrganizationRepository.GetJobChildTree(param);
            return ResponseResult(r);
        }
    }
}
*/