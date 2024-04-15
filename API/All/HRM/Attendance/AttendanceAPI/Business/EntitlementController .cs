using Microsoft.AspNetCore.Mvc;
using AttendanceDAL.Repositories;
using AttendanceDAL.ViewModels;
using Common.Extensions;
using CORE.DTO;
using CORE.Enum;
using CORE.StaticConstant;

namespace AttendanceAPI.List
{
    [HiStaffAuthorize]
    [ApiExplorerSettings(GroupName = "001-ATTENDANCE-ENTITLEMENT")]
    [ApiController]

    [Route("api/[controller]/[action]")]
    public class EntitlementController  : BaseController2
    {
        public EntitlementController (IAttendanceUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }
        [HttpPost]
        public async Task<IActionResult> QueryList(GenericQueryListDTO<AtEntitlementDTO> request)
        {
            try
            {

                var response = await _unitOfWork.EntitlementRepository.SinglePhaseQueryList(request);

                 if (response.ErrorType != EnumErrorType.NONE)
                 {
                    return Ok(new FormatedResponse()
                    {
                        ErrorType = response.ErrorType,
                        MessageCode = response.MessageCode ?? CommonMessageCode.NOT_ALL_CASES_CATCHED,
                        StatusCode = EnumStatusCode.StatusCode400,
                    });
                 }
                else
                {
                    return Ok(new FormatedResponse()
                    {
                        InnerBody = response
                    });
                }
            }
            catch (Exception ex)
            {
                return Ok(new FormatedResponse() { ErrorType = EnumErrorType.UNCATCHABLE, StatusCode = EnumStatusCode.StatusCode500, MessageCode = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Calculate(AtEntitlementInputDTO request)
        {
            try
            {
                var r = await _unitOfWork.EntitlementRepository.Calculate(request);
                if (r.StatusCode == "200")
                {
                    return Ok(new FormatedResponse() { InnerBody = r.Data, StatusCode = EnumStatusCode.StatusCode200, MessageCode= "UI_COMPONENT_LABEL_WELFARE_AUTO_CALCULATE_SUCCESS" });
                }
                else
                {
                    return Ok(new FormatedResponse() { ErrorType = EnumErrorType.CATCHABLE, MessageCode = r.Error, StatusCode = EnumStatusCode.StatusCode400 });
                }

            }
            catch (Exception ex)
            {
                return Ok(new FormatedResponse() { ErrorType = EnumErrorType.UNCATCHABLE, StatusCode = EnumStatusCode.StatusCode500, MessageCode = ex.Message });
            }
        }

        
    }
}
