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

namespace API.All.HRM.ProfessionalEmployee.ProfessionalEmployeeAPI.PeCapacityFramework
{
    public class PeCapacityFrameworkRepository : IPeCapacityFrameworkRepository
    {
        private readonly GenericUnitOfWork _uow;
        private readonly FullDbContext _dbContext;
        private IGenericRepository<PE_CAPACITY_FRAMEWORK, PeCapacityFrameworkDTO> _genericRepository;
        private readonly GenericReducer<PE_CAPACITY_FRAMEWORK, PeCapacityFrameworkDTO> _genericReducer;

        public PeCapacityFrameworkRepository(FullDbContext context, GenericUnitOfWork uow)
        {
            _dbContext = context;
            _uow = uow;
            _genericRepository = _uow.GenericRepository<PE_CAPACITY_FRAMEWORK, PeCapacityFrameworkDTO>();
            _genericReducer = new();
        }

        public async Task<GenericPhaseTwoListResponse<PeCapacityFrameworkDTO>> SinglePhaseQueryList(GenericQueryListDTO<PeCapacityFrameworkDTO> request)
        {
            var joined = (from p in _dbContext.PeCapacityFrameworks
                          from sol in _dbContext.SysOtherLists.Where(x => x.ID == p.SCORE_NOT_REQUIRED).DefaultIfEmpty()
                          select new PeCapacityFrameworkDTO
                          {
                              Id = p.ID,
                              RatioFrom = p.RATIO_FROM,
                              RatioTo = p.RATIO_TO,
                              Rating = p.RATING,
                              ScoreNotRequiredStr = sol.NAME,
                              Description = p.DESCRIPTION,
                              Status = (p.IS_ACTIVE == true) ? "Áp dụng" : "Ngừng áp dụng"
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
                    var list = new List<PE_CAPACITY_FRAMEWORK>
                    {
                        (PE_CAPACITY_FRAMEWORK)response
                    };
                    var joined = (from p in list
                                  select new PeCapacityFrameworkDTO
                                  {
                                      Id = p.ID,
                                      RatioFrom = p.RATIO_FROM,
                                      RatioTo = p.RATIO_TO,
                                      Rating = p.RATING,
                                      ScoreNotRequired = p.SCORE_NOT_REQUIRED,
                                      Description = p.DESCRIPTION,
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

        public async Task<FormatedResponse> Create(GenericUnitOfWork _uow, PeCapacityFrameworkDTO dto, string sid)
        {
            // set "IsActive = true"
            // when create a record
            dto.IsActive = true;

            var response = await _genericRepository.Create(_uow, dto, sid);
            return response;
        }

        public async Task<FormatedResponse> CreateRange(GenericUnitOfWork _uow, List<PeCapacityFrameworkDTO> dtos, string sid)
        {
            var add = new List<PeCapacityFrameworkDTO>();
            add.AddRange(dtos);
            var response = await _genericRepository.CreateRange(_uow, add, sid);
            return response;
        }

        public async Task<FormatedResponse> Update(GenericUnitOfWork _uow, PeCapacityFrameworkDTO dto, string sid, bool patchMode = true)
        {
            var response = await _genericRepository.Update(_uow, dto, sid, patchMode);
            return response;
        }

        public async Task<FormatedResponse> UpdateRange(GenericUnitOfWork _uow, List<PeCapacityFrameworkDTO> dtos, string sid, bool patchMode = true)
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
            var response = await _genericRepository.ToggleActiveIds(_uow, ids, valueToBind, sid);
            return response;
        }

        public async Task<FormatedResponse> GetPositionIsEmptyOwner(long orgId)
        {
            try
            {
                var getWorkingStatus = await _dbContext.SysOtherLists.FirstOrDefaultAsync(x => x.CODE == "ESW");

                // check "getWorkingStatus"
                if (getWorkingStatus == null)
                {
                    return new FormatedResponse()
                    {
                        ErrorType = EnumErrorType.CATCHABLE,
                        StatusCode = EnumStatusCode.StatusCode400,
                        MessageCode = CommonMessageCodes.QUERY_RESULT_IS_NULL
                    };
                }

                var positionIsOwner = await (from item in _dbContext.HuEmployees
                                             where item.POSITION_ID != null && item.WORK_STATUS_ID == getWorkingStatus.ID
                                             select item.POSITION_ID)
                                             .ToListAsync();

                var listPosition = await (from item in _dbContext.HuPositions
                                          from reference_1 in _dbContext.HuOrganizations.Where(x => x.ID == item.ORG_ID).DefaultIfEmpty()

                                          where item.ORG_ID == orgId
                                                && !(positionIsOwner.Contains(item.ID))
                                          select new
                                          {
                                              Id = item.ID,
                                              Name = item.NAME
                                          })
                                          .ToListAsync();

                return new FormatedResponse()
                {
                    InnerBody = listPosition
                };
            }
            catch (Exception ex)
            {
                return new() { MessageCode = ex.Message, ErrorType = EnumErrorType.UNCATCHABLE, StatusCode = EnumStatusCode.StatusCode500 };
            }
        }
    }
}