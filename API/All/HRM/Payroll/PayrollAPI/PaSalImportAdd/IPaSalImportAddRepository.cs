using API.DTO;
using CORE.DTO;
using CORE.GenericUOW;

namespace API.Controllers.PaSalImportAdd
{
    public interface IPaSalImportAddRepository : IGenericRepository<PA_SAL_IMPORT_ADD, PaSalImportAddDTO>
    {
        Task<GenericPhaseTwoListResponse<PaSalImportAddDTO>> SinglePhaseQueryList(GenericQueryListDTO<PaSalImportAddDTO> request);

        Task<FormatedResponse> GetListSalaries(long id);

        Task<FormatedResponse> GetObjSalAdd();
    }
}

