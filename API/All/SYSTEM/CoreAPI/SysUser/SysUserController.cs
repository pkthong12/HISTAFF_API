using API.All.DbContexts;
using API.All.Services;
using API.All.SYSTEM.Common;
using API.All.SYSTEM.CoreAPI.SysUser;
using API.DTO;
using API.Main;
using CORE.DTO;
using CORE.Enum;
using CORE.GenericUOW;
using CORE.Services.File;
using CORE.StaticConstant;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Net.Mail;
using System.Net;
using System.Text;

namespace API.Controllers.SysUser
{
    [ApiExplorerSettings(GroupName = "144-SYSTEM-SYS_USER")]
    [ApiController]
    [HiStaffAuthorize]
    [Route("api/[controller]/[action]")]
    public class SysUserController : ControllerBase
    {
        private readonly GenericUnitOfWork _uow;
        private readonly ISysUserRepository _SysUserRepository;
        private readonly FullDbContext _dbContext;
        private readonly AppSettings _appSettings;
        private readonly GenericRepository<SYS_USER, SysUserDTO> _genericRepository;

        public SysUserController(
            FullDbContext dbContext,
            IWebHostEnvironment env,
            IOptions<AppSettings> options,
            IFileService fileService,
            IEmailService emailService
            )
        {
            _uow = new GenericUnitOfWork(dbContext);
            _SysUserRepository = new SysUserRepository(dbContext, _uow, env, options, fileService, emailService);
            _appSettings = options.Value;
            _dbContext = dbContext;
            _genericRepository = new(dbContext);
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(SysUserChangePasswordRequest request)
        {
            var response = await _SysUserRepository.ChangePassword(request);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> ReadAll()
        {
            var response = await _genericRepository.ReadAll();
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> ReadAllByNumberKey(ReadAllByKeyLongRequest request)
        {
            var response = await _SysUserRepository.ReadAllByKey(request.Key, request.Value);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> ReadAllByStringKey(ReadAllByKeyStringRequest request)
        {
            var response = await _SysUserRepository.ReadAllByKey(request.Key, request.Value);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> QueryList(GenericQueryListStringIdDTO<SysUserDTO> request)
        {
            var response = await _SysUserRepository.SinglePhaseQueryList(request);
            return Ok(new FormatedResponse() { InnerBody = response, MessageCode = CommonMessageCode.QUERY_LIST_SUCCESS });
        }

        [HttpGet]
        public async Task<IActionResult> GetById(long id)
        {
            var response = await _SysUserRepository.GetById(id);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetByStringId(string id)
        {
            var response = await _SysUserRepository.GetById(id);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Create(SysUserDTO model)
        {
            await Task.Run(() => null);
            throw new NotImplementedException();
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser(SysUserCreateUpdateRequest request)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _SysUserRepository.CreateUser(request, sid);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Update(SysUserDTO model)
        {
            await Task.Run(() => null);
            throw new NotImplementedException();

        }

        [HttpPost]
        public async Task<IActionResult> UpdateUser(SysUserCreateUpdateRequest request)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _SysUserRepository.UpdateUser(request, sid);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> SynchronousAccount()
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _SysUserRepository.SynchronousAccount(sid);
            return Ok(response);
        }
        [HttpPost]
        public async Task<IActionResult> ResetAccount(SysUserDTO userIds)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _SysUserRepository.ResetAccount(userIds.UserIds!);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(SysUserDTO model)
        {
            if (model.Id != null)
            {
                var response = await _SysUserRepository.Delete(_uow, model.Id);
                return Ok(response);
            }
            else
            {
                return Ok(new FormatedResponse() { ErrorType = EnumErrorType.CATCHABLE, MessageCode = CommonMessageCode.DELETE_REQUEST_NULL_ID });
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteIds(IdsRequest model)
        {
            var response = await _SysUserRepository.DeleteIds(_uow, model.Ids);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteStringIds(StringIdsRequest model)
        {
            var response = await _SysUserRepository.DeleteIds(_uow, model.Ids);
            return Ok(response);
        }

        /* This for logged User hisself/herself */
        [HttpGet]
        public async Task<IActionResult> QueryOrgPermissionList()
        {
            var user = _dbContext.SysUsers.Single(x => x.ID == Request.Sid(_appSettings));

            if (user == null) return Unauthorized();

            var response = await _SysUserRepository.QueryOrgPermissionList(user);

            return Ok(response);    

        }

        /* This for logged User hisself/herself */
        [HttpGet]
        public async Task<IActionResult> QueryOrgWithPositions()
        {
            var user = _dbContext.SysUsers.Single(x => x.ID == Request.Sid(_appSettings));

            if (user == null) return Unauthorized();

            var response = await _SysUserRepository.QueryOrgWithPositions(user);

            return Ok(response);

        }

        /* This for userId param */
        [HttpGet]
        public async Task<IActionResult> QueryUserOrgPermissionList(string objectId, bool useGroupIfEmpty = true)
        {
            var user = _dbContext.SysUsers.Single(x => x.ID == objectId);

            if (user == null) return Ok(new FormatedResponse()
            {
                MessageCode = CommonMessageCode.USER_DOES_NOT_EXIST,
                StatusCode = EnumStatusCode.StatusCode400,
                ErrorType = EnumErrorType.CATCHABLE
            });

            var response = await _SysUserRepository.QueryUserOrgPermissionList(user, useGroupIfEmpty);

            return Ok(response);

        }

        /* This for userId param */
        [HttpGet]
        public async Task<IActionResult> QueryFunctionActionPermissionList(string objectId, bool useGroupIfEmpty = true)
        {
            var user = _dbContext.SysUsers.Single(x => x.ID == objectId);

            if (user == null) return Ok(new FormatedResponse()
            {
                MessageCode = CommonMessageCode.USER_DOES_NOT_EXIST,
                StatusCode = EnumStatusCode.StatusCode400,
                ErrorType = EnumErrorType.CATCHABLE
            });

            var response = await _SysUserRepository.QueryFunctionActionPermissionList(user, useGroupIfEmpty);

            return Ok(response);

        }

        [HttpPost]
        public async Task<IActionResult> LockAccount(SysUserDTO request)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            if (request.UserIds == null) return BadRequest();
            var response = await _SysUserRepository.LockAccount(request.UserIds, sid);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> UnlockAccount(SysUserDTO request)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            if (request.UserIds == null) return BadRequest();
            var response = await _SysUserRepository.UnlockAccount(request.UserIds, sid);
            return Ok(response);
        }


        [HttpPost]
        public async Task<IActionResult> ChangePasswordPortal(SysUserChangePasswordRequest request)
        {
            var response = await _SysUserRepository.ChangePasswordPortal(request);
            return Ok(response);
        }


        [HttpGet]
        public async Task<IActionResult> GetByStringIdPortal(string id, long time)
        {
            var response = await _SysUserRepository.GetById(id);
            return Ok(response);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> SubmitUsernameWhenForgotPassword(ResetPasswordRequest request)
        {
            var response = await _SysUserRepository.SubmitUsernameWhenForgotPassword(request);
            return Ok(response);
        }


        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> SubmitVerificationCode(ResetPasswordRequest request)
        {
            var response = await _SysUserRepository.SubmitVerificationCode(request);
            return Ok(response);
        }


        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> ChangePasswordWhenForgotPassword(ResetPasswordRequest request)
        {
            var response = await _SysUserRepository.ChangePasswordWhenForgotPassword(request);
            return Ok(response);
        }
    }
}