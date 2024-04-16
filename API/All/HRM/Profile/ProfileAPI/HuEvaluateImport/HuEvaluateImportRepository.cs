using CORE.GenericUOW;
using CORE.DTO;
using API.DTO;
using CORE.Enum;
using CORE.StaticConstant;
using API.All.DbContexts;
using CORE.AutoMapper;
using API.All.SYSTEM.CoreAPI.Xlsx;

namespace API.Controllers.HuEvaluate
{
    public class HuEvaluateImportRepository : IHuEvaluateImportRepository
    {
        private readonly GenericUnitOfWork _uow;
        private readonly FullDbContext _dbContext;
        private readonly GenericReducer<HU_EVALUATE_IMPORT, HuEvaluateImportDTO> _genericReducer;
        private readonly GenericReducer<HU_EVALUATE_CONCURRENT_IMPORT, HuEvaluateConcurrentDTO> _genericReducerConcurrent;
        private IGenericRepository<HU_EVALUATE, HuEvaluateDTO> _genericRepository;
        private readonly List<string> XLSX_COLUMNS;

        public HuEvaluateImportRepository(FullDbContext context, GenericUnitOfWork uow)
        {
            _dbContext = context;
            _uow = uow;
            _genericReducer = new();
            _genericReducerConcurrent = new();
            _genericRepository = _uow.GenericRepository<HU_EVALUATE, HuEvaluateDTO>();
            XLSX_COLUMNS = new() { "XLSX_USER_ID", "XLSX_EX_CODE", "XLSX_INSERT_ON", "XLSX_SESSION", "XLSX_FILE_NAME", "XLSX_ROW" };
        }

