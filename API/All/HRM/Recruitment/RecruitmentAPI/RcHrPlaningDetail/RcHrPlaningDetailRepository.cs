using CORE.GenericUOW;
using CORE.DTO;
using API.DTO;
using CORE.Enum;
using CORE.StaticConstant;
using API.All.DbContexts;
using CORE.AutoMapper;
using Microsoft.AspNetCore.Mvc;
using DocumentFormat.OpenXml.Office2010.Excel;

namespace API.Controllers.RcHrPlaningDetail
{
    public class RcHrPlaningDetailRepository : IRcHrPlaningDetailRepository
    {
        private readonly GenericUnitOfWork _uow;
        private readonly FullDbContext _dbContext;
        private IGenericRepository<RC_HR_PLANING_DETAIL, RcHrPlaningDetailDTO> _genericRepository;
        private readonly GenericReducer<RC_HR_PLANING_DETAIL, RcHrPlaningDetailDTO> _genericReducer;

        public RcHrPlaningDetailRepository(FullDbContext context, GenericUnitOfWork uow)
        {
            _dbContext = context;
            _uow = uow;
            _genericRepository = _uow.GenericRepository<RC_HR_PLANING_DETAIL, RcHrPlaningDetailDTO>();
            _genericReducer = new();
        }

        public async Task<GenericPhaseTwoListResponse<RcHrPlaningDetailDTO>> SinglePhaseQueryList(GenericQueryListDTO<RcHrPlaningDetailDTO> request)
        {
            var joined = from p in _dbContext.RcHrPlaningDetails
                         from o in _dbContext.HuOrganizations.AsNoTracking().Where(o => o.ID == p.ORG_ID).DefaultIfEmpty()
                         from po in _dbContext.HuPositions.AsNoTracking().Where(po => po.ID == p.POSITION_ID).DefaultIfEmpty()

                             // JOIN OTHER ENTITIES BASED ON THE BUSINESS
                         select new RcHrPlaningDetailDTO
                         {
                             Id = p.ID,
                             PositionId = p.POSITION_ID,
                             PositionName = po.NAME,
                             OrgId = p.ORG_ID,
                             OrgName = o.NAME,
                             YearPlanId = p.YEAR_PLAN_ID,
                             Month1 = p.MONTH_1,
                             Month2 = p.MONTH_2,
                             Month3 = p.MONTH_3,
                             Month4 = p.MONTH_4,
                             Month5 = p.MONTH_5,
                             Month6 = p.MONTH_6,
                             Month7 = p.MONTH_7,
                             Month8 = p.MONTH_8,
                             Month9 = p.MONTH_9,
                             Month10 = p.MONTH_10,
                             Month11 = p.MONTH_11,
                             Month12 = p.MONTH_12,
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
            var res = await _genericRepository.GetById(id);
            if (res.InnerBody != null)
            {
                var response = res.InnerBody;
                var list = new List<RC_HR_PLANING_DETAIL>
                    {
                        (RC_HR_PLANING_DETAIL)response
                    };
                var joined = (from l in list
                              from o in _dbContext.HuOrganizations.AsNoTracking().Where(p=> p.ID == l.ORG_ID).DefaultIfEmpty()
                              // JOIN OTHER ENTITIES BASED ON THE BUSINESS
                              select new RcHrPlaningDetailDTO
                              {
                                  Id = l.ID,
                                  YearPlanId = l.YEAR_PLAN_ID,
                                  OrgId = l.ORG_ID,
                                  //OrgName = o.NAME, please don't open comment
                                  Month1 = l.MONTH_1,
                                  Month2 = l.MONTH_2,
                                  Month3 = l.MONTH_3,
                                  Month4 = l.MONTH_4,
                                  Month5 = l.MONTH_5,
                                  Month6 = l.MONTH_6,
                                  Month7 = l.MONTH_7,
                                  Month8 = l.MONTH_8,
                                  Month9 = l.MONTH_9,
                                  Month10 = l.MONTH_10,
                                  Month11 = l.MONTH_11,
                                  Month12 = l.MONTH_12,
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

        public async Task<FormatedResponse> Create(GenericUnitOfWork _uow, RcHrPlaningDetailDTO dto, string sid)
        {
            var response = await _genericRepository.Create(_uow, dto, sid);
            return response;
        }

        public async Task<FormatedResponse> CreateRange(GenericUnitOfWork _uow, List<RcHrPlaningDetailDTO> dtos, string sid)
        {
            var add = new List<RcHrPlaningDetailDTO>();
            add.AddRange(dtos);
            var response = await _genericRepository.CreateRange(_uow, add, sid);
            return response;
        }

        public async Task<FormatedResponse> Update(GenericUnitOfWork _uow, RcHrPlaningDetailDTO dto, string sid, bool patchMode = true)
        {
            var response = await _genericRepository.Update(_uow, dto, sid, patchMode);
            return response;
        }

        public async Task<FormatedResponse> UpdateRange(GenericUnitOfWork _uow, List<RcHrPlaningDetailDTO> dtos, string sid, bool patchMode = true)
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

        public Task<FormatedResponse> ToggleActiveIds(GenericUnitOfWork _uow, List<long> ids, bool valueToBind, string sid)
        {
            throw new NotImplementedException();
        }

        public async Task<FormatedResponse> GetAllPositionByOrgs(RcHrPlaningDetailServiceDTO model)
        {
            var response = await (from p in _dbContext.HuPositions.AsNoTracking().Where(p => model.Ids!.Contains(p.ORG_ID!.Value)).DefaultIfEmpty()
                                  select new
                                  {
                                      Id = p.ID,
                                      Name = p.NAME,
                                  }).ToListAsync();
            var orgName = await (_dbContext.HuOrganizations.AsNoTracking().FirstAsync(p => model.Id == p.ID));
            
            return new FormatedResponse() { InnerBody = new { 
                ListPos = response,
                OrgName = orgName.NAME
            } };
        }
    }
}

