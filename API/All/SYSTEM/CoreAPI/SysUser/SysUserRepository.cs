using CORE.GenericUOW;
using CORE.DTO;
using API.DTO;
using CORE.Enum;
using CORE.StaticConstant;
using API.All.DbContexts;
using CORE.Services.File;
using Microsoft.Extensions.Options;
using API.All.SYSTEM.CoreAPI.SysUser;
using API.All.HRM.Profile.ProfileAPI.HuOrganization;
using API.All.SYSTEM.CoreAPI.Authorization;
using System.Linq.Dynamic.Core;
using ProfileDAL.ViewModels;
using System.Text.RegularExpressions;
using System.Security.Cryptography;
using CoreDAL.Common;
using API.All.Services;
using DocumentFormat.OpenXml.Spreadsheet;
using CORE.AutoMapper;

namespace API.Controllers.SysUser
{
    public class SysUserRepository : ISysUserRepository
    {
        private readonly GenericUnitOfWork _uow;
        private readonly FullDbContext _dbContext;
        private IGenericRepository<SYS_USER, SysUserDTO> _genericRepository;
        private readonly GenericReducer<SYS_USER, SysUserDTO> _genericReducer;
        private readonly IWebHostEnvironment _env;
        private readonly AppSettings _appSettings;
        private readonly IFileService _fileService;
        private readonly IEmailService _emailService;

        public SysUserRepository(FullDbContext context, GenericUnitOfWork uow, IWebHostEnvironment env, IOptions<AppSettings> options, IFileService fileService, IEmailService emailService)
        {
            _dbContext = context;
            _uow = uow;
            _genericRepository = _uow.GenericRepository<SYS_USER, SysUserDTO>();
            _genericReducer = new();
            _env = env;
            _appSettings = options.Value;
            _fileService = fileService;
            _emailService = emailService;
        }

        public async Task<GenericPhaseTwoListResponse<SysUserDTO>> SinglePhaseQueryList(GenericQueryListStringIdDTO<SysUserDTO> request)
        {
            var entity = _dbContext.SysUsers.AsNoTracking().AsQueryable();
            var joined = from p in entity
                         from c in _dbContext.SysGroups.AsNoTracking().Where(c => c.ID == p.GROUP_ID).DefaultIfEmpty()
                         from e in _dbContext.HuEmployees.AsNoTracking().Where(c => c.ID == p.EMPLOYEE_ID).DefaultIfEmpty()
                         from cv in _dbContext.HuEmployeeCvs.AsNoTracking().Where(c => c.ID == e.PROFILE_ID).DefaultIfEmpty()
                         from lj in _dbContext.HuTerminates.AsNoTracking().Where(x => x.EMPLOYEE_ID == e.ID).DefaultIfEmpty()
                         from sys in _dbContext.SysOtherLists.AsNoTracking().Where(x => x.ID == lj.STATUS_ID && x.CODE == "DD").DefaultIfEmpty()
                         select new SysUserDTO
                         {
                             Id = p.ID,
                             Username = p.USERNAME,
                             Avatar = p.AVATAR ?? cv.AVATAR,
                             EmployeeId = e.ID,
                             EmployeeCode = e.CODE,
                             Email = p.EMAIL,
                             GroupName = c.NAME,
                             Fullname = p.FULLNAME,
                             IsLock = p.IS_LOCK,
                             GroupId = p.GROUP_ID,
                             IsPortal = p.IS_PORTAL,
                             IsWebapp = p.IS_WEBAPP,
                             LeaveJobDate = lj.EFFECT_DATE
                         };
            var singlePhaseResult = await _genericReducer.SinglePhaseReduce(joined, request);
            return singlePhaseResult;
        }

        public async Task<FormatedResponse> SynchronousAccount(string sid)
        {
            _uow.CreateTransaction();
            try
            {
                var joined = from p in _dbContext.HuEmployees.AsNoTracking().AsQueryable()
                             from c in _dbContext.SysUsers.AsNoTracking().Where(x => x.EMPLOYEE_ID == p.ID)
                             where c != null
                             select p;

                var employes = await _dbContext.HuEmployees.AsNoTracking().Where(x => !joined.Contains(x)).ToListAsync();
                for (int i = 0; i < employes.Count; i++)
                {
                    var empCv = _dbContext.HuEmployeeCvs.AsNoTracking().Where(x => x.ID == employes[i].PROFILE_ID).FirstOrDefault();
                    var groupEss = _dbContext.SysGroups.AsNoTracking().Where(x => x.CODE == "ESS_USER").FirstOrDefault();
                    if(empCv != null && employes[i].CODE != null && empCv.ID_NO != null && empCv.FULL_NAME != null && groupEss != null)
                    {
                        SysUserDTO entityCreateRequest = new()
                        {
                            Id = Guid.NewGuid().ToString(),
                            Username = employes[i].CODE,
                            GroupId = groupEss?.ID,
                            Passwordhash = BCrypt.Net.BCrypt.HashPassword(empCv.ID_NO),
                            IsPortal = true,
                            EmployeeCode = employes[i].CODE,
                            Fullname = empCv.FULL_NAME,
                            Avatar = empCv.AVATAR,
                            EmployeeId = employes[i].ID,
                            IsFirstLogin = true,
                            IsWebapp = false,
                            CreatedBy = sid
                        };
                        try
                        {
                            var entityCreateResposne = await _genericRepository.Create(_uow, entityCreateRequest, sid);
                            
                        } catch (Exception ex)
                        {
                            _uow.Rollback();
                            return new FormatedResponse() { MessageCode = ex.Message };
                        }
                    }
                    
                }
                _uow.Commit();
                return new FormatedResponse() { MessageCode = CommonMessageCode.SYNCHRONOUS_SUCCESS, StatusCode = EnumStatusCode.StatusCode200 };
            } catch(Exception ex)
            {
                return new FormatedResponse() { MessageCode = ex.Message, StatusCode = EnumStatusCode.StatusCode400 };
            }


        }

