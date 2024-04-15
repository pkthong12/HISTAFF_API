using API.DTO;
using CORE.DTO;
using CORE.GenericUOW;

namespace API.Controllers.HuContractType
{
    public interface IHuContractTypeRepository: IGenericRepository<HU_CONTRACT_TYPE, HuContractTypeDTO>
    {
        Task<GenericPhaseTwoListResponse<HuContractTypeDTO>> SinglePhaseQueryList(GenericQueryListDTO<HuContractTypeDTO> request);

        Task<FormatedResponse> GetListContractTypeSys();

        Task<FormatedResponse> GetContractTypeSysById(long id);
        Task<FormatedResponse> GetContractAppendixType();

        Task<FormatedResponse> CheckCodeExists(string code);
        Task<FormatedResponse> GetContractDashboard();

    }
}

