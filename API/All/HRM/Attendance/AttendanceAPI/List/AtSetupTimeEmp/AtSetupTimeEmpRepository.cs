using CORE.GenericUOW;
using CORE.DTO;
using API.DTO;
using CORE.Enum;
using CORE.StaticConstant;
using API.All.DbContexts;
using CORE.AutoMapper;
using System;
using System.Linq.Dynamic.Core;

namespace API.Controllers.AtSetupTimeEmp
{
    public class AtSetupTimeEmpRepository : IAtSetupTimeEmpRepository
    {
        private readonly GenericUnitOfWork _uow;
        private readonly FullDbContext _dbContext;
        private IGenericRepository<AT_SETUP_TIME_EMP, AtSetupTimeEmpDTO> _genericRepository;
        private readonly GenericReducer<AT_SETUP_TIME_EMP, AtSetupTimeEmpDTO> _genericReducer;

        public AtSetupTimeEmpRepository(FullDbContext context, GenericUnitOfWork uow)
        {
            _dbContext = context;
            _uow = uow;
            _genericRepository = _uow.GenericRepository<AT_SETUP_TIME_EMP, AtSetupTimeEmpDTO>();
            _genericReducer = new();
        }

        public async Task<GenericPhaseTwoListResponse<AtSetupTimeEmpDTO>> SinglePhaseQueryList(GenericQueryListDTO<AtSetupTimeEmpDTO> request)
        {
            var joined = from s in _dbContext.AtSetupTimeEmps.AsNoTracking()
                         from e in _dbContext.HuEmployees.AsNoTracking().Where(emp => emp.ID == s.EMPLOYEE_ID).DefaultIfEmpty()
                         from p in _dbContext.HuPositions.AsNoTracking().Where(pos => pos.ID == e.POSITION_ID).DefaultIfEmpty()
                         from j in _dbContext.HuJobs.AsNoTracking().Where(j => j.ID == p.JOB_ID).DefaultIfEmpty()
                             // JOIN OTHER ENTITIES BASED ON THE BUSINESS
                         orderby j.ORDERNUM
                         select new AtSetupTimeEmpDTO
                         {
                             Id = s.ID,
                             OrgId = e.ORG_ID,
                             EmployeeId = s.EMPLOYEE_ID,
                             EmployeeName = e.Profile!.FULL_NAME,
                             EmployeeCode = e.CODE,
                             PositionName = p.NAME,
                             NumberSwipecard = s.NUMBER_SWIPECARD,
                             IsActive = s.IS_ACTIVE,
                             Status = s.IS_ACTIVE == true ? "Áp dụng" : "Ngừng áp dụng",
                             Note = s.NOTE,
                             JobOrderNum = (int)(j.ORDERNUM ?? 999),
                         };

            var singlePhaseResult = await _genericReducer.SinglePhaseReduce(joined, request);
            return singlePhaseResult;
        }

        public async Task<FormatedResponse> ReadAll()
        {
            var response = await _genericRepository.ReadAll();
            return response;
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
            var joined = (from l in _dbContext.AtSetupTimeEmps.Where(l => l.ID == id)
                          from e in _dbContext.HuEmployees.Where(e => e.ID == l.EMPLOYEE_ID).DefaultIfEmpty()
                          from cv in _dbContext.HuEmployeeCvs.Where(cv => cv.ID == e.PROFILE_ID).DefaultIfEmpty()
                          from p in _dbContext.HuPositions.Where(p => p.ID == e.POSITION_ID).DefaultIfEmpty()
                          from c in _dbContext.SysUsers.AsNoTracking().Where(c => l.CREATED_BY == null ? false : c.ID == l.CREATED_BY).DefaultIfEmpty()
                          from u in _dbContext.SysUsers.AsNoTracking().Where(u => l.UPDATED_BY == null ? false : u.ID == l.UPDATED_BY).DefaultIfEmpty()
                          select new AtSetupTimeEmpDTO
                          {
                              Id = l.ID,
                              EmployeeId = l.EMPLOYEE_ID,
                              EmployeeName = cv.FULL_NAME,
                              EmployeeCode = e.CODE,
                              PositionName = p.NAME,
                              NumberSwipecard = l.NUMBER_SWIPECARD,
                              IsActive = l.IS_ACTIVE,
                              Status = l.IS_ACTIVE.HasValue ? "Áp dụng" : "Ngừng áp dụng",
                              Note = l.NOTE,
                              CreatedDate = l.CREATED_DATE,
                              UpdatedDate = l.UPDATED_DATE,
                              CreatedByUsername = c.USERNAME,
                              UpdatedByUsername = u.USERNAME

                          }).FirstOrDefault();

            if (joined != null)
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

        public async Task<FormatedResponse> Create(GenericUnitOfWork _uow, AtSetupTimeEmpDTO dto, string sid)
        {
            var response = await _genericRepository.Create(_uow, dto, sid);
            return response;
        }

        public async Task<FormatedResponse> CreateRange(GenericUnitOfWork _uow, List<AtSetupTimeEmpDTO> dtos, string sid)
        {
            var add = new List<AtSetupTimeEmpDTO>();
            add.AddRange(dtos);
            var response = await _genericRepository.CreateRange(_uow, add, sid);
            return response;
        }

        public async Task<FormatedResponse> Update(GenericUnitOfWork _uow, AtSetupTimeEmpDTO dto, string sid, bool patchMode = true)
        {
            var response = await _genericRepository.Update(_uow, dto, sid, patchMode);
            return response;
        }

        public async Task<FormatedResponse> UpdateRange(GenericUnitOfWork _uow, List<AtSetupTimeEmpDTO> dtos, string sid, bool patchMode = true)
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
            foreach (var id in ids)
            {
                var item = await _dbContext.AtSetupTimeEmps.Where(x => x.ID == id).AsNoTracking().SingleOrDefaultAsync();
                if (item != null && item.IS_ACTIVE == true)
                {
                    _uow.Rollback();
                    return new FormatedResponse() { ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400, MessageCode = CommonMessageCode.DO_NOT_DELETE_DATA_IS_ACTIVE };
                }
            }
            var response = await _genericRepository.DeleteIds(_uow, ids);
            return response;
        }

        public async Task<FormatedResponse> DeleteIds(GenericUnitOfWork _uow, List<string> ids)
        {
            await Task.Run(() => null);
            throw new NotImplementedException();
        }

        public virtual async Task<FormatedResponse> ToggleActiveIds(GenericUnitOfWork _uow, List<long> ids, bool valueToBind, string sid)
        {
            var response = await _genericRepository.ToggleActiveIds(_uow, ids, valueToBind, sid);
            return response;
        }

    }
}