        public async Task<FormatedResponse> ResetAccount(List<string> userIds)
        {
            try
            {
                foreach (var item in userIds)
                {
                    var user = _dbContext.SysUsers.Where(x => x.ID == item).FirstOrDefault();
                    var e = _dbContext.HuEmployees.Where(x => x.ID == user!.EMPLOYEE_ID).FirstOrDefault();
                    if (e != null)
                    {
                        var cv = _dbContext.HuEmployeeCvs.Where(x => x.ID == e!.PROFILE_ID).FirstOrDefault();
                        if (cv != null && cv!.WORK_EMAIL == null)
                        {
                            return new FormatedResponse() { MessageCode = CommonMessageCode.EMPLOYEE_NOT_WORK_EMAIL, StatusCode = EnumStatusCode.StatusCode400 };
                        }
                    } else
                    {
                        return new FormatedResponse() { MessageCode = CommonMessageCode.USER_NOT_ASSIGN_EMPLOYEE, StatusCode = EnumStatusCode.StatusCode400 };
                    }

                }
                foreach (var userId in userIds)
                {
                    var user = await _dbContext.SysUsers.Where(x => x.ID == userId).FirstOrDefaultAsync();
                    var conditionReset = await _dbContext.SysConfigurationCommons.AsNoTracking().FirstOrDefaultAsync();

                    var regex = @"^";
                    if (user != null)
                    {
                        var employee = await _dbContext.HuEmployees.Where(x => x.ID == user.EMPLOYEE_ID).AsNoTracking().FirstOrDefaultAsync();
                        if (employee != null)
                        {
                            var cv = await _dbContext.HuEmployeeCvs.Where(x => x.ID == employee.PROFILE_ID).AsNoTracking().FirstOrDefaultAsync();
                            if (conditionReset != null)
                            {
                                if (conditionReset.IS_UPPERCASE == true)
                                {
                                    regex += "(?=.*[A-Z])";
                                }
                                if (conditionReset.IS_LOWERCASE == true)
                                {
                                    regex += "(?=.*[a-z])";
                                }
                                if (conditionReset.IS_NUMBER == true)
                                {
                                    regex += "(?=.*\\d)";
                                }
                                if (conditionReset.IS_SPECIAL_CHAR == true)
                                {
                                    regex += "(?=.*[!@#$%^&*()_+])";
                                }
                                regex = regex + ".*$";
                                string randomPassword = "";
                                if (conditionReset.MINIMUM_LENGTH != null)
                                {
                                    randomPassword = GenerateRandomPassword(regex, conditionReset.MINIMUM_LENGTH.Value);
                                    user!.PASSWORDHASH = BCrypt.Net.BCrypt.HashPassword(randomPassword);
                                    user!.IS_FIRST_LOGIN = true;
                                    _dbContext.UpdateRange();
                                    _dbContext.SaveChanges();
                                    if (cv != null)
                                    {
                                        if (cv.WORK_EMAIL != null)
                                        {
                                            var mail = await _dbContext.SeConfigs.FirstOrDefaultAsync();
                                            var dto = CoreMapper<SeConfigDTO, SE_CONFIG>.EntityToDto(mail!, new SeConfigDTO());
                                            await _emailService.SendEmailAfterResetPassword(cv.WORK_EMAIL, dto!, randomPassword);

                                        }
                                    }
                                }
                            }
                        }

                    }
                }
                
               
                return new FormatedResponse() { MessageCode = CommonMessageCode.RESET_EMAIL_SUCCESS, StatusCode = EnumStatusCode.StatusCode200 };
            }
            catch (Exception ex)
            {
                return new FormatedResponse() { MessageCode = ex.Message, StatusCode = EnumStatusCode.StatusCode500 };
            }
        }

        public static string GenerateRandomPassword(string regexPattern, int length)
        {
            
            if(length == 0)
            {
                length = 8;
            }
            char[] chars = Regex.Replace(regexPattern, @"[^a-zA-Z0-9!@#$%^&*()_+]", "").ToCharArray();

            RNGCryptoServiceProvider cryptoRandom = new RNGCryptoServiceProvider();

            // Mảng byte để lưu trữ số ngẫu nhiên
            byte[] randomBytes = new byte[length];

            cryptoRandom.GetBytes(randomBytes);

            char[] password = new char[length];
            for (int i = 0; i < length; i++)
            {
                password[i] = chars[randomBytes[i] % chars.Length];
            }

            return new string(password);
            
        }

        public async Task<FormatedResponse> ReadAll()
        {
            await Task.Run(() => null);
            throw new NotImplementedException();
        }

        public async Task<FormatedResponse> ReadAllByKey(string key, long value)
        {
            var resposne = await _genericRepository.ReadAllByKey(key, value);
            return new() { InnerBody = resposne };
        }

        public async Task<FormatedResponse> ReadAllByKey(string key, string value)
        {
            var resposne = await _genericRepository.ReadAllByKey(key, value);
            return new() { InnerBody = resposne };
        }

        public async Task<FormatedResponse> GetById(long id)
        {
            var res = await _genericRepository.GetById(id);
            if (res.InnerBody != null)
            {
                var response = res.InnerBody;
                var list = new List<SYS_USER>
                    {
                        (SYS_USER)response
                    };
                var joined = (from l in list
                                  // JOIN OTHER ENTITIES BASED ON THE BUSINESS
                              select new SysUserDTO
                              {
                                  Id = l.ID
                              }).FirstOrDefault();

                return new FormatedResponse() { InnerBody = joined };
            }
            else
            {
                return new FormatedResponse() { MessageCode = CommonMessageCode.ENTITY_NOT_FOUND, ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400 };
            }
        }

        public async Task<FormatedResponse> GetById(string id)
        {
            try
            {
                var response = await _genericRepository.GetById(id);
                if (response.InnerBody != null)
                {
                    var list = new List<SYS_USER>
                    {
                        (SYS_USER)response.InnerBody
                    };
                    var dtoView = (from l in list
                                   from e in _dbContext.HuEmployees.ToList().Where(x => x.ID == l.EMPLOYEE_ID).DefaultIfEmpty() // EMPLOYEE_ID may be null here
                                   from cv in _dbContext.HuEmployeeCvs.ToList().Where(x => x.ID == e?.PROFILE_ID).DefaultIfEmpty() // e may be null here
                                   from c in _dbContext.SysUsers.ToList().Where(x => x.ID == l.CREATED_BY).DefaultIfEmpty()
                                   from u in _dbContext.SysUsers.ToList().Where(x => x.ID == l.UPDATED_BY).DefaultIfEmpty()

                                   select new SysUserDTO()
                                   {
                                       Id = l.ID,
                                       Avatar = l.AVATAR,
                                       GroupId = l.GROUP_ID,
                                       Username = l.USERNAME,
                                       Fullname = l.FULLNAME,
                                       IsWebapp = l.IS_WEBAPP,
                                       IsPortal = l.IS_PORTAL,
                                       IsAdmin = l.IS_ADMIN,
                                       IsRoot = l.IS_ROOT,
                                       EmployeeId = l.EMPLOYEE_ID,
                                       EmployeeCode = e?.CODE, // e may be null  here
                                       EmployeeName = cv?.FULL_NAME, // cv may be null  here
                                       CreatedDate = l.CREATED_DATE,
                                       UpdatedDate = l.UPDATED_DATE,
                                       CreatedBy = l.CREATED_BY,
                                       UpdatedBy = l.UPDATED_BY,
                                       CreatedByUsername = c?.USERNAME, // c may be null  here
                                       UpdatedByUsername = u?.USERNAME, // u may be null  here
                                       IsFirstLogin = l.IS_FIRST_LOGIN
                                   }).SingleOrDefault();

                    return new() { InnerBody = dtoView };
                }
                else
                {
                    return new FormatedResponse() { MessageCode = CommonMessageCode.INNER_BODY_WAS_NULL, ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400 };
                }
            }
            catch (Exception ex)
            {
                return new FormatedResponse() { MessageCode = ex.Message, ErrorType = EnumErrorType.UNCATCHABLE, StatusCode = EnumStatusCode.StatusCode500 };
            }

        }

