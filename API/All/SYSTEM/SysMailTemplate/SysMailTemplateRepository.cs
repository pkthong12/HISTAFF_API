using CORE.GenericUOW;
using CORE.DTO;
using API.DTO;
using CORE.Enum;
using CORE.StaticConstant;
using API.All.DbContexts;
using CORE.AutoMapper;
using ProfileDAL.ViewModels;

namespace API.Controllers.HuCommend
{
    public class SysMailTemplateRepository : ISysMailTemplateRepository
    {
        private readonly GenericUnitOfWork _uow;
        private readonly FullDbContext _dbContext;
        private IGenericRepository<SYS_MAIL_TEMPLATE, SysMailTemplateDTO> _genericRepository;
        private readonly GenericReducer<SYS_MAIL_TEMPLATE, SysMailTemplateDTO> _genericReducer;

        public SysMailTemplateRepository(FullDbContext context, GenericUnitOfWork uow)
        {
            _dbContext = context;
            _uow = uow;
            _genericRepository = _uow.GenericRepository<SYS_MAIL_TEMPLATE, SysMailTemplateDTO>();
            _genericReducer = new();
        }

        public async Task<GenericPhaseTwoListResponse<SysMailTemplateDTO>> SinglePhaseQueryList(GenericQueryListDTO<SysMailTemplateDTO> request)
        {

            var joined = from p in _dbContext.SysMailTemplates
                         from sys in _dbContext.SysOtherLists.Where(x => x.ID == p.FUNCTIONAL_GROUP_ID).DefaultIfEmpty()
                         select new SysMailTemplateDTO
                         {
                             Id = p.ID,
                             Code = p.CODE,
                             Name = p.NAME,
                             Title = p.TITLE,
                             MailCc = p.MAIL_CC,
                             Remark = p.REMARK,
                             GroupMail = p.GROUP_MAIL,
                             Content = p.CONTENT,
                             SendFixed = p.SEND_FIXED,
                             SendTo = p.SEND_TO,
                             IsMailCc = p.IS_MAIL_CC,
                             FunctionalGroupId = p.FUNCTIONAL_GROUP_ID,
                             FunctionalGroupName = sys.NAME

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
                var list = new List<SYS_MAIL_TEMPLATE>
                    {
                        (SYS_MAIL_TEMPLATE)response
                    };
                var joined = (from l in list
                                  // JOIN OTHER ENTITIES BASED ON THE BUSINESS
                              join t in _uow.Context.Set<SYS_OTHER_LIST>().AsNoTracking().DefaultIfEmpty() on l.FUNCTIONAL_GROUP_ID equals t.ID
                              select new SysMailTemplateDTO
                              {
                                  Id = l.ID,
                                  Code = l.CODE,
                                  Name = l.NAME,
                                  IsMailCc = l.IS_MAIL_CC,
                                  Title = l.TITLE,
                                  MailCc = l.MAIL_CC,
                                  FunctionalGroupId = l.FUNCTIONAL_GROUP_ID,
                                  Remark = l.REMARK,
                                  Content = l.CONTENT,  
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

        public async Task<FormatedResponse> Create(GenericUnitOfWork _uow, SysMailTemplateDTO dto, string sid)
        {
            var response = await _genericRepository.Create(_uow, dto, sid);
            return response;
        }

        public async Task<FormatedResponse> CreateRange(GenericUnitOfWork _uow, List<SysMailTemplateDTO> dtos, string sid)
        {
            var add = new List<SysMailTemplateDTO>();
            add.AddRange(dtos);
            var response = await _genericRepository.CreateRange(_uow, add, sid);
            return response;
        }

        public async Task<FormatedResponse> Update(GenericUnitOfWork _uow, SysMailTemplateDTO dto, string sid, bool patchMode = true)
        {
            var response = await _genericRepository.Update(_uow, dto, sid, patchMode);
            return response;
        }

        public async Task<FormatedResponse> UpdateRange(GenericUnitOfWork _uow, List<SysMailTemplateDTO> dtos, string sid, bool patchMode = true)
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

