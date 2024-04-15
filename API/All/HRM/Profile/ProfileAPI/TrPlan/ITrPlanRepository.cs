using API.DTO;
using CORE.DTO;
using CORE.GenericUOW;

namespace API.Controllers.TrPlan
{
    public interface ITrPlanRepository: IGenericRepository<TR_PLAN, TrPlanDTO>
    {
       Task<GenericPhaseTwoListResponse<TrPlanDTO>> SinglePhaseQueryList(GenericQueryListDTO<TrPlanDTO> request);
       Task<FormatedResponse> GetAllCoures();
        Task<FormatedResponse> GetAllCenter();
        Task<FormatedResponse> GetAllOrg();

    }
}