        public async Task<FormatedResponse> Create(GenericUnitOfWork _uow, SysUserDTO dto, string sid)
        {
            await Task.Run(() => null);
            throw new NotImplementedException();
        }

        public async Task<FormatedResponse> CreateUser(SysUserCreateUpdateRequest request, string sid)
        {
            try
            {
                var tryFind = _dbContext.SysUsers.SingleOrDefault(x => x.USERNAME!.ToLower() == request.UserName.ToLower());
                if (request.EmployeeId != null)
                {
                    var checkDataBeforeAdd = await _dbContext.SysUsers.Where(x => x.EMPLOYEE_ID == request.EmployeeId).ToListAsync();
                    if (checkDataBeforeAdd.Count > 0)
                    {
                        return new()
                        {
                            ErrorType = EnumErrorType.CATCHABLE,
                            MessageCode = CommonMessageCode.DUBLICATE_VALUE + "EMPLOYEE",
                            StatusCode = EnumStatusCode.StatusCode400
                        };
                    }
                }

                if (tryFind != null)
                {
                    return new()
                    {
                        ErrorType = EnumErrorType.CATCHABLE,
                        MessageCode = CommonMessageCode.DUBLICATE_VALUE + " USERNAME",
                        StatusCode = EnumStatusCode.StatusCode400
                    };
                }

                if (request.Password != request.PasswordConfirm)
                {
                    return new()
                    {
                        ErrorType = EnumErrorType.CATCHABLE,
                        MessageCode = CommonMessageCode.PASSWORD_AND_PASSWORD_CONFIRN_DO_NOT_MATCH,
                        StatusCode = EnumStatusCode.StatusCode400
                    };

                }

                if (request.AvatarFileData != null && request.AvatarFileData.Length > 0 && request.AvatarFileName != null && request.AvatarFileType != null)
                {
                    string location = Path.Combine(_env.ContentRootPath, _appSettings.StaticFolders.Root, _appSettings.StaticFolders.Avatars);

                    UploadRequest uploadRequest = new() { ClientFileData = request.AvatarFileData, ClientFileName = request.AvatarFileName, ClientFileType = request.AvatarFileType };

                    var uploadResponse = await _fileService.UploadFile(uploadRequest, location, sid);

                    if (uploadResponse != null)
                    {
                        string avatar = uploadResponse.SavedAs;

                        SysUserDTO entityCreateRequest = new()
                        {
                            Id = Guid.NewGuid().ToString(),
                            Username = request.UserName,
                            GroupId = request.GroupId,
                            Passwordhash = BCrypt.Net.BCrypt.HashPassword(request.Password),
                            IsWebapp = request.IsWebapp,
                            IsPortal = request.IsPortal,
                            IsAdmin = request.IsAdmin,
                            EmployeeId = request.EmployeeId,
                            EmployeeCode = request.EmployeeCode,
                            Fullname = request.Fullname,
                            Avatar = avatar
                        };
                        var entityCreateResposne = await _genericRepository.Create(_uow, entityCreateRequest, sid);

                        if (entityCreateResposne.StatusCode == EnumStatusCode.StatusCode200)
                        {
                            return entityCreateResposne;
                        }
                        else
                        {
                            return new()
                            {
                                ErrorType = EnumErrorType.CATCHABLE,
                                MessageCode = entityCreateResposne.MessageCode,
                                StatusCode = EnumStatusCode.StatusCode400
                            };
                        }
                    }
                    else
                    {
                        return new()
                        {
                            ErrorType = EnumErrorType.CATCHABLE,
                            MessageCode = CommonMessageCode.AVATAR_UPLOAD_FAILED,
                            StatusCode = EnumStatusCode.StatusCode400
                        };
                    }
                }
                else
                {
                    SysUserDTO entityCreateRequest = new()
                    {
                        Id = Guid.NewGuid().ToString(),
                        Username = request.UserName,
                        GroupId = request.GroupId,
                        Passwordhash = BCrypt.Net.BCrypt.HashPassword(request.Password),
                        IsWebapp = request.IsWebapp,
                        IsPortal = request.IsPortal,
                        IsAdmin = request.IsAdmin,
                        EmployeeCode = request.EmployeeCode,
                        Fullname = request.Fullname,
                        IsFirstLogin = true
                    };
                    var entityCreateResposne = await _genericRepository.Create(_uow, entityCreateRequest, sid);

                    if (entityCreateResposne.StatusCode == EnumStatusCode.StatusCode200)
                    {
                        return entityCreateResposne;
                    }
                    else
                    {
                        return new()
                        {
                            ErrorType = EnumErrorType.CATCHABLE,
                            MessageCode = entityCreateResposne.MessageCode,
                            StatusCode = EnumStatusCode.StatusCode400
                        };
                    }

                }
            }
            catch (Exception ex)
            {
                return new() { MessageCode = ex.Message, ErrorType = EnumErrorType.UNCATCHABLE, StatusCode = EnumStatusCode.StatusCode500 };
            }
        }

        public async Task<FormatedResponse> CreateRange(GenericUnitOfWork _uow, List<SysUserDTO> dtos, string sid)
        {
            await Task.Run(() => null);
            throw new NotImplementedException();
        }

