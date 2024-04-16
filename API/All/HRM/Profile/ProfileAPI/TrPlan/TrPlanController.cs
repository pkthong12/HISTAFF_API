using API.All.DbContexts;
using API.All.SYSTEM.CoreAPI.Word;
using API.DTO;
using API.Main;
using Common.DataAccess;
using CORE.DTO;
using CORE.Enum;
using CORE.GenericUOW;
using CORE.Services.File;
using CORE.StaticConstant;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Data;
using System.Linq.Dynamic.Core;

namespace API.Controllers.TrPlan
{
    [ApiExplorerSettings(GroupName = "163-OTHER-TR_PLAN")]
    [ApiController]
    [HiStaffAuthorize]
    [Route("api/[controller]/[action]")]
    public class TrPlanController : ControllerBase
    {
        private readonly GenericUnitOfWork _uow;
        private readonly ITrPlanRepository _TrPlanRepository;
        private readonly AppSettings _appSettings;
        private readonly IWebHostEnvironment _env;
        private readonly IFileService _fileService;
        private readonly IWordRespsitory _wordRespsitory;
        private FullDbContext _fullDbContext;

        public TrPlanController(
            FullDbContext dbContext,
            IWebHostEnvironment env,
            IFileService fileService,
            IOptions<AppSettings> options)
        {
            _uow = new GenericUnitOfWork(dbContext);
            _TrPlanRepository = new TrPlanRepository(dbContext, _uow, env, options, fileService);
            _appSettings = options.Value;
            _env = env;
            _fileService = fileService;
            _fullDbContext = dbContext;
            _wordRespsitory = new WordRepository();

        }

