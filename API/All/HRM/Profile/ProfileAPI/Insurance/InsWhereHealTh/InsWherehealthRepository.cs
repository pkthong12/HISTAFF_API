using CORE.GenericUOW;
using CORE.DTO;
using API.DTO;
using CORE.Enum;
using CORE.StaticConstant;
using API.All.DbContexts;
using CORE.AutoMapper;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;

namespace API.Controllers.InsWherehealth
{
    public class InsWherehealthRepository : IInsWherehealthRepository
    {
        private readonly GenericUnitOfWork _uow;
        private readonly FullDbContext _dbContext;
        private IGenericRepository<INS_WHEREHEALTH, InsWherehealthDTO> _genericRepository;
        private readonly GenericReducer<INS_WHEREHEALTH, InsWherehealthDTO> _genericReducer;

        public InsWherehealthRepository(FullDbContext context, GenericUnitOfWork uow)
        {
            _dbContext = context;
            _uow = uow;
            _genericRepository = _uow.GenericRepository<INS_WHEREHEALTH, InsWherehealthDTO>();
            _genericReducer = new();
        }

        public async Task<GenericPhaseTwoListResponse<InsWherehealthDTO>> SinglePhaseQueryList(GenericQueryListDTO<InsWherehealthDTO> request)
        {
            var joined = from i in _dbContext.InsWhereHealThs.AsNoTracking()
                         from p in _uow.Context.Set<HU_PROVINCE>().AsNoTracking().Where(x => x.ID == i.PROVINCE_ID).DefaultIfEmpty()
                         from d in _uow.Context.Set<HU_DISTRICT>().AsNoTracking().Where(x => x.ID == i.DISTRICT_ID).DefaultIfEmpty()
                             // JOIN OTHER ENTITIES BASED ON THE BUSINESS
                         select new InsWherehealthDTO
                         {
                             Id = i.ID,
                             NameVn = i.NAME_VN,
                             Code = i.CODE,
                             ProvinceName = p.NAME,
                             DistrictName = d.NAME,
                             Address = i.ADDRESS,
                             Note = i.NOTE,
                             Status = (i.IS_ACTIVE ?? false) ? "Áp dụng" : "Ngừng áp dụng",
                             IsActive = i.IS_ACTIVE
                         };

            var singlePhaseResult = await _genericReducer.SinglePhaseReduce(joined, request);
            return singlePhaseResult;
        }

        public async Task<FormatedResponse> ReadAll()
        {
            var resposne = await _genericRepository.ReadAll();
            return resposne;
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
                var list = new List<INS_WHEREHEALTH>
                    {
                        (INS_WHEREHEALTH)response
                    };
                var joined = (from l in list
                                  // JOIN OTHER ENTITIES BASED ON THE BUSINESS
                              select new InsWherehealthDTO
                              {
                                  Id = l.ID,
                                  NameVn = l.NAME_VN,
                                  Code = l.CODE,
                                  ProvinceId = l.PROVINCE_ID,
                                  DistrictId = l.DISTRICT_ID,
                                  Address = l.ADDRESS,
                                  Note = l.NOTE
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

        public async Task<FormatedResponse> Create(GenericUnitOfWork _uow, InsWherehealthDTO dto, string sid)
        {
            dto.IsActive = true;
            var response = await _genericRepository.Create(_uow, dto, sid);

            return response;
        }

        public async Task<FormatedResponse> CreateRange(GenericUnitOfWork _uow, List<InsWherehealthDTO> dtos, string sid)
        {
            var add = new List<InsWherehealthDTO>();
            add.AddRange(dtos);
            var response = await _genericRepository.CreateRange(_uow, add, sid);
            return response;
        }

        public async Task<FormatedResponse> Update(GenericUnitOfWork _uow, InsWherehealthDTO dto, string sid, bool patchMode = true)
        {
            var response = await _genericRepository.Update(_uow, dto, sid, patchMode);
            return response;
        }

        public async Task<FormatedResponse> UpdateRange(GenericUnitOfWork _uow, List<InsWherehealthDTO> dtos, string sid, bool patchMode = true)
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
            // lấy các bản ghi
            // theo id
            var list_record = _dbContext.InsWhereHealThs.Where(x => ids.Contains(x.ID)).AsNoTracking().DefaultIfEmpty();

            // kiểm tra trạng thái
            foreach (var item in list_record)
            {
                // nếu bản ghi có trạng thái "áp dụng"
                // thì dừng luôn
                // không cho xóa
                if (item?.IS_ACTIVE == true)
                {
                    return new FormatedResponse()
                    {
                        ErrorType = EnumErrorType.CATCHABLE,
                        MessageCode = CommonMessageCode.DO_NOT_DELETE_DATA_IS_ACTIVE,
                        StatusCode = EnumStatusCode.StatusCode400
                    };
                }
            }


            var response = await _genericRepository.DeleteIds(_uow, ids);
            return response;
        }

        public async Task<FormatedResponse> DeleteIds(GenericUnitOfWork _uow, List<string> ids)
        {
            await Task.Run(() => null);
            throw new NotImplementedException();
        }

        public async Task<FormatedResponse> GetAllProvince()
        {
            var province = await(from p in _uow.Context.Set<HU_PROVINCE>().AsNoTracking() 
                                 select new
                                 {
                                     Id = p.ID,
                                     Name = p.NAME,
                                 }).ToListAsync();
            return new FormatedResponse() { InnerBody = province };
        }

        public async Task<FormatedResponse> GetAllDistrictByProvinceId(long provinceId)
        {
            var district = await (from d in _uow.Context.Set<HU_DISTRICT>().AsNoTracking().Where(x => x.PROVINCE_ID == provinceId)
                                  select new
                                  {
                                      Id = d.ID,
                                      Name = d.NAME,
                                  }).ToListAsync();
            return new FormatedResponse() { InnerBody =  district };
        }


        // thêm code
        // cho chức năng thay đổi trạng thái
        public virtual async Task<FormatedResponse> ToggleActiveIds(GenericUnitOfWork _uow, List<long> ids, bool valueToBind, string sid)
        {
            var response = await _genericRepository.ToggleActiveIds(_uow, ids, valueToBind, sid);

            return response;
        }
    }
}

