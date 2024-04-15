using CORE.GenericUOW;
using CORE.DTO;
using API.DTO;
using CORE.Enum;
using CORE.StaticConstant;
using API.All.DbContexts;
using CORE.AutoMapper;
using System;

namespace API.Controllers.AtPeriodStandard
{
    public class AtPeriodStandardRepository : IAtPeriodStandardRepository
    {
        private readonly GenericUnitOfWork _uow;
        private readonly FullDbContext _dbContext;
        private IGenericRepository<AT_PERIOD_STANDARD, AtPeriodStandardDTO> _genericRepository;
        private readonly GenericReducer<AT_PERIOD_STANDARD, AtPeriodStandardDTO> _genericReducer;

        public AtPeriodStandardRepository(FullDbContext context, GenericUnitOfWork uow)
        {
            _dbContext = context;
            _uow = uow;
            _genericRepository = _uow.GenericRepository<AT_PERIOD_STANDARD, AtPeriodStandardDTO>();
            _genericReducer = new();
        }

        public async Task<GenericPhaseTwoListResponse<AtPeriodStandardDTO>> SinglePhaseQueryList(GenericQueryListDTO<AtPeriodStandardDTO> request)
        {
            var joined = from p in _dbContext.AtPeriodStandards.AsNoTracking()
                             // JOIN OTHER ENTITIES BASED ON THE BUSINESS
                         from sp in _dbContext.AtSalaryPeriods.AsNoTracking().Where(s => s.ID == p.PERIOD_ID).DefaultIfEmpty()
                         from ol in _dbContext.SysOtherLists.AsNoTracking().Where(o => o.TYPE_ID == 42 && o.ID == p.OBJECT_ID).DefaultIfEmpty()
                         select new AtPeriodStandardDTO
                         {
                             Id = p.ID,
                             Year = p.YEAR,
                             PeriodName = sp.NAME,
                             ObjectName = ol.NAME,
                             PeriodStandard = p.PERIOD_STANDARD,
                             Note = p.NOTE,
                             IsActive = p.IS_ACTIVE,
                             Status = p.IS_ACTIVE == true ? "Áp dụng" : "Ngừng áp dụng",
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

            var joined = (from l in _dbContext.AtPeriodStandards.Where(l => l.ID == id)
                          from c in _dbContext.SysUsers.AsNoTracking().Where(c => l.CREATED_BY == null ? false : c.ID == l.CREATED_BY).DefaultIfEmpty()
                            from u in _dbContext.SysUsers.AsNoTracking().Where(u => l.UPDATED_BY == null ? false : u.ID == l.UPDATED_BY).DefaultIfEmpty()
                            select new AtPeriodStandardDTO
                            {
                                Id = l.ID,
                                Year = l.YEAR,
                                PeriodId = l.PERIOD_ID,
                                ObjectId = l.OBJECT_ID,
                                PeriodStandard = l.PERIOD_STANDARD,
                                IsActive = l.IS_ACTIVE,
                                Note = l.NOTE,
                                CreatedDate = l.CREATED_DATE,
                                UpdatedDate = l.UPDATED_DATE,
                                CreatedByUsername = c.USERNAME,
                                UpdatedByUsername = u.USERNAME
                            }).FirstOrDefault();

            if(joined != null)
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

        public async Task<FormatedResponse> Create(GenericUnitOfWork _uow, AtPeriodStandardDTO dto, string sid)
        {
            var response = await _genericRepository.Create(_uow, dto, sid);
            return response;
        }

        public async Task<FormatedResponse> CreateRange(GenericUnitOfWork _uow, List<AtPeriodStandardDTO> dtos, string sid)
        {
            var add = new List<AtPeriodStandardDTO>();
            add.AddRange(dtos);
            var response = await _genericRepository.CreateRange(_uow, add, sid);
            return response;
        }

        public async Task<FormatedResponse> Update(GenericUnitOfWork _uow, AtPeriodStandardDTO dto, string sid, bool patchMode = true)
        {
            var response = await _genericRepository.Update(_uow, dto, sid, patchMode);
            return response;
        }

        public async Task<FormatedResponse> UpdateRange(GenericUnitOfWork _uow, List<AtPeriodStandardDTO> dtos, string sid, bool patchMode = true)
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
            var checkActive = await _dbContext.AtPeriodStandards.AsNoTracking().Where(p => ids.Contains(p.ID)).CountAsync() > 0 ? true : false;

            if (checkActive)
            {
                return new FormatedResponse()
                {
                    MessageCode = CommonMessageCode.DO_NOT_DELETE_DATA_IS_ACTIVE,
                    
                };
            }
            var response = await _genericRepository.DeleteIds(_uow, ids);
            return response;
        }

        public async Task<FormatedResponse> DeleteIds(GenericUnitOfWork _uow, List<string> ids)
        {
            await Task.Run(() => null);
            throw new NotImplementedException();
        }


        public async Task<FormatedResponse> GetPeriod()
        {
            var query = await (from p in _dbContext.AtSalaryPeriods
                               where p.IS_ACTIVE == true
                               orderby p.START_DATE descending
                               select new
                               {
                                   Id = p.ID,
                                   Name = p.NAME,
                               }).ToListAsync();
            return new FormatedResponse() { InnerBody = query };
        }

        public async Task<FormatedResponse> GetObject()
        {
            var query = await (from o in _dbContext.SysOtherLists
                               where o.IS_ACTIVE == true && o.TYPE_ID == 42
                               orderby o.CODE
                               select new
                               {
                                   Id = o.ID,
                                   Name = o.NAME,
                                   Code = o.CODE,
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

