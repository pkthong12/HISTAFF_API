using API.DTO;
using CORE.DTO;
using CORE.GenericUOW;

namespace API.Controllers.InsTotalsalary
{
    public interface IInsTotalsalaryRepository: IGenericRepository<INS_TOTALSALARY, InsTotalsalaryDTO>
    {
       Task<GenericPhaseTwoListResponse<InsTotalsalaryDTO>> SinglePhaseQueryList(GenericQueryListDTO<InsTotalsalaryDTO> request);
        Task<FormatedResponse> Calculate(InsTotalsalaryDTO model, string sid);
        Task<FormatedResponse> GetInforByPeriod(InsTotalsalaryDTO dto);
        Task<FormatedResponse> GetInforEndPeriod(InsTotalsalaryDTO dto);
    }
}

