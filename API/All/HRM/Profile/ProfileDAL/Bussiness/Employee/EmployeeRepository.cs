using Common.Paging;
using ProfileDAL.ViewModels;
using Common.Repositories;
using Common.Extensions;
using Common.EPPlus;
using Common.BaseRequest;
using API.All.DbContexts;
using API.Entities;
using CORE.GenericUOW;
using CORE.DTO;
using CORE.Enum;
using CORE.StaticConstant;
using API.All.SYSTEM.CoreAPI.OrtherList;
using API.DTO;
using Azure;

namespace ProfileDAL.Repositories
{
    public class EmployeeRepository : RepositoryBase<HU_EMPLOYEE>, IEmployeeRepository
    {
        private ProfileDbContext _appContext => (ProfileDbContext)_context;
        private readonly GenericReducer<HU_EMPLOYEE, HuEmployeeDTO> genericReducer;
        public EmployeeRepository(ProfileDbContext context) : base(context)
        {
            genericReducer = new();
        }

        // TwoPhaseQueryList method is depricated
        /*
        public async Task<GenericPhaseTwoListResponse<EmployeeDTO>> TwoPhaseQueryList(GenericQueryListDTO<EmployeeDTO> request)
        {
            var raw = _appContext.Employees.AsQueryable();
            var phase1 = await genericReducer.FirstPhaseReduce(raw, request);

            if (phase1.ErrorType != EnumErrorType.NONE)
            {
                return new()
                {
                    ErrorType = phase1.ErrorType,
                    MessageCode = phase1.MessageCode,
                    ErrorPhase = 1
                };
            }

            if (phase1.Queryable == null)
            {
                return new()
                {
                    ErrorType = EnumErrorType.CATCHABLE,
                    MessageCode = CommonMessageCode.ENTITIES_NOT_FOUND,
                    ErrorPhase = 1
                };
            }

            var phase1IdsResult = phase1.Queryable.ToList().Aggregate("", (prev, curr) => prev + curr.ID.ToString() + ";");
            var ids = phase1IdsResult.Split(';');

            var joined = from p in _appContext.Employees.Where(p => ids.Contains(p.ID.ToString()))

                         from o in _appContext.Organizations.Where(c => c.ID == p.ORG_ID).DefaultIfEmpty()
                         from t in _appContext.Positions.Where(c => c.ID == p.POSITION_ID).DefaultIfEmpty()
                         from g in _appContext.OtherLists.Where(c => c.ID == p.Profile.GENDER_ID).DefaultIfEmpty()
                         from w in _appContext.Wards.Where(c => c.ID == p.Profile.CUR_WARD_ID).DefaultIfEmpty()
                         from d in _appContext.Districts.Where(c => c.ID == p.Profile.CUR_DISTRICT_ID).DefaultIfEmpty()
                         from pr in _appContext.Provinces.Where(c => c.ID == p.Profile.CUR_PROVINCE_ID).DefaultIfEmpty()
                         from n in _appContext.OtherLists.Where(c => c.ID == p.Profile.NATIVE_ID).DefaultIfEmpty()
                         from s in _appContext.OtherListFixs.Where(c => c.ID == p.WORK_STATUS_ID && c.TYPE == SystemConfig.STATUS_EMPLOYEE).DefaultIfEmpty()
                         from c in _appContext.Contracts.Where(c => c.ID == p.CONTRACT_ID).DefaultIfEmpty()
                         select new EmployeeDTO
                         {
                             Id = p.ID,
                             Code = p.CODE,
                             Fullname = p.Profile.FULL_NAME,
                             Image = p.Profile.IMAGE,
                             OrgId = p.ORG_ID,
                             OrgName = o.NAME,
                             PositionName = t.NAME,
                             GenderName = g.NAME,
                             BirthDate = p.Profile.BIRTH_DATE,
                             IdNo = p.Profile.ID_NO,
                             IdDate = p.Profile.ID_DATE,
                             IdPlace = p.Profile.ID_PLACE,
                             NativeName = n.NAME,
                             Address = p.Profile.CUR_ADDRESS,
                             BirthPlace = p.Profile.BIRTH_PLACE,
                             JoinDate = p.JOIN_DATE,
                             WorkStatusId = p.WORK_STATUS_ID,
                             WorkStatusName = s.NAME,
                             ProvinceName = pr.NAME,
                             DistrictName = d.NAME,
                             WardName = w.NAME,
                             TerEffectDate = p.TER_EFFECT_DATE,
                             MobilePhone = p.Profile.MOBILE_PHONE,
                             Avatar = p.Profile.AVATAR,
                             ContractExpired = p.CONTRACT_EXPIRED,
                             Email = p.Profile.WORK_EMAIL,
                             ItimeCode = p.ITIME_CODE,
                             TaxCode = p.Profile.TAX_CODE,
                             ContractNo = c.CONTRACT_NO,
                             DateStart = c.START_DATE,
                             DateEnd = c.EXPIRE_DATE
                         };
            var phase2 = await genericReducer.SecondPhaseReduce(joined, request);
            return phase2;
        }
        */

        public async Task<GenericPhaseTwoListResponse<HuEmployeeDTO>> SinglePhaseQueryList(GenericQueryListDTO<HuEmployeeDTO> request)
        {

            if (request.InOperators == null)
            {
                return new()
                {
                    ErrorType = EnumErrorType.CATCHABLE,
                    MessageCode = CommonMessageCode.THIS_QUERY_REQUIRES_IN_OPERATOR,
                    ErrorPhase = 0
                };
            }

            if (!request.InOperators.Any(x => x.Field.ToLower() == "orgid"))
            {
                return new()
                {
                    ErrorType = EnumErrorType.CATCHABLE,
                    MessageCode = CommonMessageCode.THIS_QUERY_REQUIRES_ORG_ID_IN_OPERATOR,
                    ErrorPhase = 0
                };
            }

            var joined = from p in _appContext.Employees.AsNoTracking()
                         from t in _appContext.Positions.AsNoTracking().Where(c => c.ID == p.POSITION_ID).DefaultIfEmpty()
                         from sysOff in _appContext.OtherLists.AsNoTracking().Where(c => c.NAME == "ESQ").DefaultIfEmpty()
                         from nddv in _appContext.Positions.AsNoTracking().Where(c => c.ID == t.ID && t.NAME == "Người đại diện vốn" && p.WORK_STATUS_ID != sysOff.ID).DefaultIfEmpty()
                         from o in _appContext.Organizations.AsNoTracking().Where(c => c.ID == t.ORG_ID).DefaultIfEmpty()
                         from g in _appContext.OtherLists.AsNoTracking().Where(c => c.ID == p.Profile!.GENDER_ID).DefaultIfEmpty()
                         from w in _appContext.Wards.AsNoTracking().Where(c => c.ID == p.Profile!.CUR_WARD_ID).DefaultIfEmpty()
                         from d in _appContext.Districts.AsNoTracking().Where(c => c.ID == p.Profile!.CUR_DISTRICT_ID).DefaultIfEmpty()
                         from pr in _appContext.Provinces.AsNoTracking().Where(c => c.ID == p.Profile!.CUR_PROVINCE_ID).DefaultIfEmpty()
                         from idp in _appContext.Provinces.AsNoTracking().Where(c => c.ID == p.Profile!.ID_PLACE).DefaultIfEmpty()
                         from n in _appContext.OtherLists.AsNoTracking().Where(c => c.ID == p.Profile!.NATIVE_ID).DefaultIfEmpty()
                         from q in _appContext.OtherLists.AsNoTracking().Where(c => c.ID == p.Profile!.GOVERMENT_MANAGEMENT_ID).DefaultIfEmpty()
                         from v in _appContext.OtherLists.AsNoTracking().Where(c => c.ID == p.WORK_STATUS_ID).DefaultIfEmpty()
                         from s in _appContext.OtherListFixs.AsNoTracking().Where(c => c.ID == p.WORK_STATUS_ID && c.TYPE == SystemConfig.STATUS_EMPLOYEE).DefaultIfEmpty()
                         from c in _appContext.Contracts.Where(c => c.ID == p.CONTRACT_ID).DefaultIfEmpty()
                         from com in _appContext.CompanyInfos.Where(c => c.ID == o.COMPANY_ID).DefaultIfEmpty()
                         from re in _appContext.OtherLists.Where(x => x.ID == com.REGION_ID).DefaultIfEmpty()
                         from j in _appContext.HUJobs.Where(x => x.ID == t.JOB_ID).DefaultIfEmpty()
                         orderby j.ORDERNUM
                         select new HuEmployeeDTO
                         {
                             Id = p.ID,
                             Code = p.CODE,
                             ProfileCode = p.PROFILE_CODE,
                             Fullname = p.Profile!.FULL_NAME,
                             FullNameOnConcurrently = p.CODE + " - " + p.Profile!.FULL_NAME,
                             Image = p.Profile!.IMAGE,
                             OrgId = p.ORG_ID,
                             OrgName = o.NAME,
                             PositionName = t.NAME,
                             JobOrderNum = Convert.ToInt32(j.ORDERNUM ?? 999),
                             PositionNameOnConcurrently = t.NAME,
                             GenderName = g.NAME,
                             BirthDate = p.Profile!.BIRTH_DATE,
                             IdNo = p.Profile!.ID_NO,
                             IdDate = p.Profile!.ID_DATE,
                             IdPlace = p.Profile!.ID_PLACE,
                             NativeName = n.NAME,
                             Address = p.Profile!.CUR_ADDRESS,
                             BirthPlace = p.Profile!.BIRTH_PLACE,
                             JoinDate = p.JOIN_DATE,
                             WorkStatusId = v.ID,
                             WorkStatusName = v.NAME,
                             ProvinceName = pr.NAME,
                             DistrictName = d.NAME,
                             WardName = w.NAME,
                             TerEffectDate = p.TER_EFFECT_DATE,
                             MobilePhone = p.Profile!.MOBILE_PHONE,
                             Avatar = p.Profile!.AVATAR,
                             ContractExpired = p.CONTRACT_EXPIRED,
                             Email = p.Profile!.WORK_EMAIL,
                             ItimeId = p.ITIME_ID,
                             TaxCode = p.Profile!.TAX_CODE,
                             ContractNo = c.CONTRACT_NO,
                             DateStart = c.START_DATE,
                             DateEnd = c.EXPIRE_DATE,
                             ReligionName = re.NAME,
                             ReligionId = re.ID,
                             MemberPosition = p.Profile!.MEMBER_POSITION,
                             LivingCell = p.Profile!.LIVING_CELL,
                             GovernmentManagement = q.NAME,
                             YellowFlag = p.Profile!.YELLOW_FLAG,
                             Relations = p.Profile!.RELATIONS,
                             UnionistPosition = p.Profile!.UNIONIST_POSITION,
                             UnionistDate = p.Profile!.UNIONIST_DATE,
                             UnionistAddress = p.Profile!.UNIONIST_ADDRESS,
                             YouthSaveNationDate = p.Profile!.YOUTH_SAVE_NATION_DATE,
                             YouthSaveNationPosition = p.Profile!.YOUTH_SAVE_NATION_POSITION,
                             License = p.Profile!.LICENSE,
                             Presenter = p.Profile!.PRESENTER,
                             PresenterPhoneNumber = p.Profile!.PRESENTER_PHONE_NUMBER,
                             PresenterAddress = p.Profile!.PRESENTER_ADDRESS,
                             LandlinePhone = p.Profile!.MOBILE_PHONE_LAND,
                             Company = request.Lang == "vi" ? com.NAME_VN : (request.Lang == "en" ? com.NAME_EN : "Unsupported language yet"),
                             NameOnProfileEmployee = j.NAME_VN,
                             AddressIdentity = idp.NAME,
                             IsMember = p.Profile!.IS_MEMBER, // lấy ra "là Đảng viên"
                             AddressReffererEmployee = p.Profile!.ADDRESS, // lấy thông tin người giới thiệu phải là thường trú
                             IsLeaveWork = v.CODE == "ESQ" ? true : false,  // lọc nhân viên có trạng thái nghỉ việc
                             IsRepresentative = nddv != null ? true : false // lấy ra trạng thái có name position là Người đại diện vốn
                         };

            var singlePhaseResult = await genericReducer.SinglePhaseReduce(joined, request);
            return singlePhaseResult;
        }

        public async Task<GenericPhaseTwoListResponse<HuEmployeeDTO>> QueryListEmp(GenericQueryListDTO<HuEmployeeDTO> request)
        {

            if (request.InOperators == null)
            {
                return new()
                {
                    ErrorType = EnumErrorType.CATCHABLE,
                    MessageCode = CommonMessageCode.THIS_QUERY_REQUIRES_IN_OPERATOR,
                    ErrorPhase = 0
                };
            }

            if (!request.InOperators.Any(x => x.Field.ToLower() == "orgid"))
            {
                return new()
                {
                    ErrorType = EnumErrorType.CATCHABLE,
                    MessageCode = CommonMessageCode.THIS_QUERY_REQUIRES_ORG_ID_IN_OPERATOR,
                    ErrorPhase = 0
                };
            }
            var _document = _appContext.SeDocuments.Where(x => x.IS_PERMISSVE_UPLOAD == true).Count();//sl tl phai upload
            var joined = from p in _appContext.Employees.AsNoTracking()
                         from t in _appContext.Positions.AsNoTracking().Where(c => c.ID == p.POSITION_ID).DefaultIfEmpty()
                         from o in _appContext.Organizations.AsNoTracking().Where(c => c.ID == t.ORG_ID).DefaultIfEmpty()
                         from sysOff in _appContext.OtherLists.AsNoTracking().Where(c => c.CODE == "ESQ").DefaultIfEmpty()
                         from v in _appContext.OtherLists.AsNoTracking().Where(c => c.ID == p.WORK_STATUS_ID).DefaultIfEmpty()
                         from s in _appContext.OtherListFixs.AsNoTracking().Where(c => c.ID == p.WORK_STATUS_ID && c.TYPE == SystemConfig.STATUS_EMPLOYEE).DefaultIfEmpty()
                         select new HuEmployeeDTO
                         {
                             Id = p.ID,
                             Code = p.CODE,
                             Fullname = p.Profile!.FULL_NAME,
                             OrgId = p.ORG_ID,
                             OrgName = o.NAME,
                             PositionName = t.NAME,
                             StatusDocument = (_appContext.SeDocumentInfos.Where(x => x.EMP_ID == p.ID).Count() == _document ? "Hoàn thành" : "Chưa hoàn thành"),
                             IsLeaveWork = v.CODE == "ESQ" ? true : false,  // lọc nhân viên có trạng thái nghỉ việc
                             WorkStatusId = v.ID,
                             WorkStatusName = v.NAME,
                         };

            var singlePhaseResult = await genericReducer.SinglePhaseReduce(joined, request);
            return singlePhaseResult;
        }


        public async Task<PagedResult<EmployeeDTO>> GetAll(EmployeeDTO param)
        {

            var queryable = from p in _appContext.Employees
                            from o in _appContext.Organizations.Where(c => c.ID == p.ORG_ID).DefaultIfEmpty()
                            from t in _appContext.Positions.Where(c => c.ID == p.POSITION_ID).DefaultIfEmpty()
                            from g in _appContext.OtherLists.Where(c => c.ID == p.Profile.GENDER_ID).DefaultIfEmpty()
                            from w in _appContext.Wards.Where(c => c.ID == p.Profile.CUR_WARD_ID).DefaultIfEmpty()
                            from d in _appContext.Districts.Where(c => c.ID == p.Profile.CUR_DISTRICT_ID).DefaultIfEmpty()
                            from pr in _appContext.Provinces.Where(c => c.ID == p.Profile.CUR_PROVINCE_ID).DefaultIfEmpty()
                            from n in _appContext.OtherLists.Where(c => c.ID == p.Profile.NATIVE_ID).DefaultIfEmpty()
                            from s in _appContext.OtherListFixs.Where(c => c.ID == p.WORK_STATUS_ID && c.TYPE == SystemConfig.STATUS_EMPLOYEE).DefaultIfEmpty()

                            orderby p.CODE
                            select new EmployeeDTO
                            {
                                Id = p.ID,
                                Code = p.CODE,
                                Fullname = p.Profile.FULL_NAME,
                                Image = p.Profile.IMAGE,
                                OrgId = p.ORG_ID,
                                OrgName = o.NAME,
                                PositionName = t.NAME,
                                GenderName = g.NAME,
                                BirthDate = p.Profile.BIRTH_DATE,
                                IdNo = p.Profile.ID_NO,
                                IdDate = p.Profile.ID_DATE,
                                IdPlace = p.Profile.ID_PLACE,
                                NativeName = n.NAME,
                                Address = p.Profile.CUR_ADDRESS,
                                BirthPlace = p.Profile.BIRTH_PLACE,
                                JoinDate = p.JOIN_DATE,
                                WorkStatusId = p.WORK_STATUS_ID,
                                WorkStatusName = s.NAME,
                                ProvinceName = pr.NAME,
                                DistrictName = d.NAME,
                                WardName = w.NAME,
                                TerEffectDate = p.TER_EFFECT_DATE,
                                MobilePhone = p.Profile.MOBILE_PHONE,
                                Avatar = p.Profile.AVATAR,
                                ContractExpired = p.CONTRACT_EXPIRED,
                                Email = p.Profile.WORK_EMAIL,
                                ItimeId = p.ITIME_ID,
                                TaxCode = p.Profile.TAX_CODE
                            };
            //check phan quyen theo phong ban
            //param.OrgId = param.OrgId != null ? param.OrgId : 1;

            var orgIds = await QueryData.ExecuteList(Procedures.PKG_COMMON_LIST_ORG,
                     new
                     {
                         P_IS_ADMIN = _appContext.IsAdmin == true ? 1 : 0,
                         P_ORG_ID = param.OrgId,
                         P_CURENT_USER_ID = _appContext.CurrentUserId,
                         //P_CUR = QueryData.OUT_CURSOR
                     }, false);


            List<long?> ids = orgIds.Select(c => (long?)((dynamic)c).ID).ToList();
            if (param.OrgId != null)
            {
                ids.Add(param.OrgId);
            }
            queryable = queryable.Where(p => ids.Contains(p.OrgId));


            if (!string.IsNullOrWhiteSpace(param.Fullname))
            {
                queryable = queryable.Where(p => p.Fullname.ToUpper().Contains(param.Fullname.ToUpper()));
            }
            if (!string.IsNullOrWhiteSpace(param.TaxCode))
            {
                queryable = queryable.Where(p => p.TaxCode.ToUpper().Contains(param.TaxCode.ToUpper()));
            }
            if (!string.IsNullOrWhiteSpace(param.Code))
            {
                queryable = queryable.Where(p => p.Code.ToUpper().Contains(param.Code.ToUpper()));
            }

            if (!string.IsNullOrWhiteSpace(param.OrgName))
            {
                queryable = queryable.Where(p => p.OrgName.ToUpper().Contains(param.OrgName.ToUpper()));
            }

            if (!string.IsNullOrWhiteSpace(param.PositionName))
            {
                queryable = queryable.Where(p => p.PositionName.ToUpper().Contains(param.PositionName.ToUpper()));
            }
            if (!string.IsNullOrWhiteSpace(param.IdNo))
            {
                queryable = queryable.Where(p => p.IdNo.ToUpper().Contains(param.IdNo.ToUpper()));
            }
            if (!string.IsNullOrWhiteSpace(param.BirthPlace))
            {
                queryable = queryable.Where(p => p.BirthPlace.ToUpper().Contains(param.BirthPlace.ToUpper()));
            }
            if (!string.IsNullOrWhiteSpace(param.GenderName))
            {
                queryable = queryable.Where(p => p.GenderName.ToUpper().Contains(param.GenderName.ToUpper()));
            }
            if (!string.IsNullOrWhiteSpace(param.NativeName))
            {
                queryable = queryable.Where(p => p.NativeName.ToUpper().Contains(param.NativeName.ToUpper()));
            }
            if (!string.IsNullOrWhiteSpace(param.ProvinceName))
            {
                queryable = queryable.Where(p => p.ProvinceName.ToUpper().Contains(param.ProvinceName.ToUpper()));
            }

            if (!string.IsNullOrWhiteSpace(param.DistrictName))
            {
                queryable = queryable.Where(p => p.DistrictName.ToUpper().Contains(param.DistrictName.ToUpper()));
            }

            if (!string.IsNullOrWhiteSpace(param.WardName))
            {
                queryable = queryable.Where(p => p.WardName.ToUpper().Contains(param.WardName.ToUpper()));
            }
            /*
            if (!string.IsNullOrWhiteSpace(param.JoinDate))
            {
                queryable = queryable.Where(p => p.JoinDate.Contains(param.JoinDate));
            }
            if (!string.IsNullOrWhiteSpace(param.IdDate))
            {
                queryable = queryable.Where(p => p.IdDate.Contains(param.IdDate));
            }
            */
            if (param.WorkStatusId == null || param.WorkStatusId == 0)
            {
                queryable = queryable.Where(p => p.WorkStatusId != OtherConfig.EMP_STATUS_TERMINATE || (p.WorkStatusId == OtherConfig.EMP_STATUS_TERMINATE && p.TerEffectDate > DateTime.Now));
            }

            //if (param.GenderId != null)
            //{
            //    queryable = queryable.Where(p => p.GenderId == param.GenderId);
            //}

            return await PagingList(queryable, param);
        }

