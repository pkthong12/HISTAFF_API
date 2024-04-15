using CORE.GenericUOW;
using CORE.DTO;
using CORE.Enum;
using CORE.StaticConstant;
using API.All.DbContexts;
using API.Entities.PORTAL;
using API.DTO.PortalDTO;
using Microsoft.AspNetCore.SignalR;
using API.Socket;
using API.DTO;
using System.Security.Principal;
using CORE.Services.File;
using Microsoft.Extensions.Options;

namespace API.Controllers.PortalRequestChange
{
    public class PortalRequestChangeRepository : IPortalRequestChangeRepository
    {
        private readonly GenericUnitOfWork _uow;
        private readonly FullDbContext _dbContext;
        private IGenericRepository<PORTAL_REQUEST_CHANGE, PortalRequestChangeDTO> _genericRepository;
        private readonly GenericReducer<PORTAL_REQUEST_CHANGE, PortalRequestChangeDTO> _genericReducer;
        IHubContext<SignalHub> _hubContext;
        private readonly AppSettings _appSettings;
        private readonly IWebHostEnvironment _env;
        private readonly IFileService _fileService;



        public PortalRequestChangeRepository(FullDbContext context, GenericUnitOfWork uow, IWebHostEnvironment env, IHubContext<SignalHub> hubContext, IFileService fileService, IOptions<AppSettings> options)
        {
            _dbContext = context;
            _uow = uow;
            _env = env;

            _genericRepository = _uow.GenericRepository<PORTAL_REQUEST_CHANGE, PortalRequestChangeDTO>();
            _genericReducer = new();
            _hubContext = hubContext;
            _fileService = fileService;
            _appSettings = options.Value;
        }