        public async Task<FormatedResponse> Update(GenericUnitOfWork _uow, SysUserDTO dto, string sid, bool patchMode = true)
        {
            var response = await _genericRepository.Update(_uow, dto, sid, patchMode);
            return response;
        }

        public async Task<FormatedResponse> UpdateUser(SysUserCreateUpdateRequest request, string sid)
        {
            try
            {

                var tryFind = _dbContext.SysUsers.SingleOrDefault(x => x.USERNAME!.ToLower() == request.UserName.ToLower() && x.ID != request.Id);

                if (request.EmployeeId != null)
                {
                    var checkDataBeforeAdd = await _dbContext.SysUsers.Where(x => x.EMPLOYEE_ID == request.EmployeeId).ToListAsync();
                    if (checkDataBeforeAdd.Count > 1)
                    {
                        return new()
                        {
                            ErrorType = EnumErrorType.CATCHABLE,
                            MessageCode = CommonMessageCode.DUBLICATE_VALUE + " EMPLOYEE",
                            StatusCode = EnumStatusCode.StatusCode400
                        };
                    }
                }
                if (tryFind != null)
                {
                    return new()
                    {
                        ErrorType = EnumErrorType.CATCHABLE,
                        MessageCode = CommonMessageCode.DUBLICATE_VALUE + " USERNAME",
                        StatusCode = EnumStatusCode.StatusCode400
                    };
                }

                if (request.Id == null)
                {
                    return new()
                    {
                        ErrorType = EnumErrorType.CATCHABLE,
                        MessageCode = CommonMessageCode.NO_ID_PROVIED,
                        StatusCode = EnumStatusCode.StatusCode400
                    };
                }

                if (request.Password != request.PasswordConfirm)
                {
                    return new()
                    {
                        ErrorType = EnumErrorType.CATCHABLE,
                        MessageCode = CommonMessageCode.PASSWORD_AND_PASSWORD_CONFIRN_DO_NOT_MATCH,
                        StatusCode = EnumStatusCode.StatusCode400
                    };

                }

                if (request.AvatarFileData != null && request.AvatarFileData.Length > 0 && request.AvatarFileName != null && request.AvatarFileType != null)
                {
                    string location = Path.Combine(_env.ContentRootPath, _appSettings.StaticFolders.Root, _appSettings.StaticFolders.Avatars);

                    UploadRequest uploadRequest = new() { ClientFileData = request.AvatarFileData, ClientFileName = request.AvatarFileName, ClientFileType = request.AvatarFileType };

                    var uploadResponse = await _fileService.UploadFile(uploadRequest, location, sid);

                    if (uploadResponse != null)
                    {
                        string avatar = uploadResponse.SavedAs;

                        SysUserDTO entityUpdateRequest = new()
                        {
                            Id = request.Id,
                            Username = request.UserName,
                            GroupId = request.GroupId,
                            IsWebapp = request.IsWebapp,
                            IsPortal = request.IsPortal,
                            IsAdmin = request.IsAdmin,
                            EmployeeCode = request.EmployeeCode,
                            EmployeeName = request.EmployeeName,
                            Fullname = request.Fullname,
                            Avatar = avatar,
                            EmployeeId = request.EmployeeId
                        };
                        var entityCreateResposne = await _genericRepository.Update(_uow, entityUpdateRequest, sid);

                        if (entityCreateResposne.StatusCode == EnumStatusCode.StatusCode200)
                        {
                            return entityCreateResposne;
                        }
                        else
                        {
                            return new()
                            {
                                ErrorType = EnumErrorType.CATCHABLE,
                                MessageCode = entityCreateResposne.MessageCode,
                                StatusCode = EnumStatusCode.StatusCode400
                            };
                        }
                    }
                    else
                    {
                        return new()
                        {
                            ErrorType = EnumErrorType.CATCHABLE,
                            MessageCode = CommonMessageCode.AVATAR_UPLOAD_FAILED,
                            StatusCode = EnumStatusCode.StatusCode400
                        };
                    }
                }
                else
                {
                    SysUserDTO entityUpdateRequest = new()
                    {
                        Id = request.Id,
                        Username = request.UserName,
                        GroupId = request.GroupId,
                        IsWebapp = request.IsWebapp,
                        IsPortal = request.IsPortal,
                        IsAdmin = request.IsAdmin,
                        EmployeeId = request.EmployeeId,
                        EmployeeCode = request.EmployeeCode,
                        EmployeeName = request.EmployeeName,
                        Fullname = request.Fullname,
                        Avatar = request.Avatar,
                        SysMutationLogBeforeAfterRequest = request.SysMutationLogBeforeAfterRequest,
                        ActualFormDeclaredFields = request.ActualFormDeclaredFields

                    };
                    //TENANT_USER entity = new();
                    var entityCreateResposne = await _genericRepository.Update(_uow, entityUpdateRequest, sid);

                    if (entityCreateResposne.StatusCode == EnumStatusCode.StatusCode200)
                    {
                        return entityCreateResposne;
                    }
                    else
                    {
                        return new FormatedResponse()
                        {
                            ErrorType = EnumErrorType.CATCHABLE,
                            MessageCode = entityCreateResposne.MessageCode,
                            StatusCode = EnumStatusCode.StatusCode400
                        };
                    }

                }
            }
            catch (Exception ex)
            {
                return new() { MessageCode = ex.Message, ErrorType = EnumErrorType.UNCATCHABLE, StatusCode = EnumStatusCode.StatusCode500 };
            }
        }

        public async Task<FormatedResponse> UpdateRange(GenericUnitOfWork _uow, List<SysUserDTO> dtos, string sid, bool patchMode = true)
        {
            await Task.Run(() => null);
            throw new NotImplementedException();
        }


        public async Task<FormatedResponse> Delete(GenericUnitOfWork _uow, long id)
        {
            var response = await _genericRepository.Delete(_uow, id);
            return response;
        }

        public async Task<FormatedResponse> Delete(GenericUnitOfWork _uow, string id)
        {
            await Task.Run(() => null);
            throw new NotImplementedException();
        }

        public async Task<FormatedResponse> DeleteIds(GenericUnitOfWork _uow, List<long> ids)
        {
            var response = await _genericRepository.DeleteIds(_uow, ids);
            return response;
        }

        public async Task<FormatedResponse> DeleteIds(GenericUnitOfWork _uow, List<string> ids)
        {
            var response = await _genericRepository.DeleteIds(_uow, ids);
            return response;
        }

