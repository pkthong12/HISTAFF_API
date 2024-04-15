using CORE.GenericUOW;
using CORE.DTO;
using API.DTO;
using CORE.Enum;
using CORE.StaticConstant;
using API.All.DbContexts;
using Common.Extensions;
using API.All.HRM.Insurance.InsuranceAPI.Business.InsChange;
using System.Linq.Dynamic.Core;
using Common.Interfaces;
using Common.DataAccess;

namespace API.Controllers.InsChange
{
    public class InsChangeRepository : IInsChangeRepository
    {
        private readonly GenericUnitOfWork _uow;
        private readonly FullDbContext _dbContext;
        private IGenericRepository<INS_CHANGE, InsChangeDTO> _genericRepository;
        private readonly GenericReducer<INS_CHANGE, InsChangeDTO> _genericReducer;
        protected AbsQueryDataTemplate QueryData;
        public InsChangeRepository(FullDbContext context, GenericUnitOfWork uow)
        {
            _dbContext = context;
            _uow = uow;
            _genericRepository = _uow.GenericRepository<INS_CHANGE, InsChangeDTO>();
            _genericReducer = new();
            QueryData = new SqlQueryDataTemplate(_dbContext);
        }

        public async Task<FormatedResponse> SinglePhaseQueryList(GenericQueryListDTO<InsChangeDTO> request)
        {
            var joined = from p in _dbContext.InsChanges.AsNoTracking()
                             // JOIN OTHER ENTITIES BASED ON THE BUSINESS
                         from e in _dbContext.HuEmployees.AsNoTracking().Where(emp => emp.ID == p.EMPLOYEE_ID).DefaultIfEmpty()
                             //from inf in _dbContext.InsInformations.AsNoTracking().Where(inf => inf.EMPLOYEE_ID == p.EMPLOYEE_ID).OrderByDescending(inf => inf.ID).Take(1)
                         from cv in _dbContext.HuEmployeeCvs.AsNoTracking().Where(cv => cv.ID == e.PROFILE_ID).DefaultIfEmpty()
                         from pr in _dbContext.HuProvinces.AsNoTracking().Where(c => c.ID == cv.ID_PLACE).DefaultIfEmpty()
                         from t in _dbContext.HuPositions.AsNoTracking().Where(x => x.ID == e.POSITION_ID).DefaultIfEmpty()
                         from j in _dbContext.HuJobs.AsNoTracking().Where(x => x.ID == t.JOB_ID).DefaultIfEmpty()
                         from o in _dbContext.HuOrganizations.AsNoTracking().Where(x => x.ID == e.ORG_ID).DefaultIfEmpty()
                         from cmp in _dbContext.HuCompanys.AsNoTracking().Where(cmp => cmp.ID == o.COMPANY_ID).DefaultIfEmpty()
                         from type in _dbContext.InsTypes.AsNoTracking().Where(type => type.ID == p.CHANGE_TYPE_ID).DefaultIfEmpty()
                         from otl in _dbContext.SysOtherLists.AsNoTracking().Where(otl => otl.ID == p.UNIT_INSURANCE_TYPE_ID).DefaultIfEmpty()
                         select new InsChangeDTO
                         {
                             Id = p.ID,
                             EmployeeId = p.EMPLOYEE_ID,
                             EmployeeCode = e.CODE,
                             JobOrderNum = (int)(j.ORDERNUM ?? 999),
                             EmployeeName = cv.FULL_NAME,
                             OrgId = e.ORG_ID,
                             OrgName = o.NAME,
                             PositionName = t.NAME,
                             CompName = cmp.NAME_VN,
                             BhxhNo = cv.INSURENCE_NUMBER,
                             IdNo = cv.ID_NO,
                             IdDate = cv.ID_DATE,
                             IdPlace = cv.ID_PLACE,
                             PlaceName = pr.NAME,
                             IsBhxh = p.IS_BHXH,
                             IsBhyt = p.IS_BHYT,
                             IsBhtn = p.IS_BHTN,
                             IsBnn = p.IS_BNN,
                             BirthDate = cv.BIRTH_DATE,
                             BirthPlace = cv.BIRTH_PLACE,
                             AddressIdentity = pr.NAME,
                             UnitInsuranceTypeId = p.UNIT_INSURANCE_TYPE_ID,
                             UnitInsuranceTypeName = otl.NAME,
                             ChangeTypeId = p.CHANGE_TYPE_ID,
                             ChangeTypeName = type.NAME,
                             SalaryBhxhBhytOld = p.SALARY_BHXH_BHYT_OLD ?? 0,
                             SalaryBhxhBhytNew = p.SALARY_BHXH_BHYT_NEW ?? 0,
                             SalaryBhtnOld = p.SALARY_BHTN_OLD ?? 0,
                             SalaryBhtnNew = p.SALARY_BHTN_NEW ?? 0,
                             EffectiveDate = p.EFFECTIVE_DATE,
                             ExpireDate = p.EXPIRE_DATE,
                             ChangeMonth = p.CHANGE_MONTH,
                             DeclarationPeriod = p.DECLARATION_PERIOD,
                             BhytReimbursementDate = p.BHYT_REIMBURSEMENT_DATE,
                             Note = p.NOTE,

                             ArrearsFromMonth = p.ARREARS_FROM_MONTH,
                             ArrearsToMonth = p.ARREARS_TO_MONTH,
                             ArBhxhSalaryDifference = p.AR_BHXH_SALARY_DIFFERENCE ?? 0,
                             ArBhytSalaryDifference = p.AR_BHYT_SALARY_DIFFERENCE ?? 0,
                             ArBhtnldBnnSalaryDifference = p.AR_BHTNLD_BNN_SALARY_DIFFERENCE ?? 0,
                             ArBhtnSalaryDifference = p.AR_BHTN_SALARY_DIFFERENCE ?? 0,

                             WithdrawalFromMonth = p.WITHDRAWAL_FROM_MONTH,
                             WithdrawalToMonth = p.WITHDRAWAL_TO_MONTH,
                             WdBhxhSalaryDifference = p.WD_BHXH_SALARY_DIFFERENCE ?? 0,
                             WdBhytSalaryDifference = p.WD_BHYT_SALARY_DIFFERENCE ?? 0,
                             WdBhtnldBnnSalaryDifference = p.WD_BHTNLD_BNN_SALARY_DIFFERENCE ?? 0,
                             WdBhtnSalaryDifference = p.WD_BHTN_SALARY_DIFFERENCE ?? 0,

                             ChangeMonthString = p.CHANGE_MONTH == null ? "" : p.CHANGE_MONTH!.Value.ToString("MM/yyyy"),
                             DeclarationPeriodString = p.DECLARATION_PERIOD == null ? "" : p.DECLARATION_PERIOD!.Value.ToString("MM/yyyy"),
                             ArrearsFromMonthString = p.ARREARS_FROM_MONTH == null ? "" : p.ARREARS_FROM_MONTH!.Value.ToString("MM/yyyy"),
                             ArrearsToMonthString = p.ARREARS_TO_MONTH == null ? "" : p.ARREARS_TO_MONTH!.Value.ToString("MM/yyyy"),
                             WithdrawalFromMonthString = p.WITHDRAWAL_FROM_MONTH == null ? "" : p.WITHDRAWAL_FROM_MONTH!.Value.ToString("MM/yyyy"),
                             WithdrawalToMonthString = p.WITHDRAWAL_TO_MONTH == null ? "" : p.WITHDRAWAL_TO_MONTH!.Value.ToString("MM/yyyy"),


                         };
            var searchForChangeMonth = "";
            var searchForDeclarationPeriod = "";
            var skip = request.Pagination!.Skip;
            var take = request.Pagination.Take;
            request.Pagination.Skip = 0;
            request.Pagination.Take = 9999;
            if (request.Search != null)
            {
                request.Search.ForEach(x =>
                {
                    if (x.Field == "changeMonthString")
                    {
                        searchForChangeMonth = x.SearchFor.ToString().ToLower().Trim();
                        x.SearchFor = "";
                    }
                    if (x.Field == "declarationPeriodString")
                    {
                        searchForDeclarationPeriod = x.SearchFor.ToString().ToLower().Trim();
                        x.SearchFor = "";
                    }
                });
            }

            var singlePhaseResult = await _genericReducer.SinglePhaseReduce(joined, request);

            var resultList = new List<InsChangeDTO>();
            foreach (var i in singlePhaseResult.List!)
            {
                resultList.Add(i);
            }

            resultList = resultList.Where(x => (x.ChangeMonthString!.Trim().Contains(searchForChangeMonth))).ToList();
            resultList = resultList.Where(x => (x.DeclarationPeriodString!.Trim().Contains(searchForDeclarationPeriod))).ToList();
            var result = new
            {
                Count = resultList.Count(),
                List = resultList.Skip(skip).Take(take),
                Page = (skip / take) + 1,
                PageCount = resultList.Count(),
                Skip = skip,
                Take = take,
                MessageCode = CommonMessageCode.QUERY_LIST_SUCCESS,
            };
            return new()
            {
                InnerBody = result,
            };
        }

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

