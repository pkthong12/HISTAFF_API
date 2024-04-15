using CORE.GenericUOW;
using CORE.DTO;
using API.DTO;
using CORE.Enum;
using CORE.StaticConstant;
using API.All.DbContexts;
using CORE.AutoMapper;
using Microsoft.EntityFrameworkCore;
using Azure;
using System.Data.SqlClient;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Configuration;
using System.Configuration;
using Common.Extensions;
using Common.Interfaces;
using Common.DataAccess;
using System.Reflection.Emit;
using System.Xml.Linq;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using ProfileDAL.ViewModels;
using System;
using CORE.Services.File;
using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;

namespace API.Controllers.AtDecleareSeniority
{
    public class AtDeclareSeniorityRepository : IAtDeclareSeniorityRepository
    {
        private readonly GenericUnitOfWork _uow;
        private readonly FullDbContext _dbContext;
        private IGenericRepository<AT_DECLARE_SENIORITY, AtDeclareSeniorityDTO> _genericRepository;
        private readonly GenericReducer<AT_DECLARE_SENIORITY, AtDeclareSeniorityDTO> _genericReducer;
        private readonly IWebHostEnvironment _env;
        private readonly AppSettings _appSettings;
        private readonly IFileService _fileService;

        public AtDeclareSeniorityRepository(FullDbContext context, GenericUnitOfWork uow, IWebHostEnvironment env,
            IOptions<AppSettings> options,
            IFileService fileService)
        {
            _dbContext = context;
            _uow = uow;
            _genericRepository = _uow.GenericRepository<AT_DECLARE_SENIORITY, AtDeclareSeniorityDTO>();
            _genericReducer = new();
            _env = env;
            _appSettings = options.Value;
            _fileService = fileService;
        }

