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
using System.Linq;
using System.Security.Cryptography;

namespace API.Controllers.HuContractType
{
    public class HuContractTypeRepository : IHuContractTypeRepository
    {
        private readonly GenericUnitOfWork _uow;
        private readonly FullDbContext _dbContext;
        private IGenericRepository<HU_CONTRACT_TYPE, HuContractTypeDTO> _genericRepository;
        private readonly GenericReducer<HU_CONTRACT_TYPE, HuContractTypeDTO> _genericReducer;

        public HuContractTypeRepository(FullDbContext context, GenericUnitOfWork uow)
        {
            _dbContext = context;
            _uow = uow;
            _genericRepository = _uow.GenericRepository<HU_CONTRACT_TYPE, HuContractTypeDTO>();
            _genericReducer = new();
        }

        public async Task<GenericPhaseTwoListResponse<HuContractTypeDTO>> SinglePhaseQueryList(GenericQueryListDTO<HuContractTypeDTO> request)
        {
            var joined = from c in _dbContext.HuContractTypes
                         join ct in _dbContext.SysContractTypes on c.TYPE_ID equals ct.ID into contract
                         orderby c.ID descending
                         //from ct in _dbContext.SysContractTypes.Where(ct => ct.ID == p.TYPE_ID).DefaultIfEmpty()
                         from p in contract.DefaultIfEmpty()
                         select new HuContractTypeDTO
                         {
                             Id = c.ID,
                             Code = c.CODE,
                             Name = c.NAME,
                             Period = c.PERIOD == 0 ? null : c.PERIOD,
                             TypeId = c.TYPE_ID,
                             TypeName = p.NAME,
                             DayNotice = c.DAY_NOTICE,
                             Note = c.NOTE,
                             IsLeave = c.IS_LEAVE,
                             IsBhxh = c.IS_BHXH,
                             IsBhyt = c.IS_BHYT,
                             IsBhtn = c.IS_BHTN,
                             IsBhtnldBnn = c.IS_BHTNLD_BNN,
                             Status = c.IS_ACTIVE == true ? "Áp dụng" : "Ngừng áp dụng",
                         };

            var singlePhaseResult = await _genericReducer.SinglePhaseReduce(joined, request);
            return singlePhaseResult;
        }

