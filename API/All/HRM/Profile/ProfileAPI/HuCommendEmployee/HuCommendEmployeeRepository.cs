using CORE.GenericUOW;
using CORE.DTO;
using API.DTO;
using CORE.Enum;
using CORE.StaticConstant;
using API.All.DbContexts;
using CORE.AutoMapper;
using API.Controllers.HuCommend;

namespace API.Controllers.HuCommendEmployee
{
    public class HuCommendEmployeeRepository : IHuCommendEmployeeRepository
    {
        private readonly GenericUnitOfWork _uow;
        private readonly FullDbContext _dbContext;
        private IGenericRepository<HU_COMMEND_EMPLOYEE, HuCommendEmployeeDTO> _genericRepository;
        private readonly GenericReducer<HU_COMMEND_EMPLOYEE, HuCommendEmployeeDTO> _genericReducer;
        private readonly IHuCommendRepository _huCommendRepository;

        public HuCommendEmployeeRepository(FullDbContext context, GenericUnitOfWork uow)
        {
            _dbContext = context;
            _uow = uow;
            _genericRepository = _uow.GenericRepository<HU_COMMEND_EMPLOYEE, HuCommendEmployeeDTO>();
            _huCommendRepository = new HuCommendRepository(context, _uow);
            _genericReducer = new();
        }

        public async Task<GenericPhaseTwoListResponse<HuCommendEmployeeDTO>> SinglePhaseQueryList(GenericQueryListDTO<HuCommendEmployeeDTO> request)
        {
            var joined = from p in _dbContext.HuCommendEmployees.AsNoTracking()
                             // JOIN OTHER ENTITIES BASED ON THE BUSINESS
                         select new HuCommendEmployeeDTO
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
                var list = new List<HU_COMMEND_EMPLOYEE>
                    {
                        (HU_COMMEND_EMPLOYEE)response
                    };
                var joined = (from l in list
                                  // JOIN OTHER ENTITIES BASED ON THE BUSINESS
                              select new HuCommendEmployeeDTO
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

        public async Task<FormatedResponse> Create(GenericUnitOfWork _uow, HuCommendEmployeeDTO dto, string sid)
        {
            var response = await _genericRepository.Create(_uow, dto, sid);
            return response;
        }

        public async Task<FormatedResponse> CreateRange(GenericUnitOfWork _uow, List<HuCommendEmployeeDTO> dtos, string sid)
        {
            var add = new List<HuCommendEmployeeDTO>();
            add.AddRange(dtos);
            var response = await _genericRepository.CreateRange(_uow, add, sid);
            return response;
        }

        public async Task<FormatedResponse> Update(GenericUnitOfWork _uow, HuCommendEmployeeDTO dto, string sid, bool patchMode = true)
        {
            var response = await _genericRepository.Update(_uow, dto, sid, patchMode);
            return response;
        }

        public async Task<FormatedResponse> UpdateRange(GenericUnitOfWork _uow, List<HuCommendEmployeeDTO> dtos, string sid, bool patchMode = true)
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
            await Task.Run(() => null);
            throw new NotImplementedException();
        }
        public async Task<FormatedResponse> ApproveCommend(List<long> Ids)
        {
            try
            {
                bool isCheck = false;
                string sid = "";
                bool pathMode = true;
                List<HuCommendEmployeeDTO> list = new();
                Ids.ForEach(item =>
                {
                    var response = _uow.Context.Set<HU_COMMEND_EMPLOYEE>().Where(x => x.ID == item).FirstOrDefault();
                    var getOtherList = _uow.Context.Set<SYS_OTHER_LIST>().Where(x => x.CODE == "DD").FirstOrDefault();
                    if (response == null && getOtherList == null)
                    {
                        isCheck = true;
                        return;
                    }
                    else
                    {
                        list.Add(new HuCommendEmployeeDTO()
                        {
                            Id = response!.ID,
                            StatusId = getOtherList!.ID
                        });
                    }
                });
                if (isCheck == false)
                {
                    return new FormatedResponse() { MessageCode = CommonMessageCode.ENTITY_NOT_FOUND, ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400 };
                }
                else
                {
                    var approveCommend = await _genericRepository.UpdateRange(_uow, list, sid, pathMode);
                    if (approveCommend != null)
                    {
                        return approveCommend;

                    }
                    else
                    {
                        return new FormatedResponse() { MessageCode = CommonMessageCode.ENTITY_NOT_FOUND, ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400 };
                    }
                }
            }
            catch (Exception ex)
            {

                return new FormatedResponse() { ErrorType = EnumErrorType.UNCATCHABLE, StatusCode = EnumStatusCode.StatusCode500, MessageCode = ex.Message };
            }
        }
        public async Task<FormatedResponse> OpenApproveCommend(List<long> Ids)
        {
            try
            {
                string sid = "";
                bool pathMode = true;
                bool isCheck = false;
                List<HuCommendEmployeeDTO> list = new();
                Ids.ForEach(item =>
                {
                    var response = _uow.Context.Set<HU_COMMEND_EMPLOYEE>().Where(x => x.ID == item).FirstOrDefault();
                    var getOtherList = _uow.Context.Set<SYS_OTHER_LIST>().Where(x => x.CODE == "CD").FirstOrDefault();
                    if (response!.STATUS_ID == getOtherList!.ID && getOtherList == null)
                    {
                        isCheck = true;
                        return;
                    }
                    else
                    {
                        list.Add(new()
                        {
                            Id = response.ID,
                            StatusId = getOtherList.ID
                        });
                    }
                });
                if (isCheck == true)
                {
                    return new FormatedResponse() { MessageCode = CommonMessageCode.ENTITY_NOT_FOUND, ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400 };
                }
                else
                {
                    var openApprove = await _genericRepository.UpdateRange(_uow, list, sid, pathMode);
                    if (openApprove != null)
                    {
                        return openApprove;
                    }
                    else
                    {
                        return new FormatedResponse() { MessageCode = CommonMessageCode.THE_UPDATED_FAILD, ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400 };
                    }
                }
            }
            catch (Exception ex)
            {

                return new FormatedResponse() { ErrorType = EnumErrorType.UNCATCHABLE, StatusCode = EnumStatusCode.StatusCode500, MessageCode = ex.Message };
            }
        }

    }
}

