using API.All.DbContexts;
using API.DTO;
using CORE.DTO;
using CORE.Enum;
using CORE.GenericUOW;
using CORE.StaticConstant;
using CORE.AutoMapper;
using System;
using API.Entities;
using Microsoft.AspNetCore.Http.HttpResults;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Logical;
using API.All.HRM.Payroll.PayrollAPI.PaListSal;
using System.Linq;
using Common.Extensions;
using API.All.SYSTEM.Common;
using DocumentFormat.OpenXml.Presentation;

namespace API.All.HRM.ProfessionalEmployee.ProfessionalEmployeeAPI.PeEmployeeAssessment
{
    public class UnassignedEmployeeRepository : IUnassignedEmployeeRepository
    {
        private readonly GenericUnitOfWork _uow;
        private readonly FullDbContext _dbContext;
        private IGenericRepository<HU_EMPLOYEE, EmployeeModel> _genericRepository;
        private readonly GenericReducer<HU_EMPLOYEE, EmployeeModel> _genericReducer;

        public UnassignedEmployeeRepository(FullDbContext context, GenericUnitOfWork uow)
        {
            _dbContext = context;
            _uow = uow;
            _genericRepository = _uow.GenericRepository<HU_EMPLOYEE, EmployeeModel>();
            _genericReducer = new();
        }

        public async Task<GenericPhaseTwoListResponse<EmployeeModel>> SinglePhaseQueryList(GenericQueryListDTO<EmployeeModel> request)
        {
            long? huCompetencyPeriodId = request.InOperators.FirstOrDefault(x => x.Field == "huCompetencyPeriodId").Values[0];

            var listAssignedEmployee = _dbContext.PeEmployeeAssessments
                                       .Where(x => x.HU_COMPETENCY_PERIOD_ID == huCompetencyPeriodId)
                                       .Select(x => x.EMPLOYEE_ID);

            var workStatusId = _dbContext.SysOtherLists.FirstOrDefault(x => x.CODE == "ESW");

            // check workStatusId different null
            if (workStatusId == null)
            {
                return new GenericPhaseTwoListResponse<EmployeeModel>()
                {
                    ErrorType = EnumErrorType.CATCHABLE,
                    MessageCode = CommonMessageCodes.WORK_STATUS_ID_NOT_FOUND
                };
            }

            var joined =  (from p in _dbContext.HuEmployees.Where(x => x.WORK_STATUS_ID == workStatusId.ID && !listAssignedEmployee.Contains(x.ID))
                          from hec in _dbContext.HuEmployeeCvs.Where(x => x.ID == p.PROFILE_ID).DefaultIfEmpty()
                          from ho in _dbContext.HuOrganizations.Where(x => x.ID == p.ORG_ID).DefaultIfEmpty()
                          from hp in _dbContext.HuPositions.Where(x => x.ID == p.POSITION_ID).DefaultIfEmpty()

                          select new EmployeeModel
                          {
                              Id = p.ID,
                              Code = p.CODE,
                              Fullname = hec.FULL_NAME,
                              OrgName = ho.NAME,
                              PositionName = hp.NAME,
                              OrgId = p.ORG_ID,
                              HuCompetencyPeriodId = huCompetencyPeriodId
                          });

            var singlePhaseResult = await _genericReducer.SinglePhaseReduce(joined, request);
            return singlePhaseResult;
        }

        public async Task<FormatedResponse> ReadAll()
        {
            var response = await _genericRepository.ReadAll();
            return response;
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
            await Task.Run(() => null);
            throw new NotImplementedException();
        }

        public async Task<FormatedResponse> GetById(string id)
        {
            await Task.Run(() => null);
            throw new NotImplementedException();
        }

        public async Task<FormatedResponse> Delete(GenericUnitOfWork _uow, long id)
        {
            await Task.Run(() => null);
            throw new NotImplementedException();
        }

        public async Task<FormatedResponse> Delete(GenericUnitOfWork _uow, string id)
        {
            await Task.Run(() => null);
            throw new NotImplementedException();
        }

        public async Task<FormatedResponse> DeleteIds(GenericUnitOfWork _uow, List<long> ids)
        {
            await Task.Run(() => null);
            throw new NotImplementedException();
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

        public Task<FormatedResponse> Create(GenericUnitOfWork _uow, EmployeeModel dto, string sid)
        {
            throw new NotImplementedException();
        }

        public Task<FormatedResponse> CreateRange(GenericUnitOfWork _uow, List<EmployeeModel> dtos, string sid)
        {
            throw new NotImplementedException();
        }

        public Task<FormatedResponse> Update(GenericUnitOfWork _uow, EmployeeModel dto, string sid, bool patchMode = true)
        {
            throw new NotImplementedException();
        }

        public Task<FormatedResponse> UpdateRange(GenericUnitOfWork _uow, List<EmployeeModel> dtos, string sid, bool patchMode = true)
        {
            throw new NotImplementedException();
        }
    }
}