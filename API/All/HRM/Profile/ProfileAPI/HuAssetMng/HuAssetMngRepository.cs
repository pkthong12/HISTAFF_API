using CORE.GenericUOW;
using CORE.DTO;
using API.DTO;
using CORE.Enum;
using CORE.StaticConstant;
using API.All.DbContexts;
using CORE.AutoMapper;
using System;

namespace API.Controllers.HuAssetMng
{
    public class HuAssetMngRepository : IHuAssetMngRepository
    {
        private readonly GenericUnitOfWork _uow;
        private readonly FullDbContext _dbContext;
        private IGenericRepository<HU_ASSET_MNG, HuAssetMngDTO> _genericRepository;
        private readonly GenericReducer<HU_ASSET_MNG, HuAssetMngDTO> _genericReducer;

        public HuAssetMngRepository(FullDbContext context, GenericUnitOfWork uow)
        {
            _dbContext = context;
            _uow = uow;
            _genericRepository = _uow.GenericRepository<HU_ASSET_MNG, HuAssetMngDTO>();
            _genericReducer = new();
        }

        public async Task<GenericPhaseTwoListResponse<HuAssetMngDTO>> SinglePhaseQueryList(GenericQueryListDTO<HuAssetMngDTO> request)
        {
            var joined = from p in _dbContext.HuAssetMngs.AsNoTracking()
                             // JOIN OTHER ENTITIES BASED ON THE BUSINESS
                         from e in _dbContext.HuEmployees.AsNoTracking().Where(x => x.ID == p.EMPLOYEE_ID).DefaultIfEmpty()
                         from o in _dbContext.HuOrganizations.AsNoTracking().Where(x => x.ID == e.ORG_ID).DefaultIfEmpty()
                         from pos in _dbContext.HuPositions.AsNoTracking().Where(x => x.ID == e.POSITION_ID).DefaultIfEmpty()
                         from job in _dbContext.HuJobs.AsNoTracking().Where(x => x.ID == pos.JOB_ID).DefaultIfEmpty()
                         from asset in _dbContext.HuAssets.AsNoTracking().Where(x => x.ID == p.ASSET_ID).DefaultIfEmpty()
                         from sys in _dbContext.SysOtherLists.AsNoTracking().Where(x => x.ID == asset.GROUP_ASSET_ID).DefaultIfEmpty()
                         from syss in _dbContext.SysOtherLists.AsNoTracking().Where(x => x.ID == p.STATUS_ASSET_ID).DefaultIfEmpty()
                         from sysOff in _dbContext.SysOtherLists.AsNoTracking().Where(c => c.CODE == "ESQ").DefaultIfEmpty()
                         from v in _dbContext.SysOtherLists.AsNoTracking().Where(c => c.ID == e.WORK_STATUS_ID).DefaultIfEmpty()
                         select new HuAssetMngDTO
                         {
                             Id = p.ID,
                             EmployeeId = p.EMPLOYEE_ID,
                             OrgId = e.ORG_ID, 
                             EmployeeCode = e.CODE,
                             FullName = e.Profile!.FULL_NAME,
                             PositionName = pos.NAME,
                             OrganizationName = o.NAME,
                             JobName = job.NAME_VN,
                             AssetId = asset.ID,
                             AssetName = asset.NAME,
                             GroupAssetName = sys.NAME,
                             ValueAsset = p.VALUE_ASSET,
                             DateIssue = p.DATE_ISSUE,
                             RevocationDate = p.REVOCATION_DATE,
                             StatusAssetId = p.STATUS_ASSET_ID,
                             StatusAssetName = syss.NAME,
                             IsLeaveWork = v.CODE == "ESQ" ? true : false,
                             Note = p.NOTE,
                             WorkStatusId = v.ID,
                             WorkStatusName = v.NAME,
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
            var joined = await (from p in _dbContext.HuAssetMngs.AsNoTracking().Where(x=> x.ID == id)
                             // JOIN OTHER ENTITIES BASED ON THE BUSINESS
                         from e in _dbContext.HuEmployees.AsNoTracking().Where(x => x.ID == p.EMPLOYEE_ID).DefaultIfEmpty()
                         from o in _dbContext.HuOrganizations.AsNoTracking().Where(x => x.ID == e.ORG_ID).DefaultIfEmpty()
                         from pos in _dbContext.HuPositions.AsNoTracking().Where(x => x.ID == e.POSITION_ID).DefaultIfEmpty()
                         from job in _dbContext.HuJobs.AsNoTracking().Where(x => x.ID == pos.JOB_ID).DefaultIfEmpty()
                         from asset in _dbContext.HuAssets.AsNoTracking().Where(x => x.ID == p.ASSET_ID).DefaultIfEmpty()
                         from sys in _dbContext.SysOtherLists.AsNoTracking().Where(x => x.ID == asset.GROUP_ASSET_ID).DefaultIfEmpty()
                         from syss in _dbContext.SysOtherLists.AsNoTracking().Where(x => x.ID == p.STATUS_ASSET_ID).DefaultIfEmpty()
                         select new HuAssetMngDTO
                         {
                             Id = p.ID,
                             EmployeeId = p.EMPLOYEE_ID,
                             EmployeeCode = e.CODE,
                             FullName = e.Profile!.FULL_NAME,
                             PositionName = pos.NAME,
                             OrganizationName = o.NAME,
                             JobName = job.NAME_VN,
                             AssetId = asset.ID,
                             AssetName = asset.NAME,
                             GroupAssetName = sys.NAME,
                             ValueAsset = p.VALUE_ASSET,
                             DateIssue = p.DATE_ISSUE,
                             RevocationDate = p.REVOCATION_DATE,
                             StatusAssetId = p.STATUS_ASSET_ID,
                             StatusAssetName = syss.NAME,
                             Note = p.NOTE
                         }).FirstOrDefaultAsync();
            return new FormatedResponse() { InnerBody = joined };
        }

        public async Task<FormatedResponse> GetById(string id)
        {
            await Task.Run(() => null);
            throw new NotImplementedException();
        }

        public async Task<FormatedResponse> Create(GenericUnitOfWork _uow, HuAssetMngDTO dto, string sid)
        {
            var response = await _genericRepository.Create(_uow, dto, sid);
            return response;
        }

        public async Task<FormatedResponse> CreateRange(GenericUnitOfWork _uow, List<HuAssetMngDTO> dtos, string sid)
        {
            var add = new List<HuAssetMngDTO>();
            add.AddRange(dtos);
            var response = await _genericRepository.CreateRange(_uow, add, sid);
            return response;
        }

        public async Task<FormatedResponse> Update(GenericUnitOfWork _uow, HuAssetMngDTO dto, string sid, bool patchMode = true)
        {
            var response = await _genericRepository.Update(_uow, dto, sid, patchMode);
            return response;
        }

        public async Task<FormatedResponse> UpdateRange(GenericUnitOfWork _uow, List<HuAssetMngDTO> dtos, string sid, bool patchMode = true)
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
            var response = await _genericRepository.ToggleActiveIds(_uow, ids, valueToBind, sid);
            return response;
        }

        public async Task<FormatedResponse> GetAsset()
        {
            var query = await (from p in _dbContext.HuAssets.AsNoTracking().Where(x => x.IS_ACTIVE == true)
                               select new
                               {
                                   Id = p.ID,
                                   Name = p.NAME
                               }).ToListAsync();
            return new FormatedResponse() { InnerBody = query };
        }

        public async Task<FormatedResponse> GetStatusAsset()
        {
            var code = await (from p in _dbContext.SysOtherListTypes.AsNoTracking().Where(x => x.CODE == "STATUS_ASSET") select (p.ID)).FirstAsync();
            var query = await (from a in _dbContext.SysOtherLists.AsNoTracking().Where(x => x.TYPE_ID == code)
                               select new
                               {
                                   Id = a.ID,
                                   Name = a.NAME
                               }).ToListAsync();
            return new FormatedResponse() { InnerBody = query };
        }
    }
}

