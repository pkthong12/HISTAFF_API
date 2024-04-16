using CORE.GenericUOW;
using CORE.DTO;
using API.DTO;
using CORE.Enum;
using CORE.StaticConstant;
using API.All.DbContexts;
using CORE.AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using CoreDAL.Models;
using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;

namespace API.Controllers.AtSalaryPeriod
{
    public class AtSalaryPeriodRepository : IAtSalaryPeriodRepository
    {
        private readonly GenericUnitOfWork _uow;
        private readonly FullDbContext _dbContext;
        private IGenericRepository<AT_SALARY_PERIOD, AtSalaryPeriodDTO> _genericRepository;
        private IGenericRepository<AT_ORG_PERIOD, AtOrgPeriodDTO> _genericRepositoryChild;
        private readonly GenericReducer<AT_SALARY_PERIOD, AtSalaryPeriodDTO> _genericReducer;

        public AtSalaryPeriodRepository(FullDbContext context, GenericUnitOfWork uow)
        {
            _dbContext = context;
            _uow = uow;
            _genericRepository = _uow.GenericRepository<AT_SALARY_PERIOD, AtSalaryPeriodDTO>();
            _genericRepositoryChild = _uow.GenericRepository<AT_ORG_PERIOD, AtOrgPeriodDTO>();
            _genericReducer = new();
        }

        public async Task<GenericPhaseTwoListResponse<AtSalaryPeriodDTO>> SinglePhaseQueryList(GenericQueryListDTO<AtSalaryPeriodDTO> request)
        {
            var joined = from p in _dbContext.AtSalaryPeriods.AsNoTracking().AsQueryable()
                             /*from op in _dbContext.AtOrgPeriods.AsNoTracking().Where( x => x.PERIOD_ID == p.ID)*/
                         from c in _dbContext.SysUsers.AsNoTracking().Where(x => x.ID == p.CREATED_BY).DefaultIfEmpty()
                         from u in _dbContext.SysUsers.AsNoTracking().Where(x => x.ID == p.UPDATED_BY).DefaultIfEmpty()
                             // JOIN OTHER ENTITIES BASED ON THE BUSINESS
                         select new AtSalaryPeriodDTO
                         {
                             Id = p.ID,
                             PeriodId = p.ID,
                             Name = p.NAME,
                             Year = p.YEAR,
                             Month = p.MONTH,
                             EndDate = p.END_DATE,
                             StartDate = p.START_DATE,
                             StandardWorking = p.STANDARD_WORKING,
                             Note = p.NOTE,
                             IsActive = p.IS_ACTIVE,
                             CreatedDate = p.CREATED_DATE,
                             UpdatedDate = p.UPDATED_DATE,
                             UpdatedByUsername = u.USERNAME,
                             CreatedByUsername = c.USERNAME,
                             Actflg = p.IS_ACTIVE == true ? "Áp dụng" : "Ngừng áp dụng"
                             //PeriodId = s.PERIOD_ID
                         };

            var singlePhaseResult = await _genericReducer.SinglePhaseReduce(joined, request);
            var list = singlePhaseResult.List?.ToList();

            list?.ForEach(item =>
            {
                var orgIds = new List<long>();
                var op = _dbContext.AtOrgPeriods.AsNoTracking().Where(x => x.PERIOD_ID == item.Id).DefaultIfEmpty();
                op?.ToList().ForEach(x =>
                    {
                        if (x?.ORG_ID != null) orgIds.Add((long)x.ORG_ID);
                    });
                item.OrgIds = orgIds.Distinct().ToList();
            });
            singlePhaseResult.List = list?.AsQueryable();
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
            var joined = await (from l in _dbContext.AtSalaryPeriods
                          from c in _dbContext.SysUsers.Where(f=> l.CREATED_BY == f.ID).DefaultIfEmpty()
                          where l.ID == id
                          select new AtSalaryPeriodDTO
                          {
                              Id = l.ID,
                              Year = l.YEAR,
                              Month = l.MONTH,
                              Name = l.NAME,
                              StartDate = l.START_DATE,
                              EndDate = l.END_DATE,
                              StandardWorking = l.STANDARD_WORKING,
                              Note = l.NOTE,
                              CreatedDate = l.CREATED_DATE,
                              UpdatedDate = l.UPDATED_DATE,
                              CreatedByUsername = c.USERNAME,
                              PeriodId = l.ID,
                              UpdatedByUsername = c.USERNAME,
                          }).FirstOrDefaultAsync();
            var orgIdsObject = _dbContext.AtOrgPeriods.Where(x => x.PERIOD_ID == id).Select(x => new { OrgId = x.ORG_ID }).ToList();

            List<long> orgIds = new();
            orgIdsObject?.ForEach(item =>
            {
                if (item.OrgId != null) orgIds.Add((long)item.OrgId);
            });

            if (joined != null) joined.OrgIds = orgIds;

            return new() { InnerBody = joined };
        }

