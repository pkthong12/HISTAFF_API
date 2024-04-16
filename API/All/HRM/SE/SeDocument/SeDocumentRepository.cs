using CORE.GenericUOW;
using CORE.DTO;
using API.DTO;
using CORE.Enum;
using CORE.StaticConstant;
using API.All.DbContexts;
using Common.Extensions;
using CoreDAL;
using ProfileDAL.ViewModels;

namespace API.Controllers.SeDocument
{
    public class SeDocumentRepository : ISeDocumentRepository
    {
        private readonly GenericUnitOfWork _uow;
        private readonly FullDbContext _dbContext;
        private IGenericRepository<SE_DOCUMENT, SeDocumentDTO> _genericRepository;
        private readonly GenericReducer<SE_DOCUMENT, SeDocumentDTO> _genericReducer;

        public SeDocumentRepository(FullDbContext context, GenericUnitOfWork uow)
        {
            _dbContext = context;
            _uow = uow;
            _genericRepository = _uow.GenericRepository<SE_DOCUMENT, SeDocumentDTO>();
            _genericReducer = new();
        }

        public async Task<GenericPhaseTwoListResponse<SeDocumentDTO>> SinglePhaseQueryList(GenericQueryListDTO<SeDocumentDTO> request)
        {
            var joined = from p in _dbContext.SeDocuments
                         from o in _dbContext.SysOtherLists.Where(x=>x.ID == p.DOCUMENT_TYPE).DefaultIfEmpty()
                         orderby p.CREATED_DATE descending
                         // JOIN OTHER ENTITIES BASED ON THE BUSINESS
                         select new SeDocumentDTO
                         {
                             Id = p.ID,
                             Code = p.CODE,
                             Name = p.NAME,
                             DocumentType = p.DOCUMENT_TYPE,
                             DocumentTypeName = o.NAME,
                             IsObligatory = p.IS_OBLIGATORY,
                             IsPermissveUpload = p.IS_PERMISSVE_UPLOAD,
                             Status = p.IS_ACTIVE == true ? "Áp dụng" : "Ngừng áp dụng",
                             Note = p.NOTE,
                             CreatedDate = p.CREATED_DATE
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
                var list = new List<SE_DOCUMENT>
                    {
                        (SE_DOCUMENT)response
                    };
                var joined = (from l in list
                              // JOIN OTHER ENTITIES BASED ON THE BUSINESS
                              select new SeDocumentDTO
                              {
                                  Id = l.ID,
                                  Code = l.CODE,
                                  Name = l.NAME,
                                  DocumentType = l.DOCUMENT_TYPE,
                                  IsObligatory = l.IS_OBLIGATORY,
                                  IsPermissveUpload = l.IS_PERMISSVE_UPLOAD,
                                  Note = l.NOTE
                              }).FirstOrDefault();

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

        public async Task<FormatedResponse> Create(GenericUnitOfWork _uow, SeDocumentDTO dto, string sid)
        {
            var response = await _genericRepository.Create(_uow, dto, sid);
            return response;
        }

        public async Task<FormatedResponse> CreateRange(GenericUnitOfWork _uow, List<SeDocumentDTO> dtos, string sid)
        {
            var add = new List<SeDocumentDTO>();
            add.AddRange(dtos);
            var response = await _genericRepository.CreateRange(_uow, add, sid);
            return response;
        }

        public async Task<FormatedResponse> Update(GenericUnitOfWork _uow, SeDocumentDTO dto, string sid, bool patchMode = true)
        {
            var response = await _genericRepository.Update(_uow, dto, sid, patchMode);
            return response;
        }

        public async Task<FormatedResponse> UpdateRange(GenericUnitOfWork _uow, List<SeDocumentDTO> dtos, string sid, bool patchMode = true)
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
            var check = _dbContext.SeDocuments.Where(x => ids.Contains(x.ID)).DefaultIfEmpty();
            foreach (var item in check)
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

        public virtual async Task<FormatedResponse> ToggleActiveIds(GenericUnitOfWork _uow, List<long> ids, bool valueToBind, string sid)
        {
            var response = await _genericRepository.ToggleActiveIds(_uow, ids, valueToBind, sid);
            return response;
        }

        public async Task<FormatedResponse> CreateCode()
        {
            string newCode = "";
            if (await _dbContext.SeDocuments.CountAsync() == 0)
            {
                newCode = "MTL001";
            }
            else
            {
                string lastestData = _dbContext.SeDocuments.OrderByDescending(t => t.CODE).First().CODE!.ToString();

                newCode = lastestData.Substring(0, 3) + (int.Parse(lastestData.Substring(lastestData.Length - 3)) + 1).ToString("D3");
            }

            return new FormatedResponse() { InnerBody = new { Code = newCode } };
        }

        public async Task<FormatedResponse> GetDucumentType()
        {
            var query = await (from p in _dbContext.SysOtherLists.Where(x => x.TYPE_ID == 111 && x.IS_ACTIVE == true).DefaultIfEmpty()
                                select new 
                                { 
                                    Id = p.ID, 
                                    Name = p.NAME 
                                }).ToListAsync();

            return new FormatedResponse() { InnerBody = query };
        }
    }
}

