using CORE.GenericUOW;
using CORE.DTO;
using API.DTO;
using CORE.Enum;
using CORE.StaticConstant;
using API.All.DbContexts;
using CORE.AutoMapper;
using Common.Extensions;
using System;
using Common.Interfaces;
using Common.DataAccess;
using System.Data;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Logical;

namespace API.Controllers.SeReminder
{
    public class SeReminderRepository : ISeReminderRepository
    {
        private readonly GenericUnitOfWork _uow;
        private readonly FullDbContext _dbContext;
        private IGenericRepository<SE_REMINDER, SeReminderDTO> _genericRepository;
        private readonly GenericReducer<SE_REMINDER, SeReminderDTO> _genericReducer;
        protected AbsQueryDataTemplate QueryData;
        public SeReminderRepository(FullDbContext context, GenericUnitOfWork uow)
        {
            _dbContext = context;
            _uow = uow;
            _genericRepository = _uow.GenericRepository<SE_REMINDER, SeReminderDTO>();
            _genericReducer = new();
            QueryData = new SqlQueryDataTemplate(_dbContext);
        }

        public async Task<GenericPhaseTwoListResponse<SeReminderDTO>> SinglePhaseQueryList(GenericQueryListDTO<SeReminderDTO> request)
        {
            var joined = from p in _dbContext.SeReminders.AsNoTracking()
                             // JOIN OTHER ENTITIES BASED ON THE BUSINESS
                         select new SeReminderDTO
                         {
                             Id = p.ID
                         };

            var singlePhaseResult = await _genericReducer.SinglePhaseReduce(joined, request);
            return singlePhaseResult;
        }

