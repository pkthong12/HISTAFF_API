using CORE.GenericUOW;
using CORE.DTO;
using API.DTO;
using CORE.Enum;
using CORE.StaticConstant;
using API.All.DbContexts;
using CORE.AutoMapper;
using Microsoft.AspNetCore.Http;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Information;
using System.Collections.Generic;
using AttendanceDAL.ViewModels;

namespace API.Controllers.TrCourse
{
    public class TrCourseRepository : ITrCourseRepository
    {
        private readonly GenericUnitOfWork _uow;
        private readonly FullDbContext _dbContext;
        private IGenericRepository<TR_COURSE, TrCourseDTO> _genericRepository;
        private readonly GenericReducer<TR_COURSE, TrCourseDTO> _genericReducer;

        public TrCourseRepository(FullDbContext context, GenericUnitOfWork uow)
        {
            _dbContext = context;
            _uow = uow;
            _genericRepository = _uow.GenericRepository<TR_COURSE, TrCourseDTO>();
            _genericReducer = new();
        }

        public async Task<GenericPhaseTwoListResponse<TrCourseDTO>> SinglePhaseQueryList(GenericQueryListDTO<TrCourseDTO> request)
        {
            var joined = from p in _dbContext.TrCourses.AsNoTracking()
                         from s in _dbContext.SysOtherLists.AsNoTracking().Where(s => s.ID == p.TR_TRAIN_FIELD).DefaultIfEmpty()
                         select new TrCourseDTO
                         {
                             Id = p.ID,
                             CourseCode = p.COURSE_CODE,
                             CourseName = p.COURSE_NAME,
                             CourseDate = p.COURSE_DATE,
                             Costs = p.COSTS,
                             Status = p.IS_ACTIVE == true ? "Áp dụng" : "Ngừng áp dụng",
                             Note = p.NOTE,
                             TrTrainField = p.TR_TRAIN_FIELD,
                             TrTrainFieldName = s.NAME,
                         };

            var singlePhaseResult = await _genericReducer.SinglePhaseReduce(joined, request);
            return singlePhaseResult;
        }

        public async Task<FormatedResponse> ReadAll()
        {
            await Task.Run(() => null);
            throw new NotImplementedException();
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

            var joined = await (from l in _dbContext.TrCourses.AsNoTracking().Where(l => l.ID == id)
                          from s in _dbContext.SysOtherLists.AsNoTracking().Where(s => s.ID == l.TR_TRAIN_FIELD).DefaultIfEmpty()
                          from cr in _dbContext.SysUsers.AsNoTracking().Where(c => l.CREATED_BY == null ? false : c.ID == l.CREATED_BY).DefaultIfEmpty()
                          from up in _dbContext.SysUsers.AsNoTracking().Where(u => l.UPDATED_BY == null ? false : u.ID == l.UPDATED_BY).DefaultIfEmpty()
                          select new TrCourseDTO
                          {
                              Id = l.ID,
                              CourseCode = l.COURSE_CODE,
                              CourseName = l.COURSE_NAME,
                              CourseDate = l.COURSE_DATE,
                              Note = l.NOTE,
                              Costs = l.COSTS,
                              TrTrainField = l.TR_TRAIN_FIELD,
                              TrTrainFieldName = s.NAME,
                              ProfessionalTrainning = l.PROFESSIONAL_TRAINNING
                          }).FirstOrDefaultAsync();

            if (joined != null)
            {
                return new FormatedResponse() { InnerBody = joined, StatusCode = EnumStatusCode.StatusCode200 };
            }
            else
            {
                return new FormatedResponse() { MessageCode = CommonMessageCode.ENTITY_NOT_FOUND, ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400 };
            }
        }
        public async Task<FormatedResponse> GetById(string id)
        {
            await Task.Run(() => null);
            throw new NotImplementedException();
        }

