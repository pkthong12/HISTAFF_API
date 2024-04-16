using CORE.GenericUOW;
using CORE.DTO;
using API.DTO;
using CORE.Enum;
using CORE.StaticConstant;
using API.All.DbContexts;
using CORE.AutoMapper;
using ProfileDAL.ViewModels;
using System.Linq.Dynamic.Core;
using System;
using Azure;
using Microsoft.AspNetCore.SignalR;
using API.Socket;

namespace API.Controllers.HuEmployeeCvEdit
{
    public class ApproveHuEmployeeCvEditRepository : IApproveHuEmployeeCvEditRepository
    {
        private readonly GenericUnitOfWork _uow;
        private readonly FullDbContext _dbContext;
        private IGenericRepository<HU_EMPLOYEE_CV_EDIT, HuEmployeeCvEditDTO> _genericRepository;
        private IGenericRepository<HU_EMPLOYEE_CV, HuEmployeeCvDTO> _genericParentRepository;
        private readonly GenericReducer<HU_EMPLOYEE_CV_EDIT, HuEmployeeCvEditDTO> _genericReducer;
        IHubContext<SignalHub> _hubContext;

        public ApproveHuEmployeeCvEditRepository(FullDbContext context, GenericUnitOfWork uow, IHubContext<SignalHub> hubContext)
        {
            _dbContext = context;
            _uow = uow;
            _genericRepository = _uow.GenericRepository<HU_EMPLOYEE_CV_EDIT, HuEmployeeCvEditDTO>();
            _genericParentRepository = _uow.GenericRepository<HU_EMPLOYEE_CV, HuEmployeeCvDTO>();
            _genericReducer = new();
            _hubContext = hubContext;
        }

        public async Task<GenericPhaseTwoListResponse<HuEmployeeCvEditDTO>> SinglePhaseQueryList(GenericQueryListDTO<HuEmployeeCvEditDTO> request)
        {
            var joined = from p in _dbContext.HuEmployeeCvEdits.AsNoTracking()
                             // JOIN OTHER ENTITIES BASED ON THE BUSINESS
                         select new HuEmployeeCvEditDTO
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
            var response = await _genericRepository.ReadAllByKey(key, value);
            return response;
        }

        public async Task<FormatedResponse> ReadAllByKey(string key, string value)
        {
            var response = await _genericRepository.ReadAllByKey(key, value);
            return response;
        }

