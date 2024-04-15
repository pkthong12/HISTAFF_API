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
using System.Dynamic;
using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;
using Hangfire.Storage;
using System;
using System.Data;
using ProfileDAL.ViewModels;
using System.Collections.Generic;
using System.Drawing.Printing;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;

namespace API.All.HRM.Payroll.PayrollAPI.PaSalImportBackdate
{
    public class PaSalImportBackdateRepository : IPaSalImportBackdateRepository
    {
        private readonly GenericUnitOfWork _uow;
        private readonly FullDbContext _dbContext;
        private IGenericRepository<PA_SAL_IMPORT_BACKDATE, PaSalImportBackdateDTO> _genericRepository;
        private readonly GenericReducer<PA_SAL_IMPORT_BACKDATE, PaSalImportBackdateDTO> _genericReducer;
        protected AbsQueryDataTemplate QueryData;

        public PaSalImportBackdateRepository(FullDbContext context, GenericUnitOfWork uow)
        {
            _dbContext = context;
            _uow = uow;
            _genericRepository = _uow.GenericRepository<PA_SAL_IMPORT_BACKDATE, PaSalImportBackdateDTO>();
            _genericReducer = new();
            QueryData = new SqlQueryDataTemplate(_dbContext);
        }

        public Task<FormatedResponse> ReadAll()
        {
            throw new NotImplementedException();
        }

        public Task<FormatedResponse> ReadAllByKey(string key, long value)
        {
            throw new NotImplementedException();
        }

        public Task<FormatedResponse> ReadAllByKey(string key, string value)
        {
            throw new NotImplementedException();
        }

        public Task<FormatedResponse> GetById(long id)
        {
            throw new NotImplementedException();
        }

        public Task<FormatedResponse> GetById(string id)
        {
            throw new NotImplementedException();
        }

        public Task<FormatedResponse> Create(GenericUnitOfWork _uow, PaSalImportBackdateDTO dto, string sid)
        {
            throw new NotImplementedException();
        }

        public Task<FormatedResponse> CreateRange(GenericUnitOfWork _uow, List<PaSalImportBackdateDTO> dtos, string sid)
        {
            throw new NotImplementedException();
        }

        public Task<FormatedResponse> Update(GenericUnitOfWork _uow, PaSalImportBackdateDTO dto, string sid, bool patchMode = true)
        {
            throw new NotImplementedException();
        }

        public Task<FormatedResponse> UpdateRange(GenericUnitOfWork _uow, List<PaSalImportBackdateDTO> dtos, string sid, bool patchMode = true)
        {
            throw new NotImplementedException();
        }

        public Task<FormatedResponse> Delete(GenericUnitOfWork _uow, long id)
        {
            throw new NotImplementedException();
        }

        public Task<FormatedResponse> Delete(GenericUnitOfWork _uow, string id)
        {
            throw new NotImplementedException();
        }

        public Task<FormatedResponse> DeleteIds(GenericUnitOfWork _uow, List<long> ids)
        {
            throw new NotImplementedException();
        }

        public Task<FormatedResponse> DeleteIds(GenericUnitOfWork _uow, List<string> ids)
        {
            throw new NotImplementedException();
        }

        public async Task<FormatedResponse> GetShiftDefault(PaSalImportBackdateDTO param)
        {
            try
            {
                var r = await QueryData.ExecuteNonQuery("PKG_TIMESHEET_GET_SIGN_DEFAULT",
                    new
                    {
                        P_USERNAME = _dbContext.UserName,
                        P_CREATEDBY = _dbContext.CurrentUserId,
                        // P_ORG_ID = param.ListOrgIds[0],
                        P_PERIOD_ID = param.PeriodId,
                        P_ISDISSOLVE = 0,
                    }, false);
                return new() { InnerBody = { }, MessageCode = CommonMessageCode.GET_DATA_SUCCESS };
            }
            catch (Exception ex)
            {
                return new() { ErrorType = EnumErrorType.UNCATCHABLE, StatusCode = EnumStatusCode.StatusCode500, MessageCode = ex.Message };
            }
        }

