using API;
using API.All.DbContexts;
using API.Controllers.DemoAttachment;
using API.Controllers.Family;
using API.DTO;
using API.Main;
using Common.Extensions;
using CORE.DTO;
using CORE.Enum;
using CORE.GenericUOW;
using CORE.Services.File;
using CORE.StaticConstant;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Information;

namespace ProfileAPI.List
{
    [ApiController]
    [ApiExplorerSettings(GroupName = "002-PROFILE-FAMILY")]
    [HiStaffAuthorize]
    [Route("api/[controller]/[action]")]
    public class HuFamilyController : ControllerBase
    {
        private readonly GenericUnitOfWork _uow;
        private readonly GenericReducer<HU_FAMILY, HuFamilyDTO> genericReducer;
        private IGenericRepository<HU_FAMILY, HuFamilyDTO> _genericRepository;
        private readonly IFamilyRepository _FamilyRepository;
        private AppSettings _appSettings;
        public HuFamilyController(CoreDbContext coreDbContext, IOptions<AppSettings> options, IWebHostEnvironment env, IFileService fileService)
        {
            _uow = new GenericUnitOfWork(coreDbContext);
            genericReducer = new();
            _genericRepository = _uow.GenericRepository<HU_FAMILY, HuFamilyDTO>();
            _appSettings = options.Value;
            _FamilyRepository = new FamilyRepository(coreDbContext, _uow, env, options, fileService);
        }
        [HttpPost]
        public async Task<IActionResult> QueryList(GenericQueryListDTO<HuFamilyDTO> request)
        {

            try
            {
                var entity = _uow.Context.Set<HU_FAMILY>().AsNoTracking().AsQueryable();
                var employees = _uow.Context.Set<HU_EMPLOYEE>().AsNoTracking().AsQueryable();
                var positions = _uow.Context.Set<HU_POSITION>().AsNoTracking().AsQueryable();
                var organizations = _uow.Context.Set<HU_ORGANIZATION>().AsNoTracking().AsQueryable();
                var otherLists = _uow.Context.Set<SYS_OTHER_LIST>().AsNoTracking().AsQueryable();
                var provinces = _uow.Context.Set<HU_PROVINCE>().AsNoTracking().AsQueryable();
                var nations = _uow.Context.Set<HU_NATION>().AsNoTracking().AsQueryable();
                var districts = _uow.Context.Set<HU_DISTRICT>().AsNoTracking().AsQueryable();
                var wards = _uow.Context.Set<HU_WARD>().AsNoTracking().AsQueryable();
                var jobs = _uow.Context.Set<HU_JOB>().AsQueryable();
                var joined = from p in entity
                             from e in employees.Where(x => x.ID == p.EMPLOYEE_ID).DefaultIfEmpty()
                             from t in positions.Where(x => x.ID == e.POSITION_ID).DefaultIfEmpty()
                             from o in organizations.Where(x => x.ID == e.ORG_ID).DefaultIfEmpty()
                             from j in jobs.Where(x => x.ID == t.JOB_ID).DefaultIfEmpty()
                             from r in otherLists.Where(x => x.ID == p.RELATIONSHIP_ID).DefaultIfEmpty()
                             from g in otherLists.Where(x => x.ID == p.GENDER).DefaultIfEmpty()
                             from n in nations.Where(x => x.ID == p.NATIONALITY).DefaultIfEmpty()
                             from pr in provinces.Where(x => x.ID == p.BIRTH_CER_PROVINCE).DefaultIfEmpty()
                             from d in districts.Where(x => x.ID == p.BIRTH_CER_DISTRICT).DefaultIfEmpty()
                             from w in wards.Where(x => x.ID == p.BIRTH_CER_WARD).DefaultIfEmpty()
                             from s in otherLists.Where(x => x.ID == p.STATUS_ID).DefaultIfEmpty()
                             orderby p.CREATED_DATE descending
                             select new HuFamilyDTO
                             {
                                 Id = p.ID,
                                 EmployeeId = p.EMPLOYEE_ID,
                                 EmployeeName = e.Profile.FULL_NAME,
                                 EmployeeCode = e.CODE,
                                 PositionName = t.NAME,
                                 OrgId = e.ORG_ID,
                                 OrgName = o.NAME,
                                 RelationshipId = p.RELATIONSHIP_ID,
                                 RelationshipName = r.NAME,
                                 Fullname = p.FULLNAME,
                                 Gender = p.GENDER,
                                 GenderName = g.NAME,
                                 BirthDate = p.BIRTH_DATE,
                                 PitCode = p.PIT_CODE,
                                 SameCompany = p.SAME_COMPANY,
                                 IsDead = p.IS_DEAD,
                                 IsDeduct = p.IS_DEDUCT,
                                 DeductFrom = p.DEDUCT_FROM,
                                 DeductTo = p.DEDUCT_TO,
                                 RegistDeductDate = p.REGIST_DEDUCT_DATE,
                                 IsHousehold = p.IS_HOUSEHOLD,
                                 IdNo = p.ID_NO,
                                 Career = p.CAREER,
                                 Nationality = p.NATIONALITY,
                                 NationalityName = n.NAME,
                                 BirthCerPlace = p.BIRTH_CER_PLACE,
                                 BirthCerProvince = p.BIRTH_CER_PROVINCE,
                                 BirthCerProvinceName = pr.NAME,
                                 BirthCerDistrict = p.BIRTH_CER_DISTRICT,
                                 BirthCerDistrictName = d.NAME,
                                 BirthCerWard = p.BIRTH_CER_WARD,
                                 BirthCerWardName = w.NAME,
                                 UploadFile = p.UPLOAD_FILE,
                                 StatusId = e.WORK_STATUS_ID,
                                 StatusName = s.NAME,
                                 CreatedBy = p.CREATED_BY,
                                 UpdatedBy = p.UPDATED_BY,
                                 CreatedDate = p.CREATED_DATE,
                                 UpdatedDate = p.UPDATED_DATE,
                                 Note = p.NOTE,
                                 JobOrderNum = (int)(j.ORDERNUM ?? 99)
                             };
                var response = await genericReducer.SinglePhaseReduce(joined, request);
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
        public async Task<IActionResult> DeleteIds(IdsRequest model)
        {
            try
            {
                if (model.Ids != null)
                {
                    var response = await _genericRepository.DeleteIds(_uow, model.Ids);
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

        [HttpPost]
        public async Task<IActionResult> Delete(HuFamilyDTO model)
        {
            try
            {
                if (model.Id != null)
                {
                    var response = await _genericRepository.Delete(_uow, (long)model.Id);
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


        [HttpGet]
        public async Task<IActionResult> GetFamilyByEmployee(long id)
        {
            try
            {
                var entity = _uow.Context.Set<HU_FAMILY>().AsNoTracking().AsQueryable();
                var employees = _uow.Context.Set<HU_EMPLOYEE>().AsNoTracking().AsQueryable();
                var positions = _uow.Context.Set<HU_POSITION>().AsNoTracking().AsQueryable();
                var organizations = _uow.Context.Set<HU_ORGANIZATION>().AsNoTracking().AsQueryable();
                var otherLists = _uow.Context.Set<SYS_OTHER_LIST>().AsNoTracking().AsQueryable();
                var provinces = _uow.Context.Set<HU_PROVINCE>().AsNoTracking().AsQueryable();
                var nations = _uow.Context.Set<HU_NATION>().AsNoTracking().AsQueryable();
                var districts = _uow.Context.Set<HU_DISTRICT>().AsNoTracking().AsQueryable();
                var wards = _uow.Context.Set<HU_WARD>().AsNoTracking().AsQueryable();
                var joined = await (from p in entity
                                  from e in employees.Where(x => x.ID == p.EMPLOYEE_ID).DefaultIfEmpty()
                                  from t in positions.Where(x => x.ID == e.POSITION_ID).DefaultIfEmpty()
                                  from o in organizations.Where(x => x.ID == e.ORG_ID).DefaultIfEmpty()
                                  from r in otherLists.Where(x => x.ID == p.RELATIONSHIP_ID).DefaultIfEmpty()
                                  from g in otherLists.Where(x => x.ID == p.GENDER).DefaultIfEmpty()
                                  from n in nations.Where(x => x.ID == p.NATIONALITY).DefaultIfEmpty()
                                  from pr in provinces.Where(x => x.ID == p.BIRTH_CER_PROVINCE).DefaultIfEmpty()
                                  from d in districts.Where(x => x.ID == p.BIRTH_CER_DISTRICT).DefaultIfEmpty()
                                  from w in wards.Where(x => x.ID == p.BIRTH_CER_WARD).DefaultIfEmpty()
                                  from s in otherLists.Where(x => x.ID == p.STATUS_ID).DefaultIfEmpty()
                                  where e.ID == id
                                  select new HuFamilyDTO
                                  {
                                      Id = p.ID,
                                      EmployeeId = e.ID,
                                      EmployeeName = e.Profile.FULL_NAME,
                                      EmployeeCode = e.CODE,
                                      PositionName = t.NAME,
                                      OrgName = o.NAME,
                                      RelationshipId = p.RELATIONSHIP_ID,
                                      RelationshipName = r.NAME,
                                      Fullname = p.FULLNAME,
                                      Gender = p.GENDER,
                                      GenderName = g.NAME,
                                      BirthDate = p.BIRTH_DATE,
                                      PitCode = p.PIT_CODE,
                                      SameCompany = p.SAME_COMPANY,
                                      IsDead = p.IS_DEAD,
                                      IsDeduct = p.IS_DEDUCT,
                                      DeductFrom = p.DEDUCT_FROM,
                                      DeductTo = p.DEDUCT_TO,
                                      RegistDeductDate = p.REGIST_DEDUCT_DATE,
                                      IsHousehold = p.IS_HOUSEHOLD,
                                      IdNo = p.ID_NO,
                                      Career = p.CAREER,
                                      Nationality = p.NATIONALITY,
                                      NationalityName = n.NAME,
                                      BirthCerPlace = p.BIRTH_CER_PLACE,
                                      BirthCerProvince = p.BIRTH_CER_PROVINCE,
                                      BirthCerProvinceName = pr.NAME,
                                      BirthCerDistrict = p.BIRTH_CER_DISTRICT,
                                      BirthCerDistrictName = d.NAME,
                                      BirthCerWard = p.BIRTH_CER_WARD,
                                      BirthCerWardName = w.NAME,
                                      UploadFile = p.UPLOAD_FILE,
                                      StatusId = p.STATUS_ID,
                                      StatusName = s.NAME,
                                      CreatedBy = p.CREATED_BY,
                                      UpdatedBy = p.UPDATED_BY,
                                      CreatedDate = p.CREATED_DATE,
                                      UpdatedDate = p.UPDATED_DATE,
                                      Note = p.NOTE
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
        public async Task<IActionResult> GetById(long id)
        {
            try
            {
                var entity = _uow.Context.Set<HU_FAMILY>().AsNoTracking().AsQueryable();
                var employees = _uow.Context.Set<HU_EMPLOYEE>().AsNoTracking().AsQueryable();
                var positions = _uow.Context.Set<HU_POSITION>().AsNoTracking().AsQueryable();
                var organizations = _uow.Context.Set<HU_ORGANIZATION>().AsNoTracking().AsQueryable();
                var otherLists = _uow.Context.Set<SYS_OTHER_LIST>().AsNoTracking().AsQueryable();
                var nations = _uow.Context.Set<HU_NATION>().AsNoTracking().AsQueryable();
                var provinces = _uow.Context.Set<HU_PROVINCE>().AsNoTracking().AsQueryable();
                var districts = _uow.Context.Set<HU_DISTRICT>().AsNoTracking().AsQueryable();
                var wards = _uow.Context.Set<HU_WARD>().AsNoTracking().AsQueryable();
                var joined = await (from p in entity
                             from e in employees.Where(x => x.ID == p.EMPLOYEE_ID).DefaultIfEmpty()
                             from t in positions.Where(x => x.ID == e.POSITION_ID).DefaultIfEmpty()
                             from o in organizations.Where(x => x.ID == e.ORG_ID).DefaultIfEmpty()
                             from r in otherLists.Where(x => x.ID == p.RELATIONSHIP_ID).DefaultIfEmpty()
                             from g in otherLists.Where(x => x.ID == p.GENDER).DefaultIfEmpty()
                             from n in nations.Where(x => x.ID == p.NATIONALITY).DefaultIfEmpty()
                             from pr in provinces.Where(x => x.ID == p.BIRTH_CER_PROVINCE).DefaultIfEmpty()
                             from d in districts.Where(x => x.ID == p.BIRTH_CER_DISTRICT).DefaultIfEmpty()
                             from w in wards.Where(x => x.ID == p.BIRTH_CER_WARD).DefaultIfEmpty()
                             from s in otherLists.Where(x => x.ID == p.STATUS_ID).DefaultIfEmpty()
                             where p.ID == id
                             select new 
                             {
                                 Id = p.ID,
                                 EmployeeId = e.ID,
                                 EmployeeName = e.Profile.FULL_NAME,
                                 EmployeeCode = e.CODE,
                                 PositionName = t.NAME,
                                 OrgName = o.NAME,
                                 RelationshipId = p.RELATIONSHIP_ID,
                                 RelationshipName = r.NAME,
                                 Fullname = p.FULLNAME,
                                 Gender = p.GENDER,
                                 GenderName = g.NAME,
                                 BirthDate = p.BIRTH_DATE,
                                 PitCode = p.PIT_CODE,
                                 SameCompany = p.SAME_COMPANY,
                                 IsDead = p.IS_DEAD,
                                 IsDeduct = p.IS_DEDUCT,
                                 DeductFrom = (p.DEDUCT_FROM != null) ? p.DEDUCT_FROM : DateTime.Now,
                                 DeductTo = p.DEDUCT_TO,
                                 RegistDeductDate = (p.REGIST_DEDUCT_DATE != null) ? p.REGIST_DEDUCT_DATE : DateTime.Now,
                                 IsHousehold = p.IS_HOUSEHOLD,
                                 IdNo = p.ID_NO,
                                 Career = p.CAREER,
                                 Nationality = p.NATIONALITY,
                                 NationalityName = n.NAME,
                                 BirthCerPlace = p.BIRTH_CER_PLACE,
                                 BirthCerProvince = p.BIRTH_CER_PROVINCE,
                                 BirthCerProvinceName = pr.NAME,
                                 BirthCerDistrict = p.BIRTH_CER_DISTRICT,
                                 BirthCerDistrictName = d.NAME,
                                 BirthCerWard = p.BIRTH_CER_WARD,
                                 BirthCerWardName = w.NAME,
                                 UploadFile = p.UPLOAD_FILE,
                                 StatusId = p.STATUS_ID,
                                 StatusName = s.NAME,
                                 CreatedBy = p.CREATED_BY,
                                 UpdatedBy = p.UPDATED_BY,
                                 CreatedDate = p.CREATED_DATE,
                                 UpdatedDate = p.UPDATED_DATE,
                                 Note = p.NOTE
                             }).FirstOrDefaultAsync();
                var response = new FormatedResponse() { InnerBody = joined };
                return Ok(response);
            }
            catch (Exception ex)
            {
                return Ok(new FormatedResponse() { ErrorType = EnumErrorType.UNCATCHABLE, StatusCode = EnumStatusCode.StatusCode500, MessageCode = ex.Message });
            }
        }
        [HttpGet]
        public async Task<IActionResult> GetNationality()
        {
            try
            {
                var entity = _uow.Context.Set<HU_NATION>().AsNoTracking().AsQueryable();
                var joined = from p in entity
                             where p.IS_ACTIVE == true
                             select new
                             {
                                 Id = p.ID,
                                 Name = p.NAME
                             };
                var response = new FormatedResponse() { InnerBody = joined };
                return Ok(response);
            }
            catch (Exception ex)
            {
                return Ok(new FormatedResponse() { ErrorType = EnumErrorType.UNCATCHABLE, StatusCode = EnumStatusCode.StatusCode500, MessageCode = ex.Message });
            }
        }
        [HttpGet]
        public async Task<IActionResult> GetProvince()
        {
            try
            {
                var entity = _uow.Context.Set<HU_PROVINCE>().AsNoTracking().AsQueryable();
                var joined = from p in entity
                             select new
                             {
                                 Id = p.ID,
                                 Name = p.NAME
                             };
                var response = new FormatedResponse() { InnerBody = joined };
                return Ok(response);
            }
            catch (Exception ex)
            {
                return Ok(new FormatedResponse() { ErrorType = EnumErrorType.UNCATCHABLE, StatusCode = EnumStatusCode.StatusCode500, MessageCode = ex.Message });
            }
        }
        [HttpGet]
        public async Task<IActionResult> GetDistrictByProvince(decimal id)
        {
            try
            {
                var entity = _uow.Context.Set<HU_DISTRICT>().AsNoTracking().AsQueryable();
                var joined = from p in entity
                             where p.PROVINCE_ID == id
                             select new
                             {
                                 Id = p.ID,
                                 Name = p.NAME
                             };
                var response = new FormatedResponse() { InnerBody = joined };
                return Ok(response);
            }
            catch (Exception ex)
            {
                return Ok(new FormatedResponse() { ErrorType = EnumErrorType.UNCATCHABLE, StatusCode = EnumStatusCode.StatusCode500, MessageCode = ex.Message });
            }
        }
        [HttpGet]
        public async Task<IActionResult> GetWardByDistrict(decimal id)
        {
            try
            {
                var entity = _uow.Context.Set<HU_WARD>().AsNoTracking().AsQueryable();
                var joined = from p in entity
                             where p.DISTRICT_ID == id
                             select new
                             {
                                 Id = p.ID,
                                 Name = p.NAME
                             };
                var response = new FormatedResponse() { InnerBody = joined };
                return Ok(response);
            }
            catch (Exception ex)
            {
                return Ok(new FormatedResponse() { ErrorType = EnumErrorType.UNCATCHABLE, StatusCode = EnumStatusCode.StatusCode500, MessageCode = ex.Message });
            }
        }


        [HttpGet]
        public async Task<IActionResult> GetWardById(decimal id)
        {
            try
            {
                var entity = _uow.Context.Set<HU_WARD>().AsNoTracking().AsQueryable();
                var joined = await (from p in entity
                                    where p.ID == id
                                    select new
                                    {
                                        Id = p.ID,
                                        Name = p.NAME
                                    }).FirstOrDefaultAsync();
                var response = new FormatedResponse() { InnerBody = joined };
                return Ok(response);
            }
            catch (Exception ex)
            {
                return Ok(new FormatedResponse() { ErrorType = EnumErrorType.UNCATCHABLE, StatusCode = EnumStatusCode.StatusCode500, MessageCode = ex.Message });
            }
        }
        [HttpGet]
        public async Task<IActionResult> GetDistrictById(decimal id)
        {
            try
            {
                var entity = _uow.Context.Set<HU_DISTRICT>().AsNoTracking().AsQueryable();
                var joined = await (from p in entity
                                    where p.ID == id
                                    select new
                                    {
                                        Id = p.ID,
                                        Name = p.NAME
                                    }).FirstOrDefaultAsync();
                var response = new FormatedResponse() { InnerBody = joined };
                return Ok(response);
            }
            catch (Exception ex)
            {
                return Ok(new FormatedResponse() { ErrorType = EnumErrorType.UNCATCHABLE, StatusCode = EnumStatusCode.StatusCode500, MessageCode = ex.Message });
            }
        }
        [HttpGet]
        public async Task<IActionResult> GetProvinceById(decimal id)
        {
            try
            {
                var entity = _uow.Context.Set<HU_PROVINCE>().AsNoTracking().AsQueryable();
                var joined = await (from p in entity
                                    where p.ID == id
                                    select new
                                    {
                                        Id = p.ID,
                                        Name = p.NAME
                                    }).FirstOrDefaultAsync();
                var response = new FormatedResponse() { InnerBody = joined };
                return Ok(response);
            }
            catch (Exception ex)
            {
                return Ok(new FormatedResponse() { ErrorType = EnumErrorType.UNCATCHABLE, StatusCode = EnumStatusCode.StatusCode500, MessageCode = ex.Message });
            }
        }
        [HttpPost]
        public async Task<IActionResult> Create(HuFamilyDTO model)
        {
            try
            {
                if (model.IsDeduct != true)
                {
                    model.RegistDeductDate = null;
                    model.DeductFrom = null;
                }
                if (model.DeductFrom > model.DeductTo)
                {
                    return Ok(new FormatedResponse() { ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400, MessageCode = "UI_FORM_CONTROL_ERROR_FROM_DATE_MUST_LESS_THAN_EQUAL_TO_DATE" });
                }
                var otherlists = _uow.Context.Set<SYS_OTHER_LIST>().AsNoTracking().AsQueryable();
                var types = _uow.Context.Set<SYS_OTHER_LIST_TYPE>().AsNoTracking().AsQueryable();
                var id = (from p in otherlists
                          from t in types.Where(x => x.ID == p.TYPE_ID)
                          where t.CODE == "DECISION_STATUS" && p.CODE == "CPD"
                          select p.ID).FirstOrDefault();
                model.StatusId = id;
                var sid = Request.Sid(_appSettings);
                if (sid == null) return Unauthorized();
                var response = await _FamilyRepository.Create(_uow, model, sid);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return Ok(new FormatedResponse() { MessageCode = ex.Message, ErrorType = EnumErrorType.UNCATCHABLE, StatusCode = EnumStatusCode.StatusCode500 });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Update(HuFamilyDTO model)
        {
            try
            {
                if (model.IsDeduct != true)
                {
                    model.RegistDeductDate = null;
                    model.DeductFrom = null;
                }
                if (model.DeductFrom > model.DeductTo)
                {
                    return Ok(new FormatedResponse() { ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400, MessageCode = "UI_FORM_CONTROL_ERROR_FROM_DATE_MUST_LESS_THAN_EQUAL_TO_DATE" });
                }
                var sid = Request.Sid(_appSettings);
                if (sid == null) return Unauthorized();
                var response = await _FamilyRepository.Update(_uow, model, sid);
                return Ok(new FormatedResponse() {
                    MessageCode = response.MessageCode,
                    ErrorType = response.ErrorType,
                    StatusCode = response.StatusCode,
                    InnerBody = response.InnerBody
                });
            }
            catch (Exception ex)
            {
                return Ok(new FormatedResponse() { MessageCode = ex.Message, ErrorType = EnumErrorType.UNCATCHABLE, StatusCode = EnumStatusCode.StatusCode500 });
            }
        }

        [HttpPost]
        public async Task<IActionResult> ChangeStatusApprove(GenericToggleIsActiveDTO model)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var entityList = _uow.Context.Set<HU_FAMILY>().AsNoTracking().AsQueryable();
            var response = await entityList.AsNoTracking().Where(p=> model.Ids.Contains(p.ID)).ToListAsync();
            response.ForEach(x =>
            {
                x.STATUS_ID = model.ValueToBind ? OtherConfig.STATUS_APPROVE : OtherConfig.STATUS_WAITING;
            });
            try
            {
                _uow.Context.UpdateRange(response);
                _uow.Context.SaveChanges();
                return Ok(new FormatedResponse()
                {
                    MessageCode = model.ValueToBind == true ? CommonMessageCode.APPROVE_SUCCESS : CommonMessageCode.PENDING_APPROVE_SUCCESS,
                    InnerBody = response,
                    StatusCode = EnumStatusCode.StatusCode200
                });
            }catch(Exception ex)
            {
                return Ok(new FormatedResponse()
                {
                    MessageCode = "FAIL",
                    InnerBody = response,
                    StatusCode = EnumStatusCode.StatusCode200
                });
            }
            
        }
        [HttpGet]
        public async Task<IActionResult> GetGender(long id)
        {
            try
            {
                var entity = _uow.Context.Set<SYS_OTHER_LIST>().AsNoTracking().AsQueryable();
                var joined = await (from p in entity
                                    where p.ID == id
                                    select new
                                    {
                                        Id = p.ID,
                                        Name = p.NAME
                                    }).FirstOrDefaultAsync();
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
