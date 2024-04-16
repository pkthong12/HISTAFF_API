using CORE.GenericUOW;
using CORE.DTO;
using API.DTO;
using CORE.Enum;
using CORE.StaticConstant;
using API.All.DbContexts;
using CORE.AutoMapper;
using Common.Extensions;
using CORE.Services.File;
using Microsoft.Extensions.Options;

namespace API.Controllers.TrPlan
{
    public class TrPlanRepository : ITrPlanRepository
    {
        private readonly GenericUnitOfWork _uow;
        private readonly FullDbContext _dbContext;
        private IGenericRepository<TR_PLAN, TrPlanDTO> _genericRepository;
        private readonly GenericReducer<TR_PLAN, TrPlanDTO> _genericReducer;
        private readonly IWebHostEnvironment _env;
        private readonly IFileService _fileService;
        private readonly AppSettings _appSettings;

        public TrPlanRepository(FullDbContext context, GenericUnitOfWork uow,
            IWebHostEnvironment env,
            IOptions<AppSettings> options,
            IFileService fileService)
        {
            _dbContext = context;
            _uow = uow;
            _genericRepository = _uow.GenericRepository<TR_PLAN, TrPlanDTO>();
            _genericReducer = new();
            _appSettings = options.Value;
            _fileService = fileService;
        }

