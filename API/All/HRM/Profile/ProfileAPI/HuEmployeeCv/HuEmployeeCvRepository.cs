using CORE.GenericUOW;
using CORE.DTO;
using API.DTO;
using CORE.Enum;
using CORE.StaticConstant;
using API.All.DbContexts;
using CORE.AutoMapper;
using Microsoft.AspNetCore.Identity;
using API.Main;
using Microsoft.AspNetCore.Mvc;
using ProfileDAL.ViewModels;
using System.Drawing.Printing;
using System.Linq;
using System;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.ComponentModel;
using Microsoft.IdentityModel.Tokens;
using CORE.Services.File;
using Microsoft.Extensions.Options;
using System.Globalization;
using System.Text;
using AttendanceDAL.ViewModels;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;
using System.Text.RegularExpressions;
using Microsoft.VisualBasic;
using static System.Collections.Specialized.BitVector32;
using System.Data;
using Aspose.Words;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using Aspose.Words.MailMerging;
using Aspose.Words.Drawing;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;
using Common.Extensions;
using Text = DocumentFormat.OpenXml.Wordprocessing.Text;
using API.All.HRM.Insurance.InsuranceAPI.Business.InsChange;
using API.All.HRM.Profile.ProfileAPI.HuEmployeeCv;
using Org.BouncyCastle.Asn1.X509;
using API.All.SYSTEM.Common;

namespace API.Controllers.HuEmployeeCv
{
    public class HuEmployeeCvRepository : IHuEmployeeCvRepository
    {
        private readonly GenericUnitOfWork _uow;
        private readonly FullDbContext _dbContext;
        private IGenericRepository<HU_EMPLOYEE_CV, HuEmployeeCvDTO> _genericRepository;
        private IGenericRepository<SYS_USER, SysUserDTO> _genericRepositoryUser;
        private readonly GenericReducer<HU_EMPLOYEE_CV, HuEmployeeCvDTO> _genericReducer;
        private readonly IWebHostEnvironment _env;
        private readonly AppSettings _appSettings;
        private readonly IFileService _fileService;

        public HuEmployeeCvRepository(FullDbContext context, GenericUnitOfWork uow, IWebHostEnvironment env, IOptions<AppSettings> options, IFileService fileService)
        {
            _dbContext = context;
            _uow = uow;
            _genericRepository = _uow.GenericRepository<HU_EMPLOYEE_CV, HuEmployeeCvDTO>();
            _genericRepositoryUser = _uow.GenericRepository<SYS_USER, SysUserDTO>();
            _genericReducer = new();
            _env = env;
            _appSettings = options.Value;
            _fileService = fileService;
        }

        public async Task<GenericPhaseTwoListResponse<HuEmployeeCvDTO>> SinglePhaseQueryList(GenericQueryListDTO<HuEmployeeCvDTO> request)
        {
            var joined = from p in _dbContext.HuEmployeeCvs.AsNoTracking()
                             // JOIN OTHER ENTITIES BASED ON THE BUSINESS
                         select new HuEmployeeCvDTO
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
                var list = new List<HU_EMPLOYEE_CV>
                    {
                        (HU_EMPLOYEE_CV)response
                    };
                var joined = (from l in list
                                  // JOIN OTHER ENTITIES BASED ON THE BUSINESS
                              select new HuEmployeeCvDTO
                              {
                                  Id = l.ID,
                                  Avatar = l.AVATAR,
                                  FirstName = l.FIRST_NAME,
                                  LastName = l.LAST_NAME,
                                  FullName = l.FULL_NAME,
                                  GenderId = l.GENDER_ID,
                                  Gender = GetOtherListName(l.GENDER_ID),
                                  BirthDate = l.BIRTH_DATE,
                                  IdNo = l.ID_NO,
                                  //ProfileCode = l.PROFILE_CODE,
                                  IdDate = l.ID_DATE,
                                  IdPlace = l.ID_PLACE,
                                  ReligionId = l.RELIGION_ID,
                                  NativeId = l.NATIVE_ID,
                                  NationalityId = l.NATIONALITY_ID,
                                  Address = l.ADDRESS,
                                  BirthPlace = l.BIRTH_PLACE,
                                  ProvinceId = l.PROVINCE_ID,
                                  DistrictId = l.DISTRICT_ID,
                                  WardId = l.WARD_ID,
                                  CurAddress = l.CUR_ADDRESS,
                                  CurProvinceId = l.CUR_PROVINCE_ID,
                                  CurDistrictId = l.CUR_DISTRICT_ID,
                                  CurWardId = l.CUR_WARD_ID,
                                  TaxCode = l.TAX_CODE,
                                  MobilePhone = l.MOBILE_PHONE,
                                  WorkEmail = l.WORK_EMAIL,
                                  Email = l.EMAIL,
                                  MaritalStatusId = l.MARITAL_STATUS_ID,
                                  PassNo = l.PASS_NO,
                                  PassDate = l.PASS_DATE,
                                  PassExpire = l.PASS_EXPIRE,
                                  PassPlace = l.PASS_PLACE,
                                  VisaNo = l.VISA_NO,
                                  VisaDate = l.VISA_DATE,
                                  VisaExpire = l.VISA_EXPIRE,
                                  VisaPlace = l.VISA_PLACE,
                                  WorkPermit = l.WORK_PERMIT,
                                  WorkPermitDate = l.WORK_PERMIT_DATE,
                                  WorkPermitExpire = l.WORK_PERMIT_EXPIRE,
                                  WorkPermitPlace = l.WORK_PERMIT_PLACE,
                                  WorkNo = l.WORK_NO,
                                  WorkPlace = l.WORK_PLACE,
                                  WorkScope = l.WORK_SCOPE,
                                  ContactPer = l.CONTACT_PER,
                                  ContactPerPhone = l.CONTACT_PER_PHONE,
                                  BankId = l.BANK_ID,
                                  BankBranch = l.BANK_BRANCH,
                                  BankNo = l.BANK_NO,
                                  SchoolId = l.SCHOOL_ID,
                                  QualificationId = l.QUALIFICATION_ID,
                                  Qualificationid = l.QUALIFICATIONID,
                                  TrainingFormId = l.TRAINING_FORM_ID,
                                  LearningLevelId = l.LEARNING_LEVEL_ID,
                                  Language = l.LANGUAGE,
                                  LanguageMark = l.LANGUAGE_MARK,
                                  Image = l.IMAGE,
                              }).FirstOrDefault();

                return new FormatedResponse() { InnerBody = joined };
            }
            else
            {
                return new FormatedResponse() { MessageCode = res.MessageCode, ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400 };
            }
        }
        private string GetOtherListName(long? id)
        {
            var sys = _dbContext.SysOtherLists.Where(x => x.ID == id).SingleOrDefault();
            if (sys != null && sys.NAME != null)
            {
                return sys.NAME;
            }
            else
            {
                return "";
            }
        }

        public async Task<FormatedResponse> GetById(string id)
        {
            await Task.Run(() => null);
            throw new NotImplementedException();
        }

        public async Task<FormatedResponse> Create(GenericUnitOfWork _uow, HuEmployeeCvDTO dto, string sid)
        {
            var response = await _genericRepository.Create(_uow, dto, sid);
            return response;
        }

        public async Task<FormatedResponse> CreateRange(GenericUnitOfWork _uow, List<HuEmployeeCvDTO> dtos, string sid)
        {
            var add = new List<HuEmployeeCvDTO>();
            add.AddRange(dtos);
            var response = await _genericRepository.CreateRange(_uow, add, sid);
            return response;
        }

        public async Task<FormatedResponse> Update(GenericUnitOfWork _uow, HuEmployeeCvDTO dto, string sid, bool patchMode = true)
        {
            var response = await _genericRepository.Update(_uow, dto, sid, patchMode);
            return response;
        }

        public async Task<FormatedResponse> UpdateRange(GenericUnitOfWork _uow, List<HuEmployeeCvDTO> dtos, string sid, bool patchMode = true)
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

        public async Task<FormatedResponse> GetBankInfo(long employeeCvId)
        {
            var query = await (from c in _dbContext.HuEmployeeCvs
                               from b in _dbContext.HuBanks.Where(b => b.ID == c.BANK_ID).DefaultIfEmpty()
                               from br1 in _dbContext.HuBankBranchs.Where(x => x.ID == c.BANK_BRANCH).DefaultIfEmpty()
                               from br2 in _dbContext.HuBankBranchs.Where(x => x.ID == c.BANK_BRANCH_2).DefaultIfEmpty()
                               from d in _dbContext.HuBanks.Where(b => b.ID == c.BANK_ID_2).DefaultIfEmpty()
                               where c.ID == employeeCvId

                               select new
                               {
                                   Id = employeeCvId,
                                   BankId = c.BANK_ID,
                                   BankId2 = c.BANK_ID_2,
                                   AccountBankName = RemoveDiacritics(c.FULL_NAME).ToUpper(),
                                   BankName = b.NAME ?? "",
                                   BankBranch = br1.NAME,
                                   BankNo = c.BANK_NO ?? "",
                                   BankNo2 = c.BANK_NO_2 ?? "",
                                   BankName2 = d.NAME ?? "",
                                   BankBranch2 = br2.NAME
                               }).SingleAsync();
            return new() { InnerBody = query };
        }
        public async Task<FormatedResponse> GetBank(long id)
        {
            var query = await (from c in _dbContext.HuEmployeeCvs
                               from b in _dbContext.HuBanks.Where(b => b.ID == c.BANK_ID).DefaultIfEmpty()
                               from d in _dbContext.HuBanks.Where(b => b.ID == c.BANK_ID_2).DefaultIfEmpty()
                               where c.ID == id

                               select new
                               {
                                   Id = id,
                                   BankId = c.BANK_ID,
                                   BankId2 = c.BANK_ID_2,
                                   AccountBankName = RemoveDiacritics(c.FULL_NAME).ToUpper(),
                                   BankName = b.NAME ?? "",
                                   BankBranchId = c.BANK_BRANCH,
                                   BankNo = c.BANK_NO ?? "",
                                   BankNo2 = c.BANK_NO_2 ?? "",
                                   BankName2 = d.NAME ?? "",
                                   BankBranchId2 = c.BANK_BRANCH_2
                               }).SingleAsync();
            return new() { InnerBody = query };
        }

