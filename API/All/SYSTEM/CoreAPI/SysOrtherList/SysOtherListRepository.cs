using CORE.GenericUOW;
using CORE.DTO;
using API.DTO;
using CORE.Enum;
using CORE.StaticConstant;
using API.All.DbContexts;
using CORE.AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Common.Extensions;
using System;
using Aspose.Cells;
using DocumentFormat.OpenXml.Office.CustomUI;

namespace API.Controllers.SysOtherList
{
    public class SysOtherListRepository : ISysOtherListRepository
    {
        private readonly GenericUnitOfWork _uow;
        private readonly FullDbContext _dbContext;
        private IGenericRepository<SYS_OTHER_LIST, SysOtherListDTO> _genericRepository;
        private readonly GenericReducer<SYS_OTHER_LIST, SysOtherListDTO> _genericReducer;

        public SysOtherListRepository(FullDbContext context, GenericUnitOfWork uow)
        {
            _dbContext = context;
            _uow = uow;
            _genericRepository = _uow.GenericRepository<SYS_OTHER_LIST, SysOtherListDTO>();
            _genericReducer = new();
        }

        public async Task<GenericPhaseTwoListResponse<SysOtherListDTO>> SinglePhaseQueryList(GenericQueryListDTO<SysOtherListDTO> request)
        {

            var input = from p in _dbContext.SysOtherLists.AsNoTracking().DefaultIfEmpty()
                        from t in _dbContext.SysOtherListTypes.AsNoTracking().DefaultIfEmpty()
                        where p.TYPE_ID == t.ID
                        select new SysOtherListDTO()
                        {
                            Id = p.ID,
                            Code = p.CODE,
                            Name = p.NAME,
                            TypeId = t.ID,
                            TypeName = t.NAME,
                            Status = (p.IS_ACTIVE.HasValue && p.IS_ACTIVE.Value == true ) ? "Áp dụng" : "Ngừng áp dụng",
                            Note = p.NOTE,
                            Orders = p.ORDERS,
                            ExpirationDate = p.EXPIRATION_DATE,
                            EffectiveDate = p.EFFECTIVE_DATE,
                        };

            var response = await _genericReducer.SinglePhaseReduce(input, request);
            return response;
        }

        public async Task<FormatedResponse> ReadAll()
        {
            await Task.Run(() => null);
            throw new NotImplementedException();
        }

        public async Task<FormatedResponse> ReadAllByKey(string key, long value)
        {
            var resposne = await _genericRepository.ReadAllByKey(key, value);
            return resposne;
        }

        public async Task<FormatedResponse> ReadAllByKey(string key, string value)
        {
            var resposne = await _genericRepository.ReadAllByKey(key, value);
            return resposne;
        }