            try
            {
                var joined = (from p in _dbContext.InsChanges.AsNoTracking().Where(p => p.ID == id)
                              from e in _dbContext.HuEmployees.AsNoTracking().Where(e => e.ID == p.EMPLOYEE_ID).DefaultIfEmpty()
                              from cv in _dbContext.HuEmployeeCvs.AsNoTracking().Where(cv => cv.ID == e.PROFILE_ID).DefaultIfEmpty()
                              from pr in _dbContext.HuProvinces.AsNoTracking().Where(c => c.ID == cv.ID_PLACE).DefaultIfEmpty()
                              from org in _dbContext.HuOrganizations.AsNoTracking().Where(org => org.ID == e.ORG_ID).DefaultIfEmpty()
                              from pos in _dbContext.HuPositions.AsNoTracking().Where(pos => pos.ID == e.POSITION_ID).DefaultIfEmpty()
                                  //from i in _dbContext.InsInformations.AsNoTracking().Where(i => i.EMPLOYEE_ID == p.EMPLOYEE_ID).DefaultIfEmpty()
                              from otl in _dbContext.SysOtherLists.AsNoTracking().Where(otl => otl.ID == p.UNIT_INSURANCE_TYPE_ID).DefaultIfEmpty()
                              from c in _dbContext.InsTypes.AsNoTracking().Where(c => c.ID == p.CHANGE_TYPE_ID).DefaultIfEmpty()
                              select new InsChangeDTO
                              {
                                  Id = p.ID,

                                  EmployeeId = p.EMPLOYEE_ID,
                                  EmployeeCode = e.CODE,//ma
                                  EmployeeName = cv.FULL_NAME,//ten nv
                                  PositionName = pos.NAME,//phong/ban
                                  OrgId = e.ORG_ID,
                                  OrgName = org.NAME,//chuc danh
                                  BhxhNo = cv.INSURENCE_NUMBER,
                                  //BhxhNo = i.BHXH_NO,
                                  IdNo = cv.ID_NO,
                                  IdDate = cv.ID_DATE,
                                  IdPlace = cv.ID_PLACE,
                                  BirthDate = cv.BIRTH_DATE,
                                  //BirthPlace = cv.BIRTH_PLACE,
                                  AddressIdentity = pr.NAME,
                                  IsBhxh = p.IS_BHXH,
                                  IsBhyt = p.IS_BHYT,
                                  IsBhtn = p.IS_BHTN,
                                  IsBnn = p.IS_BNN,
                                  UnitInsuranceTypeId = p.UNIT_INSURANCE_TYPE_ID,
                                  UnitInsuranceTypeName = otl.NAME,
                                  ChangeTypeId = p.CHANGE_TYPE_ID,
                                  ChangeTypeName = c.CODE,
                                  SalaryBhxhBhytOld = p.SALARY_BHXH_BHYT_OLD ?? 0,
                                  SalaryBhxhBhytNew = p.SALARY_BHXH_BHYT_NEW ?? 0,
                                  SalaryBhtnOld = p.SALARY_BHTN_OLD ?? 0,
                                  SalaryBhtnNew = p.SALARY_BHTN_NEW ?? 0,
                                  EffectiveDate = p.EFFECTIVE_DATE,
                                  ExpireDate = p.EXPIRE_DATE,
                                  ChangeMonth = p.CHANGE_MONTH,
                                  DeclarationPeriod = p.DECLARATION_PERIOD,
                                  BhytReimbursementDate = p.BHYT_REIMBURSEMENT_DATE,
                                  Note = p.NOTE,

                                  ArrearsFromMonth = p.ARREARS_FROM_MONTH,
                                  ArrearsToMonth = p.ARREARS_TO_MONTH,
                                  ArBhxhSalaryDifference = p.AR_BHXH_SALARY_DIFFERENCE ?? 0,
                                  ArBhytSalaryDifference = p.AR_BHYT_SALARY_DIFFERENCE ?? 0,
                                  ArBhtnldBnnSalaryDifference = p.AR_BHTNLD_BNN_SALARY_DIFFERENCE ?? 0,
                                  ArBhtnSalaryDifference = p.AR_BHTN_SALARY_DIFFERENCE ?? 0,

                                  WithdrawalFromMonth = p.WITHDRAWAL_FROM_MONTH,
                                  WithdrawalToMonth = p.WITHDRAWAL_TO_MONTH,
                                  WdBhxhSalaryDifference = p.WD_BHXH_SALARY_DIFFERENCE ?? 0,
                                  WdBhytSalaryDifference = p.WD_BHYT_SALARY_DIFFERENCE ?? 0,
                                  WdBhtnldBnnSalaryDifference = p.WD_BHTNLD_BNN_SALARY_DIFFERENCE ?? 0,
                                  WdBhtnSalaryDifference = p.WD_BHTN_SALARY_DIFFERENCE ?? 0,

                                  ChangeMonthString = p.CHANGE_MONTH!.Value.ToString("yyyy-MM"),
                                  DeclarationPeriodString = p.DECLARATION_PERIOD!.Value.ToString("yyyy-MM"),
                                  ArrearsFromMonthString = p.ARREARS_FROM_MONTH!.Value.ToString("yyyy-MM"),
                                  ArrearsToMonthString = p.ARREARS_TO_MONTH!.Value.ToString("yyyy-MM"),
                                  WithdrawalFromMonthString = p.WITHDRAWAL_FROM_MONTH!.Value.ToString("yyyy-MM"),
                                  WithdrawalToMonthString = p.WITHDRAWAL_TO_MONTH!.Value.ToString("yyyy-MM"),

                              }).FirstOrDefault();
                if (joined != null)
                {
                    List<int> lstcheckInsItems = new List<int>();
                    if (joined.IsBhxh.HasValue != null && joined.IsBhxh == true)
                    {
                        lstcheckInsItems.Add(1);
                    }
                    if (joined.IsBhyt.HasValue != null && joined.IsBhyt == true)
                    {
                        lstcheckInsItems.Add(2);
                    }
                    if (joined.IsBhtn.HasValue != null && joined.IsBhtn == true)
                    {
                        lstcheckInsItems.Add(3);
                    }
                    if (joined.IsBnn.HasValue != null && joined.IsBnn == true)
                    {
                        lstcheckInsItems.Add(4);
                    }

                    joined.InsuranceType.AddRange(lstcheckInsItems.Distinct().ToList());
                    return new() { InnerBody = joined };
                }
                else
                {
                    return new FormatedResponse() { MessageCode = CommonMessageCode.ENTITY_NOT_FOUND, ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400 };
                }
            }
            catch (Exception ex)
            {
                return new FormatedResponse() { MessageCode = CommonMessageCode.NULLABLE_DATE_TIME_EXTENSION_METHOD_NOT_FOUND, ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode500 };
            }

        }