        /// <summary>
        /// CMS Get All Data
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<PagedResult<EmployeeDTO>> GetEmployeeList(EmployeeDTO param)
        {

            var queryable = from p in _appContext.Employees
                            from o in _appContext.Organizations.Where(c => c.ID == p.ORG_ID).DefaultIfEmpty()
                            from t in _appContext.Positions.Where(c => c.ID == p.POSITION_ID).DefaultIfEmpty()
                            from g in _appContext.OtherLists.Where(c => c.ID == p.Profile.GENDER_ID).DefaultIfEmpty()
                            from w in _appContext.Wards.Where(c => c.ID == p.Profile.CUR_WARD_ID).DefaultIfEmpty()
                            from d in _appContext.Districts.Where(c => c.ID == p.Profile.CUR_DISTRICT_ID).DefaultIfEmpty()
                            from pr in _appContext.Provinces.Where(c => c.ID == p.Profile.CUR_PROVINCE_ID).DefaultIfEmpty()
                            from n in _appContext.OtherLists.Where(c => c.ID == p.Profile.NATIVE_ID).DefaultIfEmpty()
                            from s in _appContext.OtherListFixs.Where(c => c.ID == p.WORK_STATUS_ID && c.TYPE == SystemConfig.STATUS_EMPLOYEE).DefaultIfEmpty()
                                // where p.FULLNAME == param.Keyword
                            orderby p.CODE
                            select new EmployeeDTO
                            {
                                Id = p.ID,
                                Code = p.CODE,
                                Fullname = p.Profile.FULL_NAME,
                                Image = p.Profile.IMAGE,
                                OrgId = p.ORG_ID,
                                OrgName = o.NAME,
                                PositionName = t.NAME,
                                GenderName = g.NAME,
                                BirthDate = p.Profile.BIRTH_DATE,
                                IdNo = p.Profile.ID_NO,
                                IdDate = p.Profile.ID_DATE,
                                IdPlace = p.Profile.ID_PLACE,
                                NativeName = n.NAME,
                                Address = p.Profile.CUR_ADDRESS,
                                BirthPlace = p.Profile.BIRTH_PLACE,
                                JoinDate = p.JOIN_DATE,
                                WorkStatusId = p.WORK_STATUS_ID,
                                WorkStatusName = s.NAME,
                                ProvinceName = pr.NAME,
                                DistrictName = d.NAME,
                                WardName = w.NAME,
                                TerEffectDate = p.TER_EFFECT_DATE,
                                MobilePhone = p.Profile.MOBILE_PHONE,
                                Avatar = p.Profile.AVATAR,
                                ContractExpired = p.CONTRACT_EXPIRED,
                                Email = p.Profile.WORK_EMAIL,
                                ItimeId = p.ITIME_ID,
                                TaxCode = p.Profile.TAX_CODE
                            };
            //check phan quyen theo phong ban
            //param.OrgId = param.OrgId != null ? param.OrgId : 1;

            var orgIds = await QueryData.ExecuteList(Procedures.PKG_COMMON_LIST_ORG,
                     new
                     {
                         P_IS_ADMIN = _appContext.IsAdmin == true ? 1 : 0,

                         P_ORG_ID = param.OrgId,
                         P_CURENT_USER_ID = _appContext.CurrentUserId /*,
                         P_CUR = QueryData.OUT_CURSOR */
                     }, false);


            List<long?> ids = orgIds.Select(c => (long?)((dynamic)c).ID).ToList();
            if (param.OrgId != null)
            {
                ids.Add(param.OrgId);
            }
            queryable = queryable.Where(p => ids.Contains(p.OrgId));


            if (!string.IsNullOrWhiteSpace(param.Fullname))
            {
                queryable = queryable.Where(p => p.Fullname.ToUpper().Contains(param.Fullname.ToUpper()));
            }
            if (!string.IsNullOrWhiteSpace(param.TaxCode))
            {
                queryable = queryable.Where(p => p.TaxCode.ToUpper().Contains(param.TaxCode.ToUpper()));
            }
            if (!string.IsNullOrWhiteSpace(param.Code))
            {
                queryable = queryable.Where(p => p.Code.ToUpper().Contains(param.Code.ToUpper()));
            }

            if (!string.IsNullOrWhiteSpace(param.OrgName))
            {
                queryable = queryable.Where(p => p.OrgName.ToUpper().Contains(param.OrgName.ToUpper()));
            }

            if (!string.IsNullOrWhiteSpace(param.PositionName))
            {
                queryable = queryable.Where(p => p.PositionName.ToUpper().Contains(param.PositionName.ToUpper()));
            }
            if (!string.IsNullOrWhiteSpace(param.IdNo))
            {
                queryable = queryable.Where(p => p.IdNo.ToUpper().Contains(param.IdNo.ToUpper()));
            }
            if (!string.IsNullOrWhiteSpace(param.BirthPlace))
            {
                queryable = queryable.Where(p => p.BirthPlace.ToUpper().Contains(param.BirthPlace.ToUpper()));
            }
            if (!string.IsNullOrWhiteSpace(param.GenderName))
            {
                queryable = queryable.Where(p => p.GenderName.ToUpper().Contains(param.GenderName.ToUpper()));
            }
            if (!string.IsNullOrWhiteSpace(param.NativeName))
            {
                queryable = queryable.Where(p => p.NativeName.ToUpper().Contains(param.NativeName.ToUpper()));
            }
            if (!string.IsNullOrWhiteSpace(param.ProvinceName))
            {
                queryable = queryable.Where(p => p.ProvinceName.ToUpper().Contains(param.ProvinceName.ToUpper()));
            }

            if (!string.IsNullOrWhiteSpace(param.DistrictName))
            {
                queryable = queryable.Where(p => p.DistrictName.ToUpper().Contains(param.DistrictName.ToUpper()));
            }

            if (!string.IsNullOrWhiteSpace(param.WardName))
            {
                queryable = queryable.Where(p => p.WardName.ToUpper().Contains(param.WardName.ToUpper()));
            }
            /*
            if (!string.IsNullOrWhiteSpace(param.JoinDate))
            {
                queryable = queryable.Where(p => p.JoinDate.Contains(param.JoinDate));
            }
            if (!string.IsNullOrWhiteSpace(param.IdDate))
            {
                queryable = queryable.Where(p => p.IdDate.Contains(param.IdDate));
            }
            */
            if (param.WorkStatusId == null || param.WorkStatusId == 0)
            {
                queryable = queryable.Where(p => p.WorkStatusId != OtherConfig.EMP_STATUS_TERMINATE || (p.WorkStatusId == OtherConfig.EMP_STATUS_TERMINATE && p.TerEffectDate > DateTime.Now));
            }

            return await PagingList(queryable, param);
        }

        /// <summary>
        /// Edit Data Employee
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<ResultWithClientError> UpdateEmployee(EmployeeInput param)
        {
            try
            {
                var r = _appContext.Employees.Where(c => c.ID == _appContext.EmpId).FirstOrDefault();
                var data = Map<EmployeeInput, HU_EMPLOYEE>(param, r);
                var result = _appContext.Employees.Update(data);
                await _appContext.SaveChangesAsync();
                return new ResultWithClientError(200);
            }
            catch (Exception ex)
            {

                return new ResultWithClientError(ex);
            }
        }


        /// <summary>
        /// CMS Get Detail
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>        
        public async Task<ResultWithClientError> GetOtherListById(BaseRequest request)
        {
            try
            {
                var r = await (from p in _appContext.OtherLists
                               where p.ID == request.Id
                               select new
                               {
                                   Id = p.ID,
                                   Name = p.NAME,
                                   Code = p.CODE,
                                   TypeId = p.TYPE_ID,
                                   Orders = p.ORDERS,
                                   Note = p.NOTE,

                               }).FirstOrDefaultAsync();
                return new ResultWithClientError(r);
            }
            catch (Exception ex)
            {
                return new ResultWithClientError(ex.Message);
            }
        }

        /// <summary>
        /// CMS Get Detail
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ResultWithClientError> GetEmployeeInfo()
        {

            try
            {
                var r = await (from p in _appContext.Employees
                               join o in _appContext.Organizations on p.ORG_ID equals o.ID
                               join o2 in _appContext.Organizations on p.ORG_ID equals o2.ID into tmp1
                               from o3 in tmp1.DefaultIfEmpty()
                               join m in _appContext.Employees on o.MNG_ID equals m.ID into tmp2
                               from m2 in tmp2.DefaultIfEmpty()
                               join c in _appContext.Contracts on p.CONTRACT_ID equals c.ID into tmp3
                               from c2 in tmp3.DefaultIfEmpty()
                               join pos in _appContext.Positions on p.POSITION_ID equals pos.ID into tmp4
                               from pos2 in tmp4.DefaultIfEmpty()
                               from gt in _appContext.OtherLists.Where(x => x.ID == p.Profile.GENDER_ID).DefaultIfEmpty()
                               from re in _appContext.OtherLists.Where(x => x.ID == p.Profile.RELIGION_ID).DefaultIfEmpty()
                               from na in _appContext.OtherLists.Where(x => x.ID == p.Profile.NATIVE_ID).DefaultIfEmpty()
                               from nat in _appContext.OtherLists.Where(x => x.ID == p.Profile.NATIONALITY_ID).DefaultIfEmpty()
                               from pv in _appContext.Provinces.Where(x => x.ID == p.Profile.PROVINCE_ID).DefaultIfEmpty()
                               from di in _appContext.Districts.Where(x => x.ID == p.Profile.DISTRICT_ID).DefaultIfEmpty()
                               from wa in _appContext.Wards.Where(x => x.ID == p.Profile.WARD_ID).DefaultIfEmpty()
                               from bank in _appContext.Banks.Where(x => x.ID == p.Profile.BANK_ID).DefaultIfEmpty()
                               from train in _appContext.OtherLists.Where(x => x.ID == p.Profile.TRAINING_FORM_ID).DefaultIfEmpty()
                               from learn in _appContext.OtherLists.Where(x => x.ID == p.Profile.LEARNING_LEVEL_ID).DefaultIfEmpty()
                               from pvcur in _appContext.Provinces.Where(x => x.ID == p.Profile.CUR_PROVINCE_ID).DefaultIfEmpty()
                               from dicur in _appContext.Districts.Where(x => x.ID == p.Profile.CUR_DISTRICT_ID).DefaultIfEmpty()

                               where p.ID == _appContext.EmpId
                               select new
                               {
                                   // bản thân
                                   Id = p.ID,
                                   Code = p.CODE,
                                   FirstName = p.Profile.FIRST_NAME,
                                   Avatar = p.Profile.AVATAR,
                                   LastName = p.Profile.LAST_NAME,
                                   Fullname = p.Profile.FULL_NAME,
                                   Image = p.Profile.IMAGE,
                                   OrgId = p.ORG_ID,
                                   OrgName = o.NAME,
                                   OrgParentName = o3.NAME,
                                   OrgManager = m2.Profile.FULL_NAME,
                                   PositionId = p.POSITION_ID,
                                   PositionName = pos2.NAME,
                                   DirectManagerId = p.DIRECT_MANAGER_ID,
                                   GenderId = p.Profile.GENDER_ID,
                                   GenderName = gt.NAME,
                                   BirthDate = p.Profile.BIRTH_DATE,
                                   IdNo = p.Profile.ID_NO,
                                   IdDate = p.Profile.ID_DATE,

                                   IdPlace = p.Profile.ID_PLACE, // Địa chỉ thường trú
                                   ProvinceId = p.Profile.PROVINCE_ID,
                                   ProvinceName = pv.NAME,
                                   DistrictId = p.Profile.DISTRICT_ID,
                                   DistrictName = di.NAME,
                                   WardId = p.Profile.WARD_ID,
                                   WardName = wa.NAME,

                                   //Thông tin cá nhân
                                   ReligionId = p.Profile.RELIGION_ID,
                                   ReligionName = re.NAME,
                                   NativeId = p.Profile.NATIVE_ID,
                                   NativeName = na.NAME,
                                   NationalityId = p.Profile.NATIONALITY_ID,
                                   NationalityName = nat.NAME,
                                   Address = p.Profile.ADDRESS,
                                   BirthPlace = p.Profile.BIRTH_PLACE,
                                   JoinDate = p.JOIN_DATE,
                                   WorkStatusId = p.WORK_STATUS_ID,
                                   WorkStatusName = p.IS_NOT_CONTRACT == true ? "Đang làm việc" : " ",


                                   ContractId = p.CONTRACT_ID,
                                   ContractCode = c2.CONTRACT_NO,
                                   ContractDateEffect = c2.START_DATE,
                                   ContractDateExpire = c2.EXPIRE_DATE,
                                   LastWorkingId = p.LAST_WORKING_ID,
                                   TerEffectDate = p.TER_EFFECT_DATE,
                                   ItimeId = p.ITIME_ID, // Mã chấm công
                                   ObjectSalaryId = p.SALARY_TYPE_ID,
                                   TaxCode = p.Profile.TAX_CODE,
                                   MobilePhone = p.Profile.MOBILE_PHONE,
                                   WorkEmail = p.Profile.WORK_EMAIL,
                                   Email = p.Profile.EMAIL,

                                   //Hộ chiếu
                                   MaritalStatusId = p.Profile.MARITAL_STATUS_ID,
                                   PassNo = p.Profile.PASS_NO, // Số hộ chiếu
                                   PassDate = p.Profile.PASS_DATE, // Ngày cấp
                                   PassExpire = p.Profile.PASS_EXPIRE, // Ngày hết hạn
                                   PassPlace = p.Profile.PASS_PLACE,  // Nơi cấp

                                   //Visa
                                   VisaNo = p.Profile.VISA_NO,
                                   VisaDate = p.Profile.VISA_DATE,
                                   VisaExpire = p.Profile.VISA_EXPIRE,
                                   VisaPlace = p.Profile.VISA_PLACE,

                                   //Giấy phép lao động
                                   WorkPermit = p.Profile.WORK_PERMIT,
                                   WorkPermitDate = p.Profile.WORK_PERMIT_DATE,
                                   WorkPermitExpire = p.Profile.WORK_PERMIT_EXPIRE,
                                   WorkPermitPlace = p.Profile.WORK_PERMIT_PLACE,
                                   ContactPer = p.Profile.CONTACT_PER,
                                   ContactPerPhone = p.Profile.CONTACT_PER_PHONE,

                                   // Tài khoản
                                   BankId = p.Profile.BANK_ID,
                                   //BankName = bank.NAME,
                                   BankBranch = p.Profile.BANK_BRANCH,
                                   BankNo = p.Profile.BANK_NO,

                                   // Trình độ học vấn
                                   SchoolId = p.Profile.SCHOOL_ID,
                                   QualificationId = p.Profile.QUALIFICATION_ID,
                                   TrainingFormId = p.Profile.TRAINING_FORM_ID,
                                   TrainingFormName = train.NAME,
                                   LearningLevelId = p.Profile.LEARNING_LEVEL_ID,
                                   LearningLevelName = learn.NAME,
                                   Language = p.Profile.LANGUAGE,
                                   LanguageMark = p.Profile.LANGUAGE_MARK,

                                   ResidentId = p.RESIDENT_ID,
                                   SalTotal = p.SAL_TOTAL,

                                   //Địa chỉ thường trú
                                   CurProvinceId = p.Profile.CUR_PROVINCE_ID,
                                   CurProvinceName = pvcur.NAME,
                                   CurDistrictId = p.Profile.CUR_DISTRICT_ID,
                                   CurDistrictName = dicur.NAME,
                                   CurWardId = p.Profile.CUR_WARD_ID,
                                   CurAddress = p.Profile.CUR_ADDRESS,

                                   //Chứng chỉ hành nghề
                                   WorkNo = p.Profile.WORK_NO,
                                   WorkDate = p.WORK_DATE,
                                   WorkScope = p.Profile.WORK_SCOPE,
                                   WorkPlace = p.Profile.WORK_PLACE
                               }).FirstOrDefaultAsync();
                return new ResultWithClientError(r);
            }
            catch (Exception ex)
            {
                return new ResultWithClientError(ex.Message);
            }

        }

