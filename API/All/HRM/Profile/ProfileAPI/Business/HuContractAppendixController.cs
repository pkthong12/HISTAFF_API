using Microsoft.AspNetCore.Mvc;
using ProfileDAL.Repositories;
using ProfileDAL.ViewModels;
using CORE.DTO;
using CORE.Enum;
using CORE.StaticConstant;
using API.All.DbContexts;
using API.Main;
using Common.Extensions;
using API;
using CORE.Services.File;
using Microsoft.Extensions.Options;
using API.All.SYSTEM.CoreAPI.Word;
using Common.DataAccess;
using System.Data;
using API.All.SYSTEM.Common;

namespace ProfileAPI.List
{
    [ApiController]
    [ApiExplorerSettings(GroupName = "002-PROFILE_CONTRACTAPPENDIX")]
    [HiStaffAuthorize]
    [Route("api/[controller]/[action]")]
    
    public class HuContractAppendixController : BaseController1
    {
        //  IAttendanceBusiness _attendanceBusiness;
        private readonly ProfileDbContext _profileDbContext;
        private AppSettings _appSettings;
        private readonly IFileService _fileService;
        private readonly IWordRespsitory _wordRespsitory;
        private FullDbContext _fullDbContext;
        private readonly IWebHostEnvironment _env;

        // thêm thuộc tính
        // để làm chức năng upload file


        public HuContractAppendixController(
            IProfileUnitOfWork profileBusiness,
            ProfileDbContext context,
            IOptions<AppSettings> options,
            FullDbContext dbContext,
            IWebHostEnvironment env,
            IFileService fileService
        ) : base(profileBusiness)
        {
            //  _attendanceBusiness = attendanceBusiness;
            _profileDbContext = context;
            _appSettings = options.Value;
            _wordRespsitory = new WordRepository();
            _fullDbContext = dbContext;
            _env = env;
            _fileService = fileService;
        }
        [HttpGet]
        public async Task<ActionResult> GetAll(ContractAppendixDTO param)
        {
            var r = await _unitOfWork.ContractAppendixRepository.GetAll(param);
            return Ok(r);
        }

        [HttpPost]
        public async Task<IActionResult> QueryList(GenericQueryListDTO<ContractAppendixDTO> request)
        {
            try
            {

                var response = await _unitOfWork.ContractAppendixRepository.SinglePhaseQueryList(request);

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
                var r = await _unitOfWork.ContractAppendixRepository.GetById(Id);
                return Ok(new FormatedResponse() { InnerBody = r.Data });
            }
            catch (Exception ex)
            {
                return Ok(new FormatedResponse() { ErrorType = EnumErrorType.UNCATCHABLE, StatusCode = EnumStatusCode.StatusCode500, MessageCode = ex.Message });
            }
        }
        [HttpPost]
        public async Task<ActionResult> Create([FromBody] ContractAppendixInputDTO param)
        {
            if (!ModelState.IsValid)
            {
                return ResponseValidation();
            }


            var sid = Request.Sid(_appSettings);


            // code upload file
            if (param.AttachmentBuffer != null)
            {
                List<UploadFileResponse> uploadFiles = new();
                string location = Path.Combine(_env.ContentRootPath, _appSettings.StaticFolders.Root, _appSettings.StaticFolders.Attachments);
                var uploadFileResponse = await _fileService.UploadFile(new UploadRequest()
                {
                    ClientFileName = param.AttachmentBuffer.ClientFileName,
                    ClientFileType = param.AttachmentBuffer.ClientFileType,
                    ClientFileData = param.AttachmentBuffer.ClientFileData
                }, location, sid);

                // Assign saved paths
                var property = typeof(ContractAppendixInputDTO).GetProperty("UploadFile");

                if (property != null)
                {
                    property?.SetValue(param, uploadFileResponse.SavedAs);
                    uploadFiles.Add(uploadFileResponse);
                }
                else
                {
                    return Ok(new FormatedResponse() { ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400, MessageCode = CommonMessageCode.ASSIGN_TO_VALUE_DOES_NOT_MATCH_ANY_COLUMN + ": UploadFile" });
                }

            }


            if (param.StartDate > param.ExpireDate)
            {
                return Ok(new FormatedResponse() {MessageCode ="CONTRACT_ENDDATE_NOT_LESS_STARTDATE",
                                                    InnerBody = param,
                                                    StatusCode = EnumStatusCode.StatusCode400 }); 
            }


            // thêm mới bản ghi
            var r = await _unitOfWork.ContractAppendixRepository.CreateAsync(param);
           

            return Ok(r);
        }
        [HttpPost]
        public async Task<ActionResult> Update([FromBody] ContractAppendixInputDTO param)
        {
            var sid = Request.Sid(_appSettings);


            if (param.AttachmentBuffer != null)
            {
                List<UploadFileResponse> uploadFiles = new();
                string location = Path.Combine(_env.ContentRootPath, _appSettings.StaticFolders.Root, _appSettings.StaticFolders.Attachments);
                var uploadFileResponse = await _fileService.UploadFile(new UploadRequest()
                {
                    ClientFileName = param.AttachmentBuffer.ClientFileName,
                    ClientFileType = param.AttachmentBuffer.ClientFileType,
                    ClientFileData = param.AttachmentBuffer.ClientFileData
                }, location, sid);

                // Assign saved paths
                var property = typeof(ContractAppendixInputDTO).GetProperty("UploadFile");

                if (property != null)
                {
                    property?.SetValue(param, uploadFileResponse.SavedAs);
                    uploadFiles.Add(uploadFileResponse);
                }
                else
                {
                    return Ok(new FormatedResponse() { ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400, MessageCode = CommonMessageCode.ASSIGN_TO_VALUE_DOES_NOT_MATCH_ANY_COLUMN + ": UploadFile" });
                }

            }


            if (param.StartDate > param.ExpireDate)
            {
                return Ok(new FormatedResponse() {MessageCode ="CONTRACT_ENDDATE_NOT_LESS_STARTDATE",
                                                    InnerBody = param,
                                                    StatusCode = EnumStatusCode.StatusCode400 }); 
            }


            // lấy giá trị
            // của phiên làm việc truớc
            if (param.AttachmentBuffer == null)
            {
                var record = _profileDbContext.HuFileContracts.AsNoTracking().Where(x => x.ID == param.Id).First();

                param.UploadFile = record.UPLOAD_FILE;
            }
            
            
            var r = await _unitOfWork.ContractAppendixRepository.UpdateAsync(param);
            
            
            return Ok(r);
        }
        [HttpPost]
        public async Task<ActionResult> DeleteIds(IdsRequest model)
        {
            var r = await _unitOfWork.ContractAppendixRepository.RemoveAsync(model.Ids);
            return Ok(r);
        }
        [HttpPost]
        public async Task<ActionResult> OpenStatus([FromBody] long id)
        {
            var r = await _unitOfWork.ContractAppendixRepository.OpenStatus(id);
            return ResponseResult(r);
        }
        [HttpGet]
        public async Task<IActionResult> GetCode()
        {
            decimal num;
            var queryCode = (from x in _profileDbContext.HuFileContracts where x.CONTRACT_NO.Length == 13 select x.CONTRACT_NO).ToList();
            var existingCode = (from p in queryCode where Decimal.TryParse(p.Substring(0,4), out num) orderby p descending select p).ToList();
            string newcode = StringCodeGenerator.CreateNewCodeHD("PLHĐ", 4, existingCode);
            return Ok(new FormatedResponse() { InnerBody = new { Code = newcode } });
        }
        [HttpGet]
        public async Task<ActionResult> GetContractAppendixByEmpProfile(long EmployeeId)
        {
            var r = await _unitOfWork.ContractAppendixRepository.GetContractAppendixByEmpProfile(EmployeeId);
            return Ok(r);
        }

