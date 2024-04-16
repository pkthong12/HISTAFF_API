using Common.Paging;
using ProfileDAL.ViewModels;
using Common.Repositories;
using Common.Extensions;
using Common.EPPlus;
using System.Data;
using API.All.DbContexts;
using API.Entities;
using CORE.GenericUOW;
using CORE.DTO;
using CORE.Enum;
using CORE.StaticConstant;
using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;
using API.DTO;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.Metrics;
using RegisterServicesWithReflection.Services.Base;

namespace ProfileDAL.Repositories
{
    [ScopedRegistration]
    public class WorkingRepository : RepositoryBase<HU_WORKING>, IWorkingRepository
    {
        private ProfileDbContext _appContext => (ProfileDbContext)_context;
        private readonly GenericReducer<HU_WORKING, HuWorkingDTO> genericReducer;
        public WorkingRepository(ProfileDbContext context) : base(context)
        {
            genericReducer = new();
        }
        //DECISION - IS_WAGE == 0
        public async Task<GenericPhaseTwoListResponse<HuWorkingDTO>> TwoPhaseQueryList(GenericQueryListDTO<HuWorkingDTO> request)
        {
            //var raw = _appContext.Workings.Where(f => f.IS_WAGE == 0 || f.IS_WAGE == null).AsQueryable();
            //var phase1 = await genericReducer.FirstPhaseReduce(raw, request);

            //if (phase1.ErrorType != EnumErrorType.NONE)
            //{
            //    return new()
            //    {
            //        ErrorType = phase1.ErrorType,
            //        MessageCode = phase1.MessageCode,
            //        ErrorPhase = 1
            //    };
            //}

            //if (phase1.Queryable == null)
            //{
            //    return new()
            //    {
            //        ErrorType = EnumErrorType.CATCHABLE,
            //        MessageCode = CommonMessageCode.ENTITIES_NOT_FOUND,
            //        ErrorPhase = 1
            //    };
            //}

            //var phase1IdsResult = phase1.Queryable.ToList().Aggregate("", (prev, curr) => prev + curr.ID.ToString() + ";");
            //var ids = phase1IdsResult.Split(';');

            var joined = from p in _appContext.Workings
                         join e in _appContext.Employees on p.EMPLOYEE_ID equals e.ID
                         join t in _appContext.Positions on p.POSITION_ID equals t.ID
                         join o in _appContext.Organizations on p.ORG_ID equals o.ID
                         from f in _appContext.OtherLists.Where(c => c.ID == p.STATUS_ID).DefaultIfEmpty()
                         join l in _appContext.OtherLists on p.TYPE_ID equals l.ID
                         from s in _appContext.Employees.Where(c => c.ID == p.SIGN_ID).DefaultIfEmpty()
                         from eo in _appContext.OtherLists.Where(f => f.ID == p.EMPLOYEE_OBJ_ID).DefaultIfEmpty()
                         from co in _appContext.CompanyInfos.Where(f => f.ID == o.COMPANY_ID).DefaultIfEmpty()
                         orderby p.STATUS_ID, p.EFFECT_DATE descending
                         where p.IS_WAGE == 0 || p.IS_WAGE == null
                         select new HuWorkingDTO
                         {
                             Id = p.ID,
                             EmployeeCode = e.CODE,
                             EmployeeId = e.ID,
                             EmployeeName = e.Profile!.FULL_NAME,
                             PositionName = t.NAME,
                             SignDate = p.SIGN_DATE,
                             SignerName = p.SIGNER_NAME,
                             SignerCode = s.CODE,
                             SignerPosition = p.SIGNER_POSITION,
                             OrgId = p.ORG_ID,
                             OrgName = o.NAME,
                             EffectDate = p.EFFECT_DATE,
                             ExpireDate = p.EXPIRE_DATE,
                             DecisionNo = p.DECISION_NO,
                             StatusName = f.NAME,
                             TypeName = l.NAME,
                             EmpWorkStatus = e.WORK_STATUS_ID,
                             WorkPlaceName = co.WORK_ADDRESS,
                             EmployeeObjName = eo.NAME,
                             EmployeeObjectId = eo.ID,
                             WageId = p.WAGE_ID,
                         };
            //request.Sort = new List<SortItem>();
            //request.Sort.Add(new SortItem() { Field = "Id", SortDirection = EnumSortDirection.DESC });
            var phase2 = await genericReducer.SinglePhaseReduce(joined, request);
            return phase2;
        }
        /// <summary>
        /// CMS Get All Data
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<PagedResult<WorkingDTO>> GetAll(WorkingDTO param)
        {
            var queryable = from p in _appContext.Workings
                            join e in _appContext.Employees on p.EMPLOYEE_ID equals e.ID
                            join t in _appContext.Positions on e.POSITION_ID equals t.ID
                            join o in _appContext.Organizations on e.ORG_ID equals o.ID
                            from f in _appContext.OtherListFixs.Where(c => c.ID == p.STATUS_ID && c.TYPE == SystemConfig.STATUS_APPROVE).DefaultIfEmpty()
                            join l in _appContext.OtherLists on p.TYPE_ID equals l.ID
                            from s in _appContext.Employees.Where(c => c.ID == p.SIGN_ID).DefaultIfEmpty()
                            join tl in _appContext.SalaryTypes on p.SALARY_TYPE_ID equals tl.ID
                            orderby p.STATUS_ID, p.EFFECT_DATE descending
                            where p.IS_WAGE == 0 || p.IS_WAGE == null
                            select new WorkingDTO
                            {
                                Id = p.ID,
                                EmployeeId = p.EMPLOYEE_ID,
                                EmployeeCode = e.CODE,
                                EmployeeName = e.Profile!.FULL_NAME,
                                PositionName = t.NAME,
                                SignDate = p.SIGN_DATE,
                                SignerName = p.SIGNER_NAME,
                                SignerCode = s.CODE,
                                SignerPosition = p.SIGNER_POSITION,
                                OrgName = o.NAME,
                                OrgId = p.ORG_ID,
                                EffectDate = p.EFFECT_DATE,
                                DecisionNo = p.DECISION_NO,
                                StatusId = p.STATUS_ID,
                                StatusName = f.NAME,
                                TypeId = p.TYPE_ID,
                                TypeName = l.NAME,
                                WorkStatusId = e.WORK_STATUS_ID,
                                SalBasic = p.SAL_BASIC,
                                SalTotal = p.SAL_TOTAL,
                                SalPercent = p.SAL_PERCENT,
                                SalaryType = tl.NAME
                            };

            var orgIds = await QueryData.ExecuteList(Procedures.PKG_COMMON_LIST_ORG,
                    new
                    {
                        P_IS_ADMIN = _appContext.IsAdmin == true ? 1 : 0,
                        P_ORG_ID = param.OrgId,
                        P_CURENT_USER_ID = _appContext.CurrentUserId,
                        P_CUR = QueryData.OUT_CURSOR
                    }, false);


            List<long?> ids = orgIds.Select(c => (long?)((dynamic)c).ID).ToList();
            if (param.OrgId != null)
            {
                ids.Add(param.OrgId);
            }
            queryable = queryable.Where(p => ids.Contains(p.OrgId));
            if (!string.IsNullOrWhiteSpace(param.StatusName))
            {
                queryable = queryable.Where(p => p.StatusName.ToUpper().Contains(param.StatusName.ToUpper()));
            }

            if (!string.IsNullOrWhiteSpace(param.EmployeeCode))
            {
                queryable = queryable.Where(p => p.EmployeeCode.ToUpper().Contains(param.EmployeeCode.ToUpper()));
            }
            if (!string.IsNullOrWhiteSpace(param.EmployeeName))
            {
                queryable = queryable.Where(p => p.EmployeeName.ToUpper().Contains(param.EmployeeName.ToUpper()));
            }
            if (!string.IsNullOrWhiteSpace(param.PositionName))
            {
                queryable = queryable.Where(p => p.PositionName.ToUpper().Contains(param.PositionName.ToUpper()));
            }
            if (!string.IsNullOrWhiteSpace(param.OrgName))
            {
                queryable = queryable.Where(p => p.OrgName.ToUpper().Contains(param.OrgName.ToUpper()));
            }
            if (!string.IsNullOrWhiteSpace(param.DecisionNo))
            {
                queryable = queryable.Where(p => p.DecisionNo.ToUpper().Contains(param.DecisionNo.ToUpper()));
            }
            if (!string.IsNullOrWhiteSpace(param.StatusName))
            {
                queryable = queryable.Where(p => p.StatusName.ToUpper().Contains(param.StatusName.ToUpper()));
            }
            if (!string.IsNullOrWhiteSpace(param.TypeName))
            {
                queryable = queryable.Where(p => p.TypeName.ToUpper().Contains(param.TypeName.ToUpper()));
            }

            if (param.EffectDate != null)
            {
                queryable = queryable.Where(p => p.EffectDate == param.EffectDate);
            }
            if (param.WorkStatusId == null || param.WorkStatusId == 0)
            {
                queryable = queryable.Where(p => p.WorkStatusId != OtherConfig.EMP_STATUS_TERMINATE);
            }

            return await PagingList(queryable, param);
        }
        public async Task<PagedResult<WorkingDTO>> GetWorking(WorkingDTO param)
        {
            var queryable = from p in _appContext.Workings
                            join e in _appContext.Employees on p.EMPLOYEE_ID equals e.ID
                            join f in _appContext.OtherListFixs.Where(c => c.TYPE == SystemConfig.STATUS_APPROVE && c.ID == 2) on p.STATUS_ID equals f.ID
                            orderby p.EFFECT_DATE descending
                            where p.EMPLOYEE_ID == param.EmployeeId && (p.IS_WAGE == 0 || p.IS_WAGE == null)
                            select new WorkingDTO
                            {
                                Id = p.ID,
                                EmployeeId = p.EMPLOYEE_ID,
                                EmployeeCode = e.CODE,
                                EmployeeName = e.Profile.FULL_NAME,
                                SignDate = p.SIGN_DATE,
                                SignerName = p.SIGNER_NAME,
                                SignerPosition = p.SIGNER_POSITION,
                                OrgId = p.ORG_ID,
                                EffectDate = p.EFFECT_DATE,
                                DecisionNo = p.DECISION_NO,
                                StatusName = f.NAME,
                                StatusId = p.STATUS_ID,
                                TypeId = p.TYPE_ID,
                                Note = p.NOTE
                            };



            if (!string.IsNullOrWhiteSpace(param.DecisionNo))
            {
                queryable = queryable.Where(p => p.DecisionNo.ToUpper().Contains(param.DecisionNo.ToUpper()));
            }
            if (!string.IsNullOrWhiteSpace(param.StatusName))
            {
                queryable = queryable.Where(p => p.StatusName.ToUpper().Contains(param.StatusName.ToUpper()));
            }




            return await PagingList(queryable, param);
        }
        /// <summary>
        /// CMS Get Detail
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>        
        public async Task<ResultWithError> GetById(long id)
        {
            try
            {
                var r = await (from p in _appContext.Workings
                               from e in _appContext.Employees.Where(c => c.ID == p.EMPLOYEE_ID)
                               from o in _appContext.Organizations.Where(c => c.ID == p.ORG_ID)
                               from o2 in _appContext.Organizations.Where(c => c.ID == o.PARENT_ID).DefaultIfEmpty()
                               from co in _appContext.CompanyInfos.Where(c => c.ID == o.COMPANY_ID).DefaultIfEmpty()
                               where p.ID == id
                               select new WorkingInputDTO
                               {
                                   Id = p.ID,
                                   EmployeeId = p.EMPLOYEE_ID,
                                   EmployeeName = e.Profile.FULL_NAME,
                                   EmployeeCode = e.CODE,
                                   PositionId = p.POSITION_ID,
                                   OrgId = p.ORG_ID,
                                   OrgName = o.NAME,
                                   EffectDate = p.EFFECT_DATE,
                                   ExpireDate = p.EXPIRE_DATE,
                                   DecisionNo = p.DECISION_NO,
                                   TypeId = p.TYPE_ID,
                                   StatusId = p.STATUS_ID,
                                   Note = p.NOTE,
                                   SignId = p.SIGN_ID,
                                   SignerName = p.SIGNER_NAME,
                                   SignerPosition = p.SIGNER_POSITION,
                                   SignDate = p.SIGN_DATE,
                                   Coefficient = p.COEFFICIENT,
                                   EmployeeObjId = p.EMPLOYEE_OBJ_ID,
                                   IsResponsible = p.IS_RESPONSIBLE,
                                   WageId = p.WAGE_ID,
                                   WorkPlaceName = co.WORK_ADDRESS,
                                   Attachment = p.ATTACHMENT,
                                   CreatedDateDecision = p.CREATED_DATE_DECISION,
                                   BaseDate = p.BASE_DATE,
                                   IssuedDate = p.ISSUED_DATE,
                                   ConfirmSwapMasterInterim = 0
                               }).FirstOrDefaultAsync();
                var ConfirmSwapMasterInterim = await checkDecisionMaster(r);
                r.ConfirmSwapMasterInterim = ConfirmSwapMasterInterim.StatusCode == "200" || ConfirmSwapMasterInterim.StatusCode == "204" ? 1 : 0;
                return new ResultWithError(r);
            }
            catch (Exception ex)
            {
                return new ResultWithError(ex.Message);
            }
        }
        /// <summary>
        /// Get last working by employee.lastWorkingId
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>        
        public async Task<ResultWithError> GetLastWorking(long? empId, DateTime? date)
        {
            try
            {
                if (date == null)
                {
                    date = DateTime.Now;
                }
                var r = await (from p in _appContext.Workings
                               from t in _appContext.SalaryTypes.Where(c => c.ID == p.SALARY_TYPE_ID).DefaultIfEmpty()
                               from s in _appContext.SalaryScales.Where(c => c.ID == p.SALARY_SCALE_ID).DefaultIfEmpty()
                               from ra in _appContext.SalaryRanks.Where(c => c.ID == p.SALARY_RANK_ID).DefaultIfEmpty()
                               from l in _appContext.SalaryLevels.Where(c => c.ID == p.SALARY_LEVEL_ID).DefaultIfEmpty()
                               from dt in _appContext.OtherLists.Where(c => c.ID == p.TYPE_ID).DefaultIfEmpty()
                               where p.EMPLOYEE_ID == empId && p.EFFECT_DATE <= date && (p.IS_WAGE == 0 || p.IS_WAGE == null) && p.STATUS_ID == 994
                               orderby p.EFFECT_DATE descending
                               select new WorkingInputDTO
                               {
                                   Id = p.ID,
                                   OrgId = p.ORG_ID,
                                   PositionId = p.POSITION_ID,
                                   StatusId = p.STATUS_ID,
                                   DecisionNo = p.DECISION_NO,
                                   SalaryTypeId = p.SALARY_TYPE_ID,
                                   SalaryTypeName = t.NAME,
                                   SalaryScaleId = p.SALARY_SCALE_ID,
                                   SalaryScaleName = s.NAME,
                                   SalaryRankId = p.SALARY_RANK_ID,
                                   SalaryRankName = ra.NAME,
                                   SalaryLevelId = p.SALARY_LEVEL_ID,
                                   SalaryLevelName = l.NAME,
                                   SalBasic = p.SAL_BASIC,
                                   SalTotal = p.SAL_TOTAL,
                                   SalPercent = p.SAL_PERCENT,
                                   SignId = p.SIGN_ID,
                                   SignerName = p.SIGNER_NAME,
                                   SignerPosition = p.SIGNER_POSITION,
                                   SignDate = p.SIGN_DATE,
                                   Note = p.NOTE,
                                   TypeCode = dt.CODE,
                               }).FirstOrDefaultAsync();
                return new ResultWithError(r);
            }
            catch (Exception ex)
            {
                return new ResultWithError(ex.Message);
            }
        }
        /// <summary>
        /// get working all Employee
        /// </summary>
        /// <param name="empid"></param>
        /// <param name="date"></param>
        /// <returns></returns>        
        public async Task<ResultWithError> GetWorkingOld(long? empId, long? id)
        {
            try
            {
                DateTime? date = new DateTime();
                var itemCur = (from p in _appContext.Workings where p.ID == id select p).FirstOrDefault();
                var empExist = new HU_WORKING();
                if (id > 0)
                {
                    date = itemCur.EFFECT_DATE;
                }
                else
                {
                    id = 0;
                    date = DateTime.Now;
                }
                empExist = (from p in _appContext.Workings
                            where p.EMPLOYEE_ID == empId && p.EFFECT_DATE <= date && (p.IS_WAGE == 0 || p.IS_WAGE == null)
                            && p.STATUS_ID == 994 && p.ID != id && p.IS_RESPONSIBLE == true
                            select p).OrderByDescending(x => x.EFFECT_DATE).Take(1).FirstOrDefault();
                var r = new WorkingInputDTO();
                if (empExist != null)
                {
                    r = await (from p in _appContext.Workings
                               from e in _appContext.Employees.Where(c => c.ID == p.EMPLOYEE_ID)
                               from o in _appContext.Organizations.Where(c => c.ID == p.ORG_ID)
                               from o2 in _appContext.Organizations.Where(c => c.ID == o.PARENT_ID).DefaultIfEmpty()
                               from co in _appContext.CompanyInfos.Where(c => c.ID == o.COMPANY_ID).DefaultIfEmpty()
                               from ty in _appContext.OtherLists.Where(c => c.ID == p.TYPE_ID).DefaultIfEmpty()
                               from t in _appContext.Positions.Where(c => c.ID == p.POSITION_ID).DefaultIfEmpty()
                               from eo in _appContext.OtherLists.Where(c => c.ID == p.EMPLOYEE_OBJ_ID).DefaultIfEmpty()
                               where p.ID == empExist.ID
                               select new WorkingInputDTO
                               {
                                   EmployeeId = p.EMPLOYEE_ID,
                                   EmployeeName = e.Profile.FULL_NAME,
                                   EmployeeCode = e.CODE,
                                   PositionId = p.POSITION_ID,
                                   PositionName = t.NAME,
                                   OrgId = p.ORG_ID,
                                   OrgName = o.NAME,
                                   EffectDate = p.EFFECT_DATE,
                                   ExpireDate = p.EXPIRE_DATE,
                                   DecisionNo = p.DECISION_NO,
                                   TypeId = p.TYPE_ID,
                                   TypeName = ty.NAME,
                                   StatusId = p.STATUS_ID,
                                   EmployeeObjId = p.EMPLOYEE_OBJ_ID,
                                   EmployeeObjName = eo.NAME,
                                   IsResponsible = p.IS_RESPONSIBLE,
                                   IsResponsibleSalary = p.IS_RESPONSIBLE_SALARY,
                                   WorkPlaceName = co.WORK_ADDRESS,
                               }).FirstOrDefaultAsync();
                }
                else
                {
                    r = await (from e in _appContext.Employees
                               from o in _appContext.Organizations.Where(c => c.ID == e.ORG_ID)
                               from o2 in _appContext.Organizations.Where(c => c.ID == o.PARENT_ID).DefaultIfEmpty()
                               from co in _appContext.CompanyInfos.Where(c => c.ID == o.COMPANY_ID).DefaultIfEmpty()
                               from t in _appContext.Positions.Where(c => c.ID == e.POSITION_ID).DefaultIfEmpty()
                               where e.ID == empId
                               select new WorkingInputDTO
                               {
                                   EmployeeId = e.ID,
                                   EmployeeName = e.Profile.FULL_NAME,
                                   EmployeeCode = e.CODE,
                                   PositionId = e.POSITION_ID,
                                   PositionName = t.CODE + " - " + t.NAME,
                                   OrgId = e.ORG_ID,
                                   OrgName = o.NAME,
                                   WorkPlaceName = co.WORK_ADDRESS,
                               }).FirstOrDefaultAsync();
                }
                return new ResultWithError(r);
            }
            catch (Exception ex)
            {
                return new ResultWithError(ex.Message);
            }
        }
        /// <summary>
        /// Create Data
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<ResultWithError> CreateAsync(WorkingInputDTO param)
        {
            try
            {

                if (param.ConfirmSwapMasterInterim == 0)
                {
                    return new ResultWithError(Message.MASTER_DONOT_DECISION_SWAP);
                }
                // Kiểm tra xem có QĐ nào có ngày hiệu lực lớn hơn ngày QĐ đang làm (chỉ tính các quyết định đã phê duyệt)
                var r = await _appContext.Workings.Where(x => x.EMPLOYEE_ID == param.EmployeeId && x.STATUS_ID == OtherConfig.STATUS_APPROVE && (x.IS_WAGE == 0 || x.IS_WAGE == null)).OrderByDescending(f => f.EFFECT_DATE).FirstOrDefaultAsync();
                if (r != null && param.IsResponsible == true)
                {
                    if (r.EFFECT_DATE > param.EffectDate)
                    {
                        return new ResultWithError("EFFECTDATE_NOT_LESS_CURRENT");
                    }
                    //Check thay đổi lương
                    if (r.SAL_BASIC != param.SalBasic || r.SAL_TOTAL != param.SalTotal || r.SAL_PERCENT != param.SalPercent)
                    {
                        r.IS_CHANGE_SAL = true;
                    }
                }


                // Gencode
                //var DecisionCode = "";
                var data = Map(param, new HU_WORKING());
                //data.DECISION_NO = DecisionCode;
                await _appContext.Database.BeginTransactionAsync();
                var result = await _appContext.Workings.AddAsync(data);
                await _appContext.SaveChangesAsync();
                if (data.STATUS_ID == OtherConfig.STATUS_APPROVE)
                {
                    var e = _appContext.Employees.Where(x => x.ID == param.EmployeeId).FirstOrDefault();
                    if (e == null)
                    {
                        return new ResultWithError("EMPLOYEE_NOT_FOUND");
                    }
                    await ApproveWorking(data);
                }
                await _appContext.SaveChangesAsync();

                _appContext.Database.CommitTransaction();
                param.Id = data.ID;
                return new ResultWithError(param);
            }
            catch (Exception ex)
            {
                _appContext.Database.RollbackTransaction();
                return new ResultWithError(ex);
            }
        }