        public async Task<FormatedResponse> GetById(string id)
        {
            await Task.Run(() => null);
            throw new NotImplementedException();
        }

        public async Task<FormatedResponse> Create(GenericUnitOfWork _uow, InsChangeDTO dto, string sid)
        {
            var type = _dbContext.InsTypes.AsNoTracking().Where(p => p.ID == dto.ChangeTypeId && p.CODE == "TM").FirstOrDefault();
            if(dto.EmployeeId==null || dto.UnitInsuranceTypeId ==null || dto.ChangeTypeId ==null || dto.InsuranceType == null || dto.EffectiveDate ==null || dto.DeclarationPeriodString == null)
            {
                return new FormatedResponse() { MessageCode = CommonMessageCode.CONTROL_REQUEID_NOT_NULL, ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400 };
            }
            var check = _dbContext.InsChanges.AsNoTracking().Where(p => (type == null) ? false : (p.EMPLOYEE_ID == dto.EmployeeId && p.CHANGE_TYPE_ID == type.ID)).Any();
            if (check)
            {
                return new FormatedResponse() { MessageCode = CommonMessageCode.EMPLOYEE_HAVE_EXIST_NEW_INSCREASE, ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400 };
            }
            var monthDeclaration = (from t in _dbContext.InsTypes.AsNoTracking().Where(p => p.CODE == "TM").DefaultIfEmpty()
                                    from c in _dbContext.InsChanges.AsNoTracking().Where(p => p.CHANGE_TYPE_ID == t.ID && p.EMPLOYEE_ID == dto.EmployeeId)
                                    select c.DECLARATION_PERIOD).FirstOrDefault();
            if (monthDeclaration != null)
            {
                dto.DeclarationPeriod = DateTime.Parse(dto.DeclarationPeriodString!);
                if (monthDeclaration > dto.DeclarationPeriod)
                {
                    return new FormatedResponse() { MessageCode = CommonMessageCode.DECLARATION_PERIOD_MORE_THAN_DECLARATION_PERIOD_NEW, ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400 };
                }
            }

            // convert month/year to day/month/year
            if (dto.DeclarationPeriodString != null && dto.DeclarationPeriodString != "")
            {
                dto.DeclarationPeriod = DateTime.Parse(DateTime.Parse(dto.DeclarationPeriodString).ToString("yyyy-MM-dd HH:mm:ss.fff"));
            }
            if (dto.ChangeMonthString != null && dto.ChangeMonthString != "")
            {
                dto.ChangeMonth = DateTime.Parse(DateTime.Parse(dto.ChangeMonthString).ToString("yyyy-MM-dd HH:mm:ss.fff"));
            }
            if (dto.ArrearsFromMonthString != null && dto.ArrearsFromMonthString != "")
            {
                dto.ArrearsFromMonth = DateTime.Parse(DateTime.Parse(dto.ArrearsFromMonthString).ToString("yyyy-MM-dd HH:mm:ss.fff"));
            }
            if (dto.ArrearsToMonthString != null && dto.ArrearsToMonthString != "")
            {
                dto.ArrearsToMonth = DateTime.Parse(DateTime.Parse(dto.ArrearsToMonthString).ToString("yyyy-MM-dd HH:mm:ss.fff"));
            }
            if (dto.WithdrawalFromMonthString != null && dto.WithdrawalFromMonthString != "")
            {
                dto.WithdrawalFromMonth = DateTime.Parse(DateTime.Parse(dto.WithdrawalFromMonthString).ToString("yyyy-MM-dd HH:mm:ss.fff"));
            }
            if (dto.WithdrawalToMonthString != null && dto.WithdrawalToMonthString != "")
            {
                dto.WithdrawalToMonth = DateTime.Parse(DateTime.Parse(dto.WithdrawalToMonthString).ToString("yyyy-MM-dd HH:mm:ss.fff"));
            }
            // add insurance
            if (dto.InsuranceType != null)
            {
                dto.IsBhxh = dto.InsuranceType.Contains(1);
                dto.IsBhyt = dto.InsuranceType.Contains(2);
                dto.IsBhtn = dto.InsuranceType.Contains(3);
                dto.IsBnn = dto.InsuranceType.Contains(4);
            }
            // check ins-specifiedobject
            var specObj = _dbContext.InsSpecifiedObjectss.Where(p => dto.EffectiveDate!.Value.Date >= p.EFFECTIVE_DATE!.Value.Date).OrderByDescending(p => p.EFFECTIVE_DATE).FirstOrDefault();
            if (specObj != null)
            {
                if (specObj!.UI < dto.SalaryBhtnNew)
                {
                    return new FormatedResponse() { MessageCode = CommonMessageCode.UI_NEW_MUST_LESS_THAN_UI_OBJ, ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400 };
                }
                if (specObj!.UI < dto.SalaryBhtnOld)
                {
                    return new FormatedResponse() { MessageCode = CommonMessageCode.UI_OLD_MUST_LESS_THAN_UI_OBJ, ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400 };
                }
            }

            // check ui
            try
            {
                var region = await (from e in _dbContext.HuEmployees.AsNoTracking().Where(e => e.ID == dto.EmployeeId).DefaultIfEmpty()
                                    from p in _dbContext.HuPositions.AsNoTracking().Where(p => p.ID == e.POSITION_ID).DefaultIfEmpty()
                                    from o in _dbContext.HuOrganizations.AsNoTracking().Where(o => o.ID == p.ORG_ID).DefaultIfEmpty()
                                    from c in _dbContext.HuCompanys.AsNoTracking().Where(c => c.ID == o.COMPANY_ID).DefaultIfEmpty()
                                    from r in _dbContext.InsRegions.AsNoTracking().Where(r => r.AREA_ID == c.REGION_ID && dto.EffectiveDate!.Value.Date >= r.EFFECT_DATE!.Value.Date || (r.EFFECT_DATE!.Value.Date <= dto.EffectiveDate!.Value.Date && dto.EffectiveDate!.Value.Date <= r.EXPRIVED_DATE!.Value.Date)).DefaultIfEmpty()
                                    orderby r.EFFECT_DATE descending
                                    select new
                                    {
                                        RegionId = r.ID,
                                        RegionCeilingUi = r.CEILING_UI
                                    }).FirstOrDefaultAsync();

                //var regionSalary = _dbContext.InsRegions.Where(p => dto.EffectiveDate!.Value.Date >= p.EFFECT_DATE!.Value.Date).OrderByDescending(p => p.EFFECT_DATE).FirstOrDefault();
                if (region != null)
                {
                    if (region!.RegionCeilingUi < dto.SalaryBhtnNew)
                    {
                        return new FormatedResponse() { MessageCode = CommonMessageCode.UI_NEW_MUST_LESS_THAN_UI_OBJ, ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400 };
                    }
                    if (region!.RegionCeilingUi < dto.SalaryBhtnOld)
                    {
                        return new FormatedResponse() { MessageCode = CommonMessageCode.UI_OLD_MUST_LESS_THAN_UI_OBJ, ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400 };
                    }
                }
                var response = await _genericRepository.Create(_uow, dto, sid);
                return response;
            }
            catch (Exception ex)
            {
                return new FormatedResponse() { MessageCode = "DATA_FAILED_MAY_BE_IS_REGION", ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400 };
            }
        }

