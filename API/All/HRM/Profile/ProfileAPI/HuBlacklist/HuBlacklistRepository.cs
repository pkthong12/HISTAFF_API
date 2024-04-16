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
using ProfileDAL.ViewModels;

namespace API.All.HRM.Profile.ProfileAPI.HuBlacklist
{
    public class HuBlacklistRepository : IHuBlacklistRepository
    {
        private readonly GenericUnitOfWork _uow;
        private readonly FullDbContext _dbContext;
        private IGenericRepository<HU_BLACKLIST, HuBlacklistDTO> _genericRepository;
        private readonly GenericReducer<HU_BLACKLIST, HuBlacklistDTO> _genericReducer;

        public HuBlacklistRepository(FullDbContext context, GenericUnitOfWork uow)
        {
            _dbContext = context;
            _uow = uow;
            _genericRepository = _uow.GenericRepository<HU_BLACKLIST, HuBlacklistDTO>();
            _genericReducer = new();
        }

        public async Task<GenericPhaseTwoListResponse<HuBlacklistDTO>> SinglePhaseQueryList(GenericQueryListDTO<HuBlacklistDTO> request)
        {
            var joined = (from p in _dbContext.HuBlacklists
                          
                          from reference_1 in _dbContext.HuEmployees.Where(x => x.ID == p.EMPLOYEE_ID).DefaultIfEmpty()
                          from reference_2 in _dbContext.HuEmployeeCvs.Where(x => x.ID == reference_1.PROFILE_ID).DefaultIfEmpty()

                          from reference_3 in _dbContext.HuOrganizations.Where(x => x.ID == reference_1.ORG_ID).DefaultIfEmpty()
                          from reference_4 in _dbContext.HuPositions.Where(x => x.ID == reference_1.POSITION_ID).DefaultIfEmpty()
                          from reference_5 in _dbContext.SysOtherLists.Where(x => x.ID == p.STATUS_ID).DefaultIfEmpty()

                          select new HuBlacklistDTO
                          {
                              Id = p.ID,
                              EmployeeCode = reference_1.CODE,
                              EmployeeName = reference_2.FULL_NAME,
                              EmployeeNo = reference_2.ID_NO,
                              DateJoin = p.DATE_JOIN,
                              OrgId = reference_1.ORG_ID,
                              OrgName = reference_3.NAME,
                              PositionName = reference_4.NAME,
                              DateSendForm = p.DATE_SEND_FORM,
                              EndDateWork = p.END_DATE_WORK,
                              LastWorkingDay = p.LAST_WORKING_DAY,
                              StatusName = reference_5.NAME,
                              Note = p.NOTE,
                              ReasonForBlacklist = p.REASON_FOR_BLACKLIST
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
                    var list = new List<HU_BLACKLIST>
                    {
                        (HU_BLACKLIST)response
                    };
                    var joined = (from p in list
                                  select new HuBlacklistDTO
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

        public async Task<FormatedResponse> Create(GenericUnitOfWork _uow, HuBlacklistDTO dto, string sid)
        {
            // dto.IsActive = true;
            var response = await _genericRepository.Create(_uow, dto, sid);
            return response;
        }

        public async Task<FormatedResponse> CreateRange(GenericUnitOfWork _uow, List<HuBlacklistDTO> dtos, string sid)
        {
            var add = new List<HuBlacklistDTO>();
            add.AddRange(dtos);
            var response = await _genericRepository.CreateRange(_uow, add, sid);
            return response;
        }

        public async Task<FormatedResponse> Update(GenericUnitOfWork _uow, HuBlacklistDTO dto, string sid, bool patchMode = true)
        {
            var response = await _genericRepository.Update(_uow, dto, sid, patchMode);
            return response;
        }

        public async Task<FormatedResponse> UpdateRange(GenericUnitOfWork _uow, List<HuBlacklistDTO> dtos, string sid, bool patchMode = true)
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