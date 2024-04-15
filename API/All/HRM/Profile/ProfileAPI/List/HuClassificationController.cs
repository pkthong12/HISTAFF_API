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
    public class HuClassificationController : ControllerBase
    {
        private readonly GenericUnitOfWork _uow;
        private readonly ProfileDbContext _profileDbContext;
        private IGenericRepository<HU_CLASSIFICATION, HuClassificationDTO> _genericRepository;
        private readonly GenericReducer<HU_CLASSIFICATION, HuClassificationDTO> genericReducer;
        private AppSettings _appSettings;
        public HuClassificationController(IOptions<AppSettings> options, ProfileDbContext profileDbContext)
        {
            _profileDbContext = profileDbContext;
            _uow = new GenericUnitOfWork(_profileDbContext);
            _genericRepository = _uow.GenericRepository<HU_CLASSIFICATION, HuClassificationDTO>();
            genericReducer = new();
            _appSettings = options.Value;
        }
        
        [HttpPost]
        public async Task<IActionResult> QueryList(GenericQueryListStringIdDTO<HuClassificationDTO> request)
        {
            try
            {
                var entity = _uow.Context.Set<HU_CLASSIFICATION>().AsNoTracking().AsQueryable();
                var otherlists = _uow.Context.Set<SYS_OTHER_LIST>().AsNoTracking().AsQueryable();
                var joined = from p in entity
                             from c in otherlists.Where(x => x.ID == p.CLASSIFICATION_LEVEL).DefaultIfEmpty()
                             from a in otherlists.Where(x => x.ID == p.CLASSIFICATION_TYPE).DefaultIfEmpty()
                             orderby p.CREATED_DATE descending
                             select new HuClassificationDTO
                             {
                                 Id = p.ID,
                                 Code = p.CODE,
                                 Note = p.NOTE,
                                 ClassificationLevel = p.CLASSIFICATION_LEVEL,
                                 ClassificationLevelName = c.NAME,
                                 ClassificationType = p.CLASSIFICATION_TYPE,
                                 ClassificationTypeName = a.NAME,
                                 PointFrom = p.POINT_FROM,
                                 PointTo = p.POINT_TO,
                                 IsActive = p.IS_ACTIVE,
                                 ModifiedBy = p.MODIFIED_BY,
                                 ModifiedDate = p.MODIFIED_DATE,
                                 CreatedDate = p.CREATED_DATE,
                                 StatusName = p.IS_ACTIVE == true ? "Áp dụng" : "Ngừng áp dụng"
                             };
                //request.Sort = new List<SortItem>();
                //request.Sort.Add(new SortItem() { Field = "CreatedDate", SortDirection = EnumSortDirection.DESC });
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
                var rs = "XL001";

                var entity = _uow.Context.Set<HU_CLASSIFICATION>().AsNoTracking().AsQueryable();
                var joined = (from p in entity
                              where (p.CODE != null || p.CODE != "") && p.CODE.Length == 5
                              select new HuClassificationDTO
                              {
                                  Id = p.ID,
                                  Code = p.CODE
                              }).ToList();
                var maxCode = (from p in joined where Regex.IsMatch(p.Code, @"(XL)(\d{3}$)") orderby p.Code.Substring(2, 3) descending select p.Code.Substring(2, 3)).FirstOrDefault();
                if (maxCode != null)
                {
                    rs = "XL" + (int.Parse(maxCode) + 1).ToString("000");
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
                var entity = _uow.Context.Set<HU_CLASSIFICATION>().AsNoTracking().AsQueryable();
                var otherlists = _uow.Context.Set<SYS_OTHER_LIST>().AsNoTracking().AsQueryable();
                var joined = await(from p in entity
                                   from c in otherlists.Where(x => x.ID == p.CLASSIFICATION_LEVEL).DefaultIfEmpty()
                                   from a in otherlists.Where(x => x.ID == p.CLASSIFICATION_TYPE).DefaultIfEmpty()
                                   where p.ID == id
                                   select new
                                   {
                                       Id = p.ID,
                                       Code = p.CODE,
                                       Note = p.NOTE,
                                       ClassificationLevel = p.CLASSIFICATION_LEVEL,
                                       ClassificationLevelName = c.NAME,
                                       ClassificationType = p.CLASSIFICATION_TYPE,
                                       ClassificationTypeName = a.NAME,
                                       PointFrom = p.POINT_FROM,
                                       PointTo = p.POINT_TO,
                                       IsActive = p.IS_ACTIVE,
                                       ModifiedBy = p.MODIFIED_BY,
                                       ModifiedDate = p.MODIFIED_DATE,
                                       CreatedDate = p.CREATED_DATE,
                                       StatusName = p.IS_ACTIVE == true ? "Áp dụng" : "Ngừng áp dụng"
                                   }).FirstOrDefaultAsync();
                return Ok(new FormatedResponse(){InnerBody = joined });
            }
            catch (Exception ex)
            {
                return Ok(new FormatedResponse() { MessageCode = ex.Message, ErrorType = EnumErrorType.UNCATCHABLE, StatusCode = EnumStatusCode.StatusCode500 });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create(HuClassificationDTO model)
        {
            try
            {
                var entity = _uow.Context.Set<HU_CLASSIFICATION>().AsNoTracking().AsQueryable();
                var obj = (from p in entity where p.IS_ACTIVE == true && p.CLASSIFICATION_TYPE == model.ClassificationType && p.CLASSIFICATION_LEVEL == model.ClassificationLevel && p.ID != model.Id select p).Any();

                if (obj) return Ok(new FormatedResponse() { ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400, MessageCode = "UI_FORM_CONTROL_ERROR_RECORD_IS_EXISTED_IN_DATABASE" });

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
        public async Task<IActionResult> Update(HuClassificationDTO model)
        {
            try
            {
                var entity = _uow.Context.Set<HU_CLASSIFICATION>().AsNoTracking().AsQueryable();
                var obj = (from p in entity where p.IS_ACTIVE == true && p.CLASSIFICATION_TYPE == model.ClassificationType && p.CLASSIFICATION_LEVEL == model.ClassificationLevel && p.ID != model.Id select p).Any();

                if (obj) return Ok(new FormatedResponse() { ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400, MessageCode = "UI_FORM_CONTROL_ERROR_RECORD_IS_EXISTED_IN_DATABASE" });
                if (model.PointFrom > model.PointTo)
                    return Ok(new FormatedResponse() { ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400, MessageCode = "UI_FORM_CONTROL_ERROR_POINT_TO_MUST_GREATER_THAN_OR_EQUAL_POINT_FROM" });
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
                    var entity = _uow.Context.Set<HU_CLASSIFICATION>().AsNoTracking().AsQueryable();
                    var validate = (from p in entity where model.Ids.Contains(p.ID) && p.IS_ACTIVE == true select p).Any();
                    //var getClassificationUsingEvaluate = (from c in entity.AsNoTracking().AsQueryable()
                    //                                      from e in _uow.Context.Set<HU_EVALUATE>().Where(x => x.CLASSIFICATION_ID == c.ID)
                    //                                      where model.Ids.Contains(c.ID)
                    //                                      select new { Id = c.ID }).ToList();
                    //var getClassificationUsingEvaluateCom = (from c in entity.AsNoTracking().AsQueryable()
                    //                                      from e in _uow.Context.Set<HU_EVALUATION_COM>().Where(x => x.CLASSIFICATION_ID == c.ID)
                    //                                      where model.Ids.Contains(c.ID)
                    //                                      select new { Id = c.ID }).ToList();

                    if (validate)
                    {
                        return Ok(new FormatedResponse() { ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400, MessageCode = CommonMessageCode.DO_NOT_DELETE_DATA_IS_ACTIVE });
                    }

                    //else if(getClassificationUsingEvaluate.Any() || getClassificationUsingEvaluateCom.Any())
                    //{
                    //    return Ok(new FormatedResponse() { StatusCode = EnumStatusCode.StatusCode400, ErrorType = EnumErrorType.CATCHABLE, MessageCode = CommonMessageCode.CAN_NOT_DELETE_RECORDS_HAVE_USE });
                    //}
                    else
                    {
                        var response = await _genericRepository.DeleteIds(_uow, model.Ids);
                        return Ok(response);
                    }
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
        public async Task<IActionResult> Delete(HuClassificationDTO model)
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
        public async Task<IActionResult> GetReprensentative()
        {
            var classification = _uow.Context.Set<HU_CLASSIFICATION>().AsNoTracking().AsQueryable();
            var otherlist = _uow.Context.Set<SYS_OTHER_LIST>().AsNoTracking().AsQueryable();
            var response = await (from c in classification
                                  from o in otherlist.Where(x => x.ID == c.CLASSIFICATION_TYPE)
                                  from o2 in otherlist.Where(x => x.ID == c.CLASSIFICATION_LEVEL)
                                  where o.CODE == "LXL02"
                                  select new
                                  {
                                      Id = c.ID,
                                      ClassificationLevelName = o2.NAME,
                                      Code = c.CODE,
                                      PointFrom = c.POINT_FROM, 
                                      PointTo = c.POINT_TO,
                                  }
                                  ).ToListAsync();
            return Ok(new FormatedResponse() { InnerBody = response});
        }

        [HttpGet]
        public async Task<IActionResult> GetSatffAssessment()
        {
            var classification = _uow.Context.Set<HU_CLASSIFICATION>().AsNoTracking().AsQueryable();
            var otherlist = _uow.Context.Set<SYS_OTHER_LIST>().AsNoTracking().AsQueryable();
            var response = await(from c in classification
                                 from o in otherlist.Where(x => x.ID == c.CLASSIFICATION_TYPE)
                                 from o2 in otherlist.Where(x => x.ID == c.CLASSIFICATION_LEVEL)
                                 where o.CODE == "LXL01"
                                 select new
                                 {
                                     Id = c.ID,
                                     ClassificationLevelName = o2.NAME,
                                     Code = c.CODE,
                                     PointFrom = c.POINT_FROM,
                                     PointTo = c.POINT_TO,
                                 }
                                  ).ToListAsync();
            return Ok(new FormatedResponse() { InnerBody = response });
        }
        [HttpPost]
        public async Task<IActionResult> ToggleActiveIds(GenericToggleIsActiveDTO model)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _genericRepository.ToggleActiveIds(_uow, model.Ids, model.ValueToBind, sid);
            return Ok(new FormatedResponse()
            {
                ErrorType =EnumErrorType.UNCATCHABLE, StatusCode = EnumStatusCode.StatusCode200, MessageCode = response.MessageCode, InnerBody = response.InnerBody
                //MessageCode = response.MessageCode,
                //InnerBody = response.InnerBody,
                //StatusCode = EnumStatusCode.StatusCode200
            });
        }

    }
}
