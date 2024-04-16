using CORE.GenericUOW;
using CORE.DTO;
using API.DTO;
using CORE.Enum;
using CORE.StaticConstant;
using API.All.DbContexts;
using CORE.AutoMapper;
using Hangfire.Common;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;
using Common.Repositories;
using System;
using ProfileDAL.ViewModels;
using Common.Extensions;

namespace API.Controllers.InsHealthInsurance
{
    public class InsHealthInsuranceRepository : RepositoryBase<INS_HEALTH_INSURANCE>, IInsHealthInsuranceRepository
    {
        private readonly GenericUnitOfWork _uow;
        private readonly FullDbContext _dbContext;
        private IGenericRepository<INS_HEALTH_INSURANCE, InsHealthInsuranceDTO> _genericRepository;
        private readonly GenericReducer<INS_HEALTH_INSURANCE, InsHealthInsuranceDTO> _genericReducer;

        public InsHealthInsuranceRepository(FullDbContext context, GenericUnitOfWork uow) : base(context)
        {
            _dbContext = context;
            _uow = uow;
            _genericRepository = _uow.GenericRepository<INS_HEALTH_INSURANCE, InsHealthInsuranceDTO>();
            _genericReducer = new();
        }

        public async Task<GenericPhaseTwoListResponse<InsHealthInsuranceDTO>> SinglePhaseQueryList(GenericQueryListDTO<InsHealthInsuranceDTO> request)
        {
            var joined = from p in _dbContext.InsHealthInsurances.AsNoTracking()
                         from e in _dbContext.HuEmployees.AsNoTracking().Where(e => e.ID == p.EMPLOYEE_ID).DefaultIfEmpty()
                         from cv in _dbContext.HuEmployeeCvs.AsNoTracking().Where(cv => cv.ID == e.PROFILE_ID).DefaultIfEmpty()
                         from pos in _dbContext.HuPositions.AsNoTracking().Where(pos => pos.ID == e.POSITION_ID).DefaultIfEmpty()
                         from j in _dbContext.HuJobs.AsNoTracking().Where(x => x.ID == pos.JOB_ID).DefaultIfEmpty()
                         from o in _dbContext.HuOrganizations.AsNoTracking().Where(o => o.ID == e.ORG_ID).DefaultIfEmpty()
                         from lc in _dbContext.InsListContracts.AsNoTracking().Where(lc => lc.ID == p.INS_CONTRACT_ID).DefaultIfEmpty()
                         from f in _dbContext.HuFamilys.AsNoTracking().Where(f => f.ID == p.FAMILY_ID).DefaultIfEmpty()
                         from r in _dbContext.SysOtherLists.Where(x => x.ID == f.RELATIONSHIP_ID).DefaultIfEmpty()
                         from payee in _dbContext.SysOtherLists.Where(x => x.ID == p.DT_CHITRA).DefaultIfEmpty()
                         select new InsHealthInsuranceDTO
                         {
                             Id = p.ID,
                             EmployeeCode = e.CODE,
                             EmployeeName = e.Profile!.FULL_NAME,
                             BirthDate = cv.BIRTH_DATE,
                             IdNo = cv.ID_NO,
                             OrgId = e.ORG_ID,
                             OrgName = o.NAME,
                             PosName = pos.NAME,
                             Year = p.YEAR,
                             InsContractNo = lc.CONTRACT_INS_NO,
                             OrgInsurance = lc.ORG_INSURANCE,
                             StartDate = lc.START_DATE,
                             ExpireDate = lc.EXPIRE_DATE,
                             ValCo = lc.VAL_CO,
                             CheckBhnt = p.CHECK_BHNT,
                             FamilyId = p.FAMILY_ID,
                             FamilyMemberName = f.FULLNAME,
                             RelationshipName = r.NAME,
                             FamilyMemberBirthDate = f.BIRTH_DATE,
                             FamilyMemberIdNo = f.ID_NO,
                             // DTCHITRA
                             DtChitra = p.DT_CHITRA,
                             DtChitraName = payee.NAME,
                             JoinDate = p.JOIN_DATE,
                             EffectDate = p.EFFECT_DATE,
                             MoneyIns = p.MONEY_INS,
                             ReduceDate = p.REDUCE_DATE,
                             Refund = p.REFUND,
                             DateReceiveMoney = p.DATE_RECEIVE_MONEY,
                             EmpReceiveMoney = p.EMP_RECEIVE_MONEY,
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
            var joined = await (from p in _dbContext.InsHealthInsurances.AsNoTracking().Where(i => i.ID == id).DefaultIfEmpty()
                                from e in _dbContext.HuEmployees.AsNoTracking().Where(e => e.ID == p.EMPLOYEE_ID).DefaultIfEmpty()
                                from cv in _dbContext.HuEmployeeCvs.AsNoTracking().Where(cv => cv.ID == e.PROFILE_ID).DefaultIfEmpty()
                                from pos in _dbContext.HuPositions.AsNoTracking().Where(pos => pos.ID == e.POSITION_ID).DefaultIfEmpty()
                                from j in _dbContext.HuJobs.AsNoTracking().Where(x => x.ID == pos.JOB_ID).DefaultIfEmpty()
                                from o in _dbContext.HuOrganizations.AsNoTracking().Where(o => o.ID == e.ORG_ID).DefaultIfEmpty()
                                from lc in _dbContext.InsListContracts.AsNoTracking().Where(lc => lc.ID == p.INS_CONTRACT_ID).DefaultIfEmpty()
                                from f in _dbContext.HuFamilys.AsNoTracking().Where(f => f.ID == p.FAMILY_ID).DefaultIfEmpty()
                                from r in _dbContext.SysOtherLists.Where(x => x.ID == f.RELATIONSHIP_ID).DefaultIfEmpty()
                                from payee in _dbContext.SysOtherLists.Where(x => x.ID == p.DT_CHITRA).DefaultIfEmpty()

                                select new InsHealthInsuranceDTO
                                {
                                    Id = p.ID,
                                    EmployeeId = p.EMPLOYEE_ID,
                                    EmployeeCode = e.CODE,
                                    EmployeeName = e.Profile!.FULL_NAME,
                                    BirthDate = cv.BIRTH_DATE,
                                    IdNo = cv.ID_NO,
                                    OrgId = e.ORG_ID,
                                    OrgName = o.NAME,
                                    PosName = pos.NAME,
                                    Year = p.YEAR,
                                    InsContractId = p.INS_CONTRACT_ID,
                                    InsContractNo = lc.CONTRACT_INS_NO,
                                    OrgInsurance = lc.ORG_INSURANCE,
                                    StartDate = lc.START_DATE,
                                    ExpireDate = lc.EXPIRE_DATE,
                                    ValCo = lc.VAL_CO,
                                    CheckBhnt = p.CHECK_BHNT,
                                    FamilyId = p.FAMILY_ID,
                                    FamilyMemberName = f.FULLNAME,
                                    RelationshipName = r.NAME,
                                    FamilyMemberBirthDate = f.BIRTH_DATE,
                                    FamilyMemberIdNo = f.ID_NO,
                                    DtChitra = p.DT_CHITRA,
                                    DtChitraName = payee.NAME,
                                    JoinDate = p.JOIN_DATE,
                                    EffectDate = p.EFFECT_DATE,
                                    MoneyIns = p.MONEY_INS,
                                    ReduceDate = p.REDUCE_DATE,
                                    Refund = p.REFUND,
                                    DateReceiveMoney = p.DATE_RECEIVE_MONEY,
                                    EmpReceiveMoney = p.EMP_RECEIVE_MONEY,
                                    Note = p.NOTE,
                                    JobOrderNum = (int)(j.ORDERNUM ?? 99),
                                    InsClaimInsurances = (from p in _dbContext.InsClaimInsurances
                                                        from n in _dbContext.InsHealthInsurances.Where(x => x.ID == p.INS_HEALTH_ID).DefaultIfEmpty()
                                                        where n.ID == id
                                                        select new InsClaimInsuranceDTO
                                                        {
                                                            Id = p.ID,
                                                            ExamineDate = p.EXAMINE_DATE,
                                                            DiseaseName = p.DISEASE_NAME,
                                                            CompensationDate = p.COMPENSATION_DATE,
                                                            AmountOfClaims = p.AMOUNT_OF_CLAIMS,
                                                            AmountOfCompensation = p.AMOUNT_OF_COMPENSATION,
                                                            Note = p.NOTE
                                                        }).ToList(),
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

        public async Task<FormatedResponse> Create(GenericUnitOfWork _uow, InsHealthInsuranceDTO dto, string sid)
        {
            try
            {
                await _dbContext.Database.BeginTransactionAsync();

                var healthInsurance = Map(dto, new INS_HEALTH_INSURANCE());
                healthInsurance.CREATED_BY = sid;
                healthInsurance.CREATED_DATE = DateTime.UtcNow;
                await _dbContext.InsHealthInsurances.AddAsync(healthInsurance);
                await _dbContext.SaveChangesAsync();

                if (dto.InsClaimInsurances != null && dto.InsClaimInsurances.Count > 0)
                {
                    var d = _dbContext.InsClaimInsurances.Where(x => x.INS_HEALTH_ID == dto.Id).ToList();
                    if (d != null)
                    {
                        _dbContext.InsClaimInsurances.RemoveRange(d);
                    }
                    foreach (var item in dto.InsClaimInsurances)
                    {
                        item.Id = null;
                        var claimInsurance = Map(item, new INS_CLAIM_INSURANCE());
                        claimInsurance.INS_HEALTH_ID = healthInsurance.ID;


                        claimInsurance.CREATED_BY = sid;
                        claimInsurance.CREATED_DATE = DateTime.UtcNow;
                        await _dbContext.InsClaimInsurances.AddAsync(claimInsurance);
                        await _dbContext.SaveChangesAsync();
                    }
                }

                _dbContext.Database.CommitTransaction();
                return new FormatedResponse() { InnerBody = healthInsurance };
            }
            catch(Exception ex)
            {
                _dbContext.Database.RollbackTransaction();
                return new() { ErrorType = EnumErrorType.UNCATCHABLE, StatusCode = EnumStatusCode.StatusCode500, MessageCode = ex.Message };
            }
        }

        public async Task<FormatedResponse> CreateRange(GenericUnitOfWork _uow, List<InsHealthInsuranceDTO> dtos, string sid)
        {
            var add = new List<InsHealthInsuranceDTO>();
            add.AddRange(dtos);
            var response = await _genericRepository.CreateRange(_uow, add, sid);
            return response;
        }

        public async Task<FormatedResponse> Update(GenericUnitOfWork _uow, InsHealthInsuranceDTO dto, string sid, bool patchMode = true)
        {
            try
            {
                await _dbContext.Database.BeginTransactionAsync();

                var healthInsurance = Map(dto, new INS_HEALTH_INSURANCE());
                healthInsurance.UPDATED_BY = sid;
                healthInsurance.UPDATED_DATE = DateTime.UtcNow;
                _dbContext.InsHealthInsurances.Update(healthInsurance);
                await _dbContext.SaveChangesAsync();

                if (dto.InsClaimInsurances != null && dto.InsClaimInsurances.Count > 0)
                {
                    var d = _dbContext.InsClaimInsurances.Where(x => x.INS_HEALTH_ID == dto.Id).ToList();
                    if (d != null)
                    {
                        _dbContext.InsClaimInsurances.RemoveRange(d);
                    }
                    foreach (var item in dto.InsClaimInsurances)
                    {
                        item.Id = null;
                        var claimInsurance = Map(item, new INS_CLAIM_INSURANCE());
                        claimInsurance.INS_HEALTH_ID = healthInsurance.ID;
                        claimInsurance.CREATED_BY = sid;
                        claimInsurance.CREATED_DATE = DateTime.UtcNow;
                        _dbContext.InsClaimInsurances.Update(claimInsurance);
                        await _dbContext.SaveChangesAsync();
                    }
                }

                _dbContext.Database.CommitTransaction();
                return new FormatedResponse() { InnerBody = healthInsurance };
            }
            catch (Exception ex)
            {
                _dbContext.Database.RollbackTransaction();
                return new() { ErrorType = EnumErrorType.UNCATCHABLE, StatusCode = EnumStatusCode.StatusCode500, MessageCode = ex.Message };
            }
        }

        public async Task<FormatedResponse> UpdateRange(GenericUnitOfWork _uow, List<InsHealthInsuranceDTO> dtos, string sid, bool patchMode = true)
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
            try
            {
                await _dbContext.Database.BeginTransactionAsync();

                foreach (var id in ids)
                {
                    var claimInsurances = await _dbContext.InsClaimInsurances.AsNoTracking().Where(p => p.INS_HEALTH_ID == id).ToListAsync();
                    if (claimInsurances != null)
                    {
                        _dbContext.InsClaimInsurances.RemoveRange(claimInsurances);
                    }
                    await _dbContext.SaveChangesAsync();
                }
                _dbContext.Database.CommitTransaction();
                var response = await _genericRepository.DeleteIds(_uow, ids);
                return response;
            }
            catch (Exception ex)
            {
                _dbContext.Database.RollbackTransaction();
                return new() { ErrorType = EnumErrorType.UNCATCHABLE, StatusCode = EnumStatusCode.StatusCode500, MessageCode = ex.Message };
            }
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

