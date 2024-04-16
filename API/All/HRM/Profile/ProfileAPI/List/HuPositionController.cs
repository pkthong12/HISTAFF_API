using API;
using API.All.DbContexts;
using API.All.HRM.Profile.ProfileDAL.List.Position.Models;
using API.All.SYSTEM.CoreDAL.System.Language.Models;
using API.Entities;
using API.Main;
using Azure;
using Common.Extensions;
using CORE.DTO;
using CORE.Enum;
using CORE.GenericUOW;
using CORE.StaticConstant;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Options;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Information;
using ProfileDAL.Repositories;
using ProfileDAL.ViewModels;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace ProfileAPI.List
{
	[HiStaffAuthorize]
	[ApiExplorerSettings(GroupName = "002-PROFILE-POSITION")]
	[ApiController]
	[Route("api/[controller]/[action]")]

	public class HuPositionController : BaseController1
	{
		private IHttpContextAccessor _accessor;
		private readonly GenericUnitOfWork _uow;
		private readonly ProfileDbContext _profileDbContext;
		private IGenericRepository<HU_POSITION, PositionInputDTO> _genericRepository;
        private AppSettings _appSettings;
		public HuPositionController(IProfileUnitOfWork unitOfWork, IHttpContextAccessor accessor, ProfileDbContext coreDbContext, IOptions<AppSettings> options) : base(unitOfWork)
		{
			_accessor = accessor;
			_uow = new GenericUnitOfWork(coreDbContext);
			_genericRepository = _uow.GenericRepository<HU_POSITION, PositionInputDTO>();
            _appSettings = options.Value;
			_profileDbContext = coreDbContext;
		}
		[HttpPost]
		public async Task<IActionResult> QueryList(GenericQueryListDTO<PositionViewNoPagingDTO> request)
		{
			try
			{
				var response = await _unitOfWork.PositionRepository.SinglePhaseQueryList(request);

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
		[HttpPost]
		public async Task<IActionResult> Create(PositionInputDTO model)
		{
			try
			{
                var sid = Request.Sid(_appSettings);
                var validate = _profileDbContext.Positions.Where(x => x.CODE.ToLower().Equals(model.code.ToLower()) && x.ID != model.Id).Count();
                if (validate > 0)
                {
                    return Ok(new FormatedResponse() { MessageCode = "UI_FORM_CONTROL_ERROR_POSITION_CODE_DUPLICATED", ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode519 });
                }
                var r = await _unitOfWork.PositionRepository.CreateAsync(model);
                if (r.StatusCode == "200")
                {
                    if(model.isTDV == true)
                    {
                        var org = (from p in _profileDbContext.Organizations
                                         where p.ID == model.OrgId
                                         select p).SingleOrDefault();
                        org!.HEAD_POS_ID = (long)r.Data!.GetType().GetProperty("ID")!.GetValue(r.Data, null)!;
                        await _profileDbContext.SaveChangesAsync();
                    }
                    return Ok(new FormatedResponse() { InnerBody = r.Data, MessageCode = CommonMessageCode.CREATE_SUCCESS, StatusCode = EnumStatusCode.StatusCode200 });
                }
                else
                {
                    return Ok(new FormatedResponse() { ErrorType = EnumErrorType.CATCHABLE, MessageCode = r.Error, StatusCode = EnumStatusCode.StatusCode400 });
                }
            }
			catch (Exception ex)
			{
				return Ok(new FormatedResponse() { MessageCode = ex.Message, ErrorType = EnumErrorType.UNCATCHABLE, StatusCode = EnumStatusCode.StatusCode500 });
			}
		}
		[HttpGet]
		public async Task<ActionResult> Get(int Id)
		{
			var r = await _unitOfWork.PositionRepository.GetById(Id);
            return Ok(new FormatedResponse() { InnerBody = r.Data });
			//return ResponseResult(r);
		}
		[HttpPost]
		public async Task<ActionResult> Update([FromBody] PositionInputDTO param)
		{
            var oldData = await _profileDbContext.Positions.Where(p => p.ID == param.Id).FirstOrDefaultAsync();
            var listPosition = _profileDbContext.Positions.Where(p => p.ORG_ID == param.OrgId);
            if (oldData!.IS_TDV == true && param.isTDV == false)
            {
                var org = (from p in _profileDbContext.Organizations
                           where p.ID == param.OrgId
                           select p).SingleOrDefault();
                org!.HEAD_POS_ID = null;
            }
            foreach (var item in listPosition)
            {
                item.IS_TDV = false;
            }
            await _profileDbContext.SaveChangesAsync();
			var r = await _unitOfWork.PositionRepository.UpdateAsync(param);
            if (r.StatusCode == "200")
            {
                if (param.isTDV == true)
                {
                    var org = (from p in _profileDbContext.Organizations
                               where p.ID == param.OrgId
                               select p).SingleOrDefault();
                    org!.HEAD_POS_ID = (long)r.Data!.GetType().GetProperty("ID")!.GetValue(r.Data, null)!;
                    await _profileDbContext.SaveChangesAsync();
                }
                return Ok(new FormatedResponse() { InnerBody = r.Data, MessageCode = CommonMessageCode.UPDATE_SUCCESS, StatusCode = EnumStatusCode.StatusCode200 });
            }
            else
            {
                return Ok(new FormatedResponse() { ErrorType = EnumErrorType.CATCHABLE, MessageCode = r.Error, StatusCode = EnumStatusCode.StatusCode400 });
            }
        }
		[HttpPost]
		public async Task<IActionResult> DeleteIds(IdsRequest model)
		{
			try
			{
                bool isCheckDataUsing = false;
                bool isCheckNoneActive = false; 
				if (model.Ids != null)
				{
                    model.Ids.ForEach(item =>
                    {
                        var getDataActive = _uow.Context.Set<HU_POSITION>().Where(x => x.ID == item && x.IS_ACTIVE == true).FirstOrDefault();
                                                 
                        var getDataUsing = (from p in _uow.Context.Set<HU_POSITION>().Where(x => x.ID == item)
                                                from e in _uow.Context.Set<HU_EMPLOYEE>().Where(x => x.POSITION_ID == p.ID)
                                                select new { Id = p.ID }).ToList();
                        if(getDataActive != null)
                        {
                            isCheckNoneActive = true;
                            return;
                        }
                        if(getDataUsing.Count > 0)
                        {
                            isCheckDataUsing = true;
                            return;
                        }
                    });
                    if(isCheckDataUsing == true) 
                    {
                        return Ok(new FormatedResponse() { StatusCode = EnumStatusCode.StatusCode400, ErrorType = EnumErrorType.CATCHABLE, MessageCode = CommonMessageCode.CAN_NOT_DELETE_RECORDS_HAVE_USE });
                    }
                    else if (isCheckNoneActive == true)
                    {
                        return Ok(new FormatedResponse() { StatusCode = EnumStatusCode.StatusCode400, ErrorType = EnumErrorType.CATCHABLE, MessageCode = CommonMessageCode.CAN_NOT_DELETE_RECORDS_HAVE_IS_ACTIVE });
                    }
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

		[HttpGet]
		public async Task<IActionResult> GetScales()
		{
			try
			{
				var query = await (from p in _profileDbContext.GroupPositions.Where(x => x.IS_ACTIVE.HasValue && x.IS_ACTIVE.Value)
								   select new
								   {
									   Id = p.ID,
									   Name = p.NAME
								   }).ToListAsync();
				return Ok(new FormatedResponse()
				{
					InnerBody = query
				});
			}
			catch (Exception ex)
			{
				return Ok(new FormatedResponse() { ErrorType = EnumErrorType.UNCATCHABLE, StatusCode = EnumStatusCode.StatusCode500, MessageCode = ex.Message });
			}
		}
        [HttpGet]
        public async Task<IActionResult> GetPositionByOrgId(long orgId)
        {
            try
            {
                var masterNull = await _profileDbContext.Positions.Where(x => x.ORG_ID == orgId && x.MASTER == null).AsNoTracking().ToListAsync();
                var masterNotNull = await _profileDbContext.Positions.Where(x => x.ORG_ID == orgId && x.MASTER != null).AsNoTracking().ToListAsync();
                var workingIds = await _profileDbContext.OtherLists.Where(x => x.CODE == "DC" || x.CODE == "BN" || x.CODE == "DDBN" || x.CODE == "CT" || x.CODE == "BP" || x.CODE == "DDCV").Select(x => x.ID).ToListAsync();
                var statusWorking = await _profileDbContext.OtherLists.Where(x => x.CODE == "DD").SingleAsync();
                List<HU_POSITION> list = new List<HU_POSITION>();
                foreach (var item in masterNotNull)
                {
                    if(_profileDbContext.Workings.Where(x => x.EMPLOYEE_ID == item.MASTER && workingIds.Contains(x.TYPE_ID) && x.STATUS_ID == statusWorking.ID && x.EFFECT_DATE > DateTime.Now).Any() || _profileDbContext.Terminates.Where(x => x.EMPLOYEE_ID == item.MASTER && x.STATUS_ID == statusWorking.ID && x.EFFECT_DATE > DateTime.Now).Any())
                    {
                        list.Add(item);
                    }
                }

                var result = masterNull.Concat(list);

                return Ok(new FormatedResponse()
                {
                    InnerBody = result
                });
            }
            catch (Exception ex)
            {
                return Ok(new FormatedResponse() { ErrorType = EnumErrorType.UNCATCHABLE, StatusCode = EnumStatusCode.StatusCode500, MessageCode = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> AutoGenCodeHuPosition(long orgId, long jobId)
        {
            try
            {
                var code = "";
                var orgInfor = (from p in _profileDbContext.Organizations where p.ID == orgId select p).FirstOrDefault();
                var orgCode = orgInfor.CODE;
                if (orgInfor != null && orgInfor.COMPANY_ID != null)
                {
                    var companyCode = (from p in _profileDbContext.CompanyInfos where p.ID == orgInfor.COMPANY_ID select p.CODE).FirstOrDefault();
                    var jobCode = (from p in _profileDbContext.HUJobs where p.ID == jobId select p.CODE).FirstOrDefault();
                    code = orgCode + "_" + jobCode + "_" + companyCode;
                }
                else
                {
                    code = "";
                }
                return Ok(new FormatedResponse()
                {
                    InnerBody = code
                });
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
                var rs = "P001";

                var entity = _uow.Context.Set<HU_POSITION>().AsNoTracking().AsQueryable();
                var joined = (from p in entity
                              where (p.CODE != null || p.CODE != "") && p.CODE.Length == 4
                              select new HuPositionDTO
                              {
                                  Id = p.ID,
                                  Name = p.NAME,
                                  Code = p.CODE
                              }).ToList();
                var maxCode = (from p in joined where Regex.IsMatch(p.Code, @"(P)(\d{3}$)") orderby p.Code.Substring(1, 3) descending select p.Code.Substring(1, 3)).FirstOrDefault();
                if (maxCode != null)
                {
                    rs = "P" + (int.Parse(maxCode) + 1).ToString("000");
                }
                return Ok(new FormatedResponse() { InnerBody = new { Code = rs } });
            }
            catch (Exception ex)
            {
                return Ok(new FormatedResponse() { MessageCode = ex.Message, ErrorType = EnumErrorType.UNCATCHABLE, StatusCode = EnumStatusCode.StatusCode500 });
            }
        }

		[HttpPost]
		public async Task<ActionResult> SwapMasterInterim(PositionInputDTO param)
		{
			var r = await _unitOfWork.PositionRepository.SwapMasterInterim(param);
            if (r.StatusCode == "200")
            {
                return Ok(new FormatedResponse() { InnerBody = r.Data, MessageCode = CommonMessageCode.SWAP_SUCCESS, StatusCode = EnumStatusCode.StatusCode200 });
            }
            else
            {
                return Ok(new FormatedResponse() { ErrorType = EnumErrorType.CATCHABLE, MessageCode = r.Error, StatusCode = EnumStatusCode.StatusCode400 });
            }
        }
        [HttpPost]
        public async Task<ActionResult> ChangeStatus(PositionInputDTO request)
        {
            var r = await _unitOfWork.PositionRepository.ChangeStatusAsync(request);
            if (r.StatusCode == "200")
            {
                return Ok(new FormatedResponse() { InnerBody = r.Data, MessageCode = CommonMessageCode.TOGGLE_IS_ACTIVE_SUCCESS, StatusCode = EnumStatusCode.StatusCode200 });
            }
            else
            {
                return Ok(new FormatedResponse() { ErrorType = EnumErrorType.CATCHABLE, MessageCode = r.Error, StatusCode = EnumStatusCode.StatusCode400 });
            }
            //return Ok(new FormatedResponse() { MessageCode = request.ValueToBind == true ? CommonMessageCode.TOGGLE_IS_ACTIVE_SUCCESS : (request.ValueToBind == false ? CommonMessageCode.TOGGLE_IS_INACTIVE_SUCCESS : ""), StatusCode = EnumStatusCode.StatusCode200 });
        }

        [HttpPost]
        public async Task<ActionResult> CheckTdv(PositionInputDTO request)
        {
            var r = await _unitOfWork.PositionRepository.CheckTdvAsync(request);
            if (r.StatusCode == "200")
            {
                return Ok(new FormatedResponse() { ErrorType = EnumErrorType.CATCHABLE, InnerBody=r.InnerBody, StatusCode = EnumStatusCode.StatusCode200 });
            }
            else
            {
                return Ok(new FormatedResponse() { ErrorType = EnumErrorType.CATCHABLE, MessageCode = r.Error, StatusCode = EnumStatusCode.StatusCode204 });
            }
        }
        /* [HttpPost]
         public async Task<IActionResult> Create(PositionViewDTO model)
         {
             try
             {
                 HU_POSITION entity = new();
                 var response = await _genericRepository.Create(_uow, model, entity);
                 return Ok(response);
             }
             catch (Exception ex)
             {
                 return Ok(new FormatedResponse() { MessageCode = ex.Message, ErrorType = EnumErrorType.UNCATCHABLE, StatusCode = EnumStatusCode.StatusCode500 });
             }
         }*/
        /*[HttpGet]
        public async Task<ActionResult> GetAll(PositionViewDTO param)
        {
            var r = await _unitOfWork.PositionRepository.GetAll(param);
            return Ok(r);
        }
        [HttpGet]
        public async Task<ActionResult> Get(int Id)
        {
            var r = await _unitOfWork.PositionRepository.GetById(Id);
            return ResponseResult(r);
        }
        [HttpPost]
        public async Task<IActionResult> Create(SysLanguageDTO model)
        {
            try
            {
                HU_POSITION entity = new();
                var response = await _genericRepository.Create(_uow, model, entity);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return Ok(new FormatedResponse() { MessageCode = ex.Message, ErrorType = EnumErrorType.UNCATCHABLE, StatusCode = EnumStatusCode.StatusCode500 });
            }
        }
        [HttpPost]
        public async Task<ActionResult> Add([FromBody] PositionInputDTO param)
        {
            if (param == null)
            {
                return ResponseResult("PARAM_NOT_BLANK");
            }
            //if (param.GroupId == null)
            //{
            //    return ResponseResult("GROUP_NOT_BLANK");
            //}
            if (string.IsNullOrWhiteSpace(param.code))
            {
                return ResponseResult("CODE_NOT_BLANK");
            }
            // if (string.IsNullOrWhiteSpace(param.name))
            // {
            //     return ResponseResult("NAME_NOT_BLANK");
            // }

            var r = await _unitOfWork.PositionRepository.CreateAsync(param);
            return ResponseResult(r);
        }
        [HttpPost]
        public async Task<ActionResult> Update([FromBody] PositionInputDTO param)
        {
            if (param == null)
            {
                return ResponseResult("PARAM_NOT_BLANK");
            }
            if (param.id == null)
            {
                return ResponseResult("ID_NOT_BLANK");
            }
            //if (param.GroupId == null)
            //{
            //    return ResponseResult("GROUP_NOT_BLANK");
            //}
            if (string.IsNullOrWhiteSpace(param.code))
            {
                return ResponseResult("CODE_NOT_BLANK");
            }
            // if (string.IsNullOrWhiteSpace(param.name))
            // {
            //     return ResponseResult("NAME_NOT_BLANK");
            // }

            var r = await _unitOfWork.PositionRepository.UpdateAsync(param);
            return ResponseResult(r);
        }       
        [HttpGet]
        public async Task<ActionResult> GetList(int groupId)
        {
            var r = await _unitOfWork.PositionRepository.GetList(groupId);
            return ResponseResult(r);
        }
        [HttpGet]
        public async Task<ActionResult> GetListJob()
        {
            var r = await _unitOfWork.PositionRepository.GetListJob();
            return ResponseResult(r);
        }

        [HttpPost]
        public async Task<ActionResult> Delete([FromBody] List<long> param)
        {
            if (!ModelState.IsValid)
            {
                return ResponseValidation();
            }
            var r = await _unitOfWork.PositionRepository.Delete(param);
            return ResponseResult(r);
        }

        [HttpGet]
        [Route("positions/{org}/{emp}")]
        public async Task<ActionResult> GetPositionByOrgID(int org, int emp)
        {
            var r = await _unitOfWork.PositionRepository.GetByOrg(org, emp);
            return ResponseResult(r);
        }

        [HttpGet]
        [Route("direct-manager/{positionId}")]
        public async Task<ActionResult> GetDirectManager(int positionId)
        {
            var r = await _unitOfWork.PositionRepository.GetLM(positionId);
            return ResponseResult(r);
        }
        [HttpGet]
        public string AutoGenCodeHuTile(string tableName, string colName)
        {
            var r = _unitOfWork.PositionRepository.AutoGenCodeHuTile(tableName, colName);
            return r;
        }
        [HttpPost]
        public async Task<ActionResult> ModifyPositionById(PositionInputDTO param, int orgRight, int Address, int orgIDDefault = 1, int isDissolveDefault = 0)
        {
            if (param == null)
            {
                return ResponseResult("PARAM_NOT_BLANK");
            }

            var r = await _unitOfWork.PositionRepository.ModifyPositionById(param, orgRight, Address, orgIDDefault, isDissolveDefault);
            return ResponseResult(r);
        }
        [HttpPost]
        public async Task<ActionResult> InsertPositionNB(PositionInputDTO obj, int OrgRight, int Address, int OrgIDDefault = 1, int IsDissolveDefault = 0)
        {
            if (obj == null)
            {
                return ResponseResult("PARAM_NOT_BLANK");
            }

            var r = await _unitOfWork.PositionRepository.InsertPositionNB(obj, OrgRight, Address, OrgIDDefault, IsDissolveDefault);
            return ResponseResult(r);
        }
        [HttpGet]
        public async Task<ActionResult> GetOrgTreeApp(string sLang)
        {
            var r = await _unitOfWork.PositionRepository.GetOrgTreeApp(sLang);
            return ResponseResult(r);
        }
        [HttpGet]
        public async Task<ActionResult> GetPositionOrgID(PositionViewDTO _filter)
        {
            var r = await _unitOfWork.PositionRepository.GetPositionOrgID(_filter);
            return Ok(r);
        }*/
    }
}
