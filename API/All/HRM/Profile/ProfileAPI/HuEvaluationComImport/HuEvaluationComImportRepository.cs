using CORE.GenericUOW;
using CORE.DTO;
using API.DTO;
using CORE.Enum;
using CORE.StaticConstant;
using API.All.DbContexts;
using CORE.AutoMapper;
using API.All.SYSTEM.CoreAPI.Xlsx;
using API.Entities;

namespace API.Controllers.HuEvaluate
{
    public class HuEvaluationComImportRepository : IHuEvaluationComImportRepository
    {
        private readonly GenericUnitOfWork _uow;
        private readonly FullDbContext _dbContext;
        private readonly GenericReducer<HU_EVALUATION_COM_IMPORT, HuEvaluationComImportDTO> _genericReducer;
        private IGenericRepository<HU_EVALUATION_COM, HuEvaluationComDTO> _genericRepository;
        private readonly List<string> XLSX_COLUMNS;

        public HuEvaluationComImportRepository(FullDbContext context, GenericUnitOfWork uow)
        {
            _dbContext = context;
            _uow = uow;
            _genericReducer = new();
            _genericRepository = _uow.GenericRepository<HU_EVALUATION_COM, HuEvaluationComDTO>();
            XLSX_COLUMNS = new() { "XLSX_USER_ID", "XLSX_EX_CODE", "XLSX_INSERT_ON", "XLSX_SESSION", "XLSX_FILE_NAME", "XLSX_ROW" };
        }

        public async Task<GenericPhaseTwoListResponse<HuEvaluationComImportDTO>> SinglePhaseQueryList(GenericQueryListDTO<HuEvaluationComImportDTO> request)
        {
            var otherList = _uow.Context.Set<SYS_OTHER_LIST>().AsNoTracking().AsQueryable();
            var employee = _uow.Context.Set<HU_EMPLOYEE>().AsNoTracking().AsQueryable();
            var organization = _uow.Context.Set<HU_ORGANIZATION>().AsNoTracking().AsQueryable();
            var concurrent = _uow.Context.Set<HU_CONCURRENTLY>().AsQueryable();
            var position = _uow.Context.Set<HU_POSITION>().AsNoTracking().AsQueryable();
            var evaluate = _uow.Context.Set<HU_EVALUATE>().AsNoTracking().AsQueryable();
            var classification = _uow.Context.Set<HU_CLASSIFICATION>().AsNoTracking().AsQueryable();
            var employeCv = _uow.Context.Set<HU_EMPLOYEE_CV>().AsNoTracking().AsQueryable();
            var joined = from p in _dbContext.HuEvaluationComImports
                         from cl in otherList.Where(x => x.ID == p.CLASSIFICATION_ID).DefaultIfEmpty()
                         from e in employee.Where(x => x.ID == p.EMPLOYEE_ID).DefaultIfEmpty()
                         from cv in employeCv.Where(x => x.ID == e.PROFILE_ID).DefaultIfEmpty()
                             // JOIN OTHER ENTITIES BASED ON THE BUSINESS
                         select new HuEvaluationComImportDTO
                         {
                             Id = p.XLSX_ROW,
                             // Phần thông tin có trong tất cả các
                             XlsxUserId = p.XLSX_USER_ID,
                             XlsxExCode = p.XLSX_EX_CODE,
                             XlsxSession = p.XLSX_SESSION,
                             XlsxInsertOn = p.XLSX_INSERT_ON,
                             XlsxFileName = p.XLSX_FILE_NAME,
                             XlsxRow = p.XLSX_ROW,
                             YearEvaluationStr = p.YEAR_EVALUATION.ToString(),
                             EmployeeCode = e.CODE,
                             FullName = cv.FULL_NAME,
                             OrgId = e.ORG_ID,
                             LivingCell = cv.LIVING_CELL,
                             MemberPosition = cv.MEMBER_POSITION,
                             EvaluationCategory = cl.NAME,
                             PointEvaluationStr = p.POINT_EVALUATION.ToString(),
                             Note = p.NOTE
                         };

            var singlePhaseResult = await _genericReducer.SinglePhaseReduce(joined, request);
            return singlePhaseResult;
        }

