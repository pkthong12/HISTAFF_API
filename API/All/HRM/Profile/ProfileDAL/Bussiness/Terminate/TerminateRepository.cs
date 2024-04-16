using Common.Paging;
using ProfileDAL.ViewModels;
using Common.Repositories;
using Common.Extensions;
using API.All.DbContexts;
using CORE.DTO;
using CORE.GenericUOW;
using Microsoft.VisualBasic;
using Microsoft.EntityFrameworkCore;
using API;
using API.DTO;
using API.Controllers.AtDecleareSeniority;
using InsuranceDAL.Repositories;

namespace ProfileDAL.Repositories
{
    public class TerminateRepository : RepositoryBase<HU_TERMINATE>, ITerminateRepository
    {
        private readonly ProfileDbContext _appContext;
        private readonly GenericReducer<HU_TERMINATE, TerminateDTO> genericReducer;
        public TerminateRepository(ProfileDbContext context) : base(context)
        {
            genericReducer = new();
            _appContext = context;
        }

        public async Task<GenericPhaseTwoListResponse<TerminateDTO>> TwoPhaseQueryList(GenericQueryListDTO<TerminateDTO> request)
        {
            //var raw = _appContext.Terminates.AsQueryable();
            //var phase1 = await genericReducer.FirstPhaseReduce(raw, request);

            //if (phase1.ErrorType != EnumErrorType.NONE)
            //{
            //	return new()
            //	{
            //		ErrorType = phase1.ErrorType,
            //		MessageCode = phase1.MessageCode,
            //		ErrorPhase = 1
            //	};
            //}

            //if (phase1.Queryable == null)
            //{
            //	return new()
            //	{
            //		ErrorType = EnumErrorType.CATCHABLE,
            //		MessageCode = CommonMessageCode.ENTITIES_NOT_FOUND,
            //		ErrorPhase = 1
            //	};
            //}

            //var phase1IdsResult = phase1.Queryable.ToList().Aggregate("", (prev, curr) => prev + curr.ID.ToString() + ";");
            //var ids = phase1IdsResult.Split(';');

            var joined = from p in _appContext.Terminates
                         join e in _appContext.Employees on p.EMPLOYEE_ID equals e.ID into et
                         from ett in et.DefaultIfEmpty()
                         join t in _appContext.Positions on ett.POSITION_ID equals t.ID into tt
                         from ttt in tt.DefaultIfEmpty()
                         join j in _appContext.HUJobs on ttt.JOB_ID equals j.ID into jj
                         from jjj in jj.DefaultIfEmpty()
                         join o in _appContext.Organizations on ett.ORG_ID equals o.ID into ot
                         from ott in ot.DefaultIfEmpty()
                         join c in _appContext.Contracts on ett.CONTRACT_ID equals c.ID into tmp
                         from ct in tmp.DefaultIfEmpty()
                         from ftt in _appContext.OtherLists.Where(c => c.ID == p.STATUS_ID).DefaultIfEmpty()
                         orderby p.STATUS_ID, p.EFFECT_DATE descending
                         select new TerminateDTO
                         {
                             Id = p.ID,
                             EmployeeCode = ett.CODE,
                             EmployeeName = ett.Profile!.FULL_NAME,
                             DecisionNo = p.DECISION_NO!,
                             JobOrderNum = (int)(jjj .ORDERNUM ?? 999),
                             OrgId = ett.ORG_ID,
                             OrgName = ott.NAME,
                             ContractNo = ct.CONTRACT_NO,
                             EffectDate = p.EFFECT_DATE,
                             DateEnd = ct.EXPIRE_DATE,
                             DateStart = ct.START_DATE,
                             TerReason = p.TER_REASON!,
                             SignerName = p.SIGNER_NAME!,
                             SignerPosition = p.SIGNER_POSITION!,
                             SignDate = p.SIGN_DATE,
                             StatusId = p.STATUS_ID,
                             StatusName = ftt.NAME!,
                             LastDate = p.LAST_DATE,
                             JoinDate = ett.JOIN_DATE,
                             Seniority = ""
                         };
            var joinedRs = new List<TerminateDTO>();
            foreach (var item in joined)
            {
                if (Information.IsDate(item.JoinDate))
                {
                    var joinDate = item.JoinDate!.Value.Year + "_" + item.JoinDate.Value.Month + "_" + item.JoinDate.Value.Day;
                    var lastDate = item.LastDate!.Value.Year + "_" + item.LastDate.Value.Month + "_" + item.LastDate.Value.Day;
                    var rs = await CalculateSeniority(joinDate, lastDate);
                    item.Seniority = rs.Data.ToString();
                }
                joinedRs.Add(item);
            }

            var phase2 = await genericReducer.SinglePhaseReduce(joinedRs.AsQueryable<TerminateDTO>(), request);
            return phase2;
        }

