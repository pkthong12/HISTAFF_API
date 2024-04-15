using CORE.GenericUOW;
using CORE.DTO;
using API.DTO;
using CORE.Enum;
using CORE.StaticConstant;
using API.All.DbContexts;
using CORE.AutoMapper;

namespace API.Controllers.SysFunctionIgnore
{
    public class SysFunctionIgnoreRepository : ISysFunctionIgnoreRepository
    {
        private readonly GenericUnitOfWork _uow;
        private readonly FullDbContext _dbContext;
        private IGenericRepository<SYS_FUNCTION_IGNORE, SysFunctionIgnoreDTO> _genericRepository;
        private readonly GenericReducer<SYS_FUNCTION_IGNORE, SysFunctionIgnoreDTO> _genericReducer;

        public SysFunctionIgnoreRepository(FullDbContext context, GenericUnitOfWork uow)
        {
            _dbContext = context;
            _uow = uow;
            _genericRepository = _uow.GenericRepository<SYS_FUNCTION_IGNORE, SysFunctionIgnoreDTO>();
            _genericReducer = new();
        }

        public async Task<GenericPhaseTwoListResponse<SysFunctionIgnoreDTO>> SinglePhaseQueryList(GenericQueryListDTO<SysFunctionIgnoreDTO> request)
        {
            var joined = from p in _dbContext.SysFunctionIgnores.AsNoTracking()
                             // JOIN OTHER ENTITIES BASED ON THE BUSINESS
                         from c in _uow.Context.Set<SYS_USER>().AsNoTracking().AsQueryable().Where(x => x.ID == p.CREATED_BY).DefaultIfEmpty()
                         from u in _uow.Context.Set<SYS_USER>().AsNoTracking().AsQueryable().Where(x => x.ID == p.UPDATED_BY).DefaultIfEmpty()

                         select new SysFunctionIgnoreDTO
                         {
                             Id = p.ID,
                             Path = p.PATH,
                             CreatedDate = p.CREATED_DATE,
                             CreatedByUsername = c.USERNAME,
                             UpdatedDate = p.UPDATED_DATE,
                             UpdatedByUsername = u.USERNAME

                         };

            var singlePhaseResult = await _genericReducer.SinglePhaseReduce(joined, request);
            return singlePhaseResult;
        }

        public async Task<FormatedResponse> ReadAll()
        {
            var response = await _genericRepository.ReadAll();
            return response;
        }

        public async Task<FormatedResponse> ReadAllPathOnly()
        {
            try
            {
                var response = await _genericRepository.ReadAll();
                var pathList = new List<string>();
                if (response.InnerBody != null)
                {
                    var list = (List<SYS_FUNCTION_IGNORE>)response.InnerBody;
                    list.ForEach(item =>
                    {
                        pathList.Add(item.PATH);
                    });
                    return new() { InnerBody = pathList };
                }
                else
                {
                    throw new NullReferenceException();
                }
            } catch (Exception ex)
            {
                return new() { ErrorType = EnumErrorType.UNCATCHABLE, MessageCode = ex.Message, StatusCode = EnumStatusCode.StatusCode500 };
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
                var list = new List<SYS_FUNCTION_IGNORE>
                    {
                        (SYS_FUNCTION_IGNORE)response
                    };
                var joined = (from l in list
                              // JOIN OTHER ENTITIES BASED ON THE BUSINESS
                              select new SysFunctionIgnoreDTO
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

        public async Task<FormatedResponse> Create(GenericUnitOfWork _uow, SysFunctionIgnoreDTO dto, string sid)
        {
            var response = await _genericRepository.Create(_uow, dto, sid);
            return response;
        }

        public async Task<FormatedResponse> CreateRange(GenericUnitOfWork _uow, List<SysFunctionIgnoreDTO> dtos, string sid)
        {
            var add = new List<SysFunctionIgnoreDTO>();
            add.AddRange(dtos);
            var response = await _genericRepository.CreateRange(_uow, add, sid);
            return response;
        }

        public async Task<FormatedResponse> Update(GenericUnitOfWork _uow, SysFunctionIgnoreDTO dto, string sid, bool patchMode = true)
        {
            var response = await _genericRepository.Update(_uow, dto, sid, patchMode);
            return response;
        }

        public async Task<FormatedResponse> UpdateRange(GenericUnitOfWork _uow, List<SysFunctionIgnoreDTO> dtos, string sid, bool patchMode = true)
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
            await Task.Run(() => null);
            throw new NotImplementedException();
        }


    }
}

