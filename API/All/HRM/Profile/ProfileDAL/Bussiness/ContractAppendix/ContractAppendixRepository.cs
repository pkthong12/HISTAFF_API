using Common.Paging;
using ProfileDAL.ViewModels;
using Common.Repositories;
using Common.Extensions;
using Common.EPPlus;
using API.All.DbContexts;
using API.Entities;
using CORE.GenericUOW;
using CORE.DTO;
using CORE.Enum;
using CORE.StaticConstant;
using API.DTO;
using API;
using CORE.Services.File;

namespace ProfileDAL.Repositories
{
    public class ContractAppendixRepository : RepositoryBase<HU_FILECONTRACT>, IContractAppendixRepository
    {
        private ProfileDbContext _appContext => (ProfileDbContext)_context;
        private readonly GenericReducer<HU_FILECONTRACT, ContractAppendixDTO> genericReducer;
        public ContractAppendixRepository(ProfileDbContext context) : base(context)
        {
            genericReducer = new();
        }
        public async Task<GenericPhaseTwoListResponse<ContractAppendixDTO>> TwoPhaseQueryList(GenericQueryListDTO<ContractAppendixDTO> request)
        {
            var queryable = from p in _appContext.HuFileContracts
                            from c in _appContext.Contracts.Where(f => p.ID_CONTRACT == f.ID).DefaultIfEmpty()
                            join e in _appContext.Employees on p.EMPLOYEE_ID equals e.ID
                            join o in _appContext.Organizations on p.ORG_ID equals o.ID
                            from t in _appContext.Positions.Where(f => p.POSITION_ID == f.ID).DefaultIfEmpty()
                            from f in _appContext.OtherLists.Where(c => c.ID == p.STATUS_ID).DefaultIfEmpty()
                            join l in _appContext.ContractTypes on p.APPEND_TYPEID equals l.ID

                            orderby p.STATUS_ID, p.START_DATE descending
                            select new ContractAppendixDTO
                            {
                                Id = p.ID,
                                EmployeeId = p.EMPLOYEE_ID,
                                EmployeeCode = e.CODE,
                                EmployeeName = e.Profile!.FULL_NAME,
                                OrgName = o.NAME,
                                OrgId = e.ORG_ID,
                                StartDate = p.START_DATE,
                                ExpireDate = p.EXPIRE_DATE,
                                ContractNo = c.CONTRACT_NO,
                                SignerName = p.SIGNER_NAME,
                                SignerPosition = p.SIGNER_POSITION,
                                SignDate = p.SIGN_DATE,
                                StatusName = f.NAME,
                                WorkStatusId = e.WORK_STATUS_ID,
                                StatusId = p.STATUS_ID,
                                ContractTypeName = l.NAME,
                                ContractAppendixNo = p.CONTRACT_NO,
                                PositionName = t.NAME,
                                InformationEdit = p.INFORMATION_EDIT
                            };

            var phase2 = await genericReducer.SecondPhaseReduce(queryable, request);
            return phase2;
        }
        public async Task<GenericPhaseTwoListResponse<ContractAppendixDTO>> SinglePhaseQueryList(GenericQueryListDTO<ContractAppendixDTO> request)
        {
            var queryable = from p in _appContext.HuFileContracts
                            from c in _appContext.Contracts.Where(f => p.ID_CONTRACT == f.ID).DefaultIfEmpty()
                            join e in _appContext.Employees on p.EMPLOYEE_ID equals e.ID
                            join o in _appContext.Organizations on p.ORG_ID equals o.ID
                            from t in _appContext.Positions.Where(f => p.POSITION_ID == f.ID).DefaultIfEmpty()
                            from j in _appContext.HUJobs.Where(x => x.ID == t.JOB_ID).DefaultIfEmpty()
                            from f in _appContext.OtherLists.Where(c => c.ID == p.STATUS_ID).DefaultIfEmpty()
                            from l in _appContext.ContractTypes.Where(x => p.APPEND_TYPEID == x.ID).DefaultIfEmpty()

                            orderby p.STATUS_ID, p.START_DATE descending
                            select new ContractAppendixDTO
                            {
                                Id = p.ID,
                                EmployeeId = p.EMPLOYEE_ID,
                                EmployeeCode = e.CODE,
                                EmployeeName = e.Profile!.FULL_NAME,
                                OrgName = o.NAME,
                                OrgId = e.ORG_ID,
                                StartDate = p.START_DATE,
                                ExpireDate = p.EXPIRE_DATE,
                                ContractNo = c.CONTRACT_NO,
                                SignerName = p.SIGNER_NAME,
                                SignerPosition = p.SIGNER_POSITION,
                                SignDate = p.SIGN_DATE,
                                StatusName = f.NAME,
                                WorkStatusId = e.WORK_STATUS_ID,
                                StatusId = p.STATUS_ID,
                                ContractTypeName = l.NAME,
                                ContractAppendixNo = p.CONTRACT_NO,
                                PositionName = t.NAME,
                                JobOrderNum = (int)(j.ORDERNUM ?? 99),
                                InformationEdit = p.INFORMATION_EDIT
                            };
            var singlePhaseResult = await genericReducer.SinglePhaseReduce(queryable, request);
            return singlePhaseResult;
        }
        /// <summary>
        /// CMS Get All Data
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<PagedResult<ContractAppendixDTO>> GetAll(ContractAppendixDTO param)
        {
            var queryable = from p in _appContext.HuFileContracts
                            join e in _appContext.Employees on p.EMPLOYEE_ID equals e.ID
                            join o in _appContext.Organizations on e.ORG_ID equals o.ID
                            from f in _appContext.OtherListFixs.Where(c => c.ID == p.STATUS_ID && c.TYPE == SystemConfig.STATUS_APPROVE).DefaultIfEmpty()
                            join l in _appContext.ContractTypes on p.APPEND_TYPEID equals l.ID

                            orderby p.STATUS_ID, p.START_DATE descending
                            select new ContractAppendixDTO
                            {
                                Id = p.ID,
                                EmployeeId = p.EMPLOYEE_ID,
                                EmployeeCode = e.CODE,
                                EmployeeName = e.Profile.FULL_NAME,
                                OrgName = o.NAME,
                                OrgId = e.ORG_ID,
                                StartDate = p.START_DATE,
                                ExpireDate = p.EXPIRE_DATE,
                                ContractNo = p.CONTRACT_NO,
                                SignerName = p.SIGNER_NAME,
                                SignerPosition = p.SIGNER_POSITION,
                                SignDate = p.SIGN_DATE,
                                StatusName = f.NAME,
                                WorkStatusId = e.WORK_STATUS_ID,
                                StatusId = p.STATUS_ID,
                                ContractTypeName = l.NAME,
                                InformationEdit = p.INFORMATION_EDIT
                            };
            var orgIds = await QueryData.ExecuteList(Procedures.PKG_COMMON_LIST_ORG,
                    new
                    {
                        P_IS_ADMIN = _appContext.IsAdmin == true ? 1 : 0,

                        P_ORG_ID = param.OrgId,
                        P_CURENT_USER_ID = _appContext.CurrentUserId,
                        P_CUR = QueryData.OUT_CURSOR
                    }, false);


            List<long?> ids = orgIds.Select(c => (long?)((dynamic)c).ID).ToList();
            if (param.OrgId != null)
            {
                ids.Add(param.OrgId);
            }
            queryable = queryable.Where(p => ids.Contains(p.OrgId));

            if (!string.IsNullOrWhiteSpace(param.EmployeeCode))
            {
                queryable = queryable.Where(p => p.EmployeeCode.ToUpper().Contains(param.EmployeeCode.ToUpper()));
            }
            if (!string.IsNullOrWhiteSpace(param.EmployeeName))
            {
                queryable = queryable.Where(p => p.EmployeeName.ToUpper().Contains(param.EmployeeName.ToUpper()));
            }
            if (!string.IsNullOrWhiteSpace(param.OrgName))
            {
                queryable = queryable.Where(p => p.OrgName.ToUpper().Contains(param.OrgName.ToUpper()));
            }
            if (!string.IsNullOrWhiteSpace(param.ContractNo))
            {
                queryable = queryable.Where(p => p.ContractNo.ToUpper().Contains(param.ContractNo.ToUpper()));
            }
            if (!string.IsNullOrWhiteSpace(param.StatusName))
            {
                queryable = queryable.Where(p => p.StatusName.ToUpper().Contains(param.StatusName.ToUpper()));
            }
            if (!string.IsNullOrWhiteSpace(param.ContractTypeName))
            {
                queryable = queryable.Where(p => p.ContractTypeName.ToUpper().Contains(param.ContractTypeName.ToUpper()));
            }

            if (param.StartDate != null)
            {
                queryable = queryable.Where(p => p.StartDate == param.StartDate);
            }
            if (param.ExpireDate != null)
            {
                queryable = queryable.Where(p => p.ExpireDate == param.ExpireDate);
            }
            if (param.WorkStatusId == null || param.WorkStatusId == 0)
            {
                queryable = queryable.Where(p => p.WorkStatusId != OtherConfig.EMP_STATUS_TERMINATE);
            }
            return await PagingList(queryable, param);
        }



