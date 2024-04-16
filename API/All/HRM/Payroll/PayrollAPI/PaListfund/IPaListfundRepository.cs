using API.DTO;
using CORE.DTO;
using CORE.GenericUOW;

namespace API.Controllers.PaListfund
{
    public interface IPaListfundRepository: IGenericRepository<PA_LISTFUND, PaListfundDTO>
    {
       Task<GenericPhaseTwoListResponse<PaListfundDTO>> SinglePhaseQueryList(GenericQueryListDTO<PaListfundDTO> request);

        Task<FormatedResponse> GetCompanyTypes();
        Task<FormatedResponse> CreateCodeNew();
        Task<FormatedResponse> GetListFundByPeriodId(long periodId);
        Task<FormatedResponse> GetListFund();
    }
}

