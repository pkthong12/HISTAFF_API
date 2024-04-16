using CORE.GenericUOW;
using CORE.DTO;
using API.DTO;
using CORE.Enum;
using CORE.StaticConstant;
using API.All.DbContexts;
using CORE.AutoMapper;

namespace API.Controllers.TrProgramResult
{
    public class TrProgramResultRepository : ITrProgramResultRepository
    {
        private readonly GenericUnitOfWork _uow;
        private readonly FullDbContext _dbContext;
        private IGenericRepository<TR_PROGRAM_RESULT, TrProgramResultDTO> _genericRepository;
        private readonly GenericReducer<TR_PROGRAM_RESULT, TrProgramResultDTO> _genericReducer;

        public TrProgramResultRepository(FullDbContext context, GenericUnitOfWork uow)
        {
            _dbContext = context;
            _uow = uow;
            _genericRepository = _uow.GenericRepository<TR_PROGRAM_RESULT, TrProgramResultDTO>();
            _genericReducer = new();
        }

        public async Task<GenericPhaseTwoListResponse<TrProgramResultDTO>> SinglePhaseQueryList(GenericQueryListDTO<TrProgramResultDTO> request)
        {
            var joined = from p in _dbContext.TrProgramResults.AsNoTracking()
                         from e in _dbContext.HuEmployees.AsNoTracking().Where(e=> e.ID == p.EMPLOYEE_ID).DefaultIfEmpty()
                         from cv in _dbContext.HuEmployeeCvs.AsNoTracking().Where(cv=> cv.ID == e.PROFILE_ID).DefaultIfEmpty()
                         from o in _dbContext.HuOrganizations.AsNoTracking().Where(o=> o.ID == e.ORG_ID).DefaultIfEmpty()
                         from po in _dbContext.HuPositions.AsNoTracking().Where(po=> po.ID == e.POSITION_ID).DefaultIfEmpty()
                         from s in _dbContext.SysOtherLists.AsNoTracking().Where(s=> s.ID == p.TR_RANK_ID).DefaultIfEmpty()
                         // JOIN OTHER ENTITIES BASED ON THE BUSINESS
                         select new TrProgramResultDTO
                         {
                             Id = p.ID,
                             TrProgramId = p.TR_PROGRAM_ID,
                             EmployeeName = cv.FULL_NAME,
                             EmployeeCode = e.CODE,
                             EmployeeId = p.ID,
                             OrgName = o.NAME,
                             PositionId = po.ID,
                             PositionName = po.NAME,
                             IsReach = p.IS_REACH,
                             TrRankId = p.TR_RANK_ID,
                             TrRankName = s.NAME,
                             FinalScore = p.FINAL_SCORE,
                             Comment1 = p.COMMENT_1,
                             Comment2 = p.COMMENT_2,
                             Comment3 = p.COMMENT_3,
                             CertDate = p.CERT_DATE,
                             Note = p.NOTE
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
                var list = new List<TR_PROGRAM_RESULT>
                    {
                        (TR_PROGRAM_RESULT)response
                    };
                var joined = (from l in list
                              // JOIN OTHER ENTITIES BASED ON THE BUSINESS
                              select new TrProgramResultDTO
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

        public async Task<FormatedResponse> Create(GenericUnitOfWork _uow, TrProgramResultDTO dto, string sid)
        {
            var response = await _genericRepository.Create(_uow, dto, sid);
            return response;
        }

        public async Task<FormatedResponse> CreateRange(GenericUnitOfWork _uow, List<TrProgramResultDTO> dtos, string sid)
        {
            var add = new List<TrProgramResultDTO>();
            add.AddRange(dtos);
            var response = await _genericRepository.CreateRange(_uow, add, sid);
            return response;
        }

        public async Task<FormatedResponse> Update(GenericUnitOfWork _uow, TrProgramResultDTO dto, string sid, bool patchMode = true)
        {
            var response = await _genericRepository.Update(_uow, dto, sid, patchMode);
            return response;
        }

        public async Task<FormatedResponse> UpdateRange(GenericUnitOfWork _uow, List<TrProgramResultDTO> dtos, string sid, bool patchMode = true)
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

