using Common.Paging;
using ProfileDAL.ViewModels;
using Common.Repositories;
using Common.Extensions;
using API.All.DbContexts;
using API.Entities;

namespace ProfileDAL.Repositories
{
    public class ReportRepository : RepositoryBase<HU_REPORT>, IReportRepository
    {
        private ProfileDbContext _appContext => (ProfileDbContext)_context;
        public ReportRepository(ProfileDbContext context) : base(context)
        {

        }
        public async Task<PagedResult<ReportInputDTO>> GetAll(ReportInputDTO param)
        {
            var queryable = from p in _appContext.Reports
                            from x in _appContext.Reports.Where(c => c.ID == p.PARENT_ID).DefaultIfEmpty()
                            select new ReportInputDTO
                            {
                                Id = p.ID,
                                Name = p.NAME,
                                ParentId = p.PARENT_ID,
                                ParentName = x.NAME,

                                Note = p.NOTE
                            };
            if (param.Name != null)
            {
                queryable = queryable.Where(p => p.Name.ToUpper().Contains(param.Name.ToUpper()));
            }




            return await PagingList(queryable, param);
        }
        /// <summary>
        /// Get Data for CMS
        /// </summary>
        public async Task<ResultWithError> GetTreeView()
        {

            var data = await _appContext.Reports.Include(c => c.Childs).ToListAsync();
            return new ResultWithError(data);
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
                var r = await (from p in _appContext.Reports
                               join c in _appContext.Reports on p.PARENT_ID equals c.ID into tmp2
                               from c2 in tmp2.DefaultIfEmpty()
                               select new ReportDTO
                               {
                                   Id = p.ID,
                                   Name = p.NAME,
                                   ParentId = p.PARENT_ID,
                                   ParentName = c2.NAME,

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
        public async Task<ResultWithError> CreateAsync(ReportInputDTO param)
        {
            try
            {

                param.Id = null;
                var data = Map(param, new HU_REPORT());
                var result = await _appContext.Reports.AddAsync(data);
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
        public async Task<ResultWithError> UpdateAsync(ReportInputDTO param)
        {
            try
            {

                //var data = Map<ReportInputDTO, Report>(param, r);
                //var result = _appContext.Reports.Update(data);
                //await _appContext.SaveChangesAsync();
                return new ResultWithError(200);
            }
            catch (Exception ex)
            {

                return new ResultWithError(ex);
            }
        }
        public async Task<ResultWithError> GetList()
        {

            try
            {
                var query = await (from p in _appContext.Reports
                                   join o in _appContext.Reports on p.PARENT_ID equals o.ID into tmp1
                                   from o2 in tmp1.DefaultIfEmpty()
                                   select new
                                   {
                                       Id = p.ID,
                                       Name = p.NAME,
                                       pid = p.PARENT_ID,
                                       pName = o2.NAME,
                                       hasChild = _appContext.Reports.Where(c => c.PARENT_ID == p.ID).Any()
                                   }).ToListAsync();
                return new ResultWithError(query);
            }
            catch (Exception ex)
            {
                return new ResultWithError(ex.Message);
            }
        }
        /// <summary>
        /// Get list report for reportchart
        /// </summary>
        /// <returns></returns>
        public async Task<ResultWithError> GetListReport()
        {
            try
            {
                var query = await  _appContext.Reports.Where(d=> d.PARENT_ID == null).Include(c => c.Childs).Select(f => new { Id = f.ID, Name = f.NAME , Childs = f.Childs}).ToListAsync();
                return new ResultWithError(query);
            }
            catch (Exception ex)
            {
                return new ResultWithError(ex.Message);
            }
        }
        public async Task<ResultWithError> Delete(long id)
        {
            try
            {
                var item = await _appContext.Reports.FirstOrDefaultAsync();
                _appContext.Reports.Remove(item);
                await _appContext.SaveChangesAsync();
                return new ResultWithError(item);
            }
            catch (Exception ex)
            {
                return new ResultWithError(ex.Message);
            }
        }
        public async Task<ResultWithError> ReportIns(ReportInsInputDTO param)
        {
            try
            {
                var data = await QueryData.ExecuteList(Procedures.PKG_REPORT_REPORT_INS, new
                {
                    
                    P_USER_ID = _appContext.IsAdmin == true ? "admin" : _appContext.CurrentUserId,
                    P_ORG_ID = param.OrgId,
                    P_YEAR_ID = param.YearId,
                    P_CODE = param.EMPLOYEE_CODE,
                    P_NAME = param.EMPLOYEE_NAME,
                    P_ORG_NAME = "",
                    P_POS_NAME = "",
                    P_PAGE_NO = param.PageNo,
                    P_PAGE_SIZE = param.PageSize,
                    P_CUR = QueryData.OUT_CURSOR,
                    P_CUR_PAGE = QueryData.OUT_CURSOR
                }, false);


                return new ResultWithError(data);
            }
            catch (Exception ex)
            {
                return new ResultWithError(ex.Message);
            }
        }
        public async Task<ResultWithError> ReportInsByOrg(ReportInsByOrgDTO param)
        {
            try
            {
                var data = await QueryData.ExecuteList(Procedures.PKG_REPORT_REPORT_INS_PERIOD, new
                {
                    
                    P_USER_ID = _appContext.IsAdmin == true ? "admin" : _appContext.CurrentUserId,
                    P_ORG_ID = param.OrgId,
                    P_DATE_START = param.DateStart,
                    P_DATE_END = param.DateEnd,
                    P_CUR = QueryData.OUT_CURSOR
                }, false);


                return new ResultWithError(data);
            }
            catch (Exception ex)
            {
                return new ResultWithError(ex.Message);
            }
        }
        public async Task<PagedResult<ReportEmployeeDTO>> ReportEmployee(ReportEmployeeDTO param)
        {
            try
            {
                var queryable = from p in _appContext.Employees
                                join o in _appContext.Organizations on p.ORG_ID equals o.ID
                                join po in _appContext.Positions on p.POSITION_ID equals po.ID
                                join ot in _appContext.OtherLists on p.Profile.LEARNING_LEVEL_ID equals (int)ot.ID into tmp2
                                from ot2 in tmp2.DefaultIfEmpty()
                                join g in _appContext.OtherLists on p.Profile.GENDER_ID equals (int)g.ID into tmp3
                                from g2 in tmp3.DefaultIfEmpty()
                                join q in _appContext.Terminates.Where(c => c.STATUS_ID == OtherConfig.STATUS_APPROVE) on p.ID equals q.EMPLOYEE_ID into tmp1
                                from q2 in tmp1.DefaultIfEmpty()
                                select new ReportEmployeeDTO
                                {

                                    EmployeeCode = p.CODE,
                                    EmployeeName = p.Profile.FULL_NAME,
                                    PositionName = po.NAME,
                                    OrgId = p.ORG_ID,
                                    OrgName = o.NAME,
                                    BirthDay = p.Profile.BIRTH_DATE,
                                    Education = ot2.NAME,
                                    DateJoin = p.JOIN_DATE,
                                    Language = p.Profile.LANGUAGE,
                                    Qualification = p.Profile.QUALIFICATION_ID,
                                    Gender = g2.NAME,
                                    DateLeave = q2.EFFECT_DATE
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

                queryable = queryable.Where(f => f.DateJoin < param.ToDate && (f.DateLeave == null || f.DateLeave > param.ToDate));


                if (!String.IsNullOrWhiteSpace(param.EmployeeCode))
                {
                    queryable = queryable.Where(p => p.EmployeeCode.ToUpper().Contains(param.EmployeeCode.ToUpper()));
                }
                if (!String.IsNullOrWhiteSpace(param.EmployeeName))
                {
                    queryable = queryable.Where(p => p.EmployeeName.ToUpper().Contains(param.EmployeeName.ToUpper()));
                }
                if (!String.IsNullOrWhiteSpace(param.PositionName))
                {
                    queryable = queryable.Where(p => p.PositionName.ToUpper().Contains(param.PositionName.ToUpper()));
                }
                if (!String.IsNullOrWhiteSpace(param.Education))
                {
                    queryable = queryable.Where(p => p.Education.ToUpper().Contains(param.Education.ToUpper()));
                }
                if (!String.IsNullOrWhiteSpace(param.Language))
                {
                    queryable = queryable.Where(p => p.Language.ToUpper().Contains(param.Language.ToUpper()));
                }
                if (!String.IsNullOrWhiteSpace(param.Qualification))
                {
                    queryable = queryable.Where(p => p.Qualification.ToUpper().Contains(param.Qualification.ToUpper()));
                }
                if (!String.IsNullOrWhiteSpace(param.Gender))
                {
                    queryable = queryable.Where(p => p.Gender.ToUpper().Contains(param.Gender.ToUpper()));
                }

                return await PagingList(queryable, param);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public async Task<ResultWithError> REPORT_HU001(ReportParam param)
        {
            try
            {
                var r = await QueryData.ExecuteStoreObject(Procedures.PKG_REPORT_REPORT_HU001,
                new
                {
                    P_DATE = param.Todate,
                    P_ORG_ID = param.OrgId,
                    P_IS_ADMIN = _appContext.IsAdmin == true ? 1 : 0,
                    P_CUR_ORG = QueryData.OUT_CURSOR,
                    P_CUR_POS = QueryData.OUT_CURSOR,
                    P_CUR_ACA = QueryData.OUT_CURSOR,
                    P_CUR_EMP_MONTH = QueryData.OUT_CURSOR
                }, true);
                return new ResultWithError(r);
            }
            catch (Exception ex)
            {
                return new ResultWithError(ex.Message);
            }
        }
        public async Task<ResultWithError> REPORT_HU009(ReportParam param)
        {
            try
            {
                var r = await QueryData.ExecuteStoreObject(Procedures.PKG_REPORT_REPORT_HU009,
                new
                {
                    P_DATE = param.Todate,
                    P_ORG_ID = param.OrgId,
                    P_IS_ADMIN = _appContext.IsAdmin == true ? 1 : 0,
                    P_CUR_ORG = QueryData.OUT_CURSOR,
                    P_CUR_POS = QueryData.OUT_CURSOR,
                    P_CUR_ACA = QueryData.OUT_CURSOR,
                    P_CUR_EMP_MONTH = QueryData.OUT_CURSOR
                }, true);
                return new ResultWithError(r);
            }
            catch (Exception ex)
            {
                return new ResultWithError(ex.Message);
            }
        }
    }
}
