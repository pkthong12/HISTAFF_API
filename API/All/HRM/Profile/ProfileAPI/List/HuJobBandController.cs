using API;
using API.All.DbContexts;
using API.Entities;
using API.Main;
using Azure.Core;
using CORE.DTO;
using CORE.Enum;
using CORE.GenericUOW;
using CORE.StaticConstant;
using CoreDAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using ProfileDAL.Repositories;
using ProfileDAL.ViewModels;

namespace ProfileAPI.List
{
    [HiStaffAuthorize]
    [ApiExplorerSettings(GroupName = "002-PROFILE-JOB-BAND")]
    [ApiController]

    //[Route("api/hr/job-band")]
    [Route("api/[controller]/[action]")]
    public class HuJobBandController : BaseController1
    {
        private readonly GenericUnitOfWork _uow;
        private readonly ProfileDbContext _profileDbContext;
        private IGenericRepository<HU_JOB_BAND, HUJobBandInputDTO> _genericRepository;
        private readonly GenericReducer<HU_JOB_BAND, HUJobBandDTO> genericReducer;
        private AppSettings _appSettings;
        public HuJobBandController(IProfileUnitOfWork unitOfWork, IOptions<AppSettings> options, ProfileDbContext profileDbContext) : base(unitOfWork)
        {
            _profileDbContext = profileDbContext;
            _uow = new GenericUnitOfWork(profileDbContext);
            _genericRepository = _uow.GenericRepository<HU_JOB_BAND, HUJobBandInputDTO>();
            genericReducer = new();
            _appSettings = options.Value;
        }

        [HttpPost]
        public async Task<IActionResult> QueryList(GenericQueryListStringIdDTO<HUJobBandDTO> request)
        {
            try
            {
                var joined = from p in _profileDbContext.HUJobBands
                             select new HUJobBandDTO
                             {
                                 Id = p.ID,
                                 NameVN = p.NAME_VN,
                                 NameEN = p.NAME_EN,
                                 LevelFrom = p.LEVEL_FROM
                             };

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
        public async Task<IActionResult> GetById(long id)
        {
            try
            {
                var objRes = await (from p in _profileDbContext.HUJobBands
                                    where p.ID == id
                                    select new
                                    {
                                        Id = p.ID,
                                        NameVn = p.NAME_VN,
                                        NameEn = p.NAME_EN,
                                        LevelFrom = p.LEVEL_FROM
                                    }).FirstOrDefaultAsync();
                return Ok(new FormatedResponse() { InnerBody = objRes });
            }
            catch (Exception ex)
            {
                return Ok(new FormatedResponse() { MessageCode = ex.Message, ErrorType = EnumErrorType.UNCATCHABLE, StatusCode = EnumStatusCode.StatusCode500 });
            }
        }
        [HttpPost]
        public async Task<IActionResult> Create(HUJobBandInputDTO model)
        {
            try
            {
                model.Status = true;
                var sid = Request.Sid(_appSettings);
                var response = await _genericRepository.Create(_uow, model, sid ?? "");
                return Ok(response);
            }
            catch (Exception ex)
            {
                return Ok(new FormatedResponse() { MessageCode = ex.Message, ErrorType = EnumErrorType.UNCATCHABLE, StatusCode = EnumStatusCode.StatusCode500 });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Update(HUJobBandInputDTO model)
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
        public async Task<IActionResult> DeleteIds(IdsRequest req)
        {
            try
            {
                if (req.Ids != null)
                {
                    var response = await _genericRepository.DeleteIds(_uow, req.Ids);
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

    }
}