        public async Task<FormatedResponse> QueryOrgPermissionList(SYS_USER user)
        {
            try
            {
                var entity = _dbContext.HuOrganizations.AsNoTracking().AsQueryable();

                List<HuOrganizationMinimumDTO> currentList;
                List<HuOrganizationMinimumDTO> wholeList = await (from o in entity.AsNoTracking()
                                                                  orderby (o.ORDER_NUM ?? 9999) descending
                                                                  select new HuOrganizationMinimumDTO()
                                                                  {
                                                                      Id = o.ID,
                                                                      Name = o.NAME,
                                                                      Code = o.CODE,
                                                                      ParentId = o.PARENT_ID,
                                                                      OrderNum = o.ORDER_NUM ?? 9999,
                                                                      Protected = false,
                                                                      IsActive = o.IS_ACTIVE ?? false
                                                                  }).ToListAsync();

                if (user.IS_ADMIN)
                {
                    currentList = wholeList;
                    return new FormatedResponse()
                    {
                        InnerBody = currentList
                    };
                }
                else
                {
                    // By default Org Linear List taken from SysUserOrgs
                    currentList = await (from suo in _dbContext.SysUserOrgs.AsQueryable()
                                         where suo.USER_ID == user.ID
                                         join unit in entity on suo.ORG_ID equals unit.ID
                                         orderby (unit.ORDER_NUM ?? 9999) descending
                                         select new HuOrganizationMinimumDTO()
                                         {
                                             Id = unit.ID,
                                             Name = unit.NAME,
                                             ParentId = unit.PARENT_ID,
                                             OrderNum = unit.ORDER_NUM ?? 9999,
                                             Protected = false,
                                             IsActive = unit.IS_ACTIVE ?? false
                                         }).ToListAsync();

                    // But if no result, Org Linear List taken from SysUserGroupOrgs
                    if (currentList == null || currentList.Count == 0)
                    {
                        currentList = await (from suo in _dbContext.SysUserGroupOrgs.AsQueryable()
                                             where suo.GROUP_ID == user.GROUP_ID
                                             join unit in entity on suo.ORG_ID equals unit.ID
                                             orderby (unit.ORDER_NUM ?? 9999) descending
                                             select new HuOrganizationMinimumDTO()
                                             {
                                                 Id = unit.ID,
                                                 Name = unit.NAME,
                                                 ParentId = unit.PARENT_ID,
                                                 OrderNum = unit.ORDER_NUM ?? 9999,
                                                 Protected = false,
                                                 IsActive = unit.IS_ACTIVE ?? false
                                             }).ToListAsync();

                    }


                    var count = currentList.Count;

                    for (var i = 0; i < count; i++)
                    {
                        var currentChild = currentList[i];
                        LoopInsertProtectedParentOrg(ref wholeList, ref currentList, currentChild);
                    }

                    return new FormatedResponse()
                    {
                        InnerBody = currentList
                    };
                }
            }
            catch (Exception ex)
            {
                return new FormatedResponse() { ErrorType = EnumErrorType.UNCATCHABLE, StatusCode = EnumStatusCode.StatusCode500, MessageCode = ex.Message };
            }
        }

