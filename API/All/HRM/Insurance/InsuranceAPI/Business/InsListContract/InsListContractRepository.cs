using CORE.GenericUOW;
using CORE.DTO;
using API.DTO;
using CORE.Enum;
using CORE.StaticConstant;
using API.All.DbContexts;
using CORE.AutoMapper;
using System.Data.Entity;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.InsListContract
{
    public class InsListContractRepository : IInsListContractRepository
    {
        private readonly GenericUnitOfWork _uow;
        private readonly FullDbContext _dbContext;
        private IGenericRepository<INS_LIST_CONTRACT, InsListContractDTO> _genericRepository;
        private readonly GenericReducer<INS_LIST_CONTRACT, InsListContractDTO> _genericReducer;

        public InsListContractRepository(FullDbContext context, GenericUnitOfWork uow)
        {
            _dbContext = context;
            _uow = uow;
            _genericRepository = _uow.GenericRepository<INS_LIST_CONTRACT, InsListContractDTO>();
            _genericReducer = new();
        }

        public async Task<GenericPhaseTwoListResponse<InsListContractDTO>> SinglePhaseQueryList(GenericQueryListDTO<InsListContractDTO> request)
        {
            var joined = from p in _dbContext.InsListContracts
                         from u in _dbContext.SysOtherLists.Where(x => x.ID == p.ORG_INSURANCE).DefaultIfEmpty()
                             // JOIN OTHER ENTITIES BASED ON THE BUSINESS
                         select new InsListContractDTO
                         {
                             Id = p.ID,
                             ContractInsNo = p.CONTRACT_INS_NO,
                             Year = p.YEAR,
                             OrgInsurance = p.ORG_INSURANCE,
                             OrgInsuranceName = u.NAME,
                             StartDate = p.START_DATE,
                             ExpireDate = p.EXPIRE_DATE,
                             ValCo = p.VAL_CO,
                             BuyDate = p.BUY_DATE,
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
            try
            {
                var query =  (from p in _dbContext.InsListContracts.Where(x => x.ID == id)
                              from u in _dbContext.SysOtherLists.Where(x => x.ID == p.ORG_INSURANCE).DefaultIfEmpty()
                                   select new InsListContractDTO
                                   {
                                       Id = p.ID,
                                       ContractInsNo = p.CONTRACT_INS_NO,
                                       Year = p.YEAR,
                                       OrgInsurance = p.ORG_INSURANCE,
                                       OrgInsuranceName = u.NAME,
                                       StartDate = p.START_DATE,
                                       ExpireDate = p.EXPIRE_DATE,
                                       ValCo = p.VAL_CO,
                                       BuyDate = p.BUY_DATE,
                                       Note = p.NOTE,
                                       SalInsu = p.SAL_INSU,
                                       Program = p.PROGRAM
                                   }).FirstOrDefault();
                return new FormatedResponse() { InnerBody = query };
            }
            catch (Exception ex)
            {
                return new FormatedResponse() { MessageCode = ex.ToString(), ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode500 };
            }
        }

        public async Task<FormatedResponse> GetById(string id)
        {
            await Task.Run(() => null);
            throw new NotImplementedException();
        }

        public async Task<FormatedResponse> Create(GenericUnitOfWork _uow, InsListContractDTO dto, string sid)
        {
            dto.IsActive = true;
            var response = await _genericRepository.Create(_uow, dto, sid);
            return response;
        }

        public async Task<FormatedResponse> CreateRange(GenericUnitOfWork _uow, List<InsListContractDTO> dtos, string sid)
        {
            var add = new List<InsListContractDTO>();
            add.AddRange(dtos);
            var response = await _genericRepository.CreateRange(_uow, add, sid);
            return response;
        }

        public async Task<FormatedResponse> Update(GenericUnitOfWork _uow, InsListContractDTO dto, string sid, bool patchMode = true)
        {
            var response = await _genericRepository.Update(_uow, dto, sid, patchMode);
            return response;
        }

        public async Task<FormatedResponse> UpdateRange(GenericUnitOfWork _uow, List<InsListContractDTO> dtos, string sid, bool patchMode = true)
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

        //public async Task<FormatedResponse> GetListYearPeroid()
        //{
        //    var query = await (from p in _dbContext.AtPeriodStandards.Where(x => x.IS_ACTIVE == true)
        //                       select new
        //                       {
        //                           Id = p.ID,
        //                           Year = p.YEAR
        //                       }).Distinct().ToListAsync();
        //    return new FormatedResponse() { InnerBody = query};
        //}
        public async Task<FormatedResponse> GetListYearPeroid()
        {
            var query = await (from p in _dbContext.AtPeriodStandards.Where(x => x.IS_ACTIVE == true)
                               select new
                               {
                                   Id = p.ID,
                                   Year = p.YEAR
                               }).Distinct().ToListAsync();
            return new FormatedResponse() { InnerBody = query};
        }


        public async Task<FormatedResponse> GetInsListContract(int year)
        {
            var queryable = await (from p in _dbContext.InsListContracts.Where(p => p.IS_ACTIVE!.Value == true && p.YEAR == year)
                                   orderby p.START_DATE ascending
                                   select new
                                   {
                                       Id = p.ID,
                                       ContractInsNo = p.CONTRACT_INS_NO,
                                   }).ToListAsync();
            return new FormatedResponse() { InnerBody = queryable };
        }

    }
}

