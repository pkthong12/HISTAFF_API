using CORE.GenericUOW;
using CORE.DTO;
using API.DTO;
using CORE.Enum;
using CORE.StaticConstant;
using API.All.DbContexts;
using CORE.AutoMapper;
using System.Linq;
using System.Runtime.InteropServices;
using API.Entities;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http.HttpResults;
using Aspose.Cells;
using API.DTO.PortalDTO;
using API.Entities.PORTAL;
using Azure;
using DocumentFormat.OpenXml.Office2010.Excel;
using PayrollDAL.Models;
using Microsoft.AspNetCore.SignalR;
using API.Socket;
using Common.Extensions;
using CORE.JsonHelper;
using System.Reflection;
using CORE.Services.File;

namespace API.Controllers.HuCertificateEdit
{
    public class HuCertificateEditRepository : IHuCertificateEditRepository
    {
        private readonly GenericUnitOfWork _uow;
        private readonly FullDbContext _dbContext;
        private IGenericRepository<HU_CERTIFICATE_EDIT, HuCertificateEditDTO> _genericRepository;
        private IGenericRepository<HU_CERTIFICATE, HuCertificateDTO> _genericParentRepository;
        private readonly GenericReducer<HU_CERTIFICATE_EDIT, HuCertificateEditDTO> _genericReducer;
        IHubContext<SignalHub> _hubContext;
        private readonly AppSettings _appSettings;
        private readonly IWebHostEnvironment _env;
        private readonly IFileService _fileService;

        public HuCertificateEditRepository(FullDbContext context, AppSettings appSettings, IWebHostEnvironment env, IFileService fileService, GenericUnitOfWork uow, IHubContext<SignalHub> hubContext)
        {
            _dbContext = context;
            _uow = uow;
            _genericRepository = _uow.GenericRepository<HU_CERTIFICATE_EDIT, HuCertificateEditDTO>();
            _genericParentRepository = _uow.GenericRepository<HU_CERTIFICATE, HuCertificateDTO>();
            _genericReducer = new();
            _hubContext = hubContext;
        }
        public async Task<GenericPhaseTwoListResponse<HuCertificateEditDTO>> SinglePhaseQueryList(GenericQueryListDTO<HuCertificateEditDTO> request)
        {
            string[] arrString = Array.Empty<string>();
            var joined = from p in _dbContext.HuCertificateEdits
                         from e in _dbContext.HuEmployees.Where(x => x.ID == p.EMPLOYEE_ID).AsNoTracking().DefaultIfEmpty()
                         from cv in _dbContext.HuEmployeeCvs.Where(x => x.ID == e.PROFILE_ID).AsNoTracking().DefaultIfEmpty()
                         from org in _dbContext.HuOrganizations.Where(x => x.ID == e.ORG_ID).AsNoTracking().DefaultIfEmpty()
                         from po in _dbContext.HuPositions.Where(x => x.ID == e.POSITION_ID).AsNoTracking().DefaultIfEmpty()
                         from j in _dbContext.HuJobs.AsNoTracking().Where(x => x.ID == po.JOB_ID).DefaultIfEmpty()

                         from type in _dbContext.SysOtherLists.Where(x => x.ID == p.TYPE_CERTIFICATE).AsNoTracking().DefaultIfEmpty()
                         from s in _dbContext.SysOtherLists.Where(x => x.ID == p.STATUS_ID).AsNoTracking().DefaultIfEmpty()
                         where p.IS_APPROVE_PORTAL == false && p.IS_SEND_PORTAL == true
                         select new HuCertificateEditDTO
                         {
                             Id = p.ID,
                             EmployeeCode = e.CODE,
                             JobOrderNum = (int)(j.ORDERNUM ?? 999),
                             IdSysOtherListApprove = p.ID_SYS_OTHER_LIST_APPROVE,
                             StatusName = s.NAME,
                             EmployeeFullName = cv.FULL_NAME,
                             OrgName = org.NAME,
                             PositionName = po.NAME,
                             TypeCertificateName = type.NAME,
                             Name = p.NAME,
                             TrainFromDate = p.TRAIN_FROM_DATE,
                             TrainToDate = p.TRAIN_TO_DATE,
                             Remark = p.REMARK,
                             OrgId = e.ORG_ID,
                             TrainFromDateStr = (p.TRAIN_FROM_DATE != null) ? p.TRAIN_FROM_DATE.Value.ToString("dd/MM/yyyy") : "",
                             TrainToDateStr = (p.TRAIN_TO_DATE != null) ? p.TRAIN_TO_DATE.Value.ToString("dd/MM/yyyy") : "",
                             ModelChanges = !string.IsNullOrEmpty(p.MODEL_CHANGE) ? p.MODEL_CHANGE!.Split(";", StringSplitOptions.None) : arrString
                         };

            var singlePhaseResult = await _genericReducer.SinglePhaseReduce(joined, request);
            return singlePhaseResult;
        }

        //public async Task<GenericPhaseTwoListResponse<HuCertificateEditDTO>> SinglePhaseQueryList(GenericQueryListDTO<HuCertificateEditDTO> request)
        //{
        //    // lấy trạng thái "chờ phê duyệt"
        //    // giá trị ID có thể là 993
        //    var id_approve_default = _dbContext.SysOtherLists
        //                                .Where(x => x.CODE == "CD")
        //                                .Select(x => x.ID)
        //                                .First();


        //    var joined = from p in _dbContext.HuCertificateEdits
        //                 .Where(x => x.IS_SEND_PORTAL == true && x.STATUS_ID == id_approve_default).AsNoTracking()

        //                     // JOIN OTHER ENTITIES BASED ON THE BUSINESS
        //                 from tham_chieu1 in _dbContext.HuEmployees.Where(x => x.ID == p.EMPLOYEE_ID).AsNoTracking().DefaultIfEmpty()
        //                 from tham_chieu2 in _dbContext.HuEmployeeCvs.Where(x => x.ID == tham_chieu1.PROFILE_ID).AsNoTracking().DefaultIfEmpty()
        //                 from tham_chieu3 in _dbContext.HuOrganizations.Where(x => x.ID == tham_chieu1.ORG_ID).AsNoTracking().DefaultIfEmpty()
        //                 from tham_chieu4 in _dbContext.HuPositions.Where(x => x.ID == tham_chieu1.POSITION_ID).AsNoTracking().DefaultIfEmpty()
        //                 from tham_chieu5 in _dbContext.SysOtherLists.Where(x => x.ID == p.TYPE_CERTIFICATE).AsNoTracking().DefaultIfEmpty()
        //                 from tham_chieu6 in _dbContext.SysOtherLists.Where(x => x.ID == p.STATUS_ID).AsNoTracking().DefaultIfEmpty()

        //                 select new HuCertificateEditDTO
        //                 {
        //                     // đây là ID của bảng HU_CERTIFICATE_EDIT
        //                     Id = p.ID,

        //                     // mã nhân viên
        //                     EmployeeCode = tham_chieu1.CODE,


        //                     // trạng thái
        //                     IdSysOtherListApprove = p.ID_SYS_OTHER_LIST_APPROVE,
        //                     StatusName = tham_chieu6.NAME,


        //                     // tên nhân viên
        //                     EmployeeFullName = tham_chieu2.FULL_NAME,

        //                     // phòng/ban
        //                     OrgName = tham_chieu3.NAME,

        //                     // chức danh
        //                     PositionName = tham_chieu4.NAME,

        //                     // loại bằng cấp/chứng chỉ
        //                     TypeCertificateName = tham_chieu5.NAME,

        //                     // tên bằng cấp/chứng chỉ
        //                     Name = p.NAME,

        //                     // thời gian đào tạo từ
        //                     TrainFromDateStr = (p.TRAIN_FROM_DATE != null) ? $"{p.TRAIN_FROM_DATE.Value.Day}/{p.TRAIN_FROM_DATE.Value.Month}/{p.TRAIN_FROM_DATE.Value.Year}" : "",

        //                     // thời gian đào tạo đến
        //                     TrainToDateStr = (p.TRAIN_TO_DATE != null) ? $"{p.TRAIN_TO_DATE.Value.Day}/{p.TRAIN_TO_DATE.Value.Month}/{p.TRAIN_TO_DATE.Value.Year}" : "",

        //                     Remark = p.REMARK
        //                 };

        //    var singlePhaseResult = await _genericReducer.SinglePhaseReduce(joined, request);
        //    return singlePhaseResult;
        //}

        public async Task<FormatedResponse> ReadAll()
        {
            await Task.Run(() => null);
            throw new NotImplementedException();
        }

        public async Task<FormatedResponse> ReadAllByKey(string key, long value)
        {
            var resposne = await _genericRepository.ReadAllByKey(key, value);
            return new() { InnerBody = resposne };
        }

        public async Task<FormatedResponse> ReadAllByKey(string key, string value)
        {
            var resposne = await _genericRepository.ReadAllByKey(key, value);
            return new() { InnerBody = resposne };
        }

        public async Task<FormatedResponse> GetById(long id)
        {
            var res = await _genericRepository.GetById(id);
            if (res.InnerBody != null)
            {
                var response = res.InnerBody;
                var list = new List<HU_CERTIFICATE>
                    {
                        (HU_CERTIFICATE)response
                    };
                var joined = (from p in list
                                  // JOIN OTHER ENTITIES BASED ON THE BUSINESS
                              select new HuCertificateDTO
                              {
                                  Id = p.ID,
                                  Name = p.NAME,
                                  TrainFromDate = p.TRAIN_FROM_DATE,
                                  TrainToDate = p.TRAIN_TO_DATE,
                                  EffectFrom = p.EFFECT_FROM,
                                  EffectTo = p.EFFECT_TO,
                                  Major = p.MAJOR,
                                  ContentTrain = p.CONTENT_TRAIN,
                                  Year = p.YEAR,
                                  Mark = p.MARK,
                                  IsPrime = p.IS_PRIME,
                                  Level = p.LEVEL,
                                  Remark = p.REMARK,
                                  EmployeeId = p.EMPLOYEE_ID,
                                  TypeCertificate = p.TYPE_CERTIFICATE,
                                  LevelTrain = p.LEVEL_TRAIN,
                                  SchoolId = p.SCHOOL_ID,
                                  Classification = p.CLASSIFICATION,
                                  TypeTrain = p.TYPE_TRAIN,
                                  FileName = p.FILE_NAME,
                                  LevelId = p.LEVEL_ID,
                              }).FirstOrDefault();
              
                return new FormatedResponse() { InnerBody = joined };
            }
            else
            {
                return new FormatedResponse() { MessageCode = CommonMessageCode.ENTITY_NOT_FOUND, ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400 };
            }
        }

        #region congnc
        public async Task<FormatedResponse> GetByIdWebApp(long id)
        {
            var res = await _genericRepository.GetById(id);
            if (res.InnerBody != null)
            {
                var response = res.InnerBody;

                // cmt code cũ lại
                //var list = new List<HU_CERTIFICATE_EDIT>
                //    {
                //        (HU_CERTIFICATE_EDIT)response
                //    };


                var list = _dbContext.HuCertificateEdits.Where(x => x.ID == id);


                var joined = (from l in list

                                  // JOIN OTHER ENTITIES BASED ON THE BUSINESS
                              from tham_chieu1 in _dbContext.HuEmployees.Where(x => x.ID == l.EMPLOYEE_ID).AsNoTracking().DefaultIfEmpty()
                              from tham_chieu2 in _dbContext.SysOtherLists.Where(x => x.ID == l.TYPE_CERTIFICATE).AsNoTracking().DefaultIfEmpty()
                              from tham_chieu3 in _dbContext.SysOtherLists.Where(x => x.ID == l.SCHOOL_ID).AsNoTracking().DefaultIfEmpty()
                              from tham_chieu4 in _dbContext.SysOtherLists.Where(x => x.ID == l.TYPE_TRAIN).AsNoTracking().DefaultIfEmpty()
                              from tham_chieu5 in _dbContext.SysOtherLists.Where(x => x.ID == l.LEVEL_ID).AsNoTracking().DefaultIfEmpty()
                              from tham_chieu6 in _dbContext.SysOtherLists.Where(x => x.ID == l.LEVEL_TRAIN).AsNoTracking().DefaultIfEmpty()
                              from tham_chieu7 in _dbContext.HuEmployeeCvs.Where(x => x.ID == tham_chieu1.PROFILE_ID).AsNoTracking().DefaultIfEmpty()
                              from tham_chieu8 in _dbContext.HuOrganizations.Where(x => x.ID == tham_chieu1.ORG_ID).AsNoTracking().DefaultIfEmpty()
                              from tham_chieu9 in _dbContext.HuPositions.Where(x => x.ID == tham_chieu1.POSITION_ID).AsNoTracking().DefaultIfEmpty()

                              select new HuCertificateEditDTO
                              {
                                  // trường 0:
                                  Id = l.ID,

                                  // trường 1:
                                  // Tên bằng cấp/chứng chỉ
                                  Name = l.NAME,

                                  // trường 2:
                                  // Loại bằng cấp/Chứng chỉ
                                  TypeCertificate = l.TYPE_CERTIFICATE,
                                  TypeCertificateName = tham_chieu2.NAME,

                                  // trường 3:
                                  // Ngày chứng chỉ có hiệu lực
                                  EffectFrom = l.EFFECT_FROM,

                                  // trường 4:
                                  // Ngày chứng chỉ hết hạn
                                  EffectTo = l.EFFECT_TO,

                                  // trường 5:
                                  // Là bằng chính
                                  IsPrime = l.IS_PRIME,

                                  // trường 6:
                                  // Thời gian đào tạo từ
                                  TrainFromDate = l.TRAIN_FROM_DATE,

                                  // trường 7:
                                  // Thời gian đào tạo đến
                                  TrainToDate = l.TRAIN_TO_DATE,

                                  // trường 8:
                                  // Nội dung đào tạo
                                  ContentTrain = l.CONTENT_TRAIN,

                                  // trường 9:
                                  // Trường học
                                  SchoolId = l.SCHOOL_ID,
                                  SchoolName = tham_chieu3.NAME,

                                  // trường 10:
                                  // Năm
                                  Year = l.YEAR,

                                  // trường 11:
                                  // Điểm số
                                  Mark = l.MARK,

                                  // trường 12:
                                  // Xếp loại tốt nghiệp
                                  Classification = l.CLASSIFICATION,

                                  // trường 13:
                                  // Hình thức đào tạo
                                  TypeTrain = l.TYPE_TRAIN,
                                  TypeTrainName = tham_chieu4.NAME,

                                  // trường 14:
                                  // Chuyên môn
                                  Major = l.MAJOR,

                                  // trường 15:
                                  // Trình độ
                                  LevelId = l.LEVEL_ID,
                                  LevelName = tham_chieu5.NAME,

                                  // trường 16:
                                  // Trình độ chuyên môn
                                  LevelTrain = l.LEVEL_TRAIN,
                                  LevelTrainName = tham_chieu6.NAME,

                                  // trường 17:
                                  // File đính kèm
                                  FileName = l.FILE_NAME,

                                  // trường 18:
                                  // Ghi chú
                                  Remark = l.REMARK,

                                  // trường 19:
                                  // đã gửi portal
                                  IsSendPortal = l.IS_SEND_PORTAL,

                                  // trường 20:
                                  IsApprovePortal = l.IS_APPROVE_PORTAL,

                                  // trường 21:
                                  // record.MODEL_CHANGE = dto.ModelChange;

                                  // trường 22:
                                  // nếu ID_HU_CERTIFICATE = null
                                  // thì đây là loại thêm mới
                                  // sau đó người dùng bấm "lưu"
                                  // thì nó mới bị dto.IdHuCertificate = null
                                  IdHuCertificate = l.ID_HU_CERTIFICATE,


                                  // trường 23:
                                  EmployeeId = l.EMPLOYEE_ID,
                                  EmployeeCode = tham_chieu1.CODE,
                                  EmployeeFullName = tham_chieu7.FULL_NAME,
                                  OrgName = tham_chieu8.NAME,
                                  PositionName = tham_chieu9.NAME,


                                  // trường 24:
                                  StatusRecord = l.STATUS_RECORD,


                                  // trường 25:
                                  // trạng thái phê duyệt
                                  IdSysOtherListApprove = l.ID_SYS_OTHER_LIST_APPROVE,
                                  StatusId = l.STATUS_ID
                              }).FirstOrDefault();

                return new FormatedResponse() { InnerBody = joined };
            }
            else
            {
                return new FormatedResponse() { MessageCode = CommonMessageCode.ENTITY_NOT_FOUND, ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400 };
            }
        }
        #endregion
        public async Task<FormatedResponse> GetById(string id)
        {
            await Task.Run(() => null);
            throw new NotImplementedException();
        }

        public async Task<FormatedResponse> Create(GenericUnitOfWork _uow, HuCertificateEditDTO dto, string sid)
        {
            var response = await _genericRepository.Create(_uow, dto, sid);
            return response;
        }

