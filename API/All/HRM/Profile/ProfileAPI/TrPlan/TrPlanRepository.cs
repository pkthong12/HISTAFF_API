using CORE.GenericUOW;
using CORE.DTO;
using API.DTO;
using CORE.Enum;
using CORE.StaticConstant;
using API.All.DbContexts;
using CORE.AutoMapper;

namespace API.Controllers.TrPlan
{
    public class TrPlanRepository : ITrPlanRepository
    {
        private readonly GenericUnitOfWork _uow;
        private readonly FullDbContext _dbContext;
        private IGenericRepository<TR_PLAN, TrPlanDTO> _genericRepository;
        private readonly GenericReducer<TR_PLAN, TrPlanDTO> _genericReducer;

        public TrPlanRepository(FullDbContext context, GenericUnitOfWork uow)
        {
            _dbContext = context;
            _uow = uow;
            _genericRepository = _uow.GenericRepository<TR_PLAN, TrPlanDTO>();
            _genericReducer = new();
        }

        public async Task<GenericPhaseTwoListResponse<TrPlanDTO>> SinglePhaseQueryList(GenericQueryListDTO<TrPlanDTO> request)
        {

            var joined = from p in _dbContext.TrPlans.AsNoTracking()
                         from o in _dbContext.HuOrganizations.AsNoTracking().Where(x => x.ID == p.ORG_ID).DefaultIfEmpty()
                         from ce in _dbContext.TrCenters.AsNoTracking().Where(x => x.ID == p.CENTER_ID).DefaultIfEmpty()
                         from co in _dbContext.TrCourses.AsNoTracking().Where(x => x.ID == p.COURSE_ID).DefaultIfEmpty()
                         select new TrPlanDTO()
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
                             FormTraining = p.FORM_TRAINING,
                             AddressTraining = p.ADDRESS_TRAINING,
                             Note = p.NOTE,
                             Attachment = p.FILENAME,
                            
                             
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
                var list = new List<TR_PLAN>
                    {
                        (TR_PLAN)response
                    };
                var joined = (from l in list
                                  // JOIN OTHER ENTITIES BASED ON THE BUSINESS
                                  /* from o in _dbContext.HuOrganizations.AsNoTracking().DefaultIfEmpty()
                                   from ce in _dbContext.TrCenters.AsNoTracking().DefaultIfEmpty()
                                   from co in _dbContext.TrCourses.AsNoTracking().DefaultIfEmpty()
                                   where l.CENTER_ID == ce.ID && l.COURSE_ID == co.ID && l.ORG_ID == o.ID*/
                                  /*from uc in _dbContext.SysUsers.AsNoTracking().Where(x => x.ID == l.CREATED_BY).DefaultIfEmpty()
                                  from uu in _dbContext.SysUsers.AsNoTracking().Where(x => x.ID == l.UPDATED_BY).DefaultIfEmpty()*/
                              select new TrPlanDTO
                              {
                                  Id = l.ID,
                                  Code =l.CODE,
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
                                  FormTraining = l.FORM_TRAINING,
                                  AddressTraining = l.ADDRESS_TRAINING,
                                  Note = l.NOTE,
                                  Attachment = l.FILENAME,
                                  CreatedDate = l.CREATED_DATE,
                                  UpdatedDate = l.UPDATED_DATE,

                                  /*CreatedByUsername = uc.USERNAME,
                                  UpdatedByUsername = uu.USERNAME,*/


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
            return new FormatedResponse() { InnerBody = result};
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
    }
}

