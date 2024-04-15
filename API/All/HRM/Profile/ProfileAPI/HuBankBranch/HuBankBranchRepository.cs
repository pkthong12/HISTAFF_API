using CORE.GenericUOW;
using CORE.DTO;
using API.DTO;
using CORE.Enum;
using CORE.StaticConstant;
using API.All.DbContexts;
using CORE.AutoMapper;
using Common.Extensions;
using System;
using Azure;

namespace API.Controllers.HuBankBranch
{
    public class HuBankBranchRepository : IHuBankBranchRepository
    {
        private readonly GenericUnitOfWork _uow;
        private readonly FullDbContext _dbContext;
        private IGenericRepository<HU_BANK_BRANCH, HuBankBranchDTO> _genericRepository;
        private readonly GenericReducer<HU_BANK_BRANCH, HuBankBranchDTO> _genericReducer;

        public HuBankBranchRepository(FullDbContext context, GenericUnitOfWork uow)
        {
            _dbContext = context;
            _uow = uow;
            _genericRepository = _uow.GenericRepository<HU_BANK_BRANCH, HuBankBranchDTO>();
            _genericReducer = new();
        }

        public async Task<GenericPhaseTwoListResponse<HuBankBranchDTO>> SinglePhaseQueryList(GenericQueryListDTO<HuBankBranchDTO> request)
        {
            var joined = from p in _dbContext.HuBankBranchs.AsNoTracking()
                         from b in _dbContext.HuBanks.AsNoTracking().Where(x => x.ID == p.BANK_ID).DefaultIfEmpty()
                         select new HuBankBranchDTO
                         {
                             Id = p.ID,
                             Name = p.NAME,
                             Code = p.CODE,
                             IsActive = p.IS_ACTIVE,
                             Status = p.IS_ACTIVE == true ? "Áp dụng" : "Ngừng áp dụng",
                             Note = p.NOTE,
                             CreatedBy = p.CREATED_BY,
                             CreatedDate = p.CREATED_DATE,
                             UpdatedBy = p.UPDATED_BY,
                             UpdatedDate = p.UPDATED_DATE,
                             BankId = p.BANK_ID,
                             BankName = b.NAME
                         };

            var singlePhaseResult = await _genericReducer.SinglePhaseReduce(joined, request);
            return singlePhaseResult;
        }

        public async Task<FormatedResponse> ReadAll()
        {
            var resposne = await _genericRepository.ReadAll();
            return resposne;
        }

        public async Task<FormatedResponse> GetBrankByBankId(long? id)
        {
            var resposne = await (from p in _dbContext.HuBankBranchs
                                 where p.BANK_ID == id && p.IS_ACTIVE == true
                                 select new HuBankBranchDTO
                                 {
                                     Id = p.ID,
                                     Name = p.NAME,
                                 }).ToListAsync();

            return new() { InnerBody = resposne };
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
                var list = new List<HU_BANK_BRANCH>
                    {
                        (HU_BANK_BRANCH)response
                    };
                var joined = (from l in list
                              select new HuBankBranchDTO
                              {
                                  Id = l.ID,
                                  Name = l.NAME,
                                  Code = l.CODE,
                                  Note = l.NOTE,
                                  BankId = l.BANK_ID
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

        public async Task<FormatedResponse> Create(GenericUnitOfWork _uow, HuBankBranchDTO dto, string sid)
        {
            int existName = _dbContext.HuBankBranchs.Where(b => b.NAME == dto.Name &&  b.BANK_ID == dto.BankId).Count();
            if (existName > 0)
            {
                return new() { StatusCode = EnumStatusCode.StatusCode400, ErrorType = EnumErrorType.CATCHABLE, MessageCode = CommonMessageCode.DUBLICATE_VALUE + ": NAME" };
            }

            if (string.IsNullOrWhiteSpace(dto.Name))
            {
                return new() { StatusCode = EnumStatusCode.StatusCode400, ErrorType = EnumErrorType.CATCHABLE, MessageCode = CommonMessageCode.SHORT_NAME_NOT_BLANK };
            }

            var response = await _genericRepository.Create(_uow, dto, sid);
            return response; 
        }

        public async Task<FormatedResponse> CreateRange(GenericUnitOfWork _uow, List<HuBankBranchDTO> dtos, string sid)
        {
            var add = new List<HuBankBranchDTO>();
            add.AddRange(dtos);
            var response = await _genericRepository.CreateRange(_uow, add, sid);
            return response;
        }

        public async Task<FormatedResponse> Update(GenericUnitOfWork _uow, HuBankBranchDTO dto, string sid, bool patchMode = true)
        {
            int existName = _dbContext.HuBankBranchs.Where(b => b.NAME == dto.Name && b.ID != dto.Id).Count();
            if (existName > 0)
            {
                return new() { StatusCode = EnumStatusCode.StatusCode400, ErrorType = EnumErrorType.CATCHABLE, MessageCode = CommonMessageCode.DUBLICATE_VALUE + ": NAME" };
            }

            if (string.IsNullOrWhiteSpace(dto.Name))
            {
                return new() { StatusCode = EnumStatusCode.StatusCode400, ErrorType = EnumErrorType.CATCHABLE, MessageCode = CommonMessageCode.NAME_NOT_BLANK };
            }
            var response = await _genericRepository.Update(_uow, dto, sid, patchMode);
            return response;
        }

        public async Task<FormatedResponse> UpdateRange(GenericUnitOfWork _uow, List<HuBankBranchDTO> dtos, string sid, bool patchMode = true)
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
                var item = await _dbContext.HuBankBranchs.Where(x => x.ID == id).AsNoTracking().SingleOrDefaultAsync();
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

        /// <summary>
        /// Get List Bank Branch is Activce
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<ResultWithError> GetList(int BankId)
        {
            try
            {
                var data = await (from p in _dbContext.HuBankBranchs
                                  where p.IS_ACTIVE == true && p.BANK_ID == BankId
                                  orderby p.NAME
                                  select new
                                  {
                                      Id = p.ID,
                                      Name = p.NAME
                                  }).ToListAsync();
                return new ResultWithError(data);
            }
            catch (Exception ex)
            {
                return new ResultWithError(ex.Message);
            }
        }

        public async Task<FormatedResponse> CreateNewCode()
        {
            string newCode = "";
            if (await _dbContext.HuBankBranchs.CountAsync() == 0)
            {
                newCode = "CNNH001";
            }
            else
            {
                string lastestData = _dbContext.HuBankBranchs.OrderByDescending(t => t.CODE).First().CODE!.ToString();

                string newNumber = (Int32.Parse(lastestData.Substring(4)) + 1).ToString();
                while (newNumber.Length < 3)
                {
                    newNumber = "0" + newNumber;
                }
                newCode = lastestData.Substring(0, 4) + newNumber;
            }
            return new FormatedResponse() { InnerBody = new { Code = newCode } };
        }
        public virtual async Task<FormatedResponse> ToggleActiveIds(GenericUnitOfWork _uow, List<long> ids, bool valueToBind, string sid)
        {
            var response = await _genericRepository.ToggleActiveIds(_uow, ids, valueToBind, sid);
            return response;
        }
    }
}

