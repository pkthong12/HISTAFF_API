/* Old Core.vn class - DEPRICATED */

using Common.Paging;
using ProfileDAL.ViewModels;
using Common.Repositories;
using Common.Extensions;
using System.Data;
using API.All.DbContexts;
using API.Entities;

namespace ProfileDAL.Repositories
{
    public class OrganizationRepository : RepositoryBase<HU_ORGANIZATION>, IOrganizationRepository
    {
        private ProfileDbContext _appContext => (ProfileDbContext)_context;
        public OrganizationRepository(ProfileDbContext context) : base(context)
        {

        }
        public async Task<PagedResult<OrganizationInputDTO>> GetAll(OrganizationInputDTO param)
        {
            var queryable = from p in _appContext.Organizations
                            from x in _appContext.Organizations.Where(c => c.ID == p.PARENT_ID).DefaultIfEmpty()
                            
                            select new OrganizationInputDTO
                            {
                                Id = p.ID,
                                Code = p.CODE,
                                Name = p.NAME,
                                ParentId = p.PARENT_ID,
                                ParentName = x.NAME,
                                MngId = p.MNG_ID,
                                FoundationDate = p.FOUNDATION_DATE,
                                DissolveDate = p.DISSOLVE_DATE,
                                Phone = p.PHONE,
                                Fax = p.FAX,
                                Address = p.ADDRESS,
                                BusinessNumber = p.BUSINESS_NUMBER,
                                BusinessDate = p.BUSINESS_DATE,
                                TaxCode = p.TAX_CODE,
                                Note = p.NOTE
                            };
            if (param.Name != null)
            {
                queryable = queryable.Where(p => p.Name.ToUpper().Contains(param.Name.ToUpper()));
            }

            if (param.Code != null)
            {
                queryable = queryable.Where(p => p.Code.ToUpper().Contains(param.Code.ToUpper()));
            }

            if (param.Note != null)
            {
                queryable = queryable.Where(p => p.Note.ToUpper().Contains(param.Note.ToUpper()));
            }


            return await PagingList(queryable, param);
        }
        /// <summary>
        /// Get Data for CMS
        /// </summary>
        public async Task<ResultWithError> GetTreeView()
        {

            var data = await _appContext.Organizations.Include(c => c.PARENT_ID).ToListAsync();
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
                var r = await (from p in _appContext.Organizations
                               join e in _appContext.Employees on p.MNG_ID equals e.ID into tmp1
                               from e2 in tmp1.DefaultIfEmpty()
                               join c in _appContext.Organizations on p.PARENT_ID equals c.ID into tmp2
                               from c2 in tmp2.DefaultIfEmpty()
                               join t in _appContext.Positions on e2.POSITION_ID equals t.ID into tmp3
                               from t2 in tmp3.DefaultIfEmpty()
                               where p.ID == id 
                               select new OrganizationDTO
                               {
                                   Id = p.ID,
                                   Code = p.CODE,
                                   Name = p.NAME,
                                   ParentId = p.PARENT_ID,
                                   MngId = p.MNG_ID,
                                   MngName = e2.Profile.FULL_NAME,
                                   FoundationDate = p.FOUNDATION_DATE,
                                   DissolveDate = p.DISSOLVE_DATE,
                                   Phone = p.PHONE,
                                   Fax = p.FAX,
                                   Address = p.ADDRESS,
                                   BusinessNumber = p.BUSINESS_NUMBER,
                                   BusinessDate = p.BUSINESS_DATE,
                                   TaxCode = p.TAX_CODE,
                                   Note = p.NOTE,
                                   ParentName = c2.NAME,
                                   PosName = t2.NAME,
                                   Avatar = e2.Profile.AVATAR == null ? "/assets/images/no-img.png" : e2.Profile.AVATAR
                               }).FirstOrDefaultAsync();
                return new ResultWithError(r);
            }
            catch (Exception ex)
            {
                return new ResultWithError(ex.Message);
            }
        }

        /* Old Core.vn method - depricated */
        public async Task<ResultWithError> CreateAsync(OrganizationInputDTO param)
        {
            try
            {
                var r = _appContext.Organizations.Where(x => x.CODE == param.Code ).Count();
                if (r > 0)
                {
                    return new ResultWithError(Consts.CODE_EXISTS);
                }
                param.Id = null;
                var data = Map(param, new HU_ORGANIZATION());
                var result = await _appContext.Organizations.AddAsync(data);
                await _appContext.SaveChangesAsync();
                param.Id = data.ID;
                // add phân quyền
                var o = new SYS_USER_ORG()
                {
                    ORG_ID = data.ID,
                    USER_ID = _appContext.CurrentUserId
                };
                o.ORG_ID = data.ID;
                o.USER_ID = _appContext.CurrentUserId;
                await _appContext.UserOrganis.AddAsync(o);
                await _appContext.SaveChangesAsync();
                return new ResultWithError(param);
            }
            catch (Exception ex)
            {
                return new ResultWithError(ex.Message);
            }
        }
        