        public async Task<FormatedResponse> QueryOrgWithPositions(SYS_USER user)
        {
            try
            {
                var entity = _dbContext.HuOrganizations.AsNoTracking().AsQueryable();

                List<HuOrganizationMinimumWithPositionDTO> currentList;
                List<HuPositionMinimumDTO> allPositions =
                    (from p in _dbContext.HuPositions.AsNoTracking()
                     from j in _dbContext.HuJobs.AsNoTracking().Where(j => j.ID == p.JOB_ID).DefaultIfEmpty()
                     from eBasic in _dbContext.HuEmployees.AsNoTracking().Where(e => e.POSITION_ID == p.ID).DefaultIfEmpty()
                     from eMaster in _dbContext.HuEmployees.AsNoTracking().Where(e => e.ID == p.MASTER).DefaultIfEmpty()
                     from eInterim in _dbContext.HuEmployees.AsNoTracking().Where(e => e.ID == p.INTERIM).DefaultIfEmpty()
                     from cvBasic in _dbContext.HuEmployeeCvs.AsNoTracking().Where(cv => cv.ID == eBasic.PROFILE_ID).DefaultIfEmpty()
                     from cvMaster in _dbContext.HuEmployeeCvs.AsNoTracking().Where(cv => cv.ID == eMaster.PROFILE_ID).DefaultIfEmpty()
                     from cvInterim in _dbContext.HuEmployeeCvs.AsNoTracking().Where(cv => cv.ID == eInterim.PROFILE_ID).DefaultIfEmpty()

                     select new HuPositionMinimumDTO()
                     {
                         OrgId = p.ORG_ID,
                         Id = p.ID,
                         Code = p.CODE,
                         Name = p.NAME,
                         IsTdv = p.IS_TDV,
                         JobName = j.NAME_VN ?? j.NAME_EN,
                         PositionName = p.NAME,
                         TdvId = eBasic.ID,
                         TdvFullname = cvBasic.FULL_NAME,
                         TdvAvatar = cvBasic.AVATAR,
                         Master = p.MASTER,
                         MasterFullname = cvMaster.FULL_NAME,
                         MasterAvatar = cvMaster.AVATAR,
                         Interim = p.INTERIM,
                         InterimFullname = cvInterim.FULL_NAME,
                         InterimAvatar = cvInterim.AVATAR
                     }).ToList();

                List<HuOrganizationMinimumWithPositionDTO> wholeList = await (from o in entity.AsNoTracking()
                                                                              orderby (o.ORDER_NUM ?? 9999) descending
                                                                              select new HuOrganizationMinimumWithPositionDTO()
                                                                              {
                                                                                  Id = o.ID,
                                                                                  Name = o.NAME,
                                                                                  ParentId = o.PARENT_ID,
                                                                                  OrderNum = o.ORDER_NUM ?? 9999,
                                                                                  Protected = false,
                                                                                  IsActive = o.IS_ACTIVE != false,
                                                                              }).ToListAsync();
                List<HuPositionMinimumDTO> tdvs; // tdv
                List<HuPositionMinimumDTO> masters; // duong nhiem
                List<HuPositionMinimumDTO> interims; // ke nhiem

                if (user.IS_ADMIN)
                {
                    wholeList.ForEach(item =>
                    {
                        tdvs = new();
                        masters = new();
                        interims = new();
                        allPositions.Where(x => x.OrgId == item.Id && x.IsTdv == true).ToList().ForEach(m =>
                        {
                            tdvs.Add(m);
                        });
                        allPositions.Where(x => x.OrgId == item.Id && x.Master != null).ToList().ForEach(m =>
                        {
                            masters.Add(m);
                        });
                        allPositions.Where(x => x.OrgId == item.Id && x.Interim != null).ToList().ForEach(m =>
                        {
                            interims.Add(m);
                        });

                        item.Tdvs = tdvs;
                        item.MasterPositions = masters;
                        item.InterimPositions = interims;

                    });

                    currentList = wholeList;
                    return new FormatedResponse()
                    {
                        InnerBody = currentList
                    };
                }
                else
                {
                    currentList = await (from suo in _dbContext.SysUserOrgs.AsQueryable()
                                         where suo.USER_ID == user.ID
                                         join unit in entity on suo.ORG_ID equals unit.ID
                                         orderby (unit.ORDER_NUM ?? 9999) descending
                                         select new HuOrganizationMinimumWithPositionDTO()
                                         {
                                             Id = unit.ID,
                                             Name = unit.NAME,
                                             ParentId = unit.PARENT_ID,
                                             OrderNum = unit.ORDER_NUM ?? 9999,
                                             IsActive = unit.IS_ACTIVE != false,
                                             Protected = false
                                         }).ToListAsync();


                    var count = currentList.Count;

                    // If the user personally have no org, look among his/her group
                    if (count == 0)
                    {
                        currentList = await (from sgo in _dbContext.SysUserGroupOrgs.AsQueryable()
                                             where sgo.GROUP_ID == user.GROUP_ID
                                             join unit in entity on sgo.ORG_ID equals unit.ID
                                             orderby (unit.ORDER_NUM ?? 9999) descending
                                             select new HuOrganizationMinimumWithPositionDTO()
                                             {
                                                 Id = unit.ID,
                                                 Name = unit.NAME,
                                                 ParentId = unit.PARENT_ID,
                                                 OrderNum = unit.ORDER_NUM ?? 9999,
                                                 IsActive = unit.IS_ACTIVE != false,
                                                 Protected = false
                                             }).ToListAsync();
                        count = currentList.Count;
                    }

                    for (var i = 0; i < count; i++)
                    {
                        var currentChild = currentList[i];
                        LoopInsertProtectedParentOrg(ref wholeList, ref currentList, currentChild);
                    }

                    currentList.ForEach(item =>
                    {
                        tdvs = new();
                        masters = new();
                        interims = new();
                        allPositions.Where(x => x.OrgId == item.Id && x.IsTdv == true).ToList().ForEach(m =>
                        {
                            tdvs.Add(m);
                        });
                        allPositions.Where(x => x.OrgId == item.Id && x.Master != null).ToList().ForEach(m =>
                        {
                            masters.Add(m);
                        });
                        allPositions.Where(x => x.OrgId == item.Id && x.Interim != null).ToList().ForEach(m =>
                        {
                            interims.Add(m);
                        });

                        item.Tdvs = tdvs;
                        item.MasterPositions = masters;
                        item.InterimPositions = interims;
                    });

                    return new FormatedResponse()
                    {
                        InnerBody = currentList
                    };
                }
            }
            catch (Exception ex)
            {
                return new FormatedResponse() { ErrorType = EnumErrorType.UNCATCHABLE, StatusCode = EnumStatusCode.StatusCode500, MessageCode = ex.Message };
            }
        }

        private void LoopInsertProtectedParentOrg(ref List<HuOrganizationMinimumDTO> wholeList, ref List<HuOrganizationMinimumDTO> currentList, HuOrganizationMinimumDTO currentChild)
        {
            if (currentChild.ParentId != null)
            {
                var parentInCurrentList = currentList.Where(x => x.Id == currentChild.ParentId).FirstOrDefault();
                if (parentInCurrentList == null)
                {
                    var parentInWholeList = wholeList.Where(x => x.Id == currentChild.ParentId).FirstOrDefault();
                    if (parentInWholeList != null)
                    {
                        parentInWholeList.Protected = true;
                        currentList.Add(parentInWholeList);
                        LoopInsertProtectedParentOrg(ref wholeList, ref currentList, parentInWholeList);
                    }
                }
                else
                {
                    LoopInsertProtectedParentOrg(ref wholeList, ref currentList, parentInCurrentList);
                }
            }
        }

        private void LoopInsertProtectedParentOrg(ref List<HuOrganizationMinimumWithPositionDTO> wholeList, ref List<HuOrganizationMinimumWithPositionDTO> currentList, HuOrganizationMinimumWithPositionDTO currentChild)
        {
            if (currentChild.ParentId != null)
            {
                var parentInCurrentList = currentList.Where(x => x.Id == currentChild.ParentId).FirstOrDefault();
                if (parentInCurrentList == null)
                {
                    var parentInWholeList = wholeList.Where(x => x.Id == currentChild.ParentId).FirstOrDefault();
                    if (parentInWholeList != null)
                    {
                        parentInWholeList.Protected = true;
                        currentList.Add(parentInWholeList);
                        LoopInsertProtectedParentOrg(ref wholeList, ref currentList, parentInWholeList);
                    }
                }
                else
                {
                    LoopInsertProtectedParentOrg(ref wholeList, ref currentList, parentInCurrentList);
                }
            }
        }

        public async Task<FormatedResponse> QueryUserOrgPermissionList(/* param */SYS_USER user, bool? useGroupIfEmpty = true)
        {
            try
            {
                var entity = _dbContext.HuOrganizations.AsNoTracking().AsQueryable();
                var sysUserOrgsForCurUser = await (from suo in _dbContext.SysUserOrgs.AsQueryable()
                                                   where suo.USER_ID == user.ID
                                                   select new
                                                   {
                                                       OrgId = suo.ORG_ID
                                                   }).OrderBy(x => x.OrgId).ToListAsync();

                List<long> orgIds = new();
                sysUserOrgsForCurUser.ForEach(item => orgIds.Add(item.OrgId));

                return new()
                {
                    InnerBody = orgIds
                };
            }
            catch (Exception ex)
            {
                return new FormatedResponse() { ErrorType = EnumErrorType.UNCATCHABLE, StatusCode = EnumStatusCode.StatusCode500, MessageCode = ex.Message };
            }
        }

