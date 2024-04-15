using API;
using API.All.DbContexts;
using API.Entities;
using API.Main;
using CORE.DTO;
using CORE.Enum;
using CORE.GenericUOW;
using CORE.Services.File;
using CORE.StaticConstant;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.VisualBasic;
using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;
using ProfileDAL.Repositories;
using InsuranceDAL.Repositories;
using ProfileDAL.ViewModels;
using InsuranceAPI.Business;
using System.Text.RegularExpressions;
using API.DTO;
using Common.Extensions;
using System.Linq.Dynamic.Core;
using System;
using API.All.SYSTEM.CoreAPI.Word;
using Common.DataAccess;
using System.Data;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ProfileAPI.List
{
    [ApiController]
    [ApiExplorerSettings(GroupName = "002-PROFILE-TERMINATE")]
    [HiStaffAuthorize]
    [Route("api/[controller]/[action]")]
    public class HuTerminateController : BaseController1
    {
        private readonly GenericUnitOfWork _uow;
        private IGenericRepository<HU_TERMINATE, TerminateInputDTO> _genericRepository;
        private IInsArisingRepository _insArisingRepository;
        private readonly GenericReducer<HU_TERMINATE, TerminateInputDTO> genericReducer;
        private readonly IFileService _fileService;
        private AppSettings _appSettings;
        private readonly IWebHostEnvironment _env;
        private CoreDbContext _coreDbContext;
        private FullDbContext _fullDbContext;
        private readonly IWordRespsitory _wordRespsitory;


        public HuTerminateController(IProfileUnitOfWork unitOfWork, IOptions<AppSettings> options, IWebHostEnvironment env, IFileService fileService, CoreDbContext coreDbContext, FullDbContext fullDbContext) : base(unitOfWork)
        {
            _uow = new GenericUnitOfWork(coreDbContext);
            _genericRepository = _uow.GenericRepository<HU_TERMINATE, TerminateInputDTO>();
            _insArisingRepository = new InsArisingRepository(coreDbContext, _uow);
            genericReducer = new();
            _appSettings = options.Value;
            _env = env;
            _fileService = fileService;
            _coreDbContext = coreDbContext;
            _fullDbContext = fullDbContext;
            _wordRespsitory = new WordRepository();
        }
        [HttpPost]
        public async Task<IActionResult> QueryList(GenericQueryListDTO<TerminateDTO> request)

        {
            try
            {
                /* HERE WE CAN EITHER CHOOSE ONE OF THE 2 APPROACHS: TwoPhaseQueryList or SinglePhaseQueryList */
                /* WE WILL KNOW WHAT APPROACH IS FASTER IN THE FUTURE */

                var response = await _unitOfWork.TerminateRepository.TwoPhaseQueryList(request);

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
        public async Task<ActionResult> GetAll(TerminateDTO param)
        {
            var r = await _unitOfWork.TerminateRepository.GetAll(param);
            return Ok(r);
        }
        [HttpGet]
        public async Task<ActionResult> GetById(long Id)
        {
            var r = await _unitOfWork.TerminateRepository.GetById(Id);
            return Ok(new FormatedResponse() { InnerBody = r.Data });
        }
        [HttpGet]
        public async Task<IActionResult> GetTerminateByEmployee(long Id)
        {
            var r = await _unitOfWork.TerminateRepository.GetTerminateByEmployee(Id);
            return Ok(new FormatedResponse() { InnerBody = r.Data });
        }
        [HttpPost]
        public async Task<ActionResult> Create([FromBody] TerminateInputDTO param)
        {
            if (!ModelState.IsValid)
            {
                return ResponseValidation();
            }
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
                var property = typeof(TerminateInputDTO).GetProperty("Attachment");

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
            if (param.FileBuffer != null)
            {
                List<UploadFileResponse> uploadFiles2 = new();
                string location = Path.Combine(_env.ContentRootPath, _appSettings.StaticFolders.Root, _appSettings.StaticFolders.Attachments);
                var uploadFileResponse = await _fileService.UploadFile(new UploadRequest()
                {
                    ClientFileName = param.FileBuffer.ClientFileName,
                    ClientFileType = param.FileBuffer.ClientFileType,
                    ClientFileData = param.FileBuffer.ClientFileData
                }, location, sid);

                // Assign saved paths
                var property = typeof(TerminateInputDTO).GetProperty("FileName");

                if (property != null)
                {
                    property?.SetValue(param, uploadFileResponse.SavedAs);
                    uploadFiles2.Add(uploadFileResponse);
                }
                else
                {
                    return Ok(new FormatedResponse() { ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400, MessageCode = CommonMessageCode.ASSIGN_TO_VALUE_DOES_NOT_MATCH_ANY_COLUMN + ": FileBuffer" });
                }

            }

            var r = await _unitOfWork.TerminateRepository.CreateAsync(param);
            if (r.StatusCode == "200" && r.InnerBody != null)
            {
                var rsObjData = (TerminateInputDTO)r.InnerBody;
                var arisingObj = new InsArisingDTO()
                {
                    PkeyRef = rsObjData.Id,
                    EmployeeId = param.EmployeeId,
                    EffectDate = param.EffectDate,
                    TableRef = "TERMINATE"
                };
                var s = await _insArisingRepository.InsertArising(_uow, arisingObj, sid);
                return Ok(new FormatedResponse() { InnerBody = r.Data, MessageCode = CommonMessageCode.CREATE_SUCCESS, StatusCode = EnumStatusCode.StatusCode200 });
            }
            else
            {
                return Ok(new FormatedResponse() { InnerBody = r.Data, MessageCode = r.Error, StatusCode = EnumStatusCode.StatusCode400 });
            }
        }
        [HttpPost]
        public async Task<ActionResult> Update([FromBody] TerminateInputDTO param)
        {
            if (!ModelState.IsValid)
            {
                return ResponseValidation();
            }
            _uow.CreateTransaction();
            bool patchMode = true;
            var sid = Request.Sid(_appSettings);
            List<UploadFileResponse> uploadFiles = new();
            try
            {
                if (param.AttachmentBuffer != null)
                {
                    string location = Path.Combine(_env.ContentRootPath, _appSettings.StaticFolders.Root, _appSettings.StaticFolders.Attachments);
                    var uploadFileResponse = await _fileService.UploadFile(new UploadRequest()
                    {
                        ClientFileName = param.AttachmentBuffer.ClientFileName,
                        ClientFileType = param.AttachmentBuffer.ClientFileType,
                        ClientFileData = param.AttachmentBuffer.ClientFileData
                    }, location, sid);

                    // Assign saved paths
                    var property = typeof(TerminateInputDTO).GetProperty("Attachment");

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
                else
                {
                    var query = (from a in _fullDbContext.HuTerminates.AsNoTracking().Where(x => x.ID == param.Id) select a.ATTACHMENT).FirstOrDefault();
                    param.Attachment = query;
                }
                if (param.FileBuffer != null)
                {
                    string location = Path.Combine(_env.ContentRootPath, _appSettings.StaticFolders.Root, _appSettings.StaticFolders.Attachments);
                    var uploadFileResponse = await _fileService.UploadFile(new UploadRequest()
                    {
                        ClientFileName = param.FileBuffer.ClientFileName,
                        ClientFileType = param.FileBuffer.ClientFileType,
                        ClientFileData = param.FileBuffer.ClientFileData
                    }, location, sid);

                    // Assign saved paths
                    var property = typeof(TerminateInputDTO).GetProperty("FileName");

                    if (property != null)
                    {
                        property?.SetValue(param, uploadFileResponse.SavedAs);
                        uploadFiles.Add(uploadFileResponse);
                    }
                    else
                    {
                        return Ok(new FormatedResponse() { ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400, MessageCode = CommonMessageCode.ASSIGN_TO_VALUE_DOES_NOT_MATCH_ANY_COLUMN + ": FileBuffer" });
                    }

                }
                else
                {
                    var query = (from a in _fullDbContext.HuTerminates.AsNoTracking().Where(x => x.ID == param.Id) select a.FILE_NAME).FirstOrDefault();
                    param.FileName = query;
                }

                var r = await _unitOfWork.TerminateRepository.UpdateAsync(param);
                if (r.StatusCode == "200" && r.InnerBody != null)
                {
                    var rsObjData = (TerminateInputDTO)r.InnerBody;
                    var arisingObj = new InsArisingDTO()
                    {
                        PkeyRef = rsObjData.Id,
                        EmployeeId = param.EmployeeId,
                        EffectDate = param.EffectDate,
                        TableRef = "TERMINATE"
                    };
                    var s = await _insArisingRepository.InsertArising(_uow, arisingObj, sid);
                    return Ok(new FormatedResponse() { InnerBody = r.Data, MessageCode = CommonMessageCode.UPDATE_SUCCESS, StatusCode = EnumStatusCode.StatusCode200 });
                }
                else
                {
                    return Ok(new FormatedResponse() { ErrorType = EnumErrorType.CATCHABLE, MessageCode = r.Error, StatusCode = EnumStatusCode.StatusCode400 });
                }
            }
            catch (Exception ex)
            {
                // Try to delete uploaded files
                uploadFiles.ForEach(async f =>
                {
                    await _fileService.DeleteAttachment(f.SavedAs);
                });

                _uow.Rollback();
                return Ok(new FormatedResponse() { ErrorType = EnumErrorType.UNCATCHABLE, StatusCode = EnumStatusCode.StatusCode500, MessageCode = ex.Message });
            }
            
        }

        [HttpPost]
        public async Task<ActionResult> ChangeStatusApprove(TerminateInputDTO request)
        {
            var sid = Request.Sid(_appSettings);


            foreach (var item in request.Ids)
            {
                var r = _coreDbContext.Terminates.Where(x => x.ID == item).FirstOrDefault();
                var statusIdCur = r.STATUS_ID;
                r.STATUS_ID = request.ValueToBind == true ? OtherConfig.STATUS_APPROVE : (request.ValueToBind == false ? OtherConfig.STATUS_WAITING : null);
                if (statusIdCur != OtherConfig.STATUS_APPROVE && request.ValueToBind == true)
                {
                    var rsApprove = await _unitOfWork.TerminateRepository.Approve(r);
                    var arisingObj = new InsArisingDTO()
                    {
                        PkeyRef = r.ID,
                        EmployeeId = r.EMPLOYEE_ID,
                        EffectDate = r.EFFECT_DATE,
                        TableRef = "TERMINATE"
                    };
                    var s = await _insArisingRepository.InsertArising(_uow, arisingObj, sid);
                }
                var result = _coreDbContext.Terminates.Update(r);
            }
            await _coreDbContext.SaveChangesAsync();
            return Ok(new FormatedResponse() { MessageCode = request.ValueToBind == true ? CommonMessageCode.APPROVE_SUCCESS : (request.ValueToBind == false ? CommonMessageCode.PENDING_APPROVE_SUCCESS : ""), StatusCode = EnumStatusCode.StatusCode200 });

        }

        [HttpPost]
        public async Task<ActionResult> Remove([FromBody] List<long> ids)
        {
            var r = await _unitOfWork.TerminateRepository.RemoveAsync(ids);
            return ResponseResult(r);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(TerminateInputDTO model)
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

        [HttpPost]
        public async Task<IActionResult> DeleteIds(IdsRequest model)
        {
            try
            {
                if (model.Ids != null)
                {
                    var checkStatus = (from p in _coreDbContext.Terminates where model.Ids.Contains(p.ID) && p.STATUS_ID == OtherConfig.STATUS_APPROVE select p).ToList().Count();
                    if (checkStatus > 0)
                    {
                        return Ok(new FormatedResponse() { ErrorType = EnumErrorType.CATCHABLE, MessageCode = CommonMessageCode.DO_NOT_DELETE_DATA_IS_ACTIVE, StatusCode = EnumStatusCode.StatusCode400 });
                    }
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
        [HttpGet]
        public async Task<IActionResult> CalculateSeniority(string? dStart1, string? dStop1)
        {
            try
            {
                var r = await _unitOfWork.TerminateRepository.CalculateSeniority(dStart1.Replace("/", "_"), dStop1.Replace("/", "_"));
                return Ok(new FormatedResponse() { InnerBody = r.Data });

            }
            catch (Exception ex)
            {
                return Ok(new FormatedResponse() { ErrorType = EnumErrorType.UNCATCHABLE, StatusCode = EnumStatusCode.StatusCode500, MessageCode = ex.Message });
            }
        }


        [HttpGet]
        public async Task<IActionResult> Print(long id)
        {
            try
            {
                var QueryData = new SqlQueryDataTemplate(_fullDbContext);

                var fileType = (from p in _fullDbContext.HuTerminates
                                from o in _fullDbContext.SysOtherLists.Where(x => x.ID == p.TYPE_ID).DefaultIfEmpty()
                                where p.ID == id
                                select o).FirstOrDefault();
                ;
                string location = Path.Combine(_env.ContentRootPath, _appSettings.StaticFolders.Root, _appSettings.StaticFolders.WordTemplates);
                string relativePath = "";
                if (fileType.CODE == "CDHDLD")
                {
                    relativePath = "QD_CHHD.doc";
                }
                else if (fileType.CODE == "NH")
                {
                    relativePath = "QD_NH.doc";
                }
                else if (fileType.CODE == "00093")
                {
                    relativePath = "QD_TBNH.doc";
                }
                else
                {
                    return Ok(new FormatedResponse()
                    {
                        ErrorType = EnumErrorType.UNCATCHABLE,
                        MessageCode = "",
                        StatusCode = EnumStatusCode.StatusCode400,
                    });
                }
                var absolutePath = Path.Combine(location, relativePath);
                DataSet dataSet = new DataSet("MyDataSet");
                dataSet = QueryData.ExecuteStoreToTable("PKG_PRINT_LEAVE_JOB", new
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
        public async Task<IActionResult> GetPeroidId(long emp, string lastDate)
        {
            decimal data = 0;
            if (emp != null && lastDate != null)
            {
                DateTime? date = Convert.ToDateTime(lastDate.Replace("_", "/"));
                var query = (from p in _fullDbContext.PaPayrollsheetSums.AsNoTracking()
                                   .Where(x => x.EMPLOYEE_ID == emp && x.FROM_DATE <= date && x.TO_DATE >= date)
                             select p.CLCHINH9).FirstOrDefault();
                if(query != null)
                {
                    data = (decimal)query;
                }
                else
                {
                    data = 0;
                }
            }
            return Ok(new FormatedResponse() { InnerBody = data });
        }

        [HttpGet]
        public async Task<IActionResult> ScanApproveTerminate()
        {
            _unitOfWork.TerminateRepository.ScanApproveTerminate();
            return Ok();
        }
    }
}
