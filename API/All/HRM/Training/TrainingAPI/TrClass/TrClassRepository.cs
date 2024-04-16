using CORE.GenericUOW;
using CORE.DTO;
using API.DTO;
using CORE.Enum;
using CORE.StaticConstant;
using API.All.DbContexts;
using CORE.AutoMapper;
using System.Formats.Asn1;

namespace API.Controllers.TrClass
{
    public class TrClassRepository : ITrClassRepository
    {
        private readonly GenericUnitOfWork _uow;
        private readonly FullDbContext _dbContext;
        private IGenericRepository<TR_CLASS, TrClassDTO> _genericRepository;
        private readonly GenericReducer<TR_CLASS, TrClassDTO> _genericReducer;

        public TrClassRepository(FullDbContext context, GenericUnitOfWork uow)
        {
            _dbContext = context;
            _uow = uow;
            _genericRepository = _uow.GenericRepository<TR_CLASS, TrClassDTO>();
            _genericReducer = new();
        }

        public async Task<GenericPhaseTwoListResponse<TrClassDTO>> SinglePhaseQueryList(GenericQueryListDTO<TrClassDTO> request)
        {
            var joined = from p in _dbContext.TrClasss.AsNoTracking()
                         from pro in _dbContext.HuProvinces.AsNoTracking().Where(pro => pro.ID == p.PROVINCE_ID).DefaultIfEmpty()
                         from d in _dbContext.HuDistricts.AsNoTracking().Where(d => d.ID == p.DISTRICT_ID).DefaultIfEmpty()
                         select new TrClassDTO
                         {
                             Id = p.ID,
                             Name = p.Name,
                             Ratio = p.RATIO,
                             TrProgramId = p.TR_PROGRAM_ID,
                             //
                             StartDate = p.START_DATE,
                             EndDate = p.END_DATE,
                             TotalDay = p.TOTAL_DAY,
                             TimeFrom = p.TIME_FROM,
                             TimeFromStr = p.TIME_FROM == null ? "" : p.TIME_FROM.Value.ToString("HH:mm"),
                             TimeTo = p.TIME_TO ,
                             TimeToStr = p.TIME_TO == null ? "" : p.TIME_TO.Value.ToString("HH:mm"),
                             TotalTime = p.TOTAL_TIME,
                             TotalTimeStr = (((int)Math.Ceiling(Convert.ToDecimal(p.TOTAL_TIME / 60))).ToString().Length == 1 ? "0" + (int)Math.Ceiling(Convert.ToDecimal(p.TOTAL_TIME / 60)) : (int)Math.Ceiling(Convert.ToDecimal(p.TOTAL_TIME / 60))) 
                                            + ":" + 
                                            (((int)(p.TOTAL_TIME! - (int)Math.Ceiling(Convert.ToDecimal(p.TOTAL_TIME / 60)) * 60)).ToString().Length == 1 ? "0" + (int)(p.TOTAL_TIME! - (int)Math.Ceiling(Convert.ToDecimal(p.TOTAL_TIME / 60)) * 60) : (int)(p.TOTAL_TIME! - (int)Math.Ceiling(Convert.ToDecimal(p.TOTAL_TIME / 60)) * 60)),
                             Address = p.ADDRESS,
                             ProvinceId = p.PROVINCE_ID,
                             ProvinceName = pro.NAME,
                             DistrictId = p.DISTRICT_ID,
                             DistrictName = d.NAME,
                             Note = p.NOTE,
                             EmailContent = p.EMAIL_CONTENT,
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
            var joined = await (from p in _dbContext.TrClasss.AsNoTracking().Where(p => p.ID == id)
                                from pro in _dbContext.HuProvinces.AsNoTracking().Where(pro => pro.ID == p.PROVINCE_ID).DefaultIfEmpty()
                                from d in _dbContext.HuDistricts.AsNoTracking().Where(d => d.ID == p.DISTRICT_ID).DefaultIfEmpty()
                                select new TrClassDTO
                                {
                                    Id = p.ID,
                                    Name = p.Name,
                                    Ratio = p.RATIO,
                                    TrProgramId = p.TR_PROGRAM_ID,
                                    StartDate = p.START_DATE,
                                    EndDate = p.END_DATE,
                                    TotalDay = p.TOTAL_DAY,
                                    TimeFromStr = p.TIME_FROM == null ? "" : p.TIME_FROM.Value.ToString("HH:mm"),
                                    TimeTo = p.TIME_TO,
                                    TimeToStr = p.TIME_TO == null ? "" : p.TIME_TO.Value.ToString("HH:mm"),
                                    TotalTime = p.TOTAL_TIME,
                                    Address = p.ADDRESS,
                                    ProvinceId = p.PROVINCE_ID,
                                    ProvinceName = pro.NAME,
                                    DistrictId = p.DISTRICT_ID,
                                    DistrictName = d.NAME,
                                    Note = p.NOTE,
                                    EmailContent = p.EMAIL_CONTENT,
                                }).FirstOrDefaultAsync();
            if(joined != null)
            {
                if(joined.TotalTime != null)
                {
                    int hours = (int)Math.Ceiling(Convert.ToDecimal(joined.TotalTime / 60));
                    int minutes = (int)(joined.TotalTime - hours * 60);
                    joined.TotalTimeStr = (hours.ToString().Length == 1 ? "0" + hours : hours) + ":" + (minutes.ToString().Length == 1 ? "0" + minutes : minutes);
                }
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

        public async Task<FormatedResponse> Create(GenericUnitOfWork _uow, TrClassDTO dto, string sid)
        {
            TimeSpan diff = dto.TimeTo!.Value.Subtract(dto.TimeFrom!.Value);
            double totalMinutes = diff.TotalMinutes;
            dto.TotalTime = (long)totalMinutes;
            var response = await _genericRepository.Create(_uow, dto, sid);
            return response;
        }

        public async Task<FormatedResponse> CreateRange(GenericUnitOfWork _uow, List<TrClassDTO> dtos, string sid)
        {
            var add = new List<TrClassDTO>();
            add.AddRange(dtos);
            var response = await _genericRepository.CreateRange(_uow, add, sid);
            return response;
        }

        public async Task<FormatedResponse> Update(GenericUnitOfWork _uow, TrClassDTO dto, string sid, bool patchMode = true)
        {
            var response = await _genericRepository.Update(_uow, dto, sid, patchMode);
            return response;
        }

        public async Task<FormatedResponse> UpdateRange(GenericUnitOfWork _uow, List<TrClassDTO> dtos, string sid, bool patchMode = true)
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
    }
}

