using API.DTO;
using CORE.DTO;
using CORE.GenericUOW;

namespace API.Controllers.PaTaxAnnualImport
{
    public interface IPaTaxAnnualImportRepository : IGenericRepository<PA_TAX_ANNUAL_IMPORT, PaTaxAnnualImportDTO>
    {
        Task<GenericPhaseTwoListResponse<PaTaxAnnualImportDTO>> SinglePhaseQueryList(GenericQueryListDTO<PaTaxAnnualImportDTO> request);
        Task<FormatedResponse> GetListSalaries(long id);
        Task<FormatedResponse> GetObjSalTax();
    }
}

