using Microsoft.AspNetCore.Mvc;
using ProfileDAL.Repositories;
using ProfileDAL.ViewModels;
using Common.Paging;

namespace ProfileAPI.List
{
    [HiStaffAuthorize]
    [ApiExplorerSettings(GroupName = "002-PROFILE-PORTAL-BLOG-INTERNAL")]
    [ApiController]

    [Route("api/[controller]/[action]")]
    public class HuBlogInternalController : BaseController1
    {
        public HuBlogInternalController(IProfileUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        [HttpGet]
        public async Task<ActionResult> GetAll(BlogInternalDTO param)
        {
            var r = await _unitOfWork.BlogInternalRepository.GetAll(param);
            return Ok(r);
        }
        [HttpGet]
        public async Task<ActionResult> Get(int Id)
        {
            try
            {
                var r = await _unitOfWork.BlogInternalRepository.GetById(Id);
                return ResponseResult(r);
            }
            catch (Exception ex)
            {
                throw ex;
            }
           
        }
        [HttpPost]
        public async Task<ActionResult> Add([FromBody] BlogInternalInputDTO param)
        {
            if (!ModelState.IsValid)
            {
                return ResponseValidation();
            }

            var r = await _unitOfWork.BlogInternalRepository.CreateAsync(param);
            return ResponseResult(r);
        }

        [HttpPost]
        public async Task<ActionResult> Update([FromBody] BlogInternalInputDTO param)
        {

            if (param.Id == null)
            {
                return ResponseResult("ID_NOT_BLANK");
            }

            if (!ModelState.IsValid)
            {
                return ResponseValidation();
            }

            var r = await _unitOfWork.BlogInternalRepository.UpdateAsync(param);
            return ResponseResult(r);
        }
        [HttpPost]
        public async Task<ActionResult> ChangeStatus([FromBody] List<long> ids)
        {
            var r = await _unitOfWork.BlogInternalRepository.ChangeStatusAsync(ids);
            return ResponseResult(r);
        }

        [HttpGet]
        public async Task<ActionResult> PortalGetList(Pagings param)
        {
            try
            {
                var r = await _unitOfWork.BlogInternalRepository.PortalGetList(param);
                return ResponseResult(r);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        public async Task<ActionResult> PortalGetById(long id)
        {
            try
            {
                var r = await _unitOfWork.BlogInternalRepository.PortalGetById(id);
                return ResponseResult(r);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        public async Task<ActionResult> PortalGetNewest()
        {
            try
            {
                var r = await _unitOfWork.BlogInternalRepository.PortalGetNewest();
                return ResponseResult(r);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        public async Task<ActionResult> PortalNotify()
        {
            try
            {
                var r = await _unitOfWork.BlogInternalRepository.PortalNotify();
                return ResponseResult(r);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
        /// <summary>
        /// Portal Get slider for Home 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> PortalHomeNew()
        {
            try
            {
                var r = await _unitOfWork.BlogInternalRepository.PortalHomeNew();
                return ResponseResult(r);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