        public async Task<FormatedResponse> CreateRange(GenericUnitOfWork _uow, List<HuCertificateEditDTO> dtos, string sid)
        {
            var add = new List<HuCertificateEditDTO>();
            add.AddRange(dtos);
            var response = await _genericRepository.CreateRange(_uow, add, sid);
            return response;
        }

        public async Task<FormatedResponse> Update(GenericUnitOfWork _uow, HuCertificateEditDTO dto, string sid, bool patchMode = true)
        {
            var response = await _genericRepository.Update(_uow, dto, sid, patchMode);
            return response;
        }

        //public async Task<FormatedResponse> Update(GenericUnitOfWork _uow, HuCertificateEditDTO dto, string sid, bool patchMode = true)
        //{
        //    // bây giờ
        //    // có 2 hướng xử lý
        //    // hướng 1: duyệt bản ghi thêm mới
        //    // hướng 2: duyệt bản ghi bị sửa

        //    // dựa vào đâu
        //    // để biết duyệt bản ghi thêm mới
        //    // hay duyệt bản ghi bị sửa

        //    // câu trả lời: dựa vào ID_HU_CERTIFICATE
        //    // ở trong bảng phụ "HU_CERTIFICATE_EDIT"


        //    // nếu item.ID_HU_CERTIFICATE == null
        //    // hoặc item.ID_HU_CERTIFICATE == -1
        //    // thì thực hiện duyệt thêm mới
        //    if (dto.IdHuCertificate == null || dto.IdHuCertificate == -1)
        //    {
        //        // truy vấn để lấy ra 1 bản ghi trong bảng tạm
        //        // để lưu dữ liệu từ bảng tạm sang bảng chính
        //        var item = _dbContext.HuCertificateEdits.Where(x => x.ID == dto.Id).First();


        //        // nếu ID_SYS_OTHER_LIST_APPROVE == 994
        //        // thì người dùng chọn "Đã phê duyệt"
        //        // rồi bấm lưu
        //        if (item.ID_SYS_OTHER_LIST_APPROVE == 994)
        //        {
        //            // xóa cái giá trị của IS_SEND_PORTAL đi
        //            item.IS_SEND_PORTAL = null;

        //            // chỉ định giá trị true cho IS_APPROVE_PORTAL
        //            item.IS_APPROVE_PORTAL = true;
        //        }


        //        // nếu ID_SYS_OTHER_LIST_APPROVE == 995
        //        // thì người dùng chọn "Không phê duyệt"
        //        // rồi bấm lưu
        //        if (item.ID_SYS_OTHER_LIST_APPROVE == 995)
        //        {
        //            // xóa cái giá trị của IsSendPortal đi
        //            item.IS_SEND_PORTAL = null;

        //            // chỉ định giá trị false cho
        //            item.IS_APPROVE_PORTAL = false;
        //        }


        //        // tạo đối tượng
        //        // để thêm vào bảng HU_CERTIFICATE
        //        HU_CERTIFICATE record = new HU_CERTIFICATE()
        //        {
        //            // EMPLOYEE_ID
        //            // để lấy ra mã nhân viên,
        //            // tên nhân viên
        //            EMPLOYEE_ID = item.EMPLOYEE_ID,

        //            // là bằng chính
        //            IS_PRIME = item.IS_PRIME,

        //            // loại bằng cấp/chứng chỉ
        //            TYPE_CERTIFICATE = item.TYPE_CERTIFICATE,

        //            // tên bằng cấp/chứng chỉ
        //            NAME = item.NAME,

        //            // ngày chứng chỉ có hiệu lực
        //            EFFECT_FROM = item.EFFECT_FROM,

        //            // ngày chứng chỉ hết hạn
        //            EFFECT_TO = item.EFFECT_TO,

        //            // thời gian đào tạo từ
        //            TRAIN_FROM_DATE = item.TRAIN_FROM_DATE,

        //            // thời gian đào tạo đến
        //            TRAIN_TO_DATE = item.TRAIN_TO_DATE,

        //            // chuyên môn
        //            MAJOR = item.MAJOR,

        //            // trình độ chuyên môn
        //            LEVEL_TRAIN = item.LEVEL_TRAIN,

        //            // nội dung đào tạo
        //            CONTENT_TRAIN = item.CONTENT_TRAIN,

        //            // năm
        //            YEAR = item.YEAR,

        //            // điểm số
        //            MARK = item.MARK,

        //            // hình thức đào tạo
        //            TYPE_TRAIN = item.TYPE_TRAIN,

        //            // xếp loại tốt nghiệp
        //            CLASSIFICATION = item.CLASSIFICATION,

        //            // file đính kèm
        //            FILE_NAME = item.FILE_NAME,

        //            // ghi chú
        //            REMARK = item.REMARK,

        //            // trường học
        //            SCHOOL_ID = item.SCHOOL_ID,

        //            // trình độ
        //            LEVEL_ID = item.LEVEL_ID,


        //            // trạng thái bản ghi (portal)
        //            // STATUS_RECORD = "approved"
        //        };


        //        // thêm bản ghi vào ngữ cảnh context
        //        _dbContext.HuCertificates.Add(record);


        //        // bây giờ mới lưu thẳng dữ liệu vào trong db
        //        _dbContext.SaveChanges();


        //        return new FormatedResponse()
        //        {
        //            MessageCode = CommonMessageCode.PENDING_APPROVE_SUCCESS,
        //            InnerBody = item
        //        };
        //    }
        //    else if (dto.IdHuCertificate != null || dto.IdHuCertificate > 0)
        //    {
        //        // nếu item.ID_HU_CERTIFICATE khác null
        //        // hoặc item.ID_HU_CERTIFICATE > 0
        //        // thì thực hiện duyệt bản ghi bị sửa


        //        // lấy bản ghi trong bảng tạm
        //        // theo ID của cái dto truyền vào
        //        var item = _dbContext.HuCertificateEdits.Where(x => x.ID == dto.Id).First();


        //        // lấy 1 bản ghi
        //        // từ bảng chính HU_CERTIFICATE
        //        var record_main = _dbContext.HuCertificates
        //                            .Where(x => x.ID == item.ID_HU_CERTIFICATE)
        //                            .FirstOrDefault();



        //        // nếu bấm từ chối thì cho OUT luôn
        //        // return luôn
        //        if (dto.IdSysOtherListApprove == 995)
        //        {
        //            // nếu Pop Up được chọn "Không phê duyệt"
        //            // rồi bấm gửi
        //            // thì dto.IdSysOtherListApprove == 995


        //            // thiết lập
        //            item.IS_SEND_PORTAL = null;


        //            // điền lý do từ chối vào bản ghi
        //            item.REASON = dto.Reason;


        //            // sau khi lưu dữ liệu
        //            // từ bảng tạm
        //            // vào bảng chính
        //            // thì phải thiết lập lại
        //            // IS_APPROVE_PORTAL = true
        //            item.IS_APPROVE_PORTAL = false;


        //            // trạng thái bản ghi bảng tạm (web app)
        //            // 994 là "Đã phê duyệt"
        //            item.ID_SYS_OTHER_LIST_APPROVE = 995;


        //            // thiết lập cho bản ghi trong bảng tạm
        //            item.STATUS_RECORD = "unapproved";


        //            // sau khi duyệt hoặc từ chối
        //            // thì phải xóa ID_HU_CERTIFICATE
        //            item.ID_HU_CERTIFICATE = null;


        //            // dòng code này nên cmt lại
        //            // thiết lập thêm RECORD_NOTIFICATION = "approved"
        //            // cho bản ghi của bảng chính
        //            // record_main.STATUS_RECORD = "unapproved";


        //            // lưu db
        //            _dbContext.SaveChanges();


        //            return new FormatedResponse()
        //            {
        //                MessageCode = CommonMessageCode.UNAPPROVED_SUCCESS,
        //                InnerBody = item
        //            };
        //        }



        //        // EMPLOYEE_ID
        //        // để lấy ra mã nhân viên,
        //        // tên nhân viên
        //        record_main.EMPLOYEE_ID = item.EMPLOYEE_ID;

        //        // là bằng chính
        //        record_main.IS_PRIME = item.IS_PRIME;

        //        // loại bằng cấp/chứng chỉ
        //        record_main.TYPE_CERTIFICATE = item.TYPE_CERTIFICATE;

        //        // tên bằng cấp/chứng chỉ
        //        record_main.NAME = item.NAME;

        //        // ngày chứng chỉ có hiệu lực
        //        record_main.EFFECT_FROM = item.EFFECT_FROM;

        //        // ngày chứng chỉ hết hạn
        //        record_main.EFFECT_TO = item.EFFECT_TO;

        //        // thời gian đào tạo từ
        //        record_main.TRAIN_FROM_DATE = item.TRAIN_FROM_DATE;

        //        // thời gian đào tạo đến
        //        record_main.TRAIN_TO_DATE = item.TRAIN_TO_DATE;

        //        // chuyên môn
        //        record_main.MAJOR = item.MAJOR;

        //        // trình độ chuyên môn
        //        record_main.LEVEL_TRAIN = item.LEVEL_TRAIN;

        //        // nội dung đào tạo
        //        record_main.CONTENT_TRAIN = item.CONTENT_TRAIN;

        //        // năm
        //        record_main.YEAR = item.YEAR;

        //        // điểm số
        //        record_main.MARK = item.MARK;

        //        // hình thức đào tạo
        //        record_main.TYPE_TRAIN = item.TYPE_TRAIN;

        //        // xếp loại tốt nghiệp
        //        record_main.CLASSIFICATION = item.CLASSIFICATION;

        //        // file đính kèm
        //        record_main.FILE_NAME = item.FILE_NAME;

        //        // ghi chú
        //        record_main.REMARK = item.REMARK;

        //        // trường học
        //        record_main.SCHOOL_ID = item.SCHOOL_ID;

        //        // trình độ
        //        record_main.LEVEL_ID = item.LEVEL_ID;


        //        // nếu Pop Up được chọn "Đã phê duyệt"
        //        // rồi bấm gửi
        //        // thì dto.IdSysOtherListApprove == 994
        //        if (dto.IdSysOtherListApprove == 994)
        //        {
        //            item.IS_SEND_PORTAL = null;


        //            // sau khi lưu dữ liệu
        //            // từ bảng tạm
        //            // vào bảng chính
        //            // thì phải thiết lập lại
        //            // IS_APPROVE_PORTAL = true
        //            item.IS_APPROVE_PORTAL = true;


        //            // trạng thái bản ghi bảng tạm (web app)
        //            // 994 là "Đã phê duyệt"
        //            item.ID_SYS_OTHER_LIST_APPROVE = 994;


        //            // thiết lập cho bản ghi trong bảng tạm
        //            item.STATUS_RECORD = "approved";


        //            // sau khi duyệt hoặc từ chối
        //            // thì phải xóa ID_HU_CERTIFICATE
        //            item.ID_HU_CERTIFICATE = null;


        //            // thiết lập thêm RECORD_NOTIFICATION = "approved"
        //            // cho bản ghi của bảng chính
        //            record_main.STATUS_RECORD = "approved";
        //        }



        //        // nếu bản ghi nào có .AsNoTracking()
        //        // thì sẽ không làm được trò .SaveChanges()
        //        // tức là .SaveChanges() thì không lưu được giá trị vào db



        //        // lưu db
        //        _dbContext.SaveChanges();



        //        return new FormatedResponse()
        //        {
        //            MessageCode = CommonMessageCode.PENDING_APPROVE_SUCCESS,
        //            InnerBody = item
        //        };
        //    }


        //    var response = await _genericRepository.Update(_uow, dto, sid, patchMode);
        //    return response;
        //}

        public async Task<FormatedResponse> UpdateRange(GenericUnitOfWork _uow, List<HuCertificateEditDTO> dtos, string sid, bool patchMode = true)
        {
            var response = await _genericRepository.UpdateRange(_uow, dtos, sid, patchMode);
            return response;
        }

        public async Task<FormatedResponse> Delete(GenericUnitOfWork _uow, long id)
        {
            var response = await _genericRepository.Delete(_uow, id);
            return response;
        }

        public async Task<FormatedResponse> Delete(GenericUnitOfWork _uow, string id)
        {
            await Task.Run(() => null);
            throw new NotImplementedException();
        }

        public async Task<FormatedResponse> DeleteIds(GenericUnitOfWork _uow, List<long> ids)
        {
            var response = await _genericRepository.DeleteIds(_uow, ids);
            return response;
        }

        public async Task<FormatedResponse> DeleteIds(GenericUnitOfWork _uow, List<string> ids)
        {
            await Task.Run(() => null);
            throw new NotImplementedException();
        }

        public Task<FormatedResponse> ToggleActiveIds(GenericUnitOfWork _uow, List<long> ids, bool valueToBind, string sid)
        {
            throw new NotImplementedException();
        }

        public async Task<FormatedResponse> ApproveHuCertificateEdit(GenericUnapprovePortalDTO request, string sid)
        {
            try
            {
                List<HuCertificateEditDTO> lstApprove = new();
                List<HuCertificateDTO> lstUpdate = new();
                bool pathMode = true;
                var getOtherList = (from t in _uow.Context.Set<SYS_OTHER_LIST_TYPE>().Where(x => x.CODE == "STATUS")
                                    from o in _uow.Context.Set<SYS_OTHER_LIST>().DefaultIfEmpty()
                                    where o.CODE == "DD"
                                    select new { Id = o.ID }).FirstOrDefault();
                var getOtherList2 = (from t in _uow.Context.Set<SYS_OTHER_LIST_TYPE>().Where(x => x.CODE == "STATUS")
                                     from o in _uow.Context.Set<SYS_OTHER_LIST>().DefaultIfEmpty()
                                     where o.CODE == "TCPD"
                                     select new {Id =  o.ID}).FirstOrDefault(); 
                if(request.ValueToBind == true)
                {
                    request.Ids.ForEach(item =>
                    {
                        lstApprove.Add(new()
                        {
                            Id = item,
                            StatusId = getOtherList.Id,
                            IsApprovePortal = true,
                        });
                    });
                    var approveResponse = await _genericRepository.UpdateRange(_uow, lstApprove, sid, pathMode);
                    if(approveResponse != null)
                    {
                        request.Ids.ForEach(item =>
                        {
                            var getData = _uow.Context.Set<HU_CERTIFICATE_EDIT>().Where(x => x.ID == item).FirstOrDefault();
                            lstUpdate.Add(new()
                            {
                                Id = getData.ID_HU_CERTIFICATE,
                                IsPrime = getData.IS_PRIME,
                                EmployeeId = getData.EMPLOYEE_ID,
                                TypeCertificate = getData.TYPE_CERTIFICATE,
                                Name = getData.NAME,
                                EffectFrom = getData.EFFECT_FROM,
                                EffectTo = getData.EFFECT_TO,
                                TrainFromDate = getData.TRAIN_FROM_DATE,
                                TrainToDate = getData.TRAIN_TO_DATE,
                                ContentTrain = getData.CONTENT_TRAIN,
                                SchoolId = getData.SCHOOL_ID,
                                Year = getData.YEAR,
                                Mark = getData.MARK,
                                Classification = getData.CLASSIFICATION,
                                TypeTrain = getData.TYPE_TRAIN,
                                LevelId = getData.LEVEL_ID,
                                LevelTrain = getData.LEVEL_TRAIN,
                                FileName  = getData.FILE_NAME,
                                Remark = getData.REMARK,

                            });
                            AT_NOTIFICATION noti = new AT_NOTIFICATION()
                            {
                                CREATED_BY = sid,
                                TYPE = 6,
                                ACTION = 2,
                                TITLE = request.Reason,
                                STATUS_NOTIFY = 1,
                                EMP_NOTIFY_ID = getData.EMPLOYEE_ID.ToString(),
                                REF_ID = item,
                                MODEL_CHANGE = getData.MODEL_CHANGE,
                                CREATED_DATE = DateTime.Now
                            };
                            _dbContext.AtNotifications.AddRange(noti);
                            _dbContext.SaveChanges();
                            var employeeId = getData.EMPLOYEE_ID;
                            var users = _dbContext.SysUsers.Where(x => x.EMPLOYEE_ID == employeeId).ToList();
                            for (var i = 0; i < users.Count; i++)
                            {
                                var username = users[i].USERNAME;
                                if (!string.IsNullOrEmpty(username))
                                {
                                    _hubContext.Clients.User(username).SendAsync("ReceiveMessage", new
                                    {
                                        SignalType = "APPROVE_NOTIFICATION",
                                        Message = "Bạn có thông báo mới trên Portal"/*message*/,
                                        Data = new
                                        {

                                        }
                                    });
                                }

                            }
                        });
                        var updateResponse = await _genericParentRepository.UpdateRange(_uow, lstUpdate, sid, pathMode);
                        if(updateResponse != null)
                        {
                            return new FormatedResponse() { StatusCode = EnumStatusCode.StatusCode200, MessageCode = CommonMessageCode.APPROVED_SUCCESS };
                        }
                        else
                        {
                            return new FormatedResponse() { StatusCode = EnumStatusCode.StatusCode400, MessageCode = CommonMessageCode.APPROVE_FAIL };
                        }
                        
                    }
                    else
                    {
                        return new FormatedResponse() { StatusCode = EnumStatusCode.StatusCode400, MessageCode = CommonMessageCode.APPROVE_FAIL };
                    }
                }
                else
                {
                    request.Ids.ForEach(item =>
                    {
                        var getData = _uow.Context.Set<HU_CERTIFICATE_EDIT>().Where(x => x.ID == item).FirstOrDefault();
                        lstApprove.Add(new()
                        {
                            Id = item,
                            StatusId = getOtherList2?.Id,
                            IsApprovePortal = true,
                            IsSendPortal = false,
                            Reason = request.Reason
                        });

                        AT_NOTIFICATION noti = new AT_NOTIFICATION()
                        {
                            CREATED_BY = sid,
                            TYPE = 6,
                            ACTION = 2,
                            TITLE = request.Reason,
                            STATUS_NOTIFY = 2,
                            EMP_NOTIFY_ID = getData?.EMPLOYEE_ID.ToString(),
                            REF_ID = item,
                            MODEL_CHANGE = getData?.MODEL_CHANGE,
                            CREATED_DATE = DateTime.Now
                        };
                        _dbContext.AtNotifications.AddRange(noti);
                        _dbContext.SaveChanges();
                        var employeeId = getData.EMPLOYEE_ID;
                        var users = _dbContext.SysUsers.Where(x => x.EMPLOYEE_ID == employeeId).ToList();
                        for (var i = 0; i < users.Count; i++)
                        {
                            var username = users[i].USERNAME;
                            if (!string.IsNullOrEmpty(username))
                            {
                                _hubContext.Clients.User(username).SendAsync("ReceiveMessage", new
                                {
                                    SignalType = "APPROVE_NOTIFICATION",
                                    Message = "Bạn có thông báo mới trên Portal"/*message*/,
                                    Data = new
                                    {

                                    }
                                });
                            }

                        }

                    });
                    var updateResponse = await _genericRepository.UpdateRange(_uow, lstApprove, sid, pathMode); 
                    if(updateResponse != null )
                    {
                        return new FormatedResponse() { StatusCode = EnumStatusCode.StatusCode200, MessageCode = CommonMessageCode.UNAPPROVED_COMPLETE };
                    }
                    else
                    {
                        return new FormatedResponse() { StatusCode = EnumStatusCode.StatusCode400, MessageCode = CommonMessageCode.UNAPPROVED_FAILD };
                    }
                }
            }
            catch (Exception ex)
            {

                return new FormatedResponse() { StatusCode = EnumStatusCode.StatusCode500, ErrorType = EnumErrorType.UNCATCHABLE, MessageCode = ex.Message };
            }
        }

