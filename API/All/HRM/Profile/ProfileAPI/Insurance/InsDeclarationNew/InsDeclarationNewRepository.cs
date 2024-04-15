using CORE.GenericUOW;
using CORE.DTO;
using API.DTO;
using CORE.Enum;
using CORE.StaticConstant;
using API.All.DbContexts;
using CORE.AutoMapper;
using System;
using API.Entities;
using Microsoft.AspNetCore.Http.HttpResults;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Logical;

namespace API.Controllers.InsDeclarationNew
{
    public class InsDeclarationNewRepository : IInsDeclarationNewRepository
    {
        private readonly GenericUnitOfWork _uow;
        private readonly FullDbContext _dbContext;
        private IGenericRepository<INS_DECLARATION_NEW, InsDeclarationNewDTO> _genericRepository;
        private readonly GenericReducer<INS_DECLARATION_NEW, InsDeclarationNewDTO> _genericReducer;

        public InsDeclarationNewRepository(FullDbContext context, GenericUnitOfWork uow)
        {
            _dbContext = context;
            _uow = uow;
            _genericRepository = _uow.GenericRepository<INS_DECLARATION_NEW, InsDeclarationNewDTO>();
            _genericReducer = new();
        }

        public async Task<GenericPhaseTwoListResponse<InsDeclarationNewDTO>> SinglePhaseQueryList(GenericQueryListDTO<InsDeclarationNewDTO> request)
        {
            var joined = (from p in _dbContext.InsDeclarationNews.AsNoTracking()
                          from tham_chieu_1 in _dbContext.HuOrganizations.Where(x => x.ID == p.ID_ORG).DefaultIfEmpty()
                          from tham_chieu_2 in _dbContext.HuPositions.Where(x => x.ID == p.ID_POSITION).DefaultIfEmpty()
                          from tham_chieu_3 in _dbContext.SysOtherLists.Where(x => x.ID == p.ID_UNIT_INSURANCE).DefaultIfEmpty()
                          from tham_chieu_4 in _dbContext.HuEmployees.Where(x => x.ID == p.ID_EMPLOYEE).DefaultIfEmpty()
                          select new InsDeclarationNewDTO
                          {
                              // trường 1:
                              Id = p.ID,

                              // trường 2:
                              EmplyeeCode = p.EMPLOYEE_CODE,

                              // trường 3:
                              FullName = p.FULL_NAME,

                              // trường 4:
                              IdOrg = p.ID_ORG,

                              // trường 4.5:
                              OrgName = tham_chieu_1.NAME,

                              // trường 5:
                              IdPosition = p.ID_POSITION,

                              // trường 5.5:
                              PositionName = tham_chieu_2.NAME,

                              // trường 6:
                              IdUnitInsurance = p.ID_UNIT_INSURANCE,

                              // trường 6.5:
                              UnitInsuranceName = tham_chieu_3.NAME,

                              // trường 7:
                              IdTypeBdbh = p.ID_TYPE_BDBH,

                              // trường 7.5:
                              TypeBdbhName = tham_chieu_3.NAME,

                              // trường 8:
                              IdEmployee = p.ID_EMPLOYEE,

                              // trường 8.5:
                              // chỗ này sẽ lấy ra số của cái sổ bảo hiểm
                              // trong bảng HU_EMPLOYEE
                              // nhưng mà phải đợi dev thêm cái INSURANCE_NUMBER
                              // vào trong bảng HU_EMPLOYEE đã
                              // code đang comment bên dưới là tôi viết trước
                              // để chuẩn bị cho tương lai thôi
                              // InsuranceNumber = tham_chieu_4.INSURANCE_NUMBER

                              // trường 9:
                              EffectiveDate = p.EFFECTIVE_DATE,

                              // trường 10:
                              SalaryBhxhBhytOld = p.SALARY_BHXH_BHYT_OLD,

                              // trường 11:
                              SalaryBhtnOld = p.SALARY_BHTN_OLD,

                              // trường 12:
                              SalaryBhxhBhytNew = p.SALARY_BHXH_BHYT_NEW,

                              // trường 13:
                              SalaryBhtnNew = p.SALARY_BHTN_NEW,

                              // trường 14:
                              ChecklistBhxh = p.CHECKLIST_BHXH,

                              // trường 15:
                              ChecklistBhyt = p.CHECKLIST_BHYT,

                              // trường 16:
                              ChecklistBhtn = p.CHECKLIST_BHTN,

                              // trường 17:
                              ChecklistBhtnldBnn = p.CHECKLIST_BHTNLD_BNN,

                              // trường 18:
                              Note = p.NOTE,

                              // trường 19:
                              CreatedDate = p.CREATED_DATE,

                              // trường 20:
                              CreatedBy = p.CREATED_BY,

                              // trường 21:
                              CreatedLog = p.CREATED_LOG,

                              // trường 22:
                              UpdatedDate = p.UPDATED_DATE,

                              // trường 23:
                              UpdatedBy = p.UPDATED_BY,

                              // trường 24:
                              UpdatedLog = p.UPDATED_LOG,
                          }).OrderByDescending(x => x.CreatedDate)
                         ;

            var singlePhaseResult = await _genericReducer.SinglePhaseReduce(joined, request);
            return singlePhaseResult;
        }

