using Common.Paging;
using ProfileDAL.ViewModels;
using Common.Repositories;
using Common.Extensions;
using API.All.DbContexts;
using API.Entities;

namespace ProfileDAL.Repositories
{
    public class SalaryElementSysRepository : RepositoryBase<SYS_PA_ELEMENT>, ISalaryElementSysRepository
    {
        private ProfileDbContext _appContext => (ProfileDbContext)_context;
        public SalaryElementSysRepository(ProfileDbContext context) : base(context)
        {

        }
        /// <summary>
        /// CMS Get All Data for System
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<PagedResult<SalaryElementSysDTO>> GetAll(SalaryElementSysDTO param)
        {
            var queryable = from p in _appContext.SalaryElementSyses
                            from g in _appContext.OtherListFixs.Where(x => x.ID == p.GROUP_ID && x.TYPE == SystemConfig.PA_ELEMENT_GROUP)
                            orderby p.GROUP_ID, p.NAME
                            select new SalaryElementSysDTO
                            {
                                Id = p.ID,
                                Code = p.CODE,
                                Name = p.NAME,
                                GroupName = g.NAME,
                                AreaId = p.AREA_ID,
                                Orders = p.ORDERS,
                                Note = p.NOTE,
                                IsActive = p.IS_ACTIVE,
                                IsSystem = p.IS_SYSTEM,
                                DataType = p.DATA_TYPE
                            };

            if (!string.IsNullOrWhiteSpace(param.Name))
            {
                queryable = queryable.Where(p => p.Name.ToUpper().Contains(param.Name.ToUpper()));
            }
            if (!string.IsNullOrWhiteSpace(param.Code))
            {
                queryable = queryable.Where(p => p.Code.ToUpper().Contains(param.Code.ToUpper()));
            }
            if (!string.IsNullOrWhiteSpace(param.Note))
            {
                queryable = queryable.Where(p => p.Note.ToUpper().Contains(param.Note.ToUpper()));
            }
            if(param.AreaId != null)
            {
                queryable = queryable.Where(p => p.AreaId == param.AreaId);
            }
            if (!string.IsNullOrWhiteSpace(param.GroupName))
            {
                queryable = queryable.Where(p => p.GroupName.ToUpper().Contains(param.GroupName.ToUpper()));
            }


            return await PagingList(queryable, param);
        }
        /// <summary>
        /// CMS Get Detail for System
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>        
        public async Task<ResultWithError> GetById(long id)
        {
            try
            {
                var r = await (from p in _appContext.SalaryElementSyses
                               where p.ID == id
                               select new
                               {
                                   Id = p.ID,
                                   Code = p.CODE,
                                   Name = p.NAME,
                                   AreaId = p.AREA_ID,
                                   GroupId = p.GROUP_ID,
                                   Orders = p.ORDERS,
                                   Note = p.NOTE,
                                   IsActive = p.IS_ACTIVE,
                                   IsSystem = p.IS_SYSTEM,
                                   DataType = p.DATA_TYPE
                               }).FirstOrDefaultAsync();
                return new ResultWithError(r);
            }
            catch (Exception ex)
            {
                return new ResultWithError(ex.Message);
            }
        }
        public async Task<ResultWithError> GetListGroup()
        {
            try
            {
                var r = await (from p in _appContext.OtherListFixs
                               where p.CODE == SystemConfig.PA_ELEMENT_GROUP
                               select new
                               {
                                   Id = p.ID,
                                   Code = p.CODE,
                                   Name = p.NAME,

                               }).ToListAsync();
                return new ResultWithError(r);
            }
            catch (Exception ex)
            {
                return new ResultWithError(ex.Message);
            }
        }
        public async Task<ResultWithError> GetList(int groupid)
        {
            try
            {
                var r = await (from p in _appContext.SalaryElementSyses
                               where p.IS_ACTIVE == true && p.GROUP_ID == groupid
                               select new
                               {
                                   Id = p.ID,
                                   Code = p.CODE,
                                   Name = p.NAME,

                               }).ToListAsync();
                return new ResultWithError(r);
            }
            catch (Exception ex)
            {
                return new ResultWithError(ex.Message);
            }
        }
        public async Task<ResultWithError> GetListCal(int SalaryTypeId)
        {
            try
            {
                var r = await (from p in _appContext.SalaryElementSyses
                               join s in _appContext.SalaryStructSyses on p.ID equals s.ELEMENT_ID
                               where p.IS_ACTIVE == true  && p.IS_ACTIVE == true && s.SALARY_TYPE_ID == SalaryTypeId
                               orderby p.GROUP_ID, p.NAME
                               select new
                               {
                                   Id = p.ID,
                                   Code = p.CODE,
                                   Name = p.NAME
                               }).ToListAsync();
                return new ResultWithError(r);
            }
            catch (Exception ex)
            {
                return new ResultWithError(ex.Message);
            }
        }
        /// <summary>
        /// Create Data for System
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<ResultWithError> CreateAsync(SalaryElementSysInputDTO param)
        {
            try
            {
                // check group exist
                var g = _appContext.OtherListFixs.Where(c => c.ID == param.GroupId && c.TYPE ==  SystemConfig.PA_ELEMENT_GROUP).Count();
                if (g == 0)
                {
                    return new ResultWithError(Message.RECORD_NOT_FOUND);
                }
                // Check code
                var r = _appContext.SalaryElementSyses.Where(x => x.CODE == param.Code.Trim().ToUpper()).Count();
                if (r > 0)
                {
                    return new ResultWithError(Consts.CODE_EXISTS);
                }
               
                // 
                var data = Map(param, new SYS_PA_ELEMENT());
                data.IS_ACTIVE = true;
                var result = await _appContext.SalaryElementSyses.AddAsync(data);
                await _appContext.SaveChangesAsync();
             
                return new ResultWithError(data);
            }
            catch (Exception ex)
            {
                return new ResultWithError(ex.Message);
            }
        }
  
