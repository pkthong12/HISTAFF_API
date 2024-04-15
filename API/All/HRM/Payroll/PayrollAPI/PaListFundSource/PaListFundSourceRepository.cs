using CORE.GenericUOW;
using CORE.DTO;
using API.DTO;
using CORE.Enum;
using CORE.StaticConstant;
using API.All.DbContexts;
using CORE.AutoMapper;
using OfficeOpenXml.FormulaParsing.Utilities;

namespace API.Controllers.PaListFundSource
{
    public class PaListFundSourceRepository : IPaListFundSourceRepository
    {
        private readonly GenericUnitOfWork _uow;
        private readonly FullDbContext _dbContext;
        private IGenericRepository<PA_LIST_FUND_SOURCE, PaListFundSourceDTO> _genericRepository;
        private readonly GenericReducer<PA_LIST_FUND_SOURCE, PaListFundSourceDTO> _genericReducer;

        public PaListFundSourceRepository(FullDbContext context, GenericUnitOfWork uow)
        {
            _dbContext = context;
            _uow = uow;
            _genericRepository = _uow.GenericRepository<PA_LIST_FUND_SOURCE, PaListFundSourceDTO>();
            _genericReducer = new();
        }

        public async Task<GenericPhaseTwoListResponse<PaListFundSourceDTO>> SinglePhaseQueryList(GenericQueryListDTO<PaListFundSourceDTO> request)
        {
            var joined = from p in _dbContext.PaListFundSources.AsNoTracking()
                             // JOIN OTHER ENTITIES BASED ON THE BUSINESS
                         from c in _dbContext.HuCompanys.AsNoTracking().Where(c => p.COMPANY_ID == null ? false : c.ID == p.COMPANY_ID).DefaultIfEmpty()
                         select new PaListFundSourceDTO
                         {
                             Id = p.ID,
                             Code = p.CODE,
                             Name = p.NAME,
                             CompanyId = c.ID,
                             CompanyName = c.NAME_VN,
                             Status = p.IS_ACTIVE.Value ? "Áp dụng" : "Ngừng áp dụng",
                             Note = p.NOTE,
                         };

            var singlePhaseResult = await _genericReducer.SinglePhaseReduce(joined, request);
            return singlePhaseResult;
        }

