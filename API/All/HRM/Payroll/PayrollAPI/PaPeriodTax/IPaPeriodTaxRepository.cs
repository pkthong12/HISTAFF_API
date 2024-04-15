using API.DTO;
using CORE.DTO;
using CORE.GenericUOW;

namespace API.Controllers.PaPeriodTax
{
    public interface IPaPeriodTaxRepository : IGenericRepository<PA_PERIOD_TAX, PaPeriodTaxDTO>
    {
        Task<GenericPhaseTwoListResponse<PaPeriodTaxDTO>> SinglePhaseQueryList(GenericQueryListDTO<PaPeriodTaxDTO> request);
        Task<FormatedResponse> GetPeriod();
        public Task<FormatedResponse> GetMonth(long year);
    }
}