        /// <summary>
        /// CMS Get Detail
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>        
        public async Task<ResultWithError> GetById(long id)
        {
            try
            {
                var n = await (from p in _appContext.HuFileContracts
                               from c in _appContext.Contracts.Where(f => f.ID == p.ID_CONTRACT).DefaultIfEmpty()
                               join e in _appContext.Employees on p.EMPLOYEE_ID equals e.ID
                               join o in _appContext.Organizations on e.ORG_ID equals o.ID
                               join o2 in _appContext.Organizations on o.PARENT_ID equals o2.ID into tmp1
                               from o3 in tmp1.DefaultIfEmpty()
                               join t in _appContext.Positions on e.POSITION_ID equals t.ID into tmp2
                               from t1 in tmp2.DefaultIfEmpty()
                               join w in _appContext.Workings on p.WORKING_ID equals w.ID into tmp5
                               from w1 in tmp5.DefaultIfEmpty()
                               from wo in _appContext.Organizations.Where(c => c.ID == w1.ORG_ID).DefaultIfEmpty()
                               join ct in _appContext.ContractTypes on p.APPEND_TYPEID equals ct.ID into tmp6
                               from ct1 in tmp6.DefaultIfEmpty()
                               from tl in _appContext.SalaryTypes.Where(c => c.ID == w1.SALARY_TYPE_ID).DefaultIfEmpty()
                               from sc in _appContext.SalaryScales.Where(c => c.ID == w1.SALARY_SCALE_ID).DefaultIfEmpty()
                               from ra in _appContext.SalaryRanks.Where(c => c.ID == w1.SALARY_RANK_ID).DefaultIfEmpty()
                               from sl in _appContext.SalaryLevels.Where(c => c.ID == w1.SALARY_LEVEL_ID).DefaultIfEmpty()
                               from tax in _appContext.OtherLists.Where(c => c.ID == w1.TAXTABLE_ID).DefaultIfEmpty()
                               from scdcv in _appContext.SalaryScales.Where(c => c.ID == w1.SALARY_SCALE_DCV_ID).DefaultIfEmpty()
                               from radcv in _appContext.SalaryRanks.Where(c => c.ID == w1.SALARY_RANK_DCV_ID).DefaultIfEmpty()
                               from sldcv in _appContext.SalaryLevels.Where(c => c.ID == w1.SALARY_LEVEL_DCV_ID).DefaultIfEmpty()
                               from com in _appContext.CompanyInfos.Where(c => c.ID == wo.COMPANY_ID).DefaultIfEmpty()
                               from re in _appContext.OtherLists.Where(x => x.ID == com.REGION_ID).DefaultIfEmpty()
                               where p.ID == id
                               select new
                               {
                                   Id = p.ID,
                                   EmployeeId = p.EMPLOYEE_ID,
                                   EmployeeName = e.Profile!.FULL_NAME,
                                   EmployeeCode = e.CODE,
                                   PositionName = t1.NAME,
                                   OrgId = e.ORG_ID,
                                   OrgName = o.NAME,
                                   OrgParentName = o3.NAME,
                                   StartDate = p.START_DATE,
                                   ExpireDate = p.EXPIRE_DATE,
                                   ContractNo = p.CONTRACT_NO,
                                   AppendTypeid = p.APPEND_TYPEID,
                                   IdContract = p.ID_CONTRACT,
                                   StatusId = p.STATUS_ID,
                                   SignId = p.SIGN_ID,
                                   SignerName = p.SIGNER_NAME,
                                   SignerPosition = p.SIGNER_POSITION,
                                   SignDate = p.SIGN_DATE,
                                   WorkingId = p.WORKING_ID,
                                   WorkingNo = w1.DECISION_NO,
                                   //SalaryScaleName = s2.NAME,
                                   //SalaryRankName = r2.NAME,
                                   //SalaryLevelName = l2.NAME,
                                   //SalaryTypeName = tb.NAME,
                                   SalBasic = w1.SAL_BASIC,
                                   SalPercent = w1.SAL_PERCENT,
                                   salTotal = w1.SAL_TOTAL,
                                   Note = p.NOTE,
                                   SalaryType = tl.NAME,
                                   SalaryScaleName = sc.NAME,
                                   SalaryRankName = ra.NAME,
                                   SalaryLevelName = sl.NAME,
                                   ShortTempSalary = w1.SHORT_TEMP_SALARY,
                                   TaxtableName = tax.NAME,
                                   Coefficient = w1.COEFFICIENT,
                                   CoefficientDcv = w1.COEFFICIENT_DCV,
                                   salaryScaleDcvName = scdcv.NAME,
                                   salaryRankDcvName = radcv.NAME,
                                   salaryLevelDcvName = sldcv.NAME,
                                   regionName = re.NAME,
                                   WhenContractNoIsEmpty = (p.CONTRACT_NO == "" || p.CONTRACT_NO == null) ? true : false,
                                   UploadFile = p.UPLOAD_FILE,
                                   InformationEdit = p.INFORMATION_EDIT
                               }).FirstOrDefaultAsync();
                return new ResultWithError(n);
            }
            catch (Exception ex)
            {
                return new ResultWithError(ex.Message);
            }
        }
        /// <summary>
        /// Create Data
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<FormatedResponse> CreateAsync(ContractAppendixInputDTO param)
        {
            try
            {
                var e = _appContext.Employees.Where(x => x.ID == param.EmployeeId).FirstOrDefault();
                if (e == null)
                {
                    //return new ResultWithError(Message.EMP_NOT_EXIST);
                    return new FormatedResponse
                    {
                        MessageCode = Message.EMP_NOT_EXIST,
                        InnerBody = param,
                        StatusCode = EnumStatusCode.StatusCode400
                    };
                }
                // Kiểm tra xem ngày bắt đầu HĐ mới có lớn hơn ngày kết thúc của HĐ gần nhất không
                var r = await _appContext.HuFileContracts.Where(x => x.EMPLOYEE_ID == param.EmployeeId && x.START_DATE >= param.StartDate && x.STATUS_ID == OtherConfig.STATUS_APPROVE).CountAsync();
                if (r > 0)
                {
                    return new FormatedResponse()
                    {
                        ErrorType = EnumErrorType.CATCHABLE,
                        MessageCode = "CONTRACT_STARTDATE_NOT_LESS_CURRENT",
                        InnerBody = param,
                        StatusCode = EnumStatusCode.StatusCode400
                    };
                }
                // Gencode
                //var ContractNo = "";
                var data = Map(param, new HU_FILECONTRACT());
                //data.CONTRACT_NO = ContractNo;
                var workingMax = await GetLastWorking(param.EmployeeId, null);
                var rs = (WorkingInputDTO)workingMax.Data;
                await _appContext.Database.BeginTransactionAsync();
                List<UploadFileResponse> uploadFiles = new();

                // First of all we need to upload all the attachments
                //if (param.UploadFileBuffer != null)
                //{
                //    string location = Path.Combine(_env.ContentRootPath, _appSettings.StaticFolders.Root, _appSettings.StaticFolders.Attachments);
                //    var uploadFileResponse = await _fileService.UploadFile(new UploadRequest() { 
                //        ClientFileName = param.UploadFileBuffer.ClientFileName,
                //        ClientFileType = param.UploadFileBuffer.ClientFileType,
                //        ClientFileData = param.UploadFileBuffer.ClientFileData
                //    }, location, "ContractAppendix");

                //    // Assign saved paths
                //    var property = typeof(ContractAppendixInputDTO).GetProperty("UploadFile");

                //    if (property != null)
                //    {
                //        property?.SetValue(param, uploadFileResponse.SavedAs);
                //        uploadFiles.Add(uploadFileResponse);
                //    } else
                //    {
                //        return new ResultWithError("STARTDATE_NOT_LESS_CURRENT");
                //    }

                //}
                if (rs != null)
                {
                    data.ORG_ID = rs.OrgId;
                    data.POSITION_ID = rs.PositionId;
                }
                else
                {
                    data.ORG_ID = e.ORG_ID;
                    data.POSITION_ID = e.POSITION_ID;
                }
                var result = await _appContext.HuFileContracts.AddAsync(data);
                await _appContext.SaveChangesAsync();

                // update lai contract id cho nhan vien
                //if (data.STATUS_ID == OtherConfig.STATUS_APPROVE)
                //{
                //    var e = _appContext.Employees.Where(x => x.ID == param.EmployeeId ).FirstOrDefault();
                //    if (e == null)
                //    {
                //        return new FormatedResponse{  MessageCode =Message.EMP_NOT_EXIST,
                //                                    InnerBody = param,
                //                                    StatusCode = EnumStatusCode.StatusCode400 };
                //    }
                //    //e.CONTRACT_ID = data.ID;
                //    //e.CONTRACT_TYPE_ID = data.APPEND_TYPEID;
                //    //e.CONTRACT_EXPIRED = param.ExpireDate;
                //    //_appContext.Employees.Update(e);
                //    await _appContext.SaveChangesAsync();
                //    //await QueryData.Execute(Procedures.PKG_ENTITLEMENT_CREATED_BY_CONTRACT, new { P_START_DATE = param.StartDate, P_EMP_ID = param.EmployeeId, P_CONTRACT_TYPE = param.AppendTypeid }, true);
                //}
                _appContext.Database.CommitTransaction();

                return new FormatedResponse
                {
                    MessageCode = CommonMessageCode.CREATE_SUCCESS,
                    InnerBody = param,
                    StatusCode = EnumStatusCode.StatusCode200
                };
            }
            catch (Exception ex)
            {
                _appContext.Database.RollbackTransaction();
                return new FormatedResponse { MessageCode = ex.Message, InnerBody = param, StatusCode = EnumStatusCode.StatusCode400 };
            }
        }

