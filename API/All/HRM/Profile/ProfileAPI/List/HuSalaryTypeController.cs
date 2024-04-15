using Microsoft.AspNetCore.Mvc;
using ProfileDAL.ViewModels;
using API.All.DbContexts;
using API.Entities;
using CORE.DTO;
using CORE.Enum;
using CORE.GenericUOW;
using CORE.StaticConstant;
using Common.Extensions;
using API.DTO;
using API.Main;
using API;
using Microsoft.Extensions.Options;
using PayrollDAL.Models;
using System.Text.RegularExpressions;
using Hangfire.Storage;
using System.Linq.Dynamic.Core;

namespace ProfileAPI.List
{
    [HiStaffAuthorize]
    [ApiExplorerSettings(GroupName = "002-PROFILE-SALARY")]
    [ApiController]

    [Route("api/[controller]/[action]")]
    public class HuSalaryTypeController : ControllerBase
    {
        private readonly GenericUnitOfWork _uow;
        private readonly ProfileDbContext _profileDbContext;
        private IGenericRepository<HU_SALARY_TYPE, SalaryTypeDTO> _genericRepository;
        private readonly GenericReducer<HU_SALARY_TYPE, SalaryTypeDTO> genericReducer;
        private AppSettings _appSettings;
        public HuSalaryTypeController(IOptions<AppSettings> options, ProfileDbContext profileDbContext)
        {
            _profileDbContext = profileDbContext;
            _uow = new GenericUnitOfWork(_profileDbContext);
            _genericRepository = _uow.GenericRepository<HU_SALARY_TYPE, SalaryTypeDTO>();
            genericReducer = new();
            _appSettings = options.Value;
        }
        
