using CORE.GenericUOW;
using CORE.DTO;
using API.DTO;
using CORE.Enum;
using CORE.StaticConstant;
using API.All.DbContexts;
using CORE.AutoMapper;
using API.Entities.PORTAL;
using API.DTO.PortalDTO;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Logical;
using Common.Extensions;
using Common.Repositories;
using System;
using Microsoft.AspNetCore.SignalR;
using API.Socket;

namespace API.All.HRM.Profile.ProfileAPI.Portal.PortalApproveLeave
{
    public class PortalApproveLeaveRepository : RepositoryBase<PORTAL_REGISTER_OFF>, IPortalApproveLeaveRepository
    {
        private readonly GenericUnitOfWork _uow;
        private readonly FullDbContext _dbContext;
        private IGenericRepository<PORTAL_REGISTER_OFF, PortalApproveLeaveDTO> _genericRepository;
        private readonly GenericReducer<PORTAL_REGISTER_OFF, PortalApproveLeaveDTO> _genericReducer;
        private readonly IHubContext<SignalHub> _hubContext;

        public PortalApproveLeaveRepository(FullDbContext context, GenericUnitOfWork uow, IHubContext<SignalHub> hubContext) : base(context)
        {
            _dbContext = context;
            _uow = uow;
            _genericRepository = _uow.GenericRepository<PORTAL_REGISTER_OFF, PortalApproveLeaveDTO>();
            _genericReducer = new();
            _hubContext = hubContext;
        }

