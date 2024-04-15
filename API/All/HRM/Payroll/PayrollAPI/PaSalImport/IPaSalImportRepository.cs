using API.DTO;
using CORE.DTO;
using CORE.GenericUOW;

namespace API.Controllers.PaSalImport
{
    public interface IPaSalImportRepository : IGenericRepository<PA_SAL_IMPORT, PaSalImportDTO>
    {
        Task<GenericPhaseTwoListResponse<PaSalImportDTO>> SinglePhaseQueryList(GenericQueryListDTO<PaSalImportDTO> request);
        Task<FormatedResponse> GetListSalaries(long id);
    }
}

