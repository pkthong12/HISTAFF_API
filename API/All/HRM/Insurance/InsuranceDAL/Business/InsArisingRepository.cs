using CORE.GenericUOW;
using CORE.DTO;
using API.DTO;
using CORE.Enum;
using CORE.StaticConstant;
using API.All.DbContexts;
using CORE.AutoMapper;
using API;
using Common.Extensions;
using System;
using Microsoft.VisualBasic;
using Org.BouncyCastle.Asn1;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Information;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Logical;
using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;
using Common.Repositories;
using System.Linq.Dynamic.Core;
using System.Drawing;

namespace InsuranceDAL.Repositories
{
    public class InsArisingRepository : IInsArisingRepository
    {
        private readonly GenericUnitOfWork _uow;
        private readonly CoreDbContext _dbContext;
        private IGenericRepository<INS_ARISING, InsArisingDTO> _genericRepository;
        private readonly GenericReducer<INS_ARISING, InsArisingDTO> _genericReducer;

        public InsArisingRepository(CoreDbContext context, GenericUnitOfWork uow)
        {
            _dbContext = context;
            _uow = uow;
            _genericRepository = _uow.GenericRepository<INS_ARISING, InsArisingDTO>();
            _genericReducer = new();
        }

        public async Task<bool> InsertListArising(GenericUnitOfWork _uow, string sid)
        {
            var terminates = (from p in _dbContext.Terminates
                              where p.STATUS_ID == OtherConfig.STATUS_APPROVE && p.EFFECT_DATE.Value.Date <= DateTime.Now.Date
                                                                 && p.EFFECT_DATE >= DateTime.Now.Date.AddMonths(-1)
                              select p).ToList();
            foreach (var t in terminates)
            {
                await InsertArising(_uow, new InsArisingDTO() { PkeyRef = t.ID, EmployeeId = t.EMPLOYEE_ID, EffectDate = t.EFFECT_DATE, TableRef = "TERMINATE" }, sid);
            }
            var workings = (from p in _dbContext.Workings
                            where p.STATUS_ID == OtherConfig.STATUS_APPROVE && p.EFFECT_DATE.Value.Date <= DateTime.Now.Date
                                                             && p.EFFECT_DATE >= DateTime.Now.Date.AddMonths(-1) && p.IS_WAGE == null
                            select p).ToList();
            foreach (var t in workings)
            {
                await InsertArising(_uow, new InsArisingDTO() { PkeyRef = t.ID, EmployeeId = t.EMPLOYEE_ID, EffectDate = t.EFFECT_DATE, TableRef = "WORKING" }, sid);
            }
            var wage = (from p in _dbContext.Workings
                        where p.STATUS_ID == OtherConfig.STATUS_APPROVE && p.EFFECT_DATE.Value.Date <= DateTime.Now.Date
                                                         && p.EFFECT_DATE >= DateTime.Now.Date.AddMonths(-1) && (p.IS_WAGE == 1 || p.IS_WAGE == -1)
                        select p).ToList();
            foreach (var t in wage)
            {
                await InsertArising(_uow, new InsArisingDTO() { PkeyRef = t.ID, EmployeeId = t.EMPLOYEE_ID, EffectDate = t.EFFECT_DATE, TableRef = "WAGE" }, sid);
            }
            var contracts = (from p in _dbContext.Contracts
                             where p.STATUS_ID == OtherConfig.STATUS_APPROVE && p.START_DATE.Value.Date <= DateTime.Now.Date
                                                             && p.START_DATE >= DateTime.Now.Date.AddMonths(-1)
                             select p).ToList();
            foreach (var t in contracts)
            {
                await InsertArising(_uow, new InsArisingDTO() { PkeyRef = t.ID, EmployeeId = t.EMPLOYEE_ID, EffectDate = t.START_DATE, TableRef = "CONTRACT" }, sid);
            }
            return true;
        }

