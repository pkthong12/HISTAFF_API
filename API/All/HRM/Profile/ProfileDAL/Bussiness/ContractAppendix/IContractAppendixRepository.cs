using ProfileDAL.ViewModels;
using Common.Interfaces;
using Common.Paging;
using Common.Extensions;
using API.Entities;
using CORE.DTO;

namespace ProfileDAL.Repositories
{
    public interface IContractAppendixRepository : IRepository<HU_FILECONTRACT>
    {
        Task<PagedResult<ContractAppendixDTO>> GetAll(ContractAppendixDTO param);
        Task<ResultWithError> GetById(long id);
        Task<FormatedResponse> CreateAsync(ContractAppendixInputDTO param);
        Task<FormatedResponse> UpdateAsync(ContractAppendixInputDTO param);
        Task<FormatedResponse> RemoveAsync(List<long> param);
        Task<ResultWithError> OpenStatus(long id);

        Task<GenericPhaseTwoListResponse<ContractAppendixDTO>> SinglePhaseQueryList(GenericQueryListDTO<ContractAppendixDTO> request);
        Task<GenericPhaseTwoListResponse<ContractAppendixDTO>> TwoPhaseQueryList(GenericQueryListDTO<ContractAppendixDTO> request);
        Task<FormatedResponse> GetContractAppendixByEmpProfile(long EmployeeId);
        Task<ResultWithError> GetAllowanceWageById(long? id);

        Task<ResultWithError> GetWageByContract(long? contractId);
        Task<ResultWithError> GetExpiteDateByContract(long? contractId);
    }
}
