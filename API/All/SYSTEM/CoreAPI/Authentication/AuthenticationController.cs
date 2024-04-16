using CoreDAL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Security.Claims;
using CoreAPI.ViewModels;
using CoreDAL.Repositories;
using Common.Extensions;
using ProfileDAL.Repositories;
using CoreDAL.Utilities;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using API;
using Microsoft.Extensions.Options;
using CORE.DTO;
using CORE.Enum;
using API.All.DbContexts;
using API.DTO;
using CORE.GenericUOW;
using SysUserDTO = CoreDAL.Models.SysUserDTO;
using CORE.Services.File;
using API.All.SYSTEM.CoreAPI.Authorization;
using CORE.StaticConstant;
using API.All.Services;
using API.All.SYSTEM.Common;
using System.Xml;
using CORE.Extension;

namespace CoreAPI
{
    [ApiExplorerSettings(GroupName = "001-SYSTEM-AUTHEN")]
    [ApiController]
    [HiStaffAuthorize]
    [Route("api/[controller]/[action]")]
    public class AuthenticationController : BaseController
    {
        private readonly ILogger<AuthenticationController> _logger;
        private readonly GenericUnitOfWork _uow;
        private readonly FullDbContext _dbContext;
        private IGenericRepository<HU_ORGANIZATION, HuOrganizationDTO> _genericRepository;

        private readonly IConfiguration _config;
        private readonly UserManager<SysUser> _userManager;
        private readonly SignInManager<SysUser> _signInManager;
        private readonly IProfileUnitOfWork _ProfileBusiness;
        private readonly IRefreshTokenService _refreshTokenService;
        private readonly API.Controllers.SysUser.SysUserRepository _sysUserRepository;

        private readonly AppSettings _appSettings;

