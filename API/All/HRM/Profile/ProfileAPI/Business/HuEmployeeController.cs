using Microsoft.AspNetCore.Mvc;
using ProfileDAL.Repositories;
using ProfileDAL.ViewModels;
using Common.Extensions;
using API.Entities;
using CORE.GenericUOW;
using CORE.DTO;
using CORE.Enum;
using CORE.StaticConstant;
using API;
using Microsoft.Extensions.Options;
using API.DTO;
using API.All.DbContexts;

namespace ProfileAPI.List
{
    [ApiExplorerSettings(GroupName = "002-PROFILE-EMPLOYEE")]
    [ApiController]
    [HiStaffAuthorize]
    [Route("api/[controller]/[action]")]
    public class HuEmployeeController : BaseController1
    {
        private readonly GenericUnitOfWork _uow;
        private IGenericRepository<HU_EMPLOYEE, EmployeeDTO> _genericRepository;
        private readonly AppSettings _appSettings;
        private readonly FullDbContext _fullDbContext;

        private readonly GenericReducer<HU_EMPLOYEE, HuEmployeeDTO> _genericReducer;


        public HuEmployeeController(IProfileUnitOfWork unitOfWork, IOptions<AppSettings> options, FullDbContext fullDbContext) : base(unitOfWork)
        {
            _uow = new GenericUnitOfWork(unitOfWork.DataContext);
            _genericRepository = _uow.GenericRepository<HU_EMPLOYEE, EmployeeDTO>();
            _appSettings = options.Value;
            _fullDbContext = fullDbContext;
            _genericReducer = new();
        }