        public async Task<FormatedResponse> GetById(long id)
        {
            var res = await _genericRepository.GetById(id);
            if (res.InnerBody != null)
            {
                var response = res.InnerBody;
                var list = new List<SYS_OTHER_LIST>
                    {
                        (SYS_OTHER_LIST)response
                    };
                var joined = (from l in list
                              // JOIN OTHER ENTITIES BASED ON THE BUSINESS
                              join t in _uow.Context.Set<SYS_OTHER_LIST_TYPE>().AsNoTracking().DefaultIfEmpty() on l.TYPE_ID equals t.ID
                              select new SysOtherListDTO
                              {
                                  Id = l.ID,
                                  Code = l.CODE,
                                  Name = l.NAME,
                                  TypeId = l.TYPE_ID,
                                  TypeName = t.NAME,
                                  Note = l.NOTE,
                                  EffectiveDate = l.EFFECTIVE_DATE,
                                  ExpirationDate = l.EXPIRATION_DATE,
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

        public async Task<FormatedResponse> Create(GenericUnitOfWork _uow, SysOtherListDTO dto, string sid)
        {
            dto.IsActive = true;
            var response = await _genericRepository.Create(_uow, dto, sid);
            return response;
        }

        public async Task<FormatedResponse> CreateRange(GenericUnitOfWork _uow, List<SysOtherListDTO> dtos, string sid)
        {
            var add = new List<SysOtherListDTO>();
            add.AddRange(dtos);
            var response = await _genericRepository.CreateRange(_uow, add, sid);
            return response;
        }

        public async Task<FormatedResponse> Update(GenericUnitOfWork _uow, SysOtherListDTO dto, string sid, bool patchMode = true)
        {
            var response = await _genericRepository.Update(_uow, dto, sid, patchMode);
            return response;
        }

        public async Task<FormatedResponse> UpdateRange(GenericUnitOfWork _uow, List<SysOtherListDTO> dtos, string sid, bool patchMode = true)
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
            foreach(var id in ids)
            {
                var check = _dbContext.SysOtherLists.Where(x => x.ID == id && x.IS_ACTIVE == true).Any();
                //check data dang su dung tai man hsnv
                var empcv = await _dbContext.HuEmployeeCvs.AsNoTracking().Where(e => e.GENDER_ID == id || e.NATIONALITY_ID == id || e.MARITAL_STATUS_ID == id || e.BANK_ID == id
                                           || e.LEARNING_LEVEL_ID == id || e.TRAINING_FORM_ID == id || e.NATIVE_ID == id || e.RELIGION_ID == id || e.EDUCATION_LEVEL_ID == id
                                           || e.EMPLOYEE_OBJECT_ID == id || e.LICENSE_ID == id || e.COMPUTER_SKILL_ID == id || e.SCHOOL_ID == id || e.RELIGION_ID == id).AnyAsync();
                var emp = await _dbContext.HuEmployees.AsNoTracking().Where(x => x.WORK_STATUS_ID == id || x.STATUS_DETAIL_ID == id).AnyAsync();
                //check data dang su dung tai man bien dong bh
                var insAri = await _dbContext.InsArisings.AsNoTracking().Where(x => x.INS_ORG_ID == id).AnyAsync();
                //check data dang su dung tai quan ly ky luat
                var descipline = await _dbContext.HuDisciplines.AsNoTracking().Where(x => x.DISCIPLINE_OBJ == id || x.DISCIPLINE_TYPE == id).AnyAsync();
                //check data dang su dung tai quan ly ky luat
                //var descipline = await _dbContext.HuDisciplines.AsNoTracking().Where(x => x.DISCIPLINE_OBJ == id).AnyAsync();
                //check data dang su dung tai quan ly ky luat
                //var descipline = await _dbContext.HuDisciplines.AsNoTracking().Where(x => x.DISCIPLINE_OBJ == id).AnyAsync();
                //check data dang su dung tai quan ly ky luat
                //var descipline = await _dbContext.HuDisciplines.AsNoTracking().Where(x => x.DISCIPLINE_OBJ == id).AnyAsync();

                var seDocument = await _dbContext.SeDocuments.Where(x => x.DOCUMENT_TYPE == id ).AsNoTracking().AnyAsync();
                var huCommend = await _dbContext.HuCommends.Where(x => x.COMMEND_OBJ_ID == id || x.AWARD_TITLE_ID == id || x.REWARD_ID == id || x.STATUS_ID == id).AsNoTracking().AnyAsync();
                var huDisciplines = await _dbContext.HuDisciplines.Where(x => x.DISCIPLINE_OBJ == id || x.STATUS_ID == id).AsNoTracking().AnyAsync();
                var huCertificate = await _dbContext.HuCertificates.Where(x => x.TYPE_CERTIFICATE == id || x.TYPE_TRAIN == id || x.LEVEL_ID == id || x.LEVEL_TRAIN == id).AnyAsync();
                var paListSalaries = await _dbContext.PaListsalariess.Where(x => x.DATA_TYPE == id).AnyAsync();
                var atRegisterLave = await _dbContext.AtRegisterLeaves.Where(x => x.TYPE_ID == id).AnyAsync();
                var potalRegisterOff = await _dbContext.PortalRegisterOffs.Where(x => x.TYPE_ID == id).AnyAsync();
                var huEvaluate = await _dbContext.HuEvaluates.Where(x => x.EVALUATE_TYPE == id || x.CLASSIFICATION_ID == id).AnyAsync();
                var insRegimes = await _dbContext.InsRegimess.Where(x => x.CAL_DATE_TYPE == id).AnyAsync();
                var huTerminate = await _dbContext.HuTerminates.Where(x => x.TYPE_ID == id || x.REASON_ID == id || x.STATUS_ID == id).AnyAsync();
                var huWorking = await _dbContext.HuWorkings.Where(x => x.TYPE_ID == id || x.STATUS_ID == id || x.SALARY_TYPE_ID == id).AnyAsync();
                var paListSalary = await _dbContext.PaListSals.Where(x => x.LIST_KYHIEU_ID == id).AnyAsync();
                var huJob = await _dbContext.HuJobs.Where(x => x.JOB_FAMILY_ID == id).AnyAsync();

                //code sys_other_list
                var code = await _dbContext.SysOtherLists.Where(x => x.ID == id).Select(c => c.CODE).FirstAsync();
                var portalRQC = await _dbContext.PortalRequestChanges.Where(x => x.SYS_OTHER_CODE == code).AnyAsync();
                var huFamily = await _dbContext.HuFamilys.Where(x => x.RELATIONSHIP_ID == id).AnyAsync();
                if (descipline || insAri || empcv || huDisciplines || huCommend || seDocument || huCertificate || potalRegisterOff || atRegisterLave || paListSalary || huEvaluate
                    || insRegimes || huTerminate || huWorking || portalRQC || huFamily || paListSalaries || huJob)
                {
                    return new FormatedResponse()
                    {
                        ErrorType = EnumErrorType.CATCHABLE,
                        MessageCode = CommonMessageCode.CAN_NOT_DELETE_RECORDS_HAVE_USE,
                        StatusCode = EnumStatusCode.StatusCode400
                    };
                }

                if (check)
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

        public async Task<FormatedResponse> GetAllGroupOtherListType()
        {
            var res = await (from t in _dbContext.SysOtherListTypes.AsNoTracking().DefaultIfEmpty()
                             select new { 
                                Id = t.ID,
                                Name = t.NAME,
                             }).ToListAsync();
            return new FormatedResponse() { InnerBody = res};
        }

        public async Task<FormatedResponse> GetOtherListByType(string typeCode)
        {
            if (typeCode == null || typeCode.Trim().Length == 0)
            {
                return new() { ErrorType = EnumErrorType.UNCATCHABLE, StatusCode = EnumStatusCode.StatusCode500};
            }
            var r = await (from p in _dbContext.SysOtherLists
                           join o in _dbContext.SysOtherListTypes on p.TYPE_ID equals o.ID
                           where p.IS_ACTIVE == true && o.CODE == typeCode
                           select new
                           {
                               Id = p.ID,
                               Name = p.NAME,
                               Code = p.CODE,
                           }).ToListAsync();
            return new FormatedResponse() { InnerBody = r };
        }

        public async Task<FormatedResponse> GetAllCommendObjByKey(long id)
        {
            id = 64;
            var res = await (from o in _dbContext.SysOtherLists.AsNoTracking().Where(x => x.TYPE_ID == id)
                             join t in _dbContext.SysOtherListTypes on o.TYPE_ID equals t.ID
                             select new {
                                Id = o.ID,
                                Name = o.NAME,
                             }).ToListAsync();
            return new FormatedResponse { InnerBody = res };
        }

        public async Task<FormatedResponse> GetAllStatusByKey(long id)
        {
            id = 65;
            var res = await (from o in _dbContext.SysOtherLists.AsNoTracking().Where(x => x.TYPE_ID == id)
                             join t in _dbContext.SysOtherListTypes on o.TYPE_ID equals t.ID
                             select new
                             {
                                 Id = o.ID,
                                 Name = o.NAME,
                             }).ToListAsync();
            return new FormatedResponse { InnerBody = res };
        }

        public async Task<FormatedResponse> GetAllSourceByKey(long id)
        {
            id = 66;
            var res = await(from o in _dbContext.SysOtherLists.AsNoTracking().Where(x => x.TYPE_ID == id)
                            join t in _dbContext.SysOtherListTypes on o.TYPE_ID equals t.ID
                            select new
                            {
                                Id = o.ID,
                                Name = o.NAME,
                            }).ToListAsync();
            return new FormatedResponse { InnerBody = res };
        }

        public async Task<FormatedResponse> GetAllGender(long id)
        {
            id = 6;
            var res = await(from o in _dbContext.SysOtherLists.AsNoTracking().Where(x => x.TYPE_ID == id)
                            join t in _dbContext.SysOtherListTypes on o.TYPE_ID equals t.ID
                            select new
                            {
                                Id = o.ID,
                                Name = o.NAME,
                            }).ToListAsync();
            return new FormatedResponse { InnerBody = res };
        }

        public async Task<FormatedResponse> GetAllWelfareByKey(long id)
        {
            id = 67;
            var res = await(from o in _dbContext.SysOtherLists.AsNoTracking().Where(x => x.TYPE_ID == id)
                            join t in _dbContext.SysOtherListTypes on o.TYPE_ID equals t.ID
                            select new
                            {
                                Id = o.ID,
                                Name = o.NAME,
                            }).ToListAsync();
            return new FormatedResponse { InnerBody = res };
        }

        public virtual async Task<FormatedResponse> ToggleActiveIds(GenericUnitOfWork _uow, List<long> ids, bool valueToBind, string sid)
        {
            var response = await _genericRepository.ToggleActiveIds(_uow, ids, valueToBind, sid);
            return response;
        }

        public async Task<FormatedResponse> GetStatusCommend()
        {
            var response = await (from t in _uow.Context.Set<SYS_OTHER_LIST_TYPE>().Where(x => x.CODE == "STATUS")
                                  from o in _uow.Context.Set<SYS_OTHER_LIST>().Where(x => x.TYPE_ID == t.ID).DefaultIfEmpty()
                                  where (o.CODE != "TC")
                                  select new
                                  {
                                      Id = o.ID,
                                      Name = o.NAME,
                                      Code = o.CODE
                                  }).ToListAsync();
            return new FormatedResponse() { InnerBody =  response };
        }

        public async Task<FormatedResponse> GetStatusApproveHuFamilyEdit()
        {
            var response = await (from t in _uow.Context.Set<SYS_OTHER_LIST_TYPE>().Where(x => x.CODE == "STATUS")
                                  from o in _uow.Context.Set<SYS_OTHER_LIST>().Where(x => x.TYPE_ID == t.ID).DefaultIfEmpty()
                                  where o.CODE != "CD"
                                  select new
                                  {
                                      Id = o.ID,
                                      Name = o.NAME,
                                      Code = o.CODE
                                  }).ToArrayAsync();
            return new FormatedResponse() { InnerBody = response };
        }

        public async Task<FormatedResponse> GetEvaluateType()
        {
            var response = await (from t in _uow.Context.Set<SYS_OTHER_LIST_TYPE>().Where(x => x.CODE == "CLASSIFICATION_TYPE")
                                  from o in _uow.Context.Set<SYS_OTHER_LIST>().Where(x => x.TYPE_ID == t.ID).DefaultIfEmpty()
                                  where o.CODE != "LXL03"
                                  select new
                                  {
                                      Id = o.ID,
                                      Name = o.NAME,
                                      Code = o.CODE
                                  }).ToListAsync();
            return new FormatedResponse() { InnerBody = response };
        }

        public async Task<FormatedResponse> GetIdStatusByCode(string code)
        {
            var response = await (from item in _dbContext.SysOtherLists
                                  where item.CODE == code.ToUpper()
                                  select new
                                  {
                                      Id = item.ID,
                                      Code = item.CODE,
                                      Name = item.NAME
                                  }).FirstOrDefaultAsync();
            return new FormatedResponse() { InnerBody = response };
        }
    }
}

