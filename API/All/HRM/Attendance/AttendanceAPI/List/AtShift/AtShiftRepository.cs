using CORE.GenericUOW;
using CORE.DTO;
using API.DTO;
using CORE.Enum;
using CORE.StaticConstant;
using API.All.DbContexts;
using CORE.AutoMapper;
using System;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;
using Microsoft.IdentityModel.Tokens;

namespace API.Controllers.AtShift
{
    public class AtShiftRepository : IAtShiftRepository
    {
        private readonly GenericUnitOfWork _uow;
        private readonly FullDbContext _dbContext;
        private IGenericRepository<AT_SHIFT, AtShiftDTO> _genericRepository;
        private readonly GenericReducer<AT_SHIFT, AtShiftDTO> _genericReducer;


        public AtShiftRepository(FullDbContext context, GenericUnitOfWork uow)
        {
            _dbContext = context;
            _uow = uow;
            _genericRepository = _uow.GenericRepository<AT_SHIFT, AtShiftDTO>();
            _genericReducer = new();
        }

        public async Task<FormatedResponse> SinglePhaseQueryList(GenericQueryListDTO<AtShiftDTO> request)
        {
            var joined = from p in _dbContext.AtShifts.AsNoTracking().DefaultIfEmpty()
                         from sym in _dbContext.AtTimeTypes.Where(x => x.ID == p.TIME_TYPE_ID).DefaultIfEmpty()
                         select new AtShiftDTO
                         {
                             Id = p.ID,
                             Code = p.CODE,
                             Name = p.NAME,
                             BreaksFrom = p.BREAKS_FROM,
                             BreaksTo = p.BREAKS_TO,
                             BreaksFromStr = p.BREAKS_FROM == null ? "" : p.BREAKS_FROM.Value.ToString("HH:mm"),
                             BreaksToStr = p.BREAKS_TO == null ? "" : p.BREAKS_TO.Value.ToString("HH:mm"),
                             HoursStart = p.HOURS_START,
                             HoursStop = p.HOURS_STOP,
                             HoursStartStr = p.HOURS_START == null ? "" : p.HOURS_START.ToString("HH:mm"),
                             HoursStopStr = p.HOURS_STOP == null ? "" : p.HOURS_STOP.ToString("HH:mm"),
                             MinHoursWork = p.MIN_HOURS_WORK,
                             TimeLate = p.TIME_LATE,
                             TimeEarly = p.TIME_EARLY,
                             IsBreak = p.IS_BREAK,
                             IsBoquacc= p.IS_BOQUACC,
                             IsNight = p.IS_NIGHT,
                             IsSunday = p.IS_SUNDAY,
                             IsActive = p.IS_ACTIVE,
                             TimeStart = p.TIME_START,
                             TimeStop = p.TIME_STOP,
                             TimeTypeId = p.TIME_TYPE_ID,
                             TimeTypeName = sym.NAME,
                             Saturday = p.SATURDAY,
                             Sunday = p.SUNDAY,
                             Note = p.NOTE,
                             IsActiveStr = p.IS_ACTIVE == true ? "Áp dụng" : "Ngừng áp dụng",
                         };
            var searchForHoursStart = "";
            var searchForHoursStop = "";
            var searchForBreaksFrom = "";
            var searchForBreaksTo = "";
            var skip = request.Pagination.Skip;
            var take = request.Pagination.Take;
            request.Pagination.Skip = 0;
            request.Pagination.Take = 9999;
            if (request.Search != null)
            {
                request.Search.ForEach(x =>
                {
                    if (x.Field == "hoursStart")
                    {
                        searchForHoursStart = x.SearchFor.ToString().ToLower().Trim();
                        x.SearchFor = "";
                    }
                    if (x.Field == "hoursStop")
                    {
                        searchForHoursStop = x.SearchFor.ToString().ToLower().Trim();
                        x.SearchFor = "";
                    }
                    if (x.Field == "breaksFrom")
                    {
                        searchForBreaksFrom = x.SearchFor.ToString().ToLower().Trim();
                        x.SearchFor = "";
                    }
                    if (x.Field == "breaksTo")
                    {
                        searchForBreaksTo = x.SearchFor.ToString().ToLower().Trim();
                        x.SearchFor = "";
                    }
                });
            }
            var singlePhaseResult = await _genericReducer.SinglePhaseReduce(joined, request);

            var resultList = new List<AtShiftDTO>();
            foreach(var i in singlePhaseResult!.List)
            {
                resultList.Add(i);
            }

            resultList = resultList.Where(x => (x.HoursStartStr!.Trim().Contains(searchForHoursStart))).ToList();
            resultList = resultList.Where(x => (x.HoursStopStr!.Trim().Contains(searchForHoursStop) )).ToList();
            resultList = resultList.Where(x => (x.BreaksFromStr!.Trim().Contains(searchForBreaksFrom))).ToList();
            resultList = resultList.Where(x => (x.BreaksToStr!.Trim().Contains(searchForBreaksTo))).ToList();

            var result = new
            {
                Count = resultList.Count(),
                List = resultList.Skip(skip).Take(take),
                Page = (skip / take) + 1,
                PageCount = resultList.Count(),
                Skip = skip,
                Take = take,
                MessageCode = CommonMessageCode.QUERY_LIST_SUCCESS,
            };
            return new()
            {
                InnerBody = result,
            };
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

            var joined = (from p in _dbContext.AtShifts.Where(l => l.ID == id)
                          from sym in _dbContext.AtTimeTypes.Where(x => x.ID == p.TIME_TYPE_ID).DefaultIfEmpty()
                          from c in _dbContext.SysUsers.AsNoTracking().Where(c => p.CREATED_BY == null ? false : c.ID == p.CREATED_BY).DefaultIfEmpty()
                          from u in _dbContext.SysUsers.AsNoTracking().Where(u => p.UPDATED_BY == null ? false : u.ID == p.UPDATED_BY).DefaultIfEmpty()
                          select new AtShiftDTO
                          {
                              Id = p.ID,
                              Code = p.CODE,
                              Name = p.NAME,
                              BreaksFrom = p.BREAKS_FROM,
                              BreaksTo = p.BREAKS_TO,
                              BreaksFromStr = p.BREAKS_FROM.Value.ToString("HH:mm"),
                              BreaksToStr = p.BREAKS_TO.Value.ToString("HH:mm"),
                              HoursStart = p.HOURS_START,
                              HoursStop = p.HOURS_STOP,
                              HoursStartStr = p.HOURS_START.ToString("HH:mm"),
                              HoursStopStr = p.HOURS_STOP.ToString("HH:mm"),
                              MinHoursWork = p.MIN_HOURS_WORK,
                              TimeLate = p.TIME_LATE,
                              TimeEarly = p.TIME_EARLY,
                              IsBreak = p.IS_BREAK,
                              IsBoquacc  = p.IS_BOQUACC,
                              IsNight = p.IS_NIGHT,
                              IsSunday = p.IS_SUNDAY,
                              IsActive = p.IS_ACTIVE,
                              IsActiveStr = p.IS_ACTIVE ? "Áp dụng" : "Ngừng áp dụng",
                              TimeStart = p.TIME_START,
                              TimeStop = p.TIME_STOP,
                              TimeTypeId = p.TIME_TYPE_ID,
                              TimeTypeName = sym.NAME,
                              Saturday = p.SATURDAY,
                              Sunday = p.SUNDAY,
                              Note = p.NOTE,
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

        public async Task<FormatedResponse> Create(GenericUnitOfWork _uow, AtShiftDTO dto, string sid)
        {
            dto.IsActive = true;
            var response = await _genericRepository.Create(_uow, dto, sid);
            return response;
        }

        public async Task<FormatedResponse> CreateRange(GenericUnitOfWork _uow, List<AtShiftDTO> dtos, string sid)
        {
            var add = new List<AtShiftDTO>();
            add.AddRange(dtos);
            var response = await _genericRepository.CreateRange(_uow, add, sid);
            return response;
        }

        public async Task<FormatedResponse> Update(GenericUnitOfWork _uow, AtShiftDTO dto, string sid, bool patchMode = true)
        {
            var response = await _genericRepository.Update(_uow, dto, sid, patchMode);
            return new FormatedResponse()
            {
                MessageCode = response.MessageCode,
                InnerBody = dto,
                StatusCode = response.StatusCode,
            };
        }

        public async Task<FormatedResponse> UpdateRange(GenericUnitOfWork _uow, List<AtShiftDTO> dtos, string sid, bool patchMode = true)
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
    }
}