        public async Task<FormatedResponse> CreateRange(GenericUnitOfWork _uow, List<InsChangeDTO> dtos, string sid)
        {
            var add = new List<InsChangeDTO>();
            add.AddRange(dtos);
            var response = await _genericRepository.CreateRange(_uow, add, sid);
            return response;
        }

        public async Task<FormatedResponse> Update(GenericUnitOfWork _uow, InsChangeDTO dto, string sid, bool patchMode = true)
        {
            var type = _dbContext.InsTypes.AsNoTracking().Where(p => p.ID == dto.ChangeTypeId && p.CODE == "TM").FirstOrDefault();
            var check = _dbContext.InsChanges.AsNoTracking().Where(p => (type == null) ? false : (p.ID != dto.Id && p.EMPLOYEE_ID == dto.EmployeeId && p.CHANGE_TYPE_ID == type.ID)).Any();
            if (check)
            {
                return new FormatedResponse() { MessageCode = CommonMessageCode.EMPLOYEE_HAVE_EXIST_NEW_INSCREASE, ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400 };
            }
            var monthDeclaration = (from t in _dbContext.InsTypes.AsNoTracking().Where(p => p.CODE == "TM").DefaultIfEmpty()
                                    from c in _dbContext.InsChanges.AsNoTracking().Where(p => p.CHANGE_TYPE_ID == t.ID && p.EMPLOYEE_ID == dto.EmployeeId)
                                    select c).FirstOrDefault();
            if (monthDeclaration != null)
            {
                if (monthDeclaration!.ID != dto.Id)
                {

                    dto.DeclarationPeriod = DateTime.Parse(dto.DeclarationPeriodString!);
                    if (monthDeclaration.DECLARATION_PERIOD > dto.DeclarationPeriod)
                    {
                        return new FormatedResponse() { MessageCode = CommonMessageCode.DECLARATION_PERIOD_MORE_THAN_DECLARATION_PERIOD_NEW, ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400 };
                    }
                }
            }

            // convert month/year to day/month/year
            if (dto.DeclarationPeriodString != null && dto.DeclarationPeriodString != "")
            {
                dto.DeclarationPeriod = DateTime.Parse(DateTime.Parse(dto.DeclarationPeriodString).ToString("yyyy-MM-dd HH:mm:ss.fff"));
            }
            if (dto.ChangeMonthString != null && dto.ChangeMonthString != "")
            {
                dto.ChangeMonth = DateTime.Parse(DateTime.Parse(dto.ChangeMonthString).ToString("yyyy-MM-dd HH:mm:ss.fff"));
            }
            if (dto.ArrearsFromMonthString != null && dto.ArrearsFromMonthString != "")
            {
                dto.ArrearsFromMonth = DateTime.Parse(DateTime.Parse(dto.ArrearsFromMonthString).ToString("yyyy-MM-dd HH:mm:ss.fff"));
            }
            if (dto.ArrearsToMonthString != null && dto.ArrearsToMonthString != "")
            {
                dto.ArrearsToMonth = DateTime.Parse(DateTime.Parse(dto.ArrearsToMonthString).ToString("yyyy-MM-dd HH:mm:ss.fff"));
            }
            if (dto.WithdrawalFromMonthString != null && dto.WithdrawalFromMonthString != "")
            {
                dto.WithdrawalFromMonth = DateTime.Parse(DateTime.Parse(dto.WithdrawalFromMonthString).ToString("yyyy-MM-dd HH:mm:ss.fff"));
            }
            if (dto.WithdrawalToMonthString != null && dto.WithdrawalToMonthString != "")
            {
                dto.WithdrawalToMonth = DateTime.Parse(DateTime.Parse(dto.WithdrawalToMonthString).ToString("yyyy-MM-dd HH:mm:ss.fff"));
            }
            // add insurance
            if (dto.InsuranceType != null)
            {
                dto.IsBhxh = dto.InsuranceType.Contains(1);
                dto.IsBhyt = dto.InsuranceType.Contains(2);
                dto.IsBhtn = dto.InsuranceType.Contains(3);
                dto.IsBnn = dto.InsuranceType.Contains(4);
            }
            // check ins-specifiedobject
            var specObj = _dbContext.InsSpecifiedObjectss.Where(p => dto.EffectiveDate!.Value.Date >= p.EFFECTIVE_DATE!.Value.Date).OrderByDescending(p => p.EFFECTIVE_DATE).FirstOrDefault();
            if (specObj != null)
            {
                if (specObj!.UI < dto.SalaryBhtnNew)
                {
                    return new FormatedResponse() { MessageCode = CommonMessageCode.UI_NEW_MUST_LESS_THAN_UI_OBJ, ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400 };
                }
                if (specObj!.UI < dto.SalaryBhtnOld)
                {
                    return new FormatedResponse() { MessageCode = CommonMessageCode.UI_OLD_MUST_LESS_THAN_UI_OBJ, ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400 };
                }
            }

            // check ui
            var region = await (from e in _dbContext.HuEmployees.AsNoTracking().Where(e => e.ID == dto.EmployeeId).DefaultIfEmpty()
                                from p in _dbContext.HuPositions.AsNoTracking().Where(p => p.ID == e.POSITION_ID).DefaultIfEmpty()
                                from o in _dbContext.HuOrganizations.AsNoTracking().Where(o => o.ID == p.ORG_ID).DefaultIfEmpty()
                                from c in _dbContext.HuCompanys.AsNoTracking().Where(c => c.ID == o.COMPANY_ID).DefaultIfEmpty()
                                from r in _dbContext.InsRegions.AsNoTracking().Where(r => r.AREA_ID == c.REGION_ID && dto.EffectiveDate!.Value.Date >= r.EFFECT_DATE!.Value.Date || (r.EFFECT_DATE!.Value.Date <= dto.EffectiveDate!.Value.Date && dto.EffectiveDate!.Value.Date <= r.EXPRIVED_DATE!.Value.Date)).DefaultIfEmpty()
                                orderby r.EFFECT_DATE descending
                                select new InsRegionDTO
                                {
                                    Id = r.ID,
                                    CeilingUi = r.CEILING_UI,
                                    EffectDate = r.EFFECT_DATE,
                                    ExprivedDate = r.EXPRIVED_DATE
                                }).FirstOrDefaultAsync();
            //var regionSalary = _dbContext.InsRegions.Where(p => dto.EffectiveDate!.Value.Date >= p.EFFECT_DATE!.Value.Date).OrderByDescending(p => p.EFFECT_DATE).FirstOrDefault();
            if (region != null)
            {
                if (region!.CeilingUi < dto.SalaryBhtnNew)
                {
                    return new FormatedResponse() { MessageCode = CommonMessageCode.UI_NEW_MUST_LESS_THAN_UI_OBJ, ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400 };
                }
                if (region!.CeilingUi < dto.SalaryBhtnOld)
                {
                    return new FormatedResponse() { MessageCode = CommonMessageCode.UI_OLD_MUST_LESS_THAN_UI_OBJ, ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400 };
                }
            }
            var response = await _genericRepository.Update(_uow, dto, sid, patchMode);
            return response;
        }

