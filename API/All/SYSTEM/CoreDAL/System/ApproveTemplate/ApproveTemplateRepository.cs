using API.All.DbContexts;
using API.Entities;
using Common.Extensions;
using Common.Paging;
using Common.Repositories;
using CoreDAL.ViewModels;

namespace CoreDAL.Repositories
{
    public class ApproveTemplateRepository : RepositoryBase<SE_APP_TEMPLATE>, IApproveTemplateRepository
    {
        private CoreDbContext _appContext => (CoreDbContext)_context;

        public ApproveTemplateRepository(CoreDbContext context) : base(context)
        {

        }

        public async Task<PagedResult<ApproveTemplateDTO>> GetApproveTemplate(ApproveTemplateDTO param)
        {
            var queryable = from p in _appContext.ApproveTemplates
                            select new ApproveTemplateDTO
                            {
                                Id = p.ID,
                                TemplateName = p.TEMPLATE_NAME,
                                TemplateType = p.TEMPLATE_TYPE,
                                TemplateOrder = p.TEMPLATE_ORDER,
                                Actflg = p.ACTFLG,
                                CreatedDate = p.CREATED_DATE,
                                CreatedBy = p.CREATED_BY,
                                CreatedLog = p.CREATED_LOG,
                                ModifiedDate = p.MODIFIED_DATE,
                                ModifiedBy = p.MODIFIED_BY,
                                ModifiedLog = p.MODIFIED_LOG,
                                TemplateCode = p.TEMPLATE_CODE,
                                TemplateTypeName = p.TEMPLATE_TYPE == 0 ? "Ban/Phòng" : p.TEMPLATE_TYPE == 1 ? "Nhân viên" : ""
                            };

            return await PagingList(queryable, param);
        }

        public async Task<ResultWithError> GetApproveTemplateById(long id)
        {
            var r = await (from p in _appContext.ApproveTemplates
                           where p.ID == id
                           select new
                           {
                               Id = p.ID,
                               TemplateName = p.TEMPLATE_NAME,
                               TemplateType = p.TEMPLATE_TYPE,
                               TemplateOrder = p.TEMPLATE_ORDER,
                               Actflg = p.ACTFLG,
                               CreatedDate = p.CREATED_DATE,
                               CreatedBy = p.CREATED_BY,
                               CreatedLog = p.CREATED_LOG,
                               ModifiedDate = p.MODIFIED_DATE,
                               ModifiedBy = p.MODIFIED_BY,
                               ModifiedLog = p.MODIFIED_LOG,
                               TemplateCode = p.TEMPLATE_CODE,
                               TemplateTypeName = p.TEMPLATE_TYPE == 0 ? "Ban/Phòng" : p.TEMPLATE_TYPE == 1 ? "Nhân viên" : ""
                           }).FirstOrDefaultAsync();
            return new ResultWithError(r);
        }

        public async Task<ResultWithError> CreateApproveTemplate(ApproveTemplateDTO param)
        {
            try
            {
                var data = Map(param, new SE_APP_TEMPLATE());
                data.ACTFLG = "A";
                data.CREATED_DATE = DateTime.Now;
                data.MODIFIED_DATE = DateTime.Now;
                var result = await _appContext.ApproveTemplates.AddAsync(data);
                await _appContext.SaveChangesAsync();
                return new ResultWithError(param);
            }
            catch (Exception ex)
            {
                return new ResultWithError(ex.Message);
            }
        }

        public async Task<ResultWithError> UpdateApproveTemplate(ApproveTemplateDTO param)
        {
            try
            {
                var r = _appContext.ApproveTemplates.Where(x => x.ID == param.Id).FirstOrDefault();
                if (r == null)
                {
                    return new ResultWithError(404);
                }
                var data = Map(param, r);
                data.MODIFIED_DATE = DateTime.Now;
                var result = _appContext.ApproveTemplates.Update(data);
                await _appContext.SaveChangesAsync();
                return new ResultWithError(200);
            }
            catch (Exception ex)
            {

                return new ResultWithError(ex.Message);
            }
        }

        public async Task<ResultWithError> DeleteApproveTemplate(List<long> ids)
        {
            try
            {
                var query = from p in _appContext.ApproveTemplates where ids.Contains(p.ID) select p;
                foreach (var item in query)
                {
                    _appContext.ApproveTemplates.Remove(item);
                }
                await _appContext.SaveChangesAsync();
                return new ResultWithError(200);
            }
            catch (Exception ex)
            {
                return new ResultWithError(ex.Message);
            }
        }

        public async Task<PagedResult<ApproveTemplateDetailDTO>> GetApproveTemplateDetail(int templateId)
        {
            var queryable = from a in _appContext.ApproveTemplateDetails
                            from e in _appContext.Employees.Where(x => x.ID == a.APP_ID).DefaultIfEmpty()
                            from p in _appContext.Positions.Where(x => x.ID == a.APP_ID).DefaultIfEmpty()
                            where a.TEMPLATE_ID == templateId
                            select new ApproveTemplateDetailDTO
                            {
                                Id = a.ID,
                                TemplateId = a.TEMPLATE_ID,
                                AppLevel = a.APP_LEVEL,
                                AppType = a.APP_TYPE,
                                AppId = a.APP_ID,
                                InformDate = a.INFORM_DATE,
                                InformEmail = a.INFORM_EMAIL,
                                CreatedDate = a.CREATED_DATE,
                                CreatedBy = a.CREATED_BY,
                                CreatedLog = a.CREATED_LOG,
                                ModifiedDate = a.MODIFIED_DATE,
                                ModifiedBy = a.MODIFIED_BY,
                                ModifiedLog = a.MODIFIED_LOG,
                                TitleId = a.TITLE_ID,
                                NodeView = a.NODE_VIEW,
                                AppTypeName = a.APP_TYPE == 0 ? "Quản lý trực tiếp" : a.APP_TYPE == 1 ? "Nhân viên" : a.APP_TYPE == 2 ? "Quản lý gián tiếp" : a.APP_TYPE == 3 ? "HR" : a.APP_TYPE == 4 ? "Tổng giám đốc bộ phận" : a.APP_TYPE == 5 ? "Quản lý trực tiếp của cấp phê duyệt trước đó" : a.APP_TYPE == 6 ? "Vị trí công việc" : "",
                                EmployeeCode = a.APP_TYPE == 1 ? e.CODE : "",
                                EmployeeName = a.APP_TYPE == 1 ? e.Profile.FULL_NAME : "",
                                TitleName = a.APP_TYPE == 6 ? p.NAME : ""
                            };

            return await PagingList(queryable, new ApproveTemplateDetailDTO());
        }

