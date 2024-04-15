using CORE.GenericUOW;
using CORE.DTO;
using API.DTO;
using CORE.Enum;
using CORE.StaticConstant;
using API.All.DbContexts;
using CORE.AutoMapper;
using System;

namespace API.Controllers.AtTerminal
{
    public class AtTerminalRepository : IAtTerminalRepository
    {
        private readonly GenericUnitOfWork _uow;
        private readonly FullDbContext _dbContext;
        private IGenericRepository<AT_TERMINAL, AtTerminalDTO> _genericRepository;
        private readonly GenericReducer<AT_TERMINAL, AtTerminalDTO> _genericReducer;

        public AtTerminalRepository(FullDbContext context, GenericUnitOfWork uow)
        {
            _dbContext = context;
            _uow = uow;
            _genericRepository = _uow.GenericRepository<AT_TERMINAL, AtTerminalDTO>();
            _genericReducer = new();
        }

        public async Task<GenericPhaseTwoListResponse<AtTerminalDTO>> SinglePhaseQueryList(GenericQueryListDTO<AtTerminalDTO> request)
        {
            var joined = from p in _dbContext.AtTerminals.AsNoTracking()
                         // JOIN OTHER ENTITIES BASED ON THE BUSINESS
                         select new AtTerminalDTO
                         {
                             Id = p.ID,
                             TerminalCode = p.TERMINAL_CODE,
                             TerminalName = p.TERMINAL_NAME,
                             AddressPlace = p.ADDRESS_PLACE,
                             TerminalPort = p.TERMINAL_PORT,
                             TerminalIp = p.TERMINAL_IP,
                             Pass = p.PASS,
                             LastTimeStatus = p.LAST_TIME_STATUS,
                             TerminalStatus = p.TERMINAL_STATUS,
                             TerminalRow = p.TERMINAL_ROW,
                             LastTimeUpdate = p.LAST_TIME_UPDATE,
                             Note = p.NOTE,
                             Status = p.IS_ACTIVE == true ? "Áp dụng" : "Ngừng áp dụng"

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
            var res = await _genericRepository.GetById(id);
            if (res.InnerBody != null)
            {
                var response = res.InnerBody;
                var list = new List<AT_TERMINAL>
                    {
                        (AT_TERMINAL)response
                    };
                var joined = (from l in list
                              // JOIN OTHER ENTITIES BASED ON THE BUSINESS
                              select new AtTerminalDTO
                              {
                                  Id = l.ID,
                                  TerminalCode = l.TERMINAL_CODE,
                                  TerminalName = l.TERMINAL_NAME,
                                  AddressPlace = l.ADDRESS_PLACE,
                                  TerminalPort = l.TERMINAL_PORT,
                                  TerminalIp = l.TERMINAL_IP,
                                  Pass = l.PASS,
                                  Status = l.IS_ACTIVE.HasValue ? "Áp dụng" : "Ngừng áp dụng",
                                  Note = l.NOTE,
                                  CreatedDate = l.CREATED_DATE,
                                  CreatedBy = l.CREATED_BY,
                                  UpdatedDate = l.UPDATED_DATE,
                                  UpdatedBy = l.UPDATED_BY

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

        public async Task<FormatedResponse> Create(GenericUnitOfWork _uow, AtTerminalDTO dto, string sid)
        {
            var response = await _genericRepository.Create(_uow, dto, sid);
            return response;
        }

        public async Task<FormatedResponse> CreateRange(GenericUnitOfWork _uow, List<AtTerminalDTO> dtos, string sid)
        {
            var add = new List<AtTerminalDTO>();
            add.AddRange(dtos);
            var response = await _genericRepository.CreateRange(_uow, add, sid);
            return response;
        }

        public async Task<FormatedResponse> Update(GenericUnitOfWork _uow, AtTerminalDTO dto, string sid, bool patchMode = true)
        {
            var response = await _genericRepository.Update(_uow, dto, sid, patchMode);
            return response;
        }

        public async Task<FormatedResponse> UpdateRange(GenericUnitOfWork _uow, List<AtTerminalDTO> dtos, string sid, bool patchMode = true)
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
                var item = await _dbContext.AtTerminals.Where(x => x.ID == id).AsNoTracking().SingleOrDefaultAsync();
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

        public async Task<FormatedResponse> CreateNewCode()
        {
            string newCode = "";
            if (await _dbContext.AtTerminals.CountAsync() == 0)
            {
                newCode = "MCC001";
            }
            else
            {
                string lastestData = _dbContext.AtTerminals.OrderByDescending(t => t.TERMINAL_CODE).First().TERMINAL_CODE!.ToString();

                string newNumber = (Int32.Parse(lastestData.Substring(3)) + 1).ToString();
                while (newNumber.Length < 3)
                {
                    newNumber = "0" + newNumber;
                }
                newCode = lastestData.Substring(0, 3) + newNumber;
            }

            return new FormatedResponse() { InnerBody = new { Code = newCode } };

        }

        public virtual async Task<FormatedResponse> ToggleActiveIds(GenericUnitOfWork _uow, List<long> ids, bool valueToBind, string sid)
        {
            var response = await _genericRepository.ToggleActiveIds(_uow, ids, valueToBind, sid);
            return response;
        }
    }
}