        public async Task<FormatedResponse> GetCertificateUnapprove(long employeeId)
        {
            try
            {
                var entity = _uow.Context.Set<HU_CERTIFICATE_EDIT>().AsNoTracking().AsQueryable();
                var employees = _uow.Context.Set<HU_EMPLOYEE>().AsNoTracking().AsQueryable();
                var employeeCvs = _uow.Context.Set<HU_EMPLOYEE_CV>().AsNoTracking().AsQueryable();
                var positions = _uow.Context.Set<HU_POSITION>().AsNoTracking().AsQueryable();
                var organizations = _uow.Context.Set<HU_ORGANIZATION>().AsNoTracking().AsQueryable();
                var otherLists = _uow.Context.Set<SYS_OTHER_LIST>().AsNoTracking().AsQueryable();
                var joined = await(from p in entity
                                   from e in employees.Where(x => x.ID == p.EMPLOYEE_ID).DefaultIfEmpty()
                                   from cv in employeeCvs.Where(x => x.ID == e.PROFILE_ID).DefaultIfEmpty()
                                   from t in positions.Where(x => x.ID == e.POSITION_ID).DefaultIfEmpty()
                                   from o in organizations.Where(x => x.ID == e.ORG_ID).DefaultIfEmpty()
                                   from type in otherLists.Where(x => x.ID == p.TYPE_CERTIFICATE).DefaultIfEmpty()
                                   from lvtrain in otherLists.Where(x => x.ID == p.LEVEL_TRAIN).DefaultIfEmpty()
                                   from typeTrain in otherLists.Where(x => x.ID == p.TYPE_TRAIN).DefaultIfEmpty()
                                   from school in otherLists.Where(x => x.ID == p.SCHOOL_ID).DefaultIfEmpty()
                                   from lv in otherLists.Where(x => x.ID == p.LEVEL_ID).DefaultIfEmpty()
                                   where p.EMPLOYEE_ID == employeeId && p.IS_APPROVE_PORTAL == true && p.IS_SEND_PORTAL == false && p.IS_SAVE_PORTAL == false
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
                                       FileName = p.FILE_NAME != null ? "File Upload" : "",
                                       //IsPrimeStr = p.IS_PRIME == true ? "Là bằng chính" : "Không là bằng chính",
                                       Classification = p.CLASSIFICATION,
                                       Reason = p.REASON
                                   }).ToListAsync();
                return new FormatedResponse() { InnerBody = joined };
            }
            catch (Exception ex)
            {

                return new FormatedResponse() { StatusCode = EnumStatusCode.StatusCode500, ErrorType = EnumErrorType.UNCATCHABLE, MessageCode = ex.Message };
            }
        }
        // triển khai phương thức từ chối phê duyệt
        public async Task<FormatedResponse> UnapproveHuCertificateEdit(GenericUnapprovePortalDTO request,string sid)
        {
            // đầu tiên
            // phải sử dụng try ... catch ...
            // để bắt ngoại lệ
            try
            {
                // lấy các bản ghi trong bảng tạm
                // theo điều kiện
                // là danh sách các ID truyền vào
                var list_record = await (
                                        from item in _dbContext.HuCertificateEdits
                                        where request.Ids.Contains(item.ID)
                                        select item
                                        ).ToListAsync();


                // truy vấn người dùng đang sử dụng web app
                // từ sid
                var user_context = (from item in _dbContext.SysUsers.Where(x => x.ID == sid)
                                   from reference_1 in _dbContext.HuEmployees.Where(x => x.ID == item.EMPLOYEE_ID)
                                   select reference_1).First();


                // lấy được cái list_record rồi
                // thì dùng vòng lặp để từ chối thôi
                foreach (var item in list_record)
                {
                    // mỗi cái bản ghi
                    // phải được đổ cái giá trị lý do vào
                    item.REASON = request.Reason;


                    // xóa trạng thái gửi lên web app
                    item.IS_SEND_PORTAL = null;


                    // thiết lập trạng thái từ chối
                    item.IS_APPROVE_PORTAL = false;


                    // không được thiết lập cứng ID_SYS_OTHER_LIST_APPROVE = 995
                    // mà phải truy vấn theo trường CODE
                    // xong rồi select ID
                    // thì mới chuẩn cách dev của mn
                    var id_unapprove = _dbContext.SysOtherLists
                                                .Where(x => x.CODE == "TCPD")
                                                .Select(x => x.ID)
                                                .First();


                    // cái id_unapprove = 995
                    // tùy db mà cái id_unapprove sẽ thay đổi theo
                    // còn cái CODE thì không thay đổi
                    // CODE luôn bằng "TCPD"
                    item.STATUS_ID = id_unapprove;


                    // thêm bản ghi vào bảng AT_NOTIFICATION
                    // để làm cái chuông thông báo bị từ chối
                    // cho portal
                    AT_NOTIFICATION obj_notify = new AT_NOTIFICATION()
                    {
                        // TYPE = 6 là bằng cấp chứng chỉ
                        TYPE = 6,

                        // ACTION = 2 kết quả phê duyệt
                        ACTION = 2,

                        // EMP_CREATE_ID là người gửi phê duyệt ở Portal
                        EMP_CREATE_ID = user_context.ID,

                        // EMP_NOTIFY_ID là người phê duyệt ở web app
                        // tạm thời chưa viết
                        EMP_NOTIFY_ID = item.EMPLOYEE_ID.ToString(),

                        CREATED_DATE = DateTime.Now,
                        REF_ID = item.ID,
                        REASON = item.REASON,
                        TITLE = item.REASON,

                        // STATUS_NOTIFY = 2 là từ chối
                        STATUS_NOTIFY = 2
                    };


                    // thêm bản ghi vào bảng AT_NOTIFICATION
                    _dbContext.AtNotifications.Add(obj_notify);


                    // lưu dữ liệu xuống db
                    _dbContext.SaveChanges();

                    var employeeId = item.EMPLOYEE_ID;
                    var users = _dbContext.SysUsers.Where(x => x.EMPLOYEE_ID == employeeId).ToList();
                    for (var i = 0; i < users.Count; i++)
                    {
                        var username = users[i].USERNAME;
                        if (!string.IsNullOrEmpty(username))
                        {
                            await _hubContext.Clients.User(username).SendAsync("ReceiveMessage", new
                            {
                                SignalType = "APPROVE_NOTIFICATION",
                                Message = "Bạn có thông báo mới trên Portal"/*message*/,
                                Data = new
                                {

                                }
                            });
                        }

                    }
                }


                // lấy tất cả bản ghi
                // ở trong bảng tạm
                // ở trạng thái chưa được duyệt mới nhất
                var query = await _dbContext.HuCertificateEdits
                                    .Where(x => x.IS_SEND_PORTAL == true && x.IS_APPROVE_PORTAL == null)
                                    .ToListAsync();


                return new FormatedResponse()
                {
                    MessageCode = CommonMessageCode.UNAPPROVED_SUCCESS,

                    // tôi tạm thời ẩn InnerBody
                    // để chức năng load lại trang có thể hoạt động bình thường

                    // InnerBody = query
                };
            }
            catch (Exception ex)
            {
                return new FormatedResponse() { InnerBody = ex.Message };
            }
        }


        #region SendUpdateHuCertificateEdit phiên bản 1 (ĐANG CMT CODE)
        // triển khai
        // phương thức gửi yêu cầu sửa bằng cấp chứng chỉ
        // vào bảng tạm HU_CERTIFICATE_EDIT

        //public async Task<FormatedResponse> SendUpdateHuCertificateEdit(HuCertificateEditDTO dto)
        //{
        //    // đầu tiên
        //    // phải sử dụng try ... catch ...
        //    // để bắt ngoại lệ
        //    try
        //    {
        //        // nếu bản ghi đang có trạng thái là "saved"
        //        // thì thiết lập lại
        //        if (dto.StatusRecord == "saved")
        //        {
        //            // người dùng bấm vào bút chì
        //            // sau đó người dùng bấm gửi duyệt
        //            // thì bản ghi sẽ chạy vào trong câu lệnh điều kiện If này


        //            // thiết lập "IsSendPortal = true"
        //            // để cho cái chuông bên web app có thể thông báo
        //            dto.IsSendPortal = true;

        //            // thiết lập "IsApprovePortal = false"
        //            // để cho cái chuông bên web app có thể thông báo
        //            dto.IsApprovePortal = false;

        //            // thiết lập trạng thái bản ghi
        //            // "sent" là đã gửi để chờ phê duyệt
        //            dto.StatusRecord = "sent";


        //            // bây giờ tôi sẽ sửa bản ghi với cái dto truyền vào
        //            var record = _dbContext.HuCertificateEdits.Where(x => x.ID == dto.Id).First();


        //            // bắt đầu sửa

        //            // trường 1:
        //            // Tên bằng cấp/chứng chỉ
        //            record.NAME = dto.Name;

        //            // trường 2:
        //            // Loại bằng cấp/Chứng chỉ
        //            record.TYPE_CERTIFICATE = dto.TypeCertificate;

        //            // trường 3:
        //            // Ngày chứng chỉ có hiệu lực
        //            record.EFFECT_FROM = dto.EffectFrom;

        //            // trường 4:
        //            // Ngày chứng chỉ hết hạn
        //            record.EFFECT_TO = dto.EffectTo;

        //            // trường 5:
        //            // Là bằng chính
        //            record.IS_PRIME = dto.IsPrime;

        //            // trường 6:
        //            // Thời gian đào tạo từ
        //            record.TRAIN_FROM_DATE = dto.TrainFromDate;

        //            // trường 7:
        //            // Thời gian đào tạo đến
        //            record.TRAIN_TO_DATE = dto.TrainToDate;

        //            // trường 8:
        //            // Nội dung đào tạo
        //            record.CONTENT_TRAIN = dto.ContentTrain;

        //            // trường 9:
        //            // Trường học
        //            record.SCHOOL_ID = dto.SchoolId;

        //            // trường 10:
        //            // Năm
        //            record.YEAR = dto.Year;

        //            // trường 11:
        //            // Điểm số
        //            record.MARK = dto.Mark;

        //            // trường 12:
        //            // Xếp loại tốt nghiệp
        //            record.CLASSIFICATION = dto.Classification;

        //            // trường 13:
        //            // Hình thức đào tạo
        //            record.TYPE_TRAIN = dto.TypeTrain;

        //            // trường 14:
        //            // Chuyên môn
        //            record.MAJOR = dto.Major;

        //            // trường 15:
        //            // Trình độ
        //            record.LEVEL_ID = dto.LevelId;

        //            // trường 16:
        //            // Trình độ chuyên môn
        //            record.LEVEL_TRAIN = dto.LevelTrain;

        //            // trường 17:
        //            // File đính kèm
        //            record.FILE_NAME = dto.FileName;

        //            // trường 18:
        //            // Ghi chú
        //            record.REMARK = dto.Remark;

        //            // trường 19:
        //            // đã gửi portal
        //            record.IS_SEND_PORTAL = dto.IsSendPortal;

        //            // trường 20:
        //            record.IS_APPROVE_PORTAL = dto.IsApprovePortal;

        //            // trường 21:
        //            // record.MODEL_CHANGE = dto.ModelChange;

        //            // trường 22:
        //            // nếu ID_HU_CERTIFICATE = null
        //            // thì đây là loại thêm mới
        //            // sau đó người dùng bấm "lưu"
        //            // thì nó mới bị dto.IdHuCertificate = null
        //            record.ID_HU_CERTIFICATE = dto.IdHuCertificate;


        //            // trường 23:
        //            record.EMPLOYEE_ID = dto.EmployeeId;


        //            // trường 24:
        //            record.STATUS_RECORD = dto.StatusRecord;


        //            // nếu ID_HU_CERTIFICATE = null
        //            // thì return luôn
        //            // để đỡ phải so sánh Model Change
        //            if (record.ID_HU_CERTIFICATE == null || record.ID_HU_CERTIFICATE <= 0)
        //            {
        //                // đây là lấy cái sự thay đổi cuối cùng đem gửi đi duyệt
        //                _dbContext.SaveChanges();


        //                return new FormatedResponse()
        //                {
        //                    MessageCode = CommonMessageCode.PENDING_APPROVE_SUCCESS,
        //                    InnerBody = record
        //                };
        //            }



        //            // lấy "bản ghi" trong bảng chính "HU_CERTIFICATE" ra
        //            // để so sánh các trường bị sửa
        //            // sau đó lưu vào MODEL_CHANGE
        //            // dưới dạng chuỗi string
        //            var record_HuCertificates = await _dbContext.HuCertificates
        //                                        .Where(x => x.ID == dto.IdHuCertificate)
        //                                        .AsNoTracking()
        //                                        .FirstAsync();


        //            // tạo danh sách
        //            // để lưu tên các trường bị sửa
        //            List<string> list_ThayDoi = new List<string>();


        //            // so sánh là bằng chính
        //            if (dto.IsPrime != record_HuCertificates.IS_PRIME)
        //            {
        //                list_ThayDoi.Add("IsPrime");
        //            }

        //            // so sánh loại bằng cấp/ chứng chỉ
        //            if (dto.TypeCertificate != record_HuCertificates.TYPE_CERTIFICATE)
        //            {
        //                list_ThayDoi.Add("TypeCertificate");
        //            }


        //            // so sánh tên bằng cấp/chứng chỉ
        //            if (dto.Name != record_HuCertificates.NAME)
        //            {
        //                list_ThayDoi.Add("Name");
        //            }


        //            // so sánh ngày chứng chỉ có hiệu lực
        //            if (dto.EffectFrom != record_HuCertificates.EFFECT_FROM)
        //            {
        //                list_ThayDoi.Add("EffectFrom");
        //            }


        //            // so sánh ngày chứng chỉ hết hạn
        //            if (dto.EffectTo != record_HuCertificates.EFFECT_TO)
        //            {
        //                list_ThayDoi.Add("EffectTo");
        //            }


        //            // so sánh thời gian đào tạo từ
        //            if (dto.TrainFromDate != record_HuCertificates.TRAIN_FROM_DATE)
        //            {
        //                list_ThayDoi.Add("TrainFromDate");
        //            }


        //            // so sánh thời gian đào tạo đến
        //            if (dto.TrainToDate != record_HuCertificates.TRAIN_TO_DATE)
        //            {
        //                list_ThayDoi.Add("TrainToDate");
        //            }


