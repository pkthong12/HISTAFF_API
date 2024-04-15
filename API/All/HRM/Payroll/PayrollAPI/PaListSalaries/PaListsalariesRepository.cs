using CORE.GenericUOW;
using CORE.DTO;
using API.DTO;
using CORE.Enum;
using CORE.StaticConstant;
using API.All.DbContexts;
using CORE.AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Collections.Generic;
using Common.Interfaces;
using Common.DataAccess;
using MimeKit.Text;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace API.Controllers.PaListsalaries
{
    public class PaListsalariesRepository : IPaListsalariesRepository
    {
        private readonly GenericUnitOfWork _uow;
        private readonly FullDbContext _dbContext;
        private IGenericRepository<PA_LISTSALARIES, PaListsalariesDTO> _genericRepository;
        private readonly GenericReducer<PA_LISTSALARIES, PaListsalariesDTO> _genericReducer;
        protected AbsQueryDataTemplate QueryData;

        public PaListsalariesRepository(FullDbContext context, GenericUnitOfWork uow)
        {
            _dbContext = context;
            _uow = uow;
            _genericRepository = _uow.GenericRepository<PA_LISTSALARIES, PaListsalariesDTO>();
            _genericReducer = new();
            QueryData = new SqlQueryDataTemplate(_dbContext);
        }

        public async Task<GenericPhaseTwoListResponse<PaListsalariesDTO>> SinglePhaseQueryList(GenericQueryListDTO<PaListsalariesDTO> request)
        {
            var joined = from p in _dbContext.PaListsalariess.AsNoTracking()
                             // JOIN OTHER ENTITIES BASED ON THE BUSINESS
                         from st in _dbContext.HuSalaryTypes.AsNoTracking().Where(st => st.ID == p.OBJ_SAL_ID).DefaultIfEmpty()
                         from otl1 in _dbContext.SysOtherLists.AsNoTracking().Where(otl1 => otl1.ID == p.GROUP_TYPE).DefaultIfEmpty()
                         from otl2 in _dbContext.SysOtherLists.AsNoTracking().Where(otl2 => otl2.ID == p.DATA_TYPE).DefaultIfEmpty()
                         from cosal in _dbContext.PaListSals.AsNoTracking().Where(cosal => cosal.ID == p.CODE_SAL).DefaultIfEmpty()
                         from c in _dbContext.SysUsers.AsNoTracking().Where(c => c.ID == p.CREATED_BY).DefaultIfEmpty()
                         from u in _dbContext.SysUsers.AsNoTracking().Where(u => u.ID == p.CREATED_BY).DefaultIfEmpty()
                         select new PaListsalariesDTO
                         {
                             Id = p.ID,
                             ObjSalId = st.ID,
                             ObjSalName = st.NAME,//doi tuong luong/nhom cong thuc
                             DataTypeName = otl2.NAME,
                             CodeSalName = cosal.CODE_LISTSAL,
                             Name = p.NAME,
                             GroupType = otl1.ID,
                             GroupTypeName = otl1.NAME,
                             EffectiveDate = p.EFFECTIVE_DATE,
                             ColIndex = p.COL_INDEX,
                             Status = p.IS_ACTIVE.HasValue ? "Áp dụng" : "Ngừng áp dụng",
                             IsFormula = p.IS_FORMULA,
                             IsSumFormula = p.IS_SUM_FORMULA,
                             IsPayback = p.IS_PAYBACK,
                             IsVisible = p.IS_VISIBLE,
                             IsImport = p.IS_IMPORT,
                             IsQlTypeTn = p.IS_QL_TYPE_TN,
                             Note = p.NOTE,
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

            var joined = (from p in _dbContext.PaListsalariess.AsNoTracking().Where(p => p.ID == id)
                              // JOIN OTHER ENTITIES BASED ON THE BUSINESS
                          from st in _dbContext.HuSalaryTypes.AsNoTracking().Where(st => st.ID == p.OBJ_SAL_ID).DefaultIfEmpty()
                          from otl1 in _dbContext.SysOtherLists.AsNoTracking().Where(otl1 => otl1.ID == p.GROUP_TYPE).DefaultIfEmpty()
                          from otl2 in _dbContext.SysOtherLists.AsNoTracking().Where(otl2 => otl2.ID == p.DATA_TYPE).DefaultIfEmpty()
                          from cosal in _dbContext.PaListSals.AsNoTracking().Where(cosal => cosal.ID == p.CODE_SAL).DefaultIfEmpty()
                          select new PaListsalariesDTO
                          {
                              Id = p.ID,
                              ObjSalId = p.OBJ_SAL_ID,
                              ObjSalName = st.NAME,//doi tuong luong/nhom cong thuc
                              DataType = p.DATA_TYPE,
                              DataTypeName = otl2.NAME,
                              CodeSal = p.CODE_SAL,
                              CodeSalName = cosal.CODE_LISTSAL,
                              Name = p.NAME,
                              GroupType = p.GROUP_TYPE,
                              GroupTypeName = otl1.NAME,
                              EffectiveDate = p.EFFECTIVE_DATE,
                              ColIndex = p.COL_INDEX,
                              Status = p.IS_ACTIVE.HasValue ? "Áp dụng" : "Ngừng áp dụng",
                              IsFormula = p.IS_FORMULA,
                              IsSumFormula = p.IS_SUM_FORMULA,
                              IsPayback = p.IS_PAYBACK,
                              IsVisible = p.IS_VISIBLE,
                              IsImport = p.IS_IMPORT,
                              IsQlTypeTn = p.IS_QL_TYPE_TN,
                              Note = p.NOTE,
                          }).FirstOrDefault();
            if (joined != null)
            {
                return new() { InnerBody = joined };
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

        public async Task<FormatedResponse> Create(GenericUnitOfWork _uow, PaListsalariesDTO dto, string sid)
        {
            //var checkExists= _fullDbContext.PaListsalariess.AsNoTracking().Where(x => x.OBJ_SAL_ID == dto.ObjSalId && x.CODE_SAL == dto.CodeSal).Count();
            //if (checkExists != 0)
            //{
               
            //    return new() { ErrorType = EnumErrorType.UNCATCHABLE, MessageCode = CommonMessageCode.CREATE_OBJECT_HAS_SAME_CODE, StatusCode = EnumStatusCode.StatusCode400 };
            //}
            //else
            //{
            //    if (dto.IsImport == true)
            //    {
            //        var r = await QueryData.ExecuteNonQuery("PKG_PAYROLL_ADD_COLUMN",
            //            new
            //            {
            //                COL_NAME = dto.Name!.ToUpper().Replace(' ', '_'),
            //                DATA_TYPE = dto.DataType == 1054 ? "BIGINT" : "NVARCHAR(50)",
            //                FROM_TABLE = "PA_LISTSALARIES",
            //            }, false);
            //    }
            //    var response = await _genericRepository.Create(_uow, dto, sid);
            //    return response;
            //}

            if (dto.IsImport == true)
            {
                var data = await _dbContext.PaListSals.Where(p => p.ID == dto.CodeSal).FirstAsync();
                var r = await QueryData.ExecuteNonQuery("PKG_PAYROLL_ADD_COLUMN",
                    new
                    {
                        COL_NAME = data.CODE_LISTSAL!.ToUpper().Replace(' ', '_'),
                        DATA_TYPE = dto.DataType == 1054 ? "FLOAT" : "NVARCHAR(50)",
                        FROM_TABLE = "PA_LISTSALARIES",
                    }, false);
            }
            var response = await _genericRepository.Create(_uow, dto, sid);
            return response;
        }

        public async Task<FormatedResponse> CreateRange(GenericUnitOfWork _uow, List<PaListsalariesDTO> dtos, string sid)
        {
            var add = new List<PaListsalariesDTO>();
            add.AddRange(dtos);
            var response = await _genericRepository.CreateRange(_uow, add, sid);
            return response;
        }

        public async Task<FormatedResponse> Update(GenericUnitOfWork _uow, PaListsalariesDTO dto, string sid, bool patchMode = true)
        {
            //var query = _dbContext.PaListsalariess.AsNoTracking().Where(x => x.OBJ_SAL_ID == dto.ObjSalId).ToList();
            //var check = 
            //if (query)
            //{
            //    return new FormatedResponse()
            //    {
            //        MessageCode = CommonMessageCode.POST_UPDATE_FAILLED,
            //        InnerBody = dto,
            //        StatusCode = EnumStatusCode.StatusCode400
            //    };
            //}
            var response = await _genericRepository.Update(_uow, dto, sid, patchMode);
            return response;
        }

        public async Task<FormatedResponse> UpdateRange(GenericUnitOfWork _uow, List<PaListsalariesDTO> dtos, string sid, bool patchMode = true)
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

        public async Task<FormatedResponse> GetObjSal()
        {//lay doi tuong luong
            var query = await (from o in _dbContext.HuSalaryTypes.AsNoTracking()
                               .Where(o => o.IS_ACTIVE == true).OrderBy(o => o.NAME)
                               orderby o.ID
                               select new
                               {
                                   Id = o.ID,
                                   Name = o.NAME,
                               }).ToListAsync();
            return new FormatedResponse() { InnerBody = query };
        }

        public async Task<FormatedResponse> GetGroupType()
        {//lay nhom doi tuong luong
            var query = await (from t in _dbContext.SysOtherLists.AsNoTracking()
                                .Where(t => t.IS_ACTIVE == true && t.TYPE_ID == 86)
                               orderby t.ID
                               select new
                               {
                                   Id = t.ID,
                                   Name = t.NAME,
                               }).ToListAsync();
            return new FormatedResponse() { InnerBody = query };
        }

        public async Task<FormatedResponse> GetDataType()
        {//lay kieu dl
            var query = await (from d in _dbContext.SysOtherLists.AsNoTracking()
                              .Where(d => d.IS_ACTIVE == true && d.TYPE_ID == 85)
                               orderby d.ID
                               select new
                               {
                                   Id = d.ID,
                                   Name = d.NAME,
                               }).ToListAsync();
            return new FormatedResponse() { InnerBody = query };
        }

        public async Task<FormatedResponse> GetListSal(long idSymbol)
        {//lay ma danh muc luong theo nhom ky hieu
            var query = await (from s in _dbContext.PaListSals.AsNoTracking()
                              .Where(s => s.LIST_KYHIEU_ID == idSymbol)
                               orderby s.ID
                               select new
                               {
                                   Id = s.ID,
                                   Name = s.CODE_LISTSAL,
                               }).ToListAsync();
            return new FormatedResponse() { InnerBody = query };
        }


        public async Task<FormatedResponse> GetListSalaries(long idObjSal)
        {
            var query = await (from p in _dbContext.PaListsalariess.AsNoTracking()
                              .Where(l => l.OBJ_SAL_ID == idObjSal)
                               from st in _dbContext.HuSalaryTypes.AsNoTracking().Where(st => st.ID == idObjSal).DefaultIfEmpty()
                               from otl1 in _dbContext.SysOtherLists.AsNoTracking().Where(otl1 => otl1.ID == p.GROUP_TYPE).DefaultIfEmpty()
                               from otl2 in _dbContext.SysOtherLists.AsNoTracking().Where(otl2 => otl2.ID == p.DATA_TYPE).DefaultIfEmpty()
                               from cosal in _dbContext.PaListSals.AsNoTracking().Where(x => x.ID == p.CODE_SAL).DefaultIfEmpty()
                               select new
                               {
                                   Id = p.ID,
                                   ObjSalId = p.ID,
                                   ObjSalName = st.NAME,//doi tuong luong/nhom cong thuc
                                   DataTypeName = p.NAME,
                                   CodeSalName = cosal.CODE_LISTSAL,
                                   Name = p.NAME,
                                   GroupType = p.ID,
                                   GroupTypeName = otl1.NAME,
                                   EffectiveDate = p.EFFECTIVE_DATE,
                                   ColIndex = p.COL_INDEX,
                                   Status = p.IS_ACTIVE.HasValue ? "Áp dụng" : "Ngừng áp dụng",
                                   IsFormula = p.IS_FORMULA,
                                   IsSumFormula = p.IS_SUM_FORMULA,
                                   IsPayback = p.IS_PAYBACK,
                                   IsVisible = p.IS_VISIBLE,
                                   IsImport = p.IS_IMPORT,
                                   IsQlTypeTn = p.IS_QL_TYPE_TN,
                                   Note = p.NOTE,
                               }).ToListAsync();
            return new FormatedResponse() { InnerBody = query };
        }

        public async Task<FormatedResponse> GetListNameCode(PaListsalariesDTO param)
        {
            var query = await (from d in _dbContext.PaListsalariess.AsNoTracking().Where(d => d.OBJ_SAL_ID == param.ObjSalId)
                               from s in _dbContext.PaListSals.AsNoTracking().Where(s => s.ID == d.CODE_SAL).DefaultIfEmpty()
                               orderby d.ID
                               select new
                               {
                                   Id = d.ID,
                                   Code = s.CODE_LISTSAL,
                                   Name = $"{d.NAME} - ({s.CODE_LISTSAL})"
                               }).ToListAsync();
            return new FormatedResponse() { InnerBody = query };
        }

        public virtual async Task<FormatedResponse> ToggleActiveIds(GenericUnitOfWork _uow, List<long> ids, bool valueToBind, string sid)
        {
            await Task.Run(() => null);
            throw new NotImplementedException();
        }
        public async Task<FormatedResponse> GetListObjSalry()
        {
            var query = await (from t in _dbContext.SysOtherLists.AsNoTracking().Where(p => p.CODE == "NDTL01").DefaultIfEmpty()
                               from s in _dbContext.HuSalaryTypes.AsNoTracking().Where(o => o.IS_ACTIVE == true && o.SALARY_TYPE_GROUP == t.ID).OrderBy(o => o.NAME).DefaultIfEmpty()
                               select new
                               {
                                   Id = s.ID,
                                   Name = s.NAME,
                               }).ToListAsync();
            return new FormatedResponse() { InnerBody = query };
        }
    }
}

