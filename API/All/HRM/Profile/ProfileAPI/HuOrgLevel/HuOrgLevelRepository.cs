using CORE.GenericUOW;
using CORE.DTO;
using API.DTO;
using CORE.Enum;
using CORE.StaticConstant;
using API.All.DbContexts;
using CORE.AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Common.Extensions;

namespace API.Controllers.HuOrgLevel
{
    public class HuOrgLevelRepository : IHuOrgLevelRepository
    {
        private readonly GenericUnitOfWork _uow;
        private readonly FullDbContext _dbContext;
        private IGenericRepository<HU_ORG_LEVEL, HuOrgLevelDTO> _genericRepository;
        private readonly GenericReducer<HU_ORG_LEVEL, HuOrgLevelDTO> _genericReducer;

        public HuOrgLevelRepository(FullDbContext context, GenericUnitOfWork uow)
        {
            _dbContext = context;
            _uow = uow;
            _genericRepository = _uow.GenericRepository<HU_ORG_LEVEL, HuOrgLevelDTO>();
            _genericReducer = new();
        }

        public async Task<GenericPhaseTwoListResponse<HuOrgLevelDTO>> SinglePhaseQueryList(GenericQueryListDTO<HuOrgLevelDTO> request)
        {
            var joined = from p in _dbContext.HuOrgLevels.AsNoTracking()
                         // JOIN OTHER ENTITIES BASED ON THE BUSINESS
                         select new HuOrgLevelDTO
                         {
                             Id = p.ID,
                             Code = p.CODE,
                             Name = p.NAME,
                             OrderNum = p.ORDER_NUM,
                             Note = p.NOTE,
                             Status = p.IS_ACTIVE == true ?"Áp dụng" : "Ngừng áp dụng",
                         };

            var singlePhaseResult = await _genericReducer.SinglePhaseReduce(joined, request);
            return singlePhaseResult;
        }

        public async Task<FormatedResponse> ReadAll()
        {
            // var response = await _genericRepository.ReadAll();

            var response = await _dbContext.HuOrgLevels.Where(x => x.IS_ACTIVE == true).ToListAsync();

            return new FormatedResponse()
            {
                InnerBody = response
            };
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
                var list = new List<HU_ORG_LEVEL>
                    {
                        (HU_ORG_LEVEL)response
                    };
                var joined = (from l in list
                              // JOIN OTHER ENTITIES BASED ON THE BUSINESS
                              select new HuOrgLevelDTO
                              {
                                  Id = l.ID,
                                  Code = l.CODE,
                                  Name = l.NAME,
                                  OrderNum = l.ORDER_NUM,
                                  Note = l.NOTE,
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

        public async Task<FormatedResponse> Create(GenericUnitOfWork _uow, HuOrgLevelDTO dto, string sid)
        {
            var response = await _genericRepository.Create(_uow, dto, sid);
            return response;
        }

        public async Task<FormatedResponse> CreateRange(GenericUnitOfWork _uow, List<HuOrgLevelDTO> dtos, string sid)
        {
            var add = new List<HuOrgLevelDTO>();
            add.AddRange(dtos);
            var response = await _genericRepository.CreateRange(_uow, add, sid);
            return response;
        }

        public async Task<FormatedResponse> Update(GenericUnitOfWork _uow, HuOrgLevelDTO dto, string sid, bool patchMode = true)
        {
            var response = await _genericRepository.Update(_uow, dto, sid, patchMode);
            return response;
        }

        public async Task<FormatedResponse> UpdateRange(GenericUnitOfWork _uow, List<HuOrgLevelDTO> dtos, string sid, bool patchMode = true)
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
                bool checkDataBeforeDelete = false;
                ids.ForEach(item =>
                {
                    var getDataToCheck = _dbContext.HuOrganizations.Where(x => x.ORG_LEVEL_ID == item).ToList();
                    if (getDataToCheck.Count > 0 )
                    {
                        checkDataBeforeDelete = true;
                        return;
                    }
                });
                if (checkDataBeforeDelete == true)
                {
                    return new FormatedResponse() { ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400, MessageCode = CommonMessageCodes.CAN_NOT_DELETE_RECORD_WHEN_HAVE_USED };
                }
                else
                {
                    var response = await _genericRepository.DeleteIds(_uow, ids);
                    return response;
                }
            }
            catch (Exception ex)
            {

                return new FormatedResponse() { ErrorType = EnumErrorType.UNCATCHABLE, StatusCode = EnumStatusCode.StatusCode500, MessageCode = ex.Message };
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

