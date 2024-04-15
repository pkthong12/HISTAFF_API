using API.All.DbContexts;
using API.All.SYSTEM.CoreAPI.Xlsx;
using API.DTO;
using Common.Extensions;
using CORE.DTO;
using CORE.Enum;
using CORE.GenericUOW;
using CORE.StaticConstant;
using DocumentFormat.OpenXml.ExtendedProperties;
using System.Diagnostics;
using System.Linq.Dynamic.Core;

namespace API.All.HRM.Profile.ProfileAPI.InsInformationImport
{
    public class InsInformationImport : IInsInformationImport
    {
        private readonly GenericUnitOfWork _uow;
        private readonly FullDbContext _dbContext;
        private readonly GenericReducer<INS_INFORMATION_IMPORT, InsInformationImportDTO> _genericReducer;
        private readonly List<string> XLSX_COLUMNS;

        public InsInformationImport(FullDbContext context, GenericUnitOfWork uow)
        {
            _dbContext = context;
            _uow = uow;
            _genericReducer = new();
            XLSX_COLUMNS = new() { "XLSX_USER_ID", "XLSX_EX_CODE", "XLSX_INSERT_ON", "XLSX_SESSION", "XLSX_FILE_NAME", "XLSX_ROW" };
        }
        public async Task<GenericPhaseTwoListResponse<InsInformationImportDTO>> SinglePhaseQueryList(GenericQueryListDTO<InsInformationImportDTO> request)
        {
            /*
                var raw = from a in _dbContext.InsInformationImports 
                join b in _dbContext.HuEmployeeCvImports 
                on new { a.XLSX_USER_ID, a.XLSX_EX_CODE, a.XLSX_SESSION, a.XLSX_ROW } equals new { b.XLSX_USER_ID, b.XLSX_EX_CODE, b.XLSX_SESSION, b.XLSX_ROW }

             */

            var get_id_approve = (from record_sub in _dbContext.SysOtherListTypes.Where(x => x.CODE == "STATUS")
                                  from record_main in _dbContext.SysOtherLists.Where(x => x.CODE == "DD" && x.TYPE_ID == record_sub.ID)
                                  select record_main.ID
                                  ).First();


            var raw = (from a in _dbContext.InsInformationImports
                      from reference_1 in _dbContext.HuEmployees.Where(x => x.ID == a.EMPLOYEE_ID).DefaultIfEmpty()
                      from reference_2 in _dbContext.HuEmployeeCvs.Where(x => x.ID == reference_1.PROFILE_ID).DefaultIfEmpty()
                      from reference_3 in _dbContext.HuOrganizations.Where(x => x.ID == reference_1.ORG_ID).DefaultIfEmpty()
                      from reference_4 in _dbContext.HuPositions.Where(x => x.ID == reference_1.POSITION_ID).DefaultIfEmpty()
                      from reference_5 in _dbContext.HuProvinces.Where(x => x.ID == reference_2.ID_PLACE).DefaultIfEmpty()
                      
                      from reference_6 in _dbContext.HuCompanys.Where(x => x.ID == reference_3.COMPANY_ID).DefaultIfEmpty()
                      from reference_7 in _dbContext.SysOtherLists.Where(x => x.ID == reference_6.INS_UNIT).DefaultIfEmpty()

                      from reference_8 in _dbContext.HuContracts
                      .Where(x => x.EMPLOYEE_ID == reference_1.ID && x.STATUS_ID == get_id_approve)
                      .OrderBy(x => x.START_DATE)
                      .Take(1)
                      .DefaultIfEmpty()

                      from reference_9 in _dbContext.HuWorkings.Where(x => x.ID == reference_8.WORKING_ID).DefaultIfEmpty()

                      from reference_10 in _dbContext.InsSpecifiedObjectss
                      .Where(x => x.EFFECTIVE_DATE <= reference_8.START_DATE)
                      .OrderByDescending(x => x.EFFECTIVE_DATE)
                      .Take(1)
                      .DefaultIfEmpty()

                      from reference_11 in _dbContext.HuContractTypes.Where(x => x.ID == reference_8.CONTRACT_TYPE_ID).DefaultIfEmpty()
                      from reference_12 in _dbContext.SysOtherLists.Where(x => x.ID == a.BHXH_STATUS_ID).DefaultIfEmpty()
                      from reference_13 in _dbContext.SysOtherLists.Where(x => x.ID == a.BHYT_STATUS_ID).DefaultIfEmpty()
                      from reference_14 in _dbContext.InsWhereHealThs.Where(x => x.ID == a.BHYT_WHEREHEALTH_ID).DefaultIfEmpty()

                      select new InsInformationImportDTO()
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
                          IdNo = reference_2.ID_NO,
                          IdDateStr = (reference_2.ID_DATE != null) ? reference_2.ID_DATE.Value.ToString("dd/MM/yyyy") : "",
                          AddressIdentity = reference_5.NAME,
                          BirthDateStr = (reference_2.BIRTH_DATE != null) ? reference_2.BIRTH_DATE.Value.ToString("dd/MM/yyyy") : "",
                          BirthPlace = reference_2.BIRTH_PLACE,
                          Contact = reference_2.MOBILE_PHONE,
                          SeniorityInsurance = a.SENIORITY_INSURANCE,
                          SeniorityInsuranceInCompany = (reference_1.JOIN_DATE_STATE == null) ? 0 : (int)Math.Round((decimal)((DateTime.Now - reference_1.JOIN_DATE_STATE.Value).Days / 30)),
                          Company = reference_7.NAME,
                          SalaryBhxhYt = (reference_9.SAL_INSU == null) ? 0 : ((reference_10.UI == null) ? 0 : (reference_10.SI_HI > reference_9.SAL_INSU ? (double)reference_9.SAL_INSU : (double)reference_10.SI_HI)),
                          SalaryBhxhYtStr = (reference_9.SAL_INSU == null) ? "0" : ((reference_10.UI == null) ? "0" : (reference_10.SI_HI > reference_9.SAL_INSU ? ((double)reference_9.SAL_INSU).ToString("N0").Replace(".", ",") : ((double)reference_10.SI_HI).ToString("N0").Replace(".", ","))),
                          ListInsuranceStr = "",

                          IsBhxh = reference_11.IS_BHXH.HasValue,
                          IsBhyt = reference_11.IS_BHYT.HasValue,
                          IsBhtn = reference_11.IS_BHTN.HasValue,
                          IsBhtnldBnn = reference_11.IS_BHTNLD_BNN.HasValue,

                          BhxhFromDateString = (a.BHXH_FROM_DATE != null) ? a.BHXH_FROM_DATE.Value.ToString("MM/yyyy") : "",
                          BhxhToDateString = (a.BHXH_TO_DATE != null) ? a.BHXH_TO_DATE.Value.ToString("MM/yyyy") : "",
                          BhxhNo = reference_2.INSURENCE_NUMBER,
                          BhxhStatusString = reference_12.NAME,
                          BhxhSuppliedDateStr = (a.BHXH_SUPPLIED_DATE != null) ? a.BHXH_SUPPLIED_DATE.Value.ToString("dd/MM/yyyy") : "",
                          BhxhGrantDateStr = (a.BHXH_GRANT_DATE != null) ? a.BHXH_GRANT_DATE.Value.ToString("dd/MM/yyyy") : "",
                          BhxhDeliverer = a.BHXH_DELIVERER,
                          BhxhStorageNumber = a.BHXH_STORAGE_NUMBER,
                          BhxhReimbursementDateStr = (a.BHXH_REIMBURSEMENT_DATE != null) ? a.BHXH_REIMBURSEMENT_DATE.Value.ToString("dd/MM/yyyy") : "",
                          BhxhReceiver = a.BHXH_RECEIVER,
                          BhxhNote = a.BHXH_NOTE,

                          BhytFromDateString = (a.BHYT_FROM_DATE != null) ? a.BHYT_FROM_DATE.Value.ToString("MM/yyyy") : "",
                          BhytToDateString = (a.BHYT_TO_DATE != null) ? a.BHYT_TO_DATE.Value.ToString("MM/yyyy") : "",
                          BhytNo = reference_2.INS_CARD_NUMBER,
                          BhytStatusString = reference_13.NAME,
                          BhytEffectDateStr = (a.BHYT_EFFECT_DATE != null) ? a.BHYT_EFFECT_DATE.Value.ToString("dd/MM/yyyy") : "",
                          BhytExpireDateStr = (a.BHYT_EXPIRE_DATE != null) ? a.BHYT_EXPIRE_DATE.Value.ToString("dd/MM/yyyy") : "",
                          BhytWherehealthString = reference_14.NAME_VN,
                          BhytReceivedDateStr = (a.BHYT_RECEIVED_DATE != null) ? a.BHYT_RECEIVED_DATE.Value.ToString("dd/MM/yyyy") : "",
                          BhytReceiver = a.BHYT_RECEIVER,
                          BhytReimbursementDateStr = (a.BHYT_REIMBURSEMENT_DATE != null) ? a.BHYT_REIMBURSEMENT_DATE.Value.ToString("dd/MM/yyyy") : "",

                          BhtnFromDateString = (a.BHTN_FROM_DATE != null) ? a.BHTN_FROM_DATE.Value.ToString("MM/yyyy") : "",
                          BhtnToDateString = (a.BHTN_TO_DATE != null) ? a.BHTN_TO_DATE.Value.ToString("MM/yyyy") : "",

                          BhtnldBnnFromDateString = (a.BHTNLD_BNN_FROM_DATE != null) ? a.BHTNLD_BNN_FROM_DATE.Value.ToString("MM/yyyy") : "",
                          BhtnldBnnToDateString = (a.BHTNLD_BNN_TO_DATE != null) ? a.BHTNLD_BNN_TO_DATE.Value.ToString("MM/yyyy") : "",
                      })
                      .ToList()
                      .Select(item =>
                      {
                          List<string> lstcheckInsItems = new List<string>();

                          if (item.IsBhxh != null && item.IsBhxh == true)
                          {
                              lstcheckInsItems.Add("Bảo hiểm xã hội");
                          }

                          if (item.IsBhyt != null && item.IsBhyt == true)
                          {
                              lstcheckInsItems.Add("Bảo hiểm y tế");
                          }

                          if (item.IsBhtn != null && item.IsBhtn == true)
                          {
                              lstcheckInsItems.Add("Bảo hiểm thất nghiệp");
                          }

                          if (item.IsBhtnldBnn != null && item.IsBhtnldBnn == true)
                          {
                              lstcheckInsItems.Add("Bảo hiểm TNLD - BNN");
                          }

                          item.ListInsuranceStr = string.Join(", ", lstcheckInsItems);

                          return item;
                      });


