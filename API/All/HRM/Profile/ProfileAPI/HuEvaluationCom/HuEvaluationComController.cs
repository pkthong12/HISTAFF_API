using API.All.DbContexts;
using API.All.HRM.Profile.ProfileAPI.HuEvaluationCom;
using API.DTO;
using API.Main;
using CORE.DTO;
using CORE.Enum;
using CORE.GenericUOW;
using CORE.StaticConstant;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Information;
using System.Linq.Dynamic.Core;

namespace API.All.HRM.Profile.ProfileAPI.HuEvaluationCom
{
    // cái 567 là 1 số tự chế
    // PROFILE là theo thư mục, theo phân hệ
    // HU_EVALUATION_COM là tên bảng

    [ApiExplorerSettings(GroupName = "567-PROFILE-HU_EVALUATION_COM")]
    
    [ApiController]
    [HiStaffAuthorize]
    [Route("api/[controller]/[action]")]

    public class HuEvaluationComController : ControllerBase
    {
        private readonly GenericUnitOfWork _uow;
        private readonly HuEvaluationComRepository _HuEvaluationComRepository;
        private readonly AppSettings _appSettings;

        public HuEvaluationComController(
            FullDbContext dbContext,
            IOptions<AppSettings> options)
        {
            _uow = new GenericUnitOfWork(dbContext);
            _HuEvaluationComRepository = new HuEvaluationComRepository(dbContext, _uow);
            _appSettings = options.Value;
        }

