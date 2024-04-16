using CORE.GenericUOW;
using CORE.DTO;
using API.DTO;
using CORE.Enum;
using CORE.StaticConstant;
using API.All.DbContexts;
using CORE.AutoMapper;
using DocumentFormat.OpenXml.Office2010.Excel;

namespace API.Controllers.HuAsset
{
    public class HuAssetRepository : IHuAssetRepository
    {
        private readonly GenericUnitOfWork _uow;
        private readonly FullDbContext _dbContext;
        private IGenericRepository<HU_ASSET, HuAssetDTO> _genericRepository;
        private readonly GenericReducer<HU_ASSET, HuAssetDTO> _genericReducer;

        public HuAssetRepository(FullDbContext context, GenericUnitOfWork uow)
        {
            _dbContext = context;
            _uow = uow;
            _genericRepository = _uow.GenericRepository<HU_ASSET, HuAssetDTO>();
            _genericReducer = new();
        }

        public async Task<GenericPhaseTwoListResponse<HuAssetDTO>> SinglePhaseQueryList(GenericQueryListDTO<HuAssetDTO> request)
        {
            var joined = from p in _dbContext.HuAssets.AsNoTracking()
                             // JOIN OTHER ENTITIES BASED ON THE BUSINESS
                         from s in _dbContext.SysOtherLists.AsNoTracking().Where(x => x.ID == p.GROUP_ASSET_ID).DefaultIfEmpty()
                         select new HuAssetDTO
                         {
                             Id = p.ID,
                             Code = p.CODE,
                             Name = p.NAME,
                             GroupAssetId = p.GROUP_ASSET_ID,
                             GroupAssetName = s.NAME,
                             Note = p.NOTE,
                             Status = p.IS_ACTIVE == true ? "Áp dụng" : "Ngừng áp dụng",
                             IsActive = p.IS_ACTIVE,
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
            var joined = await (from p in _dbContext.HuAssets.AsNoTracking().Where(x => x.ID == id)
                                from s in _dbContext.SysOtherLists.AsNoTracking().Where(x => x.ID == p.GROUP_ASSET_ID).DefaultIfEmpty()
                                select new
                                {
                                    Id = p.ID,
                                    Code = p.CODE,
                                    Name = p.NAME,
                                    GroupAssetId = p.GROUP_ASSET_ID,
                                    Note = p.NOTE,
                                }).FirstOrDefaultAsync();
            return new FormatedResponse() { InnerBody = joined };
        }

        public async Task<FormatedResponse> GetById(string id)
        {
            await Task.Run(() => null);
            throw new NotImplementedException();
        }

        public async Task<FormatedResponse> Create(GenericUnitOfWork _uow, HuAssetDTO dto, string sid)
        {
            var response = await _genericRepository.Create(_uow, dto, sid);
            return response;
        }

        public async Task<FormatedResponse> CreateRange(GenericUnitOfWork _uow, List<HuAssetDTO> dtos, string sid)
        {
            var add = new List<HuAssetDTO>();
            add.AddRange(dtos);
            var response = await _genericRepository.CreateRange(_uow, add, sid);
            return response;
        }

        public async Task<FormatedResponse> Update(GenericUnitOfWork _uow, HuAssetDTO dto, string sid, bool patchMode = true)
        {
            var response = await _genericRepository.Update(_uow, dto, sid, patchMode);
            return response;
        }

        public async Task<FormatedResponse> UpdateRange(GenericUnitOfWork _uow, List<HuAssetDTO> dtos, string sid, bool patchMode = true)
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
            foreach (var item in ids)
            {
                var check = await _dbContext.HuAssets.AsNoTracking().Where(x => x.ID == item && x.IS_ACTIVE == true).AnyAsync();
                if (check)
                {
                    return new FormatedResponse()
                    {
                        ErrorType = EnumErrorType.CATCHABLE,
                        MessageCode = CommonMessageCode.CAN_NOT_DELETE_RECORD_IS_ACTIVE,
                        StatusCode = EnumStatusCode.StatusCode400
                    };
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

        public async Task<FormatedResponse> GetGroupAsset()
        {
            var idSysType = await (from p in _dbContext.SysOtherListTypes.AsNoTracking().Where(x => x.CODE == "ASSET_GROUP") select p.ID).FirstAsync();
            var query = await (from p in _dbContext.SysOtherLists.AsNoTracking().Where(x => x.TYPE_ID == idSysType && x.IS_ACTIVE == true)
                               select new
                               {
                                   Id = p.ID,
                                   Name = p.NAME
                               }).ToListAsync();
            return new FormatedResponse() { InnerBody = query };
        }
    }
}