        public async Task<FormatedResponse> UpdateRange(GenericUnitOfWork _uow, List<InsChangeDTO> dtos, string sid, bool patchMode = true)
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


        public async Task<FormatedResponse> GetOtherListType()
        {
            var query = await (from t in _dbContext.SysOtherLists.AsNoTracking()
                                .Where(t => t.IS_ACTIVE == true && t.TYPE_ID == 44)
                               orderby t.ID
                               select new
                               {
                                   UnitInsuranceTypeId = t.ID,
                                   UnitInsuranceTypeName = t.NAME,
                               }).ToListAsync();
            return new FormatedResponse() { InnerBody = query };
        }

        public async Task<FormatedResponse> GetInsTypeChange()
        {
            var query = await (from i in _dbContext.InsTypes.AsNoTracking()
                               where i.IS_ACTIVE == true
                               orderby i.ID
                               select new
                               {
                                   ChangeTypeId = i.ID,
                                   ChangeTypeName = i.NAME,
                                   Code = i.CODE
                               }).ToListAsync();
            return new FormatedResponse() { InnerBody = query };
        }


        public async Task<FormatedResponse> GetInforById(long id)
        {
            var joined = await (from e in _dbContext.HuEmployees.AsNoTracking().Where(emp => emp.ID == id)
                                from cv in _dbContext.HuEmployeeCvs.AsNoTracking().Where(cv => cv.ID == e.PROFILE_ID).DefaultIfEmpty()
                                    // JOIN OTHER ENTITIES BASED ON THE BUSINESS
                                select new InsChangeDTO
                                {
                                    EmployeeCode = e.CODE,
                                    BhxhNo = cv.INSURENCE_NUMBER
                                }).FirstOrDefaultAsync();
            if (joined != null)
            {
                return new FormatedResponse() { InnerBody = joined };
            }
            else
            {
                return new FormatedResponse() { MessageCode = CommonMessageCode.ENTITY_NOT_FOUND, ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400 };
            }
        }

