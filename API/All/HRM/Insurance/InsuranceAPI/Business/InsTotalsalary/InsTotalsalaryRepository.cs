using CORE.GenericUOW;
using CORE.DTO;
using API.DTO;
using CORE.Enum;
using CORE.StaticConstant;
using API.All.DbContexts;
using CORE.AutoMapper;
using System.Drawing;
using Microsoft.IdentityModel.Tokens;
using Azure;
using System.Dynamic;

namespace API.Controllers.InsTotalsalary
{
    public class InsTotalsalaryRepository : IInsTotalsalaryRepository
    {
        private readonly GenericUnitOfWork _uow;
        private readonly FullDbContext _dbContext;
        private IGenericRepository<INS_TOTALSALARY, InsTotalsalaryDTO> _genericRepository;
        private readonly GenericReducer<INS_TOTALSALARY, InsTotalsalaryDTO> _genericReducer;

        public InsTotalsalaryRepository(FullDbContext context, GenericUnitOfWork uow)
        {
            _dbContext = context;
            _uow = uow;
            _genericRepository = _uow.GenericRepository<INS_TOTALSALARY, InsTotalsalaryDTO>();
            _genericReducer = new();
        }

        public async Task<GenericPhaseTwoListResponse<InsTotalsalaryDTO>> SinglePhaseQueryList(GenericQueryListDTO<InsTotalsalaryDTO> request)
        {
            if (request.Filter != null)
            {
                var filterMonth = request.Filter.PeriodId;
                var periodMonth = _dbContext.AtSalaryPeriods.First(p => p.ID == filterMonth).MONTH;
                request.Filter.Month = periodMonth;
                request.Filter.PeriodId = null;
            }
            var joined = from p in _dbContext.InsTotalsalarys.AsNoTracking()
                         from e in _dbContext.HuEmployees.Where(e => e.ID == p.EMPLOYEE_ID).DefaultIfEmpty()
                         from cv in _dbContext.HuEmployeeCvs.Where(cv => cv.ID == e.PROFILE_ID).DefaultIfEmpty()
                         from o in _dbContext.HuOrganizations.Where(o => o.ID == e.ORG_ID).DefaultIfEmpty()
                         from po in _dbContext.HuPositions.Where(po => po.ID == e.POSITION_ID).DefaultIfEmpty()

                             // JOIN OTHER ENTITIES BASED ON THE BUSINESS
                         select new InsTotalsalaryDTO
                         {
                             Id = p.ID,
                             EmployeeCode = e.CODE,
                             Year = p.YEAR,
                             Month = p.MONTH,
                             InsOrgId = p.INS_ORG_ID,
                             ArisingGroupId = p.ARISING_GROUP_ID,
                             PeriodId = p.PERIOD_ID,
                             EmployeeName = cv.FULL_NAME,
                             OrgName = o.NAME,
                             PositionName = po.NAME,
                             BhxhNo = cv.INSURENCE_NUMBER,
                             DeclareDate = p.MONTH == null ? "" : new DateTime(p.YEAR!.Value, p.MONTH!.Value, 1).ToString("MM/yyyy"),
                             SiEmp = p.SI_EMP,
                             HiEmp = p.HI_EMP,
                             UiEmp = p.UI_EMP,
                             BhtnldBnnEmp = p.BHTNLD_BNN_EMP,
                             TotalEmp = p.SI_EMP + p.HI_EMP + p.UI_EMP + p.BHTNLD_BNN_EMP,
                             SiCom = p.SI_COM,
                             HiCom = p.HI_COM,
                             UiCom = p.UI_COM,
                             BhtnldBnnCom = p.BHTNLD_BNN_COM,
                             TotalCom = p.SI_COM + p.HI_COM + p.UI_COM + p.BHTNLD_BNN_COM,
                             SiAdjust = p.SI_ADJUST,
                             HiAdjust = p.HI_ADJUST,
                             UiAdjust = p.UI_ADJUST,
                             BhtnldBnnAdjustEmp = p.BHTNLD_BNN_ADJUST_EMP,
                             SiAdjustCom = p.SI_ADJUST_COM,
                             HiAdjustCom = p.HI_ADJUST_COM,
                             UiAdjustCom = p.UI_ADJUST_COM,
                             BhtnldBnnAdjustCom = p.BHTNLD_BNN_ADJUST_COM,
                             RateSiEmp = p.RATE_SI_EMP * 100,
                             RateHiEmp = p.RATE_HI_EMP * 100,
                             RateUiEmp = p.RATE_UI_EMP * 100,
                             RateBhtnldBnnEmp = p.RATE_BHTNLD_BNN_EMP * 100,
                             RateSiCom = p.RATE_SI_COM * 100,
                             RateHiCom = p.RATE_HI_COM * 100,
                             RateUiCom = p.RATE_UI_COM * 100,
                             RateBhtnldBnnCom = p.RATE_BHTNLD_BNN_COM * 100,
                         };

            var singlePhaseResult = await _genericReducer.SinglePhaseReduce(joined, request);
            return singlePhaseResult;
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
            var res = await _genericRepository.GetById(id);
            if (res.InnerBody != null)
            {
                var response = res.InnerBody;
                var list = new List<INS_TOTALSALARY>
                    {
                        (INS_TOTALSALARY)response
                    };
                var joined = (from l in list
                                  // JOIN OTHER ENTITIES BASED ON THE BUSINESS
                              select new InsTotalsalaryDTO
                              {
                                  Id = l.ID
                              }).FirstOrDefault();

                return new FormatedResponse() { InnerBody = joined };
            }
            else
            {
                return new FormatedResponse() { MessageCode = CommonMessageCode.ENTITY_NOT_FOUND, ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400 };
            }
        }

