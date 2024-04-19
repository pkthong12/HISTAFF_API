using Common.Paging;
using ProfileDAL.ViewModels;
using Common.Repositories;
using Common.Extensions;
using System.Data;
using API.All.DbContexts;
using API.Entities;
using CORE.DTO;
using CORE.GenericUOW;
using API.All.SYSTEM.CoreDAL.System.Language.Models;
using CORE.Enum;
using CORE.StaticConstant;
using System;
using System.Linq;

namespace ProfileDAL.Repositories
{
    public class PositionRepository : RepositoryBase<HU_POSITION>, IPositionRepository
    {
        private readonly ProfileDbContext _appContext;
        private readonly GenericReducer<HU_POSITION, PositionViewNoPagingDTO> genericReducer;
        public PositionRepository(ProfileDbContext context) : base(context)
        {
            _appContext = context;
            genericReducer = new();
        }
        public async Task<GenericPhaseTwoListResponse<PositionViewNoPagingDTO>> SinglePhaseQueryList(GenericQueryListDTO<PositionViewNoPagingDTO> request)
        {
            //var raw = _appContext.Positions.AsQueryable();
            //var phase1 = await genericReducer.FirstPhaseReduce(raw, request);

            //if (phase1.ErrorType != EnumErrorType.NONE)
            //{
            //    return new()
            //    {
            //        ErrorType = phase1.ErrorType,
            //        MessageCode = phase1.MessageCode,
            //        ErrorPhase = 1
            //    };
            //}

            //if (phase1.Queryable == null)
            //{
            //    return new()
            //    {
            //        ErrorType = EnumErrorType.CATCHABLE,
            //        MessageCode = CommonMessageCode.ENTITIES_NOT_FOUND,
            //        ErrorPhase = 1
            //    };
            //}

            //var phase1IdsResult = phase1.Queryable.ToList().Aggregate("", (prev, curr) => prev + curr.ID.ToString() + ";");
            //var ids = phase1IdsResult.Split(';');
            var joined = (from p in _appContext.Positions
                          from o in _appContext.Organizations.Where(x => x.ID == p.ORG_ID).DefaultIfEmpty()
                          from g in _appContext.CompanyInfos.Where(x => x.ID == o.COMPANY_ID).DefaultIfEmpty()
                              //from s in _appContext.OtherLists.Where(x => x.ID == g.INS_UNIT).DefaultIfEmpty()
                          from v in _appContext.OtherLists.Where(x => x.ID == g.REGION_ID).DefaultIfEmpty()
                          from e in _appContext.Employees.Where(x => x.POSITION_ID == p.ID).DefaultIfEmpty()
                          from m in _appContext.Employees.Where(x => x.ID == p.MASTER).DefaultIfEmpty()
                          from i in _appContext.Employees.Where(x => x.ID == p.INTERIM).DefaultIfEmpty()
                          from tt in _appContext.Positions.Where(x => x.ID == p.LM).DefaultIfEmpty()
                          from tte in _appContext.Employees.Where(x => x.ID == tt.MASTER).DefaultIfEmpty()
                          from gt in _appContext.Positions.Where(x => x.ID == p.CSM).DefaultIfEmpty()
                          from gte in _appContext.Employees.Where(x => x.ID == gt.MASTER).DefaultIfEmpty()
                          from ttj in _appContext.HUJobs.Where(x => x.ID == tt.JOB_ID).DefaultIfEmpty()
                          from j in _appContext.HUJobs.Where(x => x.ID == p.JOB_ID).DefaultIfEmpty()
                          from gtj in _appContext.HUJobs.Where(x => x.ID == gt.JOB_ID).DefaultIfEmpty()
                          orderby p.CREATED_DATE descending
                          select new PositionViewNoPagingDTO
                          {
                              Id = p.ID,
                              Name = p.NAME,
                              NameEn = p.NAME_EN,
                              Code = p.CODE,
                              Note = p.NOTE,
                              CreateBy = p.CREATED_BY,
                              CreateDate = p.CREATED_DATE,
                              UpdateBy = p.UPDATED_BY,
                              UpdateDate = p.UPDATED_DATE,
                              EffectiveDate = p.EFFECTIVE_DATE,
                              OrgName = o.NAME,
                              MasterName = m.CODE + " - " + m.Profile.FULL_NAME,
                              InterimName = i.CODE + " - " + i.Profile.FULL_NAME,
                              LmName = tte.Profile.FULL_NAME,
                              EmpLmName = tte.CODE + " - " + tte.Profile.FULL_NAME,
                              LmJobName = ttj.NAME_VN,
                              CsmName = gte.Profile.FULL_NAME,
                              CsmJobName = gtj.NAME_VN,
                              JobName = j.NAME_VN,
                              JobId = p.JOB_ID,
                              Lm = p.LM,
                              Interim = p.INTERIM,
                              Master = p.MASTER,
                              OrgId = p.ORG_ID,
                              Active = p.IS_ACTIVE == true ? "Áp dụng" : "Ngừng áp dụng",
                              JobDesc = p.JOB_DESC,
                              isTDV = p.IS_TDV,
                              isNotot = p.IS_NOTOT,
                              isplan = p.IS_PLAN,
                              isnonphysical = p.IS_NONPHYSICAL,
                              ComCode = g.CODE,
                              OrgCode = o.CODE, 
                              EmployeeCode = tt.CODE,//Thêm mã nhân viên
                              Company = g.NAME_VN, //Thêm tên công ty (hồ sơ nhân viên)
                              WorkAddress = g.WORK_ADDRESS, //Thêm địa chỉ làm việc (hồ sơ nhân viên)
                              NameOnProfileEmployee = j.NAME_VN,
                              InsurenceArea = v.NAME, //Lấy ra vùng bảo hiểm
                              InsurenceAreaId = v.ID, //Lấy ra ID vùng bảo hiểm
                              IsActive = p.IS_ACTIVE
                          }).Distinct();
            //request.Sort = new List<SortItem>();
            //request.Sort.Add(new SortItem()
            //{
            //    Field = "Id",
            //    SortDirection = EnumSortDirection.DESC
            //});
            var phase2 = await genericReducer.SinglePhaseReduce(joined, request);

            return phase2;
        }
        /// <summary>
        /// CMS Get All Data
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<PagedResult<PositionViewDTO>> GetAll(PositionViewDTO param)
        {
            var queryable = from p in _appContext.Positions
                            from o in _appContext.Organizations.Where(x => x.ID == p.ORG_ID)
                            from m in _appContext.Employees.Where(x => x.ID == p.MASTER).DefaultIfEmpty()
                            from i in _appContext.Employees.Where(x => x.ID == p.INTERIM).DefaultIfEmpty()
                            from tt in _appContext.Employees.Where(x => x.ID == p.LM).DefaultIfEmpty()
                            from j in _appContext.HUJobs.Where(x => x.ID == p.JOB_ID).DefaultIfEmpty()
                            orderby p.CREATED_DATE descending
                            select new PositionViewDTO
                            {
                                Id = p.ID,
                                Name = p.NAME,
                                NameEn = p.NAME_EN,
                                Code = p.CODE,
                                IsActive = p.IS_ACTIVE,
                                Note = p.NOTE,
                                CreateBy = p.CREATED_BY,
                                CreateDate = p.CREATED_DATE,
                                UpdateBy = p.UPDATED_BY,
                                UpdateDate = p.UPDATED_DATE,
                                EffectiveDate = p.EFFECTIVE_DATE,
                                OrgId = p.ORG_ID,
                                OrgName = o.NAME,
                                Master = p.MASTER,
                                MasterName = m.CODE + " - " + m.Profile.FULL_NAME,
                                Interim = p.INTERIM,
                                InterimName = i.CODE + " - " + i.Profile.FULL_NAME,
                                Lm = p.LM,
                                LmName = tt.Profile.FULL_NAME,
                                JobId = p.JOB_ID,
                                JobName = j.NAME_VN
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
            if (!string.IsNullOrEmpty(param.GroupName))
            {
                queryable = queryable.Where(p => p.Note.ToUpper().Contains(param.Note.ToUpper()));
            }
            if (param.IsActive != null)
            {
                queryable = queryable.Where(p => p.IsActive == param.IsActive);
            }
            if (param.GroupId != null)
            {
                queryable = queryable.Where(p => p.GroupId == param.GroupId);
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
                // LM,CSM,MASTER,INTERIM PHẢI LƯU VỊ TRÍ NÊN SỬA LẠI CHỔ NÀY
                var r = await (from p in _appContext.Positions
                               from j in _appContext.HUJobs.Where(f => f.ID == p.JOB_ID).DefaultIfEmpty()
                               from t in _appContext.HUJobBands.Where(f => f.ID == j.JOB_BAND_ID).DefaultIfEmpty()
                               from o in _appContext.Organizations.Where(f => f.ID == p.ORG_ID).DefaultIfEmpty()
                               from m in _appContext.Employees.Where(f => f.ID == p.MASTER).DefaultIfEmpty()
                               from l in _appContext.Positions.Where(f => f.ID == p.LM).DefaultIfEmpty()
                               from c in _appContext.Positions.Where(f => f.ID == p.CSM).DefaultIfEmpty()
                               from i in _appContext.Employees.Where(f => f.ID == p.INTERIM).DefaultIfEmpty()
                               from le in _appContext.Employees.Where(f => f.ID == l.MASTER).DefaultIfEmpty()
                               from ce in _appContext.Employees.Where(f => f.ID == c.MASTER).DefaultIfEmpty()
                               where p.ID == id
                               select new PositionOutputDTO
                               {
                                   Id = p.ID,
                                   Name = p.NAME,
                                   NameEn = p.NAME_EN,
                                   Code = p.CODE,
                                   IsActive = p.IS_ACTIVE,
                                   Note = p.NOTE,
                                   workingtime = p.WORKING_TIME,
                                   CreateBy = p.CREATED_BY,
                                   CreateDate = p.CREATED_DATE,
                                   UpdateBy = p.UPDATED_BY,
                                   UpdateDate = p.UPDATED_DATE,
                                   effectivedate = p.EFFECTIVE_DATE,
                                   OrgId = p.ORG_ID,
                                   orgname = o.NAME,
                                   master = p.MASTER,
                                   csm = p.CSM,
                                   csmname = ce.CODE + " - " + ce.Profile.FULL_NAME,
                                   interim = p.INTERIM,
                                   interimname = i.CODE + " - " + i.Profile.FULL_NAME,
                                   lm = p.LM,
                                   lmname = le.CODE + " - " + le.Profile.FULL_NAME,
                                   filename = p.FILENAME,
                                   uploadfile = p.UPLOADFILE,
                                   isowner = p.ISOWNER,
                                   isplan = p.IS_PLAN,
                                   isnonphysical = p.IS_NONPHYSICAL,
                                   jobid = p.JOB_ID,
                                   jobname = j.NAME_VN,
                                   isTDV = p.IS_TDV,
                                   isNotot = p.IS_NOTOT,
                                   jobDesc = p.JOB_DESC,
                                   MasterName = m.CODE + " - " + m.Profile.FULL_NAME
                               }).FirstOrDefaultAsync();
                //var r2 = await (from p_des in _appContext.PositionDesriptions
                //                from c in _appContext.OtherLists.Where(f => f.ID == p_des.COMPUTER).DefaultIfEmpty()
                //                from l in _appContext.OtherLists.Where(f => f.ID == p_des.LANGUAGE).DefaultIfEmpty()
                //                from t in _appContext.OtherLists.Where(f => f.ID == p_des.TDCM).DefaultIfEmpty()
                //                where p_des.POSITION_ID == id
                //                select new PositionDesriptionOutputDTO
                //                {
                //                    id = p_des.ID,
                //                    computer = p_des.COMPUTER,
                //                    detailresponsibility1 = p_des.DETAIL_RESPONSIBILITY_1,
                //                    detailresponsibility2 = p_des.DETAIL_RESPONSIBILITY_2,
                //                    detailresponsibility3 = p_des.DETAIL_RESPONSIBILITY_3,
                //                    detailresponsibility4 = p_des.DETAIL_RESPONSIBILITY_4,
                //                    detailresponsibility5 = p_des.DETAIL_RESPONSIBILITY_5,
                //                    filename = p_des.FILE_NAME,
                //                    internal1 = p_des.INTERNAL_1,
                //                    internal2 = p_des.INTERNAL_2,
                //                    internal3 = p_des.INTERNAL_3,
                //                    jobtarget1 = p_des.JOB_TARGET_1,
                //                    jobtarget2 = p_des.JOB_TARGET_2,
                //                    jobtarget3 = p_des.JOB_TARGET_3,
                //                    jobtarget4 = p_des.JOB_TARGET_4,
                //                    jobtarget5 = p_des.JOB_TARGET_5,
                //                    jobtarget6 = p_des.JOB_TARGET_6,
                //                    language = p_des.LANGUAGE,
                //                    major = p_des.MAJOR,
                //                    outside1 = p_des.OUTSIDE_1,
                //                    outside2 = p_des.OUTSIDE_2,
                //                    outside3 = p_des.OUTSIDE_3,
                //                    outresult1 = p_des.OUT_RESULT_1,
                //                    outresult2 = p_des.OUT_RESULT_2,
                //                    outresult3 = p_des.OUT_RESULT_3,
                //                    outresult4 = p_des.OUT_RESULT_4,
                //                    outresult5 = p_des.OUT_RESULT_5,
                //                    permission1 = p_des.PERMISSION_1,
                //                    permission2 = p_des.PERMISSION_2,
                //                    permission3 = p_des.PERMISSION_3,
                //                    permission4 = p_des.PERMISSION_4,
                //                    permission5 = p_des.PERMISSION_5,
                //                    permission6 = p_des.PERMISSION_6,
                //                    responsibility1 = p_des.RESPONSIBILITY_1,
                //                    responsibility2 = p_des.RESPONSIBILITY_2,
                //                    responsibility3 = p_des.RESPONSIBILITY_3,
                //                    responsibility4 = p_des.RESPONSIBILITY_4,
                //                    responsibility5 = p_des.RESPONSIBILITY_5,
                //                    supportskill = p_des.SUPPORT_SKILL,
                //                    tdcm = p_des.TDCM,
                //                    positionid = p_des.POSITION_ID,
                //                    uploadfile = p_des.UPLOAD_FILE,
                //                    workexp = p_des.WORK_EXP
                //                }).FirstOrDefaultAsync();

                //r.positionDesc = r2;

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
        public async Task<ResultWithError> CreateAsync(PositionInputDTO param)
        {
            try
            {

                if (param.ConfirmChangeTdv == false)
                {
                    return new ResultWithError(CommonMessageCode.CONFIRM_CHANGE_TDV_POSITION_AGAIN);
                }
                var r1 = _appContext.Positions.Where(x => x.CODE == param.code).Count();
                if (r1 > 0)
                {
                    return new ResultWithError(Consts.CODE_EXISTS);
                }
                // var r = _appContext.GroupPositions.Where(x => x.ID == param.groupId ).Count();
                // if (r == 0)
                // {
                //     return new ResultWithError("GROUP_NOT_EXISTS");
                // }
                var data = Map(param, new HU_POSITION());
                data.IS_ACTIVE = true;
                data.NAME = (from p in _appContext.HUJobs where p.ID == data.JOB_ID select p.NAME_VN).FirstOrDefault();
                var result = await _appContext.Positions.AddAsync(data);
                await _appContext.SaveChangesAsync();
                if (param.ConfirmChangeTdv == true)
                {
                    var itemInfor = (from p in _appContext.Positions where p.IS_TDV == true && p.ID != data.ID select p).ToList();
                    if (itemInfor != null)
                    {
                        foreach (var item in itemInfor)
                        {
                            item.IS_TDV = null;
                            var resultTdv = _appContext.Positions.Update(data);
                        }
                    }
                }
                await _appContext.SaveChangesAsync();
                //if (data.ID > 0)
                //{
                //    var dataDesc = Map(param._positionDesc, new HU_JOB_DESCRIPTION());
                //    dataDesc.POSITION_ID = data.ID;
                //    var resultDesc = await _appContext.PositionDesriptions.AddAsync(dataDesc);
                //    await _appContext.SaveChangesAsync();
                //}

                return new ResultWithError(data);
            }
            catch (Exception ex)
            {
                return new ResultWithError(ex);
            }
        }
        /// <summary>
        /// CMS Edit Data
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<ResultWithError> UpdateAsync(PositionInputDTO param)
        {
            try
            {
                if (param.ConfirmChangeTdv == false)
                {
                    return new ResultWithError(CommonMessageCode.CONFIRM_CHANGE_TDV_POSITION_AGAIN);
                }
                // check code
                var c = _appContext.Positions.Where(x => x.CODE.ToLower() == param.code.ToLower() && x.ID != param.Id).Count();
                if (c > 0)
                {
                    return new ResultWithError(Consts.CODE_EXISTS);
                }
                // var r = _appContext.GroupPositions.Where(x => x.ID == param.groupId ).Count();
                // if (r == 0)
                // {
                //     return new ResultWithError("GROUP_NOT_EXISTS");
                // }
                var a = _appContext.Positions.Where(x => x.ID == param.Id).FirstOrDefault();
                //var a_desc = _appContext.PositionDesriptions.Where(x => x.POSITION_ID == param.Id).FirstOrDefault();
                if (a == null)
                {
                    return new ResultWithError(404);
                }

                var data = Map(param, a);
                data.NAME = (from p in _appContext.HUJobs where p.ID == data.JOB_ID select p.NAME_VN).FirstOrDefault();
                var result = _appContext.Positions.Update(data);
                if (param.ConfirmChangeTdv == true)
                {
                    var itemInfor = (from p in _appContext.Positions where p.IS_TDV == true && p.ID != data.ID select p).ToList();
                    if (itemInfor != null)
                    {
                        foreach(var item in itemInfor)
                        {
                            item.IS_TDV = null;
                            var resultTdv = _appContext.Positions.Update(data);
                        }
                    }
                }
                //if (a_desc != null)
                //{
                //    var dataDesc = Map(param._positionDesc, a_desc);
                //    var resultDesc = _appContext.PositionDesriptions.Update(dataDesc);
                //}
                await _appContext.SaveChangesAsync();
                return new ResultWithError(data);
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
        public async Task<ResultWithError> ChangeStatusAsync(PositionInputDTO request)
        {
            try
            {
                foreach (var item in request.Ids)
                {
                    var r = _appContext.Positions.Where(x => x.ID == item).FirstOrDefault();
                    if(request.ValueToBind == false)
                    {
                        var emp = _appContext.Employees.Where(x => x.ID == r.MASTER).FirstOrDefault();
                        if(emp != null)
                        {
                            if (emp.WORK_STATUS_ID != 1028)
                            {
                                return new ResultWithError(Message.EMPLOYEE_ARE_WORKING);
                            }
                        }
                        if(r.MASTER != null && r.INTERIM != null)//vi tri dang co nguoi ngoi
                        {
                            return new ResultWithError(Message.THE_POSITION_IS_SITTING);
                        }
                        if (r == null)
                        {
                            return new ResultWithError(404);
                        }
                    }
                    r.IS_ACTIVE = request.ValueToBind;
                    var result = _appContext.Positions.Update(r);
                }
                await _appContext.SaveChangesAsync();
                return new ResultWithError(request);
            }
            catch (Exception ex)
            {
                return new ResultWithError(ex.Message);
            }
        }
        /// <summary>
        /// Get List Group is Activce
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<ResultWithError> GetList(int groupId)
        {
            try
            {
                var queryable = from p in _appContext.Positions
                                join g in _appContext.GroupPositions on p.GROUP_ID equals g.ID
                                where p.IS_ACTIVE == true
                                orderby g.CODE, p.CODE
                                select new { p };

                if (groupId != 0)
                {
                    queryable = queryable.Where(c => c.p.GROUP_ID == groupId);
                }

                var data = await queryable.Select(c => new
                {
                    Id = c.p.ID,
                    Code = c.p.CODE,
                    Name = c.p.NAME
                }).ToListAsync();
                return new ResultWithError(data);
            }
            catch (Exception ex)
            {
                return new ResultWithError(ex.Message);
            }
        }
        /// <summary>
        /// Get List Group is Activce
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<ResultWithError> GetListJob()
        {
            try
            {
                var queryable = from p in _appContext.HUJobs
                                from o in _appContext.OtOtherLists.Where(x => x.ID == p.JOB_FAMILY_ID).DefaultIfEmpty()
                                select new HUJobInputDTO
                                {
                                    Id = p.ID,
                                    NameVN = p.NAME_VN,
                                    NameEN = p.NAME_EN,
                                    Actflg = p.ACTFLG,
                                    ActflgStr = p.ACTFLG == "A" ? "Áp dụng" : "Ngừng áp dụng",
                                    Code = p.CODE,
                                    CreatedDate = p.CREATED_DATE,
                                    Note = p.NOTE
                                };

                var data = await queryable.Select(c => new
                {
                    Id = c.Id,
                    NAME = c.Code + " - " + c.NameVN
                }).ToListAsync();
                return new ResultWithError(data);
            }
            catch (Exception ex)
            {
                return new ResultWithError(ex.Message);
            }
        }
        /// <summary>
        /// Delete by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ResultWithError> Delete(List<long> ids)
        {
            try
            {
                List<HU_POSITION> Pos = new List<HU_POSITION>();
                List<HU_JOB_DESCRIPTION> PoDecss = new();
                foreach (var id in ids)
                {
                    var r = await _appContext.Employees.Where(x => x.POSITION_ID == id).CountAsync();
                    if (r > 0)
                    {
                        return new ResultWithError(Message.DATA_IS_USED);
                    }

                    var item = await _appContext.Positions.Where(x => x.ID == id).FirstOrDefaultAsync();
                    var itemDesc = await _appContext.PositionDesriptions.Where(x => x.POSITION_ID == id).FirstOrDefaultAsync();

                    Pos.Add(item);
                    PoDecss.Add(itemDesc);
                }

                _appContext.Positions.RemoveRange(Pos);
                _appContext.PositionDesriptions.RemoveRange(PoDecss);

                await _appContext.SaveChangesAsync();
                return new ResultWithError(200);
            }
            catch (Exception ex)
            {
                return new ResultWithError(ex.Message);
            }
        }
        /// <summary>
        /// Get by OrgID && EmployeeID
        /// </summary>
        /// <param name="orgId"></param>
        /// <param name="empId"></param>
        /// <returns>List Positions</returns>
        public async Task<ResultWithError> GetByOrg(int orgId, int empId)
        {
            try
            {
                var data = await (from p in _appContext.Positions
                                  from i in _appContext.Employees.Where(x => x.ID == p.INTERIM).DefaultIfEmpty()
                                  from m in _appContext.Employees.Where(x => x.ID == p.MASTER).DefaultIfEmpty()
                                  where p.IS_ACTIVE == true && p.ORG_ID == orgId && p.IS_PLAN.Value
                                  && ((p.MASTER.HasValue && p.MASTER != empId && !p.INTERIM.HasValue) || !p.MASTER.HasValue || (p.MASTER.HasValue && p.MASTER == empId))
                                  select new
                                  {
                                      Id = p.ID,
                                      name = p.CODE + " - " + p.NAME,
                                      master = (!string.IsNullOrEmpty(m.CODE) ? m.CODE + " - " + m.Profile.FULL_NAME : ""),
                                      interim = (!string.IsNullOrEmpty(i.CODE) ? i.CODE + " - " + i.Profile.FULL_NAME : "")
                                  }).ToListAsync();
                return new ResultWithError(data);
            }
            catch (Exception ex)
            {
                return new ResultWithError(ex.Message);
            }
        }

        public async Task<ResultWithError> GetLM(int positionId)
        {
            try
            {
                var data = await (from p in _appContext.Positions
                                  from m in _appContext.Positions.Where(x => x.ID == p.LM).DefaultIfEmpty()
                                  from e in _appContext.Employees.Where(x => x.ID == m.MASTER).DefaultIfEmpty()
                                  where p.ID == positionId
                                  select new
                                  {
                                      Id = p.ID,
                                      direcManagerPositionName = (!string.IsNullOrEmpty(m.CODE) ? m.NAME : ""),
                                      direcManagerName = (!string.IsNullOrEmpty(e.CODE) ? e.CODE + " - " + e.Profile.FULL_NAME : "")
                                  }).FirstOrDefaultAsync();
                return new ResultWithError(data);

            }
            catch (Exception ex)
            {
                return new ResultWithError(ex.Message);
            }
        }
        public string AutoGenCodeHuTile(string tableName, string colName)
        {
            try
            {
                // Chặn ký tự ko phải chữ hoặc số
                if (!tableName.All(f => char.IsLetterOrDigit(f) || string.Concat(f) == "_" || string.Concat(f) == ".") || !colName.All(f => char.IsLetterOrDigit(f) || string.Concat(f) == "_" || string.Concat(f) == "."))
                {

                    return "00000";
                }

                // SQL bên dưới có nguy cơ SQL Injection
                var Sql = "select NVL(MAX(" + colName + "), '" + "00000') from " + tableName + " where " + colName + " LIKE '" + "0%'";
                var str = QueryData.ExecuteStoreQuery(Sql);
                if (str.Result.ToString() == "")
                {
                    return "00001";
                }
                var number = decimal.Parse(str.Result.ToString());
                number += 1;
                var lastChar = number.ToString("00000");
                return lastChar;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        public async Task<ResultWithError> ModifyPositionById(PositionInputDTO param, int orgRight, int Address, int orgIDDefault = 1, int isDissolveDefault = 0)
        {
            try
            {
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

                // Check quyen du lieu
                List<long?> objChk = (from p in _appContext.Positions
                                      where ids.Contains(p.ORG_ID)
                                      select p.ORG_ID).ToList();

                if (orgRight > 0)
                {
                    var emp = (from p in _appContext.Employees where p.POSITION_ID == param.Id select p).FirstOrDefault();
                    if (emp != null)
                    {
                        emp.ORG_ID = orgRight;
                    }
                }
                if (objChk.Count < 1)
                {
                    return new ResultWithError(false);
                }

                ids = orgIds.Where(f => (long?)((dynamic)f).ID == orgRight).Select(c => (long?)((dynamic)c).ID).ToList();
                objChk = ids;

                if (objChk.Count < 1)
                {
                    return new ResultWithError(false);
                }

                DateTime sDate = DateTime.Now;
                List<long> lstOrg = new List<long>();
                var lstPosition = (from p in _appContext.Positions select p).ToList();
                int? OrgIdRight;
                if (orgRight == null || orgRight < 0)
                {
                    OrgIdRight = 0 - orgRight;
                }
                else
                {
                    OrgIdRight = orgRight;
                }
                if (param.isowner == true)
                {
                    //Nếu vị trí trước điều chuyển ngoài kho
                    if (param.isPlanLeft == false)
                    {
                        //Lấy Pos là trưởng phòng của đơn vị cha nếu có
                        var OrgParentLeft = (from o in _appContext.Organizations
                                             where o.ID == param.OrgId
                                             select o.PARENT_ID).FirstOrDefault();
                        if (OrgParentLeft != null)
                        {
                            var PositionId = (from t in _appContext.Positions
                                              where t.ORG_ID == OrgParentLeft && t.ISOWNER == true && t.IS_ACTIVE == true && t.IS_PLAN == false
                                              select t.ID).FirstOrDefault();
                            if (PositionId != 0)
                            {
                                //update QLTT, QLPD của tất cả các Pos cùng đơn vị có Pos chuyển đi bằng cha
                                var lstPositionChildLeft = (from d in _appContext.Positions
                                                            where (d.LM == param.Id || d.CSM == param.Id) && d.IS_ACTIVE == true
                                                            select d).ToList();
                                if (lstPositionChildLeft.Count > 0)
                                {
                                    foreach (var obj5 in lstPositionChildLeft)
                                    {
                                        if (obj5.LM == param.Id && obj5.CSM == param.Id)
                                        {
                                            var objData7 = (from p in _appContext.Positions where p.ID == obj5.ID select p).FirstOrDefault();
                                            if (objData7 != null)
                                            {
                                                //objData7.LM = PositionId
                                                //objData7.CSM = PositionId
                                                objData7.TYPE_ACTIVITIES = "UPDATE";
                                                objData7.EFFECTIVE_DATE = sDate;
                                                await _appContext.SaveChangesAsync();
                                            }
                                        }
                                        if (obj5.LM == param.Id)
                                        {
                                            var objData8 = (from p in _appContext.Positions
                                                            where p.ID == obj5.ID
                                                            select p).FirstOrDefault();
                                            // _appContext.Positions.Attach(objData8)
                                            if (objData8 != null)
                                            {
                                                // objData8.LM = PositionId
                                                objData8.TYPE_ACTIVITIES = "UPDATE";
                                                objData8.EFFECTIVE_DATE = sDate;
                                                await _appContext.SaveChangesAsync();
                                            }

                                            var objData8HU = (from p in _appContext.Positions
                                                              where p.ID == obj5.ID
                                                              select p).FirstOrDefault();
                                            // _appContext.Positions.Attach(objData8)
                                            if (objData8HU != null)
                                                // objData8HU.LM = PositionId
                                                await _appContext.SaveChangesAsync();
                                        }
                                        if (obj5.CSM == param.Id)
                                        {
                                            var objData9 = (from p in _appContext.Positions
                                                            where p.ID == obj5.ID
                                                            select p).FirstOrDefault();
                                            // _appContext.Positions.Attach(objData9)
                                            if (objData9 != null)
                                            {
                                                // objData9.CSM = PositionId
                                                objData9.TYPE_ACTIVITIES = "UPDATE";
                                                objData9.EFFECTIVE_DATE = sDate;
                                                await _appContext.SaveChangesAsync();
                                            }

                                            var objData9HU = (from p in _appContext.Positions
                                                              where p.ID == obj5.ID
                                                              select p).FirstOrDefault();
                                            // _appContext.Positions.Attach(objData9)
                                            if (objData9HU != null)
                                                // objData9HU.CSM = PositionId
                                                await _appContext.SaveChangesAsync();
                                        }
                                    }
                                }
                            }
                            else
                            {
                                // Nếu Pos cha k có thì update QLTT, QLPD của tất cả các Pos cùng đơn vị có Pos chuyển đi bằng null
                                var lstPositionChildLeft = (from d in _appContext.Positions
                                                            where (d.LM == param.Id || d.CSM == param.Id) && d.IS_ACTIVE == true
                                                            select d).ToList();
                                if (lstPositionChildLeft.Count > 0)
                                {
                                    foreach (var obj7 in lstPositionChildLeft)
                                    {
                                        if (obj7.LM == param.Id && obj7.CSM == param.Id)
                                        {
                                            var objData13 = (from p in _appContext.Positions
                                                             where p.ID == obj7.ID
                                                             select p).FirstOrDefault();
                                            // _appContext.Positions.Attach(objData7)
                                            if (objData13 != null)
                                            {
                                                // objData13.LM = Nothing
                                                // objData13.CSM = Nothing
                                                objData13.TYPE_ACTIVITIES = "UPDATE";
                                                objData13.EFFECTIVE_DATE = sDate;
                                                await _appContext.SaveChangesAsync();
                                            }
                                            // Update Positions
                                            var objData13HU = (from p in _appContext.Positions
                                                               where p.ID == obj7.ID
                                                               select p).FirstOrDefault();
                                            if (objData13HU != null)
                                                // objData13HU.LM = Nothing
                                                // objData13HU.CSM = Nothing
                                                await _appContext.SaveChangesAsync();
                                        }
                                        if (obj7.LM == param.Id)
                                        {
                                            var objData14 = (from p in _appContext.Positions
                                                             where p.ID == obj7.ID
                                                             select p).FirstOrDefault();
                                            // _appContext.Positions.Attach(objData8)
                                            if (objData14 != null)
                                            {
                                                // objData14.LM = Nothing
                                                objData14.TYPE_ACTIVITIES = "UPDATE";
                                                objData14.EFFECTIVE_DATE = sDate;
                                                await _appContext.SaveChangesAsync();
                                            }

                                            var objData14HU = (from p in _appContext.Positions
                                                               where p.ID == obj7.ID
                                                               select p).FirstOrDefault();
                                            if (objData14HU != null)
                                                // objData14HU.LM = Nothing
                                                await _appContext.SaveChangesAsync();
                                        }
                                        if (obj7.CSM == param.Id)
                                        {
                                            var objData15 = (from p in _appContext.Positions
                                                             where p.ID == obj7.ID
                                                             select p).FirstOrDefault();
                                            // _appContext.Positions.Attach(objData9)
                                            if (objData15 != null)
                                            {
                                                // objData15.CSM = Nothing
                                                objData15.TYPE_ACTIVITIES = "UPDATE";
                                                objData15.EFFECTIVE_DATE = sDate;
                                                await _appContext.SaveChangesAsync();
                                            }

                                            var objData15HU = (from p in _appContext.Positions
                                                               where p.ID == obj7.ID
                                                               select p).FirstOrDefault();
                                            // _appContext.Positions.Attach(objData9)
                                            if (objData15HU != null)
                                                // objData15HU.CSM = Nothing
                                                await _appContext.SaveChangesAsync();
                                        }
                                    }
                                }
                            }
                        }
                    }
                    // check xem đơn vị có Pos chuyển đến có đơn vị cha hay k?
                    var OrgParent = (from o in _appContext.Organizations
                                     where o.ID == OrgIdRight
                                     select o.PARENT_ID).FirstOrDefault();
                    // Nếu có Org cha
                    if (OrgParent != null)
                    {
                        var PositionId = (from t in _appContext.Positions
                                          where t.ORG_ID == OrgParent && t.ISOWNER == true && t.IS_ACTIVE == true && t.IS_PLAN == false
                                          select t.ID).FirstOrDefault();
                        if (PositionId != 0)
                        {
                            var objData = (from p in _appContext.Positions
                                           where p.ID == param.Id
                                           select p).FirstOrDefault();
                            // _appContext.Positions.Attach(objData)
                            objData.ORG_ID = (int)OrgIdRight;
                            objData.IS_PLAN = param.isplan;
                            objData.WORK_LOCATION = (int)Address;
                            // objData.LM = PositionId
                            // objData.CSM = PositionId
                            objData.TYPE_ACTIVITIES = "UPDATE";
                            objData.EFFECTIVE_DATE = sDate;
                            await _appContext.SaveChangesAsync();
                        }
                        else
                        {
                            var objData = (from p in _appContext.Positions
                                           where p.ID == param.Id
                                           select p).FirstOrDefault();
                            // _appContext.Positions.Attach(objData)
                            objData.ORG_ID = (int)OrgIdRight;
                            objData.IS_PLAN = param.isplan;
                            objData.WORK_LOCATION = (int)Address;
                            // objData.LM = Nothing
                            // objData.CSM = Nothing
                            objData.TYPE_ACTIVITIES = "UPDATE";
                            objData.EFFECTIVE_DATE = sDate;
                            await _appContext.SaveChangesAsync();
                        }
                    }
                    if (param.OrgId != orgRight)
                    {
                        // update QLPD và QLTT của tất cả các Pos cùng đơn vị có Pos chuyển đến
                        lstPosition = (from t in _appContext.Positions
                                       where t.ORG_ID == OrgIdRight && t.ISOWNER == false && t.IS_ACTIVE == true
                                       select t).ToList();
                        if (lstPosition.Count > 0)
                        {
                            foreach (var it1 in lstPosition)
                            {
                                var objData1 = (from p in _appContext.Positions
                                                where p.ID == it1.ID
                                                select p).FirstOrDefault();
                                // _appContext.Positions.Attach(objData1)
                                if (objData1 != null)
                                {
                                    // objData1.LM = param.Id
                                    // objData1.CSM = param.Id
                                    objData1.TYPE_ACTIVITIES = "UPDATE";
                                    objData1.EFFECTIVE_DATE = sDate;
                                    await _appContext.SaveChangesAsync();
                                }

                                var objData1HU = (from p in _appContext.Positions
                                                  where p.ID == it1.ID
                                                  select p).FirstOrDefault();
                                // _appContext.Positions.Attach(objData1)
                                if (objData1HU != null)
                                    // objData1HU.LM = param.Id
                                    // objData1HU.CSM = param.Id
                                    await _appContext.SaveChangesAsync();
                            }
                        }
                        if (param.isplan == false)
                        {
                            // Update tất cả các Pos là trưởng phòng của tất cả các đơn vị con của đơn vị có Pos chuyển đến
                            lstOrg = (from o in _appContext.Organizations
                                      where o.PARENT_ID == OrgIdRight
                                      select o.ID).ToList();
                            if (lstOrg.Count > 0)
                            {
                                foreach (var it in lstOrg)
                                {
                                    var lstPositionChild = (from d in _appContext.Positions
                                                            where d.ORG_ID == it && d.ISOWNER == true && d.IS_ACTIVE == true
                                                            select d).ToList();
                                    if (lstPositionChild.Count > 0)
                                    {
                                        foreach (var it2 in lstPositionChild)
                                        {
                                            var objData2 = (from p in _appContext.Positions
                                                            where p.ID == it2.ID
                                                            select p).FirstOrDefault();
                                            // _appContext.Positions.Attach(objData2)
                                            if (objData2 != null)
                                            {
                                                // objData2.LM = param.Id
                                                // objData2.CSM = param.Id
                                                objData2.TYPE_ACTIVITIES = "UPDATE";
                                                objData2.EFFECTIVE_DATE = sDate;
                                                await _appContext.SaveChangesAsync();
                                            }

                                            var objData2HU = (from p in _appContext.Positions
                                                              where p.ID == it2.ID
                                                              select p).FirstOrDefault();
                                            // _appContext.Positions.Attach(objData2)
                                            if (objData2HU != null)
                                                // objData2HU.LM = param.Id
                                                // objData2HU.CSM = param.Id
                                                await _appContext.SaveChangesAsync();
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        // Điều chuyển từ kế hoạch ra ngoài và đó là trưởng thì update lại QLTT,QLTT các vị trí = trưởng chuyển đến
                        if (param.isowner == true && param.isplan == false)
                        {
                            lstPosition = (from t in _appContext.Positions
                                           where t.ORG_ID == OrgIdRight && t.ISOWNER == false && t.IS_ACTIVE == true
                                           select t).ToList();
                            if (lstPosition.Count > 0)
                            {
                                foreach (var it1 in lstPosition)
                                {
                                    var objData1 = (from p in _appContext.Positions
                                                    where p.ID == it1.ID
                                                    select p).FirstOrDefault();
                                    if (objData1 != null)
                                    {
                                        // objData1.LM = param.Id
                                        // objData1.CSM = param.Id
                                        objData1.TYPE_ACTIVITIES = "UPDATE";
                                        objData1.EFFECTIVE_DATE = sDate;
                                        await _appContext.SaveChangesAsync();
                                    }

                                    var objData1HU = (from p in _appContext.Positions
                                                      where p.ID == it1.ID
                                                      select p).FirstOrDefault();
                                    if (objData1HU != null)
                                        // objData1HU.LM = param.Id
                                        // objData1HU.CSM = param.Id
                                        await _appContext.SaveChangesAsync();
                                }
                            }
                        }

                        // Điều chuyển trưởng vào kho thì update lại QLTT, QLTT của tất cả các vị trí = trưởng cao hơn
                        if (param.isowner == true && param.isplan == true)
                        {
                            var OrgParentA = (from o in _appContext.Organizations
                                              where o.ID == param.OrgId
                                              select o.PARENT_ID).FirstOrDefault();
                            if (OrgParentA != null)
                            {
                                var PositionLeftParent = (from t in _appContext.Positions
                                                          where t.ORG_ID == OrgParentA && t.ISOWNER == true && t.IS_PLAN == false && t.IS_ACTIVE == true
                                                          select t.ID).FirstOrDefault();
                                if (PositionLeftParent != 0)
                                {
                                    // Update QLTT, QLPD của tất cả các Pos có nó là QLTT hoặc QLPD bằng Pos trưởng Org cha
                                    lstPosition = (from t in _appContext.Positions
                                                   where t.ORG_ID == OrgIdRight && t.ISOWNER == false && t.IS_ACTIVE == true
                                                   select t).ToList();
                                    if (lstPosition.Count > 0)
                                    {
                                        foreach (var it1 in lstPosition)
                                        {
                                            var objData1 = (from p in _appContext.Positions
                                                            where p.ID == it1.ID
                                                            select p).FirstOrDefault();
                                            if (objData1 != null)
                                            {
                                                // objData1.LM = PositionLeftParent
                                                // objData1.CSM = PositionLeftParent
                                                objData1.TYPE_ACTIVITIES = "UPDATE";
                                                objData1.EFFECTIVE_DATE = sDate;
                                                await _appContext.SaveChangesAsync();
                                            }

                                            var objData1HU = (from p in _appContext.Positions
                                                              where p.ID == it1.ID
                                                              select p).FirstOrDefault();
                                            if (objData1HU != null)
                                                // objData1HU.LM = PositionLeftParent
                                                // objData1HU.CSM = PositionLeftParent
                                                await _appContext.SaveChangesAsync();
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    // Nếu trước điều chuyển là thường ngoài kho
                    if (param.isPlanLeft == false)
                    {
                        // Lấy ra trưởng của nó
                        var PositionParentLeft = (from d in _appContext.Positions
                                                  where d.ORG_ID == param.OrgId && d.ISOWNER == true && d.IS_PLAN == false && d.IS_ACTIVE == true
                                                  select d.ID).FirstOrDefault();
                        // Nếu có Pos trưởng
                        if (PositionParentLeft != 0)
                        {
                            // Update QLTT, QLPD của tất cả các Pos có nó là QLTT hoặc QLPD bằng Pos trưởng
                            var lstPositionLeft = (from d in _appContext.Positions
                                                   where (d.LM == param.Id || d.CSM == param.Id) && d.IS_ACTIVE == true
                                                   select d).ToList();
                            if (lstPositionLeft.Count > 0)
                            {
                                foreach (var obj4 in lstPositionLeft)
                                {
                                    if (obj4.LM == param.Id && obj4.CSM == param.Id)
                                    {
                                        var objData4 = (from p in _appContext.Positions
                                                        where p.ID == obj4.ID
                                                        select p).FirstOrDefault();
                                        // _appContext.Positions.Attach(objData4)
                                        if (objData4 != null)
                                        {
                                            // objData4.LM = PositionParentLeft
                                            // objData4.CSM = PositionParentLeft
                                            objData4.TYPE_ACTIVITIES = "UPDATE";
                                            objData4.EFFECTIVE_DATE = sDate;
                                            await _appContext.SaveChangesAsync();
                                        }

                                        var objData4HU = (from p in _appContext.Positions
                                                          where p.ID == obj4.ID
                                                          select p).FirstOrDefault();
                                        // _appContext.Positions.Attach(objData4)
                                        if (objData4HU != null)
                                            // objData4HU.LM = PositionParentLeft
                                            // objData4HU.CSM = PositionParentLeft
                                            await _appContext.SaveChangesAsync();
                                    }
                                    if (obj4.LM == param.Id)
                                    {
                                        var objData5 = (from p in _appContext.Positions
                                                        where p.ID == obj4.ID
                                                        select p).FirstOrDefault();
                                        // _appContext.Positions.Attach(objData5)
                                        if (objData5 != null)
                                        {
                                            // objData5.LM = PositionParentLeft
                                            objData5.TYPE_ACTIVITIES = "UPDATE";
                                            objData5.EFFECTIVE_DATE = sDate;
                                            await _appContext.SaveChangesAsync();
                                        }

                                        var objData5HU = (from p in _appContext.Positions
                                                          where p.ID == obj4.ID
                                                          select p).FirstOrDefault();
                                        // _appContext.Positions.Attach(objData5)
                                        if (objData5HU != null)
                                            // objData5HU.LM = PositionParentLeft
                                            await _appContext.SaveChangesAsync();
                                    }
                                    if (obj4.CSM == param.Id)
                                    {
                                        var objData6 = (from p in _appContext.Positions
                                                        where p.ID == obj4.ID
                                                        select p).FirstOrDefault();
                                        // _appContext.Positions.Attach(objData6)
                                        if (objData6 != null)
                                        {
                                            // objData6.CSM = PositionParentLeft
                                            objData6.TYPE_ACTIVITIES = "UPDATE";
                                            objData6.EFFECTIVE_DATE = sDate;
                                            await _appContext.SaveChangesAsync();
                                        }

                                        var objData6HU = (from p in _appContext.Positions
                                                          where p.ID == obj4.ID
                                                          select p).FirstOrDefault();
                                        // _appContext.Positions.Attach(objData6)
                                        if (objData6HU != null)
                                            // objData6HU.CSM = PositionParentLeft
                                            await _appContext.SaveChangesAsync();
                                    }
                                }
                            }
                        }
                        else
                        {
                            // Nếu k có Pos trưởng thì lấy Pos trưởng Org cha
                            var OrgParent = (from o in _appContext.Organizations
                                             where o.ID == param.OrgId
                                             select o.PARENT_ID).FirstOrDefault();
                            if (OrgParent != null)
                            {
                                var PositionLeftParent = (from t in _appContext.Positions
                                                          where t.ORG_ID == OrgParent && t.ISOWNER == true && t.IS_PLAN == false && t.IS_ACTIVE == true
                                                          select t.ID).FirstOrDefault();
                                if (PositionLeftParent != 0)
                                {
                                    // Update QLTT, QLPD của tất cả các Pos có nó là QLTT hoặc QLPD bằng Pos trưởng Org cha
                                    var lstPositionLeft = (from d in _appContext.Positions
                                                           where (d.LM == param.Id || d.CSM == param.Id) && d.IS_ACTIVE == true
                                                           select d).ToList();
                                    if (lstPositionLeft.Count > 0)
                                    {
                                        foreach (var obj6 in lstPositionLeft)
                                        {
                                            if (obj6.LM == param.Id && obj6.CSM == param.Id)
                                            {
                                                var objData10 = (from p in _appContext.Positions
                                                                 where p.ID == obj6.ID
                                                                 select p).FirstOrDefault();
                                                // _appContext.Positions.Attach(objData4)
                                                if (objData10 != null)
                                                {
                                                    // objData10.LM = PositionLeftParent
                                                    // objData10.CSM = PositionLeftParent
                                                    objData10.TYPE_ACTIVITIES = "UPDATE";
                                                    objData10.EFFECTIVE_DATE = sDate;
                                                    await _appContext.SaveChangesAsync();
                                                }

                                                var objData10HU = (from p in _appContext.Positions
                                                                   where p.ID == obj6.ID
                                                                   select p).FirstOrDefault();
                                                // _appContext.Positions.Attach(objData4)
                                                if (objData10HU != null)
                                                    // objData10HU.LM = PositionLeftParent
                                                    // objData10HU.CSM = PositionLeftParent
                                                    await _appContext.SaveChangesAsync();
                                            }
                                            if (obj6.LM == param.Id)
                                            {
                                                var objData11 = (from p in _appContext.Positions
                                                                 where p.ID == obj6.ID
                                                                 select p).FirstOrDefault();
                                                // _appContext.Positions.Attach(objData5)
                                                if (objData11 != null)
                                                {
                                                    // objData11.LM = PositionLeftParent
                                                    objData11.TYPE_ACTIVITIES = "UPDATE";
                                                    objData11.EFFECTIVE_DATE = sDate;
                                                    await _appContext.SaveChangesAsync();
                                                }

                                                var objData11HU = (from p in _appContext.Positions
                                                                   where p.ID == obj6.ID
                                                                   select p).FirstOrDefault();
                                                // _appContext.Positions.Attach(objData5)
                                                if (objData11HU != null)
                                                    // objData11HU.LM = PositionLeftParent
                                                    await _appContext.SaveChangesAsync();
                                            }
                                            if (obj6.CSM == param.Id)
                                            {
                                                var objData12 = (from p in _appContext.Positions
                                                                 where p.ID == obj6.ID
                                                                 select p).FirstOrDefault();
                                                // _appContext.Positions.Attach(objData6)
                                                if (objData12 != null)
                                                {
                                                    // objData12.CSM = PositionLeftParent
                                                    objData12.TYPE_ACTIVITIES = "UPDATE";
                                                    objData12.EFFECTIVE_DATE = sDate;
                                                    await _appContext.SaveChangesAsync();
                                                }

                                                var objData12HU = (from p in _appContext.Positions
                                                                   where p.ID == obj6.ID
                                                                   select p).FirstOrDefault();
                                                // _appContext.Positions.Attach(objData6)
                                                if (objData12HU != null)
                                                    // objData12HU.CSM = PositionLeftParent
                                                    await _appContext.SaveChangesAsync();
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    // Nếu Pos trưởng Org cha k có thì Update QLTT, QLPD của tất cả các Pos có nó là QLTT hoặc QLPD bằng null
                                    var lstPositionLeft = (from d in _appContext.Positions
                                                           where (d.LM == param.Id || d.CSM == param.Id) && d.IS_ACTIVE == true
                                                           select d).ToList();
                                    if (lstPositionLeft.Count > 0)
                                    {
                                        foreach (var obj6 in lstPositionLeft)
                                        {
                                            if (obj6.LM == param.Id && obj6.CSM == param.Id)
                                            {
                                                var objData10 = (from p in _appContext.Positions
                                                                 where p.ID == obj6.ID
                                                                 select p).FirstOrDefault();
                                                // _appContext.Positions.Attach(objData4)
                                                if (objData10 != null)
                                                {
                                                    // objData10.LM = Nothing
                                                    // objData10.CSM = Nothing
                                                    objData10.TYPE_ACTIVITIES = "UPDATE";
                                                    objData10.EFFECTIVE_DATE = sDate;
                                                    await _appContext.SaveChangesAsync();
                                                }

                                                var objData10HU = (from p in _appContext.Positions
                                                                   where p.ID == obj6.ID
                                                                   select p).FirstOrDefault();
                                                // _appContext.Positions.Attach(objData4)
                                                if (objData10HU != null)
                                                    // objData10HU.LM = Nothing
                                                    // objData10HU.CSM = Nothing
                                                    await _appContext.SaveChangesAsync();
                                            }
                                            if (obj6.LM == param.Id)
                                            {
                                                var objData11 = (from p in _appContext.Positions
                                                                 where p.ID == obj6.ID
                                                                 select p).FirstOrDefault();
                                                // _appContext.Positions.Attach(objData5)
                                                if (objData11 != null)
                                                {
                                                    // objData11.LM = Nothing
                                                    objData11.TYPE_ACTIVITIES = "UPDATE";
                                                    objData11.EFFECTIVE_DATE = sDate;
                                                    await _appContext.SaveChangesAsync();
                                                }

                                                var objData11HU = (from p in _appContext.Positions
                                                                   where p.ID == obj6.ID
                                                                   select p).FirstOrDefault();
                                                // _appContext.Positions.Attach(objData5)
                                                if (objData11HU != null)
                                                    // objData11HU.LM = Nothing
                                                    await _appContext.SaveChangesAsync();
                                            }
                                            if (obj6.CSM == param.Id)
                                            {
                                                var objData12 = (from p in _appContext.Positions
                                                                 where p.ID == obj6.ID
                                                                 select p).FirstOrDefault();
                                                // _appContext.Positions.Attach(objData6)
                                                if (objData12 != null)
                                                {
                                                    // objData12.CSM = Nothing
                                                    objData12.TYPE_ACTIVITIES = "UPDATE";
                                                    objData12.EFFECTIVE_DATE = sDate;
                                                    await _appContext.SaveChangesAsync();
                                                }

                                                var objData12HU = (from p in _appContext.Positions
                                                                   where p.ID == obj6.ID
                                                                   select p).FirstOrDefault();
                                                // _appContext.Positions.Attach(objData6)
                                                if (objData12HU != null)
                                                    // objData12HU.CSM = Nothing
                                                    await _appContext.SaveChangesAsync();
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    // Lấy Pos trưởng của đơn vị sau điều chuyển 
                    var PositionId = (from d in _appContext.Positions
                                      where d.ORG_ID == OrgIdRight && d.ISOWNER == true && d.IS_PLAN == false && d.IS_ACTIVE == true
                                      select d.ID).FirstOrDefault();
                    // Nếu có Pos trưởng thì QLTT, QLPD của nó là Pos trường
                    if (PositionId != 0)
                    {
                        var objData = (from p in _appContext.Positions
                                       where p.ID == param.Id
                                       select p).FirstOrDefault();
                        if (objData != null)
                        {
                            // _appContext.Positions.Attach(objData)
                            objData.ORG_ID = OrgIdRight;
                            objData.IS_PLAN = param.isplan;
                            objData.WORK_LOCATION = Address;
                            // objData.LM = PositionId
                            // objData.CSM = PositionId
                            objData.TYPE_ACTIVITIES = "UPDATE";
                            objData.EFFECTIVE_DATE = sDate;
                            await _appContext.SaveChangesAsync();
                        }
                    }
                    else
                    {
                        // Nếu k có Pos trường thì lấy Pos trưởng của Org cha
                        var OrgRightParent = (from o in _appContext.Organizations
                                              where o.ID == OrgIdRight
                                              select o.PARENT_ID).FirstOrDefault();
                        if (OrgRightParent != null)
                        {
                            var PositionIdParent = (from d in _appContext.Positions
                                                    where d.ORG_ID == OrgRightParent && d.ISOWNER == true && d.IS_PLAN == false && d.IS_ACTIVE == true
                                                    select d.ID).FirstOrDefault();
                            // Nếu có Pos trưởng Org cha thì QLTT, QLPD của nó là Pos trưởng Org cha
                            if (PositionIdParent != 0)
                            {
                                var objData = (from p in _appContext.Positions
                                               where p.ID == param.Id
                                               select p).FirstOrDefault();
                                if (objData != null)
                                {
                                    // _appContext.Positions.Attach(objData)
                                    objData.ORG_ID = OrgIdRight;
                                    objData.IS_PLAN = param.isplan;
                                    objData.WORK_LOCATION = Address;
                                    // objData.LM = PositionIdParent
                                    // objData.CSM = PositionIdParent
                                    objData.TYPE_ACTIVITIES = "UPDATE";
                                    objData.EFFECTIVE_DATE = sDate;
                                    await _appContext.SaveChangesAsync();
                                }
                            }
                            else
                            {
                                // Nếu k có Pos trưởng Org cha thì QLTT, QLPD của nó là null
                                var objData = (from p in _appContext.Positions
                                               where p.ID == param.Id
                                               select p).FirstOrDefault();
                                // _appContext.Positions.Attach(objData)
                                if (objData != null)
                                {
                                    objData.ORG_ID = OrgIdRight;
                                    objData.IS_PLAN = param.isplan;
                                    objData.WORK_LOCATION = Address;
                                    // objData.LM = Nothing
                                    // objData.CSM = Nothing
                                    objData.TYPE_ACTIVITIES = "UPDATE";
                                    objData.EFFECTIVE_DATE = sDate;
                                    await _appContext.SaveChangesAsync();
                                }
                            }
                        }
                        else
                        {
                            // Nếu k có Pos trưởng Org cha thì QLTT, QLPD của nó là null
                            var objData = (from p in _appContext.Positions
                                           where p.ID == param.Id
                                           select p).FirstOrDefault();
                            // _appContext.Positions.Attach(objData)
                            if (objData != null)
                            {
                                objData.ORG_ID = OrgIdRight;
                                objData.IS_PLAN = param.isplan;
                                objData.WORK_LOCATION = Address;
                                // objData.LM = Nothing
                                // objData.CSM = Nothing
                                objData.TYPE_ACTIVITIES = "UPDATE";
                                objData.EFFECTIVE_DATE = sDate;
                                await _appContext.SaveChangesAsync();
                            }
                        }
                    }
                }

                var r = await QueryData.ExecuteStore<PositionViewDTO>(Procedures.PKG_OMS_BUSINESS_JOB_AUTO_UPDATE_POSITION_TEMP, null);

                return new ResultWithError(200);
            }
            catch (Exception ex)
            {
                return new ResultWithError(ex.Message);
            }
        }
        public async Task<PagedResult<PositionViewDTO>> GetPositionOrgID(PositionViewDTO _filter)
        {
            string str = "(vacant)";
            string str1 = " (in hiring process)";
            string str2 = " (concurrent)";
            string str3 = "";
            IQueryable<PositionViewDTO> query = null;
            if (_filter.orgIdSearch == null || _filter.orgIdSearch < 0 || _filter.orgId2Search == null || _filter.orgId2Search < 0)
            {
                int? OrgId = 0;
                if (_filter.orgIdSearch != null)
                { OrgId = 0 - _filter.orgIdSearch; }
                if (_filter.orgId2Search != null)
                { OrgId = 0 - _filter.orgId2Search; }

                var lst = (from t in _appContext.Positions
                           from o in _appContext.Organizations.Where(x => x.ID == t.ORG_ID).DefaultIfEmpty()
                           from e in _appContext.Employees.Where(x => x.ID == t.MASTER).DefaultIfEmpty()
                           from a in _appContext.Employees.Where(x => x.ID == t.INTERIM).DefaultIfEmpty()
                           where t.IS_ACTIVE == true
                           select new PositionViewDTO
                           {
                               Id = t.ID,
                               Code = t.CODE,
                               Name = t.NAME,
                               NameEn = t.NAME_EN,
                               OrgId = t.ORG_ID,
                               OrgName = o.CODE + " (FTE Plan)",
                               Master = t.MASTER,
                               mastercode = t.MASTER != null ? e.CODE : "",
                               remark = e.Profile.FULL_NAME,
                               hiringStatus = t.HIRING_STATUS,
                               concurrent = t.CONCURRENT,
                               Interim = t.INTERIM,
                               InterimName = (t.INTERIM == null && t.MASTER == null ? str : (t.INTERIM != null ? a.CODE + " - " + a.Profile.FULL_NAME : str3)),
                               isowner = t.ISOWNER,
                               isplan = t.IS_PLAN,
                               flag = 1,
                               JobId = t.JOB_ID,
                               isnonphysical = t.IS_NONPHYSICAL,
                               both = t.ISOWNER == true ? 1 : 0,
                               MasterName = (t.INTERIM == null && t.MASTER == null ? str : (t.HIRING_STATUS == -1 ? str1 : (t.CONCURRENT == -1 ? e.CODE + " - " + e.Profile.FULL_NAME + str2 : (t.MASTER != null ? e.CODE + " - " + e.Profile.FULL_NAME : str3)))),
                               color = "#4BA838"
                           });

                //var orgIds = await QueryData.ExecuteList(Procedures.PKG_COMMON_LIST_ORG,
                //                                           new
                //                                           {
                //                                               P_IS_ADMIN = _appContext.IsAdmin == true ? 1 : 0,
                //                                               P_ORG_ID = _filter.orgid,
                //                                               P_CURENT_USER_ID = _appContext.CurrentUserId,
                //                                               P_CUR = QueryData.OUT_CURSOR
                //                                           }, false);


                //List<int?> ids = orgIds.Select(c => (int?)((dynamic)c).ID).ToList();

                query = lst;
                //if (_filter.orgid != null)
                //{
                //    ids.Add(_filter.orgid);
                //}
                //query = query.Where(p => ids.Contains(p.orgid));

                if (OrgId != null && OrgId > 0)
                {
                    query = query.Where(p => p.OrgId == OrgId);
                }

                if (_filter.textboxSearch != null)
                {
                    query = query.Where(f => f.Code.ToUpper().Contains(_filter.textboxSearch.ToUpper()) || f.Name.ToUpper().Contains(_filter.textboxSearch.ToUpper()) || f.MasterName.ToUpper().Contains(_filter.textboxSearch.ToUpper()) || f.mastercode.ToUpper().Contains(_filter.textboxSearch.ToUpper()));
                }
                if (_filter.textbox2Search != null)
                {
                    query = query.Where(f => f.Code.ToUpper().Contains(_filter.textbox2Search.ToUpper()) || f.Name.ToUpper().Contains(_filter.textbox2Search.ToUpper()) || f.MasterName.ToUpper().Contains(_filter.textbox2Search.ToUpper()) || f.mastercode.ToUpper().Contains(_filter.textbox2Search.ToUpper()));
                }
                if (_filter.isInterim != null)
                {
                    query = query.Where(f => f.Interim != null);
                }
                if (_filter.isMaster != null)
                {
                    query = query.Where(f => f.Master != null);
                }
                if (_filter.isConcurrently != null)
                {
                    query = query.Where(f => f.concurrent != null);
                }
            }
            else
            {
                decimal? OrgId = 0;
                if (_filter.orgIdSearch != null)
                    OrgId = _filter.orgIdSearch;
                if (_filter.orgId2Search != null)
                    OrgId = _filter.orgId2Search;

                var lst1 = (from t in _appContext.Positions
                            from o in _appContext.Organizations.Where(x => x.ID == t.ORG_ID).DefaultIfEmpty()
                            from e in _appContext.Employees.Where(x => x.ID == t.MASTER).DefaultIfEmpty()
                            from a in _appContext.Employees.Where(x => x.ID == t.INTERIM).DefaultIfEmpty()
                            where t.IS_ACTIVE == true
                            select new PositionViewDTO
                            {
                                Id = t.ID,
                                Code = t.CODE,
                                Name = t.NAME,
                                NameEn = t.NAME_EN,
                                OrgId = t.ORG_ID,
                                OrgName = t.IS_PLAN == true ? o.CODE + " (FTE Plan)" : o.CODE,
                                Master = t.MASTER,
                                mastercode = t.MASTER != null ? e.CODE : "",
                                remark = e.Profile.FULL_NAME,
                                hiringStatus = t.HIRING_STATUS,
                                concurrent = t.CONCURRENT,
                                Interim = t.INTERIM,
                                InterimName = (t.INTERIM == null && t.MASTER == null ? str : (t.INTERIM != null ? a.CODE + " - " + a.Profile.FULL_NAME : str3)),
                                isowner = t.ISOWNER,
                                isplan = t.IS_PLAN,
                                flag = 1,
                                JobId = t.JOB_ID,
                                isnonphysical = t.IS_NONPHYSICAL,
                                both = t.ISOWNER == true ? 1 : 0,
                                MasterName = (t.INTERIM == null && t.MASTER == null ? str : (t.HIRING_STATUS == -1 ? str1 : (t.CONCURRENT == -1 ? e.CODE + " - " + e.Profile.FULL_NAME + str2 : (t.MASTER != null ? e.CODE + " - " + e.Profile.FULL_NAME : str3)))),
                                color = (t.HIRING_STATUS == -1 ? "#0000FF" : (t.IS_PLAN == true ? "#4BA838" : (t.MASTER.HasValue && t.CONCURRENT == -1 ? "#4BA838" : "#070101")))
                            });

                query = lst1;
                //var orgIds = await QueryData.ExecuteList(Procedures.PKG_COMMON_LIST_ORG,
                //                                           new
                //                                           {
                //                                               P_IS_ADMIN = _appContext.IsAdmin == true ? 1 : 0,
                //                                               P_ORG_ID = _filter.orgid,
                //                                               P_CURENT_USER_ID = _appContext.CurrentUserId,
                //                                               P_CUR = QueryData.OUT_CURSOR
                //                                           }, false);


                //List<int?> ids = orgIds.Select(c => (int?)((dynamic)c).ID).ToList();

                //if (_filter.orgid != null)
                //{
                //    ids.Add(_filter.orgid);
                //}
                //query = query.Where(p => ids.Contains(p.orgid));

                if (OrgId != null && OrgId > 0)
                {
                    query = query.Where(p => p.OrgId == OrgId);
                }

                if (_filter.textboxSearch != null)
                {
                    query = query.Where(f => f.Code.ToUpper().Contains(_filter.textboxSearch.ToUpper()) || f.Name.ToUpper().Contains(_filter.textboxSearch.ToUpper()) || f.MasterName.ToUpper().Contains(_filter.textboxSearch.ToUpper()) || f.mastercode.ToUpper().Contains(_filter.textboxSearch.ToUpper()));
                }
                if (_filter.textbox2Search != null)
                {
                    query = query.Where(f => f.Code.ToUpper().Contains(_filter.textbox2Search.ToUpper()) || f.Name.ToUpper().Contains(_filter.textbox2Search.ToUpper()) || f.MasterName.ToUpper().Contains(_filter.textbox2Search.ToUpper()) || f.mastercode.ToUpper().Contains(_filter.textbox2Search.ToUpper()));
                }
                if (_filter.isInterim != null)
                {
                    query = query.Where(f => f.Interim != null);
                }
                if (_filter.isMaster != null)
                {
                    query = query.Where(f => f.Master != null);
                }
                if (_filter.isConcurrently != null)
                {
                    query = query.Where(f => f.concurrent != null);
                }
            }
            return await PagingList(query, _filter);
        }
        public async Task<ResultWithError> GetPositionByPositionID(decimal Id)
        {
            try
            {
                Task<PositionOutputDTO> query = null;
                if (Id == null || Id < 0)
                {
                    var positionId = 0 - Id;
                    await (query = (from t in _appContext.Positions
                                    from o in _appContext.Organizations.Where(f => f.ID == t.ORG_ID).DefaultIfEmpty()
                                    from e in _appContext.Employees.Where(f => f.ID == t.MASTER).DefaultIfEmpty()
                                    where t.ID == positionId
                                    select new PositionOutputDTO
                                    {
                                        Id = t.ID,
                                        Code = t.CODE,
                                        Name = t.NAME,
                                        NameEn = t.NAME_EN,
                                        OrgId = t.ORG_ID,
                                        orgname = o.NAME + " (FTE Plan)",
                                        master = t.MASTER,
                                        mastercode = e.CODE,
                                        MasterName = ReturnString(t.MASTER, t.HIRING_STATUS, e.Profile.FULL_NAME, t.CONCURRENT),
                                        remark = e.Profile.FULL_NAME,
                                        hiringStatus = t.HIRING_STATUS,
                                        concurrent = t.CONCURRENT,
                                        isowner = t.ISOWNER,
                                        isplan = t.IS_PLAN,
                                        color = "#4BA838"
                                    }).FirstOrDefaultAsync());

                }
                else
                {

                    await (query = (from t in _appContext.Positions
                                    from o in _appContext.Organizations.Where(f => f.ID == t.ORG_ID).DefaultIfEmpty()
                                    from e in _appContext.Employees.Where(f => f.ID == t.MASTER).DefaultIfEmpty()
                                    where t.ID == Id
                                    select new PositionOutputDTO
                                    {
                                        Id = t.ID,
                                        Code = t.CODE,
                                        Name = t.NAME,
                                        NameEn = t.NAME_EN,
                                        OrgId = t.ORG_ID,
                                        orgname = (t.IS_PLAN == true ? o.NAME + "(FTE Plan)" : o.NAME),
                                        master = t.MASTER,
                                        mastercode = e.CODE,
                                        MasterName = ReturnString(t.MASTER, t.HIRING_STATUS, e.Profile.FULL_NAME, t.CONCURRENT),
                                        remark = e.Profile.FULL_NAME,
                                        hiringStatus = t.HIRING_STATUS,
                                        concurrent = t.CONCURRENT,
                                        isowner = t.ISOWNER,
                                        isplan = t.IS_PLAN,
                                        color = (t.HIRING_STATUS == -1 ? "#0000FF" : (t.MASTER.HasValue && t.CONCURRENT == -1 ? "#4BA838" : "#070101"))
                                    }).FirstOrDefaultAsync());
                }
                return new ResultWithError(query);
            }
            catch (Exception ex)
            {
                return new ResultWithError(ex.Message);
            }
        }
        public string ReturnString(decimal? MASTER, decimal? HIRING_STATUS, string MASTER_NAME, decimal? CONCURRENT)
        {
            try
            {
                string str = " ";
                if (MASTER == null)
                    str = "(vacant)";
                else if (HIRING_STATUS == -1)
                    str = "(in hiring process)";
                else if (MASTER != null & CONCURRENT != -1)
                    str = MASTER_NAME;
                else if (MASTER != null & CONCURRENT == -1)
                    str = MASTER_NAME + " (concurrent)";
                return str;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<ResultWithError> GetOrgOMByID(decimal _Id)
        {
            try
            {
                var CompanyLevel = GetCompanyLevel(_Id);

                decimal Id;
                if (_Id == null || _Id < 0)
                    Id = 0 - _Id;
                else
                    Id = _Id;

                var query = await (from p in _appContext.Organizations
                                   from o in _appContext.ViewOrganizations.Where(f => f.ID == p.ID).DefaultIfEmpty()
                                   from o2 in _appContext.Organizations.Where(f => f.ID == o.ORG_ID2).DefaultIfEmpty()
                                   where p.ID == Id
                                   select new OrganizationDTO
                                   {
                                       Id = p.ID,
                                       Code = p.CODE,
                                       Name = p.NAME,
                                       Name2 = o2.NAME,
                                       orgLvlName = CompanyLevel,
                                       shortName = o2.SHORT_NAME
                                   }).FirstOrDefaultAsync();
                return new ResultWithError(query);
            }
            catch (Exception ex)
            {
                return new ResultWithError(ex.Message);
            }
        }
        public string GetCompanyLevel(decimal orgID)
        {
            try
            {
                var companyId = (from p in _appContext.OtherLists
                                 where p.TYPE_ID == 43 && p.CODE == "COM"
                                 select p.ID).FirstOrDefault();
                var orgLevel = (from p in _appContext.Organizations
                                where orgID == p.ID
                                select p).FirstOrDefault();
                if (companyId == orgLevel.LEVEL_ORG)
                    return orgLevel.NAME;
                else if (orgLevel.PARENT_ID != null)
                    return GetCompanyLevel((decimal)orgLevel.PARENT_ID);
                else
                    return "";
            }
            catch (Exception ex)
            {
                return "";
            }
        }
        public bool CheckIsOwner(decimal OrgId)
        {
            try
            {
                if (OrgId > 0)
                {
                    var exits = from t in _appContext.Positions
                                where t.IS_PLAN == false && t.ISOWNER == true && t.IS_ACTIVE == true && t.ORG_ID == (int)OrgId
                                select t;
                    if (exits?.ToList().Count > 0)
                        return true;
                    return false;
                }
                else
                {
                    var exits = from t in _appContext.Positions
                                where t.IS_PLAN == true && t.ISOWNER == true && t.IS_ACTIVE == true && t.ORG_ID == (int)OrgId
                                select t;
                    if (exits?.ToList().Count > 0)
                        return true;
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        // Nhân bản
        public async Task<ResultWithError> InsertPositionNB(PositionInputDTO obj, int OrgRight, int Address, int OrgIDDefault = 1, int IsDissolveDefault = 0)
        {
            DateTime sDate = DateTime.Now;
            int iCount = 0;
            int? OrgIdRight;
            int? gID;
            if (OrgRight == null || OrgRight < 0)
                OrgIdRight = 0 - OrgRight;
            else
                OrgIdRight = OrgRight;

            var orgIds = await QueryData.ExecuteList(Procedures.PKG_COMMON_LIST_ORG,
                                                new
                                                {
                                                    P_IS_ADMIN = _appContext.IsAdmin == true ? 1 : 0,
                                                    P_ORG_ID = obj.OrgId,
                                                    P_CURENT_USER_ID = _appContext.CurrentUserId,
                                                    P_CUR = QueryData.OUT_CURSOR
                                                }, false);


            List<long?> ids = orgIds.Select(c => (long?)((dynamic)c).ID).ToList();


            // Check quyen du lieu
            List<int?> query = (from p in _appContext.Positions select (int?)p.ID).ToList();
            if (obj.OrgId != null)
            {
                ids.Add(obj.OrgId);
            }
            query = (List<int?>)query.Where(p => ids.Contains(p.Value));

            if (query.Count < 1)
            { return new ResultWithError(400); }

            orgIds = await QueryData.ExecuteList(Procedures.PKG_COMMON_LIST_ORG,
                                                new
                                                {
                                                    P_IS_ADMIN = _appContext.IsAdmin == true ? 1 : 0,
                                                    P_ORG_ID = OrgRight,
                                                    P_CURENT_USER_ID = _appContext.CurrentUserId,
                                                    P_CUR = QueryData.OUT_CURSOR
                                                }, false);


            ids = orgIds.Select(c => (long?)((dynamic)c).ID).ToList();

            if (ids.Count < 1)
            { return new ResultWithError(400); }

            try
            {
                if (obj.isowner == true)
                {
                    var OrgParent = (from o in _appContext.Organizations
                                     where o.ID == OrgIdRight
                                     select o.PARENT_ID).FirstOrDefault();
                    if (OrgParent != null)
                    {
                        var PositionId = (from t in _appContext.Positions
                                          where t.ORG_ID == OrgParent && t.ISOWNER == true && t.IS_ACTIVE == true && t.IS_PLAN == false
                                          select t.ID).FirstOrDefault();
                        // Nếu Org cha có Pos trưởng thì QLTT và QLPD là Pos trưởng của Org cha
                        if (PositionId != 0)
                        {
                            obj.code = AutoGenCodeHuTile("HU_Position", "CODE");
                            obj.isActive = -1;
                            obj.OrgId = OrgIdRight;
                            obj.workLocation = Address;
                            obj.lm = (int?)PositionId;
                            obj.csm = PositionId;
                            obj.typeactivities = "ADDNEW";
                            obj.effectivedate = sDate;
                            var data = Map(obj, new HU_POSITION());
                            var result = await _appContext.Positions.AddAsync(data);
                            await _appContext.SaveChangesAsync();
                            gID = (int)data.ID;
                        }
                        else
                        {
                            // Nếu Org cha k có trưởng thì QLTT và QLPD để null
                            obj.code = AutoGenCodeHuTile("HU_Position", "CODE");
                            obj.isActive = -1;
                            obj.OrgId = OrgIdRight;
                            obj.workLocation = Address;
                            obj.lm = null;
                            obj.csm = null;
                            obj.typeactivities = "ADDNEW";
                            obj.effectivedate = sDate;
                            var data = Map(obj, new HU_POSITION());
                            var result = await _appContext.Positions.AddAsync(data);
                            await _appContext.SaveChangesAsync();
                            gID = (int)data.ID;
                        }
                        // update QLPD và QLTT của tất cả các Pos cùng đơn vị có Pos chuyển đến
                        var lstPosition = (from t in _appContext.Positions
                                           where t.ORG_ID == OrgIdRight && t.ISOWNER == false && t.IS_ACTIVE == true
                                           select t).ToList();
                        if (lstPosition.Count > 0)
                        {
                            foreach (var item in lstPosition)
                            {
                                var objData1 = (from p in _appContext.Positions
                                                where p.ID == item.ID
                                                select p).FirstOrDefault();
                                if (objData1 != null)
                                {
                                    objData1.LM = gID;
                                    objData1.CSM = gID;
                                    objData1.TYPE_ACTIVITIES = "UPDATE";
                                    objData1.EFFECTIVE_DATE = sDate;
                                    await _appContext.SaveChangesAsync();
                                }
                            }
                        }
                        if (obj.isplan == false)
                        {
                            // Update QLTT và QLPD tất cả các Pos là trưởng phòng của tất cả các đơn vị con của đơn vị có Pos chuyển đến
                            var lstOrg = (from o in _appContext.Organizations
                                          where (o.PARENT_ID == OrgIdRight)
                                          select o.ID).ToList();
                            if (lstOrg.Count > 0)
                            {
                                foreach (decimal item1 in lstOrg)
                                {
                                    List<HU_POSITION> lstPositionChild = (from d in _appContext.Positions
                                                                          where d.ORG_ID == item1 && d.ISOWNER == true && d.IS_ACTIVE == true
                                                                          select d).ToList();
                                    if (lstPositionChild.Count > 0)
                                    {
                                        foreach (var item2 in lstPositionChild)
                                        {
                                            var objData2 = (from p in _appContext.Positions
                                                            where p.ID == item2.ID
                                                            select p).FirstOrDefault();
                                            if (objData2 != null)
                                            {
                                                objData2.LM = gID;
                                                objData2.CSM = gID;

                                                var objData = (from p in _appContext.Positions
                                                               where p.ID == gID
                                                               select p).FirstOrDefault();
                                                if (objData != null)
                                                {
                                                    objData.TYPE_ACTIVITIES = "UPDATE";
                                                    objData.EFFECTIVE_DATE = sDate;
                                                }

                                                await _appContext.SaveChangesAsync();

                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    var PositionId = (from t in _appContext.Positions
                                      where t.ORG_ID == OrgIdRight && t.ISOWNER == true && t.IS_ACTIVE == true && t.IS_PLAN == false
                                      select t.ID).FirstOrDefault();
                    // Nếu có Pos trưởng thì QLTT,QLPD là Pos trưởng
                    if (PositionId != 0)
                    {
                        obj.code = AutoGenCodeHuTile("HU_POSITION", "CODE");
                        obj.isActive = -1;
                        obj.OrgId = OrgIdRight;
                        obj.workLocation = Address;
                        obj.lm = (int?)PositionId;
                        obj.csm = PositionId;
                        obj.typeactivities = "ADDNEW";
                        obj.effectivedate = sDate;
                        var data = Map(obj, new HU_POSITION());
                        var result = await _appContext.Positions.AddAsync(data);
                        await _appContext.SaveChangesAsync();
                        gID = (int)data.ID;

                    }
                    else
                    {
                        // Nếu có k có Pos trưởng, thì QLTT và QLPD là Pos trưởng Org cha 
                        var OrgRightParent = (from o in _appContext.Organizations
                                              where o.ID == OrgIdRight
                                              select o.PARENT_ID).FirstOrDefault();
                        if (OrgRightParent != null)
                        {
                            var PositionIdParent = (from d in _appContext.Positions
                                                    where d.ORG_ID == OrgRightParent && d.ISOWNER == true && d.IS_PLAN == false && d.IS_ACTIVE == true
                                                    select d.ID).FirstOrDefault();
                            if (PositionIdParent != 0)
                            {

                                obj.code = AutoGenCodeHuTile("HU_Position", "CODE");
                                obj.isActive = -1;
                                obj.OrgId = OrgIdRight;
                                obj.workLocation = Address;
                                obj.lm = (int?)PositionIdParent;
                                obj.csm = PositionIdParent;
                                obj.typeactivities = "ADDNEW";
                                obj.effectivedate = sDate;
                                var data = Map(obj, new HU_POSITION());
                                var result = await _appContext.Positions.AddAsync(data);
                                await _appContext.SaveChangesAsync();
                                gID = (int)data.ID;

                            }
                            else
                            {
                                obj.code = AutoGenCodeHuTile("HU_Position", "CODE");
                                obj.isActive = -1;
                                obj.OrgId = OrgIdRight;
                                obj.workLocation = Address;
                                obj.lm = null;
                                obj.csm = null;
                                obj.typeactivities = "ADDNEW";
                                obj.effectivedate = sDate;
                                var data = Map(obj, new HU_POSITION());
                                var result = await _appContext.Positions.AddAsync(data);
                                await _appContext.SaveChangesAsync();
                                gID = (int)data.ID;
                            }
                        }
                    }
                }
                var r = await QueryData.ExecuteStore<PositionViewDTO>(Procedures.PKG_OMS_BUSINESS_JOB_AUTO_UPDATE_POSITION_TEMP, null);
                return new ResultWithError(200);
            }
            catch (Exception ex)
            {
                return new ResultWithError(ex.Message);
            }
        }
        public async Task<ResultWithError> GetOrgTreeApp(string sLang)
        {
            try
            {
                var dtData = QueryData.ExecuteStoreToTable("PKG_PROFILE.GET_ORG_TREE_APP",
                    new
                    {
                        P_LANGUAGE = sLang,
                        P_USERNAME = _appContext.UserName,
                        PV_CUR = QueryData.OUT_CURSOR
                    }, false);

                if (dtData.Tables.Count > 0)
                {
                    var table = dtData.Tables[0];
                    var lst = (from DataRow dr in table.Rows
                               select new OrganizationDTO
                               {
                                   Id = Convert.ToInt32(dr["ID"]),
                                   ParentId = dr["PARENT_ID"].ToString() != "" ? Convert.ToInt32(dr["PARENT_ID"]) : null,
                                   Name = dr["NAME"].ToString(),
                                   groupProject = dr["GROUPPROJECT"].ToString() != "" ? Convert.ToInt32(dr["GROUPPROJECT"]) : null,
                               }).ToList();
                    return new ResultWithError(lst);
                }
                else
                {
                    return new ResultWithError(200);
                }
            }
            catch (Exception ex)
            {
                return new ResultWithError(ex);
            }
        }
        public async Task<ResultWithError> SwapMasterInterim(PositionInputDTO param)
        {
            try
            {
                foreach (var item in param.Ids)
                {
                    var itemInfor = (from p in _appContext.Positions where p.ID == item select p).FirstOrDefault();
                    if (itemInfor != null && (itemInfor.MASTER != null || itemInfor.INTERIM != null))
                    {
                        var interim = itemInfor.INTERIM;
                        var master = itemInfor.MASTER;
                        itemInfor.MASTER = interim;
                        itemInfor.INTERIM = master;
                        var result = _appContext.Positions.Update(itemInfor);
                    }
                }

                await _appContext.SaveChangesAsync();
                return new ResultWithError(param);
            }
            catch (Exception ex)
            {

                return new ResultWithError(ex);
            }
        }
        public async Task<ResultWithError> CheckTdvAsync(PositionInputDTO param)
        {
            try
            {
                if (param.isTDV == true)
                {
                    var itemInfor = (from p in _appContext.Positions where p.IS_TDV == true && p.ORG_ID == param.OrgId && p.ID != param.Id select p.NAME).ToList();
                    if (itemInfor != null && itemInfor.Count > 0)
                    {
                        return new ResultWithError(200, System.Environment.NewLine + string.Join(System.Environment.NewLine, itemInfor.ToArray()) + System.Environment.NewLine);
                    }
                }
                return new ResultWithError(204);
            }
            catch (Exception ex)
            {

                return new ResultWithError(ex);
            }
        }

    }
}