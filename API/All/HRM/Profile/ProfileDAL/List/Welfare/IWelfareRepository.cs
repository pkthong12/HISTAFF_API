using ProfileDAL.ViewModels;
using Common.Interfaces;
using Common.Paging;
using Common.Extensions;
using API.Entities;
using CORE.DTO;
using API.DTO;

namespace ProfileDAL.Repositories
{
    public interface IWelfareRepository : IRepository<HU_WELFARE>
    {
        Task<GenericPhaseTwoListResponse<WelfareDTO>> TwoPhaseQueryList(GenericQueryListDTO<WelfareDTO> request);
        Task<PagedResult<WelfareDTO>> GetAll(WelfareDTO param);
        Task<ResultWithError> GetById(long id);
        Task<ResultWithError> CreateAsync(WelfareInputDTO param);
        Task<ResultWithError> UpdateAsync(WelfareInputDTO param);
        Task<ResultWithError> ChangeStatusAsync(WelfareInputDTO request);

        Task<ResultWithError> GetList();
        Task<ResultWithError> GetListAuto();
        Task<FormatedResponse> GetListInPeriod(HuWelfareDTO param);
    }
}