        public async Task<FormatedResponse> ReadAll()
        {
            try
            {
                var queryable = await (from p in _dbContext.HuContractTypes
                                       where p.IS_ACTIVE == true
                                       orderby p.CODE
                                       select new
                                       {
                                           Id = p.ID,
                                           Name = p.NAME,
                                           Code = p.CODE,
                                           Month = p.PERIOD
                                       }).ToListAsync();
                return new FormatedResponse() { InnerBody = queryable };
            }
            catch (Exception ex)
            {
                return new FormatedResponse() { MessageCode = ex.Message, ErrorType = EnumErrorType.UNCATCHABLE, StatusCode = EnumStatusCode.StatusCode500 };
            }

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
                var list = new List<HU_CONTRACT_TYPE>
                    {
                        (HU_CONTRACT_TYPE)response
                    };
                var joined = (from l in list
                              select new HuContractTypeDTO
                              {
                                  Id = l.ID,
                                  Code = l.CODE,
                                  Name = l.NAME,
                                  DayNotice = l.DAY_NOTICE,
                                  Note = l.NOTE,
                                  Period = l.PERIOD == 0 ? null : l.PERIOD,
                                  TypeId = l.TYPE_ID,
                                  IsBhxh = l.IS_BHXH,
                                  IsBhyt = l.IS_BHYT,
                                  IsBhtn = l.IS_BHTN,
                                  IsBhtnldBnn = l.IS_BHTNLD_BNN,
                                  IsActive = l.IS_ACTIVE,
                                  IsLeave = l.IS_LEAVE,
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

        public async Task<FormatedResponse> CheckCodeExists(string code)
        {
            var checkExists = _dbContext.HuContractTypes.Where(p => p.CODE == code).Count();
            if(checkExists != 0)
            {
                string newCode = "";
                var listNumberOfCode = new List<int>(); 
                var listSameCode = await (from c in _dbContext.HuContractTypes.Where(p => p.CODE!.StartsWith(code)) select c.CODE).ToListAsync();
                for (int i = listSameCode.Count - 1; i >= 0; i--)
                {
                    if (listSameCode[i].Substring(code.Length) == "" || listSameCode[i].Substring(code.Length).All(Char.IsDigit) == false)
                    {
                        listSameCode.RemoveAt(i);
                    }
                    else
                    {
                        listNumberOfCode.Add(Int32.Parse(listSameCode[i].Substring(code.Length)));
                    }
                }
                if(listNumberOfCode.Count() == 0)
                {
                    newCode = code + "001";
                }
                else
                {
                    string newNumber = (listNumberOfCode.Max() + 1).ToString();
                    while (newNumber.Length < 3)
                    {
                        newNumber = "0" + newNumber;
                    }
                    newCode = code + newNumber;
                }
                return new FormatedResponse() { InnerBody = new { Code = newCode } };
            }
            return new FormatedResponse() { InnerBody = new { Code = code } };
        }

        public async Task<FormatedResponse> Create(GenericUnitOfWork _uow, HuContractTypeDTO dto, string sid)
        {
            var contractType = _dbContext.SysContractTypes.Where(p => p.ID == dto.TypeId).DefaultIfEmpty();
            if(contractType.First()!.CODE != "HDLD004")
            {
                if(dto.Period == null)
                {
                    return new FormatedResponse() { MessageCode = CommonMessageCode.PERIOD_REQUIRED, ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400 };
                }
            }
            if(_dbContext.HuContractTypes.Where(p => p.CODE == dto.Code).Count() == 0)
            {
                var response = await _genericRepository.Create(_uow, dto, sid);
                return response;        
            }
            else
            {
                return new FormatedResponse() { MessageCode = CommonMessageCode.NAME_EXISTS, ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400 };
            }
        }

        public async Task<FormatedResponse> CreateRange(GenericUnitOfWork _uow, List<HuContractTypeDTO> dtos, string sid)
        {
            var add = new List<HuContractTypeDTO>();
            add.AddRange(dtos);
            var response = await _genericRepository.CreateRange(_uow, add, sid);
            return response;
        }

        public async Task<FormatedResponse> Update(GenericUnitOfWork _uow, HuContractTypeDTO dto, string sid, bool patchMode = true)
        {
            dto.Period = (dto.Period == null ? 0 : dto.Period);
            var contractType = _dbContext.SysContractTypes.Where(p => p.ID == dto.TypeId).DefaultIfEmpty();
            if (contractType.First()!.CODE != "HDLD004")
            {
                if (dto.Period == null)
                {
                    return new FormatedResponse() { MessageCode = CommonMessageCode.PERIOD_REQUIRED, ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400 };
                }
            }
            var response = await _genericRepository.Update(_uow, dto, sid, patchMode);
            return response;
        }

        public async Task<FormatedResponse> UpdateRange(GenericUnitOfWork _uow, List<HuContractTypeDTO> dtos, string sid, bool patchMode = true)
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
                var item = await _dbContext.HuContractTypes.Where(x => x.ID == id).AsNoTracking().SingleOrDefaultAsync();
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

        public async Task<FormatedResponse> GetListContractTypeSys()
        {
            var queryable = await (from c in _dbContext.SysContractTypes
                           where c.IS_ACTIVE == true 
                            orderby c.CODE
                            select new
                            {
                                Id = c.ID,
                                Name = c.NAME,
                                Code = c.CODE,
                            }).ToListAsync();
            return new FormatedResponse() { InnerBody = queryable };
        }

        public async Task<FormatedResponse> GetContractTypeSysById(long id)
        {
             
            var joined = await (from l in _dbContext.SysContractTypes.Where(l => l.ID == id)
                            select new SysContractTypeDTO
                            {
                                Id = l.ID,
                                Code = l.CODE,
                                Name = l.NAME,
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
        public async Task<FormatedResponse> GetContractAppendixType()
        {
            var queryable = await (from c in _dbContext.HuContractTypes.AsNoTracking()
                                   from ct in _dbContext.SysContractTypes.AsNoTracking().Where(ct => ct.ID == c.TYPE_ID).DefaultIfEmpty()
                           where c.IS_ACTIVE == true && ct.CODE=="PLHD"
                            orderby c.CODE
                            select new
                            {
                                Id = c.ID,
                                Name = c.NAME,
                                Code = c.CODE,
                            }).ToListAsync();
            return new FormatedResponse() { InnerBody = queryable };
        }

        public virtual async Task<FormatedResponse> ToggleActiveIds(GenericUnitOfWork _uow, List<long> ids, bool valueToBind, string sid)
        {
            var response = await _genericRepository.ToggleActiveIds(_uow, ids, valueToBind, sid);
            return response;
        }

        public async Task<FormatedResponse> GetContractDashboard()
        {
            var query = await (
                         from c in _dbContext.HuContractTypes.AsNoTracking()
                         from e in _dbContext.HuEmployees.AsNoTracking().Where(x => x.CONTRACT_TYPE_ID == c.ID).DefaultIfEmpty()
                         where e.WORK_STATUS_ID == OtherConfig.EMP_STATUS_WORKING
                         select new
                         {
                             Id = c.ID,
                             ContractName = c.NAME
                         })
                         .GroupBy(x => new { x.Id, x.ContractName})
                         .Select(x => new {
                             Name = x.Key.ContractName,
                             Y = x.Count()
                         }).ToListAsync();

            return new FormatedResponse() { InnerBody = query };
        }
    }
}

