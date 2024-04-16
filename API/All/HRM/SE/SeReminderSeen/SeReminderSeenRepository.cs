using CORE.GenericUOW;
using CORE.DTO;
using API.DTO;
using CORE.Enum;
using CORE.StaticConstant;
using API.All.DbContexts;
using CORE.AutoMapper;
using ProfileDAL.ViewModels;
using Common.Extensions;

namespace API.Controllers.SeReminderSeen
{
    public class SeReminderSeenRepository : ISeReminderSeenRepository
    {
        private readonly GenericUnitOfWork _uow;
        private readonly FullDbContext _dbContext;
        private IGenericRepository<SE_REMINDER_SEEN, SeReminderSeenDTO> _genericRepository;
        private readonly GenericReducer<SE_REMINDER_SEEN, SeReminderSeenDTO> _genericReducer;

        public SeReminderSeenRepository(FullDbContext context, GenericUnitOfWork uow)
        {
            _dbContext = context;
            _uow = uow;
            _genericRepository = _uow.GenericRepository<SE_REMINDER_SEEN, SeReminderSeenDTO>();
            _genericReducer = new();
        }

        public async Task<GenericPhaseTwoListResponse<SeReminderSeenDTO>> SinglePhaseQueryList(GenericQueryListDTO<SeReminderSeenDTO> request)
        {
            var joined = from p in _dbContext.SeReminderSeens.AsNoTracking()
                         // JOIN OTHER ENTITIES BASED ON THE BUSINESS
                         select new SeReminderSeenDTO
                         {
                             Id = p.ID
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
            var response = await _genericRepository.ReadAllByKey(key, value);
            return response;
        }

        public async Task<FormatedResponse> InsertReminderSeen(SeReminderSeenDTO request)
        {
            var reminder = _dbContext.SeReminderSeens.Where(x => x.CODE == request.Code && x.NAME == request.Name && x.REF_ID == request.RefId).FirstOrDefault();
            
            if(reminder == null)
            {
                var entity = new SE_REMINDER_SEEN();
                var response = CoreMapper<SeReminderSeenDTO, SE_REMINDER_SEEN>.DtoToEntity(request, entity);
                if (response != null)
                {
                    await _dbContext.SeReminderSeens.AddAsync(response);
                    await _dbContext.SaveChangesAsync();
                    return new FormatedResponse() { InnerBody = request };

                }
            }
            
            return new FormatedResponse() { InnerBody = null };
        }

        public async Task<FormatedResponse> ReadAllByKey(string key, string value)
        {
            var response = await _genericRepository.ReadAllByKey(key, value);
            return response;
        }

        public async Task<FormatedResponse> GetById(long id)
        {
            var res = await _genericRepository.GetById(id);
            if (res.InnerBody != null)
            {
                var response = res.InnerBody;
                var list = new List<SE_REMINDER_SEEN>
                    {
                        (SE_REMINDER_SEEN)response
                    };
                var joined = (from l in list
                              // JOIN OTHER ENTITIES BASED ON THE BUSINESS
                              select new SeReminderSeenDTO
                              {
                                  Id = l.ID
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

        public async Task<FormatedResponse> Create(GenericUnitOfWork _uow, SeReminderSeenDTO dto, string sid)
        {
            var response = await _genericRepository.Create(_uow, dto, sid);
            return response;
        }

        public async Task<FormatedResponse> CreateRange(GenericUnitOfWork _uow, List<SeReminderSeenDTO> dtos, string sid)
        {
            var add = new List<SeReminderSeenDTO>();
            add.AddRange(dtos);
            var response = await _genericRepository.CreateRange(_uow, add, sid);
            return response;
        }

        public async Task<FormatedResponse> Update(GenericUnitOfWork _uow, SeReminderSeenDTO dto, string sid, bool patchMode = true)
        {
            var response = await _genericRepository.Update(_uow, dto, sid, patchMode);
            return response;
        }

        public async Task<FormatedResponse> UpdateRange(GenericUnitOfWork _uow, List<SeReminderSeenDTO> dtos, string sid, bool patchMode = true)
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

