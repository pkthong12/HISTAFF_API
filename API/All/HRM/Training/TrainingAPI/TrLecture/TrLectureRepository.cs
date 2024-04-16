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

namespace API.All.HRM.Training.TrainingAPI.TrLecture
{
    public class TrLectureRepository : ITrLectureRepository
    {
        private readonly GenericUnitOfWork _uow;
        private readonly FullDbContext _dbContext;
        private IGenericRepository<TR_LECTURE, TrLectureDTO> _genericRepository;
        private readonly GenericReducer<TR_LECTURE, TrLectureDTO> _genericReducer;

        public TrLectureRepository(FullDbContext context, GenericUnitOfWork uow)
        {
            _dbContext = context;
            _uow = uow;
            _genericRepository = _uow.GenericRepository<TR_LECTURE, TrLectureDTO>();
            _genericReducer = new();
        }

        public async Task<GenericPhaseTwoListResponse<TrLectureDTO>> SinglePhaseQueryList(GenericQueryListDTO<TrLectureDTO> request)
        {
            var joined = (from p in _dbContext.TrLectures
                          from reference_1 in _dbContext.TrCenters.Where(x => x.ID == p.TR_CENTER_ID).DefaultIfEmpty()
                          select new TrLectureDTO
                          {
                              Id = p.ID,
                              TeacherCode = p.TEACHER_CODE,
                              TeacherName = p.TEACHER_NAME,
                              TrCenterName = reference_1.NAME_CENTER,
                              PhoneNumber = p.PHONE_NUMBER,
                              Email = p.EMAIL,
                              AddressContact = p.ADDRESS_CONTACT,
                              SupplierCode = p.SUPPLIER_CODE,
                              SupplierName = p.SUPPLIER_NAME,
                              Website = p.WEBSITE,
                              TypeOfService = p.TYPE_OF_SERVICE,
                              Note = p.NOTE,
                              IsApply = p.IS_APPLY
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
                    var list = new List<TR_LECTURE>
                    {
                        (TR_LECTURE)response
                    };
                    var joined = (from p in list
                                  select new TrLectureDTO
                                  {
                                      Id = p.ID,
                                      TrCenterId = p.TR_CENTER_ID,
                                      TeacherCode = p.TEACHER_CODE,
                                      TeacherName = p.TEACHER_NAME,
                                      PhoneNumber = p.PHONE_NUMBER,
                                      Email = p.EMAIL,
                                      AddressContact = p.ADDRESS_CONTACT,
                                      SupplierCode = p.SUPPLIER_CODE,
                                      SupplierName = p.SUPPLIER_NAME,
                                      Website = p.WEBSITE,
                                      TypeOfService = p.TYPE_OF_SERVICE,
                                      NameOfFile = p.NAME_OF_FILE,
                                      IsInternalTeacher = p.IS_INTERNAL_TEACHER,
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

        public async Task<FormatedResponse> Create(GenericUnitOfWork _uow, TrLectureDTO dto, string sid)
        {
            // dto.IsActive = true;
            var response = await _genericRepository.Create(_uow, dto, sid);
            return response;
        }

        public async Task<FormatedResponse> CreateRange(GenericUnitOfWork _uow, List<TrLectureDTO> dtos, string sid)
        {
            var add = new List<TrLectureDTO>();
            add.AddRange(dtos);
            var response = await _genericRepository.CreateRange(_uow, add, sid);
            return response;
        }

        public async Task<FormatedResponse> Update(GenericUnitOfWork _uow, TrLectureDTO dto, string sid, bool patchMode = true)
        {
            var response = await _genericRepository.Update(_uow, dto, sid, patchMode);
            return response;
        }

        public async Task<FormatedResponse> UpdateRange(GenericUnitOfWork _uow, List<TrLectureDTO> dtos, string sid, bool patchMode = true)
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

        public async Task<FormatedResponse> GetDropDownTrainingCenter()
        {
            try
            {
                var listTrainingCenter = await (from item in _dbContext.TrCenters
                                                select new
                                                {
                                                    Id = item.ID,
                                                    Name = item.NAME_CENTER
                                                })
                                                .ToListAsync();

                return new FormatedResponse()
                {
                    InnerBody = listTrainingCenter
                };
            }
            catch (Exception ex)
            {
                return new() { MessageCode = ex.Message, ErrorType = EnumErrorType.UNCATCHABLE, StatusCode = EnumStatusCode.StatusCode500 };
            }
        }

        public async Task<FormatedResponse> GetByIdTrCenter(long id)
        {
            try
            {
                var listTrainingCenter = await (from item in _dbContext.TrCenters.Where(x => x.ID == id)
                                                select new
                                                {
                                                    Id = item.ID,
                                                    Name = item.NAME_CENTER
                                                })
                                                .FirstOrDefaultAsync();

                return new FormatedResponse()
                {
                    InnerBody = listTrainingCenter
                };
            }
            catch (Exception ex)
            {
                return new() { MessageCode = ex.Message, ErrorType = EnumErrorType.UNCATCHABLE, StatusCode = EnumStatusCode.StatusCode500 };
            }
        }

        public async Task<FormatedResponse> GetListTeacherByCenter(List<long> ids)
        {
            try
            {
                var joined = await (from p in _dbContext.TrLectures.AsNoTracking()
                                    where p.IS_APPLY == true && ids.Contains(p.TR_CENTER_ID!.Value) 
                                    orderby p.CREATED_DATE descending
                                    select new
                                    {
                                        Id = p.ID,
                                        Code = p.TEACHER_CODE,
                                        Name = "[" + p.TEACHER_CODE + "] " + p.TEACHER_NAME
                                    }).ToListAsync();
                return new FormatedResponse() { InnerBody = joined, StatusCode = EnumStatusCode.StatusCode200 };
            }
            catch (Exception ex)
            {
                return new FormatedResponse() { MessageCode = ex.Message, ErrorType = EnumErrorType.UNCATCHABLE, StatusCode = EnumStatusCode.StatusCode500 };
            }
        }
    }
}