using Microsoft.AspNetCore.Mvc;
using ProfileDAL.Repositories;
using ProfileDAL.ViewModels;
using Common.Extensions;
using Common.BaseRequest;
using API.All.SYSTEM.CoreAPI.OrtherList;
using API.DTO;

namespace ProfileAPI.List
{
    [ApiController]
    [ApiExplorerSettings(GroupName = "2-PROFILE")]
    [HiStaffAuthorize]
    [Route("api/[controller]/[action]")]
    public class HuProfileController : BaseController1
    {
        public HuProfileController(IProfileUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }
        [HttpGet]
        public async Task<ActionResult> GetEmployeeInfo()
        {
            if (!ModelState.IsValid)
            {
                return ResponseValidation();
            }
            var r = await _unitOfWork.EmployeeRepository.GetEmployeeInfo();
            return Ok(r);

        }
        [HttpPost]
        public async Task<ActionResult> GetEmployeeById([FromBody] BaseRequest request)
        {
            var r = await _unitOfWork.EmployeeRepository.GetEmployeeById(request);
            return Ok(r);
        }

        [HttpPost]
        public async Task<ActionResult> UpdateEmployee([FromBody] EmployeeInput param)
        {
            var r = await _unitOfWork.EmployeeRepository.UpdateEmployee(param);
            return Ok();
        }

        [HttpPost]
        public async Task<ActionResult> GetOtherListById([FromBody] BaseRequest request)
        {
            var r = await _unitOfWork.EmployeeRepository.GetOtherListById(request);
            return Ok(r);
        }

        /// <summary>
        /// Get list HU_EMPLOYEE
        /// </summary>
        /// <remarks>
        /// {
        ///     "orgId":"1761",
        ///     "passno": "1",
        ///     "pagesize": "10"
        /// }
        /// </remarks>
        /// <param name="employeeCVRequest"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> GetEmployeeList([FromBody]  EmployeeDTO param)
        {
            if (!ModelState.IsValid)
            {
                return ResponseValidation();
            }
            var r = await _unitOfWork.EmployeeRepository.GetEmployeeList(param);
            return Ok(r);

        }

        /// <summary>
        /// Hợp đồng
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> GetContractInfo()
        {
            if (!ModelState.IsValid)
            {
                return ResponseValidation();
            }
            var r = await _unitOfWork.EmployeeRepository.GetContractInfo();
            return Ok(r);

        }
        /// <summary>
        /// Danh sách hợp đồng
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> GetContractList([FromBody] ContractDTO param)
        {
            if (!ModelState.IsValid)
            {
                return ResponseValidation();
            }
            var r = await _unitOfWork.EmployeeRepository.GetContractList(param);
            return Ok(r);

        }
        /// <summary>
        /// Hợp đồng
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> GetContractById([FromBody] BaseRequest request)
        {
            var r = await _unitOfWork.EmployeeRepository.GetContractById(request);
            return Ok(r);
        }
      
        /// <summary>
        /// Danh sách ngân hàng
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        /*[HttpPost]
        public async Task<ActionResult> GetBankList([FromBody] HuBankDTO param)
        {
            var r = await _unitOfWork.EmployeeRepository.GetBankList(param);
            return Ok(r);
        }*/
        /// <summary>
        /// Get Bank detail
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> GetBankById([FromBody] BaseRequest request)
        {
            var r = await _unitOfWork.EmployeeRepository.GetBankById(request);
            return Ok(r);
        }

        /// <summary>
        /// Chức vụ
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> GetPositionList([FromBody] PositionViewDTO param)
        {
            var r = await _unitOfWork.EmployeeRepository.GetPositionList(param);
            return Ok(r);
        }

        /// <summary>
        /// Chức vụ detail detail
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> GetPositionById([FromBody] BaseRequest request)
        {
            var r = await _unitOfWork.EmployeeRepository.GetPositionById(request);
            return Ok(r);
        }

        /// <summary>
        /// Tỉnh thành
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> GetProvinceList([FromBody] ProvinceDTO param)
        {
            var r = await _unitOfWork.EmployeeRepository.GetProvinceList(param);
            return Ok(r);
        }

        /// <summary>
        /// Tỉnh thành detail
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> GetProvinceById([FromBody] BaseRequest request)
        {
            var r = await _unitOfWork.EmployeeRepository.GetProvinceById(request);
            return Ok(r);
        }

        /// <summary>
        /// Tỉnh thành
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> GetDistrictList([FromBody] DistrictDTO param)
        {
            var r = await _unitOfWork.EmployeeRepository.GetDistrictList(param);
            return Ok(r);
        }

        /// <summary>
        /// Tỉnh thành detail
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> GetDistrictById([FromBody] BaseRequest request)
        {
            var r = await _unitOfWork.EmployeeRepository.GetDistrictById(request);
            return Ok(r);
        }

        /// <summary>
        /// Xã phường
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> GetWardList([FromBody] WardDTO param)
        {
            var r = await _unitOfWork.EmployeeRepository.GetWardlist(param);
            return Ok(r);
        }

        /// <summary>
        /// Xã phường detail
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> GetWardById([FromBody] BaseRequest request)
        {
            var r = await _unitOfWork.EmployeeRepository.GetWardById(request);
            return Ok(r);
        }


