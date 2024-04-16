using API.DTO;
using CORE.DTO;
using CORE.GenericUOW;

namespace API.Controllers.InsHealthInsurance
{
    public interface IInsHealthInsuranceRepository: IGenericRepository<INS_HEALTH_INSURANCE, InsHealthInsuranceDTO>
    {
       Task<GenericPhaseTwoListResponse<InsHealthInsuranceDTO>> SinglePhaseQueryList(GenericQueryListDTO<InsHealthInsuranceDTO> request);
    }
}

