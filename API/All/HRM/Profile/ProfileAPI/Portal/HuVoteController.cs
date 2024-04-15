using Microsoft.AspNetCore.Mvc;
using ProfileDAL.Repositories;
using ProfileDAL.ViewModels;

namespace ProfileAPI.List
{
    [HiStaffAuthorize]
    [ApiExplorerSettings(GroupName = "002-PROFILE-PORTAL-VOTE")]
    [ApiController]

    [Route("api/[controller]/[action]")]
    public class HuVoteController : BaseController1
    {
        public HuVoteController(IProfileUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }
        [HttpPost]
        public async Task<ActionResult> CreateQuestion([FromBody] QuestionDTO param)
        {
            if (!ModelState.IsValid)
            {
                return ResponseValidation();
            }
            var r = await _unitOfWork.VoteRepository.CreateQuestion(param);
            return Ok(r);
        }

        [HttpGet]
        public async Task<ActionResult> GetQuestion(int? id)
        {
            var r = await _unitOfWork.VoteRepository.GetQuestion(id);
            return ResponseResult(r);
        }

        [HttpPost]
        public async Task<ActionResult> AddAnswer([FromBody] QuestionOutputDTO param)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return ResponseValidation();
                }
                var r = await _unitOfWork.VoteRepository.AddAnswer(param);
                return ResponseResult(r);
            }
            catch (Exception ex)
            {
                return new RedirectResult(ex.Message);
            }
        }

        [HttpGet]
        public async Task<ActionResult> GetAll(QuestionPagingDTO param)
        {
            var r = await _unitOfWork.VoteRepository.GetAll(param);
            return Ok(r);
        }
        [HttpGet]
        public async Task<ActionResult> Get(int Id)
        {
            try
            {
                var r = await _unitOfWork.VoteRepository.GetById(Id);
                return ResponseResult(r);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        public async Task<ActionResult> ChangeStatus([FromBody] List<long> ids)
        {
            var r = await _unitOfWork.VoteRepository.ChangeStatusAsync(ids);
            return ResponseResult(r);
        }

        [HttpGet]
        public async Task<ActionResult> PortalList()
        {
            var r = await _unitOfWork.VoteRepository.PortalList();
            return Ok(r);
        }
    }
}