        public async Task<bool> ApproveListWorking(string sid)
        {
            var query = (from p in _appContext.Workings
                         from e in _appContext.Employees.Where(f => f.ID == p.EMPLOYEE_ID)
                         where (p.IS_WAGE == 0 || p.IS_WAGE == null) && p.STATUS_ID == OtherConfig.STATUS_APPROVE && p.EFFECT_DATE.Value.Date == DateTime.Now.Date && (e.TER_EFFECT_DATE == null || (e.TER_EFFECT_DATE != null && e.TER_EFFECT_DATE <= DateTime.Now.Date)) && (e.LAST_WORKING_ID == null || (e.LAST_WORKING_ID != null && e.LAST_WORKING_ID != p.ID))
                         select p).ToList();

            var workings = new List<HU_WORKING>();
            workings = query.GroupBy(g => g.EMPLOYEE_ID).Select(p => p.OrderByDescending(d => d.EFFECT_DATE).First()).ToList();

            foreach (var p in workings)
            {
                await ApproveWorking(p);
            }
            return true;
        }

        public void ScanApproveWorkings()
        {
            var scanList = _appContext.Workings.Where(
                x => x.STATUS_ID == OtherConfig.STATUS_APPROVE && x.EFFECT_DATE!.Value.Date == DateTime.Now.Date && (x.IS_WAGE == 0 || x.IS_WAGE == null) && x.IS_RESPONSIBLE == true).ToList();
            scanList.ForEach(async obj =>
            {
                await ApproveWorking(obj);
            });

        }

