using API.All.DbContexts;
using API.All.SYSTEM.CoreAPI.Xlsx;
using API.DTO;
using CORE.DTO;
using CORE.Enum;
using CORE.GenericUOW;
using CORE.StaticConstant;

namespace API.All.HRM.Profile.ProfileAPI.HuCommendImport
{
    public class HuCommendImportRepository : IHuCommendImportRepository
    {
        private readonly GenericUnitOfWork _uow;
        private readonly FullDbContext _dbContext;
        private readonly GenericReducer<HU_COMMEND_IMPORT, HuCommendImportDTO> _genericReducer;
        private readonly List<string> XLSX_COLUMNS;

        public HuCommendImportRepository(FullDbContext context, GenericUnitOfWork uow)
        {
            _dbContext = context;
            _uow = uow;
            _genericReducer = new();
            XLSX_COLUMNS = new() { "XLSX_USER_ID", "XLSX_EX_CODE", "XLSX_INSERT_ON", "XLSX_SESSION", "XLSX_FILE_NAME", "XLSX_ROW" };
        }
        public async Task<GenericPhaseTwoListResponse<HuCommendImportDTO>> SinglePhaseQueryList(GenericQueryListDTO<HuCommendImportDTO> request)
        {
            var raw = from p in _dbContext.HuCommendImports
                      from b in _dbContext.HuCommendEmployeeImports.Where(b => b.XLSX_USER_ID == p.XLSX_USER_ID && b.XLSX_EX_CODE == p.XLSX_EX_CODE && p.XLSX_SESSION == p.XLSX_SESSION && b.XLSX_ROW == p.XLSX_ROW).DefaultIfEmpty()
                      from e in _dbContext.HuEmployees.Where(e => e.ID == b.EMPLOYEE_ID).DefaultIfEmpty()
                      from pos in _dbContext.HuPositions.Where(pos => pos.ID == e.POSITION_ID).DefaultIfEmpty()
                      from o in _dbContext.HuOrganizations.Where(o => o.ID == e.ORG_ID).DefaultIfEmpty()
                      from cObj in _dbContext.SysOtherLists.Where(x => x.ID == p.COMMEND_OBJ_ID).DefaultIfEmpty()
                      from rLevel in _dbContext.SysOtherLists.Where(x => x.ID == p.REWARD_LEVEL_ID).DefaultIfEmpty()
                      from sPayment in _dbContext.SysOtherLists.Where(x => x.ID == p.STATUS_PAYMENT_ID).DefaultIfEmpty()
                      from aAward in _dbContext.SysOtherLists.Where(x => x.ID == p.AWARD_TITLE_ID).DefaultIfEmpty()
                      from sReward in _dbContext.SysOtherLists.Where(x => x.ID == p.REWARD_ID).DefaultIfEmpty()
                      from ol in _dbContext.HuOrgLevels.Where(x => x.ID == p.ORG_LEVEL_ID).DefaultIfEmpty()
                      from rl in _dbContext.HuOrgLevels.Where(x => x.ID == Convert.ToInt64(p.LIST_REWARD_LEVEL_ID)).DefaultIfEmpty()
                      from eSign in _dbContext.HuEmployees.Where(e => e.ID == p.SIGN_ID).DefaultIfEmpty()
                      from posSign in _dbContext.HuPositions.Where(pos => pos.ID == eSign.POSITION_ID).DefaultIfEmpty()
                      from fSource in _dbContext.PaListFundSources.Where(x => x.ID == p.FUND_SOURCE_ID).DefaultIfEmpty()
                      from ePayment in _dbContext.HuEmployees.Where(e => e.ID == p.SIGN_PAYMENT_ID).DefaultIfEmpty()
                      from posPayment in _dbContext.HuPositions.Where(pos => pos.ID == ePayment.POSITION_ID).DefaultIfEmpty()
                      select new HuCommendImportDTO()
                      {
                          Id = p.XLSX_ROW,
                          // Phần thông tin có trong tất cả các
                          XlsxUserId = p.XLSX_USER_ID,
                          XlsxExCode = p.XLSX_EX_CODE,
                          XlsxSession = p.XLSX_SESSION,
                          XlsxInsertOn = p.XLSX_INSERT_ON,
                          XlsxFileName = p.XLSX_FILE_NAME,
                          XlsxRow = p.XLSX_ROW,
                          EmployeeId = p.EMPLOYEE_ID,
                          EmployeeCode = e.CODE,
                          EmployeeName = e.Profile!.FULL_NAME,
                          PositionName = pos.NAME,
                          OrgName = o.NAME,
                          StatusName = sPayment.NAME,
                          CommendObjId = p.COMMEND_OBJ_ID,
                          CommendObjName = cObj.NAME,
                          Year = p.YEAR,
                          No = p.NO,
                          SignDate = p.SIGN_DATE,
                          SignerName = eSign.Profile!.FULL_NAME,
                          SignerPosition = posSign.NAME,
                          SalaryIncreaseTime = p.SALARY_INCREASE_TIME,
                          AwardTitleName = aAward.NAME,
                          OrgLevelName = ol.NAME,
                          Reason = p.REASON,
                          Note = p.NOTE,
                          EffectDate = p.EFFECT_DATE!.Value,
                          PaymentNo = p.PAYMENT_NO,
                          FundSourceId = p.FUND_SOURCE_ID,
                          FundSourceName = fSource.NAME,
                          Money = p.MONEY,
                          RewardId = p.REWARD_ID,
                          RewardName = sReward.NAME,
                          ListRewardLevelId = p.LIST_REWARD_LEVEL_ID,
                          RewardLevelName = rl.NAME,
                          IsTax = p.IS_TAX,
                          MonthTax = p.MONTH_TAX,
                          PaymentContent = p.PAYMENT_CONTENT,
                          SignPaymentId = p.SIGN_PAYMENT_ID,
                          SignPaymentName = ePayment.Profile!.FULL_NAME,
                          PositionPaymentName = posPayment.NAME,
                          SignPaymentDate = p.SIGN_PAYMENT_DATE,
                          PaymentNote = p.PAYMENT_NOTE,
                      };
            var response = await _genericReducer.SinglePhaseReduce(raw, request);
            return response;
        }

