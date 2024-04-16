using CORE.GenericUOW;
using CORE.DTO;
using API.DTO;
using CORE.Enum;
using CORE.StaticConstant;
using API.All.DbContexts;

namespace API.Controllers.InsListProgram
{
    public class InsListProgramRepository : IInsListProgramRepository
    {
        private readonly GenericUnitOfWork _uow;
        private readonly FullDbContext _dbContext;
        private IGenericRepository<INS_LIST_PROGRAM, InsListProgramDTO> _genericRepository;
        private readonly GenericReducer<INS_LIST_PROGRAM, InsListProgramDTO> _genericReducer;

        public InsListProgramRepository(FullDbContext context, GenericUnitOfWork uow)
        {
            _dbContext = context;
            _uow = uow;
            _genericRepository = _uow.GenericRepository<INS_LIST_PROGRAM, InsListProgramDTO>();
            _genericReducer = new();
        }

        public async Task<GenericPhaseTwoListResponse<InsListProgramDTO>> SinglePhaseQueryList(GenericQueryListDTO<InsListProgramDTO> request)
        {
            var joined = from p in _dbContext.InsListPrograms.AsNoTracking()
                         from c in _dbContext.SysUsers.AsNoTracking().Where(c => p.CREATED_BY == null ? false : c.ID == p.CREATED_BY).DefaultIfEmpty()
                         from u in _dbContext.SysUsers.AsNoTracking().Where(u => p.UPDATED_BY == null ? false : u.ID == p.UPDATED_BY).DefaultIfEmpty()
                             // JOIN OTHER ENTITIES BASED ON THE BUSINESS
                         select new InsListProgramDTO
                         {
                             Id = p.ID,
                             Code = p.CODE,
                             Name = p.NAME,
                             Note = p.NOTE,
                             IsActive = p.IS_ACTIVE,
                             Status = p.IS_ACTIVE!.Value ? "Áp dụng" : "Ngừng áp dụng",
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
            var joined = (from l in _dbContext.InsListPrograms.AsNoTracking().Where(l => l.ID == id)
                              // JOIN OTHER ENTITIES BASED ON THE BUSINESS
                          select new InsListProgramDTO
                          {
                              Id = l.ID,
                              Code = l.CODE,
                              Name = l.NAME,
                              Note = l.NOTE,
                              IsActive = l.IS_ACTIVE,
                              CreatedBy = l.CREATED_BY,
                              CreatedDate = l.CREATED_DATE,
                              UpdatedBy = l.UPDATED_BY,
                              UpdatedDate = l.UPDATED_DATE,
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

        public async Task<FormatedResponse> Create(GenericUnitOfWork _uow, InsListProgramDTO dto, string sid)
        {
            try
            {
                var check = _dbContext.InsGroups.Where(x => x.CODE == dto.Code).Any();
                if (check) return new FormatedResponse() { MessageCode = "GROUP_CODE_EXISTED", ErrorType = EnumErrorType.UNCATCHABLE, StatusCode = EnumStatusCode.StatusCode500 };
                var response = await _genericRepository.Create(_uow, dto, sid);
                return response;
            }
            catch (Exception ex)
            {
                return new FormatedResponse()
                {
                    MessageCode = CommonMessageCode.POST_UPDATE_FAILLED,
                    StatusCode = EnumStatusCode.StatusCode500
                };
            }
        }

        public async Task<FormatedResponse> CreateRange(GenericUnitOfWork _uow, List<InsListProgramDTO> dtos, string sid)
        {
            var add = new List<InsListProgramDTO>();
            add.AddRange(dtos);
            var response = await _genericRepository.CreateRange(_uow, add, sid);
            return response;
        }

        public async Task<FormatedResponse> Update(GenericUnitOfWork _uow, InsListProgramDTO dto, string sid, bool patchMode = true)
        {
            var getDataActive = _uow.Context.Set<INS_LIST_PROGRAM>().Where(x => x.ID == dto.Id && x.IS_ACTIVE == true).Any();
            if (!getDataActive)
            {
                var response = await _genericRepository.Update(_uow, dto, sid, patchMode);
                return response;
            }
            else
            {
                return new FormatedResponse()
                {
                    MessageCode = "CAN_NOT_EDIT_HAVE_IS_ACTIVE",
                    StatusCode = EnumStatusCode.StatusCode400
                };
            }
            
        }

        public async Task<FormatedResponse> UpdateRange(GenericUnitOfWork _uow, List<InsListProgramDTO> dtos, string sid, bool patchMode = true)
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
        public async Task<FormatedResponse> CreateNewCode()
        {
            string newCode = "";
            if (await _dbContext.InsGroups.CountAsync() == 0)
            {
                newCode = "NCD001";
            }
            else
            {
                string lastestData = _dbContext.InsGroups.OrderByDescending(t => t.CODE).First().CODE!.ToString();

                newCode = lastestData.Substring(0, 3) + (int.Parse(lastestData.Substring(lastestData.Length - 3)) + 1).ToString("D3");
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