        /*
        [HttpPost]
        public async Task<IActionResult> GetAllowanceWageById(GenericQueryListDTO<WorkingAllowViewDTO> request)
        {
            try
            {
                long? _WorkingId = 0;
                if(request.Filter !=null)
                {
                    _WorkingId = request.Filter.WorkingId;
                }
                var r = await _unitOfWork.ContractAppendixRepository.GetAllowanceWageById(_WorkingId);
                return Ok(new FormatedResponse() { InnerBody = r.Data,StatusCode = EnumStatusCode.StatusCode200 });
            }
            catch (Exception ex)
            {
                return Ok(new FormatedResponse() { ErrorType = EnumErrorType.UNCATCHABLE, StatusCode = EnumStatusCode.StatusCode500, MessageCode = ex.Message });
            }
        }
        */
        [HttpGet]
        public async Task<IActionResult> GetAllowanceWageById(long workingId)
        {
            try
            {
                var r = await _unitOfWork.ContractAppendixRepository.GetAllowanceWageById(workingId);
                return Ok(new FormatedResponse() { InnerBody = r.Data, StatusCode = EnumStatusCode.StatusCode200 });
            }
            catch (Exception ex)
            {
                return Ok(new FormatedResponse() { ErrorType = EnumErrorType.UNCATCHABLE, StatusCode = EnumStatusCode.StatusCode500, MessageCode = ex.Message });
            }
        }
        [HttpGet]
        public async Task<ActionResult> GetWageByContract(long contractId)
        {
            try
            {
                var r = await _unitOfWork.ContractAppendixRepository.GetWageByContract(contractId);
                return Ok(new FormatedResponse() { InnerBody = r.Data, StatusCode = EnumStatusCode.StatusCode200 });
            }
            catch (Exception ex)
            {
                return Ok(new FormatedResponse() { ErrorType = EnumErrorType.UNCATCHABLE, StatusCode = EnumStatusCode.StatusCode500, MessageCode = ex.Message });
            }
        }
        [HttpGet]
        public async Task<ActionResult> GetExpiteDateByContract(long contractId)
        {
            try
            {
                var r = await _unitOfWork.ContractAppendixRepository.GetExpiteDateByContract(contractId);
                return Ok(new FormatedResponse() { InnerBody = r.Data, StatusCode = EnumStatusCode.StatusCode200 });
            }
            catch (Exception ex)
            {
                return Ok(new FormatedResponse() { ErrorType = EnumErrorType.UNCATCHABLE, StatusCode = EnumStatusCode.StatusCode500, MessageCode = ex.Message });
            }
        }
         [HttpPost]
        public async Task<ActionResult> ChangeStatusApprove(ContractAppendixInputDTO request)
        {
            foreach (var item in request.ids!)
            {
                var r = _profileDbContext.HuFileContracts.Where(x => x.ID == item).FirstOrDefault();
                if (r != null)
                {
                    r.STATUS_ID = request.ValueToBind == true ? OtherConfig.STATUS_APPROVE : (request.ValueToBind == false ? OtherConfig.STATUS_WAITING : null);
                    var result = _profileDbContext.HuFileContracts.Update(r);
                }
            }
            await _profileDbContext.SaveChangesAsync();
            return Ok(new FormatedResponse() { MessageCode = request.ValueToBind == true ? CommonMessageCode.APPROVE_SUCCESS : (request.ValueToBind == false ? CommonMessageCode.PENDING_APPROVE_SUCCESS : ""), StatusCode = EnumStatusCode.StatusCode200 });

        }


