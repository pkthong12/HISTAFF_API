using API.All.DbContexts;
using API.All.SYSTEM.CoreAPI.Xlsx;
using API.DTO;
using Common.Extensions;
using CORE.DTO;
using CORE.Enum;
using CORE.GenericUOW;
using CORE.StaticConstant;
using System;
using System.Diagnostics;

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
                                   select item3.ID).First();

            var get_signer_id = (from item in _dbContext.HuWorkingHslPcImports
                                   from item2 in _dbContext.HuEmployeeCvs.Where(x => x.ID == item.SIGN_PROFILE_ID)
                                   from item3 in _dbContext.HuEmployees.Where(x => x.PROFILE_ID == item2.ID)
                                   select item3.ID).First();


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

        //public async Task<FormatedResponse> Save(ImportQueryListBaseDTO request)
        //{
        //    //_uow.CreateTransaction();
        //    try
        //    {
        //        var now = DateTime.UtcNow;

        //        var tmp1 = await _dbContext.HuWorkingHslPcImports
        //                    .Where(x => x.XLSX_USER_ID == request.XlsxSid
        //                            && x.XLSX_EX_CODE == request.XlsxExCode
        //                            && x.XLSX_SESSION == request.XlsxSession)
        //                    .ToListAsync();

        //        var tmp1Type = typeof(HU_WORKING_HSL_PC_IMPORT);
        //        var tmp1Properties = tmp1Type.GetProperties()?
        //            .Where(x => !XLSX_COLUMNS.Contains(x.Name))
        //            .ToList();


        //        var hu_working_Type = typeof(HU_WORKING);
        //        var hu_working_TypeProperties = hu_working_Type.GetProperties().ToList();


        //        foreach (var tmpHuWorking in tmp1)
        //        {
        //            var obj1 = Activator.CreateInstance(typeof(HU_WORKING)) ?? throw new Exception(CommonMessageCode.ACTIVATOR_CREATE_INSTANCE_RETURNS_NULL);
        //            var huWorking = (HU_WORKING)obj1;

        //            tmp1Properties?.ForEach(tmp1Property =>
        //            {
        //                var tmp1Value = tmp1Property.GetValue(tmpHuWorking);
        //                var huWorkingProperty = hu_working_TypeProperties.SingleOrDefault(x => x.Name == tmp1Property.Name);
        //                if (huWorkingProperty != null)
        //                {
        //                    if (tmp1Value != null)
        //                    {
        //                        huWorkingProperty.SetValue(huWorking, tmp1Value);
        //                    };
        //                }
        //                else
        //                {
        //                    if (tmp1Value != null)
        //                    {
        //                        throw new Exception($"{tmp1Property.Name} was not found in HU_WORKING");
        //                    }
        //                }
        //            });

        //            huWorking.CREATED_DATE = now;
        //            huWorking.CREATED_BY = request.XlsxSid;


        //            var get_employee =    _dbContext.HuEmployees
        //                                    .Where(x => x.PROFILE_ID == tmpHuWorking.PROFILE_ID)
        //                                    .First();

        //            huWorking.EMPLOYEE_ID = get_employee.ID;
        //            huWorking.ORG_ID = get_employee.ORG_ID;
        //            huWorking.POSITION_ID = get_employee.POSITION_ID;


        //            var get_id_wait_approve = _dbContext.SysOtherLists.Where(x => x.CODE == "CD").Select(x => x.ID).First();

        //            huWorking.STATUS_ID = get_id_wait_approve;


        //            // thiết lập cứng cái IS_WAGE = -1
        //            huWorking.IS_WAGE = -1;


        //            var get_sign_id = _dbContext.HuEmployees
        //                                    .Where(x => x.PROFILE_ID == tmpHuWorking.SIGN_PROFILE_ID)
        //                                    .Select(x => x.ID)
        //                                    .First();

        //            huWorking.SIGN_ID = get_sign_id;


        //            huWorking.IS_BHXH = -1;
        //            huWorking.IS_BHYT = -1;
        //            huWorking.IS_BHTN = -1;
        //            huWorking.IS_BHTNLD_BNN = -1;


        //            var get_position_of_signer = (from item in _dbContext.HuEmployees.Where(x => x.ID == get_sign_id)
        //                                          from reference_1 in _dbContext.HuPositions.Where(x => x.ID == item.POSITION_ID)
        //                                          select reference_1.NAME).First();

        //            huWorking.SIGNER_POSITION = get_position_of_signer;


        //            // tạo bản ghi mới vào bảng chính
        //            await _dbContext.HuWorkings.AddAsync(huWorking);
        //            await _dbContext.SaveChangesAsync();


        //            // lấy luôn cái bản ghi vừa tạo
        //            var get_record = _dbContext.HuWorkings.OrderByDescending(x => x.ID).Take(1);
        //        }


        //        // Clear tmp
        //        _dbContext.HuWorkingHslPcImports.RemoveRange(tmp1);
        //        _dbContext.SaveChanges();

        //        //_uow.Commit();
        //        return new() { InnerBody = true };
        //    }
        //    catch (Exception ex)
        //    {
        //        _uow.Rollback();
        //        return new() { ErrorType = EnumErrorType.UNCATCHABLE, StatusCode = EnumStatusCode.StatusCode500, MessageCode = ex.Message };
        //    }
        //}



        public async Task<FormatedResponse> Save(ImportQueryListBaseDTO request)
        {
            _uow.CreateTransaction();
            try
            {

                var now = DateTime.UtcNow;

                var tmp1 = await _dbContext.HuWorkingHslPcImports.Where(x => x.XLSX_USER_ID == request.XlsxSid && x.XLSX_EX_CODE == request.XlsxExCode && x.XLSX_SESSION == request.XlsxSession).ToListAsync();
                var tmp2 = await _dbContext.HuWorkingAllowImports.Where(x => x.XLSX_USER_ID == request.XlsxSid && x.XLSX_EX_CODE == request.XlsxExCode && x.XLSX_SESSION == request.XlsxSession).ToListAsync();

                var tmp1Type = typeof(HU_WORKING_HSL_PC_IMPORT);
                var tmp1Properties = tmp1Type.GetProperties()?
                    .Where(x => !XLSX_COLUMNS.Contains(x.Name))
                    .ToList();
                var tmp2Type = typeof(HU_WORKING_ALLOW_IMPORT);
                var tmp2Properties = tmp2Type.GetProperties()?
                    .Where(x => !XLSX_COLUMNS.Contains(x.Name))
                    .ToList();

                var cvType = typeof(HU_WORKING);
                var cvTypeProperties = cvType.GetProperties().ToList();
                var employeeType = typeof(HU_WORKING_ALLOW);
                var employeeTypeProperties = employeeType.GetProperties().ToList();


                foreach (var tmpCv in tmp1)
                {
                    var obj1 = Activator.CreateInstance(typeof(HU_WORKING)) ?? throw new Exception(CommonMessageCode.ACTIVATOR_CREATE_INSTANCE_RETURNS_NULL);
                    var cv = (HU_WORKING)obj1;

                    // lap qua cac thuoc tinh o bang cha de map du lieu
                    tmp1Properties?.ForEach(tmp1Property =>
                    {
                        // lay gia tri theo thuoc tinh o bang tam
                        var tmp1Value = tmp1Property.GetValue(tmpCv);

                        // lay thuoc tinh o bang chinh
                        var cvProperty = cvTypeProperties.SingleOrDefault(x => x.Name == tmp1Property.Name);

                        // kiem tra thuoc tinh o bang chinh ton tai hay khong
                        if (cvProperty != null)
                        {
                            if (tmp1Value != null)
                            {
                                // gan gia tri cho thuoc tinh o bang chinh
                                cvProperty.SetValue(cv, tmp1Value);
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

                    cv.CREATED_DATE = now;
                    cv.CREATED_BY = request.XlsxSid;


                    var get_employee = _dbContext.HuEmployees.Where(x => x.PROFILE_ID == tmpCv.PROFILE_ID).First();

                    cv.EMPLOYEE_ID = get_employee.ID;
                    cv.ORG_ID = get_employee.ORG_ID;
                    cv.POSITION_ID = get_employee.POSITION_ID;

                    var get_id_wait_approve = _dbContext.SysOtherLists.Where(x => x.CODE == "CD").Select(x => x.ID).First();
                    cv.STATUS_ID = get_id_wait_approve;

                    // thiết lập cứng cái IS_WAGE = -1
                    cv.IS_WAGE = -1;

                    var get_sign_id = _dbContext.HuEmployees.Where(x => x.PROFILE_ID == tmpCv.SIGN_PROFILE_ID).Select(x => x.ID).First();
                    cv.SIGN_ID = get_sign_id;

                    cv.IS_BHXH = -1;
                    cv.IS_BHYT = -1;
                    cv.IS_BHTN = -1;
                    cv.IS_BHTNLD_BNN = -1;


                    var get_position_of_signer = (from item in _dbContext.HuEmployees.Where(x => x.ID == get_sign_id)
                                                  from reference_1 in _dbContext.HuPositions.Where(x => x.ID == item.POSITION_ID)
                                                  select reference_1.NAME).First();
                    cv.SIGNER_POSITION = get_position_of_signer;


                    // thêm mới hồ sơ lương
                    _dbContext.HuWorkings.Add(cv);
                    _dbContext.SaveChanges();



                    // lay ra ID tu bang cha
                    var newProfileId = cv.ID;


                    // lay ra doi tuong tuong ung o bang tam
                    var tmpEmployee = tmp2.Where(x => x.XLSX_ROW == tmpCv.XLSX_ROW).FirstOrDefault();

                    if (tmpEmployee != null)
                    {
                        // gan ID
                        tmpEmployee.WORKING_ID = newProfileId;
                    }

                    var obj2 = Activator.CreateInstance(typeof(HU_WORKING_ALLOW)) ?? throw new Exception(CommonMessageCode.ACTIVATOR_CREATE_INSTANCE_RETURNS_NULL);
                    var employee = (HU_WORKING_ALLOW)obj2;


                    if (tmpEmployee != null)
                    {
                        // lap qua cac thuoc tinh o bang con de map du lieu
                        tmp2Properties?.ForEach(tmp2Property =>
                        {
                            var tmp2Value = tmp2Property.GetValue(tmpEmployee);
                            var employeeProperty = employeeTypeProperties.Single(x => x.Name == tmp2Property.Name);
                            if (tmp2Value != null)
                            {
                                employeeProperty.SetValue(employee, tmp2Value);
                            };
                        });
                    }
                    

                    //employee.CREATED_DATE = now;
                    //employee.CREATED_BY = request.XlsxSid;


                    // thu được employee
                    if (employee.ALLOWANCE_ID1 != null)
                    {
                        HU_WORKING_ALLOW record = new HU_WORKING_ALLOW()
                        {
                            //ID = 0,
                            WORKING_ID = employee.WORKING_ID,

                            ALLOWANCE_ID = (long)employee.ALLOWANCE_ID1,
                            COEFFICIENT = employee.COEFFICIENT1,
                            EFFECT_DATE = employee.EFFECT_DATE1,
                            EXPIRE_DATE = employee.EXPIRE_DATE1
                        };

                        _dbContext.HuWorkingAllows.Add(record);
                    }

                    if (employee.ALLOWANCE_ID2 != null)
                    {
                        HU_WORKING_ALLOW record = new HU_WORKING_ALLOW()
                        {
                            //ID = 0,
                            WORKING_ID = employee.WORKING_ID,

                            ALLOWANCE_ID = (long)employee.ALLOWANCE_ID2,
                            COEFFICIENT = employee.COEFFICIENT2,
                            EFFECT_DATE = employee.EFFECT_DATE2,
                            EXPIRE_DATE = employee.EXPIRE_DATE2,
                        };

                        _dbContext.HuWorkingAllows.Add(record);
                    }

                    if (employee.ALLOWANCE_ID3 != null)
                    {
                        HU_WORKING_ALLOW record = new HU_WORKING_ALLOW()
                        {
                            //ID = 0,
                            WORKING_ID = employee.WORKING_ID,

                            ALLOWANCE_ID = (long)employee.ALLOWANCE_ID3,
                            COEFFICIENT = employee.COEFFICIENT3,
                            EFFECT_DATE = employee.EFFECT_DATE3,
                            EXPIRE_DATE = employee.EXPIRE_DATE3,
                        };

                        _dbContext.HuWorkingAllows.Add(record);
                    }

                    if (employee.ALLOWANCE_ID4 != null)
                    {
                        HU_WORKING_ALLOW record = new HU_WORKING_ALLOW()
                        {
                            //ID = 0,
                            WORKING_ID = employee.WORKING_ID,

                            ALLOWANCE_ID = (long)employee.ALLOWANCE_ID4,
                            COEFFICIENT = employee.COEFFICIENT4,
                            EFFECT_DATE = employee.EFFECT_DATE4,
                            EXPIRE_DATE = employee.EXPIRE_DATE4,
                        };

                        _dbContext.HuWorkingAllows.Add(record);
                    }

                    if (employee.ALLOWANCE_ID5 != null)
                    {
                        HU_WORKING_ALLOW record = new HU_WORKING_ALLOW()
                        {
                            //ID = 0,
                            WORKING_ID = employee.WORKING_ID,

                            ALLOWANCE_ID = (long)employee.ALLOWANCE_ID5,
                            COEFFICIENT = employee.COEFFICIENT5,
                            EFFECT_DATE = employee.EFFECT_DATE5,
                            EXPIRE_DATE = employee.EXPIRE_DATE5,
                        };

                        _dbContext.HuWorkingAllows.Add(record);
                    }



                    // BẮT ĐẦU TÍNH LƯƠNG ĐÓNG BẢO HIỂM
                    // lấy list danh sách hệ số của phụ cấp
                    List<decimal?> coef = new List<decimal?>();
                    if (employee.ALLOWANCE_ID1 != null) coef.Add(employee.COEFFICIENT1);
                    if (employee.ALLOWANCE_ID2 != null) coef.Add(employee.COEFFICIENT2);
                    if (employee.ALLOWANCE_ID3 != null) coef.Add(employee.COEFFICIENT3);
                    if (employee.ALLOWANCE_ID4 != null) coef.Add(employee.COEFFICIENT4);
                    if (employee.ALLOWANCE_ID5 != null) coef.Add(employee.COEFFICIENT5);

                    decimal? sumPCCoef = 0;
                    if (coef.Count() == 0) sumPCCoef = 0;
                    else sumPCCoef = coef.Sum();

                    long? get_ReligionId = (from o in _dbContext.HuOrganizations.Where(x => x.ID == cv.ORG_ID)
                                            from com in _dbContext.HuCompanys.Where(x => x.ID == o.COMPANY_ID).DefaultIfEmpty()
                                            from re in _dbContext.SysOtherLists.Where(x => x.ID == com.REGION_ID).DefaultIfEmpty()
                                            select re.ID).FirstOrDefault();
                    var moneyRegion = _dbContext.InsRegions.Where(x => x.AREA_ID == get_ReligionId && x.EFFECT_DATE <= cv.EFFECT_DATE).OrderByDescending(x => x.EFFECT_DATE).Select(x => x.MONEY).FirstOrDefault();
                    if (moneyRegion == null) moneyRegion = 0;

                    var moneyOMN = (from p in _dbContext.SysOtherLists where p.CODE == "OMN" && p.IS_ACTIVE == true select p.NOTE).FirstOrDefault();
                    if (moneyOMN == null) moneyOMN = "0";

                    if (cv.COEFFICIENT == null) cv.COEFFICIENT = 0;
                    if (cv.COEFFICIENT_DCV == null) cv.COEFFICIENT_DCV = 0;

                    //tinh luong dong bh
                    if (cv.SHORT_TEMP_SALARY == null)
                    {
                        //lam tron so
                        var listFive = new List<string>() { "TBL001", "TBL002", "TBL003", "TBL004", "TBL008" };
                        var listThree = new List<string>() { "TBL005", "TBL006", "TBL007" };

                        var salaryScale = _dbContext.HuSalaryScales.Where(x => x.ID == cv.SALARY_SCALE_ID).FirstOrDefault();

                        cv.SAL_INSU = ((((moneyRegion * cv.COEFFICIENT) + (decimal.Parse(moneyOMN) * cv.COEFFICIENT_DCV)) * cv.SAL_PERCENT) / 100) + (sumPCCoef * moneyRegion);

                        if (salaryScale == null)
                        {
                            return new FormatedResponse()
                            {
                                ErrorType = EnumErrorType.CATCHABLE,
                                MessageCode = "SALARY_SCALE_IS_NOT_EXITS",
                                StatusCode = EnumStatusCode.StatusCode400
                            };
                        }
                        else
                        {
                            if (listFive.Contains(salaryScale.CODE))
                            {
                                cv.SAL_INSU = Math.Round((decimal)cv.SAL_INSU! / 100000) * 100000;
                            }
                            if (listThree.Contains(salaryScale.CODE))
                            {
                                cv.SAL_INSU = Math.Round((decimal)cv.SAL_INSU! / 1000) * 1000;
                            }
                        }
                    }



                    _dbContext.SaveChanges();
                }

                // Clear tmp
                _dbContext.HuWorkingHslPcImports.RemoveRange(tmp1);
                _dbContext.HuWorkingAllowImports.RemoveRange(tmp2);
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