        [HttpGet]
        public async Task<IActionResult> ReadAll()
        {
            var response = await _TrPlanRepository.ReadAll();
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> ReadAllByNumberKey(ReadAllByKeyLongRequest request)
        {
            var response = await _TrPlanRepository.ReadAllByKey(request.Key, request.Value);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> ReadAllByStringKey(ReadAllByKeyStringRequest request)
        {
            var response = await _TrPlanRepository.ReadAllByKey(request.Key, request.Value);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> QueryList(GenericQueryListDTO<TrPlanDTO> request)
        {
            var response = await _TrPlanRepository.SinglePhaseQueryList(request);
            return Ok(new FormatedResponse() { InnerBody = response });
        }

        [HttpGet]
        public async Task<IActionResult> GetById(long id)
        {
            var response = await _TrPlanRepository.GetById(id);
            return Ok(response);
        }


        [HttpGet]
        public async Task<IActionResult> GetByStringId(string id)
        {
            var response = await _TrPlanRepository.GetById(id);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Create(TrPlanDTO model)
        {
            List<UploadFileResponse> uploadFiles = new();
            try
            {
                var sid = Request.Sid(_appSettings);
                if (sid == null) return Unauthorized();

                if (model.AttachmentBuffer != null)
                {
                    string location = Path.Combine(_env.ContentRootPath, _appSettings.StaticFolders.Root, _appSettings.StaticFolders.Attachments);
                    var uploadFileResponse = await _fileService.UploadFile(new UploadRequest()
                    {
                        ClientFileName = model.AttachmentBuffer.ClientFileName,
                        ClientFileType = model.AttachmentBuffer.ClientFileType,
                        ClientFileData = model.AttachmentBuffer.ClientFileData
                    }, location, sid);
                    var property = typeof(TrPlanDTO).GetProperty("Filename");

                    if (property != null)
                    {
                        property?.SetValue(model, uploadFileResponse.SavedAs);
                        uploadFiles.Add(uploadFileResponse);
                    }
                    else
                    {
                        return Ok(new FormatedResponse() { ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400, MessageCode = CommonMessageCode.ASSIGN_TO_VALUE_DOES_NOT_MATCH_ANY_COLUMN + ": Attachment" });
                    }
                }
                //var id = _uow.Context.Set<TR_PLAN>().AsNoTracking().AsQueryable().Max(p => p.ID);
                //string newcode = "";
                //if (await _uow.Context.Set<TR_PLAN>().CountAsync() == 0)
                //{
                //    newcode = "KH001";
                //}
                //else
                //{
                //    string lastestData = _uow.Context.Set<TR_PLAN>().OrderByDescending(x => x.CODE).First().CODE!.ToString();
                //    newcode = lastestData.Substring(0, 2) + (int.Parse(lastestData.Substring(lastestData.Length - 3)) + 1).ToString("D3");
                //}
                //model.Code = newcode;
                model.JobFamilyIds = (model.ListJobFamilyIds != null ? string.Join(",", model.ListJobFamilyIds.ToArray()) : "");
                model.JobIds = (model.ListJobIds != null ? string.Join(",", model.ListJobIds.ToArray()) : "");
                var response = await _TrPlanRepository.Create(_uow, model, sid);
                return Ok(response);
            }
            catch (Exception ex)
            {

                uploadFiles.ForEach(async f =>
                {
                    await _fileService.DeleteAttachment(f.SavedAs);
                });

                _uow.Rollback();
                return Ok(new FormatedResponse() { ErrorType = EnumErrorType.UNCATCHABLE, StatusCode = EnumStatusCode.StatusCode500, MessageCode = ex.Message });
            }
        }
        [HttpPost]
        public async Task<IActionResult> CreateRange(List<TrPlanDTO> models)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _TrPlanRepository.CreateRange(_uow, models, sid);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Update(TrPlanDTO model)
        {
            List<UploadFileResponse> uploadFiles = new();
            try
            {
                var sid = Request.Sid(_appSettings);
                if (sid == null) return Unauthorized();
                if (model.AttachmentBuffer != null)
                {
                    string location = Path.Combine(_env.ContentRootPath, _appSettings.StaticFolders.Root, _appSettings.StaticFolders.Attachments);
                    var uploadFileResponse = await _fileService.UploadFile(new UploadRequest()
                    {
                        ClientFileName = model.AttachmentBuffer.ClientFileName,
                        ClientFileType = model.AttachmentBuffer.ClientFileType,
                        ClientFileData = model.AttachmentBuffer.ClientFileData
                    }, location, sid);
                    var property = typeof(TrPlanDTO).GetProperty("Filename");

                    if (property != null)
                    {
                        property?.SetValue(model, uploadFileResponse.SavedAs);
                        uploadFiles.Add(uploadFileResponse);
                    }
                    else
                    {
                        return Ok(new FormatedResponse() { ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400, MessageCode = CommonMessageCode.ASSIGN_TO_VALUE_DOES_NOT_MATCH_ANY_COLUMN + ": Attachment" });
                    }
                }
                model.JobFamilyIds = (model.ListJobFamilyIds != null ? string.Join(",", model.ListJobFamilyIds.ToArray()) : "");
                model.JobIds = (model.ListJobIds != null ? string.Join(",", model.ListJobIds.ToArray()) : "");
                var response = await _TrPlanRepository.Update(_uow, model, sid);
                return Ok(response);
            }
            catch (Exception ex)
            {

                uploadFiles.ForEach(async f =>
                {
                    await _fileService.DeleteAttachment(f.SavedAs);
                });

                _uow.Rollback();
                return Ok(new FormatedResponse() { ErrorType = EnumErrorType.UNCATCHABLE, StatusCode = EnumStatusCode.StatusCode500, MessageCode = ex.Message });
            }
            //var sid = Request.Sid(_appSettings);
            //if (sid == null) return Unauthorized();
            //var response = await _TrPlanRepository.Update(_uow, model, sid);
            //return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateRange(List<TrPlanDTO> models)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _TrPlanRepository.UpdateRange(_uow, models, sid);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(TrPlanDTO model)
        {
            if (model.Id != null)
            {
                var response = await _TrPlanRepository.Delete(_uow, (long)model.Id);
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
            var response = await _TrPlanRepository.DeleteIds(_uow, model.Ids);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteStringIds(StringIdsRequest model)
        {
            var response = await _TrPlanRepository.DeleteIds(_uow, model.Ids);
            return Ok(response);
        }
        [HttpGet]
        public async Task<IActionResult> GetAllOrg()
        {
            var response = await _TrPlanRepository.GetAllOrg();
            return Ok(response);
        }
        [HttpGet]
        public async Task<IActionResult> GetAllCourse()
        {
            var response = await _TrPlanRepository.GetAllCoures();
            return Ok(response);
        }
        [HttpGet]
        public async Task<IActionResult> GetAllCenter()
        {
            var response = await _TrPlanRepository.GetAllCenter();
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetCode()
        {
            decimal num;
            var queryCode = await (from x in _uow.Context.Set<TR_PLAN>() where x.CODE.Length == 5 select x.CODE).ToListAsync();
            var existingCode = (from p in queryCode where Decimal.TryParse(p.Substring(2), out num) orderby p descending select p).ToList();
            string newcode = StringCodeGenerator.CreateNewCode("KH", 3, existingCode);
            return Ok(new FormatedResponse() { InnerBody = new { Code = newcode } });
        }

        [HttpGet]
        public async Task<IActionResult> PrintPlan(string id)
        {
            try
            {
                List<long?> numbers = new List<long?>();

                string[] values = id.Split(',');

                foreach (string value in values)
                {
                    if (long.TryParse(value, out long number))
                    {
                        numbers.Add(number);
                    }
                }

                var lstYear = _uow.Context.Set<TR_PLAN>()
                    .Where(x => numbers.Contains(x.ID))
                    .Select(x => x.YEAR)
                    .ToList().Distinct().ToList();

                if (lstYear.Count > 1)
                {
                    return Ok(new FormatedResponse()
                    {
                        ErrorType = EnumErrorType.UNCATCHABLE,
                        MessageCode = CommonMessageCode.EXIST_MULTIPLINE_YEAR,
                        StatusCode = EnumStatusCode.StatusCode400,
                    });
                }
                else
                {
                    var QueryData = new SqlQueryDataTemplate(_fullDbContext);
                    string location = Path.Combine(_env.ContentRootPath, _appSettings.StaticFolders.Root, _appSettings.StaticFolders.WordTemplates);
                    string relativePath = "Tờ trình Kế hoạch đào tạo.docx";
                    var absolutePath = Path.Combine(location, relativePath);
                    DataSet dataSet = new DataSet("MyDataSet");
                    dataSet = QueryData.ExecuteStoreToTable("PKG_PRINT_TR_PLAN", new
                    {
                        P_ID = "," + id + ",",
                    }, false);
                    var file = await _wordRespsitory.ExportWordNoImage(dataSet, absolutePath);
                    return File(file, "application/octet-stream", relativePath);
                }
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
        public async Task<IActionResult> GetTrainingForm()
        {
            var response = await _TrPlanRepository.GetTrainingForm();
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> GetJobByJobFamId(IdsRequest model)
        {
            var response = await _TrPlanRepository.GetJobByJobFamId(model.Ids);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetJobFamily()
        {
            try
            {
                var entity = _uow.Context.Set<SYS_OTHER_LIST_TYPE>().AsNoTracking().AsQueryable();
                var entitys = _uow.Context.Set<SYS_OTHER_LIST>().AsNoTracking().AsQueryable();
                var r = await (from a in entity where (a.CODE == "HU_JOB_FAMILY") select a.ID).ToListAsync();
                var joined = await (from p in entitys
                                    where r.Contains(p.TYPE_ID!.Value)
                                    select new
                                    {
                                        Id = p.ID,
                                        Name = p.NAME,
                                    }).FirstOrDefaultAsync();
                return Ok(new FormatedResponse() { InnerBody = joined });
            }
            catch (Exception ex)
            {
                return Ok(new FormatedResponse() { MessageCode = ex.Message, ErrorType = EnumErrorType.UNCATCHABLE, StatusCode = EnumStatusCode.StatusCode500 });
            }
        }
    }
}

