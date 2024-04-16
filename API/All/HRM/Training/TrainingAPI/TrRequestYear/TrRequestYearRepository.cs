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
using Google.Type;

namespace API.All.HRM.Training.TrainingAPI.TrRequestYear
{
    public class TrRequestYearRepository : ITrRequestYearRepository
    {
        private readonly GenericUnitOfWork _uow;
        private readonly FullDbContext _dbContext;
        private IGenericRepository<TR_REQUEST_YEAR, TrRequestYearDTO> _genericRepository;
        private readonly GenericReducer<TR_REQUEST_YEAR, TrRequestYearDTO> _genericReducer;

        public TrRequestYearRepository(FullDbContext context, GenericUnitOfWork uow)
        {
            _dbContext = context;
            _uow = uow;
            _genericRepository = _uow.GenericRepository<TR_REQUEST_YEAR, TrRequestYearDTO>();
            _genericReducer = new();
        }

        public async Task<GenericPhaseTwoListResponse<TrRequestYearDTO>> SinglePhaseQueryList(GenericQueryListDTO<TrRequestYearDTO> request)
        {
            var joined = (from p in _dbContext.TrRequestYears
                          from ho in _dbContext.HuOrganizations.Where(x => x.ID == p.ORG_ID).DefaultIfEmpty()
                          from tc in _dbContext.TrCourses.Where(x => x.ID == p.TR_COURSE_ID).DefaultIfEmpty()
                          from hc in _dbContext.HuCompanys.Where(x => x.ID == p.COMPANY_ID).DefaultIfEmpty()
                          from sol1 in _dbContext.SysOtherLists.Where(x => x.ID == p.QUARTER_ID).DefaultIfEmpty()
                          from sol2 in _dbContext.SysOtherLists.Where(x => x.ID == p.INITIALIZATION_LOCATION).DefaultIfEmpty()
                          from sol3 in _dbContext.SysOtherLists.Where(x => x.ID == p.PRIORITY_LEVEL).DefaultIfEmpty()
                          from sol4 in _dbContext.SysOtherLists.Where(x => x.ID == p.STATUS_ID).DefaultIfEmpty()
                          select new TrRequestYearDTO
                          {
                              Id = p.ID,
                              OrgName = ho.NAME,
                              DateOfRequest = p.DATE_OF_REQUEST,
                              TrCourseName = tc.COURSE_NAME,
                              Content = p.CONTENT,
                              CompanyName = hc.NAME_VN,
                              Participants = p.PARTICIPANTS,
                              QuantityPeople = p.QUANTITY_PEOPLE,
                              QuarterName = sol1.NAME,
                              InitializationLocationStr = sol2.NAME,
                              PriorityLevelStr = sol3.NAME,
                              Money = p.MONEY,
                              Note = p.NOTE,
                              StatusName = sol4.NAME,
                              OrgId = p.ORG_ID
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
                    var list = new List<TR_REQUEST_YEAR>
                    {
                        (TR_REQUEST_YEAR)response
                    };
                    var joined = (from p in list
                                  select new TrRequestYearDTO
                                  {
                                      Id = p.ID,
                                      Year = p.YEAR,
                                      QuarterId = p.QUARTER_ID,
                                      OrgId = p.ORG_ID,
                                      DateOfRequest = p.DATE_OF_REQUEST,
                                      TrCourseId = p.TR_COURSE_ID,
                                      Content = p.CONTENT,
                                      CompanyId = p.COMPANY_ID,
                                      Participants = p.PARTICIPANTS,
                                      QuantityPeople = p.QUANTITY_PEOPLE,
                                      InitializationLocation = p.INITIALIZATION_LOCATION,
                                      PriorityLevel = p.PRIORITY_LEVEL,
                                      StatusId = p.STATUS_ID,
                                      Money = p.MONEY,
                                      Note = p.NOTE
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

        public async Task<FormatedResponse> Create(GenericUnitOfWork _uow, TrRequestYearDTO dto, string sid)
        {
            // dto.IsActive = true;
            var response = await _genericRepository.Create(_uow, dto, sid);
            return response;
        }

        public async Task<FormatedResponse> CreateRange(GenericUnitOfWork _uow, List<TrRequestYearDTO> dtos, string sid)
        {
            var add = new List<TrRequestYearDTO>();
            add.AddRange(dtos);
            var response = await _genericRepository.CreateRange(_uow, add, sid);
            return response;
        }

        public async Task<FormatedResponse> Update(GenericUnitOfWork _uow, TrRequestYearDTO dto, string sid, bool patchMode = true)
        {
            var response = await _genericRepository.Update(_uow, dto, sid, patchMode);
            return response;
        }

        public async Task<FormatedResponse> UpdateRange(GenericUnitOfWork _uow, List<TrRequestYearDTO> dtos, string sid, bool patchMode = true)
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

        public async Task<FormatedResponse> GetDropDownListTrainingCourse()
        {
            try
            {
                var listTrainingCourse = await (from item in _dbContext.TrCourses
                                                where item.IS_ACTIVE == true
                                                select new
                                                {
                                                    Id = item.ID,
                                                    Name = item.COURSE_NAME
                                                }).ToListAsync();

                return new FormatedResponse()
                {
                    InnerBody = listTrainingCourse
                };
            }
            catch (Exception ex)
            {
                return new() {
                    ErrorType = EnumErrorType.UNCATCHABLE,
                    MessageCode = ex.Message,
                    StatusCode = EnumStatusCode.StatusCode500
                };
            }
        }

        public async Task<FormatedResponse> GetDropDownListCompany()
        {
            try
            {
                var listCompany = await (from item in _dbContext.HuCompanys
                                         where item.IS_ACTIVE == true
                                         select new
                                         {
                                             Id = item.ID,
                                             Name = item.NAME_VN
                                         }).ToListAsync();

                return new FormatedResponse()
                {
                    InnerBody = listCompany
                };
            }
            catch (Exception ex)
            {
                return new()
                {
                    ErrorType = EnumErrorType.UNCATCHABLE,
                    MessageCode = ex.Message,
                    StatusCode = EnumStatusCode.StatusCode500
                };
            }
        }
    }
}