        public async Task<FormatedResponse> UpdateBank(StaffProfileUpdateDTO request)
        {
            try
            {
                var entity = await _dbContext.HuEmployeeCvs.Where(x => x.ID == request.Id).FirstOrDefaultAsync();
                if (entity != null)
                {
                    entity.BANK_ID = request.BankId;
                    entity.BANK_ID_2 = request.BankId2;
                    entity.BANK_BRANCH = request.BankBranchId;
                    entity.BANK_BRANCH_2 = request.BankBranchId2;
                    entity.BANK_NO = request.BankNo;
                    entity.BANK_NO_2 = request.BankNo2;
                    _dbContext.SaveChanges();
                    return new() { MessageCode = CommonMessageCode.UPDATE_SUCCESS, InnerBody = true, StatusCode = EnumStatusCode.StatusCode200 };
                }
                else
                {
                    return new() { MessageCode = CommonMessageCode.ENTITY_NOT_FOUND, InnerBody = entity, StatusCode = EnumStatusCode.StatusCode400 };
                }
            }
            catch (Exception ex)
            {
                return new FormatedResponse() { MessageCode = CommonMessageCode.POST_UPDATE_FAILLED, ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400 };
            }
        }
        static string RemoveDiacritics(string input)
        {
            string normalized = input.Normalize(NormalizationForm.FormD);
            StringBuilder builder = new StringBuilder();

            foreach (char c in normalized)
            {
                if (CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                {
                    builder.Append(c);
                }
            }

            return builder.ToString();
        }

        public async Task<FormatedResponse> GetPapers(long employeeCvId)
        {
            var query = await (from c in _dbContext.HuEmployeePaperss
                               from e in _dbContext.HuEmployees.Where(x => x.ID == c.EMP_ID).DefaultIfEmpty()
                               from s in _dbContext.SysOtherLists.Where(x => x.ID == c.PAPER_ID).DefaultIfEmpty()
                               where e.PROFILE_ID == employeeCvId
                               orderby s.ORDERS
                               select new
                               {
                                   Name = "Index" + c.PAPER_ID,
                                   PaperIdCheck = true,
                                   Orders = s.ORDERS
                               }).ToListAsync();
            return new() { InnerBody = query };
        }

        public async Task<FormatedResponse> UpdatePapers(DynamicDTO model, string sid)
        {
            try
            {
                var lstPaperType = (from type in _dbContext.SysOtherLists
                                    from type_type in _dbContext.SysOtherListTypes.Where(x => x.ID == type.TYPE_ID).DefaultIfEmpty()
                                    where type_type.CODE == "PAPER"
                                    select type.ID).ToList();
                var employeeId = (from e in _dbContext.HuEmployees.Where(x => x.PROFILE_ID == (long)model["id"])
                                  select e.ID).FirstOrDefault();
                var lstPaperId = (from e in _dbContext.HuEmployeePaperss.Where(x => x.EMP_ID == employeeId)
                                  select e.PAPER_ID).ToList();


                foreach (var item in lstPaperType)
                {
                    string nameIndex = "Index" + item;
                    var check = model[nameIndex] ?? "";
                    if (check != "")
                        if ((bool)check == true)
                        {
                            if (lstPaperId.Contains((long)item) == false)
                                await _dbContext.HuEmployeePaperss.AddAsync(new HU_EMPLOYEE_PAPERS
                                {
                                    PAPER_ID = (long)item,
                                    EMP_ID = employeeId,
                                    DATE_INPUT = DateTime.Now,
                                    CREATED_DATE = DateTime.Now,
                                    CREATED_BY = sid
                                });
                        }
                        else
                        {
                            var deleteObj = _dbContext.HuEmployeePaperss.Where(x => x.PAPER_ID == item && x.EMP_ID == employeeId).FirstOrDefault();
                            if (deleteObj != null)
                            {
                                _dbContext.HuEmployeePaperss.Remove(deleteObj);
                            }
                        }
                }

                await _dbContext.SaveChangesAsync();
                return new() { InnerBody = model, MessageCode = CommonMessageCode.UPDATE_SUCCESS, StatusCode = EnumStatusCode.StatusCode200 };

            }
            catch (Exception ex)
            {
                return new FormatedResponse() { MessageCode = ex.Message, ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400 };

            }

        }

        public async Task<FormatedResponse> GetCv(long employeeCvId)
        {
            try
            {
                var query = await (from c in _dbContext.HuEmployeeCvs
                                   from p in _dbContext.HuProvinces.Where(x => x.ID == c.ID_PLACE).DefaultIfEmpty()
                                   from e in _dbContext.HuProvinces.Where(x => x.ID == c.TAX_CODE_ADDRESS).DefaultIfEmpty()
                                   from s in _dbContext.SysOtherLists.Where(s => s.ID == c.GENDER_ID).DefaultIfEmpty()
                                   from s1 in _dbContext.SysOtherLists.Where(s1 => s1.ID == c.NATIONALITY_ID).DefaultIfEmpty()
                                   from s2 in _dbContext.SysOtherLists.Where(s2 => s2.ID == c.NATIVE_ID).DefaultIfEmpty()
                                   from s3 in _dbContext.SysOtherLists.Where(s2 => s2.ID == c.RELIGION_ID).DefaultIfEmpty()
                                   from s4 in _dbContext.SysOtherLists.Where(s2 => s2.ID == c.MARITAL_STATUS_ID).DefaultIfEmpty()
                                   where c.ID == employeeCvId

                                   select new
                                   {
                                       Id = employeeCvId,
                                       Gender = s.NAME,
                                       Nationality = s1.NAME,
                                       Nation = s2.NAME,
                                       Religion = s3.NAME,
                                       MaritalStatus = s4.NAME,
                                       IdentityNumber = c.ID_NO,
                                       DateIdentity = c.ID_DATE,
                                       AddressIdentity = p.NAME,
                                       ExaminationDate = c.EXAMINATION_DATE,
                                       HealthNotes = c.HEALTH_NOTES,
                                       Heart = c.HEART,
                                       RightEye = c.RIGHT_EYE,
                                       LeftEye = c.LEFT_EYE,
                                       Height = c.HEIGHT,
                                       Weight = c.WEIGHT,
                                       BirthDay = c.BIRTH_DATE,
                                       BloodPressure = c.BLOOD_PRESSURE,
                                       Domicile = c.DOMICILE,
                                       BloodGroup = c.BLOOD_GROUP,
                                       HealthType = c.HEALTH_TYPE,
                                       BirthRegisAddress = c.BIRTH_REGIS_ADDRESS,
                                       TaxCodeDate = c.TAX_CODE_DATE,
                                       TaxCode = c.TAX_CODE,
                                       TaxCodeAddress = e.NAME,
                                       BirthPlace = c.BIRTH_PLACE,

                                   }).SingleAsync();
                return new() { InnerBody = query };
            }
            catch (Exception ex)
            {
                return new() { InnerBody = null };
            }

        }

        public async Task<FormatedResponse> GetAdditonalInfo(long employeeCvId)
        {
            var query = await (from c in _dbContext.HuEmployeeCvs
                               from d in _dbContext.HuEmployees.Where(x => x.PROFILE_ID == c.ID).DefaultIfEmpty()
                               from o in _dbContext.HuOrganizations.Where(x => x.ID == d.ORG_ID).DefaultIfEmpty()
                               from com in _dbContext.HuCompanys.Where(x => x.ID == o.COMPANY_ID).DefaultIfEmpty()
                               from v in _dbContext.SysOtherLists.Where(x => x.ID == com.REGION_ID).DefaultIfEmpty()
                               from dsts in _dbContext.SysOtherLists.Where(x => x.ID == c.DEFENSE_SECURITY_TRAINING).DefaultIfEmpty()
                               where c.ID == employeeCvId
                               select new
                               {
                                   Id = employeeCvId,
                                   Passport = c.PASS_NO,
                                   PassDate = c.PASS_DATE,
                                   PassDateExpired = c.PASS_EXPIRE,
                                   PassAddress = c.PASS_PLACE,
                                   Visa = c.VISA_NO,
                                   VisaDate = c.VISA_DATE,
                                   VisaDateExpired = c.VISA_EXPIRE,
                                   VisaAddress = c.VISA_PLACE,
                                   LaborBookNumber = c.WORK_NO,
                                   LaborBookDate = c.WORK_PERMIT_DATE,
                                   LaborBookDateExpired = c.WORK_PERMIT_EXPIRE,
                                   LaborBookAddress = c.WORK_PERMIT_PLACE,
                                   HealthNotes = c.HEALTH_NOTES,
                                   Carrer = c.CARRER,
                                   InsArea = v.NAME,
                                   InsNumber = c.INSURENCE_NUMBER,
                                   HealthCareAddress = c.HEALTH_CARE_ADDRESS,
                                   InsCardNumber = c.INS_CARD_NUMBER,
                                   FamilyMember = c.FAMILY_MEMBER,
                                   FamilyPolicy = c.FAMILY_POLICY,
                                   Veterans = c.VETERANS,
                                   PoliticalTheory = c.POLITICAL_THEORY,
                                   CareerBeforeRecruitment = c.CARRER_BEFORE_RECUITMENT,
                                   TitleConferred = c.TITLE_CONFERRED,
                                   SchoolOfWork = c.SCHOOL_OF_WORK,
                                   DefenseSecurityTrainingStr = dsts.NAME
                               }).SingleAsync();
            return new() { InnerBody = query };
        }

        public async Task<FormatedResponse> GetAdditonal(long id)
        {
            var query = await (from c in _dbContext.HuEmployeeCvs
                               from e in _dbContext.HuEmployees.Where(x => x.PROFILE_ID == c.ID).DefaultIfEmpty()
                               from o in _dbContext.HuOrganizations.Where(x => x.ID == e.ORG_ID).DefaultIfEmpty()
                               from com in _dbContext.HuCompanys.Where(x => x.ID == o.COMPANY_ID).DefaultIfEmpty()
                               from v in _dbContext.SysOtherLists.Where(x => x.ID == com.REGION_ID).DefaultIfEmpty()
                               from v2 in _dbContext.SysOtherLists.Where(x => x.ID == c.NATIONALITY_ID).DefaultIfEmpty()
                               where c.ID == id

                               select new
                               {
                                   Id = id,
                                   Passport = c.PASS_NO,
                                   PassDate = c.PASS_DATE,
                                   PassDateExpired = c.PASS_EXPIRE,
                                   PassAddress = c.PASS_PLACE,
                                   Visa = c.VISA_NO,
                                   VisaDate = c.VISA_DATE,
                                   VisaDateExpired = c.VISA_EXPIRE,
                                   VisaAddress = c.VISA_PLACE,
                                   LaborBookNumber = c.WORK_NO,
                                   LaborBookDate = c.WORK_PERMIT_DATE,
                                   LaborBookDateExpired = c.WORK_PERMIT_EXPIRE,
                                   LaborBookAddress = c.WORK_PERMIT_PLACE,
                                   HealthNotes = c.HEALTH_NOTES,
                                   Carrer = c.CARRER,
                                   InsArea = v.ID,
                                   InsNumber = c.INSURENCE_NUMBER,
                                   HealthCareAddress = c.HEALTH_CARE_ADDRESS,
                                   InsCardNumber = c.INS_CARD_NUMBER,
                                   FamilyMember = c.FAMILY_MEMBER,
                                   FamilyPolicy = c.FAMILY_POLICY,
                                   Veterans = c.VETERANS,
                                   PoliticalTheory = c.POLITICAL_THEORY,
                                   CareerBeforeRecruitment = c.CARRER_BEFORE_RECUITMENT,
                                   TitleConferred = c.TITLE_CONFERRED,
                                   SchoolOfWork = c.SCHOOL_OF_WORK,
                                   Nationality = v2.NAME,
                                   DefenseSecurityTraining = c.DEFENSE_SECURITY_TRAINING
                               }).SingleAsync();
            return new() { InnerBody = query };
        }

        public async Task<FormatedResponse> UpdateAdditonal(StaffProfileUpdateDTO request)
        {
            try
            {
                var entity = _dbContext.HuEmployeeCvs.Where(x => x.ID == request.Id).FirstOrDefault();
                var entity2 = _dbContext.HuEmployees.Where(x => x.PROFILE_ID == entity.ID).FirstOrDefault();
                if (entity != null)
                {
                    entity.PASS_NO = request.Passport;
                    entity.PASS_PLACE = request.PassAddress;
                    entity.PASS_DATE = request.PassDate;
                    entity.PASS_EXPIRE = request.PassDateExpired;
                    entity.NATIVE_ID = request.NationId;
                    entity.NATIONALITY_ID = request.NationalityId;
                    entity.VISA_NO = request.Visa;
                    entity.VISA_DATE = request.VisaDate;
                    entity.VISA_EXPIRE = request.VisaDateExpired;
                    entity.VISA_PLACE = request.VisaAddress;
                    entity.WORK_NO = request.LaborBookNumber;
                    entity.WORK_PERMIT_DATE = request.LaborBookDate;
                    entity.WORK_PERMIT_EXPIRE = request.LaborBookDateExpired;
                    entity.WORK_PERMIT_PLACE = request.LaborBookAddress;
                    entity.CARRER = request.Carrer;
                    entity2.INSURENCE_AREA_ID = request.InsArea;
                    entity.INSURENCE_NUMBER = request.InsNumber;
                    entity.INS_CARD_NUMBER = request.InsCardNumber;
                    entity.HEALTH_CARE_ADDRESS = request.HealthCareAddress;
                    entity.FAMILY_MEMBER = request.FamilyMember;
                    entity.FAMILY_POLICY = request.FamilyPolicy;
                    entity.VETERANS = request.Veterans;
                    entity.CARRER_BEFORE_RECUITMENT = request.CareerBeforeRecruitment;
                    entity.TITLE_CONFERRED = request.TitleConferred;
                    entity.POLITICAL_THEORY = request.PoliticalTheory;
                    entity.SCHOOL_OF_WORK = request.SchoolOfWork;
                    entity.DEFENSE_SECURITY_TRAINING = request.DefenseSecurityTraining;
                    _dbContext.UpdateRange();
                    _dbContext.SaveChanges();
                    return new() { MessageCode = CommonMessageCode.UPDATE_SUCCESS, InnerBody = true, StatusCode = EnumStatusCode.StatusCode200 };
                }
                else
                {
                    return new() { MessageCode = CommonMessageCode.ENTITY_NOT_FOUND, InnerBody = false, StatusCode = EnumStatusCode.StatusCode400 };
                }
            }
            catch (Exception ex)
            {
                return new FormatedResponse() { MessageCode = CommonMessageCode.POST_UPDATE_FAILLED, ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400 };
            }

        }

        public async Task<FormatedResponse> GetCurruculum(long id)
        {
            var query = await (from c in _dbContext.HuEmployeeCvs
                               from s in _dbContext.SysOtherLists.Where(s => s.ID == c.GENDER_ID).DefaultIfEmpty()
                               from s1 in _dbContext.SysOtherLists.Where(s1 => s1.ID == c.NATIONALITY_ID).DefaultIfEmpty()
                               from s2 in _dbContext.SysOtherLists.Where(s2 => s2.ID == c.NATIVE_ID).DefaultIfEmpty()
                               from s3 in _dbContext.SysOtherLists.Where(s2 => s2.ID == c.RELIGION_ID).DefaultIfEmpty()
                               from s4 in _dbContext.SysOtherLists.Where(s2 => s2.ID == c.MARITAL_STATUS_ID).DefaultIfEmpty()
                               where c.ID == id

                               select new
                               {
                                   Id = id,
                                   GenderId = s.ID,
                                   NationalityId = s1.ID,
                                   Nationality = s1.NAME,
                                   NationId = s2.ID,
                                   Nation = s2.NAME,
                                   ReligionId = s3.ID,
                                   Religion = s3.NAME,
                                   MaritalStatus = s4.NAME,
                                   MaritalStatusId = s4.ID,
                                   IdentityNumber = c.ID_NO,
                                   IdentityNumberDate = c.ID_DATE,
                                   IdentityNumberAddress = c.ID_PLACE,
                                   ExaminationDate = c.EXAMINATION_DATE,
                                   Heart = c.HEART,
                                   RightEye = c.RIGHT_EYE,
                                   LeftEye = c.LEFT_EYE,
                                   Height = c.HEIGHT,
                                   Weight = c.WEIGHT,
                                   BirthRegisAddress = c.BIRTH_REGIS_ADDRESS,
                                   BirthDay = c.BIRTH_DATE,
                                   BloodPressure = c.BLOOD_PRESSURE,
                                   Domicile = c.DOMICILE,
                                   BloodGroup = c.BLOOD_GROUP,
                                   TaxCodeDate = c.TAX_CODE_DATE,
                                   TaxCode = c.TAX_CODE,
                                   HealthNote = c.HEALTH_NOTES,
                                   HeathType = c.HEALTH_TYPE,
                                   DateExam = c.EXAMINATION_DATE,
                                   TaxCodeAddress = c.TAX_CODE_ADDRESS,
                                   BirthPlace = c.BIRTH_PLACE
                               }).SingleAsync();
            return new() { InnerBody = query };
        }

        public async Task<FormatedResponse> UpdateCurruculum(StaffProfileUpdateDTO request)
        {
            try
            {
                if (request.BirthDay != null && ((DateTime.Now.Year - request.BirthDay.Value.Year) < 15))
                {
                    return new FormatedResponse() { MessageCode = CommonMessageCode.BIRTH_DAY_NOT_BIGGER_THAN_DATE_TIME_NOW, ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400 };
                }
                var query = await (from p in _dbContext.HuEmployees
                                   from e in _dbContext.HuEmployeeCvs.Where(x => x.ID == p.PROFILE_ID).DefaultIfEmpty()
                                   where e.TAX_CODE == request.TaxCode && e.TAX_CODE != null
                                   select new
                                   {
                                       Code = p.CODE
                                   }).ToListAsync();
                List<string> result = new List<string>();
                if (query != null)
                {
                    foreach (var item in query)
                    {
                        result.Add(item.Code);
                    }
                }
                string value = String.Join("\n", result);
                if (value == "")
                {
                    var entity = _dbContext.HuEmployeeCvs.Where(x => x.ID == request.Id).FirstOrDefault();
                    if (entity != null)
                    {
                        entity.HEALTH_NOTES = request.HealthNote;
                        entity.GENDER_ID = request.GenderId;
                        entity.EXAMINATION_DATE = request.DateExam;
                        entity.HEART = request.Heart;
                        entity.RIGHT_EYE = request.RightEye;
                        entity.LEFT_EYE = request.LeftEye;
                        entity.HEALTH_TYPE = request.HeathType;
                        entity.BLOOD_PRESSURE = request.BloodPressure;
                        entity.WEIGHT = request.Weight;
                        entity.HEIGHT = request.Height;
                        entity.BLOOD_GROUP = request.BloodGroup;
                        entity.TAX_CODE_ADDRESS = request.TaxCodeAddress;
                        entity.RELIGION_ID = request.ReligionId;
                        entity.MARITAL_STATUS_ID = request.MaritalStatusId;
                        entity.TAX_CODE = request.TaxCode == "" ? null : request.TaxCode;
                        entity.TAX_CODE_DATE = request.TaxCodeDate;
                        entity.ID_NO = request.IdentityNumber;
                        entity.ID_DATE = request.IdentityNumberDate;
                        entity.ID_PLACE = request.IdentityNumberAddress;
                        entity.DOMICILE = request.Domicile;
                        entity.BIRTH_REGIS_ADDRESS = request.BirthRegisAddress;
                        entity.NATIONALITY_ID = request.NationalityId;
                        entity.NATIVE_ID = request.NationId;
                        entity.EXAMINATION_DATE = request.DateExam;
                        entity.BIRTH_PLACE = request.BirthPlace;
                        entity.BIRTH_DATE = request.BirthDay;
                        _dbContext.SaveChanges();

                        return new() { MessageCode = CommonMessageCode.UPDATE_SUCCESS, InnerBody = true, StatusCode = EnumStatusCode.StatusCode200 };
                    }
                    else
                    {
                        return new FormatedResponse() { MessageCode = CommonMessageCode.ENTITY_NOT_FOUND, InnerBody = false, StatusCode = EnumStatusCode.StatusCode400 };
                    }
                }
                else
                {
                    var getData = await (from p in _dbContext.HuEmployees
                                         from e in _dbContext.HuEmployeeCvs.Where(x => x.ID == p.PROFILE_ID).DefaultIfEmpty()
                                         where e.TAX_CODE == request.TaxCode && e.ID == request.Id
                                         select new
                                         {
                                             Code = p.CODE
                                         }).ToListAsync();
                    if(getData.Count() != 0 )
                    {
                        var entity = _dbContext.HuEmployeeCvs.Where(x => x.ID == request.Id).FirstOrDefault();
                        if (entity != null)
                        {
                            entity.HEALTH_NOTES = request.HealthNote;
                            entity.GENDER_ID = request.GenderId;
                            entity.EXAMINATION_DATE = request.DateExam;
                            entity.HEART = request.Heart;
                            entity.RIGHT_EYE = request.RightEye;
                            entity.LEFT_EYE = request.LeftEye;
                            entity.HEALTH_TYPE = request.HeathType;
                            entity.BLOOD_PRESSURE = request.BloodPressure;
                            entity.WEIGHT = request.Weight;
                            entity.HEIGHT = request.Height;
                            entity.BLOOD_GROUP = request.BloodGroup;
                            entity.TAX_CODE_ADDRESS = request.TaxCodeAddress;
                            entity.RELIGION_ID = request.ReligionId;
                            entity.MARITAL_STATUS_ID = request.MaritalStatusId;
                            entity.TAX_CODE = request.TaxCode == "" ? null : request.TaxCode;
                            entity.TAX_CODE_DATE = request.TaxCodeDate;
                            entity.ID_NO = request.IdentityNumber;
                            entity.ID_DATE = request.IdentityNumberDate;
                            entity.ID_PLACE = request.IdentityNumberAddress;
                            entity.DOMICILE = request.Domicile;
                            entity.BIRTH_REGIS_ADDRESS = request.BirthRegisAddress;
                            entity.NATIONALITY_ID = request.NationalityId;
                            entity.NATIVE_ID = request.NationId;
                            entity.EXAMINATION_DATE = request.DateExam;
                            entity.BIRTH_PLACE = request.BirthPlace;
                            entity.BIRTH_DATE = request.BirthDay;
                            _dbContext.SaveChanges();
                        }
                        return new() { MessageCode = CommonMessageCode.UPDATE_SUCCESS, InnerBody = true, StatusCode = EnumStatusCode.StatusCode200 };
                    }
                    else
                    {
                        return new FormatedResponse() { MessageCode = CommonMessageCodes.TAX_CODE_HAVE_EXISTS, ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400 };
                    }
                    // CHECK TAX CODE
                    /*var getData = await (from p in _dbContext.HuEmployees
                                       from e in _dbContext.HuEmployeeCvs.Where(x => x.ID == p.PROFILE_ID).DefaultIfEmpty()
                                       where e.TAX_CODE == request.TaxCode && e.ID == request.Id
                                       select new
                                       {
                                           Code = p.CODE
                                       }).ToListAsync();
                    List<string> listString = new List<string>();
                    if (getData != null)
                    {
                        foreach (var item in getData)
                        {
                            listString.Add(item.Code);
                        }
                    }
                    string stringValue = String.Join("\n", listString);
                    if (stringValue == "")
                    {
                        var entity = _dbContext.HuEmployeeCvs.Where(x => x.ID == request.Id).FirstOrDefault();
                        if (entity != null)
                        {
                            entity.HEALTH_NOTES = request.HealthNote;
                            entity.GENDER_ID = request.GenderId;
                            entity.EXAMINATION_DATE = request.DateExam;
                            entity.HEART = request.Heart;
                            entity.RIGHT_EYE = request.RightEye;
                            entity.LEFT_EYE = request.LeftEye;
                            entity.HEALTH_TYPE = request.HeathType;
                            entity.BLOOD_PRESSURE = request.BloodPressure;
                            entity.WEIGHT = request.Weight;
                            entity.HEIGHT = request.Height;
                            entity.BLOOD_GROUP = request.BloodGroup;
                            entity.TAX_CODE_ADDRESS = request.TaxCodeAddress;
                            entity.RELIGION_ID = request.ReligionId;
                            entity.MARITAL_STATUS_ID = request.MaritalStatusId;
                            entity.TAX_CODE = request.TaxCode == "" ? null : request.TaxCode;
                            entity.TAX_CODE_DATE = request.TaxCodeDate;
                            entity.ID_NO = request.IdentityNumber;
                            entity.ID_DATE = request.IdentityNumberDate;
                            entity.ID_PLACE = request.IdentityNumberAddress;
                            entity.DOMICILE = request.Domicile;
                            entity.BIRTH_REGIS_ADDRESS = request.BirthRegisAddress;
                            entity.NATIONALITY_ID = request.NationalityId;
                            entity.NATIVE_ID = request.NationId;
                            entity.EXAMINATION_DATE = request.DateExam;
                            entity.BIRTH_PLACE = request.BirthPlace;
                            entity.BIRTH_DATE = request.BirthDay;
                            _dbContext.SaveChanges();

                        }
                    }*/
                }
                //return new FormatedResponse() { InnerBody = true };
            }
            catch (Exception ex)
            {
                return new FormatedResponse() { MessageCode = CommonMessageCode.POST_UPDATE_FAILLED, ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400 };
            }
        }

        public async Task<FormatedResponse> GetBasic(long employeeId)
        {
            try
            {
                var getDateJoinCompany = (from item in _dbContext.HuContracts
                                          where item.EMPLOYEE_ID == employeeId
                                          orderby item.START_DATE ascending
                                          select item.START_DATE).FirstOrDefault();

                // declare list code of "official contract"
                //List<string> list = ["HXDTH12T", "HXDTH36T", "HXDTH60T", "HDKXDTH"]; code old
                List<string> list = ["HXDTH", "HDKXDTH"];

                // get list id of "official contract"
                var listOfficialContractId = _dbContext.HuContractTypes
                                            .Where(x => list.Contains(x.CODE))
                                            .Select(x => x.ID)
                                            .ToList();

                // get date of "official contract"
                // but it is the first
                // hđ ở tương lai thì k có ngày vào chính thức
                var getDateOfficialContract = (from item in _dbContext.HuContracts.Where(x => x.EMPLOYEE_ID == employeeId && x.STATUS_ID == OtherConfig.STATUS_APPROVE)
                                               from hct in _dbContext.HuContractTypes.Where(x => x.ID == item.CONTRACT_TYPE_ID)
                                               where listOfficialContractId.Contains(hct.ID)
                                               orderby item.START_DATE ascending
                                               select item.START_DATE).FirstOrDefault();
                //if (getDateOfficialContract!.Value.Date > DateTime.Now.Date)
                //{
                //    getDateOfficialContract = null;
                //}

                var query = await (from p in _dbContext.HuEmployees
                                   from q in _dbContext.HuEmployeeCvs.Where(x => x.ID == p.PROFILE_ID).DefaultIfEmpty()
                                   from o in _dbContext.HuOrganizations.Where(x => x.ID == p.ORG_ID).DefaultIfEmpty()
                                   from pos in _dbContext.HuPositions.Where(x => x.ID == p.POSITION_ID).DefaultIfEmpty()
                                   from j in _dbContext.HuJobs.Where(x => x.ID == pos.JOB_ID).DefaultIfEmpty()
                                   from di in _dbContext.HuPositions.Where(x => x.ID == pos.LM).DefaultIfEmpty()
                                   from p2 in _dbContext.HuEmployees.Where(x => x.POSITION_ID == di.ID && x.ORG_ID == di.ORG_ID && x.ID == di.MASTER).DefaultIfEmpty()
                                   from ep2 in _dbContext.HuEmployeeCvs.Where(x => x.ID == p2.PROFILE_ID).DefaultIfEmpty()
                                   from pos2 in _dbContext.HuPositions.Where(x => x.ID == p2.POSITION_ID).DefaultIfEmpty()
                                   from st in _dbContext.HuJobs.Where(x => x.ID == pos2.JOB_ID).DefaultIfEmpty()
                                   from com in _dbContext.HuCompanys.Where(x => x.ID == o.COMPANY_ID).DefaultIfEmpty()
                                   from ob in _dbContext.SysOtherLists.Where(x => x.ID == q.EMPLOYEE_OBJECT_ID).DefaultIfEmpty()

                                   where p.ID == employeeId
                                   select new
                                   {
                                       Id = employeeId,
                                       DirectManager = di != null ? ep2.FULL_NAME : "",
                                       EmployeeCode = p.CODE ?? "",
                                       IsNotContract = p.IS_NOT_CONTRACT == true ? "Có" : "Không",
                                       PositionDirectManager = di != null ? st.NAME_VN : "",
                                       Company = com.NAME_VN ?? "",
                                       Position = j.NAME_VN,
                                       ObjectEmployeeId = ob.ID,
                                       ObjectEmployee = ob.NAME,
                                       WorkingAddress = com.WORK_ADDRESS ?? "",
                                       OrgName = o.NAME ?? "",
                                       OtherName = q.OTHER_NAME ?? "",
                                       ItimeId = p.ITIME_ID,
                                       DateJoinCompany = getDateJoinCompany,
                                       DateOfficialContract = getDateOfficialContract
                                       //DateOfficialContract = p.JOIN_DATE_STATE == null ? getDateOfficialContract : p.JOIN_DATE_STATE,
                                   }).SingleAsync();
                return new() { InnerBody = query };
            }
            catch (Exception ex)
            {
                return new FormatedResponse() { MessageCode = CommonMessageCode.GET_METHOD_FAILED, ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400 };
            }

        }

        public async Task<FormatedResponse> GetPoliticalBackground(long employeeCvId)
        {
            try
            {
                var query = await (from c in _dbContext.HuEmployeeCvs
                                   where c.ID == employeeCvId
                                   select new
                                   {
                                       Id = employeeCvId,
                                       FamilyDetail = c.FAMILY_DETAIL,
                                       PrisonNote = c.PRISON_NOTE,
                                       Relations = c.RELATIONS,
                                       YellowFlag = c.YELLOW_FLAG,

                                   }).SingleAsync();
                return new() { InnerBody = query };
            }
            catch (Exception ex)
            {
                return new FormatedResponse() { MessageCode = CommonMessageCode.GET_METHOD_FAILED, ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400 };
            }
        }

        public async Task<FormatedResponse> GetPolitical(long id)
        {
            try
            {
                var query = await (from c in _dbContext.HuEmployeeCvs
                                   where c.ID == id
                                   select new
                                   {
                                       Id = id,
                                       FamilyDetail = c.FAMILY_DETAIL,
                                       PrisonNote = c.PRISON_NOTE,
                                       Relations = c.RELATIONS,
                                       YellowFlag = c.YELLOW_FLAG,
                                   }).SingleAsync();
                return new() { InnerBody = query };
            }
            catch (Exception ex)
            {
                return new FormatedResponse() { MessageCode = CommonMessageCode.GET_METHOD_FAILED, ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400 };
            }
        }

        public async Task<FormatedResponse> UpdatePolitical(StaffProfileUpdateDTO request)
        {
            try
            {
                var entity = _dbContext.HuEmployeeCvs.Where(x => x.ID == request.Id).FirstOrDefault();
                if (entity != null)
                {
                    entity.PRISON_NOTE = request.PrisonNote;
                    entity.FAMILY_DETAIL = request.FamilyDetail;
                    entity.RELATIONS = request.Relations;
                    entity.YELLOW_FLAG = request.YellowFlag;
                    _dbContext.SaveChanges();
                    return new() { MessageCode = CommonMessageCode.UPDATE_SUCCESS, InnerBody = true, StatusCode = EnumStatusCode.StatusCode200 };
                }
                else
                {
                    return new FormatedResponse() { MessageCode = CommonMessageCode.ENTITIES_NOT_FOUND, ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode500 };
                }
            }
            catch (Exception ex)
            {
                return new FormatedResponse() { MessageCode = CommonMessageCode.GET_METHOD_FAILED, ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400 };
            }
        }

        public async Task<FormatedResponse> GetPoliticalOrganization(long employeeCvId)
        {
            try
            {
                var query = await (from c in _dbContext.HuEmployeeCvs
                                   from s in _dbContext.SysOtherLists.Where(x => x.ID == c.GOVERMENT_MANAGEMENT_ID).DefaultIfEmpty()
                                   where c.ID == employeeCvId
                                   select new
                                   {
                                       Id = employeeCvId,
                                       IsUnionist = c.IS_UNIONIST == true ? "Là Đoàn viên" : "Không là Đoàn viên",
                                       UnionistPosition = c.UNIONIST_POSITION,
                                       UnionistDate = c.UNIONIST_DATE,
                                       UnionistAddress = c.UNIONIST_ADDRESS,
                                       IsJoinYouthGroup = c.IS_JOIN_YOUTH_GROUP == true ? "Đã tham gia công đoàn" : "Chưa tham gia công đoàn",
                                       IsMember = c.IS_MEMBER == true ? "Là Đảng viên" : "Không là Đảng viên",
                                       MemberPosition = c.MEMBER_POSITION,
                                       MemberAddress = c.MEMBER_ADDRESS,
                                       MemberDate = c.MEMBER_DATE,
                                       MemberOfficalDate = c.MEMBER_OFFICAL_DATE,
                                       GovernmentManagement = s.NAME,
                                       LivingCell = c.LIVING_CELL,
                                       CardNumber = c.CARD_NUMBER,
                                       PoliticalTheoryLevel = c.POLITICAL_THEORY_LEVEL,
                                       ResumeNumber = c.RESUME_NUMBER,
                                       VateransMemberDate = c.VATERANS_MEMBER_DATE,
                                       VateransPosition = c.VATERANS_POSITION,
                                       VateransAddress = c.VATERANS_ADDRESS,
                                       YouthGroupPosition = c.YOUTH_GROUP_POSITION,
                                       YouthGroupAddress = c.YOUTH_GROUP_ADDRESS,
                                       YouthGroupDate = c.YOUTH_GROUP_DATE,
                                       YouthSaveNationDate = c.YOUTH_SAVE_NATION_DATE,
                                       YouthSaveNationPosition = c.YOUTH_SAVE_NATION_POSITION,
                                       YouthSaveNationAddress = c.YOUTH_SAVE_NATION_ADDRESS,
                                       EnlistmentDate = c.ENLISTMENT_DATE,
                                       DischargeDate = c.DISCHARGE_DATE,
                                       HighestMilitaryPosition = c.HIGHEST_MILITARY_POSITION,
                                       CurrentPartyCommittee = c.CURRENT_PARTY_COMMITTEE,
                                       PartytimePartyCommittee = c.PARTYTIME_PARTY_COMMITTEE
                                   }).SingleAsync();
                return new() { InnerBody = query };
            }
            catch (Exception ex)
            {
                return new FormatedResponse() { MessageCode = CommonMessageCode.GET_METHOD_FAILED, ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400 };
            }
        }


        //public async Task<FormatedResponse> GetEducation(long employeeCvId)
        //{
        //    try
        //    {
        //        var query = await (from c in _dbContext.HuEmployeeCvs
        //                           from e in _dbContext.SysOtherLists.Where(x => x.ID == c.EDUCATION_LEVEL_ID).DefaultIfEmpty()
        //                           from z in _dbContext.SysOtherLists.Where(x => x.ID == c.LEARNING_LEVEL_ID).DefaultIfEmpty()
        //                           from q1 in _dbContext.SysOtherLists.Where(x => x.ID == c.QUALIFICATIONID).DefaultIfEmpty()
        //                           from q2 in _dbContext.SysOtherLists.Where(x => x.ID == c.QUALIFICATIONID_2).DefaultIfEmpty()
        //                           from q3 in _dbContext.SysOtherLists.Where(x => x.ID == c.QUALIFICATIONID_3).DefaultIfEmpty()
        //                           from t in _dbContext.SysOtherLists.Where(x => x.ID == c.TRAINING_FORM_ID).DefaultIfEmpty()
        //                           from t2 in _dbContext.SysOtherLists.Where(x => x.ID == c.TRAINING_FORM_ID_2).DefaultIfEmpty()
        //                           from t3 in _dbContext.SysOtherLists.Where(x => x.ID == c.TRAINING_FORM_ID_3).DefaultIfEmpty()
        //                           from l1 in _dbContext.SysOtherLists.Where(x => x.ID == c.LANGUAGE_ID).DefaultIfEmpty()
        //                           from l2 in _dbContext.SysOtherLists.Where(x => x.ID == c.LANGUAGE_ID_2).DefaultIfEmpty()
        //                           from l3 in _dbContext.SysOtherLists.Where(x => x.ID == c.LANGUAGE_ID_3).DefaultIfEmpty()
        //                           from lv1 in _dbContext.SysOtherLists.Where(x => x.ID == c.LANGUAGE_LEVEL_ID).DefaultIfEmpty()
        //                           from lv2 in _dbContext.SysOtherLists.Where(x => x.ID == c.LANGUAGE_LEVEL_ID_2).DefaultIfEmpty()
        //                           from lv3 in _dbContext.SysOtherLists.Where(x => x.ID == c.LANGUAGE_LEVEL_ID_3).DefaultIfEmpty()
        //                           from s in _dbContext.SysOtherLists.Where(x => x.ID == c.SCHOOL_ID).DefaultIfEmpty()
        //                           from s2 in _dbContext.SysOtherLists.Where(x => x.ID == c.SCHOOL_ID_2).DefaultIfEmpty()
        //                           from s3 in _dbContext.SysOtherLists.Where(x => x.ID == c.SCHOOL_ID_3).DefaultIfEmpty()
        //                           from reference_1 in _dbContext.SysOtherLists.Where(x => x.ID == c.COMPUTER_SKILL_ID).DefaultIfEmpty()
        //                           from reference_2 in _dbContext.SysOtherLists.Where(x => x.ID == c.LICENSE_ID).DefaultIfEmpty()
        //                           where c.ID == employeeCvId
        //                           select new
        //                           {
        //                               Id = employeeCvId,
        //                               EducationLevel = e.NAME,
        //                               LearningLevel = z.NAME,
        //                               ComputerSkill = reference_1.NAME,
        //                               License = reference_2.NAME,
        //                               Qualification1 = q1.NAME,
        //                               Qualification2 = q2.NAME,
        //                               Qualification3 = q3.NAME,
        //                               TrainingForm1 = t.NAME,
        //                               TrainingForm2 = t2.NAME,
        //                               TrainingForm3 = t3.NAME,
        //                               School1 = s.NAME,
        //                               School2 = s2.NAME,
        //                               School3 = s3.NAME,
        //                               Language1 = l1.NAME,
        //                               Language2 = l2.NAME,
        //                               Language3 = l3.NAME,
        //                               LanguageLevel1 = lv1.NAME,
        //                               LanguageLevel2 = lv2.NAME,
        //                               LanguageLevel3 = lv3.NAME,
        //                           }).SingleAsync();
        //        return new() { InnerBody = query };
        //    }
        //    catch (Exception ex)
        //    {
        //        return new FormatedResponse() { MessageCode = CommonMessageCode.GET_METHOD_FAILED, ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400 };

        //    }
        //}


        #region dev congnc
        public async Task<FormatedResponse> GetEducation(long employeeCvId)
        {
            try
            {
                var huEmployee = _uow.Context.Set<HU_EMPLOYEE>().Where(x => x.PROFILE_ID == employeeCvId);
                var huCertificate = _uow.Context.Set<HU_CERTIFICATE>().AsQueryable();
                var sysOtherList = _uow.Context.Set<SYS_OTHER_LIST>().AsQueryable();

                var certificates = await (from item in huEmployee
                                          from p in huCertificate.Where(x => x.EMPLOYEE_ID == item.ID && x.IS_PRIME == true)
                                          from reference_1 in sysOtherList.Where(x => x.ID == p.LEVEL_ID).DefaultIfEmpty()
                                          from reference_2 in sysOtherList.Where(x => x.ID == p.LEVEL_TRAIN).DefaultIfEmpty()
                                          from reference_3 in sysOtherList.Where(x => x.ID == p.TYPE_TRAIN).DefaultIfEmpty()
                                          from reference_4 in sysOtherList.Where(x => x.ID == p.SCHOOL_ID).DefaultIfEmpty()

                                          select new
                                          {
                                              // trình độ học vấn
                                              LevelName = reference_1.NAME,

                                              // trình độ chuyên môn
                                              LevelTrainName = reference_2.NAME,

                                              // hình thức đào tạo
                                              TypeTrainName = reference_3.NAME,

                                              // đơn vị đào tạo
                                              SchoolName = reference_4.NAME
                                          })
                               .ToListAsync();

                List<List<ItemDTO>> list_parent = new List<List<ItemDTO>>();

                for (int i = 1; i <= 10; i++)
                {
                    List<ItemDTO> list_child = new List<ItemDTO>();

                    if (i <= certificates.Count())
                    {
                        int index = i - 1;

                        // trình độ học vấn
                        list_child.Add(
                            new ItemDTO()
                            {
                                controlType = "TEXT",
                                field = $"learningLevel{i}",
                                flexSize = 3,
                                label = $"UI_EDUCATION_LEVEL_ID_{i}",
                                labelFlexSize = 0,
                                value = certificates[index].LevelName
                            }
                        );

                        // trình độ chuyên môn
                        list_child.Add(
                            new ItemDTO()
                            {
                                controlType = "TEXT",
                                field = $"qualification{i}",
                                flexSize = 3,
                                label = $"UI_EDUCATION_LEVEL_TRAIN_{i}",
                                labelFlexSize = 0,
                                value = certificates[index].LevelTrainName
                            }
                        );

                        // hình thức đào tạo
                        list_child.Add(
                            new ItemDTO()
                            {
                                controlType = "TEXT",
                                field = $"trainingForm{i}",
                                flexSize = 3,
                                label = $"UI_EDUCATION_TYPE_TRAIN_{i}",
                                labelFlexSize = 0,
                                value = certificates[index].TypeTrainName
                            }
                        );

                        // đơn vị đào tạo
                        list_child.Add(
                            new ItemDTO()
                            {
                                controlType = "TEXT",
                                field = $"school{i}",
                                flexSize = 3,
                                label = $"UI_EDUCATION_SCHOOL_ID_{i}",
                                labelFlexSize = 0,
                                value = certificates[index].SchoolName
                            }
                        );

                        list_parent.Add(list_child);
                    }
                    else
                    {
                        // trình độ học vấn
                        list_child.Add(
                            new ItemDTO()
                            {
                                controlType = "TEXT",
                                field = "",
                                flexSize = 3,
                                label = $"UI_EDUCATION_LEVEL_ID_{i}",
                                labelFlexSize = 0,
                                value = ""
                            }
                        );

                        // trình độ chuyên môn
                        list_child.Add(
                            new ItemDTO()
                            {
                                controlType = "TEXT",
                                field = "",
                                flexSize = 3,
                                label = $"UI_EDUCATION_LEVEL_TRAIN_{i}",
                                labelFlexSize = 0,
                                value = ""
                            }
                        );

                        // hình thức đào tạo
                        list_child.Add(
                            new ItemDTO()
                            {
                                controlType = "TEXT",
                                field = "",
                                flexSize = 3,
                                label = $"UI_EDUCATION_TYPE_TRAIN_{i}",
                                labelFlexSize = 0,
                                value = ""
                            }
                        );

                        // đơn vị đào tạo
                        list_child.Add(
                            new ItemDTO()
                            {
                                controlType = "TEXT",
                                field = "",
                                flexSize = 3,
                                label = $"UI_EDUCATION_SCHOOL_ID_{i}",
                                labelFlexSize = 0,
                                value = ""
                            }
                        );

                        list_parent.Add(list_child);
                    }
                }


                var query = await (from c in _dbContext.HuEmployeeCvs
                                   from e in _dbContext.SysOtherLists.Where(x => x.ID == c.EDUCATION_LEVEL_ID).DefaultIfEmpty()
                                   from z in _dbContext.SysOtherLists.Where(x => x.ID == c.LEARNING_LEVEL_ID).DefaultIfEmpty()
                                   from q1 in _dbContext.SysOtherLists.Where(x => x.ID == c.QUALIFICATIONID).DefaultIfEmpty()
                                   from q2 in _dbContext.SysOtherLists.Where(x => x.ID == c.QUALIFICATIONID_2).DefaultIfEmpty()
                                   from q3 in _dbContext.SysOtherLists.Where(x => x.ID == c.QUALIFICATIONID_3).DefaultIfEmpty()
                                   from t in _dbContext.SysOtherLists.Where(x => x.ID == c.TRAINING_FORM_ID).DefaultIfEmpty()
                                   from t2 in _dbContext.SysOtherLists.Where(x => x.ID == c.TRAINING_FORM_ID_2).DefaultIfEmpty()
                                   from t3 in _dbContext.SysOtherLists.Where(x => x.ID == c.TRAINING_FORM_ID_3).DefaultIfEmpty()
                                   from l1 in _dbContext.SysOtherLists.Where(x => x.ID == c.LANGUAGE_ID).DefaultIfEmpty()
                                   from l2 in _dbContext.SysOtherLists.Where(x => x.ID == c.LANGUAGE_ID_2).DefaultIfEmpty()
                                   from l3 in _dbContext.SysOtherLists.Where(x => x.ID == c.LANGUAGE_ID_3).DefaultIfEmpty()
                                   from lv1 in _dbContext.SysOtherLists.Where(x => x.ID == c.LANGUAGE_LEVEL_ID).DefaultIfEmpty()
                                   from lv2 in _dbContext.SysOtherLists.Where(x => x.ID == c.LANGUAGE_LEVEL_ID_2).DefaultIfEmpty()
                                   from lv3 in _dbContext.SysOtherLists.Where(x => x.ID == c.LANGUAGE_LEVEL_ID_3).DefaultIfEmpty()
                                   from s in _dbContext.SysOtherLists.Where(x => x.ID == c.SCHOOL_ID).DefaultIfEmpty()
                                   from s2 in _dbContext.SysOtherLists.Where(x => x.ID == c.SCHOOL_ID_2).DefaultIfEmpty()
                                   from s3 in _dbContext.SysOtherLists.Where(x => x.ID == c.SCHOOL_ID_3).DefaultIfEmpty()
                                   from reference_1 in _dbContext.SysOtherLists.Where(x => x.ID == c.COMPUTER_SKILL_ID).DefaultIfEmpty()
                                   from reference_2 in _dbContext.SysOtherLists.Where(x => x.ID == c.LICENSE_ID).DefaultIfEmpty()
                                   where c.ID == employeeCvId
                                   select new
                                   {
                                       Id = employeeCvId,
                                       EducationLevel = e.NAME,
                                       ComputerSkill = reference_1.NAME,
                                       License = reference_2.NAME,

                                       // trình độ học vấn 1
                                       LearningLevel1 = list_parent[0][0].value,
                                       Qualification1 = list_parent[0][1].value,
                                       TrainingForm1 = list_parent[0][2].value,
                                       School1 = list_parent[0][3].value,

                                       // trình độ học vấn 2
                                       LearningLevel2 = list_parent[1][0].value,
                                       Qualification2 = list_parent[1][1].value,
                                       TrainingForm2 = list_parent[1][2].value,
                                       School2 = list_parent[1][3].value,

                                       // trình độ học vấn 3
                                       LearningLevel3 = list_parent[2][0].value,
                                       Qualification3 = list_parent[2][1].value,
                                       TrainingForm3 = list_parent[2][2].value,
                                       School3 = list_parent[2][3].value,

                                       // trình độ học vấn 4
                                       LearningLevel4 = list_parent[3][0].value,
                                       Qualification4 = list_parent[3][1].value,
                                       TrainingForm4 = list_parent[3][2].value,
                                       School4 = list_parent[3][3].value,

                                       // trình độ học vấn 5
                                       LearningLevel5 = list_parent[4][0].value,
                                       Qualification5 = list_parent[4][1].value,
                                       TrainingForm5 = list_parent[4][2].value,
                                       School5 = list_parent[4][3].value,

                                       // trình độ học vấn 6
                                       LearningLevel6 = list_parent[5][0].value,
                                       Qualification6 = list_parent[5][1].value,
                                       TrainingForm6 = list_parent[5][2].value,
                                       School6 = list_parent[5][3].value,

                                       // trình độ học vấn 7
                                       LearningLevel7 = list_parent[6][0].value,
                                       Qualification7 = list_parent[6][1].value,
                                       TrainingForm7 = list_parent[6][2].value,
                                       School7 = list_parent[6][3].value,

                                       // trình độ học vấn 8
                                       LearningLevel8 = list_parent[7][0].value,
                                       Qualification8 = list_parent[7][1].value,
                                       TrainingForm8 = list_parent[7][2].value,
                                       School8 = list_parent[7][3].value,

                                       // trình độ học vấn 9
                                       LearningLevel9 = list_parent[8][0].value,
                                       Qualification9 = list_parent[8][1].value,
                                       TrainingForm9 = list_parent[8][2].value,
                                       School9 = list_parent[8][3].value,

                                       // trình độ học vấn 10
                                       LearningLevel10 = list_parent[9][0].value,
                                       Qualification10 = list_parent[9][1].value,
                                       TrainingForm10 = list_parent[9][2].value,
                                       School10 = list_parent[9][3].value,
                                   }).FirstAsync();

                return new() { InnerBody = query };
            }
            catch (Exception ex)
            {
                return new FormatedResponse() { MessageCode = CommonMessageCode.GET_METHOD_FAILED, ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400 };

            }
        }
        #endregion


        public async Task<FormatedResponse> GetEducationId(long id)
        {
            try
            {
                var query = await (from c in _dbContext.HuEmployeeCvs
                                   from e in _dbContext.SysOtherLists.Where(x => x.ID == c.EDUCATION_LEVEL_ID).DefaultIfEmpty()
                                   from z in _dbContext.SysOtherLists.Where(x => x.ID == c.LEARNING_LEVEL_ID).DefaultIfEmpty()
                                   from q1 in _dbContext.SysOtherLists.Where(x => x.ID == c.QUALIFICATIONID).DefaultIfEmpty()
                                   from q2 in _dbContext.SysOtherLists.Where(x => x.ID == c.QUALIFICATIONID_2).DefaultIfEmpty()
                                   from q3 in _dbContext.SysOtherLists.Where(x => x.ID == c.QUALIFICATIONID_3).DefaultIfEmpty()
                                   from t in _dbContext.SysOtherLists.Where(x => x.ID == c.TRAINING_FORM_ID).DefaultIfEmpty()
                                   from t2 in _dbContext.SysOtherLists.Where(x => x.ID == c.TRAINING_FORM_ID_2).DefaultIfEmpty()
                                   from t3 in _dbContext.SysOtherLists.Where(x => x.ID == c.TRAINING_FORM_ID_3).DefaultIfEmpty()
                                   from l1 in _dbContext.SysOtherLists.Where(x => x.ID == c.LANGUAGE_ID).DefaultIfEmpty()
                                   from l2 in _dbContext.SysOtherLists.Where(x => x.ID == c.LANGUAGE_ID_2).DefaultIfEmpty()
                                   from l3 in _dbContext.SysOtherLists.Where(x => x.ID == c.LANGUAGE_ID_3).DefaultIfEmpty()
                                   from lv1 in _dbContext.SysOtherLists.Where(x => x.ID == c.LANGUAGE_LEVEL_ID).DefaultIfEmpty()
                                   from lv2 in _dbContext.SysOtherLists.Where(x => x.ID == c.LANGUAGE_LEVEL_ID_2).DefaultIfEmpty()
                                   from lv3 in _dbContext.SysOtherLists.Where(x => x.ID == c.LANGUAGE_LEVEL_ID_3).DefaultIfEmpty()
                                   from s in _dbContext.SysOtherLists.Where(x => x.ID == c.SCHOOL_ID).DefaultIfEmpty()
                                   from s2 in _dbContext.SysOtherLists.Where(x => x.ID == c.SCHOOL_ID_2).DefaultIfEmpty()
                                   from s3 in _dbContext.SysOtherLists.Where(x => x.ID == c.SCHOOL_ID_3).DefaultIfEmpty()
                                   where c.ID == id
                                   select new
                                   {
                                       Id = id,
                                       EducationLevelId = e.ID,
                                       LearningLevelId = z.ID,
                                       ComputerSkill = c.COMPUTER_SKILL,
                                       ComputerSkillId = c.COMPUTER_SKILL_ID,
                                       License = c.LICENSE,
                                       QualificationId = q1.ID,
                                       QualificationId2 = q2.ID,
                                       QualificationId3 = q3.ID,
                                       TrainingFormId = t.ID,
                                       TrainingFormId2 = t2.ID,
                                       TrainingFormId3 = t3.ID,
                                       SchoolId = s.ID,
                                       SchoolId2 = s2.ID,
                                       SchoolId3 = s3.ID,
                                       LanguageId = l1.ID,
                                       LanguageId2 = l2.ID,
                                       LanguageId3 = l3.ID,
                                       LanguageLevelId = lv1.ID,
                                       LanguageLevelId2 = lv2.ID,
                                       LanguageLevelId3 = lv3.ID,
                                       LicenseId = c.LICENSE_ID
                                   }).SingleAsync();
                return new() { InnerBody = query };
            }
            catch (Exception ex)
            {
                return new FormatedResponse() { MessageCode = CommonMessageCode.GET_METHOD_FAILED, ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400 };

            }
        }

        public async Task<FormatedResponse> UpdateEducationId(StaffProfileUpdateDTO request, string sid)
        {
            try
            {
                var entity = await _dbContext.HuEmployeeCvs.Where(x => x.ID == request.Id).FirstOrDefaultAsync();
                if (entity != null)
                {
                    entity.UPDATED_DATE = DateTime.Now;
                    entity.UPDATED_BY = sid;
                    entity.EDUCATION_LEVEL_ID = request.EducationLevelId;
                    entity.LEARNING_LEVEL_ID = request.LearningLevelId;
                    entity.COMPUTER_SKILL = request.ComputerSkill;
                    entity.COMPUTER_SKILL_ID = request.ComputerSkillId;
                    entity.LICENSE = request.License;
                    entity.LICENSE_ID = request.LicenseId;
                    entity.QUALIFICATIONID = request.QualificationId;
                    entity.QUALIFICATIONID_2 = request.QualificationId2;
                    entity.QUALIFICATIONID_3 = request.QualificationId3;
                    entity.TRAINING_FORM_ID = request.TrainingFormId;
                    entity.TRAINING_FORM_ID_2 = request.TrainingFormId2;
                    entity.TRAINING_FORM_ID_3 = request.TrainingFormId3;
                    entity.SCHOOL_ID = request.SchoolId;
                    entity.SCHOOL_ID_2 = request.SchoolId2;
                    entity.SCHOOL_ID_3 = request.SchoolId3;
                    entity.LANGUAGE_ID = request.LanguageId;
                    entity.LANGUAGE_ID_2 = request.LanguageId2;
                    entity.LANGUAGE_ID_3 = request.LanguageId3;
                    entity.LANGUAGE_LEVEL_ID = request.LanguageLevelId;
                    entity.LANGUAGE_LEVEL_ID_2 = request.LanguageLevelId2;
                    entity.LANGUAGE_LEVEL_ID_3 = request.LanguageLevelId3;
                    _dbContext.SaveChanges();
                    return new() { MessageCode = CommonMessageCode.UPDATE_SUCCESS, InnerBody = true, StatusCode = EnumStatusCode.StatusCode200 };

                }
                else
                {
                    return new FormatedResponse() { MessageCode = CommonMessageCode.ENTITIES_NOT_FOUND, ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400 };
                }
            }
            catch (Exception ex)
            {
                return new FormatedResponse() { MessageCode = CommonMessageCode.POST_UPDATE_FAILLED, ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400 };
            }
        }

        public async Task<FormatedResponse> GetPresenter(long employeeCvId)
        {
            var query = await (from c in _dbContext.HuEmployeeCvs
                               from d in _dbContext.HuEmployees.Where(x => x.ID == c.PRESENTER).DefaultIfEmpty()
                               from v in _dbContext.HuEmployeeCvs.Where(x => x.ID == d.PROFILE_ID).DefaultIfEmpty()
                               where c.ID == employeeCvId
                               select new
                               {
                                   Id = employeeCvId,
                                   Presenter = v.FULL_NAME,
                                   PresenterAddress = c.PRESENTER_ADDRESS,
                                   PresenterPhoneNumber = c.PRESENTER_PHONE_NUMBER
                               }).SingleAsync();
            return new() { InnerBody = query };
        }

        public async Task<FormatedResponse> GetContact(long employeeCvId)
        {
            var query = await (from c in _dbContext.HuEmployeeCvs
                               from s in _dbContext.HuProvinces.Where(x => x.ID == c.PROVINCE_ID).DefaultIfEmpty()
                               from s1 in _dbContext.HuProvinces.Where(x => x.ID == c.CUR_PROVINCE_ID).DefaultIfEmpty()
                               from w in _dbContext.HuWards.Where(x => x.ID == c.WARD_ID).DefaultIfEmpty()
                               from w1 in _dbContext.HuWards.Where(x => x.ID == c.CUR_WARD_ID).DefaultIfEmpty()
                               from d1 in _dbContext.HuDistricts.Where(x => x.ID == c.CUR_DISTRICT_ID).DefaultIfEmpty()
                               from d2 in _dbContext.HuDistricts.Where(x => x.ID == c.DISTRICT_ID).DefaultIfEmpty()
                               where c.ID == employeeCvId
                               select new
                               {
                                   Id = employeeCvId,
                                   IsHost = c.IS_HOST == true ? "Là chủ hộ" : "Không phải chủ hộ",
                                   Telephone = c.MOBILE_PHONE,
                                   HouseholdCode = c.HOUSEHOLD_CODE,
                                   HouseholdNumber = c.HOUSEHOLD_NUMBER,
                                   Address = c.ADDRESS,
                                   CurAddress = c.CUR_ADDRESS,
                                   Province = s.NAME,
                                   CurProvince = s1.NAME,
                                   Ward = w.NAME,
                                   CurWard = w1.NAME,
                                   CurDistrict = d1.NAME,
                                   District = d2.NAME,
                                   LandlinePhone = c.MOBILE_PHONE_LAND,
                                   EmailCompany = c.WORK_EMAIL,
                                   EmailPersonal = c.EMAIL
                               }).SingleAsync();
            return new() { InnerBody = query };
        }

        public async Task<FormatedResponse> GetContactId(long id)
        {
            try
            {
                var query = await (from c in _dbContext.HuEmployeeCvs
                                       //from s in _dbContext.HuProvinces.Where(x => x.ID == c.PROVINCE_ID).DefaultIfEmpty()
                                       //from s1 in _dbContext.HuProvinces.Where(x => x.ID == c.CUR_PROVINCE_ID).DefaultIfEmpty()
                                       //from w in _dbContext.HuWards.Where(x => x.ID == c.WARD_ID).DefaultIfEmpty()
                                       //from w1 in _dbContext.HuWards.Where(x => x.ID == c.CUR_WARD_ID).DefaultIfEmpty()
                                       //from d1 in _dbContext.HuDistricts.Where(x => x.ID == c.CUR_DISTRICT_ID).DefaultIfEmpty()
                                       //from d2 in _dbContext.HuDistricts.Where(x => x.ID == c.DISTRICT_ID).DefaultIfEmpty()
                                   where c.ID == id
                                   select new
                                   {
                                       Id = id,
                                       IsHost = c.IS_HOST,
                                       Telephone = c.MOBILE_PHONE,
                                       HouseholdCode = c.HOUSEHOLD_CODE,
                                       HouseholdNumber = c.HOUSEHOLD_NUMBER,
                                       Address = c.ADDRESS,
                                       CurAddress = c.CUR_ADDRESS,
                                       ProvinceId = c.PROVINCE_ID,
                                       CurProvinceId = c.CUR_PROVINCE_ID,
                                       WardId = c.WARD_ID,
                                       CurWardId = c.CUR_WARD_ID,
                                       CurDistrictId = c.CUR_DISTRICT_ID,
                                       DistrictId = c.DISTRICT_ID,
                                       LandlinePhone = c.MOBILE_PHONE_LAND,
                                       EmailCompany = c.WORK_EMAIL,
                                       EmailPersonal = c.EMAIL
                                   }).SingleAsync();
                return new() { InnerBody = query };
            }
            catch (Exception ex)
            {
                return new() { InnerBody = null };
            }

        }

        public async Task<FormatedResponse> UpdateContactId(StaffProfileUpdateDTO request, string sid)
        {
            var entity = await _dbContext.HuEmployeeCvs.Where(x => x.ID == request.Id).FirstOrDefaultAsync();
            if (entity != null)
            {
                entity.IS_HOST = request.IsHost;
                entity.UPDATED_DATE = DateTime.Now;
                entity.UPDATED_BY = sid;
                entity.HOUSEHOLD_CODE = request.HouseholdCode;
                entity.HOUSEHOLD_NUMBER = request.HouseholdNumber;
                entity.ADDRESS = request.Address;
                entity.CUR_ADDRESS = request.CurAddress;
                entity.PROVINCE_ID = request.ProvinceId;
                entity.CUR_PROVINCE_ID = request.CurProvinceId;
                entity.WARD_ID = request.WardId;
                entity.CUR_WARD_ID = request.CurWardId;
                entity.DISTRICT_ID = request.DistrictId;
                entity.CUR_DISTRICT_ID = request.CurDistrictId;
                entity.MOBILE_PHONE = request.Telephone;
                entity.MOBILE_PHONE_LAND = request.LandlinePhone;
                entity.WORK_EMAIL = request.EmailCompany;
                entity.EMAIL = request.EmailPersonal;
                _dbContext.SaveChanges();
                return new() { MessageCode = CommonMessageCode.UPDATE_SUCCESS, InnerBody = true, StatusCode = EnumStatusCode.StatusCode200 };
            }
            else
            {
                return new FormatedResponse() { MessageCode = CommonMessageCode.ENTITIES_NOT_FOUND, ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400 };
            }
        }

        public async Task<FormatedResponse> UpdateSituationId(StaffProfileUpdateDTO request)
        {
            var entity = _dbContext.HuEmployeeCvs.Where(x => x.ID == request.Id).FirstOrDefault();
            if (entity != null)
            {
                entity.MAIN_INCOME = request.MainIncome;
                entity.OTHER_SOURCES = request.OtherSources;
                entity.LAND_GRANTED = request.LandGranted;
                entity.TAX_GRANTED_HOUSE = request.TaxGrantedHouse;
                entity.TOTAL_AREA = request.TotalArea;
                entity.SELF_PURCHASE_LAND = request.SelfPurchaseLand;
                entity.SELF_BUILD_HOUSE = request.SelfBuildHouse;
                entity.TOTAL_APP_AREA = request.TotalAppArea;
                entity.LAND_FOR_PRODUCTION = request.LandForProduction;
                entity.ADDITIONAL_INFOMATION = request.AdditionalInformation;
                _dbContext.SaveChanges();
                return new() { MessageCode = CommonMessageCode.UPDATE_SUCCESS, InnerBody = true, StatusCode = EnumStatusCode.StatusCode200 };
            }
            else
            {
                return new FormatedResponse() { MessageCode = CommonMessageCode.ENTITIES_NOT_FOUND, ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400 };
            }
        }

        public async Task<FormatedResponse> GetSituationId(long id)
        {
            var query = await (from c in _dbContext.HuEmployeeCvs
                               where c.ID == id
                               select new
                               {
                                   Id = id,
                                   MainIncome = c.MAIN_INCOME,
                                   OtherSources = c.OTHER_SOURCES,
                                   LandGranted = c.LAND_GRANTED,
                                   TaxGrantedHouse = c.TAX_GRANTED_HOUSE,
                                   TotalArea = c.TOTAL_AREA,
                                   SelfPurchaseLand = c.SELF_PURCHASE_LAND,
                                   SelfBuildHouse = c.SELF_BUILD_HOUSE,
                                   TotalAppArea = c.TOTAL_APP_AREA,
                                   LandForProduction = c.LAND_FOR_PRODUCTION,
                                   AdditionalInformation = c.ADDITIONAL_INFOMATION
                               }).SingleAsync();
            return new() { InnerBody = query };
        }

        public async Task<FormatedResponse> GetSituation(long employeeCvId)
        {
            var query = await (from c in _dbContext.HuEmployeeCvs
                               where c.ID == employeeCvId
                               select new
                               {
                                   Id = employeeCvId,
                                   MainIncome = c.MAIN_INCOME,
                                   OtherSources = c.OTHER_SOURCES,
                                   LandGranted = c.LAND_GRANTED,
                                   TaxGrantedHouse = c.TAX_GRANTED_HOUSE,
                                   TotalArea = c.TOTAL_AREA,
                                   SelfPurchaseLand = c.SELF_PURCHASE_LAND,
                                   SelfBuildHouse = c.SELF_BUILD_HOUSE,
                                   TotalAppArea = c.TOTAL_APP_AREA,
                                   LandForProduction = c.LAND_FOR_PRODUCTION,
                                   AdditionalInformation = c.ADDITIONAL_INFOMATION
                               }).SingleAsync();
            return new() { InnerBody = query };
        }


        public async Task<FormatedResponse> GetPresenterId(long id)
        {
            var query = await (from c in _dbContext.HuEmployeeCvs
                               where c.ID == id
                               select new
                               {
                                   Id = id,
                                   Presenter = c.PRESENTER,
                                   PresenterAddress = c.PRESENTER_ADDRESS,
                                   PresenterPhoneNumber = c.PRESENTER_PHONE_NUMBER
                               }).SingleAsync();
            return new() { InnerBody = query };
        }

        public async Task<FormatedResponse> UpdatePresenterId(StaffProfileUpdateDTO request)
        {
            var entity = _dbContext.HuEmployeeCvs.Where(x => x.ID == request.Id).FirstOrDefault();
            if (entity != null)
            {
                entity.PRESENTER = request.Presenter;
                entity.PRESENTER_ADDRESS = request.PresenterAddress;
                entity.PRESENTER_PHONE_NUMBER = request.PresenterPhoneNumber;
                _dbContext.SaveChanges();
                return new() { MessageCode = CommonMessageCode.UPDATE_SUCCESS, InnerBody = true, StatusCode = EnumStatusCode.StatusCode200 };
            }
            else
            {
                return new FormatedResponse() { MessageCode = CommonMessageCode.ENTITIES_NOT_FOUND, ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400 };

            }
        }



        public async Task<FormatedResponse> GetPoliticalOrganizationId(long id)
        {
            try
            {
                var query = await (from c in _dbContext.HuEmployeeCvs
                                   where c.ID == id
                                   select new
                                   {
                                       Id = id,
                                       IsUnionist = c.IS_UNIONIST,
                                       UnionistPosition = c.UNIONIST_POSITION,
                                       UnionistDate = c.UNIONIST_DATE,
                                       UnionistAddress = c.UNIONIST_ADDRESS,
                                       IsJoinYouthGroup = c.IS_JOIN_YOUTH_GROUP,
                                       IsMember = c.IS_MEMBER,
                                       MemberPosition = c.MEMBER_POSITION,
                                       MemberAddress = c.MEMBER_ADDRESS,
                                       MemberDate = c.MEMBER_DATE,
                                       MemberOfficalDate = c.MEMBER_OFFICAL_DATE,
                                       LivingCell = c.LIVING_CELL,
                                       CardNumber = c.CARD_NUMBER,
                                       PoliticalTheoryLevel = c.POLITICAL_THEORY_LEVEL,
                                       ResumeNumber = c.RESUME_NUMBER,
                                       VateransMemberDate = c.VATERANS_MEMBER_DATE,
                                       VateransPosition = c.VATERANS_POSITION,
                                       VateransAddress = c.VATERANS_ADDRESS,
                                       EnlistmentDate = c.ENLISTMENT_DATE,
                                       DischargeDate = c.DISCHARGE_DATE,
                                       HighestMilitaryPosition = c.HIGHEST_MILITARY_POSITION,
                                       CurrentPartyCommittee = c.CURRENT_PARTY_COMMITTEE,
                                       PartytimePartyCommittee = c.PARTYTIME_PARTY_COMMITTEE,
                                       YouthGroupAddress = c.YOUTH_GROUP_ADDRESS,
                                       YouthGroupDate = c.YOUTH_GROUP_DATE,
                                       YouthGroupPosition = c.YOUTH_GROUP_POSITION,
                                       YouthSaveNationDate = c.YOUTH_SAVE_NATION_DATE,
                                       YouthSaveNationPosition = c.YOUTH_SAVE_NATION_POSITION,
                                       YouthSaveNationAddress = c.YOUTH_SAVE_NATION_ADDRESS,
                                       GovermentManagementId = c.GOVERMENT_MANAGEMENT_ID,

                                   }).SingleAsync();
                return new() { InnerBody = query };
            }
            catch (Exception ex)
            {
                return new FormatedResponse() { MessageCode = CommonMessageCode.GET_METHOD_FAILED, ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400 };
            }
        }
        public async Task<FormatedResponse> GetAll()
        {
            try
            {
                var entity = _uow.Context.Set<HU_EMPLOYEE_CV>().AsNoTracking().AsQueryable();
                var joined = from p in entity
                             select new
                             {
                                 Id = p.ID,
                                 Name = p.FULL_NAME
                             };
                var response = await joined.ToListAsync();
                return new FormatedResponse()
                {
                    InnerBody = response,
                    StatusCode = EnumStatusCode.StatusCode200
                };
            }
            catch (Exception ex)
            {
                return new FormatedResponse() { MessageCode = ex.Message, ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400 };
            }
        }

        public async Task<FormatedResponse> GetAllIgnoreCurrentUser(string sid)
        {
            try
            {
                var empAppId = (from p in _dbContext.SysUsers where p.ID == sid select p.EMPLOYEE_ID).FirstOrDefault();
                var empProfileId = (from p in _dbContext.HuEmployees where p.ID == empAppId select p.PROFILE_ID).FirstOrDefault();
                var entity = _uow.Context.Set<HU_EMPLOYEE_CV>().AsNoTracking().AsQueryable();
                var emp = _uow.Context.Set<HU_EMPLOYEE>().AsNoTracking().AsQueryable();
                var joined = from p in entity
                             join e in emp on p.ID equals e.PROFILE_ID
                             where p.ID != empProfileId
                             select new
                             {
                                 Id = e.ID,
                                 Code = e.CODE,
                                 Name = "[" + e.CODE + "] " + p.FULL_NAME
                             };
                var response = await joined.ToListAsync();
                return new FormatedResponse()
                {
                    InnerBody = response,
                    StatusCode = EnumStatusCode.StatusCode200
                };
            }
            catch (Exception ex)
            {
                return new FormatedResponse() { MessageCode = ex.Message, ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400 };
            }
        }

        public async Task<FormatedResponse> UpdatePoliticalOrganizationId(StaffProfileUpdateDTO request)
        {
            try
            {
                var entity = _dbContext.HuEmployeeCvs.Where(x => x.ID == request.Id).FirstOrDefault();
                if (entity != null)
                {
                    entity.IS_UNIONIST = request.IsUnionist;
                    entity.UNIONIST_POSITION = request.UnionistPosition;
                    entity.UNIONIST_DATE = request.UnionistDate;
                    entity.UNIONIST_ADDRESS = request.UnionistAddress;
                    entity.IS_JOIN_YOUTH_GROUP = request.IsJoinYouthGroup;
                    entity.IS_MEMBER = request.IsMember;
                    entity.MEMBER_POSITION = request.MemberPosition;
                    entity.MEMBER_ADDRESS = request.MemberAddress;
                    entity.MEMBER_DATE = request.MemberDate;
                    entity.MEMBER_OFFICAL_DATE = request.MemberOfficalDate;
                    entity.LIVING_CELL = request.LivingCell;
                    entity.CARD_NUMBER = request.CardNumber;
                    entity.POLITICAL_THEORY_LEVEL = request.PoliticalTheoryLevel;
                    entity.RESUME_NUMBER = request.ResumeNumber;
                    entity.VATERANS_MEMBER_DATE = request.VateransMemberDate;
                    entity.VATERANS_POSITION = request.VateransPosition;
                    entity.VATERANS_ADDRESS = request.VateransAddress;
                    entity.ENLISTMENT_DATE = request.EnlistmentDate;
                    entity.DISCHARGE_DATE = request.DischargeDate;
                    entity.HIGHEST_MILITARY_POSITION = request.HighestMilitaryPosition;
                    entity.CURRENT_PARTY_COMMITTEE = request.CurrentPartyCommittee;
                    entity.PARTYTIME_PARTY_COMMITTEE = request.PartytimePartyCommittee;
                    entity.GOVERMENT_MANAGEMENT_ID = request.GovernmentManagementId;
                    entity.YOUTH_GROUP_ADDRESS = request.YouthGroupAddress;
                    entity.YOUTH_GROUP_POSITION = request.YouthGroupPosition;
                    entity.YOUTH_GROUP_DATE = request.YouthGroupDate;
                    entity.YOUTH_SAVE_NATION_DATE = request.YouthSaveNationDate;
                    entity.YOUTH_SAVE_NATION_POSITION = request.YouthSaveNationPosition;
                    entity.YOUTH_SAVE_NATION_ADDRESS = request.YouthSaveNationAddress;

                    _dbContext.SaveChanges();
                    return new() { MessageCode = CommonMessageCode.UPDATE_SUCCESS, InnerBody = true, StatusCode = EnumStatusCode.StatusCode200 };
                }
                else
                {
                    return new FormatedResponse() { MessageCode = CommonMessageCode.ENTITIES_NOT_FOUND, ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400 };
                }
            }
            catch (Exception ex)
            {
                return new FormatedResponse() { MessageCode = CommonMessageCode.POST_UPDATE_FAILLED, ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400 };
            }
        }

        public async Task<FormatedResponse> GetGeneralInfo(long id)
        {
            try
            {
                var getDateJoinCompany = (from item in _dbContext.HuContracts
                                          where item.EMPLOYEE_ID == id
                                          orderby item.START_DATE ascending
                                          select item.START_DATE).FirstOrDefault();

                // declare list code of "official contract"
                List<string> list = ["HXDTH12T", "HXDTH36T", "HXDTH60T", "HDKXDTH", "HDKXDTH", "HXDTH"];

                // get list id of "official contract"
                var listOfficialContractId = _dbContext.HuContractTypes
                                            .Where(x => list.Contains(x.CODE))
                                            .Select(x => x.ID)
                                            .ToList();

                // get date of "official contract"
                // but it is the first
                var getDateOfficialContract = (from item in _dbContext.HuContracts.Where(x => x.EMPLOYEE_ID == id)
                                               from hct in _dbContext.HuContractTypes.Where(x => x.ID == item.CONTRACT_TYPE_ID)
                                               where listOfficialContractId.Contains(hct.ID)
                                               orderby item.START_DATE ascending
                                               select item.START_DATE).FirstOrDefault();

                var query = await (from p in _dbContext.HuEmployees
                                   from e in _dbContext.HuEmployeeCvs.Where(x => x.ID == p.PROFILE_ID).DefaultIfEmpty()
                                   from o in _dbContext.HuOrganizations.Where(x => x.ID == p.ORG_ID).DefaultIfEmpty()
                                   from pos in _dbContext.HuPositions.Where(x => x.ID == p.POSITION_ID).DefaultIfEmpty()
                                   from di in _dbContext.HuPositions.Where(x => x.ID == pos.LM).DefaultIfEmpty()
                                   from p2 in _dbContext.HuEmployees.Where(x => x.POSITION_ID == di.ID && x.ORG_ID == di.ORG_ID && x.ID == di.MASTER).DefaultIfEmpty()
                                   from ep2 in _dbContext.HuEmployeeCvs.Where(x => x.ID == p2.PROFILE_ID).DefaultIfEmpty()
                                   from pos2 in _dbContext.HuPositions.Where(x => x.ID == p2.POSITION_ID).DefaultIfEmpty()
                                   from st in _dbContext.HuJobs.Where(x => x.ID == pos2.JOB_ID).DefaultIfEmpty()
                                   from com in _dbContext.HuCompanys.Where(x => x.ID == o.COMPANY_ID).DefaultIfEmpty()
                                   from ob in _dbContext.SysOtherLists.Where(x => x.ID == e.EMPLOYEE_OBJECT_ID).DefaultIfEmpty()
                                   where p.ID == id
                                   select new
                                   {
                                       Id = id,
                                       DirectManager = di != null ? ep2.FULL_NAME : "",
                                       EmployeeCode = p.CODE ?? "",
                                       PositionDirectManager = di != null ? st.NAME_VN : "",
                                       Company = com.NAME_VN ?? "",
                                       EmployeeObjectId = ob.ID,
                                       Position = pos.NAME ?? "",
                                       PositionId = p.POSITION_ID,
                                       ObjectEmployee = ob.NAME ?? "",
                                       ObjectEmployeeId = ob.ID,
                                       WorkingAddress = com.WORK_ADDRESS ?? "",
                                       OrgName = o.NAME ?? "",
                                       OrgId = pos.ORG_ID,
                                       ItimeId = p.ITIME_ID,
                                       OtherName = e.OTHER_NAME,
                                       IsNotContract = p.IS_NOT_CONTRACT,
                                       IsNotContractStr = p.IS_NOT_CONTRACT == true ? "Có" : "Không",
                                       DateJoinCompany = getDateJoinCompany,
                                       DateOfficialContract = getDateOfficialContract //code cu lay ngay vao chinh thuc o hd
                                       //DateOfficialContract = p.JOIN_DATE_STATE == null ? getDateOfficialContract : p.JOIN_DATE_STATE
                                   }).SingleAsync();
                return new() { InnerBody = query };
            }
            catch
            {
                return new FormatedResponse() { MessageCode = CommonMessageCode.GET_METHOD_FAILED, ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400 };
            }
        }
        public async Task<FormatedResponse> CheckSameName(string name)
        {
            try
            {
                var query = await (from p in _dbContext.HuEmployees
                                   from e in _dbContext.HuEmployeeCvs.Where(x => x.ID == p.PROFILE_ID).DefaultIfEmpty()
                                   where e.FULL_NAME == name
                                   select new
                                   {
                                       Code = p.CODE
                                   }).ToListAsync();
                List<string> result = new List<string>();
                if (query != null)
                {
                    foreach (var item in query)
                    {
                        result.Add(item.Code);
                    }
                }
                string value = String.Join("\n", result);
                return new() { InnerBody = value, StatusCode = EnumStatusCode.StatusCode200 };
            }
            catch (Exception ex)
            {
                return new() { InnerBody = null, StatusCode = EnumStatusCode.StatusCode400 };
            }
        }
        public async Task<FormatedResponse> CheckSameIdNo(string idNo)
        {
            try
            {
                var query = await (from p in _dbContext.HuEmployees
                                   from e in _dbContext.HuEmployeeCvs.Where(x => x.ID == p.PROFILE_ID).DefaultIfEmpty()
                                   where e.ID_NO == idNo
                                   select new
                                   {
                                       Code = p.CODE
                                   }).ToListAsync();
                List<string> result = new List<string>();
                if (query != null)
                {
                    foreach (var item in query)
                    {
                        result.Add(item.Code);
                    }
                }
                string value = String.Join("\n", result);
                return new() { InnerBody = value, StatusCode = EnumStatusCode.StatusCode200 };
            }
            catch (Exception ex)
            {
                return new() { InnerBody = null, StatusCode = EnumStatusCode.StatusCode400 };
            }
        }

        public async Task<FormatedResponse> CheckSameItimeid(string itimeId)
        {
            try
            {
                var query = await (from p in _dbContext.HuEmployees
                                       //from e in _dbContext.HuEmployeeCvs.Where(x => x.ID == p.PROFILE_ID).DefaultIfEmpty()
                                   where p.ITIME_ID == itimeId
                                   select new
                                   {
                                       Code = p.CODE
                                   }).ToListAsync();
                List<string> result = new List<string>();
                if (query != null)
                {
                    foreach (var item in query)
                    {
                        result.Add(item.Code);
                    }
                }
                string value = String.Join("\n", result);
                return new() { InnerBody = value, StatusCode = EnumStatusCode.StatusCode200 };
            }
            catch (Exception ex)
            {
                return new() { InnerBody = null, StatusCode = EnumStatusCode.StatusCode400 };
            }
        }

        public async Task<FormatedResponse> CheckSameTaxCode(string taxCode)
        {
            try
            {
                var query = await (from p in _dbContext.HuEmployees
                                   from e in _dbContext.HuEmployeeCvs.Where(x => x.ID == p.PROFILE_ID).DefaultIfEmpty()
                                   where e.TAX_CODE == taxCode
                                   select new
                                   {
                                       Code = p.CODE
                                   }).ToListAsync();
                List<string> result = new List<string>();
                if (query != null)
                {
                    foreach (var item in query)
                    {
                        result.Add(item.Code);
                    }
                }
                string value = String.Join("\n", result);
                return new() { InnerBody = value, StatusCode = EnumStatusCode.StatusCode200 };
            }
            catch (Exception ex)
            {
                return new() { InnerBody = null, StatusCode = EnumStatusCode.StatusCode400 };
            }
        }

        public async Task<FormatedResponse> UpdateGeneralInfo(StaffProfileUpdateDTO request)
        {
            try
            {
                var query = await (from p in _dbContext.HuEmployees
                                       //from e in _dbContext.HuEmployeeCvs.Where(x => x.ID == p.PROFILE_ID).DefaultIfEmpty()
                                   where p.ITIME_ID == request.ItimeId
                                   select new
                                   {
                                       Code = p.CODE
                                   }).ToListAsync();
                List<string> result = new List<string>();
                if (query != null)
                {
                    foreach (var item in query)
                    {
                        result.Add(item.Code);
                    }
                }
                string value = String.Join("\n", result);
                if(value == "")
                {
                    var entity = _dbContext.HuEmployees.Where(x => x.ID == request.Id).SingleOrDefault();

                    var entity_cv = _dbContext.HuEmployeeCvs.Where(x => x.ID == entity.PROFILE_ID).SingleOrDefault();

                    if (entity != null)
                    {
                        entity.ORG_ID = request.OrgId;
                        entity.POSITION_ID = request.PositionId;
                        entity.EMPLOYEE_OBJECT_ID = request.ObjectEmployeeId;
                        entity.ITIME_ID = request.ItimeId == "" ? null : request.ItimeId;

                        entity_cv.EMPLOYEE_OBJECT_ID = request.ObjectEmployeeId;

                        _dbContext.SaveChanges();
                        return new FormatedResponse() { MessageCode = CommonMessageCode.UPDATE_SUCCESS, InnerBody = true, StatusCode = EnumStatusCode.StatusCode200 };
                    }
                    else
                    {
                        return new FormatedResponse() { MessageCode = CommonMessageCode.ENTITIES_NOT_FOUND, ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400 };
                    }
                }
                else
                {
                    return new FormatedResponse() { MessageCode = CommonMessageCodes.ITEMID_HAS_EXISTS, ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400 };

                }

            }
            catch (Exception ex)
            {
                return new FormatedResponse() { MessageCode = CommonMessageCode.POST_UPDATE_FAILLED, ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400 };
            }
        }

        public async Task<FormatedResponse> GetEmployeeStatusList()
        {
            var list = await (from o in _dbContext.SysOtherLists.AsNoTracking()
                              from t in _dbContext.SysOtherListTypes.AsNoTracking().Where(t => o.TYPE_ID == t.ID).DefaultIfEmpty()
                              where t.CODE == "EMP_STATUS"
                              orderby o.ORDERS
                              select new
                              {
                                  Id = o.ID,
                                  Name = o.NAME ?? string.Empty,
                                  Code = o.CODE,
                                  TypeCode = t.CODE
                              }).ToListAsync();
            return new() { InnerBody = list };

        }
        public async Task<FormatedResponse> InsertStaffProfile(StaffProfileEditDTO request, string sid)
        {
            _uow.CreateTransaction();
            HuEmployeeCvDTO employeeCvDto = new HuEmployeeCvDTO();
            HU_EMPLOYEE_CV employeeCvEntity = new HU_EMPLOYEE_CV();
            HuEmployeeDTO employeeDto = new HuEmployeeDTO();
            HU_EMPLOYEE employeeEntity = new HU_EMPLOYEE();
            if (request.BirthDay.HasValue && ((DateTime.Now.Year - request.BirthDay.Value.Year) < 15))
            {
                return new FormatedResponse() { MessageCode = CommonMessageCode.BIRTH_DAY_NOT_BIGGER_THAN_DATE_TIME_NOW, ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400 };
            }
            try
            {
                if (request.AvatarFileData != null && request.AvatarFileData.Length > 0 && request.AvatarFileName != null && request.AvatarFileType != null)
                {
                    string location = Path.Combine(_env.ContentRootPath, _appSettings.StaticFolders.Root, _appSettings.StaticFolders.Avatars);

                    UploadRequest uploadRequest = new() { ClientFileData = request.AvatarFileData, ClientFileName = request.AvatarFileName, ClientFileType = request.AvatarFileType };

                    var uploadResponse = await _fileService.UploadFile(uploadRequest, location, sid);

                    if (uploadResponse != null)
                    {
                        string avatar = uploadResponse.SavedAs;
                        employeeCvEntity.AVATAR = avatar;

                    }
                }
                employeeCvEntity.CREATED_DATE = DateTime.Now;
                employeeCvEntity.CREATED_BY = sid;
                employeeCvEntity.OTHER_NAME = request.OtherName ?? "";
                employeeCvEntity.FULL_NAME = request.Name ?? "";
                //employeeCvEntity.FIRST_NAME = request.Name ?? "";
                //employeeCvEntity.LAST_NAME = request.Name ?? "";
                employeeCvEntity.BIRTH_DATE = request.BirthDay;
                employeeCvEntity.BIRTH_PLACE = request.BirthAddress;
                employeeCvEntity.GENDER_ID = request.GenderId != null ? request.GenderId : null;
                employeeCvEntity.TAX_CODE = request.TaxCode;
                employeeCvEntity.TAX_CODE_DATE = request.TaxCodeDate;
                employeeCvEntity.TAX_CODE_ADDRESS = request.TaxCodeDateAddress;
                employeeCvEntity.VISA_EXPIRE = request.VisaDateExperiod;
                employeeCvEntity.VISA_NO = request.Visa;
                employeeCvEntity.PASS_EXPIRE = request.PassportDateExperiod;
                employeeCvEntity.DOMICILE = request.Domicile;

                employeeCvEntity.ID_NO = request.IdentityNumber;
                employeeCvEntity.ID_DATE = request.IdentityNumberDate;
                employeeCvEntity.ID_PLACE = request.IdentityNumberAddress;
                employeeCvEntity.RELIGION_ID = request.ReligionId != null ? request.ReligionId : null;
                employeeCvEntity.MARITAL_STATUS_ID = request.MaritalStatusId != null ? request.MaritalStatusId : null;
                employeeCvEntity.PASS_DATE = request.PassportDate;
                employeeCvEntity.VISA_PLACE = request.VisaAddress;
                employeeCvEntity.NATIVE_ID = request.NationId != null ? request.NationId : null;
                employeeCvEntity.NATIONALITY_ID = request.NationalityId != null ? request.NationalityId : null;
                employeeCvEntity.QUALIFICATIONID = request.QualificationId != null ? request.QualificationId : null;
                employeeCvEntity.QUALIFICATIONID_2 = request.QualificationId2 != null ? request.QualificationId2 : null;
                employeeCvEntity.QUALIFICATIONID_3 = request.QualificationId3 != null ? request.QualificationId3 : null;
                employeeCvEntity.TRAINING_FORM_ID = request.TrainingFormId != null ? request.TrainingFormId : null;
                employeeCvEntity.TRAINING_FORM_ID_2 = request.TrainingFormId2 != null ? request.TrainingFormId2 : null;
                employeeCvEntity.TRAINING_FORM_ID_3 = request.TrainingFormId3 != null ? request.TrainingFormId3 : null;
                employeeCvEntity.SCHOOL_ID = request.SchoolId != null ? request.SchoolId : null;
                employeeCvEntity.SCHOOL_ID_2 = request.SchoolId2 != null ? request.SchoolId2 : null;
                employeeCvEntity.SCHOOL_ID_3 = request.SchoolId3 != null ? request.SchoolId3 : null;
                employeeCvEntity.LANGUAGE_ID = request.LanguageId != null ? request.LanguageId : null;
                employeeCvEntity.LANGUAGE_ID_2 = request.LanguageId2 != null ? request.LanguageId2 : null;
                employeeCvEntity.LANGUAGE_ID_3 = request.LanguageId3 != null ? request.LanguageId3 : null;
                employeeCvEntity.LANGUAGE_LEVEL_ID = request.LanguageLevelId != null ? request.LanguageLevelId : null;
                employeeCvEntity.LANGUAGE_LEVEL_ID_2 = request.LanguageLevelId2 != null ? request.LanguageLevelId2 : null;
                employeeCvEntity.LANGUAGE_LEVEL_ID_3 = request.LanguageLevelId3 != null ? request.LanguageLevelId3 : null;
                employeeCvEntity.PRESENTER = request.Presenter != null ? request.Presenter : null;
                employeeCvEntity.PRESENTER_ADDRESS = request.PresenterAddress != null ? request.PresenterAddress : null;
                employeeCvEntity.PRESENTER_PHONE_NUMBER = request.PresenterPhoneNumber != null ? request.PresenterPhoneNumber : null;
                employeeCvEntity.IS_HOST = request.IsHost != null ? request.IsHost : null;
                employeeCvEntity.HOUSEHOLD_NUMBER = request.HouseholdNumber != null ? request.HouseholdNumber : null;
                employeeCvEntity.HOUSEHOLD_CODE = request.HouseholdCode != null ? request.HouseholdCode : null;
                employeeCvEntity.ADDRESS = request.Address != null ? request.Address : null;
                employeeCvEntity.PROVINCE_ID = request.ProvinceId != null ? request.ProvinceId : null;
                employeeCvEntity.CUR_PROVINCE_ID = request.CurProvinceId != null ? request.CurProvinceId : null;
                employeeCvEntity.CUR_ADDRESS = request.CurAddress != null ? request.CurAddress : null;
                employeeCvEntity.WARD_ID = request.WardId != null ? request.WardId : null;
                employeeCvEntity.CUR_WARD_ID = request.CurWardId != null ? request.CurWardId : null;
                employeeCvEntity.DISTRICT_ID = request.DistrictId != null ? request.DistrictId : null;
                employeeCvEntity.CUR_DISTRICT_ID = request.CurDistrictId != null ? request.CurDistrictId : null;
                employeeCvEntity.MOBILE_PHONE = request.Telephone != null ? request.Telephone : null;
                employeeCvEntity.MOBILE_PHONE_LAND = request.LandlinePhone != null ? request.LandlinePhone : null;
                employeeCvEntity.WORK_EMAIL = request.EmailCompany != null ? request.EmailCompany : null;
                employeeCvEntity.EMAIL = request.EmailPersonal != null ? request.EmailPersonal : null;
                employeeCvEntity.BANK_ID = request.BankId != null ? request.BankId : null;
                employeeCvEntity.BANK_ID_2 = request.BankId2 != null ? request.BankId2 : null;
                employeeCvEntity.BANK_BRANCH = request.BankBranchId != null ? request.BankBranchId : null;
                employeeCvEntity.BANK_BRANCH_2 = request.BankBranchId2 != null ? request.BankBranchId2 : null;
                employeeCvEntity.BANK_NO = request.BankNo != null ? request.BankNo : null;
                employeeCvEntity.MAIN_INCOME = request.MainIncome != null ? request.MainIncome : null;
                employeeCvEntity.OTHER_SOURCES = request.OtherSources != null ? request.OtherSources : null;
                employeeCvEntity.LAND_GRANTED = request.LandGranted != null ? request.LandGranted : null;
                employeeCvEntity.TAX_GRANTED_HOUSE = request.TaxtGrantedHouse != null ? request.TaxtGrantedHouse : null;
                employeeCvEntity.TOTAL_AREA = request.TotalArea != null ? request.TotalArea : null;
                employeeCvEntity.SELF_PURCHASE_LAND = request.SelfPurchasedLand != null ? request.SelfPurchasedLand : null;
                employeeCvEntity.SELF_BUILD_HOUSE = request.SelfBuildHouse != null ? request.SelfBuildHouse : null;
                employeeCvEntity.TOTAL_APP_AREA = request.TotalAppArea != null ? request.TotalAppArea : null;
                employeeCvEntity.LAND_FOR_PRODUCTION = request.LandForProduction != null ? request.LandForProduction : null;
                employeeCvEntity.ADDITIONAL_INFOMATION = request.AdditionalInformation != null ? request.AdditionalInformation : null;
                employeeCvEntity.HEART = request.Heart != null ? request.Heart : null;
                employeeCvEntity.PASS_NO = request.Passport != null ? request.Passport : null;
                employeeCvEntity.PASS_PLACE = request.PassportAddress != null ? request.PassportAddress : null;
                employeeCvEntity.VISA_DATE = request.VisaDate != null ? request.VisaDate : null;
                employeeCvEntity.WORK_NO = request.LaborBookNumber != null ? request.LaborBookNumber : null;
                employeeCvEntity.WORK_PERMIT_DATE = request.LaborBookDate != null ? request.LaborBookDate : null;
                employeeCvEntity.WORK_PERMIT_PLACE = request.LaborBookAddress != null ? request.LaborBookAddress : null;
                employeeCvEntity.WORK_PERMIT_EXPIRE = request.LaborBookDateExperiod != null ? request.LaborBookDateExperiod : null;
                employeeCvEntity.CARRER = request.Career != null ? request.Career : null;
                employeeCvEntity.INSURENCE_NUMBER = request.InsurenceNumber != null ? request.InsurenceNumber : null;
                employeeCvEntity.INS_CARD_NUMBER = request.InsurenceCardNumber != null ? request.InsurenceCardNumber : null;
                employeeCvEntity.FAMILY_MEMBER = request.FamilyMember != null ? request.FamilyMember : null;
                employeeCvEntity.FAMILY_POLICY = request.FamilyMatters != null ? request.FamilyMatters : null;
                employeeCvEntity.VETERANS = request.Veterans != null ? request.Veterans : null;
                employeeCvEntity.CARRER_BEFORE_RECUITMENT = request.CareerBeforeRecruitment != null ? request.CareerBeforeRecruitment : null;
                employeeCvEntity.TITLE_CONFERRED = request.TitleConferred != null ? request.TitleConferred : null;
                employeeCvEntity.POLITICAL_THEORY = request.PoliticalTheoryLevel != null ? request.PoliticalTheoryLevel : null;
                employeeCvEntity.SCHOOL_OF_WORK = request.ForteWork != null ? request.ForteWork : null;
                employeeCvEntity.LEFT_EYE = request.LeftEye != null ? request.LeftEye : null;
                employeeCvEntity.RIGHT_EYE = request.RightEye != null ? request.RightEye : null;
                employeeCvEntity.EXAMINATION_DATE = request.DateMedicalExam != null ? request.DateMedicalExam : null;
                employeeCvEntity.HEALTH_TYPE = request.HealthType != null ? request.HealthType : null;
                employeeCvEntity.HEALTH_NOTES = request.HealthNotes != null ? request.HealthNotes : null;
                employeeCvEntity.BLOOD_PRESSURE = request.BloodPressure != null ? request.BloodPressure : null;
                employeeCvEntity.BLOOD_GROUP = request.BloodGroup != null ? request.BloodGroup : null;
                employeeCvEntity.HEIGHT = request.Height != null ? request.Height : null;
                employeeCvEntity.WEIGHT = request.Weight != null ? request.Weight : null;
                employeeCvEntity.BIRTH_REGIS_ADDRESS = request.BirthRegisAddress != null ? request.BirthRegisAddress : null;
                employeeCvEntity.PRISON_NOTE = request.NotePrison != null ? request.NotePrison : null;
                employeeCvEntity.RELATIONS = request.Relations != null ? request.Relations : null;
                employeeCvEntity.FAMILY_DETAIL = request.Relatives != null ? request.Relatives : null;
                employeeCvEntity.YELLOW_FLAG = request.YellowFlag != null ? request.YellowFlag : null;
                employeeCvEntity.YOUTH_GROUP_DATE = request.YouthGroupDate != null ? request.YouthGroupDate : null;
                employeeCvEntity.YOUTH_GROUP_ADDRESS = request.YouthGroupAddress != null ? request.YouthGroupAddress : null;
                employeeCvEntity.YOUTH_GROUP_POSITION = request.YouthGroupPosition != null ? request.YouthGroupPosition : null;
                employeeCvEntity.IS_JOIN_YOUTH_GROUP = request.IsJoinYouthGroup != null ? request.IsJoinYouthGroup : null;
                employeeCvEntity.IS_UNIONIST = request.IsUnionist != null ? request.IsUnionist : null;
                employeeCvEntity.UNIONIST_POSITION = request.UnionistPosition != null ? request.UnionistPosition : null;
                employeeCvEntity.UNIONIST_DATE = request.UnionistDate != null ? request.UnionistDate : null;
                employeeCvEntity.UNIONIST_ADDRESS = request.UnionistAddress != null ? request.UnionistAddress : null;
                employeeCvEntity.YOUTH_SAVE_NATION_DATE = request.YouthSaveNationDate != null ? request.YouthSaveNationDate : null;
                employeeCvEntity.YOUTH_SAVE_NATION_POSITION = request.YouthSaveNationPosition != null ? request.YouthSaveNationPosition : null;
                employeeCvEntity.YOUTH_SAVE_NATION_ADDRESS = request.YouthSaveNationAddress != null ? request.YouthSaveNationAddress : null;
                employeeCvEntity.IS_MEMBER = request.IsMember != null ? request.IsMember : null;
                employeeCvEntity.MEMBER_POSITION = request.MemberPosition != null ? request.MemberPosition : null;
                employeeCvEntity.MEMBER_DATE = request.MemberDate != null ? request.MemberDate : null;
                employeeCvEntity.MEMBER_ADDRESS = request.MemberAddress != null ? request.MemberAddress : null;
                employeeCvEntity.LIVING_CELL = request.LivingCell != null ? request.LivingCell : null;
                employeeCvEntity.CARD_NUMBER = request.CardNumber != null ? request.CardNumber : null;
                employeeCvEntity.POLITICAL_THEORY_LEVEL = request.PoliticalTheoryLevel != null ? request.PoliticalTheoryLevel : null;
                employeeCvEntity.RESUME_NUMBER = request.ResumeNumber != null ? request.ResumeNumber : null;
                employeeCvEntity.VATERANS_MEMBER_DATE = request.VateransMemberDate != null ? request.VateransMemberDate : null;
                employeeCvEntity.MEMBER_OFFICAL_DATE = request.MemberOfficalDate != null ? request.MemberOfficalDate : null;
                employeeCvEntity.VATERANS_POSITION = request.VateransPosition != null ? request.VateransPosition : null;
                employeeCvEntity.VATERANS_ADDRESS = request.VateransAddress != null ? request.VateransAddress : null;
                employeeCvEntity.ENLISTMENT_DATE = request.EnlistmentDate != null ? request.EnlistmentDate : null;
                employeeCvEntity.DISCHARGE_DATE = request.DischargeDate != null ? request.DischargeDate : null;
                employeeCvEntity.HIGHEST_MILITARY_POSITION = request.HighestMilitaryPosition != null ? request.HighestMilitaryPosition : null;
                employeeCvEntity.CURRENT_PARTY_COMMITTEE = request.CurrentPartyCommittee != null ? request.CurrentPartyCommittee : null;
                employeeCvEntity.PARTYTIME_PARTY_COMMITTEE = request.PartytimePartyCommittee != null ? request.PartytimePartyCommittee : null;
                employeeCvEntity.EDUCATION_LEVEL_ID = request.EducationLevelId != null ? request.EducationLevelId : null;
                employeeCvEntity.LEARNING_LEVEL_ID = request.LearningLevelId != null ? request.LearningLevelId : null;
                employeeCvEntity.COMPUTER_SKILL = request.ComputerSkill != null ? request.ComputerSkill : null;
                employeeCvEntity.COMPUTER_SKILL_ID = request.ComputerSkillId != null ? request.ComputerSkillId : null;
                employeeCvEntity.LICENSE = request.License != null ? request.License : null;
                employeeCvEntity.LICENSE_ID = request.LicenseId != null ? request.LicenseId : null;
                employeeCvEntity.CUR_ADDRESS = request.CurAddress != null ? request.CurAddress : null;
                employeeCvEntity.MOBILE_PHONE = request.Telephone != null ? request.Telephone : null;
                employeeCvEntity.MOBILE_PHONE_LAND = request.LandlinePhone != null ? request.LandlinePhone : null;
                employeeCvEntity.WORK_EMAIL = request.EmailCompany != null ? request.EmailCompany : null;
                employeeCvEntity.EMAIL = request.EmailPersonal != null ? request.EmailPersonal : null;
                employeeCvEntity.BANK_ID_2 = request.BankId2 != null ? request.BankId2 : null;
                employeeCvEntity.BANK_BRANCH = request.BankBranchId != null ? request.BankBranchId : null;
                employeeCvEntity.BANK_BRANCH_2 = request.BankBranchId2 != null ? request.BankBranchId2 : null;
                employeeCvEntity.TAX_GRANTED_HOUSE = request.TaxGrantedHouse != null ? request.TaxGrantedHouse : null;
                employeeCvEntity.SELF_PURCHASE_LAND = request.SelfPurchaseLand != null ? request.SelfPurchaseLand : null;
                employeeCvEntity.EMPLOYEE_OBJECT_ID = request.EmployeeObjectId != null ? request.EmployeeObjectId : null;
                employeeCvEntity.NATIONALITY_ID = request.NationalityId != null ? request.NationalityId : null;
                employeeCvEntity.HEALTH_CARE_ADDRESS = request.MedicalExamPlace != null ? request.MedicalExamPlace : null;
                employeeCvEntity.BANK_NO_2 = request.BankNo2 != null ? request.BankNo2 : null;
                employeeCvEntity.DEFENSE_SECURITY_TRAINING = request.DefenseSecurityTraining != null ? request.DefenseSecurityTraining : null;

                employeeEntity.ITIME_ID = request.ItimeId != null ? request.ItimeId : null;
                employeeEntity.IS_NOT_CONTRACT = request.IsNotContract != null ? request.IsNotContract : null;
                employeeEntity.CREATED_DATE = DateTime.Now;
                employeeEntity.CREATED_BY = sid;
                var statusOff = _dbContext.SysOtherLists.Where(x => x.CODE == "ESQ").FirstOrDefault();
                var statusOn = _dbContext.SysOtherLists.Where(x => x.CODE == "ESW").FirstOrDefault();
                var statusDetail = _dbContext.SysOtherLists.FirstOrDefault(x => x.CODE == "00068");
                if (employeeEntity.IS_NOT_CONTRACT == true)
                {
                    employeeEntity.WORK_STATUS_ID = statusOn?.ID;
                    employeeEntity.STATUS_DETAIL_ID = statusDetail?.ID;
                }
                //if (employeeEntity.IS_NOT_CONTRACT == false)
                //{
                //    employeeEntity.WORK_STATUS_ID = statusOn?.ID;
                //}
                employeeEntity.POSITION_ID = request.TitlePosition;
                employeeEntity.ORG_ID = request.OrgId;
                //employeeEntity.CODE = request.Code ?? "";

                if (request.ComCode != null) //tạo code lúc insert
                {
                    decimal num;
                    var queryCode = await (from x in _dbContext.HuEmployees
                                           where x.CODE.Length - request.ComCode.Length == 4
                                           select x.CODE).ToListAsync();
                    queryCode = queryCode.Where(x => CheckForSpecialCharacters(x) == false && x.AsEnumerable().All(c => Char.IsDigit(c)) == true).ToList();
                    var existingCode = (from p in queryCode where Decimal.TryParse(p.Substring(request.ComCode.Length), out num) orderby p descending select p).ToList();
                    string newcode = StringCodeGenerator.CreateNewCode(request.ComCode, 4, existingCode);
                    employeeEntity.CODE = newcode;
                    employeeEntity.PROFILE_CODE = newcode;
                }

                employeeEntity.INSURENCE_AREA_ID = request.InsurenceAreaId;
                employeeEntity.DIRECT_MANAGER_ID = request.DirectManagementId != null ? request.DirectManagementId : null;
                var employeeCvMapped = CoreMapper<HuEmployeeCvDTO, HU_EMPLOYEE_CV>.DtoToEntity(employeeCvDto, employeeCvEntity);

                if (employeeCvMapped != null)
                {
                    await _dbContext.HuEmployeeCvs.AddAsync(employeeCvMapped);
                    var result = await _dbContext.SaveChangesAsync();
                    if (result > 0)
                    {
                        employeeEntity.PROFILE_ID = employeeCvMapped.ID;

                        var employeeMapped = CoreMapper<HuEmployeeDTO, HU_EMPLOYEE>.DtoToEntity(employeeDto, employeeEntity);
                        if (employeeMapped != null)
                        {
                            await _dbContext.HuEmployees.AddAsync(employeeMapped);
                            var result2 = await _dbContext.SaveChangesAsync();

                            if (result2 > 0)
                            {
                                var groupUser = _dbContext.SysGroups.Where(x => x.CODE == "ESS_USER").FirstOrDefault();
                                SysUserDTO entityCreateRequest = new()
                                {
                                    Id = Guid.NewGuid().ToString(),
                                    Username = employeeMapped.CODE,
                                    GroupId = groupUser?.ID,
                                    Passwordhash = BCrypt.Net.BCrypt.HashPassword(employeeCvEntity.ID_NO),
                                    IsPortal = true,
                                    IsMobile = true,
                                    IsWebapp = false,
                                    EmployeeCode = employeeMapped.CODE,
                                    EmployeeId = employeeMapped.ID,
                                    Fullname = employeeCvEntity.FULL_NAME,
                                    Avatar = employeeCvEntity.AVATAR,
                                    IsFirstLogin = true,
                                    CreatedBy = sid
                                };
                                await _genericRepositoryUser.Create(_uow, entityCreateRequest, sid);
                            }
                            if (request.TitlePosition != null)
                            {
                                var master = _dbContext.HuPositions.Where(x => x.ID == request.TitlePosition).SingleOrDefault();
                                if (master != null)
                                {
                                    master.INTERIM = master.MASTER;
                                    master.MASTER = employeeEntity.ID;
                                    await _dbContext.SaveChangesAsync();
                                    _uow.Commit();
                                    return new FormatedResponse() { MessageCode = "Bạn đã tạo mới thành công nhân viên mới: Mã nhân viên " + employeeEntity.CODE + " và mã hồ sơ " + employeeEntity.PROFILE_CODE, ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode200 };

                                }
                                else
                                {
                                    _uow.Rollback();
                                    return new FormatedResponse() { MessageCode = CommonMessageCode.POST_INSERT_FAILLED, ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400 };
                                }
                            }
                            _uow.Rollback();
                            return new FormatedResponse() { MessageCode = CommonMessageCode.POST_INSERT_FAILLED, ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400 };


                        }
                        else
                        {
                            _uow.Rollback();
                            return new FormatedResponse() { MessageCode = CommonMessageCode.POST_INSERT_FAILLED, ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400 };
                        }
                    }
                    else
                    {
                        _uow.Rollback();
                        return new FormatedResponse() { MessageCode = CommonMessageCode.POST_INSERT_FAILLED, ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400 };
                    }
                }
                else
                {
                    _uow.Rollback();
                    return new FormatedResponse() { MessageCode = CommonMessageCode.POST_INSERT_FAILLED, ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400 };
                }

            }
            catch (Exception r)
            {
                _uow.Rollback();
                return new FormatedResponse() { MessageCode = CommonMessageCode.POST_INSERT_FAILLED, ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400 };
            }
        }

        public async Task<FormatedResponse> CheckPositionMasterInterim(long? id)
        {
            try
            {
                var position = _dbContext.HuPositions.Where(x => x.ID == id).SingleOrDefault();

                if (position != null)
                {
                    //var working = _dbContext.HuWorkings.Where(x => x.EMPLOYEE_ID == employeeEntity.ID && ).OrderByDescending().
                    var working = await (
                                         from o in _dbContext.HuWorkings.Where(x => x.EMPLOYEE_ID == position.MASTER)
                                         from t in _dbContext.SysOtherLists.AsNoTracking().Where(t => t.ID == o.TYPE_ID).DefaultIfEmpty()
                                         from t2 in _dbContext.SysOtherLists.AsNoTracking().Where(t => t.ID == o.STATUS_ID).DefaultIfEmpty()
                                         where (t.CODE == "DC" || t.CODE == "BN" || t.CODE == "DDBN" || t.CODE == "CT" || t.CODE == "BP" || t.CODE == "DDCV") && t2.CODE == "DD" //Điều chuyển, bổ nhiệm
                                         && o.EFFECT_DATE > DateTime.Now
                                         orderby o.CREATED_DATE descending
                                         select new
                                         {
                                             Id = o.ID,
                                             //Name = o.NAME ?? string.Empty
                                         }).ToListAsync();
                    var terminate = await (
                                        from t in _dbContext.HuTerminates.Where(x => x.EMPLOYEE_ID == position.MASTER)
                                        from t2 in _dbContext.SysOtherLists.AsNoTracking().Where(x => x.ID == t.STATUS_ID).DefaultIfEmpty()
                                        where t2.CODE == "DD" && t.EFFECT_DATE > DateTime.Now
                                        orderby t.CREATED_DATE descending
                                        select new
                                        {
                                            Id = t.ID,
                                        }).ToListAsync();

                    if (working.Any() || terminate.Any())
                    {
                        return new FormatedResponse() { InnerBody = position, ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode200 };
                    }
                }
                return new FormatedResponse() { InnerBody = null, ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode200 };
            }
            catch (Exception ex)
            {
                return new FormatedResponse() { MessageCode = CommonMessageCode.GET_METHOD_FAILED, ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400 };
            }
        }

        public async Task<FormatedResponse> UpdateAvatar(StaffProfileEditDTO request, string sid)
        {
            try
            {
                var entity = _dbContext.HuEmployeeCvs.Where(x => x.ID == request.Id).SingleOrDefault();
                if (entity != null)
                {
                    if (string.IsNullOrEmpty(entity.AVATAR)) //create new avatar
                    {
                        if (request.AvatarFileData != null && request.AvatarFileData.Length > 0 && request.AvatarFileName != null && request.AvatarFileType != null)
                        {
                            string location = Path.Combine(_env.ContentRootPath, _appSettings.StaticFolders.Root, _appSettings.StaticFolders.Avatars);

                            UploadRequest uploadRequest = new() { ClientFileData = request.AvatarFileData, ClientFileName = request.AvatarFileName, ClientFileType = request.AvatarFileType };

                            var uploadResponse = await _fileService.UploadFile(uploadRequest, location, sid);

                            if (uploadResponse != null)
                            {
                                string avatar = uploadResponse.SavedAs;
                                entity.AVATAR = avatar;
                                await _dbContext.SaveChangesAsync();
                            }

                            return new FormatedResponse() { InnerBody = entity.AVATAR, StatusCode = EnumStatusCode.StatusCode200, MessageCode = CommonMessageCode.INSERT_AVATAR_SUCCESS };

                        }
                    }
                    else //update avatar
                    {
                        var isDeleted = DeleteFileAvatar(entity.AVATAR);
                        if (isDeleted)
                        {
                            if (request.AvatarFileData != null && request.AvatarFileData.Length > 0 && request.AvatarFileName != null && request.AvatarFileType != null)
                            {
                                string location = Path.Combine(_env.ContentRootPath, _appSettings.StaticFolders.Root, _appSettings.StaticFolders.Avatars);

                                UploadRequest uploadRequest = new() { ClientFileData = request.AvatarFileData, ClientFileName = request.AvatarFileName, ClientFileType = request.AvatarFileType };

                                var uploadResponse = await _fileService.UploadFile(uploadRequest, location, sid);

                                if (uploadResponse != null)
                                {
                                    string avatar = uploadResponse.SavedAs;
                                    entity.AVATAR = avatar;
                                    await _dbContext.SaveChangesAsync();
                                    return new FormatedResponse() { InnerBody = avatar, StatusCode = EnumStatusCode.StatusCode200, MessageCode = CommonMessageCode.UPDATE_AVATAR_SUCCESS };
                                }

                            }
                        }
                        else
                        {
                            return new FormatedResponse() { MessageCode = CommonMessageCode.UPDATE_AVATAR_FAIL, ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400 };

                        }

                    }
                }

                return new FormatedResponse() { MessageCode = CommonMessageCode.UPDATE_AVATAR_FAIL, ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400 };
            }
            catch (Exception ex)
            {
                return new FormatedResponse() { MessageCode = CommonMessageCode.POST_UPDATE_FAILLED, ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400 };
            }
        }

        public bool DeleteFileAvatar(string fileName)
        {
            try
            {
                string location = Path.Combine(_env.ContentRootPath, _appSettings.StaticFolders.Root, _appSettings.StaticFolders.Avatars);

                if (!Directory.Exists(location))
                {
                    Directory.CreateDirectory(location);
                }
                string filePath = Path.Combine(location, fileName);
                FileInfo file = new FileInfo(filePath);
                if (file.Exists)
                {
                    file.Delete();
                }
                return true;
            }
            catch (Exception)
            {
                return false;
                throw;
            }
        }

        public async Task<string> GetCode(string code)
        {
            try
            {
                decimal num;
                var queryCode = await (from x in _dbContext.HuEmployees
                                       where x.CODE.Length - code.Length == 4
                                       select x.CODE).ToListAsync();
                queryCode = queryCode.Where(x => CheckForSpecialCharacters(x) == false && x.AsEnumerable().All(c => Char.IsDigit(c)) == true).ToList();
                var existingCode = (from p in queryCode where Decimal.TryParse(p.Substring(code.Length), out num) orderby p descending select p).ToList();
                string newcode = StringCodeGenerator.CreateNewCode(code, 4, existingCode);
                return newcode;
            }
            catch (Exception ex)
            {
                return null;
            }

        }

        public virtual async Task<FormatedResponse> ToggleActiveIds(GenericUnitOfWork _uow, List<long> ids, bool valueToBind, string sid)
        {
            await Task.Run(() => null);
            throw new NotImplementedException();
        }

        public static bool CheckForSpecialCharacters(string input)
        {
            string pattern = @"[!@#$%^&*()]_-+=\|";

            bool containsSpecialChars = Regex.IsMatch(input, pattern);

            return containsSpecialChars;
        }

        public async Task<byte[]> ExportFileWord(DataSet dsData, string filePath, string fileImagePath, int left, int top, int height, int width)
        {
            // Load the Word template
            Aspose.Words.Document doc = new Aspose.Words.Document(filePath);

            // Use bookmarks to insert data
            Bookmark bookmark = doc.Range.Bookmarks["BookmarkName"];
            if (bookmark != null)
            {
                // Move the cursor to the bookmark and insert text
                DocumentBuilder builder = new DocumentBuilder(doc);
                builder.MoveToBookmark("BookmarkName");
                builder.Write("Data to be inserted");
            }
            // Use merge fields to insert data from the dataset
            doc.MailMerge.Execute(dsData.Tables[0]);
            for (int i = 1; i < dsData.Tables.Count; i++)
            {
                doc.MailMerge.ExecuteWithRegions(dsData.Tables[i]);
            }


            DocumentBuilder builder1 = new DocumentBuilder(doc);
            MemoryStream imageStream = new MemoryStream(File.ReadAllBytes(fileImagePath));
            builder1.InsertImage(imageStream, RelativeHorizontalPosition.Margin, left, RelativeVerticalPosition.Margin, top, width, height, WrapType.None);

            doc.MailMerge.DeleteFields();

            // Remove empty paragraphs
            doc.MailMerge.CleanupOptions = MailMergeCleanupOptions.RemoveEmptyParagraphs;

            // Remove unused regions
            doc.MailMerge.CleanupOptions |= MailMergeCleanupOptions.RemoveUnusedRegions;

            // Remove header and footer from all sections
            foreach (Aspose.Words.Section section in doc.Sections)
            {
                section.ClearHeadersFooters();
            }
            // Save the filled document to a memory stream
            using (MemoryStream stream = new MemoryStream())
            {
                doc.Save(stream, SaveFormat.Docx);

                // Prepare the file content
                byte[] fileContents = stream.ToArray();
                using (MemoryStream stream1 = new MemoryStream(fileContents))
                {
                    using (WordprocessingDocument doc1 = WordprocessingDocument.Open(stream1, true))
                    {
                        // Remove header and footer from all sections
                        foreach (var section in doc1.MainDocumentPart.Document.Descendants<SectionProperties>())
                        {
                            section.RemoveAllChildren<HeaderReference>();
                            section.RemoveAllChildren<FooterReference>();
                        }

                        // Get the main document part
                        MainDocumentPart mainPart = doc1.MainDocumentPart;

                        // Find and delete the specified text
                        foreach (var textElement in mainPart.Document.Descendants<Text>())
                        {
                            if (textElement.Text.Contains("Evaluation Only. Created with Aspose.Words. Copyright 2003-2023 Aspose Pty Ltd."))
                            {
                                textElement.Text = textElement.Text.Replace("Evaluation Only. Created with Aspose.Words. Copyright 2003-2023 Aspose Pty Ltd.", string.Empty);
                            }
                        }
                    }
                    // Reset the position of the original MemoryStream
                    stream1.Position = 0;

                    // Convert the modified document stream to a byte array
                    byte[] modifiedDocumentBytes = new byte[stream1.Length];
                    await stream1.ReadAsync(modifiedDocumentBytes, 0, (int)stream1.Length);
                    return modifiedDocumentBytes;
                }
            }
        }

        public Task<DataTable> ConvertToDataTable(List<System.Dynamic.ExpandoObject> expandoList, string tableName)
        {
            if (expandoList.Count == 0)
            {
                return Task.FromResult(new DataTable(tableName));
            }

            var firstItem = expandoList[0] as IDictionary<string, object>;
            DataTable dataTable = new DataTable(tableName);

            // Add columns to the DataTable based on the keys of the first item
            foreach (var key in firstItem.Keys)
            {
                dataTable.Columns.Add(key, typeof(object));
            }

            // Add rows to the DataTable
            foreach (var expandoObject in expandoList)
            {
                var dataRow = dataTable.NewRow();

                var item = expandoObject as IDictionary<string, object>;

                // Populate the DataRow with the values from the ExpandoObject
                foreach (var key in item.Keys)
                {
                    dataRow[key] = item[key];
                }

                dataTable.Rows.Add(dataRow);
            }

            return Task.FromResult(dataTable);
        }

        public async Task<FormatedResponse> GetGenderDashboard(HuEmployeeCvInputDTO? model)
        {
            var query = await (
                         from e in _dbContext.HuEmployees.AsNoTracking().Where(p => p.WORK_STATUS_ID != OtherConfig.EMP_STATUS_TERMINATE || p.WORK_STATUS_ID == null)
                         from ecv in _dbContext.HuEmployeeCvs.AsNoTracking().Where(x => x.ID == e.PROFILE_ID).DefaultIfEmpty()
                         from t in _dbContext.SysOtherLists.AsNoTracking().Where(x => x.ID == ecv.GENDER_ID).DefaultIfEmpty()
                             //from t2 in _dbContext.SysContractTypes.AsNoTracking().Where(x => x.ID == t.TYPE_ID).DefaultIfEmpty()
                         where model.OrgIds.Count != 0 ? model.OrgIds.Contains(e.ORG_ID!.Value) : true
                         select new
                         {
                             Id = e.ID,
                             GenderId = ecv.GENDER_ID,
                             GenderName = t.NAME
                         })
                         .GroupBy(x => new { x.GenderId, x.GenderName })
                         .Select(x => new
                         {
                             Name = x.Key.GenderName,
                             Y = x.Count()
                         }).ToListAsync();

            return new FormatedResponse() { InnerBody = query };
        }

        public async Task<FormatedResponse> GetEmpMonthDashboard(HuEmployeeCvInputDTO? model)
        {
            var year = DateTime.Now.ToString("yyyy");
            var isTCT = false;
            if (model.OrgIds.Count != 0)
            {
                var getParent = await _dbContext.HuOrganizations.AsNoTracking().Where(p => p.ID == model.OrgIds[0]).Select(p => p.PARENT_ID).FirstOrDefaultAsync();
                if (getParent != null)
                {
                    isTCT = true;
                }
            }
            var query = await (from e in _dbContext.HuEmployees.AsNoTracking().Where(p => p.WORK_STATUS_ID != OtherConfig.EMP_STATUS_TERMINATE)
                               where (model.OrgIds.Count != 0) ? model.OrgIds.Contains(e.ORG_ID!.Value) : true
                               select new
                               {
                                   Id = e.ID,
                                   JoinDate = e.JOIN_DATE
                               }).ToListAsync();

            var t01 = query.Sum(x => Convert.ToInt32(x.JoinDate?.ToString("yyyyMM")) <= Convert.ToInt32(year + "01") ? 1 : 0);
            var t02 = query.Sum(x => Convert.ToInt32(x.JoinDate?.ToString("yyyyMM")) <= Convert.ToInt32(year + "02") ? 1 : 0);
            var t03 = query.Sum(x => Convert.ToInt32(x.JoinDate?.ToString("yyyyMM")) <= Convert.ToInt32(year + "03") ? 1 : 0);
            var t04 = query.Sum(x => Convert.ToInt32(x.JoinDate?.ToString("yyyyMM")) <= Convert.ToInt32(year + "04") ? 1 : 0);
            var t05 = query.Sum(x => Convert.ToInt32(x.JoinDate?.ToString("yyyyMM")) <= Convert.ToInt32(year + "05") ? 1 : 0);
            var t06 = query.Sum(x => Convert.ToInt32(x.JoinDate?.ToString("yyyyMM")) <= Convert.ToInt32(year + "06") ? 1 : 0);
            var t07 = query.Sum(x => Convert.ToInt32(x.JoinDate?.ToString("yyyyMM")) <= Convert.ToInt32(year + "07") ? 1 : 0);
            var t08 = query.Sum(x => Convert.ToInt32(x.JoinDate?.ToString("yyyyMM")) <= Convert.ToInt32(year + "08") ? 1 : 0);
            var t09 = query.Sum(x => Convert.ToInt32(x.JoinDate?.ToString("yyyyMM")) <= Convert.ToInt32(year + "09") ? 1 : 0);
            var t10 = query.Sum(x => Convert.ToInt32(x.JoinDate?.ToString("yyyyMM")) <= Convert.ToInt32(year + "10") ? 1 : 0);
            var t11 = query.Sum(x => Convert.ToInt32(x.JoinDate?.ToString("yyyyMM")) <= Convert.ToInt32(year + "11") ? 1 : 0);
            var t12 = query.Sum(x => Convert.ToInt32(x.JoinDate?.ToString("yyyyMM")) <= Convert.ToInt32(year + "12") ? 1 : 0);


            var tm01 = query.Sum(x => Convert.ToInt32(x.JoinDate?.ToString("yyyyMM")) == Convert.ToInt32(year + "01") ? 1 : 0);
            var tm02 = query.Sum(x => Convert.ToInt32(x.JoinDate?.ToString("yyyyMM")) == Convert.ToInt32(year + "02") ? 1 : 0);
            var tm03 = query.Sum(x => Convert.ToInt32(x.JoinDate?.ToString("yyyyMM")) == Convert.ToInt32(year + "03") ? 1 : 0);
            var tm04 = query.Sum(x => Convert.ToInt32(x.JoinDate?.ToString("yyyyMM")) == Convert.ToInt32(year + "04") ? 1 : 0);
            var tm05 = query.Sum(x => Convert.ToInt32(x.JoinDate?.ToString("yyyyMM")) == Convert.ToInt32(year + "05") ? 1 : 0);
            var tm06 = query.Sum(x => Convert.ToInt32(x.JoinDate?.ToString("yyyyMM")) == Convert.ToInt32(year + "06") ? 1 : 0);
            var tm07 = query.Sum(x => Convert.ToInt32(x.JoinDate?.ToString("yyyyMM")) == Convert.ToInt32(year + "07") ? 1 : 0);
            var tm08 = query.Sum(x => Convert.ToInt32(x.JoinDate?.ToString("yyyyMM")) == Convert.ToInt32(year + "08") ? 1 : 0);
            var tm09 = query.Sum(x => Convert.ToInt32(x.JoinDate?.ToString("yyyyMM")) == Convert.ToInt32(year + "09") ? 1 : 0);
            var tm10 = query.Sum(x => Convert.ToInt32(x.JoinDate?.ToString("yyyyMM")) == Convert.ToInt32(year + "10") ? 1 : 0);
            var tm11 = query.Sum(x => Convert.ToInt32(x.JoinDate?.ToString("yyyyMM")) == Convert.ToInt32(year + "11") ? 1 : 0);
            var tm12 = query.Sum(x => Convert.ToInt32(x.JoinDate?.ToString("yyyyMM")) == Convert.ToInt32(year + "12") ? 1 : 0);

            List<int> data1 = new() { t01, t02, t03, t04, t05, t06, t07, t08, t09, t10, t11, t12 };
            List<int> data2 = new() { tm01, tm02, tm03, tm04, tm05, tm06, tm07, tm08, tm09, tm10, tm11, tm12 };
            List<List<int>> data = new() { data1, data2 };

            return new FormatedResponse() { InnerBody = data };
        }

        public async Task<FormatedResponse> GetEmpSeniorityDashboard(HuEmployeeCvInputDTO? model)
        {
            int col1 = 1;
            int col2 = 1;
            int col3 = 1;
            int col4 = 1;
            var query = await (from e in _dbContext.HuEmployees.AsNoTracking()
                               where model.OrgIds.Count != 0 ? model.OrgIds.Contains(e.ORG_ID!.Value) : true
                               select new
                               {
                                   Id = e.ID,
                                   JoinDate = e.JOIN_DATE
                               }).ToListAsync();

            if (query.Count > 0)
            {
                foreach (var item in query)
                {
                    var joinDate = Convert.ToInt32(item.JoinDate?.ToString("yyyyMMdd"));
                    var dateTime = Convert.ToInt32(DateTime.Now.ToString("yyyyMMdd"));
                    int seniority = (dateTime - joinDate) / 1000;

                    if (seniority < 1) { col1++; }
                    if (seniority <= 3 && seniority >= 1) { col2++; }
                    if (seniority <= 5 && seniority > 3) { col3++; }
                    if (seniority > 5) { col4++; }
                }
            }

            List<int> data = new() {
                col1 > 0 ? col1 -1 : col1,
                col2 > 0 ? col2 -1 : col2,
                col3 > 0 ? col3 -1 : col3,
                col4 > 0 ? col4 -1 : col4,
            };

            return new FormatedResponse() { InnerBody = data };
        }
        public async Task<FormatedResponse> GetGeneralInfomationDashboard(HuEmployeeCvInputDTO? model)
        {
            var totalEmp = await (from e in _dbContext.HuEmployees.AsNoTracking().Where(p => p.WORK_STATUS_ID != OtherConfig.EMP_STATUS_TERMINATE)
                                  from cv in _dbContext.HuEmployeeCvs.AsNoTracking().Where(p => p.ID == e.PROFILE_ID).DefaultIfEmpty()
                                  where model.OrgIds.Count != 0 ? model.OrgIds.Contains(e.ORG_ID!.Value) : true
                                  select new
                                  {
                                      Id = e.ID,
                                      Age = cv.BIRTH_DATE == null ? 0 : DateTime.Now.Subtract(cv.BIRTH_DATE.Value).TotalDays / 365.25,
                                  }).ToListAsync();
            var newEmpInMonth = await _dbContext.HuEmployees.AsNoTracking().Where(p => p.JOIN_DATE == null ? false : (p.JOIN_DATE!.Value.Month == DateTime.Now.Month && p.JOIN_DATE!.Value.Year == DateTime.Now.Year) && p.WORK_STATUS_ID != OtherConfig.EMP_STATUS_TERMINATE && (model.OrgIds.Count != 0 ? model.OrgIds.Contains(p.ORG_ID!.Value) : true)).CountAsync();
            var terEmpInMonth = await _dbContext.HuEmployees.AsNoTracking().Where(p => p.TER_LAST_DATE == null ? false : (p.TER_LAST_DATE!.Value.Month == DateTime.Now.Month && p.TER_LAST_DATE!.Value.Year == DateTime.Now.Year) && (model.OrgIds.Count != 0 ? model.OrgIds.Contains(p.ORG_ID!.Value) : true)).CountAsync();
            var averageAge = totalEmp.Count == 0 ? 0 : Math.Floor(totalEmp.Sum(p => p.Age) / totalEmp.Count);
            var query = new
            {
                TotalEmp = totalEmp.Count,
                NewEmpInMonth = newEmpInMonth,
                TerEmpInMonth = terEmpInMonth,
                AverageAge = averageAge,
            };
            return new FormatedResponse() { InnerBody = query };
        }
        public async Task<FormatedResponse> GetNativeInfomationDashboard(HuEmployeeCvInputDTO? model)
        {
            var result = await (from e in _dbContext.HuEmployees.AsNoTracking().Where(p => p.WORK_STATUS_ID != OtherConfig.EMP_STATUS_TERMINATE)
                                from cv in _dbContext.HuEmployeeCvs.AsNoTracking().Where(p => p.ID == e.PROFILE_ID).DefaultIfEmpty()
                                from s in _dbContext.SysOtherLists.AsNoTracking().Where(p => p.ID == cv.NATIVE_ID).DefaultIfEmpty()
                                where model.OrgIds.Count != 0 ? model.OrgIds.Contains(e.ORG_ID!.Value) : true
                                select new
                                {
                                    Id = s.ID,
                                    NativeName = s.NAME ?? "Không xác định",
                                }).GroupBy(p => new { p.Id, p.NativeName }).Select(group => new
                                {
                                    Name = group.Key.NativeName,
                                    Y = group.Count(),
                                }).ToListAsync();

            return new FormatedResponse() { InnerBody = result };
        }
        public async Task<FormatedResponse> GetIsMemberInfomationDashboard(HuEmployeeCvInputDTO? model)
        {
            var result = await (from e in _dbContext.HuEmployees.AsNoTracking().Where(p => p.WORK_STATUS_ID != OtherConfig.EMP_STATUS_TERMINATE)
                                from cv in _dbContext.HuEmployeeCvs.AsNoTracking().Where(p => p.ID == e.PROFILE_ID).DefaultIfEmpty()
                                where model.OrgIds.Count != 0 ? model.OrgIds.Contains(e.ORG_ID!.Value) : true
                                select new
                                {
                                    IsMember = cv.IS_MEMBER == null ? 0 : cv.IS_MEMBER.Value ? 1 : 0,
                                    Name = cv.IS_MEMBER == null ? "Không là Đảng viên" : cv.IS_MEMBER.Value ? "Là Đảng viên" : "Không là Đảng viên",
                                }).GroupBy(p => new { p.IsMember, p.Name }).Select(group => new
                                {
                                    Name = group.Key.Name,
                                    Y = group.Count(),
                                }).ToListAsync();

            return new FormatedResponse() { InnerBody = result };
        }
        public async Task<FormatedResponse> GetJobInfomationDashboard(HuEmployeeCvInputDTO? model)
        {
            var result = await (from e in _dbContext.HuEmployees.AsNoTracking().Where(p => p.WORK_STATUS_ID != OtherConfig.EMP_STATUS_TERMINATE).DefaultIfEmpty()
                                from p in _dbContext.HuPositions.AsNoTracking().Where(p => p.ID == e.POSITION_ID).DefaultIfEmpty()
                                from j in _dbContext.HuJobs.AsNoTracking().Where(j => j.ACTFLG == "A" && p.JOB_ID == j.ID).DefaultIfEmpty()
                                from o in _dbContext.SysOtherLists.AsNoTracking().Where(o => o.ID == j.JOB_FAMILY_ID).DefaultIfEmpty()
                                where model.OrgIds.Count != 0 ? model.OrgIds.Contains(e.ORG_ID!.Value) : true
                                select new
                                {
                                    Id = o.ID,
                                    Name = o.NAME
                                }).GroupBy(p => new { p.Id, p.Name }).Select(group => new
                                {
                                    Name = group.Key.Name,
                                    Y = group.Count(),
                                }).ToListAsync();

            return new FormatedResponse() { InnerBody = result };
        }
        public async Task<FormatedResponse> GetPositionInfomationDashboard(HuEmployeeCvInputDTO? model)
        {

            var result = (from p in _dbContext.HuPositions
                          join j in _dbContext.HuJobs on p.JOB_ID equals j.ID into pj
                          from pjResult in pj.DefaultIfEmpty()
                          join e in _dbContext.HuEmployees on p.ID equals e.POSITION_ID into pe
                          from peResult in pe.Where(p => model.OrgIds.Count != 0 ? model.OrgIds.Contains(p.ORG_ID!.Value) : true).DefaultIfEmpty() // bo doan where neu can
                          where pjResult != null && peResult != null
                          group peResult by new { Id = p.JOB_ID, NameVn = pjResult.NAME_VN, NameEn = pjResult.NAME_EN } into g
                          select new
                          {
                              Id = g.Key.Id,
                              NameVn = g.Key.NameVn,
                              NameEn = g.Key.NameEn,
                              EmployeeCount = g.Count(),
                          }).ToList();
            List<string> listName = new List<string>();
            List<int> listValue = new List<int>();
            result.ForEach(x =>
            {
                listName.Add(x.NameVn);
                listValue.Add(x.EmployeeCount);
            });
            return new FormatedResponse()
            {
                InnerBody = new
                {
                    ListName = listName,
                    ListValue = listValue,
                }
            };
        }
        public async Task<FormatedResponse> GetLevelInfomationDashboard(HuEmployeeCvInputDTO? model)
        {

            var listLevel = await _dbContext.SysOtherLists.AsNoTracking().Where(p => p.IS_ACTIVE == true && p.TYPE_ID == 11).ToListAsync();
            List<string> listName = new List<string>();
            List<int> listValue = new List<int>();
            listLevel.ForEach(x =>
            {
                var count = (from c in _dbContext.HuCertificates.AsNoTracking().Where(p => p.LEVEL_ID == x.ID)
                             from e in _dbContext.HuEmployees.AsNoTracking().Where(p => p.ID == c.EMPLOYEE_ID && p.WORK_STATUS_ID != OtherConfig.EMP_STATUS_TERMINATE).DefaultIfEmpty()
                             where model.OrgIds.Count != 0 ? model.OrgIds.Contains(e.ORG_ID!.Value) : true
                             select c).Count();
                listName.Add(x.NAME!);
                listValue.Add(count);
            });
            return new FormatedResponse()
            {
                InnerBody = new
                {
                    ListName = listName,
                    ListValue = listValue,
                }
            };
        }
        public async Task<FormatedResponse> GetWorkingAgeInfomationDashboard(HuEmployeeCvInputDTO? model)
        {

            var listLevel = await (from e in _dbContext.HuEmployees.AsNoTracking().Where(p => p.WORK_STATUS_ID != OtherConfig.EMP_STATUS_TERMINATE).DefaultIfEmpty()
                                   from cv in _dbContext.HuEmployeeCvs.AsNoTracking().Where(p => p.ID == e.PROFILE_ID).DefaultIfEmpty()
                                   where model.OrgIds.Count != 0 ? model.OrgIds.Contains(e.ORG_ID!.Value) : true
                                   select new
                                   {
                                       Id = e.ID,
                                       Age = Math.Floor(DateTime.Now.Subtract(Convert.ToDateTime(cv.BIRTH_DATE)).Days / 365.25),
                                   }).ToListAsync();


            List<string> listName = new List<string>() { "<30", "30-35", "36-40", "41-50", "51-59", ">=60" };
            List<int> listValue = new List<int>();
            listValue.Add(listLevel.Sum(p => p.Age < 30 ? 1 : 0));
            listValue.Add(listLevel.Sum(p => p.Age >= 30 && p.Age <= 35 ? 1 : 0));
            listValue.Add(listLevel.Sum(p => p.Age >= 36 && p.Age <= 40 ? 1 : 0));
            listValue.Add(listLevel.Sum(p => p.Age >= 41 && p.Age <= 50 ? 1 : 0));
            listValue.Add(listLevel.Sum(p => p.Age >= 51 && p.Age <= 59 ? 1 : 0));
            listValue.Add(listLevel.Sum(p => p.Age >= 60 ? 1 : 0));
            return new FormatedResponse()
            {
                InnerBody = new
                {
                    ListName = listName,
                    ListValue = listValue,
                }
            };
        }
        public async Task<FormatedResponse> GetNewEmpMonthDashboard(HuEmployeeCvInputDTO? model)
        {
            var year = DateTime.Now.Year;

            var isTCT = false;
            if (model.OrgIds.Count != 0)
            {
                var getParent = await _dbContext.HuOrganizations.AsNoTracking().Where(p => p.ID == model.OrgIds[0]).Select(p => p.PARENT_ID).FirstOrDefaultAsync();
                if (getParent != null)
                {
                    isTCT = true;
                }
            }
            var query = await (from e in _dbContext.HuEmployees.AsNoTracking().Where(p => p.JOIN_DATE!.Value.Year == year)
                               where model.OrgIds.Count != 0 ? model.OrgIds.Contains(e.ORG_ID!.Value) : true
                               select new
                               {
                                   Id = e.ID,
                                   JoinDateMonth = e.JOIN_DATE!.Value.Month,
                                   TerDateMonth = e.TER_EFFECT_DATE == null ? 13 : e.TER_EFFECT_DATE.Value.Month,
                               }).ToListAsync();
            List<int> data = new List<int>();
            for (int i = 1; i <= 12; i++)
            {
                data.Add(query.Where(p => p.JoinDateMonth == i && p.TerDateMonth > i).Count());
            }
            return new FormatedResponse() { InnerBody = data };
        }

        public async Task<FormatedResponse> GetNameOrgDashboard(HuEmployeeCvInputDTO? model)
        {
            if (model.OrgIds.Count <= 0)
            {
                return new FormatedResponse()
                {
                    InnerBody = new
                    {
                        ListName = "",
                    }
                };
            }
            if (model.OrgIds.Contains(1))
            {
                return new FormatedResponse()
                {
                    InnerBody = new
                    {
                        ListName = "Tổng công ty Thép Việt Nam - CTCP",
                    }
                };
            }
            var listName = await (_dbContext.HuOrganizations.AsNoTracking().Where(p => model!.OrgIds!.Contains(p.ID)).Select(p => p.NAME)).ToListAsync();

            return new FormatedResponse()
            {
                InnerBody = new
                {
                    ListName = string.Join("; ", listName.ToArray()).ToString()
                }
            };
        }

        public async Task<FormatedResponse> UpdateGeneralInfo2(StaffProfileUpdateDTO request)
        {
            try
            {
                // check duplicate field ITIME_ID
                var check = await _dbContext.HuEmployees
                                  .AnyAsync(x =>
                                      x.ITIME_ID == request.ItimeId
                                      && x.ID != request.Id
                                      && x.ITIME_ID != ""
                                      && x.ITIME_ID != null
                                  );

                if (check)
                {
                    return new FormatedResponse()
                    {
                        ErrorType = EnumErrorType.CATCHABLE,
                        MessageCode = CommonMessageCodes.ITEMID_HAS_EXISTS,
                        StatusCode = EnumStatusCode.StatusCode400
                    };
                }
                else
                {
                    var entity = _dbContext.HuEmployees.FirstOrDefault(x => x.ID == request.Id);

                    var entity_cv = _dbContext.HuEmployeeCvs.FirstOrDefault(x => x.ID == entity!.PROFILE_ID);

                    if (entity != null)
                    {
                        // when a user updates field "ITIME_ID"
                        entity.ITIME_ID = request.ItimeId == "" ? null : request.ItimeId;

                        // when a user updates field "EMPLOYEE_OBJECT_ID"
                        entity.EMPLOYEE_OBJECT_ID = request.ObjectEmployeeId;
                        entity_cv!.EMPLOYEE_OBJECT_ID = request.ObjectEmployeeId;

                        // when a user updates field "OTHER_NAME"
                        entity_cv.OTHER_NAME = request.OtherName;

                        // when a user updates field "IS_NOT_CONTRACT"
                        entity.IS_NOT_CONTRACT = request.IsNotContract;


                        // get approve id
                        var approveId = _dbContext.SysOtherLists.FirstOrDefault(x => x.CODE == "DD")!.ID;

                        // when an employee does not have the contract
                        // set work status id
                        var check1 = _dbContext.HuContracts.Any(x => x.EMPLOYEE_ID == request.Id);

                        var check2 = _dbContext.HuContracts.Any(x => x.EMPLOYEE_ID == request.Id && x.STATUS_ID == approveId);

                        if (check1 == false || check2 == false)
                        {
                            // when "IS_NOT_CONTRACT == true"
                            if (entity.IS_NOT_CONTRACT == true)
                            {
                                // get work status id by code
                                var workStatusId = _dbContext.SysOtherLists.FirstOrDefault(x => x.CODE == "ESW")!.ID;
                                entity.WORK_STATUS_ID = workStatusId;

                                var statusDetailId = _dbContext.SysOtherLists.FirstOrDefault(x => x.CODE == "00068")!.ID;
                                entity.STATUS_DETAIL_ID = statusDetailId;
                            }
                            else
                            {
                                entity.WORK_STATUS_ID = null;
                                entity.STATUS_DETAIL_ID = null;
                            }
                        }
                        

                        _dbContext.SaveChanges();
                        
                        return new FormatedResponse()
                        {
                            MessageCode = CommonMessageCode.UPDATE_SUCCESS,
                            InnerBody = true,
                            StatusCode = EnumStatusCode.StatusCode200
                        };
                    }
                    else
                    {
                        return new FormatedResponse()
                        {
                            ErrorType = EnumErrorType.CATCHABLE,
                            MessageCode = CommonMessageCode.ENTITIES_NOT_FOUND,
                            StatusCode = EnumStatusCode.StatusCode400
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                return new FormatedResponse()
                {
                    ErrorType = EnumErrorType.UNCATCHABLE,
                    MessageCode = ex.Message,
                    StatusCode = EnumStatusCode.StatusCode400
                };
            }
        }
    }
}