        public async Task<FormatedResponse> ReadAll()
        {
            var response = await _genericRepository.ReadAll();
            return response;
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
            try
            {
                var res = await _genericRepository.GetById(id);
                if (res.InnerBody != null)
                {
                    var response = res.InnerBody;
                    var list = new List<INS_DECLARATION_NEW>
                    {
                        (INS_DECLARATION_NEW)response
                    };
                    var joined = (from p in list
                                  select new InsDeclarationNewDTO
                                  {
                                      // trường 1:
                                      Id = p.ID,

                                      // trường 2:
                                      EmplyeeCode = p.EMPLOYEE_CODE,

                                      // trường 3:
                                      FullName = p.FULL_NAME,

                                      // trường 4:
                                      IdOrg = p.ID_ORG,

                                      // trường 5:
                                      IdPosition = p.ID_POSITION,

                                      // trường 6:
                                      IdUnitInsurance = p.ID_UNIT_INSURANCE,

                                      // trường 7:
                                      IdTypeBdbh = p.ID_TYPE_BDBH,

                                      // trường 8:
                                      IdEmployee = p.ID_EMPLOYEE,

                                      // trường 9:
                                      EffectiveDate = p.EFFECTIVE_DATE,

                                      // trường 10:
                                      SalaryBhxhBhytOld = p.SALARY_BHXH_BHYT_OLD,

                                      // trường 11:
                                      SalaryBhtnOld = p.SALARY_BHTN_OLD,

                                      // trường 12:
                                      SalaryBhxhBhytNew = p.SALARY_BHXH_BHYT_NEW,

                                      // trường 13:
                                      SalaryBhtnNew = p.SALARY_BHTN_NEW,

                                      // trường 14:
                                      ChecklistBhxh = p.CHECKLIST_BHXH,

                                      // trường 15:
                                      ChecklistBhyt = p.CHECKLIST_BHYT,

                                      // trường 16:
                                      ChecklistBhtn = p.CHECKLIST_BHTN,

                                      // trường 17:
                                      ChecklistBhtnldBnn = p.CHECKLIST_BHTNLD_BNN,

                                      // trường 18:
                                      Note = p.NOTE,

                                      // trường 19:
                                      CreatedDate = p.CREATED_DATE,

                                      // trường 20:
                                      CreatedBy = p.CREATED_BY,

                                      // trường 21:
                                      CreatedLog = p.CREATED_LOG,

                                      // trường 22:
                                      UpdatedDate = p.UPDATED_DATE,

                                      // trường 23:
                                      UpdatedBy = p.UPDATED_BY,

                                      // trường 24:
                                      UpdatedLog = p.UPDATED_LOG,
                                  }).FirstOrDefault();

                    if (joined != null)
                    {
                        if (res.StatusCode == EnumStatusCode.StatusCode200)
                        {
                            // đây là chỗ truy vấn
                            // để lấy ra được tên tương ứng với id
                            var child = (from tham_chieu_1 in _dbContext.HuOrganizations.Where(x => x.ID == joined.IdOrg).DefaultIfEmpty()
                                         from tham_chieu_2 in _dbContext.HuPositions.Where(x => x.ID == joined.IdPosition).DefaultIfEmpty()
                                         from tham_chieu_3 in _dbContext.SysOtherLists.Where(x => x.ID == joined.IdUnitInsurance).DefaultIfEmpty()
                                         from tham_chieu_4 in _dbContext.HuEmployees.Where(x => x.ID == joined.IdEmployee).DefaultIfEmpty()
                                         select new
                                         {
                                             OrgName = tham_chieu_1.NAME,
                                             PositionName = tham_chieu_2.NAME,
                                             UnitInsuranceName = tham_chieu_3.NAME,
                                             TypeBdbhName = tham_chieu_3.NAME,
                                             // InsuranceNumber = tham_chieu_4.INSURANCE_NUMBER
                                         }).FirstOrDefault();
                            joined.OrgName = child?.OrgName;
                            joined.PositionName = child?.PositionName;
                            joined.UnitInsuranceName = child?.UnitInsuranceName;
                            joined.TypeBdbhName = child?.TypeBdbhName;
                            return new FormatedResponse() { InnerBody = joined };
                        }
                        else
                        {
                            return new() { ErrorType = EnumErrorType.CATCHABLE, MessageCode = CommonMessageCode.GET_LIST_BY_KEY_RESPOSNE_IS_NOT_OF_STATUS_CODE_200, StatusCode = EnumStatusCode.StatusCode400 };
                        }
                    }
                    else
                    {
                        return new() { ErrorType = EnumErrorType.CATCHABLE, MessageCode = CommonMessageCode.JOINED_QUERY_AFTER_GET_BY_ID_WAS_NULL, StatusCode = EnumStatusCode.StatusCode400 };
                    }
                }
                else
                {
                    return new FormatedResponse() { MessageCode = CommonMessageCode.ENTITY_NOT_FOUND, ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400 };
                }

            }
            catch (Exception ex)
            {
                return new FormatedResponse() { MessageCode = ex.Message, ErrorType = EnumErrorType.UNCATCHABLE, StatusCode = EnumStatusCode.StatusCode500 };

            }
        }
        public async Task<FormatedResponse> GetById(string id)
        {
            await Task.Run(() => null);
            throw new NotImplementedException();
        }

        public async Task<FormatedResponse> Create(GenericUnitOfWork _uow, InsDeclarationNewDTO dto, string sid)
        {
            // bảng InsDeclarationNewDTO
            // không có trường trạng thái
            // dto.IsActive = true;

            var response = await _genericRepository.Create(_uow, dto, sid);
            return response;
        }

        public async Task<FormatedResponse> CreateRange(GenericUnitOfWork _uow, List<InsDeclarationNewDTO> dtos, string sid)
        {
            var add = new List<InsDeclarationNewDTO>();
            add.AddRange(dtos);
            var response = await _genericRepository.CreateRange(_uow, add, sid);
            return response;
        }

        public async Task<FormatedResponse> Update(GenericUnitOfWork _uow, InsDeclarationNewDTO dto, string sid, bool patchMode = true)
        {
            var response = await _genericRepository.Update(_uow, dto, sid, patchMode);
            return response;
        }

        public async Task<FormatedResponse> UpdateRange(GenericUnitOfWork _uow, List<InsDeclarationNewDTO> dtos, string sid, bool patchMode = true)
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

    }
}

