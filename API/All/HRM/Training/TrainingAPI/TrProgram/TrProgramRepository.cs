using CORE.GenericUOW;
using CORE.DTO;
using API.DTO;
using CORE.Enum;
using CORE.StaticConstant;
using API.All.DbContexts;
using CORE.AutoMapper;
using Org.BouncyCastle.Tls;


namespace API.Controllers.TrProgram
{
    public class TrProgramRepository : ITrProgramRepository
    {
        private readonly GenericUnitOfWork _uow;
        private readonly FullDbContext _dbContext;
        private IGenericRepository<TR_PROGRAM, TrProgramDTO> _genericRepository;
        private readonly GenericReducer<TR_PROGRAM, TrProgramDTO> _genericReducer;

        public TrProgramRepository(FullDbContext context, GenericUnitOfWork uow)
        {
            _dbContext = context;
            _uow = uow;
            _genericRepository = _uow.GenericRepository<TR_PROGRAM, TrProgramDTO>();
            _genericReducer = new();
        }

        public async Task<GenericPhaseTwoListResponse<TrProgramDTO>> SinglePhaseQueryList(GenericQueryListDTO<TrProgramDTO> request)
        {
            var joined = (from p in _dbContext.TrPrograms.AsNoTracking()
                             // JOIN OTHER ENTITIES BASED ON THE BUSINESS
                         from sys in _dbContext.SysOtherLists.AsNoTracking().Where(x => x.ID == p.TR_TYPE_ID).DefaultIfEmpty()
                         from sys2 in _dbContext.SysOtherLists.AsNoTracking().Where(x => x.ID == p.PROPERTIES_NEED_ID).DefaultIfEmpty()
                         from course in _dbContext.TrCourses.AsNoTracking().Where(x => x.ID == p.TR_COURSE_ID).DefaultIfEmpty()

                         select new TrProgramDTO
                         {
                             Id = p.ID,
                             Centers = p.CENTERS,
                             TrProgramCode = p.TR_PROGRAM_CODE,
                             Year = p.YEAR,
                             TrTypeId = p.TR_TYPE_ID,
                             TrTypeName = sys.NAME,
                             TrTrainField = p.TR_TRAIN_FIELD,
                             PlanName = p.IS_PLAN == true ? "Theo nhu cầu" : "Đột xuất",
                             TrCourseName = course.COURSE_NAME,
                             PropertiesNeedName = sys2.NAME,
                             Content = p.CONTENT,
                             StartDate = p.START_DATE,
                             EndDate = p.END_DATE,
                             StudentNumber = p.STUDENT_NUMBER,
                             CostStudent = p.COST_STUDENT,
                             Certificate = p.CERTIFICATE,
                             TrAfterTrain = p.TR_AFTER_TRAIN,
                             TrCommit = p.TR_COMMIT,
                             
                         });

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
            var joined = await (from l in _dbContext.TrPrograms.AsNoTracking().Where(x => x.ID == id)
                                from sys in _dbContext.SysOtherLists.AsNoTracking().Where(x => x.ID == l.TR_TYPE_ID).DefaultIfEmpty()
                                from sys2 in _dbContext.SysOtherLists.AsNoTracking().Where(x => x.ID == l.PROPERTIES_NEED_ID).DefaultIfEmpty()
                                from course in _dbContext.TrCourses.AsNoTracking().Where(x => x.ID == l.TR_COURSE_ID).DefaultIfEmpty()
                                select new TrProgramDTO
                                {
                                    Id = l.ID,
                                    Year = l.YEAR,
                                    OrgId = l.ORG_ID,
                                    IsPlan = l.IS_PLAN,
                                    IsPlanDx = l.IS_PLAN == true ? false : true,
                                    TrProgramCode = l.TR_PROGRAM_CODE,
                                    TrCourseId = l.TR_COURSE_ID,
                                    TrCourseName = course.COURSE_NAME,
                                    TrTrainField = l.TR_TRAIN_FIELD,
                                    TrainFormId = l.TRAIN_FORM_ID,
                                    PropertiesNeedId = l.PROPERTIES_NEED_ID,
                                    Venue = l.VENUE,
                                    Content = l.CONTENT,
                                    TargetTrain = l.TARGET_TRAIN,
                                    Note = l.NOTE,
                                    StartDate = l.START_DATE,
                                    EndDate = l.END_DATE,
                                    IsPublic = l.IS_PUBLIC,
                                    PublicStatus = l.PUBLIC_STATUS,
                                    PortalRegistFrom = l.PORTAL_REGIST_FROM,
                                    PortalRegistTo = l.PORTAL_REGIST_TO,
                                    StudentNumber = l.STUDENT_NUMBER,
                                    ExpectClass = l.EXPECT_CLASS,
                                    CostStudent = l.COST_STUDENT,
                                    TrCurrencyId = l.TR_CURRENCY_ID,
                                    AttachedFile = l.ATTACHED_FILE,
                                    Certificate = l.CERTIFICATE,
                                    TrAfterTrain = l.TR_AFTER_TRAIN,
                                    TrCommit = l.TR_COMMIT,
                                    TrTypeId = l.TR_TYPE_ID,
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

        public async Task<FormatedResponse> Create(GenericUnitOfWork _uow, TrProgramDTO dto, string sid)
        {
            dto.Centers = string.Join(";",dto.ListCenter!);
            dto.Lectures = string.Join(";",dto.ListLecture!);
            var response = await _genericRepository.Create(_uow, dto, sid);
            return response;
        }

        public async Task<FormatedResponse> CreateRange(GenericUnitOfWork _uow, List<TrProgramDTO> dtos, string sid)
        {
            var add = new List<TrProgramDTO>();
            add.AddRange(dtos);
            var response = await _genericRepository.CreateRange(_uow, add, sid);
            return response;
        }

        public async Task<FormatedResponse> Update(GenericUnitOfWork _uow, TrProgramDTO dto, string sid, bool patchMode = true)
        {
            var response = await _genericRepository.Update(_uow, dto, sid, patchMode);
            return response;
        }

        public async Task<FormatedResponse> UpdateRange(GenericUnitOfWork _uow, List<TrProgramDTO> dtos, string sid, bool patchMode = true)
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

        public async Task<FormatedResponse> GetListProgram()
        {
            var queryable = await (from p in _dbContext.TrPrograms
                                   from course in _dbContext.TrCourses.AsNoTracking().Where(x => x.ID == p.TR_COURSE_ID).DefaultIfEmpty()
                                   select new
                                   {
                                       Id = p.ID,
                                       Code = p.TR_PROGRAM_CODE,
                                       Name = "[" + p.TR_PROGRAM_CODE + "] " + course.COURSE_NAME,
                                   }).ToListAsync();
            return new() { InnerBody = queryable };
        }
    }
}

