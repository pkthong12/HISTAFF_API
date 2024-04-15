using API.DTO;
using CORE.DTO;
using CORE.GenericUOW;

namespace API.Controllers.PaAuthorityTaxYear
{
    public interface IPaAuthorityTaxYearRepository : IGenericRepository<PA_AUTHORITY_TAX_YEAR, PaAuthorityTaxYearDTO>
    {
        Task<GenericPhaseTwoListResponse<PaAuthorityTaxYearDTO>> SinglePhaseQueryList(GenericQueryListDTO<PaAuthorityTaxYearDTO> request);
    }
}
