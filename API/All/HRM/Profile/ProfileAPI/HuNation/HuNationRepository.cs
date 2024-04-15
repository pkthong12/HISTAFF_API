using CORE.GenericUOW;
using CORE.DTO;
using API.DTO;
using CORE.Enum;
using CORE.StaticConstant;
using API.All.DbContexts;
using CORE.AutoMapper;
using Hangfire.Storage;
using Azure;
using System.Linq;

namespace API.Controllers.HuNation
{
    public class HuNationRepository : IHuNationRepository
    {
        private readonly GenericUnitOfWork _uow;
        private readonly FullDbContext _dbContext;
        private IGenericRepository<HU_NATION, HuNationDTO> _genericRepository;
        private readonly GenericReducer<HU_NATION, HuNationDTO> _genericReducer;

        public HuNationRepository(FullDbContext context, GenericUnitOfWork uow)
        {
            _dbContext = context;
            _uow = uow;
            _genericRepository = _uow.GenericRepository<HU_NATION, HuNationDTO>();
            _genericReducer = new();
        }

        public async Task<GenericPhaseTwoListResponse<HuNationDTO>> SinglePhaseQueryList(GenericQueryListDTO<HuNationDTO> request)
        {
            var joined = from p in _dbContext.HuNations.AsNoTracking()
                         select new HuNationDTO
                         {
                             Id = p.ID,
                             Name = p.NAME,
                             Code = p.CODE,
                             IsActive = p.IS_ACTIVE,
                             CreatedBy = p.CREATED_BY,
                             CreatedDate = p.CREATED_DATE,
                             UpdatedBy = p.UPDATED_BY,
                             UpdatedDate = p.UPDATED_DATE,
                             Note = p.NOTE,
                             IsActiveStr = p.IS_ACTIVE == true ? "Áp dụng" : "Ngừng áp dụng"

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
            try
            {
                var res = await _genericRepository.GetById(id);
                if (res.InnerBody != null)
                {
                    var response = res.InnerBody;
                    var list = new List<HU_NATION>
                    {
                        (HU_NATION)response
                    };
                    var joined = (from p in list
                                  select new HuNationDTO
                                  {
                                      Id = p.ID,
                                      Name = p.NAME,
                                      Code = p.CODE,
                                      IsActive = p.IS_ACTIVE,
                                      CreatedBy = p.CREATED_BY,
                                      CreatedDate = p.CREATED_DATE,
                                      UpdatedBy = p.UPDATED_BY,
                                      UpdatedDate = p.UPDATED_DATE,
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

        public async Task<FormatedResponse> Create(GenericUnitOfWork _uow, HuNationDTO dto, string sid)
        {


            var na = _dbContext.HuNations.Where(x => x.NAME.Trim().ToLower() == dto.Name.Trim().ToLower()).FirstOrDefault();
            if (na != null)
            {
                return new() { StatusCode = EnumStatusCode.StatusCode400, ErrorType = EnumErrorType.CATCHABLE, MessageCode = CommonMessageCode.DUBLICATE_VALUE };
            }
            string newCode = "";
            if (await _dbContext.HuNations.CountAsync() == 0)
            {
                newCode = "QG001";
            }
            else
            {
                string lastestData = _dbContext.HuNations.OrderByDescending(t => t.CODE).First().CODE!.ToString();
                newCode = lastestData.Substring(0, 2) + (int.Parse(lastestData.Substring(lastestData.Length - 3)) + 1).ToString("D3");
            }
            dto.Code = newCode;
            dto.IsActive = true;
            var response = await _genericRepository.Create(_uow, dto, sid);
            return response;
        }

        public async Task<FormatedResponse> CreateRange(GenericUnitOfWork _uow, List<HuNationDTO> dtos, string sid)
        {
            var add = new List<HuNationDTO>();
            add.AddRange(dtos);
            var response = await _genericRepository.CreateRange(_uow, add, sid);
            return response;
        }

        public async Task<FormatedResponse> Update(GenericUnitOfWork _uow, HuNationDTO dto, string sid, bool patchMode = true)
        {
            var na = _dbContext.HuNations.Where(x => x.NAME.Trim().ToLower() == dto.Name.Trim().ToLower() && x.ID != dto.Id).FirstOrDefault();
            if (na != null)
            {
                return new() { StatusCode = EnumStatusCode.StatusCode400, ErrorType = EnumErrorType.CATCHABLE, MessageCode = CommonMessageCode.DUBLICATE_VALUE };

            }
            var response = await _genericRepository.Update(_uow, dto, sid, patchMode);
            return response;
        }

        public async Task<FormatedResponse> UpdateRange(GenericUnitOfWork _uow, List<HuNationDTO> dtos, string sid, bool patchMode = true)
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

        public async Task<FormatedResponse> CreateNewCode()
        {
            string newCode = "";
            if (await _dbContext.HuNations.CountAsync() == 0)
            {
                newCode = "QG001";
            }
            else
            {
                string lastestData = _dbContext.HuNations.OrderByDescending(t => t.CODE).First().CODE!.ToString();

                newCode = lastestData.Substring(0, 2) + (int.Parse(lastestData.Substring(lastestData.Length - 3)) + 1).ToString("D3");
            }

            return new FormatedResponse() { InnerBody = new { Code = newCode } };

        }

        public async Task<FormatedResponse> CheckActive(List<long> ids)
        {
            var checkActive = (await _dbContext.HuNations.Where(x => ids.Contains(x.ID) && x.IS_ACTIVE == true).CountAsync() > 0) ? true : false;
            var checkDistint = (await _dbContext.HuProvinces.Where(x => ids.Contains((long)x.NATION_ID)).CountAsync() > 0) ? true : false;
            var checkHuEmpCv = (await _dbContext.HuEmployeeCvs.Where(x =>ids.Contains(x.NATIONALITY_ID != null ? (long)x.NATIONALITY_ID : 0)).CountAsync() > 0) ? true : false;
            var checkFamily = (await _dbContext.HuFamilys.Where(x => ids.Contains(x.NATIONALITY != null ? (long)x.NATIONALITY : 0)).CountAsync() > 0) ? true : false;
            if (checkActive)
            {
                return new FormatedResponse()
                {
                    StatusCode = EnumStatusCode.StatusCode400,
                    MessageCode = CommonMessageCode.DO_NOT_DELETE_DATA_IS_ACTIVE,
                    ErrorType = EnumErrorType.CATCHABLE,
                };
            }
            if (checkDistint || checkHuEmpCv || checkFamily)
            {
                return new FormatedResponse()
                {
                    StatusCode = EnumStatusCode.StatusCode400,
                    MessageCode = CommonMessageCode.DATA_HAS_USED,
                    ErrorType = EnumErrorType.CATCHABLE,
                };
            }
            else
            {
                return new FormatedResponse()
                {
                    StatusCode = EnumStatusCode.StatusCode200,
                };
            }
        }

        public virtual async Task<FormatedResponse> ToggleActiveIds(GenericUnitOfWork _uow, List<long> ids, bool valueToBind, string sid)
        {
            var response = await _genericRepository.ToggleActiveIds(_uow, ids, valueToBind, sid);
            return response;
        }

    }
}

