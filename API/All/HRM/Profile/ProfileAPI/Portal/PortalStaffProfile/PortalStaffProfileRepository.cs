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
using API.DTO.PortalDTO;
using System.Reflection;
using DocumentFormat.OpenXml.Drawing;
using API.Entities;

namespace API.All.HRM.Profile.ProfileAPI.Portal.PortalEducation
{
    public class PortalStaffProfileRepository : IPortalStaffProfileRepository
    {
        private readonly GenericUnitOfWork _uow;
        private readonly FullDbContext _dbContext;
        private IGenericRepository<HU_EMPLOYEE_CV, HuEmployeeCvDTO> _genericRepository;
        private readonly GenericReducer<HU_EMPLOYEE_CV, HuEmployeeCvDTO> _genericReducer;
        private readonly IWebHostEnvironment _env;
        private readonly AppSettings _appSettings;
        private readonly IFileService _fileService;


        public PortalStaffProfileRepository(FullDbContext context, GenericUnitOfWork uow, IWebHostEnvironment env, IOptions<AppSettings> options, IFileService fileService)
        {
            _dbContext = context;
            _uow = uow;
            _genericRepository = _uow.GenericRepository<HU_EMPLOYEE_CV, HuEmployeeCvDTO>();
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
                               from d in _dbContext.HuBanks.Where(b => b.ID == c.BANK_ID_2).DefaultIfEmpty()
                               where c.ID == employeeCvId

                               select new
                               {
                                   Id = employeeCvId,
                                   BankId = c.BANK_ID,
                                   BankId2 = c.BANK_ID_2,
                                   AccountBankName = RemoveDiacritics(c.FULL_NAME).ToUpper(),
                                   BankName = b.NAME ?? "",
                                   BankBranch = c.BANK_BRANCH,
                                   BankNo = c.BANK_NO ?? "",
                                   BankNo2 = c.BANK_NO_2 ?? "",
                                   BankName2 = d.NAME ?? "",
                                   BankBranch2 = c.BANK_BRANCH_2
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
                                   BankBranch = c.BANK_BRANCH,
                                   BankNo = c.BANK_NO ?? "",
                                   BankNo2 = c.BANK_NO_2 ?? "",
                                   BankName2 = d.NAME ?? "",
                                   BankBranch2 = c.BANK_BRANCH_2
                               }).SingleAsync();
            return new() { InnerBody = query };
        }

        public async Task<FormatedResponse> UpdateBank(StaffProfileUpdateDTO request)
        {
            try
            {
                var entity = _dbContext.HuEmployeeCvs.Where(x => x.ID == request.Id).FirstOrDefault();
                if (entity != null)
                {
                    entity.BANK_ID = request.BankId;
                    entity.BANK_ID_2 = request.BankId2;
                    entity.BANK_BRANCH = request.BankBranchId;
                    entity.BANK_BRANCH_2 = request.BankBranchId2;
                    entity.BANK_NO = request.BankNo;
                    entity.BANK_NO_2 = request.BankNo2;
                    _dbContext.SaveChanges();
                    return new() { MessageCode = CommonMessageCode.UPDATE_SUCCESS, InnerBody = entity, StatusCode = EnumStatusCode.StatusCode200 };
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
                               where e.PROFILE_ID == employeeCvId
                               select new
                               {
                                   Name = "Index" + c.PAPER_ID,
                                   PaperIdCheck = true
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
            var query = await (from c in _dbContext.HuEmployeeCvs
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
                                   AddressIdentity = c.ID_PLACE,
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
                                   TaxCodeAddress = c.TAX_CODE_ADDRESS,
                                   BirthPlace = c.BIRTH_PLACE
                               }).SingleAsync();
            return new() { InnerBody = query };
        }

        public async Task<FormatedResponse> GetAdditonalInfo(long employeeCvId)
        {
            var query = await (from c in _dbContext.HuEmployeeCvs
                               from d in _dbContext.HuEmployees.Where(c => c.PROFILE_ID == c.ID).DefaultIfEmpty()
                               from o in _dbContext.HuOrganizations.Where(x => x.ID == d.ORG_ID).DefaultIfEmpty()
                               from com in _dbContext.HuCompanys.Where(x => x.ID == o.COMPANY_ID).DefaultIfEmpty()
                               from s in _dbContext.SysOtherLists.Where(x => x.ID == com.INS_UNIT).DefaultIfEmpty()
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
                                   InsArea = s.NAME,
                                   InsNumber = c.INSURENCE_NUMBER,
                                   HealthCareAddress = c.HEALTH_CARE_ADDRESS,
                                   InsCardNumber = c.INS_CARD_NUMBER,
                                   FamilyMember = c.FAMILY_MEMBER,
                                   FamilyPolicy = c.FAMILY_POLICY,
                                   Veterans = c.VETERANS,
                                   PoliticalTheory = c.POLITICAL_THEORY,
                                   CareerBeforeRecruitment = c.CARRER_BEFORE_RECUITMENT,
                                   TitleConferred = c.TITLE_CONFERRED,
                                   SchoolOfWork = c.SCHOOL_OF_WORK
                               }).SingleAsync();
            return new() { InnerBody = query };
        }

        public async Task<FormatedResponse> GetAdditonal(long id)
        {
            var query = await (from c in _dbContext.HuEmployeeCvs
                               from e in _dbContext.HuEmployees.Where(x => x.PROFILE_ID == c.ID).DefaultIfEmpty()
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
                                   InsArea = e.INSURENCE_AREA_ID,
                                   InsNumber = c.INSURENCE_NUMBER,
                                   HealthCareAddress = c.HEALTH_CARE_ADDRESS,
                                   InsCardNumber = c.INS_CARD_NUMBER,
                                   FamilyMember = c.FAMILY_MEMBER,
                                   FamilyPolicy = c.FAMILY_POLICY,
                                   Veterans = c.VETERANS,
                                   PoliticalTheory = c.POLITICAL_THEORY,
                                   CareerBeforeRecruitment = c.CARRER_BEFORE_RECUITMENT,
                                   TitleConferred = c.TITLE_CONFERRED,
                                   SchoolOfWork = c.SCHOOL_OF_WORK
                               }).SingleAsync();
            return new() { InnerBody = query };
        }

