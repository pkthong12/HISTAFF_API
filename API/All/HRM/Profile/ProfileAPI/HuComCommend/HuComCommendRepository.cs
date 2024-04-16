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
    public class HuComCommendRepository : IHuComCommendRepository
    {
        private readonly GenericUnitOfWork _uow;
        private readonly FullDbContext _dbContext;
        private IGenericRepository<HU_COM_COMMEND, HuComCommendDTO> _genericRepository;
        private readonly GenericReducer<HU_COM_COMMEND, HuComCommendDTO> _genericReducer;

        public HuComCommendRepository(FullDbContext context, GenericUnitOfWork uow)
        {
            _dbContext = context;
            _uow = uow;
            _genericRepository = _uow.GenericRepository<HU_COM_COMMEND, HuComCommendDTO>();
            _genericReducer = new();
        }

        public async Task<GenericPhaseTwoListResponse<HuComCommendDTO>> SinglePhaseQueryList(GenericQueryListDTO<HuComCommendDTO> request)
        {
            var huCommend = _uow.Context.Set<HU_COMMEND>().AsNoTracking().AsQueryable();
            var huCommendEmployee =_uow.Context.Set<HU_COMMEND_EMPLOYEE>().AsNoTracking().AsQueryable();
            var otherList = _uow.Context.Set<SYS_OTHER_LIST>().AsNoTracking().AsQueryable();
            var position = _uow.Context.Set<HU_POSITION>().AsNoTracking().AsQueryable();
            var employee = _uow.Context.Set<HU_EMPLOYEE>().AsNoTracking().AsQueryable();
            var organization = _uow.Context.Set<HU_ORGANIZATION>().AsNoTracking().AsQueryable();
            var orgLevel = _uow.Context.Set<HU_ORG_LEVEL>().AsNoTracking().AsQueryable();
            var job = _uow.Context.Set<HU_JOB>().AsNoTracking().AsQueryable();  

            var joined = (from ce in huCommendEmployee
                         from c in huCommend.Where(x => x.ID == ce.COMMEND_ID)
                         from e in employee.Where(x => x.ID == ce.EMPLOYEE_ID).DefaultIfEmpty()
                         from cObj in otherList.Where(x => x.ID == c.COMMEND_OBJ_ID).DefaultIfEmpty()
                         from rLevel in otherList.Where(x => x.ID == c.REWARD_LEVEL_ID).DefaultIfEmpty()
                         from oLevel in otherList.Where(x => x.ID == c.ORG_LEVEL_ID).DefaultIfEmpty()
                         from s in otherList.Where(x => x.ID == c.STATUS_PAYMENT_ID).DefaultIfEmpty()
                         from p in position.Where(x => x.ID == e.POSITION_ID).DefaultIfEmpty()
                         from o in organization.Where(x => x.ID == e.ORG_ID).DefaultIfEmpty()
                         from ol in orgLevel.Where(x => x.ID == c.ORG_LEVEL_ID).DefaultIfEmpty()
                         from j in job.Where(x => x.ID == p.JOB_ID).DefaultIfEmpty()
                         orderby c.CREATED_DATE descending
                         // JOIN OTHER ENTITIES BASED ON THE BUSINESS
                         select new HuComCommendDTO
                         {
                             Id = ce.ID,
                            
                         }).ToList();


            // checkbox to string
            var queryList = _uow.Context.Set<HU_COMMEND>()
                                .ToList()
                                .DistinctBy(x => x.ID)
                                .Select(x => {
                                    return new
                                    {
                                        ID = x.ID,
                                        STR = (x.LIST_REWARD_LEVEL_ID != null) ? x.LIST_REWARD_LEVEL_ID : ""
                                    };
                                })
                                .Select(x =>
                                {
                                    return new
                                    {
                                        ID = x.ID,
                                        LIST_LONG = x.STR.Split(", ").Select(item => (item != "") ? long.Parse(item) : -1)
                                    };
                                })
                                .Select(x =>
                                {
                                    var list_org_level_str = orgLevel
                                                .Where(item => x.LIST_LONG.Contains(item.ID))
                                                .Select(item => item.NAME)
                                                .ToList();

                                    return new
                                    {
                                        ID = x.ID,
                                        NAME = string.Join(", ", list_org_level_str)
                                    };
                                })
                                .ToList();


            //foreach (var item_main in joined)
            //{
            //    foreach (var item_sub in queryList)
            //    {
            //        if (item_main.CommendId == item_sub.ID)
            //        {
            //            item_main.RewardLevelName = item_sub.NAME;
            //        }
            //    }
            //}


            var singlePhaseResult = await _genericReducer.SinglePhaseReduce(joined.AsQueryable(), request);
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
            var commend = _uow.Context.Set<HU_COMMEND>().AsNoTracking().AsQueryable();
            var otherList = _uow.Context.Set<SYS_OTHER_LIST>().AsNoTracking().AsQueryable();
            var commendEmployee = _uow.Context.Set<HU_COMMEND_EMPLOYEE>().AsNoTracking().AsQueryable();
            var employee = _uow.Context.Set<HU_EMPLOYEE>().AsNoTracking().AsQueryable();
            var organization = _uow.Context.Set<HU_ORGANIZATION>().AsNoTracking().AsQueryable();
            var orgLevel = _uow.Context.Set<HU_ORG_LEVEL>().AsNoTracking().AsQueryable();
            var position = _uow.Context.Set<HU_POSITION>().AsNoTracking().AsQueryable();
            var res = await _genericRepository.GetById(id);
            if (res.InnerBody != null)
            {
                var response = res.InnerBody;
                var list = new List<HU_COMMEND>
                    {
                        (HU_COMMEND)response
                    };

                var joined = (from l in list
                                  // JOIN OTHER ENTITIES BASED ON THE BUSINESS
                              select new 
                              {
                                  Id = l.ID,
                                  EffectDate = l.EFFECT_DATE,
                                  No = l.NO,
                                  SignDate = l.SIGN_DATE,
                                  SignPaymentDate = l.SIGN_PAYMENT_DATE,
                                  SignerName = l.SIGNER_NAME,
                                  SignerPosition = l.SIGNER_POSITION,
                                  OrgId = l.ORG_ID,
                                  CommendObjId = l.COMMEND_OBJ_ID,
                                  Reason = l.REASON,
                                  Money = l.MONEY,
                                  IsTax = l.IS_TAX,
                                  Year = l.YEAR,
                                  Content = l.CONTENT,
                                  EmployeeName = l.EMPLOYEE_NAME,
                                  EmployeeCode = l.EMPLOYEE_CODE,
                                  PositionName = l.POSITION_NAME,
                                  OrgName = l.ORG_NAME,
                                  OrgLevelId = l.ORG_LEVEL_ID,
                                  RewardId = l.REWARD_ID,
                                  RewardLevelId = l.REWARD_LEVEL_ID,
                                  SalaryIncreaseTime = l.SALARY_INCREASE_TIME,
                                  PaymentNo = l.PAYMENT_NO,
                                  StatusPaymentId = l.STATUS_PAYMENT_ID,
                                  SignId = l.SIGN_ID,
                                  SignPaymentId = l.SIGN_PAYMENT_ID,
                                  PositionPaymentName = l.POSITION_PAYMENT_NAME,
                                  Attachment = l.ATTACHMENT,
                                  PaymentAttachment = l.PAYMENT_ATTACHMENT,
                                  FundSourceId = l.FUND_SOURCE_ID,
                                  SignPaymentName = l.SIGN_PAYMENT_NAME,
                                  PaymentContent = l.PAYMENT_CONTENT,
                                  AwardTitleId = l.AWARD_TITLE_ID,
                                  MonthTax = l.MONTH_TAX,
                                  Note = l.NOTE,
                                  EmployeeList = new List<EmployeeDTO>(),
                                  EmployeeIds = (from ce in commendEmployee.Where(x => x.COMMEND_ID == id) select ce.EMPLOYEE_ID).ToList(),
                                  CheckListRewardLevel = l.LIST_REWARD_LEVEL_ID.Split(new char[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToList()

                              }).FirstOrDefault();



                var employeeList = (from c in commend
                                    from ce in commendEmployee.Where(x => x.COMMEND_ID == c.ID)
                                    from e in employee.Where(x => x.ID == ce.EMPLOYEE_ID)
                                    from o in organization.Where(x => x.ID == e.ORG_ID).DefaultIfEmpty()
                                    from po in position.Where(x => x.ID == e.POSITION_ID).DefaultIfEmpty()
                                    from g in otherList.Where(x => x.ID == e.Profile.GENDER_ID).DefaultIfEmpty()
                                    where ce.COMMEND_ID == id
                                    select new EmployeeDTO()
                                    {
                                        Id = e.ID,
                                        Fullname = e.Profile.FULL_NAME,
                                        Avatar = e.Profile.AVATAR,
                                        Code = e.CODE,
                                        PositionName = po.NAME,
                                        OrgName = o.NAME,
                                        JoinDate = e.JOIN_DATE,
                                        BirthDate = e.Profile.BIRTH_DATE,
                                        BirthPlace = e.Profile.BIRTH_PLACE,
                                        GenderName = g.NAME,
                                        MobilePhone = e.Profile.MOBILE_PHONE,
                                        Email = e.Profile.EMAIL,
                                        IdNo = e.Profile.ID_NO,
                                        IdDate = e.Profile.ID_DATE
                                    }).ToList();
                joined?.EmployeeList.AddRange(employeeList);

                return new FormatedResponse() { InnerBody = joined };
            }
            else
            {
                return new FormatedResponse() { MessageCode = CommonMessageCode.DATA_HAVE_BEEN_IN_DATABASE, ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400 };
            }
        }

        public async Task<FormatedResponse> GetById(string id)
        {
            await Task.Run(() => null);
            throw new NotImplementedException();
        }

        public async Task<FormatedResponse> Create(GenericUnitOfWork _uow, HuComCommendDTO dto, string sid)
        {
            var response = await _genericRepository.Create(_uow, dto, sid);
            return response;
        }

        public async Task<FormatedResponse> CreateRange(GenericUnitOfWork _uow, List<HuComCommendDTO> dtos, string sid)
        {
            var add = new List<HuComCommendDTO>();
            add.AddRange(dtos);
            var response = await _genericRepository.CreateRange(_uow, add, sid);
            return response;
        }

        public async Task<FormatedResponse> Update(GenericUnitOfWork _uow, HuComCommendDTO dto, string sid, bool patchMode = true)
        {
            var response = await _genericRepository.Update(_uow, dto, sid, patchMode);
            return response;
        }

        public async Task<FormatedResponse> UpdateRange(GenericUnitOfWork _uow, List<HuComCommendDTO> dtos, string sid, bool patchMode = true)
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
        public async Task<FormatedResponse> ApproveCommend(List<long> Ids)
        {
            try
            {
                string sid = "";
                bool pathMode = true;
                List<HuComCommendDTO> list = new();
                Ids.ForEach(item =>
                {
                    var response = _uow.Context.Set<HU_COMMEND>().Where(x => x.ID == item).FirstOrDefault();
                    var otherList = _dbContext.SysOtherLists.Where(x => x.CODE == "DD").FirstOrDefault();
                    response.STATUS_PAYMENT_ID = otherList.ID;

                    list.Add(new HuComCommendDTO()
                    {
                        Id = response.ID,
                    });
                });
                var approved = await _genericRepository.UpdateRange(_uow, list,sid,pathMode);
                if (approved != null)
                {
                    return new FormatedResponse() { InnerBody = approved };
                }
                else
                {
                   return new FormatedResponse() { MessageCode = CommonMessageCode.ENTITY_NOT_FOUND, ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400 };
                }
            }
            catch (Exception ex)
            {

                return new FormatedResponse() { ErrorType = EnumErrorType.UNCATCHABLE, StatusCode = EnumStatusCode.StatusCode500, MessageCode = ex.Message };
            }
        }

    }
}