        public async Task<GenericPhaseTwoListResponse<PortalApproveLeaveDTO>> SinglePhaseQueryList(GenericQueryListDTO<PortalApproveLeaveDTO> request)
        {
            var joined = from p in _dbContext.PortalRegisterOffs.AsNoTracking()
                             // JOIN OTHER ENTITIES BASED ON THE BUSINESS
                         select new PortalApproveLeaveDTO
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

        public async Task<FormatedResponse> GetById(string id, PortalApproveLeaveDTO model)
        {
            
            var currentDate = DateTime.Now;

            var empId = (from p in _dbContext.SysUsers where p.ID == id select p.EMPLOYEE_ID).FirstOrDefault();             // 34235
            var positionId = (from p in _dbContext.HuEmployees where p.ID == empId select p.POSITION_ID).FirstOrDefault();  // 1560

            var lstEmpLM = (from p in _dbContext.HuEmployees    // quan ly truc tiep
                            from a in _dbContext.HuPositions.Where(f => f.MASTER == p.ID)
                            where a.LM == positionId
                            select p.ID).ToList();

            var lstEmpCSM = (from p in _dbContext.HuEmployees   //  quan ly gian tiep
                             from a in _dbContext.HuPositions.Where(f => f.MASTER == p.ID)
                             where a.CSM == positionId
                             select p.ID).ToList();

            //var lstEmpLMOfLM = (from p in _dbContext.HuEmployees)

            var lstEmpAffDep = (from t in _dbContext.HuPositions    // quan ly phong ban truc thuoc 
                                from a in _dbContext.HuOrganizations.Where(f => f.ID == t.ORG_ID)
                                from p in _dbContext.HuEmployees.Where(f => f.ORG_ID == a.ID)
                                where t.ID == positionId && t.IS_TDV == true
                                select p.ID).ToList();

            var lstEmpSupDep = (from t in _dbContext.HuPositions    // quan ly phong ban cap tren
                                from a in _dbContext.HuOrganizations.Where(f => f.PARENT_ID == t.ORG_ID)
                                from p in _dbContext.HuEmployees.Where(f => f.ORG_ID == a.ID)
                                where t.ID == positionId && t.IS_TDV == true
                                select p.ID).ToList();
            // danh sách ủy quyền
            var lstEmpAuth = (from a in _dbContext.SeAuthorizeApproves
                              where a.EMPLOYEE_AUTH_ID == empId
                              select a.EMPLOYEE_ID).ToList();

            var joined = from p in _dbContext.PortalRegisterOffs
                         from e in _dbContext.HuEmployees.Where(f => f.ID == p.EMPLOYEE_ID)
                         from m in _dbContext.AtTimeTypes.Where(f => f.ID == p.TIME_TYPE_ID).DefaultIfEmpty()
                         from t in _dbContext.HuPositions.Where(f => f.ID == e.POSITION_ID).DefaultIfEmpty()
                         from o in _dbContext.HuOrganizations.Where(f => f.ID == e.ORG_ID).DefaultIfEmpty()

                         from a in _dbContext.ProcessApproveStatuses.Where(f => f.ID_REGGROUP == p.ID_REGGROUP && f.APP_LEVEL == (_dbContext.ProcessApproveStatuses.Where(h => h.ID_REGGROUP == p.ID_REGGROUP && h.APP_STATUS == 0).Min(k => k.APP_LEVEL)))
                         from c in _dbContext.ProcessApprovePoses.Where(f => f.PROCESS_APPROVE_ID == a.PROCESS_APPROVE_ID).DefaultIfEmpty()

                             /*from pa in _dbContext.ProcessApproves.Where(pa => pa.ID == a.PROCESS_APPROVE_ID).DefaultIfEmpty()
                             from sp in _dbContext.SeProcesss.Where(sp => sp.ID == pa.PROCESS_ID).DefaultIfEmpty()
                             from sa in _dbContext.SeAuthorizeApproves.Where(sa => sa.PROCESS_ID == sp.ID && sa.FROM_DATE <= currentDate && currentDate <= sa.TO_DATE).DefaultIfEmpty()
                             from es in _dbContext.HuEmployees.Where(es => es.ID == sa.EMPLOYEE_ID).DefaultIfEmpty()*/

                         from pa in _dbContext.ProcessApproves.Where(pa => pa.ID == a.PROCESS_APPROVE_ID).DefaultIfEmpty()
                         from sp in _dbContext.SeProcesss.Where(sp => sp.ID == pa.PROCESS_ID).DefaultIfEmpty()
                         from n in _dbContext.AtNotifications.Where(n => n.REF_ID == p.ID).DefaultIfEmpty()
                         from ap in _dbContext.SeAuthorizeApproves.Where(ap => n.EMP_NOTIFY_ID!.Contains(ap.EMPLOYEE_ID.ToString()!) && ap.EMPLOYEE_AUTH_ID == empId && ap.PROCESS_ID == sp.ID && ((ap.FROM_DATE <= currentDate && currentDate <= ap.TO_DATE) || ap.IS_PER_REPLACE == true)).DefaultIfEmpty()
                         from es in _dbContext.HuEmployees.Where(es => es.ID == ap.EMPLOYEE_AUTH_ID).DefaultIfEmpty()
                         from s in _dbContext.SysOtherLists.AsNoTracking().Where(s => s.ID == p.EXPLAIN_REASON).DefaultIfEmpty()
                             // detail register off
                             /*select new
                             {
                                 Id = p.ID,
                                 IdProcess = a.ID,
                                 Idreg = a.ID_REGGROUP,
                                 Applevel = (_dbContext.ProcessApproveStatuses.Where(h => h.ID_REGGROUP == p.ID_REGGROUP && h.APP_STATUS == 0).Min(k => k.APP_LEVEL)),
                                 paid = a.PROCESS_APPROVE_ID,
                                 ProcessApprovePosesId = c.ID,
                                 ProcessApprovePosesPOS = c.POS_ID,
                                 uyquyen = ap.EMPLOYEE_ID,
                                 uyquyenid = ap.ID == null ? "" : ap.ID.ToString(),
                                 pos = es.POSITION_ID,
                                 apemp = ap.EMPLOYEE_ID,
                                 nemp = n.EMP_NOTIFY_ID,
                                 code = p.TYPE_CODE,
                                 status = a.APP_STATUS,
                                 positionId = positionId
                             };*/
                         where model.Statuses!.Contains(a.APP_STATUS!.Value) && p.TYPE_CODE == model.TypeCode && ((es.POSITION_ID == positionId) || (c.POS_ID == positionId) || (c.IS_DIRECT_MANAGER == true && lstEmpLM.Contains(p.EMPLOYEE_ID)) || (c.IS_MNG_SUPERIOR_DEPARTMENTS == true && lstEmpAffDep.Contains(p.EMPLOYEE_ID))
                          || (c.IS_DIRECT_MNG_OF_DIRECT_MNG == true && lstEmpCSM.Contains(p.EMPLOYEE_ID)) || (c.IS_MNG_AFFILIATED_DEPARTMENTS == true && lstEmpSupDep.Contains(p.EMPLOYEE_ID))) && ((model.DateStartSearch!.Value.Date <= p.DATE_START!.Value.Date && p.DATE_START!.Value.Date <= model.DateEndSearch!.Value.Date) || (model.DateStartSearch!.Value.Date <= p.WORKING_DAY!.Value.Date && p.WORKING_DAY!.Value.Date <= model.DateEndSearch!.Value.Date))
                         orderby p.CREATED_DATE descending
                         select new
                         {
                             Id = p.ID,
                             EmployeeCode = e.CODE,
                             EmployeeName = e.Profile!.FULL_NAME,
                             PositionName = t.NAME,
                             OrgName = o.NAME,
                             SendDate = p.CREATED_DATE == null ? p.CREATED_DATE : p.CREATED_DATE!.Value.AddHours(7),
                             TimeTypeId = p.TIME_TYPE_ID,
                             TypeCode = m.CODE,
                             TimeTypeName = m.NAME,
                             Note = p.NOTE,
                             ReasonExplain = s.NAME,
                             DateStart = p.DATE_START == null ? p.DATE_START : p.DATE_START!.Value.AddHours(7),
                             DateEnd = p.DATE_END == null ? p.DATE_END : p.DATE_END!.Value.AddHours(7),
                             AppLevel = a.APP_LEVEL,
                             TimeStart = p.TIME_START == null ? p.TIME_START : p.TIME_START!.Value.AddHours(7),
                             TimeEnd = p.TIME_END == null ? p.TIME_END : p.TIME_END!.Value.AddHours(7),
                             TypeId = p.TYPE_ID,
                             Late = p.TIME_LATE ?? 0,
                             Comebackout = p.TIME_EARLY ?? 0,
                             TotalDay = p.TOTAL_DAY,
                             TotalOt = p.TOTAL_OT,
                             TotalTimeOt = p.TOTAL_OT == null ? "" : (TimeSpan.FromMinutes((double)p.TOTAL_OT!.Value).ToString(@"hh\:mm")),
                             WorkingDay = p.WORKING_DAY,
                             StatusApprove = p.STATUS_ID == null ? CommonMessageCode.WAITING_APPROVED : (p.STATUS_ID == 1 ? CommonMessageCode.APPROVED : CommonMessageCode.REJECTED),
                             IsEachDay = p.IS_EACH_DAY,
                             ListDetail = (from d in _dbContext.PortalRegisterOffDetails.Where(d => d.REGISTER_ID == p.ID).DefaultIfEmpty()
                                           from t in _dbContext.AtTimeTypes.Where(t => t.ID == d.MANUAL_ID).DefaultIfEmpty()
                                           from s in _dbContext.AtShifts.Where(s => s.ID == d.SHIFT_ID).DefaultIfEmpty()
                                           from to in _dbContext.SysOtherLists.Where(x => x.ID == d.TYPE_OFF).DefaultIfEmpty()
                                           select new
                                           {
                                               Date = d.LEAVE_DATE,
                                               DType = to.NAME,
                                               Number = d.NUMBER_DAY,
                                               ShiftName = s.NAME,
                                               ShiftCode = t.CODE,
                                           }).ToList()
                         };
            if (joined != null)
            {
                return new FormatedResponse() { InnerBody = joined };
            }
            else
            {
                return new FormatedResponse() { MessageCode = CommonMessageCode.ENTITY_NOT_FOUND, ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400 };
            }
        }


        public async Task<FormatedResponse> GetByIdVer2(string id, PortalApproveLeaveDTO model)
        {
            var currentDate = DateTime.Now;
            var empId = (from p in _dbContext.SysUsers where p.ID == id select p.EMPLOYEE_ID).FirstOrDefault();             // 34235
            var joined = from p in _dbContext.PortalRegisterOffs
                         from e in _dbContext.HuEmployees.Where(f => f.ID == p.EMPLOYEE_ID)
                         from m in _dbContext.AtTimeTypes.Where(f => f.ID == p.TIME_TYPE_ID).DefaultIfEmpty()
                         from t in _dbContext.HuPositions.Where(f => f.ID == e.POSITION_ID).DefaultIfEmpty()
                         from o in _dbContext.HuOrganizations.Where(f => f.ID == e.ORG_ID).DefaultIfEmpty()

                         from a in _dbContext.ProcessApproveStatuses.Where(f => f.ID_REGGROUP == p.ID_REGGROUP && f.APP_LEVEL == (_dbContext.ProcessApproveStatuses.Where(h => h.ID_REGGROUP == p.ID_REGGROUP && h.APP_STATUS == 0).Min(k => k.APP_LEVEL)))
                         from c in _dbContext.ProcessApprovePoses.Where(f => f.PROCESS_APPROVE_ID == a.PROCESS_APPROVE_ID).DefaultIfEmpty()

                             /*from pa in _dbContext.ProcessApproves.Where(pa => pa.ID == a.PROCESS_APPROVE_ID).DefaultIfEmpty()
                             from sp in _dbContext.SeProcesss.Where(sp => sp.ID == pa.PROCESS_ID).DefaultIfEmpty()
                             from sa in _dbContext.SeAuthorizeApproves.Where(sa => sa.PROCESS_ID == sp.ID && sa.FROM_DATE <= currentDate && currentDate <= sa.TO_DATE).DefaultIfEmpty()
                             from es in _dbContext.HuEmployees.Where(es => es.ID == sa.EMPLOYEE_ID).DefaultIfEmpty()*/

                         from pa in _dbContext.ProcessApproves.Where(pa => pa.ID == a.PROCESS_APPROVE_ID).DefaultIfEmpty()
                         from sp in _dbContext.SeProcesss.Where(sp => sp.ID == pa.PROCESS_ID).DefaultIfEmpty()
                         from n in _dbContext.AtNotifications.Where(n => n.REF_ID == p.ID).DefaultIfEmpty()
                         from ap in _dbContext.SeAuthorizeApproves.Where(ap => n.EMP_NOTIFY_ID!.Contains(ap.EMPLOYEE_ID.ToString()!) && ap.EMPLOYEE_AUTH_ID == empId && ap.PROCESS_ID == sp.ID && ((ap.FROM_DATE <= currentDate && currentDate <= ap.TO_DATE) || ap.IS_PER_REPLACE == true)).DefaultIfEmpty()
                         from es in _dbContext.HuEmployees.Where(es => es.ID == ap.EMPLOYEE_AUTH_ID).DefaultIfEmpty()
                         from s in _dbContext.SysOtherLists.AsNoTracking().Where(s => s.ID == p.EXPLAIN_REASON).DefaultIfEmpty()

                             // detail register off
                             /*select new
                             {
                                 Id = p.ID,
                                 IdProcess = a.ID,
                                 Idreg = a.ID_REGGROUP,
                                 Applevel = (_dbContext.ProcessApproveStatuses.Where(h => h.ID_REGGROUP == p.ID_REGGROUP && h.APP_STATUS == 0).Min(k => k.APP_LEVEL)),
                                 paid = a.PROCESS_APPROVE_ID,
                                 ProcessApprovePosesId = c.ID,
                                 ProcessApprovePosesPOS = c.POS_ID,
                                 uyquyen = ap.EMPLOYEE_ID,
                                 uyquyenid = ap.ID == null ? "" : ap.ID.ToString(),
                                 pos = es.POSITION_ID,
                                 apemp = ap.EMPLOYEE_ID,
                                 nemp = n.EMP_NOTIFY_ID,
                                 code = p.TYPE_CODE,
                                 status = a.APP_STATUS,
                                 positionId = positionId
                             };*/
                         where model.Statuses!.Contains(a.APP_STATUS!.Value) && p.TYPE_CODE == model.TypeCode && n.EMP_NOTIFY_ID.Contains(empId.ToString()) && ((model.DateStartSearch!.Value.Date <= p.DATE_START!.Value.Date && p.DATE_START!.Value.Date <= model.DateEndSearch!.Value.Date) || (model.DateStartSearch!.Value.Date <= p.WORKING_DAY!.Value.Date && p.WORKING_DAY!.Value.Date <= model.DateEndSearch!.Value.Date))
                         orderby p.CREATED_DATE descending
                         select new
                         {
                             Id = p.ID,
                             EmployeeCode = e.CODE,
                             EmployeeName = e.Profile!.FULL_NAME,
                             PositionName = t.NAME,
                             OrgName = o.NAME,
                             SendDate = p.CREATED_DATE == null ? p.CREATED_DATE : p.CREATED_DATE!.Value.AddHours(7),
                             TimeTypeId = p.TIME_TYPE_ID,
                             TypeCode = m.CODE,
                             TimeTypeName = m.NAME,
                             Note = p.NOTE,
                             ReasonExplain = s.NAME,
                             DateStart = p.DATE_START == null ? p.DATE_START : p.DATE_START!.Value.AddHours(7),
                             DateEnd = p.DATE_END == null ? p.DATE_END : p.DATE_END!.Value.AddHours(7),
                             AppLevel = a.APP_LEVEL,
                             TimeStart = p.TIME_START == null ? p.TIME_START : p.TIME_START!.Value.AddHours(7),
                             TimeEnd = p.TIME_END == null ? p.TIME_END : p.TIME_END!.Value.AddHours(7),
                             TypeId = p.TYPE_ID,
                             Late = p.TIME_LATE ?? 0,
                             Comebackout = p.TIME_EARLY ?? 0,
                             TotalDay = p.TOTAL_DAY,
                             TotalOt = p.TOTAL_OT,
                             TotalTimeOt = p.TOTAL_OT == null ? "" : (TimeSpan.FromMinutes((double)p.TOTAL_OT!.Value).ToString(@"hh\:mm")),
                             WorkingDay = p.WORKING_DAY,
                             StatusApprove = p.STATUS_ID == null ? CommonMessageCode.WAITING_APPROVED : (p.STATUS_ID == 1 ? CommonMessageCode.APPROVED : CommonMessageCode.REJECTED),
                             IsEachDay = p.IS_EACH_DAY,
                             ListDetail = (from d in _dbContext.PortalRegisterOffDetails.Where(d => d.REGISTER_ID == p.ID).DefaultIfEmpty()
                                           from t in _dbContext.AtTimeTypes.Where(t => t.ID == d.MANUAL_ID).DefaultIfEmpty()
                                           from s in _dbContext.AtShifts.Where(s => s.ID == d.SHIFT_ID).DefaultIfEmpty()
                                           from to in _dbContext.SysOtherLists.Where(x => x.ID == d.TYPE_OFF).DefaultIfEmpty()
                                           select new
                                           {
                                               Date = d.LEAVE_DATE,
                                               DType = to.NAME,
                                               Number = d.NUMBER_DAY,
                                               ShiftName = s.NAME,
                                               ShiftCode = t.CODE,
                                           }).ToList()
                         };
            if (joined != null)
            {
                return new FormatedResponse() { InnerBody = joined };
            }
            else
            {
                return new FormatedResponse() { MessageCode = CommonMessageCode.ENTITY_NOT_FOUND, ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400 };
            }
        }

        public async Task<FormatedResponse> GetPortalApproveById(long id)
        {
            var result = (from p in _dbContext.PortalRegisterOffs
                         from e in _dbContext.HuEmployees.Where(f => f.ID == p.EMPLOYEE_ID).DefaultIfEmpty()

                         from m in _dbContext.AtTimeTypes.Where(f => f.ID == p.TIME_TYPE_ID).DefaultIfEmpty()
                         from t in _dbContext.HuPositions.Where(f => f.ID == e.POSITION_ID).DefaultIfEmpty()
                         from o in _dbContext.HuOrganizations.Where(f => f.ID == e.ORG_ID).DefaultIfEmpty()

                         from a in _dbContext.ProcessApproveStatuses.Where(f => f.ID_REGGROUP == p.ID_REGGROUP && f.APP_LEVEL == (_dbContext.ProcessApproveStatuses.Where(h => h.ID_REGGROUP == p.ID_REGGROUP && h.APP_STATUS == 0).Min(k => k.APP_LEVEL))).DefaultIfEmpty()
                         from c in _dbContext.ProcessApprovePoses.Where(f => f.PROCESS_APPROVE_ID == a.PROCESS_APPROVE_ID).DefaultIfEmpty()
                         from s in _dbContext.SysOtherLists.AsNoTracking().Where(s => s.ID == p.EXPLAIN_REASON).DefaultIfEmpty()
                          where p.ID == id
                         select new 
                         {
                             Id = p.ID,
                             EmployeeCode = e.CODE,
                             EmployeeName = e.Profile!.FULL_NAME,
                             PositionName = t.NAME,
                             OrgName = o.NAME,
                             SendDate = p.CREATED_DATE == null ? p.CREATED_DATE : p.CREATED_DATE!.Value.AddHours(7),
                             TimeTypeId = p.TIME_TYPE_ID,
                             TypeCode = m.CODE,
                             TimeTypeName = m.NAME,
                             Note = p.NOTE,
                             ReasonExplain = s.NAME,
                             DateStart = p.DATE_START == null ? p.DATE_START : p.DATE_START!.Value.AddHours(7),
                             DateEnd = p.DATE_END == null ? p.DATE_END : p.DATE_END!.Value.AddHours(7),
                             AppLevel = a.APP_LEVEL ?? 0,
                             TimeStart = p.TIME_START == null ? p.TIME_START : p.TIME_START!.Value.AddHours(7),
                             TimeEnd = p.TIME_END == null ? p.TIME_END :p.TIME_END!.Value.AddHours(7),
                             TypeId = p.TYPE_ID,
                             Late = p.TIME_LATE ?? 0,
                             Comebackout = p.TIME_EARLY ?? 0,
                             TotalDay = p.TOTAL_DAY,
                             TotalOt = p.TOTAL_OT,
                             TotalTimeOt = p.TOTAL_OT == null ? "" : (TimeSpan.FromMinutes((double)p.TOTAL_OT!.Value).ToString(@"hh\:mm")),
                             WorkingDay = p.WORKING_DAY,
                             StatusApprove = p.STATUS_ID == null ? CommonMessageCode.WAITING_APPROVED : (p.STATUS_ID == 1 ? CommonMessageCode.APPROVED : CommonMessageCode.REJECTED),
                             IsEachDay = p.IS_EACH_DAY,
                             ListDetail = (from d in _dbContext.PortalRegisterOffDetails.Where(d => d.REGISTER_ID == p.ID).DefaultIfEmpty()
                                           from t in _dbContext.AtTimeTypes.Where(t => t.ID == d.MANUAL_ID).DefaultIfEmpty()
                                           from s in _dbContext.AtShifts.Where(s => s.ID == d.SHIFT_ID).DefaultIfEmpty()
                                           from to in _dbContext.SysOtherLists.Where(x => x.ID == d.TYPE_OFF).DefaultIfEmpty()
                                           select new
                                           {
                                               Date = d.LEAVE_DATE,
                                               DType = to.NAME,
                                               Number = d.NUMBER_DAY,
                                               ShiftName = s.NAME,
                                               ShiftCode = t.CODE,
                                           }).ToList()
                         }).Take(1);
            return new FormatedResponse() { InnerBody = result};
        }

        public async Task<FormatedResponse> Create(GenericUnitOfWork _uow, PortalApproveLeaveDTO dto, string sid)
        {
            var response = await _genericRepository.Create(_uow, dto, sid);
            return response;
        }
        public async Task<FormatedResponse> GetById(long id)
        {
            var response = await _genericRepository.GetById(id);
            return response;
        }

        public async Task<FormatedResponse> CreateRange(GenericUnitOfWork _uow, List<PortalApproveLeaveDTO> dtos, string sid)
        {
            var add = new List<PortalApproveLeaveDTO>();
            add.AddRange(dtos);
            var response = await _genericRepository.CreateRange(_uow, add, sid);
            return response;
        }

        public async Task<FormatedResponse> Update(GenericUnitOfWork _uow, PortalApproveLeaveDTO dto, string sid, bool patchMode = true)
        {
            var response = await _genericRepository.Update(_uow, dto, sid, patchMode);
            return response;
        }

        public async Task<FormatedResponse> Approve(GenericUnitOfWork _uow, PortalApproveLeaveDTO dto, string sid, bool patchMode = true)
        {
            var empId = await (from p in _dbContext.PortalRegisterOffs where p.ID == dto.Id select p.EMPLOYEE_ID).FirstOrDefaultAsync();

            //var user = _dbContext.SysActions 

            var empAppId = (from p in _dbContext.SysUsers where p.ID == sid select p.EMPLOYEE_ID).FirstOrDefault();
            var reggroupId = (from p in _dbContext.PortalRegisterOffs where p.ID == dto.Id select p.ID_REGGROUP).FirstOrDefault();
            var obj = new
            {
                P_EMPLOYEE_APP_ID = empAppId,
                P_EMPLOYEE_ID = empId,
                P_STATUS_ID = dto.AppStatus,
                P_PROCESS_TYPE = dto.TypeCode,
                P_NOTE = dto.Note,
                P_ID_REGGROUP = reggroupId,
                P_APP_LEVEL = dto.AppLevel
            };
            var data = QueryData.ExecuteStoreToTable(Procedures.PKG_AT_PROCESS_PRI_PROCESS, obj, false);
            if (Convert.ToDouble(data.Tables[0].Rows[0][0]) > 0)
            {
                var msg = dto.AppStatus == 1 ? CommonMessageCode.APPROVED_SUCCESSFULLY : CommonMessageCode.REJECTED_SUCCESSFULLY;
                //await _hubContext.Clients.User(username).SendAsync("ReceiveMessage", new
                //{
                //    SignalType = "TASK_PROGRESS",
                //    Message = "APP ĐANG XỬ LÝ TÁC VỤ, XIN VUI LÒNG ĐỢI..."/*message*/,
                //    Data = new
                //    {
                //        OuterMessage = outerMessage,
                //        OuterPercent = curValue.ToString() + "%",
                //        InnerMessage = innerMessage,
                //        InnerPercent = Math.Round((innerProportion * 100), 0).ToString() + "%"
                //    }
                //});

                var employeeId = data.Tables[0].Rows[0][0].ToString();
                var employeeIds = employeeId.Split(",");
                if (employeeIds?.Count() > 0)
                {
                    for (var i = 0; i < employeeIds.Count(); i++)
                    {
                        var user = await _dbContext.SysUsers.Where(x => x.EMPLOYEE_ID == Int64.Parse(employeeIds[i])).FirstOrDefaultAsync();
                        var username = user?.USERNAME;
                        if (!string.IsNullOrEmpty(username))
                        {
                            await _hubContext.Clients.User(username).SendAsync("ReceiveMessage", new
                            {
                                SignalType = "APPROVE_NOTIFICATION",
                                Message = "Bạn có thông báo mới trên Portal"/*message*/,
                                Data = new
                                {

                                }
                            });
                        }
                    }
                }



                return new FormatedResponse() { InnerBody = data.Tables[0].Rows[0][0], StatusCode = EnumStatusCode.StatusCode200, ErrorType = EnumErrorType.CATCHABLE, MessageCode = msg };
            }
            else
            {
                return new FormatedResponse() { InnerBody = data.Tables[0].Rows[0][0], StatusCode = EnumStatusCode.StatusCode400, ErrorType = EnumErrorType.CATCHABLE };
            }
        }

        public async Task<FormatedResponse> UpdateRange(GenericUnitOfWork _uow, List<PortalApproveLeaveDTO> dtos, string sid, bool patchMode = true)
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
        public virtual async Task<FormatedResponse> ToggleActiveIds(GenericUnitOfWork _uow, List<long> ids, bool valueToBind, string sid)
        {
            await Task.Run(() => null);
            throw new NotImplementedException();
        }

        public Task<FormatedResponse> GetById(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<FormatedResponse> ApproveHistory(string id, DateTime fromDate, DateTime toDate)
        {
            var empId = (from p in _dbContext.SysUsers where p.ID == id select p.EMPLOYEE_ID).FirstOrDefault();

            var listApprove = await (from pas in _dbContext.ProcessApproveStatuses.AsNoTracking().Where(pas => pas.EMPLOYEE_APPROVED == empId).DefaultIfEmpty()
                                     from r in _dbContext.PortalRegisterOffs.AsNoTracking().Where(r => r.ID_REGGROUP == pas.ID_REGGROUP)
                                     from e in _dbContext.HuEmployees.AsNoTracking().Where(e => e.ID == pas.EMPLOYEE_ID).DefaultIfEmpty()
                                     from p in _dbContext.HuPositions.AsNoTracking().Where(p => p.ID == e.POSITION_ID).DefaultIfEmpty()
                                     from o in _dbContext.HuOrganizations.AsNoTracking().Where(o => o.ID == e.ORG_ID).DefaultIfEmpty()
                                     from t in _dbContext.AtTimeTypes.AsNoTracking().Where(t => r.TIME_TYPE_ID == t.ID).DefaultIfEmpty()
                                     from s in _dbContext.SysOtherLists.AsNoTracking().Where(s => s.ID == r.EXPLAIN_REASON).DefaultIfEmpty()
                                     where pas.APP_DATE >= fromDate && pas.APP_DATE <= toDate
                                     orderby pas.ID descending
                                     select new
                                     {
                                         Id = pas.ID,
                                         RegisterCode = e.CODE,
                                         RegisterName = e.Profile!.FULL_NAME,
                                         RegisterPos = p.NAME,
                                         RegisterOrg = o.NAME,
                                         TypeId = r.TYPE_ID,
                                         TimeTypeId = r.TIME_TYPE_ID,
                                         TimeTypeName = t.NAME,
                                         IsOff = r.TYPE_CODE == "OFF" ? true : false,
                                         IsOvertime = r.TYPE_CODE == "OVERTIME" ? true : false,
                                         IsExplainWork = r.TYPE_CODE == "EXPLAINWORK" ? true : false,
                                         ProcessType = pas.PROCESS_TYPE == "OFF" ? CommonMessageCode.REGISTER_OFF : (pas.PROCESS_TYPE == "OVERTIME" ? CommonMessageCode.REGISTER_OVERTIME : CommonMessageCode.REGISTER_EXPLAIN_WORK),
                                         AppStatusId = pas.APP_STATUS,
                                         AppStatus = pas.APP_STATUS == 0 ? CommonMessageCode.WAITING_APPROVED : (pas.APP_STATUS == 1 ? CommonMessageCode.APPROVED : CommonMessageCode.REJECTED),
                                         ApproveDate = pas.APP_DATE,
                                         ApproveNote = pas.APP_NOTE,
                                         AppLevel = pas.APP_LEVEL,
                                         WorkingDay = r.WORKING_DAY,
                                         TotalOt = r.TOTAL_OT,
                                         TimeStart = r.TIME_START == null ? r.TIME_START : r.TIME_START!.Value.AddHours(7),
                                         TimeEnd = r.TIME_END == null ? r.TIME_END : r.TIME_END!.Value.AddHours(7),
                                         TotalDay = r.TOTAL_DAY,
                                         DateStart = r.DATE_START == null ? r.DATE_START : r.DATE_START!.Value.AddHours(7),
                                         DateEnd = r.DATE_END == null ? r.DATE_END : r.DATE_END!.Value.AddHours(7),
                                         Reason = r.NOTE,
                                         ReasonExplain = s.NAME,
                                         IsEachDay = r.IS_EACH_DAY,
                                         SendDate = r.CREATED_DATE == null ? r.CREATED_DATE : r.CREATED_DATE!.Value.AddHours(7),
                                         ListDetail = (from d in _dbContext.PortalRegisterOffDetails.Where(d => d.ID_REGGROUP == pas.ID_REGGROUP).DefaultIfEmpty()
                                                       from t in _dbContext.AtTimeTypes.Where(t => t.ID == d.MANUAL_ID).DefaultIfEmpty()
                                                       from s in _dbContext.AtShifts.Where(s => s.ID == d.SHIFT_ID).DefaultIfEmpty()
                                                       from to in _dbContext.SysOtherLists.Where(x => x.ID == d.TYPE_OFF).DefaultIfEmpty()
                                                       select new
                                                       {
                                                           Date = d.LEAVE_DATE,
                                                           DType = to.NAME,
                                                           Number = d.NUMBER_DAY,
                                                           ShiftName = s.NAME,
                                                           ShiftCode = t.CODE,
                                                       }).ToList()
                                     }).ToListAsync();
            return new FormatedResponse() { InnerBody = listApprove };
        }
    }
}

