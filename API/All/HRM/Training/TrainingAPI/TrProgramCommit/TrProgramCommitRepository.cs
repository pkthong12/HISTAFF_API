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

namespace API.All.HRM.Training.TrainingAPI.TrProgramCommit
{
    public class TrProgramCommitRepository : ITrProgramCommitRepository
    {
        private readonly GenericUnitOfWork _uow;
        private readonly FullDbContext _dbContext;
        private IGenericRepository<RC_EXAMS, TrProgramCommitDTO> _genericRepository;
        private readonly GenericReducer<RC_EXAMS, TrProgramCommitDTO> _genericReducer;

        public TrProgramCommitRepository(FullDbContext context, GenericUnitOfWork uow)
        {
            _dbContext = context;
            _uow = uow;
            _genericRepository = _uow.GenericRepository<RC_EXAMS, TrProgramCommitDTO>();
            _genericReducer = new();
        }

        public async Task<GenericPhaseTwoListResponse<TrProgramCommitDTO>> SinglePhaseQueryList(GenericQueryListDTO<TrProgramCommitDTO> request)
        {
            var joined = (from p in _dbContext.TrProgramCommits
                          
                          from he in _dbContext.HuEmployees.Where(x => x.ID == p.EMPLOYEE_ID).DefaultIfEmpty()
                          from hec in _dbContext.HuEmployeeCvs.Where(x => x.ID == he.PROFILE_ID).DefaultIfEmpty()

                          from ho in _dbContext.HuOrganizations.Where(x => x.ID == he.ORG_ID).DefaultIfEmpty()

                          select new TrProgramCommitDTO
                          {
                              Id = p.ID,
                              EmployeeCode = he.CODE,
                              EmployeeName = hec.FULL_NAME,
                              OrgName = ho.NAME,
                              CalculateReimbursement = p.CALCULATE_REIMBURSEMENT,
                              CommitNo = p.COMMIT_NO,
                              SignDate = p.SIGN_DATE,
                              TrainingCosts = p.TRAINING_COSTS,
                              MoneyCommit = p.MONEY_COMMIT,
                              TimeCommit = p.TIME_COMMIT,
                              DateFrom = p.DATE_FROM,
                              DateTo = p.DATE_TO,
                              DayQuantity = p.DAY_QUANTITY
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
                    var list = new List<RC_EXAMS>
                    {
                        (RC_EXAMS)response
                    };
                    var joined = (from p in list
                                  select new TrProgramCommitDTO
                                  {
                                      Id = p.ID
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

        public async Task<FormatedResponse> Create(GenericUnitOfWork _uow, TrProgramCommitDTO dto, string sid)
        {
            // dto.IsActive = true;
            var response = await _genericRepository.Create(_uow, dto, sid);
            return response;
        }

        public async Task<FormatedResponse> CreateRange(GenericUnitOfWork _uow, List<TrProgramCommitDTO> dtos, string sid)
        {
            var add = new List<TrProgramCommitDTO>();
            add.AddRange(dtos);
            var response = await _genericRepository.CreateRange(_uow, add, sid);
            return response;
        }

        public async Task<FormatedResponse> Update(GenericUnitOfWork _uow, TrProgramCommitDTO dto, string sid, bool patchMode = true)
        {
            var response = await _genericRepository.Update(_uow, dto, sid, patchMode);
            return response;
        }

        public async Task<FormatedResponse> UpdateRange(GenericUnitOfWork _uow, List<TrProgramCommitDTO> dtos, string sid, bool patchMode = true)
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