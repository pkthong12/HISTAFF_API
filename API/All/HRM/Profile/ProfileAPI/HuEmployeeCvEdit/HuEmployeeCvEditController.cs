using API.All.DbContexts;
using API.DTO;
using API.Main;
using Common.DataAccess;
using Common.Interfaces;
using CORE.DTO;
using CORE.Enum;
using CORE.GenericUOW;
using CORE.StaticConstant;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace API.Controllers.HuEmployeeCvEdit
{
    [ApiExplorerSettings(GroupName = "089-PROFILE-HU_EMPLOYEE_CV_EDIT")]
    [ApiController]
    [HiStaffAuthorize]
    [Route("api/[controller]/[action]")]
    public class HuEmployeeCvEditController : ControllerBase
    {
        private readonly GenericUnitOfWork _uow;
        private readonly IHuEmployeeCvEditRepository _HuEmployeeCvEditRepository;
        private readonly AppSettings _appSettings;
        private readonly FullDbContext _dbContext;

        public HuEmployeeCvEditController(
            FullDbContext dbContext,
            IOptions<AppSettings> options,
            FullDbContext context
        )
        {
            _uow = new GenericUnitOfWork(dbContext);
            _HuEmployeeCvEditRepository = new HuEmployeeCvEditRepository(dbContext, _uow);
            _appSettings = options.Value;


            _dbContext = context;
            QueryData = new SqlQueryDataTemplate(_dbContext);
        }

        [HttpGet]
        public async Task<IActionResult> ReadAll()
        {
            var response = await _HuEmployeeCvEditRepository.ReadAll();
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> ReadAllByNumberKey(ReadAllByKeyLongRequest request)
        {
            var response = await _HuEmployeeCvEditRepository.ReadAllByKey(request.Key, request.Value);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> ReadAllByStringKey(ReadAllByKeyStringRequest request)
        {
            var response = await _HuEmployeeCvEditRepository.ReadAllByKey(request.Key, request.Value);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> QueryList(GenericQueryListDTO<HuEmployeeCvEditDTO> request)
        {
            try
            {
                var response = await _HuEmployeeCvEditRepository.SinglePhaseQueryList(request);

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
            var response = await _HuEmployeeCvEditRepository.GetById(id);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetByStringId(string id)
        {
            var response = await _HuEmployeeCvEditRepository.GetById(id);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Create(HuEmployeeCvEditDTO model)
        {
            // lấy bản ghi nguyên mẫu (gọi là bản ghi ban đầu cũng được)
            // nó chính là bản ghi trong bảng HU_EMPLOYEE_CV
            // lấy cái bản ghi này
            // đem so sánh với model
            // kết quả thu được
            // danh danh tên các trường đã bị thay đổi

            var employee_id = model.EmployeeId;
            var profile_id = _dbContext.HuEmployees.Where(x => x.ID == employee_id).Select(x => x.PROFILE_ID).FirstOrDefault();

            // cái biến "record" chính là bản ghi ban đầu (chưa sửa)
            // trong bảng HU_EMPLOYEE_CV
            var record = _dbContext.HuEmployeeCvs.Where(x => x.ID == profile_id).FirstOrDefault();


            // tạo biến "ds_KhacNhau"
            // để lưu những tên trường sửa đổi
            // để "lập trình viên" biết được
            // người dùng sửa những cái trường nào
            List<string> ds_KhacNhau = new List<string>();


            // bạn có thể dùng câu lệnh điều kiện If()
            // để If() tám lần
            // vì có 8 trường mà người dùng có thể sửa mà


            // kiểm tra 1:
            // xem người dùng có sửa "Trình độ văn hóa" không
            if (record?.EDUCATION_LEVEL_ID != model.EducationLevelId)
            {
                // nếu record?.EDUCATION_LEVEL_ID == model.EducationLevelId
                // thì tức là người dùng không sửa
                // cái trường "Trình độ văn hóa" nha

                ds_KhacNhau.Add("EducationLevelId");
            }


            // kiểm tra 2:
            // xem người dùng có sửa "Trình độ học vấn" không
            if (record?.LEARNING_LEVEL_ID != model.LearningLevelId)
            {
                ds_KhacNhau.Add("LearningLevelId");
            }


            // kiểm tra 3:
            // xem người dùng có sửa "Trình độ chuyên môn 1" không
            if (record?.QUALIFICATIONID != model.Qualificationid)
            {
                ds_KhacNhau.Add("Qualificationid");
            }


            // kiểm tra 4:
            // xem người dùng có sửa "Trình độ chuyên môn 2" không
            if (record?.QUALIFICATIONID_2 != model.Qualificationid2)
            {
                ds_KhacNhau.Add("Qualificationid2");
            }

            // kiểm tra 5:
            // xem người dùng có sửa "Trình độ chuyên môn 3" không
            if (record?.QUALIFICATIONID_3 != model.Qualificationid3)
            {
                ds_KhacNhau.Add("Qualificationid3");
            }


            // kiểm tra 6:
            // xem người dùng có sửa "Hình thức đào tạo 1" không
            if (record?.TRAINING_FORM_ID != model.TrainingFormId)
            {
                ds_KhacNhau.Add("TrainingFormId");
            }


            // kiểm tra 7:
            // xem người dùng có sửa "Hình thức đào tạo 2" không
            if (record?.TRAINING_FORM_ID_2 != model.TrainingFormId2)
            {
                ds_KhacNhau.Add("TrainingFormId2");
            }


            // kiểm tra 8:
            // xem người dùng có sửa "Hình thức đào tạo 3" không
            if (record?.TRAINING_FORM_ID_3 != model.TrainingFormId3)
            {
                ds_KhacNhau.Add("TrainingFormId3");
            }


            // chuyển đối tượng "danh sách khác nhau"
            // thành chuỗi
            // chứa các tên trường bị sửa
            string str_KhacNhau = string.Join(", ", ds_KhacNhau);


            // nếu str_KhacNhau = ""
            // tức là người dùng không sửa
            // thì in ra thông báo cho người dùng
            if (str_KhacNhau == "" || str_KhacNhau == string.Empty || str_KhacNhau == null)
            {
                // thì in ra thông báo
                // người dùng chưa thay đổi
                // nên không cho bấm gửi phê duyệt

                return Ok(new FormatedResponse()
                {
                    StatusCode = EnumStatusCode.StatusCode500,
                    MessageCode = @"
                        Bạn chưa sửa bất kỳ dữ liệu nào của trình độ học vấn,
                        bạn phải thay đổi ít nhất 1 trường bất kỳ thì mới được gửi phê duyệt
                    "
                });
            }


            // điền cái biến "str_KhacNhau"
            // vào trường "ModelChange"
            // là danh sách các trường bị người dùng sửa trong Portal
            model.ModelChange = str_KhacNhau;


            // => Bởi vì biến "model"
            // sẽ được truyền vào phương thức Create()
            // nên viết code xử lý cho model là chuẩn rồi


            // câu hỏi:
            // bạn có thể sửa mấy trường
            // của trang "Trình độ học vấn" ở "Portal VNS"

            // trả lời:
            // bạn có thể sửa 8 trường

            // danh sách:
            // 1. Trình độ văn hóa              (educationLevelId)
            // 2. Trình độ học vấn              (learningLevelId)
            // 3. Trình độ chuyên môn 1         (qualificationid)
            // 4. Trình độ chuyên môn 2         (qualificationid2)
            // 5. Trình độ chuyên môn 3         (qualificationid3)
            // 6. Hình thức đào tạo 1           (trainingFormId)
            // 7. Hình thức đào tạo 2           (trainingFormId2)
            // 8. Hình thức đào tạo 3           (trainingFormId3)


            // chỉ định (directive) cứng giá trị cho
            // IsSendPortal là true
            model.IsSendPortal = true;


            // chỉ định (directive) cứng giá trị cho
            // IsSendPortalEducation là true
            model.IsSendPortalEducation = true;


            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();

            var response = await _HuEmployeeCvEditRepository.Create(_uow, model, sid);

            return Ok(response);
        }
        [HttpPost]
        public async Task<IActionResult> CreateRange(List<HuEmployeeCvEditDTO> models)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _HuEmployeeCvEditRepository.CreateRange(_uow, models, sid);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Update(HuEmployeeCvEditDTO model)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _HuEmployeeCvEditRepository.Update(_uow, model, sid);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateRange(List<HuEmployeeCvEditDTO> models)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _HuEmployeeCvEditRepository.UpdateRange(_uow, models, sid);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(HuEmployeeCvEditDTO model)
        {
            if (model.Id != null)
            {
                var response = await _HuEmployeeCvEditRepository.Delete(_uow, (long)model.Id);
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
            var response = await _HuEmployeeCvEditRepository.DeleteIds(_uow, model.Ids);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteStringIds(StringIdsRequest model)
        {
            var response = await _HuEmployeeCvEditRepository.DeleteIds(_uow, model.Ids);
            return Ok(response);
        }


        // khai báo thuộc tính "QueryData"
        // để gọi Stored Procedure
        // của SQL Server
        protected AbsQueryDataTemplate QueryData;


        // action duyệt trình độ học vấn
        [HttpPost]
        public async Task<IActionResult> ApproveEducationEdit(List<long>? listId)
        {
            // tham số listId
            // chính là mảng các ID
            // được post từ trang "phê duyệt trình độ học vấn"


            /*
                THIẾT LẬP CÂU LỆNH TRUY VẤN T-SQL
                (T-SQL là Transact-SQL)

                Transact dịch ra là giao dịch

                update HU_EMPLOYEE_CV_EDIT
                set IS_APPROVED_PORTAL = 1
                where ID in (@LIST_ID)

                kiểu kiểu như vậy

                => bạn có thể sử dụng "procedure"
            */


            // gọi phương thức ApproveEducationEdit()
            // tham số được truyền vào
            // là danh sách ID
            var response = await _HuEmployeeCvEditRepository.ApproveEducationEdit(listId);



            return Ok(response);
        }



        // chức năng lưu
        [HttpPost]
        public async Task<IActionResult> SaveEducation(HuEmployeeCvEditDTO request)
        {
            var response = await _HuEmployeeCvEditRepository.SaveEducation(request);
            return Ok(response);
        }



        public struct DeleteModel
        {
            public long? IdHuEmployeeCvEdit { get; set; }
        }


        // xóa bản ghi ở portal
        [HttpPost]
        public async Task<IActionResult> DeletePortalById(DeleteModel model)
        {
            var get_record_to_delete = await _uow.Context.Set<HU_EMPLOYEE_CV_EDIT>().FindAsync(model.IdHuEmployeeCvEdit);

            if (get_record_to_delete != null)
            {
                _uow.Context.Set<HU_EMPLOYEE_CV_EDIT>().Remove(get_record_to_delete);
                _uow.Context.SaveChanges();
            }

            return Ok(new FormatedResponse()
            {
                ErrorType = EnumErrorType.NONE,
                MessageCode = CommonMessageCode.DELETE_SUCCESS,
                StatusCode = EnumStatusCode.StatusCode200
            });
        }
    }
}

