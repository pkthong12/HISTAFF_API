using CORE.GenericUOW;
using CORE.DTO;
using API.DTO;
using CORE.Enum;
using CORE.StaticConstant;
using API.All.DbContexts;
using CORE.AutoMapper;
using API.All.SYSTEM.CoreAPI.SysUserOrg;
using API.All.SYSTEM.CoreAPI.SysUserGroupOrg;

namespace API.Controllers.SysUserGroupOrg
{
    public class SysUserGroupOrgRepository : ISysUserGroupOrgRepository
    {
        private readonly GenericUnitOfWork _uow;
        private readonly FullDbContext _dbContext;
        private IGenericRepository<SYS_USER_GROUP_ORG, SysUserGroupOrgDTO> _genericRepository;
        private readonly GenericReducer<SYS_USER_GROUP_ORG, SysUserGroupOrgDTO> _genericReducer;

        public SysUserGroupOrgRepository(FullDbContext context, GenericUnitOfWork uow)
        {
            _dbContext = context;
            _uow = uow;
            _genericRepository = _uow.GenericRepository<SYS_USER_GROUP_ORG, SysUserGroupOrgDTO>();
            _genericReducer = new();
        }

        public async Task<GenericPhaseTwoListResponse<SysUserGroupOrgDTO>> SinglePhaseQueryList(GenericQueryListDTO<SysUserGroupOrgDTO> request)
        {
            var joined = from p in _dbContext.SysUserGroupOrgs.AsNoTracking()
                             // JOIN OTHER ENTITIES BASED ON THE BUSINESS
                         select new SysUserGroupOrgDTO
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
                var list = new List<SYS_USER_GROUP_ORG>
                    {
                        (SYS_USER_GROUP_ORG)response
                    };
                var joined = (from l in list
                                  // JOIN OTHER ENTITIES BASED ON THE BUSINESS
                              select new SysUserGroupOrgDTO
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

        public async Task<FormatedResponse> Create(GenericUnitOfWork _uow, SysUserGroupOrgDTO dto, string sid)
        {
            var response = await _genericRepository.Create(_uow, dto, sid);
            return response;
        }

        public async Task<FormatedResponse> CreateRange(GenericUnitOfWork _uow, List<SysUserGroupOrgDTO> dtos, string sid)
        {
            var add = new List<SysUserGroupOrgDTO>();
            add.AddRange(dtos);
            var response = await _genericRepository.CreateRange(_uow, add, sid);
            return response;
        }

        public async Task<FormatedResponse> Update(GenericUnitOfWork _uow, SysUserGroupOrgDTO dto, string sid, bool patchMode = true)
        {
            var response = await _genericRepository.Update(_uow, dto, sid, patchMode);
            return response;
        }

        public async Task<FormatedResponse> UpdateRange(GenericUnitOfWork _uow, List<SysUserGroupOrgDTO> dtos, string sid, bool patchMode = true)
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

        // This is for update GroupOrgPermission
        // using "remove old range, then insert new range" approach
        public async Task<FormatedResponse> UpdateGroupOrgPermissionRange(UpdateGroupOrgPermissionRangeDTO rangeRequest, string sid)
        {
            _uow.CreateTransaction();
            try
            {
                var groupId = rangeRequest.GroupId;
                var request = rangeRequest.Range;
                if (request == null) throw new ArgumentNullException();
                var checkGroup = (from r in request
                                  select new
                                  {
                                      r.GroupId
                                  }).DistinctBy(x => x.GroupId).ToList();
                if (checkGroup.Count <= 1)
                {

                    if (checkGroup.Count == 1)
                    {
                        if (checkGroup[0].GroupId == null)
                        {
                            _uow.Rollback();
                            return new()
                            {
                                ErrorType = EnumErrorType.CATCHABLE,
                                MessageCode = CommonMessageCode.GROUP_ID_WAS_NULL_IN_RANGE_ARRAY,
                                StatusCode = EnumStatusCode.StatusCode400
                            };
                        }

                        if (checkGroup[0].GroupId != groupId) throw new ArgumentOutOfRangeException();
                    }

                    var oldValue = _dbContext.SysUserGroupOrgs.AsNoTracking().Where(x => x.GROUP_ID == groupId);
                    _dbContext.SysUserGroupOrgs.RemoveRange(oldValue);

                    var now = DateTime.UtcNow;

                    List<SYS_USER_GROUP_ORG> newValue = new();

                    request.ForEach(item =>
                    {

                        if (item.GroupId == null || item.OrgId == null) new ArgumentNullException();

                        SYS_USER_GROUP_ORG row = new()
                        {
                            GROUP_ID = groupId,
                            ORG_ID = (long)item.OrgId,
                            CREATED_DATE = now,
                            UPDATED_DATE = now,
                            CREATED_BY = sid,
                            UPDATED_BY = sid
                        };

                        newValue.Add(row);
                    });

                    if (newValue.Count > 0) await _dbContext.AddRangeAsync(newValue);


                    await _dbContext.SaveChangesAsync();

                    var updatedValue = _dbContext.SysUserGroupOrgs.AsNoTracking().Where(x => x.GROUP_ID == groupId);

                    _uow.Commit();

                    return new() { InnerBody = updatedValue };
                }
                else
                {
                    _uow.Rollback();
                    return new()
                    {
                        ErrorType = EnumErrorType.CATCHABLE,
                        MessageCode = CommonMessageCode.THIS_METHOD_IS_FOR_SINGLE_GROUP_ONLY,
                        StatusCode = EnumStatusCode.StatusCode400
                    };
                }

            }
            catch (Exception ex)
            {
                _uow.Rollback();
                return new()
                {
                    ErrorType = EnumErrorType.UNCATCHABLE,
                    MessageCode = ex.Message,
                    StatusCode = EnumStatusCode.StatusCode500
                };
            }
        }

        public virtual async Task<FormatedResponse> ToggleActiveIds(GenericUnitOfWork _uow, List<long> ids, bool valueToBind, string sid)
        {
            await Task.Run(() => null);
            throw new NotImplementedException();
        }

    }
}