        public async Task<FormatedResponse> GetById(string id)
        {
            await Task.Run(() => null);
            throw new NotImplementedException();
        }

        public async Task<FormatedResponse> Create(GenericUnitOfWork _uow, InsTotalsalaryDTO dto, string sid)
        {
            var response = await _genericRepository.Create(_uow, dto, sid);
            return response;
        }

        public async Task<FormatedResponse> CreateRange(GenericUnitOfWork _uow, List<InsTotalsalaryDTO> dtos, string sid)
        {
            var add = new List<InsTotalsalaryDTO>();
            add.AddRange(dtos);
            var response = await _genericRepository.CreateRange(_uow, add, sid);
            return response;
        }

        public async Task<FormatedResponse> Update(GenericUnitOfWork _uow, InsTotalsalaryDTO dto, string sid, bool patchMode = true)
        {
            var response = await _genericRepository.Update(_uow, dto, sid, patchMode);
            return response;
        }

        public async Task<FormatedResponse> UpdateRange(GenericUnitOfWork _uow, List<InsTotalsalaryDTO> dtos, string sid, bool patchMode = true)
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
        public async Task<FormatedResponse> Calculate(InsTotalsalaryDTO dto, string sid)
        {
            //var period = await _dbContext.AtSalaryPeriods.AsNoTracking().FirstAsync(p=> p.ID == dto.PeriodId);
            //var declareDate = new DateTime(period.YEAR, period.MONTH!.Value, 1);
            var joined = _dbContext.InsChanges.Where(p => p.UNIT_INSURANCE_TYPE_ID == dto.InsOrgId).ToList();
            var listInsTotal = new List<InsTotalsalaryDTO>();
            var specifiedObjectLastest = _dbContext.InsSpecifiedObjectss.OrderByDescending(p => p.EFFECTIVE_DATE).First(p => p.EFFECTIVE_DATE!.Value.Date <= DateTime.Now.Date);
            if (joined.Count > 0)
            {
                joined.ForEach(x =>
                {
                    var insTotalDto = new InsTotalsalaryDTO();
                    var specifiedObject = (from p in _dbContext.InsSpecifiedObjectss.Where(p => p.EFFECTIVE_DATE!.Value.Date <= x.EFFECTIVE_DATE!.Value.Date).OrderByDescending(p => p.EFFECTIVE_DATE) select p).FirstOrDefault();
                    var region = (from e in _dbContext.HuEmployees.Where(e => e.ID == x.EMPLOYEE_ID).DefaultIfEmpty()
                                  from or in _dbContext.HuOrganizations.Where(or => or.ID == e.ORG_ID).DefaultIfEmpty()
                                  from c in _dbContext.HuCompanys.Where(c => c.ID == or.COMPANY_ID).DefaultIfEmpty()
                                  from o in _dbContext.SysOtherLists.DefaultIfEmpty()
                                  from r in _dbContext.InsRegions.Where(p => p.AREA_ID == o.ID && p.EFFECT_DATE!.Value.Date <= x.EFFECTIVE_DATE!.Value.Date).DefaultIfEmpty()
                                  where o.ID == c.REGION_ID && r.EFFECT_DATE!.Value.Date <= x.EFFECTIVE_DATE!.Value.Date
                                  orderby r.EFFECT_DATE descending
                                  select r).FirstOrDefault();

                    decimal siEmp = 0, hiEmp = 0, uiEmp = 0, bnnEmp = 0;
                    decimal siComp = 0, hiComp = 0, uiComp = 0, bnnComp = 0;
                    long regionMoney = 0;
                    if (region != null) regionMoney = region.CEILING_UI != null ? (long)region.CEILING_UI : 0;


                    insTotalDto.SiEmp = 0;
                    insTotalDto.HiEmp = 0;
                    insTotalDto.UiEmp = 0;
                    insTotalDto.BhtnldBnnEmp = 0;
                    insTotalDto.SiCom = 0;
                    insTotalDto.HiCom = 0;
                    insTotalDto.UiCom = 0;
                    insTotalDto.BhtnldBnnCom = 0;
                    insTotalDto.RateSiEmp = 0;
                    insTotalDto.RateHiEmp = 0;
                    insTotalDto.RateUiEmp = 0;
                    insTotalDto.RateBhtnldBnnEmp = 0;
                    insTotalDto.RateSiCom = 0;
                    insTotalDto.RateHiCom = 0;
                    insTotalDto.RateUiCom = 0;
                    insTotalDto.RateBhtnldBnnCom = 0;
                    insTotalDto.SiAdjust = 0;
                    insTotalDto.HiAdjust = 0;
                    insTotalDto.UiAdjust = 0;
                    insTotalDto.SiAdjustCom = 0;
                    insTotalDto.HiAdjustCom = 0;
                    insTotalDto.UiAdjustCom = 0;
                    insTotalDto.BhtnldBnnAdjustEmp = 0;
                    insTotalDto.BhtnldBnnAdjustCom = 0;
                    insTotalDto.PeriodId = dto.PeriodId;
                    insTotalDto.InsChangeId = x.ID;
                    if (specifiedObject != null)
                    {
                        siEmp = specifiedObject.SI_EMP ?? 0;
                        hiEmp = specifiedObject.HI_EMP ?? 0;
                        uiEmp = specifiedObject.UI_EMP ?? 0;
                        bnnEmp = specifiedObject.AI_OAI_EMP ?? 0;
                        siComp = specifiedObject.SI_COM ?? 0;
                        hiComp = specifiedObject.HI_COM ?? 0;
                        uiComp = specifiedObject.UI_COM ?? 0;
                        bnnComp = specifiedObject.AI_OAI_COM ?? 0;
                        //muc nhan vien dong
                        insTotalDto.SiEmp = (long)((x.SALARY_NEW ?? 0) * (double)siEmp)!; insTotalDto.SiEmp = insTotalDto.SiEmp > specifiedObject!.SI_HI ? (long)specifiedObject.SI_HI : insTotalDto.SiEmp;
                        insTotalDto.HiEmp = (long)((x.SALARY_NEW ?? 0) * (double)hiEmp)!; insTotalDto.HiEmp = insTotalDto.HiEmp > specifiedObject!.SI_HI ? (long)specifiedObject.SI_HI : insTotalDto.HiEmp;
                        insTotalDto.UiEmp = (long)((x.SALARY_NEW ?? 0) * (double)uiEmp)!; insTotalDto.UiEmp = insTotalDto.UiEmp > regionMoney ? regionMoney : insTotalDto.UiEmp;
                        insTotalDto.BhtnldBnnEmp = (long)((x.SALARY_NEW ?? 0) * (double)bnnEmp)!; insTotalDto.BhtnldBnnEmp = insTotalDto.BhtnldBnnEmp > specifiedObject!.AI_OAI_EMP ? (long)specifiedObject.AI_OAI_EMP : insTotalDto.BhtnldBnnEmp;
                        //muc cong ty dong
                        insTotalDto.SiCom = (long)((x.SALARY_NEW ?? 0) * (double)siComp)!; insTotalDto.SiCom = insTotalDto.SiCom > specifiedObject!.SI_HI ? (long)specifiedObject.SI_HI : insTotalDto.SiCom;
                        insTotalDto.HiCom = (long)((x.SALARY_NEW ?? 0) * (double)hiComp)!; insTotalDto.HiCom = insTotalDto.HiCom > specifiedObject!.SI_HI ? (long)specifiedObject.SI_HI : insTotalDto.HiCom;
                        insTotalDto.UiCom = (long)((x.SALARY_NEW ?? 0) * (double)uiComp)!; insTotalDto.UiCom = insTotalDto.UiCom > regionMoney ? regionMoney : insTotalDto.UiCom;
                        insTotalDto.BhtnldBnnCom = (long)((x.SALARY_NEW ?? 0) * (double)bnnComp)!; insTotalDto.BhtnldBnnCom = insTotalDto.BhtnldBnnCom > specifiedObject!.AI_OAI_COM ? (long)specifiedObject.AI_OAI_COM : insTotalDto.BhtnldBnnCom;

                        //tinh dieu chinh nhan vien + cty
                        if ((x.AR_BHXH_SALARY_DIFFERENCE.HasValue) || (x.WD_BHXH_SALARY_DIFFERENCE.HasValue))
                        {
                            insTotalDto.SiAdjust = (long)((x.AR_BHXH_SALARY_DIFFERENCE ?? 0) * siEmp + (x.WD_BHXH_SALARY_DIFFERENCE ?? 0) * siEmp * (-1))!; // dieu chinh bnn nv = (truythu)*(%) + (thoaithu)*(%)(-1)
                            insTotalDto.SiAdjustCom = (long)((x.AR_BHXH_SALARY_DIFFERENCE ?? 0) * siComp + (x.WD_BHXH_SALARY_DIFFERENCE ?? 0) * siComp * (-1))!; // dieu chinh bnn nv = (truythu)*(%) + (thoaithu)*(%)(-1)
                        }
                        if ((x.AR_BHYT_SALARY_DIFFERENCE.HasValue) || (x.WD_BHYT_SALARY_DIFFERENCE.HasValue))
                        {
                            insTotalDto.HiAdjust = (long)((x.AR_BHYT_SALARY_DIFFERENCE ?? 0) * siEmp + (x.WD_BHYT_SALARY_DIFFERENCE ?? 0) * siEmp * (-1))!; // dieu chinh bnn nv = (truythu)*(%) + (thoaithu)*(%)(-1)
                            insTotalDto.HiAdjustCom = (long)((x.AR_BHYT_SALARY_DIFFERENCE ?? 0) * siComp + (x.WD_BHYT_SALARY_DIFFERENCE ?? 0) * siComp * (-1))!; // dieu chinh bnn nv = (truythu)*(%) + (thoaithu)*(%)(-1)
                        }
                        if ((x.AR_BHTN_SALARY_DIFFERENCE.HasValue) || (x.WD_BHTN_SALARY_DIFFERENCE.HasValue))
                        {
                            insTotalDto.UiAdjust = (long)((x.AR_BHTN_SALARY_DIFFERENCE ?? 0) * siEmp + (x.WD_BHTN_SALARY_DIFFERENCE ?? 0) * siEmp * (-1))!; // dieu chinh bnn nv = (truythu)*(%) + (thoaithu)*(%)(-1)
                            insTotalDto.UiAdjustCom = (long)((x.AR_BHTN_SALARY_DIFFERENCE ?? 0) * siComp + (x.WD_BHTN_SALARY_DIFFERENCE ?? 0) * siComp * (-1))!; // dieu chinh bnn nv = (truythu)*(%) + (thoaithu)*(%)(-1)
                        }
                        if ((x.AR_BHTNLD_BNN_SALARY_DIFFERENCE.HasValue) || (x.WD_BHTNLD_BNN_SALARY_DIFFERENCE.HasValue))
                        {
                            insTotalDto.BhtnldBnnAdjustEmp = (long)((x.AR_BHTNLD_BNN_SALARY_DIFFERENCE ?? 0) * siEmp + (x.WD_BHTNLD_BNN_SALARY_DIFFERENCE ?? 0) * siEmp * (-1))!; // dieu chinh bnn nv = (truythu)*(%) + (thoaithu)*(%)(-1)
                            insTotalDto.BhtnldBnnAdjustCom = (long)((x.AR_BHTNLD_BNN_SALARY_DIFFERENCE ?? 0) * siComp + (x.WD_BHTNLD_BNN_SALARY_DIFFERENCE ?? 0) * siComp * (-1))!; // dieu chinh bnn nv = (truythu)*(%) + (thoaithu)*(%)(-1)
                        }
                    }
                    if (specifiedObjectLastest != null)
                    {
                        insTotalDto.RateSiEmp = specifiedObjectLastest.SI_EMP;
                        insTotalDto.RateHiEmp = specifiedObjectLastest.HI_EMP;
                        insTotalDto.RateUiEmp = specifiedObjectLastest.UI_EMP;
                        insTotalDto.RateBhtnldBnnEmp = specifiedObjectLastest.AI_OAI_EMP;
                        insTotalDto.RateSiCom = specifiedObjectLastest.SI_COM;
                        insTotalDto.RateHiCom = specifiedObjectLastest.HI_COM;
                        insTotalDto.RateUiCom = specifiedObjectLastest.UI_COM;
                        insTotalDto.RateBhtnldBnnCom = specifiedObjectLastest.AI_OAI_COM;
                    }
                    if (x.DECLARATION_PERIOD != null)
                    {
                        insTotalDto.Year = x.DECLARATION_PERIOD.Value.Year;
                        insTotalDto.Month = x.DECLARATION_PERIOD.Value.Month;
                    }
                    if (x.CHANGE_TYPE_ID != null)
                    {
                        var arisingType = _dbContext.InsTypes.First(p => p.ID == x.CHANGE_TYPE_ID).TYPE_ID;
                        if (arisingType == 1088) insTotalDto.ArisingGroupId = 1;
                        if (arisingType == 1134) insTotalDto.ArisingGroupId = 2;
                        if (arisingType == 1135) insTotalDto.ArisingGroupId = 3;

                        insTotalDto.InsArisingTypeId = arisingType;
                    }
                    insTotalDto.EmployeeId = x.EMPLOYEE_ID;
                    insTotalDto.InsOrgId = x.UNIT_INSURANCE_TYPE_ID;
                    listInsTotal.Add(insTotalDto);
                });
            }

            var deleteIds = _dbContext.InsTotalsalarys.Where(p => p.INS_ORG_ID == dto.InsOrgId).Select(p => p.ID).ToList();
            if (deleteIds.Count > 0)
            {
                try
                {
                    var delRespone = await _genericRepository.DeleteIds(_uow, deleteIds);
                }
                catch (Exception ex)
                {
                    return new() { InnerBody = null, ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400, MessageCode = CommonMessageCode.CALCULATE_FAILED };
                }
            }
            try
            {
                var createRespone = await _genericRepository.CreateRange(_uow, listInsTotal, sid);
                return new()
                {
                    ErrorType = createRespone.ErrorType,
                    InnerBody = createRespone.InnerBody,
                    StatusCode = createRespone.StatusCode,
                    MessageCode = CommonMessageCode.CALCULATE_SUCCESS
                };
            }
            catch (Exception ex)
            {
                return new() { InnerBody = null, ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400, MessageCode = CommonMessageCode.CALCULATE_FAILED };
            }
        }
        public async Task<FormatedResponse> GetInforByPeriod(InsTotalsalaryDTO dto)
        {
            var period = _dbContext.AtSalaryPeriods.Single(p => p.ID == dto.PeriodId);
            var date = new DateTime(period.YEAR, period.MONTH!.Value, 25);
            var joined = _dbContext.InsTotalsalarys.Where(p => p.INS_ORG_ID == dto.InsOrgId && p.YEAR == period.YEAR && p.MONTH == period.MONTH).ToList();
            var specifield = _dbContext.InsSpecifiedObjectss.OrderByDescending(p => p.EFFECTIVE_DATE).FirstOrDefault(p => p.EFFECTIVE_DATE!.Value.Date <= date.Date);
            if (specifield == null)
            {
                specifield = new INS_SPECIFIED_OBJECTS()
                {
                    SI_EMP = 0,
                    SI_COM = 0,
                    HI_COM = 0,
                    HI_EMP = 0,
                    UI_COM = 0,
                    UI_EMP = 0,
                    AI_OAI_COM = 0,
                    AI_OAI_EMP = 0,
                };

            }
            if (joined.Count > 0)
            {
                var listTang = joined.Where(p => p.INS_ARISING_TYPE_ID == 1088).ToList();
                var listGiam = joined.Where(p => p.INS_ARISING_TYPE_ID == 1134).ToList();
                var listDieuChinh = joined.Where(p => p.INS_ARISING_TYPE_ID == 1135).ToList();

                #region tinh tang
                var listIncreChangeId = listTang.Select(p => p.INS_CHANGE_ID).ToList();
                var listIncreChange = _dbContext.InsChanges.Where(p => listIncreChangeId.Contains(p.ID)).ToList();
                //so lao dong
                var numLdBhxhTang = listIncreChange.Where(p => p.IS_BHXH.HasValue && p.IS_BHXH!.Value == true).ToList();
                var numLdBhytTang = listIncreChange.Where(p => p.IS_BHYT.HasValue && p.IS_BHYT!.Value == true).ToList();
                var numLdBhtnTang = listIncreChange.Where(p => p.IS_BHTN.HasValue && p.IS_BHTN!.Value == true).ToList();
                var numLdBhbnnTang = listIncreChange.Where(p => p.IS_BNN.HasValue && p.IS_BNN!.Value == true).ToList();
                //tong quy luong
                double totalBhxhSalIncre = numLdBhxhTang.Sum(p => p.SALARY_NEW ?? 0);
                double totalBhytSalIncre = numLdBhytTang.Sum(p => p.SALARY_NEW ?? 0);
                double totalBhtnSalIncre = numLdBhtnTang.Sum(p => p.SALARY_NEW ?? 0);
                double totalBhbnnSalIncre = numLdBhbnnTang.Sum(p => p.SALARY_NEW ?? 0);
                //so phai dong
                var mustBeBhxhIncre = totalBhxhSalIncre * (double)(specifield.SI_EMP + specifield.SI_COM)!;
                var mustBeBhythIncre = totalBhytSalIncre * (double)(specifield.HI_EMP + specifield.HI_COM)!;
                var mustBeBhtnIncre = totalBhtnSalIncre * (double)(specifield.UI_EMP + specifield.UI_COM)!;
                var mustBeBhbnnIncre = totalBhbnnSalIncre * (double)(specifield.AI_OAI_EMP + specifield.AI_OAI_COM)!;
                //dieu chinh

                #endregion

                #region tinh giam
                var listReduceChangeId = listGiam.Select(p => p.INS_CHANGE_ID).ToList();
                var listReduceChange = _dbContext.InsChanges.Where(p => listReduceChangeId.Contains(p.ID)).ToList();
                //so lao dong
                var numLdBhxhGiam = listReduceChange.Where(p => p.IS_BHXH.HasValue && p.IS_BHXH!.Value == true).ToList();
                var numLdBhytGiam = listReduceChange.Where(p => p.IS_BHYT.HasValue && p.IS_BHYT!.Value == true).ToList();
                var numLdBhtnGiam = listReduceChange.Where(p => p.IS_BHTN.HasValue && p.IS_BHTN!.Value == true).ToList();
                var numLdBhbnnGiam = listReduceChange.Where(p => p.IS_BNN.HasValue && p.IS_BNN!.Value == true).ToList();
                //tong quy luong
                double totalBhxhSalReduce = numLdBhxhGiam.Sum(p => p.SALARY_NEW ?? 0);
                double totalBhytSalReduce = numLdBhytGiam.Sum(p => p.SALARY_NEW ?? 0);
                double totalBhtnSalReduce = numLdBhtnGiam.Sum(p => p.SALARY_NEW ?? 0);
                double totalBhbnnSalReduce = numLdBhbnnGiam.Sum(p => p.SALARY_NEW ?? 0);
                //so phai dong
                var mustBeBhxhReduce = totalBhxhSalReduce * (double)(specifield.SI_EMP + specifield.SI_COM)!;
                var mustBeBhythReduce = totalBhytSalReduce * (double)(specifield.HI_EMP + specifield.HI_COM)!;
                var mustBeBhtnReduce = totalBhtnSalReduce * (double)(specifield.UI_EMP + specifield.UI_COM)!;
                var mustBeBhbnnReduce = totalBhbnnSalReduce * (double)(specifield.AI_OAI_EMP + specifield.AI_OAI_COM)!;
                //dieu chinh

                #endregion
                var response = new
                {
                    SoLaoDong = new
                    {
                        BhxhTang = numLdBhxhTang.Count,
                        BhytTang = numLdBhytTang.Count,
                        BhtnTang = numLdBhtnTang.Count,
                        BhbnnTang = numLdBhbnnTang.Count,
                        BhxhGiam = numLdBhxhGiam.Count,
                        BhytGiam = numLdBhytGiam.Count,
                        BhtnGiam = numLdBhtnGiam.Count,
                        BhbnnGiam = numLdBhbnnGiam.Count,
                    },
                    TongQuyLuong = new
                    {
                        BhxhTang = totalBhxhSalIncre,
                        BhytTang = totalBhytSalIncre,
                        BhtnTang = totalBhtnSalIncre,
                        BhbnnTang = totalBhbnnSalIncre,
                        BhxhGiam = totalBhxhSalReduce,
                        BhytGiam = totalBhytSalReduce,
                        BhtnGiam = totalBhtnSalReduce,
                        BhbnnGiam = totalBhbnnSalReduce,
                    },
                    SoPhaiDong = new
                    {
                        BhxhTang = mustBeBhxhIncre,
                        BhytTang = mustBeBhythIncre,
                        BhtnTang = mustBeBhtnIncre,
                        BhbnnTang = mustBeBhbnnIncre,
                        BhxhGiam = mustBeBhxhReduce,
                        BhytGiam = mustBeBhythReduce,
                        BhtnGiam = mustBeBhtnReduce,
                        BhbnnGiam = mustBeBhbnnReduce,
                    },
                    DieuChinh = new
                    {
                        BhxhTang = 0,
                        BhytTang = 0,
                        BhtnTang = 0,
                        BhbnnTang = 0,
                        BhxhGiam = 0,
                        BhytGiam = 0,
                        BhtnGiam = 0,
                        BhbnnGiam = 0,
                    },
                };
                return new FormatedResponse()
                {
                    InnerBody = response,
                    StatusCode = EnumStatusCode.StatusCode200,
                };
            }
            var responses = new
            {
                SoLaoDong = new
                {
                    BhxhTang = 0,
                    BhytTang = 0,
                    BhtnTang = 0,
                    BhbnnTang = 0,
                    BhxhGiam = 0,
                    BhytGiam = 0,
                    BhtnGiam = 0,
                    BhbnnGiam = 0,
                },
                TongQuyLuong = new
                {
                    BhxhTang = 0,
                    BhytTang = 0,
                    BhtnTang = 0,
                    BhbnnTang = 0,
                    BhxhGiam = 0,
                    BhytGiam = 0,
                    BhtnGiam = 0,
                    BhbnnGiam = 0,
                },
                SoPhaiDong = new
                {
                    BhxhTang = 0,
                    BhytTang = 0,
                    BhtnTang = 0,
                    BhbnnTang = 0,
                    BhxhGiam = 0,
                    BhytGiam = 0,
                    BhtnGiam = 0,
                    BhbnnGiam = 0,
                },
                DieuChinh = new
                {
                    BhxhTang = 0,
                    BhytTang = 0,
                    BhtnTang = 0,
                    BhbnnTang = 0,
                    BhxhGiam = 0,
                    BhytGiam = 0,
                    BhtnGiam = 0,
                    BhbnnGiam = 0,
                }
            };
            return new FormatedResponse()
            {
                InnerBody = responses,
                StatusCode = EnumStatusCode.StatusCode200,
            };
        }

