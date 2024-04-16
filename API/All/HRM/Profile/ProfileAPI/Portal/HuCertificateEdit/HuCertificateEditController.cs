using API.All.DbContexts;
using API.DTO;
using API.Main;
using Aspose.Cells.Tables;
using CORE.AutoMapper;
using CORE.DTO;
using CORE.Enum;
using CORE.GenericUOW;
using CORE.Services.File;
using CORE.StaticConstant;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Azure;
using System.Reflection;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;
using CORE.JsonHelper;
using API.Socket;
using Microsoft.AspNetCore.SignalR;

namespace API.Controllers.HuCertificateEdit
{
    [ApiExplorerSettings(GroupName = "079-PROFILE-HU_CERTIFICATE_EDIT")]
    [ApiController]
    [HiStaffAuthorize]
    [Route("api/[controller]/[action]")]
    public class HuCertificateEditController : ControllerBase
    {
        private readonly GenericUnitOfWork _uow;
        private readonly IHuCertificateEditRepository _HuCertificateEditRepository;
        private readonly AppSettings _appSettings;
        private readonly IWebHostEnvironment _env;
        private readonly IFileService _fileService;

        public HuCertificateEditController(
            FullDbContext dbContext,
            IOptions<AppSettings> options,
            IWebHostEnvironment env,
            IFileService fileService, IHubContext<SignalHub> hubContext)
        {
            _uow = new GenericUnitOfWork(dbContext);
            _HuCertificateEditRepository = new HuCertificateEditRepository(dbContext, _uow, hubContext);
            _appSettings = options.Value;
            _env = env;
            _fileService = fileService;
        }

