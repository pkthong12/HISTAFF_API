using CORE.GenericUOW;
using CORE.DTO;
using API.DTO;
using CORE.Enum;
using CORE.StaticConstant;
using API.All.DbContexts;
using CORE.AutoMapper;
using Common.Interfaces;
using Common.DataAccess;

namespace API.Controllers.PaTaxAnnualImport
{
    public class PaTaxAnnualImportRepository : IPaTaxAnnualImportRepository
    {
        private readonly GenericUnitOfWork _uow;
        private readonly FullDbContext _dbContext;
        private IGenericRepository<PA_TAX_ANNUAL_IMPORT, PaTaxAnnualImportDTO> _genericRepository;
        private readonly GenericReducer<PA_TAX_ANNUAL_IMPORT, PaTaxAnnualImportDTO> _genericReducer;
        protected AbsQueryDataTemplate QueryData;

        public PaTaxAnnualImportRepository(FullDbContext context, GenericUnitOfWork uow)
        {
            _dbContext = context;
            _uow = uow;
            _genericRepository = _uow.GenericRepository<PA_TAX_ANNUAL_IMPORT, PaTaxAnnualImportDTO>();
            _genericReducer = new();
            QueryData = new SqlQueryDataTemplate(_dbContext);
        }

        public async Task<GenericPhaseTwoListResponse<PaTaxAnnualImportDTO>> SinglePhaseQueryList(GenericQueryListDTO<PaTaxAnnualImportDTO> request)
        {
            DateTime startDate = DateTime.Now;
            long? year = -1;
            long? objSalaryId = -1;
            if (request.Filter != null)
            {
                if (request.Filter.Year != null)
                {
                    year = request.Filter.Year;
                    startDate = new DateTime((int)request.Filter.Year, 12, 31);
                    request.Filter.Year = null;
                }

                if (request.Filter.ObjSalaryId != null)
                {
                    objSalaryId = request.Filter.ObjSalaryId;
                    request.Filter.ObjSalaryId = null;
                }

            }
            var reJoined = from e in _dbContext.HuEmployees.Where(x => x.JOIN_DATE <= startDate && ((x.WORK_STATUS_ID ?? 0) != 1028 || ((x.WORK_STATUS_ID ?? 0) == 1028 && (x.TER_EFFECT_DATE ?? DateTime.Now) >= startDate)))
                           from cv in _dbContext.HuEmployeeCvs.Where(x => x.ID == e.PROFILE_ID)
                           from t in _dbContext.HuPositions.Where(x => x.ID == e.POSITION_ID)
                           from j in _dbContext.HuJobs.AsNoTracking().Where(j => j.ID == t.JOB_ID).DefaultIfEmpty()
                           from o in _dbContext.HuOrganizations.Where(x => x.ID == e.ORG_ID)
                           from p in _dbContext.PaTaxAnnualImports.Where(x => x.EMPLOYEE_ID == e.ID && x.YEAR == year && x.OBJ_SALARY_ID == objSalaryId).DefaultIfEmpty()
                           from ob in _dbContext.HuSalaryTypes.Where(x => x.ID == p.OBJ_SALARY_ID).DefaultIfEmpty()
                           select new PaTaxAnnualImportDTO
                           {
                               Id = e.ID,
                               EmployeeCode = e.CODE,
                               EmployeeName = cv.FULL_NAME,
                               EmployeeId = e.ID,
                               Year= p.YEAR,
                               PositionName = t.NAME,
                               OrgName = o.NAME,
                               OrgId = o.ID,
                               ObjSalaryId = p.OBJ_SALARY_ID,
                               ObjSalaryName = ob.NAME,
                               CreatedBy = p.CREATED_BY,
                               CreatedDate = p.CREATED_DATE,
                               UpdatedBy = p.UPDATED_BY,
                               UpdatedDate = p.UPDATED_DATE,
                               Tax18=p.TAX18,
                               Tax26=p.TAX26,
                               Note= p.NOTE,
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
                var list = new List<PA_TAX_ANNUAL_IMPORT>
                    {
                        (PA_TAX_ANNUAL_IMPORT)response
                    };
                var joined = (from l in list
                                  // JOIN OTHER ENTITIES BASED ON THE BUSINESS
                              select new PaTaxAnnualImportDTO
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

        public async Task<FormatedResponse> Create(GenericUnitOfWork _uow, PaTaxAnnualImportDTO dto, string sid)
        {
            var response = await _genericRepository.Create(_uow, dto, sid);
            return response;
        }

        public async Task<FormatedResponse> CreateRange(GenericUnitOfWork _uow, List<PaTaxAnnualImportDTO> dtos, string sid)
        {
            var add = new List<PaTaxAnnualImportDTO>();
            add.AddRange(dtos);
            var response = await _genericRepository.CreateRange(_uow, add, sid);
            return response;
        }

        public async Task<FormatedResponse> Update(GenericUnitOfWork _uow, PaTaxAnnualImportDTO dto, string sid, bool patchMode = true)
        {
            var response = await _genericRepository.Update(_uow, dto, sid, patchMode);
            return response;
        }

        public async Task<FormatedResponse> UpdateRange(GenericUnitOfWork _uow, List<PaTaxAnnualImportDTO> dtos, string sid, bool patchMode = true)
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
            var response = await (from p in _dbContext.PaListsalariess.AsNoTracking().Where(p => p.OBJ_SAL_ID == id && p.IS_VISIBLE == true)
                                  from c in _dbContext.PaListSals.AsNoTracking().Where(c => c.ID == p.CODE_SAL).DefaultIfEmpty()
                                  select new
                                  {
                                      Id = p.ID,
                                      Name = c.CODE_LISTSAL + " : " + p.NAME,
                                      Code = c.CODE_LISTSAL,
                                  }).ToListAsync();

            return new FormatedResponse() { InnerBody = response, StatusCode = EnumStatusCode.StatusCode200 };
        }

        public async Task<FormatedResponse> GetObjSalTax()
        {//lay doi tuong luong
            var query = await (from o in _dbContext.HuSalaryTypes.AsNoTracking().Where(o => o.IS_ACTIVE == true && (o.CODE == "ĐTL006" || o.CODE=="DTL006")).OrderBy(o => o.NAME)
                               select new
                               {
                                   Id = o.ID,
                                   Code = o.CODE,
                                   Name = o.NAME,
                               }).ToListAsync();
            return new FormatedResponse() { InnerBody = query };
        }

        public virtual async Task<FormatedResponse> ToggleActiveIds(GenericUnitOfWork _uow, List<long> ids, bool valueToBind, string sid)
        {
            await Task.Run(() => null);
            throw new NotImplementedException();
        }
    }
}

