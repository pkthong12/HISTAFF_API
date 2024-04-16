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
    public class HuWorkingImportRepository : IHuWorkingImportRepository
    {
        private readonly GenericUnitOfWork _uow;
        private readonly FullDbContext _dbContext;
        private readonly GenericReducer<HU_WORKING_IMPORT, HuWorkingImportDTO> _genericReducer;
        private IGenericRepository<HU_WORKING, HuWorkingDTO> _genericRepository;
        private readonly List<string> XLSX_COLUMNS;

        public HuWorkingImportRepository(FullDbContext context, GenericUnitOfWork uow)
        {
            _dbContext = context;
            _uow = uow;
            _genericReducer = new();
            _genericRepository = _uow.GenericRepository<HU_WORKING, HuWorkingDTO>();
            XLSX_COLUMNS = new() { "XLSX_USER_ID", "XLSX_EX_CODE", "XLSX_INSERT_ON", "XLSX_SESSION", "XLSX_FILE_NAME", "XLSX_ROW" };
        }

        public async Task<GenericPhaseTwoListResponse<HuWorkingImportDTO>> SinglePhaseQueryList(GenericQueryListDTO<HuWorkingImportDTO> request)
        {
            var otherList = _uow.Context.Set<SYS_OTHER_LIST>().AsNoTracking().AsQueryable();
            var employee = _uow.Context.Set<HU_EMPLOYEE>().AsNoTracking().AsQueryable();
            var organization = _uow.Context.Set<HU_ORGANIZATION>().AsNoTracking().AsQueryable();
            var  position = _uow.Context.Set<HU_POSITION>().AsNoTracking().AsQueryable();
            var employeCv = _uow.Context.Set<HU_EMPLOYEE_CV>().AsNoTracking().AsQueryable();
            var company = _uow.Context.Set<HU_COMPANY>().AsNoTracking().AsQueryable();  
            var joined = from p in _dbContext.HuWorkingImports
                         from e in employee.Where(x => x.ID == p.EMPLOYEE_ID).DefaultIfEmpty()
                         from cv in employeCv.Where(x => x.ID == e.PROFILE_ID).DefaultIfEmpty()
                         from po in position.Where(x => x.ID == p.POSITION_ID).DefaultIfEmpty()
                         from o in organization.Where(x => x.ID == p.ORG_ID).DefaultIfEmpty()
                         from com in company.Where(x => x.ID == o.COMPANY_ID).DefaultIfEmpty()
                         from ot in otherList.Where(x => x.ID == p.TYPE_ID).DefaultIfEmpty()
                         from emobject in otherList.Where(x => x.ID == p.EMPLOYEE_OBJ_ID).DefaultIfEmpty()
                         from curpo in position.Where(x =>x.ID == p.CUR_POSITION_ID).DefaultIfEmpty()
                             // JOIN OTHER ENTITIES BASED ON THE BUSINESS
                         select new HuWorkingImportDTO
                         {
                             Id = p.XLSX_ROW,
                             XlsxUserId = p.XLSX_USER_ID,
                             XlsxExCode = p.XLSX_EX_CODE,
                             XlsxSession = p.XLSX_SESSION,
                             XlsxInsertOn = p.XLSX_INSERT_ON,
                             XlsxFileName = p.XLSX_FILE_NAME,
                             XlsxRow = p.XLSX_ROW,
                             EmployeeCode = e.CODE,
                             EmployeeName = cv.FULL_NAME,
                             PositionName = po.NAME,
                             OrgName = o.NAME,
                             DecisionNo = p.DECISION_NO,
                             TypeName = ot.NAME,
                             EffectDate = p.EFFECT_DATE,
                             ExpireDate = p.EXPIRE_DATE,
                             WorkPlaceName = com.WORK_ADDRESS,
                             EmployeeObjName = emobject.NAME,
                             Note = p.NOTE,
                             CurPositionName = curpo.NAME
                         };

            var singlePhaseResult = await _genericReducer.SinglePhaseReduce(joined, request);
            return singlePhaseResult;
        }

        public async Task<FormatedResponse> Save(ImportQueryListBaseDTO request)
        {
            try
            {
                bool pathMode = true;
                bool checkEffectDate = false;
                bool checkNoneExpireDate = false;
                bool checkData = false;
                var now = DateTime.UtcNow;
                List<HuWorkingDTO> listAdd = new();
                var getOtherList = (from t in _uow.Context.Set<SYS_OTHER_LIST_TYPE>().Where(x => x.CODE == "STATUS")
                                    from o in _uow.Context.Set<SYS_OTHER_LIST>().Where(x => x.TYPE_ID == t.ID).DefaultIfEmpty()
                                    where o.CODE == "CD"
                                    select new { Id = o.ID }).FirstOrDefault();
                var tmp = await _dbContext.HuWorkingImports.Where(x => x.XLSX_USER_ID == request.XlsxSid && x.XLSX_EX_CODE == request.XlsxExCode && x.XLSX_SESSION == request.XlsxSession).ToListAsync();

                var tmpType = typeof(HU_WORKING_IMPORT);
                var tmpProperties = tmpType.GetProperties()?
                    .Where(x => !XLSX_COLUMNS.Contains(x.Name))
                    .ToList();

                var evaluate = typeof(HU_WORKING);
                var evaluateProperties = evaluate.GetProperties().ToList();
                tmp.ForEach(item =>
                {
                    var obj1 = Activator.CreateInstance(typeof(HU_WORKING)) ?? throw new Exception(CommonMessageCode.ACTIVATOR_CREATE_INSTANCE_RETURNS_NULL);
                    var e = (HU_WORKING)obj1;

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
                    var getCheckData = _uow.Context.Set<HU_WORKING>().Where(x => x.EMPLOYEE_ID == item.EMPLOYEE_ID).OrderByDescending(x => x.EFFECT_DATE).Take(1).FirstOrDefault();
                    if(item.EFFECT_DATE.Value < getCheckData.EFFECT_DATE.Value && getCheckData.EXPIRE_DATE == null)
                    {
                        checkNoneExpireDate = true;
                        return;
                    }
                    if(item.EFFECT_DATE.Value < getCheckData.EXPIRE_DATE)
                    {
                        checkEffectDate = true;
                        return;
                    }
                    if(item.EFFECT_DATE.Value < getCheckData.EXPIRE_DATE)
                    {

                    }
                    listAdd.Add(new()
                    {
                        EmployeeId = item.EMPLOYEE_ID,
                        PositionId = item.POSITION_ID,
                        OrgId = item.ORG_ID,
                        EffectDate = item.EFFECT_DATE,
                        ExpireDate = item.EXPIRE_DATE,
                        TypeId = item.TYPE_ID,
                        DecisionNo = item.DECISION_NO,
                        IsResponsible = item.IS_RESPONSIBLE,
                        EmployeeObjId = item.EMPLOYEE_OBJ_ID,
                        WageId = item.WAGE_ID,
                        SignId = item.SIGN_ID,
                        SignDate = item.SIGN_DATE,
                        Note = item.NOTE,
                        StatusId = getOtherList.Id
                    });


                });
                if(checkNoneExpireDate == true)
                {
                    return new FormatedResponse() { ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400, MessageCode = CommonMessageCode.EFFECT_DATE_MORE_THAN_EFFECT_DATE_OF_OLD_RECORD };
                }
                var insertResponse = await _genericRepository.CreateRange(_uow, listAdd, request.XlsxSid);
                if (insertResponse.InnerBody != null)
                {
                    _dbContext.HuWorkingImports.RemoveRange(tmp);
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

