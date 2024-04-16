using API.DTO;
using CORE.DTO;
using CORE.GenericUOW;

namespace API.All.HRM.ProfessionalEmployee.ProfessionalEmployeeAPI.PeCapacityFramework
{
    public interface IPeCapacityFrameworkRepository : IGenericRepository<PE_CAPACITY_FRAMEWORK, PeCapacityFrameworkDTO>
    {
        Task<GenericPhaseTwoListResponse<PeCapacityFrameworkDTO>> SinglePhaseQueryList(GenericQueryListDTO<PeCapacityFrameworkDTO> request);
    }
}