        public async Task<FormatedResponse> GetById(string id)
        {
            await Task.Run(() => null);
            throw new NotImplementedException();
        }

        public async Task<FormatedResponse> Create(GenericUnitOfWork _uow, AtSalaryPeriodDTO dto, string sid)
        {
            var check1 = _uow.Context.Set<AT_SALARY_PERIOD>().AsNoTracking().Where(x => x.MONTH == dto.Month);
            var check2 = _uow.Context.Set<AT_SALARY_PERIOD>().AsNoTracking().Where(x => x.YEAR == dto.Year);
            if (check1.Any() && check2.Any())
            {
                return new FormatedResponse() { MessageCode = CommonMessageCode.DATA_CAN_NOT_BE_DUPLICATED, ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400 };
            }
            else
            {
                var response = await _genericRepository.Create(_uow, dto, sid);
                return response;
            }
        }

        public async Task<FormatedResponse> CreateRange(GenericUnitOfWork _uow, List<AtSalaryPeriodDTO> dtos, string sid)
        {
            var add = new List<AtSalaryPeriodDTO>();
            add.AddRange(dtos);
            var response = await _genericRepository.CreateRange(_uow, add, sid);
            return response;
        }

        public async Task<FormatedResponse> Update(GenericUnitOfWork _uow, AtSalaryPeriodDTO dto, string sid, bool patchMode = true)
        {
            var response = await _genericRepository.Update(_uow, dto, sid, patchMode);
            return response;
        }

        public async Task<FormatedResponse> UpdateRange(GenericUnitOfWork _uow, List<AtSalaryPeriodDTO> dtos, string sid, bool patchMode = true)
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
            bool checkStatus = false;
            bool checkData = false;
            ids.ForEach(async item =>
            {
                var getSalaryPeriod = _uow.Context.Set<AT_SALARY_PERIOD>().Where(x => x.ID == item).FirstOrDefault();
                if(getSalaryPeriod.IS_ACTIVE == false)
                {
                    var getAtWorkSign = _dbContext.AtWorksigns.Where(x => x.PERIOD_ID == item).ToList();
                    var getAtTimeMonthly = _dbContext.AtTimesheetMonthlys.Where(x => x.PERIOD_ID == item).ToList();
                    var getPayrollSheetSum = _dbContext.PaPayrollsheetSums.Where(x => x.PERIOD_ID == item).ToList();
                    if (getAtWorkSign != null || getAtTimeMonthly != null || getPayrollSheetSum != null)
                    {
                        checkData = true;
                        return;
                    }
                }
                else
                {
                    checkStatus = true;
                    return;
                }
                var orgIds = _dbContext.AtOrgPeriods.Where(x => x.PERIOD_ID == item).ToList();
                if(orgIds != null)
                {
                    _dbContext.AtOrgPeriods.RemoveRange(orgIds);
                }
            });
            if (checkStatus == true)
            {
                return new FormatedResponse() { StatusCode = EnumStatusCode.StatusCode400, ErrorType = EnumErrorType.CATCHABLE, MessageCode = CommonMessageCode.CAN_NOT_DELETE_RECORDS_HAVE_IS_ACTIVE };
            }
            else if(checkData == true)
            {
                return new FormatedResponse() { StatusCode = EnumStatusCode.StatusCode400, ErrorType = EnumErrorType.CATCHABLE, MessageCode = CommonMessageCode.CAN_NOT_DELETE_RECORDS_HAVE_USE }; 
            }
            else
            {
                var response = await _genericRepository.DeleteIds(_uow, ids);
                return response;
            }
        }

