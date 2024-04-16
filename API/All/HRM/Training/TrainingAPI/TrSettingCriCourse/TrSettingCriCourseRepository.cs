using CORE.GenericUOW;
using CORE.DTO;
using API.DTO;
using CORE.Enum;
using CORE.StaticConstant;
using API.All.DbContexts;
using CORE.AutoMapper;
using Common.Repositories;

namespace API.Controllers.TrSettingCriCourse
{
    public class TrSettingCriCourseRepository : RepositoryBase<TR_SETTING_CRI_COURSE>, ITrSettingCriCourseRepository
    {
        private readonly GenericUnitOfWork _uow;
        private readonly FullDbContext _dbContext;
        private IGenericRepository<TR_SETTING_CRI_COURSE, TrSettingCriCourseDTO> _genericRepository;
        private readonly GenericReducer<TR_SETTING_CRI_COURSE, TrSettingCriCourseDTO> _genericReducer;

        public TrSettingCriCourseRepository(FullDbContext context, GenericUnitOfWork uow) : base(context)
        {
            _dbContext = context;
            _uow = uow;
            _genericRepository = _uow.GenericRepository<TR_SETTING_CRI_COURSE, TrSettingCriCourseDTO>();
            _genericReducer = new();
        }

        public async Task<GenericPhaseTwoListResponse<TrSettingCriCourseDTO>> SinglePhaseQueryList(GenericQueryListDTO<TrSettingCriCourseDTO> request)
        {
            var joined = from p in _dbContext.TrSettingCriCourses.AsNoTracking()
                         from c in _dbContext.TrCourses.AsNoTracking().Where(c => c.ID == p.TR_COURSE_ID).DefaultIfEmpty()
                         select new TrSettingCriCourseDTO
                         {
                             Id = p.ID,
                             TrCourseId = p.TR_COURSE_ID,
                             TrCourseName = c.COURSE_NAME,
                             ScalePoint = p.SCALE_POINT,
                             EffectFrom = p.EFFECT_FROM,
                             EffectTo = p.EFFECT_TO,
                             Remark = p.REMARK,
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
            
            var joined = await (from p in _dbContext.TrSettingCriCourses.AsNoTracking().Where(p => p.ID == id)
                                from c in _dbContext.TrCourses.AsNoTracking().Where(c => c.ID == p.TR_COURSE_ID).DefaultIfEmpty()
                                select new TrSettingCriCourseDTO
                                {
                                    Id = p.ID,
                                    TrCourseId = p.TR_COURSE_ID,
                                    TrCourseName = c.COURSE_NAME,
                                    ScalePoint = p.SCALE_POINT,
                                    EffectFrom = p.EFFECT_FROM,
                                    EffectTo = p.EFFECT_TO,
                                    Remark = p.REMARK,
                                    TrSettingCriDetails = (from i in _dbContext.TrSettingCriDetails
                                                           from n in _dbContext.TrSettingCriCourses.Where(x => x.ID == i.COURSE_ID).DefaultIfEmpty()
                                                           from t in _dbContext.TrCriterias.Where(x => x.ID == i.CRITERIA_ID).DefaultIfEmpty()
                                                           where n.ID == id
                                                          select new TrSettingCriDetailDTO
                                                          {
                                                              Id = i.ID,
                                                              CriteriaId = i.CRITERIA_ID,
                                                              CriteriaName = t.NAME,
                                                              Ratio = i.RATIO,
                                                              PointMax = i.POINT_MAX
                                                          }).ToList(),
                                }).FirstOrDefaultAsync();
            if(joined != null)
            {
                return new FormatedResponse() { InnerBody = joined };
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

        public async Task<FormatedResponse> Create(GenericUnitOfWork _uow, TrSettingCriCourseDTO dto, string sid)
        {
            try
            {
                await _dbContext.Database.BeginTransactionAsync();

                var trSettingCriCourse = Map(dto, new TR_SETTING_CRI_COURSE());
                trSettingCriCourse.CREATED_BY = sid;
                trSettingCriCourse.CREATED_DATE = DateTime.UtcNow;
                await _dbContext.TrSettingCriCourses.AddAsync(trSettingCriCourse);
                await _dbContext.SaveChangesAsync();

                if (dto.TrSettingCriDetails != null && dto.TrSettingCriDetails.Count > 0)
                {
                    var d = _dbContext.TrSettingCriDetails.Where(x => x.COURSE_ID == dto.Id).ToList();
                    if (d != null)
                    {
                        _dbContext.TrSettingCriDetails.RemoveRange(d);
                    }
                    foreach (var item in dto.TrSettingCriDetails)
                    {
                        item.Id = null;
                        var trSettingCriDetail = Map(item, new TR_SETTING_CRI_DETAIL());
                        trSettingCriDetail.COURSE_ID = trSettingCriCourse.ID;


                        trSettingCriDetail.CREATED_BY = sid;
                        trSettingCriDetail.CREATED_DATE = DateTime.UtcNow;
                        await _dbContext.TrSettingCriDetails.AddAsync(trSettingCriDetail);
                        await _dbContext.SaveChangesAsync();
                    }
                }

                _dbContext.Database.CommitTransaction();
                return new FormatedResponse() { InnerBody = trSettingCriCourse };
            }
            catch (Exception ex)
            {
                _dbContext.Database.RollbackTransaction();
                return new() { ErrorType = EnumErrorType.UNCATCHABLE, StatusCode = EnumStatusCode.StatusCode500, MessageCode = ex.Message };
            }
        }

        public async Task<FormatedResponse> CreateRange(GenericUnitOfWork _uow, List<TrSettingCriCourseDTO> dtos, string sid)
        {
            var add = new List<TrSettingCriCourseDTO>();
            add.AddRange(dtos);
            var response = await _genericRepository.CreateRange(_uow, add, sid);
            return response;
        }

        public async Task<FormatedResponse> Update(GenericUnitOfWork _uow, TrSettingCriCourseDTO dto, string sid, bool patchMode = true)
        {
            try
            {
                await _dbContext.Database.BeginTransactionAsync();

                var trSettingCriCourse = Map(dto, new TR_SETTING_CRI_COURSE());
                trSettingCriCourse.UPDATED_BY = sid;
                trSettingCriCourse.UPDATED_DATE = DateTime.UtcNow;
                _dbContext.TrSettingCriCourses.Update(trSettingCriCourse);
                await _dbContext.SaveChangesAsync();

                if (dto.TrSettingCriDetails != null && dto.TrSettingCriDetails.Count > 0)
                {
                    var d = _dbContext.TrSettingCriDetails.Where(x => x.COURSE_ID == dto.Id).ToList();
                    if (d != null)
                    {
                        _dbContext.TrSettingCriDetails.RemoveRange(d);
                    }
                    foreach (var item in dto.TrSettingCriDetails)
                    {
                        item.Id = null;
                        var trSettingCriDetail = Map(item, new TR_SETTING_CRI_DETAIL());
                        trSettingCriDetail.COURSE_ID = trSettingCriCourse.ID;

                        trSettingCriDetail.CREATED_BY = sid;
                        trSettingCriDetail.CREATED_DATE = DateTime.UtcNow;
                        _dbContext.TrSettingCriDetails.Update(trSettingCriDetail);
                        await _dbContext.SaveChangesAsync();
                    }
                }

                _dbContext.Database.CommitTransaction();
                return new FormatedResponse() { InnerBody = trSettingCriCourse };
            }
            catch (Exception ex)
            {
                _dbContext.Database.RollbackTransaction();
                return new() { ErrorType = EnumErrorType.UNCATCHABLE, StatusCode = EnumStatusCode.StatusCode500, MessageCode = ex.Message };
            }
        }

        public async Task<FormatedResponse> UpdateRange(GenericUnitOfWork _uow, List<TrSettingCriCourseDTO> dtos, string sid, bool patchMode = true)
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

        public Task<FormatedResponse> ToggleActiveIds(GenericUnitOfWork _uow, List<long> ids, bool valueToBind, string sid)
        {
            throw new NotImplementedException();
        }
    }
}