        public async Task<GenericPhaseTwoListResponse<HuEvaluateImportDTO>> SinglePhaseQueryList(GenericQueryListDTO<HuEvaluateImportDTO> request)
        {
            var otherList = _uow.Context.Set<SYS_OTHER_LIST>().AsNoTracking().AsQueryable();
            var employee = _uow.Context.Set<HU_EMPLOYEE>().AsNoTracking().AsQueryable();
            var organization = _uow.Context.Set<HU_ORGANIZATION>().AsNoTracking().AsQueryable();
            var concurrent = _uow.Context.Set<HU_CONCURRENTLY>().AsNoTracking().AsQueryable();
            var position = _uow.Context.Set<HU_POSITION>().AsNoTracking().AsQueryable();
            var evaluate = _uow.Context.Set<HU_EVALUATE>().AsNoTracking().AsQueryable();
            var classification = _uow.Context.Set<HU_CLASSIFICATION>().AsNoTracking().AsQueryable();
            var joined = from p in _dbContext.HuEvaluateImports
                         from et in otherList.Where(x => x.ID == p.EVALUATE_TYPE).DefaultIfEmpty()
                         from cl in otherList.Where(x => x.ID == p.CLASSIFICATION_ID).DefaultIfEmpty()
                         from e in employee.Where(x => x.PROFILE_ID == p.PROFILE_ID).DefaultIfEmpty()
                         from c in concurrent.Where(x => x.ID == p.EMPLOYEE_CONCURRENT_ID).DefaultIfEmpty()
                         from ec in employee.Where(x => x.ID == c.EMPLOYEE_ID).DefaultIfEmpty()
                         from o in organization.Where(x => x.ID == p.ORG_ID).DefaultIfEmpty()
                         from po in position.Where(x => x.ID == p.POSITION_ID).DefaultIfEmpty()
                         from se in otherList.Where(x => x.ID == e.WORK_STATUS_ID).DefaultIfEmpty()
                         from pco in position.Where(x => x.ID == c.POSITION_ID).DefaultIfEmpty()
                         from oc in organization.Where(x => x.ID == pco.ORG_ID).DefaultIfEmpty()
                             // JOIN OTHER ENTITIES BASED ON THE BUSINESS
                         select new HuEvaluateImportDTO
                         {
                             Id = p.XLSX_ROW,
                             // Phần thông tin có trong tất cả các
                             XlsxUserId = p.XLSX_USER_ID,
                             XlsxExCode = p.XLSX_EX_CODE,
                             XlsxSession = p.XLSX_SESSION,
                             XlsxInsertOn = p.XLSX_INSERT_ON,
                             XlsxFileName = p.XLSX_FILE_NAME,
                             XlsxRow = p.XLSX_ROW,

                             EvaluateType = p.EVALUATE_TYPE,
                             EvaluateName = et.NAME,
                             ClassificationId = p.CLASSIFICATION_ID,
                             ClassificationName = cl.NAME,
                             EmployeeId = p.EMPLOYEE_ID,
                             EmployeeConcurrentId = p.EMPLOYEE_CONCURRENT_ID,
                             EmployeeCode = e.CODE != null ? e.CODE : ec.CODE,
                             EmployeeName = e.Profile.FULL_NAME != "" ? e.Profile.FULL_NAME : ec.Profile.FULL_NAME,
                             OrgId = p.ORG_ID != null ? p.ORG_ID : p.ORG_CONCURRENT_ID,
                             OrgName = o.NAME != "" ? o.NAME : oc.NAME,
                             PositionId = p.ID,
                             PositionName = po.NAME != "" ? po.NAME : pco.NAME,
                             Year = p.YEAR,
                             YearSearch = p.YEAR.ToString(),
                             PointSearch = p.POINT.ToString(),
                             Point = p.POINT,
                             Note = p.NOTE,
                             WorkStatusId = e.WORK_STATUS_ID,
                             WorkStatusName = se.NAME,
                             EmployeeStatus = e.WORK_STATUS_ID,
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
                var now = DateTime.UtcNow;
                List<HuEvaluateDTO> listAdd = new();
                var tmp = await _dbContext.HuEvaluateImports.Where(x => x.XLSX_USER_ID == request.XlsxSid && x.XLSX_EX_CODE == request.XlsxExCode && x.XLSX_SESSION == request.XlsxSession).ToListAsync();

                var tmpType = typeof(HU_EVALUATE_IMPORT);
                var tmpProperties = tmpType.GetProperties()?
                    .Where(x => !XLSX_COLUMNS.Contains(x.Name))
                    .ToList();

                var evaluate = typeof(HU_EVALUATE);
                var evaluateProperties = evaluate.GetProperties().ToList();
                tmp.ForEach(item =>
                {
                    var obj1 = Activator.CreateInstance(typeof(HU_EVALUATE)) ?? throw new Exception(CommonMessageCode.ACTIVATOR_CREATE_INSTANCE_RETURNS_NULL);
                    var e = (HU_EVALUATE)obj1;

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
                    var getCv = (from cv in _uow.Context.Set<HU_EMPLOYEE_CV>()
                                 from ee in _uow.Context.Set<HU_EMPLOYEE>().Where(x => x.PROFILE_ID == cv.ID)
                                 from p in _uow.Context.Set<HU_POSITION>().Where(x => x.ID == ee.POSITION_ID)
                                 from o in _uow.Context.Set<HU_ORGANIZATION>().Where(x => x.ID == ee.ORG_ID)
                                 where cv.ID == item.PROFILE_ID
                                 select new
                                 {
                                     Id = cv.ID,
                                     EmployeeId = ee.ID,
                                     EmployeeCode = ee.CODE,
                                     EmployeeName = cv.FULL_NAME,
                                     PositionId = ee.POSITION_ID,
                                     PositionName = p.NAME,
                                     OrgId = ee.ORG_ID,
                                     OrgName = o.NAME
                                 }).FirstOrDefault();
                    var getClassification = _uow.Context.Set<HU_CLASSIFICATION>().Where(x => x.CLASSIFICATION_LEVEL == item.CLASSIFICATION_ID && x.CLASSIFICATION_TYPE == item.EVALUATE_TYPE).FirstOrDefault();
                    if (item.POINT > getClassification.POINT_FROM && item.POINT < getClassification.POINT_TO)
                    {
                        var checkBeforeInsert = (from ev in _uow.Context.Set<HU_EVALUATE>()
                                                 where ev.EVALUATE_TYPE == item.EVALUATE_TYPE && ev.EMPLOYEE_ID == item.EMPLOYEE_ID && ev.YEAR == item.YEAR
                                                 select new
                                                 {
                                                     Id = ev.ID
                                                 }).ToList();
                        if (checkBeforeInsert.Count > 0)
                        {
                            checkData = true;
                            return;
                        }
                        listAdd.Add(new()
                        {
                            EmployeeId = getCv.EmployeeId,
                            EmployeeCode = getCv.EmployeeCode,
                            EmployeeName = getCv.EmployeeName,
                            CreatedBy = request.XlsxSid,
                            EvaluateType = item.EVALUATE_TYPE,
                            Year = item.YEAR,
                            Point = item.POINT,
                            OrgId = getCv.OrgId,
                            OrgName = getCv.OrgName,
                            PositionId = getCv.PositionId,
                            PositionName = getCv.PositionName,
                            ClassificationId = getClassification.ID,
                            Note = item.NOTE
                        });

                    }
                    else
                    {
                        checkPoint = true;
                        return;
                    }

                });
                if (checkData == true)
                {
                    return new FormatedResponse() { ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400, MessageCode = CommonMessageCode.DUBLICATE_VALUE_EVALUATE_TYPE_YEAR_EMPLOYEE_CONCURRENT_ID };
                }
                if (checkPoint == true)
                {
                    return new FormatedResponse() { ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400, MessageCode = CommonMessageCode.CREATE_OBJECT_NUMBER_IS_NOT_ALLOWED };
                }
                var insertResponse = await _genericRepository.CreateRange(_uow, listAdd, request.XlsxSid);
                if (insertResponse.InnerBody != null)
                {
                    _dbContext.HuEvaluateImports.RemoveRange(tmp);
                    _dbContext.SaveChanges();
                }
                return insertResponse;
            }
            catch (Exception ex)
            {
                return new() { ErrorType = EnumErrorType.UNCATCHABLE, StatusCode = EnumStatusCode.StatusCode500, MessageCode = ex.Message };
            }
        }

