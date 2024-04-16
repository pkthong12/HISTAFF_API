using CORE.GenericUOW;
using CORE.DTO;
using API.DTO;
using CORE.Enum;
using CORE.StaticConstant;
using API.All.DbContexts;
using CORE.AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.SignalR;
using API.Socket;

namespace API.Controllers.HuFamilyEdit
{
    public class ApproveHuFamilyEditRepository : IApproveHuFamilyEditRepository
    {
        private readonly GenericUnitOfWork _uow;
        private readonly FullDbContext _dbContext;
        private IGenericRepository<HU_FAMILY_EDIT, HuFamilyEditDTO> _genericRepository;
        private IGenericRepository<HU_FAMILY, HuFamilyDTO> _genericParentRepository;
        private readonly GenericReducer<HU_FAMILY_EDIT, HuFamilyEditDTO> _genericReducer;
        IHubContext<SignalHub> _hubContext;

        public ApproveHuFamilyEditRepository(FullDbContext context, GenericUnitOfWork uow, IHubContext<SignalHub> hubContext)
        {
            _dbContext = context;
            _uow = uow;
            _genericRepository = _uow.GenericRepository<HU_FAMILY_EDIT, HuFamilyEditDTO>();
            _genericParentRepository = _uow.GenericRepository<HU_FAMILY, HuFamilyDTO>();
            _genericReducer = new();
            _hubContext = hubContext;
        }