        public async Task<ResultWithClientError> GetContractInfo()
        {
            try
            {
                var r = await (from p in _appContext.Contracts
                               join e in _appContext.Employees on p.EMPLOYEE_ID equals e.ID
                               join o in _appContext.Organizations on e.ORG_ID equals o.ID
                               from f in _appContext.OtherListFixs.Where(c => c.ID == p.STATUS_ID && c.TYPE == SystemConfig.STATUS_APPROVE).DefaultIfEmpty()
                               join l in _appContext.ContractTypes on p.CONTRACT_TYPE_ID equals l.ID
                               where e.ID == _appContext.EmpId
                               orderby p.STATUS_ID, p.START_DATE descending
                               select new ContractDTO
                               {
                                   Id = p.ID,
                                   EmployeeId = p.EMPLOYEE_ID,
                                   EmployeeCode = e.CODE,
                                   EmployeeName = e.Profile.FULL_NAME,
                                   OrgName = o.NAME,
                                   OrgId = e.ORG_ID,
                                   StartDate = p.START_DATE,
                                   ExpireDate = p.EXPIRE_DATE,
                                   ContractNo = p.CONTRACT_NO,
                                   SignerName = p.SIGNER_NAME,
                                   SignerPosition = p.SIGNER_POSITION,
                                   SignDate = p.SIGN_DATE,
                                   StatusName = f.NAME,
                                   WorkStatusId = e.WORK_STATUS_ID,
                                   StatusId = p.STATUS_ID,
                                   ContractTypeName = l.NAME
                               }).ToListAsync();

                return new ResultWithClientError(r);
            }
            catch (Exception ex)
            {
                return new ResultWithClientError(ex.Message);
            }
        }

        /// <summary>
        /// CMS Get All Data
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<PagedResults<ContractDTO>> GetContractList(ContractDTO param)
        {
            var queryable = from p in _appContext.Contracts
                            join e in _appContext.Employees on p.EMPLOYEE_ID equals e.ID
                            join o in _appContext.Organizations on e.ORG_ID equals o.ID
                            from f in _appContext.OtherListFixs.Where(c => c.ID == p.STATUS_ID && c.TYPE == SystemConfig.STATUS_APPROVE).DefaultIfEmpty()
                            join l in _appContext.ContractTypes on p.CONTRACT_TYPE_ID equals l.ID

                            orderby p.STATUS_ID, p.START_DATE descending
                            select new ContractDTO
                            {
                                Id = p.ID,
                                EmployeeId = p.EMPLOYEE_ID,
                                EmployeeCode = e.CODE,
                                EmployeeName = e.Profile.FULL_NAME,
                                OrgName = o.NAME,
                                OrgId = e.ORG_ID,
                                StartDate = p.START_DATE,
                                ExpireDate = p.EXPIRE_DATE,
                                ContractNo = p.CONTRACT_NO,
                                SignerName = p.SIGNER_NAME,
                                SignerPosition = p.SIGNER_POSITION,
                                SignDate = p.SIGN_DATE,
                                StatusName = f.NAME,
                                WorkStatusId = e.WORK_STATUS_ID,
                                StatusId = p.STATUS_ID,
                                ContractTypeName = l.NAME
                            };
            var orgIds = await QueryData.ExecuteList(Procedures.PKG_COMMON_LIST_ORG,
                    new
                    {
                        P_IS_ADMIN = _appContext.IsAdmin == true ? 1 : 0,

                        P_ORG_ID = param.OrgId,
                        P_CURENT_USER_ID = _appContext.CurrentUserId,
                        P_CUR = QueryData.OUT_CURSOR
                    }, false);


            List<long?> ids = orgIds.Select(c => (long?)((dynamic)c).ID).ToList();
            if (param.OrgId != null)
            {
                ids.Add(param.OrgId);
            }
            queryable = queryable.Where(p => ids.Contains(p.OrgId));

            if (!string.IsNullOrWhiteSpace(param.EmployeeCode))
            {
                queryable = queryable.Where(p => p.EmployeeCode.ToUpper().Contains(param.EmployeeCode.ToUpper()));
            }
            if (!string.IsNullOrWhiteSpace(param.EmployeeName))
            {
                queryable = queryable.Where(p => p.EmployeeName.ToUpper().Contains(param.EmployeeName.ToUpper()));
            }
            if (!string.IsNullOrWhiteSpace(param.OrgName))
            {
                queryable = queryable.Where(p => p.OrgName.ToUpper().Contains(param.OrgName.ToUpper()));
            }
            if (!string.IsNullOrWhiteSpace(param.ContractNo))
            {
                queryable = queryable.Where(p => p.ContractNo.ToUpper().Contains(param.ContractNo.ToUpper()));
            }
            if (!string.IsNullOrWhiteSpace(param.StatusName))
            {
                queryable = queryable.Where(p => p.StatusName.ToUpper().Contains(param.StatusName.ToUpper()));
            }
            if (!string.IsNullOrWhiteSpace(param.ContractTypeName))
            {
                queryable = queryable.Where(p => p.ContractTypeName.ToUpper().Contains(param.ContractTypeName.ToUpper()));
            }

            if (param.StartDate != null)
            {
                queryable = queryable.Where(p => p.StartDate == param.StartDate);
            }
            if (param.ExpireDate != null)
            {
                queryable = queryable.Where(p => p.ExpireDate == param.ExpireDate);
            }
            if (param.WorkStatusId == null || param.WorkStatusId == 0)
            {
                queryable = queryable.Where(p => p.WorkStatusId != OtherConfig.EMP_STATUS_TERMINATE);
            }
            return await PagingLists(queryable, param);
        }

        /// <summary>
        /// CMS Get Detail
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>        
        public async Task<ResultWithClientError> GetContractById(BaseRequest request)
        {
            try
            {
                var n = await (from p in _appContext.Contracts
                               join e in _appContext.Employees on p.EMPLOYEE_ID equals e.ID
                               join o in _appContext.Organizations on e.ORG_ID equals o.ID
                               join o2 in _appContext.Organizations on o.PARENT_ID equals o2.ID into tmp1
                               from o3 in tmp1.DefaultIfEmpty()
                               join t in _appContext.Positions on e.POSITION_ID equals t.ID into tmp2
                               from t1 in tmp2.DefaultIfEmpty()
                               join w in _appContext.Workings on p.WORKING_ID equals w.ID into tmp5
                               from w1 in tmp5.DefaultIfEmpty()
                               where p.ID == request.Id
                               select new
                               {
                                   Id = p.ID,
                                   EmployeeId = p.EMPLOYEE_ID,
                                   EmployeeName = e.Profile.FULL_NAME,
                                   EmployeeCode = e.CODE,
                                   PositionName = t1.NAME,
                                   OrgId = e.ORG_ID,
                                   OrgName = o.NAME,
                                   OrgParentName = o3.NAME,
                                   StartDate = p.START_DATE,
                                   ExpireDate = p.EXPIRE_DATE,
                                   ContractNo = p.CONTRACT_NO,
                                   ContractTypeId = p.CONTRACT_TYPE_ID,
                                   StatusId = p.STATUS_ID,
                                   SignId = p.SIGN_ID,
                                   SignerName = p.SIGNER_NAME,
                                   SignerPosition = p.SIGNER_POSITION,
                                   SignDate = p.SIGN_DATE,
                                   WorkingId = p.WORKING_ID,
                                   WorkingNo = w1.DECISION_NO,
                                   SalBasic = w1.SAL_BASIC,
                                   SalPercent = w1.SAL_PERCENT,
                                   salTotal = w1.SAL_TOTAL,
                                   Note = p.NOTE
                               }).FirstOrDefaultAsync();
                return new ResultWithClientError(n);
            }
            catch (Exception ex)
            {
                return new ResultWithClientError(ex.Message);
            }
        }

        /// <summary>
        /// CMS Get All Datarach
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        /*public async Task<PagedResults<HuBankDTO>> GetBankList(HuBankDTO param)
        {
            var queryable = from p in _appContext.Banks
                            select new HuBankDTO
                            {
                                Id = p.ID,
                                Name = p.NAME,
                                Code = p.CODE,
                                IsActive = p.IS_ACTIVE,
                                Note = p.NOTE,
                                Order = p.ORDER,
                                CreatedBy = p.CREATED_BY,
                                CreatedDate = p.CREATED_DATE,
                                UpdatedBy = p.UPDATED_BY,
                                UpdatedDate = p.UPDATED_DATE
                            };

            if (param.Name != null)
            {
                queryable = queryable.Where(p => p.Name.ToUpper().Contains(param.Name.ToUpper()));
            }

            if (param.Code != null)
            {
                queryable = queryable.Where(p => p.Code.ToUpper().Contains(param.Code.ToUpper()));
            }

            if (param.Note != null)
            {
                queryable = queryable.Where(p => p.Note.ToUpper().Contains(param.Note.ToUpper()));
            }

            if (param.IsActive != null)
            {
                queryable = queryable.Where(p => p.IsActive == param.IsActive);
            }

            return await PagingLists(queryable, param);
        }*/

        /// <summary>
        /// Bank Get Detail
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>        
        public async Task<ResultWithClientError> GetBankById(BaseRequest request)
        {
            var r = await (from p in _appContext.Banks
                           where p.ID == request.Id
                           select new HuBankDTO
                           {
                               Id = p.ID,
                               Name = p.NAME,
                               Code = p.CODE,
                               Note = p.NOTE,
                               Order = p.ORDER,
                               IsActive = p.IS_ACTIVE
                           }).FirstOrDefaultAsync();
            return new ResultWithClientError(r);
        }

        /// <summary>
        /// Chức vụ
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<PagedResults<PositionViewDTO>> GetPositionList(PositionViewDTO param)
        {
            var queryable = from p in _appContext.Positions
                            from g in _appContext.GroupPositions.Where(c => c.ID == p.GROUP_ID)

                            orderby p.CREATED_DATE descending
                            select new PositionViewDTO
                            {
                                Id = p.ID,
                                Name = p.NAME,
                                GroupName = g.NAME,
                                GroupId = p.GROUP_ID,
                                Code = p.CODE,
                                IsActive = p.IS_ACTIVE,
                                Note = p.NOTE,
                                JobDesc = p.JOB_DESC,
                                CreateBy = p.CREATED_BY,
                                CreateDate = p.CREATED_DATE,
                                UpdateBy = p.UPDATED_BY,
                                UpdateDate = p.UPDATED_DATE
                            };

            if (param.Name != null)
            {
                queryable = queryable.Where(p => p.Name.ToUpper().Contains(param.Name.ToUpper()));
            }

            if (param.Code != null)
            {
                queryable = queryable.Where(p => p.Code.ToUpper().Contains(param.Code.ToUpper()));
            }
            if (param.Note != null)
            {
                queryable = queryable.Where(p => p.Note.ToUpper().Contains(param.Note.ToUpper()));
            }
            if (!string.IsNullOrEmpty(param.GroupName))
            {
                queryable = queryable.Where(p => p.Note.ToUpper().Contains(param.Note.ToUpper()));
            }
            if (param.IsActive != null)
            {
                queryable = queryable.Where(p => p.IsActive == param.IsActive);
            }
            if (param.GroupId != null)
            {
                queryable = queryable.Where(p => p.GroupId == param.GroupId);
            }
            if (param.keyword != null)
            {
                queryable = queryable.Where(p => p.Name.ToUpper().Contains(param.keyword.ToUpper()));
            }

            return await PagingLists(queryable, param);
        }

        /// <summary>
        /// CMS Get Detail
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>        
        public async Task<ResultWithClientError> GetPositionById(BaseRequest request)
        {
            try
            {
                var r = await (from p in _appContext.Positions
                               where p.ID == request.Id
                               select new
                               {
                                   Id = p.ID,
                                   Name = p.NAME,
                                   Code = p.CODE,
                                   Note = p.NOTE,
                                   GroupId = p.GROUP_ID,
                                   JobDesc = p.JOB_DESC
                               }).FirstOrDefaultAsync();
                return new ResultWithClientError(r);
            }
            catch (Exception ex)
            {
                return new ResultWithClientError(ex.Message);
            }
        }
        /// <summary>
        /// CMS Get All Data
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<PagedResults<ProvinceDTO>> GetProvinceList(ProvinceDTO param)
        {
            var queryable = from p in _appContext.Provinces
                            select new ProvinceDTO
                            {
                                Id = p.ID,
                                Code = p.CODE,
                                Name = p.NAME,
                                IsActive = p.IS_ACTIVE,
                                CreateBy = p.CREATED_BY,
                                UpdatedBy = p.UPDATED_BY,
                                CreateDate = p.CREATED_DATE,
                                UpdatedDate = p.UPDATED_DATE
                            };


            if (!string.IsNullOrWhiteSpace(param.Name))
            {
                queryable = queryable.Where(p => p.Name.ToUpper().Contains(param.Name.ToUpper()));
            }
            if (!string.IsNullOrWhiteSpace(param.Code))
            {
                queryable = queryable.Where(p => p.Code.ToUpper().Contains(param.Code.ToUpper()));
            }
            if (param.IsActive != null)
            {
                queryable = queryable.Where(p => p.IsActive == param.IsActive);
            }
            if (param.keyword != null)
            {
                queryable = queryable.Where(p => p.Name.ToUpper().Contains(param.keyword.ToUpper()) || p.Code.ToUpper().Contains(param.keyword.ToUpper()));
            }
            return await PagingLists(queryable, param);
        }

        public async Task<ResultWithClientError> GetProvinceById(BaseRequest request)
        {
            try
            {
                var r = await (from p in _appContext.Provinces
                               where p.ID == request.Id
                               select new
                               {
                                   Id = p.ID,
                                   Code = p.CODE,
                                   Name = p.NAME,
                                   IsActive = p.IS_ACTIVE,
                               }).FirstOrDefaultAsync();
                return new ResultWithClientError(r);
            }
            catch (Exception ex)
            {
                return new ResultWithClientError(ex.Message);
            }
        }

        public async Task<PagedResults<DistrictDTO>> GetDistrictList(DistrictDTO param)
        {
            var queryable = from p in _appContext.Districts
                            select new DistrictDTO
                            {
                                Id = p.ID,
                                Code = p.CODE,
                                Name = p.NAME,
                                IsActive = p.IS_ACTIVE,
                                CreateBy = p.CREATED_BY,
                                UpdatedBy = p.UPDATED_BY,
                                CreateDate = p.CREATED_DATE,
                                UpdatedDate = p.UPDATED_DATE
                            };


            if (!string.IsNullOrWhiteSpace(param.Name))
            {
                queryable = queryable.Where(p => p.Name.ToUpper().Contains(param.Name.ToUpper()));
            }
            if (!string.IsNullOrWhiteSpace(param.Code))
            {
                queryable = queryable.Where(p => p.Code.ToUpper().Contains(param.Code.ToUpper()));
            }
            if (param.IsActive != null)
            {
                queryable = queryable.Where(p => p.IsActive == param.IsActive);
            }
            if (param.keyword != null)
            {
                queryable = queryable.Where(p => p.Name.ToUpper().Contains(param.keyword.ToUpper()) || p.Code.ToUpper().Contains(param.keyword.ToUpper()));
            }
            return await PagingLists(queryable, param);
        }

        public async Task<ResultWithClientError> GetDistrictById(BaseRequest request)
        {
            try
            {
                var r = await (from p in _appContext.Districts
                               where p.ID == request.Id
                               select new
                               {
                                   Id = p.ID,
                                   Code = p.CODE,
                                   Name = p.NAME,
                                   IsActive = p.IS_ACTIVE,
                               }).FirstOrDefaultAsync();
                return new ResultWithClientError(r);
            }
            catch (Exception ex)
            {
                return new ResultWithClientError(ex.Message);
            }
        }

        public async Task<PagedResults<WardDTO>> GetWardlist(WardDTO param)
        {
            var queryable = from p in _appContext.Wards
                            select new WardDTO
                            {
                                Id = p.ID,
                                Code = p.CODE,
                                Name = p.NAME,
                                IsActive = p.IS_ACTIVE,
                                CreateBy = p.CREATED_BY,
                                UpdatedBy = p.UPDATED_BY,
                                CreateDate = p.CREATED_DATE,
                                UpdatedDate = p.UPDATED_DATE
                            };
            if (param.keyword != null)
            {
                queryable = queryable.Where(p => p.Name.ToUpper().Contains(param.keyword.ToUpper()) || p.Code.ToUpper().Contains(param.keyword.ToUpper()));
            }
            return await PagingLists(queryable, param);
        }

        public async Task<ResultWithClientError> GetWardById(BaseRequest request)
        {
            try
            {
                var r = await (from p in _appContext.Wards
                               where p.ID == request.Id
                               select new
                               {
                                   Id = p.ID,
                                   Code = p.CODE,
                                   Name = p.NAME,
                                   IsActive = p.IS_ACTIVE,
                               }).FirstOrDefaultAsync();
                return new ResultWithClientError(r);
            }
            catch (Exception ex)
            {
                return new ResultWithClientError(ex.Message);
            }
        }

        /// <summary>
        /// Khen thưởng
        /// </summary>
        /// <returns></returns>
        public async Task<ResultWithClientError> GetCommendInfo()
        {
            try
            {
                var r = await (from p in _appContext.Commends
                               join o in _appContext.Organizations on p.ORG_ID equals o.ID into tmp1
                               from o2 in tmp1.DefaultIfEmpty()
                               join s in _appContext.OtherListFixs.Where(c => c.TYPE == SystemConfig.SOURCE_COST) on p.SOURCE_COST_ID equals s.ID
                               join f in _appContext.OtherListFixs.Where(c => c.TYPE == SystemConfig.STATUS_APPROVE) on p.STATUS_ID equals f.ID
                               join g in _appContext.OtherListFixs.Where(c => c.TYPE == SystemConfig.OBJECT_COMMEND) on p.COMMEND_OBJ_ID equals g.ID
                               join b in _appContext.CommendEmps on p.ID equals b.COMMEND_ID
                               join a in _appContext.Employees on b.EMPLOYEE_ID equals a.ID
                               orderby p.STATUS_ID, p.EFFECT_DATE descending
                               select new CommendDTO
                               {
                                   Id = p.ID,
                                   CommendObjName = g.NAME,
                                   SignerName = p.SIGNER_NAME,
                                   SignerPosition = p.SIGNER_POSITION,
                                   SignDate = p.SIGN_DATE,
                                   Employees = (from a in _appContext.CommendEmps
                                                join b in _appContext.Employees on a.EMPLOYEE_ID equals b.ID
                                                where a.COMMEND_ID == p.ID && b.ID == _appContext.EmpId
                                                select new ReferParam
                                                {
                                                    Id = b.ID,
                                                    Name = b.Profile.FULL_NAME,
                                                    Code = b.CODE
                                                }).ToList()
                                                  ,
                                   OrgName = o2.NAME,
                                   OrgId = o2.ID,
                                   EffectDate = p.EFFECT_DATE,
                                   Year = p.YEAR,
                                   No = p.NO,
                                   StatusName = f.NAME,
                                   StatusId = p.STATUS_ID,
                                   IsTax = p.IS_TAX,
                                   Reason = p.REASON,
                                   CommendType = p.COMMEND_TYPE,
                                   Money = p.MONEY,
                                   SourceCostName = s.NAME,
                                   EmployeeName = a.Profile.FULL_NAME
                               }).ToListAsync();

                return new ResultWithClientError(r);
            }
            catch (Exception ex)
            {
                return new ResultWithClientError(ex.Message);
            }
        }

