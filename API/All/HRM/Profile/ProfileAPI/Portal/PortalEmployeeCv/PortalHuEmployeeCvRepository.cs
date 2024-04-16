using CORE.GenericUOW;
using CORE.DTO;
using API.DTO;
using CORE.Enum;
using CORE.StaticConstant;
using API.All.DbContexts;
using CORE.Extension;

namespace API.Controllers.HuEmployeeCv
{
    public class PortalHuEmployeeCvRepository : IPortalHuEmployeeCvRepository
    {
        private readonly GenericUnitOfWork _uow;
        private readonly FullDbContext _dbContext;
        private IGenericRepository<HU_EMPLOYEE_CV, HuEmployeeCvDTO> _genericRepository;
        private readonly GenericReducer<HU_EMPLOYEE_CV, HuEmployeeCvDTO> _genericReducer;

        public PortalHuEmployeeCvRepository(FullDbContext context, GenericUnitOfWork uow)
        {
            _dbContext = context;
            _uow = uow;
            _genericRepository = _uow.GenericRepository<HU_EMPLOYEE_CV, HuEmployeeCvDTO>();
            _genericReducer = new();
        }

        public async Task<GenericPhaseTwoListResponse<HuEmployeeCvDTO>> SinglePhaseQueryList(GenericQueryListDTO<HuEmployeeCvDTO> request)
        {
            var joined = from e in _dbContext.HuEmployees.AsNoTracking()
                         from cv in _dbContext.HuEmployeeCvs.AsNoTracking().Where(cv => cv.ID == e.PROFILE_ID).DefaultIfEmpty()
                         from p in _dbContext.HuPositions.Where(x => x.ID == e.POSITION_ID).DefaultIfEmpty()
                         from j in _dbContext.HuJobs.Where(x => x.ID == p.JOB_ID).DefaultIfEmpty()

                         select new HuEmployeeCvDTO()
                         {
                             Id = cv.ID,
                             FullName = cv.FULL_NAME,
                             LastName = cv.LAST_NAME,
                             Avatar = cv.AVATAR,
                             JobName = j.NAME_VN,
                         };

            var singlePhaseResult = await _genericReducer.SinglePhaseReduce(joined, request);
            return singlePhaseResult;
        }

        public async Task<FormatedResponse> ReadAll()
        {
            try
            {
                var response = await (from cv in _dbContext.HuEmployeeCvs
                                      from e in _dbContext.HuEmployees.Where(x => x.PROFILE_ID == cv.ID).DefaultIfEmpty()
                                      from p in _dbContext.HuPositions.Where(x => x.ID == e.POSITION_ID).DefaultIfEmpty()
                                      from j in _dbContext.HuJobs.Where(x => x.ID == p.JOB_ID).DefaultIfEmpty()
                                      select new
                                      {
                                          Id = cv.ID,
                                          Name = cv.FULL_NAME,
                                          Avatar = cv.AVATAR,
                                          JobName = j.NAME_VN,

                                      }).ToListAsync();
                return new FormatedResponse() { InnerBody = response };
            }
            catch (Exception ex)
            {

                throw;
            }
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
        public async Task<FormatedResponse> GetContractDetail(long employeeId)
        {
            var response = await (from cv in _dbContext.HuEmployeeCvs
                                  from e in _dbContext.HuEmployees.Where(x => x.PROFILE_ID == cv.ID).DefaultIfEmpty()
                                  from o in _dbContext.SysOtherLists.Where(x => x.ID == cv.GENDER_ID).DefaultIfEmpty()
                                  from p in _dbContext.HuPositions.Where(x => x.ID == e.POSITION_ID).DefaultIfEmpty()
                                  from org in _dbContext.HuOrganizations.Where(x => x.ID == p.ORG_ID).DefaultIfEmpty()
                                  from j in _dbContext.HuJobs.Where(x => x.ID == p.JOB_ID).DefaultIfEmpty()
                                  from s in _dbContext.SysOtherLists.Where(x => x.ID == e.WORK_STATUS_ID).DefaultIfEmpty()
                                  where cv.ID == employeeId
                                  select new
                                  {
                                      Id = cv.ID,
                                      Avatar = cv.AVATAR,
                                      Name = cv.FULL_NAME,
                                      JobName = j.NAME_VN,
                                      Email = cv.EMAIL,
                                      Phone = cv.MOBILE_PHONE,
                                      EmployeeCode = e.CODE,
                                      PositionName = p.NAME,
                                      OrgName = org.NAME,
                                      Birthdate = cv.BIRTH_DATE,
                                      Gender = o.NAME,
                                      StatusName = s.NAME
                                  }).SingleAsync();
            return new FormatedResponse() { InnerBody = response };
        }

        public virtual async Task<FormatedResponse> ToggleActiveIds(GenericUnitOfWork _uow, List<long> ids, bool valueToBind, string sid)
        {
            await Task.Run(() => null);
            throw new NotImplementedException();
        }

        public virtual async Task<FormatedResponse> ComingSoonBirthdayList()
        {
            try
            {
                var now = DateTime.UtcNow;
                var thisMonth = now.Month;
                var today = now.Day;
                var list = await (from e in _dbContext.HuEmployees.AsNoTracking()
                           from h1 in _dbContext.SysOtherLists.AsNoTracking().Where(h1 => h1.ID == e.WORK_STATUS_ID).DefaultIfEmpty()
                           from i1 in _dbContext.SysOtherListTypes.AsNoTracking().Where(i1 => i1.ID == h1.TYPE_ID).DefaultIfEmpty()
                           from cv in _dbContext.HuEmployeeCvs.AsNoTracking().Where(cv => cv.ID == e.PROFILE_ID).DefaultIfEmpty()

                           where i1.CODE == "EMP_STATUS" && h1.CODE == "ESW"
                           && ((DateTime)cv.BIRTH_DATE!).Month == thisMonth && ((DateTime)cv.BIRTH_DATE!).Day >= today

                            select new
                           {
                               EmployeeId = e.ID,
                               FullName = cv.FULL_NAME,
                               Avatar = cv.AVATAR,
                               ComingDate = cv.BIRTH_DATE,
                               ProfileId = cv.ID
                           }).OrderBy(x => x.ComingDate!.Value.Day).ToListAsync();

                return new()
                {
                    InnerBody = list
                };

            } catch (Exception ex)
            {
                return new()
                {
                    ErrorType = EnumErrorType.UNCATCHABLE,
                    StatusCode = EnumStatusCode.StatusCode500,
                    MessageCode = ex.Message
                };
            }
        }

    }
}

