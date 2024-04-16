using API.DTO;
using CORE.DTO;
using CORE.GenericUOW;

namespace API.All.HRM.Training.TrainingAPI.TrRequestYear
{
    public interface ITrRequestYearRepository : IGenericRepository<TR_REQUEST_YEAR, TrRequestYearDTO>
    {
        Task<GenericPhaseTwoListResponse<TrRequestYearDTO>> SinglePhaseQueryList(GenericQueryListDTO<TrRequestYearDTO> request);
        Task<FormatedResponse> GetDropDownListTrainingCourse();
        Task<FormatedResponse> GetDropDownListCompany();
    }
}