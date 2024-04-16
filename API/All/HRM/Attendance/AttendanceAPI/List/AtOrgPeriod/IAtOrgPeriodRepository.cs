using API.DTO;
using CORE.DTO;
using CORE.GenericUOW;

namespace API.Controllers.AtOrgPeriod
{
    public interface IAtOrgPeriodRepository: IGenericRepository<AT_ORG_PERIOD, AtOrgPeriodDTO>
    {
       Task<GenericPhaseTwoListResponse<AtOrgPeriodDTO>> SinglePhaseQueryList(GenericQueryListDTO<AtOrgPeriodDTO> request);
        Task<FormatedResponse> ReadAllOrgByPeriodId(AtOrgPeriodDTO periodId);
    }
}

