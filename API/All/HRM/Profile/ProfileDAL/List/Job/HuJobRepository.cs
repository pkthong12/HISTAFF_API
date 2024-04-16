using Common.Paging;
using ProfileDAL.ViewModels;
using Common.Repositories;
using Common.Extensions;
using API.All.DbContexts;
using API.Entities;
using CORE.DTO;
using CORE.GenericUOW;

namespace ProfileDAL.Repositories
{
    public class HuJobRepository : RepositoryBase<HU_JOB>, IHuJobRepository
    {
        private ProfileDbContext _appContext => (ProfileDbContext)_context;
        private readonly ILogger _logger;
        private readonly GenericReducer<HU_JOB, HUJobInputDTO> genericReducer;
        public HuJobRepository(ProfileDbContext context) : base(context)
        {
            genericReducer = new();
        }

        public async Task<GenericPhaseTwoListResponse<HUJobInputDTO>> SinglePhaseQueryList(GenericQueryListDTO<HUJobInputDTO> request)
        {
            var queryable = from p in _appContext.HUJobs
                            from o in _appContext.OtOtherLists.Where(x => x.ID == p.JOB_FAMILY_ID).DefaultIfEmpty()
                            from gp in _appContext.GroupPositions.Where(x => x.ID == p.PHAN_LOAI_ID).DefaultIfEmpty()
                            orderby p.ID descending
                            select new HUJobInputDTO
                                {
                                    Id = p.ID,
                                    NameVN = p.NAME_VN,
                                    NameEN = p.NAME_EN,
                                    Actflg = p.ACTFLG == "A" ? "Áp dụng" : "Ngừng áp dụng",
                                    Code = p.CODE,
                                    CreatedDate = p.CREATED_DATE,
                                    Note = p.NOTE,
                                    PhanLoaiName= gp.NAME,
                                    LevelID = p.LEVEL_ID,
                                    JobFamilyID = o.NAME,
                                    Purpose = p.PURPOSE
                            };
            // nếu dùng if else thay đổi giá trị cột thì phải select lại 1 lần nữa thì filter mới được
            var query = from p in queryable
                        select new HUJobInputDTO
                        {
                            Id = p.Id,
                            NameVN = p.NameVN,
                            NameEN = p.NameEN,
                            Actflg = p.Actflg,
                            Code = p.Code,
                            CreatedDate = p.CreatedDate,
                            Note = p.Note,
                            PhanLoaiName = p.PhanLoaiName,
                            LevelID = p.LevelID,
                            JobFamilyID = p.JobFamilyID,
                            Purpose = p.Purpose
                        };
            var singlePhaseResult = await genericReducer.SinglePhaseReduce(query, request);
            return singlePhaseResult;
        }

        public async Task<PagedResult<HUJobInputDTO>> GetJobs(HUJobInputDTO param)
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
                if (param.NameVN != null)
                {
                    queryable = queryable.Where(p => p.NameVN.ToUpper().Contains(param.NameVN.ToUpper()));
                }

                if (param.NameEN != null)
                {
                    queryable = queryable.Where(p => p.NameEN.ToUpper().Contains(param.NameEN.ToUpper()));
                }

                if (param.Code != null)
                {
                    queryable = queryable.Where(p => p.Code.ToUpper().Contains(param.Code.ToUpper()));
                }

                if (param.ActflgStr != null)
                {
                    queryable = queryable.Where(p => p.ActflgStr.ToUpper().Contains(param.ActflgStr.ToUpper()));
                }

                if (param.Note != null)
                {
                    queryable = queryable.Where(p => p.Note.ToUpper().Contains(param.Note.ToUpper()));
                }

