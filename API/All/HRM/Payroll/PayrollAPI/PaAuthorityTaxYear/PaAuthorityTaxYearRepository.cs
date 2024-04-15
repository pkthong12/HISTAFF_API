using API.All.DbContexts;
using API.DTO;
using Common.Extensions;
using CORE.DTO;
using CORE.Enum;
using CORE.GenericUOW;
using CORE.StaticConstant;
using Microsoft.AspNetCore.SignalR;

namespace API.Controllers.PaAuthorityTaxYear
{
    public class PaAuthorityTaxYearRepository : IPaAuthorityTaxYearRepository
    {
        private readonly GenericUnitOfWork _uow;
        private readonly FullDbContext _dbContext;
        private IGenericRepository<PA_AUTHORITY_TAX_YEAR, PaAuthorityTaxYearDTO> _genericRepository;
        private readonly GenericReducer<PA_AUTHORITY_TAX_YEAR, PaAuthorityTaxYearDTO> _genericReducer;

        public PaAuthorityTaxYearRepository(FullDbContext context, GenericUnitOfWork uow)
        {
            _dbContext = context;
            _uow = uow;
            _genericRepository = _uow.GenericRepository<PA_AUTHORITY_TAX_YEAR, PaAuthorityTaxYearDTO>();
            _genericReducer = new();
        }

        public async Task<GenericPhaseTwoListResponse<PaAuthorityTaxYearDTO>> SinglePhaseQueryList(GenericQueryListDTO<PaAuthorityTaxYearDTO> request)
        {
            var joined = from p in _dbContext.PaAuthorityTaxYears.AsNoTracking()
                         from e in _dbContext.HuEmployees.AsNoTracking().Where(e => e.ID == p.EMPLOYEE_ID).DefaultIfEmpty()
                         from pos in _dbContext.HuPositions.AsNoTracking().Where(pos => pos.ID == e.POSITION_ID).DefaultIfEmpty()
                         from j in _dbContext.HuJobs.AsNoTracking().Where(j => j.ID == pos.JOB_ID).DefaultIfEmpty()
                         from o in _dbContext.HuOrganizations.AsNoTracking().Where(o => o.ID == e.ORG_ID).DefaultIfEmpty()
                         select new PaAuthorityTaxYearDTO
                         {
                             Id = p.ID,
                             EmployeeId = p.EMPLOYEE_ID,
                             EmployeeCode = e.CODE,
                             EmployeeName = e.Profile!.FULL_NAME,
                             PositionName = pos.NAME,
                             OrgId = o.ID,
                             DepartmentName = o.NAME,
                             Year = p.YEAR,
                             IsEmpRegister = p.IS_EMP_REGISTER,
                             IsComApprove = p.IS_COM_APPROVE,
                             ReasonReject = p.REASON_REJECT,
                             Note = p.NOTE,
                             JobOrderNum = (int)(j.ORDERNUM ?? 999),
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

            var joined = await (from p in _dbContext.PaAuthorityTaxYears.AsNoTracking().Where(p => p.ID == id)
                            from e in _dbContext.HuEmployees.AsNoTracking().Where(e => e.ID == p.EMPLOYEE_ID).DefaultIfEmpty()
                            from pos in _dbContext.HuPositions.AsNoTracking().Where(pos => pos.ID == e.POSITION_ID).DefaultIfEmpty()
                            from o in _dbContext.HuOrganizations.AsNoTracking().Where(o => o.ID == pos.ORG_ID).DefaultIfEmpty()
                            select new PaAuthorityTaxYearDTO
                            {
                                Id = p.ID,
                                EmployeeId = p.EMPLOYEE_ID,
                                EmployeeCode = e.CODE,
                                EmployeeName = e.Profile!.FULL_NAME,
                                PositionName = pos.NAME,
                                OrgId = o.ID,
                                DepartmentName = o.NAME,
                                Year = p.YEAR,
                                IsEmpRegister = p.IS_EMP_REGISTER,
                                IsComApprove = p.IS_COM_APPROVE,
                                ReasonReject = p.REASON_REJECT,
                                Note = p.NOTE
                            }).FirstOrDefaultAsync();
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

        public async Task<FormatedResponse> Create(GenericUnitOfWork _uow, PaAuthorityTaxYearDTO dto, string sid)
        {
            try
            {
                var response = new FormatedResponse();
                var listData = new List<object>();

                if (dto.EmployeeIds != null)
                {
                    _uow.CreateTransaction();
                    foreach (long i in dto.EmployeeIds)
                    {
                        dto.EmployeeId = i;
                        var check = _dbContext.PaAuthorityTaxYears.Where(p => p.EMPLOYEE_ID == dto.EmployeeId && p.YEAR == dto.Year).Count();
                        if(check != 0)
                        {
                            return new() { StatusCode = EnumStatusCode.StatusCode400, ErrorType = EnumErrorType.CATCHABLE, MessageCode = CommonMessageCode.EMPLOYEE_ONLY_HAVE_ONE_RECORD_EACH_YEAR };
                        }
                        response = await _genericRepository.Create(_uow, dto, sid);
                        if (response.MessageCode != CommonMessageCode.CREATE_SUCCESS)
                        {
                            return response;
                        }
                        listData.Add(response.InnerBody!);
                    }
                    _uow.Commit();
                    return new() { InnerBody = listData[0], MessageCode = CommonMessageCode.CREATE_SUCCESS };
                }
                else
                {
                    return new() { StatusCode = EnumStatusCode.StatusCode400, ErrorType = EnumErrorType.CATCHABLE, MessageCode = CommonMessageCode.EMPLOYEE_MUST_SELECTED };
                }
            }
            catch (Exception ex)
            {
                _uow.Rollback();
                return new() { ErrorType = EnumErrorType.UNCATCHABLE, StatusCode = EnumStatusCode.StatusCode500, MessageCode = ex.Message };
            }
        }

        public async Task<FormatedResponse> CreateRange(GenericUnitOfWork _uow, List<PaAuthorityTaxYearDTO> dtos, string sid)
        {
            var add = new List<PaAuthorityTaxYearDTO>();
            add.AddRange(dtos);
            var response = await _genericRepository.CreateRange(_uow, add, sid);
            return response;
        }

        public async Task<FormatedResponse> Update(GenericUnitOfWork _uow, PaAuthorityTaxYearDTO dto, string sid, bool patchMode = true)
        {
            var check = _dbContext.PaAuthorityTaxYears.Where(p => p.EMPLOYEE_ID == dto.EmployeeId && p.YEAR == dto.Year).Count();
            if (check != 0)
            {
                return new() { StatusCode = EnumStatusCode.StatusCode400, ErrorType = EnumErrorType.CATCHABLE, MessageCode = CommonMessageCode.EMPLOYEE_ONLY_HAVE_ONE_RECORD_EACH_YEAR };
            }
            var response = await _genericRepository.Update(_uow, dto, sid, patchMode);
            return response;
        }

        public async Task<FormatedResponse> UpdateRange(GenericUnitOfWork _uow, List<PaAuthorityTaxYearDTO> dtos, string sid, bool patchMode = true)
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