        [HttpGet]
        public async Task<IActionResult> PrintContractAppendix(long id)
        {
            try
            {
                var QueryData = new SqlQueryDataTemplate(_fullDbContext);
                string location = Path.Combine(_env.ContentRootPath, _appSettings.StaticFolders.Root, _appSettings.StaticFolders.WordTemplates);
                string relativePath = "PL HĐLĐ.doc";
                var absolutePath = Path.Combine(location, relativePath);
                DataSet dataSet = new DataSet("MyDataSet");
                dataSet = QueryData.ExecuteStoreToTable("PKG_PRINT_CONTRACT_APPENDIX", new
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


        [HttpGet]
        public async Task<IActionResult> GetFileName(long id)
        {
            try
            {
                // declare list code of "appendix contract type"
                List<string> list = ["PLHD"];

                // list id of "appendix contract type"
                var listId = _profileDbContext.ContractTypes
                                                .Where(x => list.Contains(x.CODE))
                                                .Select(x => x.ID)
                                                .ToList();

                // get record of "appendix contract"
                var appendixContract = await _profileDbContext.HuFileContracts.FirstOrDefaultAsync(x => x.ID == id);

                string fileName = "";

                // check "appendix contract type"
                if (listId.Contains((long)appendixContract.APPEND_TYPEID))
                {
                    fileName = "Phu_luc_HDLD";
                }
                else
                {
                    return Ok(new FormatedResponse()
                    {
                        ErrorType = EnumErrorType.CATCHABLE,
                        MessageCode = CommonMessageCodes.NOT_FOUND_SUITABLE_APPENDIX_CONTRACT,
                        StatusCode = EnumStatusCode.StatusCode400
                    });
                }

                return Ok(new FormatedResponse()
                {
                    InnerBody = fileName
                });
            }
            catch (Exception ex)
            {
                return Ok(new FormatedResponse()
                {
                    ErrorType = EnumErrorType.UNCATCHABLE,
                    MessageCode = ex.Message,
                    StatusCode = EnumStatusCode.StatusCode400
                });
            }
        }


        [HttpGet]
        public async Task<IActionResult> PrintAppendixContract(long id)
        {
            try
            {
                // get file path
                string relativePath = "";
                string location = Path.Combine(_env.ContentRootPath, _appSettings.StaticFolders.Root, _appSettings.StaticFolders.WordTemplates);


                // declare list code of "appendix contract type"
                List<string> list = ["PLHD"];

                // list id of "appendix contract type"
                var listId = _profileDbContext.ContractTypes
                                                .Where(x => list.Contains(x.CODE))
                                                .Select(x => x.ID)
                                                .ToList();

                // get record of "appendix contract"
                var appendixContract = await _profileDbContext.HuFileContracts.FirstOrDefaultAsync(x => x.ID == id);


                // execute stored procedure SQL Server
                var QueryData = new SqlQueryDataTemplate(_fullDbContext);

                DataSet dataSet = new DataSet("MyDataSet");


                // check "appendix contract type"
                if (listId.Contains((long)appendixContract.APPEND_TYPEID))
                {
                    relativePath = "APPENDIX_CONTRACT.docx";

                    dataSet = QueryData.ExecuteStoreToTable(
                        "PKG_PRINT_APPENDIX_CONTRACT",
                        new { P_ID = id },
                        false);
                }
                else
                {
                    return Ok(new FormatedResponse()
                    {
                        ErrorType = EnumErrorType.CATCHABLE,
                        MessageCode = CommonMessageCodes.NOT_FOUND_SUITABLE_APPENDIX_CONTRACT,
                        StatusCode = EnumStatusCode.StatusCode400
                    });
                }

                var absolutePath = Path.Combine(location, relativePath);

                // create file
                var file = await _wordRespsitory.ExportWordNoImage(dataSet, absolutePath);


                // return result
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
    }
}
