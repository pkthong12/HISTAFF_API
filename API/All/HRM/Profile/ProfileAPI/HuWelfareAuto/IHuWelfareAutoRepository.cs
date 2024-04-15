using API.DTO;
using CORE.DTO;
using CORE.GenericUOW;
using Common.Interfaces;
using Common.Extensions;

namespace API.Controllers.HuWelfareAuto
{
    public interface IHuWelfareAutoRepository: IGenericRepository<HU_WELFARE_AUTO, HuWelfareAutoDTO>
    {
        Task<GenericPhaseTwoListResponse<HuWelfareAutoDTO>> SinglePhaseQueryList(GenericQueryListDTO<HuWelfareAutoDTO> request);
        Task<ResultWithError> Calculate(long? orgId, long? welfareId, long? periodId, string? calculateDate);
        Task<FormatedResponse> GetAllPeriodYear();
    }
}