        public async Task<FormatedResponse> ReadAll()
        {
            var result = await _genericRepository.ReadAll();
            return result;
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
                var list = new List<SE_REMINDER>
                    {
                        (SE_REMINDER)response
                    };
                var joined = (from l in list
                                  // JOIN OTHER ENTITIES BASED ON THE BUSINESS
                              select new SeReminderDTO
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

        public async Task<FormatedResponse> Create(GenericUnitOfWork _uow, SeReminderDTO dto, string sid)
        {
            var response = await _genericRepository.Create(_uow, dto, sid);
            return response;
        }

        public async Task<FormatedResponse> CreateRange(GenericUnitOfWork _uow, List<SeReminderDTO> dtos, string sid)
        {
            dtos.ForEach(item =>
            {
                var tryToFind = (from a in _dbContext.SysOtherLists
                                 from b in _dbContext.SysOtherListTypes.Where(x => x.ID == a.TYPE_ID).DefaultIfEmpty()
                                 where a.CODE == item.Code && b.CODE == "WARNING_TYPE"
                                 select new
                                 {
                                     Id = a.ID
                                 }).SingleOrDefault();
                if (tryToFind != null)
                {
                    item.SysOtherlistId = tryToFind.Id;
                    /*item.Id = null;*/ // no matter null or zero 
                }
            });

            var response = await _genericRepository.CreateRange(_uow, dtos, sid);
            return response;
        }

        public async Task<FormatedResponse> Update(GenericUnitOfWork _uow, SeReminderDTO dto, string sid, bool patchMode = true)
        {
            var response = await _genericRepository.Update(_uow, dto, sid, patchMode);
            return response;
        }

        public async Task<FormatedResponse> UpdateRange(GenericUnitOfWork _uow, List<SeReminderDTO> dtos, string sid, bool patchMode = true)
        {
            dtos.ForEach(item =>
            {
                var tryToFind = (from a in _dbContext.SysOtherLists
                                 from r in _dbContext.SeReminders.Where(x => x.CODE == a.CODE).DefaultIfEmpty()
                                 from b in _dbContext.SysOtherListTypes.Where(x => x.ID == a.TYPE_ID).DefaultIfEmpty()
                                 where r.CODE == item.Code && b.CODE == "WARNING_TYPE"
                                 select new
                                 {
                                     Id = a.ID,
                                     Idr = r.ID
                                 }).SingleOrDefault();
                if (tryToFind != null)
                {
                    item.SysOtherlistId = tryToFind.Id;
                    item.Id = tryToFind.Idr;
                    /*item.Id = null;*/ // no matter null or zero 
                }
            });

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

        public async Task<FormatedResponse> GetRemind()
        {
            try
            {
                var listRemind = await (from p in _dbContext.SeReminders
                                        from o in _dbContext.SysOtherLists.Where(x => x.ID == p.SYS_OTHERLIST_ID)
                                        from t in _dbContext.SysOtherListTypes.Where(x => x.ID == o.TYPE_ID)
                                        where p.IS_ACTIVE == true
                                        select new SeReminderPushDTO
                                        {
                                            Name = o.NAME,
                                            Code = o.CODE,
                                            Day = p.VALUE
                                        }
                            ).ToArrayAsync();
                var _dayofyear = DateTime.Now.DayOfYear;
                var _dayNow = DateTime.Now.Date;

                //lay phan quyen theo phong ban
                var orgIds = await QueryData.ExecuteList(Procedures.PKG_COMMON_LIST_ORG,
                     new
                     {
                         P_IS_ADMIN = _dbContext.IsAdmin == true ? 1 : 0,
                         P_ORG_ID = 0,
                         P_CURENT_USER_ID = _dbContext.CurrentUserId,
                     }, false);
                //var orgIds = await QueryData.ExecuteList("AppSettings:ConnectionStrings:CoreDb",
                //    Procedures.PKG_COMMON_LIST_ORG, new
                //    {
                //         P_IS_ADMIN = _dbContext.IsAdmin == true ? 1 : 0,
                //         P_ORG_ID = 0,
                //         P_CURENT_USER_ID = _dbContext.CurrentUserId,
                //    });
                List<long?> ids = orgIds.Select(c => (long?)((dynamic)c).ID).ToList();
                
                foreach (var item in listRemind)
                {
                    switch (item.Code)
                    {
                        case SystemConfig.WARN01:// Het han hop dong thu viec
                            try
                            {
                                var _a = await (from p in _dbContext.HuContracts.Where(c => c.EXPIRE_DATE != null && c.STATUS_ID == OtherConfig.STATUS_APPROVE)
                                                join e in _dbContext.HuEmployees on p.EMPLOYEE_ID equals e.ID
                                                join l in _dbContext.HuContractTypes on p.CONTRACT_TYPE_ID equals l.ID
                                                join o in _dbContext.SysContractTypes on l.TYPE_ID equals o.ID
                                                where ((DateTime)p.EXPIRE_DATE).DayOfYear - _dayofyear <= item.Day && ((DateTime)p.EXPIRE_DATE).DayOfYear - _dayofyear >= 0
                                                   && e.WORK_STATUS_ID != OtherConfig.EMP_STATUS_TERMINATE
                                                   && o.CODE == "HDTV"
                                                  && ids.Contains(e.ORG_ID)
                                                select new ReminderParam
                                                {
                                                    Id = (long)p.ID,
                                                    Name = e.Profile!.FULL_NAME,
                                                    Code = e.CODE,
                                                    Avatar = e.Profile!.AVATAR
                                                }).ToArrayAsync();
                                item.Value = new List<ReminderParam>();
                                item.Value.AddRange(_a);
                                item.Count = _a.Count();
                                break;
                            }
                            catch { break; }
                        case SystemConfig.WARN02:// Nhân viên hết hạn hợp đồng chính thức //theo thang
                            try
                            {
                                var _b = await (from p in _dbContext.HuContracts.Where(c => c.EXPIRE_DATE != null && c.STATUS_ID == OtherConfig.STATUS_APPROVE)
                                                join e in _dbContext.HuEmployees on p.EMPLOYEE_ID equals e.ID
                                                join l in _dbContext.HuContractTypes on p.CONTRACT_TYPE_ID equals l.ID
                                                join o in _dbContext.SysContractTypes on l.TYPE_ID equals o.ID
                                                where ((DateTime)p.EXPIRE_DATE) <= _dayNow.AddMonths(item.Day.Value)
                                                    && ((DateTime)p.EXPIRE_DATE) >= _dayNow
                                                    && item.Day != 0
                                                    && e.WORK_STATUS_ID != OtherConfig.EMP_STATUS_TERMINATE
                                                    && o.CODE != "HDTV" && o.CODE != "HDHV"
                                                    && ids.Contains(e.ORG_ID)
                                                select new ReminderParam
                                                {
                                                    Id = (long)p.ID,
                                                    Name = e.Profile!.FULL_NAME,
                                                    Code = e.CODE
                                                }).ToArrayAsync();
                                item.Value = new List<ReminderParam>();
                                item.Value.AddRange(_b);
                                item.Count = _b.Count();
                                break;
                            }
                            catch { break; }
                        case SystemConfig.WARN03: //nhan vien sap den sinh nhat
                            try
                            {
                                var _c = await (from p in _dbContext.HuEmployees.Where(c => c.Profile!.BIRTH_DATE != null)
                                                where ((DateTime)p.Profile.BIRTH_DATE).DayOfYear - _dayofyear <= item.Day && ((DateTime)p.Profile!.BIRTH_DATE).DayOfYear - _dayofyear >= 0
                                                 && p.WORK_STATUS_ID != OtherConfig.EMP_STATUS_TERMINATE
                                                && ids.Contains(p.ORG_ID)
                                                select new ReminderParam
                                                {
                                                    Id = (long)p.ID,
                                                    Name = p.Profile!.FULL_NAME,
                                                    Code = p.CODE
                                                }).ToArrayAsync();
                                item.Value = new List<ReminderParam>();
                                item.Value.AddRange(_c);
                                item.Count = _c.Count();
                                break;
                            }
                            catch { break; }
                        case SystemConfig.WARN04: //Nhân viên chưa nộp đủ giấy tờ khi tiếp nhận
                            try
                            {
                                var _countP = (from p in _dbContext.SysOtherLists.Where(p => p.IS_ACTIVE == true)
                                               join ot in _dbContext.SysOtherListTypes on p.TYPE_ID equals ot.ID
                                               where ot.CODE == "PAPER"
                                               select p).Count();
                                var groups = from e in _dbContext.HuEmployees.Where(x => x.WORK_STATUS_ID != OtherConfig.EMP_STATUS_TERMINATE)
                                             from p in _dbContext.HuEmployeePaperss.Where(x => x.EMP_ID == e.ID).DefaultIfEmpty()
                                             group e by new { EmpId = e.ID } into g
                                             select new HuEmployeePapersCountDTO
                                             {
                                                 EmpId = g.Key.EmpId,
                                                 PaperssCount = g.Count()
                                             };
                                var _d = await (from p in groups
                                                join e in _dbContext.HuEmployees on p.EmpId equals e.ID into emp
                                                from empResult in emp.Where(x => x.WORK_STATUS_ID != OtherConfig.EMP_STATUS_TERMINATE && x.JOIN_DATE_STATE.HasValue)
                                                where p.PaperssCount != _countP
                                                && ((DateTime)empResult.JOIN_DATE_STATE).AddDays(item.Day.Value) >= _dayNow
                                                && item.Day != 0
                                                select new ReminderParam
                                                {
                                                    Id = (long)p.EmpId,
                                                    Name = empResult.Profile!.FULL_NAME,
                                                    Code = empResult.CODE
                                                }).ToArrayAsync();
                                item.Value = new List<ReminderParam>();
                                item.Value.AddRange(_d);
                                item.Count = _d.Count();
                                break;
                            }
                            catch { break; }
                        case SystemConfig.WARN05: //Nhân viên nghỉ việc trong tháng
                            try
                            {
                                var _e = await (from p in _dbContext.HuTerminates.Where(c => c.EFFECT_DATE != null && c.STATUS_ID == OtherConfig.STATUS_APPROVE)
                                                join emp in _dbContext.HuEmployees on p.EMPLOYEE_ID equals emp.ID into tmp1
                                                from e2 in tmp1
                                                where ((DateTime)p.EFFECT_DATE).DayOfYear - _dayofyear <= item.Day && ((DateTime)p.EFFECT_DATE).DayOfYear - _dayofyear >= 0
                                                && ids.Contains(e2.ORG_ID)
                                                select new ReminderParam
                                                {
                                                    Id = (long)p.ID,
                                                    Name = e2.Profile!.FULL_NAME,
                                                    Code = e2.CODE
                                                }).ToArrayAsync();
                                item.Value = new List<ReminderParam>();
                                item.Value.AddRange(_e);
                                item.Count = _e.Count();
                                break;
                            }
                            catch
                            {
                                break;
                            }
                        case SystemConfig.WARN06: //Có bảo hiểm tạo mới
                            var _f = await (from p in _dbContext.InsArisings.Where(c => c.EFFECT_DATE != null)
                                            join e in _dbContext.HuEmployees on p.EMPLOYEE_ID equals e.ID
                                            where ((DateTime)p.EFFECT_DATE!) <= _dayNow.AddMonths(item.Day!.Value)
                                                && ((DateTime)p.EFFECT_DATE) >= _dayNow
                                                && item.Day != 0
                                                && e.WORK_STATUS_ID != OtherConfig.EMP_STATUS_TERMINATE
                                                && ids.Contains(e.ORG_ID)
                                            //where ((DateTime)p.EFFECT_DATE!).DayOfYear <= _dayofyear
                                            //    && ((DateTime)p.EFFECT_DATE).DayOfYear >= _dayofyear
                                            //    && item.Day != 0
                                            //    && e.WORK_STATUS_ID != OtherConfig.EMP_STATUS_TERMINATE
                                            //    && ids.Contains(e.ORG_ID)
                                            select new ReminderParam
                                            {
                                                Id = (long)p.ID,
                                                Name = e.Profile!.FULL_NAME,
                                                Code = e.CODE
                                            }).ToArrayAsync();
                            item.Value = new List<ReminderParam>();
                            item.Value.AddRange(_f);
                            item.Count = _f.Count();
                            break; 
                        case SystemConfig.WARN07: //Nhân viên nghỉ thai sản sắp đi làm lại
                            try
                            {
                                var _g = await (from p in _dbContext.AtRegisterLeaves.Where(c => c.DATE_END != null)
                                                join s in _dbContext.AtSymbols on p.TYPE_ID equals s.ID
                                                join e in _dbContext.HuEmployees on p.EMPLOYEE_ID equals e.ID
                                                where ((DateTime)p.DATE_END!).DayOfYear - _dayofyear <= item.Day && ((DateTime)p.DATE_END).DayOfYear - _dayofyear >= 0
                                                   && e.WORK_STATUS_ID != OtherConfig.EMP_STATUS_TERMINATE
                                                  && ids.Contains(e.ORG_ID)
                                                select new ReminderParam
                                                {
                                                    Id = (long)p.ID,
                                                    Name = e.Profile!.FULL_NAME,
                                                    Code = e.CODE
                                                }).ToArrayAsync();
                                item.Value = new List<ReminderParam>();
                                item.Value.AddRange(_g);
                                item.Count = _g.Count();
                                break;
                            }
                            catch { break; }
                        case SystemConfig.WARN08: //Nhân viên đến tuổi nghỉ hưu
                            try
                            {
                                var _h = await (from p in _dbContext.HuTerminates.Where(c => c.EFFECT_DATE != null && c.STATUS_ID == OtherConfig.STATUS_APPROVE && c.TYPE_ID == OtherConfig.TYPE_TER_REASON)
                                                join emp in _dbContext.HuEmployees on p.EMPLOYEE_ID equals emp.ID into tmp1
                                                from e2 in tmp1
                                                join po in _dbContext.HuPositions on e2.POSITION_ID equals po.ID
                                                join st in _dbContext.HuJobs on po.JOB_ID equals st.ID
                                                join ost in _dbContext.SysOtherLists on st.JOB_FAMILY_ID equals ost.ID
                                                where (((DateTime)p.EFFECT_DATE!).Month + ((DateTime)p.EFFECT_DATE!).Year * 12) - (_dayNow.Month + _dayNow.Year * 12) <= item.Day
                                                && (((DateTime)p.EFFECT_DATE!).Month + ((DateTime)p.EFFECT_DATE!).Year * 12) - (_dayNow.Month + _dayNow.Year * 12) >= 0
                                                && ids.Contains(e2.ORG_ID)
                                                && (ost.CODE == "NVTH" || ost.CODE == "LDTT" || ost.CODE == "LDCM")
                                                select new ReminderParam
                                                {
                                                    Id = (long)p.ID,
                                                    Name = e2.Profile!.FULL_NAME,
                                                    Code = e2.CODE
                                                }).ToArrayAsync();

                                item.Value = new List<ReminderParam>();
                                item.Value.AddRange(_h);
                                item.Count = _h.Count();
                                break;
                            }
                            catch (Exception ex)
                            {
                                break;
                            }
                        case SystemConfig.WARN09: //Nhân viên hết hạn chứng chỉ
                            try
                            {
                                var CertifiCates = (from p in _dbContext.HuCertificates
                                                    where (p.EFFECT_TO.HasValue)
                                                    from e in _dbContext.HuEmployees.Where(c => c.WORK_STATUS_ID != OtherConfig.EMP_STATUS_TERMINATE && c.ID == p.EMPLOYEE_ID)
                                                    where
                                                       ((DateTime)p.EFFECT_TO) <= _dayNow.AddMonths(item.Day.Value)
                                                        && ((DateTime)p.EFFECT_TO) >= _dayNow
                                                        && item.Day != 0
                                                       && e.WORK_STATUS_ID != OtherConfig.EMP_STATUS_TERMINATE
                                                      && ids.Contains(e.ORG_ID)
                                                    select new HuCertificateDTO
                                                    {
                                                        Id = p.ID,
                                                        EmployeeId = p.EMPLOYEE_ID,
                                                        EmployeeCode = e.CODE,
                                                        EmployeeFullName = e.Profile.FULL_NAME,
                                                        EffectFrom = p.EFFECT_FROM,
                                                        EffectTo = p.EFFECT_TO,
                                                        Name = p.NAME
                                                    }
                                                    ).ToList();
                                var Group_CertifiCates = new List<HuCertificateDTO>();
                                Group_CertifiCates = CertifiCates.AsEnumerable().GroupBy(g => new { g.EmployeeId, g.Name }).Select(p => p.OrderByDescending(d => d.EffectTo).First()).ToList();
                                item.Value = new List<ReminderParam>();
                                foreach (var p in Group_CertifiCates)
                                {
                                    var _value = new List<ReminderParam>();
                                    _value.Add(new ReminderParam { Id = p.Id, Name = p.EmployeeFullName + " - " + p.Name, Code = p.EmployeeCode });
                                    item.Value.AddRange(_value);
                                }
                                item.Count = Group_CertifiCates.Count();

                                break;
                            }
                            catch
                            {
                                break;
                            }
                        case SystemConfig.WARN10: //Nhân viên hết kiêm nhiệm
                            try
                            {
                                var _j = await (from p in _dbContext.HuConcurrentlys.Where(c => c.EXPIRATION_DATE != null && c.STATUS_ID == OtherConfig.STATUS_APPROVE)
                                                join e in _dbContext.HuEmployees on p.EMPLOYEE_ID equals e.ID
                                                where ((DateTime)p.EXPIRATION_DATE).DayOfYear - _dayofyear <= item.Day && ((DateTime)p.EXPIRATION_DATE).DayOfYear - _dayofyear >= 0
                                                    && e.WORK_STATUS_ID != OtherConfig.EMP_STATUS_TERMINATE
                                                    && ids.Contains(e.ORG_ID)
                                                select new ReminderParam
                                                {
                                                    Id = (long)e.ID,
                                                    Name = e.Profile!.FULL_NAME,
                                                    Code = e.CODE
                                                }).ToArrayAsync();
                                item.Value = new List<ReminderParam>();
                                item.Value.AddRange(_j);
                                item.Count = _j.Count();

                                break;
                            }
                            catch
                            {
                                break;
                            }
                        case SystemConfig.WARN11: //Nhân viên có người thân hết giảm trừ gia cảnh
                            try
                            {
                                var o = _dbContext.HuFamilys.AsNoTracking().Where(c => c.DEDUCT_TO != null).DefaultIfEmpty();
                                var _k = await (from p in _dbContext.HuFamilys.AsNoTracking().Where(c => c.DEDUCT_TO != null).DefaultIfEmpty()
                                                from e in _dbContext.HuEmployees.AsNoTracking().Where(x => x.ID == p.EMPLOYEE_ID).DefaultIfEmpty()
                                                where ((DateTime)p.DEDUCT_TO).DayOfYear - _dayofyear <= item.Day && ((DateTime)p.DEDUCT_TO).DayOfYear - _dayofyear >= 0
                                                //&& e.WORK_STATUS_ID != OtherConfig.EMP_STATUS_TERMINATE
                                                //&& ids.Contains(e.ORG_ID)
                                                select new ReminderParam
                                                {
                                                    Id = (long)p.ID,
                                                    Name = e.Profile!.FULL_NAME + " - " + p.FULLNAME,
                                                    Code = e.CODE
                                                }).ToArrayAsync();
                                item.Value = new List<ReminderParam>();
                                item.Value.AddRange(_k);
                                item.Count = _k.Count();
                                break;
                            }
                            catch { break; }
                        case SystemConfig.WARN12: ////Lãnh đạo quản lý nghỉ hưu
                            try
                            {
                                var _l = await (from p in _dbContext.HuTerminates.Where(c => c.EFFECT_DATE != null && c.STATUS_ID == OtherConfig.STATUS_APPROVE && c.TYPE_ID == OtherConfig.TYPE_TER_REASON)
                                                join emp in _dbContext.HuEmployees on p.EMPLOYEE_ID equals emp.ID into tmp1
                                                from e2 in tmp1
                                                join po in _dbContext.HuPositions on e2.POSITION_ID equals po.ID
                                                join st in _dbContext.HuJobs on po.JOB_ID equals st.ID
                                                join ost in _dbContext.SysOtherLists on st.JOB_FAMILY_ID equals ost.ID
                                                where (((DateTime)p.EFFECT_DATE).Month + ((DateTime)p.EFFECT_DATE).Year * 12) - (_dayNow.Month + _dayNow.Year * 12) <= item.Day 
                                                && (((DateTime)p.EFFECT_DATE).Month + ((DateTime)p.EFFECT_DATE).Year * 12) - (_dayNow.Month + _dayNow.Year * 12) >= 0
                                                && ids.Contains(e2.ORG_ID)
                                                && (ost.CODE == "LD" || ost.CODE == "NQL")
                                                select new ReminderParam
                                                {
                                                    Id = (long)p.ID,
                                                    Name = e2.Profile!.FULL_NAME,
                                                    Code = e2.CODE
                                                }).ToArrayAsync();
                                item.Value = new List<ReminderParam>();
                                item.Value.AddRange(_l);
                                item.Count = _l.Count();
                                break;
                            }
                            catch (Exception ex)
                            {
                                break;
                            }
                        case SystemConfig.WARN13: //Lãnh đạo bổ nhiệm
                            try
                            {
                                var _m = await (from p in _dbContext.HuWorkings.Where(c => c.EXPIRE_DATE != null && c.STATUS_ID == OtherConfig.STATUS_APPROVE && c.IS_WAGE == null)
                                                join e in _dbContext.HuEmployees on p.EMPLOYEE_ID equals e.ID
                                                join o in _dbContext.SysOtherLists on p.STATUS_ID equals o.ID
                                                join t in _dbContext.SysOtherLists on p.TYPE_ID equals t.ID
                                                join po in _dbContext.HuPositions on e.POSITION_ID equals po.ID
                                                join st in _dbContext.HuJobs on po.JOB_ID equals st.ID
                                                join ost in _dbContext.SysOtherLists on st.JOB_FAMILY_ID equals ost.ID
                                                where ((DateTime)p.EXPIRE_DATE).DayOfYear - _dayofyear <= item.Day && ((DateTime)p.EXPIRE_DATE).DayOfYear - _dayofyear >= 0
                                                   && e.WORK_STATUS_ID != OtherConfig.EMP_STATUS_TERMINATE
                                                   && t.CODE == "BN" //quyet dinh bo nhiem
                                                   && (ost.CODE == "LD" || ost.CODE == "NQL")
                                                  && ids.Contains(e.ORG_ID)
                                                select new ReminderParam
                                                {
                                                    Id = (long)e.ID,
                                                    Name = e.Profile!.FULL_NAME,
                                                    Code = e.CODE
                                                }).ToArrayAsync();
                                item.Value = new List<ReminderParam>();
                                item.Value.AddRange(_m);
                                item.Count = _m.Count();
                                break;
                            }
                            catch { break; }
                        case SystemConfig.WARN14: //nhan vien bo nhiem
                            try
                            {
                                var _n = await (from p in _dbContext.HuVWorkingMaxByTypes.Where(c => c.EXPIRE_DATE != null && c.IS_WAGE == null)
                                                join e in _dbContext.HuEmployees on p.EMPLOYEE_ID equals e.ID
                                                join o in _dbContext.SysOtherLists on p.TYPE_ID equals o.ID
                                                join t in _dbContext.SysOtherLists on p.TYPE_ID equals t.ID
                                                where ((DateTime)p.EXPIRE_DATE).DayOfYear - _dayofyear <= item.Day && ((DateTime)p.EXPIRE_DATE).DayOfYear - _dayofyear >= 0
                                                   && e.WORK_STATUS_ID != OtherConfig.EMP_STATUS_TERMINATE
                                                   && t.CODE == "BN" //quyet dinh bo nhiem
                                                  && ids.Contains(e.ORG_ID)
                                                select new ReminderParam
                                                {
                                                    Id = (long)e.ID,
                                                    Name = e.Profile!.FULL_NAME,
                                                    Code = e.CODE
                                                }).ToArrayAsync();
                                item.Value = new List<ReminderParam>();
                                item.Value.AddRange(_n);
                                item.Count = _n.Count();
                                break; //Nhân viên bổ nhiệm
                            }
                            catch { break; }
                        //case SystemConfig.WARN14: //nhan vien dieu dong/biet phai
                        //    try
                        //    {
                        //        var _n = await (from p in _dbContext.HuVWorkingMaxByTypes.Where(c => c.EXPIRE_DATE != null && c.IS_WAGE == null)
                        //                        join e in _dbContext.HuEmployees on p.EMPLOYEE_ID equals e.ID
                        //                        join o in _dbContext.SysOtherLists on p.TYPE_ID equals o.ID
                        //                        join t in _dbContext.SysOtherLists on p.TYPE_ID equals t.ID
                        //                        join po in _dbContext.HuPositions on e.POSITION_ID equals po.ID
                        //                        join st in _dbContext.HuJobs on po.JOB_ID equals st.ID
                        //                        join ost in _dbContext.SysOtherLists on st.JOB_FAMILY_ID equals ost.ID
                        //                        where ((DateTime)p.EXPIRE_DATE).DayOfYear - _dayofyear <= item.Day && ((DateTime)p.EXPIRE_DATE).DayOfYear - _dayofyear >= 0
                        //                           && e.WORK_STATUS_ID != OtherConfig.EMP_STATUS_TERMINATE
                        //                           && (t.CODE == "BP" || t.CODE == "DDCV" || t.CODE == "DDBN")
                        //                           && (ost.CODE == "NVTH" || ost.CODE == "LDTT" || ost.CODE == "LDCM")
                        //                          && ids.Contains(e.ORG_ID)
                        //                        select new ReminderParam
                        //                        {
                        //                            Id = (long)e.ID,
                        //                            Name = e.Profile!.FULL_NAME,
                        //                            Code = e.CODE
                        //                        }).ToArrayAsync();
                        //        item.Value = new List<ReminderParam>();
                        //        item.Value.AddRange(_n);
                        //        item.Count = _n.Count();
                        //        break; //Nhân viên bổ nhiệm
                        //    }
                        //    catch { break; }
                        case SystemConfig.WARN15:  //Nâng lương
                            try
                            {
                                var _o = await (from w in _dbContext.HuVWageMax.Where(c => c.EXPIRE_UPSAL_DATE != null && c.IS_WAGE != null)
                                                from e in _dbContext.HuEmployees.Where(c => c.WORK_STATUS_ID != OtherConfig.EMP_STATUS_TERMINATE && c.ID == w.EMPLOYEE_ID)
                                                where ((DateTime)w.EXPIRE_UPSAL_DATE).DayOfYear - _dayofyear <= item.Day && ((DateTime)w.EXPIRE_UPSAL_DATE).DayOfYear - _dayofyear >= 0
                                                   && ids.Contains(e.ORG_ID)
                                                select new ReminderParam
                                                {
                                                    Id = (long)w.ID,
                                                    Name = e.Profile!.FULL_NAME,
                                                    Code = e.CODE
                                                }).ToArrayAsync();
                                item.Value = new List<ReminderParam>();
                                item.Value.AddRange(_o);
                                item.Count = _o.Count();
                                break;
                            }
                            catch { break; }
                        case SystemConfig.WARN16:  //khong hoan thanh nhiem vu
                            try
                            {
                                // lay ra loai quan ly danh gia xep loai
                                var IdType = (from p in _dbContext.SysOtherLists.Where(p => p.CODE == "1000")
                                              from l in _dbContext.SysOtherListTypes.Where(x => x.ID == p.TYPE_ID)
                                              where l.CODE == "LDG"
                                              select p.ID).FirstOrDefault();
                                //lay ra nhan vien khong hoan thanh nhiem vu nam truoc va nam nay
                                var groupEvaluates = from p in _dbContext.HuEvaluates.Where(c => ((c.YEAR == DateTime.Now.Year
                                                    && ((DateTime)c.CREATED_DATE).AddMonths(item.Day.Value) >= _dayNow) || c.YEAR == DateTime.Now.Year - 1)
                                                    && c.EMPLOYEE_ID.HasValue
                                                    && c.EVALUATE_TYPE == IdType)
                                                     from o in _dbContext.HuClassifications.Where(c => c.ID == p.CLASSIFICATION_ID && c.IS_ACTIVE == true && c.CODE == "XL014")
                                                     group p by new { EmpId = p.EMPLOYEE_ID } into g
                                                     select new HuEvaluateCountDTO
                                                     {
                                                         EmpId = g.Key.EmpId,
                                                         EvaluateCount = g.Count()
                                                     };
                                var _l = await (from p in groupEvaluates
                                                join e in _dbContext.HuEmployees on p.EmpId equals e.ID into emp
                                                from empResult in emp.Where(x => x.WORK_STATUS_ID != OtherConfig.EMP_STATUS_TERMINATE)
                                                where p.EvaluateCount >= 2 && item.Day > 0
                                                select new ReminderParam
                                                {
                                                    Id = (long)p.EmpId,
                                                    Name = empResult.Profile!.FULL_NAME,
                                                    Code = empResult.CODE,
                                                    OrgId = empResult.ORG_ID
                                                }).ToArrayAsync();

                                item.Value = new List<ReminderParam>();
                                item.Value.AddRange(_l);
                                item.Count = _l.Count();
                                break;
                            }
                            catch { break; }
                        case SystemConfig.WARN17: //Thay đổi sơ yếu lí lịch trên con Portal
                            var q = await (from p in _dbContext.HuEmployeeCvEdits.Where(x => x.IS_SEND_PORTAL == true && x.IS_APPROVED_PORTAL == false)
                                           join e in _dbContext.HuEmployees on p.EMPLOYEE_ID equals e.ID
                                           select new ReminderParam
                                           {
                                               Id = (int)e.ID,
                                               Name = e.Profile.FULL_NAME,
                                               Code = e.CODE
                                           }).ToArrayAsync();
                            item.Value = new List<ReminderParam>();
                            item.Value.AddRange(q);
                            item.Count = q.Count();
                            break;
                        case SystemConfig.WARN18: //Thay đổi sơ yếu lí lịch trên con Portal
                            var a = await (from p in _dbContext.HuFamilyEdits.Where(x => x.IS_SEND_PORTAL == true && x.IS_APPROVE_PORTAL == false)
                                           join e in _dbContext.HuEmployees on p.EMPLOYEE_ID equals e.ID
                                           select new ReminderParam
                                           {
                                               Id = (int)e.ID,
                                               Name = e.Profile.FULL_NAME,
                                               Code = e.CODE
                                           }).ToArrayAsync();
                            item.Value = new List<ReminderParam>();
                            item.Value.AddRange(a);
                            item.Count = a.Count();
                            break;
                        case SystemConfig.WARN19: //Thay đổi sơ yếu lí lịch trên con Portal
                            // warn19 là thông báo trình độ học vấn
                            var b = await (from p in _dbContext.HuEmployeeCvEdits
                                           .Where(x => x.IS_SEND_PORTAL == true
                                                    && x.IS_APPROVED_PORTAL == false
                                                    && x.IS_SEND_PORTAL_EDUCATION == true)
                                           join e in _dbContext.HuEmployees on p.EMPLOYEE_ID equals e.ID
                                           select new ReminderParam
                                           {
                                               Id = (int)e.ID,
                                               Name = e.Profile.FULL_NAME,
                                               Code = e.CODE
                                           }).ToArrayAsync();
                            item.Value = new List<ReminderParam>();
                            item.Value.AddRange(b);
                            item.Count = b.Count();
                            break;
                        case SystemConfig.WARN20:
                            // Thay đổi bằng cấp - chứng chỉ trên con Portal
                            // warn20 là thông báo bằng cấp - chứng chỉ
                            var queryHuCertificateEdits =
                                            await (from p in _dbContext.HuCertificateEdits
                                            .Where(x => x.IS_SEND_PORTAL == true && x.IS_APPROVE_PORTAL == null)
                                                   join e in _dbContext.HuEmployees on p.EMPLOYEE_ID equals e.ID
                                                   select new ReminderParam
                                                   {
                                                       Id = (int)e.ID,
                                                       Name = e.Profile.FULL_NAME,
                                                       Code = e.CODE
                                                   }).ToArrayAsync();
                            item.Value = new List<ReminderParam>();
                            item.Value.AddRange(queryHuCertificateEdits);
                            item.Count = queryHuCertificateEdits.Count();
                            break;
                        case SystemConfig.WARN21: //NV sắp hết hạn Quyết định Điều động/biệt phái
                            try
                            {
                                var _n = await (from p in _dbContext.HuWorkings.Where(c => c.EFFECT_DATE != null && c.STATUS_ID == OtherConfig.STATUS_APPROVE)
                                                join e in _dbContext.HuEmployees on p.EMPLOYEE_ID equals e.ID
                                                join t in _dbContext.SysOtherLists on p.TYPE_ID equals t.ID
                                                where ((DateTime)p.EFFECT_DATE).DayOfYear - _dayofyear <= item.Day && ((DateTime)p.EFFECT_DATE).DayOfYear - _dayofyear >= 0
                                                   && (t.CODE == "BP" || t.CODE == "DDCV" || t.CODE == "DDBN" || t.CODE == "DC")
                                                select new ReminderParam
                                                {
                                                    Id = (long)e.ID,
                                                    Name = e.Profile!.FULL_NAME,
                                                    Code = e.CODE,
                                                    Avatar = e.Profile!.AVATAR
                                                }).ToArrayAsync();
                                item.Value = new List<ReminderParam>();
                                item.Value.AddRange(_n);
                                item.Count = _n.Count();
                                break;
                            }
                            catch { break; }
                            
                        case SystemConfig.WARN22:// Điều chỉnh Hồ sơ nhân viên
                            try
                            {
                                var _a = await (from p in _dbContext.HuEmployeeCvEdits.Where(c => c.CREATED_DATE != null && (c.STATUS_APPROVED_CV_ID == OtherConfig.STATUS_WAITING
                                                || c.STATUS_APPOVED_INSUARENCE_INFO_ID == OtherConfig.STATUS_WAITING || c.STATUS_ADDINATIONAL_INFO_ID == OtherConfig.STATUS_WAITING
                                                || c.STATUS_APPROVED_CONTACT_ID == OtherConfig.STATUS_WAITING || c.STATUS_APPROVED_BANK_ID == OtherConfig.STATUS_WAITING
                                                || c.STATUS_APPROVED_EDUCATION_ID == OtherConfig.STATUS_WAITING))
                                                join e in _dbContext.HuEmployees on p.EMPLOYEE_ID equals e.ID
                                                where (((DateTime)p.CREATED_DATE!).Month + ((DateTime)p.CREATED_DATE!).Year * 12) - (_dayNow.Month + _dayNow.Year * 12) <= item.Day
                                                && (((DateTime)p.CREATED_DATE!).Month + ((DateTime)p.CREATED_DATE!).Year * 12) - (_dayNow.Month + _dayNow.Year * 12) >= 0
                                                && ids.Contains(e.ORG_ID)
                                                select new ReminderParam
                                                {
                                                    Id = (long)p.ID,
                                                    Name = e.Profile!.FULL_NAME,
                                                    Code = e.CODE
                                                }).ToArrayAsync();
                                item.Value = new List<ReminderParam>();
                                item.Value.AddRange(_a);
                                item.Count = _a.Count();
                                break;
                            }
                            catch { break; }

                        case SystemConfig.WARN23:// Điều chỉnh Thông tin người thân
                            try
                            {
                                var _fe = await (from p in _dbContext.HuFamilyEdits.Where(c => c.CREATED_DATE != null && c.STATUS_ID == OtherConfig.STATUS_WAITING)
                                                join e in _dbContext.HuEmployees on p.EMPLOYEE_ID equals e.ID
                                                where (((DateTime)p.CREATED_DATE!).Month + ((DateTime)p.CREATED_DATE!).Year * 12) - (_dayNow.Month + _dayNow.Year * 12) <= item.Day
                                                && (((DateTime)p.CREATED_DATE!).Month + ((DateTime)p.CREATED_DATE!).Year * 12) - (_dayNow.Month + _dayNow.Year * 12) >= 0
                                                && ids.Contains(e.ORG_ID)
                                                select new ReminderParam
                                                {
                                                    Id = (long)p.ID,
                                                    Name = e.Profile!.FULL_NAME,
                                                    Code = e.CODE
                                                }).ToArrayAsync();
                                item.Value = new List<ReminderParam>();
                                item.Value.AddRange(_fe);
                                item.Count = _fe.Count();
                                break;
                            }
                            catch { break; }

                        case SystemConfig.WARN24:// Điều chỉnh Bằng cấp - Chứng chỉ
                            try
                            {
                                var _ce = await (from p in _dbContext.HuCertificateEdits.Where(c => c.CREATED_DATE != null && c.STATUS_ID == OtherConfig.STATUS_WAITING)
                                                 join e in _dbContext.HuEmployees on p.EMPLOYEE_ID equals e.ID
                                                 where (((DateTime)p.CREATED_DATE!).Month + ((DateTime)p.CREATED_DATE!).Year * 12) - (_dayNow.Month + _dayNow.Year * 12) <= item.Day
                                                 && (((DateTime)p.CREATED_DATE!).Month + ((DateTime)p.CREATED_DATE!).Year * 12) - (_dayNow.Month + _dayNow.Year * 12) >= 0
                                                 && ids.Contains(e.ORG_ID)
                                                 select new ReminderParam
                                                 {
                                                     Id = (long)p.ID,
                                                     Name = e.Profile!.FULL_NAME,
                                                     Code = e.CODE
                                                 }).ToArrayAsync();
                                item.Value = new List<ReminderParam>();
                                item.Value.AddRange(_ce);
                                item.Count = _ce.Count();
                                break;
                            }
                            catch { break; }

                        case SystemConfig.WARN25:// Yêu cầu điều chỉnh quá trình công tác 
                            try
                            {
                                var _w = await (from p in _dbContext.PortalRequestChanges.Where(c => c.CREATED_DATE != null && c.IS_APPROVE == OtherConfig.STATUS_WAITING && c.SYS_OTHER_CODE == OtherListConst.WORKING)
                                                 join e in _dbContext.HuEmployees on p.EMPLOYEE_ID equals e.ID
                                                 where (((DateTime)p.CREATED_DATE!).Month + ((DateTime)p.CREATED_DATE!).Year * 12) - (_dayNow.Month + _dayNow.Year * 12) <= item.Day
                                                 && (((DateTime)p.CREATED_DATE!).Month + ((DateTime)p.CREATED_DATE!).Year * 12) - (_dayNow.Month + _dayNow.Year * 12) >= 0
                                                 && ids.Contains(e.ORG_ID)
                                                 select new ReminderParam
                                                 {
                                                     Id = (long)p.ID,
                                                     Name = e.Profile!.FULL_NAME,
                                                     Code = e.CODE
                                                 }).ToArrayAsync();
                                item.Value = new List<ReminderParam>();
                                item.Value.AddRange(_w);
                                item.Count = _w.Count();
                                break;
                            }
                            catch { break; }

                        case SystemConfig.WARN26:// Yêu cầu điều chỉnh quá trình công tác trước đây
                            try
                            {
                                var _wb = await (from p in _dbContext.PortalRequestChanges.Where(c => c.CREATED_DATE != null && c.IS_APPROVE == OtherConfig.STATUS_WAITING && c.SYS_OTHER_CODE == OtherListConst.WORKING_BEFORE)
                                                 join e in _dbContext.HuEmployees on p.EMPLOYEE_ID equals e.ID
                                                 where (((DateTime)p.CREATED_DATE!).Month + ((DateTime)p.CREATED_DATE!).Year * 12) - (_dayNow.Month + _dayNow.Year * 12) <= item.Day
                                                 && (((DateTime)p.CREATED_DATE!).Month + ((DateTime)p.CREATED_DATE!).Year * 12) - (_dayNow.Month + _dayNow.Year * 12) >= 0
                                                 && ids.Contains(e.ORG_ID)
                                                 select new ReminderParam
                                                 {
                                                     Id = (long)p.ID,
                                                     Name = e.Profile!.FULL_NAME,
                                                     Code = e.CODE
                                                 }).ToArrayAsync();
                                item.Value = new List<ReminderParam>();
                                item.Value.AddRange(_wb);
                                item.Count = _wb.Count();
                                break;
                            }
                            catch { break; }
                        case SystemConfig.WARN27:// Nhân viên hết hạn phụ cấp 
                            try
                            {
                                var _al = await (from p in _dbContext.HuAllowanceEmps.Where(c => c.DATE_START != null && c.DATE_END != null)
                                                join e in _dbContext.HuEmployees on p.EMPLOYEE_ID equals e.ID
                                                where ((DateTime)p.DATE_END!).DayOfYear - _dayNow.DayOfYear <= item.Day && ((DateTime)p.DATE_END).DayOfYear - _dayNow.DayOfYear >= 0
                                                   && e.WORK_STATUS_ID != OtherConfig.EMP_STATUS_TERMINATE
                                                  && ids.Contains(e.ORG_ID)
                                                select new ReminderParam
                                                {
                                                    Id = (long)p.ID,
                                                    Name = e.Profile!.FULL_NAME,
                                                    Code = e.CODE,
                                                    Avatar = e.Profile!.AVATAR
                                                }).ToArrayAsync();
                                item.Value = new List<ReminderParam>();
                                item.Value.AddRange(_al);
                                item.Count = _al.Count();
                                break;
                            }
                            catch { break; }
                        default:

                            break;
                    }
                }
                return new() { InnerBody = listRemind, StatusCode = EnumStatusCode.StatusCode200, MessageCode = CommonMessageCode.QUERY_LIST_SUCCESS };
            }
            catch (Exception ex)
            {
                return new FormatedResponse
                {
                    ErrorType = EnumErrorType.UNCATCHABLE,
                    MessageCode = ex.Message,
                    InnerBody = null,
                    StatusCode = EnumStatusCode.StatusCode500
                };
            }
        }

        public async Task<FormatedResponse> GetHistoryOrgId(long EmployeeId)
        {
            var n = await (from p in _dbContext.HuWorkings
                           where p.EMPLOYEE_ID == EmployeeId && p.STATUS_ID == OtherConfig.STATUS_APPROVE && p.ORG_ID.HasValue && (p.IS_WAGE == 0 || p.IS_WAGE == null)
                           select new
                           {
                               Id = p.ID,
                               OrgId = p.ORG_ID
                           }).Distinct().ToListAsync(); ;
            return new() { InnerBody = n, StatusCode = EnumStatusCode.StatusCode200 };
        }
        public virtual async Task<FormatedResponse> ToggleActiveIds(GenericUnitOfWork _uow, List<long> ids, bool valueToBind, string sid)
        {
            await Task.Run(() => null);
            throw new NotImplementedException();
        }

    }
}

