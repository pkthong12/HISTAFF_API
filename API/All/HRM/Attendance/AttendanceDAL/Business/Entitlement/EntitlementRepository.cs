using Common.Paging;
using AttendanceDAL.ViewModels;
using Common.Repositories;
using Common.Extensions;
using Common.EPPlus;
using API.All.DbContexts;
using API.Entities;
using CORE.DTO;
using CORE.GenericUOW;

namespace AttendanceDAL.Repositories
{
    public class EntitlementRepository : RepositoryBase<AT_ENTITLEMENT>, IEntitlementRepository
    {
        private AttendanceDbContext _appContext;
        private readonly GenericReducer<AT_ENTITLEMENT, AtEntitlementDTO> genericReducer;
        public EntitlementRepository(AttendanceDbContext context) : base(context)
        {
            _appContext = context;
            genericReducer = new();
        }
        public async Task<GenericPhaseTwoListResponse<AtEntitlementDTO>> SinglePhaseQueryList(GenericQueryListDTO<AtEntitlementDTO> request)
        {
            var joined = from p in _appContext.Entitlements
                         from e in _appContext.Employees.Where(f => p.EMPLOYEE_ID == f.ID)
                         from t in _appContext.Positions.Where(f => p.POSITION_ID == f.ID).DefaultIfEmpty()
                         from j in _appContext.Jobs.AsNoTracking().Where(x => x.ID == t.JOB_ID).DefaultIfEmpty()
                         from o in _appContext.Organizations.Where(f => p.ORG_ID == f.ID).DefaultIfEmpty()
                         select new AtEntitlementDTO
                         {
                             Id = p.ID,
                             EmployeeCode = e.CODE,
                             EmployeeName = e.Profile!.FULL_NAME,
                             PositionName = t.NAME,
                             JobOrderNum = (int)(j.ORDERNUM ?? 999),
                             OrgId = p.ORG_ID,
                             OrgName = o.NAME,
                             Year = p.YEAR,
                             JoinDate=p.JOIN_DATE,
                             PrevHave=p.PREV_HAVE,
                             PrevUsed=p.PREV_USED,
                             PrevtotalHave=p.PREVTOTAL_HAVE,
                             CurUsed1= p.CUR_USED1 + p.PREV_USED1,
                             CurUsed2= p.CUR_USED2 + p.PREV_USED2,
                             CurUsed3= p.CUR_USED3 + p.PREV_USED3,
                             CurUsed4= p.CUR_USED4 + p.PREV_USED4,
                             CurUsed5= p.CUR_USED5 + p.PREV_USED5,
                             CurUsed6= p.CUR_USED6 + p.PREV_USED6,
                             CurUsed7= p.CUR_USED7 + p.PREV_USED7,
                             CurUsed8= p.CUR_USED8 + p.PREV_USED8,
                             CurUsed9 = p.CUR_USED9 + p.PREV_USED9,
                             CurUsed10 = p.CUR_USED10 + p.PREV_USED10,
                             CurUsed11 = p.CUR_USED11 + p.PREV_USED11,
                             CurUsed12 = p.CUR_USED12 + p.PREV_USED12,
                             Expiredate=p.EXPIREDATE,
                             Seniority=p.SENIORITY,
                             SeniorityHave=p.SENIORITYHAVE,
                             SeniorityMonth=p.SENIORITY_MONTH,
                             PeriodId = p.PERIOD_ID,
                             QpMonthSum=p.QP_MONTH_SUM,
                             QpYear=p.QP_YEAR,
                             TotalHave=p.TOTAL_HAVE,
                             CurUsed=p.CUR_USED,
                             CurHave=p.CUR_HAVE,
                             QpYearXUsed=p.QP_YEARX_USED,
                             QpYearXHave=p.QP_YEARX_HAVE,
                             QpStandard=p.QP_STANDARD
                         };
            var phase2 = await genericReducer.SinglePhaseReduce(joined, request);
            return phase2;
        }
        public async Task<ResultWithError> Calculate(AtEntitlementInputDTO request)
        {
            try
            {
                
                var x = new
                {
                    P_USER_ID = _appContext.CurrentUserId,
                    P_ORG_ID = request.OrgId,
                    P_ISDISSOLVE = -1,
                    P_PERIOD_ID = request.PeriodId
                };
                var data = QueryData.ExecuteStoreToTable(Procedures.PKG_ATTENDANCE_BUSINESS_CALL_ENTITLEMENT,
                    x, false);
                if (Convert.ToDouble(data.Tables[0].Rows[0][0]) > 0)
                {
                    return new ResultWithError(200, data.Tables[0].Rows[0][0]);
                }
                else
                {

                    return new ResultWithError(400);
                }
            }
            catch (Exception ex)
            {
                return new ResultWithError(ex.Message);
            }
        }
        
    }
}