        public async Task<ResultWithError> UpdateAsync(OrganizationInputDTO param)
        {
            try
            {
                // check code
                var c = _appContext.Organizations.Where(x => x.CODE.ToLower() == param.Code.ToLower() && x.ID != param.Id ).Count();
                if (c > 0)
                {
                    return new ResultWithError(Consts.CODE_EXISTS);
                }
                var r = _appContext.Organizations.Where(x => x.ID == param.Id ).FirstOrDefault();
                if (r == null)
                {
                    return new ResultWithError(404);
                }
                var data = Map(param, r);
                var result = _appContext.Organizations.Update(data);
                await _appContext.SaveChangesAsync();
                return new ResultWithError(200);
            }
            catch (Exception ex)
            {

                return new ResultWithError(ex);
            }
        }

        public async Task<ResultWithError> SortAsync(OrganizationInputDTO param)
        {
            try
            {
                var r = _appContext.Organizations.Where(x => x.ID == param.Id ).FirstOrDefault();
                if (r == null)
                {
                    return new ResultWithError(404);
                }
                r.PARENT_ID = param.ParentId;
                var result = _appContext.Organizations.Update(r);
                await _appContext.SaveChangesAsync();
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
                var query = await (from p in _appContext.Organizations
                                   join o in _appContext.Organizations on p.PARENT_ID equals o.ID into tmp1
                                   join e in _appContext.Employees on p.MNG_ID equals e.ID into tmp2
                                   from o2 in tmp1.DefaultIfEmpty()
                                   from e2 in tmp2.DefaultIfEmpty()
                                   
                                   orderby p.ID ascending
                                   select new
                                   {
                                       Id = p.ID,
                                       Code = p.CODE,
                                       Name = p.NAME,
                                       pid = p.PARENT_ID,
                                       pName = o2.NAME,
                                       orgManager = e2.Profile.FULL_NAME,
                                       Image = e2.Profile.AVATAR == null ? SystemConfig.NO_IMG : e2.Profile.AVATAR,
                                       hasChild = _appContext.Organizations.Where(c => c.PARENT_ID == p.ID).Any(),
                                       Expand = 1
                                   }).ToListAsync();
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
                var r = await _appContext.Employees.Where(x => x.ORG_ID == id).CountAsync();
                if (r > 0)
                {
                    return new ResultWithError(Message.DATA_IS_USED);
                }
                var item = await _appContext.Organizations.Where(x => x.ID == id ).FirstOrDefaultAsync();
                _appContext.Organizations.Remove(item);
                await _appContext.SaveChangesAsync();
                return new ResultWithError(item);
            }
            catch (Exception ex)
            {
                return new ResultWithError(ex.Message);
            }
        }
        public async Task<ResultWithError> GetAllOrgChartPosition(OrgChartRptInputDTO param)
        {
            try
            {
                var positionItems = QueryData.ExecuteStoreToTable(Procedures.PKG_OMS_REPORT_GET_ORGANIZATION_CHART,
                        new
                        {
                            P_LANGUAGE = param.language,
                            P_USERNAME = _appContext.UserName,
                            P_CUR = QueryData.OUT_CURSOR
                        },false);
                if (positionItems.Tables.Count > 0)
                {
                    var table = positionItems.Tables[0];
                    var data = (from DataRow dr in table.Rows
                                select new OrgChartRptViewDTO
                                {
                                    Id = Convert.ToDecimal(dr["Id"]),
                                    ParentId = dr["parentId"].ToString() != "" ? Convert.ToDecimal(dr["parentId"]) : null,
                                    jobCnt = dr["jobcnt"].ToString() != "" ? Convert.ToDecimal(dr["jobcnt"]) : null,
                                    planFte = dr["planFte"].ToString() != "" ? Convert.ToDecimal(dr["planFte"]) : null,
                                    ytdFte = dr["ytdFte"].ToString() != "" ? Convert.ToDecimal(dr["ytdFte"]) : null,
                                    vsplanFte = dr["vsPlanFte"].ToString() != "" ? Convert.ToDecimal(dr["vsPlanFte"]) : null,
                                    isJob = dr["isJob"].ToString() != "" ? Convert.ToBoolean(dr["isJob"]) : null,
                                    Code =  dr["CODE"].ToString(),
                                    orgName =  dr["orgName"].ToString(),
                                    nhomDuan = dr["nhomDuan"].ToString() != "" ? Convert.ToBoolean(dr["nhomDuan"]) : null,
                                    fullName =  dr["fullName"].ToString(),
                                }).ToList();
                    return new ResultWithError(data);
                }
                else
                {
                    return new ResultWithError(200);
                }
            }
            catch (Exception ex)
            {
                return new ResultWithError(ex.Message);

            }

        }
        public async Task<ResultWithError> GetJobPosTree(JobPositionTreeInputDTO param)
        {
            try
            {
                var positionItems = QueryData.ExecuteStoreToTable("PKG_OMS_REPORT.GET_JOB_POS_TREE",
                        new
                        {
                            P_LANGUAGE = param.language,
                            P_USERNAME = _appContext.UserName,
                            P_CUR = QueryData.OUT_CURSOR
                        }, false);
                if (positionItems.Tables.Count > 0)
                {
                    var table = positionItems.Tables[0];
                    var data = (from DataRow dr in table.Rows
                                select new JobPositionTreeViewDTO
                                {
                                    Id = Convert.ToDecimal(dr["Id"]),
                                    ParentId = dr["parentId"].ToString() != "" ? Convert.ToDecimal(dr["parentId"]) : null,
                                    lyFte = dr["lyFte"].ToString() != "" ? Convert.ToDecimal(dr["lyFte"]) : null,
                                    totalEmp = dr["totalEmp"].ToString() != "" ? Convert.ToDecimal(dr["totalEmp"]) : null,
                                    chenhLech1 = dr["chenhLech1"].ToString() != "" ? Convert.ToDecimal(dr["chenhLech1"]) : null,
                                    totalPosition = dr["totalPosition"].ToString() != "" ? Convert.ToDecimal(dr["totalPosition"]) : null,
                                    chenhLech2 = dr["chenhLech2"].ToString() != "" ? Convert.ToDecimal(dr["chenhLech2"]) : null,
                                    isJob = dr["isJob"].ToString() != "" ? Convert.ToBoolean(dr["isJob"]) : null,
                                    Code = dr["Code"].ToString(),
                                    orgName = dr["orgName"].ToString(),
                                    nhomDuan = dr["nhomDuan"].ToString() != "" ? Convert.ToBoolean(dr["nhomDuan"]) : null,
                                }).ToList();
                    return new ResultWithError(data);
                }
                else
                {
                    return new ResultWithError(200);
                }
            }
            catch (Exception ex)
            {
                return new ResultWithError(ex.Message);

            }

        }
        public async Task<ResultWithError> UpdateCreateRptJobPosHisAsync(JobPositionTreeInputDTO param)
        {
            try
            {                
                String dateTime = "01-01-" + DateTime.Now.Year.ToString();
                DateTime createDate = Convert.ToDateTime(dateTime);

                var ACTIONIU = QueryData.ExecuteStoreObject(Procedures.PKG_OMS_REPORT_IU_RPT_JOB_POS_HIS,
                        new
                        {P_ID = param.Id,
                         P_PARENT_ID = param.ParentId,
                         P_NAME_EN = param.orgName,
                         P_NAME_VN = param.orgName,
                         P_LY_FTE_V2 = param.lyFte,
                         P_CREATED_DATE = createDate,
                         P_CUR = QueryData.OUT_NUMBER
                        }, true);
                
                return new ResultWithError(ACTIONIU);
            }
            catch (Exception ex)
            {
                return new ResultWithError(ex);
            }
        }
        public async Task<ResultWithError> GetJobChildTree(JobChildTreeInputDTO param)
        {
            try
            {
                var JobChildItems = QueryData.ExecuteStoreToTable(Procedures.PKG_OMS_REPORT_GET_JOB_CHILD_TREE,
                        new
                        {
                            P_LANGUAGE = param.language,
                            P_JOB_ID = param.jobId,
                            P_CUR = QueryData.OUT_CURSOR
                        }, false);
                if (JobChildItems.Tables.Count > 0)
                {
                    var table = JobChildItems.Tables[0];
                    var data = (from DataRow dr in table.Rows
                                select new JobChildTreeViewDTO
                                {
                                    Id = Convert.ToDecimal(dr["Id"]),
                                    ParentId = dr["parentId"].ToString() != "" ? Convert.ToDecimal(dr["parentId"]) : null,
                                    Name = dr["Name"].ToString(),
                                    FunctionName = dr["FunctionName"].ToString()
                                }).ToList();
                    return new ResultWithError(data);
                }
                else
                {
                    return new ResultWithError(200);
                }
            }
            catch (Exception ex)
            {
                return new ResultWithError(ex.Message);

            }

        }
    }
}