        public async Task<ResultWithError> GetApproveTemplateDetailById(long id)
        {
            var r = await (from a in _appContext.ApproveTemplateDetails
                           from e in _appContext.Employees.Where(x => x.ID == a.APP_ID).DefaultIfEmpty()
                           from p in _appContext.Positions.Where(x => x.ID == a.APP_ID).DefaultIfEmpty()
                           where a.ID == id
                           select new
                           {
                               Id = a.ID,
                               TemplateId = a.TEMPLATE_ID,
                               AppLevel = a.APP_LEVEL,
                               AppType = a.APP_TYPE,
                               AppId = a.APP_ID,
                               InformDate = a.INFORM_DATE,
                               InformEmail = a.INFORM_EMAIL,
                               CreatedDate = a.CREATED_DATE,
                               CreatedBy = a.CREATED_BY,
                               CreatedLog = a.CREATED_LOG,
                               ModifiedDate = a.MODIFIED_DATE,
                               ModifiedBy = a.MODIFIED_BY,
                               ModifiedLog = a.MODIFIED_LOG,
                               TitleId = a.TITLE_ID,
                               NodeView = a.NODE_VIEW,
                               AppTypeName = a.APP_TYPE == 0 ? "Quản lý trực tiếp" : a.APP_TYPE == 1 ? "Nhân viên" : a.APP_TYPE == 2 ? "Quản lý gián tiếp" : a.APP_TYPE == 3 ? "HR" : a.APP_TYPE == 4 ? "Tổng giám đốc bộ phận" : a.APP_TYPE == 5 ? "Quản lý trực tiếp của cấp phê duyệt trước đó" : a.APP_TYPE == 6 ? "Vị trí công việc" : "",
                               EmployeeCode = a.APP_TYPE == 1 ? e.CODE : "",
                               EmployeeName = a.APP_TYPE == 1 ? e.Profile.FULL_NAME : "",
                               TitleName = a.APP_TYPE == 6 ? p.NAME : ""
                           }).FirstOrDefaultAsync();
            return new ResultWithError(r);
        }

        public async Task<ResultWithError> CreateApproveTemplateDetail(ApproveTemplateDetailDTO param)
        {
            try
            {
                var data = Map(param, new SE_APP_TEMPLATE_DTL());
                data.CREATED_DATE = DateTime.Now;
                data.MODIFIED_DATE = DateTime.Now;
                var result = await _appContext.ApproveTemplateDetails.AddAsync(data);
                await _appContext.SaveChangesAsync();
                return new ResultWithError(param);
            }
            catch (Exception ex)
            {
                return new ResultWithError(ex.Message);
            }
        }

        public async Task<ResultWithError> UpdateApproveTemplateDetail(ApproveTemplateDetailDTO param)
        {
            try
            {
                var r = _appContext.ApproveTemplateDetails.Where(x => x.ID == param.Id).FirstOrDefault();
                if (r == null)
                {
                    return new ResultWithError(404);
                }
                var data = Map(param, r);
                data.MODIFIED_DATE = DateTime.Now;
                var result = _appContext.ApproveTemplateDetails.Update(data);
                await _appContext.SaveChangesAsync();
                return new ResultWithError(200);
            }
            catch (Exception ex)
            {

                return new ResultWithError(ex.Message);
            }
        }

        public async Task<ResultWithError> DeleteApproveTemplateDetail(List<long> ids)
        {
            try
            {
                var query = from p in _appContext.ApproveTemplateDetails where ids.Contains(p.ID) select p;
                foreach (var item in query)
                {
                    _appContext.ApproveTemplateDetails.Remove(item);
                }
                await _appContext.SaveChangesAsync();
                return new ResultWithError(200);
            }
            catch (Exception ex)
            {
                return new ResultWithError(ex.Message);
            }
        }

        public async Task<ResultWithError> GetListPosition()
        {
            try
            {
                var data = await (from p in _appContext.Positions
                                  from e in _appContext.Employees.Where(x => x.ID == p.MASTER).DefaultIfEmpty()
                                  //where (e.WORK_STATUS_ID == null || (e.WORK_STATUS_ID != null && (e.WORK_STATUS_ID != 2 || (e.WORK_STATUS_ID == 2 && e.TER_EFFECT_DATE > DateTime.Now)))) && p.IS_ACTIVE == true
                                  orderby p.CREATED_DATE descending
                                  select new
                                  {
                                      Id = p.ID,
                                      name = p.CODE + " - " + p.NAME,
                                      master = e.CODE + " - " + e.Profile.FULL_NAME
                                  }).ToListAsync();
                return new ResultWithError(data);
            }
            catch (Exception ex)
            {
                return new ResultWithError(ex.Message);
            }
        }
    }
}
