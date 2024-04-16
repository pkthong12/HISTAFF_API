using CORE.GenericUOW;
using CORE.DTO;
using API.DTO;
using CORE.Enum;
using CORE.StaticConstant;
using API.All.DbContexts;
using CORE.AutoMapper;

namespace API.Controllers.InsMaternityMng
{
    public class InsMaternityMngRepository : IInsMaternityMngRepository
    {
        private readonly GenericUnitOfWork _uow;
        private readonly FullDbContext _dbContext;
        private IGenericRepository<INS_MATERNITY_MNG, InsMaternityMngDTO> _genericRepository;
        private readonly GenericReducer<INS_MATERNITY_MNG, InsMaternityMngDTO> _genericReducer;

        public InsMaternityMngRepository(FullDbContext context, GenericUnitOfWork uow)
        {
            _dbContext = context;
            _uow = uow;
            _genericRepository = _uow.GenericRepository<INS_MATERNITY_MNG, InsMaternityMngDTO>();
            _genericReducer = new();
        }

        public async Task<GenericPhaseTwoListResponse<InsMaternityMngDTO>> SinglePhaseQueryList(GenericQueryListDTO<InsMaternityMngDTO> request)
        {
            var joined = from p in _dbContext.InsMaternityMngs.AsNoTracking()
                         from e in _dbContext.HuEmployees.AsNoTracking().Where(e => e.ID == p.EMPLOYEE_ID).DefaultIfEmpty()
                         from o in _dbContext.HuOrganizations.AsNoTracking().Where(o => o.ID == e.ORG_ID).DefaultIfEmpty()
                         from pos in _dbContext.HuPositions.AsNoTracking().Where(p => p.ID == e.POSITION_ID).DefaultIfEmpty()
                         from j in _dbContext.HuJobs.AsNoTracking().Where(j => j.ID == pos.JOB_ID).DefaultIfEmpty()
                         select new InsMaternityMngDTO
                         {
                             Id = p.ID,
                             EmployeeId = p.EMPLOYEE_ID,
                             EmployeeCode = e.CODE,
                             EmployeeName = e.Profile!.FULL_NAME,
                             OrgId = o.ID,
                             OrgName = o.NAME,
                             PosName = pos.NAME,
                             InsuranceNo = e.Profile!.INSURENCE_NUMBER,
                             NgayDuSinh = p.NGAY_DU_SINH,
                             IsNghiThaiSan = p.IS_NGHI_THAI_SAN,
                             FromDate = p.FROM_DATE,
                             ToDate = p.TO_DATE,
                             NgaySinh = p.NGAY_SINH,
                             SoCon = p.SO_CON,
                             TienTamUng = p.TIEN_TAM_UNG,
                             FromDateEnjoy = p.FROM_DATE_ENJOY,
                             ToDateEnjoy = p.TO_DATE_ENJOY,
                             NgayDiLamSom = p.NGAY_DI_LAM_SOM,
                             Note = p.NOTE,
                             JobOrderNum = (int)(j.ORDERNUM ?? 99)
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
                var joined = await (from p in _dbContext.InsMaternityMngs.AsNoTracking().Where(p => p.ID == id).DefaultIfEmpty()
                                    from e in _dbContext.HuEmployees.AsNoTracking().Where(e => e.ID == p.EMPLOYEE_ID).DefaultIfEmpty()
                                    from o in _dbContext.HuOrganizations.AsNoTracking().Where(o => o.ID == e.ORG_ID).DefaultIfEmpty()
                                    from pos in _dbContext.HuPositions.AsNoTracking().Where(p => p.ID == e.POSITION_ID).DefaultIfEmpty()
                                    select new InsMaternityMngDTO
                                    {
                                        Id = p.ID,
                                        EmployeeId = p.EMPLOYEE_ID,
                                        EmployeeCode = e.CODE,
                                        EmployeeName = e.Profile!.FULL_NAME,
                                        OrgName = o.NAME,
                                        PosName = pos.NAME,
                                        InsuranceNo = e.Profile!.INSURENCE_NUMBER,
                                        NgayDuSinh = p.NGAY_DU_SINH,
                                        IsNghiThaiSan = p.IS_NGHI_THAI_SAN,
                                        FromDate = p.FROM_DATE,
                                        ToDate = p.TO_DATE,
                                        NgaySinh = p.NGAY_SINH,
                                        SoCon = p.SO_CON,
                                        TienTamUng = p.TIEN_TAM_UNG,
                                        FromDateEnjoy = p.FROM_DATE_ENJOY,
                                        ToDateEnjoy = p.TO_DATE_ENJOY,
                                        NgayDiLamSom = p.NGAY_DI_LAM_SOM,
                                        Note = p.NOTE,
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

        public async Task<FormatedResponse> Create(GenericUnitOfWork _uow, InsMaternityMngDTO dto, string sid)
        {
            var response = await _genericRepository.Create(_uow, dto, sid);
            return response;
        }

        public async Task<FormatedResponse> CreateRange(GenericUnitOfWork _uow, List<InsMaternityMngDTO> dtos, string sid)
        {
            var add = new List<InsMaternityMngDTO>();
            add.AddRange(dtos);
            var response = await _genericRepository.CreateRange(_uow, add, sid);
            return response;
        }

        public async Task<FormatedResponse> Update(GenericUnitOfWork _uow, InsMaternityMngDTO dto, string sid, bool patchMode = true)
        {
            var response = await _genericRepository.Update(_uow, dto, sid, patchMode);
            return response;
        }

        public async Task<FormatedResponse> UpdateRange(GenericUnitOfWork _uow, List<InsMaternityMngDTO> dtos, string sid, bool patchMode = true)
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

        public virtual async Task<FormatedResponse> ToggleActiveIds(GenericUnitOfWork _uow, List<long> ids, bool valueToBind, string sid)
        {
            var response = await _genericRepository.ToggleActiveIds(_uow, ids, valueToBind, sid);
            return response;
        }
    }
}

