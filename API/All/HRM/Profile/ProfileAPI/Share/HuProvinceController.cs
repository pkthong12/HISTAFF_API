using API.All.DbContexts;
using API.Entities;
using CORE.DTO;
using CORE.Enum;
using CORE.GenericUOW;
using CORE.StaticConstant;
using Microsoft.AspNetCore.Mvc;
using ProfileDAL.Repositories;
using ProfileDAL.ViewModels;

namespace ProfileAPI.List
{
    [HiStaffAuthorize]
    [ApiExplorerSettings(GroupName = "002-PROFILE-SHARE-PROVINCE")]
    [ApiController]

    [Route("api/[controller]/[action]")]
    public class HuProvinceController : BaseController1
    {
		private readonly ProfileDbContext _profileDbContext;
		private readonly GenericUnitOfWork _uow;
		private IGenericRepository<HU_PROVINCE, ProvinceDTO> _genericRepository;
		public HuProvinceController(IProfileUnitOfWork unitOfWork, ProfileDbContext profileDbContext) : base(unitOfWork)
        {
			_profileDbContext = profileDbContext;
			_uow = new GenericUnitOfWork(_profileDbContext);
			_genericRepository = _uow.GenericRepository<HU_PROVINCE, ProvinceDTO>();
		}


		[HttpPost]
		public async Task<IActionResult> QueryList(GenericQueryListDTO<ProvinceDTO> request)
		{
			try
			{
				var response = await _unitOfWork.ProvinceRepository.SinglePhaseQueryList(request);

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


		[HttpGet]
        public async Task<ActionResult> GetAll(ProvinceDTO param)
        {
            if (!ModelState.IsValid)
            {
                return ResponseValidation();
            }
            var r = await _unitOfWork.ProvinceRepository.GetAll(param);
            return Ok(r);

        }
        [HttpGet]
        public async Task<ActionResult> GetListProvince()
        {
            if (!ModelState.IsValid)
            {
                return ResponseValidation();
            }
            var r = await _unitOfWork.ProvinceRepository.GetListProvince();
            return Ok(r);

        }
        [HttpGet]
        public async Task<ActionResult> GetListDistrict(int ProvinceId)
        {
            if (!ModelState.IsValid)
            {
                return ResponseValidation();
            }
            var r = await _unitOfWork.ProvinceRepository.GetListDistrict(ProvinceId);
            return Ok(r);

        }
        [HttpGet]
        public async Task<ActionResult> GetListWard(int DistrictId)
        {
            if (!ModelState.IsValid)
            {
                return ResponseValidation();
            }
            var r = await _unitOfWork.ProvinceRepository.GetListWard(DistrictId);
            return Ok(r);

        }
		[HttpGet]
		public async Task<IActionResult> GetById(long id)
		{
			try
			{
				var r = await _unitOfWork.ProvinceRepository.GetById(id);
				return ResponseResult(r);
			}
			catch (Exception ex)
			{
				return Ok(new FormatedResponse() { MessageCode = ex.Message, ErrorType = EnumErrorType.UNCATCHABLE, StatusCode = EnumStatusCode.StatusCode500 });
			}
		}
		[HttpPost]
        public async Task<ActionResult> Add([FromBody] ProvinceInputDTO param)
        {
            if (!ModelState.IsValid)
            {
                return ResponseValidation();
            }

          
            var r = await _unitOfWork.ProvinceRepository.CreateAsync(param);
            return ResponseResult(r);

        }
        [HttpPost]
        public async Task<ActionResult> Update([FromBody] ProvinceInputDTO param)
        {

            if (!ModelState.IsValid)
            {
                return ResponseValidation();
            }
         

            var r = await _unitOfWork.ProvinceRepository.UpdateAsync(param);
            return ResponseResult(r);


        }

		[HttpPost]
		public async Task<IActionResult> DeleteIds(IdsRequest req)
		{
			try
			{
				if (req.Ids != null)
				{
					var response = await _genericRepository.DeleteIds(_uow, req.Ids);
					return Ok(response);
				}
				else
				{
					return Ok(new FormatedResponse() { ErrorType = EnumErrorType.CATCHABLE, MessageCode = CommonMessageCode.DELETE_REQUEST_NULL_ID });
				}
			}
			catch (Exception ex)
			{
				return Ok(new FormatedResponse() { ErrorType = EnumErrorType.UNCATCHABLE, StatusCode = EnumStatusCode.StatusCode500, MessageCode = ex.Message });
			}
		}

	}
}
