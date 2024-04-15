using CORE.GenericUOW;
using CORE.DTO;
using API.DTO;
using CORE.Enum;
using CORE.StaticConstant;
using API.All.DbContexts;
using CORE.AutoMapper;
using System;
using API.Entities;
using Microsoft.AspNetCore.Http.HttpResults;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Logical;
using API.All.HRM.Payroll.PayrollAPI.PaListSal;

namespace API.All.HRM.Profile.ProfileAPI.HuEvaluationCom
{
    public class HuEvaluationComRepository : IHuEvaluationComRepository
    {
        private readonly GenericUnitOfWork _uow;
        private readonly FullDbContext _dbContext;
        private IGenericRepository<HU_EVALUATION_COM, HuEvaluationComDTO> _genericRepository;
        private readonly GenericReducer<HU_EVALUATION_COM, HuEvaluationComDTO> _genericReducer;

        public HuEvaluationComRepository(FullDbContext context, GenericUnitOfWork uow)
        {
            _dbContext = context;
            _uow = uow;
            _genericRepository = _uow.GenericRepository<HU_EVALUATION_COM, HuEvaluationComDTO>();
            _genericReducer = new();
        }

        public async Task<GenericPhaseTwoListResponse<HuEvaluationComDTO>> SinglePhaseQueryList(GenericQueryListDTO<HuEvaluationComDTO> request)
        {
            var joined = (from p in _dbContext.HuEvaluationComs.AsNoTracking()  // chỗ HuEvaluationComs này phải where là đảng viên đúng không
                          from tham_chieu_1 in _dbContext.HuEmployees.Where(x => x.ID == p.EMPLOYEE_ID).DefaultIfEmpty()
                          from tham_chieu_2 in _dbContext.HuEmployeeCvs.Where(x => x.ID == tham_chieu_1.PROFILE_ID).DefaultIfEmpty()
                          from t in _dbContext.HuPositions.Where(x => x.ID == tham_chieu_1.POSITION_ID)
                          from j in _dbContext.HuJobs.Where(x => x.ID == t.JOB_ID).DefaultIfEmpty()

                          // lấy ra xếp loại đánh giá
                          // bắt buộc phải dùng tham_chieu_3 và tham_chieu_4
                          from tham_chieu_3 in _dbContext.HuClassifications.Where(x => x.ID == p.CLASSIFICATION_ID).DefaultIfEmpty()
                          from tham_chieu_4 in _dbContext.SysOtherLists.Where(x => x.ID == tham_chieu_3.CLASSIFICATION_LEVEL).DefaultIfEmpty()
                          from tham_chieu_5 in _dbContext.SysOtherLists.Where(x => x.ID == tham_chieu_3.CLASSIFICATION_TYPE && x.CODE == "LXL03").DefaultIfEmpty()
                          select new HuEvaluationComDTO
                          {
                              // trường 1:
                              // ID được kế thừa
                              Id = p.ID,

                              //  trường đặc biệt:
                              //  đó là EMPLOYEE_ID
                              EmployeeId = p.EMPLOYEE_ID,

                              // trường 2:
                              // mã nhân viên
                              // dùng EMPLOYEE_ID để tham chiếu đến HU_EMPLOYEE
                              // rồi lấy ra mã nhân viên
                              EmployeeCode = tham_chieu_1.CODE,

                              // trường 3:
                              // họ và tên
                              // dùng EMPLOYEE_CV_ID để tham chiếu đến HU_EMPLOYEE_CV
                              // rồi lấy ra họ và tên
                              FullName = tham_chieu_2.FULL_NAME,

                              // trường 4:
                              // năm đánh giá
                              YearEvaluation = p.YEAR_EVALUATION,
                              YearEvaluationStr = p.YEAR_EVALUATION.ToString(),

                              // trường đặc biệt
                              // đó là EMPLOYEE_CV_ID
                              // trường này sai cách join bảng nên cmt lại
                              // EmployeeCvId = p.EMPLOYEE_CV_ID,

                              // trường 5:
                              // chức vụ đảng
                              // lấy từ bảng HU_EMPLOYEE_CV
                              MemberPosition = tham_chieu_2.MEMBER_POSITION,

                              // trường 6:
                              // chi bộ sinh hoạt
                              // hiển thị theo nhân viên
                              // Thắng làm chi bộ sinh hoạt là LIVING_CELL
                              LivingCell = tham_chieu_2.LIVING_CELL,

                              // trường đặc biệt
                              // dùng để hiển thị ra xếp loại đánh giá
                              ClassificationId = p.CLASSIFICATION_ID,

                              // trường 7:
                              // xếp loại đánh giá
                              // lấy từ màn hình danh mục xếp loại đánh giá (HU_CLASSIFICATION)
                              EvaluationCategory = tham_chieu_4.NAME,

                              // trường 8:
                              // điểm đánh giá
                              // nhập số
                              PointEvaluation = p.POINT_EVALUATION,
                              PointEvaluationStr = p.POINT_EVALUATION.ToString(),

                              // trường 9:
                              // ghi chú
                              Note = p.NOTE,

                              // trường 10:
                              // ngày tạo
                              CreatedDate = p.CREATED_DATE,

                              // trường 11:
                              // người tạo (được tạo bởi)
                              CreatedBy = p.CREATED_BY,

                              // trường 12:
                              // nhật ký tạo (log tạo)
                              CreatedLog = p.CREATED_LOG,

                              // trường 13:
                              // ngày cập nhật
                              UpdatedDate = p.UPDATED_DATE,

                              // trường 14:
                              // người cập nhật (được cập nhật bởi)
                              UpdatedBy = p.UPDATED_BY,

                              // trường 15:
                              // nhật ký cập nhật (log cập nhật)
                              UpdatedLog = p.UPDATED_LOG,

                              OrgId = tham_chieu_1.ORG_ID,

                              IsMember = tham_chieu_2.IS_MEMBER,

                              WorkStatusId = tham_chieu_1.WORK_STATUS_ID,
                              JobOrderNum = (int)(j.ORDERNUM ?? 99)
                          }).Where(x => x.IsMember == true).OrderByDescending(x => x.CreatedDate)
                         ;

            var singlePhaseResult = await _genericReducer.SinglePhaseReduce(joined, request);
            return singlePhaseResult;
        }