        public AuthenticationController(
            ILogger<AuthenticationController> logger,
            IConfiguration configuration,
            UserManager<SysUser> userManager,
            SignInManager<SysUser> signInManager,
            IUnitOfWork unitOfWork,
            IProfileUnitOfWork ProfileBusiness,
            IRefreshTokenService refreshTokenService,
            IOptions<AppSettings> options,
            FullDbContext dbContext,
            IWebHostEnvironment env,
            IFileService fileService,
            IEmailService emailService
        ) : base(unitOfWork)
        {
            _logger = logger;
            _userManager = userManager;
            _signInManager = signInManager;
            _config = configuration;
            _ProfileBusiness = ProfileBusiness;
            _refreshTokenService = refreshTokenService;
            _appSettings = options.Value;
            _uow = new GenericUnitOfWork(dbContext);
            _dbContext = dbContext;
            _genericRepository = _uow.GenericRepository<HU_ORGANIZATION, HuOrganizationDTO>();
            _appSettings = options.Value;
            _sysUserRepository = new(dbContext, _uow, env, options, fileService, emailService);
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> ClientsLogin([FromBody] LoginTenantViewModel Credentials)
        {
            try
            {
                if (Credentials == null) return Unauthorized();
                string ipAddress = IpAddress();
                var r = await _unitOfWork.SysUsers.ClientsLogin(Credentials.Username, Credentials.Password, ipAddress);
                if (r.Error != null)
                {
                    if (r.Error == "ERROR_USERNAME_INCORRECT")
                    {
                        return Ok(new FormatedResponse() { StatusCode = EnumStatusCode.StatusCode400, ErrorType = EnumErrorType.CATCHABLE, MessageCode = CommonMessageCode.ERROR_USERNAME_INCORRECT });
                    }
                    else
                    {
                        return Ok(new FormatedResponse() { StatusCode = EnumStatusCode.StatusCode400, ErrorType = EnumErrorType.CATCHABLE, MessageCode = r.Error });
                    }
                }

                AuthResponse? user = r.Data as AuthResponse;

                if (user?.IsLock == true) // dù đang là Admin mà bị Lock thì vẫn Lock như thường (ví dụ cần khóa khẩn)
                {
                    return Ok(new FormatedResponse() { StatusCode = EnumStatusCode.StatusCode400, ErrorType = EnumErrorType.CATCHABLE, MessageCode = CommonMessageCode.USER_LOCKED });
                }

                if (user != null)
                {
                    var iat = user.EmployeeId != null  ? user.EmployeeId.ToString() : "0";
                    var isAdmin = (user.IsAdmin == true).ToString();
                    var claims = new[]
                    {
                        new Claim("appType", Credentials.AppType ?? "UNKNOWN"),
                        new Claim(JwtRegisteredClaimNames.Sid, user.Id),
                        new Claim(JwtRegisteredClaimNames.Typ, user.UserName),
                        new Claim(JwtRegisteredClaimNames.Iat, iat!),
                        new Claim("IsAdmin", isAdmin!)
                    };

                    if (user.IsWebapp != true && user.IsRoot != true && user.IsAdmin != true)
                    {
                        if (Credentials.AppType == "WEBAPP")
                        {
                            return Ok(new FormatedResponse() { StatusCode = EnumStatusCode.StatusCode400, ErrorType = EnumErrorType.CATCHABLE, MessageCode = CommonMessageCode.WEBAPP_IS_NOT_ALLOWED });
                        }
                    }

                    if (user.IsPortal != true && user.IsRoot != true && user.IsAdmin != true)
                    {
                        if (Credentials.AppType == "PORTAL")
                        {
                            return Ok(new FormatedResponse() { StatusCode = EnumStatusCode.StatusCode400, ErrorType = EnumErrorType.CATCHABLE, MessageCode = CommonMessageCode.PORTAL_IS_NOT_ALLOWED});
                        }
                    }

                    if (user.IsMobile != true && user.IsRoot != true && user.IsAdmin != true)
                    {
                        if (Credentials.AppType == "MOBILE")
                        {
                            return Ok(new FormatedResponse() { StatusCode = EnumStatusCode.StatusCode400, ErrorType = EnumErrorType.CATCHABLE, MessageCode = CommonMessageCodes.MOBILE_IS_NOT_ALLOWED });
                        }
                    }

                    string tokenString = BuildToken(claims, 1);
                    AuthResponse data = new()
                    {
                        Id = user.Id,
                        EmployeeId = user.EmployeeId,
                        UserName = user.UserName,
                        FullName = user.FullName,
                        Avatar = user.Avatar,
                        IsAdmin = user.IsAdmin,
                        IsRoot = user.IsRoot,
                        Token = tokenString,
                        IsFirstLogin = user.IsFirstLogin,
                        IsLock = user.IsLock,
                        IsWebapp = user.IsWebapp,
                        IsPortal = user.IsPortal,
                        IsMobile = user.IsMobile,
                        RefreshToken = user.RefreshToken
                    };

                    /*
                    dynamic res = await _ProfileBusiness.UserOrganiRepository.GetOrgPermission(user.Id, 0, (bool)user.IsAdmin);
                    */
                    SYS_USER simpleApproachUser = _dbContext.Set<SYS_USER>().AsNoTracking().AsQueryable().Single(x => x.ID == user.Id);
                    var orgPermissionRes = await _sysUserRepository.QueryOrgPermissionList(simpleApproachUser);
                    data.OrgIds = orgPermissionRes.InnerBody ?? new List<HuOrganizationDTO>();

                    var actionPermissionRes = await _sysUserRepository.QueryFunctionActionPermissionList(simpleApproachUser);
                    if (actionPermissionRes != null)
                    {
                        if (actionPermissionRes.InnerBody != null)
                        {
                            data.PermissionActions = (List<FunctionActionPermissionDTO>)actionPermissionRes.InnerBody ?? new List<FunctionActionPermissionDTO>();
                        }
                        else
                        {
                            data.PermissionActions = new List<FunctionActionPermissionDTO>();
                        }
                    }
                    else
                    {
                        data.PermissionActions = new List<FunctionActionPermissionDTO>();
                    }

                    SetTokenCookie(user.RefreshToken.TOKEN);
                    return Ok(new FormatedResponse()
                    {
                        MessageCode = CommonMessageCode.LOG_IN_SUCCESS,
                        InnerBody = data
                    });
                }
                else
                {
                    return new JsonResult("WAR_UNABLE_TO_SIGN_IN") { StatusCode = 401 };
                }
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }

        public class RefreshTokenObject
        {
            public string? token { get; set; }
            public string? AppType { get; set; }
        }

        /*
        * When access token expired, browser client use resfresh token stored in Http Only Cookie
        * to request the server for a new access token
        * No body/payload needed
        * 
        * For mobile app we need to add a payload that contains refresh token string
        * 
        * Note that any relevant cookie has priority over payloads
        */
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> RefreshToken(RefreshTokenObject? refreshTokenObject)
        {
            try
            {
                string refreshTokenString = "";
                var refreshTokenCookie = Request.Cookies["HiStaffRefreshToken"];
                if (refreshTokenCookie == null)
                {
                    if (refreshTokenObject != null)
                    {
                        if (refreshTokenObject.token == null)
                        {
                            return Ok(new FormatedResponse()
                            {
                                MessageCode = CommonMessageCodes.NO_REFRESHTOKEN_INFORMATION_FOUND_EITHER_IN_COOKIE_AND_PAYLOAD,
                                StatusCode = EnumStatusCode.StatusCode400,
                                ErrorType = EnumErrorType.CATCHABLE
                            });
                        }
                        else
                        {
                            if (refreshTokenObject.token == string.Empty)
                            {
                                return Ok(new FormatedResponse()
                                {
                                    MessageCode = CommonMessageCodes.PAYLOAD_BASED_REFRESHTOKEN_PROVIDED_WAS_EMPTY,
                                    StatusCode = EnumStatusCode.StatusCode400,
                                    ErrorType = EnumErrorType.CATCHABLE
                                });
                            }
                            else
                            {
                                refreshTokenString = refreshTokenObject.token;
                            }
                        }
                    }
                }
                else
                {
                    refreshTokenString = refreshTokenCookie;
                }

                if (refreshTokenString == "")
                {
                    return Ok(new FormatedResponse()
                    {
                        MessageCode = CommonMessageCodes.NO_REFRESHTOKEN_INFORMATION_FOUND_EITHER_IN_COOKIE_AND_PAYLOAD,
                        StatusCode = EnumStatusCode.StatusCode400,
                        ErrorType = EnumErrorType.CATCHABLE
                    });
                }
                string ipAddress = IpAddress();
                var previousUser = await _refreshTokenService.GetUserByRefreshToken(refreshTokenString);

                if (previousUser == null)
                {
                    return Ok(new FormatedResponse()
                    {
                        MessageCode = CommonMessageCodes.NO_USER_MATCHS_PROVIDED_REFRESHTOKEN,
                        StatusCode = EnumStatusCode.StatusCode400,
                        ErrorType = EnumErrorType.CATCHABLE
                    });
                }

                var tokenCheck = await _refreshTokenService.CheckRefreshToken(refreshTokenString, ipAddress);

                if (!tokenCheck.Success)
                {
                    return Ok(new FormatedResponse()
                    {
                        MessageCode = tokenCheck.Message,
                        StatusCode = EnumStatusCode.StatusCode400,
                        ErrorType = EnumErrorType.CATCHABLE
                    });
                }

                var user = tokenCheck.User;

                if (user == null)
                {
                    return Ok(new FormatedResponse()
                    {
                        MessageCode = CommonMessageCodes.NO_USER_MATCHS_PROVIDED_REFRESHTOKEN,
                        StatusCode = EnumStatusCode.StatusCode400,
                        ErrorType = EnumErrorType.CATCHABLE
                    });
                }

                if (user?.IS_LOCK == true) // dù đang là Admin mà bị Lock thì vẫn Lock như thường (ví dụ cần khóa khẩn)
                {
                    return Ok(new FormatedResponse() { StatusCode = EnumStatusCode.StatusCode400, ErrorType = EnumErrorType.CATCHABLE, MessageCode = CommonMessageCode.USER_LOCKED });
                }

                if (user?.IS_WEBAPP != true && user?.IS_ROOT != true && user?.IS_ADMIN != true)
                {
                    if (refreshTokenObject?.AppType == "WEBAPP")
                    {
                        return Ok(new FormatedResponse() { StatusCode = EnumStatusCode.StatusCode400, ErrorType = EnumErrorType.CATCHABLE, MessageCode = CommonMessageCode.WEBAPP_IS_NOT_ALLOWED });
                    }
                }

                if (user?.IS_PORTAL != true && user?.IS_ROOT != true && user?.IS_ADMIN != true)
                {
                    if (refreshTokenObject?.AppType == "PORTAL")
                    {
                        return Ok(new FormatedResponse() { StatusCode = EnumStatusCode.StatusCode400, ErrorType = EnumErrorType.CATCHABLE, MessageCode = CommonMessageCode.PORTAL_IS_NOT_ALLOWED});
                    }
                }

                if (user?.IS_MOBILE != true && user?.IS_ROOT != true && user?.IS_ADMIN != true)
                {
                    if (refreshTokenObject?.AppType == "MOBILE")
                    {
                        return Ok(new FormatedResponse() { StatusCode = EnumStatusCode.StatusCode400, ErrorType = EnumErrorType.CATCHABLE, MessageCode = CommonMessageCodes.MOBILE_IS_NOT_ALLOWED });
                    }
                }

                var iat = user?.EMPLOYEE_ID != null ? user?.EMPLOYEE_ID.ToString() : "0";
                var claims = new[]
                {
                    new Claim("appType", refreshTokenObject?.AppType ?? "UNKNOWN"),
                    new Claim(JwtRegisteredClaimNames.Sid, user?.ID!),
                    new Claim(JwtRegisteredClaimNames.Typ, user?.USERNAME!),
                    new Claim(JwtRegisteredClaimNames.Iat, iat!),
                    new Claim("IsAdmin", (user?.IS_ADMIN == true).ToString())
                };


                string tokenString = BuildToken(claims, 1);
                AuthResponse data = new()
                {
                    Id = user!.ID,
                    UserName = user.USERNAME ?? "",
                    FullName = user.FULLNAME ?? "",
                    Avatar = user.AVATAR ?? "",
                    IsAdmin = user.IS_ADMIN,
                    IsRoot = user.IS_ROOT,
                    Token = tokenString,
                    IsFirstLogin = user.IS_FIRST_LOGIN,
                    IsLock = user.IS_LOCK,
                    IsWebapp = user.IS_WEBAPP,
                    IsPortal = user.IS_PORTAL,
                    IsMobile = user.IS_MOBILE,
                    EmployeeId = user.EMPLOYEE_ID,
                    RefreshToken = tokenCheck.NewRefreshToken!
                };

                //var res = await _ProfileBusiness.UserOrganiRepository.GetOrgPermission(user.ID, 0, user.IS_ADMIN);
                SYS_USER simpleApproachUser = _dbContext.Set<SYS_USER>().AsNoTracking().AsQueryable().Single(x => x.ID == user.ID);
                var orgPermissionRes = await _sysUserRepository.QueryOrgPermissionList(simpleApproachUser);
                data.OrgIds = orgPermissionRes.InnerBody ?? new List<HuOrganizationDTO>();

                var actionPermissionRes = await _sysUserRepository.QueryFunctionActionPermissionList(simpleApproachUser);
                if (actionPermissionRes != null)
                {
                    if (actionPermissionRes.InnerBody != null)
                    {
                        data.PermissionActions = (List<FunctionActionPermissionDTO>)actionPermissionRes.InnerBody ?? [];
                    }
                    else
                    {
                        data.PermissionActions = [];
                    }
                }
                else
                {
                    data.PermissionActions = new List<FunctionActionPermissionDTO>();
                }

                SetTokenCookie(tokenCheck.NewRefreshToken!.TOKEN);

                return Ok(new FormatedResponse() { InnerBody = data });
            }
            catch (Exception ex)
            {
                return Ok(new FormatedResponse() { StatusCode = EnumStatusCode.StatusCode500, ErrorType = EnumErrorType.UNCATCHABLE, MessageCode = ex.Message });
            }
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> SignInSys([FromBody] LoginModel Credentials)
        {
            try
            {
                if (Credentials == null) return Unauthorized();
                string tokenString = string.Empty;
                var user = await _userManager.FindByNameAsync(Credentials.Username);

                if (user != null)
                {
                    if (!user.LockoutEnabled)
                    {
                        return new JsonResult("WAR_USER_LOCKED") { StatusCode = 400 };
                    }
                    var result = await _signInManager.CheckPasswordSignInAsync(user, Credentials.Password, false);
                    if (result.Succeeded)
                    {
                        var r = await _unitOfWork.SysUsers.GetPermissonByUser(user.Id);

                        if (r.Error != null)
                        {
                            return ResponseResult(r);
                        }
                        LoginParam? userData = r.Data as LoginParam;
                        var claims = new[]
                        {
                            new Claim(JwtRegisteredClaimNames.Iat, user?.UserName!),
                            new Claim(JwtRegisteredClaimNames.Sid, user?.Id!),
                            new Claim(JwtRegisteredClaimNames.Sub, userData?.IsAdmin.ToString()!)
                        };
                        tokenString = BuildToken(claims, 1);
                        userData.Token = tokenString;
                        return ResponseResult(new ResultWithError(userData));
                    }
                    else
                    {
                        return new JsonResult("WAR_PASSWORD_ISCORECT") { StatusCode = 400 };
                    }
                }
                else
                {
                    return new JsonResult("WAR_UNABLE_TO_SIGN_IN") { StatusCode = 400 };
                }
            }
            catch (Exception ex)
            {

                return new JsonResult(ex.Message) { StatusCode = 401 };
            }
        }


        [HttpGet]
        public async Task<ActionResult> GetAll(SysUserDTO param)
        {
            var r = await _unitOfWork.SysUsers.GetAll(param);
            return Ok(r);
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] SysUserInputDTO Credentials)
        {
            // VALID REQUIRE
            if (Credentials.UserName == null || Credentials.UserName.Trim().Length == 0)
            {
                return ResponseResult("USERNAME_NOT_BLANK");
            }
            if (Credentials.Password == null || Credentials.Password.Trim().Length == 0)
            {
                return ResponseResult("PASSWORD_NOT_BLANK");
            }
            if (Credentials.GroupId == 0)
            {
                return ResponseResult("GROUP_NOT_BLANK");
            }
            var r = await _unitOfWork.SysUsers.CreateUserAsync(Credentials);
            return new JsonResult(r);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateUser([FromBody] SysUserInputDTO Credentials)
        {
            // VALID REQUIRE
            if (Credentials.Id == null)
            {
                return ResponseResult("USER_NOT_EXIST");
            }
            if (Credentials.UserName == null || Credentials.UserName.Trim().Length == 0)
            {
                return ResponseResult("USERNAME_NOT_BLANK");
            }
            if (Credentials.Password == null || Credentials.Password.Trim().Length == 0)
            {
                return ResponseResult("PASSWORD_NOT_BLANK");
            }
            if (Credentials.GroupId == 0)
            {
                return ResponseResult("GROUP_NOT_BLANK");
            }

            var r = await _unitOfWork.SysUsers.UpdateUserAsync(Credentials);
            return ResponseResult(r);
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordParam param)
        {
            if (param.UserName == null || param.UserName.Trim().Length == 0)
            {
                return ResponseResult("USERNAME_NOT_BLANK");
            }
            if (param.CurrentPassword == null || param.CurrentPassword.Trim().Length == 0)
            {
                return ResponseResult("CURRENT_PASS_NOT_BLANK");
            }
            if (param.NewPassword == null || param.NewPassword.Trim().Length == 0)
            {
                return ResponseResult("NEW_PASS_NOT_BLANK");
            }
            var r = await _unitOfWork.SysUsers.ChangePasswordAsync(param.UserName, param.CurrentPassword, param.NewPassword);
            return ResponseResult(r);
        }

        [HttpPost]
        public async Task<IActionResult> SetLockoutEnabledAsync([FromBody] SetEnableParam param)
        {
            if (param.UserName == null || param.UserName.Trim().Length == 0)
            {
                return ResponseResult("USERNAME_NOT_BLANK");
            }
            var r = await _unitOfWork.SysUsers.SetLockoutEnabledAsync(param.UserName, param.Enable);
            return ResponseResult(r);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Logout()
        {
            await Task.Run(() => DeleteTokenCookie());
            return Ok(new FormatedResponse() { InnerBody = "Successfully logged out" });
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> SignInTenantHR([FromBody] LoginTenantViewModel Credentials)
        {
            try
            {
                if (Credentials == null) return Unauthorized();

                //var r = await _unitOfWork.Tenants.TenantLogin(Credentials.Username, Credentials.Password);
                //if (r.Error != null)
                //{
                //    return ResponseResult(r);
                //}

                //LoginParam user = r.Data as LoginParam;
                //if (user != null)
                //{
                //    var claims = new[]
                //    {
                //    new Claim(JwtRegisteredClaimNames.Sid, user.Id),
                //    new Claim(JwtRegisteredClaimNames.Typ, user.UserName),
                //    new Claim("IsAdmin", user.IsAdmin.ToString())
                //};

                //    string tokenString = BuildToken(claims, 1);
                //    LoginOutput data = new LoginOutput();
                //    data.Id = user.Id;
                //    data.UserName = user.UserName;
                //    data.FullName = user.FullName;
                //    data.Avatar = user.Avatar;
                //    data.IsAdmin = user.IsAdmin;
                //    data.Token = tokenString;
                //    data.PermissionParams = user.PermissionParams;

                //    dynamic res = await _ProfileBusiness.UserOrganiRepository.GetOrgPermission(user.Id, (int)user.TenantId, (bool)user.IsAdmin);
                //    data.OrgIds = res.Data;

                //    return ResponseResult(new ResultWithError(data));
                //}
                //else
                //{
                //    return new JsonResult("WAR_UNABLE_TO_SIGN_IN") { StatusCode = 401 };

                //}
                return new JsonResult("WAR_UNABLE_TO_SIGN_IN") { StatusCode = 401 };
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }



        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> SignInPortalHR([FromBody] LoginTenantViewModel Credentials)
        {
            if (Credentials == null) return Unauthorized();
            //var r = await _unitOfWork.Tenants.SignInPortalHR(Credentials.Username, Credentials.Password, SysERP.HR, Credentials.FcmToken, Credentials.DeviceId);
            //if (r.Error != null)
            //{
            //    return ResponseResult(r);
            //}

            //LoginParam user = r.Data as LoginParam;
            //if (user != null)
            //{
            //    string[] username = user.UserName.Split("_");
            //    var claims = new[]
            //    {
            //        new Claim(JwtRegisteredClaimNames.Sid, user.Id),
            //        new Claim(JwtRegisteredClaimNames.Typ, user.UserName),
            //        new Claim(JwtRegisteredClaimNames.Iat, user.EmpId.ToString()),
            //        new Claim(JwtRegisteredClaimNames.Azp, user.TenantId.ToString())
            //    };

            //    string tokenString = BuildToken(claims, 3);
            //    LoginOutput data = new LoginOutput();
            //    data.Id = user.Id;
            //    data.UserName = username[1];
            //    data.FullName = user.FullName;
            //    data.Avatar = user.Avatar;
            //    data.IsAdmin = user.IsAdmin;
            //    data.Token = tokenString;
            //    data.PermissionParams = user.PermissionParams;
            //    return ResponseResult(new ResultWithError(data));

            //}
            //else
            //{
            //    return new JsonResult("WAR_UNABLE_TO_SIGN_IN") { StatusCode = 401 };
            //}
            return new JsonResult("WAR_UNABLE_TO_SIGN_IN") { StatusCode = 401 };
        }

        private string BuildToken(Claim[] claims, int type)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_appSettings.JwtToken.SecretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            JwtSecurityToken token;
            switch (type)
            {
                case 1:
                    token = new JwtSecurityToken(
                        _appSettings.JwtToken.Issuer,
                        _appSettings.JwtToken.Audience,
                        claims: claims,
                        expires: DateTime.Now.AddDays(_appSettings.JwtToken.WebDaysOfLife),
                        signingCredentials: creds
                    );
                    break;
                case 2:
                    token = new JwtSecurityToken(
                        _appSettings.JwtToken.Issuer,
                        _appSettings.JwtToken.Audience,
                        claims: claims,
                        expires: DateTime.Now.AddDays(_appSettings.JwtToken.WebMinutesOfLife),
                        signingCredentials: creds
                    );
                    break;
                case 3:
                    token = new JwtSecurityToken(
                        _appSettings.JwtToken.Issuer,
                        _appSettings.JwtToken.Audience,
                        claims: claims,
                        expires: DateTime.Now.AddDays(_appSettings.JwtToken.MobileDaysOfLife),
                        signingCredentials: creds
                    );
                    break;
                default:
                    token = new JwtSecurityToken(
                        _appSettings.JwtToken.Issuer,
                        _appSettings.JwtToken.Audience,
                        claims: claims,
                        expires: DateTime.Now.AddDays(_appSettings.JwtToken.WebMinutesOfLife),
                        signingCredentials: creds
                    );
                    break;
            }
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        [HttpGet]
        public async Task<ActionResult> GetList()
        {
            var r = await _unitOfWork.SysUsers.GetList();

            return ResponseResult(r);
        }

        private CookieOptions cookieOptions(int expiresIn)
        {
            return new CookieOptions
            {
                Secure = true,
                HttpOnly = true,
                SameSite = SameSiteMode.None,
                Expires = DateTime.UtcNow.AddDays(expiresIn)
            };
        }
        private void SetTokenCookie(string token)
        {
            Response.Cookies.Append("HiStaffRefreshToken", token, cookieOptions(_appSettings.JwtToken.RefreshTokenDaysOfLife));
        }

        private void DeleteTokenCookie()
        {
            Response.Cookies.Delete("HiStaffRefreshToken", cookieOptions(-1));
        }

        private string IpAddress()
        {
            if (Request.Headers.TryGetValue("X-Forwarded-For", out Microsoft.Extensions.Primitives.StringValues value))
                return value!;
            else
                return HttpContext.Connection.RemoteIpAddress!.MapToIPv4().ToString();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> HandleADFSResponse([FromForm] Dictionary<string, string> adfsData)
        {

            static XmlNamespaceManager GetNamespaceManager(XmlDocument xmlDoc)
            {
                XmlNamespaceManager nsmgr = new(xmlDoc.NameTable);
                nsmgr.AddNamespace("saml", "urn:oasis:names:tc:SAML:1.0:assertion");
                // Add other namespaces as needed

                return nsmgr;
            }

            if (adfsData != null && adfsData.TryGetValue("wresult", out string? value))
            {
                var wresult = await Task.Run(() => value);

                XmlDocument xmlDoc = new();
                xmlDoc.LoadXml(wresult);

                string? issuer = xmlDoc.SelectSingleNode("//saml:Assertion/@Issuer", GetNamespaceManager(xmlDoc))?.InnerText;
                string? subject = xmlDoc.SelectSingleNode("//saml:Assertion/saml:AttributeStatement/saml:Subject/saml:SubjectConfirmation/saml:ConfirmationMethod", GetNamespaceManager(xmlDoc))?.InnerText;
                string? nameAttribute = xmlDoc.SelectSingleNode("//saml:Assertion/saml:AttributeStatement/saml:Attribute[@AttributeName='name']", GetNamespaceManager(xmlDoc))?.InnerText;
                string? givenNameAttribute = xmlDoc.SelectSingleNode("//saml:Assertion/saml:AttributeStatement/saml:Attribute[@AttributeName='givenname']", GetNamespaceManager(xmlDoc))?.InnerText;

                if (nameAttribute == null)
                {
                    return Redirect($"{_appSettings.Saml2AdfsSetting.SPUrl}?local=1");
                }
                else
                {

                    //find the user
                    var user = _dbContext.SysUsers.FirstOrDefault(x => x.EMAIL!.ToLower() == nameAttribute.ToLower());

                    if (user != null)
                    {

                        var tokenHandler = new JwtSecurityTokenHandler();
                        var key = Encoding.ASCII.GetBytes(_appSettings.JwtToken.SecretKey);
                        var javascriptExp = DateTime.Now.GetJavascriptTimeStamp() + 30 * 1000; // 30 seconds
                        var tokenDescriptor = new SecurityTokenDescriptor
                        {
                            Subject = new ClaimsIdentity(new[] {
                        new Claim("exp_at", javascriptExp.ToString()),
                     }),
                            //Expires = expires,
                            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                        };
                        var token = tokenHandler.CreateToken(tokenDescriptor);
                        string tokenString = tokenHandler.WriteToken(token);

                        user.SHORT_LIVED_TOKEN = tokenString;
                        _dbContext.SysUsers.Update(user);
                        _dbContext.SaveChanges();

                        return Redirect($"{_appSettings.Saml2AdfsSetting.SPUrl}?token={tokenString}");
                    }
                    else
                    {
                        return Redirect($"{_appSettings.Saml2AdfsSetting.SPUrl}?local=1");
                    }

                }
            }
            else
            {
                return Redirect($"{_appSettings.Saml2AdfsSetting.SPUrl}?local=1");
            }
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> HandleADFSResponsePortal([FromForm] Dictionary<string, string> adfsData)
        {

            var replyUrl = _appSettings.Saml2AdfsSetting?.SPUrlPortal;

            if (replyUrl == null)
            {
                return BadRequest();
            }

            static XmlNamespaceManager GetNamespaceManager(XmlDocument xmlDoc)
            {
                XmlNamespaceManager nsmgr = new(xmlDoc.NameTable);
                nsmgr.AddNamespace("saml", "urn:oasis:names:tc:SAML:1.0:assertion");
                // Add other namespaces as needed

                return nsmgr;
            }

            if (adfsData != null && adfsData.TryGetValue("wresult", out string? value))
            {
                var wresult = await Task.Run(() => value);

                XmlDocument xmlDoc = new();
                xmlDoc.LoadXml(wresult);

                string? issuer = xmlDoc.SelectSingleNode("//saml:Assertion/@Issuer", GetNamespaceManager(xmlDoc))?.InnerText;
                string? subject = xmlDoc.SelectSingleNode("//saml:Assertion/saml:AttributeStatement/saml:Subject/saml:SubjectConfirmation/saml:ConfirmationMethod", GetNamespaceManager(xmlDoc))?.InnerText;
                string? nameAttribute = xmlDoc.SelectSingleNode("//saml:Assertion/saml:AttributeStatement/saml:Attribute[@AttributeName='name']", GetNamespaceManager(xmlDoc))?.InnerText;
                string? givenNameAttribute = xmlDoc.SelectSingleNode("//saml:Assertion/saml:AttributeStatement/saml:Attribute[@AttributeName='givenname']", GetNamespaceManager(xmlDoc))?.InnerText;

                if (nameAttribute == null)
                {
                    return Redirect($"{replyUrl}?local=1");
                }
                else
                {

                    //find the user
                    var user = _dbContext.SysUsers.FirstOrDefault(x => x.EMAIL!.ToLower() == nameAttribute.ToLower());

                    if (user != null)
                    {

                        var tokenHandler = new JwtSecurityTokenHandler();
                        var key = Encoding.ASCII.GetBytes(_appSettings.JwtToken.SecretKey);
                        var javascriptExp = DateTime.Now.GetJavascriptTimeStamp() + 30 * 1000; // 30 seconds
                        var tokenDescriptor = new SecurityTokenDescriptor
                        {
                            Subject = new ClaimsIdentity(new[] {
                                new Claim("exp_at", javascriptExp.ToString()),
                             }),
                            //Expires = expires,
                            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                        };
                        var token = tokenHandler.CreateToken(tokenDescriptor);
                        string tokenString = tokenHandler.WriteToken(token);

                        user.SHORT_LIVED_TOKEN = tokenString;
                        _dbContext.SysUsers.Update(user);
                        _dbContext.SaveChanges();

                        return Redirect($"{replyUrl}?token={tokenString}");
                    }
                    else
                    {
                        return Redirect($"{replyUrl}?local=1");
                    }

                }
            }
            else
            {
                return Redirect($"{replyUrl}?local=1");
            }
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ShortLivedTokenLogin(string token)
        {
            var r = _dbContext.SysUsers.FirstOrDefault(x => x.SHORT_LIVED_TOKEN == token);
            if (r != null)
            {
                // validate token
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_appSettings.JwtToken.SecretKey);
                try
                {
                    tokenHandler.ValidateToken(token, new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
                        ClockSkew = TimeSpan.Zero
                    }, out SecurityToken validatedToken);

                    var jwtToken = (JwtSecurityToken)validatedToken;
                    var expired = jwtToken.Claims.FirstOrDefault(x => x.Type == "exp_at")?.Value;

                    if (expired != null)
                    {
                        var timeExpires = long.Parse(expired);
                        var now = DateTime.UtcNow.GetJavascriptTimeStamp();
                        if (timeExpires < now)
                        {
                            _logger.LogInformation($"expired={expired}, timeExpires={timeExpires}");

                            return Ok(new FormatedResponse()
                            {
                                MessageCode = "THE_GIVEN_TOKEN_EXPIRED",
                                StatusCode = EnumStatusCode.StatusCode404,
                                ErrorType = EnumErrorType.CATCHABLE,
                                InnerBody = new
                                {
                                    JwtToken = jwtToken,
                                    Expired = expired,
                                    TimeExpires = timeExpires,
                                    Now = now
                                }
                            });
                        }
                        else
                        {

                            var data = new AuthResponse()
                            {
                                Id = r.ID,
                                UserName = r.USERNAME!,
                                FullName = r.FULLNAME!,
                                IsAdmin = r.IS_ADMIN,
                                IsRoot = r.IS_ROOT,
                                IsLock = r.IS_LOCK,
                                IsWebapp = r.IS_WEBAPP,
                                IsPortal = r.IS_PORTAL,
                                IsMobile = r.IS_MOBILE,
                                Avatar = r.AVATAR!,
                                IsFirstLogin = r.IS_FIRST_LOGIN,
                                EmployeeId = r.EMPLOYEE_ID,
                            };

                            var refreshToken = await _refreshTokenService.UpdateRefreshTokens(r.ID, IpAddress());
                            data.RefreshToken = refreshToken;

                            AuthResponse user = data;

                            if (user?.IsLock == true) // dù đang là Admin mà bị Lock thì vẫn Lock như thường (ví dụ cần khóa khẩn)
                            {
                                return Ok(new FormatedResponse() { StatusCode = EnumStatusCode.StatusCode400, ErrorType = EnumErrorType.CATCHABLE, MessageCode = CommonMessageCode.USER_LOCKED });
                            }

                            if (user != null)
                            {
                                var claims = new[]
                                {
                                        new Claim(JwtRegisteredClaimNames.Sid, user.Id),
                                        new Claim(JwtRegisteredClaimNames.Typ, user.UserName),
                                        new Claim(JwtRegisteredClaimNames.Iat, user.EmployeeId.ToString()),
                                        new Claim("IsAdmin", user.IsAdmin.ToString())
                                    };

                                /*

                                if (user.IsWebapp != true && user.IsRoot != true && user.IsAdmin != true)
                                {
                                    if (Credentials.AppType == "WEBAPP")
                                    {
                                        return Ok(new FormatedResponse() { StatusCode = EnumStatusCode.StatusCode400, ErrorType = EnumErrorType.CATCHABLE, MessageCode = CommonMessageCode.WEBAPP_IS_NOT_ALLOWED });
                                    }
                                }

                                if (user.IsPortal != true && user.IsRoot != true && user.IsAdmin != true)
                                {
                                    if (Credentials.AppType == "PORTAL")
                                    {
                                        return Ok(new FormatedResponse() { StatusCode = EnumStatusCode.StatusCode400, ErrorType = EnumErrorType.CATCHABLE, MessageCode = CommonMessageCode.PORTAL_IS_NOT_ALLOWED });
                                    }
                                }

                                if (user.IsMobile != true && user.IsRoot != true && user.IsAdmin != true)
                                {
                                    if (Credentials.AppType == "MOBILE")
                                    {
                                        return Ok(new FormatedResponse() { StatusCode = EnumStatusCode.StatusCode400, ErrorType = EnumErrorType.CATCHABLE, MessageCode = CommonMessageCode.MOBILE_IS_NOT_ALLOWED });
                                    }
                                }
                                */

                                string tokenString = BuildToken(claims, 1);
                                AuthResponse localData = new()
                                {
                                    Id = user.Id,
                                    EmployeeId = user.EmployeeId,
                                    UserName = user.UserName,
                                    FullName = user.FullName,
                                    Avatar = user.Avatar,
                                    IsAdmin = user.IsAdmin,
                                    IsRoot = user.IsRoot,
                                    Token = tokenString,
                                    IsFirstLogin = user.IsFirstLogin,
                                    IsLock = user.IsLock,
                                    IsWebapp = user.IsWebapp,
                                    IsPortal = user.IsPortal,
                                    RefreshToken = user.RefreshToken
                                };

                                SYS_USER simpleApproachUser = _dbContext.Set<SYS_USER>().AsNoTracking().AsQueryable().Single(x => x.ID == user.Id);
                                var orgPermissionRes = await _sysUserRepository.QueryOrgPermissionList(simpleApproachUser);
                                localData.OrgIds = orgPermissionRes.InnerBody ?? new List<HuOrganizationDTO>();

                                var actionPermissionRes = await _sysUserRepository.QueryFunctionActionPermissionList(simpleApproachUser);
                                if (actionPermissionRes != null)
                                {
                                    if (actionPermissionRes.InnerBody != null)
                                    {
                                        localData.PermissionActions = (List<FunctionActionPermissionDTO>)actionPermissionRes.InnerBody ?? new List<FunctionActionPermissionDTO>();
                                    }
                                    else
                                    {
                                        localData.PermissionActions = new List<FunctionActionPermissionDTO>();
                                    }
                                }
                                else
                                {
                                    localData.PermissionActions = new List<FunctionActionPermissionDTO>();
                                }

                                SetTokenCookie(user.RefreshToken.TOKEN);

                                return Ok(new FormatedResponse()
                                {
                                    MessageCode = CommonMessageCode.LOG_IN_SUCCESS,
                                    InnerBody = localData
                                });

                            }
                            else
                            {
                                return Ok(new FormatedResponse()
                                {
                                    MessageCode = "WAR_UNABLE_TO_SIGN_IN",
                                    StatusCode = EnumStatusCode.StatusCode404,
                                    ErrorType = EnumErrorType.CATCHABLE
                                });

                            }
                        }
                    }
                    else
                    {
                        return Ok(new FormatedResponse()
                        {
                            MessageCode = "EXP_AT_CLAIM_TYPE_NOT_FOUND_IN_THE_GIVEN_TOKEN", // exp_at
                            StatusCode = EnumStatusCode.StatusCode404,
                            ErrorType = EnumErrorType.CATCHABLE
                        });
                    }
                }
                catch (Exception ex)
                {
                    return Ok(new FormatedResponse()
                    {
                        MessageCode = ex.Message,
                        StatusCode = EnumStatusCode.StatusCode500,
                        ErrorType = EnumErrorType.UNCATCHABLE
                    });
                }
            }
            else
            {
                return Ok(new FormatedResponse()
                {
                    MessageCode = "NO_USER_FOUND_WITH_GIVEN_TOKEN",
                    StatusCode = EnumStatusCode.StatusCode404,
                    ErrorType = EnumErrorType.CATCHABLE
                });
            }
        }

    }
    public class LoginModel
    {
        public required string Username { get; set; }
        public required string Password { get; set; }
    }

    public class LoginTenantViewModel
    {

        public required string Username { get; set; }
        public required string Password { get; set; }
        public string? FcmToken { get; set; }
        public string? DeviceId { get; set; }
        public string? AppType { get; set; }
    }
}
