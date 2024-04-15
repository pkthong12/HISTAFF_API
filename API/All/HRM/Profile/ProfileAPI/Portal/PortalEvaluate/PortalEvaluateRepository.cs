using CORE.GenericUOW;
using CORE.DTO;
using API.DTO;
using CORE.Enum;
using CORE.StaticConstant;
using API.All.DbContexts;
using CORE.AutoMapper;

namespace API.All.HRM.Profile.ProfileAPI.Portal.PortalEvaluate
{
    public class PortalEvaluateRepository : IPortalEvaluateRepository
    {
        private readonly GenericUnitOfWork _uow;
        private readonly FullDbContext _dbContext;
        private IGenericRepository<HU_EVALUATE, HuEvaluateDTO> _genericRepository;
        private readonly GenericReducer<HU_EVALUATE, HuEvaluateDTO> _genericReducer;

        public PortalEvaluateRepository(FullDbContext context, GenericUnitOfWork uow)
        {
            _dbContext = context;
            _uow = uow;
            _genericRepository = _uow.GenericRepository<HU_EVALUATE, HuEvaluateDTO>();
            _genericReducer = new();
        }

        public async Task<GenericPhaseTwoListResponse<HuEvaluateDTO>> SinglePhaseQueryList(GenericQueryListDTO<HuEvaluateDTO> request)
        {
            var joined = from p in _dbContext.HuEvaluates.DefaultIfEmpty()
                         // JOIN OTHER ENTITIES BASED ON THE BUSINESS
                         select new HuEvaluateDTO
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
                var list = new List<HU_EVALUATE>
                    {
                        (HU_EVALUATE)response
                    };
                var joined = (from l in list
                              // JOIN OTHER ENTITIES BASED ON THE BUSINESS
                              select new HuEvaluateDTO
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

        public async Task<FormatedResponse> Create(GenericUnitOfWork _uow, HuEvaluateDTO dto, string sid)
        {
            var response = await _genericRepository.Create(_uow, dto, sid);
            return response;
        }

        public async Task<FormatedResponse> CreateRange(GenericUnitOfWork _uow, List<HuEvaluateDTO> dtos, string sid)
        {
            var add = new List<HuEvaluateDTO>();
            add.AddRange(dtos);
            var response = await _genericRepository.CreateRange(_uow, add, sid);
            return response;
        }

        public async Task<FormatedResponse> Update(GenericUnitOfWork _uow, HuEvaluateDTO dto, string sid, bool patchMode = true)
        {
            var response = await _genericRepository.Update(_uow, dto, sid, patchMode);
            return response;
        }

        public async Task<FormatedResponse> UpdateRange(GenericUnitOfWork _uow, List<HuEvaluateDTO> dtos, string sid, bool patchMode = true)
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


        // dev - congnc
        // lấy bản ghi theo EmployeeId
        public async Task<FormatedResponse> GetByEmployeeId(long employeeId)
        {
            try
            {
                // lấy bảng đánh giá HU_EVALUATE
                var HuEvaluates = _dbContext.HuEvaluates.AsNoTracking();


                // lấy bảng tham số hệ thống SYS_OTHER_LIST
                var SysOtherLists = _dbContext.SysOtherLists.AsNoTracking();


                // lấy bảng phòng ban, tổ chức HU_ORGANIZATION
                var HuOrganizations = _dbContext.HuOrganizations.AsNoTracking();


                // lấy bảng vị trí, chức danh HU_POSITION
                var HuPositions = _dbContext.HuPositions.AsNoTracking();


                // lấy bảng nhân viên
                var HuEmployees = _dbContext.HuEmployees.Where(x => x.ID == employeeId).AsNoTracking();


                // tạo biến truy vấn bản ghi
                var query = await (from item in HuEvaluates
                                   from tham_chieu_1 in SysOtherLists.Where(x => x.ID == item.EVALUATE_TYPE).DefaultIfEmpty()

                                   from query_by_employee in HuEmployees.DefaultIfEmpty()
                                   from tham_chieu_2 in HuOrganizations.Where(x => x.ID == query_by_employee.ORG_ID).DefaultIfEmpty()
                                   
                                   from tham_chieu_3 in HuPositions.Where(x => x.ID == query_by_employee.POSITION_ID).DefaultIfEmpty()

                                       // lấy bảng đánh giá xếp loại
                                   from tham_chieu_4 in _dbContext.HuClassifications.Where(x => x.ID == item.CLASSIFICATION_ID).AsNoTracking()
                                   from tham_chieu_5 in _dbContext.SysOtherLists.Where(x => x.ID == tham_chieu_4.CLASSIFICATION_LEVEL).AsNoTracking()

                                   where item.EMPLOYEE_ID == employeeId

                                   // sắp xếp giảm dần
                                   // thì phải dùng "descending"
                                   orderby item.YEAR descending
                                   
                                   select new
                                   {
                                       // id của chính cái bản ghi đấy
                                       // trong bảng HU_EVALUATE
                                       Id = item.ID,

                                       EmployeeId = employeeId,

                                       // loại đánh giá
                                       EvaluateName = tham_chieu_1.NAME,

                                       // mã nhân viên
                                       EmployeeCode = query_by_employee.CODE,

                                       // họ tên nhân viên
                                       EmployeeName = item.EMPLOYEE_NAME,

                                       // phòng ban
                                       OrgName = tham_chieu_2.NAME,

                                       // vị trí, chức danh
                                       PositionName = tham_chieu_3.NAME,

                                       // năm đánh giá
                                       Year = item.YEAR,

                                       // xếp loại đánh giá
                                       ClassificationName = tham_chieu_5.NAME,

                                       // điểm đánh giá
                                       PointEvaluate = item.POINT

                                   }).ToListAsync();


                /*
                    1 nhân viên có rất nhiều kết quả đánh giá
                
                    ví dụ:
                    năm 2020: có 1 bản ghi đánh giá
                    năm 2021: có 1 bản ghi đánh giá
                    năm 2022: có 1 bản ghi đánh giá
                    năm 2023: có 1 bản ghi đánh giá

                    kiểu như vậy đó
                */


                return new FormatedResponse() { InnerBody = query };
            }
            catch (Exception ex)
            {
                return new FormatedResponse() { MessageCode = CommonMessageCode.GET_METHOD_FAILED, ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400 };
            }
        }

        public virtual async Task<FormatedResponse> ToggleActiveIds(GenericUnitOfWork _uow, List<long> ids, bool valueToBind, string sid)
        {
            await Task.Run(() => null);
            throw new NotImplementedException();
        }

    }
}

