using API.DTO;
using CORE.DTO;
using CORE.GenericUOW;

namespace API.Controllers.AtPeriodStandard
{
    public interface IAtPeriodStandardRepository: IGenericRepository<AT_PERIOD_STANDARD, AtPeriodStandardDTO>
    {
       Task<GenericPhaseTwoListResponse<AtPeriodStandardDTO>> SinglePhaseQueryList(GenericQueryListDTO<AtPeriodStandardDTO> request);


        Task<FormatedResponse> GetPeriod();

        Task<FormatedResponse> GetObject();
    }
}

