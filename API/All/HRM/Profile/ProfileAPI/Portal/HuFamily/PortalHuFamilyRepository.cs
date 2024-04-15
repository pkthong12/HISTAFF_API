using CORE.GenericUOW;
using CORE.DTO;
using API.DTO;
using CORE.Enum;
using CORE.StaticConstant;
using API.All.DbContexts;
using CORE.AutoMapper;

namespace API.Controllers.HuFamily
{
    public class PortalHuFamilyRepository : IPortalHuFamilyRepository
    {
        private readonly GenericUnitOfWork _uow;
        private readonly FullDbContext _dbContext;
        private IGenericRepository<HU_FAMILY, HuFamilyDTO> _genericRepository;
        private readonly GenericReducer<HU_FAMILY, HuFamilyDTO> _genericReducer;

        public PortalHuFamilyRepository(FullDbContext context, GenericUnitOfWork uow)
        {
            _dbContext = context;
            _uow = uow;
            _genericRepository = _uow.GenericRepository<HU_FAMILY, HuFamilyDTO>();
            _genericReducer = new();
        }

        public async Task<GenericPhaseTwoListResponse<HuFamilyDTO>> SinglePhaseQueryList(GenericQueryListDTO<HuFamilyDTO> request)
        {
            var joined = from p in _dbContext.HuFamilys.AsNoTracking()
                         // JOIN OTHER ENTITIES BASED ON THE BUSINESS
                         select new HuFamilyDTO
                         {
                             Id = p.ID
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
                var list = new List<HU_FAMILY>
                    {
                        (HU_FAMILY)response
                    };
                var joined = (from l in list
                              // JOIN OTHER ENTITIES BASED ON THE BUSINESS
                              select new HuFamilyDTO
                              {
                                  Id = l.ID,
                                  Fullname = l.FULLNAME,
                                  RelationshipId = l.RELATIONSHIP_ID,
                                  Gender = l.GENDER,
                                  BirthDate = l.BIRTH_DATE,
                                  IdNo = l.ID_NO,
                                  SameCompany = l.SAME_COMPANY,
                                  IsDead = l.IS_DEAD,
                                  IsDeduct = l.IS_DEDUCT,
                                  RegistDeductDate = l.REGIST_DEDUCT_DATE,
                                  DeductFrom = l.DEDUCT_FROM,
                                  DeductTo = l.DEDUCT_TO,
                                  IsHousehold = l.IS_HOUSEHOLD,
                                  PitCode = l.PIT_CODE,
                                  Career = l.CAREER,
                                  Nationality = l.NATIONALITY,
                                  BirthCerProvince = l.BIRTH_CER_PROVINCE,
                                  BirthCerDistrict = l.BIRTH_CER_DISTRICT,
                                  BirthCerWard = l.BIRTH_CER_WARD,
                                  Note = l.NOTE,
                                  EmployeeId = l.EMPLOYEE_ID
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

        public async Task<FormatedResponse> Create(GenericUnitOfWork _uow, HuFamilyDTO dto, string sid)
        {
            var response = await _genericRepository.Create(_uow, dto, sid);
            return response;
        }

        public async Task<FormatedResponse> CreateRange(GenericUnitOfWork _uow, List<HuFamilyDTO> dtos, string sid)
        {
            var add = new List<HuFamilyDTO>();
            add.AddRange(dtos);
            var response = await _genericRepository.CreateRange(_uow, add, sid);
            return response;
        }

        public async Task<FormatedResponse> Update(GenericUnitOfWork _uow, HuFamilyDTO dto, string sid, bool patchMode = true)
        {
            var response = await _genericRepository.Update(_uow, dto, sid, patchMode);
            return response;
        }

        public async Task<FormatedResponse> UpdateRange(GenericUnitOfWork _uow, List<HuFamilyDTO> dtos, string sid, bool patchMode = true)
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
        public async Task<FormatedResponse> GetAllFamilyByEmployee(long employeeId)
        {
            var entity = _uow.Context.Set<HU_FAMILY>().AsNoTracking().AsQueryable();
            var employees = _uow.Context.Set<HU_EMPLOYEE>().AsNoTracking().AsQueryable();
            var positions = _uow.Context.Set<HU_POSITION>().AsNoTracking().AsQueryable();
            var organizations = _uow.Context.Set<HU_ORGANIZATION>().AsNoTracking().AsQueryable();
            var otherLists = _uow.Context.Set<SYS_OTHER_LIST>().AsNoTracking().AsQueryable();
            var provinces = _uow.Context.Set<HU_PROVINCE>().AsNoTracking().AsQueryable();
            var nations = _uow.Context.Set<HU_NATION>().AsNoTracking().AsQueryable();
            var districts = _uow.Context.Set<HU_DISTRICT>().AsNoTracking().AsQueryable();
            var wards = _uow.Context.Set<HU_WARD>().AsNoTracking().AsQueryable();
            var joined = await (from p in entity
                                from e in employees.Where(x => x.ID == p.EMPLOYEE_ID).DefaultIfEmpty()
                                from t in positions.Where(x => x.ID == e.POSITION_ID).DefaultIfEmpty()
                                from o in organizations.Where(x => x.ID == e.ORG_ID).DefaultIfEmpty()
                                from r in otherLists.Where(x => x.ID == p.RELATIONSHIP_ID).DefaultIfEmpty()
                                from g in otherLists.Where(x => x.ID == p.GENDER).DefaultIfEmpty()
                                from n in nations.Where(x => x.ID == p.NATIONALITY).DefaultIfEmpty()
                                from pr in provinces.Where(x => x.ID == p.BIRTH_CER_PROVINCE).DefaultIfEmpty()
                                from d in districts.Where(x => x.ID == p.BIRTH_CER_DISTRICT).DefaultIfEmpty()
                                from w in wards.Where(x => x.ID == p.BIRTH_CER_WARD).DefaultIfEmpty()
                                where e.ID == employeeId
                                select new HuFamilyDTO
                                {
                                    Id = p.ID,
                                    EmployeeId = e.ID,
                                    Fullname = p.FULLNAME,
                                    RelationshipId = p.RELATIONSHIP_ID,
                                    RelationshipName = r.NAME,
                                    Gender = p.GENDER,
                                    GenderName = g.NAME,
                                    BirthDate = p.BIRTH_DATE,
                                    PitCode = p.PIT_CODE,
                                    SameCompany = p.SAME_COMPANY,
                                    IsDead = p.IS_DEAD,
                                    IsDeduct = p.IS_DEDUCT,
                                    DeductFrom = p.DEDUCT_FROM,
                                    DeductTo = p.DEDUCT_TO,
                                    RegistDeductDate = p.REGIST_DEDUCT_DATE,
                                    IsHousehold = p.IS_HOUSEHOLD,
                                    IdNo = p.ID_NO,
                                    Career = p.CAREER,
                                    Nationality = p.NATIONALITY,
                                    NationalityName = n.NAME,
                                    BirthCerPlace = p.BIRTH_CER_PLACE,
                                    BirthCerProvince = p.BIRTH_CER_PROVINCE,
                                    BirthCerProvinceName = pr.NAME,
                                    BirthCerDistrict = p.BIRTH_CER_DISTRICT,
                                    BirthCerDistrictName = d.NAME,
                                    BirthCerWard = p.BIRTH_CER_WARD,
                                    BirthCerWardName = w.NAME,
                                    Note = p.NOTE
                                }).ToListAsync();
            return new FormatedResponse() { InnerBody = joined };
        }

        public virtual async Task<FormatedResponse> ToggleActiveIds(GenericUnitOfWork _uow, List<long> ids, bool valueToBind, string sid)
        {
            await Task.Run(() => null);
            throw new NotImplementedException();
        }
        //public async Task<FormatedResponse> ApproveHuFamilyEdit(List<long> ids)
        //{
        //    string sid = "";
        //    bool patchMode = true;
        //    ids.ForEach(async id =>
        //    {
        //        var response = await _genericRepository.GetById(id);
        //        if(response.InnerBody == null)
        //        {
        //            HuFamilyEditDTO dto = (HuFamilyEditDTO)(response.InnerBody ?? throw new Exception());
        //            dto.IsApprovePortal = true;
        //            _genericRepository.Update(_uow, dto, sid, patchMode);
        //        }
        //    });
            
        //}

    }
}

