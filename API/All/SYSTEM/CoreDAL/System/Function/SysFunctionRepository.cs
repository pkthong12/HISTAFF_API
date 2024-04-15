/*
using Common.Paging;
using CoreDAL.ViewModels;
using Common.Repositories;
using Common.Extensions;
using API.All.DbContexts;
using API.Entities;

namespace CoreDAL.Repositories
{
    public class SysFunctionRepository : RepositoryBase<SYS_FUNCTION>, ISysFunctionRepository
    {
        private CoreDbContext _appContext => (CoreDbContext)_context;
        public SysFunctionRepository(CoreDbContext context) : base(context)
        {

        }

        public async Task<PagedResult<SysFunctionDTO>> GetAll(SysFunctionDTO param)
        {
            var queryable = (from p in _appContext.SysFunctions
                             from g in _appContext.SysGroupFunctions.Where(x => x.ID == p.GROUP_ID)
                             from o in _appContext.SysConfigs.Where(x => x.ID == g.APPLICATION_ID && x.TYPE == SystemConfig.APPLICATION)
                             from m in _appContext.SysModules.Where( c=> c.ID == p.MODULE_ID)
                             select new SysFunctionDTO
                             {
                                 Id = p.ID,
                                 GroupId = p.GROUP_ID,
                                 Code = p.CODE,
                                 Name = p.NAME,
                                 Link = p.LINK,
                                 GroupName = g.NAME,
                                 ModuleName = m.NAME,
                                 AppId = g.APPLICATION_ID,
                                 AppName = o.NAME,
                                 IsActive = p.IS_ACTIVE,
                                 CreatedBy = p.CREATED_BY,
                                 CreatedDate = p.CREATED_DATE,
                                 UpdatedBy = p.UPDATED_BY,
                                 UpdatedDate = p.UPDATED_DATE
                             });
            if (param.Name != null)
            {
                queryable = queryable.Where(p => p.Name.ToUpper().Contains(param.Name.ToUpper()));
            }

            if (param.Code != null)
            {
                queryable = queryable.Where(p => p.Code.ToUpper().Contains(param.Code.ToUpper()));
            }

            if (param.Link != null)
            {
                queryable = queryable.Where(p => p.Link.ToUpper().Contains(param.Link.ToUpper()));
            }

            if (param.GroupName != null)
            {
                queryable = queryable.Where(p => p.GroupName.ToUpper().Contains(param.GroupName.ToUpper()));
            }
            if (param.ModuleName != null)
            {
                queryable = queryable.Where(p => p.ModuleName.ToUpper().Contains(param.ModuleName.ToUpper()));
            }
            if (param.GroupId > 0)
            {
                queryable = queryable.Where(p => p.GroupId == param.GroupId);
            }

            if (param.AppName != null)
            {
                queryable = queryable.Where(p => p.AppName.ToUpper().Contains(param.AppName.ToUpper()));
            }

            if (param.AppId > 0)
            {
                queryable = queryable.Where(p => p.AppId == param.AppId);
            }
            return await PagingList(queryable, param);
        }
        public async Task<ResultWithError> GetById(long id)
        {
            var r = await (from p in _appContext.SysFunctions
                           where p.ID == id
                           select new SysFunctionDTO
                           {
                               Id = p.ID,
                               GroupId = p.GROUP_ID,
                               Code = p.CODE,
                               Name = p.NAME,
                               Link = p.LINK,
                               ModuleId = p.MODULE_ID,

                           }).FirstOrDefaultAsync();
            return new ResultWithError(r);
        }

        public async Task<ResultWithError> CreateAsync(SysFunctionInputDTO param)
        {
            try
            {
                var r = _appContext.SysFunctions.Where(x => x.CODE.ToLower() == param.Code.ToLower()).Count();
                if (r > 0)
                {
                    return new ResultWithError(Consts.CODE_EXISTS);
                }

                var n = _appContext.SysGroupFunctions.Where(x => x.ID == param.GroupId).Count();
                if (n == 0)
                {
                    return new ResultWithError(Consts.GROUP_FUNC_NOT_EXISTS);
                }
                var data = Map(param, new SYS_FUNCTION());
                data.IS_ACTIVE = true;
                var result = await _appContext.SysFunctions.AddAsync(data);
                await _appContext.SaveChangesAsync();
                return new ResultWithError(param);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public async Task<ResultWithError> UpdateAsync(SysFunctionInputDTO param)
        {
            // Check code exists
            var c = _appContext.SysFunctions.Where(x => x.CODE.ToLower() == param.Code.ToLower() && x.ID != param.Id).Count();
            if (c > 0)
            {
                return new ResultWithError(Consts.CODE_EXISTS);
            }
            // Check Group Function exists
            var n = _appContext.SysGroupFunctions.Where(x => x.ID == param.GroupId).Count();
            if (n == 0)
            {
                return new ResultWithError(Consts.GROUP_FUNC_NOT_EXISTS);
            }

            var r = _appContext.SysFunctions.Where(x => x.ID == param.Id).FirstOrDefault();
            if (r == null)
            {
                return new ResultWithError(404);
            }
            var data = Map<SysFunctionInputDTO, SYS_FUNCTION>(param, r);
            var result = _appContext.SysFunctions.Update(data);
            await _appContext.SaveChangesAsync();
            return new ResultWithError(200);
        }
        public async Task<ResultWithError> ChangeStatusAsync(List<long> ids)
        {
            foreach (var item in ids)
            {
                var r = _appContext.SysFunctions.Where(x => x.ID == item).FirstOrDefault();
                if (r == null)
                {
                    return new ResultWithError(404);
                }
                r.IS_ACTIVE = !r.IS_ACTIVE;
                var result = _appContext.SysFunctions.Update(r);
            }
            await _appContext.SaveChangesAsync();
            return new ResultWithError(200);
        }
    }
}
*/