using API.All.DbContexts;
using API.All.HRM.Profile.ProfileAPI.HuCertificateImport;
using API.All.SYSTEM.CoreAPI.Xlsx;
using API.DTO;
using Common.Extensions;
using CORE.DTO;
using CORE.Enum;
using CORE.GenericUOW;
using CORE.StaticConstant;
using System.Diagnostics;

namespace API.All.HRM.Profile.ProfileAPI.HuCertificateImport
{
    public class HuCertificateImport : IHuCertificateImportRepository
    {
        private readonly GenericUnitOfWork _uow;
        private readonly FullDbContext _dbContext;
        private readonly GenericReducer<HU_CERTIFICATE_IMPORT, HuCertificateImportDTO> _genericReducer;
        private readonly List<string> XLSX_COLUMNS;

        public HuCertificateImport(FullDbContext context, GenericUnitOfWork uow)
        {
            _dbContext = context;
            _uow = uow;
            _genericReducer = new();
            XLSX_COLUMNS = new() { "XLSX_USER_ID", "XLSX_EX_CODE", "XLSX_INSERT_ON", "XLSX_SESSION", "XLSX_FILE_NAME", "XLSX_ROW" };
        }
        public async Task<GenericPhaseTwoListResponse<HuCertificateImportDTO>> SinglePhaseQueryList(GenericQueryListDTO<HuCertificateImportDTO> request)
        {
            /*
                var raw = from a in _dbContext.HuEmployeeImports 
                join b in _dbContext.HuEmployeeCvImports 
                on new { a.XLSX_USER_ID, a.XLSX_EX_CODE, a.XLSX_SESSION, a.XLSX_ROW } equals new { b.XLSX_USER_ID, b.XLSX_EX_CODE, b.XLSX_SESSION, b.XLSX_ROW }

             */
            var raw = from p in _dbContext.HuCertificateImports
                      from e in _dbContext.HuEmployees.Where(x => x.ID == p.EMPLOYEE_ID).DefaultIfEmpty()
                      from cv in _dbContext.HuEmployeeCvs.Where(x => x.ID == e.PROFILE_ID).DefaultIfEmpty()
                      from t in _dbContext.HuPositions.Where(x => x.ID == e.POSITION_ID).DefaultIfEmpty()
                      from o in _dbContext.HuOrganizations.Where(x => x.ID == e.ORG_ID).DefaultIfEmpty()
                      from type in _dbContext.SysOtherLists.Where(x => x.ID == p.TYPE_CERTIFICATE).DefaultIfEmpty()
                      from lvtrain in _dbContext.SysOtherLists.Where(x => x.ID == p.LEVEL_TRAIN).DefaultIfEmpty()
                      from typeTrain in _dbContext.SysOtherLists.Where(x => x.ID == p.TYPE_TRAIN).DefaultIfEmpty()
                      from school in _dbContext.SysOtherLists.Where(x => x.ID == p.SCHOOL_ID).DefaultIfEmpty()
                      from lv in _dbContext.SysOtherLists.Where(x => x.ID == p.LEVEL_ID).DefaultIfEmpty()
                      select new HuCertificateImportDTO()
                      {
                          Id = p.XLSX_ROW,
                          // Phần thông tin có trong tất cả các
                          XlsxUserId = p.XLSX_USER_ID,
                          XlsxExCode = p.XLSX_EX_CODE,
                          XlsxSession = p.XLSX_SESSION,
                          XlsxInsertOn = p.XLSX_INSERT_ON,
                          XlsxFileName = p.XLSX_FILE_NAME,
                          XlsxRow = p.XLSX_ROW,

                          // Phần thông tin thô từ bảng gốc
                          EmployeeCode = e.CODE,
                          EmployeeFullName = cv.FULL_NAME,
                          OrgName = o.NAME,
                          TitleName = t.NAME,
                          TypeCertificateName = type.NAME,
                          Name = p.NAME,
                          TrainFromDate = p.TRAIN_FROM_DATE,
                          TrainToDate = p.TRAIN_TO_DATE,
                          EffectFrom = p.EFFECT_FROM,
                          EffectTo = p.EFFECT_TO,
                          LevelTrainName = lvtrain.NAME,
                          Major = p.MAJOR,
                          ContentTrain = p.CONTENT_TRAIN,
                          SchoolName = school.NAME,
                          Year = p.YEAR,
                          Mark = p.MARK,
                          IsPrime = p.IS_PRIME,
                          Level = p.LEVEL,
                          TypeTrainName = typeTrain.NAME,
                          Remark = p.REMARK,
                          LevelId = p.LEVEL_ID,
                          LevelName = lv.NAME,
                          OrgId = e.ORG_ID,
                          IsPrimeStr = p.IS_PRIME == true ? "Là bằng chính" : "Không là bằng chính"
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
                var certificateImport = await _dbContext.HuCertificateImports.Where(x => x.XLSX_USER_ID == request.XlsxSid && x.XLSX_EX_CODE == request.XlsxExCode && x.XLSX_SESSION == request.XlsxSession).ToListAsync();

                var tmpType = typeof(HU_CERTIFICATE_IMPORT);
                var tmpProperties = tmpType.GetProperties()?
                    .Where(x => !XLSX_COLUMNS.Contains(x.Name))
                    .ToList();

                var certificateType = typeof(HU_CERTIFICATE);
                var certificateTypeProperties = certificateType.GetProperties().ToList();
                var res = new HU_CERTIFICATE();
                certificateImport.ForEach(tmpCv =>
                {
                    var obj = Activator.CreateInstance(typeof(HU_CERTIFICATE)) ?? throw new Exception(CommonMessageCode.ACTIVATOR_CREATE_INSTANCE_RETURNS_NULL);
                    var certificate = (HU_CERTIFICATE)obj;

                    tmpProperties?.ForEach(tmpProperties =>
                    {
                        var tmpValue = tmpProperties.GetValue(tmpCv);
                        var certificateProperty = certificateTypeProperties.SingleOrDefault(x => x.Name == tmpProperties.Name);
                        if (certificateProperty != null)
                        {
                            if (tmpValue != null)
                            {
                                certificateProperty.SetValue(certificate, tmpValue);
                            };
                        }
                        else
                        {
                            if (tmpValue != null)
                            {
                                throw new Exception($"{tmpProperties.Name} was not found in HU_EMPLOYEE_CV");
                            }
                        }
                    });
                    if (certificate.EFFECT_FROM > certificate.EFFECT_TO)
                    {
                        throw new Exception(CommonMessageCode.EXP_MUST_LESS_THAN_EFF);
                    }

                    if (certificate.EMPLOYEE_ID == null)
                    {
                        throw new Exception("EMPLOYEE_ID_CANNOT_NULL" + " " + tmpCv.XLSX_ROW);
                    }

                    if (certificate.EFFECT_FROM == null || certificate.EFFECT_TO == null)
                    {
                        throw new Exception("EFFECT_FROM_AND_EFFECT_TO_CANNOT_NULL" + " " + tmpCv.XLSX_ROW);
                    }

                    if (certificate.IS_PRIME == null)
                    {
                        throw new Exception("IS_PRIME_CANNOT_NULL" + " " + tmpCv.XLSX_ROW);
                    }
                    if(certificate.TYPE_CERTIFICATE == null)
                    {
                        throw new Exception("TYPE_CERTIFICATE_CANNOT_NULL" + " " + tmpCv.XLSX_ROW);
                    }
                    if(certificate.TRAIN_FROM_DATE == null)
                    {
                        throw new Exception("TRAIN_CERTIFICATE_CANNOT_NULL" + " " + tmpCv.XLSX_ROW);
                    }
                    if(certificate.TRAIN_TO_DATE == null)
                    {
                        throw new Exception("TRAIN_TO_DATE_CANNOT_NULL" + " " + tmpCv.XLSX_ROW);
                    }
                    if(certificate.LEVEL_ID == null)
                    {
                        throw new Exception("LEVEL_ID_CANNOT_NULL" + " " + tmpCv.XLSX_ROW);
                    }
                    if(certificate.SCHOOL_ID == null)
                    {
                        throw new Exception("SCHOOL_ID_CANNOT_NULL" + " " + tmpCv.XLSX_ROW);
                    }

                    certificate.CREATED_DATE = now;
                    certificate.CREATED_BY = request.XlsxSid;
                    res = certificate;
                    _dbContext.HuCertificates.Add(certificate);
                    _dbContext.SaveChanges();

                });

                // Clear tmp
                _dbContext.HuCertificateImports.RemoveRange(certificateImport);
                _dbContext.SaveChanges();

                _uow.Commit();
                return new() { InnerBody = res, StatusCode = EnumStatusCode.StatusCode200, MessageCode = CommonMessageCode.CREATE_SUCCESS };

            }
            catch (Exception ex)
            {
                _uow.Rollback();
                return new() { ErrorType = EnumErrorType.UNCATCHABLE, StatusCode = EnumStatusCode.StatusCode500, MessageCode = ex.Message };
            }
        }
    }
}
