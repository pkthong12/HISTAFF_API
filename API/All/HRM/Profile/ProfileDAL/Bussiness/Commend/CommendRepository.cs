using Common.Paging;
using ProfileDAL.ViewModels;
using Common.Repositories;
using Common.Extensions;
using API.All.DbContexts;
using API.Entities;

namespace ProfileDAL.Repositories
{
    public class CommendRepository : RepositoryBase<HU_COMMEND>, ICommendRepository
    {
        private ProfileDbContext _appContext => (ProfileDbContext)_context;
        public CommendRepository(ProfileDbContext context) : base(context)
        {

        }
        /// <summary>
        /// CMS Get All Data
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<PagedResult<CommendDTO>> GetAll(CommendDTO param)
        {
            var queryable = from p in _appContext.Commends
                            join o in _appContext.Organizations on p.ORG_ID equals o.ID into tmp1
                            from o2 in tmp1.DefaultIfEmpty()
                            join s in _appContext.OtherListFixs.Where(c => c.TYPE == SystemConfig.SOURCE_COST) on p.SOURCE_COST_ID equals s.ID
                            join f in _appContext.OtherListFixs.Where(c => c.TYPE == SystemConfig.STATUS_APPROVE) on p.STATUS_ID equals f.ID
                            join g in _appContext.OtherListFixs.Where(c => c.TYPE == SystemConfig.OBJECT_COMMEND) on p.COMMEND_OBJ_ID equals g.ID
                            
                            orderby p.STATUS_ID, p.EFFECT_DATE descending
                            select new CommendDTO
                            {
                                Id = p.ID,
                                CommendObjName = g.NAME,
                                SignerName = p.SIGNER_NAME,
                                SignerPosition = p.SIGNER_POSITION,
                                SignDate = p.SIGN_DATE,
                                Employees = (from a in _appContext.CommendEmps
                                             join b in _appContext.Employees on a.EMPLOYEE_ID equals b.ID
                                             where a.COMMEND_ID == p.ID
                                             select new ReferParam
                                             {
                                                 Id = b.ID,
                                                 Name = b.Profile.FULL_NAME,
                                                 Code = b.CODE
                                             }).ToList()
                                               ,
                                OrgName = o2.NAME,
                                OrgId = o2.ID,
                                EffectDate = p.EFFECT_DATE,
                                Year = p.YEAR,
                                No = p.NO,
                                StatusName = f.NAME,
                                StatusId = p.STATUS_ID,
                                IsTax = p.IS_TAX,
                                Reason = p.REASON,
                                CommendType = p.COMMEND_TYPE,
                                Money = p.MONEY,
                                SourceCostName = s.NAME,

                            };

            var orgIds = await QueryData.ExecuteList(Procedures.PKG_COMMON_LIST_ORG,
                    new
                    {
                        P_IS_ADMIN = _appContext.IsAdmin == true ? 1 : 0,
                        P_ORG_ID = param.OrgId,
                        P_CURENT_USER_ID = _appContext.CurrentUserId,
                    }, false);


            List<long?> ids = orgIds.Select(c => (long?)((dynamic)c).ID).ToList();
            if (param.OrgId != null)
            {
                ids.Add(param.OrgId);
            }
            queryable = queryable.Where(p => ids.Contains(p.OrgId));

            if (!string.IsNullOrWhiteSpace(param.OrgName))
            {
                queryable = queryable.Where(p => p.OrgName.ToUpper().Contains(param.OrgName.ToUpper()));
            }
            if (!string.IsNullOrWhiteSpace(param.No))
            {
                queryable = queryable.Where(p => p.No.ToUpper().Contains(param.No.ToUpper()));
            }
            if (!string.IsNullOrWhiteSpace(param.StatusName))
            {
                queryable = queryable.Where(p => p.StatusName.ToUpper().Contains(param.StatusName.ToUpper()));
            }
            if (!string.IsNullOrWhiteSpace(param.CommendType))
            {
                queryable = queryable.Where(p => p.CommendType.ToUpper().Contains(param.CommendType.ToUpper()));
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
                var r = await (from p in _appContext.Commends.Where(c => c.ID == id)
                               from cc in _appContext.OtherListFixs.Where(x => x.ID == p.COMMEND_OBJ_ID && x.TYPE == SystemConfig.OBJECT_COMMEND)
                               
                               select new
                               {
                                   Id = p.ID,
                                   EffectDate = p.EFFECT_DATE,
                                   No = p.NO,
                                   StatusId = p.STATUS_ID,
                                   SignId = p.SIGN_ID,
                                   SignerName = p.SIGNER_NAME,
                                   SignerPosition = p.SIGNER_POSITION,
                                   SignDate = p.SIGN_DATE,
                                   CommendObjId = p.COMMEND_OBJ_ID,
                                   CommendType = p.COMMEND_TYPE,
                                   CommendObjCode = cc.CODE,
                                   SourceCostId = p.SOURCE_COST_ID,
                                   Reason = p.REASON,
                                   Money = p.MONEY,
                                   Year = p.YEAR,
                                   PeriodId = p.PERIOD_ID,
                                   IsTax = p.IS_TAX,
                                   Employees = (from cme in _appContext.CommendEmps
                                                join e in _appContext.Employees on cme.EMPLOYEE_ID equals e.ID
                                                from o in _appContext.Organizations.Where(c => c.ID == e.ORG_ID).DefaultIfEmpty()
                                                from t in _appContext.Positions.Where(c => c.ID == e.POSITION_ID).DefaultIfEmpty()
                                                where cme.COMMEND_ID == id
                                                select new
                                                {
                                                    EmployeeId = p.ID,
                                                    EmployeeCode = e.CODE,
                                                    EmployeeName = e.Profile.FULL_NAME,
                                                    OrgName = o.NAME,
                                                    PositionName = t.NAME,
                                                    WorkStatusId = e.WORK_STATUS_ID
                                                }).ToList(),
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
        public async Task<ResultWithError> CreateAsync(CommendInputDTO param)
        {
            try
            {
                var r2 = _appContext.Commends.Where(x => x.NO == param.No ).Count();
                if (r2 > 0)
                {
                    return new ResultWithError(Consts.CODE_EXISTS);
                }
                // Gencode
                //var CommendNo = "";
                var obj = _appContext.OtherListFixs.Where(c => c.ID == param.CommendObjId && c.TYPE == SystemConfig.OBJECT_COMMEND).FirstOrDefault();
                if (obj == null)
                {
                    return new ResultWithError(Message.OBJECT_COMMEND_NOTE_EXIST);
                }
                if (obj.CODE == OtherListConst.OBJECT_ORG)
                {
                    var org = await _appContext.Organizations.Where(c => c.ID == param.OrgId).CountAsync();
                    if (org == 0)
                    {
                        return new ResultWithError(Message.ORG_NOT_EXIST);
                    }
                }

                var data = Map(param, new HU_COMMEND());
                //data.NO = CommendNo;
                await _appContext.Database.BeginTransactionAsync();
                var result = await _appContext.Commends.AddAsync(data);
                await _appContext.SaveChangesAsync();
                if (obj.CODE == OtherListConst.OBJECT_EMP && param.Emps != null && param.Emps.Count > 0)
                {
                    var lst = new List<HU_COMMEND_EMP>();
                    foreach (var item in param.Emps)
                    {
                        var emp = (from p in _appContext.Employees
                                   where p.ID == item
                                   select new
                                   {
                                       OrgId = p.ORG_ID,
                                       PosId = p.POSITION_ID
                                   }).FirstOrDefault();
                        var r = new HU_COMMEND_EMP();
                        r.EMPLOYEE_ID = item;
                        r.COMMEND_ID = data.ID;
                        r.ORG_ID = emp.OrgId;
                        r.POS_ID = emp.PosId;
                        lst.Add(r);
                    }
                    await _appContext.CommendEmps.AddRangeAsync(lst);
                }
                await _appContext.SaveChangesAsync();
                _appContext.Database.CommitTransaction();
                return new ResultWithError(data);
            }
            catch (Exception ex)
            {
                _appContext.Database.RollbackTransaction();
                return new ResultWithError(ex.Message);
            }
        }
        /// <summary>
        /// CMS Edit Data
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<ResultWithError> UpdateAsync(CommendInputDTO param)
        {
            try
            {
                if (param.StatusId == OtherConfig.STATUS_APPROVE && !_appContext.IsAdmin)
                {
                    return new ResultWithError(Message.RECORD_IS_APPROVED);
                }
                var r = _appContext.Commends.Where(x => x.ID == param.Id ).FirstOrDefault();
                if (r == null)
                {
                    return new ResultWithError(404);
                }
                var obj = _appContext.OtherListFixs.Where(c => c.ID == param.CommendObjId && c.TYPE == SystemConfig.OBJECT_COMMEND).FirstOrDefault();
                if (obj == null)
                {
                    return new ResultWithError(Message.OBJECT_COMMEND_NOTE_EXIST);
                }
                if (obj.CODE == OtherListConst.OBJECT_ORG)
                {
                    var org = await _appContext.Organizations.Where(c => c.ID == param.OrgId).CountAsync();
                    if (org == 0)
                    {
                        return new ResultWithError(Message.ORG_NOT_EXIST);
                    }
                }
                else if (obj.CODE == OtherListConst.OBJECT_EMP)
                {
                    param.OrgId = null;
                }
                var data = Map(param, r);
                var result = _appContext.Commends.Update(data);
                var dtl = await _appContext.CommendEmps.Where(x => x.COMMEND_ID == param.Id).ToListAsync();
                if (dtl.Count > 0)
                {
                    _appContext.CommendEmps.RemoveRange(dtl);
                }
                if (obj.CODE == OtherListConst.OBJECT_EMP && param.Emps != null && param.Emps.Count > 0)
                {
                    foreach (var item in param.Emps)
                    {
                        var emp = (from p in _appContext.Employees
                                   where p.ID == item
                                   select new
                                   {
                                       OrgId = p.ORG_ID,
                                       PosId = p.POSITION_ID
                                   }).FirstOrDefault();

                        var cm = new HU_COMMEND_EMP();
                        cm.EMPLOYEE_ID = item;
                        cm.COMMEND_ID = r.ID;
                        cm.ORG_ID = emp.OrgId;
                        cm.POS_ID = emp.PosId;
                        await _appContext.CommendEmps.AddAsync(cm);
                    }
                }
                await _appContext.SaveChangesAsync();
                return new ResultWithError(200);
            }
            catch (Exception ex)
            {
                return new ResultWithError(ex);
            }
        }

        /// <summary>
        /// CMS Change Status Data
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<ResultWithError> RemoveAsync(long id)
        {
            try
            {
                var r = _appContext.Commends.Where(x => x.ID == id ).FirstOrDefault();
                if (r == null)
                {
                    return new ResultWithError(404);
                }
                // Kiểm tra xem nếu dữ liệu đang sửa ở trạng thái phê duyệt thì không cho xóa
                if (r.STATUS_ID == OtherConfig.STATUS_APPROVE)
                {
                    return new ResultWithError("RECORD_IS_APPROVED");
                }
                // Remove Detail
                var dtl = _appContext.CommendEmps.Where(x => x.COMMEND_ID == r.ID).ToList();
                _appContext.CommendEmps.RemoveRange(dtl);
                // Remove Master
                _appContext.Commends.Remove(r);
                await _appContext.SaveChangesAsync();
                return new ResultWithError(200);
            }
            catch (Exception ex)
            {
                return new ResultWithError(ex.Message);
            }
        }

        public async Task<ResultWithError> OpenStatus(long id)
        {
            try
            {
                var r = _appContext.Commends.Where(x => x.ID == id ).FirstOrDefault();
                if (r == null)
                {
                    return new ResultWithError(404);
                }

                // Kiểm tra xem nếu dữ liệu đang sửa ở trạng thái chờ phê duyệt thì không cho mở chờ phê duyệt
                if (r.STATUS_ID == OtherConfig.STATUS_WAITING)
                {
                    return new ResultWithError("RECORD_IS_WAITED");
                }
                r.STATUS_ID = OtherConfig.STATUS_WAITING;
                var result = _appContext.Commends.Update(r);
                await _appContext.SaveChangesAsync();
                return new ResultWithError(200);
            }
            catch (Exception ex)
            {
                return new ResultWithError(ex.Message);
            }
        }

        public async Task<ResultWithError> Approve(long id)
        {
            try
            {
                var r = _appContext.Commends.Where(x => x.ID == id ).FirstOrDefault();
                if (r == null)
                {
                    return new ResultWithError(404);
                }

                // Kiểm tra xem nếu dữ liệu đang sửa ở trạng thái phê duyệt thì không cho  phê duyệt
                if (r.STATUS_ID == OtherConfig.STATUS_APPROVE)
                {
                    return new ResultWithError("RECORD_IS_APPROVED");
                }
                r.STATUS_ID = OtherConfig.STATUS_APPROVE;
                var result = _appContext.Commends.Update(r);
                await _appContext.SaveChangesAsync();
                return new ResultWithError(200);
            }
            catch (Exception ex)
            {
                return new ResultWithError(ex.Message);
            }
        }
        /// <summary>
        /// Portal Get All
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>        
        public async Task<ResultWithError> PortalGetAll()
        {
            try
            {
                var r = await (from p in _appContext.Commends
                               join d in _appContext.CommendEmps on p.ID equals d.COMMEND_ID
                               where d.EMPLOYEE_ID == _appContext.EmpId
                               orderby p.EFFECT_DATE descending
                               select new
                               {
                                   Id = p.ID,
                                   No = p.NO,
                                   EffectDate = p.EFFECT_DATE
                               }).ToListAsync();
                return new ResultWithError(r);
            }
            catch (Exception ex)
            {
                return new ResultWithError(ex.Message);
            }
        }
        /// <summary>
        /// Portal Get All
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>        
        public async Task<ResultWithError> PortalGetBy(long id)
        {
            try
            {
                var r = await (from p in _appContext.Commends
                               join d in _appContext.CommendEmps on p.ID equals d.COMMEND_ID
                               from o in _appContext.OtherListFixs.Where(x => x.ID == p.SOURCE_COST_ID && x.TYPE == SystemConfig.SOURCE_COST).DefaultIfEmpty()
                               where d.EMPLOYEE_ID == _appContext.EmpId && p.ID == id
                               orderby p.EFFECT_DATE descending
                               select new
                               {
                                   No = p.NO,
                                   EffectDate = p.EFFECT_DATE,
                                   CommendType = p.COMMEND_TYPE,
                                   SourceName = o.NAME,
                                   Reason = p.REASON,
                                   Money = p.MONEY,
                                   SignerName = p.SIGNER_NAME,
                                   SignerPos = p.SIGNER_POSITION,
                                   SignerDate = p.SIGN_DATE,
                                   Year = p.YEAR,
                                   Period = "",
                                   IsTax = p.IS_TAX
                               }).FirstOrDefaultAsync();
                return new ResultWithError(r);
            }
            catch (Exception ex)
            {
                return new ResultWithError(ex.Message);
            }
        }

    }
}