                return await PagingList(queryable, param);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString(), param);
                return null;
            }
        }

        public async Task<ResultWithError> GetJob(int id)
        {
            try
            {
                var query = await (from p in _appContext.HUJobs
                                   where p.ID == id
                                   select new HUJobDTO
                                   {
                                       Id = p.ID,
                                       NameVN = p.NAME_VN,
                                       NameEN = p.NAME_EN,
                                       Actflg = p.ACTFLG,
                                       Purpose = p.PURPOSE,
                                       Code = p.CODE,
                                       typeId = p.JOB_FAMILY_ID,
                                       Note = p.NOTE
                                   }).FirstOrDefaultAsync();

                var query_child = await (from p in _appContext.HUJobFunction
                                   where p.JOB_ID == id
                                   select new HUJobFunctionDTO
                                   {
                                       Id = p.ID,
                                       Name = p.NAME,
                                       NameEN = p.NAME_EN,
                                       FunctionID = p.FUNCTION_ID,
                                       ParentID = p.PARENT_ID,
                                       JobID = p.JOB_ID
                                   }).ToListAsync();

                query.Child = query_child;
                return new ResultWithError(query);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return new ResultWithError(ex.Message);
            }
        }
        public async Task<ResultWithError> GetJobById(long id)
        {
            var r = await (from p in _appContext.HUJobs
                                   where p.ID == id
                                   select new
                                   {
                                       Id = p.ID,
                                       code=p.CODE,
                                       NameVn = p.CODE + " - " +p.NAME_VN,
                                       NameVnNoCode = p.NAME_VN,
                                       NameEn = p.NAME_EN,
                                       PhanloaiId = p.PHAN_LOAI_ID,
                                       JobFamilyId = p.JOB_FAMILY_ID,
                                       Note = p.NOTE,
                                       levelId = p.LEVEL_ID,
                                       Purpose = p.PURPOSE,
                                       Ordernum = p.ORDERNUM,
                                       Actflg = p.ACTFLG
                                   }).FirstOrDefaultAsync();
            return new ResultWithError(r);
        }
        public async Task<ResultWithError> UpdateAsync(HUJobEditDTO param)
        {
            try
            {
                if (param.Id != 0 && param.Id != null )
                {
                    var itemInfor = _appContext.HUJobs.Where(x => x.ID == param.Id).FirstOrDefault();
                    var data = Map(param, itemInfor);
                    var result = _appContext.HUJobs.Update(data);
                    await _appContext.SaveChangesAsync();
                }
                else
                {
                    var data = Map(param, new HU_JOB());
                    var result = await _appContext.HUJobs.AddAsync(data);
                    await _appContext.SaveChangesAsync();
                    return new ResultWithError(data);
                }
                return new ResultWithError(param);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return new ResultWithError(ex.Message);
            }
        }

        public async Task<ResultWithError> ChangeStatusAsync(HUJobDTO request)
        {
            try
            {
                var query = from p in _appContext.HUJobs where request.Ids.Contains(p.ID) select p;
                foreach (var item in query)
                {
                    item.ACTFLG = request.ValueToBind == true ? "A" : "I";
                    var result = _appContext.HUJobs.Update(item);
                }
                await _appContext.SaveChangesAsync();
                return new ResultWithError(request);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return new ResultWithError(ex.Message);
            }
        }

        public async Task<ResultWithError> DeleteAsync(List<long> ids)
        {
            try
            {
                var query = from p in _appContext.HUJobs where ids.Contains(p.ID) select p;
                foreach (var item in query)
                {
                    _appContext.HUJobs.Remove(item);
                }
                await _appContext.SaveChangesAsync();
                return new ResultWithError(200);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return new ResultWithError(ex.Message);
            }
        }

        public async Task<bool> ValidateJob(HUJobEditDTO param)
        {
            try
            {
                var c = _appContext.HUJobs.Where(x => x.CODE.ToLower().Equals(param.Code.ToLower()) && x.ID != param.Id);
                return await c.AnyAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return false;
            }
        }
        /// <summary>
        /// Get List Group is Activce
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<ResultWithError> GetList()
        {
            try
            {
                var queryable = await (from p in _appContext.HUJobs
                                       where p.ACTFLG == "A"
                                       select new
                                       {
                                           Id = p.ID,
                                           Name = p.NAME_VN,
                                           Code = p.CODE
                                       }).ToListAsync();
                return new ResultWithError(queryable);
            }
            catch (Exception ex)
            {
                return new ResultWithError(ex.Message);
            }
        }

        public async Task<FormatedResponse> GetCodeByJobFamily(long id)
        {
            try
            {
                string code = "";
                string newCode = "";
                var jobFamily = await (from p in _appContext.OtherLists
                                       where p.ID == id
                                       select new
                                       {
                                           Id = p.ID,
                                           Code = p.CODE,
                                           Name = p.NAME,
                                       }).FirstOrDefaultAsync();
                switch (jobFamily!.Code)
                {
                    case "00338":
                        code = "BKS";
                        break;
                    case "00339":
                        code = "NLD";
                        break;
                    case "00340":
                        code = "DDT";
                        break;
                    case "00341":
                        code = "NDDV";
                        break;
                    case "00342":
                        code = "K";
                        break;
                    case "00343":
                        code = "NQL";
                        break;
                    case "00344":
                        code = "BP";
                        break;
                    default:
                        code = "";
                        break;
                }
                var lastestJob = _appContext.HUJobs.FirstOrDefault(x => x.CODE.Contains(code));
                if(lastestJob == null)
                {
                    newCode = code + "0001";
                }
                else
                {
                    string lastestData = _appContext.HUJobs.Where(x => x.CODE.Contains(code)).OrderByDescending(t => t.CODE).First().CODE!.ToString();

                    string newNumber = (Int32.Parse(lastestData.Substring(code.Length)) + 1).ToString();
                    while (newNumber.Length < 4)
                    {
                        newNumber = "0" + newNumber;
                    }
                    newCode = lastestData.Substring(0, code.Length) + newNumber;
                }

                return new FormatedResponse() { InnerBody = new { Code = newCode } };
            }
            catch (Exception ex)
            {
                return new FormatedResponse() { MessageCode = ex.Message };
            }
        }
    
    }
}