        public async Task<ResultWithError> GetLastWorking(long? empId, DateTime? date)
        {
            try
            {
                if (date == null)
                {
                    date = DateTime.Now;
                }
                var r = await (from p in _appContext.Workings
                               where p.EMPLOYEE_ID == empId && p.EFFECT_DATE <= date && (p.IS_WAGE == 0 || p.IS_WAGE == null) && p.STATUS_ID == 994
                               orderby p.EFFECT_DATE descending
                               select new WorkingInputDTO
                               {
                                   Id = p.ID,
                                   OrgId = p.ORG_ID,
                                   PositionId = p.POSITION_ID
                               }).FirstOrDefaultAsync();
                return new ResultWithError(r);
            }
            catch (Exception ex)
            {
                return new ResultWithError(ex.Message);
            }
        }

        /// <summary>
        /// CMS Edit Data
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<FormatedResponse> UpdateAsync(ContractAppendixInputDTO param)
        {
            try
            {
                var e = _appContext.Employees.Where(x => x.ID == param.EmployeeId).FirstOrDefault();
                if (e == null)
                {
                    //return new ResultWithError(Message.EMP_NOT_EXIST);
                    return new FormatedResponse
                    {
                        MessageCode = Message.EMP_NOT_EXIST,
                        InnerBody = param,
                        StatusCode = EnumStatusCode.StatusCode400
                    };
                }
                // Kiểm tra xem ngày bắt đầu HĐ mới có lớn hơn ngày kết thúc của HĐ gần nhất không
                var c = await _appContext.HuFileContracts.Where(x => x.EMPLOYEE_ID == param.EmployeeId && x.ID != param.Id && x.START_DATE >= param.StartDate && x.STATUS_ID == OtherConfig.STATUS_APPROVE).CountAsync();
                if (c > 0)
                {
                    return new FormatedResponse()
                    {
                        ErrorType = EnumErrorType.CATCHABLE,
                        MessageCode = "CONTRACT_STARTDATE_NOT_LESS_CURRENT",
                        InnerBody = param,
                        StatusCode = EnumStatusCode.StatusCode400
                    };
                }

                var r = _appContext.HuFileContracts.Where(x => x.ID == param.Id).FirstOrDefault();
                if (r == null)
                {
                    return new FormatedResponse
                    {
                        MessageCode = Consts.ID_NOT_FOUND,
                        InnerBody = param,
                        StatusCode = EnumStatusCode.StatusCode400
                    };
                }
                // Kiểm tra xem nếu dữ liệu đang sửa ở trạng thái phê duyệt thì không cho sửa
                if (r.STATUS_ID == OtherConfig.STATUS_APPROVE)
                {
                    return new FormatedResponse
                    {
                        MessageCode = Message.RECORD_IS_APPROVED,
                        InnerBody = param,
                        StatusCode = EnumStatusCode.StatusCode400
                    };
                }
                List<UploadFileResponse> uploadFiles = new();

                // First of all we need to upload all the attachments
                //if (param.UploadFileBuffer != null)
                //{
                //    string location = Path.Combine(_env.ContentRootPath, _appSettings.StaticFolders.Root, _appSettings.StaticFolders.Attachments);
                //    var uploadFileResponse = await _fileService.UploadFile(new UploadRequest() { 
                //        ClientFileName = param.UploadFileBuffer.ClientFileName,
                //        ClientFileType = param.UploadFileBuffer.ClientFileType,
                //        ClientFileData = param.UploadFileBuffer.ClientFileData
                //    }, location, "ContractAppendix");

                //    // Assign saved paths
                //    var property = typeof(ContractAppendixInputDTO).GetProperty("UploadFile");

                //    if (property != null)
                //    {
                //        property?.SetValue(param, uploadFileResponse.SavedAs);
                //        uploadFiles.Add(uploadFileResponse);
                //    } else
                //    {
                //        return new ResultWithError("STARTDATE_NOT_LESS_CURRENT");
                //    }

                //}

                var data = Map(param, r);
                var workingMax = await GetLastWorking(param.EmployeeId, null);
                var rs = (WorkingInputDTO)workingMax.Data;
                if (rs != null)
                {
                    data.ORG_ID = rs.OrgId;
                    data.POSITION_ID = rs.PositionId;
                }
                else
                {
                    data.ORG_ID = e.ORG_ID;
                    data.POSITION_ID = e.POSITION_ID;
                }
                // await _appContext.Database.BeginTransactionAsync();
                var result = _appContext.HuFileContracts.Update(data);
                // Nếu là trạng thái đã phê duyệt thì cập nhật thông tin mới nhất vào Employee
                //if (data.STATUS_ID == OtherConfig.STATUS_APPROVE)
                //{
                //    var e = _appContext.Employees.Where(x => x.ID == param.EmployeeId ).FirstOrDefault();
                //    if (e == null)
                //    {
                //        return new FormatedResponse{  MessageCode =Message.EMP_NOT_EXIST,
                //                                    InnerBody = param,
                //                                    StatusCode = EnumStatusCode.StatusCode400 };
                //    }
                //    //e.CONTRACT_ID = param.Id;
                //    //e.CONTRACT_TYPE_ID = data.APPEND_TYPEID;
                //    //_appContext.Employees.Update(e);
                //    //await QueryData.Execute(Procedures.PKG_ENTITLEMENT_CREATED_BY_CONTRACT, new { P_START_DATE = param.StartDate, P_EMP_ID = param.EmployeeId, P_CONTRACT_TYPE = param.AppendTypeid }, true);
                //}
                await _appContext.SaveChangesAsync();
                return new FormatedResponse
                {
                    MessageCode = CommonMessageCode.UPDATE_SUCCESS,
                    InnerBody = param,
                    StatusCode = EnumStatusCode.StatusCode200
                };
            }
            catch (Exception ex)
            {
                return new FormatedResponse
                {
                    MessageCode = ex.Message,
                    InnerBody = param,
                    StatusCode = EnumStatusCode.StatusCode400
                };
            }
        }
        /// <summary>
        /// CMS Change Status Data
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<FormatedResponse> RemoveAsync(List<long> param)
        {
            try
            {
                await _appContext.Database.BeginTransactionAsync();
                foreach (var item in param)
                {
                    var r = _appContext.HuFileContracts.Where(x => x.ID == item).FirstOrDefault();
                    if (r == null)
                    {
                        return new FormatedResponse
                        {
                            MessageCode = Consts.ID_NOT_FOUND,
                            InnerBody = param,
                            StatusCode = EnumStatusCode.StatusCode400
                        };
                    }
                    // Kiểm tra xem nếu dữ liệu đang sửa ở trạng thái phê duyệt thì không cho xóa
                    if (r.STATUS_ID == OtherConfig.STATUS_APPROVE)
                    {
                        return new FormatedResponse
                        {
                            ErrorType = EnumErrorType.CATCHABLE,
                            MessageCode = CommonMessageCode.DO_NOT_DELETE_DATA_IS_ACTIVE,
                            StatusCode = EnumStatusCode.StatusCode400
                        };
                    }
                    _appContext.HuFileContracts.Remove(r);
                }
                await _appContext.SaveChangesAsync();
                _appContext.Database.CommitTransaction();
                return new FormatedResponse
                {
                    MessageCode = CommonMessageCode.DELETE_SUCCESS,
                    InnerBody = param,
                    StatusCode = EnumStatusCode.StatusCode200
                };
            }
            catch (Exception ex)
            {
                _appContext.Database.RollbackTransaction();
                return new FormatedResponse
                {
                    MessageCode = ex.Message,
                    InnerBody = param,
                    StatusCode = EnumStatusCode.StatusCode400
                };
            }
        }
        public async Task<ResultWithError> OpenStatus(long id)
        {
            try
            {
                var r = _appContext.HuFileContracts.Where(x => x.ID == id).FirstOrDefault();
                if (r == null)
                {
                    return new ResultWithError(404);
                }
                // Kiểm tra xem HĐ mở chờ phê duyệt có phải là hđ cuối cùng không
                var c = await _appContext.HuFileContracts.Where(x => x.EMPLOYEE_ID == r.EMPLOYEE_ID && x.ID != id && x.EXPIRE_DATE > r.START_DATE && x.STATUS_ID == OtherConfig.STATUS_APPROVE).CountAsync();
                if (c > 0)
                {
                    return new ResultWithError("CONTRACT_NOT_LASTEST");
                }
                // Kiểm tra xem nếu dữ liệu đang sửa ở trạng thái chờ phê duyệt thì không cho mở chờ phê duyệt
                if (r.STATUS_ID == OtherConfig.STATUS_WAITING)
                {
                    return new ResultWithError("RECORD_IS_WAITED");
                }
                r.STATUS_ID = OtherConfig.STATUS_WAITING;
                var result = _appContext.HuFileContracts.Update(r);
                await _appContext.SaveChangesAsync();
                return new ResultWithError(200);
            }
            catch (Exception ex)
            {
                return new ResultWithError(ex.Message);
            }
        }