        //            // so sánh nội dung đào tạo
        //            if (dto.ContentTrain != record_HuCertificates.CONTENT_TRAIN)
        //            {
        //                list_ThayDoi.Add("ContentTrain");
        //            }


        //            // so sánh đơn vị đào tạo
        //            if (dto.SchoolId != record_HuCertificates.SCHOOL_ID)
        //            {
        //                list_ThayDoi.Add("SchoolId");
        //            }


        //            // so sánh năm
        //            if (dto.Year != record_HuCertificates.YEAR)
        //            {
        //                list_ThayDoi.Add("Year");
        //            }


        //            // so sánh điểm số
        //            if (dto.Mark != record_HuCertificates.MARK)
        //            {
        //                list_ThayDoi.Add("Mark");
        //            }


        //            // so sánh xếp loại tốt nghiệp
        //            if (dto.Classification != record_HuCertificates.CLASSIFICATION)
        //            {
        //                list_ThayDoi.Add("Classification");
        //            }


        //            // so sánh hình thức đào tạo
        //            if (dto.TypeTrain != record_HuCertificates.TYPE_TRAIN)
        //            {
        //                list_ThayDoi.Add("TypeTrain");
        //            }


        //            // so sánh chuyên môn
        //            if (dto.Major != record_HuCertificates.MAJOR)
        //            {
        //                list_ThayDoi.Add("Major");
        //            }


        //            // so sánh trình độ
        //            if (dto.LevelId != record_HuCertificates.LEVEL_ID)
        //            {
        //                list_ThayDoi.Add("LevelId");
        //            }


        //            // so sánh trình độ chuyên môn 
        //            if (dto.LevelTrain != record_HuCertificates.LEVEL_TRAIN)
        //            {
        //                list_ThayDoi.Add("LevelTrain");
        //            }


        //            // so sánh tệp đính kèm
        //            if (dto.FileName != record_HuCertificates.FILE_NAME)
        //            {
        //                list_ThayDoi.Add("FileName");
        //            }


        //            // so sánh ghi chú
        //            if (dto.Remark != record_HuCertificates.REMARK)
        //            {
        //                list_ThayDoi.Add("Remark");
        //            }


        //            // chuyển danh sách sang dạng chuỗi string
        //            // để lưu vào db
        //            string chuoi_ThayDoi = string.Join(", ", list_ThayDoi);


        //            // gán cái chuỗi thay đổi cho MODEL_CHANGE
        //            dto.ModelChange = chuoi_ThayDoi;


        //            // lưu vào MODEL_CHANGE
        //            record.MODEL_CHANGE = dto.ModelChange;


        //            // đây là lấy cái sự thay đổi cuối cùng đem gửi đi duyệt
        //            _dbContext.SaveChanges();


        //            return new FormatedResponse()
        //            {
        //                MessageCode = CommonMessageCode.PENDING_APPROVE_SUCCESS,
        //                InnerBody = record
        //            };
        //        }
        //        else
        //        {
        //            // nếu bản ghi không có "saved"
        //            // cụ thể dto.StatusRecord == null


        //            // thì suy ra 2 trường hợp
        //            // trường hợp 1:
        //            // lấy bản ghi ở bản chính đem đi gửi duyệt
        //            // (có ID, ID này là ID của bảng HU_CERTIFICATE)

        //            // trường hợp 2:
        //            // lấy DTO thêm mới ở ngoài giao diện đem đi gửi duyệt
        //            // (không có ID, vì bạn bấm dấu cộng để thêm mới thì làm gì có ID)

        //            // kết luận:
        //            // cả 2 trường hợp này đều phải
        //            // thêm mới bản ghi cho bảng tạm
        //            // điểm khác nhau là DTO có ID thì gán ID_HU_CERTIFICATE = DTO.ID
        //            // còn DTO không có ID thì không làm gì cả



        //            // trường hợp 1:
        //            // DTO được lấy từ bản ghi của bảng chính
        //            if (dto.Id != null || dto.Id > 0)
        //            {
        //                // trường hợp lấy bản ghi từ bảng chính
        //                // đem đi gửi duyệt
        //                // thì phải check các trường bị sửa


        //                // thiết lập "IsSendPortal = true"
        //                // tức là bản ghi đã được bấm gửi phê duyệt
        //                dto.IsSendPortal = true;


        //                // lưu cái "ID" của bảng chính "HU_CERTIFICATE"
        //                // vào trường "ID_HU_CERTIFICATE" của bảng tạm "HU_CERTIFICATE_EDIT"
        //                dto.IdHuCertificate = dto.Id;


        //                // lấy "bản ghi" trong bảng chính "HU_CERTIFICATE" ra
        //                // để so sánh các trường bị sửa
        //                // sau đó lưu vào MODEL_CHANGE
        //                // dưới dạng chuỗi string
        //                var record_HuCertificates = await _dbContext.HuCertificates
        //                                            .Where(x => x.ID == dto.Id)
        //                                            .AsNoTracking()
        //                                            .FirstAsync();

        //                // tạo danh sách
        //                // dùng để lưu tên các trường bị thay đổi
        //                List<string> list_ThayDoi = new List<string>();


        //                // so sánh là bằng chính
        //                if (dto.IsPrime != record_HuCertificates.IS_PRIME)
        //                {
        //                    list_ThayDoi.Add("IsPrime");
        //                }

        //                // so sánh loại bằng cấp/ chứng chỉ
        //                if (dto.TypeCertificate != record_HuCertificates.TYPE_CERTIFICATE)
        //                {
        //                    list_ThayDoi.Add("TypeCertificate");
        //                }


        //                // so sánh tên bằng cấp/chứng chỉ
        //                if (dto.Name != record_HuCertificates.NAME)
        //                {
        //                    list_ThayDoi.Add("Name");
        //                }


        //                // so sánh ngày chứng chỉ có hiệu lực
        //                if (dto.EffectFrom != record_HuCertificates.EFFECT_FROM)
        //                {
        //                    list_ThayDoi.Add("EffectFrom");
        //                }


        //                // so sánh ngày chứng chỉ hết hạn
        //                if (dto.EffectTo != record_HuCertificates.EFFECT_TO)
        //                {
        //                    list_ThayDoi.Add("EffectTo");
        //                }


        //                // so sánh thời gian đào tạo từ
        //                if (dto.TrainFromDate != record_HuCertificates.TRAIN_FROM_DATE)
        //                {
        //                    list_ThayDoi.Add("TrainFromDate");
        //                }


        //                // so sánh thời gian đào tạo đến
        //                if (dto.TrainToDate != record_HuCertificates.TRAIN_TO_DATE)
        //                {
        //                    list_ThayDoi.Add("TrainToDate");
        //                }


        //                // so sánh nội dung đào tạo
        //                if (dto.ContentTrain != record_HuCertificates.CONTENT_TRAIN)
        //                {
        //                    list_ThayDoi.Add("ContentTrain");
        //                }


        //                // so sánh đơn vị đào tạo
        //                if (dto.SchoolId != record_HuCertificates.SCHOOL_ID)
        //                {
        //                    list_ThayDoi.Add("SchoolId");
        //                }


        //                // so sánh năm
        //                if (dto.Year != record_HuCertificates.YEAR)
        //                {
        //                    list_ThayDoi.Add("Year");
        //                }


        //                // so sánh điểm số
        //                if (dto.Mark != record_HuCertificates.MARK)
        //                {
        //                    list_ThayDoi.Add("Mark");
        //                }


        //                // so sánh xếp loại tốt nghiệp
        //                if (dto.Classification != record_HuCertificates.CLASSIFICATION)
        //                {
        //                    list_ThayDoi.Add("Classification");
        //                }


        //                // so sánh hình thức đào tạo
        //                if (dto.TypeTrain != record_HuCertificates.TYPE_TRAIN)
        //                {
        //                    list_ThayDoi.Add("TypeTrain");
        //                }


        //                // so sánh chuyên môn
        //                if (dto.Major != record_HuCertificates.MAJOR)
        //                {
        //                    list_ThayDoi.Add("Major");
        //                }


        //                // so sánh trình độ
        //                if (dto.LevelId != record_HuCertificates.LEVEL_ID)
        //                {
        //                    list_ThayDoi.Add("LevelId");
        //                }


        //                // so sánh trình độ chuyên môn 
        //                if (dto.LevelTrain != record_HuCertificates.LEVEL_TRAIN)
        //                {
        //                    list_ThayDoi.Add("LevelTrain");
        //                }


        //                // so sánh tệp đính kèm
        //                if (dto.FileName != record_HuCertificates.FILE_NAME)
        //                {
        //                    list_ThayDoi.Add("FileName");
        //                }


        //                // so sánh ghi chú
        //                if (dto.Remark != record_HuCertificates.REMARK)
        //                {
        //                    list_ThayDoi.Add("Remark");
        //                }


        //                // chuyển danh sách sang dạng chuỗi string
        //                // để lưu vào db
        //                string chuoi_ThayDoi = string.Join(", ", list_ThayDoi);


        //                // gán cái chuỗi thay đổi cho MODEL_CHANGE
        //                dto.ModelChange = chuoi_ThayDoi;


        //                // tạo đối tượng có kiểu HU_CERTIFICATE_EDIT
        //                // để thêm vào bảng HU_CERTIFICATE_EDIT
        //                // trong ngữ cảnh context
        //                HU_CERTIFICATE_EDIT record = new HU_CERTIFICATE_EDIT()
        //                {
        //                    // nhớ là không được có ID trong này
        //                    // vì đang làm công việc thêm mới bản ghi
        //                    // vào bảng tạm


        //                    // EMPLOYEE_ID
        //                    // để lấy ra mã nhân viên,
        //                    // tên nhân viên
        //                    EMPLOYEE_ID = dto.EmployeeId,

        //                    // là bằng chính
        //                    IS_PRIME = dto.IsPrime,

        //                    // loại bằng cấp/chứng chỉ
        //                    TYPE_CERTIFICATE = dto.TypeCertificate,

        //                    // tên bằng cấp/chứng chỉ
        //                    NAME = dto.Name,

        //                    // ngày chứng chỉ có hiệu lực
        //                    EFFECT_FROM = dto.EffectFrom,

        //                    // ngày chứng chỉ hết hạn
        //                    EFFECT_TO = dto.EffectTo,

        //                    // thời gian đào tạo từ
        //                    TRAIN_FROM_DATE = dto.TrainFromDate,

        //                    // thời gian đào tạo đến
        //                    TRAIN_TO_DATE = dto.TrainToDate,

        //                    // chuyên môn
        //                    MAJOR = dto.Major,

        //                    // trình độ chuyên môn
        //                    LEVEL_TRAIN = dto.LevelTrain,

        //                    // nội dung đào tạo
        //                    CONTENT_TRAIN = dto.ContentTrain,

        //                    // năm
        //                    YEAR = dto.Year,

        //                    // điểm số
        //                    MARK = dto.Mark,

        //                    // hình thức đào tạo
        //                    TYPE_TRAIN = dto.TypeTrain,

        //                    // xếp loại tốt nghiệp
        //                    CLASSIFICATION = dto.Classification,

        //                    // file đính kèm
        //                    FILE_NAME = dto.FileName,

        //                    // ghi chú
        //                    REMARK = dto.Remark,

        //                    // trường học
        //                    SCHOOL_ID = dto.SchoolId,

        //                    // trình độ
        //                    LEVEL_ID = dto.LevelId,

        //                    // đã lưu portal
        //                    IS_SAVE_PORTAL = dto.IsSavePortal,

        //                    // đã gửi từ portal
        //                    IS_SEND_PORTAL = dto.IsSendPortal,

        //                    // đã duyệt portal
        //                    IS_APPROVE_PORTAL = dto.IsApprovePortal,

        //                    // danh sách tên các trường bị sửa
        //                    MODEL_CHANGE = dto.ModelChange,

        //                    // id của cái bản ghi bị sửa trong bảng chính
        //                    // cụ thể là bảng HU_CERTIFICATE
        //                    ID_HU_CERTIFICATE = dto.IdHuCertificate,


        //                    // thiết lập trạng thái bản ghi
        //                    STATUS_RECORD = "sent",


        //                    // thiết lập ID_SYS_OTHER_LIST_APPROVE = 993
        //                    // 993 là "chờ phê duyệt"
        //                    ID_SYS_OTHER_LIST_APPROVE = 993
        //                };


        //                // thêm bản ghi vào ngữ cảnh context
        //                _dbContext.HuCertificateEdits.Add(record);


        //                // chính thức lưu bản ghi vào db
        //                _dbContext.SaveChanges();


        //                return new FormatedResponse()
        //                {
        //                    // gửi đi thành công
        //                    MessageCode = CommonMessageCode.PENDING_APPROVE_SUCCESS,

        //                    // trả ra cái bản ghi bị sửa
        //                    InnerBody = record
        //                };

        //            }
        //            else
        //            {
        //                // DTO được lấy từ giao diện thêm mới
        //                // nên dto.Id == null

        //                // trường hợp lấy bản ghi từ giao diện thêm mới
        //                // đem đi gửi duyệt
        //                // thì không phải check các trường bị sửa
        //            }

        //        }


        //        // bản ghi nào mà bấm sửa
        //        // thì cũng có ID
        //        // nhưng để phân biệt
        //        // bản ghi từ bảng tạm
        //        // hay bản ghi từ bảng chính
        //        // thì dựa vào cái IS_APPROVE_PORTAL = false
        //        // ĐÂY LÀ TÔI ĐANG TEST THỬ
        //        // SỬA BẢN GHI TRONG BẢNG TẠM HU_CERTIFICATE_EDIT
        //        // VỚI CÁI LOẠI BẢN GHI LÀ THÊM MỚI NHƯNG CHƯA ĐƯỢC PHÊ DUYỆT
        //        if (dto.IsApprovePortal == false)
        //        {
        //            // biết được là bản ghi từ bảng tạm
        //            // thì query nó lên theo ID của bảng tạm
        //            // rồi sửa thôi
        //            // sửa xong thì submit lại chỗ cũ
        //            // vì chưa được duyệt nên cũng không cần Model Change đâu


        //            // lấy bản ghi trong bảng tạm theo ID
        //            var record1 = _dbContext.HuCertificateEdits.Where(x => x.ID == dto.Id).FirstOrDefault();

        //            // sửa cái record1 thôi

        //            // trường 1:
        //            // Tên bằng cấp/chứng chỉ
        //            record1.NAME = dto.Name;

        //            // trường 2:
        //            // Loại bằng cấp/Chứng chỉ
        //            record1.TYPE_CERTIFICATE = dto.TypeCertificate;

        //            // trường 3:
        //            // Ngày chứng chỉ có hiệu lực
        //            record1.EFFECT_FROM = dto.EffectFrom;

        //            // trường 4:
        //            // Ngày chứng chỉ hết hạn
        //            record1.EFFECT_TO = dto.EffectTo;

        //            // trường 5:
        //            // Là bằng chính
        //            record1.IS_PRIME = dto.IsPrime;

        //            // trường 6:
        //            // Thời gian đào tạo từ
        //            record1.TRAIN_FROM_DATE = dto.TrainFromDate;

        //            // trường 7:
        //            // Thời gian đào tạo đến
        //            record1.TRAIN_TO_DATE = dto.TrainToDate;

        //            // trường 8:
        //            // Nội dung đào tạo
        //            record1.CONTENT_TRAIN = dto.ContentTrain;

        //            // trường 9:
        //            // Trường học
        //            record1.SCHOOL_ID = dto.SchoolId;

        //            // trường 10:
        //            // Năm
        //            record1.YEAR = dto.Year;

        //            // trường 11:
        //            // Điểm số
        //            record1.MARK = dto.Mark;

        //            // trường 12:
        //            // Xếp loại tốt nghiệp
        //            record1.CLASSIFICATION = dto.Classification;

        //            // trường 13:
        //            // Hình thức đào tạo
        //            record1.TYPE_TRAIN = dto.TypeTrain;

        //            // trường 14:
        //            // Chuyên môn
        //            record1.MAJOR = dto.Major;

        //            // trường 15:
        //            // Trình độ
        //            record1.LEVEL_ID = dto.LevelId;

        //            // trường 16:
        //            // Trình độ chuyên môn
        //            record1.LEVEL_TRAIN = dto.LevelTrain;

        //            // trường 17:
        //            // File đính kèm
        //            record1.FILE_NAME = dto.FileName;

        //            // trường 18:
        //            // Ghi chú
        //            record1.REMARK = dto.Remark;

        //            // trường 19:
        //            // đã gửi portal
        //            // record1.IS_SEND_PORTAL = dto.IsSendPortal;

        //            // trường 20:
        //            // record1.IS_APPROVE_PORTAL = dto.IsApprovePortal;

        //            // trường 21:
        //            // record1.MODEL_CHANGE = dto.ModelChange;

        //            // trường 22:
        //            // record1.ID_HU_CERTIFICATE = dto.IdHuCertificate;

