using CORE.GenericUOW;
using CORE.DTO;
using API.DTO;
using CORE.Enum;
using CORE.StaticConstant;
using API.All.DbContexts;
using CORE.AutoMapper;
using Common.Interfaces;
using Common.DataAccess;
using DocumentFormat.OpenXml.Drawing.Charts;

namespace API.Controllers.SysPaFormula
{
    public class SysPaFormulaRepository : ISysPaFormulaRepository
    {
        private readonly GenericUnitOfWork _uow;
        private readonly FullDbContext _dbContext;
        private IGenericRepository<SYS_PA_FORMULA, SysPaFormulaDTO> _genericRepository;
        private readonly GenericReducer<SYS_PA_FORMULA, SysPaFormulaDTO> _genericReducer;
        protected AbsQueryDataTemplate QueryData;

        public SysPaFormulaRepository(FullDbContext context, GenericUnitOfWork uow)
        {
            _dbContext = context;
            _uow = uow;
            _genericRepository = _uow.GenericRepository<SYS_PA_FORMULA, SysPaFormulaDTO>();
            _genericReducer = new();
            QueryData = new SqlQueryDataTemplate(_dbContext);
        }

        public async Task<GenericPhaseTwoListResponse<SysPaFormulaDTO>> SinglePhaseQueryList(GenericQueryListDTO<SysPaFormulaDTO> request)
        {
            var joined = from p in _dbContext.PaListsalariess.AsNoTracking().Where(p => p.IS_FORMULA == true || p.IS_SUM_FORMULA == true)
                         join pls in _dbContext.PaListSals on p.CODE_SAL equals pls.ID into pl

                         from s in pl.DefaultIfEmpty()
                         join la in _dbContext.SysPaFormulas on s.CODE_LISTSAL equals la.COL_NAME into ps
                         from fgi in ps.Where(f => f.SALARY_TYPE_ID == p.OBJ_SAL_ID).DefaultIfEmpty()

                         //from r in ps.DefaultIfEmpty()
                         //from pls in _dbContext.PaListSals.Where(pls => pls.ID == p.CODE_SAL).DefaultIfEmpty()
                         select new SysPaFormulaDTO
                         {
                             Id = p.ID,
                             Orders = fgi.ORDERS,
                             SalaryTypeId = p.OBJ_SAL_ID,
                             ColName = s.CODE_LISTSAL,
                             Formula = fgi.FORMULA,
                             ObjSalary = p.NAME
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
                var list = new List<SYS_PA_FORMULA>
                    {
                        (SYS_PA_FORMULA)response
                    };
                var joined = (from l in list
                              select new SysPaFormulaDTO
                              {
                                  Id = l.ID,
                                  FormulaName = l.FORMULA_NAME,
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

        public async Task<FormatedResponse> Create(GenericUnitOfWork _uow, SysPaFormulaDTO dto, string sid)
        {
            var response = await _genericRepository.Create(_uow, dto, sid);
            return response;
        }

        public async Task<FormatedResponse> CreateRange(GenericUnitOfWork _uow, List<SysPaFormulaDTO> dtos, string sid)
        {
            var add = new List<SysPaFormulaDTO>();
            add.AddRange(dtos);
            var response = await _genericRepository.CreateRange(_uow, add, sid);
            return response;
        }

        public async Task<FormatedResponse> Update(GenericUnitOfWork _uow, SysPaFormulaDTO dto, string sid, bool patchMode = true)
        {
            if(dto.Orders == null || dto.Orders == 0)
            {
                return new FormatedResponse() { MessageCode = CommonMessageCode.ORDER_REQUIRED, ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400 };
            }
            var response = new FormatedResponse();
            var data = await _dbContext.SysPaFormulas.Where(s => s.SALARY_TYPE_ID == dto.SalaryTypeId && s.COL_NAME == dto.ColName).FirstOrDefaultAsync();
            if(data == null)
            {
                response = await _genericRepository.Create(_uow, dto, sid);
            }
            else
            {
                dto.Id = data.ID;
                response = await _genericRepository.Update(_uow, dto, sid, patchMode);
            }
            return response;
        }

        public async Task<FormatedResponse> UpdateRange(GenericUnitOfWork _uow, List<SysPaFormulaDTO> dtos, string sid, bool patchMode = true)
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

        public async Task<FormatedResponse> CheckFormula(SysPaFormulaDTO param)
        {
            var check = CheckFomuler(param.ColName!, param.Formula!, param.SalaryTypeId!.Value);
            
            if(check == true)
            {
                return new FormatedResponse() { StatusCode = EnumStatusCode.StatusCode200 };
            }
            else
            {
                return new FormatedResponse() { StatusCode = EnumStatusCode.StatusCode400 };
            }
        }

        public bool CheckFomuler(string sCol, string sFormuler, long objID)
        {
            try
            {
                string sql = "";
                string sql1 = "";
                //string sql2 = "";
                sql = "UPDATE TEMP_CALCULATE SET " + sCol + " = " + sFormuler + "";
                sql1 = "UPDATE TEMP_CALCULATE_SUM SET " + sCol + " = " + sFormuler + "";
                //sql2 = "UPDATE PA_INCOME_TAX_SUM T SET T." + sCol + " = " + sFormuler + "";
                sql += " WHERE 1=0 ";
                sql1 += " WHERE 1=0 ";
                //sql2 += " WHERE 1=0 ";
                /*if (objID == 186)
                    cls.ExecuteSQL(sql2);*/
                int r =  QueryData.ExecuteNonQuery(sql).Result;
                int r1 = QueryData.ExecuteNonQuery(sql1).Result;
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public virtual async Task<FormatedResponse> ToggleActiveIds(GenericUnitOfWork _uow, List<long> ids, bool valueToBind, string sid)
        {
            var response = await _genericRepository.ToggleActiveIds(_uow, ids, valueToBind, sid);
            return response;
        }
    }
}

