using Azure;
using CORE.DTO;
using CORE.Enum;
using CORE.GenericUOW;
using CORE.Services.File;
using CORE.StaticConstant;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using ProfileDAL.ViewModels;
using API.All.DbContexts;
using API.Controllers.HuEmployeeCv;
using API.DTO;
using API.Main;
using API.Controllers.HuEmployeeCvEdit;
using CORE.AutoMapper;
using DocumentFormat.OpenXml.Vml.Office;
using System.Reflection;
using Microsoft.AspNetCore.Http.HttpResults;
using CORE.JsonHelper;

namespace API.All.HRM.Profile.ProfileAPI.Portal.PortalEducation
{
    [ApiExplorerSettings(GroupName = "002-PORTAL-PORTAL_STAFF_PROFILE")]
    [ApiController]
    [HiStaffAuthorize]
    [Route("api/[controller]/[action]")]

    public class PortalStaffProfileController : ControllerBase
    {
        private readonly GenericUnitOfWork _uow;
        private readonly IPortalStaffProfileRepository _PortalStaffProfileRepository;
        private readonly IHuEmployeeCvEditRepository _HuEmployeeCvEditRepository;
        private readonly AppSettings _appSettings;
        private readonly FullDbContext _dbContext;


        public PortalStaffProfileController(
            FullDbContext dbContext,
            IOptions<AppSettings> options,
            IWebHostEnvironment env,
            IFileService fileService)
        {
            _uow = new GenericUnitOfWork(dbContext);
            _PortalStaffProfileRepository = new PortalStaffProfileRepository(dbContext, _uow, env, options, fileService);
            _HuEmployeeCvEditRepository = new HuEmployeeCvEditRepository(dbContext, _uow);
            _appSettings = options.Value;
            _dbContext = dbContext;
        }




        //EDUCATION
        // nếu bạn nhìn thấy tham số truyền vào có chữ "time"
        // tức là màn hình ở ngoài Front End
        // nó cần cập nhật dữ liệu hiển thị
        // thì nó cần call lại API
        // nhưng tham số truyền cho API phải có sự biến động, thay đổi
        // thì nó mới chịu gọi lại API
        // đây là xử lý trong call API của Angular 17
        [HttpGet]
        public async Task<IActionResult> GetEducationByPortal(long employeeId, long? time)
        {
            var response = await _PortalStaffProfileRepository.GetEducationByPortal(employeeId, time);
            return Ok(response);
        }