        public async Task<GenericPhaseTwoListResponse<HuEvaluateConcurrentDTO>> EvaluateConcurrentQueryList(GenericQueryListDTO<HuEvaluateConcurrentDTO> request)
        {
            var otherList = _uow.Context.Set<SYS_OTHER_LIST>().AsNoTracking().AsQueryable();
            var employee = _uow.Context.Set<HU_EMPLOYEE>().AsNoTracking().AsQueryable();
            var employeCv = _uow.Context.Set<HU_EMPLOYEE_CV>().AsNoTracking().AsQueryable();
            var organization = _uow.Context.Set<HU_ORGANIZATION>().AsNoTracking().AsQueryable();
            var concurrent = _uow.Context.Set<HU_CONCURRENTLY>().AsNoTracking().AsQueryable();
            var position = _uow.Context.Set<HU_POSITION>().AsNoTracking().AsQueryable();
            var classification = _uow.Context.Set<HU_CLASSIFICATION>().AsNoTracking().AsQueryable();
            var joined = from p in _dbContext.HuEvaluateConcurrentImports
                            from con in concurrent.Where(x => x.ID == p.EMPLOYEE_CONCURRENT_ID).DefaultIfEmpty()
                            from ee in employee.Where(x => x.ID == con.EMPLOYEE_ID).DefaultIfEmpty()
                            from cv in employeCv.Where(x => x.ID == ee.PROFILE_ID).DefaultIfEmpty()
                            from po in position.Where(x => x.ID == p.POSITION_CONCURRENT_ID).DefaultIfEmpty()
                            from org in organization.Where(x => x.ID == p.ORG_CONCURRENT_ID).DefaultIfEmpty()
                            from ot in otherList.Where(x => x.ID == p.CLASSIFICATION_ID).DefaultIfEmpty()
                                // JOIN OTHER ENTITIES BASED ON THE BUSINESS
                            select new HuEvaluateConcurrentDTO
                            {
                                Id = p.XLSX_ROW,
                                // Phần thông tin có trong tất cả các
                                XlsxUserId = p.XLSX_USER_ID,
                                XlsxExCode = p.XLSX_EX_CODE,
                                XlsxSession = p.XLSX_SESSION,
                                XlsxInsertOn = p.XLSX_INSERT_ON,
                                XlsxFileName = p.XLSX_FILE_NAME,
                                XlsxRow = p.XLSX_ROW,
                                YearSearch = p.YEAR.ToString(),
                                EmployeeCode = ee.CODE,
                                EmployeeName = cv.FULL_NAME,
                                PositionConcurrentName = po.NAME,
                                OrgConcurrentName = org.NAME,
                                ClassificationName = ot.NAME,
                                PointSearch = p.POINT.ToString(),
                                Note = p.NOTE

                            };

            var singlePhaseResult = await _genericReducerConcurrent.SinglePhaseReduce(joined, request);
            return singlePhaseResult;
        }

