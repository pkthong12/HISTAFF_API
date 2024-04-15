using ProfileDAL.ViewModels;
using Common.Interfaces;
using Common.Paging;
using Common.Extensions;
using API.Entities;

namespace ProfileDAL.Repositories
{
    public interface IVoteRepository : IRepository<HU_ANSWER>
    {
        Task<ResultWithError> AddAnswer(QuestionOutputDTO param);
        Task<ResultWithError> GetQuestion(int? id);
        Task<ResultWithError> CreateQuestion(QuestionDTO param);
        Task<PagedResult<QuestionPagingDTO>> GetAll(QuestionPagingDTO param);
        Task<ResultWithError> GetById(long id);
        Task<ResultWithError> ChangeStatusAsync(List<long> ids);
        Task<ResultWithError> PortalList();
    }
}