        public async Task<FormatedResponse> ReadAll()
        {
            var response = await _genericRepository.ReadAll();
            return response;
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
            try
            {
                var res = await _genericRepository.GetById(id);
                if (res.InnerBody != null)
                {
                    var response = res.InnerBody;
                    var list = new List<HU_EVALUATION_COM>
                    {
                        (HU_EVALUATION_COM)response
                    };
                    var joined = (from p in list
                                  select new HuEvaluationComDTO
                                  {
                                      // trường 1:
                                      // ID được kế thừa
                                      Id = p.ID,

                                      //  trường đặc biệt:
                                      //  đó là EMPLOYEE_ID
                                      EmployeeId = p.EMPLOYEE_ID,

                                      // trường 2:
                                      // mã nhân viên
                                      // dùng EMPLOYEE_ID để tham chiếu đến HU_EMPLOYEE
                                      // rồi lấy ra mã nhân viên
                                      // EmployeeCode = tham_chieu_1.CODE,

                                      // trường 3:
                                      // họ và tên
                                      // dùng EMPLOYEE_CV_ID để tham chiếu đến HU_EMPLOYEE_CV
                                      // rồi lấy ra họ và tên
                                      // FullName = tham_chieu_2.FULL_NAME,

                                      // trường 4:
                                      // năm đánh giá
                                      YearEvaluation = p.YEAR_EVALUATION,

                                      // trường đặc biệt
                                      // đó là EMPLOYEE_CV_ID
                                      // trường này sai cách join bảng nên cmt lại
                                      // EmployeeCvId = p.EMPLOYEE_CV_ID,

                                      // trường 5:
                                      // chức vụ đảng
                                      // cái cột này Thắng đang tạo, nhưng chưa tạo đến
                                      // sau này nó là nvarchar(nhắn tin với Tiến Business Analyst)
                                      // lấy từ bảng HU_EMPLOYEE_CV
                                      // PositionCommunist = tham_chieu_2.POSITION_COMMUNIST,

                                      // trường 6:
                                      // chi bộ sinh hoạt
                                      // hiển thị theo nhân viên
                                      // dùng EMPLOYEE_CV_ID để tham chiếu đến EMPLOYEE_CV
                                      // LivingArea là khu vực sống, khu vực sinh hoạt
                                      // LivingArea = tham_chieu_2.LIVING_AREA,

                                      // trường đặc biệt
                                      // dùng để hiển thị ra xếp loại đánh giá
                                      // ClassificationId = tham_chieu_3.ID,

                                      // trường 7:
                                      // xếp loại đánh giá
                                      // danh mục xếp loại đánh giá(HU_CLASSIFICATION)
                                      // EvaluationCategory = tham_chieu_3.NAME,

                                      // trường 8:
                                      // điểm đánh giá
                                      // nhập số
                                      PointEvaluation = p.POINT_EVALUATION,

                                      // trường 9:
                                      // ghi chú
                                      Note = p.NOTE,

                                      // trường 10:
                                      // ngày tạo
                                      CreatedDate = p.CREATED_DATE,

                                      // trường 11:
                                      // người tạo (được tạo bởi)
                                      CreatedBy = p.CREATED_BY,

                                      // trường 12:
                                      // nhật ký tạo (log tạo)
                                      CreatedLog = p.CREATED_LOG,

                                      // trường 13:
                                      // ngày cập nhật
                                      UpdatedDate = p.UPDATED_DATE,

                                      // trường 14:
                                      // người cập nhật (được cập nhật bởi)
                                      UpdatedBy = p.UPDATED_BY,

                                      // trường 15:
                                      // nhật ký cập nhật (log cập nhật)
                                      UpdatedLog = p.UPDATED_LOG,

                                      // lấy id cho cái child bên dưới
                                      ClassificationId = p.CLASSIFICATION_ID
                                  }).FirstOrDefault();

                    if (joined != null)
                    {
                        if (res.StatusCode == EnumStatusCode.StatusCode200)
                        {
                            // đây là chỗ truy vấn
                            // để lấy ra được tên tương ứng với id
                            var child = (from tham_chieu_1 in _dbContext.HuEmployees.Where(x => x.ID == joined.EmployeeId).DefaultIfEmpty()
                                         from tham_chieu_2 in _dbContext.HuEmployeeCvs.Where(x => x.ID == tham_chieu_1.PROFILE_ID).DefaultIfEmpty()

                                         // lấy ra xếp loại đánh giá
                                         // bắt buộc phải dùng tham_chieu_3 và tham_chieu_4
                                         from tham_chieu_3 in _dbContext.HuClassifications.Where(x => x.ID == joined.ClassificationId).DefaultIfEmpty()
                                         from tham_chieu_4 in _dbContext.SysOtherLists.Where(x => x.ID == tham_chieu_3.CLASSIFICATION_LEVEL).DefaultIfEmpty()
                                         from tham_chieu_5 in _dbContext.SysOtherLists.Where(x => x.ID == tham_chieu_3.CLASSIFICATION_TYPE && x.CODE == "LXL03").DefaultIfEmpty()
                                         select new
                                         {
                                             EmployeeCode = tham_chieu_1.CODE,
                                             FullName = tham_chieu_2.FULL_NAME,
                                             // vẫn còn mấy cái nữa phải viết
                                             // sau này người ta làm xong thì code tiếp

                                             // chức vụ đảng
                                             MemberPosition = tham_chieu_2.MEMBER_POSITION,

                                             // chi bộ sinh hoạt
                                             LivingCell  = tham_chieu_2.LIVING_CELL,

                                             // xếp loại đánh giá
                                             EvaluationCategory = tham_chieu_4.NAME,

                                             // Drop Down List
                                             // xếp loại đánh giá
                                             Name = tham_chieu_4.NAME,

                                             // lấy ra điểm từ
                                             PointFrom = tham_chieu_3.POINT_FROM,

                                             // lấy ra điểm đến
                                             PointTo = tham_chieu_3.POINT_TO
                                         }).FirstOrDefault();
                            joined.EmployeeCode = child?.EmployeeCode;
                            joined.FullName = child?.FullName;
                            joined.MemberPosition = child?.MemberPosition;
                            joined.LivingCell = child?.LivingCell;
                            joined.EvaluationCategory = child?.EvaluationCategory;
                            joined.Name = child?.Name;
                            joined.PointFrom = child?.PointFrom;
                            joined.PointTo = child?.PointTo;
                            return new FormatedResponse() { InnerBody = joined };
                        }
                        else
                        {
                            return new() { ErrorType = EnumErrorType.CATCHABLE, MessageCode = CommonMessageCode.GET_LIST_BY_KEY_RESPOSNE_IS_NOT_OF_STATUS_CODE_200, StatusCode = EnumStatusCode.StatusCode400 };
                        }
                    }
                    else
                    {
                        return new() { ErrorType = EnumErrorType.CATCHABLE, MessageCode = CommonMessageCode.JOINED_QUERY_AFTER_GET_BY_ID_WAS_NULL, StatusCode = EnumStatusCode.StatusCode400 };
                    }
                }
                else
                {
                    return new FormatedResponse() { MessageCode = CommonMessageCode.ENTITY_NOT_FOUND, ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400 };
                }

            }
            catch (Exception ex)
            {
                return new FormatedResponse() { MessageCode = ex.Message, ErrorType = EnumErrorType.UNCATCHABLE, StatusCode = EnumStatusCode.StatusCode500 };

            }
        }
        public async Task<FormatedResponse> GetById(string id)
        {
            await Task.Run(() => null);
            throw new NotImplementedException();
        }

        public async Task<FormatedResponse> Create(GenericUnitOfWork _uow, HuEvaluationComDTO dto, string sid)
        {
            // dto.IsActive = true;
            var response = await _genericRepository.Create(_uow, dto, sid);
            return response;
        }

        public async Task<FormatedResponse> CreateRange(GenericUnitOfWork _uow, List<HuEvaluationComDTO> dtos, string sid)
        {
            var add = new List<HuEvaluationComDTO>();
            add.AddRange(dtos);
            var response = await _genericRepository.CreateRange(_uow, add, sid);
            return response;
        }

        public async Task<FormatedResponse> Update(GenericUnitOfWork _uow, HuEvaluationComDTO dto, string sid, bool patchMode = true)
        {
            var response = await _genericRepository.Update(_uow, dto, sid, patchMode);
            return response;
        }

        public async Task<FormatedResponse> UpdateRange(GenericUnitOfWork _uow, List<HuEvaluationComDTO> dtos, string sid, bool patchMode = true)
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

        public virtual async Task<FormatedResponse> ToggleActiveIds(GenericUnitOfWork _uow, List<long> ids, bool valueToBind, string sid)
        {
            await Task.Run(() => null);
            throw new NotImplementedException();
        }

    }
}