        /// <summary>
        /// Kỷ luật
        /// </summary>
        /// <returns></returns>
        public async Task<ResultWithClientError> GetDisciplineInfo()
        {
            try
            {
                var r = await (from p in _appContext.Disciplines
                               from e in _appContext.Employees.Where(x => x.ID == p.EMPLOYEE_ID).DefaultIfEmpty()
                               from o in _appContext.Organizations.Where(x => x.ID == e.ORG_ID).DefaultIfEmpty()
                               from f in _appContext.OtherListFixs.Where(x => x.ID == p.STATUS_ID && x.TYPE == SystemConfig.STATUS_APPROVE)
                               where e.ID == _appContext.EmpId
                               orderby p.STATUS_ID, p.EFFECT_DATE descending
                               select new DisciplineDTO
                               {
                                   Id = p.ID,
                                   EmployeeCode = e.CODE,
                                   EmployeeName = e.Profile.FULL_NAME,
                                   Reason = p.REASON,
                                   OrgName = o.NAME,
                                   EffectDate = p.EFFECT_DATE,
                                   DisciplineType = p.DISCIPLINE_TYPE,
                                   StatusName = f.NAME,
                                   StatusId = p.STATUS_ID,
                                   CreatedBy = p.CREATED_BY,
                                   UpdatedBy = p.UPDATED_BY,
                                   CreatedDate = p.CREATED_DATE,
                                   UpdatedDate = p.UPDATED_DATE
                               }).ToListAsync();

                return new ResultWithClientError(r);
            }
            catch (Exception ex)
            {
                return new ResultWithClientError(ex.Message);
            }
        }

        /// <summary>
        /// Biến động bảo hiểm
        /// </summary>
        /// <returns></returns>
        public async Task<ResultWithClientError> GetInschangeInfo()
        {
            try
            {
                var r = await (from p in _appContext.InsChanges
                               from e in _appContext.Employees.Where(c => c.ID == p.EMPLOYEE_ID)
                               from o in _appContext.Organizations.Where(c => c.ID == e.ORG_ID)
                               from a in _appContext.InsuranceTypes.Where(c => c.ID == p.CHANGE_TYPE_ID).DefaultIfEmpty()
                               where e.ID == _appContext.EmpId
                               orderby p.CHANGE_MONTH descending, e.CODE
                               select new ProfileDAL.ViewModels.InsChangeDTO
                               {
                                   Id = p.ID,
                                   EmployeeName = e.Profile.FULL_NAME,
                                   EmployeeCode = e.CODE,
                                   OrgId = e.ORG_ID,
                                   OrgName = o.NAME,
                                   ChangeTypeName = a.NAME,
                                   ChangeMonth = p.CHANGE_MONTH,
                                   SalaryOld = (double?)p.SALARY_OLD,
                                   SalaryNew = (double?)p.SALARY_NEW,
                                   WorkStatusId = e.WORK_STATUS_ID,
                                   TerEffectDate = e.TER_EFFECT_DATE,
                                   IsBhxh = p.IS_BHXH,
                                   IsBhyt = p.IS_BHYT,
                                   IsBhtn = p.IS_BHTN,
                                   IsBnn = p.IS_BNN
                               }).ToListAsync();

                return new ResultWithClientError(r);
            }
            catch (Exception ex)
            {
                return new ResultWithClientError(ex.Message);
            }
        }

        /// <summary>
        /// Quyết đinh thay đổi
        /// </summary>
        /// <returns></returns>
        public async Task<ResultWithClientError> GetWorkingInfo()
        {
            try
            {
                var r = await (from p in _appContext.Workings
                               join e in _appContext.Employees on p.EMPLOYEE_ID equals e.ID
                               join t in _appContext.Positions on e.POSITION_ID equals t.ID
                               join o in _appContext.Organizations on e.ORG_ID equals o.ID
                               from f in _appContext.OtherListFixs.Where(c => c.ID == p.STATUS_ID && c.TYPE == SystemConfig.STATUS_APPROVE).DefaultIfEmpty()
                               join l in _appContext.OtherLists on p.TYPE_ID equals l.ID
                               from s in _appContext.Employees.Where(c => c.ID == p.SIGN_ID).DefaultIfEmpty()
                               join tl in _appContext.SalaryTypes on p.SALARY_TYPE_ID equals tl.ID
                               where e.ID == _appContext.EmpId
                               orderby p.STATUS_ID, p.EFFECT_DATE descending
                               select new WorkingDTO
                               {
                                   Id = p.ID,
                                   EmployeeId = p.EMPLOYEE_ID,
                                   EmployeeCode = e.CODE,
                                   EmployeeName = e.Profile.FULL_NAME,
                                   PositionName = t.NAME,
                                   SignDate = p.SIGN_DATE,
                                   SignerName = p.SIGNER_NAME,
                                   SignerCode = s.CODE,
                                   SignerPosition = p.SIGNER_POSITION,
                                   OrgName = o.NAME,
                                   OrgId = p.ORG_ID,
                                   EffectDate = p.EFFECT_DATE,
                                   DecisionNo = p.DECISION_NO,
                                   StatusId = p.STATUS_ID,
                                   StatusName = f.NAME,
                                   TypeId = p.TYPE_ID,
                                   TypeName = l.NAME,
                                   WorkStatusId = e.WORK_STATUS_ID,
                                   SalBasic = p.SAL_BASIC,
                                   SalTotal = p.SAL_TOTAL,
                                   SalPercent = p.SAL_PERCENT,
                                   SalaryType = tl.NAME
                               }).ToListAsync();

                return new ResultWithClientError(r);
            }
            catch (Exception ex)
            {
                return new ResultWithClientError(ex.Message);
            }
        }

        /// Portal 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>        
        public async Task<ResultWithClientError> GetEmployeeFamily()
        {
            try
            {

                var query = await (from p in _appContext.Families
                                   join r in _appContext.OtherLists on p.RELATIONSHIP_ID equals (int)r.ID
                                   where p.ID == _appContext.EmpId
                                   select new
                                   {
                                       Id = p.ID,
                                       RelationshipId = p.RELATIONSHIP_ID,
                                       RelationshipName = r.NAME,
                                       EmployeeId = p.ID,
                                   }).ToListAsync();
                return new ResultWithClientError(query);

            }
            catch (Exception ex)
            {
                return new ResultWithClientError(ex.Message);
            }
        }

        /// <summary>
        /// Portal update Infomation
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<ResultWithError> EmployeeEdit(EmployeeEditInput param)
        {
            try
            {
                var r = await (from p in _appContext.EmployeeEdits
                               where p.EMPLOYEE_ID == _appContext.EmpId
                               && p.STATUS == 1
                               select p).FirstOrDefaultAsync();

                if (r != null)
                {
                    var r2 = _appContext.EmployeeEdits.Where(c => c.EMPLOYEE_ID == _appContext.EmpId && c.STATUS == 1).OrderByDescending(c => c.ID).FirstOrDefault();
                    var dataUpdate = Map<EmployeeEditInput, HU_EMPLOYEE_EDIT>(param, r2);
                    //update
                    var result = _appContext.EmployeeEdits.Update(dataUpdate);
                }
                else
                {
                    var data = Map(param, new HU_EMPLOYEE_EDIT());
                    data.EMPLOYEE_ID = _appContext.EmpId;
                    data.STATUS = 1;
                    // insert
                    _appContext.EmployeeEdits.Add(data);
                }


                await _appContext.SaveChangesAsync();
                return new ResultWithError(200);
            }
            catch (Exception ex)
            {
                return new ResultWithError(ex);
            }
        }

        /// <summary>
        /// Main info
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<ResultWithError> EmployeeMainInfoEdit(EmployeeMainInfoDTO param)
        {
            try
            {
                var r = await (from p in _appContext.EmployeeEdits
                               where p.EMPLOYEE_ID == _appContext.EmpId
                               && p.STATUS == 1
                               select p).FirstOrDefaultAsync();

                if (r != null)
                {
                    var r2 = _appContext.EmployeeEdits.Where(c => c.EMPLOYEE_ID == _appContext.EmpId && c.STATUS == 1).OrderByDescending(c => c.ID).FirstOrDefault();
                    var dataUpdate = Map<EmployeeMainInfoDTO, HU_EMPLOYEE_EDIT>(param, r2);
                    //update
                    var result = _appContext.EmployeeEdits.Update(dataUpdate);
                }
                else
                {
                    var data = Map(param, new HU_EMPLOYEE_EDIT());
                    data.EMPLOYEE_ID = _appContext.EmpId;
                    data.STATUS = 1;
                    // insert
                    _appContext.EmployeeEdits.Add(data);
                }


                await _appContext.SaveChangesAsync();
                return new ResultWithError(200);
            }
            catch (Exception ex)
            {
                return new ResultWithError(ex);
            }
        }


        /// <summary>
        /// Thông tin cá nhân
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<ResultWithError> EmployeeInfoEdit(EmployeeInfoDTO param)
        {
            try
            {
                var r = await (from p in _appContext.EmployeeEdits
                               where p.EMPLOYEE_ID == _appContext.EmpId
                               && p.STATUS == 1
                               select p).FirstOrDefaultAsync();

                if (r != null)
                {
                    var r2 = _appContext.EmployeeEdits.Where(c => c.EMPLOYEE_ID == _appContext.EmpId && c.STATUS == 1).OrderByDescending(c => c.ID).FirstOrDefault();
                    var dataUpdate = Map<EmployeeInfoDTO, HU_EMPLOYEE_EDIT>(param, r2);
                    //update
                    var result = _appContext.EmployeeEdits.Update(dataUpdate);
                }
                else
                {
                    var data = Map(param, new HU_EMPLOYEE_EDIT());
                    data.EMPLOYEE_ID = _appContext.EmpId;
                    data.STATUS = 1;
                    // insert
                    _appContext.EmployeeEdits.Add(data);
                }


                await _appContext.SaveChangesAsync();
                return new ResultWithError(200);
            }
            catch (Exception ex)
            {
                return new ResultWithError(ex);
            }
        }

        /// <summary>
        /// Địa chỉ thường trú
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<ResultWithError> EmployeeAddressEdit(EmployeeAddressDTO param)
        {
            try
            {
                var r = await (from p in _appContext.EmployeeEdits
                               where p.EMPLOYEE_ID == _appContext.EmpId
                               && p.STATUS == 1
                               select p).FirstOrDefaultAsync();

                if (r != null)
                {
                    var r2 = _appContext.EmployeeEdits.Where(c => c.EMPLOYEE_ID == _appContext.EmpId && c.STATUS == 1).OrderByDescending(c => c.ID).FirstOrDefault();
                    var dataUpdate = Map<EmployeeAddressDTO, HU_EMPLOYEE_EDIT>(param, r2);
                    //update
                    var result = _appContext.EmployeeEdits.Update(dataUpdate);
                }
                else
                {
                    var data = Map(param, new HU_EMPLOYEE_EDIT());
                    data.EMPLOYEE_ID = _appContext.EmpId;
                    data.STATUS = 1;
                    // insert
                    _appContext.EmployeeEdits.Add(data);
                }


                await _appContext.SaveChangesAsync();
                return new ResultWithError(200);
            }
            catch (Exception ex)
            {
                return new ResultWithError(ex);
            }
        }



        /// <summary>
        /// Địa chỉ hiện tại
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<ResultWithError> EmployeeCurAddressEdit(EmployeeCurAddressDTO param)
        {
            try
            {
                var r = await (from p in _appContext.EmployeeEdits
                               where p.EMPLOYEE_ID == _appContext.EmpId
                               && p.STATUS == 1
                               select p).FirstOrDefaultAsync();

                if (r != null)
                {
                    var r2 = _appContext.EmployeeEdits.Where(c => c.EMPLOYEE_ID == _appContext.EmpId && c.STATUS == 1).OrderByDescending(c => c.ID).FirstOrDefault();
                    var dataUpdate = Map<EmployeeCurAddressDTO, HU_EMPLOYEE_EDIT>(param, r2);
                    //update
                    var result = _appContext.EmployeeEdits.Update(dataUpdate);
                }
                else
                {
                    var data = Map(param, new HU_EMPLOYEE_EDIT());
                    data.EMPLOYEE_ID = _appContext.EmpId;
                    data.STATUS = 1;
                    // insert
                    _appContext.EmployeeEdits.Add(data);
                }


                await _appContext.SaveChangesAsync();
                return new ResultWithError(200);
            }
            catch (Exception ex)
            {
                return new ResultWithError(ex);
            }
        }


        /// <summary>
        /// Thông tin liên hệ
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<ResultWithError> EmployeeContactInfoEdit(EmployeeContactInfoDTO param)
        {
            try
            {
                var r = await (from p in _appContext.EmployeeEdits
                               where p.EMPLOYEE_ID == _appContext.EmpId
                               && p.STATUS == 1
                               select p).FirstOrDefaultAsync();

                if (r != null)
                {
                    var r2 = _appContext.EmployeeEdits.Where(c => c.EMPLOYEE_ID == _appContext.EmpId && c.STATUS == 1).OrderByDescending(c => c.ID).FirstOrDefault();
                    var dataUpdate = Map<EmployeeContactInfoDTO, HU_EMPLOYEE_EDIT>(param, r2);
                    //update
                    var result = _appContext.EmployeeEdits.Update(dataUpdate);
                }
                else
                {
                    var data = Map(param, new HU_EMPLOYEE_EDIT());
                    data.EMPLOYEE_ID = _appContext.EmpId;
                    data.STATUS = 1;
                    // insert
                    _appContext.EmployeeEdits.Add(data);
                }


                await _appContext.SaveChangesAsync();
                return new ResultWithError(200);
            }
            catch (Exception ex)
            {
                return new ResultWithError(ex);
            }
        }


        /// <summary>
        /// Hộ chiếu
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<ResultWithError> EmployeePassportEdit(EmployeePassportDTO param)
        {
            try
            {
                var r = await (from p in _appContext.EmployeeEdits
                               where p.EMPLOYEE_ID == _appContext.EmpId
                               && p.STATUS == 1
                               select p).FirstOrDefaultAsync();

                if (r != null)
                {
                    var r2 = _appContext.EmployeeEdits.Where(c => c.EMPLOYEE_ID == _appContext.EmpId && c.STATUS == 1).OrderByDescending(c => c.ID).FirstOrDefault();
                    var dataUpdate = Map<EmployeePassportDTO, HU_EMPLOYEE_EDIT>(param, r2);
                    //update
                    var result = _appContext.EmployeeEdits.Update(dataUpdate);
                }
                else
                {
                    var data = Map(param, new HU_EMPLOYEE_EDIT());
                    data.EMPLOYEE_ID = _appContext.EmpId;
                    data.STATUS = 1;
                    // insert
                    _appContext.EmployeeEdits.Add(data);
                }


                await _appContext.SaveChangesAsync();
                return new ResultWithError(200);
            }
            catch (Exception ex)
            {
                return new ResultWithError(ex);
            }
        }


        /// <summary>
        /// Visa
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<ResultWithError> EmployeeVisaEdit(EmployeeVisaDTO param)
        {
            try
            {
                var r = await (from p in _appContext.EmployeeEdits
                               where p.EMPLOYEE_ID == _appContext.EmpId
                               && p.STATUS == 1
                               select p).FirstOrDefaultAsync();

                if (r != null)
                {
                    var r2 = _appContext.EmployeeEdits.Where(c => c.EMPLOYEE_ID == _appContext.EmpId && c.STATUS == 1).OrderByDescending(c => c.ID).FirstOrDefault();
                    var dataUpdate = Map<EmployeeVisaDTO, HU_EMPLOYEE_EDIT>(param, r2);
                    //update
                    var result = _appContext.EmployeeEdits.Update(dataUpdate);
                }
                else
                {
                    var data = Map(param, new HU_EMPLOYEE_EDIT());
                    data.EMPLOYEE_ID = _appContext.EmpId;
                    data.STATUS = 1;
                    // insert
                    _appContext.EmployeeEdits.Add(data);
                }


                await _appContext.SaveChangesAsync();
                return new ResultWithError(200);
            }
            catch (Exception ex)
            {
                return new ResultWithError(ex);
            }
        }

        /// <summary>
        /// Portal update Infomation
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<ResultWithError> EmployeeEducationEdit(EmployeeEducationDTO param)
        {
            try
            {
                var r = await (from p in _appContext.EmployeeEdits
                               where p.EMPLOYEE_ID == _appContext.EmpId
                               && p.STATUS == 1
                               select p).FirstOrDefaultAsync();

                if (r != null)
                {
                    var r2 = _appContext.EmployeeEdits.Where(c => c.EMPLOYEE_ID == _appContext.EmpId && c.STATUS == 1).OrderByDescending(c => c.ID).FirstOrDefault();
                    var dataUpdate = Map<EmployeeEducationDTO, HU_EMPLOYEE_EDIT>(param, r2);
                    //update
                    var result = _appContext.EmployeeEdits.Update(dataUpdate);
                }
                else
                {
                    var data = Map(param, new HU_EMPLOYEE_EDIT());
                    data.EMPLOYEE_ID = _appContext.EmpId;
                    data.STATUS = 1;
                    // insert
                    _appContext.EmployeeEdits.Add(data);
                }


                await _appContext.SaveChangesAsync();
                return new ResultWithError(200);
            }
            catch (Exception ex)
            {
                return new ResultWithError(ex);
            }
        }

        /// <summary>
        /// Portal update Infomation
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<ResultWithError> EmployeeCertificateEdit(EmployeeCertificateDTO param)
        {
            try
            {
                var r = await (from p in _appContext.EmployeeEdits
                               where p.EMPLOYEE_ID == _appContext.EmpId
                               && p.STATUS == 1
                               select p).FirstOrDefaultAsync();

                if (r != null)
                {
                    var r2 = _appContext.EmployeeEdits.Where(c => c.EMPLOYEE_ID == _appContext.EmpId && c.STATUS == 1).OrderByDescending(c => c.ID).FirstOrDefault();
                    var dataUpdate = Map<EmployeeCertificateDTO, HU_EMPLOYEE_EDIT>(param, r2);
                    //update
                    var result = _appContext.EmployeeEdits.Update(dataUpdate);
                }
                else
                {
                    var data = Map(param, new HU_EMPLOYEE_EDIT());
                    data.EMPLOYEE_ID = _appContext.EmpId;
                    data.STATUS = 1;
                    // insert
                    _appContext.EmployeeEdits.Add(data);
                }


                await _appContext.SaveChangesAsync();
                return new ResultWithError(200);
            }
            catch (Exception ex)
            {
                return new ResultWithError(ex);
            }
        }

