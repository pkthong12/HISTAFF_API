using API.DTO;
using CORE.DTO;
using CORE.GenericUOW;

namespace API.All.HRM.Profile.ProfileAPI.Portal.PortalEvaluate
{
    public interface IPortalEvaluateRepository: IGenericRepository<HU_EVALUATE, HuEvaluateDTO>
    {
       Task<GenericPhaseTwoListResponse<HuEvaluateDTO>> SinglePhaseQueryList(GenericQueryListDTO<HuEvaluateDTO> request);

       Task<FormatedResponse> GetByEmployeeId(long employeeId);
    }
}