        public virtual async Task<FormatedResponse> ToggleActiveIds(GenericUnitOfWork _uow, List<long> ids, bool valueToBind, string sid)
        {
            await Task.Run(() => null);
            throw new NotImplementedException();
        }

        public async Task<FormatedResponse> GetInschangeDashboard()
        {
            var query = await (
                             from e in _dbContext.HuEmployees.AsNoTracking()
                             from ins in _dbContext.InsChanges.AsNoTracking().Where(x => x.EMPLOYEE_ID == e.ID).DefaultIfEmpty()
                             select new
                             {
                                 Id = ins.EMPLOYEE_ID
                             }).ToListAsync();

            var result = new List<HighchartsInsDTO>
            {
                new()
                {
                    Name = "Đã tham gia",
                    Y = query.Sum(x => x.Id != null ? 1 : 0)
                },
                new()
                {
                    Name = "Chưa tham gia",
                    Y = query.Sum(x => x.Id == null ? 1 : 0)
                }
            };

            return new FormatedResponse() { InnerBody = result };
        }

        public async Task<FormatedResponse> GetLstInsCheck(long id)
        {
            List<int> lstcheckInsItems = new List<int>();
            var joined = await (from e in _dbContext.HuEmployees.AsNoTracking().Where(emp => emp.ID == id)
                                from a in _dbContext.InsArisings.AsNoTracking().Where(a => a.EMPLOYEE_ID == e.ID).DefaultIfEmpty()
                                    //from c in _dbContext.InsChanges.AsNoTracking().Where(c => c.EMPLOYEE_ID == e.ID).DefaultIfEmpty()
                                select new InsChangeDTO
                                {
                                    Id = e.ID,
                                    IsBhtn = a.UI,
                                    IsBhyt = a.HI,
                                    IsBhxh = a.SI,
                                    IsBnn = a.AI,
                                }).FirstAsync();
            if (joined != null)
            {
                if (joined.IsBhxh.HasValue != null && joined.IsBhxh == true)
                {
                    lstcheckInsItems.Add(1);
                }
                if (joined.IsBhyt.HasValue != null && joined.IsBhyt == true)
                {
                    lstcheckInsItems.Add(2);
                }
                if (joined.IsBhtn.HasValue != null && joined.IsBhtn == true)
                {
                    lstcheckInsItems.Add(3);
                }
                if (joined.IsBnn.HasValue != null && joined.IsBnn == true)
                {
                    lstcheckInsItems.Add(4);
                }

                joined.InsuranceType.AddRange(lstcheckInsItems.Distinct().ToList());
                return new FormatedResponse() { InnerBody = joined };
            }
            else
            {
                return new FormatedResponse() { MessageCode = CommonMessageCode.ENTITY_NOT_FOUND, ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400 };
            }
        }

