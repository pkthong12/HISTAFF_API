using CORE.GenericUOW;
using CORE.DTO;
using API.DTO;
using CORE.Enum;
using CORE.StaticConstant;
using API.All.DbContexts;
using CORE.AutoMapper;
using System;
using API.Entities;
using Microsoft.AspNetCore.Http.HttpResults;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Logical;
using API.All.HRM.Payroll.PayrollAPI.PaListSal;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Information;

namespace API.All.SYSTEM.CoreAPI.SysConfigurationCommon
{
    public class SysConfigurationCommonRepository : ISysConfigurationCommonRepository
    {
        private readonly GenericUnitOfWork _uow;
        private readonly FullDbContext _dbContext;
        private IGenericRepository<SYS_CONFIGURATION_COMMON, SysConfigurationCommonDTO> _genericRepository;
        private readonly GenericReducer<SYS_CONFIGURATION_COMMON, SysConfigurationCommonDTO> _genericReducer;

        public SysConfigurationCommonRepository(FullDbContext context, GenericUnitOfWork uow)
        {
            _dbContext = context;
            _uow = uow;
            _genericRepository = _uow.GenericRepository<SYS_CONFIGURATION_COMMON, SysConfigurationCommonDTO>();
            _genericReducer = new();
        }

        public async Task<GenericPhaseTwoListResponse<SysConfigurationCommonDTO>> SinglePhaseQueryList(GenericQueryListDTO<SysConfigurationCommonDTO> request)
        {
            var joined = (from p in _dbContext.SysConfigurationCommons.AsNoTracking()
                          select new SysConfigurationCommonDTO
                          {
                              // trường 0:
                              // ID được kế thừa
                              Id = p.ID,

                              // trường 1:
                              YourMaximumTurn = p.YOUR_MAXIMUM_TURN,

                              // trường 2:
                              PortalPort = p.PORTAL_PORT,

                              // trường 3:
                              ApplicationPort = p.APPLICATION_PORT,

                              // trường 4:
                              MinimumLength = p.MINIMUM_LENGTH,

                              // trường 5:
                              IsUppercase = p.IS_UPPERCASE,

                              // trường 6:
                              IsNumber = p.IS_NUMBER,

                              // trường 7:
                              IsLowercase = p.IS_LOWERCASE,

                              // trường 8:
                              IsSpecialChar = p.IS_SPECIAL_CHAR,

                              // trường 9:
                              // ngày tạo
                              CreatedDate = p.CREATED_DATE,

                              // trường 10:
                              // người tạo (được tạo bởi)
                              CreatedBy = p.CREATED_BY,

                              // trường 11:
                              // nhật ký tạo (log tạo)
                              CreatedLog = p.CREATED_LOG,

                              // trường 12:
                              // ngày cập nhật
                              UpdatedDate = p.UPDATED_DATE,

                              // trường 13:
                              // người cập nhật (được cập nhật bởi)
                              UpdatedBy = p.UPDATED_BY,

                              // trường 14:
                              // nhật ký cập nhật (log cập nhật)
                              UpdatedLog = p.UPDATED_LOG,
                          }).OrderByDescending(x => x.CreatedDate)
                         ;

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
                    var list = new List<SYS_CONFIGURATION_COMMON>
                    {
                        (SYS_CONFIGURATION_COMMON)response
                    };
                    var joined = (from p in list
                                  select new SysConfigurationCommonDTO
                                  {
                                      // trường 0:
                                      // ID được kế thừa
                                      Id = p.ID,

                                      // trường 1:
                                      YourMaximumTurn = p.YOUR_MAXIMUM_TURN,

                                      // trường 2:
                                      PortalPort = p.PORTAL_PORT,

                                      // trường 3:
                                      ApplicationPort = p.APPLICATION_PORT,

                                      // trường 4:
                                      MinimumLength = p.MINIMUM_LENGTH,

                                      // trường 5:
                                      IsUppercase = p.IS_UPPERCASE,

                                      // trường 6:
                                      IsNumber = p.IS_NUMBER,

                                      // trường 7:
                                      IsLowercase = p.IS_LOWERCASE,

                                      // trường 8:
                                      IsSpecialChar = p.IS_SPECIAL_CHAR,

                                      // trường 9:
                                      // ngày tạo
                                      CreatedDate = p.CREATED_DATE,

                                      // trường 10:
                                      // người tạo (được tạo bởi)
                                      CreatedBy = p.CREATED_BY,

                                      // trường 11:
                                      // nhật ký tạo (log tạo)
                                      CreatedLog = p.CREATED_LOG,

                                      // trường 12:
                                      // ngày cập nhật
                                      UpdatedDate = p.UPDATED_DATE,

                                      // trường 13:
                                      // người cập nhật (được cập nhật bởi)
                                      UpdatedBy = p.UPDATED_BY,

                                      // trường 14:
                                      // nhật ký cập nhật (log cập nhật)
                                      UpdatedLog = p.UPDATED_LOG,
                                  }).FirstOrDefault();

                    if (joined != null)
                    {
                        if (res.StatusCode == EnumStatusCode.StatusCode200)
                        {
                            // đây là chỗ truy vấn
                            // để lấy ra được tên tương ứng với id
                            //var child = (from tham_chieu_1 in _dbContext.HuEmployees.DefaultIfEmpty()

                            //             select new
                            //             {

                            //             }).FirstOrDefault();

                            // tạm thời bảng này chưa cần tham chiếu

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

        public async Task<FormatedResponse> Create(GenericUnitOfWork _uow, SysConfigurationCommonDTO dto, string sid)
        {
            // dto.IsActive = true;
            var response = await _genericRepository.Create(_uow, dto, sid);
            return response;
        }

        public async Task<FormatedResponse> CreateRange(GenericUnitOfWork _uow, List<SysConfigurationCommonDTO> dtos, string sid)
        {
            var add = new List<SysConfigurationCommonDTO>();
            add.AddRange(dtos);
            var response = await _genericRepository.CreateRange(_uow, add, sid);
            return response;
        }

        public async Task<FormatedResponse> Update(GenericUnitOfWork _uow, SysConfigurationCommonDTO dto, string sid, bool patchMode = true)
        {
            var response = await _genericRepository.Update(_uow, dto, sid, patchMode);
            return response;
        }

        public async Task<FormatedResponse> UpdateRange(GenericUnitOfWork _uow, List<SysConfigurationCommonDTO> dtos, string sid, bool patchMode = true)
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