        public async Task<GenericPhaseTwoListResponse<AtDeclareSeniorityDTO>> SinglePhaseQueryList(GenericQueryListDTO<AtDeclareSeniorityDTO> request)
        {
            var joined = from p in _dbContext.AtDeclareSenioritys.AsNoTracking()
                         from e in _dbContext.HuEmployees.Where(x => x.ID == p.EMPLOYEE_ID).DefaultIfEmpty()
                         from o in _dbContext.HuOrganizations.Where(x => x.ID == e.ORG_ID).DefaultIfEmpty()
                         from ecv in _dbContext.HuEmployeeCvs.Where(x => x.ID == e.PROFILE_ID).DefaultIfEmpty()
                         from po in _dbContext.HuPositions.Where(x => x.ID == e.POSITION_ID).DefaultIfEmpty()
                         from j in _dbContext.HuJobs.AsNoTracking().Where(x => x.ID == po.JOB_ID).DefaultIfEmpty()
                         from ma in _dbContext.AtSalaryPeriods.Where(x => x.ID == p.MONTH_ADJUST).DefaultIfEmpty()
                         from md in _dbContext.AtSalaryPeriods.Where(x => x.ID == p.MONTH_DAY_OFF).DefaultIfEmpty()
                         select new AtDeclareSeniorityDTO
                         {
                             Id = p.ID,
                             EmployeeId = p.EMPLOYEE_ID,
                             EmployeeCode = e.CODE,
                             EmployeeName = ecv.FULL_NAME,
                             JobOrderNum = (int)(j.ORDERNUM ?? 999),
                             OrgId = e.ORG_ID,
                             OrgName = o.NAME,
                             PositionName = po.NAME,
                             YearDeclare = p.YEAR_DECLARE,
                             MonthAdjust = p.MONTH_ADJUST,
                             MonthAdjustName = ma.NAME,
                             MonthAdjustNumber = p.MONTH_ADJUST_NUMBER,
                             ReasonAdjust = p.REASON_ADJUST,
                             MonthDayOff = p.MONTH_DAY_OFF,
                             MonthDayOffName = md.NAME,
                             NumberDayOff = p.NUMBER_DAY_OFF,
                             ReasonAdjustDayOff = p.REASON_ADJUST_DAY_OFF,
                             Total = p.TOTAL,
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
                var list = new List<AT_DECLARE_SENIORITY>
                    {
                        (AT_DECLARE_SENIORITY)response
                    };
                var joined = (from p in list
                              select new AtDeclareSeniorityDTO
                              {
                                  Id = p.ID,
                                  EmployeeId = p.EMPLOYEE_ID,
                                  YearDeclare = p.YEAR_DECLARE,
                                  MonthAdjust = p.MONTH_ADJUST,
                                  MonthAdjustNumber = p.MONTH_ADJUST_NUMBER,
                                  ReasonAdjust = p.REASON_ADJUST,
                                  MonthDayOff = p.MONTH_DAY_OFF,
                                  NumberDayOff = p.NUMBER_DAY_OFF,
                                  ReasonAdjustDayOff = p.REASON_ADJUST_DAY_OFF,
                                  Total = p.TOTAL,
                              }).FirstOrDefault();


                if (joined != null)
                {
                    if (res.StatusCode == EnumStatusCode.StatusCode200)
                    {
                        var child = (from e in _dbContext.HuEmployees.Where(x => x.ID == joined.EmployeeId).DefaultIfEmpty()
                                     from o in _dbContext.HuOrganizations.Where(x => x.ID == e.ORG_ID).DefaultIfEmpty()
                                     from ecv in _dbContext.HuEmployeeCvs.Where(x => x.ID == e.PROFILE_ID).DefaultIfEmpty()
                                     from po in _dbContext.HuPositions.Where(x => x.ID == e.POSITION_ID).DefaultIfEmpty()
                                     from ma in _dbContext.AtSalaryPeriods.Where(x => x.ID == joined.MonthAdjust).DefaultIfEmpty()
                                     from md in _dbContext.AtSalaryPeriods.Where(x => x.ID == joined.MonthDayOff).DefaultIfEmpty()
                                     select new
                                     {
                                         EmployeeCode = e.CODE,
                                         EmployeeName = ecv.FULL_NAME,
                                         OrgId = e.ORG_ID,
                                         OrgName = o.NAME,
                                         PositionName = po.NAME,
                                         MonthAdjustName = ma.NAME,
                                         MonthDayOffName = md.NAME,
                                     }).FirstOrDefault();
                        joined.EmployeeCode = child?.EmployeeCode;
                        joined.EmployeeName = child?.EmployeeName;
                        joined.OrgId = child?.OrgId;
                        joined.OrgName = child?.OrgName;
                        joined.PositionName = child?.PositionName;
                        joined.MonthAdjustName = child?.MonthAdjustName;
                        joined.MonthDayOffName = child?.MonthDayOffName;
                        return new FormatedResponse() { InnerBody = joined };
                    }
                    else
                    {
                        return new() { ErrorType = EnumErrorType.CATCHABLE, MessageCode = CommonMessageCode.GET_LIST_BY_KEY_RESPOSNE_IS_NOT_OF_STATUS_CODE_200, StatusCode = EnumStatusCode.StatusCode400 };
                    }
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


        public async Task<FormatedResponse> CalculateTotal(long employeeId)
        {
            try
            {
                var joined = await _dbContext.AtEntitlements.AsNoTracking()
                    .Where(x => x.EMPLOYEE_ID == employeeId)
                    .OrderByDescending(x => x.YEAR)
                    .ThenByDescending(x => x.MONTH)
                    .ToListAsync();

                var obj = joined.FirstOrDefault();
                if (obj != null)
                {
                    var dayNum = obj.SENIORITYHAVE + obj.TOTAL_HAVE;
                    return new FormatedResponse() { InnerBody = dayNum };
                }
                else { return new FormatedResponse() { InnerBody = 0 }; }
            }
            catch
            {
                return new FormatedResponse() { MessageCode = CommonMessageCode.ENTITY_NOT_FOUND, ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400 };
            }
        }


        public async Task<FormatedResponse> GetById(string id)
        {
            await Task.Run(() => null);
            throw new NotImplementedException();
        }

        public async Task<FormatedResponse> Create(GenericUnitOfWork _uow, AtDeclareSeniorityDTO dto, string sid)
        {
            var response = await _genericRepository.Create(_uow, dto, sid);
            return new FormatedResponse()
            {
                MessageCode = response.MessageCode,
                InnerBody = response,
                StatusCode = response.StatusCode
            };
        }

        public async Task<FormatedResponse> CreateRange(GenericUnitOfWork _uow, List<AtDeclareSeniorityDTO> dtos, string sid)
        {
            var add = new List<AtDeclareSeniorityDTO>();
            add.AddRange(dtos);
            var response = await _genericRepository.CreateRange(_uow, add, sid);
            return response;
        }

        public async Task<FormatedResponse> Update(GenericUnitOfWork _uow, AtDeclareSeniorityDTO dto, string sid, bool patchMode = true)
        {
            var response = await _genericRepository.Update(_uow, dto, sid, patchMode);
            return new FormatedResponse()
            {
                MessageCode = response.MessageCode,
                InnerBody = response,
                StatusCode = response.StatusCode
            };
        }


        public async Task<FormatedResponse> UpdateRange(GenericUnitOfWork _uow, List<AtDeclareSeniorityDTO> dtos, string sid, bool patchMode = true)
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

    }
}

