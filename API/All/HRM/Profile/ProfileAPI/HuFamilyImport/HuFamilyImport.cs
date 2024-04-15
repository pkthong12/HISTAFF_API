using API.All.DbContexts;
using API.All.SYSTEM.CoreAPI.Xlsx;
using API.DTO;
using CORE.DTO;
using CORE.Enum;
using CORE.GenericUOW;
using CORE.StaticConstant;
using Microsoft.AspNetCore.Http;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Information;
using System.Diagnostics;

namespace API.All.HRM.Profile.ProfileAPI.HuFamilyImport
{
    public class HuFamilyImport : IHuFamilyImport
    {
        private readonly GenericUnitOfWork _uow;
        private readonly FullDbContext _dbContext;
        private readonly GenericReducer<HU_FAMILY_IMPORT, HuFamilyImportDTO> _genericReducer;
        private readonly List<string> XLSX_COLUMNS;

        public HuFamilyImport(FullDbContext context, GenericUnitOfWork uow)
        {
            _dbContext = context;
            _uow = uow;
            _genericReducer = new();
            XLSX_COLUMNS = new() { "XLSX_USER_ID", "XLSX_EX_CODE", "XLSX_INSERT_ON", "XLSX_SESSION", "XLSX_FILE_NAME", "XLSX_ROW" };
        }
        public async Task<GenericPhaseTwoListResponse<HuFamilyImportDTO>> SinglePhaseQueryList(GenericQueryListDTO<HuFamilyImportDTO> request)
        {
            /*
                var raw = from a in _dbContext.HuFamilyImports 
                join b in _dbContext.HuEmployeeCvImports 
                on new { a.XLSX_USER_ID, a.XLSX_EX_CODE, a.XLSX_SESSION, a.XLSX_ROW } equals new { b.XLSX_USER_ID, b.XLSX_EX_CODE, b.XLSX_SESSION, b.XLSX_ROW }

             */
            var raw = from a in _dbContext.HuFamilyImports
                      from reference_1 in _dbContext.HuEmployees.Where(x => x.ID == a.EMPLOYEE_ID).DefaultIfEmpty()
                      from reference_2 in _dbContext.HuEmployeeCvs.Where(x => x.ID == reference_1.PROFILE_ID).DefaultIfEmpty()
                      from reference_3 in _dbContext.HuOrganizations.Where(x => x.ID == reference_1.ORG_ID).DefaultIfEmpty()
                      from reference_4 in _dbContext.HuPositions.Where(x => x.ID == reference_1.POSITION_ID).DefaultIfEmpty()
                      from reference_5 in _dbContext.SysOtherLists.Where(x => x.ID == a.GENDER).DefaultIfEmpty()
                      from reference_6 in _dbContext.SysOtherLists.Where(x => x.ID == a.RELATIONSHIP_ID).DefaultIfEmpty()
                      from reference_7 in _dbContext.HuNations.Where(x => x.ID == a.NATIONALITY).DefaultIfEmpty()
                      from reference_8 in _dbContext.HuProvinces.Where(x => x.ID == a.BIRTH_CER_PROVINCE).DefaultIfEmpty()
                      from reference_9 in _dbContext.HuDistricts.Where(x => x.ID == a.BIRTH_CER_DISTRICT).DefaultIfEmpty()
                      from reference_10 in _dbContext.HuWards.Where(x => x.ID == a.BIRTH_CER_WARD).DefaultIfEmpty()
                      select new HuFamilyImportDTO()
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
                          Fullname = a.FULLNAME,
                          RelationshipName = reference_6.NAME,
                          BirthDate = a.BIRTH_DATE,
                          GenderName = reference_5.NAME,
                          PitCode = a.PIT_CODE,
                          SameCompany = a.SAME_COMPANY,
                          IsDead = a.IS_DEAD,
                          IsDeduct = a.IS_DEDUCT,
                          RegistDeductDate = a.REGIST_DEDUCT_DATE,
                          DeductFrom = a.DEDUCT_FROM,
                          DeductTo = a.DEDUCT_TO,
                          IdNo = a.ID_NO,
                          Career = a.CAREER,
                          NationalityName = reference_7.NAME,
                          BirthCerPlace = a.BIRTH_CER_PLACE,
                          BirthCerProvinceName = reference_8.NAME,
                          BirthCerDistrictName = reference_9.NAME,
                          BirthCerWardName = reference_10.NAME,
                          Note= a.NOTE
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

                var tmp1 = await _dbContext.HuFamilyImports
                            .Where(x => x.XLSX_USER_ID == request.XlsxSid
                                    && x.XLSX_EX_CODE == request.XlsxExCode
                                    && x.XLSX_SESSION == request.XlsxSession)
                            .ToListAsync();

                var tmp1Type = typeof(HU_FAMILY_IMPORT);
                var tmp1Properties = tmp1Type.GetProperties()?
                    .Where(x => !XLSX_COLUMNS.Contains(x.Name))
                    .ToList();


                var hu_family_Type = typeof(HU_FAMILY);
                var hu_family_TypeProperties = hu_family_Type.GetProperties().ToList();


                foreach (var tmpHuFamily in tmp1)
                {
                    var obj1 = Activator.CreateInstance(typeof(HU_FAMILY)) ?? throw new Exception(CommonMessageCode.ACTIVATOR_CREATE_INSTANCE_RETURNS_NULL);
                    var huFamily = (HU_FAMILY)obj1;

                    tmp1Properties?.ForEach(tmp1Property =>
                    {
                        var tmp1Value = tmp1Property.GetValue(tmpHuFamily);
                        var huFamilyProperty = hu_family_TypeProperties.SingleOrDefault(x => x.Name == tmp1Property.Name);
                        if (huFamilyProperty != null)
                        {
                            if (tmp1Value != null)
                            {
                                huFamilyProperty.SetValue(huFamily, tmp1Value);
                            };
                        }
                        else
                        {
                            if (tmp1Value != null)
                            {
                                throw new Exception($"{tmp1Property.Name} was not found in HU_FAMILY");
                            }
                        }
                    });

                    huFamily.CREATED_DATE = now;
                    huFamily.CREATED_BY = request.XlsxSid;


                    // kiểm tra bắt buộc nhập mã nhân viên
                    if (huFamily.EMPLOYEE_ID == null)
                    {
                        return new FormatedResponse()
                        {
                            ErrorType = EnumErrorType.CATCHABLE,
                            MessageCode = CommonMessageCode.YOU_MUST_ENTER_EMPLOYEE_CODE,
                            StatusCode = EnumStatusCode.StatusCode400
                        };
                    }


                    // kiểm tra bắt buộc nhập họ và tên người thân
                    if (huFamily.FULLNAME == null || huFamily.FULLNAME == "")
                    {
                        return new FormatedResponse()
                        {
                            ErrorType = EnumErrorType.CATCHABLE,
                            MessageCode = CommonMessageCode.YOU_MUST_ENTER_FULLNAME_FAMILY,
                            StatusCode = EnumStatusCode.StatusCode400
                        };
                    }


                    // kiểm tra bắt buộc nhập quan hệ
                    if (huFamily.RELATIONSHIP_ID == null)
                    {
                        return new FormatedResponse()
                        {
                            ErrorType = EnumErrorType.CATCHABLE,
                            MessageCode = CommonMessageCode.YOU_MUST_ENTER_RELATIONSHIP,
                            StatusCode = EnumStatusCode.StatusCode400
                        };
                    }


                    // kiểm tra bắt buộc nhập ngày sinh
                    if (huFamily.BIRTH_DATE == null)
                    {
                        return new FormatedResponse()
                        {
                            ErrorType = EnumErrorType.CATCHABLE,
                            MessageCode = CommonMessageCode.YOU_MUST_ENTER_BIRTH_DATE_FAMILY,
                            StatusCode = EnumStatusCode.StatusCode400
                        };
                    }


                    // kiểm tra thuộc đối tượng giảm trừ
                    if (huFamily.IS_DEDUCT == true)
                    {
                        if (huFamily.REGIST_DEDUCT_DATE == null)
                        {
                            return new FormatedResponse()
                            {
                                ErrorType = EnumErrorType.CATCHABLE,
                                MessageCode = CommonMessageCode.YOU_MUST_ENTER_REGIST_DEDUCT_DATE,
                                StatusCode = EnumStatusCode.StatusCode400
                            };
                        }

                        if (huFamily.DEDUCT_FROM == null)
                        {
                            return new FormatedResponse()
                            {
                                ErrorType = EnumErrorType.CATCHABLE,
                                MessageCode = CommonMessageCode.YOU_MUST_ENTER_DEDUCT_FROM,
                                StatusCode = EnumStatusCode.StatusCode400
                            };
                        }
                    }
                    else
                    {
                        huFamily.REGIST_DEDUCT_DATE = null;
                        huFamily.DEDUCT_FROM = null;
                        huFamily.DEDUCT_TO = null;
                    }


                    _dbContext.HuFamilys.Add(huFamily);
                    _dbContext.SaveChanges();
                }


                // Clear tmp
                _dbContext.HuFamilyImports.RemoveRange(tmp1);
                _dbContext.SaveChanges();

                _uow.Commit();

                var get_new_record = _dbContext.HuFamilys.OrderByDescending(x => x.ID).FirstOrDefault();

                return new() {
                    StatusCode = EnumStatusCode.StatusCode200,
                    MessageCode = CommonMessageCode.IMPORT_FAMILY_SUCCESS,
                    InnerBody = get_new_record
                };
            }
            catch (Exception ex)
            {
                _uow.Rollback();
                return new() { ErrorType = EnumErrorType.UNCATCHABLE, StatusCode = EnumStatusCode.StatusCode500, MessageCode = ex.Message };
            }
        }
    }
}
