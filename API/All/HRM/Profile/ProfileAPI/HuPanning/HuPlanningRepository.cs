using CORE.GenericUOW;
using CORE.DTO;
using API.DTO;
using CORE.Enum;
using CORE.StaticConstant;
using API.All.DbContexts;
using CORE.AutoMapper;
using ProfileDAL.ViewModels;
using PayrollDAL.Models;
using System;
using CORE.Services.File;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;
using Org.BouncyCastle.Tls;

namespace API.Controllers.HuPlanning
{
    public class HuPlanningRepository : IHuPlanningRepository
    {
        private readonly GenericUnitOfWork _uow;
        private readonly FullDbContext _dbContext;
        private IGenericRepository<HU_PLANNING, HuPlanningDTO> _genericRepository;
        private readonly GenericReducer<HU_PLANNING, HuPlanningDTO> _genericReducer;

        public HuPlanningRepository(FullDbContext context, GenericUnitOfWork uow)
        {
            _dbContext = context;
            _uow = uow;
            _genericRepository = _uow.GenericRepository<HU_PLANNING, HuPlanningDTO>();
            _genericReducer = new();
        }

        public async Task<GenericPhaseTwoListResponse<HuPlanningDTO>> SinglePhaseQueryList(GenericQueryListDTO<HuPlanningDTO> request)
        {
            var joined = from pe in _dbContext.HuPlanningEmps.AsNoTracking()
                             // JOIN OTHER ENTITIES BASED ON THE BUSINESS
                         from p in _dbContext.HuPlannings.AsNoTracking().Where(x => x.ID == pe.PLANNING_ID)
                         from j in _dbContext.HuJobs.AsNoTracking().Where(x => x.ID == p.APP_LEVEL).DefaultIfEmpty()
                         from e in _dbContext.HuEmployees.AsNoTracking().Where(x => x.ID == pe.EMPLOYEE_ID).DefaultIfEmpty()
                         from pos in _dbContext.HuPositions.AsNoTracking().Where(x => x.ID == e.POSITION_ID).DefaultIfEmpty()
                         from o in _dbContext.HuOrganizations.AsNoTracking().Where(x => x.ID == e.ORG_ID).DefaultIfEmpty()
                         from p1 in _dbContext.SysOtherLists.AsNoTracking().Where(x => x.ID == p.PLANNING_PERIOD_ID).DefaultIfEmpty()
                         from p2 in _dbContext.SysOtherLists.AsNoTracking().Where(x => x.ID == pe.PLANNING_TITLE_ID).DefaultIfEmpty()
                         from p3 in _dbContext.SysOtherLists.AsNoTracking().Where(x => x.ID == pe.PLANNING_TYPE_ID).DefaultIfEmpty()
                         from g in _dbContext.SysOtherLists.AsNoTracking().Where(x => x.ID == e.Profile!.GENDER_ID).DefaultIfEmpty()
                         from n in _dbContext.SysOtherLists.AsNoTracking().Where(x => x.ID == e.Profile!.NATIVE_ID).DefaultIfEmpty()
                         from pla in _dbContext.SysOtherLists.AsNoTracking().Where(x => x.ID == e.Profile!.ID_PLACE).DefaultIfEmpty()

                         from cer in _dbContext.HuCertificates.AsNoTracking().Where(x => x.EMPLOYEE_ID == pe.EMPLOYEE_ID && x.IS_PRIME == true).OrderByDescending(x => x.EFFECT_FROM).Take(1).DefaultIfEmpty()
                         from levT in _dbContext.SysOtherLists.AsNoTracking().Where(x => x.ID == cer.LEVEL_TRAIN).DefaultIfEmpty()
                         from levId in _dbContext.SysOtherLists.AsNoTracking().Where(x => x.ID == cer.LEVEL_TRAIN).DefaultIfEmpty()
                         select new HuPlanningDTO
                         {
                             Id = pe.ID,
                             PlanningId = pe.PLANNING_ID,
                             EmployeeId = pe.EMPLOYEE_ID,
                             EmployeeCode = e.CODE,
                             EmployeeName = e.Profile!.FULL_NAME,
                             PositionName = pos.NAME,
                             OrgId = e.ORG_ID,
                             OrgName = o.NAME,
                             DecisionNo = p.DECISION_NO,
                             PlanningTitleName = p2.NAME,
                             PlanningTypeName = p3.NAME,
                             PlanningPeriodId = p.PLANNING_PERIOD_ID,
                             PlanningPeriodName = p1.NAME,
                             FromYearId = p.FROM_YEAR_ID,
                             ToYearId = p.TO_YEAR_ID,
                             WorkStatusId = e.WORK_STATUS_ID,
                             StartDate = e.JOIN_DATE,
                             GenderName = g.NAME,
                             BirthDate = e.Profile!.BIRTH_DATE,
                             NativeName = n.NAME,
                             PlaceName = pla.NAME,
                             MemberDate = e.Profile!.MEMBER_DATE,
                             PoliticalTheoryLevel = e.Profile!.POLITICAL_THEORY_LEVEL,
                             LevelTrainName = levT.NAME,
                             LevelName = levId.NAME
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

            var planningEmp = _uow.Context.Set<HU_PLANNING_EMP>().AsNoTracking().AsQueryable();
            var planning = _uow.Context.Set<HU_PLANNING>().AsNoTracking().AsQueryable();
            var employee = _uow.Context.Set<HU_EMPLOYEE>().AsNoTracking().AsQueryable();
            var otherList = _uow.Context.Set<SYS_OTHER_LIST>().AsNoTracking().AsQueryable();
            var position = _uow.Context.Set<HU_POSITION>().AsNoTracking().AsQueryable();
            var organization = _uow.Context.Set<HU_ORGANIZATION>().AsNoTracking().AsQueryable();


            var joined = await (from l in planning.AsNoTracking().Where(x => x.ID == id)
                                from e in employee.AsNoTracking().Where(e=> e.ID == l.SIGNER_ID).DefaultIfEmpty()
                                from cv in _dbContext.HuEmployeeCvs.AsNoTracking().Where(cv=> cv.ID == e.PROFILE_ID).DefaultIfEmpty()
                                from pos in _dbContext.HuPositions.AsNoTracking().Where(pos=> pos.ID == e.POSITION_ID).DefaultIfEmpty()
                              // JOIN OTHER ENTITIES BASED ON THE BUSINESS
                          select new HuPlanningInputDTO
                          {
                              Id = l.ID,
                              PlanningPeriodId = l.PLANNING_PERIOD_ID,
                              AppLevel = l.APP_LEVEL,
                              FromYearId = l.FROM_YEAR_ID,
                              ToYearId = l.TO_YEAR_ID,
                              DecisionNo = l.DECISION_NO,
                              EffectDate = l.EFFECT_DATE,
                              ExpireDate = l.EXPIRE_DATE,
                              TotalPersonnel = l.TOTAL_PERSONNEL,
                              SignDate = l.SIGN_DATE,
                              SignerId = l.SIGNER_ID,
                              PositionName = pos.NAME, 
                              SignerName = cv.FULL_NAME,
                              //AttachmentBuffer = l.ATTACHMENT,
                              AppLevelId = l.APP_LEVEL,
                              Note = l.NOTE,
                              Attachment = l.ATTACHMENT,
                              EmployeeList = new List<EmployeeDTO>(),
                              EmployeeIds = (from pe in planningEmp.Where(x => x.PLANNING_ID == id) select pe.EMPLOYEE_ID).ToList(),
                          }).FirstOrDefaultAsync();


            var employeeList = (from p in planning
                                from pe in planningEmp.Where(x => x.PLANNING_ID == p.ID)
                                from e in employee.Where(x => x.ID == pe.EMPLOYEE_ID)
                                from pos in position.Where(x => x.ID == e.POSITION_ID).DefaultIfEmpty()
                                from o in organization.Where(x => x.ID == e.ORG_ID).DefaultIfEmpty()
                                from p1 in otherList.Where(x => x.ID == pe.PLANNING_TITLE_ID).DefaultIfEmpty()
                                from p2 in otherList.Where(x => x.ID == pe.PLANNING_TYPE_ID).DefaultIfEmpty()
                                from w in otherList.Where(x => x.ID == e.WORK_STATUS_ID).DefaultIfEmpty()
                                where pe.PLANNING_ID == id
                                select new EmployeeDTO()
                                {
                                    Id = e.ID,
                                    Fullname = e.Profile!.FULL_NAME,
                                    Code = e.CODE,
                                    PositionName = pos.NAME,
                                    OrgName = o.NAME,
                                    PlanningTitleName = p1.NAME,
                                    PlanningTitleId = pe.PLANNING_TITLE_ID,
                                    PlanningTypeId = pe.PLANNING_TYPE_ID,
                                    PlanningTypeName = p2.NAME,
                                    WorkStatusId = e.WORK_STATUS_ID,
                                    WorkStatusName = w.NAME
                                }).ToList();
            if (joined != null)
            {
                joined?.EmployeeList.AddRange(employeeList);
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

        public async Task<FormatedResponse> Create(GenericUnitOfWork _uow, HuPlanningDTO dto, string sid)
        {
            //check ban ghi nv co dot quy hoach + gd tu-den > 3: loi
            //var _ck = await _dbContext.HuPlannings.AsNoTracking().Where(x => x.ID == dto.Id && x.PLANNING_PERIOD_ID == dto.PlanningPeriodId && x.FROM_YEAR_ID == dto.FromYearId && x.TO_YEAR_ID == dto.ToYearId).FirstOrDefaultAsync();
            //if (dto.EmployeeIds != null && _ck != null)
            //{
            //    foreach (var item in dto.EmployeeIds)
            //    {
            //        var _ckpos = await _dbContext.HuPlanningEmps.Where(x => x.EMPLOYEE_ID == item && x.PLANNING_ID == _ck.ID).GroupBy(p => p.PLANNING_TITLE_ID).CountAsync();
            //        if (_ckpos > 3)
            //        {
            //            return new FormatedResponse()
            //            {
            //                ErrorType = EnumErrorType.CATCHABLE,
            //                StatusCode = EnumStatusCode.StatusCode400,
            //                MessageCode = "EMPLOYEES_EXCEEDING_THREE_DESIGNATED_POSITIONS"//1 nv co nhieu hon 3 chuc danh quy hoach
            //            };
            //        }

            //        var _ckemp = await _dbContext.HuPlanningEmps.AsNoTracking().Where(x => x.EMPLOYEE_ID == item && x.PLANNING_ID == _ck.ID).CountAsync();
            //        if (_ckemp > 3)
            //        {
            //            return new FormatedResponse()
            //            {
            //                ErrorType = EnumErrorType.CATCHABLE,
            //                StatusCode = EnumStatusCode.StatusCode400,
            //                MessageCode = "THE_EMPLOYEE_HAS_MORE_THAN_THREE_RECORDS_IN_THE_PLANNING_ROUND"//nv co nhieu hon 3 ban ghi trong dot quy hoach
            //            };
            //        }
            //    }

            //}

            var response = await _genericRepository.Create(_uow, dto, sid);
            return response;
        }

        public async Task<FormatedResponse> CreateRange(GenericUnitOfWork _uow, List<HuPlanningDTO> dtos, string sid)
        {
            var add = new List<HuPlanningDTO>();
            add.AddRange(dtos);
            var response = await _genericRepository.CreateRange(_uow, add, sid);
            return response;
        }

        public async Task<FormatedResponse> Update(GenericUnitOfWork _uow, HuPlanningDTO dto, string sid, bool patchMode = true)
        {
            //var _ck = await _dbContext.HuPlannings.AsNoTracking().Where(x => x.ID == dto.Id && x.PLANNING_PERIOD_ID == dto.PlanningPeriodId && x.FROM_YEAR_ID == dto.FromYearId && x.TO_YEAR_ID == dto.ToYearId).FirstOrDefaultAsync();
            //if (dto.EmployeeIds != null && _ck != null)
            //{
            //    foreach (var item in dto.EmployeeIds)
            //    {
            //        var _ckpos = await _dbContext.HuPlanningEmps.Where(x => x.EMPLOYEE_ID == item && x.PLANNING_ID == _ck.ID).GroupBy(p => p.PLANNING_TITLE_ID).CountAsync();
            //        if (_ckpos > 3)
            //        {
            //            return new FormatedResponse()
            //            {
            //                ErrorType = EnumErrorType.CATCHABLE,
            //                StatusCode = EnumStatusCode.StatusCode400,
            //                MessageCode = "EMPLOYEES_EXCEEDING_THREE_DESIGNATED_POSITIONS"//1 nv co nhieu hon 3 chuc danh quy hoach
            //            };
            //        }

            //        var _ckemp = await _dbContext.HuPlanningEmps.AsNoTracking().Where(x => x.EMPLOYEE_ID == item && x.PLANNING_ID == _ck.ID).CountAsync();
            //        if (_ckemp >= 4)
            //        {
            //            return new FormatedResponse()
            //            {
            //                ErrorType = EnumErrorType.CATCHABLE,
            //                StatusCode = EnumStatusCode.StatusCode400,
            //                MessageCode = "THE_EMPLOYEE_HAS_MORE_THAN_THREE_RECORDS_IN_THE_PLANNING_ROUND"
            //            };
            //        }
            //    }

            //}
            var response = await _genericRepository.Update(_uow, dto, sid, patchMode);
            return response;
        }

        public async Task<FormatedResponse> UpdateRange(GenericUnitOfWork _uow, List<HuPlanningDTO> dtos, string sid, bool patchMode = true)
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
            ids.ForEach(item =>
            {
                var getPlanEmp = _uow.Context.Set<HU_PLANNING_EMP>().Where(x => x.ID == item).FirstOrDefault();
                _dbContext.HuPlanningEmps.RemoveRange(_dbContext.HuPlanningEmps.Where(x => x.ID == item));
            });
            var response = await _genericRepository.DeleteIds(_uow, ids);
            return response;
        }

        public async Task<FormatedResponse> DeleteIds(GenericUnitOfWork _uow, List<string> ids)
        {
            await Task.Run(() => null);
            throw new NotImplementedException();
        }

        public Task<FormatedResponse> ToggleActiveIds(GenericUnitOfWork _uow, List<long> ids, bool valueToBind, string sid)
        {
            throw new NotImplementedException();
        }

        public async Task<FormatedResponse> GetCertificateByEmp(long emp)
        {
            var lvTrainId = await (from c in _dbContext.HuCertificates.AsNoTracking().Where(x => x.EMPLOYEE_ID == emp && x.IS_PRIME == true)
                                  orderby c.EFFECT_FROM descending select c.LEVEL_TRAIN).FirstOrDefaultAsync();
            if (lvTrainId != null)
            {
                var response = await (from res in _dbContext.SysOtherLists.Where(x => x.ID == lvTrainId)
                                      select new
                                      {
                                          Id = res.ID,
                                          Name = res.NAME
                                      }).FirstAsync();
                return new FormatedResponse() { InnerBody = response };
            }
            else
            {
                return new FormatedResponse() { InnerBody = null };
            }
        }
    }
}

