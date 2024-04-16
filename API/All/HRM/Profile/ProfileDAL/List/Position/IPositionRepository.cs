using ProfileDAL.ViewModels;
using Common.Interfaces;
using Common.Paging;
using Common.Extensions;
using API.Entities;
using CORE.DTO;

namespace ProfileDAL.Repositories
{
    public interface IPositionRepository : IRepository<HU_POSITION>
    {
        Task<PagedResult<PositionViewDTO>> GetAll(PositionViewDTO param);
        Task<GenericPhaseTwoListResponse<PositionViewNoPagingDTO>> SinglePhaseQueryList(GenericQueryListDTO<PositionViewNoPagingDTO> request);
        Task<ResultWithError> GetById(long id);
        Task<ResultWithError> CreateAsync(PositionInputDTO param);
        Task<ResultWithError> UpdateAsync(PositionInputDTO param);
        Task<ResultWithError> ChangeStatusAsync(PositionInputDTO request);
        Task<ResultWithError> GetList(int groupId);
        Task<ResultWithError> GetListJob();
        Task<ResultWithError> Delete(List<long> ids);
        Task<ResultWithError> GetByOrg(int orgId, int empId);
        Task<ResultWithError> GetLM(int positionId);
        string AutoGenCodeHuTile(string tableName, string colName);
        Task<ResultWithError> ModifyPositionById(PositionInputDTO obj, int OrgRight, int Address, int OrgIDDefault = 1, int IsDissolveDefault = 0);
        Task<ResultWithError> InsertPositionNB(PositionInputDTO obj, int OrgRight, int Address, int OrgIDDefault = 1, int IsDissolveDefault = 0);
        Task<ResultWithError> GetOrgTreeApp(string sLang);
        Task<PagedResult<PositionViewDTO>> GetPositionOrgID(PositionViewDTO _filter);
        Task<ResultWithError> SwapMasterInterim(PositionInputDTO param);
        Task<ResultWithError> CheckTdvAsync(PositionInputDTO param);
        Task<FormatedResponse> TransferPosition(List<long> listTransfer, long orgId, string userId);
        Task<FormatedResponse> CloningPosition(List<long> listCloning, long orgId, int aMount, string userId);
        Task<FormatedResponse> PositionTransferSave(string userId);
        Task<FormatedResponse> PositionTransferRevert(string userId);
        void PositionTransferDelete(string userId);
    }
}