        /// <summary>
        /// Portal update giấy phép lao động
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<ResultWithError> EmployeeWorkPermitEdit(EmployeeWorkPermitDTO param)
        {
            try
            {
                var r = await (from p in _appContext.EmployeeEdits
                               where p.EMPLOYEE_ID == _appContext.EmpId
                               && p.STATUS == 1
                               select p).FirstOrDefaultAsync();

                if (r != null)
                {
                    var r2 = _appContext.EmployeeEdits.Where(c => c.EMPLOYEE_ID == _appContext.EmpId && c.STATUS == 1).OrderByDescending(c => c.ID).FirstOrDefault();
                    var dataUpdate = Map<EmployeeWorkPermitDTO, HU_EMPLOYEE_EDIT>(param, r2);
                    //update
                    var result = _appContext.EmployeeEdits.Update(dataUpdate);
                }
                else
                {
                    var data = Map(param, new HU_EMPLOYEE_EDIT());
                    data.EMPLOYEE_ID = _appContext.EmpId;
                    data.STATUS = 1;
                    // insert
                    _appContext.EmployeeEdits.Add(data);
                }


                await _appContext.SaveChangesAsync();
                return new ResultWithError(200);
            }
            catch (Exception ex)
            {
                return new ResultWithError(ex);
            }
        }

        /// <summary>
        /// Tài khoản ngân hàng
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<ResultWithError> EmployeeBankEdit(EmployeeBankDTO param)
        {
            try
            {
                var r = await (from p in _appContext.EmployeeEdits
                               where p.EMPLOYEE_ID == _appContext.EmpId
                               && p.STATUS == 1
                               select p).FirstOrDefaultAsync();

                if (r != null)
                {
                    var r2 = _appContext.EmployeeEdits.Where(c => c.EMPLOYEE_ID == _appContext.EmpId && c.STATUS == 1).OrderByDescending(c => c.ID).FirstOrDefault();
                    var dataUpdate = Map<EmployeeBankDTO, HU_EMPLOYEE_EDIT>(param, r2);
                    //update
                    var result = _appContext.EmployeeEdits.Update(dataUpdate);
                }
                else
                {
                    var data = Map(param, new HU_EMPLOYEE_EDIT());
                    data.EMPLOYEE_ID = _appContext.EmpId;
                    data.STATUS = 1;
                    // insert
                    _appContext.EmployeeEdits.Add(data);
                }


                await _appContext.SaveChangesAsync();
                return new ResultWithError(200);
            }
            catch (Exception ex)
            {
                return new ResultWithError(ex);
            }
        }

        /// <summary>
        /// Get List paper by Emp
        /// </summary>
        /// <param name="param">EmpId</param>
        /// <returns></returns>
        public async Task<ResultWithClientError> GetListEmployeePaper()
        {

            try
            {
                var r = await (from o in _appContext.EmployeePaperses.Where(c => c.EMP_ID == _appContext.EmpId).DefaultIfEmpty()
                               select new ListPaperView
                               {
                                   Id = o.ID == null ? 0 : o.ID,
                                   TypeId = o.ID == null ? 0 : 1,
                                   DateInput = o.DATE_INPUT,
                                   statusId = o.STATUS_ID == null ? false : (o.STATUS_ID == 1 ? true : false),
                                   Url = o.URL,
                                   Note = o.NOTE

                               }).ToListAsync();
                return new ResultWithClientError(r);
            }
            catch (Exception ex)
            {
                return new ResultWithClientError(ex.Message);
            }

        }


        /// <summary>
        /// CMS Get Detail
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>        
        public async Task<ResultWithError> GetById(long id)
        {
            try
            {
                var r = await (from p in _appContext.Employees
                               from s in _appContext.OtherLists.Where(x => x.ID == p.WORK_STATUS_ID).DefaultIfEmpty()
                               from s2 in _appContext.OtherLists.Where(x => x.ID == p.STATUS_DETAIL_ID).DefaultIfEmpty()
                               from o in _appContext.Organizations.Where(x => x.ID == p.ORG_ID).DefaultIfEmpty()
                               from o3 in _appContext.Organizations.Where(x => x.ID == o.PARENT_ID).DefaultIfEmpty()
                               from c in _appContext.Contracts.Where(x => x.ID == p.CONTRACT_ID).DefaultIfEmpty()
                               from pos in _appContext.Positions.Where(x => x.ID == p.POSITION_ID).DefaultIfEmpty()
                               from di in _appContext.Positions.Where(x => x.ID == pos.LM).DefaultIfEmpty()
                               from de in _appContext.Employees.Where(x => x.ID == di.MASTER).DefaultIfEmpty()
                               from st in _appContext.HUJobBands.Where(x => x.ID == p.STAFF_RANK_ID).DefaultIfEmpty()
                               where p.ID == id
                               select new
                               {
                                   Id = p.ID,
                                   Profileid = p.PROFILE_ID,
                                   ProfileCode = p.PROFILE_CODE,
                                   Code = p.CODE,
                                   FirstName = p.Profile.FIRST_NAME,
                                   Avatar = p.Profile.AVATAR,
                                   LastName = p.Profile.LAST_NAME,
                                   Fullname = p.Profile.FULL_NAME,
                                   Image = p.Profile.IMAGE,
                                   OrgId = p.ORG_ID,
                                   OrgName = o.NAME,
                                   OrgParentName = o3.NAME,
                                   PositionId = p.POSITION_ID,
                                   PositionName = pos.NAME,
                                   StaffRankId = p.STAFF_RANK_ID,
                                   StaffRankName = st.NAME_VN,
                                   DirectManagerId = p.DIRECT_MANAGER_ID,
                                   DirectManagerName = (!string.IsNullOrEmpty(de.CODE) ? de.CODE + " - " + de.Profile.FULL_NAME : ""),
                                   DirectManagerTitleName = (!string.IsNullOrEmpty(di.CODE) ? di.CODE + " - " + di.NAME : ""),
                                   GenderId = p.Profile.GENDER_ID,
                                   BirthDate = p.Profile.BIRTH_DATE,
                                   IdNo = p.Profile.ID_NO,
                                   IdDate = p.Profile.ID_DATE,
                                   IdPlace = p.Profile.ID_PLACE,
                                   ReligionId = p.Profile.RELIGION_ID,
                                   NativeId = p.Profile.NATIVE_ID,
                                   NationalityId = p.Profile.NATIONALITY_ID,
                                   Address = p.Profile.ADDRESS,
                                   BirthPlace = p.Profile.BIRTH_PLACE,
                                   JoinDate = p.JOIN_DATE,
                                   WorkStatusId = p.WORK_STATUS_ID,
                                   WorkStatus = s.NAME,
                                   WorkStatusDetailId = p.STATUS_DETAIL_ID,
                                   WorkStatusDetail = s2.NAME,
                                   ProvinceId = p.Profile.PROVINCE_ID,
                                   DistrictId = p.Profile.DISTRICT_ID,
                                   WardId = p.Profile.WARD_ID,
                                   ContractId = p.CONTRACT_ID,
                                   ContractCode = c.CONTRACT_NO,
                                   ContractDateEffect = c.START_DATE,
                                   ContractDateExpire = c.EXPIRE_DATE,
                                   LastWorkingId = p.LAST_WORKING_ID,
                                   TerEffectDate = p.TER_EFFECT_DATE,
                                   ItimeId = p.ITIME_ID,
                                   ObjectSalaryId = p.SALARY_TYPE_ID,
                                   TaxCode = p.Profile.TAX_CODE,
                                   MobilePhone = p.Profile.MOBILE_PHONE,
                                   WorkEmail = p.Profile.WORK_EMAIL,
                                   Email = p.Profile.EMAIL,
                                   MaritalStatusId = p.Profile.MARITAL_STATUS_ID,
                                   PassNo = p.Profile.PASS_NO,
                                   PassDate = p.Profile.PASS_DATE,
                                   PassExpire = p.Profile.PASS_EXPIRE,
                                   PassPlace = p.Profile.PASS_PLACE,
                                   VisaNo = p.Profile.VISA_NO,
                                   VisaDate = p.Profile.VISA_DATE,
                                   VisaExpire = p.Profile.VISA_EXPIRE,
                                   VisaPlace = p.Profile.VISA_PLACE,
                                   WorkPermit = p.Profile.WORK_PERMIT,
                                   WorkPermitDate = p.Profile.WORK_PERMIT_DATE,
                                   WorkPermitExpire = p.Profile.WORK_PERMIT_EXPIRE,
                                   WorkPermitPlace = p.Profile.WORK_PERMIT_PLACE,
                                   ContactPer = p.Profile.CONTACT_PER,
                                   ContactPerPhone = p.Profile.CONTACT_PER_PHONE,
                                   BankId = p.Profile.BANK_ID,
                                   BankBranch = p.Profile.BANK_BRANCH,
                                   BankNo = p.Profile.BANK_NO,
                                   SchoolId = p.Profile.SCHOOL_ID,
                                   QualificationId = p.Profile.QUALIFICATION_ID,
                                   TrainingFormId = p.Profile.TRAINING_FORM_ID,
                                   LearningLevelId = p.Profile.LEARNING_LEVEL_ID,
                                   Language = p.Profile.LANGUAGE,
                                   LanguageMark = p.Profile.LANGUAGE_MARK,
                                   ResidentId = p.RESIDENT_ID,
                                   SalTotal = p.SAL_TOTAL,
                                   CurProvinceId = p.Profile.CUR_PROVINCE_ID,
                                   CurDistrictId = p.Profile.CUR_DISTRICT_ID,
                                   CurWardId = p.Profile.CUR_WARD_ID,
                                   CurAddress = p.Profile.CUR_ADDRESS,
                                   WorkNo = p.Profile.WORK_NO,
                                   WorkDate = p.WORK_DATE,
                                   WorkScope = p.Profile.WORK_SCOPE,
                                   WorkPlace = p.Profile.WORK_PLACE,
                                   FullNameOnConcurrently = p.CODE + " - " + p.Profile.FULL_NAME
                               }).FirstOrDefaultAsync();
                return new ResultWithError(r);
            }
            catch (Exception ex)
            {
                return new ResultWithError(ex.Message);
            }
        }

        public async Task<FormatedResponse> GetByIdOnPortal(long? id)
        {
            try
            {
                var r = await (from p in _appContext.Employees.Where(x => x.ID == id).DefaultIfEmpty()
                               from c in _appContext.EmployeeCvs.Where(x => x.ID == p.PROFILE_ID).DefaultIfEmpty()
                               from d in _appContext.Districts.Where(x => x.ID == c.DISTRICT_ID).DefaultIfEmpty()
                               from v in _appContext.Provinces.Where(x => x.ID == c.PROVINCE_ID).DefaultIfEmpty()
                               from w in _appContext.Wards.Where(x => x.ID == c.WARD_ID).DefaultIfEmpty()
                               from s in _appContext.OtherLists.Where(x => x.ID == c.NATIONALITY_ID).DefaultIfEmpty()
                               from s2 in _appContext.OtherLists.Where(x => x.ID == c.NATIVE_ID).DefaultIfEmpty()
                               select new
                               {
                                   BirthDay = c.BIRTH_DATE,

                               }).FirstOrDefaultAsync();
                return new FormatedResponse()
                {
                    InnerBody = r,
                    StatusCode = EnumStatusCode.StatusCode200
                };
            }
            catch (Exception ex)
            {
                return new FormatedResponse()
                {
                    InnerBody = ex.Message,
                    StatusCode = EnumStatusCode.StatusCode400
                };
            }
        }



        /// <summary>
        /// CMS Get Detail
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>        
        public async Task<ResultWithClientError> GetEmployeeById(BaseRequest request)
        {
            try
            {
                var r = await (from p in _appContext.Employees
                               join o in _appContext.Organizations on p.ORG_ID equals o.ID
                               join o2 in _appContext.Organizations on p.ORG_ID equals o2.ID into tmp1
                               from o3 in tmp1.DefaultIfEmpty()
                               join m in _appContext.Employees on o.MNG_ID equals m.ID into tmp2
                               from m2 in tmp2.DefaultIfEmpty()
                               join c in _appContext.Contracts on p.CONTRACT_ID equals c.ID into tmp3
                               from c2 in tmp3.DefaultIfEmpty()
                               join pos in _appContext.Positions on p.POSITION_ID equals pos.ID into tmp4
                               from pos2 in tmp4.DefaultIfEmpty()
                               where p.ID == request.Id
                               select new
                               {
                                   Id = p.ID,
                                   Code = p.CODE,
                                   FirstName = p.Profile.FIRST_NAME,
                                   Avatar = p.Profile.AVATAR,
                                   LastName = p.Profile.LAST_NAME,
                                   Fullname = p.Profile.FULL_NAME,
                                   Image = p.Profile.IMAGE,
                                   OrgId = p.ORG_ID,
                                   OrgName = o.NAME,
                                   OrgParentName = o3.NAME,
                                   OrgManager = m2.Profile.FULL_NAME,
                                   PositionId = p.POSITION_ID,
                                   PositionName = pos2.NAME,
                                   DirectManagerId = p.DIRECT_MANAGER_ID,
                                   GenderId = p.Profile.GENDER_ID,
                                   BirthDate = p.Profile.BIRTH_DATE,
                                   IdNo = p.Profile.ID_NO,
                                   IdDate = p.Profile.ID_DATE,
                                   IdPlace = p.Profile.ID_PLACE,
                                   ReligionId = p.Profile.RELIGION_ID,
                                   NativeId = p.Profile.NATIVE_ID,
                                   NationalityId = p.Profile.NATIONALITY_ID,
                                   Address = p.Profile.ADDRESS,
                                   BirthPlace = p.Profile.BIRTH_PLACE,
                                   JoinDate = p.JOIN_DATE,
                                   WorkStatusId = p.WORK_STATUS_ID,
                                   ProvinceId = p.Profile.PROVINCE_ID,
                                   DistrictId = p.Profile.DISTRICT_ID,
                                   WardId = p.Profile.WARD_ID,
                                   ContractId = p.CONTRACT_ID,
                                   ContractCode = c2.CONTRACT_NO,
                                   ContractDateEffect = c2.START_DATE,
                                   ContractDateExpire = c2.EXPIRE_DATE,
                                   LastWorkingId = p.LAST_WORKING_ID,
                                   TerEffectDate = p.TER_EFFECT_DATE,
                                   ItimeId = p.ITIME_ID,
                                   ObjectSalaryId = p.SALARY_TYPE_ID,
                                   TaxCode = p.Profile.TAX_CODE,
                                   MobilePhone = p.Profile.MOBILE_PHONE,
                                   WorkEmail = p.Profile.WORK_EMAIL,
                                   Email = p.Profile.EMAIL,
                                   MaritalStatusId = p.Profile.MARITAL_STATUS_ID,
                                   PassNo = p.Profile.PASS_NO,
                                   PassDate = p.Profile.PASS_DATE,
                                   PassExpire = p.Profile.PASS_EXPIRE,
                                   PassPlace = p.Profile.PASS_PLACE,
                                   VisaNo = p.Profile.VISA_NO,
                                   VisaDate = p.Profile.VISA_DATE,
                                   VisaExpire = p.Profile.VISA_EXPIRE,
                                   VisaPlace = p.Profile.VISA_PLACE,
                                   WorkPermit = p.Profile.WORK_PERMIT,
                                   WorkPermitDate = p.Profile.WORK_PERMIT_DATE,
                                   WorkPermitExpire = p.Profile.WORK_PERMIT_EXPIRE,
                                   WorkPermitPlace = p.Profile.WORK_PERMIT_PLACE,
                                   ContactPer = p.Profile.CONTACT_PER,
                                   ContactPerPhone = p.Profile.CONTACT_PER_PHONE,
                                   BankId = p.Profile.BANK_ID,
                                   BankBranch = p.Profile.BANK_BRANCH,
                                   BankNo = p.Profile.BANK_NO,
                                   SchoolId = p.Profile.SCHOOL_ID,
                                   QualificationId = p.Profile.QUALIFICATION_ID,
                                   TrainingFormId = p.Profile.TRAINING_FORM_ID,
                                   LearningLevelId = p.Profile.LEARNING_LEVEL_ID,
                                   Language = p.Profile.LANGUAGE,
                                   LanguageMark = p.Profile.LANGUAGE_MARK,
                                   ResidentId = p.RESIDENT_ID,
                                   SalTotal = p.SAL_TOTAL,
                                   CurProvinceId = p.Profile.CUR_PROVINCE_ID,
                                   CurDistrictId = p.Profile.CUR_DISTRICT_ID,
                                   CurWardId = p.Profile.CUR_WARD_ID,
                                   CurAddress = p.Profile.CUR_ADDRESS,
                                   WorkNo = p.Profile.WORK_NO,
                                   WorkDate = p.WORK_DATE,
                                   WorkScope = p.Profile.WORK_SCOPE,
                                   WorkPlace = p.Profile.WORK_PLACE
                               }).FirstOrDefaultAsync();
                return new ResultWithClientError(r);
            }
            catch (Exception ex)
            {
                return new ResultWithClientError(ex.Message);
            }
        }

        public async Task<ResultWithError> GetList()
        {
            try
            {
                var orgIds = await QueryData.ExecuteList(Procedures.PKG_COMMON_LIST_ORG,
                    new
                    {
                        P_IS_ADMIN = _appContext.IsAdmin == true ? 1 : 0,

                        P_ORG_ID = 0,
                        P_CURENT_USER_ID = _appContext.CurrentUserId,
                        P_CUR = QueryData.OUT_CURSOR
                    }, false);
                List<long?> ids = orgIds.Select(c => (long?)((dynamic)c).ID).ToList();
                var query = await (from e in _appContext.Employees
                                   where ids.Contains(e.ORG_ID) && e.WORK_STATUS_ID == OtherConfig.EMP_STATUS_WORKING
                                   select new
                                   {
                                       Id = e.ID,
                                       Code = e.CODE,
                                       Name = "[" + e.CODE + "] " + e.Profile.FULL_NAME,
                                   }).ToListAsync();
                return new ResultWithError(query);
            }
            catch (Exception ex)
            {
                return new ResultWithError(ex.Message);
            }
        }
        public async Task<ResultWithError> GetListByOrg(int orgId)
        {
            try
            {
                var orgIds = await QueryData.ExecuteList(Procedures.PKG_COMMON_LIST_ORG,
                    new
                    {
                        P_IS_ADMIN = _appContext.IsAdmin == true ? 1 : 0,

                        P_ORG_ID = orgId,
                        P_CURENT_USER_ID = _appContext.CurrentUserId,
                        P_CUR = QueryData.OUT_CURSOR
                    }, false);


                List<long?> ids = orgIds.Select(c => (long?)((dynamic)c).ID).ToList();
                ids.Add(orgId);

                var query = await (from e in _appContext.Employees
                                   where ids.Contains(e.ORG_ID)
                                   select new
                                   {
                                       Id = e.ID,
                                       Code = e.CODE,
                                       Name = e.Profile.FULL_NAME,
                                   }).ToListAsync();
                return new ResultWithError(query);
            }
            catch (Exception ex)
            {
                return new ResultWithError(ex.Message);
            }
        }
        public async Task<ResultWithError> ListSituation(long EmpId)
        {
            try
            {
                var query = await (from p in _appContext.Families
                                   join r in _appContext.OtherLists on p.RELATIONSHIP_ID equals (int)r.ID
                                   where p.ID == EmpId
                                   select new
                                   {
                                       Id = p.ID,
                                       RelationshipId = p.RELATIONSHIP_ID,
                                       RelationshipName = r.NAME,
                                   }).ToListAsync();
                return new ResultWithError(query);
            }
            catch (Exception ex)
            {
                return new ResultWithError(ex.Message);
            }
        }
        public async Task<ResultWithError> RemoveRelation(int id)
        {
            try
            {
                var r = await _appContext.Families.Where(x => x.ID == id).FirstOrDefaultAsync();
                if (r == null)
                {
                    return new ResultWithError(404);
                }

                _appContext.Families.Remove(r);
                await _appContext.SaveChangesAsync();
                return new ResultWithError(200);
            }
            catch (Exception ex)
            {
                return new ResultWithError(ex.Message);
            }
        }