            var response = await _genericReducer.SinglePhaseReduce(raw.AsQueryable(), request);
            return response;
        }

        public async Task<FormatedResponse> Save(ImportQueryListBaseDTO request)
        {
            _uow.CreateTransaction();
            try
            {

                var now = DateTime.UtcNow;

                var tmp1 = await _dbContext.InsInformationImports
                            .Where(x => x.XLSX_USER_ID == request.XlsxSid
                                    && x.XLSX_EX_CODE == request.XlsxExCode
                                    && x.XLSX_SESSION == request.XlsxSession)
                            .ToListAsync();

                var tmp1Type = typeof(INS_INFORMATION_IMPORT);
                var tmp1Properties = tmp1Type.GetProperties()?
                    .Where(x => !XLSX_COLUMNS.Contains(x.Name))
                    .ToList();


                var INS_INFORMATION_Type = typeof(INS_INFORMATION);
                var INS_INFORMATION_TypeProperties = INS_INFORMATION_Type.GetProperties().ToList();


                foreach (var tmpInsInformation in tmp1)
                {
                    // kiểm tra xem có tồn tại thông tin bảo hiểm cũ không
                    // nếu có thì báo lỗi
                    int count_exist_record = _dbContext.InsInformations
                                                .Where(x => x.EMPLOYEE_ID == tmpInsInformation.EMPLOYEE_ID)
                                                .Count();

                    if (count_exist_record > 0)
                    {
                        var record = _dbContext.HuEmployees.Where(x => x.ID == tmpInsInformation.EMPLOYEE_ID).First();

                        return new FormatedResponse()
                        {
                            ErrorType = EnumErrorType.CATCHABLE,
                            MessageCode = "Đã tồn tại thông tin bảo hiểm của nhân viên có mã " + record.CODE,
                            InnerBody = record,
                            StatusCode = EnumStatusCode.StatusCode400
                        };
                    }


                    var obj1 = Activator.CreateInstance(typeof(INS_INFORMATION)) ?? throw new Exception(CommonMessageCode.ACTIVATOR_CREATE_INSTANCE_RETURNS_NULL);
                    var InsInformation = (INS_INFORMATION)obj1;

                    tmp1Properties?.ForEach(tmp1Property =>
                    {
                        var tmp1Value = tmp1Property.GetValue(tmpInsInformation);
                        var InsInformationProperty = INS_INFORMATION_TypeProperties.SingleOrDefault(x => x.Name == tmp1Property.Name);
                        if (InsInformationProperty != null)
                        {
                            if (tmp1Value != null)
                            {
                                InsInformationProperty.SetValue(InsInformation, tmp1Value);
                            };
                        }
                        else
                        {
                            if (tmp1Value != null)
                            {
                                throw new Exception($"{tmp1Property.Name} was not found in INS_INFORMATION");
                            }
                        }
                    });

                    InsInformation.CREATED_DATE = now;
                    InsInformation.CREATED_BY = request.XlsxSid;

                    // kiểm tra từ tháng (BHXH)
                    if (InsInformation.BHXH_FROM_DATE == null)
                    {
                        return new FormatedResponse()
                        {
                            ErrorType = EnumErrorType.CATCHABLE,
                            MessageCode = "Từ tháng (BHXH) không được để trống",
                            InnerBody = InsInformation,
                            StatusCode = EnumStatusCode.StatusCode400
                        };
                    }

                    // kiểm tra từ tháng (BHYT)
                    if (InsInformation.BHYT_FROM_DATE == null)
                    {
                        return new FormatedResponse()
                        {
                            ErrorType = EnumErrorType.CATCHABLE,
                            MessageCode = "Từ tháng (BHYT) không được để trống",
                            InnerBody = InsInformation,
                            StatusCode = EnumStatusCode.StatusCode400
                        };
                    }

                    // kiểm tra từ tháng (BHTN)
                    if (InsInformation.BHTN_FROM_DATE == null)
                    {
                        return new FormatedResponse()
                        {
                            ErrorType = EnumErrorType.CATCHABLE,
                            MessageCode = "Từ tháng (BHTN) không được để trống",
                            InnerBody = InsInformation,
                            StatusCode = EnumStatusCode.StatusCode400
                        };
                    }

                    // kiểm tra từ tháng (TNLD-BNN)
                    if (InsInformation.BHTNLD_BNN_FROM_DATE == null)
                    {
                        return new FormatedResponse()
                        {
                            ErrorType = EnumErrorType.CATCHABLE,
                            MessageCode = "Từ tháng (TNLD-BNN) không được để trống",
                            InnerBody = InsInformation,
                            StatusCode = EnumStatusCode.StatusCode400
                        };
                    }

                    _dbContext.InsInformations.Add(InsInformation);
                    _dbContext.SaveChanges();
                }


                // Clear tmp
                _dbContext.InsInformationImports.RemoveRange(tmp1);
                _dbContext.SaveChanges();

                _uow.Commit();

                var new_record = _dbContext.InsInformations.OrderByDescending(x => x.ID).First();

                return new() { InnerBody = new_record };
            }
            catch (Exception ex)
            {
                _uow.Rollback();
                return new() { ErrorType = EnumErrorType.UNCATCHABLE, StatusCode = EnumStatusCode.StatusCode500, MessageCode = ex.Message };
            }
        }
    }
}
