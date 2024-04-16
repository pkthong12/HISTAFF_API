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
using DocumentFormat.OpenXml.Office.CustomUI;
using DocumentFormat.OpenXml.Wordprocessing;

namespace API.All.HRM.Recruitment.RecruitmentAPI.RcRequest
{
    public class RcRequestRepository : IRcRequestRepository
    {
        private readonly GenericUnitOfWork _uow;
        private readonly FullDbContext _dbContext;
        private IGenericRepository<RC_REQUEST, RcRequestDTO> _genericRepository;
        private readonly GenericReducer<RC_REQUEST, RcRequestDTO> _genericReducer;

        public RcRequestRepository(FullDbContext context, GenericUnitOfWork uow)
        {
            _dbContext = context;
            _uow = uow;
            _genericRepository = _uow.GenericRepository<RC_REQUEST, RcRequestDTO>();
            _genericReducer = new();
        }

        public async Task<GenericPhaseTwoListResponse<RcRequestDTO>> SinglePhaseQueryList(GenericQueryListDTO<RcRequestDTO> request)
        {
            var joined = (from p in _dbContext.RcRequests
                          from reference_1 in _dbContext.HuOrganizations.Where(x => x.ID == p.ORG_ID).DefaultIfEmpty()
                          from reference_2 in _dbContext.HuPositions.Where(x => x.ID == p.POSITION_ID).DefaultIfEmpty()
                          
                          from reference_3 in _dbContext.HuEmployees.Where(x => x.ID == p.PETITIONER_ID).DefaultIfEmpty()
                          from reference_4 in _dbContext.HuEmployeeCvs.Where(x => x.ID == reference_3.PROFILE_ID).DefaultIfEmpty()

                          from reference_5 in _dbContext.SysOtherLists.Where(x => x.ID == p.RECRUITMENT_REASON).DefaultIfEmpty()

                          from reference_6 in _dbContext.HuEmployees.Where(x => x.ID == p.PERSON_IN_CHARGE).DefaultIfEmpty()
                          from reference_7 in _dbContext.HuEmployeeCvs.Where(x => x.ID == reference_6.PROFILE_ID).DefaultIfEmpty()

                          from reference_8 in _dbContext.SysOtherLists.Where(x => x.ID == p.STATUS_ID).DefaultIfEmpty()

                          select new RcRequestDTO
                          {
                              Id = p.ID,
                              Code = p.CODE,
                              ResterName = p.RESTER_NAME,
                              PetitionerName = reference_4.FULL_NAME,
                              DateVote = p.DATE_VOTE,
                              DateNeedResponse = p.DATE_NEED_RESPONSE,
                              OrgName = reference_1.NAME,
                              PositionName = reference_2.NAME,
                              NumberNeed = p.NUMBER_NEED,
                              RecruitmentReasonStr = reference_5.NAME,
                              PersonInChargeStr = reference_7.FULL_NAME,
                              StatusName = reference_8.NAME,
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
                    var list = new List<RC_REQUEST>
                    {
                        (RC_REQUEST)response
                    };
                    var joined = (from p in list
                                  select new RcRequestDTO
                                  {
                                      Id = p.ID,
                                      Code = p.CODE,
                                      ResterName = p.RESTER_NAME,
                                      ListRadio = (p.IS_IN_BOUNDARY == true) ? 1 : ((p.IS_OUT_BOUNDARY == true) ? 2 : null),
                                      OrgId = p.ORG_ID,
                                      PositionId = p.POSITION_ID,
                                      PositionGroupOfRecruitment = p.POSITION_GROUP_OF_RECRUITMENT,
                                      PetitionerId = p.PETITIONER_ID,
                                      DateVote = p.DATE_VOTE,
                                      DateNeedResponse = p.DATE_NEED_RESPONSE,
                                      RecruitmentForm = p.RECRUITMENT_FORM,
                                      Workplace = p.WORKPLACE,
                                      SalaryLevel = p.SALARY_LEVEL,
                                      QuantityAvailable = p.QUANTITY_AVAILABLE,
                                      BoundaryQuantity = p.BOUNDARY_QUANTITY,
                                      PlanLeave = p.PLAN_LEAVE,
                                      RecruitmentReason = p.RECRUITMENT_REASON,
                                      NumberNeed = p.NUMBER_NEED,
                                      IsBeyondBoundary = p.IS_BEYOND_BOUNDARY,
                                      IsRequireComputer = p.IS_REQUIRE_COMPUTER,
                                      DetailExplanation = p.DETAIL_EXPLANATION,
                                      EducationLevelId = p.EDUCATION_LEVEL_ID,
                                      SpecializeLevelId = p.SPECIALIZE_LEVEL_ID,
                                      OtherProfessionalQualifications = p.OTHER_PROFESSIONAL_QUALIFICATIONS,
                                      AgeFrom = p.AGE_FROM,
                                      AgeTo = p.AGE_TO,
                                      LanguageId = p.LANGUAGE_ID,
                                      LanguageLevelId = p.LANGUAGE_LEVEL_ID,
                                      LanguagePoint = p.LANGUAGE_POINT,
                                      ForeignLanguageAbility = p.FOREIGN_LANGUAGE_ABILITY,
                                      MinimumYearExp = p.MINIMUM_YEAR_EXP,
                                      GenderPriorityId = p.GENDER_PRIORITY_ID,
                                      ComputerLevel = p.COMPUTER_LEVEL,
                                      JobDescription = p.JOB_DESCRIPTION,
                                      LevelPriority = p.LEVEL_PRIORITY,
                                      NameOfFile = p.NAME_OF_FILE,
                                      OtherRequire = p.OTHER_REQUIRE,
                                      IsApprove = p.IS_APPROVE,
                                      PersonInCharge = p.PERSON_IN_CHARGE
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

        public async Task<FormatedResponse> Create(GenericUnitOfWork _uow, RcRequestDTO dto, string sid)
        {
            var getWaitApprove = await _dbContext.SysOtherLists.FirstOrDefaultAsync(x => x.CODE == "CD");

            if (getWaitApprove != null)
            {
                dto.StatusId = getWaitApprove.ID;
            }

            var response = await _genericRepository.Create(_uow, dto, sid);
            return response;
        }

        public async Task<FormatedResponse> CreateRange(GenericUnitOfWork _uow, List<RcRequestDTO> dtos, string sid)
        {
            var add = new List<RcRequestDTO>();
            add.AddRange(dtos);
            var response = await _genericRepository.CreateRange(_uow, add, sid);
            return response;
        }

        public async Task<FormatedResponse> Update(GenericUnitOfWork _uow, RcRequestDTO dto, string sid, bool patchMode = true)
        {
            var response = await _genericRepository.Update(_uow, dto, sid, patchMode);
            return response;
        }

        public async Task<FormatedResponse> UpdateRange(GenericUnitOfWork _uow, List<RcRequestDTO> dtos, string sid, bool patchMode = true)
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

        public async Task<FormatedResponse> GetDropDownRecruitmentForm(string code)
        {
            try
            {
                var listRecruitmentForm = await (from item in _dbContext.SysOtherListTypes.Where(x => x.CODE.ToUpper() == code.ToUpper())
                                                 from mainItem in _dbContext.SysOtherLists.Where(x => x.TYPE_ID == item.ID).DefaultIfEmpty()
                                                 select mainItem
                                                 ).ToListAsync();

                return new FormatedResponse()
                {
                    InnerBody = listRecruitmentForm
                };
            }
            catch (Exception ex)
            {
                return new() { MessageCode = ex.Message, ErrorType = EnumErrorType.UNCATCHABLE, StatusCode = EnumStatusCode.StatusCode500 };
            }
        }

        public async Task<FormatedResponse> GetWorkingAddressAccordingToCompany(long orgId)
        {
            try
            {
                var listWorkingAddress = await (from item in _dbContext.HuOrganizations.Where(x => x.ID == orgId)
                                                from mainItem in _dbContext.HuCompanys.Where(x => x.ID == item.COMPANY_ID).DefaultIfEmpty()
                                                select new
                                                {
                                                    Id = mainItem.ID,
                                                    Name = mainItem.WORK_ADDRESS
                                                }
                                                ).ToListAsync();

                return new FormatedResponse()
                {
                    InnerBody = listWorkingAddress
                };
            }
            catch (Exception ex)
            {
                return new() { MessageCode = ex.Message, ErrorType = EnumErrorType.UNCATCHABLE, StatusCode = EnumStatusCode.StatusCode500 };
            }
        }

        public async Task<FormatedResponse> GetDropDownRecruitmentReason(string code)
        {
            try
            {
                var listRecruitmentForm = await (from item in _dbContext.SysOtherListTypes.Where(x => x.CODE.ToUpper() == code.ToUpper())
                                                 from mainItem in _dbContext.SysOtherLists.Where(x => x.TYPE_ID == item.ID).DefaultIfEmpty()
                                                 select mainItem
                                                 ).ToListAsync();

                return new FormatedResponse()
                {
                    InnerBody = listRecruitmentForm
                };
            }
            catch (Exception ex)
            {
                return new() { MessageCode = ex.Message, ErrorType = EnumErrorType.UNCATCHABLE, StatusCode = EnumStatusCode.StatusCode500 };
            }
        }

        public async Task<FormatedResponse> ReadWorkAddress(long id)
        {
            try
            {
                var readWorkAddress = await (from item in _dbContext.HuCompanys.Where(x => x.ID == id)
                                             select new
                                             {
                                                 Id = item.ID,
                                                 Name = item.WORK_ADDRESS
                                             }
                                             ).FirstAsync();

                return new FormatedResponse()
                {
                    InnerBody = readWorkAddress
                };
            }
            catch (Exception ex)
            {
                return new() { MessageCode = ex.Message, ErrorType = EnumErrorType.UNCATCHABLE, StatusCode = EnumStatusCode.StatusCode500 };
            }
        }
    }
}