        /// <summary>
        /// Create Data
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<ResultWithError> CreateAsync(EmployeeInput param)
        {
            try
            {

                var c = _appContext.Employees.Where(x => x.CODE.ToLower() == param.Code.ToLower()).Count();
                if (c > 0)
                {
                    return new ResultWithError(Consts.CODE_EXISTS);
                }
                var r = _appContext.Organizations.Where(x => x.ID == param.OrgId).Count();
                if (r == 0)
                {
                    return new ResultWithError(Message.ORG_NOT_EXIST);
                }
                var t = _appContext.Positions.Where(x => x.ID == param.PositionId).Count();
                if (t == 0)
                {
                    return new ResultWithError(Message.POSITION_NOT_EXIST);
                }
                param.Fullname = param.FirstName + " " + param.LastName;
                var data = Map(param, new HU_EMPLOYEE());
                data.WORK_STATUS_ID = OtherConfig.EMP_STATUS_WORKING;
                var result = await _appContext.Employees.AddAsync(data);
                await _appContext.SaveChangesAsync();
                return new ResultWithError(data);
            }
            catch (Exception ex)
            {
                return new ResultWithError(ex);
            }
        }
        /// <summary>
        /// Create Data
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<ResultWithError> CreateSituation(SituationDTO param)
        {
            try
            {
                var r = _appContext.OtherLists.Where(x => x.ID == param.RelationshipId).Count();
                if (r == 0)
                {
                    return new ResultWithError(Message.RELATION_SHIP_NOT_EXIST);
                }
                var t = _appContext.Employees.Where(x => x.ID == param.EmployeeId).Count();
                if (t == 0)
                {
                    return new ResultWithError(Message.EMP_NOT_EXIST);
                }
                var p = await _appContext.Families.FindAsync(param.Id);
                if (p == null)
                {
                    var data = Map(param, new HU_FAMILY());
                    await _appContext.Families.AddAsync(data);
                }
                else
                {
                    var data = Map(param, p);
                    _appContext.Families.Update(data);
                }

                await _appContext.SaveChangesAsync();
                return new ResultWithError(200);
            }
            catch (Exception ex)
            {
                return new ResultWithError(ex);
            }
        }
        /// <summary>
        /// CMS Edit Data
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<ResultWithError> UpdateAsync(EmployeeInput param)
        {
            try
            {
                var r = _appContext.Employees.Where(c => c.ID == param.Id).FirstOrDefault();
                if (r == null)
                {
                    return new ResultWithError(Consts.ID_NOT_FOUND);
                }

                var code = _appContext.Employees.Where(x => x.CODE.ToLower() == param.Code.ToLower() && x.ID != param.Id).Count();
                if (code > 0)
                {
                    return new ResultWithError(Consts.CODE_EXISTS);
                }
                var o = _appContext.Organizations.Where(x => x.ID == param.OrgId).Count();
                if (o == 0)
                {
                    return new ResultWithError("ORGANIZATION_NOT_EXISTS");
                }
                var t = _appContext.Positions.Where(x => x.ID == param.PositionId).Count();
                if (t == 0)
                {
                    return new ResultWithError("POSITION_NOT_EXISTS");
                }

                var data = Map(param, r);
                var result = _appContext.Employees.Update(data);
                await _appContext.SaveChangesAsync();
                return new ResultWithError(200);
            }
            catch (Exception ex)
            {

                return new ResultWithError(ex);
            }
        }

        /// <summary>
        /// Portal 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>        
        public async Task<ResultWithError> PortalGetBy(int type)
        {
            try
            {
                if (type == 1)
                {
                    var r = await (from p in _appContext.Employees
                                   from g in _appContext.OtherLists.Where(x => x.ID == p.Profile.GENDER_ID).DefaultIfEmpty()
                                   from m in _appContext.OtherLists.Where(x => x.ID == p.Profile.MARITAL_STATUS_ID).DefaultIfEmpty()
                                   from n in _appContext.OtherLists.Where(x => x.ID == p.Profile.NATIONALITY_ID).DefaultIfEmpty()
                                   from t in _appContext.OtherLists.Where(x => x.ID == p.Profile.NATIVE_ID).DefaultIfEmpty()
                                   from l in _appContext.OtherLists.Where(x => x.ID == p.Profile.RELIGION_ID).DefaultIfEmpty()
                                   from v in _appContext.Provinces.Where(x => x.ID == p.Profile.CUR_PROVINCE_ID).DefaultIfEmpty()
                                   from d in _appContext.Districts.Where(x => x.ID == p.Profile.CUR_DISTRICT_ID).DefaultIfEmpty()
                                   from w in _appContext.Wards.Where(x => x.ID == p.Profile.CUR_WARD_ID).DefaultIfEmpty()
                                   from v1 in _appContext.Provinces.Where(x => x.ID == p.Profile.PROVINCE_ID).DefaultIfEmpty()
                                   from d1 in _appContext.Districts.Where(x => x.ID == p.Profile.DISTRICT_ID).DefaultIfEmpty()
                                   from w1 in _appContext.Wards.Where(x => x.ID == p.Profile.WARD_ID).DefaultIfEmpty()
                                   where p.ID == _appContext.EmpId
                                   select new
                                   {
                                       // bản thân
                                       Id = p.ID,
                                       Code = p.CODE,
                                       Fullname = p.Profile.FULL_NAME,
                                       GenderName = g.NAME,
                                       BirthDate = p.Profile.BIRTH_DATE == null ? DateTime.Parse("01/01/1900") : p.Profile.BIRTH_DATE,
                                       BirthPlace = p.Profile.BIRTH_PLACE,
                                       Nationality = n.NAME, // Quốc tịch
                                       NativeName = t.NAME, // Dân tộc
                                       RelagionName = l.NAME, // Tôn giáo
                                       MaritalName = m.NAME, // Hôn nhân                                                             
                                       IdNo = p.Profile.ID_NO,// CMND
                                       IdDate = p.Profile.ID_DATE == null ? DateTime.Parse("01/01/1900") : p.Profile.ID_DATE,
                                       IdPlace = p.Profile.ID_PLACE,
                                       // address
                                       Address = p.Profile.CUR_ADDRESS,
                                       ProvinceName = v.NAME,
                                       DistrictName = d.NAME,
                                       WardName = w.NAME,
                                       // per address
                                       PerAddress = p.Profile.ADDRESS,
                                       PerProvince = v1.NAME,
                                       PerDistrict = d1.NAME,
                                       PerWard = w1.NAME,
                                       // Thông tin liên hệ
                                       MobilePhone = p.Profile.MOBILE_PHONE,
                                       WorkEmail = p.Profile.WORK_EMAIL,
                                       Email = p.Profile.EMAIL,
                                       ContactPer = p.Profile.CONTACT_PER,
                                       ContactPerPhone = p.Profile.CONTACT_PER_PHONE
                                   }).FirstOrDefaultAsync();
                    return new ResultWithError(r);
                }
                else if (type == 2) // Thông tin phụ
                {
                    var r = await (from p in _appContext.Employees
                                   where p.ID == _appContext.EmpId
                                   select new
                                   {
                                       // Thông tin phụ
                                       PassNo = p.Profile.PASS_NO,
                                       PassDate = p.Profile.PASS_DATE,
                                       PassExpire = p.Profile.PASS_EXPIRE,
                                       PassPlace = p.Profile.PASS_PLACE,
                                       VisaNo = p.Profile.VISA_NO,
                                       VisaDate = p.Profile.VISA_DATE,
                                       VisaExpire = p.Profile.VISA_EXPIRE,
                                       VisaPlace = p.Profile.VISA_PLACE,
                                       WorkPermit = p.Profile.WORK_PERMIT,
                                       WorkPermitDate = p.Profile.WORK_PERMIT_DATE,
                                       WorkPermitExpire = p.Profile.WORK_PERMIT_EXPIRE,
                                       WorkPermitPlace = p.Profile.WORK_PERMIT_PLACE,
                                   }).FirstOrDefaultAsync();
                    return new ResultWithError(r);
                }
                else if (type == 3) // Tài khoản
                {
                    var r = await (from p in _appContext.Employees
                                   from b in _appContext.Banks.Where(x => x.ID == p.Profile.BANK_ID).DefaultIfEmpty()
                                   where p.ID == _appContext.EmpId
                                   select new
                                   {
                                       // Tài khoản
                                       BankName = b.NAME,
                                       BranchName = p.Profile.BANK_BRANCH,
                                       BankNo = p.Profile.BANK_NO
                                   }).FirstOrDefaultAsync();
                    return new ResultWithError(r);
                }
                else
                {
                    var r = await (from p in _appContext.Employees
                                   from f in _appContext.OtherLists.Where(x => x.ID == p.Profile.TRAINING_FORM_ID).DefaultIfEmpty()
                                   from i in _appContext.OtherLists.Where(x => x.ID == p.Profile.LEARNING_LEVEL_ID).DefaultIfEmpty()
                                   where p.ID == _appContext.EmpId
                                   select new
                                   {
                                       // Trình độ học vấn
                                       SchoolName = p.Profile.SCHOOL_ID,
                                       TrainingFormName = f.NAME, // Hình thức đào tạo
                                       LearningName = i.NAME,// Trình độ học vấn
                                       QualificaName = p.Profile.QUALIFICATION_ID, // Trình độ chuyên môn
                                       Language = p.Profile.LANGUAGE,
                                       LanguageMark = p.Profile.LANGUAGE_MARK
                                   }).FirstOrDefaultAsync();
                    return new ResultWithError(r);
                }
            }
            catch (Exception ex)
            {
                return new ResultWithError(ex.Message);
            }
        }
        /// <summary>
        /// Portal 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>        
        public async Task<ResultWithError> PortalGetFamily()
        {
            try
            {

                var r = await (from p in _appContext.Families
                               from g in _appContext.OtherLists.Where(x => x.ID == p.RELATIONSHIP_ID).DefaultIfEmpty()
                               where p.ID == _appContext.EmpId
                               select new
                               {
                                   RelationName = g.NAME
                               }).ToListAsync();
                return new ResultWithError(r);

            }
            catch (Exception ex)
            {
                return new ResultWithError(ex.Message);
            }
        }

        /// <summary>
        /// Portal Get Infomation
        /// </summary>
        /// <returns></returns>
        public async Task<ResultWithError> GetInfo()
        {
            try
            {
                var r = await QueryData.ExecuteStore<InfoView>(Procedures.PKG_PROFILE_GET_INFO, new
                {

                    P_EMP_ID = _appContext.EmpId,
                    P_CUR = QueryData.OUT_CURSOR
                });
                return new ResultWithError(r[0]);
            }
            catch (Exception ex)
            {
                return new ResultWithError(ex.Message);
            }

        }
        /// <summary>
        ///  Get infor when choise employee in module contractinfor
        /// </summary>
        /// <returns></returns>
        public async Task<ResultWithError> GetInforContract(long Id)
        {
            try
            {
                var r = await (
                               from p in _appContext.Employees
                               join o in _appContext.Organizations on p.ORG_ID equals o.ID
                               join x in _appContext.Organizations on o.PARENT_ID equals x.ID into tmp1
                               join c in _appContext.Positions on p.POSITION_ID equals c.ID into tmp2
                               from x2 in tmp1.DefaultIfEmpty()
                               from c2 in tmp2.DefaultIfEmpty()
                               where p.ID == Id
                               select new EmployeeOutput
                               {
                                   EmployeeId = p.ID,
                                   EmployeeCode = p.CODE,
                                   EmployeeName = p.Profile.FULL_NAME,
                                   PositionId = p.POSITION_ID,
                                   PositionName = c2.NAME,
                                   OrgId = (long)p.ORG_ID,
                                   OrgName = o.NAME,
                                   OrgParentName = x2.NAME,
                                   Avatar = p.Profile.AVATAR,
                                   ContracExpired = p.CONTRACT_EXPIRED,
                                   WorkingId = p.LAST_WORKING_ID
                                   //DateStart = (DateTime?)_appContext.Contracts
                                   //                         .Where(s => s.EMPLOYEE_ID == Id )
                                   //                         .OrderByDescending(d => d.ID).Select(f => f.EXPIRE_DATE).FirstOrDefault(),
                                   //LastWorkingId = _appContext.Workings.Where(s => s.EMPLOYEE_ID == Id )
                                   //                 .OrderByDescending(d => d.EFFECT_DATE).Select(f => f.ID).FirstOrDefault()
                               }).FirstAsync();

                r.StartDate = r.ContracExpired != null ? ((DateTime)r.ContracExpired).AddDays(1) : DateTime.Now;
                return new ResultWithError(r);
            }
            catch (Exception ex)
            {
                return new ResultWithError(ex.Message);
            }

        }
        /// <summary>
        ///  Get infor when choise employee in module contractinfor
        /// </summary>
        /// <returns></returns>
        public async Task<ResultWithError> GetInforLeaveJob(long Id)
        {
            try
            {
                var r = await (
                               from p in _appContext.Employees
                               join o in _appContext.Organizations on p.ORG_ID equals o.ID
                               join x in _appContext.Organizations on o.PARENT_ID equals x.ID into tmp1
                               join c in _appContext.Positions on p.POSITION_ID equals c.ID into tmp2
                               join g in _appContext.Contracts on p.CONTRACT_ID equals g.ID into tmp3
                               from x2 in tmp1.DefaultIfEmpty()
                               from c2 in tmp2.DefaultIfEmpty()
                               from g2 in tmp3.DefaultIfEmpty()
                               where p.ID == Id
                               select new EmployeeOutput
                               {
                                   EmployeeId = p.ID,
                                   EmployeeCode = p.CODE,
                                   EmployeeName = p.Profile.FULL_NAME,
                                   PositionId = p.POSITION_ID,
                                   PositionName = c2.NAME,
                                   OrgId = p.ORG_ID,
                                   OrgName = o.NAME,
                                   OrgParentName = x2.NAME,
                                   ContractNo = g2.CONTRACT_NO,
                                   DateStart = g2.START_DATE,
                                   DateEnd = g2.EXPIRE_DATE

                               }).FirstAsync();

                return new ResultWithError(r);
            }
            catch (Exception ex)
            {
                return new ResultWithError(ex.Message);
            }

        }
        public async Task<ResultWithError> GetEmpAlowance(List<long> Ids)
        {
            try
            {
                var r = await (
                               from p in _appContext.Employees
                               join o in _appContext.Organizations on p.ORG_ID equals o.ID
                               join x in _appContext.Organizations on o.PARENT_ID equals x.ID into tmp1
                               join c in _appContext.Positions on p.POSITION_ID equals c.ID into tmp2
                               join g in _appContext.Contracts on p.CONTRACT_ID equals g.ID into tmp3
                               from x2 in tmp1.DefaultIfEmpty()
                               from c2 in tmp2.DefaultIfEmpty()
                               from g2 in tmp3.DefaultIfEmpty()
                               where Ids.Contains(p.ID)
                               select new EmployeeOutput
                               {
                                   EmployeeId = p.ID,
                                   EmployeeCode = p.CODE,
                                   EmployeeName = p.Profile.FULL_NAME,
                                   PositionId = p.POSITION_ID,
                                   PositionName = c2.NAME,
                                   OrgId = (long)p.ORG_ID,
                                   OrgName = o.NAME,
                                   OrgParentName = x2.NAME,
                                   ContractNo = g2.CONTRACT_NO,
                                   DateStart = g2.START_DATE,
                                   DateEnd = g2.EXPIRE_DATE,
                                   WorkStatusId = p.WORK_STATUS_ID

                               }).ToListAsync();

                return new ResultWithError(r);
            }
            catch (Exception ex)
            {
                return new ResultWithError(ex.Message);
            }

        }

