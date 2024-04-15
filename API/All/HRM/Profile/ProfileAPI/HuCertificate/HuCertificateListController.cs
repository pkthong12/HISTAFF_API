using API.All.DbContexts;
using API.DTO;
using API.Main;
using CORE.DTO;
using CORE.Enum;
using CORE.GenericUOW;
using CORE.Services.File;
using CORE.StaticConstant;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace API.Controllers.HuCertificate
{
	[ApiExplorerSettings(GroupName = "047-PROFILE-HU_CERTIFICATE")]
	[ApiController]
	[HiStaffAuthorize]
	[Route("api/[controller]/[action]")]
	public class HuCertificateListController : ControllerBase
	{
		private readonly GenericUnitOfWork _uow;
		private readonly IHuCertificateRepository _HuCertificateRepository;
		private readonly AppSettings _appSettings;

		public HuCertificateListController(
			FullDbContext dbContext,
			IOptions<AppSettings> options,
						IWebHostEnvironment env,
			IFileService fileService)
		{
			_uow = new GenericUnitOfWork(dbContext);
			_HuCertificateRepository = new HuCertificateRepository(dbContext, _uow, env, options, fileService);
			_appSettings = options.Value;
		}

		[HttpGet]
		public async Task<IActionResult> ReadAll()
		{
			var response = await _HuCertificateRepository.ReadAll();
			return Ok(response);
		}

		[HttpPost]
		public async Task<IActionResult> ReadAllByNumberKey(ReadAllByKeyLongRequest request)
		{
			var response = await _HuCertificateRepository.ReadAllByKey(request.Key, request.Value);
			return Ok(response);
		}

		[HttpPost]
		public async Task<IActionResult> ReadAllByStringKey(ReadAllByKeyStringRequest request)
		{
			var response = await _HuCertificateRepository.ReadAllByKey(request.Key, request.Value);
			return Ok(response);
		}

		[HttpPost]
		public async Task<IActionResult> QueryList(GenericQueryListDTO<HuCertificateDTO> request)
		{
			try
			{
				var response = await _HuCertificateRepository.SinglePhaseQueryList(request);

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
			var response = await _HuCertificateRepository.GetById(id);
			return Ok(response);
		}

		[HttpGet]
		public async Task<IActionResult> GetByStringId(string id)
		{
			var response = await _HuCertificateRepository.GetById(id);
			return Ok(response);
		}

		[HttpPost]
		public async Task<IActionResult> Create(HuCertificateDTO model)
		{
			if (model.TrainFromDate  > model.TrainToDate)
			{
				return Ok(new FormatedResponse() { MessageCode = CommonMessageCode.EFFECTIVEDATE_NOT_BIGGER_THAN_EXPIRATIONDATE, ErrorType = EnumErrorType.UNCATCHABLE, StatusCode = EnumStatusCode.StatusCode500 });
			}
            if (model.EffectFrom > model.EffectTo)
            {
                return Ok(new FormatedResponse() { MessageCode = CommonMessageCode.EFFECTIVEDATE_NOT_BIGGER_THAN_EXPIRATIONDATE, ErrorType = EnumErrorType.UNCATCHABLE, StatusCode = EnumStatusCode.StatusCode500 });
            }
            var sid = Request.Sid(_appSettings);
			if (sid == null) return Unauthorized();
			var response = await _HuCertificateRepository.Create(_uow, model, sid);
			return Ok(response);
		}
		[HttpPost]
		public async Task<IActionResult> CreateRange(List<HuCertificateDTO> models)
		{
			var sid = Request.Sid(_appSettings);
			if (sid == null) return Unauthorized();
			var response = await _HuCertificateRepository.CreateRange(_uow, models, sid);
			return Ok(response);
		}

		[HttpPost]
		public async Task<IActionResult> Update(HuCertificateDTO model)
		{
			if (model.TrainFromDate > model.TrainToDate)
			{
				return Ok(new FormatedResponse() { MessageCode = CommonMessageCode.EFFECTIVEDATE_NOT_BIGGER_THAN_EXPIRATIONDATE, ErrorType = EnumErrorType.UNCATCHABLE, StatusCode = EnumStatusCode.StatusCode500 });
			}
			var sid = Request.Sid(_appSettings);
			if (sid == null) return Unauthorized();
			var response = await _HuCertificateRepository.Update(_uow, model, sid);
			return Ok(response);
		}

		[HttpPost]
		public async Task<IActionResult> UpdateRange(List<HuCertificateDTO> models)
		{
			var sid = Request.Sid(_appSettings);
			if (sid == null) return Unauthorized();
			var response = await _HuCertificateRepository.UpdateRange(_uow, models, sid);
			return Ok(response);
		}

		[HttpPost]
		public async Task<IActionResult> Delete(HuCertificateDTO model)
		{
			if (model.Id != null)
			{
				var response = await _HuCertificateRepository.Delete(_uow, (long)model.Id);
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
			var response = await _HuCertificateRepository.DeleteIds(_uow, model.Ids);
			return Ok(response);
		}

		[HttpPost]
		public async Task<IActionResult> DeleteStringIds(StringIdsRequest model)
		{
			var response = await _HuCertificateRepository.DeleteIds(_uow, model.Ids);
			return Ok(response);
		}


        [HttpGet]
        public async Task<IActionResult> GetCertificateByEmployee(long id)
        {
            try
            {
                var entity = _uow.Context.Set<HU_CERTIFICATE>().AsNoTracking().AsQueryable();
                var employees = _uow.Context.Set<HU_EMPLOYEE>().AsNoTracking().AsQueryable();
                var employeeCvs = _uow.Context.Set<HU_EMPLOYEE_CV>().AsNoTracking().AsQueryable();
                var positions = _uow.Context.Set<HU_POSITION>().AsNoTracking().AsQueryable();
                var organizations = _uow.Context.Set<HU_ORGANIZATION>().AsNoTracking().AsQueryable();
                var otherLists = _uow.Context.Set<SYS_OTHER_LIST>().AsNoTracking().AsQueryable();
                var joined = await (from p in entity
                                    from e in employees.Where(x => x.ID == p.EMPLOYEE_ID).DefaultIfEmpty()
                                    from cv in employeeCvs .Where(x => x.ID == e.PROFILE_ID).DefaultIfEmpty()
                                    from t in positions.Where(x => x.ID == e.POSITION_ID).DefaultIfEmpty()
                                    from o in organizations.Where(x => x.ID == e.ORG_ID).DefaultIfEmpty()
                                    from type in otherLists.Where(x => x.ID == p.TYPE_CERTIFICATE).DefaultIfEmpty()
                                    from lvtrain in otherLists.Where(x => x.ID == p.LEVEL_TRAIN).DefaultIfEmpty()
                                    from typeTrain in otherLists.Where(x => x.ID == p.TYPE_TRAIN).DefaultIfEmpty()
                                    from school in otherLists.Where(x => x.ID == p.SCHOOL_ID).DefaultIfEmpty()
                                    from lv in otherLists.Where(x => x.ID == p.LEVEL_ID).DefaultIfEmpty()
                                    where p.EMPLOYEE_ID == id
                                    select new HuCertificateDTO
                                    {
                                        Id = p.ID,
                                        EmployeeCode = e.CODE,
                                        EmployeeFullName = cv.FULL_NAME,
                                        OrgName = o.NAME,
                                        TitleName = t.NAME,
                                        TypeCertificateName = type.NAME,
                                        Name = p.NAME,
                                        TrainFromDate = p.TRAIN_FROM_DATE,
                                        TrainToDate = p.TRAIN_TO_DATE,
                                        EffectFrom = p.EFFECT_FROM,
                                        LevelTrainName = lvtrain.NAME,
                                        Major = p.MAJOR,
                                        ContentTrain = p.CONTENT_TRAIN,
                                        SchoolName = school.NAME,
                                        Year = p.YEAR,
                                        Mark = p.MARK,
                                        IsPrime = p.IS_PRIME,
                                        Level = p.LEVEL,
                                        TypeTrainName = typeTrain.NAME,
                                        Remark = p.REMARK,
                                        LevelId = p.LEVEL_ID,
                                        LevelName = lv.NAME,
                                        OrgId = e.ORG_ID,
                                        IsPrimeStr = p.IS_PRIME == true ? "Là bằng chính" : "Không là bằng chính",
										Classification= p.CLASSIFICATION
                                    }).ToListAsync();
                var response = new FormatedResponse() { InnerBody = joined };
                return Ok(response);
            }
            catch (Exception ex)
            {
                return Ok(new FormatedResponse() { ErrorType = EnumErrorType.UNCATCHABLE, StatusCode = EnumStatusCode.StatusCode500, MessageCode = ex.Message });
            }
        }


        [HttpGet]
        public async Task<IActionResult> GetIdOfTypeCertificateByCode(string code)
        {
            try
            {
                var otherLists = _uow.Context.Set<SYS_OTHER_LIST>().AsNoTracking().AsQueryable();
                
				var joined = await (from item in otherLists.Where(x => x.CODE.ToUpper() == code.ToUpper()).DefaultIfEmpty()
                                    select new SysOtherListDTO
                                    {
                                        Id = item.ID,
										Code = item.CODE,
										Name = item.NAME
                                    }).FirstAsync();
                var response = new FormatedResponse() { InnerBody = joined };
                return Ok(response);
            }
            catch (Exception ex)
            {
                return Ok(new FormatedResponse() { ErrorType = EnumErrorType.UNCATCHABLE, StatusCode = EnumStatusCode.StatusCode500, MessageCode = ex.Message });
            }
        }
    }
}

