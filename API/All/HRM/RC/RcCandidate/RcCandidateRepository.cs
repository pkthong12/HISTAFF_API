using CORE.GenericUOW;
using CORE.DTO;
using API.DTO;
using CORE.Enum;
using CORE.StaticConstant;
using API.All.DbContexts;
using CORE.AutoMapper;

using System.Security.Cryptography.Xml;
using Common.Repositories;

namespace API.Controllers.RcCandidate
{
    public class RcCandidateRepository : RepositoryBase<RC_CANDIDATE>, IRcCandidateRepository 
    {
        private readonly GenericUnitOfWork _uow;
        private readonly FullDbContext _dbContext;
        private IGenericRepository<RC_CANDIDATE, RcCandidateDTO> _genericRepository;
        private readonly GenericReducer<RC_CANDIDATE, RcCandidateDTO> _genericReducer;

        public RcCandidateRepository(FullDbContext context, GenericUnitOfWork uow) : base(context)
        {
            _dbContext = context;
            _uow = uow;
            _genericRepository = _uow.GenericRepository<RC_CANDIDATE, RcCandidateDTO>();
            _genericReducer = new();
        }

        public async Task<GenericPhaseTwoListResponse<RcCandidateDTO>> SinglePhaseQueryList(GenericQueryListDTO<RcCandidateDTO> request)
        {
            var joined = from p in _dbContext.RcCandidates.AsNoTracking()
                         from g in _dbContext.SysOtherLists.AsNoTracking().Where(x => x.ID == p.GENDER_ID).DefaultIfEmpty()
                         from s in _dbContext.SysOtherLists.AsNoTracking().Where(x => x.ID == p.RC_SOURCE_REC_ID).DefaultIfEmpty()
                         from o in _dbContext.HuOrganizations.AsNoTracking().Where(x => x.ID == p.ORG_ID).DefaultIfEmpty()
                         from v in _dbContext.HuPositions.AsNoTracking().Where(x => x.ID == p.WANTED_LOCATION1).DefaultIfEmpty()
                         from v2 in _dbContext.HuPositions.AsNoTracking().Where(x => x.ID == p.WANTED_LOCATION2).DefaultIfEmpty()
                         from a in _dbContext.RcCandidateCvs.AsNoTracking().Where(x => x.CANDIDATE_ID == p.ID).DefaultIfEmpty()
                         select new RcCandidateDTO
                         {
                             Id = p.ID,
                             Avatar = a.IMAGE,
                             CandidateCode = p.CANDIDATE_CODE,
                             FullnameVn = p.FULLNAME_VN,
                             GenderName = g.NAME,
                             RcSourceRecId = p.RC_SOURCE_REC_ID,
                             RcSourceRecName = s.NAME,
                             OrgId = p.ORG_ID,
                             //CompanyName = (_dbContext.HuCompanys.AsNoTracking().First(x => x.ID == p.ORG_ID).NAME_VN),
                             WantedLocation1Name = v.NAME,
                             WantedLocation2Name = v.NAME,
                             LevelSalaryWish = a.WISH_SALARY,
                             WorkPermitNo = p.WORK_PERMIT_NO,
                             PermitStartDate = p.PERMIT_START_DATE,
                             PermitEndDate = p.PERMIT_END_DATE,
                             IsWorkPermit = p.IS_WORK_PERMIT,
                             UpdatedDate = p.UPDATED_DATE,
                             StatusId = p.STATUS_ID,
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
                var joined = await (from c in _dbContext.RcCandidates.AsNoTracking().Where(x => x.ID == id)
                                    //from c.Profile! in _dbContext.RcCandidatec.Profile!s.AsNoTracking().Where(x => x.ID == c.PROFILE_ID).DefaultIfEmpty()
                                    from org in _dbContext.HuOrganizations.AsNoTracking().Where(x => x.ID == c.ORG_ID).DefaultIfEmpty()
                                    from pos1 in _dbContext.HuPositions.AsNoTracking().Where(x => x.ID == c.Profile!.POS_WISH1_ID).DefaultIfEmpty()
                                    from pos2 in _dbContext.HuPositions.AsNoTracking().Where(x => x.ID == c.Profile!.POS_WISH2_ID).DefaultIfEmpty()
                                    //from s1 in _dbContext.SysOtherLists.AsNoTracking().Where(x => x.ID == c.GENDER_ID).DefaultIfEmpty()
                                    //from s2 in _dbContext.SysOtherLists.AsNoTracking().Where(x => x.ID == c.RC_SOURCE_REC_ID).DefaultIfEmpty()
                                    //from s3 in _dbContext.SysOtherLists.AsNoTracking().Where(x => x.ID == c.Profile!.MARITAL_STATUS).DefaultIfEmpty()
                                    select new  
                                    {
                                        Id = c.ID,
                                        ProfileId = c.PROFILE_ID,
                                        Avatar = c.Profile!.IMAGE,
                                        CandidateCode = c.CANDIDATE_CODE,
                                        FullnameVn = c.FULLNAME_VN,
                                        OrgId = c.ORG_ID,
                                        OrgName = org.NAME,
                                        RcSourceRecId = c.RC_SOURCE_REC_ID,
                                        GenderId = c.GENDER_ID,
                                        MaritalStatus = c.Profile!.MARITAL_STATUS,
                                        NationId = c.Profile!.NATION_ID,
                                        ReligionId = c.Profile!.RELIGION_ID,
                                        NationalityId = c.Profile!.NATIONALITY_ID,
                                        BirthDate = c.Profile!.BIRTH_DATE,
                                        BirthAddress = c.Profile!.BIRTH_ADDRESS,
                                        IdNo = c.Profile!.ID_NO,
                                        IdDate = c.Profile!.ID_DATE,
                                        IdDateExpire = c.Profile!.ID_DATE_EXPIRE,
                                        IdPlace = c.Profile!.ID_PLACE,
                                        PerAddress = c.Profile!.PER_ADDRESS,
                                        PerProvince = c.Profile!.PER_PROVINCE,
                                        PerDistrict = c.Profile!.PER_DISTRICT,
                                        PerWard = c.Profile!.PER_WARD,
                                        ContactAddressTemp = c.Profile!.CONTACT_ADDRESS_TEMP,
                                        ContactProvinceTemp = c.Profile!.CONTACT_PROVINCE_TEMP,
                                        ContactDistrictTemp = c.Profile!.CONTACT_DISTRICT_TEMP,
                                        ContactWardTemp = c.Profile!.CONTACT_WARD_TEMP,
                                        PerEmail = c.Profile!.PER_EMAIL,
                                        MobilePhone = c.Profile!.MOBILE_PHONE,
                                        FinderSdt = c.Profile!.FINDER_SDT,
                                        IsWorkPermit = c.Profile!.IS_WORK_PERMIT,
                                        EducationLevelId = c.Profile!.EDUCATION_LEVEL_ID,
                                        LearningLevelId = c.Profile!.LEARNING_LEVEL_ID,
                                        GraduateSchoolId = c.Profile!.GRADUATE_SCHOOL_ID,
                                        MajorId = c.Profile!.MAJOR_ID,
                                        YearGraduation = c.Profile!.YEAR_GRADUATION,
                                        Rating = c.Profile!.RATING,
                                        RcComputerLevelId = c.Profile!.RC_COMPUTER_LEVEL_ID,
                                        TypeClassificationId = c.Profile!.TYPE_CLASSIFICATION_ID,
                                        LanguageId = c.Profile!.LANGUAGE_ID,
                                        LanguageLevelId = c.Profile!.LANGUAGE_LEVEL_ID,
                                        Mark = c.Profile!.MARK,
                                        PosWish1Id = c.Profile!.POS_WISH1_ID,
                                        PosWish2Id = c.Profile!.POS_WISH2_ID,
                                        ProbationSalary = c.Profile!.PROBATION_SALARY,
                                        WishSalary = c.Profile!.WISH_SALARY,
                                        DesiredWorkplace = c.Profile!.DESIRED_WORKPLACE,
                                        StartDateWork = c.Profile!.START_DATE_WORK,
                                        LevelDesired = c.Profile!.LEVEL_DESIRED,
                                        NumExperience = c.Profile!.NUM_EXPERIENCE,
                                        IsHsvHv = c.Profile!.IS_HSV_HV,
                                        OtherSuggestions = c.Profile!.OTHER_SUGGESTIONS,
                                        ReName = c.Profile!.RE_NAME,
                                        ReRelationship = c.Profile!.RE_RELATIONSHIP,
                                        RePhone = c.Profile!.RE_PHONE,
                                        ReAddress = c.Profile!.RE_ADDRESS,
                                        InName = c.Profile!.IN_NAME,
                                        InPhone = c.Profile!.IN_PHONE,
                                        InNote = c.Profile!.IN_NOTE,
                                        Height = c.Profile!.HEIGHT,
                                        EarNoseThroat = c.Profile!.EAR_NOSE_THROAT,
                                        Weight = c.Profile!.WEIGHT,
                                        Dentomaxillofacial = c.Profile!.DENTOMAXILLOFACIAL,
                                        BloodGroup = c.Profile!.BLOOD_GROUP,
                                        Heart = c.Profile!.HEART,
                                        BloodPressure = c.Profile!.BLOOD_PRESSURE,
                                        LungsAndChest = c.Profile!.LUNGS_AND_CHEST,
                                        LeftEyeVision = c.Profile!.LEFT_EYE_VISION,
                                        RightEyeVision = c.Profile!.RIGHT_EYE_VISION,
                                        HepatitisB = c.Profile!.HEPATITIS_B,
                                        LeatherVenereal = c.Profile!.LEATHER_VENEREAL,
                                        HealthType = c.Profile!.HEALTH_TYPE,
                                        NoteSk = c.Profile!.NOTE_SK,
                                    }).FirstOrDefaultAsync();
            if(joined != null)
            {
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

        public async Task<FormatedResponse> Create(GenericUnitOfWork _uow, RcCandidateDTO dto, string sid)
        {
            var response = await _genericRepository.Create(_uow, dto, sid);
            return response;
        }

        public async Task<FormatedResponse> InsertProfileRecruitment(GenericUnitOfWork _uow, CandidateEditDTO request, string sid)
        {
            try
            {
                _uow.CreateTransaction();
            
                if (request.BirthDate.HasValue && ((DateTime.Now.Year - request.BirthDate.Value.Year) < 15))
                {
                    return new FormatedResponse() { MessageCode = CommonMessageCode.BIRTH_DAY_NOT_BIGGER_THAN_DATE_TIME_NOW, ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400 };
                }
                var rcCandidate = Map(request, new RC_CANDIDATE());
                rcCandidate.CREATED_BY = sid;
                rcCandidate.CREATED_DATE = DateTime.UtcNow;
                await _dbContext.RcCandidates.AddAsync(rcCandidate);
                await _dbContext.SaveChangesAsync();

                var d = _dbContext.RcCandidateCvs.Where(x => x.CANDIDATE_ID == request.Id).ToList();
                if (d != null)
                {
                    _dbContext.RcCandidateCvs.RemoveRange(d);
                }
                var rcCandidateCv = Map(request, new RC_CANDIDATE_CV());
                rcCandidateCv.CANDIDATE_ID = rcCandidate.ID;
                rcCandidateCv.CREATED_BY = sid;
                rcCandidateCv.CREATED_DATE = DateTime.UtcNow;
                await _dbContext.RcCandidateCvs.AddAsync(rcCandidateCv);
                await _dbContext.SaveChangesAsync();


                _dbContext.Database.CommitTransaction();
                return new FormatedResponse() { InnerBody = rcCandidate };
            }
            catch(Exception ex)
            {
                _dbContext.Database.RollbackTransaction();
                return new() { ErrorType = EnumErrorType.UNCATCHABLE, StatusCode = EnumStatusCode.StatusCode500, MessageCode = ex.Message };
            }

        }

        public async Task<FormatedResponse> CreateRange(GenericUnitOfWork _uow, List<RcCandidateDTO> dtos, string sid)
        {
            var add = new List<RcCandidateDTO>();
            add.AddRange(dtos);
            var response = await _genericRepository.CreateRange(_uow, add, sid);
            return response;
        }

        public async Task<FormatedResponse> Update(GenericUnitOfWork _uow, RcCandidateDTO dto, string sid, bool patchMode = true)
        {
            var response = await _genericRepository.Update(_uow, dto, sid, patchMode);
            return response;
        }

        public async Task<FormatedResponse> UpdateRange(GenericUnitOfWork _uow, List<RcCandidateDTO> dtos, string sid, bool patchMode = true)
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
    }
}