        public async Task<ResultWithError> GetListEmpToImport()
        {
            try
            {
                var r = await (
                               from p in _appContext.Employees
                               join o in _appContext.Organizations on p.ORG_ID equals o.ID
                               join x in _appContext.Organizations on o.PARENT_ID equals x.ID into tmp1
                               join c in _appContext.Positions on p.POSITION_ID equals c.ID into tmp2
                               from x2 in tmp1.DefaultIfEmpty()
                               from c2 in tmp2.DefaultIfEmpty()
                               where p.WORK_STATUS_ID == OtherConfig.EMP_STATUS_WORKING
                               select new
                               {
                                   PeriodId = "",
                                   EmpId = p.ID,
                                   EmployeeCode = p.CODE,
                                   EmployeeName = p.Profile.FULL_NAME,
                                   OrgName = o.NAME,
                                   PositionName = c2.NAME,
                               }).ToListAsync();
                return new ResultWithError(r);
            }
            catch (Exception ex)
            {
                return new ResultWithError(ex.Message);
            }
        }
        /// <summary>
        /// Get List For Popup
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<PagedResult<EmpPopupDTO>> GetListPopup(EmpPopupDTO param)
        {
            var queryable = from p in _appContext.Employees
                            from o in _appContext.Organizations.Where(c => c.ID == p.ORG_ID).DefaultIfEmpty()
                            from t in _appContext.Positions.Where(c => c.ID == p.POSITION_ID).DefaultIfEmpty()

                            orderby p.CODE
                            select new EmpPopupDTO
                            {
                                Id = p.ID,
                                Code = p.CODE,
                                Fullname = p.Profile.FULL_NAME,
                                OrgId = p.ORG_ID,
                                OrgName = o.NAME,
                                PositionName = t.NAME,
                                WorkStatusId = p.WORK_STATUS_ID,
                                TerEffectDate = p.TER_EFFECT_DATE,
                            };
            //check phan quyen theo phong ban
            //param.OrgId = param.OrgId != null ? param.OrgId : 1;

            var orgIds = await QueryData.ExecuteList(Procedures.PKG_COMMON_LIST_ORG,
                     new
                     {
                         P_IS_ADMIN = _appContext.IsAdmin == true ? 1 : 0,

                         P_ORG_ID = param.OrgId,
                         P_CURENT_USER_ID = _appContext.CurrentUserId,
                         P_CUR = QueryData.OUT_CURSOR
                     }, false);


            List<long?> ids = orgIds.Select(c => (long?)((dynamic)c).ID).ToList();
            if (param.OrgId != null)
            {
                ids.Add(param.OrgId);
            }
            queryable = queryable.Where(p => ids.Contains(p.OrgId));

            if (!string.IsNullOrWhiteSpace(param.Fullname))
            {
                queryable = queryable.Where(p => p.Fullname.ToUpper().Contains(param.Fullname.ToUpper()));
            }

            if (!string.IsNullOrWhiteSpace(param.Code))
            {
                queryable = queryable.Where(p => p.Code.ToUpper().Contains(param.Code.ToUpper()));
            }

            if (!string.IsNullOrWhiteSpace(param.OrgName))
            {
                queryable = queryable.Where(p => p.OrgName.ToUpper().Contains(param.OrgName.ToUpper()));
            }

            if (!string.IsNullOrWhiteSpace(param.PositionName))
            {
                queryable = queryable.Where(p => p.PositionName.ToUpper().Contains(param.PositionName.ToUpper()));
            }

            if (param.WorkStatusId == null || param.WorkStatusId == 0)
            {
                queryable = queryable.Where(p => p.WorkStatusId != OtherConfig.EMP_STATUS_TERMINATE || (p.WorkStatusId == OtherConfig.EMP_STATUS_TERMINATE && p.TerEffectDate > DateTime.Now));
            }
            return await PagingList(queryable, param);
        }