        //            // trường 23:
        //            record1.EMPLOYEE_ID = dto.EmployeeId;


        //            // sửa xong rồi thì lưu vào db
        //            _dbContext.SaveChanges();

        //            return new FormatedResponse()
        //            {
        //                MessageCode = CommonMessageCode.PENDING_APPROVE_SUCCESS,
        //                InnerBody = record1
        //            };
        //        }



        //        // return cho đỡ báo lỗi
        //        return new FormatedResponse()
        //        {
        //            // gửi đi thành công
        //            MessageCode = CommonMessageCode.PENDING_APPROVE_SUCCESS,

        //            // trả ra cái bản ghi bị sửa
        //            InnerBody = null
        //        };
        //    }
        //    catch (Exception ex)
        //    {
        //        return new FormatedResponse() { InnerBody = ex.Message };
        //    }
        //}
        #endregion

        #region SendUpdateHuCertificateEdit phiên bản 2 (ĐANG DÙNG)
        // triển khai
        // phương thức gửi yêu cầu sửa bằng cấp chứng chỉ
        // vào bảng tạm HU_CERTIFICATE_EDIT

        public async Task<FormatedResponse> SendUpdateHuCertificateEdit(HuCertificateEditDTO dto)
        {
            // đầu tiên
            // phải sử dụng try ... catch ...
            // để bắt ngoại lệ
            try
            {
                // lấy cái dto
                // thêm mới bản ghi vào trong bảng tạm
                // vì mỗi lần gửi phê duyệt
                // là 1 lần thêm mới bản ghi


                // thiết lập "IsSendPortal = true"
                // để cho cái chuông bên web app có thể thông báo
                dto.IsSendPortal = true;

                // tạo đối tượng có kiểu
                HU_CERTIFICATE_EDIT item = new HU_CERTIFICATE_EDIT()
                {
                    EMPLOYEE_ID = dto.EmployeeId,
                    IS_PRIME = dto.IsPrime,
                    TYPE_CERTIFICATE = dto.TypeCertificate,
                    NAME = dto.Name,
                    EFFECT_FROM = dto.EffectFrom,
                    EFFECT_TO = dto.EffectTo,
                    TRAIN_FROM_DATE = dto.TrainFromDate,
                    TRAIN_TO_DATE = dto.TrainToDate,
                    MAJOR = dto.Major,
                    LEVEL_TRAIN = dto.LevelTrain,
                    CONTENT_TRAIN = dto.ContentTrain,
                    YEAR = dto.Year,
                    MARK = dto.Mark,
                    TYPE_TRAIN = dto.TypeTrain,
                    CLASSIFICATION = dto.Classification,
                    FILE_NAME = dto.FileName,
                    REMARK = dto.Remark,
                    SCHOOL_ID = dto.SchoolId,
                    LEVEL_ID = dto.LevelId,
                    IS_SAVE_PORTAL = dto.IsSavePortal,
                    IS_SEND_PORTAL = true,
                    IS_APPROVE_PORTAL = dto.IsApprovePortal,
                    MODEL_CHANGE = dto.ModelChange,
                    ID_HU_CERTIFICATE = dto.Id,
                    STATUS_RECORD = "sent",
                    ID_SYS_OTHER_LIST_APPROVE = 993
                };


                // thêm vào bảng tạm
                await _dbContext.HuCertificateEdits.AddAsync(item);
                _dbContext.SaveChanges();


                // lấy ra bản ghi mà bạn vừa bấm gửi duyệt
                // bấm gửi duyệt thì nó tự thêm 1 bản ghi mới vào bảng tạm
                var record_top1 = _dbContext.HuCertificateEdits.OrderByDescending(x => x.ID).Take(1);


                return new FormatedResponse()
                {
                    // gửi đi thành công
                    MessageCode = CommonMessageCode.PENDING_APPROVE_SUCCESS,

                    // trả ra cái bản ghi bị sửa
                    InnerBody = record_top1
                };
            }
            catch (Exception ex)
            {
                return new FormatedResponse() { InnerBody = ex.Message };
            }
        }
        #endregion



        #region GetPortalByEmployeeId phiên bản 1 (ĐANG CMT CODE)
        // lấy các bản ghi
        // là loại phê duyệt thêm mới
        // trong bảng tạm HU_CERTIFICATE_EDIT
        // có đặc điểm:
        // 1. IS_SEND_PORTAL = true
        // 2. IS_APPROVE_PORTAL = false
        // 3. ID_HU_CERTIFICATE = null hoặc ID_HU_CERTIFICATE <= 0

        //public async Task<FormatedResponse> GetPortalByEmployeeId(long id)
        //{
        //    // đầu tiên
        //    // phải sử dụng try ... catch ...
        //    // để bắt ngoại lệ
        //    try
        //    {
        //        // lấy ra các bản ghi trong bảng tạm HU_CERTIFICATE_EDIT
        //        // thỏa mãn các điều kiện sau đây:
        //        // 1. bản ghi đã lưu ()
        //        // 2. bản ghi đã gửi (IS_SEND_PORTAL = true)
        //        // 3. bản ghi bị từ chối (IS_APPROVE_PORTAL = false)
        //        // đại khái là bản ghi trong bảng chính có ID
        //        // bằng với ID_HU_CERTIFICATE trong bảng tạm
        //        // lại còn rơi vào 1 trong 3 trường hợp trên
        //        // thì sẽ bị ẩn đi

        //        // nhưng mà tôi chỉ lấy ID_HU_CERTIFICATE thôi
        //        // để tôi làm công việc xóa phần tử ở bên dưới

        //        // lần đầu tiên người dùng sử dụng Portal
        //        // thì có thể ListId.Count = 0 đấy
        //        List<long?> ListId =    _dbContext.HuCertificateEdits
        //                                .Where(x =>
        //                                    /* bản ghi trong bảng tạm ở trạng thái "đã lưu" */
        //                                    (x.IS_SAVE_PORTAL == true

        //                                    /* bản ghi trong bảng tạm ở trạng thái "chờ phê duyệt" */
        //                                    || x.IS_SEND_PORTAL == true

        //                                    /* bản ghi trong bảng tạm ở trạng thái "từ chối phê duyệt" */
        //                                    || x.IS_APPROVE_PORTAL == false)

        //                                    /* đồng thời, ID_HU_CERTIFICATE phải khác null */
        //                                    && x.ID_HU_CERTIFICATE != null
        //                                )
        //                                .Select(x => x.ID_HU_CERTIFICATE)
        //                                .ToList();


        //        // nếu bản ghi trong bảng chính HU_CERTIFICATE
        //        // bị bấm nút lưu
        //        // thì lúc show ra màn hình ở Portal phải bị ẩn đi
        //        // sử dụng kỹ thuật xóa phần tử trong List C#
        //        List<long?> ListId_saved =  _dbContext.HuCertificateEdits
        //                                    .Where( x => x.IS_APPROVE_PORTAL == null &&
        //                                            x.ID_HU_CERTIFICATE != null &&
        //                                            (x.STATUS_RECORD == "saved" || x.STATUS_RECORD == "sent"))
        //                                .Select(x => x.ID_HU_CERTIFICATE)
        //                                .ToList();



        //        // lấy tất cả bản ghi
        //        // trong bảng chính
        //        // theo employee_id = id
        //        var query1 = await (
        //                            from item in _dbContext.HuCertificates.Where(x => x.EMPLOYEE_ID == id).DefaultIfEmpty()

        //                            // kết hợp dữ liệu (join)
        //                            from tham_chieu1 in _dbContext.SysOtherLists.Where(x => x.ID == item.TYPE_CERTIFICATE).DefaultIfEmpty()
        //                            from tham_chieu2 in _dbContext.SysOtherLists.Where(x => x.ID == item.SCHOOL_ID).DefaultIfEmpty()
        //                            from tham_chieu3 in _dbContext.SysOtherLists.Where(x => x.ID == item.TYPE_TRAIN).DefaultIfEmpty()
        //                            from tham_chieu4 in _dbContext.SysOtherLists.Where(x => x.ID == item.LEVEL_ID).DefaultIfEmpty()
        //                            from tham_chieu5 in _dbContext.SysOtherLists.Where(x => x.ID == item.LEVEL_TRAIN).DefaultIfEmpty()
        //                            select new HuCertificateEditDTO
        //                            {
        //                                // trường 0:
        //                                // Id của bảng chính
        //                                Id = item.ID,

        //                                // trường 1:
        //                                // Tên bằng cấp/chứng chỉ
        //                                Name = item.NAME,

        //                                // trường 2:
        //                                // Loại bằng cấp/Chứng chỉ
        //                                TypeCertificate = item.TYPE_CERTIFICATE,
        //                                TypeCertificateName = tham_chieu1.NAME,

        //                                // trường 3:
        //                                // Ngày chứng chỉ có hiệu lực
        //                                EffectFrom = item.EFFECT_FROM,

        //                                // trường 4:
        //                                // Ngày chứng chỉ hết hạn
        //                                EffectTo = item.EFFECT_TO,

        //                                // trường 5:
        //                                // Là bằng chính
        //                                IsPrime = item.IS_PRIME,

        //                                // trường 6:
        //                                // Thời gian đào tạo từ
        //                                TrainFromDate = item.TRAIN_FROM_DATE,

        //                                // trường 7:
        //                                // Thời gian đào tạo đến
        //                                TrainToDate = item.TRAIN_TO_DATE,

        //                                // trường 8:
        //                                // Nội dung đào tạo
        //                                ContentTrain = item.CONTENT_TRAIN,

        //                                // trường 9:
        //                                // Trường học
        //                                SchoolId = item.SCHOOL_ID,
        //                                SchoolName = tham_chieu2.NAME,

        //                                // trường 10:
        //                                // Năm
        //                                Year = item.YEAR,

        //                                // trường 11:
        //                                // Điểm số
        //                                Mark = item.MARK,

        //                                // trường 12:
        //                                // Xếp loại tốt nghiệp
        //                                Classification = item.CLASSIFICATION,

        //                                // trường 13:
        //                                // Hình thức đào tạo
        //                                TypeTrain = item.TYPE_TRAIN,
        //                                TypeTrainName = tham_chieu3.NAME,

        //                                // trường 14:
        //                                // Chuyên môn
        //                                Major = item.MAJOR,

        //                                // trường 15:
        //                                // Trình độ
        //                                LevelId = item.LEVEL_ID,
        //                                LevelName = tham_chieu4.NAME,

        //                                // trường 16:
        //                                // Trình độ chuyên môn
        //                                LevelTrain = item.LEVEL_TRAIN,
        //                                LevelTrainName = tham_chieu5.NAME,

        //                                // trường 17:
        //                                // File đính kèm
        //                                FileName = item.FILE_NAME,

        //                                // trường 18:
        //                                // Ghi chú
        //                                Remark = item.REMARK,

        //                                // trường 18.5:
        //                                // đã lưu
        //                                IsSavePortal = null,

        //                                // trường 19:
        //                                // đã gửi portal
        //                                IsSendPortal = null,

        //                                // trường 20:
        //                                IsApprovePortal = null,

        //                                // trường 21:
        //                                ModelChange = null,

        //                                // trường 22:
        //                                IdHuCertificate = null,

        //                                // trường 23:
        //                                EmployeeId = item.EMPLOYEE_ID,

        //                                // trường 24:
        //                                // trạng thái bản ghi
        //                                StatusRecord = item.STATUS_RECORD
        //                            }
        //                            ).ToListAsync();


        //        // loại bỏ các phần tử bên trong query1
        //        // có đặc điểm như ListId
        //        if (ListId.Count > 0)
        //        {
        //            query1.RemoveAll(x => ListId.Contains(x.Id));
        //        }


        //        // loại bỏ các phần tử bên trong query1
        //        // có đặc điểm như ListId_saved
        //        if (ListId_saved.Count > 0)
        //        {
        //            query1.RemoveAll(x => ListId_saved.Contains(x.Id));
        //        }


        //        // kết hợp dữ liệu (join)
        //        var join_data = 
        //                        (

        //                        // lấy các bản ghi trong HU_CERTIFICATE_EDIT
        //                        // theo EMPLOYEE_ID
        //                        // với điều kiện:
        //                        // 1. hoặc đã lưu (IS_SAVE_PORTAL == true)
        //                        // 2. hoặc đã gửi - chờ phê duyệt (IS_SEND_PORTAL == true)
        //                        // 3. hoặc bị từ chối phê duyệt (IS_APPROVE_PORTAL == false)

        //                        // bản ghi đã bấm lưu (STATUS_RECORD == "saved")
        //                        // bản ghi đã bấm gửi (STATUS_RECORD == "sent")

        //                        from item in _dbContext.HuCertificateEdits
        //                            .Where(x =>
        //                                /* đã lưu */
        //                                (x.IS_SAVE_PORTAL == true

        //                                /* đã gửi và chờ phê duyệt */
        //                                || x.IS_SEND_PORTAL == true

        //                                /* bị từ chối phê duyệt */
        //                                || x.IS_APPROVE_PORTAL == false)

        //                                /* lấy dữ liệu theo EMPLOYEE_ID */
        //                                && x.EMPLOYEE_ID == id

        //                                // và chắc chắn rằng những bản ghi này
        //                                // phải chưa được duyệt
        //                                && x.IS_APPROVE_PORTAL != true
        //                            )
        //                            // sử dụng .DefaultIfEmpty() ở đây thì coi chừng lỗi đấy
        //                            //.DefaultIfEmpty()

        //                        // kết hợp dữ liệu (join)
        //                        from tham_chieu1 in _dbContext.SysOtherLists.Where(x => x.ID == item.TYPE_CERTIFICATE).DefaultIfEmpty()
        //                        from tham_chieu2 in _dbContext.SysOtherLists.Where(x => x.ID == item.SCHOOL_ID).DefaultIfEmpty()
        //                        from tham_chieu3 in _dbContext.SysOtherLists.Where(x => x.ID == item.TYPE_TRAIN).DefaultIfEmpty()
        //                        from tham_chieu4 in _dbContext.SysOtherLists.Where(x => x.ID == item.LEVEL_ID).DefaultIfEmpty()
        //                        from tham_chieu5 in _dbContext.SysOtherLists.Where(x => x.ID == item.LEVEL_TRAIN).DefaultIfEmpty()

        //                        select new HuCertificateEditDTO
        //                        {
        //                            // trường 0:
        //                            // Id của bảng tạm
        //                            Id = item.ID,

        //                            // trường 1:
        //                            // Tên bằng cấp/chứng chỉ
        //                            Name = item.NAME,

        //                            // trường 2:
        //                            // Loại bằng cấp/Chứng chỉ
        //                            TypeCertificate = item.TYPE_CERTIFICATE,
        //                            TypeCertificateName = tham_chieu1.NAME,

        //                            // trường 3:
        //                            // Ngày chứng chỉ có hiệu lực
        //                            EffectFrom = item.EFFECT_FROM,

        //                            // trường 4:
        //                            // Ngày chứng chỉ hết hạn
        //                            EffectTo = item.EFFECT_TO,

        //                            // trường 5:
        //                            // Là bằng chính
        //                            IsPrime = item.IS_PRIME,

        //                            // trường 6:
        //                            // Thời gian đào tạo từ
        //                            TrainFromDate = item.TRAIN_FROM_DATE,

        //                            // trường 7:
        //                            // Thời gian đào tạo đến
        //                            TrainToDate = item.TRAIN_TO_DATE,

        //                            // trường 8:
        //                            // Nội dung đào tạo
        //                            ContentTrain = item.CONTENT_TRAIN,

        //                            // trường 9:
        //                            // Trường học
        //                            SchoolId = item.SCHOOL_ID,
        //                            SchoolName = tham_chieu2.NAME,

        //                            // trường 10:
        //                            // Năm
        //                            Year = item.YEAR,

        //                            // trường 11:
        //                            // Điểm số
        //                            Mark = item.MARK,

        //                            // trường 12:
        //                            // Xếp loại tốt nghiệp
        //                            Classification = item.CLASSIFICATION,

        //                            // trường 13:
        //                            // Hình thức đào tạo
        //                            TypeTrain = item.TYPE_TRAIN,
        //                            TypeTrainName = tham_chieu3.NAME,

        //                            // trường 14:
        //                            // Chuyên môn
        //                            Major = item.MAJOR,

        //                            // trường 15:
        //                            // Trình độ
        //                            LevelId = item.LEVEL_ID,
        //                            LevelName = tham_chieu4.NAME,

        //                            // trường 16:
        //                            // Trình độ chuyên môn
        //                            LevelTrain = item.LEVEL_TRAIN,
        //                            LevelTrainName = tham_chieu5.NAME,

        //                            // trường 17:
        //                            // File đính kèm
        //                            FileName = item.FILE_NAME,

        //                            // trường 18:
        //                            // Ghi chú
        //                            Remark = item.REMARK,

        //                            // trường 18.5:
        //                            // đã lưu
        //                            IsSavePortal = item.IS_SAVE_PORTAL,

