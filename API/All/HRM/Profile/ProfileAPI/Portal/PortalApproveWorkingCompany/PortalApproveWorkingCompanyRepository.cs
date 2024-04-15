using CORE.GenericUOW;
using CORE.DTO;
using API.DTO;
using CORE.Enum;
using CORE.StaticConstant;
using API.All.DbContexts;
using CORE.AutoMapper;
using DocumentFormat.OpenXml.Office2010.Excel;
using API.DTO.PortalDTO;
using API.Entities.PORTAL;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;
using DocumentFormat.OpenXml.Math;
using Microsoft.Extensions.Options;

namespace API.All.HRM.Profile.ProfileAPI.Portal.PortalApproveHuWorking
{
    public class PortalApproveWorkingCompanyRepository : IPortalApproveWorkingCompanyRepository
    {
        private readonly GenericUnitOfWork _uow;
        private readonly FullDbContext _dbContext;
        private IGenericRepository<PORTAL_REQUEST_CHANGE, PortalRequestChangeDTO> _genericRepository;
        private readonly GenericReducer<PORTAL_REQUEST_CHANGE, PortalRequestChangeDTO> _genericReducer;
        private readonly IWebHostEnvironment _env;
        private readonly AppSettings _appSettings;
        public PortalApproveWorkingCompanyRepository(FullDbContext context, GenericUnitOfWork uow, IWebHostEnvironment env, IOptions<AppSettings> options)
        {
            _env = env;
            _appSettings = options.Value;
            _dbContext = context;
            _uow = uow;
            _genericRepository = _uow.GenericRepository<PORTAL_REQUEST_CHANGE, PortalRequestChangeDTO>();
            _genericReducer = new();
        }

