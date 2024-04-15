using API.All.DbContexts;
using API.All.HRM.Profile.ProfileAPI.HuEmployeeCv;
using API.All.SYSTEM.CoreAPI.Word;
using API.DTO;
using API.Main;
using Aspose.Cells.Drawing.ActiveXControls;
using Azure;
using Common.DataAccess;
using CORE.DTO;
using CORE.Enum;
using CORE.GenericUOW;
using CORE.Services.File;
using CORE.StaticConstant;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Org.BouncyCastle.Asn1.X509;
using ProfileDAL.ViewModels;
using System.Data;
using System.Dynamic;

namespace API.Controllers.HuEmployeeCv
{
    [ApiExplorerSettings(GroupName = "058-PROFILE-HU_EMPLOYEE_CV")]
    [ApiController]
    [HiStaffAuthorize]
    [Route("api/[controller]/[action]")]
    public class HuEmployeeCvController : ControllerBase
    {
        private readonly GenericUnitOfWork _uow;
        private readonly IHuEmployeeCvRepository _HuEmployeeCvRepository;
        private readonly IWordRespsitory _wordRespsitory;
        private readonly AppSettings _appSettings;
        private readonly FullDbContext _dbContext;
        private readonly IWebHostEnvironment _env;

        public HuEmployeeCvController(
            FullDbContext dbContext,
            IOptions<AppSettings> options,
            IWebHostEnvironment env,
            IFileService fileService)
        {
            _uow = new GenericUnitOfWork(dbContext);
            _HuEmployeeCvRepository = new HuEmployeeCvRepository(dbContext, _uow, env, options, fileService);
            _wordRespsitory = new WordRepository();
            _appSettings = options.Value;
            _dbContext = dbContext;
            _env = env;
        }

        [HttpGet]
        public async Task<IActionResult> ReadAll()
        {
            var response = await _HuEmployeeCvRepository.ReadAll();
            return Ok(new FormatedResponse() { InnerBody = response });
        }

        [HttpPost]
        public async Task<IActionResult> ReadAllByNumberKey(ReadAllByKeyLongRequest request)
        {
            var response = await _HuEmployeeCvRepository.ReadAllByKey(request.Key, request.Value);
            return Ok(new FormatedResponse() { InnerBody = response });
        }

        [HttpPost]
        public async Task<IActionResult> ReadAllByStringKey(ReadAllByKeyStringRequest request)
        {
            var response = await _HuEmployeeCvRepository.ReadAllByKey(request.Key, request.Value);
            return Ok(new FormatedResponse() { InnerBody = response });
        }

        [HttpPost]
        public async Task<IActionResult> QueryList(GenericQueryListDTO<HuEmployeeCvDTO> request)
        {
            var response = await _HuEmployeeCvRepository.SinglePhaseQueryList(request);
            return Ok(new FormatedResponse() { InnerBody = response });
        }

        [HttpGet]
        public async Task<IActionResult> GetById(long id)
        {
            var response = await _HuEmployeeCvRepository.GetById(id);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetByStringId(string id)
        {
            var response = await _HuEmployeeCvRepository.GetById(id);
            return Ok(new FormatedResponse() { InnerBody = response });
        }

        [HttpPost]
        public async Task<IActionResult> Create(HuEmployeeCvDTO model)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _HuEmployeeCvRepository.Create(_uow, model, sid);
            return Ok(new FormatedResponse() { InnerBody = response });
        }
        [HttpPost]
        public async Task<IActionResult> CreateRange(List<HuEmployeeCvDTO> models)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _HuEmployeeCvRepository.CreateRange(_uow, models, sid);
            return Ok(new FormatedResponse() { InnerBody = response });
        }

