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
using System.Text.RegularExpressions;
using API.Main;
using API.DTO;
using API.All.SYSTEM.CoreAPI.Xlsx;
using System.Globalization;
using System.Diagnostics.Contracts;
using System.Drawing;
using DocumentFormat.OpenXml.Office2010.Excel;


namespace ProfileDAL.Repositories
{
    public class ContractRepository : RepositoryBase<HU_CONTRACT>, IContractRepository
    {
        private ProfileDbContext _appContext => (ProfileDbContext)_context;
        private readonly GenericReducer<HU_CONTRACT, ContractDTO> genericReducer;
        private readonly GenericReducer<HU_CONTRACT_IMPORT, HuContractImportDTO> _genericReducerImport;
        private readonly List<string> XLSX_COLUMNS;
        public ContractRepository(ProfileDbContext context) : base(context)
        {
            genericReducer = new();
            _genericReducerImport = new();
            XLSX_COLUMNS = new() { "XLSX_USER_ID", "XLSX_EX_CODE", "XLSX_INSERT_ON", "XLSX_SESSION", "XLSX_FILE_NAME", "XLSX_ROW" };
        }
        public async Task<GenericPhaseTwoListResponse<ContractDTO>> SinglePhaseQueryList(GenericQueryListDTO<ContractDTO> request)
        {
            var queryable = from p in _appContext.Contracts
                            join e in _appContext.Employees on p.EMPLOYEE_ID equals e.ID
                            
                            from ho in _appContext.Organizations.Where(x => x.ID == p.ORG_ID).DefaultIfEmpty()
                            from hp in _appContext.Positions.Where(x => x.ID == p.POSITION_ID).DefaultIfEmpty()
                            
                            from j in _appContext.HUJobs.Where(x => x.ID == hp.JOB_ID).DefaultIfEmpty()
                            from f in _appContext.OtherLists.Where(c => c.ID == p.STATUS_ID).DefaultIfEmpty()
                            join l in _appContext.ContractTypes on p.CONTRACT_TYPE_ID equals l.ID
                            //orderby p.STATUS_ID, p.START_DATE descending
                            orderby j.ORDERNUM ascending
                            select new ContractDTO
                            {
                                Id = p.ID,
                                EmployeeId = p.EMPLOYEE_ID,
                                EmployeeCode = e.CODE,
                                EmployeeName = e.Profile!.FULL_NAME,
                                OrgName = ho.NAME,
                                OrgId = p.ORG_ID,
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
                                PosId = hp.ID,
                                PositionName = hp.NAME,
                                LiquidationDate = p.LIQUIDATION_DATE,
                                LiquidationReason = p.LIQUIDATION_REASON,
                                JobOrderNum = Convert.ToInt32(j.ORDERNUM ?? 999)
                            };
            var singlePhaseResult = await genericReducer.SinglePhaseReduce(queryable, request);
            return singlePhaseResult;
        }
        /// <summary>
        /// CMS Get All Data
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        /// 
        public async Task<GenericPhaseTwoListResponse<HuContractImportDTO>> SinglePhaseQueryListImport(GenericQueryListDTO<HuContractImportDTO> request)
        {
            var joined = from p in _appContext.ContractImports.AsNoTracking()
                         from e in _appContext.Employees.AsNoTracking().Where(e => e.ID == p.EMPLOYEE_ID).DefaultIfEmpty()
                         from cv1 in _appContext.EmployeeCvs.AsNoTracking().Where(cv => e.PROFILE_ID == cv.ID).DefaultIfEmpty()
                         from o in _appContext.Organizations.AsNoTracking().Where(o => o.ID == e.ORG_ID).DefaultIfEmpty()
                         from t in _appContext.Positions.Where(f => p.POSITION_ID == f.ID).DefaultIfEmpty()
                         from s in _appContext.Employees.AsNoTracking().Where(s => s.ID == p.SIGN_ID).DefaultIfEmpty()
                         from cv2 in _appContext.EmployeeCvs.AsNoTracking().Where(cv2 => s.PROFILE_ID == cv2.ID).DefaultIfEmpty()
                         from t2 in _appContext.Positions.Where(f => s.POSITION_ID == f.ID).DefaultIfEmpty()
                         from f in _appContext.OtherLists.Where(c => c.ID == p.STATUS_ID).DefaultIfEmpty()
                         from l in _appContext.ContractTypes.Where(c => c.ID == p.CONTRACT_TYPE_ID).DefaultIfEmpty()
                         orderby p.STATUS_ID, p.START_DATE descending
                         select new HuContractImportDTO
                         {
                             Id = p.XLSX_ROW,
                             XlsxUserId = p.XLSX_USER_ID,
                             XlsxExCode = p.XLSX_EX_CODE,
                             XlsxSession = p.XLSX_SESSION,
                             XlsxInsertOn = p.XLSX_INSERT_ON,
                             XlsxFileName = p.XLSX_FILE_NAME,
                             XlsxRow = p.XLSX_ROW,

                             EmployeeId = p.EMPLOYEE_ID,
                             EmployeeCode = e.CODE,
                             EmployeeName = cv1.FULL_NAME,
                             OrgName = o.NAME,
                             PositionName = t.NAME,
                             OrgId = o.ID,
                             StartDate = p.START_DATE,
                             ExpireDate = p.EXPIRE_DATE,
                             ContractNo = p.CONTRACT_NO,
                             SignerName = cv2.FULL_NAME,
                             SignerPosition = t2.NAME,
                             SignDate = p.SIGN_DATE,
                             StatusName = f.NAME,
                             //StatusId = p.STATUS_ID,
                             ContractTypeName = l.NAME,
                         };
            var singlePhaseResult = await _genericReducerImport.SinglePhaseReduce(joined, request);
            return singlePhaseResult;
        }
        public async Task<PagedResult<ContractDTO>> GetAll(ContractDTO param)
        {
            var queryable = from p in _appContext.Contracts
                            join e in _appContext.Employees on p.EMPLOYEE_ID equals e.ID
                            join o in _appContext.Organizations on e.ORG_ID equals o.ID
                            from f in _appContext.OtherListFixs.Where(c => c.ID == p.STATUS_ID && c.TYPE == SystemConfig.STATUS_APPROVE).DefaultIfEmpty()
                            join l in _appContext.ContractTypes on p.CONTRACT_TYPE_ID equals l.ID

                            orderby p.STATUS_ID, p.START_DATE descending
                            select new ContractDTO
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
                                ContractTypeName = l.NAME
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
                // get workingId
                var workingId = _appContext.Contracts.FirstOrDefault(x => x.ID == id)!.WORKING_ID;

                // get record name is "salary records"
                var record = _appContext.Workings.FirstOrDefault(x => x.ID == workingId);

                record!.EFFECT_DATE = new DateTime(record.EFFECT_DATE!.Value.Year, record.EFFECT_DATE.Value.Month, record.EFFECT_DATE.Value.Day, 0, 0, 0);

                var getAreaId = (from item in _appContext.Workings.Where(x => x.ID == workingId)
                                 from he in _appContext.Employees.Where(x => x.ID == item.EMPLOYEE_ID).DefaultIfEmpty()
                                 from ho in _appContext.Organizations.Where(x => x.ID == he.ORG_ID).DefaultIfEmpty()
                                 from hc in _appContext.CompanyInfos.Where(x => x.ID == ho.COMPANY_ID).DefaultIfEmpty()
                                 select hc.REGION_ID).FirstOrDefault();

                var listInsRegions = (from item in _appContext.InsRegions
                                      where item.AREA_ID == getAreaId
                                            && item.IS_ACTIVE == true
                                      orderby item.EFFECT_DATE descending
                                      select item).ToList();

                var list = listInsRegions.Select(x => new
                {
                    EffectDate = new DateTime(x.EFFECT_DATE!.Value.Year, x.EFFECT_DATE.Value.Month, x.EFFECT_DATE.Value.Day, 0, 0, 0),
                    Money = x.MONEY
                });

                var regionMinimumWage = (from item in list
                                         where item.EffectDate <= record.EFFECT_DATE
                                         select item.Money).FirstOrDefault();

                var n = await (from p in _appContext.Contracts
                               join e in _appContext.Employees on p.EMPLOYEE_ID equals e.ID
                               
                               from ho in _appContext.Organizations.Where(x => x.ID == p.ORG_ID).DefaultIfEmpty()
                               from hp in _appContext.Positions.Where(x => x.ID == p.POSITION_ID).DefaultIfEmpty()

                               from ho2 in _appContext.Organizations.Where(x => x.ID == ho.PARENT_ID).DefaultIfEmpty()
                               join w in _appContext.Workings on p.WORKING_ID equals w.ID into tmp5
                               from w1 in tmp5.DefaultIfEmpty()
                               from wo in _appContext.Organizations.Where(c => c.ID == w1.ORG_ID).DefaultIfEmpty()
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
                                   PositionName = hp.NAME,
                                   OrgId = e.ORG_ID,
                                   OrgName = ho.NAME,
                                   OrgParentName = ho2.NAME,
                                   StartDate = p.START_DATE,
                                   ExpireDate = p.EXPIRE_DATE,
                                   ContractNo = p.CONTRACT_NO,
                                   ContractTypeId = p.CONTRACT_TYPE_ID,
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
                                   salTotal = w1.SAL_TOTAL,
                                   Note = p.NOTE,
                                   SalPercent = w1.SAL_PERCENT,
                                   SalaryType = tl.NAME,
                                   SalaryScaleName = sc.NAME,
                                   SalaryRankName = ra.NAME,
                                   SalaryLevelName = sl.NAME,
                                   ShortTempSalary = w1.SHORT_TEMP_SALARY,
                                   TaxTableName = tax.NAME,
                                   Coefficient = w1.COEFFICIENT,
                                   CoefficientDcv = w1.COEFFICIENT_DCV,
                                   salaryScaleDcvName = scdcv.NAME,
                                   salaryRankDcvName = radcv.NAME,
                                   salaryLevelDcvName = sldcv.NAME,
                                   regionName = re.NAME,
                                   UploadFile = p.UPLOAD_FILE,
                                   RegionMinimumWage = regionMinimumWage,
                                   SalInsu = record.SAL_INSU
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
        public async Task<FormatedResponse> CreateAsync(ContractInputDTO param)
        {
            try
            {
                await _appContext.Database.BeginTransactionAsync();
                var _emp = _appContext.Employees.Where(x => x.ID == param.EmployeeId).FirstOrDefault();
                var eprofile = (from p in _appContext.Employees.Where(x => x.ID == param.EmployeeId) select p.Profile).FirstOrDefault();
                if (_emp == null)
                {
                    //return new ResultWithError(Message.EMP_NOT_EXIST);
                    return new FormatedResponse
                    {
                        MessageCode = Message.EMP_NOT_EXIST,
                        InnerBody = param,
                        StatusCode = EnumStatusCode.StatusCode400
                    };
                }

                // get latest contract
                var latest_contract = _appContext.Contracts.Where(x => x.EMPLOYEE_ID == param.EmployeeId).OrderByDescending(x => x.START_DATE).FirstOrDefault();
                if (latest_contract != null)
                {
                    if (latest_contract.LIQUIDATION_DATE != null)
                    {
                        DateTime DateLiquidation = latest_contract.LIQUIDATION_DATE.Value;
                        DateLiquidation = DateLiquidation.Date.Add(new TimeSpan(0, 0, 0));

                        DateTime START_DATE = param.StartDate;
                        START_DATE = START_DATE.Date.Add(new TimeSpan(0, 0, 0));

                        int checkDate1 = DateTime.Compare(START_DATE, DateLiquidation);

                        // new start date <= liquidate contract
                        if (checkDate1 <= 0)
                        {
                            return new FormatedResponse
                            {
                                ErrorType = EnumErrorType.CATCHABLE,
                                MessageCode = CommonMessageCode.NEW_START_DATE_LIQUIDATE_CONTRACT,
                                StatusCode = EnumStatusCode.StatusCode400
                            };
                        }
                    }
                }

                // Kiểm tra xem ngày bắt đầu HĐ mới có lớn hơn ngày kết thúc của HĐ gần nhất không
                var r = await _appContext.Contracts.Where(x => x.EMPLOYEE_ID == param.EmployeeId && (x.EXPIRE_DATE >= param.StartDate || x.START_DATE > param.StartDate) && x.STATUS_ID == OtherConfig.STATUS_APPROVE).CountAsync();
                if (r > 0 && latest_contract!.LIQUIDATION_DATE == null)
                {
                    return new FormatedResponse
                    {
                        MessageCode = "CONTRACT_STARTDATE_NOT_LESS_CURRENT",
                        InnerBody = param,
                        StatusCode = EnumStatusCode.StatusCode400,
                        ErrorType = EnumErrorType.CATCHABLE,
                    };
                }
                var workingMax = await GetLastWorking(param.EmployeeId, null);
                var rs = (WorkingInputDTO)workingMax.Data;
                // Gencode
                //var ContractNo = "";
                var data = Map(param, new HU_CONTRACT());
                //data.CONTRACT_NO = ContractNo;
                if (rs != null)
                {
                    data.ORG_ID = rs.OrgId;
                    data.POSITION_ID = rs.PositionId;
                }
                else
                {
                    data.ORG_ID = _emp.ORG_ID;
                    data.POSITION_ID = _emp.POSITION_ID;
                }

                var result = await _appContext.Contracts.AddAsync(data);
                await _appContext.SaveChangesAsync();
                param.Id = data.ID;

                // when user choose "Đã phê duyệt"
                var getStatusApproveId = _appContext.OtherLists.FirstOrDefault(x => x.CODE == "DD")?.ID;
                if (data.STATUS_ID == getStatusApproveId && data.START_DATE!.Value.Date <= DateTime.Now.Date) await ApproveContract(param);

                await _appContext.SaveChangesAsync();
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
                return new FormatedResponse
                {
                    MessageCode = ex.Message,
                    InnerBody = param,
                    StatusCode = EnumStatusCode.StatusCode400
                };
            }
        }

        /// <summary>
        /// CMS Edit Data
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<FormatedResponse> UpdateAsync(ContractInputDTO param)
        {
            try
            {
                var _emp = _appContext.Employees.Where(x => x.ID == param.EmployeeId).FirstOrDefault();
                var eprofile = (from p in _appContext.Employees.Where(x => x.ID == param.EmployeeId) select p.Profile).FirstOrDefault();
                if (_emp == null)
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
                var c = await _appContext.Contracts.Where(x => x.EMPLOYEE_ID == param.EmployeeId && x.ID != param.Id && (x.EXPIRE_DATE >= param.StartDate || x.START_DATE > param.StartDate) && x.STATUS_ID == OtherConfig.STATUS_APPROVE).CountAsync();
                if (c > 0)
                {
                    return new FormatedResponse
                    {
                        ErrorType = EnumErrorType.CATCHABLE,
                        MessageCode = "CONTRACT_STARTDATE_NOT_LESS_CURRENT",
                        InnerBody = param,
                        StatusCode = EnumStatusCode.StatusCode400
                    };
                }

                var r = _appContext.Contracts.Where(x => x.ID == param.Id).FirstOrDefault();
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
                var workingMax = await GetLastWorking(param.EmployeeId, null);
                var rs = (WorkingInputDTO)workingMax.Data;
                var data = Map(param, r);
                if (rs != null)
                {
                    data.ORG_ID = rs.OrgId;
                    data.POSITION_ID = rs.PositionId;
                }
                else
                {
                    data.ORG_ID = _emp.ORG_ID;
                    data.POSITION_ID = _emp.POSITION_ID;
                }
                var result = _appContext.Contracts.Update(data);

                // when user choose "Đã phê duyệt"
                var getStatusApproveId = _appContext.OtherLists.FirstOrDefault(x => x.CODE == "DD")?.ID;
                if (data.STATUS_ID == getStatusApproveId && data.START_DATE!.Value.Date <= DateTime.Now.Date) await ApproveContract(param);

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
        public async Task<ResultWithError> GetLastWorking(long? empId, DateTime? date)
        {
            try
            {
                if (date == null)
                {
                    date = DateTime.Now;
                }
                var r = await (from p in _appContext.Workings
                               where p.EMPLOYEE_ID == empId && p.EFFECT_DATE <= date && (p.IS_WAGE == 0 || p.IS_WAGE == null) && p.STATUS_ID == 994 && p.IS_RESPONSIBLE == true
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
                    var r = _appContext.Contracts.Where(x => x.ID == item).FirstOrDefault();
                    if (r == null)
                    {
                        return new FormatedResponse
                        {
                            ErrorType = EnumErrorType.CATCHABLE,
                            MessageCode = Consts.ID_NOT_FOUND,
                            StatusCode = EnumStatusCode.StatusCode400
                        };
                    }
                    // Kiểm tra xem nếu dữ liệu đang sửa ở trạng thái phê duyệt thì không cho xóa
                    if (r.STATUS_ID == OtherConfig.STATUS_APPROVE)
                    {
                        return new FormatedResponse
                        {
                            ErrorType = EnumErrorType.CATCHABLE,
                            StatusCode = EnumStatusCode.StatusCode400,
                            MessageCode = Message.RECORD_IS_APPROVED,
                        };
                    }
                    _appContext.Contracts.Remove(r);
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
                var r = _appContext.Contracts.Where(x => x.ID == id).FirstOrDefault();
                if (r == null)
                {
                    return new ResultWithError(404);
                }
                // Kiểm tra xem HĐ mở chờ phê duyệt có phải là hđ cuối cùng không
                var c = await _appContext.Contracts.Where(x => x.EMPLOYEE_ID == r.EMPLOYEE_ID && x.ID != id && x.EXPIRE_DATE > r.START_DATE && x.STATUS_ID == OtherConfig.STATUS_APPROVE).CountAsync();
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
                var result = _appContext.Contracts.Update(r);
                await _appContext.SaveChangesAsync();
                return new ResultWithError(200);
            }
            catch (Exception ex)
            {
                return new ResultWithError(ex.Message);
            }
        }
        public async Task<ResultWithError> TemplateImport(int orgId)
        {
            try
            {
                // xử lý fill dữ liệu vào master data
                var ds = QueryData.ExecuteStoreToTable(Procedures.PKG_IMPORT_CONTRACT_DATA_IMPORT,
                new
                {
                    P_ORG_ID = orgId,
                    P_CUR_STATUS = QueryData.OUT_CURSOR,
                    P_CUR_CONTRACT = QueryData.OUT_CURSOR,
                    P_CUR = QueryData.OUT_CURSOR
                }, true);

                if (ds.Tables[0].Rows.Count <= 0)
                {
                    return new ResultWithError("DATA_EMPTY");
                }
                ds.Tables[0].TableName = "Status";
                ds.Tables[1].TableName = "Contract";
                ds.Tables[2].TableName = "Data";
                var pathTemp = _appContext._config["urlTempContract"];
                var memoryStream = Template.FillTemplate(pathTemp, ds);
                return new ResultWithError(memoryStream);
            }
            catch (Exception ex)
            {
                return new ResultWithError(ex.Message);
            }
        }
        /// <summary>
        /// Import Data
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<ResultWithError> ImportTemplate(ImportCTractParam param)
        {
            try
            {
                param.Data.RemoveRange(0, 2);
                if (param.Data.Count == 0)
                {
                    return new ResultWithError(404);
                }
                var lst = new List<TMP_HU_CONTRACT>();
                var guid = Guid.NewGuid().ToString();
                var error = false;
                var lstError = new List<ImportCTractDtlParam>();
                foreach (var item in param.Data)
                {
                    var dtl = new TMP_HU_CONTRACT();
                    if (string.IsNullOrWhiteSpace(item.TypeName))
                    {
                        error = true;
                        item.TypeName = "!Không được để trống";
                    }
                    if (string.IsNullOrWhiteSpace(item.ContractNo))
                    {
                        error = true;
                        item.ContractNo = "!Không được để trống";
                    }
                    try
                    {

                        dtl.DATE_START = DateTime.ParseExact(item.DateStart.Trim(), "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    }
                    catch
                    {
                        error = true;
                        item.DateStart = "!Không đúng định dạng dd/MM/yyyy";
                    }
                    try
                    {
                        if (!string.IsNullOrWhiteSpace(item.DateEnd))
                        {
                            dtl.DATE_END = DateTime.ParseExact(item.DateEnd.Trim(), "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                        }
                    }
                    catch
                    {
                        error = true;
                        item.DateEnd = "!Không đúng định dạng dd/MM/yyyy";
                    }
                    if (!string.IsNullOrWhiteSpace(item.SignDate))
                    {
                        try
                        {
                            dtl.SIGN_DATE = DateTime.ParseExact(item.SignDate.Trim(), "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                        }
                        catch
                        {
                            error = true;
                            item.SignDate = "!Không đúng định dạng dd/MM/yyyy";
                        }
                    }
                    if (string.IsNullOrWhiteSpace(item.SalaryBasic))
                    {
                        error = true;
                        item.SalaryBasic = "!Không được để trống";
                    }
                    else
                    {
                        try
                        {
                            dtl.SAL_BASIC = decimal.Parse(item.SalaryBasic);
                        }
                        catch
                        {

                            error = true;
                            item.SalaryBasic = "!Sai định dạng kiểu số";
                        }
                    }

                    if (string.IsNullOrWhiteSpace(item.SalaryPercent))
                    {
                        error = true;
                        item.SalaryPercent = "!Không được để trống";
                    }
                    else
                    {
                        try
                        {
                            dtl.SAL_PERCENT = decimal.Parse(item.SalaryPercent);
                        }
                        catch
                        {
                            error = true;
                            item.SalaryPercent = "!Sai định dạng kiểu số";
                        }
                    }
                    if (string.IsNullOrWhiteSpace(item.StatusName))
                    {
                        error = true;
                        item.StatusName = "!Không được để trống";
                    }
                    if (error)
                    {
                        error = false;
                        lstError.Add(item);
                    }
                    else
                    {
                        dtl.REF_CODE = guid;
                        dtl.CODE = item.Code;
                        dtl.CONTRACT_NO = item.ContractNo;
                        dtl.CONTRACT_TYPE_NAME = item.TypeName.Trim();
                        dtl.STATUS_NAME = item.StatusName.Trim();
                        if (item.SignName != null)
                        {
                            dtl.SIGNER_NAME = item.SignName.Trim();
                        }
                        if (item.SignPosition != null)
                        {
                            dtl.SIGNER_POSITION = item.SignPosition.Trim();
                        }
                        lst.Add(dtl);
                    }
                }

                if (lstError.Count > 0)
                {
                    var pathTemp = _appContext._config["urlTempContract"];
                    var memoryStream = Template.FillTemp(pathTemp, lstError);
                    return new ResultWithError(memoryStream);
                }
                else
                {
                    if (lst.Count > 0)
                    {
                        await _appContext.ContractTmps.AddRangeAsync(lst);
                        await _appContext.SaveChangesAsync();
                        // xử lý fill dữ liệu vào master data
                        var ds = QueryData.ExecuteStoreToTable("PKG_IMPORT.CONTRACT_IMPORT",
                        new
                        {
                            P_REF_CODE = guid,
                            P_CUR = QueryData.OUT_CURSOR
                        }, true);

                        if (ds.Tables.Count > 0)
                        {
                            ds.Tables[0].TableName = "Data";
                            var pathTemp = _appContext._config["urlTempContract"];
                            var memoryStream = Template.FillTemplate(pathTemp, ds);
                            return new ResultWithError(memoryStream);
                        }
                    }
                }
                return new ResultWithError(200);
            }
            catch (Exception ex)
            {
                return new ResultWithError(204);
            }
        }

        /// <summary>
        /// Portal
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<ResultWithError> PortalGetAll()
        {
            try
            {
                var r = await (from p in _appContext.Contracts
                               join o in _appContext.ContractTypes on p.CONTRACT_TYPE_ID equals o.ID
                               where p.EMPLOYEE_ID == _appContext.EmpId && p.STATUS_ID == OtherConfig.STATUS_APPROVE
                               orderby p.START_DATE descending
                               select new
                               {
                                   Id = p.ID,
                                   No = p.CONTRACT_NO,
                                   StartDate = p.START_DATE,
                                   EndDate = p.EXPIRE_DATE,
                                   TypeName = o.NAME
                               }).ToListAsync();
                return new ResultWithError(r);
            }
            catch (Exception ex)
            {
                return new ResultWithError(ex.Message);
            }
        }

        /// <summary>
        /// Portal
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<ResultWithError> PortalGetBy(long id)
        {
            try
            {
                var r = await (from p in _appContext.Contracts
                               join o in _appContext.ContractTypes on p.CONTRACT_TYPE_ID equals o.ID
                               where p.EMPLOYEE_ID == _appContext.EmpId && p.ID == id
                               select new
                               {
                                   No = p.CONTRACT_NO,
                                   TypeName = o.NAME,
                                   StartDate = p.START_DATE,
                                   EndDate = p.EXPIRE_DATE,
                                   SignName = p.SIGNER_NAME,
                                   SignPos = p.SIGNER_POSITION,
                                   SignDate = p.SIGN_DATE,
                                   SalBasic = p.SAL_BASIC,
                                   SalPercent = p.SAL_PERCENT
                               }).ToListAsync();
                return new ResultWithError(r);
            }
            catch (Exception ex)
            {
                return new ResultWithError(ex.Message);
            }
        }
        public async Task<FormatedResponse> GetByEmployeeId(long EmployeeId)
        {
            var n = await (from p in _appContext.Contracts
                           join l in _appContext.ContractTypes on p.CONTRACT_TYPE_ID equals l.ID
                           join o in _appContext.OtherLists on p.STATUS_ID equals o.ID
                           where p.EMPLOYEE_ID == EmployeeId && o.CODE == "DD"
                           orderby p.ID descending, p.START_DATE descending
                           select new
                           {
                               Id = p.ID,
                               Name = p.CONTRACT_NO
                           }).ToListAsync();
            return new() { InnerBody = n };

        }
        public async Task<FormatedResponse> GetContractByEmpProfile(long EmployeeId)
        {
            var n = await (from p in _appContext.Contracts
                           join l in _appContext.ContractTypes on p.CONTRACT_TYPE_ID equals l.ID
                           join o in _appContext.OtherLists on p.STATUS_ID equals o.ID
                           where p.EMPLOYEE_ID == EmployeeId && p.STATUS_ID == OtherConfig.STATUS_APPROVE
                           orderby p.ID descending, p.START_DATE descending
                           select new
                           {
                               Id = p.ID,
                               EmployeeId = EmployeeId,
                               ContractNo = p.CONTRACT_NO,
                               StartDate = p.START_DATE,
                               ExpireDate = p.EXPIRE_DATE,
                               SignerName = p.SIGNER_NAME,
                               SignPos = p.SIGNER_POSITION,
                               SignDate = p.SIGN_DATE,
                               StatusName = o.NAME,
                               Note = p.NOTE,
                               ContractType = l.NAME,
                               Appendix = (from f in _appContext.HuFileContracts
                                           where f.EMPLOYEE_ID == EmployeeId
                                           join w in _appContext.Contracts on f.ID_CONTRACT equals w.ID into tmp1
                                           from w1 in tmp1.DefaultIfEmpty()
                                           join l in _appContext.ContractTypes on w1.CONTRACT_TYPE_ID equals l.ID into tmp2
                                           from ct1 in tmp2.DefaultIfEmpty()
                                           join ct in _appContext.ContractTypes on f.APPEND_TYPEID equals ct.ID into tmp3
                                           from ct2 in tmp3.DefaultIfEmpty()
                                           join o in _appContext.OtherLists on p.STATUS_ID equals o.ID

                                           join sal in _appContext.Workings on f.WORKING_ID equals sal.ID into tmp4
                                           from sa in tmp4.DefaultIfEmpty()
                                           join wa in _appContext.WorkingAllowances on sa.ID equals wa.WORKING_ID  into tmp5
                                           from wl in tmp5.DefaultIfEmpty()
                                           join al in _appContext.Allowances on wl.ALLOWANCE_ID equals al.ID into tmp6
                                           from all in tmp6.DefaultIfEmpty()

                                           where p.ID == f.ID_CONTRACT
                                           select new
                                           {
                                               Id = f.ID,
                                               StartDate = f.START_DATE,
                                               ExpireDate = f.EXPIRE_DATE,
                                               ContractAppendixNo = f.CONTRACT_NO,
                                               ContractName = ct1.NAME,
                                               SignDate = f.SIGN_DATE,
                                               ContractType = ct2.NAME,
                                               SalInsu = sa.SAL_INSU,
                                               Coefficient = sa.COEFFICIENT,
                                               AllowanceName = all.NAME

                                           }).ToList(),
                           }).ToListAsync();
            return new() { InnerBody = n };

        }

        public async Task<FormatedResponse> GetContractByEmpProfilePortal(long EmployeeId)
        {
            var n = await (from p in _appContext.Contracts
                           join l in _appContext.ContractTypes on p.CONTRACT_TYPE_ID equals l.ID
                           join o in _appContext.OtherLists on p.STATUS_ID equals o.ID
                           where p.EMPLOYEE_ID == EmployeeId && p.STATUS_ID == OtherConfig.STATUS_APPROVE
                           orderby p.ID descending, p.START_DATE descending
                           select new
                           {
                               Id = p.ID,
                               ContractNo = p.CONTRACT_NO,
                               StartDate = p.START_DATE,
                               ExpireDate = p.EXPIRE_DATE,
                               SignerName = p.SIGNER_NAME,
                               SignPos = p.SIGNER_POSITION,
                               SignDate = p.SIGN_DATE,
                               StatusName = o.NAME,
                               Note = p.NOTE,
                               ContractType = l.NAME,
                               Appendix = (from f in _appContext.HuFileContracts
                                           where f.EMPLOYEE_ID == EmployeeId
                                           join w in _appContext.Contracts on f.ID_CONTRACT equals w.ID into tmp1
                                           from w1 in tmp1.DefaultIfEmpty()
                                           join l in _appContext.ContractTypes on w1.CONTRACT_TYPE_ID equals l.ID into tmp2
                                           from ct1 in tmp2.DefaultIfEmpty()
                                           join ct in _appContext.ContractTypes on f.APPEND_TYPEID equals ct.ID into tmp3
                                           from ct2 in tmp3.DefaultIfEmpty()
                                           join o in _appContext.OtherLists on p.STATUS_ID equals o.ID

                                           join sal in _appContext.Workings on f.WORKING_ID equals sal.ID into tmp4
                                           from sa in tmp4.DefaultIfEmpty()
                                           //join wa in _appContext.WorkingAllowances on sa.ID equals wa.WORKING_ID into tmp5
                                           //from wl in tmp5.DefaultIfEmpty()
                                           //join al in _appContext.Allowances on wl.ALLOWANCE_ID equals al.ID into tmp6
                                           //from all in tmp6.DefaultIfEmpty()

                                           where p.ID == f.ID_CONTRACT
                                           select new
                                           {
                                               Id = f.ID,
                                               StartDate = f.START_DATE,
                                               ExpireDate = f.EXPIRE_DATE,
                                               ContractAppendixNo = f.CONTRACT_NO,
                                               ContractName = ct1.NAME,
                                               SignDate = f.SIGN_DATE,
                                               ContractType = ct2.NAME,
                                               SalInsu = sa.SAL_INSU,
                                               AllowanceList = (from wa in _appContext.WorkingAllowances
                                                                from n in _appContext.Allowances.Where(x => x.ID == wa.ALLOWANCE_ID)
                                                                from a in _appContext.HuFileContracts.Where(x => x.WORKING_ID == wa.WORKING_ID)
                                                                where  wa.WORKING_ID == p.ID
                                                                select new
                                                                {
                                                                    Id = wa.ID,
                                                                    WorkingId = wa.WORKING_ID,
                                                                    AllowanceId = wa.ALLOWANCE_ID,
                                                                    //AllowanceName = n.NAME,
                                                                }).ToList(),
                                           }).ToList(),
                           }).ToListAsync();
            return new() { InnerBody = n };

        }


        public async Task<FormatedResponse> GetLastContract(long? empId, string? date)
        {
            var n = await (from p in _appContext.Contracts
                           join l in _appContext.ContractTypes on p.CONTRACT_TYPE_ID equals l.ID
                           where p.EMPLOYEE_ID == empId && p.START_DATE <= Convert.ToDateTime(date.Replace("_", "/")) && p.STATUS_ID == OtherConfig.STATUS_APPROVE
                           orderby p.START_DATE descending
                           select new
                           {
                               Id = p.ID,
                               ContractNo = p.CONTRACT_NO,
                               StartDate = p.START_DATE,
                               ExpireDate = p.EXPIRE_DATE,
                               SignerName = p.SIGNER_NAME,
                               SignPos = p.SIGNER_POSITION,
                               SignDate = p.SIGN_DATE,
                               Note = p.NOTE,
                               ContractType = l.NAME,
                           }).FirstOrDefaultAsync();
            return new() { InnerBody = n };

        }
        public async Task<FormatedResponse> GetContractType()
        {
            var queryable = await (from c in _appContext.ContractTypes.AsNoTracking()
                                   from ct in _appContext.ContractTypeSyses.AsNoTracking().Where(ct => ct.ID == c.TYPE_ID && ct.IS_ACTIVE == true)
                                   where c.IS_ACTIVE == true && ct.CODE != "PLHD"
                                   orderby c.CODE
                                   select new
                                   {
                                       Id = c.ID,
                                       Name = c.NAME,
                                       Code = c.CODE,
                                   }).ToListAsync();
            return new FormatedResponse() { InnerBody = queryable };
        }
        public async Task<FormatedResponse> GetWageByStartdateContract(long? empId, string? date)
        {
            try
            {

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
                               //from m in _appContext.InsRegions.Where(x => x.)
                               where p.EMPLOYEE_ID == empId && p.EFFECT_DATE!.Value.Date <= Convert.ToDateTime(date.Replace("_", "/")).Date && (p.IS_WAGE == -1 || p.IS_WAGE == 1) && p.STATUS_ID == 994
                               orderby p.EFFECT_DATE descending
                               select new HuWorkingDTO
                               {
                                   Id = p.ID,
                                   OrgId = p.ORG_ID,
                                   EmployeeId = e.ID,
                                   EmployeeCode = e.CODE,
                                   EmployeeName = e.Profile!.FULL_NAME,
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
                                   StatusId = p.STATUS_ID,
                                   SalInsu = p.SAL_INSU,
                               }).FirstOrDefaultAsync();
                if (r != null)
                {

                    var getAreaId = (from item in _appContext.Workings.Where(x => x.ID == r.Id)
                                     from he in _appContext.Employees.Where(x => x.ID == item.EMPLOYEE_ID).DefaultIfEmpty()
                                     from ho in _appContext.Organizations.Where(x => x.ID == he.ORG_ID).DefaultIfEmpty()
                                     from hc in _appContext.CompanyInfos.Where(x => x.ID == ho.COMPANY_ID).DefaultIfEmpty()
                                     select hc.REGION_ID).FirstOrDefault();

                    var listInsRegions = (from item in _appContext.InsRegions
                                          where item.AREA_ID == getAreaId
                                                && item.IS_ACTIVE == true
                                          orderby item.EFFECT_DATE descending
                                          select item).ToList();

                    var list = listInsRegions.Select(x => new
                    {
                        EffectDate = new DateTime(x.EFFECT_DATE!.Value.Year, x.EFFECT_DATE.Value.Month, x.EFFECT_DATE.Value.Day, 0, 0, 0),
                        Money = x.MONEY
                    });

                    var regionMinimumWage = (from item in list
                                             where item.EffectDate <= r.EffectDate
                                             select item.Money).FirstOrDefault();

                    r.RegionMinimumWage = (long?)regionMinimumWage;
                }
                return new FormatedResponse() { InnerBody = r, StatusCode = EnumStatusCode.StatusCode200 };
            }
            catch (Exception ex)
            {
                return new FormatedResponse
                {
                    MessageCode = ex.Message,
                    InnerBody = "",
                    StatusCode = EnumStatusCode.StatusCode400
                };
            }
        }
        public bool IsFirstContract(long? empId, long? contractId)
        {
            bool check = _appContext.Contracts.Where(x => x.STATUS_ID == OtherConfig.STATUS_APPROVE && x.EMPLOYEE_ID == empId && x.ID != contractId).Any();
            return !check;
        }
        /// <summary>
        /// check la hop dong chinh thuc dau tien
        /// </summary>
        /// <param name="empId"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        public bool IsFirstOficialContract(long? empId, long? contractId)
        {
            bool check = (from p in _appContext.Contracts.Where(x => x.STATUS_ID == OtherConfig.STATUS_APPROVE)
                          join l in _appContext.ContractTypes on p.CONTRACT_TYPE_ID equals l.ID
                          join o in _appContext.ContractTypeSyses on l.TYPE_ID equals o.ID
                          where o.CODE != "HDTV" && o.CODE != "HDHV" && o.CODE != "PLHD" && p.EMPLOYEE_ID == empId && p.ID != contractId
                          select p).Any();

            return !check;
        }
        public async Task<bool> ApproveContract(ContractInputDTO param)
        {
            try
            {
                var r = _appContext.Contracts.Where(x => x.ID == param.Id).FirstOrDefault();

                if (r != null)
                {
                    var e = _appContext.Employees.Where(x => x.ID == r.EMPLOYEE_ID).FirstOrDefault();
                    var eprofile = (from p in _appContext.Employees.Where(x => x.ID == r.EMPLOYEE_ID) select p.Profile).FirstOrDefault();
                    if (param.StatusId == OtherConfig.STATUS_APPROVE)
                    {
                        //InsertDecision
                        if (IsFirstContract(e.ID, r.ID) && r.IS_RECEIVE != false)
                        {
                            //get DecisionNo
                            //decimal num;
                            //string formatCode = "/QĐ-VNS";
                            //string formatDecisionNo = "xxxx" + formatCode;
                            //int numOfDecisionNo = 4;
                            //var queryCode = (from x in _appContext.Workings where x.DECISION_NO.Length == 11 && x.IS_WAGE != -1 select x.DECISION_NO).ToList();
                            //var existingCode = (from p in queryCode where Decimal.TryParse(p.Substring(0, numOfDecisionNo), out num) orderby p descending select p).ToList();
                            //string newcode = StringCodeGenerator.CreateNewCode(formatCode, numOfDecisionNo, existingCode, "RIGHT");
                            var _typeId = (from p in _appContext.OtherLists.Where(p => p.IS_ACTIVE == true)
                                           join ot in _appContext.OtherListTypes on p.TYPE_ID equals ot.ID
                                           where ot.CODE == "TYPE_DECISION" && p.CODE == "CT"
                                           select p.ID).FirstOrDefault();
                            //------insert-----------------------------------------------------------
                            WorkingInputDTO paramW = new WorkingInputDTO();
                            paramW.EmployeeId = r.EMPLOYEE_ID;
                            paramW.TypeId = _typeId;
                            paramW.DecisionNo = " ";
                            paramW.EffectDate = r.START_DATE;
                            paramW.OrgId = r.ORG_ID;
                            paramW.PositionId = r.POSITION_ID;
                            paramW.EmployeeObjId = eprofile.EMPLOYEE_OBJECT_ID;
                            paramW.WageId = r.WORKING_ID;
                            paramW.StatusId = OtherConfig.STATUS_WAITING;
                            var dataWorking = Map(paramW, new HU_WORKING());
                            var resultW = await _appContext.Workings.AddAsync(dataWorking);
                            e.JOIN_DATE = r.START_DATE;
                        }
                        //-------------------------------------------------------------------------------------------

                        var _TypeEmpStatus = (from p in _appContext.OtherLists.Where(p => p.IS_ACTIVE == true)
                                              join ot in _appContext.OtherListTypes on p.TYPE_ID equals ot.ID
                                              where ot.CODE == "EMP_STATUS" && p.CODE == "ESW"
                                              select p.ID).FirstOrDefault();
                        var _TypeEmpStatusDetail = (from p in _appContext.OtherLists.Where(p => p.IS_ACTIVE == true)
                                                    join ot in _appContext.OtherListTypes on p.TYPE_ID equals ot.ID
                                                    where ot.CODE == "STATUS_DETAIL" && ot.IS_ACTIVE == true && p.IS_ACTIVE == true
                                                    select p).ToArray();

                        var contractType = (from p in _appContext.ContractTypes.Where(p => p.IS_ACTIVE == true)
                                            join ot in _appContext.ContractTypeSyses on p.TYPE_ID equals ot.ID
                                            where p.ID == r.CONTRACT_TYPE_ID
                                            select p).FirstOrDefault();
                                            //select ot).FirstOrDefault(); code cu
                        
                        if (e != null)
                        {
                            e.CONTRACT_ID = param.Id;
                            e.CONTRACT_TYPE_ID = r.CONTRACT_TYPE_ID;
                            if (e.WORK_STATUS_ID == OtherConfig.EMP_STATUS_TERMINATE){}
                            else
                            {
                                if (_TypeEmpStatus != null)
                                {
                                    e.WORK_STATUS_ID = _TypeEmpStatus;
                                }
                                if (contractType != null && _TypeEmpStatusDetail.Length > 0)
                                {
                                    //cap nhat trang thai chi tiet
                                    switch (contractType.CODE.ToUpper())
                                    {
                                        case "HDTV"://thu viec
                                            var _IDHDTV = (from p in _TypeEmpStatusDetail where p.CODE == "00003" select p.ID).FirstOrDefault();
                                            if (_IDHDTV != null)
                                            {
                                                e.STATUS_DETAIL_ID = _IDHDTV;
                                            }
                                            break;
                                        case "HDNH"://ngan han
                                            var _IDHDNH = (from p in _TypeEmpStatusDetail where p.CODE == "00359" select p.ID).FirstOrDefault();
                                            if (_IDHDNH != null)
                                            {
                                                e.STATUS_DETAIL_ID = _IDHDNH;
                                            }

                                            break;
                                        case "HDKXDTH"://khong xd thoi han
                                            var _IDHDKXDTH = (from p in _TypeEmpStatusDetail where p.CODE == "00005" select p.ID).FirstOrDefault();
                                            if (_IDHDKXDTH != null)
                                            {
                                                e.STATUS_DETAIL_ID = _IDHDKXDTH;
                                            }
                                            if (IsFirstOficialContract(r.EMPLOYEE_ID, r.ID))
                                            {
                                                e.JOIN_DATE_STATE = r.START_DATE;
                                            }
                                            break;
                                        case "HXDTH"://xac dinh thoi han
                                            var _IDHXDTH = (from p in _TypeEmpStatusDetail where p.CODE == "00004" select p.ID).FirstOrDefault();
                                            if (_IDHXDTH != null)
                                            {
                                                e.STATUS_DETAIL_ID = _IDHXDTH;
                                            }
                                            if (IsFirstOficialContract(r.EMPLOYEE_ID, r.ID))
                                            {
                                                e.JOIN_DATE_STATE = r.START_DATE;
                                            }
                                            break;
                                        case "KTDTKH" or "TTCTL":
                                            var _IDTTCTL = (from p in _TypeEmpStatusDetail where p.CODE == "00068" select p.ID).FirstOrDefault();
                                            if (_IDTTCTL != null)
                                            {
                                                e.STATUS_DETAIL_ID = _IDTTCTL;
                                            }
                                            break;
                                        default:
                                            break;
                                    }
                                }
                            }
                            _appContext.Employees.Update(e);
                        }
                    }
                    await _appContext.SaveChangesAsync();
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public async Task<FormatedResponse> ChangeStatusApprove(ContractInputDTO param)
        {
            try
            {
                var r = _appContext.Contracts.Where(x => x.ID == param.Id).FirstOrDefault();
                var e = _appContext.Employees.Where(x => x.ID == r.EMPLOYEE_ID).FirstOrDefault();
                if (r != null)
                {
                    r.STATUS_ID = param.ValueToBind == true ? OtherConfig.STATUS_APPROVE : (param.ValueToBind == false ? OtherConfig.STATUS_WAITING : null);
                    _appContext.Contracts.Update(r);

                    if (r.STATUS_ID == OtherConfig.STATUS_APPROVE && r.START_DATE!.Value.Date <= DateTime.Now.Date)
                    {
                        param.StatusId = r.STATUS_ID;
                        await ApproveContract(param);
                    }
                    else
                    {
                        if (e != null && e.CONTRACT_ID == r.ID)
                        {
                            e.CONTRACT_ID = null;
                        }
                        _appContext.Employees.Update(e);
                    }

                }
                await _appContext.SaveChangesAsync();
                param.StartDate = Convert.ToDateTime(r.START_DATE).Date;
                param.EmployeeId = e.ID;
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

        public async Task<FormatedResponse> Save(ImportQueryListBaseDTO request)
        {
            try
            {
                //await _appContext.Database.BeginTransactionAsync();
                var now = DateTime.UtcNow;

                var tmp1 = await _appContext.ContractImports.Where(x => x.XLSX_USER_ID == request.XlsxSid && x.XLSX_EX_CODE == request.XlsxExCode && x.XLSX_SESSION == request.XlsxSession).ToListAsync();
                var tmp1Type = typeof(HU_CONTRACT_IMPORT);
                var tmp1Properties = tmp1Type.GetProperties()?
                    .Where(x => !XLSX_COLUMNS.Contains(x.Name))
                    .ToList();

                var contract = typeof(HU_CONTRACT);
                var cvTypeProperties = contract.GetProperties().ToList();

                tmp1.ForEach(async tmpCv =>
                {
                    var obj1 = Activator.CreateInstance(typeof(HU_CONTRACT)) ?? throw new Exception(CommonMessageCode.ACTIVATOR_CREATE_INSTANCE_RETURNS_NULL);
                    var cv = (HU_CONTRACT)obj1;

                    tmp1Properties?.ForEach(tmp1Property =>
                    {
                        var tmp1Value = tmp1Property.GetValue(tmpCv);
                        var cvProperty = cvTypeProperties.SingleOrDefault(x => x.Name == tmp1Property.Name);
                        if (cvProperty != null)
                        {
                            if (tmp1Value != null)
                            {
                                cvProperty.SetValue(cv, tmp1Value);
                            };
                        }
                        else
                        {
                            if (tmp1Value != null)
                            {
                                throw new Exception($"{tmp1Property.Name} was not found in HU_EMPLOYEE_CV");
                            }
                        }
                    });
                    //check HSL chua phe duyet
                    var checkWorking = _appContext.Workings.AsNoTracking().SingleOrDefault(p => p.ID == cv.WORKING_ID);
                    if (checkWorking != null)
                    {
                        if (checkWorking.STATUS_ID == OtherConfig.STATUS_WAITING)
                        {
                            throw new Exception("WORKING_IS_NOT_APPROVE");
                        }
                    }
                    else
                    {
                        throw new Exception("WORKING_IS_NOT_EXIST");
                    }
                    //check ton tai so hop dong
                    var checkContractNo = _appContext.Contracts.AsNoTracking().Where(p => p.CONTRACT_NO.Trim().ToUpper() == cv.CONTRACT_NO.Trim().ToUpper()).Any();
                    if (checkContractNo)
                    {
                        throw new Exception("CONTRACT_NO_IS_EXIST" + " " + cv.CONTRACT_NO.ToUpper());
                    }
                    //lay phong ban nhan vien
                    var emp = _appContext.Employees.AsNoTracking().Where(p => p.ID == cv.EMPLOYEE_ID).FirstOrDefault();
                    cv.ORG_ID = emp!.ORG_ID;
                    //lay chuc danh nguoi ky
                    cv.POSITION_ID = emp!.POSITION_ID;
                    //lay ten nguoi ky
                    var signer = (from e in _appContext.Employees.AsNoTracking().Where(p => p.ID == cv.SIGN_ID)
                                  from cvs in _appContext.EmployeeCvs.AsNoTracking().Where(p => p.ID == e.PROFILE_ID).DefaultIfEmpty()
                                  select cvs).FirstOrDefault();
                    cv.SIGNER_NAME = signer!.FULL_NAME ?? "";
                    //lay chuc danh nguoi ky
                    var signerPos = (from e in _appContext.Employees.AsNoTracking().Where(p => p.ID == cv.SIGN_ID)
                                     from p in _appContext.Positions.AsNoTracking().Where(p => p.ID == e.POSITION_ID).DefaultIfEmpty()
                                     select p).FirstOrDefault();
                    cv.SIGNER_POSITION = signerPos!.NAME ?? "";

                    cv.CREATED_DATE = now;
                    cv.CREATED_BY = request.XlsxSid;
                    var contractInput = new ContractInputDTO()
                    {
                        ContractNo = cv.CONTRACT_NO,
                        ContractTypeId = cv.CONTRACT_TYPE_ID,
                        EmployeeId = (long)cv.EMPLOYEE_ID!,
                        ExpireDate = cv.EXPIRE_DATE,
                        WorkingId = cv.WORKING_ID,
                        SignDate = cv.SIGN_DATE,
                        StatusId = cv.STATUS_ID,
                        StartDate = cv.START_DATE!.Value,
                        SignId = cv.SIGN_ID,
                        SignerPosition = cv.SIGNER_POSITION,
                        SignerName = cv.SIGNER_NAME,
                        Note = cv.NOTE,

                    };
                    var res = await CreateAsync(contractInput);
                    if (res.StatusCode != EnumStatusCode.StatusCode200)
                    {
                        throw new Exception(res.MessageCode);
                    }
                    //_appContext.HuContracts.Add(cv);
                });

                // Clear tmp
                await _appContext.SaveChangesAsync();
                //_appContext.Database.CommitTransaction();
                return new() { InnerBody = true };

            }
            catch (Exception ex)
            {
                //_appContext.Database.RollbackTransaction();
                return new() { ErrorType = EnumErrorType.UNCATCHABLE, StatusCode = EnumStatusCode.StatusCode500, MessageCode = ex.Message };
            }
        }


        public async Task<bool> IsReceive(ContractInputDTO request)
        {
            var x = _appContext.Contracts.AsNoTracking().Where(p => p.EMPLOYEE_ID == request.EmployeeId && p.STATUS_ID == OtherConfig.STATUS_APPROVE).Any();
            return x;
        }


        public async Task<FormatedResponse> LiquidationContract(ContractInputDTO request)
        {
            var get_employeeId = await _appContext.Contracts.Where(x => x.ID == request.Id).Select(x => x.EMPLOYEE_ID).FirstAsync();

            var latest_contract = _appContext.Contracts.Where(x => x.EMPLOYEE_ID == get_employeeId && x.STATUS_ID == OtherConfig.STATUS_APPROVE).OrderByDescending(x => x.START_DATE).First();

            // check is latest contract
            if (request.Id != latest_contract.ID)
            {
                return new FormatedResponse()
                {
                    ErrorType = EnumErrorType.CATCHABLE,
                    MessageCode = CommonMessageCode.CHECK_IS_LATEST_CONTRACT,
                    StatusCode = EnumStatusCode.StatusCode400,
                };
            }

            var get_id_approve = _appContext.OtherLists.Where(x => x.CODE.ToUpper() == "DD").Select(x => x.ID).First();

            // check latest contract is approved
            if (latest_contract.STATUS_ID != get_id_approve)
            {
                return new FormatedResponse()
                {
                    ErrorType = EnumErrorType.CATCHABLE,
                    MessageCode = CommonMessageCode.CHECK_LATEST_CONTRACT_IS_APPROVED,
                    StatusCode = EnumStatusCode.StatusCode400,
                };
            }


            // set hour/minute/second for "request.DateLiquidation.Value"
            DateTime DateLiquidation = request.DateLiquidation.Value;
            DateLiquidation = DateLiquidation.Date.Add(new TimeSpan(0, 0, 0));


            // set hour/minute/second for "latest_contract.START_DATE.Value"
            DateTime START_DATE = latest_contract.START_DATE.Value;
            START_DATE = START_DATE.Date.Add(new TimeSpan(0, 0, 0));


            // check liquidate contract >= start date
            int checkDate1 = DateTime.Compare(DateLiquidation, START_DATE);
            if (checkDate1 < 0)
            {
                return new FormatedResponse()
                {
                    ErrorType = EnumErrorType.CATCHABLE,
                    MessageCode = CommonMessageCode.CHECK_LIQUIDATE_CONTRACT_START_DATE,
                    StatusCode = EnumStatusCode.StatusCode400,
                };
            }

            // if expire date is exist
            if (latest_contract.EXPIRE_DATE != null)
            {
                // set hour/minute/second for "latest_contract.EXPIRE_DATE.Value"
                DateTime EXPIRE_DATE = latest_contract.EXPIRE_DATE.Value;
                EXPIRE_DATE = EXPIRE_DATE.Date.Add(new TimeSpan(0, 0, 0));

                // check liquidate contract <= expire date
                int checkDate2 = DateTime.Compare(DateLiquidation, EXPIRE_DATE);
                if (checkDate2 > 0)
                {
                    return new FormatedResponse()
                    {
                        ErrorType = EnumErrorType.CATCHABLE,
                        MessageCode = CommonMessageCode.CHECK_LIQUIDATE_CONTRACT_EXPIRE_DATE,
                        StatusCode = EnumStatusCode.StatusCode400,
                    };
                }
            }

            // set liquidate date
            latest_contract.LIQUIDATION_DATE = request.DateLiquidation;
            // set liquidate reason
            latest_contract.LIQUIDATION_REASON = request.ReasonLiquidation;

            await _appContext.SaveChangesAsync();

            return new FormatedResponse()
            {
                MessageCode = "LIQUIDATION_CONTRACT_SUCCESS",
                StatusCode = EnumStatusCode.StatusCode200,
                InnerBody = latest_contract
            };
        }


        public async Task<bool> UpdateStatusEmpDetailOfContract(HU_CONTRACT obj)
        {
            try
            {
                if (obj.START_DATE!.Value.Date == DateTime.Now.Date && obj.STATUS_ID == OtherConfig.STATUS_APPROVE)
                {
                    var r = _appContext.Contracts.Where(x => x.ID == obj.ID).FirstOrDefault();

                    if (r != null)
                    {
                        var e = _appContext.Employees.Where(x => x.ID == r.EMPLOYEE_ID).FirstOrDefault();
                        //var terminate = _appContext.Terminates.Where(x => x.EMPLOYEE_ID == r.EMPLOYEE_ID).OrderByDescending(x => x.EFFECT_DATE).FirstOrDefault();
                        var eprofile = (from p in _appContext.Employees.Where(x => x.ID == r.EMPLOYEE_ID) select p.Profile).FirstOrDefault();
                        if (obj.STATUS_ID == OtherConfig.STATUS_APPROVE)
                        {

                            var _TypeEmpStatus = (from p in _appContext.OtherLists.Where(p => p.IS_ACTIVE == true)
                                                  join ot in _appContext.OtherListTypes on p.TYPE_ID equals ot.ID
                                                  where ot.CODE == "EMP_STATUS" && p.CODE == "ESW"
                                                  select p.ID).FirstOrDefault();
                            var _TypeEmpStatusDetail = (from p in _appContext.OtherLists.Where(p => p.IS_ACTIVE == true)
                                                        join ot in _appContext.OtherListTypes on p.TYPE_ID equals ot.ID
                                                        where ot.CODE == "STATUS_DETAIL" && ot.IS_ACTIVE == true && p.IS_ACTIVE == true
                                                        select p).ToArray();

                            //var contractType = (from p in _appContext.ContractTypes.Where(p => p.IS_ACTIVE == true)
                            //                    join ot in _appContext.ContractTypes on p.TYPE_ID equals ot.ID
                            //                    where p.ID == r.CONTRACT_TYPE_ID
                            //                    select ot).FirstOrDefault();

                            var contractType = (from p in _appContext.ContractTypes.Where(p => p.IS_ACTIVE == true && p.ID == r.CONTRACT_TYPE_ID)
                                                select p).FirstOrDefault();

                            if (e != null)
                            {
                                e.CONTRACT_ID = obj.ID;
                                e.CONTRACT_TYPE_ID = r.CONTRACT_TYPE_ID;
                                if (_TypeEmpStatus != null)
                                {
                                    e.WORK_STATUS_ID = _TypeEmpStatus;
                                }
                                if (contractType != null && _TypeEmpStatusDetail.Length > 0)
                                {
                                    //cap nhat trang thai chi tiet
                                    switch (contractType.CODE.ToUpper())
                                    {
                                        case "HDTV":
                                            var _IDHDTV = (from p in _TypeEmpStatusDetail where p.CODE == "00003" select p.ID).FirstOrDefault();
                                            if (_IDHDTV != null)
                                            {
                                                e.STATUS_DETAIL_ID = _IDHDTV;
                                            }
                                            break;
                                        case "HDNH":
                                            var _IDHDNH = (from p in _TypeEmpStatusDetail where p.CODE == "00359" select p.ID).FirstOrDefault();
                                            if (_IDHDNH != null)
                                            {
                                                e.STATUS_DETAIL_ID = _IDHDNH;
                                            }
                                            break;
                                        case "HDLDKXD" or "HDKXDTH":
                                            var _IDHDKXDTH = (from p in _TypeEmpStatusDetail where p.CODE == "00005" select p.ID).FirstOrDefault();
                                            if (_IDHDKXDTH != null)
                                            {
                                                e.STATUS_DETAIL_ID = _IDHDKXDTH;
                                            }
                                            break;
                                        case "HDLDXD" or "HXDTH":
                                            var _IDHXDTH = (from p in _TypeEmpStatusDetail where p.CODE == "00004" select p.ID).FirstOrDefault();
                                            if (_IDHXDTH != null)
                                            {
                                                e.STATUS_DETAIL_ID = _IDHXDTH;
                                            }
                                            break;
                                        case "KTDTKH" or "TTCTL" or "KTDTKHDLD":
                                            var _IDTTCTL = (from p in _TypeEmpStatusDetail where p.CODE == "00068" select p.ID).FirstOrDefault();
                                            if (_IDTTCTL != null)
                                            {
                                                e.STATUS_DETAIL_ID = _IDTTCTL;
                                            }
                                            break;
                                        default:
                                            var _IDHDK = (from p in _TypeEmpStatusDetail where p.CODE == "00009" select p.ID).FirstOrDefault();
                                            if (_IDHDK != null)
                                            {
                                                e.STATUS_DETAIL_ID = _IDHDK;
                                            }
                                            break;
                                    }
                                }else
                                {

                                }
                                _appContext.Employees.Update(e);
                            }
                        }
                        await _appContext.SaveChangesAsync();
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public void ScanUpdateStatusEmpDetail()
        {
            var scanList = _appContext.Contracts.Where(
                x => x.STATUS_ID == OtherConfig.STATUS_APPROVE && x.START_DATE!.Value.Date == DateTime.Now.Date).ToList();
            scanList.ForEach(async data =>
            {
                await UpdateStatusEmpDetailOfContract(data);
            });

        }

        public async Task<bool> ApproveList(string sid)
        {
            var query = (from p in _appContext.Contracts
                         from e in _appContext.Employees.Where(f => f.ID == p.EMPLOYEE_ID)
                         where p.STATUS_ID == OtherConfig.STATUS_APPROVE && p.START_DATE!.Value.Date == DateTime.Now.Date
                         select p).ToList();

            var contracts = new List<HU_CONTRACT>();
            contracts = query.GroupBy(g => g.EMPLOYEE_ID).Select(p => p.OrderByDescending(d => d.START_DATE).First()).ToList();

            foreach (var p in contracts)
            {
                await UpdateStatusEmpDetailOfContract(p);
            }
            return true;
        }
    }
}