        public async Task<GenericPhaseTwoListResponse<TrPlanDTO>> SinglePhaseQueryList(GenericQueryListDTO<TrPlanDTO> request)
        {

            var joined = from p in _dbContext.TrPlans.AsNoTracking()
                         from o in _dbContext.HuOrganizations.AsNoTracking().Where(x => x.ID == p.ORG_ID).DefaultIfEmpty()
                         from ce in _dbContext.TrCenters.AsNoTracking().Where(x => x.ID == p.CENTER_ID).DefaultIfEmpty()
                         from co in _dbContext.TrCourses.AsNoTracking().Where(x => x.ID == p.COURSE_ID).DefaultIfEmpty()
                         from f in _dbContext.SysOtherLists.AsNoTracking().Where(x => x.ID == p.FORM_TRAINING_ID).DefaultIfEmpty()
                         from m in _dbContext.SysOtherLists.AsNoTracking().Where(x => x.ID == p.UNIT_MONEY_ID).DefaultIfEmpty()
                         select new TrPlanDTO
                         {
                             Id = p.ID,
                             Code = p.CODE,
                             Name = p.NAME,
                             OrgId = p.ORG_ID,
                             OrgName = o.NAME,
                             Year = p.YEAR,
                             StartDatePlan = p.START_DATE_PLAN,
                             EndDatePlan = p.END_DATE_PLAN,
                             StartDateReal = p.START_DATE_REAL,
                             EndDateReal = p.END_DATE_REAL,
                             PersonNumReal = p.PERSON_NUM_REAL,
                             PersonNumPlan = p.PERSON_NUM_PLAN,
                             ExpectedCost = p.EXPECTED_COST,
                             ActualCost = p.ACTUAL_COST,
                             CourseId = p.COURSE_ID,
                             CourseName = co.COURSE_NAME,
                             CenterId = p.CENTER_ID,
                             CenterName = ce.NAME_CENTER,
                             Content = p.CONTENT,
                             FormTrainingId = p.FORM_TRAINING_ID,
                             FormTrainingName = f.NAME,
                             AddressTraining = p.ADDRESS_TRAINING,
                             Note = p.NOTE,
                             Attachment = p.FILENAME,
                             IsCertificate = p.IS_CERTIFICATE,
                             IsCommitTrain = p.IS_COMMIT_TRAIN,
                             IsPostTrain = p.IS_POST_TRAIN,
                             UnitMoneyId = p.UNIT_MONEY_ID,
                             UnitMoneyName = p.NAME,
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
            var joined = await (from l in _dbContext.TrPlans.AsNoTracking().Where(x => x.ID == id)
                                    // JOIN OTHER ENTITIES BASED ON THE BUSINESS
                                from o in _dbContext.HuOrganizations.AsNoTracking().DefaultIfEmpty()
                                from ce in _dbContext.TrCenters.AsNoTracking().Where(x => x.ID == l.CENTER_ID).DefaultIfEmpty()
                                from co in _dbContext.TrCourses.AsNoTracking().Where(x => x.ID == l.COURSE_ID).DefaultIfEmpty()
                                    //from po in _dbContext.HuPositions.AsNoTracking().Where(x => x.ID == l.POSITION_ID).DefaultIfEmpty()
                                from s in _dbContext.SysOtherLists.AsNoTracking().Where(x => x.ID == l.FORM_TRAINING_ID).DefaultIfEmpty()
                                from prop in _dbContext.SysOtherLists.AsNoTracking().Where(x => x.ID == l.PROPERTIES_NEED_ID).DefaultIfEmpty()
                                from tt in _dbContext.SysOtherLists.AsNoTracking().Where(x => x.ID == l.TYPE_TRAINING_ID).DefaultIfEmpty()
                                from m in _dbContext.SysOtherLists.AsNoTracking().Where(x => x.ID == l.UNIT_MONEY_ID).DefaultIfEmpty()
                                from t in _dbContext.SysOtherLists.AsNoTracking().Where(s => s.ID == co.TR_TRAIN_FIELD).DefaultIfEmpty()
                                select new TrPlanDTO
                                {
                                    Id = l.ID,
                                    Code = l.CODE,
                                    Name = l.NAME,
                                    OrgId = l.ORG_ID,
                                    Year = l.YEAR,
                                    StartDatePlan = l.START_DATE_PLAN,
                                    EndDatePlan = l.END_DATE_PLAN,
                                    StartDateReal = l.START_DATE_REAL,
                                    EndDateReal = l.END_DATE_REAL,
                                    PersonNumReal = l.PERSON_NUM_REAL,
                                    PersonNumPlan = l.PERSON_NUM_PLAN,
                                    ExpectedCost = l.EXPECTED_COST,

                                    ActualCost = l.ACTUAL_COST,
                                    CourseId = l.COURSE_ID,
                                    CenterId = l.CENTER_ID,
                                    Content = l.CONTENT,
                                    FormTrainingId = l.FORM_TRAINING_ID,
                                    FormTrainingName = s.NAME,
                                    AddressTraining = l.ADDRESS_TRAINING,
                                    Note = l.NOTE,
                                    Attachment = l.FILENAME,
                                    CreatedDate = l.CREATED_DATE,
                                    UpdatedDate = l.UPDATED_DATE,
                                    ExpectClass = l.EXPECT_CLASS,
                                    IsCertificate = l.IS_CERTIFICATE,
                                    IsPostTrain = l.IS_COMMIT_TRAIN,
                                    IsCommitTrain = l.IS_COMMIT_TRAIN,
                                    PropertiesNeedId = l.PROPERTIES_NEED_ID,
                                    PropertiesNeedName = prop.NAME,
                                    TypeTrainingId = l.TYPE_TRAINING_ID,
                                    TypeTrainingName = tt.NAME,
                                    CertificateName = l.CERTIFICATE_NAME,
                                    //JobName =  
                                    JobFamilyIds = l.JOB_FAMILY_IDS,
                                    //JobFamilyName = po.NAME,
                                    JobIds = l.JOB_IDS,
                                    EvaluationDueDate1 = l.EVALUATION_DUE_DATE1,
                                    EvaluationDueDate2 = l.EVALUATION_DUE_DATE3,
                                    EvaluationDueDate3 = l.EVALUATION_DUE_DATE3,
                                    UnitMoneyId = l.UNIT_MONEY_ID,
                                    UnitMoneyName = m.NAME,
                                    //TrTrainFeildName = (from t in _dbContext.TrCourses
                                    //                    from q in _dbContext.SysOtherLists.Where(x => x.ID == t.TR_TRAIN_FIELD)
                                    //                    where t.ID == l.COURSE_ID
                                    //                    select q.NAME),
                                    //PositionName = f.NAME
                                    /*CreatedByUsername = uc.USERNAME,
                                    UpdatedByUsername = uu.USERNAME,*/
                                }).FirstOrDefaultAsync();

            if (joined != null)
            {
                if (joined.JobFamilyIds != null)
                {
                    List<long?> JobF = new List<long?>();
                    var jf = joined!.JobFamilyIds!.Split(",").ToList();
                    jf.ForEach(f =>
                    {
                        if (long.TryParse(f, out long n)) { JobF.Add(n); }
                    });
                    joined.ListJobFamilyIds = JobF;
                }
                if (joined.JobIds != null)
                {
                    List<long?> Job = new List<long?>();
                    var j = joined.JobIds!.Split(",").ToList();
                    j.ForEach(j =>
                    {
                        if (long.TryParse(j, out long n)) { Job.Add(n); }
                    });
                    joined.ListJobIds = Job;
                }
                if (joined.CourseId != null)
                {
                    var a = (from t in _dbContext.TrCourses
                                               from q in _dbContext.SysOtherLists.Where(x => x.ID == t.TR_TRAIN_FIELD)
                                               where t.ID == joined.CourseId
                                               select q.NAME).FirstOrDefault();
                    joined.TrTrainFeildName = a;
                }
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

        public async Task<FormatedResponse> Create(GenericUnitOfWork _uow, TrPlanDTO dto, string sid)
        {
            var response = await _genericRepository.Create(_uow, dto, sid);
            return response;
        }

        public async Task<FormatedResponse> CreateRange(GenericUnitOfWork _uow, List<TrPlanDTO> dtos, string sid)
        {
            var add = new List<TrPlanDTO>();
            add.AddRange(dtos);
            var response = await _genericRepository.CreateRange(_uow, add, sid);
            return response;
        }

        public async Task<FormatedResponse> Update(GenericUnitOfWork _uow, TrPlanDTO dto, string sid, bool patchMode = true)
        {
            var response = await _genericRepository.Update(_uow, dto, sid, patchMode);
            return response;
        }

        public async Task<FormatedResponse> UpdateRange(GenericUnitOfWork _uow, List<TrPlanDTO> dtos, string sid, bool patchMode = true)
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

        public async Task<FormatedResponse> GetAllCoures()
        {
            var result = await (from co in _dbContext.TrCourses.AsNoTracking().DefaultIfEmpty()
                                where co.IS_ACTIVE == true
                                select new
                                {
                                    Id = co.ID,
                                    CourseName = co.COURSE_NAME,
                                }).ToListAsync();
            return new FormatedResponse() { InnerBody = result };
        }

        public async Task<FormatedResponse> GetAllCenter()
        {
            var result = await (from ce in _dbContext.TrCenters.AsNoTracking().DefaultIfEmpty()
                                where ce.IS_ACTIVE == true
                                select new { Id = ce.ID, NameCenter = ce.NAME_CENTER }).ToListAsync();
            return new FormatedResponse() { InnerBody = result };
        }

        public async Task<FormatedResponse> GetAllOrg()
        {
            var result = await (from o in _dbContext.HuOrganizations.AsNoTracking().DefaultIfEmpty()
                                select new { Id = o.ID, Name = o.NAME }).ToListAsync();
            return new FormatedResponse() { InnerBody = result };
        }

        public virtual async Task<FormatedResponse> ToggleActiveIds(GenericUnitOfWork _uow, List<long> ids, bool valueToBind, string sid)
        {
            await Task.Run(() => null);
            throw new NotImplementedException();
        }

        public async Task<FormatedResponse> GetTrainingForm()
        {
            var r = (from t in _dbContext.SysOtherListTypes where t.CODE == "TRAINING_FORM" select t.ID).FirstOrDefault();
            var query = await (from p in _dbContext.SysOtherLists.AsNoTracking().DefaultIfEmpty()
                               where p.TYPE_ID == r
                               select new
                               {
                                   Id = p.ID,
                                   NAME = p.NAME,
                               }).ToListAsync();
            return new FormatedResponse() { InnerBody = query };
        }

        public async Task<FormatedResponse> GetJobByJobFamId(List<long>? ids)
        {
            var result = await (from o in _dbContext.HuJobs.AsNoTracking()
                                where ids!.Contains(o.JOB_FAMILY_ID!.Value) && o.ACTFLG == "A"
                                select new
                                {
                                    Id = o.ID,
                                    Name = o.NAME_VN
                                }).ToListAsync();
            return new FormatedResponse() { InnerBody = result };
        }
    }
}