        //                            // trường 19:
        //                            // đã gửi portal
        //                            IsSendPortal = item.IS_SEND_PORTAL,

        //                            // trường 20:
        //                            IsApprovePortal = item.IS_APPROVE_PORTAL,

        //                            // trường 21:
        //                            ModelChange = item.MODEL_CHANGE,

        //                            // trường 22:
        //                            IdHuCertificate = item.ID_HU_CERTIFICATE,

        //                            // trường 23:
        //                            EmployeeId = item.EMPLOYEE_ID,

        //                            // trường 24:
        //                            // trạng thái bản ghi
        //                            StatusRecord = item.STATUS_RECORD
        //                        }).ToList();


        //        // nếu có bản ghi ở bảng tạm
        //        // thì mới thêm phần tử cho
        //        // danh sách query1
        //        if (join_data.Count() > 0)
        //        {
        //            // thêm join_data vào trong list bản ghi
        //            foreach (var record in join_data)
        //            {
        //                query1.Add(record);
        //            }
        //        }


        //        return new FormatedResponse()
        //        {
        //            InnerBody = query1
        //        };
        //    }
        //    catch (Exception ex)
        //    {
        //        // in ra ngoại lệ
        //        return new FormatedResponse() { MessageCode = ex.Message };
        //    }
        //}
        #endregion

        #region GetPortalByEmployeeId phiên bản 2, dev theo "Dat NV" (ĐANG DÙNG)
        public async Task<FormatedResponse> GetPortalByEmployeeId(long id)
        {
            // đầu tiên
            // phải sử dụng try ... catch ...
            // để bắt ngoại lệ
            try
            {
                // lấy tất cả bản ghi
                // trong bảng chính
                // theo employee_id = id
                var query1 = await (
                                    from item in _dbContext.HuCertificates.Where(x => x.EMPLOYEE_ID == id).DefaultIfEmpty()

                                        // kết hợp dữ liệu (join)
                                    from tham_chieu1 in _dbContext.SysOtherLists.Where(x => x.ID == item.TYPE_CERTIFICATE).DefaultIfEmpty()
                                    from tham_chieu2 in _dbContext.SysOtherLists.Where(x => x.ID == item.SCHOOL_ID).DefaultIfEmpty()
                                    from tham_chieu3 in _dbContext.SysOtherLists.Where(x => x.ID == item.TYPE_TRAIN).DefaultIfEmpty()
                                    from tham_chieu4 in _dbContext.SysOtherLists.Where(x => x.ID == item.LEVEL_ID).DefaultIfEmpty()
                                    from tham_chieu5 in _dbContext.SysOtherLists.Where(x => x.ID == item.LEVEL_TRAIN).DefaultIfEmpty()
                                    select new HuCertificateEditDTO
                                    {
                                        Id = item.ID,
                                        Name = item.NAME,
                                        TypeCertificate = item.TYPE_CERTIFICATE,
                                        TypeCertificateName = tham_chieu1.NAME,
                                        EffectFrom = item.EFFECT_FROM,
                                        EffectTo = item.EFFECT_TO,
                                        IsPrime = item.IS_PRIME,
                                        TrainFromDate = item.TRAIN_FROM_DATE,
                                        TrainToDate = item.TRAIN_TO_DATE,
                                        ContentTrain = item.CONTENT_TRAIN,
                                        SchoolId = item.SCHOOL_ID,
                                        SchoolName = tham_chieu2.NAME,
                                        Year = item.YEAR,
                                        Mark = item.MARK,
                                        Classification = item.CLASSIFICATION,
                                        TypeTrain = item.TYPE_TRAIN,
                                        TypeTrainName = tham_chieu3.NAME,
                                        Major = item.MAJOR,
                                        LevelId = item.LEVEL_ID,
                                        LevelName = tham_chieu4.NAME,
                                        LevelTrain = item.LEVEL_TRAIN,
                                        LevelTrainName = tham_chieu5.NAME,
                                        FileName = item.FILE_NAME,
                                        Remark = item.REMARK,
                                        IsSavePortal = null,
                                        IsSendPortal = null,
                                        IsApprovePortal = null,
                                        ModelChange = null,
                                        IdHuCertificate = null,
                                        EmployeeId = item.EMPLOYEE_ID,
                                        StatusRecord = item.STATUS_RECORD
                                    }
                                    ).ToListAsync();


                // lấy các bản ghi trong bảng tạm
                // với điều kiện là đã gửi phê duyệt
                var query2 = await (
                                    from item in _dbContext.HuCertificateEdits.Where(x => x.EMPLOYEE_ID == id).DefaultIfEmpty()

                                        // kết hợp dữ liệu (join)
                                    from tham_chieu1 in _dbContext.SysOtherLists.Where(x => x.ID == item.TYPE_CERTIFICATE).DefaultIfEmpty()
                                    from tham_chieu2 in _dbContext.SysOtherLists.Where(x => x.ID == item.SCHOOL_ID).DefaultIfEmpty()
                                    from tham_chieu3 in _dbContext.SysOtherLists.Where(x => x.ID == item.TYPE_TRAIN).DefaultIfEmpty()
                                    from tham_chieu4 in _dbContext.SysOtherLists.Where(x => x.ID == item.LEVEL_ID).DefaultIfEmpty()
                                    from tham_chieu5 in _dbContext.SysOtherLists.Where(x => x.ID == item.LEVEL_TRAIN).DefaultIfEmpty()
                                    where item.IS_SEND_PORTAL == true

                                    select new HuCertificateEditDTO
                                    {
                                        Id = item.ID,
                                        Name = item.NAME,
                                        TypeCertificate = item.TYPE_CERTIFICATE,
                                        TypeCertificateName = tham_chieu1.NAME,
                                        EffectFrom = item.EFFECT_FROM,
                                        EffectTo = item.EFFECT_TO,
                                        IsPrime = item.IS_PRIME,
                                        TrainFromDate = item.TRAIN_FROM_DATE,
                                        TrainToDate = item.TRAIN_TO_DATE,
                                        ContentTrain = item.CONTENT_TRAIN,
                                        SchoolId = item.SCHOOL_ID,
                                        SchoolName = tham_chieu2.NAME,
                                        Year = item.YEAR,
                                        Mark = item.MARK,
                                        Classification = item.CLASSIFICATION,
                                        TypeTrain = item.TYPE_TRAIN,
                                        TypeTrainName = tham_chieu3.NAME,
                                        Major = item.MAJOR,
                                        LevelId = item.LEVEL_ID,
                                        LevelName = tham_chieu4.NAME,
                                        LevelTrain = item.LEVEL_TRAIN,
                                        LevelTrainName = tham_chieu5.NAME,
                                        FileName = item.FILE_NAME,
                                        Remark = item.REMARK,
                                        IsSavePortal = item.IS_SAVE_PORTAL,
                                        IsSendPortal = item.IS_SEND_PORTAL,
                                        IsApprovePortal = item.IS_APPROVE_PORTAL,
                                        ModelChange = item.MODEL_CHANGE,
                                        IdHuCertificate = item.ID_HU_CERTIFICATE,
                                        EmployeeId = item.EMPLOYEE_ID,
                                        StatusRecord = item.STATUS_RECORD
                                    }
                                    ).ToListAsync();

                var query3 = await (from p in _dbContext.PortalCertificates.Where(x => x.EMPLOYEE_ID == id).DefaultIfEmpty()
                                        // kết hợp dữ liệu (join)
                                    from so1 in _dbContext.SysOtherLists.Where(x => x.ID == p.TYPE_CERTIFICATE).DefaultIfEmpty()
                                    from so2 in _dbContext.SysOtherLists.Where(x => x.ID == p.SCHOOL_ID).DefaultIfEmpty()
                                    from so3 in _dbContext.SysOtherLists.Where(x => x.ID == p.TYPE_TRAIN).DefaultIfEmpty()
                                    from so4 in _dbContext.SysOtherLists.Where(x => x.ID == p.LEVEL_ID).DefaultIfEmpty()
                                    from so5 in _dbContext.SysOtherLists.Where(x => x.ID == p.LEVEL_TRAIN).DefaultIfEmpty()

                                    select new HuCertificateEditDTO
                                    {
                                        Id = p.ID,
                                        Name = p.NAME,
                                        TypeCertificate = p.TYPE_CERTIFICATE,
                                        TypeCertificateName = so1.NAME,
                                        EffectFrom = p.EFFECT_FROM,
                                        EffectTo = p.EFFECT_TO,
                                        IsPrime = p.IS_PRIME,
                                        TrainFromDate = p.TRAIN_FROM_DATE,
                                        TrainToDate = p.TRAIN_TO_DATE,
                                        ContentTrain = p.CONTENT_TRAIN,
                                        SchoolId = p.SCHOOL_ID,
                                        SchoolName = so2.NAME,
                                        Year = p.YEAR,
                                        Mark = p.MARK,
                                        Classification = p.CLASSIFICATION,
                                        TypeTrain = p.TYPE_TRAIN,
                                        TypeTrainName = so3.NAME,
                                        Major = p.MAJOR,
                                        LevelId = p.LEVEL_ID,
                                        LevelName = so4.NAME,
                                        LevelTrain = p.LEVEL_TRAIN,
                                        LevelTrainName = so5.NAME,
                                        FileName = p.FILE_NAME,
                                        Remark = p.REMARK,
                                        IsSavePortal = p.IS_SAVE,
                                        //IsSendPortal = p.IS_SEND_PORTAL,
                                        //IsApprovePortal = p.IS_APPROVE_PORTAL,
                                        //ModelChange = p.MODEL_CHANGE,
                                        //IdHuCertificate = p.ID_HU_CERTIFICATE,
                                        EmployeeId = p.EMPLOYEE_ID,
                                        //StatusRecord = p.STATUS_RECORD
                                    }
                                    ).ToListAsync();


                // nếu query2 khác null, lớn hơn 0
                // thì thực hiện nối 2 danh sách lại với nhau
                if (query2 != null || query2.Count() > 0)
                {
                    foreach (var item in query2)
                    {
                        query1.Add(item);
                    }
                }
                if (query3 != null || query3.Count() > 0)
                {
                    foreach (var a in query3)
                    {
                        query1.Add(a);
                    }
                }


                return new FormatedResponse()
                {
                    InnerBody = query1
                };
            }
            catch (Exception ex)
            {
                // in ra ngoại lệ
                return new FormatedResponse() { MessageCode = ex.Message };
            }
        }
        #endregion



        #region GetByIdHuCertificateEdit phiên bản 1 (ĐANG CMT CODE)
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

        //public async Task<FormatedResponse> GetByIdHuCertificateEdit(long? Id, string? StatusRecord)
        //{
        //    // đầu tiên
        //    // phải sử dụng try ... catch ...
        //    // để bắt ngoại lệ
        //    try
        //    {
        //        // lấy bản ghi trong bảng tạm
        //        // là loại đã bị người dùng bấm button lưu
        //        if (StatusRecord == "saved")
        //        {
        //            // lấy bản ghi theo ID
        //            // trong bảng tạm
        //            var record = await (
        //                                from data in _dbContext.HuCertificateEdits.Where(x => x.ID == Id)

        //                                    // kết hợp dữ liệu
        //                                from tham_chieu1 in _dbContext.SysOtherLists.Where(x => x.ID == data.TYPE_CERTIFICATE).DefaultIfEmpty()
        //                                from tham_chieu2 in _dbContext.SysOtherLists.Where(x => x.ID == data.SCHOOL_ID).DefaultIfEmpty()
        //                                from tham_chieu3 in _dbContext.SysOtherLists.Where(x => x.ID == data.TYPE_TRAIN).DefaultIfEmpty()
        //                                from tham_chieu4 in _dbContext.SysOtherLists.Where(x => x.ID == data.LEVEL_ID).DefaultIfEmpty()
        //                                from tham_chieu5 in _dbContext.SysOtherLists.Where(x => x.ID == data.LEVEL_TRAIN).DefaultIfEmpty()

        //                                select new HuCertificateEditDTO
        //                                {
        //                                    // trường 0:
        //                                    // Id của bảng tạm
        //                                    Id = data.ID,

        //                                    // trường 1:
        //                                    // Tên bằng cấp/chứng chỉ
        //                                    Name = data.NAME,

        //                                    // trường 2:
        //                                    // Loại bằng cấp/Chứng chỉ
        //                                    TypeCertificate = data.TYPE_CERTIFICATE,
        //                                    TypeCertificateName = tham_chieu1.NAME,

        //                                    // trường 3:
        //                                    // Ngày chứng chỉ có hiệu lực
        //                                    EffectFrom = data.EFFECT_FROM,

        //                                    // trường 4:
        //                                    // Ngày chứng chỉ hết hạn
        //                                    EffectTo = data.EFFECT_TO,

        //                                    // trường 5:
        //                                    // Là bằng chính
        //                                    IsPrime = data.IS_PRIME,

        //                                    // trường 6:
        //                                    // Thời gian đào tạo từ
        //                                    TrainFromDate = data.TRAIN_FROM_DATE,

        //                                    // trường 7:
        //                                    // Thời gian đào tạo đến
        //                                    TrainToDate = data.TRAIN_TO_DATE,

        //                                    // trường 8:
        //                                    // Nội dung đào tạo
        //                                    ContentTrain = data.CONTENT_TRAIN,

        //                                    // trường 9:
        //                                    // Trường học
        //                                    SchoolId = data.SCHOOL_ID,
        //                                    SchoolName = tham_chieu2.NAME,

        //                                    // trường 10:
        //                                    // Năm
        //                                    Year = data.YEAR,

        //                                    // trường 11:
        //                                    // Điểm số
        //                                    Mark = data.MARK,

        //                                    // trường 12:
        //                                    // Xếp loại tốt nghiệp
        //                                    Classification = data.CLASSIFICATION,

        //                                    // trường 13:
        //                                    // Hình thức đào tạo
        //                                    TypeTrain = data.TYPE_TRAIN,
        //                                    TypeTrainName = tham_chieu3.NAME,

        //                                    // trường 14:
        //                                    // Chuyên môn
        //                                    Major = data.MAJOR,

        //                                    // trường 15:
        //                                    // Trình độ
        //                                    LevelId = data.LEVEL_ID,
        //                                    LevelName = tham_chieu4.NAME,

        //                                    // trường 16:
        //                                    // Trình độ chuyên môn
        //                                    LevelTrain = data.LEVEL_TRAIN,
        //                                    LevelTrainName = tham_chieu5.NAME,

        //                                    // trường 17:
        //                                    // File đính kèm
        //                                    FileName = data.FILE_NAME,

        //                                    // trường 18:
        //                                    // Ghi chú
        //                                    Remark = data.REMARK,

        //                                    // trường 19:
        //                                    // đã gửi portal
        //                                    IsSendPortal = data.IS_SEND_PORTAL,

        //                                    // trường 20:
        //                                    IsApprovePortal = data.IS_APPROVE_PORTAL,

        //                                    // trường 21:
        //                                    ModelChange = data.MODEL_CHANGE,

        //                                    // trường 22:
        //                                    IdHuCertificate = data.ID_HU_CERTIFICATE,

        //                                    // trường 23:
        //                                    EmployeeId = data.EMPLOYEE_ID,

        //                                    // trường 24:
        //                                    StatusRecord = data.STATUS_RECORD
        //                                }
        //                                ).FirstAsync();


        //            return new FormatedResponse()
        //            {
        //                // in ra 1 bản ghi trong bảng tạm
        //                InnerBody = record
        //            };
        //        }
        //        else
        //        {
        //            // lấy bản ghi theo ID
        //            // trong bảng chính
        //            var record = await (
        //                                from data in _dbContext.HuCertificates.Where(x => x.ID == Id)

        //                                    // kết hợp dữ liệu (join)
        //                                from tham_chieu1 in _dbContext.SysOtherLists.Where(x => x.ID == data.TYPE_CERTIFICATE).DefaultIfEmpty()
        //                                from tham_chieu2 in _dbContext.SysOtherLists.Where(x => x.ID == data.SCHOOL_ID).DefaultIfEmpty()
        //                                from tham_chieu3 in _dbContext.SysOtherLists.Where(x => x.ID == data.TYPE_TRAIN).DefaultIfEmpty()
        //                                from tham_chieu4 in _dbContext.SysOtherLists.Where(x => x.ID == data.LEVEL_ID).DefaultIfEmpty()
        //                                from tham_chieu5 in _dbContext.SysOtherLists.Where(x => x.ID == data.LEVEL_TRAIN).DefaultIfEmpty()

        //                                select new HuCertificateEditDTO
        //                                {
        //                                    // trường 0:
        //                                    // Id của bảng tạm
        //                                    Id = data.ID,

        //                                    // trường 1:
        //                                    // Tên bằng cấp/chứng chỉ
        //                                    Name = data.NAME,

        //                                    // trường 2:
        //                                    // Loại bằng cấp/Chứng chỉ
        //                                    TypeCertificate = data.TYPE_CERTIFICATE,
        //                                    TypeCertificateName = tham_chieu1.NAME,

