using CORE.GenericUOW;
using CORE.DTO;
using API.DTO;
using CORE.Enum;
using CORE.StaticConstant;
using API.All.DbContexts;
using CORE.AutoMapper;

namespace API.Controllers.SeAuthorizeApprove
{
    public class SeAuthorizeApproveRepository : ISeAuthorizeApproveRepository
    {
        private readonly GenericUnitOfWork _uow;
        private readonly FullDbContext _dbContext;
        private IGenericRepository<SE_AUTHORIZE_APPROVE, SeAuthorizeApproveDTO> _genericRepository;
        private readonly GenericReducer<SE_AUTHORIZE_APPROVE, SeAuthorizeApproveDTO> _genericReducer;

        public SeAuthorizeApproveRepository(FullDbContext context, GenericUnitOfWork uow)
        {
            _dbContext = context;
            _uow = uow;
            _genericRepository = _uow.GenericRepository<SE_AUTHORIZE_APPROVE, SeAuthorizeApproveDTO>();
            _genericReducer = new();
        }

        public async Task<GenericPhaseTwoListResponse<SeAuthorizeApproveDTO>> SinglePhaseQueryList(GenericQueryListDTO<SeAuthorizeApproveDTO> request)
        {
            var joined = from p in _dbContext.SeAuthorizeApproves.AsNoTracking()// p.PROCESS_ID == null ? false : p.LEVEL_ORDER_ID == null ? false :
                         from e in _dbContext.HuEmployees.AsNoTracking().Where(emp => emp.ID == p.EMPLOYEE_ID).DefaultIfEmpty()
                         from sp in _dbContext.SeProcesss.AsNoTracking().Where(sp => sp.ID == p.PROCESS_ID).DefaultIfEmpty()
                         from lv in _dbContext.SysOtherLists.AsNoTracking().Where(lv => lv.ID == p.LEVEL_ORDER_ID).DefaultIfEmpty()
                         from cv in _dbContext.HuEmployeeCvs.AsNoTracking().Where(cv => cv.ID == e.PROFILE_ID).DefaultIfEmpty()
                         from ea in _dbContext.HuEmployees.AsNoTracking().Where(ea => ea.ID == p.EMPLOYEE_AUTH_ID).DefaultIfEmpty()
                         select new SeAuthorizeApproveDTO
                         {
                             Id = p.ID,
                             ProcessId = p.PROCESS_ID,
                             OrgId = e.ORG_ID,
                             EmployeeId = p.EMPLOYEE_ID,
                             EmployeeCode = e.CODE,
                             EmployeeName = cv.FULL_NAME,
                             EmployeeAuthId = p.EMPLOYEE_AUTH_ID,
                             EmployeeAuthCode = ea.CODE,
                             EmployeeAuthName = ea.Profile!.FULL_NAME,
                             LevelOrderId = p.LEVEL_ORDER_ID,
                             LevelOrderName = lv.NAME,
                             ProcessName = sp.NAME,
                             FromDate = p.FROM_DATE,
                             ToDate = p.TO_DATE,
                             IsPerReplace = p.IS_PER_REPLACE
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
            var joined = await (from p in _dbContext.SeAuthorizeApproves.AsNoTracking().Where(s => s.ID == id)
                                from sp in _dbContext.SeProcesss.AsNoTracking().Where(sp => sp.ID == p.PROCESS_ID).DefaultIfEmpty()
                                from lv in _dbContext.SysOtherLists.AsNoTracking().Where(lv => lv.ID == p.LEVEL_ORDER_ID).DefaultIfEmpty()
                                from e in _dbContext.HuEmployees.AsNoTracking().Where(emp => emp.ID == p.EMPLOYEE_ID).DefaultIfEmpty()
                                from cv in _dbContext.HuEmployeeCvs.AsNoTracking().Where(cv => cv.ID == e.PROFILE_ID).DefaultIfEmpty()
                                from ea in _dbContext.HuEmployees.AsNoTracking().Where(ea => ea.ID == p.EMPLOYEE_AUTH_ID).DefaultIfEmpty()
                                select new SeAuthorizeApproveDTO
                                {
                                    Id = p.ID,
                                    ProcessId = p.PROCESS_ID,
                                    OrgId = e.ORG_ID,
                                    EmployeeId = p.EMPLOYEE_ID,
                                    EmployeeCode = e.CODE,
                                    EmployeeName = cv.FULL_NAME,
                                    EmployeeAuthId = p.EMPLOYEE_AUTH_ID,
                                    EmployeeAuthCode = ea.CODE,
                                    EmployeeAuthName = ea.Profile!.FULL_NAME,
                                    LevelOrderId = p.LEVEL_ORDER_ID,
                                    LevelOrderName = lv.NAME,
                                    ProcessName = sp.NAME,
                                    FromDate = p.FROM_DATE,
                                    ToDate = p.TO_DATE,
                                    IsPerReplace = p.IS_PER_REPLACE
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

        public async Task<FormatedResponse> Create(GenericUnitOfWork _uow, SeAuthorizeApproveDTO dto, string sid)
        {
            try
            {
                var response = await _genericRepository.Create(_uow, dto, sid);
                return response;
            }
            catch(Exception ex)
            {
                return new FormatedResponse() {  MessageCode = ex.Message, StatusCode = EnumStatusCode.StatusCode400 };
            }
            
        }

        public async Task<FormatedResponse> CreateRange(GenericUnitOfWork _uow, List<SeAuthorizeApproveDTO> dtos, string sid)
        {
            var add = new List<SeAuthorizeApproveDTO>();
            add.AddRange(dtos);
            var response = await _genericRepository.CreateRange(_uow, add, sid);
            return response;
        }

        public async Task<FormatedResponse> Update(GenericUnitOfWork _uow, SeAuthorizeApproveDTO dto, string sid, bool patchMode = true)
        {
            
            try
            {
                var response = await _genericRepository.Update(_uow, dto, sid, patchMode);
                return response;
            }
            catch (Exception ex)
            {
                return new FormatedResponse() { MessageCode = ex.Message, StatusCode = EnumStatusCode.StatusCode400 };
            }
        }

        public async Task<FormatedResponse> UpdateRange(GenericUnitOfWork _uow, List<SeAuthorizeApproveDTO> dtos, string sid, bool patchMode = true)
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

        public async Task<FormatedResponse> GetLevelOrder()
        {
            var joined = await (from p in _dbContext.SysOtherListTypes.AsNoTracking().Where(p=> p.CODE == "LEVEL_ORDER").DefaultIfEmpty()
                                from c in _dbContext.SysOtherLists.AsNoTracking().Where(c=> c.TYPE_ID == p.ID).DefaultIfEmpty()
                                    // JOIN OTHER ENTITIES BASED ON THE BUSINESS
                                select new
                                {
                                    Id = c.ID,
                                    Name = c.NAME,
                                }).ToListAsync();
            return new FormatedResponse() { InnerBody = joined };
        }
        public async Task<FormatedResponse> GetLevelOrderById(long id)
        {
            var joined = await (from p in _dbContext.SysOtherListTypes.AsNoTracking().Where(p => p.CODE == "LEVEL_ORDER").DefaultIfEmpty()
                                from c in _dbContext.SysOtherLists.AsNoTracking().Where(c => c.TYPE_ID == p.ID && c.ID == id).DefaultIfEmpty()
                                    // JOIN OTHER ENTITIES BASED ON THE BUSINESS
                                select new
                                {
                                    Id = c.ID,
                                    Name = c.NAME,
                                }).FirstOrDefaultAsync();
            return new FormatedResponse() { InnerBody = joined };
        }
        public async Task<FormatedResponse> GetProcess()
        {
            var joined = await(from p in _dbContext.SeProcesss.AsNoTracking()
                                   // JOIN OTHER ENTITIES BASED ON THE BUSINESS
                               select new
                               {
                                   Id = p.ID,
                                   Name = p.NAME,
                               }).ToListAsync();
            return new FormatedResponse() { InnerBody = joined };
        }

        public virtual async Task<FormatedResponse> ToggleActiveIds(GenericUnitOfWork _uow, List<long> ids, bool valueToBind, string sid)
        {
            await Task.Run(() => null);
            throw new NotImplementedException();
        }
    }
}