        public async Task<FormatedResponse> QueryFunctionActionPermissionList(SYS_USER user, bool? useGroupIfEmpty = true)
        {
            try
            {
                // by default given from SysUserFunctionActions
                var raw = (from ufa in _dbContext.SysUserFunctionActions.AsNoTracking()
                                .Where(x => x.USER_ID == user.ID)
                           from fa in _dbContext.SysFunctionActions.AsNoTracking().Where(x => x.FUNCTION_ID == ufa.FUNCTION_ID && x.ACTION_ID == ufa.ACTION_ID).DefaultIfEmpty()
                           from f in _dbContext.SysFunctions.AsNoTracking().Where(x => x.ID == ufa.FUNCTION_ID).DefaultIfEmpty()
                           from m in _dbContext.SysModules.AsNoTracking().Where(x => x.ID == f.MODULE_ID).DefaultIfEmpty()
                           from a in _dbContext.SysActions.AsNoTracking().Where(x => x.ID == ufa.ACTION_ID).DefaultIfEmpty()

                           where fa != null && f.ROOT_ONLY != true

                           select new SysUserFunctionActionDTO()
                           {
                               ModuleCode = m.CODE,
                               FunctionId = ufa.FUNCTION_ID,
                               FunctionCode = f.CODE,
                               ActionId = ufa.ACTION_ID,
                               FunctionPath = f.PATH,
                               ActionCode = a.CODE
                           }).OrderBy(x => x.ModuleCode).ThenBy(x => x.FunctionCode).ThenBy(x => x.ActionCode);

                // when no result, => given from SysGroupFunctionActions
                if ((raw == null || raw.ToList().Count == 0) && useGroupIfEmpty == true)
                {
                    raw = (from ufa in _dbContext.SysGroupFunctionActions.AsNoTracking()
                                    .Where(x => x.GROUP_ID == user.GROUP_ID)
                           from f in _dbContext.SysFunctions.Where(x => x.ID == ufa.FUNCTION_ID).DefaultIfEmpty()
                           from m in _dbContext.SysModules.Where(x => x.ID == f.MODULE_ID).DefaultIfEmpty()
                           from a in _dbContext.SysActions.Where(x => x.ID == ufa.ACTION_ID).DefaultIfEmpty()

                           from fa in _dbContext.SysFunctionActions.AsNoTracking().Where(x => x.FUNCTION_ID == ufa.FUNCTION_ID && x.ACTION_ID == ufa.ACTION_ID).DefaultIfEmpty()
                           where fa != null

                           select new SysUserFunctionActionDTO()
                           {
                               ModuleCode = m.CODE,
                               FunctionId = ufa.FUNCTION_ID,
                               FunctionCode = f.CODE,
                               ActionId = ufa.ACTION_ID,
                               FunctionPath = f.PATH,
                               ActionCode = a.CODE
                           }).OrderBy(x => x.ModuleCode).ThenBy(x => x.FunctionCode).ThenBy(x => x.ActionCode);
                }

                List<FunctionActionPermissionDTO> result = new();
                var list = await raw.Where(x => x.ModuleCode != null && x.FunctionCode != null && x.FunctionPath != null && x.ActionCode != null).ToListAsync();

                long? functionId = null;
                string moduleCode = "";
                string functionCode = "";
                string functionPath = "";
                List<long> actionIds = new();
                List<string> actionCodes = new();
                list.ForEach(r =>
                {
                    moduleCode = r.ModuleCode ?? "";
                    functionCode = r.FunctionCode ?? "";
                    functionPath = r.FunctionPath ?? "";
                    if (r.FunctionId != functionId)
                    {
                        if (functionId != null)
                        {
                            result.Add(new FunctionActionPermissionDTO()
                            {
                                FunctionId = (long)functionId,
                                AllowedActionIds = actionIds,
                                ModuleCode = moduleCode,
                                FunctionCode = functionCode,
                                FunctionUrl = functionPath,
                                AllowedActionCodes = actionCodes
                            });
                            actionIds = new();
                            actionCodes = new();
                            functionId = r.FunctionId;
                        }
                    }
                    functionId = r.FunctionId;
                    actionIds.Add((r.ActionId ?? 0));
                    actionCodes.Add(r.ActionCode ?? "");
                });
                // The tail
                if (functionId != null)
                {
                    result.Add(new FunctionActionPermissionDTO()
                    {
                        FunctionId = (long)functionId,
                        AllowedActionIds = actionIds,
                        ModuleCode = moduleCode,
                        FunctionCode = functionCode,
                        FunctionUrl = functionPath,
                        AllowedActionCodes = actionCodes
                    });
                }

                return new() { InnerBody = result };
            }
            catch (Exception ex)
            {
                return new FormatedResponse() { ErrorType = EnumErrorType.UNCATCHABLE, StatusCode = EnumStatusCode.StatusCode500, MessageCode = ex.Message };
            }
        }

        public async Task<FormatedResponse> ChangePassword(SysUserChangePasswordRequest request)
        {
            try
            {
                if (request.Id == null)
                {
                    return new()
                    {
                        ErrorType = EnumErrorType.CATCHABLE,
                        MessageCode = CommonMessageCode.NO_ID_PROVIED,
                        StatusCode = EnumStatusCode.StatusCode400
                    };
                }

                if (request.NewPassword != request.ConfirmNewPassword)
                {
                    return new()
                    {
                        ErrorType = EnumErrorType.CATCHABLE,
                        MessageCode = CommonMessageCode.PASSWORD_AND_PASSWORD_CONFIRN_DO_NOT_MATCH,
                        StatusCode = EnumStatusCode.StatusCode400
                    };

                }

                var entityToUpdate = await _dbContext.SysUsers.AsNoTracking().SingleAsync(x => x.ID == request.Id);

                if (entityToUpdate != null)
                {

                    if (BCrypt.Net.BCrypt.Verify(request.OldPassword, entityToUpdate.PASSWORDHASH))
                    {
                        entityToUpdate.PASSWORDHASH = BCrypt.Net.BCrypt.HashPassword(request.NewPassword);
                        entityToUpdate.IS_FIRST_LOGIN = false;
                        _dbContext.SysUsers.Update(entityToUpdate);
                        _dbContext.SaveChanges();
                        return new()
                        {
                            MessageCode = CommonMessageCode.SUCCESS_CHANGE_PASSWORD,
                            InnerBody = entityToUpdate
                        };
                    }
                    else
                    {
                        return new()
                        {
                            ErrorType = EnumErrorType.CATCHABLE,
                            MessageCode = CommonMessageCode.WRONG_CURRENT_PASSWORD,
                            StatusCode = EnumStatusCode.StatusCode400
                        };
                    }
                }
                else
                {
                    return new()
                    {
                        ErrorType = EnumErrorType.CATCHABLE,
                        MessageCode = CommonMessageCode.ENTITY_NOT_FOUND,
                        StatusCode = EnumStatusCode.StatusCode400
                    };
                }
            }
            catch (Exception ex)
            {
                return new() { MessageCode = ex.Message, ErrorType = EnumErrorType.UNCATCHABLE, StatusCode = EnumStatusCode.StatusCode500 };
            }
        }