        public async Task<FormatedResponse> Save(ImportQueryListBaseDTO request)
        {
            try
            {
                bool checkData = false;
                bool checkPoint = false;
                bool pathMode = true;
                bool checkIsMember = false;
                var now = DateTime.UtcNow;
                List<HuEvaluationComDTO> listAdd = new();
                var tmp = await _dbContext.HuEvaluationComImports.Where(x => x.XLSX_USER_ID == request.XlsxSid && x.XLSX_EX_CODE == request.XlsxExCode && x.XLSX_SESSION == request.XlsxSession).ToListAsync();

                var tmpType = typeof(HU_EVALUATION_COM_IMPORT);
                var tmpProperties = tmpType.GetProperties()?
                    .Where(x => !XLSX_COLUMNS.Contains(x.Name))
                    .ToList();

                var evaluate = typeof(HU_EVALUATION_COM);
                var evaluateProperties = evaluate.GetProperties().ToList();
                tmp.ForEach(item =>
                {
                    var obj1 = Activator.CreateInstance(typeof(HU_EVALUATION_COM)) ?? throw new Exception(CommonMessageCode.ACTIVATOR_CREATE_INSTANCE_RETURNS_NULL);
                    var e = (HU_EVALUATION_COM)obj1;

                    tmpProperties?.ForEach(tmpProperty =>
                    {
                        var tmpValue = tmpProperty.GetValue(item);
                        var evaluateProperty = evaluateProperties.SingleOrDefault(x => x.Name == tmpProperty.Name);
                        if (evaluateProperty != null)
                        {
                            if (tmpValue != null)
                            {
                                evaluateProperty.SetValue(e, tmpValue);
                            };
                        }
                        else
                        {
                            if (tmpValue != null)
                            {
                                throw new Exception($"{tmpProperty.Name} was not found in HU_EMPLOYEE_CV");
                            }
                        }
                    });
                    var getClassification = (from cl in _uow.Context.Set<HU_CLASSIFICATION>().Where(x => x.CLASSIFICATION_LEVEL == item.CLASSIFICATION_ID)
                                             from sys in _uow.Context.Set<SYS_OTHER_LIST>().Where(x => x.ID == cl.CLASSIFICATION_TYPE && x.CODE == "LXL03")
                                             select new
                                             {
                                                 Id = cl.ID,
                                                 PointFrom = cl.POINT_FROM,
                                                 PointTo = cl.POINT_TO
                                             }).FirstOrDefault();

                    var checkBeforeInsert = (from ev in _uow.Context.Set<HU_EVALUATION_COM>()
                                             where ev.EMPLOYEE_ID == item.EMPLOYEE_ID && ev.YEAR_EVALUATION == item.YEAR_EVALUATION
                                             select new
                                             {
                                                 Id = ev.ID
                                             }).ToList();
                    var checkMember = (from employee in _uow.Context.Set<HU_EMPLOYEE>().Where(x => x.ID == item.EMPLOYEE_ID)
                                       from employeeCv in _uow.Context.Set<HU_EMPLOYEE_CV>().Where(x => x.ID == employee.PROFILE_ID)
                                       where employeeCv.IS_MEMBER == true
                                       select new { Id = employeeCv.ID }).FirstOrDefault();
                    if (checkBeforeInsert.Count > 0)
                    {
                        checkData = true;
                        return;
                    }
                    if (checkIsMember == null)
                    {
                        checkIsMember = true;
                        return;
                    }
                    listAdd.Add(new()
                    {
                        EmployeeId = item.EMPLOYEE_ID,
                        CreatedBy = request.XlsxSid,
                        YearEvaluation = item.YEAR_EVALUATION,
                        PointEvaluation = item.POINT_EVALUATION,
                        ClassificationId = getClassification.Id,
                        Note = item.NOTE
                    });
                });
                if (checkData == true)
                {
                    return new FormatedResponse() { ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400, MessageCode = CommonMessageCode.DUBLICATE_VALUE_EVALUATE_TYPE_YEAR_EMPLOYEE_CONCURRENT_ID };
                }
                if (checkIsMember == true)
                {
                    return new FormatedResponse() { ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400, MessageCode = CommonMessageCode.EMPLOYEE_NOT_IS_MEMBER };
                }
                var insertResponse = await _genericRepository.CreateRange(_uow, listAdd, request.XlsxSid);
                if (insertResponse.InnerBody != null)
                {
                    _dbContext.HuEvaluationComImports.RemoveRange(tmp);
                    _dbContext.SaveChanges();
                }
                return insertResponse;
            }
            catch (Exception ex)
            {
                return new() { ErrorType = EnumErrorType.UNCATCHABLE, StatusCode = EnumStatusCode.StatusCode500, MessageCode = ex.Message };
            }
        }



    }
}

