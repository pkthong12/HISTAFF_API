using API.All.DbContexts;
using API.DTO;
using API.Main;
using CORE.AutoMapper;
using CORE.DTO;
using CORE.Enum;
using CORE.GenericUOW;
using CORE.StaticConstant;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace API.Controllers.HuDistrict
{
	[ApiExplorerSettings(GroupName = "054-PROFILE-HU_DISTRICT")]
	[ApiController]
	[HiStaffAuthorize]
	[Route("api/[controller]/[action]")]
	public class HuDistrictController : ControllerBase
	{
		private readonly GenericUnitOfWork _uow;
		private readonly IHuDistrictRepository _HuDistrictRepository;
		private readonly AppSettings _appSettings;
		private readonly FullDbContext _con;

		public HuDistrictController(
			FullDbContext dbContext,
			IOptions<AppSettings> options)
		{
			_uow = new GenericUnitOfWork(dbContext);
			_HuDistrictRepository = new HuDistrictRepository(dbContext, _uow);
			_appSettings = options.Value;
			_con = dbContext;
		}

		[HttpGet]
		public async Task<IActionResult> ReadAll()
		{
			var response = await _HuDistrictRepository.ReadAll();
			return Ok(response);
		}

		[HttpPost]
		public async Task<IActionResult> ReadAllByNumberKey(ReadAllByKeyLongRequest request)
		{
			var response = await _HuDistrictRepository.ReadAllByKey(request.Key, request.Value);
			return Ok(response);
		}

		[HttpPost]
		public async Task<IActionResult> ReadAllByStringKey(ReadAllByKeyStringRequest request)
		{
			var response = await _HuDistrictRepository.ReadAllByKey(request.Key, request.Value);
			return Ok(response);
		}

		[HttpPost]
		public async Task<IActionResult> QueryList(GenericQueryListDTO<HuDistrictDTO> request)
		{

			try
			{
				var response = await _HuDistrictRepository.SinglePhaseQueryList(request);

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
		public async Task<IActionResult> GetScalesProvince()
		{
			try
			{
				using (_con)
				{
					var query = await (from p in _con.HuProvinces.Where(x => x.IS_ACTIVE && x.IS_ACTIVE)
									   select new
									   {
										   Id = p.ID,
										   Name = p.NAME
									   }).ToListAsync();
					return Ok(new FormatedResponse()
					{
						InnerBody = query
					});
				}
			}
			catch (Exception ex)
			{
				return Ok(new FormatedResponse() { ErrorType = EnumErrorType.UNCATCHABLE, StatusCode = EnumStatusCode.StatusCode500, MessageCode = ex.Message });
			}
		}

		[HttpGet]
		public async Task<IActionResult> GetById(long id)
		{
			var response = await _HuDistrictRepository.GetById(id);
			return Ok(response);
		}

		[HttpGet]
		public async Task<IActionResult> GetByStringId(string id)
		{
			var response = await _HuDistrictRepository.GetById(id);
			return Ok(response);
		}

		[HttpPost]
		public async Task<IActionResult> Create([FromBody] HuDistrictDTO model)
		{
            try
            {
                var sid = Request.Sid(_appSettings);
                if (sid == null) return Unauthorized();
                model.IsActive = true;
                var response = await _HuDistrictRepository.Create(_uow, model, sid);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return Ok(new FormatedResponse() { MessageCode = ex.Message, ErrorType = EnumErrorType.UNCATCHABLE, StatusCode = EnumStatusCode.StatusCode500 });
            }
        }
		[HttpPost]
		public async Task<IActionResult> CreateRange(List<HuDistrictDTO> models)
		{
			var sid = Request.Sid(_appSettings);
			if (sid == null) return Unauthorized();
			var response = await _HuDistrictRepository.CreateRange(_uow, models, sid);
			return Ok(response);
		}

		[HttpPost]
		public async Task<IActionResult> Update(HuDistrictDTO model)
		{
			var sid = Request.Sid(_appSettings);
			if (sid == null) return Unauthorized();
			var response = await _HuDistrictRepository.Update(_uow, model, sid);
			return Ok(response);
		}

		[HttpPost]
		public async Task<IActionResult> UpdateRange(List<HuDistrictDTO> models)
		{
			var sid = Request.Sid(_appSettings);
			if (sid == null) return Unauthorized();
			var response = await _HuDistrictRepository.UpdateRange(_uow, models, sid);
			return Ok(response);
		}

		[HttpPost]
		public async Task<IActionResult> Delete(HuDistrictDTO model)
		{
			if (model.Id != null)
			{
				var response = await _HuDistrictRepository.Delete(_uow, (long)model.Id);
				return Ok(response);
			}
			else
			{
				return Ok(new FormatedResponse() { ErrorType = EnumErrorType.CATCHABLE, MessageCode = CommonMessageCode.DELETE_REQUEST_NULL_ID });
			}
		}

		[HttpPost]
		public async Task<IActionResult> DeleteIds(IdsRequest model)
		{
            var check = await _HuDistrictRepository.CheckActive(model.Ids);
            if (check.StatusCode == EnumStatusCode.StatusCode400)
            {
                return Ok(check);
            }
            if (check.StatusCode == EnumStatusCode.StatusCode200)
            {
                var response = await _HuDistrictRepository.DeleteIds(_uow, model.Ids);
                return Ok(response);
            }
            return Ok(check);
        }

		[HttpPost]
		public async Task<IActionResult> DeleteStringIds(StringIdsRequest model)
		{
			var response = await _HuDistrictRepository.DeleteIds(_uow, model.Ids);
			return Ok(response);
		}

		[HttpGet]
		public async Task<IActionResult> CreateNewCode()
		{
			var response = await _HuDistrictRepository.CreateNewCode();
			return Ok(response);
		}

        [HttpPost]
        public async Task<IActionResult> ToggleActiveIds(GenericToggleIsActiveDTO model)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _HuDistrictRepository.ToggleActiveIds(_uow, model.Ids, model.ValueToBind, sid);
            return Ok(response);
        }

    }
}