        /// <summary>
        /// API IMPORT 
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<ResultWithError> ImportProfile(EmpImportParam param)
        {
            try
            {
                param.Data.RemoveRange(0, 2);
                if (param.Data.Count == 0)
                {
                    return new ResultWithError(404);
                }
                var lst = new List<HU_EMPLOYEE_TMP>();
                var guid = Guid.NewGuid().ToString();
                var error = false;
                var lstError = new List<EmployeeInputImport>();
                foreach (var item in param.Data)
                {
                    var itemEmp = Map(item, new HU_EMPLOYEE_TMP());
                    if (string.IsNullOrWhiteSpace(item.Code))
                    {
                        error = true;
                        item.Code = "!Không được để trống";
                    }

                    if (string.IsNullOrWhiteSpace(item.OrgName))
                    {
                        error = true;
                        item.OrgName = "!Không được để trống";
                    }
                    else
                    {
                        var Org = item.OrgName.Split("-");
                        try
                        {
                            itemEmp.ORG_ID = int.Parse(Org[0]);
                        }
                        catch
                        {
                            error = true;
                            item.OrgName = "!Không đúng định dạng";
                        }
                    }
                    if (string.IsNullOrWhiteSpace(item.Position))
                    {
                        error = true;
                        item.Position = "!Không được để trống";
                    }
                    if (!string.IsNullOrWhiteSpace(item.BirthDate))
                    {
                        try
                        {
                            itemEmp.BIRTH_DATE_INPUT = DateTime.ParseExact(item.BirthDate, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                        }
                        catch
                        {
                            error = true;
                            item.BirthDate = "!Không đúng định dạng (dd/MM/yyyy)";
                        }
                    }
                    if (!string.IsNullOrWhiteSpace(item.IdDate))
                    {
                        try
                        {
                            itemEmp.ID_DATE_INPUT = DateTime.ParseExact(item.IdDate, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                        }
                        catch
                        {
                            error = true;
                            item.IdDate = "!Không đúng định dạng (dd/MM/yyyy)";
                        }
                    }
                    if (!string.IsNullOrWhiteSpace(item.PassDate))
                    {
                        try
                        {
                            itemEmp.PASS_DATE_INPUT = DateTime.ParseExact(item.PassDate, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                        }
                        catch
                        {
                            error = true;
                            item.PassDate = "!Không đúng định dạng (dd/MM/yyyy)";
                        }
                    }
                    if (!string.IsNullOrWhiteSpace(item.PassExpire))
                    {
                        try
                        {
                            itemEmp.PASS_EXPIRE_INPUT = DateTime.ParseExact(item.PassExpire, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                        }
                        catch
                        {
                            error = true;
                            item.PassExpire = "!Không đúng định dạng (dd/MM/yyyy)";
                        }
                    }
                    if (!string.IsNullOrWhiteSpace(item.VisaDate))
                    {
                        try
                        {
                            itemEmp.VISA_DATE_INPUT = DateTime.ParseExact(item.VisaDate, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                        }
                        catch
                        {
                            error = true;
                            item.VisaDate = "!Không đúng định dạng (dd/MM/yyyy)";
                        }
                    }
                    if (!string.IsNullOrWhiteSpace(item.VisaExpire))
                    {
                        try
                        {
                            itemEmp.VISA_EXPIRE_INPUT = DateTime.ParseExact(item.VisaExpire, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                        }
                        catch
                        {
                            error = true;
                            item.VisaExpire = "!Không đúng định dạng (dd/MM/yyyy)";
                        }
                    }
                    if (!string.IsNullOrWhiteSpace(item.WorkPermitDate))
                    {
                        try
                        {
                            itemEmp.WORK_PERMIT_DATE_INPUT = DateTime.ParseExact(item.WorkPermitDate, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                        }
                        catch
                        {
                            error = true;
                            item.WorkPermitDate = "!Không đúng định dạng (dd/MM/yyyy)";
                        }
                    }
                    if (!string.IsNullOrWhiteSpace(item.WorkPermitExpire))
                    {
                        try
                        {
                            itemEmp.WORK_PERMIT_EXPIRE_INPUT = DateTime.ParseExact(item.WorkPermitExpire, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                        }
                        catch
                        {
                            error = true;
                            item.WorkPermitDate = "!Không đúng định dạng (dd/MM/yyyy)";
                        }
                    }
                    if (error)
                    {
                        error = false;
                        lstError.Add(item);
                    }
                    else
                    {
                        itemEmp.REF_CODE = guid;
                        lst.Add(itemEmp);
                    }
                }

                if (lstError.Count > 0)
                {
                    var pathTemp = _appContext._config["urlTemplateProfile"];
                    var memoryStream = Template.FillTemp(pathTemp, lstError);
                    return new ResultWithError(memoryStream);
                }
                else
                {
                    if (lst.Count > 0)
                    {
                        await _appContext.EmployeeTmps.AddRangeAsync(lst);
                        await _appContext.SaveChangesAsync();
                        // xử lý fill dữ liệu vào master data
                        var ds = QueryData.ExecuteStoreToTable("PKG_PROFILE_IMPORT_EMP",
                        new
                        {

                            P_REF_CODE = guid,
                            P_CUR = QueryData.OUT_CURSOR
                        }, false);

                        if (ds.Tables.Count > 0)
                        {
                            ds.Tables[0].TableName = "Data";
                            var pathTemp = _appContext._config["urlTemplateProfile"];
                            var memoryStream = Template.FillTemplate(pathTemp, ds);
                            return new ResultWithError(memoryStream);
                        }
                        else
                        {
                            return new ResultWithError(200);
                        }
                    }
                }
                return new ResultWithError(200);
            }
            catch (Exception ex)
            {
                return new ResultWithError(204);
            }
        }

        public async Task<ResultWithError> TemplateImport()
        {
            try
            {
                // xử lý fill dữ liệu vào master data
                var ds = QueryData.ExecuteStoreToTable(Procedures.PKG_PROFILE_GET_DATA_IMPORT,
                new
                {

                    P_CUR_ORG = QueryData.OUT_CURSOR,
                    P_CUR_POS = QueryData.OUT_CURSOR
                }, false);

                if (ds.Tables[0].Rows.Count <= 0)
                {
                    return new ResultWithError("DATA_EMPTY");
                }
                ds.Tables[0].TableName = "OrgName";
                ds.Tables[1].TableName = "PosName";
                var pathTemp = _appContext._config["urlTemplateProfile"];
                var memoryStream = Template.FillReport(pathTemp, ds);
                return new ResultWithError(memoryStream);
            }
            catch (Exception ex)
            {
                return new ResultWithError(ex.Message);
            }
        }
        public async Task<ResultWithError> Delete(List<long> ids)
        {
            try
            {
                List<HU_EMPLOYEE> Emps = new();
                foreach (var id in ids)
                {
                    // kiểm tra coi là đã làm hợp đồng và quyết địng chưa
                    // hợp đồng
                    var contract = _appContext.Contracts.Where(x => x.EMPLOYEE_ID == id).Count();
                    if (contract > 0)
                    {
                        return new ResultWithError("CONTRACT_EXITS");
                    }

                    var decition = _appContext.Workings.Where(x => x.EMPLOYEE_ID == id).Count();
                    if (decition > 0)
                    {
                        return new ResultWithError("DECITION_EXITS");
                    }

                    var r = _appContext.Employees.Where(x => x.ID == id).FirstOrDefault();
                    Emps.Add(r);
                }
                _appContext.Employees.RemoveRange(Emps);
                await _appContext.SaveChangesAsync();
                return new ResultWithError(200);
            }
            catch (Exception ex)
            {
                return new ResultWithError(ex.Message);
            }
        }

        /// <summary>
        /// Get List paper by Emp
        /// </summary>
        /// <param name="param">EmpId</param>
        /// <returns></returns>
        public async Task<ResultWithError> GetListPaper(int EmpId)
        {

            try
            {
                var pos = await _appContext.Employees.Where(x => x.ID == EmpId).Select(x => x.POSITION_ID).FirstOrDefaultAsync();
                var r = await (from p in _appContext.OtherLists
                               join a in _appContext.PostionPaperses on p.ID equals a.PAPER_ID
                               from o in _appContext.EmployeePaperses.Where(c => c.PAPER_ID == p.ID && c.EMP_ID == EmpId).DefaultIfEmpty()
                               where a.POS_ID == pos
                               orderby p.CODE
                               select new ListPaperView
                               {
                                   Id = o.ID == null ? 0 : o.ID,
                                   TypeId = o.ID == null ? 0 : 1,
                                   PageName = p.NAME,
                                   DateInput = o.DATE_INPUT,
                                   statusId = o.STATUS_ID == 1,
                                   Url = o.URL,
                                   Note = o.NOTE,
                                   PaperId = p.ID
                               }).ToListAsync();
                return new ResultWithError(r);
            }
            catch (Exception ex)
            {
                return new ResultWithError(ex.Message);
            }

        }
        /// <summary>
        /// Create Data
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<ResultWithError> CreatePaperAsync(PaperInput param)
        {
            try
            {
                if (param.Id == 0)
                {
                    var data = Map(param, new HU_EMPLOYEE_PAPERS());
                    await _appContext.EmployeePaperses.AddAsync(data);
                    await _appContext.SaveChangesAsync();
                }
                else
                {
                    var r = await _appContext.EmployeePaperses.Where(x => x.ID == param.Id).FirstOrDefaultAsync();
                    var data = Map<PaperInput, HU_EMPLOYEE_PAPERS>(param, r);
                    _appContext.EmployeePaperses.Update(data);
                    await _appContext.SaveChangesAsync();
                }

                return new ResultWithError(200);
            }
            catch (Exception ex)
            {
                return new ResultWithError(ex);
            }
        }
        /// <summary>
        /// Get popup
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<PagedResult<EmployeePopup>> GetPopup(EmployeePopup param)
        {
            var queryable = from p in _appContext.Employees
                            join o in _appContext.Organizations on p.ORG_ID equals o.ID
                            join t in _appContext.Positions on p.POSITION_ID equals t.ID
                            from n in _appContext.OtherLists.Where(x => x.ID == p.Profile.NATIONALITY_ID).DefaultIfEmpty()
                            from o1 in _appContext.Organizations.Where(x => x.ID == o.PARENT_ID).DefaultIfEmpty()

                            orderby p.CODE
                            select new EmployeePopup
                            {
                                EmployeeId = p.ID,
                                EmployeeCode = p.CODE,
                                EmployeeName = p.Profile.FULL_NAME,
                                OrgId = p.ORG_ID,
                                OrgName = o.NAME,
                                PositionName = t.NAME,
                                PositionId = p.POSITION_ID,
                                OrgParentName = o1.NAME,
                                WorkStatusId = p.WORK_STATUS_ID,
                                TerEffectDate = p.TER_EFFECT_DATE,
                                ContractExpired = p.CONTRACT_EXPIRED,
                                WorkingId = p.LAST_WORKING_ID,
                                NationalityName = n.NAME
                            };

            //check phan quyen theo phong ban
            //param.OrgId = param.OrgId != null ? param.OrgId : 1;

            var orgIds = await QueryData.ExecuteList(Procedures.PKG_COMMON_LIST_ORG,
                     new
                     {
                         P_IS_ADMIN = _appContext.IsAdmin == true ? 1 : 0,

                         P_ORG_ID = param.OrgId,
                         P_CURENT_USER_ID = _appContext.CurrentUserId,
                     }, false);


            List<long?> ids = orgIds.Select(c => (long?)((dynamic)c).ID).ToList();
            if (param.OrgId != null)
            {
                ids.Add(param.OrgId);
            }
            queryable = queryable.Where(p => ids.Contains(p.OrgId));
            if (param.EmployeeId != null)
            {
                queryable = queryable.Where(p => p.EmployeeId == param.EmployeeId);
            }


            if (!string.IsNullOrWhiteSpace(param.EmployeeName))
            {
                queryable = queryable.Where(p => p.EmployeeName.ToUpper().Contains(param.EmployeeName.ToUpper()));
            }

            if (!string.IsNullOrWhiteSpace(param.EmployeeCode))
            {
                queryable = queryable.Where(p => p.EmployeeCode.ToUpper().Contains(param.EmployeeCode.ToUpper()));
            }

            if (!string.IsNullOrWhiteSpace(param.OrgName))
            {
                queryable = queryable.Where(p => p.OrgName.ToUpper().Contains(param.OrgName.ToUpper()));
            }

            if (!string.IsNullOrWhiteSpace(param.PositionName))
            {
                queryable = queryable.Where(p => p.PositionName.ToUpper().Contains(param.PositionName.ToUpper()));
            }

            if (param.WorkStatusId == null || param.WorkStatusId == 0)
            {
                queryable = queryable.Where(p => p.WorkStatusId != OtherConfig.EMP_STATUS_TERMINATE || (p.WorkStatusId == OtherConfig.EMP_STATUS_TERMINATE && p.TerEffectDate > DateTime.Now));
            }

            return await PagingList(queryable, param);
        }

        /// <summary>
        /// PortalContactBook
        /// </summary>
        /// <returns></returns>
        public async Task<ResultWithError> PortalContactBook(string name)
        {

            try
            {
                var r = await QueryData.ExecuteStore<PhoneBookView>(Procedures.PKG_PROFILE_PHONE_BOOK, new
                {

                    P_EMP_ID = _appContext.EmpId,
                    P_NAME = name,
                    P_CUR = QueryData.OUT_CURSOR
                });
                return new ResultWithError(r);
            }
            catch (Exception ex)
            {
                return new ResultWithError(ex.Message);
            }
        }

        public async Task<ResultWithError> PortalGetContact(int EmpId)
        {
            try
            {
                var r = await QueryData.ExecuteStore<PhoneBookView>(Procedures.PKG_PROFILE_PHONE_BOOK_DTL, new
                {

                    P_EMP_ID = EmpId,
                    P_CUR = QueryData.OUT_CURSOR
                });
                return new ResultWithError(r);
            }
            catch (Exception ex)
            {
                return new ResultWithError(ex.Message);
            }
        }

        public async Task<ResultWithError> ScanQRCode()
        {
            try
            {

                var r = await (from p in _appContext.Employees
                               join s in _appContext.Positions on p.POSITION_ID equals s.ID
                               where p.ID == _appContext.EmpId
                               select new
                               {
                                   FullName = p.Profile.FULL_NAME,
                                   Title = s.NAME,
                                   Phone = p.Profile.MOBILE_PHONE,
                                   Email = p.Profile.EMAIL,
                                   Link = ""
                               }).FirstOrDefaultAsync();
                return new ResultWithError(r);
            }
            catch (Exception ex)
            {
                return new ResultWithError(ex.Message);
            }

        }
        /// <summary>
        /// Portal update Infomation
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<ResultWithError> PortalEditInfomation(EmployeeEditInput param)
        {

            try
            {
                var r = await (from p in _appContext.EmployeeEdits
                               where p.EMPLOYEE_ID == _appContext.EmpId && p.STATUS == OtherConfig.STATUS_WAITING
                               select p).FirstOrDefaultAsync();
                // delete
                if (r != null) _appContext.EmployeeEdits.Remove(r);
                var data = Map(param, new HU_EMPLOYEE_EDIT());
                data.EMPLOYEE_ID = _appContext.EmpId;
                data.STATUS = 1;
                // insert
                _appContext.EmployeeEdits.Add(data);
                await _appContext.SaveChangesAsync();
                return new ResultWithError(200);
            }
            catch (Exception ex)
            {
                return new ResultWithError(ex);
            }
        }

        public async Task<ResultWithError> EditInfomationBy(int id)
        {

            try
            {
                var r = await (from p in _appContext.EmployeeEdits
                               from c2 in _appContext.Contracts.Where(x => x.ID == p.CONTRACT_ID).DefaultIfEmpty()
                               join e in _appContext.Employees on p.EMPLOYEE_ID equals e.ID
                               join t in _appContext.Positions on e.POSITION_ID equals t.ID
                               join o in _appContext.Organizations on e.ORG_ID equals o.ID
                               from learn in _appContext.OtherLists.Where(x => x.ID == p.LEARNING_LEVEL_ID).DefaultIfEmpty()
                               from train in _appContext.OtherLists.Where(x => x.ID == p.TRAINING_FORM_ID).DefaultIfEmpty()
                               from dicur in _appContext.Districts.Where(x => x.ID == p.CUR_DISTRICT_ID).DefaultIfEmpty()
                               from pvcur in _appContext.Provinces.Where(x => x.ID == p.CUR_PROVINCE_ID).DefaultIfEmpty()
                               from re in _appContext.OtherLists.Where(x => x.ID == p.RELIGION_ID).DefaultIfEmpty()
                               from na in _appContext.OtherLists.Where(x => x.ID == p.NATIVE_ID).DefaultIfEmpty()
                               from nat in _appContext.OtherLists.Where(x => x.ID == p.NATIONALITY_ID).DefaultIfEmpty()
                               from pos2 in _appContext.Positions.Where(x => x.ID == p.POSITION_ID).DefaultIfEmpty()
                               from gt in _appContext.OtherLists.Where(x => x.ID == p.GENDER_ID).DefaultIfEmpty()
                               from pv in _appContext.Provinces.Where(x => x.ID == p.PROVINCE_ID).DefaultIfEmpty()
                               from wa in _appContext.Wards.Where(x => x.ID == p.WARD_ID).DefaultIfEmpty()
                               from di in _appContext.Districts.Where(x => x.ID == p.DISTRICT_ID).DefaultIfEmpty()

                               where p.ID == id
                               select new
                               {
                                   // bản thân

                                   Code = e.CODE,
                                   Fullname = e.Profile.FULL_NAME,
                                   GenderId = p.GENDER_ID,
                                   LastName = p.LAST_NAME,
                                   FirstName = p.FIRST_NAME,
                                   Avatar = e.Profile.AVATAR,
                                   PName = t.NAME,
                                   OrgName = o.NAME,
                                   BirthDate = p.BIRTH_DATE,
                                   IdDate = p.ID_DATE,
                                   IdPlace = p.ID_PLACE,
                                   IdNo = p.ID_NO,

                                   ProvinceId = p.PROVINCE_ID,
                                   DistrictId = p.DISTRICT_ID,
                                   WardId = p.WARD_ID,

                                   Id = p.ID,
                                   OrgId = p.ORG_ID,
                                   PositionId = p.POSITION_ID,
                                   PositionName = pos2.NAME,
                                   DirectManagerId = p.DIRECT_MANAGER_ID,
                                   GenderName = gt.NAME,
                                   ProvinceName = pv.NAME,
                                   DistrictName = di.NAME,
                                   WardName = wa.NAME,

                                   //Thông tin cá nhân
                                   ReligionId = p.RELIGION_ID,
                                   ReligionName = re.NAME,
                                   NativeId = p.NATIVE_ID,
                                   NativeName = na.NAME,
                                   NationalityId = p.NATIONALITY_ID,
                                   NationalityName = nat.NAME,
                                   Address = p.ADDRESS,
                                   BirthPlace = p.BIRTH_PLACE,
                                   JoinDate = p.JOIN_DATE,
                                   WorkStatusId = p.WORK_STATUS_ID,

                                   ContractId = p.CONTRACT_ID,
                                   ContractCode = c2.CONTRACT_NO,
                                   ContractDateEffect = c2.START_DATE,
                                   ContractDateExpire = c2.EXPIRE_DATE,
                                   LastWorkingId = p.LAST_WORKING_ID,
                                   TerEffectDate = p.TER_EFFECT_DATE,
                                   ItimeId = p.ITIME_ID, // Mã chấm công
                                   ObjectSalaryId = p.SALARY_TYPE_ID,
                                   TaxCode = p.TAX_CODE,
                                   MobilePhone = p.MOBILE_PHONE,
                                   WorkEmail = p.WORK_EMAIL,
                                   Email = p.EMAIL,



                                   //Hộ chiếu
                                   MaritalStatusId = p.MARITAL_STATUS_ID,
                                   PassNo = p.PASS_NO, // Số hộ chiếu
                                   PassDate = p.PASS_DATE, // Ngày cấp
                                   PassExpire = p.PASS_EXPIRE, // Ngày hết hạn
                                   PassPlace = p.PASS_PLACE,  // Nơi cấp

                                   //Visa
                                   VisaNo = p.VISA_NO,
                                   VisaDate = p.VISA_DATE,
                                   VisaExpire = p.VISA_EXPIRE,
                                   VisaPlace = p.VISA_PLACE,

                                   //Giấy phép lao động
                                   WorkPermit = p.WORK_PERMIT,
                                   WorkPermitDate = p.WORK_PERMIT_DATE,
                                   WorkPermitExpire = p.WORK_PERMIT_EXPIRE,
                                   WorkPermitPlace = p.WORK_PERMIT_PLACE,
                                   ContactPer = p.CONTACT_PER,
                                   ContactPerPhone = p.CONTACT_PER_PHONE,

                                   // Tài khoản
                                   BankId = p.BANK_ID,
                                   //BankName = bank.NAME,
                                   BankBranch = p.BANK_BRANCH,
                                   BankNo = p.BANK_NO,

                                   // Trình độ học vấn
                                   SchoolName = p.SCHOOLNAME,
                                   SchoolId = p.SCHOOL_ID,
                                   QualificationId = p.QUALIFICATION_ID,
                                   TrainingFormId = p.TRAINING_FORM_ID,
                                   TrainingFormName = train.NAME,
                                   LearningLevelId = p.LEARNING_LEVEL_ID,
                                   LearningLevelName = learn.NAME,
                                   Language = p.LANGUAGE,
                                   LanguageMark = p.LANGUAGE_MARK,
                                   ResidentId = p.RESIDENT_ID,
                                   SalTotal = p.SAL_TOTAL,

                                   //Địa chỉ thường trú
                                   CurProvinceId = p.CUR_PROVINCE_ID,
                                   CurProvinceName = pvcur.NAME,
                                   CurDistrictId = p.CUR_DISTRICT_ID,
                                   CurDistrictName = dicur.NAME,
                                   CurWardId = p.CUR_WARD_ID,
                                   CurAddress = p.CUR_ADDRESS,

                                   //Chứng chỉ hành nghề
                                   WorkNo = p.WORK_NO,
                                   WorkDate = p.WORK_DATE,
                                   WorkScope = p.WORK_SCOPE,
                                   WorkPlace = p.WORK_PLACE



                               }).FirstOrDefaultAsync();

                return new ResultWithError(r);
            }
            catch (Exception ex)
            {
                return new ResultWithError(ex);
            }
        }
        public async Task<ResultWithError> ApproveEditInfomation(int id)
        {

            try
            {
                var r = await _appContext.EmployeeEdits.Where(x => x.ID == id).FirstOrDefaultAsync();
                if (r == null)
                {
                    return new ResultWithError(404);
                }
                // delete
                r.STATUS = 1;
                // insert
                _appContext.EmployeeEdits.Update(r);
                // update sang employee

                _appContext.SaveChanges();
                return new ResultWithError(200);
            }
            catch (Exception ex)
            {
                return new ResultWithError(ex);
            }
        }
        public async Task<ResultWithError> PortalAddSituation(SituationEditDTO param)
        {
            try
            {
                var data = Map(param, new HU_FAMILY_EDIT());
                //data.STATUS = 1;
                data.EMPLOYEE_ID = _appContext.EmpId;
                await _appContext.SituationEdits.AddAsync(data);
                await _appContext.SaveChangesAsync();
                return new ResultWithError(200);
            }
            catch (Exception ex)
            {
                return new ResultWithError(ex);
            }
        }
        /// <summary>
        /// CMS Get All Data User edit Profile On Portal
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<PagedResult<EmployeeEditDTO>> GetAllProfileEdit(EmployeeEditDTO param)
        {

            var queryable = from p in _appContext.EmployeeEdits
                            join e in _appContext.Employees on p.EMPLOYEE_ID equals e.ID
                            join o in _appContext.Organizations on e.ORG_ID equals o.ID
                            join t in _appContext.Positions on e.POSITION_ID equals t.ID
                            join s in _appContext.OtherListFixs.Where(c => c.TYPE == SystemConfig.STATUS_APPROVE) on p.STATUS equals s.ID

                            orderby p.STATUS, e.CODE
                            select new EmployeeEditDTO
                            {
                                Id = p.ID,
                                Code = e.CODE,
                                Fullname = e.Profile.FULL_NAME,
                                Image = e.Profile.IMAGE,
                                OrgId = e.ORG_ID,
                                OrgName = o.NAME,
                                PositionName = t.NAME,
                                CreateDate = p.CREATED_DATE,
                                UpdatedBy = p.UPDATED_BY,
                                UpdatedDate = p.UPDATED_DATE,
                                StatusName = s.NAME,
                                StatusId = p.STATUS
                            };
            var orgIds = await QueryData.ExecuteList(Procedures.PKG_COMMON_LIST_ORG,
                     new
                     {
                         P_IS_ADMIN = _appContext.IsAdmin == true ? 1 : 0,
                         P_ORG_ID = param.OrgId,
                         P_CURENT_USER_ID = _appContext.CurrentUserId,
                     }, false);


            List<long?> ids = orgIds.Select(c => (long?)((dynamic)c).ID).ToList();
            if (param.OrgId != null)
            {
                ids.Add(param.OrgId);
            }
            queryable = queryable.Where(p => ids.Contains(p.OrgId));


            if (!string.IsNullOrWhiteSpace(param.Fullname))
            {
                queryable = queryable.Where(p => p.Fullname.ToUpper().Contains(param.Fullname.ToUpper()));
            }

            if (!string.IsNullOrWhiteSpace(param.Code))
            {
                queryable = queryable.Where(p => p.Code.ToUpper().Contains(param.Code.ToUpper()));
            }

            if (!string.IsNullOrWhiteSpace(param.OrgName))
            {
                queryable = queryable.Where(p => p.OrgName.ToUpper().Contains(param.OrgName.ToUpper()));
            }

            if (!string.IsNullOrWhiteSpace(param.PositionName))
            {
                queryable = queryable.Where(p => p.PositionName.ToUpper().Contains(param.PositionName.ToUpper()));
            }
            if (!string.IsNullOrWhiteSpace(param.StatusName))
            {
                queryable = queryable.Where(p => p.StatusName.ToUpper().Contains(param.StatusName.ToUpper()));
            }

            return await PagingList(queryable, param);
        }
        /// <summary>
        /// CMS Get All Data User Add family On Portal
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<PagedResult<FamilyEditDTO>> GetAllFamilyAdd(FamilyEditDTO param)
        {

            var queryable = from p in _appContext.SituationEdits
                            join e in _appContext.Employees on p.EMPLOYEE_ID equals e.ID
                            join o in _appContext.Organizations on e.ORG_ID equals o.ID
                            join t in _appContext.Positions on e.POSITION_ID equals t.ID
                            join o1 in _appContext.OtherLists on p.RELATIONSHIP_ID equals (int)o1.ID
                            //join s in _appContext.OtherListFixs.Where(c => c.TYPE == SystemConfig.STATUS_APPROVE) on p.STATUS equals s.ID

                            //orderby p.STATUS, e.CODE
                            select new FamilyEditDTO
                            {
                                Id = p.ID,
                                Avatar = e.Profile.AVATAR,
                                Code = e.CODE,
                                Fullname = e.Profile.FULL_NAME,
                                OrgId = e.ORG_ID,
                                OrgName = o.NAME,
                                PName = t.NAME,
                                RelationName = o1.NAME,
                                CreateDate = p.CREATED_DATE,
                                UpdatedBy = p.UPDATED_BY,
                                UpdatedDate = p.UPDATED_DATE,
                                //StatusName = s.NAME,
                                //Name = p.NAME,
                                //BirthDay = p.BIRTH,
                                //No = p.NO
                            };
            var orgIds = await QueryData.ExecuteList(Procedures.PKG_COMMON_LIST_ORG,
                     new
                     {
                         P_IS_ADMIN = _appContext.IsAdmin == true ? 1 : 0,

                         P_ORG_ID = param.OrgId,
                         P_CURENT_USER_ID = _appContext.CurrentUserId,
                         P_CUR = QueryData.OUT_CURSOR
                     }, false);


            List<long?> ids = orgIds.Select(c => (long?)((dynamic)c).ID).ToList();
            if (param.OrgId != null)
            {
                ids.Add(param.OrgId);
            }
            queryable = queryable.Where(p => ids.Contains(p.OrgId));


            if (!string.IsNullOrWhiteSpace(param.Fullname))
            {
                queryable = queryable.Where(p => p.Fullname.ToUpper().Contains(param.Fullname.ToUpper()));
            }

            if (!string.IsNullOrWhiteSpace(param.Code))
            {
                queryable = queryable.Where(p => p.Code.ToUpper().Contains(param.Code.ToUpper()));
            }

            if (!string.IsNullOrWhiteSpace(param.OrgName))
            {
                queryable = queryable.Where(p => p.OrgName.ToUpper().Contains(param.OrgName.ToUpper()));
            }

            if (!string.IsNullOrWhiteSpace(param.Name))
            {
                queryable = queryable.Where(p => p.Name.ToUpper().Contains(param.Name.ToUpper()));
            }
            if (!string.IsNullOrWhiteSpace(param.RelationName))
            {
                queryable = queryable.Where(p => p.RelationName.ToUpper().Contains(param.RelationName.ToUpper()));
            }
            if (!string.IsNullOrWhiteSpace(param.StatusName))
            {
                queryable = queryable.Where(p => p.StatusName.ToUpper().Contains(param.StatusName.ToUpper()));
            }

            return await PagingList(queryable, param);
        }
        /// <summary>
        /// CMS Get All Data User edit Profile On Portal
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<ResultWithError> ApproveProfileEdit(int id, int type)
        {
            try
            {
                var r = await _appContext.EmployeeEdits.Where(x => x.ID == id).FirstOrDefaultAsync();
                if (r == null)
                {
                    return new ResultWithError(404);
                }
                r.STATUS = type;
                if (type == OtherConfig.STATUS_APPROVE) // appro   
                {
                    r.STATUS = OtherConfig.STATUS_APPROVE;
                    var empId = r.EMPLOYEE_ID;
                    var e = await _appContext.Employees.Where(x => x.ID == empId).FirstOrDefaultAsync();
                    r.ID = null;
                    var data = Map<HU_EMPLOYEE_EDIT, HU_EMPLOYEE>(r, e);
                    _appContext.Employees.Update(data);
                }
                else
                {
                    r.STATUS = OtherConfig.STATUS_DECLINE;
                }
                r.ID = id;
                r.UPDATED_DATE = DateTime.Now;
                r.UPDATED_BY = _appContext.UserName;
                _appContext.EmployeeEdits.Update(r);
                await _appContext.SaveChangesAsync();
                return new ResultWithError(200);
            }
            catch (Exception ex)
            {
                return new ResultWithError(ex.Message);
            }
        }


        #region Danh ba nhan su

        public async Task<GenericPhaseTwoListResponse<HuEmployeeDTO>> QueryListPersonnelDirectory(GenericQueryListDTO<HuEmployeeDTO> request)
        {

            if (request.InOperators == null)
            {
                return new()
                {
                    ErrorType = EnumErrorType.CATCHABLE,
                    MessageCode = CommonMessageCode.THIS_QUERY_REQUIRES_IN_OPERATOR,
                    ErrorPhase = 0
                };
            }

            if (!request.InOperators.Any(x => x.Field.ToLower() == "orgid"))
            {
                return new()
                {
                    ErrorType = EnumErrorType.CATCHABLE,
                    MessageCode = CommonMessageCode.THIS_QUERY_REQUIRES_ORG_ID_IN_OPERATOR,
                    ErrorPhase = 0
                };
            }

            var joined = from p in _appContext.Employees.AsNoTracking()
                         from t in _appContext.Positions.AsNoTracking().Where(c => c.ID == p.POSITION_ID).DefaultIfEmpty()
                         from sysOff in _appContext.OtherLists.AsNoTracking().Where(c => c.NAME == "ESQ").DefaultIfEmpty()
                         //from nddv in _appContext.Positions.AsNoTracking().Where(c => c.ID == t.ID && t.NAME == "Người đại diện vốn" && p.WORK_STATUS_ID != sysOff.ID).DefaultIfEmpty()
                         from o in _appContext.Organizations.AsNoTracking().Where(c => c.ID == t.ORG_ID).DefaultIfEmpty()
                         from g in _appContext.OtherLists.AsNoTracking().Where(c => c.ID == p.Profile!.GENDER_ID).DefaultIfEmpty()
                         from v in _appContext.OtherLists.AsNoTracking().Where(c => c.ID == p.WORK_STATUS_ID).DefaultIfEmpty()
                         from c in _appContext.Contracts.Where(c => c.ID == p.CONTRACT_ID).DefaultIfEmpty()
                         from j in _appContext.HUJobs.Where(x => x.ID == t.JOB_ID).DefaultIfEmpty()
                         orderby j.ORDERNUM
                         select new HuEmployeeDTO
                         {
                             Id = p.ID,
                             Code = p.CODE,
                             Fullname = p.Profile!.FULL_NAME,
                             OrgId = p.ORG_ID,
                             OrgName = o.NAME,
                             PositionName = t.NAME,
                             JobOrderNum = Convert.ToInt32(j.ORDERNUM ?? 999),
                             GenderName = g.NAME,
                             BirthDate = p.Profile!.BIRTH_DATE,
                             WorkStatusId = v.ID,
                             WorkStatusName = v.NAME,
                             MobilePhone = p.Profile!.MOBILE_PHONE,
                             PresenterPhoneNumber = p.Profile!.PRESENTER_PHONE_NUMBER,
                             ContractExpired = p.CONTRACT_EXPIRED,
                             WorkEmail = p.Profile!.WORK_EMAIL,
                             Email = p.Profile!.EMAIL,
                             IsLeaveWork = v.CODE == "ESQ" ? true : false,  // lọc nhân viên có trạng thái nghỉ việc
                             DateStart = c.START_DATE,
                             JoinDate = p.JOIN_DATE,
                         };

            var singlePhaseResult = await genericReducer.SinglePhaseReduce(joined, request);
            return singlePhaseResult;
        }

        public async Task<FormatedResponse> GetPersonnelDirectoryById(long Id)
        {
            var joined = await (from p in _appContext.Employees.AsNoTracking()
                                from o in _appContext.Organizations.Where(x => x.ID == p.ORG_ID).DefaultIfEmpty()
                                from pos in _appContext.Positions.Where(x => x.ID == p.POSITION_ID).DefaultIfEmpty()
                                from g in _appContext.OtherLists.AsNoTracking().Where(c => c.ID == p.Profile!.GENDER_ID).DefaultIfEmpty()

                                where p.ID == Id
                                select new
                                {
                                    EmployeeId = p.ID,
                                    EmployeeCode = p.CODE,
                                    EmployeeName = p.Profile!.FULL_NAME,
                                    PositionId = p.POSITION_ID,
                                    PositionName = pos.NAME,
                                    OrgId = p.ORG_ID,
                                    OrgName = o.NAME,
                                    GenderName = g.NAME,
                                    BirthDate = p.Profile!.BIRTH_DATE,
                                    WorkEmail = p.Profile!.WORK_EMAIL,
                                    Email = p.Profile!.EMAIL,
                                    MobilePhone = p.Profile!.MOBILE_PHONE,
                                }).FirstAsync();

            if (joined != null)
            {
                return new FormatedResponse() { InnerBody = joined };
            }
            else
            {
                return new FormatedResponse() { MessageCode = CommonMessageCode.ENTITY_NOT_FOUND, ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400 };
            }

        }

        #endregion



    }
}
