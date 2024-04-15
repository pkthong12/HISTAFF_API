using API.DTO;
using CORE.DTO;
using CORE.GenericUOW;

namespace API.Controllers.PaListFundSource
{
    public interface IPaListFundSourceRepository : IGenericRepository<PA_LIST_FUND_SOURCE, PaListFundSourceDTO>
    {
        Task<GenericPhaseTwoListResponse<PaListFundSourceDTO>> SinglePhaseQueryList(GenericQueryListDTO<PaListFundSourceDTO> request);
        Task<FormatedResponse> GetCompany();
        Task<FormatedResponse> GetCompanyById(long id);
        Task<FormatedResponse> CreateNewCode();
    }
}