        [HttpGet]
        public async Task<IActionResult> ReadAll()
        {
            var response = await _HuCertificateEditRepository.ReadAll();
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> ReadAllByNumberKey(ReadAllByKeyLongRequest request)
        {
            var response = await _HuCertificateEditRepository.ReadAllByKey(request.Key, request.Value);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> ReadAllByStringKey(ReadAllByKeyStringRequest request)
        {
            var response = await _HuCertificateEditRepository.ReadAllByKey(request.Key, request.Value);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> QueryList(GenericQueryListDTO<HuCertificateEditDTO> request)
        {
            // sử dụng try... catch...
            // để bắt ngoại lệ
            try
            {
                var response = await _HuCertificateEditRepository.SinglePhaseQueryList(request);


                // kiểm tra ErrorType
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
                // in ra ngoại lệ
                return Ok(new FormatedResponse()
                {
                    ErrorType = EnumErrorType.UNCATCHABLE,
                    StatusCode = EnumStatusCode.StatusCode500,
                    MessageCode = ex.Message
                });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetById(long id)
        {
            var response = await _HuCertificateEditRepository.GetById(id);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetByIdWebApp(long id)
        {
            var response = await _HuCertificateEditRepository.GetByIdWebApp(id);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetByStringId(string id)
        {
            var response = await _HuCertificateEditRepository.GetById(id);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Create(HuCertificateEditDTO model)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();


            // thiết lập mặc định
            // cho dữ liệu vào bảng tạm "HU_CERTIFICATE_EDIT"
            // IS_SEND_PORTAL = true
            model.IsSendPortal = true;


            // thiết lập mặc định
            // cho dữ liệu vào bảng tạm "HU_CERTIFICATE_EDIT"
            // IS_APPROVE_PORTAL = false
            // model.IsApprovePortal = false;


            // thiết lập ID_SYS_OTHER_LIST_APPROVE = 993
            model.IdSysOtherListApprove = 993;


            var response = await _HuCertificateEditRepository.Create(_uow, model, sid);


            // sửa lại thông báo
            // nó phải là kiểu gửi duyệt thành công mới đúng
            response.MessageCode = CommonMessageCode.PENDING_APPROVE_SUCCESS;


            return Ok(response);
        }
        [HttpPost]
        public async Task<IActionResult> CreateRange(List<HuCertificateEditDTO> models)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _HuCertificateEditRepository.CreateRange(_uow, models, sid);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Update(HuCertificateEditDTO model)
        {
            /*
                câu lệnh truy vấn để lấy ra trạng thái phê duyệt

                select		b1.ID,
                            b1.CODE,
			                b1.NAME

                from		SYS_OTHER_LIST b1
                left join	SYS_OTHER_LIST_TYPE b2
                on			b1.TYPE_ID = b2.ID

                where		b2.CODE = 'status'


                
                kết quả truy vấn
                993 - CD - "Chờ phê duyệt"
                994 - DD - "Đã phê duyệt"
                995 - TCPD - "Không phê duyệt"
            */


            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _HuCertificateEditRepository.Update(_uow, model, sid);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateRange(List<HuCertificateEditDTO> models)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _HuCertificateEditRepository.UpdateRange(_uow, models, sid);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(HuCertificateEditDTO model)
        {
            if (model.Id != null)
            {
                var response = await _HuCertificateEditRepository.Delete(_uow, (long)model.Id);
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
            var response = await _HuCertificateEditRepository.DeleteIds(_uow, model.Ids);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteStringIds(StringIdsRequest model)
        {
            var response = await _HuCertificateEditRepository.DeleteIds(_uow, model.Ids);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> ApproveHuCertificateEdit(GenericUnapprovePortalDTO request)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _HuCertificateEditRepository.ApproveHuCertificateEdit(request, sid);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetCertificateUnapprove(long employeeId)
        {
            var response = await _HuCertificateEditRepository.GetCertificateUnapprove(employeeId);
            return Ok(response);
        }
        // phương thức
        // gửi yêu cầu phê duyệt sửa bằng cấp/chứng chỉ
        // sửa bản ghi thêm mới (nhưng chưa được duyệt - bảng tạm)
        // sửa bản ghi ở bảng chính (đang submit trong bảng tạm)
        [HttpPost]
        public async Task<IActionResult> SendUpdateHuCertificateEdit(HuCertificateEditDTO model)
        {
            // cái model.Id
            // chính là Id của bảng chính HU_CERTIFICATE
            // chúng ta sẽ cần lưu cái "model.Id"
            // vào cái "ID_HU_CERTIFICATE"
            // của bảng tạm "HU_CERTIFICATE_EDIT"


            // thêm 1 bản ghi mới
            // xong rồi thiết lập
            // IS_SEND_PORTAL = true
            // IS_APPROVE_PORTAL = false


            // lưu tên của các trường bị sửa
            // vào trong "MODEL_CHANGE" dưới dạng chuỗi string


            // ngoài ra còn lưu thêm ID_HU_CERTIFICATE
            // để lúc phê duyệt
            // còn biết sẽ thiết lập vào bản ghi nào
            // trong bảng chính
            // bảng chính là bảng "HU_CERTIFICATE"


            // gọi phương thức SendUpdateHuCertificateEdit()
            var response = await _HuCertificateEditRepository.SendUpdateHuCertificateEdit(model);

            return Ok(response);
        }


        // lấy các bản ghi
        // là loại phê duyệt thêm mới
        // trong bảng tạm HU_CERTIFICATE_EDIT
        // có đặc điểm:
        // 1. IS_SEND_PORTAL = true
        // 2. IS_APPROVE_PORTAL = false
        // 3. ID_HU_CERTIFICATE = null
        [HttpGet]
        public async Task<IActionResult> GetPortalByEmployeeId(long id)
        {
            var response = await _HuCertificateEditRepository.GetPortalByEmployeeId(id);

            return Ok(response);
        }





        /*
            lấy bản ghi theo ID
            mặc định sẽ lấy bản ghi theo ID của bảng HU_CERTIFICATE

            nếu xuất hiện IS_APPROVE_PORTAL = false
            thì lấy bản ghi theo ID của bảng HU_CERTIFICATE_EDIT

            tham số của phương thức GetByIdHuCertificateEdit()
            sẽ phải là HuCertificateEditModel
            bên trong model thì có 2 thuộc tính
            1. long? ID
            2. bool? IS_APPROVE_PORTAL
        */
        [HttpPost]
        public async Task<IActionResult> GetByIdHuCertificateEdit(HuCertificateEditDTO model)
        {
            var response = await _HuCertificateEditRepository.GetByIdHuCertificateEdit(model.Id, model.StatusRecord);

            return Ok(response);
        }



        // khi click button lưu ở portal
        [HttpPost]
        public async Task<IActionResult> ClickSave(HuCertificateEditDTO model)
        {
            // nếu người dùng bấm vào button lưu
            // thì thêm mới 1 bản ghi vào trong bảng tạm HU_CERTIFICATE_EDIT
            // trong đó IS_SEND_PORTAL = null
            // và IS_APPROVE_PORTAL = null
            // để im cho 2 cái này bằng null
            var response = await _HuCertificateEditRepository.ClickSave(model);

            return Ok(response);
        }



        // lấy danh sách trạng thái phê duyệt
        // để làm DropDownList
        [HttpGet]
        public async Task<IActionResult> GetListNameOfApprove()
        {
            var response = await _HuCertificateEditRepository.GetListNameOfApprove();

            return Ok(response);
        }



        // khai báo phương thức
        // để lấy tên của cái trạng thái
        // để phục vụ cho 1 ô input
        [HttpGet]
        public async Task<IActionResult> GetListNameOfApproveById(long id)
        {
            var response = await _HuCertificateEditRepository.GetListNameOfApproveById(id);

            return Ok(response);
        }

        //[HttpGet]
        //public async Task<IActionResult> GetCertificateByEmployee(long id)
        //{
        //    try
        //    {
        //        var entity = _uow.Context.Set<HU_CERTIFICATE>().AsNoTracking().AsQueryable();
        //        var employees = _uow.Context.Set<HU_EMPLOYEE>().AsNoTracking().AsQueryable();
        //        var employeeCvs = _uow.Context.Set<HU_EMPLOYEE_CV>().AsNoTracking().AsQueryable();
        //        var positions = _uow.Context.Set<HU_POSITION>().AsNoTracking().AsQueryable();
        //        var organizations = _uow.Context.Set<HU_ORGANIZATION>().AsNoTracking().AsQueryable();
        //        var otherLists = _uow.Context.Set<SYS_OTHER_LIST>().AsNoTracking().AsQueryable();
        //        var joined = await (from p in entity
        //                            from e in employees.Where(x => x.ID == p.EMPLOYEE_ID).DefaultIfEmpty()
        //                            from cv in employeeCvs.Where(x => x.ID == e.PROFILE_ID).DefaultIfEmpty()
        //                            from t in positions.Where(x => x.ID == e.POSITION_ID).DefaultIfEmpty()
        //                            from o in organizations.Where(x => x.ID == e.ORG_ID).DefaultIfEmpty()
        //                            from type in otherLists.Where(x => x.ID == p.TYPE_CERTIFICATE).DefaultIfEmpty()
        //                            from lvtrain in otherLists.Where(x => x.ID == p.LEVEL_TRAIN).DefaultIfEmpty()
        //                            from typeTrain in otherLists.Where(x => x.ID == p.TYPE_TRAIN).DefaultIfEmpty()
        //                            from school in otherLists.Where(x => x.ID == p.SCHOOL_ID).DefaultIfEmpty()
        //                            from lv in otherLists.Where(x => x.ID == p.LEVEL_ID).DefaultIfEmpty()
        //                            where p.EMPLOYEE_ID == id
        //                            select new HuCertificateDTO
        //                            {
        //                                Id = p.ID,
        //                                EmployeeCode = e.CODE,
        //                                EmployeeFullName = cv.FULL_NAME,
        //                                OrgName = o.NAME,
        //                                TitleName = t.NAME,
        //                                TypeCertificateName = type.NAME,
        //                                Name = p.NAME,
        //                                TrainFromDate = p.TRAIN_FROM_DATE,
        //                                TrainToDate = p.TRAIN_TO_DATE,
        //                                EffectFrom = p.EFFECT_FROM,
        //                                LevelTrainName = lvtrain.NAME,
        //                                Major = p.MAJOR,
        //                                ContentTrain = p.CONTENT_TRAIN,
        //                                SchoolName = school.NAME,
        //                                Year = p.YEAR,
        //                                Mark = p.MARK,
        //                                IsPrime = p.IS_PRIME,
        //                                Level = p.LEVEL,
        //                                TypeTrainName = typeTrain.NAME,
        //                                Remark = p.REMARK,
        //                                LevelId = p.LEVEL_ID,
        //                                LevelName = lv.NAME,
        //                                OrgId = e.ORG_ID,
        //                                IsPrimeStr = p.IS_PRIME == true ? "Là b?ng chính" : "Không là b?ng chính",
        //                                Classification = p.CLASSIFICATION
        //                            }).ToListAsync();
        //        var response = new FormatedResponse() { InnerBody = joined };
        //        return Ok(response);
        //    }
        //    catch (Exception ex)
        //    {
        //        return Ok(new FormatedResponse() { ErrorType = EnumErrorType.UNCATCHABLE, StatusCode = EnumStatusCode.StatusCode500, MessageCode = ex.Message });
        //    }
        //}




        [HttpGet]
        public async Task<IActionResult> GetCertificateIsApprove(long employeeId)
        {//cho phe duyet
            try
            {
                var entity = _uow.Context.Set<HU_CERTIFICATE_EDIT>().AsNoTracking().AsQueryable();
                var employees = _uow.Context.Set<HU_EMPLOYEE>().AsNoTracking().AsQueryable();
                var employeeCvs = _uow.Context.Set<HU_EMPLOYEE_CV>().AsNoTracking().AsQueryable();
                var positions = _uow.Context.Set<HU_POSITION>().AsNoTracking().AsQueryable();
                var organizations = _uow.Context.Set<HU_ORGANIZATION>().AsNoTracking().AsQueryable();
                var otherLists = _uow.Context.Set<SYS_OTHER_LIST>().AsNoTracking().AsQueryable();
                var joined = await (from p in entity
                                    from e in employees.Where(x => x.ID == p.EMPLOYEE_ID).DefaultIfEmpty()
                                    from cv in employeeCvs.Where(x => x.ID == e.PROFILE_ID).DefaultIfEmpty()
                                    from t in positions.Where(x => x.ID == e.POSITION_ID).DefaultIfEmpty()
                                    from o in organizations.Where(x => x.ID == e.ORG_ID).DefaultIfEmpty()
                                    from type in otherLists.Where(x => x.ID == p.TYPE_CERTIFICATE).DefaultIfEmpty()
                                    from lvtrain in otherLists.Where(x => x.ID == p.LEVEL_TRAIN).DefaultIfEmpty()
                                    from typeTrain in otherLists.Where(x => x.ID == p.TYPE_TRAIN).DefaultIfEmpty()
                                    from school in otherLists.Where(x => x.ID == p.SCHOOL_ID).DefaultIfEmpty()
                                    from lv in otherLists.Where(x => x.ID == p.LEVEL_ID).DefaultIfEmpty()
                                    where p.EMPLOYEE_ID == employeeId && p.IS_SEND_PORTAL == true && p.IS_APPROVE_PORTAL == false
                                    select new HuCertificateEditDTO
                                    {
                                        Id = p.ID,
                                        EmployeeCode = e.CODE,
                                        EmployeeId = p.EMPLOYEE_ID,
                                        EmployeeFullName = cv.FULL_NAME,
                                        OrgName = o.NAME,
                                        TitleName = t.NAME,
                                        TypeCertificate = p.TYPE_CERTIFICATE,
                                        TypeCertificateName = type.NAME,
                                        Name = p.NAME,
                                        TrainFromDate = p.TRAIN_FROM_DATE,
                                        TrainToDate = p.TRAIN_TO_DATE,
                                        EffectFrom = p.EFFECT_FROM,
                                        LevelTrain = p.LEVEL_TRAIN,
                                        LevelTrainName = lvtrain.NAME,
                                        Major = p.MAJOR,
                                        ContentTrain = p.CONTENT_TRAIN,
                                        SchoolId = p.SCHOOL_ID,
                                        SchoolName = school.NAME,
                                        Year = p.YEAR,
                                        Mark = p.MARK,
                                        IsPrime = p.IS_PRIME,
                                        Level = p.LEVEL,
                                        TypeTrain = p.TYPE_TRAIN,
                                        TypeTrainName = typeTrain.NAME,
                                        Remark = p.REMARK,
                                        LevelId = p.LEVEL_ID,
                                        LevelName = lv.NAME,
                                        OrgId = e.ORG_ID,
                                        Classification = p.CLASSIFICATION
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
        public async Task<IActionResult> GetCertificateIsSave(long employeeId)
        {
            try
            {
                var entity = _uow.Context.Set<HU_CERTIFICATE_EDIT>().AsNoTracking().AsQueryable();
                var employees = _uow.Context.Set<HU_EMPLOYEE>().AsNoTracking().AsQueryable();
                var employeeCvs = _uow.Context.Set<HU_EMPLOYEE_CV>().AsNoTracking().AsQueryable();
                var positions = _uow.Context.Set<HU_POSITION>().AsNoTracking().AsQueryable();
                var organizations = _uow.Context.Set<HU_ORGANIZATION>().AsNoTracking().AsQueryable();
                var otherLists = _uow.Context.Set<SYS_OTHER_LIST>().AsNoTracking().AsQueryable();
                var joined = await (from p in entity
                                    from e in employees.Where(x => x.ID == p.EMPLOYEE_ID).DefaultIfEmpty()
                                    from cv in employeeCvs.Where(x => x.ID == e.PROFILE_ID).DefaultIfEmpty()
                                    from t in positions.Where(x => x.ID == e.POSITION_ID).DefaultIfEmpty()
                                    from o in organizations.Where(x => x.ID == e.ORG_ID).DefaultIfEmpty()
                                    from type in otherLists.Where(x => x.ID == p.TYPE_CERTIFICATE).DefaultIfEmpty()
                                    from lvtrain in otherLists.Where(x => x.ID == p.LEVEL_TRAIN).DefaultIfEmpty()
                                    from typeTrain in otherLists.Where(x => x.ID == p.TYPE_TRAIN).DefaultIfEmpty()
                                    from school in otherLists.Where(x => x.ID == p.SCHOOL_ID).DefaultIfEmpty()
                                    from lv in otherLists.Where(x => x.ID == p.LEVEL_ID).DefaultIfEmpty()
                                    where p.EMPLOYEE_ID == employeeId && p.IS_SAVE_PORTAL == true
                                    select new HuCertificateEditDTO
                                    {
                                        Id = p.ID,
                                        EmployeeCode = e.CODE,
                                        EmployeeId = p.EMPLOYEE_ID,
                                        EmployeeFullName = cv.FULL_NAME,
                                        OrgName = o.NAME,
                                        TitleName = t.NAME,
                                        TypeCertificate = p.TYPE_CERTIFICATE,
                                        TypeCertificateName = type.NAME,
                                        Name = p.NAME,
                                        TrainFromDate = p.TRAIN_FROM_DATE,
                                        TrainToDate = p.TRAIN_TO_DATE,
                                        EffectFrom = p.EFFECT_FROM,
                                        LevelTrain = p.LEVEL_TRAIN,
                                        LevelTrainName = lvtrain.NAME,
                                        Major = p.MAJOR,
                                        ContentTrain = p.CONTENT_TRAIN,
                                        SchoolId = p.SCHOOL_ID,
                                        SchoolName = school.NAME,
                                        Year = p.YEAR,
                                        Mark = p.MARK,
                                        IsPrime = p.IS_PRIME,
                                        Level = p.LEVEL,
                                        TypeTrain = p.TYPE_TRAIN,
                                        TypeTrainName = typeTrain.NAME,
                                        Remark = p.REMARK,
                                        LevelId = p.LEVEL_ID,
                                        LevelName = lv.NAME,
                                        OrgId = e.ORG_ID,
                                        Classification = p.CLASSIFICATION,
                                        FileName = p.FILE_NAME != null ? "File Upload" : "",
                                        IdHuCertificate = p.ID_HU_CERTIFICATE
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
        public async Task<IActionResult> GetCertificateById(long id)
        {
            var response = await _HuCertificateEditRepository.GetCertificateById(id);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetCertificateEditSaveById(long id) 
        {
            var response = await _HuCertificateEditRepository.GetCertificateEditSaveById(id);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Save(HuCertificateEditDTO dto)
        {
            try
            {
                var sid = Request.Sid(_appSettings);
                bool pathMode = true;
                if (sid == null) return Unauthorized();
                var getData = _uow.Context.Set<HU_CERTIFICATE_EDIT>().Where(x => x.EMPLOYEE_ID == dto.EmployeeId && x.IS_SAVE_PORTAL == true);
                if(dto.Id != null && dto.IsSavePortal == true)
                {
                    List<UploadFileResponse> uploadFiles1 = new();
                    // First of all we need to upload all the attachments
                    if (dto.FirstAttachmentBuffer != null)
                    {
                        string location = Path.Combine(_env.ContentRootPath, _appSettings.StaticFolders.Root, _appSettings.StaticFolders.Attachments);
                        var uploadFileResponse = await _fileService.UploadFile(new UploadRequest()
                        {
                            ClientFileName = dto.FirstAttachmentBuffer.ClientFileName,
                            ClientFileType = dto.FirstAttachmentBuffer.ClientFileType,
                            ClientFileData = dto.FirstAttachmentBuffer.ClientFileData
                        }, location, sid);

                        // Assign saved paths
                        var property = typeof(HuCertificateEditDTO).GetProperty("FileName");

                        if (property != null)
                        {
                            property?.SetValue(dto, uploadFileResponse.SavedAs);
                            uploadFiles1.Add(uploadFileResponse);
                        }
                        else
                        {
                            return Ok(new FormatedResponse()
                            {
                                ErrorType = EnumErrorType.CATCHABLE,
                                StatusCode = EnumStatusCode.StatusCode400,
                                MessageCode = CommonMessageCode.ASSIGN_TO_VALUE_DOES_NOT_MATCH_ANY_COLUMN + ": FirstAttachment"
                            });
                        }
                    }

                    var updateResponse = await _HuCertificateEditRepository.Update(_uow, dto, sid , pathMode);
                    return Ok(updateResponse);
                }
                if(dto.Id != null && dto.IsSavePortal == false)
                {
                    dto.IsSavePortal = true;

                    List<UploadFileResponse> uploadFiles1 = new();
                    // First of all we need to upload all the attachments
                    if (dto.FirstAttachmentBuffer != null)
                    {
                        string location = Path.Combine(_env.ContentRootPath, _appSettings.StaticFolders.Root, _appSettings.StaticFolders.Attachments);
                        var uploadFileResponse = await _fileService.UploadFile(new UploadRequest()
                        {
                            ClientFileName = dto.FirstAttachmentBuffer.ClientFileName,
                            ClientFileType = dto.FirstAttachmentBuffer.ClientFileType,
                            ClientFileData = dto.FirstAttachmentBuffer.ClientFileData
                        }, location, sid);

                        // Assign saved paths
                        var property = typeof(HuCertificateEditDTO).GetProperty("FileName");

                        if (property != null)
                        {
                            property?.SetValue(dto, uploadFileResponse.SavedAs);
                            uploadFiles1.Add(uploadFileResponse);
                        }
                        else
                        {
                            return Ok(new FormatedResponse()
                            {
                                ErrorType = EnumErrorType.CATCHABLE,
                                StatusCode = EnumStatusCode.StatusCode400,
                                MessageCode = CommonMessageCode.ASSIGN_TO_VALUE_DOES_NOT_MATCH_ANY_COLUMN + ": FirstAttachment"
                            });
                        }
                    }

                    var updateResponse = await _HuCertificateEditRepository.Update(_uow, dto, sid, pathMode);
                    return Ok(updateResponse);
                }
                if (getData.Any())
                {
                    return Ok(new FormatedResponse() { StatusCode = EnumStatusCode.StatusCode400, ErrorType = EnumErrorType.CATCHABLE, MessageCode = CommonMessageCode.HAD_RECORD_IS_SAVE });
                }
                if (dto.Id == null)
                {
                    dto.IsSavePortal = true;

                    List<UploadFileResponse> uploadFiles1 = new();
                    // First of all we need to upload all the attachments
                    if (dto.FirstAttachmentBuffer != null)
                    {
                        string location = Path.Combine(_env.ContentRootPath, _appSettings.StaticFolders.Root, _appSettings.StaticFolders.Attachments);
                        var uploadFileResponse = await _fileService.UploadFile(new UploadRequest()
                        {
                            ClientFileName = dto.FirstAttachmentBuffer.ClientFileName,
                            ClientFileType = dto.FirstAttachmentBuffer.ClientFileType,
                            ClientFileData = dto.FirstAttachmentBuffer.ClientFileData
                        }, location, sid);

                        // Assign saved paths
                        var property = typeof(HuCertificateEditDTO).GetProperty("FileName");

                        if (property != null)
                        {
                            property?.SetValue(dto, uploadFileResponse.SavedAs);
                            uploadFiles1.Add(uploadFileResponse);
                        }
                        else
                        {
                            return Ok(new FormatedResponse()
                            {
                                ErrorType = EnumErrorType.CATCHABLE,
                                StatusCode = EnumStatusCode.StatusCode400,
                                MessageCode = CommonMessageCode.ASSIGN_TO_VALUE_DOES_NOT_MATCH_ANY_COLUMN + ": FirstAttachment"
                            });
                        }
                    }

                    var createResponse = await _HuCertificateEditRepository.Create(_uow, dto, sid);
                    return Ok(createResponse);
                }

                dto.IsSavePortal = true;
                dto.IdHuCertificate = dto.Id;
                dto.Id = null;

                List<UploadFileResponse> uploadFiles = new();
                // First of all we need to upload all the attachments
                if (dto.FirstAttachmentBuffer != null)
                {
                    string location = Path.Combine(_env.ContentRootPath, _appSettings.StaticFolders.Root, _appSettings.StaticFolders.Attachments);
                    var uploadFileResponse = await _fileService.UploadFile(new UploadRequest()
                    {
                        ClientFileName = dto.FirstAttachmentBuffer.ClientFileName,
                        ClientFileType = dto.FirstAttachmentBuffer.ClientFileType,
                        ClientFileData = dto.FirstAttachmentBuffer.ClientFileData
                    }, location, sid);

                    // Assign saved paths
                    var property = typeof(HuCertificateEditDTO).GetProperty("FileName");

                    if (property != null)
                    {
                        property?.SetValue(dto, uploadFileResponse.SavedAs);
                        uploadFiles.Add(uploadFileResponse);
                    }
                    else
                    {
                        return Ok(new FormatedResponse()
                        {
                            ErrorType = EnumErrorType.CATCHABLE,
                            StatusCode = EnumStatusCode.StatusCode400,
                            MessageCode = CommonMessageCode.ASSIGN_TO_VALUE_DOES_NOT_MATCH_ANY_COLUMN + ": FirstAttachment"
                        });
                    }
                }

                var response = await _HuCertificateEditRepository.Create(_uow, dto, sid);
                return Ok(response);
                




                //if(dto.Id == null)
                //{
                //    dto.IsSavePortal = true;
                //    List<UploadFileResponse> uploadFiles = new();
                //    // First of all we need to upload all the attachments
                //    if (dto.FirstAttachmentBuffer != null)
                //    {
                //        string location = Path.Combine(_env.ContentRootPath, _appSettings.StaticFolders.Root, _appSettings.StaticFolders.Attachments);
                //        var uploadFileResponse = await _fileService.UploadFile(new UploadRequest()
                //        {
                //            ClientFileName = dto.FirstAttachmentBuffer.ClientFileName,
                //            ClientFileType = dto.FirstAttachmentBuffer.ClientFileType,
                //            ClientFileData = dto.FirstAttachmentBuffer.ClientFileData
                //        }, location, sid);

                //        // Assign saved paths
                //        var property = typeof(HuCertificateEditDTO).GetProperty("FileName");

                //        if (property != null)
                //        {
                //            property?.SetValue(dto, uploadFileResponse.SavedAs);
                //            uploadFiles.Add(uploadFileResponse);
                //        }
                //        else
                //        {
                //            return Ok(new FormatedResponse() { StatusCode = EnumStatusCode.StatusCode400, MessageCode = CommonMessageCode.ASSIGN_TO_VALUE_DOES_NOT_MATCH_ANY_COLUMN + ": FirstAttachment" });
                //        }

                //    }
                //    var response = await _HuCertificateEditRepository.Create(_uow, dto, sid);
                //    _uow.Commit();
                //    return Ok(response);

                //}
                //else
                //{

                //    dto.IdHuCertificate = dto.Id;
                //    dto.Id = 0;
                //    dto.IsSavePortal = true;
                //    List<UploadFileResponse> uploadFiles = new();
                //    // First of all we need to upload all the attachments
                //    if (dto.FirstAttachmentBuffer != null)
                //    {
                //        string location = Path.Combine(_env.ContentRootPath, _appSettings.StaticFolders.Root, _appSettings.StaticFolders.Attachments);
                //        var uploadFileResponse = await _fileService.UploadFile(new UploadRequest()
                //        {
                //            ClientFileName = dto.FirstAttachmentBuffer.ClientFileName,
                //            ClientFileType = dto.FirstAttachmentBuffer.ClientFileType,
                //            ClientFileData = dto.FirstAttachmentBuffer.ClientFileData
                //        }, location, sid);

                //        // Assign saved paths
                //        var property = typeof(HuCertificateEditDTO).GetProperty("FileName");

                //        if (property != null)
                //        {
                //            property?.SetValue(dto, uploadFileResponse.SavedAs);
                //            uploadFiles.Add(uploadFileResponse);
                //        }
                //        else
                //        {
                //            return Ok(new FormatedResponse() { StatusCode = EnumStatusCode.StatusCode400, MessageCode = CommonMessageCode.ASSIGN_TO_VALUE_DOES_NOT_MATCH_ANY_COLUMN + ": FirstAttachment" });
                //        }

                //    }
                //    var response = await _HuCertificateEditRepository.Create(_uow, dto, sid);
                //    _uow.Commit();
                //    return Ok(response);
                //}
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }

        }


        [HttpGet]
        public async Task<IActionResult> GetListCertificate(long employeeId)
        {
            var response = await _HuCertificateEditRepository.GetListCertificate(employeeId);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetByIdCertificate(long id)
        {
            var response = await _HuCertificateEditRepository.GetByIdCertificate(id);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetCertificateApproving(long employeeId)
        {
            var response = await _HuCertificateEditRepository.GetCertificateApproving(employeeId);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> SendApproveCertificate(HuCertificateEditDTO request)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            bool pathMode = true;
            var getOtherList = (from t in _uow.Context.Set<SYS_OTHER_LIST_TYPE>().Where(x => x.CODE == "STATUS")
                                from o in _uow.Context.Set<SYS_OTHER_LIST>().Where(x => x.CODE == "CD")
                                select new { Id = o.ID }).FirstOrDefault();
            var getData = _uow.Context.Set<HU_CERTIFICATE_EDIT>().Where(x => x.EMPLOYEE_ID == request.EmployeeId && x.IS_SEND_PORTAL == true && x.IS_APPROVE_PORTAL == false);
            if (getData.Any())
            {
                return Ok(new FormatedResponse() { StatusCode = EnumStatusCode.StatusCode400, MessageCode = CommonMessageCode.HAD_RECORD_IS_APPROVING });
;           }
            if(request.Id != null && request.IsSavePortal == true) //gửi duyệt bản ghi đã lưu
            {
                request.IsSendPortal = true;
                request.IsSavePortal = false;
                request.IsApprovePortal = false;
                request.StatusId = getOtherList?.Id;
                if (request.IdHuCertificate != null)
                {
                    var certificate = _uow.Context.Set<HU_CERTIFICATE>().Where(x => x.ID == request.IdHuCertificate).AsNoTracking().AsQueryable();
                    List<string> listModelChange = new List<string>();
                    if (certificate != null)
                    {
                        var entityType = typeof(HU_EMPLOYEE_CV);
                        var dtoType = typeof(HuEmployeeCvDTO);
                        var entityPropList = entityType.GetProperties().ToList();
                        var dtoPropList = dtoType.GetProperties().ToList();

                        var query = Activator.CreateInstance(dtoType);


                        entityPropList.ForEach(prop =>
                        {
                            var value = prop.GetValue(certificate);
                            var dtoProp = dtoPropList.SingleOrDefault(x => x.Name == prop.Name.SnakeToCamelCase().CamelToPascalCase());
                            dtoProp?.SetValue(query, value);

                        });


                        if (query != null)
                        {
                            Type type = query.GetType();
                            Type type2 = request.GetType();
                            IList<PropertyInfo> props = new List<PropertyInfo>(type.GetProperties());
                            IList<PropertyInfo> prop2s = new List<PropertyInfo>(type2.GetProperties());
                            foreach (PropertyInfo prop in props)
                            {
                                foreach (PropertyInfo prop2 in prop2s)
                                {

                                    if (prop.Name != "Id" && prop.Name != null && prop.Name == prop2.Name && prop.GetValue(query) != null && prop2.GetValue(request) != null && prop.GetValue(query)!.ToString() != prop2.GetValue(request)!.ToString())
                                    {
                                        listModelChange.Add(Char.ToLowerInvariant(prop.Name[0]) + prop.Name.Substring(1));
                                    }

                                }
                            }
                            request.ModelChange = string.Join(";", listModelChange);
                        }
                    }
                }
                

                List<UploadFileResponse> uploadFiles1 = new();
                // First of all we need to upload all the attachments
                if (request.FirstAttachmentBuffer != null)
                {
                    string location = Path.Combine(_env.ContentRootPath, _appSettings.StaticFolders.Root, _appSettings.StaticFolders.Attachments);
                    var uploadFileResponse = await _fileService.UploadFile(new UploadRequest()
                    {
                        ClientFileName = request.FirstAttachmentBuffer.ClientFileName,
                        ClientFileType = request.FirstAttachmentBuffer.ClientFileType,
                        ClientFileData = request.FirstAttachmentBuffer.ClientFileData
                    }, location, sid);

                    // Assign saved paths
                    var property = typeof(HuCertificateEditDTO).GetProperty("FileName");

                    if (property != null)
                    {
                        property?.SetValue(request, uploadFileResponse.SavedAs);
                        uploadFiles1.Add(uploadFileResponse);
                    }
                    else
                    {
                        return Ok(new FormatedResponse()
                        {
                            ErrorType = EnumErrorType.CATCHABLE,
                            StatusCode = EnumStatusCode.StatusCode400,
                            MessageCode = CommonMessageCode.ASSIGN_TO_VALUE_DOES_NOT_MATCH_ANY_COLUMN + ": FirstAttachment"
                        });
                    }
                }

                var updateResponse = await _HuCertificateEditRepository.Update(_uow, request, sid, pathMode);
                return Ok(updateResponse);  
            }
            if(request.Id != null && request.IsSavePortal == false) //gửi duyệt bản ghi đã bị từ chối
            {
                request.IsSendPortal = true;
                request.IsApprovePortal = false;
                request.StatusId = getOtherList?.Id;
                

                var certificate = _uow.Context.Set<HU_CERTIFICATE>().Where(x => x.ID == request.IdHuCertificate).AsNoTracking().AsQueryable();
                List<string> listModelChange = new List<string>();
                if (certificate != null)
                {
                    var entityType = typeof(HU_EMPLOYEE_CV);
                    var dtoType = typeof(HuEmployeeCvDTO);
                    var entityPropList = entityType.GetProperties().ToList();
                    var dtoPropList = dtoType.GetProperties().ToList();

                    var query = Activator.CreateInstance(dtoType);


                    entityPropList.ForEach(prop =>
                    {
                        var value = prop.GetValue(certificate);
                        var dtoProp = dtoPropList.SingleOrDefault(x => x.Name == prop.Name.SnakeToCamelCase().CamelToPascalCase());
                        dtoProp?.SetValue(query, value);

                    });


                    if (query != null)
                    {
                        Type type = query.GetType();
                        Type type2 = request.GetType();
                        IList<PropertyInfo> props = new List<PropertyInfo>(type.GetProperties());
                        IList<PropertyInfo> prop2s = new List<PropertyInfo>(type2.GetProperties());
                        foreach (PropertyInfo prop in props)
                        {
                            foreach (PropertyInfo prop2 in prop2s)
                            {

                                if (prop.Name != "Id" && prop.Name != null && prop.Name == prop2.Name && prop.GetValue(query) != null && prop2.GetValue(request) != null && prop.GetValue(query)!.ToString() != prop2.GetValue(request)!.ToString())
                                {
                                    listModelChange.Add(Char.ToLowerInvariant(prop.Name[0]) + prop.Name.Substring(1));
                                }

                            }
                        }
                        request.ModelChange = string.Join(";", listModelChange);
                    }
                }
                List<UploadFileResponse> uploadFiles1 = new();
                // First of all we need to upload all the attachments
                if (request.FirstAttachmentBuffer != null)
                {
                    string location = Path.Combine(_env.ContentRootPath, _appSettings.StaticFolders.Root, _appSettings.StaticFolders.Attachments);
                    var uploadFileResponse = await _fileService.UploadFile(new UploadRequest()
                    {
                        ClientFileName = request.FirstAttachmentBuffer.ClientFileName,
                        ClientFileType = request.FirstAttachmentBuffer.ClientFileType,
                        ClientFileData = request.FirstAttachmentBuffer.ClientFileData
                    }, location, sid);

                    // Assign saved paths
                    var property = typeof(HuCertificateEditDTO).GetProperty("FileName");

                    if (property != null)
                    {
                        property?.SetValue(request, uploadFileResponse.SavedAs);
                        uploadFiles1.Add(uploadFileResponse);
                    }
                    else
                    {
                        return Ok(new FormatedResponse()
                        {
                            ErrorType = EnumErrorType.CATCHABLE,
                            StatusCode = EnumStatusCode.StatusCode400,
                            MessageCode = CommonMessageCode.ASSIGN_TO_VALUE_DOES_NOT_MATCH_ANY_COLUMN + ": FirstAttachment"
                        });
                    }
                }

                var updateResponse = await _HuCertificateEditRepository.Update(_uow, request, sid, pathMode);
                return Ok(updateResponse);
            }
            if(request.Id == null || request.Id == 0) //thêm mới bản ghi và gửi duyệt
            {
                request.IsSendPortal = true;
                request.IsSavePortal = false;
                request.IsApprovePortal = false;
                request.StatusId = getOtherList?.Id;

                List<UploadFileResponse> uploadFiles1 = new();
                // First of all we need to upload all the attachments
                if (request.FirstAttachmentBuffer != null)
                {
                    string location = Path.Combine(_env.ContentRootPath, _appSettings.StaticFolders.Root, _appSettings.StaticFolders.Attachments);
                    var uploadFileResponse = await _fileService.UploadFile(new UploadRequest()
                    {
                        ClientFileName = request.FirstAttachmentBuffer.ClientFileName,
                        ClientFileType = request.FirstAttachmentBuffer.ClientFileType,
                        ClientFileData = request.FirstAttachmentBuffer.ClientFileData
                    }, location, sid);

                    // Assign saved paths
                    var property = typeof(HuCertificateEditDTO).GetProperty("FileName");

                    if (property != null)
                    {
                        property?.SetValue(request, uploadFileResponse.SavedAs);
                        uploadFiles1.Add(uploadFileResponse);
                    }
                    else
                    {
                        return Ok(new FormatedResponse()
                        {
                            ErrorType = EnumErrorType.CATCHABLE,
                            StatusCode = EnumStatusCode.StatusCode400,
                            MessageCode = CommonMessageCode.ASSIGN_TO_VALUE_DOES_NOT_MATCH_ANY_COLUMN + ": FirstAttachment"
                        });
                    }
                }

                var createResponse = await _HuCertificateEditRepository.Create(_uow, request, sid);
                return Ok(createResponse);
            }

            request.IsSendPortal = true;
            request.IsSavePortal = false;
            request.IsApprovePortal = false;
            request.StatusId = getOtherList?.Id;
            //request.IdHuCertificate = request.Id;
            request.Id = null;

            if (request.IdHuCertificate != null)
            {
                var certificate = _uow.Context.Set<HU_CERTIFICATE>().Where(x => x.ID == request.IdHuCertificate).AsNoTracking().AsQueryable();
                List<string> listModelChange = new List<string>();
                if (certificate != null)
                {
                    var entityType = typeof(HU_EMPLOYEE_CV);
                    var dtoType = typeof(HuEmployeeCvDTO);
                    var entityPropList = entityType.GetProperties().ToList();
                    var dtoPropList = dtoType.GetProperties().ToList();

                    var query = Activator.CreateInstance(dtoType);


                    entityPropList.ForEach(prop =>
                    {
                        var value = prop.GetValue(certificate);
                        var dtoProp = dtoPropList.SingleOrDefault(x => x.Name == prop.Name.SnakeToCamelCase().CamelToPascalCase());
                        dtoProp?.SetValue(query, value);

                    });


                    if (query != null)
                    {
                        Type type = query.GetType();
                        Type type2 = request.GetType();
                        IList<PropertyInfo> props = new List<PropertyInfo>(type.GetProperties());
                        IList<PropertyInfo> prop2s = new List<PropertyInfo>(type2.GetProperties());
                        foreach (PropertyInfo prop in props)
                        {
                            foreach (PropertyInfo prop2 in prop2s)
                            {

                                if (prop.Name != "Id" && prop.Name != null && prop.Name == prop2.Name && prop.GetValue(query) != null && prop2.GetValue(request) != null && prop.GetValue(query)!.ToString() != prop2.GetValue(request)!.ToString())
                                {
                                    listModelChange.Add(Char.ToLowerInvariant(prop.Name[0]) + prop.Name.Substring(1));
                                }

                            }
                        }
                        request.ModelChange = string.Join(";", listModelChange);
                    }
                }
            }
            List<UploadFileResponse> uploadFiles = new();
            // First of all we need to upload all the attachments
            if (request.FirstAttachmentBuffer != null)
            {
                string location = Path.Combine(_env.ContentRootPath, _appSettings.StaticFolders.Root, _appSettings.StaticFolders.Attachments);
                var uploadFileResponse = await _fileService.UploadFile(new UploadRequest()
                {
                    ClientFileName = request.FirstAttachmentBuffer.ClientFileName,
                    ClientFileType = request.FirstAttachmentBuffer.ClientFileType,
                    ClientFileData = request.FirstAttachmentBuffer.ClientFileData
                }, location, sid);

                // Assign saved paths
                var property = typeof(HuCertificateEditDTO).GetProperty("FileName");

                if (property != null)
                {
                    property?.SetValue(request, uploadFileResponse.SavedAs);
                    uploadFiles.Add(uploadFileResponse);
                }
                else
                {
                    return Ok(new FormatedResponse()
                    {
                        ErrorType = EnumErrorType.CATCHABLE,
                        StatusCode = EnumStatusCode.StatusCode400,
                        MessageCode = CommonMessageCode.ASSIGN_TO_VALUE_DOES_NOT_MATCH_ANY_COLUMN + ": FirstAttachment"
                    });
                }
            }


            var response = await _HuCertificateEditRepository.Create(_uow, request, sid);
            return Ok(response);

            //request.IsSendPortal = true;
            //request.IsSavePortal = false;
            //request.IsApprovePortal = false;
            //request.StatusId = getOtherList?.Id;


            ////List<string> listModelChange = new List<string>();
            ////HuCertificateDTO query = new HuCertificateDTO();
            ////HU_CERTIFICATE entity = new HU_CERTIFICATE();


            //if (request.Id != null)
            //{
            //    //if (request.IdHuCertificate != null)
            //    //{
            //    //    entity = _uow.Context.Set<HU_CERTIFICATE>().Where(x => x.ID == request.IdHuCertificate).FirstOrDefault()!;
            //    //}
            //    //else
            //    //{
            //    //    entity = _uow.Context.Set<HU_CERTIFICATE>().Where(x => x.ID == request.Id).FirstOrDefault()!;
            //    //}


            //    //if (entity != null)
            //    //{

            //    //    query = CoreMapper<HuCertificateDTO, HU_CERTIFICATE>.EntityToDto(entity, new HuCertificateDTO());

            //    //    if (query != null)
            //    //    {
            //    //        Type type = request.GetType();
            //    //        Type type2 = query.GetType();
            //    //        if (request != null)
            //    //        {
            //    //            IList<PropertyInfo> props = new List<PropertyInfo>(type.GetProperties());
            //    //            IList<PropertyInfo> prop2s = new List<PropertyInfo>(type2.GetProperties());

            //    //            foreach (PropertyInfo prop in props)
            //    //            {
            //    //                foreach (PropertyInfo prop2 in prop2s)
            //    //                {

            //    //                    if (prop.Name != "Id" && prop.Name != null && prop.Name == prop2.Name && prop.GetValue(request) != null && prop2.GetValue(query) != null && prop.GetValue(request)!.ToString() != prop2.GetValue(query)!.ToString())
            //    //                    {
            //    //                        listModelChange.Add(Char.ToLowerInvariant(prop.Name[0]) + prop.Name.Substring(1));
            //    //                    }

            //    //                }
            //    //            }
            //    //            request.ModelChange = string.Join(";", listModelChange);
            //    //        }
            //    //    }
            //    //}

            //    //request.IdHuCertificate = request.Id;
            //    //request.IdHuCertificate = request.IdHuCertificate;
            //    //request.Id = null;
            //    List<UploadFileResponse> uploadFiles = new();
            //    // First of all we need to upload all the attachments
            //    if (request.FirstAttachmentBuffer != null)
            //    {
            //        string location = Path.Combine(_env.ContentRootPath, _appSettings.StaticFolders.Root, _appSettings.StaticFolders.Attachments);
            //        var uploadFileResponse = await _fileService.UploadFile(new UploadRequest()
            //        {
            //            ClientFileName = request.FirstAttachmentBuffer.ClientFileName,
            //            ClientFileType = request.FirstAttachmentBuffer.ClientFileType,
            //            ClientFileData = request.FirstAttachmentBuffer.ClientFileData
            //        }, location, sid);

            //        // Assign saved paths
            //        var property = typeof(HuCertificateEditDTO).GetProperty("FileName");

            //        if (property != null)
            //        {
            //            property?.SetValue(request, uploadFileResponse.SavedAs);
            //            uploadFiles.Add(uploadFileResponse);
            //        }
            //        else
            //        {
            //            return Ok(new FormatedResponse() { StatusCode = EnumStatusCode.StatusCode400, MessageCode = CommonMessageCode.ASSIGN_TO_VALUE_DOES_NOT_MATCH_ANY_COLUMN + ": FirstAttachment" });
            //        }

            //    }
            //    var response = await _HuCertificateEditRepository.Update(_uow, request, sid);
            //    return Ok(response);
        }
        //else
        //{
        //    List<UploadFileResponse> uploadFiles = new();
        //    // First of all we need to upload all the attachments
        //    if (request.FirstAttachmentBuffer != null)
        //    {
        //        string location = Path.Combine(_env.ContentRootPath, _appSettings.StaticFolders.Root, _appSettings.StaticFolders.Attachments);
        //        var uploadFileResponse = await _fileService.UploadFile(new UploadRequest()
        //        {
        //            ClientFileName = request.FirstAttachmentBuffer.ClientFileName,
        //            ClientFileType = request.FirstAttachmentBuffer.ClientFileType,
        //            ClientFileData = request.FirstAttachmentBuffer.ClientFileData
        //        }, location, sid);

        //        // Assign saved paths
        //        var property = typeof(HuCertificateEditDTO).GetProperty("FileName");

        //        if (property != null)
        //        {
        //            property?.SetValue(request, uploadFileResponse.SavedAs);
        //            uploadFiles.Add(uploadFileResponse);
        //        }
        //        else
        //        {
        //            return Ok(new FormatedResponse() { StatusCode = EnumStatusCode.StatusCode400, MessageCode = CommonMessageCode.ASSIGN_TO_VALUE_DOES_NOT_MATCH_ANY_COLUMN + ": FirstAttachment" });
        //        }

        //    }
        //    var response = await _HuCertificateEditRepository.Create(_uow, request, sid);
        //    return Ok(response);
        //}
        //}

    }
}