        public async Task<FormatedResponse> GetById(long id)
        {
            var res = await _genericRepository.GetById(id);
            if (res.InnerBody != null)
            {
                var response = res.InnerBody;
                var list = new List<HU_EMPLOYEE_CV_EDIT>
                    {
                        (HU_EMPLOYEE_CV_EDIT)response
                    };
                var joined = (from l in list
                                  // JOIN OTHER ENTITIES BASED ON THE BUSINESS
                              select new HuEmployeeCvEditDTO
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

        public async Task<FormatedResponse> Create(GenericUnitOfWork _uow, HuEmployeeCvEditDTO dto, string sid)
        {
            var response = await _genericRepository.Create(_uow, dto, sid);
            return response;
        }

        public async Task<FormatedResponse> CreateRange(GenericUnitOfWork _uow, List<HuEmployeeCvEditDTO> dtos, string sid)
        {
            var add = new List<HuEmployeeCvEditDTO>();
            add.AddRange(dtos);
            var response = await _genericRepository.CreateRange(_uow, add, sid);
            return response;
        }

        public async Task<FormatedResponse> Update(GenericUnitOfWork _uow, HuEmployeeCvEditDTO dto, string sid, bool patchMode = true)
        {
            var response = await _genericRepository.Update(_uow, dto, sid, patchMode);
            return response;
        }

        public async Task<FormatedResponse> UpdateRange(GenericUnitOfWork _uow, List<HuEmployeeCvEditDTO> dtos, string sid, bool patchMode = true)
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
            var response = await _genericRepository.ToggleActiveIds(_uow, ids, valueToBind, sid);
            return response;
        }

        public async Task<GenericPhaseTwoListResponse<HuEmployeeCvEditDTO>> QueryListCvEdit(GenericQueryListDTO<HuEmployeeCvEditDTO> request)
        {
            string[] arrEmpty = Array.Empty<string>();
            var employee = _uow.Context.Set<HU_EMPLOYEE>().AsNoTracking().AsQueryable();
            var employeeEdit = _uow.Context.Set<HU_EMPLOYEE_CV_EDIT>().AsNoTracking().AsQueryable();
            var employeeCv = _uow.Context.Set<HU_EMPLOYEE_CV>().AsNoTracking().AsQueryable();
            var position = _uow.Context.Set<HU_POSITION>().AsNoTracking().AsQueryable();
            var org = _uow.Context.Set<HU_ORGANIZATION>().AsNoTracking().AsQueryable();
            var otherList = _uow.Context.Set<SYS_OTHER_LIST>().AsNoTracking().AsQueryable();
            var province = _uow.Context.Set<HU_PROVINCE>().AsNoTracking().AsQueryable();
            var job = _uow.Context.Set<HU_JOB>().AsNoTracking().AsQueryable();
            var response = from ee in employeeEdit
                           from e in employee.Where(x => x.ID == ee.EMPLOYEE_ID).DefaultIfEmpty()
                           from cv in employeeCv.Where(x => x.ID == e.PROFILE_ID).DefaultIfEmpty()
                           from p in position.Where(x => x.ID == e.POSITION_ID).DefaultIfEmpty()
                           from j in job.Where(x => x.ID == p.JOB_ID).DefaultIfEmpty()
                           from o in org.Where(x => x.ID == p.ORG_ID).DefaultIfEmpty()
                           from r in otherList.Where(x => x.ID == ee.RELIGION_ID).DefaultIfEmpty()
                           from n in otherList.Where(x => x.ID == ee.NATIVE_ID).DefaultIfEmpty()
                           from rs in otherList.Where(x => x.ID == ee.MARITAL_STATUS_ID).DefaultIfEmpty()
                           from s in otherList.Where(x => x.ID == ee.STATUS_APPROVED_CV_ID).DefaultIfEmpty()
                           from pr in province.Where(x => x.ID == ee.IDENTITY_ADDRESS).DefaultIfEmpty()
                           where ee.IS_SEND_PORTAL_CV == true && ee.IS_APPROVED_CV == false && ee.IS_SAVE_CV == false
                           select new HuEmployeeCvEditDTO()
                           {
                               Id = ee.ID,
                               Code = e.CODE,
                               JobOrderNum = (int)(j.ORDERNUM ?? 999),
                               Fullname = cv.FULL_NAME,
                               OrgName = o.NAME,
                               PositionName = p.NAME,
                               ReligionName = r.NAME,
                               NativeName = n.NAME,
                               MaritalStatusName = rs.NAME,
                               IdNo = ee.ID_NO,
                               IdDate = ee.ID_DATE,
                               IdPlace = ee.ID_PLACE,
                               StatusName = s.NAME,
                               IdentityAddressName = pr.NAME,
                               OrgId = e.ORG_ID,
                               IdDateStr = (ee.ID_DATE != null) ? ee.ID_DATE.Value.ToString("dd/MM/yyyy") : "",
                               ModelChanges = !string.IsNullOrEmpty(ee.MODEL_CHANGE) ? ee.MODEL_CHANGE!.Split(";", StringSplitOptions.None) : arrEmpty
                           };
            var singlePhaseResult = await _genericReducer.SinglePhaseReduce(response, request);
            return singlePhaseResult;
        }
        public async Task<GenericPhaseTwoListResponse<HuEmployeeCvEditDTO>> QueryListContactEdit(GenericQueryListDTO<HuEmployeeCvEditDTO> request)
        {
            string[] arrEmpty = Array.Empty<string>();
            var employeeCvEdit = _uow.Context.Set<HU_EMPLOYEE_CV_EDIT>().AsNoTracking().AsQueryable();
            var employeeCv = _uow.Context.Set<HU_EMPLOYEE_CV>().AsNoTracking().AsQueryable();
            var employee = _uow.Context.Set<HU_EMPLOYEE>().AsNoTracking().AsNoTracking();
            var district = _uow.Context.Set<HU_DISTRICT>().AsNoTracking().AsQueryable();
            var province = _uow.Context.Set<HU_PROVINCE>().AsNoTracking().AsQueryable();
            var ward = _uow.Context.Set<HU_WARD>().AsNoTracking().AsQueryable();
            var position = _uow.Context.Set<HU_POSITION>().AsNoTracking().AsNoTracking();
            var org = _uow.Context.Set<HU_ORGANIZATION>().AsNoTracking().AsNoTracking();
            var otherList = _uow.Context.Set<SYS_OTHER_LIST>().AsNoTracking().AsNoTracking();
            var job = _uow.Context.Set<HU_JOB>().AsNoTracking().AsQueryable();
            var response = from ee in employeeCvEdit
                           from e in employee.Where(x => x.ID == ee.EMPLOYEE_ID).DefaultIfEmpty()
                           from cv in employeeCv.Where(x => x.ID == e.PROFILE_ID).DefaultIfEmpty()
                           from po in position.Where(x => x.ID == e.POSITION_ID).DefaultIfEmpty()
                           from j in job.Where(x => x.ID == po.JOB_ID).DefaultIfEmpty()
                           from o in org.Where(x => x.ID == e.ORG_ID).DefaultIfEmpty()
                           from d in district.Where(x => x.ID == ee.DISTRICT_ID).DefaultIfEmpty()
                           from p in province.Where(x => x.ID == ee.PROVINCE_ID).DefaultIfEmpty()
                           from w in ward.Where(x => x.ID == ee.WARD_ID).DefaultIfEmpty()
                           from cp in province.Where(x => x.ID == ee.CUR_PROVINCE_ID).DefaultIfEmpty()
                           from cd in district.Where(x => x.ID == ee.CUR_DISTRICT_ID).DefaultIfEmpty()
                           from cw in ward.Where(x => x.ID == ee.WARD_ID).DefaultIfEmpty()
                           from sys in otherList.Where(x => x.ID == ee.STATUS_APPROVED_CONTACT_ID).DefaultIfEmpty()
                           where ee.IS_SAVE_CONTACT == false && ee.IS_SEND_PORTAL_CONTACT == true && ee.IS_APPROVED_CONTACT == false
                           select new HuEmployeeCvEditDTO()
                           {
                               Id = ee.ID,
                               Code = e.CODE,
                               Fullname = cv.FULL_NAME,
                               JobOrderNum = (int)(j.ORDERNUM ?? 999),
                               PositionName = po.NAME,
                               OrgName = o.NAME,
                               Address = ee.ADDRESS,
                               ProvinceName = p.NAME,
                               DistrictName = d.NAME,
                               WardName = w.NAME,
                               CurAddress = ee.CUR_ADDRESS,
                               CurProvinceName = cp.NAME,
                               CurDistrictName = cd.NAME,
                               CurWardName = cw.NAME,
                               MobilePhone = ee.MOBILE_PHONE,
                               MobilePhoneLand = ee.MOBILE_PHONE_LAND,
                               Email = ee.EMAIL,
                               StatusName = sys.NAME,
                               OrgId = e.ORG_ID,
                               ModelChanges = !string.IsNullOrEmpty(ee.MODEL_CHANGE) ? ee.MODEL_CHANGE!.Split(";",StringSplitOptions.None) : arrEmpty
                           };
            var singlePhaseResult = await _genericReducer.SinglePhaseReduce(response, request);
            return singlePhaseResult;

        }
        public async Task<GenericPhaseTwoListResponse<HuEmployeeCvEditDTO>> QueryListAdditionalInfoEdit(GenericQueryListDTO<HuEmployeeCvEditDTO> request)
        {
            string[] arrEmpty = Array.Empty<string>();
            var employeeCvEdit = _uow.Context.Set<HU_EMPLOYEE_CV_EDIT>().AsNoTracking().AsQueryable();
            var employee = _uow.Context.Set<HU_EMPLOYEE>().AsNoTracking().AsQueryable();
            var positon = _uow.Context.Set<HU_POSITION>().AsNoTracking().AsQueryable();
            var org = _uow.Context.Set<HU_ORGANIZATION>().AsNoTracking().AsQueryable();
            var otherList = _uow.Context.Set<SYS_OTHER_LIST>().AsNoTracking().AsQueryable();
            var job = _uow.Context.Set<HU_JOB>().AsNoTracking().AsQueryable();
            var response = from ee in employeeCvEdit
                           from e in employee.Where(x => x.ID == ee.EMPLOYEE_ID).DefaultIfEmpty()
                           from po in positon.Where(x => x.ID == e.POSITION_ID).DefaultIfEmpty()
                           from j in job.Where(x => x.ID == po.JOB_ID).DefaultIfEmpty()
                           from o in org.Where(x => x.ID == e.ORG_ID).DefaultIfEmpty()
                           from sys in otherList.Where(x => x.ID == ee.STATUS_ADDINATIONAL_INFO_ID).DefaultIfEmpty()
                           where ee.IS_APPROVED_ADDITIONAL_INFO == false && ee.IS_SEND_PORTAL_ADDITIONAL_INFO == true
                           select new HuEmployeeCvEditDTO()
                           {
                               Id = ee.ID,
                               Code = e.CODE,
                               Fullname = e.Profile.FULL_NAME,
                               JobOrderNum = (int)(j.ORDERNUM ?? 999),
                               OrgName = o.NAME,
                               PositionName = po.NAME,
                               PassNo = ee.PASS_NO,
                               PassDate = ee.PASS_DATE,
                               PassExpire = ee.PASS_EXPIRE,
                               PassPlace = ee.PASS_PLACE,
                               VisaNo = ee.VISA_NO,
                               VisaDate = ee.VISA_DATE,
                               VisaPlace = ee.VISA_PLACE,
                               VisaExpire = ee.VISA_EXPIRE,
                               WorkNo = ee.WORK_NO,
                               WorkPermitDate = ee.WORK_PERMIT_DATE,
                               WorkPermitExpire = ee.WORK_PERMIT_EXPIRE,
                               WorkPermitPlace = ee.WORK_PERMIT_PLACE,
                               StatusName = sys.NAME,
                               OrgId = e.ORG_ID,
                               ModelChanges = !string.IsNullOrEmpty(ee.MODEL_CHANGE) ? ee.MODEL_CHANGE!.Split(";",StringSplitOptions.None) : arrEmpty

                           };
            var singlePhaseResult = await _genericReducer.SinglePhaseReduce(response, request);
            return singlePhaseResult;
        }
        public async Task<GenericPhaseTwoListResponse<HuEmployeeCvEditDTO>> QueryListBankInfoEdit(GenericQueryListDTO<HuEmployeeCvEditDTO> request)
        {
            string[] arrEmpty = Array.Empty<string>();
            var employeeCvEdit = _uow.Context.Set<HU_EMPLOYEE_CV_EDIT>().AsNoTracking().AsQueryable();
            var employee = _uow.Context.Set<HU_EMPLOYEE>().AsNoTracking().AsQueryable();
            var bank = _uow.Context.Set<HU_BANK>().AsNoTracking().AsQueryable();
            var bankBrach = _uow.Context.Set<HU_BANK_BRANCH>().AsNoTracking().AsQueryable();
            var position = _uow.Context.Set<HU_POSITION>().AsNoTracking().AsNoTracking();
            var org = _uow.Context.Set<HU_ORGANIZATION>().AsNoTracking().AsNoTracking();
            var otherList = _uow.Context.Set<SYS_OTHER_LIST>().AsNoTracking().AsNoTracking();
            var job = _uow.Context.Set<HU_JOB>().AsNoTracking().AsQueryable();
            var response = from ee in employeeCvEdit
                           from e in employee.Where(x => x.ID == ee.EMPLOYEE_ID).DefaultIfEmpty()
                           from b1 in bank.Where(x => x.ID == ee.BANK_ID).DefaultIfEmpty()
                           from b2 in bank.Where(x => x.ID == ee.BANK_ID_2).DefaultIfEmpty()
                           from br1 in bankBrach.Where(x => x.ID == ee.BANK_BRANCH_ID).DefaultIfEmpty()
                           from br2 in bankBrach.Where(x => x.ID == ee.BANK_BRANCH_ID_2).DefaultIfEmpty()
                           from po in position.Where(x => x.ID == e.POSITION_ID).DefaultIfEmpty()
                           from j in job.Where(x => x.ID == po.JOB_ID).DefaultIfEmpty()
                           from o in org.Where(x => x.ID == e.ORG_ID).DefaultIfEmpty()
                           from sys in otherList.Where(x => x.ID == ee.STATUS_APPROVED_BANK_ID).DefaultIfEmpty()
                           where ee.IS_APPROVED_BANK_INFO == false && ee.IS_SEND_PORTAL_BANK_INFO == true
                           select new HuEmployeeCvEditDTO()
                           {
                               Id = ee.ID,
                               Code = e.CODE,
                               Fullname = e.Profile.FULL_NAME,
                               JobOrderNum = (int)(j.ORDERNUM ?? 999),
                               PositionName = po.NAME,
                               OrgName = o.NAME,
                               BankNo = ee.BANK_NO,
                               BankBranchName = br1.NAME,
                               BankName = b1.NAME,
                               BankNo2 = ee.BANK_NO_2,
                               BankName2 = b2.NAME,
                               BankBranchName2 = br2.NAME,
                               StatusName = sys.NAME,
                               OrgId = e.ORG_ID,
                               ModelChanges = !string.IsNullOrEmpty(ee.MODEL_CHANGE) ? ee.MODEL_CHANGE!.Split(";", StringSplitOptions.None) : arrEmpty
                           };
            var singlePhaseResult = await _genericReducer.SinglePhaseReduce(response, request);
            return singlePhaseResult;
        }

        public async Task<GenericPhaseTwoListResponse<HuEmployeeCvEditDTO>> QueryListEducationEdit(GenericQueryListDTO<HuEmployeeCvEditDTO> request)
        {
            string[] arrEmpty = Array.Empty<string>();
            var employeeCvEdit = _uow.Context.Set<HU_EMPLOYEE_CV_EDIT>().AsNoTracking().AsQueryable();
            var employee = _uow.Context.Set<HU_EMPLOYEE>().AsNoTracking().AsQueryable();
            var position = _uow.Context.Set<HU_POSITION>().AsNoTracking().AsNoTracking();
            var org = _uow.Context.Set<HU_ORGANIZATION>().AsNoTracking().AsNoTracking();
            var otherList = _uow.Context.Set<SYS_OTHER_LIST>().AsNoTracking().AsQueryable();
            var job = _uow.Context.Set<HU_JOB>().AsNoTracking().AsQueryable();
            var response = from ee in employeeCvEdit
                           from e in employee.Where(x => x.ID == ee.EMPLOYEE_ID).DefaultIfEmpty()
                           from po in position.Where(x => x.ID == e.POSITION_ID).DefaultIfEmpty()
                           from j in job.Where(x => x.ID == po.JOB_ID).DefaultIfEmpty()
                           from o in org.Where(x => x.ID == e.ORG_ID).DefaultIfEmpty()
                           from ol in otherList.Where(x => x.ID == ee.SCHOOL_ID).DefaultIfEmpty()
                           from el in otherList.Where(x => x.ID == ee.EDUCATION_LEVEL_ID).DefaultIfEmpty()
                           from ql in otherList.Where(x => x.ID == ee.QUALIFICATIONID).DefaultIfEmpty()
                           from ql2 in otherList.Where(x => x.ID == ee.QUALIFICATIONID_2).DefaultIfEmpty()
                           from ql3 in otherList.Where(x => x.ID == ee.QUALIFICATIONID_3).DefaultIfEmpty()
                           from ll in otherList.Where(x => x.ID == ee.LEARNING_LEVEL_ID).DefaultIfEmpty()
                           from tf in otherList.Where(x => x.ID == ee.TRAINING_FORM_ID).DefaultIfEmpty()
                           from tf2 in otherList.Where(x => x.ID == ee.TRAINING_FORM_ID_2).DefaultIfEmpty()
                           from tf3 in otherList.Where(x => x.ID == ee.TRAINING_FORM_ID_3).DefaultIfEmpty()
                           from s in otherList.Where(x => x.ID == ee.STATUS_APPROVED_EDUCATION_ID).DefaultIfEmpty()
                           from reference_1 in _dbContext.SysOtherLists.Where(x => x.ID == ee.COMPUTER_SKILL_ID).DefaultIfEmpty()
                           from reference_2 in _dbContext.SysOtherLists.Where(x => x.ID == ee.LICENSE_ID).DefaultIfEmpty()

                           where ee.IS_SEND_PORTAL == true && ee.IS_APPROVED_EDUCATION == false && ee.IS_SEND_PORTAL_EDUCATION == true

                           select new HuEmployeeCvEditDTO()
                           {
                               Id = ee.ID,
                               Code = e.CODE,
                               Fullname = e.Profile.FULL_NAME,
                               JobOrderNum = (int)(j.ORDERNUM ?? 999),
                               PositionName = po.NAME,
                               OrgName = o.NAME,
                               EducationLevel = el.NAME,
                               SchoolName = ol.NAME,
                               LearningLevel = ll.NAME,
                               StatusName = s.NAME,
                               QualificationName = ql.NAME,
                               QualificationName2 = ql2.NAME,
                               QualificationName3 = ql3.NAME,
                               TraningFormName = tf.NAME,
                               TrainingFormName2 = tf2.NAME,
                               TrainingFormName3 = tf.NAME,
                               OrgId = e.ORG_ID,
                               ComputerSkill = reference_1.NAME,
                               License = reference_2.NAME,
                               ModelChanges = !string.IsNullOrEmpty(ee.MODEL_CHANGE) ? ee.MODEL_CHANGE!.Split(";",StringSplitOptions.None) : arrEmpty
                           };
            var singlePhaseResult = await _genericReducer.SinglePhaseReduce(response, request);
            return singlePhaseResult;
        }

        public async Task<FormatedResponse> ApproveCvEdit(GenericUnapprovePortalDTO model)
        {
            try
            {
                string sid = "";
                bool pathMode = true;
                HuEmployeeCvDTO dto = new();
                List<HuEmployeeCvEditDTO> list = new();
                List<HuEmployeeCvDTO> lstUpdate = new();
                var getOtherList = (from t in _uow.Context.Set<SYS_OTHER_LIST_TYPE>().Where(x => x.CODE == "STATUS")
                                    from o in _uow.Context.Set<SYS_OTHER_LIST>().Where(x => x.TYPE_ID == t.ID)
                                    where o.CODE == "DD"
                                    select new { Id = o.ID }).FirstOrDefault();
                var getOtherList2 = (from t in _uow.Context.Set<SYS_OTHER_LIST_TYPE>().Where(x => x.CODE == "STATUS")
                                     from o in _uow.Context.Set<SYS_OTHER_LIST>().DefaultIfEmpty()
                                     where o.CODE == "TCPD"
                                     select new { Id = o.ID }).FirstOrDefault();
                if (model.ValueToBind == true)
                {
                    model.Ids.ForEach(item =>
                    {
                        list.Add(new()
                        {
                            Id = item,
                            IsApprovedCv = true,
                            IsSendPortalCv = false,
                            StatusApprovedCvId = getOtherList?.Id


                        });

                    });
                    var approvedResponse = await _genericRepository.UpdateRange(_uow, list, sid, pathMode);
                    if (approvedResponse != null)
                    {
                        model.Ids.ForEach(item =>
                        {
                            var getData = _uow.Context.Set<HU_EMPLOYEE_CV_EDIT>().Where(x => x.ID == item).FirstOrDefault();
                            lstUpdate.Add(new()
                            {
                                Id = getData.HU_EMPLOYEE_CV_ID,
                                ReligionId = getData.RELIGION_ID,
                                NativeId = getData.NATIVE_ID,
                                MaritalStatusId = getData.MARITAL_STATUS_ID,
                                IdNo = getData.ID_NO,
                                IdDate = getData.ID_DATE,
                                IdPlace = getData.IDENTITY_ADDRESS,
                                IdExpireDate = getData.ID_EXPIRE_DATE

                            });
                            AT_NOTIFICATION noti = new AT_NOTIFICATION()
                            {
                                CREATED_BY = sid,
                                TYPE = 4,
                                ACTION = 2,
                                TITLE = model.Reason,
                                STATUS_NOTIFY = 1,
                                EMP_NOTIFY_ID = getData.EMPLOYEE_ID.ToString(),
                                REF_ID = item,
                                MODEL_CHANGE = getData.MODEL_CHANGE,
                                CREATED_DATE = DateTime.Now
                            };
                            _dbContext.AtNotifications.AddRange(noti);
                            _dbContext.SaveChanges();
                            var employeeId = getData.EMPLOYEE_ID;
                            var users = _dbContext.SysUsers.Where(x => x.EMPLOYEE_ID == employeeId).ToList();
                            for (var i = 0; i < users.Count; i++)
                            {
                                var username = users[i].USERNAME;
                                if (!string.IsNullOrEmpty(username))
                                {
                                    _hubContext.Clients.User(username).SendAsync("ReceiveMessage", new
                                    {
                                        SignalType = "APPROVE_NOTIFICATION",
                                        Message = "Bạn có thông báo mới trên Portal"/*message*/,
                                        Data = new
                                        {

                                        }
                                    });
                                }

                            }
                        });
                        var updateReponse = await _genericParentRepository.UpdateRange(_uow, lstUpdate, sid, pathMode);
                        if (updateReponse != null)
                        {
                            return new FormatedResponse() { StatusCode = EnumStatusCode.StatusCode200, MessageCode = CommonMessageCode.APPROVED_SUCCESS };
                        }
                        else
                        {
                            return new FormatedResponse() { StatusCode = EnumStatusCode.StatusCode400, ErrorType = EnumErrorType.CATCHABLE, MessageCode = CommonMessageCode.APPROVE_FAIL };
                        }
                    }
                    else
                    {
                        return new FormatedResponse() { StatusCode = EnumStatusCode.StatusCode400, ErrorType = EnumErrorType.CATCHABLE, MessageCode = CommonMessageCode.APPROVE_FAIL };
                    }
                }
                else
                {


                    model.Ids.ForEach(item =>
                    {
                        var getData = _uow.Context.Set<HU_EMPLOYEE_CV_EDIT>().Where(x => x.ID == item).FirstOrDefault();
                        list.Add(new()
                        {
                            Id = item,
                            IsApprovedCv = false,
                            IsSendPortalCv = false,
                            StatusApprovedCvId = getOtherList2.Id,
                            Reason = model.Reason
                        });
                        AT_NOTIFICATION noti = new AT_NOTIFICATION()
                        {
                            CREATED_BY = sid,
                            TYPE = 4,
                            ACTION = 2,
                            TITLE = model.Reason,
                            STATUS_NOTIFY = 2,
                            EMP_NOTIFY_ID = getData.EMPLOYEE_ID.ToString(),
                            REF_ID = item,
                            MODEL_CHANGE = getData.MODEL_CHANGE,
                            CREATED_DATE = DateTime.Now
                        };
                        _dbContext.AtNotifications.AddRange(noti);
                        _dbContext.SaveChanges();
                        var employeeId = getData.EMPLOYEE_ID;
                        var users = _dbContext.SysUsers.Where(x => x.EMPLOYEE_ID == employeeId).ToList();
                        for (var i = 0; i < users.Count; i++)
                        {
                            var username = users[i].USERNAME;
                            if (!string.IsNullOrEmpty(username))
                            {
                                _hubContext.Clients.User(username).SendAsync("ReceiveMessage", new
                                {
                                    SignalType = "APPROVE_NOTIFICATION",
                                    Message = "Bạn có thông báo mới trên Portal"/*message*/,
                                    Data = new
                                    {

                                    }
                                });
                            }

                        }
                    });
                    var unApproved = await _genericRepository.UpdateRange(_uow, list, sid, pathMode);
                    if (unApproved != null)
                    {
                        return new FormatedResponse() { StatusCode = EnumStatusCode.StatusCode200, MessageCode = CommonMessageCode.UNAPPROVED_COMPLETE };
                    }
                    else
                    {
                        return new FormatedResponse() { StatusCode = EnumStatusCode.StatusCode400, ErrorType = EnumErrorType.CATCHABLE, MessageCode = CommonMessageCode.UNAPPROVED_FAILD };
                    }
                }

            }
            catch (Exception ex)
            {

                return new FormatedResponse() { StatusCode = EnumStatusCode.StatusCode500, ErrorType = EnumErrorType.UNCATCHABLE, MessageCode = ex.Message };
            }
        }

        public async Task<FormatedResponse> ApproveContactEdit(GenericUnapprovePortalDTO model)
        {
            try
            {
                string sid = "";
                bool pathMode = true;
                HuEmployeeCvDTO dto = new();
                List<HuEmployeeCvEditDTO> list = new();
                List<HuEmployeeCvDTO> lstUpdate = new();
                var getOtherList = (from t in _uow.Context.Set<SYS_OTHER_LIST_TYPE>().Where(x => x.CODE == "STATUS")
                                    from o in _uow.Context.Set<SYS_OTHER_LIST>().Where(x => x.TYPE_ID == t.ID)
                                    where o.CODE == "DD"
                                    select new { Id = o.ID }).FirstOrDefault();
                var getOtherList2 = (from t in _uow.Context.Set<SYS_OTHER_LIST_TYPE>().Where(x => x.CODE == "STATUS")
                                     from o in _uow.Context.Set<SYS_OTHER_LIST>().DefaultIfEmpty()
                                     where o.CODE == "TCPD"
                                     select new { Id = o.ID }).FirstOrDefault();
                if (model.ValueToBind == true)
                {
                    model.Ids.ForEach(item =>
                    {
                        list.Add(new()
                        {
                            Id = item,
                            IsApprovedContact = true,
                            StatusApprovedContactId = getOtherList?.Id
                        });

                    });
                    var approvedResponse = await _genericRepository.UpdateRange(_uow, list, sid, pathMode);
                    if (approvedResponse != null)
                    {
                        model.Ids.ForEach(item =>
                        {
                            var getData = _uow.Context.Set<HU_EMPLOYEE_CV_EDIT>().Where(x => x.ID == item).FirstOrDefault();
                            lstUpdate.Add(new()
                            {
                                Id = getData.HU_EMPLOYEE_CV_ID,
                                Address = getData.ADDRESS,
                                ProvinceId = getData.PROVINCE_ID,
                                DistrictId = getData.DISTRICT_ID,
                                WardId = getData.WARD_ID,
                                CurAddress = getData.CUR_ADDRESS,
                                CurProvinceId = getData.CUR_PROVINCE_ID,
                                CurDistrictId = getData.CUR_DISTRICT_ID,
                                CurWardId = getData.CUR_WARD_ID,
                                MobilePhone = getData.MOBILE_PHONE,
                                MobilePhoneLand = getData.MOBILE_PHONE_LAND,
                                Email = getData.EMAIL
                            });
                            AT_NOTIFICATION noti = new AT_NOTIFICATION()
                            {
                                CREATED_BY = sid,
                                TYPE = 9,
                                ACTION = 2,
                                TITLE = model.Reason,
                                STATUS_NOTIFY = 1,
                                EMP_NOTIFY_ID = getData.EMPLOYEE_ID.ToString(),
                                REF_ID = item,
                                MODEL_CHANGE = getData.MODEL_CHANGE,
                                CREATED_DATE = DateTime.Now
                            };
                            _dbContext.AtNotifications.AddRange(noti);
                            _dbContext.SaveChanges();
                            var employeeId = getData.EMPLOYEE_ID;
                            var users = _dbContext.SysUsers.Where(x => x.EMPLOYEE_ID == employeeId).ToList();
                            for (var i = 0; i < users.Count; i++)
                            {
                                var username = users[i].USERNAME;
                                if (!string.IsNullOrEmpty(username))
                                {
                                    _hubContext.Clients.User(username).SendAsync("ReceiveMessage", new
                                    {
                                        SignalType = "APPROVE_NOTIFICATION",
                                        Message = "Bạn có thông báo mới trên Portal"/*message*/,
                                        Data = new
                                        {

                                        }
                                    });
                                }

                            }

                        });
                        var updateResponse = await _genericParentRepository.UpdateRange(_uow, lstUpdate, sid, pathMode);
                        if (updateResponse != null)
                        {
                            return new FormatedResponse() { StatusCode = EnumStatusCode.StatusCode200, MessageCode = CommonMessageCode.APPROVED_SUCCESS };
                        }
                        else
                        {
                            return new FormatedResponse() { StatusCode = EnumStatusCode.StatusCode400, ErrorType = EnumErrorType.CATCHABLE, MessageCode = CommonMessageCode.UNAPPROVED_FAILD };
                        }
                    }
                    else
                    {
                        return new FormatedResponse() { StatusCode = EnumStatusCode.StatusCode400, ErrorType = EnumErrorType.CATCHABLE, MessageCode = CommonMessageCode.APPROVE_FAIL };
                    }
                }
                else
                {
                    model.Ids.ForEach(item =>
                    {
                        var getData = _uow.Context.Set<HU_EMPLOYEE_CV_EDIT>().Where(x => x.ID == item).FirstOrDefault();

                        list.Add(new()
                        {
                            Id = item,
                            IsApprovedContact = true,
                            IsSendPortalContact = false,
                            Reason = model.Reason,
                            StatusApprovedContactId = getOtherList2.Id
                        });
                        AT_NOTIFICATION noti = new AT_NOTIFICATION()
                        {
                            CREATED_BY = sid,
                            TYPE = 9,
                            ACTION = 2,
                            TITLE = model.Reason,
                            STATUS_NOTIFY = 2,
                            EMP_NOTIFY_ID = getData.EMPLOYEE_ID.ToString(),
                            REF_ID = item,
                            MODEL_CHANGE = getData.MODEL_CHANGE,
                            CREATED_DATE = DateTime.Now
                        };
                        _dbContext.AtNotifications.AddRange(noti);
                        _dbContext.SaveChanges();
                        var employeeId = getData.EMPLOYEE_ID;
                        var users = _dbContext.SysUsers.Where(x => x.EMPLOYEE_ID == employeeId).ToList();
                        for (var i = 0; i < users.Count; i++)
                        {
                            var username = users[i].USERNAME;
                            if (!string.IsNullOrEmpty(username))
                            {
                                _hubContext.Clients.User(username).SendAsync("ReceiveMessage", new
                                {
                                    SignalType = "APPROVE_NOTIFICATION",
                                    Message = "Bạn có thông báo mới trên Portal"/*message*/,
                                    Data = new
                                    {

                                    }
                                });
                            }

                        }
                    });
                    var unApproved = await _genericRepository.UpdateRange(_uow, list, sid, pathMode);
                    if (unApproved != null)
                    {
                        return new FormatedResponse() { StatusCode = EnumStatusCode.StatusCode200, MessageCode = CommonMessageCode.UNAPPROVED_COMPLETE };
                    }
                    else
                    {
                        return new FormatedResponse() { StatusCode = EnumStatusCode.StatusCode400, ErrorType = EnumErrorType.CATCHABLE, MessageCode = CommonMessageCode.UNAPPROVED_FAILD };
                    }
                }
            }
            catch (Exception ex)
            {

                return new FormatedResponse() { StatusCode = EnumStatusCode.StatusCode500, ErrorType = EnumErrorType.UNCATCHABLE, MessageCode = ex.Message };
            }
        }

        public async Task<FormatedResponse> ApproveAdditionalEdit(GenericUnapprovePortalDTO model)
        {
            try
            {
                string sid = "";
                bool pathMode = true;
                HuEmployeeCvDTO dto = new();
                List<HuEmployeeCvEditDTO> list = new();
                List<HuEmployeeCvDTO> lstUpdate = new();
                var getOtherList = (from t in _uow.Context.Set<SYS_OTHER_LIST_TYPE>().Where(x => x.CODE == "STATUS")
                                    from o in _uow.Context.Set<SYS_OTHER_LIST>().DefaultIfEmpty()
                                    where o.CODE == "DD"
                                    select new { Id = o.ID }).FirstOrDefault();
                var getOtherList2 = (from t in _uow.Context.Set<SYS_OTHER_LIST_TYPE>().Where(x => x.CODE == "STATUS")
                                     from o in _uow.Context.Set<SYS_OTHER_LIST>().DefaultIfEmpty()
                                     where o.CODE == "TCPD"
                                     select new { Id = o.ID }).FirstOrDefault();

                if (model.ValueToBind == true)
                {
                    model.Ids.ForEach(item =>
                    {
                        var getData = _uow.Context.Set<HU_EMPLOYEE_CV_EDIT>().Where(x => x.ID == item).FirstOrDefault();
                        list.Add(new()
                        {
                            Id = item,
                            IsApprovedAdditionalInfo = true,
                            StatusAddinationalInfoId = getOtherList.Id

                        });
                        AT_NOTIFICATION noti = new AT_NOTIFICATION()
                        {
                            CREATED_BY = sid,
                            TYPE = 10,
                            ACTION = 2,
                            TITLE = model.Reason,
                            STATUS_NOTIFY = 1,
                            EMP_NOTIFY_ID = getData.EMPLOYEE_ID.ToString(),
                            REF_ID = item,
                            MODEL_CHANGE = getData.MODEL_CHANGE,
                            CREATED_DATE = DateTime.Now
                        };
                        _dbContext.AtNotifications.AddRange(noti);
                        _dbContext.SaveChanges();
                        var employeeId = getData.EMPLOYEE_ID;
                        var users = _dbContext.SysUsers.Where(x => x.EMPLOYEE_ID == employeeId).ToList();
                        for (var i = 0; i < users.Count; i++)
                        {
                            var username = users[i].USERNAME;
                            if (!string.IsNullOrEmpty(username))
                            {
                                _hubContext.Clients.User(username).SendAsync("ReceiveMessage", new
                                {
                                    SignalType = "APPROVE_NOTIFICATION",
                                    Message = "Bạn có thông báo mới trên Portal"/*message*/,
                                    Data = new { }
                                });
                            }

                        }

                    });
                    var approvedResponse = await _genericRepository.UpdateRange(_uow, list, sid, pathMode);
                    if (approvedResponse != null)
                    {
                        model.Ids.ForEach(item =>
                        {
                            var getData = _uow.Context.Set<HU_EMPLOYEE_CV_EDIT>().Where(x => x.ID == item).FirstOrDefault();

                            lstUpdate.Add(new()
                            {
                                Id = getData.HU_EMPLOYEE_CV_ID,
                                PassNo = getData.PASS_NO,
                                PassDate = getData.PASS_DATE,
                                PassExpire = getData.PASS_EXPIRE,
                                PassPlace = getData.PASS_PLACE,
                                VisaNo = getData.VISA_NO,
                                VisaDate = getData.VISA_DATE,
                                VisaExpire = getData.VISA_EXPIRE,
                                VisaPlace = getData.VISA_PLACE,
                                WorkNo = getData.WORK_NO,
                                WorkPermitDate = getData.WORK_PERMIT_DATE,
                                WorkPermitExpire = getData.WORK_PERMIT_EXPIRE,
                                WorkPermitPlace = getData.WORK_PERMIT_PLACE
                            });
                        });
                        var updateResponse = await _genericParentRepository.UpdateRange(_uow, lstUpdate, sid, pathMode);
                        if (updateResponse != null)
                        {
                            return new FormatedResponse() { StatusCode = EnumStatusCode.StatusCode200, MessageCode = CommonMessageCode.APPROVED_SUCCESS };
                        }
                        else
                        {
                            return new FormatedResponse() { StatusCode = EnumStatusCode.StatusCode400, MessageCode = CommonMessageCode.APPROVE_FAIL };
                        }
                    }
                    else
                    {
                        return new FormatedResponse() { StatusCode = EnumStatusCode.StatusCode400, ErrorType = EnumErrorType.CATCHABLE, MessageCode = CommonMessageCode.APPROVE_FAIL };
                    }
                }
                else
                {
                    model.Ids.ForEach(item =>
                    {
                        var getData = _uow.Context.Set<HU_EMPLOYEE_CV_EDIT>().Where(x => x.ID == item).FirstOrDefault();
                        list.Add(new()
                        {
                            Id = item,
                            IsApprovedAdditionalInfo = true,
                            IsSendPortalAdditionalInfo = false,
                            StatusAddinationalInfoId = getOtherList2.Id,
                            Reason = model.Reason
                        });
                        AT_NOTIFICATION noti = new AT_NOTIFICATION()
                        {
                            CREATED_BY = sid,
                            TYPE = 10,
                            ACTION = 2,
                            TITLE = model.Reason,
                            STATUS_NOTIFY = 2,
                            EMP_NOTIFY_ID = getData.EMPLOYEE_ID.ToString(),
                            REF_ID = item,
                            MODEL_CHANGE = getData.MODEL_CHANGE,
                            CREATED_DATE = DateTime.Now
                        };
                        _dbContext.AtNotifications.AddRange(noti);
                        _dbContext.SaveChanges();
                        var employeeId = getData.EMPLOYEE_ID;
                        var users = _dbContext.SysUsers.Where(x => x.EMPLOYEE_ID == employeeId).ToList();
                        for (var i = 0; i < users.Count; i++)
                        {
                            var username = users[i].USERNAME;
                            if (!string.IsNullOrEmpty(username))
                            {
                                _hubContext.Clients.User(username).SendAsync("ReceiveMessage", new
                                {
                                    SignalType = "APPROVE_NOTIFICATION",
                                    Message = "Bạn có thông báo mới trên Portal"/*message*/,
                                    Data = new
                                    {

                                    }
                                });
                            }

                        }
                    });
                    var unApproved = await _genericRepository.UpdateRange(_uow, list, sid, pathMode);
                    if (unApproved != null)
                    {
                        return new FormatedResponse() { StatusCode = EnumStatusCode.StatusCode200, MessageCode = CommonMessageCode.UNAPPROVED_COMPLETE };
                    }
                    else
                    {
                        return new FormatedResponse() { StatusCode = EnumStatusCode.StatusCode400, ErrorType = EnumErrorType.CATCHABLE, MessageCode = CommonMessageCode.UNAPPROVED_FAILD };
                    }
                }

            }
            catch (Exception ex)
            {

                return new FormatedResponse() { StatusCode = EnumStatusCode.StatusCode500, ErrorType = EnumErrorType.UNCATCHABLE, MessageCode = ex.Message };
            }
        }

        public async Task<FormatedResponse> ApproveBankInfoEdit(GenericUnapprovePortalDTO model, string sid)
        {
            try
            {

                bool pathMode = true;
                HuEmployeeCvDTO dto = new();
                List<HuEmployeeCvEditDTO> list = new();
                List<HuEmployeeCvDTO> lstUpdate = new();
                var getOtherList = (from t in _uow.Context.Set<SYS_OTHER_LIST_TYPE>().Where(x => x.CODE == "STATUS")
                                    from o in _uow.Context.Set<SYS_OTHER_LIST>().DefaultIfEmpty()
                                    where o.CODE == "DD"
                                    select new { Id = o.ID }).FirstOrDefault();
                var getOtherList2 = (from t in _uow.Context.Set<SYS_OTHER_LIST_TYPE>().Where(x => x.CODE == "STATUS")
                                     from o in _uow.Context.Set<SYS_OTHER_LIST>().DefaultIfEmpty()
                                     where o.CODE == "TCPD"
                                     select new { Id = o.ID }).FirstOrDefault();
                if (model.ValueToBind == true)
                {
                    model.Ids.ForEach(item =>
                    {
                        list.Add(new()
                        {
                            Id = item,
                            IsApprovedBankInfo = true,
                            StatusApprovedBankId = getOtherList.Id

                        });


                    });
                    var approvedResponse = await _genericRepository.UpdateRange(_uow, list, sid, pathMode);
                    if (approvedResponse != null)
                    {
                        model.Ids.ForEach(item =>
                        {
                            var getData = _uow.Context.Set<HU_EMPLOYEE_CV_EDIT>().Where(x => x.ID == item).FirstOrDefault();
                            lstUpdate.Add(new()
                            {
                                Id = getData.HU_EMPLOYEE_CV_ID,
                                BankId = getData.BANK_ID,
                                BankNo = getData.BANK_NO,
                                BankBranch = getData.BANK_BRANCH_ID,
                                BankNo2 = getData.BANK_NO_2,
                                BankBranch2 = getData.BANK_BRANCH_ID_2,
                                BankId2 = getData.BANK_ID_2
                            });
                            AT_NOTIFICATION noti = new AT_NOTIFICATION()
                            {
                                CREATED_BY = sid,
                                TYPE = 12,
                                ACTION = 2,
                                TITLE = model.Reason,
                                STATUS_NOTIFY = 1,
                                EMP_NOTIFY_ID = getData.EMPLOYEE_ID.ToString(),
                                REF_ID = item,
                                MODEL_CHANGE = getData.MODEL_CHANGE,
                                CREATED_DATE = DateTime.Now
                            };
                            _dbContext.AtNotifications.AddRange(noti);
                            _dbContext.SaveChanges();
                            var employeeId = getData.EMPLOYEE_ID;
                            var users = _dbContext.SysUsers.Where(x => x.EMPLOYEE_ID == employeeId).ToList();
                            for (var i = 0; i < users.Count; i++)
                            {
                                var username = users[i].USERNAME;
                                if (!string.IsNullOrEmpty(username))
                                {
                                    _hubContext.Clients.User(username).SendAsync("ReceiveMessage", new
                                    {
                                        SignalType = "APPROVE_NOTIFICATION",
                                        Message = "Bạn có thông báo mới trên Portal"/*message*/,
                                        Data = new
                                        {

                                        }
                                    });
                                }

                            }
                        });
                        var updateResponse = await _genericParentRepository.UpdateRange(_uow, lstUpdate, sid, pathMode);
                        if (updateResponse != null)
                        {
                            return new FormatedResponse() { StatusCode = EnumStatusCode.StatusCode200, MessageCode = CommonMessageCode.APPROVED_SUCCESS };
                        }
                        else
                        {
                            return new FormatedResponse() { StatusCode = EnumStatusCode.StatusCode400, MessageCode = CommonMessageCode.APPROVE_FAIL };
                        }
                    }
                    else
                    {
                        return new FormatedResponse() { StatusCode = EnumStatusCode.StatusCode400, ErrorType = EnumErrorType.CATCHABLE, MessageCode = CommonMessageCode.APPROVE_FAIL };
                    }
                }
                else
                {

                    model.Ids.ForEach(item =>
                    {
                        var getData = _uow.Context.Set<HU_EMPLOYEE_CV_EDIT>().Where(x => x.ID == item).FirstOrDefault();
                        list.Add(new()
                        {
                            Id = item,
                            IsApprovedBankInfo = true,
                            IsSendPortalBankInfo = false,
                            StatusApprovedBankId = getOtherList2.Id,
                            Reason = model.Reason
                        });
                        AT_NOTIFICATION noti = new AT_NOTIFICATION()
                        {
                            CREATED_BY = sid,
                            TYPE = 12,
                            ACTION = 2,
                            TITLE = model.Reason,
                            STATUS_NOTIFY = 2,
                            EMP_NOTIFY_ID = getData.EMPLOYEE_ID.ToString(),
                            MODEL_CHANGE = getData.MODEL_CHANGE,
                            CREATED_DATE = DateTime.Now
                        };
                        _dbContext.AtNotifications.Add(noti);
                        _dbContext.SaveChanges();
                        var employeeId = getData.EMPLOYEE_ID;
                        var users = _dbContext.SysUsers.Where(x => x.EMPLOYEE_ID == employeeId).ToList();
                        for (var i = 0; i < users.Count; i++)
                        {
                            var username = users[i].USERNAME;
                            if (!string.IsNullOrEmpty(username))
                            {
                                _hubContext.Clients.User(username).SendAsync("ReceiveMessage", new
                                {
                                    SignalType = "APPROVE_NOTIFICATION",
                                    Message = "Bạn có thông báo mới trên Portal"/*message*/,
                                    Data = new
                                    {

                                    }
                                });
                            }

                        }
                    });
                    var unApproved = await _genericRepository.UpdateRange(_uow, list, sid, pathMode);
                    if (unApproved != null)
                    {

                        return new FormatedResponse() { StatusCode = EnumStatusCode.StatusCode200, MessageCode = CommonMessageCode.UNAPPROVED_COMPLETE };
                    }
                    else
                    {
                        return new FormatedResponse() { StatusCode = EnumStatusCode.StatusCode400, MessageCode = CommonMessageCode.UNAPPROVED_FAILD };
                    }
                }

            }
            catch (Exception ex)
            {

                return new FormatedResponse() { StatusCode = EnumStatusCode.StatusCode500, ErrorType = EnumErrorType.UNCATCHABLE, MessageCode = ex.Message };
            }
        }

        public async Task<FormatedResponse> ApproveEducationEdit(GenericUnapprovePortalDTO model)
        {
            try
            {
                string sid = "";
                bool pathMode = true;
                HuEmployeeCvDTO dto = new();
                List<HuEmployeeCvEditDTO> list = new();
                List<HuEmployeeCvDTO> lstUpdate = new();
                var getOtherList = (from t in _uow.Context.Set<SYS_OTHER_LIST_TYPE>().Where(x => x.CODE == "STATUS")
                                    from o in _uow.Context.Set<SYS_OTHER_LIST>().DefaultIfEmpty()
                                    where o.CODE == "DD"
                                    select new { Id = o.ID }).FirstOrDefault();
                var getOtherList2 = (from t in _uow.Context.Set<SYS_OTHER_LIST_TYPE>().Where(x => x.CODE == "STATUS")
                                     from o in _uow.Context.Set<SYS_OTHER_LIST>().DefaultIfEmpty()
                                     where o.CODE == "TCPD"
                                     select new { Id = o.ID }).FirstOrDefault();
                if (model.ValueToBind == true)
                {
                    model.Ids.ForEach(item =>
                    {
                        var getData = _uow.Context.Set<HU_EMPLOYEE_CV_EDIT>().Where(x => x.ID == item).FirstOrDefault();
                        list.Add(new()
                        {
                            Id = item,
                            IsApprovedEducation = true,
                            StatusApprovedEducationId = getOtherList.Id,
                            IsApprovedPortal = true

                        });
                        AT_NOTIFICATION noti = new AT_NOTIFICATION()
                        {
                            CREATED_BY = sid,
                            TYPE = 11,
                            ACTION = 2,
                            TITLE = model.Reason,
                            STATUS_NOTIFY = 1,
                            EMP_NOTIFY_ID = getData.EMPLOYEE_ID.ToString(),
                            REF_ID = item,
                            MODEL_CHANGE = getData.MODEL_CHANGE,
                            CREATED_DATE = DateTime.Now
                        };
                        _dbContext.AtNotifications.AddRange(noti);
                        _dbContext.SaveChanges();
                        var employeeId = getData.EMPLOYEE_ID;
                        var users = _dbContext.SysUsers.Where(x => x.EMPLOYEE_ID == employeeId).ToList();
                        for (var i = 0; i < users.Count; i++)
                        {
                            var username = users[i].USERNAME;
                            if (!string.IsNullOrEmpty(username))
                            {
                                _hubContext.Clients.User(username).SendAsync("ReceiveMessage", new
                                {
                                    SignalType = "APPROVE_NOTIFICATION",
                                    Message = "Bạn có thông báo mới trên Portal"/*message*/,
                                    Data = new
                                    {

                                    }
                                });
                            }

                        }

                    });
                    var approvedResponse = await _genericRepository.UpdateRange(_uow, list, sid, pathMode);
                    if (approvedResponse != null)
                    {
                        model.Ids.ForEach(item =>
                        {
                            var getData = _uow.Context.Set<HU_EMPLOYEE_CV_EDIT>().Where(x => x.ID == item).FirstOrDefault();

                            lstUpdate.Add(new()
                            {
                                Id = getData.HU_EMPLOYEE_CV_ID,
                                EducationLevelId = getData.EDUCATION_LEVEL_ID,
                                ComputerSkillId = getData.COMPUTER_SKILL_ID,
                                LicenseId = getData.LICENSE_ID

                                //LearningLevelId = getData.LEARNING_LEVEL_ID,
                                //Qualificationid = getData.QUALIFICATIONID,
                                //Qualificationid2 = getData.QUALIFICATIONID_2,
                                //Qualificationid3 = getData.QUALIFICATIONID_3,
                                //TrainingFormId = getData.TRAINING_FORM_ID,
                                //TrainingFormId2 = getData.TRAINING_FORM_ID_2,
                                //TrainingFormId3 = getData.TRAINING_FORM_ID_3
                            });

                        });
                        var updateResponse = await _genericParentRepository.UpdateRange(_uow, lstUpdate, sid, pathMode);
                        if (updateResponse != null)
                        {
                            return new FormatedResponse() { StatusCode = EnumStatusCode.StatusCode200, MessageCode = CommonMessageCode.APPROVED_SUCCESS };
                        }
                        else
                        {
                            return new FormatedResponse() { StatusCode = EnumStatusCode.StatusCode400, MessageCode = CommonMessageCode.APPROVE_FAIL };
                        }
                    }
                    else
                    {
                        return new FormatedResponse() { StatusCode = EnumStatusCode.StatusCode400, MessageCode = CommonMessageCode.APPROVE_FAIL };
                    }
                }
                else
                {
                    model.Ids.ForEach(item =>
                    {
                        var getData = _uow.Context.Set<HU_EMPLOYEE_CV_EDIT>().Where(x => x.ID == item).FirstOrDefault();
                        list.Add(new()
                        {
                            Id = item,
                            IsApprovedEducation = true,
                            IsApprovedPortal = true,
                            StatusApprovedEducationId = getOtherList2.Id,

                            IsSendPortal = false,

                            Reason = model.Reason
                        });
                        AT_NOTIFICATION noti = new AT_NOTIFICATION()
                        {
                            CREATED_BY = sid,
                            TYPE = 11,
                            ACTION = 2,
                            TITLE = model.Reason,
                            STATUS_NOTIFY = 2,
                            EMP_NOTIFY_ID = getData.EMPLOYEE_ID.ToString(),
                            MODEL_CHANGE = getData.MODEL_CHANGE,
                            CREATED_DATE = DateTime.Now
                        };
                        _dbContext.AtNotifications.Add(noti);
                        _dbContext.SaveChanges();
                        var employeeId = getData.EMPLOYEE_ID;
                        var users = _dbContext.SysUsers.Where(x => x.EMPLOYEE_ID == employeeId).ToList();
                        for (var i = 0; i < users.Count; i++)
                        {
                            var username = users[i].USERNAME;
                            if (!string.IsNullOrEmpty(username))
                            {
                                _hubContext.Clients.User(username).SendAsync("ReceiveMessage", new
                                {
                                    SignalType = "APPROVE_NOTIFICATION",
                                    Message = "Bạn có thông báo mới trên Portal"/*message*/,
                                    Data = new
                                    {

                                    }
                                });
                            }

                        }

                    });
                    var unApproved = await _genericRepository.UpdateRange(_uow, list, sid, pathMode);
                    if (unApproved != null)
                    {
                        return new FormatedResponse() { StatusCode = EnumStatusCode.StatusCode200, MessageCode = CommonMessageCode.UNAPPROVED_COMPLETE };
                    }
                    else
                    {
                        return new FormatedResponse() { StatusCode = EnumStatusCode.StatusCode400, MessageCode = CommonMessageCode.UNAPPROVED_FAILD };
                    }
                }

            }
            catch (Exception ex)
            {

                return new FormatedResponse() { StatusCode = EnumStatusCode.StatusCode500, ErrorType = EnumErrorType.UNCATCHABLE, MessageCode = ex.Message };
            }
        }
    }
}

