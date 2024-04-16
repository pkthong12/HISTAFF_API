using API.DTO;
using CORE.DTO;
using CORE.GenericUOW;

namespace API.All.HRM.Competency.CompetencyAPI.HuCompetencyPeroid
{
    public interface IHuCompetencyPeriodRepository : IGenericRepository<HU_COMPETENCY_PERIOD, HuCompetencyPeriodDTO>
    {
        Task<GenericPhaseTwoListResponse<HuCompetencyPeriodDTO>> SinglePhaseQueryList(GenericQueryListDTO<HuCompetencyPeriodDTO> request);
        Task<FormatedResponse> GetQuarter();
        Task<FormatedResponse> GetPeriodYear();
    }
}