        public async Task<FormatedResponse> Create(GenericUnitOfWork _uow, TrCourseDTO dto, string sid)
        {
            string newcode = "";
            if (await _dbContext.TrCourses.CountAsync() == 0)
            {
                newcode = "KDT001";
            }
            else
            {
                string lastestData = _dbContext.TrCourses.OrderByDescending(x => x.COURSE_CODE).First().COURSE_CODE!.ToString();
                newcode = lastestData.Substring(0, 3) + (int.Parse(lastestData.Substring(lastestData.Length - 3)) + 1).ToString("D3");
            }
            try
            {
                dto.CourseCode = newcode;
                dto.IsActive = true;
                var response = await _genericRepository.Create(_uow, dto, sid);
                return response;
            }
            catch (Exception ex)
            {
                return new FormatedResponse()
                {
                    MessageCode = CommonMessageCode.CREATED_FAILD,
                    StatusCode = EnumStatusCode.StatusCode500
                };
            }

        }

        public async Task<FormatedResponse> CreateRange(GenericUnitOfWork _uow, List<TrCourseDTO> dtos, string sid)
        {
            var add = new List<TrCourseDTO>();
            add.AddRange(dtos);
            var response = await _genericRepository.CreateRange(_uow, add, sid);
            return response;
        }

        public async Task<FormatedResponse> Update(GenericUnitOfWork _uow, TrCourseDTO dto, string sid, bool patchMode = true)
        {
            var response = await _genericRepository.Update(_uow, dto, sid, patchMode);
            return response;

        }

        public async Task<FormatedResponse> UpdateRange(GenericUnitOfWork _uow, List<TrCourseDTO> dtos, string sid, bool patchMode = true)
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
            foreach(var item in ids)
            {
                var a = _dbContext.TrPlans.Where(x => x.COURSE_ID == item).Any();
                if (a)
                {
                    return new FormatedResponse()
                    {
                        ErrorType = EnumErrorType.CATCHABLE,
                        MessageCode = "CAN_NOT_DELETE_RECORD_IS_USING",
                        StatusCode = EnumStatusCode.StatusCode400
                    };
                }
            }

            var checkAc =  _dbContext.TrCourses.Where(x => ids.Contains(x.ID)).AsNoTracking().DefaultIfEmpty();
            foreach (var item in checkAc)
            {
                if (item?.IS_ACTIVE == true)
                {
                    return new FormatedResponse()
                    {
                        ErrorType = EnumErrorType.CATCHABLE,
                        MessageCode = CommonMessageCode.DO_NOT_DELETE_DATA_IS_ACTIVE,
                        StatusCode = EnumStatusCode.StatusCode400
                    };
                }
            }
            var response = await _genericRepository.DeleteIds(_uow, ids);
            return response;
        }

        public async Task<FormatedResponse> DeleteIds(GenericUnitOfWork _uow, List<string> ids)
        {
            await Task.Run(() => null);
            throw new NotImplementedException();
        }
        public async Task<FormatedResponse> CreateNewCode()
        {
            string newCode = "";
            if (await _dbContext.TrCourses.CountAsync() == 0)
            {
                newCode = "KDT001";
            }
            else
            {
                string lastestData = _dbContext.TrCourses.OrderByDescending(t => t.COURSE_CODE).First().COURSE_CODE!.ToString();
                newCode = lastestData.Substring(0, 3) + (int.Parse(lastestData.Substring(lastestData.Length - 3)) + 1).ToString("D3");
            }
            return new FormatedResponse() { InnerBody = new { Code = newCode } };
        }

        public virtual async Task<FormatedResponse> ToggleActiveIds(GenericUnitOfWork _uow, List<long> ids, bool valueToBind, string sid)
        {
            var response = await _genericRepository.ToggleActiveIds(_uow, ids, valueToBind, sid);
            return response;
        }

        public async Task<FormatedResponse> GetListCourse()
        {
            try
            {
                var joined = await(from p in _dbContext.TrCourses.AsNoTracking()
                                    where p.IS_ACTIVE == true
                                    orderby p.CREATED_DATE descending
                                    select new 
                                    {
                                        Id = p.ID,
                                        Code = p.COURSE_CODE,
                                        Name = "[" + p.COURSE_CODE + "] " + p.COURSE_NAME
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

