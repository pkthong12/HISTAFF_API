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

namespace API.All.HRM.ProfessionalEmployee.ProfessionalEmployeeAPI.PeEmployeeAssessment
{
    public class PeEmployeeAssessmentRepository : IPeEmployeeAssessmentRepository
    {
        private readonly GenericUnitOfWork _uow;
        private readonly FullDbContext _dbContext;
        private IGenericRepository<PE_EMPLOYEE_ASSESSMENT, PeEmployeeAssessmentDTO> _genericRepository;
        private readonly GenericReducer<PE_EMPLOYEE_ASSESSMENT, PeEmployeeAssessmentDTO> _genericReducer;

        public PeEmployeeAssessmentRepository(FullDbContext context, GenericUnitOfWork uow)
        {
            _dbContext = context;
            _uow = uow;
            _genericRepository = _uow.GenericRepository<PE_EMPLOYEE_ASSESSMENT, PeEmployeeAssessmentDTO>();
            _genericReducer = new();
        }

        public async Task<GenericPhaseTwoListResponse<PeEmployeeAssessmentDTO>> SinglePhaseQueryList(GenericQueryListDTO<PeEmployeeAssessmentDTO> request)
        {
            var joined = (from p in _dbContext.PeEmployeeAssessments
                          from he in _dbContext.HuEmployees.Where(x => x.ID == p.EMPLOYEE_ID).DefaultIfEmpty()
                          from hec in _dbContext.HuEmployeeCvs.Where(x => x.ID == he.PROFILE_ID).DefaultIfEmpty()
                          from ho in _dbContext.HuOrganizations.Where(x => x.ID == he.ORG_ID).DefaultIfEmpty()
                          from hp in _dbContext.HuPositions.Where(x => x.ID == he.POSITION_ID).DefaultIfEmpty()
                          select new PeEmployeeAssessmentDTO
                          {
                              Id = p.ID,
                              Code = he.CODE,
                              FullName = hec.FULL_NAME,
                              OrgName = ho.NAME,
                              PositionName = hp.NAME,
                              HuCompetencyPeriodId = p.HU_COMPETENCY_PERIOD_ID
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
            try
            {
                var res = await _genericRepository.GetById(id);
                if (res.InnerBody != null)
                {
                    var response = res.InnerBody;
                    var list = new List<PE_EMPLOYEE_ASSESSMENT>
                    {
                        (PE_EMPLOYEE_ASSESSMENT)response
                    };
                    var joined = (from p in list
                                  select new PeEmployeeAssessmentDTO
                                  {
                                      Id = p.ID,
                                  }).FirstOrDefault();

                    if (joined != null)
                    {
                        if (res.StatusCode == EnumStatusCode.StatusCode200)
                        {
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

        public async Task<FormatedResponse> Create(GenericUnitOfWork _uow, PeEmployeeAssessmentDTO dto, string sid)
        {
            // dto.IsActive = true;
            var response = await _genericRepository.Create(_uow, dto, sid);
            return response;
        }

        public async Task<FormatedResponse> CreateRange(GenericUnitOfWork _uow, List<PeEmployeeAssessmentDTO> dtos, string sid)
        {
            var add = new List<PeEmployeeAssessmentDTO>();
            add.AddRange(dtos);
            var response = await _genericRepository.CreateRange(_uow, add, sid);
            return response;
        }

        public async Task<FormatedResponse> Update(GenericUnitOfWork _uow, PeEmployeeAssessmentDTO dto, string sid, bool patchMode = true)
        {
            var response = await _genericRepository.Update(_uow, dto, sid, patchMode);
            return response;
        }

        public async Task<FormatedResponse> UpdateRange(GenericUnitOfWork _uow, List<PeEmployeeAssessmentDTO> dtos, string sid, bool patchMode = true)
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

        public async Task<FormatedResponse> AddEmployee(RequestModel requestModel, GenericUnitOfWork _uow, string sid)
        {
            try
            {
                FormatedResponse formatedResponse = new FormatedResponse();

                if (requestModel.ids != null)
                {
                    foreach (var item in requestModel.ids)
                    {
                        PeEmployeeAssessmentDTO dto = new PeEmployeeAssessmentDTO();

                        dto.EmployeeId = item;
                        dto.HuCompetencyPeriodId = requestModel.huCompetencyPeriodId;

                        var response = await _genericRepository.Create(_uow, dto, sid);

                        formatedResponse = response;
                    }
                }

                return formatedResponse;
            }
            catch (Exception ex)
            {
                return new() {
                    MessageCode = ex.Message,
                    ErrorType = EnumErrorType.UNCATCHABLE,
                    StatusCode = EnumStatusCode.StatusCode500
                };
            }
        }

        public async Task<FormatedResponse> DeleteEmployee(List<long> ids, GenericUnitOfWork _uow)
        {
            try
            {
                var response = await _genericRepository.DeleteIds(_uow, ids);

                return response;
            }
            catch (Exception ex)
            {
                return new()
                {
                    MessageCode = ex.Message,
                    ErrorType = EnumErrorType.UNCATCHABLE,
                    StatusCode = EnumStatusCode.StatusCode500
                };
            }
        }

        public async Task<FormatedResponse> GetDropDownListEvaluationPeriod()
        {
            try
            {
                var list = await _dbContext.HuCompetencyPeriods
                                .Where(x => x.IS_ACTIVE == true)
                                .Select(x => new
                                {
                                    Id = x.ID,
                                    Name = x.NAME
                                })
                                .ToListAsync();

                return new FormatedResponse()
                {
                    InnerBody = list
                };
            }
            catch (Exception ex)
            {
                return new()
                {
                    MessageCode = ex.Message,
                    ErrorType = EnumErrorType.UNCATCHABLE,
                    StatusCode = EnumStatusCode.StatusCode500
                };
            }
        }
    }
}