        /// <summary>
        /// CMS Get All Data
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<PagedResult<TerminateView>> GetAll(TerminateDTO param)
        {
            var queryable = from p in _appContext.Terminates
                            join e in _appContext.Employees on p.EMPLOYEE_ID equals e.ID
                            join t in _appContext.Positions on e.POSITION_ID equals t.ID
                            join o in _appContext.Organizations on e.ORG_ID equals o.ID
                            join c in _appContext.Contracts on e.CONTRACT_ID equals c.ID into tmp
                            from ct in tmp.DefaultIfEmpty()
                            from ftt in _appContext.OtherListTypes.Where(c => c.CODE == SystemConfig.STATUS && c.ID == p.STATUS_ID).DefaultIfEmpty()
                            orderby p.STATUS_ID, p.EFFECT_DATE descending
                            select new TerminateView
                            {
                                Id = p.ID,
                                EmployeeCode = e.CODE,
                                EmployeeName = e.Profile!.FULL_NAME,
                                DecisionNo = p.DECISION_NO!,
                                OrgId = e.ORG_ID,
                                OrgName = o.NAME,
                                ContractNo = ct.CONTRACT_NO,
                                EffectDate = p.EFFECT_DATE,
                                DateEnd = ct.EXPIRE_DATE,
                                DateStart = ct.START_DATE,
                                TerReason = p.TER_REASON!,
                                SignerName = p.SIGNER_NAME!,
                                SignerPosition = p.SIGNER_POSITION!,
                                SignDate = p.SIGN_DATE,
                                StatusId = p.STATUS_ID,
                                StatusName = ftt.NAME,
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

            if (!string.IsNullOrWhiteSpace(param.EmployeeCode))
            {
                queryable = queryable.Where(p => p.EmployeeCode.ToUpper().Contains(param.EmployeeCode.ToUpper()));
            }
            if (!string.IsNullOrWhiteSpace(param.StatusName))
            {
                queryable = queryable.Where(p => p.StatusName.ToUpper().Contains(param.StatusName.ToUpper()));
            }

            if (!string.IsNullOrWhiteSpace(param.EmployeeName))
            {
                queryable = queryable.Where(p => p.EmployeeName.ToUpper().Contains(param.EmployeeName.ToUpper()));
            }

            if (!string.IsNullOrWhiteSpace(param.OrgName))
            {
                queryable = queryable.Where(p => p.OrgName.ToUpper().Contains(param.OrgName.ToUpper()));
            }
            if (!string.IsNullOrWhiteSpace(param.ContractNo))
            {
                queryable = queryable.Where(p => p.ContractNo.ToUpper().Contains(param.ContractNo.ToUpper()));
            }
            if (param.EffectDate != null)
            {
                queryable = queryable.Where(p => p.EffectDate == param.EffectDate);
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
                var r = await (from p in _appContext.Terminates.Where(x => x.ID == id)
                               from e in _appContext.Employees.Where(x => x.ID == p.EMPLOYEE_ID).DefaultIfEmpty()
                               from t in _appContext.Positions.Where(x => x.ID == e.POSITION_ID).DefaultIfEmpty()
                               from o in _appContext.Organizations.Where(x => x.ID == e.ORG_ID).DefaultIfEmpty()
                               from f in _appContext.OtherLists.Where(x => x.ID == p.STATUS_ID).DefaultIfEmpty()

                               select new TerminateDTO
                               {
                                   Id = p.ID,
                                   EmployeeId = p.EMPLOYEE_ID,
                                   EmployeeCode = e.CODE,
                                   EmployeeName = e.Profile!.FULL_NAME,
                                   DecisionNo = p.DECISION_NO!,
                                   PositionName = t.NAME!,
                                   OrgId = e.ORG_ID,
                                   OrgName = o.NAME,
                                   EffectDate = p.EFFECT_DATE,
                                   SendDate = p.SEND_DATE,
                                   LastDate = p.LAST_DATE,
                                   TerReason = p.TER_REASON!,
                                   SignId = p.SIGN_ID,
                                   SignerName = p.SIGNER_NAME!,
                                   SignerPosition = p.SIGNER_POSITION!,
                                   SignDate = p.SIGN_DATE,
                                   StatusId = p.STATUS_ID,
                                   ReasonId = p.REASON_ID,
                                   TypeId = p.TYPE_ID,
                                   IsCalSeveranceAllowance = p.IS_CAL_SEVERANCE_ALLOWANCE,
                                   AvgSalSixMo = p.AVG_SAL_SIX_MO,
                                   SeveranceAllowance = p.SEVERANCE_ALLOWANCE,
                                   PaymentRemainingDay = p.PAYMENT_REMAINING_DAY,
                                   NoticeNo = p.NOTICE_NO,
                                   Attachment = p.ATTACHMENT,
                                   JoinDate = e.JOIN_DATE,
                                   Seniority = "",
                                   FileName = p.FILE_NAME,
                                   IsBlackList = p.IS_BLACK_LIST
                               }).FirstOrDefaultAsync();
                if (Information.IsDate(r.JoinDate))
                {
                    var joinDate = r.JoinDate!.Value.Year + "_" + r.JoinDate.Value.Month + "_" + r.JoinDate.Value.Day;
                    var lastDate = r.LastDate!.Value.Year + "_" + r.LastDate.Value.Month + "_" + r.LastDate.Value.Day;
                    var rs = await CalculateSeniority(joinDate, lastDate);
                    r.Seniority = rs.Data.ToString();
                }
                return new ResultWithError(r);
            }
            catch (Exception ex)
            {
                return new ResultWithError(ex.Message);
            }
        }

        /// <summary>
        /// CMS Get Detail
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>        
        public async Task<ResultWithError> GetTerminateByEmployee(long id)
        {
            try
            {
                var r = await (from p in _appContext.Terminates.Where(x => x.EMPLOYEE_ID == id)
                               from e in _appContext.Employees.Where(x => x.ID == p.EMPLOYEE_ID).DefaultIfEmpty()
                               from t in _appContext.Positions.Where(x => x.ID == e.POSITION_ID).DefaultIfEmpty()
                               from o in _appContext.Organizations.Where(x => x.ID == e.ORG_ID).DefaultIfEmpty()
                               from f in _appContext.OtherLists.Where(x => x.ID == p.STATUS_ID).DefaultIfEmpty()
                               where p.STATUS_ID == OtherConfig.STATUS_APPROVE
                               select new TerminateDTO
                               {
                                   Id = p.ID,
                                   EmployeeId = p.EMPLOYEE_ID,
                                   EmployeeCode = e.CODE,
                                   EmployeeName = e.Profile!.FULL_NAME,
                                   DecisionNo = p.DECISION_NO!,
                                   PositionName = t.NAME!,
                                   OrgId = e.ORG_ID,
                                   OrgName = o.NAME,
                                   ContractNo = "",
                                   ContractTypeName = "",
                                   EffectDate = p.EFFECT_DATE,
                                   DateEnd = null,
                                   DateStart = null,
                                   SendDate = p.SEND_DATE,
                                   LastDate = p.LAST_DATE,
                                   TerReason = p.TER_REASON!,
                                   SignId = p.SIGN_ID,
                                   SignerName = p.SIGNER_NAME!,
                                   SignerPosition = p.SIGNER_POSITION!,
                                   SignDate = p.SIGN_DATE,
                                   StatusId = p.STATUS_ID,
                                   ReasonId = p.REASON_ID,
                                   TypeId = p.TYPE_ID,
                                   IsCalSeveranceAllowance = p.IS_CAL_SEVERANCE_ALLOWANCE,
                                   AvgSalSixMo = p.AVG_SAL_SIX_MO,
                                   SeveranceAllowance = p.SEVERANCE_ALLOWANCE,
                                   PaymentRemainingDay = p.PAYMENT_REMAINING_DAY,
                                   NoticeNo = p.NOTICE_NO,
                                   Attachment = p.ATTACHMENT,
                                   JoinDate = e.JOIN_DATE,
                                   Seniority = "",
                                   FileName = p.FILE_NAME
                               }).ToListAsync();
                var joinedRs = new List<TerminateDTO>();
                foreach (var item in r)
                {
                    if (Information.IsDate(item.JoinDate))
                    {
                        var joinDate = item.JoinDate!.Value.Year + "_" + item.JoinDate.Value.Month + "_" + item.JoinDate.Value.Day;
                        var lastDate = item.LastDate!.Value.Year + "_" + item.LastDate.Value.Month + "_" + item.LastDate.Value.Day;
                        var rs = await CalculateSeniority(joinDate, lastDate);
                        item.Seniority = rs.Data.ToString();
                    }
                    if (Information.IsDate(item.EffectDate))
                    {
                        var infor = (from p in _appContext.Contracts
                                     join l in _appContext.ContractTypes on p.CONTRACT_TYPE_ID equals l.ID
                                     where p.EMPLOYEE_ID == item.EmployeeId && p.START_DATE <= item.EffectDate && p.STATUS_ID == OtherConfig.STATUS_APPROVE
                                     orderby p.START_DATE descending
                                     select new
                                     {
                                         StartDate = p.START_DATE,
                                         ExpireDate = p.EXPIRE_DATE,
                                         ContractTypeName = l.NAME,
                                     }).FirstOrDefault();
                        if (infor != null)
                        {
                            //item.ContractNo = infor.ContractNo;
                            item.ContractTypeName = infor.ContractTypeName;
                            item.DateStart = infor.StartDate;
                            item.DateEnd = infor.ExpireDate;
                        }

                    }
                    joinedRs.Add(item);
                }

                return new ResultWithError(joinedRs);
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
        public async Task<ResultWithError> CreateAsync(TerminateInputDTO param, string sid)
        {
            try
            {
                // Gencode
                //var No = "";

                var appStatusId = _appContext.OtherLists.FirstOrDefault(x => x.CODE == "DD");

                var status = await _appContext.OtherLists.CountAsync(x => x.ID == param.StatusId);
                if (status == 0) return new ResultWithError(Message.STATUS_NOT_EXIST);

                var emp = await _appContext.Employees.CountAsync(x => x.ID == param.EmployeeId);
                if (emp == 0) return new ResultWithError(Message.EMP_NOT_EXIST);

                var signer = await _appContext.Employees.CountAsync(x => x.ID == param.SignId);
                if (param.SignId != null && signer == 0) return new ResultWithError(Message.SIGNER_NOT_EXIST);

                var checkApprove = await _appContext.Terminates.CountAsync(x => x.EMPLOYEE_ID == param.EmployeeId && x.STATUS_ID == appStatusId!.ID);
                if (checkApprove > 0) return new ResultWithError(Message.RECORD_EXIST_APPROVED);

                var data = Map(param, new HU_TERMINATE());
                //data.NO = No;
                data.CREATED_DATE = DateTime.UtcNow;
                data.CREATED_BY = sid;
                var result = await _appContext.Terminates.AddAsync(data);


                // call Approve2()
                bool condition1 = data.STATUS_ID == appStatusId!.ID;
                if (condition1) await Approve2(data);


                await _appContext.SaveChangesAsync();
                param.Id = data.ID;
                return new ResultWithError(param);
            }
            catch (Exception ex)
            {
                return new ResultWithError(ex.Message);
            }
        }
        /// <summary>
        /// CMS Edit Data
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<ResultWithError> UpdateAsync(TerminateInputDTO param, string sid)
        {
            try
            {
                var r = _appContext.Terminates.FirstOrDefault(x => x.ID == param.Id);
                if (r == null) return new ResultWithError(404);

                var appStatusId = _appContext.OtherLists.FirstOrDefault(x => x.CODE == "DD");
                if (r.STATUS_ID == appStatusId!.ID) return new ResultWithError(Message.RECORD_IS_APPROVED);

                var emp = await _appContext.Employees.CountAsync(x => x.ID == param.EmployeeId);
                if (emp == 0) return new ResultWithError(Message.EMP_NOT_EXIST);

                var signer = await _appContext.Employees.CountAsync(x => x.ID == param.SignId);
                if (param.SignId != null && signer == 0) return new ResultWithError(Message.SIGNER_NOT_EXIST);

                var data = Map(param, r);
                data.UPDATED_DATE = DateTime.UtcNow;
                data.UPDATED_BY = sid;
                var result = _appContext.Terminates.Update(data);


                // call Approve2()
                bool condition1 = data.STATUS_ID == appStatusId!.ID;
                if (condition1) await Approve2(data);


                await _appContext.SaveChangesAsync();
                return new ResultWithError(param);
            }
            catch (Exception ex)
            {
                return new ResultWithError(ex.Message);
            }
        }
        //run with hangfire
        public async Task<bool> ApproveList(string sid)
        {
            var query = (from p in _appContext.Terminates
                         from e in _appContext.Employees.Where(f => f.ID == p.EMPLOYEE_ID)
                         where p.STATUS_ID == OtherConfig.STATUS_APPROVE && p.EFFECT_DATE!.Value.Date == DateTime.Now.Date
                         select p).ToList();

            var terminates = new List<HU_TERMINATE>();
            terminates = query.GroupBy(g => g.EMPLOYEE_ID).Select(p => p.OrderByDescending(d => d.EFFECT_DATE).First()).ToList();

            foreach (var p in terminates)
            {
                await Approve(p);
            }
            return true;
        }
        //run with single row
        public async Task<bool> Approve(HU_TERMINATE obj)
        {
            try
            {
                var e = await _appContext.Employees.FirstOrDefaultAsync(x => x.ID == obj.EMPLOYEE_ID);

                var reasonInfor = _appContext.OtherLists.FirstOrDefault(x => x.ID == obj.REASON_ID);

                var workStatusEmp = (from p in _appContext.OtherLists
                                     from dt in _appContext.OtherListTypes.Where(f => f.ID == p.TYPE_ID)
                                     where dt.CODE == "EMP_STATUS"
                                     select p).ToList();

                var workStatusEmpDetail = (from p in _appContext.OtherLists
                                           from dt in _appContext.OtherListTypes.Where(f => f.ID == p.TYPE_ID)
                                           where dt.CODE == "STATUS_DETAIL"
                                           select p).ToList();

                var positionInfor = (from p in _appContext.Positions where p.MASTER == obj.EMPLOYEE_ID || p.INTERIM == obj.EMPLOYEE_ID && obj.EFFECT_DATE!.Value.Date == DateTime.UtcNow.Date select p ).ToList();

                e!.TER_EFFECT_DATE = obj.EFFECT_DATE;
                e.TER_LAST_DATE = obj.LAST_DATE;

                // khi tới ngày hiệu lực thì nhân viên tạo nghỉ việc là MASTER hoặc INTERIM sẽ bị đá ra khỏi ghế
                foreach (var item in positionInfor)
                {
                    if (item.MASTER == obj.EMPLOYEE_ID)
                    {
                        item.MASTER = null;
                    }
                    else if (item.INTERIM == obj.EMPLOYEE_ID)
                    {
                        item.INTERIM = null;
                    }
                    var resultPosition = _appContext.Positions.Update(item);
                }

                //QĐ đã phê duyệt chưa đến thời hạn
                if (obj.EFFECT_DATE!.Value.Date > DateTime.Now.Date && obj.STATUS_ID == OtherConfig.STATUS_APPROVE)
                {
                    var workStatusEmp_resault = new SYS_OTHER_LIST();
                    var workStatusEmpDetail_resault = new SYS_OTHER_LIST();
                    workStatusEmp_resault = (from p in workStatusEmp where p.CODE == "ESW" select p).FirstOrDefault();
                    workStatusEmpDetail_resault = (from p in workStatusEmpDetail where p.CODE == "00289" select p).FirstOrDefault();
                    if(workStatusEmp_resault != null)
                    {
                        e.WORK_STATUS_ID = workStatusEmp_resault.ID;
                    }
                    if (workStatusEmpDetail_resault != null)
                    {
                        e.STATUS_DETAIL_ID = workStatusEmpDetail_resault.ID;
                    }
                }
                else if (obj.EFFECT_DATE!.Value.Date <= DateTime.Now.Date && obj.STATUS_ID == OtherConfig.STATUS_APPROVE) //QĐ phê duyệt đến tg
                {
                    if (reasonInfor != null)
                    {
                        var workStatusEmp_resault = new SYS_OTHER_LIST();
                        var workStatusEmpDetail_resault = new SYS_OTHER_LIST();
                        switch (reasonInfor.CODE!.Trim().ToUpper())
                        {

                            case "976": // Đơn phương chấm dứt hợp đồng lao động
                                //Đã nghỉ việc
                                workStatusEmp_resault = (from p in workStatusEmp where p.CODE == "ESQ" select p).FirstOrDefault();
                                //Đơn phương chấm dứt hợp đồng lao động
                                workStatusEmpDetail_resault = (from p in workStatusEmpDetail where p.CODE == "00010" select p).FirstOrDefault();
                                if (workStatusEmp_resault != null)
                                {
                                    e.WORK_STATUS_ID = workStatusEmp_resault.ID;
                                }

                                if (workStatusEmpDetail_resault != null)
                                {
                                    e.STATUS_DETAIL_ID = workStatusEmpDetail_resault.ID;
                                }
                                break;
                            case "973":// Thỏa thuận chấm dứt hợp đồng lao động
                                //Đã nghỉ việc
                                workStatusEmp_resault = (from p in workStatusEmp where p.CODE == "ESQ" select p).FirstOrDefault();
                                //Thỏa thuận chấm dứt hợp đồng lao động
                                workStatusEmpDetail_resault = (from p in workStatusEmpDetail where p.CODE == "00011" select p).FirstOrDefault();
                                if (workStatusEmp_resault != null)
                                {
                                    e.WORK_STATUS_ID = workStatusEmp_resault.ID;
                                }

                                if (workStatusEmpDetail_resault != null)
                                {
                                    e.STATUS_DETAIL_ID = workStatusEmpDetail_resault.ID;
                                }
                                break;
                            case "974": //Nghỉ hưu
                                //Đã nghỉ việc
                                workStatusEmp_resault = (from p in workStatusEmp where p.CODE == "ESQ" select p).FirstOrDefault();
                                //Nghỉ hưu
                                workStatusEmpDetail_resault = (from p in workStatusEmpDetail where p.CODE == "00012" select p).FirstOrDefault();
                                if (workStatusEmp_resault != null)
                                {
                                    e.WORK_STATUS_ID = workStatusEmp_resault.ID;
                                }

                                if (workStatusEmpDetail_resault != null)
                                {
                                    e.STATUS_DETAIL_ID = workStatusEmpDetail_resault.ID;
                                }
                                break;
                            case "972": //Sa thải
                                // Đã nghỉ việc
                                workStatusEmp_resault = (from p in workStatusEmp where p.CODE == "ESQ" select p).FirstOrDefault();
                                //Sa thải
                                workStatusEmpDetail_resault = (from p in workStatusEmpDetail where p.CODE == "00013" select p).FirstOrDefault();
                                if (workStatusEmp_resault != null)
                                {
                                    e.WORK_STATUS_ID = workStatusEmp_resault.ID;
                                }

                                if (workStatusEmpDetail_resault != null)
                                {
                                    e.STATUS_DETAIL_ID = workStatusEmpDetail_resault.ID;
                                }
                                break;
                            case "00025": //Mất việc làm
                                // Đã nghỉ việc
                                workStatusEmp_resault = (from p in workStatusEmp where p.CODE == "ESQ" select p).FirstOrDefault();
                                //Mất việc làm
                                workStatusEmpDetail_resault = (from p in workStatusEmpDetail where p.CODE == "00014" select p).FirstOrDefault();
                                if (workStatusEmp_resault != null)
                                {
                                    e.WORK_STATUS_ID = workStatusEmp_resault.ID;
                                }

                                if (workStatusEmpDetail_resault != null)
                                {
                                    e.STATUS_DETAIL_ID = workStatusEmpDetail_resault.ID;
                                }
                                break;
                            case "00026": //Bị kết án tù giam
                                // Đã nghỉ việc
                                workStatusEmp_resault = (from p in workStatusEmp where p.CODE == "ESQ" select p).FirstOrDefault();
                                //Bị kết án tù giam
                                workStatusEmpDetail_resault = (from p in workStatusEmpDetail where p.CODE == "00015" select p).FirstOrDefault();
                                if (workStatusEmp_resault != null)
                                {
                                    e.WORK_STATUS_ID = workStatusEmp_resault.ID;
                                }

                                if (workStatusEmpDetail_resault != null)
                                {
                                    e.STATUS_DETAIL_ID = workStatusEmpDetail_resault.ID;
                                }
                                break;
                            case "975": //Chết 
                                //Đã nghỉ việc
                                workStatusEmp_resault = (from p in workStatusEmp where p.CODE == "ESQ" select p).FirstOrDefault();
                                //Chết 
                                workStatusEmpDetail_resault = (from p in workStatusEmpDetail where p.CODE == "00016" select p).FirstOrDefault();
                                if (workStatusEmp_resault != null)
                                {
                                    e.WORK_STATUS_ID = workStatusEmp_resault.ID;
                                }

                                if (workStatusEmpDetail_resault != null)
                                {
                                    e.STATUS_DETAIL_ID = workStatusEmpDetail_resault.ID;
                                }
                                break;
                            case "00027": //Mất tích
                                //Đã nghỉ việc
                                workStatusEmp_resault = (from p in workStatusEmp where p.CODE == "ESQ" select p).FirstOrDefault();
                                //Mất tích
                                workStatusEmpDetail_resault = (from p in workStatusEmpDetail where p.CODE == "00017" select p).FirstOrDefault();
                                if (workStatusEmp_resault != null)
                                {
                                    e.WORK_STATUS_ID = workStatusEmp_resault.ID;
                                }

                                if (workStatusEmpDetail_resault != null)
                                {
                                    e.STATUS_DETAIL_ID = workStatusEmpDetail_resault.ID;
                                }
                                break;
                            case "968":
                                // Đã nghỉ việc
                                workStatusEmp_resault = (from p in workStatusEmp where p.CODE == "ESQ" select p).FirstOrDefault();
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
                            case "00028":
                                // TRƯỜNG HỢP KHÁC
                                workStatusEmp_resault = (from p in workStatusEmp where p.CODE == "00023" select p).FirstOrDefault();
                                //Bị tạm giam
                                workStatusEmpDetail_resault = (from p in workStatusEmpDetail where p.CODE == "00007" select p).FirstOrDefault();
                                if (workStatusEmp_resault != null)
                                {
                                    e.WORK_STATUS_ID = workStatusEmp_resault.ID;
                                }

                                if (workStatusEmpDetail_resault != null)
                                {
                                    e.STATUS_DETAIL_ID = workStatusEmpDetail_resault.ID;
                                }

                                break;
                            default:
                                // Đã nghỉ việc
                                workStatusEmp_resault = (from p in workStatusEmp where p.CODE == "ESQ" select p).FirstOrDefault();
                                if (workStatusEmp_resault != null)
                                {
                                    e.WORK_STATUS_ID = workStatusEmp_resault.ID;
                                }
                                break;
                        }
                    }
                }
                #region comment
                //if (reasonInfor != null)
                //{
                //    var workStatusEmp_resault = new SYS_OTHER_LIST();
                //    var workStatusEmpDetail_resault = new SYS_OTHER_LIST();
                //    switch (reasonInfor.CODE.Trim().ToUpper())
                //    {

                //        case "976": // Đơn phương chấm dứt hợp đồng lao động
                //            //Đã nghỉ việc
                //            workStatusEmp_resault = (from p in workStatusEmp where p.CODE == "ESQ" select p).FirstOrDefault();
                //            //Đơn phương chấm dứt hợp đồng lao động
                //            workStatusEmpDetail_resault = (from p in workStatusEmpDetail where p.CODE == "00010" select p).FirstOrDefault();
                //            if (workStatusEmp_resault != null)
                //            {
                //                e.WORK_STATUS_ID = workStatusEmp_resault.ID;
                //            }

                //            if (workStatusEmpDetail_resault != null)
                //            {
                //                e.STATUS_DETAIL_ID = workStatusEmpDetail_resault.ID;
                //            }
                //            break;
                //        case "973":// Thỏa thuận chấm dứt hợp đồng lao động
                //            //Đã nghỉ việc
                //            workStatusEmp_resault = (from p in workStatusEmp where p.CODE == "ESQ" select p).FirstOrDefault();
                //            //Thỏa thuận chấm dứt hợp đồng lao động
                //            workStatusEmpDetail_resault = (from p in workStatusEmpDetail where p.CODE == "00011" select p).FirstOrDefault();
                //            if (workStatusEmp_resault != null)
                //            {
                //                e.WORK_STATUS_ID = workStatusEmp_resault.ID;
                //            }

                //            if (workStatusEmpDetail_resault != null)
                //            {
                //                e.STATUS_DETAIL_ID = workStatusEmpDetail_resault.ID;
                //            }
                //            break;
                //        case "974": //Nghỉ hưu
                //            //Đã nghỉ việc
                //            workStatusEmp_resault = (from p in workStatusEmp where p.CODE == "ESQ" select p).FirstOrDefault();
                //            //Nghỉ hưu
                //            workStatusEmpDetail_resault = (from p in workStatusEmpDetail where p.CODE == "00012" select p).FirstOrDefault();
                //            if (workStatusEmp_resault != null)
                //            {
                //                e.WORK_STATUS_ID = workStatusEmp_resault.ID;
                //            }

                //            if (workStatusEmpDetail_resault != null)
                //            {
                //                e.STATUS_DETAIL_ID = workStatusEmpDetail_resault.ID;
                //            }
                //            break;
                //        case "972": //Sa thải
                //            // Đã nghỉ việc
                //            workStatusEmp_resault = (from p in workStatusEmp where p.CODE == "ESQ" select p).FirstOrDefault();
                //            //Sa thải
                //            workStatusEmpDetail_resault = (from p in workStatusEmpDetail where p.CODE == "00013" select p).FirstOrDefault();
                //            if (workStatusEmp_resault != null)
                //            {
                //                e.WORK_STATUS_ID = workStatusEmp_resault.ID;
                //            }

                //            if (workStatusEmpDetail_resault != null)
                //            {
                //                e.STATUS_DETAIL_ID = workStatusEmpDetail_resault.ID;
                //            }
                //            break;
                //        case "00025": //Mất việc làm
                //            // Đã nghỉ việc
                //            workStatusEmp_resault = (from p in workStatusEmp where p.CODE == "ESQ" select p).FirstOrDefault();
                //            //Mất việc làm
                //            workStatusEmpDetail_resault = (from p in workStatusEmpDetail where p.CODE == "00014" select p).FirstOrDefault();
                //            if (workStatusEmp_resault != null)
                //            {
                //                e.WORK_STATUS_ID = workStatusEmp_resault.ID;
                //            }

                //            if (workStatusEmpDetail_resault != null)
                //            {
                //                e.STATUS_DETAIL_ID = workStatusEmpDetail_resault.ID;
                //            }
                //            break;
                //        case "00026": //Bị kết án tù giam
                //            // Đã nghỉ việc
                //            workStatusEmp_resault = (from p in workStatusEmp where p.CODE == "ESQ" select p).FirstOrDefault();
                //            //Bị kết án tù giam
                //            workStatusEmpDetail_resault = (from p in workStatusEmpDetail where p.CODE == "00015" select p).FirstOrDefault();
                //            if (workStatusEmp_resault != null)
                //            {
                //                e.WORK_STATUS_ID = workStatusEmp_resault.ID;
                //            }

                //            if (workStatusEmpDetail_resault != null)
                //            {
                //                e.STATUS_DETAIL_ID = workStatusEmpDetail_resault.ID;
                //            }
                //            break;
                //        case "975": //Chết 
                //            //Đã nghỉ việc
                //            workStatusEmp_resault = (from p in workStatusEmp where p.CODE == "ESQ" select p).FirstOrDefault();
                //            //Chết 
                //            workStatusEmpDetail_resault = (from p in workStatusEmpDetail where p.CODE == "00016" select p).FirstOrDefault();
                //            if (workStatusEmp_resault != null)
                //            {
                //                e.WORK_STATUS_ID = workStatusEmp_resault.ID;
                //            }

                //            if (workStatusEmpDetail_resault != null)
                //            {
                //                e.STATUS_DETAIL_ID = workStatusEmpDetail_resault.ID;
                //            }
                //            break;
                //        case "00027": //Mất tích
                //            //Đã nghỉ việc
                //            workStatusEmp_resault = (from p in workStatusEmp where p.CODE == "ESQ" select p).FirstOrDefault();
                //            //Mất tích
                //            workStatusEmpDetail_resault = (from p in workStatusEmpDetail where p.CODE == "00017" select p).FirstOrDefault();
                //            if (workStatusEmp_resault != null)
                //            {
                //                e.WORK_STATUS_ID = workStatusEmp_resault.ID;
                //            }

                //            if (workStatusEmpDetail_resault != null)
                //            {
                //                e.STATUS_DETAIL_ID = workStatusEmpDetail_resault.ID;
                //            }
                //            break;
                //        case "968":
                //            // Đã nghỉ việc
                //            workStatusEmp_resault = (from p in workStatusEmp where p.CODE == "ESQ" select p).FirstOrDefault();
                //            //Trường hợp khác
                //            workStatusEmpDetail_resault = (from p in workStatusEmpDetail where p.CODE == "00009" select p).FirstOrDefault();
                //            if (workStatusEmp_resault != null)
                //            {
                //                e.WORK_STATUS_ID = workStatusEmp_resault.ID;
                //            }

                //            if (workStatusEmpDetail_resault != null)
                //            {
                //                e.STATUS_DETAIL_ID = workStatusEmpDetail_resault.ID;
                //            }

                //            break;
                //        case "00028":
                //            // TRƯỜNG HỢP KHÁC
                //            workStatusEmp_resault = (from p in workStatusEmp where p.CODE == "00023" select p).FirstOrDefault();
                //            //Bị tạm giam
                //            workStatusEmpDetail_resault = (from p in workStatusEmpDetail where p.CODE == "00007" select p).FirstOrDefault();
                //            if (workStatusEmp_resault != null)
                //            {
                //                e.WORK_STATUS_ID = workStatusEmp_resault.ID;
                //            }

                //            if (workStatusEmpDetail_resault != null)
                //            {
                //                e.STATUS_DETAIL_ID = workStatusEmpDetail_resault.ID;
                //            }

                //            break;
                //        default:
                //            // Đã nghỉ việc
                //            workStatusEmp_resault = (from p in workStatusEmp where p.CODE == "ESQ" select p).FirstOrDefault();
                //            if (workStatusEmp_resault != null)
                //            {
                //                e.WORK_STATUS_ID = workStatusEmp_resault.ID;
                //            }
                //            break;
                //    }
                //}
                #endregion
                var result = _appContext.Employees.Update(e);
                var rs = _appContext.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public void ScanApproveTerminate()
        {
            var scanList = _appContext.Terminates.Where(
                x => x.STATUS_ID == OtherConfig.STATUS_APPROVE && x.EFFECT_DATE!.Value.Date == DateTime.Now.Date).ToList();
            scanList.ForEach(async obj =>
            {
                await Approve(obj);
            });

        }
        public async Task<ResultWithError> RemoveAsync(List<long> ids)
        {
            try
            {
                var lst = new List<HU_TERMINATE>();
                foreach (var item in ids)
                {
                    var c = await _appContext.Terminates.Where(x => x.ID == item).FirstOrDefaultAsync();
                    if (c.STATUS_ID == Consts.ACTION_APPROVE)
                    {
                        return new ResultWithError("DATA_IS_APPROVED");
                    }
                    lst.Add(c);
                }
                _appContext.Terminates.RemoveRange(lst);
                await _appContext.SaveChangesAsync();
                return new ResultWithError(200);
            }
            catch (Exception ex)
            {
                return new ResultWithError(ex.Message);
            }
        }
        public async Task<ResultWithError> CalculateSeniority(string? dStart1, string? dStop1)
        {
            double dSoNam = 0;
            double dSoThang = 0;
            double Cal_Month = 0;
            string str = "";
            double Total_Month = 0;
            double totalDays = 0;
            DateTime? dStart = Convert.ToDateTime(dStart1!.Replace("_", "/"));
            DateTime? dStop = Convert.ToDateTime(dStop1!.Replace("_", "/"));
            try
            {
                if (Information.IsDate(dStart))
                {
                    totalDays = (dStop - dStart).Value.TotalDays;
                    // Cal_Month = Math.Round((dStop - dStart).Value.TotalDays / 30, 2);
                    //if (dStop.Value.Day >= dStart.Value.Day)
                    //    Total_Month = Cal_Month;
                    //else
                    //    Total_Month = Cal_Month - 1;

                    if (Information.IsNumeric(Total_Month))
                    {
                        //dSoNam = Math.Round(Total_Month / 12, 0);
                        //dSoThang = Math.Round(Total_Month % 12, 2);
                        //str = (dSoNam > 0 ? dSoNam.ToString() + " Năm " : "") + (Math.Round(dSoThang % 12, 2) > 0 ? Math.Round(dSoThang % 12, 2).ToString() + " Tháng" : "");
                        str = Math.Round(totalDays / 365, 2) + " Năm ";
                    }
                }
                object Data = str;
                return new ResultWithError(Data);
            }
            catch (Exception ex)
            {
                return new ResultWithError(ex.Message);
            }
        }

        //public async Task<ResultWithError> GetPeriodId(long emp, DateTime lastDate)
        //{
        //    if (emp != null && lastDate != null)
        //    {
        //        var query = (from p in _appContext.PaPayrollsheetSums.AsNoTracking()
        //                           .Where(x => x.EMPLOYEE_ID == emp && (x.FROM_DATE <= lastDate || x.TO_DATE >= lastDate))
        //                     select p.CLCHINH9).FirstOrDefault();
        //        var clchinh9 = (decimal)query;
        //    }
        //    return Ok(new FormatedResponse() { InnerBody = clchinh9 });
        //}

        public async Task<bool> Approve2(HU_TERMINATE obj)
        {
            try
            {
                var employee = await _appContext.Employees.FirstOrDefaultAsync(x => x.ID == obj.EMPLOYEE_ID);

                // get id of type "employee has quit"
                var employeeHasQuitId = _appContext.OtherLists.FirstOrDefault(x => x.CODE == "ESQ")!.ID;

                // get id of type "other case"
                var otherCaseId = _appContext.OtherLists.FirstOrDefault(x => x.CODE == "00023")!.ID;

                // get approve id
                var approveId = _appContext.OtherLists.FirstOrDefault(x => x.CODE == "DD")!.ID;

                // get id of type "about to quit"
                var aboutToQuitId = _appContext.OtherLists.FirstOrDefault(x => x.CODE == "00289")!.ID;


                // when you approve record
                // web app sets value for "TER_EFFECT_DATE" and "TER_LAST_DATE"
                employee!.TER_EFFECT_DATE = obj.EFFECT_DATE;
                employee.TER_LAST_DATE = obj.LAST_DATE;


                var positionInfor = (from p in _appContext.Positions
                                     where (p.MASTER == obj.EMPLOYEE_ID
                                            || p.INTERIM == obj.EMPLOYEE_ID)
                                            && obj.EFFECT_DATE!.Value.Date <= DateTime.UtcNow.Date
                                     select p)
                                     .ToList();

                
                // khi tới ngày hiệu lực thì nhân viên tạo nghỉ việc là MASTER hoặc INTERIM sẽ bị đá ra khỏi ghế
                foreach (var item in positionInfor)
                {
                    if (item.MASTER == obj.EMPLOYEE_ID)
                    {
                        item.MASTER = null;
                    }
                    else if (item.INTERIM == obj.EMPLOYEE_ID)
                    {
                        item.INTERIM = null;
                    }
                    var resultPosition = _appContext.Positions.Update(item);
                }

                //QĐ đã phê duyệt chưa đến thời hạn
                if (obj.EFFECT_DATE!.Value.Date > DateTime.Now.Date && obj.STATUS_ID == approveId)
                {
                    // before the deadline
                    // then "WORK_STATUS_ID" is unchanged
                    // but "STATUS_DETAIL_ID" is changed
                    employee.STATUS_DETAIL_ID = aboutToQuitId;
                }
                else if (obj.EFFECT_DATE!.Value.Date <= DateTime.Now.Date && obj.STATUS_ID == approveId) //QĐ phê duyệt đến tg
                {
                    employee.WORK_STATUS_ID = employeeHasQuitId;
                    employee.STATUS_DETAIL_ID = obj.TYPE_ID;
                }
                
                var result = _appContext.Employees.Update(employee);
                
                var rs = _appContext.SaveChanges();
                
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