        public async Task<FormatedResponse> Create(GenericUnitOfWork _uow, InsArisingDTO dto, string sid)
        {
            try
            {
                var checkgrouptype = (from p in _dbContext.Arisings.Where(p => dto.Ids!.Contains(p.ID)) select p.INS_GROUP_TYPE).Distinct().Count();
                var joined = new INS_ARISING();
                foreach (var id in dto.Ids!)
                {
                    var arising = (from p in _dbContext.Arisings.Where(p => p.ID == id) select p).FirstOrDefault();
                    joined = arising;
                    var specifiedObject = (from p in _dbContext.SpecifiedObjects.Where(p => p.EFFECTIVE_DATE.Value.Date <= arising.EFFECT_DATE.Value.Date).OrderByDescending(p => p.EFFECTIVE_DATE) select p).FirstOrDefault();
                    //var specifiedObject1 = (from p in _dbContext.SpecifiedObjects.Where(p => p.EFFECTIVE_DATE < arising.EFFECT_DATE).OrderByDescending(p => p.EFFECTIVE_DATE) select p).FirstOrDefault();

                    var getIdTm = (from p in _dbContext.Types.Where(p => p.CODE == "TM") select p).FirstOrDefault();

                    if (specifiedObject == null)
                    {
                        return new FormatedResponse() { ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400, MessageCode = "UI_FORM_CONTROL_ERROR_SPECIFIED_OBJECT_NOT_FOUND" };
                    }
                    if (getIdTm != null)
                    {
                        var checkTmArising = (from p in _dbContext.Changes.Where(p => p.CHANGE_TYPE_ID == getIdTm.ID && p.EMPLOYEE_ID == arising.EMPLOYEE_ID && p.CHANGE_TYPE_ID == dto.InsTypeId) select p).Any();
                        if (checkTmArising)
                        {
                            return new FormatedResponse() { ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400, MessageCode = "UI_FORM_CONTROL_ERROR_CHANGE_TM_IS_EXISTS" };
                        }
                    }

                    if (arising.EFFECT_DATE.Value.Day > specifiedObject.CHANGE_DAY)
                    {
                        if (int.Parse(arising.EFFECT_DATE.Value.ToString("yyyyMM")) >= int.Parse(dto.DeclaredMonth.Value.ToString("yyyyMM")))
                        {
                            return new FormatedResponse() { ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400, MessageCode = "UI_FORM_CONTROL_ERROR_DECLARE_MONTH_IS_INVALID" };
                        }
                    }

                    string[] strArrT = { "TD", "TM", "ON" };
                    string[] strArrG = { "GH", "GC", "TS", "OM", "OP", "OF", "KL" };
                    var insChangeCheck = (from p in _dbContext.Changes.Where(p => p.EMPLOYEE_ID == arising.EMPLOYEE_ID) select p).FirstOrDefault();
                    var insTypeCheck = (from p in _dbContext.Types.Where(p => p.ID == dto.InsTypeChooseId && p.CODE == "TM") select p).FirstOrDefault();
                    var insType = (from p in _dbContext.Types.Where(p => p.ID == dto.InsTypeChooseId) select p).FirstOrDefault();
                    var arisingTypeCode = (from p in _dbContext.SysOtherLists.Where(p => p.ID == insType.TYPE_ID) select p.CODE).FirstOrDefault();

                    if (insChangeCheck == null)
                    {
                        if (insTypeCheck == null)
                        {
                            return new FormatedResponse() { ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400, MessageCode = "UI_FORM_CONTROL_ERROR_FIRST_ARISING_TYPE_MUST_BE_AN_INCREASEMENT" };
                        }
                    }
                    else
                    {
                        if (insChangeCheck.DECLARATION_PERIOD != null)
                        {
                            if (int.Parse(insChangeCheck.DECLARATION_PERIOD.Value.ToString("yyyyMM")) > int.Parse(dto.DeclaredMonth.Value.ToString("yyyyMM")))
                            {
                                return new FormatedResponse() { ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400, MessageCode = "UI_FORM_CONTROL_ERROR_DECLARE_MONTH_MUST_GREATER_THAN_LAST_ARISING_CHANGE" };
                            }
                        }
                    }
                    if (arising.NEW_SAL != null)
                    {
                        if (arising.NEW_SAL > 0)
                        {
                            if (arisingTypeCode == "799")
                            {
                                return new FormatedResponse() { ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400, MessageCode = "UI_FORM_CONTROL_ERROR_CANNOT_DECLARE_DECREASE_FOR_ARISING" };
                            }
                        }
                        else if (arising.NEW_SAL == 0)
                        {
                            if (arisingTypeCode != "799")
                            {
                                return new FormatedResponse() { ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400, MessageCode = "UI_FORM_CONTROL_ERROR_CANNOT_DECLARE_INCREASE_OR_AJUST_FOR_ARISING" };
                            }
                        }
                    }

                    var employee = (from p in _dbContext.Employees.Where(p => p.ID == arising.EMPLOYEE_ID) select p).FirstOrDefault();
                    //var terminate = (from p in _dbContext.Terminates.Where(p => p.ID == arising.EMPLOYEE_ID && p.STATUS_ID == OtherConfig.STATUS_APPROVE && p.EFFECT_DATE.Value.Date == arising.EFFECT_DATE.Value.Date) select p).FirstOrDefault();
                    var organization = (from p in _dbContext.Organizations.Where(p => p.ID == employee.ORG_ID) select p).FirstOrDefault();
                    var company = (from p in _dbContext.Companies.Where(p => p.ID == organization.COMPANY_ID) select p).FirstOrDefault();

                    var insEffectDate = arising.EFFECT_DATE.Value.Day <= specifiedObject.CHANGE_DAY ? arising.EFFECT_DATE : arising.EFFECT_DATE.Value.AddMonths(1);
                    var haveInsInfo = (from p in _dbContext.Informations.Where(p => p.EMPLOYEE_ID == arising.EMPLOYEE_ID) select p).Any();


                    if (!haveInsInfo)
                    {
                        var infomation = new INS_INFORMATION();
                        infomation.EMPLOYEE_ID = arising.EMPLOYEE_ID;
                        infomation.BHXH_NO = employee.Profile != null ? employee.Profile.INSURENCE_NUMBER : null;
                        infomation.BHYT_EFFECT_DATE = dto.EffectDate;
                        infomation.BHTNLD_BNN_FROM_DATE = insEffectDate;
                        //infomation.BHTNLD_BNN_TO_DATE = new DateTime(insEffectDate.Value.Year, insEffectDate.Value.Month, DateTime.DaysInMonth(insEffectDate.Value.Year, insEffectDate.Value.Month));
                        infomation.BHTN_FROM_DATE = insEffectDate;
                        //infomation.BHTN_TO_DATE = new DateTime(insEffectDate.Value.Year, insEffectDate.Value.Month, DateTime.DaysInMonth(insEffectDate.Value.Year, insEffectDate.Value.Month));
                        infomation.BHXH_FROM_DATE = insEffectDate;
                        //infomation.BHXH_TO_DATE = new DateTime(insEffectDate.Value.Year, insEffectDate.Value.Month, DateTime.DaysInMonth(insEffectDate.Value.Year, insEffectDate.Value.Month));
                        infomation.BHYT_FROM_DATE = insEffectDate;
                        //infomation.BHYT_TO_DATE = new DateTime(insEffectDate.Value.Year, insEffectDate.Value.Month, DateTime.DaysInMonth(insEffectDate.Value.Year, insEffectDate.Value.Month));
                        infomation.BHYT_NO = employee.Profile != null ? employee.Profile.INS_CARD_NUMBER : null;
                        infomation.CREATED_BY = sid;
                        infomation.UPDATED_BY = sid;
                        infomation.CREATED_DATE = DateTime.Now;
                        infomation.UPDATED_DATE = DateTime.Now;
                        var resultInsInfo = await _dbContext.Informations.AddAsync(infomation);
                    }
                    else
                    {
                        if (insType!.CODE == "TM")
                        {
                            return new FormatedResponse() { ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400, MessageCode = "EMPLOYEE_EXIST_INFORMATION_INSURANCE" };

                        }
                    }
                    DateTime? fromDate = new DateTime(insEffectDate.Value.Year, insEffectDate.Value.Month, 1);
                    DateTime? toDate = new DateTime(dto.DeclaredMonth!.Value.AddMonths(-1).Year, dto.DeclaredMonth!.Value.AddMonths(-1).Month, 3);

                    //AI: BHTNLD-BNN
                    //UI: BHTN
                    //SI: BHXH
                    //HI: BHYT
                    decimal? aSi = 0, aHi = 0, aUi = 0, aAi = 0, rSi = 0, rHi = 0, rUi = 0, rAi = 0, si = 0, hi = 0, ui = 0, ai = 0;
                    long? unit = 0;
                    if (fromDate < toDate)
                    {
                        si = specifiedObject.SI_EMP + specifiedObject.SI_COM;// 
                        hi = specifiedObject.HI_EMP + specifiedObject.HI_COM;//
                        ui = specifiedObject.UI_EMP + specifiedObject.UI_COM;//
                        ai = specifiedObject.AI_OAI_EMP + specifiedObject.AI_OAI_COM;//
                    }
                    var oldSal = arising.OLD_SAL == null ? 0 : arising.OLD_SAL;
                    var newSal = arising.NEW_SAL == null ? 0 : arising.NEW_SAL;
                    decimal? siSal = specifiedObject.SI_HI == null ? 0 : ((decimal)newSal < specifiedObject.SI_HI) ? (decimal)newSal : specifiedObject.SI_HI, uiSal = 0;

                    decimal? siSalOld = specifiedObject.SI_HI == null ? 0 : ((decimal)oldSal < specifiedObject.SI_HI) ? (decimal)oldSal : specifiedObject.SI_HI, uiSalOld = 0, regionMoney = 0;


                    if (company != null)
                    {
                        var regionId = company.REGION_ID;

                        var region = (from o in _dbContext.SysOtherLists
                                      from r in _dbContext.Regions.Where(p => p.AREA_ID == o.ID && p.EFFECT_DATE!.Value.Date <= dto.DeclaredDate!.Value.Date)
                                      where o.ID == regionId && r.EFFECT_DATE <= dto.DeclaredDate
                                      orderby r.EFFECT_DATE descending
                                      select r).FirstOrDefault();

                        if (region != null) regionMoney = region.CEILING_UI; uiSal = (decimal)newSal < region.CEILING_UI ? (decimal)newSal : region.CEILING_UI;

                        var region1 = (from o in _dbContext.SysOtherLists
                                       from r in _dbContext.Regions.Where(p => p.AREA_ID == o.ID && p.EFFECT_DATE.Value.Date <= dto.DeclaredDate.Value.Date)
                                       where o.ID == regionId && r.EFFECT_DATE < dto.DeclaredDate
                                       orderby r.EFFECT_DATE descending
                                       select r).FirstOrDefault();

                        if (region1 != null) uiSalOld = (decimal)oldSal < region1.CEILING_UI ? (decimal)oldSal : region1.CEILING_UI;

                        unit = await (from e in _dbContext.Employees.AsNoTracking().Where(emp => emp.ID == arising.EMPLOYEE_ID)
                                      from o in _dbContext.Organizations.AsNoTracking().Where(o => o.ID == e.ORG_ID).DefaultIfEmpty()
                                      from c in _dbContext.Companies.AsNoTracking().Where(c => c.ID == o.COMPANY_ID).DefaultIfEmpty()
                                      from s in _dbContext.SysOtherLists.AsNoTracking().Where(s => s.ID == c.INS_UNIT).DefaultIfEmpty()
                                      select s).Select(p => p.ID).FirstOrDefaultAsync();
                    }

                    
                    var monthTT = (decimal)Math.Ceiling(toDate.Value.Subtract(fromDate.Value).Days / (365.25 / 12) + 1);
                    if (toDate.Value.Date > fromDate.Value.Date)
                    {
                        if (strArrT.Contains(insType!.CODE))
                        {
                            aSi = (si * siSal * monthTT);
                            aHi = (hi * siSal * monthTT);
                            aUi = (ui * uiSal * monthTT);
                            aAi = (ai * siSal * monthTT);
                        }
                        if (strArrG.Contains(insType!.CODE))
                        {
                            rSi = (si * siSalOld * monthTT);
                            rHi = (si * siSalOld * monthTT);
                            rUi = (ui * uiSalOld * monthTT);
                            rAi = (ai * siSalOld * monthTT);
                        }
                    }
                    var insChange = new INS_CHANGE();
                    insChange.EMPLOYEE_ID = arising.EMPLOYEE_ID;
                    insChange.UNIT_INSURANCE_TYPE_ID = arising.INS_ORG_ID;
                    insChange.CHANGE_TYPE_ID = dto.InsTypeChooseId == null ? null : (int)dto.InsTypeChooseId;
                    insChange.SALARY_OLD = arising.OLD_SAL == null ? null : (double)arising.OLD_SAL;//double
                    insChange.SALARY_NEW = arising.NEW_SAL == null ? null : (double)arising.NEW_SAL;//double
                    insChange.EFFECTIVE_DATE = arising.EFFECT_DATE;
                    insChange.DECLARATION_PERIOD = dto.DeclaredMonth;
                    insChange.CHANGE_MONTH = insEffectDate;
                    insChange.IS_BHXH = arising.SI;
                    insChange.IS_BHYT = arising.HI;
                    insChange.IS_BNN = arising.AI;
                    insChange.IS_BHTN = arising.UI;
                    insChange.UNIT_INSURANCE_TYPE_ID = unit??0;
                    insChange.SALARY_BHXH_BHYT_OLD = siSalOld;
                    insChange.SALARY_BHXH_BHYT_NEW = siSal;
                    //muc dong moi
                    insChange.SALARY_BHXH_NEW = (long)siSal!;
                    insChange.SALARY_BHYT_NEW = (long)siSal!;
                    insChange.SALARY_BHBNN_NEW = (long)siSal!;
                    insChange.SALARY_BHTN_NEW = uiSal;
                    //muc dong cu
                    insChange.SALARY_BHXH_OLD = (long)siSalOld!;
                    insChange.SALARY_BHYT_OLD = (long)siSalOld!;
                    insChange.SALARY_BHBNN_OLD = (long)siSalOld!;
                    insChange.SALARY_BHTN_OLD = uiSalOld;
                    if (strArrT.Contains(insType!.CODE) && toDate!.Value.Date > fromDate!.Value.Date)
                    {
                        insChange.ARREARS_FROM_MONTH = fromDate;
                        insChange.ARREARS_TO_MONTH = toDate;
                        insChange.AR_BHXH_SALARY_DIFFERENCE = aSi < (specifiedObject.SI_HI ?? 0) ? aSi : specifiedObject.SI_HI;
                        insChange.AR_BHYT_SALARY_DIFFERENCE = aHi < (specifiedObject.SI_HI ?? 0) ? aHi : specifiedObject.SI_HI;
                        insChange.AR_BHTN_SALARY_DIFFERENCE = aUi < (regionMoney) ? aSi : regionMoney;
                        insChange.AR_BHTNLD_BNN_SALARY_DIFFERENCE = aAi < (specifiedObject.SI_HI ?? 0) ? aAi : specifiedObject.SI_HI;
                    }
                    if (strArrG.Contains(insType!.CODE) && toDate!.Value.Date > fromDate!.Value.Date)
                    {
                        insChange.WITHDRAWAL_FROM_MONTH = fromDate;
                        insChange.WITHDRAWAL_TO_MONTH = toDate;
                        insChange.WD_BHXH_SALARY_DIFFERENCE = rSi < (specifiedObject.SI_HI ?? 0) ? rSi : specifiedObject.SI_HI;
                        insChange.WD_BHYT_SALARY_DIFFERENCE = rHi < (specifiedObject.SI_HI ?? 0) ? rHi : specifiedObject.SI_HI; ;
                        insChange.WD_BHTN_SALARY_DIFFERENCE = rUi < (regionMoney) ? rUi : regionMoney;
                        insChange.WD_BHTNLD_BNN_SALARY_DIFFERENCE = rAi < (specifiedObject.SI_HI ?? 0) ? rAi : specifiedObject.SI_HI;
                    }
                    var resultInsChange = await _dbContext.Changes.AddAsync(insChange);
                    if (arising.INS_GROUP_TYPE == 2 && insType.CODE == "GH")
                    {
                        var infomation = (from p in _dbContext.Informations.Where(p => p.EMPLOYEE_ID == arising.EMPLOYEE_ID) select p).FirstOrDefault();
                        if (infomation != null)
                        {
                            var lastDateOfMonth = new DateTime(dto.DeclaredMonth.Value.Year, dto.DeclaredMonth.Value.Month, DateTime.DaysInMonth(dto.DeclaredMonth.Value.Year, dto.DeclaredMonth.Value.Month));
                            infomation.BHYT_EXPIRE_DATE = infomation.BHYT_EFFECT_DATE != null ? lastDateOfMonth : null;
                            infomation.BHYT_TO_DATE = infomation.BHYT_FROM_DATE != null ? lastDateOfMonth : null;
                            infomation.BHXH_TO_DATE = infomation.BHXH_FROM_DATE != null ? lastDateOfMonth : null;
                            infomation.BHTN_TO_DATE = infomation.BHTN_FROM_DATE != null ? lastDateOfMonth : null;
                            infomation.BHTNLD_BNN_TO_DATE = infomation.BHTNLD_BNN_FROM_DATE != null ? lastDateOfMonth : null;
                            //_dbContext.Informations.Update(infomation);
                        }
                    }
                    if (arising.INS_GROUP_TYPE == 1 && insType.CODE == "TM")
                    {
                        var infomation = (from p in _dbContext.Informations.Where(p => p.EMPLOYEE_ID == arising.EMPLOYEE_ID) select p).FirstOrDefault();
                        if (infomation != null)
                        {
                            infomation.BHYT_EFFECT_DATE = insEffectDate;
                            infomation.BHYT_FROM_DATE = insEffectDate;
                            infomation.BHXH_FROM_DATE = insEffectDate;
                            infomation.BHTN_FROM_DATE = insEffectDate;
                            infomation.BHTNLD_BNN_FROM_DATE = insEffectDate;
                            infomation.BHXH_NO = employee.Profile != null ? employee.Profile.INSURENCE_NUMBER : null;
                            //_dbContext.Informations.Update(infomation);
                        }
                    }
                    arising.STATUS = 1;
                    arising.UPDATED_BY = sid;
                    arising.UPDATED_DATE = DateTime.Now;
                    arising.DECLARED_DATE = dto.DeclaredMonth;
                    arising.INS_TYPE_CHOOSE = dto.InsTypeChooseId;

                    await _dbContext.SaveChangesAsync();
                }
                return new() { MessageCode = CommonMessageCode.CREATE_SUCCESS, InnerBody = joined, StatusCode = EnumStatusCode.StatusCode200 };
            }
            catch (Exception ex)
            {
                return new FormatedResponse() { ErrorType = EnumErrorType.UNCATCHABLE, StatusCode = EnumStatusCode.StatusCode500, MessageCode = ex.Message };
            }
        }

