using API.DTO;
using CORE.DTO;
using CORE.GenericUOW;

namespace API.All.HRM.ProfessionalEmployee.ProfessionalEmployeeAPI.PeEmployeeAssessment
{
    public interface IPeEmployeeAssessmentRepository : IGenericRepository<PE_EMPLOYEE_ASSESSMENT, PeEmployeeAssessmentDTO>
    {
        Task<GenericPhaseTwoListResponse<PeEmployeeAssessmentDTO>> SinglePhaseQueryList(GenericQueryListDTO<PeEmployeeAssessmentDTO> request);
        Task<FormatedResponse> AddEmployee(RequestModel requestModel, GenericUnitOfWork _uow, string sid);
        Task<FormatedResponse> DeleteEmployee(List<long> ids, GenericUnitOfWork _uow);
        Task<FormatedResponse> GetDropDownListEvaluationPeriod();
    }
}