        public async Task<GenericPhaseTwoListResponse<PortalRequestChangeDTO>> SinglePhaseQueryList(GenericQueryListDTO<PortalRequestChangeDTO> request)
        {
            var joined = from p in _dbContext.PortalRequestChanges
                         
                         // JOIN OTHER ENTITIES BASED ON THE BUSINESS
                         select new PortalRequestChangeDTO
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
                var list = new List<PORTAL_REQUEST_CHANGE>
                    {
                        (PORTAL_REQUEST_CHANGE)response
                    };
                var joined = (from l in list
                                  // JOIN OTHER ENTITIES BASED ON THE BUSINESS
                              select new PortalRequestChangeDTO
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

        public async Task<FormatedResponse> Create(GenericUnitOfWork _uow, PortalRequestChangeDTO dto, string sid)
        {
            var response = await _genericRepository.Create(_uow, dto, sid);
            return response;
        }

        public async Task<FormatedResponse> CreateRange(GenericUnitOfWork _uow, List<PortalRequestChangeDTO> dtos, string sid)
        {
            var add = new List<PortalRequestChangeDTO>();
            add.AddRange(dtos);
            var response = await _genericRepository.CreateRange(_uow, add, sid);
            return response;
        }

        public async Task<FormatedResponse> Update(GenericUnitOfWork _uow, PortalRequestChangeDTO dto, string sid, bool patchMode = true)
        {
            var response = await _genericRepository.Update(_uow, dto, sid, patchMode);
            return response;
        }

        public async Task<FormatedResponse> UpdateRange(GenericUnitOfWork _uow, List<PortalRequestChangeDTO> dtos, string sid, bool patchMode = true)
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
            throw new NotImplementedException();
        }


        // lấy tất cả bản ghi trong PORTAL_REQUEST_CHANGE
        public async Task<GenericPhaseTwoListResponse<PortalRequestChangeDTO>> GetAllRecord(GenericQueryListDTO<PortalRequestChangeDTO> request)
        {
            var get_id_wait_approve = _dbContext.SysOtherLists.Where(x => x.CODE!.ToUpper() == "CD").Select(x => x.ID).First();

            List<string> listCode = new List<string>()
            {
                "00043",
                "00044",
                "00045",
                "00046",
                "00047",
                "00048",
                "00049",
                "00050"
            };

            string location = Path.Combine(_env.ContentRootPath, _appSettings.StaticFolders.Root, _appSettings.StaticFolders.Attachments);

            var list_data =  (from item in _dbContext.PortalRequestChanges.Where(x => listCode.Contains(x.SYS_OTHER_CODE) && x.IS_APPROVE == get_id_wait_approve).ToList()

                             // kết hợp dữ liệu
                             from reference_1 in _dbContext.HuEmployees.Where(x => x.ID == item.EMPLOYEE_ID).ToList()
                             from reference_2 in _dbContext.HuEmployeeCvs.Where(x => x.ID == reference_1.PROFILE_ID).ToList()
                             from reference_5 in _dbContext.SysOtherLists.Where(x => x.ID == item.IS_APPROVE).ToList()
                             from reference_6 in _dbContext.HuOrganizations.Where(x => x.ID == reference_1.ORG_ID).ToList()
                             from reference_7 in _dbContext.HuPositions.Where(x => x.ID == reference_1.POSITION_ID).ToList()
                             from reference_10 in _dbContext.SysOtherLists.Where(x => x.CODE == item.SYS_OTHER_CODE).ToList()
                             from j in _dbContext.HuJobs.AsNoTracking().Where(x => x.ID == reference_7.JOB_ID).DefaultIfEmpty()
                             select new PortalRequestChangeDTO
                             {
                                // Id của bảng PORTAL_REQUEST_CHANGE
                                Id = item.ID,

                                // trường 1: trạng thái
                                StatusName = reference_5.NAME,

                                // trường 2: lý do
                                ReasonChange = item.REASON_CHANGE,

                                // trường 3: Mã nhân viên
                                EmployeeCode = reference_1.CODE,

                                // trường 4: họ và tên
                                EmployeeName = reference_2.FULL_NAME,

                                // trường 7: Phòng/Ban
                                OrgName = reference_6.NAME,

                                // trường 8: Vị trí chức danh
                                PositionName = reference_7.NAME,

                                // trường 15: ORG_ID
                                OrgId = reference_1.ORG_ID,

                                // trường 16: tên chức năng
                                FunctionName = reference_10.NAME,

                                FileName = item.FILE_NAME ?? ""
                             })
                             .ToList();

            var response = list_data.AsQueryable();


            // cách này hay
            // StatusName = ee.IS_APPROVED_EDUCATION == true ? "Phê duyệt" : (ee.IS_APPROVED_EDUCATION == false ? "Từ chối" : "Chờ phê duyệt")

            var singlePhaseResult = await _genericReducer.SinglePhaseReduce(response, request);
            return singlePhaseResult;
        }


        public async Task<FormatedResponse> ApproveHuWorking(GenericToggleIsActiveDTO model)
        {
            try
            {
                // truy vấn các bản ghi theo danh sách id
                // được truyền vào
                var list_record = await _dbContext.PortalRequestChanges.Where(x => model.Ids.Contains(x.ID)).ToListAsync();


                // lấy ID trong bảng SYS_OTHER_LIST
                // có tên là "đã phê duyệt"
                // mã "DD" là "Đã phê duyệt"
                var id_approve = _dbContext.SysOtherLists
                                    .Where(x => x.CODE == "DD")
                                    .Select(x => x.ID)
                                    .First();


                // thiết lập IS_APPROVE = 994
                // nếu người dùng bấm phê duyệt bản ghi
                foreach (var item in list_record)
                {
                    item.IS_APPROVE = id_approve;
                }


                // lưu db
                _dbContext.SaveChanges();


                // lấy ra bản ghi mới nhất trong db
                var record_top1 = _dbContext.PortalRequestChanges.OrderByDescending(x => x.ID).Take(1);


                return new FormatedResponse()
                {
                    StatusCode = EnumStatusCode.StatusCode200,
                    MessageCode = CommonMessageCode.APPROVED_SUCCESS,
                    InnerBody = record_top1
                };
            }
            catch (Exception ex)
            {

                return new FormatedResponse() {
                    StatusCode = EnumStatusCode.StatusCode500,
                    ErrorType = EnumErrorType.UNCATCHABLE,
                    MessageCode = ex.Message
                };
            }
        }


        public async Task<FormatedResponse> UnapproveHuWorking(GenericUnapprovePortalDTO model)
        {
            try
            {
                // truy vấn các bản ghi theo danh sách id
                // được truyền vào
                var list_record = await _dbContext.PortalRequestChanges.Where(x => model.Ids.Contains(x.ID)).ToListAsync();


                // lấy ID trong bảng SYS_OTHER_LIST
                // có tên là "Không phê duyệt"
                // mã "TCPD" là "Không phê duyệt"
                var id_unapprove = _dbContext.SysOtherLists
                                    .Where(x => x.CODE == "TCPD")
                                    .Select(x => x.ID)
                                    .First();


                // thiết lập IS_APPROVE = 995
                // nếu người dùng bấm từ chối phê duyệt bản ghi
                foreach (var item in list_record)
                {
                    // thiết lập giá trị từ chối phê duyệt
                    item.IS_APPROVE = id_unapprove;

                    // thiết lập lý do từ chối phê duyệt
                    item.REASON_DISCARD = model.Reason;
                }


                // lưu db
                _dbContext.SaveChanges();


                // lấy ra bản ghi mới nhất trong db
                var record_top1 = _dbContext.PortalRequestChanges.OrderByDescending(x => x.ID).Take(1);


                return new FormatedResponse()
                {
                    StatusCode = EnumStatusCode.StatusCode200,
                    MessageCode = CommonMessageCode.DISCARD_APPROVED_SUCCESSFULLY,
                    InnerBody = record_top1
                };
            }
            catch (Exception ex)
            {

                return new FormatedResponse()
                {
                    StatusCode = EnumStatusCode.StatusCode500,
                    ErrorType = EnumErrorType.UNCATCHABLE,
                    MessageCode = ex.Message
                };
            }
        }
    }
}

