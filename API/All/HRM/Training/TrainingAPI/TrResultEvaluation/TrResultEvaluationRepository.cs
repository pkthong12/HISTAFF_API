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

namespace API.All.HRM.Training.TrainingAPI.TrResultEvaluation
{
    public class TrResultEvaluationRepository : ITrResultEvaluationRepository
    {
        private readonly GenericUnitOfWork _uow;
        private readonly FullDbContext _dbContext;
        private IGenericRepository<TR_RESULT_EVALUATION, TrResultEvaluationDTO> _genericRepository;
        private readonly GenericReducer<TR_RESULT_EVALUATION, TrResultEvaluationDTO> _genericReducer;

        public TrResultEvaluationRepository(FullDbContext context, GenericUnitOfWork uow)
        {
            _dbContext = context;
            _uow = uow;
            _genericRepository = _uow.GenericRepository<TR_RESULT_EVALUATION, TrResultEvaluationDTO>();
            _genericReducer = new();
        }

        public async Task<GenericPhaseTwoListResponse<TrResultEvaluationDTO>> SinglePhaseQueryList(GenericQueryListDTO<TrResultEvaluationDTO> request)
        {
            var joined = (from p in _dbContext.TrResultEvaluations
                          select new TrResultEvaluationDTO
                          {
                              Id = p.ID,
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
                    var list = new List<TR_RESULT_EVALUATION>
                    {
                        (TR_RESULT_EVALUATION)response
                    };
                    var joined = (from p in list
                                  select new TrResultEvaluationDTO
                                  {
                                      Id = p.ID,
                                      TrSettingCriDetailId = p.TR_SETTING_CRI_DETAIL_ID,
                                      PointEvaluate = p.POINT_EVALUATE,
                                      GeneralOpinion = p.GENERAL_OPINION,
                                      TrAssessmentResultId = p.TR_ASSESSMENT_RESULT_ID
                                  }).FirstOrDefault();

                    if (joined != null)
                    {
                        var child = (from tscd in _dbContext.TrSettingCriDetails.Where(x => x.ID == joined.TrSettingCriDetailId).DefaultIfEmpty()
                                     from tc in _dbContext.TrCriterias.Where(x => x.ID == tscd.CRITERIA_ID).DefaultIfEmpty()
                                     from tar in _dbContext.TrAssessmentResults.Where(x => x.ID == joined.TrAssessmentResultId).DefaultIfEmpty()
                                     select new
                                     {
                                         CriteriaCode = tc.CODE,
                                         CriteriaName = tc.NAME,
                                         Ratio = tscd.RATIO,
                                         PointMax = tscd.POINT_MAX,
                                         PointEvaluate = joined.PointEvaluate,
                                         GeneralOpinion = joined.GeneralOpinion,
                                         TrAssessmentResultId = joined.TrAssessmentResultId,
                                         Question1 = tar.QUESTION1,
                                         Question2 = tar.QUESTION2,
                                         Question3 = tar.QUESTION3,
                                         Question4 = tar.QUESTION4
                                     }).FirstOrDefault();

                        if (child != null)
                        {
                            joined.CriteriaCode = child.CriteriaCode;
                            joined.CriteriaName = child.CriteriaName;
                            joined.Ratio = child.Ratio;
                            joined.PointMax = child.PointMax;
                            joined.PointEvaluate = child.PointEvaluate;
                            joined.GeneralOpinion = child.GeneralOpinion;
                            joined.TrAssessmentResultId = child.TrAssessmentResultId;
                            joined.Question1 = child.Question1;
                            joined.Question2 = child.Question2;
                            joined.Question3 = child.Question3;
                            joined.Question4 = child.Question4;
                        }

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

        public async Task<FormatedResponse> Create(GenericUnitOfWork _uow, TrResultEvaluationDTO dto, string sid)
        {
            // dto.IsActive = true;
            var response = await _genericRepository.Create(_uow, dto, sid);
            return response;
        }

        public async Task<FormatedResponse> CreateRange(GenericUnitOfWork _uow, List<TrResultEvaluationDTO> dtos, string sid)
        {
            var add = new List<TrResultEvaluationDTO>();
            add.AddRange(dtos);
            var response = await _genericRepository.CreateRange(_uow, add, sid);
            return response;
        }

        public async Task<FormatedResponse> Update(GenericUnitOfWork _uow, TrResultEvaluationDTO dto, string sid, bool patchMode = true)
        {
            var record = _dbContext.TrAssessmentResults.FirstOrDefault(x => x.ID == dto.TrAssessmentResultId);

            if (record != null)
            {
                record.QUESTION1 = dto.Question1;
                record.QUESTION2 = dto.Question2;
                record.QUESTION3 = dto.Question3;
                record.QUESTION4 = dto.Question4;

                record.UPDATED_BY = sid;

                // anh Văn Tân nói phải dùng "DateTime.UtcNow"
                record.UPDATED_DATE = DateTime.UtcNow;
            }


            // delete value of TR_RESULT_EVALUATION
            dto.Question1 = null;
            dto.Question2 = null;
            dto.Question3 = null;
            dto.Question4 = null;


            var response = await _genericRepository.Update(_uow, dto, sid, patchMode);
            return response;
        }

        public async Task<FormatedResponse> UpdateRange(GenericUnitOfWork _uow, List<TrResultEvaluationDTO> dtos, string sid, bool patchMode = true)
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

        public async Task<GenericPhaseTwoListResponse<TrResultEvaluationDTO>> SinglePhaseQueryListForEmployee(GenericQueryListDTO<TrResultEvaluationDTO> request)
        {
            var joined = (from p in _dbContext.TrResultEvaluations
                          
                          from tscd in _dbContext.TrSettingCriDetails.Where(x => x.ID == p.TR_SETTING_CRI_DETAIL_ID).DefaultIfEmpty()
                          from tc in _dbContext.TrCriterias.Where(x => x.ID == tscd.CRITERIA_ID).DefaultIfEmpty()

                          select new TrResultEvaluationDTO
                          {
                              Id = p.ID,
                              CriteriaCode = tc.CODE,
                              CriteriaName = tc.NAME,
                              Ratio = tscd.RATIO,
                              PointMax = tscd.POINT_MAX,
                              PointEvaluate = p.POINT_EVALUATE,
                              GeneralOpinion = p.GENERAL_OPINION,
                              TrAssessmentResultId = p.TR_ASSESSMENT_RESULT_ID,
                          });

            var singlePhaseResult = await _genericReducer.SinglePhaseReduce(joined, request);
            return singlePhaseResult;
        }
    }
}