        [HttpPost]
        public async Task<IActionResult> Update(HuEmployeeCvDTO model)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _HuEmployeeCvRepository.Update(_uow, model, sid);
            return Ok(new FormatedResponse() { InnerBody = response });
        }

        [HttpPost]
        public async Task<IActionResult> UpdateRange(List<HuEmployeeCvDTO> models)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _HuEmployeeCvRepository.UpdateRange(_uow, models, sid);
            return Ok(new FormatedResponse() { InnerBody = response });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(HuEmployeeCvDTO model)
        {
            if (model.Id != null)
            {
                var response = await _HuEmployeeCvRepository.Delete(_uow, (long)model.Id);
                return Ok(new FormatedResponse() { InnerBody = response });
            }
            else
            {
                return Ok(new FormatedResponse() { ErrorType = EnumErrorType.CATCHABLE, MessageCode = CommonMessageCode.DELETE_REQUEST_NULL_ID });
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteIds(IdsRequest model)
        {
            var response = await _HuEmployeeCvRepository.DeleteIds(_uow, model.Ids);
            return Ok(new FormatedResponse() { InnerBody = response });
        }

        [HttpPost]
        public async Task<IActionResult> DeleteStringIds(StringIdsRequest model)
        {
            var response = await _HuEmployeeCvRepository.DeleteIds(_uow, model.Ids);
            return Ok(new FormatedResponse() { InnerBody = response });
        }

        [HttpGet("{employeeCvId:long}")]
        public async Task<IActionResult> GetBankInfo(long employeeCvId)
        {
            var response = await _HuEmployeeCvRepository.GetBankInfo(employeeCvId);
            return Ok(response);
        }

        [HttpGet("{employeeCvId:long}")]
        public async Task<IActionResult> GetCv(long employeeCvId)
        {
            var response = await _HuEmployeeCvRepository.GetCv(employeeCvId);
            return Ok(response);
        }

        [HttpGet("{employeeId:long}")]
        public async Task<IActionResult> GetBasic(long employeeId)
        {
            var response = await _HuEmployeeCvRepository.GetBasic(employeeId);
            return Ok(response);
        }

        [HttpGet("{employeeCvId:long}")]
        public async Task<IActionResult> GetAdditonalInfo(long employeeCvId)
        {
            var response = await _HuEmployeeCvRepository.GetAdditonalInfo(employeeCvId);
            return Ok(response);
        }

        [HttpGet("{employeeCvId:long}")]
        public async Task<IActionResult> GetPoliticalBackground(long employeeCvId)
        {
            var response = await _HuEmployeeCvRepository.GetPoliticalBackground(employeeCvId);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetGeneralInfo(long id)
        {
            var response = await _HuEmployeeCvRepository.GetGeneralInfo(id);
            return Ok(response);
        }
        
        [HttpGet]
        public async Task<IActionResult> CheckSameName(string name)
        {
            var response = await _HuEmployeeCvRepository.CheckSameName(name);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> CheckSameTaxCode(string taxCode)
        {
            var response = await _HuEmployeeCvRepository.CheckSameTaxCode(taxCode);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> CheckSameItimeid(string itimeId)
        {
            var response = await _HuEmployeeCvRepository.CheckSameItimeid(itimeId);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> CheckSameIdNo(string idNo)
        {
            var response = await _HuEmployeeCvRepository.CheckSameIdNo(idNo);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetBank(long id)
        {
            var response = await _HuEmployeeCvRepository.GetBank(id);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetPolitical(long id)
        {
            var response = await _HuEmployeeCvRepository.GetPolitical(id);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateGeneralInfo(StaffProfileUpdateDTO request)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _HuEmployeeCvRepository.UpdateGeneralInfo(request, sid);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> UpdatePolitical(StaffProfileUpdateDTO request)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _HuEmployeeCvRepository.UpdatePolitical(request, sid);
            return Ok(response);
        }

        [HttpGet("{employeeCvId:long}")]
        public async Task<IActionResult> GetPoliticalOrganization(long employeeCvId)
        {
            var response = await _HuEmployeeCvRepository.GetPoliticalOrganization(employeeCvId);
            return Ok(response);
        }

        [HttpGet("{employeeCvId:long}")]
        public async Task<IActionResult> GetEducation(long employeeCvId)
        {
            var response = await _HuEmployeeCvRepository.GetEducation(employeeCvId);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetEducationId(long id)
        {
            var response = await _HuEmployeeCvRepository.GetEducationId(id);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateEducationId(StaffProfileUpdateDTO request)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _HuEmployeeCvRepository.UpdateEducationId(request, sid);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> UpdatePresenterId(StaffProfileUpdateDTO request)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _HuEmployeeCvRepository.UpdatePresenterId(request, sid);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetPoliticalOrganizationId(long id)
        {
            var response = await _HuEmployeeCvRepository.GetPoliticalOrganizationId(id);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> UpdatePoliticalOrganizationId(StaffProfileUpdateDTO request)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _HuEmployeeCvRepository.UpdatePoliticalOrganizationId(request, sid);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateBank(StaffProfileUpdateDTO request)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _HuEmployeeCvRepository.UpdateBank(request, sid);
            return Ok(response);
        }

        [HttpGet("{employeeCvId:long}")]
        public async Task<IActionResult> GetPapers(long employeeCvId)
        {
            var response = await _HuEmployeeCvRepository.GetPapers(employeeCvId);
            return Ok(response);
        }

        [HttpGet("{employeeCvId:long}")]
        public async Task<IActionResult> GetPresenter(long employeeCvId)
        {
            var response = await _HuEmployeeCvRepository.GetPresenter(employeeCvId);
            return Ok(response);
        }

        [HttpGet("{employeeCvId:long}")]
        public async Task<IActionResult> GetContact(long employeeCvId)
        {
            var response = await _HuEmployeeCvRepository.GetContact(employeeCvId);
            return Ok(response);
        }

        [HttpGet("{employeeCvId:long}")]
        public async Task<IActionResult> GetSituation(long employeeCvId)
        {
            var response = await _HuEmployeeCvRepository.GetSituation(employeeCvId);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetPresenterId(long id)
        {
            var response = await _HuEmployeeCvRepository.GetPresenterId(id);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetSituationId(long id)
        {
            var response = await _HuEmployeeCvRepository.GetSituationId(id);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> UpdatePapers(DynamicDTO model)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _HuEmployeeCvRepository.UpdatePapers(model, sid);
            return Ok(new FormatedResponse() { MessageCode = response.MessageCode, StatusCode = response.StatusCode });
        }

        [HttpGet]
        public async Task<IActionResult> GetEmployeeStatusList()
        {
            var response = await _HuEmployeeCvRepository.GetEmployeeStatusList();
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> CheckPositionMasterInterim(long? id)
        {
            var response = await _HuEmployeeCvRepository.CheckPositionMasterInterim(id);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> InsertStaffProfile(StaffProfileEditDTO request)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _HuEmployeeCvRepository.InsertStaffProfile(request, sid);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateAvatar(StaffProfileEditDTO request)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _HuEmployeeCvRepository.UpdateAvatar(request, sid);
            return Ok(response);
        }

        //[HttpGet]
        //public async Task<IActionResult> GetCode(string code)
        //{
        //    var response = await _HuEmployeeCvRepository.GetCode(code);
        //    return Ok(response);
        //}

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var response = await _HuEmployeeCvRepository.GetAll();
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllIgnoreCurrentUser()
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _HuEmployeeCvRepository.GetAllIgnoreCurrentUser(sid);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetCurruculum(long id)
        {
            var response = await _HuEmployeeCvRepository.GetCurruculum(id);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetContactId(long id)
        {
            var response = await _HuEmployeeCvRepository.GetContactId(id);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateCurruculum(StaffProfileUpdateDTO request)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _HuEmployeeCvRepository.UpdateCurruculum(request, sid);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateContactId(StaffProfileUpdateDTO request)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _HuEmployeeCvRepository.UpdateContactId(request, sid);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateSituationId(StaffProfileUpdateDTO request)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _HuEmployeeCvRepository.UpdateSituationId(request, sid);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateAdditonal(StaffProfileUpdateDTO request)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _HuEmployeeCvRepository.UpdateAdditonal(request, sid);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetAdditonal(long id)
        {
            var response = await _HuEmployeeCvRepository.GetAdditonal(id);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> Get2C_TCTW_98(long id)
        {
            try
            {
                var QueryData = new SqlQueryDataTemplate(_dbContext);
                DataSet dataSet = new DataSet("MyDataSet");
                dataSet = QueryData.ExecuteStoreToTable("PKG_PRINT_TCTW_98", new
                {
                    P_EMPLOYEE_ID = id,
                }, false);
                string location = Path.Combine(_env.ContentRootPath, _appSettings.StaticFolders.Root, _appSettings.StaticFolders.WordTemplates);
                var relativePath = "2C_TCTW_98.doc";
                var absolutePath = Path.Combine(location, relativePath);
                var primaryObj = dataSet.Tables[0];
                string avtPath = primaryObj.Rows[0]["AVATAR"].ToString();
                string locationAvt = Path.Combine(_env.ContentRootPath, _appSettings.StaticFolders.Root, _appSettings.StaticFolders.Avatars);
                var avatarPath = Path.Combine(locationAvt, avtPath);
                var file = await _wordRespsitory.ExportFileWord(dataSet, absolutePath, avtPath == "" ? avtPath : avatarPath, 0, 93, 115, 90);
                return File(file, "application/octet-stream", "2C_TCTW_98.doc");
            }
            catch(Exception ex)
            {
                return Ok(new FormatedResponse()
                {
                    ErrorType = EnumErrorType.UNCATCHABLE,
                    MessageCode =ex.Message,
                    StatusCode = EnumStatusCode.StatusCode400,
                });
            }

        }

        [HttpGet]
        public async Task<IActionResult> Get2C_BNV_2008(long id)
        {
            try
            {
                var QueryData = new SqlQueryDataTemplate(_dbContext);
                DataSet dataSet = new DataSet("MyDataSet");
                dataSet = QueryData.ExecuteStoreToTable("PKG_PRINT_TCTW_98", new
                {
                    P_EMPLOYEE_ID = id,
                }, false);
                string location = Path.Combine(_env.ContentRootPath, _appSettings.StaticFolders.Root, _appSettings.StaticFolders.WordTemplates);
                var relativePath = "2C_BNV_2008 .doc";
                var absolutePath = Path.Combine(location, relativePath);
                var primaryObj = dataSet.Tables[0];
                string avtPath = primaryObj.Rows[0]["AVATAR"].ToString();
                string locationAvt = Path.Combine(_env.ContentRootPath, _appSettings.StaticFolders.Root, _appSettings.StaticFolders.Avatars);
                var avatarPath = Path.Combine(locationAvt, avtPath);
                var file = await _wordRespsitory.ExportFileWord(dataSet, absolutePath, avtPath == "" ? avtPath : avatarPath, 0, 100, 120, 75);
                return File(file, "application/octet-stream", "2C_BNV_2008 .doc");
            }
            catch (Exception ex)
            {
                return Ok(new FormatedResponse()
                {
                    ErrorType = EnumErrorType.UNCATCHABLE,
                    MessageCode = ex.Message,
                    StatusCode = EnumStatusCode.StatusCode400,
                });
            }

        }

        [HttpGet]
        public async Task<IActionResult> GetFileName(long id)
        {
            try
            {

                var fileType = await (from  e in _dbContext.HuEmployees 
                                      join cv in _dbContext.HuEmployeeCvs on e.PROFILE_ID equals cv.ID
                                      where e.ID == id
                                      select new
                                      {
                                          EMPLOYEE_NAME = cv.FULL_NAME,
                                          EMPLOYEE_CODE = e.CODE
                                      }).FirstOrDefaultAsync();
                string relativePath = fileType.EMPLOYEE_NAME + "_"+ fileType.EMPLOYEE_CODE;

                    return Ok(new FormatedResponse() { InnerBody = relativePath });
                
            }
            catch (Exception ex)
            {
            }
            return null;

        }



        [HttpPost]
        public async Task<IActionResult> GetGenderDashboard(HuEmployeeCvInputDTO? model)
        {
            var response = await _HuEmployeeCvRepository.GetGenderDashboard(model);
            return Ok(response);
        }
        [HttpPost]
        public async Task<IActionResult> GetEmpMonthDashboard(HuEmployeeCvInputDTO? model)
        {
            var response = await _HuEmployeeCvRepository.GetEmpMonthDashboard(model);
            return Ok(response);
        }
        [HttpPost]
        public async Task<IActionResult> GetEmpSeniorityDashboard(HuEmployeeCvInputDTO? model)
        {
            var response = await _HuEmployeeCvRepository.GetEmpSeniorityDashboard(model);
            return Ok(response);
        }



        // lấy bằng lái xe theo Id
        [HttpGet]
        public async Task<IActionResult> GetLicenseById(long id)
        {
            var sysOtherList = _uow.Context.Set<SYS_OTHER_LIST>();

            // lấy ra bằng lái xe
            // ví dụ ID = 1276
            var r = await (from p in sysOtherList.Where(x => x.ID == id).AsNoTracking()
                           select new
                           {
                               Id = p.ID,
                               Name = p.NAME
                           }).FirstAsync();

            return Ok(new FormatedResponse() { InnerBody = r });
        }


        
        // lấy trình độ tin học theo Id
        [HttpGet]
        public async Task<IActionResult> GetComputerSkillById(long id)
        {
            var sysOtherList = _uow.Context.Set<SYS_OTHER_LIST>();

            // lấy ra trình độ tin học
            // ví dụ ID = 926
            var r = await (from p in sysOtherList.Where(x => x.ID == id).AsNoTracking()
                           select new
                           {
                               Id = p.ID,
                               Name = p.NAME
                           }).FirstAsync();

            return Ok(new FormatedResponse() { InnerBody = r });
        }



        // lấy 10 cái bản ghi bằng cấp chứng chỉ
        // (nhưng là bằng chính)
        [HttpGet]
        public async Task<IActionResult> Get10RecordCertificate(long employeeCvId)
        {
            var huEmployee = _uow.Context.Set<HU_EMPLOYEE>().Where(x => x.PROFILE_ID == employeeCvId);
            var huCertificate = _uow.Context.Set<HU_CERTIFICATE>().AsQueryable();
            var sysOtherList = _uow.Context.Set<SYS_OTHER_LIST>().AsQueryable();

            var query = await (from item in huEmployee
                           from p in huCertificate.Where(x => x.EMPLOYEE_ID == item.ID && x.IS_PRIME == true)
                           from reference_1 in sysOtherList.Where(x => x.ID == p.LEVEL_ID)
                           from reference_2 in sysOtherList.Where(x => x.ID == p.LEVEL_TRAIN)
                           from reference_3 in sysOtherList.Where(x => x.ID == p.TYPE_TRAIN)
                           from reference_4 in sysOtherList.Where(x => x.ID == p.SCHOOL_ID)

                           select new
                           {
                               // trình độ học vấn
                               LevelName = reference_1.NAME,

                               // trình độ chuyên môn
                               LevelTrainName = reference_2.NAME,

                               // hình thức đào tạo
                               TypeTrainName = reference_3.NAME,

                               // đơn vị đào tạo
                               SchoolName = reference_4.NAME
                           })
                           .ToListAsync();
            
            List<List<ItemDTO>> list_parent = new List<List<ItemDTO>>();

            for (int i = 1; i <= 10; i++)
            {
                List<ItemDTO> list_child = new List<ItemDTO>();

                if (i <= query.Count())
                {
                    int index = i - 1;

                    // trình độ học vấn
                    list_child.Add(
                        new ItemDTO()
                        {
                            controlType = "TEXT",
                            field = $"learningLevel{i}",
                            flexSize = 3,
                            label = $"UI_EDUCATION_LEVEL_ID_{i}",
                            labelFlexSize = 0,
                            value = query[index].LevelName
                        }
                    );

                    // trình độ chuyên môn
                    list_child.Add(
                        new ItemDTO()
                        {
                            controlType = "TEXT",
                            field = $"qualification{i}",
                            flexSize = 3,
                            label = $"UI_EDUCATION_LEVEL_TRAIN_{i}",
                            labelFlexSize = 0,
                            value = query[index].LevelTrainName
                        }
                    );

                    // hình thức đào tạo
                    list_child.Add(
                        new ItemDTO()
                        {
                            controlType = "TEXT",
                            field = $"trainingForm{i}",
                            flexSize = 3,
                            label = $"UI_EDUCATION_TYPE_TRAIN_{i}",
                            labelFlexSize = 0,
                            value = query[index].TypeTrainName
                        }
                    );

                    // đơn vị đào tạo
                    list_child.Add(
                        new ItemDTO()
                        {
                            controlType = "TEXT",
                            field = $"school{i}",
                            flexSize = 3,
                            label = $"UI_EDUCATION_SCHOOL_ID_{i}",
                            labelFlexSize = 0,
                            value = query[index].SchoolName
                        }
                    );

                    list_parent.Add(list_child);
                }
                else
                {
                    // trình độ học vấn
                    list_child.Add(
                        new ItemDTO()
                        {
                            controlType = "TEXT",
                            field = "",
                            flexSize = 3,
                            label = $"UI_EDUCATION_LEVEL_ID_{i}",
                            labelFlexSize = 0,
                            value = ""
                        }
                    );

                    // trình độ chuyên môn
                    list_child.Add(
                        new ItemDTO()
                        {
                            controlType = "TEXT",
                            field = "",
                            flexSize = 3,
                            label = $"UI_EDUCATION_LEVEL_TRAIN_{i}",
                            labelFlexSize = 0,
                            value = ""
                        }
                    );

                    // hình thức đào tạo
                    list_child.Add(
                        new ItemDTO()
                        {
                            controlType = "TEXT",
                            field = "",
                            flexSize = 3,
                            label = $"UI_EDUCATION_TYPE_TRAIN_{i}",
                            labelFlexSize = 0,
                            value = ""
                        }
                    );

                    // đơn vị đào tạo
                    list_child.Add(
                        new ItemDTO()
                        {
                            controlType = "TEXT",
                            field = "",
                            flexSize = 3,
                            label = $"UI_EDUCATION_SCHOOL_ID_{i}",
                            labelFlexSize = 0,
                            value = ""
                        }
                    );

                    list_parent.Add(list_child);
                }
            }

            return Ok(new FormatedResponse() { InnerBody = list_parent });
        }
        [HttpPost]
        public async Task<IActionResult> GetGeneralInfomationDashboard(HuEmployeeCvInputDTO? model)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _HuEmployeeCvRepository.GetGeneralInfomationDashboard(model);
            return Ok(response);
        }
        [HttpPost]
        public async Task<IActionResult> GetNativeInfomationDashboard(HuEmployeeCvInputDTO? model)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _HuEmployeeCvRepository.GetNativeInfomationDashboard(model);
            return Ok(response);
        }
        [HttpPost]
        public async Task<IActionResult> GetIsMemberInfomationDashboard(HuEmployeeCvInputDTO? model)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _HuEmployeeCvRepository.GetIsMemberInfomationDashboard(model);
            return Ok(response);
        }
        [HttpPost]
        public async Task<IActionResult> GetJobInfomationDashboard(HuEmployeeCvInputDTO? model)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _HuEmployeeCvRepository.GetJobInfomationDashboard(model);
            return Ok(response);
        }
        [HttpPost]
        public async Task<IActionResult> GetPositionInfomationDashboard(HuEmployeeCvInputDTO? model)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _HuEmployeeCvRepository.GetPositionInfomationDashboard(model);
            return Ok(response);
        } 
        [HttpPost]
        public async Task<IActionResult> GetLevelInfomationDashboard(HuEmployeeCvInputDTO? model)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _HuEmployeeCvRepository.GetLevelInfomationDashboard(model);
            return Ok(response);
        }
        [HttpPost]
        public async Task<IActionResult> GetWorkingAgeInfomationDashboard(HuEmployeeCvInputDTO? model)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _HuEmployeeCvRepository.GetWorkingAgeInfomationDashboard(model);
            return Ok(response);
        }
        [HttpPost]
        public async Task<IActionResult> GetNewEmpMonthDashboard(HuEmployeeCvInputDTO? model)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _HuEmployeeCvRepository.GetNewEmpMonthDashboard(model);
            return Ok(response);
        }
        [HttpPost]
        public async Task<IActionResult> GetNameOrgDashboard(HuEmployeeCvInputDTO? model)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _HuEmployeeCvRepository.GetNameOrgDashboard(model);
            return Ok(response);
        }
        [HttpPost]
        public async Task<IActionResult> GetIdOrgDissolve(HuEmployeeCvInputDTO? model)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _HuEmployeeCvRepository.GetIdOrgDissolve(model);
            return Ok(response);
        }
    }
}

