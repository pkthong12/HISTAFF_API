using CORE.GenericUOW;
using CORE.DTO;
using API.DTO;
using CORE.Enum;
using CORE.StaticConstant;
using API.All.DbContexts;
using API.Controllers.SysUser;
using Microsoft.Extensions.Options;
using CORE.Services.File;
using API.All.HRM.Profile.ProfileAPI.HuOrganization;
using Common.Extensions;
using Common.Interfaces;
using Common.DataAccess;
using API.All.Services;
using Common.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.HuOrganization
{
    public class HuOrganizationRepository : RepositoryBase<HU_ORGANIZATION>, IHuOrganizationRepository
    {
        private readonly GenericUnitOfWork _uow;
        private readonly FullDbContext _dbContext;
        private IGenericRepository<HU_ORGANIZATION, HuOrganizationDTO> _genericRepository;
        private SysUserRepository _sysUserRepository;
        private readonly GenericReducer<HU_ORGANIZATION, HuOrganizationDTO> _genericReducer;
        protected AbsQueryDataTemplate QueryData;
        private readonly IWebHostEnvironment _env;
        private readonly AppSettings _appSettings;
        private readonly IFileService _fileService;
        private IEmailService emailService;

        public HuOrganizationRepository(FullDbContext context, IWebHostEnvironment env, IOptions<AppSettings> options, IFileService fileService, IEmailService emailService) : base(context)
        {
            _dbContext = context;
            _uow = new(context);
            _genericRepository = _uow.GenericRepository<HU_ORGANIZATION, HuOrganizationDTO>();
            _sysUserRepository = new(context, _uow, env, options, fileService, emailService);
            _genericReducer = new();
            QueryData = new SqlQueryDataTemplate(_dbContext);
            _env = env;
            _appSettings = options.Value;
            _fileService = fileService;
            this.emailService = emailService;
        }

        public async Task<GenericPhaseTwoListResponse<HuOrganizationDTO>> SinglePhaseQueryList(GenericQueryListDTO<HuOrganizationDTO> request)
        {
            var joined = from p in _dbContext.HuOrganizations.AsNoTracking()
                         from p1 in _dbContext.HuOrganizations.AsNoTracking().Where(x => p.PARENT_ID == x.ID && x.ID != p.ID).DefaultIfEmpty()
                         select new HuOrganizationDTO
                         {
                             Id = p.ID,
                             Code = p.CODE,
                             Name = p.NAME,
                             ParentName = p1.NAME,
                             Note = p.NOTE,
                             StatusString = p.IS_ACTIVE == true ? "Hoạt động" : "Giải thể"
                         };

            var singlePhaseResult = await _genericReducer.SinglePhaseReduce(joined, request);
            return singlePhaseResult;
        }

        public async Task<List<HuOrganizationTreeBlockDTO>> BuildOrgTree(string sid)
        {
            var user = _dbContext.SysUsers.Single(x => x.ID == sid);
            List<HuOrganizationTreeBlockDTO> result = new();


            if (user != null)
            {
                var permissionOrgListResponse = await _sysUserRepository.QueryOrgPermissionList(user);
                if (permissionOrgListResponse.StatusCode == EnumStatusCode.StatusCode200)
                {
                    if (permissionOrgListResponse.InnerBody != null)
                    {
                        List<HuOrganizationMinimumDTO>? list = (List<HuOrganizationMinimumDTO>)permissionOrgListResponse.InnerBody;
                        if (list != null)
                        {
                            list.ForEach(item =>
                            {
                                result.Add(new HuOrganizationTreeBlockDTO
                                {
                                    Id = item.Id,
                                    Name = item.Name,
                                    ParentId = item.ParentId,
                                    OrderNum = item.OrderNum,
                                    Code = item.Code,
                                    Children = new()
                                });
                            });

                            var i = result.Count;

                            if (i > 0)
                            {
                                while (i-- > 0)
                                {
                                    if (i < result.Count)
                                    {
                                        var item = result[i];
                                        LoopCurrentTreeItem(result, item);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return result;
        }

        private void LoopCurrentTreeItem(List<HuOrganizationTreeBlockDTO> currentList, HuOrganizationTreeBlockDTO currentParent)
        {
            var id = currentParent.Id;
            var children = currentList.Where(x => x.ParentId == id).ToList();
            if (children != null)
            {
                var i = children.Count;
                if (i > 0)
                {
                    while (i-- > 0)
                    {
                        if (i < children.Count)
                        {
                            var child = children[i];
                            currentParent.Children.Add(child);
                            var index = currentList.IndexOf(child);

                            if (index > -1) currentList.Remove(child);
                        }
                    }
                }

                i = currentParent.Children.Count;
                if (i > 0)
                {
                    while (i-- > 0)
                    {
                        if (i < currentParent.Children.Count)
                        {
                            var child = currentParent.Children[i];
                            LoopCurrentTreeItem(currentList, child);
                        }
                    }
                }
            }
        }

        public async Task<List<HuOrganizationTreeBlockPositionDTO>> BuildOrgPositionTree(string sid)
        {
            var user = _dbContext.SysUsers.Single(x => x.ID == sid);
            List<HuOrganizationTreeBlockPositionDTO> result = new();


            if (user != null)
            {
                var permissionOrgListResponse = await _sysUserRepository.QueryOrgPermissionList(user);
                if (permissionOrgListResponse.StatusCode == EnumStatusCode.StatusCode200)
                {
                    if (permissionOrgListResponse.InnerBody != null)
                    {
                        List<HuOrganizationMinimumDTO>? list = (List<HuOrganizationMinimumDTO>)permissionOrgListResponse.InnerBody;
                        if (list != null)
                        {
                            list.ForEach(item =>
                            {
                                List<HuPositionMinimumDTO> list = new();
                                var positions = (from p in _dbContext.HuPositions.AsNoTracking().Where(x => x.ORG_ID == item.Id)
                                                 from e in _dbContext.HuEmployees.AsNoTracking().Where(x => x.ID == p.MASTER).DefaultIfEmpty()
                                                 from c in _dbContext.HuEmployeeCvs.AsNoTracking().Where(x => x.ID == e.PROFILE_ID).DefaultIfEmpty()

                                                 select new HuPositionMinimumDTO()
                                                 {
                                                     Id = p.ID,
                                                     Code = p.CODE ?? "",
                                                     Name = p.NAME ?? "",
                                                     Master = p.MASTER,
                                                     MasterFullname = c.FULL_NAME
                                                 }).ToList();

                                if (positions != null)
                                {
                                    list = positions;
                                }

                                result.Add(new HuOrganizationTreeBlockPositionDTO
                                {
                                    Id = item.Id,
                                    Name = item.Name,
                                    Code = item.Code,
                                    ParentId = item.ParentId,
                                    OrderNum = item.OrderNum,
                                    Children = new(),
                                    Positions = list
                                });
                            });

                            var i = result.Count;

                            if (i > 0)
                            {
                                while (i-- > 0)
                                {
                                    if (i < result.Count)
                                    {
                                        var item = result[i];
                                        LoopCurrentTreeItem(result, item);
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return result;

        }

        private void LoopCurrentTreeItem(List<HuOrganizationTreeBlockPositionDTO> currentList, HuOrganizationTreeBlockPositionDTO currentParent)
        {
            var id = currentParent.Id;
            var children = currentList.Where(x => x.ParentId == id).ToList();
            if (children != null)
            {
                var i = children.Count;
                if (i > 0)
                {
                    while (i-- > 0)
                    {
                        if (i < children.Count)
                        {
                            var child = children[i];
                            currentParent.Children.Add(child);
                            var index = currentList.IndexOf(child);

                            if (index > -1) currentList.Remove(child);
                        }
                    }
                }

                i = currentParent.Children.Count;
                if (i > 0)
                {
                    while (i-- > 0)
                    {
                        if (i < currentParent.Children.Count)
                        {
                            var child = currentParent.Children[i];
                            LoopCurrentTreeItem(currentList, child);
                        }
                    }
                }
            }
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
            try
            {
                var res = await _genericRepository.GetById(id);
                if (res.InnerBody != null)
                {
                    var response = res.InnerBody;
                    string TdvEmployeeNames = "";
                    string TdvPositionNames = "";

                    var list = new List<HU_ORGANIZATION>
                    {
                        (HU_ORGANIZATION)response
                    };

                    /*var IsTdvPositions = from p in _dbContext.HuPositions where p.IS_TDV == true && p.ORG_ID == id select p;
                    var IsTdvEmployeeList = await (from p in IsTdvPositions
                                                join e in _dbContext.HuEmployees on p.ID equals e.POSITION_ID into tdvEmployees

                                                from tdvEmployeesResult in tdvEmployees.DefaultIfEmpty()
                                                join ecv in _dbContext.HuEmployeeCvs on tdvEmployeesResult.PROFILE_ID  equals ecv.ID into ecvs
                                                from ecvsResult in ecvs.DefaultIfEmpty()

                                                select new HuEmployeeDTO
                                                {
                                                    Fullname = ecvsResult.FULL_NAME
                                                }).ToListAsync();


                    var IsTdvPositionList = await (from p in IsTdvPositions
                                                select new HuPositionDTO
                                                {
                                                    Name = p.NAME
                                                }).ToListAsync();

                    
                    

                    if (IsTdvPositionList != null && IsTdvPositionList.Count > 0)
                    {
                        IsTdvPositionList.ForEach(name => TdvPositionNames += name.Name + " ");
                        TdvPositionNames = TdvPositionNames[..^1];
                    }

                    if (IsTdvEmployeeList != null && IsTdvEmployeeList.Count > 0)
                    {
                        IsTdvEmployeeList.ForEach(name => TdvEmployeeNames += name.Fullname + " ");
                        TdvEmployeeNames = TdvEmployeeNames[..^1];
                    }
*/

                    string tdvPositionName = "", tdvEmployeeName = "";
                    var tdvPositionID = _dbContext.HuOrganizations.Where(p => p.ID == id).Select(p => p.HEAD_POS_ID).First();
                    if (tdvPositionID != null)
                    {
                        tdvPositionName = _dbContext.HuPositions.Where(p => p.ID == tdvPositionID).First().NAME!;
                        var tdvEmployeeID = _dbContext.HuPositions.Where(p => p.ID == tdvPositionID).Select(p => p.MASTER).First();
                        if (tdvEmployeeID != null)
                        {
                            var employee = (from e in _dbContext.HuEmployees.AsNoTracking().Where(p => p.ID == tdvEmployeeID).DefaultIfEmpty()
                                            from cv in _dbContext.HuEmployeeCvs.AsNoTracking().Where(p => p.ID == e.PROFILE_ID).DefaultIfEmpty()
                                            select new HuEmployeeCvDTO
                                            {
                                                Id = e.ID,
                                                FullName = cv.FULL_NAME
                                            }).First();
                            tdvEmployeeName = employee.FullName!;
                        }
                    }

                    var joined = (from o in list
                                      // JOIN OTHER ENTITIES BASED ON THE BUSINESS
                                  join p in _dbContext.HuOrganizations on o?.PARENT_ID equals p.ID into parent
                                  from parentResult in parent.DefaultIfEmpty()

                                  join l in _dbContext.HuOrgLevels on o?.ORG_LEVEL_ID equals l.ID into level
                                  from levelResult in level.DefaultIfEmpty()

                                  join c in _dbContext.HuCompanys on o?.COMPANY_ID equals c.ID into company
                                  from companyResult in company.DefaultIfEmpty()


                                  select new HuOrganizationDTO
                                  {
                                      Id = o.ID,
                                      Code = o.CODE,
                                      Name = o.NAME,
                                      ParentId = o.PARENT_ID,
                                      ParentName = parentResult?.NAME,
                                      ParentNameEn = parentResult?.NAME_EN,
                                      CompanyId = o.COMPANY_ID,
                                      CompanyNameVn = companyResult != null ? companyResult?.NAME_VN : null,
                                      Address = companyResult != null ? companyResult?.WORK_ADDRESS : null,
                                      CompanyNameEn = companyResult != null ? companyResult?.NAME_EN : null,
                                      OrgLevelId = o.ORG_LEVEL_ID,
                                      OrgLevelName = levelResult?.NAME,
                                      FoundationDate = o.FOUNDATION_DATE,
                                      DissolveDate = o.DISSOLVE_DATE,
                                      HeadEmployeeNames = o.HEAD_POS_ID == null ? "" : tdvEmployeeName,
                                      HeadPosId = o.HEAD_POS_ID,
                                      HeadPosName = o.HEAD_POS_ID == null ? "" : tdvPositionName,
                                      OrderNum = o.ORDER_NUM,
                                      Note = o.NOTE,
                                      AttachedFile = o.ATTACHED_FILE,
                                      IsActive = o.IS_ACTIVE
                                  }).FirstOrDefault();

                    if (joined != null)
                    {
                        return new FormatedResponse() { InnerBody = joined };
                    }
                    else
                    {
                        return new() { ErrorType = EnumErrorType.CATCHABLE, MessageCode = CommonMessageCode.JOINED_QUERY_AFTER_GET_BY_ID_WAS_NULL, StatusCode = EnumStatusCode.StatusCode400 };
                    }
                }
                else
                {
                    return new FormatedResponse() { MessageCode = CommonMessageCode.ENTITY_NOT_FOUND, ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400 };
                }
            }
            catch (Exception ex)
            {
                return new FormatedResponse() { MessageCode = ex.Message, ErrorType = EnumErrorType.UNCATCHABLE, StatusCode = EnumStatusCode.StatusCode500 };
            }
        }

        public async Task<FormatedResponse> GetById(string id)
        {
            await Task.Run(() => null);
            throw new NotImplementedException();
        }

        public async Task<FormatedResponse> Create(GenericUnitOfWork _uow, HuOrganizationDTO dto, string sid)
        {

            _uow.CreateTransaction();
            List<UploadFileResponse> uploadFiles = new();
            try
            {
                if (dto.AttachedFileBuffer != null)
                {
                    string location = Path.Combine(_env.ContentRootPath, _appSettings.StaticFolders.Root, _appSettings.StaticFolders.Attachments);
                    var uploadFileResponse = await _fileService.UploadFile(new UploadRequest()
                    {
                        ClientFileName = dto.AttachedFileBuffer.ClientFileName,
                        ClientFileType = dto.AttachedFileBuffer.ClientFileType,
                        ClientFileData = dto.AttachedFileBuffer.ClientFileData
                    }, location, sid);

                    // Assign saved paths
                    var property = typeof(HuOrganizationDTO).GetProperty("AttachedFile");

                    if (property != null)
                    {
                        property?.SetValue(dto, uploadFileResponse.SavedAs);
                        uploadFiles.Add(uploadFileResponse);
                    }
                    else
                    {
                        return new() { ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400, MessageCode = CommonMessageCode.ASSIGN_TO_VALUE_DOES_NOT_MATCH_ANY_COLUMN + ": AttachmentFile" };
                    }
                }
                if(dto.FoundationDate == null)
                {
                    dto.FoundationDate = null;
                }
                if(dto.DissolveDate == null)
                {
                    dto.DissolveDate = null;
                }
                var response = await _genericRepository.Create(_uow, dto, sid);

                if (response.StatusCode == EnumStatusCode.StatusCode200)
                {
                    /* ADD new org ID to object permission */
                    var now = DateTime.UtcNow;

                    // check at user level
                    var level1 = _dbContext.SysUserOrgs.AsNoTracking().Any(x => x.USER_ID == sid);
                    if (level1)
                    {
                        SYS_USER_ORG newOrgPermission = new()
                        {
                            USER_ID = sid,
                            ORG_ID = ((HU_ORGANIZATION)response?.InnerBody!).ID,
                            CREATED_DATE = now,
                            CREATED_BY = sid
                        };
                        _dbContext.SysUserOrgs.Add(newOrgPermission);
                        _dbContext.SaveChanges();
                    }

                    // and always add to his/her group's permission
                    SYS_USER user = _dbContext.SysUsers.AsNoTracking().Single(x => x.ID == sid);
                    SYS_USER_GROUP_ORG newGroupOrgPermission = new()
                    {
                        GROUP_ID = (long)user!.GROUP_ID!,
                        ORG_ID = ((HU_ORGANIZATION)response?.InnerBody!).ID,
                        CREATED_DATE = now,
                        CREATED_BY = sid
                    };
                    _dbContext.SysUserGroupOrgs.Add(newGroupOrgPermission);
                    _dbContext.SaveChanges();

                    _uow.Commit();
                    return response;
                }
                else
                {
                    _uow.Rollback();
                    return new() { ErrorType = EnumErrorType.CATCHABLE, StatusCode = response.StatusCode, MessageCode = response.MessageCode };
                }
            }
            catch (Exception ex)
            {
                uploadFiles.ForEach(async f =>
                {
                    await _fileService.DeleteAttachment(f.SavedAs);
                });

                _uow.Rollback();
                return new() { ErrorType = EnumErrorType.UNCATCHABLE, StatusCode = EnumStatusCode.StatusCode500, MessageCode = ex.Message };
            }

        }

        public async Task<FormatedResponse> CreateRange(GenericUnitOfWork _uow, List<HuOrganizationDTO> dtos, string sid)
        {
            var add = new List<HuOrganizationDTO>();
            add.AddRange(dtos);
            var response = await _genericRepository.CreateRange(_uow, add, sid);
            return response;
        }

        public async Task<FormatedResponse> Update(GenericUnitOfWork _uow, HuOrganizationDTO dto, string sid, bool patchMode = true)
        {
            HU_ORGANIZATION org = new();
            if (dto.Id == dto.ParentId)
            {
                return new FormatedResponse() { MessageCode = CommonMessageCode.PARENT_ID_CANNOT_SAME_WITH_ID, ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode500 };
            }
            _uow.CreateTransaction();
            List<UploadFileResponse> uploadFiles = new();
            try
            {
                //dto.IsActive = null;
                var data = await _dbContext.HuOrganizations.Where(p => p.ID == dto.Id).FirstAsync();

                if (dto.AttachedFileBuffer != null)
                {
                    string location = Path.Combine(_env.ContentRootPath, _appSettings.StaticFolders.Root, _appSettings.StaticFolders.Attachments);
                    var uploadFileResponse = await _fileService.UploadFile(new UploadRequest()
                    {
                        ClientFileName = dto.AttachedFileBuffer.ClientFileName,
                        ClientFileType = dto.AttachedFileBuffer.ClientFileType,
                        ClientFileData = dto.AttachedFileBuffer.ClientFileData
                    }, location, sid);

                    // Assign saved paths
                    var property = typeof(HuOrganizationDTO).GetProperty("AttachedFile");

                    if (property != null)
                    {
                        property?.SetValue(dto, uploadFileResponse.SavedAs);
                        uploadFiles.Add(uploadFileResponse);
                    }
                    else
                    {
                        return new() { ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400, MessageCode = CommonMessageCode.ASSIGN_TO_VALUE_DOES_NOT_MATCH_ANY_COLUMN + ": AttachmentFile" };
                    }
                }
                if (dto.DissolveDate != null && dto.DissolveDate.Value.Date == DateTime.Now.Date)
                {

                    if (data != null)
                    {
                        var _checkEmp = await (from p in _dbContext.HuEmployees where p.ORG_ID == data.ID select p).FirstOrDefaultAsync();//nv thuoc phong ban
                        if(_checkEmp != null)
                        {
                            if (_checkEmp.WORK_STATUS_ID == OtherConfig.EMP_STATUS_WORKING)
                            {
                                return new FormatedResponse() { MessageCode = CommonMessageCode.DEPARTMENTS_CANNOT_BE_DISSOLVED_WHILE_EMPLOYEES_WORK, ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400 };
                            }
                        }
                    }
                    dto.IsActive = false;
                }
                var r = Map(dto, data);
                //org.ID = (long)dto.Id!;
                //org.NAME = dto.Name!;
                //org.CODE = dto.Code!;
                //org.COMPANY_ID = dto.CompanyId;
                //org.PARENT_ID = dto.ParentId;
                //org.FOUNDATION_DATE = dto.FoundationDate;
                //org.DISSOLVE_DATE = dto.DissolveDate;
                //org.ORG_LEVEL_ID = dto.OrgLevelId;
                //org.HEAD_POS_ID = dto.HeadPosId;
                //org.ORDER_NUM = dto.OrderNum;
                //org.ADDRESS = dto.Address;
                //org.ATTACHED_FILE = dto.AttachedFile;
                //org.IS_ACTIVE = dto.IsActive;
                //org.NOTE = dto.Note; 

                _dbContext.UpdateRange(r);
                _uow.Save();
                _uow.Commit();
                return new FormatedResponse() { MessageCode = CommonMessageCode.UPDATE_SUCCESS, StatusCode = EnumStatusCode.StatusCode200, InnerBody = org };
            }
            catch (Exception ex)
            {
                uploadFiles.ForEach(async f =>
                {
                    await _fileService.DeleteAttachment(f.SavedAs);
                });

                _uow.Rollback();
                return new() { ErrorType = EnumErrorType.UNCATCHABLE, StatusCode = EnumStatusCode.StatusCode500, MessageCode = ex.Message };
            }


        }

        public async Task<FormatedResponse> UpdateRange(GenericUnitOfWork _uow, List<HuOrganizationDTO> dtos, string sid, bool patchMode = true)
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
            var query = _dbContext.HuOrganizations.Where(x => ids.Contains(x.ID));

            foreach (var item in query)
            {
                if (item.STATUS == true)
                {
                    return new FormatedResponse()
                    {
                        ErrorType = EnumErrorType.CATCHABLE,
                        MessageCode = "Không được xóa bản ghi đang áp dụng",
                        StatusCode = EnumStatusCode.StatusCode400
                    };
                }
            }

            var response = await _genericRepository.DeleteIds(_uow, ids);
            return response;
        }

        public async Task<FormatedResponse> DeleteIds(GenericUnitOfWork _uow, List<string> ids)
        {
            await Task.Run(() => null);
            throw new NotImplementedException();
        }

        public virtual async Task<FormatedResponse> ToggleActiveIds(GenericUnitOfWork _uow, List<long> ids, bool valueToBind, string sid)
        {
            /* Nếu ngừng áp dụng một org unit */
            /*
             *  Không có đơn vị đơn con nào còn hoạt động
                Chuyển các vị trí áp dụng sang phòng ban khác. Tất cả Vị trí tại phòng ban (ngừng áp dụng) có trạng thái ngừng áp dụng.
                Check HU_employee: Gắn Positon_id = null hoặc Gắn Positon_id !=null nhưng có trạng thái đã nghỉ việc (code =ESQ)
                Khi nhấn ngừng áp dụng tự động cập nhật ngày giải thể bằng ngày nhấn ngừng áp dụng
             */

            // local functions
            bool IncludesActiveElement(HU_ORGANIZATION currentNode)
            {
                bool result = false;
                var children = _dbContext.HuOrganizations.AsNoTracking().Where(x => x.PARENT_ID == currentNode.ID);
                if (children != null)
                {
                    var list = children.ToList();
                    for (var i = 0; i < list.Count; i++)
                    {
                        var child = list[i];
                        if (child.IS_ACTIVE != false)
                        {
                            return true;
                        }
                        result = IncludesActiveElement(child);
                    }
                }
                else
                {
                    return false;
                }
                return result;
            };

            if (valueToBind == false)
            {
                // Không có phòng ban con nào ở trạng thái đang áp dụng

                foreach (var id in ids)
                {

                    var node = _dbContext.HuOrganizations.AsNoTracking().Single(x => x.ID == id);
                    if (IncludesActiveElement(node))
                    {
                        return new FormatedResponse() { MessageCode = CommonMessageCode.CURRENT_ORG_UNIT_INCLUDE_ACTIVE_ELEMENT_AND_CANNOT_BE_INACTIVE, ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400 };
                    }
                    var checkExists = _dbContext.HuPositions.Where(p => p.ORG_ID == id && p.MASTER != null).Count();
                    var obj = new
                    {
                        P_ORGID = id,
                        P_ISDISSOLVE = 1,
                        P_IS_ADMIN = _dbContext.IsAdmin == true ? 1 : 0,
                    };
                    // Hướng tới multi-db format, hạn chế dùng PKG
                    var data = QueryData.ExecuteStoreToTable(Procedures.PKG_CHECK_ACTIVE_ORG, obj, true);
                    if (Convert.ToDouble(data.Tables[0].Rows[0][0]) != 0)
                    {
                        return new FormatedResponse() { MessageCode = CommonMessageCode.DATA_HAS_USED, ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode500 };
                    }

                }
            }
            var response = await _genericRepository.ToggleActiveIds(_uow, ids, valueToBind, sid);
            return response;
        }

        public async Task<bool> Dissolution(HU_ORGANIZATION obj)
        {
            try
            {
                //var _checkPos = _dbContext.HuPositions.Where(p => p.ORG_ID == obj.ID).DefaultIfEmpty();//ds vtri cua org
                var _checkPos = await (from p in _dbContext.HuPositions where p.ORG_ID == obj.ID select p).ToListAsync();
                var _checkOrg = await (from p in _dbContext.HuOrganizations where p.ID == obj.ID select p).FirstOrDefaultAsync();
                //_checkOrg!.IS_ACTIVE = null;
                //ktra vi tri co nv dang o tt lam viec
                if (_checkPos != null && _checkPos.Count != 0)
                {
                    foreach(var i in _checkPos)
                    {
                        var _checkEmp = (from p in _dbContext.HuEmployees where p.ID == i.MASTER select p).FirstOrDefault();//nv o vi tri
                        if (_checkEmp != null && _checkEmp!.WORK_STATUS_ID != OtherConfig.EMP_STATUS_TERMINATE)//kt nv tt nghi viec
                        {
                            if (_checkOrg != null)
                            {
                                _checkOrg!.IS_ACTIVE = true;
                                _checkOrg.DISSOLVE_DATE = null;
                            }
                        }
                        else
                        {
                            _checkOrg!.IS_ACTIVE = false;
                        }
                    }
                }
                else
                {
                    if (_checkOrg != null)
                    {
                        _checkOrg.IS_ACTIVE = false;
                    }
                }
                 _dbContext.HuOrganizations.Update(_checkOrg!);
                var re = _dbContext.SaveChanges();
                return true;

            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public void ScanDissolveOrg()
        {
            var scanList = _dbContext.HuOrganizations.Where(x=>x.DISSOLVE_DATE!.Value.Date == DateTime.Now.Date).ToList();
            scanList.ForEach(async obj =>
            {
                await Dissolution(obj);
            });

        }

        public virtual async Task<FormatedResponse> ToggleActiveIds2(GenericUnitOfWork _uow, List<long> ids, bool valueToBind, string sid)
        {
            var response = await _genericRepository.ToggleActiveIds(_uow, ids, valueToBind, sid);

            var query = _dbContext.HuOrganizations.Where(x => ids.Contains(x.ID));

            foreach (var item in query)
            {
                item.STATUS = valueToBind;
            }

            _dbContext.SaveChanges();

            return response;
        }

        public async Task<FormatedResponse> Update2(HuOrganizationDTO dto)
        {
            var record = await _dbContext.HuOrganizations.FirstOrDefaultAsync(x => x.ID == dto.Id);

            record!.CODE = dto.Code;
            record.NAME = dto.Name;
            record.NOTE = dto.Note;

            _dbContext.SaveChanges();


            return new FormatedResponse()
            {
                InnerBody = dto,
                MessageCode = CommonMessageCode.UPDATE_SUCCESS,
                StatusCode = EnumStatusCode.StatusCode200
            };
        }
    }
}