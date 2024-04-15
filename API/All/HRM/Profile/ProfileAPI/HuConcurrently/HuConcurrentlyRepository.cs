using CORE.GenericUOW;
using CORE.DTO;
using API.DTO;
using CORE.Enum;
using CORE.StaticConstant;
using API.All.DbContexts;
using CORE.AutoMapper;

namespace API.Controllers.HuConcurrently
{
    public class HuConcurrentlyRepository : IHuConcurrentlyRepository
    {
        private readonly GenericUnitOfWork _uow;
        private readonly FullDbContext _dbContext;
        private IGenericRepository<HU_CONCURRENTLY, HuConcurrentlyDTO> _genericRepository;
        private IGenericRepository<HU_POSITION, HuPositionDTO> _positionRepository;
        private readonly GenericReducer<HU_CONCURRENTLY, HuConcurrentlyDTO> _genericReducer;

        public HuConcurrentlyRepository(FullDbContext context, GenericUnitOfWork uow)
        {
            _dbContext = context;
            _uow = uow;
            _genericRepository = _uow.GenericRepository<HU_CONCURRENTLY, HuConcurrentlyDTO>();
            _positionRepository = _uow.GenericRepository<HU_POSITION, HuPositionDTO>();
            _genericReducer = new();
        }

        public async Task<GenericPhaseTwoListResponse<HuConcurrentlyDTO>> SinglePhaseQueryList(GenericQueryListDTO<HuConcurrentlyDTO> request)
        {
            var joined = from p in _dbContext.HuConcurrentlys.AsNoTracking()
                         from o in _dbContext.HuOrganizations.Where(x => x.ID == p.ORG_ID).DefaultIfEmpty()
                         from c in _dbContext.HuPositions.Where(x => x.ID == p.POSITION_ID).DefaultIfEmpty()
                         from y in _dbContext.SysOtherLists.Where(x => x.ID == p.STATUS_ID).DefaultIfEmpty()
                         from e in _dbContext.HuEmployees.Where(x => x.ID == p.EMPLOYEE_ID).DefaultIfEmpty()
                         from f in _dbContext.HuEmployees.Where(x => x.ID == p.SIGNING_EMPLOYEE_ID).DefaultIfEmpty()
                         from q in _dbContext.HuEmployeeCvs.Where(x => x.ID == f.PROFILE_ID).DefaultIfEmpty()
                         from otherList in _dbContext.SysOtherLists.Where(x => x.ID == p.POSITION_POLITICAL_ID).DefaultIfEmpty()
                             // JOIN OTHER ENTITIES BASED ON THE BUSINESS
                         select new HuConcurrentlyDTO
                         {
                             Id = p.ID,
                             IsActive = p.IS_ACTIVE,
                             EffectiveDate = p.EFFECTIVE_DATE,
                             ExpirationDate = p.EXPIRATION_DATE,
                             DecisionNumber = p.DECISION_NUMBER,
                             OrgId = p.ORG_ID,
                             PositionId = p.POSITION_ID,
                             OrgName = o.NAME,
                             PositionTitle = c.NAME,
                             EmployeeId = p.EMPLOYEE_ID,
                             SigningEmployeeId = p.SIGNING_EMPLOYEE_ID,
                             SigningEmployeeName = q.FULL_NAME,
                             SigningPositionName = p.SIGNING_POSITION_NAME,
                             SigningDate = p.SIGNING_DATE,
                             Note = p.NOTE,
                             StatusId = p.STATUS_ID,
                             Status = y.NAME,
                             PositionConcurrentName = c.NAME,
                             OrgConcurrentName = o.NAME,
                             Code = c.CODE,
                             EmployeeCode = e.CODE,
                             EmployeeName = e.Profile!.FULL_NAME,
                             Name = c.NAME,
                             IsFromWorking = p.IS_FROM_WORKING,
                             PositionPoliticalName = otherList.NAME
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
            try
            {
                var joined = await (from l in _dbContext.HuConcurrentlys.AsNoTracking()
                              from e in _dbContext.HuEmployees.Where(p => p.ID == l.EMPLOYEE_ID).AsNoTracking().DefaultIfEmpty()
                              from cv in _dbContext.HuEmployeeCvs.Where(p => p.ID == e.PROFILE_ID).AsNoTracking().DefaultIfEmpty()
                              from p in _dbContext.HuPositions.Where(p => p.ID == l.POSITION_ID).AsNoTracking().DefaultIfEmpty()
                              from s in _dbContext.SysOtherLists.Where(s => s.ID == l.STATUS_ID).AsNoTracking().DefaultIfEmpty()
                                  // JOIN OTHER ENTITIES BASED ON THE BUSINESS
                              where l.ID == id
                              select new HuConcurrentlyDTO
                              {
                                  Id = l.ID,
                                  IsActive = l.IS_ACTIVE,
                                  EffectiveDate = l.EFFECTIVE_DATE,
                                  ExpirationDate = l.EXPIRATION_DATE,
                                  DecisionNumber = l.DECISION_NUMBER,
                                  OrgId = l.ORG_ID,
                                  PositionId = l.POSITION_ID,
                                  SigningDate = l.SIGNING_DATE,
                                  SigningEmployeeId = l.SIGNING_EMPLOYEE_ID,
                                  EmployeeId = l.EMPLOYEE_ID,
                                  SigningPositionName = l.SIGNING_POSITION_NAME,
                                  FullNameOnConcurrently = e.CODE + " - " + cv.FULL_NAME,
                                  Note = l.NOTE,
                                  StatusId = l.STATUS_ID,
                                  Status = s.NAME,
                                  Name = p.CODE + " - " + p.NAME,
                                  IsFromWorking = l.IS_FROM_WORKING,
                                  PositionPoliticalId = l.POSITION_POLITICAL_ID
                              }).FirstOrDefaultAsync();

                return new FormatedResponse() { InnerBody = joined };
            }
            catch (Exception ex)
            {
                return new FormatedResponse() { MessageCode = CommonMessageCode.ENTITY_NOT_FOUND, ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400 };
            }

        }
        
        public async Task<FormatedResponse> GetById(string id)
        {
            await Task.Run(() => null);
            throw new NotImplementedException();
        }

        public async Task<FormatedResponse> Create(GenericUnitOfWork _uow, HuConcurrentlyDTO dto, string sid)
        {
            var getData = _dbContext.HuConcurrentlys.Where(x => x.EMPLOYEE_ID == dto.EmployeeId && x.POSITION_POLITICAL_ID == dto.PositionPoliticalId)
                .OrderByDescending(x => x.EFFECTIVE_DATE).Take(1).FirstOrDefault();
            if (getData != null && getData.EFFECTIVE_DATE > dto.EffectiveDate)
            {
                return new FormatedResponse() { StatusCode = EnumStatusCode.StatusCode400, MessageCode = CommonMessageCode.DATE_EFFECTIVE_EXISTS };
            }
            var response = await _genericRepository.Create(_uow, dto, sid);
            return response;
        }

        public async Task<FormatedResponse> CreateRange(GenericUnitOfWork _uow, List<HuConcurrentlyDTO> dtos, string sid)
        {
            var add = new List<HuConcurrentlyDTO>();
            add.AddRange(dtos);
            var response = await _genericRepository.CreateRange(_uow, add, sid);
            return response;
        }

        public async Task<FormatedResponse> Update(GenericUnitOfWork _uow, HuConcurrentlyDTO dto, string sid, bool patchMode = true)
        {
            var response = await _genericRepository.Update(_uow, dto, sid, patchMode);
            return response;
        }

        public async Task<FormatedResponse> UpdateRange(GenericUnitOfWork _uow, List<HuConcurrentlyDTO> dtos, string sid, bool patchMode = true)
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

        public async Task<GenericPhaseTwoListResponse<HuConcurrentlyDTO>> GetConcurrentEmployee(GenericQueryListDTO<HuConcurrentlyDTO> request)
        {
            var joined = from p in _dbContext.HuConcurrentlys.AsNoTracking()
                         from o in _dbContext.HuOrganizations.Where(x => x.ID == p.ORG_ID).DefaultIfEmpty()
                         from c in _dbContext.HuPositions.Where(x => x.ID == p.POSITION_ID).DefaultIfEmpty()
                         from y in _dbContext.SysOtherLists.Where(x => x.ID == p.STATUS_ID).DefaultIfEmpty()
                         from e in _dbContext.HuEmployees.Where(x => x.ID == p.EMPLOYEE_ID).DefaultIfEmpty()
                             // JOIN OTHER ENTITIES BASED ON THE BUSINESS
                         select new HuConcurrentlyDTO
                         {
                             Id = p.ID,
                             IsActive = p.IS_ACTIVE,
                             EffectiveDate = p.EFFECTIVE_DATE,
                             ExpirationDate = p.EXPIRATION_DATE,
                             DecisionNumber = p.DECISION_NUMBER,
                             OrgId = p.ORG_ID,
                             PositionId = p.POSITION_ID,
                             OrgName = o.NAME,
                             PositionTitle = c.NAME,
                             EmployeeId = p.EMPLOYEE_ID,
                             SigningEmployeeId = p.SIGNING_EMPLOYEE_ID,
                             SigningPositionName = p.SIGNING_POSITION_NAME,
                             Note = p.NOTE,
                             StatusId = p.STATUS_ID,
                             Status = y.NAME,
                             PositionConcurrentName = c.NAME,
                             OrgConcurrentName = o.NAME,
                             Code = c.CODE,
                             EmployeeCode = e.CODE,
                             EmployeeName = e.Profile!.FULL_NAME,
                             Name = c.NAME,
                             IsFromWorking = p.IS_FROM_WORKING
                         };

            var singlePhaseResult = await _genericReducer.SinglePhaseReduce(joined, request);
            return singlePhaseResult;
        }

        public virtual async Task<FormatedResponse> ToggleActiveIds(GenericUnitOfWork _uow, List<long> ids, bool valueToBind, string sid)
        {
            await Task.Run(() => null);
            throw new NotImplementedException();
        }
        public async Task<FormatedResponse> GetEmployeeByConcurrentId(long id)
        {
            var response = await (from c in _dbContext.HuConcurrentlys
                                  from e in _dbContext.HuEmployees.Where(x => x.ID == c.EMPLOYEE_ID)
                                  where c.ID == id
                                  select new
                                  {
                                      Id = c.ID,
                                      EmployeeCode = e.CODE,
                                      EmployeeConcurrentName = e.Profile.FULL_NAME
                                  }).SingleOrDefaultAsync();
            return new FormatedResponse() { InnerBody = response };
        }
        public async Task<FormatedResponse> GetPositionPolitical()
        {
            try
            {
                var response = await (from t in _dbContext.SysOtherListTypes.Where(x => x.CODE == "POLITICAL_ORGANIZATION_TYPE")
                                      from o in _dbContext.SysOtherLists.Where(x => x.TYPE_ID == t.ID).DefaultIfEmpty()
                                      select new
                                      {
                                          Id = o.ID,
                                          Name = o.NAME,
                                      }).ToListAsync();
                return new FormatedResponse() { InnerBody = response };
            }
            catch (Exception ex)
            {

                return new FormatedResponse() { ErrorType = EnumErrorType.UNCATCHABLE, MessageCode = ex.Message, StatusCode = EnumStatusCode.StatusCode500 };
            }
        }
        public void TranferPosition()
        {
            List<HuPositionDTO> listUpdatePosition = new();
            var getListConcurrent = _uow.Context.Set<HU_CONCURRENTLY>().Where(x => x.EFFECTIVE_DATE == DateTime.Now).ToList();
            getListConcurrent.ForEach(item =>
            {
                var getPosition = _uow.Context.Set<HU_POSITION>().Where(x => x.ID == item.POSITION_ID).FirstOrDefault();
                if (getPosition?.MASTER != null)
                {
                    getPosition.INTERIM = getPosition.MASTER;
                    getPosition.MASTER = item.EMPLOYEE_ID;
                    _uow.Context.Update(getPosition);
                    _uow.Save();

                }
                else
                {
                    getPosition.MASTER = item.EMPLOYEE_ID;
                    _uow.Context.Update(getPosition);
                    _uow.Save();
                }
            });
        }

        public void ChangePositionPoliticalByDate()
        {
            List<HU_EMPLOYEE_CV> listEmployeeCv = new();
            var getListConccurent = (from c in _uow.Context.Set<HU_CONCURRENTLY>()
                                     from e in _uow.Context.Set<HU_EMPLOYEE>().Where(x => x.ID == c.EMPLOYEE_ID).DefaultIfEmpty()
                                     from cv in _uow.Context.Set<HU_EMPLOYEE_CV>().Where(x => x.ID == e.PROFILE_ID).DefaultIfEmpty()
                                     from sys in _uow.Context.Set<SYS_OTHER_LIST>().Where(x => x.ID == c.POSITION_POLITICAL_ID).DefaultIfEmpty()
                                     select new 
                                     {
                                         Id = cv.ID,
                                         IsUnionist = cv.IS_UNIONIST,
                                         IsJoinYouthGroup = cv.IS_JOIN_YOUTH_GROUP,
                                         IsMember = cv.IS_MEMBER,
                                         SysCode = sys.CODE,
                                         EffectDate = c.EFFECTIVE_DATE,
                                         ExpireDate = c.EXPIRATION_DATE
                                     }).ToList();
            getListConccurent.ForEach(item =>
            {


                if(item.EffectDate >= DateTime.Now)
                {
                    if (item.SysCode == "00290")
                    {
                        listEmployeeCv.Add(new()
                        {
                            ID = item.Id,
                            IS_UNIONIST = true
                        });
                    }
                    if (item.SysCode == "00291")
                    {
                        listEmployeeCv.Add(new()
                        {
                            ID = item.Id,
                            IS_JOIN_YOUTH_GROUP = true
                        });
                    }
                    if (item.SysCode == "00292")
                    {
                        listEmployeeCv.Add(new()
                        {
                            ID = item.Id,
                            IS_MEMBER = true
                        });
                    }
                }
                if(item.ExpireDate != null && item.ExpireDate <= DateTime.Now)
                {
                    if (item.SysCode == "00290")
                    {
                        listEmployeeCv.Add(new()
                        {
                            ID = item.Id,
                            IS_UNIONIST = false
                        });
                    }
                    if (item.SysCode == "00291")
                    {
                        listEmployeeCv.Add(new()
                        {
                            ID = item.Id,
                            IS_JOIN_YOUTH_GROUP = false
                        });
                    }
                    if (item.SysCode == "00292")
                    {
                        listEmployeeCv.Add(new()
                        {
                            ID = item.Id,
                            IS_MEMBER = false
                        });
                    }
                }
            });

            _uow.Context.UpdateRange(listEmployeeCv);
            _uow.Context.SaveChanges();


        }
        public async Task<FormatedResponse> GetAllConcurrentByEmployeeCvId(long employeeCvId)
        {
            try
            {
                var response = await (from cv in _dbContext.HuEmployeeCvs
                                      from e in _dbContext.HuEmployees.Where(x => x.PROFILE_ID == cv.ID).DefaultIfEmpty()
                                      from con in _dbContext.HuConcurrentlys.Where(x => x.EMPLOYEE_ID == e.ID).DefaultIfEmpty()
                                      from sys in _dbContext.SysOtherLists.Where(x => x.ID == con.POSITION_POLITICAL_ID).DefaultIfEmpty()
                                      where cv.ID == employeeCvId
                                      select new
                                      {
                                          Id = con.ID,
                                          SysCode = sys.CODE
                                      }).ToListAsync();
                return new FormatedResponse() { StatusCode = EnumStatusCode.StatusCode200, InnerBody = response };
            }
            catch (Exception ex)
            {

                return new FormatedResponse() { ErrorType = EnumErrorType.UNCATCHABLE, MessageCode = ex.Message, StatusCode = EnumStatusCode.StatusCode500 };
            }
                                  
        }
    }
}

