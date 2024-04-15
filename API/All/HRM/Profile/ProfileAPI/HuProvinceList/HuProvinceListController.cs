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

namespace API.Controllers.HuProvinceList
{
	[ApiExplorerSettings(GroupName = "079-PROFILE-HU_Province")]
	[ApiController]
	[HiStaffAuthorize]
	[Route("api/[controller]/[action]")]
	public class HuProvinceListController : ControllerBase
	{
		private readonly GenericUnitOfWork _uow;
		private readonly IHuProvinceRepository _HuProvinceRepository;
		private readonly AppSettings _appSettings;
		private readonly FullDbContext _con;
		public HuProvinceListController(
			FullDbContext dbContext,
			IOptions<AppSettings> options)
		{
			_uow = new GenericUnitOfWork(dbContext);
			_HuProvinceRepository = new HuProvinceRepository(dbContext, _uow);
			_appSettings = options.Value;
			_con = dbContext;
		}

		[HttpPost]
		public async Task<IActionResult> QueryList(GenericQueryListDTO<HuProvinceDTO> request)
		{

			try
			{
				var response = await _HuProvinceRepository.SinglePhaseQueryList(request);

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
		public async Task<IActionResult> GetById(long id)
		{
			var response = await _HuProvinceRepository.GetById(id);
			return Ok(response);
		}
		[HttpPost]
		public async Task<IActionResult> Create([FromBody] HuProvinceDTO model)
		{
            try
            {
                var sid = Request.Sid(_appSettings);
                if (sid == null) return Unauthorized();
                model.IsActive = true;
                var response = await _HuProvinceRepository.Create(_uow, model, sid);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return Ok(new FormatedResponse() { MessageCode = ex.Message, ErrorType = EnumErrorType.UNCATCHABLE, StatusCode = EnumStatusCode.StatusCode500 });
            }
        }

		[HttpPost]
		public async Task<IActionResult> Update(HuProvinceDTO model)
		{
			var sid = Request.Sid(_appSettings);
			if (sid == null) return Unauthorized();
			var response = await _HuProvinceRepository.Update(_uow, model, sid);
			return Ok(response);
		}

		[HttpPost]
		public async Task<IActionResult> DeleteIds(IdsRequest model)
		{
			var check = await _HuProvinceRepository.CheckActive(model.Ids);
            if (check.StatusCode == EnumStatusCode.StatusCode400)
            {
                return Ok(check);
            }
            if(check.StatusCode == EnumStatusCode.StatusCode200)
            {
                var response = await _HuProvinceRepository.DeleteIds(_uow, model.Ids);
                return Ok(response);
            }
			return Ok(check);
        }
		[HttpGet]
		public async Task<IActionResult> CreateNewCode()
		{
			var response = await _HuProvinceRepository.CreateNewCode();
			return Ok(response);
		}

		[HttpGet]
		public async Task<IActionResult> GetScalesNation()
		{
			try
			{
				using (_con)
				{
					var query = await (from p in _con.HuNations.Where(x => x.IS_ACTIVE!.Value && x.IS_ACTIVE.Value)
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
		public async Task<IActionResult> GetScalesProvince(string naId)
		{
			try
			{
				var query = await (from p in _con.HuProvinces.Where(x => x.IS_ACTIVE && x.IS_ACTIVE && x.NATION_ID == long.Parse(naId))
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
			catch (Exception ex)
			{
				return Ok(new FormatedResponse() { ErrorType = EnumErrorType.UNCATCHABLE, StatusCode = EnumStatusCode.StatusCode500, MessageCode = ex.Message });
			}
		}
        [HttpPost]
        public async Task<IActionResult> ToggleActiveIds(GenericToggleIsActiveDTO model)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _HuProvinceRepository.ToggleActiveIds(_uow, model.Ids, model.ValueToBind, sid);
            return Ok(new FormatedResponse()
            {
                MessageCode = response.MessageCode,
                InnerBody = response.InnerBody,
                StatusCode = EnumStatusCode.StatusCode200
            });
        }

        //// cập nhật trạng thái
        //// bấm vào button áp dụng
        //[HttpPost]
        //public async Task<IActionResult> SubmitActivate(List<long>? Ids)
        //{
        //    var response = await _HuProvinceRepository.SubmitActivate(Ids);

        //    return Ok(response);
        //}


        //// cập nhật trạng thái
        //// bấm vào button ngừng áp dụng
        //[HttpPost]
        //public async Task<IActionResult> SubmitStopActivate(List<long>? Ids)
        //{
        //    var response = await _HuProvinceRepository.SubmitStopActivate(Ids);

        //    return Ok(response);
        //}
    }
}
