using Microsoft.AspNetCore.Mvc;
using ProfileDAL.Repositories;
using ProfileDAL.ViewModels;
using CORE.DTO;
using CORE.Enum;
using CORE.StaticConstant;
using Azure;
using API.All.DbContexts;
using API.Main;
using InsuranceDAL.Repositories;
using CORE.GenericUOW;
using CORE.Services.File;
using Microsoft.Extensions.Options;
using API;
using Common.Extensions;
using API.Controllers.HuContract;
using System.Data;
using Common.DataAccess;
using API.All.SYSTEM.CoreAPI.Word;
using API.DTO;
using API.All.SYSTEM.CoreAPI.Xlsx;
using Microsoft.AspNetCore.Http;
using System.Globalization;

namespace ProfileAPI.List
{
    [ApiController]
    [ApiExplorerSettings(GroupName = "002-PROFILE-CONTRACT")]
    [HiStaffAuthorize]
    [Route("api/[controller]/[action]")]
    public class HuContractController : BaseController1
    {
        //  IAttendanceBusiness _attendanceBusiness;
        private readonly CoreDbContext _profileDbContext;
        private readonly GenericUnitOfWork _uow;
        private IInsArisingRepository _insArisingRepository;
        private readonly IFileService _fileService;
        private AppSettings _appSettings;
        private readonly IWordRespsitory _wordRespsitory;
        private FullDbContext _fullDbContext;
        private readonly IWebHostEnvironment _env;
        public HuContractController(

            IProfileUnitOfWork profileBusiness,
            CoreDbContext context,
            IOptions<AppSettings> options,
            IWebHostEnvironment env,
            IFileService fileService,

            // thêm code
            FullDbContext dbContext
        ) : base(profileBusiness)
        {
            //  _attendanceBusiness = attendanceBusiness;
            _profileDbContext = context;
            _uow = new GenericUnitOfWork(context);
            _insArisingRepository = new InsArisingRepository(context, _uow);

            _appSettings = options.Value;
            _env = env;
            _fileService = fileService;
            _wordRespsitory = new WordRepository();
            _fullDbContext = dbContext;
            // thêm code
            _HuContractRepository = new HuContractRepository(dbContext, _uow);
        }

        [HttpGet]
        public async Task<ActionResult> GetAll(ContractDTO param)
        {
            var r = await _unitOfWork.ContractRepository.GetAll(param);
            return Ok(r);
        }
        [HttpGet]
        public async Task<ActionResult> GetByEmployeeId(long EmployeeId)
        {
            var r = await _unitOfWork.ContractRepository.GetByEmployeeId(EmployeeId);
            return Ok(r);
        }
        [HttpPost]
        public async Task<IActionResult> QueryList(GenericQueryListDTO<ContractDTO> request)

