using API.All.SYSTEM.CoreAPI.Xlsx;
using API.DTO;
using CORE.DTO;
using CORE.GenericUOW;
using ProfileDAL.ViewModels;

namespace API.Controllers.HuFilecontractImport
{
    public interface IHuFilecontractImportRepository : IGenericRepository<HU_FILECONTRACT_IMPORT, HuFilecontractImportDTO>
    {
        Task<GenericPhaseTwoListResponse<HuFilecontractImportDTO>> SinglePhaseQueryList(GenericQueryListDTO<HuFilecontractImportDTO> request);
        Task<FormatedResponse> Save(ImportQueryListBaseDTO param);
    }
}