        /// <summary>
        /// CMS Edit Data for System
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<ResultWithError> UpdateAsync(SalaryElementSysInputDTO param)
        {
            try
            {   
                // check code
                var code = _appContext.SalaryElementSyses.Where(x => x.CODE == param.Code.Trim().ToUpper() && x.ID != param.Id).Count();
                if (code > 0)
                {
                    return new ResultWithError(Consts.CODE_EXISTS);
                }
                var r = _appContext.SalaryElementSyses.Where(x => x.ID == param.Id ).FirstOrDefault();
                if (r == null)
                {
                    return new ResultWithError(404);
                }
                //if(r.IS_SYSTEM == true)
                //{
                //    return new ResultWithError("NOT_PERMISSION_EDIT");
                //}
                var data = Map(param, r);
                var result = _appContext.SalaryElementSyses.Update(data);
                await _appContext.SaveChangesAsync();
                return new ResultWithError(200);
            }
            catch (Exception ex)
            {
                return new ResultWithError(ex);
            }
        }

        /// <summary>
        /// CMS Get All Data
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<ResultWithError> GetSalaryElement(GroupElementSysDTO param)
        {
            var data = await (from p in _appContext.SalaryElementSyses
                              where  p.GROUP_ID == param.groupId && p.IS_ACTIVE == true && p.AREA_ID == param.AreaId
                              orderby p.ORDERS
                              select new
                              {
                                  Id = p.ID,
                                  ColName = p.CODE,
                                  Name = p.NAME
                              }).ToListAsync();
            return new ResultWithError(data);
        }
        /// <summary>
        /// CMS Change Status Data
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<ResultWithError> ChangeStatusAsync(List<long> ids)
        {
            try
            {
                foreach (var item in ids)
                {
                    var r = _appContext.SalaryElementSyses.Where(x => x.ID == item).FirstOrDefault();
                    if (r == null)
                    {
                        return new ResultWithError(404);
                    }
                    r.IS_ACTIVE = !r.IS_ACTIVE;
                    var result = _appContext.SalaryElementSyses.Update(r);
                }
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
