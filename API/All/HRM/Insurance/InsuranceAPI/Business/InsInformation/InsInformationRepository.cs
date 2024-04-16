using CORE.GenericUOW;
using CORE.DTO;
using API.DTO;
using CORE.Enum;
using CORE.StaticConstant;
using API.All.DbContexts;
using System.Globalization;
using Common.Extensions;
using System.Linq.Dynamic.Core;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers.InsInformation
{
    public class InsInformationRepository : IInsInformationRepository
    {
        private readonly GenericUnitOfWork _uow;
        private readonly FullDbContext _dbContext;

        private IGenericRepository<INS_INFORMATION, InsInformationDTO> _genericRepository;
        private readonly GenericReducer<INS_INFORMATION, InsInformationDTO> _genericReducer;

        public InsInformationRepository(FullDbContext context, GenericUnitOfWork uow)
        {
            _dbContext = context;
            _uow = uow;
            _genericRepository = _uow.GenericRepository<INS_INFORMATION, InsInformationDTO>();
            _genericReducer = new();
        }

        public async Task<GenericPhaseTwoListResponse<InsInformationDTO>> SinglePhaseQueryList(GenericQueryListDTO<InsInformationDTO> request)
        {
            var sysContractType = _dbContext.SysContractTypes.AsNoTracking().Where(p => p.CODE == "HDCNSDLD" || p.CODE == "HDLD001" || p.CODE == "HDLD003" || p.CODE == "HDLD002" || p.CODE == "HDLD004" || p.CODE == "HDCT").Select(p => p.ID).ToList();
            var contractType = _dbContext.HuContractTypes.AsNoTracking().Where(p => sysContractType.Contains((long)p.TYPE_ID!)).Select(p => p.ID).ToList();

            var joined = (from l in _dbContext.InsInformations.AsNoTracking()
                          from e in _dbContext.HuEmployees.AsNoTracking().Where(emp => emp.ID == l.EMPLOYEE_ID).DefaultIfEmpty()
                          from c in _dbContext.HuContracts.AsNoTracking().Where(cn => cn.ID == e.CONTRACT_ID && cn.STATUS_ID == OtherConfig.STATUS_APPROVE && contractType.Contains((long)cn.CONTRACT_TYPE_ID!)).OrderBy(ic => ic.START_DATE).Take(1).DefaultIfEmpty()
                          from ct in _dbContext.HuContractTypes.AsNoTracking().Where(ct => ct.ID == c.CONTRACT_TYPE_ID).DefaultIfEmpty()
                          from wk in _dbContext.HuWorkings.AsNoTracking().Where(p => p.ID == c.WORKING_ID).DefaultIfEmpty()
                          from tl in _dbContext.InsSpecifiedObjectss.AsNoTracking().Where(p => p.EFFECTIVE_DATE <= c.START_DATE).OrderByDescending(cn => cn.EFFECTIVE_DATE).Take(1).DefaultIfEmpty()
                          from cv in _dbContext.HuEmployeeCvs.AsNoTracking().Where(cv => cv.ID == e.PROFILE_ID).DefaultIfEmpty()
                          from pr in _dbContext.HuProvinces.AsNoTracking().Where(c => c.ID == cv.ID_PLACE).DefaultIfEmpty()
                          from t in _dbContext.HuPositions.AsNoTracking().Where(x => x.ID == e.POSITION_ID).DefaultIfEmpty()
                          from j in _dbContext.HuJobs.AsNoTracking().Where(x => x.ID == t.JOB_ID).DefaultIfEmpty()
                          from o in _dbContext.HuOrganizations.AsNoTracking().Where(x => x.ID == e.ORG_ID).DefaultIfEmpty()
                          from cmp in _dbContext.HuCompanys.AsNoTracking().Where(cmp => cmp.ID == o.COMPANY_ID).DefaultIfEmpty()
                          from dv in _dbContext.SysOtherLists.AsNoTracking().Where(c => c.ID == cmp.INS_UNIT).DefaultIfEmpty()

                              // JOIN OTHER ENTITIES BASED ON THE BUSINESS
                          select new InsInformationDTO
                          {
                              Id = l.ID,
                              EmployeeCode = e.CODE,
                              EmployeeId = l.EMPLOYEE_ID,
                              JobOrderNum = (int)(j.ORDERNUM ?? 999),
                              EmployeeName = cv.FULL_NAME,
                              PositionName = t.NAME,
                              OrgName = o.NAME,
                              OrgId = e.ORG_ID,
                              IdNo = cv.ID_NO,
                              IdDate = cv.ID_DATE,
                              AddressIdentity = pr.NAME,
                              BirthDate = cv.BIRTH_DATE,
                              BirthPlace = cv.BIRTH_PLACE,
                              Contact = cv.MOBILE_PHONE,//
                              SeniorityInsurance = l.SENIORITY_INSURANCE,
                              SeniorityInsuranceString = l.SENIORITY_INSURANCE.ToString() == null ? "0 tháng" : l.SENIORITY_INSURANCE.ToString() + " tháng",
                              SeniorityInsuranceInCompany = e.JOIN_DATE_STATE!.Value == null ? 0 : (int)Math.Round((decimal)((DateTime.Now - e.JOIN_DATE_STATE!.Value).Days / 30)),
                              SeniorityInsuranceInCompanyString = e.JOIN_DATE_STATE!.Value == null ? "0 tháng" : ((int)Math.Round((decimal)((DateTime.Now - e.JOIN_DATE_STATE!.Value).Days / 30))).ToString() + " tháng",
                              Company = dv.NAME,
                              IsBhxh = ct.IS_BHXH,
                              IsBhtn = ct.IS_BHTN,
                              IsBhtnldBnn = ct.IS_BHTNLD_BNN,
                              IsBhyt = ct.IS_BHYT,
                              BhxhNo = cv.INSURENCE_NUMBER,
                              SalaryBhTn = wk.SAL_INSU == null ? 0 : (tl.UI == null ? 0 : (tl.UI > wk.SAL_INSU ? (double)wk.SAL_INSU : (double)tl.UI)),
                              SalaryBhxhYt = wk.SAL_INSU == null ? 0 : (tl.UI == null ? 0 : (tl.SI_HI > wk.SAL_INSU ? (double)wk.SAL_INSU : (double)tl.SI_HI)),
                              BhxhFromDate = l.BHXH_FROM_DATE,
                              BhxhToDate = l.BHXH_TO_DATE,
                              BhxhStatusId = l.BHXH_STATUS_ID,
                              BhxhGrantDate = l.BHXH_GRANT_DATE,
                              BhxhDeliverer = l.BHXH_DELIVERER,
                              BhtnFromDate = l.BHTN_FROM_DATE,
                              BhtnldBnnFromDate = l.BHTNLD_BNN_FROM_DATE,
                              BhtnldBnnToDate = l.BHTNLD_BNN_TO_DATE,
                              BhtnToDate = l.BHTN_TO_DATE,
                              BhxhReceiver = l.BHXH_RECEIVER,
                              BhxhStorageNumber = l.BHXH_STORAGE_NUMBER,
                              BhxhReimbursementDate = l.BHXH_REIMBURSEMENT_DATE,
                              BhxhSuppliedDate = l.BHXH_SUPPLIED_DATE,
                              BhytEffectDate = l.BHYT_EFFECT_DATE,
                              BhxhNote = l.BHXH_NOTE,
                              BhytExpireDate = l.BHYT_EXPIRE_DATE,
                              BhytFromDate = l.BHYT_FROM_DATE,
                              BhytNo = cv.INS_CARD_NUMBER,
                              BhytReceivedDate = l.BHYT_RECEIVED_DATE,
                              BhytReceiver = l.BHYT_RECEIVER,
                              BhytReimbursementDate = l.BHYT_REIMBURSEMENT_DATE,
                              BhytStatusId = l.BHYT_STATUS_ID,
                              BhytToDate = l.BHYT_TO_DATE,
                              BhytWherehealthId = l.BHYT_WHEREHEALTH_ID,
                              BhtnldBnnToDateString = l.BHTNLD_BNN_TO_DATE.Value == null ? null : l.BHTNLD_BNN_TO_DATE.Value.ToString("yyyy-MM"),
                              BhtnldBnnFromDateString = l.BHTNLD_BNN_FROM_DATE.Value == null ? null : l.BHTNLD_BNN_FROM_DATE.Value.ToString("yyyy-MM"),
                              BhtnToDateString = l.BHTN_TO_DATE.Value == null ? null : l.BHTN_TO_DATE.Value.ToString("yyyy-MM"),
                              BhtnFromDateString = l.BHTN_FROM_DATE.Value == null ? null : l.BHTN_FROM_DATE.Value.ToString("yyyy-MM"),
                              BhxhFromDateString = l.BHXH_FROM_DATE.Value == null ? null : l.BHXH_FROM_DATE.Value.ToString("yyyy-MM"),
                              BhxhToDateString = l.BHXH_TO_DATE.Value == null ? null : l.BHXH_TO_DATE.Value.ToString("yyyy-MM"),
                              BhytFromDateString = l.BHYT_FROM_DATE.Value == null ? null : l.BHYT_FROM_DATE.Value.ToString("yyyy-MM"),
                              RegionId = cmp.REGION_ID,
                          }).ToList();
            joined.ForEach(x =>
            {
                decimal? siSal = 0;
                decimal? uiSal = 0;
                var salary = _dbContext.InsChanges.AsNoTracking().Where(p => p.EMPLOYEE_ID == x.EmployeeId && p.EFFECTIVE_DATE!.Value.Date <= DateTime.Now.Date).OrderByDescending(p => p.EFFECTIVE_DATE).FirstOrDefault();
                if(salary != null)
                {
                    uiSal = salary.SALARY_BHTN_NEW;
                    siSal = salary.SALARY_BHXH_BHYT_NEW;
                }

                x.SalaryBhxhYt = (double)siSal;
                x.SalaryBhTn = (double)uiSal!;
            });
            var result = joined.AsQueryable();
            var singlePhaseResult = await _genericReducer.SinglePhaseReduce(result, request);
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
            try
            {
                List<int> lstcheckInsItems = new List<int>();
                TimeSpan time;
                var joined = await (from l in _dbContext.InsInformations.AsNoTracking().Where(l => l.ID == id)
                                    from w in _dbContext.InsWhereHealThs.AsNoTracking().Where(w => l.BHYT_WHEREHEALTH_ID == null ? false : w.ID == l.BHYT_WHEREHEALTH_ID).DefaultIfEmpty()
                                    from e in _dbContext.HuEmployees.AsNoTracking().Where(emp => emp.ID == l.EMPLOYEE_ID).DefaultIfEmpty()
                                    from cn in _dbContext.HuContracts.AsNoTracking().Where(cn => cn.EMPLOYEE_ID == e.ID && cn.STATUS_ID == OtherConfig.STATUS_APPROVE).OrderBy(cn => cn.START_DATE).Take(1).DefaultIfEmpty()
                                    from ct in _dbContext.HuContractTypes.AsNoTracking().Where(ct => ct.ID == cn.CONTRACT_TYPE_ID).DefaultIfEmpty()
                                    from wk in _dbContext.HuWorkings.AsNoTracking().Where(p => p.ID == cn.WORKING_ID).DefaultIfEmpty()
                                    from cv in _dbContext.HuEmployeeCvs.AsNoTracking().Where(cv => cv.ID == e.PROFILE_ID).DefaultIfEmpty()
                                    from tl in _dbContext.InsSpecifiedObjectss.AsNoTracking().Where(p => p.EFFECTIVE_DATE <= cn.START_DATE).OrderByDescending(cn => cn.EFFECTIVE_DATE).Take(1).DefaultIfEmpty()
                                    from pr in _dbContext.HuProvinces.AsNoTracking().Where(c => c.ID == cv.ID_PLACE).DefaultIfEmpty()
                                    from t in _dbContext.HuPositions.AsNoTracking().Where(x => x.ID == e.POSITION_ID).DefaultIfEmpty()
                                    from o in _dbContext.HuOrganizations.AsNoTracking().Where(x => x.ID == e.ORG_ID).DefaultIfEmpty()
                                    from cmp in _dbContext.HuCompanys.AsNoTracking().Where(cmp => cmp.ID == o.COMPANY_ID).DefaultIfEmpty()
                                    from dv in _dbContext.SysOtherLists.AsNoTracking().Where(c => c.ID == cmp.INS_UNIT).DefaultIfEmpty()
                                    from re in _dbContext.SysOtherLists.AsNoTracking().Where(x => x.ID == cmp.REGION_ID).DefaultIfEmpty()
                                    from insr in _dbContext.InsRegions.AsNoTracking().Where(insr => insr.OTHER_LIST_CODE == re.CODE).DefaultIfEmpty()
                                    from c in _dbContext.SysUsers.AsNoTracking().Where(c => l.CREATED_BY == null ? false : c.ID == l.CREATED_BY).DefaultIfEmpty()
                                    from u in _dbContext.SysUsers.AsNoTracking().Where(u => l.UPDATED_BY == null ? false : u.ID == l.UPDATED_BY).DefaultIfEmpty()
                                        // JOIN OTHER ENTITIES BASED ON THE BUSINESS
                                    select new InsInformationDTO
                                    {
                                        Id = l.ID,
                                        EmployeeCode = e.CODE,
                                        EmployeeId = l.EMPLOYEE_ID,
                                        EmployeeName = cv.FULL_NAME,
                                        PositionName = t.NAME,
                                        OrgName = o.NAME,
                                        IdNo = cv.ID_NO,
                                        IdDate = cv.ID_DATE,
                                        AddressIdentity = pr.NAME,
                                        BirthDate = cv.BIRTH_DATE,
                                        BirthPlace = cv.BIRTH_PLACE,
                                        Contact = cv.MOBILE_PHONE,
                                        SeniorityInsurance = l.SENIORITY_INSURANCE,
                                        SeniorityInsuranceInCompany = e!.JOIN_DATE_STATE!.Value! == null ? 0 : (int)Math.Round((decimal)((DateTime.Now - e!.JOIN_DATE_STATE!.Value).Days / 30)),
                                        Company = dv.NAME,
                                        BhxhNo = cv.INSURENCE_NUMBER,
                                        BhxhFromDate = l.BHXH_FROM_DATE,
                                        BhxhToDate = l.BHXH_TO_DATE,
                                        BhxhStatusId = l.BHXH_STATUS_ID,
                                        BhxhStatusString = insr.OTHER_LIST_CODE,
                                        BhxhGrantDate = l.BHXH_GRANT_DATE,
                                        BhxhDeliverer = l.BHXH_DELIVERER,
                                        BhtnFromDate = l.BHTN_FROM_DATE,
                                        BhtnldBnnFromDate = l.BHTNLD_BNN_FROM_DATE,
                                        BhtnldBnnToDate = l.BHTNLD_BNN_TO_DATE,
                                        BhtnToDate = l.BHTN_TO_DATE,
                                        BhxhReceiver = l.BHXH_RECEIVER,
                                        BhxhStorageNumber = l.BHXH_STORAGE_NUMBER,
                                        BhxhReimbursementDate = l.BHXH_REIMBURSEMENT_DATE,
                                        BhxhSuppliedDate = l.BHXH_SUPPLIED_DATE,
                                        BhytEffectDate = l.BHYT_EFFECT_DATE,
                                        BhxhNote = l.BHXH_NOTE,
                                        BhytExpireDate = l.BHYT_EXPIRE_DATE,
                                        BhytWherehealthString = w.NAME_VN,
                                        BhytFromDate = l.BHYT_FROM_DATE,
                                        BhytNo = cv.INS_CARD_NUMBER,
                                        BhytReceivedDate = l.BHYT_RECEIVED_DATE,
                                        BhytReceiver = l.BHYT_RECEIVER,
                                        BhytReimbursementDate = l.BHYT_REIMBURSEMENT_DATE,
                                        BhytStatusId = l.BHYT_STATUS_ID,
                                        BhytToDate = l.BHYT_TO_DATE,
                                        BhytWherehealthId = l.BHYT_WHEREHEALTH_ID,
                                        IsBhxh = ct.IS_BHXH.HasValue,
                                        IsBhtn = ct.IS_BHTN.HasValue,
                                        IsBhtnldBnn = ct.IS_BHTNLD_BNN.HasValue,
                                        IsBhyt = ct.IS_BHYT.HasValue,
                                        CreatedByUsername = c.USERNAME,
                                        CreatedDate = l.CREATED_DATE,
                                        UpdatedByUsername = u.USERNAME,
                                        UpdatedDate = l.UPDATED_DATE,
                                        BhtnldBnnToDateString = l!.BHTNLD_BNN_TO_DATE!.Value == null ? null : l!.BHTNLD_BNN_TO_DATE!.Value.ToString("yyyy-MM"),
                                        BhtnldBnnFromDateString = l!.BHTNLD_BNN_FROM_DATE!.Value == null ? null : l.BHTNLD_BNN_FROM_DATE.Value.ToString("yyyy-MM"),
                                        BhtnToDateString = l!.BHTN_TO_DATE!.Value == null ? null : l.BHTN_TO_DATE.Value.ToString("yyyy-MM"),
                                        BhtnFromDateString = l.BHTN_FROM_DATE!.Value == null ? null : l.BHTN_FROM_DATE.Value.ToString("yyyy-MM"),
                                        BhxhFromDateString = l.BHXH_FROM_DATE!.Value == null ? null : l.BHXH_FROM_DATE.Value.ToString("yyyy-MM"),
                                        BhxhToDateString = l.BHXH_TO_DATE.Value! == null ? null : l.BHXH_TO_DATE.Value.ToString("yyyy-MM"),
                                        BhytFromDateString = l.BHYT_FROM_DATE!.Value == null ? null : l.BHYT_FROM_DATE.Value.ToString("yyyy-MM"),
                                        BhytToDateString = l.BHYT_TO_DATE!.Value == null ? null : l.BHYT_TO_DATE.Value.ToString("yyyy-MM"),
                                    }).FirstOrDefaultAsync();

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
                    if (joined.IsBhtnldBnn.HasValue != null && joined.IsBhtnldBnn == true)
                    {
                        lstcheckInsItems.Add(4);
                    }

                    joined.LstCheckInsItems = new List<int>();

                    joined.LstCheckInsItems.AddRange(lstcheckInsItems.Distinct().ToList());
                    return new FormatedResponse() { InnerBody = joined };
                }
                else
                {
                    return new FormatedResponse() { MessageCode = CommonMessageCode.ENTITY_NOT_FOUND, ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400 };
                }
            }
            catch (Exception ex)
            {
                return new FormatedResponse() { MessageCode = ex.ToString(), ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode500 };
            }

        }

        public async Task<FormatedResponse> GetById(string id)
        {
            await Task.Run(() => null);
            throw new NotImplementedException();
        }

        public async Task<FormatedResponse> Create(GenericUnitOfWork _uow, InsInformationDTO dto, string sid)
        {
            try
            {
                var c1 = await _dbContext.InsInformations.Where(x => x.ID != dto.Id && x.EMPLOYEE_ID == dto.EmployeeId).CountAsync();
                if (c1 > 0)
                {
                    return new FormatedResponse() { MessageCode = CommonMessageCode.EMPLOYEE_IS_EXIST_RECORD, ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400 };
                }
                if (await _dbContext.InsInformations.Where(x => x.EMPLOYEE_ID == dto.EmployeeId).CountAsync() == 0)
                {
                    if (dto.BhxhFromDateString != null && dto.BhxhFromDateString != "")
                    {
                        dto.BhxhFromDate = DateTime.Parse(DateTime.Parse(dto.BhxhFromDateString).ToString("yyyy-MM-dd HH:mm:ss.fff"));
                    }
                    if (dto.BhxhToDateString != null && dto.BhxhToDateString != "")
                    {
                        dto.BhxhToDate = DateTime.Parse(DateTime.Parse(dto.BhxhToDateString).ToString("yyyy-MM-dd HH:mm:ss.fff"));
                    }
                    if (dto.BhytFromDateString != null && dto.BhytFromDateString != "")
                    {
                        dto.BhytFromDate = DateTime.Parse(DateTime.Parse(dto.BhytFromDateString).ToString("yyyy-MM-dd HH:mm:ss.fff"));
                    }
                    if (dto.BhytToDateString != null && dto.BhytToDateString != "")
                    {
                        dto.BhytToDate = DateTime.Parse(DateTime.Parse(dto.BhytToDateString).ToString("yyyy-MM-dd HH:mm:ss.fff"));
                    }
                    if (dto.BhtnFromDateString != null && dto.BhtnFromDateString != "")
                    {
                        dto.BhtnFromDate = DateTime.Parse(DateTime.Parse(dto.BhtnFromDateString).ToString("yyyy-MM-dd HH:mm:ss.fff"));
                    }
                    if (dto.BhtnToDateString != null && dto.BhtnToDateString != "")
                    {
                        dto.BhtnToDate = DateTime.Parse(DateTime.Parse(dto.BhtnToDateString).ToString("yyyy-MM-dd HH:mm:ss.fff"));
                    }
                    if (dto.BhtnldBnnFromDateString != null && dto.BhtnldBnnFromDateString != "")
                    {
                        dto.BhtnldBnnFromDate = DateTime.Parse(DateTime.Parse(dto.BhtnldBnnFromDateString).ToString("yyyy-MM-dd HH:mm:ss.fff"));
                    }
                    if (dto.BhtnldBnnToDateString != null && dto.BhtnldBnnToDateString != "")
                    {
                        dto.BhtnldBnnToDate = DateTime.Parse(DateTime.Parse(dto.BhtnldBnnToDateString).ToString("yyyy-MM-dd HH:mm:ss.fff"));
                    }
                    try
                    {
                        var response = await _genericRepository.Create(_uow, dto, sid);
                        return new FormatedResponse() { MessageCode = CommonMessageCode.CREATE_SUCCESS, InnerBody = dto, StatusCode = EnumStatusCode.StatusCode200 };

                    }
                    catch (Exception ex)
                    {
                        return new FormatedResponse() { MessageCode = ex.ToString(), ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400 };

                    }
                }
                else
                {
                    return new FormatedResponse() { MessageCode = CommonMessageCode.CREATE_OBJECT_HAS_SAME_CODE, StatusCode = EnumStatusCode.StatusCode400 };
                }
            }
            catch (Exception ex)
            {
                return new FormatedResponse() { MessageCode = ex.ToString(), ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400 };
            }
        }

        public async Task<FormatedResponse> CreateRange(GenericUnitOfWork _uow, List<InsInformationDTO> dtos, string sid)
        {
            var add = new List<InsInformationDTO>();
            add.AddRange(dtos);
            var response = await _genericRepository.CreateRange(_uow, add, sid);
            return response;
        }

        public async Task<FormatedResponse> Update(GenericUnitOfWork _uow, InsInformationDTO dto, string sid, bool patchMode = true)
        {
            try
            {
                var c1 = await _dbContext.InsInformations.Where(x => x.ID != dto.Id && x.EMPLOYEE_ID == dto.EmployeeId).CountAsync();
                if (c1 > 0)
                {
                    return new FormatedResponse() { MessageCode = CommonMessageCode.EMPLOYEE_IS_EXIST_RECORD, ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400 };
                }
                var c = await _dbContext.InsInformations.Where(x => x.ID == dto.Id && x.EMPLOYEE_ID == dto.EmployeeId).CountAsync();
                if (c > 0)
                {
                    if (dto.BhxhFromDateString != null && dto.BhxhFromDateString != "")
                    {
                        dto.BhxhFromDate = DateTime.Parse(DateTime.Parse(dto.BhxhFromDateString).ToString("yyyy-MM-dd HH:mm:ss.fff"));
                    }
                    if (dto.BhxhToDateString != null && dto.BhxhToDateString != "")
                    {
                        dto.BhxhToDate = DateTime.Parse(DateTime.Parse(dto.BhxhToDateString).ToString("yyyy-MM-dd HH:mm:ss.fff"));
                    }
                    if (dto.BhytFromDateString != null && dto.BhytFromDateString != "")
                    {
                        dto.BhytFromDate = DateTime.Parse(DateTime.Parse(dto.BhytFromDateString).ToString("yyyy-MM-dd HH:mm:ss.fff"));
                    }
                    if (dto.BhytToDateString != null && dto.BhytToDateString != "")
                    {
                        dto.BhytToDate = DateTime.Parse(DateTime.Parse(dto.BhytToDateString).ToString("yyyy-MM-dd HH:mm:ss.fff"));
                    }
                    if (dto.BhtnFromDateString != null && dto.BhtnFromDateString != "")
                    {
                        dto.BhtnFromDate = DateTime.Parse(DateTime.Parse(dto.BhtnFromDateString).ToString("yyyy-MM-dd HH:mm:ss.fff"));
                    }
                    if (dto.BhtnToDateString != null && dto.BhtnToDateString != "")
                    {
                        dto.BhtnToDate = DateTime.Parse(DateTime.Parse(dto.BhtnToDateString).ToString("yyyy-MM-dd HH:mm:ss.fff"));
                    }
                    if (dto.BhtnldBnnFromDateString != null && dto.BhtnldBnnFromDateString != "")
                    {
                        dto.BhtnldBnnFromDate = DateTime.Parse(DateTime.Parse(dto.BhtnldBnnFromDateString).ToString("yyyy-MM-dd HH:mm:ss.fff"));
                    }
                    if (dto.BhtnldBnnToDateString != null && dto.BhtnldBnnToDateString != "")
                    {
                        dto.BhtnldBnnToDate = DateTime.Parse(DateTime.Parse(dto.BhtnldBnnToDateString).ToString("yyyy-MM-dd HH:mm:ss.fff"));
                    }
                    try
                    {
                        var response = await _genericRepository.Update(_uow, dto, sid, patchMode);
                        return new FormatedResponse() { MessageCode = CommonMessageCode.UPDATE_SUCCESS, InnerBody = dto, StatusCode = EnumStatusCode.StatusCode200 };

                    }
                    catch (Exception ex)
                    {
                        return new FormatedResponse() { MessageCode = ex.ToString(), ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode500 };

                    }
                }
                return new FormatedResponse() { MessageCode = CommonMessageCode.ENTITY_NOT_FOUND, ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400 };
            }
            catch (Exception ex)
            {
                return new FormatedResponse() { MessageCode = ex.ToString(), ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode500 };
            }
        }


        public async Task<FormatedResponse> UpdateRange(GenericUnitOfWork _uow, List<InsInformationDTO> dtos, string sid, bool patchMode = true)
        {
            try
            {
                var response = await _genericRepository.UpdateRange(_uow, dtos, sid, patchMode);
                return response;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return null;
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
        public async Task<FormatedResponse> GetInforById(long id)
        {
            var empT = new InsInformationDTO();
            var emp = _dbContext.HuEmployees.AsNoTracking().Where(p => p.ID == id).FirstOrDefault();

            var sysContractType = await _dbContext.SysContractTypes.AsNoTracking().Where(p => p.CODE == "HDCNSDLD" || p.CODE == "HDLD001" || p.CODE == "HDLD003" || p.CODE == "HDLD002" || p.CODE == "HDLD004" || p.CODE == "HDCT").Select(p => p.ID).ToListAsync();
            var contract = (from ct in _dbContext.HuContracts.AsNoTracking().Where(p => p.EMPLOYEE_ID == id && p.STATUS_ID == OtherConfig.STATUS_APPROVE).DefaultIfEmpty()
                            from ctt in _dbContext.HuContractTypes.AsNoTracking().Where(p => p.ID == ct.CONTRACT_TYPE_ID && sysContractType.Contains(p.TYPE_ID!.Value)).DefaultIfEmpty()
                            select new
                            {
                                Id = ct.WORKING_ID,
                                StartDate = ct.START_DATE,
                            }).OrderBy(p => p.StartDate).FirstOrDefault();
            decimal? siSal = 0;
            var salary = _dbContext.InsChanges.AsNoTracking().Where(p => p.EMPLOYEE_ID == id && p.EFFECTIVE_DATE!.Value.Date <= DateTime.Now.Date).OrderByDescending(p => p.EFFECTIVE_DATE).FirstOrDefault();
            if (salary != null)
            {
                siSal = salary.SALARY_BHXH_BHYT_NEW;
            }
            var joined = await (from e in _dbContext.HuEmployees.AsNoTracking().Where(emp => emp.ID == id)
                                from cv in _dbContext.HuEmployeeCvs.AsNoTracking().Where(cv => cv.ID == e.PROFILE_ID).DefaultIfEmpty()
                                from o in _dbContext.HuOrganizations.AsNoTracking().Where(x => x.ID == e.ORG_ID).DefaultIfEmpty()
                                from cmp in _dbContext.HuCompanys.AsNoTracking().Where(cmp => cmp.ID == o.COMPANY_ID).DefaultIfEmpty()
                                from dv in _dbContext.SysOtherLists.AsNoTracking().Where(c => c.ID == cmp.INS_UNIT).DefaultIfEmpty()
                                    // JOIN OTHER ENTITIES BASED ON THE BUSINESS
                                select new
                                {
                                    EmployeeId = id,
                                    EmployeeCode = e.CODE,
                                    BhytNo = cv.INS_CARD_NUMBER,
                                    BhxhNo = cv.INSURENCE_NUMBER,
                                    SeniorityInsuranceInCompany = e.JOIN_DATE_STATE == null ? 0 : (int)Math.Round((decimal)((DateTime.Now - e.JOIN_DATE_STATE!.Value).Days / 30)),
                                    SalaryBhxhYt = siSal,
                                    Company = dv.NAME,
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
        public async Task<FormatedResponse> GetBhxhStatus()
        {
            var query = await (from o in _dbContext.SysOtherLists.AsNoTracking().Where(o => o.TYPE_ID == 80)
                               orderby o.ID
                               select new
                               {
                                   Id = o.ID,
                                   Name = o.NAME,
                                   Code = o.CODE,
                               }).ToListAsync();
            return new FormatedResponse() { InnerBody = query };
        }
        public async Task<FormatedResponse> GetBhYtStatus()
        {
            var query = await (from o in _dbContext.SysOtherLists.AsNoTracking().Where(o => o.TYPE_ID == 81)
                               orderby o.ID
                               select new
                               {
                                   Id = o.ID,
                                   Name = o.NAME,
                                   Code = o.CODE,
                               }).ToListAsync();
            return new FormatedResponse() { InnerBody = query };
        }
        public async Task<FormatedResponse> GetInsWhereHealth()
        {
            var query = await (from o in _dbContext.InsWhereHealThs.AsNoTracking().Where(o => o.IS_ACTIVE == true)
                               orderby o.ID
                               select new
                               {
                                   Id = o.ID,
                                   Name = o.NAME_VN,
                                   Code = o.CODE,
                               }).ToListAsync();
            return new FormatedResponse() { InnerBody = query };
        }
        public async Task<FormatedResponse> GetBhxhStatusById(long id)
        {
            var query = await (from o in _dbContext.SysOtherLists.AsNoTracking().Where(o => o.TYPE_ID == 80 && o.ID == id)
                               select new
                               {
                                   Id = o.ID,
                                   Name = o.NAME,
                                   Code = o.CODE,
                               }).FirstAsync();
            return new FormatedResponse() { InnerBody = query };
        }
        public async Task<FormatedResponse> GetBhYtStatusById(long id)
        {
            var query = await (from o in _dbContext.SysOtherLists.AsNoTracking().Where(o => o.TYPE_ID == 81 && o.ID == id)
                               select new
                               {
                                   Id = o.ID,
                                   Name = o.NAME,
                                   Code = o.CODE,
                               }).FirstAsync();
            return new FormatedResponse() { InnerBody = query };
        }
        public async Task<FormatedResponse> GetInsWhereHealthById(long id)
        {
            var query = await (from o in _dbContext.InsWhereHealThs.AsNoTracking().Where(o => o.ID == id)
                               select new
                               {
                                   Id = o.ID,
                                   Name = o.NAME_VN,
                                   Code = o.CODE,
                               }).FirstAsync();
            return new FormatedResponse() { InnerBody = query };
        }


        public async Task<FormatedResponse> GetLstInsCheck(long id)
        {
            List<int> lstcheckInsItems = new List<int>();
            var empT = new InsInformationDTO();
            var emp = _dbContext.HuEmployees.AsNoTracking().Where(p => p.ID == id).FirstOrDefault();
            var sysContractType = _dbContext.SysContractTypes.AsNoTracking().Where(p => p.CODE == "HDCNSDLD" || p.CODE == "HDLD001" || p.CODE == "HDLD003" || p.CODE == "HDLD004").Select(p => p.ID).ToList();
            var contractType = _dbContext.HuContractTypes.AsNoTracking().Where(p => sysContractType.Contains((long)p.TYPE_ID!)).Select(p => p.ID).ToList();
            var contract = _dbContext.HuContracts.AsNoTracking().Where(p => p.EMPLOYEE_ID == emp.ID && contractType.Contains((long)p.CONTRACT_TYPE_ID!) && p.STATUS_ID == OtherConfig.STATUS_APPROVE).OrderBy(ic => ic.START_DATE).FirstOrDefault();

            if (contract != null)
            {
                var joined = _dbContext.HuContractTypes.AsNoTracking().Where(p => p.ID == contract.CONTRACT_TYPE_ID).First();
                if (joined != null)
                {
                    if (joined.IS_BHXH != null && joined.IS_BHXH == true)
                    {
                        lstcheckInsItems.Add(1);
                    }
                    if (joined.IS_BHYT != null && joined.IS_BHYT == true)
                    {
                        lstcheckInsItems.Add(2);
                    }
                    if (joined.IS_BHTN != null && joined.IS_BHTN == true)
                    {
                        lstcheckInsItems.Add(3);
                    }
                    if (joined.IS_BHTNLD_BNN != null && joined.IS_BHTNLD_BNN == true)
                    {
                        lstcheckInsItems.Add(4);
                    }

                    empT.LstCheckInsItems = new List<int>();

                    empT.LstCheckInsItems.AddRange(lstcheckInsItems.Distinct().ToList());
                    return new FormatedResponse() { InnerBody = empT };
                }
                return new FormatedResponse() { InnerBody = null };

            }
            return new FormatedResponse() { InnerBody = empT };

        }

        public virtual async Task<FormatedResponse> ToggleActiveIds(GenericUnitOfWork _uow, List<long> ids, bool valueToBind, string sid)
        {
            await Task.Run(() => null);
            throw new NotImplementedException();
        }
    }
}

