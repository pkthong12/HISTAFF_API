using Common.Paging;
using PayrollDAL.ViewModels;
using Common.Repositories;
using Common.Extensions;
using API.All.DbContexts;
using API.Entities;

namespace PayrollDAL.Repositories
{
    public class SalaryElementRepository : RepositoryBase<PA_ELEMENT>, ISalaryElementRepository
    {
        private PayrollDbContext _appContext => (PayrollDbContext)_context;
        public SalaryElementRepository(PayrollDbContext context) : base(context)
        {

        }
        /// <summary>
        /// CMS Get All Data for System
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<PagedResult<SalaryElementDTO>> GetAll(SalaryElementDTO param)
        {
            var queryable = from p in _appContext.SalaryElements
                            from g in _appContext.ElementGroups.Where(x => x.ID == p.GROUP_ID)
                            
                            orderby p.GROUP_ID, p.NAME
                            select new SalaryElementDTO
                            {
                                Id = p.ID,
                                Code = p.CODE,
                                Name = p.NAME,
                                GroupId = p.GROUP_ID,
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
            if (param.GroupId != null)
            {
                queryable = queryable.Where(p => p.GroupId == param.GroupId);
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
                var r = await (from p in _appContext.SalaryElements
                               where p.ID == id 
                               select new
                               {
                                   Id = p.ID,
                                   Code = p.CODE,
                                   Name = p.NAME,
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
                var r = await (from p in _appContext.ElementGroups
                               where p.IS_ACTIVE == true
                               orderby p.ORDERS
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
        public async Task<ResultWithError> GetList(int groupid)
        {
            try
            {
                var r = await (from p in _appContext.SalaryElements
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
        /// <summary>
        /// Get List All Element
        /// </summary>
        /// <param name="param">EmpId</param>
        /// <returns></returns>
        public async Task<ResultWithError> GetListAll()
        {

            try
            {
                var r = await (from p in _appContext.SalaryElements
                               join a in _appContext.ElementGroups on p.GROUP_ID equals a.ID
                               
                               orderby a.ORDERS, p.NAME
                               select new
                               {
                                   Id = p.ID,
                                   GroupName = a.NAME,
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
        /// Get List Element by Type for Setting Fomular
        /// </summary>
        /// <param name="SalaryTypeId"></param>
        /// <returns></returns>
        public async Task<ResultWithError> GetListCal(int SalaryTypeId)
        {
            try
            {
                var r = await (from p in _appContext.SalaryElements
                                   // join s in _appContext.SalaryStructures on p.ID equals s.ELEMENT_ID
                               where p.IS_ACTIVE == true  //&& s.SALARY_TYPE_ID == SalaryTypeId
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
        public async Task<ResultWithError> CreateAsync(SalaryElementInputDTO param)
        {
            try
            {
                // check group exist
                var g = _appContext.ElementGroups.Where(c => c.ID == param.GroupId).Count();
                if (g == 0)
                {
                    return new ResultWithError(Message.RECORD_NOT_FOUND);
                }
                // Check code
                var r = _appContext.SalaryElements.Where(x => x.CODE == param.Code.Trim().ToUpper() ).Count();
                if (r > 0)
                {
                    return new ResultWithError(Consts.CODE_EXISTS);
                }

                // 
                var data = Map(param, new PA_ELEMENT());
                data.IS_ACTIVE = true;
                var result = await _appContext.SalaryElements.AddAsync(data);
                await _appContext.SaveChangesAsync();
                // Them column
                dynamic a = await QueryData.ExecuteObject(Procedures.PKG_PAYROLL_ADD_ELEMENT_SAL, new
                {
                    
                    P_CODE = data.CODE,
                    P_TYPE_ID = param.DataType,
                    P_CUR = QueryData.OUT_CURSOR
                }, false);
                if (a.STATUS == 400)
                {
                    return new ResultWithError(a.MESSAGE);
                }
                return new ResultWithError(data);
            }
            catch (Exception ex)
            {
                return new ResultWithError(ex.Message);
            }
        }
        /// <summary>
        /// Thêm phần tử lương khi thêm phụ cấp
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<ResultWithError> AllowanceToElement(ReferParam param, int type)
        {
            try
            {
                // Check code
                var r = _appContext.SalaryElements.Where(x => x.CODE == param.Code.Trim().ToUpper() ).Count();
                if (r > 0)
                {
                    return new ResultWithError("ELEMENT_CODE_EXISTS");
                }

                var data = new PA_ELEMENT();
               
                data.NAME = param.Name;
               
                data.ORDERS = 1;
                data.NOTE = "Tạo tự động";
                data.IS_ACTIVE = true;
                data.IS_SYSTEM = true; // không được sửa xóa
                long g = 0;
                if (type == 1) // Phụ cấp
                {
                    data.CODE = param.Code;
                    g = _appContext.ElementGroups.Where(c => c.CODE == Consts.ALLOWANCE).Select(c => c.ID).FirstOrDefault();
                    if (g == 0)
                    {
                        return new ResultWithError("GROUP_ELEMENT_NOT_FOUND");
                    }
                }
                else
                {
                    data.CODE = "SHIFT_" + param.Code;
                    g = _appContext.ElementGroups.Where(c => c.CODE == "NC").Select(c => c.ID).FirstOrDefault();
                    if (g == 0)
                    {
                        return new ResultWithError("GROUP_ELEMENT_NOT_FOUND");
                    }
                }
                data.GROUP_ID = g;
                var result = await _appContext.SalaryElements.AddAsync(data);
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
        public async Task<ResultWithError> UpdateAsync(SalaryElementInputDTO param)
        {
            try
            {
                //// check code
                //var code = _appContext.SalaryElements.Where(x => x.CODE == param.Code.Trim().ToUpper()  && x.ID != param.Id).Count();
                //if (code > 0)
                //{
                //    return new ResultWithError(Consts.CODE_EXISTS);
                //}
                var r = _appContext.SalaryElements.Where(x => x.ID == param.Id ).FirstOrDefault();
                if (r == null)
                {
                    return new ResultWithError(404);
                }
                //if (r.IS_SYSTEM == true)
                //{
                //    return new ResultWithError("NOT_PERMISSION_EDIT");
                //}
                var data = Map(param, r);
                data.CODE = r.CODE;
                var result = _appContext.SalaryElements.Update(data);
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
        public async Task<ResultWithError> GetSalaryElement(long groupId)
        {
            var data = await (from p in _appContext.SalaryElements
                              where p.GROUP_ID == groupId && p.IS_ACTIVE == true
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
        /// get salary is system
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<ResultWithError> GetSalaryElementSys()
        {
            var data = await (from p in _appContext.SalaryElements
                              where p.IS_SYSTEM == false && p.IS_ACTIVE == true
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
                    var r = _appContext.SalaryElements.Where(x => x.ID == item).FirstOrDefault();
                    if (r == null)
                    {
                        return new ResultWithError(404);
                    }
                    r.IS_ACTIVE = !r.IS_ACTIVE;
                    var result = _appContext.SalaryElements.Update(r);
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