        public async Task<bool> ApproveWorking(HU_WORKING obj)
        {

            if (obj.EFFECT_DATE.Value.Date > DateTime.Now.Date || obj.STATUS_ID != OtherConfig.STATUS_APPROVE)
            {
                return true;
            }
            var e = (from p in _appContext.Employees where p.ID == obj.EMPLOYEE_ID select p).FirstOrDefault();
            var typeInfor = (from p in _appContext.OtherLists where p.ID == obj.TYPE_ID select p).FirstOrDefault();
            var workStatusEmp = (from p in _appContext.OtherLists
                                 from dt in _appContext.OtherListTypes.Where(f => f.ID == p.TYPE_ID)
                                 where dt.CODE == "EMP_STATUS"
                                 select p).ToList();
            var workStatusEmpDetail = (from p in _appContext.OtherLists
                                       from dt in _appContext.OtherListTypes.Where(f => f.ID == p.TYPE_ID)
                                       where dt.CODE == "STATUS_DETAIL"
                                       select p).ToList();

            var posIdCur = e.POSITION_ID;

            e.ORG_ID = obj.ORG_ID;
            e.POSITION_ID = obj.POSITION_ID;
            e.LAST_WORKING_ID = obj.ID;
            e.EFFECT_DATE = obj.EFFECT_DATE;
            if (e.JOIN_DATE == null)
            {
                e.JOIN_DATE = obj.EFFECT_DATE;
            }
            e.EMPLOYEE_OBJECT_ID = obj.EMPLOYEE_OBJ_ID;

            var positionItemApp = _appContext.Positions.AsNoTracking().Where(x => x.ID == obj.POSITION_ID).FirstOrDefault();
            var master = positionItemApp.MASTER;
            var interim = positionItemApp.INTERIM;

            positionItemApp.MASTER = obj.EMPLOYEE_ID;
            if (master != null)
            {
                var DecisionMaster = await GetLastWorking(master, obj.EFFECT_DATE);
                var DecisionMasterDto = (WorkingInputDTO)DecisionMaster.Data;
                if (DecisionMasterDto == null)
                {
                    if (master == obj.EMPLOYEE_ID)
                    {
                        positionItemApp.INTERIM = null;
                    }
                    else
                    {
                        positionItemApp.INTERIM = master;
                    }
                }
                else
                {
                    if ((DecisionMasterDto.TypeCode.Trim().ToUpper() == "BN" || DecisionMasterDto.TypeCode.Trim().ToUpper() == "DC" || DecisionMasterDto.TypeCode.Trim().ToUpper() == "DDCV" || DecisionMasterDto.TypeCode.Trim().ToUpper() == "DDBN" || DecisionMasterDto.TypeCode.Trim().ToUpper() == "BP") && positionItemApp.INTERIM == master)
                    {
                        positionItemApp.INTERIM = null;
                        positionItemApp.MASTER = null;
                    }
                    else
                    {
                        if (master == obj.EMPLOYEE_ID)
                        {
                            positionItemApp.INTERIM = null;
                        }
                        else
                        {
                            positionItemApp.INTERIM = master;
                        }
                    }
                }
            }
            if (typeInfor != null)
            {
                var workStatusEmp_resault = new SYS_OTHER_LIST();
                var workStatusEmpDetail_resault = new SYS_OTHER_LIST();
                switch (typeInfor.CODE.Trim().ToUpper())
                {

                    case "00024": // tạm hoãn HĐLĐ                                  
                        workStatusEmp_resault = (from p in workStatusEmp where p.CODE == "00023" select p).FirstOrDefault();
                        //Điều động, luân chuyển
                        workStatusEmpDetail_resault = (from p in workStatusEmpDetail where p.CODE == "00006" select p).FirstOrDefault();
                        if (workStatusEmp_resault != null)
                        {
                            e.WORK_STATUS_ID = workStatusEmp_resault.ID;
                        }

                        if (workStatusEmpDetail_resault != null)
                        {
                            e.STATUS_DETAIL_ID = workStatusEmpDetail_resault.ID;
                        }
                        break;
                    case "DC":// Quyết định điều chuyển nhân sự
                        workStatusEmp_resault = (from p in workStatusEmp where p.CODE == "00023" select p).FirstOrDefault();
                        //Điều động, luân chuyển
                        workStatusEmpDetail_resault = (from p in workStatusEmpDetail where p.CODE == "00008" select p).FirstOrDefault();
                        if (workStatusEmp_resault != null)
                        {
                            e.WORK_STATUS_ID = workStatusEmp_resault.ID;
                        }

                        if (workStatusEmpDetail_resault != null)
                        {
                            e.STATUS_DETAIL_ID = workStatusEmpDetail_resault.ID;
                        }
                        break;
                    case "DDCV": //Quyết định về điều động chuyên viên
                        workStatusEmp_resault = (from p in workStatusEmp where p.CODE == "00023" select p).FirstOrDefault();
                        //Điều động, luân chuyển
                        workStatusEmpDetail_resault = (from p in workStatusEmpDetail where p.CODE == "00008" select p).FirstOrDefault();
                        if (workStatusEmp_resault != null)
                        {
                            e.WORK_STATUS_ID = workStatusEmp_resault.ID;
                        }

                        if (workStatusEmpDetail_resault != null)
                        {
                            e.STATUS_DETAIL_ID = workStatusEmpDetail_resault.ID;
                        }
                        break;
                    case "DDBN": //Quyết định về điều động, bổ nhiệm lại cán bộ
                        workStatusEmp_resault = (from p in workStatusEmp where p.CODE == "00023" select p).FirstOrDefault();
                        //Điều động, luân chuyển
                        workStatusEmpDetail_resault = (from p in workStatusEmpDetail where p.CODE == "00008" select p).FirstOrDefault();
                        if (workStatusEmp_resault != null)
                        {
                            e.WORK_STATUS_ID = workStatusEmp_resault.ID;
                        }

                        if (workStatusEmpDetail_resault != null)
                        {
                            e.STATUS_DETAIL_ID = workStatusEmpDetail_resault.ID;
                        }
                        break;
                    case "K":
                        // Trường hợp khác
                        workStatusEmp_resault = (from p in workStatusEmp where p.CODE == "00023" select p).FirstOrDefault();
                        //Trường hợp khác
                        workStatusEmpDetail_resault = (from p in workStatusEmpDetail where p.CODE == "00009" select p).FirstOrDefault();
                        if (workStatusEmp_resault != null)
                        {
                            e.WORK_STATUS_ID = workStatusEmp_resault.ID;
                        }

                        if (workStatusEmpDetail_resault != null)
                        {
                            e.STATUS_DETAIL_ID = workStatusEmpDetail_resault.ID;
                        }

                        break;
                }
            }

            //xoa vi tri khi da dieu chuyen
            var t = _appContext.Positions.AsNoTracking().Where(p => p.ID == posIdCur).FirstOrDefault();
            if ((typeInfor.CODE.Trim().ToUpper() == "BN" || typeInfor.CODE.Trim().ToUpper() == "DC" || typeInfor.CODE.Trim().ToUpper() == "DDCV" || typeInfor.CODE.Trim().ToUpper() == "DDBN" || typeInfor.CODE.Trim().ToUpper() == "BP"))
            {

                if (t.INTERIM == obj.EMPLOYEE_ID)
                {
                    t.INTERIM = null;
                }
                if (t.MASTER == obj.EMPLOYEE_ID)
                {
                    t.MASTER = null;
                }
            }
            List<HU_POSITION> newPos = new List<HU_POSITION>() { positionItemApp };
            if (t.ID != positionItemApp.ID) newPos.Add(t);

            var result = _appContext.Employees.Update(e);
            _appContext.Positions.UpdateRange(newPos);
            //var result2 = _appContext.Positions.Update(positionItemApp);
            var rs = _appContext.SaveChanges();
            return true;
        }

        /// <summary>
        /// CMS Edit Data
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<ResultWithError> UpdateAsync(WorkingInputDTO param)
        {
            try
            {
                var r = _appContext.Workings.Where(x => x.ID == param.Id).FirstOrDefault();

                if (r.STATUS_ID == OtherConfig.STATUS_APPROVE)
                {
                    return new ResultWithError(CommonMessageCode.NOT_UPDATE_BECAUSE_ROW_APPROVED);
                }
                if (param.ConfirmSwapMasterInterim == 0)
                {
                    if (r.EMPLOYEE_ID != param.EmployeeId)
                        return new ResultWithError(Message.MASTER_DONOT_DECISION_SWAP);
                }
                // Kiểm tra xem có QĐ nào có ngày hiệu lực lớn hơn ngày QĐ đang làm (chỉ tính các quyết định đã phê duyệt)
                var c = await _appContext.Workings.Where(x => x.EMPLOYEE_ID == param.EmployeeId && x.ID != param.Id && x.EFFECT_DATE > param.EffectDate && x.STATUS_ID == OtherConfig.STATUS_APPROVE && (x.IS_WAGE == 0 || x.IS_WAGE == null)).CountAsync();
                if (c > 0)
                {
                    return new ResultWithError("EFFECTDATE_NOT_LESS_CURRENT");
                }

                if (r == null)
                {
                    return new ResultWithError(404);
                }
                // Kiểm tra xem nếu dữ liệu đang sửa ở trạng thái phê duyệt thì không cho sửa
                if (r.STATUS_ID == OtherConfig.STATUS_APPROVE && !_appContext.IsAdmin)
                {
                    return new ResultWithError(Message.RECORD_IS_APPROVED);
                }

                //param.DecisionNo = null;
                var data = Map(param, r);
                var result = _appContext.Workings.Update(data);
                // Nếu là trạng thái đã phê duyệt thì cập nhật thông tin mới nhất vào Employee
                if (data.STATUS_ID == OtherConfig.STATUS_APPROVE && data.IS_RESPONSIBLE == true)
                {
                    var e = _appContext.Employees.Where(x => x.ID == param.EmployeeId).FirstOrDefault();
                    if (e == null)
                    {
                        return new ResultWithError(Message.EMP_NOT_EXIST);
                    }
                    await ApproveWorking(data);
                }

                await _appContext.SaveChangesAsync();
                return new ResultWithError(param);
            }
            catch (Exception ex)
            {

                return new ResultWithError(ex);
            }
        }
        /// <summary>
        /// CMS Change Status Data
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<ResultWithError> RemoveAsync(List<long> param)
        {
            try
            {
                await _appContext.Database.BeginTransactionAsync();
                foreach (var item in param)
                {
                    var r = await _appContext.Workings.Where(x => x.ID == item).FirstOrDefaultAsync();
                    if (r == null)
                    {
                        return new ResultWithError(404);
                    }
                    // Kiểm tra xem nếu dữ liệu đang sửa ở trạng thái phê duyệt thì không cho xóa
                    if (r.STATUS_ID == OtherConfig.STATUS_APPROVE)
                    {
                        return new ResultWithError("RECORD_IS_APPROVED");
                    }
                    _appContext.Workings.Remove(r);
                }
                await _appContext.SaveChangesAsync();
                _appContext.Database.CommitTransaction();
                return new ResultWithError(200);
            }
            catch (Exception ex)
            {
                _appContext.Database.RollbackTransaction();
                return new ResultWithError(ex.Message);
            }
        }
        public async Task<ResultWithError> OpenStatus(long id)
        {
            try
            {
                var r = _appContext.Workings.Where(x => x.ID == id).FirstOrDefault();
                if (r == null)
                {
                    return new ResultWithError(404);
                }
                // Kiểm tra xem QĐ mở chờ phê duyệt có phải là quyết định cuối cùng không
                var c = await _appContext.Workings.Where(x => x.EMPLOYEE_ID == r.EMPLOYEE_ID && x.ID != id && x.EFFECT_DATE > r.EFFECT_DATE && x.STATUS_ID == OtherConfig.STATUS_APPROVE).CountAsync();
                if (c > 0)
                {
                    return new ResultWithError("EFFECTDATE_NOT_LESS_CURRENT");
                }
                // Kiểm tra xem nếu dữ liệu đang sửa ở trạng thái chờ phê duyệt thì không cho mở chờ phê duyệt
                if (r.STATUS_ID == OtherConfig.STATUS_WAITING)
                {
                    return new ResultWithError("RECORD_IS_WAITED");
                }
                r.STATUS_ID = OtherConfig.STATUS_WAITING;
                var result = _appContext.Workings.Update(r);
                await _appContext.SaveChangesAsync();
                return new ResultWithError(200);
            }
            catch (Exception ex)
            {
                return new ResultWithError(ex.Message);
            }
        }
        public async Task<ResultWithError> TemplateImport(int orgId)
        {
            try
            {
                // xử lý fill dữ liệu vào master data
                var ds = QueryData.ExecuteStoreToTable(Procedures.PKG_IMPORT_DECISION_DATA_IMPORT,
                new
                {
                    P_ORG_ID = orgId,
                    P_CUR_ORG = QueryData.OUT_CURSOR,
                    P_CUR_POS = QueryData.OUT_CURSOR,
                    P_CUR_LVL = QueryData.OUT_CURSOR,
                    P_CUR_SAL_TYPE = QueryData.OUT_CURSOR,
                    P_CUR_STATUS = QueryData.OUT_CURSOR,
                    P_CUR_DECISION = QueryData.OUT_CURSOR,
                    P_CUR = QueryData.OUT_CURSOR
                }, true);

                if (ds.Tables[0].Rows.Count <= 0)
                {
                    return new ResultWithError("DATA_EMPTY");
                }
                ds.Tables[0].TableName = "Org";
                ds.Tables[1].TableName = "Pos";
                ds.Tables[2].TableName = "Lvl";
                ds.Tables[3].TableName = "SalType";
                ds.Tables[4].TableName = "Status";
                ds.Tables[5].TableName = "Decision";
                ds.Tables[6].TableName = "Data";
                var pathTemp = _appContext._config["urlTempDecision"];
                var memoryStream = Template.FillTemplate(pathTemp, ds);
                return new ResultWithError(memoryStream);
            }
            catch (Exception ex)
            {
                return new ResultWithError(ex.Message);
            }
        }