        public virtual async Task<FormatedResponse> ToggleActiveIds(GenericUnitOfWork _uow, List<long> ids, bool valueToBind, string sid)
        {
            await Task.Run(() => null);
            throw new NotImplementedException();
        }

        public virtual async Task<FormatedResponse> LockAccount(List<string> userIds, string sid)
        {
            _dbContext.Database.BeginTransaction();
            try
            {
                var listToLock = _dbContext.SysUsers.Where(x => userIds.Contains(x.ID)).ToList();
                listToLock.ForEach(user => {
                        user.IS_LOCK = true;
                        user.UPDATED_BY = sid;
                    user.UPDATED_DATE = DateTime.UtcNow;
                    });
                _dbContext.SysUsers.UpdateRange(listToLock);
                await _dbContext.SaveChangesAsync();
                _dbContext.Database.CommitTransaction();
                return new() { InnerBody = listToLock, MessageCode = CommonMessageCode.LOCK_ACCOUNTS_SUCCESS };
            } catch (Exception ex)
            {
                _dbContext.Database.RollbackTransaction();
                return new() { MessageCode = ex.Message, ErrorType = EnumErrorType.UNCATCHABLE, StatusCode = EnumStatusCode.StatusCode500 };
            }
        }

        public virtual async Task<FormatedResponse> UnlockAccount(List<string> userIds, string sid)
        {
            _dbContext.Database.BeginTransaction();
            try
            {
                var listToUnlock = _dbContext.SysUsers.Where(x => userIds.Contains(x.ID)).ToList();
                listToUnlock.ForEach(user => {
                    user.IS_LOCK = false;
                    user.UPDATED_BY = sid;
                    user.UPDATED_DATE = DateTime.UtcNow;
                    });
                _dbContext.SysUsers.UpdateRange(listToUnlock);
                await _dbContext.SaveChangesAsync();
                _dbContext.Database.CommitTransaction();
                return new() { InnerBody = listToUnlock, MessageCode = CommonMessageCode.UNLOCK_ACCOUNTS_SUCCESS };
            }
            catch (Exception ex)
            {
                _dbContext.Database.RollbackTransaction();
                return new() { MessageCode = ex.Message, ErrorType = EnumErrorType.UNCATCHABLE, StatusCode = EnumStatusCode.StatusCode500};
            }
        }


        public async Task<FormatedResponse> ChangePasswordPortal(SysUserChangePasswordRequest request)
        {
            try
            {
                if (request.Id == null)
                {
                    return new()
                    {
                        ErrorType = EnumErrorType.CATCHABLE,
                        MessageCode = CommonMessageCode.NO_ID_PROVIED,
                        StatusCode = EnumStatusCode.StatusCode400
                    };
                }

                if (request.NewPassword != request.ConfirmNewPassword)
                {
                    return new()
                    {
                        ErrorType = EnumErrorType.CATCHABLE,
                        MessageCode = CommonMessageCode.PASSWORD_AND_PASSWORD_CONFIRN_DO_NOT_MATCH,
                        StatusCode = EnumStatusCode.StatusCode400
                    };

                }

                var entityToUpdate = await _dbContext.SysUsers.AsNoTracking().SingleAsync(x => x.ID == request.Id);

                if (entityToUpdate != null)
                {

                    if (BCrypt.Net.BCrypt.Verify(request.OldPassword, entityToUpdate.PASSWORDHASH))
                    {
                        entityToUpdate.PASSWORDHASH = BCrypt.Net.BCrypt.HashPassword(request.NewPassword);
                        entityToUpdate.IS_FIRST_LOGIN = false;
                        _dbContext.SysUsers.Update(entityToUpdate);
                        _dbContext.SaveChanges();
                        return new()
                        {
                            MessageCode = CommonMessageCode.SUCCESS_CHANGE_PASSWORD,
                            InnerBody = entityToUpdate
                        };
                    }
                    else
                    {
                        return new()
                        {
                            ErrorType = EnumErrorType.CATCHABLE,
                            MessageCode = CommonMessageCode.WRONG_CURRENT_PASSWORD,
                            StatusCode = EnumStatusCode.StatusCode400
                        };
                    }
                }
                else
                {
                    return new()
                    {
                        ErrorType = EnumErrorType.CATCHABLE,
                        MessageCode = CommonMessageCode.ENTITY_NOT_FOUND,
                        StatusCode = EnumStatusCode.StatusCode400
                    };
                }
            }
            catch (Exception ex)
            {
                return new() { MessageCode = ex.Message, ErrorType = EnumErrorType.UNCATCHABLE, StatusCode = EnumStatusCode.StatusCode500 };
            }
        }

        public async Task<FormatedResponse> GetUserByEmployeeId(long employeeId)
        {
            try
            {
                var response = await _dbContext.SysUsers.Where(x => x.EMPLOYEE_ID == employeeId).FirstOrDefaultAsync();
                return new FormatedResponse() { InnerBody = response };
            }
            catch (Exception ex)
            {

                return new FormatedResponse() { ErrorType = EnumErrorType.UNCATCHABLE, StatusCode = EnumStatusCode.StatusCode500, MessageCode = ex.Message };
            }
            
        }

        public async void TurnOffAccountUser()
        {
            var sysOtherList = _dbContext.SysOtherLists.SingleOrDefault(x => x.CODE == "DD");
            var getLeaveJob = await _dbContext.HuTerminates.Where(x => x.EFFECT_DATE <= DateTime.Now && x.STATUS_ID == sysOtherList!.ID).ToListAsync();
            getLeaveJob.ForEach( item =>
            {
                var getSysUserToTurnOff = _dbContext.SysUsers.Where(x => x.EMPLOYEE_ID == item.EMPLOYEE_ID).FirstOrDefault();
                getSysUserToTurnOff!.IS_LOCK = true;
                _dbContext.SaveChanges();
            });
        }
    }
}

