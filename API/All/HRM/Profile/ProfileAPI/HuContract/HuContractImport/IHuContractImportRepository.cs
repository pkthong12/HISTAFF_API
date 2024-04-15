using API.DTO;
using CORE.DTO;
using CORE.GenericUOW;
using API.DTO;
using API.All.SYSTEM.CoreAPI.Xlsx;

namespace API.Controllers.HuContractImport
{
    public interface IHuContractImportRepository: IGenericRepository<HU_CONTRACT_IMPORT, HuContractImportDTO>
    {
       Task<GenericPhaseTwoListResponse<HuContractImportDTO>> SinglePhaseQueryList(GenericQueryListDTO<HuContractImportDTO> request);

        Task<FormatedResponse> Save(ImportQueryListBaseDTO param);

    }
}

