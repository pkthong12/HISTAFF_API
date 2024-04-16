using CORE.GenericUOW;
using CORE.DTO;
using API.DTO;
using CORE.Enum;
using CORE.StaticConstant;
using API.All.DbContexts;
using CORE.AutoMapper;
using System.Collections.Generic;

namespace API.Controllers.PaListfund
{
    public class PaListfundRepository : IPaListfundRepository
    {
        private readonly GenericUnitOfWork _uow;
        private readonly FullDbContext _dbContext;
        private IGenericRepository<PA_LISTFUND, PaListfundDTO> _genericRepository;
        private readonly GenericReducer<PA_LISTFUND, PaListfundDTO> _genericReducer;

        public PaListfundRepository(FullDbContext context, GenericUnitOfWork uow)
        {
            _dbContext = context;
            _uow = uow;
            _genericRepository = _uow.GenericRepository<PA_LISTFUND, PaListfundDTO>();
            _genericReducer = new();
        }

        public async Task<GenericPhaseTwoListResponse<PaListfundDTO>> SinglePhaseQueryList(GenericQueryListDTO<PaListfundDTO> request)
        {
            var joined = from p in _dbContext.PaListfunds.AsNoTracking()
                         from c in _dbContext.HuCompanys.AsNoTracking().Where(c => p.COMPANY_ID == null ? false : c.ID == p.COMPANY_ID).DefaultIfEmpty()
                             // JOIN OTHER ENTITIES BASED ON THE BUSINESS
                         select new PaListfundDTO
                         {
                             Id = p.ID,
                             ListfundCode = p.LISTFUND_CODE,
                             ListfundName = p.LISTFUND_NAME,
                             CompanyId = c.ID,
                             CompanyName = c.NAME_VN,
                             Status = p.IS_ACTIVE == true ? "Áp dụng" : "Ngừng áp dụng",
                             Note = p.NOTE,
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
            var joined = (from l in _dbContext.PaListfunds.AsNoTracking().Where(l => l.ID == id)
                              // JOIN OTHER ENTITIES BASED ON THE BUSINESS
                          from c in _dbContext.HuCompanys.AsNoTracking().Where(p => l.COMPANY_ID == null ? false : p.ID == l.COMPANY_ID).DefaultIfEmpty()
                          
                          select new PaListfundDTO
                          {
                              Id = l.ID,
                              ListfundCode = l.LISTFUND_CODE,
                              ListfundName = l.LISTFUND_NAME,
                              CompanyId = c.ID,
                              CompanyName = c.NAME_VN,
                              Note = l.NOTE,
                          }).FirstOrDefault();

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

        public async Task<FormatedResponse> Create(GenericUnitOfWork _uow, PaListfundDTO dto, string sid)
        {
            try
            {
                var a = await _dbContext.PaListfunds.AsNoTracking()
                        .Where(a => a.LISTFUND_NAME.ToLower().Trim() == dto.ListfundName.ToLower().Trim()
                        && a.COMPANY_ID == dto.CompanyId).AnyAsync();
                if (a)
                {
                    //return new FormatedResponse()
                    //{
                    //    MessageCode = CommonMessageCode.NAME_EXISTS,
                    //    InnerBody = dto,
                    //    StatusCode = EnumStatusCode.StatusCode400
                    //};
                    return new() { ErrorType = EnumErrorType.CATCHABLE, MessageCode = CommonMessageCode.NAME_EXISTS, StatusCode = EnumStatusCode.StatusCode400, InnerBody = dto };
                }
                dto.IsActive = true;
                var response = await _genericRepository.Create(_uow, dto, sid);
                return response;
            }
            catch (Exception ex)
            {
                return new FormatedResponse()
                {
                    MessageCode = CommonMessageCode.POST_UPDATE_FAILLED,
                    StatusCode = EnumStatusCode.StatusCode500
                };
            }
        }

        public async Task<FormatedResponse> CreateRange(GenericUnitOfWork _uow, List<PaListfundDTO> dtos, string sid)
        {
            var add = new List<PaListfundDTO>();
            add.AddRange(dtos);
            var response = await _genericRepository.CreateRange(_uow, add, sid);
            return response;
        }

        public async Task<FormatedResponse> Update(GenericUnitOfWork _uow, PaListfundDTO dto, string sid, bool patchMode = true)
        {
            try
            {
                var check = await _dbContext.PaListfunds.AsNoTracking()
                        .Where(a => a.LISTFUND_NAME.ToLower().Trim() == dto.ListfundName.ToLower().Trim()
                        && a.COMPANY_ID == dto.CompanyId && dto.Id != a.ID).AnyAsync();
                if (check)
                {
                    //return new FormatedResponse()
                    //{
                    //    MessageCode = CommonMessageCode.NAME_EXISTS,
                    //    InnerBody = dto,
                    //    StatusCode = EnumStatusCode.StatusCode400
                    //};
                    return new() { ErrorType = EnumErrorType.CATCHABLE, MessageCode = CommonMessageCode.NAME_EXISTS, StatusCode = EnumStatusCode.StatusCode400, InnerBody = dto };
                }
                var response = await _genericRepository.Update(_uow, dto, sid, patchMode);
                return response;
            }
            catch (Exception ex)
            {
                return new FormatedResponse()
                {
                    MessageCode = CommonMessageCode.POST_UPDATE_FAILLED,
                    StatusCode = EnumStatusCode.StatusCode500
                };
            }
            //var response = await _genericRepository.Update(_uow, dto, sid, patchMode);
            //return response;
        }

        public async Task<FormatedResponse> UpdateRange(GenericUnitOfWork _uow, List<PaListfundDTO> dtos, string sid, bool patchMode = true)
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
                foreach( var item in ids)
                {
                    var query = _dbContext.PaListfunds.Where(x => x.ID == item && x.IS_ACTIVE == true).Any();
                    if (query)
                    {
                        return new FormatedResponse()
                        {
                            MessageCode = CommonMessageCode.CAN_NOT_DELETE_RECORD_IS_ACTIVE,
                            StatusCode = EnumStatusCode.StatusCode400,
                            ErrorType = EnumErrorType.CATCHABLE,
                        };
                    }
                }
                
                var response = await _genericRepository.DeleteIds(_uow, ids);
                return response;
            }
            catch(Exception ex)
            {
                return new FormatedResponse()
                {
                    MessageCode = ex.ToString(),
                    StatusCode = EnumStatusCode.StatusCode500
                };
            }
            //var response = await _genericRepository.DeleteIds(_uow, ids);
            //return response;
        }