        /// <summary>
        /// Khen thưởng
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> GetBonusInfo()
        {
            var r = await _unitOfWork.EmployeeRepository.GetCommendInfo();
            return Ok(r);
        }
        /// <summary>
        /// Kỷ luật
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> GetDisciplineInfo()
        {
            var r = await _unitOfWork.EmployeeRepository.GetDisciplineInfo();
            return Ok(r);
        }
        /// <summary>
        /// Biến động bảo hiểm
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> GetInschangeInfo()
        {
            var r = await _unitOfWork.EmployeeRepository.GetInschangeInfo();
            return Ok(r);
        }

        /// <summary>
        /// Quyết định thay đổi
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> GetWorkingInfo()
        {
            var r = await _unitOfWork.EmployeeRepository.GetWorkingInfo();
            return Ok(r);
        }

        
        [HttpGet]
        public async Task<ActionResult> GetListEmployeePaper()
        {
            if (!ModelState.IsValid)
            {
                return ResponseValidation();
            }
            var r = await _unitOfWork.EmployeeRepository.GetListEmployeePaper();
            return Ok(r);
        }

        [HttpPost]
        public async Task<ActionResult> EmployeeEdit([FromBody] EmployeeEditInput param)
        {
            
            if (param == null){
                return Ok("Ko có dữ liệu");
            }
            var r = await _unitOfWork.EmployeeRepository.EmployeeEdit(param);
            return Ok(r);
        }

        /// <summary>
        /// Thông tin chính
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> EmployeeMainInfoEdit([FromBody] EmployeeMainInfoDTO param)
        {
            if (param == null)
            {
                return Ok("Ko có dữ liệu");
            }
            var r = await _unitOfWork.EmployeeRepository.EmployeeMainInfoEdit(param);
            return Ok(r);
        }

        /// <summary>
        /// Thông cá nhân
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> EmployeeInfoEdit([FromBody] EmployeeInfoDTO param)
        {
            if (param == null)
            {
                return Ok("Ko có dữ liệu");
            }
            var r = await _unitOfWork.EmployeeRepository.EmployeeInfoEdit(param);
            return Ok(r);
        }

        /// <summary>
        /// Địa chỉ thường trú
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> EmployeeAddressEdit([FromBody] EmployeeAddressDTO param)
        {
            if (param == null)
            {
                return Ok("Ko có dữ liệu");
            }
            var r = await _unitOfWork.EmployeeRepository.EmployeeAddressEdit(param);
            return Ok(r);
        }

        /// <summary>
        /// Thông cá nhân
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> EmployeeCurAddressEdit([FromBody] EmployeeCurAddressDTO param)
        {
            if (param == null)
            {
                return Ok("Ko có dữ liệu");
            }
            var r = await _unitOfWork.EmployeeRepository.EmployeeCurAddressEdit(param);
            return Ok(r);
        }

        /// <summary>
        /// Thông cá nhân
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> EmployeeContactInfoEdit([FromBody] EmployeeContactInfoDTO param)
        {
            if (param == null)
            {
                return Ok("Ko có dữ liệu");
            }
            var r = await _unitOfWork.EmployeeRepository.EmployeeContactInfoEdit(param);
            return Ok(r);
        }

        /// <summary>
        /// Họ chiếu
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> EmployeePassportEdit([FromBody] EmployeePassportDTO param)
        {
            if (param == null)
            {
                return Ok("Ko có dữ liệu");
            }
            var r = await _unitOfWork.EmployeeRepository.EmployeePassportEdit(param);
            return Ok(r);
        }
        /// <summary>
        /// Giấy phép lao động
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> EmployeeVisaEdit([FromBody] EmployeeVisaDTO param)
        {
            if (param == null)
            {
                return Ok("Ko có dữ liệu");
            }
            var r = await _unitOfWork.EmployeeRepository.EmployeeVisaEdit(param);
            return Ok(r);
        }

        /// <summary>
        /// Giấy phép lao động
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> EmployeeWorkPermitEdit([FromBody] EmployeeWorkPermitDTO param)
        {
            if (param == null)
            {
                return Ok("Ko có dữ liệu");
            }
            var r = await _unitOfWork.EmployeeRepository.EmployeeWorkPermitEdit(param);
            return Ok(r);
        }

        /// <summary>
        /// Chứng chỉ hành nghề
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> EmployeeCertificateEdit([FromBody] EmployeeCertificateDTO param)
        {
            if (param == null)
            {
                return Ok("Ko có dữ liệu");
            }
            var r = await _unitOfWork.EmployeeRepository.EmployeeCertificateEdit(param);
            return Ok(r);
        }

        /// <summary>
        /// Trình độ văn hóa
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> EmployeeEducationEdit([FromBody] EmployeeEducationDTO param)
        {
            if (param == null)
            {
                return Ok("Ko có dữ liệu");
            }
            var r = await _unitOfWork.EmployeeRepository.EmployeeEducationEdit(param);
            return Ok(r);
        }

        /// <summary>
        /// Tài khoản ngân hàng
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> EmployeeBankEdit([FromBody] EmployeeBankDTO param)
        {
            if (param == null)
            {
                return Ok("Ko có dữ liệu");
            }
            var r = await _unitOfWork.EmployeeRepository.EmployeeBankEdit(param);
            return Ok(r);
        }

        [HttpGet]
        public async Task<ActionResult> GetEmployeeFamily()
        {
            var r = await _unitOfWork.EmployeeRepository.GetEmployeeFamily();
            return Ok(r);
        }
    }
}