        /// <summary>
        /// Import Data
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<ResultWithError> ImportTemplate(ImportParam param)
        {
            try
            {
                param.Data.RemoveRange(0, 2);
                if (param.Data.Count == 0)
                {
                    return new ResultWithError(404);
                }
                var lst = new List<TMP_HU_WORKING>();
                var guid = Guid.NewGuid().ToString();
                var error = false;
                var lstError = new List<ImportDtlParam>();
                foreach (var item in param.Data)
                {
                    var dtl = new TMP_HU_WORKING();
                    if (string.IsNullOrWhiteSpace(item.OrgId))
                    {
                        error = true;
                        item.OrgId = "!Không được để trống";
                    }
                    else
                    {
                        var org = item.OrgId.Split("-");
                        try
                        {
                            dtl.ORG_ID = int.Parse(org[0]);
                        }
                        catch
                        {
                            error = true;
                            item.OrgId = "!Phòng ban không tồn tại";
                        }
                    }

                    try
                    {
                        dtl.EFFECT_DATE = DateTime.ParseExact(item.EffectDate.Trim(), "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    }
                    catch
                    {
                        error = true;
                        item.EffectDate = "!Không đúng định dạng dd/MM/yyyy";
                    }
                    if (!string.IsNullOrWhiteSpace(item.SalaryLevelId))
                    {
                        try
                        {
                            var salType = item.SalaryLevelId.Trim().Split("-");
                            dtl.SALARY_LEVEL_ID = int.Parse(salType[0]);
                        }
                        catch
                        {
                            error = true;
                            item.SalaryLevelId = "!Bậc lương không tồn tại";
                        }
                    }
                    if (!string.IsNullOrWhiteSpace(item.SignDate))
                    {
                        try
                        {
                            dtl.SIGN_DATE = DateTime.ParseExact(item.SignDate.Trim(), "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                        }
                        catch
                        {
                            error = true;
                            item.SignDate = "!Không đúng định dạng dd/MM/yyyy";
                        }
                    }
                    if (string.IsNullOrWhiteSpace(item.SalaryBasic))
                    {
                        error = true;
                        item.SalaryBasic = "!Không được để trống";
                    }
                    else
                    {
                        try
                        {
                            dtl.SAL_BASIC = decimal.Parse(item.SalaryBasic);
                        }
                        catch
                        {

                            error = true;
                            item.SalaryBasic = "!Sai định dạng kiểu số";
                        }
                    }
                    //if (!string.IsNullOrWhiteSpace(item.Coefficient))
                    //{
                    //    try
                    //    {
                    //        dtl.COEFFICIENT = decimal.Parse(item.Coefficient);
                    //    }
                    //    catch
                    //    {

                    //        error = true;
                    //        item.Coefficient = "!Sai định dạng kiểu số";
                    //    }
                    //}
                    if (string.IsNullOrWhiteSpace(item.SalaryTotal))
                    {
                        error = true;
                        item.SalaryTotal = "!Không được để trống";
                    }
                    else
                    {
                        try
                        {
                            dtl.SAL_TOTAL = decimal.Parse(item.SalaryTotal);
                        }
                        catch
                        {

                            error = true;
                            item.SalaryTotal = "!Sai định dạng kiểu số";
                        }
                    }
                    if (string.IsNullOrWhiteSpace(item.SalaryPercent))
                    {
                        error = true;
                        item.SalaryPercent = "!Không được để trống";
                    }
                    else
                    {
                        try
                        {
                            dtl.SAL_PERCENT = decimal.Parse(item.SalaryPercent);
                        }
                        catch
                        {
                            error = true;
                            item.SalaryPercent = "!Sai định dạng kiểu số";
                        }
                    }
                    if (error)
                    {
                        error = false;
                        lstError.Add(item);
                    }
                    else
                    {
                        dtl.REF_CODE = guid;
                        dtl.CODE = item.Code;
                        dtl.POS_NAME = item.PosName;
                        dtl.DECISION_NO = item.DecisionNo;
                        dtl.TYPE_NAME = item.TypeName;
                        dtl.SALARY_TYPE_NAME = item.SalaryTypeName;
                        dtl.STATUS_NAME = item.StatusName;
                        dtl.SIGNER_NAME = item.SignName;
                        dtl.SIGNER_POSITION = item.SignPosition;
                        lst.Add(dtl);
                    }
                }

                if (lstError.Count > 0)
                {
                    var pathTemp = _appContext._config["urlTempDecision"];
                    var memoryStream = Template.FillTemp(pathTemp, lstError);
                    return new ResultWithError(memoryStream);
                }
                else
                {
                    if (lst.Count > 0)
                    {
                        await _appContext.WorkingTmps.AddRangeAsync(lst);
                        await _appContext.SaveChangesAsync();
                        // xử lý fill dữ liệu vào master data
                        var ds = QueryData.ExecuteStoreToTable("PKG_IMPORT_DECISION_IMPORT",
                        new
                        {
                            P_REF_CODE = guid,
                        }, true);

                        if (ds.Tables.Count > 0)
                        {
                            ds.Tables[0].TableName = "Data";
                            var pathTemp = _appContext._config["urlTempDecision"];
                            var memoryStream = Template.FillTemplate(pathTemp, ds);
                            return new ResultWithError(memoryStream);
                        }
                    }
                }
                return new ResultWithError(200);
            }
            catch (Exception ex)
            {
                return new ResultWithError(204);
                // throw ex;
            }
        }

        /// <summary>
        /// Portal
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<ResultWithError> PortalGetAll()
        {
            try
            {
                var r = await (from p in _appContext.Workings
                               join o in _appContext.OtherLists on p.TYPE_ID equals o.ID
                               where p.EMPLOYEE_ID == _appContext.EmpId && p.STATUS_ID == OtherConfig.STATUS_APPROVE
                               orderby p.EFFECT_DATE descending
                               where p.IS_WAGE == 0 || p.IS_WAGE == null
                               select new
                               {
                                   Id = p.ID,
                                   No = p.DECISION_NO,
                                   EffectDate = p.EFFECT_DATE,
                                   TypeName = o.NAME
                               }).ToListAsync();
                return new ResultWithError(r);
            }
            catch (Exception ex)
            {
                return new ResultWithError(ex.Message);
            }
        }

        /// <summary>
        /// Portal
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<ResultWithError> PortalGetBy(long id)
        {
            try
            {
                var r = await (from p in _appContext.Workings
                               join o in _appContext.OtherLists on p.TYPE_ID equals o.ID
                               join b in _appContext.Organizations on p.ORG_ID equals b.ID
                               join t in _appContext.Positions on p.POSITION_ID equals t.ID
                               from s in _appContext.SalaryScales.Where(x => x.ID == p.SALARY_SCALE_ID).DefaultIfEmpty()
                               from n in _appContext.SalaryRanks.Where(x => x.ID == p.SALARY_RANK_ID).DefaultIfEmpty()
                               from l in _appContext.SalaryLevels.Where(x => x.ID == p.SALARY_LEVEL_ID).DefaultIfEmpty()
                               where p.EMPLOYEE_ID == _appContext.EmpId && p.ID == id && (p.IS_WAGE == 0 || p.IS_WAGE == null)
                               select new
                               {
                                   No = p.DECISION_NO,
                                   EffectDate = p.EFFECT_DATE,
                                   TypeName = o.NAME,
                                   OrgName = b.NAME,
                                   PosName = t.NAME,
                                   ScaleName = s.NAME,
                                   RankName = n.NAME,
                                   LevelName = l.NAME,
                                   SalBasic = p.SAL_BASIC,
                                   SalTotal = p.SAL_TOTAL,
                                   SalPercent = p.SAL_PERCENT,
                                   SignName = p.SIGNER_NAME,
                                   SignPos = p.SIGNER_POSITION,
                                   SignDate = p.SIGN_DATE
                               }).ToListAsync();
                return new ResultWithError(r);
            }
            catch (Exception ex)
            {
                return new ResultWithError(ex.Message);
            }
        }


        // WAGE - IS_WAGE <> 0

        public async Task<GenericPhaseTwoListResponse<HuWorkingDTO>> TwoPhaseQueryListWage(GenericQueryListDTO<HuWorkingDTO> request)

        {
            //var raw = _appContext.Workings.AsQueryable();
            //var phase1 = await genericReducer.FirstPhaseReduce(raw, request);

            //if (phase1.ErrorType != EnumErrorType.NONE)
            //{
            //    return new()
            //    {
            //        ErrorType = phase1.ErrorType,
            //        MessageCode = phase1.MessageCode,
            //        ErrorPhase = 1
            //    };
            //}

            //if (phase1.Queryable == null)
            //{
            //    return new()
            //    {
            //        ErrorType = EnumErrorType.CATCHABLE,
            //        MessageCode = CommonMessageCode.ENTITIES_NOT_FOUND,
            //        ErrorPhase = 1
            //    };
            //}

            //var phase1IdsResult = phase1.Queryable.ToList().Aggregate("", (prev, curr) => prev + curr.ID.ToString() + ";");
            //var ids = phase1IdsResult.Split(';');

            var joined = from p in _appContext.Workings
                         from e in _appContext.Employees.Where(c => c.ID == p.EMPLOYEE_ID)
                         from t in _appContext.Positions.Where(c => c.ID == p.POSITION_ID)
                         from o in _appContext.Organizations.Where(c => c.ID == p.ORG_ID)
                         from j in _appContext.HUJobs.Where(c => c.ID == t.JOB_ID)
                         from f in _appContext.OtherLists.Where(c => c.ID == p.STATUS_ID).DefaultIfEmpty()
                         from l in _appContext.OtherLists.Where(c => c.ID == p.TYPE_ID)
                         from s in _appContext.Employees.Where(c => c.ID == p.SIGN_ID).DefaultIfEmpty()
                         from tl in _appContext.SalaryTypes.Where(c => c.ID == p.SALARY_TYPE_ID).DefaultIfEmpty()
                         from sc in _appContext.SalaryScales.Where(c => c.ID == p.SALARY_SCALE_ID).DefaultIfEmpty()
                         from ra in _appContext.SalaryRanks.Where(c => c.ID == p.SALARY_RANK_ID).DefaultIfEmpty()
                         from sl in _appContext.SalaryLevels.Where(c => c.ID == p.SALARY_LEVEL_ID).DefaultIfEmpty()
                         from tax in _appContext.OtherLists.Where(c => c.ID == p.TAXTABLE_ID).DefaultIfEmpty()
                         from tldcv in _appContext.SalaryTypes.Where(c => c.ID == p.SALARY_TYPE_ID).DefaultIfEmpty()
                         from scdcv in _appContext.SalaryScales.Where(c => c.ID == p.SALARY_SCALE_DCV_ID).DefaultIfEmpty()
                         from radcv in _appContext.SalaryRanks.Where(c => c.ID == p.SALARY_RANK_DCV_ID).DefaultIfEmpty()
                         from sldcv in _appContext.SalaryLevels.Where(c => c.ID == p.SALARY_LEVEL_DCV_ID).DefaultIfEmpty()
                         from com in _appContext.CompanyInfos.Where(c => c.ID == o.COMPANY_ID).DefaultIfEmpty()
                         from re in _appContext.OtherLists.Where(x => x.ID == com.REGION_ID).DefaultIfEmpty()
                         where p.IS_WAGE == -1
                         orderby p.STATUS_ID, p.EFFECT_DATE descending
                         select new HuWorkingDTO
                         {
                             Id = p.ID,
                             OrgId = p.ORG_ID,
                             EmployeeId = e.ID,
                             EmployeeCode = e.CODE,
                             EmployeeName = e.Profile.FULL_NAME,
                             PositionName = t.NAME,
                             SignDate = p.SIGN_DATE,
                             SignerName = p.SIGNER_NAME,
                             SignerCode = s.CODE,
                             SignerPosition = p.SIGNER_POSITION,
                             OrgName = o.NAME,
                             EffectDate = p.EFFECT_DATE,
                             ExpireDate = p.EXPIRE_DATE,
                             ExpireUpsalDate = p.EXPIRE_UPSAL_DATE,
                             DecisionNo = p.DECISION_NO,
                             StatusName = f.NAME,
                             TypeName = l.NAME,
                             SalBasic = p.SAL_BASIC,
                             SalTotal = p.SAL_TOTAL,
                             SalPercent = p.SAL_PERCENT,
                             SalaryType = tl.NAME,
                             SalaryScaleName = sc.NAME,
                             SalaryRankName = ra.NAME,
                             SalaryLevelName = sl.NAME,
                             ShortTempSalary = p.SHORT_TEMP_SALARY,
                             TaxTableName = tax.NAME,
                             Coefficient = p.COEFFICIENT,
                             CoefficientDcv = p.COEFFICIENT_DCV,
                             SalaryScaleDcvName = scdcv.NAME,
                             SalaryRankDcvName = sldcv.NAME,
                             SalaryLevelDcvName = sldcv.NAME,
                             RegionName = re.NAME,
                             EmpWorkStatus = e.WORK_STATUS_ID,
                             EffectUpsalDate = p.EFFECT_UPSAL_DATE,
                             ReasonUpsal = p.REASON_UPSAL,
                             StatusId = p.STATUS_ID,
                             JobOrderNum = (int)(j.ORDERNUM ?? 999)
                         };

            var phase2 = await genericReducer.SinglePhaseReduce(joined, request);
            return phase2;
        }
        /// <summary>
        /// CMS Get All Data
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<PagedResult<WorkingDTO>> GetAllWage(WorkingDTO param)
        {
            var queryable = from p in _appContext.Workings
                            join e in _appContext.Employees on p.EMPLOYEE_ID equals e.ID
                            join t in _appContext.Positions on e.POSITION_ID equals t.ID
                            join o in _appContext.Organizations on e.ORG_ID equals o.ID
                            from f in _appContext.OtherListFixs.Where(c => c.ID == p.STATUS_ID && c.TYPE == SystemConfig.STATUS_APPROVE).DefaultIfEmpty()
                            join l in _appContext.OtherLists on p.TYPE_ID equals l.ID
                            from s in _appContext.Employees.Where(c => c.ID == p.SIGN_ID).DefaultIfEmpty()
                            join tl in _appContext.SalaryTypes on p.SALARY_TYPE_ID equals tl.ID
                            orderby p.STATUS_ID, p.EFFECT_DATE descending
                            select new WorkingDTO
                            {
                                Id = p.ID,
                                EmployeeId = p.EMPLOYEE_ID,
                                EmployeeCode = e.CODE,
                                EmployeeName = e.Profile.FULL_NAME,
                                PositionName = t.NAME,
                                SignDate = p.SIGN_DATE,
                                SignerName = p.SIGNER_NAME,
                                SignerCode = s.CODE,
                                SignerPosition = p.SIGNER_POSITION,
                                OrgName = o.NAME,
                                OrgId = p.ORG_ID,
                                EffectDate = p.EFFECT_DATE,
                                DecisionNo = p.DECISION_NO,
                                StatusId = p.STATUS_ID,
                                StatusName = f.NAME,
                                TypeId = p.TYPE_ID,
                                TypeName = l.NAME,
                                WorkStatusId = e.WORK_STATUS_ID,
                                SalBasic = p.SAL_BASIC,
                                SalTotal = p.SAL_TOTAL,
                                SalPercent = p.SAL_PERCENT,
                                SalaryType = tl.NAME
                            };

            var orgIds = await QueryData.ExecuteList(Procedures.PKG_COMMON_LIST_ORG,
                    new
                    {
                        P_IS_ADMIN = _appContext.IsAdmin == true ? 1 : 0,
                        P_ORG_ID = param.OrgId,
                        P_CURENT_USER_ID = _appContext.CurrentUserId,
                        P_CUR = QueryData.OUT_CURSOR
                    }, false);


            List<long?> ids = orgIds.Select(c => (long?)((dynamic)c).ID).ToList();
            if (param.OrgId != null)
            {
                ids.Add(param.OrgId);
            }
            queryable = queryable.Where(p => ids.Contains(p.OrgId));
            if (!string.IsNullOrWhiteSpace(param.StatusName))
            {
                queryable = queryable.Where(p => p.StatusName.ToUpper().Contains(param.StatusName.ToUpper()));
            }

            if (!string.IsNullOrWhiteSpace(param.EmployeeCode))
            {
                queryable = queryable.Where(p => p.EmployeeCode.ToUpper().Contains(param.EmployeeCode.ToUpper()));
            }
            if (!string.IsNullOrWhiteSpace(param.EmployeeName))
            {
                queryable = queryable.Where(p => p.EmployeeName.ToUpper().Contains(param.EmployeeName.ToUpper()));
            }
            if (!string.IsNullOrWhiteSpace(param.PositionName))
            {
                queryable = queryable.Where(p => p.PositionName.ToUpper().Contains(param.PositionName.ToUpper()));
            }
            if (!string.IsNullOrWhiteSpace(param.OrgName))
            {
                queryable = queryable.Where(p => p.OrgName.ToUpper().Contains(param.OrgName.ToUpper()));
            }
            if (!string.IsNullOrWhiteSpace(param.DecisionNo))
            {
                queryable = queryable.Where(p => p.DecisionNo.ToUpper().Contains(param.DecisionNo.ToUpper()));
            }
            if (!string.IsNullOrWhiteSpace(param.StatusName))
            {
                queryable = queryable.Where(p => p.StatusName.ToUpper().Contains(param.StatusName.ToUpper()));
            }
            if (!string.IsNullOrWhiteSpace(param.TypeName))
            {
                queryable = queryable.Where(p => p.TypeName.ToUpper().Contains(param.TypeName.ToUpper()));
            }

            if (param.EffectDate != null)
            {
                queryable = queryable.Where(p => p.EffectDate == param.EffectDate);
            }
            if (param.WorkStatusId == null || param.WorkStatusId == 0)
            {
                queryable = queryable.Where(p => p.WorkStatusId != OtherConfig.EMP_STATUS_TERMINATE);
            }

            return await PagingList(queryable, param);
        }
        public async Task<PagedResult<WorkingDTO>> GetWage(WorkingDTO param)
        {
            var queryable = from p in _appContext.Workings
                            join e in _appContext.Employees on p.EMPLOYEE_ID equals e.ID
                            join f in _appContext.OtherListFixs.Where(c => c.TYPE == SystemConfig.STATUS_APPROVE && c.ID == 2) on p.STATUS_ID equals f.ID
                            orderby p.EFFECT_DATE descending
                            where p.EMPLOYEE_ID == param.EmployeeId
                            select new WorkingDTO
                            {
                                Id = p.ID,
                                EmployeeId = p.EMPLOYEE_ID,
                                EmployeeCode = e.CODE,
                                EmployeeName = e.Profile.FULL_NAME,
                                SignDate = p.SIGN_DATE,
                                SignerName = p.SIGNER_NAME,
                                SignerPosition = p.SIGNER_POSITION,
                                OrgId = p.ORG_ID,
                                EffectDate = p.EFFECT_DATE,
                                DecisionNo = p.DECISION_NO,
                                StatusName = f.NAME,
                                StatusId = p.STATUS_ID,
                                TypeId = p.TYPE_ID,
                                Note = p.NOTE
                            };



            if (!string.IsNullOrWhiteSpace(param.DecisionNo))
            {
                queryable = queryable.Where(p => p.DecisionNo.ToUpper().Contains(param.DecisionNo.ToUpper()));
            }
            if (!string.IsNullOrWhiteSpace(param.StatusName))
            {
                queryable = queryable.Where(p => p.StatusName.ToUpper().Contains(param.StatusName.ToUpper()));
            }




            return await PagingList(queryable, param);
        }
        /// <summary>
        /// CMS Get Detail
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>        
        public async Task<ResultWithError> GetWageById(long id)
        {
            try
            {
                List<int> lstcheckInsItems = new List<int>();
                var r = await (from p in _appContext.Workings
                               from e in _appContext.Employees.Where(c => c.ID == p.EMPLOYEE_ID)
                               from o in _appContext.Organizations.Where(c => c.ID == p.ORG_ID)
                               from o2 in _appContext.Organizations.Where(c => c.ID == o.PARENT_ID).DefaultIfEmpty()
                               from sc in _appContext.SalaryScales.Where(c => c.ID == p.SALARY_SCALE_ID).DefaultIfEmpty()
                               from ra in _appContext.SalaryRanks.Where(c => c.ID == p.SALARY_RANK_ID).DefaultIfEmpty()
                               from sl in _appContext.SalaryLevels.Where(c => c.ID == p.SALARY_LEVEL_ID).DefaultIfEmpty()
                               from po in _appContext.Positions.Where(c => c.ID == p.POSITION_ID).DefaultIfEmpty()
                               from com in _appContext.CompanyInfos.Where(c => c.ID == o.COMPANY_ID).DefaultIfEmpty()
                               from re in _appContext.OtherLists.Where(x => x.ID == com.REGION_ID).DefaultIfEmpty()
                               where p.ID == id
                               select new
                               {
                                   Id = p.ID,
                                   EmployeeId = p.EMPLOYEE_ID,
                                   EmployeeName = e.Profile.FULL_NAME,
                                   EmployeeCode = e.CODE,
                                   PositionId = p.POSITION_ID,
                                   PositionName = po.NAME,
                                   EmployeeObjId = p.EMPLOYEE_OBJ_ID,
                                   OrgId = p.ORG_ID,
                                   OrgName = o.NAME,
                                   EffectDate = p.EFFECT_DATE,
                                   ExpireDate = p.EXPIRE_DATE,
                                   DecisionNo = p.DECISION_NO,
                                   TypeId = p.TYPE_ID,
                                   StatusId = p.STATUS_ID,
                                   Note = p.NOTE,
                                   TaxtableId = p.TAXTABLE_ID,
                                   SalaryTypeId = p.SALARY_TYPE_ID,
                                   SalaryScaleId = p.SALARY_SCALE_ID,
                                   SalaryRankId = p.SALARY_RANK_ID,
                                   SalaryLevelId = p.SALARY_LEVEL_ID,
                                   SalaryScaleDcvId = p.SALARY_SCALE_DCV_ID,
                                   SalaryRankDcvId = p.SALARY_RANK_DCV_ID,
                                   SalaryLevelDcvId = p.SALARY_LEVEL_DCV_ID,
                                   SalBasic = p.SAL_BASIC,
                                   SalTotal = p.SAL_TOTAL,
                                   SalPercent = p.SAL_PERCENT,
                                   SignId = p.SIGN_ID,
                                   SignerName = p.SIGNER_NAME,
                                   SignerPosition = p.SIGNER_POSITION,
                                   SignDate = p.SIGN_DATE,
                                   Coefficient = p.COEFFICIENT,
                                   CoefficientDcv = p.COEFFICIENT_DCV,
                                   ReligionId = re.ID,
                                   orgParentName = o2.NAME,
                                   Allowances = (from p in _appContext.WorkingAllowances
                                                 from n in _appContext.Allowances.Where(x => x.ID == p.ALLOWANCE_ID).DefaultIfEmpty()
                                                 where p.WORKING_ID == id
                                                 select new WorkingAllowInputDTO
                                                 {
                                                     Id = p.ID,
                                                     AllowanceId = p.ALLOWANCE_ID,
                                                     AllowanceName = n.NAME,
                                                     Amount = p.AMOUNT,
                                                     Effectdate = p.EFFECT_DATE,
                                                     ExpireDate = p.EXPIRE_DATE,
                                                     Coefficient = p.COEFFICIENT
                                                 }).ToList(),
                                   RegionName = re.NAME,
                                   ShortTempSalary = p.SHORT_TEMP_SALARY,
                                   ExpireUpsalDate = p.EXPIRE_UPSAL_DATE,
                                   isBHXH = p.IS_BHXH,
                                   isBHYT = p.IS_BHYT,
                                   isBHTNLDBNN = p.IS_BHTNLD_BNN,
                                   isBHTN = p.IS_BHTN,
                                   lstCheckIns = lstcheckInsItems,
                                   Attachment = p.ATTACHMENT,
                                   //AttachmentBuffer = p.ATTACHMENT,
                               }).FirstOrDefaultAsync();
                lstcheckInsItems = new List<int>();
                if (r.isBHXH != null && r.isBHXH == -1)
                {
                    lstcheckInsItems.Add(1);
                }
                if (r.isBHYT != null && r.isBHYT == -1)
                {
                    lstcheckInsItems.Add(2);
                }
                if (r.isBHTNLDBNN != null && r.isBHTNLDBNN == -1)
                {
                    lstcheckInsItems.Add(3);
                }
                if (r.isBHTN != null && r.isBHTN == -1)
                {
                    lstcheckInsItems.Add(4);
                }
                r.lstCheckIns.AddRange(lstcheckInsItems.Distinct().ToList());
                return new ResultWithError(r);
            }
            catch (Exception ex)
            {
                return new ResultWithError(ex.Message);
            }
        }
        public async Task<ResultWithError> GetLastWage(long? empId, DateTime? date)
        {
            try
            {
                if (date == null)
                {
                    date = DateTime.Now;
                }
                var r = await (from p in _appContext.Workings
                               from t in _appContext.SalaryTypes.Where(c => c.ID == p.SALARY_TYPE_ID).DefaultIfEmpty()
                               from s in _appContext.SalaryScales.Where(c => c.ID == p.SALARY_SCALE_ID).DefaultIfEmpty()
                               from ra in _appContext.SalaryRanks.Where(c => c.ID == p.SALARY_RANK_ID).DefaultIfEmpty()
                               from l in _appContext.SalaryLevels.Where(c => c.ID == p.SALARY_LEVEL_ID).DefaultIfEmpty()
                               where p.EMPLOYEE_ID == empId && p.EFFECT_DATE <= date && p.IS_WAGE == -1 && p.STATUS_ID == 994
                               orderby p.EFFECT_DATE descending
                               select new
                               {
                                   Id = p.ID,
                                   OrgId = p.ORG_ID,
                                   PositionId = p.POSITION_ID,
                                   StatusId = p.STATUS_ID,
                                   WorkingNo = p.DECISION_NO,
                                   SalaryTypeId = p.SALARY_TYPE_ID,
                                   SalaryTypeName = t.NAME,
                                   SalaryScaleId = p.SALARY_SCALE_ID,
                                   SalaryScaleName = s.NAME,
                                   SalaryRankId = p.SALARY_RANK_ID,
                                   SalaryRankName = ra.NAME,
                                   SalaryLevelId = p.SALARY_LEVEL_ID,
                                   SalaryLevelName = l.NAME,
                                   SalBasic = p.SAL_BASIC,
                                   SalTotal = p.SAL_TOTAL,
                                   SalPercent = p.SAL_PERCENT,
                                   SignId = p.SIGN_ID,
                                   SignerName = p.SIGNER_NAME,
                                   SignerPosition = p.SIGNER_POSITION,
                                   SignDate = p.SIGN_DATE,
                                   Note = p.NOTE
                               }).FirstOrDefaultAsync();
                return new ResultWithError(r);
            }
            catch (Exception ex)
            {
                return new ResultWithError(ex.Message);
            }
        }
        /// <summary>
        /// Create Data
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<ResultWithError> CreateWageAsync(WorkingInputDTO param, string sid)
        {
            try
            {
                var e = _appContext.Employees.Where(x => x.ID == param.EmployeeId).FirstOrDefault();

                if (e == null)
                {
                    return new ResultWithError(Message.EMP_NOT_EXIST);
                }

                var organization = (from p in _appContext.Organizations.Where(p => p.ID == e.ORG_ID) select p).FirstOrDefault();
                var company = (from p in _appContext.CompanyInfos.Where(p => p.ID == organization.COMPANY_ID) select p).FirstOrDefault();
                //check effdate 0 namm trong vung
                DateTime effDate = param.EffectDate!.Value;
                //var insRegion = (from p in _appContext.Regions.Where(x => x.AREA_ID == company!.REGION_ID && x.EFFECT_DATE <= param.EffectDate).OrderByDescending(x => x.CREATED_DATE) select p).Any();
                var insRegion = _appContext.Regions.Where(x => x.AREA_ID == company!.REGION_ID && x.EFFECT_DATE < param.EffectDate).Any();

                if (!insRegion)
                {
                    return new ResultWithError("THERE_IS_NO_AVAILABILITY_MINIMUM_WAGE");
                }
                var workingMax = await GetLastWorking(param.EmployeeId, null);
                var rs = (WorkingInputDTO)workingMax.Data;
                //var r2 = _appContext.Workings.Where(x => x.DECISION_NO == param.DecisionNo).Count();
                //if (r2 > 0)
                //{
                //    return new ResultWithError(Consts.CODE_EXISTS);
                //}
                // Kiểm tra xem có QĐ nào có ngày hiệu lực lớn hơn ngày QĐ đang làm (chỉ tính các quyết định đã phê duyệt)
                var c = await _appContext.Workings.Where(x => x.EMPLOYEE_ID == param.EmployeeId && x.ID != param.Id && x.STATUS_ID == OtherConfig.STATUS_APPROVE && x.IS_WAGE == -1).FirstOrDefaultAsync();
                if (c != null)
                {
                    if (c.EFFECT_DATE > param.EffectDate)
                    {
                        return new ResultWithError("EFFECTDATE_NOT_LESS_CURRENT");
                    }
                    //Check thay đổi lương
                    if (c.SAL_BASIC != param.SalBasic || c.SAL_TOTAL != param.SalTotal || c.SAL_PERCENT != param.SalPercent)
                    {
                        c.IS_CHANGE_SAL = true;
                    }
                }


                // Gencode
                //var DecisionCode = "";
                var data = Map(param, new HU_WORKING());
                var PCCoef = (from p in _appContext.WorkingAllowances
                              from a in _appContext.Allowances.Where(f => f.ID == p.ALLOWANCE_ID)
                              where p.WORKING_ID == data.ID && p.COEFFICIENT != null
                              select p.COEFFICIENT).ToList();
                var coef = param.Allowances!.Select(p => p.Coefficient).ToList();
                decimal? sumPCCoef = 0;
                if (coef.Count() == 0)
                {
                    sumPCCoef = 0;
                }
                else
                {
                    sumPCCoef = coef.Sum();
                }
                var moneyRegion = (from p in _appContext.Regions where p.AREA_ID == param.ReligionId && p.EFFECT_DATE <= data.EFFECT_DATE orderby p.EFFECT_DATE descending select p.MONEY).FirstOrDefault();
                if (moneyRegion == null)
                {
                    moneyRegion = 0;
                }
                var moneyOMN = (from p in _appContext.OtherLists where p.CODE == "OMN" && p.IS_ACTIVE == true select p.NOTE).FirstOrDefault();
                if (moneyOMN == null)
                {
                    moneyOMN = "0";
                }
                if (data.COEFFICIENT == null)
                {
                    data.COEFFICIENT = 0;
                }
                if (data.COEFFICIENT_DCV == null)
                {
                    data.COEFFICIENT_DCV = 0;
                }
                //tinh luong dong bh
                if (param.shortTempSalary == null)
                {
                    //lam tron so
                    var listFive = new List<string>() { "TBL001", "TBL002", "TBL003", "TBL004", "TBL010" };
                    var listThree = new List<string>() { "TBL005", "TBL006", "TBL007", "TBL008", "TBL011 ", "TBL012", "TBL013", "TBL014" };

                    var salaryScale = _appContext.SalaryScales.AsNoTracking().Where(p => p.ID == param.SalaryScaleId).FirstOrDefault();

                    data.SAL_INSU = (((moneyRegion * data.COEFFICIENT) + (decimal.Parse(moneyOMN) * data.COEFFICIENT_DCV))) + (sumPCCoef * moneyRegion);

                    if (salaryScale == null)
                    {
                        return new ResultWithError(400, "SALARY_SCALE_IS_NOT_EXITS");
                    }
                    else
                    {
                        if (listFive.Contains(salaryScale.CODE))
                        {
                            data.SAL_INSU = Math.Round((decimal)data.SAL_INSU! / 100000) * 100000;
                        }
                        if (listThree.Contains(salaryScale.CODE))
                        {
                            data.SAL_INSU = Math.Round((decimal)data.SAL_INSU! / 1000) * 1000;
                        }
                    }
                }
                else
                {
                    data.SAL_INSU = 0;
                }
                //data.DECISION_NO = DecisionCode;
                await _appContext.Database.BeginTransactionAsync();
                if (data.EXPIRE_UPSAL_DATE != null)
                {
                    // vì trên màn hình lưu bị -1 nên phải bù lại
                    data.EXPIRE_UPSAL_DATE = data.EXPIRE_UPSAL_DATE.Value.AddDays(1);
                }
                data.IS_WAGE = -1;
                if (rs != null)
                {
                    data.ORG_ID = rs.OrgId;
                    data.POSITION_ID = rs.PositionId;
                }
                else
                {
                    data.ORG_ID = e.ORG_ID;
                    data.POSITION_ID = e.POSITION_ID;
                }
                if (param.lstCheckIns != null && param.lstCheckIns.Count > 0)
                {
                    foreach (var item in param.lstCheckIns)
                    {
                        switch (item)
                        {
                            case 1:
                                data.IS_BHXH = -1;
                                break;
                            case 2:
                                data.IS_BHYT = -1;
                                break;
                            case 3:
                                data.IS_BHTNLD_BNN = -1;
                                break;
                            case 4:
                                data.IS_BHTN = -1;
                                break;
                        }
                    }
                }
                data.CREATED_BY = sid;
                data.CREATED_DATE = DateTime.UtcNow;
                var result = await _appContext.Workings.AddAsync(data);
                await _appContext.SaveChangesAsync();
                if (data.STATUS_ID == OtherConfig.STATUS_APPROVE)
                {
                    //e.ORG_ID = data.ORG_ID;
                    //e.POSITION_ID = data.POSITION_ID;
                    //e.LAST_WORKING_ID = param.Id;
                    e.SALARY_TYPE_ID = data.SALARY_TYPE_ID;
                    e.SAL_TOTAL = data.SAL_TOTAL;
                    e.SAL_BASIC = data.SAL_BASIC;
                    e.SAL_RATE = data.SAL_PERCENT;
                    e.EFFECT_DATE = data.EFFECT_DATE;
                    //if (e.JOIN_DATE == null)
                    //{
                    //    e.JOIN_DATE = param.EffectDate;
                    //}
                    _appContext.Employees.Update(e);
                }

                if (param.Allowances != null && param.Allowances.Count > 0)
                {
                    var d = _appContext.WorkingAllowances.Where(x => x.WORKING_ID == param.Id).ToList();
                    if (d != null)
                    {
                        _appContext.WorkingAllowances.RemoveRange(d);
                    }
                    foreach (var item in param.Allowances)
                    {
                        item.Id = null;
                        var dataAllow = Map(item, new HU_WORKING_ALLOW());
                        dataAllow.WORKING_ID = data.ID;


                        // Round to 5 decimal places
                        int decimalPlaces = 5;
                        dataAllow.COEFFICIENT = Math.Round((decimal)dataAllow.COEFFICIENT, decimalPlaces);


                        await _appContext.WorkingAllowances.AddAsync(dataAllow);
                    }
                }
                await _appContext.SaveChangesAsync();

                _appContext.Database.CommitTransaction();
                param.Id = data.ID;
                return new ResultWithError(param);
            }
            catch (Exception ex)
            {
                _appContext.Database.RollbackTransaction();
                return new ResultWithError(ex);
            }
        }
        /// <summary>
        /// CMS Edit Data
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<ResultWithError> UpdateWageAsync(WorkingInputDTO param, string sid)
        {
            try
            {

                var r = (from p in _appContext.Workings where p.ID == param.Id select p).FirstOrDefault();

                // Kiểm tra xem nếu dữ liệu đang sửa ở trạng thái phê duyệt thì không cho sửa
                if (r.STATUS_ID == OtherConfig.STATUS_APPROVE)
                {
                    return new ResultWithError(CommonMessageCode.NOT_UPDATE_BECAUSE_ROW_APPROVED);
                }
                var organization = (from p in _appContext.Organizations.Where(p => p.ID == r.ORG_ID) select p).FirstOrDefault();
                var company = (from p in _appContext.CompanyInfos.Where(p => p.ID == organization!.COMPANY_ID) select p).FirstOrDefault();
                //check effdate 0 namm trong vung
                DateTime effDate = param.EffectDate!.Value;
                var insRegion = _appContext.Regions.Where(x => x.AREA_ID == company!.REGION_ID && x.EFFECT_DATE < param.EffectDate).Any();

                if (!insRegion)
                {
                    return new ResultWithError("THERE_IS_NO_AVAILABILITY_MINIMUM_WAGE");
                }

                var workingMax = await GetLastWorking(param.EmployeeId, null);
                var rs = (WorkingInputDTO)workingMax.Data;
                var e = _appContext.Employees.Where(x => x.ID == param.EmployeeId).FirstOrDefault();
                if (e == null)
                {
                    return new ResultWithError(Message.EMP_NOT_EXIST);
                }
                // Kiểm tra xem có QĐ nào có ngày hiệu lực lớn hơn ngày QĐ đang làm (chỉ tính các quyết định đã phê duyệt)
                var c = await _appContext.Workings.Where(x => x.EMPLOYEE_ID == param.EmployeeId && x.ID != param.Id && x.EFFECT_DATE > param.EffectDate && x.STATUS_ID == OtherConfig.STATUS_APPROVE && x.IS_WAGE == -1).CountAsync();
                if (c > 0)
                {
                    return new ResultWithError("EFFECTDATE_NOT_LESS_CURRENT");
                }

                //param.DecisionNo = null;
                var data = Map(param, r);
                if (data.EXPIRE_UPSAL_DATE != null)
                {
                    // vì trên màn hình lưu bị -1 nên phải bù lại
                    data.EXPIRE_UPSAL_DATE = data.EXPIRE_UPSAL_DATE.Value.AddDays(1);
                }
                data.IS_WAGE = -1;

                var PCCoef = (from p in _appContext.WorkingAllowances
                              from a in _appContext.Allowances.Where(f => f.ID == p.ALLOWANCE_ID)
                              where p.WORKING_ID == data.ID && p.COEFFICIENT != null
                              select p.COEFFICIENT).ToList();
                var coef = param.Allowances.Select(p => p.Coefficient).ToList();
                decimal? sumPCCoef = 0;
                if (coef.Count() == 0)
                {
                    sumPCCoef = 0;
                }
                else
                {
                    sumPCCoef = coef.Sum();
                }
                var moneyRegion = (from p in _appContext.Regions where p.AREA_ID == param.ReligionId && p.EFFECT_DATE <= data.EFFECT_DATE orderby p.EFFECT_DATE descending select p.MONEY).FirstOrDefault();
                if (moneyRegion == null)
                {
                    moneyRegion = 0;
                }
                var moneyOMN = (from p in _appContext.OtherLists where p.CODE == "OMN" && p.IS_ACTIVE == true select p.NOTE).FirstOrDefault();
                if (moneyOMN == null)
                {
                    moneyOMN = "0";
                }
                if (data.COEFFICIENT == null)
                {
                    data.COEFFICIENT = 0;
                }
                if (data.COEFFICIENT_DCV == null)
                {
                    data.COEFFICIENT_DCV = 0;
                }

                //tinh luong dong bh
                if (param.shortTempSalary == null)
                {
                    //lam tron so
                    var listFive = new List<string>() { "TBL001", "TBL002", "TBL003", "TBL004", "TBL010" };
                    var listThree = new List<string>() { "TBL005", "TBL006", "TBL007", "TBL008", "TBL011 ", "TBL012", "TBL013", "TBL014" };

                    var salaryScale = _appContext.SalaryScales.AsNoTracking().Where(p => p.ID == param.SalaryScaleId).FirstOrDefault();

                    data.SAL_INSU = (((moneyRegion * data.COEFFICIENT) + (decimal.Parse(moneyOMN) * data.COEFFICIENT_DCV))) + (sumPCCoef * moneyRegion);

                    if (salaryScale == null)
                    {
                        return new ResultWithError(400, "SALARY_SCALE_IS_NOT_EXITS");
                    }
                    else
                    {
                        if (listFive.Contains(salaryScale.CODE))
                        {
                            data.SAL_INSU = Math.Round((decimal)data.SAL_INSU! / 100000) * 100000;
                        }
                        if (listThree.Contains(salaryScale.CODE))
                        {
                            data.SAL_INSU = Math.Round((decimal)data.SAL_INSU! / 1000) * 1000;
                        }
                    }
                }
                else
                {
                    data.SAL_INSU = 0;
                }
                

                if (rs != null)
                {
                    data.ORG_ID = rs.OrgId;
                    data.POSITION_ID = rs.PositionId;
                }
                else
                {
                    data.ORG_ID = e.ORG_ID;
                    data.POSITION_ID = e.POSITION_ID;
                }
                if (param.lstCheckIns != null && param.lstCheckIns.Count > 0)
                {
                    data.IS_BHXH = 0;
                    data.IS_BHYT = 0;
                    data.IS_BHTNLD_BNN = 0;
                    data.IS_BHTN = 0;
                    foreach (var item in param.lstCheckIns)
                    {
                        switch (item)
                        {
                            case 1:
                                data.IS_BHXH = -1;
                                break;
                            case 2:
                                data.IS_BHYT = -1;
                                break;
                            case 3:
                                data.IS_BHTNLD_BNN = -1;
                                break;
                            case 4:
                                data.IS_BHTN = -1;
                                break;
                        }
                    }
                }
                data.UPDATED_BY = sid;
                data.UPDATED_DATE = DateTime.UtcNow;
                var result = _appContext.Workings.Update(data);
                // Nếu là trạng thái đã phê duyệt thì cập nhật thông tin mới nhất vào Employee
                if (data.STATUS_ID == OtherConfig.STATUS_APPROVE)
                {
                    //e.ORG_ID = data.ORG_ID;
                    //e.POSITION_ID = data.POSITION_ID;
                    //e.LAST_WORKING_ID = param.Id;
                    e.SALARY_TYPE_ID = data.SALARY_TYPE_ID;
                    e.SAL_TOTAL = data.SAL_TOTAL;
                    e.SAL_BASIC = data.SAL_BASIC;
                    e.SAL_RATE = data.SAL_PERCENT;
                    e.EFFECT_DATE = data.EFFECT_DATE;
                    //if (e.JOIN_DATE == null)
                    //{
                    //    e.JOIN_DATE = param.EffectDate;
                    //}
                    _appContext.Employees.Update(e);
                }

                if (param.Allowances != null && param.Allowances.Count > 0)
                {
                    var d = _appContext.WorkingAllowances.Where(x => x.WORKING_ID == param.Id).ToList();
                    if (d != null)
                    {
                        _appContext.WorkingAllowances.RemoveRange(d);
                    }
                    foreach (var item in param.Allowances)
                    {
                        item.Id = null;
                        var dataAllow = Map(item, new HU_WORKING_ALLOW());
                        dataAllow.WORKING_ID = data.ID;


                        // Round to 5 decimal places
                        int decimalPlaces = 5;
                        dataAllow.COEFFICIENT = Math.Round((decimal)dataAllow.COEFFICIENT, decimalPlaces);


                        await _appContext.WorkingAllowances.AddAsync(dataAllow);
                    }
                }


                // this test case is delete all record
                // in table HU_WORKING_ALLOW
                if (param.Allowances != null && param.Allowances.Count == 0)
                {
                    var d = _appContext.WorkingAllowances.Where(x => x.WORKING_ID == param.Id).ToList();
                    if (d != null)
                    {
                        _appContext.WorkingAllowances.RemoveRange(d);
                    }
                }


                await _appContext.SaveChangesAsync();
                return new ResultWithError(param);
            }
            catch (Exception ex)
            {
                return new ResultWithError(400, ex);
            }
        }
        /// <summary>
        /// CMS Change Status Data
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<ResultWithError> RemoveWageAsync(List<long> param)
        {
            try
            {
                await _appContext.Database.BeginTransactionAsync();
                foreach (var item in param)
                {
                    var r = await _appContext.Workings.Where(x => x.ID == item).FirstOrDefaultAsync();
                    if (r == null)
                    {
                        return new ResultWithError(404);
                    }
                    // Kiểm tra xem nếu dữ liệu đang sửa ở trạng thái phê duyệt thì không cho xóa
                    if (r.STATUS_ID == OtherConfig.STATUS_APPROVE)
                    {
                        return new ResultWithError("RECORD_IS_APPROVED");
                    }
                    _appContext.Workings.Remove(r);
                }
                await _appContext.SaveChangesAsync();
                _appContext.Database.CommitTransaction();
                return new ResultWithError(200);
            }
            catch (Exception ex)
            {
                _appContext.Database.RollbackTransaction();
                return new ResultWithError(ex.Message);
            }
        }
        public async Task<ResultWithError> OpenStatusWage(long id)
        {
            try
            {
                var r = _appContext.Workings.Where(x => x.ID == id).FirstOrDefault();
                if (r == null)
                {
                    return new ResultWithError(404);
                }
                // Kiểm tra xem QĐ mở chờ phê duyệt có phải là quyết định cuối cùng không
                var c = await _appContext.Workings.Where(x => x.EMPLOYEE_ID == r.EMPLOYEE_ID && x.ID != id && x.EFFECT_DATE > r.EFFECT_DATE && x.STATUS_ID == OtherConfig.STATUS_APPROVE).CountAsync();
                if (c > 0)
                {
                    return new ResultWithError("EFFECTDATE_NOT_LESS_CURRENT");
                }
                // Kiểm tra xem nếu dữ liệu đang sửa ở trạng thái chờ phê duyệt thì không cho mở chờ phê duyệt
                if (r.STATUS_ID == OtherConfig.STATUS_WAITING)
                {
                    return new ResultWithError("RECORD_IS_WAITED");
                }
                r.STATUS_ID = OtherConfig.STATUS_WAITING;
                var result = _appContext.Workings.Update(r);
                await _appContext.SaveChangesAsync();
                return new ResultWithError(200);
            }
            catch (Exception ex)
            {
                return new ResultWithError(ex.Message);
            }
        }
        public async Task<ResultWithError> TemplateImportWage(int orgId)
        {
            try
            {
                // xử lý fill dữ liệu vào master data
                var ds = QueryData.ExecuteStoreToTable(Procedures.PKG_IMPORT_DECISION_DATA_IMPORT,
                new
                {
                    P_ORG_ID = orgId,
                    P_CUR_ORG = QueryData.OUT_CURSOR,
                    P_CUR_POS = QueryData.OUT_CURSOR,
                    P_CUR_LVL = QueryData.OUT_CURSOR,
                    P_CUR_SAL_TYPE = QueryData.OUT_CURSOR,
                    P_CUR_STATUS = QueryData.OUT_CURSOR,
                    P_CUR_DECISION = QueryData.OUT_CURSOR,
                    P_CUR = QueryData.OUT_CURSOR
                }, true);

                if (ds.Tables[0].Rows.Count <= 0)
                {
                    return new ResultWithError("DATA_EMPTY");
                }
                ds.Tables[0].TableName = "Org";
                ds.Tables[1].TableName = "Pos";
                ds.Tables[2].TableName = "Lvl";
                ds.Tables[3].TableName = "SalType";
                ds.Tables[4].TableName = "Status";
                ds.Tables[5].TableName = "Decision";
                ds.Tables[6].TableName = "Data";
                var pathTemp = _appContext._config["urlTempDecision"];
                var memoryStream = Template.FillTemplate(pathTemp, ds);
                return new ResultWithError(memoryStream);
            }
            catch (Exception ex)
            {
                return new ResultWithError(ex.Message);
            }
        }

        /// <summary>
        /// Import Data
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<ResultWithError> ImportTemplateWage(ImportParam param)
        {
            try
            {
                param.Data.RemoveRange(0, 2);
                if (param.Data.Count == 0)
                {
                    return new ResultWithError(404);
                }
                var lst = new List<TMP_HU_WORKING>();
                var guid = Guid.NewGuid().ToString();
                var error = false;
                var lstError = new List<ImportDtlParam>();
                foreach (var item in param.Data)
                {
                    var dtl = new TMP_HU_WORKING();
                    if (string.IsNullOrWhiteSpace(item.OrgId))
                    {
                        error = true;
                        item.OrgId = "!Không được để trống";
                    }
                    else
                    {
                        var org = item.OrgId.Split("-");
                        try
                        {
                            dtl.ORG_ID = int.Parse(org[0]);
                        }
                        catch
                        {
                            error = true;
                            item.OrgId = "!Phòng ban không tồn tại";
                        }
                    }

                    try
                    {
                        dtl.EFFECT_DATE = DateTime.ParseExact(item.EffectDate.Trim(), "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    }
                    catch
                    {
                        error = true;
                        item.EffectDate = "!Không đúng định dạng dd/MM/yyyy";
                    }
                    if (!string.IsNullOrWhiteSpace(item.SalaryLevelId))
                    {
                        try
                        {
                            var salType = item.SalaryLevelId.Trim().Split("-");
                            dtl.SALARY_LEVEL_ID = int.Parse(salType[0]);
                        }
                        catch
                        {
                            error = true;
                            item.SalaryLevelId = "!Bậc lương không tồn tại";
                        }
                    }
                    if (!string.IsNullOrWhiteSpace(item.SignDate))
                    {
                        try
                        {
                            dtl.SIGN_DATE = DateTime.ParseExact(item.SignDate.Trim(), "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                        }
                        catch
                        {
                            error = true;
                            item.SignDate = "!Không đúng định dạng dd/MM/yyyy";
                        }
                    }
                    if (string.IsNullOrWhiteSpace(item.SalaryBasic))
                    {
                        error = true;
                        item.SalaryBasic = "!Không được để trống";
                    }
                    else
                    {
                        try
                        {
                            dtl.SAL_BASIC = decimal.Parse(item.SalaryBasic);
                        }
                        catch
                        {

                            error = true;
                            item.SalaryBasic = "!Sai định dạng kiểu số";
                        }
                    }
                    //if (!string.IsNullOrWhiteSpace(item.Coefficient))
                    //{
                    //    try
                    //    {
                    //        dtl.COEFFICIENT = decimal.Parse(item.Coefficient);
                    //    }
                    //    catch
                    //    {

                    //        error = true;
                    //        item.Coefficient = "!Sai định dạng kiểu số";
                    //    }
                    //}
                    if (string.IsNullOrWhiteSpace(item.SalaryTotal))
                    {
                        error = true;
                        item.SalaryTotal = "!Không được để trống";
                    }
                    else
                    {
                        try
                        {
                            dtl.SAL_TOTAL = decimal.Parse(item.SalaryTotal);
                        }
                        catch
                        {

                            error = true;
                            item.SalaryTotal = "!Sai định dạng kiểu số";
                        }
                    }
                    if (string.IsNullOrWhiteSpace(item.SalaryPercent))
                    {
                        error = true;
                        item.SalaryPercent = "!Không được để trống";
                    }
                    else
                    {
                        try
                        {
                            dtl.SAL_PERCENT = decimal.Parse(item.SalaryPercent);
                        }
                        catch
                        {
                            error = true;
                            item.SalaryPercent = "!Sai định dạng kiểu số";
                        }
                    }
                    if (error)
                    {
                        error = false;
                        lstError.Add(item);
                    }
                    else
                    {
                        dtl.REF_CODE = guid;
                        dtl.CODE = item.Code;
                        dtl.POS_NAME = item.PosName;
                        dtl.DECISION_NO = item.DecisionNo;
                        dtl.TYPE_NAME = item.TypeName;
                        dtl.SALARY_TYPE_NAME = item.SalaryTypeName;
                        dtl.STATUS_NAME = item.StatusName;
                        dtl.SIGNER_NAME = item.SignName;
                        dtl.SIGNER_POSITION = item.SignPosition;
                        lst.Add(dtl);
                    }
                }

                if (lstError.Count > 0)
                {
                    var pathTemp = _appContext._config["urlTempDecision"];
                    var memoryStream = Template.FillTemp(pathTemp, lstError);
                    return new ResultWithError(memoryStream);
                }
                else
                {
                    if (lst.Count > 0)
                    {
                        await _appContext.WorkingTmps.AddRangeAsync(lst);
                        await _appContext.SaveChangesAsync();
                        // xử lý fill dữ liệu vào master data
                        var ds = QueryData.ExecuteStoreToTable("PKG_IMPORT_DECISION_IMPORT",
                        new
                        {
                            P_REF_CODE = guid,
                        }, true);

                        if (ds.Tables.Count > 0)
                        {
                            ds.Tables[0].TableName = "Data";
                            var pathTemp = _appContext._config["urlTempDecision"];
                            var memoryStream = Template.FillTemplate(pathTemp, ds);
                            return new ResultWithError(memoryStream);
                        }
                    }
                }
                return new ResultWithError(200);
            }
            catch (Exception ex)
            {
                return new ResultWithError(204);
                // throw ex;
            }
        }

        /// <summary>
        /// Portal
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<ResultWithError> PortalWageGetAll()
        {
            try
            {
                var r = await (from p in _appContext.Workings
                               join o in _appContext.OtherLists on p.TYPE_ID equals o.ID
                               where p.EMPLOYEE_ID == _appContext.EmpId && p.STATUS_ID == OtherConfig.STATUS_APPROVE
                               orderby p.EFFECT_DATE descending
                               select new
                               {
                                   Id = p.ID,
                                   No = p.DECISION_NO,
                                   EffectDate = p.EFFECT_DATE,
                                   TypeName = o.NAME
                               }).ToListAsync();
                return new ResultWithError(r);
            }
            catch (Exception ex)
            {
                return new ResultWithError(ex.Message);
            }
        }

        /// <summary>
        /// Portal
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<ResultWithError> PortalWageGetBy(long id)
        {
            try
            {
                var r = await (from p in _appContext.Workings
                               join o in _appContext.OtherLists on p.TYPE_ID equals o.ID
                               join b in _appContext.Organizations on p.ORG_ID equals b.ID
                               join t in _appContext.Positions on p.POSITION_ID equals t.ID
                               from s in _appContext.SalaryScales.Where(x => x.ID == p.SALARY_SCALE_ID).DefaultIfEmpty()
                               from n in _appContext.SalaryRanks.Where(x => x.ID == p.SALARY_RANK_ID).DefaultIfEmpty()
                               from l in _appContext.SalaryLevels.Where(x => x.ID == p.SALARY_LEVEL_ID).DefaultIfEmpty()
                               where p.EMPLOYEE_ID == _appContext.EmpId && p.ID == id
                               select new
                               {
                                   No = p.DECISION_NO,
                                   EffectDate = p.EFFECT_DATE,
                                   TypeName = o.NAME,
                                   OrgName = b.NAME,
                                   PosName = t.NAME,
                                   ScaleName = s.NAME,
                                   RankName = n.NAME,
                                   LevelName = l.NAME,
                                   SalBasic = p.SAL_BASIC,
                                   SalTotal = p.SAL_TOTAL,
                                   SalPercent = p.SAL_PERCENT,
                                   SignName = p.SIGNER_NAME,
                                   SignPos = p.SIGNER_POSITION,
                                   SignDate = p.SIGN_DATE
                               }).ToListAsync();
                return new ResultWithError(r);
            }
            catch (Exception ex)
            {
                return new ResultWithError(ex.Message);
            }
        }
        public async Task<ResultWithError> CalculateExpireShortTemp(long? p_empId, string? p_dDate, long? levelId)
        {
            DateTime? dDate = Convert.ToDateTime(p_dDate.Replace("_", "/"));
            try
            {
                var levelItem = new HU_SALARY_LEVEL();
                if (levelId != null)
                {
                    levelItem = await (from p in _appContext.SalaryLevels where p.ID == levelId select p).FirstOrDefaultAsync();
                }
                int? holdingMonth = 0;
                if (levelItem != null && levelItem.HOLDING_MONTH != null)
                {
                    holdingMonth = levelItem.HOLDING_MONTH;
                }
                var commendItem = await (from p in _appContext.Commends
                                         from ce in _appContext.CommendEmployees.Where(f => f.COMMEND_ID == p.ID)
                                         where ce.EMPLOYEE_ID == p_empId && p.EFFECT_DATE <= dDate && p.STATUS_PAYMENT_ID == OtherConfig.STATUS_APPROVE
                                         select p).FirstOrDefaultAsync();
                int? salaryIncreaseTime = 0;
                if (commendItem != null && commendItem.SALARY_INCREASE_TIME != null)
                {
                    salaryIncreaseTime = commendItem.SALARY_INCREASE_TIME;
                }
                var disciplineItem = await (from p in _appContext.Disciplines
                                            from ce in _appContext.DisciplineEmps.Where(f => f.DISCIPLINE_ID == p.ID)
                                            where ce.EMPLOYEE_ID == p_empId && p.EFFECT_DATE <= dDate && p.STATUS_ID == OtherConfig.STATUS_APPROVE
                                            select p).FirstOrDefaultAsync();
                long? extendSalTime = 0;
                if (disciplineItem != null && disciplineItem.EXTEND_SAL_TIME != null)
                {
                    extendSalTime = disciplineItem.EXTEND_SAL_TIME;
                }
                var months = holdingMonth - salaryIncreaseTime + extendSalTime;
                var str = dDate.Value.AddMonths((int)months).ToString("MM/dd/yyyy");
                object Data = str;
                return new ResultWithError(Data);
            }
            catch (Exception ex)
            {
                return new ResultWithError(ex.Message);
            }
        }
        public async Task<ResultWithError> UpdateUpsal(WorkingInputDTO request)
        {
            try
            {
                var dateUpdate = DateTime.Parse(request.dateCal);
                foreach (long? item in request.ids)
                {
                    var itemInfor = await (from p in _appContext.Workings where p.ID == item select p).FirstOrDefaultAsync();
                    if (itemInfor != null)
                    {

                        itemInfor.REASON_UPSAL = request.reasonUpsal;
                        itemInfor.EFFECT_UPSAL_DATE = dateUpdate;
                        itemInfor.EXPIRE_UPSAL_DATE = dateUpdate;
                        var rs = _appContext.Workings.Update(itemInfor);
                    }
                }
                await _appContext.SaveChangesAsync();
                return new ResultWithError(request);
            }
            catch (Exception ex)
            {
                return new ResultWithError(ex.Message);
            }
        }
        public async Task<ResultWithError> checkDecisionMaster(WorkingInputDTO request)
        {
            try
            {
                var positionItemApp = await (from p in _appContext.Positions where p.ID == request.PositionId select p).FirstOrDefaultAsync();
                if (positionItemApp.INTERIM != null && positionItemApp.MASTER != null)
                {
                    return new ResultWithError(Message.DO_NOT_INSERT_EMPLOYEE_POSITION);
                }
                if (positionItemApp != null && positionItemApp.MASTER != null)
                {
                    var DecisionMaster = await GetLastWorking(positionItemApp.MASTER, request.EffectDate);
                    var checkMasterTerminate = await (from p in _appContext.Terminates where p.EMPLOYEE_ID == positionItemApp.MASTER && p.STATUS_ID == 994 && p.EFFECT_DATE >= DateTime.Now select p).FirstOrDefaultAsync();
                    var checkMasterWorkingFuture = await (from p in _appContext.Workings
                                                          from dt in _appContext.OtherLists.Where(c => c.ID == p.TYPE_ID).DefaultIfEmpty()
                                                          where p.EMPLOYEE_ID == positionItemApp.MASTER && p.EFFECT_DATE >= DateTime.Now && (p.IS_WAGE == 0 || p.IS_WAGE == null) && p.STATUS_ID == 994 && (dt.CODE != null && (dt.CODE.Trim().ToUpper() == "BN" || dt.CODE.Trim().ToUpper() == "DC" || dt.CODE.Trim().ToUpper() == "DDCV" || dt.CODE.Trim().ToUpper() == "DDBN" || dt.CODE.Trim().ToUpper() == "BP"))
                                                          orderby p.EFFECT_DATE descending
                                                          select p).FirstOrDefaultAsync();
                    //var countDecision = (from p in _appContext.Workings where p.EMPLOYEE_ID == positionItemApp.MASTER && p.IS_WAGE != -1 && p.STATUS_ID == 994 select p).ToList().Count();
                    if (checkMasterTerminate != null)
                    {
                        return new ResultWithError(request);
                    }
                    else if (request.EmployeeId == positionItemApp.MASTER)
                    {
                        // case này không cần xét vì vừa là master vừa là nhân viên tạo quyết định
                        return new ResultWithError(204);
                    }
                    else if (checkMasterWorkingFuture != null)
                    {
                        return new ResultWithError(request);
                    }
                    else if (DecisionMaster.Data != null)
                    {
                        var DecisionMasterDto = (WorkingInputDTO)DecisionMaster.Data;
                        if (DecisionMasterDto.TypeCode != null && (DecisionMasterDto.TypeCode.Trim().ToUpper() == "BN" || DecisionMasterDto.TypeCode.Trim().ToUpper() == "DC" || DecisionMasterDto.TypeCode.Trim().ToUpper() == "DDCV" || DecisionMasterDto.TypeCode.Trim().ToUpper() == "DDBN" || DecisionMasterDto.TypeCode.Trim().ToUpper() == "BP"))
                        {
                            return new ResultWithError(request);
                        }
                        else
                        {
                            return new ResultWithError(Message.MASTER_DONOT_DECISION_SWAP);
                        }
                    }
                    else if (positionItemApp.INTERIM != null && positionItemApp.MASTER > 0 && positionItemApp.INTERIM > 0)
                    {
                        return new ResultWithError(Message.DO_NOT_INSERT_EMPLOYEE_POSITION);
                    }
                    else
                    {
                        return new ResultWithError(Message.MASTER_DONOT_DECISION_SWAP);
                    }
                }
                else
                {
                    return new ResultWithError(204);
                }
            }
            catch (Exception ex)
            {
                return new ResultWithError(ex.Message);
            }
        }
    }
}