        //                                    // trường 3:
        //                                    // Ngày chứng chỉ có hiệu lực
        //                                    EffectFrom = data.EFFECT_FROM,

        //                                    // trường 4:
        //                                    // Ngày chứng chỉ hết hạn
        //                                    EffectTo = data.EFFECT_TO,

        //                                    // trường 5:
        //                                    // Là bằng chính
        //                                    IsPrime = data.IS_PRIME,

        //                                    // trường 6:
        //                                    // Thời gian đào tạo từ
        //                                    TrainFromDate = data.TRAIN_FROM_DATE,

        //                                    // trường 7:
        //                                    // Thời gian đào tạo đến
        //                                    TrainToDate = data.TRAIN_TO_DATE,

        //                                    // trường 8:
        //                                    // Nội dung đào tạo
        //                                    ContentTrain = data.CONTENT_TRAIN,

        //                                    // trường 9:
        //                                    // Trường học
        //                                    SchoolId = data.SCHOOL_ID,
        //                                    SchoolName = tham_chieu2.NAME,

        //                                    // trường 10:
        //                                    // Năm
        //                                    Year = data.YEAR,

        //                                    // trường 11:
        //                                    // Điểm số
        //                                    Mark = data.MARK,

        //                                    // trường 12:
        //                                    // Xếp loại tốt nghiệp
        //                                    Classification = data.CLASSIFICATION,

        //                                    // trường 13:
        //                                    // Hình thức đào tạo
        //                                    TypeTrain = data.TYPE_TRAIN,
        //                                    TypeTrainName = tham_chieu3.NAME,

        //                                    // trường 14:
        //                                    // Chuyên môn
        //                                    Major = data.MAJOR,

        //                                    // trường 15:
        //                                    // Trình độ
        //                                    LevelId = data.LEVEL_ID,
        //                                    LevelName = tham_chieu4.NAME,

        //                                    // trường 16:
        //                                    // Trình độ chuyên môn
        //                                    LevelTrain = data.LEVEL_TRAIN,
        //                                    LevelTrainName = tham_chieu5.NAME,

        //                                    // trường 17:
        //                                    // File đính kèm
        //                                    FileName = data.FILE_NAME,

        //                                    // trường 18:
        //                                    // Ghi chú
        //                                    Remark = data.REMARK,

        //                                    // trường 19:
        //                                    // đã gửi portal
        //                                    IsSendPortal = null,

        //                                    // trường 20:
        //                                    IsApprovePortal = null,

        //                                    // trường 21:
        //                                    ModelChange = null,

        //                                    // trường 22:
        //                                    IdHuCertificate = null,

        //                                    // trường 23:
        //                                    EmployeeId = data.EMPLOYEE_ID
        //                                }
        //                                ).FirstAsync();


        //            return new FormatedResponse()
        //            {
        //                // in ra 1 bản ghi trong bảng chính
        //                InnerBody = record
        //            };
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        // in ra ngoại lệ
        //        return new FormatedResponse() { MessageCode = ex.Message };
        //    }
        //}
        #endregion

        #region GetByIdHuCertificateEdit phiên bản 2 (ĐANG DÙNG)
        public async Task<FormatedResponse> GetByIdHuCertificateEdit(long? Id, string? StatusRecord)
        {
            // đầu tiên
            // phải sử dụng try ... catch ...
            // để bắt ngoại lệ
            try
            {
                // bản ghi hiển thị ra ở Portal
                // để mà bấm bút chì ấy
                // thì là bản ghi (ở bảng chính)
                // nên phải call dữ liệu ở bảng chính
                // lấy theo ID của bảng chính
                var record = await (
                    from data in _dbContext.HuCertificates.Where(x => x.ID == Id)

                        // kết hợp dữ liệu (join)
                    from tham_chieu1 in _dbContext.SysOtherLists.Where(x => x.ID == data.TYPE_CERTIFICATE).DefaultIfEmpty()
                    from tham_chieu2 in _dbContext.SysOtherLists.Where(x => x.ID == data.SCHOOL_ID).DefaultIfEmpty()
                    from tham_chieu3 in _dbContext.SysOtherLists.Where(x => x.ID == data.TYPE_TRAIN).DefaultIfEmpty()
                    from tham_chieu4 in _dbContext.SysOtherLists.Where(x => x.ID == data.LEVEL_ID).DefaultIfEmpty()
                    from tham_chieu5 in _dbContext.SysOtherLists.Where(x => x.ID == data.LEVEL_TRAIN).DefaultIfEmpty()

                    select new HuCertificateEditDTO
                    {
                        // trường 0:
                        // Id của bảng tạm
                        Id = data.ID,

                        // trường 1:
                        // Tên bằng cấp/chứng chỉ
                        Name = data.NAME,

                        // trường 2:
                        // Loại bằng cấp/Chứng chỉ
                        TypeCertificate = data.TYPE_CERTIFICATE,
                        TypeCertificateName = tham_chieu1.NAME,

                        // trường 3:
                        // Ngày chứng chỉ có hiệu lực
                        EffectFrom = data.EFFECT_FROM,

                        // trường 4:
                        // Ngày chứng chỉ hết hạn
                        EffectTo = data.EFFECT_TO,

                        // trường 5:
                        // Là bằng chính
                        IsPrime = data.IS_PRIME,

                        // trường 6:
                        // Thời gian đào tạo từ
                        TrainFromDate = data.TRAIN_FROM_DATE,

                        // trường 7:
                        // Thời gian đào tạo đến
                        TrainToDate = data.TRAIN_TO_DATE,

                        // trường 8:
                        // Nội dung đào tạo
                        ContentTrain = data.CONTENT_TRAIN,

                        // trường 9:
                        // Trường học
                        SchoolId = data.SCHOOL_ID,
                        SchoolName = tham_chieu2.NAME,

                        // trường 10:
                        // Năm
                        Year = data.YEAR,

                        // trường 11:
                        // Điểm số
                        Mark = data.MARK,

                        // trường 12:
                        // Xếp loại tốt nghiệp
                        Classification = data.CLASSIFICATION,

                        // trường 13:
                        // Hình thức đào tạo
                        TypeTrain = data.TYPE_TRAIN,
                        TypeTrainName = tham_chieu3.NAME,

                        // trường 14:
                        // Chuyên môn
                        Major = data.MAJOR,

                        // trường 15:
                        // Trình độ
                        LevelId = data.LEVEL_ID,
                        LevelName = tham_chieu4.NAME,

                        // trường 16:
                        // Trình độ chuyên môn
                        LevelTrain = data.LEVEL_TRAIN,
                        LevelTrainName = tham_chieu5.NAME,

                        // trường 17:
                        // File đính kèm
                        FileName = data.FILE_NAME,

                        // trường 18:
                        // Ghi chú
                        Remark = data.REMARK,

                        // trường 19:
                        // đã gửi portal
                        IsSendPortal = null,

                        // trường 20:
                        IsApprovePortal = null,

                        // trường 21:
                        ModelChange = null,

                        // trường 22:
                        IdHuCertificate = null,

                        // trường 23:
                        EmployeeId = data.EMPLOYEE_ID
                    }).FirstAsync();

                return new FormatedResponse()
                {
                    // in ra 1 bản ghi trong bảng chính
                    InnerBody = record
                };
            }
            catch (Exception ex)
            {
                // in ra ngoại lệ
                return new FormatedResponse() { MessageCode = ex.Message };
            }
        }
        #endregion



        // khi bấm nút lưu ở Portal
        public async Task<FormatedResponse> ClickSave(HuCertificateEditDTO model)
        {
            // đầu tiên
            // phải sử dụng try ... catch ...
            // để bắt ngoại lệ
            try
            {
                // nếu người dùng bấm vào dấu cộng
                // người dùng nhập dữ liệu trên form
                // người dùng bấm lưu
                // thì chương trình sẽ chạy vào action ClickSave()
                // đặc điểm là không có ID
                // nếu không có ID thì thêm mới bản ghi trong bảng tạm
                // và thiết lập STATUS_RECORD = "saved"

                // không được
                // thiết lập IS_APPROVE_PORTAL = false
                // tại vì IS_APPROVE_PORTAL = false là đã gửi phê duyệt
                // nhưng chưa được phê duyệt
                if (model.Id == null || model.Id <= 0)
                {
                    // tạo đối tượng có kiểu HU_CERTIFICATE_EDIT
                    var record = new HU_CERTIFICATE_EDIT()
                    {
                        // trường 1:
                        // Tên bằng cấp/chứng chỉ
                        NAME = model.Name,

                        // trường 2:
                        // Loại bằng cấp/Chứng chỉ
                        TYPE_CERTIFICATE = model.TypeCertificate,

                        // trường 3:
                        // Ngày chứng chỉ có hiệu lực
                        EFFECT_FROM = model.EffectFrom,

                        // trường 4:
                        // Ngày chứng chỉ hết hạn
                        EFFECT_TO = model.EffectTo,

                        // trường 5:
                        // Là bằng chính
                        IS_PRIME = model.IsPrime,

                        // trường 6:
                        // Thời gian đào tạo từ
                        TRAIN_FROM_DATE = model.TrainFromDate,

                        // trường 7:
                        // Thời gian đào tạo đến
                        TRAIN_TO_DATE = model.TrainToDate,

                        // trường 8:
                        // Nội dung đào tạo
                        CONTENT_TRAIN = model.ContentTrain,

                        // trường 9:
                        // Trường học
                        SCHOOL_ID = model.SchoolId,

                        // trường 10:
                        // Năm
                        YEAR = model.Year,

                        // trường 11:
                        // Điểm số
                        MARK = model.Mark,

                        // trường 12:
                        // Xếp loại tốt nghiệp
                        CLASSIFICATION = model.Classification,

                        // trường 13:
                        // Hình thức đào tạo
                        TYPE_TRAIN = model.TypeTrain,

                        // trường 14:
                        // Chuyên môn
                        MAJOR = model.Major,

                        // trường 15:
                        // Trình độ
                        LEVEL_ID = model.LevelId,

                        // trường 16:
                        // Trình độ chuyên môn
                        LEVEL_TRAIN = model.LevelTrain,

                        // trường 17:
                        // File đính kèm
                        FILE_NAME = model.FileName,

                        // trường 18:
                        // Ghi chú
                        REMARK = model.Remark,

                        // trường 19:
                        // đã gửi portal
                        IS_SEND_PORTAL = null,

                        // trường 20:
                        IS_APPROVE_PORTAL = null,

                        // trường 21:
                        MODEL_CHANGE = null,

                        // trường 22:
                        ID_HU_CERTIFICATE = null,

                        // trường 23:
                        EMPLOYEE_ID = model.EmployeeId,

                        // trường 24:
                        // fix cứng là "saved"
                        // để ám chỉ đây là bản ghi đã bị bấm lưu
                        STATUS_RECORD = "saved"
                    };

                    // thêm mới bản ghi vào ngữ cảnh context
                    _dbContext.HuCertificateEdits.Add(record);

                    // lưu bản ghi xuống db
                    _dbContext.SaveChanges();
                }
                else
                {
                    // coi chừng cái ID này là của bảng chính nha
                    // làm sao để biết cái ID là của bảng chính
                    // chúng ta dựa vào STATUS_RECORD
                    // nếu STATUS_RECORD = "saved" thì bản ghi này trong bảng tạm
                    // nếu STATUS_RECORD = null thì bản ghi này trong bảng chính
                    if (model.StatusRecord == null)
                    {
                        // bản ghi trong bảng chính
                        // mà bị bấm lưu
                        // thì phải thêm mới bản ghi trong bảng tạm
                        // có đặc điểm là:
                        // 1. có ID_HU_CERTIFICATE
                        // 2. STATUS_RECORD được thiết lập là "saved"

                        // tạo đối tượng có kiểu HU_CERTIFICATE_EDIT
                        var dt = new HU_CERTIFICATE_EDIT()
                        {
                            // trường 1:
                            // Tên bằng cấp/chứng chỉ
                            NAME = model.Name,

                            // trường 2:
                            // Loại bằng cấp/Chứng chỉ
                            TYPE_CERTIFICATE = model.TypeCertificate,

                            // trường 3:
                            // Ngày chứng chỉ có hiệu lực
                            EFFECT_FROM = model.EffectFrom,

                            // trường 4:
                            // Ngày chứng chỉ hết hạn
                            EFFECT_TO = model.EffectTo,

                            // trường 5:
                            // Là bằng chính
                            IS_PRIME = model.IsPrime,

                            // trường 6:
                            // Thời gian đào tạo từ
                            TRAIN_FROM_DATE = model.TrainFromDate,

                            // trường 7:
                            // Thời gian đào tạo đến
                            TRAIN_TO_DATE = model.TrainToDate,

                            // trường 8:
                            // Nội dung đào tạo
                            CONTENT_TRAIN = model.ContentTrain,

                            // trường 9:
                            // Trường học
                            SCHOOL_ID = model.SchoolId,

                            // trường 10:
                            // Năm
                            YEAR = model.Year,

                            // trường 11:
                            // Điểm số
                            MARK = model.Mark,

                            // trường 12:
                            // Xếp loại tốt nghiệp
                            CLASSIFICATION = model.Classification,

                            // trường 13:
                            // Hình thức đào tạo
                            TYPE_TRAIN = model.TypeTrain,

                            // trường 14:
                            // Chuyên môn
                            MAJOR = model.Major,

                            // trường 15:
                            // Trình độ
                            LEVEL_ID = model.LevelId,

                            // trường 16:
                            // Trình độ chuyên môn
                            LEVEL_TRAIN = model.LevelTrain,

                            // trường 17:
                            // File đính kèm
                            FILE_NAME = model.FileName,

                            // trường 18:
                            // Ghi chú
                            REMARK = model.Remark,

                            // trường 19:
                            // đã gửi portal
                            IS_SEND_PORTAL = null,

                            // trường 20:
                            IS_APPROVE_PORTAL = null,

                            // trường 21:
                            MODEL_CHANGE = null,

                            // trường 22:
                            // model.Id là id của bảng chính đấy
                            ID_HU_CERTIFICATE = model.Id,

                            // trường 23:
                            EMPLOYEE_ID = model.EmployeeId,

                            // trường 24:
                            // fix cứng là "saved"
                            // để ám chỉ đây là bản ghi đã bị bấm lưu
                            STATUS_RECORD = "saved"
                        };

                        // thêm mới bản ghi vào ngữ cảnh context
                        _dbContext.HuCertificateEdits.Add(dt);

                        // lưu bản ghi xuống db
                        _dbContext.SaveChanges();


                        return new FormatedResponse()
                        {
                            InnerBody = model
                        };
                    }



                    // trường hợp else
                    // tức là bản ghi có ID
                    // mà cái ID này là ID trong bảng tạm HU_CERTIFICATE_EDIT
                    // NÚT LƯU THÌ ID CỦA BẢNG TẠM

                    // chương trình sẽ lấy bản ghi dựa trên ID
                    // mà bạn truyền vào
                    // lấy bản ghi trong bảng tạm
                    // rồi sửa cái bản ghi đấy
                    // sửa xong thì .SaveChanges()

                    var record = await _dbContext.HuCertificateEdits
                                        .Where(x => x.ID == model.Id)
                                        .FirstAsync();


                    // trường 1:
                    // Tên bằng cấp/chứng chỉ
                    record.NAME = model.Name;

                    // trường 2:
                    // Loại bằng cấp/Chứng chỉ
                    record.TYPE_CERTIFICATE = model.TypeCertificate;

                    // trường 3:
                    // Ngày chứng chỉ có hiệu lực
                    record.EFFECT_FROM = model.EffectFrom;

                    // trường 4:
                    // Ngày chứng chỉ hết hạn
                    record.EFFECT_TO = model.EffectTo;

                    // trường 5:
                    // Là bằng chính
                    record.IS_PRIME = model.IsPrime;

                    // trường 6:
                    // Thời gian đào tạo từ
                    record.TRAIN_FROM_DATE = model.TrainFromDate;

                    // trường 7:
                    // Thời gian đào tạo đến
                    record.TRAIN_TO_DATE = model.TrainToDate;

                    // trường 8:
                    // Nội dung đào tạo
                    record.CONTENT_TRAIN = model.ContentTrain;

                    // trường 9:
                    // Trường học
                    record.SCHOOL_ID = model.SchoolId;

                    // trường 10:
                    // Năm
                    record.YEAR = model.Year;

                    // trường 11:
                    // Điểm số
                    record.MARK = model.Mark;

                    // trường 12:
                    // Xếp loại tốt nghiệp
                    record.CLASSIFICATION = model.Classification;

                    // trường 13:
                    // Hình thức đào tạo
                    record.TYPE_TRAIN = model.TypeTrain;

                    // trường 14:
                    // Chuyên môn
                    record.MAJOR = model.Major;

                    // trường 15:
                    // Trình độ
                    record.LEVEL_ID = model.LevelId;

                    // trường 16:
                    // Trình độ chuyên môn
                    record.LEVEL_TRAIN = model.LevelTrain;

                    // trường 17:
                    // File đính kèm
                    record.FILE_NAME = model.FileName;

                    // trường 18:
                    // Ghi chú
                    record.REMARK = model.Remark;

                    // lưu bản ghi vừa sửa xuống db
                    _dbContext.SaveChanges();
                }

                return new FormatedResponse()
                {
                    InnerBody = model
                };
            }
            catch (Exception ex)
            {
                // in ra ngoại lệ
                return new FormatedResponse() { MessageCode = ex.Message };
            }
        }




