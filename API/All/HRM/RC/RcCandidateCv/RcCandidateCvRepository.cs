using CORE.GenericUOW;
using CORE.DTO;
using API.DTO;
using CORE.Enum;
using CORE.StaticConstant;
using API.All.DbContexts;
using CORE.AutoMapper;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;
using DocumentFormat.OpenXml.Drawing.Charts;
using DocumentFormat.OpenXml.ExtendedProperties;
using DocumentFormat.OpenXml.Wordprocessing;

namespace API.Controllers.RcCandidateCv
{
    public class RcCandidateCvRepository : IRcCandidateCvRepository
    {
        private readonly GenericUnitOfWork _uow;
        private readonly FullDbContext _dbContext;
        private IGenericRepository<RC_CANDIDATE_CV, RcCandidateCvDTO> _genericRepository;
        private readonly GenericReducer<RC_CANDIDATE_CV, RcCandidateCvDTO> _genericReducer;

        public RcCandidateCvRepository(FullDbContext context, GenericUnitOfWork uow)
        {
            _dbContext = context;
            _uow = uow;
            _genericRepository = _uow.GenericRepository<RC_CANDIDATE_CV, RcCandidateCvDTO>();
            _genericReducer = new();
        }

        public async Task<GenericPhaseTwoListResponse<RcCandidateCvDTO>> SinglePhaseQueryList(GenericQueryListDTO<RcCandidateCvDTO> request)
        {
            var joined = from p in _dbContext.RcCandidateCvs.AsNoTracking()
                         // JOIN OTHER ENTITIES BASED ON THE BUSINESS
                         select new RcCandidateCvDTO
                         {
                             Id = p.ID,
                             
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
                var list = new List<RC_CANDIDATE_CV>
                    {
                        (RC_CANDIDATE_CV)response
                    };
                var joined = (from l in list
                              // JOIN OTHER ENTITIES BASED ON THE BUSINESS
                              select new RcCandidateCvDTO
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

        public async Task<FormatedResponse> Create(GenericUnitOfWork _uow, RcCandidateCvDTO dto, string sid)
        {
            var response = await _genericRepository.Create(_uow, dto, sid);
            return response;
        }

        public async Task<FormatedResponse> CreateRange(GenericUnitOfWork _uow, List<RcCandidateCvDTO> dtos, string sid)
        {
            var add = new List<RcCandidateCvDTO>();
            add.AddRange(dtos);
            var response = await _genericRepository.CreateRange(_uow, add, sid);
            return response;
        }

        public async Task<FormatedResponse> Update(GenericUnitOfWork _uow, RcCandidateCvDTO dto, string sid, bool patchMode = true)
        {
            var response = await _genericRepository.Update(_uow, dto, sid, patchMode);
            return response;
        }

        public async Task<FormatedResponse> UpdateRange(GenericUnitOfWork _uow, List<RcCandidateCvDTO> dtos, string sid, bool patchMode = true)
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

        public Task<FormatedResponse> ToggleActiveIds(GenericUnitOfWork _uow, List<long> ids, bool valueToBind, string sid)
        {
            throw new NotImplementedException();
        }

        public async Task<FormatedResponse> GetListPos()
        {
            var query = await (from p in _dbContext.HuPositions.AsNoTracking().DefaultIfEmpty()
                               where p.IS_ACTIVE == true
                               select new
                               {
                                   Id = p.ID,
                                   Name = p.NAME
                               }).ToListAsync();
            return new FormatedResponse() { InnerBody = query };
        }

        public async Task<FormatedResponse> GetCv(long candidateCvId)
        {
            try
            {
                var query = await (from c in _dbContext.RcCandidateCvs
                                   from p in _dbContext.HuProvinces.Where(x => x.ID == c.ID_PLACE).DefaultIfEmpty()
                                   from p1 in _dbContext.HuProvinces.Where(x => x.ID == c.PER_PROVINCE).DefaultIfEmpty()
                                   from p2 in _dbContext.HuProvinces.Where(x => x.ID == c.CONTACT_PROVINCE_TEMP).DefaultIfEmpty()
                                   from d1 in _dbContext.HuDistricts.Where(x => x.ID == c.PER_DISTRICT).DefaultIfEmpty()
                                   from d2 in _dbContext.HuDistricts.Where(x => x.ID == c.CONTACT_DISTRICT_TEMP).DefaultIfEmpty()
                                   from w1 in _dbContext.HuWards.Where(x => x.ID == c.PER_WARD).DefaultIfEmpty()
                                   from w2 in _dbContext.HuWards.Where(x => x.ID == c.CONTACT_WARD_TEMP).DefaultIfEmpty()
                                   from s in _dbContext.SysOtherLists.Where(s => s.ID == c.GENDER_ID).DefaultIfEmpty()
                                   from s1 in _dbContext.SysOtherLists.Where(s1 => s1.ID == c.NATIONALITY_ID).DefaultIfEmpty()
                                   from s2 in _dbContext.SysOtherLists.Where(s2 => s2.ID == c.NATION_ID).DefaultIfEmpty()
                                   from s3 in _dbContext.SysOtherLists.Where(s2 => s2.ID == c.RELIGION_ID).DefaultIfEmpty()
                                   from s4 in _dbContext.SysOtherLists.Where(s2 => s2.ID == c.MARITAL_STATUS).DefaultIfEmpty()
                                   where c.ID == candidateCvId

                                   select new
                                   {
                                       Id = candidateCvId,
                                       Gender = s.NAME,
                                       MaritalStatus = s4.NAME,
                                       Nation = s2.NAME,
                                       Religion = s3.NAME,
                                       Nationality = s1.NAME,
                                       BirthDate = c.BIRTH_DATE,
                                       BirthAddress = c.BIRTH_ADDRESS,
                                       IdNo = c.ID_NO,
                                       IdDate = c.ID_DATE,
                                       IdDateExpire = c.ID_DATE_EXPIRE,
                                       IdPlace = p.NAME,
                                       PerAddress = c.PER_ADDRESS,
                                       PerProvince = p1.NAME,
                                       PerDistrict = d1.NAME,
                                       PerWard = w1.NAME,
                                       ContactAddressTemp = c.CONTACT_ADDRESS_TEMP,
                                       ContactProvinceTemp = p2.NAME,
                                       ContactDistrictTemp = d2.NAME,
                                       ContactWardTemp = w2.NAME,
                                       PerEmail = c.PER_EMAIL,
                                       MobilePhone = c.MOBILE_PHONE,
                                       FinderSdt = c.FINDER_SDT,
                                       IsWorkPermit = c.IS_WORK_PERMIT!.Value == true ? "Đã có GPLĐ" : "Chưa có GPLĐ",
                                   }).SingleAsync();
                return new() { InnerBody = query };
            }
            catch (Exception ex)
            {
                return new() { InnerBody = null, MessageCode = ex.Message };
            }
        }

        public async Task<FormatedResponse> GetCvById(long id)
        {
            try
            {
                var query = await (from c in _dbContext.RcCandidateCvs
                                   from p in _dbContext.HuProvinces.Where(x => x.ID == c.ID_PLACE).DefaultIfEmpty()
                                   from p1 in _dbContext.HuProvinces.Where(x => x.ID == c.PER_PROVINCE).DefaultIfEmpty()
                                   from p2 in _dbContext.HuProvinces.Where(x => x.ID == c.CONTACT_PROVINCE_TEMP).DefaultIfEmpty()
                                   from d1 in _dbContext.HuDistricts.Where(x => x.ID == c.PER_DISTRICT).DefaultIfEmpty()
                                   from d2 in _dbContext.HuDistricts.Where(x => x.ID == c.CONTACT_DISTRICT_TEMP).DefaultIfEmpty()
                                   from w1 in _dbContext.HuWards.Where(x => x.ID == c.PER_WARD).DefaultIfEmpty()
                                   from w2 in _dbContext.HuWards.Where(x => x.ID == c.CONTACT_WARD_TEMP).DefaultIfEmpty()
                                   from s in _dbContext.SysOtherLists.Where(s => s.ID == c.GENDER_ID).DefaultIfEmpty()
                                   from s1 in _dbContext.SysOtherLists.Where(s1 => s1.ID == c.NATIONALITY_ID).DefaultIfEmpty()
                                   from s2 in _dbContext.SysOtherLists.Where(s2 => s2.ID == c.NATION_ID).DefaultIfEmpty()
                                   from s3 in _dbContext.SysOtherLists.Where(s2 => s2.ID == c.RELIGION_ID).DefaultIfEmpty()
                                   from s4 in _dbContext.SysOtherLists.Where(s2 => s2.ID == c.MARITAL_STATUS).DefaultIfEmpty()
                                   where c.ID == id
                                   select new
                                   {
                                       Id = id,
                                       GenderId = c.GENDER_ID,
                                       MaritalStatus = c.MARITAL_STATUS,
                                       NationId = c.NATION_ID,
                                       ReligionId = c.RELIGION_ID,
                                       NationalityId = c.NATIONALITY_ID,
                                       BirthDate = c.BIRTH_DATE,
                                       BirthAddress = c.BIRTH_ADDRESS,
                                       IdNo = c.ID_NO,
                                       IdDate = c.ID_DATE,
                                       IdDateExpire = c.ID_DATE_EXPIRE,
                                       IdPlace = c.ID_PLACE,
                                       PerAddress = c.PER_ADDRESS,
                                       PerProvince = c.PER_PROVINCE,
                                       PerDistrict = c.PER_DISTRICT,
                                       PerWard = c.PER_WARD,
                                       ContactAddressTemp = c.CONTACT_ADDRESS_TEMP,
                                       ContactProvinceTemp = c.CONTACT_PROVINCE_TEMP,
                                       ContactDistrictTemp = c.CONTACT_DISTRICT_TEMP,
                                       ContactWardTemp = c.CONTACT_WARD_TEMP,
                                       PerEmail = c.PER_EMAIL,
                                       MobilePhone = c.MOBILE_PHONE,
                                       FinderSdt = c.FINDER_SDT,
                                       IsWorkPermit = c.IS_WORK_PERMIT,
                                   }).SingleAsync();
                return new() { InnerBody = query };
            }
            catch (Exception ex)
            {
                return new() { InnerBody = null, MessageCode = ex.Message };
            }
        }

        public async Task<FormatedResponse> UpdateCv(CandidateEditDTO request)
        {
            try
            {
                _dbContext.Database.BeginTransaction();

                if (request.BirthDate.HasValue && ((DateTime.Now.Year - request.BirthDate.Value.Year) < 15))
                {
                    return new FormatedResponse() { MessageCode = CommonMessageCode.BIRTH_DAY_NOT_BIGGER_THAN_DATE_TIME_NOW, ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400 };
                }
                var entity = _dbContext.RcCandidateCvs.Where(x => x.ID == request.Id).FirstOrDefault();
                if (entity != null)
                {
                    entity.GENDER_ID = request.GenderId;
                    entity.MARITAL_STATUS = request.MaritalStatus;
                    entity.NATION_ID = request.NationId;
                    entity.RELIGION_ID = request.ReligionId;
                    entity.NATIONALITY_ID = request.NationalityId;
                    entity.BIRTH_DATE = request.BirthDate;
                    entity.BIRTH_ADDRESS = request.BirthAddress;
                    entity.ID_NO = request.IdNo;
                    entity.ID_DATE = request.IdDate;
                    entity.ID_DATE_EXPIRE = request.IdDateExpire;
                    entity.ID_PLACE = request.IdPlace;
                    entity.PER_ADDRESS = request.PerAddress;
                    entity.PER_PROVINCE = request.PerProvince;
                    entity.PER_DISTRICT = request.PerDistrict;
                    entity.PER_WARD = request.PerWard;
                    entity.CONTACT_ADDRESS_TEMP = request.ContactAddressTemp;
                    entity.CONTACT_PROVINCE_TEMP = request.ContactProvinceTemp;
                    request.ContactDistrictTemp = request.ContactDistrictTemp;
                    entity.CONTACT_WARD_TEMP = request.ContactWardTemp;
                    entity.PER_EMAIL = request.PerEmail;
                    entity.MOBILE_PHONE = request.MobilePhone;
                    entity.FINDER_SDT = request.FinderSdt;
                    entity.IS_WORK_PERMIT = request.IsWorkPermit;

                    await _dbContext.SaveChangesAsync();
                    await _dbContext.Database.CommitTransactionAsync();


                    return new() { MessageCode = CommonMessageCode.UPDATE_SUCCESS, InnerBody = entity, StatusCode = EnumStatusCode.StatusCode200 };
                }
                else
                {
                    return new() { MessageCode = CommonMessageCode.ENTITY_NOT_FOUND, InnerBody = false, StatusCode = EnumStatusCode.StatusCode400 };
                }
            }
            catch (Exception ex)
            {
                await _dbContext.Database.RollbackTransactionAsync();
                await _dbContext.DisposeAsync();
                return new() { ErrorType = EnumErrorType.UNCATCHABLE, StatusCode = EnumStatusCode.StatusCode500, MessageCode = ex.Message };
            }
        }

        public async Task<FormatedResponse> GetLevelInfo(long candidateCvId)
        {
            try
            {
                var query = await (from c in _dbContext.RcCandidateCvs
                                   from s1 in _dbContext.SysOtherLists.AsNoTracking().Where(x => x.ID == c.EDUCATION_LEVEL_ID).DefaultIfEmpty()
                                   from s2 in _dbContext.SysOtherLists.AsNoTracking().Where(x => x.ID == c.LEARNING_LEVEL_ID).DefaultIfEmpty()
                                   from s3 in _dbContext.SysOtherLists.AsNoTracking().Where(x => x.ID == c.GRADUATE_SCHOOL_ID).DefaultIfEmpty()
                                   from s4 in _dbContext.SysOtherLists.AsNoTracking().Where(x => x.ID == c.MAJOR_ID).DefaultIfEmpty()
                                   from s5 in _dbContext.SysOtherLists.AsNoTracking().Where(x => x.ID == c.RC_COMPUTER_LEVEL_ID).DefaultIfEmpty()
                                   from s6 in _dbContext.SysOtherLists.AsNoTracking().Where(x => x.ID == c.TYPE_CLASSIFICATION_ID).DefaultIfEmpty()
                                   from s7 in _dbContext.SysOtherLists.AsNoTracking().Where(x => x.ID == c.LANGUAGE_ID).DefaultIfEmpty()
                                   from s8 in _dbContext.SysOtherLists.AsNoTracking().Where(x => x.ID == c.LANGUAGE_LEVEL_ID).DefaultIfEmpty()
                                   where c.ID == candidateCvId
                                   select new
                                   {
                                       Id = candidateCvId,
                                       EducationLevel = s1.NAME,
                                       LearningLevel = s2.NAME,
                                       GraduateSchool = s3.NAME,
                                       MajorId = s4.NAME,
                                       YearGraduation = c.YEAR_GRADUATION,
                                       Rating = c.RATING,
                                       RcComputerLevel = s5.NAME,
                                       TypeClassification = s6.NAME,
                                       Language = s7.NAME,
                                       LanguageLevel = s8.NAME,
                                       Mark = c.MARK,
                                   }).SingleAsync();
                return new() { InnerBody = query };
            }
            catch (Exception ex)
            {
                return new() { InnerBody = null, MessageCode = ex.Message };
            }
        }

        public async Task<FormatedResponse> GetLevelInfoById(long id)
        {
            try
            {
                var query = await (from c in _dbContext.RcCandidateCvs
                                   from s1 in _dbContext.SysOtherLists.AsNoTracking().Where(x => x.ID == c.EDUCATION_LEVEL_ID).DefaultIfEmpty()
                                   from s2 in _dbContext.SysOtherLists.AsNoTracking().Where(x => x.ID == c.LEARNING_LEVEL_ID).DefaultIfEmpty()
                                   from s3 in _dbContext.SysOtherLists.AsNoTracking().Where(x => x.ID == c.GRADUATE_SCHOOL_ID).DefaultIfEmpty()
                                   from s4 in _dbContext.SysOtherLists.AsNoTracking().Where(x => x.ID == c.MAJOR_ID).DefaultIfEmpty()
                                   from s5 in _dbContext.SysOtherLists.AsNoTracking().Where(x => x.ID == c.RC_COMPUTER_LEVEL_ID).DefaultIfEmpty()
                                   from s6 in _dbContext.SysOtherLists.AsNoTracking().Where(x => x.ID == c.TYPE_CLASSIFICATION_ID).DefaultIfEmpty()
                                   from s7 in _dbContext.SysOtherLists.AsNoTracking().Where(x => x.ID == c.LANGUAGE_ID).DefaultIfEmpty()
                                   from s8 in _dbContext.SysOtherLists.AsNoTracking().Where(x => x.ID == c.LANGUAGE_LEVEL_ID).DefaultIfEmpty()
                                   where c.ID == id
                                   select new
                                   {
                                       Id = id,
                                       EducationLevelId = c.EDUCATION_LEVEL_ID,
                                       LearningLevelId = c.LEARNING_LEVEL_ID,
                                       GraduateSchoolId = c.GRADUATE_SCHOOL_ID,
                                       MajorId = c.MAJOR_ID,
                                       YearGraduation = c.YEAR_GRADUATION,
                                       Rating = c.RATING,
                                       RcComputerLevelId = c.RC_COMPUTER_LEVEL_ID,
                                       TypeClassificationId = c.TYPE_CLASSIFICATION_ID,
                                       LanguageId = c.LANGUAGE_ID,
                                       LanguageLevelId = c.LANGUAGE_LEVEL_ID,
                                       Mark = c.MARK,
                                   }).SingleAsync();
                return new() { InnerBody = query };
            }
            catch (Exception ex)
            {
                return new() { InnerBody = null, MessageCode = ex.Message };
            }
        }

        public async Task<FormatedResponse> UpdateLevelInfo(CandidateEditDTO request)
        {
            try
            {
                _dbContext.Database.BeginTransaction();

                var entity = _dbContext.RcCandidateCvs.Where(x => x.ID == request.Id).FirstOrDefault();
                if (entity != null)
                {
                    entity.EDUCATION_LEVEL_ID = request.EducationLevelId;
                    entity.LEARNING_LEVEL_ID = request.LearningLevelId;
                    entity.GRADUATE_SCHOOL_ID = request.GraduateSchoolId;
                    entity.MAJOR_ID = request.MajorId;
                    entity.YEAR_GRADUATION = request.YearGraduation;
                    entity.RATING = request.Rating;
                    entity.RC_COMPUTER_LEVEL_ID = request.RcComputerLevelId;
                    entity.TYPE_CLASSIFICATION_ID = request.TypeClassificationId;
                    entity.LANGUAGE_ID = request.LanguageId;
                    entity.LANGUAGE_LEVEL_ID = request.LanguageLevelId;
                    entity.MARK = request.Mark;

                    await _dbContext.SaveChangesAsync();
                    await _dbContext.Database.CommitTransactionAsync();

                    return new() { MessageCode = CommonMessageCode.UPDATE_SUCCESS, InnerBody = entity, StatusCode = EnumStatusCode.StatusCode200 };
                }
                else
                {
                    return new() { MessageCode = CommonMessageCode.ENTITY_NOT_FOUND, InnerBody = false, StatusCode = EnumStatusCode.StatusCode400 };
                }
            }
            catch (Exception ex)
            {
                await _dbContext.Database.RollbackTransactionAsync();
                await _dbContext.DisposeAsync();
                return new() { ErrorType = EnumErrorType.UNCATCHABLE, StatusCode = EnumStatusCode.StatusCode500, MessageCode = ex.Message };
            }
        }

        public async Task<FormatedResponse> GetWish(long candidateCvId)
        {
            try
            {
                var query = await (from c in _dbContext.RcCandidateCvs
                                   from  p1 in _dbContext.HuPositions.AsNoTracking().Where(x => x.ID == c.POS_WISH1_ID).DefaultIfEmpty()
                                   from  p2 in _dbContext.HuPositions.AsNoTracking().Where(x => x.ID == c.POS_WISH2_ID).DefaultIfEmpty()
                                   where c.ID == candidateCvId
                                   select new
                                   {
                                       Id = candidateCvId,
                                       PosWish1 = p1.NAME,
                                       PosWish2 = p2.NAME,
                                       ProbationSalary = c.PROBATION_SALARY,
                                       WishSalary = c.WISH_SALARY,
                                       DesiredWorkplace = c.DESIRED_WORKPLACE,
                                       StartDateWork = c.START_DATE_WORK,
                                       LevelDesired = c.LEVEL_DESIRED,
                                       NumExperience = c.NUM_EXPERIENCE,
                                       IsHsvHv = c.IS_HSV_HV!.Value == true ? "Đã từng làm HSV/HV" : "Chưa từng làm HSV/HV",
                                       OtherSuggestions = c.OTHER_SUGGESTIONS,
                                   }).SingleAsync();
                return new() { InnerBody = query };
            }
            catch (Exception ex)
            {
                return new() { InnerBody = null, MessageCode = ex.Message };
            }
        }

        public async Task<FormatedResponse> GetWishById(long id)
        {
            try
            {
                var query = await (from c in _dbContext.RcCandidateCvs
                                   from p1 in _dbContext.HuPositions.AsNoTracking().Where(x => x.ID == c.POS_WISH1_ID).DefaultIfEmpty()
                                   from p2 in _dbContext.HuPositions.AsNoTracking().Where(x => x.ID == c.POS_WISH2_ID).DefaultIfEmpty()
                                   where c.ID == id
                                   select new
                                   {
                                       Id = id,
                                       PosWish1 = c.POS_WISH1_ID,
                                       PosWish2 = c.POS_WISH2_ID,
                                       ProbationSalary = c.PROBATION_SALARY,
                                       WishSalary = c.WISH_SALARY,
                                       DesiredWorkplace = c.DESIRED_WORKPLACE,
                                       StartDateWork = c.START_DATE_WORK,
                                       LevelDesired = c.LEVEL_DESIRED,
                                       NumExperience = c.NUM_EXPERIENCE,
                                       IsHsvHv = c.IS_HSV_HV,
                                       OtherSuggestions = c.OTHER_SUGGESTIONS,
                                   }).SingleAsync();
                return new() { InnerBody = query };
            }
            catch (Exception ex)
            {
                return new() { InnerBody = null, MessageCode = ex.Message };
            }
        }

        public async Task<FormatedResponse> UpdateWish(CandidateEditDTO request)
        {
            try
            {
                _dbContext.Database.BeginTransaction();

                var entity = _dbContext.RcCandidateCvs.Where(x => x.ID == request.Id).FirstOrDefault();
                if (entity != null)
                {
                    entity.POS_WISH1_ID =  request.PosWish1Id;
                    entity.POS_WISH2_ID =  request.PosWish2Id;
                    entity.PROBATION_SALARY = request.ProbationSalary;
                    entity.WISH_SALARY =  request.WishSalary;
                    entity.DESIRED_WORKPLACE =  request.DesiredWorkplace;
                    entity.START_DATE_WORK =  request.StartDateWork;
                    entity.LEVEL_DESIRED =  request.LevelDesired;
                    entity.NUM_EXPERIENCE =  request.NumExperience;
                    entity.IS_HSV_HV =  request.IsHsvHv;
                    entity.OTHER_SUGGESTIONS = request.OtherSuggestions;

                    await _dbContext.SaveChangesAsync();
                    await _dbContext.Database.CommitTransactionAsync();

                    return new() { MessageCode = CommonMessageCode.UPDATE_SUCCESS, InnerBody = entity, StatusCode = EnumStatusCode.StatusCode200 };
                }
                else
                {
                    return new() { MessageCode = CommonMessageCode.ENTITY_NOT_FOUND, InnerBody = false, StatusCode = EnumStatusCode.StatusCode400 };
                }
            }
            catch (Exception ex)
            {
                await _dbContext.Database.RollbackTransactionAsync();
                await _dbContext.DisposeAsync();
                return new() { ErrorType = EnumErrorType.UNCATCHABLE, StatusCode = EnumStatusCode.StatusCode500, MessageCode = ex.Message };
            }
        }

        public async Task<FormatedResponse> GetInfoOther(long candidateCvId)
        {
            try
            {
                var query = await (from c in _dbContext.RcCandidateCvs
                                   from s1 in _dbContext.SysOtherLists.AsNoTracking().Where(x => x.ID == c.RE_RELATIONSHIP).DefaultIfEmpty()
                                   from s2 in _dbContext.SysOtherLists.AsNoTracking().Where(x => x.ID == c.HEALTH_TYPE).DefaultIfEmpty()
                                   where c.ID == candidateCvId
                                   select new
                                   {
                                       Id = candidateCvId,
                                       ReName = c.RE_NAME,
                                       ReRelationship = s1.NAME,
                                       RePhone = c.RE_PHONE,
                                       ReAddress = c.RE_ADDRESS,
                                       InName = c.IN_NAME,
                                       InPhone = c.IN_PHONE,
                                       InNote = c.IN_NOTE,
                                       Height = c.HEIGHT,
                                       EarNoseThroat = c.EAR_NOSE_THROAT,
                                       Weight = c.WEIGHT,
                                       Dentomaxillofacial = c.DENTOMAXILLOFACIAL,
                                       BloodGroup = c.BLOOD_GROUP,
                                       Heart = c.HEART,
                                       BloodPressure = c.BLOOD_PRESSURE,
                                       LungsAndChest = c.LUNGS_AND_CHEST,
                                       LeftEyeVision = c.LEFT_EYE_VISION,
                                       RightEyeVision = c.RIGHT_EYE_VISION,
                                       HepatitisB = c.HEPATITIS_B,
                                       LeatherVenereal = c.LEATHER_VENEREAL,
                                       HealthType = s2.NAME,
                                       NoteSk = c.NOTE_SK

                                   }).SingleAsync();
                return new() { InnerBody = query };
            }
            catch (Exception ex)
            {
                return new() { InnerBody = null, MessageCode = ex.Message };
            }
        }

        public async Task<FormatedResponse> GetInfoOtherById(long id)
        {
            try
            {
                var query = await (from c in _dbContext.RcCandidateCvs
                                   from s1 in _dbContext.SysOtherLists.AsNoTracking().Where(x => x.ID == c.RE_RELATIONSHIP).DefaultIfEmpty()
                                   from s2 in _dbContext.SysOtherLists.AsNoTracking().Where(x => x.ID == c.HEALTH_TYPE).DefaultIfEmpty()
                                   where c.ID == id
                                   select new
                                   {
                                       Id = id,
                                       ReName = c.RE_NAME,
                                       ReRelationship = c.RE_RELATIONSHIP,
                                       RePhone = c.RE_PHONE,
                                       ReAddress = c.RE_ADDRESS,
                                       InName = c.IN_NAME,
                                       InPhone = c.IN_PHONE,
                                       InNote = c.IN_NOTE,
                                       Height = c.HEIGHT,
                                       EarNoseThroat = c.EAR_NOSE_THROAT,
                                       Weight = c.WEIGHT,
                                       Dentomaxillofacial = c.DENTOMAXILLOFACIAL,
                                       BloodGroup = c.BLOOD_GROUP,
                                       Heart = c.HEART,
                                       BloodPressure = c.BLOOD_PRESSURE,
                                       LungsAndChest = c.LUNGS_AND_CHEST,
                                       LeftEyeVision = c.LEFT_EYE_VISION,
                                       RightEyeVision = c.RIGHT_EYE_VISION,
                                       HepatitisB = c.HEPATITIS_B,
                                       LeatherVenereal = c.LEATHER_VENEREAL,
                                       HealthType = c.HEALTH_TYPE,
                                       NoteSk = c.NOTE_SK

                                   }).SingleAsync();
                return new() { InnerBody = query };
            }
            catch (Exception ex)
            {
                return new() { InnerBody = null, MessageCode = ex.Message };
            }
        }

        public async Task<FormatedResponse> UpdateInfoOther(CandidateEditDTO request)
        {
            try
            {
                _dbContext.Database.BeginTransaction();

                var entity = _dbContext.RcCandidateCvs.Where(x => x.ID == request.Id).FirstOrDefault();
                if (entity != null)
                {
                    entity.RE_NAME = request.ReName;
                    entity.RE_RELATIONSHIP = request.ReRelationship;
                    entity.RE_PHONE = request.RePhone;
                    entity.RE_ADDRESS = request.ReAddress;
                    entity.IN_NAME = request.InName;
                    entity.IN_PHONE = request.InPhone;
                    entity.IN_NOTE = request.InNote;
                    entity.HEIGHT = request.Height;
                    entity.EAR_NOSE_THROAT = request.EarNoseThroat;
                    entity.WEIGHT = request.Weight;
                    entity.DENTOMAXILLOFACIAL = request.Dentomaxillofacial;
                    entity.BLOOD_GROUP = request.BloodGroup;
                    entity.HEART = request.Heart;
                    entity.BLOOD_PRESSURE = request.BloodPressure;
                    entity.LUNGS_AND_CHEST = request.LungsAndChest;
                    entity.LEFT_EYE_VISION = request.LeftEyeVision;
                    entity.RIGHT_EYE_VISION = request.RightEyeVision;
                    entity.HEPATITIS_B = request.HepatitisB;
                    entity.LEATHER_VENEREAL = request.LeatherVenereal;
                    entity.HEALTH_TYPE = request.HealthType;
                    entity.NOTE_SK = request.NoteSk;

                    await _dbContext.SaveChangesAsync();
                    await _dbContext.Database.CommitTransactionAsync();

                    return new() { MessageCode = CommonMessageCode.UPDATE_SUCCESS, InnerBody = entity, StatusCode = EnumStatusCode.StatusCode200 };
                }
                else
                {
                    return new() { MessageCode = CommonMessageCode.ENTITY_NOT_FOUND, InnerBody = false, StatusCode = EnumStatusCode.StatusCode400 };
                }
            }
            catch (Exception ex)
            {
                await _dbContext.Database.RollbackTransactionAsync();
                await _dbContext.DisposeAsync();
                return new() { ErrorType = EnumErrorType.UNCATCHABLE, StatusCode = EnumStatusCode.StatusCode500, MessageCode = ex.Message };
            }
        }
    }
}