        public async Task<GenericPhaseTwoListResponse<PaSalImportBackdateDTO>> SinglePhaseQueryList(GenericQueryListDTO<PaSalImportBackdateDTO> request)
        {

            DateTime endDate = DateTime.Now;
            DateTime startDate = DateTime.Now;
            long? periodId = -1;
            long? objSalId = -1;
            long? periodAddId = -1;
            if (request.Filter != null)
            {
                if (request.Filter.PeriodId != null)
                {
                    var objPerriod = _dbContext.AtSalaryPeriods.Where(x => x.ID == request.Filter.PeriodId).FirstOrDefault();
                    endDate = objPerriod.END_DATE;
                    startDate = objPerriod.START_DATE;
                    periodId = request.Filter.PeriodId;
                    request.Filter.PeriodId = null;
                }
                if (request.Filter.ObjSalaryId != null)
                {
                    objSalId = request.Filter.ObjSalaryId;
                    request.Filter.ObjSalaryId = null;
                }
                if (request.Filter.PeriodAddId != null)
                {
                    periodAddId = request.Filter.PeriodAddId;
                    request.Filter.PeriodAddId = null;

                }
            }



            var groupedData = _dbContext.HuWorkings
                .Where(e => e.EFFECT_DATE < endDate && e.STATUS_ID == 994 && (e.IS_WAGE ?? 0) == -1)
                .GroupBy(e => e.EMPLOYEE_ID)
                .ToList();

            var result = groupedData
                .SelectMany(g => g.OrderByDescending(e => e.EFFECT_DATE).ThenByDescending(e => e.ID).Take(1))
                .Select(ab => ab.ID)
                .ToList();

            var reJoined = from e in _dbContext.HuEmployees.Where(x => x.JOIN_DATE <= endDate && ((x.WORK_STATUS_ID ?? 0) != 1028 || ((x.WORK_STATUS_ID ?? 0) == 1028 && (x.TER_EFFECT_DATE ?? DateTime.Now) >= startDate)))
                           from cv in _dbContext.HuEmployeeCvs.Where(x => x.ID == e.PROFILE_ID)
                           from t in _dbContext.HuPositions.Where(x => x.ID == e.POSITION_ID)
                           from j in _dbContext.HuJobs.AsNoTracking().Where(j => j.ID == t.JOB_ID).DefaultIfEmpty()
                           from o in _dbContext.HuOrganizations.Where(x => x.ID == e.ORG_ID)
                           from wo in _dbContext.HuWorkings.Where(x => x.EMPLOYEE_ID == e.ID && result.Contains(x.ID))
                           from ob in _dbContext.HuSalaryTypes.Where(x => x.ID == wo.SALARY_TYPE_ID).DefaultIfEmpty()
                           from p in _dbContext.PaSalImportBackdates.Where(x => x.EMPLOYEE_ID == e.ID && x.PERIOD_ID == periodId && x.OBJ_SALARY_ID == objSalId  && x.PERIOD_ADD_ID == periodAddId).DefaultIfEmpty()
                           select new PaSalImportBackdateDTO
                           {
                               Id = e.ID,
                               EmployeeCode = e.CODE,
                               EmployeeId = e.ID,
                               EmployeeName = cv.FULL_NAME,
                               PositionName = t.NAME,
                               OrgName = o.NAME,
                               OrgId = o.ID,
                               ObjSalaryId = wo.SALARY_TYPE_ID,
                               ObjSalaryName = ob.NAME,
                               CreatedBy = p.CREATED_BY,
                               CreatedDate = p.CREATED_DATE,
                               UpdatedBy = p.UPDATED_BY,
                               UpdatedDate = p.UPDATED_DATE,
                               Deduct5 = p.DEDUCT5,
                               Clchinh8 = p.CLCHINH8,
                               Cl1 = p.CL1,
                               Cl27 = p.CL27,
                               PeriodId = p.PERIOD_ID,
                               Note = p.NOTE,
                               PeriodAddId = p.PERIOD_ADD_ID,
                               JobOrderNum = (int)(j.ORDERNUM ?? 999),
                           };



            var singlePhaseResult = await _genericReducer.SinglePhaseReduce(reJoined, request);
            return singlePhaseResult;
        }

        public async Task<FormatedResponse> GetCurrentPeriodSalary()
        {
            var datetimeNow = DateTime.Now;
            var res = await (from e in _dbContext.AtSalaryPeriods
                             where e.START_DATE.Date <= datetimeNow.Date && e.END_DATE.Date >= datetimeNow.Date
                             select new
                             {
                                 Id = e.ID,
                                 StartDate = e.START_DATE,
                                 EndDate = e.END_DATE,
                             }).FirstOrDefaultAsync();
            return new() { InnerBody = res };
        }


        public async Task<FormatedResponse> GetEmployeeInfo(PaSalImportBackdateDTO param)
        {
            try
            {
                var result = await (from e in _dbContext.HuEmployees.AsNoTracking().Where(e => e.ID == param.EmployeeId).AsQueryable()
                                    from cv in _dbContext.HuEmployeeCvs.AsNoTracking().Where(cv => cv.ID == e.PROFILE_ID).DefaultIfEmpty()
                                    from p in _dbContext.HuPositions.AsNoTracking().Where(pos => pos.ID == e.POSITION_ID).DefaultIfEmpty()
                                    from d in _dbContext.HuOrganizations.AsNoTracking().Where(o => o.ID == e.ORG_ID).DefaultIfEmpty()
                                    select new
                                    {
                                        Id = e.ID,
                                        Code = e.CODE,
                                        Name = cv.FULL_NAME,
                                        DepartmentName = d.NAME,
                                        PositionName = p.NAME,
                                    }).FirstOrDefaultAsync();
                return new() { InnerBody = result };
            }
            catch (Exception ex)
            {
                return new() { ErrorType = EnumErrorType.UNCATCHABLE, StatusCode = EnumStatusCode.StatusCode500, MessageCode = ex.Message };
            }
        }

        public virtual async Task<FormatedResponse> ToggleActiveIds(GenericUnitOfWork _uow, List<long> ids, bool valueToBind, string sid)
        {
            await Task.Run(() => null);
            throw new NotImplementedException();
        }

        public async Task<FormatedResponse> GetListSalaryInYear(AtSalaryPeriodDTO param)
        {
            var queryable = await (from p in _dbContext.AtSalaryPeriods
                                   where p.IS_ACTIVE == true && p.YEAR == param.Year || (p.YEAR == param.Year + 1 && p.MONTH == 1)
                                   orderby p.START_DATE ascending
                                   select new
                                   {
                                       Id = p.ID,
                                       Name = p.NAME,
                                       Month = p.MONTH,
                                   }).ToListAsync();
            return new() { InnerBody = queryable };
        }
    }
}
