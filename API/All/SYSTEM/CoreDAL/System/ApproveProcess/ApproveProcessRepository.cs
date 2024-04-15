using API.All.DbContexts;
using API.Entities;
using Common.Extensions;
using Common.Paging;
using Common.Repositories;
using CoreDAL.ViewModels;

namespace CoreDAL.Repositories
{
    public class ApproveProcessRepository : RepositoryBase<SE_APP_PROCESS>, IApproveProcessRepository
    {
        private CoreDbContext _appContext => (CoreDbContext)_context;

        public ApproveProcessRepository(CoreDbContext context) : base(context)
        {

        }

        public async Task<PagedResult<ApproveProcessDTO>> GetAll(ApproveProcessDTO param, long application)
        {
            var queryable = from p in _appContext.ApproveProcess
                            select new ApproveProcessDTO
                            {
                                Id = p.ID,
                                Name = p.NAME,
                                Actflg = p.ACTFLG,
                                ProcessCode = p.PROCESS_CODE,
                                NumRequest = p.NUMREQUEST,
                                Email = p.EMAIL,
                                IsSendEmail = p.IS_SEND_EMAIL
                            };

            if (param.Name != null)
            {
                queryable = queryable.Where(p => p.Name.ToUpper().Contains(param.Name.ToUpper()));
            }

            if (param.Actflg != null)
            {
                queryable = queryable.Where(p => p.Actflg.ToUpper().Contains(param.Actflg.ToUpper()));
            }

            if (param.ProcessCode != null)
            {
                queryable = queryable.Where(p => p.ProcessCode.ToUpper().Contains(param.ProcessCode.ToUpper()));
            }

            if (param.NumRequest != null)
            {
                queryable = queryable.Where(p => p.NumRequest == param.NumRequest);
            }

            if (param.Email != null)
            {
                queryable = queryable.Where(p => p.Email.ToUpper().Contains(param.Email.ToUpper()));
            }

            return await PagingList(queryable, param);
        }

        public async Task<ResultWithError> GetById(long id, long application)
        {
            var r = await (from p in _appContext.ApproveProcess
                           where p.ID == id
                           select new
                           {
                               Id = p.ID,
                               Name = p.NAME,
                               Actflg = p.ACTFLG,
                               ProcessCode = p.PROCESS_CODE,
                               NumRequest = p.NUMREQUEST,
                               Email = p.EMAIL,
                               IsSendEmail = p.IS_SEND_EMAIL
                           }).FirstOrDefaultAsync();
            return new ResultWithError(r);
        }

        public async Task<ResultWithError> UpdateAsync(ApproveProcessDTO param, long application)
        {
            try
            {
                var r = _appContext.ApproveProcess.Where(x => x.ID == param.Id).FirstOrDefault();
                if (r == null)
                {
                    return new ResultWithError(404);
                }

                r.NAME = param.Name;
                r.ACTFLG = param.Actflg != null ? param.Actflg : "A";
                r.PROCESS_CODE = param.ProcessCode;
                r.NUMREQUEST = param.NumRequest != null ? param.NumRequest : 1;
                r.EMAIL = param.Email;

                _appContext.ApproveProcess.Update(r);
                await _appContext.SaveChangesAsync();
                return new ResultWithError(200);
            }
            catch (Exception ex)
            {
                return new ResultWithError(ex.Message);
            }

        }
    }
}
