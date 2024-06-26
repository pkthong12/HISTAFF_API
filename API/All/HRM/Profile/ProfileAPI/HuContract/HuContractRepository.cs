﻿using CORE.GenericUOW;
using CORE.DTO;
using API.DTO;
using CORE.Enum;
using CORE.StaticConstant;
using API.All.DbContexts;
using CORE.AutoMapper;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;
using System;
using Common.Extensions;
using ProfileDAL.ViewModels;

namespace API.Controllers.HuContract
{
    public class HuContractRepository : IHuContractRepository
    {
        private readonly GenericUnitOfWork _uow;
        private readonly FullDbContext _dbContext;
        private IGenericRepository<HU_CONTRACT, HuContractDTO> _genericRepository;
        private readonly GenericReducer<HU_CONTRACT, HuContractDTO> _genericReducer;

        public HuContractRepository(FullDbContext context, GenericUnitOfWork uow)
        {
            _dbContext = context;
            _uow = uow;
            _genericRepository = _uow.GenericRepository<HU_CONTRACT, HuContractDTO>();
            _genericReducer = new();
        }

        public async Task<GenericPhaseTwoListResponse<HuContractDTO>> SinglePhaseQueryList(GenericQueryListDTO<HuContractDTO> request)
        {
            var joined = from p in _dbContext.HuContracts.AsNoTracking()
                         // JOIN OTHER ENTITIES BASED ON THE BUSINESS
                         select new HuContractDTO
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
                
                var list = new List<HU_CONTRACT>
                    {
                        (HU_CONTRACT)response
                    };
                
                var joined = (from l in list

                              // JOIN OTHER ENTITIES BASED ON THE BUSINESS
                              from tham_chieu_1 in _dbContext.HuEmployees.Where(x => x.ID == l.EMPLOYEE_ID).DefaultIfEmpty()

                              join t in _dbContext.HuPositions on tham_chieu_1.POSITION_ID equals t.ID into tmp2
                              from t1 in tmp2.DefaultIfEmpty()

                              join o in _dbContext.HuOrganizations on tham_chieu_1.ORG_ID equals o.ID

                              join o2 in _dbContext.HuOrganizations on o.PARENT_ID equals o2.ID into tmp1
                              from o3 in tmp1.DefaultIfEmpty()

                              join w in _dbContext.HuWorkings on l.WORKING_ID equals w.ID into tmp5
                              from w1 in tmp5.DefaultIfEmpty()

                              from tl in _dbContext.HuSalaryTypes.Where(c => c.ID == w1.SALARY_TYPE_ID).DefaultIfEmpty()

                              from sc in _dbContext.HuSalaryScales.Where(c => c.ID == w1.SALARY_SCALE_ID).DefaultIfEmpty()

                              from ra in _dbContext.HuSalaryRanks.Where(c => c.ID == w1.SALARY_RANK_ID).DefaultIfEmpty()

                              from sl in _dbContext.HuSalaryLevels.Where(c => c.ID == w1.SALARY_LEVEL_ID).DefaultIfEmpty()

                              from tax in _dbContext.SysOtherLists.Where(c => c.ID == w1.TAXTABLE_ID).DefaultIfEmpty()

                              from scdcv in _dbContext.HuSalaryScales.Where(c => c.ID == w1.SALARY_SCALE_DCV_ID).DefaultIfEmpty()

                              from radcv in _dbContext.HuSalaryRanks.Where(c => c.ID == w1.SALARY_RANK_DCV_ID).DefaultIfEmpty()

                              from sldcv in _dbContext.HuSalaryLevels.Where(c => c.ID == w1.SALARY_LEVEL_DCV_ID).DefaultIfEmpty()

                              from wo in _dbContext.HuOrganizations.Where(c => c.ID == w1.ORG_ID).DefaultIfEmpty()
                              from com in _dbContext.HuCompanys.Where(c => c.ID == wo.COMPANY_ID).DefaultIfEmpty()
                              from re in _dbContext.SysOtherLists.Where(x => x.ID == com.REGION_ID).DefaultIfEmpty()

                              select new
                              {
                                  Id = l.ID,
                                  EmployeeId = l.EMPLOYEE_ID,
                                  EmployeeName = tham_chieu_1.Profile!.FULL_NAME,
                                  EmployeeCode = tham_chieu_1.CODE,
                                  PositionName = t1.NAME,
                                  OrgId = tham_chieu_1.ORG_ID,
                                  OrgName = o.NAME,
                                  OrgParentName = o3.NAME,
                                  StartDate = l.START_DATE,
                                  ExpireDate = l.EXPIRE_DATE,
                                  ContractNo = l.CONTRACT_NO,
                                  ContractTypeId = l.CONTRACT_TYPE_ID,
                                  StatusId = l.STATUS_ID,
                                  SignId = l.SIGN_ID,
                                  SignerName = l.SIGNER_NAME,
                                  SignerPosition = l.SIGNER_POSITION,
                                  SignDate = l.SIGN_DATE,
                                  WorkingId = l.WORKING_ID,
                                  WorkingNo = w1.DECISION_NO,
                                  SalBasic = w1.SAL_BASIC,
                                  salTotal = w1.SAL_TOTAL,
                                  Note = l.NOTE,
                                  SalPercent = w1.SAL_PERCENT,
                                  SalaryType = tl.NAME,
                                  SalaryScaleName = sc.NAME,
                                  SalaryRankName = ra.NAME,
                                  SalaryLevelName = sl.NAME,
                                  ShortTempSalary = w1.SHORT_TEMP_SALARY,
                                  TaxtableName = tax.NAME,
                                  Coefficient = w1.COEFFICIENT,
                                  CoefficientDcv = w1.COEFFICIENT_DCV,
                                  salaryScaleDcvName = scdcv.NAME,
                                  salaryRankDcvName = radcv.NAME,
                                  salaryLevelDcvName = sldcv.NAME,
                                  regionName = re.NAME
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

        public async Task<FormatedResponse> Create(GenericUnitOfWork _uow, HuContractDTO dto, string sid)
        {
            var response = await _genericRepository.Create(_uow, dto, sid);
            return response;
        }

        public async Task<FormatedResponse> CreateRange(GenericUnitOfWork _uow, List<HuContractDTO> dtos, string sid)
        {
            var add = new List<HuContractDTO>();
            add.AddRange(dtos);
            var response = await _genericRepository.CreateRange(_uow, add, sid);
            return response;
        }

        public async Task<FormatedResponse> Update(GenericUnitOfWork _uow, HuContractDTO dto, string sid, bool patchMode = true)
        {
            var response = await _genericRepository.Update(_uow, dto, sid, patchMode);
            return response;
        }

        public async Task<FormatedResponse> UpdateRange(GenericUnitOfWork _uow, List<HuContractDTO> dtos, string sid, bool patchMode = true)
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

        public Task<FormatedResponse> ToggleActiveIds(GenericUnitOfWork _uow, List<long> ids, bool valueToBind, string sid)
        {
            // bạn muốn viết gì
            // để triển khai
            // thì tự viết ở đây
            throw new NotImplementedException();
        }

        public async Task<bool> UpdateStatusEmpDetailOfContract(HU_CONTRACT obj)
        {
            try
            {
                if (obj.START_DATE!.Value.Date == DateTime.Now.Date && obj.STATUS_ID == OtherConfig.STATUS_APPROVE)
                {
                    var r = _dbContext.HuContracts.Where(x => x.ID == obj.ID).FirstOrDefault();

                    if (r != null)
                    {
                        var e = _dbContext.HuEmployees.Where(x => x.ID == r.EMPLOYEE_ID).FirstOrDefault();
                        var terminate = _dbContext.HuTerminates.Where(x => x.EMPLOYEE_ID == r.EMPLOYEE_ID).OrderByDescending(x => x.EFFECT_DATE).FirstOrDefault();
                        var eprofile = (from p in _dbContext.HuEmployees.Where(x => x.ID == r.EMPLOYEE_ID) select p.Profile).FirstOrDefault();
                        if (obj.STATUS_ID == OtherConfig.STATUS_APPROVE)
                        {

                            var _TypeEmpStatus = (from p in _dbContext.SysOtherLists.Where(p => p.IS_ACTIVE == true)
                                                  join ot in _dbContext.SysOtherListTypes on p.TYPE_ID equals ot.ID
                                                  where ot.CODE == "EMP_STATUS" && p.CODE == "ESW"
                                                  select p.ID).FirstOrDefault();
                            var _TypeEmpStatusDetail = (from p in _dbContext.SysOtherLists.Where(p => p.IS_ACTIVE == true)
                                                        join ot in _dbContext.SysOtherListTypes on p.TYPE_ID equals ot.ID
                                                        where ot.CODE == "STATUS_DETAIL" && ot.IS_ACTIVE == true && p.IS_ACTIVE == true
                                                        select p).ToArray();

                            var contractType = (from p in _dbContext.HuContractTypes.Where(p => p.IS_ACTIVE == true)
                                                join ot in _dbContext.SysContractTypes on p.TYPE_ID equals ot.ID
                                                where p.ID == r.CONTRACT_TYPE_ID
                                                select ot).FirstOrDefault();

                            if (e != null)
                            {
                                e.CONTRACT_ID = obj.ID;
                                e.CONTRACT_TYPE_ID = r.CONTRACT_TYPE_ID;
                                if (_TypeEmpStatus != null && (terminate == null ? true : r.START_DATE > terminate?.EFFECT_DATE))
                                {
                                    e.WORK_STATUS_ID = _TypeEmpStatus;
                                }
                                if (contractType != null && _TypeEmpStatusDetail.Length > 0)
                                {
                                    //cap nhat trang thai chi tiet
                                    switch (contractType.CODE!.ToUpper())
                                    {
                                        case "HDTV":
                                            var _IDHDTV = (from p in _TypeEmpStatusDetail where p.CODE == "00003" select p.ID).FirstOrDefault();
                                            if (_IDHDTV != null)
                                            {
                                                e.STATUS_DETAIL_ID = _IDHDTV;
                                            }
                                            break;
                                        case "HDLD001" or "HDLD002" or "HDLD003":
                                            var _IDXDTH = (from p in _TypeEmpStatusDetail where p.CODE == "00004" select p.ID).FirstOrDefault();
                                            if (_IDXDTH != null)
                                            {
                                                e.STATUS_DETAIL_ID = _IDXDTH;
                                            }
                                            break;
                                        case "HDLD004":
                                            var _IDKXDTH = (from p in _TypeEmpStatusDetail where p.CODE == "00005" select p.ID).FirstOrDefault();
                                            if (_IDKXDTH != null)
                                            {
                                                e.STATUS_DETAIL_ID = _IDKXDTH;
                                            }
                                            break;
                                        default:
                                            break;
                                    }
                                }
                                _dbContext.HuEmployees.Update(e);
                            }
                        }
                        await _dbContext.SaveChangesAsync();
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public void ScanUpdateStatusEmpDetail()
        {
            var scanList = _dbContext.HuContracts.Where(
                x => x.STATUS_ID == OtherConfig.STATUS_APPROVE && x.START_DATE!.Value.Date == DateTime.Now.Date).ToList();
            scanList.ForEach(async data =>
            {
                await UpdateStatusEmpDetailOfContract(data);
            });

        }

    }
}

