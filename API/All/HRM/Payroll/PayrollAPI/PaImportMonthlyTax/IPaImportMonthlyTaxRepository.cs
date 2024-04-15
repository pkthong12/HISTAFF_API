using API.DTO;
using CORE.DTO;
using CORE.GenericUOW;

namespace API.Controllers.PaImportMonthlyTax
{
    public interface IPaImportMonthlyTaxRepository : IGenericRepository<PA_IMPORT_MONTHLY_TAX, PaImportMonthlyTaxDTO>
    {
        Task<GenericPhaseTwoListResponse<PaImportMonthlyTaxDTO>> SinglePhaseQueryList(GenericQueryListDTO<PaImportMonthlyTaxDTO> request);

        Task<FormatedResponse> GetListSalaries(long id);

        Task<FormatedResponse> GetTaxDate(long periodId);

        Task<FormatedResponse> GetObjSalTax();
    }
}

