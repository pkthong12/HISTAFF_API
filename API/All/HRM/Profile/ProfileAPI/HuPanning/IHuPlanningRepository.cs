using API.DTO;
using CORE.DTO;
using CORE.GenericUOW;

namespace API.Controllers.HuPlanning
{
    public interface IHuPlanningRepository: IGenericRepository<HU_PLANNING, HuPlanningDTO>
    {
       Task<GenericPhaseTwoListResponse<HuPlanningDTO>> SinglePhaseQueryList(GenericQueryListDTO<HuPlanningDTO> request);
        Task<FormatedResponse> GetCertificateByEmp(long emp);
    }
}

