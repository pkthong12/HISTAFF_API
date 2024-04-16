using CORE.GenericUOW;
using CORE.DTO;
using API.DTO;
using CORE.Enum;
using CORE.StaticConstant;
using API.All.DbContexts;
using CORE.AutoMapper;
using OfficeOpenXml.FormulaParsing.Utilities;
using Common.Interfaces;
using Common.DataAccess;

namespace API.Controllers.PaSalImport
{
    public class PaSalImportRepository : IPaSalImportRepository
    {
        private readonly GenericUnitOfWork _uow;
        private readonly FullDbContext _dbContext;
        private IGenericRepository<PA_SAL_IMPORT, PaSalImportDTO> _genericRepository;
        private readonly GenericReducer<PA_SAL_IMPORT, PaSalImportDTO> _genericReducer;
        protected AbsQueryDataTemplate QueryData;
        public PaSalImportRepository(FullDbContext context, GenericUnitOfWork uow)
        {
            _dbContext = context;
            _uow = uow;
            _genericRepository = _uow.GenericRepository<PA_SAL_IMPORT, PaSalImportDTO>();
            _genericReducer = new();
            QueryData = new SqlQueryDataTemplate(_dbContext);
        }

        public async Task<GenericPhaseTwoListResponse<PaSalImportDTO>> SinglePhaseQueryList(GenericQueryListDTO<PaSalImportDTO> request)
        {

            DateTime endDate = DateTime.Now;
            DateTime startDate = DateTime.Now;
            long? periodId = -1;
            long? objSalId = -1;
            if (request.Filter != null)
            {
                if (request.Filter.PeriodId != null)
                {
                    var objPerriod = _dbContext.AtSalaryPeriods.Where(x => x.ID == request.Filter.PeriodId).FirstOrDefault();
                    endDate = objPerriod.END_DATE;
                    startDate = objPerriod.START_DATE;
                    periodId = request.Filter.PeriodId;
                    request.Filter.PeriodId = null;
                }
                if (request.Filter.ObjSalaryId != null)
                {
                    objSalId = request.Filter.ObjSalaryId;
                }
            }



            var groupedData = _dbContext.HuWorkings
                .Where(e => e.EFFECT_DATE < endDate && e.STATUS_ID == 994 && (e.IS_WAGE ?? 0) == -1)
                .GroupBy(e => e.EMPLOYEE_ID)
                .ToList();

            var result = groupedData
                .SelectMany(g => g.OrderByDescending(e => e.EFFECT_DATE).ThenByDescending(e => e.ID).Take(1))
                .Select(ab => ab.ID)
                .ToList();

            var reJoined = from e in _dbContext.HuEmployees.Where(x => x.JOIN_DATE <= endDate && ((x.WORK_STATUS_ID ?? 0) != 1028 || ((x.WORK_STATUS_ID ?? 0) == 1028 && (x.TER_EFFECT_DATE ?? DateTime.Now) >= startDate)))
                           from cv in _dbContext.HuEmployeeCvs.Where(x => x.ID == e.PROFILE_ID)
                           from t in _dbContext.HuPositions.Where(x => x.ID == e.POSITION_ID)
                           from j in _dbContext.HuJobs.AsNoTracking().Where(j => j.ID == t.JOB_ID).DefaultIfEmpty()
                           from o in _dbContext.HuOrganizations.Where(x => x.ID == e.ORG_ID)
                           from wo in _dbContext.HuWorkings.Where(x => x.EMPLOYEE_ID == e.ID && result.Contains(x.ID))
                           from ob in _dbContext.HuSalaryTypes.Where(x => x.ID == wo.SALARY_TYPE_ID).DefaultIfEmpty()
                           from p in _dbContext.PaSalImports.Where(x => x.EMPLOYEE_ID == e.ID && x.PERIOD_ID == periodId && x.OBJ_SALARY_ID == objSalId).DefaultIfEmpty()
                           orderby j.ORDERNUM
                           select new PaSalImportDTO
                           {
                               Id = e.ID,
                               EmployeeCode = e.CODE,
                               EmployeeName = cv.FULL_NAME,
                               EmployeeId = e.ID,
                               PositionName = t.NAME,
                               OrgName = o.NAME,
                               OrgId = o.ID,
                               ObjSalaryId = wo.SALARY_TYPE_ID,
                               ObjSalaryName = ob.NAME,
                               CreatedBy = p.CREATED_BY,
                               CreatedDate = p.CREATED_DATE,
                               UpdatedBy = p.UPDATED_BY,
                               UpdatedDate = p.UPDATED_DATE,
                               Cl11 = p.CL11,
                               Cl29 = p.CL29,
                               Cl31 = p.CL31,
                               Cl33 = p.CL33,
                               Clchinh8 = p.CLCHINH8,
                               Deduct3 = p.DEDUCT3,
                               Tax54 = p.TAX54,
                               Csum1 = p.CSUM1,
                               Csum2 = p.CSUM2,
                               PeriodId = p.PERIOD_ID,
                               Note = p.NOTE,
                               JobOrderNum = (int)(j.ORDERNUM ?? 999),
                           };



            var singlePhaseResult = await _genericReducer.SinglePhaseReduce(reJoined, request);
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
                var list = new List<PA_SAL_IMPORT>
                    {
                        (PA_SAL_IMPORT)response
                    };
                var joined = (from l in list
                                  // JOIN OTHER ENTITIES BASED ON THE BUSINESS
                              select new PaSalImportDTO
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

        public async Task<FormatedResponse> Create(GenericUnitOfWork _uow, PaSalImportDTO dto, string sid)
        {
            var response = await _genericRepository.Create(_uow, dto, sid);
            return response;
        }

        public async Task<FormatedResponse> CreateRange(GenericUnitOfWork _uow, List<PaSalImportDTO> dtos, string sid)
        {
            var add = new List<PaSalImportDTO>();
            add.AddRange(dtos);
            var response = await _genericRepository.CreateRange(_uow, add, sid);
            return response;
        }

        public async Task<FormatedResponse> Update(GenericUnitOfWork _uow, PaSalImportDTO dto, string sid, bool patchMode = true)
        {
            var response = await _genericRepository.Update(_uow, dto, sid, patchMode);
            return response;
        }

        public async Task<FormatedResponse> UpdateRange(GenericUnitOfWork _uow, List<PaSalImportDTO> dtos, string sid, bool patchMode = true)
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
        public async Task<FormatedResponse> GetListSalaries(long id)
        {
            var response = await (from p in _dbContext.PaListsalariess.AsNoTracking().Where(p => p.OBJ_SAL_ID == id && p.IS_IMPORT == true)
                                  from c in _dbContext.PaListSals.AsNoTracking().Where(c => c.ID == p.CODE_SAL).DefaultIfEmpty()
                                  select new
                                  {
                                      Id = p.ID,
                                      Name = c.CODE_LISTSAL + ": " + p.NAME,
                                      Code = c.CODE_LISTSAL,
                                  }).ToListAsync();

            return new FormatedResponse() { InnerBody = response, StatusCode = EnumStatusCode.StatusCode200 };
        }

        public virtual async Task<FormatedResponse> ToggleActiveIds(GenericUnitOfWork _uow, List<long> ids, bool valueToBind, string sid)
        {
            await Task.Run(() => null);
            throw new NotImplementedException();
        }
    }
}

