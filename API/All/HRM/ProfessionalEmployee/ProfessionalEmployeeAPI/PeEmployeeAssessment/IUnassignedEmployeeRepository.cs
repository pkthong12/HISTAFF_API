using API.DTO;
using CORE.DTO;
using CORE.GenericUOW;

namespace API.All.HRM.ProfessionalEmployee.ProfessionalEmployeeAPI.PeEmployeeAssessment
{
    public interface IUnassignedEmployeeRepository : IGenericRepository<HU_EMPLOYEE, EmployeeModel>
    {
        Task<GenericPhaseTwoListResponse<EmployeeModel>> SinglePhaseQueryList(GenericQueryListDTO<EmployeeModel> request);
    }
}