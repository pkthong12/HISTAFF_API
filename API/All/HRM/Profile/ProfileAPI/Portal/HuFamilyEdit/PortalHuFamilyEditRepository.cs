using CORE.GenericUOW;
using CORE.DTO;
using API.DTO;
using CORE.Enum;
using CORE.StaticConstant;
using API.All.DbContexts;
using CORE.AutoMapper;
using CORE.JsonHelper;
using System.Reflection;

namespace API.Controllers.HuFamilyEdit
{
    public class PortalHuFamilyEditRepository : IPortalHuFamilyEditRepository
    {
        private readonly GenericUnitOfWork _uow;
        private readonly FullDbContext _dbContext;
        private IGenericRepository<HU_FAMILY_EDIT, HuFamilyEditDTO> _genericRepository;
        private readonly GenericReducer<HU_FAMILY_EDIT, HuFamilyEditDTO> _genericReducer;

        public PortalHuFamilyEditRepository(FullDbContext context, GenericUnitOfWork uow)
        {
            _dbContext = context;
            _uow = uow;
            _genericRepository = _uow.GenericRepository<HU_FAMILY_EDIT, HuFamilyEditDTO>();
            _genericReducer = new();
        }

        public async Task<GenericPhaseTwoListResponse<HuFamilyEditDTO>> SinglePhaseQueryList(GenericQueryListDTO<HuFamilyEditDTO> request)
        {
            var joined = from p in _dbContext.HuFamilyEdits.AsNoTracking()
                         // JOIN OTHER ENTITIES BASED ON THE BUSINESS
                         select new HuFamilyEditDTO
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
                var list = new List<HU_FAMILY_EDIT>
                    {
                        (HU_FAMILY_EDIT)response
                    };
                var joined = (from l in list
                              // JOIN OTHER ENTITIES BASED ON THE BUSINESS
                              select new HuFamilyEditDTO
                              {
                                  Id = l.ID
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

        public async Task<FormatedResponse> Create(GenericUnitOfWork _uow, HuFamilyEditDTO dto, string sid)
        {
            var response = await _genericRepository.Create(_uow, dto, sid);
            return response;
        }

        public async Task<FormatedResponse> CreateRange(GenericUnitOfWork _uow, List<HuFamilyEditDTO> dtos, string sid)
        {
            var add = new List<HuFamilyEditDTO>();
            add.AddRange(dtos);
            var response = await _genericRepository.CreateRange(_uow, add, sid);
            return response;
        }

        public async Task<FormatedResponse> Update(GenericUnitOfWork _uow, HuFamilyEditDTO dto, string sid, bool patchMode = true)
        {
            var response = await _genericRepository.Update(_uow, dto, sid, patchMode);
            return response;
        }

        public async Task<FormatedResponse> UpdateRange(GenericUnitOfWork _uow, List<HuFamilyEditDTO> dtos, string sid, bool patchMode = true)
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

        public async Task<FormatedResponse> GetHuFamilyEditNotApproved(long employeeId)
        {
            var entity = _uow.Context.Set<HU_FAMILY_EDIT>().AsNoTracking().AsQueryable();
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
                                where e.ID == employeeId && p.IS_SEND_PORTAL == true && p.IS_APPROVE_PORTAL != true
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

        public async Task<FormatedResponse> GetHuFamilyEditSave(long employeeId)
        {
            var entity = _uow.Context.Set<HU_FAMILY_EDIT>().AsNoTracking().AsQueryable();
            var employees = _uow.Context.Set<HU_EMPLOYEE>().AsNoTracking().AsQueryable();
            var positions = _uow.Context.Set<HU_POSITION>().AsNoTracking().AsQueryable();
            var organizations = _uow.Context.Set<HU_ORGANIZATION>().AsNoTracking().AsQueryable();
            var otherLists = _uow.Context.Set<SYS_OTHER_LIST>().AsNoTracking().AsQueryable();
            var provinces = _uow.Context.Set<HU_PROVINCE>().AsNoTracking().AsQueryable();
            var nations = _uow.Context.Set<HU_NATION>().AsNoTracking().AsQueryable();
            var districts = _uow.Context.Set<HU_DISTRICT>().AsNoTracking().AsQueryable();
            var wards = _uow.Context.Set<HU_WARD>().AsNoTracking().AsQueryable();
            var joined = await(from p in entity
                               from e in employees.Where(x => x.ID == p.EMPLOYEE_ID).DefaultIfEmpty()
                               from t in positions.Where(x => x.ID == e.POSITION_ID).DefaultIfEmpty()
                               from o in organizations.Where(x => x.ID == e.ORG_ID).DefaultIfEmpty()
                               from r in otherLists.Where(x => x.ID == p.RELATIONSHIP_ID).DefaultIfEmpty()
                               from g in otherLists.Where(x => x.ID == p.GENDER).DefaultIfEmpty()
                               from n in nations.Where(x => x.ID == p.NATIONALITY).DefaultIfEmpty()
                               from pr in provinces.Where(x => x.ID == p.BIRTH_CER_PROVINCE).DefaultIfEmpty()
                               from d in districts.Where(x => x.ID == p.BIRTH_CER_DISTRICT).DefaultIfEmpty()
                               from w in wards.Where(x => x.ID == p.BIRTH_CER_WARD).DefaultIfEmpty()
                               where e.ID == employeeId && p.IS_SAVE_PORTAL == true
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


        //list HuFamily refuse
        public async Task<FormatedResponse> GetHuFamilyEditRefuse(long employeeId)
        {
            var entity = _uow.Context.Set<HU_FAMILY_EDIT>().AsNoTracking().AsQueryable();
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
                                where e.ID == employeeId && p.STATUS_ID == 995
                                select new 
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
                                    Note = p.NOTE,
                                    Reason = p.REASON,
                                }).ToListAsync();
            return new FormatedResponse() { InnerBody = joined };
        }
        public async Task<FormatedResponse> GetHuFamilyEditSaveById(long id)
        {
            try
            {
                var response = await (from f in _uow.Context.Set<HU_FAMILY_EDIT>().Where(x => x.ID == id)
                                      select new HuFamilyEditDTO
                                      {
                                          Id = f.ID,
                                          Fullname = f.FULLNAME,
                                          RelationshipId = f.RELATIONSHIP_ID,
                                          Gender = f.GENDER,
                                          BirthDate = f.BIRTH_DATE,
                                          IdNo = f.ID_NO,
                                          SameCompany = f.SAME_COMPANY,
                                          IsDead = f.IS_DEAD,
                                          IsDeduct = f.IS_DEDUCT,
                                          RegistDeductDate = f.REGIST_DEDUCT_DATE,
                                          DeductFrom = f.DEDUCT_FROM,
                                          DeductTo = f.DEDUCT_TO,
                                          IsHousehold = f.IS_HOUSEHOLD,
                                          PitCode = f.PIT_CODE,
                                          Career = f.CAREER,
                                          Nationality = f.NATIONALITY,
                                          BirthCerProvince = f.BIRTH_CER_PROVINCE,
                                          BirthCerDistrict = f.BIRTH_CER_DISTRICT,
                                          BirthCerWard = f.BIRTH_CER_WARD,
                                          Note = f.NOTE,
                                          EmployeeId = f.EMPLOYEE_ID,
                                          IsSavePortal = f.IS_SAVE_PORTAL,
                                          HuFamilyId = f.HU_FAMILY_ID,
                                      }).SingleAsync();
                return new FormatedResponse() { InnerBody = response };
            }
            catch (Exception ex)
            {

                return new FormatedResponse() { StatusCode = EnumStatusCode.StatusCode500, ErrorType = EnumErrorType.UNCATCHABLE, MessageCode = ex.Message };
            }
        }
        
        public async Task<FormatedResponse> GetHuFamilyEditByIdCorrect(long id)
        {
            try
            {
                var response = await (from f in _uow.Context.Set<HU_FAMILY_EDIT>().Where(x => x.ID == id)
                                      from e in _dbContext.HuEmployees.Where(x => x.ID == f.EMPLOYEE_ID).DefaultIfEmpty()
                                      from t in _dbContext.HuPositions.Where(x => x.ID == e.POSITION_ID).DefaultIfEmpty()
                                      from o in _dbContext.HuOrganizations.Where(x => x.ID == e.ORG_ID).DefaultIfEmpty()
                                      from r in _dbContext.SysOtherLists.Where(x => x.ID == f.RELATIONSHIP_ID).DefaultIfEmpty()
                                      from g in _dbContext.SysOtherLists.Where(x => x.ID == f.GENDER).DefaultIfEmpty()
                                      from n in _dbContext.HuNations.Where(x => x.ID == f.NATIONALITY).DefaultIfEmpty()
                                      from pr in _dbContext.HuProvinces.Where(x => x.ID == f.BIRTH_CER_PROVINCE).DefaultIfEmpty()
                                      from d in _dbContext.HuDistricts.Where(x => x.ID == f.BIRTH_CER_DISTRICT).DefaultIfEmpty()
                                      from w in _dbContext.HuWards.Where(x => x.ID == f.BIRTH_CER_WARD).DefaultIfEmpty()
                                      
                                      select new HuFamilyEditDTO
                                      {
                                          Id = f.ID,
                                          Fullname = f.FULLNAME,
                                          RelationshipName = r.NAME,
                                          GenderName = g.NAME,
                                          BirthDate = f.BIRTH_DATE,
                                          IdNo = f.ID_NO,
                                          SameCompany = f.SAME_COMPANY,
                                          IsDead = f.IS_DEAD,
                                          IsDeduct = f.IS_DEDUCT,
                                          RegistDeductDate = f.REGIST_DEDUCT_DATE,
                                          DeductFrom = f.DEDUCT_FROM,
                                          DeductTo = f.DEDUCT_TO,
                                          IsHousehold = f.IS_HOUSEHOLD,
                                          PitCode = f.PIT_CODE,
                                          Career = f.CAREER,
                                          NationalityName = n.NAME,
                                          BirthCerProvinceName = pr.NAME,
                                          BirthCerDistrictName = d.NAME,
                                          BirthCerWardName = w.NAME,
                                          Note = f.NOTE,
                                          EmployeeId = f.EMPLOYEE_ID,
                                          IsSavePortal = f.IS_SAVE_PORTAL,
                                      }).ToListAsync();
                return new FormatedResponse() { InnerBody = response };
            }
            catch (Exception ex)
            {

                return new FormatedResponse() { StatusCode = EnumStatusCode.StatusCode500, ErrorType = EnumErrorType.UNCATCHABLE, MessageCode = ex.Message };
            }
        }

        public async Task<FormatedResponse> InsertHuFamilyEdit(HuFamilyEditDTO request)
        {
            try
            {
                string sid = "";
                bool pathMode = true;
                var getOtherList = (from t in _uow.Context.Set<SYS_OTHER_LIST_TYPE>().Where(x => x.CODE == "STATUS")
                                    from o in _uow.Context.Set<SYS_OTHER_LIST>().Where(x => x.CODE == "CD")
                                    select new { Id = o.ID }).FirstOrDefault();

                if (request.Id != null && request.IsSavePortal == true)
                {
                    request.IsSavePortal = false;
                    request.IsApprovePortal = false;
                    request.StatusId = getOtherList?.Id;
                    request.IsSendPortal = true;
                    if (request.HuFamilyId != null)
                    {
                        var familySave = _uow.Context.Set<HU_FAMILY>().Where(x => x.ID == request.HuFamilyId).FirstOrDefault();
                        List<string> listModelSaveChange = new List<string>();
                        if (familySave != null)
                        {
                            var entityType = typeof(HU_FAMILY);
                            var dtoType = typeof(HuFamilyDTO);
                            var entityPropList = entityType.GetProperties().ToList();
                            var dtoPropList = dtoType.GetProperties().ToList();

                            var query = Activator.CreateInstance(dtoType);


                            entityPropList.ForEach(prop =>
                            {
                                var value = prop.GetValue(familySave);
                                var dtoProp = dtoPropList.SingleOrDefault(x => x.Name == prop.Name.SnakeToCamelCase().CamelToPascalCase());
                                dtoProp?.SetValue(query, value);

                            });


                            if (query != null)
                            {
                                Type type = query.GetType();
                                Type type2 = request.GetType();
                                IList<PropertyInfo> props = new List<PropertyInfo>(type.GetProperties());
                                IList<PropertyInfo> prop2s = new List<PropertyInfo>(type2.GetProperties());
                                foreach (PropertyInfo prop in props)
                                {
                                    foreach (PropertyInfo prop2 in prop2s)
                                    {

                                        if (prop.Name != "Id" && prop.Name != null && prop.Name == prop2.Name && prop.GetValue(query) != null && prop2.GetValue(request) != null && prop.GetValue(query)!.ToString() != prop2.GetValue(request)!.ToString())
                                        {
                                            listModelSaveChange.Add(Char.ToLowerInvariant(prop.Name[0]) + prop.Name.Substring(1));
                                        }

                                    }
                                }
                                request.ModelChange = string.Join(";", listModelSaveChange);
                            }
                        }
                    }
                    else
                    {
                        string[] arrModelChanges = {"relationshipId","fullname","gender","birthDate","pitCode",
                                                "deductFrom","deductTo","registerDeductDate","idNo","carrer",
                                                "nationality","birthCerProvince","birthCerDistrict","birthCerWard","note"};
                        request.ModelChange = string.Join(";", arrModelChanges);
                    }
                    var updateResponse = await _genericRepository.Update(_uow, request, sid, pathMode);
                    return updateResponse;
                }
                if (request.Id != null && request.IsSavePortal == false)
                {
                    request.IsApprovePortal = false;
                    request.StatusId = getOtherList?.Id;
                    request.IsSendPortal = true;
                    var updateResponse = await _genericRepository.Update(_uow, request, sid, pathMode);
                    var family2 = _uow.Context.Set<HU_FAMILY>().Where(x => x.ID == request.HuFamilyId).FirstOrDefault();
                    List<string> listModelChange2 = new List<string>();
                    if (family2 != null)
                    {
                        var entityType = typeof(HU_FAMILY);
                        var dtoType = typeof(HuFamilyDTO);
                        var entityPropList = entityType.GetProperties().ToList();
                        var dtoPropList = dtoType.GetProperties().ToList();

                        var query = Activator.CreateInstance(dtoType);


                        entityPropList.ForEach(prop =>
                        {
                            var value = prop.GetValue(family2);
                            var dtoProp = dtoPropList.SingleOrDefault(x => x.Name == prop.Name.SnakeToCamelCase().CamelToPascalCase());
                            dtoProp?.SetValue(query, value);

                        });


                        if (query != null)
                        {
                            Type type = query.GetType();
                            Type type2 = request.GetType();
                            IList<PropertyInfo> props = new List<PropertyInfo>(type.GetProperties());
                            IList<PropertyInfo> prop2s = new List<PropertyInfo>(type2.GetProperties());
                            foreach (PropertyInfo prop in props)
                            {
                                foreach (PropertyInfo prop2 in prop2s)
                                {

                                    if (prop.Name != "Id" && prop.Name != null && prop.Name == prop2.Name && prop.GetValue(query) != null && prop2.GetValue(request) != null && prop.GetValue(query)!.ToString() != prop2.GetValue(request)!.ToString())
                                    {
                                        listModelChange2.Add(Char.ToLowerInvariant(prop.Name[0]) + prop.Name.Substring(1));
                                    }

                                }
                            }
                            request.ModelChange = string.Join(";", listModelChange2);
                        }
                    }
                    return updateResponse;
                }
                var getData = _uow.Context.Set<HU_FAMILY_EDIT>().Where(x => x.EMPLOYEE_ID == request.EmployeeId && x.IS_APPROVE_PORTAL == false && x.IS_SEND_PORTAL == true);
                if (getData.Any())
                {
                    return new FormatedResponse() { StatusCode = EnumStatusCode.StatusCode400, MessageCode = CommonMessageCode.HAD_RECORD_IS_APPROVING };
                }
                if (request.Id == 0)
                {
                    request.IsApprovePortal = false;
                    request.StatusId = getOtherList?.Id;
                    request.IsSendPortal = true;
                    request.IsSavePortal = false;
                    string[] arrModelChanges = {"relationshipId","fullname","gender","birthDate","pitCode",
                                                "deductFrom","deductTo","registerDeductDate","idNo","carrer",
                                                "nationality","birthCerProvince","birthCerDistrict","birthCerWard","note"};
                    request.ModelChange = string.Join(";", arrModelChanges);
                    var createResponse = await _genericRepository.Create(_uow, request, sid);
                    return createResponse;
                }

                request.IsSendPortal = true;
                request.IsSavePortal = false;
                request.IsApprovePortal = false;
                request.HuFamilyId = request.Id;
                request.Id = null;
                var family = _uow.Context.Set<HU_FAMILY>().Where(x => x.ID == request.HuFamilyId).FirstOrDefault();
                List<string> listModelChange = new List<string>();
                if (family != null)
                {
                    var entityType = typeof(HU_FAMILY);
                    var dtoType = typeof(HuFamilyDTO);
                    var entityPropList = entityType.GetProperties().ToList();
                    var dtoPropList = dtoType.GetProperties().ToList();

                    var query = Activator.CreateInstance(dtoType);


                    entityPropList.ForEach(prop =>
                    {
                        var value = prop.GetValue(family);
                        var dtoProp = dtoPropList.SingleOrDefault(x => x.Name == prop.Name.SnakeToCamelCase().CamelToPascalCase());
                        dtoProp?.SetValue(query, value);

                    });


                    if (query != null)
                    {
                        Type type = query.GetType();
                        Type type2 = request.GetType();
                        IList<PropertyInfo> props = new List<PropertyInfo>(type.GetProperties());
                        IList<PropertyInfo> prop2s = new List<PropertyInfo>(type2.GetProperties());
                        foreach (PropertyInfo prop in props)
                        {
                            foreach (PropertyInfo prop2 in prop2s)
                            {

                                if (prop.Name != "Id" && prop.Name != null && prop.Name == prop2.Name && prop.GetValue(query) != null && prop2.GetValue(request) != null && prop.GetValue(query)!.ToString() != prop2.GetValue(request)!.ToString())
                                {
                                    listModelChange.Add(Char.ToLowerInvariant(prop.Name[0]) + prop.Name.Substring(1));
                                }

                            }
                        }
                        request.ModelChange = string.Join(";", listModelChange);
                    }
                }
                var response = await _genericRepository.Create(_uow, request,sid);
                return response;
            }
            catch (Exception ex)
            {

                return new FormatedResponse() { ErrorType = EnumErrorType.UNCATCHABLE, StatusCode = EnumStatusCode.StatusCode500, MessageCode = ex.Message };
            }
        }
    }
}