        [HttpGet]
        public async Task<IActionResult> ReadAll()
        {
            var response = await _HuEvaluationComRepository.ReadAll();
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> ReadAllByNumberKey(ReadAllByKeyLongRequest request)
        {
            var response = await _HuEvaluationComRepository.ReadAllByKey(request.Key, request.Value);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> ReadAllByStringKey(ReadAllByKeyStringRequest request)
        {
            var response = await _HuEvaluationComRepository.ReadAllByKey(request.Key, request.Value);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> QueryList(GenericQueryListDTO<HuEvaluationComDTO> request)
        {
            try
            {
                var response = await _HuEvaluationComRepository.SinglePhaseQueryList(request);

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
            var response = await _HuEvaluationComRepository.GetById(id);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetByStringId(string id)
        {
            var response = await _HuEvaluationComRepository.GetById(id);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] HuEvaluationComDTO model)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();


            // kiểm tra người đang đánh giá
            // có phải Đảng viên không
            // nếu không phải thì bắn ra thông báo
            var profile_id = _uow.Context.Set<HU_EMPLOYEE>().Where(x => x.ID == model.EmployeeId).Select(x => x.PROFILE_ID).FirstOrDefault();

            // lấy ra 1 nhân viên theo "profile_id"
            var record_employee = _uow.Context.Set<HU_EMPLOYEE_CV>().Where(x => x.ID == profile_id).FirstOrDefault();


            // kiểm tra đảng viên
            if (record_employee != null)
            {
                // IS_MEMBER == false
                // là không phải Đảng viên
                if (record_employee.IS_MEMBER == false || record_employee.IS_MEMBER == null)
                {
                    return Ok(new FormatedResponse()
                    {
                        StatusCode = EnumStatusCode.StatusCode404,
                        MessageCode = $"{record_employee.FULL_NAME} không phải Đảng viên"
                    });
                }
            }


            // bảng huClassification theo ngữ cảnh
            var huClassification = _uow.Context.Set<HU_CLASSIFICATION>();


            // truy vấn để lấy ra điểm từ
            // và điểm đến
            var query = await (from item in huClassification.Where(x => x.ID == model.ClassificationId).AsNoTracking()
                               select new
                               {
                                   PointFrom = item.POINT_FROM,
                                   PointTo = item.POINT_TO
                               }
                               ).ToListAsync();

            // lấy ra điểm min, max
            // chắc chắn lúc truy vấn sẽ trả về 1 bản ghi
            // sau đó bản ghi này được ToList()
            // thế thì phải truy cập vào cái index = 0
            // của cái biến "query"
            int min = (int)query[0].PointFrom;
            int max = (int)query[0].PointTo;


            // 1 người
            // thì chỉ có 1 năm đánh giá
            // với cái ID là 999 chẳng hạn
            // thì kiểm tra xem có trùng năm không
            // nếu có thì in ra thông báo lỗi
            var huEvaluationCom = _uow.Context.Set<HU_EVALUATION_COM>();


            // truy vấn danh sách năm đánh giá
            // của 1 người theo id
            // sử dụng phương thức Distinct()
            // để lấy ra các phần tử "năm" khác nhau
            var list_year = (from item in huEvaluationCom.Where(x => x.EMPLOYEE_ID == model.EmployeeId).AsNoTracking()
                             select item.YEAR_EVALUATION).Distinct();


            // kiểm tra năm đánh giá
            foreach ( var item in list_year )
            {
                if (model.YearEvaluation == item)
                {
                    var result = new FormatedResponse()
                    {
                        ErrorType = EnumErrorType.CATCHABLE,
                        StatusCode = EnumStatusCode.StatusCode400,
                        MessageCode = $"Năm đánh giá {model.YearEvaluation} của {model.FullName} đã tồn tại (Bạn không được tạo trùng năm đánh giá)"
                    };

                    return Ok(result);
                }
            }

            // BA yêu cầu kiểm tra ngoài giao diện
            // BA Tuấn log task VNS 584 bỏ thông báo check năm
            // nên tôi cmt code
            // kiểm tra điểm
            //if (model.PointEvaluation > max  || model.PointEvaluation < min)
            //{
            //    var result = new FormatedResponse()
            //    {
            //        ErrorType = EnumErrorType.CATCHABLE,
            //        StatusCode = EnumStatusCode.StatusCode400,
            //        MessageCode = $"Điểm đánh giá\nphải lớn hơn (hoặc bằng) {min} và nhỏ hơn (hoặc bằng) {max}"
            //    };

            //    return Ok(result);
            //}

            var response = await _HuEvaluationComRepository.Create(_uow, model, sid);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> CreateRange(List<HuEvaluationComDTO> models)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _HuEvaluationComRepository.CreateRange(_uow, models, sid);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Update(HuEvaluationComDTO model)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();


            // kiểm tra người đang đánh giá
            // có phải Đảng viên không
            // nếu không phải thì bắn ra thông báo
            var profile_id = _uow.Context.Set<HU_EMPLOYEE>().Where(x => x.ID == model.EmployeeId).Select(x => x.PROFILE_ID).FirstOrDefault();

            // lấy ra 1 nhân viên theo "profile_id"
            var record_employee = _uow.Context.Set<HU_EMPLOYEE_CV>().Where(x => x.ID == profile_id).FirstOrDefault();


            // kiểm tra đảng viên
            if (record_employee != null)
            {
                // IS_MEMBER == false
                // là không phải Đảng viên
                if (record_employee.IS_MEMBER == false || record_employee.IS_MEMBER == null)
                {
                    return Ok(new FormatedResponse()
                    {
                        StatusCode = EnumStatusCode.StatusCode404,
                        MessageCode = $"{record_employee.FULL_NAME} không phải Đảng viên"
                    });
                }
            }


            // bảng huClassification theo ngữ cảnh
            var huClassification = _uow.Context.Set<HU_CLASSIFICATION>();


            // truy vấn để lấy ra điểm từ
            // và điểm đến
            var query = await (from item in huClassification.Where(x => x.ID == model.ClassificationId).AsNoTracking()
                               select new
                               {
                                   PointFrom = item.POINT_FROM,
                                   PointTo = item.POINT_TO
                               }
                               ).ToListAsync();

            // lấy ra điểm min, max
            // chắc chắn lúc truy vấn sẽ trả về 1 bản ghi
            // sau đó bản ghi này được ToList()
            // thế thì phải truy cập vào cái index = 0
            // của cái biến "query"
            int min = (int)query[0].PointFrom;
            int max = (int)query[0].PointTo;


            // 1 người
            // thì chỉ có 1 năm đánh giá
            // với cái ID là 999 chẳng hạn
            // thì kiểm tra xem có trùng năm không
            // nếu có thì in ra thông báo lỗi
            var huEvaluationCom = _uow.Context.Set<HU_EVALUATION_COM>();


            // truy vấn danh sách năm đánh giá
            // của 1 người theo id
            // sử dụng phương thức Distinct()
            // để lấy ra các phần tử "năm" khác nhau
            var list_year_and_id = (from item in huEvaluationCom.Where(x => x.EMPLOYEE_ID == model.EmployeeId).AsNoTracking()
                             select new {
                                 id = item.ID,
                                 year = item.YEAR_EVALUATION
                             }).Distinct();


            // kiểm tra năm đánh giá
            foreach (var item in list_year_and_id)
            {
                if (model.YearEvaluation == item.year && model.Id != item.id)
                {
                    var result = new FormatedResponse()
                    {
                        ErrorType = EnumErrorType.CATCHABLE,
                        StatusCode = EnumStatusCode.StatusCode400,
                        MessageCode = $"Năm đánh giá {model.YearEvaluation} của {model.FullName} đã tồn tại (Năm đánh giá không được trùng)"
                    };

                    return Ok(result);
                }
            }


            // BA yêu cầu kiểm tra ngoài giao diện
            // BA Tuấn log task VNS 584 bỏ thông báo check năm
            // nên tôi cmt code
            //if (model.PointEvaluation > max || model.PointEvaluation < min)
            //{
            //    var result = new FormatedResponse() {
            //        ErrorType = EnumErrorType.CATCHABLE,
            //        StatusCode = EnumStatusCode.StatusCode400,
            //        MessageCode = $"Điểm đánh giá\nphải lớn hơn (hoặc bằng) {min} và nhỏ hơn (hoặc bằng) {max}"
            //    };

            //    return Ok(result);
            //}

            var response = await _HuEvaluationComRepository.Update(_uow, model, sid);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateRange(List<HuEvaluationComDTO> models)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _HuEvaluationComRepository.UpdateRange(_uow, models, sid);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(HuEvaluationComDTO model)
        {
            if (model.Id != null)
            {
                var response = await _HuEvaluationComRepository.Delete(_uow, (long)model.Id);
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
            var response = await _HuEvaluationComRepository.DeleteIds(_uow, model.Ids);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteStringIds(StringIdsRequest model)
        {
            var response = await _HuEvaluationComRepository.DeleteIds(_uow, model.Ids);
            return Ok(response);
        }



        // thêm cái dòng code này
        // để lấy ra xếp loại đánh giá
        // cái hàm này
        // giúp load dữ liệu cho Drop Down List
        [HttpGet]
        public async Task<IActionResult> LayXepLoaiDanhGia(string? code)
        {
            // lúc gọi hàm này
            // thì sẽ truyền vào code = "LXL03" (của bảng SYS_OTHER_LIST)
            // vì "LXL03" là Đảng

            var huClassification = _uow.Context.Set<HU_CLASSIFICATION>();
            var sysOtherList = _uow.Context.Set<SYS_OTHER_LIST>();

            var r = await (from p in huClassification.AsNoTracking()
                           from tham_chieu_1 in sysOtherList.Where(x => x.ID == p.CLASSIFICATION_TYPE && x.CODE == code)
                           from tham_chieu_2 in sysOtherList.Where(x => x.ID == p.CLASSIFICATION_LEVEL)
                           select new
                           {
                               Id = p.ID,
                               Name = tham_chieu_2.NAME,
                               PointFrom = p.POINT_FROM,
                               PointTo = p.POINT_TO

                               // Code = p.CODE,
                               // IndexId = "Index" + p.ID
                           }).ToListAsync();

            /* CÂU LỆNH TRÊN TƯƠNG ĐƯƠNG VỚI

            select  b1.ID,
		            b2.[NAME] as N'Tên',
		            b2.CODE,
		            b3.name
            from    HU_CLASSIFICATION b1, SYS_OTHER_LIST b2, SYS_OTHER_LIST b3
            where   b1.CLASSIFICATION_TYPE = b2.ID
                    and b1.CLASSIFICATION_LEVEL = b3.ID
                    and b2.code = 'LXL03'
            */

            return Ok(new FormatedResponse() { InnerBody = r });
        }



        // cài hàm này
        // có tác dụng lấy dữ liệu cho cái ô xếp loại đánh giá
        // nó lấy theo id
        // bình thường thì HU_EVALUATION_COM_READ là đủ
        // nhưng nó cái "xếp loại đánh giá"
        // nó lại tách thành 1 component riêng
        // nên lại phải lấy dữ liệu riêng cho nó
        // sử dụng cách thức lấy dữ liệu như 1 bản ghi
        // đúng là lấy dữ liệu cho 1 bản ghi
        // nhưng chỉ lấy 1 giá trị trong cái bản ghi
        // cắt đúng 1 phần trong cái bản ghi đấy
        // xong rồi ném cái dữ liệu đấy vào "xếp loại đánh giá" trên giao diện
        [HttpGet]
        public async Task<IActionResult> GetXepLoaiById(long id)
        {
            var huClassification = _uow.Context.Set<HU_CLASSIFICATION>();
            var sysOtherList = _uow.Context.Set<SYS_OTHER_LIST>();

            var r = await (from p in huClassification.AsNoTracking()
                           from tham_chieu_1 in sysOtherList.Where(x => x.ID == p.CLASSIFICATION_LEVEL)
                           from tham_chieu_2 in sysOtherList.Where(x => x.ID == p.CLASSIFICATION_TYPE && x.CODE == "LXL03")
                           select new
                           {
                               Id = p.ID,
                               Name = tham_chieu_1.NAME
                           }).ToListAsync();


            return Ok(new FormatedResponse() { InnerBody = r.FirstOrDefault(x => x.Id == id) });
        }
    }
}