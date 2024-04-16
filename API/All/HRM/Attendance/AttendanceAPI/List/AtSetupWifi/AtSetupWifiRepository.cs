using CORE.GenericUOW;
using CORE.DTO;
using API.DTO;
using CORE.Enum;
using CORE.StaticConstant;
using API.All.DbContexts;
using CORE.AutoMapper;

namespace API.Controllers.AtSetupWifi
{
    public class AtSetupWifiRepository : IAtSetupWifiRepository
    {
        private readonly GenericUnitOfWork _uow;
        private readonly FullDbContext _dbContext;
        private IGenericRepository<AT_SETUP_WIFI, AtSetupWifiDTO> _genericRepository;
        private readonly GenericReducer<AT_SETUP_WIFI, AtSetupWifiDTO> _genericReducer;

        public AtSetupWifiRepository(FullDbContext context, GenericUnitOfWork uow)
        {
            _dbContext = context;
            _uow = uow;
            _genericRepository = _uow.GenericRepository<AT_SETUP_WIFI, AtSetupWifiDTO>();
            _genericReducer = new();
        }

        public async Task<GenericPhaseTwoListResponse<AtSetupWifiDTO>> SinglePhaseQueryList(GenericQueryListDTO<AtSetupWifiDTO> request)
        {
            var joined = from p in _dbContext.AtSetupWifis.AsNoTracking()
                         from o in _dbContext.HuOrganizations.AsNoTracking().Where(x => x.ID == p.ORG_ID).DefaultIfEmpty()
                         // JOIN OTHER ENTITIES BASED ON THE BUSINESS
                         select new AtSetupWifiDTO
                         {
                             Id = p.ID,
                             OrgName = o.NAME,
                             OrgId = p.ORG_ID,
                             NameVn = p.NAME_VN,
                             NameWifi = p.NAME_WIFI,
                             Ip = p.IP,
                             Status = p.IS_ACTIVE == true ? "Áp dụng" : "Ngừng áp dụng",
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
            var query = await (from p in _dbContext.AtSetupWifis.AsNoTracking().Where(x => x.ID == id)
                               from o in _dbContext.HuOrganizations.AsNoTracking().Where(x => x.ID == p.ORG_ID).DefaultIfEmpty()
                               select new AtSetupWifiDTO
                               {
                                   Id = p.ID,
                                   OrgId = p.ORG_ID,
                                   OrgName = o.NAME,
                                   NameVn = p.NAME_VN,
                                   NameWifi = p.NAME_WIFI,
                                   Ip = p.IP,
                                   Note = p.NOTE
                               }).FirstOrDefaultAsync();
            if (query != null)
            {
                return new FormatedResponse() { InnerBody = query };
            }
            else
            {
                return new FormatedResponse() { MessageCode = CommonMessageCode.ENTITY_NOT_FOUND, ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode500 };
            }
        }

        public async Task<FormatedResponse> GetById(string id)
        {
            await Task.Run(() => null);
            throw new NotImplementedException();
        }

        public async Task<FormatedResponse> Create(GenericUnitOfWork _uow, AtSetupWifiDTO dto, string sid)
        {
            dto.IsActive = true;
            var response = await _genericRepository.Create(_uow, dto, sid);
            return response;
        }

        public async Task<FormatedResponse> CreateRange(GenericUnitOfWork _uow, List<AtSetupWifiDTO> dtos, string sid)
        {
            var add = new List<AtSetupWifiDTO>();
            add.AddRange(dtos);
            var response = await _genericRepository.CreateRange(_uow, add, sid);
            return response;
        }

        public async Task<FormatedResponse> Update(GenericUnitOfWork _uow, AtSetupWifiDTO dto, string sid, bool patchMode = true)
        {
            var response = await _genericRepository.Update(_uow, dto, sid, patchMode);
            return response;
        }

        public async Task<FormatedResponse> UpdateRange(GenericUnitOfWork _uow, List<AtSetupWifiDTO> dtos, string sid, bool patchMode = true)
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
                var item = await _dbContext.AtSetupWifis.Where(x => x.ID == id).AsNoTracking().SingleOrDefaultAsync();
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

        public virtual async Task<FormatedResponse> ToggleActiveIds(GenericUnitOfWork _uow, List<long> ids, bool valueToBind, string sid)
        {
            var response = await _genericRepository.ToggleActiveIds(_uow, ids, valueToBind, sid);
            return response;
        }
    }
}

