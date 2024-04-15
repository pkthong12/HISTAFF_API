using Common.Paging;
using ProfileDAL.ViewModels;
using Common.Repositories;
using Common.Extensions;
using API.All.DbContexts;
using API.Entities;

namespace ProfileDAL.Repositories
{
    public class SalaryStructureSysRepository : RepositoryBase<SYS_SALARY_STRUCTURE>, ISalaryStructureSysRepository
    {
        private ProfileDbContext _appContext => (ProfileDbContext)_context;
        public SalaryStructureSysRepository(ProfileDbContext context) : base(context)
        {

        }
        /// <summary>
        /// CMS Get All Data for System
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<PagedResult<SalaryStructureSysDTO>> GetAll(SalaryStructureSysDTO param)
        {

            try
            {
                var queryable = from p in _appContext.SalaryStructSyses
                                join t in _appContext.SalaryTypeSyses on p.SALARY_TYPE_ID equals t.ID 
                                join e in _appContext.SalaryElementSyses on p.ELEMENT_ID equals e.ID 
                                join g in _appContext.OtherListFixs on e.GROUP_ID equals g.ID
                                where g.TYPE == SystemConfig.PA_ELEMENT_GROUP
                                orderby p.ID
                                select new SalaryStructureSysDTO
                                {
                                    Id = p.ID,
                                    ColName = e.CODE,
                                    Name = e.NAME,
                                    SalaryTypeName = t.NAME,
                                    GroupName = g.NAME,
                                    ElementName = e.NAME,
                                    Orders = p.ORDERS,
                                    IsCalculate = p.IS_CALCULATE,
                                    IsVisible = p.IS_VISIBLE,
                                    IsImport = p.IS_IMPORT,
                                    AreaId = p.AREA_ID,
                                    ElementId = p.ELEMENT_ID

                                };


                if (!string.IsNullOrWhiteSpace(param.Name))
                {
                    queryable = queryable.Where(p => p.Name.ToUpper().Contains(param.Name.ToUpper()));
                }
                if (!string.IsNullOrWhiteSpace(param.ColName))
                {
                    queryable = queryable.Where(p => p.ColName.ToUpper().Contains(param.ColName.ToUpper()));
                }
                if (!string.IsNullOrWhiteSpace(param.SalaryTypeName))
                {
                    queryable = queryable.Where(p => p.SalaryTypeName.ToUpper().Contains(param.SalaryTypeName.ToUpper()));
                }
                if (!string.IsNullOrWhiteSpace(param.GroupName))
                {
                    queryable = queryable.Where(p => p.GroupName.ToUpper().Contains(param.GroupName.ToUpper()));
                }
                if (!string.IsNullOrWhiteSpace(param.ElementName))
                {
                    queryable = queryable.Where(p => p.ElementName.ToUpper().Contains(param.ElementName.ToUpper()));
                }
                if (param.AreaId != null)
                {
                    queryable = queryable.Where(p => p.AreaId == param.AreaId);
                }
                return await PagingList(queryable, param);

            }
            catch (Exception ex)
            {

                throw ex;
            }
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
                var r = await (from p in _appContext.SalaryStructSyses
                               join e in _appContext.SalaryElementSyses on p.ELEMENT_ID equals e.ID into tmp1
                               from e2 in tmp1.DefaultIfEmpty()
                               where p.ID == id
                               select new
                               {
                                   Id = p.ID,
                                   ColName = e2.CODE,
                                   Name = e2.NAME,
                                   SalaryTypeId = p.SALARY_TYPE_ID,
                                   GroupId = e2.GROUP_ID,
                                   ElementId = p.ELEMENT_ID,
                                   Orders = p.ORDERS,
                                   IsCalculate = p.IS_CALCULATE,
                                   IsVisible = p.IS_VISIBLE,
                                   IsImport = p.IS_IMPORT,
                                   AreaId = p.AREA_ID
                               }).FirstOrDefaultAsync();
                return new ResultWithError(r);
            }
            catch (Exception ex)
            {
                return new ResultWithError(ex.Message);
            }
        }
        public async Task<ResultWithError> GetList(int id)
        {
            try
            {
                var r = await (from p in _appContext.SalaryStructSyses
                               join e in _appContext.SalaryElementSyses on p.ELEMENT_ID equals e.ID
                               where p.SALARY_TYPE_ID == id && p.IS_VISIBLE == true
                               orderby p.ORDERS
                               select new
                               {
                                   Name = e.NAME,
                                   Code = e.CODE
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
        public async Task<ResultWithError> CreateAsync(SalaryStructureSysInputDTO param)
        {
            try
            {
                // check đã tạo structure cho bảng lương
                var p = await _appContext.SalaryStructSyses.Where(c => c.ELEMENT_ID == param.ElementId && c.SALARY_TYPE_ID == param.SalaryTypeId).AnyAsync();
                if(p == true)
                {
                    return new ResultWithError(Message.DATA_IS_EXISTS);
                }
                
                var data = Map(param, new SYS_SALARY_STRUCTURE());
                var result = await _appContext.SalaryStructSyses.AddAsync(data);
                await _appContext.SaveChangesAsync();
                return new ResultWithError(data);
            }
            catch (Exception ex)
            {
                return new ResultWithError(ex);
            }
        }
        /// <summary>
        /// CMS Edit Data for System
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<ResultWithError> UpdateAsync(SalaryStructureSysInputDTO param)
        {
            try
            {
                var r = _appContext.SalaryStructSyses.Where(x => x.ID == param.Id ).FirstOrDefault();
                if (r == null)
                {
                    return new ResultWithError(404);
                }

                var data = Map(param, r);
                var result = _appContext.SalaryStructSyses.Update(data);
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
        public async Task<ResultWithError> GetElement(long salaryTypeId)
        {
            var data = await (from p in _appContext.SalaryElementSyses
                              from s in _appContext.SalaryStructSyses.Where(x => x.ELEMENT_ID == p.ID)
                              where s.SALARY_TYPE_ID == salaryTypeId
                              orderby p.ORDERS
                              select new
                              {
                                  Id = p.ID,
                                  ColName = p.CODE,
                                  Name = p.NAME
                              }).ToListAsync();

            return new ResultWithError(data);

        }
    }
}