        public async Task<GenericPhaseTwoListResponse<HuFamilyEditDTO>> SinglePhaseQueryList(GenericQueryListDTO<HuFamilyEditDTO> request)
        {
            var joined = from p in _dbContext.HuFamilyEdits.AsNoTracking()
                         from e in _dbContext.HuEmployees.AsNoTracking().Where(x => x.ID == p.EMPLOYEE_ID).DefaultIfEmpty()
                         from po in _dbContext.HuPositions.AsNoTracking().Where(x => x.ID == e.POSITION_ID).DefaultIfEmpty()
                         from j in _dbContext.HuJobs.AsNoTracking().Where(x=> x.ID == po.JOB_ID).DefaultIfEmpty()
                         from og in _dbContext.HuOrganizations.AsNoTracking().Where(x => x.ID == e.ORG_ID).DefaultIfEmpty()
                         from cv in _dbContext.HuEmployeeCvs.AsNoTracking().Where(x => x.ID == e.PROFILE_ID).DefaultIfEmpty()
                         from r in _dbContext.SysOtherLists.AsNoTracking().Where(x => x.ID == p.RELATIONSHIP_ID).DefaultIfEmpty()
                         from g in _dbContext.SysOtherLists.AsNoTracking().Where(x => x.ID == p.GENDER).DefaultIfEmpty()
                         from n in _dbContext.HuNations.AsNoTracking().Where(x => x.ID == p.NATIONALITY).DefaultIfEmpty()
                         from bp in _dbContext.HuProvinces.AsNoTracking().Where(x => x.ID == p.BIRTH_CER_PROVINCE).DefaultIfEmpty()
                         from bd in _dbContext.HuDistricts.AsNoTracking().Where(x => x.ID == p.BIRTH_CER_DISTRICT).DefaultIfEmpty()
                         from w in _dbContext.HuWards.AsNoTracking().Where(x => x.ID == p.BIRTH_CER_WARD).DefaultIfEmpty()
                         from or in _dbContext.SysOtherLists.AsNoTracking().Where(x => x.ID == cv.RELIGION_ID).DefaultIfEmpty()
                         from s in _dbContext.SysOtherLists.AsNoTracking().Where(x => x.ID == p.STATUS_ID).DefaultIfEmpty()
                         where(p.IS_SEND_PORTAL == true)
                         // JOIN OTHER ENTITIES BASED ON THE BUSINESS
                         select new HuFamilyEditDTO
                         {
                             Id = p.ID,
                             EmployeeId = p.EMPLOYEE_ID,
                             EmployeeCode = e.CODE,
                             JobOrderNum = (int)(j.ORDERNUM ?? 999),
                             EmployeeName = e.Profile.FULL_NAME,
                             RelationshipName = r.NAME,
                             CreatedDate = p.CREATED_DATE,
                             BirthDate = p.BIRTH_DATE,
                             Fullname = p.FULLNAME,
                             PitCode = p.PIT_CODE,
                             IdNo = p.ID_NO,
                             SameCompany = p.SAME_COMPANY,
                             IsDead = p.IS_DEAD,
                             IsDeduct = p.IS_DEDUCT,
                             IsHousehold = p.IS_HOUSEHOLD,
                             Career = p.CAREER,
                             RegistDeductDate = p.REGIST_DEDUCT_DATE,
                             DeductFrom = p.DEDUCT_FROM,
                             DeductTo = p.DEDUCT_TO,
                             NationalityName = n.NAME,
                             BirthCerDistrictName = bd.NAME,
                             BirthCerProvinceName = bp.NAME,
                             BirthCerWardName = w.NAME,
                             Note = p.NOTE,
                             ReligionName = or.NAME,
                             GenderName = g.NAME,
                             PositionName = po.NAME,
                             OrgName = og.NAME,
                             StatusName = s.NAME,
                             OrgId = e.ORG_ID

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
                var list = new List<HU_FAMILY_EDIT>
                    {
                        (HU_FAMILY_EDIT)response
                    };
                var joined = (from l in list
                                  // JOIN OTHER ENTITIES BASED ON THE BUSINESS
                              select new HuFamilyEditDTO
                              {
                                  Id = l.ID,
                                  Fullname = l.FULLNAME,
                                  RelationshipId = l.RELATIONSHIP_ID,
                                  Gender = l.GENDER,
                                  BirthDate = l.BIRTH_DATE,
                                  PitCode = l.PIT_CODE,
                                  SameCompany = l.SAME_COMPANY,
                                  IsDead = l.IS_DEAD,
                                  IsDeduct = l.IS_DEDUCT,
                                  RegistDeductDate = l.REGIST_DEDUCT_DATE,
                                  DeductFrom = l.DEDUCT_FROM,
                                  DeductTo = l.DEDUCT_TO,
                                  IsHousehold = l.IS_HOUSEHOLD,
                                  IdNo = l.ID_NO,
                                  Career = l.CAREER,
                                  Nationality = l.NATIONALITY,
                                  BirthCerProvince = l.BIRTH_CER_PROVINCE,
                                  BirthCerDistrict = l.BIRTH_CER_DISTRICT,
                                  BirthCerWard = l.BIRTH_CER_WARD,
                                  Note = l.NOTE,
                                  BirthCerPlace = l.BIRTH_CER_PLACE,
                                  StatusId = l.STATUS_ID
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

        public async Task<FormatedResponse> Create(GenericUnitOfWork _uow, HuFamilyEditDTO dto, string sid)
        {
            var response = await _genericRepository.Create(_uow, dto, sid);
            return response;
        }

        public async Task<FormatedResponse> CreateRange(GenericUnitOfWork _uow, List<HuFamilyEditDTO> dtos, string sid)
        {
            var add = new List<HuFamilyEditDTO>();
            add.AddRange(dtos);
            var response = await _genericRepository.CreateRange(_uow, add, sid);
            return response;
        }

        public async Task<FormatedResponse> Update(GenericUnitOfWork _uow, HuFamilyEditDTO dto, string sid, bool patchMode = true)
        {
            var response = await _genericRepository.Update(_uow, dto, sid, patchMode);
            return response;
        }

        public async Task<FormatedResponse> UpdateRange(GenericUnitOfWork _uow, List<HuFamilyEditDTO> dtos, string sid, bool patchMode = true)
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

        public async Task<FormatedResponse> GetAllHuFamilyEdit()
        {
            var joined = await( from p in _dbContext.HuFamilyEdits.AsNoTracking()
                         from e in _dbContext.HuEmployees.AsNoTracking().Where(x => x.ID == p.EMPLOYEE_ID).DefaultIfEmpty()
                         from po in _dbContext.HuPositions.AsNoTracking().Where(x => x.ID == e.POSITION_ID).DefaultIfEmpty()
                         from og in _dbContext.HuOrganizations.AsNoTracking().Where( x => x.ID == po.ORG_ID).DefaultIfEmpty()
                         from cv in _dbContext.HuEmployeeCvs.AsNoTracking().Where(x => x.ID == e.PROFILE_ID).DefaultIfEmpty()
                         from r in _dbContext.SysOtherLists.AsNoTracking().Where(x => x.ID == p.RELATIONSHIP_ID).DefaultIfEmpty()
                         from g in _dbContext.SysOtherLists.AsNoTracking().Where(x => x.ID == p.GENDER).DefaultIfEmpty()
                         from n in _dbContext.SysOtherLists.AsNoTracking().Where(x => x.ID == p.NATIONALITY).DefaultIfEmpty()
                         from bp in _dbContext.SysOtherLists.AsNoTracking().Where(x => x.ID == p.BIRTH_CER_PROVINCE).DefaultIfEmpty()
                         from bd in _dbContext.SysOtherLists.AsNoTracking().Where(x => x.ID == p.BIRTH_CER_DISTRICT).DefaultIfEmpty()
                         from w in _dbContext.SysOtherLists.AsNoTracking().Where(x => x.ID == p.BIRTH_CER_WARD).DefaultIfEmpty()
                         from or in _dbContext.SysOtherLists.AsNoTracking().Where(x => x.ID == cv.RELIGION_ID).DefaultIfEmpty()
                         where (p.IS_SEND_PORTAL == true && p.IS_APPROVE_PORTAL != true)
                         // JOIN OTHER ENTITIES BASED ON THE BUSINESS
                         select new HuFamilyEditDTO
                         {
                             Id = p.ID,
                             EmployeeId = p.EMPLOYEE_ID,
                             EmployeeCode = e.CODE,
                             EmployeeName = e.Profile.FULL_NAME,
                             OrgName = og.NAME,
                             PositionName = po.NAME,
                             RelationshipName = r.NAME,
                             CreatedDate = p.CREATED_DATE,
                             BirthDate = p.BIRTH_DATE,
                             Fullname = p.FULLNAME,
                             PitCode = p.PIT_CODE,
                             IdNo = p.ID_NO,
                             GenderName = g.NAME,
                             SameCompany = p.SAME_COMPANY,
                             IsDead = p.IS_DEAD,
                             IsDeduct = p.IS_DEDUCT,
                             DeductFrom = p.DEDUCT_FROM,
                             DeductTo = p.DEDUCT_TO,
                             RegistDeductDate = p.REGIST_DEDUCT_DATE,
                             IsHousehold = p.IS_HOUSEHOLD,
                             Career = p.CAREER,
                             NationalityName = n.NAME,
                             BirthCerDistrictName = bd.NAME,
                             BirthCerProvinceName = bp.NAME,
                             BirthCerWardName = w.NAME,
                             Note = p.NOTE,
                             ReligionName = or.NAME

                         }).ToListAsync();
            return new FormatedResponse() { InnerBody = joined };
        }

        public Task<FormatedResponse> ToggleActiveIds(GenericUnitOfWork _uow, List<long> ids, bool valueToBind, string sid)
        {
            throw new NotImplementedException();
        }
        public async Task<FormatedResponse> ApproveHuFamilyEdit(GenericUnapprovePortalDTO request)
        {
            try
            {
                string sid = "";
                bool pathMode = true;
                List<HuFamilyEditDTO> addList = new();
                List<HuFamilyDTO> updateListHuFamily = new();
                List<HuFamilyDTO> addEntity = new();
                var getOtherList = (from t in _uow.Context.Set<SYS_OTHER_LIST_TYPE>().Where(x => x.CODE == "STATUS")
                                    from o in _uow.Context.Set<SYS_OTHER_LIST>().DefaultIfEmpty()
                                    where o.CODE == "DD"
                                    select new { Id = o.ID }).FirstOrDefault();
                var getOtherList2 = (from t in _uow.Context.Set<SYS_OTHER_LIST_TYPE>().Where(x => x.CODE == "STATUS")
                                     from o in _uow.Context.Set<SYS_OTHER_LIST>().DefaultIfEmpty()
                                     where o.CODE == "TCPD"
                                     select new { Id = o.ID }).FirstOrDefault();

                if (request.ValueToBind == true)
                {
                    request.Ids.ForEach(item =>
                    {
                        var getData = _uow.Context.Set<HU_EMPLOYEE_CV_EDIT>().Where(x => x.ID == item).FirstOrDefault();
                        addList.Add(new()
                        {
                            Id = item,
                            StatusId = getOtherList?.Id,
                            IsSendPortal = false,
                        });
                        
                    });
                    var response = await _genericRepository.UpdateRange(_uow, addList, sid, pathMode);
                    if (response != null)
                    {

                        request.Ids.ForEach(item =>
                        {
                            var getData = _dbContext.HuFamilyEdits.Where(x => x.ID == item).FirstOrDefault();
                            if (getData != null)
                            {
                                
                                if (getData.HU_FAMILY_ID != null)
                                {
                                    updateListHuFamily.Add(new()
                                    {
                                        Id = getData.HU_FAMILY_ID,
                                        EmployeeId = getData.EMPLOYEE_ID,
                                        RelationshipId = getData.RELATIONSHIP_ID,
                                        Fullname = getData.FULLNAME,
                                        Gender = getData.GENDER,
                                        BirthDate = getData.BIRTH_DATE,
                                        PitCode = getData.PIT_CODE,
                                        SameCompany = getData.SAME_COMPANY,
                                        IsDead = getData.IS_DEAD,
                                        IsDeduct = getData.IS_DEDUCT,
                                        DeductFrom = getData.DEDUCT_FROM,
                                        DeductTo = getData.DEDUCT_TO,
                                        RegistDeductDate = getData.REGIST_DEDUCT_DATE,
                                        IsHousehold = getData.IS_HOUSEHOLD,
                                        IdNo = getData.ID_NO,
                                        Career = getData.CAREER,
                                        Nationality = getData.NATIONALITY,
                                        BirthCerProvince = getData.BIRTH_CER_PROVINCE,
                                        BirthCerDistrict = getData.BIRTH_CER_DISTRICT,
                                        BirthCerWard = getData.BIRTH_CER_WARD,
                                        UploadFile = getData.UPLOAD_FILE,
                                        StatusId = getData.STATUS_ID,
                                        Note = getData.NOTE,
                                        BirthCerPlace = getData.BIRTH_CER_PLACE
                                    });

                                    
                                    
                                }
                                else
                                {
                                    addEntity.Add(new()
                                    {
                                        Id = getData.HU_FAMILY_ID,
                                        EmployeeId = getData.EMPLOYEE_ID,
                                        RelationshipId = getData.RELATIONSHIP_ID,
                                        Fullname = getData.FULLNAME,
                                        Gender = getData.GENDER,
                                        BirthDate = getData.BIRTH_DATE,
                                        PitCode = getData.PIT_CODE,
                                        SameCompany = getData.SAME_COMPANY,
                                        IsDead = getData.IS_DEAD,
                                        IsDeduct = getData.IS_DEDUCT,
                                        DeductFrom = getData.DEDUCT_FROM,
                                        DeductTo = getData.DEDUCT_TO,
                                        RegistDeductDate = getData.REGIST_DEDUCT_DATE,
                                        IsHousehold = getData.IS_HOUSEHOLD,
                                        IdNo = getData.ID_NO,
                                        Career = getData.CAREER,
                                        Nationality = getData.NATIONALITY,
                                        BirthCerProvince = getData.BIRTH_CER_PROVINCE,
                                        BirthCerDistrict = getData.BIRTH_CER_DISTRICT,
                                        BirthCerWard = getData.BIRTH_CER_WARD,
                                        UploadFile = getData.UPLOAD_FILE,
                                        StatusId = getData.STATUS_ID,
                                        Note = getData.NOTE,
                                        BirthCerPlace = getData.BIRTH_CER_PLACE
                                    });
                                }

                                AT_NOTIFICATION noti = new AT_NOTIFICATION()
                                {
                                    CREATED_BY = sid,
                                    TYPE = 5,
                                    ACTION = 2,
                                    TITLE = request.Reason,
                                    STATUS_NOTIFY = 1,
                                    EMP_NOTIFY_ID = getData.EMPLOYEE_ID.ToString(),
                                    REF_ID = item,
                                    MODEL_CHANGE = getData.MODEL_CHANGE,
                                    CREATED_DATE = DateTime.Now
                                };
                                _dbContext.AtNotifications.AddRange(noti);
                                _dbContext.SaveChanges();

                                var employeeId = getData.EMPLOYEE_ID;
                                var users = _dbContext.SysUsers.Where(x => x.EMPLOYEE_ID == employeeId).ToList();
                                for(var i = 0; i < users.Count; i++)
                                {
                                    var username = users[i].USERNAME;
                                    if (!string.IsNullOrEmpty(username))
                                    {
                                        _hubContext.Clients.User(username).SendAsync("ReceiveMessage", new
                                        {
                                            SignalType = "APPROVE_NOTIFICATION",
                                            Message = "Bạn có thông báo mới trên Portal"/*message*/,
                                            Data = new
                                            {

                                            }
                                        });
                                    }
                                    
                                }
                                
                            }
                        });
                    }

                    if (updateListHuFamily.Count > 0)
                    {
                        var updateResponse = await _genericParentRepository.UpdateRange(_uow, updateListHuFamily, sid, pathMode);
                        return updateResponse;
                    }

                    if (addEntity.Count > 0)
                    {
                        var addResponse = await _genericParentRepository.CreateRange(_uow, addEntity, sid);
                    }
                    return response;
                }
                else
                {
                    if (request.Ids != null)
                    {
                        request.Ids.ForEach(item =>
                        {
                            var getData = _dbContext.HuFamilyEdits.Where(x => x.ID == item).FirstOrDefault();
                            addList.Add(new()
                            {
                                Id = item,
                                StatusId = getOtherList2?.Id,
                                IsSendPortal = false,
                                IsApprovePortal = true,
                                Reason = request.Reason
                            });
                            AT_NOTIFICATION noti = new AT_NOTIFICATION()
                            {
                                CREATED_BY = sid,
                                TYPE = 5,
                                ACTION = 2,
                                TITLE = request.Reason,
                                STATUS_NOTIFY = 2,
                                EMP_NOTIFY_ID = getData?.EMPLOYEE_ID.ToString(),
                                REF_ID = item,
                                MODEL_CHANGE = getData?.MODEL_CHANGE,
                                CREATED_DATE = DateTime.Now
                            };
                            _dbContext.AtNotifications.AddRange(noti);
                            _dbContext.SaveChanges();
                            var employeeId = getData.EMPLOYEE_ID;
                            var users = _dbContext.SysUsers.Where(x => x.EMPLOYEE_ID == employeeId).ToList();
                            for (var i = 0; i < users.Count; i++)
                            {
                                var username = users[i].USERNAME;
                                if (!string.IsNullOrEmpty(username))
                                {
                                    _hubContext.Clients.User(username).SendAsync("ReceiveMessage", new
                                    {
                                        SignalType = "APPROVE_NOTIFICATION",
                                        Message = "Bạn có thông báo mới trên Portal"/*message*/,
                                        Data = new
                                        {

                                        }
                                    });
                                }

                            }
                        });
                        var unapproveResponse = await _genericRepository.UpdateRange(_uow, addList, sid, pathMode);
                        return unapproveResponse;
                    }
                    else
                    {
                        return new FormatedResponse() { StatusCode = EnumStatusCode.StatusCode400, ErrorType = EnumErrorType.CATCHABLE, MessageCode = CommonMessageCode.APPROVE_FAIL };
                    }
                }
                
                
            }
            catch (Exception ex)
            {
                return new FormatedResponse() { MessageCode = ex.Message, ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode500 };
            }
        }
    }
}

