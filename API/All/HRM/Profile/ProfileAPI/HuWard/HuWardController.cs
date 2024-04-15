using API.All.DbContexts;
using API.DTO;
using API.Main;
using CORE.DTO;
using CORE.Enum;
using CORE.GenericUOW;
using CORE.StaticConstant;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Drawing;

namespace API.Controllers.HuWard
{
    [ApiExplorerSettings(GroupName = "083-PROFILE-HU_WARD")]
    [ApiController]
    [HiStaffAuthorize]
    [Route("api/[controller]/[action]")]
    public class HuWardController : ControllerBase
    {
        private readonly GenericUnitOfWork _uow;
        private readonly IHuWardRepository _HuWardRepository;
        private readonly AppSettings _appSettings;
		private readonly FullDbContext _FullDbContext;

		public HuWardController(
            FullDbContext dbContext,
            IOptions<AppSettings> options)
        {
            _uow = new GenericUnitOfWork(dbContext);
            _HuWardRepository = new HuWardRepository(dbContext, _uow);
            _appSettings = options.Value;
            _FullDbContext = dbContext;

		}

        [HttpGet]
        public async Task<IActionResult> ReadAll()
        {
            var response = await _HuWardRepository.ReadAll();
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> ReadAllByNumberKey(ReadAllByKeyLongRequest request)
        {
            var response = await _HuWardRepository.ReadAllByKey(request.Key, request.Value);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> ReadAllByStringKey(ReadAllByKeyStringRequest request)
        {
            var response = await _HuWardRepository.ReadAllByKey(request.Key, request.Value);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> QueryList(GenericQueryListDTO<HuWardDTO> request)
        {

			try
			{
				var response = await _HuWardRepository.SinglePhaseQueryList(request);

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
            var response = await _HuWardRepository.GetById(id);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetByStringId(string id)
        {
            var response = await _HuWardRepository.GetById(id);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Create(HuWardDTO model)
        {
            try
            {
                var sid = Request.Sid(_appSettings);
                if (sid == null) return Unauthorized();
                model.IsActive = true;
                var response = await _HuWardRepository.Create(_uow, model, sid);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return Ok(new FormatedResponse() { MessageCode = ex.Message, ErrorType = EnumErrorType.UNCATCHABLE, StatusCode = EnumStatusCode.StatusCode500 });
            }
        }
        [HttpPost]
        public async Task<IActionResult> CreateRange(List<HuWardDTO> models)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _HuWardRepository.CreateRange(_uow, models, sid);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Update(HuWardDTO model)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _HuWardRepository.Update(_uow, model, sid);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateRange(List<HuWardDTO> models)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _HuWardRepository.UpdateRange(_uow, models, sid);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(HuWardDTO model)
        {
            if (model.Id != null)
            {
                var response = await _HuWardRepository.Delete(_uow, (long)model.Id);
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
            var check = await _HuWardRepository.CheckActive(model.Ids);
            if (check.StatusCode == EnumStatusCode.StatusCode400)
            {
                return Ok(check);
            }
            if (check.StatusCode == EnumStatusCode.StatusCode200)
            {
                var response = await _HuWardRepository.DeleteIds(_uow, model.Ids);
                return Ok(response);
            }
            return Ok(check);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteStringIds(StringIdsRequest model)
        {
            var response = await _HuWardRepository.DeleteIds(_uow, model.Ids);
            return Ok(response);
        }

		[HttpGet]
		public async Task<IActionResult> CreateNewCode()
		{
			var response = await _HuWardRepository.CreateNewCode();
			return Ok(response);    
		}

		[HttpGet]
		public async Task<IActionResult> GetScalesDistrict(long disId)
		{
			try
			{
				var query = await (from p in _FullDbContext.HuDistricts.Where(x => x.IS_ACTIVE && x.IS_ACTIVE && x.PROVINCE_ID == disId)
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

        [HttpGet]
        public async Task<IActionResult> GetScalesProvince(long Id)
        {
            try
            {
                var query = await (from p in _FullDbContext.HuProvinces.Where(x => x.IS_ACTIVE && x.IS_ACTIVE && x.NATION_ID == Id)
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
            var response = await _HuWardRepository.ToggleActiveIds(_uow, model.Ids, model.ValueToBind, sid);
            return Ok(response);
        }

        //// cập nhật trạng thái
        //// bấm vào button áp dụng
        //[HttpPost]
        //public async Task<IActionResult> SubmitActivate(List<long>? Ids)
        //{
        //    var response = await _HuWardRepository.SubmitActivate(Ids);

        //    return Ok(response);
        //}


        //// cập nhật trạng thái
        //// bấm vào button ngừng áp dụng
        //[HttpPost]
        //public async Task<IActionResult> SubmitStopActivate(List<long>? Ids)
        //{
        //    var response = await _HuWardRepository.SubmitStopActivate(Ids);

        //    return Ok(response);
        //}
    }
}

