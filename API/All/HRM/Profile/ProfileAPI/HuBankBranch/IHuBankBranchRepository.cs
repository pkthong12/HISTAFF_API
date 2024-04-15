using API.DTO;
using CORE.DTO;
using CORE.GenericUOW;

namespace API.Controllers.HuBankBranch
{
    public interface IHuBankBranchRepository: IGenericRepository<HU_BANK_BRANCH, HuBankBranchDTO>
    {
       Task<GenericPhaseTwoListResponse<HuBankBranchDTO>> SinglePhaseQueryList(GenericQueryListDTO<HuBankBranchDTO> request);

        Task<FormatedResponse> CreateNewCode();
        Task<FormatedResponse> GetBrankByBankId(long? id);
    }
}