        [HttpPost]
        public async Task<IActionResult> QueryList(GenericQueryListDTO<HuEmployeeDTO> request)
        {
            try
            {
                /* HERE WE CAN EITHER CHOOSE ONE OF THE 2 APPROACHS: TwoPhaseQueryList or SinglePhaseQueryList */
                /* WE WILL KNOW WHAT APPROACH IS FASTER IN THE FUTURE */
                /* UPDATE 2023.10.04: TwoPhaseQueryList was depricated and is not being improved since 2023 Sep 
                 * Please use only SinglePhaseQueryList
                */

                //var response = await _unitOfWork.EmployeeRepository.TwoPhaseQueryList(request);
                var response = await _unitOfWork.EmployeeRepository.SinglePhaseQueryList(request);

                if (response.ErrorType != EnumErrorType.NONE)
                {
                    return Ok(new FormatedResponse()
                    {
                        ErrorType = response.ErrorType,
                        MessageCode = response.MessageCode ?? CommonMessageCode.NOT_ALL_CASES_CATCHED,
                        StatusCode = EnumStatusCode.StatusCode400,
                    });
                } else
                {
                    return Ok(new FormatedResponse()
                    {
                        InnerBody = response
                    });
                }
            }
            catch (Exception ex)
            {
                return Ok(new FormatedResponse() { ErrorType = EnumErrorType.UNCATCHABLE, StatusCode = EnumStatusCode.StatusCode500, MessageCode = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> QueryListEmp(GenericQueryListDTO<HuEmployeeDTO> request)
        {
            try
            {
                var response = await _unitOfWork.EmployeeRepository.QueryListEmp(request);

                if (response.ErrorType != EnumErrorType.NONE)
                {
                    return Ok(new FormatedResponse()
                    {
                        ErrorType = response.ErrorType,
                        MessageCode = response.MessageCode ?? CommonMessageCode.NOT_ALL_CASES_CATCHED,
                        StatusCode = EnumStatusCode.StatusCode400,
                    });
                }
                else
                {
                    return Ok(new FormatedResponse()
                    {
                        InnerBody = response
                    });
                }
            }
            catch (Exception ex)
            {
                return Ok(new FormatedResponse() { ErrorType = EnumErrorType.UNCATCHABLE, StatusCode = EnumStatusCode.StatusCode500, MessageCode = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> QueryListForOrgOverview(GenericQueryListDTO<HuEmployeeDTO> request)
        {
            var employees = from p in _fullDbContext.HuPositions
                            join j in _fullDbContext.HuJobs on p.JOB_ID equals j.ID into jp
                            from jpResult in jp.DefaultIfEmpty()  

                            join o in _fullDbContext.HuOrganizations on p.ORG_ID equals o.ID into po
                            from poResult in po.DefaultIfEmpty()

                            join e in _fullDbContext.HuEmployees on p.ID equals e.POSITION_ID into pe
                            from peResult in pe.DefaultIfEmpty()

                            join v in _fullDbContext.HuEmployeeCvs on peResult.PROFILE_ID equals v.ID into pev
                            from pevResult in pev.DefaultIfEmpty()

                            where request.InOperators != null && request.InOperators[0].Values.Contains(p.ORG_ID ?? -1)
                            && request.Filter != null && request.Filter.JobId == p.JOB_ID

                            && poResult != null && peResult != null

                            select new HuEmployeeDTO()
                            {
                                Id = peResult.ID,
                                OrgId = p.ORG_ID,
                                JobId = p.JOB_ID,
                                Code = peResult.CODE,
                                Fullname = pevResult.FULL_NAME,
                                OrgName = poResult.NAME,
                                PositionName = jpResult.NAME_VN
                            };

            var response = await _genericReducer.SinglePhaseReduce(employees, request);

            return Ok(new FormatedResponse() { InnerBody = response });
        }

        [HttpGet]
        public async Task<ActionResult> GetAll(EmployeeDTO param)
        {
            if (!ModelState.IsValid)
            {
                return ResponseValidation();
            }
            var r = await _unitOfWork.EmployeeRepository.GetAll(param);
            return Ok(r);

        }
        [HttpGet]
        public async Task<ActionResult> GetPopup(EmployeePopup param)
        {
            if (!ModelState.IsValid)
            {
                return ResponseValidation();
            }
            var r = await _unitOfWork.EmployeeRepository.GetPopup(param);
            return Ok(r);

        }
        [HttpGet]
        public async Task<ActionResult> GetById(long Id)
        {
            if (!ModelState.IsValid)
            {
                return ResponseValidation();
            }
            var r = await _unitOfWork.EmployeeRepository.GetById(Id);
            return Ok(new FormatedResponse() { InnerBody = r.Data});

        }
        [HttpGet]
        public async Task<ActionResult> GetList()
        {
            var r = await _unitOfWork.EmployeeRepository.GetList();
            return ResponseResult(r);
        }
        [HttpGet]
        public async Task<ActionResult> GetListByOrg(int OrgId)
        {
            var r = await _unitOfWork.EmployeeRepository.GetListByOrg(OrgId);
            return ResponseResult(r);
        }
        [HttpGet]
        public async Task<ActionResult> ListSituation(long empId)
        {
            var r = await _unitOfWork.EmployeeRepository.ListSituation(empId);
            return ResponseResult(r);
        }
        [HttpPost]
        public async Task<ActionResult> AddSituation([FromBody] SituationDTO param)
        {
            if (!ModelState.IsValid)
            {
                return ResponseValidation();
            }

            var r = await _unitOfWork.EmployeeRepository.CreateSituation(param);
            return ResponseResult(r);

        }
        [HttpPost]
        public async Task<ActionResult> Add([FromBody] EmployeeInput param)
        {
            if (!ModelState.IsValid)
            {
                return ResponseValidation();
            }

            var r = await _unitOfWork.EmployeeRepository.CreateAsync(param);
            return ResponseResult(r);

        }
       

        [HttpPost]
        public async Task<ActionResult> Update([FromBody] EmployeeInput param)
        {

            if (!ModelState.IsValid)
            {
                return ResponseValidation();
            }


            var r = await _unitOfWork.EmployeeRepository.UpdateAsync(param);
            return ResponseResult(r);
        }
        [HttpPost]
        public async Task<ActionResult> RemoveRelation([FromBody] int id)
        {

            if (!ModelState.IsValid)
            {
                return ResponseValidation();
            }
            var r = await _unitOfWork.EmployeeRepository.RemoveRelation(id);
            return ResponseResult(r);
        }
        [HttpGet]
        public async Task<ActionResult> PortalGetCurriculum()
        {
            var r = await _unitOfWork.EmployeeRepository.PortalGetBy(1);
            return ResponseResult(r);
        }

        [HttpGet]
        public async Task<ActionResult> PortalGetadditional()
        {
            var r = await _unitOfWork.EmployeeRepository.PortalGetBy(2);
            return ResponseResult(r);
        }

        [HttpGet]
        public async Task<ActionResult> PortalGetCultural()
        {
            var r = await _unitOfWork.EmployeeRepository.PortalGetBy(4);
            return ResponseResult(r);
        }

        [HttpGet]
        public async Task<ActionResult> PortalGetBank()
        {
            var r = await _unitOfWork.EmployeeRepository.PortalGetBy(3);
            return ResponseResult(r);
        }

        [HttpGet]
        public async Task<ActionResult> PortalGetFamily()
        {
            var r = await _unitOfWork.EmployeeRepository.PortalGetFamily();
            return ResponseResult(r);
        }
        [HttpGet]
        public async Task<ActionResult> GetInforLeaveJob(long Id)
        {
            var r = await _unitOfWork.EmployeeRepository.GetInforLeaveJob(Id);
            return ResponseResult(r);
        }
        [HttpGet]
        public async Task<ActionResult> GetInforContract(long Id)
        {
            var r = await _unitOfWork.EmployeeRepository.GetInforContract(Id);
            return ResponseResult(r);
        }
        [HttpPost]
        public async Task<ActionResult> GetEmpAlowance([FromBody] List<long> Ids)
        {
            var r = await _unitOfWork.EmployeeRepository.GetEmpAlowance(Ids);
            return ResponseResult(r);
        }
        [HttpGet]
        public async Task<ActionResult> GetListEmpToImport()
        {
            var r = await _unitOfWork.EmployeeRepository.GetListEmpToImport();
            return ResponseResult(r);
        }
        [HttpPost]
        public async Task<ActionResult> TemplateImport()
        {
            try
            {
                var stream = await _unitOfWork.EmployeeRepository.TemplateImport();
                var fileName = "templateProfile.xlsx";
                if (stream.StatusCode == "200")
                {
                    return new FileStreamResult(stream.memoryStream, "application/octet-stream") { FileDownloadName = fileName };
                }
                return ResponseResult(stream);
            }
            catch (Exception ex)
            {
                return ResponseResult(ex.Message);
            }
        }
        [HttpPost]
        public async Task<ActionResult> ImportProfile([FromBody] EmpImportParam param)
        {

            try
            {
                if (param== null)
                {
                    return ResponseValidation();
                }

                var r = await _unitOfWork.EmployeeRepository.ImportProfile(param);
                if (r.memoryStream != null)
                {
                    var fileName = "TemplateProfileError.xlsx";
                    return new FileStreamResult(r.memoryStream, "application/octet-stream") { FileDownloadName = fileName };
                }
                else
                {
                    return ResponseResult(int.Parse(r.StatusCode));
                }
            }
            catch (Exception ex)
            {
                return ResponseResult(ex.Message);
            }
        }
        [HttpPost]
        public async Task<ActionResult> Delete([FromBody] List<long> param)
        {
            var r = await _unitOfWork.EmployeeRepository.Delete(param);
            return ResponseResult(r);
        }

        [HttpPost]
        public async Task<ActionResult> DeleteIds(IdsRequest model)
        {
            var r = await _unitOfWork.EmployeeRepository.Delete(model.Ids);
            return Ok(new FormatedResponse() { InnerBody = r.StatusCode, MessageCode = r.Error });
        }

        [HttpGet]
        public async Task<ActionResult> GetListPaper(int EmpId)
        {
            if (!ModelState.IsValid)
            {
                return ResponseValidation();
            }
            var r = await _unitOfWork.EmployeeRepository.GetListPaper(EmpId);
            return Ok(r);
        }
        [HttpPost]
        public async Task<ActionResult> CreatePaper([FromBody] PaperInput param)
        {
            var r = await _unitOfWork.EmployeeRepository.CreatePaperAsync(param);
            return ResponseResult(r);
        }

        [HttpGet]
        public async Task<ActionResult> PortalContactBook(string name)
        {
            if (!ModelState.IsValid)
            {
                return ResponseValidation();
            }
            var r = await _unitOfWork.EmployeeRepository.PortalContactBook(name);
            return Ok(r);
        }

        [HttpGet]
        public async Task<ActionResult> PortalGetContact(int EmpId)
        {
            if (!ModelState.IsValid)
            {
                return ResponseValidation();
            }
            var r = await _unitOfWork.EmployeeRepository.PortalGetContact(EmpId);
            return Ok(r);
        }

        [HttpGet]
        public async Task<ActionResult> ScanQRCode()
        {
            var r = await _unitOfWork.EmployeeRepository.ScanQRCode();
            return Ok(r);
        }

        [HttpGet]
        public async Task<ActionResult> GetAllProfileEdit(EmployeeEditDTO param)
        {
            if (!ModelState.IsValid)
            {
                return ResponseValidation();
            }
            var r = await _unitOfWork.EmployeeRepository.GetAllProfileEdit(param);
            return Ok(r);
        }

        [HttpGet]
        public async Task<ActionResult> GetAllFamilyAdd(FamilyEditDTO param)
        {
            if (!ModelState.IsValid)
            {
                return ResponseValidation();
            }
            var r = await _unitOfWork.EmployeeRepository.GetAllFamilyAdd(param);
            return Ok(r);

        }

        [HttpGet]
        public async Task<ActionResult> EditInfomationBy(int id)
        {
            if (!ModelState.IsValid)
            {
                return ResponseValidation();
            }
            var r = await _unitOfWork.EmployeeRepository.EditInfomationBy(id);
            return Ok(r);

        }

        [HttpPost]
        public async Task<ActionResult> ApproveProfileEdit([FromBody] int id)
        {
            var r = await _unitOfWork.EmployeeRepository.ApproveProfileEdit(id, OtherConfig.STATUS_APPROVE);
            return ResponseResult(r);
        }

        [HttpPost]
        public async Task<ActionResult> RejectProfileEdit([FromBody] int id)
        {
            var r = await _unitOfWork.EmployeeRepository.ApproveProfileEdit(id, OtherConfig.STATUS_DECLINE);
            return ResponseResult(r);
        }

        [HttpGet]
        public async Task<ActionResult> GetByIdOnPortal(long? id)
        {
            var response = await _unitOfWork.EmployeeRepository.GetByIdOnPortal(id);
            return Ok(response);
        }

        #region Danh ba nhan su
        [HttpPost]
        public async Task<IActionResult> QueryListPersonnelDirectory(GenericQueryListDTO<HuEmployeeDTO> request)
        {
            try
            {
                var response = await _unitOfWork.EmployeeRepository.QueryListPersonnelDirectory(request);

                if (response.ErrorType != EnumErrorType.NONE)
                {
                    return Ok(new FormatedResponse()
                    {
                        ErrorType = response.ErrorType,
                        MessageCode = response.MessageCode ?? CommonMessageCode.NOT_ALL_CASES_CATCHED,
                        StatusCode = EnumStatusCode.StatusCode400,
                    });
                }
                else
                {
                    return Ok(new FormatedResponse()
                    {
                        InnerBody = response
                    });
                }
            }
            catch (Exception ex)
            {
                return Ok(new FormatedResponse() { ErrorType = EnumErrorType.UNCATCHABLE, StatusCode = EnumStatusCode.StatusCode500, MessageCode = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetPersonnelDirectoryById(long Id)
        {
            var response = await _unitOfWork.EmployeeRepository.GetPersonnelDirectoryById(Id);
            return Ok(response);
        }
        #endregion
    }
}