        public async Task<FormatedResponse> GetInforEndPeriod(InsTotalsalaryDTO dto)
        {
            var period = _dbContext.AtSalaryPeriods.Single(p => p.ID == dto.PeriodId);
            var date = new DateTime(period.YEAR, period.MONTH!.Value, 25);
            var joined = _dbContext.InsTotalsalarys.Where(p => p.INS_ORG_ID == dto.InsOrgId && p.YEAR == period.YEAR && p.MONTH == period.MONTH).ToList();
            var specifield = _dbContext.InsSpecifiedObjectss.OrderByDescending(p => p.EFFECTIVE_DATE).FirstOrDefault(p => p.EFFECTIVE_DATE!.Value.Date <= date.Date);
            if (specifield == null)
            {
                specifield = new INS_SPECIFIED_OBJECTS()
                {
                    SI_EMP = 0,
                    SI_COM = 0,
                    HI_COM = 0,
                    HI_EMP = 0,
                    UI_COM = 0,
                    UI_EMP = 0,
                    AI_OAI_COM = 0,
                    AI_OAI_EMP = 0,
                };

            }
            if (joined.Count > 0)
            {
                var listTang = joined.Where(p => p.INS_ARISING_TYPE_ID == 1088).ToList();
                var listGiam = joined.Where(p => p.INS_ARISING_TYPE_ID == 1134).ToList();
                var listDieuChinh = joined.Where(p => p.INS_ARISING_TYPE_ID == 1135).ToList();

                #region tinh tang
                var listIncreChangeId = listTang.Select(p => p.INS_CHANGE_ID).ToList();
                var listIncreChange = _dbContext.InsChanges.Where(p => listIncreChangeId.Contains(p.ID)).ToList();
                //so lao dong
                var numLdBhxhTang = listIncreChange.Where(p => p.IS_BHXH.HasValue && p.IS_BHXH!.Value == true).ToList();
                var numLdBhytTang = listIncreChange.Where(p => p.IS_BHYT.HasValue && p.IS_BHYT!.Value == true).ToList();
                var numLdBhtnTang = listIncreChange.Where(p => p.IS_BHTN.HasValue && p.IS_BHTN!.Value == true).ToList();
                var numLdBhbnnTang = listIncreChange.Where(p => p.IS_BNN.HasValue && p.IS_BNN!.Value == true).ToList();
                //tong quy luong
                double totalBhxhSalIncre = numLdBhxhTang.Sum(p => p.SALARY_NEW ?? 0);
                double totalBhytSalIncre = numLdBhytTang.Sum(p => p.SALARY_NEW ?? 0);
                double totalBhtnSalIncre = numLdBhtnTang.Sum(p => p.SALARY_NEW ?? 0);
                double totalBhbnnSalIncre = numLdBhbnnTang.Sum(p => p.SALARY_NEW ?? 0);
                //so phai dong
                var mustBeBhxhIncre = totalBhxhSalIncre * (double)(specifield.SI_EMP + specifield.SI_COM)!;
                var mustBeBhythIncre = totalBhytSalIncre * (double)(specifield.HI_EMP + specifield.HI_COM)!;
                var mustBeBhtnIncre = totalBhtnSalIncre * (double)(specifield.UI_EMP + specifield.UI_COM)!;
                var mustBeBhbnnIncre = totalBhbnnSalIncre * (double)(specifield.AI_OAI_EMP + specifield.AI_OAI_COM)!;
                //dieu chinh

                #endregion

                #region tinh giam
                var listReduceChangeId = listGiam.Select(p => p.INS_CHANGE_ID).ToList();
                var listReduceChange = _dbContext.InsChanges.Where(p => listReduceChangeId.Contains(p.ID)).ToList();
                //so lao dong
                var numLdBhxhGiam = listReduceChange.Where(p => p.IS_BHXH.HasValue && p.IS_BHXH!.Value == true).ToList();
                var numLdBhytGiam = listReduceChange.Where(p => p.IS_BHYT.HasValue && p.IS_BHYT!.Value == true).ToList();
                var numLdBhtnGiam = listReduceChange.Where(p => p.IS_BHTN.HasValue && p.IS_BHTN!.Value == true).ToList();
                var numLdBhbnnGiam = listReduceChange.Where(p => p.IS_BNN.HasValue && p.IS_BNN!.Value == true).ToList();
                //tong quy luong
                double totalBhxhSalReduce = numLdBhxhGiam.Sum(p => p.SALARY_NEW ?? 0);
                double totalBhytSalReduce = numLdBhytGiam.Sum(p => p.SALARY_NEW ?? 0);
                double totalBhtnSalReduce = numLdBhtnGiam.Sum(p => p.SALARY_NEW ?? 0);
                double totalBhbnnSalReduce = numLdBhbnnGiam.Sum(p => p.SALARY_NEW ?? 0);
                //so phai dong
                var mustBeBhxhReduce = totalBhxhSalReduce * (double)(specifield.SI_EMP + specifield.SI_COM)!;
                var mustBeBhythReduce = totalBhytSalReduce * (double)(specifield.HI_EMP + specifield.HI_COM)!;
                var mustBeBhtnReduce = totalBhtnSalReduce * (double)(specifield.UI_EMP + specifield.UI_COM)!;
                var mustBeBhbnnReduce = totalBhbnnSalReduce * (double)(specifield.AI_OAI_EMP + specifield.AI_OAI_COM)!;
                //dieu chinh

                #endregion
                var response = new
                {
                    SoLaoDong = new
                    {
                        BhxhTang = numLdBhxhTang.Count,
                        BhytTang = numLdBhytTang.Count,
                        BhtnTang = numLdBhtnTang.Count,
                        BhbnnTang = numLdBhbnnTang.Count,
                        BhxhGiam = numLdBhxhGiam.Count,
                        BhytGiam = numLdBhytGiam.Count,
                        BhtnGiam = numLdBhtnGiam.Count,
                        BhbnnGiam = numLdBhbnnGiam.Count,
                    },
                    TongQuyLuong = new
                    {
                        BhxhTang = totalBhxhSalIncre,
                        BhytTang = totalBhytSalIncre,
                        BhtnTang = totalBhtnSalIncre,
                        BhbnnTang = totalBhbnnSalIncre,
                        BhxhGiam = totalBhxhSalReduce,
                        BhytGiam = totalBhytSalReduce,
                        BhtnGiam = totalBhtnSalReduce,
                        BhbnnGiam = totalBhbnnSalReduce,
                    },
                    SoPhaiDong = new
                    {
                        BhxhTang = mustBeBhxhIncre,
                        BhytTang = mustBeBhythIncre,
                        BhtnTang = mustBeBhtnIncre,
                        BhbnnTang = mustBeBhbnnIncre,
                        BhxhGiam = mustBeBhxhReduce,
                        BhytGiam = mustBeBhythReduce,
                        BhtnGiam = mustBeBhtnReduce,
                        BhbnnGiam = mustBeBhbnnReduce,
                    },
                    DieuChinh = new
                    {
                        BhxhTang = 0,
                        BhytTang = 0,
                        BhtnTang = 0,
                        BhbnnTang = 0,
                        BhxhGiam = 0,
                        BhytGiam = 0,
                        BhtnGiam = 0,
                        BhbnnGiam = 0,
                    },
                };
                return new FormatedResponse()
                {
                    InnerBody = response,
                    StatusCode = EnumStatusCode.StatusCode200,
                };
            }
            var responses = new
            {
                SoLaoDong = new
                {
                    BhxhTang = 0,
                    BhytTang = 0,
                    BhtnTang = 0,
                    BhbnnTang = 0,
                    BhxhGiam = 0,
                    BhytGiam = 0,
                    BhtnGiam = 0,
                    BhbnnGiam = 0,
                },
                TongQuyLuong = new
                {
                    BhxhTang = 0,
                    BhytTang = 0,
                    BhtnTang = 0,
                    BhbnnTang = 0,
                    BhxhGiam = 0,
                    BhytGiam = 0,
                    BhtnGiam = 0,
                    BhbnnGiam = 0,
                },
                SoPhaiDong = new
                {
                    BhxhTang = 0,
                    BhytTang = 0,
                    BhtnTang = 0,
                    BhbnnTang = 0,
                    BhxhGiam = 0,
                    BhytGiam = 0,
                    BhtnGiam = 0,
                    BhbnnGiam = 0,
                },
                DieuChinh = new
                {
                    BhxhTang = 0,
                    BhytTang = 0,
                    BhtnTang = 0,
                    BhbnnTang = 0,
                    BhxhGiam = 0,
                    BhytGiam = 0,
                    BhtnGiam = 0,
                    BhbnnGiam = 0,
                }
            };
            return new FormatedResponse()
            {
                InnerBody = responses,
                StatusCode = EnumStatusCode.StatusCode200,
            };
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
    }
}

