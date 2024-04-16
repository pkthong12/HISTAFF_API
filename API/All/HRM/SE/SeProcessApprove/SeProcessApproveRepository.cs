using CORE.GenericUOW;
using CORE.DTO;
using API.DTO;
using CORE.Enum;
using CORE.StaticConstant;
using API.All.DbContexts;
using CORE.AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using ProfileDAL.ViewModels;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;

namespace API.Controllers.SeProcessApprove
{
    public class SeProcessApproveRepository : ISeProcessApproveRepository
    {
        private readonly GenericUnitOfWork _uow;
        private readonly FullDbContext _dbContext;
        private IGenericRepository<SE_PROCESS_APPROVE, SeProcessApproveDTO> _genericRepository;
        private readonly GenericReducer<SE_PROCESS_APPROVE, SeProcessApproveDTO> _genericReducer;

        public SeProcessApproveRepository(FullDbContext context, GenericUnitOfWork uow)
        {
            _dbContext = context;
            _uow = uow;
            _genericRepository = _uow.GenericRepository<SE_PROCESS_APPROVE, SeProcessApproveDTO>();
            _genericReducer = new();
        }

        public async Task<GenericPhaseTwoListResponse<SeProcessApproveDTO>> SinglePhaseQueryList(GenericQueryListDTO<SeProcessApproveDTO> request)
        {
            var joined = from p in _dbContext.SeProcessApproves.AsNoTracking()
                             // JOIN OTHER ENTITIES BASED ON THE BUSINESS
                         from pr in _dbContext.SeProcesss.AsNoTracking().Where(x => x.ID == p.PROCESS_ID).DefaultIfEmpty()//quy trinh
                         select new SeProcessApproveDTO
                         {
                             Id = p.ID,
                             ApprovalLevelName = p.APPROVAL_LEVEL_NAME,
                             LevelOrderId = p.LEVEL_ORDER_ID,
                             ApprovalPosition = p.APPROVAL_POSITION,
                             SameApprover = p.SAME_APPROVER,
                             ProcessId = p.PROCESS_ID,
                             ProcessName = pr.NAME,
                             DirectManager = p.DIRECT_MANAGER,
                             ManagerAffiliatedDepartments = p.MANAGER_AFFILIATED_DEPARTMENTS,
                             ManagerSuperiorDepartments = p.MANAGER_SUPERIOR_DEPARTMENTS,
                             Approve = p.APPROVE,
                             Refuse = p.REFUSE,
                             ChooseAnApprover = p.CHOOSE_AN_APPROVER,
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
            var processApprove = _uow.Context.Set<SE_PROCESS_APPROVE>().AsNoTracking().AsQueryable();
            var processApprovePos = _uow.Context.Set<SE_PROCESS_APPROVE_POS>().AsNoTracking().AsQueryable();
            var process = _uow.Context.Set<SE_PROCESS>().AsNoTracking().AsQueryable();
            var position = _uow.Context.Set<HU_POSITION>().AsNoTracking().AsQueryable();
            var res = await _genericRepository.GetById(id);
            if (res.InnerBody != null)
            {
                var response = res.InnerBody;
                var list = new List<SE_PROCESS_APPROVE>
                    {
                        (SE_PROCESS_APPROVE)response
                    };
                var joined = (from p in list
                                  // JOIN OTHER ENTITIES BASED ON THE BUSINESS
                              select new
                              {
                                  Id = p.ID,
                                  ApprovalLevelName = p.APPROVAL_LEVEL_NAME,
                                  LevelOrderId = p.LEVEL_ORDER_ID,
                                  ApprovalPosition = p.APPROVAL_POSITION,
                                  SameApprover = p.SAME_APPROVER,
                                  ProcessId = p.PROCESS_ID,
                                  DirectManager = p.DIRECT_MANAGER,
                                  ManagerAffiliatedDepartments = p.MANAGER_AFFILIATED_DEPARTMENTS,
                                  ManagerSuperiorDepartments = p.MANAGER_SUPERIOR_DEPARTMENTS,
                                  Approve = p.APPROVE,
                                  Refuse = p.REFUSE,
                                  ChooseAnApprover = p.CHOOSE_AN_APPROVER,
                                  CheckList = new List<long>(),
                                  ListCheck = (p.DIRECT_MANAGER!.Value == true ? 1 : (p.MANAGER_AFFILIATED_DEPARTMENTS!.Value == true ? 2 : (p.MANAGER_SUPERIOR_DEPARTMENTS!.Value == true ? 3 : 0))),
                                  PosList = new List<HuPositionDTO>(),
                                  PosIds = (from pa in processApprovePos.Where(x => x.PROCESS_APPROVE_ID == id) select pa.POS_ID).ToList(),
                              }).FirstOrDefault();

                var posList = (from pa in processApprove
                               from pap in processApprovePos.Where(x => x.PROCESS_APPROVE_ID == pa.ID)
                               from p in position.Where(x => x.ID == pap.POS_ID)
                               from o in _dbContext.HuOrganizations.Where(x => x.ID == p.ORG_ID).DefaultIfEmpty()
                               from g in _dbContext.HuCompanys.Where(x => x.ID == o.COMPANY_ID).DefaultIfEmpty()
                               from v in _dbContext.SysOtherLists.Where(x => x.ID == g.REGION_ID).DefaultIfEmpty()
                               from e in _dbContext.HuEmployees.Where(x => x.POSITION_ID == p.ID).DefaultIfEmpty()
                               from m in _dbContext.HuEmployees.Where(x => x.ID == p.MASTER).DefaultIfEmpty()
                               from i in _dbContext.HuEmployees.Where(x => x.ID == p.INTERIM).DefaultIfEmpty()
                               from tt in _dbContext.HuPositions.Where(x => x.ID == p.LM).DefaultIfEmpty()
                               from tte in _dbContext.HuEmployees.Where(x => x.ID == tt.MASTER).DefaultIfEmpty()
                               from gt in _dbContext.HuPositions.Where(x => x.ID == p.CSM).DefaultIfEmpty()
                               from gte in _dbContext.HuEmployees.Where(x => x.ID == gt.MASTER).DefaultIfEmpty()
                               from ttj in _dbContext.HuJobs.Where(x => x.ID == tt.JOB_ID).DefaultIfEmpty()
                               from j in _dbContext.HuJobs.Where(x => x.ID == p.JOB_ID).DefaultIfEmpty()
                               from gtj in _dbContext.HuJobs.Where(x => x.ID == gt.JOB_ID).DefaultIfEmpty()
                               where pap.PROCESS_APPROVE_ID == id
                               select new HuPositionDTO()
                               {
                                   Id = p.ID,
                                   Code = p.CODE,
                                   Name = p.NAME,
                                   EffectiveDate = p.EFFECTIVE_DATE,
                                   OrgName = o.NAME,
                                   MasterName = m.CODE + " - " + m.Profile.FULL_NAME,
                                   InterimName = i.CODE + " - " + i.Profile.FULL_NAME,
                                   LmName = tte.Profile.FULL_NAME,
                                   EmpLmName = tte.CODE + " - " + tte.Profile.FULL_NAME,
                                   LmJobName = tt.CODE + " - " + ttj.NAME_VN,
                                   CsmName = gte.Profile.FULL_NAME,
                                   CsmJobName = gt.CODE + " - " + gtj.NAME_VN,
                                   JobName = j.CODE + " - " + j.NAME_VN,
                                   JobId = p.JOB_ID,
                                   Lm = p.LM,
                                   Interim = p.INTERIM,
                                   Master = p.MASTER,
                                   OrgId = p.ORG_ID,
                                   IsActive = p.IS_ACTIVE,
                                   JobDesc = p.JOB_DESC,
                                   IsTdv = p.IS_TDV,
                                   IsNotot = p.IS_NOTOT,

                               }).ToList();

                joined?.PosList.AddRange(posList);
                if (joined != null)
                {
                    if(joined.ApprovalPosition.HasValue != null && joined.ApprovalPosition == true)
                    {
                        joined.CheckList.Add(1);
                    }
                    if(joined.SameApprover.HasValue != null && joined.SameApprover == true)
                    {
                        joined.CheckList.Add(2);
                    }
                }

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

        public async Task<FormatedResponse> Create(GenericUnitOfWork _uow, SeProcessApproveDTO dto, string sid)
        {
            var response = await _genericRepository.Create(_uow, dto, sid);
            return response;
        }

        public async Task<FormatedResponse> CreateRange(GenericUnitOfWork _uow, List<SeProcessApproveDTO> dtos, string sid)
        {
            var add = new List<SeProcessApproveDTO>();
            add.AddRange(dtos);
            var response = await _genericRepository.CreateRange(_uow, add, sid);
            return response;
        }

        public async Task<FormatedResponse> Update(GenericUnitOfWork _uow, SeProcessApproveDTO dto, string sid, bool patchMode = true)
        {
            var response = await _genericRepository.Update(_uow, dto, sid, patchMode);
            return response;
        }

        public async Task<FormatedResponse> UpdateRange(GenericUnitOfWork _uow, List<SeProcessApproveDTO> dtos, string sid, bool patchMode = true)
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

        public async Task<FormatedResponse> GetLevelOrder()
        {//lay thu tu cap
            var query = await (from p in _dbContext.SysOtherLists.AsNoTracking().Where(p => p.TYPE_ID == 103)
                               select new
                               {
                                   LevelOrderId = p.ID,
                                   LevelOrderName = p.NAME,
                               }).ToListAsync();
            return new FormatedResponse() { InnerBody = query };
        }

        public async Task<FormatedResponse> GetListProcess()
        {//lay ds quy trinh
            var query = await (from p in _dbContext.SeProcesss.AsNoTracking()
                               select new
                               {
                                   Id = p.ID,
                                   Name = p.NAME,
                               }).ToListAsync();
            return new FormatedResponse() { InnerBody = query };
        }

        public async Task<FormatedResponse> GetListByProcess(long processId)
        {//lay list phe duyet -> quy trinh
            var query = from p in _dbContext.SeProcessApproves.AsNoTracking().Where(p => p.PROCESS_ID == processId).DefaultIfEmpty()
                            // JOIN OTHER ENTITIES BASED ON THE BUSINESS
                        from pr in _dbContext.SeProcesss.AsNoTracking().Where(x => x.ID == p.PROCESS_ID).DefaultIfEmpty()//quy trinh
                        select new SeProcessApproveDTO
                        {
                            Id = p.ID,
                            ApprovalLevelName = p.APPROVAL_LEVEL_NAME,
                            LevelOrderId = p.LEVEL_ORDER_ID,
                            ApprovalPosition = p.APPROVAL_POSITION,
                            SameApprover = p.SAME_APPROVER,
                            ProcessId = p.PROCESS_ID,
                            ProcessName = pr.NAME,
                            DirectManager = p.DIRECT_MANAGER,
                            ManagerAffiliatedDepartments = p.MANAGER_AFFILIATED_DEPARTMENTS,
                            ManagerSuperiorDepartments = p.MANAGER_SUPERIOR_DEPARTMENTS,
                            Approve = p.APPROVE,
                            Refuse = p.REFUSE,
                            ChooseAnApprover = p.CHOOSE_AN_APPROVER,
                        };
            return new FormatedResponse() { InnerBody = query };
        }

        public virtual async Task<FormatedResponse> ToggleActiveIds(GenericUnitOfWork _uow, List<long> ids, bool valueToBind, string sid)
        {
            await Task.Run(() => null);
            throw new NotImplementedException();
        }
    }
}

