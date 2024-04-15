using ProfileDAL.ViewModels;
using Common.Interfaces;
using Common.Paging;
using Common.Extensions;
using API.Entities;
using CORE.DTO;

namespace ProfileDAL.Repositories
{
    public interface IGroupPositionRepository : IRepository<HU_POSITION_GROUP>
    {
        Task<GenericPhaseTwoListResponse<GroupPositionViewDTO>> TwoPhaseQueryList(GenericQueryListDTO<GroupPositionViewDTO> request);
        Task<PagedResult<GroupPositionDTO>> GetAll(GroupPositionDTO param);
        Task<ResultWithError> GetById(int id);
        Task<ResultWithError> CreateAsync(GroupPositionInputDTO param);
        Task<ResultWithError> UpdateAsync(GroupPositionInputDTO param);
        Task<ResultWithError> ChangeStatusAsync(List<int> ids);
        Task<ResultWithError> GetList();
        Task<ResultWithError> Delete(int id);
    }
}