        // khai báo phương thức
        // để lấy danh sách trạng thái phê duyệt
        public async Task<FormatedResponse> GetListNameOfApprove()
        {
            // đầu tiên
            // sử dụng try... catch...
            // để bắt ngoại lệ
            try
            {
                // lấy các bản ghi
                // trong bảng SYS_OTHER_LIST
                // có TYPE_ID = 65

                // nhưng mà thực tế số 65
                // sau này sẽ là ID rác

                // nên bạn phải join thêm với bảng SYS_OTHER_LIST_TYPE
                // với điều kiện kết nối
                // là [SYS_OTHER_LIST].TYPE_ID = [SYS_OTHER_LIST_TYPE].ID
                // thêm điều kiện where là [SYS_OTHER_LIST_TYPE].CODE = 'STATUS'



                // tạo biến truy vấn "query"
                var query = await (
                                    from item in _dbContext.SysOtherLists

                                        // kết hợp dữ liệu (join)
                                    join tham_chieu1 in _dbContext.SysOtherListTypes

                                    // on outerKey equals innerKey
                                    // outerKey: khóa ngoại
                                    // innerKey: khóa bên trong (có thể là khóa chính)
                                    on item.TYPE_ID equals tham_chieu1.ID

                                    where (tham_chieu1.CODE).ToUpper() == "STATUS"

                                            // chỗ này chỉ lấy
                                            // đã phê duyệt (DD)
                                            // hoặc không phê duyệt (TCPD)
                                            && (item.CODE == "DD" || item.CODE == "TCPD")

                                    select new
                                    {
                                        // ID của bảng SYS_OTHER_LIST
                                        Id = item.ID,

                                        // "Tên phê duyệt" để in ra ngoài
                                        Name = item.NAME,

                                        // mã của từng loại trạng thái
                                        Code = item.CODE
                                    }
                                    ).ToListAsync();

                return new FormatedResponse()
                {
                    InnerBody = query
                };
            }
            catch (Exception ex)
            {
                // in ra ngoại lệ
                return new FormatedResponse() { MessageCode = ex.Message };
            }
        }




        // khai báo phương thức
        // để lấy tên của cái trạng thái
        // để phục vụ cho 1 ô input
        public async Task<FormatedResponse> GetListNameOfApproveById(long id)
        {
            try
            {
                // cái tham số id
                // đây là ID_SYS_OTHER_LIST_APPROVE (khóa ngoại)
                // nhưng nó cũng bằng ID (khóa chính) của SYS_OTHER_LIST
                var item = await _dbContext.SysOtherLists
                            .Where(x => x.ID == id)
                            .Select(x => new { Id = x.ID, Name = x.NAME })
                            .FirstAsync();

                return new FormatedResponse()
                {
                    InnerBody = item
                };
            }
            catch (Exception ex)
            {
                // in ra ngoại lệ
                return new FormatedResponse() { MessageCode = ex.Message };
            }
        }


        public async Task<FormatedResponse> GetListCertificate(long employeeId)
        {
            var entity = _uow.Context.Set<HU_CERTIFICATE>().AsNoTracking().AsQueryable();
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
                                where p.EMPLOYEE_ID == employeeId
                                select new HuCertificateDTO
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
                                    FileName = p.FILE_NAME != null ? "File Upload" : "",
                                    //IsPrimeStr = p.IS_PRIME == true ? "Là bằng chính" : "Không là bằng chính",
                                    Classification = p.CLASSIFICATION
                                }).ToListAsync();
            return new FormatedResponse() { InnerBody = joined };
        }

        public async Task<FormatedResponse> GetByIdCertificate(long id)
        {
            try
            {
                    var joined = (from p in _dbContext.HuCertificateEdits.Where(x => x.ID == id).DefaultIfEmpty()
                                  from e in _dbContext.HuEmployees.Where(x => x.ID == p.EMPLOYEE_ID).DefaultIfEmpty()
                                  from cv in _dbContext.HuEmployeeCvs.Where(x => x.ID == e.PROFILE_ID).DefaultIfEmpty()
                                  from t in _dbContext.HuPositions.Where(x => x.ID == e.POSITION_ID).DefaultIfEmpty()
                                  from o in _dbContext.HuOrganizations.Where(x => x.ID == e.ORG_ID).DefaultIfEmpty()
                                  from type in _dbContext.SysOtherLists.Where(x => x.ID == p.TYPE_CERTIFICATE).DefaultIfEmpty()
                                  from lvtrain in _dbContext.SysOtherLists.Where(x => x.ID == p.LEVEL_TRAIN).DefaultIfEmpty()
                                  from typeTrain in _dbContext.SysOtherLists.Where(x => x.ID == p.TYPE_TRAIN).DefaultIfEmpty()
                                  from school in _dbContext.SysOtherLists.Where(x => x.ID == p.SCHOOL_ID).DefaultIfEmpty()
                                  from lv in _dbContext.SysOtherLists.Where(x => x.ID == p.LEVEL_ID).DefaultIfEmpty()
                                      // JOIN OTHER ENTITIES BASED ON THE BUSINESS
                                  select new
                                  {
                                      Id = p.ID,
                                      Name = p.NAME,
                                      TrainFromDate = p.TRAIN_FROM_DATE,
                                      TrainToDate = p.TRAIN_TO_DATE,
                                      EffectFrom = p.EFFECT_FROM,
                                      EffectTo = p.EFFECT_TO,
                                      Major = p.MAJOR,
                                      ContentTrain = p.CONTENT_TRAIN,
                                      Year = p.YEAR,
                                      Mark = p.MARK,
                                      IsPrime = p.IS_PRIME,
                                      Level = p.LEVEL,
                                      Remark = p.REMARK,
                                      EmployeeId = p.EMPLOYEE_ID,
                                      TypeCertificateName = type.NAME ?? "",
                                      LevelTrainName = lvtrain.NAME ?? "",
                                      SchoolName = school.NAME ?? "",
                                      Classification = p.CLASSIFICATION ?? "",
                                      TypeTrainName = typeTrain.NAME ?? "",
                                      FileName = p.FILE_NAME,
                                      LevelName = lv.NAME ?? "",
                                      Reason = p.REASON ?? "",
                                      ModelChange = p.MODEL_CHANGE,
                                  }).ToList();

                    return new FormatedResponse() { InnerBody = joined };

            } catch (Exception ex)
            {
                return new FormatedResponse() { MessageCode = CommonMessageCode.ENTITY_NOT_FOUND, ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400 };
            }

        }


        public async Task<FormatedResponse> GetCertificateSave(long employeeId)
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
                                        FileName = p.FILE_NAME != null ? "File Upload" : "",
                                        //IsPrimeStr = p.IS_PRIME == true ? "Là bằng chính" : "Không là bằng chính",
                                        Classification = p.CLASSIFICATION
                                    }).ToListAsync();
                return new FormatedResponse() { InnerBody = joined };
            }
            catch (Exception ex)
            {

                return new FormatedResponse() { StatusCode = EnumStatusCode.StatusCode500, ErrorType = EnumErrorType.UNCATCHABLE, MessageCode = ex.Message };
            }
        }

        public async Task<FormatedResponse> GetCertificateApproving(long employeeId)
        {
            try
            {
                var entity = _uow.Context.Set<HU_CERTIFICATE_EDIT>().AsNoTracking().AsQueryable();
                var employees = _uow.Context.Set<HU_EMPLOYEE>().AsNoTracking().AsQueryable();
                var employeeCvs = _uow.Context.Set<HU_EMPLOYEE_CV>().AsNoTracking().AsQueryable();
                var positions = _uow.Context.Set<HU_POSITION>().AsNoTracking().AsQueryable();
                var organizations = _uow.Context.Set<HU_ORGANIZATION>().AsNoTracking().AsQueryable();
                var otherLists = _uow.Context.Set<SYS_OTHER_LIST>().AsNoTracking().AsQueryable();
                var joined = await(from p in entity
                                   from e in employees.Where(x => x.ID == p.EMPLOYEE_ID).DefaultIfEmpty()
                                   from cv in employeeCvs.Where(x => x.ID == e.PROFILE_ID).DefaultIfEmpty()
                                   from t in positions.Where(x => x.ID == e.POSITION_ID).DefaultIfEmpty()
                                   from o in organizations.Where(x => x.ID == e.ORG_ID).DefaultIfEmpty()
                                   from type in otherLists.Where(x => x.ID == p.TYPE_CERTIFICATE).DefaultIfEmpty()
                                   from lvtrain in otherLists.Where(x => x.ID == p.LEVEL_TRAIN).DefaultIfEmpty()
                                   from typeTrain in otherLists.Where(x => x.ID == p.TYPE_TRAIN).DefaultIfEmpty()
                                   from school in otherLists.Where(x => x.ID == p.SCHOOL_ID).DefaultIfEmpty()
                                   from lv in otherLists.Where(x => x.ID == p.LEVEL_ID).DefaultIfEmpty()
                                   where p.EMPLOYEE_ID == employeeId && p.IS_APPROVE_PORTAL == false && p.IS_SEND_PORTAL == true && p.STATUS_ID == OtherConfig.STATUS_WAITING
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
                                       FileName = p.FILE_NAME != null ? "File Upload" : "",
                                       //IsPrimeStr = p.IS_PRIME == true ? "Là bằng chính" : "Không là bằng chính",
                                       Classification = p.CLASSIFICATION
                                   }).ToListAsync();
                return new FormatedResponse() { InnerBody = joined };
            }
            catch (Exception ex)
            {

                return new FormatedResponse() { StatusCode = EnumStatusCode.StatusCode500, ErrorType = EnumErrorType.UNCATCHABLE, MessageCode = ex.Message };
            }
        }
        public async Task<FormatedResponse> GetCertificateById(long id)
        {
            try
            {
                var response = await (from c in _dbContext.HuCertificates
                                      where c.ID == id
                                      select new HuCertificateDTO()
                                      {
                                          Id = c.ID,
                                          Name = c.NAME,
                                          TrainFromDate = c.TRAIN_FROM_DATE,
                                          TrainToDate = c.TRAIN_TO_DATE,
                                          EffectFrom = c.EFFECT_FROM,
                                          EffectTo = c.EFFECT_TO,
                                          Major = c.MAJOR,
                                          ContentTrain = c.CONTENT_TRAIN,
                                          Year = c.YEAR,
                                          Mark = c.MARK,
                                          IsPrime = c.IS_PRIME,
                                          Level = c.LEVEL,
                                          Remark = c.REMARK,
                                          EmployeeId = c.EMPLOYEE_ID,
                                          TypeCertificate = c.TYPE_CERTIFICATE,
                                          LevelTrain = c.LEVEL_TRAIN,
                                          SchoolId = c.SCHOOL_ID,
                                          Classification = c.CLASSIFICATION,
                                          TypeTrain = c.TYPE_TRAIN,
                                          FileName = c.FILE_NAME,
                                          LevelId = c.LEVEL_ID,
                                      }).SingleAsync();
                return new FormatedResponse() { InnerBody = response };
            }
            catch (Exception ex)
            {

                return new FormatedResponse() { StatusCode = EnumStatusCode.StatusCode500, ErrorType = EnumErrorType.UNCATCHABLE, MessageCode = ex.Message };
            }
        }
        public async Task<FormatedResponse> GetCertificateEditSaveById(long id)
        {
            try
            {
                var response = await(from c in _dbContext.HuCertificateEdits.Where(x => x.ID == id)
                                     select new HuCertificateEditDTO()
                                     {
                                         Id = c.ID,
                                         Name = c.NAME,
                                         TrainFromDate = c.TRAIN_FROM_DATE,
                                         TrainToDate = c.TRAIN_TO_DATE,
                                         EffectFrom = c.EFFECT_FROM,
                                         EffectTo = c.EFFECT_TO,
                                         Major = c.MAJOR,
                                         ContentTrain = c.CONTENT_TRAIN,
                                         Year = c.YEAR,
                                         Mark = c.MARK,
                                         IsPrime = c.IS_PRIME,
                                         Level = c.LEVEL,
                                         Remark = c.REMARK,
                                         EmployeeId = c.EMPLOYEE_ID,
                                         TypeCertificate = c.TYPE_CERTIFICATE,
                                         LevelTrain = c.LEVEL_TRAIN,
                                         SchoolId = c.SCHOOL_ID,
                                         Classification = c.CLASSIFICATION,
                                         TypeTrain = c.TYPE_TRAIN,
                                         FileName = c.FILE_NAME,
                                         LevelId = c.LEVEL_ID,
                                         IsSavePortal = c.IS_SAVE_PORTAL,
                                     }).SingleAsync();
                return new FormatedResponse() { InnerBody = response };
            }
            catch (Exception ex)
            {

                return new FormatedResponse() { StatusCode = EnumStatusCode.StatusCode500, ErrorType = EnumErrorType.UNCATCHABLE, MessageCode = ex.Message };
            }
        }

        public async Task<FormatedResponse> SendApproveCertificate(HuCertificateEditDTO request)
        {
            bool pathMode = true;
            string sid = "";
            var getOtherList = (from t in _uow.Context.Set<SYS_OTHER_LIST_TYPE>().Where(x => x.CODE == "STATUS")
                                from o in _uow.Context.Set<SYS_OTHER_LIST>().Where(x => x.CODE == "CD")
                                select new { Id = o.ID }).FirstOrDefault();
            var getData = _uow.Context.Set<HU_CERTIFICATE_EDIT>().Where(x => x.EMPLOYEE_ID == request.EmployeeId && x.IS_SEND_PORTAL == true && x.IS_APPROVE_PORTAL == false);
            if (getData.Any())
            {
                return new FormatedResponse() { StatusCode = EnumStatusCode.StatusCode400, MessageCode = CommonMessageCode.HAD_RECORD_IS_APPROVING };
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
                else
                {
                    string[] arrModelChanges = {"typeCertificate","name","effectFrom","trainFromDate","trainToDate",
                                            "mojor","levelTrain","contentTrain","year","mark","schoolId",
                                            "typeTrain","codeCertificate","classification","fileName","remark",
                                            "levelId","level","effectTo","reason"};
                    request.ModelChange = string.Join(";", arrModelChanges);
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
                        return new FormatedResponse()
                        {
                            ErrorType = EnumErrorType.CATCHABLE,
                            StatusCode = EnumStatusCode.StatusCode400,
                            MessageCode = CommonMessageCode.ASSIGN_TO_VALUE_DOES_NOT_MATCH_ANY_COLUMN + ": FirstAttachment"
                        };
                    }
                }

                var updateResponse = await _genericRepository.Update(_uow, request, sid, pathMode);
                return updateResponse;  
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
                        return new FormatedResponse()
                        {
                            ErrorType = EnumErrorType.CATCHABLE,
                            StatusCode = EnumStatusCode.StatusCode400,
                            MessageCode = CommonMessageCode.ASSIGN_TO_VALUE_DOES_NOT_MATCH_ANY_COLUMN + ": FirstAttachment"
                        };
                    }
                }

                var updateResponse = await _genericRepository.Update(_uow, request, sid, pathMode);
                return updateResponse;
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
                        return new FormatedResponse()
                        {
                            ErrorType = EnumErrorType.CATCHABLE,
                            StatusCode = EnumStatusCode.StatusCode400,
                            MessageCode = CommonMessageCode.ASSIGN_TO_VALUE_DOES_NOT_MATCH_ANY_COLUMN + ": FirstAttachment"
                        };
                    }
                }
                string[] arrModelChanges = {"typeCertificate","name","effectFrom","trainFromDate","trainToDate",
                                            "mojor","levelTrain","contentTrain","year","mark","schoolId",
                                            "typeTrain","codeCertificate","classification","fileName","remark",
                                            "levelId","level","effectTo","reason"};
                request.ModelChange = string.Join(";", arrModelChanges);

                var createResponse = await _genericRepository.Create(_uow, request, sid);
                return createResponse;
            }

            request.IsSendPortal = true;
            request.IsSavePortal = false;
            request.IsApprovePortal = false;
            request.StatusId = getOtherList?.Id;
            request.IdHuCertificate = request.Id;
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
                    return new FormatedResponse()
                    {
                        ErrorType = EnumErrorType.CATCHABLE,
                        StatusCode = EnumStatusCode.StatusCode400,
                        MessageCode = CommonMessageCode.ASSIGN_TO_VALUE_DOES_NOT_MATCH_ANY_COLUMN + ": FirstAttachment"
                    };
                }
            }
            var response = await _genericRepository.Create(_uow, request, sid);
            return response;
        }

    }
}

