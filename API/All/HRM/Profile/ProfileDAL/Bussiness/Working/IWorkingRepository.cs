using ProfileDAL.ViewModels;
using Common.Interfaces;
using Common.Paging;
using Common.Extensions;
using API.Entities;
using CORE.DTO;
using API.DTO;

namespace ProfileDAL.Repositories
{
    public interface IWorkingRepository : IRepository<HU_WORKING>
    {
        //DECISION
        //----------------------------------
        Task<GenericPhaseTwoListResponse<HuWorkingDTO>> TwoPhaseQueryList(GenericQueryListDTO<HuWorkingDTO> request);
        Task<PagedResult<WorkingDTO>> GetAll(WorkingDTO param);
        Task<PagedResult<WorkingDTO>> GetWorking(WorkingDTO param);
        Task<ResultWithError> GetById(Int64 id);
        Task<ResultWithError> CreateAsync(WorkingInputDTO param);
        Task<ResultWithError> UpdateAsync(WorkingInputDTO param);
        Task<ResultWithError> RemoveAsync(List<long> param);
        Task<ResultWithError> OpenStatus(Int64 id);
        Task<ResultWithError> TemplateImport(int orgId);
        Task<ResultWithError> ImportTemplate(ImportParam param);
        Task<ResultWithError> GetLastWorking(long? empId, DateTime? date);
        Task<ResultWithError> GetWorkingOld(long? empId, long? id);
        Task<ResultWithError> PortalGetAll();
        Task<ResultWithError> PortalGetBy(long id);
        Task<bool> ApproveListWorking(string sid);
        Task<bool> ApproveWorking(HU_WORKING obj);
        void ScanApproveWorkings();
        //WAGE
        //------------------------------------
        Task<GenericPhaseTwoListResponse<HuWorkingDTO>> TwoPhaseQueryListWage(GenericQueryListDTO<HuWorkingDTO> request);
        Task<PagedResult<WorkingDTO>> GetAllWage(WorkingDTO param);
        Task<PagedResult<WorkingDTO>> GetWage(WorkingDTO param);
        Task<ResultWithError> GetWageById(long id);
        Task<ResultWithError> GetLastWage(long? empId, DateTime? date);
        Task<ResultWithError> CreateWageAsync(WorkingInputDTO param, string sid);
        Task<ResultWithError> UpdateWageAsync(WorkingInputDTO param, string sid);
        Task<ResultWithError> RemoveWageAsync(List<long> param);
        Task<ResultWithError> OpenStatusWage(long id);
        Task<ResultWithError> TemplateImportWage(int orgId);
        Task<ResultWithError> ImportTemplateWage(ImportParam param);
        Task<ResultWithError> PortalWageGetAll();
        Task<ResultWithError> PortalWageGetBy(long id);
        Task<ResultWithError> CalculateExpireShortTemp(long? empId, string? date, long? levelId);
        Task<ResultWithError> UpdateUpsal(WorkingInputDTO request);
        Task<ResultWithError> checkDecisionMaster(WorkingInputDTO request);
        Task<FormatedResponse> GetSalaryMinimumOfRegion(ContextModel request);
        Task<bool> CheckIsResign(HU_WORKING obj);
    }
}