        public async Task<FormatedResponse> DeleteIds(GenericUnitOfWork _uow, List<string> ids)
        {
            await Task.Run(() => null);
            throw new NotImplementedException();
        }

        public async Task<FormatedResponse> AddNewWithAtOrgPeriods(AtSalaryPeriodDTO reqquest, string sid)
        {
            // StartTransaction

            _uow.CreateTransaction();

            try
            {
                var check1 = _uow.Context.Set<AT_SALARY_PERIOD>().AsNoTracking().Where(x => x.MONTH == reqquest.Month && x.YEAR == reqquest.Year);
                reqquest.IsActive = true;
                DateTime startDate = new DateTime(reqquest.Year, (int)reqquest.Month, 1);
                DateTime endDate = new DateTime(reqquest.Year, startDate.Month, DateTime.DaysInMonth(reqquest.Year, (int)reqquest.Month));

                if (check1.Any())
                {
                    return new FormatedResponse() { MessageCode = CommonMessageCode.DATA_CAN_NOT_BE_DUPLICATED, ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400 };
                }
                else
                {
                    if(reqquest.StartDate > reqquest.EndDate)
                    {
                        return new() { StatusCode = EnumStatusCode.StatusCode400, ErrorType = EnumErrorType.CATCHABLE, MessageCode = CommonMessageCode.THE_START_DATE_MUST_BE_LESS_THAN_THE_END_DATE };
                    }
                    else
                    {
                        if (reqquest.StartDate < startDate && reqquest.EndDate > endDate /*&& reqquest.StartDate > reqquest.EndDate*/)
                        {
                            return new() { StatusCode = EnumStatusCode.StatusCode400, ErrorType = EnumErrorType.CATCHABLE, MessageCode = CommonMessageCode.THE_START_DATE_AND_END_DATE_MUST_BELONG_TO_THAT_MONTH };
                        }
                        else
                        {
                            if (reqquest.StartDate.Value.Year == reqquest.Year && reqquest.StartDate.Value.Month == reqquest.Month && reqquest.EndDate.Value.Year == reqquest.Year && reqquest.EndDate.Value.Month == reqquest.Month)
                            {
                                var newAtSalaryPeriodResponse = await _genericRepository.Create(_uow, reqquest, sid);

                                if (newAtSalaryPeriodResponse != null)
                                {
                                    var newAtSalaryPeriod = newAtSalaryPeriodResponse.InnerBody as AT_SALARY_PERIOD;

                                    if (newAtSalaryPeriod != null)
                                    {

                                        AtOrgPeriodDTO res = new AtOrgPeriodDTO()
                                        {
                                            PeriodId = newAtSalaryPeriod.ID,
                                        };
                                        if (reqquest.OrgIds != null)
                                        {
                                            List<AtOrgPeriodDTO> list = new();
                                            reqquest.OrgIds.ForEach(item =>
                                            {
                                                list.Add(new()
                                                {
                                                    OrgId = item,
                                                    PeriodId = newAtSalaryPeriod.ID

                                                }); ;
                                            });

                                            var addChildrenResponse = await _genericRepositoryChild.CreateRange(_uow, list, sid);

                                            if (addChildrenResponse != null)
                                            {
                                                if (addChildrenResponse.InnerBody != null)
                                                {
                                                    _uow.Commit();

                                                    return newAtSalaryPeriodResponse;

                                                }
                                                else
                                                {
                                                    _uow.Rollback();
                                                    return new() { StatusCode = EnumStatusCode.StatusCode400, ErrorType = EnumErrorType.CATCHABLE, MessageCode = addChildrenResponse.MessageCode };
                                                }
                                            }
                                            else
                                            {
                                                _uow.Rollback();
                                                return new() { StatusCode = EnumStatusCode.StatusCode400, ErrorType = EnumErrorType.CATCHABLE, MessageCode = CommonMessageCode.CREATE_RANGE_FOR_CHIDREN_RETURN_NULL };
                                            }
                                        }
                                        else
                                        {
                                            _uow.Commit();

                                            return newAtSalaryPeriodResponse;
                                        }

                                    }
                                    else
                                    {
                                        _uow.Rollback();
                                        return new() { StatusCode = EnumStatusCode.StatusCode400, ErrorType = EnumErrorType.CATCHABLE, MessageCode = CommonMessageCode.CREATE_PARENT_RESPONSE_RETURN_NULL };

                                    }
                                }
                                else
                                {
                                    _uow.Rollback();
                                    return new() { StatusCode = EnumStatusCode.StatusCode400, ErrorType = EnumErrorType.CATCHABLE, MessageCode = CommonMessageCode.CREATE_PARENT_RESPONSE_RETURN_NULL };
                                }
                            }
                            else
                            {
                                return new() { StatusCode = EnumStatusCode.StatusCode400, ErrorType = EnumErrorType.CATCHABLE, MessageCode = CommonMessageCode.THE_DATE_IS_NOT_IN_THE_MONTH };
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                _uow.Rollback();
                return new() { ErrorType = EnumErrorType.UNCATCHABLE, MessageCode = ex.Message, StatusCode = EnumStatusCode.StatusCode500 };

            }

        }

        public async Task<FormatedResponse> UpdateWithAtOrgPeriods(GenericUnitOfWork _uow, AtSalaryPeriodDTO reqquest, string sid, bool patchMode = true)
        {
            _uow.CreateTransaction();
            try
            {
                var check1 = _uow.Context.Set<AT_SALARY_PERIOD>().AsNoTracking().Where(x => x.MONTH == reqquest.Month && x.YEAR == reqquest.Year);
                var checkObject = _uow.Context.Set<AT_SALARY_PERIOD>().Where(x => x.ID == reqquest.Id && x.YEAR == reqquest.Year && x.MONTH == reqquest.Month);
                if(checkObject != null)
                {
                    DateTime dateStart = new DateTime(reqquest.Year, (int)reqquest.Month, 1);
                    DateTime dateEnd = new DateTime(reqquest.Year, dateStart.Month, DateTime.DaysInMonth(reqquest.Year, (int)reqquest.Month));
                    if (reqquest.StartDate > reqquest.EndDate)
                    {
                        return new() { StatusCode = EnumStatusCode.StatusCode400, ErrorType = EnumErrorType.CATCHABLE, MessageCode = CommonMessageCode.THE_START_DATE_MUST_BE_LESS_THAN_THE_END_DATE };
                    }
                    else
                    {
                        if(reqquest.StartDate.Value.Year == reqquest.Year && reqquest.StartDate.Value.Month == reqquest.Month && reqquest.EndDate.Value.Year == reqquest.Year && reqquest.EndDate.Value.Month == reqquest.Month)
                        {
                            var updateSalaryPeriodResponse = await _genericRepository.Update(_uow, reqquest, sid, patchMode);
                            if (updateSalaryPeriodResponse != null)
                            {
                                var updateSalaryPeriod = updateSalaryPeriodResponse.InnerBody as AT_SALARY_PERIOD;
                                if (updateSalaryPeriod != null)
                                {
                                    if (reqquest.OrgIds != null)
                                    {
                                        var orgIds = _dbContext.AtOrgPeriods.Where(x => x.PERIOD_ID == reqquest.Id).ToList();
                                        if (orgIds != null)
                                        {
                                            _dbContext.AtOrgPeriods.RemoveRange(orgIds);

                                        }
                                        List<AtOrgPeriodDTO> list = new();

                                        reqquest.OrgIds.ForEach(item =>
                                        {
                                            list.Add(new()
                                            {
                                                PeriodId = updateSalaryPeriod.ID,
                                                OrgId = item
                                            });
                                        });
                                        var updateChildrenResponse = await _genericRepositoryChild.CreateRange(_uow, list, sid);

                                        if (updateChildrenResponse != null)
                                        {
                                            if (updateChildrenResponse.InnerBody != null)
                                            {
                                                _uow.Commit();
                                                return updateSalaryPeriodResponse;

                                            }
                                            else
                                            {
                                                _uow.Rollback();
                                                return new() { StatusCode = EnumStatusCode.StatusCode400, ErrorType = EnumErrorType.CATCHABLE, MessageCode = updateChildrenResponse.MessageCode };
                                            }
                                        }
                                        else
                                        {
                                            _uow.Rollback();
                                            return new() { StatusCode = EnumStatusCode.StatusCode400, ErrorType = EnumErrorType.CATCHABLE, MessageCode = CommonMessageCode.CREATE_RANGE_FOR_CHIDREN_RETURN_NULL };
                                        }

                                    }
                                    else
                                    {
                                        _uow.Commit();

                                        return updateSalaryPeriodResponse;
                                    }
                                }
                                else
                                {
                                    _uow.Rollback();
                                    return new() { StatusCode = EnumStatusCode.StatusCode400, ErrorType = EnumErrorType.CATCHABLE, MessageCode = CommonMessageCode.CREATE_PARENT_RESPONSE_RETURN_NULL };

                                }
                            }
                            else
                            {
                                _uow.Rollback();
                                return new() { StatusCode = EnumStatusCode.StatusCode400, ErrorType = EnumErrorType.CATCHABLE, MessageCode = CommonMessageCode.CREATE_PARENT_RESPONSE_RETURN_NULL };
                            }
                        }
                        else
                        {
                            return new() { StatusCode = EnumStatusCode.StatusCode400, ErrorType = EnumErrorType.CATCHABLE, MessageCode = CommonMessageCode.THE_DATE_IS_NOT_IN_THE_MONTH };
                        }
                    }
                }
                else
                {
                    return new() { StatusCode = EnumStatusCode.StatusCode400, ErrorType=EnumErrorType.CATCHABLE, MessageCode = CommonMessageCode.THE_UPDATED_FAILD };    
                }

            }
            catch (Exception ex)
            {
                _uow.Rollback();
                return new() { ErrorType = EnumErrorType.UNCATCHABLE, MessageCode = ex.Message, StatusCode = EnumStatusCode.StatusCode500 };

            }
        }
        public async Task<FormatedResponse> GetListSalaryInYear(AtSalaryPeriodDTO param)
        {
            var queryable = await (from p in _dbContext.AtSalaryPeriods
                                   where p.IS_ACTIVE == true && p.YEAR == param.Year
                                   orderby p.START_DATE ascending
                                   select new
                                   {
                                       Id = p.ID,
                                       Name = p.NAME,
                                       Month = p.MONTH,

                                   }).ToListAsync();
            return new() { InnerBody = queryable };
        }

        public async Task<FormatedResponse> GetListSalaryPeriod(AtSalaryPeriodDTO param)
        {
            var queryable = await (from p in _dbContext.AtSalaryPeriods
                                   where p.ID == param.Id
                                   select new
                                   {
                                       Id = p.ID,
                                       Name = p.NAME,
                                       StartDate = p.START_DATE.Date,
                                       EndDate = p.END_DATE.Date,
                                       Year = p.YEAR,
                                       Month = p.MONTH,
                                   }).FirstOrDefaultAsync();
            return new() { InnerBody = queryable };
        }

        public virtual async Task<FormatedResponse> ToggleActiveIds(GenericUnitOfWork _uow, List<long> ids, bool valueToBind, string sid)
        {
            var response = await _genericRepository.ToggleActiveIds(_uow, ids, valueToBind, sid);
            return response;
        }
    }
}