        public async Task<bool> InsertArising(GenericUnitOfWork _uow, InsArisingDTO obj, string sid)
        {
            //383 - với quyết định thì không cần chờ tới ngày
            //if (obj.EffectDate.Value.Date > DateTime.Now.Date && obj.TableRef != "WORKING")
            //{
            //    return true;
            //}
            var specifiedObject = (from p in _dbContext.SpecifiedObjects.Where(p => p.EFFECTIVE_DATE.Value.Date <= obj.EffectDate.Value.Date).OrderByDescending(p => p.EFFECTIVE_DATE) select p).FirstOrDefault();
            var employee = (from p in _dbContext.Employees.Where(p => p.ID == obj.EmployeeId) select p).FirstOrDefault();
            var contract = (from p in _dbContext.Contracts.Where(p => p.EMPLOYEE_ID == obj.EmployeeId && p.STATUS_ID == OtherConfig.STATUS_APPROVE && p.START_DATE.Value.Date <= obj.EffectDate.Value.Date).OrderByDescending(p => p.START_DATE) select p).FirstOrDefault();
            var wage = (from p in _dbContext.Workings.Where(p => p.EMPLOYEE_ID == obj.EmployeeId && p.STATUS_ID == OtherConfig.STATUS_APPROVE && p.IS_WAGE == -1 && p.EFFECT_DATE.Value.Date <= obj.EffectDate!.Value.Date).OrderByDescending(p => p.EFFECT_DATE) select p).FirstOrDefault(); //&& p.IS_RESPONSIBLE == true
            var organization = (from p in _dbContext.Organizations.Where(p => p.ID == employee.ORG_ID) select p).FirstOrDefault();
            var company = (from p in _dbContext.Companies.Where(p => p.ID == organization.COMPANY_ID) select p).FirstOrDefault();
            if (company != null) obj.InsOrgId = company.INS_UNIT == null ? null : company.INS_UNIT;

            obj.IsDeleted = false;
            obj.Status = 0;
            if (specifiedObject != null && specifiedObject.ID > 0)
            {
                obj.InsSpecifiedId = specifiedObject.ID;
            }
            if (contract != null)
            {
                var contractType = (from p in _dbContext.Contracttypes.Where(p => p.ID == contract.CONTRACT_TYPE_ID) select p).FirstOrDefault();
                if (contractType != null)
                {
                    obj.Ai = contractType.IS_BHTNLD_BNN;
                    obj.Si = contractType.IS_BHXH;
                    obj.Ui = contractType.IS_BHTN;
                    obj.Hi = contractType.IS_BHYT;
                }
            }

            if (obj.TableRef == "TERMINATE")
            {
                var arising = (from p in _dbContext.Arisings.Where(p => p.PKEY_REF == obj.PkeyRef && p.TABLE_REF == "TERMINATE" && p.EMPLOYEE_ID == obj.EmployeeId) select p).FirstOrDefault();
                if (arising != null) return true;

                var terminate = (from p in _dbContext.Terminates.Where(p => p.ID == obj.PkeyRef) select p).FirstOrDefault();
                if (terminate!.STATUS_ID != OtherConfig.STATUS_APPROVE) return true;
                var changeObject = await (from p in _dbContext.Changes.AsNoTracking().Where(p => p.EMPLOYEE_ID == obj.EmployeeId)
                                          from c in _dbContext.Types.Where(c => c.ID == p.CHANGE_TYPE_ID && c.CODE == "GH")
                                          select p).AnyAsync();
                if (changeObject) return true;
                obj.EffectDate = terminate.EFFECT_DATE;
                obj.InsGroupType = 2;
                obj.InsTypeId = 88;
                obj.OldOrgId = employee!.ORG_ID;
                obj.NewOrgId = employee.ORG_ID;
                obj.OldPositionId = employee.POSITION_ID;
                obj.NewPositionId = employee.POSITION_ID;
                var wage_ter = (from p in _dbContext.Workings.Where(p => p.EMPLOYEE_ID == obj.EmployeeId && p.STATUS_ID == OtherConfig.STATUS_APPROVE && p.IS_WAGE == -1 && p.EFFECT_DATE!.Value.Date <= terminate.EFFECT_DATE!.Value.Date).OrderByDescending(p => p.EFFECT_DATE) select p).FirstOrDefault();//&& p.IS_RESPONSIBLE == true TẠI SAO CẦN CHUYÊN TRÁCH Ở ĐÂY?
                if (wage_ter != null)
                {
                    obj.OldSal = wage_ter.SAL_INSU == null ? null : (float)wage_ter.SAL_INSU;
                }
                obj.NewSal = 0;
                if (specifiedObject != null)
                {
                    if (obj.EffectDate!.Value.Day <= specifiedObject.CHANGE_DAY)
                    {
                        obj.DeclaredDate = obj.EffectDate;
                    }
                    else
                    {
                        obj.DeclaredDate = obj.EffectDate.Value.AddMonths(1);
                    };
                }
                obj.Reasons = "Giảm hẳn";
            }

            if (obj.TableRef == "WORKING")
            {
                var arising = (from p in _dbContext.Arisings.Where(p => p.PKEY_REF == obj.PkeyRef && p.TABLE_REF == "WORKING") select p).FirstOrDefault();
                if (arising != null) return true;

                var working = (from p in _dbContext.Workings.Where(p => p.ID == obj.PkeyRef) select p).FirstOrDefault();//quyet dinh moi
                if (working!.STATUS_ID != OtherConfig.STATUS_APPROVE) return true;

                var oldWage = (from p in _dbContext.Workings where p.EMPLOYEE_ID == obj.EmployeeId &&  p.ID != obj.PkeyRef && p.STATUS_ID == OtherConfig.STATUS_APPROVE && p.IS_WAGE == -1  orderby p.EFFECT_DATE descending select p).FirstOrDefault();// tim kiem ho so luong gan nhat && p.IS_RESPONSIBLE == true
                if (oldWage == null)
                {
                    return false; // chưa có hồ sơ lương
                }
                var oldWorking = (from p in _dbContext.Workings where p.EMPLOYEE_ID == obj.EmployeeId && p.ID != obj.PkeyRef && p.STATUS_ID == OtherConfig.STATUS_APPROVE && p.IS_WAGE != -1 orderby p.EFFECT_DATE descending select p).FirstOrDefault();//Find the most recent decision 
                //Phát sinh biến động cho quyết định thay đổi chức danh
                //ban ghi qtct dau tien khong sinh bien dong
                if (oldWorking != null && oldWage.POSITION_ID != working.POSITION_ID)
                {
                    var oldWageOfDecision = (from p in _dbContext.Workings where  p.ID == oldWorking.WAGE_ID && p.STATUS_ID == OtherConfig.STATUS_APPROVE  select p).FirstOrDefault();// get wage of the most recent decision  && p.IS_RESPONSIBLE == true
                    obj.EffectDate = working.EFFECT_DATE;
                    obj.InsGroupType = 3;
                    var typeId = _dbContext.Types.AsNoTracking().Where(p => p.CODE == "CD").FirstOrDefault();
                    if (typeId != null)
                    {
                        obj.InsTypeId = typeId.ID;
                    }
                    obj.OldOrgId = oldWorking.ORG_ID;
                    obj.NewOrgId = working.ORG_ID;
                    obj.OldPositionId = oldWorking.POSITION_ID;
                    obj.NewPositionId = working.POSITION_ID;
                    if (wage != null)
                    {
                        if (oldWageOfDecision != null)
                        {
                            if (working.WAGE_ID == oldWorking.WAGE_ID)
                            {
                                obj.OldSal = oldWageOfDecision.SAL_INSU == null ? null : (float)oldWageOfDecision.SAL_INSU;
                                obj.NewSal = oldWageOfDecision.SAL_INSU == null ? null : (float)oldWageOfDecision.SAL_INSU;
                            }
                            else
                            {
                                var newWageOfDecision = (from p in _dbContext.Workings where p.ID == working.WAGE_ID && p.STATUS_ID == OtherConfig.STATUS_APPROVE select p).FirstOrDefault();// get wage of the most recent decision   && p.IS_RESPONSIBLE == true
                                if(newWageOfDecision != null)
                                {
                                    obj.OldSal = oldWageOfDecision.SAL_INSU == null ? null : (float)oldWageOfDecision.SAL_INSU;
                                    obj.NewSal = newWageOfDecision.SAL_INSU == null ? null : (float)newWageOfDecision.SAL_INSU;
                                }else
                                {
                                    obj.OldSal = oldWageOfDecision.SAL_INSU == null ? null : (float)oldWageOfDecision.SAL_INSU;
                                    obj.NewInsSal = 0;
                                }
                            }
                        }
                        else
                        {
                            obj.OldSal = (float)wage.SAL_INSU == null ? null : (float)wage.SAL_INSU;
                            obj.NewSal = 0;
                        }

                        obj.Ai = wage.IS_BHTNLD_BNN == null ? null : Convert.ToBoolean(wage.IS_BHTNLD_BNN);
                        obj.Si = wage.IS_BHXH == null ? null : Convert.ToBoolean(wage.IS_BHXH);
                        obj.Ui = wage.IS_BHTN == null ? null : Convert.ToBoolean(wage.IS_BHTN);
                        obj.Hi = wage.IS_BHYT == null ? null : Convert.ToBoolean(wage.IS_BHYT);
                    }
                    var insinfomation = (from p in _dbContext.Informations.Where(p => p.EMPLOYEE_ID == obj.EmployeeId) select p).FirstOrDefault();
                    if (insinfomation != null)
                    {
                        obj.InsInformationId = insinfomation.ID;
                    }

                    if (specifiedObject != null)
                    {
                        if (obj.EffectDate.Value.Day <= specifiedObject.CHANGE_DAY)
                        {
                            obj.DeclaredDate = obj.EffectDate;
                        }
                        else
                        {
                            obj.DeclaredDate = obj.EffectDate.Value.AddMonths(1);
                        };
                    }
                    obj.Reasons = "Thay đổi chức danh";
                }
                else
                {
                    return true;
                }
            }
            //Wage
            if (obj.TableRef == "WAGE")
            {
                var arising = (from p in _dbContext.Arisings.Where(p => p.PKEY_REF == obj.PkeyRef && p.TABLE_REF == "WAGE") select p).FirstOrDefault();
                if (arising != null) return true;

                var _wage = (from p in _dbContext.Workings.Where(p => p.ID == obj.PkeyRef) select p).FirstOrDefault();
                if (_wage == null || _wage.STATUS_ID != OtherConfig.STATUS_APPROVE) return true;

                var oldWage = (from p in _dbContext.Workings where p.ID != obj.PkeyRef && p.EMPLOYEE_ID == obj.EmployeeId && p.STATUS_ID == OtherConfig.STATUS_APPROVE && (p.IS_WAGE == 1 || p.IS_WAGE == -1) orderby p.EFFECT_DATE descending select p).FirstOrDefault();
                //Phát sinh biến động cho quyết định thay đổi lương
                if (oldWage != null && oldWage.SAL_INSU != _wage.SAL_INSU)
                {
                    obj.EffectDate = _wage.EFFECT_DATE;
                    obj.InsGroupType = 3;
                    obj.InsTypeId = 98;
                    obj.OldOrgId = _wage.ORG_ID;
                    obj.NewOrgId = _wage.ORG_ID;
                    obj.OldPositionId = _wage.POSITION_ID;
                    obj.NewPositionId = _wage.POSITION_ID;

                    obj.OldSal = oldWage.SAL_INSU == null ? null : (float)oldWage.SAL_INSU;
                    obj.NewSal = _wage.SAL_INSU == null ? null : (float)_wage.SAL_INSU;

                    obj.Ai = _wage.IS_BHTNLD_BNN == null ? null : Convert.ToBoolean(_wage.IS_BHTNLD_BNN);
                    obj.Si = _wage.IS_BHXH == null ? null : Convert.ToBoolean(_wage.IS_BHXH);
                    obj.Ui = _wage.IS_BHTN == null ? null : Convert.ToBoolean(_wage.IS_BHTN);
                    obj.Hi = _wage.IS_BHYT == null ? null : Convert.ToBoolean(_wage.IS_BHYT);
                    var insinfomation = (from p in _dbContext.Informations.Where(p => p.EMPLOYEE_ID == obj.EmployeeId) select p).FirstOrDefault();
                    if (insinfomation != null)
                    {
                        obj.InsInformationId = insinfomation.ID;
                    }

                    if (specifiedObject != null)
                    {
                        if (obj.EffectDate.Value.Day <= specifiedObject.CHANGE_DAY)
                        {
                            obj.DeclaredDate = obj.EffectDate;
                        }
                        else
                        {
                            obj.DeclaredDate = obj.EffectDate.Value.AddMonths(1);
                        };
                    }
                    obj.Reasons = "Thay đổi lương";
                }
                else
                {
                    return true;
                }
            }
            if (obj.TableRef == "CONTRACT")
            {
                //ktrllong nay da co bien dong chua
                //co thi khong sinh -> moi hd chi sinh 1 bien dong
                // chi co 1 bien dong doi voi 1 nv + hd
                //var arising = (from p in _dbContext.Arisings.Where(p => p.PKEY_REF == obj.PkeyRef && p.TABLE_REF == "CONTRACT") select p).FirstOrDefault();
                var arising = (from p in _dbContext.Arisings.Where(p => p.EMPLOYEE_ID == obj.EmployeeId && p.TABLE_REF == "CONTRACT") select p).FirstOrDefault();

                if (arising != null) return true;

                var _contract = (from p in _dbContext.Contracts.Where(p => p.ID == obj.PkeyRef) select p).FirstOrDefault();
                //Phát sinh biến động 
                if (_contract != null)
                {
                    //var checkContact = await (from c in _dbContext.Contracts.AsNoTracking().Where(p => p.EMPLOYEE_ID == obj.EmployeeId).DefaultIfEmpty()
                    //                          from t in _dbContext.Contracttypes.AsNoTracking().Where(p => p.ID == c.CONTRACT_TYPE_ID).DefaultIfEmpty()
                    //                          from ct in _dbContext.SysContracttypes.AsNoTracking().Where(p => p.ID == t.TYPE_ID).DefaultIfEmpty()
                    //                          where ct.CODE == "HDLDKXD" || ct.CODE == "HDLDXD"  //where ct.CODE == "HDKXDTH" || ct.CODE == "HXDTH001" -> code old
                    //                          select c).ToListAsync();
                    ////check ton tai hop dong chinh thuc
                    //if (checkContact.Count > 1) return true;

                    if (_contract.STATUS_ID != OtherConfig.STATUS_APPROVE) return true;//hd phe duyet hay chua

                    //kiem tra hop dong nay can phat sinh bien dong khong
                    //sinh bien dong: HĐ -> 1.HĐKXĐTH-hợp đồng ko xác định thời hạn 2.HXDTH-Hợp đồng xác định thời hạn
                    var _contractType = (from p in _dbContext.Contracttypes
                                         from ct in _dbContext.SysContracttypes.Where(c => c.ID == p.TYPE_ID)
                                         where p.ID == _contract.CONTRACT_TYPE_ID && (ct.CODE == "HDKXDTH" || ct.CODE == "HXDTH")
                                         select p).FirstOrDefault();
                    if (_contractType == null)
                    {
                        return true;
                    }
                    if (_contractType.IS_BHTNLD_BNN == false && _contractType.IS_BHXH == false && _contractType.IS_BHTN == false && _contractType.IS_BHYT == false)
                    {
                        return true;
                    }
                    obj.Ai = _contractType.IS_BHTNLD_BNN;
                    obj.Si = _contractType.IS_BHXH;
                    obj.Ui = _contractType.IS_BHTN;
                    obj.Hi = _contractType.IS_BHYT;
                    obj.EffectDate = _contract.START_DATE;
                    obj.InsGroupType = 1;
                    var typeId = _dbContext.Types.AsNoTracking().Where(p => p.CODE == "TM").FirstOrDefault();
                    if (typeId != null)
                    {
                        obj.InsTypeId = typeId.ID;
                    }

                    var _Working = (from p in _dbContext.Workings where p.STATUS_ID == OtherConfig.STATUS_APPROVE && p.EFFECT_DATE.Value.Date <= _contract.START_DATE.Value.Date && p.IS_WAGE == null && p.IS_RESPONSIBLE == true orderby p.EFFECT_DATE descending select p).FirstOrDefault();
                    if (_Working != null)
                    {
                        obj.OldOrgId = _contract.ORG_ID;
                        obj.NewOrgId = _contract.ORG_ID;
                        obj.OldPositionId = _Working.POSITION_ID;
                        obj.NewPositionId = _Working.POSITION_ID;
                    }
                    var _wage = (from p in _dbContext.Workings where p.ID == _contract.WORKING_ID select p).FirstOrDefault();
                    if (_wage == null || _wage.SAL_INSU == null || _wage.SAL_INSU == 0) return true;
                    if (_wage != null)
                    {
                        obj.OldSal = (float)0;
                        obj.NewSal = _wage.SAL_INSU == null ? null : (float)_wage.SAL_INSU;
                    }
                    var insinfomation = (from p in _dbContext.Informations.Where(p => p.EMPLOYEE_ID == obj.EmployeeId) select p).FirstOrDefault();
                    if (insinfomation != null)
                    {
                        obj.InsInformationId = insinfomation.ID;
                    }
                    if (specifiedObject != null)
                    {
                        if (obj.EffectDate!.Value.Day <= specifiedObject.CHANGE_DAY)
                        {
                            obj.DeclaredDate = obj.EffectDate;
                        }
                        else
                        {
                            obj.DeclaredDate = obj.EffectDate.Value.AddMonths(1);
                        };
                    }
                    obj.Reasons = "Tăng mới";
                }
                else
                {
                    return true;
                }
            }

            try
            {
                var response = await _genericRepository.Create(_uow, obj, sid);
                //_dbContext.Database.CommitTransactionAsync();
                //_dbContext.Database.CommitTransaction();
                _dbContext.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<GenericPhaseTwoListResponse<InsArisingDTO>> SinglePhaseQueryList(GenericQueryListDTO<InsArisingDTO> request)
        {
            var joined = from p in _dbContext.Arisings.AsNoTracking()
                             // JOIN OTHER ENTITIES BASED ON THE BUSINESS
                         select new InsArisingDTO
                         {
                             Id = p.ID
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
                var list = new List<INS_ARISING>
                    {
                        (INS_ARISING)response
                    };
                var joined = (from l in list
                                  // JOIN OTHER ENTITIES BASED ON THE BUSINESS
                              select new InsArisingDTO
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
        public async Task<FormatedResponse> Update(GenericUnitOfWork _uow, InsArisingDTO dto, string sid, bool patchMode = true)
        {
            var response = await _genericRepository.Create(_uow, dto, sid);
            return response;
        }

        public async Task<FormatedResponse> CreateRange(GenericUnitOfWork _uow, List<InsArisingDTO> dtos, string sid)
        {
            var add = new List<InsArisingDTO>();
            add.AddRange(dtos);
            var response = await _genericRepository.CreateRange(_uow, add, sid);
            return response;
        }

        public async Task<FormatedResponse> UpdateRange(GenericUnitOfWork _uow, List<InsArisingDTO> dtos, string sid, bool patchMode = true)
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

