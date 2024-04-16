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

namespace API.All.HRM.Attendance.AttendanceAPI.Business.AtTimeWorkStandard
{
    public class AtTimeWorkStandardRepository : IAtTimeWorkStandardRepository
    {
        private readonly GenericUnitOfWork _uow;
        private readonly FullDbContext _dbContext;
        private IGenericRepository<AT_TIME_WORK_STANDARD, AtTimeWorkStandardDTO> _genericRepository;
        private readonly GenericReducer<AT_TIME_WORK_STANDARD, AtTimeWorkStandardDTO> _genericReducer;

        public AtTimeWorkStandardRepository(FullDbContext context, GenericUnitOfWork uow)
        {
            _dbContext = context;
            _uow = uow;
            _genericRepository = _uow.GenericRepository<AT_TIME_WORK_STANDARD, AtTimeWorkStandardDTO>();
            _genericReducer = new();
        }

        public async Task<GenericPhaseTwoListResponse<AtTimeWorkStandardDTO>> SinglePhaseQueryList(GenericQueryListDTO<AtTimeWorkStandardDTO> request)
        {
            var joined = (from p in _dbContext.AtTimeWorkStandards
                          from reference_1 in _dbContext.HuOrganizations.Where(x => x.ID == p.ORG_ID)
                          from reference_2 in _dbContext.SysOtherLists.Where(x => x.ID == p.OBJ_EMPLOYEE_ID)
                          from reference_3 in _dbContext.SysOtherLists.Where(x => x.ID == p.WORK_ENVIRONMENT_ID)
                          select new AtTimeWorkStandardDTO
                          {
                              Id = p.ID,
                              EffectiveYear = p.EFFECTIVE_YEAR,
                              OrgName = reference_1.NAME,
                              ObjEmployeeName = reference_2.NAME,
                              WorkEnvironmentName = reference_3.NAME,
                              Coefficient = p.COEFFICIENT,
                              T1 = p.T1,
                              T2 = p.T2,
                              T3 = p.T3,
                              T4 = p.T4,
                              T5 = p.T5,
                              T6 = p.T6,
                              T7 = p.T7,
                              T8 = p.T8,
                              T9 = p.T9,
                              T10 = p.T10,
                              T11 = p.T11,
                              T12 = p.T12,
                              OrgId = p.ORG_ID,
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
                    var list = new List<AT_TIME_WORK_STANDARD>
                    {
                        (AT_TIME_WORK_STANDARD)response
                    };
                    var joined = (from p in list
                                  select new AtTimeWorkStandardDTO
                                  {
                                      Id = p.ID,
                                      EffectiveYear = p.EFFECTIVE_YEAR,
                                      OrgId = p.ORG_ID,
                                      ObjEmployeeId = p.OBJ_EMPLOYEE_ID,
                                      WorkEnvironmentId = p.WORK_ENVIRONMENT_ID,
                                      IsNotSaturdayIncluded = p.IS_NOT_SATURDAY_INCLUDED,
                                      IsNotSundayIncluded = p.IS_NOT_SUNDAY_INCLUDED,
                                      IsNotHalfSaturdayIncluded = p.IS_NOT_HALF_SATURDAY_INCLUDED,
                                      IsNotTwoSaturdays = p.IS_NOT_TWO_SATURDAYS,
                                      DeductWorkDuringMonth = p.DEDUCT_WORK_DURING_MONTH,
                                      DefaultWork = p.DEFAULT_WORK,
                                      Coefficient = p.COEFFICIENT,
                                      T1 = p.T1,
                                      T2 = p.T2,
                                      T3 = p.T3,
                                      T4 = p.T4,
                                      T5 = p.T5,
                                      T6 = p.T6,
                                      T7 = p.T7,
                                      T8 = p.T8,
                                      T9 = p.T9,
                                      T10 = p.T10,
                                      T11 = p.T11,
                                      T12 = p.T12,
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

        public async Task<FormatedResponse> Create(GenericUnitOfWork _uow, AtTimeWorkStandardDTO dto, string sid)
        {
            // dto.IsActive = true;
            var response = await _genericRepository.Create(_uow, dto, sid);
            return response;
        }

        public async Task<FormatedResponse> CreateRange(GenericUnitOfWork _uow, List<AtTimeWorkStandardDTO> dtos, string sid)
        {
            var add = new List<AtTimeWorkStandardDTO>();
            add.AddRange(dtos);
            var response = await _genericRepository.CreateRange(_uow, add, sid);
            return response;
        }

        public async Task<FormatedResponse> Update(GenericUnitOfWork _uow, AtTimeWorkStandardDTO dto, string sid, bool patchMode = true)
        {
            var response = await _genericRepository.Update(_uow, dto, sid, patchMode);
            return response;
        }

        public async Task<FormatedResponse> UpdateRange(GenericUnitOfWork _uow, List<AtTimeWorkStandardDTO> dtos, string sid, bool patchMode = true)
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