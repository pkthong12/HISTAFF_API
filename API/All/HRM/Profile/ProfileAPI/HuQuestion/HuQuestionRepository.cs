using CORE.GenericUOW;
using CORE.DTO;
using CORE.AutoMapper;
using API.DTO;
using CORE.Enum;
using CORE.StaticConstant;
using API.All.DbContexts;
using API.Entities;

namespace API.Controllers.HuQuestion
{
    public class HuQuestionRepository : IHuQuestionRepository
    {
        private readonly GenericUnitOfWork _uow;
        private readonly FullDbContext _dbContext;
        private IGenericRepository<HU_QUESTION, HuQuestionDTO> _genericRepository; // CHA
        private IGenericRepository<HU_ANSWER, HuAnswerDTO> _genericChilrenRepository; // CON
        private readonly GenericReducer<HU_QUESTION, HuQuestionDTO> _genericReducer;

        public HuQuestionRepository(FullDbContext context, GenericUnitOfWork uow)
        {
            _dbContext = context;
            _uow = uow;
            _genericRepository = _uow.GenericRepository<HU_QUESTION, HuQuestionDTO>();
            _genericChilrenRepository = _uow.GenericRepository<HU_ANSWER, HuAnswerDTO>();
            _genericReducer = new();
        }

        public async Task<GenericPhaseTwoListResponse<HuQuestionDTO>> SinglePhaseQueryList(GenericQueryListDTO<HuQuestionDTO> request)
        {
            var joined = from p in _dbContext.HuQuestions.AsNoTracking()
                             // JOIN OTHER ENTITIES BASED ON THE BUSINESS
                         from c in _dbContext.SysUsers.AsNoTracking().Where(c => p.CREATED_BY == null ? false : c.ID == p.CREATED_BY).DefaultIfEmpty()
                         from u in _dbContext.SysUsers.AsNoTracking().Where(u => p.UPDATED_BY == null ? false : u.ID == p.UPDATED_BY).DefaultIfEmpty()
                         select new HuQuestionDTO
                         {
                             Id = p.ID,
                             Name = p.NAME,
                             IsMultiple = p.IS_MULTIPLE,
                             IsAddAnswer = p.IS_ADD_ANSWER,
                             IsActive = p.IS_ACTIVE,
                             CreatedDate = p.CREATED_DATE,
                             CreatedByUsername = c.USERNAME,
                             UpdatedDate = p.UPDATED_DATE,
                             UpdatedByUsername = u.USERNAME
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
            var res = await _genericRepository.GetById(id);
            if (res.InnerBody != null)
            {
                var response = res.InnerBody;
                var list = new List<HU_QUESTION>
                    {
                        (HU_QUESTION)response
                    };
                var joined = (from l in list
                                  // JOIN OTHER ENTITIES BASED ON THE BUSINESS
                              select new HuQuestionDTO
                              {
                                  Id = l.ID,
                                  Name = l.NAME,
                                  Expire = l.EXPIRE,
                                  IsMultiple = l.IS_MULTIPLE,
                                  IsAddAnswer = l.IS_ADD_ANSWER,
                                  IsActive = l.IS_ACTIVE,
                                  Answers = new List<HuAnswerDTO>()
                              }).FirstOrDefault();

                if (joined != null)
                {

                    // Get chidren (List<HU_ANSWER>)
                    var chidrenResponse = await _genericChilrenRepository.ReadAllByKey("QUESTION_ID", id);

                    if (res.StatusCode == EnumStatusCode.StatusCode200)
                    {
                        if (chidrenResponse != null)
                        {
                            if (chidrenResponse.InnerBody != null)
                            {
                                var entityResult = (List<HU_ANSWER>)chidrenResponse.InnerBody;
                                joined.Answers = CoreMapper<HuAnswerDTO, HU_ANSWER>.ListEntityToListDTO(entityResult);
                                return new FormatedResponse() { InnerBody = joined };
                            }
                            else
                            {
                                return new() { ErrorType = EnumErrorType.CATCHABLE, MessageCode = CommonMessageCode.GET_LIST_BY_KEY_INNER_BODY_WAS_NULL, StatusCode = EnumStatusCode.StatusCode400 };
                            }
                        } else
                        {
                            return new FormatedResponse() { InnerBody = joined };
                        }
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

        public async Task<FormatedResponse> GetById(string id)
        {
            await Task.Run(() => null);
            throw new NotImplementedException();
        }

        public async Task<FormatedResponse> Create(GenericUnitOfWork _uow, HuQuestionDTO dto, string sid) // sid is the string ID of the authenticated user
        {
            // IT MUST FOLLOW THE PRINCIPPLE OF "ALL SUCCESS OR ALL FAIL"
            _uow.CreateTransaction();

            try
            {

                var res = await _genericRepository.Create(_uow, dto, sid);

                if (res.StatusCode == EnumStatusCode.StatusCode200)
                {

                    if (res.InnerBody != null)
                    {

                        var parentResponse = (HU_QUESTION)res.InnerBody;
                        
                        var parentResponseDto = CoreMapper<HuQuestionDTO, HU_QUESTION>.EntityToDto(parentResponse, dto);

                        if (parentResponseDto != null)
                        {

                            if (dto.Answers != null)
                            {
                                // CREATE CHILDREN
                                dto.Answers.ForEach(child =>
                                {
                                    child.Id = null; // Allow EF core to insert Id automacically
                                    child.QuestionId = parentResponseDto.Id;
                                });

                                // TRY TO CREATE RANGE OF CHILDREN
                                var createRangeForChildrenResponse = await _genericChilrenRepository.CreateRange(_uow, dto.Answers, sid);

                                if (createRangeForChildrenResponse.StatusCode == EnumStatusCode.StatusCode200)
                                {
                                    if (createRangeForChildrenResponse.InnerBody != null)
                                    {

                                        List<HU_ANSWER> HU_ANSWERs = (List<HU_ANSWER>)createRangeForChildrenResponse.InnerBody;
                                        var huAnswerDTOs = CoreMapper<HuAnswerDTO, HU_ANSWER>.ListEntityToListDTO(HU_ANSWERs);

                                        parentResponseDto.Answers = huAnswerDTOs;

                                        _uow.Commit();

                                        return new FormatedResponse() { InnerBody = parentResponseDto };
                                    }
                                    else
                                    {
                                        _uow.Commit();

                                        parentResponseDto.Answers = new List<HuAnswerDTO>();
                                        return new FormatedResponse() { InnerBody = parentResponseDto };
                                    }
                                }
                                else
                                {
                                    _uow.Rollback();

                                    return new() { ErrorType = EnumErrorType.CATCHABLE, MessageCode = CommonMessageCode.CREATE_RANGE_FOR_CHIDREN_IS_NOT_OF_STATUS_CODE_200, StatusCode = EnumStatusCode.StatusCode400 };
                                }
                            }
                            else
                            {
                                _uow.Rollback();

                                return new FormatedResponse() { InnerBody = parentResponseDto };
                            }

                        }
                        else
                        {
                            _uow.Rollback();

                            return new() { ErrorType = EnumErrorType.CATCHABLE, MessageCode = CommonMessageCode.CREATE_PARENT_RESPONSE_WAS_NULL, StatusCode = EnumStatusCode.StatusCode400 };
                        }
                    }
                    else
                    {
                        _uow.Rollback();

                        return new() { ErrorType = EnumErrorType.CATCHABLE, MessageCode = CommonMessageCode.CREATE_PARENT_RESPONSE_INNER_BODY_WAS_NULL, StatusCode = EnumStatusCode.StatusCode400 };
                    }
                } else
                {
                    return new() { ErrorType = EnumErrorType.CATCHABLE, MessageCode = CommonMessageCode.CREATE_PARENT_RESPONSE_IS_NOT_OF_STATUS_CODE_200, StatusCode = EnumStatusCode.StatusCode400 };
                }
            } catch (Exception ex)
            {
                _uow.Rollback();

                return new() { ErrorType = EnumErrorType.UNCATCHABLE, MessageCode = ex.Message, StatusCode = EnumStatusCode.StatusCode500 };
            }
        }

        public async Task<FormatedResponse> CreateRange(GenericUnitOfWork _uow, List<HuQuestionDTO> dtos, string sid)
        {
            var add = new List<HuQuestionDTO>();
            add.AddRange(dtos);
            var response = await _genericRepository.CreateRange(_uow, add, sid);
            return response;
        }

        public async Task<FormatedResponse> Update(GenericUnitOfWork _uow, HuQuestionDTO dto, string sid, bool patchMode = true)
        {
            // IT MUST FOLLOW THE PRINCIPPLE OF "ALL SUCCESS OR ALL FAIL"
            _uow.CreateTransaction();

            try
            {

                var res = await _genericRepository.Update(_uow, dto, sid);

                if (res.StatusCode == EnumStatusCode.StatusCode200)
                {

                    if (res.InnerBody != null)
                    {
                        var parentResponse = (HU_QUESTION)res.InnerBody;

                        var parentResponseDto = CoreMapper<HuQuestionDTO, HU_QUESTION>.EntityToDto(parentResponse, dto);

                        if (parentResponseDto != null)
                        {

                            if (dto.Answers != null)
                            {

                                // DELETE OLD CHIDREN OF THE PARENT
                                var oldChidrenResponse = await _genericChilrenRepository.ReadAllByKey("QUESTION_ID", parentResponse.ID);

                                if (oldChidrenResponse.StatusCode == EnumStatusCode.StatusCode200)
                                {

                                    if (oldChidrenResponse.InnerBody != null)
                                    {

                                        List<HU_ANSWER> HU_ANSWERs_1 = (List<HU_ANSWER>)oldChidrenResponse.InnerBody;
                                        var huAnswerDTOs_1 = CoreMapper<HuAnswerDTO, HU_ANSWER>.ListEntityToListDTO(HU_ANSWERs_1);
                                        List<long> Ids = new();

                                        if (huAnswerDTOs_1 != null)
                                        {
                                            huAnswerDTOs_1.ForEach(answer =>
                                            {
                                                if (answer != null)
                                                {
                                                    if (answer.Id != null)
                                                    {
                                                        Ids.Add((long)answer.Id);
                                                    }
                                                }
                                            });

                                            // DELETE OLD CHIDREN OF THE PARENT
                                            var deleteOldListResponse = await _genericChilrenRepository.DeleteIds(_uow, Ids);

                                            if (deleteOldListResponse.StatusCode != EnumStatusCode.StatusCode200)
                                            {
                                                return new() { ErrorType = EnumErrorType.CATCHABLE, MessageCode = CommonMessageCode.DELETE_IDS_FOR_CHIDREN_IS_NOT_OF_STATUS_CODE_200, StatusCode = EnumStatusCode.StatusCode400 };
                                            }


                                        } else
                                        {
                                            _uow.Rollback();

                                            return new() { ErrorType = EnumErrorType.CATCHABLE, MessageCode = CommonMessageCode.ERROR_WHILE_CONVERTING_ENTITY_LIST_TO_DTO_LIST, StatusCode = EnumStatusCode.StatusCode400 };

                                        }

                                        // CREATE CHILDREN
                                        dto.Answers.ForEach(child =>
                                        {
                                            child.Id = null; // Allow EF core to insert Id automacically
                                            child.QuestionId = parentResponseDto.Id;
                                        });

                                        // TRY TO CREATE RANGE OF CHILDREN
                                        var createRangeForChildrenResponse = await _genericChilrenRepository.CreateRange(_uow, dto.Answers, sid);

                                        if (createRangeForChildrenResponse.StatusCode == EnumStatusCode.StatusCode200)
                                        {
                                            if (createRangeForChildrenResponse.InnerBody != null)
                                            {

                                                List<HU_ANSWER> HU_ANSWERs_2 = (List<HU_ANSWER>)createRangeForChildrenResponse.InnerBody;
                                                var huAnswerDTOs_2 = CoreMapper<HuAnswerDTO, HU_ANSWER>.ListEntityToListDTO(HU_ANSWERs_2);

                                                parentResponseDto.Answers = huAnswerDTOs_2;

                                                _uow.Commit();

                                                return new FormatedResponse() { InnerBody = parentResponseDto };
                                            }
                                            else
                                            {
                                                _uow.Commit();

                                                parentResponseDto.Answers = new List<HuAnswerDTO>();
                                                return new FormatedResponse() { InnerBody = parentResponseDto };
                                            }
                                        }
                                        else
                                        {
                                            _uow.Rollback();

                                            return new() { ErrorType = EnumErrorType.CATCHABLE, MessageCode = CommonMessageCode.CREATE_RANGE_FOR_CHIDREN_IS_NOT_OF_STATUS_CODE_200, StatusCode = EnumStatusCode.StatusCode400 };
                                        }
                                    } else
                                    {

                                        _uow.Commit();

                                        parentResponseDto.Answers = new List<HuAnswerDTO>();
                                        return new FormatedResponse() { InnerBody = parentResponseDto };

                                    }
                                } else
                                {
                                    _uow.Rollback();

                                    return new() { ErrorType = EnumErrorType.CATCHABLE, MessageCode = CommonMessageCode.READ_EXISTING_CHIDREN_RESPONSE_IS_NOT_OF_STATUS_CODE_200, StatusCode = EnumStatusCode.StatusCode400 };

                                }
                            }
                            else
                            {
                                _uow.Rollback();

                                return new FormatedResponse() { InnerBody = parentResponseDto };
                            }

                        }
                        else
                        {
                            _uow.Rollback();

                            return new() { ErrorType = EnumErrorType.CATCHABLE, MessageCode = CommonMessageCode.CREATE_PARENT_RESPONSE_WAS_NULL, StatusCode = EnumStatusCode.StatusCode400 };
                        }
                    }
                    else
                    {
                        _uow.Rollback();

                        return new() { ErrorType = EnumErrorType.CATCHABLE, MessageCode = CommonMessageCode.CREATE_PARENT_RESPONSE_INNER_BODY_WAS_NULL, StatusCode = EnumStatusCode.StatusCode400 };
                    }
                } else
                {
                    _uow.Rollback();

                    return new() { ErrorType = EnumErrorType.CATCHABLE, MessageCode = CommonMessageCode.UPDATE_PARENT_RESPONSE_IS_NOT_OF_STATUS_CODE_200, StatusCode = EnumStatusCode.StatusCode400 };
                }
            }
            catch (Exception ex)
            {
                _uow.Rollback();

                return new() { ErrorType = EnumErrorType.UNCATCHABLE, MessageCode = ex.Message, StatusCode = EnumStatusCode.StatusCode500 };
            }
        }

        public async Task<FormatedResponse> UpdateRange(GenericUnitOfWork _uow, List<HuQuestionDTO> dtos, string sid, bool patchMode = true)
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

