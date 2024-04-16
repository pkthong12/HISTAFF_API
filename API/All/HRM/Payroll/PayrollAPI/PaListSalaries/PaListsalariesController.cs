using API.All.DbContexts;
using API.DTO;
using API.Main;
using CORE.DTO;
using CORE.Enum;
using CORE.GenericUOW;
using CORE.StaticConstant;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using OfficeOpenXml.FormulaParsing.Excel.Functions.RefAndLookup;
using Org.BouncyCastle.Bcpg.OpenPgp;

namespace API.Controllers.PaListsalaries
{
    [ApiExplorerSettings(GroupName = "131-PAYROLL-PA_LISTSALARIES")]
    [ApiController]
    [HiStaffAuthorize]
    [Route("api/[controller]/[action]")]
    public class PaListsalariesController : ControllerBase
    {
        private readonly GenericUnitOfWork _uow;
        private readonly IPaListsalariesRepository _PaListsalariesRepository;
        private readonly AppSettings _appSettings;

        public PaListsalariesController(
            FullDbContext dbContext,
            IOptions<AppSettings> options)
        {
            _uow = new GenericUnitOfWork(dbContext);
            _PaListsalariesRepository = new PaListsalariesRepository(dbContext, _uow);
            _appSettings = options.Value;
        }

        [HttpGet]
        public async Task<IActionResult> ReadAll()
        {
            var response = await _PaListsalariesRepository.ReadAll();
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> ReadAllByNumberKey(ReadAllByKeyLongRequest request)
        {
            var response = await _PaListsalariesRepository.ReadAllByKey(request.Key, request.Value);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> ReadAllByStringKey(ReadAllByKeyStringRequest request)
        {
            var response = await _PaListsalariesRepository.ReadAllByKey(request.Key, request.Value);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> QueryList(GenericQueryListDTO<PaListsalariesDTO> request)
        {
            var response = await _PaListsalariesRepository.SinglePhaseQueryList(request);
            return Ok(new FormatedResponse() { InnerBody = response });
        }

        [HttpGet]
        public async Task<IActionResult> GetById(long id)
        {
            var response = await _PaListsalariesRepository.GetById(id);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetListObj(string typeCode)
        {
            var response = await _PaListsalariesRepository.GetListObj(typeCode);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetByStringId(string id)
        {
            var response = await _PaListsalariesRepository.GetById(id);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Create(PaListsalariesDTO model)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            model.IsActive = true;
            var response = await _PaListsalariesRepository.Create(_uow, model, sid);
            return Ok(response);
        }
        
        [HttpPost]
        public async Task<IActionResult> CreateRange(List<PaListsalariesDTO> models)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _PaListsalariesRepository.CreateRange(_uow, models, sid);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Update(PaListsalariesDTO model)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _PaListsalariesRepository.Update(_uow, model, sid);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateRange(List<PaListsalariesDTO> models)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _PaListsalariesRepository.UpdateRange(_uow, models, sid);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(PaListsalariesDTO model)
        {
            if (model.Id != null)
            {
                var response = await _PaListsalariesRepository.Delete(_uow, (long)model.Id);
                return Ok(response);
            }
            else
            {
                return Ok(new FormatedResponse() { ErrorType = EnumErrorType.CATCHABLE, MessageCode = CommonMessageCode.DELETE_REQUEST_NULL_ID });
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteIds(IdsRequest model)
        {
            var response = await _PaListsalariesRepository.DeleteIds(_uow, model.Ids);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteStringIds(StringIdsRequest model)
        {
            var response = await _PaListsalariesRepository.DeleteIds(_uow, model.Ids);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetObjSal()
        {
            var response = await _PaListsalariesRepository.GetObjSal();
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetGroupType()
        {
            var response = await _PaListsalariesRepository.GetGroupType();
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetDataType()
        {
            var response = await _PaListsalariesRepository.GetDataType();
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetListSal(long idSymbol)
        {
            var response = await _PaListsalariesRepository.GetListSal(idSymbol);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> GetListSalaries(long idObjectSal)
        {
            var response = await _PaListsalariesRepository.GetListSalaries(idObjectSal);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> GetListNameCode(PaListsalariesDTO param)
        {
            var response = await _PaListsalariesRepository.GetListNameCode(param);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetListObjSalry()
        {
            var response = await _PaListsalariesRepository.GetListObjSalry();
            return Ok(response);
        }
    }
}