        [HttpGet]
        public async Task<IActionResult> GetEducationByPortalCorrect(long id)
        {
            var response = await _PortalStaffProfileRepository.GetEducationByPortalCorrect(id);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetHuEmployeeCvEditAdditionalInfoSaveEdit(long id)
        {
            var response = await _PortalStaffProfileRepository.GetHuEmployeeCvEditAdditionalInfoSaveEdit(id);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> InsertEducationHuEmployeeCvEdt(HuEmployeeCvEditDTO dto)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            bool pathMode = true;
            var getOtherList = (from t in _uow.Context.Set<SYS_OTHER_LIST_TYPE>().Where(x => x.CODE == "STATUS")
                                from o in _uow.Context.Set<SYS_OTHER_LIST>().Where(x => x.CODE == "CD")
                                select new { Id = o.ID }).FirstOrDefault();

            var cv = _dbContext.HuEmployeeCvs.Where(x => x.ID == dto.Id).FirstOrDefault();
            List<string> listModelChange = new List<string>();
            if (cv != null)
            {
                var entityType = typeof(HU_EMPLOYEE_CV);
                var dtoType = typeof(HuEmployeeCvDTO);
                var entityPropList = entityType.GetProperties().ToList();
                var dtoPropList = dtoType.GetProperties().ToList();

                var query = Activator.CreateInstance(dtoType);


                entityPropList.ForEach(prop =>
                {
                    var value = prop.GetValue(cv);
                    var dtoProp = dtoPropList.SingleOrDefault(x => x.Name == prop.Name.SnakeToCamelCase().CamelToPascalCase());
                    dtoProp?.SetValue(query, value);

                });


                if (query != null)
                {
                    Type type = query.GetType();
                    Type type2 = dto.GetType();
                    IList<PropertyInfo> props = new List<PropertyInfo>(type.GetProperties());
                    IList<PropertyInfo> prop2s = new List<PropertyInfo>(type2.GetProperties());
                    foreach (PropertyInfo prop in props)
                    {
                        foreach (PropertyInfo prop2 in prop2s)
                        {

                            if (prop.Name != "Id" && prop.Name != null && prop.Name == prop2.Name && prop.GetValue(query) != null && prop2.GetValue(dto) != null && prop.GetValue(query)!.ToString() != prop2.GetValue(dto)!.ToString())
                            {
                                listModelChange.Add(Char.ToLowerInvariant(prop.Name[0]) + prop.Name.Substring(1));
                            }

                        }
                    }
                    dto.ModelChange = string.Join(";", listModelChange);
                }
            }
            if (dto.Id != null && dto.IsSaveEducation == true)
            {
                dto.IsSendPortal = true;
                dto.IsSaveEducation = false;
                dto.IsSendPortalEducation = true;
                dto.StatusApprovedEducationId = getOtherList.Id;
                dto.IsApprovedEducation = false;

                // lúc SAVE thì đã có HuEmployeeCvId rồi
                //dto.HuEmployeeCvId = dto.Id;

                var updateResponse = await _HuEmployeeCvEditRepository.Update(_uow, dto, sid, pathMode);
                return Ok(updateResponse);
            }
            dto.IsSendPortal = true;
            dto.IsSaveEducation = false;
            dto.IsSendPortalEducation = true;
            dto.IsApprovedEducation = false;
            dto.HuEmployeeCvId = dto.Id;
            dto.StatusApprovedEducationId = getOtherList.Id;
            dto.Id = null;
            var response = await _HuEmployeeCvEditRepository.Create(_uow, dto, sid);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> SaveEducationHuEmployeeCvEdit(HuEmployeeCvEditDTO dto)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();

            dto.StatusApprovedEducationId = -1;

            bool pathMode = true;
            var getOtherList = (from t in _uow.Context.Set<SYS_OTHER_LIST_TYPE>().Where(x => x.CODE == "STATUS")
                                from o in _uow.Context.Set<SYS_OTHER_LIST>().Where(x => x.CODE == "CD")
                                select new { Id = o.ID }).FirstOrDefault();
            if (dto.Id != null && dto.IsSaveEducation == true)
            {
                var updateResponse = await _HuEmployeeCvEditRepository.Update(_uow, dto, sid, pathMode);
                return Ok(updateResponse);
            }
            if (dto.Id != null && dto.IsSaveEducation == false)
            {
                dto.IsSaveEducation = true;
                var updateResponse = await _HuEmployeeCvEditRepository.Update(_uow, dto, sid, pathMode);
                return Ok(updateResponse);
            }


            // bị tuột huEmployeeCvId nên phải thêm code
            // congnc thêm dòng code này
            dto.HuEmployeeCvId = dto.Id;


            dto.IsSaveEducation = true;
            dto.Id = null;

            var response = await _HuEmployeeCvEditRepository.Create(_uow, dto, sid);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetProfileInfoByPortal(long employeeId)
        {
            var response = await _PortalStaffProfileRepository.GetProfileInfoByPortal(employeeId);
            return Ok(response);
        }


        [HttpGet]
        public async Task<IActionResult> GetProfileInfoByPortal2(long employeeId)
        {
            var response = await _PortalStaffProfileRepository.GetProfileInfoByPortal2(employeeId);
            return Ok(response);
        }


        [HttpGet]
        public async Task<IActionResult> GetCurriculumByPortal(long employeeId, long? time)
        {
            var response = await _PortalStaffProfileRepository.GetCurriculumByPortal(employeeId);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateCurriculumByPortal(HuEmployeeCvEditDTO request)
        {
            var response = await _PortalStaffProfileRepository.UpdateCurriculumByPortal(request);
            return Ok(response);
        }


        //CV
        [HttpPost]
        public async Task<IActionResult> InsertCvHuEmployeeCvEdit(HuEmployeeCvEditDTO dto)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();

            bool pathMode = true;
            var getOtherList = (from t in _uow.Context.Set<SYS_OTHER_LIST_TYPE>().Where(x => x.CODE == "STATUS")
                                from o in _uow.Context.Set<SYS_OTHER_LIST>().Where(x => x.CODE == "CD")
                                select new { Id = o.ID }).FirstOrDefault();

            var cv = _dbContext.HuEmployeeCvs.Where(x => x.ID == dto.Id).FirstOrDefault();
            List<string> listModelChange = new List<string>();
            if (cv != null)
            {
                var entityType = typeof(HU_EMPLOYEE_CV);
                var dtoType = typeof(HuEmployeeCvDTO);
                var entityPropList = entityType.GetProperties().ToList();
                var dtoPropList = dtoType.GetProperties().ToList();

                var query = Activator.CreateInstance(dtoType);


                entityPropList.ForEach(prop =>
                {
                    var value = prop.GetValue(cv);
                    var dtoProp = dtoPropList.SingleOrDefault(x => x.Name == prop.Name.SnakeToCamelCase().CamelToPascalCase());
                    dtoProp?.SetValue(query, value);

                });


                if (query != null)
                {
                    Type type = query.GetType();
                    Type type2 = dto.GetType();
                    IList<PropertyInfo> props = new List<PropertyInfo>(type.GetProperties());
                    IList<PropertyInfo> prop2s = new List<PropertyInfo>(type2.GetProperties());
                    foreach (PropertyInfo prop in props)
                    {
                        foreach (PropertyInfo prop2 in prop2s)
                        {
                            //if(prop2.Name == "IdentityAddress")
                            //{
                            //    listModelChange.Add("IdentityAddress");
                            //}

                            if (prop.Name != "Id" && prop.Name != null && prop.Name == prop2.Name && prop.GetValue(query) != null && prop2.GetValue(dto) != null && prop.GetValue(query)!.ToString() != prop2.GetValue(dto)!.ToString())
                            {
                                listModelChange.Add(Char.ToLowerInvariant(prop.Name[0]) + prop.Name.Substring(1));
                            }

                        }
                    }
                    if(dto.IdentityAddress != null && cv.ID_PLACE != dto.IdentityAddress)
                    {
                        listModelChange.Add("IdentityAddress");
                    }
                    dto.ModelChange = string.Join(";", listModelChange);
                }
            }
            if (dto.Id != null && dto.IsSaveCv == true)
            {
                dto.IsApprovedCv = false;
                dto.StatusApprovedBankId = getOtherList?.Id;
                dto.FullName = dto.Fullname;
                dto.HuEmployeeCvId = dto.Id;
                dto.IsSaveCv = false;
                dto.IsSendPortalCv = true;
                var upadateResponse = await _HuEmployeeCvEditRepository.Update(_uow, dto, sid, pathMode);
            }
            if (dto.Id != null && dto.IsSaveCv == false)
            {
                dto.StatusApprovedCvId = getOtherList?.Id;
                dto.HuEmployeeCvId = dto.Id;
                dto.IsSaveCv = false;
                dto.IsSendPortalCv = true;
                dto.IsApprovedCv = false;
                var updateRepone2 = await _HuEmployeeCvEditRepository.Update(_uow, dto, sid, pathMode);
                return Ok(updateRepone2);
            }
            dto.StatusApprovedCvId = getOtherList?.Id;
            dto.HuEmployeeCvId = dto.Id;
            dto.IsSaveCv = false;
            dto.IsSendPortalCv = true;
            dto.IsApprovedCv = false;
            dto.Id = null;
            var response = await _HuEmployeeCvEditRepository.Create(_uow, dto, sid);
            return Ok(response);

        }

        [HttpGet]
        public async Task<IActionResult> GetHuEmployeeCvEditCvApproving(long employeeId)
        {
            var response = await _PortalStaffProfileRepository.GetHuEmployeeCvEditCvApproving(employeeId);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetHuEmployeeCvEditById(long id)
        {
            var response = await _PortalStaffProfileRepository.GetHuEmployeeCvEditById(id);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetHuEmployeeCvEditCorrectById(long id)
        {
            var response = await _PortalStaffProfileRepository.GetHuEmployeeCvEditCorrectById(id);
            return Ok(response);
        }
        [HttpPost]
        public async Task<IActionResult> SaveCvHuEmployeeCvEdit(HuEmployeeCvEditDTO dto)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            bool pathMode = true;
            var getOtherList = (from t in _uow.Context.Set<SYS_OTHER_LIST_TYPE>().Where(x => x.CODE == "STATUS")
                                from o in _uow.Context.Set<SYS_OTHER_LIST>().Where(x => x.CODE == "CD")
                                select new { Id = o.ID }).FirstOrDefault();
            if (dto.Id != null && dto.IsSaveCv == true)
            {
                var updateResponse = await _HuEmployeeCvEditRepository.Update(_uow, dto, sid, pathMode);
                return Ok(updateResponse);
            }
            if (dto.Id != null && dto.IsSaveCv == false)
            {
                dto.IsSaveCv = true;

                var updateResponse = await _HuEmployeeCvEditRepository.Update(_uow, dto, sid, pathMode);
                return Ok(updateResponse);
            }
            dto.IsSaveCv = true;
            dto.HuEmployeeCvId = dto.Id;
            dto.Id = null;
            var response = await _HuEmployeeCvEditRepository.Create(_uow, dto, sid);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetHuEmployeeCvSave(long employeeId, long? time)
        {
            var response = await _PortalStaffProfileRepository.GetHuEmployeeCvSave(employeeId);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetHuEmployeeCvEditCvSaveById(long id, long? time)
        {
            var response = await _PortalStaffProfileRepository.GetHuEmployeeCvEditCvSaveById(id);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetHuEmployeeCvUnapprove(long employeeId, long? time)
        {
            var response = await _PortalStaffProfileRepository.GetHuEmployeeCvUnapprove(employeeId);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetHuEmployeeCvUnapproveById(long id, long? time)
        {
            var response = await _PortalStaffProfileRepository.GetHuEmployeeCvUnapproveById(id);
            return Ok(response);
        }
        //BANK_INFO
        [HttpGet]
        public async Task<IActionResult> GetBankInfoByEmployeeId(long employeeId, long? time)
        {
            var response = await _PortalStaffProfileRepository.GetBankInfoByEmployeeId(employeeId);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> InsertBankInfoHuEmployeeCvEdit(HuEmployeeCvEditDTO dto)
        {
            try
            {
                var sid = Request.Sid(_appSettings);
                if (sid == null) return Unauthorized();
                List<string> listModelChange = new List<string>();
                bool pathMode = true;
                var getOtherList = (from t in _uow.Context.Set<SYS_OTHER_LIST_TYPE>().Where(x => x.CODE == "STATUS")
                                    from o in _uow.Context.Set<SYS_OTHER_LIST>().Where(x => x.CODE == "CD")
                                    select new { Id = o.ID }).FirstOrDefault();

                var cv = _dbContext.HuEmployeeCvs.Where(x => x.ID == dto.Id).FirstOrDefault();
                if (cv != null)
                {
                    var entityType = typeof(HU_EMPLOYEE_CV);
                    var dtoType = typeof(HuEmployeeCvDTO);
                    var entityPropList = entityType.GetProperties().ToList();
                    var dtoPropList = dtoType.GetProperties().ToList();

                    var query = Activator.CreateInstance(dtoType);


                    entityPropList.ForEach(prop =>
                    {
                        var value = prop.GetValue(cv);
                        var dtoProp = dtoPropList.SingleOrDefault(x => x.Name == prop.Name.SnakeToCamelCase().CamelToPascalCase());
                        dtoProp?.SetValue(query, value);

                    });


                    if (query != null)
                    {
                        Type type = query.GetType();
                        Type type2 = dto.GetType();
                        IList<PropertyInfo> props = new List<PropertyInfo>(type.GetProperties());
                        IList<PropertyInfo> prop2s = new List<PropertyInfo>(type2.GetProperties());
                        foreach (PropertyInfo prop in props)
                        {
                            foreach (PropertyInfo prop2 in prop2s)
                            {

                                if (prop.Name != "Id" && prop.Name != null && prop.Name == prop2.Name && prop.GetValue(query) != null && prop2.GetValue(dto) != null && prop.GetValue(query)!.ToString() != prop2.GetValue(dto)!.ToString())
                                {
                                    listModelChange.Add(Char.ToLowerInvariant(prop.Name[0]) + prop.Name.Substring(1));
                                }

                            }
                        }
                        dto.ModelChange = string.Join(";", listModelChange);
                    }
                }

                if (dto.Id != null && dto.IsSaveBankInfo == true)
                {
                    dto.IsApprovedBankInfo = false;
                    dto.StatusApprovedBankId = getOtherList?.Id;
                    dto.IsSendPortalBankInfo = true;
                    dto.IsSaveBankInfo = false;
                    dto.HuEmployeeCvId = dto.Id;
                    var updateResponse = await _HuEmployeeCvEditRepository.Update(_uow, dto, sid, pathMode);
                    return Ok(updateResponse);
                }
                if (dto.Id != null && dto.IsSaveBankInfo == false)
                {
                    dto.IsApprovedBankInfo = false;
                    dto.StatusApprovedBankId = getOtherList?.Id;
                    dto.IsSendPortalBankInfo = true;
                    var updateResponse = await _HuEmployeeCvEditRepository.Update(_uow, dto, sid, pathMode);
                    return Ok(updateResponse);
                }
                dto.IsApprovedBankInfo = false;
                dto.IsSendPortalBankInfo = true;
                dto.StatusApprovedBankId = getOtherList?.Id;
                dto.HuEmployeeCvId = dto.Id;
                dto.IsSaveBankInfo = false;
                dto.Id = null;
                var response = await _HuEmployeeCvEditRepository.Create(_uow, dto, sid);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return Ok(new FormatedResponse() { InnerBody = null, StatusCode = EnumStatusCode.StatusCode500 });
            }

        }

        [HttpGet]
        public async Task<IActionResult> GetHuEmployeeCvEditBankInfoApproving(long employeeId, long? time)
        {
            var response = await _PortalStaffProfileRepository.GetHuEmployeeCvEditBankInfoApproving(employeeId);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> SaveBankInfoHuEmployeeCvEdit(HuEmployeeCvEditDTO dto)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            bool pathMode = true;
            var getOtherList = (from t in _uow.Context.Set<SYS_OTHER_LIST_TYPE>().Where(x => x.CODE == "STATUS")
                                from o in _uow.Context.Set<SYS_OTHER_LIST>().Where(x => x.CODE == "CD")
                                select new { Id = o.ID }).FirstOrDefault();
            if (dto.Id != null && dto.IsSaveBankInfo == true)
            {
                dto.FullName = dto.Fullname;
                var updateResponse = await _HuEmployeeCvEditRepository.Update(_uow, dto, sid, pathMode);
                return Ok(updateResponse);
            }
            dto.IsSaveBankInfo = true;
            //dto.FullName = dto.Fullname;
            dto.Id = null;
            var response = await _HuEmployeeCvEditRepository.Create(_uow, dto, sid);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetHuEmployeeCvEditBankInfoSave(long employeeId, long? time)
        {
            var response = await _PortalStaffProfileRepository.GetHuEmployeeCvEditBankInfoSave(employeeId);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetHuEmployeeCvEditBankInfoById(long id, long? time)
        {
            var response = await _PortalStaffProfileRepository.GetHuEmployeeCvEditBankInfoById(id);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetHuEmployeeCvEditBankInfoUnapprove(long employeeId, long? time)
        {
            var response = await _PortalStaffProfileRepository.GetHuEmployeeCvEditBankInfoUnapprove(employeeId);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetHuEmployeeCvEditBankInfoUnapproveById(long id, long? time)
        {
            var response = await _PortalStaffProfileRepository.GetHuEmployeeCvEditBankInfoUnapproveById(id);
            return Ok(response);
        }
        //CONTACT
        [HttpGet]
        public async Task<IActionResult> GetContactByEmployeeId(long employeeId)
        {
            var response = await _PortalStaffProfileRepository.GetContactByEmployeeId(employeeId);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> InsertContactHuEmployeeCvEdit(HuEmployeeCvEditDTO dto)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            bool pathMode = true;
            var getOtherList = (from t in _uow.Context.Set<SYS_OTHER_LIST_TYPE>().Where(x => x.CODE == "STATUS")
                                from o in _uow.Context.Set<SYS_OTHER_LIST>().Where(x => x.CODE == "CD")
                                select new { Id = o.ID }).FirstOrDefault();

            var cv = _dbContext.HuEmployeeCvs.Where(x => x.ID == dto.Id).FirstOrDefault();
            List<string> listModelChange = new List<string>();
            if (cv != null)
            {
                var entityType = typeof(HU_EMPLOYEE_CV);
                var dtoType = typeof(HuEmployeeCvDTO);
                var entityPropList = entityType.GetProperties().ToList();
                var dtoPropList = dtoType.GetProperties().ToList();

                var query = Activator.CreateInstance(dtoType);


                entityPropList.ForEach(prop =>
                {
                    var value = prop.GetValue(cv);
                    var dtoProp = dtoPropList.SingleOrDefault(x => x.Name == prop.Name.SnakeToCamelCase().CamelToPascalCase());
                    dtoProp?.SetValue(query, value);

                });


                if (query != null)
                {
                    Type type = query.GetType();
                    Type type2 = dto.GetType();
                    IList<PropertyInfo> props = new List<PropertyInfo>(type.GetProperties());
                    IList<PropertyInfo> prop2s = new List<PropertyInfo>(type2.GetProperties());
                    foreach (PropertyInfo prop in props)
                    {
                        foreach (PropertyInfo prop2 in prop2s)
                        {
                            if (prop.Name != "Id" && prop.Name != null && prop.Name == prop2.Name && prop.GetValue(query) != null && prop2.GetValue(dto) != null && prop.GetValue(query)!.ToString() != prop2.GetValue(dto)!.ToString())
                            {
                                listModelChange.Add(Char.ToLowerInvariant(prop.Name[0]) + prop.Name.Substring(1));
                            }

                        }
                    }
                    dto.ModelChange = string.Join(";", listModelChange);
                }
            }
            if (dto.Id != null && dto.IsSaveContact == true)
            {
                dto.IsSendPortalContact = true;
                dto.IsApprovedContact = false;
                dto.StatusApprovedContactId = getOtherList?.Id;
                dto.HuEmployeeCvId = dto.Id;
                dto.IsSaveContact = false;
                var updateResponse = await _HuEmployeeCvEditRepository.Update(_uow, dto, sid, pathMode);
                return Ok(updateResponse);
            }
            if (dto.Id != null && dto.IsSaveContact == false)
            {
                dto.IsSendPortalContact = true;
                dto.IsApprovedContact = false;
                dto.StatusApprovedContactId = getOtherList?.Id;
                dto.HuEmployeeCvId = dto.Id;
                var updateResponse = await _HuEmployeeCvEditRepository.Update(_uow, dto, sid, pathMode);
                return Ok(updateResponse);
            }
            dto.IsApprovedContact = false;
            dto.IsSendPortalContact = true;
            dto.StatusApprovedContactId = getOtherList?.Id;
            dto.HuEmployeeCvId = dto.Id;
            dto.IsSaveContact = false;
            dto.Id = null;
            var response = await _HuEmployeeCvEditRepository.Create(_uow, dto, sid);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetHuEmployeeCvEditContactApproving(long employeeId)
        {
            var response = await _PortalStaffProfileRepository.GetHuEmployeeCvEditContactApproving(employeeId);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetHuEmployeeCvEditContactCorrect(long id)
        {
            var response = await _PortalStaffProfileRepository.GetHuEmployeeCvEditContactCorrect(id);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> SaveContactHuEmployeeCvEdit(HuEmployeeCvEditDTO dto)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            bool pathMode = true;
            var getOtherList = (from t in _uow.Context.Set<SYS_OTHER_LIST_TYPE>().Where(x => x.CODE == "STATUS")
                                from o in _uow.Context.Set<SYS_OTHER_LIST>().Where(x => x.CODE == "CD")
                                select new { Id = o.ID }).FirstOrDefault();
            if (dto.Id != null && dto.IsSaveContact == true)
            {
                dto.FullName = dto.Fullname;
                var updateResponse = await _HuEmployeeCvEditRepository.Update(_uow, dto, sid, pathMode);
                return Ok(updateResponse);
            }
            dto.IsSaveContact = true;
            dto.FullName = dto.Fullname;
            dto.Id = null;
            var response = await _HuEmployeeCvEditRepository.Create(_uow, dto, sid);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetHuEmployeeCvEditContactSave(long employeeId)
        {
            var response = await _PortalStaffProfileRepository.GetHuEmployeeCvEditContactSave(employeeId);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetHuEmployeeCvEditContactSaveById(long id)
        {
            var response = await _PortalStaffProfileRepository.GetHuEmployeeCvEditContactSaveById(id);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetHuEmployeeCvEditContactUnapprove(long employeeId)
        {
            var response = await _PortalStaffProfileRepository.GetHuEmployeeCvEditContactUnapprove(employeeId);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetHuEmployeeCvEditUnapproveById(long id)
        {
            var response = await _PortalStaffProfileRepository.GetHuEmployeeCvEditContactUnapproveById(id);
            return Ok(response);
        }
        //ADDITIONAL_INFO
        [HttpGet]
        public async Task<IActionResult> GetHuEmployeeCvAdditionalInfoByEmployeeId(long employeeId, long? time)
        {
            var response = await _PortalStaffProfileRepository.GetHuEmployeeCvAdditionalInfoByEmployeeId(employeeId, time);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> InsertAdditionalInfoHuEmployeeCvEdit(HuEmployeeCvEditDTO dto)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            bool pathMode = true;
            var getOtherList = (from t in _uow.Context.Set<SYS_OTHER_LIST_TYPE>().Where(x => x.CODE == "STATUS")
                                from o in _uow.Context.Set<SYS_OTHER_LIST>().Where(x => x.CODE == "CD")
                                select new { Id = o.ID }).FirstOrDefault();

            var cv = _dbContext.HuEmployeeCvs.Where(x => x.ID == dto.Id).FirstOrDefault();
            if (cv != null)
            {
                List<string> listModelChange = new List<string>();
                var entityType = typeof(HU_EMPLOYEE_CV);
                var dtoType = typeof(HuEmployeeCvDTO);
                var entityPropList = entityType.GetProperties().ToList();
                var dtoPropList = dtoType.GetProperties().ToList();

                var query = Activator.CreateInstance(dtoType);


                entityPropList.ForEach(prop =>
                {
                    var value = prop.GetValue(cv);
                    var dtoProp = dtoPropList.SingleOrDefault(x => x.Name == prop.Name.SnakeToCamelCase().CamelToPascalCase());
                    dtoProp?.SetValue(query, value);

                });


                if (query != null)
                {
                    Type type = query.GetType();
                    Type type2 = dto.GetType();
                    IList<PropertyInfo> props = new List<PropertyInfo>(type.GetProperties());
                    IList<PropertyInfo> prop2s = new List<PropertyInfo>(type2.GetProperties());
                    foreach (PropertyInfo prop in props)
                    {
                        foreach (PropertyInfo prop2 in prop2s)
                        {

                            if (prop.Name != "Id" && prop.Name != null && prop.Name == prop2.Name && prop.GetValue(query) != null && prop2.GetValue(dto) != null && prop.GetValue(query)!.ToString() != prop2.GetValue(dto)!.ToString())
                            {
                                listModelChange.Add(Char.ToLowerInvariant(prop.Name[0]) + prop.Name.Substring(1));
                            }

                        }
                    }
                    dto.ModelChange = string.Join(";", listModelChange);
                }
            }
            if (dto.Id != null && dto.IsSaveAdditionalInfo == true)
            {
                dto.IsSendPortalAdditionalInfo = true;
                dto.IsApprovedAdditionalInfo = false;
                dto.StatusAddinationalInfoId = getOtherList?.Id;
                dto.HuEmployeeCvId = dto.Id;
                dto.IsSaveAdditionalInfo = false;
                var updateResponse = await _HuEmployeeCvEditRepository.Update(_uow, dto, sid, pathMode);
                return Ok(updateResponse);
            }
            if (dto.Id != null && dto.IsSaveAdditionalInfo == false)
            {
                dto.IsSendPortalAdditionalInfo = true;
                dto.IsApprovedAdditionalInfo = false;
                dto.StatusAddinationalInfoId = getOtherList?.Id;
                dto.HuEmployeeCvId = dto.Id;
                var updateResponse = await _HuEmployeeCvEditRepository.Update(_uow, dto, sid, pathMode);
                return Ok(updateResponse);
            }
            dto.IsSendPortalAdditionalInfo = true;
            dto.IsApprovedAdditionalInfo = false;
            dto.StatusAddinationalInfoId = getOtherList?.Id;
            dto.IsSaveAdditionalInfo = false;
            dto.HuEmployeeCvId = dto.Id;
            dto.Id = null;
            var response = await _HuEmployeeCvEditRepository.Create(_uow, dto, sid);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetHuEmployeeCvAdditionalInfoApproving(long employeeId, long? time)
        {
            var response = await _PortalStaffProfileRepository.GetHuEmployeeCvAdditionalInfoApproving(employeeId, time);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> SaveAdditionalInfoHuEmployeeCvEdit(HuEmployeeCvEditDTO dto)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            bool pathMode = true;
            var getOtherList = (from t in _uow.Context.Set<SYS_OTHER_LIST_TYPE>().Where(x => x.CODE == "STATUS")
                                from o in _uow.Context.Set<SYS_OTHER_LIST>().Where(x => x.CODE == "CD")
                                select new { Id = o.ID }).FirstOrDefault();
            if (dto.Id != null && dto.IsSaveAdditionalInfo == true)
            {
                var updateResponse = await _HuEmployeeCvEditRepository.Update(_uow, dto, sid, pathMode);
                return Ok(updateResponse);
            }

            if(dto.Id != null && dto.IsSaveAdditionalInfo == false)
            {
                dto.IsSaveAdditionalInfo = true;
                var updateResponse = await _HuEmployeeCvEditRepository.Update(_uow, dto, sid, pathMode);
            }
            dto.IsSaveAdditionalInfo = true;
            dto.Id = null;
            var response = await _HuEmployeeCvEditRepository.Create(_uow, dto, sid);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetHuEmployeeCvEditAdditionalInfoSave(long employeeId, long? time)
        {
            var response = await _PortalStaffProfileRepository.GetHuEmployeeCvEditAdditionalInfoSave(employeeId, time);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetHuEmployeeCvEditAddtionalInfoById(long id)
        {
            var response = await _PortalStaffProfileRepository.GetHuEmployeeCvEditAddtionalInfoById(id);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetHuEmployeeCvAdditionalInfoUnapprove(long employeeId, long? time)
        {
            var response = await _PortalStaffProfileRepository.GetHuEmployeeCvAdditionalInfoUnapprove(employeeId, time);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetHuEmployeeCvAdditionalInfoUnapproveById(long id)
        {
            var response = await _PortalStaffProfileRepository.GetHuEmployeeCvAdditionalInfoUnapproveById(id);
            return Ok(response);
        }

        //INSUARENCE_INFO
        [HttpGet]
        public async Task<IActionResult> GetInsuarenceInfoByEmployeeId(long employeeId)
        {
            var response = await _PortalStaffProfileRepository.GetInsuarenceInfoByEmployeeId(employeeId);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> InsertInsuarenceInfoHuEmployeeCvEdit(HuEmployeeCvEditDTO dto)
        {
            var sid = Request.Sid(_appSettings);
            bool pathMode = true;
            if (sid == null) return Unauthorized();
            var getOtherList = (from t in _uow.Context.Set<SYS_OTHER_LIST_TYPE>().Where(x => x.CODE == "STATUS")
                                from o in _uow.Context.Set<SYS_OTHER_LIST>().Where(x => x.CODE == "CD")
                                select new { Id = o.ID }).FirstOrDefault();

            var cv = _dbContext.HuEmployeeCvs.Where(x => x.ID == dto.Id).FirstOrDefault();
            List<string> listModelChange = new List<string>();
            if (cv != null)
            {
                var entityType = typeof(HU_EMPLOYEE_CV);
                var dtoType = typeof(HuEmployeeCvDTO);
                var entityPropList = entityType.GetProperties().ToList();
                var dtoPropList = dtoType.GetProperties().ToList();

                var query = Activator.CreateInstance(dtoType);


                entityPropList.ForEach(prop =>
                {
                    var value = prop.GetValue(cv);
                    var dtoProp = dtoPropList.SingleOrDefault(x => x.Name == prop.Name.SnakeToCamelCase().CamelToPascalCase());
                    dtoProp?.SetValue(query, value);

                });


                if (query != null)
                {
                    Type type = query.GetType();
                    Type type2 = dto.GetType();
                    IList<PropertyInfo> props = new List<PropertyInfo>(type.GetProperties());
                    IList<PropertyInfo> prop2s = new List<PropertyInfo>(type2.GetProperties());
                    foreach (PropertyInfo prop in props)
                    {
                        foreach (PropertyInfo prop2 in prop2s)
                        {

                            if (prop.Name != "Id" && prop.Name != null && prop.Name == prop2.Name && prop.GetValue(query) != null && prop2.GetValue(dto) != null && prop.GetValue(query)!.ToString() != prop2.GetValue(dto)!.ToString())
                            {
                                listModelChange.Add(Char.ToLowerInvariant(prop.Name[0]) + prop.Name.Substring(1));
                            }

                        }
                    }
                    dto.ModelChange = string.Join(";", listModelChange);
                }
            }
            if (dto.Id != null && dto.IsSaveInsurence == true)
            {
                dto.IsSendPortal = true;
                dto.IsApprovedInsuarenceInfo = false;
                dto.StatusApprovedInsuarenceId = getOtherList.Id;
                dto.HuEmployeeCvId = dto.Id;
                dto.IsSaveInsurence = false;
                var updateResponse = await _HuEmployeeCvEditRepository.Update(_uow, dto, sid, true);
                return Ok(updateResponse);
            }

            dto.IsSendPortal = true;
            dto.IsApprovedInsuarenceInfo = false;
            dto.StatusApprovedInsuarenceId = getOtherList.Id;
            dto.HuEmployeeCvId = dto.Id;
            dto.IsSaveInsurence = false;
            dto.Id = null;
            var response = await _HuEmployeeCvEditRepository.Create(_uow, dto, sid);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetHuEmployeeCvEditInsuarenceApproving(long employeeId)
        {
            var response = await _PortalStaffProfileRepository.GetHuEmployeeCvEditInsuarenceApproving(employeeId);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> SaveHuEmpoyeeCvEditInsurence(HuEmployeeCvEditDTO dto)
        {
            var sid = Request.Sid(_appSettings);
            bool pathMode = true;
            if (sid == null) return Unauthorized();
            var getOtherList = (from t in _uow.Context.Set<SYS_OTHER_LIST_TYPE>().Where(x => x.CODE == "STATUS")
                                from o in _uow.Context.Set<SYS_OTHER_LIST>().Where(x => x.CODE == "CD")
                                select new { Id = o.ID }).FirstOrDefault();
            if (dto.IsSaveInsurence == true)
            {
                var updateResponse = await _HuEmployeeCvEditRepository.Update(_uow, dto, sid, pathMode);
                return Ok(updateResponse);
            }
            else
            {
                dto.Id = null;
                dto.IsSaveInsurence = true;
                var response = await _HuEmployeeCvEditRepository.Create(_uow, dto, sid);
                return Ok(response);
            }



        }

        [HttpGet]
        public async Task<IActionResult> GetHuEmployeeCvEditInsurenceSave(long employeeId)
        {
            var response = await _PortalStaffProfileRepository.GetHuEmployeeCvEditInsurenceSave(employeeId);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetInsurenceSaveById(long id)
        {
            var response = await _PortalStaffProfileRepository.GetInsurenceSaveById(id);
            return Ok(response);
        }
    }
}