        public async Task<FormatedResponse> DeleteIds(GenericUnitOfWork _uow, List<string> ids)
        {
            await Task.Run(() => null);
            throw new NotImplementedException();
        }

        public async Task<FormatedResponse> GetCompanyTypes()
        {
            var query = await (from o in _dbContext.HuCompanys
                               where o.IS_ACTIVE == true
                               orderby o.CODE
                               select new
                               {
                                   Id = o.ID,
                                   Name = o.NAME_VN,
                                   Code = o.CODE,
                               }).ToListAsync();
            return new FormatedResponse() { InnerBody = query };
        }

        public async Task<FormatedResponse> CreateCodeNew()
        {
            string newCode = "";
            if (await _dbContext.PaListfunds.CountAsync() == 0)
            {
                newCode = "QL001";
            }
            else
            {
                string lastestData = _dbContext.PaListfunds.OrderByDescending(t => t.LISTFUND_CODE).First().LISTFUND_CODE!.ToString();

                newCode = lastestData.Substring(0, 2) + (int.Parse(lastestData.Substring(lastestData.Length - 3)) + 1).ToString("D3");
            }
            return new FormatedResponse() { InnerBody = new { ListfundCode = newCode } };
        }

        public async Task<FormatedResponse> GetListFund()
        {
            var query = await (from s in _dbContext.PaListfunds.AsNoTracking()
                              .Where(s => s.IS_ACTIVE == true)
                               orderby s.ID
                               select new
                               {
                                   Id = s.ID,
                                   Name = s.LISTFUND_NAME,
                               }).ToListAsync();
            return new FormatedResponse() { InnerBody = query };
        }

        public async Task<FormatedResponse> GetListFundByPeriodId(long periodId)
        {
            var query = await (from s in _dbContext.PaListfunds.AsNoTracking().Where(s => s.IS_ACTIVE == true).DefaultIfEmpty()
                               from t in _dbContext.PaPayrollFunds.AsNoTracking().Where(t => t.LIST_FUND_ID == s.ID).DefaultIfEmpty()
                               where t.SALARY_PERIOD_ID == periodId
                               orderby s.ID
                               select new
                               {
                                   Id = s.ID,
                                   Name = s.LISTFUND_NAME,
                               }).ToListAsync();
            return new FormatedResponse() { InnerBody = query };
        }

        public virtual async Task<FormatedResponse> ToggleActiveIds(GenericUnitOfWork _uow, List<long> ids, bool valueToBind, string sid)
        {
            var response = await _genericRepository.ToggleActiveIds(_uow, ids, valueToBind, sid);
            return response;
        }
    }
}