        public async Task<FormatedResponse> GetUnit(long id)
        {
            var joined = await (from e in _dbContext.HuEmployees.AsNoTracking().Where(emp => emp.ID == id)
                                from o in _dbContext.HuOrganizations.AsNoTracking().Where(o => o.ID == e.ORG_ID).DefaultIfEmpty()
                                from c in _dbContext.HuCompanys.AsNoTracking().Where(c => c.ID == o.COMPANY_ID).DefaultIfEmpty()
                                from s in _dbContext.SysOtherLists.AsNoTracking().Where(s => s.ID == c.INS_UNIT).DefaultIfEmpty()
                                select new
                                {
                                    Id = s.ID,
                                    Code = s.CODE,
                                    Name = s.NAME
                                }).FirstAsync();
            if (joined != null)
            {
                return new FormatedResponse() { InnerBody = joined };
            }
            else
            {
                return new FormatedResponse() { MessageCode = CommonMessageCode.ENTITY_NOT_FOUND, ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400 };
            }
        }
        public async Task<FormatedResponse> SpsInsArisingManualLoad(InsChangeDTO dto)
        {
            var r = await QueryData.ExecuteList("SPS_INS_ARISING_MANUAL_LOAD",
                   new
                   {
                       P_EMPLOYEEID = dto.EmployeeId,
                       P_INS_ARISING_TYPE_ID = dto.ChangeTypeId,
                       P_ARISING_FROM_MONTH = dto.ChangeMonthString.Replace("-", "/"),
                       P_DECLARE_DATE = dto.DeclarationPeriodString.Replace("-", "/")
                   }, false);
            return new FormatedResponse() { InnerBody = r[0], StatusCode = EnumStatusCode.StatusCode200 };
        }
        public async Task<FormatedResponse> SpsInsArisingManualLoad2(InsChangeDTO dto)
        {
            var r = await QueryData.ExecuteList("SPS_INS_ARISING_MANUAL_LOAD2",
                   new
                   {
                       P_EMPLOYEEID = dto.EmployeeId,
                       P_INS_ARISING_TYPE_ID = dto.ChangeTypeId,
                       P_EFFECTIVE_DATE = dto.IsTruyThu == 1 ? dto.ArrearsFromMonthString.Replace("-", "/") : dto.WithdrawalFromMonthString.Replace("-", "/"),
                       P_DECLARE_DATE = dto.IsTruyThu == 1 ? dto.ArrearsToMonthString.Replace("-", "/") : dto.WithdrawalToMonthString.Replace("-", "/"),
                       P_TRUYTHU = dto.IsTruyThu
                   }, false);
            return new FormatedResponse() { InnerBody = r[0], StatusCode = EnumStatusCode.StatusCode200 };
        }
        public async Task<FormatedResponse> SpsInsArisingManualGet(InsChangeDTO dto)
        {
            if (dto.ChangeMonthString != null)
            {
                var cm = DateTime.Parse(DateTime.Parse(dto.ChangeMonthString + "-02").ToString("yyyy-MM-dd HH:mm:ss.fff"));
                var cd = DateTime.Parse(DateTime.Parse(dto.DeclarationPeriodString + "-06").ToString("yyyy-MM-dd HH:mm:ss.fff"));
                var trFrom = DateTime.Parse(DateTime.Parse(dto.ArrearsFromMonthString + "-03").ToString("yyyy-MM-dd HH:mm:ss.fff"));
                var trTo = DateTime.Parse(DateTime.Parse(dto.ArrearsToMonthString + "-06").ToString("yyyy-MM-dd HH:mm:ss.fff"));
                var r = _dbContext.InsSpecifiedObjectss.AsNoTracking().Where(p => p.EFFECTIVE_DATE <= cm).OrderByDescending(p => p.EFFECTIVE_DATE).FirstOrDefault();
                double trXH = 0;
                double trYT = 0;
                double trTN = 0;
                double trBNN = 0;
                if (trFrom < cm || cm > cd || trFrom > trTo)
                {
                    return new FormatedResponse()
                    {
                        InnerBody = new
                        {
                            TRXH = trXH,
                            TRYT = trYT,
                            TRTN = trTN,
                            TRBNN = trBNN,
                        },
                        StatusCode = EnumStatusCode.StatusCode200
                    };
                }
                if (r != null)
                {
                    TimeSpan m = trTo - trFrom;
                    var mx = (double)Math.Ceiling(m.TotalDays / 30) > 0 ? (double)Math.Ceiling(m.TotalDays / 30) : 1;
                    trXH = (double)(r.SI_EMP! + r.SI_COM!) * (double)(dto.SalaryBhxhBhytNew! ?? 0) * (mx);
                    trYT = (double)(r.HI_EMP! + r.HI_COM!) * (double)(dto.SalaryBhxhBhytNew! ?? 0) * (mx);
                    trTN = (double)(r.UI_EMP! + r.UI_COM!) * (double)(dto.SalaryBhtnNew! ?? 0) * (mx);
                    trBNN = (double)(r.AI_OAI_EMP! + r.AI_OAI_COM!) * (double)(dto.SalaryBhxhBhytNew! ?? 0) * (mx);
                }
                return new FormatedResponse()
                {
                    InnerBody = new
                    {
                        TRXH = trXH,
                        TRYT = trYT,
                        TRTN = trTN,
                        TRBNN = trBNN,
                    },
                    StatusCode = EnumStatusCode.StatusCode200
                };
            }
            return new FormatedResponse()
            {
                InnerBody = new
                {
                    TRXH = 0,
                    TRYT = 0,
                    TRTN = 0,
                    TRBNN = 0,
                },
                StatusCode = EnumStatusCode.StatusCode200
            };
        }
        public async Task<FormatedResponse> SpsInsArisingManualGet2(InsChangeDTO dto)
        {
            if (dto.ChangeMonthString != null)
            {
                var cm = DateTime.Parse(DateTime.Parse(dto.ChangeMonthString + "-02").ToString("yyyy-MM-dd HH:mm:ss.fff"));
                var cd = DateTime.Parse(DateTime.Parse(dto.DeclarationPeriodString + "-06").ToString("yyyy-MM-dd HH:mm:ss.fff"));
                var ttFrom = DateTime.Parse(DateTime.Parse(dto.WithdrawalFromMonthString + "-03").ToString("yyyy-MM-dd HH:mm:ss.fff"));
                var ttTo = DateTime.Parse(DateTime.Parse(dto.WithdrawalToMonthString + "-06").ToString("yyyy-MM-dd HH:mm:ss.fff"));
                var r = _dbContext.InsSpecifiedObjectss.AsNoTracking().Where(p => p.EFFECTIVE_DATE <= cm).OrderByDescending(p => p.EFFECTIVE_DATE).FirstOrDefault();
                double ttXH = 0;
                double ttYT = 0;
                double ttTN = 0;
                double ttBNN = 0;
                if (ttFrom < cm || cm > cd || ttFrom > ttTo)
                {
                    return new FormatedResponse()
                    {
                        InnerBody = new
                        {
                            TTXH = ttXH,
                            TTYT = ttYT,
                            TTTN = ttTN,
                            TTBNN = ttBNN,
                        },
                        StatusCode = EnumStatusCode.StatusCode200
                    };
                }
                if (r != null)
                {
                    TimeSpan m = ttTo - ttFrom;
                    var mx = (double)Math.Ceiling(m.TotalDays / 30) > 0 ? (double)Math.Ceiling(m.TotalDays / 30) : 1;
                    ttXH = (double)(r.SI_EMP! + r.SI_COM!) * (double)(dto.SalaryBhxhBhytOld! ?? 0) * (mx);
                    ttYT = (double)(r.HI_EMP! + r.HI_COM!) * (double)(dto.SalaryBhxhBhytOld! ?? 0) * (mx);
                    ttTN = (double)(r.UI_EMP! + r.UI_COM!) * (double)(dto.SalaryBhtnOld! ?? 0) * (mx);
                    ttBNN = (double)(r.AI_OAI_EMP! + r.AI_OAI_COM!) * (double)(dto.SalaryBhxhBhytOld! ?? 0) * (mx);
                }
                return new FormatedResponse()
                {
                    InnerBody = new
                    {
                        TTXH = ttXH,
                        TTYT = ttYT,
                        TTTN = ttTN,
                        TTBNN = ttBNN,
                    },
                    StatusCode = EnumStatusCode.StatusCode200
                };
            }
            return new FormatedResponse()
            {
                InnerBody = new
                {
                    TTXH = 0,
                    TTYT = 0,
                    TTTN = 0,
                    TTBNN = 0,
                },
                StatusCode = EnumStatusCode.StatusCode200
            };
        }


    }
}