        public async Task<FormatedResponse> EvaluateConcurrentSave(ImportQueryListBaseDTO request)
        {
            try
            {
                bool checkData = false;
                bool checkPoint = false;
                bool pathMode = true;
                var now = DateTime.UtcNow;
                List<HuEvaluateDTO> listAdd = new();
                var tmp = await _dbContext.HuEvaluateConcurrentImports.Where(x => x.XLSX_USER_ID == request.XlsxSid && x.XLSX_EX_CODE == request.XlsxExCode && x.XLSX_SESSION == request.XlsxSession).ToListAsync();

                var tmpType = typeof(HU_EVALUATE_CONCURRENT_IMPORT);
                var tmpProperties = tmpType.GetProperties()?
                    .Where(x => !XLSX_COLUMNS.Contains(x.Name))
                    .ToList();

                var evaluate = typeof(HU_EVALUATE);
                var evaluateProperties = evaluate.GetProperties().ToList();
                tmp.ForEach(item =>
                {
                    var obj1 = Activator.CreateInstance(typeof(HU_EVALUATE)) ?? throw new Exception(CommonMessageCode.ACTIVATOR_CREATE_INSTANCE_RETURNS_NULL);
                    var e = (HU_EVALUATE)obj1;

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
                    var getConcurrent = (from con in _uow.Context.Set<HU_CONCURRENTLY>().Where(x => x.ID == item.EMPLOYEE_CONCURRENT_ID)
                                         from ee in _uow.Context.Set<HU_EMPLOYEE>().Where(x => x.ID == con.EMPLOYEE_ID).DefaultIfEmpty()
                                         from cv in _uow.Context.Set<HU_EMPLOYEE_CV>().Where(x => x.ID == ee.PROFILE_ID).DefaultIfEmpty()
                                         from po in _uow.Context.Set<HU_POSITION>().Where(x => x.ID == item.POSITION_CONCURRENT_ID).DefaultIfEmpty()
                                         from org in _uow.Context.Set<HU_ORGANIZATION>().Where(x => x.ID == item.ORG_CONCURRENT_ID).DefaultIfEmpty()
                                 where cv.ID == item.PROFILE_ID
                                 select new
                                 {
                                     Id = cv.ID,
                                     ConCurrentID = con.ID,
                                     EmployeeId = ee.ID,
                                     EmployeeCode = ee.CODE,
                                     EmployeeName = cv.FULL_NAME,
                                     PositionId = po.ID,
                                     PositionName = po.NAME,
                                     OrgId = org.ID,
                                     OrgName = org.NAME
                                 }).FirstOrDefault();
                    var getClassification = (from cl in _uow.Context.Set<HU_CLASSIFICATION>().Where(x => x.CLASSIFICATION_LEVEL == item.CLASSIFICATION_ID)
                                             from ot in _uow.Context.Set<SYS_OTHER_LIST>().Where(x => x.ID == cl.CLASSIFICATION_TYPE)
                                             where ot.CODE == "LXL02"
                                             select new
                                             {
                                                 Id = cl.ID,
                                                 EvaluateType = cl.CLASSIFICATION_TYPE,
                                                 PointFrom = cl.POINT_FROM,
                                                 PointTo = cl.POINT_TO,
                                             }).FirstOrDefault();
                    if (item.POINT > getClassification.PointFrom && item.POINT < getClassification.PointTo)
                    {
                        var checkBeforeInsert = (from ev in _uow.Context.Set<HU_EVALUATE>()
                                                 where ev.EVALUATE_TYPE == item.EVALUATE_TYPE && ev.EMPLOYEE_CONCURRENT_ID == item.EMPLOYEE_CONCURRENT_ID && ev.YEAR == item.YEAR
                                                 select new
                                                 {
                                                     Id = ev.ID
                                                 }).ToList();
                        if (checkBeforeInsert.Count > 0)
                        {
                            checkData = true;
                            return;
                        }
                        listAdd.Add(new()
                        {
                            
                            EmployeeConcurrentId = getConcurrent.ConCurrentID,
                            EmployeeConcurrentName = getConcurrent.EmployeeName,
                            EmployeeName = "",
                            CreatedBy = request.XlsxSid,
                            EvaluateType = getClassification.EvaluateType,
                            Year = item.YEAR,
                            Point = item.POINT,
                            OrgConcurrentId = getConcurrent.OrgId,
                            OrgConcurrentName = getConcurrent.OrgName,
                            OrgName = "",
                            PositionConcurrentId = getConcurrent.PositionId,
                            PositionName = "",
                            PositionConcurrentName = getConcurrent.PositionName,
                            ClassificationId = getClassification.Id,
                            Note = item.NOTE
                        });

                    }
                    else
                    {
                        checkPoint = true;
                        return;
                    }

                });
                if (checkData == true)
                {
                    return new FormatedResponse() { ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400, MessageCode = CommonMessageCode.DUBLICATE_VALUE_EVALUATE_TYPE_YEAR_EMPLOYEE_CONCURRENT_ID };
                }
                if (checkPoint == true)
                {
                    return new FormatedResponse() { ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400, MessageCode = CommonMessageCode.CREATE_OBJECT_NUMBER_IS_NOT_ALLOWED };
                }
                var insertResponse = await _genericRepository.CreateRange(_uow, listAdd, request.XlsxSid);
                if (insertResponse.InnerBody != null)
                {
                    _dbContext.HuEvaluateConcurrentImports.RemoveRange(tmp);
                    _dbContext.SaveChanges();
                }
                return insertResponse;
            }
            catch (Exception ex)
            {

                return new FormatedResponse() { ErrorType = EnumErrorType.UNCATCHABLE, StatusCode = EnumStatusCode.StatusCode500, MessageCode = ex.Message };
            }
        }
    }
}

