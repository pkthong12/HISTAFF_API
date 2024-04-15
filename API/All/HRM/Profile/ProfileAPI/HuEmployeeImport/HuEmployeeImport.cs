using API.All.DbContexts;
using API.All.SYSTEM.CoreAPI.Xlsx;
using API.DTO;
using CORE.DTO;
using CORE.Enum;
using CORE.GenericUOW;
using CORE.StaticConstant;
using System.Diagnostics;

namespace API.All.HRM.Profile.ProfileAPI.HuEmployeeImport
{
    public class HuEmployeeImport : IHuEmployeeImport
    {
        private readonly GenericUnitOfWork _uow;
        private readonly FullDbContext _dbContext;
        private readonly GenericReducer<HU_EMPLOYEE_CV_IMPORT, HuEmployeeImportDTO> _genericReducer;
        private readonly List<string> XLSX_COLUMNS;

        public HuEmployeeImport(FullDbContext context, GenericUnitOfWork uow)
        {
            _dbContext = context;
            _uow = uow;
            _genericReducer = new();
            XLSX_COLUMNS = new() { "XLSX_USER_ID", "XLSX_EX_CODE", "XLSX_INSERT_ON", "XLSX_SESSION", "XLSX_FILE_NAME", "XLSX_ROW" };
        }
        public async Task<GenericPhaseTwoListResponse<HuEmployeeImportDTO>> SinglePhaseQueryList(GenericQueryListDTO<HuEmployeeImportDTO> request)
        {
            /*
                var raw = from a in _dbContext.HuEmployeeImports 
                join b in _dbContext.HuEmployeeCvImports 
                on new { a.XLSX_USER_ID, a.XLSX_EX_CODE, a.XLSX_SESSION, a.XLSX_ROW } equals new { b.XLSX_USER_ID, b.XLSX_EX_CODE, b.XLSX_SESSION, b.XLSX_ROW }

             */
            var raw = from a in _dbContext.HuEmployeeImports
                      from b in _dbContext.HuEmployeeCvImports.Where(b => b.XLSX_USER_ID == a.XLSX_USER_ID && b.XLSX_EX_CODE == a.XLSX_EX_CODE && b.XLSX_SESSION == a.XLSX_SESSION && b.XLSX_ROW == a.XLSX_ROW).DefaultIfEmpty()
                      from p in _dbContext.HuPositions.Where(p => p.ID == a.POSITION_ID).DefaultIfEmpty()
                      from o in _dbContext.HuOrganizations.Where(o => o.ID == p.ORG_ID).DefaultIfEmpty()
                      from h1 in _dbContext.SysOtherLists.Where(h => h.ID == a.EMPLOYEE_OBJECT_ID).DefaultIfEmpty()
                      from h2 in _dbContext.SysOtherLists.Where(h => h.ID == b.GENDER_ID).DefaultIfEmpty()
                      from h3 in _dbContext.SysOtherLists.Where(h => h.ID == b.NATIONALITY_ID).DefaultIfEmpty()
                      from h4 in _dbContext.SysOtherLists.Where(h => h.ID == b.NATIVE_ID).DefaultIfEmpty()
                      from h5 in _dbContext.SysOtherLists.Where(h => h.ID == b.MARITAL_STATUS_ID).DefaultIfEmpty()
                      from v in _dbContext.HuProvinces.Where(v => v.ID == b.ID_PLACE).DefaultIfEmpty()
                      from t1 in _dbContext.HuProvinces.Where(t1 => t1.ID == b.PROVINCE_ID).DefaultIfEmpty()
                      from q1 in _dbContext.HuDistricts.Where(q1 => q1.ID == b.DISTRICT_ID).DefaultIfEmpty()
                      from x1 in _dbContext.HuWards.Where(x1 => x1.ID == b.WARD_ID).DefaultIfEmpty()
                      from t2 in _dbContext.HuProvinces.Where(t2 => t2.ID == b.CUR_PROVINCE_ID).DefaultIfEmpty()
                      from q2 in _dbContext.HuDistricts.Where(q2 => q2.ID == b.CUR_DISTRICT_ID).DefaultIfEmpty()
                      from x2 in _dbContext.HuWards.Where(x2 => x2.ID == b.CUR_WARD_ID).DefaultIfEmpty()
                      from k in _dbContext.HuBanks.Where(k => k.ID == b.BANK_ID).DefaultIfEmpty()
                      from r in _dbContext.HuBankBranchs.Where(r => r.ID == b.BANK_BRANCH).DefaultIfEmpty()
                      from tax in _dbContext.HuProvinces.Where(x => x.ID == b.TAX_CODE_ADDRESS).DefaultIfEmpty()
                      from region in _dbContext.InsRegions.Where(x => x.ID == a.INSURENCE_AREA).DefaultIfEmpty()
                      from wh in _dbContext.InsWhereHealThs.Where(x => x.ID == b.INS_WHEREHEALTH_ID).DefaultIfEmpty()
                      from elevel in _dbContext.SysOtherLists.Where(x =>x.ID == b.EDUCATION_LEVEL_ID).DefaultIfEmpty()
                      from computerSkill in _dbContext.SysOtherLists.Where(x => x.ID == b.COMPUTER_SKILL_ID).DefaultIfEmpty()
                      from license in _dbContext.SysOtherLists.Where(x => x.ID == b.LICENSE_ID).DefaultIfEmpty()
                      from b2 in _dbContext.HuBanks.Where(x => x.ID == b.BANK_ID_2).DefaultIfEmpty()
                      from br2 in _dbContext.HuBankBranchs.Where(x => x.ID == b.BANK_BRANCH_2).DefaultIfEmpty()
                      from idAddress in _dbContext.HuProvinces.Where(x => x.ID == b.ID_PLACE).DefaultIfEmpty()



                      select new HuEmployeeImportDTO()
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
                          Code = a.CODE,
                          OrgName = o.NAME,
                          PositionName = p.NAME,
                          ItimeId = a.ITIME_ID,
                          EmployeeObjectName = h1.NAME,

                          Fullname = b.FULL_NAME,
                          OtherName = b.OTHER_NAME,
                          GenderName = h2.NAME,
                          NationalityName = h3.NAME,
                          Domicile = b.DOMICILE,
                          NativeName = h4.NAME,
                          BirthDate = b.BIRTH_DATE,
                          BirthPlace = b.BIRTH_PLACE,
                          BirthRegisAddress = b.BIRTH_REGIS_ADDRESS,
                          ProvinceName = t1.NAME,
                          DistrictName = q1.NAME,
                          WardName = x1.NAME,
                          Address = b.ADDRESS,
                          CurProvinceName = t2.NAME,
                          CurDistrictName = q2.NAME,
                          CurWardName = x2.NAME,
                          CurAddress = b.CUR_ADDRESS,
                          MaritalStatusName = h5.NAME,
                          IdNo = b.ID_NO,
                          IdDate = b.ID_DATE,
                          IdPlaceName = idAddress.NAME,
                          PassNo = b.PASS_NO,
                          PassDate = b.PASS_DATE,
                          PassPlace = b.PASS_PLACE,
                          PassExpire = b.PASS_EXPIRE,
                          BankName = k.NAME,
                          BankBranchName = r.NAME,
                          BankNo = b.BANK_NO,
                          BankName2 = b2.NAME,
                          BankBranchName2 = br2.NAME,
                          BankNo2 = b.BANK_NO_2,
                          TaxCode = b.TAX_CODE,
                          TaxCodeDate = b.TAX_CODE_DATE,
                          TaxCodeAddressName = tax.NAME,
                          BloodGroup = b.BLOOD_GROUP,
                          Height = b.HEIGHT,
                          Weight = b.WEIGHT,
                          BloodPressure = b.BLOOD_PRESSURE,
                          HealthType = b.HEALTH_TYPE,
                          LeftEye = b.LEFT_EYE,
                          RightEye = b.RIGHT_EYE,
                          Heart = b.HEART,
                          ExaminationDate = b.EXAMINATION_DATE,
                          HealthNotes = b.HEALTH_NOTES,
                          VisaNo = b.VISA_NO,
                          VisaDate = b.VISA_DATE,
                          VisaExpire = b.VISA_EXPIRE,
                          VisaPlace = b.VISA_PLACE,
                          InsurenceAreaName = region.REGION_CODE,
                          InsurenceNumber = b.INSURENCE_NUMBER,
                          InsCardNumber = b.INS_CARD_NUMBER,
                          InsWhereHealthName = wh.NAME_VN,
                          FamilyMember = b.FAMILY_MEMBER,
                          FamilyPolicy = b.FAMILY_POLICY,
                          SchoolOfWork = b.SCHOOL_OF_WORK,
                          Veterans = b.VETERANS,
                          TitleConferred = b.TITLE_CONFERRED,
                          PrisonNote = b.PRISON_NOTE,
                          FamilyDetail = b.FAMILY_DETAIL,
                          YellowFlag = b.YELLOW_FLAG,
                          Relations = b.RELATIONS,
                          YouthSaveNationPosition = b.YOUTH_SAVE_NATION_POSITION,
                          YouthSaveNationDate = b.YOUTH_SAVE_NATION_DATE,
                          YouthSaveNationAddress = b.YOUTH_SAVE_NATION_ADDRESS,
                          IsUnionist = b.IS_UNIONIST,
                          UnionistAddress = b.UNIONIST_ADDRESS,
                          UnionistDate = b.UNIONIST_DATE,
                          UnionistPosition = b.UNIONIST_POSITION,
                          IsJoinYouthGroup = b.IS_JOIN_YOUTH_GROUP,
                          YouthGroupPosition = b.YOUTH_GROUP_POSITION,
                          YouthGroupAddress = b.YOUTH_GROUP_ADDRESS,
                          IsMember = b.IS_MEMBER,
                          MemberPosition = b.MEMBER_POSITION,
                          MemberAddress = b.MEMBER_ADDRESS,
                          MemberDate = b.MEMBER_DATE,
                          MemberOfficalDate = b.MEMBER_OFFICAL_DATE,
                          LivingCell = b.LIVING_CELL,
                          CardNumber = b.CARD_NUMBER,
                          ResumeNumber = b.RESUME_NUMBER,
                          EnlistmentDate = b.ENLISTMENT_DATE,
                          DisChargeDate = b.DISCHARGE_DATE,
                          HighestMilitaryPosition = b.HIGHEST_MILITARY_POSITION,
                          VateransPosition = b.VATERANS_POSITION,
                          VateransMemberDate = b.VATERANS_MEMBER_DATE,
                          VateransAddress = b.VATERANS_ADDRESS,
                          PoliticalTheory = b.POLITICAL_THEORY,
                          CurrentPartyCommittee = b.CURRENT_PARTY_COMMITTEE,
                          PartytimePartyCommittee = b.PARTYTIME_PARTY_COMMITTEE,
                          EducationLevelName = elevel.NAME,
                          ComputerSkillName = computerSkill.NAME,
                          License = license.NAME,   
                          HouseholdNumber = b.HOUSEHOLD_NUMBER,
                          HouseholdCode = b.HOUSEHOLD_CODE,
                          MobilePhone = b.MOBILE_PHONE,
                          MobilePhoneLand = b.MOBILE_PHONE_LAND,
                          WorkEmail = b.WORK_EMAIL,
                          Email = b.EMAIL,
                          MainIncome = b.MAIN_INCOME,
                          OtherSources = b.OTHER_SOURCES,
                          LandGranted = b.LAND_GRANTED,
                          TaxGrantedHouse = b.TAX_GRANTED_HOUSE,
                          TotalArea = b.TOTAL_AREA,
                          SelfPurcharseLand = b.SELF_PURCHASE_LAND,
                          SelfBuildHouse = b.SELF_BUILD_HOUSE,
                          LandForProduction = b.LAND_FOR_PRODUCTION,
                          AdditionalInformation = b.ADDITIONAL_INFOMATION,


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

                var tmp1 = await _dbContext.HuEmployeeCvImports.Where(x => x.XLSX_USER_ID == request.XlsxSid && x.XLSX_EX_CODE == request.XlsxExCode && x.XLSX_SESSION == request.XlsxSession).ToListAsync();
                var tmp2 = await _dbContext.HuEmployeeImports.Where(x => x.XLSX_USER_ID == request.XlsxSid && x.XLSX_EX_CODE == request.XlsxExCode && x.XLSX_SESSION == request.XlsxSession).ToListAsync();
                
                //Check identity for employeecv and employee
                if(tmp1.Count > 0 && tmp2.Count > 0)
                {
                    tmp1.ForEach(check =>
                    {
                        if(check.ID_NO == "")
                        {
                            throw new Exception("Cột Căn cước công dân không được để trống");
                        }
                    });
                    tmp2.ForEach(check =>
                    {
                        if(check.CODE == "")
                        {
                            throw new Exception("Mã nhân viên không được để trống");
                        }
                        if(check.ITIME_ID == "")
                        {
                            throw new Exception("Mã chấm công không được để trống");
                        }
                    });
                }
                var tmp1Type = typeof(HU_EMPLOYEE_CV_IMPORT);
                var tmp1Properties = tmp1Type.GetProperties()?
                    .Where(x => !XLSX_COLUMNS.Contains(x.Name))
                    .ToList();
                var tmp2Type = typeof(HU_EMPLOYEE_IMPORT);
                var tmp2Properties = tmp2Type.GetProperties()?
                    .Where(x => !XLSX_COLUMNS.Contains(x.Name))
                    .ToList();

                var cvType = typeof(HU_EMPLOYEE_CV);
                var cvTypeProperties = cvType.GetProperties().ToList();
                var employeeType = typeof(HU_EMPLOYEE);
                var employeeTypeProperties = employeeType.GetProperties().ToList();

                tmp1.ForEach(tmpCv =>
                {
                    var obj1 = Activator.CreateInstance(typeof(HU_EMPLOYEE_CV)) ?? throw new Exception(CommonMessageCode.ACTIVATOR_CREATE_INSTANCE_RETURNS_NULL);
                    var cv = (HU_EMPLOYEE_CV)obj1;
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
                        } else
                        {
                            if (tmp1Value != null)
                            {
                                throw new Exception($"{tmp1Property.Name} was not found in HU_EMPLOYEE_CV");
                            }
                        }
                    });