        {
            try
            {

                var response = await _unitOfWork.ContractRepository.SinglePhaseQueryList(request);

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
        public async Task<IActionResult> QueryListImport(GenericQueryListDTO<HuContractImportDTO> request)
        {
            try
            {

                var response = await _unitOfWork.ContractRepository.SinglePhaseQueryListImport(request);

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
        public async Task<ActionResult> GetById(long Id)
        {
            try
            {
                var r = await _unitOfWork.ContractRepository.GetById(Id);
                return Ok(new FormatedResponse() { InnerBody = r.Data });
            }
            catch (Exception ex)
            {
                return Ok(new FormatedResponse() { ErrorType = EnumErrorType.UNCATCHABLE, StatusCode = EnumStatusCode.StatusCode500, MessageCode = ex.Message });
            }
        }


        [HttpPost]
        public async Task<ActionResult> Create([FromBody] ContractInputDTO param)
        {
            if (!ModelState.IsValid)
            {
                return ResponseValidation();
            }
            if (param.StartDate > param.ExpireDate)
            {
                return Ok(new FormatedResponse()
                {
                    ErrorType = EnumErrorType.CATCHABLE,
                    MessageCode = "CONTRACT_ENDDATE_NOT_LESS_STARTDATE",
                    InnerBody = param,
                    StatusCode = EnumStatusCode.StatusCode400
                });
            }
            var sid = Request.Sid(_appSettings);
            if (param.UploadFileBuffer != null)
            {
                List<UploadFileResponse> uploadFiles = new();
                string location = Path.Combine(_env.ContentRootPath, _appSettings.StaticFolders.Root, _appSettings.StaticFolders.Attachments);
                var uploadFileResponse = await _fileService.UploadFile(new UploadRequest()
                {
                    ClientFileName = param.UploadFileBuffer.ClientFileName,
                    ClientFileType = param.UploadFileBuffer.ClientFileType,
                    ClientFileData = param.UploadFileBuffer.ClientFileData
                }, location, sid);

                // Assign saved paths
                var property = typeof(ContractInputDTO).GetProperty("UploadFile");

                if (property != null)
                {
                    property?.SetValue(param, uploadFileResponse.SavedAs);
                    uploadFiles.Add(uploadFileResponse);
                }
                else
                {
                    return Ok(new FormatedResponse() { ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400, MessageCode = CommonMessageCode.ASSIGN_TO_VALUE_DOES_NOT_MATCH_ANY_COLUMN + ": Attachment" });
                }

            }
            var r = await _unitOfWork.ContractRepository.CreateAsync(param);
            if (r.StatusCode == EnumStatusCode.StatusCode200 && r.InnerBody != null)
            {
                var rsObjData = (ContractInputDTO)r.InnerBody;
                var arisingObj = new API.DTO.InsArisingDTO()
                {
                    PkeyRef = rsObjData.Id,
                    EmployeeId = param.EmployeeId,
                    EffectDate = param.StartDate,
                    TableRef = "CONTRACT"
                };
                var s = await _insArisingRepository.InsertArising(_uow, arisingObj, sid);
            }

            return Ok(r);
        }
        [HttpPost]
        public async Task<ActionResult> Update([FromBody] ContractInputDTO param)
        {
            if (param.StartDate > param.ExpireDate)
            {
                return Ok(new FormatedResponse()
                {
                    ErrorType = EnumErrorType.CATCHABLE,
                    MessageCode = "CONTRACT_ENDDATE_NOT_LESS_STARTDATE",
                    InnerBody = param,
                    StatusCode = EnumStatusCode.StatusCode400
                });
            }
            var sid = Request.Sid(_appSettings);
            if (param.UploadFileBuffer != null)
            {
                List<UploadFileResponse> uploadFiles = new();
                string location = Path.Combine(_env.ContentRootPath, _appSettings.StaticFolders.Root, _appSettings.StaticFolders.Attachments);
                var uploadFileResponse = await _fileService.UploadFile(new UploadRequest()
                {
                    ClientFileName = param.UploadFileBuffer.ClientFileName,
                    ClientFileType = param.UploadFileBuffer.ClientFileType,
                    ClientFileData = param.UploadFileBuffer.ClientFileData
                }, location, sid);

                // Assign saved paths
                var property = typeof(ContractInputDTO).GetProperty("UploadFile");

                if (property != null)
                {
                    property?.SetValue(param, uploadFileResponse.SavedAs);
                    uploadFiles.Add(uploadFileResponse);
                }
                else
                {
                    return Ok(new FormatedResponse() { ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400, MessageCode = CommonMessageCode.ASSIGN_TO_VALUE_DOES_NOT_MATCH_ANY_COLUMN + ": Attachment" });
                }

            }
            if (param.UploadFileBuffer == null)
            {
                var e = _profileDbContext.Contracts.AsNoTracking().Where(p => p.ID == param.Id).First();
                param.UploadFile = e.UPLOAD_FILE;
            }
            var r = await _unitOfWork.ContractRepository.UpdateAsync(param);
            if (r.StatusCode == EnumStatusCode.StatusCode200 && r.InnerBody != null)
            {
                var rsObjData = (ContractInputDTO)r.InnerBody;
                var arisingObj = new API.DTO.InsArisingDTO()
                {
                    PkeyRef = rsObjData.Id,
                    EmployeeId = param.EmployeeId,
                    EffectDate = param.StartDate,
                    TableRef = "CONTRACT"
                };
                var s = await _insArisingRepository.InsertArising(_uow, arisingObj, sid);
            }
            return Ok(r);
        }
        [HttpPost]
        public async Task<ActionResult> DeleteIds(IdsRequest model)
        {
            var r = await _unitOfWork.ContractRepository.RemoveAsync(model.Ids);
            return Ok(r);
        }
        [HttpPost]
        public async Task<ActionResult> OpenStatus([FromBody] long id)
        {
            var r = await _unitOfWork.ContractRepository.OpenStatus(id);
            return ResponseResult(r);
        }

        [HttpPost]
        public async Task<ActionResult> TemplateImport([FromBody] int orgId)
        {
            try
            {
                var stream = await _unitOfWork.ContractRepository.TemplateImport(orgId);
                var fileName = "TempContract.xlsx";
                if (stream.StatusCode == "200")
                {
                    return new FileStreamResult(stream.memoryStream, "application/octet-stream") { FileDownloadName = fileName };
                }
                return ResponseResult(stream);
            }
            catch (Exception ex)
            {
                return ResponseResult(ex.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult> ImportTemplate([FromBody] ImportCTractParam param)
        {
            try
            {
                var r = await _unitOfWork.ContractRepository.ImportTemplate(param);
                if (r.memoryStream != null)
                {
                    var fileName = "TempContractError.xlsx";
                    return new FileStreamResult(r.memoryStream, "application/octet-stream") { FileDownloadName = fileName };
                }
                return ImportResult(r);
            }
            catch (Exception ex)
            {
                return ResponseResult(ex.Message);
            }
        }


        [HttpGet]
        public async Task<ActionResult> PortalGetAll()
        {
            var r = await _unitOfWork.ContractRepository.PortalGetAll();
            return ResponseResult(r);
        }
        [HttpGet]
        public async Task<ActionResult> PortalGetBy(long id)
        {
            var r = await _unitOfWork.ContractRepository.PortalGetBy(id);
            return ResponseResult(r);
        }

        [HttpGet]
        public async Task<IActionResult> GetCode()
        {
            decimal num;
            var queryCode = (from x in _profileDbContext.Contracts where x.CONTRACT_NO.Length == 11 select x.CONTRACT_NO).ToList();
            var existingCode = (from p in queryCode where Decimal.TryParse(p.Substring(0, 4), out num) orderby p descending select p).ToList();
            string newcode = StringCodeGenerator.CreateNewCodeHD("HĐ", 4, existingCode);
            return Ok(new FormatedResponse() { InnerBody = new { Code = newcode } });
        }
        [HttpGet]
        public async Task<ActionResult> GetContractByEmpProfile(long EmployeeId)
        {
            var r = await _unitOfWork.ContractRepository.GetContractByEmpProfile(EmployeeId);
            return Ok(r);
        }

        [HttpGet]
        public async Task<ActionResult> GetContractByEmpProfilePortal(long EmployeeId)
        {
            var r = await _unitOfWork.ContractRepository.GetContractByEmpProfile(EmployeeId);
            return Ok(r);
        }

        [HttpGet]
        public async Task<ActionResult> GetLastContract(long? empId, string? date)
        {
            var r = await _unitOfWork.ContractRepository.GetLastContract(empId, date.Replace("/", "_"));
            return Ok(r);
        }
        [HttpGet]
        public async Task<IActionResult> GetContractType()
        {
            var response = await _unitOfWork.ContractRepository.GetContractType();
            return Ok(response);
        }
        [HttpGet]
        public async Task<ActionResult> GetWageByStartdateContract(long? empId, string? date)
        {
            try
            {
                var r = await _unitOfWork.ContractRepository.GetWageByStartdateContract(empId, date.Replace("/", "_"));
                return Ok(r);
            }
            catch (Exception ex)
            {
                return Ok(new FormatedResponse() { ErrorType = EnumErrorType.UNCATCHABLE, StatusCode = EnumStatusCode.StatusCode500, MessageCode = ex.Message });
            }
        }
        [HttpPost]
        public async Task<ActionResult> ChangeStatusApprove(ContractInputDTO request)
        {
            var sid = Request.Sid(_appSettings);
            foreach (var item in request.ids!)
            {
                request.Id = item;
                var r = await _unitOfWork.ContractRepository.ChangeStatusApprove(request);
                //phat sinh bao hiem
                if (request.ValueToBind == true && r.StatusCode == EnumStatusCode.StatusCode200 && r.InnerBody != null)
                {
                    var rsObjData = (ContractInputDTO)r.InnerBody;
                    var arisingObj = new API.DTO.InsArisingDTO()
                    {
                        PkeyRef = request.Id,
                        EmployeeId = rsObjData.EmployeeId,
                        EffectDate = request.StartDate,
                        TableRef = "CONTRACT"
                    };
                    var s = await _insArisingRepository.InsertArising(_uow, arisingObj, sid);
                }
            }
            await _profileDbContext.SaveChangesAsync();
            return Ok(new FormatedResponse() { MessageCode = request.ValueToBind == true ? CommonMessageCode.APPROVE_SUCCESS : (request.ValueToBind == false ? CommonMessageCode.PENDING_APPROVE_SUCCESS : ""), StatusCode = EnumStatusCode.StatusCode200 });

        }


        // khai báo thuộc tính
        private readonly IHuContractRepository _HuContractRepository;


        // viết phương thức GetById2()
        // để fix bug drop down list
        // không hiện dữ liệu
        [HttpGet]
        public async Task<IActionResult> GetById2(long id)
        {
            var response = await _HuContractRepository.GetById(id);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetFileName(long id)
        {
            try
            {
                var QueryData = new SqlQueryDataTemplate(_fullDbContext);

                var fileType = await  (from p in _fullDbContext.HuContracts
                                from e in _fullDbContext.HuEmployees.Where(x => x.ID == p.EMPLOYEE_ID).DefaultIfEmpty()
                                from cv in _fullDbContext.HuEmployeeCvs.Where(x => x.ID == e.PROFILE_ID).DefaultIfEmpty()
                                from t in _fullDbContext.HuContractTypes.Where(x => x.ID == p.CONTRACT_TYPE_ID).DefaultIfEmpty()
                                from st in _fullDbContext.SysContractTypes.Where(x => x.ID == t.TYPE_ID).DefaultIfEmpty()
                                where p.ID == id
                                select new
                                {
                                    code = st.CODE,
                                    EmployeeCode = e.CODE,
                                    EmployeeName = cv.FULL_NAME
                                }).ToListAsync();
                ;
                string location = Path.Combine(_env.ContentRootPath, _appSettings.StaticFolders.Root, _appSettings.StaticFolders.WordTemplates);
                string relativePath = "";
                if (fileType.Where(x => x.code == "HDTV").Count() > 0)
                {
                    relativePath = "Hợp đồng thử việc_" + fileType.FirstOrDefault().EmployeeCode + "_" + fileType.FirstOrDefault().EmployeeName;
                }
                else
                {
                    relativePath = "Hợp đồng chính thức_" + fileType.FirstOrDefault().EmployeeCode + "_" + fileType.FirstOrDefault().EmployeeName;
                }

                return Ok(new FormatedResponse() { InnerBody = relativePath });

            }
            catch
            {
            }
            return null;

        }

        [HttpGet]
        public async Task<IActionResult> PrintContractInfo(long id)
        {
            try
            {
                var QueryData = new SqlQueryDataTemplate(_fullDbContext);

                var fileType = (from p in _fullDbContext.HuContracts
                                from t in _fullDbContext.HuContractTypes.Where(x => x.ID == p.CONTRACT_TYPE_ID).DefaultIfEmpty()
                                from st in _fullDbContext.SysContractTypes.Where(x => x.ID == t.TYPE_ID).DefaultIfEmpty()
                                where st.CODE == "HDTV" && p.ID == id
                                select p).Count() > 0 ? true : false
                                ;
                string location = Path.Combine(_env.ContentRootPath, _appSettings.StaticFolders.Root, _appSettings.StaticFolders.WordTemplates);
                string relativePath = "";
                if (fileType)
                {
                    relativePath = "HD_TV.docx";
                }
                else
                {
                    relativePath = "HD_PRIMARY.doc";
                }
                var absolutePath = Path.Combine(location, relativePath);
                DataSet dataSet = new DataSet("MyDataSet");
                dataSet = QueryData.ExecuteStoreToTable("PKG_PRINT_CONTRACT_INFO", new
                {
                    P_ID = id,
                }, false);
                var file = await _wordRespsitory.ExportWordNoImage(dataSet, absolutePath);
                return File(file, "application/octet-stream", relativePath);
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

        [HttpPost]
        public async Task<IActionResult> Save(ImportQueryListBaseDTO request)
        {
            var response = await _unitOfWork.ContractRepository.Save(request);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> IsReceive(ContractInputDTO request)
        {
            var response = await _unitOfWork.ContractRepository.IsReceive(request);

            return Ok(new FormatedResponse()
            {
                InnerBody = null,
                StatusCode = response ? EnumStatusCode.StatusCode400 : EnumStatusCode.StatusCode200
            });
        }

        [HttpPost]
        public async Task<IActionResult> LiquidationContract(ContractInputDTO request)
        {
            var response = await _unitOfWork.ContractRepository.LiquidationContract(request);

            return Ok(response);
        }


        [HttpGet]
        public async Task<ActionResult> GetContractByEmpProfile2(long EmployeeId)
        {
            var r = await _unitOfWork.ContractRepository.GetContractByEmpProfile2(EmployeeId);
            return Ok(r);
        }

        [HttpGet]
        public async Task<IActionResult> ScanUpdateStatusEmpDetail()
        {
            _unitOfWork.ContractRepository.ScanUpdateStatusEmpDetail();
            return Ok();
        }
    }
}
