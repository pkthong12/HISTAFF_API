using CORE.GenericUOW;
using CORE.DTO;
using API.DTO;
using CORE.Enum;
using CORE.StaticConstant;
using API.All.DbContexts;
using CORE.AutoMapper;

namespace API.Controllers.HuEvaluate
{
    public class HuEvaluateRepository : IHuEvaluateRepository
    {
        private readonly GenericUnitOfWork _uow;
        private readonly FullDbContext _dbContext;
        private IGenericRepository<HU_EVALUATE, HuEvaluateDTO> _genericRepository;
        private readonly GenericReducer<HU_EVALUATE, HuEvaluateDTO> _genericReducer;

        public HuEvaluateRepository(FullDbContext context, GenericUnitOfWork uow)
        {
            _dbContext = context;
            _uow = uow;
            _genericRepository = _uow.GenericRepository<HU_EVALUATE, HuEvaluateDTO>();
            _genericReducer = new();
        }

        public async Task<GenericPhaseTwoListResponse<HuEvaluateDTO>> SinglePhaseQueryList(GenericQueryListDTO<HuEvaluateDTO> request)
        {
            var otherList = _uow.Context.Set<SYS_OTHER_LIST>().AsQueryable();
            var employee = _uow.Context.Set<HU_EMPLOYEE>().AsQueryable();
            var organization = _uow.Context.Set<HU_ORGANIZATION>().AsQueryable();
            var concurrent = _uow.Context.Set<HU_CONCURRENTLY>().AsQueryable();
            var  position = _uow.Context.Set<HU_POSITION>().AsQueryable();
            var evaluate = _uow.Context.Set<HU_EVALUATE>().AsQueryable();
            var classification = _uow.Context.Set<HU_CLASSIFICATION>().AsQueryable();
            var job = _uow.Context.Set<HU_JOB>().AsQueryable();
            var joined = from p in _dbContext.HuEvaluates
                         from et in otherList.Where(x => x.ID == p.EVALUATE_TYPE).DefaultIfEmpty()
                         from cl in classification.Where(x => x.ID == p.CLASSIFICATION_ID).DefaultIfEmpty()
                         from ot in otherList.Where(x => x.ID == cl.CLASSIFICATION_LEVEL).DefaultIfEmpty()
                         from e in employee.Where(x => x.ID == p.EMPLOYEE_ID).DefaultIfEmpty()
                         from c in concurrent.Where(x => x.ID == p.EMPLOYEE_CONCURRENT_ID).DefaultIfEmpty()
                         from ec in employee.Where(x => x.ID == c.EMPLOYEE_ID).DefaultIfEmpty()
                         from o in organization.Where(x => x.ID == p.ORG_ID).DefaultIfEmpty()
                         from po in position.Where(x => x.ID == p.POSITION_ID).DefaultIfEmpty()
                         from j in job.Where(x => x.ID == po.JOB_ID).DefaultIfEmpty()
                         from pco in position.Where(x => x.ID == p.POSITION_CONCURRENT_ID).DefaultIfEmpty()
                         from jc in job.Where(x => x.ID == pco.JOB_ID).DefaultIfEmpty()
                         from se in otherList.Where(x => x.ID == e.WORK_STATUS_ID).DefaultIfEmpty()
                             // JOIN OTHER ENTITIES BASED ON THE BUSINESS
                         select new HuEvaluateDTO
                         {
                             Id = p.ID,
                             EvaluateType = p.EVALUATE_TYPE,
                             EvaluateName = et.NAME,
                             ClassificationId = p.CLASSIFICATION_ID,
                             ClassificationName = ot.NAME,
                             EmployeeConcurrentId = p.EMPLOYEE_CONCURRENT_ID,
                             EmployeeCode = p.EMPLOYEE_ID != null ? e.CODE : ec.CODE,
                             EmployeeName = p.EMPLOYEE_NAME != "" ? p.EMPLOYEE_NAME : p.EMPLOYEE_CONCURRENT_NAME,
                             OrgId = p.ORG_ID  != null ? p.ORG_ID : p.ORG_CONCURRENT_ID , 
                             OrgName = p.ORG_NAME != "" ? p.ORG_NAME : p.ORG_CONCURRENT_NAME,
                             PositionName =  p.POSITION_NAME != "" ? p.POSITION_NAME : p.POSITION_CONCURRENT_NAME ,
                             Year = p.YEAR,
                             YearSearch = p.YEAR.ToString(),
                             PointSearch = p.POINT.ToString(),
                             Point = p.POINT,
                             Note = p.NOTE,
                             CreatedDate = p.CREATED_DATE,
                             UpdatedDate = p.UPDATED_DATE,
                             WorkStatusId = e.WORK_STATUS_ID,
                             WorkStatusName = se.NAME,
                             EmployeeStatus = e.WORK_STATUS_ID,
                             JobOrderNum = (int)((p.POSITION_NAME != "" ? (j.ORDERNUM ?? 99) : (jc.ORDERNUM ?? 99)))
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
                var list = new List<HU_EVALUATE>
                    {
                        (HU_EVALUATE)response
                    };
                var joined = (from l in list
                              // JOIN OTHER ENTITIES BASED ON THE BUSINESS
                              select new HuEvaluateDTO
                              {
                                  Id = l.ID,
                                  EvaluateType = l.EVALUATE_TYPE,
                                  PositionId = l.POSITION_ID,
                                  OrgId = l.ORG_ID,
                                  EmployeeId = l.EMPLOYEE_ID,
                                  EmployeeCode = l.EMPLOYEE_CODE,
                                  EmployeeName = l.EMPLOYEE_NAME,
                                  PositionConcurrentName = l.POSITION_CONCURRENT_NAME,
                                  OrgConcurrentName = l.ORG_CONCURRENT_NAME,
                                  Year = l.YEAR,
                                  ClassificationId = l.CLASSIFICATION_ID,
                                  Point = l.POINT,
                                  Note = l.NOTE,
                                  PositionName = l.POSITION_NAME,
                                  OrgName = l.ORG_NAME,
                                  PositionConcurrentId= l.POSITION_CONCURRENT_ID,
                                  EmployeeConcurrentId = l.EMPLOYEE_CONCURRENT_ID,
                                  EmployeeConcurrentName = l.EMPLOYEE_CONCURRENT_NAME
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

        public async Task<FormatedResponse> Create(GenericUnitOfWork _uow, HuEvaluateDTO dto, string sid)
        {
            try
            {
                if (dto.Point <= 100 && dto.Point >= 0)
                {
                    if(dto.EmployeeId != null)
                    {
                        var getDataByEmpId = _dbContext.HuEvaluates.Where(x => x.EMPLOYEE_ID == dto.EmployeeId && x.YEAR == dto.Year && x.EVALUATE_TYPE == dto.EvaluateType).ToList();
                        if(getDataByEmpId.Count > 0)
                        {
                            return new FormatedResponse() { StatusCode = EnumStatusCode.StatusCode400, ErrorType = EnumErrorType.CATCHABLE, MessageCode = CommonMessageCode.DUBLICATE_VALUE_EVALUATE_TYPE_YEAR_EMPLOYEE_CONCURRENT_ID };
                        }
                        else
                        {
                            var response = await _genericRepository.Create(_uow, dto, sid);
                            return response;
                        }
                    }
                    else if(dto.EmployeeConcurrentId != null)
                    {
                        var getDataByEmpConId = _dbContext.HuEvaluates.Where(x => x.EMPLOYEE_CONCURRENT_ID == dto.EmployeeConcurrentId && x.YEAR == dto.Year && x.EVALUATE_TYPE == dto.EvaluateType).ToList();
                        if(getDataByEmpConId.Count > 0)
                        {
                            return new FormatedResponse() { StatusCode = EnumStatusCode.StatusCode400, ErrorType = EnumErrorType.CATCHABLE, MessageCode = CommonMessageCode.DUBLICATE_VALUE_EVALUATE_TYPE_YEAR_EMPLOYEE_CONCURRENT_ID };
                        }
                        else
                        {
                            var response = await _genericRepository.Create(_uow, dto, sid);
                            return response;
                        }
                    }
                    else
                    {
                        return new FormatedResponse() { StatusCode = EnumStatusCode.StatusCode400, ErrorType = EnumErrorType.CATCHABLE, MessageCode = CommonMessageCode.CREATED_FAILD };
                    }
                    
                }
                else
                {
                    return new() { StatusCode = EnumStatusCode.StatusCode400, ErrorType = EnumErrorType.CATCHABLE, MessageCode = CommonMessageCode.CREATE_OBJECT_NUMBER_IS_NOT_ALLOWED };
                }

            }catch (Exception ex)
            {
                return new() { StatusCode = EnumStatusCode.StatusCode500, ErrorType = EnumErrorType.CATCHABLE, MessageCode = ex.Message };

            }
        }

        public async Task<FormatedResponse> CreateRange(GenericUnitOfWork _uow, List<HuEvaluateDTO> dtos, string sid)
        {
            var add = new List<HuEvaluateDTO>();
            add.AddRange(dtos);
            var response = await _genericRepository.CreateRange(_uow, add, sid);
            return response;
        }

        public async Task<FormatedResponse> Update(GenericUnitOfWork _uow, HuEvaluateDTO dto, string sid, bool patchMode = true)
        {
            try
            {
                if (dto.Point <= 100 && dto.Point >= 0)
                {
                    if (dto.EmployeeId != null)
                    {
                        var getDataByEmpId = _dbContext.HuEvaluates.Where(x => x.ID == dto.Id && x.EMPLOYEE_ID == dto.EmployeeId && x.EVALUATE_TYPE == dto.EvaluateType && x.YEAR == dto.Year).ToList();
                        if (getDataByEmpId != null)
                        {
                            var response = await _genericRepository.Update(_uow, dto, sid, patchMode);
                            return response;

                        }
                        else
                        {
                            var getDataCheck = _dbContext.HuEvaluates.Where(x => x.EMPLOYEE_ID == dto.EmployeeId && x.EVALUATE_TYPE == dto.EvaluateType && x.YEAR == dto.Year).ToList();
                            if (getDataCheck.Count > 0)
                            {
                                return new FormatedResponse() { StatusCode = EnumStatusCode.StatusCode400, ErrorType = EnumErrorType.CATCHABLE, MessageCode = CommonMessageCode.DUBLICATE_VALUE_EVALUATE_TYPE_YEAR_EMPLOYEE_CONCURRENT_ID };
                            }
                            else
                            {
                                var response = await _genericRepository.Update(_uow, dto, sid, patchMode);
                                return response;
                            }
                        }
                    }
                    else if (dto.EmployeeConcurrentId != null)
                    {
                        var getDataByEmpConId = _dbContext.HuEvaluates.Where(x => x.ID == dto.Id && x.EMPLOYEE_CONCURRENT_ID == dto.EmployeeConcurrentId && x.EVALUATE_TYPE == dto.EvaluateType && x.YEAR == dto.Year).ToList();
                        if (getDataByEmpConId != null)
                        {
                            var response = await _genericRepository.Update(_uow, dto, sid, patchMode);
                            return response;

                        }
                        else
                        {
                            var getDataCheck = _dbContext.HuEvaluates.Where(x => x.EMPLOYEE_CONCURRENT_ID == dto.EmployeeConcurrentId && x.EVALUATE_TYPE == dto.EvaluateType && x.YEAR == dto.Year).ToList();
                            if (getDataCheck.Count > 0)
                            {
                                return new FormatedResponse() { StatusCode = EnumStatusCode.StatusCode400, ErrorType = EnumErrorType.CATCHABLE, MessageCode = CommonMessageCode.DUBLICATE_VALUE_EVALUATE_TYPE_YEAR_EMPLOYEE_CONCURRENT_ID };
                            }
                            else
                            {
                                var response = await _genericRepository.Update(_uow, dto, sid, patchMode);
                                return response;
                            }
                        }
                    }
                    else
                    {
                        return new FormatedResponse() { StatusCode = EnumStatusCode.StatusCode400, ErrorType = EnumErrorType.CATCHABLE, MessageCode = CommonMessageCode.UPDATED_FAILD };
                    }
                }
                else
                {
                    return new() { StatusCode = EnumStatusCode.StatusCode400, ErrorType = EnumErrorType.CATCHABLE, MessageCode = CommonMessageCode.CREATE_OBJECT_NUMBER_IS_NOT_ALLOWED };
                }
            }
            catch (Exception ex)
            {

                return new() { StatusCode = EnumStatusCode.StatusCode500, ErrorType = EnumErrorType.CATCHABLE, MessageCode = ex.Message };
            }
        }

        public async Task<FormatedResponse> UpdateRange(GenericUnitOfWork _uow, List<HuEvaluateDTO> dtos, string sid, bool patchMode = true)
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

    }
}

