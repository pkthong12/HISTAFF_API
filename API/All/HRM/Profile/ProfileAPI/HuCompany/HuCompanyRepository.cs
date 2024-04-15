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

namespace API.Controllers.HuCompany
{
    public class HuCompanyRepository : IHuCompanyRepository
    {
        private readonly GenericUnitOfWork _uow;
        private readonly FullDbContext _dbContext;
        private IGenericRepository<HU_COMPANY, HuCompanyDTO> _genericRepository;
        private readonly GenericReducer<HU_COMPANY, HuCompanyDTO> _genericReducer;

        public HuCompanyRepository(FullDbContext context, GenericUnitOfWork uow)
        {
            _dbContext = context;
            _uow = uow;
            _genericRepository = _uow.GenericRepository<HU_COMPANY, HuCompanyDTO>();
            _genericReducer = new();
        }

        public async Task<GenericPhaseTwoListResponse<HuCompanyDTO>> SinglePhaseQueryList(GenericQueryListDTO<HuCompanyDTO> request)
        {
            var joined = (from p in _dbContext.HuCompanys.AsNoTracking()
                          from region in _dbContext.SysOtherLists.Where(x => x.ID == p.REGION_ID).DefaultIfEmpty()
                          from ins in _dbContext.SysOtherLists.Where(x => x.ID == p.INS_UNIT).DefaultIfEmpty()
                          from e in _dbContext.HuEmployees.Where(x => x.ID == p.REPRESENTATIVE_ID).DefaultIfEmpty()
                          from cv in _dbContext.HuEmployeeCvs.Where(x => x.ID == e.PROFILE_ID).DefaultIfEmpty()
                          from po in _dbContext.HuPositions.Where(x => x.ID == e.POSITION_ID).DefaultIfEmpty()
                          select new HuCompanyDTO
                          {
                              Id = p.ID,
                              Code = p.CODE,
                              NameEn = p.NAME_EN,
                              NameVn = p.NAME_VN,
                              ShortName = p.SHORT_NAME,
                              RegionId = p.REGION_ID,
                              RegionName = region.NAME,
                              GpkdAddress = p.GPKD_ADDRESS,
                              WorkAddress = p.WORK_ADDRESS,
                              InsUnit = p.INS_UNIT,
                              InsUnitName = ins.NAME,
                              PitCode = p.PIT_CODE,
                              PitCodeDate = p.PIT_CODE_DATE,
                              RepresentativeId = p.REPRESENTATIVE_ID,
                              RepresentativeName = cv.FULL_NAME,
                              RepresentativeTitle = po.NAME,
                              GpkdNo = p.GPKD_NO,
                              PhoneNumber = p.PHONE_NUMBER,
                              Email = p.EMAIL,
                              Fax = p.FAX,
                              Note = p.NOTE,
                              Order = p.ORDER,
                              IsActive = p.IS_ACTIVE,
                              IsActiveStr = p.IS_ACTIVE ? "Áp dụng" : "Ngừng áp dụng"
                          }).OrderByDescending(x => x.Order)
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
                    var list = new List<HU_COMPANY>
                    {
                        (HU_COMPANY)response
                    };
                    var joined = (from p in list
                                  select new HuCompanyDTO
                                  {
                                      Id = p.ID,
                                      Code = p.CODE,
                                      NameEn = p.NAME_EN,
                                      NameVn = p.NAME_VN,
                                      ShortName = p.SHORT_NAME,
                                      RegionId = p.REGION_ID,
                                      GpkdAddress = p.GPKD_ADDRESS,
                                      WorkAddress = p.WORK_ADDRESS,
                                      InsUnit = p.INS_UNIT,
                                      PitCode = p.PIT_CODE,
                                      PitCodeDate = p.PIT_CODE_DATE,
                                      RepresentativeId = p.REPRESENTATIVE_ID,
                                      GpkdNo = p.GPKD_NO,
                                      PhoneNumber = p.PHONE_NUMBER,
                                      Email = p.EMAIL,
                                      Fax = p.FAX,
                                      Note = p.NOTE,
                                      Order = p.ORDER,
                                      IsActive = p.IS_ACTIVE
                                  }).FirstOrDefault();

                    if (joined != null)
                    {
                        if (res.StatusCode == EnumStatusCode.StatusCode200)
                        {
                            var child = (from e in _dbContext.HuEmployees.Where(x => x.ID == joined.RepresentativeId)
                                         from cv in _dbContext.HuEmployeeCvs.Where(x => x.ID == e.PROFILE_ID).DefaultIfEmpty()
                                         from po in _dbContext.HuPositions.Where(x => x.ID == e.POSITION_ID).DefaultIfEmpty()
                                         select new
                                         {
                                             RepresentativeName = cv.FULL_NAME,
                                             RepresentativeTitle = po.NAME,
                                         }).FirstOrDefault();
                            joined.RepresentativeName = child?.RepresentativeName;
                            joined.RepresentativeTitle = child?.RepresentativeTitle;
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

        public async Task<FormatedResponse> Create(GenericUnitOfWork _uow, HuCompanyDTO dto, string sid)
        {
            dto.IsActive = true;
            var response = await _genericRepository.Create(_uow, dto, sid);
            return response;
        }

        public async Task<FormatedResponse> CreateRange(GenericUnitOfWork _uow, List<HuCompanyDTO> dtos, string sid)
        {
            var add = new List<HuCompanyDTO>();
            add.AddRange(dtos);
            var response = await _genericRepository.CreateRange(_uow, add, sid);
            return response;
        }

        public async Task<FormatedResponse> Update(GenericUnitOfWork _uow, HuCompanyDTO dto, string sid, bool patchMode = true)
        {
            var response = await _genericRepository.Update(_uow, dto, sid, patchMode);
            return response;
        }

        public async Task<FormatedResponse> UpdateRange(GenericUnitOfWork _uow, List<HuCompanyDTO> dtos, string sid, bool patchMode = true)
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
            try
            {
                bool isCheckDataUsing = false;
                bool isCheckNoneActive = false;
                ids.ForEach(item =>
                {
                    var getDataUsing = (from c in _uow.Context.Set<HU_COMPANY>()
                                  from o in _uow.Context.Set<HU_ORGANIZATION>().Where(x => x.COMPANY_ID == c.ID)
                                  where c.ID == item
                                  select new
                                  {
                                      Id = c.ID
                                  }).ToList();
                    var getDataNoneActive = from c in _uow.Context.Set<HU_COMPANY>().Where(x => x.ID == item && x.IS_ACTIVE == true)
                                            select new
                                            {
                                                Id = c.ID
                                            };
                    if (getDataUsing.Any())
                    {
                        isCheckDataUsing = true;
                        return;
                    }
                    if(getDataNoneActive.Any())
                    {
                        isCheckNoneActive = true;
                        return;
                    }
                });
                if(isCheckDataUsing == true)
                {
                    return new FormatedResponse() { StatusCode = EnumStatusCode.StatusCode400, ErrorType = EnumErrorType.CATCHABLE, MessageCode = CommonMessageCode.CAN_NOT_DELETE_RECORD_IS_USING };
                }
                else if(isCheckNoneActive == true)
                {
                    return new FormatedResponse() { StatusCode = EnumStatusCode.StatusCode400, ErrorType = EnumErrorType.UNCATCHABLE, MessageCode = CommonMessageCode.CAN_NOT_DELETE_RECORD_IS_ACTIVE };
                }
                else
                {
                    var response = await _genericRepository.DeleteIds(_uow, ids);
                    return response;

                }
            }
            catch (Exception ex)
            {

                return new FormatedResponse() { StatusCode = EnumStatusCode.StatusCode400, ErrorType = EnumErrorType.UNCATCHABLE, MessageCode = ex.Message };
            }
        }

        public async Task<FormatedResponse> DeleteIds(GenericUnitOfWork _uow, List<string> ids)
        {
            await Task.Run(() => null);
            throw new NotImplementedException();
        }

        public async Task<bool> CheckActive(List<long> ids)
        {
            if (await _dbContext.HuCompanys.Where(x => ids.Contains(x.ID) && x.IS_ACTIVE == true).CountAsync() == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public virtual async Task<FormatedResponse> ToggleActiveIds(GenericUnitOfWork _uow, List<long> ids, bool valueToBind, string sid)
        {
            var response = await _genericRepository.ToggleActiveIds(_uow, ids, valueToBind, sid);

            return response;
        }

        public async Task<FormatedResponse> CreateNewCode()
        {
            string newCode = "";
            if (await _dbContext.HuCompanys.CountAsync() == 0)
            {
                newCode = "01";
            }
            else
            {
                string lastestData = _dbContext.HuCompanys.OrderByDescending(t => t.ID).First().CODE!.ToString();

                newCode = (int.Parse(lastestData.Substring(lastestData.Length - 2)) + 1).ToString("D2");
            }

            return new FormatedResponse() { InnerBody = new { Code = newCode } };
        }


        // triển khai
        // phương thức submit áp dụng
        public async Task<FormatedResponse> SubmitActivate(List<long>? Ids)
        {
            // đầu tiên
            // phải sử dụng try ... catch ...
            // để bắt ngoại lệ
            try
            {
                // kiểm tra Ids phải khác null
                // và Ids phải có bản ghi
                // thì mới thực hiện công việc
                // thiết lập IS_ACTIVE = true
                if (Ids != null && Ids.Count() > 0)
                {
                    // truy vấn để lấy các bản ghi
                    // có ID tương ứng với ID
                    // của Ids
                    // cụ thể: sử dụng phương thức Contains()
                    var list_record = await _dbContext.HuCompanys.Where(x => Ids.Contains(x.ID) && x.IS_ACTIVE == false).ToListAsync();


                    // sử dụng vòng lặp foreach()
                    // để thiết lập IS_ACTIVE = true
                    // cho từng bản ghi truy vấn được
                    foreach (var item in list_record)
                    {
                        // thiết lập "IS_ACTIVE = true"
                        item.IS_ACTIVE = true;

                        // lưu sự thay đổi
                        // cụ thể: gọi phương thức SaveChanges()
                        _dbContext.SaveChanges();
                    }

                    return new FormatedResponse()
                    {
                        StatusCode = EnumStatusCode.StatusCode200,
                        MessageCode = "Thiết lập trạng thái áp dụng thành công"
                    };
                }
                else
                {
                    // viết code cho trường hợp
                    // người dùng chưa chọn bản ghi
                    // mà đã bấm phê duyệt
                    return new FormatedResponse()
                    {
                        StatusCode = EnumStatusCode.StatusCode400,
                        MessageCode = "Bạn chưa chọn bản ghi để thiết lập trạng thái áp dụng"
                    };
                }
            }
            catch (Exception ex)
            {
                return new FormatedResponse() { InnerBody = ex.Message };
            }
        }


        // triển khai
        // phương thức submit ngừng áp dụng
        public async Task<FormatedResponse> SubmitStopActivate(List<long>? Ids)
        {
            // đầu tiên
            // phải sử dụng try ... catch ...
            // để bắt ngoại lệ
            try
            {
                // kiểm tra Ids phải khác null
                // và Ids phải có bản ghi
                // thì mới thực hiện công việc
                // thiết lập IS_ACTIVE = false
                if (Ids != null && Ids.Count() > 0)
                {
                    // truy vấn để lấy các bản ghi
                    // có ID tương ứng với ID
                    // của Ids
                    // cụ thể: sử dụng phương thức Contains()
                    var list_record = await _dbContext.HuCompanys.Where(x => Ids.Contains(x.ID) && x.IS_ACTIVE == true).ToListAsync();


                    // sử dụng vòng lặp foreach()
                    // để thiết lập IS_ACTIVE = false
                    // cho từng bản ghi truy vấn được
                    foreach (var item in list_record)
                    {
                        // thiết lập "IS_ACTIVE = false"
                        item.IS_ACTIVE = false;

                        // lưu sự thay đổi
                        // cụ thể: gọi phương thức SaveChanges()
                        _dbContext.SaveChanges();
                    }

                    return new FormatedResponse()
                    {
                        StatusCode = EnumStatusCode.StatusCode200,
                        MessageCode = "Thiết lập trạng thái ngừng áp dụng thành công"
                    };
                }
                else
                {
                    // viết code cho trường hợp
                    // người dùng chưa chọn bản ghi
                    // mà đã bấm phê duyệt
                    return new FormatedResponse()
                    {
                        StatusCode = EnumStatusCode.StatusCode400,
                        MessageCode = "Bạn chưa chọn bản ghi để thiết lập trạng ngừng thái áp dụng"
                    };
                }
            }
            catch (Exception ex)
            {
                return new FormatedResponse() { InnerBody = ex.Message };
            }
        }

    }
}