        [HttpPost]
        public async Task<IActionResult> QueryList(GenericQueryListStringIdDTO<SalaryTypeDTO> request)
        {
            try
            {
                var entity = _uow.Context.Set<HU_SALARY_TYPE>().AsNoTracking().AsQueryable();
                var otherlists = _uow.Context.Set<SYS_OTHER_LIST>().AsNoTracking().AsQueryable();
                var joined = from p in entity
                             from c in otherlists.Where(x => x.ID == p.SALARY_TYPE_GROUP).DefaultIfEmpty()
                             orderby p.CREATED_DATE descending
                             select new SalaryTypeDTO
                             {
                                 Id = p.ID,
                                 Code = p.CODE,
                                 Name = p.NAME,
                                 Note = p.NOTE,
                                 SalaryTypeGroup = p.SALARY_TYPE_GROUP,
                                 SalaryTypeGroupName = c.NAME,
                                 Description = p.DESCRIPTION,
                                 IsActive = p.IS_ACTIVE,
                                 EffectDate = p.EFFECT_DATE,
                                 UpdatedBy = p.UPDATED_BY,
                                 UpdatedDate = p.UPDATED_DATE,
                                 CreateDate = p.CREATED_DATE,
                                 Status = p.IS_ACTIVE == true ? "Áp dụng" : "Ngừng áp dụng"
                             };
                request.Sort = new List<SortItem>();
                request.Sort.Add(new SortItem() { Field = "CreateDate", SortDirection = EnumSortDirection.DESC });
                var response = await genericReducer.SinglePhaseReduce(joined, request);
                if (response.ErrorType != EnumErrorType.NONE)
                {
                    return Ok(new FormatedResponse()
                    {
                        ErrorType = response.ErrorType,
                        MessageCode = response.MessageCode ?? CommonMessageCode.NOT_ALL_CASES_CATCHED,
                        StatusCode = EnumStatusCode.StatusCode400,
                    });
                }
                else
                {
                    return Ok(new FormatedResponse()
                    {
                        InnerBody = response
                    });
                }
            }
            catch (Exception ex)
            {
                return Ok(new FormatedResponse() { ErrorType = EnumErrorType.UNCATCHABLE, StatusCode = EnumStatusCode.StatusCode500, MessageCode = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> CreateCodeAuto()
        {
            try
            {
                decimal num;
                string str;
                var rs = "ĐTL001";

                var entity = _uow.Context.Set<HU_SALARY_TYPE>().AsNoTracking().AsQueryable();
                var joined = (from p in entity
                              where (p.CODE != null || p.CODE != "") && p.CODE.Length == 6
                              select new SalaryTypeDTO
                              {
                                  Id = p.ID,
                                  Name = p.NAME,
                                  Code = p.CODE
                              }).ToList();
                var maxCode = (from p in joined where Regex.IsMatch(p.Code, @"(ĐTL)(\d{3}$)") orderby p.Code.Substring(3, 3) descending select p.Code.Substring(3, 3)).FirstOrDefault();
                if (maxCode != null)
                {
                    rs = "ĐTL" + (int.Parse(maxCode) + 1).ToString("000");
                }
                return Ok(new FormatedResponse() { InnerBody = new { Code = rs } });
            }
            catch (Exception ex)
            {
                return Ok(new FormatedResponse() { MessageCode = ex.Message, ErrorType = EnumErrorType.UNCATCHABLE, StatusCode = EnumStatusCode.StatusCode500 });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetById(long id)
        {
            try
            {
                var entity = _uow.Context.Set<HU_SALARY_TYPE>().AsNoTracking().AsQueryable();
                var otherlists = _uow.Context.Set<SYS_OTHER_LIST>().AsNoTracking().AsQueryable();
                var joined = await (from p in entity
                                     from c in otherlists.Where(x => x.ID == p.SALARY_TYPE_GROUP).DefaultIfEmpty()
                                     where p.ID == id
                                     select new
                                            {
                                                Id = p.ID,
                                                Code = p.CODE,
                                                Name = p.NAME,
                                                Note = p.NOTE,
                                                EffectDate = p.EFFECT_DATE,
                                                SalaryTypeGroup = p.SALARY_TYPE_GROUP,
                                                SalaryTypeGroupName = c.NAME,
                                                Description = p.DESCRIPTION,
                                                IsActive = p.IS_ACTIVE,
                                                UpdatedBy = p.UPDATED_BY,
                                                UpdatedDate = p.UPDATED_DATE,
                                                Status = (p.IS_ACTIVE.HasValue && p.IS_ACTIVE.Value) ? "Áp dụng" : "Ngừng áp dụng"
                                            }).FirstOrDefaultAsync();
                return Ok(new FormatedResponse(){InnerBody = joined });
            }
            catch (Exception ex)
            {
                return Ok(new FormatedResponse() { MessageCode = ex.Message, ErrorType = EnumErrorType.UNCATCHABLE, StatusCode = EnumStatusCode.StatusCode500 });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create(SalaryTypeDTO model)
        {
            try
            {
                model.IsActive = true;
                var sid = Request.Sid(_appSettings);
                var response = await _genericRepository.Create(_uow, model, sid ?? "");
                response.MessageCode = CommonMessageCode.CREATE_SUCCESS;
                return Ok(response);
            }
            catch (Exception ex)
            {
                return Ok(new FormatedResponse() { MessageCode = ex.Message, ErrorType = EnumErrorType.UNCATCHABLE, StatusCode = EnumStatusCode.StatusCode500 });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Update(SalaryTypeDTO model)
        {
            try
            {
                var response = await _genericRepository.Update(_uow, model, Request.Sid(_appSettings));
                return Ok(response);
            }
            catch (Exception ex)
            {
                return Ok(new FormatedResponse() { MessageCode = ex.Message, ErrorType = EnumErrorType.UNCATCHABLE, StatusCode = EnumStatusCode.StatusCode500 });
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteIds(IdsRequest model)
        {
            try
            {
                if (model.Ids != null)
                {
                    var entity = _uow.Context.Set<HU_SALARY_TYPE>().AsNoTracking().AsQueryable();
                    var validate = (from p in entity where model.Ids.Contains(p.ID) && p.IS_ACTIVE == true select p).Any();
                    if (validate)
                    {
                        return Ok(new FormatedResponse() { ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400, MessageCode = CommonMessageCode.DO_NOT_DELETE_DATA_IS_ACTIVE });
                    }
                    var response = await _genericRepository.DeleteIds(_uow, model.Ids);
                    return Ok(response);
                }
                else
                {
                    return Ok(new FormatedResponse() { ErrorType = EnumErrorType.CATCHABLE, MessageCode = CommonMessageCode.DELETE_REQUEST_NULL_ID });
                }
            }
            catch (Exception ex)
            {
                return Ok(new FormatedResponse() { ErrorType = EnumErrorType.UNCATCHABLE, StatusCode = EnumStatusCode.StatusCode500, MessageCode = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Delete(SalaryTypeDTO model)
        {
            try
            {
                if (model.Id != null)
                {
                    var response = await _genericRepository.Delete(_uow, (long)model.Id);
                    return Ok(response);
                }
                else
                {
                    return Ok(new FormatedResponse() { ErrorType = EnumErrorType.CATCHABLE, MessageCode = CommonMessageCode.DELETE_REQUEST_NULL_ID });
                }
            }
            catch (Exception ex)
            {
                return Ok(new FormatedResponse() { ErrorType = EnumErrorType.UNCATCHABLE, StatusCode = EnumStatusCode.StatusCode500, MessageCode = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetList()
        {
            try
            {
                var sysOtherLists = _uow.Context.Set<SYS_OTHER_LIST>().AsNoTracking().AsQueryable();
                var get_group_id_obj_salary = (from item in sysOtherLists
                                               where item.CODE == "NDTL01"
                                               select item.ID).First();


                var entity = _uow.Context.Set<HU_SALARY_TYPE>().AsNoTracking().AsQueryable();
                var joined = await (from p in entity
                                    where p.IS_ACTIVE == true && p.SALARY_TYPE_GROUP == get_group_id_obj_salary
                                    orderby p.ID descending
                                    select new SalaryTypeDTO
                                    {
                                        Id = p.ID,
                                        Code = p.CODE,
                                        Name = p.NAME,
                                    }).ToListAsync();

                return Ok(new FormatedResponse()
                {
                    InnerBody = joined
                });

            }
            catch (Exception ex)
            {
                return Ok(new FormatedResponse() { ErrorType = EnumErrorType.UNCATCHABLE, StatusCode = EnumStatusCode.StatusCode500, MessageCode = ex.Message });
            }
        }


        [HttpPost]
        public async Task<IActionResult> ToggleActiveIds(GenericToggleIsActiveDTO model)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _genericRepository.ToggleActiveIds(_uow, model.Ids, model.ValueToBind, sid);
            return Ok(new FormatedResponse()
            {
                MessageCode = response.MessageCode,
                InnerBody = response.InnerBody,
                StatusCode = EnumStatusCode.StatusCode200
            });
        }
    }
}
