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
using ProfileDAL.ViewModels;

namespace API.All.HRM.Training.TrainingAPI.TrTranningRecord
{
    public class TrTranningRecordRepository : ITrTranningRecordRepository
    {
        private readonly GenericUnitOfWork _uow;
        private readonly FullDbContext _dbContext;
        private IGenericRepository<TR_TRANNING_RECORD, TrTranningRecordDTO> _genericRepository;
        private readonly GenericReducer<TR_TRANNING_RECORD, TrTranningRecordDTO> _genericReducer;

        public TrTranningRecordRepository(FullDbContext context, GenericUnitOfWork uow)
        {
            _dbContext = context;
            _uow = uow;
            _genericRepository = _uow.GenericRepository<TR_TRANNING_RECORD, TrTranningRecordDTO>();
            _genericReducer = new();
        }

        public async Task<GenericPhaseTwoListResponse<TrTranningRecordDTO>> SinglePhaseQueryList(GenericQueryListDTO<TrTranningRecordDTO> request)
        {
            var joined = (from p in _dbContext.TrTranningRecords
                          
                          from reference_1 in _dbContext.HuEmployees.Where(x => x.ID == p.EMPLOYEE_ID).DefaultIfEmpty()
                          from reference_2 in _dbContext.HuEmployeeCvs.Where(x => x.ID == reference_1.PROFILE_ID).DefaultIfEmpty()

                          from reference_3 in _dbContext.HuPositions.Where(x => x.ID == reference_1.POSITION_ID).DefaultIfEmpty()
                          from reference_4 in _dbContext.HuOrganizations.Where(x => x.ID == reference_1.ORG_ID).DefaultIfEmpty()
                          from reference_5 in _dbContext.TrCourses.Where(x => x.ID == p.TR_COURSE_ID).DefaultIfEmpty()

                          select new TrTranningRecordDTO
                          {
                              Id = p.ID,
                              EmployeeCode = reference_1.CODE,
                              EmployeeName = reference_2.FULL_NAME,
                              PositionName = reference_3.NAME,
                              OrgName = reference_4.NAME,
                              CourseName = reference_5.COURSE_NAME,
                              DateFrom = p.DATE_FROM,
                              DateTo = p.DATE_TO,
                              Content = p.CONTENT,
                              TargetText = p.TARGET_TEXT,
                              TrainingPlace = p.TRAINING_PLACE,
                              TrainingCenter = p.TRAINING_CENTER,
                              Result = p.RESULT,
                              Scores = p.SCORES,
                              Rating = p.RATING,
                              EvaluateDue1 = p.EVALUATE_DUE1,
                              EvaluateDue2 = p.EVALUATE_DUE2,
                              EvaluateDue3 = p.EVALUATE_DUE3,
                              CertificateText = p.CERTIFICATE_TEXT,
                              CertificateIssuanceDate = p.CERTIFICATE_ISSUANCE_DATE,
                              CommitmentNumber = p.COMMITMENT_NUMBER,
                              CommitmentAmount = p.COMMITMENT_AMOUNT,
                              MonthCommitment = p.MONTH_COMMITMENT,
                              CommitmentStartDate = p.COMMITMENT_START_DATE,
                              CommitmentEndDate = p.COMMITMENT_END_DATE,
                              EmployeeId = p.EMPLOYEE_ID,
                              TrCourseId = p.TR_COURSE_ID,
                              WorkStatusId = reference_1.WORK_STATUS_ID
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
                    var list = new List<TR_TRANNING_RECORD>
                    {
                        (TR_TRANNING_RECORD)response
                    };
                    var joined = (from p in list
                                  select new TrTranningRecordDTO
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

        public async Task<FormatedResponse> Create(GenericUnitOfWork _uow, TrTranningRecordDTO dto, string sid)
        {
            // dto.IsActive = true;
            var response = await _genericRepository.Create(_uow, dto, sid);
            return response;
        }

        public async Task<FormatedResponse> CreateRange(GenericUnitOfWork _uow, List<TrTranningRecordDTO> dtos, string sid)
        {
            var add = new List<TrTranningRecordDTO>();
            add.AddRange(dtos);
            var response = await _genericRepository.CreateRange(_uow, add, sid);
            return response;
        }

        public async Task<FormatedResponse> Update(GenericUnitOfWork _uow, TrTranningRecordDTO dto, string sid, bool patchMode = true)
        {
            var response = await _genericRepository.Update(_uow, dto, sid, patchMode);
            return response;
        }

        public async Task<FormatedResponse> UpdateRange(GenericUnitOfWork _uow, List<TrTranningRecordDTO> dtos, string sid, bool patchMode = true)
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

        public async Task<FormatedResponse> GetDropDownCourse()
        {
            try
            {
                var listCourse = await _dbContext.TrCourses
                                .Select(x =>
                                new {
                                    Id = x.ID,
                                    Name = x.COURSE_NAME
                                })
                                .ToListAsync();

                return new FormatedResponse()
                {
                    InnerBody = listCourse
                };
            }
            catch (Exception ex)
            {
                return new() { MessageCode = ex.Message, ErrorType = EnumErrorType.UNCATCHABLE, StatusCode = EnumStatusCode.StatusCode500 };
            }
        }
    }
}