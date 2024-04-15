using CORE.GenericUOW;
using CORE.DTO;
using API.DTO;
using CORE.Enum;
using CORE.StaticConstant;
using API.All.DbContexts;
using CORE.AutoMapper;

namespace API.Controllers.AtOtherList
{
    public class AtOtherListRepository : IAtOtherListRepository
    {
        private readonly GenericUnitOfWork _uow;
        private readonly FullDbContext _dbContext;
        private IGenericRepository<AT_OTHER_LIST, AtOtherListDTO> _genericRepository;
        private readonly GenericReducer<AT_OTHER_LIST, AtOtherListDTO> _genericReducer;

        public AtOtherListRepository(FullDbContext context, GenericUnitOfWork uow)
        {
            _dbContext = context;
            _uow = uow;
            _genericRepository = _uow.GenericRepository<AT_OTHER_LIST, AtOtherListDTO>();
            _genericReducer = new();
        }

        public async Task<GenericPhaseTwoListResponse<AtOtherListDTO>> SinglePhaseQueryList(GenericQueryListDTO<AtOtherListDTO> request)
        {
            var joined = from p in _dbContext.AtOtherLists.AsNoTracking()
                             // JOIN OTHER ENTITIES BASED ON THE BUSINESS
                         select new AtOtherListDTO
                         {
                             Id = p.ID,
                             EffectDate = p.EFFECT_DATE,
                             EffectiveDateString = p.EFFECT_DATE!.Value.ToString("dd/MM/yyyy"),
                             ExpirationDate = p.EXPIRATION_DATE,
                             IsEntireYear = p.IS_ENTIRE_YEAR,
                             MaxWorkingMonth = p.MAX_WORKING_MONTH,
                             MaxWorkingYear = p.MAX_WORKING_YEAR,
                             OvertimeDayWeekday = p.OVERTIME_DAY_WEEKDAY,
                             OvertimeDayHoliday = p.OVERTIME_NIGHT_HOLIDAY,
                             OvertimeDayOff = p.OVERTIME_DAY_OFF,
                             OvertimeNightHoliday = p.OVERTIME_NIGHT_HOLIDAY,
                             OvertimeNightWeekday = p.OVERTIME_NIGHT_WEEKDAY,
                             OvertimeNightOff = p.OVERTIME_NIGHT_OFF,
                             PersonalDeductionAmount = p.PERSONAL_DEDUCTION_AMOUNT,
                             SelfDeductionAmount = p.SELF_DEDUCTION_AMOUNT,
                             BaseSalary = p.BASE_SALARY,
                             StatusName = (p.IS_ACTIVE == true ? "Áp dụng" : "Không áp dụng"),
                             Note = p.NOTE,
                             WorkdayUnitPrice = p.WORKDAY_UNIT_PRICE
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
                var list = new List<AT_OTHER_LIST>
                    {
                        (AT_OTHER_LIST)response
                    };
                var joined = (from l in list
                                  // JOIN OTHER ENTITIES BASED ON THE BUSINESS
                              select new AtOtherListDTO
                              {
                                  Id = l.ID,
                                  EffectDate = l.EFFECT_DATE,
                                  ExpirationDate = l.EXPIRATION_DATE,
                                  IsEntireYear = l.IS_ENTIRE_YEAR,
                                  MaxWorkingMonth = l.MAX_WORKING_MONTH,
                                  MaxWorkingYear = l.MAX_WORKING_YEAR,
                                  OvertimeDayWeekday = l.OVERTIME_DAY_WEEKDAY,
                                  OvertimeDayHoliday = l.OVERTIME_NIGHT_HOLIDAY,
                                  OvertimeDayOff = l.OVERTIME_DAY_OFF,
                                  OvertimeNightHoliday = l.OVERTIME_NIGHT_HOLIDAY,
                                  OvertimeNightWeekday = l.OVERTIME_NIGHT_WEEKDAY,
                                  OvertimeNightOff = l.OVERTIME_NIGHT_OFF,
                                  PersonalDeductionAmount = l.PERSONAL_DEDUCTION_AMOUNT,
                                  SelfDeductionAmount = l.SELF_DEDUCTION_AMOUNT,
                                  BaseSalary = l.BASE_SALARY,
                                  Note = l.NOTE,
                                  WorkdayUnitPrice = l.WORKDAY_UNIT_PRICE

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

        public async Task<FormatedResponse> Create(GenericUnitOfWork _uow, AtOtherListDTO dto, string sid)
        {
            dto.IsActive = true;
            var response = await _genericRepository.Create(_uow, dto, sid);
            return response;
        }

        public async Task<FormatedResponse> CreateRange(GenericUnitOfWork _uow, List<AtOtherListDTO> dtos, string sid)
        {
            var add = new List<AtOtherListDTO>();
            add.AddRange(dtos);
            var response = await _genericRepository.CreateRange(_uow, add, sid);
            return response;
        }

        public async Task<FormatedResponse> Update(GenericUnitOfWork _uow, AtOtherListDTO dto, string sid, bool patchMode = true)
        {
            var response = await _genericRepository.Update(_uow, dto, sid, patchMode);
            return response;
        }

        public async Task<FormatedResponse> UpdateRange(GenericUnitOfWork _uow, List<AtOtherListDTO> dtos, string sid, bool patchMode = true)
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

