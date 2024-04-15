using API.DTO;
using CORE.DTO;
using CORE.GenericUOW;

namespace API.Controllers.HuBank
{
    public interface IHuBankRepository: IGenericRepository<HU_BANK, HuBankDTO>
    {
       Task<GenericPhaseTwoListResponse<HuBankDTO>> SinglePhaseQueryList(GenericQueryListDTO<HuBankDTO> request);

        Task<FormatedResponse> CreateNewCode();

        Task<FormatedResponse> GetList();
    }
}