        public async Task<GenericPhaseTwoListResponse<PortalRequestChangeDTO>> SinglePhaseQueryList(GenericQueryListDTO<PortalRequestChangeDTO> request)
        {
            var joined = from p in _dbContext.PortalRegisterOffs.AsNoTracking()
                             // JOIN OTHER ENTITIES BASED ON THE BUSINESS
                         select new PortalRequestChangeDTO
                         {
                             Id = p.ID
                         };

            var singlePhaseResult = await _genericReducer.SinglePhaseReduce(joined, request);
            return singlePhaseResult;
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
                var list = new List<PORTAL_REQUEST_CHANGE>
                    {
                        (PORTAL_REQUEST_CHANGE)response
                    };
                var joined = (from l in list
                                  // JOIN OTHER ENTITIES BASED ON THE BUSINESS
                              select new PortalRequestChangeDTO
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
            await Task.Run(() => null);
            throw new NotImplementedException();
        }

        public async Task<FormatedResponse> Create(GenericUnitOfWork _uow, PortalRequestChangeDTO dto, string sid)
        {
            var response = await _genericRepository.Create(_uow, dto, sid);
            return response;
        }

        public async Task<FormatedResponse> CreateRange(GenericUnitOfWork _uow, List<PortalRequestChangeDTO> dtos, string sid)
        {
            var add = new List<PortalRequestChangeDTO>();
            add.AddRange(dtos);
            var response = await _genericRepository.CreateRange(_uow, add, sid);
            return response;
        }

        public async Task<FormatedResponse> Update(GenericUnitOfWork _uow, PortalRequestChangeDTO dto, string sid, bool patchMode = true)
        {
            var response = await _genericRepository.Update(_uow, dto, sid, patchMode);
            return response;
        }

        public async Task<FormatedResponse> Approve(GenericUnitOfWork _uow, PortalRequestChangeDTO dto, string sid, bool patchMode = true)
        {
            await Task.Run(() => null);
            throw new NotImplementedException();
        }

        public async Task<FormatedResponse> UpdateRange(GenericUnitOfWork _uow, List<PortalRequestChangeDTO> dtos, string sid, bool patchMode = true)
        {
            var response = await _genericRepository.UpdateRange(_uow, dtos, sid, patchMode);
            return response;
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
            await Task.Run(() => null);
            throw new NotImplementedException();
        }
        public async Task<FormatedResponse> SendRequest(PortalRequestChangeDTO dto, string sid)
        {
            dto.IsApprove = 993;
            List<UploadFileResponse> uploadFiles = new();
            if (dto.AttachmentBuffer != null)
            {
                string location = Path.Combine(_env.ContentRootPath, _appSettings.StaticFolders.Root, _appSettings.StaticFolders.Attachments);
                var uploadFileResponse = await _fileService.UploadFile(new UploadRequest()
                {
                    ClientFileName = dto.AttachmentBuffer.ClientFileName,
                    ClientFileType = dto.AttachmentBuffer.ClientFileType,
                    ClientFileData = dto.AttachmentBuffer.ClientFileData
                }, location, sid);
                var property = typeof(PortalRequestChangeDTO).GetProperty("FileName");

                if (property != null)
                {
                    property?.SetValue(dto, uploadFileResponse.SavedAs);
                    uploadFiles.Add(uploadFileResponse);
                }
                else
                {
                    return new FormatedResponse() { ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400, MessageCode = CommonMessageCode.ASSIGN_TO_VALUE_DOES_NOT_MATCH_ANY_COLUMN + ": Attachment" };
                }
            }
            var response = await _genericRepository.Create(_uow, dto, sid);
            return new() {
                InnerBody = response.InnerBody,
                MessageCode = CommonMessageCode.SEND_SUCCESS,
                StatusCode = EnumStatusCode.StatusCode200
            };
        }

        public async Task<FormatedResponse> GetSalAllowanceProcessByEmp(long id)
        {
            try
            {
                var entity = _uow.Context.Set<HU_WORKING>().AsNoTracking().AsQueryable();
                var employees = _uow.Context.Set<HU_EMPLOYEE>().AsNoTracking().AsQueryable();
                var positions = _uow.Context.Set<HU_POSITION>().AsNoTracking().AsQueryable();
                var organizations = _uow.Context.Set<HU_ORGANIZATION>().AsNoTracking().AsQueryable();
                var otherLists = _uow.Context.Set<SYS_OTHER_LIST>().AsNoTracking().AsQueryable();
                var salaryTypes = _uow.Context.Set<HU_SALARY_TYPE>().AsNoTracking().AsQueryable();
                var salaryRanks = _uow.Context.Set<HU_SALARY_RANK>().AsNoTracking().AsQueryable();
                var salaryScales = _uow.Context.Set<HU_SALARY_SCALE>().AsNoTracking().AsQueryable();
                var salaryLevels = _uow.Context.Set<HU_SALARY_LEVEL>().AsNoTracking().AsQueryable();
                var companyInfos = _uow.Context.Set<HU_COMPANY>().AsNoTracking().AsQueryable();
                var allowanceEmp = _uow.Context.Set<HU_ALLOWANCE_EMP>().AsNoTracking().AsQueryable();
                var allowance = _uow.Context.Set<HU_ALLOWANCE>().AsNoTracking().AsQueryable();
                var workingAllow = _uow.Context.Set<HU_WORKING_ALLOW>().AsNoTracking().AsQueryable();

                var joined = await (from p in entity
                                    from e in employees.Where(c => c.ID == p.EMPLOYEE_ID)
                                    from t in positions.Where(c => c.ID == p.POSITION_ID)
                                    from o in organizations.Where(c => c.ID == p.ORG_ID)
                                    from f in otherLists.Where(c => c.ID == p.STATUS_ID).DefaultIfEmpty()
                                    from l in otherLists.Where(c => c.ID == p.TYPE_ID)
                                    from s in employees.Where(c => c.ID == p.SIGN_ID).DefaultIfEmpty()
                                    from tl in salaryTypes.Where(c => c.ID == p.SALARY_TYPE_ID).DefaultIfEmpty()
                                    from sc in salaryScales.Where(c => c.ID == p.SALARY_SCALE_ID).DefaultIfEmpty()
                                    from sdcv in salaryScales.Where(c => c.ID == p.SALARY_SCALE_DCV_ID).DefaultIfEmpty()
                                    from ra in salaryRanks.Where(c => c.ID == p.SALARY_RANK_ID).DefaultIfEmpty()
                                    from sl in salaryLevels.Where(c => c.ID == p.SALARY_LEVEL_ID).DefaultIfEmpty()
                                    from sldcv in salaryLevels.Where(c => c.ID == p.SALARY_LEVEL_DCV_ID).DefaultIfEmpty()
                                    from tax in otherLists.Where(c => c.ID == p.TAXTABLE_ID).DefaultIfEmpty()
                                    from rdcv in salaryRanks.Where(c => c.ID == p.SALARY_RANK_DCV_ID).DefaultIfEmpty()
                                    from ci in companyInfos.Where(c => c.ID == o.COMPANY_ID).DefaultIfEmpty()
                                    from r in otherLists.Where(x => x.ID == ci.REGION_ID).DefaultIfEmpty()
                                    where p.IS_WAGE == -1 && p.EMPLOYEE_ID == id
                                    orderby p.EFFECT_DATE
                                    select new
                                    {
                                        Id = p.ID,
                                        OrgId = p.ORG_ID,
                                        EmployeeId = id,
                                        EmployeeCode = e.CODE,
                                        EmployeeName = e.Profile.FULL_NAME,
                                        PositionName = t.NAME,
                                        SignDate = p.SIGN_DATE,
                                        SignerName = p.SIGNER_NAME,
                                        SignerCode = s.CODE,
                                        SignerPosition = p.SIGNER_POSITION,
                                        OrgName = o.NAME,
                                        EffectDate = p.EFFECT_DATE,
                                        ExpireDate = p.EXPIRE_DATE,
                                        DecisionNo = p.DECISION_NO,
                                        StatusName = f.NAME,
                                        TypeName = l.NAME,
                                        SalBasic = p.SAL_BASIC,
                                        ShortTempSalary = p.SHORT_TEMP_SALARY,
                                        RegionName = r.NAME,
                                        TaxTableName = tax.NAME,
                                        Coefficient = p.COEFFICIENT,
                                        CoefficientDcv = p.COEFFICIENT_DCV,
                                        SalTotal = p.SAL_TOTAL,
                                        SalPercent = p.SAL_PERCENT,
                                        SalaryType = tl.NAME,
                                        salaryRankDcvName = rdcv.NAME,
                                        SalaryScaleDcvName = sdcv.NAME,
                                        SalaryScaleName = sc.NAME,
                                        SalaryRankName = ra.NAME,
                                        SalaryLevelName = sl.NAME,
                                        SalInsu = p.SAL_INSU,
                                        salaryLevelDcvName = sldcv.NAME,
                                        AllowanceList = (from wa in workingAllow
                                                         from n in allowance.Where(x => x.ID == wa.ALLOWANCE_ID)
                                                         from a in entity.Where(x => x.ID == wa.WORKING_ID)
                                                         where a.EMPLOYEE_ID == id && wa.WORKING_ID == p.ID
                                                         select new
                                                         {
                                                             Id = wa.ID,
                                                             WorkingId = wa.WORKING_ID,
                                                             AllowanceId = wa.ALLOWANCE_ID,
                                                             AllowanceName = n.NAME,
                                                             Amount = wa.AMOUNT,
                                                             Effectdate = wa.EFFECT_DATE,
                                                             ExpireDate = wa.EXPIRE_DATE,
                                                             Coefficient = wa.COEFFICIENT
                                                         }).ToList(),

                                    }).ToListAsync();
                return new FormatedResponse() { InnerBody = joined };
            }
            catch (Exception ex)
            {
                return new FormatedResponse() { ErrorType = EnumErrorType.UNCATCHABLE, StatusCode = EnumStatusCode.StatusCode500, MessageCode = ex.Message };
            }
        }

        public async Task<FormatedResponse> GetSalInsuByEmployeeId(long employeeId)
        {
            var getSalInsu = await (from w in _uow.Context.Set<HU_WORKING>().Where(x => x.EMPLOYEE_ID == employeeId)
                                    select new
                                    {
                                        Id = w.ID,
                                        SalInsu = w.SAL_INSU
                                    }).FirstOrDefaultAsync();
            return new FormatedResponse() { InnerBody = getSalInsu };
        }

        public async Task<FormatedResponse> GetConcurrentlyByEmpId(long id)
        {
            TimeSpan timeSpan;
            var joined = await (from p in _dbContext.HuConcurrentlys.AsNoTracking().Where(x => x.EMPLOYEE_ID == id)
                                from o in _dbContext.HuOrganizations.Where(x => x.ID == p.ORG_ID).DefaultIfEmpty()
                                from c in _dbContext.HuPositions.Where(x => x.ID == p.POSITION_ID).DefaultIfEmpty()
                                from y in _dbContext.SysOtherLists.Where(x => x.ID == p.STATUS_ID).DefaultIfEmpty()
                                from e in _dbContext.HuEmployees.Where(x => x.ID == p.EMPLOYEE_ID).DefaultIfEmpty()
                                    // JOIN OTHER ENTITIES BASED ON THE BUSINESS
                                select new
                                {
                                    Id = p.ID,
                                    IsActive = p.IS_ACTIVE,
                                    EffectiveDate = p.EFFECTIVE_DATE,
                                    ExpirationDate = p.EXPIRATION_DATE,
                                    EmployeeId = p.EMPLOYEE_ID,
                                    StatusId = p.STATUS_ID,
                                    Status = y.NAME,
                                    PositionConcurrentName = c.NAME,
                                    OrgConcurrentName = o.NAME,
                                    TimeConcurrentYear = ((p.EXPIRATION_DATE.Value - p.EFFECTIVE_DATE.Value).Days / 30) / 12,
                                    TimeConcurrentMonth = Math.Ceiling((decimal)(((p.EXPIRATION_DATE.Value - p.EFFECTIVE_DATE.Value).Days / 30) % 12)),
                                }).ToListAsync();
            return new() { InnerBody = joined, MessageCode = CommonMessageCode.SEND_SUCCESS, StatusCode = EnumStatusCode.StatusCode200 };
        }

        public virtual async Task<FormatedResponse> ToggleActiveIds(GenericUnitOfWork _uow, List<long> ids, bool valueToBind, string sid)
        {
            await Task.Run(() => null);
            throw new NotImplementedException();
        }

        public async Task<FormatedResponse> ApproveWorkingBeforeIds(List<long> Ids)
        {
            try
            {
                string sid = "";
                bool pathMode = true;
                List<PortalRequestChangeDTO> list = new();
                Ids.ForEach(item =>
                {
                    var response = _uow.Context.Set<PORTAL_REQUEST_CHANGE>().Where(x => x.ID == item).FirstOrDefault();
                    var otherList = _dbContext.SysOtherLists.Where(x => x.CODE == "DD").FirstOrDefault();
                    response.IS_APPROVE = otherList?.ID;
                    list.Add(new PortalRequestChangeDTO()
                    {
                        Id = response.ID,
                        IsApprove = response.IS_APPROVE
                    });
                    AT_NOTIFICATION noti = new AT_NOTIFICATION()
                    {
                        TYPE = response.SYS_OTHER_CODE == "00042" ? 8 : (response.SYS_OTHER_CODE == "00043" ? 7 : 0),
                        ACTION = 2,
                        EMP_NOTIFY_ID = response.EMPLOYEE_ID.ToString(),
                        STATUS_NOTIFY = 1,
                        REF_ID = response.ID,

                    };
                    _dbContext.AtNotifications.AddRange(noti);
                    _dbContext.SaveChanges();
                    var employeeId = response.EMPLOYEE_ID;
                    var users = _dbContext.SysUsers.Where(x => x.EMPLOYEE_ID == employeeId).ToList();
                    for (var i = 0; i < users.Count; i++)
                    {
                        var username = users[i].USERNAME;
                        if (!string.IsNullOrEmpty(username))
                        {
                            _hubContext.Clients.User(username).SendAsync("ReceiveMessage", new
                            {
                                SignalType = "APPROVE_NOTIFICATION",
                                Message = "Bạn có thông báo mới trên Portal"/*message*/,
                                Data = new
                                {

                                }
                            });
                        }

                    }
                });
                var approved = await _genericRepository.UpdateRange(_uow, list, sid, pathMode);
                if (approved != null)
                {
                    return approved;
                }
                else
                {
                    return new FormatedResponse() { MessageCode = CommonMessageCode.ENTITY_NOT_FOUND, ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400 };
                }
            }
            catch (Exception ex)
            {

                return new FormatedResponse() { ErrorType = EnumErrorType.UNCATCHABLE, StatusCode = EnumStatusCode.StatusCode500, MessageCode = ex.Message };
            }
        }
    }
}

