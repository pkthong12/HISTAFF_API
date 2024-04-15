using API.All.DbContexts;
using API.Entities;
using Common.Extensions;
using Common.Repositories;
using ProfileDAL.ViewModels;

namespace ProfileDAL.Repositories
{
    public class FormListSysRepository : RepositoryBase<SYS_FORM_LIST>, IFormListSysRepository
    {
        private ProfileDbContext _appContext => (ProfileDbContext)_context;
        public FormListSysRepository(ProfileDbContext context) : base(context)
        {

        }

        /// <summary>
        /// Get Element by FormId(Lấy các thuộc tính theo biểu mẫu)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ResultWithError> GetById(long id)
        {
            try
            {
                // Get ra bieu mau
                var r = await _appContext.FormListSyses.Where(c => c.ID_MAP == id )
                    .Select(d => d.TEXT)
                    .FirstOrDefaultAsync();
                //var zzz = _appContext.FormListSyses.Count();
                // Get list cac phan tu 
                while (id >= 10) id /= 10;
                var x = await QueryData.ExecuteStore<ReferParam>(Procedures.PKG_SYSTEM_FORM_ELEMENT, new
                {
                    P_TYPE_ID = id,
                    P_CUR = QueryData.OUT_CURSOR
                });



                return new ResultWithError(new { Text = r, Elements = x });
            }
            catch (Exception ex)
            {
                return new ResultWithError(ex.Message);
            }
        }

        /// <summary>
        /// Get List Form (Lấy danh sách biểu mẫu để thiết lập)
        /// </summary>
        /// <returns></returns>
        public async Task<ResultWithError> GetList()
        {
            try
            {

                var r = await QueryData.ExecuteStore<FormListSysDTO>
               (Procedures.PKG_SYSTEM_FORM_LIST,
               new
               {
                   P_CUR = QueryData.OUT_CURSOR
               });


                return new ResultWithError(r);
            }
            catch (Exception ex)
            {
                return new ResultWithError(ex.Message);
            }
        }
        
        /// <summary>
        /// Update Form (Cập nhật biểu mẫu)
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<ResultWithError> UpdateAsync(FormListSysDTO param)
        {
            try
            {
                var x = await _appContext.FormListSyses.Where(c => c.ID_MAP == param.IdMap).FirstOrDefaultAsync();
                int? parentId = param.IdMap;
                while (parentId >= 10) parentId /= 10;
                if (x == null)
                {
                    var r = new SYS_FORM_LIST();
                    r.ID_MAP = param.IdMap;
                    r.TEXT = param.Text;
                    _appContext.FormListSyses.Add(r);
                }
                else
                {
                    x.TEXT = param.Text;
                    _appContext.FormListSyses.Update(x);

                }
                _appContext.SaveChanges();
                return new ResultWithError(200);
            }
            catch (Exception ex)
            {
                return new ResultWithError(ex);
            }
        }       

    }
}
