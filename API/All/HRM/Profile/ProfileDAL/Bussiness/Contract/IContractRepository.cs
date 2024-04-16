using ProfileDAL.ViewModels;
using Common.Interfaces;
using Common.Paging;
using Common.Extensions;
using API.Entities;
using CORE.DTO;
using API.DTO;
using API.All.SYSTEM.CoreAPI.Xlsx;

namespace ProfileDAL.Repositories
{
    public interface IContractRepository : IRepository<HU_CONTRACT>
    {
        Task<PagedResult<ContractDTO>> GetAll(ContractDTO param);
        Task<ResultWithError> GetById(long id);
        Task<FormatedResponse> CreateAsync(ContractInputDTO param);
        Task<FormatedResponse> UpdateAsync(ContractInputDTO param);
        Task<FormatedResponse> RemoveAsync(List<long> param);
        Task<ResultWithError> OpenStatus(long id);
        Task<ResultWithError> TemplateImport(int orgId);
        Task<ResultWithError> ImportTemplate(ImportCTractParam param);
        Task<ResultWithError> PortalGetAll();
        Task<ResultWithError> PortalGetBy(long id);

        Task<GenericPhaseTwoListResponse<ContractDTO>> SinglePhaseQueryList(GenericQueryListDTO<ContractDTO> request);
        Task<GenericPhaseTwoListResponse<HuContractImportDTO>> SinglePhaseQueryListImport(GenericQueryListDTO<HuContractImportDTO> request);
        Task<FormatedResponse> GetByEmployeeId(long EmployeeId);
        Task<FormatedResponse> GetContractByEmpProfile(long EmployeeId);
        Task<FormatedResponse> GetLastContract(long? empId, string? date);
        Task<FormatedResponse> GetWageByStartdateContract(long? empId, string? date);
        Task<FormatedResponse> GetContractType();

        Task<FormatedResponse> ChangeStatusApprove(ContractInputDTO param);
        Task<FormatedResponse> Save(ImportQueryListBaseDTO param);
        Task<bool> IsReceive(ContractInputDTO param);
        Task<FormatedResponse> GetContractByEmpProfilePortal(long EmployeeId);
        Task<FormatedResponse> LiquidationContract(ContractInputDTO param);

        void ScanUpdateStatusEmpDetail();
        Task<bool> ApproveList(string sid);
        Task<bool> UpdateStatusEmpDetailOfContract(HU_CONTRACT obj);
    }
}