        public async Task<FormatedResponse> UpdateAdditonal(StaffProfileUpdateDTO request)
        {
            try
            {
                var entity = _dbContext.HuEmployeeCvs.Where(x => x.ID == request.Id).FirstOrDefault();
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
                    //entity.INSURENCE_AREA = request.InsArea;
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
                    _dbContext.SaveChanges();
                    return new() { MessageCode = CommonMessageCode.UPDATE_SUCCESS, InnerBody = entity, StatusCode = EnumStatusCode.StatusCode200 };
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
                    entity.TAX_CODE = request.TaxCode;
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
                    _dbContext.SaveChanges();

                    return new() { MessageCode = CommonMessageCode.UPDATE_SUCCESS, InnerBody = entity, StatusCode = EnumStatusCode.StatusCode200 };
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

        public async Task<FormatedResponse> GetBasic(long employeeId)
        {
            try
            {
                var query = await (from p in _dbContext.HuEmployees
                                   from o in _dbContext.HuOrganizations.Where(x => x.ID == p.ORG_ID).DefaultIfEmpty()
                                   from pos in _dbContext.HuPositions.Where(x => x.ID == p.POSITION_ID).DefaultIfEmpty()
                                   from di in _dbContext.HuPositions.Where(x => x.ID == pos.LM).DefaultIfEmpty()
                                   from p2 in _dbContext.HuEmployees.Where(x => x.POSITION_ID == di.ID && x.ORG_ID == di.ORG_ID).DefaultIfEmpty()
                                   from ep2 in _dbContext.HuEmployeeCvs.Where(x => x.ID == p2.PROFILE_ID).DefaultIfEmpty()
                                   from pos2 in _dbContext.HuPositions.Where(x => x.ID == p2.POSITION_ID).DefaultIfEmpty()
                                   from st in _dbContext.HuJobs.Where(x => x.ID == pos2.JOB_ID).DefaultIfEmpty()
                                   from com in _dbContext.HuCompanys.Where(x => x.ID == o.COMPANY_ID).DefaultIfEmpty()
                                   from ob in _dbContext.SysOtherLists.Where(x => x.ID == p.EMPLOYEE_OBJECT_ID).DefaultIfEmpty()
                                   where p.ID == employeeId
                                   select new
                                   {
                                       Id = employeeId,
                                       DirectManager = di != null ? ep2.FULL_NAME : "",
                                       EmployeeCode = p.CODE ?? "",
                                       PositionDirectManager = di != null ? st.NAME_VN : "",
                                       Company = com.NAME_VN ?? "",
                                       Position = pos.NAME ?? "",
                                       ObjectEmployee = ob.NAME ?? "",
                                       WorkingAddress = com.WORK_ADDRESS ?? "",
                                       OrgName = o.NAME ?? ""
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
                    return new() { MessageCode = CommonMessageCode.UPDATE_SUCCESS, InnerBody = entity, StatusCode = EnumStatusCode.StatusCode200 };
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
                                       HighestMilitaryPosition = c.HIGHEST_MILITARY_POSITION
                                   }).SingleAsync();
                return new() { InnerBody = query };
            }
            catch (Exception ex)
            {
                return new FormatedResponse() { MessageCode = CommonMessageCode.GET_METHOD_FAILED, ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400 };
            }
        }


        public async Task<FormatedResponse> GetEducation(long employeeCvId)
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
                                   where c.ID == employeeCvId
                                   select new
                                   {
                                       Id = employeeCvId,
                                       EducationLevel = e.NAME,
                                       LearningLevel = z.NAME,
                                       ComputerSkill = c.COMPUTER_SKILL,
                                       License = c.LICENSE,
                                       Qualification1 = q1.NAME,
                                       Qualification2 = q2.NAME,
                                       Qualification3 = q3.NAME,
                                       TrainingForm1 = t.NAME,
                                       TrainingForm2 = t2.NAME,
                                       TrainingForm3 = t3.NAME,
                                       School1 = s.NAME,
                                       School2 = s2.NAME,
                                       School3 = s3.NAME,
                                       Language1 = l1.NAME,
                                       Language2 = l2.NAME,
                                       Language3 = l3.NAME,
                                       LanguageLevel1 = lv1.NAME,
                                       LanguageLevel2 = lv2.NAME,
                                       LanguageLevel3 = lv3.NAME,
                                   }).SingleAsync();
                return new() { InnerBody = query };
            }
            catch (Exception ex)
            {
                return new FormatedResponse() { MessageCode = CommonMessageCode.GET_METHOD_FAILED, ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400 };

            }
        }


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
                                   }).SingleAsync();
                return new() { InnerBody = query };
            }
            catch (Exception ex)
            {
                return new FormatedResponse() { MessageCode = CommonMessageCode.GET_METHOD_FAILED, ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400 };

            }
        }

        public async Task<FormatedResponse> UpdateEducationId(StaffProfileUpdateDTO request)
        {
            try
            {
                var entity = _dbContext.HuEmployeeCvs.Where(x => x.ID == request.Id).FirstOrDefault();
                if (entity != null)
                {
                    entity.EDUCATION_LEVEL_ID = request.EducationLevelId;
                    entity.LEARNING_LEVEL_ID = request.LearningLevelId;
                    entity.COMPUTER_SKILL = request.ComputerSkill;
                    entity.LICENSE = request.License;
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
                    return new() { MessageCode = CommonMessageCode.UPDATE_SUCCESS, InnerBody = entity, StatusCode = EnumStatusCode.StatusCode200 };

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
                               where c.ID == employeeCvId
                               select new
                               {
                                   Id = employeeCvId,
                                   Presenter = c.PRESENTER,
                                   PresenterAddress = c.PRESENTER_ADDRESS,
                                   PresenterPhoneNumber = c.PRESENTER_PHONE_NUMBER
                               }).SingleAsync();
            return new() { InnerBody = query };
        }

        public async Task<FormatedResponse> GetContact(long employeeCvId)
        {
            var query = await (from c in _dbContext.HuEmployeeCvs
                               from s in _dbContext.HuProvinces.Where(x => x.ID == c.PROVINCE_ID).DefaultIfEmpty()
                               from s1 in _dbContext.HuProvinces.Where(x => x.ID == c.PROVINCE_ID).DefaultIfEmpty()
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
            var query = await (from c in _dbContext.HuEmployeeCvs
                               from s in _dbContext.HuProvinces.Where(x => x.ID == c.PROVINCE_ID).DefaultIfEmpty()
                               from s1 in _dbContext.HuProvinces.Where(x => x.ID == c.PROVINCE_ID).DefaultIfEmpty()
                               from w in _dbContext.HuWards.Where(x => x.ID == c.WARD_ID).DefaultIfEmpty()
                               from w1 in _dbContext.HuWards.Where(x => x.ID == c.CUR_WARD_ID).DefaultIfEmpty()
                               from d1 in _dbContext.HuDistricts.Where(x => x.ID == c.CUR_DISTRICT_ID).DefaultIfEmpty()
                               from d2 in _dbContext.HuDistricts.Where(x => x.ID == c.DISTRICT_ID).DefaultIfEmpty()
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
                                   ProvinceId = s.ID,
                                   CurProvinceId = s1.ID,
                                   WardId = w.ID,
                                   CurWardId = w1.ID,
                                   CurDistrictId = d1.ID,
                                   DistrictId = d2.ID,
                                   LandlinePhone = c.MOBILE_PHONE_LAND,
                                   EmailCompany = c.WORK_EMAIL,
                                   EmailPersonal = c.EMAIL
                               }).SingleAsync();
            return new() { InnerBody = query };
        }

        public async Task<FormatedResponse> UpdateContactId(StaffProfileUpdateDTO request)
        {
            var entity = _dbContext.HuEmployeeCvs.Where(x => x.ID == request.Id).FirstOrDefault();
            if (entity != null)
            {
                entity.IS_HOST = request.IsHost;
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
                return new() { MessageCode = CommonMessageCode.UPDATE_SUCCESS, InnerBody = entity, StatusCode = EnumStatusCode.StatusCode200 };
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
                return new() { MessageCode = CommonMessageCode.UPDATE_SUCCESS, InnerBody = entity, StatusCode = EnumStatusCode.StatusCode200 };
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
                return new() { MessageCode = CommonMessageCode.UPDATE_SUCCESS, InnerBody = entity, StatusCode = EnumStatusCode.StatusCode200 };
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
                                       GovermentManagementId = c.GOVERMENT_MANAGEMENT_ID
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
                    return new() { MessageCode = CommonMessageCode.UPDATE_SUCCESS, InnerBody = entity, StatusCode = EnumStatusCode.StatusCode200 };
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
                var query = await (from p in _dbContext.HuEmployees
                                   from o in _dbContext.HuOrganizations.Where(x => x.ID == p.ORG_ID).DefaultIfEmpty()
                                   from pos in _dbContext.HuPositions.Where(x => x.ID == p.POSITION_ID).DefaultIfEmpty()
                                   from di in _dbContext.HuPositions.Where(x => x.ID == pos.LM).DefaultIfEmpty()
                                   from p2 in _dbContext.HuEmployees.Where(x => x.POSITION_ID == di.ID && x.ORG_ID == di.ORG_ID).DefaultIfEmpty()
                                   from ep2 in _dbContext.HuEmployeeCvs.Where(x => x.ID == p2.PROFILE_ID).DefaultIfEmpty()
                                   from pos2 in _dbContext.HuPositions.Where(x => x.ID == p2.POSITION_ID).DefaultIfEmpty()
                                   from st in _dbContext.HuJobs.Where(x => x.ID == pos2.JOB_ID).DefaultIfEmpty()
                                   from com in _dbContext.HuCompanys.Where(x => x.ID == o.COMPANY_ID).DefaultIfEmpty()
                                   from ob in _dbContext.SysOtherLists.Where(x => x.ID == p.EMPLOYEE_OBJECT_ID).DefaultIfEmpty()
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
                                       OrgId = pos.ORG_ID
                                   }).SingleAsync();
                return new() { InnerBody = query };
            }
            catch
            {
                return new FormatedResponse() { MessageCode = CommonMessageCode.GET_METHOD_FAILED, ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400 };
            }
        }

        public async Task<FormatedResponse> UpdateGeneralInfo(StaffProfileUpdateDTO request)
        {
            try
            {
                var entity = _dbContext.HuEmployees.Where(x => x.ID == request.Id).SingleOrDefault();
                if (entity != null)
                {
                    entity.ORG_ID = request.OrgId;
                    entity.POSITION_ID = request.PositionId;
                    entity.EMPLOYEE_OBJECT_ID = request.ObjectEmployeeId;
                    _dbContext.SaveChanges();
                    return new() { MessageCode = CommonMessageCode.UPDATE_SUCCESS, InnerBody = entity, StatusCode = EnumStatusCode.StatusCode200 };
                }
                else
                {
                    return new FormatedResponse() { MessageCode = CommonMessageCode.ENTITIES_NOT_FOUND, ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode500 };
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
                              select new SysOtherListDTO()
                              {
                                  Id = o.ID,
                                  Name = o.NAME ?? string.Empty
                              }).ToListAsync();
            return new() { InnerBody = list };

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
                            string location = System.IO.Path.Combine(_env.ContentRootPath, _appSettings.StaticFolders.Root, _appSettings.StaticFolders.Avatars);

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
                                string location = System.IO.Path.Combine(_env.ContentRootPath, _appSettings.StaticFolders.Root, _appSettings.StaticFolders.Avatars);

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
                string location = System.IO.Path.Combine(_env.ContentRootPath, _appSettings.StaticFolders.Root, _appSettings.StaticFolders.Avatars);

                if (!Directory.Exists(location))
                {
                    Directory.CreateDirectory(location);
                }
                string filePath = System.IO.Path.Combine(location, fileName);
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

        public async Task<FormatedResponse> GetCode(string code)
        {
            decimal num;
            var queryCode = await (from x in _dbContext.HuEmployees where x.CODE.Length - code.Length == 4 select x.CODE).ToListAsync();
            var existingCode = (from p in queryCode where Decimal.TryParse(p.Substring(code.Length), out num) orderby p descending select p).ToList();
            string newcode = StringCodeGenerator.CreateNewCode(code, 4, existingCode);
            return new FormatedResponse() { InnerBody = new { Code = newcode } };
        }


        /*
            khai báo phương thức GetEducationByPortal()
            có tham số truyền vào là employeeId
            tức là ID của bảng HU_EMPLOYEE

            còn cái phương thức cũ GetEducation()
            thì nó dùng ID của bảng HU_EMPLOYEE_CV

            phương thức GetEducationByPortal()
            có tác dụng lấy ra trình độ học vấn
        */
        public async Task<FormatedResponse> GetEducationByPortal(long employeeId, long? time)
        {
            try
            {
                var query1 = await (from table in _dbContext.HuEmployees.Where(x => x.ID == employeeId).DefaultIfEmpty()
                                    from c in _dbContext.HuEmployeeCvs.Where(x => x.ID == table.PROFILE_ID).DefaultIfEmpty()
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
                                    select new HuEmployeeCvEditDTO
                                    {
                                        // id của HuEmployeeCvs
                                        Id = c.ID,

                                        // id của HuEmployee
                                        EmployeeId = table.ID,

                                        // Trình độ văn hóa
                                        EducationLevelId = c.EDUCATION_LEVEL_ID,
                                        EducationLevel = e.NAME,

                                        // Trình độ học vấn
                                        LearningLevelId = c.LEARNING_LEVEL_ID,
                                        LearningLevel = z.NAME,

                                        // Trình độ tin học
                                        ComputerSkillId = c.COMPUTER_SKILL_ID,
                                        ComputerSkill = reference_1.NAME,

                                        // Bằng lái xe
                                        LicenseId = c.LICENSE_ID,
                                        License = reference_2.NAME,

                                        // Trình độ chuyên môn 1
                                        Qualificationid = c.QUALIFICATIONID,
                                        Qualification1 = q1.NAME,

                                        // Trình độ chuyên môn 2
                                        Qualificationid2 = c.QUALIFICATIONID_2,
                                        Qualification2 = q2.NAME,

                                        // Trình độ chuyên môn 3
                                        Qualificationid3 = c.QUALIFICATIONID_3,
                                        Qualification3 = q3.NAME,

                                        // Hình thức đào tạo 1
                                        TrainingFormId = c.TRAINING_FORM_ID,
                                        TrainingForm1 = t.NAME,

                                        // Hình thức đào tạo 2
                                        TrainingFormId2 = c.TRAINING_FORM_ID_2,
                                        TrainingForm2 = t2.NAME,

                                        // Hình thức đào tạo 3
                                        TrainingFormId3 = c.TRAINING_FORM_ID_3,
                                        TrainingForm3 = t3.NAME,

                                        // Trường học 1
                                        School1 = s.NAME,

                                        // Trường học 2
                                        School2 = s2.NAME,

                                        // Trường học 3
                                        School3 = s3.NAME,

                                        // Ngoại ngữ 1
                                        Language1 = l1.NAME,

                                        // Ngoại ngữ 2
                                        Language2 = l2.NAME,

                                        // Ngoại ngữ 3
                                        Language3 = l3.NAME,

                                        // Trình độ ngoại ngữ 1
                                        LanguageLevel1 = lv1.NAME,

                                        // Trình độ ngoại ngữ 2
                                        LanguageLevel2 = lv2.NAME,

                                        // Trình độ ngoại ngữ 3
                                        LanguageLevel3 = lv3.NAME,
                                    }).FirstAsync();


                // lấy dữ liệu bằng cấp/chứng chỉ
                // chỉ lấy bản ghi "là bằng chính"
                // IS_PRIME == true
                var list_certificate = (from p in _dbContext.HuCertificates.Where(x => x.EMPLOYEE_ID == employeeId && x.IS_PRIME == true)
                                        from reference_1 in _dbContext.SysOtherLists.Where(x => x.ID == p.LEVEL_ID).DefaultIfEmpty()
                                        from reference_2 in _dbContext.SysOtherLists.Where(x => x.ID == p.LEVEL_TRAIN).DefaultIfEmpty()
                                        from reference_3 in _dbContext.SysOtherLists.Where(x => x.ID == p.TYPE_TRAIN).DefaultIfEmpty()
                                        from reference_4 in _dbContext.SysOtherLists.Where(x => x.ID == p.SCHOOL_ID).DefaultIfEmpty()
                                        select new CertificateModel
                                        {
                                            // trình độ học vấn
                                            LevelName = reference_1.NAME,

                                            // trình độ chuyên môn
                                            LevelTrainName = reference_2.NAME,

                                            // hình thức đào tạo
                                            TypeTrainName = reference_3.NAME,

                                            // đơn vị đào tạo
                                            SchoolName = reference_4.NAME
                                        }).ToList();


                // nếu không có 10 bản ghi bằng cấp/chứng chỉ
                // thì thêm đối tượng vào trong list
                // tới 10 đối tượng thì ngưng

                // nếu BA bắt render 10 bản ghi
                // thì mở cmt code while() này ra

                //while (list_certificate.Count() < 10)
                //{
                //    list_certificate.Add( new CertificateModel() { LevelName = null, LevelTrainName = null, TypeTrainName = null, SchoolName = null });
                //}


                // gán 10 cái bản ghi
                // cho thuộc tính ListCertificate
                query1.ListCertificate = list_certificate;


                var get_id_unapprove = (from record_sub in _dbContext.SysOtherListTypes.Where(x => x.CODE == "STATUS")
                                        from record_main in _dbContext.SysOtherLists.Where(x => x.CODE == "TCPD" && x.TYPE_ID == record_sub.ID)
                                        select record_main.ID
                                        ).First();


                // lấy bản ghi ở bảng tạm
                // sau đó kết hợp vào bảng chính
                var query2 = await (
                                from data in _dbContext.HuEmployeeCvEdits
                                            .Where(x => x.EMPLOYEE_ID == employeeId
                                            && (x.IS_APPROVED_EDUCATION == false
                                                || x.IS_SAVE_EDUCATION == true
                                                || x.STATUS_APPROVED_EDUCATION_ID == get_id_unapprove)
                                            )
                                    //.DefaultIfEmpty()
                                from table in _dbContext.HuEmployees.Where(x => x.ID == data.EMPLOYEE_ID).DefaultIfEmpty()
                                    //from c in _dbContext.HuEmployeeCvs.Where(x => x.ID == table.PROFILE_ID).DefaultIfEmpty()
                                from e in _dbContext.SysOtherLists.Where(x => x.ID == data.EDUCATION_LEVEL_ID).DefaultIfEmpty()
                                from z in _dbContext.SysOtherLists.Where(x => x.ID == data.LEARNING_LEVEL_ID).DefaultIfEmpty()
                                from q1 in _dbContext.SysOtherLists.Where(x => x.ID == data.QUALIFICATIONID).DefaultIfEmpty()
                                from q2 in _dbContext.SysOtherLists.Where(x => x.ID == data.QUALIFICATIONID_2).DefaultIfEmpty()
                                from q3 in _dbContext.SysOtherLists.Where(x => x.ID == data.QUALIFICATIONID_3).DefaultIfEmpty()
                                from t in _dbContext.SysOtherLists.Where(x => x.ID == data.TRAINING_FORM_ID).DefaultIfEmpty()
                                from t2 in _dbContext.SysOtherLists.Where(x => x.ID == data.TRAINING_FORM_ID_2).DefaultIfEmpty()
                                from t3 in _dbContext.SysOtherLists.Where(x => x.ID == data.TRAINING_FORM_ID_3).DefaultIfEmpty()
                                from l1 in _dbContext.SysOtherLists.Where(x => x.ID == data.LANGUAGE_ID).DefaultIfEmpty()
                                from l2 in _dbContext.SysOtherLists.Where(x => x.ID == data.LANGUAGE_ID_2).DefaultIfEmpty()
                                from l3 in _dbContext.SysOtherLists.Where(x => x.ID == data.LANGUAGE_ID_3).DefaultIfEmpty()
                                from lv1 in _dbContext.SysOtherLists.Where(x => x.ID == data.LANGUAGE_LEVEL_ID).DefaultIfEmpty()
                                from lv2 in _dbContext.SysOtherLists.Where(x => x.ID == data.LANGUAGE_LEVEL_ID_2).DefaultIfEmpty()
                                from lv3 in _dbContext.SysOtherLists.Where(x => x.ID == data.LANGUAGE_LEVEL_ID_3).DefaultIfEmpty()
                                from s in _dbContext.SysOtherLists.Where(x => x.ID == data.SCHOOL_ID).DefaultIfEmpty()
                                from s2 in _dbContext.SysOtherLists.Where(x => x.ID == data.SCHOOL_ID_2).DefaultIfEmpty()
                                from s3 in _dbContext.SysOtherLists.Where(x => x.ID == data.SCHOOL_ID_3).DefaultIfEmpty()
                                from reference_1 in _dbContext.SysOtherLists.Where(x => x.ID == data.COMPUTER_SKILL_ID).DefaultIfEmpty()
                                from reference_2 in _dbContext.SysOtherLists.Where(x => x.ID == data.LICENSE_ID).DefaultIfEmpty()
                                select new HuEmployeeCvEditDTO
                                {
                                    // id của HuEmployeeCvEdits
                                    Id = data.ID,

                                    // id của HuEmployee
                                    EmployeeId = table.ID,

                                    // Trình độ văn hóa
                                    EducationLevelId = data.EDUCATION_LEVEL_ID,
                                    EducationLevel = e.NAME,

                                    // Trình độ học vấn
                                    LearningLevelId = data.LEARNING_LEVEL_ID,
                                    LearningLevel = z.NAME,

                                    // Trình độ tin học
                                    ComputerSkillId = data.COMPUTER_SKILL_ID,
                                    ComputerSkill = reference_1.NAME,

                                    // Bằng lái xe
                                    LicenseId = data.LICENSE_ID,
                                    License = reference_2.NAME,

                                    // Trình độ chuyên môn 1
                                    Qualificationid = data.QUALIFICATIONID,
                                    Qualification1 = q1.NAME,

                                    // Trình độ chuyên môn 2
                                    Qualificationid2 = data.QUALIFICATIONID_2,
                                    Qualification2 = q2.NAME,

                                    // Trình độ chuyên môn 3
                                    Qualificationid3 = data.QUALIFICATIONID_3,
                                    Qualification3 = q3.NAME,

                                    // Hình thức đào tạo 1
                                    TrainingFormId = data.TRAINING_FORM_ID,
                                    TrainingForm1 = t.NAME,

                                    // Hình thức đào tạo 2
                                    TrainingFormId2 = data.TRAINING_FORM_ID_2,
                                    TrainingForm2 = t2.NAME,

                                    // Hình thức đào tạo 3
                                    TrainingFormId3 = data.TRAINING_FORM_ID_3,
                                    TrainingForm3 = t3.NAME,

                                    // Trường học 1
                                    School1 = s.NAME,

                                    // Trường học 2
                                    School2 = s2.NAME,

                                    // Trường học 3
                                    School3 = s3.NAME,

                                    // Ngoại ngữ 1
                                    Language1 = l1.NAME,

                                    // Ngoại ngữ 2
                                    Language2 = l2.NAME,

                                    // Ngoại ngữ 3
                                    Language3 = l3.NAME,

                                    // Trình độ ngoại ngữ 1
                                    LanguageLevel1 = lv1.NAME,

                                    // Trình độ ngoại ngữ 2
                                    LanguageLevel2 = lv2.NAME,

                                    // Trình độ ngoại ngữ 3
                                    LanguageLevel3 = lv3.NAME,

                                    // trạng thái gửi phê duyệt (portal)
                                    IsSendPortal = data.IS_SEND_PORTAL,

                                    // trạng thái đã lưu
                                    IsSavePortal = data.IS_SAVE_PORTAL,

                                    HuEmployeeCvId = data.HU_EMPLOYEE_CV_ID,

                                    IsSaveEducation = data.IS_SAVE_EDUCATION,

                                    IsUnapprove = (data.STATUS_APPROVED_EDUCATION_ID == get_id_unapprove) ? true : false
                                }).ToListAsync();


                foreach (var item in query2)
                {
                    item.ListCertificate = list_certificate;
                }


                List<HuEmployeeCvEditDTO> ds = new List<HuEmployeeCvEditDTO>();


                ds.Add(query1);


                // nếu query2 khác null
                // thì thêm query2 vào danh sách
                if (query2 != null && query2.Count > 0)
                {
                    foreach (var item in query2)
                    {
                        ds.Add(item);
                    }
                }


                return new() { InnerBody = ds };
            }
            catch (Exception ex)
            {
                return new FormatedResponse() { MessageCode = CommonMessageCode.GET_METHOD_FAILED, ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400 };

            }
        }
        
        public async Task<FormatedResponse> GetEducationByPortalCorrect(long id)
        {
            try 
            {

                       var query = await (from data in _dbContext.HuEmployeeCvEdits.Where(x => x.ID == id).DefaultIfEmpty()
                       from e in _dbContext.SysOtherLists.Where(x => x.ID == data.EDUCATION_LEVEL_ID).DefaultIfEmpty()
                       from z in _dbContext.SysOtherLists.Where(x => x.ID == data.LEARNING_LEVEL_ID).DefaultIfEmpty()
                       from q1 in _dbContext.SysOtherLists.Where(x => x.ID == data.QUALIFICATIONID).DefaultIfEmpty()
                       from q2 in _dbContext.SysOtherLists.Where(x => x.ID == data.QUALIFICATIONID_2).DefaultIfEmpty()
                       from q3 in _dbContext.SysOtherLists.Where(x => x.ID == data.QUALIFICATIONID_3).DefaultIfEmpty()
                       from t in _dbContext.SysOtherLists.Where(x => x.ID == data.TRAINING_FORM_ID).DefaultIfEmpty()
                       from t2 in _dbContext.SysOtherLists.Where(x => x.ID == data.TRAINING_FORM_ID_2).DefaultIfEmpty()
                       from t3 in _dbContext.SysOtherLists.Where(x => x.ID == data.TRAINING_FORM_ID_3).DefaultIfEmpty()
                       from l1 in _dbContext.SysOtherLists.Where(x => x.ID == data.LANGUAGE_ID).DefaultIfEmpty()
                       from l2 in _dbContext.SysOtherLists.Where(x => x.ID == data.LANGUAGE_ID_2).DefaultIfEmpty()
                       from l3 in _dbContext.SysOtherLists.Where(x => x.ID == data.LANGUAGE_ID_3).DefaultIfEmpty()
                       from lv1 in _dbContext.SysOtherLists.Where(x => x.ID == data.LANGUAGE_LEVEL_ID).DefaultIfEmpty()
                       from lv2 in _dbContext.SysOtherLists.Where(x => x.ID == data.LANGUAGE_LEVEL_ID_2).DefaultIfEmpty()
                       from lv3 in _dbContext.SysOtherLists.Where(x => x.ID == data.LANGUAGE_LEVEL_ID_3).DefaultIfEmpty()
                       from s in _dbContext.SysOtherLists.Where(x => x.ID == data.SCHOOL_ID).DefaultIfEmpty()
                       from s2 in _dbContext.SysOtherLists.Where(x => x.ID == data.SCHOOL_ID_2).DefaultIfEmpty()
                       from s3 in _dbContext.SysOtherLists.Where(x => x.ID == data.SCHOOL_ID_3).DefaultIfEmpty()

                       select new HuEmployeeCvEditDTO
                       {
                           Id = data.ID,

                           EducationLevelId = data.EDUCATION_LEVEL_ID,
                           EducationLevel = e.NAME,

                           LearningLevelId = data.LEARNING_LEVEL_ID,
                           LearningLevel = z.NAME,

                           ComputerSkill = data.COMPUTER_SKILL,

                           License = data.LICENSE,

                           Qualificationid = data.QUALIFICATIONID,
                           Qualification1 = q1.NAME,

                           Qualificationid2 = data.QUALIFICATIONID_2,
                           Qualification2 = q2.NAME,

                           Qualificationid3 = data.QUALIFICATIONID_3,
                           Qualification3 = q3.NAME,

                           TrainingFormId = data.TRAINING_FORM_ID,
                           TrainingForm1 = t.NAME,

                           TrainingFormId2 = data.TRAINING_FORM_ID_2,
                           TrainingForm2 = t2.NAME,

                           TrainingFormId3 = data.TRAINING_FORM_ID_3,
                           TrainingForm3 = t3.NAME,

                           School1 = s.NAME,

                           School2 = s2.NAME,

                           School3 = s3.NAME,

                           Language1 = l1.NAME,

                           Language2 = l2.NAME,

                           Language3 = l3.NAME,

                           LanguageLevel1 = lv1.NAME,

                           LanguageLevel2 = lv2.NAME,

                           LanguageLevel3 = lv3.NAME,

                           //IsSendPortal = data.IS_SEND_PORTAL,

                           //IsSavePortal = data.IS_SAVE_PORTAL,

                           //HuEmployeeCvId = data.HU_EMPLOYEE_CV_ID,

                           //IsSaveEducation = data.IS_SAVE_EDUCATION
                       }).ToListAsync();
                return new() { InnerBody = query };
            } catch(Exception ex)
            {
                return new() { InnerBody = null };
            }
        }
        public async Task<FormatedResponse> GetCurriculumByPortal(long employeeId)
        {
            try
            {
                var query = await (from table in _dbContext.HuEmployees.Where(x => x.ID == employeeId).DefaultIfEmpty()
                                   from c in _dbContext.HuEmployeeCvs.Where(x => x.ID == table.PROFILE_ID).DefaultIfEmpty()
                                   from hp in _dbContext.HuProvinces.Where(x=> x.ID == c.TAX_CODE_ADDRESS).DefaultIfEmpty()
                                   from sys in _dbContext.SysOtherLists.Where(x => x.ID == c.NATIONALITY_ID).DefaultIfEmpty()
                                   from sys2 in _dbContext.SysOtherLists.Where(x => x.ID == c.NATIVE_ID).DefaultIfEmpty()
                                   from sys3 in _dbContext.SysOtherLists.Where(x => x.ID == c.RELIGION_ID).DefaultIfEmpty()
                                   from sys4 in _dbContext.SysOtherLists.Where(x => x.ID == c.MARITAL_STATUS_ID).DefaultIfEmpty()
                                   from p in _dbContext.HuProvinces.Where(x => x.ID == c.ID_PLACE).DefaultIfEmpty()
                                   select new
                                   {
                                       Id = table.ID,
                                       BirthDate = c.BIRTH_DATE,
                                       BirthRegisAddress = c.BIRTH_REGIS_ADDRESS,
                                       BirthPlace = c.BIRTH_PLACE,
                                       IdNo = c.ID_NO,
                                       IdDate = c.ID_DATE,
                                       IdPlaceName = p.NAME,
                                       IdentityAddress = c.ID_PLACE,
                                       Domicile = c.DOMICILE,
                                       TaxCode = c.TAX_CODE,
                                       TaxCodeDate = c.TAX_CODE_DATE,
                                       TaxCodeAddress = hp.NAME,
                                       Nationality = sys.NAME,
                                       NativeName = sys2.NAME,
                                       ReligionName = sys3.NAME,
                                       MaritalStatusName = sys4.NAME,
                                       PassNo = c.PASS_NO,
                                       PassDate = c.PASS_DATE,
                                       PassExpire = c.PASS_EXPIRE,
                                       PassPlace = c.PASS_PLACE,
                                   }).SingleAsync();
                return new() { InnerBody = query };
            }
            catch (Exception ex)
            {
                return new FormatedResponse() { MessageCode = CommonMessageCode.GET_METHOD_FAILED, ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400 };
            }
        }

        public async Task<FormatedResponse> UpdateCurriculumByPortal(HuEmployeeCvEditDTO request)
        {
            try
            {
                var employee = _dbContext.HuEmployees.Where(x => x.ID == request.EmployeeId).SingleOrDefault();
                HuEmployeeCvEditDTO query = new HuEmployeeCvEditDTO();
                List<string> listObjects = new List<string>();
                HU_EMPLOYEE_CV employeeCv = new HU_EMPLOYEE_CV();
                if (employee != null)
                {
                    if (employee.PROFILE_ID.HasValue)
                    {
                        employeeCv = _dbContext.HuEmployeeCvs.Where(x => x.ID == employee.PROFILE_ID).SingleOrDefault();
                    }
                    if (employeeCv != null)
                    {
                        HU_EMPLOYEE_CV_EDIT employeeCvEdit = new()
                        {
                            EMPLOYEE_ID = request.EmployeeId,
                            BIRTH_DATE = request.BirthDate == null ? employeeCv.BIRTH_DATE : request.BirthDate,
                            BIRTH_PLACE = request.BirthPlace == null ? employeeCv.BIRTH_PLACE : request.BirthPlace,
                            DOMICILE = request.Domicile == null ? employeeCv.DOMICILE : request.Domicile,
                            NATIONALITY_ID = request.NationalityId == null ? employeeCv.NATIONALITY_ID : request.NationalityId,
                            NATIVE_ID = request.NativeId == null ? employeeCv.NATIVE_ID : request.NativeId,
                            ID_NO = request.IdNo == null ? employeeCv.ID_NO : request.IdNo,
                            ID_DATE = request.IdDate == null ? employeeCv.ID_DATE : request.IdDate,
                            ID_PLACE = request.IdPlace == null ? employeeCv.ID_PLACE : request.IdPlace,
                            RELIGION_ID = request.ReligionId == null ? employeeCv.RELIGION_ID : request.ReligionId,
                            MARITAL_STATUS_ID = request.MaritalStatusId == null ? employeeCv.MARITAL_STATUS_ID : request.MaritalStatusId,
                            TAX_CODE = request.TaxCode == null ? employeeCv.TAX_CODE : request.TaxCode,
                            TAX_CODE_DATE = request.TaxCodeDate == null ? employeeCv.TAX_CODE_DATE : request.TaxCodeDate,
                            TAX_CODE_ADDRESS = request.TaxCodeAddress == null ? employeeCv.TAX_CODE_ADDRESS : request.TaxCodeAddress,
                            PASS_NO = request.PassNo == null ? employeeCv.PASS_NO : request.PassNo,
                            PASS_DATE = request.PassDate == null ? employeeCv.PASS_DATE : request.PassDate,
                            PASS_PLACE = request.PassPlace == null ? employeeCv.PASS_PLACE : request.PassPlace,
                            PASS_EXPIRE = request.PassExpire == null ? employeeCv.PASS_EXPIRE : request.PassExpire,
                            IS_SEND_PORTAL = true,
                            IS_APPROVED_PORTAL = false
                        };
                        await _dbContext.HuEmployeeCvEdits.AddAsync(employeeCvEdit);
                        await _dbContext.SaveChangesAsync();
                        query = CoreMapper<HuEmployeeCvEditDTO, HU_EMPLOYEE_CV_EDIT>.EntityToDto(employeeCvEdit, new HuEmployeeCvEditDTO());
                        if (query != null)
                        {
                            Type type = request.GetType();
                            if (request != null)
                            {
                                IList<PropertyInfo> props = new List<PropertyInfo>(type.GetProperties());

                                foreach (PropertyInfo prop in props)
                                {
                                    if (prop != null)
                                    {
                                        object propValue = prop.GetValue(request, null);
                                        if (propValue != null && prop.Name != "Id")
                                        {

                                            listObjects.Add(Char.ToLowerInvariant(prop.Name[0]) + prop.Name.Substring(1));
                                        }
                                    }


                                }
                                query.ModelChange = string.Join(";", listObjects);
                            }

                        }

                    }
                }

                return new() { InnerBody = query };
            }
            catch (Exception ex)
            {
                return new FormatedResponse() { MessageCode = ex.Message, ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400 };
            }
        }

        public async Task<FormatedResponse> GetProfileInfoByPortal(long employeeId)
        {
            try
            {
                var query = await (from table in _dbContext.HuEmployees.Where(x => x.ID == employeeId).DefaultIfEmpty()
                                   from c in _dbContext.HuEmployeeCvs.Where(x => x.ID == table.PROFILE_ID).DefaultIfEmpty()
                                   from p in _dbContext.HuPositions.Where(x => x.ID == table.POSITION_ID).DefaultIfEmpty()
                                   from o in _dbContext.HuOrganizations.Where(x => x.ID == table.ORG_ID).DefaultIfEmpty()
                                   from company in _dbContext.HuCompanys.Where(x => x.ID == o.COMPANY_ID).DefaultIfEmpty()
                                   from s in _dbContext.SysOtherLists.Where(x => x.ID == c.GENDER_ID).DefaultIfEmpty()
                                   from s2 in _dbContext.SysOtherLists.Where(x => x.ID == table.EMPLOYEE_OBJECT_ID).DefaultIfEmpty()
                                   from s3 in _dbContext.SysOtherLists.Where(x => x.ID == table.WORK_STATUS_ID).DefaultIfEmpty()
                                   from pc in _dbContext.HuPositions.Where(x => x.ID == p.LM).DefaultIfEmpty()
                                   from ec in _dbContext.HuEmployees.Where(x => x.ID == pc.MASTER).DefaultIfEmpty()

                                   from contract in _dbContext.HuContracts.Where(x => x.EMPLOYEE_ID == table.ID).OrderBy(x => x.START_DATE).Take(1).DefaultIfEmpty()
                                   from workingTime in _dbContext.HuContracts.Where(x => x.EMPLOYEE_ID == table.ID).OrderByDescending(x => x.START_DATE).Take(1).DefaultIfEmpty()
                                   from contractType in _dbContext.HuContractTypes.Where(x => x.ID == workingTime.CONTRACT_TYPE_ID).DefaultIfEmpty()

                                   select new
                                   {
                                       Id = table.ID,
                                       Avatar = c.AVATAR,
                                       Gender = s.NAME,
                                       WorkStatus = s3.NAME,
                                       EmployeeObject = s2.NAME,
                                       FullName = c.FULL_NAME,
                                       PositionName = p.NAME,
                                       EmployeeCode = table.CODE,
                                       OrgName = o.NAME,
                                       CompanyName = company.NAME_VN,
                                       CmsName = ec.Profile!.FULL_NAME,
                                       CmsJobName = pc.NAME,
                                       //CmsName = pce.Profile.FULL_NAME,
                                       //CmsJobName = pc.NAME,
                                       Address = company.WORK_ADDRESS,
                                       Seniority = (DateTime.Now.Year - contract.START_DATE.Value.Year) * 12 + DateTime.Now.Month - contract.START_DATE.Value.Month < 0 
                                                    ? 0 : (DateTime.Now.Year - contract.START_DATE.Value.Year) * 12 + DateTime.Now.Month - contract.START_DATE.Value.Month,
                                       WorkingTime = (DateTime.Now.Year - workingTime.START_DATE.Value.Year) * 12 + DateTime.Now.Month - workingTime.START_DATE.Value.Month < 0 
                                                      ? 0 : (DateTime.Now.Year - workingTime.START_DATE.Value.Year) * 12 + DateTime.Now.Month - workingTime.START_DATE.Value.Month,
                                       ContractNo = workingTime.CONTRACT_NO,
                                       ContractType = contractType.NAME,
                                       StartDate = workingTime.START_DATE,
                                       ExpireDate = workingTime.EXPIRE_DATE
                                   }).FirstOrDefaultAsync();
                return new() { InnerBody = query, StatusCode = EnumStatusCode.StatusCode200 };
            }
            catch (Exception ex)
            {
                return new FormatedResponse() { MessageCode = CommonMessageCode.GET_METHOD_FAILED, ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400 };
            }
        }

        public virtual async Task<FormatedResponse> ToggleActiveIds(GenericUnitOfWork _uow, List<long> ids, bool valueToBind, string sid)
        {
            await Task.Run(() => null);
            throw new NotImplementedException();
        }

        //CV
        public async Task<FormatedResponse> GetHuEmployeeCvEditCvApproving(long employeeId)
        {
            try
            {
                var query = await (from table in _dbContext.HuEmployeeCvEdits.AsNoTracking().AsQueryable()
                                   from e in _dbContext.HuEmployees.Where(x => x.ID == table.EMPLOYEE_ID).DefaultIfEmpty()
                                   from c in _dbContext.HuEmployeeCvs.Where(x => x.ID == e.PROFILE_ID).DefaultIfEmpty()
                                   from hp in _dbContext.HuProvinces.Where(x => x.ID == c.TAX_CODE_ADDRESS).DefaultIfEmpty()
                                   from sys in _dbContext.SysOtherLists.Where(x => x.ID == c.NATIONALITY_ID).DefaultIfEmpty()
                                   from sys2 in _dbContext.SysOtherLists.Where(x => x.ID == c.NATIVE_ID).DefaultIfEmpty()
                                   from sys3 in _dbContext.SysOtherLists.Where(x => x.ID == table.RELIGION_ID).DefaultIfEmpty()
                                   from sys4 in _dbContext.SysOtherLists.Where(x => x.ID == table.MARITAL_STATUS_ID).DefaultIfEmpty()
                                   from p in _dbContext.HuProvinces.Where(x => x.ID == table.IDENTITY_ADDRESS).DefaultIfEmpty()
                                   where table.IS_SEND_PORTAL_CV == true && table.EMPLOYEE_ID == employeeId && table.IS_APPROVED_CV == false
                                   select new
                                   {
                                       Id = table.ID,
                                       BirthResgisAddress = c.BIRTH_REGIS_ADDRESS,
                                       BirthDate = c.BIRTH_DATE,
                                       BirthPlace = c.BIRTH_PLACE,
                                       Domicele = c.DOMICILE,
                                       NationName = sys.NAME,
                                       NativeName = sys2.NAME,
                                       RelegionName = sys3.NAME,
                                       MaritalStatusName = sys4.NAME,
                                       IdNo = table.ID_NO,
                                       IdDate = table.ID_DATE,
                                       IdPlace = table.ID_PLACE,
                                       TaxCode = c.TAX_CODE,
                                       TaxDate = c.TAX_CODE_DATE,
                                       TaxPlace = hp.NAME,
                                       PassNo = c.PASS_NO,
                                       PassPlace = c.PASS_PLACE,
                                       PassDate = c.PASS_DATE,
                                       PassExpireDate = c.PASS_EXPIRE,
                                       IdentityAddressName = p.NAME


                                   }).ToListAsync();
                return new() { InnerBody = query };
            }
            catch (Exception ex)
            {

                return new FormatedResponse() { StatusCode = EnumStatusCode.StatusCode500, ErrorType = EnumErrorType.UNCATCHABLE, MessageCode = ex.Message };
            }
        }

        public async Task<FormatedResponse> GetHuEmployeeCvEditById(long id)
        {
            var response = await (from ee in _uow.Context.Set<HU_EMPLOYEE_CV>()
                                  from e in _uow.Context.Set<HU_EMPLOYEE>().Where(x => x.PROFILE_ID == ee.ID)
                                  where e.ID == id
                                  select new
                                  {
                                      Id = ee.ID,
                                      ReligionId = ee.RELIGION_ID,
                                      NationalityId = ee.NATIONALITY_ID,
                                      NativeId = ee.NATIVE_ID,
                                      MaritalStatusId = ee.MARITAL_STATUS_ID,
                                      IdNo = ee.ID_NO,
                                      IdDate = ee.ID_DATE,
                                      IdPlace = ee.ID_PLACE,
                                      IdentityAddress = ee.ID_PLACE

                                  }).FirstOrDefaultAsync();
            return new FormatedResponse() { InnerBody = response };
        }

        public async Task<FormatedResponse> GetHuEmployeeCvEditCorrectById(long id)
        {
            var response = await (from ee in _uow.Context.Set<HU_EMPLOYEE_CV_EDIT>()
                                  from sys in _dbContext.SysOtherLists.Where(x => x.ID == ee.NATIONALITY_ID).DefaultIfEmpty()
                                  from sys2 in _dbContext.SysOtherLists.Where(x => x.ID == ee.NATIVE_ID).DefaultIfEmpty()
                                  from sys3 in _dbContext.SysOtherLists.Where(x => x.ID == ee.RELIGION_ID).DefaultIfEmpty()
                                  from sys4 in _dbContext.SysOtherLists.Where(x => x.ID == ee.MARITAL_STATUS_ID).DefaultIfEmpty()
                                  from p in _dbContext.HuProvinces.Where(x => x.ID == ee.IDENTITY_ADDRESS).DefaultIfEmpty()
                                  where ee.ID == id
                                  select new
                                  {
                                      Id = ee.ID,
                                      BirthRegisAddress = ee.BIRTH_REGIS_ADDRESS,
                                      BirthDate = ee.BIRTH_DATE,
                                      BirthPlace = ee.BIRTH_PLACE,
                                      Domicele = ee.DOMICILE,
                                      NationName = sys.NAME,
                                      NativeName = sys2.NAME,
                                      RelegionName = sys3.NAME,
                                      MaritalStatusName = sys4.NAME,
                                      IdNo = ee.ID_NO,
                                      IdDate = ee.ID_DATE,
                                      IdPlace = ee.ID_PLACE,
                                      TaxCode = ee.TAX_CODE,
                                      TaxDate = ee.TAX_CODE_DATE,
                                      TaxPlace = ee.TAX_CODE_ADDRESS,
                                      PassNo = ee.PASS_NO,
                                      PassPlace = ee.PASS_PLACE,
                                      PassDate = ee.PASS_DATE,
                                      PassExpireDate = ee.PASS_EXPIRE,
                                      IdentityAddressName = p.NAME
                                  }).FirstOrDefaultAsync();
            return new FormatedResponse() { InnerBody = response };
        }
        public async Task<FormatedResponse> GetHuEmployeeCvSave(long employeeId)
        {
            try
            {
                var query = await (from table in _dbContext.HuEmployeeCvEdits.AsNoTracking().AsQueryable()
                                   from e in _dbContext.HuEmployees.Where(x => x.ID == table.EMPLOYEE_ID).DefaultIfEmpty()
                                   from c in _dbContext.HuEmployeeCvs.Where(x => x.ID == e.PROFILE_ID).DefaultIfEmpty()
                                   from hp in _dbContext.HuProvinces.Where(x => x.ID == c.TAX_CODE_ADDRESS).DefaultIfEmpty()
                                   from sys in _dbContext.SysOtherLists.Where(x => x.ID == c.NATIONALITY_ID).DefaultIfEmpty()
                                   from sys2 in _dbContext.SysOtherLists.Where(x => x.ID == table.NATIVE_ID).DefaultIfEmpty()
                                   from sys3 in _dbContext.SysOtherLists.Where(x => x.ID == table.RELIGION_ID).DefaultIfEmpty()
                                   from sys4 in _dbContext.SysOtherLists.Where(x => x.ID == table.MARITAL_STATUS_ID).DefaultIfEmpty()
                                   from p in _dbContext.HuProvinces.Where(x => x.ID == table.IDENTITY_ADDRESS).DefaultIfEmpty()
                                   where table.EMPLOYEE_ID == employeeId && table.IS_SAVE_CV == true
                                   select new
                                   {
                                       Id = table.ID,
                                       BirthResgisAddress = c.BIRTH_REGIS_ADDRESS,
                                       BirthDate = c.BIRTH_DATE,
                                       BirthPlace = c.BIRTH_PLACE,
                                       Domicele = c.DOMICILE,
                                       NationName = sys.NAME,
                                       NativeName = sys2.NAME,
                                       RelegionName = sys3.NAME,
                                       MaritalStatusName = sys4.NAME,
                                       IdNo = table.ID_NO,
                                       IdDate = table.ID_DATE,
                                       IdPlace = table.ID_PLACE,
                                       TaxCode = c.TAX_CODE,
                                       TaxDate = c.TAX_CODE_DATE,
                                       TaxPlace = hp.NAME,
                                       PassNo = c.PASS_NO,
                                       PassPlace = c.PASS_PLACE,
                                       PassDate = c.PASS_DATE,
                                       PassExpireDate = c.PASS_EXPIRE,
                                       IdentityAddressName = p.NAME


                                   }).ToListAsync();
                return new() { InnerBody = query };
            }
            catch (Exception ex)
            {

                return new FormatedResponse() { StatusCode = EnumStatusCode.StatusCode400, ErrorType = EnumErrorType.UNCATCHABLE, MessageCode = ex.Message };
            }
        }

        public async Task<FormatedResponse> GetHuEmployeeCvEditCvSaveById(long id)
        {
            try
            {
                var response = await (from e in _uow.Context.Set<HU_EMPLOYEE_CV_EDIT>().Where(x => x.ID == id)
                                      select new HuEmployeeCvEditDTO()
                                      {
                                          Id = e.ID,
                                          ReligionId = e.RELIGION_ID,
                                          NationalityId = e.NATIONALITY_ID,
                                          NativeId = e.NATIVE_ID,
                                          MaritalStatusId = e.MARITAL_STATUS_ID,
                                          IdNo = e.ID_NO,
                                          IdDate = e.ID_DATE,
                                          IsSaveCv = e.IS_SAVE_CV,
                                          IdExpireDate = e.ID_EXPIRE_DATE,
                                          IdentityAddress = e.IDENTITY_ADDRESS,
                                          EmployeeId = e.EMPLOYEE_ID

                                      }).FirstOrDefaultAsync();
                return new FormatedResponse() { InnerBody = response };
            }
            catch (Exception ex)
            {

                return new FormatedResponse() { StatusCode = EnumStatusCode.StatusCode400, ErrorType = EnumErrorType.UNCATCHABLE, MessageCode = ex.Message };
            }
        }

        public async Task<FormatedResponse> GetHuEmployeeCvUnapprove(long employeeId)
        {
            try
            {
                var query = await (from table in _dbContext.HuEmployeeCvEdits.AsNoTracking().AsQueryable()
                                   from e in _dbContext.HuEmployees.Where(x => x.ID == table.EMPLOYEE_ID).DefaultIfEmpty()
                                   from c in _dbContext.HuEmployeeCvs.Where(x => x.ID == e.PROFILE_ID).DefaultIfEmpty()
                                   from hp in _dbContext.HuProvinces.Where(x => x.ID == c.TAX_CODE_ADDRESS).DefaultIfEmpty()
                                   from sys in _dbContext.SysOtherLists.Where(x => x.ID == table.NATIONALITY_ID).DefaultIfEmpty()
                                   from sys2 in _dbContext.SysOtherLists.Where(x => x.ID == table.NATIVE_ID).DefaultIfEmpty()
                                   from sys3 in _dbContext.SysOtherLists.Where(x => x.ID == table.RELIGION_ID).DefaultIfEmpty()
                                   from sys4 in _dbContext.SysOtherLists.Where(x => x.ID == table.MARITAL_STATUS_ID).DefaultIfEmpty()
                                   from p in _dbContext.HuProvinces.Where(x => x.ID == table.IDENTITY_ADDRESS)
                                   where table.EMPLOYEE_ID == employeeId && table.IS_APPROVED_CV == false && table.IS_SEND_PORTAL_CV == false && table.IS_SAVE_CV == false
                                   select new
                                   {
                                       Id = table.ID,
                                       BirthResgisAddress = c.BIRTH_REGIS_ADDRESS,
                                       BirthDate = c.BIRTH_DATE,
                                       BirthPlace = c.BIRTH_PLACE,
                                       Domicele = c.DOMICILE,
                                       NationName = sys.NAME,
                                       NativeName = sys2.NAME,
                                       RelegionName = sys3.NAME,
                                       MaritalStatusName = sys4.NAME,
                                       IdNo = c.ID_NO,
                                       IdDate = table.ID_DATE,
                                       IdPlace = table.ID_PLACE,
                                       IdPlaceName = p.NAME,
                                       TaxCode = c.TAX_CODE,
                                       TaxDate = c.TAX_CODE_DATE,
                                       TaxPlace = hp.NAME,
                                       PassNo = c.PASS_NO,
                                       PassPlace = c.PASS_PLACE,
                                       PassDate = c.PASS_DATE,
                                       PassExpireDate = c.PASS_EXPIRE,
                                       Reason = table.REASON,
                                   }).ToListAsync();
                return new() { InnerBody = query };
            }
            catch (Exception ex)
            {

                return new FormatedResponse() { StatusCode = EnumStatusCode.StatusCode500, ErrorType = EnumErrorType.UNCATCHABLE, MessageCode = ex.Message };
            }
        }

        public async Task<FormatedResponse> GetHuEmployeeCvUnapproveById(long id)
        {
            try
            {
                var response = await (from e in _uow.Context.Set<HU_EMPLOYEE_CV_EDIT>().Where(x => x.ID == id)
                                      select new HuEmployeeCvEditDTO()
                                      {
                                          Id = e.ID,
                                          ReligionId = e.RELIGION_ID,
                                          NationalityId = e.NATIONALITY_ID,
                                          NativeId = e.NATIVE_ID,
                                          MaritalStatusId = e.MARITAL_STATUS_ID,
                                          IdNo = e.ID_NO,
                                          IdDate = e.ID_DATE,
                                          IsSaveCv = e.IS_SAVE_CV,
                                          IdExpireDate = e.ID_EXPIRE_DATE,
                                          IdentityAddress = e.IDENTITY_ADDRESS,
                                          EmployeeId = e.EMPLOYEE_ID

                                      }).FirstOrDefaultAsync();
                return new FormatedResponse() { InnerBody = response };
            }
            catch (Exception ex)
            {

                return new FormatedResponse() { StatusCode = EnumStatusCode.StatusCode500, ErrorType = EnumErrorType.UNCATCHABLE, MessageCode = ex.Message };
            }
        }

        //BANK-INFO

        public async Task<FormatedResponse> GetBankInfoByEmployeeId(long employeeId)
        {
            try
            {
                var response = await (from cv in _uow.Context.Set<HU_EMPLOYEE_CV>()
                                      from e in _uow.Context.Set<HU_EMPLOYEE>().Where(x => x.PROFILE_ID == cv.ID).DefaultIfEmpty()
                                      from b1 in _uow.Context.Set<HU_BANK>().Where(x => x.ID == cv.BANK_ID).DefaultIfEmpty()
                                      from br1 in _uow.Context.Set<HU_BANK_BRANCH>().Where(x => x.ID == cv.BANK_BRANCH).DefaultIfEmpty()
                                      from b2 in _uow.Context.Set<HU_BANK>().Where(x => x.ID == cv.BANK_ID_2).DefaultIfEmpty()
                                      from br2 in _uow.Context.Set<HU_BANK_BRANCH>().Where(x => x.ID == cv.BANK_BRANCH_2).DefaultIfEmpty()
                                      where e.ID == employeeId
                                      select new HuEmployeeCvDTO()
                                      {
                                          Id = cv.ID,
                                          TaxCode = cv.TAX_CODE,
                                          FullName = RemoveDiacritics(cv.FULL_NAME).ToUpper(),
                                          BankNo = cv.BANK_NO,
                                          BankId = cv.BANK_ID,
                                          BankId2 = cv.BANK_ID_2,
                                          BankName = b1.NAME,
                                          BankBranchId = cv.BANK_BRANCH,
                                          BankBranchId2 = cv.BANK_BRANCH_2,
                                          BankBranchName = br1.NAME,
                                          BankNo2 = cv.BANK_NO_2,
                                          BankName2 = b2.NAME,
                                          BankBranchName2 = br2.NAME,
                                          EmployeeId = e.ID,
                                      }).SingleAsync();
                return new FormatedResponse() { InnerBody = response };
            }
            catch (Exception ex)
            {

                return new FormatedResponse() { StatusCode = EnumStatusCode.StatusCode500, ErrorType = EnumErrorType.UNCATCHABLE, MessageCode = ex.Message };
            }
        }

        public async Task<FormatedResponse> GetHuEmployeeCvEditBankInfoApproving(long employeeId)
        {
            try
            {
                var response = await (from cv in _uow.Context.Set<HU_EMPLOYEE_CV_EDIT>()
                                      from e in _uow.Context.Set<HU_EMPLOYEE>().Where(x => x.PROFILE_ID == cv.ID).DefaultIfEmpty()
                                      from b1 in _uow.Context.Set<HU_BANK>().Where(x => x.ID == cv.BANK_ID).DefaultIfEmpty()
                                      from br1 in _uow.Context.Set<HU_BANK_BRANCH>().Where(x => x.ID == cv.BANK_BRANCH_ID).DefaultIfEmpty()
                                      from b2 in _uow.Context.Set<HU_BANK>().Where(x => x.ID == cv.BANK_ID_2).DefaultIfEmpty()
                                      from br2 in _uow.Context.Set<HU_BANK_BRANCH>().Where(x => x.ID == cv.BANK_BRANCH_ID_2).DefaultIfEmpty()
                                      where cv.EMPLOYEE_ID == employeeId && cv.IS_APPROVED_BANK_INFO == false && cv.IS_SEND_PORTAL_BANK_INFO == true    
                                      select new HuEmployeeCvEditDTO()
                                      {
                                          Id = cv.ID,
                                          TaxCode = cv.TAX_CODE,
                                          FullName = RemoveDiacritics(cv.FULL_NAME).ToUpper(),
                                          BankId = cv.BANK_ID,

                                          BankNo = cv.BANK_NO,
                                          BankName = b1.NAME,
                                          BankBranchName = br1.NAME,
                                          BankNo2 = cv.BANK_NO_2,
                                          BankName2 = b2.NAME,
                                          BankBranchName2 = br2.NAME,
                                      }).ToListAsync();
                return new FormatedResponse() { InnerBody = response };
            }
            catch (Exception ex)
            {

                return new FormatedResponse() { StatusCode = EnumStatusCode.StatusCode500, ErrorType = EnumErrorType.UNCATCHABLE, MessageCode = ex.Message };
            }
        }

        public async Task<FormatedResponse> GetHuEmployeeCvEditBankInfoSave(long employeeId)
        {
            try
            {
                var response = await (from cv in _uow.Context.Set<HU_EMPLOYEE_CV_EDIT>()
                                      from e in _uow.Context.Set<HU_EMPLOYEE>().Where(x => x.PROFILE_ID == cv.ID).DefaultIfEmpty()
                                      from b1 in _uow.Context.Set<HU_BANK>().Where(x => x.ID == cv.BANK_ID).DefaultIfEmpty()
                                      from br1 in _uow.Context.Set<HU_BANK_BRANCH>().Where(x => x.ID == cv.BANK_BRANCH_ID).DefaultIfEmpty()
                                      from b2 in _uow.Context.Set<HU_BANK>().Where(x => x.ID == cv.BANK_ID_2).DefaultIfEmpty()
                                      from br2 in _uow.Context.Set<HU_BANK_BRANCH>().Where(x => x.ID == cv.BANK_BRANCH_ID_2).DefaultIfEmpty()
                                      where cv.EMPLOYEE_ID == employeeId && cv.IS_SAVE_BANK_INFO == true
                                      select new HuEmployeeCvEditDTO()
                                      {
                                          Id = cv.ID,
                                          TaxCode = cv.TAX_CODE,
                                          FullName = RemoveDiacritics(cv.FULL_NAME).ToUpper(),
                                          BankId = cv.BANK_ID,
                                          BankNo = cv.BANK_NO,
                                          BankName = b1.NAME,
                                          BankBranchName = br1.NAME,
                                          BankNo2 = cv.BANK_NO_2,
                                          BankName2 = b2.NAME,
                                          BankBranchName2 = br2.NAME,
                                      }).ToListAsync();
                return new FormatedResponse() { InnerBody = response };
            }
            catch (Exception ex)
            {

                return new FormatedResponse() { StatusCode = EnumStatusCode.StatusCode500, ErrorType = EnumErrorType.UNCATCHABLE, MessageCode = ex.Message };
            }
        }

        public async Task<FormatedResponse> GetHuEmployeeCvEditBankInfoById(long id)
        {
            try
            {
                var response = await (from cv in _uow.Context.Set<HU_EMPLOYEE_CV_EDIT>()
                                      where cv.ID == id
                                      select new HuEmployeeCvEditDTO()
                                      {
                                          Id = cv.ID,
                                          TaxCode = cv.TAX_CODE,
                                          FullName = RemoveDiacritics(cv.FULL_NAME).ToUpper(),
                                          BankId = cv.BANK_ID,
                                          BankNo = cv.BANK_NO,
                                          BankBranchId = cv.BANK_BRANCH_ID,
                                          BankNo2 = cv.BANK_NO_2,
                                          BankId2 = cv.BANK_ID_2,
                                          BankBranchId2 = cv.BANK_BRANCH_ID_2,
                                          IsSaveBankInfo = cv.IS_SAVE_BANK_INFO,
                                          EmployeeId = cv.EMPLOYEE_ID,
                                      }).SingleAsync();
                return new FormatedResponse() { InnerBody = response };
            }
            catch (Exception ex)
            {

                return new FormatedResponse() { StatusCode = EnumStatusCode.StatusCode500, ErrorType = EnumErrorType.UNCATCHABLE, MessageCode = ex.Message };
            }
        }

        public async Task<FormatedResponse> GetHuEmployeeCvEditBankInfoUnapprove(long employeeId)
        {
            try
            {
                var response = await(from cv in _uow.Context.Set<HU_EMPLOYEE_CV_EDIT>()
                                     from e in _uow.Context.Set<HU_EMPLOYEE>().Where(x => x.PROFILE_ID == cv.ID).DefaultIfEmpty()
                                     from b1 in _uow.Context.Set<HU_BANK>().Where(x => x.ID == cv.BANK_ID).DefaultIfEmpty()
                                     from br1 in _uow.Context.Set<HU_BANK_BRANCH>().Where(x => x.ID == cv.BANK_BRANCH_ID).DefaultIfEmpty()
                                     from b2 in _uow.Context.Set<HU_BANK>().Where(x => x.ID == cv.BANK_ID_2).DefaultIfEmpty()
                                     from br2 in _uow.Context.Set<HU_BANK_BRANCH>().Where(x => x.ID == cv.BANK_BRANCH_ID_2).DefaultIfEmpty()
                                     where cv.EMPLOYEE_ID == employeeId && cv.IS_APPROVED_BANK_INFO == true && cv.IS_SEND_PORTAL_BANK_INFO == false
                                     select new HuEmployeeCvEditDTO()
                                     {
                                         Id = cv.ID,
                                         TaxCode = cv.TAX_CODE,
                                         FullName = RemoveDiacritics(cv.FULL_NAME).ToUpper(),
                                         BankId = cv.BANK_ID,
                                         BankNo = cv.BANK_NO,
                                         BankName = b1.NAME,
                                         BankBranchName = br1.NAME,
                                         BankNo2 = cv.BANK_NO_2,
                                         BankName2 = b2.NAME,
                                         BankBranchName2 = br2.NAME,
                                         Reason = cv.REASON
                                     }).ToListAsync();
                return new FormatedResponse() { InnerBody = response };
            }
            catch (Exception ex)
            {

                return new FormatedResponse() { StatusCode = EnumStatusCode.StatusCode500, ErrorType = EnumErrorType.UNCATCHABLE, MessageCode = ex.Message };
            }
        }

        public async Task<FormatedResponse> GetHuEmployeeCvEditBankInfoUnapproveById(long id)
        {
            try
            {
                var response = await(from cv in _uow.Context.Set<HU_EMPLOYEE_CV_EDIT>()
                                     where cv.ID == id
                                     select new HuEmployeeCvEditDTO()
                                     {
                                         Id = cv.ID,
                                         TaxCode = cv.TAX_CODE,
                                         FullName = RemoveDiacritics(cv.FULL_NAME).ToUpper(),
                                         BankId = cv.BANK_ID,
                                         BankNo = cv.BANK_NO,
                                         BankBranchId = cv.BANK_BRANCH_ID,
                                         BankNo2 = cv.BANK_NO_2,
                                         BankId2 = cv.BANK_ID_2,
                                         BankBranchId2 = cv.BANK_BRANCH_ID_2,
                                         IsSaveBankInfo = cv.IS_SAVE_BANK_INFO,
                                     }).SingleAsync();
                return new FormatedResponse() { InnerBody = response };
            }
            catch (Exception ex)
            {

                return new FormatedResponse() { StatusCode = EnumStatusCode.StatusCode500, ErrorType = EnumErrorType.UNCATCHABLE, MessageCode = ex.Message };
            }
        }
        //CONTACT
        public async Task<FormatedResponse> GetContactByEmployeeId(long employeeId)
        {
            var response = await (from cv in _uow.Context.Set<HU_EMPLOYEE_CV>()
                                  from e in _uow.Context.Set<HU_EMPLOYEE>().Where(x => x.PROFILE_ID == cv.ID).DefaultIfEmpty()
                                  from p in _uow.Context.Set<HU_PROVINCE>().Where(x => x.ID == cv.PROVINCE_ID).DefaultIfEmpty()
                                  from d in _uow.Context.Set<HU_DISTRICT>().Where(x => x.ID == cv.DISTRICT_ID).DefaultIfEmpty()
                                  from w in _uow.Context.Set<HU_WARD>().Where(x => x.ID == cv.WARD_ID).DefaultIfEmpty()
                                  from pc in _uow.Context.Set<HU_PROVINCE>().Where(x => x.ID == cv.CUR_PROVINCE_ID).DefaultIfEmpty()
                                  from dc in _uow.Context.Set<HU_DISTRICT>().Where(x => x.ID == cv.CUR_DISTRICT_ID).DefaultIfEmpty()
                                  from wc in _uow.Context.Set<HU_WARD>().Where(x => x.ID == cv.CUR_WARD_ID).DefaultIfEmpty()
                                  where e.ID == employeeId
                                  select new HuEmployeeCvDTO()
                                  {
                                      Id = cv.ID,
                                      Address = cv.ADDRESS,
                                      ProvinceId = cv.PROVINCE_ID,
                                      ProvinceName = p.NAME,
                                      DistrictId = cv.DISTRICT_ID,
                                      DistrictName = d.NAME,
                                      WardId = cv.WARD_ID,
                                      WardName = w.NAME,
                                      CurAddress = cv.CUR_ADDRESS,
                                      CurProvinceId = cv.CUR_PROVINCE_ID,
                                      CurProvinceName = pc.NAME,
                                      CurDistrictId = cv.CUR_DISTRICT_ID,
                                      CurDistrictName = dc.NAME,
                                      CurWardId = cv.CUR_WARD_ID,
                                      CurWardName = wc.NAME,
                                      Email = cv.EMAIL,
                                      MobilePhoneLand = cv.MOBILE_PHONE_LAND,
                                      MobilePhone = cv.MOBILE_PHONE,
                                      EmployeeId = e.ID
                                  }).SingleAsync();

            return new FormatedResponse() { InnerBody = response };
        }

        public async Task<FormatedResponse> GetHuEmployeeCvEditContactApproving(long employeeId)
        {
            try
            {
                var response = await (from cv in _uow.Context.Set<HU_EMPLOYEE_CV_EDIT>()
                                      from e in _uow.Context.Set<HU_EMPLOYEE>().Where(x => x.PROFILE_ID == cv.ID).DefaultIfEmpty()
                                      from p in _uow.Context.Set<HU_PROVINCE>().Where(x => x.ID == cv.PROVINCE_ID).DefaultIfEmpty()
                                      from d in _uow.Context.Set<HU_DISTRICT>().Where(x => x.ID == cv.DISTRICT_ID).DefaultIfEmpty()
                                      from w in _uow.Context.Set<HU_WARD>().Where(x => x.ID == cv.WARD_ID).DefaultIfEmpty()
                                      from pc in _uow.Context.Set<HU_PROVINCE>().Where(x => x.ID == cv.CUR_PROVINCE_ID).DefaultIfEmpty()
                                      from dc in _uow.Context.Set<HU_DISTRICT>().Where(x => x.ID == cv.CUR_DISTRICT_ID).DefaultIfEmpty()
                                      from wc in _uow.Context.Set<HU_WARD>().Where(x => x.ID == cv.CUR_WARD_ID).DefaultIfEmpty()
                                      where cv.EMPLOYEE_ID == employeeId && cv.IS_APPROVED_CONTACT == false && cv.IS_SEND_PORTAL_CONTACT == true
                                      select new HuEmployeeCvEditDTO()
                                      {
                                          Id = cv.ID,
                                          Address = cv.ADDRESS,
                                          ProvinceId = cv.PROVINCE_ID,
                                          ProvinceName = p.NAME,
                                          DistrictId = cv.DISTRICT_ID,
                                          DistrictName = d.NAME,
                                          WardId = cv.WARD_ID,
                                          WardName = w.NAME,
                                          CurAddress = cv.CUR_ADDRESS,
                                          CurProvinceId = cv.CUR_PROVINCE_ID,
                                          CurProvinceName = pc.NAME,
                                          CurDistrictId = cv.CUR_DISTRICT_ID,
                                          CurDistrictName = dc.NAME,
                                          CurWardId = cv.CUR_WARD_ID,
                                          CurWardName = wc.NAME,
                                          Email = cv.EMAIL,
                                          MobilePhoneLand = cv.MOBILE_PHONE_LAND,
                                          MobilePhone = cv.MOBILE_PHONE,
                                      }).ToListAsync();

                return new FormatedResponse() { InnerBody = response };
            }
            catch (Exception ex)
            {

                return new FormatedResponse() { StatusCode = EnumStatusCode.StatusCode500, ErrorType = EnumErrorType.UNCATCHABLE, MessageCode = ex.Message };
            }
        }
        
        public async Task<FormatedResponse> GetHuEmployeeCvEditContactCorrect(long id)
        {
            try
            {
                var response = await (from cv in _uow.Context.Set<HU_EMPLOYEE_CV_EDIT>()
                                      from p in _uow.Context.Set<HU_PROVINCE>().Where(x => x.ID == cv.PROVINCE_ID).DefaultIfEmpty()
                                      from d in _uow.Context.Set<HU_DISTRICT>().Where(x => x.ID == cv.DISTRICT_ID).DefaultIfEmpty()
                                      from w in _uow.Context.Set<HU_WARD>().Where(x => x.ID == cv.WARD_ID).DefaultIfEmpty()
                                      from pc in _uow.Context.Set<HU_PROVINCE>().Where(x => x.ID == cv.CUR_PROVINCE_ID).DefaultIfEmpty()
                                      from dc in _uow.Context.Set<HU_DISTRICT>().Where(x => x.ID == cv.CUR_DISTRICT_ID).DefaultIfEmpty()
                                      from wc in _uow.Context.Set<HU_WARD>().Where(x => x.ID == cv.CUR_WARD_ID).DefaultIfEmpty()
                                      where cv.ID == id
                                      select new HuEmployeeCvEditDTO()
                                      {
                                          Id = cv.ID,
                                          Address = cv.ADDRESS,
                                          ProvinceId = cv.PROVINCE_ID,
                                          ProvinceName = p.NAME,
                                          DistrictId = cv.DISTRICT_ID,
                                          DistrictName = d.NAME,
                                          WardId = cv.WARD_ID,
                                          WardName = w.NAME,
                                          CurAddress = cv.CUR_ADDRESS,
                                          CurProvinceId = cv.CUR_PROVINCE_ID,
                                          CurProvinceName = pc.NAME,
                                          CurDistrictId = cv.CUR_DISTRICT_ID,
                                          CurDistrictName = dc.NAME,
                                          CurWardId = cv.CUR_WARD_ID,
                                          CurWardName = wc.NAME,
                                          Email = cv.EMAIL,
                                          MobilePhoneLand = cv.MOBILE_PHONE_LAND,
                                          MobilePhone = cv.MOBILE_PHONE,
                                      }).FirstOrDefaultAsync();

                return new FormatedResponse() { InnerBody = response };
            }
            catch (Exception ex)
            {

                return new FormatedResponse() { StatusCode = EnumStatusCode.StatusCode500, ErrorType = EnumErrorType.UNCATCHABLE, MessageCode = ex.Message };
            }
        }

        public async Task<FormatedResponse> GetHuEmployeeCvEditContactSave(long employeeId)
        {
            try
            {
                var response = await (from cv in _uow.Context.Set<HU_EMPLOYEE_CV_EDIT>()
                                      from e in _uow.Context.Set<HU_EMPLOYEE>().Where(x => x.PROFILE_ID == cv.ID).DefaultIfEmpty()
                                      from p in _uow.Context.Set<HU_PROVINCE>().Where(x => x.ID == cv.PROVINCE_ID).DefaultIfEmpty()
                                      from d in _uow.Context.Set<HU_DISTRICT>().Where(x => x.ID == cv.DISTRICT_ID).DefaultIfEmpty()
                                      from w in _uow.Context.Set<HU_WARD>().Where(x => x.ID == cv.WARD_ID).DefaultIfEmpty()
                                      from pc in _uow.Context.Set<HU_PROVINCE>().Where(x => x.ID == cv.CUR_PROVINCE_ID).DefaultIfEmpty()
                                      from dc in _uow.Context.Set<HU_DISTRICT>().Where(x => x.ID == cv.CUR_DISTRICT_ID).DefaultIfEmpty()
                                      from wc in _uow.Context.Set<HU_WARD>().Where(x => x.ID == cv.CUR_WARD_ID).DefaultIfEmpty()
                                      where cv.EMPLOYEE_ID == employeeId && cv.IS_SAVE_CONTACT == true
                                      select new HuEmployeeCvEditDTO()
                                      {
                                          Id = cv.ID,
                                          Address = cv.ADDRESS,
                                          ProvinceId = cv.PROVINCE_ID,
                                          ProvinceName = p.NAME,
                                          DistrictId = cv.DISTRICT_ID,
                                          DistrictName = d.NAME,
                                          WardId = cv.WARD_ID,
                                          WardName = w.NAME,
                                          CurAddress = cv.CUR_ADDRESS,
                                          CurProvinceId = cv.CUR_PROVINCE_ID,
                                          CurProvinceName = pc.NAME,
                                          CurDistrictId = cv.CUR_DISTRICT_ID,
                                          CurDistrictName = dc.NAME,
                                          CurWardId = cv.CUR_WARD_ID,
                                          CurWardName = wc.NAME,
                                          Email = cv.EMAIL,
                                          MobilePhoneLand = cv.MOBILE_PHONE_LAND,
                                          MobilePhone = cv.MOBILE_PHONE,
                                      }).ToListAsync();

                return new FormatedResponse() { InnerBody = response };
            }
            catch (Exception ex)
            {

                return new FormatedResponse() { StatusCode = EnumStatusCode.StatusCode500, ErrorType = EnumErrorType.UNCATCHABLE, MessageCode = ex.Message };
            }
        }

        public async Task<FormatedResponse> GetHuEmployeeCvEditContactSaveById(long id)
        {
            try
            {
                var response = await (from cv in _uow.Context.Set<HU_EMPLOYEE_CV_EDIT>()
                                      where cv.ID == id
                                      select new HuEmployeeCvEditDTO()
                                      {
                                          Id = cv.ID,
                                          Address = cv.ADDRESS,
                                          ProvinceId = cv.PROVINCE_ID,
                                          DistrictId = cv.DISTRICT_ID,
                                          WardId = cv.WARD_ID,
                                          CurAddress = cv.CUR_ADDRESS,
                                          CurProvinceId = cv.CUR_PROVINCE_ID,
                                          CurDistrictId = cv.CUR_DISTRICT_ID,
                                          CurWardId = cv.CUR_WARD_ID,
                                          Email = cv.EMAIL,
                                          MobilePhoneLand = cv.MOBILE_PHONE_LAND,
                                          MobilePhone = cv.MOBILE_PHONE,
                                          IsSaveContact = cv.IS_SAVE_CONTACT
                                      }).SingleAsync();

                return new FormatedResponse() { InnerBody = response };
            }
            catch (Exception ex)
            {

                return new FormatedResponse() { StatusCode = EnumStatusCode.StatusCode500, ErrorType = EnumErrorType.UNCATCHABLE, MessageCode = ex.Message };
            }
        }

        public async Task<FormatedResponse> GetHuEmployeeCvEditContactUnapprove(long employeeId)
        {
            try
            {
                var response = await (from cv in _uow.Context.Set<HU_EMPLOYEE_CV_EDIT>()
                                      from e in _uow.Context.Set<HU_EMPLOYEE>().Where(x => x.PROFILE_ID == cv.ID).DefaultIfEmpty()
                                      from p in _uow.Context.Set<HU_PROVINCE>().Where(x => x.ID == cv.PROVINCE_ID).DefaultIfEmpty()
                                      from d in _uow.Context.Set<HU_DISTRICT>().Where(x => x.ID == cv.DISTRICT_ID).DefaultIfEmpty()
                                      from w in _uow.Context.Set<HU_WARD>().Where(x => x.ID == cv.WARD_ID).DefaultIfEmpty()
                                      from pc in _uow.Context.Set<HU_PROVINCE>().Where(x => x.ID == cv.CUR_PROVINCE_ID).DefaultIfEmpty()
                                      from dc in _uow.Context.Set<HU_DISTRICT>().Where(x => x.ID == cv.CUR_DISTRICT_ID).DefaultIfEmpty()
                                      from wc in _uow.Context.Set<HU_WARD>().Where(x => x.ID == cv.CUR_WARD_ID).DefaultIfEmpty()
                                      where cv.EMPLOYEE_ID == employeeId && cv.IS_APPROVED_CONTACT == true && cv.IS_SEND_PORTAL_CONTACT == false
                                      select new HuEmployeeCvEditDTO()
                                      {
                                          Id = cv.ID,
                                          Address = cv.ADDRESS,
                                          ProvinceId = cv.PROVINCE_ID,
                                          ProvinceName = p.NAME,
                                          DistrictId = cv.DISTRICT_ID,
                                          DistrictName = d.NAME,
                                          WardId = cv.WARD_ID,
                                          WardName = w.NAME,
                                          CurAddress = cv.CUR_ADDRESS,
                                          CurProvinceId = cv.CUR_PROVINCE_ID,
                                          CurProvinceName = pc.NAME,
                                          CurDistrictId = cv.CUR_DISTRICT_ID,
                                          CurDistrictName = dc.NAME,
                                          CurWardId = cv.CUR_WARD_ID,
                                          CurWardName = wc.NAME,
                                          Email = cv.EMAIL,
                                          MobilePhoneLand = cv.MOBILE_PHONE_LAND,
                                          MobilePhone = cv.MOBILE_PHONE,
                                          Reason = cv.REASON
                                      }).ToListAsync();

                return new FormatedResponse() { InnerBody = response };
            }
            catch (Exception ex)
            {

                return new FormatedResponse() { StatusCode = EnumStatusCode.StatusCode500, ErrorType = EnumErrorType.UNCATCHABLE, MessageCode = ex.Message };
            }
        }

        public async Task<FormatedResponse> GetHuEmployeeCvEditContactUnapproveById(long id)
        {
            try
            {
                var response = await (from cv in _uow.Context.Set<HU_EMPLOYEE_CV_EDIT>()
                                      where cv.ID == id
                                      select new HuEmployeeCvEditDTO()
                                      {
                                          Id = cv.ID,
                                          Address = cv.ADDRESS,
                                          ProvinceId = cv.PROVINCE_ID,
                                          DistrictId = cv.DISTRICT_ID,
                                          WardId = cv.WARD_ID,
                                          CurAddress = cv.CUR_ADDRESS,
                                          CurProvinceId = cv.CUR_PROVINCE_ID,
                                          CurDistrictId = cv.CUR_DISTRICT_ID,
                                          CurWardId = cv.CUR_WARD_ID,
                                          Email = cv.EMAIL,
                                          MobilePhoneLand = cv.MOBILE_PHONE_LAND,
                                          MobilePhone = cv.MOBILE_PHONE,
                                          IsSaveContact = cv.IS_SAVE_CONTACT
                                      }).SingleAsync();

                return new FormatedResponse() { InnerBody = response };
            }
            catch (Exception ex)
            {

                return new FormatedResponse() { StatusCode = EnumStatusCode.StatusCode500, ErrorType = EnumErrorType.UNCATCHABLE, MessageCode = ex.Message };
            }
        }
        //ADDITIONAL_INFO
        public async Task<FormatedResponse> GetHuEmployeeCvAdditionalInfoByEmployeeId(long employeeId , long? time)
        {
            try
            {
                var response = await (from cv in _uow.Context.Set<HU_EMPLOYEE_CV>()
                                      from e in _uow.Context.Set<HU_EMPLOYEE>().Where(x => x.PROFILE_ID == cv.ID).DefaultIfEmpty()
                                      where e.ID == employeeId
                                      select new HuEmployeeCvDTO()
                                      {
                                          Id = cv.ID,
                                          PassNo = cv.PASS_NO,
                                          PassDate = cv.PASS_DATE,
                                          PassExpire = cv.PASS_EXPIRE,
                                          PassPlace = cv.PASS_PLACE,
                                          VisaNo = cv.VISA_NO,
                                          VisaDate = cv.VISA_DATE,
                                          VisaExpire = cv.VISA_EXPIRE,
                                          VisaPlace = cv.VISA_PLACE,
                                          WorkNo = cv.WORK_NO,
                                          WorkPermitDate = cv.WORK_PERMIT_DATE,
                                          WorkPermitExpire = cv.WORK_PERMIT_EXPIRE,
                                          WorkPermitPlace = cv.WORK_PERMIT_PLACE,
                                          EmployeeId = e.ID
                                      }).SingleAsync();
                return new FormatedResponse() { InnerBody = response };
            }
            catch (Exception ex)
            {

                return new FormatedResponse() { StatusCode = EnumStatusCode.StatusCode500, ErrorType = EnumErrorType.UNCATCHABLE, MessageCode = ex.Message };
            }
        }

        public async Task<FormatedResponse> GetHuEmployeeCvAdditionalInfoApproving(long employeeId, long? time)
        {
            var response = await (from cv in _uow.Context.Set<HU_EMPLOYEE_CV_EDIT>()
                                  from e in _uow.Context.Set<HU_EMPLOYEE>().Where(x => x.PROFILE_ID == cv.ID).DefaultIfEmpty()
                                  where cv.EMPLOYEE_ID == employeeId && cv.IS_SEND_PORTAL_ADDITIONAL_INFO == true && cv.IS_APPROVED_ADDITIONAL_INFO == false
                                  select new HuEmployeeCvDTO()
                                  {
                                      Id = cv.ID,
                                      PassNo = cv.PASS_NO,
                                      PassDate = cv.PASS_DATE,
                                      PassExpire = cv.PASS_EXPIRE,
                                      PassPlace = cv.PASS_PLACE,
                                      VisaNo = cv.VISA_NO,
                                      VisaDate = cv.VISA_DATE,
                                      VisaExpire = cv.VISA_EXPIRE,
                                      VisaPlace = cv.VISA_PLACE,
                                      WorkNo = cv.WORK_NO,
                                      WorkPermitDate = cv.WORK_PERMIT_DATE,
                                      WorkPermitExpire = cv.WORK_PERMIT_EXPIRE,
                                      WorkPermitPlace = cv.WORK_PERMIT_PLACE,
                                      EmployeeId = e.ID
                                  }).ToListAsync();
            return new FormatedResponse() { InnerBody = response };
        }

        public async Task<FormatedResponse> GetHuEmployeeCvEditAdditionalInfoSave(long employeeId, long? time)
        {
            try
            {
                var response = await (from cv in _uow.Context.Set<HU_EMPLOYEE_CV_EDIT>()
                                      from e in _uow.Context.Set<HU_EMPLOYEE>().Where(x => x.PROFILE_ID == cv.ID).DefaultIfEmpty()
                                      where cv.EMPLOYEE_ID == employeeId && cv.IS_SAVE_ADDITIONAL_INFO == true
                                      select new HuEmployeeCvEditDTO()
                                      {
                                          Id = cv.ID,
                                          PassNo = cv.PASS_NO,
                                          PassDate = cv.PASS_DATE,
                                          PassExpire = cv.PASS_EXPIRE,
                                          PassPlace = cv.PASS_PLACE,
                                          VisaNo = cv.VISA_NO,
                                          VisaDate = cv.VISA_DATE,
                                          VisaExpire = cv.VISA_EXPIRE,
                                          VisaPlace = cv.VISA_PLACE,
                                          WorkNo = cv.WORK_NO,
                                          WorkPermitDate = cv.WORK_PERMIT_DATE,
                                          WorkPermitExpire = cv.WORK_PERMIT_EXPIRE,
                                          WorkPermitPlace = cv.WORK_PERMIT_PLACE
                                      }).ToListAsync();
                return new FormatedResponse() { InnerBody = response };
            }
            catch (Exception ex)
            {

                return new FormatedResponse() { StatusCode = EnumStatusCode.StatusCode500, ErrorType = EnumErrorType.UNCATCHABLE, MessageCode = ex.Message };
            }
        }

        public async Task<FormatedResponse> GetHuEmployeeCvEditAdditionalInfoSaveEdit(long id)
        {
            try
            {
                var response = await (from cv in _uow.Context.Set<HU_EMPLOYEE_CV_EDIT>()
                                      where cv.ID == id
                                      select new HuEmployeeCvEditDTO()
                                      {
                                          Id = cv.ID,
                                          PassNo = cv.PASS_NO,
                                          PassDate = cv.PASS_DATE,
                                          PassExpire = cv.PASS_EXPIRE,
                                          PassPlace = cv.PASS_PLACE,
                                          VisaNo = cv.VISA_NO,
                                          VisaDate = cv.VISA_DATE,
                                          VisaExpire = cv.VISA_EXPIRE,
                                          VisaPlace = cv.VISA_PLACE,
                                          WorkNo = cv.WORK_NO,
                                          WorkPermitDate = cv.WORK_PERMIT_DATE,
                                          WorkPermitExpire = cv.WORK_PERMIT_EXPIRE,
                                          WorkPermitPlace = cv.WORK_PERMIT_PLACE
                                      }).FirstOrDefaultAsync();
                return new FormatedResponse() { InnerBody = response };
            }
            catch (Exception ex)
            {

                return new FormatedResponse() { StatusCode = EnumStatusCode.StatusCode500, ErrorType = EnumErrorType.UNCATCHABLE, MessageCode = ex.Message };
            }
        }

        public async Task<FormatedResponse> GetHuEmployeeCvEditAddtionalInfoById(long id)
        {
            try
            {
                var response = await (from cv in _uow.Context.Set<HU_EMPLOYEE_CV_EDIT>()
                                      from e in _uow.Context.Set<HU_EMPLOYEE>().Where(x => x.PROFILE_ID == cv.ID).DefaultIfEmpty()
                                      where cv.ID == id
                                      select new HuEmployeeCvEditDTO()
                                      {
                                          Id = cv.ID,
                                          PassNo = cv.PASS_NO,
                                          PassDate = cv.PASS_DATE,
                                          PassExpire = cv.PASS_EXPIRE,
                                          PassPlace = cv.PASS_PLACE,
                                          VisaNo = cv.VISA_NO,
                                          VisaDate = cv.VISA_DATE,
                                          VisaExpire = cv.VISA_EXPIRE,
                                          VisaPlace = cv.VISA_PLACE,
                                          WorkNo = cv.WORK_NO,
                                          WorkPermitDate = cv.WORK_PERMIT_DATE,
                                          WorkPermitExpire = cv.WORK_PERMIT_EXPIRE,
                                          WorkPermitPlace = cv.WORK_PERMIT_PLACE,
                                          IsSaveAdditionalInfo = cv.IS_SAVE_ADDITIONAL_INFO
                                      }).SingleAsync();
                return new FormatedResponse() { InnerBody = response };
            }
            catch (Exception ex)
            {

                return new FormatedResponse() { StatusCode = EnumStatusCode.StatusCode500, ErrorType = EnumErrorType.UNCATCHABLE, MessageCode = ex.Message };
            }
        }

        public async Task<FormatedResponse> GetHuEmployeeCvAdditionalInfoUnapprove(long employeeId, long? time)
        {
            try
            {
                var response = await(from cv in _uow.Context.Set<HU_EMPLOYEE_CV_EDIT>()
                                     from e in _uow.Context.Set<HU_EMPLOYEE>().Where(x => x.PROFILE_ID == cv.ID).DefaultIfEmpty()
                                     where cv.EMPLOYEE_ID == employeeId && cv.IS_SEND_PORTAL_ADDITIONAL_INFO == false && cv.IS_APPROVED_ADDITIONAL_INFO == true && cv.IS_SAVE_ADDITIONAL_INFO == false
                                     select new HuEmployeeCvEditDTO()
                                     {
                                         Id = cv.ID,
                                         PassNo = cv.PASS_NO,
                                         PassDate = cv.PASS_DATE,
                                         PassExpire = cv.PASS_EXPIRE,
                                         PassPlace = cv.PASS_PLACE,
                                         VisaNo = cv.VISA_NO,
                                         VisaDate = cv.VISA_DATE,
                                         VisaExpire = cv.VISA_EXPIRE,
                                         VisaPlace = cv.VISA_PLACE,
                                         WorkNo = cv.WORK_NO,
                                         WorkPermitDate = cv.WORK_PERMIT_DATE,
                                         WorkPermitExpire = cv.WORK_PERMIT_EXPIRE,
                                         WorkPermitPlace = cv.WORK_PERMIT_PLACE,
                                         Reason = cv.REASON
                                         
                                     }).ToListAsync();
                return new FormatedResponse() { InnerBody = response };
            }
            catch (Exception ex)
            {

                return new FormatedResponse() { StatusCode = EnumStatusCode.StatusCode500, ErrorType = EnumErrorType.UNCATCHABLE, MessageCode = ex.Message };
            }
        }

        public async Task<FormatedResponse> GetHuEmployeeCvAdditionalInfoUnapproveById(long id)
        {
            try
            {
                var response = await(from cv in _uow.Context.Set<HU_EMPLOYEE_CV_EDIT>()
                                     from e in _uow.Context.Set<HU_EMPLOYEE>().Where(x => x.PROFILE_ID == cv.ID).DefaultIfEmpty()
                                     where cv.ID == id
                                     select new HuEmployeeCvEditDTO()
                                     {
                                         Id = cv.ID,
                                         PassNo = cv.PASS_NO,
                                         PassDate = cv.PASS_DATE,
                                         PassExpire = cv.PASS_EXPIRE,
                                         PassPlace = cv.PASS_PLACE,
                                         VisaNo = cv.VISA_NO,
                                         VisaDate = cv.VISA_DATE,
                                         VisaExpire = cv.VISA_EXPIRE,
                                         VisaPlace = cv.VISA_PLACE,
                                         WorkNo = cv.WORK_NO,
                                         WorkPermitDate = cv.WORK_PERMIT_DATE,
                                         WorkPermitExpire = cv.WORK_PERMIT_EXPIRE,
                                         WorkPermitPlace = cv.WORK_PERMIT_PLACE,
                                         IsSaveAdditionalInfo = cv.IS_SAVE_ADDITIONAL_INFO
                                     }).SingleAsync();
                return new FormatedResponse() { InnerBody = response };
            }
            catch (Exception ex)
            {

                return new FormatedResponse() { StatusCode = EnumStatusCode.StatusCode500, ErrorType = EnumErrorType.UNCATCHABLE, MessageCode = ex.Message };
            }
        }
        //INSUARENCE_INFO

        public async Task<FormatedResponse> GetInsuarenceInfoByEmployeeId(long employeeId)
        {
            var response = await (from cv in _uow.Context.Set<HU_EMPLOYEE_CV>()
                                  from e in _uow.Context.Set<HU_EMPLOYEE>().Where(x => x.PROFILE_ID == cv.ID).DefaultIfEmpty()
                                  from w in _uow.Context.Set<INS_WHEREHEALTH>().Where(x => x.ID == cv.INS_WHEREHEALTH_ID).DefaultIfEmpty()
                                  where e.ID == employeeId
                                  select new
                                  {
                                      Id = cv.ID,
                                      InsurenceNumber = cv.INSURENCE_NUMBER,
                                      InsWherehealthName = w.NAME_VN,
                                      InsCardNumber = cv.INS_CARD_NUMBER
                                  }).SingleAsync();
            return new FormatedResponse() { InnerBody = response };
        }

        public async Task<FormatedResponse> GetHuEmployeeCvEditInsuarenceApproving(long employeeId)
        {
            var response = await(from cv in _uow.Context.Set<HU_EMPLOYEE_CV_EDIT>()
                                 from e in _uow.Context.Set<HU_EMPLOYEE>().Where(x => x.PROFILE_ID == cv.ID).DefaultIfEmpty()
                                 from w in _uow.Context.Set<INS_WHEREHEALTH>().Where(x => x.ID == cv.INS_WHEREHEALTH_ID).DefaultIfEmpty()
                                 where cv.EMPLOYEE_ID == employeeId && cv.IS_APPROVED_INSUARENCE_INFO == false
                                 select new
                                 {
                                     Id = cv.ID,
                                     InsurenceNumber = cv.INSURENCE_NUMBER,
                                     InsWherehealthName = w.NAME_VN,
                                     InsCardNumber = cv.INS_CARD_NUMBER,
                                     IsSaveInsurence = cv.IS_SAVE_INSURENCE
                                 }).ToListAsync();
            return new FormatedResponse() { InnerBody = response };
        }
        
        public async Task<FormatedResponse> GetHuEmployeeCvEditInsurenceSave(long employeeId)
        {
            var response = await(from cv in _uow.Context.Set<HU_EMPLOYEE_CV_EDIT>()
                                 from e in _uow.Context.Set<HU_EMPLOYEE>().Where(x => x.PROFILE_ID == cv.ID).DefaultIfEmpty()
                                 from w in _uow.Context.Set<INS_WHEREHEALTH>().Where(x => x.ID == cv.INS_WHEREHEALTH_ID).DefaultIfEmpty()
                                 where cv.EMPLOYEE_ID == employeeId && cv.IS_SAVE_INSURENCE == true
                                 select new
                                 {
                                     Id = cv.ID,
                                     InsurenceNumber = cv.INSURENCE_NUMBER,
                                     InsWherehealthName = w.NAME_VN,
                                     InsCardNumber = cv.INS_CARD_NUMBER,
                                     IsSaveInsurence = cv.IS_SAVE_INSURENCE
                                 }).ToListAsync();
            return new FormatedResponse() { InnerBody = response };
        }

        public async Task<FormatedResponse> GetInsurenceSaveById(long id)
        {
            var response = await (from cv in _uow.Context.Set<HU_EMPLOYEE_CV_EDIT>()
                                  where cv.ID == id
                                  select new HuEmployeeCvEditDTO()
                                  {
                                      Id = cv.ID,
                                      InsurenceNumber = cv.INSURENCE_NUMBER,
                                      InsCardNumber = cv.INS_CARD_NUMBER,
                                      InsWherehealthId = cv.INS_WHEREHEALTH_ID,
                                      IsSaveInsurence = cv.IS_SAVE_INSURENCE,
                                      EmployeeId = cv.EMPLOYEE_ID
                                  }).SingleAsync();
            return new FormatedResponse() { InnerBody = response };
        }



        public class ProfileInfoByPortalDTO
        {
            public long Id { get; set; }
            public string? Avatar { get; set; }
            public string? Gender { get; set; }
            public string? WorkStatus { get; set; }
            public string? EmployeeObject { get; set; }
            public string? FullName { get; set; }
            public string? PositionName { get; set; }
            public string? EmployeeCode { get; set; }
            public string? OrgName { get; set; }
            public string? CompanyName { get; set; }
            public string? CmsName { get; set; }
            public string? CmsJobName { get; set; }
            public string? Address { get; set; }
            public int? Seniority { get; set; }
            public string? SeniorityStr { get; set; }
            public int? WorkingTime { get; set; }
            public string? WorkingTimeStr { get; set; }
            public string? ContractNo { get; set; }
            public string? ContractType { get; set; }
            public DateTime? StartDate { get; set; }
            public DateTime? ExpireDate { get; set; }
        }


        public async Task<FormatedResponse> GetProfileInfoByPortal2(long employeeId)
        {
            try
            {
                var query = await (from table in _dbContext.HuEmployees.Where(x => x.ID == employeeId).DefaultIfEmpty()
                                   from c in _dbContext.HuEmployeeCvs.Where(x => x.ID == table.PROFILE_ID).DefaultIfEmpty()
                                   from p in _dbContext.HuPositions.Where(x => x.ID == table.POSITION_ID).DefaultIfEmpty()
                                   from o in _dbContext.HuOrganizations.Where(x => x.ID == table.ORG_ID).DefaultIfEmpty()
                                   from company in _dbContext.HuCompanys.Where(x => x.ID == o.COMPANY_ID).DefaultIfEmpty()
                                   from s in _dbContext.SysOtherLists.Where(x => x.ID == c.GENDER_ID).DefaultIfEmpty()
                                   from s2 in _dbContext.SysOtherLists.Where(x => x.ID == table.EMPLOYEE_OBJECT_ID).DefaultIfEmpty()
                                   from s3 in _dbContext.SysOtherLists.Where(x => x.ID == table.WORK_STATUS_ID).DefaultIfEmpty()
                                   from pc in _dbContext.HuPositions.Where(x => x.ID == p.LM).DefaultIfEmpty()
                                   from ec in _dbContext.HuEmployees.Where(x => x.ID == pc.MASTER).DefaultIfEmpty()

                                   from contract in _dbContext.HuContracts.Where(x => x.EMPLOYEE_ID == table.ID).OrderBy(x => x.START_DATE).Take(1).DefaultIfEmpty()
                                   from workingTime in _dbContext.HuContracts.Where(x => x.EMPLOYEE_ID == table.ID).OrderByDescending(x => x.START_DATE).Take(1).DefaultIfEmpty()
                                   from contractType in _dbContext.HuContractTypes.Where(x => x.ID == workingTime.CONTRACT_TYPE_ID).DefaultIfEmpty()

                                   select new ProfileInfoByPortalDTO
                                   {
                                       Id = table.ID,
                                       Avatar = c.AVATAR,
                                       Gender = s.NAME,
                                       WorkStatus = s3.NAME,
                                       EmployeeObject = s2.NAME,
                                       FullName = c.FULL_NAME,
                                       PositionName = p.NAME,
                                       EmployeeCode = table.CODE,
                                       OrgName = o.NAME,
                                       CompanyName = company.NAME_VN,
                                       CmsName = ec.Profile.FULL_NAME,
                                       CmsJobName = pc.NAME,
                                       //CmsName = pce.Profile.FULL_NAME,
                                       //CmsJobName = pc.NAME,
                                       Address = company.WORK_ADDRESS,
                                       Seniority = (DateTime.Now.Year - contract.START_DATE.Value.Year) * 12 + DateTime.Now.Month - contract.START_DATE.Value.Month < 0
                                                    ? 0 : (DateTime.Now.Year - contract.START_DATE.Value.Year) * 12 + DateTime.Now.Month - contract.START_DATE.Value.Month,
                                       WorkingTime = (DateTime.Now.Year - workingTime.START_DATE.Value.Year) * 12 + DateTime.Now.Month - workingTime.START_DATE.Value.Month < 0
                                                      ? 0 : (DateTime.Now.Year - workingTime.START_DATE.Value.Year) * 12 + DateTime.Now.Month - workingTime.START_DATE.Value.Month,
                                       ContractNo = workingTime.CONTRACT_NO,
                                       ContractType = contractType.NAME,
                                       StartDate = workingTime.START_DATE,
                                       ExpireDate = workingTime.EXPIRE_DATE
                                   }).FirstOrDefaultAsync();




                var get_employee = await _dbContext.HuEmployees.Where(x => x.ID == employeeId).FirstAsync();

                // đang làm việc (CODE = "ESW")
                // trường hợp khác (CODE = "00023")
                // đã nghỉ việc (CODE = "ESQ")

                var get_is_working_by_code = (from table1 in _dbContext.SysOtherListTypes.Where(x => x.CODE == "EMP_STATUS")
                                              from table2 in _dbContext.SysOtherLists.Where(x => x.CODE == "ESW" && x.TYPE_ID == table1.ID)
                                              select table2.ID).First();

                var get_is_not_working_by_code = (from table1 in _dbContext.SysOtherListTypes.Where(x => x.CODE == "EMP_STATUS")
                                                  from table2 in _dbContext.SysOtherLists.Where(x => x.CODE == "ESQ" && x.TYPE_ID == table1.ID)
                                                  select table2.ID).First();

                var get_other_case = (from table1 in _dbContext.SysOtherListTypes.Where(x => x.CODE == "EMP_STATUS")
                                      from table2 in _dbContext.SysOtherLists.Where(x => x.CODE == "00023" && x.TYPE_ID == table1.ID)
                                      select table2.ID).First();

                // calculate seniority
                if (get_employee.WORK_STATUS_ID == get_is_working_by_code || get_employee.WORK_STATUS_ID == get_other_case)
                {
                    var start_date = _dbContext.HuContracts
                                    .Where(x => x.EMPLOYEE_ID == employeeId)
                                    .OrderBy(x => x.START_DATE)
                                    .Select(x => x.START_DATE)
                                    .First();

                    var expire_date = _dbContext.HuContracts
                                    .Where(x => x.EMPLOYEE_ID == employeeId)
                                    .OrderByDescending(x => x.START_DATE)
                                    .Select(x => x.EXPIRE_DATE)
                                    .FirstOrDefault();

                    if (DateTime.Now <= expire_date)
                    {
                        // seniority = now date - start date of contract
                        var seniority1 = DateTime.Now.Subtract((DateTime)start_date).Days;

                        // calculate month
                        var calculate_month = Math.Floor(((double)seniority1 / 30) * 10) / 10;

                        if (seniority1 > 365)
                        {
                            // calculate year and calculate month
                            var calculate_year = Math.Floor((double)seniority1 / 365);

                            calculate_month = ((double)(seniority1 - calculate_year * 365)) / 30;
                            calculate_month = Math.Floor(calculate_month * 10) / 10;

                            // calculate surplus
                            var calculate_surplus = seniority1 % 365;

                            query.SeniorityStr = (calculate_surplus != 0) ? $"{calculate_year} năm {calculate_month} tháng" : $"{calculate_year} năm";
                            
                            query.WorkingTimeStr = (calculate_surplus != 0) ? $"{calculate_year} năm {calculate_month} tháng" : $"{calculate_year} năm";
                        }
                        else if (seniority1 < 365)
                        {
                            query.SeniorityStr = $"{calculate_month} tháng";

                            query.WorkingTimeStr = $"{calculate_month} tháng";
                        }
                        else if (seniority1 == 365)
                        {
                            query.SeniorityStr = $"1 năm";

                            query.WorkingTimeStr = $"1 năm";
                        }
                    }
                    else if (expire_date == null)
                    {
                        // seniority = now date - start date of contract
                        var seniority1 = DateTime.Now.Subtract((DateTime)start_date).Days;

                        // calculate month
                        var calculate_month = Math.Floor(((double)seniority1 / 30) * 10) / 10;

                        if (seniority1 > 365)
                        {
                            // calculate year and calculate month
                            var calculate_year = Math.Floor((double)seniority1 / 365);

                            calculate_month = ((double)(seniority1 - calculate_year * 365)) / 30;
                            calculate_month = Math.Floor(calculate_month * 10) / 10;

                            // calculate surplus
                            var calculate_surplus = seniority1 % 365;

                            query.SeniorityStr = (calculate_surplus != 0) ? $"{calculate_year} năm {calculate_month} tháng" : $"{calculate_year} năm";

                            query.WorkingTimeStr = (calculate_surplus != 0) ? $"{calculate_year} năm {calculate_month} tháng" : $"{calculate_year} năm";
                        }
                        else if (seniority1 < 365)
                        {
                            query.SeniorityStr = $"{calculate_month} tháng";

                            query.WorkingTimeStr = $"{calculate_month} tháng";
                        }
                        else if (seniority1 == 365)
                        {
                            query.SeniorityStr = $"1 năm";

                            query.WorkingTimeStr = $"1 năm";
                        }
                    }
                    else
                    {
                        // seniority = expire date - start date of contract
                        var seniority1 = ((DateTime)expire_date).Subtract((DateTime)start_date).Days;

                        // calculate month
                        var calculate_month = Math.Floor(((double)seniority1 / 30) * 10) / 10;

                        if (seniority1 > 365)
                        {
                            // calculate year and calculate month
                            var calculate_year = Math.Floor((double)seniority1 / 365);

                            calculate_month = ((double)(seniority1 - calculate_year * 365)) / 30;
                            calculate_month = Math.Floor(calculate_month * 10) / 10;

                            // calculate surplus
                            var calculate_surplus = seniority1 % 365;

                            query.SeniorityStr = (calculate_surplus != 0) ? $"{calculate_year} năm {calculate_month} tháng" : $"{calculate_year} năm";

                            query.WorkingTimeStr = (calculate_surplus != 0) ? $"{calculate_year} năm {calculate_month} tháng" : $"{calculate_year} năm";
                        }
                        else if (seniority1 < 365)
                        {
                            query.SeniorityStr = $"{calculate_month} tháng";

                            query.WorkingTimeStr = $"{calculate_month} tháng";
                        }
                        else if (seniority1 == 365)
                        {
                            query.SeniorityStr = $"1 năm";
                            
                            query.WorkingTimeStr = $"1 năm";
                        }
                    }

                }
                else if (get_employee.WORK_STATUS_ID == get_is_not_working_by_code)
                {
                    var start_date = _dbContext.HuContracts
                                    .Where(x => x.EMPLOYEE_ID == employeeId)
                                    .OrderBy(x => x.START_DATE)
                                    .Select(x => x.START_DATE)
                                    .First();

                    var terminate_contract_date = _dbContext.HuTerminates
                                                    .Where(x => x.EMPLOYEE_ID == employeeId)
                                                    .OrderByDescending(x => x.LAST_DATE)
                                                    .Select(x => x.LAST_DATE)
                                                    .First();

                    if (terminate_contract_date != null)
                    {
                        // seniority = terminate contract date - start date of contract
                        var seniority2 = ((DateTime)terminate_contract_date).Subtract((DateTime)start_date).Days;

                        // calculate month
                        var calculate_month = Math.Floor(((double)seniority2 / 30) * 10) / 10;

                        if (seniority2 > 365)
                        {
                            // calculate year and calculate month
                            var calculate_year = Math.Floor((double)seniority2 / 365);

                            calculate_month = ((double)(seniority2 - calculate_year * 365)) / 30;
                            calculate_month = Math.Floor(calculate_month * 10) / 10;

                            // calculate surplus
                            var calculate_surplus = seniority2 % 365;

                            query.SeniorityStr = (calculate_surplus != 0) ? $"{calculate_year} năm {calculate_month} tháng" : $"{calculate_year} năm";

                            query.WorkingTimeStr = (calculate_surplus != 0) ? $"{calculate_year} năm {calculate_month} tháng" : $"{calculate_year} năm";
                        }
                        else if (seniority2 < 365)
                        {
                            query.SeniorityStr = $"{calculate_month} tháng";

                            query.WorkingTimeStr = $"{calculate_month} tháng";
                        }
                        else if (seniority2 == 365)
                        {
                            query.SeniorityStr = $"1 năm";

                            query.WorkingTimeStr = $"1 năm";
                        }
                    }
                }

                return new() { InnerBody = query, StatusCode = EnumStatusCode.StatusCode200 };
            }
            catch (Exception ex)
            {
                return new FormatedResponse() { MessageCode = CommonMessageCode.GET_METHOD_FAILED, ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400 };
            }
        }
    }
}