        public async Task<FormatedResponse> GetContractAppendixByEmpProfile(long EmployeeId)
        {
            var n = await (from p in _appContext.HuFileContracts
                           join e in _appContext.Employees on p.EMPLOYEE_ID equals e.ID
                           join w in _appContext.Contracts on p.ID_CONTRACT equals w.ID into tmp1
                           from w1 in tmp1.DefaultIfEmpty()
                           join l in _appContext.ContractTypes on w1.CONTRACT_TYPE_ID equals l.ID into tmp2
                           from ct1 in tmp2.DefaultIfEmpty()
                           join ct in _appContext.ContractTypes on p.APPEND_TYPEID equals ct.ID into tmp3
                           from ct2 in tmp3.DefaultIfEmpty()
                           join o in _appContext.OtherLists on p.STATUS_ID equals o.ID
                           where p.EMPLOYEE_ID == EmployeeId && p.STATUS_ID == OtherConfig.STATUS_APPROVE
                           select new
                           {
                               Id = p.ID,
                               StartDate = p.START_DATE,
                               ContractId = p.ID_CONTRACT,
                               ExpireDate = p.EXPIRE_DATE,
                               ContractAppendixNo = p.CONTRACT_NO,
                               ContractName = ct1.NAME,
                               StatusName = o.NAME,
                               SignId = p.SIGN_ID,
                               SignerName = p.SIGNER_NAME,
                               SignPos = p.SIGNER_POSITION,
                               SignDate = p.SIGN_DATE,
                               ContractType = ct2.NAME,
                               Note = p.NOTE
                           }).ToListAsync();
            return new() { InnerBody = n };

        }
        public async Task<ResultWithError> GetAllowanceWageById(long? id)
        {
            try
            {
                var r = await (from p in _appContext.WorkingAllowances
                               from n in _appContext.Allowances.Where(x => x.ID == p.ALLOWANCE_ID).DefaultIfEmpty()
                               where p.WORKING_ID == id
                               select new WorkingAllowInputDTO
                               {
                                   Id = p.ID,
                                   AllowanceId = p.ALLOWANCE_ID,
                                   AllowanceName = n.NAME,
                                   Amount = p.AMOUNT,
                                   Effectdate = p.EFFECT_DATE,
                                   ExpireDate = p.EXPIRE_DATE,
                                   Coefficient = p.COEFFICIENT
                               }).ToListAsync();
                return new ResultWithError(r);
            }
            catch (Exception ex)
            {
                return new ResultWithError(ex.Message);
            }
        }
        public async Task<ResultWithError> GetExpiteDateByContract(long? contractId)
        {
            try
            {
                var r = await (from p in _appContext.Contracts
                               join l in _appContext.ContractTypes on p.CONTRACT_TYPE_ID equals l.ID
                               join o in _appContext.ContractTypeSyses on l.TYPE_ID equals o.ID
                               where p.ID == contractId
                               select new
                               {
                                   Id = p.ID,
                                   StartDate = p.START_DATE,
                                   ExpireDate = p.EXPIRE_DATE,
                                   EmployeeId = p.EMPLOYEE_ID,
                                   ContractTypeCode = o.CODE
                               }).FirstOrDefaultAsync();
                return new ResultWithError(r);
            }
            catch (Exception ex)
            {
                return new ResultWithError(ex.Message);
            }
        }
        public async Task<ResultWithError> GetWageByContract(long? contractId)
        {
            try
            {
                var infoContract = _appContext.Contracts.Where(x => x.ID == contractId).FirstOrDefault();
                //if (infoContract != null)
                //{
                DateTime? _startDate = infoContract.START_DATE;
                long? _empID = infoContract.EMPLOYEE_ID;

                var r = await (from p in _appContext.Workings
                               from e in _appContext.Employees.Where(c => c.ID == p.EMPLOYEE_ID)
                               from t in _appContext.Positions.Where(c => c.ID == p.POSITION_ID)
                               from o in _appContext.Organizations.Where(c => c.ID == p.ORG_ID)
                               from f in _appContext.OtherLists.Where(c => c.ID == p.STATUS_ID).DefaultIfEmpty()
                               from l in _appContext.OtherLists.Where(c => c.ID == p.TYPE_ID)
                               from s in _appContext.Employees.Where(c => c.ID == p.SIGN_ID).DefaultIfEmpty()
                               from tl in _appContext.SalaryTypes.Where(c => c.ID == p.SALARY_TYPE_ID).DefaultIfEmpty()
                               from sc in _appContext.SalaryScales.Where(c => c.ID == p.SALARY_SCALE_ID).DefaultIfEmpty()
                               from ra in _appContext.SalaryRanks.Where(c => c.ID == p.SALARY_RANK_ID).DefaultIfEmpty()
                               from sl in _appContext.SalaryLevels.Where(c => c.ID == p.SALARY_LEVEL_ID).DefaultIfEmpty()
                               from tax in _appContext.OtherLists.Where(c => c.ID == p.TAXTABLE_ID).DefaultIfEmpty()
                               from tldcv in _appContext.SalaryTypes.Where(c => c.ID == p.SALARY_TYPE_ID).DefaultIfEmpty()
                               from scdcv in _appContext.SalaryScales.Where(c => c.ID == p.SALARY_SCALE_DCV_ID).DefaultIfEmpty()
                               from radcv in _appContext.SalaryRanks.Where(c => c.ID == p.SALARY_RANK_DCV_ID).DefaultIfEmpty()
                               from sldcv in _appContext.SalaryLevels.Where(c => c.ID == p.SALARY_LEVEL_DCV_ID).DefaultIfEmpty()
                               from com in _appContext.CompanyInfos.Where(c => c.ID == o.COMPANY_ID).DefaultIfEmpty()
                               from re in _appContext.OtherLists.Where(x => x.ID == com.REGION_ID).DefaultIfEmpty()
                               where p.EMPLOYEE_ID == _empID && p.EFFECT_DATE <= _startDate && (p.IS_WAGE == -1 || p.IS_WAGE == 1) && p.STATUS_ID == 994
                               orderby p.EFFECT_DATE descending
                               select new
                               {
                                   Id = p.ID,
                                   OrgId = p.ORG_ID,
                                   EmployeeId = e.ID,
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
                                   ExpireUpsalDate = p.EXPIRE_UPSAL_DATE,
                                   DecisionNo = p.DECISION_NO,
                                   StatusName = f.NAME,
                                   TypeName = l.NAME,
                                   SalBasic = p.SAL_BASIC,
                                   SalTotal = p.SAL_TOTAL,
                                   SalPercent = p.SAL_PERCENT,
                                   SalaryType = tl.NAME,
                                   SalaryScaleName = sc.NAME,
                                   SalaryRankName = ra.NAME,
                                   SalaryLevelName = sl.NAME,
                                   ShortTempSalary = p.SHORT_TEMP_SALARY,
                                   TaxTableName = tax.NAME,
                                   Coefficient = p.COEFFICIENT,
                                   CoefficientDcv = p.COEFFICIENT_DCV,
                                   SalaryScaleDcvName = scdcv.NAME,
                                   SalaryRankDcvName = sldcv.NAME,
                                   SalaryLevelDcvName = sldcv.NAME,
                                   RegionName = re.NAME,
                                   EmpWorkStatus = e.WORK_STATUS_ID,
                                   EffectUpsalDate = p.EFFECT_UPSAL_DATE,
                                   ReasonUpsal = p.REASON_UPSAL,
                                   StatusId = p.STATUS_ID
                               }).FirstOrDefaultAsync();
                return new ResultWithError(r);
            }
            catch (Exception ex)
            {
                return new ResultWithError(ex.Message);
            }
        }



    }
}
