using CORE.GenericUOW;
using CORE.DTO;
using API.DTO;
using CORE.Enum;
using CORE.StaticConstant;
using API.All.DbContexts;
using CORE.AutoMapper;
using System.Linq;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;
using DocumentFormat.OpenXml.Drawing.Charts;

namespace API.Controllers.PaPhaseAdvance
{
    public class PaPhaseAdvanceRepository : IPaPhaseAdvanceRepository
    {
        private readonly GenericUnitOfWork _uow;
        private readonly FullDbContext _dbContext;
        private IGenericRepository<PA_PHASE_ADVANCE, PaPhaseAdvanceDTO> _genericRepository;
        private readonly GenericReducer<PA_PHASE_ADVANCE, PaPhaseAdvanceDTO> _genericReducer;

        public PaPhaseAdvanceRepository(FullDbContext context, GenericUnitOfWork uow)
        {
            _dbContext = context;
            _uow = uow;
            _genericRepository = _uow.GenericRepository<PA_PHASE_ADVANCE, PaPhaseAdvanceDTO>();
            _genericReducer = new();
        }

        public async Task<GenericPhaseTwoListResponse<PaPhaseAdvanceDTO>> SinglePhaseQueryList(GenericQueryListDTO<PaPhaseAdvanceDTO> request)
        {
            var joined = from p in _dbContext.PaPhaseAdvances.AsNoTracking()
                             // JOIN OTHER ENTITIES BASED ON THE BUSINESS
                         from o in _dbContext.HuOrganizations.AsNoTracking().Where(o => o.ID == p.ORG_ID).DefaultIfEmpty()
                         from s in _dbContext.AtSalaryPeriods.AsNoTracking().Where(s => s.ID == p.PERIOD_ID).DefaultIfEmpty()
                             //from a in _dbContext.PaPhaseAdvanceSymbols.AsNoTracking().Where(x => x.PHASE_ADVANCE_ID == p.ID).DefaultIfEmpty()
                        from s1 in _dbContext.AtSalaryPeriods.AsNoTracking().Where(x => x.ID == p.FROM_SALARY).DefaultIfEmpty()
                        from s2 in _dbContext.AtSalaryPeriods.AsNoTracking().Where(x => x.ID == p.TO_SALARY).DefaultIfEmpty()
                         select new PaPhaseAdvanceDTO
                         {
                             Id = p.ID,
                             OrgId = p.ORG_ID,
                             OrgName = o.NAME,
                             Year = p.YEAR,
                             PhaseDay = p.PHASE_DAY,
                             NameVn = p.NAME_VN,
                             MonthLbs = p.MONTH_LBS,
                             FromDate = p.FROM_DATE,
                             ToDate = p.TO_DATE,
                             PeriodId = p.PERIOD_ID,
                             PeriodName = s.NAME,
                             SymbolName = string.Join("; ",(from x in _dbContext.PaPhaseAdvanceSymbols.AsNoTracking().Where(e=> e.PHASE_ADVANCE_ID == p.ID)
                                                           from h in _dbContext.AtSymbols.AsNoTracking().Where(h=> h.ID == x.SYMBOL_ID)
                                                           select h.CODE +"-"+h.NAME).ToList()),
                             Note = p.NOTE,
                             IsActive = p.IS_ACTIVE,
                             Seniority = p.SENIORITY,
                             Status = p.IS_ACTIVE == true ? "Áp dụng" : "Ngừng áp dụng",
                             FromSalary = p.FROM_SALARY,
                             ToSalary = p.TO_SALARY,
                             FromSalaryName = s1.NAME,
                             ToSalaryName = s2.NAME,

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
            var entity = _uow.Context.Set<PA_PHASE_ADVANCE>().AsNoTracking().AsQueryable();
            var org = _uow.Context.Set<HU_ORGANIZATION>().AsNoTracking().AsQueryable();
            var atSal = _uow.Context.Set<AT_SALARY_PERIOD>().AsNoTracking().AsQueryable();
            var res = await _genericRepository.GetById(id);
            var joined = (from l in entity.Where(p => p.ID == id)
                          from o in org.Where(o => o.ID == l.ORG_ID).DefaultIfEmpty()
                          from pa in atSal.Where(pa => pa.ID == l.PERIOD_ID).DefaultIfEmpty()
                              // JOIN OTHER ENTITIES BASED ON THE BUSINESS
                          select new PaPhaseAdvanceDTO
                          {
                              Id = l.ID,
                              OrgId = l.ORG_ID,
                              OrgName = o.NAME,
                              Year = l.YEAR,
                              PhaseDay = l.PHASE_DAY,
                              NameVn = l.NAME_VN,
                              MonthLbs = l.MONTH_LBS,
                              FromDate = l.FROM_DATE,
                              ToDate = l.TO_DATE,
                              PeriodId = l.PERIOD_ID,
                              PeriodName = pa.NAME,
                              Note = l.NOTE,
                              SymbolId = l.SYMBOL_ID,
                              Symbol = l.SYMBOL,
                              Seniority = l.SENIORITY,
                              Status = l.IS_ACTIVE!.Value == true ? "Áp dụng" : "Ngừng áp dụng",
                              FromSalary = l.FROM_SALARY,
                              ToSalary = l.TO_SALARY,
                          }).FirstOrDefault();

            var listSymbol = _dbContext.PaPhaseAdvanceSymbols.AsNoTracking().Where(x => x.PHASE_ADVANCE_ID == id).Select(x => x.SYMBOL_ID).ToList();
            //var listSymbol =

            if (joined != null && listSymbol != null)
            {
                joined.ListSymbolId = listSymbol;
            }

            return new FormatedResponse() { InnerBody = joined };
        }

        public async Task<FormatedResponse> GetById(string id)
        {
            await Task.Run(() => null);
            throw new NotImplementedException();
        }

        public async Task<FormatedResponse> Create(GenericUnitOfWork _uow, PaPhaseAdvanceDTO dto, string sid)
        {
            dto.IsActive = true;
            var response = await _genericRepository.Create(_uow, dto, sid);
            return response;
        }

        public async Task<FormatedResponse> CreateRange(GenericUnitOfWork _uow, List<PaPhaseAdvanceDTO> dtos, string sid)
        {
            var add = new List<PaPhaseAdvanceDTO>();
            add.AddRange(dtos);
            var response = await _genericRepository.CreateRange(_uow, add, sid);
            return response;
        }

        public async Task<FormatedResponse> Update(GenericUnitOfWork _uow, PaPhaseAdvanceDTO dto, string sid, bool patchMode = true)
        {
            //string symbolIdString = "";
            //if (dto.ListSymbolId != null && dto.ListSymbolId!.Count != 0)
            //{
            //    foreach (var id in dto.ListSymbolId)
            //    {
            //        symbolIdString += id.ToString() + ",";
            //    }
            //    symbolIdString = symbolIdString.Remove(symbolIdString.Length - 1, 1).ToString();
            //}

            //dto.Symbol = symbolIdString;
            var response = await _genericRepository.Update(_uow, dto, sid, patchMode);
            return response;
        }

        public async Task<FormatedResponse> UpdateRange(GenericUnitOfWork _uow, List<PaPhaseAdvanceDTO> dtos, string sid, bool patchMode = true)
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

        public async Task<FormatedResponse> GetYearPeriod()
        {
            var query = await (from p in _dbContext.AtSalaryPeriods.AsNoTracking()
                               .Where(x => x.IS_ACTIVE == true)
                               orderby p.YEAR
                               select new
                               {
                                   Id = p.ID,
                                   //Year = p.YEAR,
                                   Year = p.YEAR.ToString() + '-' + p.NAME,
                                   ValYear = p.YEAR
                               }).ToListAsync();
            return new FormatedResponse() { InnerBody = query };
        }

        public async Task<FormatedResponse> GetOrgId()
        {
            var query = await (from o in _dbContext.HuOrganizations
                               where o.IS_ACTIVE == true
                               orderby o.ID
                               select new
                               {
                                   Id = o.ID,
                                   OrgName = o.NAME,
                               }).ToListAsync();
            return new FormatedResponse() { InnerBody = query };
        }

        public async Task<FormatedResponse> GetMonthPeriodAt(int year)
        {
            var query = await (from pa in _dbContext.AtSalaryPeriods.AsNoTracking()
                               .Where(pa => pa.YEAR == year && pa.IS_ACTIVE == true)
                               orderby pa.YEAR
                               select new
                               {
                                   Id = pa.ID,
                                   Month = pa.NAME
                               }).ToListAsync();
            return new FormatedResponse() { InnerBody = query };
        }

        public async Task<FormatedResponse> GetAtSymbol()
        {
            var query = await (from pa in _dbContext.AtSymbols.AsNoTracking()
                               .Where(pa => pa.IS_ACTIVE == true)
                               orderby pa.ID
                               select new
                               {
                                   Id = pa.ID,
                                   Name = pa.CODE + '-' + pa.NAME
                               }).ToListAsync();
            return new FormatedResponse() { InnerBody = query };
        }

        public async Task<FormatedResponse> GetOtherListSal()
        {
            var query = await (from o in _dbContext.SysOtherLists.AsNoTracking()
                               .Where(o => o.IS_ACTIVE == true && o.TYPE_ID == 96)
                               orderby o.ID
                               select new
                               {
                                   Id = o.ID,
                                   SalMonthName = o.NAME,
                               }).ToListAsync();
            return new FormatedResponse() { InnerBody = query };
        }

        public async Task<FormatedResponse> GetYearByPeriod(long id)
        {
            var query = await (from pa in _dbContext.AtSalaryPeriods.AsNoTracking()
                               .Where(x => x.ID == id)
                               orderby pa.YEAR
                               select new
                               {
                                   Id = pa.ID,
                                   Year = pa.YEAR
                               }).ToListAsync();
            return new FormatedResponse() { InnerBody = query };
        }

        public virtual async Task<FormatedResponse> ToggleActiveIds(GenericUnitOfWork _uow, List<long> ids, bool valueToBind, string sid)
        {
            var response = await _genericRepository.ToggleActiveIds(_uow, ids, valueToBind, sid);
            return response;
        }
    }
}

