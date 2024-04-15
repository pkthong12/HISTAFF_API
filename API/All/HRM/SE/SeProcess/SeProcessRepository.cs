using CORE.GenericUOW;
using CORE.DTO;
using API.DTO;
using CORE.Enum;
using CORE.StaticConstant;
using API.All.DbContexts;
using CORE.AutoMapper;

namespace API.Controllers.SeProcess
{
    public class SeProcessRepository : ISeProcessRepository
    {
        private readonly GenericUnitOfWork _uow;
        private readonly FullDbContext _dbContext;
        private IGenericRepository<SE_PROCESS, SeProcessDTO> _genericRepository;
        private readonly GenericReducer<SE_PROCESS, SeProcessDTO> _genericReducer;

        public SeProcessRepository(FullDbContext context, GenericUnitOfWork uow)
        {
            _dbContext = context;
            _uow = uow;
            _genericRepository = _uow.GenericRepository<SE_PROCESS, SeProcessDTO>();
            _genericReducer = new();
        }

        public async Task<GenericPhaseTwoListResponse<SeProcessDTO>> SinglePhaseQueryList(GenericQueryListDTO<SeProcessDTO> request)
        {
            var joined = from l in _dbContext.SeProcesss.AsNoTracking()
                         from t in _dbContext.SeHrProcessTypes.AsNoTracking().Where(t=> t.ID == l.PROCESS_TYPE_ID).DefaultIfEmpty()
                         // JOIN OTHER ENTITIES BASED ON THE BUSINESS
                         select new SeProcessDTO
                         {
                             Id = l.ID,
                             Code = l.CODE,
                             Name = l.NAME,
                             ApprovedContent = l.APPROVED_CONTENT,
                             NotApprovedContent = l.NOT_APPROVED_CONTENT,
                             ProcessTypeId = l.PROCESS_TYPE_ID,
                             ProcessTypeName = t.NAME,
                             ProDescription = l.PRO_DESCRIPTION,
                             ApprovedSucessContent = l.APPROVED_SUCESS_CONTENT,
                             Approve = l.APPROVE,
                             Refuse = l.REFUSE,
                             AdjustmentParam = l.ADJUSTMENT_PARAM,
                             IsNotiApprove = l.IS_NOTI_APPROVE,
                             IsNotiApproveSuccess = l.IS_NOTI_APPROVE_SUCCESS,
                             IsNotiNotApprove = l.IS_NOTI_NOT_APPROVE,
                             CreatedBy = l.CREATED_BY,
                             UpdatedBy = l.UPDATED_BY,
                             CreatedDate = l.CREATED_DATE,
                             UpdatedDate = l.UPDATED_DATE,
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
                var list = new List<SE_PROCESS>
                    {
                        (SE_PROCESS)response
                    };
                var joined = (from l in list
                              // JOIN OTHER ENTITIES BASED ON THE BUSINESS
                              select new SeProcessDTO
                              {
                                  Id = l.ID,
                                  Code = l.CODE,
                                  Name = l.NAME,
                                  ApprovedContent = l.APPROVED_CONTENT,
                                  NotApprovedContent = l.NOT_APPROVED_CONTENT,
                                  ProcessTypeId = l.PROCESS_TYPE_ID,
                                  ProDescription = l.PRO_DESCRIPTION,
                                  ApprovedSucessContent = l.APPROVED_SUCESS_CONTENT,
                                  Approve = l.APPROVE,
                                  Refuse = l.REFUSE,
                                  AdjustmentParam = l.ADJUSTMENT_PARAM,
                                  IsNotiApprove = l.IS_NOTI_APPROVE,
                                  IsNotiApproveSuccess = l.IS_NOTI_APPROVE_SUCCESS,
                                  IsNotiNotApprove = l.IS_NOTI_NOT_APPROVE,
                                  CreatedBy = l.CREATED_BY,
                                  UpdatedBy = l.UPDATED_BY,
                                  CreatedDate = l.CREATED_DATE,
                                  UpdatedDate = l.UPDATED_DATE,
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

        public async Task<FormatedResponse> Create(GenericUnitOfWork _uow, SeProcessDTO dto, string sid)
        {
            var response = await _genericRepository.Create(_uow, dto, sid);
            return response;
        }

        public async Task<FormatedResponse> CreateRange(GenericUnitOfWork _uow, List<SeProcessDTO> dtos, string sid)
        {
            var add = new List<SeProcessDTO>();
            add.AddRange(dtos);
            var response = await _genericRepository.CreateRange(_uow, add, sid);
            return response;
        }

        public async Task<FormatedResponse> Update(GenericUnitOfWork _uow, SeProcessDTO dto, string sid, bool patchMode = true)
        {
            var response = await _genericRepository.Update(_uow, dto, sid, patchMode);
            return response;
        }

        public async Task<FormatedResponse> UpdateRange(GenericUnitOfWork _uow, List<SeProcessDTO> dtos, string sid, bool patchMode = true)
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
            if (await _dbContext.SeProcesss.CountAsync() == 0)
            {
                newCode = "QT001";
            }
            else
            {
                string lastestData = _dbContext.SeProcesss.OrderByDescending(t => t.CODE).First().CODE!.ToString();

                newCode = lastestData.Substring(0, 2) + (int.Parse(lastestData.Substring(lastestData.Length - 3)) + 1).ToString("D3");
            }

            return new FormatedResponse() { InnerBody = new { Code = newCode } };

        }

        public async Task<FormatedResponse> GetProcessType()
        {
            var joined = await (from l in _dbContext.SeHrProcessTypes.AsNoTracking()
                                select new
                                {
                                    Id = l.ID,
                                    Code = l.CODE,
                                    Name = l.NAME,
                                }).ToListAsync();

            return new FormatedResponse() { InnerBody = joined };

        }
        public async Task<FormatedResponse> GetProcessTypeById(long id)
        {
            var joined = await (from l in _dbContext.SeHrProcessTypes.AsNoTracking().Where(l => l.ID == id).DefaultIfEmpty()
                                select new
                                {
                                    Id = l.ID,
                                    Code = l.CODE,
                                    Name = l.NAME,
                                }).SingleAsync();

            return new FormatedResponse() { InnerBody = joined };

        }

        public virtual async Task<FormatedResponse> ToggleActiveIds(GenericUnitOfWork _uow, List<long> ids, bool valueToBind, string sid)
        {
            await Task.Run(() => null);
            throw new NotImplementedException();
        }
    }
}

