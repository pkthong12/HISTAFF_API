using CORE.GenericUOW;
using CORE.DTO;
using API.DTO;
using CORE.Enum;
using CORE.StaticConstant;
using API.All.DbContexts;
using CORE.AutoMapper;

namespace API.Controllers.InsRegimes
{
    public class InsRegimesRepository : IInsRegimesRepository
    {
        private readonly GenericUnitOfWork _uow;
        private readonly FullDbContext _dbContext;
        private IGenericRepository<INS_REGIMES, InsRegimesDTO> _genericRepository;
        private readonly GenericReducer<INS_REGIMES, InsRegimesDTO> _genericReducer;

        public InsRegimesRepository(FullDbContext context, GenericUnitOfWork uow)
        {
            _dbContext = context;
            _uow = uow;
            _genericRepository = _uow.GenericRepository<INS_REGIMES, InsRegimesDTO>();
            _genericReducer = new();
        }

        public async Task<GenericPhaseTwoListResponse<InsRegimesDTO>> SinglePhaseQueryList(GenericQueryListDTO<InsRegimesDTO> request)
        {
            var joined = from p in _dbContext.InsRegimess.AsNoTracking()
                         from g in _dbContext.InsGroups.AsNoTracking().Where(g => g.ID == p.INS_GROUP_ID).DefaultIfEmpty()
                         from c in _dbContext.SysUsers.AsNoTracking().Where(c => p.CREATED_BY == null ? false : c.ID == p.CREATED_BY).DefaultIfEmpty()
                         from u in _dbContext.SysUsers.AsNoTracking().Where(u => p.UPDATED_BY == null ? false : u.ID == p.UPDATED_BY).DefaultIfEmpty()
                         from o in _dbContext.SysOtherLists.AsNoTracking().Where(o=> o.ID == p.CAL_DATE_TYPE).DefaultIfEmpty()
                             // JOIN OTHER ENTITIES BASED ON THE BUSINESS
                         select new InsRegimesDTO
                         {
                             Id = p.ID,
                             Name = p.NAME,
                             Code = p.CODE,
                             InsGroupId = p.INS_GROUP_ID,
                             InsGroupName = g.NAME,
                             TotalDay = p.TOTAL_DAY,
                             BenefitsLevels = p.BENEFITS_LEVELS,
                             CalDateType = p.CAL_DATE_TYPE,
                             CalDateTypeString = o.NAME,
                             IsActive = p.IS_ACTIVE,
                             Status = p.IS_ACTIVE == true ? "Áp dụng" : "Ngừng áp dụng",
                             Note = p.NOTE,
                             CreatedBy = p.CREATED_BY,
                             CreatedByUsername = c.USERNAME,
                             CreatedDate = p.CREATED_DATE,
                             UpdatedBy = p.UPDATED_BY,
                             UpdatedByUsername = u.USERNAME,
                             UpdatedDate = p.UPDATED_DATE,
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
            var joined = (from p in _dbContext.InsRegimess.AsNoTracking().Where(p => p.ID == id)
                          from g in _dbContext.InsGroups.AsNoTracking().Where(g => g.ID == p.INS_GROUP_ID).DefaultIfEmpty()
                          from c in _dbContext.SysUsers.AsNoTracking().Where(c => p.CREATED_BY == null ? false : c.ID == p.CREATED_BY).DefaultIfEmpty()
                          from u in _dbContext.SysUsers.AsNoTracking().Where(u => p.UPDATED_BY == null ? false : u.ID == p.UPDATED_BY).DefaultIfEmpty()
                          from o in _dbContext.SysOtherLists.AsNoTracking().Where(o => o.ID == p.CAL_DATE_TYPE).DefaultIfEmpty()
                              // JOIN OTHER ENTITIES BASED ON THE BUSINESS
                          select new InsRegimesDTO
                          {
                              Id = p.ID,
                              Name = p.NAME,
                              Code = p.CODE,
                              InsGroupId = p.INS_GROUP_ID,
                              InsGroupName = g.NAME,
                              CalDateType = p.CAL_DATE_TYPE,
                              CalDateTypeString = o.NAME,
                              TotalDay = p.TOTAL_DAY,
                              BenefitsLevels = p.BENEFITS_LEVELS,
                              Note = p.NOTE,
                              IsActive = p.IS_ACTIVE,
                              CreatedBy = p.CREATED_BY,
                              CreatedByUsername = c.USERNAME,
                              CreatedDate = p.CREATED_DATE,
                              UpdatedBy = p.UPDATED_BY,
                              UpdatedByUsername = u.USERNAME,
                              UpdatedDate = p.UPDATED_DATE,
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

        public async Task<FormatedResponse> Create(GenericUnitOfWork _uow, InsRegimesDTO dto, string sid)
        {
            try
            {
                var c = await _dbContext.InsRegimess.AsNoTracking().Where(x => x.CODE == dto.Code).FirstOrDefaultAsync();
                if (c != null)
                {
                    return new FormatedResponse() { MessageCode = CommonMessageCode.CREATE_OBJECT_HAS_SAME_CODE, ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400 };
                }
                var response = await _genericRepository.Create(_uow, dto, sid);
                return response;
            }
            catch (Exception ex)
            {
                return new FormatedResponse() { MessageCode = ex.ToString(), ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400 };
            }

        }

        public async Task<FormatedResponse> CreateRange(GenericUnitOfWork _uow, List<InsRegimesDTO> dtos, string sid)
        {
            var add = new List<InsRegimesDTO>();
            add.AddRange(dtos);
            var response = await _genericRepository.CreateRange(_uow, add, sid);
            return response;
        }

        public async Task<FormatedResponse> Update(GenericUnitOfWork _uow, InsRegimesDTO dto, string sid, bool patchMode = true)
        {
            var c = await _dbContext.InsRegimess.AsNoTracking().Where(x => x.ID == dto.Id && x.CODE == dto.Code).FirstOrDefaultAsync();
            if (c != null)
            {
                var response = await _genericRepository.Update(_uow, dto, sid, patchMode);
                return response;
            }
            else
            {
                return new FormatedResponse() { MessageCode = CommonMessageCode.ENTITY_NOT_FOUND, ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400 };
            }
        }

        public async Task<FormatedResponse> UpdateRange(GenericUnitOfWork _uow, List<InsRegimesDTO> dtos, string sid, bool patchMode = true)
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
            var getDataActive = _dbContext.InsRegimess.AsNoTracking().Where(x => ids.Contains(x.ID) && x.IS_ACTIVE == true).Any();
            var getDataUsing = _dbContext.InsRegimesMngs.AsNoTracking().Where(x => ids.Contains((long)x.REGIME_ID)).Any();
            if (getDataActive)
            {
                return new FormatedResponse() { StatusCode = EnumStatusCode.StatusCode400, ErrorType = EnumErrorType.CATCHABLE, MessageCode = CommonMessageCode.CAN_NOT_DELETE_RECORDS_HAVE_IS_ACTIVE };
            }
            if (getDataUsing)
            {
                return new FormatedResponse() { StatusCode = EnumStatusCode.StatusCode400, ErrorType = EnumErrorType.CATCHABLE, MessageCode = CommonMessageCode.CAN_NOT_DELETE_RECORDS_HAVE_USE };
            }
            var response = await _genericRepository.DeleteIds(_uow, ids);
            return response;
        }

        public async Task<FormatedResponse> DeleteIds(GenericUnitOfWork _uow, List<string> ids)
        {
            await Task.Run(() => null);
            throw new NotImplementedException();
        }
        public async Task<FormatedResponse> GetInsGroup()
        {
            var query = await (from o in _dbContext.InsGroups
                               where o.IS_ACTIVE == true
                               orderby o.ID
                               select new
                               {
                                   Id = o.ID,
                                   Name = o.NAME,
                                   Code = o.CODE,
                               }).ToListAsync();
            return new FormatedResponse() { InnerBody = query };
        }
        public async Task<FormatedResponse> GetCalDateType()
        {
            var query = await (from o in _dbContext.SysOtherListTypes.AsNoTracking().Where(x=> x.CODE == "TYPE_DAY_CAL")
                               from ol in _dbContext.SysOtherLists.AsNoTracking().Where(ol=> ol.TYPE_ID == o.ID).DefaultIfEmpty()
                               where o.IS_ACTIVE == true && ol.IS_ACTIVE == true
                               select new
                               {
                                   Id = ol.ID,
                                   Name = ol.NAME,
                                   Code = ol.CODE,
                               }).ToListAsync();
            return new FormatedResponse() { InnerBody = query };
        }

        public async Task<FormatedResponse> GetCalDateTypeById(long id)
        {
            var query = await (from ol in _dbContext.SysOtherLists.AsNoTracking().Where(ol => ol.ID == id).DefaultIfEmpty()
                               where ol.IS_ACTIVE == true
                               select new
                               {
                                   Id = ol.ID,
                                   Name = ol.NAME,
                                   Code = ol.CODE,
                               }).FirstAsync();
            return new FormatedResponse() { InnerBody = query };
        }

        public async Task<FormatedResponse> CreateNewCode()
        {
            string newCode = "";
            if (await _dbContext.InsRegimess.CountAsync() == 0)
            {
                newCode = "CD001";
            }
            else
            {
                string lastestData = _dbContext.InsRegimess.OrderByDescending(t => t.CODE).First().CODE!.ToString();

                newCode = lastestData.Substring(0, 2) + (int.Parse(lastestData.Substring(lastestData.Length - 3)) + 1).ToString("D3");
            }

            return new FormatedResponse() { InnerBody = new { Code = newCode } };

        }

        public virtual async Task<FormatedResponse> ToggleActiveIds(GenericUnitOfWork _uow, List<long> ids, bool valueToBind, string sid)
        {
            await Task.Run(() => null);
            throw new NotImplementedException();
        }
    }
}

