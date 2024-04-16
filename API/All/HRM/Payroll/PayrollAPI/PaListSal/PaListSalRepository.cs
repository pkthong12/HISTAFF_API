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
using Common.Interfaces;
using Common.DataAccess;

namespace API.All.HRM.Payroll.PayrollAPI.PaListSal
{
    public class PaListSalRepository : IPaListSalRepository
    {
        private readonly GenericUnitOfWork _uow;
        private readonly FullDbContext _dbContext;
        private IGenericRepository<PA_LISTSAL, PaListSalDTO> _genericRepository;
        private readonly GenericReducer<PA_LISTSAL, PaListSalDTO> _genericReducer;
        protected AbsQueryDataTemplate QueryData;

        public PaListSalRepository(FullDbContext context, GenericUnitOfWork uow)
        {
            _dbContext = context;
            _uow = uow;
            _genericRepository = _uow.GenericRepository<PA_LISTSAL, PaListSalDTO>();
            _genericReducer = new();
            QueryData = new SqlQueryDataTemplate(_dbContext);
        }

        public async Task<GenericPhaseTwoListResponse<PaListSalDTO>> SinglePhaseQueryList(GenericQueryListDTO<PaListSalDTO> request)
        {
            var joined = (from p in _dbContext.PaListSals.AsNoTracking()
                          from tham_chieu_1 in _dbContext.SysOtherLists.Where(x => x.ID == p.DATA_TYPE_ID).DefaultIfEmpty()
                          from tham_chieu_2 in _dbContext.SysOtherLists.Where(x => x.ID == p.LIST_KYHIEU_ID).DefaultIfEmpty()
                          select new PaListSalDTO
                          {
                              Id = p.ID,
                              CodeListsal = p.CODE_LISTSAL,
                              NameEn = p.NAME_EN,
                              NameVn = p.NAME_VN,
                              DataTypeId = p.DATA_TYPE_ID,
                              DataTypeName = tham_chieu_1.NAME,
                              ListKyhieuId = p.LIST_KYHIEU_ID,
                              ListKyHieuName = tham_chieu_2.NAME,
                              ThuTu = p.THU_TU,
                              IsActive = p.IS_ACTIVE,
                              IsActiveStr = p.IS_ACTIVE ? "Áp dụng" : "Ngừng áp dụng",
                              Note = p.NOTE,
                              CreatedDate = p.CREATED_DATE,
                              CreatedBy = p.CREATED_BY,
                              CreatedLog = p.CREATED_LOG,
                              UpdatedDate = p.UPDATED_DATE,
                              UpdatedBy = p.UPDATED_BY,
                              UpdateLog = p.UPDATE_LOG,
                              Order = p.ORDER
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
                    var list = new List<PA_LISTSAL>
                    {
                        (PA_LISTSAL)response
                    };
                    var joined = (from p in list
                                  select new PaListSalDTO
                                  {
                                      Id = p.ID,
                                      CodeListsal = p.CODE_LISTSAL,
                                      NameEn = p.NAME_EN,
                                      NameVn = p.NAME_VN,
                                      DataTypeId = p.DATA_TYPE_ID,
                                      ListKyhieuId = p.LIST_KYHIEU_ID,
                                      ThuTu = p.THU_TU,
                                      IsActive = p.IS_ACTIVE,
                                      Note = p.NOTE,
                                      CreatedDate = p.CREATED_DATE,
                                      CreatedBy = p.CREATED_BY,
                                      CreatedLog = p.CREATED_LOG,
                                      UpdatedDate = p.UPDATED_DATE,
                                      UpdatedBy = p.UPDATED_BY,
                                      UpdateLog = p.UPDATE_LOG,
                                      Order = p.ORDER
                                  }).FirstOrDefault();

                    if (joined != null)
                    {
                        if (res.StatusCode == EnumStatusCode.StatusCode200)
                        {
                            // đây là chỗ truy vấn
                            // để lấy ra được tên tương ứng với id
                            var child = (from tham_chieu_1 in _dbContext.SysOtherLists.Where(x => x.ID == joined.DataTypeId).DefaultIfEmpty()
                                         from tham_chieu_2 in _dbContext.SysOtherLists.Where(x => x.ID == joined.ListKyhieuId).DefaultIfEmpty()
                                         select new
                                         {
                                             DataTypeName = tham_chieu_1.NAME,
                                             ListKyHieuName = tham_chieu_2.NAME,
                                         }).FirstOrDefault();
                            joined.DataTypeName = child?.DataTypeName;
                            joined.ListKyHieuName = child?.ListKyHieuName;
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

        public async Task<FormatedResponse> Create(GenericUnitOfWork _uow, PaListSalDTO dto, string sid)
        {
            try
            {
                var r = await QueryData.ExecuteNonQuery("PKG_PAYROLL_ADD_COLUMN",
                    new
                    {
                        COL_NAME = dto.CodeListsal!.ToUpper().Replace(' ', '_'),
                        DATA_TYPE = dto.DataTypeId == 1054 ? "FLOAT" : "NVARCHAR(50)",
                        FROM_TABLE = "PA_LISTSAL",
                    }, false);
            }catch(Exception ex)
            {
                return new FormatedResponse()
                {
                    MessageCode = "COLUMN_IS_EXIST",
                    StatusCode = EnumStatusCode.StatusCode400,
                    ErrorType = EnumErrorType.CATCHABLE,
                };
            }

            dto.IsActive = true;
            var response = await _genericRepository.Create(_uow, dto, sid);
            return response;
        }

        public async Task<FormatedResponse> CreateRange(GenericUnitOfWork _uow, List<PaListSalDTO> dtos, string sid)
        {
            var add = new List<PaListSalDTO>();
            add.AddRange(dtos);
            var response = await _genericRepository.CreateRange(_uow, add, sid);
            return response;
        }

        public async Task<FormatedResponse> Update(GenericUnitOfWork _uow, PaListSalDTO dto, string sid, bool patchMode = true)
        {
            var response = await _genericRepository.Update(_uow, dto, sid, patchMode);
            return response;
        }

        public async Task<FormatedResponse> UpdateRange(GenericUnitOfWork _uow, List<PaListSalDTO> dtos, string sid, bool patchMode = true)
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
            return new FormatedResponse()
            {
                MessageCode = response.MessageCode,
                InnerBody = response.InnerBody,
                StatusCode = EnumStatusCode.StatusCode200
            };
        }
    }
}