        public async Task<FormatedResponse> ReadAll()
        {
            var response = await _genericRepository.ReadAll();
            return response;
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

            var joined = (from l in _dbContext.PaListFundSources.AsNoTracking().Where(l => l.ID == id)
                              // JOIN OTHER ENTITIES BASED ON THE BUSINESS
                          from p in _dbContext.HuCompanys.AsNoTracking().Where(p => l.COMPANY_ID == null ? false : p.ID == l.COMPANY_ID).DefaultIfEmpty()
                          from c in _dbContext.SysUsers.AsNoTracking().Where(c => l.CREATED_BY == null ? false : c.ID == l.CREATED_BY).DefaultIfEmpty()
                          from u in _dbContext.SysUsers.AsNoTracking().Where(u => l.UPDATED_BY == null ? false : u.ID == l.UPDATED_BY).DefaultIfEmpty()
                          select new PaListFundSourceDTO
                          {
                              Id = l.ID,
                              Code = l.CODE,
                              Name = l.NAME,
                              CompanyId = l.COMPANY_ID,
                              CompanyName = p.NAME_VN,
                              Status = l.IS_ACTIVE.HasValue ? "Áp dụng" : "Ngừng áp dụng",
                              Note = l.NOTE,
                              CreatedByUsername = c.USERNAME,
                              CreatedDate = l.CREATED_DATE,
                              UpdatedByUsername = u.USERNAME,
                              UpdatedDate = l.UPDATED_DATE,
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

        public async Task<FormatedResponse> Create(GenericUnitOfWork _uow, PaListFundSourceDTO dto, string sid)
        {
            try
            {
                var x = await _dbContext.PaListFundSources.AsNoTracking().Where(x => 
                                                        x.NAME.ToLower().Trim() == dto.Name.ToLower().Trim() 
                                                                && x.COMPANY_ID == dto.CompanyId).AnyAsync();
                if (x)
                {
                    return new() { ErrorType = EnumErrorType.CATCHABLE, MessageCode = CommonMessageCode.NAME_EXISTS, StatusCode = EnumStatusCode.StatusCode400, InnerBody = dto };
                    //return new FormatedResponse()
                    //{
                    //    MessageCode = CommonMessageCode.NAME_EXISTS,
                    //    InnerBody = dto,
                    //    StatusCode = EnumStatusCode.StatusCode400
                    //};
                }
                string newCode = "";
                if (await _dbContext.PaListFundSources.CountAsync() == 0)
                {
                    newCode = "NQ001";
                }
                else
                {
                    string lastestData = _dbContext.PaListFundSources.OrderByDescending(t => t.CODE).First().CODE!.ToString();
                    newCode = lastestData.Substring(0, 2) + (int.Parse(lastestData.Substring(lastestData.Length - 3)) + 1).ToString("D3");
                }
                dto.Code = newCode;
                dto.IsActive = true;
                var response = await _genericRepository.Create(_uow, dto, sid);
                return response;
            }
            catch(Exception ex)
            {
                return new FormatedResponse()
                {
                    MessageCode = CommonMessageCode.POST_UPDATE_FAILLED,
                    StatusCode = EnumStatusCode.StatusCode500
                };
            }
        }

        public async Task<FormatedResponse> CreateRange(GenericUnitOfWork _uow, List<PaListFundSourceDTO> dtos, string sid)
        {
            var add = new List<PaListFundSourceDTO>();
            add.AddRange(dtos);
            var response = await _genericRepository.CreateRange(_uow, add, sid);
            return response;
        }

        public async Task<FormatedResponse> Update(GenericUnitOfWork _uow, PaListFundSourceDTO dto, string sid, bool patchMode = true)
        {
            
            try
            {
                var x = await _dbContext.PaListFundSources.AsNoTracking().Where(x =>
                                                        x.NAME.ToLower().Trim() == dto.Name.ToLower().Trim()
                                                                && x.COMPANY_ID == dto.CompanyId && x.ID !=dto.Id).AnyAsync();
                if (x)
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
        }

        public async Task<FormatedResponse> UpdateRange(GenericUnitOfWork _uow, List<PaListFundSourceDTO> dtos, string sid, bool patchMode = true)
        {
            var response = await _genericRepository.UpdateRange(_uow, dtos, sid, patchMode);
            return response;
        }

        public async Task<FormatedResponse> Delete(GenericUnitOfWork _uow, long id)
        {
            try
            {
                var response = await _genericRepository.Delete(_uow, id);
                if (response.StatusCode != EnumStatusCode.StatusCode500)
                {
                    return new FormatedResponse()
                    {
                        MessageCode = CommonMessageCode.DO_NOT_DELETE_DATA_IS_ACTIVE,
                    };
                }
                return response;
            }
            catch (Exception ex)
            {
                return new FormatedResponse()
                {
                    MessageCode = ex.ToString(),
                    StatusCode = EnumStatusCode.StatusCode400
                };
            }
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
                foreach(var item in ids)
                {
                    var query = _dbContext.PaListFundSources.Where(x => x.ID == item && x.IS_ACTIVE == true).Any();
                    if (query)
                    {
                        return new FormatedResponse()
                        {
                            MessageCode = CommonMessageCode.CAN_NOT_DELETE_RECORD_IS_ACTIVE,
                            StatusCode = EnumStatusCode.StatusCode400
                        };
                    }
                }
                var response = await _genericRepository.DeleteIds(_uow, ids);
                return response;
            }
            catch (Exception ex)
            {
                return new FormatedResponse()
                {
                    MessageCode = ex.ToString(),
                    StatusCode = EnumStatusCode.StatusCode400
                };
            }
        }

        public async Task<FormatedResponse> DeleteIds(GenericUnitOfWork _uow, List<string> ids)
        {
            await Task.Run(() => null);
            throw new NotImplementedException();
        }
        public async Task<FormatedResponse> GetCompany()
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
        }public async Task<FormatedResponse> GetCompanyById(long id)
        {
            var query = await (from o in _dbContext.HuCompanys
                               where o.IS_ACTIVE == true && o.ID == id
                               select new
                               {
                                   Id = o.ID,
                                   Name = o.NAME_VN,
                                   Code = o.CODE,
                               }).FirstOrDefaultAsync();
            return new FormatedResponse() { InnerBody = query };
        }
        public async Task<FormatedResponse> CreateNewCode()
        {
            string newCode = "";
            if (await _dbContext.PaListFundSources.CountAsync() == 0)
            {
                newCode = "NQ001";
            }
            else
            {
                string lastestData = _dbContext.PaListFundSources.OrderByDescending(t => t.CODE).First().CODE!.ToString();
                newCode = lastestData.Substring(0, 2) + (int.Parse(lastestData.Substring(lastestData.Length - 3)) + 1).ToString("D3");
            }
            return new FormatedResponse() { InnerBody = new { Code = newCode } };
        }

        public virtual async Task<FormatedResponse> ToggleActiveIds(GenericUnitOfWork _uow, List<long> ids, bool valueToBind, string sid)
        {
            var response = await _genericRepository.ToggleActiveIds(_uow,ids, valueToBind, sid);
            return new FormatedResponse()
            {
                MessageCode = response.MessageCode,
                InnerBody = response.InnerBody,
                StatusCode = EnumStatusCode.StatusCode200
            };
        }
    }
}