                    cv.CREATED_DATE = now;
                    cv.CREATED_BY = request.XlsxSid;

                    _dbContext.HuEmployeeCvs.Add(cv);
                    _dbContext.SaveChanges();

                    // lay ra ID tu bang cha
                    var newProfileId = cv.ID;

                    // lay ra doi tuong tuong ung o bang tam
                    var tmpEmployee = tmp2.Single(x => x.XLSX_ROW == tmpCv.XLSX_ROW);

                    // gan ID
                    tmpEmployee.PROFILE_ID = newProfileId;

                    var obj2 = Activator.CreateInstance(typeof(HU_EMPLOYEE)) ?? throw new Exception(CommonMessageCode.ACTIVATOR_CREATE_INSTANCE_RETURNS_NULL);
                    var employee = (HU_EMPLOYEE)obj2;

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

                    employee.CREATED_DATE = now;
                    employee.CREATED_BY = request.XlsxSid;

                    _dbContext.HuEmployees.Add(employee);
                    _dbContext.SaveChanges();

                });

                // Clear tmp
                _dbContext.HuEmployeeCvImports.RemoveRange(tmp1);
                _dbContext.HuEmployeeImports.RemoveRange(tmp2);
                _dbContext.SaveChanges();

                _uow.Commit();
                return new() { InnerBody = true };

            } catch (Exception ex)
            {
                _uow.Rollback();
                return new() { ErrorType = EnumErrorType.UNCATCHABLE, StatusCode = EnumStatusCode.StatusCode500, MessageCode = ex.Message };
            }
        }
    }
}