        public async Task<FormatedResponse> Save(ImportQueryListBaseDTO request)
        {
            _uow.CreateTransaction();
            try
            {
                var now = DateTime.UtcNow;
                string currentNo = "";
                long newCommendId = 0 ;
                long newStatus = 0;
                // lay ra du lieu o bang tam
                var tmp1 = await _dbContext.HuCommendImports.Where(x => x.XLSX_USER_ID == request.XlsxSid && x.XLSX_EX_CODE == request.XlsxExCode && x.XLSX_SESSION == request.XlsxSession).ToListAsync();
                var tmp2 = await _dbContext.HuCommendEmployeeImports.Where(x => x.XLSX_USER_ID == request.XlsxSid && x.XLSX_EX_CODE == request.XlsxExCode && x.XLSX_SESSION == request.XlsxSession).ToListAsync();

                // Lay ra cac thuoc tinh o bang tam va bang chinh de map du lieu (bang tam bo qua cac thuoc tinh lien quan den XLSX)
                var tmp1Type = typeof(HU_COMMEND_IMPORT);
                var tmp1Properties = tmp1Type.GetProperties()?
                    .Where(x => !XLSX_COLUMNS.Contains(x.Name))
                    .ToList();
                var tmp2Type = typeof(HU_COMMEND_EMPLOYEE_IMPORT);
                var tmp2Properties = tmp2Type.GetProperties()?
                    .Where(x => !XLSX_COLUMNS.Contains(x.Name))
                    .ToList();

                var commendType = typeof(HU_COMMEND);
                var commendTypeProperties = commendType.GetProperties().ToList();
                var commendEmpType = typeof(HU_COMMEND_EMPLOYEE);
                var commendEmpTypeProperties = commendEmpType.GetProperties().ToList();
                var res = new HU_COMMEND_EMPLOYEE();
                // cha: HU_COMMEND, con: HU_COMMEND_EMPLOYEE
                tmp1.ForEach(item =>
                {
                    // trung so quyet dinh => bo qua
                    if(currentNo != item.NO)
                    {
                        var obj1 = Activator.CreateInstance(typeof(HU_COMMEND)) ?? throw new Exception(CommonMessageCode.ACTIVATOR_CREATE_INSTANCE_RETURNS_NULL);
                        var commend = (HU_COMMEND)obj1;
                        // lap qua cac thuoc tinh o bang cha de map du lieu
                        tmp1Properties?.ForEach(tmp1Property =>
                        {
                            // lay gia tri theo thuoc tinh o bang tam
                            var tmp1Value = tmp1Property.GetValue(item);
                            // lay thuoc tinh o bang chinh
                            var cvProperty = commendTypeProperties.SingleOrDefault(x => x.Name == tmp1Property.Name);
                            // kiem tra thuoc tinh o bang chinh ton tai hay khong
                            if (cvProperty != null)
                            {
                                if (tmp1Value != null)
                                {
                                    // gan gia tri cho thuoc tinh o bang chinh
                                    cvProperty.SetValue(commend, tmp1Value);
                                };
                            }
                            else
                            {
                                if (tmp1Value != null)
                                {
                                    throw new Exception($"{tmp1Property.Name} was not found in HU_COMMEND");
                                }
                            }
                        });

                        if(commend.COMMEND_OBJ_ID == null)
                        {
                            throw new Exception("COMMEND_OBJ_ID_CANNOT_NULL_IN_ROW_IN_ROW" + " " + item.XLSX_ROW);
                        }

                        if (commend.YEAR == null)
                        {
                            throw new Exception("YEAR_CANNOT_NULL_IN_ROW_IN_ROW" + " " + item.XLSX_ROW);
                        }

                        if (commend.NO == null)
                        {
                            throw new Exception("NO_CANNOT_NULL_IN_ROW_IN_ROW" + " " + item.XLSX_ROW);
                        }

                        if (commend.REWARD_ID == null)
                        {
                            throw new Exception("REWARD_ID_CANNOT_NULL_IN_ROW" + " " + item.XLSX_ROW);
                        }

                        if (commend.ORG_LEVEL_ID == null)
                        {
                            throw new Exception("ORG_LEVEL_ID_CANNOT_NULL_IN_ROW" + " " + item.XLSX_ROW);
                        }

                        if (commend.EFFECT_DATE == null)
                        {
                            throw new Exception("EFFECT_DATE_CANNOT_NULL_IN_ROW" + " " + item.XLSX_ROW);
                        }

                        if (commend.PAYMENT_NO == null)
                        {
                            throw new Exception("PAYMENT_NO_CANNOT_NULL_IN_ROW" + " " + item.XLSX_ROW);
                        }

                        if (commend.LIST_REWARD_LEVEL_ID == null)
                        {
                            throw new Exception("LIST_REWARD_LEVEL_ID_CANNOT_NULL_IN_ROW" + " " + item.XLSX_ROW);
                        }

                        commend.CREATED_DATE = now;
                        commend.CREATED_BY = request.XlsxSid;

                        _dbContext.HuCommends.Add(commend);
                        _dbContext.SaveChanges();

                        currentNo = commend.NO!;
                        // lay ra ID tu bang cha
                        newCommendId = commend.ID;
                        newStatus = commend.STATUS_PAYMENT_ID!.Value;
                    }


                    // lay ra doi tuong tuong ung o bang tam
                    var tmpCommendEmployee = tmp2.Single(x => x.XLSX_ROW == item.XLSX_ROW);

                    // gan ID
                    tmpCommendEmployee.COMMEND_ID = newCommendId;
                    tmpCommendEmployee.STATUS_ID = newStatus;

                    var obj2 = Activator.CreateInstance(typeof(HU_COMMEND_EMPLOYEE)) ?? throw new Exception(CommonMessageCode.ACTIVATOR_CREATE_INSTANCE_RETURNS_NULL);
                    var commendEmployee = (HU_COMMEND_EMPLOYEE)obj2;

                    // lap qua cac thuoc tinh o bang con de map du lieu
                    tmp2Properties?.ForEach(tmp2Property =>
                    {
                        var tmp2Value = tmp2Property.GetValue(tmpCommendEmployee);
                        var commendEmployeeProperty = commendEmpTypeProperties.Single(x => x.Name == tmp2Property.Name);
                        if (tmp2Value != null)
                        {
                            commendEmployeeProperty.SetValue(commendEmployee, tmp2Value);
                        };
                    });

                    commendEmployee.CREATED_DATE = now;
                    commendEmployee.CREATED_BY = request.XlsxSid;
                    res = commendEmployee;
                    _dbContext.HuCommendEmployees.Add(commendEmployee);
                    _dbContext.SaveChanges();

                });

                // Clear tmp
                _dbContext.HuCommendImports.RemoveRange(tmp1);
                _dbContext.HuCommendEmployeeImports.RemoveRange(tmp2);
                _dbContext.SaveChanges();



                _uow.Commit();
                return new() { InnerBody = res, StatusCode = EnumStatusCode.StatusCode200, MessageCode = CommonMessageCode.CREATE_SUCCESS };

            }
            catch (Exception ex)
            {
                _uow.Rollback();
                return new() { ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400, MessageCode = ex.Message };
            }
        }
    }
}
