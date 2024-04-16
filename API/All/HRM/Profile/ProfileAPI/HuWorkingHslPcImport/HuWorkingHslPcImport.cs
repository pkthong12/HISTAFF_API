using API.All.DbContexts;
using API.All.SYSTEM.Common;
using API.All.SYSTEM.CoreAPI.Xlsx;
using API.DTO;
using API.Entities;
using Common.Extensions;
using CORE.DTO;
using CORE.Enum;
using CORE.GenericUOW;
using CORE.StaticConstant;
using System;
using System.Diagnostics;
using System.Linq.Dynamic.Core;

namespace API.All.HRM.Profile.ProfileAPI.HuWorkingHslPcImport
{
    public class HuWorkingHslPcImport : IHuWorkingHslPcImport
    {
        private readonly GenericUnitOfWork _uow;
        private readonly FullDbContext _dbContext;
        private readonly GenericReducer<HU_WORKING_HSL_PC_IMPORT, HuWorkingHslPcImportDTO> _genericReducer;
        private readonly List<string> XLSX_COLUMNS;

        public HuWorkingHslPcImport(FullDbContext context, GenericUnitOfWork uow)
        {
            _dbContext = context;
            _uow = uow;
            _genericReducer = new();
            XLSX_COLUMNS = new() { "XLSX_USER_ID", "XLSX_EX_CODE", "XLSX_INSERT_ON", "XLSX_SESSION", "XLSX_FILE_NAME", "XLSX_ROW" };
        }
        public async Task<GenericPhaseTwoListResponse<HuWorkingHslPcImportDTO>> SinglePhaseQueryList(GenericQueryListDTO<HuWorkingHslPcImportDTO> request)
        {
            /*
                var raw = from a in _dbContext.HuWorkingHslPcImports 
                join b in _dbContext.HuEmployeeCvImports 
                on new { a.XLSX_USER_ID, a.XLSX_EX_CODE, a.XLSX_SESSION, a.XLSX_ROW } equals new { b.XLSX_USER_ID, b.XLSX_EX_CODE, b.XLSX_SESSION, b.XLSX_ROW }

             */

            var get_employee_id = (from item in _dbContext.HuWorkingHslPcImports
                                   from item2 in _dbContext.HuEmployeeCvs.Where(x => x.ID == item.PROFILE_ID)
                                   from item3 in _dbContext.HuEmployees.Where(x => x.PROFILE_ID == item2.ID)
                                   select item3.ID).FirstOrDefault();

            var get_signer_id = (from item in _dbContext.HuWorkingHslPcImports
                                   from item2 in _dbContext.HuEmployeeCvs.Where(x => x.ID == item.SIGN_PROFILE_ID)
                                   from item3 in _dbContext.HuEmployees.Where(x => x.PROFILE_ID == item2.ID)
                                   select item3.ID).FirstOrDefault();


            var raw = from a in _dbContext.HuWorkingHslPcImports
                      from reference_0 in _dbContext.HuEmployeeCvs.Where(x => x.ID == a.PROFILE_ID).DefaultIfEmpty()

                      from reference_1 in _dbContext.HuEmployees.Where(x => x.PROFILE_ID == reference_0.ID).DefaultIfEmpty()
                      
                      from reference_2 in _dbContext.HuEmployeeCvs.Where(x => x.ID == reference_1.PROFILE_ID).DefaultIfEmpty()
                      
                      from reference_3 in _dbContext.HuOrganizations.Where(x => x.ID == reference_1.ORG_ID).DefaultIfEmpty()
                      from reference_4 in _dbContext.HuPositions.Where(x => x.ID == reference_1.POSITION_ID).DefaultIfEmpty()
                      from reference_5 in _dbContext.SysOtherLists.Where(x => x.ID == a.TYPE_ID).DefaultIfEmpty()
                      from reference_6 in _dbContext.HuSalaryTypes.Where(x => x.ID == a.SALARY_TYPE_ID).DefaultIfEmpty()
                      from reference_7 in _dbContext.SysOtherLists.Where(x => x.ID == a.TAXTABLE_ID).DefaultIfEmpty()

                      from reference_8 in _dbContext.HuOrganizations.Where(x => x.ID == reference_1.ORG_ID)
                      from reference_9 in _dbContext.HuCompanys.Where(x => x.ID == reference_8.COMPANY_ID).DefaultIfEmpty()
                      from reference_10 in _dbContext.SysOtherLists.Where(x => x.ID == reference_9.REGION_ID).DefaultIfEmpty()

                      from reference_11 in _dbContext.HuSalaryScales.Where(x => x.ID == a.SALARY_SCALE_ID).DefaultIfEmpty()
                      from reference_12 in _dbContext.HuSalaryRanks.Where(x => x.ID == a.SALARY_RANK_ID).DefaultIfEmpty()
                      from reference_13 in _dbContext.HuSalaryLevels.Where(x => x.ID == a.SALARY_LEVEL_ID).DefaultIfEmpty()

                      from reference_14 in _dbContext.HuSalaryScales.Where(x => x.ID == a.SALARY_SCALE_DCV_ID).DefaultIfEmpty()
                      from reference_15 in _dbContext.HuSalaryRanks.Where(x => x.ID == a.SALARY_RANK_DCV_ID).DefaultIfEmpty()
                      from reference_16 in _dbContext.HuSalaryLevels.Where(x => x.ID == a.SALARY_LEVEL_DCV_ID).DefaultIfEmpty()

                      from reference_17 in _dbContext.HuEmployeeCvs.Where(x => x.ID == a.SIGN_PROFILE_ID).DefaultIfEmpty()
                      from reference_18 in _dbContext.HuEmployees.Where(x => x.ID == get_signer_id).DefaultIfEmpty()
                      from reference_19 in _dbContext.HuPositions.Where(x => x.ID == reference_18.POSITION_ID).DefaultIfEmpty()

                      select new HuWorkingHslPcImportDTO()
                      {
                          Id = a.XLSX_ROW,
                          // Phần thông tin có trong tất cả các
                          XlsxUserId = a.XLSX_USER_ID,
                          XlsxExCode = a.XLSX_EX_CODE,
                          XlsxSession = a.XLSX_SESSION,
                          XlsxInsertOn = a.XLSX_INSERT_ON,
                          XlsxFileName = a.XLSX_FILE_NAME,
                          XlsxRow = a.XLSX_ROW,

                          // Phần thông tin thô từ bảng gốc
                          EmployeeCode = reference_1.CODE,
                          EmployeeName = reference_2.FULL_NAME,
                          OrgName = reference_3.NAME,
                          PositionName = reference_4.NAME,
                          TypeName = reference_5.NAME,
                          DecisionNo = a.DECISION_NO,
                          EffectDateStr = (a.EFFECT_DATE != null) ? a.EFFECT_DATE.Value.ToString("dd/MM/yyyy") : "",
                          ExpireDateStr = (a.EXPIRE_DATE != null) ? a.EXPIRE_DATE.Value.ToString("dd/MM/yyyy") : "",
                          ExpireUpsalDateStr = (a.EXPIRE_UPSAL_DATE != null) ? a.EXPIRE_UPSAL_DATE.Value.ToString("dd/MM/yyyy") : "",
                          ShortTempSalary = a.SHORT_TEMP_SALARY,
                          SalaryType = reference_6.NAME,
                          
                          // đối với màn hình "hồ sơ lương và phụ cấp"
                          // thì nó fix cứng luôn
                          // BHXH - có
                          // BHYT - có
                          // BHTN - có
                          // BHTNLD_BNN - có
                          BhxhStatusName = "Có",
                          BhytStatusName = "Có",
                          BhtnStatusName = "Có",
                          BhtnldBnnStatusName = "Có",

                          TaxTableName = reference_7.NAME,
                          RegionName = reference_10.NAME,

                          SalaryScaleName = reference_11.NAME,
                          SalaryRankName = reference_12.NAME,
                          SalaryLevelName = reference_13.NAME,
                          Coefficient = a.COEFFICIENT,

                          SalaryScaleDcvName = reference_14.NAME,
                          SalaryRankDcvName = reference_15.NAME,
                          SalaryLevelDcvName = reference_16.NAME,
                          CoefficientDcv = a.COEFFICIENT_DCV,

                          SalPercent = a.SAL_PERCENT,
                          SignerName = reference_17.FULL_NAME,
                          SignerPosition = reference_19.NAME,
                          SignDateStr = (a.SIGN_DATE != null) ? a.SIGN_DATE.Value.ToString("dd/MM/yyyy") : "",
                          
                          Note = a.NOTE
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

                var tmp1 = await _dbContext.HuWorkingHslPcImports.Where(x => x.XLSX_USER_ID == request.XlsxSid && x.XLSX_EX_CODE == request.XlsxExCode && x.XLSX_SESSION == request.XlsxSession).ToListAsync();

                var tmp1Type = typeof(HU_WORKING_HSL_PC_IMPORT);
                var tmp1Properties = tmp1Type.GetProperties()?
                    .Where(x => !XLSX_COLUMNS.Contains(x.Name))
                    .ToList();

                var huWorkingType = typeof(HU_WORKING);
                var huWorkingTypeProperties = huWorkingType.GetProperties().ToList();

                foreach (var tmpHuWorking in tmp1)
                {
                    var obj1 = Activator.CreateInstance(typeof(HU_WORKING)) ?? throw new Exception(CommonMessageCode.ACTIVATOR_CREATE_INSTANCE_RETURNS_NULL);
                    var huWorking = (HU_WORKING)obj1;

                    // lap qua cac thuoc tinh o bang cha de map du lieu
                    tmp1Properties?.ForEach(tmp1Property =>
                    {
                        // lay gia tri theo thuoc tinh o bang tam
                        var tmp1Value = tmp1Property.GetValue(tmpHuWorking);

                        // lay thuoc tinh o bang chinh
                        var huWorkingProperty = huWorkingTypeProperties.SingleOrDefault(x => x.Name == tmp1Property.Name);

                        // kiem tra thuoc tinh o bang chinh ton tai hay khong
                        if (huWorkingProperty != null)
                        {
                            if (tmp1Value != null)
                            {
                                // gan gia tri cho thuoc tinh o bang chinh
                                huWorkingProperty.SetValue(huWorking, tmp1Value);
                            };
                        }
                        else
                        {
                            if (tmp1Value != null)
                            {
                                throw new Exception($"{tmp1Property.Name} was not found in HU_WORKING");
                            }
                        }
                    });

                    if (huWorking.TYPE_ID == 0 || huWorking.TYPE_ID == null)
                    {
                        return new FormatedResponse()
                        {
                            ErrorType = EnumErrorType.CATCHABLE,
                            MessageCode = "Phải nhập loại văn bản",
                            StatusCode = EnumStatusCode.StatusCode400
                        };
                    }

                    if (huWorking.DECISION_NO == "" || huWorking.DECISION_NO == null)
                    {
                        return new FormatedResponse()
                        {
                            ErrorType = EnumErrorType.CATCHABLE,
                            MessageCode = "Phải nhập số quyết định",
                            StatusCode = EnumStatusCode.StatusCode400
                        };
                    }

                    if (huWorking.EFFECT_DATE == null)
                    {
                        return new FormatedResponse()
                        {
                            ErrorType = EnumErrorType.CATCHABLE,
                            MessageCode = "Phải nhập ngày hiệu lực",
                            StatusCode = EnumStatusCode.StatusCode400
                        };
                    }

                    if (huWorking.TAXTABLE_ID == null)
                    {
                        return new FormatedResponse()
                        {
                            ErrorType = EnumErrorType.CATCHABLE,
                            MessageCode = "Phải nhập biểu thuế",
                            StatusCode = EnumStatusCode.StatusCode400
                        };
                    }

                    if (huWorking.SALARY_TYPE_ID == null)
                    {
                        return new FormatedResponse()
                        {
                            ErrorType = EnumErrorType.CATCHABLE,
                            MessageCode = "Phải nhập đối tượng lương",
                            StatusCode = EnumStatusCode.StatusCode400
                        };
                    }

                    if (huWorking.SAL_PERCENT == null)
                    {
                        return new FormatedResponse()
                        {
                            ErrorType = EnumErrorType.CATCHABLE,
                            MessageCode = "Phải nhập % hưởng lương",
                            StatusCode = EnumStatusCode.StatusCode400
                        };
                    }

                    huWorking.CREATED_DATE = now;
                    huWorking.CREATED_BY = request.XlsxSid;


                    var get_employee = _dbContext.HuEmployees.First(x => x.PROFILE_ID == tmpHuWorking.PROFILE_ID);

                    huWorking.EMPLOYEE_ID = get_employee.ID;
                    huWorking.ORG_ID = get_employee.ORG_ID;
                    huWorking.POSITION_ID = get_employee.POSITION_ID;

                    var get_id_wait_approve = _dbContext.SysOtherLists.Where(x => x.CODE == "CD").Select(x => x.ID).First();
                    huWorking.STATUS_ID = get_id_wait_approve;

                    // thiết lập cứng cái IS_WAGE = -1
                    huWorking.IS_WAGE = -1;

                    var get_sign_id = _dbContext.HuEmployees.FirstOrDefault(x => x.PROFILE_ID == tmpHuWorking.SIGN_PROFILE_ID)?.ID;

                    huWorking.SIGN_ID = get_sign_id;

                    huWorking.IS_BHXH = -1;
                    huWorking.IS_BHYT = -1;
                    huWorking.IS_BHTN = -1;
                    huWorking.IS_BHTNLD_BNN = -1;


                    var get_position_of_signer = (from item in _dbContext.HuEmployees.Where(x => x.ID == get_sign_id)
                                                  from reference_1 in _dbContext.HuPositions.Where(x => x.ID == item.POSITION_ID)
                                                  select reference_1.NAME).FirstOrDefault();
                    huWorking.SIGNER_POSITION = get_position_of_signer;


                    // tính SAL_INSU mới theo task VEAM-1
                    if (huWorking.SHORT_TEMP_SALARY == null)
                    {
                        var getCoefficient = _dbContext.HuSalaryLevels.FirstOrDefault(x => x.ID == huWorking.SALARY_LEVEL_ID)?.COEFFICIENT;

                        huWorking.COEFFICIENT = getCoefficient;

                        if (huWorking.COEFFICIENT != null)
                        {
                            var effectDate = new DateTime(huWorking.EFFECT_DATE.Value.Year, huWorking.EFFECT_DATE.Value.Month, huWorking.EFFECT_DATE.Value.Day, 0, 0, 0);

                            var getOrgId = _dbContext.HuEmployees.FirstOrDefault(x => x.ID == huWorking.EMPLOYEE_ID)?.ORG_ID;

                            var getCompanyId = _dbContext.HuOrganizations.FirstOrDefault(x => x.ID == getOrgId)?.COMPANY_ID;

                            var getRegionId = _dbContext.HuCompanys.FirstOrDefault(x => x.ID == getCompanyId)?.REGION_ID;

                            var list = await _dbContext.InsRegions
                                             .Where(x => x.IS_ACTIVE == true && x.AREA_ID == getRegionId)
                                             .OrderByDescending(x => x.EFFECT_DATE)
                                             .Select(x => new {
                                                 Money = x.MONEY,
                                                 EffectDate = new DateTime(x.EFFECT_DATE.Value.Year, x.EFFECT_DATE.Value.Month, x.EFFECT_DATE.Value.Day, 0, 0, 0)
                                             })
                                             .ToListAsync();

                            var moneyRegion = (from item in list
                                               where item.EffectDate <= effectDate
                                               select item.Money).FirstOrDefault();

                            if (moneyRegion == null)
                            {
                                return new FormatedResponse()
                                {
                                    ErrorType = EnumErrorType.CATCHABLE,
                                    MessageCode = CommonMessageCodes.THERE_IS_NO_AVAILABILITY_MINIMUM_WAGE,
                                    StatusCode = EnumStatusCode.StatusCode400
                                };
                            }

                            var salInsu = huWorking.COEFFICIENT * moneyRegion;

                            huWorking.SAL_INSU = Math.Floor((decimal)salInsu);
                        }
                    }


                    // thêm mới hồ sơ lương
                    _dbContext.HuWorkings.Add(huWorking);
                    _dbContext.SaveChanges();
                }

                // Clear tmp
                _dbContext.HuWorkingHslPcImports.RemoveRange(tmp1);
                _dbContext.SaveChanges();

                _uow.Commit();

                return new() { InnerBody = true };
            }
            catch (Exception ex)
            {
                _uow.Rollback();
                return new() { ErrorType = EnumErrorType.UNCATCHABLE, StatusCode = EnumStatusCode.StatusCode500, MessageCode = ex.Message };
            }
        }
    }
}